// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.LanguageStyle
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class LanguageStyle
{
  private string m_country;
  private string m_language;
  private string m_rFCLanguageTag;
  private string m_script;

  internal string Country
  {
    get => this.m_country;
    set => this.m_country = value;
  }

  internal string Language
  {
    get => this.m_language;
    set => this.m_language = value;
  }

  internal string RFCLanguageTag
  {
    get => this.m_rFCLanguageTag;
    set => this.m_rFCLanguageTag = value;
  }

  internal string Script
  {
    get => this.m_script;
    set => this.m_script = value;
  }
}
