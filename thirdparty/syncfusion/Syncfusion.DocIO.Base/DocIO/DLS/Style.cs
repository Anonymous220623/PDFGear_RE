﻿// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Style
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public abstract class Style : XDLSSerializableBase, IStyle
{
  protected const int DEF_USER_STYLE_ID = 4094;
  private int m_styleId = 4094;
  private string m_strName;
  protected IStyle m_baseStyle;
  protected WCharacterFormat m_chFormat;
  protected string m_nextStyle;
  protected string m_linkStyle;
  private string m_styleIDName;
  protected WordStyleType m_typeCode;
  protected byte[] m_tapx;
  private byte m_bFlags;
  private int uiPriority = int.MinValue;
  private List<Entity> m_rangeCollection;
  internal bool IsRemoving;

  internal byte[] TableStyleData
  {
    get => this.m_tapx;
    set => this.m_tapx = value;
  }

  internal WordStyleType TypeCode
  {
    get => this.m_typeCode;
    set
    {
      if (this.StyleType == StyleType.ParagraphStyle && value != WordStyleType.ParagraphStyle)
        this.RemoveBaseStyle();
      this.m_typeCode = value;
    }
  }

  public WCharacterFormat CharacterFormat => this.m_chFormat;

  public string Name
  {
    get => this.m_strName;
    set
    {
      if (value == null || value.Length == 0)
        throw new ArgumentNullException(nameof (Name));
      if (this.StyleType == StyleType.ParagraphStyle && value == "Normal" && !this.Document.IsNormalStyleDefined)
      {
        (this.Document.Styles as StyleCollection).InnerList.Remove((object) this.m_baseStyle);
        this.RemoveBaseStyle();
        this.Document.IsNormalStyleDefined = true;
      }
      else if (this.StyleType == StyleType.CharacterStyle && (value == "DefaultParagraphFont" || value == "Default Paragraph Font") && !this.Document.IsDefaultParagraphFontStyleDefined)
      {
        (this.Document.Styles as StyleCollection).InnerList.Remove((object) this.m_baseStyle);
        this.RemoveBaseStyle();
        this.Document.IsDefaultParagraphFontStyleDefined = true;
      }
      if (!this.Document.IsOpening && !this.Document.IsMailMerge && !this.Document.IsCloning && this.Document != null && this.Document.Styles.FindByName(value, this.StyleType) != null)
        throw new ArgumentException("Name of style already exists");
      string lower = value.Replace(" ", string.Empty).ToLower();
      Dictionary<string, int> builtinStyleIds = this.GetBuiltinStyleIds();
      this.StyleId = !builtinStyleIds.ContainsKey(lower) ? 4094 : builtinStyleIds[lower];
      this.m_strName = value;
    }
  }

  internal IStyle BaseStyle => this.m_baseStyle;

  internal int StyleId
  {
    get => this.m_styleId;
    set => this.m_styleId = value;
  }

  public abstract StyleType StyleType { get; }

  public BuiltinStyle BuiltInStyleIdentifier => Style.NameToBuiltIn(this.Name);

  internal string NextStyle
  {
    get
    {
      if (this.m_nextStyle == null)
        return this.Name;
      if (this.Document.StyleNameIds.ContainsKey(this.m_nextStyle))
        return this.Document.StyleNameIds[this.m_nextStyle];
      return this.Document.Styles.FindByName(this.m_nextStyle) != null ? this.m_nextStyle : this.Name;
    }
    set => this.m_nextStyle = value;
  }

  internal string LinkStyle
  {
    get => this.m_linkStyle;
    set => this.m_linkStyle = value;
  }

  internal string StyleIDName
  {
    get => this.m_styleIDName;
    set => this.m_styleIDName = value;
  }

  public bool IsPrimaryStyle
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsSemiHidden
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  internal bool UnhideWhenUsed
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal bool IsCustom
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  internal int UIPriority
  {
    get => this.uiPriority;
    set => this.uiPriority = value;
  }

  internal List<Entity> RangeCollection
  {
    get
    {
      if (this.m_rangeCollection == null)
        this.m_rangeCollection = new List<Entity>();
      return this.m_rangeCollection;
    }
  }

  protected Style(WordDocument doc)
    : base(doc, (Entity) doc)
  {
    this.m_chFormat = new WCharacterFormat((IWordDocument) this.Document);
    this.m_chFormat.SetOwner((OwnerHolder) this);
    this.m_strName = nameof (Style) + (object) doc.Styles.Count;
  }

  public virtual void ApplyBaseStyle(string styleName)
  {
    if (styleName == this.Name)
    {
      switch (this.StyleType)
      {
        case StyleType.ParagraphStyle:
          if (this.Name == "Normal")
            return;
          this.m_baseStyle = this.m_doc.Styles.FindByName("Normal");
          break;
        case StyleType.CharacterStyle:
          if (this.Name == "Default Paragraph Font" || this.Name == "DefaultParagraphFont")
            return;
          this.m_baseStyle = this.m_doc.Styles.FindByName("Default Paragraph Font");
          break;
        case StyleType.TableStyle:
          if (this.Name == "Normal Table" || this.Name == "TableNormal")
            return;
          this.m_baseStyle = this.m_doc.Styles.FindByName("Normal Table");
          break;
        case StyleType.NumberingStyle:
          if (this.Name == "No List" || this.Name == "NoList")
            return;
          this.m_baseStyle = this.m_doc.Styles.FindByName("No List");
          break;
      }
    }
    else
      this.m_baseStyle = this.m_doc.Styles.FindByName(styleName, this.StyleType);
    if (this.m_baseStyle == null)
      this.m_baseStyle = this.m_doc.Styles.FindByName(styleName);
    if (this.m_baseStyle == null && this.StyleType == StyleType.CharacterStyle)
      this.m_baseStyle = (IStyle) new WCharacterStyle(this.m_doc);
    else if (this.m_baseStyle == null)
      this.m_baseStyle = (IStyle) new WParagraphStyle((IWordDocument) this.m_doc);
    this.CharacterFormat.ApplyBase((FormatBase) ((Style) this.BaseStyle).CharacterFormat);
  }

  public void ApplyBaseStyle(BuiltinStyle bStyle)
  {
    IStyle style = this.m_doc.AddStyle(bStyle);
    if (style == null)
      return;
    this.ApplyBaseStyle(style.Name);
  }

  public void Remove()
  {
    if (!this.IsCustom || this.RangeCollection.Count <= 0)
      return;
    this.IsRemoving = true;
    if (this is WParagraphStyle)
    {
      foreach (Entity range in this.RangeCollection)
      {
        if (range is WParagraph)
          (range as WParagraph).ApplyStyle(BuiltinStyle.Normal);
      }
      foreach (Style style in (IEnumerable) this.Document.Styles)
      {
        if (style is WParagraphStyle wparagraphStyle && this == style.BaseStyle)
        {
          wparagraphStyle.ParagraphFormat.CopyFormat((FormatBase) (this as WParagraphStyle).ParagraphFormat);
          wparagraphStyle.CharacterFormat.CopyFormat((FormatBase) (this as WParagraphStyle).CharacterFormat);
          style.ApplyBaseStyle(BuiltinStyle.Normal);
        }
      }
    }
    else if (this is WCharacterStyle)
    {
      foreach (Entity range in this.RangeCollection)
        (range as ParagraphItem).GetCharFormat().CharStyleName = "Default Paragraph Font";
      foreach (Style style in (IEnumerable) this.Document.Styles)
      {
        if (style is WCharacterStyle wcharacterStyle && this == style.BaseStyle)
        {
          wcharacterStyle.CharacterFormat.CopyFormat((FormatBase) this.CharacterFormat);
          wcharacterStyle.ApplyBaseStyle(BuiltinStyle.DefaultParagraphFont);
        }
      }
    }
    else if (this.StyleType == StyleType.NumberingStyle)
    {
      foreach (Entity range in this.RangeCollection)
      {
        if (range is WParagraph)
          (range as WParagraph).ListFormat.RemoveList();
      }
      foreach (ListStyle listStyle in (CollectionImpl) this.Document.ListStyles)
      {
        if (listStyle.StyleLink == this.Name)
          listStyle.StyleLink = (string) null;
      }
    }
    (this.Document.Styles as StyleCollection).Remove((IStyle) this);
    this.Close();
  }

  public abstract IStyle Clone();

  internal virtual void ApplyBaseStyle(Style baseStyle)
  {
    this.m_baseStyle = (IStyle) baseStyle;
    this.CharacterFormat.ApplyBase((FormatBase) ((Style) this.BaseStyle).CharacterFormat);
  }

  internal void RemoveBaseStyle()
  {
    switch (this)
    {
      case WParagraphStyle _:
        WParagraphStyle wparagraphStyle = this as WParagraphStyle;
        wparagraphStyle.CharacterFormat.BaseFormat = (FormatBase) null;
        wparagraphStyle.ParagraphFormat.ApplyBase((FormatBase) null);
        wparagraphStyle.m_baseStyle = (IStyle) null;
        break;
      case WCharacterStyle _:
        WCharacterStyle wcharacterStyle = this as WCharacterStyle;
        if (wcharacterStyle.CharacterFormat.BaseFormat != null)
          wcharacterStyle.CharacterFormat.BaseFormat = (FormatBase) null;
        if (wcharacterStyle.m_baseStyle == null)
          break;
        wcharacterStyle.m_baseStyle = (IStyle) null;
        break;
    }
  }

  internal void SetStyleName(string name)
  {
    this.m_strName = name != null && name.Length != 0 ? name : throw new ArgumentNullException("Style Name should not be null or empty");
  }

  protected override object CloneImpl()
  {
    Style owner = (Style) base.CloneImpl();
    owner.m_chFormat = new WCharacterFormat((IWordDocument) this.Document);
    owner.m_chFormat.ImportContainer((FormatBase) this.CharacterFormat);
    owner.m_chFormat.CopyProperties((FormatBase) this.CharacterFormat);
    owner.m_chFormat.SetOwner((OwnerHolder) owner);
    return (object) owner;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    if (doc == this.Document)
      return;
    if (this.m_baseStyle != null && (this.m_baseStyle as Style).ImportStyleTo(doc, false) is Style baseStyle)
    {
      switch (this)
      {
        case WParagraphStyle _:
          (this as WParagraphStyle).ApplyBaseStyle(baseStyle);
          break;
        case WTableStyle _:
          (this as WTableStyle).ApplyBaseStyle(baseStyle);
          break;
        default:
          this.ApplyBaseStyle(baseStyle);
          break;
      }
    }
    this.CharacterFormat.CloneRelationsTo(doc);
    switch (this)
    {
      case WParagraphStyle _:
        (this as WParagraphStyle).ListFormat.CloneListRelationsTo(doc, this.Name);
        break;
      case WTableStyle _:
        (this as WTableStyle).ListFormat.CloneListRelationsTo(doc, this.Name);
        break;
    }
    this.SetOwner((OwnerHolder) doc);
  }

  internal static bool HasGuid(string styleName, out string guid)
  {
    guid = string.Empty;
    char[] chArray = new char[1]{ '-' };
    if (styleName.Contains("_") && styleName.Contains("-"))
    {
      int startIndex = styleName.LastIndexOf("_") + 1;
      if (styleName.Length > startIndex)
        guid = styleName.Substring(startIndex);
      string[] strArray = guid.Split(chArray);
      if (strArray.Length == 5 && guid.Length - 4 == 32 /*0x20*/ && strArray[0].Length == 8 && strArray[1].Length == 4 && strArray[2].Length == 4 && strArray[3].Length == 4 && strArray[4].Length == 12)
        return true;
    }
    return false;
  }

  internal virtual bool Compare(Style style)
  {
    if (this.StyleType != style.StyleType)
      return false;
    if (this.BaseStyle != null && style.BaseStyle != null)
    {
      if (!(this.BaseStyle as Style).Compare((Style) style.BaseStyle))
        return false;
    }
    else if (this.BaseStyle != null && style.BaseStyle == null || this.BaseStyle == null && style.BaseStyle != null)
      return false;
    return style.CharacterFormat == null || this.CharacterFormat == null || this.CharacterFormat.Compare(style.CharacterFormat);
  }

  internal IStyle ImportStyleTo(WordDocument doc, bool isParagraphStyle)
  {
    if (doc == this.Document)
      return (IStyle) this;
    List<string> styelNames = new List<string>();
    bool isDiffTypeStyleFound = false;
    if (!((doc.Styles as StyleCollection).FindByName(this.Name, this.StyleType, ref styelNames, ref isDiffTypeStyleFound) is Style foundStyle))
    {
      Style style = this.Clone() as Style;
      if (!isDiffTypeStyleFound)
      {
        if (isParagraphStyle && doc.UpdateAlternateChunk && style is WParagraphStyle)
          this.CopyBaseStyleFormatting(doc, style as WParagraphStyle);
        foundStyle = this.AddNewStyle(doc, style, false, styelNames) as Style;
      }
      else
        foundStyle = !doc.ImportStylesOnTypeMismatch ? (this.StyleType != StyleType.CharacterStyle ? (this.StyleType != StyleType.ParagraphStyle ? (this.StyleType != StyleType.TableStyle ? doc.Styles.FindByName("No List", StyleType.NumberingStyle) as Style : doc.Styles.FindByName("Normal Table", StyleType.TableStyle) as Style) : doc.Styles.FindByName("Normal", StyleType.ParagraphStyle) as Style) : doc.Styles.FindByName("Default Paragraph Font", StyleType.CharacterStyle) as Style) : this.AddNewStyle(doc, style, true, styelNames) as Style;
    }
    else if (doc.ImportStyles)
      foundStyle = this.CompareAndImportStyle(foundStyle, doc, styelNames);
    styelNames.Clear();
    return (IStyle) foundStyle;
  }

  private Style CompareAndImportStyle(Style foundStyle, WordDocument doc, List<string> styleNames)
  {
    if (this.StyleType == StyleType.CharacterStyle || this.StyleType == StyleType.ParagraphStyle)
    {
      if (this.Compare(foundStyle))
        return foundStyle;
      foreach (Style style in (IEnumerable) doc.Styles)
      {
        if (style.StyleType == this.StyleType && style.Name.StartsWithExt(this.Name + "_") && this.Compare(style))
          return style;
      }
    }
    foundStyle = this.AddNewStyle(doc, this.Clone() as Style, true, styleNames) as Style;
    return foundStyle;
  }

  private IStyle AddNewStyle(
    WordDocument doc,
    Style newStyle,
    bool isToClone,
    List<string> styleNames)
  {
    string name = this.Name;
    if (isToClone)
    {
      if (newStyle != null)
      {
        newStyle.SetStyleName(this.GetUniqueStyleName(name, styleNames));
        newStyle.StyleId = 4094;
      }
    }
    else if (this.Document.StyleNameIds.ContainsValue(name) && !doc.StyleNameIds.ContainsValue(name))
    {
      string styleNameId = this.GetStyleNameId(name);
      if (!doc.StyleNameIds.ContainsKey(styleNameId))
        doc.StyleNameIds.Add(styleNameId, name);
    }
    newStyle.RangeCollection.Clear();
    doc.Styles.Add((IStyle) newStyle);
    return (IStyle) newStyle;
  }

  private void CopyBaseStyleFormatting(WordDocument destDocument, WParagraphStyle paraStyle)
  {
    WParagraphStyle baseStyle = paraStyle.BaseStyle;
    if (baseStyle == null || string.IsNullOrEmpty(baseStyle.Name) || !(destDocument.Styles.FindByName(baseStyle.Name) is WParagraphStyle byName))
      return;
    paraStyle.ParagraphFormat.UpdateSourceFormat(byName.ParagraphFormat);
    paraStyle.CharacterFormat.UpdateSourceFormat(byName.CharacterFormat);
  }

  internal string GetUniqueStyleName(string styleName, List<string> styleNames)
  {
    while (styleNames.Contains(styleName))
      styleName = $"{styleName}_{(object) Guid.NewGuid()}";
    return styleName;
  }

  private string GetStyleNameId(string styleName)
  {
    string styleNameId1 = "";
    foreach (KeyValuePair<string, string> styleNameId2 in this.Document.StyleNameIds)
    {
      if (styleNameId2.Value == styleName)
      {
        styleNameId1 = styleNameId2.Key;
        break;
      }
    }
    return styleNameId1;
  }

  public static IStyle CreateBuiltinStyle(BuiltinStyle bStyle, WordDocument doc)
  {
    IStyle style = (IStyle) new WParagraphStyle((IWordDocument) doc);
    if (doc.Styles.FindByName(Style.BuiltInToName(bStyle), StyleType.ParagraphStyle) is WParagraphStyle byName)
      return (IStyle) byName;
    Style.BuiltinStyleLoader.LoadStyle(style, bStyle);
    if (style.Name == "Normal" && style.StyleType == StyleType.ParagraphStyle)
      (style as WParagraphStyle).CharacterFormat.LocaleIdASCII = (short) 1033;
    if (style.Name == "Normal (Web)" && style.StyleType == StyleType.ParagraphStyle && doc.ActualFormatType == FormatType.Html)
    {
      (style as WParagraphStyle).ParagraphFormat.BeforeSpacing = 5f;
      (style as WParagraphStyle).ParagraphFormat.AfterSpacing = 5f;
      (style as WParagraphStyle).ParagraphFormat.SpaceAfterAuto = true;
      (style as WParagraphStyle).ParagraphFormat.SpaceBeforeAuto = true;
    }
    return style;
  }

  internal static IStyle CreateBuiltinCharacterStyle(BuiltinStyle bStyle, WordDocument doc)
  {
    IStyle style = (IStyle) new WCharacterStyle(doc);
    if (doc.Styles.FindByName(Style.BuiltInToName(bStyle), StyleType.CharacterStyle) is WCharacterStyle byName)
      return (IStyle) byName;
    Style.BuiltinStyleLoader.LoadStyle(style, bStyle);
    return style;
  }

  internal static IStyle CreateBuiltinStyle(BuiltinTableStyle bStyle, WordDocument doc)
  {
    IStyle style = (IStyle) new WTableStyle((IWordDocument) doc);
    Style.BuiltinStyleLoader.LoadStyle(style, bStyle);
    return style;
  }

  public static IStyle CreateBuiltinStyle(BuiltinStyle bStyle, StyleType type, WordDocument doc)
  {
    IStyle style = (IStyle) null;
    switch (type)
    {
      case StyleType.ParagraphStyle:
        style = (IStyle) new WParagraphStyle((IWordDocument) doc);
        break;
      case StyleType.CharacterStyle:
        style = (IStyle) new WCharacterStyle(doc);
        break;
      case StyleType.OtherStyle:
        style = (IStyle) new ListStyle(doc);
        break;
    }
    Style.BuiltinStyleLoader.LoadStyle(style, bStyle);
    return style;
  }

  internal static string BuiltInToName(BuiltinStyle bstyle)
  {
    return Style.BuiltinStyleLoader.BuiltinStyleNames[(int) bstyle];
  }

  internal static string BuiltInToName(BuiltinTableStyle bstyle)
  {
    return Style.BuiltinStyleLoader.BuiltinTableStyleNames[(int) bstyle];
  }

  public static BuiltinStyle NameToBuiltIn(string styleName)
  {
    string str = styleName.Trim();
    BuiltinStyle builtIn = BuiltinStyle.User;
    int length = Style.BuiltinStyleLoader.BuiltinStyleNames.Length;
    for (int index = 0; index < length; ++index)
    {
      if (Style.BuiltinStyleLoader.BuiltinStyleNames[index] == str)
      {
        builtIn = (BuiltinStyle) index;
        break;
      }
    }
    return builtIn;
  }

  internal static bool IsListStyle(BuiltinStyle bstyle)
  {
    return Style.BuiltinStyleLoader.IsListStyle(bstyle);
  }

  void IStyle.Close() => this.Close();

  internal new virtual void Close()
  {
    if (this.m_chFormat != null)
    {
      this.m_chFormat.Close();
      this.m_chFormat = (WCharacterFormat) null;
    }
    if (this.m_tapx != null)
      this.m_tapx = (byte[]) null;
    if (this.m_baseStyle != null)
      this.m_baseStyle = (IStyle) null;
    if (this.m_rangeCollection != null)
    {
      this.m_rangeCollection.Clear();
      this.m_rangeCollection = (List<Entity>) null;
    }
    base.Close();
  }

  internal Dictionary<string, string> GetBuiltinStyles()
  {
    return new Dictionary<string, string>()
    {
      {
        "normal",
        "Normal"
      },
      {
        "heading 1",
        "Heading 1"
      },
      {
        "heading 2",
        "Heading 2"
      },
      {
        "heading 3",
        "Heading 3"
      },
      {
        "heading 4",
        "Heading 4"
      },
      {
        "heading 5",
        "Heading 5"
      },
      {
        "heading 6",
        "Heading 6"
      },
      {
        "heading 7",
        "Heading 7"
      },
      {
        "heading 8",
        "Heading 8"
      },
      {
        "heading 9",
        "Heading 9"
      },
      {
        "index 1",
        "Index 1"
      },
      {
        "index 2",
        "Index 2"
      },
      {
        "index 3",
        "Index 3"
      },
      {
        "index 4",
        "Index 4"
      },
      {
        "index 5",
        "Index 5"
      },
      {
        "index 6",
        "Index 6"
      },
      {
        "index 7",
        "Index 7"
      },
      {
        "index 8",
        "Index 8"
      },
      {
        "index 9",
        "Index 9"
      },
      {
        "toc 1",
        "TOC 1"
      },
      {
        "toc 2",
        "TOC 2"
      },
      {
        "toc 3",
        "TOC 3"
      },
      {
        "toc 4",
        "TOC 4"
      },
      {
        "toc 5",
        "TOC 5"
      },
      {
        "toc 6",
        "TOC 6"
      },
      {
        "toc 7",
        "TOC 7"
      },
      {
        "toc 8",
        "TOC 8"
      },
      {
        "toc 9",
        "TOC 9"
      },
      {
        "normal indent",
        "Normal Indent"
      },
      {
        "footnote text",
        "Footnote Text"
      },
      {
        "comment text",
        "Comment Text"
      },
      {
        "header",
        "Header"
      },
      {
        "footer",
        "Footer"
      },
      {
        "index heading",
        "Index Heading"
      },
      {
        "caption",
        "Caption"
      },
      {
        "table of figures",
        "Table of Figures"
      },
      {
        "footnote reference",
        "Footnote Reference"
      },
      {
        "comment reference",
        "Comment Reference"
      },
      {
        "line number",
        "Line Number"
      },
      {
        "page number",
        "Page Number"
      },
      {
        "endnote reference",
        "Endnote Reference"
      },
      {
        "endnote text",
        "Endnote Text"
      },
      {
        "table of authorities",
        "Table of Authorities"
      },
      {
        "macro",
        "Macro Text"
      },
      {
        "toa heading",
        "TOA Heading"
      },
      {
        "list",
        "List"
      },
      {
        "list bullet",
        "List Bullet"
      },
      {
        "list number",
        "List Number"
      },
      {
        "list 2",
        "List 2"
      },
      {
        "list 3",
        "List 3"
      },
      {
        "list 4",
        "List 4"
      },
      {
        "list 5",
        "List 5"
      },
      {
        "list bullet 2",
        "List Bullet 2"
      },
      {
        "list bullet 3",
        "List Bullet 3"
      },
      {
        "list bullet 4",
        "List Bullet 4"
      },
      {
        "list bullet 5",
        "List Bullet 5"
      },
      {
        "list number 2",
        "List Number 2"
      },
      {
        "list number 3",
        "List Number 3"
      },
      {
        "list number 4",
        "List Number 4"
      },
      {
        "list number 5",
        "List Number 5"
      },
      {
        "title",
        "Title"
      },
      {
        "closing",
        "Closing"
      },
      {
        "signature",
        "Signature"
      },
      {
        "default paragraph font",
        "Default Paragraph Font"
      },
      {
        "body text",
        "Body Text"
      },
      {
        "body text indent",
        "Body Text Indent"
      },
      {
        "list continue",
        "List Continue"
      },
      {
        "list continue 2",
        "List Continue 2"
      },
      {
        "list continue 3",
        "List Continue 3"
      },
      {
        "list continue 4",
        "List Continue 4"
      },
      {
        "list continue 5",
        "List Continue 5"
      },
      {
        "message header",
        "Message Header"
      },
      {
        "subtitle",
        "Subtitle"
      },
      {
        "salutation",
        "Salutation"
      },
      {
        "date",
        "Date"
      },
      {
        "body text first indent",
        "Body Text First Indent"
      },
      {
        "body text first indent 2",
        "Body Text First Indent 2"
      },
      {
        "note heading",
        "Note Heading"
      },
      {
        "body text 2",
        "Body Text 2"
      },
      {
        "body text 3",
        "Body Text 3"
      },
      {
        "body text indent 2",
        "Body Text Indent 2"
      },
      {
        "body text indent 3",
        "Body Text Indent 3"
      },
      {
        "block text",
        "Block Text"
      },
      {
        "hyperlink",
        "Hyperlink"
      },
      {
        "followedhyperlink",
        "FollowedHyperlink"
      },
      {
        "strong",
        "Strong"
      },
      {
        "emphasis",
        "Emphasis"
      },
      {
        "document map",
        "Document Map"
      },
      {
        "plain text",
        "Plain Text"
      },
      {
        "e-mail signature",
        "E-mail Signature"
      },
      {
        "normal (web)",
        "Normal (Web)"
      },
      {
        "html acronym",
        "HTML Acronym"
      },
      {
        "html address",
        "HTML Address"
      },
      {
        "html cite",
        "HTML Cite"
      },
      {
        "html code",
        "HTML Code"
      },
      {
        "html definition",
        "HTML Definition"
      },
      {
        "html keyboard",
        "HTML Keyboard"
      },
      {
        "html preformatted",
        "HTML Preformatted"
      },
      {
        "html sample",
        "HTML Sample"
      },
      {
        "html typewriter",
        "HTML Typewriter"
      },
      {
        "html variable",
        "HTML Variable"
      },
      {
        "comment subject",
        "Comment Subject"
      },
      {
        "no list",
        "No List"
      },
      {
        "balloon text",
        "Balloon Text"
      },
      {
        "user",
        "User"
      },
      {
        "nostyle",
        "NoStyle"
      },
      {
        "list paragraph",
        "List Paragraph"
      },
      {
        "quote",
        "Quote"
      },
      {
        "normal table",
        "Normal Table"
      },
      {
        "table grid",
        "Table Grid"
      },
      {
        "light shading",
        " Light Shading"
      },
      {
        "light shading accent 1",
        "Light Shading Accent 1"
      },
      {
        "light shading accent 2",
        "Light Shading Accent 2"
      },
      {
        "light shading accent 3",
        "Light Shading Accent 3"
      },
      {
        "light shading accent 4",
        "Light Shading Accent 4"
      },
      {
        "light shading accent 5",
        "Light Shading Accent 5"
      },
      {
        "light shading accent 6",
        "Light Shading Accent 6"
      },
      {
        "light list",
        "Light List"
      },
      {
        "light list accent 1",
        "Light List Accent 1"
      },
      {
        "light list accent 2",
        "Light List Accent 2"
      },
      {
        "light list accent 3",
        "Light List Accent 3"
      },
      {
        "light list accent 4",
        "Light List Accent 4"
      },
      {
        "light list accent 5",
        "Light List Accent 5"
      },
      {
        "light list accent 6",
        "Light List Accent 6"
      },
      {
        "light grid",
        "Light Grid"
      },
      {
        "light grid accent 1",
        "Light Grid Accent 1"
      },
      {
        "light grid accent 2",
        "Light Grid Accent 2"
      },
      {
        "light grid accent 3",
        " Light Grid Accent 3"
      },
      {
        "light grid accent 4",
        "Light Grid Accent 4"
      },
      {
        "light grid accent 5",
        "Light Grid Accent 5"
      },
      {
        "light grid accent 6",
        "Light Grid Accent 6"
      },
      {
        "medium shading 1",
        "Medium Shading 1"
      },
      {
        "medium shading 1 accent 1",
        "Medium Shading 1 Accent 1"
      },
      {
        "medium shading 1 accent 2",
        "Medium Shading 1 Accent 2"
      },
      {
        "medium shading 1 accent 3",
        "Medium Shading 1 Accent 3"
      },
      {
        "medium shading 1 accent 4",
        "Medium Shading 1 Accent 4"
      },
      {
        "medium shading 1 accent 5",
        "Medium Shading 1 Accent 5"
      },
      {
        "medium shading 1 accent 6",
        "Medium Shading 1 Accent 6"
      },
      {
        "medium shading 2",
        "Medium Shading 2"
      },
      {
        "medium shading 2 accent 1",
        "Medium Shading 2 Accent 1"
      },
      {
        "medium shading 2 accent 2",
        "Medium Shading 2 Accent 2"
      },
      {
        "medium shading 2 accent 3",
        "Medium Shading 2 Accent 3"
      },
      {
        "medium shading 2 accent 4",
        "Medium Shading 2 Accent 4"
      },
      {
        "medium shading 2 accent 5",
        "Medium Shading 2 Accent 5"
      },
      {
        "medium shading 2 accent 6",
        "Medium Shading 2 Accent 6"
      },
      {
        "medium list 1",
        "Medium List 1"
      },
      {
        "medium list 1 accent 1",
        "Medium List 1 Accent 1"
      },
      {
        "medium list 1 accent 2",
        "Medium List 1 Accent 2"
      },
      {
        "medium list 1 accent 3",
        "Medium List 1 Accent 3"
      },
      {
        "medium list 1 accent 4",
        "Medium List 1 Accent 4"
      },
      {
        "medium list 1 accent 5",
        "Medium List 1 Accent 5"
      },
      {
        "medium list 1 accent 6",
        "Medium List 1 Accent 6"
      },
      {
        "medium list 2",
        "Medium List 2"
      },
      {
        "medium list 2 accent 1",
        "Medium List 2 Accent 1"
      },
      {
        "medium list 2 accent 2",
        "Medium List 2 Accent 2"
      },
      {
        "medium list 2 accent 3",
        "Medium List 2 Accent 3"
      },
      {
        "medium list 2 accent 4",
        "Medium List 2 Accent 4"
      },
      {
        "medium list 2 accent 5",
        "Medium List 2 Accent 5"
      },
      {
        "medium list 2 accent 6",
        "Medium List 2 Accent 6"
      },
      {
        "medium grid 1",
        "Medium Grid 1"
      },
      {
        "medium grid 1 accent 1",
        "Medium Grid 1 Accent 1"
      },
      {
        "medium grid 1 accent 2",
        "Medium Grid 1 Accent 2"
      },
      {
        "medium grid 1 accent 3",
        "Medium Grid 1 Accent 3"
      },
      {
        "medium grid 1 accent 4",
        "Medium Grid 1 Accent 4"
      },
      {
        "medium grid 1 accent 5",
        "Medium Grid 1 Accent 5"
      },
      {
        "medium grid 1 accent 6",
        "Medium Grid 1 Accent 6"
      },
      {
        "medium grid 2",
        "Medium Grid 2"
      },
      {
        "medium grid 2 accent 1",
        "Medium Grid 2 Accent 1"
      },
      {
        "medium grid 2 accent 2",
        "Medium Grid 2 Accent 2"
      },
      {
        "medium grid 2 accent 3",
        "Medium Grid 2 Accent 3"
      },
      {
        "medium grid 2 accent 4",
        "Medium Grid 2 Accent 4"
      },
      {
        "medium grid 2 accent 5",
        "Medium Grid 2 Accent 5"
      },
      {
        "medium grid 2 accent 6",
        "Medium Grid 2 Accent 6"
      },
      {
        "medium grid 3",
        "Medium Grid 3"
      },
      {
        "medium grid 3 accent 1",
        "Medium Grid 3 Accent 1"
      },
      {
        "medium grid 3 accent 2",
        "Medium Grid 3 Accent 2"
      },
      {
        "medium grid 3 accent 3",
        "Medium Grid 3 Accent 3"
      },
      {
        "medium grid 3 accent 4",
        "Medium Grid 3 Accent 4"
      },
      {
        "medium grid 3 accent 5",
        "Medium Grid 3 Accent5"
      },
      {
        "medium grid 3 accent 6",
        "Medium Grid 3 Accent 6"
      },
      {
        "dark list",
        "Dark List"
      },
      {
        "dark list accent 1",
        "Dark List Accent 1"
      },
      {
        "dark list accent 2",
        "Dark List Accent 2"
      },
      {
        "dark list accent 3",
        "Dark List Accent 3"
      },
      {
        "dark list accent 4",
        "Dark List Accent 4"
      },
      {
        "dark list accent 5",
        "Dark List Accent 5"
      },
      {
        "dark list accent 6",
        "Dark List Accent 6"
      },
      {
        "colorful shading",
        "Colorful Shading"
      },
      {
        "colorful shading accent 1",
        "Colorful Shading Accent 1"
      },
      {
        "colorful shading accent 2",
        "Colorful Shading Accent 2"
      },
      {
        "colorful shading accent 3",
        "Colorful Shading Accent 3"
      },
      {
        "colorful shading accent 4",
        "Colorful Shading Accent 4"
      },
      {
        "colorful shading accent 5",
        "Colorful Shading Accent 5"
      },
      {
        "colorful shading accent 6",
        "Colorful Shading Accent 6"
      },
      {
        "colorful list",
        "Colorful List"
      },
      {
        "colorful list accent 1",
        "Colorful List Accent 1"
      },
      {
        "colorful list accent 2",
        "Colorful List Accent 2"
      },
      {
        "colorful list accent 3",
        "Colorful List Accent 3"
      },
      {
        "colorful list accent 4",
        "Colorful List Accent 4"
      },
      {
        "colorful list accent 5",
        "Colorful List Accent 5"
      },
      {
        "colorful list accent 6",
        "Colorful List Accent 6"
      },
      {
        "colorful grid",
        "Colorful Grid"
      },
      {
        "colorful grid accent 1",
        "Colorful Grid Accent 1"
      },
      {
        "colorful grid accent 2",
        "Colorful Grid Accent 2"
      },
      {
        "colorful grid accent 3",
        "Colorful Grid Accent 3"
      },
      {
        "colorful grid accent 4",
        "Colorful Grid Accent 4"
      },
      {
        "colorful grid accent 5",
        "Colorful Grid Accent 5"
      },
      {
        "colorful grid accent 6",
        "Colorful Grid Accent 6"
      },
      {
        "table 3d effects 1",
        "Table 3D effects 1"
      },
      {
        "table 3d effects 2",
        "Table 3D effects 2"
      },
      {
        "table 3d effects 3",
        "Table 3D effects 3"
      },
      {
        "table classic 1",
        "Table Classic 1"
      },
      {
        "table classic 2",
        "Table Classic 2"
      },
      {
        "table classic 3",
        "Table Classic 3"
      },
      {
        "table classic 4",
        "Table Classic 4"
      },
      {
        "table colorful 1",
        "Table Colorful 1"
      },
      {
        "table colorful 2",
        "Table Colorful 2"
      },
      {
        "table colorful 3",
        "Table Colorful 3"
      },
      {
        "table columns 1",
        "Table Columns 1"
      },
      {
        "table columns 2",
        "Table Columns 2"
      },
      {
        "table columns 3",
        "Table Columns 3"
      },
      {
        "table columns 4",
        "Table Columns 4"
      },
      {
        "table columns 5",
        "Table Columns 5"
      },
      {
        "table contemporary",
        "Table Contemporary"
      },
      {
        "table elegant",
        "Table Elegant"
      },
      {
        "table grid 1",
        "Table Grid 1"
      },
      {
        "table grid 2",
        "Table Grid 2"
      },
      {
        "table grid 3",
        "Table Grid 3"
      },
      {
        "table grid 4",
        "Table Grid 4"
      },
      {
        "table grid 5",
        "Table Grid 5"
      },
      {
        "table grid 6",
        "Table Grid 6"
      },
      {
        "table grid 7",
        "Table Grid 7"
      },
      {
        "table grid 8",
        "Table Grid 8"
      },
      {
        "table list 1",
        "Table List 1"
      },
      {
        "table list 2",
        "Table List 2"
      },
      {
        "table list 3",
        "Table List 3"
      },
      {
        "table list 4",
        "Table List 4"
      },
      {
        "table list 5",
        "Table List 5"
      },
      {
        "table list 6",
        "Table List 6"
      },
      {
        "table list 7",
        "Table List 7"
      },
      {
        "table list 8",
        "Table List 8"
      },
      {
        "table professional",
        "Table Professional"
      },
      {
        "table simple 1",
        "Table Simple 1"
      },
      {
        "table simple 2",
        "Table Simple 2"
      },
      {
        "table simple 3",
        "Table Simple 3"
      },
      {
        "table subtle 1",
        "Table Subtle 1"
      },
      {
        "table subtle 2",
        "Table Subtle 2"
      },
      {
        "table theme",
        "Table Theme"
      },
      {
        "table web 1",
        "Table Web 1"
      },
      {
        "table web 2",
        "Table Web 2"
      },
      {
        "table web 3",
        "Table Web 3"
      }
    };
  }

  internal Dictionary<string, int> GetBuiltinStyleIds()
  {
    return new Dictionary<string, int>()
    {
      {
        "normal",
        0
      },
      {
        "defaultparagraphfont",
        65
      },
      {
        "nospacing",
        157
      },
      {
        "heading1",
        1
      },
      {
        "heading2",
        2
      },
      {
        "heading3",
        3
      },
      {
        "heading4",
        4
      },
      {
        "heading5",
        5
      },
      {
        "heading6",
        6
      },
      {
        "heading7",
        7
      },
      {
        "heading8",
        8
      },
      {
        "heading9",
        9
      },
      {
        "title",
        62
      },
      {
        "subtitle",
        74
      },
      {
        "subtleemphasis",
        260
      },
      {
        "emphasis",
        88
      },
      {
        "intenseemphasis",
        261
      },
      {
        "strong",
        87
      },
      {
        "quote",
        180
      },
      {
        "intensequote",
        181
      },
      {
        "subtlereference",
        262
      },
      {
        "intensereference",
        263
      },
      {
        "booktitle",
        264
      },
      {
        "listparagraph",
        179
      },
      {
        "caption",
        34
      },
      {
        "bibliography",
        265
      },
      {
        "toc1",
        19
      },
      {
        "toc2",
        20
      },
      {
        "toc3",
        21
      },
      {
        "toc4",
        22
      },
      {
        "toc5",
        23
      },
      {
        "toc6",
        24
      },
      {
        "toc7",
        25
      },
      {
        "toc8",
        26
      },
      {
        "toc9",
        27
      },
      {
        "tocheading",
        266
      },
      {
        "tablegrid",
        154
      },
      {
        "lightshading",
        158
      },
      {
        "lightshadingaccent1",
        172
      },
      {
        "lightshadingaccent2",
        190
      },
      {
        "lightshadingaccent3",
        204
      },
      {
        "lightshadingaccent4",
        218
      },
      {
        "lightshadingaccent5",
        232
      },
      {
        "lightshadingaccent6",
        246
      },
      {
        "lightlist",
        159
      },
      {
        "lightlistaccent1",
        173
      },
      {
        "lightlistaccent2",
        191
      },
      {
        "lightlistaccent3",
        205
      },
      {
        "lightlistaccent4",
        219
      },
      {
        "lightlistaccent5",
        233
      },
      {
        "lightlistaccent6",
        247
      },
      {
        "lightgrid",
        160 /*0xA0*/
      },
      {
        "lightgridaccent1",
        174
      },
      {
        "lightgridaccent2",
        192 /*0xC0*/
      },
      {
        "lightgridaccent3",
        206
      },
      {
        "lightgridaccent4",
        220
      },
      {
        "lightgridaccent5",
        234
      },
      {
        "lightgridaccent6",
        248
      },
      {
        "mediumshading1",
        161
      },
      {
        "mediumshading1accent1",
        175
      },
      {
        "mediumshading1accent2",
        193
      },
      {
        "mediumshading1accent3",
        207
      },
      {
        "mediumshading1accent4",
        221
      },
      {
        "mediumshading1accent5",
        235
      },
      {
        "mediumshading1accent6",
        249
      },
      {
        "mediumshading2",
        162
      },
      {
        "mediumshading2accent1",
        176 /*0xB0*/
      },
      {
        "mediumshading2accent2",
        194
      },
      {
        "mediumshading2accent3",
        208 /*0xD0*/
      },
      {
        "mediumshading2accent4",
        222
      },
      {
        "mediumshading2accent5",
        236
      },
      {
        "mediumshading2accent6",
        250
      },
      {
        "mediumlist1",
        163
      },
      {
        "mediumlist1accent1",
        177
      },
      {
        "mediumlist1accent2",
        195
      },
      {
        "mediumlist1accent3",
        209
      },
      {
        "mediumlist1accent4",
        223
      },
      {
        "mediumlist1accent5",
        237
      },
      {
        "mediumlist1accent6",
        251
      },
      {
        "mediumlist2",
        164
      },
      {
        "mediumlist2accent1",
        182
      },
      {
        "mediumlist2accent2",
        196
      },
      {
        "mediumlist2accent3",
        210
      },
      {
        "mediumlist2accent4",
        224 /*0xE0*/
      },
      {
        "mediumlist2accent5",
        238
      },
      {
        "mediumlist2accent6",
        252
      },
      {
        "mediumgrid1",
        165
      },
      {
        "mediumgrid1accent1",
        183
      },
      {
        "mediumgrid1accent2",
        197
      },
      {
        "mediumgrid1accent3",
        211
      },
      {
        "mediumgrid1accent4",
        225
      },
      {
        "mediumgrid1accent5",
        239
      },
      {
        "mediumgrid1accent6",
        253
      },
      {
        "mediumgrid2",
        166
      },
      {
        "mediumgrid2accent1",
        184
      },
      {
        "mediumgrid2accent2",
        198
      },
      {
        "mediumgrid2accent3",
        212
      },
      {
        "mediumgrid2accent4",
        226
      },
      {
        "mediumgrid2accent5",
        240 /*0xF0*/
      },
      {
        "mediumgrid2accent6",
        254
      },
      {
        "mediumgrid3",
        167
      },
      {
        "mediumgrid3accent1",
        185
      },
      {
        "mediumgrid3accent2",
        199
      },
      {
        "mediumgrid3accent3",
        213
      },
      {
        "mediumgrid3accent4",
        227
      },
      {
        "mediumgrid3accent5",
        241
      },
      {
        "mediumgrid3accent6",
        (int) byte.MaxValue
      },
      {
        "darklist",
        168
      },
      {
        "darklistaccent1",
        186
      },
      {
        "darklistaccent2",
        200
      },
      {
        "darklistaccent3",
        214
      },
      {
        "darklistaccent4",
        228
      },
      {
        "darklistaccent5",
        242
      },
      {
        "darklistaccent6",
        256 /*0x0100*/
      },
      {
        "colorfulshading",
        169
      },
      {
        "colorfulshadingaccent1 ",
        187
      },
      {
        "colorfulshadingaccent2",
        201
      },
      {
        "colorfulshadingaccent3 ",
        215
      },
      {
        "colorfulshadingaccent4",
        229
      },
      {
        "colorfulshadingaccent5",
        243
      },
      {
        "colorfulshadingaccent6",
        257
      },
      {
        "colorfullist",
        170
      },
      {
        "colorfullistaccent1",
        188
      },
      {
        "colorfullistaccent2",
        202
      },
      {
        "colorfullistaccent3",
        216
      },
      {
        "colorfullistaccent4",
        230
      },
      {
        "colorfullistaccent5",
        244
      },
      {
        "colorfullistaccent6",
        258
      },
      {
        "colorfulgrid",
        171
      },
      {
        "colorfulgridaccent1",
        189
      },
      {
        "colorfulgridaccent2",
        203
      },
      {
        "colorfulgridaccent3",
        217
      },
      {
        "colorfulgridaccent4",
        231
      },
      {
        "colorfulgridaccent5",
        245
      },
      {
        "colorfulgridaccent6",
        259
      },
      {
        "balloontext",
        153
      },
      {
        "blocktext",
        84
      },
      {
        "bodytext",
        66
      },
      {
        "bodytext2",
        80 /*0x50*/
      },
      {
        "bodytext3",
        81
      },
      {
        "bodytextfirstindent",
        77
      },
      {
        "bodytextfirstindent2",
        78
      },
      {
        "bodytextindent",
        67
      },
      {
        "bodytextindent2",
        82
      },
      {
        "bodytextindent3",
        83
      },
      {
        "closing",
        63 /*0x3F*/
      },
      {
        "commentreference",
        39
      },
      {
        "commentsubject",
        106
      },
      {
        "commenttext",
        30
      },
      {
        "date",
        76
      },
      {
        "documentmap",
        89
      },
      {
        "e-mailsignature",
        91
      },
      {
        "endnotereference",
        42
      },
      {
        "endnotetext",
        43
      },
      {
        "envelopeaddress",
        36
      },
      {
        "envelopereturn",
        37
      },
      {
        "followedhyperlink",
        86
      },
      {
        "footer",
        32 /*0x20*/
      },
      {
        "footnotereference",
        38
      },
      {
        "footnotetext",
        29
      },
      {
        "header",
        31 /*0x1F*/
      },
      {
        "htmlacronym",
        95
      },
      {
        "htmladdress",
        96 /*0x60*/
      },
      {
        "htmlcite",
        97
      },
      {
        "htmlcode",
        98
      },
      {
        "htmldefinition",
        99
      },
      {
        "htmlkeyboard",
        100
      },
      {
        "htmlpreformatted",
        101
      },
      {
        "htmlsample",
        102
      },
      {
        "htmltypewriter",
        103
      },
      {
        "htmlvariable",
        104
      },
      {
        "hyperlink",
        85
      },
      {
        "index1",
        10
      },
      {
        "index2",
        11
      },
      {
        "index3",
        12
      },
      {
        "index4",
        13
      },
      {
        "index5",
        14
      },
      {
        "index6",
        15
      },
      {
        "index7",
        16 /*0x10*/
      },
      {
        "index8",
        17
      },
      {
        "index9",
        18
      },
      {
        "indexheading",
        33
      },
      {
        "linenumber",
        40
      },
      {
        "list",
        47
      },
      {
        "list2",
        50
      },
      {
        "list3",
        51
      },
      {
        "list4",
        52
      },
      {
        "list5",
        53
      },
      {
        "listbullet",
        48 /*0x30*/
      },
      {
        "listbullet2",
        54
      },
      {
        "listbullet3",
        55
      },
      {
        "listbullet4",
        56
      },
      {
        "listbullet5",
        57
      },
      {
        "listcontinue",
        68
      },
      {
        "listcontinue2",
        69
      },
      {
        "listcontinue3",
        70
      },
      {
        "listcontinue4",
        71
      },
      {
        "listcontinue5",
        72
      },
      {
        "listnumber",
        49
      },
      {
        "listnumber2",
        58
      },
      {
        "listnumber3",
        59
      },
      {
        "listnumber4",
        60
      },
      {
        "listnumber5",
        61
      },
      {
        "macrotext",
        45
      },
      {
        "messageheader",
        73
      },
      {
        "nolist",
        107
      },
      {
        "normal(web)",
        94
      },
      {
        "normalindent",
        28
      },
      {
        "noteheading",
        79
      },
      {
        "pagenumber",
        41
      },
      {
        "placeholdertext",
        156
      },
      {
        "plaintext",
        90
      },
      {
        "salutation",
        75
      },
      {
        "signature",
        64 /*0x40*/
      },
      {
        "table3deffects1",
        142
      },
      {
        "table3deffects2",
        143
      },
      {
        "table3deffects3",
        144 /*0x90*/
      },
      {
        "tableclassic1",
        114
      },
      {
        "tableclassic2",
        115
      },
      {
        "tableclassic3",
        116
      },
      {
        "tableclassic4",
        117
      },
      {
        "tablecolorful1",
        118
      },
      {
        "tablecolorful2",
        119
      },
      {
        "tablecolorful3",
        120
      },
      {
        "tablecolumns1",
        121
      },
      {
        "tablecolumns2",
        122
      },
      {
        "tablecolumns3",
        123
      },
      {
        "tablecolumns4",
        124
      },
      {
        "tablecolumns5",
        125
      },
      {
        "tablecontemporary",
        145
      },
      {
        "tableelegant",
        146
      },
      {
        "tablegrid1",
        126
      },
      {
        "tablegrid2",
        (int) sbyte.MaxValue
      },
      {
        "tablegrid3",
        128 /*0x80*/
      },
      {
        "tablegrid4",
        129
      },
      {
        "tablegrid5",
        130
      },
      {
        "tablegrid6",
        131
      },
      {
        "tablegrid7",
        132
      },
      {
        "tablegrid8",
        133
      },
      {
        "tablelist1",
        134
      },
      {
        "tablelist2",
        135
      },
      {
        "tablelist3",
        136
      },
      {
        "tablelist4",
        137
      },
      {
        "tablelist5",
        138
      },
      {
        "tablelist6",
        139
      },
      {
        "tablelist7",
        140
      },
      {
        "tablelist8",
        141
      },
      {
        "tablenormal",
        105
      },
      {
        "normaltable",
        105
      },
      {
        "tableofauthorities",
        44
      },
      {
        "tableoffigures",
        35
      },
      {
        "tableprofessional",
        147
      },
      {
        "tablesimple1",
        111
      },
      {
        "tablesimple2",
        112 /*0x70*/
      },
      {
        "tablesimple3",
        113
      },
      {
        "tablesubtle1",
        148
      },
      {
        "tablesubtle2",
        149
      },
      {
        "tabletheme",
        155
      },
      {
        "tableweb1",
        150
      },
      {
        "tableweb2",
        151
      },
      {
        "tableweb3",
        152
      },
      {
        "toaheading",
        46
      },
      {
        "htmltopofform",
        92
      },
      {
        "htmlbottomofform",
        93
      },
      {
        "revision",
        178
      },
      {
        "outlinelist1",
        108
      },
      {
        "outlinelist2",
        109
      },
      {
        "outlinelist3",
        110
      }
    };
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    writer.WriteValue("Name", this.Name);
    writer.WriteValue("StyleId", this.m_styleId);
    writer.WriteValue("type", (Enum) this.StyleType);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    this.m_strName = reader.ReadString("Name");
    this.m_styleId = reader.ReadInt("StyleId");
  }

  protected override void InitXDLSHolder()
  {
    this.XDLSHolder.EnableID = true;
    this.XDLSHolder.AddRefElement("base", (object) this.m_baseStyle);
    this.XDLSHolder.AddElement("character-format", (object) this.m_chFormat);
  }

  protected override void RestoreReference(string name, int index)
  {
    if (index <= -1)
      return;
    this.m_baseStyle = this.Document.Styles[index];
  }

  public class BuiltinStyleLoader
  {
    private const string DEF_DOCIO_RESOURCES = "Syncfusion.DocIO.Resources";
    private const string DEF_STYLE_TAG = "builtin-styles";
    private const int DEF_LIST_STYLES_NUMBER = 10;
    public static readonly string[] BuiltinStyleNames = new string[106]
    {
      "Normal",
      "Heading 1",
      "Heading 2",
      "Heading 3",
      "Heading 4",
      "Heading 5",
      "Heading 6",
      "Heading 7",
      "Heading 8",
      "Heading 9",
      "Index 1",
      "Index 2",
      "Index 3",
      "Index 4",
      "Index 5",
      "Index 6",
      "Index 7",
      "Index 8",
      "Index 9",
      "TOC 1",
      "TOC 2",
      "TOC 3",
      "TOC 4",
      "TOC 5",
      "TOC 6",
      "TOC 7",
      "TOC 8",
      "TOC 9",
      "Normal Indent",
      "Footnote Text",
      "Comment Text",
      "Header",
      "Footer",
      "Index Heading",
      "Caption",
      "Table of Figures",
      "Footnote Reference",
      "Comment Reference",
      "Line Number",
      "Page Number",
      "Endnote Reference",
      "Endnote Text",
      "Table of Authorities",
      "Macro Text",
      "TOA Heading",
      "List",
      "List Bullet",
      "List Number",
      "List 2",
      "List 3",
      "List 4",
      "List 5",
      "List Bullet 2",
      "List Bullet 3",
      "List Bullet 4",
      "List Bullet 5",
      "List Number 2",
      "List Number 3",
      "List Number 4",
      "List Number 5",
      "Title",
      "Closing",
      "Signature",
      "Default Paragraph Font",
      "Body Text",
      "Body Text Indent",
      "List Continue",
      "List Continue 2",
      "List Continue 3",
      "List Continue 4",
      "List Continue 5",
      "Message Header",
      "Subtitle",
      "Salutation",
      "Date",
      "Body Text First Indent",
      "Body Text First Indent 2",
      "Note Heading",
      "Body Text 2",
      "Body Text 3",
      "Body Text Indent 2",
      "Body Text Indent 3",
      "Block Text",
      "Hyperlink",
      "FollowedHyperlink",
      "Strong",
      "Emphasis",
      "Document Map",
      "Plain Text",
      "E-mail Signature",
      "Normal (Web)",
      "HTML Acronym",
      "HTML Address",
      "HTML Cite",
      "HTML Code",
      "HTML Definition",
      "HTML Keyboard",
      "HTML Preformatted",
      "HTML Sample",
      "HTML Typewriter",
      "HTML Variable",
      "Comment Subject",
      "No List",
      "Balloon Text",
      "User",
      "NoStyle"
    };
    internal static readonly string[] BuiltinTableStyleNames = new string[143]
    {
      "Normal Table",
      "Table Grid",
      "Light Shading",
      "Light Shading Accent 1",
      "Light Shading Accent 2",
      "Light Shading Accent 3",
      "Light Shading Accent 4",
      "Light Shading Accent 5",
      "Light Shading Accent 6",
      "Light List",
      "Light List Accent 1",
      "Light List Accent 2",
      "Light List Accent 3",
      "Light List Accent 4",
      "Light List Accent 5",
      "Light List Accent 6",
      "Light Grid",
      "Light Grid Accent 1",
      "Light Grid Accent 2",
      "Light Grid Accent 3",
      "Light Grid Accent 4",
      "Light Grid Accent 5",
      "Light Grid Accent 6",
      "Medium Shading 1",
      "Medium Shading 1 Accent 1",
      "Medium Shading 1 Accent 2",
      "Medium Shading 1 Accent 3",
      "Medium Shading 1 Accent 4",
      "Medium Shading 1 Accent 5",
      "Medium Shading 1 Accent 6",
      "Medium Shading 2",
      "Medium Shading 2 Accent 1",
      "Medium Shading 2 Accent 2",
      "Medium Shading 2 Accent 3",
      "Medium Shading 2 Accent 4",
      "Medium Shading 2 Accent 5",
      "Medium Shading 2 Accent 6",
      "Medium List 1",
      "Medium List 1 Accent 1",
      "Medium List 1 Accent 2",
      "Medium List 1 Accent 3",
      "Medium List 1 Accent 4",
      "Medium List 1 Accent 5",
      "Medium List 1 Accent 6",
      "Medium List 2",
      "Medium List 2 Accent 1",
      "Medium List 2 Accent 2",
      "Medium List 2 Accent 3",
      "Medium List 2 Accent 4",
      "Medium List 2 Accent 5",
      "Medium List 2 Accent 6",
      "Medium Grid 1",
      "Medium Grid 1 Accent 1",
      "Medium Grid 1 Accent 2",
      "Medium Grid 1 Accent 3",
      "Medium Grid 1 Accent 4",
      "Medium Grid 1 Accent 5",
      "Medium Grid 1 Accent 6",
      "Medium Grid 2",
      "Medium Grid 2 Accent 1",
      "Medium Grid 2 Accent 2",
      "Medium Grid 2 Accent 3",
      "Medium Grid 2 Accent 4",
      "Medium Grid 2 Accent 5",
      "Medium Grid 2 Accent 6",
      "Medium Grid 3",
      "Medium Grid 3 Accent 1",
      "Medium Grid 3 Accent 2",
      "Medium Grid 3 Accent 3",
      "Medium Grid 3 Accent 4",
      "Medium Grid 3 Accent5",
      "Medium Grid 3 Accent 6",
      "Dark List",
      "Dark List Accent 1",
      "Dark List Accent 2",
      "Dark List Accent 3",
      "Dark List Accent 4",
      "Dark List Accent 5",
      "Dark List Accent 6",
      "Colorful Shading",
      "Colorful Shading Accent 1",
      "Colorful Shading Accent 2",
      "Colorful Shading Accent 3",
      "Colorful Shading Accent 4",
      "Colorful Shading Accent 5",
      "Colorful Shading Accent 6",
      "Colorful List",
      "Colorful List Accent 1",
      "Colorful List Accent 2",
      "Colorful List Accent 3",
      "Colorful List Accent 4",
      "Colorful List Accent 5",
      "Colorful List Accent 6",
      "Colorful Grid",
      "Colorful Grid Accent 1",
      "Colorful Grid Accent 2",
      "Colorful Grid Accent 3",
      "Colorful Grid Accent 4",
      "Colorful Grid Accent 5",
      "Colorful Grid Accent 6",
      "Table 3D effects 1",
      "Table 3D effects 2",
      "Table 3D effects 3",
      "Table Classic 1",
      "Table Classic 2",
      "Table Classic 3",
      "Table Classic 4",
      "Table Colorful 1",
      "Table Colorful 2",
      "Table Colorful 3",
      "Table Columns 1",
      "Table Columns 2",
      "Table Columns 3",
      "Table Columns 4",
      "Table Columns 5",
      "Table Contemporary",
      "Table Elegant",
      "Table Grid 1",
      "Table Grid 2",
      "Table Grid 3",
      "Table Grid 4",
      "Table Grid 5",
      "Table Grid 6",
      "Table Grid 7",
      "Table Grid 8",
      "Table List 1",
      "Table List 2",
      "Table List 3",
      "Table List 4",
      "Table List 5",
      "Table List 6",
      "Table List 7",
      "Table List 8",
      "Table Professional",
      "Table Simple 1",
      "Table Simple 2",
      "Table Simple 3",
      "Table Subtle 1",
      "Table Subtle 2",
      "Table Theme",
      "Table Web 1",
      "Table Web 2",
      "Table Web 3"
    };

    internal static void LoadStyle(IStyle style, BuiltinStyle bStyle)
    {
      Stream input = Style.BuiltinStyleLoader.UpdateXMLResAndReader();
      input.Position = 0L;
      XmlReader reader = (XmlReader) new XmlTextReader(input);
      while (reader.Name != "builtin-styles")
        reader.Read();
      reader.Read();
      string name = Style.BuiltInToName(bStyle);
      string empty = string.Empty;
      while (!reader.EOF)
      {
        if (reader.NodeType == XmlNodeType.Element)
        {
          if (reader.GetAttribute("Name") == name)
          {
            new XDLSReader(reader).ReadChildElement((object) style);
            break;
          }
          reader.Skip();
        }
        else
          reader.Read();
      }
    }

    internal static void LoadStyle(IStyle style, BuiltinTableStyle bStyle)
    {
      style.Name = Style.BuiltInToName(bStyle);
      switch (bStyle)
      {
        case BuiltinTableStyle.TableNormal:
          Style.BuiltinStyleLoader.LoadStyleTableNormal(style);
          break;
        case BuiltinTableStyle.TableGrid:
          Style.BuiltinStyleLoader.LoadStyleTableGrid(style);
          break;
        case BuiltinTableStyle.LightShading:
          Style.BuiltinStyleLoader.LoadStyleLightShading(style, Color.Black, Color.FromArgb((int) byte.MaxValue, 0, 0, 0), Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/));
          break;
        case BuiltinTableStyle.LightShadingAccent1:
          Style.BuiltinStyleLoader.LoadStyleLightShading(style, Color.FromArgb((int) byte.MaxValue, 54, 95, 145), Color.FromArgb((int) byte.MaxValue, 79, 129, 189), Color.FromArgb((int) byte.MaxValue, 211, 223, 238));
          break;
        case BuiltinTableStyle.LightShadingAccent2:
          Style.BuiltinStyleLoader.LoadStyleLightShading(style, Color.FromArgb((int) byte.MaxValue, 148, 54, 52), Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 80 /*0x50*/, 77), Color.FromArgb((int) byte.MaxValue, 239, 211, 210));
          break;
        case BuiltinTableStyle.LightShadingAccent3:
          Style.BuiltinStyleLoader.LoadStyleLightShading(style, Color.FromArgb((int) byte.MaxValue, 118, 146, 60), Color.FromArgb((int) byte.MaxValue, 155, 187, 89), Color.FromArgb((int) byte.MaxValue, 230, 238, 213));
          break;
        case BuiltinTableStyle.LightShadingAccent4:
          Style.BuiltinStyleLoader.LoadStyleLightShading(style, Color.FromArgb((int) byte.MaxValue, 95, 73, 122), Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 100, 162), Color.FromArgb((int) byte.MaxValue, 223, 216, 232));
          break;
        case BuiltinTableStyle.LightShadingAccent5:
          Style.BuiltinStyleLoader.LoadStyleLightShading(style, Color.FromArgb((int) byte.MaxValue, 49, 132, 155), Color.FromArgb((int) byte.MaxValue, 75, 172, 198), Color.FromArgb((int) byte.MaxValue, 210, 234, 241));
          break;
        case BuiltinTableStyle.LightShadingAccent6:
          Style.BuiltinStyleLoader.LoadStyleLightShading(style, Color.FromArgb((int) byte.MaxValue, 227, 108, 10), Color.FromArgb((int) byte.MaxValue, 247, 150, 70), Color.FromArgb((int) byte.MaxValue, 253, 228, 208 /*0xD0*/));
          break;
        case BuiltinTableStyle.LightList:
          Style.BuiltinStyleLoader.LoadStyleLightList(style, Color.FromArgb((int) byte.MaxValue, 0, 0, 0), Color.Black);
          break;
        case BuiltinTableStyle.LightListAccent1:
          Style.BuiltinStyleLoader.LoadStyleLightList(style, Color.FromArgb((int) byte.MaxValue, 79, 129, 189), Color.FromArgb((int) byte.MaxValue, 79, 129, 189));
          break;
        case BuiltinTableStyle.LightListAccent2:
          Style.BuiltinStyleLoader.LoadStyleLightList(style, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 80 /*0x50*/, 77), Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 80 /*0x50*/, 77));
          break;
        case BuiltinTableStyle.LightListAccent3:
          Style.BuiltinStyleLoader.LoadStyleLightList(style, Color.FromArgb((int) byte.MaxValue, 155, 187, 89), Color.FromArgb((int) byte.MaxValue, 155, 187, 89));
          break;
        case BuiltinTableStyle.LightListAccent4:
          Style.BuiltinStyleLoader.LoadStyleLightList(style, Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 100, 162), Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 100, 162));
          break;
        case BuiltinTableStyle.LightListAccent5:
          Style.BuiltinStyleLoader.LoadStyleLightList(style, Color.FromArgb((int) byte.MaxValue, 75, 172, 198), Color.FromArgb((int) byte.MaxValue, 75, 172, 198));
          break;
        case BuiltinTableStyle.LightListAccent6:
          Style.BuiltinStyleLoader.LoadStyleLightList(style, Color.FromArgb((int) byte.MaxValue, 247, 150, 70), Color.FromArgb((int) byte.MaxValue, 247, 150, 70));
          break;
        case BuiltinTableStyle.LightGrid:
          Style.BuiltinStyleLoader.LoadStyleLightGrid(style, Color.FromArgb((int) byte.MaxValue, 0, 0, 0), Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/));
          break;
        case BuiltinTableStyle.LightGridAccent1:
          Style.BuiltinStyleLoader.LoadStyleLightGrid(style, Color.FromArgb((int) byte.MaxValue, 79, 129, 189), Color.FromArgb((int) byte.MaxValue, 211, 223, 238));
          break;
        case BuiltinTableStyle.LightGridAccent2:
          Style.BuiltinStyleLoader.LoadStyleLightGrid(style, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 80 /*0x50*/, 77), Color.FromArgb((int) byte.MaxValue, 239, 211, 210));
          break;
        case BuiltinTableStyle.LightGridAccent3:
          Style.BuiltinStyleLoader.LoadStyleLightGrid(style, Color.FromArgb((int) byte.MaxValue, 155, 187, 89), Color.FromArgb((int) byte.MaxValue, 230, 238, 213));
          break;
        case BuiltinTableStyle.LightGridAccent4:
          Style.BuiltinStyleLoader.LoadStyleLightGrid(style, Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 100, 162), Color.FromArgb((int) byte.MaxValue, 223, 216, 232));
          break;
        case BuiltinTableStyle.LightGridAccent5:
          Style.BuiltinStyleLoader.LoadStyleLightGrid(style, Color.FromArgb((int) byte.MaxValue, 75, 172, 198), Color.FromArgb((int) byte.MaxValue, 210, 234, 241));
          break;
        case BuiltinTableStyle.LightGridAccent6:
          Style.BuiltinStyleLoader.LoadStyleLightGrid(style, Color.FromArgb((int) byte.MaxValue, 247, 150, 70), Color.FromArgb((int) byte.MaxValue, 253, 228, 208 /*0xD0*/));
          break;
        case BuiltinTableStyle.MediumShading1:
          Style.BuiltinStyleLoader.LoadStyleMediumShading1(style, Color.FromArgb((int) byte.MaxValue, 64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/), Color.Black, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/));
          break;
        case BuiltinTableStyle.MediumShading1Accent1:
          Style.BuiltinStyleLoader.LoadStyleMediumShading1(style, Color.FromArgb((int) byte.MaxValue, 123, 160 /*0xA0*/, 205), Color.FromArgb((int) byte.MaxValue, 79, 129, 189), Color.FromArgb((int) byte.MaxValue, 211, 223, 238));
          break;
        case BuiltinTableStyle.MediumShading1Accent2:
          Style.BuiltinStyleLoader.LoadStyleMediumShading1(style, Color.FromArgb((int) byte.MaxValue, 207, 123, 121), Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 80 /*0x50*/, 77), Color.FromArgb((int) byte.MaxValue, 239, 211, 210));
          break;
        case BuiltinTableStyle.MediumShading1Accent3:
          Style.BuiltinStyleLoader.LoadStyleMediumShading1(style, Color.FromArgb((int) byte.MaxValue, 179, 204, 130), Color.FromArgb((int) byte.MaxValue, 155, 187, 89), Color.FromArgb((int) byte.MaxValue, 230, 238, 213));
          break;
        case BuiltinTableStyle.MediumShading1Accent4:
          Style.BuiltinStyleLoader.LoadStyleMediumShading1(style, Color.FromArgb((int) byte.MaxValue, 159, 138, 185), Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 100, 162), Color.FromArgb((int) byte.MaxValue, 223, 216, 232));
          break;
        case BuiltinTableStyle.MediumShading1Accent5:
          Style.BuiltinStyleLoader.LoadStyleMediumShading1(style, Color.FromArgb((int) byte.MaxValue, 120, 192 /*0xC0*/, 212), Color.FromArgb((int) byte.MaxValue, 75, 172, 198), Color.FromArgb((int) byte.MaxValue, 210, 234, 241));
          break;
        case BuiltinTableStyle.MediumShading1Accent6:
          Style.BuiltinStyleLoader.LoadStyleMediumShading1(style, Color.FromArgb((int) byte.MaxValue, 249, 176 /*0xB0*/, 116), Color.FromArgb((int) byte.MaxValue, 247, 150, 70), Color.FromArgb((int) byte.MaxValue, 253, 228, 208 /*0xD0*/));
          break;
        case BuiltinTableStyle.MediumShading2:
          Style.BuiltinStyleLoader.LoadStyleMediumShading2(style, Color.Black);
          break;
        case BuiltinTableStyle.MediumShading2Accent1:
          Style.BuiltinStyleLoader.LoadStyleMediumShading2(style, Color.FromArgb((int) byte.MaxValue, 79, 129, 189));
          break;
        case BuiltinTableStyle.MediumShading2Accent2:
          Style.BuiltinStyleLoader.LoadStyleMediumShading2(style, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 80 /*0x50*/, 77));
          break;
        case BuiltinTableStyle.MediumShading2Accent3:
          Style.BuiltinStyleLoader.LoadStyleMediumShading2(style, Color.FromArgb((int) byte.MaxValue, 155, 187, 89));
          break;
        case BuiltinTableStyle.MediumShading2Accent4:
          Style.BuiltinStyleLoader.LoadStyleMediumShading2(style, Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 100, 162));
          break;
        case BuiltinTableStyle.MediumShading2Accent5:
          Style.BuiltinStyleLoader.LoadStyleMediumShading2(style, Color.FromArgb((int) byte.MaxValue, 75, 172, 198));
          break;
        case BuiltinTableStyle.MediumShading2Accent6:
          Style.BuiltinStyleLoader.LoadStyleMediumShading2(style, Color.FromArgb((int) byte.MaxValue, 247, 150, 70));
          break;
        case BuiltinTableStyle.MediumList1:
          Style.BuiltinStyleLoader.LoadStyleMediumList1(style, Color.FromArgb((int) byte.MaxValue, 0, 0, 0), Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/));
          break;
        case BuiltinTableStyle.MediumList1Accent1:
          Style.BuiltinStyleLoader.LoadStyleMediumList1(style, Color.FromArgb((int) byte.MaxValue, 79, 129, 189), Color.FromArgb((int) byte.MaxValue, 211, 223, 238));
          break;
        case BuiltinTableStyle.MediumList1Accent2:
          Style.BuiltinStyleLoader.LoadStyleMediumList1(style, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 80 /*0x50*/, 77), Color.FromArgb((int) byte.MaxValue, 239, 211, 210));
          break;
        case BuiltinTableStyle.MediumList1Accent3:
          Style.BuiltinStyleLoader.LoadStyleMediumList1(style, Color.FromArgb((int) byte.MaxValue, 155, 187, 89), Color.FromArgb((int) byte.MaxValue, 230, 238, 213));
          break;
        case BuiltinTableStyle.MediumList1Accent4:
          Style.BuiltinStyleLoader.LoadStyleMediumList1(style, Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 100, 162), Color.FromArgb((int) byte.MaxValue, 223, 216, 232));
          break;
        case BuiltinTableStyle.MediumList1Accent5:
          Style.BuiltinStyleLoader.LoadStyleMediumList1(style, Color.FromArgb((int) byte.MaxValue, 75, 172, 198), Color.FromArgb((int) byte.MaxValue, 210, 234, 241));
          break;
        case BuiltinTableStyle.MediumList1Accent6:
          Style.BuiltinStyleLoader.LoadStyleMediumList1(style, Color.FromArgb((int) byte.MaxValue, 247, 150, 70), Color.FromArgb((int) byte.MaxValue, 253, 228, 208 /*0xD0*/));
          break;
        case BuiltinTableStyle.MediumList2:
          Style.BuiltinStyleLoader.LoadStyleMediumList2(style, Color.FromArgb((int) byte.MaxValue, 0, 0, 0), Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/));
          break;
        case BuiltinTableStyle.MediumList2Accent1:
          Style.BuiltinStyleLoader.LoadStyleMediumList2(style, Color.FromArgb((int) byte.MaxValue, 79, 129, 189), Color.FromArgb((int) byte.MaxValue, 211, 223, 238));
          break;
        case BuiltinTableStyle.MediumList2Accent2:
          Style.BuiltinStyleLoader.LoadStyleMediumList2(style, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 80 /*0x50*/, 77), Color.FromArgb((int) byte.MaxValue, 239, 211, 210));
          break;
        case BuiltinTableStyle.MediumList2Accent3:
          Style.BuiltinStyleLoader.LoadStyleMediumList2(style, Color.FromArgb((int) byte.MaxValue, 155, 187, 89), Color.FromArgb((int) byte.MaxValue, 230, 238, 213));
          break;
        case BuiltinTableStyle.MediumList2Accent4:
          Style.BuiltinStyleLoader.LoadStyleMediumList2(style, Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 100, 162), Color.FromArgb((int) byte.MaxValue, 223, 216, 232));
          break;
        case BuiltinTableStyle.MediumList2Accent5:
          Style.BuiltinStyleLoader.LoadStyleMediumList2(style, Color.FromArgb((int) byte.MaxValue, 75, 172, 198), Color.FromArgb((int) byte.MaxValue, 210, 234, 241));
          break;
        case BuiltinTableStyle.MediumList2Accent6:
          Style.BuiltinStyleLoader.LoadStyleMediumList2(style, Color.FromArgb((int) byte.MaxValue, 247, 150, 70), Color.FromArgb((int) byte.MaxValue, 253, 228, 208 /*0xD0*/));
          break;
        case BuiltinTableStyle.MediumGrid1:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid1(style, Color.FromArgb((int) byte.MaxValue, 64 /*0x40*/, 64 /*0x40*/, 64 /*0x40*/), Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/), Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/));
          break;
        case BuiltinTableStyle.MediumGrid1Accent1:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid1(style, Color.FromArgb((int) byte.MaxValue, 123, 160 /*0xA0*/, 205), Color.FromArgb((int) byte.MaxValue, 211, 223, 238), Color.FromArgb((int) byte.MaxValue, 167, 191, 222));
          break;
        case BuiltinTableStyle.MediumGrid1Accent2:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid1(style, Color.FromArgb((int) byte.MaxValue, 207, 123, 121), Color.FromArgb((int) byte.MaxValue, 239, 211, 210), Color.FromArgb((int) byte.MaxValue, 223, 167, 166));
          break;
        case BuiltinTableStyle.MediumGrid1Accent3:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid1(style, Color.FromArgb((int) byte.MaxValue, 179, 204, 130), Color.FromArgb((int) byte.MaxValue, 230, 238, 213), Color.FromArgb((int) byte.MaxValue, 205, 221, 172));
          break;
        case BuiltinTableStyle.MediumGrid1Accent4:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid1(style, Color.FromArgb((int) byte.MaxValue, 159, 138, 185), Color.FromArgb((int) byte.MaxValue, 223, 216, 232), Color.FromArgb((int) byte.MaxValue, 191, 177, 208 /*0xD0*/));
          break;
        case BuiltinTableStyle.MediumGrid1Accent5:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid1(style, Color.FromArgb((int) byte.MaxValue, 120, 192 /*0xC0*/, 212), Color.FromArgb((int) byte.MaxValue, 210, 234, 241), Color.FromArgb((int) byte.MaxValue, 165, 213, 226));
          break;
        case BuiltinTableStyle.MediumGrid1Accent6:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid1(style, Color.FromArgb((int) byte.MaxValue, 249, 176 /*0xB0*/, 116), Color.FromArgb((int) byte.MaxValue, 253, 228, 208 /*0xD0*/), Color.FromArgb((int) byte.MaxValue, 251, 202, 162));
          break;
        case BuiltinTableStyle.MediumGrid2:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid2(style, Color.FromArgb((int) byte.MaxValue, 0, 0, 0), Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/), Color.FromArgb((int) byte.MaxValue, 230, 230, 230), Color.FromArgb((int) byte.MaxValue, 204, 204, 204), Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/));
          break;
        case BuiltinTableStyle.MediumGrid2Accent1:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid2(style, Color.FromArgb((int) byte.MaxValue, 79, 129, 189), Color.FromArgb((int) byte.MaxValue, 211, 223, 238), Color.FromArgb((int) byte.MaxValue, 237, 242, 248), Color.FromArgb((int) byte.MaxValue, 219, 229, 241), Color.FromArgb((int) byte.MaxValue, 167, 191, 222));
          break;
        case BuiltinTableStyle.MediumGrid2Accent2:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid2(style, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 80 /*0x50*/, 77), Color.FromArgb((int) byte.MaxValue, 239, 211, 210), Color.FromArgb((int) byte.MaxValue, 248, 237, 237), Color.FromArgb((int) byte.MaxValue, 242, 219, 219), Color.FromArgb((int) byte.MaxValue, 223, 167, 166));
          break;
        case BuiltinTableStyle.MediumGrid2Accent3:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid2(style, Color.FromArgb((int) byte.MaxValue, 155, 187, 89), Color.FromArgb((int) byte.MaxValue, 230, 238, 213), Color.FromArgb((int) byte.MaxValue, 245, 248, 238), Color.FromArgb((int) byte.MaxValue, 234, 241, 221), Color.FromArgb((int) byte.MaxValue, 205, 221, 172));
          break;
        case BuiltinTableStyle.MediumGrid2Accent4:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid2(style, Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 100, 162), Color.FromArgb((int) byte.MaxValue, 223, 216, 232), Color.FromArgb((int) byte.MaxValue, 242, 239, 246), Color.FromArgb((int) byte.MaxValue, 229, 223, 236), Color.FromArgb((int) byte.MaxValue, 191, 177, 208 /*0xD0*/));
          break;
        case BuiltinTableStyle.MediumGrid2Accent5:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid2(style, Color.FromArgb((int) byte.MaxValue, 75, 172, 198), Color.FromArgb((int) byte.MaxValue, 210, 234, 241), Color.FromArgb((int) byte.MaxValue, 237, 246, 249), Color.FromArgb((int) byte.MaxValue, 218, 238, 243), Color.FromArgb((int) byte.MaxValue, 165, 213, 226));
          break;
        case BuiltinTableStyle.MediumGrid2Accent6:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid2(style, Color.FromArgb((int) byte.MaxValue, 247, 150, 70), Color.FromArgb((int) byte.MaxValue, 253, 228, 208 /*0xD0*/), Color.FromArgb((int) byte.MaxValue, 254, 244, 236), Color.FromArgb((int) byte.MaxValue, 253, 233, 217), Color.FromArgb((int) byte.MaxValue, 251, 202, 162));
          break;
        case BuiltinTableStyle.MediumGrid3:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid3(style, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/), Color.FromArgb((int) byte.MaxValue, 0, 0, 0), Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/));
          break;
        case BuiltinTableStyle.MediumGrid3Accent1:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid3(style, Color.FromArgb((int) byte.MaxValue, 211, 223, 238), Color.FromArgb((int) byte.MaxValue, 79, 129, 189), Color.FromArgb((int) byte.MaxValue, 167, 191, 222));
          break;
        case BuiltinTableStyle.MediumGrid3Accent2:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid3(style, Color.FromArgb((int) byte.MaxValue, 239, 211, 210), Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 80 /*0x50*/, 77), Color.FromArgb((int) byte.MaxValue, 223, 167, 166));
          break;
        case BuiltinTableStyle.MediumGrid3Accent3:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid3(style, Color.FromArgb((int) byte.MaxValue, 230, 238, 213), Color.FromArgb((int) byte.MaxValue, 155, 187, 89), Color.FromArgb((int) byte.MaxValue, 205, 221, 172));
          break;
        case BuiltinTableStyle.MediumGrid3Accent4:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid3(style, Color.FromArgb((int) byte.MaxValue, 223, 216, 232), Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 100, 162), Color.FromArgb((int) byte.MaxValue, 191, 177, 208 /*0xD0*/));
          break;
        case BuiltinTableStyle.MediumGrid3Accent5:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid3(style, Color.FromArgb((int) byte.MaxValue, 210, 234, 241), Color.FromArgb((int) byte.MaxValue, 75, 172, 198), Color.FromArgb((int) byte.MaxValue, 165, 213, 226));
          break;
        case BuiltinTableStyle.MediumGrid3Accent6:
          Style.BuiltinStyleLoader.LoadStyleMediumGrid3(style, Color.FromArgb((int) byte.MaxValue, 253, 228, 208 /*0xD0*/), Color.FromArgb((int) byte.MaxValue, 247, 150, 70), Color.FromArgb((int) byte.MaxValue, 251, 202, 162));
          break;
        case BuiltinTableStyle.DarkList:
          Style.BuiltinStyleLoader.LoadStyleDarkList(style, Color.FromArgb((int) byte.MaxValue, 0, 0, 0), Color.FromArgb((int) byte.MaxValue, 0, 0, 0), Color.FromArgb((int) byte.MaxValue, 0, 0, 0));
          break;
        case BuiltinTableStyle.DarkListAccent1:
          Style.BuiltinStyleLoader.LoadStyleDarkList(style, Color.FromArgb((int) byte.MaxValue, 79, 129, 189), Color.FromArgb((int) byte.MaxValue, 36, 63 /*0x3F*/, 96 /*0x60*/), Color.FromArgb((int) byte.MaxValue, 54, 95, 145));
          break;
        case BuiltinTableStyle.DarkListAccent2:
          Style.BuiltinStyleLoader.LoadStyleDarkList(style, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 80 /*0x50*/, 77), Color.FromArgb((int) byte.MaxValue, 98, 36, 35), Color.FromArgb((int) byte.MaxValue, 148, 54, 52));
          break;
        case BuiltinTableStyle.DarkListAccent3:
          Style.BuiltinStyleLoader.LoadStyleDarkList(style, Color.FromArgb((int) byte.MaxValue, 155, 187, 89), Color.FromArgb((int) byte.MaxValue, 78, 97, 40), Color.FromArgb((int) byte.MaxValue, 118, 146, 60));
          break;
        case BuiltinTableStyle.DarkListAccent4:
          Style.BuiltinStyleLoader.LoadStyleDarkList(style, Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 100, 162), Color.FromArgb((int) byte.MaxValue, 63 /*0x3F*/, 49, 81), Color.FromArgb((int) byte.MaxValue, 95, 73, 122));
          break;
        case BuiltinTableStyle.DarkListAccent5:
          Style.BuiltinStyleLoader.LoadStyleDarkList(style, Color.FromArgb((int) byte.MaxValue, 75, 172, 198), Color.FromArgb((int) byte.MaxValue, 32 /*0x20*/, 88, 103), Color.FromArgb((int) byte.MaxValue, 49, 132, 155));
          break;
        case BuiltinTableStyle.DarkListAccent6:
          Style.BuiltinStyleLoader.LoadStyleDarkList(style, Color.FromArgb((int) byte.MaxValue, 247, 150, 70), Color.FromArgb((int) byte.MaxValue, 151, 71, 6), Color.FromArgb((int) byte.MaxValue, 227, 108, 10));
          break;
        case BuiltinTableStyle.ColorfulShading:
          Style.BuiltinStyleLoader.LoadStyleColorfulShading(style, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 80 /*0x50*/, 77), Color.FromArgb((int) byte.MaxValue, 0, 0, 0), Color.FromArgb((int) byte.MaxValue, 230, 230, 230), Color.FromArgb((int) byte.MaxValue, 0, 0, 0), Color.FromArgb((int) byte.MaxValue, 153, 153, 153), Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/));
          break;
        case BuiltinTableStyle.ColorfulShadingAccent1:
          Style.BuiltinStyleLoader.LoadStyleColorfulShading(style, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 80 /*0x50*/, 77), Color.FromArgb((int) byte.MaxValue, 79, 129, 189), Color.FromArgb((int) byte.MaxValue, 237, 242, 248), Color.FromArgb((int) byte.MaxValue, 44, 76, 116), Color.FromArgb((int) byte.MaxValue, 184, 204, 228), Color.FromArgb((int) byte.MaxValue, 167, 191, 222));
          break;
        case BuiltinTableStyle.ColorfulShadingAccent2:
          Style.BuiltinStyleLoader.LoadStyleColorfulShading(style, Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 80 /*0x50*/, 77), Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 80 /*0x50*/, 77), Color.FromArgb((int) byte.MaxValue, 248, 237, 237), Color.FromArgb((int) byte.MaxValue, 119, 44, 42), Color.FromArgb((int) byte.MaxValue, 229, 184, 183), Color.FromArgb((int) byte.MaxValue, 223, 167, 166));
          break;
        case BuiltinTableStyle.ColorfulShadingAccent3:
          Style.BuiltinStyleLoader.LoadStyleColorfulShading(style, Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 100, 162), Color.FromArgb((int) byte.MaxValue, 155, 187, 89), Color.FromArgb((int) byte.MaxValue, 245, 248, 238), Color.FromArgb((int) byte.MaxValue, 94, 117, 48 /*0x30*/), Color.FromArgb((int) byte.MaxValue, 214, 227, 188), Color.FromArgb((int) byte.MaxValue, 205, 221, 172));
          break;
        case BuiltinTableStyle.ColorfulShadingAccent4:
          Style.BuiltinStyleLoader.LoadStyleColorfulShading(style, Color.FromArgb((int) byte.MaxValue, 155, 187, 89), Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 100, 162), Color.FromArgb((int) byte.MaxValue, 242, 239, 246), Color.FromArgb((int) byte.MaxValue, 76, 59, 98), Color.FromArgb((int) byte.MaxValue, 204, 192 /*0xC0*/, 217), Color.FromArgb((int) byte.MaxValue, 191, 177, 208 /*0xD0*/));
          break;
        case BuiltinTableStyle.ColorfulShadingAccent5:
          Style.BuiltinStyleLoader.LoadStyleColorfulShading(style, Color.FromArgb((int) byte.MaxValue, 247, 150, 70), Color.FromArgb((int) byte.MaxValue, 75, 172, 198), Color.FromArgb((int) byte.MaxValue, 237, 246, 249), Color.FromArgb((int) byte.MaxValue, 39, 106, 124), Color.FromArgb((int) byte.MaxValue, 182, 221, 232), Color.FromArgb((int) byte.MaxValue, 165, 213, 226));
          break;
        case BuiltinTableStyle.ColorfulShadingAccent6:
          Style.BuiltinStyleLoader.LoadStyleColorfulShading(style, Color.FromArgb((int) byte.MaxValue, 75, 172, 198), Color.FromArgb((int) byte.MaxValue, 247, 150, 70), Color.FromArgb((int) byte.MaxValue, 254, 244, 236), Color.FromArgb((int) byte.MaxValue, 182, 86, 8), Color.FromArgb((int) byte.MaxValue, 251, 212, 180), Color.FromArgb((int) byte.MaxValue, 251, 202, 162));
          break;
        case BuiltinTableStyle.ColorfulList:
          Style.BuiltinStyleLoader.LoadStyleColorfulList(style, Color.FromArgb((int) byte.MaxValue, 230, 230, 230), Color.FromArgb((int) byte.MaxValue, 158, 58, 56), Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/), Color.FromArgb((int) byte.MaxValue, 204, 204, 204));
          break;
        case BuiltinTableStyle.ColorfulListAccent1:
          Style.BuiltinStyleLoader.LoadStyleColorfulList(style, Color.FromArgb((int) byte.MaxValue, 237, 242, 248), Color.FromArgb((int) byte.MaxValue, 158, 58, 56), Color.FromArgb((int) byte.MaxValue, 211, 223, 238), Color.FromArgb((int) byte.MaxValue, 219, 229, 241));
          break;
        case BuiltinTableStyle.ColorfulListAccent2:
          Style.BuiltinStyleLoader.LoadStyleColorfulList(style, Color.FromArgb((int) byte.MaxValue, 248, 237, 237), Color.FromArgb((int) byte.MaxValue, 158, 58, 56), Color.FromArgb((int) byte.MaxValue, 239, 211, 210), Color.FromArgb((int) byte.MaxValue, 242, 219, 219));
          break;
        case BuiltinTableStyle.ColorfulListAccent3:
          Style.BuiltinStyleLoader.LoadStyleColorfulList(style, Color.FromArgb((int) byte.MaxValue, 245, 248, 238), Color.FromArgb((int) byte.MaxValue, 102, 78, 130), Color.FromArgb((int) byte.MaxValue, 230, 238, 213), Color.FromArgb((int) byte.MaxValue, 234, 241, 221));
          break;
        case BuiltinTableStyle.ColorfulListAccent4:
          Style.BuiltinStyleLoader.LoadStyleColorfulList(style, Color.FromArgb((int) byte.MaxValue, 242, 239, 246), Color.FromArgb((int) byte.MaxValue, 126, 156, 64 /*0x40*/), Color.FromArgb((int) byte.MaxValue, 223, 216, 232), Color.FromArgb((int) byte.MaxValue, 229, 223, 236));
          break;
        case BuiltinTableStyle.ColorfulListAccent5:
          Style.BuiltinStyleLoader.LoadStyleColorfulList(style, Color.FromArgb((int) byte.MaxValue, 237, 246, 249), Color.FromArgb((int) byte.MaxValue, 242, 115, 10), Color.FromArgb((int) byte.MaxValue, 210, 234, 241), Color.FromArgb((int) byte.MaxValue, 218, 238, 243));
          break;
        case BuiltinTableStyle.ColorfulListAccent6:
          Style.BuiltinStyleLoader.LoadStyleColorfulList(style, Color.FromArgb((int) byte.MaxValue, 254, 244, 236), Color.FromArgb((int) byte.MaxValue, 52, 141, 165), Color.FromArgb((int) byte.MaxValue, 253, 228, 208 /*0xD0*/), Color.FromArgb((int) byte.MaxValue, 253, 233, 217));
          break;
        case BuiltinTableStyle.ColorfulGrid:
          Style.BuiltinStyleLoader.LoadStyleColorfulGrid(style, Color.FromArgb((int) byte.MaxValue, 204, 204, 204), Color.FromArgb((int) byte.MaxValue, 153, 153, 153), Color.FromArgb((int) byte.MaxValue, 0, 0, 0), Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/));
          break;
        case BuiltinTableStyle.ColorfulGridAccent1:
          Style.BuiltinStyleLoader.LoadStyleColorfulGrid(style, Color.FromArgb((int) byte.MaxValue, 219, 229, 241), Color.FromArgb((int) byte.MaxValue, 184, 204, 228), Color.FromArgb((int) byte.MaxValue, 54, 95, 145), Color.FromArgb((int) byte.MaxValue, 167, 191, 222));
          break;
        case BuiltinTableStyle.ColorfulGridAccent2:
          Style.BuiltinStyleLoader.LoadStyleColorfulGrid(style, Color.FromArgb((int) byte.MaxValue, 242, 219, 219), Color.FromArgb((int) byte.MaxValue, 229, 184, 183), Color.FromArgb((int) byte.MaxValue, 148, 54, 52), Color.FromArgb((int) byte.MaxValue, 223, 167, 166));
          break;
        case BuiltinTableStyle.ColorfulGridAccent3:
          Style.BuiltinStyleLoader.LoadStyleColorfulGrid(style, Color.FromArgb((int) byte.MaxValue, 234, 241, 221), Color.FromArgb((int) byte.MaxValue, 214, 227, 188), Color.FromArgb((int) byte.MaxValue, 118, 146, 60), Color.FromArgb((int) byte.MaxValue, 205, 221, 172));
          break;
        case BuiltinTableStyle.ColorfulGridAccent4:
          Style.BuiltinStyleLoader.LoadStyleColorfulGrid(style, Color.FromArgb((int) byte.MaxValue, 229, 223, 236), Color.FromArgb((int) byte.MaxValue, 204, 192 /*0xC0*/, 217), Color.FromArgb((int) byte.MaxValue, 95, 73, 122), Color.FromArgb((int) byte.MaxValue, 191, 177, 208 /*0xD0*/));
          break;
        case BuiltinTableStyle.ColorfulGridAccent5:
          Style.BuiltinStyleLoader.LoadStyleColorfulGrid(style, Color.FromArgb((int) byte.MaxValue, 218, 238, 243), Color.FromArgb((int) byte.MaxValue, 182, 221, 232), Color.FromArgb((int) byte.MaxValue, 49, 132, 155), Color.FromArgb((int) byte.MaxValue, 165, 213, 226));
          break;
        case BuiltinTableStyle.ColorfulGridAccent6:
          Style.BuiltinStyleLoader.LoadStyleColorfulGrid(style, Color.FromArgb((int) byte.MaxValue, 253, 233, 217), Color.FromArgb((int) byte.MaxValue, 251, 212, 180), Color.FromArgb((int) byte.MaxValue, 227, 108, 10), Color.FromArgb((int) byte.MaxValue, 251, 202, 162));
          break;
        case BuiltinTableStyle.Table3Deffects1:
          Style.BuiltinStyleLoader.LoadStyleTable3Deffects1(style);
          break;
        case BuiltinTableStyle.Table3Deffects2:
          Style.BuiltinStyleLoader.LoadStyleTable3Deffects2(style);
          break;
        case BuiltinTableStyle.Table3Deffects3:
          Style.BuiltinStyleLoader.LoadStyleTable3Deffects3(style);
          break;
        case BuiltinTableStyle.TableClassic1:
          Style.BuiltinStyleLoader.LoadStyleTableClassic1(style);
          break;
        case BuiltinTableStyle.TableClassic2:
          Style.BuiltinStyleLoader.LoadStyleTableClassic2(style);
          break;
        case BuiltinTableStyle.TableClassic3:
          Style.BuiltinStyleLoader.LoadStyleTableClassic3(style);
          break;
        case BuiltinTableStyle.TableClassic4:
          Style.BuiltinStyleLoader.LoadStyleTableClassic4(style);
          break;
        case BuiltinTableStyle.TableColorful1:
          Style.BuiltinStyleLoader.LoadStyleTableColorful1(style);
          break;
        case BuiltinTableStyle.TableColorful2:
          Style.BuiltinStyleLoader.LoadStyleTableColorful2(style);
          break;
        case BuiltinTableStyle.TableColorful3:
          Style.BuiltinStyleLoader.LoadStyleTableColorful3(style);
          break;
        case BuiltinTableStyle.TableColumns1:
          Style.BuiltinStyleLoader.LoadStyleTableColumns1(style);
          break;
        case BuiltinTableStyle.TableColumns2:
          Style.BuiltinStyleLoader.LoadStyleTableColumns2(style);
          break;
        case BuiltinTableStyle.TableColumns3:
          Style.BuiltinStyleLoader.LoadStyleTableColumns3(style);
          break;
        case BuiltinTableStyle.TableColumns4:
          Style.BuiltinStyleLoader.LoadStyleTableColumns4(style);
          break;
        case BuiltinTableStyle.TableColumns5:
          Style.BuiltinStyleLoader.LoadStyleTableColumns5(style);
          break;
        case BuiltinTableStyle.TableContemporary:
          Style.BuiltinStyleLoader.LoadStyleTableContemporary(style);
          break;
        case BuiltinTableStyle.TableElegant:
          Style.BuiltinStyleLoader.LoadStyleTableElegant(style);
          break;
        case BuiltinTableStyle.TableGrid1:
          Style.BuiltinStyleLoader.LoadStyleTableGrid1(style);
          break;
        case BuiltinTableStyle.TableGrid2:
          Style.BuiltinStyleLoader.LoadStyleTableGrid2(style);
          break;
        case BuiltinTableStyle.TableGrid3:
          Style.BuiltinStyleLoader.LoadStyleTableGrid3(style);
          break;
        case BuiltinTableStyle.TableGrid4:
          Style.BuiltinStyleLoader.LoadStyleTableGrid4(style);
          break;
        case BuiltinTableStyle.TableGrid5:
          Style.BuiltinStyleLoader.LoadStyleTableGrid5(style);
          break;
        case BuiltinTableStyle.TableGrid6:
          Style.BuiltinStyleLoader.LoadStyleTableGrid6(style);
          break;
        case BuiltinTableStyle.TableGrid7:
          Style.BuiltinStyleLoader.LoadStyleTableGrid7(style);
          break;
        case BuiltinTableStyle.TableGrid8:
          Style.BuiltinStyleLoader.LoadStyleTableGrid8(style);
          break;
        case BuiltinTableStyle.TableList1:
          Style.BuiltinStyleLoader.LoadStyleTableList1(style);
          break;
        case BuiltinTableStyle.TableList2:
          Style.BuiltinStyleLoader.LoadStyleTableList2(style);
          break;
        case BuiltinTableStyle.TableList3:
          Style.BuiltinStyleLoader.LoadStyleTableList3(style);
          break;
        case BuiltinTableStyle.TableList4:
          Style.BuiltinStyleLoader.LoadStyleTableList4(style);
          break;
        case BuiltinTableStyle.TableList5:
          Style.BuiltinStyleLoader.LoadStyleTableList5(style);
          break;
        case BuiltinTableStyle.TableList6:
          Style.BuiltinStyleLoader.LoadStyleTableList6(style);
          break;
        case BuiltinTableStyle.TableList7:
          Style.BuiltinStyleLoader.LoadStyleTableList7(style);
          break;
        case BuiltinTableStyle.TableList8:
          Style.BuiltinStyleLoader.LoadStyleTableList8(style);
          break;
        case BuiltinTableStyle.TableProfessional:
          Style.BuiltinStyleLoader.LoadStyleTableProfessional(style);
          break;
        case BuiltinTableStyle.TableSimple1:
          Style.BuiltinStyleLoader.LoadStyleTableSimple1(style);
          break;
        case BuiltinTableStyle.TableSimple2:
          Style.BuiltinStyleLoader.LoadStyleTableSimple2(style);
          break;
        case BuiltinTableStyle.TableSimple3:
          Style.BuiltinStyleLoader.LoadStyleTableSimple3(style);
          break;
        case BuiltinTableStyle.TableSubtle1:
          Style.BuiltinStyleLoader.LoadStyleTableSubtle1(style);
          break;
        case BuiltinTableStyle.TableSubtle2:
          Style.BuiltinStyleLoader.LoadStyleTableSubtle2(style);
          break;
        case BuiltinTableStyle.TableTheme:
          Style.BuiltinStyleLoader.LoadStyleTableTheme(style);
          break;
        case BuiltinTableStyle.TableWeb1:
          Style.BuiltinStyleLoader.LoadStyleTableWeb1(style);
          break;
        case BuiltinTableStyle.TableWeb2:
          Style.BuiltinStyleLoader.LoadStyleTableWeb2(style);
          break;
        case BuiltinTableStyle.TableWeb3:
          Style.BuiltinStyleLoader.LoadStyleTableWeb3(style);
          break;
      }
    }

    private static void LoadStyleTableNormal(IStyle style)
    {
      (style as Style).IsSemiHidden = true;
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
    }

    private static void LoadStyleTableGrid(IStyle style)
    {
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(52, (object) 12f);
      (style as WTableStyle).ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 0.5f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 0.5f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 0.5f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 0.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.5f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.5f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
    }

    private static void LoadStyleLightShading(
      IStyle style,
      Color textColor,
      Color borderColor,
      Color backColor)
    {
      (style as WTableStyle).CharacterFormat.TextColor = textColor;
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(52, (object) 12f);
      (style as WTableStyle).ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.ParagraphFormat.SetPropertyValue(8, (object) 0.0f);
      conditionalFormattingStyle1.ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      conditionalFormattingStyle1.ParagraphFormat.SetPropertyValue(52, (object) 12f);
      conditionalFormattingStyle1.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Top.LineWidth = 1f;
      conditionalFormattingStyle1.CellProperties.Borders.Top.Color = borderColor;
      conditionalFormattingStyle1.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 1f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = borderColor;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.ParagraphFormat.SetPropertyValue(8, (object) 0.0f);
      conditionalFormattingStyle2.ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      conditionalFormattingStyle2.ParagraphFormat.SetPropertyValue(52, (object) 12f);
      conditionalFormattingStyle2.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle5.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.BackColor = backColor;
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle6.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle6.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle6.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle6.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle6.CellProperties.BackColor = backColor;
      conditionalFormattingStyle6.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.TextureStyle = TextureStyle.TextureNone;
    }

    private static void LoadStyleLightList(IStyle style, Color borderColor, Color backColor)
    {
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(52, (object) 12f);
      (style as WTableStyle).ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.ParagraphFormat.SetPropertyValue(8, (object) 0.0f);
      conditionalFormattingStyle1.ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      conditionalFormattingStyle1.ParagraphFormat.SetPropertyValue(52, (object) 12f);
      conditionalFormattingStyle1.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.BackColor = backColor;
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.ParagraphFormat.SetPropertyValue(8, (object) 0.0f);
      conditionalFormattingStyle2.ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      conditionalFormattingStyle2.ParagraphFormat.SetPropertyValue(52, (object) 12f);
      conditionalFormattingStyle2.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Double;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Left.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Left.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Right.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Right.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Right.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle5.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle5.CellProperties.Borders.Top.LineWidth = 1f;
      conditionalFormattingStyle5.CellProperties.Borders.Top.Color = borderColor;
      conditionalFormattingStyle5.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.LineWidth = 1f;
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.Color = borderColor;
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle5.CellProperties.Borders.Left.LineWidth = 1f;
      conditionalFormattingStyle5.CellProperties.Borders.Left.Color = borderColor;
      conditionalFormattingStyle5.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle5.CellProperties.Borders.Right.LineWidth = 1f;
      conditionalFormattingStyle5.CellProperties.Borders.Right.Color = borderColor;
      conditionalFormattingStyle5.CellProperties.Borders.Right.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle6.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Top.LineWidth = 1f;
      conditionalFormattingStyle6.CellProperties.Borders.Top.Color = borderColor;
      conditionalFormattingStyle6.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.LineWidth = 1f;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.Color = borderColor;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Left.LineWidth = 1f;
      conditionalFormattingStyle6.CellProperties.Borders.Left.Color = borderColor;
      conditionalFormattingStyle6.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Right.LineWidth = 1f;
      conditionalFormattingStyle6.CellProperties.Borders.Right.Color = borderColor;
      conditionalFormattingStyle6.CellProperties.Borders.Right.Space = 0.0f;
    }

    private static void LoadStyleLightGrid(IStyle style, Color borderColor, Color backColor)
    {
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(52, (object) 12f);
      (style as WTableStyle).ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.ParagraphFormat.SetPropertyValue(8, (object) 0.0f);
      conditionalFormattingStyle1.ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      conditionalFormattingStyle1.ParagraphFormat.SetPropertyValue(52, (object) 12f);
      conditionalFormattingStyle1.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Top.LineWidth = 1f;
      conditionalFormattingStyle1.CellProperties.Borders.Top.Color = borderColor;
      conditionalFormattingStyle1.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 2.25f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = borderColor;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Left.LineWidth = 1f;
      conditionalFormattingStyle1.CellProperties.Borders.Left.Color = borderColor;
      conditionalFormattingStyle1.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Right.LineWidth = 1f;
      conditionalFormattingStyle1.CellProperties.Borders.Right.Color = borderColor;
      conditionalFormattingStyle1.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Vertical.LineWidth = 1f;
      conditionalFormattingStyle1.CellProperties.Borders.Vertical.Color = borderColor;
      conditionalFormattingStyle1.CellProperties.Borders.Vertical.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.ParagraphFormat.SetPropertyValue(8, (object) 0.0f);
      conditionalFormattingStyle2.ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      conditionalFormattingStyle2.ParagraphFormat.SetPropertyValue(52, (object) 12f);
      conditionalFormattingStyle2.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Double;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Left.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Left.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Right.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Right.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Vertical.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Vertical.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Vertical.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle4.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle4.CellProperties.Borders.Top.LineWidth = 1f;
      conditionalFormattingStyle4.CellProperties.Borders.Top.Color = borderColor;
      conditionalFormattingStyle4.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle4.CellProperties.Borders.Bottom.LineWidth = 1f;
      conditionalFormattingStyle4.CellProperties.Borders.Bottom.Color = borderColor;
      conditionalFormattingStyle4.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle4.CellProperties.Borders.Left.LineWidth = 1f;
      conditionalFormattingStyle4.CellProperties.Borders.Left.Color = borderColor;
      conditionalFormattingStyle4.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle4.CellProperties.Borders.Right.LineWidth = 1f;
      conditionalFormattingStyle4.CellProperties.Borders.Right.Color = borderColor;
      conditionalFormattingStyle4.CellProperties.Borders.Right.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle5.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle5.CellProperties.Borders.Top.LineWidth = 1f;
      conditionalFormattingStyle5.CellProperties.Borders.Top.Color = borderColor;
      conditionalFormattingStyle5.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.LineWidth = 1f;
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.Color = borderColor;
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle5.CellProperties.Borders.Left.LineWidth = 1f;
      conditionalFormattingStyle5.CellProperties.Borders.Left.Color = borderColor;
      conditionalFormattingStyle5.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle5.CellProperties.Borders.Right.LineWidth = 1f;
      conditionalFormattingStyle5.CellProperties.Borders.Right.Color = borderColor;
      conditionalFormattingStyle5.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.BackColor = backColor;
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle6.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Top.LineWidth = 1f;
      conditionalFormattingStyle6.CellProperties.Borders.Top.Color = borderColor;
      conditionalFormattingStyle6.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.LineWidth = 1f;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.Color = borderColor;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Left.LineWidth = 1f;
      conditionalFormattingStyle6.CellProperties.Borders.Left.Color = borderColor;
      conditionalFormattingStyle6.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Right.LineWidth = 1f;
      conditionalFormattingStyle6.CellProperties.Borders.Right.Color = borderColor;
      conditionalFormattingStyle6.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Vertical.LineWidth = 1f;
      conditionalFormattingStyle6.CellProperties.Borders.Vertical.Color = borderColor;
      conditionalFormattingStyle6.CellProperties.Borders.Vertical.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.BackColor = backColor;
      conditionalFormattingStyle6.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle7 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.EvenRowBanding);
      conditionalFormattingStyle7.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle7.CellProperties.Borders.Top.LineWidth = 1f;
      conditionalFormattingStyle7.CellProperties.Borders.Top.Color = borderColor;
      conditionalFormattingStyle7.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle7.CellProperties.Borders.Bottom.LineWidth = 1f;
      conditionalFormattingStyle7.CellProperties.Borders.Bottom.Color = borderColor;
      conditionalFormattingStyle7.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle7.CellProperties.Borders.Left.LineWidth = 1f;
      conditionalFormattingStyle7.CellProperties.Borders.Left.Color = borderColor;
      conditionalFormattingStyle7.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle7.CellProperties.Borders.Right.LineWidth = 1f;
      conditionalFormattingStyle7.CellProperties.Borders.Right.Color = borderColor;
      conditionalFormattingStyle7.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      conditionalFormattingStyle7.CellProperties.Borders.Vertical.LineWidth = 1f;
      conditionalFormattingStyle7.CellProperties.Borders.Vertical.Color = borderColor;
      conditionalFormattingStyle7.CellProperties.Borders.Vertical.Space = 0.0f;
    }

    private static void LoadStyleMediumShading1(
      IStyle style,
      Color borderColor,
      Color firstRowBackColor,
      Color backColor)
    {
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(52, (object) 12f);
      (style as WTableStyle).ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.ParagraphFormat.SetPropertyValue(8, (object) 0.0f);
      conditionalFormattingStyle1.ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      conditionalFormattingStyle1.ParagraphFormat.SetPropertyValue(52, (object) 12f);
      conditionalFormattingStyle1.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Top.LineWidth = 1f;
      conditionalFormattingStyle1.CellProperties.Borders.Top.Color = borderColor;
      conditionalFormattingStyle1.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 1f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = borderColor;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Left.LineWidth = 1f;
      conditionalFormattingStyle1.CellProperties.Borders.Left.Color = borderColor;
      conditionalFormattingStyle1.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Right.LineWidth = 1f;
      conditionalFormattingStyle1.CellProperties.Borders.Right.Color = borderColor;
      conditionalFormattingStyle1.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.BackColor = firstRowBackColor;
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.ParagraphFormat.SetPropertyValue(8, (object) 0.0f);
      conditionalFormattingStyle2.ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      conditionalFormattingStyle2.ParagraphFormat.SetPropertyValue(52, (object) 12f);
      conditionalFormattingStyle2.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Double;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Left.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Left.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Right.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Right.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle5.CellProperties.BackColor = backColor;
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle6.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle6.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle6.CellProperties.BackColor = backColor;
      conditionalFormattingStyle6.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle7 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.EvenRowBanding);
      conditionalFormattingStyle7.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle7.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
    }

    private static void LoadStyleMediumShading2(IStyle style, Color backColor)
    {
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(52, (object) 12f);
      (style as WTableStyle).ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 2.25f;
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 2.25f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.ParagraphFormat.SetPropertyValue(8, (object) 0.0f);
      conditionalFormattingStyle1.ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      conditionalFormattingStyle1.ParagraphFormat.SetPropertyValue(52, (object) 12f);
      conditionalFormattingStyle1.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Top.LineWidth = 2.25f;
      conditionalFormattingStyle1.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 2.25f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.BackColor = backColor;
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.ParagraphFormat.SetPropertyValue(8, (object) 0.0f);
      conditionalFormattingStyle2.ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      conditionalFormattingStyle2.ParagraphFormat.SetPropertyValue(52, (object) 12f);
      conditionalFormattingStyle2.ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      conditionalFormattingStyle2.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Double;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.LineWidth = 2.25f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle3.CellProperties.Borders.Bottom.LineWidth = 2.25f;
      conditionalFormattingStyle3.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.BackColor = backColor;
      conditionalFormattingStyle3.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle4.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.BackColor = backColor;
      conditionalFormattingStyle4.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle5.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, 216, 216, 216);
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle6.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, 216, 216, 216);
      conditionalFormattingStyle6.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle7 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowLastCell);
      conditionalFormattingStyle7.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle7.CellProperties.Borders.Top.LineWidth = 2.25f;
      conditionalFormattingStyle7.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle7.CellProperties.Borders.Bottom.LineWidth = 2.25f;
      conditionalFormattingStyle7.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle7.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle7.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle7.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      ConditionalFormattingStyle conditionalFormattingStyle8 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowFirstCell);
      conditionalFormattingStyle8.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle8.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle8.CellProperties.Borders.Top.LineWidth = 2.25f;
      conditionalFormattingStyle8.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle8.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle8.CellProperties.Borders.Bottom.LineWidth = 2.25f;
      conditionalFormattingStyle8.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle8.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle8.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle8.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle8.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
    }

    private static void LoadStyleMediumList1(IStyle style, Color borderColor, Color backColor)
    {
      (style as WTableStyle).CharacterFormat.TextColor = Color.Black;
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(52, (object) 12f);
      (style as WTableStyle).ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 1f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = borderColor;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 31 /*0x1F*/, 73, 125);
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle4.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle4.CellProperties.Borders.Top.LineWidth = 1f;
      conditionalFormattingStyle4.CellProperties.Borders.Top.Color = borderColor;
      conditionalFormattingStyle4.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle4.CellProperties.Borders.Bottom.LineWidth = 1f;
      conditionalFormattingStyle4.CellProperties.Borders.Bottom.Color = borderColor;
      conditionalFormattingStyle4.CellProperties.Borders.Bottom.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle5.CellProperties.BackColor = backColor;
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle6.CellProperties.BackColor = backColor;
      conditionalFormattingStyle6.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.TextureStyle = TextureStyle.TextureNone;
    }

    private static void LoadStyleMediumList2(IStyle style, Color borderColor, Color backColor)
    {
      (style as WTableStyle).CharacterFormat.TextColor = Color.Black;
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(52, (object) 12f);
      (style as WTableStyle).ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.SetPropertyValue(3, (object) 12f);
      conditionalFormattingStyle1.CharacterFormat.SetPropertyValue(62, (object) 12f);
      conditionalFormattingStyle1.CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 3f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = borderColor;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Bottom.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle3.CellProperties.Borders.Right.LineWidth = 1f;
      conditionalFormattingStyle3.CellProperties.Borders.Right.Color = borderColor;
      conditionalFormattingStyle3.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Bottom.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle4.CellProperties.Borders.Left.LineWidth = 1f;
      conditionalFormattingStyle4.CellProperties.Borders.Left.Color = borderColor;
      conditionalFormattingStyle4.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle5.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.BackColor = backColor;
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle6.CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle6.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle6.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle6.CellProperties.BackColor = backColor;
      conditionalFormattingStyle6.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle7 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowFirstCell);
      conditionalFormattingStyle7.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle7.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle7.CellProperties.TextureStyle = TextureStyle.TextureNone;
      (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRowFirstCell).CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
    }

    private static void LoadStyleMediumGrid1(
      IStyle style,
      Color borderColor,
      Color backColor,
      Color bandCellColor)
    {
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(52, (object) 12f);
      (style as WTableStyle).ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = backColor;
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 2.25f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = borderColor;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle5.CellProperties.BackColor = bandCellColor;
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle6.CellProperties.BackColor = bandCellColor;
      conditionalFormattingStyle6.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.TextureStyle = TextureStyle.TextureNone;
    }

    private static void LoadStyleMediumGrid2(
      IStyle style,
      Color borderColor,
      Color backColor,
      Color firstRowColor,
      Color lastColumnColor,
      Color bandCellColor)
    {
      (style as WTableStyle).CharacterFormat.TextColor = Color.Black;
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(52, (object) 12f);
      (style as WTableStyle).ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = backColor;
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.Black;
      conditionalFormattingStyle1.CellProperties.BackColor = firstRowColor;
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CharacterFormat.TextColor = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 1.5f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CharacterFormat.TextColor = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Bottom.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = false;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = false;
      conditionalFormattingStyle4.CharacterFormat.TextColor = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Bottom.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.BackColor = lastColumnColor;
      conditionalFormattingStyle4.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle5.CellProperties.BackColor = bandCellColor;
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle6.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Horizontal.LineWidth = 0.75f;
      conditionalFormattingStyle6.CellProperties.Borders.Horizontal.Color = borderColor;
      conditionalFormattingStyle6.CellProperties.Borders.Horizontal.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Vertical.LineWidth = 0.75f;
      conditionalFormattingStyle6.CellProperties.Borders.Vertical.Color = borderColor;
      conditionalFormattingStyle6.CellProperties.Borders.Vertical.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.BackColor = bandCellColor;
      conditionalFormattingStyle6.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle7 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowFirstCell);
      conditionalFormattingStyle7.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle7.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle7.CellProperties.TextureStyle = TextureStyle.TextureNone;
    }

    private static void LoadStyleMediumGrid3(
      IStyle style,
      Color backColor,
      Color firstRowColor,
      Color bandCellColor)
    {
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(52, (object) 12f);
      (style as WTableStyle).ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 1f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = backColor;
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CharacterFormat.Italic = false;
      conditionalFormattingStyle1.CharacterFormat.ItalicBidi = false;
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Top.LineWidth = 1f;
      conditionalFormattingStyle1.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 3f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Left.LineWidth = 1f;
      conditionalFormattingStyle1.CellProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Right.LineWidth = 1f;
      conditionalFormattingStyle1.CellProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Vertical.LineWidth = 1f;
      conditionalFormattingStyle1.CellProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.Vertical.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = firstRowColor;
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CharacterFormat.Italic = false;
      conditionalFormattingStyle2.CharacterFormat.ItalicBidi = false;
      conditionalFormattingStyle2.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 3f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Left.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Right.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Vertical.LineWidth = 1f;
      conditionalFormattingStyle2.CellProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.Borders.Vertical.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.BackColor = firstRowColor;
      conditionalFormattingStyle2.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CharacterFormat.Italic = false;
      conditionalFormattingStyle3.CharacterFormat.ItalicBidi = false;
      conditionalFormattingStyle3.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle3.CellProperties.Borders.Left.LineWidth = 1f;
      conditionalFormattingStyle3.CellProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle3.CellProperties.Borders.Right.LineWidth = 3f;
      conditionalFormattingStyle3.CellProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.BackColor = firstRowColor;
      conditionalFormattingStyle3.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle4.CharacterFormat.Italic = false;
      conditionalFormattingStyle4.CharacterFormat.ItalicBidi = false;
      conditionalFormattingStyle4.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Bottom.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle4.CellProperties.Borders.Left.LineWidth = 3f;
      conditionalFormattingStyle4.CellProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.BackColor = firstRowColor;
      conditionalFormattingStyle4.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle5.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle5.CellProperties.Borders.Top.LineWidth = 1f;
      conditionalFormattingStyle5.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.LineWidth = 1f;
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle5.CellProperties.Borders.Left.LineWidth = 1f;
      conditionalFormattingStyle5.CellProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle5.CellProperties.Borders.Right.LineWidth = 1f;
      conditionalFormattingStyle5.CellProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.BackColor = bandCellColor;
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle6.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Top.LineWidth = 1f;
      conditionalFormattingStyle6.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.LineWidth = 1f;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Left.LineWidth = 1f;
      conditionalFormattingStyle6.CellProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Right.LineWidth = 1f;
      conditionalFormattingStyle6.CellProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Horizontal.LineWidth = 1f;
      conditionalFormattingStyle6.CellProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.Borders.Horizontal.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Vertical.LineWidth = 1f;
      conditionalFormattingStyle6.CellProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.Borders.Vertical.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.BackColor = bandCellColor;
      conditionalFormattingStyle6.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.TextureStyle = TextureStyle.TextureNone;
    }

    private static void LoadStyleDarkList(
      IStyle style,
      Color backColor,
      Color lastRowColor,
      Color bandCellColor)
    {
      (style as WTableStyle).CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(52, (object) 12f);
      (style as WTableStyle).ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).CellProperties.BackColor = backColor;
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 2.25f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 2.25f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle2.CellProperties.BackColor = lastRowColor;
      conditionalFormattingStyle2.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Bottom.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle3.CellProperties.Borders.Right.LineWidth = 2.25f;
      conditionalFormattingStyle3.CellProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.BackColor = bandCellColor;
      conditionalFormattingStyle3.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Bottom.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle4.CellProperties.Borders.Left.LineWidth = 2.25f;
      conditionalFormattingStyle4.CellProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.BackColor = bandCellColor;
      conditionalFormattingStyle4.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle5.CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.BackColor = bandCellColor;
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle6.CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle6.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle6.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle6.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle6.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle6.CellProperties.BackColor = bandCellColor;
      conditionalFormattingStyle6.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.TextureStyle = TextureStyle.TextureNone;
    }

    private static void LoadStyleColorfulShading(
      IStyle style,
      Color topBorderColor,
      Color borderColor,
      Color backColor,
      Color lastRowColor,
      Color bandColumnColor,
      Color bandRowColor)
    {
      (style as WTableStyle).CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(52, (object) 12f);
      (style as WTableStyle).ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 3f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = topBorderColor;
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 0.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 0.5f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 0.5f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = borderColor;
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.5f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.5f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = backColor;
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 3f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = topBorderColor;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.BackColor = lastRowColor;
      conditionalFormattingStyle2.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Bottom.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      conditionalFormattingStyle3.CellProperties.Borders.Horizontal.LineWidth = 0.5f;
      conditionalFormattingStyle3.CellProperties.Borders.Horizontal.Color = lastRowColor;
      conditionalFormattingStyle3.CellProperties.Borders.Horizontal.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle3.CellProperties.BackColor = lastRowColor;
      conditionalFormattingStyle3.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Bottom.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle4.CellProperties.BackColor = lastRowColor;
      conditionalFormattingStyle4.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle5.CellProperties.BackColor = bandColumnColor;
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle6.CellProperties.BackColor = bandRowColor;
      conditionalFormattingStyle6.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.TextureStyle = TextureStyle.TextureNone;
      if (!(style.Name != "Colorful Shading Accent 3"))
        return;
      (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowLastCell).CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowFirstCell).CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
    }

    private static void LoadStyleColorfulList(
      IStyle style,
      Color backColor,
      Color rowColor,
      Color bandColumnColor,
      Color bandRowColor)
    {
      (style as WTableStyle).CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(52, (object) 12f);
      (style as WTableStyle).ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).CellProperties.BackColor = backColor;
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 1.5f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = rowColor;
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CharacterFormat.TextColor = rowColor;
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 1.5f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle5.CellProperties.Borders.Top.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Left.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Right.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Horizontal.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.Borders.Vertical.BorderType = BorderStyle.Cleared;
      conditionalFormattingStyle5.CellProperties.BackColor = bandColumnColor;
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle6.CellProperties.BackColor = bandRowColor;
      conditionalFormattingStyle6.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.TextureStyle = TextureStyle.TextureNone;
    }

    private static void LoadStyleColorfulGrid(
      IStyle style,
      Color backColor,
      Color rowColor,
      Color columnColor,
      Color bandColor)
    {
      (style as WTableStyle).CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(9, (object) 0.0f);
      (style as WTableStyle).ParagraphFormat.SetPropertyValue(52, (object) 12f);
      (style as WTableStyle).ParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.5f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = backColor;
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CellProperties.BackColor = rowColor;
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.BackColor = rowColor;
      conditionalFormattingStyle2.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.BackColor = columnColor;
      conditionalFormattingStyle3.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.BackColor = columnColor;
      conditionalFormattingStyle4.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle5.CellProperties.BackColor = bandColor;
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle6.CellProperties.BackColor = bandColor;
      conditionalFormattingStyle6.CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.TextureStyle = TextureStyle.TextureNone;
    }

    private static void LoadStyleTable3Deffects1(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 0, 128 /*0x80*/);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle3.CellProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
      conditionalFormattingStyle3.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.Right.LineWidth = 0.75f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle4.CellProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.Left.LineWidth = 0.75f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowLastCell);
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.Left.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.Left.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.Left.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowFirstCell);
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.LineWidth = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Right.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.Right.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Right.LineWidth = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle7 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRowLastCell);
      conditionalFormattingStyle7.CellProperties.Borders.Top.BorderType = BorderStyle.None;
      conditionalFormattingStyle7.CellProperties.Borders.Top.Color = Color.Black;
      conditionalFormattingStyle7.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.Top.LineWidth = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.Left.BorderType = BorderStyle.None;
      conditionalFormattingStyle7.CellProperties.Borders.Left.Color = Color.Black;
      conditionalFormattingStyle7.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.Left.LineWidth = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle8 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRowFirstCell);
      conditionalFormattingStyle8.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      conditionalFormattingStyle8.CellProperties.Borders.Top.BorderType = BorderStyle.None;
      conditionalFormattingStyle8.CellProperties.Borders.Top.Color = Color.Black;
      conditionalFormattingStyle8.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle8.CellProperties.Borders.Top.LineWidth = 0.0f;
      conditionalFormattingStyle8.CellProperties.Borders.Right.BorderType = BorderStyle.None;
      conditionalFormattingStyle8.CellProperties.Borders.Right.Color = Color.Black;
      conditionalFormattingStyle8.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle8.CellProperties.Borders.Right.LineWidth = 0.0f;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTable3Deffects2(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
      conditionalFormattingStyle2.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Right.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle3.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle3.CellProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.Right.LineWidth = 0.75f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle4.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle4.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
      conditionalFormattingStyle4.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle4.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle4.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRowFirstCell);
      conditionalFormattingStyle5.CharacterFormat.Bold = true;
      conditionalFormattingStyle5.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTable3Deffects3(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
      conditionalFormattingStyle2.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Right.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle3.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle3.CellProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.Right.LineWidth = 0.75f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle4.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle4.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/);
      conditionalFormattingStyle4.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.EvenColumnBanding);
      conditionalFormattingStyle5.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle5.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.Texture50Percent;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle6.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
      conditionalFormattingStyle6.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle7 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRowFirstCell);
      conditionalFormattingStyle7.CharacterFormat.Bold = true;
      conditionalFormattingStyle7.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableClassic1(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Italic = true;
      conditionalFormattingStyle1.CharacterFormat.ItalicBidi = true;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle3.CellProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle3.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.Right.LineWidth = 0.75f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowLastCell);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle4.CharacterFormat.Italic = false;
      conditionalFormattingStyle4.CharacterFormat.ItalicBidi = false;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRowFirstCell);
      conditionalFormattingStyle5.CharacterFormat.Bold = true;
      conditionalFormattingStyle5.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableClassic2(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 0, 128 /*0x80*/);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/);
      conditionalFormattingStyle3.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowLastCell);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowFirstCell);
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 0, 128 /*0x80*/);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRowFirstCell);
      conditionalFormattingStyle6.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableClassic3(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CharacterFormat.Italic = true;
      conditionalFormattingStyle1.CharacterFormat.ItalicBidi = true;
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 1.5f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableClassic4(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CharacterFormat.Italic = true;
      conditionalFormattingStyle1.CharacterFormat.ItalicBidi = true;
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.Texture50Percent;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.TextureStyle = TextureStyle.Texture50Percent;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowFirstCell);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRowFirstCell);
      conditionalFormattingStyle5.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableColorful1(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 128 /*0x80*/);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CharacterFormat.Italic = true;
      conditionalFormattingStyle1.CharacterFormat.ItalicBidi = true;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CharacterFormat.Italic = true;
      conditionalFormattingStyle2.CharacterFormat.ItalicBidi = true;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      conditionalFormattingStyle2.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowFirstCell);
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle3.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRowFirstCell);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle4.CharacterFormat.Italic = false;
      conditionalFormattingStyle4.CharacterFormat.ItalicBidi = false;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableColorful2(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.Texture20Percent;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CharacterFormat.Italic = true;
      conditionalFormattingStyle1.CharacterFormat.ItalicBidi = true;
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 1.5f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 0, 0);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CharacterFormat.Italic = true;
      conditionalFormattingStyle2.CharacterFormat.ItalicBidi = true;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/);
      conditionalFormattingStyle3.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRowFirstCell);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle4.CharacterFormat.Italic = false;
      conditionalFormattingStyle4.CharacterFormat.ItalicBidi = false;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableColorful3(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 2.25f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 2.25f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 2.25f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 2.25f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/);
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 128 /*0x80*/);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.Texture25Percent;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 128 /*0x80*/);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle2.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Left.LineWidth = 4.5f;
      conditionalFormattingStyle2.CellProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Right.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 128 /*0x80*/);
      conditionalFormattingStyle2.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowFirstCell);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle3.CellProperties.TextureStyle = TextureStyle.TextureSolid;
    }

    private static void LoadStyleTableColumns1(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).CharacterFormat.Bold = true;
      (style as WTableStyle).CharacterFormat.BoldBidi = true;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = false;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = false;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Double;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = false;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = false;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = false;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = false;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = false;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = false;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle5.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle5.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.Texture25Percent;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.EvenColumnBanding);
      conditionalFormattingStyle6.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle6.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
      conditionalFormattingStyle6.CellProperties.TextureStyle = TextureStyle.Texture25Percent;
      ConditionalFormattingStyle conditionalFormattingStyle7 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowLastCell);
      conditionalFormattingStyle7.CharacterFormat.Bold = true;
      conditionalFormattingStyle7.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle8 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRowFirstCell);
      conditionalFormattingStyle8.CharacterFormat.Bold = true;
      conditionalFormattingStyle8.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableColumns2(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).CharacterFormat.Bold = true;
      (style as WTableStyle).CharacterFormat.BoldBidi = true;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = false;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = false;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = false;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = false;
      conditionalFormattingStyle3.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = false;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = false;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle5.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle5.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.Texture30Percent;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.EvenColumnBanding);
      conditionalFormattingStyle6.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle6.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue, 0);
      conditionalFormattingStyle6.CellProperties.TextureStyle = TextureStyle.Texture25Percent;
      ConditionalFormattingStyle conditionalFormattingStyle7 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowLastCell);
      conditionalFormattingStyle7.CharacterFormat.Bold = true;
      conditionalFormattingStyle7.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle8 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRowFirstCell);
      conditionalFormattingStyle8.CharacterFormat.Bold = true;
      conditionalFormattingStyle8.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle8.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableColumns3(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).CharacterFormat.Bold = true;
      (style as WTableStyle).CharacterFormat.BoldBidi = true;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = false;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = false;
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = false;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = false;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = false;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = false;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle5.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle5.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.EvenColumnBanding);
      conditionalFormattingStyle6.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle6.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle6.CellProperties.TextureStyle = TextureStyle.Texture10Percent;
      ConditionalFormattingStyle conditionalFormattingStyle7 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowLastCell);
      conditionalFormattingStyle7.CharacterFormat.Bold = true;
      conditionalFormattingStyle7.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableColumns4(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle4.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle4.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 128 /*0x80*/);
      conditionalFormattingStyle4.CellProperties.TextureStyle = TextureStyle.Texture50Percent;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.EvenColumnBanding);
      conditionalFormattingStyle5.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle5.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.Texture10Percent;
    }

    private static void LoadStyleTableColumns5(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.ColumnStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/);
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CharacterFormat.Italic = true;
      conditionalFormattingStyle1.CharacterFormat.ItalicBidi = true;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddColumnBanding);
      conditionalFormattingStyle5.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle5.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.EvenColumnBanding).CharacterFormat.TextColor = Color.Empty;
    }

    private static void LoadStyleTableContemporary(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 2.25f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 2.25f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.Texture20Percent;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle2.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.TextureStyle = TextureStyle.Texture5Percent;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.EvenRowBanding);
      conditionalFormattingStyle3.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle3.CellProperties.TextureStyle = TextureStyle.Texture20Percent;
    }

    private static void LoadStyleTableElegant(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Double;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Double;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Double;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Double;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle.CharacterFormat.AllCaps = true;
      conditionalFormattingStyle.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableGrid1(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle1.CharacterFormat.Italic = true;
      conditionalFormattingStyle1.CharacterFormat.ItalicBidi = true;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle2.CharacterFormat.Italic = true;
      conditionalFormattingStyle2.CharacterFormat.ItalicBidi = true;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableGrid2(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableGrid3(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.Texture30Percent;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableGrid4(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.Texture30Percent;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
      conditionalFormattingStyle2.CellProperties.TextureStyle = TextureStyle.Texture30Percent;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableGrid5(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 1.5f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowFirstCell);
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.Single;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.75f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableGrid6(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowFirstCell);
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.Single;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.75f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableGrid7(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).CharacterFormat.Bold = true;
      (style as WTableStyle).CharacterFormat.BoldBidi = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = false;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = false;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 1.5f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = false;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = false;
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = false;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = false;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = false;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = false;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowFirstCell);
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.Single;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.LineWidth = 0.75f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableGrid8(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableList1(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CharacterFormat.Italic = true;
      conditionalFormattingStyle1.CharacterFormat.ItalicBidi = true;
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle3.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/);
      conditionalFormattingStyle3.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.EvenRowBanding);
      conditionalFormattingStyle4.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRowFirstCell);
      conditionalFormattingStyle5.CharacterFormat.Bold = true;
      conditionalFormattingStyle5.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableList2(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.RowStripe = 2L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 0);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 128 /*0x80*/);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.Texture75Percent;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle3.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, (int) byte.MaxValue, 0);
      conditionalFormattingStyle3.CellProperties.TextureStyle = TextureStyle.Texture20Percent;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.EvenRowBanding);
      conditionalFormattingStyle4.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRowFirstCell);
      conditionalFormattingStyle5.CharacterFormat.Bold = true;
      conditionalFormattingStyle5.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableList3(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 1.5f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 1.5f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRowFirstCell);
      conditionalFormattingStyle3.CharacterFormat.Italic = true;
      conditionalFormattingStyle3.CharacterFormat.ItalicBidi = true;
      conditionalFormattingStyle3.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 128 /*0x80*/);
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableList4(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle.CharacterFormat.Bold = true;
      conditionalFormattingStyle.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.Bottom.LineWidth = 1.5f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
      conditionalFormattingStyle.CellProperties.TextureStyle = TextureStyle.TextureSolid;
    }

    private static void LoadStyleTableList5(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 1.5f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableList6(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.Texture50Percent;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 1.5f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Right.LineWidth = 1.5f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle3.CellProperties.TextureStyle = TextureStyle.Texture25Percent;
    }

    private static void LoadStyleTableList7(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 1.5f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 192 /*0xC0*/, 192 /*0xC0*/, 192 /*0xC0*/);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 1.5f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle5.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.Texture20Percent;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.EvenRowBanding);
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle6.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
      conditionalFormattingStyle6.CellProperties.TextureStyle = TextureStyle.Texture25Percent;
    }

    private static void LoadStyleTableList8(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CharacterFormat.Italic = true;
      conditionalFormattingStyle1.CharacterFormat.ItalicBidi = true;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle1.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
      conditionalFormattingStyle1.CellProperties.TextureStyle = TextureStyle.TextureSolid;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle5.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 0);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.Texture25Percent;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.EvenRowBanding);
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle6.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle6.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 0, 0);
      conditionalFormattingStyle6.CellProperties.TextureStyle = TextureStyle.Texture50Percent;
    }

    private static void LoadStyleTableProfessional(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle.CharacterFormat.Bold = true;
      conditionalFormattingStyle.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle.CellProperties.TextureStyle = TextureStyle.TextureSolid;
    }

    private static void LoadStyleTableSimple1(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableSimple2(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CharacterFormat.Bold = true;
      conditionalFormattingStyle1.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 1.5f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CharacterFormat.Bold = true;
      conditionalFormattingStyle2.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle2.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CharacterFormat.Bold = true;
      conditionalFormattingStyle3.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle3.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle3.CellProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle3.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.Right.LineWidth = 1.5f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CharacterFormat.Bold = true;
      conditionalFormattingStyle4.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle4.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle4.CellProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle4.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.Left.LineWidth = 0.75f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowLastCell);
      conditionalFormattingStyle5.CharacterFormat.Bold = true;
      conditionalFormattingStyle5.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle5.CellProperties.Borders.Left.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.Left.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.Left.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRowFirstCell);
      conditionalFormattingStyle6.CharacterFormat.Bold = true;
      conditionalFormattingStyle6.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle6.CellProperties.Borders.Top.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.Top.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.Top.LineWidth = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableSimple3(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 1.5f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      ConditionalFormattingStyle conditionalFormattingStyle = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle.CharacterFormat.Bold = true;
      conditionalFormattingStyle.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle.CharacterFormat.TextColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle.CellProperties.TextureStyle = TextureStyle.TextureSolid;
    }

    private static void LoadStyleTableSubtle1(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.RowStripe = 1L;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Top.LineWidth = 0.75f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 1.5f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 1.5f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle2.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 0, 128 /*0x80*/);
      conditionalFormattingStyle2.CellProperties.TextureStyle = TextureStyle.Texture25Percent;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle3.CellProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle3.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.Right.LineWidth = 1.5f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle4.CellProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle4.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.Left.LineWidth = 1.5f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.OddRowBanding);
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.Bottom.LineWidth = 0.75f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle5.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 0);
      conditionalFormattingStyle5.CellProperties.TextureStyle = TextureStyle.Texture25Percent;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowLastCell);
      conditionalFormattingStyle6.CharacterFormat.Bold = true;
      conditionalFormattingStyle6.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle7 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRowFirstCell);
      conditionalFormattingStyle7.CharacterFormat.Bold = true;
      conditionalFormattingStyle7.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle7.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableSubtle2(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle1 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.Bottom.LineWidth = 1.5f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle1.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle2 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRow);
      conditionalFormattingStyle2.CellProperties.Borders.Top.BorderType = BorderStyle.Single;
      conditionalFormattingStyle2.CellProperties.Borders.Top.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle2.CellProperties.Borders.Top.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.Top.LineWidth = 1.5f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle2.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle3 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstColumn);
      conditionalFormattingStyle3.CellProperties.Borders.Right.BorderType = BorderStyle.Single;
      conditionalFormattingStyle3.CellProperties.Borders.Right.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle3.CellProperties.Borders.Right.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.Right.LineWidth = 1.5f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle3.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle3.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle3.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 0, 128 /*0x80*/, 0);
      conditionalFormattingStyle3.CellProperties.TextureStyle = TextureStyle.Texture25Percent;
      ConditionalFormattingStyle conditionalFormattingStyle4 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastColumn);
      conditionalFormattingStyle4.CellProperties.Borders.Left.BorderType = BorderStyle.Single;
      conditionalFormattingStyle4.CellProperties.Borders.Left.Color = Color.FromArgb((int) byte.MaxValue, 0, 0, 0);
      conditionalFormattingStyle4.CellProperties.Borders.Left.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.Left.LineWidth = 1.5f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle4.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      conditionalFormattingStyle4.CellProperties.BackColor = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      conditionalFormattingStyle4.CellProperties.ForeColor = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 0);
      conditionalFormattingStyle4.CellProperties.TextureStyle = TextureStyle.Texture25Percent;
      ConditionalFormattingStyle conditionalFormattingStyle5 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRowLastCell);
      conditionalFormattingStyle5.CharacterFormat.Bold = true;
      conditionalFormattingStyle5.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle5.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
      ConditionalFormattingStyle conditionalFormattingStyle6 = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.LastRowFirstCell);
      conditionalFormattingStyle6.CharacterFormat.Bold = true;
      conditionalFormattingStyle6.CharacterFormat.BoldBidi = true;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle6.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableTheme(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 0.5f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 0.5f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 0.5f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 0.5f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.5f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Single;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.5f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
    }

    private static void LoadStyleTableWeb1(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.CellSpacing = 1f;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Outset;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Outset;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Outset;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Outset;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Outset;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Outset;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      (style as WTableStyle).RowProperties.CellSpacing = 1f;
      ConditionalFormattingStyle conditionalFormattingStyle = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableWeb2(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.CellSpacing = 1f;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Inset;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Inset;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Inset;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Inset;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Inset;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Inset;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      (style as WTableStyle).RowProperties.CellSpacing = 1f;
      ConditionalFormattingStyle conditionalFormattingStyle = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static void LoadStyleTableWeb3(IStyle style)
    {
      (style as Style).UnhideWhenUsed = true;
      (style as WTableStyle).TableProperties.CellSpacing = 1f;
      (style as WTableStyle).TableProperties.LeftIndent = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Top = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Bottom = 0.0f;
      (style as WTableStyle).TableProperties.Paddings.Left = 5.4f;
      (style as WTableStyle).TableProperties.Paddings.Right = 5.4f;
      (style as WTableStyle).TableProperties.Borders.Top.BorderType = BorderStyle.Outset;
      (style as WTableStyle).TableProperties.Borders.Top.LineWidth = 3f;
      (style as WTableStyle).TableProperties.Borders.Top.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Top.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Bottom.BorderType = BorderStyle.Outset;
      (style as WTableStyle).TableProperties.Borders.Bottom.LineWidth = 3f;
      (style as WTableStyle).TableProperties.Borders.Bottom.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Bottom.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Left.BorderType = BorderStyle.Outset;
      (style as WTableStyle).TableProperties.Borders.Left.LineWidth = 3f;
      (style as WTableStyle).TableProperties.Borders.Left.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Left.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Right.BorderType = BorderStyle.Outset;
      (style as WTableStyle).TableProperties.Borders.Right.LineWidth = 3f;
      (style as WTableStyle).TableProperties.Borders.Right.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Right.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.BorderType = BorderStyle.Outset;
      (style as WTableStyle).TableProperties.Borders.Horizontal.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Horizontal.Space = 0.0f;
      (style as WTableStyle).TableProperties.Borders.Vertical.BorderType = BorderStyle.Outset;
      (style as WTableStyle).TableProperties.Borders.Vertical.LineWidth = 0.75f;
      (style as WTableStyle).TableProperties.Borders.Vertical.Color = Color.Black;
      (style as WTableStyle).TableProperties.Borders.Vertical.Space = 0.0f;
      (style as WTableStyle).CellProperties.BackColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.ForeColor = Color.FromArgb(0, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      (style as WTableStyle).CellProperties.TextureStyle = TextureStyle.TextureNone;
      (style as WTableStyle).RowProperties.CellSpacing = 1f;
      ConditionalFormattingStyle conditionalFormattingStyle = (style as WTableStyle).ConditionalFormat(ConditionalFormattingType.FirstRow);
      conditionalFormattingStyle.CharacterFormat.TextColor = Color.Empty;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.BorderType = BorderStyle.None;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.Color = Color.Black;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.Space = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalDown.LineWidth = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.BorderType = BorderStyle.None;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.Color = Color.Black;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.Space = 0.0f;
      conditionalFormattingStyle.CellProperties.Borders.DiagonalUp.LineWidth = 0.0f;
    }

    private static Stream UpdateXMLResAndReader()
    {
      return Style.BuiltinStyleLoader.GetManifestResourceStream("builtin-styles.xml") ?? throw new Exception("Resource file builtin-styles.xml not found.");
    }

    private static Stream GetManifestResourceStream(string fileName)
    {
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      foreach (string manifestResourceName in executingAssembly.GetManifestResourceNames())
      {
        if (manifestResourceName.EndsWith("." + fileName))
        {
          fileName = manifestResourceName;
          break;
        }
      }
      return executingAssembly.GetManifestResourceStream(fileName);
    }

    internal static bool IsListStyle(BuiltinStyle bstyle)
    {
      bool flag = false;
      for (int index = 0; index < 10; ++index)
      {
        if (bstyle.ToString() == ((BuiltinListStyle) index).ToString())
        {
          flag = true;
          break;
        }
      }
      return flag;
    }
  }
}
