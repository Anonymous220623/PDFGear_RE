// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.RangeProtectionRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
[Biff(TBIFFRecord.RangeProtection)]
public class RangeProtectionRecord : BiffRecordRaw
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
  private ErrorIndicatorImpl m_errorIndicator;
  internal MemoryStream m_preservedData;
  internal List<UnknownRecord> m_continueRecords;

  public ExcelIgnoreError IgnoreOptions
  {
    get => this.m_ignoreOpt;
    set => this.m_ignoreOpt = value;
  }

  public ErrorIndicatorImpl ErrorIndicator
  {
    get => this.m_errorIndicator;
    set => this.m_errorIndicator = value;
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
    ExcelVersion version)
  {
    int num1 = (int) provider.ReadUInt16(iOffset + 19);
    int num2 = 27 + num1 * 8;
    if (num2 > iLength)
    {
      this.m_preservedData = new MemoryStream(8224);
      byte[] numArray = new byte[iLength];
      provider.ReadArray(iOffset, numArray);
      this.m_preservedData.Write(numArray, 0, numArray.Length);
    }
    else
    {
      this.m_ignoreOpt = (ExcelIgnoreError) provider.ReadUInt16(iOffset + num2);
      iOffset += 27;
      if (this.m_ignoreOpt == ExcelIgnoreError.None)
        return;
      this.m_errorIndicator = new ErrorIndicatorImpl(this.m_ignoreOpt);
      for (int index = 0; index < num1; ++index)
      {
        int y = (int) provider.ReadUInt16(iOffset);
        iOffset += 2;
        int num3 = (int) provider.ReadUInt16(iOffset);
        iOffset += 2;
        int x = (int) provider.ReadUInt16(iOffset);
        iOffset += 2;
        int num4 = (int) provider.ReadUInt16(iOffset);
        iOffset += 2;
        this.m_errorIndicator.AddRange(new Rectangle(x, y, num4 - x, num3 - y));
      }
    }
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
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
      List<Rectangle> cellList = this.m_errorIndicator.CellList;
      int count = cellList.Count;
      provider.WriteUInt16(iOffset, (ushort) count);
      iOffset += 2;
      int length = this.DEF_SECOND_UNKNOWN_BYTES.Length;
      provider.WriteBytes(iOffset, this.DEF_SECOND_UNKNOWN_BYTES, 0, length);
      iOffset += length;
      for (int index = 0; index < count; ++index)
      {
        Rectangle rectangle = cellList[index];
        provider.WriteUInt16(iOffset, (ushort) rectangle.Y);
        iOffset += 2;
        provider.WriteUInt16(iOffset, (ushort) rectangle.Bottom);
        iOffset += 2;
        provider.WriteUInt16(iOffset, (ushort) rectangle.X);
        iOffset += 2;
        provider.WriteUInt16(iOffset, (ushort) rectangle.Right);
        iOffset += 2;
      }
      provider.WriteInt32(iOffset, (int) this.m_ignoreOpt);
    }
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    if (this.m_preservedData != null)
      return (int) this.m_preservedData.Length;
    this.OptimizeStorage();
    return 27 + (this.m_errorIndicator == null ? 0 : this.m_errorIndicator.CellList.Count) * 8 + 4;
  }

  private void OptimizeStorage()
  {
    List<Rectangle> cellList = this.m_errorIndicator.CellList;
    if (cellList.Count <= 1)
      return;
    int count1 = cellList.Count;
    int num;
    do
    {
      num = count1;
      SortedDictionary<int, SortedList<int, Rectangle>> sortedDictionary = new SortedDictionary<int, SortedList<int, Rectangle>>();
      int index1 = 0;
      for (int count2 = cellList.Count; index1 < count2; ++index1)
      {
        Rectangle rectangle = cellList[index1];
        SortedList<int, Rectangle> sortedList;
        if (!sortedDictionary.TryGetValue(rectangle.Top, out sortedList))
        {
          sortedList = new SortedList<int, Rectangle>();
          sortedDictionary.Add(rectangle.Top, sortedList);
        }
        sortedList.Add(rectangle.Left, rectangle);
      }
      this.m_errorIndicator.Clear();
      foreach (int key in sortedDictionary.Keys)
      {
        IList<Rectangle> rectangleList = this.CombineSameRowRectangles(sortedDictionary[key].Values);
        int index2 = 0;
        for (int count3 = rectangleList.Count; index2 < count3; ++index2)
          this.m_errorIndicator.AddRange(rectangleList[index2]);
      }
      count1 = this.m_errorIndicator.CellList.Count;
    }
    while (num != count1);
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
