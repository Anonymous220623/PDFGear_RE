// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ExcelImportDataOptions
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public class ExcelImportDataOptions
{
  private int m_firstRow = 1;
  private int m_firstColumn = 1;
  private bool m_includeHeader = true;
  private bool m_includeHeaderParent;
  private ExcelNestedDataLayoutOptions m_nestedDataLayoutOptions;
  private ExcelNestedDataGroupOptions m_nestedDataGroupOptions = ~ExcelNestedDataGroupOptions.Expand;
  private int m_collapseLevel = 1;
  private bool m_preserveTypes = true;

  public int FirstRow
  {
    get => this.m_firstRow;
    set => this.m_firstRow = value;
  }

  public int FirstColumn
  {
    get => this.m_firstColumn;
    set => this.m_firstColumn = value;
  }

  public bool IncludeHeader
  {
    get => this.m_includeHeader;
    set => this.m_includeHeader = value;
  }

  public bool IncludeHeaderParent
  {
    get => this.m_includeHeaderParent;
    set => this.m_includeHeaderParent = value;
  }

  public ExcelNestedDataLayoutOptions NestedDataLayoutOptions
  {
    get => this.m_nestedDataLayoutOptions;
    set => this.m_nestedDataLayoutOptions = value;
  }

  public ExcelNestedDataGroupOptions NestedDataGroupOptions
  {
    get => this.m_nestedDataGroupOptions;
    set => this.m_nestedDataGroupOptions = value;
  }

  public int CollapseLevel
  {
    get => this.m_collapseLevel;
    set => this.m_collapseLevel = value;
  }

  public bool PreserveTypes
  {
    get => this.m_preserveTypes;
    set => this.m_preserveTypes = value;
  }
}
