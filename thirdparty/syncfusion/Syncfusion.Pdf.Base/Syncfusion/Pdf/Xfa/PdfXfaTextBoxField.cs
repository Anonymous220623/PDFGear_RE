// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaTextBoxField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaTextBoxField : PdfXfaStyledField, ICloneable
{
  private float m_minimumHeight;
  private float m_minimumWidth;
  private float m_maxHeight;
  private float m_maxWidth;
  private string m_text = string.Empty;
  private int m_maxLength;
  private int m_combLength;
  private char m_passwordChar;
  private PdfPaddings m_padding = new PdfPaddings(0.0f, 0.0f, 0.0f, 0.0f);
  private PdfXfaTextBoxType m_type;
  private PdfXfaCaption m_caption = new PdfXfaCaption();
  internal new PdfXfaForm parent;

  public PdfXfaCaption Caption
  {
    get => this.m_caption;
    set => this.m_caption = value;
  }

  public PdfXfaTextBoxType Type
  {
    get => this.m_type;
    set => this.m_type = value;
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

  public float MinimumHeight
  {
    set => this.m_minimumHeight = value;
    get => this.m_minimumHeight;
  }

  public float MinimumWidth
  {
    set => this.m_minimumWidth = value;
    get => this.m_minimumWidth;
  }

  public float MaximumHeight
  {
    set => this.m_maxHeight = value;
    get => this.m_maxHeight;
  }

  public float MaximumWidth
  {
    set => this.m_maxWidth = value;
    get => this.m_maxWidth;
  }

  public string Text
  {
    set => this.m_text = value;
    get => this.m_text;
  }

  public int MaximumLength
  {
    get => this.m_maxLength;
    set => this.m_maxLength = value;
  }

  public int CombLength
  {
    get => this.m_combLength;
    set => this.m_combLength = value;
  }

  public char PasswordCharacter
  {
    get => this.m_passwordChar;
    set => this.m_passwordChar = value;
  }

  public PdfXfaTextBoxField(string fieldName, SizeF minimumSize)
  {
    this.MinimumHeight = minimumSize.Height;
    this.MinimumWidth = minimumSize.Width;
    this.Name = fieldName;
    this.HorizontalAlignment = PdfXfaHorizontalAlignment.Left;
    this.Padding.Left = 3f;
    this.Padding.Right = 3f;
  }

  public PdfXfaTextBoxField(string fieldName, SizeF minimumSize, string defaultText)
  {
    this.MinimumHeight = minimumSize.Height;
    this.MinimumWidth = minimumSize.Width;
    this.Name = fieldName;
    this.Text = defaultText;
    this.HorizontalAlignment = PdfXfaHorizontalAlignment.Left;
    this.Padding.Left = 3f;
    this.Padding.Right = 3f;
  }

  public PdfXfaTextBoxField(
    string fieldName,
    SizeF minimumSize,
    string defaultText,
    PdfXfaTextBoxType fieldType)
  {
    this.MinimumHeight = minimumSize.Height;
    this.MinimumWidth = minimumSize.Width;
    this.Name = fieldName;
    this.Text = defaultText;
    this.Type = fieldType;
    this.HorizontalAlignment = PdfXfaHorizontalAlignment.Left;
    this.Padding.Left = 3f;
    this.Padding.Right = 3f;
  }

  public PdfXfaTextBoxField(string fieldName, SizeF minimumSize, PdfXfaTextBoxType fieldType)
  {
    this.MinimumHeight = minimumSize.Height;
    this.MinimumWidth = minimumSize.Width;
    this.Name = fieldName;
    this.Type = fieldType;
    this.HorizontalAlignment = PdfXfaHorizontalAlignment.Left;
    this.Padding.Left = 3f;
    this.Padding.Right = 3f;
  }

  public PdfXfaTextBoxField(string fieldName, float minWidth, float minHeight)
  {
    this.MinimumHeight = minHeight;
    this.MinimumWidth = minWidth;
    this.Name = fieldName;
    this.HorizontalAlignment = PdfXfaHorizontalAlignment.Left;
    this.Padding.Left = 3f;
    this.Padding.Right = 3f;
  }

  public PdfXfaTextBoxField(string fieldName, float minWidth, float minHeight, string defaultText)
  {
    this.MinimumHeight = minHeight;
    this.MinimumWidth = minWidth;
    this.Name = fieldName;
    this.Text = defaultText;
    this.HorizontalAlignment = PdfXfaHorizontalAlignment.Left;
    this.Padding.Left = 3f;
    this.Padding.Right = 3f;
  }

  public PdfXfaTextBoxField(
    string fieldName,
    float minWidth,
    float minHeight,
    PdfXfaTextBoxType fieldType)
  {
    this.MinimumHeight = minHeight;
    this.MinimumWidth = minWidth;
    this.Name = fieldName;
    this.Type = fieldType;
    this.HorizontalAlignment = PdfXfaHorizontalAlignment.Left;
    this.Padding.Left = 3f;
    this.Padding.Right = 3f;
  }

  public PdfXfaTextBoxField(
    string fieldName,
    float minWidth,
    float minHeight,
    string defaultText,
    PdfXfaTextBoxType fieldType)
  {
    this.MinimumHeight = minHeight;
    this.MinimumWidth = minWidth;
    this.Name = fieldName;
    this.Text = defaultText;
    this.Type = fieldType;
    this.HorizontalAlignment = PdfXfaHorizontalAlignment.Left;
    this.Padding.Left = 3f;
    this.Padding.Right = 3f;
  }

  internal void Save(XfaWriter xfaWriter, PdfXfaType type)
  {
    if (this.Name == "" || this.Name == string.Empty)
      this.Name = "textBox" + xfaWriter.m_fieldCount.ToString();
    xfaWriter.Write.WriteStartElement("field");
    xfaWriter.Write.WriteAttributeString("name", this.Name);
    if (this.Type == PdfXfaTextBoxType.Password || this.Type == PdfXfaTextBoxType.Comb)
    {
      if ((double) this.Height <= 0.0)
        this.Height = this.MinimumHeight;
      if ((double) this.Width <= 0.0)
        this.Width = this.MinimumWidth;
    }
    this.SetSize(xfaWriter, type);
    xfaWriter.SetRPR(this.Rotate, this.Visibility, this.ReadOnly);
    if (this.Type == PdfXfaTextBoxType.Password)
      xfaWriter.WriteUI("passwordEdit", new Dictionary<string, string>()
      {
        {
          "passwordChar",
          this.PasswordCharacter.ToString()
        }
      }, this.Border, 0, this.Padding);
    else if (this.Type == PdfXfaTextBoxType.Comb)
    {
      if (this.CombLength > 0)
      {
        xfaWriter.WriteUI("textEdit", (Dictionary<string, string>) null, this.Border, this.CombLength, this.Padding);
      }
      else
      {
        int comb = 0;
        if (this.Text != null && this.Text != string.Empty)
          comb = this.Text.Length;
        xfaWriter.WriteUI("textEdit", (Dictionary<string, string>) null, this.Border, comb, this.Padding);
      }
    }
    else if (this.Type == PdfXfaTextBoxType.Multiline)
      xfaWriter.WriteUI("textEdit", new Dictionary<string, string>()
      {
        {
          "multiLine",
          "1"
        }
      }, this.Border, this.Padding);
    else
      xfaWriter.WriteUI("textEdit", (Dictionary<string, string>) null, this.Border, this.Padding);
    if (this.Text != null && this.Text != string.Empty)
      xfaWriter.WriteValue(this.Text, this.m_maxLength);
    else if (this.m_maxLength > 0)
      xfaWriter.WriteValue("", this.m_maxLength);
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

  internal void Save(XfaWriter xfaWriter) => this.Save(xfaWriter, PdfXfaType.Dynamic);

  internal PdfField SaveAcroForm(PdfPage page, RectangleF bounds, string name)
  {
    PdfTextBoxField field = new PdfTextBoxField((PdfPageBase) page, name);
    switch (this.Type)
    {
      case PdfXfaTextBoxType.Multiline:
        field.Multiline = true;
        break;
      case PdfXfaTextBoxType.Password:
        field.Password = true;
        break;
    }
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
    if (this.MaximumLength > 0)
      field.MaxLength = this.MaximumLength;
    SizeF size = this.GetSize();
    field.StringFormat.LineAlignment = (PdfVerticalAlignment) this.VerticalAlignment;
    field.TextAlignment = this.ConvertToPdfTextAlignment(this.HorizontalAlignment);
    if (this.Text != string.Empty)
      field.Text = this.Text;
    if (this.Visibility == PdfXfaVisibility.Invisible)
      field.Visibility = PdfFormFieldVisibility.Hidden;
    RectangleF bounds1 = new RectangleF();
    bounds1.Location = new PointF(bounds.Location.X + this.Margins.Left, bounds.Location.Y + this.Margins.Top);
    bounds1.Size = new SizeF(size.Width - (this.Margins.Right + this.Margins.Left), size.Height - (this.Margins.Top + this.Margins.Bottom));
    if (this.Visibility != PdfXfaVisibility.Invisible)
      this.Caption.DrawText((PdfPageBase) page, bounds1, this.GetRotationAngle());
    field.Bounds = this.GetBounds(bounds1, this.Rotate, this.Caption);
    field.Widget.WidgetAppearance.RotationAngle = this.GetRotationAngle();
    return (PdfField) field;
  }

  private void SetSize(XfaWriter xfaWriter, PdfXfaType type)
  {
    SizeF sizeF = new SizeF();
    if ((double) this.Caption.Width > 0.0)
      sizeF.Width = sizeF.Height = this.Caption.Width;
    else
      sizeF = this.Caption.MeasureString();
    float fixedHeight = this.Height;
    float fixedWidth = this.Width;
    if ((double) this.Height <= 0.0)
      fixedHeight = this.MinimumHeight;
    if ((double) this.Width <= 0.0)
      fixedWidth = this.MinimumWidth;
    if (this.Caption.Position == PdfXfaPosition.Bottom || this.Caption.Position == PdfXfaPosition.Top)
    {
      if (type == PdfXfaType.Dynamic)
        xfaWriter.SetSize(this.Height, this.Width, this.MinimumHeight, this.MinimumWidth, this.MaximumHeight, this.MaximumWidth);
      else
        xfaWriter.SetSize(fixedHeight, fixedWidth, 0.0f, 0.0f, 0.0f, 0.0f);
      this.Caption.Width = sizeF.Height;
    }
    else
    {
      if (this.Caption.Position != PdfXfaPosition.Left && this.Caption.Position != PdfXfaPosition.Right)
        return;
      if (type == PdfXfaType.Dynamic)
        xfaWriter.SetSize(this.Height, this.Width, this.MinimumHeight, this.MinimumWidth, this.MaximumHeight, this.MaximumWidth);
      else
        xfaWriter.SetSize(fixedHeight, fixedWidth, 0.0f, 0.0f, 0.0f, 0.0f);
      this.Caption.Width = sizeF.Width;
    }
  }

  internal new SizeF GetSize()
  {
    SizeF size = new SizeF();
    size.Height = (double) this.Height <= 0.0 ? this.MinimumHeight : this.Height;
    size.Width = (double) this.Width <= 0.0 ? this.MinimumWidth : this.Width;
    if (this.Rotate == PdfXfaRotateAngle.RotateAngle270 || this.Rotate == PdfXfaRotateAngle.RotateAngle90)
      size = new SizeF(size.Height, size.Width);
    return size;
  }

  public object Clone()
  {
    PdfXfaTextBoxField pdfXfaTextBoxField = (PdfXfaTextBoxField) this.MemberwiseClone();
    pdfXfaTextBoxField.Caption = (PdfXfaCaption) this.Caption.Clone();
    return (object) pdfXfaTextBoxField;
  }
}
