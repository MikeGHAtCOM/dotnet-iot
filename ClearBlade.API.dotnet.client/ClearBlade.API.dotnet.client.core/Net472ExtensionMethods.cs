using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClearBlade.API.dotnet.client.core
{
    internal static class Net472ExtensionMethods
    {
#if NET472

        internal const int MaxBufferSize = int.MaxValue;

        private static void CheckDisposed(HttpContent value)
        {
            var fieldLookup = value.GetType().GetField("_disposed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (fieldLookup != null) {
                if ((bool)(fieldLookup.GetValue(value)))
                {
                    throw new ObjectDisposedException(value.GetType().ToString());
                }
            }
            else { return; }
        }

        private static bool IsBuffered
        {
            get { return _bufferedContent != null; }
        }

        private static MemoryStream? _bufferedContent;
        internal static readonly Encoding DefaultStringEncoding = Encoding.UTF8;

        private const int UTF8CodePage = 65001;
        private const int UTF8PreambleLength = 3;
        private const byte UTF8PreambleByte0 = 0xEF;
        private const byte UTF8PreambleByte1 = 0xBB;
        private const byte UTF8PreambleByte2 = 0xBF;
        private const int UTF8PreambleFirst2Bytes = 0xEFBB;

        private const int UTF32CodePage = 12000;
        private const int UTF32PreambleLength = 4;
        private const byte UTF32PreambleByte0 = 0xFF;
        private const byte UTF32PreambleByte1 = 0xFE;
        private const byte UTF32PreambleByte2 = 0x00;
        private const byte UTF32PreambleByte3 = 0x00;
        private const int UTF32OrUnicodePreambleFirst2Bytes = 0xFFFE;

        private const int UnicodeCodePage = 1200;
        private const int UnicodePreambleLength = 2;
        private const byte UnicodePreambleByte0 = 0xFF;
        private const byte UnicodePreambleByte1 = 0xFE;

        private const int BigEndianUnicodeCodePage = 1201;
        private const int BigEndianUnicodePreambleLength = 2;
        private const byte BigEndianUnicodePreambleByte0 = 0xFE;
        private const byte BigEndianUnicodePreambleByte1 = 0xFF;
        private const int BigEndianUnicodePreambleFirst2Bytes = 0xFEFF;

        private static bool BufferHasPrefix(ArraySegment<byte> buffer, byte[] prefix)
        {
            byte[]? byteArray = buffer.Array;
            if (prefix == null || byteArray == null || prefix.Length > buffer.Count || prefix.Length == 0)
                return false;

            for (int i = 0, j = buffer.Offset; i < prefix.Length; i++, j++)
            {
                if (prefix[i] != byteArray[j])
                    return false;
            }

            return true;
        }

        private static int GetPreambleLength(ArraySegment<byte> buffer, Encoding encoding)
        {
            byte[]? data = buffer.Array;
            int offset = buffer.Offset;
            int dataLength = buffer.Count;

            Debug.Assert(data != null);
            Debug.Assert(encoding != null);

            switch (encoding.CodePage)
            {
                case UTF8CodePage:
                    return (dataLength >= UTF8PreambleLength
                        && data[offset + 0] == UTF8PreambleByte0
                        && data[offset + 1] == UTF8PreambleByte1
                        && data[offset + 2] == UTF8PreambleByte2) ? UTF8PreambleLength : 0;
                case UTF32CodePage:
                    return (dataLength >= UTF32PreambleLength
                        && data[offset + 0] == UTF32PreambleByte0
                        && data[offset + 1] == UTF32PreambleByte1
                        && data[offset + 2] == UTF32PreambleByte2
                        && data[offset + 3] == UTF32PreambleByte3) ? UTF32PreambleLength : 0;
                case UnicodeCodePage:
                    return (dataLength >= UnicodePreambleLength
                        && data[offset + 0] == UnicodePreambleByte0
                        && data[offset + 1] == UnicodePreambleByte1) ? UnicodePreambleLength : 0;

                case BigEndianUnicodeCodePage:
                    return (dataLength >= BigEndianUnicodePreambleLength
                        && data[offset + 0] == BigEndianUnicodePreambleByte0
                        && data[offset + 1] == BigEndianUnicodePreambleByte1) ? BigEndianUnicodePreambleLength : 0;

                default:
                    byte[] preamble = encoding.GetPreamble();
                    return BufferHasPrefix(buffer, preamble) ? preamble.Length : 0;
            }
        }

        private static bool TryDetectEncoding(ArraySegment<byte> buffer, out Encoding? encoding, out int preambleLength)
        {
            byte[]? data = buffer.Array;
            int offset = buffer.Offset;
            int dataLength = buffer.Count;

            Debug.Assert(data != null);

            if (dataLength >= 2)
            {
                int first2Bytes = data[offset + 0] << 8 | data[offset + 1];

                switch (first2Bytes)
                {
                    case UTF8PreambleFirst2Bytes:
                        if (dataLength >= UTF8PreambleLength && data[offset + 2] == UTF8PreambleByte2)
                        {
                            encoding = Encoding.UTF8;
                            preambleLength = UTF8PreambleLength;
                            return true;
                        }
                        break;

                    case UTF32OrUnicodePreambleFirst2Bytes:
                        // UTF32 not supported on Phone
                        if (dataLength >= UTF32PreambleLength && data[offset + 2] == UTF32PreambleByte2 && data[offset + 3] == UTF32PreambleByte3)
                        {
                            encoding = Encoding.UTF32;
                            preambleLength = UTF32PreambleLength;
                        }
                        else
                        {
                            encoding = Encoding.Unicode;
                            preambleLength = UnicodePreambleLength;
                        }
                        return true;

                    case BigEndianUnicodePreambleFirst2Bytes:
                        encoding = Encoding.BigEndianUnicode;
                        preambleLength = BigEndianUnicodePreambleLength;
                        return true;
                }
            }

            encoding = null;
            preambleLength = 0;
            return false;
        }

        internal static string ReadBufferAsString(ArraySegment<byte> buffer, HttpContentHeaders headers)
        {
            // We don't validate the Content-Encoding header: If the content was encoded, it's the caller's
            // responsibility to make sure to only call ReadAsString() on already decoded content. E.g. if the
            // Content-Encoding is 'gzip' the user should set HttpClientHandler.AutomaticDecompression to get a
            // decoded response stream.

            Encoding? encoding = null;
            int bomLength = -1;

            string? charset = headers.ContentType?.CharSet;

            // If we do have encoding information in the 'Content-Type' header, use that information to convert
            // the content to a string.
            if (charset != null)
            {
                try
                {
                    // Remove at most a single set of quotes.
                    if (charset.Length > 2 &&
                        charset[0] == '\"' &&
                        charset[charset.Length - 1] == '\"')
                    {
                        encoding = Encoding.GetEncoding(charset.Substring(1, charset.Length - 2));
                    }
                    else
                    {
                        encoding = Encoding.GetEncoding(charset);
                    }

                    // Byte-order-mark (BOM) characters may be present even if a charset was specified.
                    bomLength = GetPreambleLength(buffer, encoding);
                }
                catch (ArgumentException e)
                {
                    throw new InvalidOperationException("Invalid Charset", e);
                }
            }

            // If no content encoding is listed in the ContentType HTTP header, or no Content-Type header present,
            // then check for a BOM in the data to figure out the encoding.
            if (encoding == null)
            {
                if (!TryDetectEncoding(buffer, out encoding, out bomLength))
                {
                    // Use the default encoding (UTF8) if we couldn't detect one.
                    encoding = DefaultStringEncoding;

                    // We already checked to see if the data had a UTF8 BOM in TryDetectEncoding
                    // and DefaultStringEncoding is UTF8, so the bomLength is 0.
                    bomLength = 0;
                }
            }

            // Drop the BOM when decoding the data.
            return encoding.GetString(buffer.Array!, buffer.Offset + bomLength, buffer.Count - bomLength);
        }

        private static string ReadBufferedContentAsString(this HttpContent value)
        {
            Debug.Assert(IsBuffered);

            if (_bufferedContent!.Length == 0)
            {
                return string.Empty;
            }

            ArraySegment<byte> buffer;
            if (!TryGetBuffer(out buffer))
            {
                buffer = new ArraySegment<byte>(_bufferedContent.ToArray());
            }

            return ReadBufferAsString(buffer, value.Headers);
        }

        internal static bool TryGetBuffer(out ArraySegment<byte> buffer)
        {
            if (_bufferedContent != null)
            {
                return _bufferedContent.TryGetBuffer(out buffer);
            }
            buffer = default;
            return false;
        }

        public static async Task<string> ReadAsStringAsync(this HttpContent value, CancellationToken cancellationToken)
        {
            CheckDisposed(value);
            return await WaitAndReturnAsync(LoadIntoBufferAsync(value, cancellationToken), value, static s => s.ReadBufferedContentAsString());
        }

        internal static Task LoadIntoBufferAsync(HttpContent value, CancellationToken cancellationToken) =>
            LoadIntoBufferAsync(value, MaxBufferSize, cancellationToken);

        internal static Task LoadIntoBufferAsync(HttpContent value, long maxBufferSize, CancellationToken cancellationToken)
        {
            CheckDisposed(value);

            if (!CreateTemporaryBuffer(value, maxBufferSize, out MemoryStream? tempBuffer, out Exception? error))
            {
                // If we already buffered the content, just return a completed task.
                return Task.CompletedTask;
            }

            if (tempBuffer == null)
            {
                // We don't throw in LoadIntoBufferAsync(): return a faulted task.
                return Task.FromException(error!);
            }

            try
            {
                Task task = null;
                var sts = value.GetType().GetMethod("SerializeToStreamAsync", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (sts != null)
                {
                    if (sts.GetParameters().Length == 3)
                    {
                        task = (Task)sts.Invoke(value, new object[] { tempBuffer, null, cancellationToken });
                        CheckTaskNotNull(task);
                    }
                    if (sts.GetParameters().Length == 2)
                    {
                        task = (Task)sts.Invoke(value, new object[] { tempBuffer, null });
                        CheckTaskNotNull(task);
                    }
                }
                return LoadIntoBufferAsyncCore(task, tempBuffer);
            }
            catch (Exception e) when (StreamCopyExceptionNeedsWrapping(e))
            {
                return Task.FromException(GetStreamCopyException(e));
            }
            // other synchronous exceptions from SerializeToStreamAsync/CheckTaskNotNull will propagate
        }

        private static async Task LoadIntoBufferAsyncCore(Task serializeToStreamTask, MemoryStream tempBuffer)
        {
            try
            {
                await serializeToStreamTask.ConfigureAwait(false);
            }
            catch (Exception e)
            {
                tempBuffer.Dispose(); // Cleanup partially filled stream.
                Exception we = GetStreamCopyException(e);
                if (we != e) throw we;
                throw;
            }

            try
            {
                tempBuffer.Seek(0, SeekOrigin.Begin); // Rewind after writing data.
                _bufferedContent = tempBuffer;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        internal static bool StreamCopyExceptionNeedsWrapping(Exception e) => e is IOException || e is ObjectDisposedException;

        private static Exception GetStreamCopyException(Exception originalException)
        {
            // HttpContent derived types should throw HttpRequestExceptions if there is an error. However, since the stream
            // provided by CopyToAsync() can also throw, we wrap such exceptions in HttpRequestException. This way custom content
            // types don't have to worry about it. The goal is that users of HttpContent don't have to catch multiple
            // exceptions (depending on the underlying transport), but just HttpRequestExceptions
            // Custom stream should throw either IOException or HttpRequestException.
            // We don't want to wrap other exceptions thrown by Stream (e.g. InvalidOperationException), since we
            // don't want to hide such "usage error" exceptions in HttpRequestException.
            // ObjectDisposedException is also wrapped, since aborting HWR after a request is complete will result in
            // the response stream being closed.
            return StreamCopyExceptionNeedsWrapping(originalException) ?
                WrapStreamCopyException(originalException) :
                originalException;
        }

        internal static Exception WrapStreamCopyException(Exception e)
        {
            Debug.Assert(StreamCopyExceptionNeedsWrapping(e));
            return new HttpRequestException("Http Content Stream copy error", e);
        }

        private static void CheckTaskNotNull(Task task)
        {
            if (task == null)
            {
                var e = new InvalidOperationException("No task in HTTP Content");
                throw e;
            }
        }
        private static bool CreateTemporaryBuffer(HttpContent value, long maxBufferSize, out MemoryStream? tempBuffer, out Exception? error)
        {
            if (maxBufferSize > MaxBufferSize)
            {
                // This should only be hit when called directly; HttpClient/HttpClientHandler
                // will not exceed this limit.
                throw new ArgumentOutOfRangeException(nameof(maxBufferSize), maxBufferSize, "Buffer is too big");
            }

            if (IsBuffered)
            {
                // If we already buffered the content, just return false.
                tempBuffer = default;
                error = default;
                return false;
            }

            tempBuffer = CreateMemoryStream(value, maxBufferSize, out error);
            return true;
        }
        private static MemoryStream? CreateMemoryStream(HttpContent value, long maxBufferSize, out Exception? error)
        {
            error = null;

            // If we have a Content-Length allocate the right amount of buffer up-front. Also check whether the
            // content length exceeds the max. buffer size.
            long? contentLength = value.Headers.ContentLength;

            if (contentLength != null)
            {
                Debug.Assert(contentLength >= 0);

                if (contentLength > maxBufferSize)
                {
                    error = new HttpRequestException("Buffer size exceeded");
                    return null;
                }

                // We can safely cast contentLength to (int) since we just checked that it is <= maxBufferSize.
                return new LimitMemoryStream((int)maxBufferSize, (int)contentLength);
            }

            // We couldn't determine the length of the buffer. Create a memory stream with an empty buffer.
            return new LimitMemoryStream((int)maxBufferSize, 0);
        }

        private static async Task<TResult> WaitAndReturnAsync<TState, TResult>(Task waitTask, TState state, Func<TState, TResult> returnFunc)
        {
            await waitTask.ConfigureAwait(false);
            return returnFunc(state);
        }

        internal sealed class LimitMemoryStream : MemoryStream
        {
            private readonly int _maxSize;

            public LimitMemoryStream(int maxSize, int capacity)
                : base(capacity)
            {
                Debug.Assert(capacity <= maxSize);
                _maxSize = maxSize;
            }

            public byte[] GetSizedBuffer()
            {
                ArraySegment<byte> buffer;
                return TryGetBuffer(out buffer) && buffer.Offset == 0 && buffer.Count == buffer.Array!.Length ?
                    buffer.Array :
                    ToArray();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                CheckSize(count);
                base.Write(buffer, offset, count);
            }

            public override void WriteByte(byte value)
            {
                CheckSize(1);
                base.WriteByte(value);
            }

            public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                CheckSize(count);
                return base.WriteAsync(buffer, offset, count, cancellationToken);
            }

/*            public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken)
            {
                CheckSize(buffer.Length);
                return base.WriteAsync(buffer, cancellationToken);
            }*/

            public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
            {
                CheckSize(count);
                return base.BeginWrite(buffer, offset, count, callback, state);
            }

            public override void EndWrite(IAsyncResult asyncResult)
            {
                base.EndWrite(asyncResult);
            }

            public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
            {
                ArraySegment<byte> buffer;
                if (TryGetBuffer(out buffer))
                {
                    ValidateCopyToArguments(destination, bufferSize);

                    long pos = Position;
                    long length = Length;
                    Position = length;

                    long bytesToWrite = length - pos;
                    return destination.WriteAsync(buffer.Array!, (int)(buffer.Offset + pos), (int)bytesToWrite, cancellationToken);
                }

                return base.CopyToAsync(destination, bufferSize, cancellationToken);
            }

            /// <summary>Validates arguments provided to the <see cref="CopyTo(Stream, int)"/> or <see cref="CopyToAsync(Stream, int, CancellationToken)"/> methods.</summary>
            /// <param name="destination">The <see cref="Stream"/> "destination" argument passed to the copy method.</param>
            /// <param name="bufferSize">The integer "bufferSize" argument passed to the copy method.</param>
            /// <exception cref="ArgumentNullException"><paramref name="destination"/> was null.</exception>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> was not a positive value.</exception>
            /// <exception cref="NotSupportedException"><paramref name="destination"/> does not support writing.</exception>
            /// <exception cref="ObjectDisposedException"><paramref name="destination"/> does not support writing or reading.</exception>
            protected static void ValidateCopyToArguments(Stream destination, int bufferSize)
            {
                if (destination is null)
                {
                    throw new ArgumentNullException(nameof(destination));
                }

                if (bufferSize <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(bufferSize), bufferSize, "Argument out of range");
                }

                if (!destination.CanWrite)
                {
                    if (destination.CanRead)
                    {
                        throw new NotSupportedException("Unwriteable stream");

                    }
                    
                    throw new ObjectDisposedException(destination.GetType().Name);
                }
            }

            private void CheckSize(int countToAdd)
            {
                if (_maxSize - Length < countToAdd)
                {
                    throw CreateOverCapacityException(_maxSize);
                }
            }

            private static Exception CreateOverCapacityException(int maxBufferSize)
            {
                return new HttpRequestException("Over capacity");
            }
        }

        internal sealed class LimitArrayPoolWriteStream : Stream
        {
            private const int InitialLength = 256;

            private readonly int _maxBufferSize;
            private byte[] _buffer;
            private int _length;

            public LimitArrayPoolWriteStream(int maxBufferSize) : this(maxBufferSize, InitialLength) { }

            public LimitArrayPoolWriteStream(int maxBufferSize, long capacity)
            {
                if (capacity < InitialLength)
                {
                    capacity = InitialLength;
                }
                else if (capacity > maxBufferSize)
                {
                    throw CreateOverCapacityException(maxBufferSize);
                }

                _maxBufferSize = maxBufferSize;
                _buffer = ArrayPool<byte>.Shared.Rent((int)capacity);
            }

            protected override void Dispose(bool disposing)
            {
                Debug.Assert(_buffer != null);

                ArrayPool<byte>.Shared.Return(_buffer);
                _buffer = null!;

                base.Dispose(disposing);
            }

            public ArraySegment<byte> GetBuffer() => new ArraySegment<byte>(_buffer, 0, _length);

            public byte[] ToArray()
            {
                var arr = new byte[_length];
                Buffer.BlockCopy(_buffer, 0, arr, 0, _length);
                return arr;
            }

            private void EnsureCapacity(int value)
            {
                if ((uint)value > (uint)_maxBufferSize) // value cast handles overflow to negative as well
                {
                    throw CreateOverCapacityException(_maxBufferSize);
                }
                else if (value > _buffer.Length)
                {
                    Grow(value);
                }
            }

            private void Grow(int value)
            {
                Debug.Assert(value > _buffer.Length);

                // Extract the current buffer to be replaced.
                byte[] currentBuffer = _buffer;
                _buffer = null!;

                // Determine the capacity to request for the new buffer.  It should be
                // at least twice as long as the current one, if not more if the requested
                // value is more than that.  If the new value would put it longer than the max
                // allowed byte array, than shrink to that (and if the required length is actually
                // longer than that, we'll let the runtime throw).
                uint twiceLength = 2 * (uint)currentBuffer.Length;
                int newCapacity = twiceLength > 0X7FFFFFC7 ?
                    Math.Max(value, 0X7FFFFFC7) :
                    Math.Max(value, (int)twiceLength);

                // this magic number is Array.MaxLength in .net 7+

                // Get a new buffer, copy the current one to it, return the current one, and
                // set the new buffer as current.
                byte[] newBuffer = ArrayPool<byte>.Shared.Rent(newCapacity);
                Buffer.BlockCopy(currentBuffer, 0, newBuffer, 0, _length);
                ArrayPool<byte>.Shared.Return(currentBuffer);
                _buffer = newBuffer;
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                Debug.Assert(buffer != null);
                Debug.Assert(offset >= 0);
                Debug.Assert(count >= 0);

                EnsureCapacity(_length + count);
                Buffer.BlockCopy(buffer, offset, _buffer, _length, count);
                _length += count;
            }

            public void Write(ReadOnlySpan<byte> buffer)
            {
                EnsureCapacity(_length + buffer.Length);
                buffer.CopyTo(new Span<byte>(_buffer, _length, buffer.Length));
                _length += buffer.Length;
            }

            public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            {
                Write(buffer, offset, count);
                return Task.CompletedTask;
            }

            public ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
            {
                Write(buffer.Span);
                return default;
            }

            public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? asyncCallback, object? asyncState) =>
                TaskToApm2.Begin(WriteAsync(buffer, offset, count, CancellationToken.None), asyncCallback, asyncState);

            public override void EndWrite(IAsyncResult asyncResult) =>
                TaskToApm2.End(asyncResult);

            public override void WriteByte(byte value)
            {
                int newLength = _length + 1;
                EnsureCapacity(newLength);
                _buffer[_length] = value;
                _length = newLength;
            }

            public override void Flush() { }
            public override Task FlushAsync(CancellationToken cancellationToken) => Task.CompletedTask;

            public override long Length => _length;
            public override bool CanWrite => true;
            public override bool CanRead => false;
            public override bool CanSeek => false;

            public override long Position { get { throw new NotSupportedException(); } set { throw new NotSupportedException(); } }
            public override int Read(byte[] buffer, int offset, int count) { throw new NotSupportedException(); }
            public override long Seek(long offset, SeekOrigin origin) { throw new NotSupportedException(); }
            public override void SetLength(long value) { throw new NotSupportedException(); }

            private static Exception CreateOverCapacityException(int maxBufferSize)
            {
                return new HttpRequestException("Over capacity");
            }
        }

#endif
    }
}
