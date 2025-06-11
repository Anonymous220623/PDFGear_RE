// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaComboBoxField
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

public class PdfXfaComboBoxField : PdfXfaStyledField
{
  private bool m_allowTextEntry;
  private List<string> m_items = new List<string>();
  private int m_selectedIndex = -1;
  private string m_selectedValue = string.Empty;
  private PdfXfaCaption m_caption = new PdfXfaCaption();
  internal PdfPage m_page;
  internal RectangleF currentBounds = RectangleF.Empty;
  internal new PdfXfaForm parent;
  private object m_dataSource;
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

  public object DataSource
  {
    get => this.m_dataSource;
    set
    {
      if (value == null)
        return;
      this.m_dataSource = value;
      if (this.m_dataSource is List<string>)
      {
        this.Items = (List<string>) this.m_dataSource;
      }
      else
      {
        if (!(this.m_dataSource is string[]))
          return;
        foreach (string str in this.m_dataSource as string[])
          this.Items.Add(str);
      }
    }
  }

  public PdfXfaCaption Caption
  {
    get => this.m_caption;
    set => this.m_caption = value;
  }

  public List<string> Items
  {
    get => this.m_items;
    set
    {
      if (value == null)
        return;
      this.m_items = value;
    }
  }

  public int SelectedIndex
  {
    get => this.m_selectedIndex;
    set
    {
      if (value < 0 || value >= this.m_items.Count)
        return;
      this.m_selectedIndex = value;
    }
  }

  public string SelectedValue
  {
    get => this.m_selectedValue;
    set
    {
      if (value == null)
        return;
      this.m_selectedValue = value;
    }
  }

  public bool AllowTextEntry
  {
    get => this.m_allowTextEntry;
    set => this.m_allowTextEntry = value;
  }

  public PdfXfaComboBoxField(string name, SizeF size)
  {
    this.Height = size.Height;
    this.Width = size.Width;
    this.Name = name;
    this.Padding.Left = 3f;
    this.Padding.Right = 3f;
  }

  public PdfXfaComboBoxField(string name, SizeF size, List<string> items)
  {
    this.Height = size.Height;
    this.Width = size.Width;
    this.Name = name;
    this.Items = items;
    this.Padding.Left = 3f;
    this.Padding.Right = 3f;
  }

  public PdfXfaComboBoxField(string name, float width, float height)
  {
    this.Height = height;
    this.Width = width;
    this.Name = name;
    this.Padding.Left = 3f;
    this.Padding.Right = 3f;
  }

  public PdfXfaComboBoxField(string name, float width, float height, List<string> items)
  {
    this.Height = height;
    this.Width = width;
    this.Name = name;
    this.Items = items;
    this.Padding.Left = 3f;
    this.Padding.Right = 3f;
  }

  internal void Save(XfaWriter xfaWriter)
  {
    if (this.Name == "" || this.Name == string.Empty)
      this.Name = "comboBox" + xfaWriter.m_fieldCount.ToString();
    xfaWriter.Write.WriteStartElement("field");
    xfaWriter.Write.WriteAttributeString("name", this.Name);
    this.SetSize(xfaWriter);
    xfaWriter.SetRPR(this.Rotate, this.Visibility, this.ReadOnly);
    Dictionary<string, string> values = new Dictionary<string, string>();
    values.Add("open", "onEntry");
    if (this.AllowTextEntry)
      values.Add("textEntry", "1");
    xfaWriter.WriteUI("choiceList", values, this.Border, 0, this.Padding);
    xfaWriter.WriteListItems(this.Items, "1");
    if (this.m_selectedValue != null && this.m_selectedValue != string.Empty)
    {
      if (this.Items.Contains(this.m_selectedValue))
        xfaWriter.WriteValue(this.m_selectedValue, 0);
      else if (!this.AllowTextEntry)
        throw new PdfException("The selected value doesn't match.");
    }
    else if (this.m_selectedIndex > 0)
    {
      if (this.m_selectedIndex > this.Items.Count)
        throw new ArgumentOutOfRangeException("SelectedIndex");
      xfaWriter.WriteValue(this.Items[this.m_selectedIndex], 0);
    }
    this.SetMFTP(xfaWriter);
    if (this.Caption != null)
      this.Caption.Save(xfaWriter);
    xfaWriter.Write.WriteEndElement();
  }

  internal PdfField SaveAcroForm(PdfPage page, RectangleF bounds, string name)
  {
    PdfComboBoxField field = new PdfComboBoxField((PdfPageBase) page, name);
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
    foreach (string text in this.Items)
      field.Items.Add(new PdfListFieldItem(text, text));
    if (this.SelectedIndex != -1)
      field.SelectedIndex = this.SelectedIndex;
    if (this.SelectedValue != string.Empty && this.SelectedValue != null)
      field.SelectedIndex = this.Items.IndexOf(this.SelectedValue);
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
    if (this.Caption != null)
    {
      if (this.Caption.Font == null && this.Font != null)
        this.Caption.Font = this.Font;
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
    else
      xfaWriter.SetSize(this.Height, this.Width, 0.0f, 0.0f);
  }

  public object Clone()
  {
    PdfXfaComboBoxField xfaComboBoxField = (PdfXfaComboBoxField) this.MemberwiseClone();
    xfaComboBoxField.Caption = this.Caption.Clone() as PdfXfaCaption;
    return (object) xfaComboBoxField;
  }
}
