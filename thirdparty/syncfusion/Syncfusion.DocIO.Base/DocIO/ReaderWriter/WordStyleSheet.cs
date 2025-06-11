// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.WordStyleSheet
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter;

[CLSCompliant(false)]
internal class WordStyleSheet
{
  internal const string DEF_FONT_NAME = "Times New Roman";
  private const string DEF_NORMAL_STYLE = "Normal";
  private const string DEF_DPF_STYLE = "Default Paragraph Font";
  private const string DEF_LIST_FONT_NAME = "Wingdings";
  private const int DEF_STDCOUNT = 15;
  internal bool IsFixedIndex13HasStyle;
  internal bool IsFixedIndex14HasStyle;
  internal string FixedIndex13StyleName = string.Empty;
  internal string FixedIndex14StyleName = string.Empty;
  private List<string> m_fontNameList = new List<string>();
  private Dictionary<string, int> m_fontNames = new Dictionary<string, int>();
  private string[] m_defFontNames = new string[6]
  {
    "Times New Roman",
    "Symbol",
    "Arial",
    "Verdana",
    "Wingdings",
    "Courier New"
  };
  private List<WordStyle> m_styleList = new List<WordStyle>();
  private int m_defStyleIndex;
  private Dictionary<string, string> m_fontSubstitutionTable;
  private Dictionary<int, string> m_styleNames;

  internal WordStyleSheet()
  {
    WordStyle style = new WordStyle(this, "Normal");
    style.ID = 0;
    this.m_defStyleIndex = this.AddStyle(style);
    this.UpdateFontNames(this.m_defFontNames);
    style.CHPX.PropertyModifiers.SetUShortValue(19023, (ushort) 0);
    for (int index = 0; index < 14; ++index)
      this.AddEmptyStyle();
  }

  internal WordStyleSheet(bool createDefCharStyle)
  {
    this.m_defStyleIndex = this.AddStyle(new WordStyle(this, "Normal")
    {
      ID = 0
    });
    this.UpdateFontNames(this.m_defFontNames);
    if (createDefCharStyle)
    {
      for (int index = 1; index < 10; ++index)
        this.AddEmptyStyle();
      this.AddStyle(new WordStyle(this, "Default Paragraph Font")
      {
        ID = 65,
        IsCharacterStyle = true,
        BaseStyleIndex = 4095 /*0x0FFF*/,
        HasUpe = true
      });
      for (int index = 11; index < 15; ++index)
        this.AddEmptyStyle();
    }
    else
    {
      for (int index = 0; index < 14; ++index)
        this.AddEmptyStyle();
    }
  }

  internal Dictionary<int, string> StyleNames
  {
    get
    {
      if (this.m_styleNames == null)
        this.m_styleNames = new Dictionary<int, string>();
      return this.m_styleNames;
    }
  }

  internal Dictionary<string, string> FontSubstitutionTable
  {
    get
    {
      if (this.m_fontSubstitutionTable == null)
        this.m_fontSubstitutionTable = new Dictionary<string, string>();
      return this.m_fontSubstitutionTable;
    }
    set => this.m_fontSubstitutionTable = value;
  }

  internal List<string> FontNamesList => this.m_fontNameList;

  internal int DefaultStyleIndex => this.m_defStyleIndex;

  internal int StylesCount => this.m_styleList.Count;

  internal WordStyle CreateStyle(string name) => this.CreateStyle(name, false);

  internal WordStyle CreateStyle(string name, bool characterStyle)
  {
    WordStyle style = new WordStyle(this, name, characterStyle);
    this.AddStyle(style);
    return style;
  }

  internal WordStyle CreateStyle(string name, int index)
  {
    if (index < 15)
      throw new ArgumentOutOfRangeException("index must be greater than 14");
    this.ValidateNameParameter(name, index);
    while (this.StylesCount < index)
      this.AddEmptyStyle();
    WordStyle style = new WordStyle(this, name);
    this.AddStyle(style);
    return style;
  }

  internal int AddStyle(WordStyle style)
  {
    if (style == null)
      throw new ArgumentNullException(nameof (style));
    this.m_styleList.Add(style);
    return this.m_styleList.Count - 1;
  }

