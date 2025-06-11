// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.RichText.TextBody
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.Layouting;
using Syncfusion.Presentation.SlideImplementation;
using Syncfusion.Presentation.TableImplementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.RichText;

internal class TextBody : ITextBody
{
  private Shape _baseShape;
  private int _fontScale;
  private int _noOfCol;
  private bool _isAutoMargins;
  private bool _isAutoSize;
  private bool _isWrapText;
  private bool _anchorCenter;
  private int _lineSpcRedValue;
  private double _marginBottom;
  private double _marginLeft;
  private double _marginRight;
  private double _marginTop;
  private bool _isRotationSet;
  private int _rotation;
  private Syncfusion.Presentation.RichText.Paragraphs _paragraphCollection;
  private Dictionary<string, Paragraph> _styleList;
  private TextOverflowType _txtHorizontalFlowType;
  private Syncfusion.Presentation.VerticalAlignment _txtVerticalAlignment;
  private TextOverflowType _txtVerticalFlowType;
  private Syncfusion.Presentation.TextDirection _txtDirectionType;
  private BaseSlide _baseSlide;
  private bool _isVAlignTypeChanged;
  private Cell _cell;
  private Syncfusion.Presentation.Presentation _presentation;
  private AutoMarginType _autoFitType;
  private Dictionary<string, Stream> _preservedElements;
  private bool _isFontScaleAccessed;
  private bool _hasFontScale;
  private double _spaceBetweenColumns;
  private bool _rtlColumns;
  private bool _isFitTextOptionChanged;

  internal TextBody(Shape shape)
  {
    this._baseShape = shape;
    this._baseSlide = this._baseShape.BaseSlide;
    this._presentation = this._baseSlide.Presentation;
    this._paragraphCollection = new Syncfusion.Presentation.RichText.Paragraphs(this);
    this._isWrapText = true;
    this.InitializeDefaultMargins();
  }

  internal TextBody(BaseSlide baseSlide)
  {
    this._baseSlide = baseSlide;
    this._presentation = this._baseSlide.Presentation;
    this._paragraphCollection = new Syncfusion.Presentation.RichText.Paragraphs(this);
    this._isWrapText = true;
    this.InitializeDefaultMargins();
  }

  internal TextBody(Cell cell)
  {
    this._cell = cell;
    this._baseShape = (Shape) cell.Table;
    this._baseSlide = this._baseShape.BaseSlide;
    this._presentation = this._baseSlide.Presentation;
    this._paragraphCollection = new Syncfusion.Presentation.RichText.Paragraphs(this);
    this._isWrapText = true;
    this.InitializeDefaultMargins();
  }

  internal TextBody(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
    this._paragraphCollection = new Syncfusion.Presentation.RichText.Paragraphs(this);
    this._isWrapText = true;
    this.InitializeDefaultMargins();
  }

  internal int Rotation
  {
    get => this._rotation;
    set
    {
      this._rotation = value / 60000;
      this._isRotationSet = true;
    }
  }

  internal AutoMarginType AutoFitType
  {
    get => this._autoFitType;
    set => this._autoFitType = value;
  }

  internal bool IsAutoMargins
  {
    get
    {
      return this._marginBottom == (double) int.MinValue && this._marginRight == (double) int.MinValue && this._marginLeft == (double) int.MinValue && this._marginTop == (double) int.MinValue || this._isAutoMargins;
    }
    set => this._isAutoMargins = value;
  }

  internal Cell Cell => this._cell;

  internal bool IsAutoSize
  {
    get => this._isAutoSize;
    set => this._isAutoSize = value;
  }

  internal int NumberOfColumns
  {
    get => this._noOfCol;
    set => this._noOfCol = value;
  }

  internal double SpaceBetweenColumns
  {
    get => this._spaceBetweenColumns;
    set => this._spaceBetweenColumns = value;
  }

  internal bool RTLColumns
  {
    get => this._rtlColumns;
    set => this._rtlColumns = value;
  }

  public IParagraph AddParagraph()
  {
    IParagraph paragraph1 = (IParagraph) new Paragraph(this._paragraphCollection);
    if (this.Cell != null)
    {
      if (this._paragraphCollection.Count > 0)
      {
        IParagraph paragraph2 = this._paragraphCollection[this._paragraphCollection.Count - 1];
        (paragraph2 as Paragraph).SetIsLastPara(false);
        if ((paragraph2 as Paragraph).GetEndParaProps() != null)
          (paragraph1 as Paragraph).SetEndParaProps((paragraph2 as Paragraph).GetEndParaProps());
        else if (paragraph2.TextParts.Count > 0)
        {
          ITextPart textPart = paragraph2.TextParts[paragraph2.TextParts.Count - 1];
          (paragraph1 as Paragraph).SetEndParaProps(textPart.Font);
        }
      }
      (paragraph1 as Paragraph).SetIsLastPara(true);
    }
    this._paragraphCollection.Add(paragraph1);
    return paragraph1;
  }

