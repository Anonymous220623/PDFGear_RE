// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotDataField
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.PivotTable;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

public class PivotDataField : IPivotDataField, ICloneParent
{
  private const int BaseItemPrevious = 1048828;
  private const int BaseItemNext = 1048829;
  private string m_strName;
  private PivotSubtotalTypes m_subtotal;
  private PivotFieldImpl m_field;
  private PivotFieldDataFormat m_showDataAs = PivotFieldDataFormat.Normal;
  private Dictionary<PivotFieldDataFormat, string> m_showDataCollections;
  private List<PivotFieldDataFormat> m_excel2010Data;
  private int m_baseItem;
  private int m_baseField;
  private PivotViewFieldsExRecord m_viewFieldsEx = (PivotViewFieldsExRecord) BiffRecordFactory.GetRecord(TBIFFRecord.PivotViewFieldsEx);

  public string Name
  {
    get => this.m_strName;
    set
    {
      this.m_strName = value != null && value.Length != 0 ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }
  }

  public PivotSubtotalTypes Subtotal
  {
    get => this.m_subtotal;
    set => this.m_subtotal = value;
  }

  public PivotFieldImpl Field => this.m_field;

  internal int NumberFormatIndex
  {
    get => (int) this.m_viewFieldsEx.NumberFormat;
    set
    {
      this.m_viewFieldsEx.NumberFormat = value >= 0 ? (ushort) value : throw new ArgumentOutOfRangeException(nameof (value));
    }
  }

  public string NumberFormat
  {
    get => this.Field.m_table.Workbook.InnerFormats[this.NumberFormatIndex]?.FormatString;
    set
    {
      this.NumberFormatIndex = this.Field.m_table.Workbook.InnerFormats.FindOrCreateFormat(value);
    }
  }

  public PivotFieldDataFormat ShowDataAs
  {
    get => this.m_showDataAs;
    set => this.m_showDataAs = value;
  }

  public int BaseItem
  {
    get => this.m_baseItem;
    set => this.m_baseItem = value;
  }

  public int BaseField
  {
    get => this.m_baseField;
    set => this.m_baseField = value;
  }

  public PivotDataField(string name, PivotSubtotalTypes subtotal, PivotFieldImpl parentField)
  {
    if (parentField == null)
      throw new ArgumentNullException(nameof (parentField));
    this.m_strName = name;
    this.m_subtotal = subtotal;
    this.m_field = parentField;
    parentField.IsDataField = true;
    if (parentField.Axis == PivotAxisTypes.None)
      parentField.Axis = PivotAxisTypes.Data;
    this.InitializeShowData();
    this.InitializeExcel2010Data();
  }

  public void SetPreviousBaseItem() => this.BaseItem = 1048828;

  public void SetNextBaseItem() => this.BaseItem = 1048829;

  private void InitializeShowData()
  {
    if (this.m_showDataCollections != null)
      return;
    this.m_showDataCollections = new Dictionary<PivotFieldDataFormat, string>();
    this.m_showDataCollections.Add(PivotFieldDataFormat.Difference, "difference");
    this.m_showDataCollections.Add(PivotFieldDataFormat.Index, "index");
    this.m_showDataCollections.Add(PivotFieldDataFormat.Normal, "normal");
    this.m_showDataCollections.Add(PivotFieldDataFormat.Percent, "percent");
    this.m_showDataCollections.Add(PivotFieldDataFormat.PercentageOfDifference, "percentDiff");
    this.m_showDataCollections.Add(PivotFieldDataFormat.PercentageOfColumn, "percentOfCol");
    this.m_showDataCollections.Add(PivotFieldDataFormat.PercentageOfRow, "percentOfRow");
    this.m_showDataCollections.Add(PivotFieldDataFormat.PercentageOfTotal, "percentOfTotal");
    this.m_showDataCollections.Add(PivotFieldDataFormat.RunTotal, "runTotal");
    this.m_showDataCollections.Add(PivotFieldDataFormat.PercentageOfParentColumn, "percentOfParentCol");
    this.m_showDataCollections.Add(PivotFieldDataFormat.PercentageOfParentRow, "percentOfParentRow");
    this.m_showDataCollections.Add(PivotFieldDataFormat.RankAscending, "rankAscending");
    this.m_showDataCollections.Add(PivotFieldDataFormat.RankDecending, "rankDescending");
    this.m_showDataCollections.Add(PivotFieldDataFormat.PercentageOfRunningTotal, "percentOfRunningTotal");
    this.m_showDataCollections.Add(PivotFieldDataFormat.PercentageOfParent, "percentOfParent");
  }

  private void InitializeExcel2010Data()
  {
    if (this.m_excel2010Data != null)
      return;
    this.m_excel2010Data = new List<PivotFieldDataFormat>();
    this.m_excel2010Data.Add(PivotFieldDataFormat.PercentageOfParentColumn);
    this.m_excel2010Data.Add(PivotFieldDataFormat.PercentageOfParentRow);
    this.m_excel2010Data.Add(PivotFieldDataFormat.RankAscending);
    this.m_excel2010Data.Add(PivotFieldDataFormat.RankDecending);
    this.m_excel2010Data.Add(PivotFieldDataFormat.PercentageOfRunningTotal);
    this.m_excel2010Data.Add(PivotFieldDataFormat.PercentageOfParent);
  }

  internal PivotFieldDataFormat SetShowData(string value)
  {
    foreach (KeyValuePair<PivotFieldDataFormat, string> showDataCollection in this.m_showDataCollections)
    {
      if (showDataCollection.Value == value)
        return showDataCollection.Key;
    }
    return PivotFieldDataFormat.Normal;
  }

  internal string GetShowData(PivotFieldDataFormat value) => this.m_showDataCollections[value];

  internal bool IsExcel2010Data()
  {
    return Array.IndexOf<PivotFieldDataFormat>(this.m_excel2010Data.ToArray(), this.ShowDataAs) >= 0;
  }

  public object Clone(object parent)
  {
    PivotDataField pivotDataField = (PivotDataField) this.MemberwiseClone();
    PivotDataFields parent1 = (PivotDataFields) CommonObject.FindParent(parent, typeof (PivotDataFields));
    PivotTableImpl parent2 = (PivotTableImpl) CommonObject.FindParent(parent, typeof (PivotTableImpl));
    pivotDataField.m_field = (PivotFieldImpl) parent2.Fields[this.m_field.Name];
    return (object) pivotDataField;
  }
}
