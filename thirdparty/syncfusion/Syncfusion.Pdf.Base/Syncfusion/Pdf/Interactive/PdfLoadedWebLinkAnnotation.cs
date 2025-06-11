// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfLoadedWebLinkAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfLoadedWebLinkAnnotation : PdfLoadedUriAnnotation
{
  private PdfCrossTable m_crossTable;

  internal PdfLoadedWebLinkAnnotation(
    PdfDictionary dictionary,
    PdfCrossTable crossTable,
    RectangleF rectangle,
    string text)
    : base(dictionary, crossTable, rectangle, text)
  {
    if (text == null)
      throw new ArgumentNullException(nameof (text));
    this.Dictionary = dictionary;
    this.m_crossTable = crossTable;
    this.Text = text;
  }
}
