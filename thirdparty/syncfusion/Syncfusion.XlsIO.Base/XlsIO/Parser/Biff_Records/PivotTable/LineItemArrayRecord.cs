// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.PivotTable.LineItemArrayRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;

[CLSCompliant(false)]
[Biff(TBIFFRecord.LineItemArray)]
public class LineItemArrayRecord : BiffRecordWithContinue
{
  private List<LineItem> m_arrItems;
  private bool m_bNeedDataArray = true;

  public List<LineItem> Items
  {
    get => this.m_arrItems;
    set
    {
      this.m_arrItems = value != null ? value : throw new ArgumentNullException(nameof (value));
    }
  }

  public override bool NeedDataArray => this.m_bNeedDataArray;

  protected override bool AddHeaderToProvider => true;

  public override void ParseStructure()
  {
  }

  public void ParseStructure(int iFieldsCount)
  {
  }

  public override void InfillInternalData(ExcelVersion version)
  {
    this.m_iFirstLength = this.m_iLength > 8224 ? 8224 : -1;
  }

  public override int GetStoreSize(ExcelVersion version) => this.m_iLength;

  public override object Clone()
  {
    LineItemArrayRecord lineItemArrayRecord = (LineItemArrayRecord) base.Clone();
    lineItemArrayRecord.m_arrItems = CloneUtils.CloneCloneable<LineItem>(this.m_arrItems);
    return (object) lineItemArrayRecord;
  }
}
