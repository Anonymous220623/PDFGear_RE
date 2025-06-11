// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.RichText.Font
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Office;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using Syncfusion.Presentation.TableImplementation;
using Syncfusion.Presentation.Themes;
using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.RichText;

internal class Font : IFont
{
  private const float DefaultSize = 18f;
  private int? _characterSpacing;
  private int? _kern;
  private EffectList _effectList;
  private ColorObject _colorObject;
  private Syncfusion.Presentation.Drawing.Fill _fillFormat;
  private string _fontName = "";
  private string _altLang;
  private short _languageID = 1033;
  private bool _isLangSet;
  private int _fontSize;
  private bool _isSizeChanged;
  private bool _isNameChanged;
  private bool _isSpellingError;
  private int _baseLine;
  private int _options;
  private int _optionsDefault;
  private Paragraph _paragraph;
  private bool _isColorChanged;
  private bool _isColorSet;
  private TextPart _textPart;
  private string _symbolName;
  private string _eastAsianFont;
  private string bidiFontName;
  private Dictionary<string, Stream> _preservedElements;
  private BaseSlide _baseSlide;
  private ColorObject _underlineColor;
  private bool _isUnderlineColorSet;
  private bool _lastParaTextPart;
  private byte _pFlags;
  private bool? _rightToLeft;
  private bool _hasFontName;
  private bool _bidi;

  internal Font(Paragraph paragraph)
  {
    this._paragraph = paragraph;
    this._baseSlide = paragraph.BaseSlide;
    this._fillFormat = new Syncfusion.Presentation.Drawing.Fill(this);
    this._colorObject = new ColorObject(true, this);
  }

  internal Font(TextPart textPart)
  {
    this._textPart = textPart;
    this._paragraph = textPart.Paragraph;
    this._baseSlide = textPart.Paragraph.BaseSlide;
    this._fillFormat = new Syncfusion.Presentation.Drawing.Fill(this);
    this._underlineColor = new ColorObject(true);
    this._colorObject = new ColorObject(true, this);
  }

  internal bool Bidi
  {
    get => this._bidi;
    set => this._bidi = value;
  }

  internal bool HasBoldValue
  {
    get => ((int) this._pFlags & 1) != 0;
    set => this._pFlags = (byte) ((int) this._pFlags & 254 | (value ? 1 : 0));
  }

  internal IFill Fill => (IFill) this._fillFormat;

  public TextCapsType CapsType
  {
    get
    {
      if ((this._optionsDefault & 16 /*0x10*/) == 0)
      {
        Font fontObject = this._paragraph.GetFontObject();
        if (fontObject != null && (fontObject._optionsDefault & 16 /*0x10*/) != 0)
          return this.GetCapsTypeFromOptions(fontObject._options);
      }
      return this.GetCapsTypeFromOptions(this._options);
    }
    set
    {
      this._optionsDefault |= 16 /*0x10*/;
      this._options &= -49;
      switch (value)
      {
        case TextCapsType.Small:
          this._options |= 16 /*0x10*/;
          break;
        case TextCapsType.All:
          this._options |= 32 /*0x20*/;
          break;
      }
    }
  }

  public short LanguageID
  {
    get
    {
      if (this._isLangSet)
        return this._languageID;
      string defaultLanguage = this.GetDefaultLanguage();
      return !string.IsNullOrEmpty(defaultLanguage) && Enum.IsDefined(typeof (LocaleIDs), (object) defaultLanguage.Replace('-', '_')) ? (short) (LocaleIDs) Enum.Parse(typeof (LocaleIDs), defaultLanguage.Replace('-', '_')) : Helper.GetLanguageID(defaultLanguage);
    }
    set
    {
      this._languageID = value;
      this._isLangSet = true;
    }
  }

  internal TextCapsType GetCapsTypeFromOptions(int options)
  {
    switch ((options & 48 /*0x30*/) >> 4)
    {
      case 0:
        return TextCapsType.None;
      case 1:
        return TextCapsType.Small;
      case 2:
        return TextCapsType.All;
      default:
        return TextCapsType.None;
    }
  }

  internal TextCapsType GetDefaultCapsType()
  {
    if (this._paragraph.IsWithinList || (this._optionsDefault & 16 /*0x10*/) != 0)
      return this.GetCapsTypeFromOptions(this._options);
    if (((this._paragraph.Font as Font).GetDefaultOptions() & 16 /*0x10*/) != 0)
      return this.GetCapsTypeFromOptions((this._paragraph.Font as Font).GetOptions());
    return this._paragraph.BaseSlide is Slide ? this.DefaultCapsType() : this.DefaultCapsTypeForOtherSlide();
  }

  internal bool Compare(Font inputFont)
  {
    if (this.AltLanguage != inputFont.AltLanguage || this.Bold != inputFont.Bold || this.CapsType != inputFont.CapsType || (double) this.CharacterSpacing != (double) inputFont.CharacterSpacing || this.GetName() != inputFont.GetName() || this.FontNameBidi != inputFont.FontNameBidi || this.GetSize() != inputFont.GetSize() || this.Italic != inputFont.Italic)
      return false;
    int? kerning1 = this.Kerning;
    int? kerning2 = inputFont.Kerning;
    if ((kerning1.GetValueOrDefault() != kerning2.GetValueOrDefault() ? 1 : (kerning1.HasValue != kerning2.HasValue ? 1 : 0)) != 0 || this.Language != inputFont.Language)
      return false;
    bool? rightToLeft1 = this.RightToLeft;
    bool? rightToLeft2 = inputFont.RightToLeft;
    return (rightToLeft1.GetValueOrDefault() != rightToLeft2.GetValueOrDefault() ? 1 : (rightToLeft1.HasValue != rightToLeft2.HasValue ? 1 : 0)) == 0 && this.StrikeType == inputFont.StrikeType && this.Subscript == inputFont.Subscript && this.Superscript == inputFont.Superscript && this.Underline == inputFont.Underline && (int) this.Color.R == (int) inputFont.Color.R && (int) this.Color.G == (int) inputFont.Color.G && (int) this.Color.B == (int) inputFont.Color.B && this.Fill.FillType == inputFont.Fill.FillType;
  }

  internal void CopyFont(IFont sourceFont)
  {
    if (sourceFont.Bold || !sourceFont.Bold && (sourceFont as Font).HasBoldValue)
      this.Bold = sourceFont.Bold;
    if (this.CapsType != sourceFont.CapsType)
      this.CapsType = sourceFont.CapsType;
    if (sourceFont.Color.R != (byte) 0 || sourceFont.Color.G != (byte) 0 || sourceFont.Color.B != (byte) 0)
      this.Color = sourceFont.Color;
    if ((double) sourceFont.FontSize > 0.0 && (sourceFont as Font).IsSizeChanged)
      this.FontSize = sourceFont.FontSize;
    if (this.FontName != sourceFont.FontName)
      this.FontName = sourceFont.FontName;
    if (sourceFont.Italic)
      this.Italic = sourceFont.Italic;
    if (this.StrikeType != sourceFont.StrikeType)
      this.StrikeType = sourceFont.StrikeType;
    if (sourceFont.Subscript)
      this.Subscript = sourceFont.Subscript;
    if (sourceFont.Superscript)
      this.Superscript = sourceFont.Superscript;
    if (this.Underline == sourceFont.Underline)
      return;
    this.Underline = sourceFont.Underline;
  }

  private TextCapsType DefaultCapsTypeForOtherSlide()
  {
    bool isCapsSet = false;
    TextCapsType textCapsType = TextCapsType.None;
    Dictionary<string, Paragraph> styleList = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (styleList != null)
      textCapsType = this.GetCapsFromStyleList(styleList, ref isCapsSet);
    return !isCapsSet ? this.DefaultCapsFromPresentation(ref isCapsSet) : textCapsType;
  }

  private TextCapsType DefaultCapsFromPresentation(ref bool isCapsSet)
  {
    if (this._paragraph.BaseShape.BaseSlide.Presentation.DefaultTextStyle != null)
    {
      Dictionary<string, Paragraph> styleList = this._paragraph.BaseShape.BaseSlide.Presentation.DefaultTextStyle.StyleList;
      if (styleList != null && styleList.Count != 0)
      {
        string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
        if (styleList.ContainsKey(key))
        {
          Paragraph paragraph = styleList[key];
          if (paragraph.GetFontObject() == null)
            return TextCapsType.None;
          if ((paragraph.GetFontObject()._optionsDefault & 16 /*0x10*/) != 0)
            isCapsSet = true;
          return paragraph.GetFontObject().GetDefaultCapsType();
        }
      }
    }
    return TextCapsType.None;
  }

