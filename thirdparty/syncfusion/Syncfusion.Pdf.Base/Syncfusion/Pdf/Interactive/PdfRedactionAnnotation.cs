// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfRedactionAnnotation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using Syncfusion.Pdf.Redaction;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfRedactionAnnotation : PdfAnnotation
{
  private PdfColor borderColor;
  private PdfColor textColor;
  private PdfFont font;
  private PdfTextAlignment alignment;
  private LineBorder border = new LineBorder();
  private string overlayText;
  private bool repeat;
  private bool flatten;
  private float m_borderWidth;

  public PdfColor TextColor
  {
    get => this.textColor;
    set
    {
      this.textColor = value;
      this.Dictionary.SetProperty("C", (IPdfPrimitive) this.textColor.ToArray());
    }
  }

  public PdfTextAlignment TextAlignment
  {
    get => this.alignment;
    set
    {
      if (this.alignment == value)
        return;
      this.alignment = value;
      this.Dictionary.SetProperty("Q", (IPdfPrimitive) new PdfNumber((int) this.alignment));
    }
  }

  public string OverlayText
  {
    get => this.overlayText;
    set
    {
      this.overlayText = value;
      this.Dictionary.SetString(nameof (OverlayText), this.overlayText);
    }
  }

  public PdfFont Font
  {
    get => this.font;
    set => this.font = value;
  }

  public PdfColor BorderColor
  {
    get => this.borderColor;
    set
    {
      this.borderColor = value;
      this.Dictionary.SetProperty("OC", (IPdfPrimitive) this.borderColor.ToArray());
    }
  }

  public LineBorder Border
  {
    get => this.border;
    set => this.border = value;
  }

  public bool RepeatText
  {
    get => this.repeat;
    set
    {
      this.repeat = value;
      this.Dictionary.SetBoolean("Repeat", this.repeat);
    }
  }

  public new bool Flatten
  {
    get => this.flatten;
    set => this.flatten = value;
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName("Redact"));
  }

  protected override void Save()
  {
    base.Save();
    this.CheckFlatten();
    if (!this.Flatten && !this.SetAppearanceDictionary)
      return;
    PdfTemplate normalAppearance = this.CreateNormalAppearance(this.OverlayText, this.Font, this.RepeatText, this.TextColor, this.TextAlignment, this.Border);
    if (this.Flatten)
    {
      if (this.LoadedPage == null)
        return;
      this.RemoveAnnoationFromPage((PdfPageBase) this.LoadedPage, (PdfAnnotation) this);
    }
    else
    {
      this.Appearance.Normal = this.CreateBorderAppearance(this.BorderColor, this.Border);
      this.Appearance.MouseHover = normalAppearance;
      this.Dictionary.SetProperty("AP", (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.Appearance));
    }
  }

  internal void ApplyRedaction(PdfLoadedPage page)
  {
    PdfTemplate normalAppearance = this.CreateNormalAppearance(this.OverlayText, this.Font, this.RepeatText, this.TextColor, this.TextAlignment, this.Border);
    page.Redactions.Add(new PdfRedaction(this.Bounds)
    {
      m_success = true,
      Appearance = normalAppearance
    });
  }
}
