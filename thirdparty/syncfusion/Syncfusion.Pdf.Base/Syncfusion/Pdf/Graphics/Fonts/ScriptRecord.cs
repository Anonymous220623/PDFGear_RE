// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.ScriptRecord
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal struct ScriptRecord
{
  private string m_scriptTag;
  private Syncfusion.Pdf.Graphics.Fonts.LanguageRecord m_language;
  private Syncfusion.Pdf.Graphics.Fonts.LanguageRecord[] m_LanguageRecord;

  internal string ScriptTag
  {
    get => this.m_scriptTag;
    set => this.m_scriptTag = value;
  }

  internal Syncfusion.Pdf.Graphics.Fonts.LanguageRecord Language
  {
    get => this.m_language;
    set => this.m_language = value;
  }

  internal Syncfusion.Pdf.Graphics.Fonts.LanguageRecord[] LanguageRecord
  {
    get => this.m_LanguageRecord;
    set => this.m_LanguageRecord = value;
  }
}
