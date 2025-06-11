// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfCheckBoxField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfCheckBoxField(PdfPageBase page, string name) : PdfCheckFieldBase(page, name)
{
  private bool m_checked;

  public bool Checked
  {
    get => this.m_checked;
    set
    {
      if (this.m_checked == value)
        return;
      this.m_checked = value;
      if (this.m_checked)
        this.Dictionary.SetName("V", "Yes");
      else
        this.Dictionary.Remove("V");
    }
  }

  internal override void Save()
  {
    base.Save();
    if (this.Form == null && !this.isXfa)
      return;
    if (!this.Checked)
      this.Widget.AppearanceState = "Off";
    else
      this.Widget.AppearanceState = "Yes";
  }

  internal override void Draw()
  {
    base.Draw();
    PaintParams paintParams = new PaintParams(this.Bounds, this.BackBrush, this.ForeBrush, this.BorderPen, this.BorderStyle, this.BorderWidth, this.ShadowBrush, this.RotationAngle);
    PdfCheckFieldState state = PdfCheckFieldState.Checked;
    if (!this.Checked)
      state = PdfCheckFieldState.Unchecked;
    FieldPainter.DrawCheckBox(this.Page.Graphics, paintParams, this.StyleToString(this.Style), state);
  }

  protected override void DrawAppearance()
  {
    base.DrawAppearance();
    PaintParams paintParams = new PaintParams(new RectangleF(PointF.Empty, this.Size), this.BackBrush, this.ForeBrush, this.BorderPen, this.BorderStyle, this.BorderWidth, this.ShadowBrush, this.RotationAngle);
    FieldPainter.DrawCheckBox(this.Widget.ExtendedAppearance.Normal.On.Graphics, paintParams, this.StyleToString(this.Style), PdfCheckFieldState.Checked, this.Font);
    FieldPainter.DrawCheckBox(this.Widget.ExtendedAppearance.Normal.Off.Graphics, paintParams, this.StyleToString(this.Style), PdfCheckFieldState.Unchecked, this.Font);
    FieldPainter.DrawCheckBox(this.Widget.ExtendedAppearance.Pressed.On.Graphics, paintParams, this.StyleToString(this.Style), PdfCheckFieldState.PressedChecked, this.Font);
    FieldPainter.DrawCheckBox(this.Widget.ExtendedAppearance.Pressed.Off.Graphics, paintParams, this.StyleToString(this.Style), PdfCheckFieldState.PressedUnchecked, this.Font);
  }
}
