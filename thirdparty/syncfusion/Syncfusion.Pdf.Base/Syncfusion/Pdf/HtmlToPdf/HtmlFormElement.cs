// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.HtmlToPdf.HtmlFormElement
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Collections;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.HtmlToPdf;

internal class HtmlFormElement
{
  private string m_elementType;
  private RectangleF m_bounds;
  private string m_type;
  private string m_name;
  private string m_value;
  private bool m_checked;
  private string m_parentName;
  private int m_maxLength;
  private string m_align;
  private bool m_readOnly;
  private Color m_backRectColor;
  private Color m_backgroundColor;
  private Color m_textColor;
  private Color m_borderColor;
  private bool m_multiple;
  private int[] m_selectedindex;
  private System.Collections.Generic.List<ArrayList> m_list;

  internal string ElementType
  {
    get => this.m_elementType;
    set => this.m_elementType = value;
  }

  internal RectangleF Bounds
  {
    get => this.m_bounds;
    set => this.m_bounds = value;
  }

  internal string Type
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal string Value
  {
    get => this.m_value;
    set => this.m_value = value;
  }

  internal bool Checked
  {
    get => this.m_checked;
    set => this.m_checked = value;
  }

  internal string ParentName
  {
    get => this.m_parentName;
    set => this.m_parentName = value;
  }

  internal int MaxLength
  {
    get => this.m_maxLength;
    set => this.m_maxLength = value;
  }

  internal string Align
  {
    get => this.m_align;
    set => this.m_align = value;
  }

  internal bool ReadOnly
  {
    get => this.m_readOnly;
    set => this.m_readOnly = value;
  }

  internal Color BackRectColor
  {
    get => this.m_backRectColor;
    set => this.m_backRectColor = value;
  }

  internal Color BackgroundColor
  {
    get => this.m_backgroundColor;
    set => this.m_backgroundColor = value;
  }

  internal Color TextColor
  {
    get => this.m_textColor;
    set => this.m_textColor = value;
  }

  internal Color BorderColor
  {
    get => this.m_borderColor;
    set => this.m_borderColor = value;
  }

  internal System.Collections.Generic.List<ArrayList> List
  {
    get => this.m_list;
    set => this.m_list = value;
  }

  internal bool Multiple
  {
    get => this.m_multiple;
    set => this.m_multiple = value;
  }

  internal int[] SelectedIndex
  {
    get => this.m_selectedindex;
    set => this.m_selectedindex = value;
  }

  public HtmlFormElement()
  {
  }

  public HtmlFormElement(
    string ElementType,
    RectangleF Bounds,
    string Type,
    string Name,
    string Value,
    Color BackRectColor)
  {
    this.m_elementType = ElementType;
    this.m_bounds = Bounds;
    this.m_type = Type;
    this.m_name = Name;
    this.m_value = Value;
    this.m_backRectColor = BackRectColor;
    this.ConvertBoundsToPoint();
  }

  public HtmlFormElement(
    string ParentName,
    RectangleF Bounds,
    string Type,
    string Name,
    string Value,
    bool Checked,
    Color BackRectColor,
    string ElementType)
  {
    this.m_elementType = ElementType;
    this.m_bounds = Bounds;
    this.m_type = Type;
    this.m_name = Name;
    this.m_value = Value;
    this.m_checked = Checked;
    this.m_parentName = ParentName;
    this.m_backRectColor = BackRectColor;
    this.ConvertBoundsToPoint();
  }

  public HtmlFormElement(
    RectangleF Bounds,
    string Type,
    string Name,
    string Value,
    int MaxLength,
    string Align,
    bool ReadOnly,
    Color BackRectColor,
    Color BackgroundColor,
    Color TextColor,
    Color BorderColor,
    string ElementType)
  {
    this.m_elementType = ElementType;
    this.m_bounds = Bounds;
    this.m_type = Type;
    this.m_name = Name;
    this.m_value = Value;
    this.m_maxLength = MaxLength;
    this.m_align = Align;
    this.m_readOnly = ReadOnly;
    this.m_backRectColor = BackRectColor;
    this.m_backgroundColor = BackgroundColor;
    this.m_textColor = TextColor;
    this.m_borderColor = BorderColor;
    this.ConvertBoundsToPoint();
  }

  public HtmlFormElement(
    RectangleF Bounds,
    string Type,
    string Name,
    string Value,
    Color BackRectColor,
    string ElementType)
  {
    this.m_elementType = ElementType;
    this.m_bounds = Bounds;
    this.m_type = Type;
    this.m_name = Name;
    this.m_value = Value;
    this.m_backRectColor = BackRectColor;
    this.ConvertBoundsToPoint();
  }

  public HtmlFormElement(
    RectangleF Bounds,
    string Type,
    string Name,
    string Value,
    bool Checked,
    Color BackRectColor,
    string ElementType)
  {
    this.m_elementType = ElementType;
    this.m_bounds = Bounds;
    this.m_type = Type;
    this.m_name = Name;
    this.m_value = Value;
    this.m_checked = Checked;
    this.m_backRectColor = BackRectColor;
    this.ConvertBoundsToPoint();
  }

  public HtmlFormElement(
    RectangleF Bounds,
    string Type,
    string Name,
    bool ReadOnly,
    string Value,
    Color BackRectColor,
    Color BackgroundColor,
    Color TextColor,
    Color BorderColor,
    string Align,
    string ElementType)
  {
    this.m_elementType = ElementType;
    this.m_bounds = Bounds;
    this.m_type = Type;
    this.m_name = Name;
    this.m_value = Value;
    this.m_align = Align;
    this.m_readOnly = ReadOnly;
    this.m_backRectColor = BackRectColor;
    this.m_backgroundColor = BackgroundColor;
    this.m_textColor = TextColor;
    this.m_borderColor = BorderColor;
    this.ConvertBoundsToPoint();
  }

  public HtmlFormElement(
    string ElementType,
    string Type,
    string Name,
    string Value,
    RectangleF Bounds,
    bool Multiple,
    int[] SelectedIndex,
    System.Collections.Generic.List<ArrayList> List,
    Color BackRectColor)
  {
    this.m_elementType = ElementType;
    this.m_bounds = Bounds;
    this.m_type = Type;
    this.m_name = Name;
    this.m_value = Value;
    this.m_multiple = Multiple;
    this.m_selectedindex = SelectedIndex;
    this.m_list = List;
    this.m_backRectColor = BackRectColor;
    this.ConvertBoundsToPoint();
  }

  internal void ConvertBoundsToPoint()
  {
    this.m_bounds = new PdfUnitConvertor().ConvertFromPixels(this.m_bounds, PdfGraphicsUnit.Point);
  }
}