  internal int AddEmptyStyle()
  {
    int count = this.m_styleList.Count;
    this.m_styleList.Add(WordStyle.Empty);
    return count;
  }

  internal int StyleNameToIndex(string name, bool isCharacter)
  {
    if (string.IsNullOrEmpty(name))
      throw new ArgumentNullException("Style name can't be null or empty");
    int index = 0;
    for (int count = this.m_styleList.Count; index < count; ++index)
    {
      WordStyle style = this.m_styleList[index];
      if (style.Name == name && style.IsCharacterStyle == isCharacter)
        return index;
    }
    return -1;
  }

  internal int StyleNameToIndex(string name)
  {
    if (string.IsNullOrEmpty(name))
      throw new ArgumentNullException("Style name can't be null or empty");
    int index = 0;
    for (int count = this.m_styleList.Count; index < count; ++index)
    {
      if (this.m_styleList[index].Name == name)
        return index;
    }
    return -1;
  }

  internal int FontNameToIndex(string name)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    return this.m_fontNames.ContainsKey(name) ? this.m_fontNames[name] : -1;
  }

  internal WordStyle GetStyleByIndex(int index)
  {
    if (index < 0 || index > this.m_styleList.Count - 1)
      index = 0;
    return this.m_styleList[index];
  }

  internal WordStyle UpdateStyle(int index, string name)
  {
    if (index < 0 || index > this.StylesCount - 1)
      throw new ArgumentOutOfRangeException($"Index should be between 0 and {this.StylesCount - 1}");
    this.ValidateNameParameter(name, index);
    WordStyle wordStyle = this.GetStyleByIndex(index);
    if (wordStyle == WordStyle.Empty)
      this.m_styleList[index] = wordStyle = new WordStyle(this, name);
    else
      wordStyle.UpdateName(name);
    return wordStyle;
  }

  internal void RemoveStyleByIndex(int index) => this.m_styleList.RemoveAt(index);

  internal void InsertStyle(int index, WordStyle style) => this.m_styleList.Insert(index, style);

  internal void UpdateFontSubstitutionTable(FontFamilyNameRecord ffnRecord)
  {
    if (ffnRecord.AlternativeFontName == null || !(ffnRecord.AlternativeFontName != string.Empty))
      return;
    if (!this.FontSubstitutionTable.ContainsKey(ffnRecord.FontName))
      this.m_fontSubstitutionTable.Add(ffnRecord.FontName, ffnRecord.AlternativeFontName);
    else
      this.FontSubstitutionTable[ffnRecord.FontName] = ffnRecord.AlternativeFontName;
  }

  internal void UpdateFontName(string name)
  {
    this.UpdateFontNames(new string[1]{ name });
  }

  internal void UpdateFontNames(string[] names)
  {
    this.m_fontNameList.AddRange((IEnumerable<string>) names);
    for (int index = 0; index < names.Length; ++index)
    {
      try
      {
        this.m_fontNames.Add(names[index], this.m_fontNames.Count);
      }
      catch
      {
      }
    }
  }

  internal void ClearFontNames()
  {
    this.m_fontNameList.Clear();
    this.m_fontNames.Clear();
  }

  internal void Close()
  {
    if (this.m_fontNameList != null)
    {
      this.m_fontNameList.Clear();
      this.m_fontNameList = (List<string>) null;
    }
    if (this.m_fontNames != null)
    {
      this.m_fontNames.Clear();
      this.m_fontNames = (Dictionary<string, int>) null;
    }
    this.m_defFontNames = (string[]) null;
    if (this.m_styleList != null)
    {
      this.m_styleList.Clear();
      this.m_styleList = (List<WordStyle>) null;
    }
    if (this.m_fontSubstitutionTable != null)
    {
      this.m_fontSubstitutionTable.Clear();
      this.m_fontSubstitutionTable = (Dictionary<string, string>) null;
    }
    if (this.m_styleNames == null)
      return;
    this.m_styleNames.Clear();
    this.m_styleNames = (Dictionary<int, string>) null;
  }

  private void ValidateNameParameter(string name, int withoutIndex)
  {
    for (int index = 0; index < this.m_styleList.Count; ++index)
    {
      WordStyle style = this.m_styleList[index];
    }
  }
}
