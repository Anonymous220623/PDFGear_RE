// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotTableFilter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class PivotTableFilter
{
  private string m_DescriptionAttribute;
  private int i_EvalOrder;
  private int i_Field;
  private int i_FilterId;
  private string str_Value1;
  private string str_Value2;
  private PivotFilterType type;
  private int m_iMeasureFld;
  private int m_iMeasureHier;
  private List<PivotAutoFilter> m_pivotAutoFilter = new List<PivotAutoFilter>();

  public string DescriptionAttribute
  {
    get => this.m_DescriptionAttribute;
    set => this.m_DescriptionAttribute = value;
  }

  public int EvalOrder
  {
    get => this.i_EvalOrder;
    set => this.i_EvalOrder = value;
  }

  public int Field
  {
    get => this.i_Field;
    set => this.i_Field = value;
  }

  public int MeasureFld
  {
    get => this.m_iMeasureFld;
    set => this.m_iMeasureFld = value;
  }

  internal int MeasureHier
  {
    get => this.m_iMeasureHier;
    set => this.m_iMeasureHier = value;
  }

  public int FilterId
  {
    get => this.i_FilterId;
    set => this.i_FilterId = value;
  }

  public string Value1
  {
    get => this.str_Value1;
    set => this.str_Value1 = value;
  }

  public string Value2
  {
    get => this.str_Value2;
    set => this.str_Value2 = value;
  }

  public PivotFilterType Type
  {
    get => this.type;
    set => this.type = value;
  }

  public PivotAutoFilter this[int index]
  {
    get
    {
      if (this.m_pivotAutoFilter.Count > 0 && index < this.m_pivotAutoFilter.Count)
        return this.m_pivotAutoFilter[index];
      throw new ArgumentOutOfRangeException("Index");
    }
  }

  public int Count => this.m_pivotAutoFilter.Count;

  public void Add(PivotAutoFilter AutoFilter) => this.m_pivotAutoFilter.Add(AutoFilter);
}
