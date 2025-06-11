// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedUriAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedUriAnnotation : PdfLoadedStyledAnnotation
{
  private PdfCrossTable m_crossTable;
  private string m_uri;

  public string Uri
  {
    get => this.GetUriText();
    set
    {
      switch (value)
      {
        case null:
          throw new ArgumentNullException("uri");
        case "":
          throw new ArgumentException("Uri can not be an empty string");
        default:
          if (!(this.m_uri != value))
            break;
          this.m_uri = value;
          PdfDictionary dictionary = this.Dictionary;
          if (!this.Dictionary.ContainsKey("A"))
            break;
          if (PdfCrossTable.Dereference(this.Dictionary["A"]) is PdfDictionary pdfDictionary)
            pdfDictionary.SetString("URI", this.m_uri);
          this.Dictionary.Modify();
          break;
      }
    }
  }

  internal PdfLoadedUriAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rectangle,
    string text)
    : base(dictionary, crossTable)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    this.Dictionary = dictionary;
    this.m_crossTable = crossTable;
    this.Text = text;
  }

  private string GetUriText()
  {
    string empty = string.Empty;
    if (this.Dictionary.ContainsKey("A") && PdfCrossTable.Dereference(this.Dictionary["A"]) is PdfDictionary pdfDictionary && pdfDictionary.ContainsKey("URI") && PdfCrossTable.Dereference(pdfDictionary["URI"]) is PdfString pdfString)
      empty = Encoding.UTF8.GetString(pdfString.Bytes, 0, pdfString.Bytes.Length);
    return empty;
  }
}
