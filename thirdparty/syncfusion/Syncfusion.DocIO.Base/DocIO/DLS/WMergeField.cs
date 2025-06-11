// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WMergeField
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WMergeField : WField, IWMergeField, IWField, IWTextRange, IParagraphItem, IEntity
{
  protected string m_fieldName = "";
  private string m_textBefore = "";
  private string m_textAfter = "";
  private string m_prefix = "";
  private string m_numberFormat = "";
  private string m_dateFormat = "";
  private ParagraphItemCollection m_pItemColl;
  private readonly string SlashSymbol = '\\'.ToString();
  private readonly string InvertedCommas = '"'.ToString();

  public override EntityType EntityType => EntityType.MergeField;

  public string FieldName
  {
    get => this.m_fieldName;
    set
    {
      string fieldName = this.m_fieldName;
      this.m_fieldName = value;
      if (this.Document.IsOpening || this.Document.IsMailMerge || this.FieldEnd == null || !(fieldName != value))
        return;
      this.FieldCode = this.GetFieldCodeForUnknownFieldType().Replace(fieldName, value);
    }
  }

  public string TextBefore
  {
    get => this.m_textBefore;
    set
    {
      string textBefore = this.m_textBefore;
      this.m_textBefore = value;
      if (this.Document.IsOpening || this.Document.IsMailMerge)
        return;
      string unknownFieldType = this.GetFieldCodeForUnknownFieldType();
      if (unknownFieldType.ToLower().Contains("\\b"))
        this.FieldCode = unknownFieldType.Replace(textBefore, value);
      else
        this.FieldCode = $"{unknownFieldType}\\b \"{value}\"";
    }
  }

  public string TextAfter
  {
    get => this.m_textAfter;
    set
    {
      string textAfter = this.m_textAfter;
      this.m_textAfter = value;
      if (this.Document.IsOpening || this.Document.IsMailMerge)
        return;
      string unknownFieldType = this.GetFieldCodeForUnknownFieldType();
      if (unknownFieldType.ToLower().Contains("\\f"))
        this.FieldCode = unknownFieldType.Replace(textAfter, value);
      else
        this.FieldCode = $"{unknownFieldType}\\f \"{value}\"";
    }
  }

  public string Prefix
  {
    get => this.m_prefix;
    internal set => this.m_prefix = value;
  }

  public string NumberFormat => this.m_numberFormat;

  public string DateFormat => this.m_dateFormat;

  [Obsolete("This property has been deprecated. Use the Text property of WField class to get result text of the field.")]
  public ParagraphItemCollection TextItems
  {
    get
    {
      if (this.m_pItemColl == null)
      {
        this.m_pItemColl = new ParagraphItemCollection(this.Document);
        this.m_pItemColl.SetOwner((OwnerHolder) this);
      }
      return this.m_pItemColl;
    }
  }

  public WMergeField(IWordDocument doc)
    : base(doc)
  {
    this.m_paraItemType = ParagraphItemType.MergeField;
    this.m_pItemColl = new ParagraphItemCollection(doc as WordDocument);
    this.m_pItemColl.SetOwner((OwnerHolder) this);
    this.m_fieldType = FieldType.FieldMergeField;
  }

  protected override object CloneImpl()
  {
    WMergeField owner = (WMergeField) base.CloneImpl();
    if (this.m_pItemColl != null)
    {
      owner.m_pItemColl = new ParagraphItemCollection(this.Document);
      owner.m_pItemColl.SetOwner((OwnerHolder) owner);
      this.m_pItemColl.CloneItemsTo(owner.m_pItemColl);
    }
    return (object) owner;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    if (this.m_pItemColl == null)
      return;
    int index = 0;
    for (int count = this.m_pItemColl.Count; index < count; ++index)
      this.m_pItemColl[index].CloneRelationsTo(doc, nextOwner);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    this.m_fieldName = reader.ReadString("FieldName");
    if (reader.HasAttribute("BeforeText"))
      this.m_textBefore = reader.ReadString("BeforeText");
    if (reader.HasAttribute("AfterText"))
      this.m_textAfter = reader.ReadString("AfterText");
    if (reader.HasAttribute("NumberFormat"))
      this.m_numberFormat = reader.ReadString("NumberFormat");
    if (reader.HasAttribute("DateFormat"))
      this.m_dateFormat = reader.ReadString("DateFormat");
    if (!reader.HasAttribute("Prefix"))
      return;
    this.m_prefix = reader.ReadString("Prefix");
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    if (this.FieldName != string.Empty)
      writer.WriteValue("FieldName", this.FieldName);
    if (this.m_textBefore != string.Empty)
      writer.WriteValue("BeforeText", this.TextBefore);
    if (this.m_textAfter != string.Empty)
      writer.WriteValue("AfterText", this.TextAfter);
    if (this.m_numberFormat != string.Empty)
      writer.WriteValue("NumberFormat", this.NumberFormat);
    if (this.m_dateFormat != string.Empty)
      writer.WriteValue("DateFormat", this.DateFormat);
    if (!(this.m_prefix != string.Empty))
      return;
    writer.WriteValue("Prefix", this.m_prefix);
  }

  protected internal override void ParseFieldCode(string fieldCode)
  {
    this.UpdateFieldCode(fieldCode);
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

  protected internal override void UpdateFieldCode(string fieldCode)
  {
    bool flag = true;
    string[] fieldValues = this.GetFieldValues(this.UpdateFieldValue(fieldCode));
    for (int index = 1; index < fieldValues.Length; ++index)
    {
      string str1 = fieldValues[index];
      if (str1.Length > 0)
      {
        string str2 = WMergeField.ClearStringFromOtherCharacters(str1);
        switch (str1[0])
        {
          case 'B':
          case 'b':
            this.m_textBefore = str2;
            flag = false;
            continue;
          case 'F':
          case 'f':
            this.m_textAfter = str2;
            flag = false;
            continue;
          case 'M':
          case 'V':
          case 'm':
          case 'v':
            flag = false;
            continue;
          default:
            if (flag)
            {
              string[] strArray;
              (strArray = fieldValues)[0] = $"{strArray[0]}\\{fieldValues[index]}";
              continue;
            }
            continue;
        }
      }
    }
    this.ParseFieldName(fieldValues[0]);
  }

  internal void ApplyBaseFormat()
  {
    for (int index = 0; index < this.TextItems.Count; ++index)
      this.TextItems[index].ParaItemCharFormat.ApplyBase(this.CharacterFormat.BaseFormat);
  }

  internal override void Close()
  {
    if (this.m_pItemColl != null)
    {
      this.m_pItemColl.Close();
      this.m_pItemColl = (ParagraphItemCollection) null;
    }
    if (this.m_textAfter != null)
      this.m_textAfter = (string) null;
    if (this.m_textBefore != null)
      this.m_textBefore = (string) null;
    if (this.m_fieldName != null)
      this.m_fieldName = (string) null;
    if (this.m_dateFormat != null)
      this.m_dateFormat = (string) null;
    if (this.m_numberFormat != null)
      this.m_numberFormat = (string) null;
    if (this.m_prefix != null)
      this.m_prefix = (string) null;
    base.Close();
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
    this.UpdateMergeFieldResult();
  }

  internal void ParseFieldName(string fieldName)
  {
    string[] strArray = fieldName.Trim().Split(' ');
    strArray[0] = strArray[0].ToUpper();
    string str = "";
    for (int index = 0; index < strArray.Length; ++index)
      str = $"{str}{strArray[index]} ";
    fieldName = str;
    if (str.StartsWithExt("MERGEFIELD") && (str.Contains("\\") || str.Contains(":")))
      this.ParseFieldNameHavingGroupExpression(fieldName);
    else
      this.ParseFieldNameUsingRegex(fieldName);
  }

  private void ParseFieldNameUsingRegex(string fieldName)
  {
    Match match = new Regex("MERGEFIELD\\s+\"?([^:\"]+):?([^\"]*)\"?").Match(fieldName.Trim());
    if (match.Groups[2].Length == 0)
    {
      this.m_prefix = "";
      this.m_fieldName = match.Groups[1].Value;
    }
    else
    {
      this.m_prefix = match.Groups[1].Value;
      this.m_fieldName = match.Groups[2].Value;
    }
  }

  private void ParseFieldNameHavingGroupExpression(string fieldName)
  {
    bool flag = false;
    string str1 = fieldName.Replace("MERGEFIELD ", string.Empty).Trim();
    if (str1.IndexOf("\"") == 0 && str1.LastIndexOf("\"") == str1.Length - 1)
    {
      string str2 = str1.Remove(0, 1);
      str1 = str2.Remove(str2.Length - 1, 1);
    }
    if (str1.Contains("\\\"") || str1.Contains("\\\\"))
      str1 = str1.Replace("\\", "");
    if (str1.Contains(":"))
    {
      this.m_prefix = str1.Substring(0, str1.IndexOf(":"));
      if (this.m_prefix == "BeginGroup" || this.m_prefix == "EndGroup" || this.m_prefix == "TableStart" || this.m_prefix == "TableEnd" || this.m_prefix == "Image")
      {
        flag = true;
        if (!fieldName.Contains("\\"))
        {
          this.ParseFieldNameUsingRegex(fieldName);
          return;
        }
      }
      else
        this.m_prefix = string.Empty;
    }
    if (flag)
      str1 = str1.Substring(str1.IndexOf(":") + 1, str1.Length - (str1.IndexOf(":") + 1));
    this.m_fieldName = str1;
  }

  internal string GetInstructionText()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(" MERGEFIELD ");
    if (this.Prefix != string.Empty)
    {
      stringBuilder.AppendFormat(this.Prefix);
      stringBuilder.Append(":");
    }
    stringBuilder.Append(this.FieldName);
    if (this.TextBefore != string.Empty)
    {
      stringBuilder.Append($" {this.SlashSymbol}b ");
      stringBuilder.Append(this.TextBefore);
    }
    if (this.TextAfter != string.Empty)
    {
      stringBuilder.Append($" {this.SlashSymbol}f ");
      stringBuilder.Append(this.TextAfter);
    }
    if (!string.IsNullOrEmpty(this.DateFormat))
    {
      stringBuilder.Append($" {this.SlashSymbol}@ ");
      stringBuilder.Append(this.InvertedCommas + this.DateFormat + this.InvertedCommas);
    }
    if (!string.IsNullOrEmpty(this.NumberFormat))
    {
      stringBuilder.Append($" {this.SlashSymbol}# ");
      stringBuilder.Append(this.InvertedCommas + this.NumberFormat + this.InvertedCommas);
    }
    if (this.TextFormat != TextFormat.None)
    {
      stringBuilder.Append($" {this.SlashSymbol}* ");
      stringBuilder.Append(this.GetTextFormat(this.TextFormat));
    }
    stringBuilder.Append(this.FormattingString);
    return stringBuilder.ToString();
  }

  private string GetTextFormat(TextFormat format)
  {
    string textFormat = "";
    switch (format)
    {
      case TextFormat.Uppercase:
        textFormat = "Upper";
        break;
      case TextFormat.Lowercase:
        textFormat = "Lower";
        break;
      case TextFormat.FirstCapital:
        textFormat = "FirstCap";
        break;
      case TextFormat.Titlecase:
        textFormat = "Caps";
        break;
    }
    return textFormat;
  }

  private static string ClearStringFromOtherCharacters(string value)
  {
    return value.Remove(0, 1).Trim().Trim('"');
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
    if (fieldvalue.Contains("\\#"))
      fieldvalue = this.UpdateFormatIndexAndFieldValue(fieldvalue, ref formatIndex, "\\#");
    else if (fieldvalue.Contains("\\n"))
      fieldvalue = this.UpdateFormatIndexAndFieldValue(fieldvalue, ref formatIndex, "\\n");
    else if (fieldvalue.Contains("\\N"))
      fieldvalue = this.UpdateFormatIndexAndFieldValue(fieldvalue, ref formatIndex, "\\N");
    if (fieldvalue.Contains("\\@"))
      fieldvalue = this.UpdateFormatIndexAndFieldValue(fieldvalue, ref formatIndex, "\\@");
    else if (fieldvalue.Contains("\\d"))
      fieldvalue = this.UpdateFormatIndexAndFieldValue(fieldvalue, ref formatIndex, "\\d");
    else if (fieldvalue.Contains("\\D"))
      fieldvalue = this.UpdateFormatIndexAndFieldValue(fieldvalue, ref formatIndex, "\\D");
    else if (fieldvalue.Contains("\\"))
      fieldvalue = this.UpdateFormatIndexAndFieldValue(fieldvalue, ref formatIndex, "\\");
    formatIndex.Sort();
    for (int index = 0; index < formatIndex.Count; ++index)
    {
      int length = index == formatIndex.Count - 1 ? fieldCode.Length - formatIndex[index] : formatIndex[index + 1] - formatIndex[index];
      string str = fieldCode.Substring(formatIndex[index], length);
      string mergeFormat = str.Substring(1, str.Length - 1);
      if (mergeFormat.Contains("\\") && mergeFormat[0] != '@')
        mergeFormat = mergeFormat.Substring(0, mergeFormat.IndexOf("\\"));
      this.ParseSwitches(mergeFormat);
    }
    return fieldvalue;
  }

  private void ParseSwitches(string mergeFormat)
  {
    string empty = string.Empty;
    if (mergeFormat.Length <= 0)
      return;
    string str = WMergeField.ClearStringFromOtherCharacters(mergeFormat);
    switch (mergeFormat[0])
    {
      case '#':
      case 'N':
      case 'n':
        this.m_numberFormat = str;
        break;
      case '*':
        switch (str.ToUpper())
        {
          case "UPPER":
            this.m_textFormat = TextFormat.Uppercase;
            return;
          case "LOWER":
            this.m_textFormat = TextFormat.Lowercase;
            return;
          case "CAPS":
            this.m_textFormat = TextFormat.Titlecase;
            return;
          case "FIRSTCAP":
            this.m_textFormat = TextFormat.Titlecase;
            return;
          default:
            WMergeField wmergeField = this;
            wmergeField.m_formattingString = $"{wmergeField.m_formattingString} \\{mergeFormat}";
            return;
        }
      case '@':
      case 'D':
      case 'd':
        this.m_dateFormat = str;
        break;
    }
  }

  private string UpdateFieldValue(string fieldValue, List<int> formatIndex, string mergeSwitch)
  {
    if (mergeSwitch == "\\@")
      fieldValue = fieldValue.Replace('\\', '/');
    int count = fieldValue.Substring(formatIndex[formatIndex.Count - 1] + 1).IndexOf("\\");
    fieldValue = count != -1 ? fieldValue.Remove(formatIndex[formatIndex.Count - 1], count) : fieldValue.Substring(0, formatIndex[formatIndex.Count - 1]);
    return fieldValue;
  }

  private string UpdateFormatIndexAndFieldValue(
    string fieldvalue,
    ref List<int> formatIndex,
    string mergeSwitch)
  {
    int num = fieldvalue.LastIndexOf(mergeSwitch);
    char[] chArray = new char[8]
    {
      'b',
      'B',
      'f',
      'F',
      'm',
      'M',
      'v',
      'V'
    };
    if (mergeSwitch == "\\" && num + 1 < fieldvalue.Length)
    {
      foreach (char ch in chArray)
      {
        if ((int) fieldvalue[fieldvalue.LastIndexOf("\\") + 1] == (int) ch)
          return fieldvalue;
      }
    }
    if (fieldvalue[fieldvalue.LastIndexOf("\\") + 1] == '\\' || fieldvalue[fieldvalue.LastIndexOf("\\") + 1] == '"')
      return fieldvalue;
    formatIndex.Add(fieldvalue.LastIndexOf(mergeSwitch));
    fieldvalue = this.UpdateFieldValue(fieldvalue, formatIndex, mergeSwitch);
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

  internal void UpdateMergeFieldResult()
  {
    this.CheckFieldSeparator();
    this.RemovePreviousResult();
    int inOwnerCollection = this.FieldEnd.GetIndexInOwnerCollection();
    char ch1 = '«';
    char ch2 = '»';
    WTextRange wtextRange1 = new WTextRange((IWordDocument) this.Document);
    string str1 = this.Prefix != string.Empty ? this.Prefix + ":" : string.Empty;
    WTextRange wtextRange2 = wtextRange1;
    string str2;
    if (!(this.m_fieldName != string.Empty))
      str2 = "Error! No bookmark name given.";
    else
      str2 = this.TextBefore + (object) ch1 + str1 + this.m_fieldName + (object) ch2 + this.TextAfter;
    wtextRange2.Text = str2;
    if (this.ResultFormat != null)
      wtextRange1.ApplyCharacterFormat(this.ResultFormat);
    this.FieldEnd.OwnerParagraph.Items.Insert(inOwnerCollection, (IEntity) wtextRange1);
  }
}
