// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.OTableColumn
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class OTableColumn
{
  private string m_defaultCellStyleName;
  private int m_repeatedRowColumns;
  private string m_styleName;
  private bool m_visibility;
  private int m_outlineLevel;
  private bool m_isCollapsed;

  internal string DefaultCellStyleName
  {
    get => this.m_defaultCellStyleName;
    set => this.m_defaultCellStyleName = value;
  }

  internal int RepeatedRowColumns
  {
    get => this.m_repeatedRowColumns;
    set => this.m_repeatedRowColumns = value;
  }

  internal string StyleName
  {
    get => this.m_styleName;
    set => this.m_styleName = value;
  }

  internal bool Visibility
  {
    get => this.m_visibility;
    set => this.m_visibility = value;
  }

  internal int OutlineLevel
  {
    get => this.m_outlineLevel;
    set => this.m_outlineLevel = value;
  }

  internal bool IsCollapsed
  {
    get => this.m_isCollapsed;
    set => this.m_isCollapsed = value;
  }
}
