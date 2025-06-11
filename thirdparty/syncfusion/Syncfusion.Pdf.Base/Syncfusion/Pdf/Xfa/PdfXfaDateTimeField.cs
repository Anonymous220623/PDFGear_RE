// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaDateTimeField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public class PdfXfaDateTimeField : PdfXfaStyledField
{
  private DateTime m_value;
  private PdfXfaDatePattern m_datePatterns;
  private PdfXfaTimePattern m_timePatterns;
  private PdfXfaDateTimeFormat m_format;
  private bool m_requireValidation;
  internal bool isSet;
  private PdfXfaCaption m_caption = new PdfXfaCaption();
  internal new PdfXfaForm parent;
  private PdfPaddings m_padding = new PdfPaddings(0.0f, 0.0f, 0.0f, 0.0f);

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

  public PdfXfaCaption Caption
  {
    get => this.m_caption;
    set => this.m_caption = value;
  }

  public DateTime Value
  {
    get => this.m_value;
    set
    {
      this.isSet = true;
      this.m_value = value;
    }
  }

  public PdfXfaDatePattern DatePattern
  {
    get => this.m_datePatterns;
    set => this.m_datePatterns = value;
  }

  public PdfXfaDateTimeFormat Format
  {
    get => this.m_format;
    set => this.m_format = value;
  }

  public PdfXfaTimePattern TimePattern
  {
    get => this.m_timePatterns;
    set => this.m_timePatterns = value;
  }

  public bool RequireValidation
  {
    get => this.m_requireValidation;
    set => this.m_requireValidation = value;
  }

  public PdfXfaDateTimeField(string name, SizeF size)
  {
    this.Width = size.Width;
    this.Height = size.Height;
    this.Name = name;
    this.Padding.Left = 3f;
    this.Padding.Right = 3f;
  }

  public PdfXfaDateTimeField(string name, float width, float height)
  {
    this.Width = width;
    this.Height = height;
    this.Name = name;
    this.Padding.Left = 3f;
    this.Padding.Right = 3f;
  }

  internal void Save(XfaWriter xfaWriter)
  {
    if (this.Name == "" || this.Name == string.Empty)
      this.Name = "dateTimeField" + xfaWriter.m_fieldCount.ToString();
    xfaWriter.Write.WriteStartElement("field");
    xfaWriter.Write.WriteAttributeString("name", this.Name);
    this.SetSize(xfaWriter);
    xfaWriter.SetRPR(this.Rotate, this.Visibility, this.ReadOnly);
    xfaWriter.Write.WriteStartElement("ui");
    xfaWriter.Write.WriteStartElement("dateTimeEdit");
    xfaWriter.Write.WriteStartElement("pictures");
    xfaWriter.Write.WriteString(this.GetDatePattern());
    xfaWriter.Write.WriteEndElement();
    xfaWriter.DrawBorder(this.Border);
    xfaWriter.WriteMargins(this.m_padding.Left, this.m_padding.Right, this.m_padding.Bottom, this.m_padding.Top);
    xfaWriter.Write.WriteEndElement();
    xfaWriter.Write.WriteEndElement();
    this.SetMFTP(xfaWriter);
    if (this.Caption != null)
      this.Caption.Save(xfaWriter);
    xfaWriter.Write.WriteStartElement("value");
    switch (this.Format)
    {
      case PdfXfaDateTimeFormat.Date:
        xfaWriter.Write.WriteStartElement("date");
        if (this.isSet)
          xfaWriter.Write.WriteString(this.Value.ToString(xfaWriter.GetDatePattern(this.DatePattern)));
        xfaWriter.Write.WriteEndElement();
        xfaWriter.Write.WriteEndElement();
        xfaWriter.WritePattern(this.GetDatePattern(), this.RequireValidation);
        break;
      case PdfXfaDateTimeFormat.Time:
        xfaWriter.Write.WriteStartElement("time");
        if (this.isSet)
          xfaWriter.Write.WriteString(this.Value.ToString(xfaWriter.GetTimePattern(this.TimePattern)));
        xfaWriter.Write.WriteEndElement();
        xfaWriter.Write.WriteEndElement();
        xfaWriter.WritePattern(this.GetDatePattern(), this.RequireValidation);
        break;
      case PdfXfaDateTimeFormat.DateTime:
        xfaWriter.Write.WriteStartElement("dateTime");
        if (this.isSet)
          xfaWriter.Write.WriteString(this.Value.ToString(xfaWriter.GetDateTimePattern(this.DatePattern, this.TimePattern)));
        xfaWriter.Write.WriteEndElement();
        xfaWriter.Write.WriteEndElement();
        break;
    }
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
    field.StringFormat.Alignment = this.ConvertToPdfTextAlignment(this.HorizontalAlignment);
    if (this.Font == null)
      field.Font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 11f, PdfFontStyle.Regular);
    else
      field.Font = this.Font;
    if (this.Border != null)
      this.Border.ApplyAcroBorder((PdfStyledField) field);
    if (this.ReadOnly || this.parent.ReadOnly)
      field.ReadOnly = true;
    if (this.Visibility == PdfXfaVisibility.Invisible)
      field.Visibility = PdfFormFieldVisibility.Hidden;
    if (this.isSet)
    {
      if (this.Format == PdfXfaDateTimeFormat.Date)
        field.Text = this.Value.ToString(this.GetDatePattern(this.DatePattern));
      else if (this.Format == PdfXfaDateTimeFormat.DateTime)
        field.Text = this.Value.ToString(this.GetDateTimePattern(this.DatePattern, this.TimePattern));
      else if (this.Format == PdfXfaDateTimeFormat.Time)
        field.Text = this.Value.ToString(this.GetTimePattern(this.TimePattern));
    }
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

  private string GetDatePattern(PdfXfaDatePattern pattern)
  {
    string datePattern = string.Empty;
    switch (pattern)
    {
      case PdfXfaDatePattern.Default:
        datePattern = "MMM d, yyyy";
        break;
      case PdfXfaDatePattern.Short:
        datePattern = "M/d/yyyy";
        break;
      case PdfXfaDatePattern.Medium:
        datePattern = "MMM d, yyyy";
        break;
      case PdfXfaDatePattern.Long:
        datePattern = "MMMM d, yyyy";
        break;
      case PdfXfaDatePattern.Full:
        datePattern = "dddd, MMMM dd, yyyy";
        break;
      case PdfXfaDatePattern.MDYY:
        datePattern = "M/d/yy";
        break;
      case PdfXfaDatePattern.MMMD_YYYY:
        datePattern = "MMM d, yyyy";
        break;
      case PdfXfaDatePattern.MMMMD_YYYY:
        datePattern = "MMMM d, yyyy";
        break;
      case PdfXfaDatePattern.EEEE_MMMMD_YYYY:
        datePattern = "dddd, MMMM d, yyyy";
        break;
      case PdfXfaDatePattern.MDYYYY:
        datePattern = "M/d/yyyy";
        break;
      case PdfXfaDatePattern.MMDDYY:
        datePattern = "MM/dd/yy";
        break;
      case PdfXfaDatePattern.MMDDYYYY:
        datePattern = "MM/dd/yyyy";
        break;
      case PdfXfaDatePattern.YYMMDD:
        datePattern = "yy/MM/dd";
        break;
      case PdfXfaDatePattern.YYYYMMDD:
        datePattern = "yyyy-MM-dd";
        break;
      case PdfXfaDatePattern.DDMMMYY:
        datePattern = "dd-MMM-yy";
        break;
      case PdfXfaDatePattern.EEEEMMMMDDYYYY:
        datePattern = "dddd, MMMM dd, yyyy";
        break;
      case PdfXfaDatePattern.MMMMDDYYYY:
        datePattern = "MMMM dd, yyyy}";
        break;
      case PdfXfaDatePattern.EEEEDDMMMMYYYY:
        datePattern = "dddd, dd MMMM, yyyy";
        break;
      case PdfXfaDatePattern.DDMMMMYYYY:
        datePattern = "dd MMMM, yyyy";
        break;
      case PdfXfaDatePattern.MMMMYYYY:
        datePattern = "MMMM, yyyy";
        break;
    }
    return datePattern;
  }

  private string GetTimePattern(PdfXfaTimePattern pattern)
  {
    string timePattern = string.Empty;
    switch (pattern)
    {
      case PdfXfaTimePattern.Default:
        timePattern = "h:mm:ss";
        break;
      case PdfXfaTimePattern.Short:
        timePattern = "t";
        break;
      case PdfXfaTimePattern.Medium:
        timePattern = "h:mm:ss";
        break;
      case PdfXfaTimePattern.Long:
        timePattern = "T";
        break;
      case PdfXfaTimePattern.Full:
        timePattern = "hh:mm:ss tt zzz";
        break;
      case PdfXfaTimePattern.H_MM_A:
        timePattern = "h:MM tt";
        break;
      case PdfXfaTimePattern.H_MM_SS_A:
        timePattern = "H:MM:ss tt";
        break;
      case PdfXfaTimePattern.H_MM_SS_A_Z:
        timePattern = "H:MM:ss tt z";
        break;
      case PdfXfaTimePattern.HH_MM_SS_A:
        timePattern = "hh:MM:ss tt";
        break;
      case PdfXfaTimePattern.H_MM_SS:
        timePattern = "H:MM:ss";
        break;
      case PdfXfaTimePattern.HH_MM_SS:
        timePattern = "HH:MM:ss";
        break;
    }
    return timePattern;
  }

  private string GetDateTimePattern(PdfXfaDatePattern d, PdfXfaTimePattern t)
  {
    return $"{this.GetDatePattern(d)} {this.GetTimePattern(t)}";
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
      xfaWriter.SetSize(this.Height, this.Width, 0.0f, 0.0f);
      this.Caption.Width = sizeF.Height;
    }
    else
    {
      if (this.Caption.Position != PdfXfaPosition.Left && this.Caption.Position != PdfXfaPosition.Right)
        return;
      xfaWriter.SetSize(this.Height, this.Width, 0.0f, 0.0f);
      this.Caption.Width = sizeF.Width;
    }
  }

  private string GetDatePattern()
  {
    string datePattern = (string) null;
    switch (this.Format)
    {
      case PdfXfaDateTimeFormat.Date:
        switch (this.DatePattern)
        {
          case PdfXfaDatePattern.Default:
            datePattern = (string) null;
            break;
          case PdfXfaDatePattern.Short:
            datePattern = "date.short{}";
            break;
          case PdfXfaDatePattern.Medium:
            datePattern = "date.medium{}";
            break;
          case PdfXfaDatePattern.Long:
            datePattern = "date.long{}";
            break;
          case PdfXfaDatePattern.Full:
            datePattern = "date.full{}";
            break;
          case PdfXfaDatePattern.MDYY:
            datePattern = "date{M/D/YY}";
            break;
          case PdfXfaDatePattern.MMMD_YYYY:
            datePattern = "date{MMM D, YYYY}";
            break;
          case PdfXfaDatePattern.MMMMD_YYYY:
            datePattern = "date{MMMM D, YYYY}";
            break;
          case PdfXfaDatePattern.EEEE_MMMMD_YYYY:
            datePattern = "date{EEEE, MMMM D, YYYY}";
            break;
          case PdfXfaDatePattern.MDYYYY:
            datePattern = "date{M/D/YYYY}";
            break;
          case PdfXfaDatePattern.MMDDYY:
            datePattern = "date{MM/DD/YY}";
            break;
          case PdfXfaDatePattern.MMDDYYYY:
            datePattern = "date{MM/DD/YYYY}";
            break;
          case PdfXfaDatePattern.YYMMDD:
            datePattern = "date{YY/MM/DD}";
            break;
          case PdfXfaDatePattern.YYYYMMDD:
            datePattern = "date{YYYY-MM-DD}";
            break;
          case PdfXfaDatePattern.DDMMMYY:
            datePattern = "date{DD-MMM-YY}";
            break;
          case PdfXfaDatePattern.EEEEMMMMDDYYYY:
            datePattern = "date{EEEE, MMMM DD, YYYY}";
            break;
          case PdfXfaDatePattern.MMMMDDYYYY:
            datePattern = "date{MMMM DD, YYYY}";
            break;
          case PdfXfaDatePattern.EEEEDDMMMMYYYY:
            datePattern = "date{EEEE, DD MMMM, YYYY}";
            break;
          case PdfXfaDatePattern.DDMMMMYYYY:
            datePattern = "date{DD MMMM, YYYY}";
            break;
          case PdfXfaDatePattern.MMMMYYYY:
            datePattern = "date{MMMM, YYYY}";
            break;
        }
        break;
      case PdfXfaDateTimeFormat.Time:
        switch (this.TimePattern)
        {
          case PdfXfaTimePattern.Default:
            datePattern = (string) null;
            break;
          case PdfXfaTimePattern.Short:
            datePattern = "time.short{}";
            break;
          case PdfXfaTimePattern.Medium:
            datePattern = "time.medium{}";
            break;
          case PdfXfaTimePattern.Long:
            datePattern = "time.long{}";
            break;
          case PdfXfaTimePattern.Full:
            datePattern = "time.full{}";
            break;
          case PdfXfaTimePattern.H_MM_A:
            datePattern = "time{h:MM A}";
            break;
          case PdfXfaTimePattern.H_MM_SS_A:
            datePattern = "time{H:MM:SS A}";
            break;
          case PdfXfaTimePattern.H_MM_SS_A_Z:
            datePattern = "time{H:MM:SS A Z}";
            break;
          case PdfXfaTimePattern.HH_MM_SS_A:
            datePattern = "time{hh:MM:SS A}";
            break;
          case PdfXfaTimePattern.H_MM_SS:
            datePattern = "time{H:MM:SS}";
            break;
          case PdfXfaTimePattern.HH_MM_SS:
            datePattern = "time{HH:MM:SS}";
            break;
        }
        break;
    }
    return datePattern;
  }

  public object Clone()
  {
    PdfXfaDateTimeField xfaDateTimeField = (PdfXfaDateTimeField) this.MemberwiseClone();
    xfaDateTimeField.Caption = this.Caption.Clone() as PdfXfaCaption;
    return (object) xfaDateTimeField;
  }
}
