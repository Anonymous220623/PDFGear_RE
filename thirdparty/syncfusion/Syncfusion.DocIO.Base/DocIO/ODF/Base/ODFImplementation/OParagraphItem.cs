// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ODF.Base.ODFImplementation.OParagraphItem
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.ODF.Base.ODFImplementation;

internal class OParagraphItem
{
  private TextProperties m_TextProperties;
  private ODFParagraphProperties m_ParagraphProperties;
  private string m_text;
  private bool m_span;
  private bool m_space;
  private string m_styleName;

  internal string StyleName
  {
    get => this.m_styleName;
    set => this.m_styleName = value;
  }

  internal bool Space
  {
    get => this.m_space;
    set => this.m_space = value;
  }

  internal bool Span
  {
    get => this.m_span;
    set => this.m_span = value;
  }

  internal string Text
  {
    get => this.m_text;
    set => this.m_text = value;
  }

  internal ODFParagraphProperties ParagraphProperties
  {
    get => this.m_ParagraphProperties;
    set => this.m_ParagraphProperties = value;
  }

  internal TextProperties TextProperties
  {
    get => this.m_TextProperties;
    set => this.m_TextProperties = value;
  }

  internal void Dispose()
  {
    if (this.m_ParagraphProperties != null)
    {
      this.m_ParagraphProperties.Close();
      this.m_ParagraphProperties = (ODFParagraphProperties) null;
    }
    if (this.m_TextProperties == null)
      return;
    this.m_TextProperties = (TextProperties) null;
  }
}
