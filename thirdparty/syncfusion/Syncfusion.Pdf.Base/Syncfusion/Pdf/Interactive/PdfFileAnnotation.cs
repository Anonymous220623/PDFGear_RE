// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfFileAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public abstract class PdfFileAnnotation : PdfAnnotation
{
  private PdfAppearance m_appearance;

  public abstract string FileName { get; set; }

  public new PdfAppearance Appearance
  {
    get
    {
      if (this.m_appearance == null)
        this.m_appearance = new PdfAppearance((PdfAnnotation) this);
      return this.m_appearance;
    }
    set
    {
      if (this.m_appearance == value)
        return;
      this.m_appearance = value;
    }
  }

  protected PdfFileAnnotation()
  {
  }

  protected PdfFileAnnotation(RectangleF rectangle)
    : base(rectangle)
  {
  }

  protected override void Save()
  {
    base.Save();
    if (this.m_appearance == null || this.m_appearance.Normal == null)
      return;
    this.Dictionary.SetProperty("AP", (IPdfWrapper) this.m_appearance);
  }
}
