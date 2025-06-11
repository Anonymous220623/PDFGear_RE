// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.RichText.TextPart
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Office;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Globalization;

#nullable disable
namespace Syncfusion.Presentation.RichText;

internal class TextPart : ITextPart
{
  private Syncfusion.Presentation.RichText.Font _font;
  private Syncfusion.Presentation.RichText.Font _lineBreakFont;
  private Paragraph _paragraph;
  private string _text;
  private string _uniqueId;
  private string _type;
  private Syncfusion.Presentation.RichText.Hyperlink _hyperlinkClick;
  private FontScriptType _scriptType;
  private CharacterRangeType _characterRangeType;
  private bool hasUnicode;

  internal TextPart(Paragraph paragraph, string text)
  {
    this._paragraph = paragraph;
    this._text = text;
  }

  internal TextPart(Paragraph paragraph) => this._paragraph = paragraph;

  internal FontScriptType ScriptType
  {
    get => this._scriptType;
    set => this._scriptType = value;
  }

  internal bool HasUnicode
  {
    get => this.hasUnicode;
    set => this.hasUnicode = value;
  }

  internal CharacterRangeType CharacterRange
  {
    get => this._characterRangeType;
    set => this._characterRangeType = value;
  }

  internal bool IsKerning
  {
    get
    {
      int kerningValue = this.GetKerningValue();
      return kerningValue > 0 && (double) kerningValue <= (double) this.Font.FontSize;
    }
  }

  public IColor UnderlineColor
  {
    get
    {
      Syncfusion.Presentation.Presentation presentation = this._paragraph.BaseSlide != null ? this._paragraph.BaseSlide.Presentation : this._paragraph.Presentation;
      ((Syncfusion.Presentation.RichText.Font) this.Font).GetUnderlineColorObject().UpdateColorObject((object) presentation);
      return (IColor) this._font.GetUnderlineColorObject();
    }
    set
    {
      ((Syncfusion.Presentation.RichText.Font) this.Font).GetUnderlineColorObject().SetColor(ColorType.RGB, ((ColorObject) value).ToArgb());
    }
  }

  public IFont Font => (IFont) this._font ?? (IFont) (this._font = new Syncfusion.Presentation.RichText.Font(this));

  public IHyperLink Hyperlink => (IHyperLink) this._hyperlinkClick;

  internal Paragraph Paragraph => this._paragraph;

  public string Text
  {
    get
    {
      if (this.UniqueId != null && this.Type != null)
      {
        if (this.Type.Contains("datetime"))
          return this.GetDateTimeString();
        if (this.Type.Contains("slidenum"))
          return this.GetSlideNumberValue();
      }
      return this._text;
    }
    set
    {
      this._text = value;
      if (this._font == null)
        return;
      this._font.IsSpellingError = false;
    }
  }

  public string UniqueId
  {
    get => this._uniqueId;
    set => this._uniqueId = value;
  }

  public string Type
  {
    get => this._type;
    set => this._type = value;
  }

  public IHyperLink SetHyperlink(string link) => (IHyperLink) this.AddHyperLink(link);

  internal string GetSlideNumberValue()
  {
    if (this.Paragraph == null || this.Paragraph.BaseSlide == null)
      return this._text;
    BaseSlide baseSlide = this.Paragraph.BaseSlide;
    switch (baseSlide)
    {
      case Slide _:
        return (baseSlide as Slide).SlideNumber.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      case NotesSlide _:
        if ((baseSlide as NotesSlide).ParentSlide != null)
          return (baseSlide as NotesSlide).ParentSlide.SlideNumber.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        break;
    }
    return "‹#›";
  }

  internal string GetDateTimeString()
  {
    DateTimeFormatType dateTimeFormatType = Helper.GetDateTimeFormatType(this.Type);
    DateTime now = DateTime.Now;
    string shortDatePattern = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
    switch (dateTimeFormatType)
    {
      case DateTimeFormatType.None:
        return this._text;
      case DateTimeFormatType.DateTimeMdyy:
        return now.ToString(shortDatePattern);
      case DateTimeFormatType.DateTimeddddMMMMddyyyy:
        return now.ToString("dddd, MMMM dd, yyyy", (IFormatProvider) CultureInfo.InvariantCulture);
      case DateTimeFormatType.DateTimedMMMMyyyy:
        return now.ToString("d MMMM yyyy", (IFormatProvider) CultureInfo.InvariantCulture);
      case DateTimeFormatType.DateTimeMMMMdyyyy:
        return now.ToString("MMMM d, yyyy", (IFormatProvider) CultureInfo.InvariantCulture);
      case DateTimeFormatType.DateTimedMMMyy:
        return now.ToString("d-MMM-yy", (IFormatProvider) CultureInfo.InvariantCulture);
      case DateTimeFormatType.DateTimeMMMMyy:
        return now.ToString("MMMM yy", (IFormatProvider) CultureInfo.InvariantCulture);
      case DateTimeFormatType.DateTimeMMMyy:
        return now.ToString("MMM-yy", (IFormatProvider) CultureInfo.InvariantCulture);
      case DateTimeFormatType.DateTimeMMddyyhmmAMPM:
        return now.ToString(shortDatePattern + " h:mm tt", (IFormatProvider) CultureInfo.InvariantCulture);
      case DateTimeFormatType.DateTimeMMddyyhmmssAMPM:
        return now.ToString(shortDatePattern + " h:mm:ss tt", (IFormatProvider) CultureInfo.InvariantCulture);
      case DateTimeFormatType.DateTimeHmm:
        return now.ToString("H:mm", (IFormatProvider) CultureInfo.InvariantCulture);
      case DateTimeFormatType.DateTimeHmmss:
        return now.ToString("H:mm:ss", (IFormatProvider) CultureInfo.InvariantCulture);
      case DateTimeFormatType.DateTimehmmAMPM:
        return now.ToString("h:mm tt", (IFormatProvider) CultureInfo.InvariantCulture);
      case DateTimeFormatType.DateTimehmmssAMPM:
        return now.ToString("h:mm:ss tt", (IFormatProvider) CultureInfo.InvariantCulture);
      default:
        return this._text;
    }
  }

