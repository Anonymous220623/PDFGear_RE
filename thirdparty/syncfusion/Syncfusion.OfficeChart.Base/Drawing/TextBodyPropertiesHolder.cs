// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.TextBodyPropertiesHolder
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart;
using System.Xml;

#nullable disable
namespace Syncfusion.Drawing;

internal class TextBodyPropertiesHolder
{
  private TextVertOverflowType m_textVertOverflowType;
  private TextHorzOverflowType m_textHorzOverflowType;
  private TextDirection m_textDirection;
  private int m_leftMarginPt;
  private int m_topMarginPt;
  private int m_rightMarginPt;
  private int m_bottomMarginPt;
  private bool m_wrapTextInShape = true;
  private TextFrameColumns m_columns;
  private OfficeVerticalAlignment m_verticalAlignment;
  private OfficeHorizontalAlignment m_horizontalAlignment;
  private bool m_isAutoSize;
  private bool m_isAutoMargins = true;

  internal TextVertOverflowType TextVertOverflowType
  {
    get => this.m_textVertOverflowType;
    set => this.m_textVertOverflowType = value;
  }

  internal TextHorzOverflowType TextHorzOverflowType
  {
    get => this.m_textHorzOverflowType;
    set => this.m_textHorzOverflowType = value;
  }

  internal TextDirection TextDirection
  {
    get => this.m_textDirection;
    set => this.m_textDirection = value;
  }

  internal int LeftMarginPt
  {
    get => this.m_leftMarginPt;
    set => this.m_leftMarginPt = value;
  }

  internal int TopMarginPt
  {
    get => this.m_topMarginPt;
    set => this.m_topMarginPt = value;
  }

  internal int RightMarginPt
  {
    get => this.m_rightMarginPt;
    set => this.m_rightMarginPt = value;
  }

  internal int BottomMarginPt
  {
    get => this.m_bottomMarginPt;
    set => this.m_bottomMarginPt = value;
  }

  internal bool WrapTextInShape
  {
    get => this.m_wrapTextInShape;
    set => this.m_wrapTextInShape = value;
  }

  internal OfficeVerticalAlignment VerticalAlignment
  {
    get => this.m_verticalAlignment;
    set => this.m_verticalAlignment = value;
  }

  internal OfficeHorizontalAlignment HorizontalAlignment
  {
    get => this.m_horizontalAlignment;
    set => this.m_horizontalAlignment = value;
  }

  internal bool IsAutoSize
  {
    get => this.m_isAutoSize;
    set => this.m_isAutoSize = value;
  }

  internal bool IsAutoMargins
  {
    get => this.m_isAutoMargins;
    set => this.m_isAutoMargins = value;
  }

  internal int Number
  {
    get => this.m_columns.Number;
    set => this.m_columns.Number = value;
  }

  internal int SpacingPt
  {
    get => this.m_columns.SpacingPt;
    set => this.m_columns.SpacingPt = value;
  }

  internal bool GetAnchorPosition(
    TextDirection textDirection,
    OfficeVerticalAlignment verticalAlignment,
    OfficeHorizontalAlignment horizontalAlignment,
    out string anchor)
  {
    anchor = "t";
    switch (textDirection)
    {
      case TextDirection.Horizontal:
        switch (verticalAlignment)
        {
          case OfficeVerticalAlignment.Top:
            anchor = "t";
            return false;
          case OfficeVerticalAlignment.Middle:
            anchor = "ctr";
            return false;
          case OfficeVerticalAlignment.Bottom:
            anchor = "b";
            return false;
          case OfficeVerticalAlignment.TopCentered:
            anchor = "t";
            return true;
          case OfficeVerticalAlignment.MiddleCentered:
            anchor = "ctr";
            return true;
          case OfficeVerticalAlignment.BottomCentered:
            anchor = "b";
            return true;
        }
        break;
      case TextDirection.RotateAllText90:
      case TextDirection.StackedRightToLeft:
        switch (horizontalAlignment)
        {
          case OfficeHorizontalAlignment.Left:
            anchor = "b";
            return false;
          case OfficeHorizontalAlignment.Center:
            anchor = "ctr";
            return false;
          case OfficeHorizontalAlignment.Right:
            anchor = "t";
            return false;
          case OfficeHorizontalAlignment.LeftMiddle:
            anchor = "b";
            return true;
          case OfficeHorizontalAlignment.CenterMiddle:
            anchor = "ctr";
            return true;
          case OfficeHorizontalAlignment.RightMiddle:
            anchor = "t";
            return true;
        }
        break;
      case TextDirection.RotateAllText270:
      case TextDirection.StackedLeftToRight:
        switch (horizontalAlignment)
        {
          case OfficeHorizontalAlignment.Left:
            anchor = "t";
            return false;
          case OfficeHorizontalAlignment.Center:
            anchor = "ctr";
            return false;
          case OfficeHorizontalAlignment.Right:
            anchor = "b";
            return false;
          case OfficeHorizontalAlignment.LeftMiddle:
            anchor = "t";
            return true;
          case OfficeHorizontalAlignment.CenterMiddle:
            anchor = "ctr";
            return true;
          case OfficeHorizontalAlignment.RightMiddle:
            anchor = "b";
            return true;
        }
        break;
    }
    return false;
  }

