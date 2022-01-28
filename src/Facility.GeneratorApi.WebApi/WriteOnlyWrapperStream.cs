namespace Facility.GeneratorApi.WebApi;

// from http://blog.stephencleary.com/2016/11/streaming-zip-on-aspnet-core.html
public sealed class WriteOnlyWrapperStream : Stream
{
	public WriteOnlyWrapperStream(Stream stream) => m_stream = stream;

	public override bool CanRead => false;

	public override bool CanSeek => false;

	public override bool CanWrite => true;

	public override long Position
	{
		get => m_position;
		set => throw new NotSupportedException();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		m_position += count;
		m_stream.Write(buffer, offset, count);
	}

	public override void WriteByte(byte value)
	{
		m_position += 1;
		m_stream.WriteByte(value);
	}

	public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
	{
		m_position += count;
		return m_stream.WriteAsync(buffer, offset, count, cancellationToken);
	}

	public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
	{
		m_position += buffer.Length;
		return base.WriteAsync(buffer, cancellationToken);
	}

	public override bool CanTimeout => m_stream.CanTimeout;

	public override int ReadTimeout
	{
		get => m_stream.ReadTimeout;
		set => m_stream.ReadTimeout = value;
	}

	public override int WriteTimeout
	{
		get => m_stream.WriteTimeout;
		set => m_stream.WriteTimeout = value;
	}

	public override void Flush() => m_stream.Flush();

	public override Task FlushAsync(CancellationToken cancellationToken) => m_stream.FlushAsync(cancellationToken);

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing)
				m_stream.Dispose();
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken) =>
		m_stream.CopyToAsync(destination, bufferSize, cancellationToken);

	public override long Length => throw new NotSupportedException();

	public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

	public override void SetLength(long value) => throw new NotSupportedException();

	public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();

	private readonly Stream m_stream;
	private long m_position;
}
