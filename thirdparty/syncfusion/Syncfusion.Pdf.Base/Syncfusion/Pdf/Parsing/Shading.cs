// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.Shading
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using Syncfusion.PdfViewer.Base;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal abstract class Shading
{
  private Colorspace m_alternateColorspace;
  private bool m_isRectangle;
  private bool m_isCircle;
  private string m_rectangleWidth;

  internal Colorspace AlternateColorspace
  {
    get => this.m_alternateColorspace;
    set => this.m_alternateColorspace = value;
  }

  public Shading()
  {
  }

  public Shading(PdfDictionary dictionary)
  {
    this.m_alternateColorspace = this.GetColorspace(dictionary);
  }

  private Colorspace GetColorspace(PdfDictionary dictionary)
  {
    if (dictionary.Items.ContainsKey(new PdfName("ColorSpace")))
    {
      IPdfPrimitive pdfPrimitive = dictionary.Items[new PdfName("ColorSpace")];
      PdfReferenceHolder pdfReferenceHolder = pdfPrimitive as PdfReferenceHolder;
      if (pdfReferenceHolder != (PdfReferenceHolder) null)
      {
        if (pdfReferenceHolder.Object is PdfArray array)
          return Colorspace.CreateColorSpace((array[0] as PdfName).Value, (IPdfPrimitive) array);
        PdfName pdfName = pdfReferenceHolder.Object as PdfName;
        if (pdfName != (PdfName) null)
          return Colorspace.CreateColorSpace(pdfName.Value);
      }
      if (pdfPrimitive is PdfArray array1)
        return Colorspace.CreateColorSpace((array1[0] as PdfName).Value, (IPdfPrimitive) array1);
      PdfName pdfName1 = pdfPrimitive as PdfName;
      if (pdfName1 != (PdfName) null)
        return Colorspace.CreateColorSpace(pdfName1.Value);
    }
    return (Colorspace) null;
  }

  internal virtual void SetOperatorValues(bool IsRectangle, bool IsCircle, string RectangleWidth)
  {
    this.m_isRectangle = IsRectangle;
    this.m_isCircle = IsCircle;
    this.m_rectangleWidth = RectangleWidth;
  }

  internal abstract Brush GetBrushColor(Matrix transformMatrix);
}
