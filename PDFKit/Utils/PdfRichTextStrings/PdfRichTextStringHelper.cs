// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfRichTextStrings.PdfRichTextStringHelper
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using PDFKit.Utils.PdfRichTextStrings.HtmlToXaml;
using System;
using System.Text;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace PDFKit.Utils.PdfRichTextStrings;

public static class PdfRichTextStringHelper
{
  public static string GetUnicodeText(string text)
  {
    if (text == null)
      return (string) null;
    if (string.IsNullOrEmpty(text))
      return string.Empty;
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < text.Length; ++index)
    {
      int utf32 = char.ConvertToUtf32(text, index);
      stringBuilder.AppendFormat("&#{0};", (object) utf32);
    }
    return stringBuilder.ToString();
  }

  public static PdfRichTextString FromRichTextBox(
    RichTextBox rtb,
    PdfRichTextStyle? defaultStyle,
    string annotName = "")
  {
    string str1 = rtb != null ? HtmlFromXamlConverter.ConvertXamlToHtml(XamlWriter.Save((object) rtb.Document)) : throw new ArgumentNullException(nameof (rtb));
    if (!string.IsNullOrEmpty(str1) && str1.Length >= 26 && str1.StartsWith("<html><body>") && str1.EndsWith("</body></html>"))
    {
      string str2 = str1.Substring(12, str1.Length - 26);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("<?xml version=\"1.0\"?><body xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:xfa=\"http://www.xfa.org/schema/xfa-data/1.0/\" xfa:APIVersion=\"Acrobat:11.0.0\" xfa:spec=\" 2.0.2\">");
      stringBuilder.Append(str2);
      stringBuilder.Append("</body>");
      string richText = stringBuilder.ToString();
      if (defaultStyle.HasValue)
      {
        PdfRichTextString pdfRichTextString;
        if (PdfRichTextString.TryParse(richText, defaultStyle.Value, out pdfRichTextString, annotName))
          return pdfRichTextString;
      }
      else
      {
        PdfRichTextString pdfRichTextString;
        if (PdfRichTextString.TryParse(richText, out pdfRichTextString, annotName))
          return pdfRichTextString;
      }
    }
    return new PdfRichTextString()
    {
      Text = (PdfRichTextRawElement) new PdfRichTextElement()
      {
        Tag = PdfRichTextElementTag.Body
      }
    };
  }
}
