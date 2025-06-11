// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Shapes.DefaultRunProperties
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Shapes;

internal class DefaultRunProperties
{
  private bool m_isBold;
  private bool m_isItalic;
  private UnderlineStyle m_isUnderline = UnderlineStyle.None;
  private int m_fontSize;
  private StrikeType m_strikeType;

  internal bool IsBold
  {
    get => this.m_isBold;
    set => this.m_isBold = value;
  }

  internal UnderlineStyle UnderlineStyle
  {
    get => this.m_isUnderline;
    set => this.m_isUnderline = value;
  }

  internal bool IsItalic
  {
    get => this.m_isItalic;
    set => this.m_isItalic = value;
  }

  internal int FontSize
  {
    get => this.m_fontSize;
    set => this.m_fontSize = value;
  }

  internal StrikeType StrikeType
  {
    get => this.m_strikeType;
    set => this.m_strikeType = value;
  }
}
