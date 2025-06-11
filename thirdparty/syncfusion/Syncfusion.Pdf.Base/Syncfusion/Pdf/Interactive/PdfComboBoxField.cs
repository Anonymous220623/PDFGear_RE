// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfComboBoxField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfComboBoxField : PdfListField
{
  private bool m_editable;

  public PdfComboBoxField(PdfPageBase page, string name)
    : base(page, name)
  {
  }

  internal PdfComboBoxField()
  {
  }

  public new bool ComplexScript
  {
    get => base.ComplexScript;
    set => base.ComplexScript = value;
  }

  public bool Editable
  {
    get => this.m_editable;
    set
    {
      if (this.m_editable == value)
        return;
      this.m_editable = value;
      if (this.m_editable)
        this.Flags |= FieldFlags.Edit;
      else
        this.Flags &= FieldFlags.Edit;
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
      RectangleF bounds1 = this.Bounds with
      {
        Location = PointF.Empty
      };
      PdfFont font = this.Font ?? PdfDocument.DefaultFont;
      PaintParams paintParams = new PaintParams(bounds1, this.BackBrush, this.ForeBrush, this.BorderPen, this.BorderStyle, this.BorderWidth, this.ShadowBrush, this.RotationAngle);
      PdfTemplate template = new PdfTemplate(bounds1.Size);
      string s = string.Empty;
      if (this.SelectedIndex != -1)
        s = this.SelectedItem.Text;
      FieldPainter.DrawComboBox(template.Graphics, paintParams);
      PointF empty = PointF.Empty;
      float borderWidth = paintParams.BorderWidth;
      float num1 = 2f * borderWidth;
      bool flag = paintParams.BorderStyle == PdfBorderStyle.Inset || paintParams.BorderStyle == PdfBorderStyle.Beveled;
      if (flag)
      {
        empty.X = 2f * num1;
        empty.Y = 2f * borderWidth;
      }
      else
      {
        empty.X = num1;
        empty.Y = 1f * borderWidth;
      }
      PdfBrush foreBrush = paintParams.ForeBrush;
      float num2 = paintParams.Bounds.Width - num1;
      RectangleF bounds2 = paintParams.Bounds;
      if (flag)
        bounds2.Height -= num1;
      else
        bounds2.Height -= borderWidth;
      RectangleF layoutRectangle = new RectangleF(empty.X, empty.Y, num2 - empty.X, bounds2.Height);
      template.Graphics.DrawString(s, font, this.ForeBrush, layoutRectangle, this.StringFormat);
      this.Page.Graphics.DrawPdfTemplate(template, this.Bounds.Location, bounds1.Size);
    }
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Flags |= FieldFlags.Combo;
  }

  protected override void DrawAppearance(PdfTemplate template)
  {
    base.DrawAppearance(template);
    PaintParams paintParams = new PaintParams(new RectangleF(PointF.Empty, this.Size), this.BackBrush, this.ForeBrush, this.BorderPen, this.BorderStyle, this.BorderWidth, this.ShadowBrush, this.RotationAngle);
    if (this.SelectedIndex != -1)
    {
      PdfFont font = this.Font == null ? (PdfFont) new PdfStandardFont(PdfFontFamily.TimesRoman, this.GetFontHeight(PdfFontFamily.Helvetica)) : this.Font;
      FieldPainter.DrawComboBox(template.Graphics, paintParams, this.SelectedValue, font, this.StringFormat);
    }
    else
      FieldPainter.DrawComboBox(template.Graphics, paintParams);
  }

  internal float GetFontHeight(PdfFontFamily family)
  {
    float size = 0.0f;
    if (this.SelectedIndex != -1)
    {
      float width = new PdfStandardFont(family, 12f).MeasureString(this.SelectedValue).Width;
      size = (double) width == 0.0 ? 12f : (float) (12.0 * ((double) this.Bounds.Size.Width - 4.0 * (double) this.BorderWidth)) / width;
      if (this.SelectedIndex != -1)
      {
        PdfFont pdfFont = (PdfFont) new PdfStandardFont(family, size);
        string selectedValue = this.SelectedValue;
        SizeF sizeF1 = pdfFont.MeasureString(selectedValue);
        if ((double) sizeF1.Width > (double) this.Bounds.Width || (double) sizeF1.Height > (double) this.Bounds.Height)
        {
          float num1 = this.Bounds.Width - 4f * this.BorderWidth;
          float num2 = this.Bounds.Height - 4f * this.BorderWidth;
          float num3 = 0.248f;
          for (float num4 = 1f; (double) num4 <= (double) this.Bounds.Height; ++num4)
          {
            pdfFont.Size = num4;
            SizeF sizeF2 = pdfFont.MeasureString(selectedValue);
            if ((double) sizeF2.Width > (double) this.Bounds.Width || (double) sizeF2.Height > (double) num2)
            {
              float num5 = num4;
              do
              {
                num5 -= 1f / 1000f;
                pdfFont.Size = num5;
                float lineWidth = pdfFont.GetLineWidth(selectedValue, this.StringFormat);
                if ((double) num5 < (double) num3)
                {
                  pdfFont.Size = num3;
                  break;
                }
                sizeF2 = pdfFont.MeasureString(selectedValue, this.StringFormat);
                if ((double) lineWidth < (double) num1 && (double) sizeF2.Height < (double) num2)
                {
                  pdfFont.Size = num5;
                  break;
                }
              }
              while ((double) num5 > (double) num3);
              size = num5;
              break;
            }
          }
        }
      }
      else if ((double) size > 12.0)
        size = 12f;
    }
    return size;
  }
}
