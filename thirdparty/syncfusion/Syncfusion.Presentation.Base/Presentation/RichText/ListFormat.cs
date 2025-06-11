// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.RichText.ListFormat
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Drawing;
using Syncfusion.Office;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Presentation.RichText;

internal class ListFormat : IListFormat
{
  private char _character;
  private string _imageRelationId;
  private Paragraph _paragraph;
  private int _bulletSize;
  private int _startValue;
  private ListType _type;
  private NumListStyle _numberStyle;
  private ListPatternType _patternType;
  private string _numberPrefix;
  private string _numberSufix;
  private SizeType _sizeType;
  private bool _isSizeChanged;
  private byte[] _picStream;
  private Image _imageData;
  private string _fontName;
  private bool _isNameChanged;
  private ColorObject _color;
  private bool _isColorSet;
  private bool _isTextColor;
  private string _picturePath;
  private string _imageExtension;
  private bool _isTextFont;
  private bool _isTextSize;
  private string _numberedString;
  private bool _hasBulletCharacter;
  private bool _isTypeChanged;

  internal ListFormat(Paragraph paragraph)
  {
    this._paragraph = paragraph;
    this._type = ListType.NotDefined;
    this._fontName = "";
    this._character = '•';
    this._color = new ColorObject(true);
  }

  internal bool IsTypeChanged
  {
    get => this._isTypeChanged;
    set => this._isTypeChanged = value;
  }

  internal bool IsTextSize
  {
    get => this._isTextSize;
    set => this._isTextSize = value;
  }

  internal bool HasBulletCharacter
  {
    get => this._hasBulletCharacter;
    set => this._hasBulletCharacter = value;
  }

  internal bool IsTextColor
  {
    get => this._isTextColor;
    set => this._isTextColor = value;
  }

  internal bool IsSizeChanged
  {
    get => this._isSizeChanged;
    set => this._isSizeChanged = value;
  }

  internal bool IsTextFont
  {
    get => this._isTextFont;
    set => this._isTextFont = value;
  }

