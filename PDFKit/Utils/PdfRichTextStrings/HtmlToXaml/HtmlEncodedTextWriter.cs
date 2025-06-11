// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.PdfRichTextStrings.HtmlToXaml.HtmlEncodedTextWriter
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System.IO;
using System.Net;
using System.Xml;

#nullable disable
namespace PDFKit.Utils.PdfRichTextStrings.HtmlToXaml;

public class HtmlEncodedTextWriter(TextWriter w) : XmlTextWriter(w)
{
  public override void WriteString(string text)
  {
    text = WebUtility.HtmlEncode(text);
    this.WriteRaw(text);
  }
}
