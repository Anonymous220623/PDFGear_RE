// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.PreferredWidthInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class PreferredWidthInfo
{
  private int m_widthTypeKey;
  private FormatBase m_ownerFormat;

  internal float Width
  {
    get
    {
      return this.m_ownerFormat is RowFormat ? (float) (this.m_ownerFormat as RowFormat).GetPropertyValue(this.m_widthTypeKey + 1) : (float) (this.m_ownerFormat as CellFormat).GetPropertyValue(this.m_widthTypeKey + 1);
    }
    set
    {
      if (this.m_ownerFormat is RowFormat)
        (this.m_ownerFormat as RowFormat).SetPropertyValue(this.m_widthTypeKey + 1, (object) value);
      else
        (this.m_ownerFormat as CellFormat).SetPropertyValue(this.m_widthTypeKey + 1, (object) value);
    }
  }

  internal FtsWidth WidthType
  {
    get
    {
      return this.m_ownerFormat is RowFormat ? (FtsWidth) (this.m_ownerFormat as RowFormat).GetPropertyValue(this.m_widthTypeKey) : (FtsWidth) (this.m_ownerFormat as CellFormat).GetPropertyValue(this.m_widthTypeKey);
    }
    set
    {
      if (this.m_ownerFormat is RowFormat)
      {
        (this.m_ownerFormat as RowFormat).SetPropertyValue(this.m_widthTypeKey, (object) value);
      }
      else
      {
        (this.m_ownerFormat as CellFormat).SetPropertyValue(this.m_widthTypeKey, (object) value);
        if (value != FtsWidth.Percentage || !((this.m_ownerFormat as CellFormat).OwnerBase is WTableCell) || !((this.m_ownerFormat as CellFormat).OwnerBase as WTableCell).Document.IsOpening || ((this.m_ownerFormat as CellFormat).OwnerBase as WTableCell).OwnerRow == null || ((this.m_ownerFormat as CellFormat).OwnerBase as WTableCell).OwnerRow.OwnerTable == null || ((this.m_ownerFormat as CellFormat).OwnerBase as WTableCell).OwnerRow.OwnerTable.PreferredTableWidth.WidthType != FtsWidth.None)
          return;
        ((this.m_ownerFormat as CellFormat).OwnerBase as WTableCell).OwnerRow.OwnerTable.IsTableCellWidthDefined = true;
      }
    }
  }

  internal void Close()
  {
    if (this.m_ownerFormat == null)
      return;
    this.m_ownerFormat = (FormatBase) null;
  }

  internal PreferredWidthInfo(FormatBase ownerFormat, int key)
  {
    this.m_ownerFormat = ownerFormat;
    this.m_widthTypeKey = key;
  }
}
