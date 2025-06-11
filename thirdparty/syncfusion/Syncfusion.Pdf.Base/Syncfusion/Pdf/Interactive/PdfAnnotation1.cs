// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfAnnotation1
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public abstract class PdfAnnotation1 : IPdfWrapper
{
  private PdfColor m_color = PdfColor.Empty;
  private PdfAnnotationBorder m_border;
  private RectangleF m_rectangle = RectangleF.Empty;
  private PdfPage m_page;
  private string m_text = string.Empty;
  private PdfAnnotationFlags m_annotationFlags;
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfAnnotationCollection m_annotations;

  public PdfColor Color
  {
    get => this.m_color;
    set
    {
      if (!(this.m_color != value))
        return;
      this.m_color = value;
      PdfColorSpace colorSpace = PdfColorSpace.RGB;
      if (this.Page != null)
        colorSpace = this.Page.Section.Parent.Document.ColorSpace;
      this.m_dictionary.SetProperty("C", (IPdfPrimitive) this.m_color.ToArray(colorSpace));
    }
  }

  public PdfAnnotationBorder Border
  {
    get
    {
      if (this.m_border == null)
        this.m_border = new PdfAnnotationBorder();
      return this.m_border;
    }
    set => this.m_border = value;
  }

  public RectangleF Bounds
  {
    get => this.m_rectangle;
    set
    {
      if (!(this.m_rectangle != value))
        return;
      this.m_rectangle = value;
    }
  }

  public PointF Location
  {
    get => this.m_rectangle.Location;
    set => this.m_rectangle.Location = value;
  }

  public SizeF Size
  {
    get => this.m_rectangle.Size;
    set => this.m_rectangle.Size = value;
  }

  public PdfPage Page => this.m_page;

  public string Text
  {
    get => this.m_text;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (Text));
      if (!(this.m_text != value))
        return;
      this.m_text = value;
      this.m_dictionary.SetString("Contents", this.m_text);
    }
  }

  public PdfAnnotationFlags AnnotationFlags
  {
    get => this.m_annotationFlags;
    set
    {
      if (this.m_annotationFlags == value)
        return;
      this.m_annotationFlags = value;
      this.m_dictionary.SetNumber("F", (int) this.m_annotationFlags);
    }
  }

  internal PdfDictionary Dictionary => this.m_dictionary;

  protected PdfAnnotation1()
  {
    if (this.Dictionary.ContainsKey("Annots"))
      this.m_annotations.Annotations = this.Dictionary["Annots"] as PdfArray;
    this.Initialize();
  }

  protected PdfAnnotation1(RectangleF bounds)
  {
    this.Initialize();
    this.Bounds = bounds;
  }

  internal void SetPage(PdfPage page)
  {
    this.m_page = page;
    if (this.m_page == null)
      this.m_dictionary.Remove(new PdfName("P"));
    else
      this.m_dictionary.SetProperty("P", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_page));
  }

  internal void AssignLocation(PointF location) => this.m_rectangle.Location = location;

  internal void AssignSize(SizeF size) => this.m_rectangle.Size = size;

  protected virtual void Initialize()
  {
    this.m_dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    this.m_dictionary.SetProperty("Type", (IPdfPrimitive) new PdfName("Annot"));
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars) => this.Save();

  protected virtual void Save()
  {
    if ((this.GetType().ToString().Contains("Pdf3DAnnotation") || this.GetType().ToString().Contains("PdfAttachmentAnnotation") || this.GetType().ToString().Contains("PdfSoundAnnotation") || this.GetType().ToString().Contains("PdfActionAnnotation")) && PdfDocument.ConformanceLevel != PdfConformanceLevel.None)
      throw new PdfConformanceException("The specified annotation type is not supported by PDF/A1-B standard document.");
    if (this.m_border != null)
      this.m_dictionary.SetProperty("Border", (IPdfWrapper) this.m_border);
    RectangleF rectangle = new RectangleF(this.m_rectangle.X, this.m_rectangle.Bottom, this.m_rectangle.Width, this.m_rectangle.Height);
    if (this.m_page != null)
    {
      PdfSection section = this.m_page.Section;
      rectangle.Location = section.PointToNativePdf(this.Page, rectangle.Location);
    }
    this.m_dictionary.SetProperty("Rect", (IPdfPrimitive) PdfArray.FromRectangle(rectangle));
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
