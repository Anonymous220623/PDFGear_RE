// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotValueLableFilter
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

internal class PivotValueLableFilter : IPivotValueLableFilter
{
  private string m_strValue1;
  private string m_strValue2;
  private PivotFilterType m_type;
  private IPivotField m_field;

  public string Value1
  {
    get => this.m_strValue1;
    set => this.m_strValue1 = value;
  }

  public string Value2
  {
    get => this.m_strValue2;
    set => this.m_strValue2 = value;
  }

  public PivotFilterType Type
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  public IPivotField DataField
  {
    get => this.m_field;
    set => this.m_field = value;
  }
}
