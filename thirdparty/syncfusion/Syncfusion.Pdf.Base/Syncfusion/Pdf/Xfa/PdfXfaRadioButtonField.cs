// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaRadioButtonField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaRadioButtonField : PdfXfaStyledField
{
  private string m_toolTip = string.Empty;
  private PdfXfaRotateAngle m_rotate;
  private PdfXfaCheckedStyle m_checkedStyle = PdfXfaCheckedStyle.Circle;
  private PdfXfaCheckBoxAppearance m_radioButtonAppearance = PdfXfaCheckBoxAppearance.Round;
  private bool m_isChecked;
  private float m_radioButtonSize = 10f;
  private PdfXfaCaption m_caption = new PdfXfaCaption();
  internal PdfXfaField parent;

  public bool IsChecked
  {
    get => this.m_isChecked;
    set => this.m_isChecked = value;
  }

  public PdfXfaCaption Caption
  {
    get => this.m_caption;
    set => this.m_caption = value;
  }

  public PdfXfaCheckedStyle CheckedStyle
  {
    set => this.m_checkedStyle = value;
    get => this.m_checkedStyle;
  }

  public PdfXfaCheckBoxAppearance RadioButtonAppearance
  {
    set => this.m_radioButtonAppearance = value;
    get => this.m_radioButtonAppearance;
  }

  public float RadioButtonSize
  {
    get => this.m_radioButtonSize;
    set => this.m_radioButtonSize = value;
  }

  public PdfXfaRadioButtonField(string name, SizeF size)
  {
    this.Name = name;
    this.Width = size.Width;
    this.Height = size.Height;
  }

  public PdfXfaRadioButtonField(string name, float width, float height)
  {
    this.Name = name;
    this.Width = width;
    this.Height = height;
  }

  internal void Save(XfaWriter xfaWriter, int index)
  {
    xfaWriter.Write.WriteStartElement("field");
    if (this.Name != null)
      xfaWriter.Write.WriteAttributeString("name", this.Name);
    xfaWriter.SetSize(this.Height, this.Width, 0.0f, 0.0f);
    xfaWriter.SetRPR(this.Rotate, this.Visibility, this.ReadOnly);
    xfaWriter.WriteUI("checkButton", new Dictionary<string, string>()
    {
      {
        "shape",
        this.RadioButtonAppearance.ToString().ToLower()
      },
      {
        "mark",
        this.CheckedStyle.ToString().ToLower()
      },
      {
        "size",
        this.RadioButtonSize.ToString() + "pt"
      }
    }, this.Border);
    xfaWriter.WriteItems(index.ToString());
    if (this.IsChecked)
      xfaWriter.WriteValue(index.ToString(), 0);
    this.SetMFTP(xfaWriter);
    if (this.Caption != null)
      this.Caption.Save(xfaWriter);
    xfaWriter.Write.WriteEndElement();
  }

  internal new void SetMFTP(XfaWriter xfaWriter)
  {
    xfaWriter.WriteFontInfo(this.Font, this.ForeColor);
    xfaWriter.WriteMargins(this.Margins);
    if (this.ToolTip != null && this.ToolTip != "")
      xfaWriter.WriteToolTip(this.ToolTip);
    xfaWriter.WritePragraph(this.VerticalAlignment, this.HorizontalAlignment);
  }

  internal PdfRadioButtonListItem SaveAcroForm(PdfPage page, RectangleF bounds)
  {
    PdfRadioButtonListItem field = new PdfRadioButtonListItem(this.Name);
    field.isXfa = true;
    field.Style = this.GetStyle(this.CheckedStyle);
    if (this.Font != null)
      field.Font = this.Font;
    if (this.ReadOnly || (this.parent as PdfXfaRadioButtonGroup).ReadOnly || (this.parent as PdfXfaRadioButtonGroup).parent.m_isReadOnly)
      field.ReadOnly = true;
    if (this.Border != null)
      this.Border.ApplyAcroBorder((PdfStyledField) field);
    if (this.ForeColor != PdfColor.Empty)
      field.ForeColor = this.ForeColor;
    if (this.Visibility == PdfXfaVisibility.Invisible)
      field.Visibility = PdfFormFieldVisibility.Hidden;
    RectangleF bounds1 = new RectangleF();
    SizeF size = this.GetSize();
    bounds1.Location = new PointF(bounds.Location.X + this.Margins.Left, bounds.Location.Y + this.Margins.Top);
    bounds1.Size = new SizeF(size.Width - (this.Margins.Right + this.Margins.Left), size.Height - (this.Margins.Top + this.Margins.Bottom));
    if ((double) this.Caption.Width == 0.0)
      this.Caption.Width = this.Caption.Position == PdfXfaPosition.Top || this.Caption.Position == PdfXfaPosition.Bottom ? (this.Rotate == PdfXfaRotateAngle.RotateAngle90 || this.Rotate == PdfXfaRotateAngle.RotateAngle270 ? bounds1.Width - this.RadioButtonSize : bounds1.Height - this.RadioButtonSize) : (this.Rotate == PdfXfaRotateAngle.RotateAngle90 || this.Rotate == PdfXfaRotateAngle.RotateAngle270 ? bounds1.Height - this.RadioButtonSize : bounds1.Width - this.RadioButtonSize);
    if (this.Visibility != PdfXfaVisibility.Invisible)
      this.Caption.DrawText((PdfPageBase) page, bounds1, this.GetRotationAngle());
    float num1;
    float num2;
    if (this.Caption.Position == PdfXfaPosition.Top || this.Caption.Position == PdfXfaPosition.Bottom)
    {
      num1 = (float) ((double) this.Width - (double) this.RadioButtonSize - ((double) this.RadioButtonSize + (double) this.Margins.Left + (double) this.Margins.Right));
      num2 = (float) ((double) this.Height - (double) this.Caption.Width - ((double) this.RadioButtonSize + (double) this.Margins.Bottom + (double) this.Margins.Top));
    }
    else
    {
      num1 = (float) ((double) this.Width - (double) this.Caption.Width - ((double) this.RadioButtonSize + (double) this.Margins.Left + (double) this.Margins.Right));
      num2 = this.Height - (this.RadioButtonSize + this.Margins.Bottom + this.Margins.Top);
    }
    if (this.Rotate == PdfXfaRotateAngle.RotateAngle0)
    {
      if (this.Caption.Position == PdfXfaPosition.Top || this.Caption.Position == PdfXfaPosition.Bottom)
      {
        if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Left)
          num1 = 0.0f;
        else if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Center)
          num1 /= 2f;
        if (this.VerticalAlignment == PdfXfaVerticalAlignment.Top)
          num2 = 0.0f;
        else if (this.VerticalAlignment == PdfXfaVerticalAlignment.Middle)
          num2 /= 2f;
        bounds1 = this.Caption.Position != PdfXfaPosition.Top ? new RectangleF(new PointF(bounds1.Location.X + num1, bounds1.Location.Y + num2), new SizeF(this.RadioButtonSize, this.RadioButtonSize)) : new RectangleF(new PointF(bounds1.Location.X + num1, bounds1.Location.Y + this.Caption.Width + num2), new SizeF(this.RadioButtonSize, this.RadioButtonSize));
      }
      else if (this.Caption.Position == PdfXfaPosition.Left || this.Caption.Position == PdfXfaPosition.Right)
      {
        if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Left)
          num1 = 0.0f;
        else if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Center)
          num1 /= 2f;
        if (this.VerticalAlignment == PdfXfaVerticalAlignment.Top)
          num2 = 0.0f;
        else if (this.VerticalAlignment == PdfXfaVerticalAlignment.Middle)
          num2 /= 2f;
        bounds1 = this.Caption.Position != PdfXfaPosition.Left ? new RectangleF(new PointF(bounds1.Location.X + num1, bounds1.Location.Y + num2), new SizeF(this.RadioButtonSize, this.RadioButtonSize)) : new RectangleF(new PointF(bounds1.Location.X + this.Caption.Width + num1, bounds1.Location.Y + num2), new SizeF(this.RadioButtonSize, this.RadioButtonSize));
      }
    }
    else if (this.Rotate == PdfXfaRotateAngle.RotateAngle180)
    {
      if (this.Caption.Position == PdfXfaPosition.Top || this.Caption.Position == PdfXfaPosition.Bottom)
      {
        if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Right)
          num1 = 0.0f;
        else if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Center)
          num1 /= 2f;
        if (this.VerticalAlignment == PdfXfaVerticalAlignment.Bottom)
          num2 = 0.0f;
        else if (this.VerticalAlignment == PdfXfaVerticalAlignment.Middle)
          num2 /= 2f;
        bounds1 = this.Caption.Position != PdfXfaPosition.Top ? new RectangleF(new PointF(bounds1.Location.X + num1, bounds1.Location.Y + this.Caption.Width + num2), new SizeF(this.RadioButtonSize, this.RadioButtonSize)) : new RectangleF(new PointF(bounds1.Location.X + num1, bounds1.Location.Y + num2), new SizeF(this.RadioButtonSize, this.RadioButtonSize));
      }
      else if (this.Caption.Position == PdfXfaPosition.Left || this.Caption.Position == PdfXfaPosition.Right)
      {
        if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Right)
          num1 = 0.0f;
        else if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Center)
          num1 /= 2f;
        if (this.VerticalAlignment == PdfXfaVerticalAlignment.Bottom)
          num2 = 0.0f;
        else if (this.VerticalAlignment == PdfXfaVerticalAlignment.Middle)
          num2 /= 2f;
        bounds1 = this.Caption.Position != PdfXfaPosition.Left ? new RectangleF(new PointF(bounds1.Location.X + this.Caption.Width + num1, bounds1.Location.Y + num2), new SizeF(this.RadioButtonSize, this.RadioButtonSize)) : new RectangleF(new PointF(bounds1.Location.X + num1, bounds1.Location.Y + num2), new SizeF(this.RadioButtonSize, this.RadioButtonSize));
      }
    }
    else if (this.Rotate == PdfXfaRotateAngle.RotateAngle90)
    {
      if (this.Caption.Position == PdfXfaPosition.Top || this.Caption.Position == PdfXfaPosition.Bottom)
      {
        if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Right)
          num1 = 0.0f;
        else if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Center)
          num1 /= 2f;
        if (this.VerticalAlignment == PdfXfaVerticalAlignment.Top)
          num2 = 0.0f;
        else if (this.VerticalAlignment == PdfXfaVerticalAlignment.Middle)
          num2 /= 2f;
        bounds1 = this.Caption.Position != PdfXfaPosition.Top ? new RectangleF(new PointF(bounds1.Location.X + num2, bounds1.Location.Y + num1), new SizeF(this.RadioButtonSize, this.RadioButtonSize)) : new RectangleF(new PointF(bounds1.Location.X + this.Caption.Width + num2, bounds1.Location.Y + num1), new SizeF(this.RadioButtonSize, this.RadioButtonSize));
      }
      else if (this.Caption.Position == PdfXfaPosition.Left || this.Caption.Position == PdfXfaPosition.Right)
      {
        if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Right)
          num1 = 0.0f;
        else if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Center)
          num1 /= 2f;
        if (this.VerticalAlignment == PdfXfaVerticalAlignment.Top)
          num2 = 0.0f;
        else if (this.VerticalAlignment == PdfXfaVerticalAlignment.Middle)
          num2 /= 2f;
        bounds1 = this.Caption.Position != PdfXfaPosition.Left ? new RectangleF(new PointF(bounds1.Location.X + num2, bounds1.Location.Y + num1 + this.Caption.Width), new SizeF(this.RadioButtonSize, this.RadioButtonSize)) : new RectangleF(new PointF(bounds1.Location.X + num2, bounds1.Location.Y + num1), new SizeF(this.RadioButtonSize, this.RadioButtonSize));
      }
    }
    else if (this.Rotate == PdfXfaRotateAngle.RotateAngle270)
    {
      if (this.Caption.Position == PdfXfaPosition.Top || this.Caption.Position == PdfXfaPosition.Bottom)
      {
        if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Left)
          num1 = 0.0f;
        else if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Center)
          num1 /= 2f;
        if (this.VerticalAlignment == PdfXfaVerticalAlignment.Bottom)
          num2 = 0.0f;
        else if (this.VerticalAlignment == PdfXfaVerticalAlignment.Middle)
          num2 /= 2f;
        bounds1 = this.Caption.Position != PdfXfaPosition.Bottom ? new RectangleF(new PointF(bounds1.Location.X + num2, bounds1.Location.Y + num1), new SizeF(this.RadioButtonSize, this.RadioButtonSize)) : new RectangleF(new PointF(bounds1.Location.X + this.Caption.Width + num2, bounds1.Location.Y + num1), new SizeF(this.RadioButtonSize, this.RadioButtonSize));
      }
      else if (this.Caption.Position == PdfXfaPosition.Left || this.Caption.Position == PdfXfaPosition.Right)
      {
        if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Left)
          num1 = 0.0f;
        else if (this.HorizontalAlignment == PdfXfaHorizontalAlignment.Center)
          num1 /= 2f;
        if (this.VerticalAlignment == PdfXfaVerticalAlignment.Bottom)
          num2 = 0.0f;
        else if (this.VerticalAlignment == PdfXfaVerticalAlignment.Middle)
          num2 /= 2f;
        bounds1 = this.Caption.Position != PdfXfaPosition.Right ? new RectangleF(new PointF(bounds1.Location.X + num2, bounds1.Location.Y + num1 + this.Caption.Width), new SizeF(this.RadioButtonSize, this.RadioButtonSize)) : new RectangleF(new PointF(bounds1.Location.X + num2, bounds1.Location.Y + num1), new SizeF(this.RadioButtonSize, this.RadioButtonSize));
      }
    }
    field.Bounds = bounds1;
    field.Widget.WidgetAppearance.RotationAngle = this.GetRotationAngle();
    return field;
  }

  private PdfCheckBoxStyle GetStyle(PdfXfaCheckedStyle style)
  {
    PdfCheckBoxStyle style1 = PdfCheckBoxStyle.Check;
    switch (style)
    {
      case PdfXfaCheckedStyle.Check:
        style1 = PdfCheckBoxStyle.Check;
        break;
      case PdfXfaCheckedStyle.Circle:
        style1 = PdfCheckBoxStyle.Circle;
        break;
      case PdfXfaCheckedStyle.Cross:
        style1 = PdfCheckBoxStyle.Cross;
        break;
      case PdfXfaCheckedStyle.Diamond:
        style1 = PdfCheckBoxStyle.Diamond;
        break;
      case PdfXfaCheckedStyle.Square:
        style1 = PdfCheckBoxStyle.Square;
        break;
      case PdfXfaCheckedStyle.Star:
        style1 = PdfCheckBoxStyle.Star;
        break;
    }
    return style1;
  }

  public object Clone()
  {
    PdfXfaRadioButtonField radioButtonField = (PdfXfaRadioButtonField) this.MemberwiseClone();
    radioButtonField.Caption = this.Caption.Clone() as PdfXfaCaption;
    return (object) radioButtonField;
  }
}
