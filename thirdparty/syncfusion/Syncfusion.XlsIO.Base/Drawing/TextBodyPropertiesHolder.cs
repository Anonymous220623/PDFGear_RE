// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.TextBodyPropertiesHolder
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO;
using System.Xml;

#nullable disable
namespace Syncfusion.Drawing;

internal class TextBodyPropertiesHolder
{
  private TextVertOverflowType m_textVertOverflowType;
  private TextHorzOverflowType m_textHorzOverflowType;
  private TextDirection m_textDirection;
  private double m_leftMarginPt = 7.2;
  private double m_topMarginPt = 3.6;
  private double m_rightMarginPt = 7.2;
  private double m_bottomMarginPt = 3.6;
  private bool m_wrapTextInShape = true;
  private TextFrameColumns m_columns;
  private ExcelVerticalAlignment m_verticalAlignment;
  private ExcelHorizontalAlignment m_horizontalAlignment;
  private bool m_isAutoSize;
  private bool m_isAutoMargins = true;
  private bool m_presetWrapTextInShape;

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

  internal double LeftMarginPt
  {
    get => this.m_leftMarginPt;
    set => this.m_leftMarginPt = value;
  }

  internal double TopMarginPt
  {
    get => this.m_topMarginPt;
    set => this.m_topMarginPt = value;
  }

  internal double RightMarginPt
  {
    get => this.m_rightMarginPt;
    set => this.m_rightMarginPt = value;
  }

  internal double BottomMarginPt
  {
    get => this.m_bottomMarginPt;
    set => this.m_bottomMarginPt = value;
  }

  internal bool WrapTextInShape
  {
    get => this.m_wrapTextInShape;
    set => this.m_wrapTextInShape = value;
  }

  internal ExcelVerticalAlignment VerticalAlignment
  {
    get => this.m_verticalAlignment;
    set => this.m_verticalAlignment = value;
  }

  internal ExcelHorizontalAlignment HorizontalAlignment
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

  internal bool PresetWrapTextInShape
  {
    get => this.m_presetWrapTextInShape;
    set => this.m_presetWrapTextInShape = value;
  }

  internal bool GetVerticalAnchorPosition(
    TextDirection textDirection,
    ExcelVerticalAlignment verticalAlignment,
    ExcelHorizontalAlignment horizontalAlignment,
    out string anchor)
  {
    anchor = "t";
    switch (textDirection)
    {
      case TextDirection.Horizontal:
        switch (verticalAlignment)
        {
          case ExcelVerticalAlignment.Top:
            anchor = "t";
            return false;
          case ExcelVerticalAlignment.Middle:
            anchor = "ctr";
            return false;
          case ExcelVerticalAlignment.Bottom:
            anchor = "b";
            return false;
          case ExcelVerticalAlignment.TopCentered:
            anchor = "t";
            return true;
          case ExcelVerticalAlignment.MiddleCentered:
            anchor = "ctr";
            return true;
          case ExcelVerticalAlignment.BottomCentered:
            anchor = "b";
            return true;
        }
        break;
      case TextDirection.RotateAllText90:
      case TextDirection.StackedRightToLeft:
        switch (horizontalAlignment)
        {
          case ExcelHorizontalAlignment.Left:
            anchor = "b";
            return false;
          case ExcelHorizontalAlignment.Center:
            anchor = "ctr";
            return false;
          case ExcelHorizontalAlignment.Right:
            anchor = "t";
            return false;
          case ExcelHorizontalAlignment.LeftMiddle:
            anchor = "b";
            return true;
          case ExcelHorizontalAlignment.CenterMiddle:
            anchor = "ctr";
            return true;
          case ExcelHorizontalAlignment.RightMiddle:
            anchor = "t";
            return true;
        }
        break;
      case TextDirection.RotateAllText270:
      case TextDirection.StackedLeftToRight:
        switch (horizontalAlignment)
        {
          case ExcelHorizontalAlignment.Left:
            anchor = "t";
            return false;
          case ExcelHorizontalAlignment.Center:
            anchor = "ctr";
            return false;
          case ExcelHorizontalAlignment.Right:
            anchor = "b";
            return false;
          case ExcelHorizontalAlignment.LeftMiddle:
            anchor = "t";
            return true;
          case ExcelHorizontalAlignment.CenterMiddle:
            anchor = "ctr";
            return true;
          case ExcelHorizontalAlignment.RightMiddle:
            anchor = "b";
            return true;
        }
        break;
    }
    return false;
  }