  public NumberedListStyle NumberStyle
  {
    get
    {
      switch (this._numberStyle)
      {
        case NumListStyle.AlphaLcPeriod:
          return NumberedListStyle.AlphaLcPeriod;
        case NumListStyle.AlphaUcPeriod:
          return NumberedListStyle.AlphaUcPeriod;
        case NumListStyle.ArabicParenRight:
          return NumberedListStyle.ArabicParenRight;
        case NumListStyle.ArabicPeriod:
          return NumberedListStyle.ArabicPeriod;
        case NumListStyle.RomanLcParenBoth:
          return NumberedListStyle.RomanLcParenBoth;
        case NumListStyle.RomanLcParenRight:
          return NumberedListStyle.RomanLcParenRight;
        case NumListStyle.RomanLcPeriod:
          return NumberedListStyle.RomanLcPeriod;
        case NumListStyle.RomanUcPeriod:
          return NumberedListStyle.RomanUcPeriod;
        case NumListStyle.AlphaLcParenBoth:
          return NumberedListStyle.AlphaLcParenBoth;
        case NumListStyle.AlphaLcParenRight:
          return NumberedListStyle.AlphaLcParenRight;
        case NumListStyle.AlphaUcParenBoth:
          return NumberedListStyle.AlphaUcParenBoth;
        case NumListStyle.AlphaUcParenRight:
          return NumberedListStyle.AlphaUcParenRight;
        case NumListStyle.ArabicParenBoth:
          return NumberedListStyle.ArabicParenBoth;
        case NumListStyle.ArabicPlain:
          return NumberedListStyle.ArabicPlain;
        case NumListStyle.RomanUcParenBoth:
          return NumberedListStyle.RomanUcParenBoth;
        case NumListStyle.RomanUcParenRight:
          return NumberedListStyle.RomanUcParenRight;
        case NumListStyle.CircleNumDbPlain:
          return NumberedListStyle.CircleNumDbPlain;
        case NumListStyle.CircleNumWdWhitePlain:
          return NumberedListStyle.CircleNumWdWhitePlain;
        case NumListStyle.CircleNumWdBlackPlain:
          return NumberedListStyle.CircleNumWdBlackPlain;
        case NumListStyle.HebrewAlphaDash:
          return NumberedListStyle.HebrewAlphaDash;
        case NumListStyle.ArabicDbPlain:
          return NumberedListStyle.ArabicDbPlain;
        case NumListStyle.ArabicDbPeriod:
          return NumberedListStyle.ArabicDbPeriod;
        case NumListStyle.ThaiAlphaPeriod:
          return NumberedListStyle.ThaiAlphaPeriod;
        case NumListStyle.ThaiAlphaParenRight:
          return NumberedListStyle.ThaiAlphaParenRight;
        case NumListStyle.ThaiAlphaParenBoth:
          return NumberedListStyle.ThaiAlphaParenBoth;
        case NumListStyle.ThaiNumPeriod:
          return NumberedListStyle.ThaiNumPeriod;
        case NumListStyle.ThaiNumParenRight:
          return NumberedListStyle.ThaiNumParenRight;
        case NumListStyle.ThaiNumParenBoth:
          return NumberedListStyle.ThaiNumParenBoth;
        case NumListStyle.HindiAlphaPeriod:
          return NumberedListStyle.HindiAlphaPeriod;
        case NumListStyle.HindiNumPeriod:
          return NumberedListStyle.HindiNumPeriod;
        case NumListStyle.HindiNumParenRight:
          return NumberedListStyle.HindiNumParenRight;
        case NumListStyle.HindiAlpha1Period:
          return NumberedListStyle.HindiAlpha1Period;
        default:
          return NumberedListStyle.ArabicPlain;
      }
    }
    set
    {
      switch (this._numberStyle = (NumListStyle) Enum.Parse(typeof (NumListStyle), value.ToString(), true))
      {
        case NumListStyle.AlphaLcPeriod:
          this._patternType = ListPatternType.LowLetter;
          this._numberPrefix = "";
          this._numberSufix = ".";
          break;
        case NumListStyle.AlphaUcPeriod:
          this._patternType = ListPatternType.UpLetter;
          this._numberPrefix = "";
          this._numberSufix = ".";
          break;
        case NumListStyle.ArabicParenRight:
          this._patternType = ListPatternType.Arabic;
          this._numberPrefix = "";
          this._numberSufix = ")";
          break;
        case NumListStyle.ArabicPeriod:
          this._patternType = ListPatternType.Arabic;
          this._numberPrefix = "";
          this._numberSufix = ".";
          break;
        case NumListStyle.RomanLcParenBoth:
          this._patternType = ListPatternType.LowRoman;
          this._numberPrefix = "(";
          this._numberSufix = ")";
          break;
        case NumListStyle.RomanLcParenRight:
          this._patternType = ListPatternType.LowRoman;
          this._numberPrefix = "";
          this._numberSufix = ")";
          break;
        case NumListStyle.RomanLcPeriod:
          this._patternType = ListPatternType.LowRoman;
          this._numberPrefix = "";
          this._numberSufix = ".";
          break;
        case NumListStyle.RomanUcPeriod:
          this._patternType = ListPatternType.UpRoman;
          this._numberPrefix = "";
          this._numberSufix = ".";
          break;
        case NumListStyle.AlphaLcParenBoth:
          this._patternType = ListPatternType.LowLetter;
          this._numberPrefix = "(";
          this._numberSufix = ")";
          break;
        case NumListStyle.AlphaLcParenRight:
          this._patternType = ListPatternType.LowLetter;
          this._numberPrefix = "";
          this._numberSufix = ")";
          break;
        case NumListStyle.AlphaUcParenBoth:
          this._patternType = ListPatternType.UpLetter;
          this._numberPrefix = "(";
          this._numberSufix = ")";
          break;
        case NumListStyle.AlphaUcParenRight:
          this._patternType = ListPatternType.UpLetter;
          this._numberPrefix = "";
          this._numberSufix = ")";
          break;
        case NumListStyle.ArabicParenBoth:
          this._patternType = ListPatternType.Arabic;
          this._numberPrefix = "(";
          this._numberSufix = ")";
          break;
        case NumListStyle.ArabicPlain:
          this._patternType = ListPatternType.Arabic;
          this._numberPrefix = "";
          this._numberSufix = "";
          break;
        case NumListStyle.RomanUcParenBoth:
          this._patternType = ListPatternType.UpRoman;
          this._numberPrefix = "(";
          this._numberSufix = ")";
          break;
        case NumListStyle.RomanUcParenRight:
          this._patternType = ListPatternType.UpRoman;
          this._numberPrefix = "";
          this._numberSufix = ")";
          break;
        case NumListStyle.ThaiNumParenBoth:
          this._patternType = ListPatternType.UpRoman;
          this._numberPrefix = "";
          this._numberSufix = ".";
          break;
      }
    }
  }

  internal bool IsNameChanged
  {
    get => this._isNameChanged;
    set => this._isNameChanged = value;
  }

  internal string PicturePath
  {
    get => this._picturePath;
    set => this._picturePath = value;
  }

  public string FontName
  {
    get => this._fontName;
    set
    {
      this._fontName = value;
      this._isNameChanged = true;
    }
  }

  internal string GetDefaultBulletFontName()
  {
    if (!this._isNameChanged && !this._paragraph.IsWithinList)
      return this.DefaultBulletFontName();
    if (this._type != ListType.Numbered && this._fontName != "")
    {
      if (this._paragraph.BaseShape != null)
        return Syncfusion.Presentation.Drawing.Helper.GetFontNameFromTheme(this._paragraph.BaseSlide.BaseTheme, this._fontName, FontScriptType.English, (string) null);
    }
    else if (this._paragraph.TextParts.Count != 0)
      return ((Font) this._paragraph.TextParts[0].Font).GetDefaultFontName(FontScriptType.English, (string) null);
    return Syncfusion.Presentation.Drawing.Helper.GetFontNameFromTheme((this._paragraph.BaseSlide != null ? this._paragraph.BaseSlide.Presentation : this._paragraph.GetTextBody().Presentation).Theme, this._fontName, FontScriptType.English, (string) null);
  }

