// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfTilingBrush
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System.Collections;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfTilingBrush : PdfBrush, IPdfWrapper
{
  private RectangleF m_box;
  private PdfGraphics m_graphics;
  private PdfStream m_brushStream;
  private PdfResources m_resources;
  private bool m_bStroking;
  private PdfPage m_page;
  private PointF m_location;
  private PdfTransformationMatrix m_transformationMatrix;
  internal bool isXPSBrush;

  internal PointF Location
  {
    get => this.m_location;
    set => this.m_location = value;
  }

  internal PdfTransformationMatrix TransformationMatrix
  {
    get => this.m_transformationMatrix;
    set => this.m_transformationMatrix = value;
  }

  public PdfTilingBrush(RectangleF rectangle)
  {
    this.m_brushStream = new PdfStream();
    this.m_resources = new PdfResources();
    this.m_brushStream[nameof (Resources)] = (IPdfPrimitive) this.m_resources;
    this.SetBox(rectangle);
    this.SetObligatoryFields();
  }

  public PdfTilingBrush(RectangleF rectangle, PdfPage page)
  {
    this.m_page = page;
    this.m_brushStream = new PdfStream();
    this.m_resources = new PdfResources();
    this.m_brushStream[nameof (Resources)] = (IPdfPrimitive) this.m_resources;
    this.SetBox(rectangle);
    this.SetObligatoryFields();
    this.Graphics.ColorSpace = page.Document.ColorSpace;
  }

  public PdfTilingBrush(SizeF size)
    : this(new RectangleF(PointF.Empty, size))
  {
  }

  public PdfTilingBrush(SizeF size, PdfPage page)
    : this(new RectangleF(PointF.Empty, size), page)
  {
  }

  internal PdfTilingBrush(
    RectangleF rectangle,
    PdfPage page,
    PointF location,
    PdfTransformationMatrix matrix)
  {
    this.m_page = page;
    this.m_location = location;
    this.m_transformationMatrix = matrix;
    this.m_brushStream = new PdfStream();
    this.m_resources = new PdfResources();
    this.m_brushStream[nameof (Resources)] = (IPdfPrimitive) this.m_resources;
    this.SetBox(rectangle);
    this.SetObligatoryFields();
  }

  private void SetObligatoryFields()
  {
    this.m_brushStream["PatternType"] = (IPdfPrimitive) new PdfNumber(1);
    this.m_brushStream["PaintType"] = (IPdfPrimitive) new PdfNumber(1);
    this.m_brushStream["TilingType"] = (IPdfPrimitive) new PdfNumber(1);
    this.m_brushStream["XStep"] = (IPdfPrimitive) new PdfNumber(this.m_box.Right - this.m_box.Left);
    this.m_brushStream["YStep"] = (IPdfPrimitive) new PdfNumber(this.m_box.Bottom - this.m_box.Top);
    if (this.m_page == null)
      return;
    if (this.m_transformationMatrix == null)
    {
      this.m_brushStream["Matrix"] = (IPdfPrimitive) new PdfArray(new float[6]
      {
        1f,
        0.0f,
        0.0f,
        1f,
        this.m_location.X,
        this.m_page.Size.Height % this.Rectangle.Size.Height - this.Location.Y
      });
    }
    else
    {
      float[] elements = this.m_transformationMatrix.Matrix.Elements;
      float num = (double) this.m_page.Size.Height <= (double) this.Rectangle.Size.Height ? this.m_page.Size.Height % this.Rectangle.Size.Height + this.m_transformationMatrix.OffsetY : this.m_transformationMatrix.OffsetY - this.m_page.Size.Height % this.Rectangle.Size.Height;
      this.m_brushStream["Matrix"] = (IPdfPrimitive) new PdfArray(new float[6]
      {
        elements[0],
        elements[1],
        elements[2],
        elements[3],
        elements[4],
        num
      });
    }
  }

  private void SetBox(RectangleF box)
  {
    this.m_box = box;
    this.m_brushStream["BBox"] = (IPdfPrimitive) PdfArray.FromRectangle(this.m_box);
  }

  public RectangleF Rectangle => this.m_box;

  public SizeF Size => this.m_box.Size;

  public PdfGraphics Graphics
  {
    get
    {
      if (this.m_graphics == null)
      {
        this.m_graphics = new PdfGraphics(this.Size, new PdfGraphics.GetResources(this.ObtainResources), this.m_brushStream);
        this.m_graphics.InitializeCoordinates();
      }
      return this.m_graphics;
    }
  }

  internal PdfResources Resources => this.m_resources;

  internal bool Stroking
  {
    get => this.m_bStroking;
    set => this.m_bStroking = value;
  }

  private PdfResources ObtainResources() => this.Resources;

  public override PdfBrush Clone()
  {
    PdfTilingBrush pdfTilingBrush = new PdfTilingBrush(this.Rectangle, this.m_page, this.Location, this.m_transformationMatrix);
    if (this.isXPSBrush && this.m_transformationMatrix != null && this.m_transformationMatrix.Matrix != null)
      pdfTilingBrush.m_brushStream["Matrix"] = (IPdfPrimitive) new PdfArray(this.m_transformationMatrix.Matrix.Elements);
    pdfTilingBrush.m_brushStream.Data = this.m_brushStream.Data;
    pdfTilingBrush.m_resources = new PdfResources((PdfDictionary) this.m_resources);
    pdfTilingBrush.m_brushStream["Resources"] = (IPdfPrimitive) pdfTilingBrush.m_resources;
    return (PdfBrush) pdfTilingBrush;
  }

  internal override bool MonitorChanges(
    PdfBrush brush,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveChanges,
    PdfColorSpace currentColorSpace)
  {
    bool flag = false;
    if (brush != this)
    {
      streamWriter.SetColorSpace("Pattern", this.m_bStroking);
      PdfName name = getResources().GetName((IPdfWrapper) this);
      streamWriter.SetColourWithPattern((IList) null, name, this.m_bStroking);
      flag = true;
    }
    return flag;
  }

  internal override bool MonitorChanges(
    PdfBrush brush,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveChanges,
    PdfColorSpace currentColorSpace,
    bool check)
  {
    bool flag = false;
    if (brush != this)
    {
      streamWriter.SetColorSpace("Pattern", this.m_bStroking);
      PdfName name = getResources().GetName((IPdfWrapper) this);
      streamWriter.SetColourWithPattern((IList) null, name, this.m_bStroking);
      flag = true;
    }
    return flag;
  }

  internal override bool MonitorChanges(
    PdfBrush brush,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveChanges,
    PdfColorSpace currentColorSpace,
    bool check,
    bool iccbased)
  {
    bool flag = false;
    if (brush != this)
    {
      streamWriter.SetColorSpace("Pattern", this.m_bStroking);
      PdfName name = getResources().GetName((IPdfWrapper) this);
      streamWriter.SetColourWithPattern((IList) null, name, this.m_bStroking);
      flag = true;
    }
    return flag;
  }

  internal override bool MonitorChanges(
    PdfBrush brush,
    PdfStreamWriter streamWriter,
    PdfGraphics.GetResources getResources,
    bool saveChanges,
    PdfColorSpace currentColorSpace,
    bool check,
    bool iccbased,
    bool indexed)
  {
    bool flag = false;
    if (brush != this)
    {
      streamWriter.SetColorSpace("Pattern", this.m_bStroking);
      PdfName name = getResources().GetName((IPdfWrapper) this);
      streamWriter.SetColourWithPattern((IList) null, name, this.m_bStroking);
      flag = true;
    }
    return flag;
  }

  internal override void ResetChanges(PdfStreamWriter streamWriter)
  {
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_brushStream;
}
