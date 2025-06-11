// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.ShadingPattern
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using Syncfusion.PdfViewer.Base;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class ShadingPattern : Pattern
{
  private Shading m_shading;
  private PdfDictionary m_shadingDictionary;
  private PdfArray m_patternMatrix;
  private PdfArray m_bBox;
  private bool m_isRectangle;
  private bool m_isCircle;
  private string m_rectangleWidth;

  internal new PdfArray PatternMatrix
  {
    get => this.m_patternMatrix;
    set => this.m_patternMatrix = value;
  }

  internal Shading ShadingType
  {
    get => this.m_shading;
    set => this.m_shading = value;
  }

  internal PdfArray BBox
  {
    get
    {
      if (this.m_bBox == null && this.m_shadingDictionary.ContainsKey(new PdfName(nameof (BBox))))
        this.m_bBox = this.m_shadingDictionary[new PdfName(nameof (BBox))] as PdfArray;
      return this.m_bBox;
    }
    set => this.m_bBox = value;
  }

  public ShadingPattern()
  {
  }

  public ShadingPattern(
    PdfDictionary dictionary,
    bool IsRectangle,
    bool IsCircle,
    string RectangleWidth)
  {
    this.m_shadingDictionary = dictionary;
    this.m_shading = this.CreateShading(dictionary);
    PdfArray pdfArray;
    if (!dictionary.Items.ContainsKey(new PdfName("Matrix")))
      pdfArray = new PdfArray(new int[6]{ 1, 0, 0, 1, 0, 0 });
    else
      pdfArray = dictionary.Items[new PdfName("Matrix")] as PdfArray;
    this.m_patternMatrix = pdfArray;
    this.m_isRectangle = IsRectangle;
    this.m_isCircle = IsCircle;
    this.m_rectangleWidth = RectangleWidth;
  }

  internal void SetShadingValue(IPdfPrimitive shadingResource)
  {
    this.m_shadingDictionary = shadingResource as PdfDictionary;
    if (this.m_shadingDictionary == null)
      return;
    this.m_shading = this.CreateShading(this.m_shadingDictionary);
    PdfArray pdfArray;
    if (!this.m_shadingDictionary.Items.ContainsKey(new PdfName("Matrix")))
      pdfArray = new PdfArray(new int[6]{ 1, 0, 0, 1, 0, 0 });
    else
      pdfArray = this.m_shadingDictionary.Items[new PdfName("Matrix")] as PdfArray;
    this.m_patternMatrix = pdfArray;
  }

  internal override void SetOperatorValues(bool IsRectangle, bool IsCircle, string RectangleWidth)
  {
    this.m_isRectangle = IsRectangle;
    this.m_isCircle = IsCircle;
    this.m_rectangleWidth = RectangleWidth;
  }

  internal Shading CreateShading(PdfDictionary dictionary)
  {
    if (dictionary.Items.ContainsKey(new PdfName("Shading")))
    {
      IPdfPrimitive dictionary1 = dictionary.Items[new PdfName("Shading")];
      if (!(dictionary1 is PdfDictionary pdfDictionary))
        pdfDictionary = (dictionary1 as PdfReferenceHolder).Object as PdfDictionary;
      if (pdfDictionary != null)
      {
        switch ((pdfDictionary.Items[new PdfName("ShadingType")] as PdfNumber).IntValue)
        {
          case 2:
            return dictionary1 is PdfDictionary ? (Shading) new AxialShading(dictionary1 as PdfDictionary) : (Shading) new AxialShading((dictionary1 as PdfReferenceHolder).Object as PdfDictionary);
          case 3:
            return dictionary1 is PdfDictionary ? (Shading) new RadialShading(dictionary1 as PdfDictionary) : (Shading) new RadialShading((dictionary1 as PdfReferenceHolder).Object as PdfDictionary);
        }
      }
    }
    else if (dictionary.Items.ContainsKey(new PdfName("ShadingType")))
    {
      switch ((dictionary.Items[new PdfName("ShadingType")] as PdfNumber).IntValue)
      {
        case 2:
          return (Shading) new AxialShading(dictionary);
        case 3:
          return (Shading) new RadialShading(dictionary);
      }
    }
    return (Shading) null;
  }

  internal Brush CreateBrush()
  {
    if (this.ShadingType == null)
      return Brushes.Transparent;
    this.ShadingType.SetOperatorValues(this.m_isRectangle, this.m_isCircle, this.m_rectangleWidth);
    return this.ShadingType.GetBrushColor(this.ConvertArrayToMatrix(this.PatternMatrix));
  }

  private Matrix ConvertArrayToMatrix(PdfArray array)
  {
    return new Matrix((double) (array[0] as PdfNumber).FloatValue, (double) (array[1] as PdfNumber).FloatValue, (double) (array[2] as PdfNumber).FloatValue, (double) (array[3] as PdfNumber).FloatValue, (double) (array[4] as PdfNumber).FloatValue, (double) (array[5] as PdfNumber).FloatValue);
  }
}