  private string DefaultBulletFontName()
  {
    if (this._isTextFont)
      return ((Font) this._paragraph.TextParts[0].Font).GetDefaultFontName(FontScriptType.English, (string) null);
    string str = "";
    Dictionary<string, Paragraph> styleList1 = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (str == "" && styleList1 != null)
      str = this.GetFontFromStyleList(styleList1);
    if (str == "")
    {
      if (this._paragraph.BaseShape.DrawingType == DrawingType.PlaceHolder)
      {
        string styleName = this.SetTempName(Syncfusion.Presentation.Drawing.Helper.GetNameFromPlaceholder(this._paragraph.BaseShape.ShapeName));
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
                  if (Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat))
                  {
                    Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                    if (styleList2 != null)
                    {
                      str = this.GetFontFromStyleList(styleList2);
                      break;
                    }
                    break;
                  }
                }
                else
                  break;
              }
            }
            if (str == "" && this.Type != ListType.Numbered)
            {
              MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
              if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                styleName = "Content";
              if (this._paragraph.BaseShape.PlaceholderFormat == null)
              {
                str = this.GetFontFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList);
              }
              else
              {
                switch (this._paragraph.BaseShape.GetPlaceholder().GetPlaceholderType())
                {
                  case PlaceholderType.Title:
                  case PlaceholderType.Body:
                  case PlaceholderType.CenterTitle:
                  case PlaceholderType.Subtitle:
                    foreach (Shape shape in (IEnumerable<ISlideItem>) masterSlide.Shapes)
                    {
                      if (shape.PlaceholderFormat != null && Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
                      {
                        Dictionary<string, Paragraph> styleList3 = ((TextBody) shape.TextBody).StyleList;
                        if (styleList3 != null)
                        {
                          str = this.GetFontFromStyleList(styleList3);
                          break;
                        }
                        break;
                      }
                    }
                    if (str == "")
                    {
                      str = this.GetFontFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList);
                      break;
                    }
                    break;
                  default:
                    str = this.GetFontFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList);
                    break;
                }
              }
            }
            else if (this.Type == ListType.Numbered)
              str = ((Font) this._paragraph.TextParts[0].Font).GetDefaultFontName(FontScriptType.English, (string) null);
          }
        }
      }
      else
        str = this.GetFontFromStyleList(this._paragraph.BaseSlide.Presentation.DefaultTextStyle.StyleList);
    }
    return !(str == "") ? str : ((Font) this._paragraph.TextParts[0].Font).GetDefaultFontName(FontScriptType.English, (string) null);
  }

  internal string GetFontFromStyleList(Dictionary<string, Paragraph> styleList)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
        return ((ListFormat) styleList[key].ListFormat).FontName;
    }
    return "";
  }

  public byte[] Data
  {
    get
    {
      return this._picStream != null && this._picStream.Length > 0 ? (byte[]) this._picStream.Clone() : (byte[]) null;
    }
    set
    {
      if (value == null || value.Length <= 0)
        return;
      this.AddImageStream((Stream) new MemoryStream(value));
    }
  }

  internal void AddImageStream()
  {
    RelationCollection topRelation = this._paragraph.BaseSlide.TopRelation;
    if (topRelation == null)
      return;
    string itemPathByRelation = topRelation.GetItemPathByRelation(this._imageRelationId);
    this._picturePath = itemPathByRelation;
    if (itemPathByRelation == null)
      return;
    string[] strArray = itemPathByRelation.Split('/');
    Stream imageStream = this._paragraph.BaseSlide.Presentation.DataHolder.GetImageStream($"ppt/{strArray[strArray.Length - 2]}{(object) '/'}{strArray[strArray.Length - 1]}");
    if (imageStream != null && imageStream.Length > 0L)
      this.AddImageStream(imageStream);
    topRelation.GetImageRemoveList().Add(this._imageRelationId);
  }

  private void GetImageFormat()
  {
    byte[] data = this.Data;
    if (data == null || data.Length <= 0)
      return;
    Image image = Image.FromStream((Stream) new MemoryStream(data, false));
    ImageFormat rawFormat = image.RawFormat;
    image.Close();
    if (rawFormat.Equals((object) ImageFormat.Jpeg))
      this._imageExtension = ".jpeg";
    else if (rawFormat.Equals((object) ImageFormat.Bmp))
      this._imageExtension = ".bmp";
    else if (rawFormat.Equals((object) ImageFormat.Png))
      this._imageExtension = ".png";
    else if (rawFormat.Equals((object) ImageFormat.Emf))
      this._imageExtension = ".emf";
    else if (rawFormat.Equals((object) ImageFormat.Exif))
      this._imageExtension = ".exif";
    else if (rawFormat.Equals((object) ImageFormat.Gif))
      this._imageExtension = ".gif";
    else if (rawFormat.Equals((object) ImageFormat.Icon))
      this._imageExtension = ".ico";
    else if (rawFormat.Equals((object) ImageFormat.MemoryBmp))
      this._imageExtension = ".bmp";
    else if (rawFormat.Equals((object) ImageFormat.Tiff))
      this._imageExtension = ".tiff";
    else
      this._imageExtension = ".wmf";
  }

  internal void AddImageToArchive()
  {
    this.GetImageFormat();
    Relation relation;
    if (this._imageRelationId == null && this._picturePath == null)
    {
      string relationId = Syncfusion.Presentation.Drawing.Helper.GenerateRelationId(this._paragraph.BaseSlide.TopRelation);
      relation = new Relation(relationId, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", $"../media/image{(object) ++this._paragraph.BaseSlide.Presentation.ImageCount}{this._imageExtension}", (string) null);
      this.ImageRelationId = relationId;
      this._paragraph.BaseSlide.Presentation.DataHolder.AddImageToArchive($"ppt/media/image{(object) this._paragraph.BaseSlide.Presentation.ImageCount}{this._imageExtension}", new MemoryStream(this._picStream));
    }
    else
      relation = new Relation(this._imageRelationId, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/image", this._picturePath, (string) null);
    this._paragraph.BaseSlide.TopRelation.Add(relation.Id, relation);
    string partName = this._imageExtension.Replace(".", "");
    this._paragraph.BaseSlide.Presentation.AddDefaultContentType(partName, "image/" + partName);
  }

  private void AddImageStream(Stream input)
  {
    MemoryStream output = new MemoryStream();
    ListFormat.CopyStream(input, (Stream) output);
    this._picStream = output.ToArray();
  }

  private static void CopyStream(Stream input, Stream output)
  {
    if (input == null)
      throw new ArgumentNullException(nameof (input));
    if (output == null)
      throw new ArgumentNullException(nameof (output));
    input.Position = 0L;
    input.CopyTo(output);
  }

  internal Image Image
  {
    get
    {
      if (this._imageData == null && this._picStream != null)
        this._imageData = Image.FromStream((Stream) new MemoryStream(this._picStream, false));
      return this._imageData;
    }
    set => this._imageData = value;
  }

  public char BulletCharacter
  {
    get => this._character;
    set => this._character = value;
  }

  internal string GetDefaultBulletCharacter()
  {
    switch (this._type)
    {
      case ListType.NotDefined:
        if (!this._paragraph.IsWithinList)
          return this.DefaultBulletCharacter();
        break;
      case ListType.Numbered:
        return this.GetNumberedValue();
    }
    return this._character.ToString();
  }

  private string DefaultBulletCharacter()
  {
    if (this.Type == ListType.Numbered)
      return this.GetNumberedValue();
    string str = "";
    Dictionary<string, Paragraph> styleList1 = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (styleList1 != null)
      str = this.GetCharFromStyleList(styleList1);
    if (str == "" && this._paragraph.BaseShape.DrawingType != DrawingType.PlaceHolder)
      return this.GetCharFromStyleList(this._paragraph.BaseSlide.Presentation.DefaultTextStyle.StyleList);
    if (str == "")
    {
      string styleName = this.SetTempName(Syncfusion.Presentation.Drawing.Helper.GetNameFromPlaceholder(this._paragraph.BaseShape.ShapeName));
      if (this._paragraph.BaseSlide is Slide)
      {
        Slide baseSlide = (Slide) this._paragraph.BaseSlide;
        string layoutIndex = baseSlide.ObtainLayoutIndex();
        if (layoutIndex != null)
        {
          LayoutSlide layoutSlide = baseSlide.Presentation.GetSlideLayout()[layoutIndex];
          foreach (Shape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
          {
            if ((shape.PlaceholderFormat != null || this._paragraph.BaseShape.PlaceholderFormat == null) && Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat))
            {
              Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
              if (styleList2 != null)
              {
                switch (this.GetTypeFromStyleList(styleList2))
                {
                  case ListType.NotDefined:
                  case ListType.None:
                    goto label_19;
                  default:
                    str = this.GetCharFromStyleList(styleList2);
                    goto label_19;
                }
              }
              else
                break;
            }
          }
label_19:
          if (str == "")
          {
            MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
            if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
              styleName = "Content";
            str = this.GetCharFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList);
          }
        }
      }
    }
    return str;
  }

  private string GetCharFromStyleList(Dictionary<string, Paragraph> styleList)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
        return ((ListFormat) styleList[key].ListFormat).GetDefaultBulletCharacter();
    }
    return "";
  }

  private string GetNumberedValue()
  {
    if (this._character == '•')
      this._character = '0';
    string str = this._numberedString == null ? "0" : int.Parse(this._numberedString, NumberStyles.Any).ToString();
    int listItemIndex = 0;
    if (this.StartValue != 0)
      listItemIndex = Convert.ToInt32(str) + this.StartValue - 2;
    else if (str != "")
      listItemIndex = Convert.ToInt32(str) - 1;
    return this.GetNumBulletCharacter(listItemIndex);
  }

  public IColor Color
  {
    get
    {
      this._color.UpdateColorObject(this._paragraph.BaseSlide != null ? (object) this._paragraph.BaseSlide.Presentation : (object) this._paragraph.GetTextBody().Presentation);
      return (IColor) this._color;
    }
    set
    {
      this._color.SetColor(ColorType.AutomaticIndex, ((ColorObject) value).ToArgb());
      this._isColorSet = true;
    }
  }

  internal IColor GetDefaultBulletColor()
  {
    if (!this._isColorSet && !this._paragraph.IsWithinList)
      return this.DefaultBulletColor();
    this._color.UpdateColorObject(this._paragraph.BaseSlide != null ? (object) this._paragraph.BaseSlide.Presentation : (object) this._paragraph.GetTextBody().Presentation);
    return (IColor) this._color;
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
      }
      if (tempName == "shape")
      {
        if (this._paragraph.BaseShape.GetPlaceholder().Index != 0U)
          tempName = "Subtitle";
        switch (this._paragraph.BaseShape.GetPlaceholder().Size)
        {
          case PlaceholderSize.Half:
          case PlaceholderSize.Quarter:
          case PlaceholderSize.Full:
            tempName = "Subtitle";
            break;
        }
      }
    }
    return tempName;
  }

  private IColor DefaultBulletColor()
  {
    IColor color = (IColor) null;
    bool isColorSet = false;
    Dictionary<string, Paragraph> styleList1 = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (styleList1 != null)
      color = this.GetColorFromStyleList(styleList1, ref isColorSet);
    if (this._paragraph.BaseShape.DrawingType == DrawingType.PlaceHolder)
    {
      if (color == null || !isColorSet)
      {
        string styleName = this.SetTempName(Syncfusion.Presentation.Drawing.Helper.GetNameFromPlaceholder(this._paragraph.BaseShape.ShapeName));
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
                  if (Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat))
                  {
                    Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                    if (styleList2 != null)
                    {
                      color = this.GetColorFromStyleList(styleList2, ref isColorSet);
                      break;
                    }
                    break;
                  }
                }
                else
                  break;
              }
            }
            if (!isColorSet || color == null)
            {
              MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
              if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                styleName = "Content";
              if (this._paragraph.BaseShape.PlaceholderFormat == null)
              {
                color = this.GetColorFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList, ref isColorSet);
              }
              else
              {
                switch (this._paragraph.BaseShape.GetPlaceholder().GetPlaceholderType())
                {
                  case PlaceholderType.Title:
                  case PlaceholderType.Body:
                  case PlaceholderType.CenterTitle:
                  case PlaceholderType.Subtitle:
                    foreach (Shape shape in (IEnumerable<ISlideItem>) masterSlide.Shapes)
                    {
                      if (shape.PlaceholderFormat != null && Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
                      {
                        Dictionary<string, Paragraph> styleList3 = ((TextBody) shape.TextBody).StyleList;
                        if (styleList3 != null)
                        {
                          color = this.GetColorFromStyleList(styleList3, ref isColorSet);
                          break;
                        }
                        break;
                      }
                    }
                    if (color == null || !isColorSet)
                    {
                      color = this.GetColorFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList, ref isColorSet);
                      break;
                    }
                    break;
                  default:
                    color = this.GetColorFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList, ref isColorSet);
                    break;
                }
              }
            }
          }
        }
        if (this._isTextColor || !isColorSet)
          return ((Font) this._paragraph.TextParts[0].Font).GetDefaultColor();
      }
    }
    else if (!isColorSet)
      color = this.GetColorFromStyleList(this._paragraph.BaseSlide.Presentation.DefaultTextStyle.StyleList, ref isColorSet);
    if (color == null || !isColorSet)
      color = ((Font) this._paragraph.TextParts[0].Font).GetDefaultColor();
    return color;
  }

  private IColor GetColorFromStyleList(Dictionary<string, Paragraph> styleList, ref bool isColorSet)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        IColor defaultBulletColor = ((ListFormat) style.ListFormat).GetDefaultBulletColor();
        isColorSet = ((ListFormat) style.ListFormat)._isColorSet;
        return defaultBulletColor;
      }
    }
    return (IColor) null;
  }

  internal bool IsColorSet
  {
    get => this._isColorSet;
    set => this._isColorSet = value;
  }

  internal string ImageRelationId
  {
    get => this._imageRelationId;
    set
    {
      if (value == null)
        return;
      this._imageRelationId = value;
    }
  }

  public float Size
  {
    get
    {
      switch (this._sizeType)
      {
        case SizeType.Points:
          return (float) this._bulletSize / 100f;
        case SizeType.Percentage:
          return (float) this._bulletSize / 1000f;
        default:
          return (float) this._bulletSize;
      }
    }
    set
    {
      if ((double) value > 400.0 || (double) value < 25.0)
        throw new ArgumentException("Invalid Size " + value.ToString());
      this._sizeType = SizeType.Percentage;
      this._bulletSize = Convert.ToInt32(value * 1000f);
    }
  }

  private string GetPicturePathFromHierarchy()
  {
    string pathFromHierarchy = (string) null;
    Dictionary<string, Paragraph> styleList1 = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (styleList1 != null)
      pathFromHierarchy = this.GetPicturePathFromStyleList(styleList1);
    if (pathFromHierarchy == null && this._paragraph.BaseShape.DrawingType == DrawingType.PlaceHolder)
    {
      if (this._paragraph.BaseShape.ShapeName.Contains("TextBox"))
        return (string) null;
      string str = Syncfusion.Presentation.Drawing.Helper.GetNameFromPlaceholder(this._paragraph.BaseShape.ShapeName);
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
                if (Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat))
                {
                  Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                  if (styleList2 != null)
                  {
                    pathFromHierarchy = this.GetPicturePathFromStyleList(styleList2);
                    break;
                  }
                  break;
                }
              }
              else
                break;
            }
          }
          if (pathFromHierarchy == null)
          {
            MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
            if (this._paragraph.BaseShape.PlaceholderFormat != null)
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
                      if (current.PlaceholderFormat != null && Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(current.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
                      {
                        Dictionary<string, Paragraph> styleList3 = ((TextBody) current.TextBody).StyleList;
                        if (styleList3 != null)
                        {
                          pathFromHierarchy = this.GetPicturePathFromStyleList(styleList3);
                          break;
                        }
                        break;
                      }
                    }
                    break;
                  }
                default:
                  if (str == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                    str = this.SetTempName(str);
                  pathFromHierarchy = this.GetPicturePathFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, str).StyleList);
                  break;
              }
            }
          }
        }
      }
    }
    else if (pathFromHierarchy == null)
      return this.DefaultPicturePathFromPresentation();
    return pathFromHierarchy;
  }

  private Image GetDefaultImage()
  {
    Image defaultImage = (Image) null;
    Dictionary<string, Paragraph> styleList1 = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (styleList1 != null)
      defaultImage = this.GetImageFromStyleList(styleList1);
    if (defaultImage == null && this._paragraph.BaseShape.DrawingType == DrawingType.PlaceHolder)
    {
      if (this._paragraph.BaseShape.ShapeName.Contains("TextBox"))
        return (Image) null;
      string str = Syncfusion.Presentation.Drawing.Helper.GetNameFromPlaceholder(this._paragraph.BaseShape.ShapeName);
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
                if (Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat))
                {
                  Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                  if (styleList2 != null)
                  {
                    defaultImage = this.GetImageFromStyleList(styleList2);
                    break;
                  }
                  break;
                }
              }
              else
                break;
            }
          }
          if (defaultImage == null)
          {
            MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
            if (this._paragraph.BaseShape.PlaceholderFormat != null)
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
                      if (current.PlaceholderFormat != null && Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(current.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
                      {
                        Dictionary<string, Paragraph> styleList3 = ((TextBody) current.TextBody).StyleList;
                        if (styleList3 != null)
                        {
                          defaultImage = this.GetImageFromStyleList(styleList3);
                          break;
                        }
                        break;
                      }
                    }
                    break;
                  }
                default:
                  if (str == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                    str = this.SetTempName(str);
                  defaultImage = this.GetImageFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, str).StyleList);
                  break;
              }
            }
          }
        }
      }
    }
    else if (defaultImage == null)
      return this.DefaultImageFromPresentation();
    return defaultImage;
  }

  private string GetPicturePathFromStyleList(Dictionary<string, Paragraph> styleList)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
        return ((ListFormat) styleList[key].ListFormat).GetDefaultPicturePath();
    }
    return (string) null;
  }

  private Image GetImageFromStyleList(Dictionary<string, Paragraph> styleList)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
        return ((ListFormat) styleList[key].ListFormat).GetBulletImage();
    }
    return (Image) null;
  }

  internal Image GetBulletImage()
  {
    return !this._paragraph.IsWithinList && this.Image == null && !(this._paragraph.BaseShape is ITable) ? this.GetDefaultImage() : this.Image;
  }

  internal string GetDefaultPicturePath()
  {
    return !this._paragraph.IsWithinList && this.PicturePath == null && !(this._paragraph.BaseShape is ITable) ? this.GetPicturePathFromHierarchy() : this.PicturePath;
  }

  internal float GetDefaultBulletSize()
  {
    if (!this._isSizeChanged)
      return this.DefaultBulletSize();
    switch (this._sizeType)
    {
      case SizeType.Points:
        return (float) (this._bulletSize / 100);
      case SizeType.Percentage:
        return (float) ((double) (this._bulletSize / 1000) * (double) ((Font) this._paragraph.TextParts[0].Font).GetDefaultSize() / 100.0);
      default:
        return (float) this._bulletSize;
    }
  }

  private float DefaultBulletSize()
  {
    float num = 0.0f;
    Dictionary<string, Paragraph> styleList1 = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (styleList1 != null)
      num = this.GetSizeFromStyleList(styleList1);
    if (this._isTextSize)
      return this._bulletSize != 0 ? this.SizeFromType(this._sizeType, (float) this._bulletSize) : ((Font) this._paragraph.TextParts[0].Font).GetDefaultSize();
    if ((double) num == 0.0)
    {
      if (this._paragraph.BaseShape.DrawingType == DrawingType.PlaceHolder)
      {
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
                  if (Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat))
                  {
                    Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                    if (styleList2 != null)
                    {
                      num = this.GetSizeFromStyleList(styleList2);
                      if ((double) num == 0.0)
                      {
                        num = this.GetSizeFromStyleList(styleList2, shape);
                        break;
                      }
                      break;
                    }
                    break;
                  }
                }
                else
                  break;
              }
            }
            if ((double) num == 0.0)
            {
              MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
              string styleName = this.SetTempName(Syncfusion.Presentation.Drawing.Helper.GetNameFromPlaceholder(this._paragraph.BaseShape.ShapeName));
              if (styleName == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                styleName = "Content";
              num = this.CheckFontScale(this.GetSizeFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, styleName).StyleList) * 100f, this._paragraph.BaseShape.TextBody);
            }
          }
        }
      }
      else if (this._paragraph.BaseSlide.Presentation.DefaultTextStyle != null)
        num = this.GetSizeFromStyleList(this._paragraph.BaseSlide.Presentation.DefaultTextStyle.StyleList);
    }
    return (double) num != 0.0 ? num : ((Font) this._paragraph.TextParts[0].Font).GetDefaultSize();
  }

  private float CheckFontScale(float tempSize, ITextBody textFrame)
  {
    TextBody textBody = (TextBody) textFrame;
    return textBody.IsAutoSize && !textBody.IsFontScaleAccessed && textBody.GetFontScale() != 0 ? (float) Convert.ToUInt16((double) tempSize / 100.0 * (double) textBody.GetFontScale() / 10000000.0 * 100.0) : tempSize / 100f;
  }

  private float GetSizeFromStyleList(Dictionary<string, Paragraph> styleList, Shape layoutShape)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        ListFormat listFormat = (ListFormat) style.ListFormat;
        if (listFormat._type != ListType.None && listFormat._type != ListType.NotDefined && listFormat._bulletSize == 0)
        {
          if (style.GetFontObject() == null)
            return 0.0f;
          if ((double) style.GetFontObject().GetDefaultSize() * 100.0 == 0.0)
            return this.SizeFromType(SizeType.Points, ((Font) layoutShape.TextBody.Paragraphs[0].TextParts[0].Font).GetDefaultSize() * 100f);
        }
        return this.SizeFromType(listFormat._sizeType, (float) listFormat._bulletSize);
      }
    }
    return 0.0f;
  }

  private float SizeFromType(SizeType sizeType, float bulletSize)
  {
    switch (sizeType)
    {
      case SizeType.Points:
        return bulletSize / 100f;
      case SizeType.Percentage:
        double defaultSize = (double) ((Font) this._paragraph.TextParts[0].Font).GetDefaultSize();
        return (float) (int) Math.Ceiling((double) bulletSize / 1000.0 * defaultSize / 100.0);
      default:
        return 0.0f;
    }
  }

  private float GetSizeFromStyleList(Dictionary<string, Paragraph> styleList)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
      {
        Paragraph style = styleList[key];
        ListFormat listFormat = (ListFormat) style.ListFormat;
        return listFormat._bulletSize == 0 && style.GetFontObject() == null ? 0.0f : this.SizeFromType(listFormat._sizeType, (float) listFormat._bulletSize);
      }
    }
    return 0.0f;
  }

  public int StartValue
  {
    get => this._startValue;
    set
    {
      this._startValue = value >= 1 && value <= (int) short.MaxValue ? value : throw new ArgumentException("Invalid Start Value " + value.ToString());
    }
  }

  internal Paragraph Paragraph => this._paragraph;

  public ListType Type
  {
    get => this._type;
    set
    {
      this._type = value;
      if (this._paragraph.GetTextBody().GetBaseShape() == null || this._paragraph.GetTextBody().GetBaseShape().DrawingType == DrawingType.PlaceHolder)
        return;
      switch (this._type)
      {
        case ListType.Bulleted:
          this._paragraph.SetIndent(283464);
          break;
        case ListType.Numbered:
          this._paragraph.SetIndent(347472);
          break;
      }
    }
  }

  internal ListType GetDefaultListType()
  {
    return !this._paragraph.IsWithinList && this._type == ListType.NotDefined && !(this._paragraph.BaseShape is ITable) ? this.DefaultListType() : this._type;
  }

  private ListType DefaultListType()
  {
    ListType listType = ListType.NotDefined;
    Dictionary<string, Paragraph> styleList1 = ((TextBody) this._paragraph.BaseShape.TextBody).StyleList;
    if (styleList1 != null)
      listType = this.GetTypeFromStyleList(styleList1);
    if (listType == ListType.NotDefined && this._paragraph.BaseShape.DrawingType == DrawingType.PlaceHolder)
    {
      if (this._paragraph.BaseShape.ShapeName.Contains("TextBox"))
        return ListType.None;
      string str = Syncfusion.Presentation.Drawing.Helper.GetNameFromPlaceholder(this._paragraph.BaseShape.ShapeName);
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
                if (Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(shape.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat))
                {
                  Dictionary<string, Paragraph> styleList2 = ((TextBody) shape.TextBody).StyleList;
                  if (styleList2 != null)
                  {
                    listType = this.GetTypeFromStyleList(styleList2);
                    break;
                  }
                  break;
                }
              }
              else
                break;
            }
          }
          if (listType == ListType.NotDefined)
          {
            MasterSlide masterSlide = (MasterSlide) layoutSlide.MasterSlide;
            if (this._paragraph.BaseShape.PlaceholderFormat != null)
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
                      if (current.PlaceholderFormat != null && Syncfusion.Presentation.Drawing.Helper.CheckPlaceholder(current.PlaceholderFormat, this._paragraph.BaseShape.PlaceholderFormat, true))
                      {
                        Dictionary<string, Paragraph> styleList3 = ((TextBody) current.TextBody).StyleList;
                        if (styleList3 != null)
                        {
                          listType = this.GetTypeFromStyleList(styleList3);
                          break;
                        }
                        break;
                      }
                    }
                    break;
                  }
                default:
                  if (str == "shape" && this._paragraph.BaseShape.PlaceholderFormat != null)
                    str = this.SetTempName(str);
                  listType = this.GetTypeFromStyleList(Syncfusion.Presentation.Drawing.Helper.GetTextStyle(masterSlide, str).StyleList);
                  if (listType == ListType.NotDefined)
                  {
                    listType = ListType.None;
                    break;
                  }
                  break;
              }
            }
          }
          if (listType == ListType.NotDefined)
            listType = ListType.None;
        }
      }
    }
    else if (listType == ListType.NotDefined)
      return this.DefaultTypeFromPresentation();
    return listType;
  }

  private string DefaultPicturePathFromPresentation()
  {
    return this._paragraph.BaseSlide.Presentation.DefaultTextStyle == null ? (string) null : this.GetPicturePathFromStyleList(this._paragraph.BaseSlide.Presentation.DefaultTextStyle.StyleList);
  }

  private Image DefaultImageFromPresentation()
  {
    return this._paragraph.BaseSlide.Presentation.DefaultTextStyle == null ? (Image) null : this.GetImageFromStyleList(this._paragraph.BaseSlide.Presentation.DefaultTextStyle.StyleList);
  }

  private ListType DefaultTypeFromPresentation()
  {
    return this._paragraph.BaseSlide.Presentation.DefaultTextStyle == null ? ListType.None : this.GetTypeFromStyleList(this._paragraph.BaseSlide.Presentation.DefaultTextStyle.StyleList);
  }

  private ListType GetTypeFromStyleList(Dictionary<string, Paragraph> styleList)
  {
    if (styleList.Count != 0)
    {
      string key = $"lvl{(object) (this._paragraph.IndentLevelNumber + 1)}pPr";
      if (styleList.ContainsKey(key))
        return ((ListFormat) styleList[key].ListFormat).GetDefaultListType();
    }
    return ListType.NotDefined;
  }

  internal void AssignType(ListType bulletType) => this._type = bulletType;

  internal ListType GetListType() => this._type;

  internal void SetCharacter(string bulletCharacter)
  {
    switch (bulletCharacter)
    {
      case "":
        return;
      case null:
        return;
      case " ":
        if (!this._hasBulletCharacter)
          return;
        break;
    }
    if (this._type == ListType.Numbered)
      this._numberedString = bulletCharacter;
    else
      this._character = Convert.ToChar(bulletCharacter[0]);
  }

  internal string GetCharacter() => this._character.ToString();

  internal ColorObject GetColorObject() => this._color;

  internal string GetBulletFontName() => this._fontName;

  public void Picture(Stream pictureStream) => this.AddImageStream(pictureStream);

  internal void SetBulletSize(int bulletSize, SizeType sizeType)
  {
    this._bulletSize = bulletSize;
    this._sizeType = sizeType;
  }

  internal SizeType GetSizeType() => this._sizeType;

  internal int GetBulletSize() => this._bulletSize;

  internal void SetColorObject(ColorObject colorObject)
  {
    this._color = colorObject;
    this._isColorSet = true;
  }

  private string GetNumBulletCharacter(int listItemIndex)
  {
    switch (this._patternType)
    {
      case ListPatternType.Arabic:
        return this._numberPrefix + (object) (listItemIndex + 1) + this._numberSufix;
      case ListPatternType.UpRoman:
        return this._numberPrefix + this.GetAsRoman(listItemIndex + 1).ToUpper() + this._numberSufix;
      case ListPatternType.LowRoman:
        return this._numberPrefix + this.GetAsRoman(listItemIndex + 1).ToLower() + this._numberSufix;
      case ListPatternType.UpLetter:
        return this._numberPrefix + this.GetAsLetter(listItemIndex + 1).ToUpper() + this._numberSufix;
      case ListPatternType.LowLetter:
        return this._numberPrefix + this.GetAsLetter(listItemIndex + 1).ToLower() + this._numberSufix;
      case ListPatternType.LeadingZero:
        if (listItemIndex >= 9)
          return this._numberPrefix + (object) (listItemIndex + 1) + this._numberSufix;
        return $"{this._numberPrefix}0{(object) (listItemIndex + 1)}{this._numberSufix}";
      case ListPatternType.None:
        return "";
      default:
        return this._numberPrefix + (object) (listItemIndex + 1) + this._numberSufix;
    }
  }

  private string GetAsRoman(int number)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.GenerateNumber(ref number, 1000, "M"));
    stringBuilder.Append(this.GenerateNumber(ref number, 900, "CM"));
    stringBuilder.Append(this.GenerateNumber(ref number, 500, "D"));
    stringBuilder.Append(this.GenerateNumber(ref number, 400, "CD"));
    stringBuilder.Append(this.GenerateNumber(ref number, 100, "C"));
    stringBuilder.Append(this.GenerateNumber(ref number, 90, "XC"));
    stringBuilder.Append(this.GenerateNumber(ref number, 50, "L"));
    stringBuilder.Append(this.GenerateNumber(ref number, 40, "XL"));
    stringBuilder.Append(this.GenerateNumber(ref number, 10, "X"));
    stringBuilder.Append(this.GenerateNumber(ref number, 9, "IX"));
    stringBuilder.Append(this.GenerateNumber(ref number, 5, "V"));
    stringBuilder.Append(this.GenerateNumber(ref number, 4, "IV"));
    stringBuilder.Append(this.GenerateNumber(ref number, 1, "I"));
    return stringBuilder.ToString();
  }

  private string GenerateNumber(ref int value, int magnitude, string letter)
  {
    StringBuilder stringBuilder = new StringBuilder();
    while (value >= magnitude)
    {
      value -= magnitude;
      stringBuilder.Append(letter);
    }
    return stringBuilder.ToString();
  }

  internal string GetAsLetter(int number)
  {
    if (number <= 0)
      return "";
    int num1 = number / 26;
    int num2 = number % 26;
    if (num2 == 0)
    {
      num2 = 26;
      --num1;
    }
    char ch = (char) (64 /*0x40*/ + num2);
    string asLetter = ch.ToString();
    for (; num1 > 0; --num1)
      asLetter += ch.ToString();
    return asLetter;
  }

  internal void Close()
  {
    if (this._imageData != null)
    {
      this._imageData.Close();
      this._imageData = (Image) null;
    }
    if (this._picStream != null)
      this._picStream = (byte[]) null;
    if (this._color != null)
      this._color.Close();
    this._paragraph = (Paragraph) null;
  }

  public ListFormat Clone()
  {
    ListFormat listFormat = (ListFormat) this.MemberwiseClone();
    listFormat._color = this._color.CloneColorObject();
    if (this._imageData != null)
      listFormat._imageData = this._imageData.Clone();
    listFormat._picStream = Syncfusion.Presentation.Drawing.Helper.CloneByteArray(this._picStream);
    return listFormat;
  }

  internal void SetParent(Paragraph paragraph) => this._paragraph = paragraph;
}
