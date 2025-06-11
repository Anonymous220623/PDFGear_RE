// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.FontScheme
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class FontScheme
{
  private string m_fontSchemeName;
  private MajorMinorFontScheme m_majorFontScheme;
  private MajorMinorFontScheme m_minorFontScheme;

  internal string FontSchemeName
  {
    get => this.m_fontSchemeName;
    set => this.m_fontSchemeName = value;
  }

  internal MajorMinorFontScheme MajorFontScheme
  {
    get => this.m_majorFontScheme;
    set => this.m_majorFontScheme = new MajorMinorFontScheme();
  }

  internal MajorMinorFontScheme MinorFontScheme
  {
    get => this.m_minorFontScheme;
    set => this.m_minorFontScheme = new MajorMinorFontScheme();
  }

  public FontScheme()
  {
    this.m_majorFontScheme = new MajorMinorFontScheme();
    this.m_minorFontScheme = new MajorMinorFontScheme();
  }

  internal void Close()
  {
    if (this.m_majorFontScheme != null)
    {
      this.m_majorFontScheme.Close();
      this.m_majorFontScheme = (MajorMinorFontScheme) null;
    }
    if (this.m_minorFontScheme == null)
      return;
    this.m_minorFontScheme.Close();
    this.m_minorFontScheme = (MajorMinorFontScheme) null;
  }
}
