// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.CssStyle
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class CssStyle
{
  private string m_color;
  private string m_borderCollapse;
  private string m_fontFamily;
  private string m_fontSize;
  private string m_bgColor;
  private string m_border;
  private string m_topBorder;
  private string m_bottomBorder;
  private string m_rightBorder;
  private string m_leftBorder;
  private string m_textAlign;
  private string m_width;

  internal string Color
  {
    get => this.m_color;
    set => this.m_color = value;
  }

  internal string BgColor
  {
    get => this.m_bgColor;
    set => this.m_bgColor = value;
  }

  internal string FontFamily
  {
    get => this.m_fontFamily;
    set => this.m_fontFamily = value;
  }

  internal string FontSize
  {
    get => this.m_fontSize;
    set => this.m_fontSize = value;
  }

  internal string Border
  {
    get => this.m_border;
    set => this.m_border = value;
  }

  internal string TopBorder
  {
    get => this.m_topBorder;
    set => this.m_topBorder = value;
  }

  internal string BottomBorder
  {
    get => this.m_bottomBorder;
    set => this.m_bottomBorder = value;
  }

  internal string LeftBorder
  {
    get => this.m_leftBorder;
    set => this.m_leftBorder = value;
  }

  internal string RightBorder
  {
    get => this.m_rightBorder;
    set => this.m_rightBorder = value;
  }

  internal string BorderCollapse
  {
    get => this.m_borderCollapse;
    set => this.m_borderCollapse = value;
  }

  internal string TextAlign
  {
    get => this.m_textAlign;
    set => this.m_textAlign = value;
  }

  internal string Width
  {
    get => this.m_width;
    set => this.m_width = value;
  }
}