  internal string GetTextDirection(TextDirection textDirection)
  {
    switch (textDirection)
    {
      case TextDirection.Horizontal:
        return "horz";
      case TextDirection.RotateAllText90:
        return "vert";
      case TextDirection.RotateAllText270:
        return "vert270";
      case TextDirection.StackedLeftToRight:
        return "wordArtVert";
      case TextDirection.StackedRightToLeft:
        return "wordArtVertRtl";
      default:
        return "horz";
    }
  }

  internal int GetLeftMargin() => (int) ((double) this.m_leftMarginPt * 12700.0 + 0.5);

  internal void SetLeftMargin(int value) => this.m_leftMarginPt = (int) ((double) value / 12700.0);

  internal int GetTopMargin() => (int) ((double) this.m_topMarginPt * 12700.0 + 0.5);

  internal void SetTopMargin(int value) => this.m_topMarginPt = (int) ((double) value / 12700.0);

  internal int GetRightMargin() => (int) ((double) this.m_rightMarginPt * 12700.0 + 0.5);

  internal void SetRightMargin(int value)
  {
    this.m_rightMarginPt = (int) ((double) value / 12700.0);
  }

  internal int GetBottomMargin() => (int) ((double) this.m_bottomMarginPt * 12700.0 + 0.5);

  internal void SetBottomMargin(int value)
  {
    this.m_bottomMarginPt = (int) ((double) value / 12700.0);
  }

  internal bool GetAnchorPosition(out string anchor)
  {
    return this.GetAnchorPosition(this.m_textDirection, this.m_verticalAlignment, this.m_horizontalAlignment, out anchor);
  }

  internal void SerialzieTextBodyProperties(
    XmlWriter xmlTextWriter,
    string prefix,
    string nameSpace)
  {
    xmlTextWriter.WriteStartElement(prefix, "bodyPr", nameSpace);
    if (this.TextVertOverflowType != TextVertOverflowType.OverFlow)
      xmlTextWriter.WriteAttributeString("vertOverflow", Helper.GetVerticalFlowType(this.TextVertOverflowType));
    if (this.TextHorzOverflowType != TextHorzOverflowType.OverFlow)
      xmlTextWriter.WriteAttributeString("horzOverflow", Helper.GetHorizontalFlowType(this.TextHorzOverflowType));
    string str = "square";
    if (!this.WrapTextInShape)
      str = "none";
    xmlTextWriter.WriteAttributeString("wrap", str);
    if (!this.IsAutoMargins)
    {
      xmlTextWriter.WriteAttributeString("lIns", Helper.ToString(this.GetLeftMargin()));
      xmlTextWriter.WriteAttributeString("tIns", Helper.ToString(this.GetTopMargin()));
      xmlTextWriter.WriteAttributeString("rIns", Helper.ToString(this.GetRightMargin()));
      xmlTextWriter.WriteAttributeString("bIns", Helper.ToString(this.GetBottomMargin()));
    }
    string anchor = "t";
    bool anchorPosition = this.GetAnchorPosition(out anchor);
    if (this.TextDirection != TextDirection.Horizontal)
    {
      string textDirection = this.GetTextDirection(this.TextDirection);
      if (textDirection != null)
        xmlTextWriter.WriteAttributeString("vert", textDirection);
    }
    xmlTextWriter.WriteAttributeString("anchor", anchor);
    if (anchorPosition)
      xmlTextWriter.WriteAttributeString("anchorCtr", "1");
    else
      xmlTextWriter.WriteAttributeString("anchorCtr", "0");
    if (this.IsAutoSize)
      xmlTextWriter.WriteElementString("a", "spAutoFit", "http://schemas.openxmlformats.org/drawingml/2006/main", (string) null);
    if (this.Number > 0)
      xmlTextWriter.WriteAttributeString("numCol", Helper.ToString(this.Number));
    int num = (int) ((double) this.SpacingPt * 12700.0 + 0.5);
    if (num > 0)
      xmlTextWriter.WriteAttributeString("spcCol", Helper.ToString(num));
    xmlTextWriter.WriteEndElement();
  }
}
