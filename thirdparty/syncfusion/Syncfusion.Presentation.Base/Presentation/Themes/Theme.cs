// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Themes.Theme
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Themes;

internal class Theme
{
  private DefaultFonts _majorFont;
  private DefaultFonts _minorFont;
  private Dictionary<string, string> _colorCheck;
  private Dictionary<string, string> _themeColors;
  private List<Fill> _fillFormatList;
  private List<LineFormat> _lineFormatList;
  private List<Fill> _bgFillFormatList;
  private string _name;
  private List<EffectStyle> _effectStyleList;
  private RelationCollection _topRelation;
  private string _fontSchemeName;
  private string _formatSchemeName;
  internal string[] fontTag = new string[3]
  {
    "latin",
    "ea",
    "cs"
  };
  private object _parent;
  private Dictionary<string, string> _mergedImagePathList;

  internal Theme(MasterSlide masterSlide)
  {
    this._parent = (object) masterSlide;
    this._themeColors = new Dictionary<string, string>(13);
  }

  internal Theme(HandoutMaster handoutMaster)
  {
    this._parent = (object) handoutMaster;
    this._themeColors = new Dictionary<string, string>(13);
  }

  internal Theme(NotesMasterSlide notesMasterSlide)
  {
    this._parent = (object) notesMasterSlide;
    this._themeColors = new Dictionary<string, string>(13);
  }

  internal Dictionary<string, string> MergedImagePathList
  {
    get
    {
      return this._mergedImagePathList ?? (this._mergedImagePathList = new Dictionary<string, string>());
    }
  }

  internal List<Fill> FillFormats
  {
    get => this._fillFormatList ?? (this._fillFormatList = new List<Fill>());
  }

  internal List<LineFormat> LineFormats
  {
    get => this._lineFormatList ?? (this._lineFormatList = new List<LineFormat>());
  }

  internal List<Fill> BgFillFormats
  {
    get => this._bgFillFormatList ?? (this._bgFillFormatList = new List<Fill>());
  }

  internal string Name
  {
    get => this._name;
    set => this._name = value;
  }

  internal DefaultFonts MajorFont
  {
    get => this._majorFont;
    set => this._majorFont = value;
  }

  internal Dictionary<string, string> ColorCheck
  {
    get => this._colorCheck ?? (this._colorCheck = new Dictionary<string, string>());
  }

  internal List<EffectStyle> EffectStyles
  {
    get => this._effectStyleList ?? (this._effectStyleList = new List<EffectStyle>());
  }

  internal DefaultFonts MinorFont
  {
    get => this._minorFont;
    set => this._minorFont = value;
  }

  internal Dictionary<string, string> ThemeColors => this._themeColors;

  internal RelationCollection TopRelation
  {
    get
    {
      if (this._mergedImagePathList != null && this._topRelation != null)
      {
        foreach (Relation relation in this._topRelation.GetRelationList())
          relation.Target = this._mergedImagePathList[relation.Id];
      }
      return this._topRelation;
    }
    set => this._topRelation = value;
  }

  internal BaseSlide BaseSlide
  {
    get
    {
      if (this._parent is MasterSlide)
        return (BaseSlide) (this._parent as MasterSlide);
      return this._parent is NotesMasterSlide ? (BaseSlide) this._parent : (BaseSlide) (this._parent as HandoutMaster);
    }
  }

  internal string FontSchemeName
  {
    get => this._fontSchemeName;
    set => this._fontSchemeName = value;
  }

  internal string FormatSchemeName
  {
    get => this._formatSchemeName;
    set => this._formatSchemeName = value;
  }

  internal void SetFillFormats(List<Fill> fillFormats) => this._fillFormatList = fillFormats;

  internal void SetBgFillFormats(List<Fill> fillFormats) => this._bgFillFormatList = fillFormats;

  internal IColor GetThemeColor(string index)
  {
    switch (index)
    {
      case "bg1":
      case "bg2":
      case "tx1":
      case "tx2":
        index = Helper.GetThemeIndex(index, this._parent as MasterSlide);
        break;
    }
    return Helper.GetColor(this._themeColors[index]);
  }

  internal int GetThemeColorValue(string index)
  {
    switch (index)
    {
      case "bg1":
      case "bg2":
      case "tx1":
      case "tx2":
        index = Helper.GetThemeIndex(index, this._parent as MasterSlide);
        break;
    }
    return Helper.GetColor(this._themeColors[index]).ToArgb();
  }

  internal void Close() => this.ClearAll();

