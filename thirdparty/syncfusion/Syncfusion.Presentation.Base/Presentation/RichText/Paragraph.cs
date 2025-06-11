// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.RichText.Paragraph
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Office;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.Layouting;
using Syncfusion.Presentation.SlideImplementation;
using Syncfusion.Presentation.TableImplementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

#nullable disable
namespace Syncfusion.Presentation.RichText;

internal class Paragraph : IParagraph
{
  private const float DEF_SCRIPT_FACTOR = 1.5f;
  private const float DEF_FONT_SIZE = 18f;
  private const float DEF_PICBULLET_SCALE_FACTOR = 15f;
  private ParagraphInfo _paragrahInfo;
  private StringFormat _stringformat;
  private Syncfusion.Presentation.RichText.ListFormat _bulletFormat;
  private Syncfusion.Presentation.RichText.Font _charProps;
  private Syncfusion.Presentation.RichText.Font _defaultFont;
  private int _indent;
  private int _indentLevel;
  private bool _isLast;
  private int _lineSpacing;
  private int _marginLeft;
  private int _marginRight;
  private Paragraphs _paragraphCollection;
  private int _spaceAfter;
  private int _spaceBefore;
  private Syncfusion.Presentation.HorizontalAlignment _textAlignmentType;
  private FontAlignmentType _fontAlignmentType;
  private bool? _rightToLeft;
  private Syncfusion.Presentation.RichText.TextParts _textPartCollection;
  private int _index;
  private long _defTabSz = -1;
  private bool _isWithinList;
  private bool _isAlignChanged;
  private bool _isMarginChanged;
  private bool _isIndentChanged;
  private bool _isTabSizeChanged;
  private SizeType _lineSpacingType;
  private SizeType _spaceAfterType;
  private SizeType _spaceBeforeType;
  private SizeType CurrentLnSpcType;
  internal SizeType CurrentSpcAftType;
  private SizeType CurrentSpcBefType;
  private bool _isDisposed;
  private List<long> _tabPositionList;
  private List<TabAlignmentType> _tabAlignmentTypeList;
  private Guid _nodeId;

  internal Paragraph(Paragraphs paragraphCollection)
  {
    this._paragraphCollection = paragraphCollection;
    this._textPartCollection = new Syncfusion.Presentation.RichText.TextParts(this);
    this._bulletFormat = new Syncfusion.Presentation.RichText.ListFormat(this);
  }

  public string Text
  {
    get
    {
      string text = "";
      if (this._textPartCollection == null)
        return text;
      foreach (ITextPart textPart in this._textPartCollection)
        text += ((TextPart) textPart).Text;
      return text;
    }
    set
    {
      if (this._paragraphCollection.TextBody.FitTextOption == FitTextOption.ShrinkTextOnOverFlow)
        this._paragraphCollection.TextBody.SetFitTextOptionChanged(true);
      if (value == null)
        return;
      TextPart textPart;
      if (this._textPartCollection.Count > 0)
      {
        textPart = (TextPart) this._textPartCollection[this._textPartCollection.Count - 1];
      }
      else
      {
        textPart = new TextPart(this);
        if (this._paragraphCollection.Count == 1 && this.GetEndParaProps() != null)
        {
          Syncfusion.Presentation.RichText.Font font = this.GetEndParaProps().Clone();
          font.SetParent(textPart);
          textPart.SetFont(font);
        }
      }
      textPart.UniqueId = (string) null;
      textPart.Type = (string) null;
      textPart.Text = value;
      this._textPartCollection.Clear();
      this._textPartCollection.Add((ITextPart) textPart);
    }
  }

  public IParagraph Clone() => (IParagraph) this.InternalClone();

  public ITextPart AddTextPart()
  {
    if (this._paragraphCollection.TextBody.FitTextOption == FitTextOption.ShrinkTextOnOverFlow)
      this._paragraphCollection.TextBody.SetFitTextOptionChanged(true);
    TextPart textPart = new TextPart(this);
    this._textPartCollection.Add((ITextPart) textPart);
    return (ITextPart) textPart;
  }

  public IHyperLink AddHyperlink(string textToDisplay, string link)
  {
    if (this._paragraphCollection.TextBody.FitTextOption == FitTextOption.ShrinkTextOnOverFlow)
      this._paragraphCollection.TextBody.SetFitTextOptionChanged(true);
    return (IHyperLink) ((TextPart) this.AddTextPart(textToDisplay)).AddHyperLink(link);
  }

  public ITextPart AddTextPart(string text)
  {
    if (this._paragraphCollection.TextBody.FitTextOption == FitTextOption.ShrinkTextOnOverFlow)
      this._paragraphCollection.TextBody.SetFitTextOptionChanged(true);
    TextPart textPart = new TextPart(this, text);
    this._textPartCollection.Add((ITextPart) textPart);
    return (ITextPart) textPart;
  }

  internal Syncfusion.Presentation.Presentation Presentation
  {
    get => this._paragraphCollection.TextBody.Presentation;
  }

  public HorizontalAlignmentType HorizontalAlignment
  {
    get
    {
      return this._isAlignChanged ? this.GetHorizontalAlignmentType() : this.GetDefaultAlignmentType();
    }
    set
    {
      this._textAlignmentType = (Syncfusion.Presentation.HorizontalAlignment) Enum.Parse(typeof (Syncfusion.Presentation.HorizontalAlignment), value.ToString(), true);
      this._isAlignChanged = true;
    }
  }

  private HorizontalAlignmentType GetHorizontalAlignmentType()
  {
    switch (this._textAlignmentType)
    {
      case Syncfusion.Presentation.HorizontalAlignment.None:
        return HorizontalAlignmentType.None;
      case Syncfusion.Presentation.HorizontalAlignment.Left:
        return HorizontalAlignmentType.Left;
      case Syncfusion.Presentation.HorizontalAlignment.Center:
        return HorizontalAlignmentType.Center;
      case Syncfusion.Presentation.HorizontalAlignment.Right:
        return HorizontalAlignmentType.Right;
      case Syncfusion.Presentation.HorizontalAlignment.Justify:
        return HorizontalAlignmentType.Justify;
      case Syncfusion.Presentation.HorizontalAlignment.Distributed:
        return HorizontalAlignmentType.Distributed;
      default:
        return HorizontalAlignmentType.Left;
    }
  }

