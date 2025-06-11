// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Shapes.LevelProperties
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Shapes;

internal class LevelProperties
{
  private DefaultRunProperties m_runProperties;
  private TextAlignType m_textAlignment;
  private int m_defaultTab;
  private bool m_IsEaBreak;
  private FontAlignmentType m_fontAlignment;
  private int m_indent = -342900;
  private bool m_isLatinBreak;
  private int m_leftMargin;
  private int m_rightMargin;
  private bool m_IsRightToLeft;

  internal DefaultRunProperties RunProperties
  {
    get
    {
      if (this.m_runProperties == null)
        this.m_runProperties = new DefaultRunProperties();
      return this.m_runProperties;
    }
    set => this.m_runProperties = value;
  }

  internal TextAlignType TextAlignment
  {
    get => this.m_textAlignment;
    set => this.m_textAlignment = value;
  }

  internal int TabSize
  {
    get => this.m_defaultTab;
    set => this.m_defaultTab = value;
  }

  internal bool EastAsianBreak
  {
    get => this.m_IsEaBreak;
    set => this.m_IsEaBreak = value;
  }

  internal FontAlignmentType FontAlignment
  {
    get => this.m_fontAlignment;
    set => this.m_fontAlignment = value;
  }

  internal int Indent
  {
    get => this.m_indent;
    set => this.m_indent = value;
  }

  internal bool IsLatinBreak
  {
    get => this.m_isLatinBreak;
    set => this.m_isLatinBreak = value;
  }

  internal int LeftMargin
  {
    get => this.m_leftMargin;
    set => this.m_leftMargin = value;
  }

  internal int RightMargin
  {
    get => this.m_rightMargin;
    set => this.m_rightMargin = value;
  }

  internal bool IsRightToLeft
  {
    get => this.m_IsRightToLeft;
    set => this.m_IsRightToLeft = value;
  }
}