  private void ClearAll()
  {
    if (this._majorFont != null)
    {
      this._majorFont.Close();
      this._majorFont = (DefaultFonts) null;
    }
    if (this._minorFont != null)
    {
      this._minorFont.Close();
      this._minorFont = (DefaultFonts) null;
    }
    if (this._themeColors != null)
    {
      this._themeColors.Clear();
      this._themeColors = (Dictionary<string, string>) null;
    }
    if (this._fillFormatList != null)
    {
      foreach (Fill fillFormat in this._fillFormatList)
        fillFormat.Close();
      this._fillFormatList.Clear();
      this._fillFormatList = (List<Fill>) null;
    }
    if (this._lineFormatList != null)
    {
      foreach (LineFormat lineFormat in this._lineFormatList)
        lineFormat.Close();
      this._lineFormatList.Clear();
      this._lineFormatList = (List<LineFormat>) null;
    }
    if (this._bgFillFormatList != null)
    {
      foreach (Fill bgFillFormat in this._bgFillFormatList)
        bgFillFormat.Close();
      this._bgFillFormatList.Clear();
      this._bgFillFormatList = (List<Fill>) null;
    }
    if (this._effectStyleList != null)
    {
      foreach (EffectStyle effectStyle in this._effectStyleList)
        effectStyle.Close();
      this._effectStyleList.Clear();
      this._effectStyleList = (List<EffectStyle>) null;
    }
    if (this._colorCheck != null)
    {
      this._colorCheck.Clear();
      this._colorCheck = (Dictionary<string, string>) null;
    }
    if (this._topRelation != null)
    {
      this._topRelation.Close();
      this._topRelation = (RelationCollection) null;
    }
    if (this.fontTag != null)
      this.fontTag = (string[]) null;
    if (this._mergedImagePathList != null)
    {
      this._mergedImagePathList.Clear();
      this._mergedImagePathList = (Dictionary<string, string>) null;
    }
    this._parent = (object) null;
  }

  public Theme Clone()
  {
    Theme theme = (Theme) this.MemberwiseClone();
    if (this._bgFillFormatList != null)
      theme._bgFillFormatList = this.CloneFillList(this._bgFillFormatList, theme);
    if (this._colorCheck != null)
      theme._colorCheck = Helper.CloneDictionary(this._colorCheck);
    if (this._effectStyleList != null)
      theme._effectStyleList = this.CloneEffectStyleList();
    if (this._fillFormatList != null)
      theme._fillFormatList = this.CloneFillList(this._fillFormatList, theme);
    if (this._lineFormatList != null)
      theme._lineFormatList = this.CloneLineFormatList(this._lineFormatList);
    if (this._majorFont != null)
      theme._majorFont = this._majorFont.Clone();
    if (this._minorFont != null)
      theme._minorFont = this._minorFont.Clone();
    theme._themeColors = Helper.CloneDictionary(this._themeColors);
    if (this._topRelation != null)
      theme._topRelation = this._topRelation.Clone();
    return theme;
  }

  private List<LineFormat> CloneLineFormatList(List<LineFormat> list)
  {
    List<LineFormat> lineFormatList = new List<LineFormat>();
    foreach (LineFormat lineFormat1 in list)
    {
      LineFormat lineFormat2 = lineFormat1.Clone();
      lineFormatList.Add(lineFormat2);
    }
    return lineFormatList;
  }

  internal void SetParent(BaseSlide baseSlide)
  {
    foreach (LineFormat lineFormat in this._lineFormatList)
      lineFormat.SetParent(baseSlide.Presentation);
    foreach (EffectStyle effectStyle in this._effectStyleList)
      effectStyle.SetParent(baseSlide.Presentation);
    this._parent = (object) baseSlide;
  }

  private List<EffectStyle> CloneEffectStyleList()
  {
    List<EffectStyle> effectStyleList = new List<EffectStyle>();
    foreach (EffectStyle effectStyle1 in this._effectStyleList)
    {
      EffectStyle effectStyle2 = effectStyle1.Clone();
      effectStyleList.Add(effectStyle2);
    }
    return effectStyleList;
  }

  private List<Fill> CloneFillList(List<Fill> fillList, Theme theme)
  {
    List<Fill> fillList1 = new List<Fill>();
    foreach (Fill fill1 in fillList)
    {
      Fill fill2 = fill1.Clone();
      fill2.SetParent((object) theme);
      fillList1.Add(fill2);
    }
    return fillList1;
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    foreach (EffectStyle effectStyle in this._effectStyleList)
      effectStyle.SetParent(presentation);
  }
}