  internal int GetKerningValue()
  {
    int defaultKernValue;
    if ((this.Font as Syncfusion.Presentation.RichText.Font).Kerning.HasValue)
    {
      int? kerning = (this.Font as Syncfusion.Presentation.RichText.Font).Kerning;
      defaultKernValue = (kerning.HasValue ? new int?(kerning.GetValueOrDefault() / 1000) : new int?()).Value;
    }
    else
      defaultKernValue = this.Paragraph.GetDefaultKernValue(this.Paragraph.BaseSlide.Presentation.DefaultTextStyle.StyleList);
    return defaultKernValue;
  }

  internal Syncfusion.Presentation.RichText.Hyperlink AddHyperLink(string url)
  {
    if (url == null)
      throw new ArgumentException("Link cannot be null");
    int result = 0;
    this._hyperlinkClick = new Syncfusion.Presentation.RichText.Hyperlink(this);
    ISlides slides = this._paragraph.BaseSlide.Presentation.Slides;
    if (int.TryParse(url, out result) && result > 0 && result < slides.Count)
    {
      this.AddHyperLink(slides[result]);
      return this._hyperlinkClick;
    }
    if (url.StartsWith("www"))
      url = "http://" + url;
    else if (url.Contains("@") && !url.StartsWith("@") && !url.EndsWith("."))
    {
      if (!url.StartsWith("mailto:"))
        url = "mailto:" + url;
    }
    else if (url.Contains("#"))
    {
      url.IndexOf("#");
      url = url.Remove(result, url.Length - result);
    }
    this._hyperlinkClick.ActionString = !url.ToLowerInvariant().EndsWith(".pptx") ? "ppaction://hlinkfile" : "ppaction://hlinkpres";
    this._hyperlinkClick.SetLink(url);
    return this._hyperlinkClick;
  }

  public void AddHyperLink(ISlide slide)
  {
    this._hyperlinkClick = new Syncfusion.Presentation.RichText.Hyperlink(this);
    this._hyperlinkClick.ActionString = "ppaction://hlinksldjump";
    this._hyperlinkClick.SetTargetSlide(slide);
    this._hyperlinkClick.SetTargetSlideRelation();
  }

  public void RemoveHyperLink()
  {
    this._hyperlinkClick.Close();
    if (this._paragraph.BaseSlide is Slide)
      (this._paragraph.BaseSlide as Slide).TopRelation.RemoveRelationByKeword("hyperlink");
    this._hyperlinkClick = (Syncfusion.Presentation.RichText.Hyperlink) null;
  }

  internal void SetHyperlink(Syncfusion.Presentation.RichText.Hyperlink hyperLink)
  {
    this._hyperlinkClick = hyperLink;
  }

  internal bool Compare(ITextPart textPart)
  {
    TextPart textPart1 = (TextPart) textPart;
    return this.Font.Equals((object) textPart1._font) && this._text != textPart1._text;
  }

  internal IFont GetLineBreakProps() => (IFont) this._lineBreakFont;

  internal bool IsWordSplitCharacter() => this.CharacterRange == CharacterRangeType.WordSplit;

  internal bool IsHindiScript() => this.ScriptType == FontScriptType.Hindi;

  internal void SetFont(Syncfusion.Presentation.RichText.Font font) => this._font = font;

  internal void SetLineBreakProps(Syncfusion.Presentation.RichText.Font font)
  {
    this._lineBreakFont = font;
  }

  internal void Close()
  {
    if (this._font != null)
    {
      this._font.Close();
      this._font = (Syncfusion.Presentation.RichText.Font) null;
    }
    if (this._lineBreakFont != null)
    {
      this._lineBreakFont.Close();
      this._lineBreakFont = (Syncfusion.Presentation.RichText.Font) null;
    }
    if (this._hyperlinkClick != null)
    {
      this._hyperlinkClick.Close();
      this._hyperlinkClick = (Syncfusion.Presentation.RichText.Hyperlink) null;
    }
    this._paragraph = (Paragraph) null;
  }

  public TextPart Clone()
  {
    TextPart textPart = (TextPart) this.MemberwiseClone();
    if (this._font != null)
    {
      textPart._font = this._font.Clone();
      textPart._font.SetParent(textPart);
    }
    if (this._hyperlinkClick != null)
      textPart._hyperlinkClick = this._hyperlinkClick.Clone();
    if (this._lineBreakFont != null)
    {
      textPart._lineBreakFont = this._lineBreakFont.Clone();
      textPart._lineBreakFont.SetParent(textPart);
    }
    return textPart;
  }

  internal void SetParent(Paragraph paragraph)
  {
    this._paragraph = paragraph;
    if (this._font == null)
      return;
    this._font.SetParent(paragraph);
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    if (this._font == null)
      return;
    this._font.SetParent(presentation);
  }

  internal Syncfusion.Presentation.RichText.Font GetFontObject() => this._font;

  internal void SetParent(Shape shape)
  {
    if (this._hyperlinkClick == null)
      return;
    this._hyperlinkClick.SetParent(shape);
  }
}
