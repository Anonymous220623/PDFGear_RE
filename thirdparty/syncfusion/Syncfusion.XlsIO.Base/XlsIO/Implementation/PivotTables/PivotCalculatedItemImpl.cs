// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotTables.PivotCalculatedItemImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Interfaces.PivotTables;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotTables;

public class PivotCalculatedItemImpl : IPivotCalculatedItem
{
  private string m_formula;
  private PivotCacheFieldImpl fieldImpl;
  private int m_iFieldIndex;
  private PivotArea m_pivotArea;

  public string Formula
  {
    get => this.m_formula;
    set => this.m_formula = value;
  }

  public PivotArea PivotArea => this.m_pivotArea;

  internal PivotCacheFieldImpl cacheField
  {
    get => this.cacheField;
    set => this.cacheField = value;
  }

  internal int FieldIndex
  {
    get => this.m_iFieldIndex;
    set => this.m_iFieldIndex = value;
  }

  public PivotCalculatedItemImpl(PivotCacheFieldImpl cacheField)
  {
    this.fieldImpl = cacheField;
    this.m_pivotArea = new PivotArea(this.fieldImpl);
    this.m_iFieldIndex = -1;
  }
}