  public IParagraph AddParagraph(string text)
  {
    if (this.FitTextOption == FitTextOption.ShrinkTextOnOverFlow)
      this.SetFitTextOptionChanged(true);
    IParagraph paragraph1;
    if (this.Cell != null && this.Cell.IsInsertedCell)
    {
      paragraph1 = this.Cell.TextBody.Paragraphs[this.Cell.TextBody.Paragraphs.Count - 1];
      ITextPart textPart = paragraph1.AddTextPart(text);
      if (this.Cell.PrevCellTextPartFont != null)
      {
        (textPart.Font as Font).CopyFont(this.Cell.PrevCellTextPartFont);
        (textPart.Font as Font).HasBoldValue = (this.Cell.PrevCellTextPartFont as Font).HasBoldValue;
      }
      this.Cell.IsInsertedCell = false;
    }
    else
    {
      paragraph1 = (IParagraph) new Paragraph(this._paragraphCollection);
      ITextPart textPart1 = (ITextPart) new TextPart((Paragraph) paragraph1, text);
      if (this.Cell != null)
      {
        if (this._paragraphCollection.Count > 0)
        {
          IParagraph paragraph2 = this._paragraphCollection[this._paragraphCollection.Count - 1];
          (paragraph2 as Paragraph).SetIsLastPara(false);
          if ((paragraph2 as Paragraph).GetEndParaProps() != null)
            (paragraph1 as Paragraph).SetEndParaProps((paragraph2 as Paragraph).GetEndParaProps());
          if (paragraph2.TextParts.Count > 0)
          {
            ITextPart textPart2 = paragraph2.TextParts[paragraph2.TextParts.Count - 1];
            (textPart1.Font as Font).CopyFont(textPart2.Font);
          }
        }
        (paragraph1 as Paragraph).SetIsLastPara(true);
      }
      this._paragraphCollection.Add(paragraph1);
      ((TextParts) paragraph1.TextParts).Add(textPart1);
    }
    return paragraph1;
  }

  public double MarginBottom
  {
    get => Helper.EmuToPoint(this._marginBottom);
    set
    {
      this._marginBottom = value >= 0.0 && value <= 1584.0 ? (double) Helper.PointToEmu(value) : throw new ArgumentException("Invalid Margin Bottom " + value.ToString());
      this._isAutoMargins = false;
    }
  }

  internal double GetDefaultBottomMargin()
  {
    if (this._marginBottom != (double) int.MinValue)
      return Helper.EmuToPoint(this._marginBottom);
    return this._baseShape.DrawingType == DrawingType.PlaceHolder ? this.DefaultBottomMargin() : Helper.EmuToPoint(45720);
  }

