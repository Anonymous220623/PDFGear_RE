// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfRichTextStrings.PdfRichTextRawElement
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Diagnostics;

#nullable disable
namespace PDFKit.Utils.PdfRichTextStrings;

[DebuggerDisplay("Text = {Text}")]
public class PdfRichTextRawElement
{
  private string textReplaceSpace;

  public PdfRichTextRawElement()
  {
  }

  public PdfRichTextRawElement(string text)
  {
    this.Text = !string.IsNullOrEmpty(text) ? text : throw new ArgumentException(nameof (text));
  }

  public virtual string Text { get; }

  public string TextUnicode => PdfRichTextStringHelper.GetUnicodeText(this.Text);

  public override string ToString()
  {
    if (this.textReplaceSpace == null)
      this.textReplaceSpace = this.Text.Replace(" ", " ");
    return this.textReplaceSpace;
  }
}
