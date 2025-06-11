// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.BiffWriter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Security;
using Syncfusion.XlsIO.Parser.Biff_Records;
using System;
using System.Collections;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser;

[CLSCompliant(false)]
public class BiffWriter : IDisposable
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

  public void WriteRecord(BiffRecordRaw raw, IEncryptor encryptor)
  {
    if (raw == null)
      throw new ArgumentNullException(nameof (raw));
    raw.FillStream(this.m_writer, (DataProvider) this.m_provider, encryptor, (int) this.m_writer.BaseStream.Position);
    if (raw.NeedDataArray)
      return;
    raw.ClearData();
    raw.NeedInfill = true;
  }

  [CLSCompliant(false)]
  public void WriteRecord(OffsetArrayList records, IEncryptor encryptor)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    int position = (int) this.m_writer.BaseStream.Position;
    int index = 0;
    for (int count = records.Count; index < count; ++index)
    {
      IBiffStorage record = records[index];
      if (record != null)
      {
        position += record.FillStream(this.m_writer, (DataProvider) this.m_provider, encryptor, position);
        if (!record.NeedDataArray && record is BiffRecordRawWithArray)
        {
          BiffRecordRawWithArray recordRawWithArray = record as BiffRecordRawWithArray;
          recordRawWithArray.Data = new byte[0];
          recordRawWithArray.NeedInfill = true;
        }
      }
    }
  }

  public void WriteRecord(ICollection collection, IEncryptor encryptor)
  {
    if (collection == null)
      throw new ArgumentNullException(nameof (collection));
    int position = (int) this.m_writer.BaseStream.Position;
    foreach (BiffRecordRaw biffRecordRaw in (IEnumerable) collection)
    {
      position += biffRecordRaw.FillStream(this.m_writer, (DataProvider) this.m_provider, encryptor, position);
      if (!biffRecordRaw.NeedDataArray)
      {
        biffRecordRaw.Data = new byte[0];
        biffRecordRaw.NeedInfill = true;
      }
    }
  }
}