  private TextCapsType DefaultCapsType()
  {
    bool isCapsSet = false;
    TextCapsType textCapsType = TextCapsType.None;
    Dictionary<string, Paragraph> styleList1 = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (styleList1 != null)
      textCapsType = this.GetCapsFromStyleList(styleList1, ref isCapsSet);
    if (!isCapsSet && this._paragraph.BaseShape.DrawingType != DrawingType.PlaceHolder)
      return this.DefaultCapsFromPresentation(ref isCapsSet);
    if (!isCapsSet)
    {
      string tempName = this.SetTempName(Helper.GetNameFromPlaceholder(this._paragraph.BaseShape.ShapeName));
      Slide baseSlide = (Slide) this._paragraph.BaseSlide;
      string layoutIndex = baseSlide.ObtainLayoutIndex();
      if (layoutIndex != null)
      {
        LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
        foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
        {
          if (shape.PlaceholderFormat != null)
          {
            if (this._paragraph.BaseShape.PlaceholderFormat != null)
            {
              if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat))
              {
                Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                if (styleList2 != null)
                {
                  textCapsType = this.GetCapsFromStyleList(styleList2, ref isCapsSet);
                  break;
                }
                break;
              }
            }
            else
              break;
          }
        }
        if (!isCapsSet)
        {
          MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
          if (this._paragraph.BaseShape.PlaceholderFormat == null)
          {
            if (tempName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
              tempName = "Content";
            textCapsType = this.GetCapsFromStyleList(Helper.GetTextStyle(masterSlide, this.SetTempName(tempName)).StyleList, ref isCapsSet);
          }
          else
          {
            switch (this._paragraph.BaseShape.GetPlaceholder().GetPlaceholderType())
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
                    if (current.PlaceholderFormat != null && Helper.CheckPlaceholder(current.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
                    {
                      Dictionary<string, Paragraph> styleList3 = ((TextBody) current.TextBody).StyleList;
                      if (styleList3 != null)
                      {
                        textCapsType = this.GetCapsFromStyleList(styleList3, ref isCapsSet);
                        break;
                      }
                      break;
                    }
                  }
                  break;
                }
              default:
                if (tempName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                  tempName = "Content";
                textCapsType = this.GetCapsFromStyleList(Helper.GetTextStyle(masterSlide, this.SetTempName(tempName)).StyleList, ref isCapsSet);
                break;
            }
          }
        }
      }
    }
    return textCapsType;
  }

  private TextCapsType GetCapsFromStyleList(
    Dictionary<string, Paragraph> styleList,
    ref bool isCapsSet)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        if (style.GetFontObject() == null)
          return TextCapsType.None;
        if ((style.GetFontObject()._optionsDefault & 16 /*0x10*/) != 0)
          isCapsSet = true;
        return style.GetFontObject().GetDefaultCapsType();
      }
    }
    return TextCapsType.None;
  }

  internal bool HasFontName
  {
    get => this._hasFontName;
    set => this._hasFontName = value;
  }

  internal bool IsColorChanged
  {
    get => this._isColorChanged;
    set => this._isColorChanged = value;
  }

  public IColor Color
  {
    get
    {
      Syncfusion.Presentation.Presentation presentation = this._paragraph.BaseSlide != null ? this._paragraph.BaseSlide.Presentation : this._paragraph.Presentation;
      if (!this._isColorChanged)
      {
        Font fontObject = this._paragraph.GetFontObject();
        if (fontObject != null)
        {
          fontObject._colorObject.UpdateColorObject((object) presentation);
          return fontObject._colorObject.R > (byte) 0 || fontObject._colorObject.G > (byte) 0 || fontObject._colorObject.B > (byte) 0 ? (IColor) fontObject._colorObject : this.GetDefaultColor();
        }
      }
      this._colorObject.UpdateColorObject((object) presentation);
      return (IColor) this._colorObject;
    }
    set
    {
      this._colorObject.SetColor(ColorType.RGB, ((ColorObject) value).ToArgb());
      this._isColorChanged = true;
    }
  }

  internal IColor GetDefaultColor()
  {
    if (!this._paragraph.IsWithinList && !this._isColorChanged && !((Font) this._paragraph.Font)._isColorChanged)
      return this._paragraph.BaseSlide is Slide ? this.DefaultFontColor() : this.DefaultColorForOtherSlide();
    string themeColorValue = this._colorObject.ThemeColorValue;
    if (themeColorValue != null && this._paragraph.BaseSlide is Slide)
    {
      Slide baseSlide = (Slide) this._paragraph.BaseSlide;
      string layoutIndex = baseSlide.ObtainLayoutIndex();
      if (layoutIndex != null && baseSlide.Presentation.GetSlideLayout().ContainsKey(layoutIndex))
      {
        LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
        if (layoutSlide.ColorMap.Count != 0)
        {
          string themeIndex = Helper.GetThemeIndex(Helper.GetDefaultString(themeColorValue, layoutSlide.MasterSlide as MasterSlide), layoutSlide);
          ColorObject defaultColor = new ColorObject(true);
          defaultColor.SetColor(ColorType.Theme, themeIndex);
          defaultColor.UpdateColorObject((object) this._baseSlide.Presentation);
          return (IColor) defaultColor;
        }
      }
    }
    Syncfusion.Presentation.Presentation presentation = this._paragraph.BaseSlide != null ? this._paragraph.BaseSlide.Presentation : this._paragraph.Presentation;
    if (!this._isColorChanged && ((Font) this._paragraph.Font)._isColorChanged)
    {
      ColorObject colorObject = ((Font) this._paragraph.Font).GetColorObject();
      colorObject.UpdateColorObject((object) presentation);
      return (IColor) colorObject;
    }
    this._colorObject.UpdateColorObject((object) presentation);
    return (IColor) this._colorObject;
  }

  internal int GetOptions() => this._options;

  private IColor DefaultColorForOtherSlide()
  {
    IColor color = (IColor) null;
    if (this._paragraph.BaseShape != null)
    {
      if (this._paragraph.BaseShape.PreservedElements.Count != 0)
      {
        if (this._paragraph.BaseShape.PreservedElements.ContainsKey("style"))
          return this._paragraph.BaseShape.GetThemeColor("fontRef");
      }
      else
        color = this.GetColorFromStyleList(((TextBody) this._paragraph.BaseShape.TextBody).StyleList);
      if ((color == null || color.R == (byte) 0 && color.G == (byte) 0 && color.B == (byte) 0) && (this._paragraph.BaseSlide is MasterSlide || this._paragraph.BaseSlide is LayoutSlide))
        color = this.GetColorFromMasterSlide(!(this._paragraph.BaseSlide is LayoutSlide) ? this._paragraph.BaseSlide as MasterSlide : (this._paragraph.BaseSlide as LayoutSlide).MasterSlide as MasterSlide, Helper.GetNameFromPlaceholder(this._paragraph.BaseShape.ShapeName));
      if (!this._isColorSet)
        return this.DefaultColorFromPresentation();
    }
    return color ?? (IColor) new ColorObject();
  }

  public bool Bold
  {
    get
    {
      if ((this._optionsDefault & 1) == 0)
      {
        Font fontObject = this._paragraph.GetFontObject();
        if (fontObject != null)
          return (fontObject._optionsDefault & 1) != 0;
      }
      return (this._options & 1) != 0;
    }
    set
    {
      this._optionsDefault |= 1;
      if (value)
        this._options |= 1;
      else
        this._options &= -2;
    }
  }

  internal bool GetDefaultBold()
  {
    if (this._paragraph.IsWithinList || (this._optionsDefault & 1) != 0)
      return (this._options & 1) != 0;
    if (((this._paragraph.Font as Font).GetDefaultOptions() & 1) != 0)
      return ((this._paragraph.Font as Font).GetOptions() & 1) != 0;
    return this._paragraph.BaseSlide is Slide ? this.DefaultIsBold() : this.DefaultBoldForOtherSlide();
  }

  private bool DefaultBoldForOtherSlide()
  {
    bool isBoldSet = false;
    bool flag = false;
    Dictionary<string, Paragraph> styleList = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (styleList != null)
      flag = this.GetBoldFromStyleList(styleList, ref isBoldSet);
    else if (!isBoldSet && this._paragraph.BaseShape.DrawingType != DrawingType.PlaceHolder)
      flag = this.GetBoldFromStyleList(this._paragraph.BaseSlide.Presentation.DefaultTextStyle.StyleList, ref isBoldSet);
    return flag;
  }

  private bool DefaultIsBold()
  {
    if (this._paragraph.BaseShape is ITable)
      return this.DefaultBoldFromTable();
    bool isBoldSet = false;
    bool flag = false;
    Dictionary<string, Paragraph> styleList1 = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (styleList1 != null)
      flag = this.GetBoldFromStyleList(styleList1, ref isBoldSet);
    if (!isBoldSet && this._paragraph.BaseShape.DrawingType != DrawingType.PlaceHolder && this._paragraph.BaseSlide.Presentation.DefaultTextStyle != null)
      flag = this.GetBoldFromStyleList(this._paragraph.BaseSlide.Presentation.DefaultTextStyle.StyleList, ref isBoldSet);
    if (!isBoldSet)
    {
      string styleName = this.SetTempName(Helper.GetNameFromPlaceholder(this._paragraph.BaseShape.ShapeName));
      Slide baseSlide = (Slide) this._paragraph.BaseSlide;
      string layoutIndex = baseSlide.ObtainLayoutIndex();
      if (layoutIndex != null)
      {
        LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
        foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
        {
          if (shape.PlaceholderFormat != null)
          {
            if (this._paragraph.BaseShape.PlaceholderFormat != null)
            {
              if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat))
              {
                Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                if (styleList2 != null)
                {
                  flag = this.GetBoldFromStyleList(styleList2, ref isBoldSet);
                  break;
                }
                break;
              }
            }
            else
              break;
          }
        }
        if (!isBoldSet)
        {
          MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
          if (this._paragraph.BaseShape.PlaceholderFormat == null)
          {
            if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
              styleName = "Content";
            flag = this.GetBoldFromStyleList(Helper.GetTextStyle(masterSlide, styleName).StyleList, ref isBoldSet);
          }
          else
          {
            switch (this._paragraph.BaseShape.GetPlaceholder().GetPlaceholderType())
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
                    if (current.PlaceholderFormat != null && Helper.CheckPlaceholder(current.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
                    {
                      Dictionary<string, Paragraph> styleList3 = ((TextBody) current.TextBody).StyleList;
                      if (styleList3 != null)
                      {
                        flag = this.GetBoldFromStyleList(styleList3, ref isBoldSet);
                        break;
                      }
                      break;
                    }
                  }
                  break;
                }
              default:
                if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                  styleName = "Content";
                flag = this.GetBoldFromStyleList(Helper.GetTextStyle(masterSlide, styleName).StyleList, ref isBoldSet);
                break;
            }
          }
        }
      }
    }
    return flag;
  }

  private bool DefaultBoldFromTable()
  {
    OnOffStyleType onOffStyleType = OnOffStyleType.Def;
    Table baseShape = (Table) this._paragraph.BaseShape;
    TableStyle tableStyle = baseShape.TableStyle;
    Cell cell = this._paragraph.Parent.TextBody.Cell;
    Dictionary<string, Paragraph> styleList = ((TextBody) cell.TextBody).StyleList;
    if (styleList != null)
    {
      bool isBoldSet = false;
      this.GetBoldFromStyleList(styleList, ref isBoldSet);
      if (isBoldSet)
        return isBoldSet;
    }
    if (tableStyle != null)
    {
      if (cell.RowIndex == 1)
      {
        if (baseShape.HasHeaderRow && tableStyle.FirstRow != null && tableStyle.FirstRow.TableTextStyle != null)
        {
          onOffStyleType = tableStyle.FirstRow.TableTextStyle.Bold;
          if (onOffStyleType == OnOffStyleType.Def)
            onOffStyleType = tableStyle.FirstRow.TableTextStyle.Bold;
        }
      }
      else if (cell.RowIndex == baseShape.Rows.Count && baseShape.HasTotalRow && tableStyle.LastRow != null && tableStyle.LastRow.TableTextStyle != null)
      {
        onOffStyleType = tableStyle.LastRow.TableTextStyle.Bold;
        if (onOffStyleType == OnOffStyleType.Def)
          onOffStyleType = tableStyle.LastRow.TableTextStyle.Bold;
      }
      if (cell.ColumnIndex == 1)
      {
        if (baseShape.HasFirstColumn && tableStyle.FirstColumn != null && tableStyle.FirstColumn.TableTextStyle != null)
        {
          onOffStyleType = tableStyle.FirstColumn.TableTextStyle.Bold;
          if (onOffStyleType == OnOffStyleType.Def)
            onOffStyleType = tableStyle.FirstColumn.TableTextStyle.Bold;
        }
      }
      else if (cell.ColumnIndex == baseShape.Columns.Count && baseShape.HasLastColumn && tableStyle.LastColumn != null && tableStyle.LastRow != null && tableStyle.LastRow.TableTextStyle != null)
      {
        onOffStyleType = tableStyle.LastRow.TableTextStyle.Bold;
        if (onOffStyleType == OnOffStyleType.Def)
          onOffStyleType = tableStyle.LastRow.TableTextStyle.Bold;
      }
      if (cell.RowIndex != 1 && baseShape.HasBandedRows && baseShape.HasHeaderRow && tableStyle.HorizontalBand1Style != null && tableStyle.HorizontalBand1Style.TableTextStyle != null)
      {
        onOffStyleType = tableStyle.HorizontalBand1Style.TableTextStyle.Bold;
        if (onOffStyleType == OnOffStyleType.Def)
          onOffStyleType = tableStyle.HorizontalBand1Style.TableTextStyle.Bold;
      }
      if (onOffStyleType == OnOffStyleType.Def)
      {
        onOffStyleType = tableStyle.WholeTableStyle.TableTextStyle.Bold;
        if (onOffStyleType == OnOffStyleType.Def)
          onOffStyleType = tableStyle.WholeTableStyle.TableTextStyle.Bold;
      }
    }
    return Helper.GetBoolFromOnOff(onOffStyleType);
  }

  private bool GetBoldFromStyleList(Dictionary<string, Paragraph> styleList, ref bool isBoldSet)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        if (style.GetFontObject() == null)
          return false;
        if ((style.GetFontObject()._optionsDefault & 1) != 0)
          isBoldSet = true;
        return style.GetFontObject().GetDefaultBold();
      }
    }
    return false;
  }

  public bool Italic
  {
    get
    {
      if ((this._optionsDefault & 2) == 0)
      {
        Font fontObject = this._paragraph.GetFontObject();
        if (fontObject != null)
          return (fontObject._options & 2) != 0;
      }
      return (this._options & 2) != 0;
    }
    set
    {
      this._optionsDefault |= 2;
      if (value)
        this._options |= 2;
      else
        this._options &= -3;
    }
  }

  internal bool GetDefaultItalic()
  {
    if (this._paragraph.IsWithinList || (this._optionsDefault & 2) != 0)
      return (this._options & 2) != 0;
    if (((this._paragraph.Font as Font).GetDefaultOptions() & 2) != 0)
      return ((this._paragraph.Font as Font).GetOptions() & 2) != 0;
    return this._paragraph.BaseSlide is Slide ? this.DefaultIsItalic() : this.DefaultItalicForOtherSlide();
  }

  private bool DefaultItalicForOtherSlide()
  {
    bool isItalicSet = false;
    bool flag = false;
    Dictionary<string, Paragraph> styleList = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (styleList != null)
      flag = this.GetItalicFromStyleList(styleList, ref isItalicSet);
    else if (!isItalicSet && this._paragraph.BaseShape.DrawingType != DrawingType.PlaceHolder)
      flag = this.GetItalicFromStyleList(this._paragraph.BaseSlide.Presentation.DefaultTextStyle.StyleList, ref isItalicSet);
    return flag;
  }

  private bool DefaultIsItalic()
  {
    if (this._paragraph.BaseShape is ITable)
      return this.DefaultItalicFromTable();
    bool isItalicSet = false;
    bool flag = false;
    Dictionary<string, Paragraph> styleList1 = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (styleList1 != null)
      flag = this.GetItalicFromStyleList(styleList1, ref isItalicSet);
    if (!isItalicSet && this._paragraph.BaseShape != null && this._paragraph.BaseShape.DrawingType != DrawingType.PlaceHolder)
      return this.DefaultItalicFromPresentation(ref isItalicSet);
    if (!isItalicSet)
    {
      string styleName = this.SetTempName(Helper.GetNameFromPlaceholder(this._paragraph.BaseShape.ShapeName));
      Slide baseSlide = (Slide) this._paragraph.BaseSlide;
      string layoutIndex = baseSlide.ObtainLayoutIndex();
      if (layoutIndex != null)
      {
        LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
        foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
        {
          if (shape.PlaceholderFormat != null)
          {
            if (this._paragraph.BaseShape.PlaceholderFormat != null)
            {
              if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat))
              {
                Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                if (styleList2 != null)
                {
                  flag = this.GetItalicFromStyleList(styleList2, ref isItalicSet);
                  break;
                }
                break;
              }
            }
            else
              break;
          }
        }
        if (!isItalicSet)
        {
          MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
          if (this._paragraph.BaseShape.PlaceholderFormat == null)
          {
            if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
              styleName = "Content";
            flag = this.GetItalicFromStyleList(Helper.GetTextStyle(masterSlide, styleName).StyleList, ref isItalicSet);
          }
          else
          {
            switch (this._paragraph.BaseShape.GetPlaceholder().GetPlaceholderType())
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
                    if (current.PlaceholderFormat != null && Helper.CheckPlaceholder(current.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
                    {
                      Dictionary<string, Paragraph> styleList3 = ((TextBody) current.TextBody).StyleList;
                      if (styleList3 != null)
                      {
                        flag = this.GetItalicFromStyleList(styleList3, ref isItalicSet);
                        break;
                      }
                      break;
                    }
                  }
                  break;
                }
              default:
                if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                  styleName = "Content";
                flag = this.GetItalicFromStyleList(Helper.GetTextStyle(masterSlide, styleName).StyleList, ref isItalicSet);
                break;
            }
          }
        }
      }
    }
    return flag;
  }

  private bool DefaultItalicFromPresentation(ref bool isItalicSet)
  {
    bool flag = false;
    if (this._paragraph.BaseShape.BaseSlide.Presentation.DefaultTextStyle != null)
      flag = this.GetItalicFromStyleList(this._paragraph.BaseShape.BaseSlide.Presentation.DefaultTextStyle.StyleList, ref isItalicSet);
    return flag;
  }

  private bool DefaultItalicFromTable()
  {
    OnOffStyleType onOffStyleType = OnOffStyleType.Def;
    Table baseShape = (Table) this._paragraph.BaseShape;
    TableStyle tableStyle = baseShape.TableStyle;
    Cell cell = this._paragraph.Parent.TextBody.Cell;
    if (tableStyle != null)
    {
      if (cell.RowIndex == 1)
      {
        if (baseShape.HasHeaderRow && tableStyle.FirstRow != null && tableStyle.FirstRow.TableTextStyle != null)
        {
          onOffStyleType = tableStyle.FirstRow.TableTextStyle.Italic;
          if (onOffStyleType == OnOffStyleType.Def)
            onOffStyleType = tableStyle.FirstRow.TableTextStyle.Italic;
        }
      }
      else if (cell.RowIndex == baseShape.Rows.Count && baseShape.HasTotalRow && tableStyle.LastRow != null && tableStyle.LastRow.TableTextStyle != null)
      {
        onOffStyleType = tableStyle.LastRow.TableTextStyle.Italic;
        if (onOffStyleType == OnOffStyleType.Def)
          onOffStyleType = tableStyle.LastRow.TableTextStyle.Italic;
      }
      if (cell.ColumnIndex == 1)
      {
        if (baseShape.HasFirstColumn && tableStyle.FirstColumn != null && tableStyle.FirstColumn.TableTextStyle != null)
        {
          onOffStyleType = tableStyle.FirstColumn.TableTextStyle.Italic;
          if (onOffStyleType == OnOffStyleType.Def)
            onOffStyleType = tableStyle.FirstColumn.TableTextStyle.Italic;
        }
      }
      else if (cell.ColumnIndex == baseShape.Columns.Count && baseShape.HasLastColumn && tableStyle.LastColumn != null && tableStyle.LastRow != null && tableStyle.LastRow.TableTextStyle != null)
      {
        onOffStyleType = tableStyle.LastRow.TableTextStyle.Italic;
        if (onOffStyleType == OnOffStyleType.Def)
          onOffStyleType = tableStyle.LastRow.TableTextStyle.Italic;
      }
      if (cell.RowIndex != 1 && baseShape.HasBandedRows && baseShape.HasHeaderRow && tableStyle.HorizontalBand1Style != null && tableStyle.HorizontalBand1Style.TableTextStyle != null)
      {
        onOffStyleType = tableStyle.HorizontalBand1Style.TableTextStyle.Italic;
        if (onOffStyleType == OnOffStyleType.Def)
          onOffStyleType = tableStyle.HorizontalBand1Style.TableTextStyle.Italic;
      }
      if (onOffStyleType == OnOffStyleType.Def)
      {
        onOffStyleType = tableStyle.WholeTableStyle.TableTextStyle.Italic;
        if (onOffStyleType == OnOffStyleType.Def)
          onOffStyleType = tableStyle.WholeTableStyle.TableTextStyle.Italic;
      }
    }
    return Helper.GetBoolFromOnOff(onOffStyleType);
  }

  private bool GetItalicFromStyleList(Dictionary<string, Paragraph> styleList, ref bool isItalicSet)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        if (style.GetFontObject() == null)
          return false;
        if ((style.GetFontObject()._optionsDefault & 2) != 0)
          isItalicSet = true;
        return style.GetFontObject().GetDefaultItalic();
      }
    }
    return false;
  }

  public bool Subscript
  {
    get
    {
      if ((this._optionsDefault & 32 /*0x20*/) == 0)
      {
        Font fontObject = this._paragraph.GetFontObject();
        if (fontObject != null)
          return fontObject._baseLine < 0;
      }
      return this._baseLine < 0;
    }
    set
    {
      this._optionsDefault |= 32 /*0x20*/;
      if (value)
      {
        if (this._baseLine < 0)
          return;
        this._baseLine = -25000;
      }
      else
      {
        if (this._baseLine >= 0)
          return;
        this._baseLine = 0;
      }
    }
  }

  public bool Superscript
  {
    get
    {
      if ((this._optionsDefault & 32 /*0x20*/) == 0)
      {
        Font fontObject = this._paragraph.GetFontObject();
        if (fontObject != null)
          return fontObject._baseLine > 0;
      }
      return this._baseLine > 0;
    }
    set
    {
      this._optionsDefault |= 32 /*0x20*/;
      if (value)
      {
        if (this._baseLine > 0)
          return;
        this._baseLine = 30000;
      }
      else
      {
        if (this._baseLine <= 0)
          return;
        this._baseLine = 0;
      }
    }
  }

  public string FontName
  {
    get
    {
      Syncfusion.Presentation.Presentation presentation = this._paragraph.BaseSlide != null ? this._paragraph.BaseSlide.Presentation : this._paragraph.Presentation;
      return !this._isNameChanged && !this._paragraph.IsWithinList ? this.DefaultFontName(FontScriptType.English, (string) null) : Helper.GetFontNameFromTheme(presentation.Theme, this._fontName, FontScriptType.English, (string) null);
    }
    set
    {
      this._fontName = value;
      this._eastAsianFont = value;
      this.bidiFontName = value;
      this._isNameChanged = true;
      this._hasFontName = true;
    }
  }

  internal string GetFontNameToRender(FontScriptType scriptType)
  {
    string defaultLanguage = this.GetDefaultLanguage();
    string defaultFontName = this.GetDefaultFontName(scriptType, defaultLanguage);
    return string.IsNullOrEmpty(defaultFontName) ? Helper.GetDefaultFontName(scriptType) : defaultFontName;
  }

  internal string GetDefaultFontName(FontScriptType scriptType, string lang)
  {
    Syncfusion.Presentation.Presentation presentation = this._paragraph.BaseSlide != null ? this._paragraph.BaseSlide.Presentation : this._paragraph.Presentation;
    Theme theme = presentation.Theme;
    if (presentation.Masters.Count > 1 && this._paragraph.BaseSlide is MasterSlide)
      theme = (this._paragraph.BaseSlide as MasterSlide).Theme;
    if (this._paragraph.IsWithinList || this._isNameChanged || !string.IsNullOrEmpty(this.GetFontName(scriptType)))
      return Helper.GetFontNameFromTheme(theme, this.GetFontName(scriptType), scriptType, lang);
    string fontName = (this._paragraph.Font as Font).GetFontName(scriptType);
    if (!string.IsNullOrEmpty(fontName))
      return Helper.GetFontNameFromTheme(presentation.Theme, fontName, scriptType, lang);
    if (this._paragraph.BaseSlide is Slide || this._paragraph.BaseSlide is NotesSlide)
      return this.DefaultFontName(scriptType, lang);
    return this._paragraph.BaseSlide is HandoutMaster ? ((HandoutMaster) this._paragraph.BaseSlide).ThemeCollection.MinorFont.Latin : this.DefaultFontNameForOtherSlide(scriptType, lang);
  }

  private string DefaultFontNameForOtherSlide(FontScriptType scriptType, string lang)
  {
    string str = "";
    Dictionary<string, Paragraph> styleList = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (styleList != null)
      str = this.GetNameFromStyleList(styleList, scriptType, lang);
    if (this._paragraph.BaseShape != null && this._paragraph.BaseShape.DrawingType != DrawingType.PlaceHolder)
    {
      if (str != "")
        return str;
      return this._paragraph.BaseShape.PreservedElements.ContainsKey("style") ? Helper.GetFontNameFromTheme(this._paragraph.BaseSlide.BaseTheme, this._paragraph.BaseShape.GetFontFromStyle("fontRef"), scriptType, lang) : this.DefaultNameFromPresentation(scriptType, lang);
    }
    return !(str == "") ? str : "Arial";
  }

  internal int GetSize() => this._fontSize == 0 ? 1800 : this._fontSize;

  public float FontSize
  {
    get
    {
      double num = 0.0;
      if (this._isSizeChanged)
        num = (double) this._fontSize;
      else if ((this._paragraph.Font as Font)._fontSize != 0)
        num = (double) (this._paragraph.Font as Font)._fontSize;
      else if (this._paragraph.Font != null)
      {
        num = this._paragraph.GetFontObject().DefaultFontSize() * 100.0;
        if (num == 1800.0 && this._paragraph.TextParts.Count > 1 && (this._paragraph.TextParts[0].Font as Font)._fontSize > 0)
          num = (double) (this._paragraph.TextParts[0].Font as Font)._fontSize;
      }
      return Convert.ToSingle(num == 0.0 ? 18.0 : num / 100.0);
    }
    set
    {
      double num = (double) value * 100.0;
      this._fontSize = num >= 100.0 && num <= 400000.0 ? (int) num : throw new ArgumentException("Invalid FontSize");
      this._isSizeChanged = true;
    }
  }

  internal float GetDefaultCharacterSpacing()
  {
    if (this.IsCharacterSpacingApplied())
      return this.CharacterSpacing;
    if ((this._paragraph.Font as Font).IsCharacterSpacingApplied())
      return (this._paragraph.Font as Font).CharacterSpacing;
    if (this._paragraph.BaseShape != null && this._paragraph.BaseShape.DrawingType == DrawingType.PlaceHolder)
      return this.DefaultCharSpacing();
    if (this._paragraph.BaseShape == null || this._paragraph.BaseShape is Table)
      return 0.0f;
    float? nullable = new float?();
    if (((TextBody) this._paragraph.BaseShape.TextBody).StyleList != null)
      nullable = this.GetCharSpacingFromStyleList(((TextBody) this._paragraph.BaseShape.TextBody).StyleList);
    if (!nullable.HasValue)
      nullable = this.DefaultCharSpacingFromPresentation();
    return nullable.HasValue ? nullable.Value : 0.0f;
  }

  internal string GetDefaultLanguage()
  {
    if (this.Language != null)
      return this.Language;
    if ((this._paragraph.Font as Font).Language != null)
      return (this._paragraph.Font as Font).Language;
    if (this._paragraph.BaseShape != null && this._paragraph.BaseShape.DrawingType == DrawingType.PlaceHolder)
      return this.DefaultLanguage();
    if (this._paragraph.BaseShape == null || this._paragraph.BaseShape is Table)
      return this.Language;
    return ((TextBody) this._paragraph.BaseShape.TextBody).StyleList != null ? this.GetLanguageFromStyleList(((TextBody) this._paragraph.BaseShape.TextBody).StyleList) : this.DefaultLanguageFromPresentation();
  }

  internal float GetDefaultSize()
  {
    if (this._paragraph.BaseShape != null && this._paragraph.BaseShape.DrawingType == DrawingType.PlaceHolder)
    {
      if (!this._paragraph.IsWithinList && !this._isSizeChanged && !((Font) this._paragraph.Font)._isSizeChanged)
      {
        if (this._paragraph.BaseSlide is Slide || this._paragraph.BaseSlide is NotesSlide)
          return Convert.ToSingle(this.DefaultFontSize());
        if (this._paragraph.BaseSlide is LayoutSlide)
          return Convert.ToSingle(this.DefaultSizeForLayout());
        if (this._paragraph.BaseSlide is HandoutMaster)
          return Convert.ToSingle(this.DefaultSizeForHandout());
      }
      if (this._paragraph.BaseShape != null && ((TextBody) this._paragraph.BaseShape.TextBody).IsAutoSize)
        return Convert.ToSingle(this.SetFontScale());
      return this._fontSize == 0 && ((Font) this._paragraph.Font)._isSizeChanged ? Convert.ToSingle((double) (this._paragraph.Font as Font)._fontSize == 0.0 ? 18.0 : (double) (this._paragraph.Font as Font)._fontSize / 100.0) : Convert.ToSingle(this._fontSize == 0 ? 18.0 : (double) this._fontSize / 100.0);
    }
    if (!this._paragraph.IsWithinList && !this._isSizeChanged && !((Font) this._paragraph.Font)._isSizeChanged)
    {
      if (this._paragraph.BaseShape == null || this._paragraph.BaseShape is Table)
        return Convert.ToSingle((double) (this._paragraph.Font as Font)._fontSize == 0.0 ? 18.0 : (double) (this._paragraph.Font as Font)._fontSize / 100.0);
      double num = 0.0;
      if (((TextBody) this._paragraph.BaseShape.TextBody).StyleList != null)
      {
        TextBody textBody = (TextBody) this._paragraph.BaseShape.TextBody;
        num = (double) this.GetSizeFromStyleList(textBody.StyleList);
        if (textBody.HasFontScale)
          num = (double) this.ApplyFontScale(num, textBody.GetFontScale());
      }
      return Convert.ToSingle(num == 0.0 ? this.DefaultSizeFromPresentation() : num);
    }
    if (this._isSizeChanged && this._paragraph.BaseShape != null && !((TextBody) this._paragraph.BaseShape.TextBody).IsAutoSize)
      return Convert.ToSingle((double) this._fontSize == 0.0 ? 18.0 : (double) this._fontSize / 100.0);
    if (((Font) this._paragraph.Font)._isSizeChanged)
      return Convert.ToSingle((double) (this._paragraph.Font as Font)._fontSize == 0.0 ? 18.0 : (double) (this._paragraph.Font as Font)._fontSize / 100.0);
    return this._paragraph.BaseShape != null && ((TextBody) this._paragraph.BaseShape.TextBody).IsAutoSize ? Convert.ToSingle(this.SetFontScale()) : Convert.ToSingle((double) this._fontSize == 0.0 ? 18.0 : (double) this._fontSize / 100.0);
  }

  private double DefaultSizeForHandout()
  {
    double num = 0.0;
    Dictionary<string, Paragraph> styleList = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (styleList != null)
      num = (double) this.GetSizeFromStyleList(styleList);
    return num != 0.0 ? num : 18.0;
  }

  private double DefaultSizeFromPresentation()
  {
    double num = 0.0;
    if (this._paragraph.BaseShape.BaseSlide.Presentation.DefaultTextStyle != null)
    {
      Dictionary<string, Paragraph> styleList = this._paragraph.BaseShape.BaseSlide.Presentation.DefaultTextStyle.StyleList;
      if (styleList != null && styleList.Count != 0)
      {
        string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
        if (styleList.ContainsKey(key))
        {
          Paragraph paragraph = styleList[key];
          if (paragraph.GetFontObject() != null)
            num = (double) paragraph.GetFontObject().GetDefaultSize();
        }
      }
    }
    return num != 0.0 ? num : 18.0;
  }

  private double SetFontScale()
  {
    TextBody textBody = (TextBody) this._paragraph.BaseShape.TextBody;
    if (textBody.IsAutoSize)
    {
      double fontScale = (double) textBody.GetFontScale();
      if (fontScale != 0.0)
      {
        int uint16 = (int) Convert.ToUInt16(Math.Ceiling(Math.Round((double) ((this._fontSize > 0 ? this._fontSize : (this._paragraph.Font as Font).GetSize()) / 100) * (fontScale / 10000.0) / 10.0, MidpointRounding.AwayFromZero)));
        return uint16 > 0 ? (double) uint16 : 18.0;
      }
    }
    return (double) this._fontSize != 0.0 ? (double) this._fontSize / 100.0 : 18.0;
  }

  private double DefaultSizeForLayout()
  {
    double num = 0.0;
    Dictionary<string, Paragraph> styleList = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (styleList != null)
      num = (double) this.GetSizeFromStyleList(styleList);
    if (num == 0.0)
    {
      string nameFromPlaceholder = Helper.GetNameFromPlaceholder(this._paragraph.BaseShape.ShapeName);
      MasterSlide masterSlide = (MasterSlide) ((LayoutSlide) this._paragraph.BaseSlide).MasterSlide;
      if (this._paragraph.BaseShape.ShapeName.Contains(nameFromPlaceholder) || this._paragraph.BaseShape.DrawingType == DrawingType.None)
        num = (double) this.GetSizeFromStyleList(Helper.GetTextStyle(masterSlide, nameFromPlaceholder).StyleList);
      if (num != 0.0)
        return num;
    }
    return num;
  }

  private IColor DefaultFontColor()
  {
    if (this._paragraph.BaseShape is ITable)
      return this.GetColorFromTableStyle();
    if (((Font) this._paragraph.Font).IsColorSet && ((Font) this._paragraph.Font).IsColorChanged)
      return ((Font) this._paragraph.Font).Color;
    if (this._paragraph.BaseShape != null && this._paragraph.BaseShape.DrawingType != DrawingType.PlaceHolder)
    {
      IColor color = (IColor) null;
      Dictionary<string, Paragraph> styleList = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
      if (styleList != null && styleList.Count != 0)
        color = this.GetColorFromStyleList(styleList);
      if (this._isColorSet && color != null)
        return color;
      return this._paragraph.BaseShape.PreservedElements.Count != 0 && this._paragraph.BaseShape.PreservedElements.ContainsKey("style") ? this._paragraph.BaseShape.GetThemeColor("fontRef") : this.DefaultColorFromPresentation();
    }
    if (this._paragraph.BaseShape != null && this._paragraph.BaseShape.DrawingType == DrawingType.PlaceHolder)
    {
      IColor color = (IColor) null;
      Dictionary<string, Paragraph> styleList1 = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
      if (styleList1 != null)
        color = this.GetColorFromStyleList(styleList1);
      if (!this._isColorSet || color == null)
      {
        string nameFromPlaceholder = Helper.GetNameFromPlaceholder(this._paragraph.BaseShape.ShapeName);
        if (this._paragraph.BaseSlide is Slide)
        {
          Slide baseSlide = (Slide) this._paragraph.BaseSlide;
          string layoutIndex = baseSlide.ObtainLayoutIndex();
          if (layoutIndex != null && baseSlide.Presentation.GetSlideLayout().ContainsKey(layoutIndex))
          {
            LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
            foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
            {
              if (shape.PlaceholderFormat != null)
              {
                if (this._paragraph.BaseShape.PlaceholderFormat != null)
                {
                  if (!(shape is Picture) && Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat))
                  {
                    Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                    if (styleList2 != null)
                    {
                      color = this.GetColorFromStyleList(styleList2);
                      break;
                    }
                    break;
                  }
                }
                else
                  break;
              }
            }
            if (color == null || !this._isColorSet)
            {
              MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
              color = this.GetColorFromMasterSlide(masterSlide, nameFromPlaceholder);
              if (layoutSlide.ColorMap.Count != 0 && color != null)
              {
                ColorObject colorObject = new ColorObject(true);
                string themeColorValue = ((ColorObject) color).ThemeColorValue;
                if (themeColorValue != null)
                {
                  string themeIndex = Helper.GetThemeIndex(Helper.GetDefaultString(themeColorValue, masterSlide), layoutSlide);
                  colorObject.SetColor(ColorType.Theme, themeIndex);
                  colorObject.UpdateColorObject((object) this._baseSlide.Presentation);
                  color = (IColor) colorObject;
                }
              }
            }
          }
        }
      }
      if (color != null)
        return color;
    }
    return (IColor) this._colorObject;
  }

  private IColor GetColorFromMasterSlide(MasterSlide masterSlide, string tempName)
  {
    IColor colorFromMasterSlide = (IColor) null;
    if (this._paragraph.BaseShape.PlaceholderFormat == null)
    {
      tempName = this.SetTempName(tempName);
      colorFromMasterSlide = this.GetColorFromStyleList(Helper.GetTextStyle(masterSlide, tempName).StyleList);
    }
    else
    {
      PlaceholderType placeholderType = this._paragraph.BaseShape.GetPlaceholder().GetPlaceholderType();
      switch (placeholderType)
      {
        case PlaceholderType.Title:
        case PlaceholderType.Body:
        case PlaceholderType.CenterTitle:
        case PlaceholderType.Subtitle:
        case PlaceholderType.SlideNumber:
        case PlaceholderType.Header:
        case PlaceholderType.Footer:
        case PlaceholderType.Date:
          foreach (Shape shape in (IEnumerable<ISlideItem>) masterSlide.Shapes)
          {
            if (shape.PlaceholderFormat != null && Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
            {
              Dictionary<string, Paragraph> styleList = ((TextBody) shape.TextBody).StyleList;
              if (styleList != null)
              {
                colorFromMasterSlide = this.GetColorFromStyleList(styleList);
                break;
              }
              break;
            }
          }
          if (!Helper.IsHeaderFooterShape(placeholderType) && (colorFromMasterSlide == null || !this._isColorSet))
          {
            tempName = this.SetTempName(tempName);
            colorFromMasterSlide = this.GetColorFromStyleList(Helper.GetTextStyle(masterSlide, tempName).StyleList);
            break;
          }
          break;
        default:
          tempName = this.SetTempName(tempName);
          colorFromMasterSlide = this.GetColorFromStyleList(Helper.GetTextStyle(masterSlide, tempName).StyleList);
          break;
      }
    }
    return colorFromMasterSlide;
  }

  private IColor DefaultColorFromPresentation()
  {
    IColor color = (IColor) null;
    if (this._paragraph.BaseShape.BaseSlide.Presentation.DefaultTextStyle != null)
    {
      Dictionary<string, Paragraph> styleList = this._paragraph.BaseShape.BaseSlide.Presentation.DefaultTextStyle.StyleList;
      if (styleList != null && styleList.Count != 0)
      {
        string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
        if (styleList.ContainsKey(key))
        {
          Paragraph paragraph = styleList[key];
          if (paragraph.GetFontObject() != null)
            color = paragraph.GetFontObject().GetDefaultColor();
        }
      }
    }
    IColor empty;
    return color ?? (empty = ColorObject.Empty);
  }

  private IColor GetColorFromTableStyle()
  {
    IColor color = (IColor) null;
    Table baseShape = (Table) this._paragraph.BaseShape;
    TableStyle tableStyle = baseShape.TableStyle;
    Cell cell = this._paragraph.Parent.TextBody.Cell;
    if (tableStyle != null)
    {
      if (cell.RowIndex == 1)
      {
        if (baseShape.HasHeaderRow && tableStyle.FirstRow != null && tableStyle.FirstRow.TableTextStyle != null && tableStyle.FirstRow.TableTextStyle.HasFontColor)
          color = tableStyle.FirstRow.TableTextStyle.TextColor ?? tableStyle.FirstRow.TableTextStyle.TextRefColor;
      }
      else if (cell.RowIndex == baseShape.Rows.Count && baseShape.HasTotalRow && tableStyle.LastRow != null && tableStyle.LastRow.TableTextStyle != null && tableStyle.LastRow.TableTextStyle.HasFontColor)
        color = tableStyle.LastRow.TableTextStyle.TextColor ?? tableStyle.LastRow.TableTextStyle.TextRefColor;
      if (cell.ColumnIndex == 1)
      {
        if (baseShape.HasFirstColumn && tableStyle.FirstColumn != null && tableStyle.FirstColumn.TableTextStyle != null && tableStyle.FirstColumn.TableTextStyle.HasFontColor)
          color = tableStyle.FirstColumn.TableTextStyle.TextColor ?? tableStyle.FirstColumn.TableTextStyle.TextRefColor;
      }
      else if (cell.ColumnIndex == baseShape.Columns.Count && baseShape.HasLastColumn && tableStyle.LastColumn != null && tableStyle.LastRow != null && tableStyle.LastRow.TableTextStyle != null && tableStyle.LastRow.TableTextStyle.HasFontColor)
        color = tableStyle.LastRow.TableTextStyle.TextColor ?? tableStyle.LastRow.TableTextStyle.TextRefColor;
      if (cell.RowIndex != 1 && baseShape.HasBandedRows && baseShape.HasHeaderRow && tableStyle.HorizontalBand1Style != null && tableStyle.HorizontalBand1Style.TableTextStyle != null && tableStyle.HorizontalBand1Style.TableTextStyle.HasFontColor)
        color = tableStyle.HorizontalBand1Style.TableTextStyle.TextColor ?? tableStyle.HorizontalBand1Style.TableTextStyle.TextRefColor;
      if (color == null)
        color = tableStyle.WholeTableStyle.TableTextStyle.TextColor ?? tableStyle.WholeTableStyle.TableTextStyle.TextRefColor;
    }
    return color ?? (IColor) this._colorObject;
  }

  private IColor GetColorFromStyleList(Dictionary<string, Paragraph> styleList)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        if (style.GetFontObject() == null)
          return (IColor) null;
        this._isColorSet = style.GetFontObject()._isColorSet;
        return style.GetFontObject().GetDefaultColor();
      }
    }
    return (IColor) null;
  }

  private string DefaultFontName(FontScriptType scriptType, string lang)
  {
    string fontName = this.GetFontName(scriptType);
    Syncfusion.Presentation.Presentation presentation = this._paragraph.BaseSlide != null ? this._paragraph.BaseSlide.Presentation : this._paragraph.Presentation;
    if (!string.IsNullOrEmpty(fontName))
      return Helper.GetFontNameFromTheme(presentation.Theme, fontName, scriptType, lang);
    string name = (this._paragraph.Font as Font).GetFontName(scriptType);
    if (!string.IsNullOrEmpty(name))
      return Helper.GetFontNameFromTheme(presentation.Theme, name, scriptType, lang);
    if (this._paragraph.BaseShape is ITable)
      return this.GetNameFromTableStyle(scriptType, lang);
    Dictionary<string, Paragraph> styleList1 = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (styleList1 != null)
    {
      name = this.GetNameFromStyleList(styleList1, scriptType, lang);
      if (name != "")
        return name;
    }
    if (this._paragraph.BaseShape != null && this._paragraph.BaseShape.DrawingType != DrawingType.PlaceHolder)
    {
      if (name != "")
        return name;
      return this._paragraph.BaseShape.PreservedElements.ContainsKey("style") ? Helper.GetFontNameFromTheme(this._paragraph.BaseSlide.BaseTheme, this._paragraph.BaseShape.GetFontFromStyle("fontRef"), scriptType, lang) : this.DefaultNameFromPresentation(scriptType, lang);
    }
    if (name == "")
    {
      string nameFromPlaceholder = Helper.GetNameFromPlaceholder(this._paragraph.BaseShape.ShapeName);
      if (this._paragraph.BaseSlide is Slide)
      {
        Slide baseSlide = (Slide) this._paragraph.BaseSlide;
        string layoutIndex = baseSlide.ObtainLayoutIndex();
        if (layoutIndex != null)
          return this.GetFontNameFromLayoutSlide((ILayoutSlide) baseSlide.Presentation.GetSlideLayout()[layoutIndex], nameFromPlaceholder, styleList1, scriptType, lang);
      }
      else if (this._paragraph.BaseSlide is NotesSlide)
      {
        foreach (Shape shape in (IEnumerable<ISlideItem>) this._paragraph.BaseSlide.Presentation.NotesMaster.Shapes)
        {
          if (shape.PlaceholderFormat != null)
          {
            if (this._paragraph.BaseShape.PlaceholderFormat != null)
            {
              if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
              {
                Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                if (styleList2 != null)
                {
                  name = this.GetNameFromStyleList(styleList2, scriptType, lang);
                  break;
                }
                break;
              }
            }
            else
              break;
          }
        }
        if (string.IsNullOrEmpty(name))
          name = this.DefaultNameFromNotesMaster(scriptType, lang);
      }
      else
      {
        if (this._paragraph.BaseSlide is LayoutSlide)
          return this.GetFontNameFromLayoutSlide((ILayoutSlide) (this._paragraph.BaseSlide as LayoutSlide), nameFromPlaceholder, styleList1, scriptType, lang);
        if (this._paragraph.BaseSlide is MasterSlide)
          return this.GetFontNameFromMasterSlide(this._paragraph.BaseSlide as MasterSlide, nameFromPlaceholder, styleList1, scriptType, lang);
      }
    }
    else
      name = Helper.GetFontNameFromTheme(this._paragraph.BaseSlide.BaseTheme, name, scriptType, lang);
    return !(name == "") ? name : "Arial";
  }

  private string GetFontNameFromLayoutSlide(
    ILayoutSlide layoutSlide,
    string tempName,
    Dictionary<string, Paragraph> styleList,
    FontScriptType scriptType,
    string lang)
  {
    string name = "";
    foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
    {
      if (shape.PlaceholderFormat != null)
      {
        if (this._paragraph.BaseShape.PlaceholderFormat != null)
        {
          if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat))
          {
            styleList = ((TextBody) shape.TextBody).StyleList;
            if (styleList != null)
            {
              name = this.GetNameFromStyleList(styleList, scriptType, lang);
              break;
            }
            break;
          }
        }
        else
          break;
      }
    }
    return name == "" ? this.GetFontNameFromMasterSlide((MasterSlide) layoutSlide.MasterSlide, tempName, styleList, scriptType, lang) : Helper.GetFontNameFromTheme(this._paragraph.BaseSlide.BaseTheme, name, scriptType, lang);
  }

  private string GetFontNameFromMasterSlide(
    MasterSlide masterSlide,
    string tempName,
    Dictionary<string, Paragraph> styleList,
    FontScriptType scriptType,
    string lang)
  {
    string name = "";
    if (this._paragraph.BaseShape.PlaceholderFormat == null)
    {
      tempName = this.SetTempName(tempName);
      if (this._paragraph.BaseShape.ShapeName.Contains(tempName) || this._paragraph.BaseShape.DrawingType != DrawingType.Table)
        name = this.GetNameFromStyleList(Helper.GetTextStyle(masterSlide, tempName).StyleList, scriptType, lang);
      if (name != "")
        return Helper.GetFontNameFromTheme(this._paragraph.BaseSlide.BaseTheme, name, scriptType, lang);
    }
    else
    {
      PlaceholderType placeholderType = this._paragraph.BaseShape.GetPlaceholder().GetPlaceholderType();
      switch (placeholderType)
      {
        case PlaceholderType.Title:
        case PlaceholderType.Body:
        case PlaceholderType.CenterTitle:
        case PlaceholderType.Subtitle:
        case PlaceholderType.SlideNumber:
        case PlaceholderType.Header:
        case PlaceholderType.Footer:
        case PlaceholderType.Date:
          foreach (Shape shape in (IEnumerable<ISlideItem>) masterSlide.Shapes)
          {
            if (shape.PlaceholderFormat != null && Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
            {
              styleList = ((TextBody) shape.TextBody).StyleList;
              if (styleList != null)
              {
                name = this.GetNameFromStyleList(styleList, scriptType, lang);
                break;
              }
              break;
            }
          }
          if (!Helper.IsHeaderFooterShape(placeholderType) && name == "")
          {
            tempName = this.SetTempName(tempName);
            if (this._paragraph.BaseShape.ShapeName.Contains(tempName) || this._paragraph.BaseShape.DrawingType != DrawingType.Table)
            {
              TextBody textStyle = Helper.GetTextStyle(masterSlide, tempName);
              this.SetTempName(tempName);
              name = this.GetNameFromStyleList(textStyle.StyleList, scriptType, lang);
            }
            if (name != "")
              return Helper.GetFontNameFromTheme(this._paragraph.BaseSlide.BaseTheme, name, scriptType, lang);
            break;
          }
          if (name == "")
            return this.DefaultNameFromPresentation(scriptType, lang);
          break;
        default:
          tempName = this.SetTempName(tempName);
          if (this._paragraph.BaseShape.ShapeName.Contains(tempName) || this._paragraph.BaseShape.DrawingType != DrawingType.Table)
          {
            TextBody textStyle = Helper.GetTextStyle(masterSlide, tempName);
            this.SetTempName(tempName);
            name = this.GetNameFromStyleList(textStyle.StyleList, scriptType, lang);
          }
          if (name != "")
            return Helper.GetFontNameFromTheme(this._paragraph.BaseSlide.BaseTheme, name, scriptType, lang);
          break;
      }
    }
    return name;
  }

  internal IFill GetDefaultFillFormat()
  {
    IFill defaultFillFormat = (IFill) null;
    if (this._fillFormat.FillType == FillType.Automatic)
    {
      Dictionary<string, Paragraph> styleList1 = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
      if (styleList1 != null)
        defaultFillFormat = this.GetFillFormatFromStyleList(styleList1);
      if (!this.IsColorChanged && this._paragraph.BaseShape.DrawingType != DrawingType.PlaceHolder && this._paragraph.BaseSlide.Presentation.DefaultTextStyle != null)
        defaultFillFormat = this.GetFillFormatFromStyleList(this._paragraph.BaseSlide.Presentation.DefaultTextStyle.StyleList);
      if ((defaultFillFormat == null || defaultFillFormat.FillType == FillType.Automatic) && this._paragraph.BaseSlide is Slide)
      {
        string styleName = this.SetTempName(Helper.GetNameFromPlaceholder(this._paragraph.BaseShape.ShapeName));
        string layoutIndex = ((Slide) this._paragraph.BaseSlide).ObtainLayoutIndex();
        if (layoutIndex != null)
        {
          LayoutSlide layoutSlide = this._paragraph.BaseSlide.Presentation.GetSlideLayout()[layoutIndex];
          foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
          {
            if (shape.PlaceholderFormat != null)
            {
              if (this._paragraph.BaseShape.PlaceholderFormat != null)
              {
                if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat))
                {
                  Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                  if (styleList2 != null)
                  {
                    defaultFillFormat = this.GetFillFormatFromStyleList(styleList2);
                    break;
                  }
                  break;
                }
              }
              else
                break;
            }
          }
          if (defaultFillFormat == null || defaultFillFormat.FillType == FillType.Automatic)
          {
            MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
            if (this._paragraph.BaseShape.PlaceholderFormat == null)
            {
              if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                styleName = "Content";
              defaultFillFormat = this.GetFillFormatFromStyleList(Helper.GetTextStyle(masterSlide, styleName).StyleList);
            }
            else
            {
              PlaceholderType placeholderType = this._paragraph.BaseShape.GetPlaceholder().GetPlaceholderType();
              switch (placeholderType)
              {
                case PlaceholderType.Title:
                case PlaceholderType.Body:
                case PlaceholderType.CenterTitle:
                case PlaceholderType.Subtitle:
                case PlaceholderType.SlideNumber:
                case PlaceholderType.Header:
                case PlaceholderType.Footer:
                case PlaceholderType.Date:
                  foreach (Shape shape in (IEnumerable<ISlideItem>) masterSlide.Shapes)
                  {
                    if (shape.PlaceholderFormat != null && Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
                    {
                      Dictionary<string, Paragraph> styleList3 = ((TextBody) shape.TextBody).StyleList;
                      if (styleList3 != null)
                      {
                        defaultFillFormat = this.GetFillFormatFromStyleList(styleList3);
                        break;
                      }
                      break;
                    }
                  }
                  if (!Helper.IsHeaderFooterShape(placeholderType) && (defaultFillFormat == null || defaultFillFormat.FillType == FillType.Automatic))
                  {
                    if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                      styleName = "Content";
                    defaultFillFormat = this.GetFillFormatFromStyleList(Helper.GetTextStyle(masterSlide, styleName).StyleList);
                    break;
                  }
                  break;
                default:
                  if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                    styleName = "Content";
                  defaultFillFormat = this.GetFillFormatFromStyleList(Helper.GetTextStyle(masterSlide, styleName).StyleList);
                  break;
              }
            }
          }
          if (defaultFillFormat == null)
          {
            SolidFill solidFill = (SolidFill) this._fillFormat.SolidFill;
            this._fillFormat.SolidFill.Color = (this._fillFormat.Parent as Font).GetDefaultColor();
            return (IFill) this._fillFormat;
          }
          if (defaultFillFormat.FillType == FillType.Automatic)
          {
            if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
              styleName = "Content";
            return this.GetFillFormatFromStyleList(Helper.GetTextStyle((MasterSlide) layoutSlide.MasterSlide, styleName).StyleList);
          }
        }
      }
      else if (defaultFillFormat == null && this._paragraph.BaseSlide is NotesSlide)
        defaultFillFormat = this.GetFillFormatFromStyleList(this._paragraph.BaseSlide.Presentation.NotesMaster.NotesTextStyle.StyleList);
    }
    if (defaultFillFormat == null)
      return (IFill) this._fillFormat;
    if (defaultFillFormat.FillType == FillType.Automatic)
      defaultFillFormat.FillType = FillType.None;
    return defaultFillFormat;
  }

  internal IFill GetFillFormatFromStyleList(Dictionary<string, Paragraph> styleList)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        return style.GetFontObject() == null ? (IFill) null : (IFill) style.GetFontObject()._fillFormat;
      }
    }
    return (IFill) null;
  }

  private string DefaultNameFromNotesMaster(FontScriptType scriptType, string lang)
  {
    string str = "";
    if (this._paragraph.BaseShape.BaseSlide.Presentation.NotesMaster.NotesTextStyle != null)
    {
      Dictionary<string, Paragraph> styleList = this._paragraph.BaseShape.BaseSlide.Presentation.NotesMaster.NotesTextStyle.StyleList;
      if (styleList != null && styleList.Count != 0)
      {
        string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
        if (styleList.ContainsKey(key))
        {
          Paragraph paragraph = styleList[key];
          if (paragraph.GetFontObject() != null)
            str = paragraph.GetFontObject().GetDefaultFontName(scriptType, lang);
        }
      }
    }
    return str;
  }

  private string DefaultNameFromPresentation(FontScriptType scriptType, string lang)
  {
    string str = "";
    if (this._paragraph.BaseShape.BaseSlide.Presentation.DefaultTextStyle != null)
    {
      Dictionary<string, Paragraph> styleList = this._paragraph.BaseShape.BaseSlide.Presentation.DefaultTextStyle.StyleList;
      if (styleList != null && styleList.Count != 0)
      {
        string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
        if (styleList.ContainsKey(key))
        {
          Paragraph paragraph = styleList[key];
          if (paragraph.GetFontObject() != null)
            str = paragraph.GetFontObject().GetDefaultFontName(scriptType, lang);
        }
      }
    }
    return str;
  }

  private float? DefaultCharSpacingFromPresentation()
  {
    return this._paragraph.BaseShape.BaseSlide.Presentation.DefaultTextStyle != null ? this.GetCharSpacingFromStyleList(this._paragraph.BaseShape.BaseSlide.Presentation.DefaultTextStyle.StyleList) : new float?();
  }

  private string DefaultLanguageFromPresentation()
  {
    return this._paragraph.BaseShape.BaseSlide.Presentation.DefaultTextStyle != null ? this.GetLanguageFromStyleList(this._paragraph.BaseShape.BaseSlide.Presentation.DefaultTextStyle.StyleList) : this.Language;
  }

  private string GetNameFromTableStyle(FontScriptType scriptType, string lang)
  {
    string nameFromTableStyle = "";
    Table baseShape = (Table) this._paragraph.BaseShape;
    TableStyle tableStyle = baseShape.TableStyle;
    Cell cell = this._paragraph.Parent.TextBody.Cell;
    Dictionary<string, Paragraph> styleList = ((TextBody) cell.TextBody).StyleList;
    if (styleList != null)
    {
      string nameFromStyleList = this.GetNameFromStyleList(styleList, scriptType, lang);
      if (nameFromStyleList != "")
        return nameFromStyleList;
    }
    if (tableStyle != null)
    {
      if (cell.RowIndex == 1)
      {
        if (baseShape.HasHeaderRow && tableStyle.FirstRow != null && tableStyle.FirstRow.TableTextStyle != null)
          nameFromTableStyle = tableStyle.FirstRow.TableTextStyle.Latin;
      }
      else if (cell.RowIndex == baseShape.Rows.Count && baseShape.HasTotalRow && tableStyle.LastRow != null && tableStyle.LastRow.TableTextStyle != null)
        nameFromTableStyle = tableStyle.LastRow.TableTextStyle.Latin;
      if (cell.ColumnIndex == 1)
      {
        if (baseShape.HasFirstColumn && tableStyle.FirstColumn != null && tableStyle.FirstColumn.TableTextStyle != null)
          nameFromTableStyle = tableStyle.FirstColumn.TableTextStyle.Latin;
      }
      else if (cell.ColumnIndex == baseShape.Columns.Count && baseShape.HasLastColumn && tableStyle.LastColumn != null && tableStyle.LastRow != null && tableStyle.LastRow.TableTextStyle != null)
        nameFromTableStyle = tableStyle.LastColumn.TableTextStyle.Latin;
      if (cell.RowIndex != 1 && baseShape.HasBandedRows && baseShape.HasHeaderRow && tableStyle.HorizontalBand1Style != null && tableStyle.HorizontalBand1Style.TableTextStyle != null)
        nameFromTableStyle = tableStyle.HorizontalBand1Style.TableTextStyle.Latin;
      if (string.IsNullOrEmpty(nameFromTableStyle))
        nameFromTableStyle = tableStyle.WholeTableStyle.TableTextStyle.Latin;
    }
    return nameFromTableStyle;
  }

  private float DefaultCharSpacing()
  {
    float? nullable = new float?();
    Dictionary<string, Paragraph> styleList1 = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (styleList1 != null)
      nullable = this.GetCharSpacingFromStyleList(styleList1);
    if (!nullable.HasValue)
    {
      string styleName = this.SetTempName(Helper.GetNameFromPlaceholder(this._paragraph.BaseShape.ShapeName));
      if (this._paragraph.BaseSlide is Slide)
      {
        Slide baseSlide = (Slide) this._paragraph.BaseSlide;
        string layoutIndex = baseSlide.ObtainLayoutIndex();
        if (layoutIndex != null)
        {
          LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
          foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
          {
            if (shape.PlaceholderFormat != null)
            {
              if (this._paragraph.BaseShape.PlaceholderFormat != null)
              {
                if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat))
                {
                  Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                  if (styleList2 != null)
                  {
                    nullable = this.GetCharSpacingFromStyleList(styleList2);
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
            if (this._paragraph.BaseShape.PlaceholderFormat == null)
            {
              if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                styleName = "Content";
              nullable = this.GetCharSpacingFromStyleList(Helper.GetTextStyle(masterSlide, styleName).StyleList);
            }
            else
            {
              switch (this._paragraph.BaseShape.GetPlaceholder().GetPlaceholderType())
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
                      if (current.PlaceholderFormat != null && Helper.CheckPlaceholder(current.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
                      {
                        Dictionary<string, Paragraph> styleList3 = ((TextBody) current.TextBody).StyleList;
                        if (styleList3 != null)
                        {
                          nullable = this.GetCharSpacingFromStyleList(styleList3);
                          break;
                        }
                        break;
                      }
                    }
                    break;
                  }
                default:
                  if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                    styleName = "Content";
                  nullable = this.GetCharSpacingFromStyleList(Helper.GetTextStyle(masterSlide, styleName).StyleList);
                  break;
              }
            }
          }
        }
      }
      else if (this._paragraph.BaseSlide is LayoutSlide)
      {
        LayoutSlide baseSlide = this._paragraph.BaseSlide as LayoutSlide;
        foreach (Shape shape in (IEnumerable<ISlideItem>) baseSlide.Shapes)
        {
          if (shape.PlaceholderFormat != null)
          {
            if (this._paragraph.BaseShape.PlaceholderFormat != null)
            {
              if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat))
              {
                Dictionary<string, Paragraph> styleList4 = ((TextBody) shape.TextBody).StyleList;
                if (styleList4 != null)
                {
                  nullable = this.GetCharSpacingFromStyleList(styleList4);
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
          if (this._paragraph.BaseShape.PlaceholderFormat == null)
          {
            if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
              styleName = "Content";
            nullable = this.GetCharSpacingFromStyleList(Helper.GetTextStyle(masterSlide, styleName).StyleList);
          }
          else
          {
            switch (this._paragraph.BaseShape.GetPlaceholder().GetPlaceholderType())
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
                    if (current.PlaceholderFormat != null && Helper.CheckPlaceholder(current.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
                    {
                      Dictionary<string, Paragraph> styleList5 = ((TextBody) current.TextBody).StyleList;
                      if (styleList5 != null)
                      {
                        nullable = this.GetCharSpacingFromStyleList(styleList5);
                        break;
                      }
                      break;
                    }
                  }
                  break;
                }
              default:
                if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                  styleName = "Content";
                nullable = this.GetCharSpacingFromStyleList(Helper.GetTextStyle(masterSlide, styleName).StyleList);
                break;
            }
          }
        }
      }
      else if (this._paragraph.BaseSlide is MasterSlide)
      {
        MasterSlide baseSlide = this._paragraph.BaseSlide as MasterSlide;
        if (this._paragraph.BaseShape.PlaceholderFormat == null)
        {
          if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
            styleName = "Content";
          nullable = this.GetCharSpacingFromStyleList(Helper.GetTextStyle(baseSlide, styleName).StyleList);
        }
        else
        {
          switch (this._paragraph.BaseShape.GetPlaceholder().GetPlaceholderType())
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
                  if (current.PlaceholderFormat != null && Helper.CheckPlaceholder(current.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
                  {
                    Dictionary<string, Paragraph> styleList6 = ((TextBody) current.TextBody).StyleList;
                    if (styleList6 != null)
                    {
                      nullable = this.GetCharSpacingFromStyleList(styleList6);
                      break;
                    }
                    break;
                  }
                }
                break;
              }
            default:
              if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                styleName = "Content";
              nullable = this.GetCharSpacingFromStyleList(Helper.GetTextStyle(baseSlide, styleName).StyleList);
              break;
          }
        }
      }
    }
    return nullable.HasValue ? nullable.Value : 0.0f;
  }

  private string DefaultLanguage()
  {
    string str = (string) null;
    Dictionary<string, Paragraph> styleList1 = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (styleList1 != null)
      str = this.GetLanguageFromStyleList(styleList1);
    if (str == null)
    {
      string styleName = this.SetTempName(Helper.GetNameFromPlaceholder(this._paragraph.BaseShape.ShapeName));
      if (this._paragraph.BaseSlide is Slide)
      {
        Slide baseSlide = (Slide) this._paragraph.BaseSlide;
        string layoutIndex = baseSlide.ObtainLayoutIndex();
        if (layoutIndex != null)
        {
          LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
          foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
          {
            if (shape.PlaceholderFormat != null)
            {
              if (this._paragraph.BaseShape.PlaceholderFormat != null)
              {
                if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat))
                {
                  Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                  if (styleList2 != null)
                  {
                    str = this.GetLanguageFromStyleList(styleList2);
                    break;
                  }
                  break;
                }
              }
              else
                break;
            }
          }
          if (str == null)
          {
            MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
            if (this._paragraph.BaseShape.PlaceholderFormat == null)
            {
              if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                styleName = "Content";
              str = this.GetLanguageFromStyleList(Helper.GetTextStyle(masterSlide, styleName).StyleList);
            }
            else
            {
              switch (this._paragraph.BaseShape.GetPlaceholder().GetPlaceholderType())
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
                      if (current.PlaceholderFormat != null && Helper.CheckPlaceholder(current.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
                      {
                        Dictionary<string, Paragraph> styleList3 = ((TextBody) current.TextBody).StyleList;
                        if (styleList3 != null)
                        {
                          str = this.GetLanguageFromStyleList(styleList3);
                          break;
                        }
                        break;
                      }
                    }
                    break;
                  }
                default:
                  if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                    styleName = "Content";
                  str = this.GetLanguageFromStyleList(Helper.GetTextStyle(masterSlide, styleName).StyleList);
                  break;
              }
            }
          }
        }
      }
      else if (this._paragraph.BaseSlide is LayoutSlide)
      {
        LayoutSlide baseSlide = this._paragraph.BaseSlide as LayoutSlide;
        foreach (Shape shape in (IEnumerable<ISlideItem>) baseSlide.Shapes)
        {
          if (shape.PlaceholderFormat != null)
          {
            if (this._paragraph.BaseShape.PlaceholderFormat != null)
            {
              if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat))
              {
                Dictionary<string, Paragraph> styleList4 = ((TextBody) shape.TextBody).StyleList;
                if (styleList4 != null)
                {
                  str = this.GetLanguageFromStyleList(styleList4);
                  break;
                }
                break;
              }
            }
            else
              break;
          }
        }
        if (str == null)
        {
          MasterSlide masterSlide = (MasterSlide) baseSlide.MasterSlide;
          if (this._paragraph.BaseShape.PlaceholderFormat == null)
          {
            if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
              styleName = "Content";
            str = this.GetLanguageFromStyleList(Helper.GetTextStyle(masterSlide, styleName).StyleList);
          }
          else
          {
            switch (this._paragraph.BaseShape.GetPlaceholder().GetPlaceholderType())
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
                    if (current.PlaceholderFormat != null && Helper.CheckPlaceholder(current.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
                    {
                      Dictionary<string, Paragraph> styleList5 = ((TextBody) current.TextBody).StyleList;
                      if (styleList5 != null)
                      {
                        str = this.GetLanguageFromStyleList(styleList5);
                        break;
                      }
                      break;
                    }
                  }
                  break;
                }
              default:
                if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                  styleName = "Content";
                str = this.GetLanguageFromStyleList(Helper.GetTextStyle(masterSlide, styleName).StyleList);
                break;
            }
          }
        }
      }
      else if (this._paragraph.BaseSlide is MasterSlide)
      {
        MasterSlide baseSlide = this._paragraph.BaseSlide as MasterSlide;
        if (this._paragraph.BaseShape.PlaceholderFormat == null)
        {
          if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
            styleName = "Content";
          str = this.GetLanguageFromStyleList(Helper.GetTextStyle(baseSlide, styleName).StyleList);
        }
        else
        {
          switch (this._paragraph.BaseShape.GetPlaceholder().GetPlaceholderType())
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
                  if (current.PlaceholderFormat != null && Helper.CheckPlaceholder(current.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
                  {
                    Dictionary<string, Paragraph> styleList6 = ((TextBody) current.TextBody).StyleList;
                    if (styleList6 != null)
                    {
                      str = this.GetLanguageFromStyleList(styleList6);
                      break;
                    }
                    break;
                  }
                }
                break;
              }
            default:
              if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                styleName = "Content";
              str = this.GetLanguageFromStyleList(Helper.GetTextStyle(baseSlide, styleName).StyleList);
              break;
          }
        }
      }
    }
    return str ?? this.Language;
  }

  private double DefaultFontSize()
  {
    double tempSize = 0.0;
    Dictionary<string, Paragraph> styleList1 = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    TextBody textBody = (TextBody) this._paragraph.BaseShape.TextBody;
    int fontScale = textBody.GetFontScale();
    if (styleList1 != null)
      tempSize = (double) this.GetSizeFromStyleList(styleList1);
    if (tempSize == 0.0)
    {
      string tempName = this.SetTempName(Helper.GetNameFromPlaceholder(this._paragraph.BaseShape.ShapeName));
      if (this._paragraph.BaseSlide is Slide)
      {
        Slide baseSlide = (Slide) this._paragraph.BaseSlide;
        string layoutIndex = baseSlide.ObtainLayoutIndex();
        if (layoutIndex != null)
          tempSize = this.GetFontSizeFromLayoutSlide((ILayoutSlide) baseSlide.Presentation.GetSlideLayout()[layoutIndex], tempSize, tempName, styleList1, textBody);
      }
      else if (this._paragraph.BaseSlide is NotesSlide)
      {
        foreach (Shape shape in (IEnumerable<ISlideItem>) this._paragraph.BaseSlide.Presentation.NotesMaster.Shapes)
        {
          if (shape.PlaceholderFormat != null)
          {
            if (this._paragraph.BaseShape.PlaceholderFormat != null)
            {
              if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
              {
                Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                if (styleList2 != null)
                {
                  tempSize = (double) this.GetSizeFromStyleList(styleList2);
                  break;
                }
                break;
              }
            }
            else
              break;
          }
        }
        if (tempSize == 0.0)
          tempSize = this.DefaultSizeFromNotesMaster();
      }
      else if (this._paragraph.BaseSlide is LayoutSlide)
        tempSize = this.GetFontSizeFromLayoutSlide((ILayoutSlide) (this._paragraph.BaseSlide as LayoutSlide), tempSize, tempName, styleList1, textBody);
      else if (this._paragraph.BaseSlide is MasterSlide)
        tempSize = this.GetFontSizeFromMasterSlide(this._paragraph.BaseSlide as MasterSlide, tempSize, tempName, styleList1, textBody);
    }
    else
    {
      double num = tempSize;
      if (textBody.IsAutoSize && (double) fontScale != 0.0)
        tempSize = (double) this.ApplyFontScale(num, fontScale);
    }
    return tempSize != 0.0 ? tempSize : 18.0;
  }

  private double GetFontSizeFromLayoutSlide(
    ILayoutSlide layoutSlide,
    double tempSize,
    string tempName,
    Dictionary<string, Paragraph> styleList,
    TextBody currentTextBody)
  {
    int fontScale = currentTextBody.GetFontScale();
    foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
    {
      if (shape.PlaceholderFormat != null)
      {
        if (this._paragraph.BaseShape.PlaceholderFormat != null)
        {
          if (Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat))
          {
            styleList = ((TextBody) shape.TextBody).StyleList;
            if (styleList != null)
            {
              tempSize = (double) this.GetSizeFromStyleList(styleList);
              break;
            }
            break;
          }
        }
        else
          break;
      }
    }
    if (tempSize == 0.0)
    {
      tempSize = this.GetFontSizeFromMasterSlide((MasterSlide) layoutSlide.MasterSlide, tempSize, tempName, styleList, currentTextBody);
    }
    else
    {
      double num = tempSize;
      if (currentTextBody.IsAutoSize && (double) fontScale != 0.0)
        tempSize = (double) this.ApplyFontScale(num, fontScale);
    }
    return tempSize;
  }

  private double GetFontSizeFromMasterSlide(
    MasterSlide masterSlide,
    double tempSize,
    string tempName,
    Dictionary<string, Paragraph> styleList,
    TextBody currentTextBody)
  {
    int fontScale = currentTextBody.GetFontScale();
    if (this._paragraph.BaseShape.PlaceholderFormat == null)
    {
      if (tempName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
        tempName = "Content";
      tempSize = (double) this.GetSizeFromStyleList(Helper.GetTextStyle(masterSlide, tempName).StyleList);
      if (tempSize != 0.0)
      {
        double num = tempSize;
        if (currentTextBody.IsAutoSize && fontScale != 0)
          tempSize = (double) this.ApplyFontScale(num, fontScale);
      }
    }
    else
    {
      PlaceholderType placeholderType = this._paragraph.BaseShape.GetPlaceholder().GetPlaceholderType();
      switch (placeholderType)
      {
        case PlaceholderType.Title:
        case PlaceholderType.Body:
        case PlaceholderType.CenterTitle:
        case PlaceholderType.Subtitle:
        case PlaceholderType.SlideNumber:
        case PlaceholderType.Header:
        case PlaceholderType.Footer:
        case PlaceholderType.Date:
          foreach (Shape shape in (IEnumerable<ISlideItem>) masterSlide.Shapes)
          {
            if (shape.PlaceholderFormat != null && Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
            {
              styleList = ((TextBody) shape.TextBody).StyleList;
              if (styleList != null)
              {
                tempSize = (double) this.GetSizeFromStyleList(styleList);
                break;
              }
              break;
            }
          }
          if (!Helper.IsHeaderFooterShape(placeholderType) && tempSize == 0.0)
          {
            if (tempName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
              tempName = "Content";
            tempSize = (double) this.GetSizeFromStyleList(Helper.GetTextStyle(masterSlide, tempName).StyleList);
            if (tempSize != 0.0)
            {
              double num = tempSize;
              if (currentTextBody.IsAutoSize && (double) fontScale != 0.0)
              {
                tempSize = (double) this.ApplyFontScale(num, fontScale);
                break;
              }
              break;
            }
            break;
          }
          break;
        default:
          if (tempName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
            tempName = "Content";
          tempSize = (double) this.GetSizeFromStyleList(Helper.GetTextStyle(masterSlide, tempName).StyleList);
          if (tempSize != 0.0)
          {
            double num = tempSize;
            if (currentTextBody.IsAutoSize && (double) fontScale != 0.0)
            {
              tempSize = (double) this.ApplyFontScale(num, fontScale);
              break;
            }
            break;
          }
          break;
      }
    }
    return tempSize;
  }

  private double DefaultSizeFromNotesMaster()
  {
    double num = 0.0;
    if (this._paragraph.BaseShape.BaseSlide.Presentation.NotesMaster.NotesTextStyle != null)
    {
      Dictionary<string, Paragraph> styleList = this._paragraph.BaseShape.BaseSlide.Presentation.NotesMaster.NotesTextStyle.StyleList;
      if (styleList != null && styleList.Count != 0)
      {
        string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
        if (styleList.ContainsKey(key))
        {
          Paragraph paragraph = styleList[key];
          if (paragraph.GetFontObject() != null)
            num = (double) paragraph.GetFontObject().GetDefaultSize();
        }
      }
    }
    return num != 0.0 ? num : 18.0;
  }

  internal int ApplyFontScale(double value, int fontScale)
  {
    return (int) Convert.ToUInt16(Math.Round(value * (double) fontScale / 100000.0, MidpointRounding.AwayFromZero));
  }

  private string SetTempName(string tempName)
  {
    if (this._paragraph.BaseShape.DrawingType == DrawingType.PlaceHolder && tempName == "shape")
    {
      switch (this._paragraph.BaseShape.GetPlaceholder().GetPlaceholderType())
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
        default:
          if (this._paragraph.BaseShape.GetPlaceholder().Index == 1U)
          {
            tempName = "Content";
            break;
          }
          break;
      }
    }
    return tempName;
  }

  private string GetNameFromStyleList(
    Dictionary<string, Paragraph> styleList,
    FontScriptType scriptType,
    string lang)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        return style.GetFontObject() == null ? "" : style.GetFontObject().GetDefaultFontName(scriptType, lang);
      }
    }
    return "";
  }

  internal float? GetCharSpacingFromStyleList(Dictionary<string, Paragraph> styleList)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        if (style.GetFontObject() == null)
          return new float?();
        if (style.GetFontObject().IsCharacterSpacingApplied())
          return new float?(style.GetFontObject().CharacterSpacing);
      }
    }
    return new float?();
  }

  internal string GetLanguageFromStyleList(Dictionary<string, Paragraph> styleList)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        return style.GetFontObject() == null ? this.Language : style.GetFontObject().Language;
      }
    }
    return this.Language;
  }

  internal int GetSizeFromStyleList(Dictionary<string, Paragraph> styleList)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        return style.GetFontObject() == null ? 0 : style.GetFontObject()._fontSize / 100;
      }
    }
    return 0;
  }

  public TextStrikethroughType StrikeType
  {
    get
    {
      if ((this._optionsDefault & 4) == 0)
      {
        Font fontObject = this._paragraph.GetFontObject();
        if (fontObject != null)
          return this.GetStrikeThroughTypeFromOptions(fontObject._options);
      }
      return this.GetStrikeThroughTypeFromOptions(this._options);
    }
    set
    {
      this._optionsDefault |= 4;
      this._options &= -13;
      switch (value)
      {
        case TextStrikethroughType.Single:
          this._options |= 4;
          break;
        case TextStrikethroughType.Double:
          this._options |= 8;
          break;
      }
    }
  }

  private TextStrikethroughType GetStrikeThroughTypeFromOptions(int options)
  {
    switch ((options & 12) >> 2)
    {
      case 0:
        return TextStrikethroughType.None;
      case 1:
        return TextStrikethroughType.Single;
      case 2:
        return TextStrikethroughType.Double;
      default:
        return TextStrikethroughType.None;
    }
  }

  public TextUnderlineType Underline
  {
    get
    {
      return (this._optionsDefault & 8) == 0 && this._paragraph.GetFontObject() != null ? (TextUnderlineType) ((this._options & 1984) >> 6) : (TextUnderlineType) ((this._options & 1984) >> 6);
    }
    set
    {
      this._optionsDefault |= 8;
      byte num = (byte) value;
      this._options &= -1985;
      this._options |= (int) num << 6;
    }
  }

  internal TextUnderlineType GetDefaultUnderline()
  {
    return this._textPart != null && this._textPart.Hyperlink != null ? TextUnderlineType.Single : (TextUnderlineType) ((this._options & 1984) >> 6);
  }

  internal TextUnderline GetUnderlineType() => (TextUnderline) ((this._options & 1984) >> 6);

  internal bool? RightToLeft
  {
    get => this._rightToLeft;
    set => this._rightToLeft = value;
  }

  internal string FontNameBidi
  {
    get => this.bidiFontName;
    set => this.bidiFontName = value;
  }

  internal bool IsColorSet
  {
    get => this._isColorSet;
    set => this._isColorSet = value;
  }

  internal bool LastParaTextPart
  {
    get => this._lastParaTextPart;
    set => this._lastParaTextPart = value;
  }

  internal bool IsUnderlineColorSet
  {
    get => this._isUnderlineColorSet;
    set
    {
      this._isUnderlineColorSet = value;
      if (!value)
        return;
      this._underlineColor = new ColorObject(true);
    }
  }

  internal BaseSlide BaseSlide => this._baseSlide;

  internal Paragraph Paragraph => this._paragraph;

  internal bool IsSpellingError
  {
    get => this._isSpellingError;
    set => this._isSpellingError = value;
  }

  internal Dictionary<string, Stream> PreservedElements
  {
    get => this._preservedElements ?? (this._preservedElements = new Dictionary<string, Stream>());
  }

  internal string AltLanguage
  {
    get => this._altLang;
    set => this._altLang = value;
  }

  internal EffectList EffectList
  {
    get => this._effectList;
    set => this._effectList = value;
  }

  internal string Language
  {
    get
    {
      string language = (string) null;
      if (this._isLangSet)
        language = !Enum.IsDefined(typeof (LocaleIDs), (object) (int) this._languageID) ? Helper.GetLanguage(this._languageID) : ((LocaleIDs) this._languageID).ToString().Replace('_', '-');
      return language;
    }
  }

  internal bool IsSizeChanged
  {
    get => this._isSizeChanged;
    set => this._isSizeChanged = value;
  }

  internal float CharacterSpacing
  {
    get => this._characterSpacing.HasValue ? (float) this._characterSpacing.Value / 100f : 0.0f;
    set
    {
      this._characterSpacing = (double) value <= 4000.0 && (double) value >= -4000.0 ? new int?(Convert.ToInt32(value * 100f)) : throw new ArgumentException("The Character spacing range must be within -4000 to 4000");
    }
  }

  internal int? Kerning
  {
    get => this._kern;
    set
    {
      int? nullable1 = value;
      if ((nullable1.GetValueOrDefault() >= 0 ? 0 : (nullable1.HasValue ? 1 : 0)) == 0)
      {
        int? nullable2 = value;
        if ((nullable2.GetValueOrDefault() <= 400000 ? 0 : (nullable2.HasValue ? 1 : 0)) == 0)
        {
          this._kern = value;
          return;
        }
      }
      throw new ArgumentException("The value range of kerning must be within 0 to 400000");
    }
  }

  internal string GetName() => this._fontName;

  internal string GetFontName(FontScriptType scriptType)
  {
    if (this._isNameChanged)
      return this._fontName;
    if (Helper.IsEastAsianScript(scriptType))
      return this._eastAsianFont;
    return Helper.IsComplexScript(scriptType) ? this.bidiFontName : this._fontName;
  }

  internal void SetName(string name)
  {
    this._fontName = name;
    this._hasFontName = true;
  }

  internal void SetSymbolFontName(string symbolName) => this._symbolName = symbolName;

  internal string GetSymbolFontName() => this._symbolName;

  internal void SetEastAsianFontName(string eastAsianFont) => this._eastAsianFont = eastAsianFont;

  internal string GetEastAsianFontName() => this._eastAsianFont;

  internal string GetComplexScriptFontName() => this.bidiFontName;

  internal int GetDefaultOptions() => this._optionsDefault;

  internal void SetUnderlineColorObject(ColorObject colorObject)
  {
    this._underlineColor = colorObject;
  }

  internal ColorObject GetUnderlineColorObject() => this._underlineColor;

  internal int GetBaseline() => this._baseLine;

  internal void SetBaseLine(double value)
  {
    this._baseLine = (int) value;
    this._optionsDefault |= 32 /*0x20*/;
  }

  internal ColorObject GetColorObject() => this._colorObject;

  internal void SetColorObject(ColorObject colorObject)
  {
    this._colorObject = colorObject;
    this._isColorChanged = true;
  }

  internal void SetFontSize(int value)
  {
    this._fontSize = value;
    this._isSizeChanged = true;
  }

  internal void Close()
  {
    if (this._colorObject != null)
    {
      this._colorObject.Close();
      this._colorObject = (ColorObject) null;
    }
    if (this._preservedElements != null)
    {
      foreach (KeyValuePair<string, Stream> preservedElement in this._preservedElements)
        preservedElement.Value.Dispose();
      this._preservedElements.Clear();
      this._preservedElements = (Dictionary<string, Stream>) null;
    }
    if (this._effectList != null)
      this._effectList.Close();
    if (this._underlineColor != null)
      this._underlineColor.Close();
    this._baseSlide = (BaseSlide) null;
    this._paragraph = (Paragraph) null;
    this._textPart = (TextPart) null;
  }

  public Font Clone()
  {
    Font font = (Font) this.MemberwiseClone();
    font._colorObject = this._colorObject.CloneColorObject();
    if (this._effectList != null)
      font._effectList = this._effectList.Clone();
    if (this._fillFormat != null)
      font._fillFormat = this._fillFormat.Clone();
    if (this._preservedElements != null)
      font._preservedElements = Helper.CloneDictionary(this._preservedElements);
    return font;
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    if (this._effectList == null)
      return;
    this._effectList.SetParent(presentation);
  }

  internal void SetParent(TextPart textPart) => this._textPart = textPart;

  internal void SetParent(Paragraph paragraph)
  {
    this._paragraph = paragraph;
    this._fillFormat.SetParent((object) this);
  }

  internal void AssignCharacterSpacing(int characterSpacing)
  {
    this._characterSpacing = new int?(characterSpacing);
  }

  internal int ObtainCharacterSpacing()
  {
    return this._characterSpacing.HasValue ? this._characterSpacing.Value : 0;
  }

  internal bool IsCharacterSpacingApplied() => this._characterSpacing.HasValue;
}
