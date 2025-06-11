// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.OutlineWrapper
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Parser.Biff_Records;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public class OutlineWrapper : CommonWrapper, IOutlineWrapper, IOutline
{
  private int m_firstIndex;
  private int m_lastIndex;
  private IOutline m_outline;
  private IRange m_outlineRange;
  private ExcelGroupBy m_groupBy;
  private ushort m_outlineLevel;
  private bool m_bIsCollapsed;
  private bool m_bIsHidden;
  private ushort m_formatIndex;
  private ushort m_index;

  public int FirstIndex
  {
    get => this.m_firstIndex;
    set => this.m_firstIndex = value;
  }

  public int LastIndex
  {
    get => this.m_lastIndex;
    set => this.m_lastIndex = value;
  }

  public IOutline Outline
  {
    get => this.m_outline;
    set => this.m_outline = value;
  }

  public ushort Index
  {
    get => this.m_index;
    set => this.m_index = value;
  }

  public IRange OutlineRange
  {
    get => this.m_outlineRange;
    set => this.m_outlineRange = value;
  }

  public ExcelGroupBy GroupBy
  {
    get => this.m_groupBy;
    set => this.m_groupBy = value;
  }

  public ushort ExtendedFormatIndex
  {
    get => this.m_formatIndex;
    set => this.m_formatIndex = value;
  }

  public bool IsHidden
  {
    get => this.m_bIsHidden;
    set => this.m_bIsHidden = value;
  }

  public bool IsCollapsed
  {
    get => this.m_bIsCollapsed;
    set => this.m_bIsCollapsed = value;
  }

  public ushort OutlineLevel
  {
    get => this.m_outlineLevel;
    set => this.m_outlineLevel = value;
  }
}
