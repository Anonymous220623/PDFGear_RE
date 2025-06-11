// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.HeaderFooterImageRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.HeaderFooterImage)]
internal class HeaderFooterImageRecord : MSODrawingGroupRecord, ILengthSetter
{
  internal static readonly byte[] DEF_RECORD_START = new byte[14]
  {
    (byte) 102,
    (byte) 8,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 2,
    (byte) 0
  };
  internal static readonly byte[] DEF_WORKSHEET_RECORD_START = new byte[14]
  {
    (byte) 102,
    (byte) 8,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 1,
    (byte) 0
  };
  internal static readonly byte[] DEF_CONTINUE_START = new byte[14]
  {
    (byte) 102,
    (byte) 8,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 6,
    (byte) 0
  };
  internal static readonly int DEF_DATA_OFFSET = HeaderFooterImageRecord.DEF_RECORD_START.Length;

  public HeaderFooterImageRecord()
  {
  }

  public HeaderFooterImageRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public HeaderFooterImageRecord(int iReserve)
    : base(iReserve)
  {
  }

  protected override int StructuresOffset => HeaderFooterImageRecord.DEF_DATA_OFFSET;

  protected override Stream CreateDataList(out int iStartIndex)
  {
    int count = this.m_arrStructures.Count;
    iStartIndex = 1;
    MemoryStream dataList = new MemoryStream();
    dataList.Write(HeaderFooterImageRecord.DEF_RECORD_START, 0, HeaderFooterImageRecord.DEF_RECORD_START.Length);
    return (Stream) dataList;
  }

  protected override int AddRecordData(List<byte[]> arrRecords, BiffRecordRaw record)
  {
    if (arrRecords == null)
      throw new ArgumentNullException(nameof (arrRecords));
    byte[] src = record != null ? record.Data : throw new ArgumentNullException(nameof (record));
    int num = src.Length;
    int count = num - HeaderFooterImageRecord.DEF_DATA_OFFSET;
    if (count > 0)
    {
      byte[] dst = new byte[count];
      num = count;
      Buffer.BlockCopy((Array) src, HeaderFooterImageRecord.DEF_DATA_OFFSET, (Array) dst, 0, count);
      src = dst;
    }
    arrRecords.Add(src);
    return num;
  }

  protected override ContinueRecordBuilder CreateBuilder()
  {
    ContinueRecordBuilder builder = (ContinueRecordBuilder) new HeaderContinueRecordBuilder((BiffContinueRecordRaw) this);
    builder.OnFirstContinue += new EventHandler(((BiffContinueRecordRaw) this).builder_OnFirstContinue);
    return builder;
  }

  public void SetLength(int iLength) => this.m_iLength = iLength;
}
