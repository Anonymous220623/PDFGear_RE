// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.BiffWriter
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser;

[CLSCompliant(false)]
internal class BiffWriter : IDisposable
{
  private const int DEF_BUFFER_SIZE = 1048576 /*0x100000*/;
  private Stream m_stream;
  private bool m_bDisposed;
  private bool m_bDestroyStream;
  private BinaryWriter m_writer;
  private byte[] m_arrBuffer = new byte[8228];
  private ByteArrayDataProvider m_provider;

  public Stream BaseStream => this.m_stream;

  public byte[] Buffer => this.m_arrBuffer;

  private BiffWriter() => this.m_provider = new ByteArrayDataProvider(this.m_arrBuffer);

  public BiffWriter(Stream stream)
    : this(stream, false)
  {
  }

  public BiffWriter(Stream stream, bool bControlsStream)
  {
    this.m_provider = new ByteArrayDataProvider(this.m_arrBuffer);
    this.m_bDestroyStream = bControlsStream;
    this.m_stream = stream;
    this.m_writer = new BinaryWriter(this.m_stream);
  }

  public void Dispose()
  {
    if (this.m_bDisposed)
      return;
    this.m_bDisposed = true;
    this.m_writer.Flush();
    if (this.m_bDestroyStream)
    {
      this.m_stream.SetLength(this.m_stream.Position);
      this.m_stream.Dispose();
    }
    this.m_stream = (Stream) null;
    this.m_provider = (ByteArrayDataProvider) null;
  }
}
