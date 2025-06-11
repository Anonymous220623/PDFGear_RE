// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.NumberFormat
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

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
