// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfTextBoxField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfTextBoxField : PdfAppearanceField
{
  private const string m_passwordValue = "*";
  private string m_text = string.Empty;
  private string m_defaultValue = string.Empty;
  private bool m_spellCheck;
  private bool m_insertSpaces;
  private bool m_multiline;
  private bool m_password;
  private bool m_scrollable = true;
  private int m_maxLength;
  private bool m_autoResizeText;

  public new bool ComplexScript
  {
    get => base.ComplexScript;
    set => base.ComplexScript = value;
  }

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
      this.Dictionary.SetString("V", this.m_text);
    }
  }

  public string DefaultValue
  {
    get => this.m_defaultValue;
    set
    {
      if (value == null)
        throw new ArgumentNullException(nameof (DefaultValue));
      if (!(this.m_defaultValue != value))
        return;
      this.m_defaultValue = value;
      this.Dictionary.SetString("DV", this.m_defaultValue);
    }
  }

  public bool SpellCheck
  {
    get => this.m_spellCheck;
    set
    {
      if (this.m_spellCheck == value)
        return;
      this.m_spellCheck = value;
      if (this.m_spellCheck)
        this.Flags &= ~FieldFlags.DoNotSpellCheck;
      else
        this.Flags |= FieldFlags.DoNotSpellCheck;
    }
  }

  public bool InsertSpaces
  {
    get
    {
      this.m_insertSpaces = (FieldFlags.Comb & this.Flags) != FieldFlags.Default && (this.Flags & FieldFlags.Multiline) == FieldFlags.Default && (this.Flags & FieldFlags.Password) == FieldFlags.Default && (this.Flags & FieldFlags.FileSelect) == FieldFlags.Default;
      return this.m_insertSpaces;
    }
    set
    {
      if (this.m_insertSpaces == value)
        return;
      this.m_insertSpaces = value;
      if (this.m_insertSpaces)
        this.Flags |= FieldFlags.Comb;
      else
        this.Flags &= ~FieldFlags.Comb;
    }
  }

  public bool Multiline
  {
    get => this.m_multiline;
    set
    {
      if (this.m_multiline == value)
        return;
      this.m_multiline = value;
      if (this.m_multiline)
      {
        this.Flags |= FieldFlags.Multiline;
        this.StringFormat.LineAlignment = PdfVerticalAlignment.Top;
      }
      else
      {
        this.Flags &= ~FieldFlags.Multiline;
        this.StringFormat.LineAlignment = PdfVerticalAlignment.Middle;
      }
    }
  }

  public bool Password
  {
    get => this.m_password;
    set
    {
      if (this.m_password == value)
        return;
      this.m_password = value;
      if (this.m_password)
        this.Flags |= FieldFlags.Password;
      else
        this.Flags &= ~FieldFlags.Password;
    }
  }

  public bool Scrollable
  {
    get => this.m_scrollable;
    set
    {
      if (this.m_scrollable == value)
        return;
      this.m_scrollable = value;
      if (this.m_scrollable)
        this.Flags &= ~FieldFlags.DoNotScroll;
      else
        this.Flags |= FieldFlags.DoNotScroll;
    }
  }

  public int MaxLength
  {
    get => this.m_maxLength;
    set
    {
      if (this.m_maxLength == value)
        return;
      this.m_maxLength = value;
      this.Dictionary.SetNumber("MaxLen", this.m_maxLength);
    }
  }

  public bool AutoResizeText
  {
    get => this.m_autoResizeText;
    set
    {
      this.m_autoResizeText = value;
      if (this.m_widget == null)
        return;
      this.m_widget.isAutoResize = value;
    }
  }

  public PdfTextBoxField(PdfPageBase page, string name)
    : base(page, name)
  {
    if (PdfDocument.ConformanceLevel != PdfConformanceLevel.None && PdfDocument.ConformanceLevel != PdfConformanceLevel.Pdf_X1A2001)
      return;
    this.Font = PdfDocument.DefaultFont;
  }

  internal PdfTextBoxField()
  {
  }

  internal override void Draw()
  {
    if (this.fieldItems != null && this.fieldItems.Count > 0)
    {
      foreach (PdfTextBoxField fieldItem in this.fieldItems)
      {
        fieldItem.Text = this.Text;
        fieldItem.Draw();
      }
    }
    base.Draw();
    if (this.Widget.ObtainAppearance() != null)
    {
      this.Page.Graphics.DrawPdfTemplate(this.Appearance.Normal, this.Location);
    }
    else
    {
      PaintParams paintParams = new PaintParams(this.Bounds, this.BackBrush, this.ForeBrush, this.BorderPen, this.BorderStyle, this.BorderWidth, this.ShadowBrush, this.RotationAngle);
      if (this.AutoResizeText && this.Flatten)
        this.SetFittingFontSize(paintParams, this.Text);
      FieldPainter.DrawTextBox(this.Page.Graphics, paintParams, this.Text, this.Font, this.StringFormat, this.Multiline, this.Scrollable);
    }
  }

  protected override void Initialize()
  {
    base.Initialize();
    this.Flags |= FieldFlags.DoNotSpellCheck;
    this.Dictionary.SetProperty("FT", (IPdfPrimitive) new PdfName("Tx"));
  }

  protected override void DrawAppearance(PdfTemplate template)
  {
    if (this.fieldItems != null && this.fieldItems.Count > 0)
    {
      foreach (PdfTextBoxField fieldItem in this.fieldItems)
      {
        fieldItem.Text = this.Text;
        fieldItem.DrawAppearance(fieldItem.Widget.Appearance.Normal);
      }
    }
    base.DrawAppearance(template);
    PaintParams paintParams = new PaintParams(new RectangleF(PointF.Empty, this.Size), this.BackBrush, this.ForeBrush, this.BorderPen, this.BorderStyle, this.BorderWidth, this.ShadowBrush, this.RotationAngle);
    paintParams.InsertSpace = this.InsertSpaces;
    string text = this.Text;
    if (this.Password)
    {
      text = string.Empty;
      for (int index = 0; index < this.Text.Length; ++index)
        text += "*";
    }
    template.m_writeTransformation = false;
    PdfGraphics graphics = template.Graphics;
    graphics.StreamWriter.BeginMarkupSequence("Tx");
    graphics.InitializeCoordinates();
    paintParams.BackBrush = (PdfBrush) null;
    if (this.AutoResizeText)
      this.SetFittingFontSize(paintParams, text);
    FieldPainter.DrawTextBox(graphics, paintParams, text, this.ObtainFont(), this.StringFormat, this.Multiline, this.Scrollable, this.MaxLength);
    graphics.StreamWriter.EndMarkupSequence();
  }

  private void SetFittingFontSize(PaintParams prms, string text)
  {
    float num1 = prms.BorderStyle == PdfBorderStyle.Beveled || prms.BorderStyle == PdfBorderStyle.Inset ? this.Bounds.Width - 8f * prms.BorderWidth : this.Bounds.Width - 4f * prms.BorderWidth;
    float num2 = this.Bounds.Height - 2f * this.BorderWidth;
    float num3 = 0.248f;
    PdfFont pdfFont = !(this.Font is PdfStandardFont) ? (PdfFont) (this.Font as PdfTrueTypeFont) : (PdfFont) (this.Font as PdfStandardFont);
    if (text.EndsWith(" "))
      this.StringFormat.MeasureTrailingSpaces = true;
    for (float num4 = 0.0f; (double) num4 <= (double) this.Bounds.Height; ++num4)
    {
      if (this.Font is PdfStandardFont)
        this.Font.Size = num4;
      else
        this.Font.Size = num4;
      SizeF sizeF1 = this.Font.MeasureString(text, this.StringFormat);
      if (text != null && ((double) sizeF1.Width > (double) this.Bounds.Width || (double) sizeF1.Height > (double) num2))
      {
        float num5 = num4;
        do
        {
          num5 -= 1f / 1000f;
          pdfFont.Size = num5;
          float lineWidth = this.Font.GetLineWidth(text, this.StringFormat);
          if ((double) num5 < (double) num3)
          {
            this.Font.Size = num3;
            break;
          }
          SizeF sizeF2 = this.Font.MeasureString(text, this.StringFormat);
          if ((double) lineWidth < (double) num1 && (double) sizeF2.Height < (double) num2)
          {
            this.Font.Size = num5;
            break;
          }
        }
        while ((double) num5 > (double) num3);
        break;
      }
    }
  }
}
