// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.TextFrame
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart;

#nullable disable
namespace Syncfusion.Drawing;

internal class TextFrame : ITextFrame
{
  private bool wrapTextInShape = true;
  private bool isAutoMargins = true;
  private TextBodyPropertiesHolder m_textBodyProperties;
  private bool isTextOverFlow;
  private int marginLeftPt;
  private int topMarginPt;
  private int rightMarginPt;
  private int bottomMarginPt;
  private TextDirection textDirection;
  private OfficeVerticalAlignment verticalAlignment;
  private OfficeHorizontalAlignment horizontalAlignment;
  private TextVertOverflowType textVertOverflowType;
  private TextHorzOverflowType textHorzOverflowType;
  private ShapeImplExt shape;
  private bool isAutoSize;
  private Syncfusion.Drawing.TextRange m_textRange;
  internal TextFrameColumns Columns;

  internal TextFrame(ShapeImplExt shape) => this.shape = shape;

  public bool IsTextOverFlow
  {
    get => this.isTextOverFlow;
    set
    {
      this.isTextOverFlow = value;
      this.SetVisible();
    }
  }

  public bool WrapTextInShape
  {
    get => this.wrapTextInShape;
    set
    {
      this.wrapTextInShape = value;
      this.SetVisible();
    }
  }

  public int MarginLeftPt
  {
    get => this.marginLeftPt;
    set => this.marginLeftPt = value;
  }

  public int TopMarginPt
  {
    get => this.topMarginPt;
    set => this.topMarginPt = value;
  }

  public int RightMarginPt
  {
    get => this.rightMarginPt;
    set => this.rightMarginPt = value;
  }

  public int BottomMarginPt
  {
    get => this.bottomMarginPt;
    set => this.bottomMarginPt = value;
  }

  public bool IsAutoMargins
  {
    get => this.isAutoMargins;
    set
    {
      this.isAutoMargins = value;
      this.SetVisible();
    }
  }

  public bool IsAutoSize
  {
    get => this.isAutoSize;
    set
    {
      this.isAutoSize = value;
      this.SetVisible();
    }
  }

  public TextVertOverflowType TextVertOverflowType
  {
    get => this.textVertOverflowType;
    set
    {
      this.textVertOverflowType = value;
      this.SetVisible();
    }
  }

  public TextHorzOverflowType TextHorzOverflowType
  {
    get => this.textHorzOverflowType;
    set
    {
      this.textHorzOverflowType = value;
      this.SetVisible();
    }
  }

  public OfficeHorizontalAlignment HorizontalAlignment
  {
    get => this.horizontalAlignment;
    set
    {
      this.horizontalAlignment = value;
      this.SetVisible();
    }
  }

  public OfficeVerticalAlignment VerticalAlignment
  {
    get => this.verticalAlignment;
    set
    {
      this.verticalAlignment = value;
      this.SetVisible();
    }
  }

  public TextDirection TextDirection
  {
    get => this.textDirection;
    set
    {
      this.textDirection = value;
      this.SetVisible();
    }
  }

  public ITextRange TextRange
  {
    get
    {
      if (this.m_textRange == null)
        this.m_textRange = new Syncfusion.Drawing.TextRange(this, this.shape.Logger);
      return (ITextRange) this.m_textRange;
    }
  }

  internal TextFrame Clone(object parent)
  {
    TextFrame parent1 = (TextFrame) this.MemberwiseClone();
    this.shape = (ShapeImplExt) parent;
    if (this.m_textRange != null)
      parent1.m_textRange = this.m_textRange.Clone((object) parent1);
    return parent1;
  }

  internal TextBodyPropertiesHolder TextBodyProperties
  {
    get => this.m_textBodyProperties;
    set => this.m_textBodyProperties = value;
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

  internal int GetLeftMargin() => (int) ((double) this.marginLeftPt * 12700.0 + 0.5);

  internal void SetLeftMargin(int value) => this.marginLeftPt = (int) ((double) value / 12700.0);

  internal int GetTopMargin() => (int) ((double) this.topMarginPt * 12700.0 + 0.5);

  internal void SetTopMargin(int value) => this.topMarginPt = (int) ((double) value / 12700.0);

  internal int GetRightMargin() => (int) ((double) this.rightMarginPt * 12700.0 + 0.5);

  internal void SetRightMargin(int value) => this.rightMarginPt = (int) ((double) value / 12700.0);

  internal int GetBottomMargin() => (int) ((double) this.bottomMarginPt * 12700.0 + 0.5);

  internal void SetBottomMargin(int value)
  {
    this.bottomMarginPt = (int) ((double) value / 12700.0);
  }

  internal bool GetAnchorPosition(out string anchor)
  {
    return this.GetAnchorPosition(this.textDirection, this.verticalAlignment, this.horizontalAlignment, out anchor);
  }

  internal IWorkbook GetWorkbook() => this.shape.Worksheet.Workbook;

  internal void SetVisible() => this.shape.Logger.SetFlag(PreservedFlag.RichText);
}
