// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WSeqField
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.Globalization;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WSeqField : WField
{
  private CaptionNumberingFormat m_numberFormat = ~CaptionNumberingFormat.Number;
  private ParagraphItemCollection m_pItemColl;
  private string m_bookmarkname = string.Empty;
  private bool m_insertnextnumber = true;
  private int m_resetheadinglevel = -1;
  private int m_resetnumber = -1;
  private bool m_repeatnearestnumber;
  private bool m_hideresult;

  public override EntityType EntityType => EntityType.SeqField;

  public new string FormattingString => this.m_formattingString;

  public CaptionNumberingFormat NumberFormat
  {
    get => this.m_numberFormat;
    set
    {
      this.m_numberFormat = value;
      if (this.Document.IsOpening || this.Document.IsMailMerge)
        return;
      string str1 = this.ClearSwitchString(this.GetFieldCodeForUnknownFieldType());
      string str2 = this.ConvertSwitchesToString();
      if (this.m_formattingString != string.Empty)
      {
        int num = str1.LastIndexOf(this.m_formattingString);
        this.FieldCode = str1.Insert(num + this.m_formattingString.Length, str2);
      }
      else if (this.BookmarkName != string.Empty)
      {
        int num = str1.LastIndexOf(this.BookmarkName);
        this.FieldCode = str1.Insert(num + this.BookmarkName.Length, str2);
      }
      else
      {
        int num = str1.LastIndexOf(this.CaptionName);
        this.FieldCode = str1.Insert(num + this.CaptionName.Length, str2);
      }
    }
  }

  public string CaptionName
  {
    get => this.m_fieldValue;
    set
    {
      string fieldValue = this.m_fieldValue;
      this.m_fieldValue = value;
      if (this.Document.IsOpening || this.Document.IsMailMerge || this.FieldEnd == null)
        return;
      this.FieldCode = this.GetFieldCodeForUnknownFieldType().Replace(fieldValue, value);
    }
  }

  public string BookmarkName
  {
    get => this.m_bookmarkname;
    set
    {
      string bookmarkname = this.m_bookmarkname;
      this.m_bookmarkname = value;
      if (this.Document.IsOpening || this.Document.IsMailMerge)
        return;
      string unknownFieldType = this.GetFieldCodeForUnknownFieldType();
      if (unknownFieldType.Contains(this.m_bookmarkname))
      {
        this.FieldCode = unknownFieldType.Replace(bookmarkname, value);
      }
      else
      {
        int num = unknownFieldType.LastIndexOf(this.CaptionName);
        this.FieldCode = unknownFieldType.Insert(num + this.CaptionName.Length + 1, value);
      }
    }
  }

  public bool InsertNextNumber
  {
    get => this.m_insertnextnumber;
    set
    {
      this.m_insertnextnumber = value;
      this.SwitchUpdation(this.m_insertnextnumber, " \\n");
    }
  }

  public bool RepeatNearestNumber
  {
    get => this.m_repeatnearestnumber;
    set
    {
      this.m_repeatnearestnumber = value;
      this.SwitchUpdation(this.m_repeatnearestnumber, " \\c");
    }
  }

  public bool HideResult
  {
    get => this.m_hideresult;
    set
    {
      this.m_hideresult = value;
      this.SwitchUpdation(this.m_hideresult, " \\h");
    }
  }

  public int ResetNumber
  {
    get => this.m_resetnumber;
    set
    {
      int resetnumber = this.m_resetnumber;
      this.m_resetnumber = value;
      this.SwitchUpdationLevel(resetnumber.ToString(), " \\r ", this.m_resetnumber);
    }
  }

  public int ResetHeadingLevel
  {
    get => this.m_resetheadinglevel;
    set
    {
      int resetheadinglevel = this.m_resetheadinglevel;
      this.m_resetheadinglevel = value;
      this.SwitchUpdationLevel(resetheadinglevel.ToString(), " \\s ", this.m_resetheadinglevel);
    }
  }

  public WSeqField(IWordDocument doc)
    : base(doc)
  {
    this.m_paraItemType = ParagraphItemType.SeqField;
    this.m_pItemColl = new ParagraphItemCollection(doc as WordDocument);
    this.m_pItemColl.SetOwner((OwnerHolder) this);
    this.m_fieldType = FieldType.FieldSequence;
  }

  protected internal WSeqField(WField field)
    : base((IWordDocument) field.Document)
  {
  }

  protected internal override void ParseFieldCode(string fieldCode)
  {
    this.UpdateFieldCode(fieldCode);
  }

  protected internal override void UpdateFieldCode(string fieldCode)
  {
    string[] fieldValues = this.GetFieldValues(this.UpdateFieldValue(fieldCode));
    if (fieldValues.Length > 1)
    {
      string str = fieldValues[1];
      if (str.Length > 0)
      {
        string s = WSeqField.ClearStringFromOtherCharacters(str);
        switch (str[0])
        {
          case 'c':
            this.m_repeatnearestnumber = true;
            break;
          case 'h':
            this.m_hideresult = true;
            break;
          case 'n':
            this.m_insertnextnumber = true;
            break;
          case 'r':
            this.m_resetnumber = !(s != string.Empty) ? 0 : int.Parse(s);
            break;
          case 's':
            int result = 0;
            this.m_resetheadinglevel = !int.TryParse(s, NumberStyles.Integer, (IFormatProvider) CultureInfo.CurrentCulture, out result) ? 0 : result;
            break;
        }
      }
    }
    for (int index = 1; index < fieldValues.Length; ++index)
    {
      if (fieldValues[index] == "h " || fieldValues[index] == "h")
      {
        this.m_hideresult = true;
        break;
      }
    }
    string[] strArray = fieldValues[0].Trim().Split(' ');
    this.m_fieldValue = strArray[1];
    try
    {
      if (strArray[2] == null)
        return;
      this.m_bookmarkname = strArray[2];
    }
    catch (IndexOutOfRangeException ex)
    {
    }
  }

  private string UpdateFieldValue(string fieldCode)
  {
    string fieldValue = fieldCode;
    string empty = string.Empty;
    this.m_formattingString = string.Empty;
    List<int> formatIndex = new List<int>();
    string fieldvalue = this.UpdateSwitchesIndexInFieldValue(fieldValue);
    while (fieldvalue.Contains("\\*"))
      fieldvalue = this.UpdateFormatIndexAndFieldValue(fieldvalue, ref formatIndex, "\\*");
    while (fieldvalue.Contains("\\#"))
      fieldvalue = this.UpdateFormatIndexAndFieldValue(fieldvalue, ref formatIndex, "\\#");
    formatIndex.Sort();
    for (int index = 0; index < formatIndex.Count; ++index)
    {
      int length = index == formatIndex.Count - 1 ? fieldCode.Length - formatIndex[index] : formatIndex[index + 1] - formatIndex[index];
      string str = fieldCode.Substring(formatIndex[index], length);
      string seqFormat = str.Substring(1, str.Length - 1);
      if (seqFormat.Contains("\\"))
        seqFormat = seqFormat.Substring(0, seqFormat.IndexOf("\\"));
      this.ParseSwitches(seqFormat);
    }
    return fieldvalue;
  }

  private string UpdateSwitchesIndexInFieldValue(string fieldValue)
  {
    string str;
    for (int index = fieldValue.LastIndexOf('\\'); index > 0; index = str.LastIndexOf('\\'))
    {
      for (; index > 0 && fieldValue[index] == '\\' && fieldValue[index - 1] == '\\'; --index)
        fieldValue = fieldValue.Remove(index, 1);
      str = fieldValue.Substring(0, index);
      if (!str.Contains("\\"))
        break;
    }
    return fieldValue;
  }

  private string UpdateFormatIndexAndFieldValue(
    string fieldvalue,
    ref List<int> formatIndex,
    string seqSwitch)
  {
    fieldvalue.LastIndexOf(seqSwitch);
    if (!formatIndex.Contains(fieldvalue.LastIndexOf(seqSwitch)))
      formatIndex.Add(fieldvalue.LastIndexOf(seqSwitch));
    fieldvalue = this.UpdateFieldValue(fieldvalue, formatIndex, seqSwitch);
    return fieldvalue;
  }

  private string UpdateFieldValue(string fieldValue, List<int> formatIndex, string seqSwitch)
  {
    int num = fieldValue.Substring(formatIndex[formatIndex.Count - 1] + 1).IndexOf("\\");
    fieldValue = num != -1 ? fieldValue.Remove(formatIndex[formatIndex.Count - 1], num + 1) : fieldValue.Substring(0, formatIndex[formatIndex.Count - 1]);
    return fieldValue;
  }

  private void ParseSwitches(string seqFormat)
  {
    string empty = string.Empty;
    if (seqFormat.Length <= 0)
      return;
    string str = WSeqField.ClearStringFromOtherCharacters(seqFormat);
    switch (seqFormat[0])
    {
      case '#':
        this.m_formattingString = str;
        break;
      case '*':
        switch (str)
        {
          case "Arabic":
            this.m_numberFormat = CaptionNumberingFormat.Number;
            return;
          case "roman":
            this.m_numberFormat = CaptionNumberingFormat.LowerRoman;
            return;
          case "ROMAN":
            this.m_numberFormat = CaptionNumberingFormat.Roman;
            return;
          case "Lower":
            this.m_numberFormat = CaptionNumberingFormat.Lowercase;
            return;
          case "Upper":
            this.m_numberFormat = CaptionNumberingFormat.Uppercase;
            return;
          case "ALPHABETIC":
            this.m_numberFormat = CaptionNumberingFormat.Alphabetic;
            return;
          case "alphabetic":
            this.m_numberFormat = CaptionNumberingFormat.LowerAlphabetic;
            return;
          case "Ordinal":
            this.m_numberFormat = CaptionNumberingFormat.Ordinal;
            return;
          case "CardText":
            this.m_numberFormat = CaptionNumberingFormat.CardinalText;
            return;
          case "OrdText":
            this.m_numberFormat = CaptionNumberingFormat.OrdinalText;
            return;
          case "Hex":
            this.m_numberFormat = CaptionNumberingFormat.Hexa;
            return;
          case "DollarText":
            this.m_numberFormat = CaptionNumberingFormat.DollarText;
            return;
          case "FirstCap":
            this.m_numberFormat = CaptionNumberingFormat.FirstCapital;
            return;
          case "Caps":
            this.m_numberFormat = CaptionNumberingFormat.TitleCase;
            return;
          case null:
            return;
          default:
            return;
        }
    }
  }

  protected internal string[] GetFieldValues(string fieldvalue)
  {
    List<int> collection = new List<int>();
    List<KeyValuePair<int, int>> keyValuePairList = new List<KeyValuePair<int, int>>();
    int key = 0;
    bool flag = true;
    for (int index = 0; index < fieldvalue.Length; ++index)
    {
      if (fieldvalue[index] == '\\')
        collection.Add(index);
      else if (index < fieldvalue.Length - 1 && fieldvalue[index] == '"')
      {
        if (flag)
        {
          key = index;
          flag = false;
        }
        else
        {
          int num = index;
          flag = true;
          keyValuePairList.Add(new KeyValuePair<int, int>(key, num));
        }
      }
    }
    List<int> intList = new List<int>((IEnumerable<int>) collection);
    foreach (int num in collection)
    {
      foreach (KeyValuePair<int, int> keyValuePair in keyValuePairList)
      {
        if (keyValuePair.Key < num && keyValuePair.Value > num)
        {
          intList.Remove(num);
          break;
        }
      }
    }
    string[] fieldValues = new string[intList.Count + 1];
    if (intList.Count > 0)
    {
      int startIndex = 0;
      int index;
      for (index = 0; index < intList.Count; ++index)
      {
        fieldValues[index] = fieldvalue.Substring(startIndex, intList[index] - startIndex);
        startIndex = intList[index] + 1;
      }
      fieldValues[index] = fieldvalue.Substring(startIndex);
    }
    else
      fieldValues[0] = fieldvalue;
    return fieldValues;
  }

  private static string ClearStringFromOtherCharacters(string value)
  {
    return value.Remove(0, 1).Trim().Trim('"');
  }

  private void SwitchUpdation(bool switchValue, string switchType)
  {
    if (this.Document.IsOpening || this.Document.IsMailMerge)
      return;
    string str1 = this.ClearFieldSwitch(this.GetFieldCodeForUnknownFieldType());
    if (switchValue)
    {
      string str2 = this.ConvertSwitchesToString();
      if (str2 != string.Empty)
      {
        if (!str1.Contains(str2))
          return;
        int num = str1.LastIndexOf(str2);
        this.FieldCode = str1.Insert(num + str2.Length, switchType);
      }
      else if (this.m_formattingString != string.Empty)
      {
        int num = str1.LastIndexOf(this.m_formattingString);
        this.FieldCode = str1.Insert(num + this.m_formattingString.Length, switchType);
      }
      else if (this.BookmarkName != string.Empty)
      {
        int num = str1.LastIndexOf(this.BookmarkName);
        this.FieldCode = str1.Insert(num + this.BookmarkName.Length, switchType);
      }
      else
      {
        int num = str1.LastIndexOf(this.CaptionName);
        this.FieldCode = str1.Insert(num + this.CaptionName.Length, switchType);
      }
    }
    else
      this.FieldCode = str1;
  }

  private void SwitchUpdationLevel(string oldValue, string switchType, int switchValue)
  {
    if (this.Document.IsOpening || this.Document.IsMailMerge)
      return;
    string str1 = this.ClearFieldSwitchLevel(this.ClearFieldSwitch(this.GetFieldCodeForUnknownFieldType()), oldValue.ToString());
    string str2 = this.ConvertSwitchesToString();
    if (str2 != string.Empty)
    {
      if (!str1.Contains(str2))
        return;
      int num = str1.LastIndexOf(str2);
      this.FieldCode = str1.Insert(num + str2.Length, switchType + (object) switchValue);
    }
    else if (this.m_formattingString != string.Empty)
    {
      int num = str1.LastIndexOf(this.m_formattingString);
      this.FieldCode = str1.Insert(num + this.m_formattingString.Length, switchType + (object) switchValue);
    }
    else if (this.BookmarkName != string.Empty)
    {
      int num = str1.LastIndexOf(this.BookmarkName);
      this.FieldCode = str1.Insert(num + this.BookmarkName.Length, switchType + (object) switchValue);
    }
    else
    {
      int num = str1.LastIndexOf(this.CaptionName);
      this.FieldCode = str1.Insert(num + this.CaptionName.Length, switchType + (object) switchValue);
    }
  }

  private string ClearFieldSwitch(string fieldCode)
  {
    fieldCode = fieldCode.Replace("\\n", string.Empty);
    fieldCode = fieldCode.Replace("\\c", string.Empty);
    fieldCode = fieldCode.Replace("\\h", string.Empty);
    fieldCode = fieldCode.Replace("\\r", string.Empty);
    fieldCode = fieldCode.Replace("\\s", string.Empty);
    return fieldCode;
  }

  private string ClearFieldSwitchLevel(string fieldCode, string oldLevel)
  {
    fieldCode = fieldCode.Replace(oldLevel, string.Empty);
    fieldCode = fieldCode.Replace(oldLevel, string.Empty);
    return fieldCode;
  }

  private string ClearSwitchString(string fieldCode)
  {
    fieldCode = fieldCode.Replace("\\* Arabic", string.Empty);
    fieldCode = fieldCode.Replace("\\* ALPHABETIC", string.Empty);
    fieldCode = fieldCode.Replace("\\*ROMAN", string.Empty);
    fieldCode = fieldCode.Replace("\\* roman", string.Empty);
    fieldCode = fieldCode.Replace("\\* Lower", string.Empty);
    fieldCode = fieldCode.Replace("\\* Upper", string.Empty);
    fieldCode = fieldCode.Replace("\\* Ordinal", string.Empty);
    fieldCode = fieldCode.Replace("\\* CardText", string.Empty);
    fieldCode = fieldCode.Replace("\\* OrdText", string.Empty);
    fieldCode = fieldCode.Replace("\\* Hexa", string.Empty);
    fieldCode = fieldCode.Replace("\\* DollarText", string.Empty);
    fieldCode = fieldCode.Replace("\\* FirstCap", string.Empty);
    fieldCode = fieldCode.Replace("\\* Caps", string.Empty);
    return fieldCode;
  }

  protected internal override string ConvertSwitchesToString()
  {
    string empty = string.Empty;
    switch (this.m_numberFormat)
    {
      case CaptionNumberingFormat.Number:
        empty += " \\* Arabic";
        break;
      case CaptionNumberingFormat.Roman:
        empty += " \\* ROMAN";
        break;
      case CaptionNumberingFormat.Alphabetic:
        empty += " \\* ALPHABETIC";
        break;
      case CaptionNumberingFormat.LowerRoman:
        empty += "\\* roman";
        break;
      case CaptionNumberingFormat.Lowercase:
        empty += "\\* Lower";
        break;
      case CaptionNumberingFormat.Uppercase:
        empty += "\\* Upper";
        break;
      case CaptionNumberingFormat.LowerAlphabetic:
        empty += "\\* alphabetic";
        break;
      case CaptionNumberingFormat.Ordinal:
        empty += "\\* Ordinal";
        break;
      case CaptionNumberingFormat.CardinalText:
        empty += "\\* CardText";
        break;
      case CaptionNumberingFormat.OrdinalText:
        empty += "\\* OrdText";
        break;
      case CaptionNumberingFormat.Hexa:
        empty += "\\* Hex";
        break;
      case CaptionNumberingFormat.DollarText:
        empty += "\\* DollarText";
        break;
      case CaptionNumberingFormat.FirstCapital:
        empty += "\\* FirstCap";
        break;
      case CaptionNumberingFormat.TitleCase:
        empty += "\\* Caps";
        break;
    }
    return empty;
  }

  internal void UpdateFieldMarks()
  {
    int inOwnerCollection1 = this.GetIndexInOwnerCollection();
    bool flag = false;
    if (this.FieldEnd != null && this.FieldEnd.ParentField != this)
    {
      if (this.FieldSeparator != null)
      {
        if (this.FieldSeparator.ParentField != this)
        {
          WFieldMark wfieldMark = new WFieldMark((IWordDocument) this.Document, FieldMarkType.FieldSeparator);
          this.OwnerParagraph.ChildEntities.Insert(inOwnerCollection1 + 1, (IEntity) wfieldMark);
          this.FieldSeparator = wfieldMark;
        }
        if (this.FieldSeparator.Owner is WParagraph)
        {
          int inOwnerCollection2 = this.FieldSeparator.GetIndexInOwnerCollection();
          WFieldMark wfieldMark = new WFieldMark((IWordDocument) this.Document, FieldMarkType.FieldEnd);
          this.OwnerParagraph.ChildEntities.Insert(inOwnerCollection2 + 1, (IEntity) wfieldMark);
          this.FieldEnd = wfieldMark;
          flag = true;
        }
      }
      else
      {
        WFieldMark wfieldMark = new WFieldMark((IWordDocument) this.Document, FieldMarkType.FieldEnd);
        this.OwnerParagraph.ChildEntities.Insert(inOwnerCollection1 + 1, (IEntity) wfieldMark);
        this.FieldEnd = wfieldMark;
      }
    }
    if (!flag)
      return;
    this.UpdateSequenceFieldResult();
  }

  internal void UpdateSequenceFieldResult()
  {
    this.CheckFieldSeparator();
    this.RemovePreviousResult();
    int inOwnerCollection = this.FieldEnd.GetIndexInOwnerCollection();
    WTextRange wtextRange = new WTextRange((IWordDocument) this.Document);
    wtextRange.Text = this.CaptionName != string.Empty ? this.CaptionName : "Error! No bookmark name given.";
    if (this.ResultFormat != null)
      wtextRange.ApplyCharacterFormat(this.ResultFormat);
    this.FieldEnd.OwnerParagraph.Items.Insert(inOwnerCollection, (IEntity) wtextRange);
  }
}
