// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.BiffReader
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser;

[CLSCompliant(false)]
internal class BiffReader : IDisposable
{
  private const int DEF_BUFFER_SIZE = 262144 /*0x040000*/;
  private const int BIFF8_VERSION = 1536 /*0x0600*/;
  private Stream m_stream;
  private BinaryReader m_reader;
  private bool m_bDisposed;
  private bool m_bDestroyStream;
  private int m_iMinimalVersion = 1536 /*0x0600*/;
  private byte[] m_arrBuffer = new byte[8228];
  private DataProvider m_dataProvider;

  public Stream BaseStream => this.m_stream;

  public BinaryReader BaseReader => this.m_reader;

  public int MinimalVersion
  {
    get => this.m_iMinimalVersion;
    set => this.m_iMinimalVersion = value;
  }

  public byte[] Buffer => this.m_arrBuffer;

  public DataProvider DataProvider => this.m_dataProvider;

  private BiffReader()
  {
    this.m_dataProvider = (DataProvider) new ByteArrayDataProvider(this.m_arrBuffer);
  }

  public BiffReader(Stream stream)
    : this()
  {
    this.m_stream = stream != null ? stream : throw new ArgumentNullException(nameof (stream));
    this.m_reader = new BinaryReader(this.m_stream);
  }

  public BiffReader(Stream stream, bool bControlStream)
    : this(stream)
  {
    this.m_bDestroyStream = bControlStream;
  }

  public void Dispose()
  {
    if (this.m_bDisposed)
      return;
    this.m_bDisposed = true;
    if (this.m_bDestroyStream)
    {
      this.m_reader.Dispose();
      this.m_stream.Dispose();
    }
    this.m_stream = (Stream) null;
    this.m_reader = (BinaryReader) null;
    this.m_arrBuffer = (byte[]) null;
    if (this.m_dataProvider == null)
      return;
    this.m_dataProvider.Dispose();
    this.m_dataProvider = (DataProvider) null;
  }

  public bool IsEOF
  {
    get
    {
      if (this.m_stream == null)
        throw new ArgumentNullException("internal stream");
      try
      {
        if (this.m_stream.Position == this.m_stream.Length)
          return true;
        int num = (int) this.m_reader.ReadInt16();
        this.m_reader.BaseStream.Position -= 2L;
        return num == 0;
      }
      catch (Exception ex)
      {
        return true;
      }
    }
  }

  public BiffRecordRaw GetRecord()
  {
    if (this.m_stream == null)
      throw new ArgumentNullException("internal stream");
    return (BiffRecordRaw) null;
  }

  public BiffRecordRaw PeekRecord()
  {
    long num = this.m_stream != null ? this.m_stream.Position : throw new ArgumentNullException("internal stream");
    BiffRecordRaw record = this.GetRecord();
    this.m_stream.Position = num;
    return record;
  }

  public TBIFFRecord PeekRecordType()
  {
    long num = this.m_stream != null ? this.m_stream.Position : throw new ArgumentNullException("internal stream");
    TBIFFRecord recordType = (TBIFFRecord) BiffRecordFactory.ExtractRecordType(this.m_reader);
    this.m_stream.Position = num;
    return recordType;
  }

  protected BiffRecordRaw TestPeekRecord()
  {
    if (this.m_stream == null)
      throw new ArgumentNullException("internal stream");
    Stream baseStream = this.m_reader.BaseStream;
    long position = baseStream.Position;
    BiffRecordRaw biffRecordRaw = (BiffRecordRaw) null;
    if (this.m_reader.ReadInt16() > (short) 0)
      biffRecordRaw = UnknownRecord.Empty;
    baseStream.Position = position;
    return biffRecordRaw;
  }
}