  internal bool GetHorizontalAnchorPostion(
    TextDirection textDirection,
    ExcelVerticalAlignment verticalAlignment,
    ExcelHorizontalAlignment horizontalAlignment,
    out string align)
  {
    align = "l";
    switch (textDirection)
    {
      case TextDirection.Horizontal:
        switch (this.HorizontalAlignment)
        {
          case ExcelHorizontalAlignment.Left:
            align = "l";
            return false;
          case ExcelHorizontalAlignment.Center:
            align = "ctr";
            return false;
          case ExcelHorizontalAlignment.Right:
            align = "r";
            return false;
          case ExcelHorizontalAlignment.LeftMiddle:
            align = "l";
            return true;
          case ExcelHorizontalAlignment.CenterMiddle:
            align = "ctr";
            return true;
          case ExcelHorizontalAlignment.RightMiddle:
            align = "r";
            return true;
        }
        break;
      case TextDirection.RotateAllText90:
      case TextDirection.StackedRightToLeft:
        switch (this.VerticalAlignment)
        {
          case ExcelVerticalAlignment.Top:
            align = "r";
            return false;
          case ExcelVerticalAlignment.Middle:
            align = "ctr";
            return false;
          case ExcelVerticalAlignment.Bottom:
            align = "l";
            return false;
          case ExcelVerticalAlignment.TopCentered:
            align = "r";
            return true;
          case ExcelVerticalAlignment.MiddleCentered:
            align = "ctr";
            return true;
          case ExcelVerticalAlignment.BottomCentered:
            align = "l";
            return true;
        }
        break;
      case TextDirection.RotateAllText270:
      case TextDirection.StackedLeftToRight:
        switch (verticalAlignment)
        {
          case ExcelVerticalAlignment.Top:
            align = "l";
            return false;
          case ExcelVerticalAlignment.Middle:
            align = "ctr";
            return false;
          case ExcelVerticalAlignment.Bottom:
            align = "r";
            return false;
          case ExcelVerticalAlignment.TopCentered:
            align = "t";
            return true;
          case ExcelVerticalAlignment.MiddleCentered:
            align = "ctr";
            return true;
          case ExcelVerticalAlignment.BottomCentered:
            align = "b";
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

  internal int GetLeftMargin() => (int) (this.m_leftMarginPt * 12700.0 + 0.5);

  internal void SetLeftMargin(int value) => this.m_leftMarginPt = (double) value / 12700.0;

  internal int GetTopMargin() => (int) (this.m_topMarginPt * 12700.0 + 0.5);

  internal void SetTopMargin(int value) => this.m_topMarginPt = (double) value / 12700.0;

  internal int GetRightMargin() => (int) (this.m_rightMarginPt * 12700.0 + 0.5);

  internal void SetRightMargin(int value) => this.m_rightMarginPt = (double) value / 12700.0;

  internal int GetBottomMargin() => (int) (this.m_bottomMarginPt * 12700.0 + 0.5);

  internal void SetBottomMargin(int value) => this.m_bottomMarginPt = (double) value / 12700.0;

  internal bool GetVerticalAnchorPosition(out string anchor)
  {
    return this.GetVerticalAnchorPosition(this.m_textDirection, this.m_verticalAlignment, this.m_horizontalAlignment, out anchor);
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
    bool verticalAnchorPosition = this.GetVerticalAnchorPosition(out anchor);
    if (this.TextDirection != TextDirection.Horizontal)
    {
      string textDirection = this.GetTextDirection(this.TextDirection);
      if (textDirection != null)
        xmlTextWriter.WriteAttributeString("vert", textDirection);
    }
    xmlTextWriter.WriteAttributeString("anchor", anchor);
    if (verticalAnchorPosition)
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

  internal TextBodyPropertiesHolder Clone() => (TextBodyPropertiesHolder) this.MemberwiseClone();
}
