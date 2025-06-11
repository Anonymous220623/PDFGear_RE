// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfRichTextStrings.PdfRichTextElement
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace PDFKit.Utils.PdfRichTextStrings;

public class PdfRichTextElement : PdfRichTextRawElement
{
  public PdfRichTextElement() => this.Children = new List<PdfRichTextRawElement>();

  public PdfRichTextElementTag Tag { get; set; }

  public PdfRichTextStyle? Style { get; set; }

  public List<PdfRichTextRawElement> Children { get; }

  public override string Text
  {
    get
    {
      if (this.Children.Count == 0)
        return string.Empty;
      if (this.Children.Count == 1)
        return this.Children[0].Text;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (PdfRichTextRawElement child in this.Children)
        stringBuilder.Append(child.Text);
      return stringBuilder.ToString();
    }
  }
}
