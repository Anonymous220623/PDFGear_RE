// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Convertors.RtfFont
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS.Convertors;

internal class RtfFont
{
  private int m_fontNumber;
  private string m_fontID;
  private string m_fontName;
  private short m_fontCharSet = 1;
  private string m_alternateFontName;

  internal int FontNumber
  {
    get => this.m_fontNumber;
    set => this.m_fontNumber = value;
  }

  internal string AlternateFontName
  {
    get => this.m_alternateFontName;
    set => this.m_alternateFontName = value;
  }

  internal string FontID
  {
    get => this.m_fontID;
    set => this.m_fontID = value;
  }

  internal string FontName
  {
    get => this.m_fontName;
    set => this.m_fontName = value;
  }

  internal short FontCharSet
  {
    get => this.m_fontCharSet;
    set => this.m_fontCharSet = value;
  }

  internal RtfFont Clone() => (RtfFont) this.MemberwiseClone();
}
