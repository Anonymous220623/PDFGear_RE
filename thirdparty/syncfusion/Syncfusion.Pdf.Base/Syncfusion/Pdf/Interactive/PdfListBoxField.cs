// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfListBoxField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfListBoxField(PdfPageBase page, string name) : PdfListField(page, name)
{
  private bool m_multiselect;

  public new bool ComplexScript
  {
    get => base.ComplexScript;
    set => base.ComplexScript = value;
  }

  public bool MultiSelect
  {
    get => this.m_multiselect;
    set
    {
      if (this.m_multiselect == value)
        return;
      this.m_multiselect = value;
      if (this.m_multiselect)
        this.Flags |= FieldFlags.MultiSelect;
      else
        this.Flags -= FieldFlags.MultiSelect;
    }
  }

  internal override void Draw()
  {
    base.Draw();
    if (this.Widget.ObtainAppearance() != null)
    {
      this.Page.Graphics.DrawPdfTemplate(this.Appearance.Normal, this.Location);
    }
    else
    {
      RectangleF bounds = this.Bounds with
      {
        Location = PointF.Empty
      };
      PdfFont font = this.Font ?? PdfDocument.DefaultFont;
      PaintParams paintParams = new PaintParams(bounds, this.BackBrush, this.ForeBrush, this.BorderPen, this.BorderStyle, this.BorderWidth, this.ShadowBrush, this.RotationAngle);
      PdfTemplate template = new PdfTemplate(bounds.Size);
      if (this.SelectedIndexes.Length > 1)
        FieldPainter.DrawListBox(template.Graphics, paintParams, this.Items, this.SelectedIndexes, font, this.StringFormat);
      else
        FieldPainter.DrawListBox(template.Graphics, paintParams, this.Items, new int[1]
        {
          this.SelectedIndex
        }, font, this.StringFormat);
      this.Page.Graphics.DrawPdfTemplate(template, this.Bounds.Location, bounds.Size);
    }
  }

  protected override void Initialize() => base.Initialize();

  protected override void DrawAppearance(PdfTemplate template)
  {
    base.DrawAppearance(template);
    PaintParams paintParams = new PaintParams(new RectangleF(PointF.Empty, this.Size), this.BackBrush, this.ForeBrush, this.BorderPen, this.BorderStyle, this.BorderWidth, this.ShadowBrush, this.RotationAngle);
    if (!this.m_isBCSet)
      paintParams.BackBrush = (PdfBrush) new PdfSolidBrush(PdfColor.Empty);
    PdfFont font = this.Font ?? (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, 12f);
    template.Graphics.StreamWriter.Clear();
    template.Graphics.StreamWriter.BeginMarkupSequence("Tx");
    template.Graphics.InitializeCoordinates();
    if (this.SelectedIndexes.Length > 1)
      FieldPainter.DrawListBox(template.Graphics, paintParams, this.Items, this.SelectedIndexes, font, this.StringFormat);
    else
      FieldPainter.DrawListBox(template.Graphics, paintParams, this.Items, new int[1]
      {
        this.SelectedIndex
      }, font, this.StringFormat);
    template.Graphics.StreamWriter.EndMarkupSequence();
  }
}