  internal HorizontalAlignmentType GetDefaultAlignmentTypefromTable()
  {
    if (this.BaseShape.PlaceholderFormat == null && this._paragraphCollection.TextBody.Cell != null)
    {
      HorizontalAlignmentType alignmentTypefromTable = HorizontalAlignmentType.None;
      Dictionary<string, Paragraph> styleList = this._paragraphCollection.TextBody.StyleList;
      if (styleList != null)
        alignmentTypefromTable = this.GetAlignFromStyleList(styleList);
      if (alignmentTypefromTable == HorizontalAlignmentType.None)
      {
        IMasterSlide masterSlide = (IMasterSlide) null;
        if (this.BaseSlide is Slide)
          masterSlide = (this.BaseSlide as Slide).LayoutSlide.MasterSlide;
        else if (this.BaseSlide is LayoutSlide)
          masterSlide = (this.BaseSlide as LayoutSlide).MasterSlide;
        else if (this.BaseSlide is MasterSlide)
          masterSlide = (IMasterSlide) (this.BaseSlide as MasterSlide);
        if (masterSlide != null)
        {
          TextBody textStyle = Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide as MasterSlide, "shape");
          if (textStyle != null && textStyle.StyleList != null)
            alignmentTypefromTable = this.GetAlignFromStyleList(textStyle.StyleList);
        }
      }
      if (alignmentTypefromTable != HorizontalAlignmentType.None)
        return alignmentTypefromTable;
    }
    return HorizontalAlignmentType.Left;
  }

  internal HorizontalAlignmentType GetDefaultAlignmentType()
  {
    if (this._isAlignChanged || this._isWithinList)
      return this.GetHorizontalAlignmentType();
    HorizontalAlignmentType defaultAlignmentType = HorizontalAlignmentType.None;
    if (this.BaseShape != null)
    {
      switch (this.BaseShape.DrawingType)
      {
        case DrawingType.None:
        case DrawingType.TextBox:
          if (((TextBody) this.BaseShape.TextBody).StyleList != null)
            defaultAlignmentType = this.GetAlignFromStyleList(((TextBody) this.BaseShape.TextBody).StyleList);
          return defaultAlignmentType == HorizontalAlignmentType.None ? this.DefaultAlignFromPresentation() : defaultAlignmentType;
        case DrawingType.Table:
          return this.GetDefaultAlignmentTypefromTable();
      }
    }
    Dictionary<string, Paragraph> styleList1 = this._paragraphCollection.TextBody.StyleList;
    if (styleList1 != null)
      defaultAlignmentType = this.GetAlignFromStyleList(styleList1);
    if (defaultAlignmentType == HorizontalAlignmentType.None && this.BaseShape != null)
    {
      string styleName = this.SetTempName(Syncfusion.Presentation.Drawing.Helper.GetNameFromPlaceholder(this.BaseShape.ShapeName));
      if (this.BaseSlide is Slide)
      {
        Slide baseSlide = (Slide) this.BaseSlide;
        string layoutIndex = baseSlide.ObtainLayoutIndex();
        if (layoutIndex != null)
        {
          LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
          foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
          {
            if (shape.PlaceholderFormat != null)
            {
              if (this.BaseShape.PlaceholderFormat != null)
              {
                if (Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this.BaseShape.PlaceholderFormat))
                {
                  Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                  if (styleList2 != null)
                  {
                    defaultAlignmentType = this.GetAlignFromStyleList(styleList2);
                    break;
                  }
                  break;
                }
              }
              else
                break;
            }
          }
          if (defaultAlignmentType != HorizontalAlignmentType.None)
            return defaultAlignmentType;
          MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
          if (this.BaseShape.PlaceholderFormat == null)
          {
            defaultAlignmentType = this.GetAlignFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList);
          }
          else
          {
            switch (this.BaseShape.GetPlaceholder().GetPlaceholderType())
            {
              case PlaceholderType.SlideNumber:
              case PlaceholderType.Header:
              case PlaceholderType.Footer:
              case PlaceholderType.Date:
                using (IEnumerator<ISlideItem> enumerator = masterSlide.Shapes.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    Shape current = (Shape) enumerator.Current;
                    if (current.PlaceholderFormat != null && Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(current.PlaceholderFormat, this.BaseShape.PlaceholderFormat, true))
                    {
                      Dictionary<string, Paragraph> styleList3 = ((TextBody) current.TextBody).StyleList;
                      if (styleList3 != null)
                      {
                        defaultAlignmentType = this.GetAlignFromStyleList(styleList3);
                        break;
                      }
                      break;
                    }
                  }
                  break;
                }
              default:
                defaultAlignmentType = this.GetAlignFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList);
                break;
            }
          }
        }
      }
      else if (this.BaseSlide is NotesSlide)
      {
        foreach (Shape shape in (IEnumerable<ISlideItem>) this.BaseSlide.Presentation.NotesMaster.Shapes)
        {
          if (shape.PlaceholderFormat != null)
          {
            if (this.BaseShape.PlaceholderFormat != null)
            {
              if (Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this.BaseShape.PlaceholderFormat, true))
              {
                Dictionary<string, Paragraph> styleList4 = ((TextBody) shape.TextBody).StyleList;
                if (styleList4 != null)
                {
                  defaultAlignmentType = this.GetAlignFromStyleList(styleList4);
                  break;
                }
                break;
              }
            }
            else
              break;
          }
        }
      }
    }
    return defaultAlignmentType;
  }

  private HorizontalAlignmentType DefaultAlignFromPresentation()
  {
    HorizontalAlignmentType horizontalAlignmentType = HorizontalAlignmentType.None;
    if (this.BaseSlide.Presentation.DefaultTextStyle != null)
    {
      Dictionary<string, Paragraph> styleList = this.BaseSlide.Presentation.DefaultTextStyle.StyleList;
      if (styleList != null && styleList.Count != 0)
      {
        string key = $"lvl{(object) (this.IndentLevelNumber + 1)}pPr";
        if (styleList.ContainsKey(key))
          return styleList[key].GetDefaultAlignmentType();
      }
    }
    return horizontalAlignmentType;
  }

  private string SetTempName(string tempName)
  {
    if (this.BaseShape.DrawingType == DrawingType.PlaceHolder && tempName == "shape")
    {
      switch (this.BaseShape.GetPlaceholder().GetPlaceholderType())
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

  private HorizontalAlignmentType GetAlignFromStyleList(Dictionary<string, Paragraph> styleList)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
        return styleList[key].GetDefaultAlignmentType();
    }
    return HorizontalAlignmentType.None;
  }

  internal bool? RightToLeft
  {
    get => this._rightToLeft;
    set => this._rightToLeft = value;
  }

  public ITextParts TextParts => (ITextParts) this._textPartCollection;

  internal Guid NodeId
  {
    get => this._nodeId;
    set => this._nodeId = value;
  }

  internal bool IsIndentChanged
  {
    get => this._isIndentChanged;
    set => this._isIndentChanged = value;
  }

  internal int Index
  {
    get => this._index;
    set => this._index = value;
  }

  public IListFormat ListFormat => (IListFormat) this._bulletFormat;

  internal FontAlignmentType FontAlignmentType
  {
    get => this._fontAlignmentType;
    set => this._fontAlignmentType = value;
  }

  internal Shape BaseShape => this._paragraphCollection.TextBody.GetBaseShape();

  public int IndentLevelNumber
  {
    get => this._indentLevel;
    set
    {
      this._indentLevel = value <= 8 && value >= 0 ? value : throw new ArgumentException("Invalid Indent Level Number " + value.ToString());
    }
  }

  internal void SetLevelValue()
  {
    Dictionary<int, LevelContainer> levelHolder = this._paragraphCollection.GetLevelHolder(this._indentLevel);
    Paragraph previousParagraph = this._paragraphCollection.GetPreviousParagraph();
    if (levelHolder.ContainsKey(this._indentLevel))
    {
      if (previousParagraph == null)
        return;
      if (this._indentLevel < previousParagraph._indentLevel)
      {
        if (this._indentLevel != 0)
        {
          for (int indentLevel = previousParagraph._indentLevel; indentLevel != this._indentLevel; --indentLevel)
          {
            if (levelHolder.ContainsKey(indentLevel))
              levelHolder[indentLevel] = levelHolder[indentLevel].Clear();
          }
        }
        else
          this._paragraphCollection.ClearAllBullets();
        if (!(previousParagraph.Text != ""))
          return;
        levelHolder[this._indentLevel] = levelHolder[this._indentLevel].Update();
      }
      else if (this._indentLevel > previousParagraph._indentLevel)
      {
        if (levelHolder.ContainsKey(this._indentLevel))
          levelHolder.Remove(this._indentLevel);
        levelHolder.Add(this._indentLevel, new LevelContainer(1));
      }
      else
      {
        if (!(previousParagraph.Text != ""))
          return;
        levelHolder[this._indentLevel] = levelHolder[this._indentLevel].Update();
      }
    }
    else
      levelHolder.Add(this._indentLevel, new LevelContainer(1));
  }

  public double LineSpacing
  {
    get
    {
      switch (this._lineSpacingType)
      {
        case SizeType.Points:
          this.CurrentLnSpcType = SizeType.Points;
          return Math.Round((double) this.SetLineSpaceReduction() / 100.0);
        case SizeType.Percentage:
          this.CurrentLnSpcType = SizeType.Percentage;
          return Math.Round((double) this.SetLineSpaceReduction() / 100000.0, 1);
        default:
          return 1.0;
      }
    }
    set
    {
      if (value < 0.0 || value > 1584.0)
        throw new ArgumentException("Invalid Line Spacing " + value.ToString());
      this._lineSpacingType = SizeType.Points;
      this.CurrentLnSpcType = SizeType.Points;
      this._lineSpacing = Convert.ToInt32(value * 100.0);
    }
  }

  internal double GetDefaultLineSpacing()
  {
    switch (this._lineSpacingType)
    {
      case SizeType.Points:
        this.CurrentLnSpcType = SizeType.Points;
        return Math.Round((double) this.SetLineSpaceReduction() / 100.0);
      case SizeType.Percentage:
        this.CurrentLnSpcType = SizeType.Percentage;
        return Math.Round((double) this.SetLineSpaceReduction() / 100000.0, 1);
      default:
        return this.BaseShape.DrawingType != DrawingType.Table ? this.DefaultLineSpacing() : 1.0;
    }
  }

  private int SetLineSpaceReduction()
  {
    long num = 0;
    if (this._paragraphCollection.TextBody.IsAutoSize)
    {
      num = (long) this._paragraphCollection.TextBody.GetLnSpcReductionValue();
      if (num != 0L)
        num = (long) this._lineSpacing - (long) this._lineSpacing * num / 100000L;
    }
    return num != 0L ? Convert.ToInt32(num) : this._lineSpacing;
  }

  private double DefaultLineSpacing()
  {
    double num = 0.0;
    Dictionary<string, Paragraph> styleList1 = this._paragraphCollection.TextBody.StyleList;
    if (styleList1 != null)
      num = this.GetLnSpaceValFromStyleList(styleList1);
    if (num == 0.0 && this.BaseShape != null && this.BaseShape.DrawingType != DrawingType.PlaceHolder)
      return this.DefaultLnSpcFromPresentation();
    if (num == 0.0 && this.BaseShape != null)
    {
      string styleName = this.SetTempName(Syncfusion.Presentation.Drawing.Helper.GetNameFromPlaceholder(this.BaseShape.ShapeName));
      if (this.BaseSlide is Slide)
      {
        Slide baseSlide = (Slide) this.BaseSlide;
        string layoutIndex = baseSlide.ObtainLayoutIndex();
        if (layoutIndex != null)
        {
          LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
          foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
          {
            if (shape.ShapeName != null && shape.PlaceholderFormat != null)
            {
              if (this.BaseShape.PlaceholderFormat != null)
              {
                if (Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this.BaseShape.PlaceholderFormat))
                {
                  Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                  if (styleList2 != null)
                  {
                    num = this.GetLnSpaceValFromStyleList(styleList2);
                    break;
                  }
                  break;
                }
              }
              else
                break;
            }
          }
          if (num == 0.0)
          {
            MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
            if (styleName == "shape" && this.BaseShape.PlaceholderFormat != null)
              styleName = "Content";
            num = this.GetLnSpaceValFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList);
          }
        }
      }
    }
    if (this._paragraphCollection.TextBody.IsAutoSize)
    {
      double spcReductionValue = (double) this._paragraphCollection.TextBody.GetLnSpcReductionValue();
      if (spcReductionValue != 0.0 && num == 0.0)
        num = 1.0;
      this.CurrentLnSpcType = SizeType.Percentage;
      num -= num * spcReductionValue / 100000.0;
    }
    return num != 0.0 ? num : 1.0;
  }

  private double DefaultLnSpcFromPresentation()
  {
    double num = 0.0;
    if (this.BaseSlide.Presentation.DefaultTextStyle != null)
    {
      Dictionary<string, Paragraph> styleList = this.BaseSlide.Presentation.DefaultTextStyle.StyleList;
      if (styleList != null && styleList.Count != 0)
      {
        string key = $"lvl{(object) (this.IndentLevelNumber + 1)}pPr";
        if (styleList.ContainsKey(key))
        {
          Paragraph paragraph = styleList[key];
          this.CurrentLnSpcType = paragraph.LineSpacingType;
          num = this.GetSpaceValue(paragraph._lineSpacing, paragraph._lineSpacingType);
          if (this._paragraphCollection.TextBody.IsAutoSize)
          {
            double spcReductionValue = (double) this._paragraphCollection.TextBody.GetLnSpcReductionValue();
            if (spcReductionValue != 0.0 && num == 0.0)
              num = 1.0;
            num -= num * spcReductionValue / 100000.0;
            if (this.CurrentLnSpcType == SizeType.None)
              this.CurrentLnSpcType = SizeType.Percentage;
          }
        }
      }
    }
    if (num == 0.0)
    {
      num = 1.0;
      this.CurrentLnSpcType = SizeType.Percentage;
    }
    return num;
  }

  private double GetLnSpaceValFromStyleList(Dictionary<string, Paragraph> styleList)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        this.CurrentLnSpcType = style.LineSpacingType;
        return this.GetSpaceValue(style._lineSpacing, style._lineSpacingType);
      }
    }
    return 0.0;
  }

  private double GetSpaceValue(int value, SizeType sizeType)
  {
    switch (sizeType)
    {
      case SizeType.Points:
        return (double) value / 100.0;
      case SizeType.Percentage:
        return (double) value / 100000.0;
      default:
        return 0.0;
    }
  }

  internal SizeType LineSpacingType
  {
    get => this._lineSpacingType;
    set => this._lineSpacingType = value;
  }

  internal SizeType SpaceAfterType
  {
    get => this._spaceAfterType;
    set => this._spaceAfterType = value;
  }

  internal SizeType SpaceBeforeType
  {
    get => this._spaceBeforeType;
    set => this._spaceBeforeType = value;
  }

  public double SpaceAfter
  {
    get
    {
      switch (this._spaceAfterType)
      {
        case SizeType.Points:
          this.CurrentSpcAftType = SizeType.Points;
          return (double) (this._spaceAfter / 100);
        case SizeType.Percentage:
          this.CurrentSpcAftType = SizeType.Percentage;
          return (double) (this._spaceAfter / 100000);
        default:
          return 0.0;
      }
    }
    set
    {
      if (value < 0.0 || value > 1584.0)
        throw new ArgumentException("Invalid Space After" + value.ToString());
      this.CurrentSpcAftType = SizeType.Points;
      this._spaceAfterType = SizeType.Points;
      this._spaceAfter = Convert.ToInt32(value * 100.0);
    }
  }

  internal double GetDefaultSpaceAfter()
  {
    switch (this._spaceAfterType)
    {
      case SizeType.Points:
        this.CurrentSpcAftType = SizeType.Points;
        return (double) (this._spaceAfter / 100);
      case SizeType.Percentage:
        this.CurrentSpcAftType = SizeType.Percentage;
        return (double) (this._spaceAfter / 100000);
      default:
        return this.DefaultSpaceAfter();
    }
  }

  private double DefaultSpaceAfter()
  {
    double num = 0.0;
    Dictionary<string, Paragraph> styleList1 = this._paragraphCollection.TextBody.StyleList;
    if (styleList1 != null)
      num = this.GetSpcAftValFromStyleList(styleList1);
    if (this.BaseShape != null && this.BaseShape.DrawingType != DrawingType.PlaceHolder)
      return num != 0.0 ? num : this.DefaultSpcAftFromPresentation();
    if (num == 0.0 && this.BaseShape != null)
    {
      string styleName = this.SetTempName(Syncfusion.Presentation.Drawing.Helper.GetNameFromPlaceholder(this.BaseShape.ShapeName));
      if (this.BaseSlide is Slide)
      {
        Slide baseSlide = (Slide) this.BaseSlide;
        string layoutIndex = baseSlide.ObtainLayoutIndex();
        if (layoutIndex != null)
        {
          LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
          foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
          {
            if (shape.ShapeName != null && shape.PlaceholderFormat != null && this.BaseShape.PlaceholderFormat != null && (shape.ShapeName.Contains(styleName) || (int) shape.GetPlaceholder().Index == (int) this.BaseShape.GetPlaceholder().Index))
            {
              Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
              if (styleList2 != null)
              {
                num = this.GetSpcAftValFromStyleList(styleList2);
                break;
              }
              break;
            }
          }
          if (num == 0.0)
          {
            MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
            if (styleName == "shape" && this.BaseShape.PlaceholderFormat != null)
              styleName = "Content";
            num = this.GetSpcAftValFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList);
          }
        }
      }
    }
    return num;
  }

  private double DefaultSpcAftFromPresentation()
  {
    int emu = 0;
    if (this.BaseSlide.Presentation.DefaultTextStyle != null)
    {
      Dictionary<string, Paragraph> styleList = this.BaseSlide.Presentation.DefaultTextStyle.StyleList;
      if (styleList != null && styleList.Count != 0)
      {
        string key = $"lvl{(object) (this.IndentLevelNumber + 1)}pPr";
        if (styleList.ContainsKey(key))
          emu = styleList[key]._spaceAfter;
      }
    }
    return Syncfusion.Presentation.Drawing.Helper.EmuToPoint(emu);
  }

  private double GetSpcAftValFromStyleList(Dictionary<string, Paragraph> styleList)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        this.CurrentSpcAftType = style._spaceAfterType;
        return this.GetSpaceValue(style._spaceAfter, style._spaceAfterType);
      }
    }
    return 0.0;
  }

  public double SpaceBefore
  {
    get
    {
      switch (this._spaceBeforeType)
      {
        case SizeType.Points:
          this.CurrentSpcBefType = SizeType.Points;
          return (double) this._spaceBefore / 100.0;
        case SizeType.Percentage:
          this.CurrentSpcBefType = SizeType.Percentage;
          return (double) this._spaceBefore / 100000.0;
        default:
          return 0.0;
      }
    }
    set
    {
      if (value < 0.0 || value > 1584.0)
        throw new ArgumentException("Invalid Space Before " + value.ToString());
      this.CurrentSpcBefType = SizeType.Points;
      this._spaceBeforeType = SizeType.Points;
      this._spaceBefore = Convert.ToInt32(value * 100.0);
    }
  }

  internal double GetDefaultSpaceBefore()
  {
    switch (this._spaceBeforeType)
    {
      case SizeType.Points:
        this.CurrentSpcBefType = SizeType.Points;
        return (double) this._spaceBefore / 100.0;
      case SizeType.Percentage:
        this.CurrentSpcBefType = SizeType.Percentage;
        return (double) this._spaceBefore / 100000.0;
      default:
        return this.DefaultSpaceBefore();
    }
  }

  private double DefaultSpaceBefore()
  {
    double num = 0.0;
    bool isValSet = false;
    Dictionary<string, Paragraph> styleList1 = this._paragraphCollection.TextBody.StyleList;
    if (styleList1 != null)
      num = this.GetSpcBefValFromStyleList(styleList1, ref isValSet);
    if (this.BaseShape != null && this.BaseShape.DrawingType != DrawingType.PlaceHolder)
      return num == 0.0 && !isValSet ? this.DefaultSpcBefFromPresentation() : num;
    if (num == 0.0 && !isValSet && this.BaseShape != null)
    {
      string styleName = this.SetTempName(Syncfusion.Presentation.Drawing.Helper.GetNameFromPlaceholder(this.BaseShape.ShapeName));
      if (this.BaseSlide is Slide)
      {
        Slide baseSlide = (Slide) this.BaseSlide;
        string layoutIndex = baseSlide.ObtainLayoutIndex();
        if (layoutIndex != null)
        {
          LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
          foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
          {
            if (shape.PlaceholderFormat != null)
            {
              if (this.BaseShape.PlaceholderFormat != null)
              {
                if (Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this.BaseShape.PlaceholderFormat))
                {
                  Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                  if (styleList2 != null)
                  {
                    num = this.GetSpcBefValFromStyleList(styleList2, ref isValSet);
                    break;
                  }
                  break;
                }
              }
              else
                break;
            }
          }
          if (num == 0.0 && !isValSet)
          {
            MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
            if (this.BaseShape.PlaceholderFormat == null)
            {
              if (styleName == "shape" && this.BaseShape.PlaceholderFormat != null)
                styleName = "Content";
              num = this.GetSpcBefValFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList, ref isValSet);
            }
            else
            {
              switch (this.BaseShape.GetPlaceholder().GetPlaceholderType())
              {
                case PlaceholderType.SlideNumber:
                case PlaceholderType.Header:
                case PlaceholderType.Footer:
                case PlaceholderType.Date:
                  using (IEnumerator<ISlideItem> enumerator = masterSlide.Shapes.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      Shape current = (Shape) enumerator.Current;
                      if (current.PlaceholderFormat != null && Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(current.PlaceholderFormat, this.BaseShape.PlaceholderFormat, true))
                      {
                        Dictionary<string, Paragraph> styleList3 = ((TextBody) current.TextBody).StyleList;
                        if (styleList3 != null)
                        {
                          num = this.GetSpcBefValFromStyleList(styleList3, ref isValSet);
                          break;
                        }
                        break;
                      }
                    }
                    break;
                  }
                default:
                  if (styleName == "shape" && this.BaseShape.PlaceholderFormat != null)
                    styleName = "Content";
                  num = this.GetSpcBefValFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList, ref isValSet);
                  break;
              }
            }
          }
        }
      }
    }
    return num;
  }

  private double DefaultSpcBefFromPresentation()
  {
    int num = 0;
    if (this.BaseSlide.Presentation.DefaultTextStyle != null)
    {
      Dictionary<string, Paragraph> styleList = this.BaseSlide.Presentation.DefaultTextStyle.StyleList;
      if (styleList != null && styleList.Count != 0)
      {
        string key = $"lvl{(object) (this.IndentLevelNumber + 1)}pPr";
        if (styleList.ContainsKey(key))
        {
          Paragraph paragraph = styleList[key];
          this.CurrentSpcBefType = paragraph._spaceBeforeType;
          num = paragraph._spaceBefore;
        }
      }
    }
    return this.GetSpaceValue(num, this.CurrentSpcBefType);
  }

  private double GetSpcBefValFromStyleList(
    Dictionary<string, Paragraph> styleList,
    ref bool isValSet)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        this.CurrentSpcBefType = style.SpaceBeforeType;
        if (this.CurrentSpcBefType != SizeType.None)
          isValSet = true;
        return this.GetSpaceValue(style._spaceBefore, style._spaceBeforeType);
      }
    }
    return 0.0;
  }

  public double FirstLineIndent
  {
    get => Syncfusion.Presentation.Drawing.Helper.EmuToPoint(this._indent);
    set
    {
      this._indent = value >= -4032.0 && value <= 4032.0 ? Syncfusion.Presentation.Drawing.Helper.PointToEmu(value) : throw new ArgumentException("Invalid First Line Indent" + value.ToString());
      this._isIndentChanged = true;
    }
  }

  internal double GetDefaultIndent()
  {
    if (this._isIndentChanged)
      return Syncfusion.Presentation.Drawing.Helper.EmuToPoint(this._indent);
    int emu = 0;
    bool isIndentSet = false;
    Dictionary<string, Paragraph> styleList1 = this._paragraphCollection.TextBody.StyleList;
    if (styleList1 != null)
      emu = this.GetIndentFromStyleList(styleList1, ref isIndentSet);
    if (this.BaseShape != null && this.BaseShape.DrawingType != DrawingType.PlaceHolder)
      return !isIndentSet ? this.DefaultIndentFromPresentation() : Syncfusion.Presentation.Drawing.Helper.EmuToPoint(emu);
    if (emu == 0 && !isIndentSet && this.BaseShape != null)
    {
      string styleName = this.SetTempName(Syncfusion.Presentation.Drawing.Helper.GetNameFromPlaceholder(this.BaseShape.ShapeName));
      if (this.BaseSlide is Slide)
      {
        Slide baseSlide = (Slide) this.BaseSlide;
        string layoutIndex = baseSlide.ObtainLayoutIndex();
        if (layoutIndex != null)
        {
          LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
          foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
          {
            if (shape.PlaceholderFormat != null)
            {
              if (this.BaseShape.PlaceholderFormat != null)
              {
                if (Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this.BaseShape.PlaceholderFormat))
                {
                  Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                  if (styleList2 != null)
                  {
                    emu = this.GetIndentFromStyleList(styleList2, ref isIndentSet);
                    break;
                  }
                  break;
                }
              }
              else
                break;
            }
          }
          if (emu == 0 && !isIndentSet)
          {
            MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
            if (this.BaseShape.PlaceholderFormat == null)
            {
              if (styleName == "shape" && this.BaseShape.PlaceholderFormat != null)
                styleName = "Content";
              emu = this.GetIndentFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList, ref isIndentSet);
            }
            else
            {
              switch (this.BaseShape.GetPlaceholder().GetPlaceholderType())
              {
                case PlaceholderType.SlideNumber:
                case PlaceholderType.Header:
                case PlaceholderType.Footer:
                case PlaceholderType.Date:
                  using (IEnumerator<ISlideItem> enumerator = masterSlide.Shapes.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      Shape current = (Shape) enumerator.Current;
                      if (current.PlaceholderFormat != null && Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(current.PlaceholderFormat, this.BaseShape.PlaceholderFormat, true))
                      {
                        Dictionary<string, Paragraph> styleList3 = ((TextBody) current.TextBody).StyleList;
                        if (styleList3 != null)
                        {
                          emu = this.GetIndentFromStyleList(styleList3, ref isIndentSet);
                          break;
                        }
                        break;
                      }
                    }
                    break;
                  }
                default:
                  if (styleName == "shape" && this.BaseShape.PlaceholderFormat != null)
                    styleName = "Content";
                  emu = this.GetIndentFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList, ref isIndentSet);
                  break;
              }
            }
          }
        }
      }
    }
    return Syncfusion.Presentation.Drawing.Helper.EmuToPoint(emu);
  }

  private double DefaultIndentFromPresentation()
  {
    int emu = 0;
    if (this.BaseSlide.Presentation.DefaultTextStyle != null)
    {
      Dictionary<string, Paragraph> styleList = this.BaseSlide.Presentation.DefaultTextStyle.StyleList;
      if (styleList != null && styleList.Count != 0)
      {
        string key = $"lvl{(object) (this.IndentLevelNumber + 1)}pPr";
        if (styleList.ContainsKey(key))
          emu = styleList[key]._indent;
      }
    }
    return Syncfusion.Presentation.Drawing.Helper.EmuToPoint(emu);
  }

  private long DefaultTabSizeFromPresentation()
  {
    long num = -1;
    if (this.BaseSlide.Presentation.DefaultTextStyle != null)
    {
      Dictionary<string, Paragraph> styleList = this.BaseSlide.Presentation.DefaultTextStyle.StyleList;
      if (styleList != null && styleList.Count != 0)
      {
        string key = $"lvl{(object) (this.IndentLevelNumber + 1)}pPr";
        if (styleList.ContainsKey(key))
          num = styleList[key]._defTabSz;
      }
    }
    return num;
  }

  private int GetIndentFromStyleList(Dictionary<string, Paragraph> styleList, ref bool isIndentSet)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        isIndentSet = style._isIndentChanged;
        return style._indent;
      }
    }
    return 0;
  }

  private long GetTabSizeFromStyleList(
    Dictionary<string, Paragraph> styleList,
    ref bool isTabSizeSet)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        isTabSizeSet = style._isTabSizeChanged;
        return style._defTabSz;
      }
    }
    return -1;
  }

  public double LeftIndent
  {
    get => Syncfusion.Presentation.Drawing.Helper.EmuToPoint(this._marginLeft);
    set
    {
      this._marginLeft = value >= 0.0 && value <= 4032.0 ? Syncfusion.Presentation.Drawing.Helper.PointToEmu(value) : throw new ArgumentException("Invalid Left Indent" + value.ToString());
      this._isMarginChanged = true;
    }
  }

  internal double GetDefaultMarginLeft()
  {
    if (this._isMarginChanged)
      return Syncfusion.Presentation.Drawing.Helper.EmuToPoint(this._marginLeft);
    int emu = 0;
    bool isMarginSet = false;
    Dictionary<string, Paragraph> styleList1 = this._paragraphCollection.TextBody.StyleList;
    if (styleList1 != null)
      emu = this.GetMarginFromStyleList(styleList1, ref isMarginSet);
    if (this.BaseShape != null && this.BaseShape.DrawingType != DrawingType.PlaceHolder)
      return !isMarginSet ? this.DefaultMarginFromPresentation() : Syncfusion.Presentation.Drawing.Helper.EmuToPoint(emu);
    if (emu == 0 && !isMarginSet && this.BaseShape != null)
    {
      string styleName = this.SetTempName(Syncfusion.Presentation.Drawing.Helper.GetNameFromPlaceholder(this.BaseShape.ShapeName));
      if (this.BaseSlide is Slide)
      {
        Slide baseSlide = (Slide) this.BaseSlide;
        string layoutIndex = baseSlide.ObtainLayoutIndex();
        if (layoutIndex != null)
        {
          LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
          foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
          {
            if (shape.PlaceholderFormat != null)
            {
              if (this.BaseShape.PlaceholderFormat != null)
              {
                if (Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this.BaseShape.PlaceholderFormat))
                {
                  Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                  if (styleList2 != null)
                  {
                    emu = this.GetMarginFromStyleList(styleList2, ref isMarginSet);
                    break;
                  }
                  break;
                }
              }
              else
                break;
            }
          }
          if (emu == 0 && !isMarginSet)
          {
            MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
            if (this.BaseShape.PlaceholderFormat == null)
            {
              if (styleName == "shape" && this.BaseShape.PlaceholderFormat != null)
                styleName = "Content";
              emu = this.GetMarginFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList, ref isMarginSet);
            }
            else
            {
              switch (this.BaseShape.GetPlaceholder().GetPlaceholderType())
              {
                case PlaceholderType.SlideNumber:
                case PlaceholderType.Header:
                case PlaceholderType.Footer:
                case PlaceholderType.Date:
                  using (IEnumerator<ISlideItem> enumerator = masterSlide.Shapes.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      Shape current = (Shape) enumerator.Current;
                      if (current.PlaceholderFormat != null && Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(current.PlaceholderFormat, this.BaseShape.PlaceholderFormat, true))
                      {
                        Dictionary<string, Paragraph> styleList3 = ((TextBody) current.TextBody).StyleList;
                        if (styleList3 != null)
                        {
                          emu = this.GetMarginFromStyleList(styleList3, ref isMarginSet);
                          break;
                        }
                        break;
                      }
                    }
                    break;
                  }
                default:
                  if (styleName == "shape" && this.BaseShape.PlaceholderFormat != null)
                    styleName = "Content";
                  emu = this.GetMarginFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList, ref isMarginSet);
                  break;
              }
            }
          }
        }
      }
    }
    return Syncfusion.Presentation.Drawing.Helper.EmuToPoint(emu);
  }

  private double DefaultMarginFromPresentation()
  {
    int emu = 0;
    if (this.BaseSlide.Presentation.DefaultTextStyle != null)
    {
      Dictionary<string, Paragraph> styleList = this.BaseSlide.Presentation.DefaultTextStyle.StyleList;
      if (styleList != null && styleList.Count != 0)
      {
        string key = $"lvl{(object) (this.IndentLevelNumber + 1)}pPr";
        if (styleList.ContainsKey(key))
          emu = styleList[key]._marginLeft;
      }
    }
    return Syncfusion.Presentation.Drawing.Helper.EmuToPoint(emu);
  }

  private int GetMarginFromStyleList(Dictionary<string, Paragraph> styleList, ref bool isMarginSet)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        isMarginSet = style._isMarginChanged;
        return style._marginLeft;
      }
    }
    return 0;
  }

  internal BaseSlide BaseSlide => this._paragraphCollection.BaseSlide;

  internal ParagraphInfo ParagraphInfo => this._paragrahInfo;

  internal StringFormat StringFormt
  {
    get
    {
      if (this._stringformat == null)
      {
        this._stringformat = new StringFormat(StringFormat.GenericTypographic);
        this._stringformat.FormatFlags &= ~StringFormatFlags.LineLimit;
        this._stringformat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
        this._stringformat.FormatFlags |= StringFormatFlags.NoClip;
        bool? defaultRtl = this.GetDefaultRTL();
        if (defaultRtl.HasValue)
        {
          bool? nullable = defaultRtl;
          if ((!nullable.GetValueOrDefault() ? 0 : (nullable.HasValue ? 1 : 0)) != 0)
            this._stringformat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
        }
        this._stringformat.Trimming = StringTrimming.Word;
      }
      return this._stringformat;
    }
  }

  internal Paragraphs GetParagraphCollection() => this._paragraphCollection;

  internal void SetParagraphCollection(Paragraphs paragraphs)
  {
    this._paragraphCollection = paragraphs;
  }

  internal int GetDefaultKernValue(Dictionary<string, Paragraph> styleList)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        if ((style.Font as Syncfusion.Presentation.RichText.Font).Kerning.HasValue)
        {
          int? kerning = (style.Font as Syncfusion.Presentation.RichText.Font).Kerning;
          return (kerning.HasValue ? new int?(kerning.GetValueOrDefault() / 1000) : new int?()).Value;
        }
      }
    }
    return 0;
  }

  internal TextBody GetTextBody() => this._paragraphCollection.TextBody;

  internal void SetIndentLevel(int indentLevelNumber) => this._indentLevel = indentLevelNumber;

  internal int GetIndentLevel() => this._indentLevel;

  internal void SetBulletCharacter(Syncfusion.Presentation.RichText.ListFormat bulletFormat)
  {
    if (!this._paragraphCollection.LevelHolder.ContainsKey(this._indentLevel))
      return;
    bulletFormat.SetCharacter(this._paragraphCollection.LevelHolder[this._indentLevel].Number.ToString((IFormatProvider) CultureInfo.InvariantCulture));
  }

  internal int ObtainLineSpacing() => this._lineSpacing;

  internal int ObtainSpaceBefore() => this._spaceBefore;

  internal int ObtainSpaceAfter() => this._spaceAfter;

  internal void SetLineSpacing(int lineSpacing) => this._lineSpacing = lineSpacing;

  internal void SetSpaceBefore(int spaceBefore) => this._spaceBefore = spaceBefore;

  internal void SetSpaceAfter(int spaceAfter) => this._spaceAfter = spaceAfter;

  internal void SetAlignmentType(Syncfusion.Presentation.HorizontalAlignment textAlignmentType)
  {
    this._textAlignmentType = textAlignmentType;
    this._isAlignChanged = true;
  }

  internal void SetFontAlignType(FontAlignmentType fontAlignmentType)
  {
    this._fontAlignmentType = fontAlignmentType;
  }

  internal void SetMarginLeft(int marginLeft)
  {
    this._marginLeft = marginLeft;
    this._isMarginChanged = true;
  }

  internal int GetMarginLeft()
  {
    if (!this._isMarginChanged)
      this._marginLeft += Convert.ToInt32(457200.0 * (double) this._indentLevel);
    return this._marginLeft;
  }

  internal void SetMarginRight(int marginRight) => this._marginRight = marginRight;

  internal int GetMarginRight() => this._marginRight;

  internal bool IsMarginChanged => this._isMarginChanged;

  internal void SetIndent(int indent)
  {
    this._indent = indent;
    this._isIndentChanged = true;
  }

  internal int GetIndent() => this._indent;

  internal Syncfusion.Presentation.HorizontalAlignment GetAlignmentType()
  {
    return this._textAlignmentType;
  }

  internal FontAlignmentType GetFontAlignType() => this._fontAlignmentType;

  internal void SetDefaultTabSize(long defaultTabSize)
  {
    this._defTabSz = defaultTabSize;
    this._isTabSizeChanged = true;
  }

  internal long GetDefaultTabsize()
  {
    if (this._isTabSizeChanged)
      return this._defTabSz;
    long defaultTabsize = -1;
    bool isTabSizeSet = false;
    Dictionary<string, Paragraph> styleList1 = this._paragraphCollection.TextBody.StyleList;
    if (styleList1 != null)
      defaultTabsize = this.GetTabSizeFromStyleList(styleList1, ref isTabSizeSet);
    if (this.BaseShape != null && this.BaseShape.DrawingType != DrawingType.PlaceHolder)
      return !isTabSizeSet ? this.DefaultTabSizeFromPresentation() : defaultTabsize;
    if (defaultTabsize == -1L && !isTabSizeSet && this.BaseShape != null)
    {
      string styleName = this.SetTempName(Syncfusion.Presentation.Drawing.Helper.GetNameFromPlaceholder(this.BaseShape.ShapeName));
      if (this.BaseSlide is Slide)
      {
        Slide baseSlide = (Slide) this.BaseSlide;
        string layoutIndex = baseSlide.ObtainLayoutIndex();
        if (layoutIndex != null)
        {
          LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
          foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
          {
            if (shape.PlaceholderFormat != null)
            {
              if (this.BaseShape.PlaceholderFormat != null)
              {
                if (Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this.BaseShape.PlaceholderFormat))
                {
                  Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                  if (styleList2 != null)
                  {
                    defaultTabsize = this.GetTabSizeFromStyleList(styleList2, ref isTabSizeSet);
                    break;
                  }
                  break;
                }
              }
              else
                break;
            }
          }
          if (defaultTabsize == -1L && !isTabSizeSet)
          {
            MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
            if (this.BaseShape.PlaceholderFormat == null)
            {
              if (styleName == "shape" && this.BaseShape.PlaceholderFormat != null)
                styleName = "Content";
              defaultTabsize = this.GetTabSizeFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList, ref isTabSizeSet);
            }
            else
            {
              switch (this.BaseShape.GetPlaceholder().GetPlaceholderType())
              {
                case PlaceholderType.SlideNumber:
                case PlaceholderType.Header:
                case PlaceholderType.Footer:
                case PlaceholderType.Date:
                  using (IEnumerator<ISlideItem> enumerator = masterSlide.Shapes.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      Shape current = (Shape) enumerator.Current;
                      if (current.PlaceholderFormat != null && Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(current.PlaceholderFormat, this.BaseShape.PlaceholderFormat, true))
                      {
                        Dictionary<string, Paragraph> styleList3 = ((TextBody) current.TextBody).StyleList;
                        if (styleList3 != null)
                        {
                          defaultTabsize = this.GetTabSizeFromStyleList(styleList3, ref isTabSizeSet);
                          break;
                        }
                        break;
                      }
                    }
                    break;
                  }
                default:
                  if (styleName == "shape" && this.BaseShape.PlaceholderFormat != null)
                    styleName = "Content";
                  defaultTabsize = this.GetTabSizeFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList, ref isTabSizeSet);
                  if (defaultTabsize == -1L)
                  {
                    defaultTabsize = this.GetTabSizeFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, "default").StyleList, ref isTabSizeSet);
                    break;
                  }
                  break;
              }
            }
          }
        }
        return defaultTabsize != -1L || isTabSizeSet ? defaultTabsize : this.DefaultTabSizeFromPresentation();
      }
    }
    return defaultTabsize;
  }

  internal bool ContainsLineBreak()
  {
    foreach (ITextPart textPart in (IEnumerable<ITextPart>) this.TextParts)
    {
      if ((textPart as TextPart).GetLineBreakProps() != null)
        return true;
    }
    return false;
  }

  internal Paragraphs Parent => this._paragraphCollection;

  internal void Add(TextPart textPart) => this._textPartCollection.Add((ITextPart) textPart);

  public IFont Font => (IFont) this._defaultFont ?? (IFont) (this._defaultFont = new Syncfusion.Presentation.RichText.Font(this));

  internal Syncfusion.Presentation.RichText.Font GetDefaultParagraphFont()
  {
    Syncfusion.Presentation.RichText.Font font = (Syncfusion.Presentation.RichText.Font) null;
    if (this._isLast)
      font = this._charProps;
    if (font == null)
    {
      Dictionary<string, Paragraph> styleList1 = this._paragraphCollection.TextBody.StyleList;
      if (styleList1 != null)
        font = this.GetFontFromStyleList(styleList1);
      if (font == null)
      {
        string styleName = this.SetTempName(Syncfusion.Presentation.Drawing.Helper.GetNameFromPlaceholder(this.BaseShape.ShapeName));
        if (this.BaseSlide is Slide)
        {
          Slide baseSlide = (Slide) this.BaseSlide;
          string layoutIndex = baseSlide.ObtainLayoutIndex();
          if (layoutIndex != null)
          {
            LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
            foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
            {
              if (shape.PlaceholderFormat != null)
              {
                if (this.BaseShape.PlaceholderFormat != null)
                {
                  if (Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this.BaseShape.PlaceholderFormat))
                  {
                    Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                    if (styleList2 != null)
                    {
                      font = this.GetFontFromStyleList(styleList2);
                      break;
                    }
                    break;
                  }
                }
                else
                  break;
              }
            }
            if (font == null)
            {
              MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
              if (this.BaseShape.PlaceholderFormat == null)
              {
                if (styleName == "shape" && this.BaseShape.PlaceholderFormat != null)
                  styleName = "Content";
                font = this.GetFontFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList);
              }
              else
              {
                switch (this.BaseShape.GetPlaceholder().GetPlaceholderType())
                {
                  case PlaceholderType.SlideNumber:
                  case PlaceholderType.Header:
                  case PlaceholderType.Footer:
                  case PlaceholderType.Date:
                    using (IEnumerator<ISlideItem> enumerator = masterSlide.Shapes.GetEnumerator())
                    {
                      while (enumerator.MoveNext())
                      {
                        Shape current = (Shape) enumerator.Current;
                        if (current.PlaceholderFormat != null && Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(current.PlaceholderFormat, this.BaseShape.PlaceholderFormat))
                        {
                          Dictionary<string, Paragraph> styleList3 = ((TextBody) current.TextBody).StyleList;
                          if (styleList3 != null)
                          {
                            font = this.GetFontFromStyleList(styleList3);
                            break;
                          }
                          break;
                        }
                      }
                      break;
                    }
                  default:
                    if (styleName == "shape" && this.BaseShape.PlaceholderFormat != null)
                      styleName = "Content";
                    font = this.GetFontFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList);
                    break;
                }
              }
            }
          }
        }
      }
    }
    return font ?? new Syncfusion.Presentation.RichText.Font(this);
  }

  private Syncfusion.Presentation.RichText.Font GetFontFromStyleList(
    Dictionary<string, Paragraph> styleList)
  {
    if (styleList != null && styleList.Count != 0)
    {
      string key = $"lvl{(object) (this.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
        return styleList[key].GetFontObject();
    }
    return (Syncfusion.Presentation.RichText.Font) null;
  }

  private System.Drawing.Font GetFont(IFont font, FontScriptType scriptType)
  {
    FontStyle fontStyle = this.GetFontStyle(font, scriptType);
    string fontName = ((Syncfusion.Presentation.RichText.Font) font).GetFontNameToRender(scriptType);
    if (fontName == "Courier")
      fontName = "Courier New";
    if (fontName == "Calibri Bold")
      fontName = "Calibri";
    return this.Presentation.FontSettings.GetFont(fontName, ((Syncfusion.Presentation.RichText.Font) font).GetDefaultSize(), fontStyle);
  }

  private SizeF MeasurePictureBullet(float bulletSize, Syncfusion.Drawing.Image image)
  {
    SizeF sizeF = new SizeF((float) image.Width, (float) image.Height);
    float num1 = bulletSize;
    float num2 = (float) Math.Round((double) sizeF.Width / (double) sizeF.Height, MidpointRounding.AwayFromZero);
    float num3 = (float) Math.Round((double) sizeF.Height / (double) sizeF.Width, MidpointRounding.AwayFromZero);
    float num4 = (double) num2 <= 0.0 ? 1f : num2;
    float num5 = (double) num3 <= 0.0 ? 1f : num3;
    sizeF.Width = (double) num1 <= 18.0 ? num4 * num1 : num4 * (float) ((double) num1 + 15.0 - (double) num1 / 2.0);
    sizeF.Height = (double) num1 <= 18.0 ? num5 * num1 : (float) ((double) num1 + 15.0 - (double) num1 / 2.0);
    if ((double) num4 != (double) num5)
    {
      float num6 = (double) num4 <= (double) num5 ? ((double) num1 <= 18.0 ? sizeF.Width - num1 : sizeF.Height - sizeF.Width) : ((double) num1 <= 18.0 ? sizeF.Width - num1 : sizeF.Width - sizeF.Height);
      sizeF.Width = num6 + (float) ((double) num1 / 2.0 - 3.0);
    }
    return sizeF;
  }

  private void SplitTextParts()
  {
    TextSplitter splitter = new TextSplitter();
    int num;
    for (int index1 = 0; index1 < this.TextParts.Count; index1 += num)
    {
      num = 1;
      TextPart textPart1 = this.TextParts[index1] as TextPart;
      List<FontScriptType> fontScriptTypes = new List<FontScriptType>();
      string[] strArray = splitter.SplitTextByFontScriptType(textPart1.Text, ref fontScriptTypes);
      if (strArray.Length > 1)
      {
        for (int index2 = 0; index2 < strArray.Length; ++index2)
        {
          string str = strArray[index2];
          if (index2 > 0)
          {
            TextPart textPart2 = textPart1.Clone();
            textPart2.Text = str;
            textPart2.ScriptType = fontScriptTypes[index2];
            (this.TextParts as Syncfusion.Presentation.RichText.TextParts).Insert(index1 + index2, (ITextPart) textPart2);
            ++num;
          }
          else
          {
            textPart1.Text = str;
            textPart1.ScriptType = fontScriptTypes[index2];
          }
        }
      }
      else if (strArray.Length > 0)
        textPart1.ScriptType = fontScriptTypes[0];
      fontScriptTypes.Clear();
    }
    this.SplitTextPartsByRTL(splitter);
  }

  private bool HasWordSplitCharacter(List<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection)
  {
    string empty = string.Empty;
    foreach (Syncfusion.Presentation.Layouting.TextInfo textInfo in textInfoCollection)
    {
      if (textInfo.TextPart != null)
        empty += textInfo.Text;
    }
    return empty.Contains(" ") || empty.Contains("-") || empty.Contains("\t");
  }

  internal Syncfusion.Presentation.RichText.Font GetEndParaProps() => this._charProps;

  internal void SetEndParaProps(IFont sourceFont) => this._charProps = sourceFont as Syncfusion.Presentation.RichText.Font;

  internal Syncfusion.Presentation.RichText.Font GetFontObject() => this._defaultFont;

  internal bool GetIsLastPara() => this._isLast;

  internal void SetEndParaProps(Syncfusion.Presentation.RichText.Font font)
  {
    this._charProps = font;
  }

  internal bool IsWithinList
  {
    get => this._isWithinList;
    set => this._isWithinList = value;
  }

  internal void SetFont(Syncfusion.Presentation.RichText.Font font) => this._defaultFont = font;

  internal void SetIsLastPara(bool value) => this._isLast = value;

  internal bool? GetRTLFromStyleList(Dictionary<string, Paragraph> styleList)
  {
    string key = $"lvl{(object) (this.IndentLevelNumber + 1)}pPr";
    if (styleList.ContainsKey(key))
    {
      Paragraph style = styleList[key];
      if (style.RightToLeft.HasValue)
      {
        bool? rightToLeft = style.RightToLeft;
        if ((!rightToLeft.GetValueOrDefault() ? 0 : (rightToLeft.HasValue ? 1 : 0)) != 0)
          return new bool?(true);
      }
      if (style.RightToLeft.HasValue)
        return new bool?(false);
    }
    return new bool?();
  }

  internal bool? GetDefaultRTL()
  {
    bool? defaultRtl = new bool?();
    if (this._rightToLeft.HasValue)
    {
      bool? rightToLeft = this._rightToLeft;
      if ((!rightToLeft.GetValueOrDefault() ? 0 : (rightToLeft.HasValue ? 1 : 0)) != 0)
        return new bool?(true);
    }
    if (this._rightToLeft.HasValue)
      return new bool?(false);
    if (this.BaseShape != null && this.BaseShape.DrawingType == DrawingType.PlaceHolder)
      return this.DefaultRTL();
    if (this.BaseShape != null && !(this.BaseShape is Table))
    {
      if (((TextBody) this.BaseShape.TextBody).StyleList != null)
        defaultRtl = this.GetRTLFromStyleList(((TextBody) this.BaseShape.TextBody).StyleList);
      if (!defaultRtl.HasValue)
        return this.DefaultRTLFromPresentation();
    }
    return defaultRtl;
  }

  private bool? DefaultRTL()
  {
    bool? nullable = new bool?();
    if (this._rightToLeft.HasValue)
    {
      bool? rightToLeft = this._rightToLeft;
      if ((!rightToLeft.GetValueOrDefault() ? 0 : (rightToLeft.HasValue ? 1 : 0)) != 0)
        return new bool?(true);
    }
    Dictionary<string, Paragraph> styleList1 = ((TextBody) this.BaseShape.TextBody).StyleList;
    if (styleList1 != null)
      nullable = this.GetRTLFromStyleList(styleList1);
    if (!nullable.HasValue)
    {
      string styleName = this.SetTempName(Syncfusion.Presentation.Drawing.Helper.GetNameFromPlaceholder(this.BaseShape.ShapeName));
      if (this.BaseSlide is Slide)
      {
        Slide baseSlide = (Slide) this.BaseSlide;
        string layoutIndex = baseSlide.ObtainLayoutIndex();
        if (layoutIndex != null)
        {
          LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
          foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
          {
            if (shape.PlaceholderFormat != null)
            {
              if (this.BaseShape.PlaceholderFormat != null)
              {
                if (Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this.BaseShape.PlaceholderFormat))
                {
                  Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                  if (styleList2 != null)
                  {
                    nullable = this.GetRTLFromStyleList(styleList2);
                    break;
                  }
                  break;
                }
              }
              else
                break;
            }
          }
          if (!nullable.HasValue)
          {
            MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
            if (this.BaseShape.PlaceholderFormat == null)
            {
              if (styleName == "shape" && this.BaseShape.PlaceholderFormat != null)
                styleName = "Content";
              nullable = this.GetRTLFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList);
            }
            else
            {
              switch (this.BaseShape.GetPlaceholder().GetPlaceholderType())
              {
                case PlaceholderType.SlideNumber:
                case PlaceholderType.Header:
                case PlaceholderType.Footer:
                case PlaceholderType.Date:
                  using (IEnumerator<ISlideItem> enumerator = masterSlide.Shapes.GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                    {
                      Shape current = (Shape) enumerator.Current;
                      if (current.PlaceholderFormat != null && Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(current.PlaceholderFormat, this.BaseShape.PlaceholderFormat, true))
                      {
                        Dictionary<string, Paragraph> styleList3 = ((TextBody) current.TextBody).StyleList;
                        if (styleList3 != null)
                        {
                          nullable = this.GetRTLFromStyleList(styleList3);
                          break;
                        }
                        break;
                      }
                    }
                    break;
                  }
                default:
                  if (styleName == "shape" && this.BaseShape.PlaceholderFormat != null)
                    styleName = "Content";
                  nullable = this.GetRTLFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList);
                  break;
              }
            }
          }
        }
      }
      else if (this.BaseSlide is LayoutSlide)
      {
        LayoutSlide baseSlide = this.BaseSlide as LayoutSlide;
        foreach (Shape shape in (IEnumerable<ISlideItem>) baseSlide.Shapes)
        {
          if (shape.PlaceholderFormat != null)
          {
            if (this.BaseShape.PlaceholderFormat != null)
            {
              if (Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this.BaseShape.PlaceholderFormat))
              {
                Dictionary<string, Paragraph> styleList4 = ((TextBody) shape.TextBody).StyleList;
                if (styleList4 != null)
                {
                  nullable = this.GetRTLFromStyleList(styleList4);
                  break;
                }
                break;
              }
            }
            else
              break;
          }
        }
        if (!nullable.HasValue)
        {
          MasterSlide masterSlide = (MasterSlide) baseSlide.MasterSlide;
          if (this.BaseShape.PlaceholderFormat == null)
          {
            if (styleName == "shape" && this.BaseShape.PlaceholderFormat != null)
              styleName = "Content";
            nullable = this.GetRTLFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList);
          }
          else
          {
            switch (this.BaseShape.GetPlaceholder().GetPlaceholderType())
            {
              case PlaceholderType.SlideNumber:
              case PlaceholderType.Header:
              case PlaceholderType.Footer:
              case PlaceholderType.Date:
                using (IEnumerator<ISlideItem> enumerator = masterSlide.Shapes.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    Shape current = (Shape) enumerator.Current;
                    if (current.PlaceholderFormat != null && Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(current.PlaceholderFormat, this.BaseShape.PlaceholderFormat, true))
                    {
                      Dictionary<string, Paragraph> styleList5 = ((TextBody) current.TextBody).StyleList;
                      if (styleList5 != null)
                      {
                        nullable = this.GetRTLFromStyleList(styleList5);
                        break;
                      }
                      break;
                    }
                  }
                  break;
                }
              default:
                if (styleName == "shape" && this.BaseShape.PlaceholderFormat != null)
                  styleName = "Content";
                nullable = this.GetRTLFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList);
                break;
            }
          }
        }
      }
      else if (this.BaseSlide is MasterSlide)
      {
        MasterSlide baseSlide = this.BaseSlide as MasterSlide;
        if (this.BaseShape.PlaceholderFormat == null)
        {
          if (styleName == "shape" && this.BaseShape.PlaceholderFormat != null)
            styleName = "Content";
          nullable = this.GetRTLFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(baseSlide, styleName).StyleList);
        }
        else
        {
          switch (this.BaseShape.GetPlaceholder().GetPlaceholderType())
          {
            case PlaceholderType.SlideNumber:
            case PlaceholderType.Header:
            case PlaceholderType.Footer:
            case PlaceholderType.Date:
              using (IEnumerator<ISlideItem> enumerator = baseSlide.Shapes.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  Shape current = (Shape) enumerator.Current;
                  if (current.PlaceholderFormat != null && Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(current.PlaceholderFormat, this.BaseShape.PlaceholderFormat, true))
                  {
                    Dictionary<string, Paragraph> styleList6 = ((TextBody) current.TextBody).StyleList;
                    if (styleList6 != null)
                    {
                      nullable = this.GetRTLFromStyleList(styleList6);
                      break;
                    }
                    break;
                  }
                }
                break;
              }
            default:
              if (styleName == "shape" && this.BaseShape.PlaceholderFormat != null)
                styleName = "Content";
              nullable = this.GetRTLFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(baseSlide, styleName).StyleList);
              break;
          }
        }
      }
    }
    return nullable;
  }

  private bool? DefaultRTLFromPresentation()
  {
    bool? nullable = new bool?();
    if (this._rightToLeft.HasValue)
    {
      bool? rightToLeft = this._rightToLeft;
      if ((!rightToLeft.GetValueOrDefault() ? 0 : (rightToLeft.HasValue ? 1 : 0)) != 0)
        nullable = new bool?(true);
    }
    return this.BaseShape.BaseSlide.Presentation.DefaultTextStyle != null ? this.GetRTLFromStyleList(this.BaseShape.BaseSlide.Presentation.DefaultTextStyle.StyleList) : nullable;
  }

  internal bool IsRTLLanguage(string text)
  {
    return !Syncfusion.Presentation.Drawing.Helper.IsNullOrWhiteSpace(text) && (text.Contains("he") || text.Contains("ar"));
  }

  internal bool IsRTL(string text)
  {
    if (text != null)
    {
      for (int index = 0; index < text.Length; ++index)
      {
        if (TextSplitter.IsRTLChar(text[index]))
          return true;
      }
    }
    return false;
  }

  internal bool IsRTLText(string text)
  {
    bool flag = false;
    if (text != null)
    {
      for (int index = 0; index < text.Length; ++index)
      {
        char ch = text[index];
        if (ch >= '\u0590' && ch <= '\u05FF' || ch >= '\u0600' && ch <= 'ۿ' || ch >= 'ݐ' && ch <= 'ݿ' || ch >= 'ࢠ' && ch <= 'ࣿ' || ch >= 'ﭐ' && ch <= '\uFDFF' || ch >= 'ﹰ' && ch <= '\uFEFF' || ch >= 'ꦀ' && ch <= '꧟' || ch >= '܀' && ch <= 'ݏ' || ch >= 'ހ' && ch <= '\u07BF' || ch >= 'ࡀ' && ch <= '\u085F' || ch >= '߀' && ch <= '\u07FF' || ch >= 'ࠀ' && ch <= '\u083F' || ch >= 'ⴰ' && ch <= '⵿')
        {
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  private bool IsRightToLeftLang(string lang)
  {
    return lang == "ar-AE" || lang == "ar-BH" || lang == "ar-DZ" || lang == "ar-EG" || lang == "ar-IQ" || lang == "ar-JO" || lang == "ar-KW" || lang == "ar-LB" || lang == "ar-LY" || lang == "ar-OM" || lang == "ar-QA" || lang == "ar-SA" || lang == "ar-SY" || lang == "ar-TN" || lang == "ar-YE";
  }

  private void SplitTextPartsByRTL(TextSplitter splitter)
  {
    bool? isPrevLTRText = new bool?();
    bool hasRTLCharacter = false;
    List<CharacterRangeType> characterRangeTypes = new List<CharacterRangeType>();
    int num;
    for (int index1 = 0; index1 < this.TextParts.Count; index1 += num)
    {
      num = 1;
      TextPart textPart1 = this.TextParts[index1] as TextPart;
      string text = textPart1.Text;
      bool isTextBidi = false;
      string defaultLanguage = (textPart1.Font as Syncfusion.Presentation.RichText.Font).GetDefaultLanguage();
      if (this.IsRTLLanguage(defaultLanguage))
        isTextBidi = (textPart1.Font as Syncfusion.Presentation.RichText.Font).Bidi = true;
      bool isRTLLang = false;
      if (isTextBidi && this.IsRightToLeftLang(defaultLanguage))
        isRTLLang = true;
      int count = characterRangeTypes.Count;
      string[] strArray = splitter.SplitTextByConsecutiveLtrAndRtl(text, isTextBidi, isRTLLang, ref characterRangeTypes, ref isPrevLTRText, ref hasRTLCharacter);
      if (strArray.Length > 1)
      {
        for (int index2 = 0; index2 < strArray.Length; ++index2)
        {
          string str = strArray[index2];
          if (index2 > 0)
          {
            TextPart textPart2 = textPart1.Clone();
            textPart2.Text = str;
            textPart2.CharacterRange = characterRangeTypes[index2 + count];
            if ((textPart2.CharacterRange == CharacterRangeType.Number || textPart2.CharacterRange == CharacterRangeType.WordSplit) && isTextBidi)
            {
              if (defaultLanguage.TrimStart().ToLower().StartsWith("he-"))
                textPart2.ScriptType = FontScriptType.Hebrew;
              else if (defaultLanguage.TrimStart().ToLower().StartsWith("ar-"))
                textPart2.ScriptType = FontScriptType.Arabic;
            }
            (this.TextParts as Syncfusion.Presentation.RichText.TextParts).Insert(index1 + index2, (ITextPart) textPart2);
            ++num;
          }
          else
          {
            textPart1.Text = str;
            textPart1.CharacterRange = characterRangeTypes[index2 + count];
          }
        }
      }
      else if (strArray.Length > 0)
        textPart1.CharacterRange = characterRangeTypes[count];
      if ((textPart1.CharacterRange == CharacterRangeType.Number || textPart1.CharacterRange == CharacterRangeType.WordSplit) && isTextBidi)
      {
        if (defaultLanguage.TrimStart().ToLower().StartsWith("he-"))
          textPart1.ScriptType = FontScriptType.Hebrew;
        else if (defaultLanguage.TrimStart().ToLower().StartsWith("ar-"))
          textPart1.ScriptType = FontScriptType.Arabic;
      }
    }
    characterRangeTypes.Clear();
    this.CombineconsecutiveRTL();
  }

  private void CombineconsecutiveRTL()
  {
    for (int index = 0; index <= this.TextParts.Count - 2; ++index)
    {
      TextPart textPart1 = this.TextParts[index] as TextPart;
      TextPart textPart2 = this.TextParts[index + 1] as TextPart;
      if (textPart1.CharacterRange == CharacterRangeType.RTL && textPart2.CharacterRange == CharacterRangeType.RTL && textPart1.Text.Length > 0 && textPart2.Text.Length > 0 && !TextSplitter.IsWordSplitChar(textPart1.Text[textPart1.Text.Length - 1]) && !TextSplitter.IsWordSplitChar(textPart2.Text[0]) && (textPart1.Font as Syncfusion.Presentation.RichText.Font).Compare(textPart2.Font as Syncfusion.Presentation.RichText.Font))
      {
        textPart1.Text += textPart2.Text;
        (this.TextParts as Syncfusion.Presentation.RichText.TextParts).RemoveAt(index + 1);
        --index;
      }
    }
  }

  internal void Layout(
    RectangleF shapeBounds,
    ref float usedHeight,
    bool isWrap,
    ref float maxWidth)
  {
    this.SplitTextParts();
    this._paragrahInfo = new ParagraphInfo((IParagraph) this);
    List<LineInfo> lineInfoCollection = this._paragrahInfo.LineInfoCollection;
    List<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection = new List<Syncfusion.Presentation.Layouting.TextInfo>();
    float usedWidth = 0.0f;
    float maxAscent = 0.0f;
    float maxHeight1 = 0.0f;
    float marginLeft = 0.0f;
    float usedHeight1 = usedHeight;
    SizeF bulletSize = new SizeF();
    float indentWidth = 0.0f;
    this.CalculateBulletSize(this.ListFormat, ref usedWidth, ref usedHeight, shapeBounds, ref bulletSize, ref marginLeft, ref textInfoCollection, ref indentWidth);
    float defaultTabsize = (float) this.GetDefaultTabsize();
    float num1 = (double) defaultTabsize == -1.0 ? 72f : (float) Syncfusion.Presentation.Drawing.Helper.EmuToPoint((double) defaultTabsize);
    float num2 = usedWidth;
    bool flag = false;
    if (!string.IsNullOrEmpty(this.Text))
      flag = this.Text[0] == '\t';
    for (int index1 = 0; index1 < this.TextParts.Count; ++index1)
    {
      TextPart textPart = this.TextParts[index1] as TextPart;
      string text1 = textPart.Text;
      if (textPart.Type == "slidenum" && (text1 == "‹#›" || text1 == "‹N°›") && this.BaseSlide.Presentation.GetExportingSlide() != null)
        text1 = this.BaseSlide.Presentation.GetExportingSlide().SlideNumber.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      if (textPart.GetLineBreakProps() != null)
      {
        IFont lineBreakProps = textPart.GetLineBreakProps();
        System.Drawing.Font font1 = this.Presentation.FontSettings.GetFont(((Syncfusion.Presentation.RichText.Font) lineBreakProps).GetFontNameToRender(textPart.ScriptType), ((Syncfusion.Presentation.RichText.Font) lineBreakProps).GetDefaultSize(), FontStyle.Regular);
        float height1 = this.MeasureString("0", font1, TextCapsType.None).Height;
        if ((double) maxHeight1 == 0.0)
          maxHeight1 = height1;
        if ((double) maxAscent == 0.0)
          maxAscent = this.FindAscent(font1);
        this.LayoutNewLine(shapeBounds, ref usedHeight, lineInfoCollection, ref textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight1, marginLeft, false, ref maxWidth, string.Empty);
        num2 = usedWidth;
        if (index1 + 1 == this.TextParts.Count)
        {
          Syncfusion.Presentation.RichText.Font font2 = this._charProps != null ? this._charProps : this.Font as Syncfusion.Presentation.RichText.Font;
          System.Drawing.Font font3 = this.Presentation.FontSettings.GetFont(font2.GetFontNameToRender(FontScriptType.English), font2.GetDefaultSize(), FontStyle.Regular);
          maxAscent = this.FindAscent(font3);
          float height2 = this.MeasureString("0", font3, TextCapsType.None).Height;
          this.LayoutNewLine(shapeBounds, ref usedHeight, lineInfoCollection, ref textInfoCollection, ref usedWidth, ref maxAscent, ref height2, marginLeft, false, ref maxWidth, string.Empty);
          num2 = usedWidth;
        }
      }
      else if (!string.IsNullOrEmpty(text1))
      {
        Syncfusion.Presentation.RichText.Font font = textPart.Font as Syncfusion.Presentation.RichText.Font;
        System.Drawing.Font updatedFont = this.GetUpdatedFont((IFont) font, textPart.ScriptType);
        TextCapsType capsType = font.GetDefaultCapsType();
        switch (capsType)
        {
          case TextCapsType.None:
          case TextCapsType.All:
            bool hasUnicode = false;
            string[] strArray = this.SplitTextByUnicode(text1, ref hasUnicode);
            textPart.HasUnicode = hasUnicode;
            for (int currPosition1 = 0; currPosition1 < strArray.Length; ++currPosition1)
            {
              string text2 = strArray[currPosition1];
              this.UpdatedFont(text2, ref updatedFont, font, textPart.ScriptType);
              SizeF size = this.MeasureString(text2, updatedFont, capsType);
              float characterSpacing = textPart.GetFontObject().GetDefaultCharacterSpacing();
              if ((double) characterSpacing != 0.0)
                size.Width += (float) text2.Length * characterSpacing;
              if (text2.Equals("\t"))
              {
                if (!flag)
                {
                  float num3 = num2 % num1;
                  size.Width = (double) num3 <= 0.0 ? num1 : num1 - num3;
                }
                else
                {
                  size.Width = (double) marginLeft == 0.0 ? num1 + marginLeft : marginLeft;
                  flag = false;
                }
              }
              float ascent = this.FindAscent(updatedFont);
              if ((double) size.Width <= (double) shapeBounds.Width - (double) usedWidth || !isWrap || this.IsNullOrWhiteSpace(text2) && !text2.Equals("\t"))
              {
                num2 += size.Width;
                Syncfusion.Presentation.Layouting.TextInfo textInfo = this.LayoutText(ref usedWidth, usedHeight, ref maxAscent, ref maxHeight1, (ITextPart) textPart, size, ascent, shapeBounds);
                textInfo.Position = currPosition1;
                textInfo.Length = text2.Length;
                textInfoCollection.Add(textInfo);
              }
              else
              {
                if (!text2.TrimStart(' ').TrimEnd(' ').Contains(" "))
                {
                  if (!text2.TrimStart(' ').TrimEnd(' ').Contains("-") && !text2.EndsWith(" "))
                  {
                    this.SplitByCharacter(shapeBounds, ref usedHeight, lineInfoCollection, ref textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight1, marginLeft, (ITextPart) textPart, text2, capsType, updatedFont, ref size, ascent, currPosition1, ref maxWidth);
                    continue;
                  }
                }
                int currPosition2 = 0;
                while (currPosition2 < text2.Length)
                {
                  int index2 = this.GetSpaceIndexBeforeText(text2, currPosition2);
                  int num4 = text2.IndexOf('-', currPosition2);
                  if (num4 >= 0 && (num4 < index2 || index2 == -1))
                    index2 = num4;
                  string str;
                  if (index2 >= 0)
                    str = text2.Substring(currPosition2, index2 + 1 - currPosition2);
                  else
                    str = text2.Substring(currPosition2, text2.Length - currPosition2).TrimEnd(' ');
                  string text3 = str;
                  size = this.MeasureString(text3, updatedFont, capsType);
                  if ((double) characterSpacing != 0.0)
                    size.Width += (float) text3.Length * characterSpacing;
                  if ((double) size.Width <= (double) shapeBounds.Width - (double) usedWidth || this.IsNullOrWhiteSpace(text3))
                  {
                    num2 += size.Width;
                    this.LayoutSplittedText(shapeBounds, usedHeight, textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight1, (ITextPart) textPart, text2, ref size, ascent, ref currPosition2, index2);
                  }
                  else
                  {
                    if (index2 > 0)
                    {
                      string text4 = text2.Substring(currPosition2, index2 + 1 - currPosition2).TrimEnd(' ');
                      if (text4 == string.Empty)
                      {
                        currPosition2 = index2 + 1;
                        continue;
                      }
                      if ((double) this.MeasureString(text4, updatedFont, capsType).Width <= (double) shapeBounds.Width - (double) usedWidth)
                      {
                        num2 += size.Width;
                        this.LayoutSplittedText(shapeBounds, usedHeight, textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight1, (ITextPart) textPart, text2, ref size, ascent, ref currPosition2, index2);
                        continue;
                      }
                    }
                    if ((double) size.Width <= (double) shapeBounds.Width - (double) marginLeft && (double) shapeBounds.Width - (double) usedWidth > 0.0)
                    {
                      this.LayoutNewLine(shapeBounds, ref usedHeight, lineInfoCollection, ref textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight1, marginLeft, false, ref maxWidth, text3);
                      this.LayoutSplittedText(shapeBounds, usedHeight, textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight1, (ITextPart) textPart, text2, ref size, ascent, ref currPosition2, index2);
                      num2 = usedWidth;
                    }
                    else
                    {
                      this.SplitByCharacter(shapeBounds, ref usedHeight, lineInfoCollection, ref textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight1, marginLeft, (ITextPart) textPart, text3, capsType, updatedFont, ref size, ascent, currPosition2, ref maxWidth);
                      currPosition2 += text3.Length;
                    }
                  }
                }
              }
            }
            continue;
          default:
            if (this.IsRTLText(text1))
            {
              capsType = TextCapsType.None;
              goto case TextCapsType.None;
            }
            goto case TextCapsType.None;
        }
      }
    }
    float maxHeight2 = this.UpdateEmptyParagraphHeight(maxHeight1, ref maxAscent);
    this.LayoutNewLine(shapeBounds, ref usedHeight, lineInfoCollection, ref textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight2, marginLeft, true, ref maxWidth, string.Empty);
    usedHeight += this.LayoutSpaceAfter(lineInfoCollection);
    for (int index = 0; index < lineInfoCollection.Count; ++index)
      this.ShiftWidgetsForRTLLayouting(lineInfoCollection[index], shapeBounds, usedHeight1);
  }

  private void ShiftWidgetsForRTLLayouting(
    LineInfo currentLine,
    RectangleF shapeBounds,
    float usedHeight)
  {
    bool flag = this.IsLineContainsRTL(currentLine);
    bool paraBidi = false;
    bool? defaultRtl = this.GetDefaultRTL();
    if (defaultRtl.HasValue && defaultRtl.Value)
      paraBidi = true;
    if (!flag && !paraBidi)
      return;
    List<CharacterRangeType> characterRangeTypes = new List<CharacterRangeType>();
    List<bool> splittedWidgetBidiValues = new List<bool>();
    for (int index = 0; index < currentLine.TextInfoCollection.Count; ++index)
    {
      if (currentLine.TextInfoCollection[index].TextPart is TextPart textPart)
      {
        if ((textPart.Font as Syncfusion.Presentation.RichText.Font).Bidi || paraBidi)
          splittedWidgetBidiValues.Add(true);
        else
          splittedWidgetBidiValues.Add(false);
        characterRangeTypes.Add(textPart.CharacterRange);
      }
      else
      {
        splittedWidgetBidiValues.Add(false);
        characterRangeTypes.Add(CharacterRangeType.LTR);
      }
    }
    int rtlStartIndex = -1;
    bool? nullable = new bool?();
    for (int index = 0; index < characterRangeTypes.Count; ++index)
    {
      if (index + 1 < splittedWidgetBidiValues.Count && splittedWidgetBidiValues[index] != splittedWidgetBidiValues[index + 1])
      {
        if (rtlStartIndex != -1)
        {
          this.UpdateCharacterRange(index, rtlStartIndex, splittedWidgetBidiValues, ref characterRangeTypes, currentLine);
          rtlStartIndex = -1;
        }
        nullable = new bool?();
      }
      else
      {
        if (characterRangeTypes[index] == CharacterRangeType.RTL || characterRangeTypes[index] == CharacterRangeType.LTR || characterRangeTypes[index] == CharacterRangeType.Number && rtlStartIndex != -1 || (!nullable.HasValue || !nullable.Value) && splittedWidgetBidiValues[index])
        {
          if (rtlStartIndex == -1 && characterRangeTypes[index] != CharacterRangeType.LTR)
          {
            rtlStartIndex = index;
          }
          else
          {
            if (rtlStartIndex == -1)
            {
              if (characterRangeTypes[index] == CharacterRangeType.LTR)
              {
                nullable = new bool?(true);
                continue;
              }
              if (characterRangeTypes[index] == CharacterRangeType.RTL)
              {
                nullable = new bool?(false);
                continue;
              }
              continue;
            }
            if (characterRangeTypes[index] == CharacterRangeType.LTR)
            {
              this.UpdateCharacterRange(index, rtlStartIndex, splittedWidgetBidiValues, ref characterRangeTypes, currentLine);
              rtlStartIndex = characterRangeTypes[index] == CharacterRangeType.RTL || characterRangeTypes[index] == CharacterRangeType.Number && rtlStartIndex != -1 ? index : -1;
            }
          }
        }
        if (characterRangeTypes[index] == CharacterRangeType.LTR)
          nullable = new bool?(true);
        else if (characterRangeTypes[index] == CharacterRangeType.RTL)
          nullable = new bool?(false);
      }
    }
    if (rtlStartIndex != -1 && rtlStartIndex < characterRangeTypes.Count - 1)
      this.UpdateCharacterRange(characterRangeTypes.Count - 1, rtlStartIndex, splittedWidgetBidiValues, ref characterRangeTypes, currentLine);
    if (characterRangeTypes.Count != currentLine.TextInfoCollection.Count)
      throw new Exception("Splitted Widget count mismatch while reordering layouted child widgets of a line");
    List<Syncfusion.Presentation.Layouting.TextInfo> reorderedWidgets = this.ReorderWidgets(characterRangeTypes, splittedWidgetBidiValues, paraBidi, currentLine);
    splittedWidgetBidiValues.Clear();
    characterRangeTypes.Clear();
    if (currentLine.TextInfoCollection.Count <= 0)
      return;
    this.UpdateBounds(reorderedWidgets, currentLine.TextInfoCollection, paraBidi, shapeBounds, usedHeight);
    reorderedWidgets.Reverse();
    currentLine.TextInfoCollection = reorderedWidgets;
  }

  private void UpdateBounds(
    List<Syncfusion.Presentation.Layouting.TextInfo> reorderedWidgets,
    List<Syncfusion.Presentation.Layouting.TextInfo> originalWidgets,
    bool paraBidi,
    RectangleF shapeBounds,
    float usedHeight)
  {
    float startPosition = this.GetStartPosition(originalWidgets, paraBidi, shapeBounds);
    if (paraBidi)
    {
      float x = startPosition;
      if (this.ListFormat != null)
      {
        float num1 = 0.0f;
        float num2 = 0.0f;
        double defaultMarginLeft = this.GetDefaultMarginLeft();
        if (defaultMarginLeft > 0.0)
          num2 = (float) defaultMarginLeft;
        float defaultIndent = (float) this.GetDefaultIndent();
        float usedWidth = num1 + num2;
        if (originalWidgets[0].TextPart == null)
        {
          switch (((Syncfusion.Presentation.RichText.ListFormat) this.ListFormat).GetDefaultListType())
          {
            case ListType.Bulleted:
            case ListType.Numbered:
            case ListType.Picture:
              float bulletIndent;
              if ((double) defaultIndent > 0.0)
              {
                bulletIndent = num2;
                usedWidth += defaultIndent;
              }
              else if ((double) num2 + (double) defaultIndent >= 0.0)
              {
                bulletIndent = num2 + defaultIndent;
              }
              else
              {
                bulletIndent = 0.0f;
                usedWidth = -defaultIndent;
              }
              SizeF size = new SizeF();
              Syncfusion.Presentation.Layouting.TextInfo textInfo = this.LayoutBullet(usedHeight, ref usedWidth, bulletIndent, shapeBounds, false, ref size);
              originalWidgets[0].Bounds = new RectangleF(textInfo.Bounds.X, originalWidgets[0].Bounds.Y, originalWidgets[0].Bounds.Width, originalWidgets[0].Bounds.Height);
              if ((double) x - (double) usedWidth > (double) textInfo.Bounds.X)
              {
                x -= x - usedWidth - textInfo.Bounds.X;
                break;
              }
              break;
          }
        }
        x -= usedWidth;
      }
      for (int index = reorderedWidgets.Count - 1; index >= 0; --index)
      {
        if (reorderedWidgets[index].TextPart != null)
        {
          x -= reorderedWidgets[index].Bounds.Width;
          reorderedWidgets[index].Bounds = new RectangleF(x, reorderedWidgets[index].Bounds.Y, reorderedWidgets[index].Bounds.Width, reorderedWidgets[index].Bounds.Height);
        }
      }
    }
    else
    {
      bool flag = true;
      for (int index = 0; index < reorderedWidgets.Count; ++index)
      {
        if (reorderedWidgets[index].TextPart != null)
        {
          if (flag && !Syncfusion.Presentation.Drawing.Helper.IsNullOrWhiteSpace(reorderedWidgets[index].TextPart.Text))
            flag = false;
          if (!flag)
          {
            reorderedWidgets[index].Bounds = new RectangleF(startPosition, reorderedWidgets[index].Bounds.Y, reorderedWidgets[index].Bounds.Width, reorderedWidgets[index].Bounds.Height);
            startPosition += reorderedWidgets[index].Bounds.Width;
          }
        }
      }
    }
  }

  private float GetStartPosition(
    List<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection,
    bool paraBidi,
    RectangleF shapeBounds)
  {
    float left = shapeBounds.Left;
    float right = shapeBounds.Right;
    if (paraBidi)
    {
      float num = 0.0f;
      if (textInfoCollection.Count > 0)
      {
        float x = textInfoCollection[textInfoCollection.Count - 1].Bounds.X;
        if (!Syncfusion.Presentation.Drawing.Helper.IsNullOrWhiteSpace(textInfoCollection[textInfoCollection.Count - 1].Text))
          x += textInfoCollection[textInfoCollection.Count - 1].Bounds.Width;
        num = right - x;
      }
      return right - num;
    }
    if (textInfoCollection.Count <= 0)
      return left;
    return textInfoCollection[0].TextPart == null && textInfoCollection.Count > 1 ? textInfoCollection[1].Bounds.X : textInfoCollection[0].Bounds.X;
  }

  private List<Syncfusion.Presentation.Layouting.TextInfo> ReorderWidgets(
    List<CharacterRangeType> characterRangeTypes,
    List<bool> splittedWidgetBidiValues,
    bool paraBidi,
    LineInfo currentLine)
  {
    int index1 = 0;
    int num1 = -1;
    int num2 = 0;
    int num3 = 0;
    List<Syncfusion.Presentation.Layouting.TextInfo> textInfoList = new List<Syncfusion.Presentation.Layouting.TextInfo>();
    CharacterRangeType characterRangeType = CharacterRangeType.LTR;
    bool flag1 = false;
    for (int index2 = 0; index2 < currentLine.TextInfoCollection.Count; ++index2)
    {
      Syncfusion.Presentation.Layouting.TextInfo textInfo = currentLine.TextInfoCollection[index2];
      textInfo.CharacterRange = characterRangeTypes[index2];
      bool flag2 = (textInfo.CharacterRange & CharacterRangeType.RTL) == CharacterRangeType.RTL || textInfo.CharacterRange == CharacterRangeType.Number;
      bool flag3 = splittedWidgetBidiValues[index2];
      if (characterRangeTypes[index2] == CharacterRangeType.Tab)
      {
        if (paraBidi)
        {
          index1 = 0;
          num1 = -1;
          num2 = 0;
          characterRangeType = CharacterRangeType.LTR;
          flag1 = false;
          textInfoList.Insert(index1, textInfo);
          continue;
        }
        if (flag3)
          flag3 = false;
      }
      if (index2 > 0 && flag1 != flag3)
      {
        if (paraBidi)
        {
          index1 = 0;
          num1 = -1;
          num2 = 0;
        }
        else
          num1 = textInfoList.Count - 1;
        num3 = 0;
      }
      if (!flag3 && !flag2)
      {
        if (paraBidi)
        {
          if (num2 > 0 && flag1 == flag3)
            index1 += num2;
          textInfoList.Insert(index1, textInfo);
          ++index1;
        }
        else
        {
          textInfoList.Add(textInfo);
          index1 = index2 + 1;
        }
        num2 = 0;
        num1 = paraBidi ? index1 - 1 : textInfoList.Count - 1;
      }
      else if (flag2 || flag3 && textInfo.CharacterRange == CharacterRangeType.WordSplit && (characterRangeType == CharacterRangeType.RTL || this.IsInsertWordSplitToLeft(characterRangeTypes, splittedWidgetBidiValues, index2)))
      {
        ++num2;
        index1 = num1 + 1;
        if (textInfo.CharacterRange == CharacterRangeType.Number)
        {
          if (characterRangeType == CharacterRangeType.Number)
            index1 += num3;
          ++num3;
        }
        textInfoList.Insert(index1, textInfo);
      }
      else
      {
        textInfoList.Insert(index1, textInfo);
        ++index1;
        num2 = 0;
      }
      if (textInfo.CharacterRange != CharacterRangeType.Number)
        num3 = 0;
      if (textInfo.CharacterRange != CharacterRangeType.WordSplit)
        characterRangeType = textInfo.CharacterRange;
      flag1 = flag3;
    }
    return textInfoList;
  }

  private bool IsInsertWordSplitToLeft(
    List<CharacterRangeType> characterRangeTypes,
    List<bool> splittedWidgetBidiValues,
    int widgetIndex)
  {
    for (int index = widgetIndex + 1; index < characterRangeTypes.Count && (characterRangeTypes[index] & CharacterRangeType.RTL) != CharacterRangeType.RTL; ++index)
    {
      if (characterRangeTypes[index] == CharacterRangeType.LTR)
        return !splittedWidgetBidiValues[index];
    }
    return true;
  }

  private void UpdateCharacterRange(
    int i,
    int rtlStartIndex,
    List<bool> splittedWidgetBidiValues,
    ref List<CharacterRangeType> characterRangeTypes,
    LineInfo currentLine)
  {
    int num = i;
    if (!splittedWidgetBidiValues[i])
    {
      if (characterRangeTypes[i] == CharacterRangeType.LTR)
        --num;
      for (int index = num; index >= rtlStartIndex; --index)
      {
        if (characterRangeTypes[index] != CharacterRangeType.WordSplit)
        {
          num = index;
          break;
        }
      }
    }
    for (int index1 = rtlStartIndex; index1 <= num; ++index1)
    {
      if (characterRangeTypes[index1] == CharacterRangeType.WordSplit)
      {
        characterRangeTypes[index1] = CharacterRangeType.RTL | CharacterRangeType.WordSplit;
        int index2 = index1 - 1;
        int index3 = index1 + 1;
        if (index2 >= 0 && index3 < characterRangeTypes.Count && characterRangeTypes[index2] == CharacterRangeType.RTL && (characterRangeTypes[index3] == CharacterRangeType.RTL || characterRangeTypes[index3] == CharacterRangeType.Number) && currentLine.TextInfoCollection[index1].TextPart != null)
        {
          TextPart textPart = currentLine.TextInfoCollection[index1].TextPart as TextPart;
          if ((textPart.Font as Syncfusion.Presentation.RichText.Font).GetEastAsianFontName() == "Times New Roman")
          {
            char[] charArray = textPart.Text.ToCharArray();
            Array.Reverse((Array) charArray);
            textPart.Text = new string(charArray);
          }
        }
      }
    }
  }

  private bool IsLineContainsRTL(LineInfo currentLine)
  {
    List<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection = currentLine.TextInfoCollection;
    for (int index = 0; index < textInfoCollection.Count; ++index)
    {
      if (textInfoCollection[index].TextPart is TextPart textPart && this.IsRTL(textPart.Text))
        return true;
    }
    return false;
  }

  internal void CalculateBulletSize(
    IListFormat ListFormat,
    ref float usedWidth,
    ref float usedHeight,
    RectangleF shapeBounds,
    ref SizeF bulletSize,
    ref float marginLeft,
    ref List<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection,
    ref float indentWidth)
  {
    double defaultMarginLeft = this.GetDefaultMarginLeft();
    if (defaultMarginLeft > 0.0)
      marginLeft = (float) defaultMarginLeft;
    indentWidth = (float) this.GetDefaultIndent();
    usedWidth += marginLeft;
    if (ListFormat == null || this.Text == null || this.Text.Equals(string.Empty))
      return;
    switch (((Syncfusion.Presentation.RichText.ListFormat) ListFormat).GetDefaultListType())
    {
      case ListType.NotDefined:
      case ListType.None:
        if ((double) marginLeft + (double) indentWidth >= 0.0)
        {
          usedWidth += indentWidth;
          break;
        }
        usedWidth = 0.0f;
        break;
      case ListType.Bulleted:
      case ListType.Numbered:
      case ListType.Picture:
        float bulletIndent;
        if ((double) indentWidth > 0.0)
        {
          bulletIndent = marginLeft;
          usedWidth += indentWidth;
        }
        else if ((double) marginLeft + (double) indentWidth >= 0.0)
        {
          bulletIndent = marginLeft + indentWidth;
        }
        else
        {
          bulletIndent = 0.0f;
          usedWidth = -indentWidth;
        }
        Syncfusion.Presentation.Layouting.TextInfo textInfo = this.LayoutBullet(usedHeight, ref usedWidth, bulletIndent, shapeBounds, true, ref bulletSize);
        textInfoCollection.Add(textInfo);
        break;
    }
  }

  internal void UpdatedFont(string text, ref System.Drawing.Font font, Syncfusion.Presentation.RichText.Font syncFont, FontScriptType scriptType)
  {
    if (Syncfusion.Presentation.Drawing.Helper.IsSymbol(text) && syncFont.GetSymbolFontName() != null)
      font = this.Presentation.FontSettings.GetFont(syncFont.GetSymbolFontName(), font.Size, font.Style);
    else if (Syncfusion.Presentation.Drawing.Helper.IsGeometricShapesSymbol(text))
      font = this.Presentation.FontSettings.GetFont("Segoe UI Symbol", font.Size, font.Style);
    else
      font = this.GetUpdatedFont((IFont) syncFont, scriptType);
  }

  private void CheckPreviousElement(
    List<LineInfo> lineInfoCollection,
    ref List<Syncfusion.Presentation.Layouting.TextInfo> currTextInfoCollection,
    string text,
    ref float usedWidth,
    float usedHeight,
    RectangleF shapeBounds)
  {
    if (lineInfoCollection.Count <= 0 || !(text != string.Empty) || text.StartsWith(" ") || text.Equals("\t"))
      return;
    LineInfo lineInfo = lineInfoCollection[lineInfoCollection.Count - 1];
    List<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection = lineInfo.TextInfoCollection;
    string text1 = lineInfo.Text;
    if (text1 == string.Empty || !text1.Contains(" ") || text1.EndsWith(" "))
      return;
    int num1 = 0;
    float num2 = 0.0f;
    for (int index = textInfoCollection.Count - 1; index >= 0; --index)
    {
      Syncfusion.Presentation.Layouting.TextInfo textInfo1 = textInfoCollection[index];
      if (textInfo1.TextPart != null)
      {
        string text2 = textInfo1.Text;
        if (text2 == "\t")
        {
          ++num1;
          break;
        }
        if (text2 != string.Empty && !text2.Contains(" ") && !text2.EndsWith("-"))
        {
          ++num1;
        }
        else
        {
          if (text2.TrimEnd(' ').Contains(" ") && !text2.EndsWith(" "))
          {
            string str = text2.TrimEnd(' ');
            int num3 = str.LastIndexOf(' ') + 1;
            Syncfusion.Presentation.Layouting.TextInfo textInfo2 = new Syncfusion.Presentation.Layouting.TextInfo(textInfo1.TextPart);
            textInfo2.Ascent = textInfo1.Ascent;
            textInfo2.Position = num3;
            textInfo2.Length = str.Length - num3;
            textInfo1.Length = num3;
            SizeF sizeF1 = this.MeasureString(textInfo1.Text, this.GetUpdatedFont(textInfo1.TextPart.Font, (textInfo1.TextPart as TextPart).ScriptType), ((Syncfusion.Presentation.RichText.Font) textInfo1.TextPart.Font).GetDefaultCapsType());
            textInfo1.Width = sizeF1.Width;
            textInfo2.X = shapeBounds.Left + usedWidth;
            textInfo2.Y = shapeBounds.Top + usedHeight;
            SizeF sizeF2 = this.MeasureString(textInfo2.Text, this.GetUpdatedFont(textInfo2.TextPart.Font, (textInfo2.TextPart as TextPart).ScriptType), ((Syncfusion.Presentation.RichText.Font) textInfo2.TextPart.Font).GetDefaultCapsType());
            usedWidth += num2 = textInfo2.Width = sizeF2.Width;
            textInfo2.Height = sizeF2.Height;
            currTextInfoCollection.Add(textInfo2);
            break;
          }
          if (text2.Equals(string.Empty))
            ++num1;
          else
            break;
        }
      }
      else
        break;
    }
    int index1 = textInfoCollection.Count - num1;
    while (textInfoCollection.Count != index1)
    {
      Syncfusion.Presentation.Layouting.TextInfo textInfo = textInfoCollection[index1];
      textInfoCollection.RemoveAt(index1);
      currTextInfoCollection.Add(textInfo);
      textInfo.X = shapeBounds.Left + usedWidth;
      textInfo.Y = shapeBounds.Top + usedHeight + this.AlignSuperOrSubScript(textInfo.TextPart.Font, (textInfo.TextPart as TextPart).ScriptType);
      usedWidth += textInfo.Width;
      num2 += textInfo.Width;
    }
    if ((double) num2 == 0.0)
      return;
    switch (this.GetDefaultAlignmentType())
    {
      case HorizontalAlignmentType.Center:
        float num4 = num2 + this.RemoveWhiteSpaces(textInfoCollection);
        this.UpdateXPosition((IEnumerable<Syncfusion.Presentation.Layouting.TextInfo>) textInfoCollection, num4 / 2f);
        break;
      case HorizontalAlignmentType.Right:
        float x = num2 + this.RemoveWhiteSpaces(textInfoCollection);
        this.UpdateXPosition((IEnumerable<Syncfusion.Presentation.Layouting.TextInfo>) textInfoCollection, x);
        break;
      case HorizontalAlignmentType.Justify:
        string empty = string.Empty;
        float num5 = -this.RemoveWhiteSpaces(textInfoCollection);
        for (int index2 = 0; index2 < textInfoCollection.Count; ++index2)
        {
          Syncfusion.Presentation.Layouting.TextInfo textInfo = textInfoCollection[index2];
          if (textInfo.TextPart != null && textInfo.Text != null)
            empty += textInfo.Text;
          if (textInfo.TextPart == null && index2 == 0 && textInfoCollection.Count > 1)
            num5 += textInfoCollection[index2 + 1].X - shapeBounds.X;
          else if (index2 == 0)
            num5 += textInfoCollection[index2].X - shapeBounds.X + textInfo.Width;
          else
            num5 += textInfo.Width;
        }
        int num6 = empty.TrimEnd(' ').Length - empty.Replace(" ", string.Empty).Length;
        for (int index3 = 0; index3 < textInfoCollection.Count; ++index3)
        {
          if (textInfoCollection[index3].TextPart != null)
            Paragraph.UpdateLineJustifyPosition(textInfoCollection, (shapeBounds.Width - num5) / (float) num6, index3);
        }
        break;
    }
  }

  private void UpdateXPosition(IEnumerable<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection, float x)
  {
    foreach (Syncfusion.Presentation.Layouting.TextInfo textInfo in textInfoCollection)
      textInfo.X += x;
  }

  internal System.Drawing.Font GetUpdatedFont(IFont font, FontScriptType scriptType)
  {
    FontStyle fontStyle = this.GetFontStyle(font, scriptType);
    string fontName = ((Syncfusion.Presentation.RichText.Font) font).GetFontNameToRender(scriptType);
    if (fontName == "Courier")
      fontName = "Courier New";
    if (fontName == "Calibri Bold")
      fontName = "Calibri";
    return this.Presentation.FontSettings.GetFont(fontName, this.UpdateFontSize(font), fontStyle);
  }

  internal float UpdateFontSize(IFont font)
  {
    float defaultSize = ((Syncfusion.Presentation.RichText.Font) font).GetDefaultSize();
    float num = (double) defaultSize == 0.0 ? 0.5f : defaultSize;
    if (font.Superscript || font.Subscript)
      num /= 1.5f;
    return num;
  }

  private float AlignSuperOrSubScript(IFont font, FontScriptType scriptType)
  {
    float num = 0.0f;
    if (font.Superscript)
    {
      System.Drawing.Font font1 = this.GetFont(font, scriptType);
      float ascent = this.FindAscent(font1);
      int height = font1.Height;
      num = (float) (((double) num + (double) ascent + ((double) height / 1.7000000476837158 - (double) height)) * ((double) ((Syncfusion.Presentation.RichText.Font) font).GetBaseline() / -30000.0));
    }
    else if (font.Subscript)
    {
      System.Drawing.Font font2 = this.GetFont(font, scriptType);
      float ascent = this.FindAscent(font2);
      int height = font2.Height;
      num = (float) (((double) num + (double) ascent + ((double) height / 1.7000000476837158 - (double) height)) * ((double) ((Syncfusion.Presentation.RichText.Font) font).GetBaseline() / -25000.0));
    }
    return num;
  }

  private void SplitByCharacter(
    RectangleF shapeBounds,
    ref float usedHeight,
    List<LineInfo> lineInfoCollection,
    ref List<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection,
    ref float usedWidth,
    ref float maxAscent,
    ref float maxHeight,
    float indentLevelWidth,
    ITextPart textPart,
    string text,
    TextCapsType capsType,
    System.Drawing.Font font,
    ref SizeF size,
    float ascent,
    int currPosition,
    ref float maxWidth)
  {
    int textIndexAfterSpace = this.GetTextIndexAfterSpace(text.TrimEnd(' '), 0);
    int length = 1;
    float num1 = 0.0f;
    string text1 = string.Empty;
    if ((double) usedWidth > 0.0 && textInfoCollection.Count != 0 && (textInfoCollection.Count > 1 || textInfoCollection[0].TextPart != null) && (double) size.Width <= (double) shapeBounds.Width - (double) indentLevelWidth && (this.HasWordSplitCharacter(textInfoCollection) || text.StartsWith(" ")))
      this.LayoutNewLine(shapeBounds, ref usedHeight, lineInfoCollection, ref textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight, indentLevelWidth, false, ref maxWidth, text);
    while (textIndexAfterSpace + length <= text.Length)
    {
      text1 = text.Substring(textIndexAfterSpace, length);
      size = this.MeasureString(text1, font, capsType);
      if (text.Equals("\t"))
      {
        float defaultTabsize = (float) this.GetDefaultTabsize();
        float num2 = (double) defaultTabsize == -1.0 ? 72f : (float) Syncfusion.Presentation.Drawing.Helper.EmuToPoint((double) defaultTabsize);
        size.Width = (double) usedWidth <= 0.0 ? num2 : num2 - usedWidth % num2;
      }
      if ((double) size.Width > (double) shapeBounds.Width - (double) usedWidth)
      {
        if (text1.Length > 1)
          size.Width = num1;
        Syncfusion.Presentation.Layouting.TextInfo textInfo = this.LayoutText(ref usedWidth, usedHeight, ref maxAscent, ref maxHeight, textPart, size, ascent, shapeBounds);
        textInfo.Position = textIndexAfterSpace + currPosition;
        if ((double) shapeBounds.Width - (double) usedWidth < 0.0)
        {
          textInfo.Length = length;
          textInfo.Width = this.MeasureString(text1, font, capsType).Width;
        }
        else
          textInfo.Length = (double) usedWidth != 0.0 ? length - 1 : length;
        textInfoCollection.Add(textInfo);
        if (text1.Length == 1 && textInfoCollection.Count == 1)
          textIndexAfterSpace += length;
        else
          textIndexAfterSpace += length - 1;
        length = 1;
        this.LayoutNewLine(shapeBounds, ref usedHeight, lineInfoCollection, ref textInfoCollection, ref usedWidth, ref maxAscent, ref maxHeight, indentLevelWidth, false, ref maxWidth, text1[text1.Length - 1].ToString((IFormatProvider) CultureInfo.InvariantCulture));
        text1 = string.Empty;
      }
      else
      {
        num1 = size.Width;
        ++length;
      }
    }
    if (!(text1 != string.Empty))
      return;
    Syncfusion.Presentation.Layouting.TextInfo textInfo1 = this.LayoutText(ref usedWidth, usedHeight, ref maxAscent, ref maxHeight, textPart, size, ascent, shapeBounds);
    textInfo1.Position = textIndexAfterSpace + currPosition;
    textInfo1.Length = length - 1;
    textInfoCollection.Add(textInfo1);
  }

  private void LayoutSplittedText(
    RectangleF shapeBounds,
    float usedHeight,
    List<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection,
    ref float usedWidth,
    ref float maxAscent,
    ref float maxHeight,
    ITextPart textPart,
    string text,
    ref SizeF size,
    float ascent,
    ref int currPosition,
    int index)
  {
    Syncfusion.Presentation.Layouting.TextInfo textInfo = this.LayoutText(ref usedWidth, usedHeight, ref maxAscent, ref maxHeight, textPart, size, ascent, shapeBounds);
    textInfo.Position = currPosition;
    textInfoCollection.Add(textInfo);
    if (index > -1)
    {
      textInfo.Length = index + 1 - currPosition;
      currPosition = index + 1;
    }
    else
    {
      textInfo.Length = text.Length - currPosition;
      currPosition = text.Length;
    }
  }

  private float UpdateEmptyParagraphHeight(float maxHeight, ref float maxAscent)
  {
    if ((this.Text == null || this.Text.Equals(string.Empty)) && !this.ContainsLineBreak())
    {
      IFont defaultParagraphFont = (IFont) this.GetDefaultParagraphFont();
      System.Drawing.Font font = this.Presentation.FontSettings.GetFont(((Syncfusion.Presentation.RichText.Font) defaultParagraphFont).GetFontNameToRender(FontScriptType.English), ((Syncfusion.Presentation.RichText.Font) defaultParagraphFont).GetDefaultSize(), this.GetFontStyle(defaultParagraphFont, FontScriptType.English));
      maxHeight = this.MeasureString("0", font, TextCapsType.None).Height;
      maxAscent = this.FindAscent(font);
    }
    return maxHeight;
  }

  private bool IsFirstParagraph() => this.Index == 0;

  private bool IsLastParagraph() => this.Index == this._paragraphCollection.Count - 1;

  internal bool IsNullOrWhiteSpace(string text)
  {
    if (text == null)
      return true;
    foreach (char c in text)
    {
      if (!char.IsWhiteSpace(c))
        return false;
    }
    return true;
  }

  internal SizeF MeasureString(string text, System.Drawing.Font font, TextCapsType capsType)
  {
    Graphics graphics = !this.BaseSlide.Presentation.IsInternalGraphics ? this.BaseSlide.Presentation.Graphics : this.BaseSlide.Presentation.InternalGraphics;
    SizeF sizeF = new SizeF();
    switch (capsType)
    {
      case TextCapsType.None:
        sizeF = this.BaseSlide.Presentation.Renderer == null ? graphics.MeasureString(text, font, new PointF(0.0f, 0.0f), this.StringFormt) : this.BaseSlide.Presentation.Renderer.MeasureString(text, font, this.StringFormt, graphics);
        break;
      case TextCapsType.Small:
        sizeF = this.MeasureSmallCapString(text, font);
        break;
      case TextCapsType.All:
        sizeF = this.BaseSlide.Presentation.Renderer == null ? graphics.MeasureString(text.ToUpper(), font, new PointF(0.0f, 0.0f), this.StringFormt) : this.BaseSlide.Presentation.Renderer.MeasureString(text.ToUpper(), font, this.StringFormt, graphics);
        break;
    }
    sizeF.Height = font.SizeInPoints * 1.2f;
    return sizeF;
  }

  private SizeF MeasureSmallCapString(string text, System.Drawing.Font font)
  {
    System.Drawing.Font font1 = this.Presentation.FontSettings.GetFont(font.Name, (double) font.Size * 0.8 > 3.0 ? font.Size * 0.8f : 2f, font.Style);
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    foreach (char c in text)
    {
      if (char.IsUpper(c) || !char.IsLetter(c) && !c.Equals(' '))
        empty1 += c.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      else
        empty2 += c.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }
    SizeF sizeF1 = this.MeasureString(empty1, font, TextCapsType.None);
    SizeF sizeF2 = this.MeasureString(empty2.ToUpper(), font1, TextCapsType.None);
    if ((double) sizeF1.Height > (double) sizeF2.Height)
      sizeF2.Height = sizeF1.Height;
    sizeF2.Width += sizeF1.Width;
    return sizeF2;
  }

  internal FontStyle GetFontStyle(IFont font, FontScriptType scriptType)
  {
    FontStyle fontStyle = FontStyle.Regular;
    string fontNameToRender = ((Syncfusion.Presentation.RichText.Font) font).GetFontNameToRender(scriptType);
    if (((Syncfusion.Presentation.RichText.Font) font).GetDefaultBold())
    {
      switch (fontNameToRender)
      {
        case "Lucida Sans Unicode":
        case "Lucida Console":
        case "Arial Unicode MS":
          break;
        default:
          fontStyle |= FontStyle.Bold;
          break;
      }
    }
    if (((Syncfusion.Presentation.RichText.Font) font).GetDefaultItalic())
      fontStyle |= FontStyle.Italic;
    if (font.StrikeType != TextStrikethroughType.None)
      fontStyle |= FontStyle.Strikeout;
    if (((Syncfusion.Presentation.RichText.Font) font).GetDefaultUnderline() != TextUnderlineType.None)
      fontStyle |= FontStyle.Underline;
    return fontStyle;
  }

  private void LayoutNewLine(
    RectangleF shapeBounds,
    ref float usedHeight,
    List<LineInfo> lineInfoCollection,
    ref List<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection,
    ref float usedWidth,
    ref float maxAscent,
    ref float maxHeight,
    float marginLeft,
    bool isLastLine,
    ref float maxWidth,
    string text)
  {
    LineInfo lineInfo = new LineInfo();
    lineInfo.TextInfoCollection = textInfoCollection;
    lineInfoCollection.Add(lineInfo);
    float y = 0.0f;
    if (lineInfoCollection.Count == 1)
      y = this.LayoutSpaceBefore(lineInfo);
    usedHeight += y;
    usedWidth = Paragraph.RemoveSpace(lineInfoCollection, textInfoCollection, usedWidth);
    lineInfo.Height = maxHeight;
    lineInfo.MaximumAscent = maxAscent;
    this.LayoutXYPosition(usedWidth, maxAscent, shapeBounds.Width, textInfoCollection, isLastLine, ref maxHeight, y, ref maxWidth, ref usedHeight);
    textInfoCollection = new List<Syncfusion.Presentation.Layouting.TextInfo>();
    usedHeight += maxHeight;
    usedWidth = marginLeft;
    maxHeight = 0.0f;
    maxAscent = 0.0f;
    this.CheckPreviousElement(lineInfoCollection, ref textInfoCollection, text, ref usedWidth, usedHeight, shapeBounds);
  }

  private static float RemoveSpace(
    List<LineInfo> lineInfoCollection,
    List<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection,
    float usedWidth)
  {
    if (lineInfoCollection.Count > 1 && textInfoCollection.Count > 0 && textInfoCollection[0].Text.StartsWith(" "))
    {
      float num1 = 0.0f;
      int num2 = 0;
      foreach (Syncfusion.Presentation.Layouting.TextInfo textInfo in textInfoCollection)
      {
        if (textInfo.Text.Replace(" ", string.Empty).Length == 0)
        {
          num1 += textInfo.Bounds.Width;
          ++num2;
        }
        else
          break;
      }
      for (; num2 > 0; --num2)
        textInfoCollection.RemoveAt(0);
      foreach (Syncfusion.Presentation.Layouting.TextInfo textInfo in textInfoCollection)
        textInfo.X -= num1;
      usedWidth -= num1;
    }
    return usedWidth;
  }

  internal float LayoutSpaceBefore(LineInfo lineInfo)
  {
    if (this.IsFirstParagraph())
      return 0.0f;
    double defaultSpaceBefore = this.GetDefaultSpaceBefore();
    if (this.CurrentSpcBefType == SizeType.Points)
      return (float) defaultSpaceBefore;
    float defaultSize = ((Syncfusion.Presentation.RichText.Font) this.GetMaximumFont((IEnumerable<Syncfusion.Presentation.Layouting.TextInfo>) lineInfo.TextInfoCollection)).GetDefaultSize();
    return (float) (defaultSpaceBefore * 1.2) * defaultSize;
  }

  internal float LayoutSpaceAfter(List<LineInfo> lineInfoCollection)
  {
    if (this.IsLastParagraph())
      return 0.0f;
    double defaultSpaceAfter = this.GetDefaultSpaceAfter();
    if (this.CurrentSpcAftType == SizeType.Points)
      return (float) defaultSpaceAfter;
    float defaultSize = ((Syncfusion.Presentation.RichText.Font) this.GetMaximumFont((IEnumerable<Syncfusion.Presentation.Layouting.TextInfo>) lineInfoCollection[lineInfoCollection.Count - 1].TextInfoCollection)).GetDefaultSize();
    return (float) (defaultSpaceAfter * 1.2) * defaultSize;
  }

  private IFont GetMaximumFont(IEnumerable<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection)
  {
    IFont font1 = (IFont) null;
    float num = 0.0f;
    foreach (Syncfusion.Presentation.Layouting.TextInfo textInfo in textInfoCollection)
    {
      if (textInfo.TextPart != null)
      {
        IFont font2 = textInfo.TextPart.Font;
        float defaultSize = ((Syncfusion.Presentation.RichText.Font) font2).GetDefaultSize();
        if ((double) defaultSize > (double) num)
        {
          font1 = font2;
          num = defaultSize;
        }
      }
    }
    return font1 != null && (double) ((Syncfusion.Presentation.RichText.Font) font1).GetDefaultSize() != 0.0 ? font1 : (IFont) this.GetDefaultParagraphFont();
  }

  internal Syncfusion.Presentation.Layouting.TextInfo LayoutBullet(
    float usedHeight,
    ref float usedWidth,
    float bulletIndent,
    RectangleF shapeBounds,
    bool isLeft,
    ref SizeF size)
  {
    IListFormat listFormat = this.ListFormat;
    Syncfusion.Presentation.Layouting.TextInfo textInfo = new Syncfusion.Presentation.Layouting.TextInfo((ITextPart) null);
    if (((Syncfusion.Presentation.RichText.ListFormat) listFormat).GetDefaultListType() == ListType.Picture)
    {
      Syncfusion.Drawing.Image bulletImage = ((Syncfusion.Presentation.RichText.ListFormat) listFormat).GetBulletImage();
      if (bulletImage != null)
      {
        FontStyle fontStyle = FontStyle.Regular;
        string fontName = "Segoe UI Symbol";
        float defaultBulletSize = ((Syncfusion.Presentation.RichText.ListFormat) listFormat).GetDefaultBulletSize();
        System.Drawing.Font font = this.Presentation.FontSettings.GetFont(fontName, defaultBulletSize, fontStyle);
        SizeF sizeF = this.MeasurePictureBullet(defaultBulletSize, bulletImage);
        size = sizeF;
        if ((double) sizeF.Height > (double) defaultBulletSize)
          sizeF.Height = (double) sizeF.Height != (double) sizeF.Width ? defaultBulletSize : (sizeF.Width = defaultBulletSize);
        textInfo.Bounds = (double) defaultBulletSize < 30.0 ? ((double) defaultBulletSize >= 20.0 ? (!isLeft ? new RectangleF(shapeBounds.Right - bulletIndent - sizeF.Width, (float) ((double) shapeBounds.Top + (double) usedHeight + 5.0), sizeF.Width, sizeF.Height) : new RectangleF(shapeBounds.Left + bulletIndent, (float) ((double) shapeBounds.Top + (double) usedHeight + 5.0), sizeF.Width, sizeF.Height)) : (!isLeft ? new RectangleF(shapeBounds.Right - bulletIndent - sizeF.Width, shapeBounds.Top + usedHeight, sizeF.Width, sizeF.Height) : new RectangleF(shapeBounds.Left + bulletIndent, shapeBounds.Top + usedHeight, sizeF.Width, sizeF.Height))) : (!isLeft ? new RectangleF(shapeBounds.Right - bulletIndent - sizeF.Width, (float) ((double) shapeBounds.Top + (double) usedHeight + (double) defaultBulletSize / 2.0 - 12.5), sizeF.Width, sizeF.Height) : new RectangleF(shapeBounds.Left + bulletIndent, (float) ((double) shapeBounds.Top + (double) usedHeight + (double) defaultBulletSize / 2.0 - 12.5), sizeF.Width, sizeF.Height));
        textInfo.Ascent = this.FindAscent(font);
        if ((double) usedWidth == (double) bulletIndent)
          usedWidth += sizeF.Width;
        else if ((double) bulletIndent + (double) sizeF.Width > (double) usedWidth)
          usedWidth = bulletIndent + sizeF.Width;
        else if ((double) textInfo.Bounds.Width == 0.0)
          usedWidth = (float) this.GetDefaultMarginLeft() + (float) this.GetDefaultIndent();
      }
    }
    else
    {
      string defaultBulletCharacter = ((Syncfusion.Presentation.RichText.ListFormat) listFormat).GetDefaultBulletCharacter();
      FontStyle fontStyle = FontStyle.Regular;
      if (((Syncfusion.Presentation.RichText.ListFormat) listFormat).GetDefaultListType() == ListType.Numbered)
      {
        if (this._textPartCollection.Count != 0)
          fontStyle = this.GetFontStyle(this._textPartCollection[0].Font, FontScriptType.English);
        if (Syncfusion.Presentation.Drawing.Helper.HasFlag(fontStyle, FontStyle.Underline))
          fontStyle ^= FontStyle.Underline;
      }
      System.Drawing.Font font = this.Presentation.FontSettings.GetFont(!Syncfusion.Presentation.Drawing.Helper.IsGeometricShapesSymbol(defaultBulletCharacter) ? ((Syncfusion.Presentation.RichText.ListFormat) listFormat).GetDefaultBulletFontName() : "Segoe UI Symbol", ((Syncfusion.Presentation.RichText.ListFormat) listFormat).GetDefaultBulletSize(), fontStyle);
      SizeF sizeF = this.MeasureString(defaultBulletCharacter, font, TextCapsType.None);
      size = sizeF;
      textInfo.Bounds = !isLeft ? new RectangleF(shapeBounds.Right - bulletIndent - sizeF.Width, shapeBounds.Top + usedHeight, sizeF.Width, sizeF.Height) : new RectangleF(shapeBounds.Left + bulletIndent, shapeBounds.Top + usedHeight, sizeF.Width, sizeF.Height);
      textInfo.Ascent = this.FindAscent(font);
      if ((double) usedWidth == (double) bulletIndent)
        usedWidth += sizeF.Width;
      else if ((double) bulletIndent + (double) sizeF.Width > (double) usedWidth)
        usedWidth = bulletIndent + sizeF.Width;
      else if ((double) textInfo.Bounds.Width == 0.0)
        usedWidth = (float) this.GetDefaultMarginLeft() + (float) this.GetDefaultIndent();
    }
    return textInfo;
  }

  internal float FindAscent(System.Drawing.Font font)
  {
    int emHeight = font.FontFamily.GetEmHeight(font.Style);
    int cellAscent = font.FontFamily.GetCellAscent(font.Style);
    return font.SizeInPoints * (float) cellAscent / (float) emHeight;
  }

  internal float FindLineSpacing(System.Drawing.Font font)
  {
    int emHeight = font.FontFamily.GetEmHeight(font.Style);
    int lineSpacing = font.FontFamily.GetLineSpacing(font.Style);
    return font.SizeInPoints * (float) lineSpacing / (float) emHeight;
  }

  internal float FindDescent(System.Drawing.Font font)
  {
    int emHeight = font.FontFamily.GetEmHeight(font.Style);
    int cellDescent = font.FontFamily.GetCellDescent(font.Style);
    return font.SizeInPoints * (float) cellDescent / (float) emHeight;
  }

  internal float GetAscentSpace(System.Drawing.Font font)
  {
    return (float) ((double) (font.GetHeight() * this.FindAscent(font) / this.FindLineSpacing(font)) * 72.0 / 96.0);
  }

  internal float GetDescentSpace(System.Drawing.Font font)
  {
    return (float) ((double) (font.GetHeight() * this.FindDescent(font) / this.FindLineSpacing(font)) / 2.0 * 72.0 / 96.0);
  }

  private Syncfusion.Presentation.Layouting.TextInfo LayoutText(
    ref float usedWidth,
    float usedHeight,
    ref float maxAscent,
    ref float maxHeight,
    ITextPart textPart,
    SizeF size,
    float ascent,
    RectangleF shapeBounds)
  {
    float x = shapeBounds.Left + usedWidth;
    float y = shapeBounds.Top + usedHeight + this.AlignSuperOrSubScript(textPart.Font, (textPart as TextPart).ScriptType);
    float width = size.Width;
    float height = size.Height;
    usedWidth += size.Width;
    if ((double) size.Height > (double) maxHeight)
      maxHeight = size.Height;
    if ((double) ascent > (double) maxAscent)
      maxAscent = ascent;
    return new Syncfusion.Presentation.Layouting.TextInfo(textPart)
    {
      Bounds = new RectangleF(x, y, width, height),
      Ascent = ascent
    };
  }

  private void LayoutXYPosition(
    float usedWidth,
    float maxAscent,
    float shapeWidth,
    List<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection,
    bool isLastLine,
    ref float maxHeight,
    float y,
    ref float maxWidth,
    ref float usedHeight)
  {
    float x1 = 0.0f;
    y += this.GetLineSpace(ref maxHeight);
    switch (this.GetDefaultAlignmentType())
    {
      case HorizontalAlignmentType.None:
      case HorizontalAlignmentType.Left:
        this.UpdateXYPosition(maxAscent, (IEnumerable<Syncfusion.Presentation.Layouting.TextInfo>) textInfoCollection, x1, y);
        break;
      case HorizontalAlignmentType.Center:
        usedWidth -= this.RemoveWhiteSpaces(textInfoCollection);
        float x2 = (float) (((double) shapeWidth - (double) usedWidth) / 2.0);
        this.UpdateXYPosition(maxAscent, (IEnumerable<Syncfusion.Presentation.Layouting.TextInfo>) textInfoCollection, x2, y);
        break;
      case HorizontalAlignmentType.Right:
        usedWidth -= this.RemoveWhiteSpaces(textInfoCollection);
        float x3 = shapeWidth - usedWidth;
        this.UpdateXYPosition(maxAscent, (IEnumerable<Syncfusion.Presentation.Layouting.TextInfo>) textInfoCollection, x3, y);
        break;
      case HorizontalAlignmentType.Justify:
        if (textInfoCollection.Count > 0)
        {
          string text = textInfoCollection[0].TextPart == null ? textInfoCollection[1].Text : textInfoCollection[0].Text;
          bool? defaultRtl = this.GetDefaultRTL();
          if (!Syncfusion.Presentation.Drawing.Helper.IsNullOrWhiteSpace(text) && (this.IsRTLText(text) || defaultRtl.HasValue && defaultRtl.Value))
          {
            usedWidth -= this.RemoveWhiteSpaces(textInfoCollection);
            float x4 = shapeWidth - usedWidth;
            if (!isLastLine)
            {
              this.LineJustify(ref usedWidth, maxAscent, shapeWidth, textInfoCollection, y);
              break;
            }
            this.UpdateXYPosition(maxAscent, (IEnumerable<Syncfusion.Presentation.Layouting.TextInfo>) textInfoCollection, x4, y);
            break;
          }
          if (!isLastLine)
          {
            this.LineJustify(ref usedWidth, maxAscent, shapeWidth, textInfoCollection, y);
            break;
          }
          this.UpdateXYPosition(maxAscent, (IEnumerable<Syncfusion.Presentation.Layouting.TextInfo>) textInfoCollection, x1, y);
          break;
        }
        break;
    }
    if ((double) usedWidth <= (double) maxWidth)
      return;
    maxWidth = usedWidth;
  }

  internal float GetLineSpace(ref float maxHeight)
  {
    float lineSpace = 0.0f;
    float defaultLineSpacing = (float) this.GetDefaultLineSpacing();
    if ((double) defaultLineSpacing != 0.0)
    {
      switch (this.CurrentLnSpcType)
      {
        case SizeType.Points:
          lineSpace = (float) (((double) defaultLineSpacing - (double) maxHeight) * 2.0 / 3.0);
          maxHeight = defaultLineSpacing;
          break;
        case SizeType.Percentage:
          if ((double) defaultLineSpacing != 1.0)
          {
            float num = maxHeight * defaultLineSpacing;
            lineSpace = (float) (((double) num - (double) maxHeight) * 2.0 / 3.0);
            maxHeight = num;
            break;
          }
          break;
      }
    }
    return lineSpace;
  }

  private void LineJustify(
    ref float usedWidth,
    float maxAscent,
    float shapeWidth,
    List<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection,
    float y)
  {
    List<Syncfusion.Presentation.Layouting.TextInfo> collection = new List<Syncfusion.Presentation.Layouting.TextInfo>();
    float x1 = textInfoCollection[0].TextPart != null ? textInfoCollection[0].Bounds.X : textInfoCollection[1].Bounds.X;
    string empty = string.Empty;
    foreach (Syncfusion.Presentation.Layouting.TextInfo textInfo1 in textInfoCollection)
    {
      if (textInfo1.TextPart == null)
      {
        collection.Add(textInfo1);
      }
      else
      {
        TextPart textPart = textInfo1.TextPart as TextPart;
        string text1 = textPart.Text;
        int startIndex1 = textInfo1.Position;
        int position = textInfo1.Position;
        if (!textPart.HasUnicode && text1.Substring(position, text1.Length - position).Contains(" "))
        {
          while (startIndex1 < textInfo1.Length + textInfo1.Position)
          {
            int startIndex2 = startIndex1;
            startIndex1 = this.GetTextIndexAfterSpace(text1, startIndex1);
            int index = text1.IndexOf('-', startIndex2) + 1;
            while (index > 0 && index < text1.Length && text1[index].ToString() == " ")
              ++index;
            if (index > 0 && (index < startIndex1 || startIndex1 == 0))
              startIndex1 = index;
            if (startIndex1 != 0 || position != text1.Length)
            {
              if (startIndex1 == 0)
                startIndex1 = text1.Length;
              if (startIndex1 <= text1.Length)
              {
                if (text1.Substring(position, startIndex1 - position).TrimEnd(' ').StartsWith(" ") && !string.IsNullOrEmpty(text1))
                {
                  position += text1.Length - text1.TrimStart(' ').Length;
                }
                else
                {
                  Syncfusion.Presentation.Layouting.TextInfo textInfo2 = new Syncfusion.Presentation.Layouting.TextInfo(textInfo1.TextPart);
                  string text2 = text1.Substring(position, startIndex1 - position);
                  textInfo2.Position = position;
                  textInfo2.Length = startIndex1 - position;
                  position += textInfo2.Length;
                  System.Drawing.Font updatedFont = this.GetUpdatedFont(textInfo1.TextPart.Font, (textInfo1.TextPart as TextPart).ScriptType);
                  SizeF sizeF = this.MeasureString(text2, updatedFont, ((Syncfusion.Presentation.RichText.Font) textInfo1.TextPart.Font).GetDefaultCapsType());
                  textInfo2.Bounds = new RectangleF(x1, textInfo1.Bounds.Y, sizeF.Width, sizeF.Height);
                  textInfo2.Ascent = textInfo1.Ascent;
                  collection.Add(textInfo2);
                  empty += text2;
                  x1 += sizeF.Width;
                }
              }
            }
            else
              break;
          }
        }
        else
        {
          empty += textInfo1.Text;
          collection.Add(textInfo1);
          x1 += textInfo1.Width;
        }
      }
    }
    textInfoCollection.Clear();
    textInfoCollection.AddRange((IEnumerable<Syncfusion.Presentation.Layouting.TextInfo>) collection);
    usedWidth -= this.RemoveWhiteSpaces(textInfoCollection);
    int num = empty.TrimEnd(' ').Length - empty.Replace(" ", "").Length;
    float x2 = (shapeWidth - usedWidth) / (float) num;
    for (int index = 0; index < textInfoCollection.Count; ++index)
    {
      Syncfusion.Presentation.Layouting.TextInfo textInfo = textInfoCollection[index];
      textInfo.Y += maxAscent - textInfo.Ascent + y;
      if (textInfo.TextPart != null)
        Paragraph.UpdateLineJustifyPosition(textInfoCollection, x2, index);
    }
  }

  private static void UpdateLineJustifyPosition(
    List<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection,
    float x,
    int index)
  {
    if (index == 0 || textInfoCollection[index - 1].TextPart == null)
      return;
    if (textInfoCollection[index - 1].Text.EndsWith(" "))
      textInfoCollection[index].X = textInfoCollection[index - 1].Bounds.Right + x;
    else
      textInfoCollection[index].X = textInfoCollection[index - 1].Bounds.Right;
  }

  private void UpdateXYPosition(
    float maxAscent,
    IEnumerable<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection,
    float x,
    float y)
  {
    foreach (Syncfusion.Presentation.Layouting.TextInfo textInfo in textInfoCollection)
    {
      textInfo.X += x;
      textInfo.Y += maxAscent - textInfo.Ascent + y;
    }
  }

  private float RemoveWhiteSpaces(List<Syncfusion.Presentation.Layouting.TextInfo> textInfoCollection)
  {
    float num = 0.0f;
    if (textInfoCollection.Count == 0)
      return num;
    Syncfusion.Presentation.Layouting.TextInfo textInfo = textInfoCollection[textInfoCollection.Count - 1];
    if (textInfo.TextPart == null)
      return num;
    string empty = string.Empty;
    for (int index = textInfo.Text.Length - 1; index >= 0; --index)
    {
      char ch = textInfo.Text[index];
      if (ch == ' ')
        empty += ch.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      else
        break;
    }
    if (!empty.Equals(string.Empty))
    {
      TextPart textPart = textInfo.TextPart as TextPart;
      num = this.MeasureString(empty, this.Presentation.FontSettings.GetFont(((Syncfusion.Presentation.RichText.Font) textPart.Font).GetFontNameToRender(textPart.ScriptType), ((Syncfusion.Presentation.RichText.Font) textPart.Font).GetDefaultSize(), this.GetFontStyle(textPart.Font, textPart.ScriptType)), TextCapsType.None).Width;
    }
    return num;
  }

  private int GetTextIndexAfterSpace(string text, int startIndex)
  {
    int index = text.IndexOf(' ', startIndex) + 1;
    if (index == 0 || index == text.Length)
      return index;
    while (text[index] == ' ')
    {
      ++index;
      if (index == text.Length)
        break;
    }
    return index;
  }

  internal int GetSpaceIndexBeforeText(string text, int startIndex)
  {
    int index = text.IndexOf(' ', startIndex);
    if (index < 0 || index == text.Length)
      return index;
    while (text[index] == ' ')
    {
      ++index;
      if (index == text.Length)
        break;
    }
    return index - 1;
  }

  internal string[] SplitTextByUnicode(string text, ref bool hasUnicode)
  {
    bool isChineseChar = false;
    bool isSymbol = false;
    bool isText = false;
    bool isTabSpace = false;
    this.ContainsChineeseCharOrSymbolOrText(text, ref isChineseChar, ref isSymbol, ref isText, ref isTabSpace);
    List<string> stringList;
    if (isChineseChar && isSymbol || isChineseChar && isText || isSymbol && isText || isTabSpace)
    {
      hasUnicode = true;
      stringList = new List<string>(text.Length);
      foreach (char ch in text.ToCharArray())
        stringList.Add(ch.ToString((IFormatProvider) CultureInfo.InvariantCulture));
    }
    else
    {
      stringList = new List<string>(1);
      stringList.Add(text);
    }
    return stringList.ToArray();
  }

  internal void ContainsChineeseCharOrSymbolOrText(
    string text,
    ref bool isChineseChar,
    ref bool isSymbol,
    ref bool isText,
    ref bool isTabSpace)
  {
    if (text == null)
      return;
    foreach (char ch in text.ToCharArray())
    {
      if (ch >= '　' && ch <= 'ヿ' || ch >= '\uFF00' && ch <= '\uFFEF' || ch >= '一' && ch <= '龯' || ch >= '㐀' && ch <= '\u4DBF')
        isChineseChar = true;
      else if (ch >= '◰' && ch <= '◿' || ch >= '가' && ch <= '\uFFEF')
        isSymbol = true;
      else if (ch == '\t')
        isTabSpace = true;
      else
        isText = true;
    }
  }

  internal void Close()
  {
    if (this._isDisposed)
      return;
    this.ClearAll();
    this._isDisposed = true;
  }

  private void ClearAll()
  {
    if (this._bulletFormat != null)
    {
      this._bulletFormat.Close();
      this._bulletFormat = (Syncfusion.Presentation.RichText.ListFormat) null;
    }
    if (this._charProps != null)
    {
      this._charProps.Close();
      this._charProps = (Syncfusion.Presentation.RichText.Font) null;
    }
    if (this._defaultFont != null)
    {
      this._defaultFont.Close();
      this._defaultFont = (Syncfusion.Presentation.RichText.Font) null;
    }
    if (this._textPartCollection != null)
    {
      this._textPartCollection.Close();
      this._textPartCollection = (Syncfusion.Presentation.RichText.TextParts) null;
    }
    this._paragraphCollection = (Paragraphs) null;
    if (this._paragrahInfo != null)
    {
      this._paragrahInfo.Close();
      this._paragrahInfo = (ParagraphInfo) null;
    }
    if (this._stringformat == null)
      return;
    this._stringformat.Dispose();
    this._stringformat = (StringFormat) null;
  }

  internal Paragraph InternalClone()
  {
    Paragraph paragraph = (Paragraph) this.MemberwiseClone();
    paragraph._bulletFormat = this._bulletFormat.Clone();
    paragraph._bulletFormat.SetParent(paragraph);
    if (this._charProps != null)
    {
      paragraph._charProps = this._charProps.Clone();
      paragraph._charProps.SetParent(paragraph);
    }
    if (this._defaultFont != null)
    {
      paragraph._defaultFont = this._defaultFont.Clone();
      paragraph._defaultFont.SetParent(paragraph);
    }
    if (this._paragrahInfo != null)
    {
      paragraph._paragrahInfo = this._paragrahInfo.Clone();
      paragraph._paragrahInfo.SetParent(paragraph);
    }
    if (this._stringformat != null)
      paragraph._stringformat = (StringFormat) this._stringformat.Clone();
    paragraph._textPartCollection = this._textPartCollection.Clone();
    paragraph._textPartCollection.SetParent(paragraph);
    paragraph._paragraphCollection = (Paragraphs) null;
    return paragraph;
  }

  internal List<long> TabPositionList
  {
    get => this._tabPositionList;
    set => this._tabPositionList = value;
  }

  internal List<TabAlignmentType> TabAlignmentTypeList
  {
    get => this._tabAlignmentTypeList;
    set => this._tabAlignmentTypeList = value;
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    if (this._charProps != null)
      this._charProps.SetParent(presentation);
    if (this._defaultFont != null)
      this._defaultFont.SetParent(presentation);
    this._textPartCollection.SetParent(presentation);
  }

  internal void SetParent(Paragraphs newParent) => this._paragraphCollection = newParent;

  internal void SetParent(Shape shape) => this._textPartCollection.SetParent(shape);
}
