// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WListLevel
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WListLevel : XDLSSerializableBase
{
  internal const string Level1Str = "\0";
  internal const string Level2Str = "\u0001";
  internal const string Level3Str = "\u0002";
  internal const string Level4Str = "\u0003";
  internal const string Level5Str = "\u0004";
  internal const string Level6Str = "\u0005";
  internal const string Level7Str = "\u0006";
  internal const string Level8Str = "\a";
  internal const string Level9Str = "\b";
  internal int restartLevel = -1;
  private readonly string[] DEF_NUMBER_WORDS = new string[19]
  {
    "one",
    "two",
    "three",
    "four",
    "five",
    "six",
    "seven",
    "eight",
    "nine",
    "ten",
    "eleven",
    "twelve",
    "thirteen",
    "fourteen",
    "fifteen",
    "sixteen",
    "seventeen",
    "eighteen",
    "nineteen"
  };
  private readonly string[] DEF_TENS_WORDS = new string[9]
  {
    "ten",
    "twenty",
    "thirty",
    "forty",
    "fifty",
    "sixty",
    "seventy",
    "eighty",
    "ninety"
  };
  private WCharacterFormat m_chFormat;
  private WParagraphFormat m_prFormat;
  private string m_numberPrefix;
  private string m_numberSufix;
  private string m_layoutNumPref = string.Empty;
  private string m_bulletChar;
  private int m_startAt;
  private ListNumberAlignment m_alignment;
  private ListPatternType m_patternType;
  private FollowCharacterType m_followChar;
  private byte[] m_charOffset = new byte[9];
  private int m_legacySpace;
  private int m_legacyIndent;
  private string m_pStyle;
  private WPicture m_picBullet;
  private short m_picButtetId;
  private byte m_bFlags;
  private string m_levelText;

  public ListNumberAlignment NumberAlignment
  {
    get => this.m_alignment;
    set => this.m_alignment = value;
  }

  public int StartAt
  {
    get => this.m_startAt;
    set => this.m_startAt = value;
  }

  public float TabSpaceAfter
  {
    get => this.m_prFormat.Tabs.Count > 0 ? this.m_prFormat.Tabs[0].Position : 0.0f;
    set => this.m_prFormat.Tabs.AddTab(value);
  }

  public float TextPosition
  {
    get => this.m_prFormat.LeftIndent;
    set => this.m_prFormat.SetPropertyValue(2, (object) value);
  }

  public string NumberPrefix
  {
    get => this.m_numberPrefix;
    set => this.m_numberPrefix = value;
  }

  [Obsolete("This property has been deprecated. Use the NumberSuffix property of WListLevel class to set/get the suffix after the number for the specified list level.")]
  public string NumberSufix
  {
    get => this.m_numberSufix;
    set => this.m_numberSufix = value;
  }

  public string NumberSuffix
  {
    get => this.m_numberSufix;
    set => this.m_numberSufix = value;
  }

  public string BulletCharacter
  {
    get => this.m_bulletChar;
    set => this.m_bulletChar = value;
  }

  public ListPatternType PatternType
  {
    get => this.m_patternType;
    set => this.m_patternType = value;
  }

  public bool NoRestartByHigher
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  public WCharacterFormat CharacterFormat => this.m_chFormat;

  public WParagraphFormat ParagraphFormat => this.m_prFormat;

  protected ListStyle OwnerListStyle => this.OwnerBase as ListStyle;

  protected WListLevel PreviousLevel
  {
    get
    {
      ListStyle ownerListStyle = this.OwnerListStyle;
      if (ownerListStyle != null)
      {
        int num = ownerListStyle.Levels.IndexOf(this);
        if (num > 0)
          return ownerListStyle.Levels[num - 1];
      }
      return (WListLevel) null;
    }
  }

  public FollowCharacterType FollowCharacter
  {
    get => this.m_followChar;
    set => this.m_followChar = value;
  }

  public bool IsLegalStyleNumbering
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  public float NumberPosition
  {
    get => this.m_prFormat.FirstLineIndent;
    set => this.m_prFormat.SetPropertyValue(5, (object) value);
  }

  public bool UsePrevLevelPattern
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  internal bool Word6Legacy
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

  internal int LegacySpace
  {
    get => this.m_legacySpace;
    set => this.m_legacySpace = value;
  }

  internal int LegacyIndent
  {
    get => this.m_legacyIndent;
    set => this.m_legacyIndent = value;
  }

  internal string ParaStyleName
  {
    get => this.m_pStyle;
    set => this.m_pStyle = value;
  }

  internal bool NoLevelText
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
  }

  internal int LevelNumber
  {
    get
    {
      if (this.OwnerListStyle != null)
        return this.OwnerListStyle.Levels.IndexOf(this);
      if (!(this.OwnerBase is OverrideLevelFormat))
        return -1;
      OverrideLevelFormat ownerBase = this.OwnerBase as OverrideLevelFormat;
      return (ownerBase.OwnerBase as ListOverrideStyle).OverrideLevels.GetLevelNumber(ownerBase);
    }
  }

  internal WPicture PicBullet
  {
    get => this.m_picBullet;
    set
    {
      this.m_picBullet = value;
      this.m_picBullet.SetOwner((OwnerHolder) this);
    }
  }

  internal short PicBulletId
  {
    get => this.m_picButtetId;
    set => this.m_picButtetId = value;
  }

  internal int PicIndex => this.CharacterFormat.ListPictureIndex;

  internal bool IsEmptyPicture
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 223 | (value ? 1 : 0) << 5);
  }

  internal string LevelText
  {
    get => this.m_levelText;
    set => this.m_levelText = value;
  }

  public WListLevel(ListStyle listStyle)
    : this(listStyle.Document)
  {
    this.SetOwner((OwnerHolder) listStyle);
  }

  internal WListLevel(WordDocument doc)
    : base(doc, (Entity) null)
  {
    this.m_chFormat = this.m_doc.CreateCharacterFormatImpl();
    this.m_chFormat.SetOwner((OwnerHolder) this);
    this.m_prFormat = this.m_doc.CreateParagraphFormatImpl();
    this.m_prFormat.SetOwner((OwnerHolder) this);
  }

  public void CreateLayoutData(string numStr, byte[] characterOffsets, int levelNumber)
  {
    char[] chArray = new char[2]
    {
      '\\',
      Convert.ToChar(levelNumber)
    };
    string[] strArray = numStr.Split(chArray);
    int num = strArray[0].Length + 1;
    for (int index = 0; index < 9; ++index)
    {
      if ((int) characterOffsets[index] == num)
      {
        if (index == 0)
        {
          this.m_numberPrefix = numStr.Substring(0, num - 1);
        }
        else
        {
          int characterOffset = (int) characterOffsets[index - 1];
          int length = num - 1 - (int) characterOffsets[index - 1];
          this.m_numberPrefix = numStr.Substring(characterOffset, length);
        }
        if (index == 8 || characterOffsets[index + 1] == (byte) 0)
        {
          this.m_numberSufix = strArray[1];
          break;
        }
        int length1 = (int) characterOffsets[index + 1] - (num + 1);
        int startIndex = num + 1;
        this.m_numberSufix = numStr.Substring(startIndex, length1);
        break;
      }
    }
  }

  public string GetListItemText(int listItemIndex, ListType listType)
  {
    return this.GetListItemText(listItemIndex, listType, new WParagraph((IWordDocument) this.Document));
  }

  internal string GetListItemText(int listItemIndex, ListType listType, WParagraph paragraph)
  {
    string listItemText = string.Empty;
    if (listType == ListType.Bulleted && this.PatternType != ListPatternType.Bullet)
      listType = ListType.Numbered;
    switch (listType)
    {
      case ListType.Numbered:
        if (this.m_numberPrefix != null && this.m_numberSufix != null)
        {
          listItemText = this.GetNumberedItemText(listItemIndex, paragraph);
          break;
        }
        break;
      case ListType.Bulleted:
        listItemText = this.m_bulletChar;
        break;
      default:
        listItemText = "";
        break;
    }
    return listItemText;
  }

  public WListLevel Clone() => (WListLevel) this.CloneImpl();

  internal static WListLevel CreateDefBulletLvl(float dxLeft, string str, ListStyle listStyle)
  {
    WListLevel listLevelImpl = listStyle.Document.CreateListLevelImpl(listStyle);
    listLevelImpl.m_startAt = 1;
    listLevelImpl.m_patternType = ListPatternType.Bullet;
    string str1 = "Times New Roman";
    switch (str)
    {
      case "\uF0B7":
        str1 = "Symbol";
        break;
      case "o":
        str1 = "Courier New";
        break;
      case "\uF0A7":
        str1 = "Wingdings";
        break;
    }
    listLevelImpl.m_chFormat.FontName = str1;
    listLevelImpl.m_prFormat.SetPropertyValue(2, (object) dxLeft);
    listLevelImpl.m_bulletChar = str;
    return listLevelImpl;
  }

  internal static WListLevel CreateDefNumberLvl(
    int dxLeft,
    int levelNumber,
    ListPatternType patType,
    ListNumberAlignment align,
    ListStyle listStyle)
  {
    WListLevel listLevelImpl = listStyle.Document.CreateListLevelImpl(listStyle);
    listLevelImpl.m_startAt = 1;
    listLevelImpl.m_patternType = patType;
    listLevelImpl.m_alignment = align;
    listLevelImpl.NumberPrefix = string.Empty;
    listLevelImpl.NumberSuffix = ".";
    listLevelImpl.m_prFormat.SetPropertyValue(2, (object) (float) dxLeft);
    return listLevelImpl;
  }

  protected override object CloneImpl()
  {
    WListLevel owner = (WListLevel) base.CloneImpl();
    owner.m_chFormat = new WCharacterFormat((IWordDocument) this.Document);
    owner.m_chFormat.ImportContainer((FormatBase) this.CharacterFormat);
    owner.m_chFormat.CopyProperties((FormatBase) this.CharacterFormat);
    owner.m_chFormat.SetOwner((OwnerHolder) owner);
    owner.m_prFormat = new WParagraphFormat((IWordDocument) this.Document);
    owner.m_prFormat.ImportContainer((FormatBase) this.ParagraphFormat);
    owner.m_prFormat.CopyProperties((FormatBase) this.ParagraphFormat);
    owner.m_prFormat.SetOwner((OwnerHolder) owner);
    if (this.PicBullet != null)
      owner.PicBullet = this.m_picBullet.Clone() as WPicture;
    owner.m_charOffset = new byte[this.m_charOffset.Length];
    this.m_charOffset.CopyTo((Array) owner.m_charOffset, 0);
    return (object) owner;
  }

  private string GetNumberedItemText(int listItemIndex, WParagraph paragraph)
  {
    switch (this.m_patternType)
    {
      case ListPatternType.Arabic:
        return this.m_numberPrefix + (listItemIndex + 1).ToString() + this.m_numberSufix;
      case ListPatternType.UpRoman:
        return this.m_numberPrefix + this.Document.GetAsRoman(listItemIndex + 1).ToUpper() + this.m_numberSufix;
      case ListPatternType.LowRoman:
        return this.m_numberPrefix + this.Document.GetAsRoman(listItemIndex + 1).ToLower() + this.m_numberSufix;
      case ListPatternType.UpLetter:
        return this.m_numberPrefix + this.Document.GetAsLetter(listItemIndex + 1).ToUpper() + this.m_numberSufix;
      case ListPatternType.LowLetter:
        return this.m_numberPrefix + this.Document.GetAsLetter(listItemIndex + 1).ToLower() + this.m_numberSufix;
      case ListPatternType.Ordinal:
        return this.m_numberPrefix + this.Document.GetOrdinal(listItemIndex + 1, this.CharacterFormat) + this.m_numberSufix;
      case ListPatternType.Number:
        if (paragraph.BreakCharacterFormat != null && paragraph.BreakCharacterFormat.HasValue(73) && paragraph.BreakCharacterFormat.LocaleIdASCII == (short) 3082 || paragraph.ParaStyle is WParagraphStyle && (paragraph.ParaStyle as WParagraphStyle).CharacterFormat.HasValue(73) && (paragraph.ParaStyle as WParagraphStyle).CharacterFormat.LocaleIdASCII == (short) 3082 || paragraph.ParaStyle is WParagraphStyle && (paragraph.ParaStyle as WParagraphStyle).BaseStyle != null && (paragraph.ParaStyle as WParagraphStyle).BaseStyle.CharacterFormat.HasValue(73) && (paragraph.ParaStyle as WParagraphStyle).BaseStyle.CharacterFormat.LocaleIdASCII == (short) 3082)
        {
          string cardinalTextString = this.Document.GetSpanishCardinalTextString(true, (listItemIndex + 1).ToString());
          return this.m_numberPrefix + (cardinalTextString[0].ToString().ToUpper() + cardinalTextString.Substring(1)) + this.m_numberSufix;
        }
        string cardTextString = this.Document.GetCardTextString(true, (listItemIndex + 1).ToString());
        return this.m_numberPrefix + (cardTextString[0].ToString().ToUpper() + cardTextString.Substring(1)) + this.m_numberSufix;
      case ListPatternType.OrdinalText:
        if (paragraph.BreakCharacterFormat != null && paragraph.BreakCharacterFormat.HasValue(73) && paragraph.BreakCharacterFormat.LocaleIdASCII == (short) 3082 || paragraph.ParaStyle is WParagraphStyle && (paragraph.ParaStyle as WParagraphStyle).CharacterFormat.HasValue(73) && (paragraph.ParaStyle as WParagraphStyle).CharacterFormat.LocaleIdASCII == (short) 3082 || paragraph.ParaStyle is WParagraphStyle && (paragraph.ParaStyle as WParagraphStyle).BaseStyle != null && (paragraph.ParaStyle as WParagraphStyle).BaseStyle.CharacterFormat.HasValue(73) && (paragraph.ParaStyle as WParagraphStyle).BaseStyle.CharacterFormat.LocaleIdASCII == (short) 3082)
        {
          string ordinalTextString = this.Document.GetSpanishOrdinalTextString(true, (listItemIndex + 1).ToString());
          return this.m_numberPrefix + (ordinalTextString[0].ToString().ToUpper() + ordinalTextString.Substring(1)) + this.m_numberSufix;
        }
        string ordTextString = this.Document.GetOrdTextString(true, (listItemIndex + 1).ToString());
        return this.m_numberPrefix + (ordTextString[0].ToString().ToUpper() + ordTextString.Substring(1)) + this.m_numberSufix;
      case ListPatternType.KanjiDigit:
      case ListPatternType.ChineseCountingThousand:
        return this.m_numberPrefix + this.Document.GetChineseExpression(listItemIndex + 1, this.m_patternType) + this.m_numberSufix;
      case ListPatternType.LeadingZero:
        return listItemIndex < 9 ? $"{this.m_numberPrefix}0{(listItemIndex + 1).ToString()}{this.m_numberSufix}" : this.m_numberPrefix + (listItemIndex + 1).ToString() + this.m_numberSufix;
      case ListPatternType.None:
        return "";
      default:
        return this.m_numberPrefix + (listItemIndex + 1).ToString() + this.m_numberSufix;
    }
  }

  private string GetAsWord(int number, bool isOrdinal)
  {
    if (isOrdinal)
      throw new NotImplementedException("style list not implemented now");
    if (number > 99)
      throw new ArgumentOutOfRangeException("Cannot support number greater than 99");
    string asWord;
    if (number < 20)
    {
      asWord = this.DEF_NUMBER_WORDS[number];
    }
    else
    {
      int index = (int) Math.Floor((double) number / 10.0);
      asWord = $"{this.DEF_TENS_WORDS[index]}-{this.DEF_NUMBER_WORDS[number - index * 10]}";
    }
    return asWord;
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

  internal new void Close()
  {
    this.m_charOffset = (byte[]) null;
    if (this.m_chFormat != null)
    {
      this.m_chFormat.Close();
      this.m_chFormat = (WCharacterFormat) null;
    }
    if (this.m_prFormat == null)
      return;
    this.m_prFormat.Close();
    this.m_chFormat = (WCharacterFormat) null;
  }

  internal bool Compare(WListLevel listLevel)
  {
    if (this.BulletCharacter != listLevel.BulletCharacter || this.FollowCharacter != listLevel.FollowCharacter || this.IsLegalStyleNumbering != listLevel.IsLegalStyleNumbering || this.LegacyIndent != listLevel.LegacyIndent || this.LegacySpace != listLevel.LegacySpace || this.LevelNumber != listLevel.LevelNumber || this.NoLevelText != listLevel.NoLevelText || this.NoRestartByHigher != listLevel.NoRestartByHigher || this.NumberAlignment != listLevel.NumberAlignment || (double) this.NumberPosition != (double) listLevel.NumberPosition || this.NumberPrefix != listLevel.NumberPrefix || this.NumberSuffix != listLevel.NumberSuffix || this.ParaStyleName != null && listLevel.ParaStyleName != null && !this.ParaStyleName.StartsWithExt(this.RemoveGUID(listLevel.ParaStyleName + "_")) && !listLevel.ParaStyleName.StartsWithExt(this.RemoveGUID(this.ParaStyleName + "_")) && !this.ParaStyleName.Equals(listLevel.ParaStyleName) || this.PatternType != listLevel.PatternType || this.PicBullet != listLevel.PicBullet || (int) this.PicBulletId != (int) listLevel.PicBulletId || this.PicIndex != listLevel.PicIndex || this.StartAt != listLevel.StartAt || (double) this.TabSpaceAfter != (double) listLevel.TabSpaceAfter || (double) this.TextPosition != (double) listLevel.TextPosition || this.UsePrevLevelPattern != listLevel.UsePrevLevelPattern || this.Word6Legacy != listLevel.Word6Legacy)
      return false;
    if (this.ParagraphFormat != null && listLevel.ParagraphFormat != null)
    {
      if (!this.ParagraphFormat.Compare(listLevel.ParagraphFormat))
        return false;
    }
    else if (this.ParagraphFormat != null && listLevel.ParagraphFormat == null || this.ParagraphFormat == null && listLevel.ParagraphFormat != null)
      return false;
    if (this.CharacterFormat != null && listLevel.CharacterFormat != null)
    {
      if (!this.CharacterFormat.Compare(listLevel.CharacterFormat))
        return false;
    }
    else if (this.CharacterFormat != null && listLevel.CharacterFormat == null || this.CharacterFormat == null && listLevel.CharacterFormat != null)
      return false;
    return true;
  }

  private string RemoveGUID(string styleName)
  {
    string guid;
    if (Style.HasGuid(styleName, out guid))
      styleName = styleName.Substring(0, styleName.IndexOf(guid));
    return styleName;
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (reader.HasAttribute("Indent"))
      this.TextPosition = reader.ReadFloat("Indent");
    this.NumberPrefix = !reader.HasAttribute("PrefPattern") ? (string) null : reader.ReadString("PrefPattern");
    this.NumberSuffix = !reader.HasAttribute("SufPattern") ? (string) null : reader.ReadString("SufPattern");
    if (reader.HasAttribute("BulletPattern"))
      this.BulletCharacter = reader.ReadString("BulletPattern");
    if (reader.HasAttribute("PatternType"))
      this.PatternType = (ListPatternType) reader.ReadEnum("PatternType", typeof (ListPatternType));
    if (reader.HasAttribute("PrevPattern"))
      this.UsePrevLevelPattern = reader.ReadBoolean("PrevPattern");
    if (reader.HasAttribute("StartAt"))
      this.StartAt = reader.ReadInt("StartAt");
    if (reader.HasAttribute("NumberAlign"))
      this.NumberAlignment = (ListNumberAlignment) reader.ReadEnum("NumberAlign", typeof (ListNumberAlignment));
    if (reader.HasAttribute("FollowCharacter"))
      this.FollowCharacter = (FollowCharacterType) reader.ReadEnum("FollowCharacter", typeof (FollowCharacterType));
    if (reader.HasAttribute("IsLegal"))
      this.IsLegalStyleNumbering = reader.ReadBoolean("IsLegal");
    if (reader.HasAttribute("NoRestart"))
      this.NoRestartByHigher = reader.ReadBoolean("NoRestart");
    if (reader.HasAttribute("Legacy"))
      this.Word6Legacy = reader.ReadBoolean("Legacy");
    if (reader.HasAttribute("LegacyIndent"))
      this.m_legacyIndent = reader.ReadInt("LegacyIndent");
    if (!reader.HasAttribute("LegacySpace"))
      return;
    this.m_legacySpace = reader.ReadInt("LegacySpace");
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("Indent", this.TextPosition);
    writer.WriteValue("PrefPattern", this.NumberPrefix);
    writer.WriteValue("SufPattern", this.NumberSuffix);
    writer.WriteValue("BulletPattern", this.BulletCharacter);
    writer.WriteValue("PatternType", (Enum) this.PatternType);
    writer.WriteValue("PrevPattern", this.UsePrevLevelPattern);
    writer.WriteValue("StartAt", this.StartAt);
    ListStyle ownerListStyle = this.OwnerListStyle;
    if (ownerListStyle != null && ownerListStyle.ListType == ListType.Numbered)
      writer.WriteValue("NumberAlign", (Enum) this.NumberAlignment);
    writer.WriteValue("IsLegal", this.IsLegalStyleNumbering);
    writer.WriteValue("FollowCharacter", (Enum) this.FollowCharacter);
    writer.WriteValue("NoRestart", this.NoRestartByHigher);
    if (!this.Word6Legacy)
      return;
    writer.WriteValue("Legacy", this.Word6Legacy);
    writer.WriteValue("LegacyIndent", this.m_legacyIndent);
    writer.WriteValue("LegacySpace", this.m_legacySpace);
  }

  protected override void InitXDLSHolder()
  {
    base.InitXDLSHolder();
    this.XDLSHolder.AddElement("paragraph-format", (object) this.m_prFormat);
    this.XDLSHolder.AddElement("character-format", (object) this.m_chFormat);
  }
}
