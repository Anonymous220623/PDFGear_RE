// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotCustomFilter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class PivotCustomFilter
{
  private FilterOperator2007 m_filterOperator;
  private string m_strValue;

  public FilterOperator2007 FilterOperator
  {
    get => this.m_filterOperator;
    set => this.m_filterOperator = value;
  }

  public string Value
  {
    get => this.m_strValue;
    set => this.m_strValue = value;
  }
}
