// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfCheckFieldBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfCheckFieldBase : PdfStyledField
{
  private PdfCheckBoxStyle m_style;
  private PdfTemplate m_checkedTemplate;
  private PdfTemplate m_uncheckedTemplate;
  private PdfTemplate m_pressedCheckedTemplate;
  private PdfTemplate m_pressedUncheckedTemplate;

  public PdfCheckFieldBase(PdfPageBase page, string name)
    : base(page, name)
  {
  }

  internal PdfCheckFieldBase()
  {
  }

  public PdfCheckBoxStyle Style
  {
    get => this.m_style;
    set
    {
      if (this.m_style == value)
        return;
      this.m_style = value;
      this.Widget.WidgetAppearance.NormalCaption = this.StyleToString(this.m_style);
    }
  }

  internal PdfTemplate CheckedTemplate
  {
    get => this.m_checkedTemplate;
    set => this.m_checkedTemplate = value;
  }

  internal PdfTemplate UncheckedTemplate
  {
    get => this.m_uncheckedTemplate;
    set => this.m_uncheckedTemplate = value;
  }

  internal PdfTemplate PressedCheckedTemplate
  {
    get => this.m_pressedCheckedTemplate;
    set => this.m_pressedCheckedTemplate = value;
  }

  internal PdfTemplate PressedUncheckedTemplate
  {
    get => this.m_pressedUncheckedTemplate;
    set => this.m_pressedUncheckedTemplate = value;
  }

  protected string StyleToString(PdfCheckBoxStyle style)
  {
    switch (style)
    {
      case PdfCheckBoxStyle.Circle:
        return "l";
      case PdfCheckBoxStyle.Cross:
        return "8";
      case PdfCheckBoxStyle.Diamond:
        return "u";
      case PdfCheckBoxStyle.Square:
        return "n";
      case PdfCheckBoxStyle.Star:
        return "H";
      default:
        return "4";
    }
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Dictionary.SetProperty("FT", (IPdfPrimitive) new PdfName("Btn"));
  }

  internal override void Save()
  {
    base.Save();
    if (this.Form != null || this.isXfa)
    {
      this.CreateTemplate(ref this.m_checkedTemplate);
      this.CreateTemplate(ref this.m_uncheckedTemplate);
      this.CreateTemplate(ref this.m_pressedCheckedTemplate);
      this.CreateTemplate(ref this.m_pressedUncheckedTemplate);
      this.Widget.ExtendedAppearance.Normal.On = this.m_checkedTemplate;
      this.Widget.ExtendedAppearance.Normal.Off = this.m_uncheckedTemplate;
      this.Widget.ExtendedAppearance.Pressed.On = this.m_pressedCheckedTemplate;
      this.Widget.ExtendedAppearance.Pressed.Off = this.m_pressedUncheckedTemplate;
      this.DrawAppearance();
    }
    else
    {
      this.ReleaseTemplate(this.m_checkedTemplate);
      this.ReleaseTemplate(this.m_uncheckedTemplate);
      this.ReleaseTemplate(this.m_pressedCheckedTemplate);
      this.ReleaseTemplate(this.m_pressedUncheckedTemplate);
    }
  }

  protected virtual void DrawAppearance()
  {
  }

  private void CreateTemplate(ref PdfTemplate template)
  {
    if (template == null)
      template = new PdfTemplate(this.Size);
    else
      template.Reset(this.Size);
  }

  private void ReleaseTemplate(PdfTemplate template)
  {
    if (template == null)
      return;
    template.Reset();
    this.Widget.ExtendedAppearance = (PdfExtendedAppearance) null;
  }

  internal override void Draw() => base.Draw();
}
