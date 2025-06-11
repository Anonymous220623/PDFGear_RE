// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfEllipseAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfEllipseAnnotation : PdfCircleAnnotation
{
  public PdfEllipseAnnotation(RectangleF rectangle, string text)
    : base(rectangle)
  {
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Circle"));
    this.Text = text;
  }

  public new PdfPopupAnnotationCollection ReviewHistory
  {
    get
    {
      return this.m_reviewHistory != null ? this.m_reviewHistory : (this.m_reviewHistory = new PdfPopupAnnotationCollection((PdfAnnotation) this, true));
    }
  }

  public new PdfPopupAnnotationCollection Comments
  {
    get
    {
      return this.m_comments != null ? this.m_comments : (this.m_comments = new PdfPopupAnnotationCollection((PdfAnnotation) this, false));
    }
  }

  protected override void Initialize() => base.Initialize();

  protected override void Save() => base.Save();
}
