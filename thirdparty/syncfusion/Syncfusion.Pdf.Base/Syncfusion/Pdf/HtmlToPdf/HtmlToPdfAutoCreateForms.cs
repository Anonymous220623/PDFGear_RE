// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.HtmlToPdf.HtmlToPdfAutoCreateForms
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.HtmlToPdf;

internal class HtmlToPdfAutoCreateForms
{
  private string m_elementId;
  private string m_elementValue;
  private bool m_isReadOnly;
  private bool m_isSelected;
  private string m_elementType;
  private int m_elementPageNo;
  private RectangleF m_elementBounds;
  private string m_optionValue;

  internal string OptionValue
  {
    get => this.m_optionValue;
    set => this.m_optionValue = value;
  }

  internal string ElementId
  {
    get => this.m_elementId;
    set => this.m_elementId = value;
  }

  internal string ElementValue
  {
    get => this.m_elementValue;
    set => this.m_elementValue = value;
  }

  internal bool IsReadOnly
  {
    get => this.m_isReadOnly;
    set => this.m_isReadOnly = value;
  }

  internal bool IsSelected
  {
    get => this.m_isSelected;
    set => this.m_isSelected = value;
  }

  internal string ElementType
  {
    get => this.m_elementType;
    set => this.m_elementType = value;
  }

  internal int ElementPageNo
  {
    get => this.m_elementPageNo;
    set => this.m_elementPageNo = value;
  }

  internal RectangleF ElementBounds
  {
    get => this.m_elementBounds;
    set => this.m_elementBounds = value;
  }

  internal HtmlToPdfAutoCreateForms(
    string id,
    string value,
    bool isReadonly,
    bool selected,
    string type,
    int pageNo,
    RectangleF bounds,
    string optionValue)
  {
    this.m_elementId = id;
    this.m_elementValue = value;
    this.m_isReadOnly = isReadonly;
    this.m_isSelected = selected;
    this.m_elementType = type;
    this.m_elementPageNo = pageNo;
    this.m_elementBounds = bounds;
    this.m_optionValue = optionValue;
  }
}
