// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfTemplate
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public class PdfTemplate : PdfShapeElement, IPdfWrapper
{
  private PdfGraphics m_graphics;
  internal PdfStream m_content;
  internal PdfResources m_resources;
  private SizeF m_size;
  private bool m_bIsReadonly;
  internal bool m_writeTransformation = true;
  internal bool isLoadedPageTemplate;
  private string m_customPdfTemplateName;
  private bool m_isSignatureAppearanceValidation;
  private bool m_isAnnotationTemplate;
  private bool m_isScaleAnnotation;

  public PdfTemplate(SizeF size)
    : this(size.Width, size.Height)
  {
  }

  public PdfTemplate(RectangleF rect)
    : this(rect.X, rect.Y, rect.Width, rect.Height)
  {
  }

  internal PdfTemplate(SizeF size, bool writeTransformation)
    : this(size.Width, size.Height)
  {
    this.m_writeTransformation = writeTransformation;
  }

  public PdfTemplate(float width, float height)
  {
    this.m_content = new PdfStream();
    this.SetSize(new SizeF(width, height));
    this.Initialize();
  }

  public PdfTemplate(float x, float y, float width, float height)
  {
    this.m_content = new PdfStream();
    this.SetBounds(new RectangleF(x, y, width, height));
    this.Initialize();
  }

  internal PdfTemplate(
    PointF origin,
    SizeF size,
    MemoryStream stream,
    PdfDictionary resources,
    bool isLoadedPage,
    PdfPageBase page)
  {
    if (size == SizeF.Empty)
      throw new ArgumentException("The size of the new PdfTemplate can't be empty.");
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.m_content = new PdfStream();
    PdfLoadedPage pdfLoadedPage = (PdfLoadedPage) null;
    if (isLoadedPage)
      pdfLoadedPage = page as PdfLoadedPage;
    if (pdfLoadedPage != null && ((double) pdfLoadedPage.CropBox.X > 0.0 || (double) pdfLoadedPage.CropBox.Y > 0.0))
    {
      RectangleF cropBox = pdfLoadedPage.GetCropBox();
      this.m_content["BBox"] = (IPdfPrimitive) PdfArray.FromRectangle(cropBox);
      this.SetSize(cropBox.Size);
    }
    else if ((double) origin.X < 0.0 || (double) origin.Y < 0.0)
      this.SetSize(origin, size);
    else
      this.SetSize(size);
    this.Initialize();
    stream.WriteTo((Stream) this.m_content.InternalStream);
    if (resources != null)
    {
      this.m_content["Resources"] = (IPdfPrimitive) new PdfDictionary(resources);
      this.m_resources = new PdfResources(resources);
    }
    this.isLoadedPageTemplate = isLoadedPage;
    this.m_bIsReadonly = true;
  }

  internal PdfTemplate(SizeF size, MemoryStream stream, PdfDictionary resources)
  {
    if (size == SizeF.Empty)
      throw new ArgumentException("The size of the new PdfTemplate can't be empty.");
    if (stream == null)
      throw new ArgumentNullException(nameof (stream));
    this.m_content = new PdfStream();
    this.SetSize(size);
    this.Initialize();
    stream.WriteTo((Stream) this.m_content.InternalStream);
    if (resources != null)
    {
      this.m_content["Resources"] = (IPdfPrimitive) new PdfDictionary(resources);
      this.m_resources = new PdfResources(resources);
    }
    this.m_bIsReadonly = true;
  }

  internal PdfTemplate(PdfStream template)
  {
    this.m_content = template != null ? template : throw new ArgumentNullException(nameof (template));
    this.m_size = (PdfCrossTable.Dereference(this.m_content["BBox"]) as PdfArray).ToRectangle().Size;
    this.m_bIsReadonly = true;
  }

  internal PdfTemplate(PdfStream template, bool isTransformBBox)
  {
    this.m_content = template != null ? template : throw new ArgumentNullException("template not to be null value");
    RectangleF bBoxValue = RectangleF.Empty;
    if (PdfCrossTable.Dereference(template["BBox"]) is PdfArray pdfArray1)
      bBoxValue = pdfArray1.ToRectangle();
    if (isTransformBBox)
    {
      if (PdfCrossTable.Dereference(template["Matrix"]) is PdfArray pdfArray2)
      {
        float[] matrix = new float[pdfArray2.Count];
        for (int index = 0; index < pdfArray2.Count; ++index)
        {
          if (pdfArray2.Elements[index] is PdfNumber element)
            matrix[index] = element.FloatValue;
        }
        this.m_size = this.TransformBBoxByMatrix(bBoxValue, matrix).Size;
      }
      else
        this.m_size = bBoxValue.Size;
    }
    else
      this.m_size = bBoxValue.Size;
    this.m_bIsReadonly = true;
  }

  private PdfTemplate() => this.m_content = new PdfStream();

  public PdfGraphics Graphics
  {
    get
    {
      if (this.m_bIsReadonly)
        this.m_graphics = (PdfGraphics) null;
      else if (this.m_graphics == null)
      {
        this.m_graphics = new PdfGraphics(this.Size, new PdfGraphics.GetResources(this.GetResources), this.m_content);
        if (this.m_writeTransformation)
          this.m_graphics.InitializeCoordinates();
        if (this.PdfTag != null)
          this.m_graphics.Tag = this.PdfTag;
        this.m_graphics.IsTemplateGraphics = true;
      }
      return this.m_graphics;
    }
  }

  public SizeF Size => this.m_size;

  public float Width => this.Size.Width;

  public float Height => this.Size.Height;

  public bool ReadOnly => this.m_bIsReadonly;

  internal string CustomPdfTemplateName
  {
    set
    {
      if (value == null)
        return;
      this.m_customPdfTemplateName = value;
    }
    get => this.m_customPdfTemplateName;
  }

  internal bool IsSignatureAppearanceValidation
  {
    get => this.m_isSignatureAppearanceValidation;
    set => this.m_isSignatureAppearanceValidation = value;
  }

  public void Reset(SizeF size)
  {
    this.SetSize(size);
    this.Reset();
  }

  public void Reset()
  {
    if (this.m_resources != null)
    {
      this.m_resources = (PdfResources) null;
      this.m_content.Remove("Resources");
    }
    if (this.m_graphics == null)
      return;
    this.m_graphics.Reset(this.Size);
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_content;

  protected override RectangleF GetBoundsInternal() => new RectangleF(PointF.Empty, this.Size);

  protected override void DrawInternal(PdfGraphics graphics)
  {
    if (graphics == null)
      throw new ArgumentNullException(nameof (graphics));
    graphics.DrawPdfTemplate(this, PointF.Empty);
  }

  private void Initialize()
  {
    this.AddType();
    this.AddSubType();
  }

  private PdfResources GetResources()
  {
    if (this.m_resources == null)
    {
      this.m_resources = new PdfResources();
      this.m_content["Resources"] = (IPdfPrimitive) this.m_resources;
    }
    return this.m_resources;
  }

  private void AddType()
  {
    this.m_content["Type"] = (IPdfPrimitive) this.m_content.GetName("XObject");
  }

  private void AddSubType()
  {
    this.m_content["Subtype"] = (IPdfPrimitive) this.m_content.GetName("Form");
  }

  private void SetSize(SizeF size)
  {
    this.m_content["BBox"] = (IPdfPrimitive) PdfArray.FromRectangle(new RectangleF(PointF.Empty, size));
    this.m_size = size;
  }

  private void SetBounds(RectangleF bounds)
  {
    this.m_content["BBox"] = (IPdfPrimitive) PdfArray.FromRectangle(new RectangleF(bounds.X, bounds.Y, bounds.Width, bounds.Height));
    this.m_size = bounds.Size;
  }

  private void SetSize(PointF origin, SizeF size)
  {
    this.m_content["BBox"] = (IPdfPrimitive) new PdfArray(new float[4]
    {
      origin.X,
      origin.Y,
      size.Width,
      size.Height
    });
    this.m_size = size;
  }

  internal void CloneResources(PdfCrossTable crossTable)
  {
    if (this.m_resources == null || crossTable == null)
      return;
    List<PdfReference> prevReference = crossTable.PrevReference;
    crossTable.PrevReference = new List<PdfReference>();
    PdfDictionary baseDictionary = this.m_resources.Clone(crossTable) as PdfDictionary;
    crossTable.PrevReference.AddRange((IEnumerable<PdfReference>) prevReference);
    this.m_resources = new PdfResources(baseDictionary);
    this.m_content["Resources"] = (IPdfPrimitive) baseDictionary;
  }

  internal void Clear(string Data)
  {
    this.m_content.Clear();
    this.m_content.Write(Data);
  }

  private RectangleF TransformBBoxByMatrix(RectangleF bBoxValue, float[] matrix)
  {
    float[] numArray1 = new float[4];
    float[] numArray2 = new float[4];
    PointF pointF1 = this.TransformPoint(bBoxValue.Left, bBoxValue.Bottom, matrix);
    numArray1[0] = pointF1.X;
    numArray2[0] = pointF1.Y;
    PointF pointF2 = this.TransformPoint(bBoxValue.Right, bBoxValue.Top, matrix);
    numArray1[1] = pointF2.X;
    numArray2[1] = pointF2.Y;
    PointF pointF3 = this.TransformPoint(bBoxValue.Left, bBoxValue.Top, matrix);
    numArray1[2] = pointF3.X;
    numArray2[2] = pointF3.Y;
    PointF pointF4 = this.TransformPoint(bBoxValue.Right, bBoxValue.Bottom, matrix);
    numArray1[3] = pointF4.X;
    numArray2[3] = pointF4.Y;
    return new RectangleF(this.MinValue(numArray1), this.MinValue(numArray2), this.MaxValue(numArray1), this.MaxValue(numArray2));
  }

  private float MaxValue(float[] value)
  {
    float num = value[0];
    for (int index = 1; index < value.Length; ++index)
    {
      if ((double) value[index] > (double) num)
        num = value[index];
    }
    return num;
  }

  private float MinValue(float[] value)
  {
    float num = value[0];
    for (int index = 1; index < value.Length; ++index)
    {
      if ((double) value[index] < (double) num)
        num = value[index];
    }
    return num;
  }

  private PointF TransformPoint(float x, float y, float[] matrix)
  {
    return new PointF()
    {
      X = (float) ((double) matrix[0] * (double) x + (double) matrix[2] * (double) y) + matrix[4],
      Y = (float) ((double) matrix[1] * (double) x + (double) matrix[3] * (double) y) + matrix[5]
    };
  }

  internal bool IsAnnotationTemplate
  {
    get => this.m_isAnnotationTemplate;
    set => this.m_isAnnotationTemplate = value;
  }

  internal bool NeedScaling
  {
    get => this.m_isScaleAnnotation;
    set => this.m_isScaleAnnotation = value;
  }
}
