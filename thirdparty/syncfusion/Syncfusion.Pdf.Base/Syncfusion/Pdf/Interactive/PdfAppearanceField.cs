// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfAppearanceField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public abstract class PdfAppearanceField : PdfStyledField
{
  protected PdfAppearanceField()
  {
  }

  protected PdfAppearanceField(PdfPageBase page, string name)
    : base(page, name)
  {
  }

  public PdfAppearance Appearance => this.Widget.Appearance;

  internal override void Save()
  {
    base.Save();
    if (this.Page != null && this.Page.FormFieldsTabOrder == PdfFormFieldsTabOrder.Manual && this.Page is PdfPage)
    {
      PdfPage page = this.Page as PdfPage;
      PdfStyledField pdfStyledField = (PdfStyledField) this;
      if (pdfStyledField != null)
      {
        page.Annotations.Remove((PdfAnnotation) pdfStyledField.Widget);
        page.Annotations.Insert(this.TabIndex, (PdfAnnotation) pdfStyledField.Widget);
      }
    }
    if (this.Form == null || this.Form.NeedAppearances || this.Widget.ObtainAppearance() != null)
      return;
    this.DrawAppearance(this.Widget.Appearance.Normal);
  }

  internal override void Draw() => base.Draw();

  protected virtual void DrawAppearance(PdfTemplate template)
  {
  }
}
