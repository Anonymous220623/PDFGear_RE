// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.RangeProtectionRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.RangeProtection)]
internal class RangeProtectionRecord : BiffRecordRaw
{
  private const int DEF_LENGTH_OFFSET = 19;
  private const int DEF_DATA_OFFSET = 27;
  private const int DEF_SUBRECORD_SIZE = 8;
  private const int DEF_FINISH_OFFSET = 4;
  public const int DEF_MAX_SUBRECORDS_SIZE = 1024 /*0x0400*/;
  private readonly byte[] DEF_FIRST_UNKNOWN_BYTES = new byte[19]
  {
    (byte) 104,
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
    (byte) 3,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0
  };
  private readonly byte[] DEF_SECOND_UNKNOWN_BYTES = new byte[6]
  {
    (byte) 4,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0,
    (byte) 0
  };
  private ExcelIgnoreError m_ignoreOpt;
  internal MemoryStream m_preservedData;
  internal List<UnknownRecord> m_continueRecords;

  public ExcelIgnoreError IgnoreOptions
  {
    get => this.m_ignoreOpt;
    set => this.m_ignoreOpt = value;
  }

  public override int MinimumRecordSize => 39;

  public RangeProtectionRecord()
  {
  }

  public RangeProtectionRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public RangeProtectionRecord(int iReserve)
    : base(iReserve)
  {
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    OfficeVersion version)
  {
    int num = 27 + (int) provider.ReadUInt16(iOffset + 19) * 8;
    if (num > iLength)
    {
      this.m_preservedData = new MemoryStream(8224);
      byte[] numArray = new byte[iLength];
      provider.ReadArray(iOffset, numArray);
      this.m_preservedData.Write(numArray, 0, numArray.Length);
    }
    else
    {
      this.m_ignoreOpt = (ExcelIgnoreError) provider.ReadUInt16(iOffset + num);
      iOffset += 27;
      int ignoreOpt = (int) this.m_ignoreOpt;
    }
  }

  public override void InfillInternalData(
    DataProvider provider,
    int iOffset,
    OfficeVersion version)
  {
    if (this.m_preservedData != null)
    {
      this.m_preservedData.Position = 0L;
      provider.WriteBytes(iOffset, this.m_preservedData.ToArray());
    }
    else
    {
      this.m_iLength = this.GetStoreSize(version);
      provider.WriteBytes(iOffset, this.DEF_FIRST_UNKNOWN_BYTES, 0, this.DEF_FIRST_UNKNOWN_BYTES.Length);
      iOffset += 19;
    }
  }

  public override int GetStoreSize(OfficeVersion version)
  {
    if (this.m_preservedData != null)
      return (int) this.m_preservedData.Length;
    this.OptimizeStorage();
    return 39;
  }

  private void OptimizeStorage()
  {
  }

  private IList<Rectangle> CombineSameRowRectangles(IList<Rectangle> lstRects)
  {
    if (lstRects == null || lstRects.Count == 0)
      return lstRects;
    List<Rectangle> rectangleList = new List<Rectangle>();
    rectangleList.Add(lstRects[0]);
    int index1 = 1;
    for (int count = lstRects.Count; index1 < count; ++index1)
    {
      int index2 = rectangleList.Count - 1;
      Rectangle rectangle = rectangleList[index2];
      Rectangle lstRect = lstRects[index1];
      if (rectangle.Top == lstRect.Top && rectangle.Bottom == lstRect.Bottom && rectangle.Right + 1 == lstRect.Left)
      {
        rectangle = Rectangle.FromLTRB(rectangle.Left, rectangle.Top, lstRect.Right, rectangle.Bottom);
        rectangleList[index2] = rectangle;
      }
      else
        rectangleList.Add(lstRect);
    }
    return (IList<Rectangle>) rectangleList;
  }
}
