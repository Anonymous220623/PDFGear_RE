// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TableOfContent
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class TableOfContent : ParagraphItem, ILayoutInfo
{
  private const int DEF_UPPER_HEADING_LEVEL = 3;
  private const int DEF_LOWER_HEADING_LEVEL = 1;
  private const char DEF_HEADING_LEVELS_SWITCH = 'o';
  private const char DEF_HYPERLINK_SWITCH = 'h';
  private const char DEF_PAGE_NUMBERS_SWITCH = 'n';
  private const char DEF_SEPARATOR_SWITCH = 'p';
  private const char DEF_USE_OUTLINE_SWITCH = 'u';
  private const char DEF_USE_FIELDS_SWITCH = 'f';
  private const char DEF_STYLES_SWITCH = 't';
  private WField m_tocField;
  private int m_upperHeadingLevel = 3;
  private int m_lowerHeadingLevel = 1;
  private string m_tableID;
  private Dictionary<int, List<WParagraphStyle>> m_tocStyles;
  private Dictionary<Entity, Entity> m_tocEntryEntities;
  private string m_lstSepar = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
  private Dictionary<int, List<string>> m_tocLevels;
  private WParagraph m_tocParagraph;
  internal Entity m_tocEntryLastEntity;
  private byte m_bFlags = 45;

  public bool UseHeadingStyles
  {
    get
    {
      this.OnGetValue();
      return ((int) this.m_bFlags & 1) != 0;
    }
    set
    {
      this.OnChange();
      this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
      if (this.Document.IsOpening)
        return;
      this.UpdateTOCFieldCode();
    }
  }

  public int UpperHeadingLevel
  {
    get
    {
      this.OnGetValue();
      return this.m_upperHeadingLevel;
    }
    set
    {
      this.CheckLevelNumber(nameof (UpperHeadingLevel), value);
      this.OnChange();
      this.m_upperHeadingLevel = value;
      if (this.Document.IsOpening)
        return;
      this.UpdateTOCFieldCode();
    }
  }

  public int LowerHeadingLevel
  {
    get
    {
      this.OnGetValue();
      return this.m_lowerHeadingLevel;
    }
    set
    {
      this.CheckLevelNumber(nameof (LowerHeadingLevel), value);
      this.OnChange();
      this.m_lowerHeadingLevel = value;
      if (this.Document.IsOpening)
        return;
      this.UpdateTOCFieldCode();
    }
  }

  public bool UseTableEntryFields
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set
    {
      this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
      if (this.Document.IsOpening)
        return;
      this.UpdateTOCFieldCode();
    }
  }

  public string TableID
  {
    get => this.m_tableID;
    set
    {
      this.m_tableID = value;
      if (this.Document.IsOpening)
        return;
      this.UpdateTOCFieldCode();
    }
  }

  public bool RightAlignPageNumbers
  {
    get
    {
      this.OnGetValue();
      return ((int) this.m_bFlags & 4) >> 2 != 0;
    }
    set
    {
      this.OnChange();
      this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
      if (this.Document.IsOpening)
        return;
      this.UpdateTOCFieldCode();
    }
  }

  public bool IncludePageNumbers
  {
    get
    {
      this.OnGetValue();
      return ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    }
    set
    {
      this.OnChange();
      this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
      if (this.Document.IsOpening)
        return;
      this.UpdateTOCFieldCode();
    }
  }

  public bool UseHyperlinks
  {
    get
    {
      this.OnGetValue();
      return ((int) this.m_bFlags & 8) >> 3 != 0;
    }
    set
    {
      this.OnChange();
      this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
      if (this.Document.IsOpening)
        return;
      this.UpdateTOCFieldCode();
    }
  }

  public bool UseOutlineLevels
  {
    get
    {
      this.OnGetValue();
      return ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    }
    set
    {
      this.OnChange();
      this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
      if (this.Document.IsOpening)
        return;
      this.UpdateTOCFieldCode();
    }
  }

  public override EntityType EntityType => EntityType.TOC;

  internal string FormattingString
  {
    get => this.m_tocField.m_formattingString;
    set => this.m_tocField.m_formattingString = value;
  }

  internal WField TOCField => this.m_tocField;

  internal Dictionary<int, List<WParagraphStyle>> TOCStyles
  {
    get
    {
      if (this.m_tocStyles == null)
        this.m_tocStyles = new Dictionary<int, List<WParagraphStyle>>();
      return this.m_tocStyles;
    }
  }

  internal Dictionary<Entity, Entity> TOCEntryEntities
  {
    get => this.m_tocEntryEntities ?? (this.m_tocEntryEntities = new Dictionary<Entity, Entity>());
  }

  internal Dictionary<int, List<string>> TOCLevels
  {
    get
    {
      if (this.m_tocLevels == null)
        this.m_tocLevels = new Dictionary<int, List<string>>();
      return this.m_tocLevels;
    }
  }

  private WParagraph LastTOCParagraph
  {
    get
    {
      if (this.m_tocParagraph == null)
        this.m_tocParagraph = this.OwnerParagraph;
      return this.m_tocParagraph;
    }
  }

  private bool InvalidFormatString
  {
    get => ((int) this.m_bFlags & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 191 | (value ? 1 : 0) << 6);
  }

  private bool FormattingParsed
  {
    get => ((int) this.m_bFlags & 128 /*0x80*/) >> 1 != 0;
    set
    {
      this.m_bFlags = (byte) ((int) this.m_bFlags & (int) sbyte.MaxValue | (value ? 1 : 0) << 7);
    }
  }

  public TableOfContent(IWordDocument doc)
    : base(doc as WordDocument)
  {
    this.m_tocField = new WField(doc);
    this.m_tocField.FieldType = FieldType.FieldTOC;
    this.m_tableID = string.Empty;
  }

  public TableOfContent(IWordDocument doc, string switches)
    : this(doc)
  {
    this.TOCField.m_formattingString = switches;
    this.m_bFlags &= (byte) 247;
    this.ParseSwitches();
  }

  public void SetTOCLevelStyle(int levelNumber, string styleName)
  {
    this.CheckLevelNumber(nameof (levelNumber), levelNumber);
    this.SetStyleForTOCLevel(levelNumber, styleName, true);
    if (this.Document.IsOpening)
      return;
    this.UpdateTOCFieldCode();
  }

  public string GetTOCLevelStyle(int levelNumber) => this.GetTOCLevelStyles(levelNumber)[0];

  public List<string> GetTOCLevelStyles(int levelNumber)
  {
    if (levelNumber < this.m_lowerHeadingLevel || levelNumber > this.m_upperHeadingLevel)
      throw new ArgumentException("Level index must be >= LowerHeadingLevel and <= UpperHeadingLevel");
    this.ParseSwitches();
    List<string> tocLevelStyles = new List<string>();
    if (this.m_tocStyles.ContainsKey(levelNumber))
    {
      foreach (WParagraphStyle wparagraphStyle in this.m_tocStyles[levelNumber])
        tocLevelStyles.Add(wparagraphStyle.Name);
    }
    else
    {
      WParagraphStyle builtinStyle = this.GetBuiltinStyle((BuiltinStyle) levelNumber) as WParagraphStyle;
      tocLevelStyles.Add(builtinStyle.Name);
    }
    return tocLevelStyles;
  }

  internal WField FindKey()
  {
    foreach (KeyValuePair<WField, TableOfContent> keyValuePair in this.Document.TOC)
    {
      if (keyValuePair.Value == this)
        return keyValuePair.Key;
    }
    return (WField) null;
  }

  private void ParseSwitches()
  {
    if (this.FormattingParsed)
      return;
    string str = this.TOCField.m_formattingString;
    if (str.Contains("\\* MERGEFORMAT"))
      str = str.Remove(str.IndexOf("\\* MERGEFORMAT")).Trim();
    else if (str.Contains("\\* Mergeformat"))
      str = str.Remove(str.IndexOf("\\* Mergeformat")).Trim();
    string[] strArray = str.Split('\\');
    bool isHeadingLevelDefined = false;
    int index = 0;
    for (int length = strArray.Length; index < length; ++index)
    {
      string optionString = strArray[index];
      if (optionString.Length != 0)
      {
        switch (optionString[0])
        {
          case 'f':
            this.ParseUseField(optionString);
            continue;
          case 'h':
            this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | 8);
            continue;
          case 'n':
            this.m_bFlags &= (byte) 223;
            continue;
          case 'o':
            this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | 1);
            isHeadingLevelDefined = true;
            this.ParseHeadingLevels(optionString);
            continue;
          case 'p':
            this.m_bFlags &= (byte) 251;
            continue;
          case 't':
            this.ParseHeaderStylesAndUpdateLevel(optionString, isHeadingLevelDefined);
            continue;
          case 'u':
            this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | 16 /*0x10*/);
            continue;
          default:
            continue;
        }
      }
    }
    if (this.TOCStyles.Count == 0 && !isHeadingLevelDefined)
      this.m_upperHeadingLevel = 9;
    this.FormattingParsed = true;
  }

  private IWParagraphStyle GetBuiltinStyle(BuiltinStyle builtinStyle)
  {
    if (!(this.m_doc.Styles.FindByName(Style.BuiltInToName(builtinStyle), StyleType.ParagraphStyle) is IWParagraphStyle builtinStyle1))
    {
      builtinStyle1 = (IWParagraphStyle) Style.CreateBuiltinStyle(builtinStyle, this.m_doc);
      this.m_doc.Styles.Add((IStyle) builtinStyle1);
    }
    return builtinStyle1;
  }

  private void CreateDefStylesColl()
  {
    for (int key = 1; key <= 9; ++key)
    {
      List<WParagraphStyle> wparagraphStyleList = new List<WParagraphStyle>();
      BuiltinStyle builtinStyle = (BuiltinStyle) key;
      wparagraphStyleList.Add(this.GetBuiltinStyle(builtinStyle) as WParagraphStyle);
      this.TOCStyles.Add(key, wparagraphStyleList);
    }
  }

  private void UpdateFormattingString()
  {
    if (!this.InvalidFormatString)
      return;
    this.TOCField.m_formattingString = string.Empty;
    if (((int) this.m_bFlags & 1) != 0)
      this.UpdateTOCLevels();
    this.UpdateHeaderStyles();
    this.UpdateUsePageNumbers();
    this.UpdatePageNumberAlign();
    this.UpdateUseField();
    this.UpdateHyperlinks();
    this.UpdateUseOutlineLevels();
    this.FormattingParsed = true;
  }

  internal void UpdateTOCFieldCode()
  {
    this.UpdateFormattingString();
    if (this.NextSibling is WTextRange && (this.NextSibling as WTextRange).NextSibling is WFieldMark)
    {
      (this.NextSibling as WTextRange).Text = "TOC " + this.m_tocField.FormattingString;
    }
    else
    {
      this.RemovePreviousFieldCodeItems();
      this.OwnerParagraph.Items.Insert(this.Index + 1, (IEntity) new WTextRange((IWordDocument) this.Document)
      {
        Text = ("TOC " + this.m_tocField.FormattingString)
      });
    }
  }

  private void RemovePreviousFieldCodeItems()
  {
    for (int index = this.Index + 1; index < this.OwnerParagraph.Items.Count && !(this.OwnerParagraph.Items[index] is WFieldMark); index = index - 1 + 1)
      this.OwnerParagraph.Items.RemoveAt(index);
  }

  private void UpdateTOCLevels()
  {
    this.TOCField.m_formattingString += $"\\o \"{this.m_lowerHeadingLevel}-{this.m_upperHeadingLevel}\" ";
  }

  private void UpdateHyperlinks()
  {
    if (((int) this.m_bFlags & 8) >> 3 == 0)
      return;
    this.TOCField.m_formattingString += "\\h \\z ";
  }

  private void UpdateUsePageNumbers()
  {
    if (this.IncludePageNumbers)
      return;
    WField tocField = this.TOCField;
    tocField.m_formattingString = $"{tocField.m_formattingString}\\{(object) 'n'} ";
  }

  private void UpdatePageNumberAlign()
  {
    if (((int) this.m_bFlags & 4) >> 2 != 0)
      return;
    WField tocField = this.TOCField;
    tocField.m_formattingString = $"{tocField.m_formattingString}\\{(object) 'p'} \" \" ";
  }

  private void UpdateUseOutlineLevels()
  {
    if (((int) this.m_bFlags & 16 /*0x10*/) >> 4 == 0)
      return;
    WField tocField = this.TOCField;
    tocField.m_formattingString = $"{tocField.m_formattingString}\\{(object) 'u'} ";
  }

  private void UpdateUseField()
  {
    if (!this.UseTableEntryFields)
      return;
    WField tocField = this.TOCField;
    tocField.m_formattingString = $"{tocField.m_formattingString}\\{(object) 'f'} {this.m_tableID}";
  }

  private void UpdateHeaderStyles()
  {
    if (this.m_tocStyles == null || this.m_tocStyles.Count == 0)
      return;
    WField tocField1 = this.TOCField;
    tocField1.m_formattingString = $"{tocField1.m_formattingString}\\{(object) 't'} \"";
    for (int lowerHeadingLevel = this.m_lowerHeadingLevel; lowerHeadingLevel <= this.m_upperHeadingLevel; ++lowerHeadingLevel)
    {
      if (this.TOCStyles.ContainsKey(lowerHeadingLevel))
      {
        foreach (WParagraphStyle wparagraphStyle in this.TOCStyles[lowerHeadingLevel])
        {
          if (Style.BuiltinStyleLoader.BuiltinStyleNames[lowerHeadingLevel] != wparagraphStyle.Name)
          {
            WField tocField2 = this.TOCField;
            tocField2.m_formattingString = tocField2.m_formattingString + wparagraphStyle.Name + this.m_lstSepar + (object) lowerHeadingLevel + this.m_lstSepar;
          }
        }
      }
    }
    this.TOCField.m_formattingString += "\"";
  }

  private void ParseHeadingLevels(string optionString)
  {
    MatchCollection matchCollection = new Regex("[0-9]").Matches(optionString);
    if (matchCollection.Count == 2)
    {
      this.m_lowerHeadingLevel = int.Parse(matchCollection[0].Groups[0].Value);
      this.m_upperHeadingLevel = int.Parse(matchCollection[1].Groups[0].Value);
    }
    else
    {
      if (matchCollection.Count != 0)
        return;
      this.m_upperHeadingLevel = 9;
    }
  }

  private void ParseNumberAlignment(string optionString)
  {
    if (new Regex("[\"][ ][\"]").Match(optionString).Captures.Count != 1)
      return;
    this.m_bFlags &= (byte) 251;
  }

  private void ParseUseField(string optionString)
  {
    this.UseTableEntryFields = true;
    this.m_tableID = optionString.Substring(1, optionString.Length - 1).Trim();
  }

  private void ParseHeaderStylesAndUpdateLevel(string optionString, bool isHeadingLevelDefined)
  {
    char ch = this.m_lstSepar.ToCharArray()[0];
    optionString = optionString.Replace('“', '"');
    optionString = optionString.Replace('”', '"');
    string[] strArray = optionString.Split('"')[1].Split(ch);
    bool flag1 = false;
    bool flag2 = false;
    List<string> stringList = new List<string>();
    for (int index = 0; index < strArray.Length; ++index)
    {
      if (flag1)
        ++index;
      if (index < strArray.Length)
      {
        if (index != strArray.Length - 1)
        {
          if (!this.IsHeadingStyleContainsLevel(strArray[index + 1]))
            flag2 = true;
        }
        else
        {
          if (!this.IsHeadingStyleContainsLevel(strArray[index]))
            flag2 = true;
          if (string.IsNullOrEmpty(strArray[index]))
            continue;
        }
        if (!stringList.Contains(strArray[index]))
        {
          if (!this.IsHeadingStyleContainsLevel(strArray[index]))
            stringList.Add(strArray[index]);
          else
            stringList.Add(strArray[index + 1]);
          int levelNumber;
          if (flag2)
          {
            levelNumber = index == 0 || index == strArray.Length - 1 ? 1 : (!flag1 ? stringList.Count : 9);
            flag1 = false;
            flag2 = false;
          }
          else
          {
            levelNumber = int.Parse(strArray[index + 1]);
            flag1 = true;
          }
          this.SetStyleForTOCLevel(levelNumber, strArray[index], false);
        }
      }
      else
        break;
    }
    if (this.TOCStyles.Count > 0 && !isHeadingLevelDefined)
      this.m_bFlags &= (byte) 254;
    this.InvalidFormatString = false;
  }

  private bool IsHeadingStyleContainsLevel(string text)
  {
    int result = 0;
    return int.TryParse(text, out result);
  }

  private void OnChange()
  {
    this.ParseSwitches();
    this.InvalidFormatString = true;
  }

  private void OnGetValue() => this.ParseSwitches();

  private void SetStyleForTOCLevel(int levelNumber, string styleName, bool onSetProperty)
  {
    if (onSetProperty)
      this.OnChange();
    BuiltinStyle builtIn = Style.NameToBuiltIn(styleName);
    IWParagraphStyle wparagraphStyle1 = this.m_doc.Styles.FindByName(styleName, StyleType.ParagraphStyle) as IWParagraphStyle;
    IWParagraphStyle byName = this.m_doc.Styles.FindByName(styleName.ToLower(), StyleType.ParagraphStyle) as IWParagraphStyle;
    if ((wparagraphStyle1 != null || builtIn != BuiltinStyle.User) && wparagraphStyle1 == null && byName == null)
    {
      wparagraphStyle1 = (IWParagraphStyle) Style.CreateBuiltinStyle(builtIn, this.m_doc);
      this.m_doc.Styles.Add((IStyle) wparagraphStyle1);
    }
    IWParagraphStyle wparagraphStyle2 = (IWParagraphStyle) null;
    if (wparagraphStyle1 != null)
      wparagraphStyle2 = wparagraphStyle1;
    else if (byName != null)
      wparagraphStyle2 = byName;
    if (wparagraphStyle2 == null)
      return;
    if (this.TOCStyles.ContainsKey(levelNumber))
    {
      List<WParagraphStyle> tocStyle = this.TOCStyles[levelNumber];
      if (tocStyle.Contains(wparagraphStyle2 as WParagraphStyle))
        return;
      tocStyle.Add(wparagraphStyle2 as WParagraphStyle);
    }
    else
      this.TOCStyles.Add(levelNumber, new List<WParagraphStyle>()
      {
        wparagraphStyle2 as WParagraphStyle
      });
  }

  private void CheckLevelNumber(string parameterName, int levelNumber)
  {
    if (levelNumber < 1 || levelNumber > 9)
      throw new ArgumentOutOfRangeException(parameterName, "Level number value must be greater than 1 and smaller than 10.");
  }

  internal List<string> UpdateTOCStyleLevels()
  {
    List<string> stringList1 = new List<string>();
    List<string> stringList2 = new List<string>();
    if (this.TOCLevels.Count > 0)
      this.TOCLevels.Clear();
    if (this.TOCStyles.Count > 0)
    {
      foreach (KeyValuePair<int, List<WParagraphStyle>> tocStyle in this.TOCStyles)
      {
        if (tocStyle.Key >= this.m_lowerHeadingLevel && tocStyle.Key <= this.m_upperHeadingLevel)
        {
          List<string> stringList3 = new List<string>();
          foreach (WParagraphStyle wparagraphStyle in tocStyle.Value)
          {
            if (!stringList2.Contains(wparagraphStyle.Name))
            {
              stringList3.Add(wparagraphStyle.Name);
              stringList2.Add(wparagraphStyle.Name);
            }
          }
          this.TOCLevels.Add(tocStyle.Key, stringList3);
        }
      }
    }
    if (this.UseHeadingStyles)
    {
      string str1 = "Heading ";
      for (int lowerHeadingLevel = this.m_lowerHeadingLevel; lowerHeadingLevel <= this.m_upperHeadingLevel; ++lowerHeadingLevel)
      {
        string str2 = str1 + lowerHeadingLevel.ToString();
        if (!stringList2.Contains(str2))
        {
          List<string> stringList4;
          if (this.TOCLevels.ContainsKey(lowerHeadingLevel))
          {
            stringList4 = this.TOCLevels[lowerHeadingLevel];
          }
          else
          {
            stringList4 = new List<string>();
            this.TOCLevels.Add(lowerHeadingLevel, stringList4);
          }
          stringList4.Add(str2);
          stringList2.Add(str2);
        }
      }
      foreach (Style style in (IEnumerable) this.Document.Styles)
      {
        if (style is WParagraphStyle)
        {
          int key = (int) (byte) (style as WParagraphStyle).ParagraphFormat.OutlineLevel + 1;
          string name = (style as WParagraphStyle).Name;
          if (key > 0 && key <= 9 && key >= this.m_lowerHeadingLevel && key <= this.m_upperHeadingLevel && !name.ToLower().StartsWithExt("heading") && !name.ToLower().StartsWithExt("normal") && !stringList2.Contains(name))
          {
            List<string> stringList5;
            if (this.TOCLevels.ContainsKey(key))
            {
              stringList5 = this.TOCLevels[key];
            }
            else
            {
              stringList5 = new List<string>();
              this.TOCLevels.Add(key, stringList5);
            }
            stringList5.Add(name);
            stringList2.Add(name);
          }
        }
      }
    }
    foreach (Style style in (IEnumerable) this.Document.Styles)
    {
      if (style is WCharacterStyle && (style as WCharacterStyle).LinkStyle != null && stringList2.Contains((style as WCharacterStyle).LinkStyle) && !stringList1.Contains((style as WCharacterStyle).Name))
        stringList1.Add((style as WCharacterStyle).Name);
    }
    stringList2.Clear();
    return stringList1;
  }

  internal List<ParagraphItem> ParseDocument(List<string> tocLinkCharacterStyleNames)
  {
    List<ParagraphItem> tocParaItems = new List<ParagraphItem>();
    bool isParsingTOCParagraph = false;
    foreach (IWSection section in (CollectionImpl) this.Document.Sections)
      this.ParseTextBody(section.Body, tocLinkCharacterStyleNames, tocParaItems, ref isParsingTOCParagraph);
    return tocParaItems;
  }

  private void ParseTextBody(
    WTextBody textBody,
    List<string> tocLinkCharacterStyleNames,
    List<ParagraphItem> tocParaItems,
    ref bool isParsingTOCParagraph)
  {
    for (int index = 0; index < textBody.Items.Count; ++index)
    {
      if (textBody.Items[index] is WParagraph)
      {
        IWParagraph paragraph = (IWParagraph) (textBody.Items[index] as WParagraph);
        this.ParseParagraph(paragraph, tocLinkCharacterStyleNames, tocParaItems, ref isParsingTOCParagraph);
        index = textBody.Items.IndexOf((IEntity) paragraph);
      }
      else if (textBody.Items[index] is WTable)
      {
        IWTable childEntity = (IWTable) (textBody.ChildEntities[index] as WTable);
        this.ParseTable(childEntity, tocLinkCharacterStyleNames, tocParaItems, ref isParsingTOCParagraph);
        index = textBody.Items.IndexOf((IEntity) childEntity);
      }
      else if (textBody.Items[index] is BlockContentControl)
      {
        BlockContentControl childEntity = textBody.ChildEntities[index] as BlockContentControl;
        this.ParseTextBody(childEntity.TextBody, tocLinkCharacterStyleNames, tocParaItems, ref isParsingTOCParagraph);
        index = textBody.Items.IndexOf((IEntity) childEntity);
      }
    }
  }

  private void ParseTable(
    IWTable table,
    List<string> tocLinkCharacterStyleNames,
    List<ParagraphItem> tocParaItems,
    ref bool isParsingTOCParagraph)
  {
    for (int index1 = 0; index1 < table.Rows.Count; ++index1)
    {
      WTableRow row = table.Rows[index1];
      for (int index2 = 0; index2 < row.Cells.Count; ++index2)
        this.ParseTextBody((WTextBody) row.Cells[index2], tocLinkCharacterStyleNames, tocParaItems, ref isParsingTOCParagraph);
    }
  }

  private void ParseParagraph(
    IWParagraph paragraph,
    List<string> tocLinkCharacterStyleNames,
    List<ParagraphItem> tocParaItems,
    ref bool isParsingTOCParagraph)
  {
    bool isTocReferLinkStyle = false;
    bool flag1 = false;
    if ((!(paragraph as WParagraph).IsEmptyParagraph() || paragraph.ListFormat.ListType == ListType.Numbered) && (this.CheckParagraphStyle(paragraph.StyleName) || this.CheckAndGetTOCLinkStyle(paragraph as WParagraph, ref isTocReferLinkStyle, tocLinkCharacterStyleNames, tocParaItems) != ""))
    {
      this.CheckAndSplitParagraph(paragraph);
      if (!string.IsNullOrEmpty(paragraph.Text) || paragraph.ListFormat.ListType == ListType.Numbered || (paragraph as WParagraph).IsContainsInLineImage())
      {
        int startIndex = 0;
        int num1 = 0;
        int num2 = -1;
        bool flag2 = false;
        foreach (ParagraphItem paragraphItem in (CollectionImpl) paragraph.Items)
        {
          if (!isParsingTOCParagraph && this.OwnerParagraph == paragraph)
            isParsingTOCParagraph = true;
          switch (paragraphItem)
          {
            case WPicture _ when (paragraphItem as WPicture).TextWrappingStyle == TextWrappingStyle.Inline:
              startIndex = paragraph.Items.IndexOf((IEntity) paragraphItem);
              goto label_21;
            case WTextRange _ when !(paragraphItem is WField) && num1 == 0:
              if (this.OwnerParagraph != paragraph || !((paragraphItem as WTextRange).Text == "TOC"))
              {
                if ((paragraphItem as WTextRange).Text == ControlChar.Tab)
                {
                  num2 = -1;
                  continue;
                }
                startIndex = num2 > -1 ? num2 : paragraph.Items.IndexOf((IEntity) paragraphItem);
                goto label_21;
              }
              continue;
            case WField _:
            case TableOfContent _:
              if (paragraphItem is WField && num1 == 0)
                num2 = paragraph.Items.IndexOf((IEntity) paragraphItem);
              ++num1;
              flag2 = true;
              continue;
            default:
              if (flag2 && paragraphItem is WFieldMark && ((paragraphItem as WFieldMark).Type == FieldMarkType.FieldSeparator || (paragraphItem as WFieldMark).Type == FieldMarkType.FieldEnd))
              {
                flag2 = false;
                --num1;
                continue;
              }
              continue;
          }
        }
label_21:
        if (!isParsingTOCParagraph)
          this.InsertBookmark(paragraph, (WField) null, startIndex, paragraph.Items.Count + 1, ref isTocReferLinkStyle, tocLinkCharacterStyleNames, tocParaItems);
        flag1 = true;
      }
    }
    else if (!isParsingTOCParagraph && paragraph.ListFormat.CurrentListStyle != null)
    {
      WListLevel nearLevel = paragraph.ListFormat.CurrentListStyle.GetNearLevel(paragraph.ListFormat.ListLevelNumber);
      this.Document.UpdateListValue(paragraph as WParagraph, paragraph.ListFormat, nearLevel);
    }
    if (isParsingTOCParagraph && this.TOCField != null && paragraph.Items.IndexOf((IEntity) this.TOCField.FieldEnd) != -1)
      isParsingTOCParagraph = false;
    List<int> intList = new List<int>();
    for (int index = 0; index < paragraph.Items.Count; ++index)
    {
      if (paragraph.Items[index] is WTextBox)
        this.ParseTextBody((paragraph.Items[index] as WTextBox).TextBoxBody, tocLinkCharacterStyleNames, tocParaItems, ref isParsingTOCParagraph);
      else if (paragraph.Items[index] is Shape)
        this.ParseTextBody((paragraph.Items[index] as Shape).TextBody, tocLinkCharacterStyleNames, tocParaItems, ref isParsingTOCParagraph);
      else if (paragraph.Items[index] is WField && (paragraph.Items[index] as WField).FieldType == FieldType.FieldTOCEntry && this.UseTableEntryFields)
        intList.Add(index);
    }
    for (int index = 0; index < intList.Count; ++index)
    {
      int num3 = intList[index] + index * 2;
      intList.RemoveAt(index);
      WField field = paragraph.Items[num3] as WField;
      int num4 = num3;
      for (IEntity entity = (IEntity) field; entity != null; entity = entity.NextSibling)
      {
        ++num4;
        if (entity is WFieldMark && (entity as WFieldMark).Type == FieldMarkType.FieldEnd)
          break;
      }
      bool flag3 = false;
      string formattingString = field.FormattingString;
      char[] chArray = new char[1]{ '\\' };
      foreach (string str in formattingString.Split(chArray))
      {
        if (str.StartsWithExt("f"))
        {
          flag3 = !string.IsNullOrEmpty(str.Substring(1).Replace(" ", ""));
          break;
        }
      }
      if (!flag3)
      {
        this.InsertBookmark(paragraph, field, num3, num4 + 1, ref isTocReferLinkStyle, tocLinkCharacterStyleNames, tocParaItems);
        this.m_tocEntryLastEntity = (Entity) field;
      }
    }
    if (flag1)
      this.m_tocEntryLastEntity = (Entity) (paragraph as WParagraph);
    isTocReferLinkStyle = false;
  }

  private void CheckAndSplitParagraph(IWParagraph paragraph)
  {
    if (!paragraph.Text.Contains(ControlChar.Tab) && !paragraph.Text.Contains(ControlChar.LineFeed) && !paragraph.Text.Contains(ControlChar.CarriegeReturn))
      return;
    for (int index = 0; index < paragraph.Items.Count; ++index)
    {
      if (paragraph.Items[index] is WTextRange && !(paragraph.Items[index] is WField))
      {
        WTextRange textRange = paragraph.Items[index] as WTextRange;
        if (textRange.Text != ControlChar.Tab && textRange.Text.Contains(ControlChar.Tab))
          this.UpdateTabCharacters(textRange);
        if (textRange.Text.Contains(ControlChar.LineFeed))
        {
          this.UpdateNewLineCharacters(textRange, ControlChar.LineFeed);
          break;
        }
        if (textRange.Text.Contains(ControlChar.CarriegeReturn))
        {
          this.UpdateNewLineCharacters(textRange, ControlChar.CarriegeReturn);
          break;
        }
      }
    }
  }

  private void UpdateTabCharacters(WTextRange textRange)
  {
    WParagraph ownerParagraph = textRange.OwnerParagraph;
    string text = textRange.Text;
    int num1 = text.IndexOf(ControlChar.Tab);
    int num2 = ownerParagraph.Items.IndexOf((IEntity) textRange);
    string str = text.Substring(num1 + 1);
    WTextRange wtextRange = textRange.Clone() as WTextRange;
    if (num1 > 0)
    {
      wtextRange.Text = text.Substring(num1);
      textRange.Text = text.Substring(0, num1);
    }
    else if (str != string.Empty)
    {
      wtextRange.Text = str;
      textRange.Text = ControlChar.Tab;
    }
    ownerParagraph.Items.Insert(num2 + 1, (IEntity) wtextRange);
  }

  private void UpdateNewLineCharacters(WTextRange textRange, string splitText)
  {
    WParagraph ownerParagraph = textRange.OwnerParagraph;
    string text = textRange.Text;
    int length = text.IndexOf(splitText);
    int num = ownerParagraph.Items.IndexOf((IEntity) textRange);
    string str = text.Substring(length + 1);
    if (str != string.Empty)
    {
      WTextRange wtextRange = textRange.Clone() as WTextRange;
      wtextRange.Text = str;
      ownerParagraph.Items.Insert(num + 1, (IEntity) wtextRange);
      textRange.Text = text.Substring(0, length);
    }
    else
      textRange.Text = text.Substring(0, length);
    this.CreateParagraph(ownerParagraph, num + 1);
    if (!(textRange.Text == string.Empty))
      return;
    ownerParagraph.Items.Remove((IEntity) textRange);
  }

  private void CreateParagraph(WParagraph paragraph, int index)
  {
    WTextBody ownerTextBody = paragraph.OwnerTextBody;
    int num = ownerTextBody.Items.IndexOf((IEntity) paragraph);
    WParagraph wparagraph = paragraph.Clone() as WParagraph;
    wparagraph.Items.Clear();
    ownerTextBody.Items.Insert(num + 1, (IEntity) wparagraph);
    while (paragraph.Items.Count > index)
      wparagraph.Items.Insert(wparagraph.Items.Count, (IEntity) paragraph.Items[index]);
  }

  private void SplitTOCParagraph(WParagraph tocParagraph, ref int paraIndex)
  {
    if (tocParagraph.ChildEntities.Count <= 0 || tocParagraph.ChildEntities[0] is TableOfContent || !this.ContainsValidItems(tocParagraph))
      return;
    WParagraph wparagraph = tocParagraph.Clone() as WParagraph;
    wparagraph.ChildEntities.Clear();
    wparagraph.ChildEntities.Add((IEntity) tocParagraph.ChildEntities[0]);
    while (tocParagraph.ChildEntities.Count > 0 && !(tocParagraph.ChildEntities[0] is TableOfContent))
      wparagraph.ChildEntities.Add((IEntity) tocParagraph.ChildEntities[0]);
    tocParagraph.OwnerTextBody.ChildEntities.Insert(paraIndex, (IEntity) wparagraph);
    ++paraIndex;
  }

  private bool ContainsValidItems(WParagraph paragraph)
  {
    foreach (Entity childEntity in (CollectionImpl) paragraph.ChildEntities)
    {
      switch (childEntity)
      {
        case BookmarkStart _:
        case BookmarkEnd _:
          continue;
        case TableOfContent _:
          goto label_8;
        default:
          return true;
      }
    }
label_8:
    return false;
  }

  internal void RemoveUpdatedTocEntries()
  {
    WParagraph ownerParagraph1 = this.OwnerParagraph;
    WTextBody ownerTextBody = ownerParagraph1.OwnerTextBody;
    int paraIndex = ownerTextBody.Items.IndexOf((IEntity) ownerParagraph1);
    this.SplitTOCParagraph(ownerParagraph1, ref paraIndex);
    WParagraph ownerParagraph2 = this.OwnerParagraph;
    bool flag1 = true;
    bool flag2 = false;
    int index1 = paraIndex;
    int num1 = 0;
    for (; index1 < ownerTextBody.Items.Count; ++index1)
    {
      if (ownerTextBody.Items[index1] is WParagraph)
      {
        WParagraph wparagraph = ownerTextBody.Items[index1] as WParagraph;
        int num2 = 0;
        if (index1 == paraIndex)
          num2 = wparagraph.Items.IndexOf((IEntity) this);
        int num3;
        for (int index2 = num2; index2 < wparagraph.Items.Count; index2 = num3 + 1)
        {
          if (wparagraph.Items[index2] == this)
          {
            for (int index3 = index2; index3 + 1 < wparagraph.Items.Count; ++index3)
            {
              if (wparagraph.Items[index3 + 1] is WFieldMark && (wparagraph.Items[index3 + 1] as WFieldMark).Type == FieldMarkType.FieldSeparator)
              {
                index2 = index3;
                break;
              }
            }
            if (wparagraph.Items[index2 + 1] is WFieldMark && (wparagraph.Items[index2 + 1] as WFieldMark).Type == FieldMarkType.FieldSeparator)
            {
              ++num1;
              num3 = index2 + 1;
              continue;
            }
          }
          if (wparagraph.Items[index2] is WFieldMark)
          {
            WFieldMark wfieldMark = wparagraph.Items[index2] as WFieldMark;
            if (wfieldMark.Type == FieldMarkType.FieldSeparator)
              ++num1;
            else if (wfieldMark.Type == FieldMarkType.FieldEnd)
            {
              flag2 = this.TOCField.FieldEnd == wfieldMark;
              if (index2 != 0 && !flag2 || num1 != 1)
                --num1;
              else
                break;
            }
          }
          else if (wparagraph.Items[index2] is WTextRange && (wparagraph.Items[index2] as WTextRange).Text == "TOC")
          {
            flag1 = false;
            break;
          }
          wparagraph.Items.Remove((IEntity) wparagraph.Items[index2]);
          num3 = index2 - 1;
        }
        if (wparagraph.Items.Count == 0)
        {
          ownerTextBody.Items.Remove((IEntity) wparagraph);
          --index1;
        }
        else
        {
          if ((wparagraph.Items[0] is WFieldMark || flag2) && flag1)
          {
            int index4 = 0;
            for (int index5 = this.Index; index5 < ownerParagraph2.Items.Count; ++index5)
            {
              ParagraphItem paragraphItem = ownerParagraph2.Items[index5];
              wparagraph.Items.Insert(index4, (IEntity) paragraphItem);
              ++index4;
              if (!(paragraphItem is WFieldMark) || (paragraphItem as WFieldMark).Type != FieldMarkType.FieldSeparator)
              {
                if (wparagraph != ownerParagraph2)
                  --index5;
              }
              else
                break;
            }
            wparagraph.Items.Insert(index4, (IEntity) new WTextRange((IWordDocument) this.Document)
            {
              Text = "TOC"
            });
            if (wparagraph == ownerParagraph2)
              break;
            ownerTextBody.Items.Remove((IEntity) ownerParagraph2);
            break;
          }
          if (!flag1)
            break;
        }
      }
    }
  }

  internal void RemoveExistingTocBookmarks()
  {
    for (int index = this.Document.Bookmarks.Count - 1; index >= 0; --index)
    {
      Bookmark bookmark = this.Document.Bookmarks[index];
      if (bookmark.Name.StartsWithExt("_Toc"))
        this.Document.Bookmarks.Remove(bookmark);
    }
  }

  private bool CheckParagraphStyle(string styleName)
  {
    styleName = styleName == null ? "normal" : styleName.ToLower().Replace(" ", "");
    foreach (KeyValuePair<int, List<string>> tocLevel in this.TOCLevels)
    {
      foreach (string str1 in tocLevel.Value)
      {
        string str2 = str1.ToLower().Replace(" ", "");
        if (styleName.StartsWithExt(str2))
          return true;
      }
    }
    return false;
  }

  private string CheckAndGetTOCLinkStyle(
    WParagraph paragraph,
    ref bool isTocReferLinkStyle,
    List<string> tocLinkCharacterStyleNames,
    List<ParagraphItem> tocParaItems)
  {
    int tocValidItemIndex = this.GetTOCValidItemIndex(paragraph);
    if (tocValidItemIndex == int.MinValue)
      return "";
    for (int index = tocValidItemIndex; index < paragraph.ChildEntities.Count; ++index)
    {
      switch (paragraph.ChildEntities[index].EntityType)
      {
        case EntityType.TextRange:
          if (!tocLinkCharacterStyleNames.Contains((paragraph.ChildEntities[index] as WTextRange).CharacterFormat.CharStyleName))
            return "";
          isTocReferLinkStyle = true;
          if (!tocParaItems.Contains(paragraph.ChildEntities[index] as ParagraphItem))
          {
            ParagraphItem childEntity = paragraph.ChildEntities[index] as ParagraphItem;
            tocParaItems.Add(childEntity);
            this.m_tocEntryLastEntity = (Entity) childEntity;
          }
          return (paragraph.ChildEntities[index] as WTextRange).CharacterFormat.CharStyleName;
        case EntityType.Picture:
          if (!tocLinkCharacterStyleNames.Contains((paragraph.ChildEntities[index] as WPicture).CharacterFormat.CharStyleName))
            return "";
          isTocReferLinkStyle = true;
          if (!tocParaItems.Contains(paragraph.ChildEntities[index] as ParagraphItem))
          {
            ParagraphItem childEntity = paragraph.ChildEntities[index] as ParagraphItem;
            tocParaItems.Add(childEntity);
            this.m_tocEntryLastEntity = (Entity) childEntity;
          }
          return (paragraph.ChildEntities[index] as WPicture).CharacterFormat.CharStyleName;
        case EntityType.MergeField:
          if (!tocLinkCharacterStyleNames.Contains((paragraph.ChildEntities[index] as WMergeField).CharacterFormat.CharStyleName))
            return "";
          isTocReferLinkStyle = true;
          if (!tocParaItems.Contains(paragraph.ChildEntities[index] as ParagraphItem))
          {
            ParagraphItem childEntity = paragraph.ChildEntities[index] as ParagraphItem;
            tocParaItems.Add(childEntity);
            this.m_tocEntryLastEntity = (Entity) childEntity;
          }
          return (paragraph.ChildEntities[index] as WMergeField).CharacterFormat.CharStyleName;
        default:
          continue;
      }
    }
    return "";
  }

  private int GetTOCLevel(string styleName)
  {
    int tocLevel1 = 0;
    styleName = styleName.ToLower().Replace(" ", "");
    foreach (KeyValuePair<int, List<string>> tocLevel2 in this.TOCLevels)
    {
      foreach (string str1 in tocLevel2.Value)
      {
        string str2 = str1.ToLower().Replace(" ", "");
        if (styleName.StartsWithExt(str2))
        {
          tocLevel1 = tocLevel2.Key;
          return tocLevel1;
        }
      }
    }
    return tocLevel1;
  }

  private void InsertBookmark(
    IWParagraph paragraph,
    WField field,
    int startIndex,
    int endIndex,
    ref bool isTocReferLinkStyle,
    List<string> tocLinkCharacterStyleNames,
    List<ParagraphItem> tocParaItems)
  {
    string bookmarkName = this.GenerateBookmarkName();
    BookmarkStart bookmarkStart = new BookmarkStart((IWordDocument) this.Document, bookmarkName);
    paragraph.Items.Insert(startIndex, (IEntity) bookmarkStart);
    this.InsertBookmarkHyperlink(paragraph, field, bookmarkName, ref isTocReferLinkStyle, tocLinkCharacterStyleNames, tocParaItems);
    if (field == null)
      endIndex = paragraph.Items.Count;
    BookmarkEnd bookmarkEnd = new BookmarkEnd((IWordDocument) this.Document, bookmarkName);
    paragraph.Items.Insert(endIndex, (IEntity) bookmarkEnd);
  }

  private void InsertBookmarkHyperlink(
    IWParagraph paragraph,
    WField field,
    string bookmark,
    ref bool isTocReferLinkStyle,
    List<string> tocLinkCharacterStyleNames,
    List<ParagraphItem> tocParaItems)
  {
    WParagraph paragraph1 = paragraph as WParagraph;
    int result = isTocReferLinkStyle ? this.GetTOCLevel(this.CheckAndGetTOCLinkStyle(paragraph1, ref isTocReferLinkStyle, tocLinkCharacterStyleNames, tocParaItems)) : this.GetTOCLevel(paragraph.StyleName);
    string text = string.Empty;
    bool flag = true;
    if (field != null)
    {
      text = field.FieldValue;
      result = 1;
      string formattingString = field.FormattingString;
      char[] chArray = new char[1]{ '\\' };
      foreach (string str in formattingString.Split(chArray))
      {
        if (str.StartsWithExt("l"))
        {
          if (!int.TryParse(str.Substring(1).Replace(" ", ""), out result))
            result = 1;
        }
        else if (str.StartsWithExt("n"))
          flag = false;
      }
    }
    WParagraph tocParagraph = this.CreateTOCParagraph(result);
    this.CreateHyperlink(paragraph, tocParagraph, text, bookmark, ref isTocReferLinkStyle, tocLinkCharacterStyleNames);
    if (this.IncludePageNumbers && flag)
      this.AddTabsAndPageRefField(tocParagraph, bookmark);
    if (tocParagraph == null || this.TOCEntryEntities.ContainsKey((Entity) tocParagraph))
      return;
    if (isTocReferLinkStyle && this.m_tocEntryLastEntity != null)
    {
      this.TOCEntryEntities.Add((Entity) tocParagraph, this.m_tocEntryLastEntity);
    }
    else
    {
      if (field == null && paragraph1 == null)
        return;
      if (field != null)
        this.TOCEntryEntities.Add((Entity) tocParagraph, (Entity) field);
      else
        this.TOCEntryEntities.Add((Entity) tocParagraph, (Entity) paragraph1);
    }
  }

  private void CreateHyperlink(
    IWParagraph paragraph,
    WParagraph tocParagraph,
    string text,
    string bookmark,
    ref bool isTocReferLinkStyle,
    List<string> tocLinkStyles)
  {
    WField hyperlink1 = new WField((IWordDocument) this.Document);
    hyperlink1.FieldType = FieldType.FieldHyperlink;
    hyperlink1.CharacterFormat.CharStyleName = "Hyperlink";
    tocParagraph.Items.Add((IEntity) hyperlink1);
    tocParagraph.Items.Add((IEntity) new WTextRange((IWordDocument) this.Document)
    {
      Text = "HYPERLINK"
    });
    hyperlink1.FieldSeparator = tocParagraph.AppendFieldMark(FieldMarkType.FieldSeparator);
    hyperlink1.FieldSeparator.CharacterFormat.CharStyleName = "Hyperlink";
    if (!string.IsNullOrEmpty(text))
    {
      if (isTocReferLinkStyle)
      {
        bool flag = false;
        WParagraphStyle paraStyle = (paragraph as WParagraph).ParaStyle as WParagraphStyle;
        foreach (ParagraphItem paragraphItem in (CollectionImpl) paragraph.Items)
        {
          if (paragraphItem is WField && (paragraphItem as WField).FieldType == FieldType.FieldTOCEntry)
            flag = true;
          WTextRange paragraphTextRange = paragraphItem is WField ? (WTextRange) null : paragraphItem as WTextRange;
          if (flag && paragraphTextRange != null && paragraphTextRange.Text != "\" " && paragraphTextRange.Text != ControlChar.Tab)
            this.AppendTextToTocParagraph(paragraphTextRange, paragraphTextRange.Text, paraStyle, tocParagraph);
          if (flag && paragraphItem is WFieldMark)
          {
            if ((paragraphItem as WFieldMark).Type == FieldMarkType.FieldEnd)
              break;
          }
        }
      }
      else
        tocParagraph.AppendText(text).CharacterFormat.CharStyleName = "Hyperlink";
    }
    else
      this.CreateHyperLink(paragraph, tocParagraph, isTocReferLinkStyle, tocLinkStyles);
    WFieldMark wfieldMark = new WFieldMark((IWordDocument) this.Document, FieldMarkType.FieldEnd);
    tocParagraph.Items.Add((IEntity) wfieldMark);
    wfieldMark.CharacterFormat.CharStyleName = "Hyperlink";
    hyperlink1.FieldEnd = wfieldMark;
    Hyperlink hyperlink2 = new Hyperlink(hyperlink1)
    {
      Type = HyperlinkType.Bookmark,
      BookmarkName = bookmark
    };
  }

  private void CreateHyperLink(
    IWParagraph paragraph,
    WParagraph tocParagraph,
    bool isTocReferLinkStyle,
    List<string> tocLinkStyles)
  {
    bool isTabAdded = false;
    this.UpdateList(paragraph, tocParagraph, ref isTabAdded);
    WParagraphStyle paraStyle = (paragraph as WParagraph).ParaStyle as WParagraphStyle;
    if (!isTocReferLinkStyle)
    {
      int num = 0;
      bool flag = false;
      for (int index = 0; index < paragraph.ChildEntities.Count; ++index)
      {
        ParagraphItem childEntity = paragraph.ChildEntities[index] as ParagraphItem;
        WTextRange paragraphTextRange = childEntity is WField ? (WTextRange) null : childEntity as WTextRange;
        if (childEntity is InlineContentControl && (childEntity as InlineContentControl).MappedItem is WTextRange)
          paragraphTextRange = (childEntity as InlineContentControl).MappedItem as WTextRange;
        if (paragraph.ChildEntities[index] is WPicture && (paragraph.ChildEntities[index] as WPicture).TextWrappingStyle == TextWrappingStyle.Inline)
          tocParagraph.Items.Add((IEntity) paragraph.ChildEntities[index].Clone());
        else if (paragraphTextRange != null && num == 0)
        {
          if (paragraphTextRange.Text != ControlChar.Tab)
            this.AppendTextToTocParagraph(paragraphTextRange, paragraphTextRange.Text, paraStyle, tocParagraph);
          else if (this.IsNeedToAddTabStop(paragraph as WParagraph, index))
          {
            if (!isTabAdded)
            {
              bool isTabStopPosFromStyle = false;
              float position = this.UpdateTabStopPosition(tocParagraph, ref isTabStopPosFromStyle);
              if (!isTabStopPosFromStyle)
                tocParagraph.ParagraphFormat.Tabs.AddTab(position, TabJustification.Left, TabLeader.NoLeader);
              tocParagraph.AppendText(ControlChar.Tab);
              isTabAdded = true;
            }
            else
              this.AppendTextToTocParagraph(paragraphTextRange, ControlChar.Space, paraStyle, tocParagraph);
          }
        }
        else if (childEntity is WField || childEntity is TableOfContent)
        {
          ++num;
          flag = true;
        }
        else if (flag && childEntity is WFieldMark && ((childEntity as WFieldMark).Type == FieldMarkType.FieldSeparator || (childEntity as WFieldMark).Type == FieldMarkType.FieldEnd))
        {
          --num;
          flag = false;
        }
      }
    }
    else
      this.createHyperLinkForLinkStyle(paragraph, tocParagraph, tocLinkStyles);
  }

  private void AppendTextToTocParagraph(
    WTextRange paragraphTextRange,
    string txtValue,
    WParagraphStyle paragraphStyle,
    WParagraph tocParagraph)
  {
    IWTextRange wtextRange = tocParagraph.AppendText(txtValue);
    WCharacterStyle charStyle = paragraphTextRange.CharacterFormat.CharStyle;
    if (paragraphTextRange.CharacterFormat.HasKey(2))
      wtextRange.CharacterFormat.FontName = paragraphTextRange.CharacterFormat.FontName;
    if (this.IsNeedToApplyFormatting((short) 4, paragraphStyle, paragraphTextRange, charStyle))
      wtextRange.CharacterFormat.Bold = paragraphTextRange.CharacterFormat.Bold;
    if (this.IsNeedToApplyFormatting((short) 5, paragraphStyle, paragraphTextRange, charStyle))
      wtextRange.CharacterFormat.Italic = paragraphTextRange.CharacterFormat.Italic;
    if (this.IsNeedToApplyFormatting((short) 2, paragraphStyle, paragraphTextRange, charStyle))
      wtextRange.CharacterFormat.FontName = paragraphTextRange.CharacterFormat.FontName;
    if (this.IsNeedToApplyFormatting((short) 63 /*0x3F*/, paragraphStyle, paragraphTextRange, charStyle))
      wtextRange.CharacterFormat.HighlightColor = paragraphTextRange.CharacterFormat.HighlightColor;
    if (this.IsNeedToApplyFormatting((short) 10, paragraphStyle, paragraphTextRange, charStyle))
      wtextRange.CharacterFormat.SubSuperScript = paragraphTextRange.CharacterFormat.SubSuperScript;
    if (this.IsNeedToApplyFormatting((short) 55, paragraphStyle, paragraphTextRange, charStyle))
      wtextRange.CharacterFormat.SmallCaps = paragraphTextRange.CharacterFormat.SmallCaps;
    if (this.IsNeedToApplyFormatting((short) 54, paragraphStyle, paragraphTextRange, charStyle))
      wtextRange.CharacterFormat.AllCaps = paragraphTextRange.CharacterFormat.AllCaps;
    if (this.IsNeedToApplyFormatting((short) 53, paragraphStyle, paragraphTextRange, charStyle))
      wtextRange.CharacterFormat.Hidden = paragraphTextRange.CharacterFormat.Hidden;
    if (this.IsNeedToApplyFormatting((short) 6, paragraphStyle, paragraphTextRange, charStyle))
      wtextRange.CharacterFormat.Strikeout = paragraphTextRange.CharacterFormat.Strikeout;
    if (this.IsNeedToApplyFormatting((short) 14, paragraphStyle, paragraphTextRange, charStyle))
      wtextRange.CharacterFormat.DoubleStrike = paragraphTextRange.CharacterFormat.DoubleStrike;
    wtextRange.CharacterFormat.CharStyleName = "Hyperlink";
  }

  private bool IsNeedToApplyFormatting(
    short key,
    WParagraphStyle paragraphStyle,
    WTextRange paragraphTextRange,
    WCharacterStyle charStyle)
  {
    return !paragraphStyle.CharacterFormat.HasValue((int) key) && (paragraphTextRange.CharacterFormat.HasValue((int) key) || charStyle != null && charStyle.CharacterFormat.HasValue((int) key));
  }

  private bool IsNeedToAddTabStop(WParagraph paragraph, int currentTabItemIndex)
  {
    return currentTabItemIndex != 0 && currentTabItemIndex != paragraph.ChildEntities.Count - 1 && this.IsNeedToAddTabStop(0, currentTabItemIndex + 1, paragraph) && this.IsNeedToAddTabStop(currentTabItemIndex, paragraph.ChildEntities.Count, paragraph);
  }

  private bool IsNeedToAddTabStop(int startIndex, int endIndex, WParagraph ownerParagraph)
  {
    for (int index = startIndex; index < endIndex; ++index)
    {
      WTextRange childEntity = ownerParagraph.ChildEntities[index] is WField ? (WTextRange) null : ownerParagraph.ChildEntities[index] as WTextRange;
      if (childEntity != null && childEntity.Text != ControlChar.Tab && !string.IsNullOrEmpty(childEntity.Text))
        return true;
    }
    return false;
  }

  private float UpdateTabStopPosition(WParagraph paragraph, ref bool isTabStopPosFromStyle)
  {
    float num1 = 0.0f;
    float tabPosition = this.GetTabPosition((Entity) paragraph);
    float preTextLength = num1 + paragraph.ParagraphFormat.LeftIndent;
    DrawingContext drawingContext = new DrawingContext();
    int num2 = 0;
    foreach (ParagraphItem paragraphItem in (CollectionImpl) paragraph.Items)
    {
      WTextRange txtRange = paragraphItem is WField ? (WTextRange) null : paragraphItem as WTextRange;
      if (txtRange != null && num2 == 0)
      {
        preTextLength += drawingContext.MeasureTextRange(txtRange, txtRange.Text).Width;
        if ((double) preTextLength > (double) tabPosition)
        {
          preTextLength = 0.0f;
          preTextLength += paragraph.ParagraphFormat.LeftIndent;
          preTextLength += drawingContext.MeasureTextRange(txtRange, txtRange.Text).Width;
        }
      }
      else
      {
        switch (paragraphItem)
        {
          case WField _:
          case TableOfContent _:
            ++num2;
            continue;
          case WFieldMark _:
            if ((paragraphItem as WFieldMark).Type == FieldMarkType.FieldSeparator || (paragraphItem as WFieldMark).Type == FieldMarkType.FieldEnd)
            {
              --num2;
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
    int num3 = 11;
    float tabStopPosition1 = preTextLength + (float) num3;
    if ((double) preTextLength >= 77.0)
      return paragraph.ParaStyle != null && paragraph.ParaStyle.ParagraphFormat.Tabs.Count != 0 ? this.GetTabPositionBasedParagraphStyle(paragraph.ParaStyle.ParagraphFormat.Tabs, ref isTabStopPosFromStyle, preTextLength, tabStopPosition1) : tabStopPosition1;
    float tabStopPosition2 = (float) (((int) preTextLength / num3 + 2) * num3);
    return paragraph.ParaStyle != null && paragraph.ParaStyle.ParagraphFormat.Tabs.Count != 0 ? this.GetTabPositionBasedParagraphStyle(paragraph.ParaStyle.ParagraphFormat.Tabs, ref isTabStopPosFromStyle, preTextLength, tabStopPosition2) : tabStopPosition2;
  }

  private float GetTabPositionBasedParagraphStyle(
    TabCollection tabs,
    ref bool isTabStopPosFromStyle,
    float preTextLength,
    float tabStopPosition)
  {
    for (int index = 0; index < tabs.Count; ++index)
    {
      if ((double) tabs[index].Position > (double) preTextLength && index != tabs.Count - 1 || (double) tabs[index].Position == (double) tabStopPosition)
      {
        isTabStopPosFromStyle = true;
        return tabs[index].Position;
      }
    }
    return tabStopPosition;
  }

  private void createHyperLinkForLinkStyle(
    IWParagraph paragraph,
    WParagraph tocParagraph,
    List<string> tocLinkStyles)
  {
    int tocValidItemIndex = this.GetTOCValidItemIndex(paragraph as WParagraph);
    if (tocValidItemIndex == int.MinValue)
      return;
    for (int index = tocValidItemIndex; index < paragraph.ChildEntities.Count; ++index)
    {
      bool flag = false;
      WParagraphStyle paraStyle = (paragraph as WParagraph).ParaStyle as WParagraphStyle;
      switch (paragraph.ChildEntities[index].EntityType)
      {
        case EntityType.TextRange:
        case EntityType.MergeField:
          if (tocLinkStyles.Contains((paragraph.ChildEntities[index] as WTextRange).CharacterFormat.CharStyleName))
          {
            if (paragraph.ChildEntities[index] is WTextRange && !(paragraph.ChildEntities[index] is WField) && (paragraph.ChildEntities[index] as WTextRange).Text != ControlChar.Tab)
            {
              IWTextRange wtextRange = tocParagraph.AppendText((paragraph.ChildEntities[index] as WTextRange).Text);
              if (!paraStyle.CharacterFormat.HasValue(4) && (paragraph.ChildEntities[index] as WTextRange).CharacterFormat.HasValue(4))
                wtextRange.CharacterFormat.Bold = (paragraph.ChildEntities[index] as WTextRange).CharacterFormat.Bold;
              if (!paraStyle.CharacterFormat.HasValue(5) && (paragraph.ChildEntities[index] as WTextRange).CharacterFormat.HasValue(5))
                wtextRange.CharacterFormat.Italic = (paragraph.ChildEntities[index] as WTextRange).CharacterFormat.Italic;
              wtextRange.CharacterFormat.CharStyleName = "Hyperlink";
              break;
            }
            break;
          }
          flag = true;
          break;
        case EntityType.Picture:
          if (tocLinkStyles.Contains((paragraph.ChildEntities[index] as WPicture).CharacterFormat.CharStyleName))
          {
            WPicture wpicture = (WPicture) paragraph.ChildEntities[index].Clone();
            tocParagraph.ChildEntities.Insert(tocParagraph.ChildEntities.Count, (IEntity) wpicture);
            break;
          }
          if (!paragraph.ChildEntities[index].IsFloatingItem(false))
          {
            flag = true;
            break;
          }
          break;
        case EntityType.Field:
          if ((paragraph.ChildEntities[index] as WField).FieldType == FieldType.FieldTOCEntry)
          {
            ++index;
            if (paragraph.ChildEntities[index] is WTextRange && tocLinkStyles.Contains((paragraph.ChildEntities[index] as WTextRange).CharacterFormat.CharStyleName))
            {
              while (!(paragraph.ChildEntities[index] is WFieldMark) || (paragraph.ChildEntities[index] as WFieldMark).Type != FieldMarkType.FieldEnd)
                ++index;
              break;
            }
            if (paragraph.ChildEntities[index] is WTextRange)
            {
              flag = true;
              break;
            }
            if (!tocLinkStyles.Contains((paragraph.ChildEntities[index] as WField).CharacterFormat.CharStyleName))
              flag = true;
            else
              break;
          }
          if (paragraph.ChildEntities[index] is WField)
          {
            do
            {
              ++index;
            }
            while (!(paragraph.ChildEntities[index] is WFieldMark) || (paragraph.ChildEntities[index] as WFieldMark).Type != FieldMarkType.FieldSeparator);
            break;
          }
          break;
        case EntityType.Break:
          if (!tocLinkStyles.Contains((paragraph.ChildEntities[index] as Break).CharacterFormat.CharStyleName))
          {
            flag = true;
            break;
          }
          if ((paragraph.ChildEntities[index] as Break).TextRange.Text != ControlChar.Tab)
          {
            IWTextRange wtextRange = tocParagraph.AppendText(" ");
            if (!paraStyle.CharacterFormat.HasValue(4) && (paragraph.ChildEntities[index] as Break).CharacterFormat.HasValue(4))
              wtextRange.CharacterFormat.Bold = (paragraph.ChildEntities[index] as Break).CharacterFormat.Bold;
            if (!paraStyle.CharacterFormat.HasValue(5) && (paragraph.ChildEntities[index] as Break).CharacterFormat.HasValue(5))
              wtextRange.CharacterFormat.Italic = (paragraph.ChildEntities[index] as Break).CharacterFormat.Italic;
            wtextRange.CharacterFormat.CharStyleName = "Hyperlink";
            break;
          }
          break;
      }
      if (flag)
        break;
    }
  }

  private int GetTOCValidItemIndex(WParagraph paragraph)
  {
    for (int index = 0; index < paragraph.ChildEntities.Count; ++index)
    {
      if ((!(paragraph.ChildEntities[index] is WTextRange) || !((paragraph.ChildEntities[index] as WTextRange).Text.Trim() == "")) && !(paragraph.ChildEntities[index] is WIfField) && !(paragraph.ChildEntities[index] is BookmarkStart) && !(paragraph.ChildEntities[index] is BookmarkEnd) && !paragraph.ChildEntities[index].IsFloatingItem(false) && (!(paragraph.ChildEntities[index] is WField) || (paragraph.ChildEntities[index] as WField).FieldType != FieldType.FieldTOCEntry) && !(paragraph.ChildEntities[index] is Break))
      {
        if (!(paragraph.ChildEntities[index] is WField))
          return index;
        do
        {
          int num = index + 1;
          if (paragraph.ChildEntities.Count - 1 < (index = num + 1))
            return int.MinValue;
        }
        while (!(paragraph.ChildEntities[index] is WFieldMark) || (paragraph.ChildEntities[index] as WFieldMark).Type != FieldMarkType.FieldSeparator);
        int num1;
        return num1 = index + 1;
      }
    }
    return int.MinValue;
  }

  private void AddTabsAndPageRefField(WParagraph paragraph, string bookmark)
  {
    if (this.RightAlignPageNumbers)
    {
      if ((paragraph.ParaStyle as WParagraphStyle).ParagraphFormat.Tabs.Count == 0)
        paragraph.ParagraphFormat.Tabs.AddTab(this.GetTabPosition((Entity) paragraph), TabJustification.Right, TabLeader.Dotted);
      paragraph.Items.Insert(paragraph.Items.Count - 1, (IEntity) new WTextRange((IWordDocument) this.Document)
      {
        Text = ControlChar.Tab
      });
    }
    WField wfield = new WField((IWordDocument) this.Document);
    wfield.FieldType = FieldType.FieldPageRef;
    wfield.m_fieldValue = bookmark + " \\h";
    paragraph.Items.Insert(paragraph.Items.Count - 1, (IEntity) wfield);
    WTextRange wtextRange1 = new WTextRange((IWordDocument) this.Document);
    wtextRange1.Text = $"PAGEREF {bookmark} \\h";
    wtextRange1.ApplyCharacterFormat(wfield.CharacterFormat);
    paragraph.Items.Insert(paragraph.Items.Count - 1, (IEntity) wtextRange1);
    WFieldMark wfieldMark1 = new WFieldMark((IWordDocument) this.Document, FieldMarkType.FieldSeparator);
    paragraph.Items.Insert(paragraph.Items.Count - 1, (IEntity) wfieldMark1);
    wfield.FieldSeparator = wfieldMark1;
    WTextRange wtextRange2 = new WTextRange((IWordDocument) this.Document);
    paragraph.Items.Insert(paragraph.Items.Count - 1, (IEntity) wtextRange2);
    WFieldMark wfieldMark2 = new WFieldMark((IWordDocument) this.Document, FieldMarkType.FieldEnd);
    paragraph.Items.Insert(paragraph.Items.Count - 1, (IEntity) wfieldMark2);
    wfield.FieldEnd = wfieldMark2;
  }

  private float GetTabPosition(Entity entity)
  {
    float tabPosition = 0.0f;
    Entity entity1 = entity;
    while (!(entity1 is WSection) && entity1.Owner != null)
      entity1 = entity1.Owner;
    if (entity1 is WSection)
      tabPosition = (entity1 as WSection).Columns.Count <= 1 ? (entity1 as WSection).PageSetup.ClientWidth - 0.5f : (entity1 as WSection).Columns[0].Width - 0.5f;
    return tabPosition;
  }

  private WParagraph CreateTOCParagraph(int level)
  {
    WTextBody ownerTextBody = this.LastTOCParagraph.OwnerTextBody;
    ownerTextBody.Items.IndexOf((IEntity) this.LastTOCParagraph);
    int index1 = this.LastTOCParagraph.Items.IndexOf((IEntity) this);
    if (index1 > 0)
    {
      this.CreateParagraph(this.LastTOCParagraph, index1);
      this.m_tocParagraph = this.OwnerParagraph;
    }
    int index2 = ownerTextBody.Items.IndexOf((IEntity) this.LastTOCParagraph);
    WParagraph tocParagraph = new WParagraph((IWordDocument) this.Document);
    ownerTextBody.Items.Insert(index2, (IEntity) tocParagraph);
    level += 18;
    tocParagraph.ApplyStyle((BuiltinStyle) level, false);
    bool flag = true;
    if (this.LastTOCParagraph == this.OwnerParagraph)
    {
      for (int index3 = 0; index3 < this.LastTOCParagraph.Items.Count; ++index3)
      {
        if (this.LastTOCParagraph.Items[index3] == this)
        {
          tocParagraph.Items.Insert(tocParagraph.Items.Count, (IEntity) this.LastTOCParagraph.Items[index3]);
          --index3;
        }
        else if (this.LastTOCParagraph.Items[index3] is WFieldMark)
        {
          tocParagraph.Items.Insert(tocParagraph.Items.Count, (IEntity) this.LastTOCParagraph.Items[index3]);
          --index3;
          flag = false;
        }
        else if (this.LastTOCParagraph.Items[index3] is WTextRange)
        {
          if (flag)
            tocParagraph.Items.Insert(tocParagraph.Items.Count, (IEntity) this.LastTOCParagraph.Items[index3]);
          else
            this.LastTOCParagraph.Items.Remove((IEntity) this.LastTOCParagraph.Items[index3]);
          --index3;
          if (!flag)
            break;
        }
      }
    }
    return tocParagraph;
  }

  private string GenerateBookmarkName()
  {
    ++this.m_doc.m_tocBookmarkID;
    return "_Toc" + $"{this.m_doc.m_tocBookmarkID:0000000000}";
  }

  internal void UpdatePageNumbers(Dictionary<Entity, int> tocEntryPageNumbers)
  {
    WParagraph ownerParagraph = this.OwnerParagraph;
    WTextBody ownerTextBody = ownerParagraph.OwnerTextBody;
    int num1 = ownerTextBody.ChildEntities.IndexOf((IEntity) ownerParagraph);
    int num2 = ownerTextBody.ChildEntities.IndexOf((IEntity) this.LastTOCParagraph);
    int index1 = num1;
    for (int index2 = 0; index1 < num2 && index2 < tocEntryPageNumbers.Count; ++index1)
    {
      WParagraph childEntity = ownerTextBody.ChildEntities[index1] as WParagraph;
      Entity entity = (Entity) null;
      if (childEntity != null && this.TOCEntryEntities.ContainsKey((Entity) childEntity))
        entity = this.TOCEntryEntities[(Entity) childEntity];
      if (childEntity != null && entity != null && tocEntryPageNumbers.ContainsKey(entity))
      {
        if (childEntity.Items.Count > 2 && childEntity.Items[childEntity.Items.Count - 3] is WTextRange)
        {
          string numberFormatValue = tocEntryPageNumbers[entity].ToString();
          if (entity.GetOwnerSection(entity) is WSection ownerSection)
            numberFormatValue = ownerSection.PageSetup.GetNumberFormatValue((byte) ownerSection.PageSetup.PageNumberStyle, tocEntryPageNumbers[entity]);
          (childEntity.Items[childEntity.Items.Count - 3] as WTextRange).Text = numberFormatValue;
        }
        ++index2;
      }
    }
  }

  private void UpdateList(IWParagraph paragraph, WParagraph tocParagraph, ref bool isTabAdded)
  {
    WParagraph paragraph1 = paragraph as WParagraph;
    WListFormat listFormat = (WListFormat) null;
    WParagraphStyle paraStyle = paragraph1.ParaStyle as WParagraphStyle;
    if (paragraph1.ListFormat.ListType != ListType.NoList || paragraph1.ListFormat.IsEmptyList)
      listFormat = paragraph1.ListFormat;
    else if (paraStyle.ListFormat.ListType != ListType.NoList || paraStyle.ListFormat.IsEmptyList)
      listFormat = paraStyle.ListFormat;
    if (listFormat == null || listFormat.CurrentListStyle == null)
      return;
    ListStyle currentListStyle = listFormat.CurrentListStyle;
    int levelNumber = 0;
    if (paragraph1.ListFormat.HasKey(0))
      levelNumber = paragraph1.ListFormat.ListLevelNumber;
    else if (paraStyle.ListFormat.HasKey(0))
      levelNumber = paraStyle.ListFormat.ListLevelNumber;
    WListLevel level = currentListStyle.GetNearLevel(levelNumber);
    ListOverrideStyle listOverrideStyle = (ListOverrideStyle) null;
    if (listFormat.LFOStyleName != null && listFormat.LFOStyleName.Length > 0)
      listOverrideStyle = this.Document.ListOverrides.FindByName(listFormat.LFOStyleName);
    if (listOverrideStyle != null && listOverrideStyle.OverrideLevels.HasOverrideLevel(levelNumber) && listOverrideStyle.OverrideLevels[levelNumber].OverrideFormatting)
      level = listOverrideStyle.OverrideLevels[levelNumber].OverrideListLevel;
    string listValue = this.Document.UpdateListValue(paragraph1, listFormat, level);
    if (!(listValue != string.Empty))
      return;
    this.AddListValueAndTab(paragraph, tocParagraph, paraStyle, listValue, ref isTabAdded, level.CharacterFormat);
  }

  private void AddListValueAndTab(
    IWParagraph paragraph,
    WParagraph tocParagraph,
    WParagraphStyle tocStyle,
    string listValue,
    ref bool isTabAdded,
    WCharacterFormat characterFormat)
  {
    IWTextRange wtextRange = tocParagraph.AppendText(listValue);
    WParagraphStyle paraStyle = (paragraph as WParagraph).ParaStyle as WParagraphStyle;
    if (!paraStyle.CharacterFormat.HasValue(4) && paragraph.BreakCharacterFormat.HasValue(4))
      wtextRange.CharacterFormat.Bold = paragraph.BreakCharacterFormat.Bold;
    if (!paraStyle.CharacterFormat.HasValue(5) && paragraph.BreakCharacterFormat.HasValue(5))
      wtextRange.CharacterFormat.Italic = paragraph.BreakCharacterFormat.Italic;
    if (characterFormat.HasKey(2))
      wtextRange.CharacterFormat.FontName = characterFormat.FontName;
    wtextRange.CharacterFormat.CharStyleName = "Hyperlink";
    bool isTabStopPosFromStyle = false;
    float position = this.UpdateTabStopPosition(tocParagraph, ref isTabStopPosFromStyle);
    if (!isTabStopPosFromStyle)
      tocParagraph.ParagraphFormat.Tabs.AddTab(position, TabJustification.Left, TabLeader.NoLeader);
    tocParagraph.AppendText(ControlChar.Tab);
    isTabAdded = true;
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutInfo();
    if (this.TOCField.FieldSeparator == null)
      return;
    this.TOCField.SetSkipForFieldCode(this.NextSibling);
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  protected override void InitXDLSHolder()
  {
    if (this.InvalidFormatString)
      this.UpdateFormattingString();
    this.XDLSHolder.AddElement("toc-field", (object) this.m_tocField);
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("type", (Enum) ParagraphItemType.TOC);
  }

  protected override object CloneImpl()
  {
    if (this.InvalidFormatString)
      this.UpdateFormattingString();
    TableOfContent tableOfContent = (TableOfContent) base.CloneImpl();
    tableOfContent.m_tocField = (WField) this.m_tocField.Clone();
    return (object) tableOfContent;
  }

  internal override void Close()
  {
    base.Close();
    this.m_tocField = (WField) null;
    if (this.m_tocStyles != null)
    {
      foreach (List<WParagraphStyle> wparagraphStyleList in this.m_tocStyles.Values)
        wparagraphStyleList.Clear();
      this.m_tocStyles.Clear();
      this.m_tocStyles = (Dictionary<int, List<WParagraphStyle>>) null;
    }
    if (this.m_tocLevels == null)
      return;
    foreach (List<string> stringList in this.m_tocLevels.Values)
      stringList.Clear();
    this.m_tocLevels.Clear();
    this.m_tocLevels = (Dictionary<int, List<string>>) null;
  }

  SizeF ILayoutInfo.Size
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  SyncFont ILayoutInfo.Font
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  bool ILayoutInfo.IsClipped
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  bool ILayoutInfo.IsVerticalText
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  bool ILayoutInfo.IsSkip
  {
    get => true;
    set => throw new NotImplementedException();
  }

  bool ILayoutInfo.IsSkipBottomAlign
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  bool ILayoutInfo.IsLineContainer => throw new NotImplementedException();

  ChildrenLayoutDirection ILayoutInfo.ChildrenLayoutDirection
  {
    get => throw new NotImplementedException();
  }

  bool ILayoutInfo.IsLineBreak
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  bool ILayoutInfo.TextWrap
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  bool ILayoutInfo.IsPageBreakItem
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  bool ILayoutInfo.IsFirstItemInPage
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  bool ILayoutInfo.IsKeepWithNext
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }

  bool ILayoutInfo.IsHiddenRow
  {
    get => throw new NotImplementedException();
    set => throw new NotImplementedException();
  }
}
