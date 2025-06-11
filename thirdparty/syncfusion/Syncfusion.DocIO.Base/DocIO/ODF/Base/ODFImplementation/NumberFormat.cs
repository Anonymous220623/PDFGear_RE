// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.NumberFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class NumberFormat : LanguageStyle
{
  private string m_title;
  private string m_transliterationCountry;
  private string m_transliterationFormat;
  private string m_transliterationLanguage;
  private string m_transliterationStyle;

  internal string Title
  {
    get => this.m_title;
    set => this.m_title = value;
  }

  internal string TransliterationCountry
  {
    get => this.m_transliterationCountry;
    set => this.m_transliterationCountry = value;
  }

  internal string TransliterationFormat
  {
    get => this.m_transliterationFormat;
    set => this.m_transliterationFormat = value;
  }

  internal string TransliterationLanguage
  {
    get => this.m_transliterationLanguage;
    set => this.m_transliterationLanguage = value;
  }

  internal string TransliterationStyle
  {
    get => this.m_transliterationStyle;
    set => this.m_transliterationStyle = value;
  }
}
