// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.PageItemRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.PageItem)]
public class PageItemRecord : BiffRecordRawWithArray
{
  private List<PageItemRecord.FieldInfo> m_arrFieldItems = new List<PageItemRecord.FieldInfo>();

  public PageItemRecord()
  {
  }

  public PageItemRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public PageItemRecord(int iReserve)
    : base(iReserve)
  {
  }

  public List<PageItemRecord.FieldInfo> Items => this.m_arrFieldItems;

  public override void ParseStructure()
  {
    int offset1 = 0;
    while (offset1 < this.m_iLength)
    {
      PageItemRecord.FieldInfo fieldInfo = new PageItemRecord.FieldInfo();
      fieldInfo.ViewItemIndex = this.GetUInt16(offset1);
      int offset2 = offset1 + 2;
      fieldInfo.ViewFieldIndex = this.GetUInt16(offset2);
      int offset3 = offset2 + 2;
      fieldInfo.ObjectId = this.GetUInt16(offset3);
      offset1 = offset3 + 2;
      this.m_arrFieldItems.Add(fieldInfo);
    }
  }

  public override void InfillInternalData(ExcelVersion version)
  {
    int count = this.m_arrFieldItems.Count;
    this.m_iLength = 0;
    this.m_data = new byte[count * 6];
    for (int index = 0; index < count; ++index)
    {
      PageItemRecord.FieldInfo arrFieldItem = this.m_arrFieldItems[index];
      this.SetUInt16(this.m_iLength, arrFieldItem.ViewItemIndex);
      this.m_iLength += 2;
      this.SetUInt16(this.m_iLength, arrFieldItem.ViewFieldIndex);
      this.m_iLength += 2;
      this.SetUInt16(this.m_iLength, arrFieldItem.ObjectId);
      this.m_iLength += 2;
    }
  }

  public override object Clone()
  {
    PageItemRecord pageItemRecord = (PageItemRecord) base.Clone();
    pageItemRecord.m_arrFieldItems = new List<PageItemRecord.FieldInfo>();
    int index = 0;
    for (int count = this.m_arrFieldItems.Count; index < count; ++index)
      pageItemRecord.m_arrFieldItems[index] = this.m_arrFieldItems[index].Clone();
    return (object) pageItemRecord;
  }

  public class FieldInfo
  {
    [BiffRecordPos(0, 2)]
    public ushort ViewItemIndex;
    [BiffRecordPos(2, 2)]
    public ushort ViewFieldIndex;
    [BiffRecordPos(4, 2)]
    public ushort ObjectId;

    internal PageItemRecord.FieldInfo Clone() => (PageItemRecord.FieldInfo) this.MemberwiseClone();
  }
}
