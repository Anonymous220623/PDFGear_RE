// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaNumericField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaNumericField : PdfXfaStyledField
{
  private double m_value = double.NaN;
  private int m_combLength;
  private PdfXfaNumericType m_fieldType;
  private PdfPaddings m_padding = new PdfPaddings(0.0f, 0.0f, 0.0f, 0.0f);
  private PdfXfaCaption m_caption = new PdfXfaCaption();
  private string m_patternString = string.Empty;
  internal new PdfXfaForm parent;
  private string m_culture = string.Empty;

  public string Culture
  {
    get => this.m_culture;
    set
    {
      if (value == null)
        return;
      this.m_culture = value;
    }
  }

  public PdfXfaCaption Caption
  {
    get => this.m_caption;
    set => this.m_caption = value;
  }

  public PdfPaddings Padding
  {
    get => this.m_padding;
    set
    {
      if (value == null)
        return;
      this.m_padding = value;
    }
  }

  public double NumericValue
  {
    set => this.m_value = value;
    get => this.m_value;
  }

  public int CombLength
  {
    get => this.m_combLength;
    set => this.m_combLength = value;
  }

  public PdfXfaNumericType FieldType
  {
    get => this.m_fieldType;
    set => this.m_fieldType = value;
  }

  public string PatternString
  {
    get => this.m_patternString;
    set
    {
      if (value == null)
        return;
      this.m_patternString = value;
    }
  }

  public PdfXfaNumericField(string name, SizeF size)
  {
    this.Height = size.Height;
    this.Width = size.Width;
    this.Name = name;
    this.HorizontalAlignment = PdfXfaHorizontalAlignment.Left;
    this.Padding.Left = 3f;
    this.Padding.Right = 3f;
  }

  public PdfXfaNumericField(string name, float width, float height)
  {
    this.Height = height;
    this.Width = width;
    this.Name = name;
    this.HorizontalAlignment = PdfXfaHorizontalAlignment.Left;
    this.Padding.Left = 3f;
    this.Padding.Right = 3f;
  }

  internal void Save(XfaWriter xfaWriter)
  {
    if (this.Name == "" || this.Name == string.Empty)
      this.Name = "numeric" + xfaWriter.m_fieldCount.ToString();
    xfaWriter.Write.WriteStartElement("field");
    xfaWriter.Write.WriteAttributeString("name", this.Name);
    this.SetSize(xfaWriter);
    if (this.Culture != null && this.Culture != string.Empty)
      xfaWriter.Write.WriteAttributeString("locale", this.Culture);
    xfaWriter.SetRPR(this.Rotate, this.Visibility, this.ReadOnly);
    if (this.CombLength > 0)
      xfaWriter.WriteUI("numericEdit", (Dictionary<string, string>) null, this.Border, this.CombLength, this.Padding);
    else
      xfaWriter.WriteUI("numericEdit", (Dictionary<string, string>) null, this.Border, this.Padding);
    string lower = this.FieldType.ToString().ToLower();
    if (this.FieldType == PdfXfaNumericType.Currency || this.FieldType == PdfXfaNumericType.Percent || this.PatternString != string.Empty)
      lower = PdfXfaNumericType.Float.ToString().ToLower();
    if (double.IsNaN(this.NumericValue))
    {
      xfaWriter.WriteValue("", lower, 0);
    }
    else
    {
      if (this.FieldType == PdfXfaNumericType.Integer)
        this.NumericValue = (double) (int) this.NumericValue;
      xfaWriter.WriteValue(this.NumericValue.ToString(), lower, 0);
    }
    xfaWriter.Write.WriteStartElement("format");
    xfaWriter.Write.WriteStartElement("picture");
    if (this.m_patternString != string.Empty)
      xfaWriter.Write.WriteString($"num{{{this.m_patternString}}}");
    else
      xfaWriter.Write.WriteString($"num.{this.FieldType.ToString().ToLower()}{{}}");
    xfaWriter.Write.WriteEndElement();
    xfaWriter.Write.WriteEndElement();
    this.SetMFTP(xfaWriter);
    if (this.Caption != null)
      this.Caption.Save(xfaWriter);
    if (this.parent != null && this.parent.m_formType == PdfXfaType.Static)
    {
      xfaWriter.Write.WriteStartElement("keep");
      xfaWriter.Write.WriteAttributeString("intact", "contentArea");
      xfaWriter.Write.WriteEndElement();
    }
    xfaWriter.Write.WriteEndElement();
  }

  internal PdfField SaveAcroForm(PdfPage page, RectangleF bounds, string name)
  {
    PdfTextBoxField field = new PdfTextBoxField((PdfPageBase) page, name);
    field.StringFormat.LineAlignment = (PdfVerticalAlignment) this.VerticalAlignment;
    field.TextAlignment = this.ConvertToPdfTextAlignment(this.HorizontalAlignment);
    if (this.Font == null)
      field.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
    else
      field.Font = this.Font;
    if (this.ReadOnly || this.parent.ReadOnly || this.parent.m_isReadOnly)
      field.ReadOnly = true;
    if (this.Border != null)
      this.Border.ApplyAcroBorder((PdfStyledField) field);
    if (this.ForeColor != PdfColor.Empty)
      field.ForeColor = this.ForeColor;
    if (this.Visibility == PdfXfaVisibility.Invisible)
      field.Visibility = PdfFormFieldVisibility.Hidden;
    if (!double.IsNaN(this.NumericValue))
      field.Text = this.NumericValue.ToString();
    RectangleF bounds1 = new RectangleF();
    SizeF size = this.GetSize();
    bounds1.Location = new PointF(bounds.Location.X + this.Margins.Left, bounds.Location.Y + this.Margins.Top);
    bounds1.Size = new SizeF(size.Width - (this.Margins.Right + this.Margins.Left), size.Height - (this.Margins.Top + this.Margins.Bottom));
    if (this.Visibility != PdfXfaVisibility.Invisible)
      this.Caption.DrawText((PdfPageBase) page, bounds1, this.GetRotationAngle());
    field.Bounds = this.GetBounds(bounds1, this.Rotate, this.Caption);
    field.Widget.WidgetAppearance.RotationAngle = this.GetRotationAngle();
    return (PdfField) field;
  }

  private void SetSize(XfaWriter xfaWriter)
  {
    SizeF sizeF = new SizeF();
    if ((double) this.Caption.Width > 0.0)
      sizeF.Width = sizeF.Height = this.Caption.Width;
    else
      sizeF = this.Caption.MeasureString();
    if (this.Caption.Position == PdfXfaPosition.Bottom || this.Caption.Position == PdfXfaPosition.Top)
    {
      xfaWriter.SetSize(this.Height, this.Width, 0.0f, 0.0f, 0.0f, 0.0f);
      this.Caption.Width = sizeF.Height;
    }
    else
    {
      if (this.Caption.Position != PdfXfaPosition.Left && this.Caption.Position != PdfXfaPosition.Right)
        return;
      xfaWriter.SetSize(this.Height, this.Width, 0.0f, 0.0f, 0.0f, 0.0f);
      this.Caption.Width = sizeF.Width;
    }
  }

  public object Clone()
  {
    PdfXfaNumericField pdfXfaNumericField = (PdfXfaNumericField) this.MemberwiseClone();
    pdfXfaNumericField.Caption = this.Caption.Clone() as PdfXfaCaption;
    return (object) pdfXfaNumericField;
  }
}