  private double DefaultBottomMargin()
  {
    int num = int.MinValue;
    if (this._baseShape.BaseSlide is Slide)
    {
      Slide baseSlide = (Slide) this._baseShape.BaseSlide;
      string layoutIndex = baseSlide.ObtainLayoutIndex();
      if (layoutIndex != null)
      {
        LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
        foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
        {
          if (shape.PlaceholderFormat != null)
          {
            if (this._baseShape.PlaceholderFormat != null)
            {
              if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat))
              {
                num = ((TextBody) shape.TextBody).GetBottomMargin();
                break;
              }
            }
            else
              break;
          }
        }
        if (num == int.MinValue)
        {
          foreach (Shape shape in (IEnumerable<ISlideItem>) ((BaseSlide) layoutSlide.MasterSlide).Shapes)
          {
            if (shape.PlaceholderFormat != null)
            {
              if (this._baseShape.PlaceholderFormat != null)
              {
                if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat, true))
                {
                  num = ((TextBody) shape.TextBody).GetBottomMargin();
                  break;
                }
              }
              else
                break;
            }
          }
        }
      }
    }
    return Helper.EmuToPoint(num == int.MinValue ? 45720 : num);
  }

  public double MarginLeft
  {
    get => Helper.EmuToPoint(this._marginLeft);
    set
    {
      this._marginLeft = value >= 0.0 && value <= 1584.0 ? (double) Helper.PointToEmu(value) : throw new ArgumentException("Invalid Margin Left " + value.ToString());
      this._isAutoMargins = false;
    }
  }

  internal double GetDefaultLeftMargin()
  {
    if (this._marginLeft != (double) int.MinValue)
      return Helper.EmuToPoint(this._marginLeft);
    return this._baseShape.DrawingType == DrawingType.PlaceHolder ? this.DefaultLeftMargin() : Helper.EmuToPoint(91440);
  }

  private double DefaultLeftMargin()
  {
    int num = int.MinValue;
    if (this._baseShape.BaseSlide is Slide)
    {
      Slide baseSlide = (Slide) this._baseShape.BaseSlide;
      string layoutIndex = baseSlide.ObtainLayoutIndex();
      if (layoutIndex != null)
      {
        LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
        foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
        {
          if (shape.PlaceholderFormat != null)
          {
            if (this._baseShape.PlaceholderFormat != null)
            {
              if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat))
              {
                num = ((TextBody) shape.TextBody).GetLeftMargin();
                break;
              }
            }
            else
              break;
          }
        }
        if (num == int.MinValue)
        {
          foreach (Shape shape in (IEnumerable<ISlideItem>) ((BaseSlide) layoutSlide.MasterSlide).Shapes)
          {
            if (shape.PlaceholderFormat != null)
            {
              if (this._baseShape.PlaceholderFormat != null)
              {
                if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat, true))
                {
                  num = ((TextBody) shape.TextBody).GetLeftMargin();
                  break;
                }
              }
              else
                break;
            }
          }
        }
      }
    }
    return Helper.EmuToPoint(num == int.MinValue ? 91440 : num);
  }

  public double MarginRight
  {
    get => Helper.EmuToPoint(this._marginRight);
    set
    {
      this._marginRight = value >= 0.0 && value <= 1584.0 ? (double) Helper.PointToEmu(value) : throw new ArgumentException("Invalid Margin Right " + value.ToString());
      this._isAutoMargins = false;
    }
  }

  internal double GetDefaultRightMargin()
  {
    if (this._marginRight != (double) int.MinValue)
      return Helper.EmuToPoint(this._marginRight);
    return this._baseShape.DrawingType == DrawingType.PlaceHolder ? this.DefaultRightMargin() : Helper.EmuToPoint(91440);
  }

  private double DefaultRightMargin()
  {
    int num = int.MinValue;
    if (this._baseShape.BaseSlide is Slide)
    {
      Slide baseSlide = (Slide) this._baseShape.BaseSlide;
      string layoutIndex = baseSlide.ObtainLayoutIndex();
      if (layoutIndex != null)
      {
        LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
        foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
        {
          if (shape.PlaceholderFormat != null)
          {
            if (this._baseShape.PlaceholderFormat != null)
            {
              if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat))
              {
                num = ((TextBody) shape.TextBody).GetRightMargin();
                break;
              }
            }
            else
              break;
          }
        }
        if (num == int.MinValue)
        {
          foreach (Shape shape in (IEnumerable<ISlideItem>) ((BaseSlide) layoutSlide.MasterSlide).Shapes)
          {
            if (shape.PlaceholderFormat != null)
            {
              if (this._baseShape.PlaceholderFormat != null)
              {
                if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat, true))
                {
                  num = ((TextBody) shape.TextBody).GetRightMargin();
                  break;
                }
              }
              else
                break;
            }
          }
        }
      }
    }
    return Helper.EmuToPoint(num == int.MinValue ? 91440 : num);
  }

  public double MarginTop
  {
    get => Helper.EmuToPoint(this._marginTop);
    set
    {
      this._marginTop = value >= 0.0 && value <= 1584.0 ? (double) Helper.PointToEmu(value) : throw new ArgumentException("Invalid Margin Top " + value.ToString());
      this._isAutoMargins = false;
    }
  }

  internal double GetDefaultTopMargin()
  {
    if (this._marginTop != (double) int.MinValue)
      return Helper.EmuToPoint(this._marginTop);
    return this._baseShape.DrawingType == DrawingType.PlaceHolder ? this.DefaultTopMargin() : Helper.EmuToPoint(45720);
  }

  private double DefaultTopMargin()
  {
    int num = int.MinValue;
    if (this._baseShape.BaseSlide is Slide)
    {
      Slide baseSlide = (Slide) this._baseShape.BaseSlide;
      string layoutIndex = baseSlide.ObtainLayoutIndex();
      if (layoutIndex != null)
      {
        LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
        foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
        {
          if (shape.PlaceholderFormat != null)
          {
            if (this._baseShape.PlaceholderFormat != null)
            {
              if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat))
              {
                num = ((TextBody) shape.TextBody).GetTopMargin();
                break;
              }
            }
            else
              break;
          }
        }
        if (num == int.MinValue)
        {
          foreach (Shape shape in (IEnumerable<ISlideItem>) ((BaseSlide) layoutSlide.MasterSlide).Shapes)
          {
            if (shape.PlaceholderFormat != null)
            {
              if (this._baseShape.PlaceholderFormat != null)
              {
                if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat, true))
                {
                  num = ((TextBody) shape.TextBody).GetTopMargin();
                  break;
                }
              }
              else
                break;
            }
          }
        }
      }
    }
    return Helper.EmuToPoint(num == int.MinValue ? 45720 : num);
  }

  public IParagraphs Paragraphs
  {
    get
    {
      return (IParagraphs) this._paragraphCollection ?? (IParagraphs) (this._paragraphCollection = new Syncfusion.Presentation.RichText.Paragraphs(this));
    }
  }

  public string Text
  {
    get => this._paragraphCollection.Text;
    set
    {
      if (this.FitTextOption == FitTextOption.ShrinkTextOnOverFlow)
        this.SetFitTextOptionChanged(true);
      if (value == null)
        return;
      this._paragraphCollection.Text = value;
    }
  }

  public bool WrapText
  {
    get => this._isWrapText;
    set => this._isWrapText = value;
  }

  public FitTextOption FitTextOption
  {
    get => this.GetFitTextOption(this._autoFitType);
    set
    {
      switch (value)
      {
        case FitTextOption.DoNotAutoFit:
          this._autoFitType = AutoMarginType.NoAutoFit;
          if (!this.IsAutoSize)
            break;
          this.IsAutoSize = false;
          break;
        case FitTextOption.ShrinkTextOnOverFlow:
          this.IsAutoSize = false;
          this._autoFitType = AutoMarginType.NormalAutoFit;
          this._isFitTextOptionChanged = true;
          break;
        case FitTextOption.ResizeShapeToFitText:
          throw new Exception("Syncfusion Presentation library do not support ResizeShapeToFitText option");
      }
    }
  }

  internal bool HasFontScale
  {
    get => this._hasFontScale;
    set => this._hasFontScale = value;
  }

  internal Syncfusion.Presentation.Presentation Presentation => this._presentation;

  internal Dictionary<string, Stream> PreservedElements
  {
    get => this._preservedElements ?? (this._preservedElements = new Dictionary<string, Stream>());
  }

  internal TextOverflowType TextHorizontalOverflow
  {
    get => this._txtHorizontalFlowType;
    set => this._txtHorizontalFlowType = value;
  }

  public bool AnchorCenter
  {
    get => this._anchorCenter;
    set => this._anchorCenter = value;
  }

  internal int GetDefaultRotation()
  {
    if (this._isRotationSet || this._baseShape.DrawingType != DrawingType.PlaceHolder)
      return this._rotation;
    int defaultRotation = 0;
    if (this._baseShape.BaseSlide is Slide)
    {
      Slide baseSlide = (Slide) this._baseShape.BaseSlide;
      string layoutIndex = baseSlide.ObtainLayoutIndex();
      if (layoutIndex != null)
      {
        LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
        foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
        {
          if (shape.PlaceholderFormat != null)
          {
            if (this._baseShape.PlaceholderFormat != null)
            {
              if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat))
              {
                defaultRotation = ((TextBody) shape.TextBody)._rotation;
                break;
              }
            }
            else
              break;
          }
        }
        if (defaultRotation == 0)
        {
          foreach (Shape shape in (IEnumerable<ISlideItem>) ((BaseSlide) layoutSlide.MasterSlide).Shapes)
          {
            if (shape.PlaceholderFormat != null)
            {
              if (this._baseShape.PlaceholderFormat != null)
              {
                if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat, true))
                {
                  defaultRotation = ((TextBody) shape.TextBody)._rotation;
                  break;
                }
              }
              else
                break;
            }
          }
        }
      }
    }
    return defaultRotation;
  }

  internal void SetAnchor(int rotation, long offsetX, long offsetY, long offsetCx, long offsetCy)
  {
    this._marginLeft = this._marginLeft + (double) offsetX - (double) this._baseShape.ShapeFrame.OffsetX;
    this._marginTop = this._marginTop + (double) offsetY - (double) this._baseShape.ShapeFrame.OffsetY;
    this._marginRight = (double) (this._baseShape.ShapeFrame.OffsetX + this._baseShape.ShapeFrame.OffsetCX) + this._marginRight - (double) (offsetCx + offsetX);
    this._marginBottom = (double) (this._baseShape.ShapeFrame.OffsetY + this._baseShape.ShapeFrame.OffsetCY) + this._marginBottom - (double) (offsetCy + offsetY);
    if (rotation == -1)
      return;
    if (rotation < 0)
      this.Rotation = 21600000 + rotation;
    else
      this.Rotation = rotation;
  }

  internal bool GetDefaultAnchorCenter()
  {
    if (this._anchorCenter)
      return this._anchorCenter;
    bool defaultAnchorCenter = false;
    string str = (string) null;
    if (this._baseShape.ShapeType != ShapeType.GraphicFrame)
    {
      if (this._baseShape.ShapeName != null)
        str = this.SetTempName(Helper.GetNameFromPlaceholder(this._baseShape.ShapeName));
      if (this.BaseSlide is Slide)
      {
        Slide baseSlide = (Slide) this.BaseSlide;
        string layoutIndex = baseSlide.ObtainLayoutIndex();
        if (layoutIndex != null)
        {
          LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
          if (str == null)
            Helper.GetName(this._baseShape.GetPlaceholder().GetPlaceholderType());
          foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
          {
            if (this._baseShape.PlaceholderFormat != null)
            {
              if (shape.PlaceholderFormat != null && Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat))
              {
                defaultAnchorCenter = ((TextBody) shape.TextBody)._anchorCenter;
                break;
              }
            }
            else
              break;
          }
          if (!defaultAnchorCenter)
          {
            foreach (Shape shape in (IEnumerable<ISlideItem>) ((BaseSlide) layoutSlide.MasterSlide).Shapes)
            {
              if (this._baseShape.PlaceholderFormat != null)
              {
                if (shape.PlaceholderFormat != null && Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat))
                {
                  defaultAnchorCenter = ((TextBody) shape.TextBody)._anchorCenter;
                  break;
                }
              }
              else
                break;
            }
          }
        }
      }
      else if (this.BaseSlide is NotesSlide)
      {
        foreach (Shape shape in (IEnumerable<ISlideItem>) this.BaseSlide.Presentation.NotesMaster.Shapes)
        {
          if (this._baseShape.PlaceholderFormat != null)
          {
            if (shape.PlaceholderFormat != null && Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat, true))
            {
              defaultAnchorCenter = ((TextBody) shape.TextBody)._anchorCenter;
              break;
            }
          }
          else
            break;
        }
      }
    }
    return defaultAnchorCenter;
  }

  public VerticalAlignmentType VerticalAlignment
  {
    get
    {
      return this._isVAlignTypeChanged ? this.ObtainVerticalAlignment() : this.GetDefaultVerticalAlign();
    }
    set
    {
      this._txtVerticalAlignment = (Syncfusion.Presentation.VerticalAlignment) Enum.Parse(typeof (Syncfusion.Presentation.VerticalAlignment), value.ToString(), true);
      this._isVAlignTypeChanged = true;
    }
  }

  private VerticalAlignmentType ObtainVerticalAlignment()
  {
    switch (this._txtVerticalAlignment)
    {
      case Syncfusion.Presentation.VerticalAlignment.None:
        return VerticalAlignmentType.None;
      case Syncfusion.Presentation.VerticalAlignment.Top:
        return VerticalAlignmentType.Top;
      case Syncfusion.Presentation.VerticalAlignment.Middle:
        return VerticalAlignmentType.Middle;
      case Syncfusion.Presentation.VerticalAlignment.Bottom:
        return VerticalAlignmentType.Bottom;
      default:
        return VerticalAlignmentType.None;
    }
  }

  internal VerticalAlignmentType GetDefaultVerticalAlign()
  {
    if (this._isVAlignTypeChanged)
      return this.ObtainVerticalAlignment();
    VerticalAlignmentType defaultVerticalAlign = VerticalAlignmentType.None;
    string str = (string) null;
    if (this._baseShape.ShapeType != ShapeType.GraphicFrame)
    {
      if (this._baseShape.ShapeName != null)
        str = this.SetTempName(Helper.GetNameFromPlaceholder(this._baseShape.ShapeName));
      if (this.BaseSlide is Slide)
      {
        Slide baseSlide = (Slide) this.BaseSlide;
        string layoutIndex = baseSlide.ObtainLayoutIndex();
        if (layoutIndex != null)
        {
          LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
          if (str == null)
            Helper.GetName(this._baseShape.GetPlaceholder().GetPlaceholderType());
          foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
          {
            if (this._baseShape.PlaceholderFormat != null)
            {
              if (shape.PlaceholderFormat != null && Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat))
              {
                defaultVerticalAlign = shape.TextBody.VerticalAlignment;
                break;
              }
            }
            else
              break;
          }
          if (defaultVerticalAlign == VerticalAlignmentType.None)
          {
            foreach (Shape shape in (IEnumerable<ISlideItem>) ((BaseSlide) layoutSlide.MasterSlide).Shapes)
            {
              if (this._baseShape.PlaceholderFormat != null)
              {
                if (shape.PlaceholderFormat != null && Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat, true))
                {
                  defaultVerticalAlign = shape.TextBody.VerticalAlignment;
                  break;
                }
              }
              else
                break;
            }
          }
          if (defaultVerticalAlign == VerticalAlignmentType.None)
            defaultVerticalAlign = VerticalAlignmentType.Top;
        }
      }
      if (this.BaseSlide is NotesSlide)
      {
        foreach (Shape shape in (IEnumerable<ISlideItem>) this._presentation.NotesMaster.Shapes)
        {
          if (this._baseShape.PlaceholderFormat != null)
          {
            if (shape.PlaceholderFormat != null && Helper.CheckPlaceholder(shape.PlaceholderFormat, this._baseShape.PlaceholderFormat, true))
            {
              defaultVerticalAlign = shape.TextBody.VerticalAlignment;
              break;
            }
          }
          else
            break;
        }
        if (defaultVerticalAlign == VerticalAlignmentType.None)
          defaultVerticalAlign = VerticalAlignmentType.Top;
      }
    }
    return defaultVerticalAlign;
  }

  private string SetTempName(string tempName)
  {
    if (this._baseShape.DrawingType == DrawingType.PlaceHolder && tempName == "shape")
    {
      switch (this._baseShape.GetPlaceholder().GetPlaceholderType())
      {
        case PlaceholderType.Title:
        case PlaceholderType.CenterTitle:
          tempName = "Title";
          break;
        case PlaceholderType.Body:
          tempName = "Content";
          break;
        case PlaceholderType.Subtitle:
          tempName = "Subtitle";
          break;
      }
    }
    return tempName;
  }

  internal TextOverflowType TextVerticalOverflow
  {
    get => this._txtVerticalFlowType;
    set => this._txtVerticalFlowType = value;
  }

  public TextDirectionType TextDirection
  {
    get
    {
      switch (this._txtDirectionType)
      {
        case Syncfusion.Presentation.TextDirection.Horizontal:
          return TextDirectionType.Horizontal;
        case Syncfusion.Presentation.TextDirection.Vertical:
          return TextDirectionType.Vertical;
        case Syncfusion.Presentation.TextDirection.Vertical270:
          return TextDirectionType.Vertical270;
        case Syncfusion.Presentation.TextDirection.WordArtVertical:
          return TextDirectionType.WordArtVertical;
        default:
          return TextDirectionType.Horizontal;
      }
    }
    set
    {
      this._txtDirectionType = (Syncfusion.Presentation.TextDirection) Enum.Parse(typeof (Syncfusion.Presentation.TextDirection), value.ToString(), true);
    }
  }

  internal BaseSlide BaseSlide => this._baseSlide;

  internal Dictionary<string, Paragraph> StyleList
  {
    get => this._styleList ?? (this._styleList = new Dictionary<string, Paragraph>());
  }

  internal void SetFitTextOptionChanged(bool value) => this._isFitTextOptionChanged = value;

  internal bool GetFitTextOptionChanged() => this._isFitTextOptionChanged;

  internal Syncfusion.Presentation.VerticalAlignment GetVerticalAlignmentType()
  {
    return this._txtVerticalAlignment;
  }

  internal Syncfusion.Presentation.TextDirection ObatinTextDirection() => this._txtDirectionType;

  internal void SetTextDirection(Syncfusion.Presentation.TextDirection textDirection)
  {
    this._txtDirectionType = textDirection;
  }

  internal void SetVerticalAlign(Syncfusion.Presentation.VerticalAlignment textVerticalAlignmentType)
  {
    this._txtVerticalAlignment = textVerticalAlignmentType;
    this._isVAlignTypeChanged = true;
  }

  private void InitializeDefaultMargins()
  {
    this._marginBottom = (double) int.MinValue;
    this._marginLeft = (double) int.MinValue;
    this._marginRight = (double) int.MinValue;
    this._marginTop = (double) int.MinValue;
  }

  internal Shape GetBaseShape() => this._baseShape;

  internal int GetBottomMargin() => (int) this._marginBottom;

  internal int GetUpdatedFontScale()
  {
    bool flag = false;
    float modifiedSize = 0.0f;
    Graphics graphics = this.BaseSlide.Presentation.InternalGraphics = Graphics.FromImage((Image) new Bitmap((int) this.BaseSlide.Presentation.SlideSize.Width, (int) this.BaseSlide.Presentation.SlideSize.Height));
    this.BaseSlide.Presentation.IsInternalGraphics = true;
    Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.SetGraphicsProperties(graphics);
    Shape baseShape = this._baseShape;
    ShapeInfo shapeInfo = new ShapeInfo((IShape) baseShape);
    shapeInfo.Bounds = new RectangleF((float) baseShape.ShapeFrame.GetDefaultLeft(), (float) baseShape.ShapeFrame.GetDefaultTop(), (float) baseShape.ShapeFrame.GetDefaultWidth(), (float) baseShape.ShapeFrame.GetDefaultHeight());
    RectangleF layoutRect = new RectangleF(0.0f, 0.0f, shapeInfo.Bounds.Width, shapeInfo.Bounds.Height);
    baseShape.UpdateShapeBoundsToLayoutTextBody(ref layoutRect, shapeInfo.Bounds);
    int count = this._paragraphCollection.Count;
    int num1 = baseShape.TextBody.WrapText ? 1 : 0;
    this._fontScale = 100000;
    float[] numArray1 = new float[this.Text.Length];
    int index1 = 0;
    float marginLeft = 0.0f;
    float usedHeight = 0.0f;
    for (int index2 = 0; index2 < count; ++index2)
    {
      Paragraph paragraph = baseShape.TextBody.Paragraphs[index2] as Paragraph;
      float fontSize = baseShape.TextBody.Paragraphs[index2].Font.FontSize;
label_71:
      for (int index3 = 0; index3 < paragraph.TextParts.Count; ++index3)
      {
        while (this._fontScale != 25000)
        {
          if (flag)
          {
            marginLeft = 0.0f;
            usedHeight = 0.0f;
            numArray1 = new float[this.Text.Length];
            index1 = 0;
          }
          float usedWidth = 0.0f;
          flag = false;
          Font font = paragraph.TextParts[index3].Font as Font;
          System.Drawing.Font updatedFont = paragraph.GetUpdatedFont((IFont) font, (paragraph.TextParts[index3] as TextPart).ScriptType);
          string text1 = paragraph.TextParts[index3].Text;
          if (!string.IsNullOrEmpty(this.Text))
          {
            int num2 = (int) this.Text[0];
          }
          float indentWidth = 0.0f;
          SizeF bulletSize = new SizeF();
          if (!string.IsNullOrEmpty(text1))
          {
            List<TextInfo> textInfoCollection = new List<TextInfo>();
            paragraph.CalculateBulletSize(paragraph.ListFormat, ref usedWidth, ref usedHeight, layoutRect, ref bulletSize, ref marginLeft, ref textInfoCollection, ref indentWidth);
            if ((double) bulletSize.Height != 0.0)
            {
              usedWidth += bulletSize.Width;
              numArray1[index1] = bulletSize.Height;
              ++index1;
            }
            TextCapsType capsType = font.GetDefaultCapsType();
            switch (capsType)
            {
              case TextCapsType.None:
              case TextCapsType.All:
                bool hasUnicode = false;
                string[] strArray = paragraph.SplitTextByUnicode(text1, ref hasUnicode);
                (paragraph.TextParts[index3] as TextPart).HasUnicode = hasUnicode;
                for (int index4 = 0; index4 < strArray.Length; ++index4)
                {
                  string text2 = strArray[index4];
                  paragraph.UpdatedFont(text2, ref updatedFont, font, (paragraph.TextParts[index3] as TextPart).ScriptType);
                  SizeF sizeF1 = paragraph.MeasureString(text2, updatedFont, capsType);
                  float num3 = 0.0f;
                  for (int index5 = 0; index5 < numArray1.Length; ++index5)
                    num3 += numArray1[index5];
                  usedHeight = num3;
                  if ((double) sizeF1.Width > (double) layoutRect.Width - (double) usedWidth && (double) sizeF1.Height > (double) layoutRect.Height + this.DefaultLeftMargin() - (double) usedHeight + this.DefaultTopMargin())
                  {
                    this.ChangeFontScale(ref this._fontScale, index2, index3, ref modifiedSize, paragraph, (double) fontSize, this._paragraphCollection, font);
                    flag = true;
                    break;
                  }
                  if ((double) sizeF1.Width <= (double) layoutRect.Width - (double) usedWidth && (double) sizeF1.Height <= (double) layoutRect.Height + this.DefaultLeftMargin() - (double) usedHeight + this.DefaultTopMargin())
                  {
                    numArray1[index1] = sizeF1.Height;
                    ++index1;
                  }
                  else if ((double) sizeF1.Width > (double) layoutRect.Width - (double) usedWidth && (double) sizeF1.Height < (double) layoutRect.Height + this.DefaultLeftMargin() - (double) usedHeight + this.DefaultTopMargin())
                  {
                    int startIndex = 0;
                    while (startIndex < text2.Length)
                    {
                      int num4 = paragraph.GetSpaceIndexBeforeText(text2, startIndex);
                      int num5 = text2.IndexOf('-', startIndex);
                      if (num5 >= 0 && (num5 < num4 || num4 == -1))
                        num4 = num5;
                      string str;
                      if (num4 >= 0)
                        str = text2.Substring(startIndex, num4 + 1 - startIndex);
                      else
                        str = text2.Substring(startIndex, text2.Length - startIndex).TrimEnd(' ');
                      string text3 = str;
                      sizeF1 = paragraph.MeasureString(text3, updatedFont, capsType);
                      SizeF sizeF2 = new SizeF();
                      if (num4 > 0)
                      {
                        string text4 = text2.Substring(startIndex, num4 + 1 - startIndex).TrimEnd(' ');
                        sizeF2 = paragraph.MeasureString(text4, updatedFont, capsType);
                      }
                      if ((double) sizeF1.Width <= (double) layoutRect.Width - (double) usedWidth || (double) sizeF2.Width > 0.0 && (double) sizeF2.Width <= (double) layoutRect.Width - (double) usedWidth && (double) sizeF1.Width > (double) layoutRect.Width - (double) usedWidth)
                      {
                        usedWidth += sizeF1.Width;
                        if (startIndex == 0 && (double) bulletSize.Height <= 0.0)
                        {
                          numArray1[index1] = sizeF1.Height;
                          ++index1;
                        }
                        startIndex = num4 <= -1 ? text2.Length : num4 + 1;
                      }
                      else if ((double) sizeF1.Width > (double) layoutRect.Width - (double) usedWidth)
                      {
                        float num6 = 0.0f;
                        for (int index6 = 0; index6 < numArray1.Length; ++index6)
                          num6 += numArray1[index6];
                        usedHeight = num6;
                        if ((double) sizeF1.Height < (double) layoutRect.Height + this.DefaultLeftMargin() - (double) usedHeight + this.DefaultTopMargin())
                        {
                          if ((double) sizeF2.Width > 0.0 && (double) sizeF2.Width <= (double) layoutRect.Width - (double) usedWidth)
                          {
                            numArray1[index1] = sizeF2.Height;
                            ++index1;
                            usedWidth += sizeF1.Width;
                          }
                          else if ((double) sizeF1.Width < (double) layoutRect.Width || (double) sizeF2.Width > 0.0 && (double) sizeF2.Width < (double) layoutRect.Width)
                          {
                            usedWidth = sizeF1.Width + indentWidth + bulletSize.Width + marginLeft;
                            numArray1[index1] = sizeF1.Height;
                            ++index1;
                          }
                          else if ((double) sizeF1.Width > (double) layoutRect.Width)
                          {
                            int index7 = 0;
                            int num7 = 0;
                            float[] numArray2 = new float[text3.Length];
                            char[] charArray = text3.ToCharArray();
                            for (int index8 = 0; index8 < text3.Length; ++index8)
                            {
                              SizeF sizeF3 = paragraph.MeasureString(charArray[index8].ToString(), updatedFont, capsType);
                              numArray2[index7] = sizeF3.Height;
                              ++index7;
                              if ((double) sizeF3.Width < (double) layoutRect.Width - (double) usedWidth)
                                usedWidth += sizeF3.Width;
                              else if (index8 == 0 && (double) sizeF3.Width > (double) layoutRect.Width - (double) usedWidth)
                              {
                                ++num7;
                                usedWidth = sizeF3.Width + bulletSize.Width + indentWidth + marginLeft;
                                numArray1[index1] = sizeF3.Height;
                                ++index1;
                              }
                              else
                              {
                                if (num7 == 0)
                                {
                                  numArray1[index1] = sizeF3.Height;
                                  ++index1;
                                }
                                usedHeight = 0.0f;
                                float num8 = 0.0f;
                                for (int index9 = 0; index9 < numArray1.Length; ++index9)
                                  num8 += numArray1[index9];
                                usedHeight = num8;
                                if ((double) sizeF1.Height < (double) layoutRect.Height + this.DefaultLeftMargin() - (double) usedHeight + this.DefaultTopMargin())
                                {
                                  numArray1[index1] = sizeF3.Height;
                                  ++index1;
                                  usedWidth = sizeF3.Width + indentWidth + bulletSize.Width + marginLeft;
                                }
                                else
                                {
                                  this.ChangeFontScale(ref this._fontScale, index2, index3, ref modifiedSize, paragraph, (double) fontSize, this._paragraphCollection, font);
                                  flag = true;
                                  break;
                                }
                              }
                            }
                            if (flag)
                              break;
                          }
                          startIndex += text3.Length;
                        }
                        else
                        {
                          this.ChangeFontScale(ref this._fontScale, index2, index3, ref modifiedSize, paragraph, (double) fontSize, this._paragraphCollection, font);
                          flag = true;
                          break;
                        }
                      }
                    }
                    if (flag)
                      break;
                  }
                  else
                  {
                    this.ChangeFontScale(ref this._fontScale, index2, index3, ref modifiedSize, paragraph, (double) fontSize, this._paragraphCollection, font);
                    flag = true;
                    break;
                  }
                }
                if (flag && index2 > 0)
                {
                  index2 = -1;
                  index3 = paragraph.TextParts.Count;
                  goto label_71;
                }
                break;
              default:
                if (paragraph.IsRTLText(text1))
                {
                  capsType = TextCapsType.None;
                  goto case TextCapsType.None;
                }
                goto case TextCapsType.None;
            }
          }
          if (!flag)
            break;
        }
      }
    }
    if (this.BaseSlide.Presentation.InternalGraphics != null)
    {
      this.BaseSlide.Presentation.InternalGraphics.Dispose();
      this.BaseSlide.Presentation.InternalGraphics = (Graphics) null;
    }
    this.BaseSlide.Presentation.IsInternalGraphics = false;
    this._isFontScaleAccessed = true;
    this._isFitTextOptionChanged = false;
    this.UpdateFontSize();
    return this._fontScale;
  }

  internal void ChangeFontScale(
    ref int _fontScale,
    int paragraphIndex,
    int textPartIndex,
    ref float modifiedSize,
    Paragraph paragraph,
    double newSize,
    Syncfusion.Presentation.RichText.Paragraphs _paragraphCollection,
    Font syncFont)
  {
    _fontScale -= 7500;
    int num = paragraphIndex;
    if (_fontScale == 25000)
      num = _paragraphCollection.Count - 1;
    for (int index1 = 0; index1 <= num; ++index1)
    {
      modifiedSize = (float) syncFont.ApplyFontScale((double) _paragraphCollection[index1].Font.FontSize, _fontScale);
      for (int index2 = 0; index2 <= textPartIndex; ++index2)
        _paragraphCollection[index1].TextParts[index2].Font.FontSize = modifiedSize;
    }
  }

  private void UpdateFontSize()
  {
    foreach (IParagraph paragraph in this._paragraphCollection)
    {
      int count = paragraph.TextParts.Count;
      for (int index = 0; index < count; ++index)
        (paragraph.TextParts[index].Font as Font).IsSizeChanged = false;
    }
  }

  internal int GetFontScale()
  {
    this._isFontScaleAccessed = true;
    return this._fontScale;
  }

  internal bool IsFontScaleAccessed => this._isFontScaleAccessed;

  internal int GetLeftMargin() => (int) this._marginLeft;

  internal int GetLnSpcReductionValue() => this._lineSpcRedValue;

  internal int GetRightMargin() => (int) this._marginRight;

  internal int GetTopMargin() => (int) this._marginTop;

  internal void SetFontScale(int value)
  {
    this._fontScale = value;
    this._isAutoSize = true;
  }

  internal void SetMargin(int leftMargin, int topMargin, int rightMargin, int bottomMargin)
  {
    this._marginLeft = (double) leftMargin;
    this._marginTop = (double) topMargin;
    this._marginRight = (double) rightMargin;
    this._marginBottom = (double) bottomMargin;
  }

  internal void SetLnSpcReductionValue(int value)
  {
    this._lineSpcRedValue = value;
    this._isAutoSize = true;
  }

  internal FitTextOption GetFitTextOption(AutoMarginType autoMarginType)
  {
    switch (autoMarginType)
    {
      case AutoMarginType.TextShapeAutoFit:
        return FitTextOption.ResizeShapeToFitText;
      case AutoMarginType.NormalAutoFit:
        return FitTextOption.ShrinkTextOnOverFlow;
      default:
        return FitTextOption.DoNotAutoFit;
    }
  }

  internal void Close() => this.ClearAll();

  private void ClearAll()
  {
    if (this._paragraphCollection != null)
    {
      this._paragraphCollection.Close();
      this._paragraphCollection = (Syncfusion.Presentation.RichText.Paragraphs) null;
    }
    if (this._styleList != null)
    {
      foreach (KeyValuePair<string, Paragraph> style in this._styleList)
        style.Value.Close();
      this._styleList.Clear();
      this._styleList = (Dictionary<string, Paragraph>) null;
    }
    this._baseShape = (Shape) null;
    this._baseSlide = (BaseSlide) null;
    this._presentation = (Syncfusion.Presentation.Presentation) null;
    this._cell = (Cell) null;
  }

  private Dictionary<string, Paragraph> CloneStyleList(Syncfusion.Presentation.RichText.Paragraphs newParent)
  {
    Dictionary<string, Paragraph> dictionary = new Dictionary<string, Paragraph>();
    foreach (KeyValuePair<string, Paragraph> style in this._styleList)
    {
      Paragraph paragraph = style.Value.InternalClone();
      paragraph.SetParent(newParent);
      dictionary.Add(style.Key, paragraph);
    }
    return dictionary;
  }

  private void CloneTextFrameElements(TextBody textBodyClone)
  {
    textBodyClone._paragraphCollection = this._paragraphCollection.Clone();
    textBodyClone._paragraphCollection.SetParent(textBodyClone);
    if (this._preservedElements != null)
      textBodyClone._preservedElements = Helper.CloneDictionary(this._preservedElements);
    if (this._styleList == null)
      return;
    textBodyClone._styleList = this.CloneStyleList(textBodyClone._paragraphCollection);
  }

  public TextBody Clone()
  {
    TextBody textBodyClone = (TextBody) this.MemberwiseClone();
    this.CloneTextFrameElements(textBodyClone);
    if (textBodyClone.BaseSlide != null)
      textBodyClone._baseSlide = (BaseSlide) null;
    return textBodyClone;
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
    this._paragraphCollection.SetParent(presentation);
    if (this._styleList == null)
      return;
    foreach (KeyValuePair<string, Paragraph> style in this._styleList)
      style.Value.SetParent(presentation);
  }

  internal void SetParent(Shape shape)
  {
    this._baseShape = shape;
    this._paragraphCollection.SetParent(shape);
  }

  internal void SetParent(Cell cellClone) => this._cell = cellClone;

  internal void SetParent(BaseSlide baseSlide) => this._baseSlide = baseSlide;
}
