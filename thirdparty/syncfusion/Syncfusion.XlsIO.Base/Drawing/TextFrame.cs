// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.TextFrame
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO;

#nullable disable
namespace Syncfusion.Drawing;

public class TextFrame : ITextFrame
{
  private bool isTextOverFlow;
  private ShapeImplExt shape;
  private Syncfusion.Drawing.TextRange m_textRange;
  private TextBodyPropertiesHolder m_textBodyProperties;

  internal TextFrame(ShapeImplExt shape)
  {
    this.shape = shape;
    this.m_textBodyProperties = new TextBodyPropertiesHolder();
  }

  public bool IsTextOverFlow
  {
    get => this.isTextOverFlow;
    set
    {
      this.isTextOverFlow = value;
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

  public bool WrapTextInShape
  {
    get => this.m_textBodyProperties.WrapTextInShape;
    set
    {
      this.m_textBodyProperties.WrapTextInShape = value;
      this.SetVisible();
    }
  }

  public int MarginLeftPt
  {
    get => (int) this.m_textBodyProperties.LeftMarginPt;
    set => this.m_textBodyProperties.LeftMarginPt = (double) value;
  }

  public int TopMarginPt
  {
    get => (int) this.m_textBodyProperties.TopMarginPt;
    set => this.m_textBodyProperties.TopMarginPt = (double) value;
  }

  public int RightMarginPt
  {
    get => (int) this.m_textBodyProperties.RightMarginPt;
    set => this.m_textBodyProperties.RightMarginPt = (double) value;
  }

  public int BottomMarginPt
  {
    get => (int) this.m_textBodyProperties.BottomMarginPt;
    set => this.m_textBodyProperties.BottomMarginPt = (double) value;
  }

  public bool IsAutoMargins
  {
    get => this.m_textBodyProperties.IsAutoMargins;
    set
    {
      this.m_textBodyProperties.IsAutoMargins = value;
      this.SetVisible();
    }
  }

  public bool IsAutoSize
  {
    get => this.m_textBodyProperties.IsAutoSize;
    set
    {
      this.m_textBodyProperties.IsAutoSize = value;
      this.SetVisible();
    }
  }

  public TextVertOverflowType TextVertOverflowType
  {
    get => this.m_textBodyProperties.TextVertOverflowType;
    set
    {
      this.m_textBodyProperties.TextVertOverflowType = value;
      this.SetVisible();
    }
  }

  public TextHorzOverflowType TextHorzOverflowType
  {
    get => this.m_textBodyProperties.TextHorzOverflowType;
    set
    {
      this.m_textBodyProperties.TextHorzOverflowType = value;
      this.SetVisible();
    }
  }

  public ExcelHorizontalAlignment HorizontalAlignment
  {
    get => this.m_textBodyProperties.HorizontalAlignment;
    set
    {
      this.m_textBodyProperties.HorizontalAlignment = value;
      this.SetVisible();
    }
  }

  public ExcelVerticalAlignment VerticalAlignment
  {
    get => this.m_textBodyProperties.VerticalAlignment;
    set
    {
      this.m_textBodyProperties.VerticalAlignment = value;
      this.SetVisible();
    }
  }

  public TextDirection TextDirection
  {
    get => this.m_textBodyProperties.TextDirection;
    set
    {
      this.m_textBodyProperties.TextDirection = value;
      this.SetVisible();
    }
  }

  internal TextBodyPropertiesHolder TextBodyProperties
  {
    get => this.m_textBodyProperties;
    set => this.m_textBodyProperties = value;
  }

  internal IWorkbook GetWorkbook()
  {
    return this.shape.Worksheet != null ? this.shape.Worksheet.Workbook : this.shape.ParentSheet.Workbook;
  }

  internal void SetVisible() => this.shape.Logger.SetFlag(PreservedFlag.RichText);

  internal TextFrame Clone(object parent)
  {
    TextFrame parent1 = (TextFrame) this.MemberwiseClone();
    this.shape = (ShapeImplExt) parent;
    if (this.m_textRange != null)
      parent1.m_textRange = this.m_textRange.Clone((object) parent1);
    return parent1;
  }
}
