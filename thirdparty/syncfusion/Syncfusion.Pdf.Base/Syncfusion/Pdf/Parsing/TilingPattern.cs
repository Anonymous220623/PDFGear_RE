// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.TilingPattern
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System.Drawing;
using System.Drawing.Drawing2D;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class TilingPattern : Pattern
{
  private int m_paintType;
  private int m_tilingType;
  private PdfArray m_boundBox;
  private PdfNumber m_xStep;
  private PdfNumber m_yStep;
  private PdfDictionary m_resource;
  private PdfArray m_patternMatrix;
  private PdfStream m_data;
  private Image m_embeddedImage;
  private Matrix m_tilingPatternMatrix;

  internal Rectangle BoundingRectangle => this.GetBoundingRectangle();

  internal Matrix TilingPatternMatrix
  {
    get => this.m_tilingPatternMatrix;
    set => this.m_tilingPatternMatrix = value;
  }

  internal int PaintType
  {
    get => this.m_paintType;
    set => this.m_paintType = value;
  }

  internal int TilingType
  {
    get => this.m_tilingType;
    set => this.m_tilingType = value;
  }

  internal PdfArray BBox
  {
    get => this.m_boundBox;
    set => this.m_boundBox = value;
  }

  internal PdfNumber XStep
  {
    get => this.m_xStep;
    set => this.m_xStep = value;
  }

  internal PdfNumber YStep
  {
    get => this.m_yStep;
    set => this.m_yStep = value;
  }

  internal PdfDictionary Resources
  {
    get => this.m_resource;
    set => this.m_resource = value;
  }

  internal new PdfArray PatternMatrix
  {
    get => this.m_patternMatrix;
    set => this.m_patternMatrix = value;
  }

  internal PdfStream Data
  {
    get => this.m_data;
    set => this.m_data = value;
  }

  internal Image EmbeddedImage
  {
    get => this.m_embeddedImage;
    set => this.m_embeddedImage = value;
  }

  public TilingPattern()
  {
  }

  public TilingPattern(PdfDictionary dictionary)
  {
    this.m_tilingType = (dictionary.Items[new PdfName(nameof (TilingType))] as PdfNumber).IntValue;
    this.m_paintType = (dictionary.Items[new PdfName(nameof (PaintType))] as PdfNumber).IntValue;
    this.m_xStep = dictionary.Items[new PdfName(nameof (XStep))] as PdfNumber;
    this.m_yStep = dictionary.Items[new PdfName(nameof (YStep))] as PdfNumber;
    this.m_resource = this.GetResource(dictionary);
    this.m_boundBox = dictionary.Items[new PdfName(nameof (BBox))] as PdfArray;
    PdfArray pdfArray;
    if (!dictionary.Items.ContainsKey(new PdfName("Matrix")))
      pdfArray = new PdfArray(new int[6]{ 1, 0, 0, 1, 0, 0 });
    else
      pdfArray = dictionary.Items[new PdfName("Matrix")] as PdfArray;
    this.m_patternMatrix = pdfArray;
    this.m_data = dictionary is PdfStream ? dictionary as PdfStream : (PdfStream) null;
  }

  private Rectangle GetBoundingRectangle()
  {
    return new Rectangle((this.m_boundBox[0] as PdfNumber).IntValue, (this.m_boundBox[1] as PdfNumber).IntValue, (this.m_boundBox[2] as PdfNumber).IntValue, (this.m_boundBox[3] as PdfNumber).IntValue);
  }

  internal Brush CreateBrush()
  {
    if (this.EmbeddedImage == null)
      return Brushes.Transparent;
    Rectangle dstRect = this.BoundingRectangle;
    if (this.BoundingRectangle.Width - this.BoundingRectangle.X < 1 && this.BoundingRectangle.Height - this.BoundingRectangle.Y < 1)
      dstRect = new Rectangle(this.BoundingRectangle.X, this.BoundingRectangle.Y, 2, 2);
    if (dstRect.X < 0 && dstRect.Y < 0)
      dstRect = new Rectangle(0, 0, this.BoundingRectangle.Width, this.BoundingRectangle.Height);
    else if (dstRect.X > 0 && dstRect.Y > 0)
      dstRect = new Rectangle(dstRect.X, dstRect.Y, this.BoundingRectangle.Width - this.BoundingRectangle.X, this.BoundingRectangle.Height - this.BoundingRectangle.Y);
    if (this.TilingType == 3)
      return (Brush) new TextureBrush(this.EmbeddedImage, dstRect)
      {
        Transform = new Matrix(this.TilingPatternMatrix.Elements[0], this.TilingPatternMatrix.Elements[1], this.TilingPatternMatrix.Elements[2], this.TilingPatternMatrix.Elements[3], 0.0f, 0.0f)
      };
    return (Brush) new TextureBrush(this.EmbeddedImage, dstRect)
    {
      Transform = new Matrix(this.TilingPatternMatrix.Elements[0], this.TilingPatternMatrix.Elements[1], this.TilingPatternMatrix.Elements[2], this.TilingPatternMatrix.Elements[3], this.TilingPatternMatrix.Elements[4], this.TilingPatternMatrix.Elements[5])
    };
  }

  private PdfDictionary GetResource(PdfDictionary dictionary)
  {
    if (dictionary.Items[new PdfName("Resources")] is PdfDictionary resource)
      return resource;
    return (dictionary.Items[new PdfName("Resources")] as PdfReferenceHolder).Object is PdfDictionary pdfDictionary ? pdfDictionary : (PdfDictionary) null;
  }
}
