// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WField
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.Rendering;
using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.Rendering;
using Syncfusion.Layouting;
using Syncfusion.Office;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WField : WTextRange, IWField, IWTextRange, IParagraphItem, IEntity, ILeafWidget, IWidget
{
  private const char COMMASEPARATOR = ',';
  private const char OPENPARENTHESIS = '(';
  private const char CLOSEPARENTHESIS = ')';
  private const string PARAGRAPHMARK = "\r";
  private const string CELLMARK = "\a";
  private const string ROWMARK = "\r\a";
  private const char TableStartMark = '\u0013';
  private const char TableEndMark = '\u0015';
  private const float DefaultIntegralSymbolSize = 17.7260742f;
  private string m_fieldPattern = "{0}";
  protected FieldType m_fieldType;
  protected bool m_bConvertedToText;
  private bool m_bIsLocal;
  protected ParagraphItemType m_paraItemType;
  protected internal string m_formattingString = string.Empty;
  protected internal string m_fieldValue = string.Empty;
  protected TextFormat m_textFormat;
  private string m_localRef;
  private short m_sourceFldType;
  private short m_bFlags;
  private Range m_range;
  private string m_fieldResult = string.Empty;
  internal Stack<WField> m_nestedFields = new Stack<WField>();
  private WFieldMark m_fieldSeparator;
  private WFieldMark m_fieldEnd;
  internal string m_detachedFieldCode = string.Empty;
  internal string m_currentPageNumber;
  private WCharacterFormat m_resultFormat;
  private WField originalField;
  private Stack<Entity> entities;
  private List<WField> nestedFields;
  private Stack<Dictionary<string, object>> m_unlinkNestedFieldStack = new Stack<Dictionary<string, object>>();
  private string m_screenTip;
  private List<string> functions;

  public TextFormat TextFormat
  {
    get => this.m_textFormat;
    set
    {
      TextFormat textFormat = this.m_textFormat;
      this.m_textFormat = value;
      if (this.Document.IsOpening || textFormat == this.m_textFormat)
        return;
      this.FieldCode = this.UpdateTextFormatSwitchString(this.m_textFormat);
    }
  }

  public override EntityType EntityType => EntityType.Field;

  public string FieldPattern
  {
    get => this.m_fieldPattern;
    set => this.m_fieldPattern = value;
  }

  public string FieldValue => this.m_fieldValue;

  public FieldType FieldType
  {
    get => this.m_fieldType;
    set
    {
      FieldType fieldType = this.m_fieldType;
      this.m_fieldType = value;
      if (fieldType == FieldType.FieldUnknown && value == FieldType.FieldAutoNum && !this.Document.Fields.SortedAutoNumFields.Contains(this))
        this.Document.Fields.InsertAutoNumFieldInAsc(this);
      if (this.Document.IsOpening || fieldType == value)
        return;
      if (this.FieldEnd != null)
        this.RemovePreviousFieldCode();
      if (this.OwnerParagraph == null || !(this.NextSibling is WTextRange))
        return;
      WTextRange nextSibling = this.NextSibling as WTextRange;
      nextSibling.Text = FieldTypeDefiner.GetFieldCode(this.m_fieldType);
      if (this is WMergeField && !string.IsNullOrEmpty((this as WMergeField).FieldName))
      {
        WTextRange wtextRange = nextSibling;
        wtextRange.Text = $"{wtextRange.Text} {(this as WMergeField).FieldName}";
      }
      this.UpdateFieldCode(nextSibling.Text);
    }
  }

  internal bool IsDirty
  {
    get => ((int) this.m_bFlags & 2048 /*0x0800*/) >> 11 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 63487 | (value ? 1 : 0) << 11);
  }

  internal bool IsLocked
  {
    get => ((int) this.m_bFlags & 4096 /*0x1000*/) >> 12 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 61439 /*0xEFFF*/ | (value ? 1 : 0) << 12);
  }

  internal bool IsLocal
  {
    get => this.m_bIsLocal;
    set
    {
      this.m_bIsLocal = value;
      this.SetLocalSwitchString();
    }
  }

  internal string FormattingString
  {
    get => this.m_formattingString;
    set => this.m_formattingString = value;
  }

  internal string LocalReference
  {
    get => this.m_localRef;
    set => this.m_localRef = value;
  }

  internal string ScreenTip
  {
    get => this.m_screenTip;
    set => this.m_screenTip = value;
  }

  public string FieldCode
  {
    get
    {
      if (this.IsFormField())
        return FieldTypeDefiner.GetFieldCode(this.FieldType);
      return !this.Document.IsInternalManipulation() && this.Owner == null ? this.m_detachedFieldCode : this.UpdateNestedFieldCode(true, (WMergeField) null);
    }
    set
    {
      if (this.IsFormField())
        return;
      if (!this.Document.IsInternalManipulation() && this.Owner == null)
      {
        this.m_detachedFieldCode = value;
      }
      else
      {
        string fieldCode1 = this.FieldCode;
        string fieldCode2 = value;
        string upper = fieldCode2.ToUpper();
        if (upper.Contains("\\* FUSIONFORMAT"))
        {
          int startIndex = upper.IndexOf("\\* FUSIONFORMAT");
          fieldCode2 = fieldCode2.Remove(startIndex, "\\* FUSIONFORMAT".Length).Insert(startIndex, "\\* MERGEFORMAT");
        }
        if (upper.Contains("NBPAGES") && this.m_fieldType == FieldType.FieldNumPages)
        {
          int startIndex = upper.IndexOf("NBPAGES");
          fieldCode2 = fieldCode2.Remove(startIndex, "NBPAGES".Length).Insert(startIndex, "NUMPAGES");
        }
        if (this.Document.IsOpening || !(fieldCode1 != fieldCode2) || this.OwnerParagraph == null)
          return;
        if (this.FieldEnd != null)
          this.RemovePreviousFieldCode();
        this.m_fieldType = FieldTypeDefiner.GetFieldType(fieldCode2);
        WTextRange wtextRange = new WTextRange((IWordDocument) this.m_doc);
        wtextRange.ApplyCharacterFormat(this.CharacterFormat);
        wtextRange.Text = fieldCode2;
        if (wtextRange.CharacterFormat.PropertiesHash.ContainsKey(106))
          wtextRange.CharacterFormat.PropertiesHash.Remove(106);
        this.OwnerParagraph.Items.Insert(this.Index + 1, (IEntity) wtextRange);
        this.UpdateFieldCode(fieldCode2);
        if (!this.Document.IsMailMerge && this.FieldType == FieldType.FieldMergeField && this.FieldEnd != null && this.Document.Settings.UpdateResultOnFieldCodeChange)
          (this as WMergeField).UpdateMergeFieldResult();
        if (this.Document.IsMailMerge || this.FieldType != FieldType.FieldSequence || this.FieldEnd == null || !this.Document.Settings.UpdateResultOnFieldCodeChange)
          return;
        (this as WSeqField).UpdateSequenceFieldResult();
      }
    }
  }

  internal string InternalFieldCode
  {
    get => this.NextSibling is WTextRange ? (this.NextSibling as WTextRange).Text : string.Empty;
  }

  internal short SourceFieldType
  {
    get => this.m_sourceFldType;
    set => this.m_sourceFldType = value;
  }

  internal string FieldResult
  {
    get => this.m_fieldResult;
    set => this.m_fieldResult = value;
  }

  internal Range Range
  {
    get
    {
      if (this.m_range == null)
        this.m_range = new Range(this.Document, (OwnerHolder) this);
      else if (!this.IsFieldRangeUpdated && !this.Document.IsOpening && !this.Document.IsCloning)
        this.m_range.Items.Clear();
      if (!this.IsFieldRangeUpdated && !this.Document.IsOpening && !this.Document.IsCloning)
        this.UpdateFieldRange();
      return this.m_range;
    }
  }

  internal WFieldMark FieldSeparator
  {
    get => this.m_fieldSeparator;
    set
    {
      this.m_fieldSeparator = value;
      if (value == null)
        return;
      this.m_fieldSeparator.ParentField = this;
    }
  }

  internal WFieldMark FieldEnd
  {
    get => this.m_fieldEnd;
    set
    {
      this.m_fieldEnd = value;
      if (value == null)
        return;
      this.m_fieldEnd.ParentField = this;
    }
  }

  internal new bool IsCloned
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65534 | (value ? 1 : 0));
  }

  internal bool IsAdded
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65533 | (value ? 1 : 0) << 1);
  }

  internal bool IsFieldRangeUpdated
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65531 | (value ? 1 : 0) << 2);
  }

  internal bool IsFieldSeparator
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65527 | (value ? 1 : 0) << 3);
  }

  internal bool IsSkip
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65519 | (value ? 1 : 0) << 4);
  }

  internal bool IsUpdated
  {
    get => ((int) this.m_bFlags & 32 /*0x20*/) >> 5 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65503 | (value ? 1 : 0) << 5);
  }

  internal bool IsInFieldResult
  {
    get => ((int) this.m_bFlags & 64 /*0x40*/) >> 6 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65471 | (value ? 1 : 0) << 6);
  }

  internal bool IsPgNum
  {
    get => ((int) this.m_bFlags & 1024 /*0x0400*/) >> 10 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 64511 | (value ? 1 : 0) << 10);
  }

  internal bool IsNumPagesInsideExpressionField
  {
    get => ((int) this.m_bFlags & 128 /*0x80*/) >> 7 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65407 | (value ? 1 : 0) << 7);
  }

  internal bool IsNumPageUsedForEvaluation
  {
    get => ((int) this.m_bFlags & 256 /*0x0100*/) >> 8 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65279 | (value ? 1 : 0) << 8);
  }

  internal bool IsFieldInsideUnknownField
  {
    get => ((int) this.m_bFlags & 512 /*0x0200*/) >> 9 != 0;
    set => this.m_bFlags = (short) ((int) this.m_bFlags & 65023 | (value ? 1 : 0) << 9);
  }

  internal WCharacterFormat ResultFormat => this.m_resultFormat;

  public override string Text
  {
    get
    {
      return this.FieldType == FieldType.FieldAutoNum ? this.GetAutoNumFieldValue(this) : this.GetFieldResultValue();
    }
    set
    {
      if (this.Document.IsOpening || this.Document.IsCloning || this.Document.IsMailMerge || this.IsCloned || this.FieldType == FieldType.FieldAutoNum)
        return;
      if (this.IsFieldWithoutSeparator)
        throw new Exception("Field Result is not available for this field ");
      this.UpdateFieldResult(value);
    }
  }

  internal bool IsFieldWithoutSeparator => this.CheckFieldWithoutSeparator();

  internal WField OriginalField
  {
    get => this.originalField;
    set => this.originalField = value;
  }

  internal List<string> Functions
  {
    get
    {
      if (this.functions == null)
        this.functions = new List<string>((IEnumerable<string>) new string[18]
        {
          "product",
          "sum",
          "average",
          "mod",
          "abs",
          "int",
          "round",
          "sign",
          "count",
          "defined",
          "or",
          "and",
          "not",
          "max",
          "min",
          "true",
          "false",
          "if"
        });
      return this.functions;
    }
  }

  public WField(IWordDocument doc)
    : base(doc)
  {
    this.m_paraItemType = ParagraphItemType.Field;
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    this.m_fieldType = (FieldType) reader.ReadEnum("FieldType", typeof (FieldType));
    this.m_bConvertedToText = reader.ReadBoolean("ConvertedToText");
    if (reader.HasAttribute("TextFormat"))
      this.m_textFormat = (TextFormat) reader.ReadEnum("TextFormat", typeof (TextFormat));
    if (reader.HasAttribute("IsLocal"))
      this.m_bIsLocal = reader.ReadBoolean("IsLocal");
    if (reader.HasAttribute("FieldFormatting"))
      this.m_formattingString = reader.ReadString("FieldFormatting");
    if (!reader.HasAttribute("FieldValue"))
      return;
    this.m_fieldValue = reader.ReadString("FieldValue");
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("type", (Enum) this.m_paraItemType);
    writer.WriteValue("FieldType", (Enum) this.FieldType);
    writer.WriteValue("TextFormat", (Enum) this.m_textFormat);
    if (this.m_bIsLocal)
      writer.WriteValue("IsLocal", this.m_bIsLocal);
    if (this.m_formattingString != string.Empty)
      writer.WriteValue("FieldFormatting", this.m_formattingString);
    if (this.m_fieldValue == null || !(this.m_fieldValue != ""))
      return;
    writer.WriteValue("FieldValue", this.m_fieldValue);
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new FieldLayoutInfo(ChildrenLayoutDirection.Horizontal);
    if (this.FieldType == FieldType.FieldNumPages || this.FieldType == FieldType.FieldPage || this.FieldType == FieldType.FieldSectionPages || this.FieldType == FieldType.FieldAutoNum)
      this.m_layoutInfo.Font = new SyncFont(this.GetCharacterFormatValue().GetFontToRender(this.GetFontScriptType()));
    else
      this.m_layoutInfo.Font = new SyncFont(this.GetCharFormat().GetFontToRender(this.ScriptType));
    if (this.CharacterFormat.Hidden)
      this.m_layoutInfo.IsSkip = true;
    if ((DocumentLayouter.IsUpdatingTOC ? (this.FieldType != FieldType.FieldTOCEntry ? 1 : 0) : 1) != 0 && (this.FieldSeparator == null || this.FieldSeparator.Owner == null))
    {
      if (this.FieldType != FieldType.FieldFormCheckBox && this.FieldType != FieldType.FieldFormDropDown && this.FieldType != FieldType.FieldSymbol && this.FieldType != FieldType.FieldExpression && this.FieldType != FieldType.FieldAutoNum && (this.FieldType != FieldType.FieldPage && this.FieldType != FieldType.FieldNumPages && this.FieldType != FieldType.FieldSectionPages || !DocumentLayouter.IsLayoutingHeaderFooter))
        this.m_layoutInfo.IsSkip = true;
      if (this.FieldType == FieldType.FieldMacroButton)
        this.SkipMacroButtonFieldCode();
      else
        this.SkipLayoutingOfFieldCode();
    }
    else
    {
      if (this.FieldSeparator != null)
      {
        if (this.FieldType != FieldType.FieldPage && this.FieldType != FieldType.FieldNumPages && this.FieldType != FieldType.FieldSectionPages && this.FieldType != FieldType.FieldTOCEntry && this.FieldType != FieldType.FieldPageRef && this.FieldType != FieldType.FieldFormCheckBox && this.FieldType != FieldType.FieldExpression && this.FieldType != FieldType.FieldFormDropDown && this.FieldType != FieldType.FieldHyperlink && this.FieldType != FieldType.FieldRef && this.FieldType != FieldType.FieldSymbol)
          this.m_layoutInfo.IsSkip = true;
        if (this.Owner is InlineContentControl && this.FieldEnd != null)
          this.SkipLayoutingFieldItems(true);
        else
          this.SkipLayoutingOfFieldCode();
        if (this.IsSkipFieldResult())
          this.SkipLayoutingFieldItems(false);
      }
      if (this.IsNumPagesInsideExpressionField || !DocumentLayouter.IsLayoutingHeaderFooter || this.IsFieldInsideUnknownField || this.FieldType != FieldType.FieldIf && this.FieldType != FieldType.FieldCompare && this.FieldType != FieldType.FieldFormula)
        return;
      this.Update();
      this.SkipLayoutingOfFieldCode();
    }
  }

  internal bool IsSkipFieldResult()
  {
    if (this.FieldEnd == null)
      return false;
    if (this.FieldType == FieldType.FieldPage || this.FieldType == FieldType.FieldAutoNum || this.FieldType == FieldType.FieldSet || this.FieldType == FieldType.FieldSymbol || this.FieldType == FieldType.FieldUnknown && this.IsInvalidFieldCode() || this.FieldType == FieldType.FieldNumPages && DocumentLayouter.IsFirstLayouting)
      return true;
    return this.FieldType == FieldType.FieldSectionPages && DocumentLayouter.IsFirstLayouting;
  }

  private bool IsInvalidFieldCode()
  {
    string str = this.UpdateNestedFieldCode(false, (WMergeField) null).Trim();
    if (string.IsNullOrEmpty(str) || str.Contains("(") || str.Contains(")"))
      return true;
    char c = str[0];
    if ((int) c == (int) ControlChar.DoubleQuote && str.Length > 1)
      c = str[1];
    return !char.IsLetter(c);
  }

  internal void SetSkipForFieldCode(IEntity entity)
  {
    for (; entity != null && entity != this.FieldSeparator; entity = entity.NextSibling)
      (entity as IWidget).LayoutInfo.IsSkip = true;
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  internal void SetFieldTypeValue(FieldType fieldType) => this.m_fieldType = fieldType;

  protected internal virtual void ParseFieldCode(string fieldCode)
  {
    if (!(this.InternalFieldCode == "") || this.FieldType != FieldType.FieldFormTextInput)
      this.m_fieldType = FieldTypeDefiner.GetFieldType(fieldCode);
    this.UpdateFieldCode(fieldCode);
  }

  protected internal virtual void UpdateFieldCode(string fieldCode)
  {
    switch (this.m_fieldType)
    {
      case FieldType.FieldTOC:
        fieldCode = fieldCode.Trim();
        this.m_formattingString = new Regex("(TOC\\s+)(?<Options>.*)").Match(fieldCode).Groups["Options"].Value;
        break;
      case FieldType.FieldFormula:
        fieldCode = fieldCode.Trim();
        fieldCode = fieldCode.Replace("=", string.Empty);
        this.m_fieldValue = fieldCode;
        break;
      case FieldType.FieldPageRef:
        fieldCode = fieldCode.Trim();
        this.m_fieldValue = new Regex("(\\w+)\\s+\"?([^:\"]+):?([^\"]*)\"?").Match(fieldCode).Groups[2].Value;
        break;
      case FieldType.FieldFillIn:
        fieldCode = fieldCode.Trim();
        Match match1 = new Regex("(\\w+)\\s+\"?([^:\"]+)?([^\"]*)\"?").Match(fieldCode);
        int groupnum = 2;
        for (int count = match1.Groups.Count; groupnum < count; ++groupnum)
        {
          if (match1.Groups[groupnum].Length > 0)
            this.m_fieldValue += match1.Groups[groupnum].Value;
        }
        break;
      case FieldType.FieldLink:
        fieldCode = fieldCode.Trim();
        fieldCode = fieldCode.Replace("LINK ", string.Empty);
        this.m_fieldValue = fieldCode;
        break;
      case FieldType.FieldIncludePicture:
        fieldCode = fieldCode.Trim();
        Match match2 = new Regex("INCLUDEPICTURE\\s+\"([^\"]+)\"(?<Options>.*)").Match(fieldCode);
        this.m_fieldValue = $"\"{match2.Groups[1].Value}\"";
        this.m_formattingString = match2.Groups[2].Value;
        break;
      case FieldType.FieldHyperlink:
        fieldCode = fieldCode.Trim();
        Match match3 = new Regex("HYPERLINK\\s+(\\\\l\\s+)?[\"]?([^\"]+)(\"| )").Match(fieldCode);
        if (match3.Groups[2].Value == string.Empty)
          this.m_fieldValue = fieldCode.Replace("HYPERLINK", string.Empty);
        else if (match3.Groups[2].Value == "\\l")
        {
          this.m_fieldValue = fieldCode.Replace("HYPERLINK", string.Empty);
          this.m_fieldValue = this.m_fieldValue.Replace("\\l", string.Empty);
        }
        else
          this.m_fieldValue = $"\"{match3.Groups[2].Value}\"";
        int switchStartIndex1 = fieldCode.IndexOf("\\o");
        if (switchStartIndex1 != -1)
          this.SetScreenTipAndPositionSwitch(fieldCode, switchStartIndex1, "\\o ");
        int switchStartIndex2 = fieldCode.IndexOf("\\t");
        if (switchStartIndex2 != -1)
          this.SetScreenTipAndPositionSwitch(fieldCode, switchStartIndex2, "\\t ");
        int startPos = fieldCode.IndexOf("\\l");
        if (match3.Groups[1].Length <= 0 && startPos == -1)
          break;
        this.m_bIsLocal = true;
        this.SetLocalSwitchString();
        if (fieldCode.IndexOf(this.m_fieldValue) >= startPos)
          break;
        this.ParseLocalRef(fieldCode, startPos);
        break;
      default:
        this.ParseField(fieldCode);
        break;
    }
  }

  private bool CheckFieldWithoutSeparator()
  {
    switch (this.FieldType)
    {
      case FieldType.FieldIndexEntry:
      case FieldType.FieldTOCEntry:
      case FieldType.FieldRefDoc:
      case FieldType.FieldTOAEntry:
      case FieldType.FieldPrivate:
        return true;
      default:
        return false;
    }
  }

  internal bool AddFormattingString()
  {
    return this.m_fieldType == FieldType.FieldDocVariable || this.m_fieldType == FieldType.FieldTitle || this.m_fieldType == FieldType.FieldSubject;
  }

  internal string FindFieldCode() => FieldTypeDefiner.GetFieldCode(this.FieldType) + " ";

  private void SetTextFormatSwitchString()
  {
    this.m_formattingString = this.m_formattingString.Replace("\\* Upper", string.Empty);
    this.m_formattingString = this.m_formattingString.Replace("\\* Lower", string.Empty);
    this.m_formattingString = this.m_formattingString.Replace("\\* FirstCap", string.Empty);
    this.m_formattingString = this.m_formattingString.Replace("\\* Caps", string.Empty);
    switch (this.m_textFormat)
    {
      case TextFormat.Uppercase:
        this.m_formattingString += " \\* Upper ";
        break;
      case TextFormat.Lowercase:
        this.m_formattingString += " \\* Lower ";
        break;
      case TextFormat.FirstCapital:
        this.m_formattingString += " \\* FirstCap ";
        break;
      case TextFormat.Titlecase:
        this.m_formattingString += " \\* Caps ";
        break;
    }
  }

  private string UpdateTextFormatSwitchString(TextFormat newType)
  {
    string str = this.GetFieldCodeForUnknownFieldType();
    int startIndex = -1;
    if (str.Contains("\\* Upper"))
    {
      startIndex = str.IndexOf("\\* Upper");
      str = str.Replace("\\* Upper", string.Empty);
    }
    if (str.Contains("\\* Lower"))
    {
      startIndex = str.IndexOf("\\* Lower");
      str = str.Replace("\\* Lower", string.Empty);
    }
    if (str.Contains("\\* FirstCap"))
    {
      startIndex = str.IndexOf("\\* FirstCap");
      str = str.Replace("\\* FirstCap", string.Empty);
    }
    if (str.Contains("\\* Caps"))
    {
      startIndex = str.IndexOf("\\* Caps");
      str = str.Replace("\\* Caps", string.Empty);
    }
    if (startIndex == -1)
      startIndex = str.IndexOf("\\* MERGEFORMAT");
    switch (newType)
    {
      case TextFormat.Uppercase:
        str = startIndex == -1 ? str + " \\* Upper " : str.Insert(startIndex, " \\* Upper ");
        break;
      case TextFormat.Lowercase:
        str = startIndex == -1 ? str + " \\* Lower " : str.Insert(startIndex, " \\* Lower ");
        break;
      case TextFormat.FirstCapital:
        str = startIndex == -1 ? str + " \\* FirstCap " : str.Insert(startIndex, " \\* FirstCap ");
        break;
      case TextFormat.Titlecase:
        str = startIndex == -1 ? str + " \\* Caps " : str.Insert(startIndex, " \\* Caps ");
        break;
    }
    return str;
  }

  private void SetLocalSwitchString()
  {
    this.m_formattingString = this.m_formattingString.Replace(" \\l", string.Empty);
    if (!this.IsLocal)
      return;
    this.m_formattingString += " \\l";
  }

  private void SetScreenTipAndPositionSwitch(
    string fieldCode,
    int switchStartIndex,
    string formattingSwitch)
  {
    switchStartIndex += 2;
    string str = fieldCode.Substring(switchStartIndex, fieldCode.Length - switchStartIndex).Trim();
    if (!this.StartsWithExt(str.TrimStart(), "\""))
      return;
    this.m_formattingString += formattingSwitch;
    string[] strArray = str.Split(new char[1]{ '"' }, StringSplitOptions.RemoveEmptyEntries);
    if (strArray.Length <= 0)
      return;
    WField wfield = this;
    wfield.m_formattingString = $"{wfield.m_formattingString}\"{strArray[0]}\"";
    this.m_screenTip = formattingSwitch == "\\o " ? strArray[0] : this.m_screenTip;
  }

  protected internal virtual string ConvertSwitchesToString()
  {
    this.SetTextFormatSwitchString();
    this.SetLocalSwitchString();
    return this.m_formattingString;
  }

  internal override void AttachToParagraph(WParagraph owner, int itemPos)
  {
    this.Attach(owner, itemPos, true);
    if (!this.DeepDetached)
    {
      this.Document.Fields.Add(this);
      this.IsCloned = false;
    }
    else
      this.IsCloned = true;
  }

  internal override void Detach()
  {
    if (this.FieldEnd != null && this.FieldEnd.Owner is WParagraph && !this.Document.IsSkipFieldDetach && this.Owner is WParagraph && !this.OwnerParagraph.Document.IsClosing)
    {
      bool isFoundFieldEnd = false;
      this.RemoveItemsUptoFieldEnd(this.OwnerParagraph, this.GetIndexInOwnerCollection() + 1, ref isFoundFieldEnd);
      if (this.OwnerParagraph.OwnerTextBody != null && this.FieldEnd.OwnerParagraph != null && this.FieldEnd.OwnerParagraph.OwnerTextBody != null && this.OwnerParagraph.OwnerTextBody == this.FieldEnd.OwnerParagraph.OwnerTextBody && this.OwnerParagraph != this.FieldEnd.OwnerParagraph)
      {
        for (int index = this.OwnerParagraph.GetIndexInOwnerCollection() + 1; index < this.OwnerParagraph.OwnerTextBody.Items.Count; index = index - 1 + 1)
        {
          Entity entity = (Entity) this.OwnerParagraph.OwnerTextBody.Items[index];
          if (entity == this.FieldEnd.OwnerParagraph)
          {
            WParagraph ownerParagraph = this.FieldEnd.OwnerParagraph;
            this.RemoveItemsUptoFieldEnd(ownerParagraph, 0, ref isFoundFieldEnd);
            while (ownerParagraph.ChildEntities.Count > 0)
              this.OwnerParagraph.ChildEntities.Add((IEntity) ownerParagraph.ChildEntities[0]);
            this.OwnerParagraph.ParagraphFormat.CopyFormat((FormatBase) ownerParagraph.ParagraphFormat);
            this.OwnerParagraph.BreakCharacterFormat.CopyFormat((FormatBase) ownerParagraph.BreakCharacterFormat);
            ownerParagraph.RemoveSelf();
            break;
          }
          this.OwnerParagraph.OwnerTextBody.Items.Remove((IEntity) entity);
        }
      }
    }
    else if (this.FieldEnd != null && (this.Owner is InlineContentControl || this.FieldEnd.Owner is InlineContentControl))
    {
      WParagraph ownerParagraphValue1 = this.GetOwnerParagraphValue();
      WParagraph ownerParagraphValue2 = this.FieldEnd != null ? this.FieldEnd.GetOwnerParagraphValue() : (WParagraph) null;
      if (this.FieldEnd != null && ownerParagraphValue2 != null && !this.Document.IsSkipFieldDetach && !ownerParagraphValue1.Document.IsClosing)
        this.RemoveUptoFieldEndInInlineControl(ownerParagraphValue1, ownerParagraphValue2);
    }
    this.Detach(true);
    if (this.DeepDetached)
      return;
    this.Document.Fields.Remove(this);
  }

  private void RemoveUptoFieldEndInInlineControl(WParagraph ownerPara, WParagraph fieldEndOwnerPara)
  {
    bool isFoundFieldEnd = false;
    if (this.Owner is WParagraph)
      this.RemoveItemsUptoFieldEnd(this.OwnerParagraph, this.GetIndexInOwnerCollection() + 1, ref isFoundFieldEnd);
    else if (this.Owner is InlineContentControl)
    {
      this.RemoveItemsUptoFieldEnd(this.Owner as InlineContentControl, this.GetIndexInOwnerCollection() + 1, ref isFoundFieldEnd);
      if (!isFoundFieldEnd)
      {
        Entity owner = this.Owner;
        while (!(owner is WParagraph) && owner != null && !(owner.Owner is WParagraph))
          owner = owner.Owner;
        this.RemoveItemsUptoFieldEnd(owner.Owner as WParagraph, owner.Index + 1, ref isFoundFieldEnd);
      }
    }
    if (ownerPara.OwnerTextBody == null || fieldEndOwnerPara == null || fieldEndOwnerPara.OwnerTextBody == null || ownerPara.OwnerTextBody != fieldEndOwnerPara.OwnerTextBody || ownerPara == fieldEndOwnerPara || isFoundFieldEnd)
      return;
    for (int index = ownerPara.GetIndexInOwnerCollection() + 1; index < ownerPara.OwnerTextBody.Items.Count && !isFoundFieldEnd; index = index - 1 + 1)
    {
      Entity entity = (Entity) ownerPara.OwnerTextBody.Items[index];
      if (entity == fieldEndOwnerPara)
      {
        this.RemoveItemsUptoFieldEnd(fieldEndOwnerPara, 0, ref isFoundFieldEnd);
        break;
      }
      this.OwnerParagraph.OwnerTextBody.Items.Remove((IEntity) entity);
    }
  }

  private void RemoveItemsUptoFieldEnd(
    WParagraph ownerParagraph,
    int startItemIndex,
    ref bool isFoundFieldEnd)
  {
    int num;
    for (int index = startItemIndex; index < ownerParagraph.ChildEntities.Count && !isFoundFieldEnd; index = num + 1)
    {
      Entity inlineContenControl = (Entity) ownerParagraph.Items[index];
      if (inlineContenControl is InlineContentControl && this.FieldEnd.Owner == inlineContenControl)
        this.RemoveItemsUptoFieldEnd(inlineContenControl as InlineContentControl, 0, ref isFoundFieldEnd);
      else
        ownerParagraph.Items.Remove((IEntity) inlineContenControl);
      num = index - 1;
      if (inlineContenControl == this.FieldEnd)
      {
        isFoundFieldEnd = true;
        break;
      }
    }
  }

  private void RemoveItemsUptoFieldEnd(
    InlineContentControl inlineContenControl,
    int startItemIndex,
    ref bool isFoundFieldEnd)
  {
    int num;
    for (int index = startItemIndex; index < inlineContenControl.ParagraphItems.Count && !isFoundFieldEnd; index = num + 1)
    {
      Entity paragraphItem = (Entity) inlineContenControl.ParagraphItems[index];
      if (paragraphItem is InlineContentControl && this.FieldEnd.Owner == paragraphItem)
        this.RemoveItemsUptoFieldEnd(paragraphItem as InlineContentControl, 0, ref isFoundFieldEnd);
      else
        inlineContenControl.ParagraphItems.Remove((IEntity) paragraphItem);
      num = index - 1;
      if (paragraphItem == this.FieldEnd)
      {
        isFoundFieldEnd = true;
        break;
      }
    }
    if (isFoundFieldEnd)
      return;
    Entity owner = inlineContenControl.Owner;
    if (!(owner is InlineContentControl))
      return;
    this.RemoveItemsUptoFieldEnd(owner as InlineContentControl, inlineContenControl.GetIndexInOwnerCollection() + 1, ref isFoundFieldEnd);
  }

  internal override void AttachToDocument()
  {
    if (!this.IsCloned)
      return;
    this.Document.Fields.Add(this);
    this.IsCloned = false;
  }

  protected override object CloneImpl()
  {
    WField owner = (WField) this.CloneImpl(true);
    owner.m_range = new Range(this.Document, (OwnerHolder) owner);
    owner.m_bFlags = (short) 1;
    if (this.FieldType == FieldType.FieldAutoNum)
      owner.OriginalField = this;
    return (object) owner;
  }

  internal override void Close()
  {
    if (this.m_range != null)
    {
      this.m_range.Close();
      this.m_range = (Range) null;
    }
    if (this.m_nestedFields != null)
    {
      this.m_nestedFields.Clear();
      this.m_nestedFields = (Stack<WField>) null;
    }
    this.m_fieldSeparator = (WFieldMark) null;
    this.m_fieldEnd = (WFieldMark) null;
    if (this.m_fieldValue != null)
      this.m_fieldValue = (string) null;
    if (this.m_fieldPattern != null)
      this.m_fieldPattern = (string) null;
    if (this.m_formattingString != null)
      this.m_formattingString = (string) null;
    if (this.m_fieldResult != null)
      this.m_fieldResult = (string) null;
    this.originalField = (WField) null;
    if (this.functions != null)
    {
      this.functions.Clear();
      this.functions = (List<string>) null;
    }
    base.Close();
  }

  internal WSymbol GetAsSymbol()
  {
    string[] strArray = this.FieldCode.Split('\\');
    string empty = string.Empty;
    float num = 0.0f;
    WSymbol asSymbol = new WSymbol((IWordDocument) this.Document);
    asSymbol.SetOwner((OwnerHolder) this.OwnerParagraph);
    foreach (string str1 in strArray)
    {
      char[] chArray = new char[1]{ ControlChar.SpaceChar };
      string text = str1.TrimStart(chArray);
      if (this.StartsWithExt(text.ToUpper(), "SYMBOL"))
        asSymbol.CharacterCode = Convert.ToByte(text.ToUpper().Replace("SYMBOL", string.Empty).Trim(ControlChar.SpaceChar));
      else if (this.StartsWithExt(text, "f"))
      {
        string str2 = text.Replace("f", string.Empty).Trim(ControlChar.SpaceChar);
        asSymbol.FontName = str2.Trim('"');
      }
      else if (this.StartsWithExt(text, "s"))
        num = (float) Convert.ToDouble(text.Replace("s", string.Empty).Trim(ControlChar.SpaceChar));
    }
    asSymbol.CharacterFormat.ImportContainer((FormatBase) this.CharacterFormat);
    asSymbol.CharacterFormat.CopyProperties((FormatBase) this.CharacterFormat);
    if (this.CharacterFormat.BaseFormat != null)
      asSymbol.CharacterFormat.ApplyBase(this.CharacterFormat.BaseFormat);
    if ((double) num > 0.0)
      asSymbol.CharacterFormat.SetPropertyValue(3, (object) num);
    return asSymbol;
  }

  internal string GetFieldCodeForUnknownFieldType()
  {
    string internalFieldCode = this.InternalFieldCode;
    if (this.OwnerParagraph == null)
      return internalFieldCode;
    WFieldMark wfieldMark = this.FieldSeparator == null ? this.FieldEnd : this.FieldSeparator;
    if (wfieldMark == null || wfieldMark.OwnerParagraph == null)
      return internalFieldCode;
    this.IsFieldRangeUpdated = false;
    if (this.Range.Items.Count == 0)
      this.UpdateFieldRange();
    string unknownFieldType = this.UpdateNestedFieldCode(false, (WMergeField) null);
    this.IsFieldRangeUpdated = false;
    this.Range.Items.Clear();
    return unknownFieldType;
  }

  internal void UpdateUnknownFieldType(WCharacterFormat resultFormat)
  {
    string unknownFieldType = this.GetFieldCodeForUnknownFieldType();
    if (!(unknownFieldType != string.Empty))
      return;
    string fieldCode = unknownFieldType.Trim();
    FieldType fieldType = FieldTypeDefiner.GetFieldType(fieldCode);
    bool flag = true;
    if (fieldType == this.FieldType)
    {
      if (unknownFieldType != this.InternalFieldCode)
        this.UpdateFieldCode(unknownFieldType);
      if (resultFormat == null)
        return;
      this.ApplyCharacterFormat(resultFormat);
    }
    else if (fieldType != FieldType.FieldUnknown)
    {
      WField wfield = (WField) null;
      switch (fieldType)
      {
        case FieldType.FieldIf:
          wfield = (WField) new WIfField((IWordDocument) this.m_doc);
          break;
        case FieldType.FieldTOC:
          if (this.m_doc.TOC.Count == 0)
          {
            ParagraphItemCollection items = this.OwnerParagraph.Items;
            int index = this.Index;
            this.UpdateFieldCode(unknownFieldType);
            TableOfContent tableOfContent = new TableOfContent((IWordDocument) this.m_doc, this.FormattingString);
            this.m_doc.TOC.Add(tableOfContent.TOCField, tableOfContent);
            items.Remove((IEntity) this);
            items.Insert(index, (IEntity) tableOfContent);
            if (resultFormat != null)
              tableOfContent.SetParagraphItemCharacterFormat(resultFormat);
          }
          flag = false;
          break;
        case FieldType.FieldMergeField:
          wfield = (WField) new WMergeField((IWordDocument) this.m_doc);
          break;
        case FieldType.FieldFormTextInput:
        case FieldType.FieldFormCheckBox:
        case FieldType.FieldFormDropDown:
          if (this.StartsWithExt(fieldCode.ToUpper(), "TEXTINPUT") || this.StartsWithExt(fieldCode.ToUpper(), "FORMTEXT"))
            wfield = (WField) new WTextFormField((IWordDocument) this.m_doc);
          else if (this.StartsWithExt(fieldCode.ToUpper(), "DDLIST") || this.StartsWithExt(fieldCode.ToUpper(), "FORMDROPDOWN"))
            wfield = (WField) new WDropDownFormField((IWordDocument) this.m_doc);
          else if (this.StartsWithExt(fieldCode.ToUpper(), "CHECKBOX") || this.StartsWithExt(fieldCode.ToUpper(), "FORMCHECKBOX"))
            wfield = (WField) new WCheckBox((IWordDocument) this.m_doc);
          if (wfield != null)
          {
            (wfield as WFormField).HasFFData = false;
            break;
          }
          break;
        default:
          this.m_fieldType = fieldType;
          this.UpdateFieldCode(unknownFieldType);
          if (resultFormat != null)
            this.ApplyCharacterFormat(resultFormat);
          flag = false;
          break;
      }
      if (!flag || wfield == null)
        return;
      ParagraphItemCollection items1 = this.OwnerParagraph.Items;
      int index1 = this.Index;
      items1.RemoveFromInnerList(index1);
      items1.InsertToInnerList(index1, (IEntity) wfield);
      wfield.SetOwner((OwnerHolder) this.OwnerParagraph);
      wfield.FieldSeparator = this.FieldSeparator;
      wfield.FieldEnd = this.FieldEnd;
      if (wfield.FieldEnd != null)
        wfield.RemovePreviousFieldCode();
      wfield.m_fieldType = fieldType;
      WTextRange wtextRange = new WTextRange((IWordDocument) this.m_doc);
      wtextRange.Text = unknownFieldType;
      wtextRange.ApplyCharacterFormat(wfield.CharacterFormat);
      items1.Insert(index1 + 1, (IEntity) wtextRange);
      wfield.UpdateFieldCode(unknownFieldType);
      if (resultFormat != null)
        wfield.ApplyCharacterFormat(resultFormat);
      else if (this.CharacterFormat != null)
        wfield.ApplyCharacterFormat(this.CharacterFormat);
      int index2 = this.Document.Fields.InnerList.IndexOf((object) this);
      if (index2 == -1)
        return;
      this.Document.Fields.InnerList.RemoveAt(index2);
      this.Document.Fields.InnerList.Insert(index2, (object) wfield);
    }
    else
      this.m_fieldType = fieldType;
  }

  internal WField CreateFieldByType(string fieldCode, FieldType fieldType)
  {
    WField fieldByType = (WField) null;
    if (fieldType == FieldType.FieldUnknown)
      fieldType = FieldTypeDefiner.GetFieldType(fieldCode.Trim());
    switch (fieldType)
    {
      case FieldType.FieldIf:
        fieldByType = (WField) new WIfField((IWordDocument) this.Document);
        break;
      case FieldType.FieldSequence:
        fieldByType = (WField) new WSeqField((IWordDocument) this.Document);
        break;
      case FieldType.FieldEmbed:
        fieldByType = (WField) new WEmbedField((IWordDocument) this.Document);
        break;
      case FieldType.FieldMergeField:
        fieldByType = (WField) new WMergeField((IWordDocument) this.Document);
        break;
      case FieldType.FieldFormTextInput:
        fieldByType = (WField) new WTextFormField((IWordDocument) this.Document);
        this.Document.SetTriggerElement(ref this.Document.m_supportedElementFlag_2, 14);
        break;
      case FieldType.FieldFormCheckBox:
        fieldByType = (WField) new WCheckBox((IWordDocument) this.Document);
        this.Document.SetTriggerElement(ref this.Document.m_supportedElementFlag_1, 9);
        break;
      case FieldType.FieldFormDropDown:
        fieldByType = (WField) new WDropDownFormField((IWordDocument) this.Document);
        this.Document.SetTriggerElement(ref this.Document.m_supportedElementFlag_1, 15);
        break;
      case FieldType.FieldOCX:
        fieldByType = (WField) new WControlField((IWordDocument) this.Document);
        break;
      default:
        if (fieldByType == null)
        {
          fieldByType = new WField((IWordDocument) this.Document);
          break;
        }
        break;
    }
    if (fieldByType is WFormField)
      (fieldByType as WFormField).HasFFData = false;
    fieldByType.FieldType = fieldType;
    this.Document.SetTriggerElement(ref this.Document.m_supportedElementFlag_1, 18);
    return fieldByType;
  }

  internal bool IsFormField()
  {
    switch (this.FieldType)
    {
      case FieldType.FieldFormTextInput:
      case FieldType.FieldFormCheckBox:
      case FieldType.FieldFormDropDown:
        return true;
      default:
        return false;
    }
  }

  internal void RemoveFieldCodeItems()
  {
    this.IsFieldRangeUpdated = false;
    if (this.Range.Items.Count == 0)
      this.UpdateFieldRange();
    WFieldMark wfieldMark = this.FieldSeparator == null ? this.FieldEnd : this.FieldSeparator;
    for (int index = 0; index < this.Range.Items.Count; ++index)
    {
      Entity entity = this.Range.Items[index] as Entity;
      if (entity != wfieldMark)
      {
        if (entity is WTextRange)
          entity.RemoveSelf();
      }
      else
        break;
    }
    this.Range.Items.Clear();
    this.IsFieldRangeUpdated = false;
  }

  private string FindFieldResult()
  {
    if (this.FieldSeparator == null)
      return string.Empty;
    this.IsFieldRangeUpdated = false;
    if (this.Range.Items.Count == 0)
      this.UpdateFieldRange();
    string fieldResult = string.Empty;
    this.IsFieldSeparator = false;
    int num = 0;
    if (this.OwnerParagraph == this.FieldSeparator.OwnerParagraph)
      num = this.Range.Items.IndexOf((object) this.FieldSeparator) + 1;
    else if (this.FieldSeparator.OwnerParagraph != null)
    {
      WParagraph ownerParagraph = this.FieldSeparator.OwnerParagraph;
      for (int index = this.FieldSeparator.Index + 1; index < ownerParagraph.Items.Count; ++index)
      {
        if (ownerParagraph.Items[index] == this.FieldEnd)
        {
          this.Range.Items.Clear();
          this.IsFieldRangeUpdated = false;
          return fieldResult;
        }
        fieldResult += this.UpdateTextForParagraphItem((Entity) ownerParagraph.Items[index], false);
      }
      num = this.Range.Items.IndexOf((object) ownerParagraph) + 1;
    }
    for (int index1 = num; index1 < this.Range.Items.Count; ++index1)
    {
      if (index1 == this.Range.Items.Count - 1 && this.Range.Items[index1] is WParagraph && this.Range.Items[index1] == this.FieldEnd.OwnerParagraph)
      {
        WParagraph ownerParagraph = this.FieldEnd.OwnerParagraph;
        for (int index2 = 0; index1 < ownerParagraph.Items.Count && ownerParagraph.Items[index2] != this.FieldEnd; ++index1)
          fieldResult += this.UpdateTextForParagraphItem((Entity) ownerParagraph.Items[index2], false);
      }
      else
        fieldResult = !(this.Range.Items[index1] is ParagraphItem) ? fieldResult + this.UpdateTextForTextBodyItem(this.Range.Items[index1] as Entity, false) : fieldResult + this.UpdateTextForParagraphItem(this.Range.Items[index1] as Entity, false);
    }
    this.Range.Items.Clear();
    this.IsFieldRangeUpdated = false;
    return fieldResult;
  }

  internal void ReplaceAsTOCField()
  {
    if (this.Document.TOC.ContainsKey(this))
      return;
    if (this.Document.TOC.Count > 0)
      this.RemoveNestedTOCFields();
    this.Document.SetTriggerElement(ref this.Document.m_supportedElementFlag_2, 15);
    this.UpdateFieldCode(this.GetFieldCodeForUnknownFieldType());
    WField key = this;
    WParagraph ownerParagraph = this.OwnerParagraph;
    bool isSkipFieldDetach = this.Document.IsSkipFieldDetach;
    TableOfContent tableOfContent = new TableOfContent((IWordDocument) this.Document, this.FormattingString);
    tableOfContent.TOCField.FieldSeparator = this.FieldSeparator;
    tableOfContent.TOCField.FieldEnd = this.FieldEnd;
    this.Document.TOC.Add(key, tableOfContent);
    this.Document.IsSkipFieldDetach = true;
    ownerParagraph.Items.Remove((IEntity) key);
    this.Document.IsSkipFieldDetach = isSkipFieldDetach;
    ownerParagraph.Items.Insert(this.Index, (IEntity) tableOfContent);
  }

  private void RemoveNestedTOCFields()
  {
    this.IsFieldRangeUpdated = false;
    if (this.Range.Items.Count == 0)
      this.UpdateFieldRange();
    for (int index = 0; index < this.Range.Items.Count; ++index)
    {
      if (this.Range.Items[index] is TableOfContent)
      {
        TableOfContent tableOfContent = this.Range.Items[index] as TableOfContent;
        WField key = tableOfContent.FindKey();
        if (key != null)
        {
          this.Document.TOC.Remove(key);
          WParagraph ownerParagraph = tableOfContent.OwnerParagraph;
          ownerParagraph.Items.Insert(tableOfContent.Index, (IEntity) key);
          ownerParagraph.Items.Remove((IEntity) tableOfContent);
          break;
        }
        break;
      }
    }
    this.Range.Items.Clear();
    this.IsFieldRangeUpdated = false;
  }

  internal WField ReplaceValidField()
  {
    ParagraphItemCollection paragraphItemCollection = this.OwnerParagraph.Items;
    if (this.Owner is InlineContentControl)
      paragraphItemCollection = (this.Owner as InlineContentControl).ParagraphItems;
    int index1 = this.Index;
    WField fieldByType = this.CreateFieldByType(string.Empty, this.FieldType);
    fieldByType.FieldSeparator = this.FieldSeparator;
    fieldByType.FieldEnd = this.FieldEnd;
    WField wfield = paragraphItemCollection[index1] as WField;
    int index2 = this.m_doc.Fields.InnerList.IndexOf((object) wfield);
    if (index2 != -1)
      this.m_doc.Fields.InnerList.RemoveAt(index2);
    paragraphItemCollection.RemoveFromInnerList(index1);
    paragraphItemCollection.Insert(index1, (IEntity) fieldByType);
    if (wfield.CharacterFormat != null)
      fieldByType.ApplyCharacterFormat(wfield.CharacterFormat);
    for (int index3 = 0; index3 < wfield.RevisionsInternal.Count; ++index3)
    {
      fieldByType.RevisionsInternal.Add(wfield.RevisionsInternal[index3]);
      int index4 = wfield.RevisionsInternal[index3].Range.InnerList.IndexOf((object) wfield);
      if (index4 >= 0)
        wfield.RevisionsInternal[index3].Range.InnerList.Remove((object) wfield);
      wfield.RevisionsInternal[index3].Range.InnerList.Insert(index4, (object) fieldByType);
    }
    return fieldByType;
  }

  internal void SetUnknownFieldType()
  {
    WFieldMark wfieldMark = this.FieldSeparator != null ? this.FieldSeparator : this.FieldEnd;
    if (this.FieldEnd == null || this.OwnerParagraph == null)
      return;
    ParagraphItemCollection paragraphItemCollection = this.OwnerParagraph.Items;
    if (this.Owner is InlineContentControl)
      paragraphItemCollection = (this.Owner as InlineContentControl).ParagraphItems;
    this.IsFieldSeparator = false;
    string str = string.Empty;
    for (int index = this.Index + 1; index < paragraphItemCollection.Count; ++index)
    {
      ParagraphItem paragraphItem = paragraphItemCollection[index];
      if (index > this.Index + 1 && paragraphItem is WField && this.UpdateFieldType(str))
      {
        this.IsFieldSeparator = false;
        return;
      }
      str += this.UpdateTextForParagraphItem((Entity) paragraphItem, false);
      if (paragraphItem == wfieldMark)
      {
        this.UpdateFieldType(str);
        this.IsFieldSeparator = false;
        return;
      }
    }
    if (this.OwnerParagraph != wfieldMark.OwnerParagraph)
      str = this.UpdateTextBodyFieldCode(this.OwnerParagraph.OwnerTextBody, wfieldMark.OwnerParagraph, this.OwnerParagraph.Index + 1, str);
    this.UpdateFieldType(str);
    this.IsFieldSeparator = false;
  }

  internal void EnusreSpaceInResultText(
    WMergeField mergeField,
    string resultText,
    string textBefore,
    string textAfter)
  {
    bool isDoubleQuote = false;
    string str1 = this.RemoveText(this.UpdateNestedFieldCode(false, mergeField).TrimStart(), "if", false);
    char[] chArray = new char[3]{ ' ', ' ', '"' };
    List<string> operators = new List<string>((IEnumerable<string>) new string[6]
    {
      "<=",
      ">=",
      "<>",
      "=",
      "<",
      ">"
    });
    bool flag = false;
    if (str1 == string.Empty)
    {
      mergeField.TextBefore = " " + textBefore;
    }
    else
    {
      string text = str1.TrimEnd(chArray);
      if (str1.Length == text.Length)
      {
        List<int> operatorIndex = this.GetOperatorIndex(operators, text, ref isDoubleQuote);
        if (flag = operatorIndex.Count > 0)
        {
          string str2 = text.Substring(operatorIndex[0]);
          if (operators.Contains(str2))
            mergeField.TextBefore = " " + textBefore;
        }
      }
      else
        flag = this.GetOperatorIndex(operators, text, ref isDoubleQuote).Count > 0;
    }
    if (flag)
      return;
    string str3 = this.RemoveText(this.UpdateNestedFieldCode(false, (WMergeField) null).TrimStart(), "if", false).Remove(0, str1.Length).TrimStart(chArray);
    if (str3.StartsWith(mergeField.Text))
      str3 = str3.Remove(0, mergeField.Text.Length);
    string text1 = str3.TrimStart(chArray);
    if (str3.Length != text1.Length)
      return;
    List<int> operatorIndex1 = this.GetOperatorIndex(operators, text1, ref isDoubleQuote);
    if (operatorIndex1.Count <= 0 || !(text1.Remove(operatorIndex1[0]) == string.Empty))
      return;
    mergeField.TextAfter = textAfter + " ";
  }

  private string UpdateTextBodyFieldCode(
    WTextBody tBody,
    WParagraph endParagraph,
    int startIndex,
    string code)
  {
    for (int index = startIndex; index < tBody.Items.Count; ++index)
    {
      Entity entity = (Entity) tBody.Items[index];
      code = !(entity is BlockContentControl) ? code + this.UpdateTextForTextBodyItem(entity, false) : code + this.UpdateTextBodyFieldCode((entity as BlockContentControl).TextBody, endParagraph, index == startIndex ? startIndex : 0, code);
      if (entity == endParagraph)
        return code;
    }
    return code;
  }

  private bool UpdateFieldType(string fieldCode)
  {
    FieldType fieldType = FieldTypeDefiner.GetFieldType(fieldCode);
    if (fieldType == FieldType.FieldUnknown)
      return false;
    this.FieldType = fieldType;
    return true;
  }

  internal void RemovePreviousFieldCode()
  {
    if (!(this.Owner is WParagraph))
      return;
    WFieldMark wfieldMark = this.FieldSeparator == null ? this.FieldEnd : this.FieldSeparator;
    if (wfieldMark == null || !(wfieldMark.Owner is WParagraph) || this.NextSibling is WFieldMark && ((this.NextSibling as WFieldMark).Type == FieldMarkType.FieldEnd || (this.NextSibling as WFieldMark).Type == FieldMarkType.FieldSeparator))
      return;
    WParagraph ownerParagraph1 = this.OwnerParagraph;
    WParagraph ownerParagraph2 = wfieldMark.OwnerParagraph;
    if (ownerParagraph1.OwnerTextBody == null || this.GetSection((Entity) ownerParagraph1.OwnerTextBody) == null || ownerParagraph2.OwnerTextBody == null || this.GetSection((Entity) ownerParagraph2.OwnerTextBody) == null)
    {
      if (ownerParagraph1 == ownerParagraph2)
      {
        int index1 = this.GetIndexInOwnerCollection() + 1;
        int inOwnerCollection = wfieldMark.GetIndexInOwnerCollection();
        for (int index2 = index1; index2 < inOwnerCollection && index1 < ownerParagraph1.ChildEntities.Count; ++index2)
          ownerParagraph1.ChildEntities.RemoveFromInnerList(index1);
      }
    }
    else
    {
      int index = this.GetIndexInOwnerCollection() + 1;
      BookmarkStart bookmarkStart = new BookmarkStart((IWordDocument) this.m_doc, "_FieldBookmark");
      BookmarkEnd bookmarkEnd = new BookmarkEnd((IWordDocument) this.m_doc, "_FieldBookmark");
      ownerParagraph1.Items.Insert(index, (IEntity) bookmarkStart);
      this.EnsureBookmarkStart(bookmarkStart);
      int inOwnerCollection = wfieldMark.GetIndexInOwnerCollection();
      ownerParagraph2.Items.Insert(inOwnerCollection, (IEntity) bookmarkEnd);
      this.EnsureBookmarkStart(bookmarkEnd);
      BookmarksNavigator bookmarksNavigator = new BookmarksNavigator((IWordDocument) this.m_doc);
      bookmarksNavigator.MoveToBookmark("_FieldBookmark");
      bookmarksNavigator.RemoveEmptyParagraph = false;
      this.Document.IsSkipFieldDetach = true;
      bookmarksNavigator.DeleteBookmarkContent(false);
      this.Document.IsSkipFieldDetach = false;
      if (ownerParagraph1.Items.Contains((IEntity) bookmarkStart))
        ownerParagraph1.Items.Remove((IEntity) bookmarkStart);
      if (ownerParagraph2.Items.Contains((IEntity) bookmarkEnd))
        ownerParagraph2.Items.Remove((IEntity) bookmarkEnd);
    }
    this.IsFieldRangeUpdated = false;
  }

  private Entity GetSection(Entity entity)
  {
    while (!(entity is WSection))
    {
      entity = entity.Owner;
      if (entity == null)
        break;
    }
    return entity;
  }

  internal void EnsureBookmarkStart(BookmarkStart bookmarkStart)
  {
    if (this.m_doc.Bookmarks.FindByName(bookmarkStart.Name) != null)
      return;
    this.m_doc.Bookmarks.AttachBookmarkStart(bookmarkStart);
  }

  internal void EnsureBookmarkStart(BookmarkEnd bookmarkEnd)
  {
    Bookmark byName = this.m_doc.Bookmarks.FindByName(bookmarkEnd.Name);
    if (byName == null || byName.BookmarkEnd != null)
      return;
    this.m_doc.Bookmarks.AttachBookmarkEnd(bookmarkEnd);
  }

  internal List<WCharacterFormat> GetResultCharacterFormatting()
  {
    List<WCharacterFormat> characterFormats = new List<WCharacterFormat>();
    if ((this.Document.Settings.MaintainFormattingOnFieldUpdate || this.FormattingString.ToUpper().Contains("\\* MERGEFORMAT")) && this.FieldSeparator != null && this.FieldSeparator.NextSibling != null && this.FieldSeparator.NextSibling != this.FieldEnd)
    {
      int num = this.Range.Items.IndexOf((object) this.FieldSeparator.NextSibling);
      if (num == -1 && this.FieldSeparator.NextSibling.Owner != null)
        num = this.Range.Items.IndexOf((object) this.FieldSeparator.NextSibling.Owner);
      List<object> items = new List<object>();
      if (num != -1)
      {
        for (int index = num; index < this.Range.Items.Count; ++index)
        {
          object obj = this.Range.Items[index];
          if (!(obj is Entity) || !(obj as Entity).Equals((object) this.FieldEnd))
            items.Add(obj);
          else
            break;
        }
      }
      if (items.Count <= 1)
        characterFormats.Add((this.FieldSeparator.NextSibling as ParagraphItem).ParaItemCharFormat);
      else
        this.UpdateRangeCharacterFormats((IList) items, ref characterFormats, false);
      return characterFormats;
    }
    if (this.InternalFieldCode.Trim() != string.Empty)
    {
      characterFormats.Add(this.CharacterFormat);
      return characterFormats;
    }
    this.UpdateRangeCharacterFormats(this.Range.Items, ref characterFormats, true);
    return characterFormats;
  }

  private void UpdateRangeCharacterFormats(
    IList items,
    ref List<WCharacterFormat> characterFormats,
    bool isNotMergeFormat)
  {
    for (int index1 = 0; index1 < items.Count; ++index1)
    {
      Entity textRange = items[index1] as Entity;
      int count = 0;
      if (textRange is ParagraphItem && textRange.EntityType == EntityType.TextRange)
      {
        WCharacterFormat charFormatFromRange = this.GetResultCharFormatFromRange(textRange as WTextRange, index1, ref count);
        if (charFormatFromRange != null)
        {
          if (isNotMergeFormat)
          {
            characterFormats.Add(charFormatFromRange);
            break;
          }
          if (count >= 2 || index1 == 0)
          {
            for (; count > 0; --count)
              characterFormats.Add(charFormatFromRange);
          }
        }
      }
      else if (textRange is WParagraph)
      {
        for (int index2 = 0; index2 < (textRange as WParagraph).ChildEntities.Count; ++index2)
        {
          Entity childEntity = (textRange as WParagraph).ChildEntities[index2];
          if (childEntity is ParagraphItem && childEntity.EntityType == EntityType.TextRange)
          {
            WCharacterFormat charFormatFromRange = this.GetResultCharFormatFromRange(childEntity as WTextRange, index1, ref count);
            if (charFormatFromRange != null)
            {
              if (isNotMergeFormat)
              {
                characterFormats.Add(charFormatFromRange);
                break;
              }
              if (count >= 2 || index1 == 0)
              {
                for (; count > 0; --count)
                  characterFormats.Add(charFormatFromRange);
              }
            }
          }
        }
      }
      else if (isNotMergeFormat)
        break;
    }
  }

  private WCharacterFormat GetResultCharFormatFromRange(WTextRange textRange, int i, ref int count)
  {
    char[] chArray = new char[9]
    {
      ' ',
      ',',
      '.',
      '/',
      '-',
      ':',
      '\t',
      '�',
      '�'
    };
    string str = textRange.Text;
    if ((!(str.Trim() != string.Empty) || !(str.Trim(chArray) != string.Empty)) && i != 0)
      return (WCharacterFormat) null;
    count = str.Length;
    foreach (char ch in chArray)
      str = str.Replace(ch.ToString(), string.Empty);
    count = count - str.Length + 1;
    return textRange.GetCharFormat();
  }

  internal ParagraphItem GetIncudePictureFieldResult()
  {
    if (this.FieldType != FieldType.FieldIncludePicture || this.FieldSeparator == null || !(this.FieldSeparator.Owner is WParagraph))
      return (ParagraphItem) null;
    int num;
    if (this.Range.Items.Contains((object) this.FieldSeparator))
    {
      num = this.Range.Items.IndexOf((object) this.FieldSeparator);
    }
    else
    {
      if (!this.Range.Items.Contains((object) this.FieldSeparator.OwnerParagraph))
        return (ParagraphItem) null;
      num = this.Range.Items.IndexOf((object) this.FieldSeparator.OwnerParagraph);
    }
    if (num != -1)
    {
      for (int index = num; index < this.Range.Items.Count; ++index)
      {
        Entity pictureFieldResult = this.Range.Items[index] as Entity;
        switch (pictureFieldResult)
        {
          case WPicture _:
            return pictureFieldResult as ParagraphItem;
          case WParagraph _:
            IEnumerator enumerator = (pictureFieldResult as WParagraph).ChildEntities.GetEnumerator();
            try
            {
              while (enumerator.MoveNext())
              {
                ParagraphItem current = (ParagraphItem) enumerator.Current;
                if (current is WPicture)
                  return current;
              }
              break;
            }
            finally
            {
              if (enumerator is IDisposable disposable)
                disposable.Dispose();
            }
        }
      }
    }
    return (ParagraphItem) null;
  }

  public void Update()
  {
    this.IsFieldRangeUpdated = false;
    if (!this.IsDeleteRevision)
    {
      switch (this.FieldType)
      {
        case FieldType.FieldRef:
          this.UpdateRefField();
          break;
        case FieldType.FieldSet:
          this.UpdateSetFields();
          break;
        case FieldType.FieldIf:
          (this as WIfField).UpdateIfField();
          break;
        case FieldType.FieldSequence:
          string empty1 = string.Empty;
          if (this.Document.SequenceFieldResults != null)
          {
            empty1 = this.GetSequenceFieldResult().ToString();
          }
          else
          {
            this.Document.SequenceFieldResults = new Dictionary<string, int>();
            for (int index = 0; index < this.Document.Fields.Count; ++index)
            {
              if (this.Document.Fields[index].FieldType == FieldType.FieldSequence)
              {
                if (!string.IsNullOrEmpty((this as WSeqField).BookmarkName) && this.IsBooKMarkSeqFieldUpdated(this.Document.Fields[index]))
                  return;
                this.Document.Fields[index].Update();
                if (string.IsNullOrEmpty((this as WSeqField).BookmarkName) && this.Document.Fields[index] == this)
                {
                  this.ClearSeqFieldInternalCollection();
                  return;
                }
              }
            }
            this.ClearSeqFieldInternalCollection();
          }
          this.UpdateSequenceFieldResult(empty1);
          break;
        case FieldType.FieldTitle:
          this.UpdateDocumentBuiltInProperties("Title");
          break;
        case FieldType.FieldSubject:
          this.UpdateDocumentBuiltInProperties("Subject");
          break;
        case FieldType.FieldAuthor:
          this.UpdateDocumentBuiltInProperties("Author");
          break;
        case FieldType.FieldComments:
          this.UpdateDocumentBuiltInProperties("Comments");
          break;
        case FieldType.FieldNumPages:
          this.UpdateNumberFormatResult(this.Document.PageCount.ToString());
          break;
        case FieldType.FieldDate:
        case FieldType.FieldTime:
          this.UpdateFieldResult(this.UpdateDateField(this.FormattingString, WordDocument.DisableDateTimeUpdating ? DateTime.MaxValue : DateTime.Now));
          break;
        case FieldType.FieldPage:
          if (DocumentLayouter.IsLayoutingHeaderFooter)
          {
            WSection ownerSection = this.GetOwnerSection((Entity) this.OwnerParagraph) as WSection;
            this.m_currentPageNumber = ownerSection.PageSetup.GetNumberFormatValue((byte) ownerSection.PageSetup.PageNumberStyle, DocumentLayouter.PageNumber);
            this.UpdateNumberFormatResult(this.m_currentPageNumber);
            break;
          }
          break;
        case FieldType.FieldFormula:
          string upper = this.FieldCode.Replace(" ", "").ToUpper();
          if (!upper.StartsWith("=SUM(ABOVE)") && !upper.StartsWith("=MAX(ABOVE)"))
          {
            this.UpdateFormulaField();
            break;
          }
          break;
        case FieldType.FieldDocVariable:
          string variable = this.m_doc.Variables[this.FieldValue.Replace('"'.ToString(), string.Empty)];
          if (variable != null)
          {
            string empty2 = string.Empty;
            this.RemoveMergeFormat(this.FieldCode, ref empty2);
            string text = variable;
            this.UpdateFieldResult(string.IsNullOrEmpty(empty2) ? this.UpdateTextFormat(text) : this.UpdateNumberFormat(text, empty2));
            break;
          }
          break;
        case FieldType.FieldSection:
          this.UpdateSectionField();
          break;
        case FieldType.FieldCompare:
          this.UpdateCompareField();
          break;
        case FieldType.FieldDocProperty:
          this.UpdateDocPropertyField();
          break;
        case FieldType.FieldUnknown:
          this.UpdateUnknownField();
          break;
      }
    }
    if (this.IsUpdated)
      return;
    this.IsUpdated = true;
  }

  private bool IsBooKMarkSeqFieldUpdated(WField field)
  {
    Bookmark byName = this.Document.Bookmarks.FindByName((this as WSeqField).BookmarkName);
    if (byName != null)
    {
      string str = byName.BookmarkEnd.NextSibling is WSeqField ? (byName.BookmarkEnd.NextSibling as Entity).GetHierarchicalIndex(string.Empty) : byName.BookmarkEnd.GetHierarchicalIndex(string.Empty);
      string hierarchicalIndex = field.GetHierarchicalIndex(string.Empty);
      if (!(str != hierarchicalIndex) || !this.CompareHierarchicalIndex(str, field.GetHierarchicalIndex(string.Empty)))
        return false;
      this.UpdateSequenceFieldResult(this.Document.SequenceFieldResults[(this as WSeqField).CaptionName].ToString());
      this.ClearSeqFieldInternalCollection();
      return true;
    }
    this.UpdateSequenceFieldResult(0.ToString());
    this.ClearSeqFieldInternalCollection();
    return true;
  }

  private void ClearSeqFieldInternalCollection()
  {
    this.Document.SequenceFieldResults.Clear();
    this.Document.SequenceFieldResults = (Dictionary<string, int>) null;
    if (this.IsUpdated)
      return;
    this.IsUpdated = true;
  }

  internal void UpdateSequenceFieldResult(string fieldResultNumber)
  {
    string empty = string.Empty;
    this.RemoveMergeFormat(this.FieldCode, ref empty);
    string text = fieldResultNumber;
    fieldResultNumber = string.IsNullOrEmpty(empty) ? this.UpdateTextFormat(text) : this.UpdateNumberFormat(text, empty);
    this.UpdateFieldResult(fieldResultNumber);
  }

  private int GetSequenceFieldResult()
  {
    if (!string.IsNullOrEmpty((this as WSeqField).BookmarkName))
      return 0;
    if (this.Document.SequenceFieldResults.ContainsKey((this as WSeqField).CaptionName))
    {
      if ((this as WSeqField).ResetNumber != -1)
        return this.Document.SequenceFieldResults[(this as WSeqField).CaptionName] = (this as WSeqField).ResetNumber;
      if ((this as WSeqField).ResetHeadingLevel != -1 && this.IsNeedToResetHeadingLevel())
        return this.Document.SequenceFieldResults[(this as WSeqField).CaptionName] = 1;
      Dictionary<string, int> sequenceFieldResults;
      string captionName;
      return (this as WSeqField).RepeatNearestNumber ? this.Document.SequenceFieldResults[(this as WSeqField).CaptionName] : ((sequenceFieldResults = this.Document.SequenceFieldResults)[captionName = (this as WSeqField).CaptionName] = sequenceFieldResults[captionName] + 1);
    }
    if ((this as WSeqField).RepeatNearestNumber)
      return 0;
    if ((this as WSeqField).ResetNumber != -1)
    {
      this.Document.SequenceFieldResults.Add((this as WSeqField).CaptionName, (this as WSeqField).ResetNumber);
      return (this as WSeqField).ResetNumber;
    }
    this.Document.SequenceFieldResults.Add((this as WSeqField).CaptionName, 1);
    return 1;
  }

  private bool IsNeedToResetHeadingLevel()
  {
    WParagraph paragrph = this.GetOwnerParagraphValue();
    int headingLevel1 = this.GetHeadingLevel(paragrph.ParaStyle as WParagraphStyle);
    if (headingLevel1 != -1 && headingLevel1 <= (this as WSeqField).ResetHeadingLevel)
      return true;
    WSeqField prevSeqField = this.GetPrevSeqField();
    if (prevSeqField == null)
      return false;
    WParagraph ownerParagraphValue = prevSeqField.GetOwnerParagraphValue();
    if (ownerParagraphValue == null)
      return false;
    for (Entity ownerShape = this.GetOwnerShape((Entity) ownerParagraphValue); ownerShape != null; ownerShape = this.GetOwnerShape((Entity) ownerParagraphValue))
      ownerParagraphValue = (ownerShape as ParagraphItem).GetOwnerParagraphValue();
    if (paragrph == ownerParagraphValue)
      return false;
    do
    {
      paragrph = this.GetPreviousParagraph(paragrph);
      if (paragrph == null)
        return false;
      IWParagraphStyle paraStyle = paragrph.ParaStyle;
      int headingLevel2 = this.GetHeadingLevel(paragrph.ParaStyle as WParagraphStyle);
      if (headingLevel2 != -1 && headingLevel2 <= (this as WSeqField).ResetHeadingLevel)
        return true;
    }
    while (paragrph != ownerParagraphValue);
    return false;
  }

  internal WParagraph GetPreviousParagraph(WParagraph paragrph)
  {
    IEntity previousSibling = paragrph.PreviousSibling;
    switch (previousSibling)
    {
      case WParagraph _:
        return previousSibling as WParagraph;
      case BlockContentControl _:
        return this.GetPreviousParagraphIsInSDTContent(previousSibling as BlockContentControl);
      case WTable _:
        return this.GetPreviousParagraphIsInTable(previousSibling as WTable);
      default:
        if (previousSibling == null && paragrph.IsInCell)
          return this.GetPreviousParagraphIsInCell(paragrph);
        return previousSibling == null ? this.GetPreviousParagraphIsInSection((IEntity) paragrph) : (WParagraph) null;
    }
  }

  private WParagraph GetPreviousParagraphIsInTable(WTable table)
  {
    IEntity widgetInner = (IEntity) table.LastCell.WidgetInnerCollection[table.LastCell.WidgetInnerCollection.Count - 1];
    switch (widgetInner)
    {
      case WParagraph _:
        return widgetInner as WParagraph;
      case BlockContentControl _:
        return this.GetPreviousParagraphIsInSDTContent(widgetInner as BlockContentControl);
      case WTable _:
        return this.GetPreviousParagraphIsInTable(widgetInner as WTable);
      default:
        return (WParagraph) null;
    }
  }

  private WParagraph GetPreviousParagraphIsInCell(WParagraph paragraph)
  {
    WTableCell ownerTableCell = paragraph.GetOwnerTableCell(paragraph.OwnerTextBody);
    WTableCell previousSibling1 = ownerTableCell.PreviousSibling as WTableCell;
    Entity paragraphIsInCell = previousSibling1 == null ? (!(ownerTableCell.OwnerRow.PreviousSibling is WTableRow previousSibling2) || previousSibling2.Cells.Count <= 0 ? ownerTableCell.OwnerRow.OwnerTable.PreviousSibling as Entity : previousSibling2.Cells[previousSibling2.Cells.Count - 1].Items.LastItem) : previousSibling1.Items.LastItem;
    switch (paragraphIsInCell)
    {
      case WParagraph _:
        return paragraphIsInCell as WParagraph;
      case BlockContentControl _:
        return this.GetPreviousParagraphIsInSDTContent(paragraphIsInCell as BlockContentControl);
      case WTable _:
        return this.GetPreviousParagraphIsInTable(paragraphIsInCell as WTable);
      case null:
        return this.GetPreviousParagraphIsInSection((IEntity) paragraph.GetOwnerTableCell(paragraph.OwnerTextBody).OwnerRow.OwnerTable);
      default:
        return (WParagraph) null;
    }
  }

  private WParagraph GetPreviousParagraphIsInSection(IEntity textBodyItem)
  {
    if (!(textBodyItem.Owner is WSection))
      return (WParagraph) null;
    if (!((textBodyItem.Owner as WSection).PreviousSibling is WSection previousSibling))
      return (WParagraph) null;
    IEntity lastItem = (IEntity) previousSibling.Body.Items.LastItem;
    switch (lastItem)
    {
      case WParagraph _:
        return lastItem as WParagraph;
      case BlockContentControl _:
        return this.GetPreviousParagraphIsInSDTContent(lastItem as BlockContentControl);
      case WTable _:
        return this.GetPreviousParagraphIsInTable(lastItem as WTable);
      default:
        return (WParagraph) null;
    }
  }

  private WParagraph GetPreviousParagraphIsInSDTContent(BlockContentControl sdtContent)
  {
    BodyItemCollection items = sdtContent.TextBody.Items;
    IEntity paragraphIsInSdtContent = (IEntity) items[items.Count - 1];
    switch (paragraphIsInSdtContent)
    {
      case WParagraph _:
        return paragraphIsInSdtContent as WParagraph;
      case BlockContentControl _:
        return this.GetPreviousParagraphIsInSDTContent(paragraphIsInSdtContent as BlockContentControl);
      case WTable _:
        return this.GetPreviousParagraphIsInTable(paragraphIsInSdtContent as WTable);
      default:
        return (WParagraph) null;
    }
  }

  private WSeqField GetPrevSeqField()
  {
    if (this.Document.Fields.Count > 1)
    {
      for (int index = this.Document.Fields.InnerList.IndexOf((object) this) - 1; index >= 0; --index)
      {
        if (this.Document.Fields[index] is WSeqField && (this as WSeqField).CaptionName == (this.Document.Fields[index] as WSeqField).CaptionName)
          return this.Document.Fields[index] as WSeqField;
      }
    }
    return (WSeqField) null;
  }

  private int GetHeadingLevel(WParagraphStyle paragraphStyle)
  {
    if (!paragraphStyle.BuiltInStyleIdentifier.ToString().ToLower().Contains("heading"))
      return -1;
    string str = paragraphStyle.BuiltInStyleIdentifier.ToString();
    if (str.Contains(","))
      str = str.Split(',')[0];
    if (str.Contains("+"))
      str = str.Split('+')[0];
    char[] charArray = str.ToCharArray();
    string s = "";
    foreach (char ch in charArray)
    {
      if (ch != '_')
      {
        int result;
        if (int.TryParse(ch.ToString((IFormatProvider) CultureInfo.InvariantCulture), out result))
          s += result.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      }
      else
        break;
    }
    return int.Parse(s);
  }

  private int ParseIntegerValue(string value)
  {
    int result1 = 0;
    double result2 = 0.0;
    if (double.TryParse(value, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result2))
    {
      if (value.Contains("."))
      {
        int length = value.IndexOf('.');
        if (length > 0)
          value = value.Substring(0, length);
        else if (length == 0)
          value = "0";
      }
      int.TryParse(value, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result1);
    }
    return result1;
  }

  private void UpdateUnknownField()
  {
    string fieldCode = this.FieldCode.Trim();
    if (!(fieldCode != ""))
      return;
    string str = fieldCode.Split(' ')[0];
    if (!(str.ToUpper() != "BIBLIOGRAPHY"))
      return;
    if (this.Document.Bookmarks.FindByName(str) != null)
    {
      BookmarksNavigator bookmarksNavigator = new BookmarksNavigator((IWordDocument) this.Document);
      bookmarksNavigator.MoveToBookmark(str);
      TextBodyPart bookmarkContent = bookmarksNavigator.GetBookmarkContent();
      List<Entity> fieldResult = new List<Entity>();
      foreach (TextBodyItem bodyItem in (CollectionImpl) bookmarkContent.BodyItems)
      {
        if (bodyItem is WParagraph)
        {
          Entity nextItem = (Entity) null;
          int itemIndex = 0;
          List<Entity> clonedParagraph = this.GetClonedParagraph((Entity) bodyItem, (string) null, ref nextItem, ref itemIndex);
          if (fieldResult.Count == 0 && clonedParagraph[0] is WParagraph)
          {
            foreach (ParagraphItem paragraphItem in (CollectionImpl) (clonedParagraph[0] as WParagraph).Items)
              fieldResult.Add((Entity) paragraphItem);
          }
          else
          {
            foreach (Entity entity in clonedParagraph)
              fieldResult.Add(entity);
          }
        }
        else if (bodyItem is WTable)
          fieldResult.Add(this.GetClonedTable((Entity) bodyItem));
      }
      this.UpdateFieldResult(fieldResult);
    }
    else if (this.IsFunction(fieldCode.ToLower()))
    {
      bool isFieldCodeStartWithCurrencySymbol = false;
      this.UpdateFieldResult(this.UpdateFunction(fieldCode, ref isFieldCodeStartWithCurrencySymbol));
    }
    else
      this.UpdateFieldResult("Error! Bookmark not defined.");
  }

  private string GetAutoNumFieldValue(WField field)
  {
    string autoNumFieldValue = string.Empty;
    if (field.FieldSeparator == null && field.FieldType == FieldType.FieldAutoNum)
      autoNumFieldValue = field.OwnerParagraph == null ? this.UpdateTextFormat(this.Document.Fields.GetAutoNumFieldResult(field)) + (object) this.GetSeperatorCode(field) : (!this.IsInValidTextBody(field) ? "Main Document Only" : this.UpdateTextFormat(this.Document.Fields.GetAutoNumFieldResult(field)) + (object) this.GetSeperatorCode(field));
    return autoNumFieldValue;
  }

  private char GetSeperatorCode(WField field)
  {
    char[] chArray = new char[0];
    string[] strArray = field.FieldCode.Split(new char[1]
    {
      ' '
    }, StringSplitOptions.RemoveEmptyEntries);
    return strArray.Length <= 4 ? '.' : strArray[4].ToCharArray()[0];
  }

  private bool IsInValidTextBody(WField field)
  {
    WParagraph ownerParagraph = field.OwnerParagraph;
    Entity ownerEntity = ownerParagraph.GetOwnerEntity();
    return !(ownerEntity is WTextBox) && !(ownerEntity is Shape) && !ownerParagraph.IsInHeaderFooter();
  }

  internal void UpdateSetFields()
  {
    string empty = string.Empty;
    string[] strArray = this.RemoveMergeFormat(this.FieldCode, ref empty).Split(new char[1]
    {
      ' '
    }, StringSplitOptions.RemoveEmptyEntries);
    if (strArray.Length != 3)
      return;
    string str = strArray[1];
    this.UpdateFieldResult(strArray[2]);
    this.RemoveBookMarksForSetFields(str);
    this.UpdateBookMarkForSetFields(str);
  }

  private void UpdateBookMarkForSetFields(string bkName)
  {
    int index = this.FieldSeparator.GetIndexInOwnerCollection() + 1;
    WParagraph ownerParagraph = this.OwnerParagraph;
    BookmarkStart bookmarkStart = new BookmarkStart((IWordDocument) this.m_doc, bkName);
    BookmarkEnd bookmarkEnd = new BookmarkEnd((IWordDocument) this.m_doc, bkName);
    ownerParagraph.ChildEntities.Insert(index, (IEntity) bookmarkStart);
    int inOwnerCollection = this.FieldEnd.GetIndexInOwnerCollection();
    ownerParagraph.ChildEntities.Insert(inOwnerCollection, (IEntity) bookmarkEnd);
  }

  private void RemoveBookMarksForSetFields(string bookMarkName)
  {
    Bookmark byName = this.Document.Bookmarks.FindByName(bookMarkName);
    if (byName == null)
      return;
    this.Document.Bookmarks.Remove(byName);
  }

  private void UpdateDocumentBuiltInProperties(string propertyName)
  {
    if (string.IsNullOrEmpty(this.FieldCode))
      return;
    string fieldCode1 = this.FieldCode;
    string fieldCode2 = this.RemoveMergeFormat(fieldCode1.TrimStart(' '));
    bool isHavingStringFormat = true;
    string str = this.RemoveStringFormat(fieldCode2, out isHavingStringFormat);
    char[] separator = new char[5]
    {
      '\n',
      '\v',
      '\r',
      '\t',
      ' '
    };
    str.TrimStart(separator);
    string[] strArray = str.Split(separator, StringSplitOptions.RemoveEmptyEntries);
    if (strArray.Length > 1 && !string.IsNullOrEmpty(strArray[1]))
    {
      strArray[1].TrimStart(separator);
      if (this.StartsWithExt(strArray[1], "\""))
        strArray = fieldCode1.TrimStart(' ').Split(new char[1]
        {
          '"'
        }, StringSplitOptions.RemoveEmptyEntries);
      else if (strArray[1].Contains("\""))
        strArray[1] = strArray[1].Split(new char[1]{ '"' }, StringSplitOptions.RemoveEmptyEntries)[0];
      if (Regex.Matches(strArray[1], Regex.Escape("\\")).Count >= 1)
      {
        int num = strArray[1].IndexOf('\\');
        string input = strArray[1].Substring(0, num + 1);
        strArray[1] = Regex.Replace(input, "\\\\", "") + strArray[1].Substring(num + 1);
      }
      switch (propertyName.ToLower())
      {
        case "author":
        case "title":
        case "subject":
        case "comments":
          strArray[1] = isHavingStringFormat ? this.UpdateTextFormat(strArray[1]) : strArray[1];
          this.UpdateFieldResult(strArray[1]);
          break;
      }
    }
    else
    {
      if (strArray.Length != 1)
        return;
      switch (propertyName.ToLower())
      {
        case "author":
          if (string.IsNullOrEmpty(this.Document.BuiltinDocumentProperties.Author))
            break;
          this.UpdateFieldResult(this.Document.BuiltinDocumentProperties.Author);
          break;
        case "title":
          if (string.IsNullOrEmpty(this.Document.BuiltinDocumentProperties.Title))
            break;
          this.UpdateFieldResult(this.Document.BuiltinDocumentProperties.Title);
          break;
        case "subject":
          if (string.IsNullOrEmpty(this.Document.BuiltinDocumentProperties.Subject))
            break;
          this.UpdateFieldResult(this.Document.BuiltinDocumentProperties.Subject);
          break;
        case "comments":
          if (string.IsNullOrEmpty(this.Document.BuiltinDocumentProperties.Comments))
            break;
          this.UpdateFieldResult(this.Document.BuiltinDocumentProperties.Comments);
          break;
      }
    }
  }

  private string RemoveStringFormat(string fieldCode, out bool isHavingStringFormat)
  {
    isHavingStringFormat = true;
    string[] strArray1 = fieldCode.Split(new string[1]
    {
      "\\*"
    }, StringSplitOptions.RemoveEmptyEntries);
    bool flag = true;
    for (int index1 = 1; index1 < strArray1.Length; ++index1)
    {
      string input = strArray1[index1].ToLower().Trim(' ');
      int count = Regex.Matches(input, Regex.Escape("\\")).Count;
      if (count > 0)
      {
        if (count == 1)
          input = Regex.Replace(input, "\\\\", "");
        if (count < 2 && !(input == "lower") && !(input == "upper") && !(input == "firstcap") && !(input == "caps"))
        {
          if (input.Contains("\""))
          {
            char[] separator = new char[5]
            {
              '\n',
              '\v',
              '\r',
              '\t',
              ' '
            };
            strArray1[0].TrimStart(separator);
            string[] strArray2 = strArray1[0].Split(separator, StringSplitOptions.RemoveEmptyEntries);
            for (int index2 = 1; index2 < strArray2.Length; ++index2)
            {
              if (strArray2[index2].Contains("\""))
              {
                flag = true;
                break;
              }
            }
            if (flag)
              continue;
          }
          flag = false;
          isHavingStringFormat = false;
          break;
        }
      }
    }
    return flag ? strArray1[0] : string.Empty;
  }

  internal string UpdateDateField(string text, DateTime currentDateTime)
  {
    if (!this.IsFieldRangeUpdated && this.Document.IsOpening)
    {
      this.Range.Items.Clear();
      this.UpdateFieldRange();
    }
    bool isMeridiemDefined = false;
    if (text.Contains("\\* MERGEFORMAT"))
      text = text.Remove(text.IndexOf("\\* MERGEFORMAT")).Trim();
    else if (text.Contains("\\* Mergeformat"))
      text = text.Remove(text.IndexOf("\\* Mergeformat")).Trim();
    if (text.LastIndexOf("\\*") != -1 && text.Substring(text.LastIndexOf('*') + 1).Trim().ToUpper() == "CHARFORMAT")
      text = text.Remove(text.LastIndexOf("\\*")).Trim();
    bool ordinalString = false;
    if (text.ToLower().Contains("ordinal"))
    {
      text = text.Remove(text.ToLower().IndexOf("ordinal")).Trim();
      ordinalString = true;
      if (text.ToLower().EndsWith("\\*"))
        text = text.Remove(text.ToLower().IndexOf("\\*")).Trim();
    }
    if (text.Contains("\\@"))
    {
      text = this.ParseSwitches(text, text.IndexOf("\\@"));
      text = this.RemoveMeridiem(text, out isMeridiemDefined);
      text = this.UpdateDateValue(text, currentDateTime);
      text = this.GetOrdinalstring(ordinalString, text);
      if (isMeridiemDefined)
        text = this.UpdateMeridiem(text, currentDateTime);
    }
    else
    {
      CultureInfo cultureInfo = this.CharacterFormat.HasValue(73) || this.CharacterFormat.BaseFormat.HasValue(73) ? this.GetCulture((LocaleIDs) this.CharacterFormat.LocaleIdASCII) : CultureInfo.CurrentCulture;
      text = currentDateTime.ToString(cultureInfo.DateTimeFormat.ShortDatePattern);
    }
    return text;
  }

  private string GetOrdinalstring(bool ordinalString, string text)
  {
    if (ordinalString)
    {
      text = text.Trim().ToString();
      for (int index = 0; index < text.Length; ++index)
      {
        if (char.IsLetter(text[index]))
        {
          ordinalString = false;
          break;
        }
      }
      if (ordinalString)
      {
        if (text.Length >= 2 && char.IsNumber(text[text.Length - 1]) && char.IsNumber(text[text.Length - 2]))
        {
          int num = int.Parse(text.Substring(text.Length - 2, 2));
          text = text.Remove(text.Length - 2, 2);
          text += this.Document.GetOrdinal(num, this.CharacterFormat);
        }
        else if (text.Length >= 1 && char.IsNumber(text[text.Length - 1]))
        {
          int num = int.Parse(text.Substring(text.Length - 1));
          text = text.Remove(text.Length - 1);
          text += this.Document.GetOrdinal(num, this.CharacterFormat);
        }
      }
    }
    return text;
  }

  internal bool UpdateNextIfField()
  {
    string empty1 = string.Empty;
    string text = this.RemoveText(this.RemoveMergeFormat(this.FieldCode, ref empty1), "nextif");
    string empty2 = string.Empty;
    return this.UpdateCondition(text, (List<int>) null, (string) null) == "1";
  }

  private void UpdateSectionField()
  {
    Entity entity = this.OwnerBase as Entity;
    int num = 1;
    while (!(entity is WSection) && entity.Owner != null)
      entity = entity.Owner;
    if (entity != null && entity is WSection)
      num += (entity as WSection).GetIndexInOwnerCollection();
    this.UpdateNumberFormatResult(num.ToString());
  }

  private bool IsPictureSwitchIsInSecondPlace()
  {
    string str = Regex.Replace(this.InternalFieldCode, "\\s", "");
    return str.Contains("\\#") && str.Contains("\\*") && str.IndexOf("\\#") > str.IndexOf("\\*");
  }

  internal void UpdateNumberFormatResult(string result)
  {
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string text1;
    if (Regex.Matches(this.InternalFieldCode, Regex.Escape("\\#")).Count > 1)
      text1 = "Error! Too many picture switches defined.";
    else if (this.IsPictureSwitchIsInSecondPlace())
    {
      text1 = "Error! Picture switch must be first formatting switch.";
    }
    else
    {
      this.RemoveMergeFormat(this.FieldCode, ref empty1);
      string text2 = result;
      text1 = string.IsNullOrEmpty(empty1) ? this.UpdateTextFormat(text2) : this.UpdateNumberFormat(text2, empty1);
    }
    this.UpdateFieldResult(text1);
  }

  private void UpdateDocPropertyField()
  {
    string str1 = this.m_fieldValue.Trim();
    string text1 = string.Empty;
    string str2 = "Error! Unknown document property name.";
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    foreach (string key in this.Document.CustomDocumentProperties.CustomHash.Keys)
    {
      string str3 = this.Document.CustomDocumentProperties.CustomHash[key].Value.ToString();
      dictionary.Add(key.ToLower(), str3);
    }
    if (dictionary.ContainsKey(str1.ToLower()))
    {
      text1 = dictionary[str1.ToLower()];
    }
    else
    {
      switch (str1.ToLower())
      {
        case "author":
          if (this.Document.BuiltinDocumentProperties.Author != null)
          {
            text1 = this.Document.BuiltinDocumentProperties.Author;
            break;
          }
          break;
        case "bytes":
          text1 = this.Document.BuiltinDocumentProperties.BytesCount.ToString();
          break;
        case "category":
          if (this.Document.BuiltinDocumentProperties.Category != null)
          {
            text1 = this.Document.BuiltinDocumentProperties.Category.ToString();
            break;
          }
          break;
        case "characters":
        case "characterswithspaces":
          text1 = this.Document.BuiltinDocumentProperties.CharCount.ToString();
          break;
        case "comments":
          text1 = this.Document.BuiltinDocumentProperties.Comments != null ? this.Document.BuiltinDocumentProperties.Comments : str2;
          break;
        case "company":
          if (this.Document.BuiltinDocumentProperties.Company != null)
          {
            text1 = this.Document.BuiltinDocumentProperties.Company;
            break;
          }
          break;
        case "createtime":
          DateTime createDate = this.Document.BuiltinDocumentProperties.CreateDate;
          text1 = this.Document.BuiltinDocumentProperties.CreateDate.ToString("g");
          break;
        case "keywords":
          text1 = this.Document.BuiltinDocumentProperties.Keywords != null ? this.Document.BuiltinDocumentProperties.Keywords : str2;
          break;
        case "lastprinted":
          DateTime lastPrinted = this.Document.BuiltinDocumentProperties.LastPrinted;
          text1 = this.Document.BuiltinDocumentProperties.LastPrinted.ToString("g");
          break;
        case "lastsavedby":
          if (this.Document.BuiltinDocumentProperties.LastAuthor != null)
          {
            text1 = this.Document.BuiltinDocumentProperties.LastAuthor;
            break;
          }
          break;
        case "lastsavedtime":
          DateTime lastSaveDate = this.Document.BuiltinDocumentProperties.LastSaveDate;
          text1 = this.Document.BuiltinDocumentProperties.LastSaveDate.ToString("g");
          break;
        case "lines":
          text1 = this.Document.BuiltinDocumentProperties.LinesCount.ToString();
          break;
        case "manager":
          if (this.Document.BuiltinDocumentProperties.Manager != null)
          {
            text1 = this.Document.BuiltinDocumentProperties.Manager;
            break;
          }
          break;
        case "nameofapplication":
          if (this.Document.BuiltinDocumentProperties.ApplicationName != null)
          {
            text1 = this.Document.BuiltinDocumentProperties.ApplicationName;
            break;
          }
          break;
        case "odmadocid":
          text1 = "Error! This property is only valid for ODMA documents.";
          break;
        case "pages":
          text1 = this.Document.BuiltinDocumentProperties.PageCount.ToString();
          break;
        case "paragraphs":
          text1 = this.Document.BuiltinDocumentProperties.ParagraphCount.ToString();
          break;
        case "revisionnumber":
          if (this.Document.BuiltinDocumentProperties.RevisionNumber != null)
          {
            text1 = this.Document.BuiltinDocumentProperties.RevisionNumber.ToString();
            break;
          }
          break;
        case "security":
          text1 = this.Document.BuiltinDocumentProperties.DocSecurity.ToString();
          break;
        case "subject":
          text1 = this.Document.BuiltinDocumentProperties.Subject != null ? this.Document.BuiltinDocumentProperties.Subject : str2;
          break;
        case "template":
          if (this.Document.BuiltinDocumentProperties.Template != null)
          {
            text1 = this.Document.BuiltinDocumentProperties.Template;
            break;
          }
          break;
        case "title":
          text1 = this.Document.BuiltinDocumentProperties.Title != null ? this.Document.BuiltinDocumentProperties.Title : str2;
          break;
        case "totaleditingtime":
          if (this.Document.BuiltinDocumentProperties.TotalEditingTime.TotalMinutes > 0.0)
          {
            text1 = this.Document.BuiltinDocumentProperties.TotalEditingTime.TotalMinutes.ToString();
            break;
          }
          break;
        case "words":
          text1 = this.Document.BuiltinDocumentProperties.WordCount.ToString();
          break;
        default:
          text1 = str2;
          break;
      }
    }
    if (text1 != str2)
    {
      string text2 = this.UpdateTextFormat(text1);
      string numberFormat = string.Empty;
      this.RemoveMergeFormat(this.FieldCode, ref numberFormat);
      if (numberFormat.Contains(" "))
        numberFormat = numberFormat.Remove(numberFormat.IndexOf(" "));
      text1 = this.UpdateNumberFormat(text2, numberFormat);
    }
    this.UpdateFieldResult(text1);
  }

  internal string UpdateTextFormat(string text)
  {
    string fieldCode = this.FieldCode;
    return fieldCode.Contains("\\") ? this.UpdateTextFormat(text, fieldCode.Substring(fieldCode.IndexOf("\\"))) : text;
  }

  internal string UpdateTextFormat(string text, string formattingString)
  {
    string[] strArray = formattingString.Trim().Split(new char[1]
    {
      '\\'
    }, StringSplitOptions.RemoveEmptyEntries);
    DateTime result1;
    bool flag1 = DateTime.TryParse(text, out result1);
    bool isNum = false;
    bool flag2 = false;
    bool flag3 = false;
    string text1 = string.Empty;
    string text2 = text;
    for (int index1 = 0; index1 < strArray.Length; ++index1)
    {
      string str1 = strArray[index1];
      if (str1.Length > 0)
      {
        string number = this.GetNumber(text2, ref isNum);
        string upper1 = WField.ClearStringFromOtherCharacters(str1).Trim().ToUpper();
        switch (str1[0])
        {
          case '*':
            switch (upper1)
            {
              case "UPPER":
                text = text.ToUpper();
                continue;
              case "LOWER":
                text = text.ToLower();
                continue;
              case "CAPS":
                text = this.GetCapsstring(text);
                continue;
              case "FIRSTCAP":
                if (this is WMergeField wmergeField && !string.IsNullOrEmpty(wmergeField.TextBefore))
                {
                  string textBefore = wmergeField.TextBefore;
                  if (char.IsLetter(textBefore[0]))
                  {
                    wmergeField.TextBefore = char.ToUpper(textBefore[0]).ToString() + textBefore.Substring(1);
                    continue;
                  }
                  int num = 0;
                  while (num < textBefore.Length && !char.IsLetter(textBefore[num]))
                    ++num;
                  if (num == textBefore.Length)
                  {
                    if (text != string.Empty && char.IsLetter(text[0]))
                    {
                      wmergeField.TextBefore = char.ToUpper(text[0]).ToString() + text.Substring(1);
                      continue;
                    }
                    continue;
                  }
                  wmergeField.TextBefore = textBefore.Substring(0, num) + (object) char.ToUpper(textBefore[num]) + textBefore.Substring(num + 1);
                  continue;
                }
                if (text != string.Empty)
                {
                  string upper2 = text[0].ToString().ToUpper();
                  text = text.Remove(0, 1);
                  text = upper2 + text;
                  continue;
                }
                continue;
              case "ROMAN":
                if (isNum && !flag1)
                {
                  text = this.Document.GetAsRoman(Convert.ToInt32(number));
                  if (Regex.Replace(str1, "\\s+", "")[1] == 'r')
                  {
                    text = text.ToLower();
                    continue;
                  }
                  continue;
                }
                if (flag1 && !flag3)
                {
                  if (flag2)
                  {
                    flag3 = true;
                    text = this.GetDateValue(text1, result1);
                    text = this.Document.GetAsRoman(Convert.ToInt32(text));
                    continue;
                  }
                  text = string.Empty;
                  continue;
                }
                continue;
              case "ALPHABETIC":
                if (isNum && !flag1)
                {
                  text = this.Document.GetAsLetter(Convert.ToInt32(number));
                  if (Regex.Replace(str1, "\\s+", "")[1] == 'a')
                  {
                    text = text.ToLower();
                    continue;
                  }
                  continue;
                }
                if (flag1 && !flag3)
                {
                  if (flag2)
                    return "Error! Number cannot be represented in specified format.";
                  text = string.Empty;
                  continue;
                }
                continue;
              case "HEX":
                if (isNum && !flag1)
                {
                  text = Convert.ToInt32(number).ToString("X");
                  continue;
                }
                if (flag1 && !flag3)
                {
                  if (flag2)
                  {
                    flag3 = true;
                    text = this.GetDateValue(text1, result1);
                    text = Convert.ToInt32(text).ToString("X");
                    continue;
                  }
                  text = "0";
                  continue;
                }
                continue;
              case "ORDINAL":
                if (isNum && !flag1)
                {
                  text = this.GetOrdinalstring(true, number);
                  continue;
                }
                if (flag1 && !flag3)
                {
                  if (flag2)
                  {
                    flag3 = true;
                    text = this.GetDateValue(text1, result1);
                    text = this.GetOrdinalstring(true, text);
                    continue;
                  }
                  text = "0TH";
                  continue;
                }
                continue;
              case "CARDTEXT":
                if (isNum && !flag1)
                {
                  text = this.Document.GetCardTextString(true, number.Contains(".") ? Math.Round(Convert.ToDecimal(number)).ToString() : number);
                  continue;
                }
                continue;
              case "ORDTEXT":
                if (isNum && !flag1)
                {
                  text = this.Document.GetOrdTextString(true, number);
                  continue;
                }
                continue;
              case "DOLLARTEXT":
                if (isNum && !flag1)
                {
                  text = this.Document.GetCardTextString(true, number.Contains(".") ? Decimal.ToInt32(Convert.ToDecimal(number)).ToString() : number);
                  double num = double.Parse(number);
                  string str2 = $"{num - (double) Decimal.Truncate(Convert.ToDecimal(num)):#.00}";
                  text = $"{text} and {str2.Substring(1)}/100";
                  continue;
                }
                continue;
              default:
                if (this != null && this.FieldType == FieldType.FieldFormula)
                {
                  text = "Error! Unknown switch argument.";
                  continue;
                }
                continue;
            }
          case '@':
            if (!(this is WMergeField) || !((this as WMergeField).DateFormat != string.Empty))
            {
              text1 = this.ParseSwitches(str1, str1.IndexOf("@"));
              if (DateTime.TryParse(text, out result1))
              {
                flag2 = true;
                text = this.UpdateDateValue(text1, result1);
                continue;
              }
              if (text1.Replace(' ', ' ') == "MMMM d, yyyy")
              {
                List<string> stringList = new List<string>();
                string empty = string.Empty;
                for (int index2 = 0; index2 < text.Length; ++index2)
                {
                  if (text[index2] == '-' && text[index2 - 1] != '-')
                  {
                    stringList.Add(empty);
                    empty = string.Empty;
                  }
                  else
                    empty += (string) (object) text[index2];
                  if (index2 == text.Length - 1)
                    stringList.Add(empty);
                }
                if (stringList.Count == 3)
                {
                  string s = string.Empty;
                  for (int index3 = 0; index3 < stringList.Count; ++index3)
                  {
                    int result2;
                    if (int.TryParse(stringList[index3], out result2))
                    {
                      s = index3 != 2 || result2 >= 0 ? s + result2.ToString() + "-" : s + DateTime.Now.Year.ToString();
                    }
                    else
                    {
                      s = string.Empty;
                      break;
                    }
                  }
                  if (DateTime.TryParse(s, out result1))
                  {
                    flag2 = true;
                    text = this.UpdateDateValue(text1, result1);
                    continue;
                  }
                  continue;
                }
                continue;
              }
              continue;
            }
            continue;
          default:
            continue;
        }
      }
    }
    return text;
  }

  private string GetCapsstring(string text)
  {
    char[] chArray = new char[2]{ ' ', '-' };
    text = text.ToLower();
    foreach (char separator in chArray)
      text = this.CapsConversion(text, separator);
    return text;
  }

  private string CapsConversion(string text, char separator)
  {
    string[] strArray = text.Split(separator);
    text = string.Empty;
    for (int index = 0; index < strArray.Length; ++index)
    {
      string str1 = strArray[index];
      if (str1 != string.Empty)
      {
        string upper = str1[0].ToString().ToUpper();
        string str2 = str1.Remove(0, 1);
        text = text + upper + str2;
      }
      text += (string) (object) separator;
    }
    text = text.TrimEnd(separator);
    return text;
  }

  private string GetNumberFormat(string numberFormat)
  {
    int num1 = numberFormat.LastIndexOf(NumberFormatInfo.InvariantInfo.CurrencyGroupSeparator);
    int num2 = numberFormat.LastIndexOf(NumberFormatInfo.InvariantInfo.CurrencyDecimalSeparator);
    if (num1 > -1 && num2 > -1 && num1 < num2 || NumberFormatInfo.InvariantInfo.CurrencyGroupSeparator == NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator && NumberFormatInfo.InvariantInfo.CurrencyDecimalSeparator == NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator)
      return numberFormat;
    string tempNumberFormat = numberFormat;
    if (NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator == ControlChar.NonBreakingSpace)
      tempNumberFormat = numberFormat.Replace(ControlChar.Space, ControlChar.NonBreakingSpace);
    else if (NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator == ControlChar.Space)
      tempNumberFormat = numberFormat.Replace(ControlChar.NonBreakingSpace, ControlChar.Space);
    int num3 = tempNumberFormat.LastIndexOf(NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator);
    int num4 = tempNumberFormat.LastIndexOf(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
    return num3 > -1 && num4 > -1 && num3 < num4 || num4 > -1 ? this.ChangeToInvariantFormat(tempNumberFormat) : numberFormat;
  }

  private string ChangeToInvariantFormat(string tempNumberFormat)
  {
    string invariantFormat = "";
    bool flag = false;
    for (int index = tempNumberFormat.Length - 1; index >= 0; --index)
    {
      string str = tempNumberFormat[index].ToString();
      if (!flag && str == NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator)
      {
        str = NumberFormatInfo.InvariantInfo.CurrencyDecimalSeparator;
        flag = true;
      }
      else if (flag && str == NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator)
        str = NumberFormatInfo.InvariantInfo.CurrencyGroupSeparator;
      invariantFormat = str + invariantFormat;
    }
    tempNumberFormat = (string) null;
    return invariantFormat;
  }

  private void UpdateRefField()
  {
    string[] strArray = this.InternalFieldCode.Split(new char[1]
    {
      ' '
    }, StringSplitOptions.RemoveEmptyEntries);
    if (strArray.Length == 1)
    {
      this.UpdateFieldResult("Error! No bookmark name given.");
    }
    else
    {
      bool flag = false;
      ReferenceKind referenceKind = ReferenceKind.ContentText;
      bool isHiddenBookmark = false;
      BookmarkStart bookmarkOfCrossRefField = this.GetBookmarkOfCrossRefField(ref isHiddenBookmark);
      if (bookmarkOfCrossRefField != null)
      {
        for (int index = 2; index < strArray.Length; ++index)
        {
          switch (strArray[index].ToLower())
          {
            case "\\p":
              if (flag)
                referenceKind = ReferenceKind.AboveBelow;
              flag = true;
              break;
            case "\\r":
              referenceKind = ReferenceKind.NumberRelativeContext;
              break;
            case "\\n":
              referenceKind = ReferenceKind.NumberNoContext;
              break;
            case "\\w":
              referenceKind = ReferenceKind.NumberFullContext;
              break;
          }
        }
        if (referenceKind == ReferenceKind.NumberRelativeContext || referenceKind == ReferenceKind.NumberNoContext || referenceKind == ReferenceKind.NumberFullContext)
          return;
        if (referenceKind == ReferenceKind.ContentText && !flag || (referenceKind == ReferenceKind.ContentText && flag || referenceKind == ReferenceKind.AboveBelow) && !this.CompareOwnerTextBody((Entity) bookmarkOfCrossRefField))
        {
          BookmarksNavigator bookmarksNavigator = new BookmarksNavigator((IWordDocument) this.Document);
          bookmarksNavigator.MoveToBookmark(bookmarkOfCrossRefField.Name);
          TextBodyPart bookmarkContent = bookmarksNavigator.GetBookmarkContent();
          List<Entity> fieldResult = new List<Entity>();
          foreach (TextBodyItem bodyItem in (CollectionImpl) bookmarkContent.BodyItems)
          {
            if (bodyItem is WParagraph)
            {
              Entity nextItem = (Entity) null;
              int itemIndex = 0;
              List<Entity> clonedParagraph = this.GetClonedParagraph((Entity) bodyItem, (string) null, ref nextItem, ref itemIndex);
              if (fieldResult.Count == 0 && clonedParagraph[0] is WParagraph)
              {
                foreach (ParagraphItem paragraphItem in (CollectionImpl) (clonedParagraph[0] as WParagraph).Items)
                {
                  int num = this.FieldEnd.Index - ((this.FieldSeparator == null ? this.Index : this.FieldSeparator.Index) + 1);
                  if ((clonedParagraph[0] as WParagraph).Items.Count > 0 && num == 1 && this.FormattingString.ToUpper().Contains("\\* MERGEFORMAT") && paragraphItem is WTextRange)
                  {
                    WCharacterFormat characterFormatValue = this.GetCharacterFormatValue();
                    if (characterFormatValue.HasValue(106))
                      characterFormatValue.PropertiesHash.Remove(106);
                    (paragraphItem as WTextRange).CharacterFormat.ImportContainer((FormatBase) characterFormatValue);
                    (paragraphItem as WTextRange).CharacterFormat.CopyProperties((FormatBase) characterFormatValue);
                  }
                  else if ((clonedParagraph[0] as WParagraph).Items.Count > 0 && num > 1 && this.FormattingString.ToUpper().Contains("\\* MERGEFORMAT") && paragraphItem is WTextRange)
                  {
                    WCharacterFormat characterFormatValue = this.GetCharacterFormatValue(paragraphItem.Index);
                    if (characterFormatValue.HasValue(106))
                      characterFormatValue.PropertiesHash.Remove(106);
                    (paragraphItem as WTextRange).CharacterFormat.ImportContainer((FormatBase) characterFormatValue);
                    (paragraphItem as WTextRange).CharacterFormat.CopyProperties((FormatBase) characterFormatValue);
                  }
                  fieldResult.Add((Entity) paragraphItem);
                }
              }
              else
              {
                foreach (Entity entity in clonedParagraph)
                  fieldResult.Add(entity);
              }
            }
            else if (bodyItem is WTable)
              fieldResult.Add(this.GetClonedTable((Entity) bodyItem));
          }
          this.UpdateFieldResult(fieldResult);
        }
        else
        {
          if ((referenceKind != ReferenceKind.ContentText || !flag) && referenceKind != ReferenceKind.AboveBelow || !this.CompareOwnerTextBody((Entity) bookmarkOfCrossRefField))
            return;
          this.UpdateFieldResult(this.GetPositionValue(bookmarkOfCrossRefField));
        }
      }
      else
      {
        if (bookmarkOfCrossRefField == null || this.StartsWithExt(bookmarkOfCrossRefField.Name, "_"))
          return;
        this.UpdateFieldResult("Error! Reference source not found.");
      }
    }
  }

  internal bool CompareOwnerTextBody(Entity bookmark)
  {
    WTextBody ownerTextBody1 = (bookmark as ParagraphItem).OwnerParagraph.OwnerTextBody;
    WTextBody ownerTextBody2 = this.OwnerParagraph.OwnerTextBody;
    Entity ownerEntity1 = (bookmark as ParagraphItem).OwnerParagraph.GetOwnerEntity();
    Entity ownerEntity2 = this.OwnerParagraph.GetOwnerEntity();
    if ((this.OwnerParagraph.IsInCell || ownerTextBody2.Owner is WSection || ownerTextBody2.Owner is BlockContentControl) && !(ownerTextBody2 is HeaderFooter) && ((bookmark as ParagraphItem).OwnerParagraph.IsInCell || ownerTextBody1.Owner is WSection || ownerTextBody1.Owner is BlockContentControl) && !(ownerTextBody1 is HeaderFooter) || ownerTextBody2 is HeaderFooter && ownerTextBody1 is HeaderFooter || ownerTextBody2.Owner is WFootnote && (ownerTextBody2.Owner as WFootnote).FootnoteType == FootnoteType.Footnote && ownerTextBody1.Owner is WFootnote && (ownerTextBody1.Owner as WFootnote).FootnoteType == FootnoteType.Footnote || ownerTextBody2.Owner is WFootnote && (ownerTextBody2.Owner as WFootnote).FootnoteType == FootnoteType.Endnote && ownerTextBody1.Owner is WFootnote && (ownerTextBody1.Owner as WFootnote).FootnoteType == FootnoteType.Endnote || (ownerEntity2 is Shape || ownerEntity2 is WTextBox) && (ownerEntity1 is Shape || ownerEntity1 is WTextBox))
      return true;
    return ownerTextBody2.Owner is WComment && ownerTextBody1.Owner is WComment;
  }

  private void UpdateFieldResult(List<Entity> fieldResult)
  {
    this.CheckFieldSeparator();
    this.RemovePreviousResult();
    WParagraph ownerParagraph = this.OwnerParagraph;
    if (this.FieldSeparator.OwnerParagraph == this.FieldEnd.OwnerParagraph)
      ownerParagraph.ChildEntities.Remove((IEntity) this.FieldEnd);
    int index1 = this.FieldSeparator.GetIndexInOwnerCollection() + 1;
    int index2 = this.FieldSeparator.OwnerParagraph.GetIndexInOwnerCollection() + 1;
    WTextBody ownerTextBody = this.FieldSeparator.OwnerParagraph.OwnerTextBody;
    for (int index3 = 0; index3 < fieldResult.Count; ++index3)
    {
      if (fieldResult[index3] is ParagraphItem)
      {
        this.FieldSeparator.OwnerParagraph.Items.Insert(index1, (IEntity) fieldResult[index3]);
        ++index1;
        if (index3 == fieldResult.Count - 1 && this.FieldSeparator.OwnerParagraph != this.FieldEnd.OwnerParagraph)
          this.FieldSeparator.OwnerParagraph.Items.Insert(index1, (IEntity) this.FieldEnd);
      }
      else
      {
        if (this.FieldEnd.OwnerParagraph == null)
          ownerTextBody.Items.Add((IEntity) new WParagraph((IWordDocument) this.Document)
          {
            ChildEntities = {
              (IEntity) this.FieldEnd
            }
          });
        if (index3 == fieldResult.Count - 1 && fieldResult[index3] is WParagraph)
        {
          if (this.FieldSeparator.OwnerParagraph == this.FieldEnd.OwnerParagraph)
          {
            (ownerTextBody.Items[index2] as WParagraph).Items.Add((IEntity) this.FieldEnd);
          }
          else
          {
            int count = (fieldResult[index3] as WParagraph).ChildEntities.Count;
            for (int index4 = 0; index4 < count; ++index4)
              this.FieldEnd.OwnerParagraph.ChildEntities.Insert(0, (IEntity) (fieldResult[index3] as WParagraph).ChildEntities[(fieldResult[index3] as WParagraph).ChildEntities.Count - 1]);
          }
        }
        else
          ownerTextBody.Items.Insert(index2, (IEntity) fieldResult[index3]);
        ++index2;
      }
    }
  }

  internal string GetHierarchicalIndex(Entity entity)
  {
    string hierarchicalIndex = entity.GetIndexInOwnerCollection().ToString() + ";";
    Entity owner = entity.Owner;
    while (owner != this.Document)
    {
      string str1;
      switch (owner)
      {
        case WParagraph _:
          str1 = "WP";
          break;
        case WTable _:
          str1 = "WT";
          break;
        case WTableRow _:
          str1 = "WTR";
          break;
        case WTableCell _:
          str1 = "WTC";
          break;
        case WSection _:
          str1 = "WS";
          break;
        case WTextBody _:
          str1 = "WTB";
          break;
        case WComment _:
          str1 = "WC";
          break;
        case WFootnote _:
          str1 = (owner as WFootnote).FootnoteType == FootnoteType.Footnote ? "WFF" : "WFE";
          break;
        case BlockContentControl _:
          str1 = "SDTB";
          break;
        default:
          str1 = "";
          break;
      }
      string str2 = owner.GetIndexInOwnerCollection().ToString();
      string str3 = hierarchicalIndex;
      hierarchicalIndex = $"{str1}{str2};{str3}";
      owner = owner.Owner;
      if (owner is BlockContentControl)
        owner = owner.Owner;
      if (owner == null || owner is WordDocument || owner is Shape || owner is WTextBox)
        break;
    }
    return hierarchicalIndex;
  }

  internal string GetPositionValue(BookmarkStart bkStart)
  {
    bool flag = false;
    WParagraph ownerParagraph1 = bkStart.OwnerParagraph;
    WTextBody entityOwnerTextBody1 = this.Document.GetEntityOwnerTextBody(ownerParagraph1);
    WParagraph ownerParagraph2 = this.OwnerParagraph;
    WTextBody entityOwnerTextBody2 = this.Document.GetEntityOwnerTextBody(ownerParagraph2);
    if (entityOwnerTextBody2 is HeaderFooter && entityOwnerTextBody1 is HeaderFooter)
    {
      if (entityOwnerTextBody1 == entityOwnerTextBody2)
      {
        if (ownerParagraph1 == ownerParagraph2)
        {
          if (bkStart.GetIndexInOwnerCollection() < this.GetIndexInOwnerCollection())
            flag = true;
        }
        else if (ownerParagraph1.GetIndexInOwnerCollection() < ownerParagraph2.GetIndexInOwnerCollection())
          flag = true;
      }
      else if ((entityOwnerTextBody1 as HeaderFooter).Type == HeaderFooterType.FirstPageHeader || (entityOwnerTextBody1 as HeaderFooter).Type == HeaderFooterType.OddHeader || (entityOwnerTextBody1 as HeaderFooter).Type == HeaderFooterType.EvenHeader)
        flag = true;
    }
    else if (entityOwnerTextBody2.Owner is WComment && entityOwnerTextBody1.Owner is WComment)
      flag = this.CompareHierarchicalIndex(this.GetHierarchicalIndex((Entity) bkStart), this.GetHierarchicalIndex((Entity) this));
    else if ((entityOwnerTextBody2.Owner is Shape || entityOwnerTextBody2.Owner is WTextBox) && (entityOwnerTextBody1.Owner is Shape || entityOwnerTextBody1.Owner is WTextBox))
    {
      if (entityOwnerTextBody2 == entityOwnerTextBody1)
        flag = this.CompareHierarchicalIndex(this.GetHierarchicalIndex((Entity) bkStart), this.GetHierarchicalIndex((Entity) this));
      else if ((entityOwnerTextBody2.Owner as ParagraphItem).OwnerParagraph == (entityOwnerTextBody1.Owner as ParagraphItem).OwnerParagraph)
      {
        if ((entityOwnerTextBody1.Owner as ParagraphItem).GetIndexInOwnerCollection() < (entityOwnerTextBody2.Owner as ParagraphItem).GetIndexInOwnerCollection())
          flag = true;
      }
      else
        flag = this.CompareHierarchicalIndex(this.GetHierarchicalIndex((Entity) (entityOwnerTextBody1.Owner as ParagraphItem).OwnerParagraph), this.GetHierarchicalIndex((Entity) (entityOwnerTextBody2.Owner as ParagraphItem).OwnerParagraph));
    }
    else
      flag = !(entityOwnerTextBody2.Owner is WFootnote) || (entityOwnerTextBody2.Owner as WFootnote).FootnoteType != FootnoteType.Footnote || !(entityOwnerTextBody1.Owner is WFootnote) || (entityOwnerTextBody1.Owner as WFootnote).FootnoteType != FootnoteType.Footnote ? (!(entityOwnerTextBody2.Owner is WFootnote) || (entityOwnerTextBody2.Owner as WFootnote).FootnoteType != FootnoteType.Endnote || !(entityOwnerTextBody1.Owner is WFootnote) || (entityOwnerTextBody1.Owner as WFootnote).FootnoteType != FootnoteType.Endnote ? this.CompareHierarchicalIndex(this.GetHierarchicalIndex((Entity) bkStart), this.GetHierarchicalIndex((Entity) this)) : this.CompareHierarchicalIndex(this.GetHierarchicalIndex((Entity) bkStart), this.GetHierarchicalIndex((Entity) this))) : this.CompareHierarchicalIndex(this.GetHierarchicalIndex((Entity) bkStart), this.GetHierarchicalIndex((Entity) this));
    return !flag ? "below" : "above";
  }

  internal bool CompareHierarchicalIndex(string value1, string value2)
  {
    string[] strArray1 = value1.Split(new char[1]{ ';' }, StringSplitOptions.RemoveEmptyEntries);
    string[] strArray2 = value2.Split(new char[1]{ ';' }, StringSplitOptions.RemoveEmptyEntries);
    int num = strArray1.Length < strArray2.Length ? strArray1.Length : strArray2.Length;
    for (int index = 0; index < num; ++index)
    {
      if (strArray1[index] != strArray2[index])
      {
        if (int.Parse(Regex.Match(strArray1[index], "\\d+").Value) < int.Parse(Regex.Match(strArray2[index], "\\d+").Value))
          return true;
        break;
      }
    }
    return false;
  }

  private string GetDateValue(string text, DateTime date)
  {
    int count = 0;
    int dateValue = 0;
    int index = 0;
    while (index < text.Length)
    {
      switch (text[index])
      {
        case '\'':
        case '\\':
          ++index;
          continue;
        case 'D':
        case 'd':
          while (index < text.Length && (text[index] == 'd' || text[index] == 'D'))
          {
            ++index;
            ++count;
          }
          dateValue = this.UpdateCustomDay(dateValue, date, count);
          count = 0;
          continue;
        case 'M':
          while (index < text.Length && text[index] == 'M')
          {
            ++index;
            ++count;
          }
          dateValue = this.UpdateCustomMonth(dateValue, date, count);
          count = 0;
          continue;
        case 'Y':
        case 'y':
          while (index < text.Length && (text[index] == 'y' || text[index] == 'Y'))
          {
            ++index;
            ++count;
          }
          dateValue = this.UpdateCustomYear(dateValue, date, count);
          count = 0;
          continue;
        default:
          ++index;
          continue;
      }
    }
    return dateValue.ToString();
  }

  private int UpdateCustomDay(int dateValue, DateTime currentDateTime, int count)
  {
    int num;
    switch (count)
    {
      case 1:
      case 2:
        num = currentDateTime.Day;
        break;
      default:
        num = 0;
        break;
    }
    dateValue += num;
    return num;
  }

  private int UpdateCustomMonth(int dateValue, DateTime currentDateTime, int count)
  {
    int num = 0;
    switch (count)
    {
      case 1:
      case 2:
        num = currentDateTime.Month;
        break;
    }
    dateValue += num;
    return dateValue;
  }

  private int UpdateCustomYear(int dateValue, DateTime currentDateTime, int count)
  {
    int num;
    switch (count)
    {
      case 1:
      case 2:
        num = currentDateTime.Year % 100;
        break;
      default:
        num = currentDateTime.Year;
        break;
    }
    dateValue += num;
    return dateValue;
  }

  private string GetNumber(string text, ref bool isNum)
  {
    isNum = int.TryParse(text, out int _);
    if (isNum)
      return text;
    string numberAlphabet = this.GetNumberAlphabet(text);
    if (!(numberAlphabet != string.Empty))
      return text;
    isNum = true;
    return numberAlphabet;
  }

  private string GetNumberAlphabet(string text)
  {
    string empty = string.Empty;
    foreach (char c in text)
    {
      if (!char.IsLetter(c))
      {
        if (char.IsNumber(c) || c == '.')
          empty += (string) (object) c;
      }
      else
        break;
    }
    return empty;
  }

  private string TrimBeginingText(string fieldCode)
  {
    while (fieldCode != string.Empty && (this.StartsWithExt(fieldCode, ControlChar.DoubleQuoteString) || this.StartsWithExt(fieldCode, ControlChar.LeftDoubleQuoteString) || this.StartsWithExt(fieldCode, ControlChar.RightDoubleQuoteString) || this.StartsWithExt(fieldCode, ControlChar.DoubleLowQuoteString)))
      fieldCode = fieldCode.Remove(0, 1);
    return fieldCode;
  }

  private string TrimEndText(string fieldCode)
  {
    while (fieldCode != string.Empty && (fieldCode.EndsWith(ControlChar.DoubleQuoteString) || fieldCode.EndsWith(ControlChar.LeftDoubleQuoteString) || fieldCode.EndsWith(ControlChar.RightDoubleQuoteString) || fieldCode.EndsWith(ControlChar.DoubleLowQuoteString)))
      fieldCode = fieldCode.Substring(0, fieldCode.Length - 1);
    return fieldCode;
  }

  protected string RemoveText(string text, string textToRevome)
  {
    return this.RemoveText(text, textToRevome, true);
  }

  private string RemoveText(string text, string textToRevome, bool isTrim)
  {
    if (text.StartsWith(textToRevome, StringComparison.InvariantCultureIgnoreCase))
      text = text.Substring(textToRevome.Length, text.Length - textToRevome.Length);
    if (isTrim)
      text = text.Trim();
    return text;
  }

  protected List<string> SplitIfArguments(
    string text,
    ref List<int> operatorIndexForDoubleQuotes,
    ref string operatorValue)
  {
    List<string> arguments = new List<string>();
    List<string> operators = new List<string>((IEnumerable<string>) new string[6]
    {
      "<=",
      ">=",
      "<>",
      "=",
      "<",
      ">"
    });
    string empty = string.Empty;
    bool isOperator = false;
    bool isDoubleQuote = false;
    List<int> operatorIndex = this.GetOperatorIndex(operators, text, ref isDoubleQuote);
    if (isDoubleQuote)
    {
      operatorIndexForDoubleQuotes = new List<int>((IEnumerable<int>) operatorIndex);
      operatorValue = this.GetOperatorValue(operatorIndex, text);
    }
    if (operatorIndex.Count == 0)
      return arguments;
    try
    {
      while (text != string.Empty)
      {
        int tableStart = text.IndexOf('\u0013');
        if (this.StartsWithExt(text, "\""))
        {
          int length = text.Length;
          this.SplitFieldCode(tableStart, ref text, ref empty);
          this.UpdateOperatorIndex(operatorIndex, length - text.Length);
          if (isDoubleQuote && !string.IsNullOrEmpty(empty) && !isOperator && arguments.Count == 0)
            this.UpdateOperatorIndexForDoubleQuote(operatorIndexForDoubleQuotes, empty.Length);
        }
        else
          this.SplitFieldCode(operators, arguments, operatorIndex, isOperator, ref text, ref empty);
        if (isOperator && arguments.Count == 0)
        {
          arguments.Insert(0, empty);
          empty = string.Empty;
          isOperator = false;
        }
        else if (arguments.Count > 0)
        {
          arguments.Add(empty);
          empty = string.Empty;
          if (!this.StartsWithExt(text, "\"") && this.StartsWithExt(text.TrimStart(), "\""))
            text = text.TrimStart();
          arguments.Add(text.Trim('"'));
          text = string.Empty;
        }
        if (arguments.Count == 0)
          isOperator = this.IsOperator(operators, ref text, ref empty);
      }
    }
    catch
    {
      while (arguments.Count < 3)
        arguments.Add(string.Empty);
    }
    return arguments;
  }

  private void SplitFieldCode(int tableStart, ref string text, ref string condition)
  {
    text = text.Substring(text.IndexOf("\"") + 1);
    tableStart = text.IndexOf('\u0013');
    if (tableStart >= 0 && tableStart < text.IndexOf("\""))
      condition += this.GetTextInTable(ref text);
    condition += text.Substring(0, text.IndexOf("\""));
    text = text.Substring(text.IndexOf("\"") + 1);
  }

  private void SplitFieldCode(
    List<string> operators,
    List<string> arguments,
    List<int> operatorIndex,
    bool isOperator,
    ref string text,
    ref string condition)
  {
    for (int index = 0; index < operators.Count && arguments.Count == 0; ++index)
    {
      if (text.Contains(operators[index]) && operatorIndex[0] == text.IndexOf(operators[index]))
      {
        condition += text.Substring(0, text.IndexOf(operators[index])).Trim(ControlChar.SpaceChar);
        text = text.Substring(text.IndexOf(operators[index])).Trim(ControlChar.SpaceChar);
        break;
      }
    }
    if (!isOperator && arguments.Count <= 0)
      return;
    condition += text.Substring(0, text.IndexOf(" ")).Trim(ControlChar.SpaceChar);
    text = text.Substring(text.IndexOf(" ")).Trim(ControlChar.SpaceChar);
  }

  private string GetOperatorValue(List<int> operatorIndex, string text)
  {
    if (operatorIndex == null || operatorIndex.Count <= 0)
      return (string) null;
    string empty = string.Empty;
    foreach (int index in operatorIndex)
      empty += (string) (object) text[index];
    return empty;
  }

  private void UpdateOperatorIndex(List<int> operatorsIndex, int count)
  {
    for (int index = 0; index < operatorsIndex.Count; ++index)
      operatorsIndex[index] -= count;
  }

  private void UpdateOperatorIndexForDoubleQuote(List<int> operatorsIndex, int count)
  {
    for (int index = 0; index < operatorsIndex.Count; ++index)
      operatorsIndex[index] = count + index;
  }

  private bool IsOperator(List<string> operators, ref string text, ref string condition)
  {
    for (int index = 0; index < operators.Count; ++index)
    {
      if (this.StartsWithExt(text, operators[index]))
      {
        condition += operators[index];
        text = text.Substring(text.IndexOf(operators[index]) + operators[index].Length).Trim();
        return true;
      }
    }
    return false;
  }

  private List<int> GetOperatorIndex(List<string> operators, string text, ref bool isDoubleQuote)
  {
    int num1 = -1;
    int num2 = -1;
    if (this.StartsWithExt(text, " "))
      text = text.TrimStart(' ');
    if (this.StartsWithExt(text, ControlChar.DoubleQuoteString))
    {
      isDoubleQuote = true;
      text.Split('"');
      num1 = text.IndexOf(ControlChar.DoubleQuote);
      for (int index = num1 + 1; index < text.Length; ++index)
      {
        if ((int) text[index] == (int) ControlChar.DoubleQuote)
        {
          num2 = index;
          break;
        }
      }
    }
    List<int> operatorIndex = new List<int>();
    for (int index = 0; index < operators.Count; ++index)
    {
      if (text.Contains(operators[index]) && (num1 == -1 || text.IndexOf(operators[index]) > num2) && !operatorIndex.Contains(text.IndexOf(operators[index])))
        operatorIndex.Add(text.IndexOf(operators[index]));
    }
    operatorIndex.Sort();
    return operatorIndex;
  }

  private string GetTextInTable(ref string text)
  {
    string empty = string.Empty;
    for (int index1 = text.IndexOf('\u0013'); index1 >= 0 && index1 < text.IndexOf("\""); index1 = text.IndexOf('\u0013'))
    {
      string str1 = text.Substring(0, text.IndexOf('\u0015') + 1);
      empty += str1;
      text = text.Substring(text.IndexOf('\u0015') + 1);
      string str2 = str1;
      char[] chArray = new char[1]{ '\u0013' };
      for (int index2 = str2.Split(chArray).Length - 2; index2 > 0; --index2)
      {
        empty += text.Substring(0, text.IndexOf('\u0015') + 1);
        text = text.Substring(text.IndexOf('\u0015') + 1);
      }
    }
    return empty;
  }

  protected string UpdateCondition(string text, List<int> operatorIndex, string operatorValue)
  {
    List<string> stringList = new List<string>((IEnumerable<string>) new string[6]
    {
      "<=",
      ">=",
      "<>",
      "=",
      "<",
      ">"
    });
    string[] strArray = new string[2];
    bool isOperatorAtIndex = false;
    if (!string.IsNullOrEmpty(operatorValue) && operatorIndex != null && operatorIndex[0] > 0)
      isOperatorAtIndex = this.ValidateOperatorIndex(text, operatorValue, operatorIndex);
    if (isOperatorAtIndex)
    {
      strArray[0] = text.Substring(0, operatorIndex[0]);
      strArray[1] = text.Substring(operatorIndex[operatorIndex.Count - 1] + 1);
    }
    else
      strArray = text.Split(stringList.ToArray(), StringSplitOptions.RemoveEmptyEntries);
    if (strArray.Length > 1)
    {
      strArray[0] = strArray[0].Trim('"');
      strArray[1] = strArray[1].Trim('"');
      if (this.FieldType != FieldType.FieldIf)
      {
        strArray[0] = strArray[0].Trim();
        strArray[1] = strArray[1].Trim();
      }
    }
    else if (strArray.Length == 0)
      strArray = new string[2]{ string.Empty, string.Empty };
    else if (!string.IsNullOrEmpty(operatorValue) && text.IndexOf(operatorValue) == 0)
      strArray = new string[2]{ string.Empty, strArray[0] };
    else
      strArray = new string[2]{ strArray[0], string.Empty };
    string str = "1";
    if (strArray.Length > 1)
    {
      double result1;
      if (!double.TryParse(strArray[0], out result1))
      {
        try
        {
          bool isFieldCodeStartWithCurrencySymbol = false;
          strArray[0] = this.UpdateFormula(strArray[0].Trim(), ref isFieldCodeStartWithCurrencySymbol);
        }
        catch
        {
        }
      }
      double result2;
      if (!double.TryParse(strArray[1], out result2))
      {
        try
        {
          bool isFieldCodeStartWithCurrencySymbol = false;
          strArray[1] = this.UpdateFormula(strArray[1].Trim(), ref isFieldCodeStartWithCurrencySymbol);
        }
        catch
        {
        }
      }
      if (double.TryParse(strArray[0], out result1) && double.TryParse(strArray[1], out result2))
      {
        foreach (string operation in stringList)
        {
          if (text.Contains(operation))
          {
            str = this.CompareExpression(result1, result2, operation);
            break;
          }
        }
      }
      else if (this.HasOperatorinText(isOperatorAtIndex, operatorValue, "=", text))
      {
        switch (strArray[1].Trim())
        {
          case "?":
            str = strArray[0].Trim().Length == 1 ? "1" : "0";
            break;
          case "*":
            str = strArray[0].Trim().Length > 0 ? "1" : "0";
            break;
          default:
            str = strArray[0] == strArray[1] ? "1" : "0";
            break;
        }
      }
      else if (this.HasOperatorinText(isOperatorAtIndex, operatorValue, "<>", text))
      {
        switch (strArray[1].Trim())
        {
          case "?":
            str = strArray[0].Trim().Length != 1 ? "1" : "0";
            break;
          case "*":
            str = strArray[0].Trim().Length > 0 ? "0" : "1";
            break;
          default:
            str = strArray[0] != strArray[1] ? "1" : "0";
            break;
        }
      }
      else if (strArray[0] == string.Empty && operatorValue != null && operatorValue.Contains(">"))
        str = "0";
    }
    return str;
  }

  private bool HasOperatorinText(
    bool isOperatorAtIndex,
    string operatorValue,
    string expectedoperator,
    string text)
  {
    return !isOperatorAtIndex ? text.Contains(expectedoperator) : expectedoperator == operatorValue;
  }

  private bool ValidateOperatorIndex(string text, string expectedOperator, List<int> operatorIndex)
  {
    if (operatorIndex.Count <= 0 || operatorIndex[operatorIndex.Count - 1] >= text.Length)
      return false;
    string empty = string.Empty;
    foreach (int index in operatorIndex)
      empty += (string) (object) text[index];
    return expectedOperator == empty;
  }

  private void UpdateCompareField()
  {
    string empty1 = string.Empty;
    string text1 = this.RemoveText(this.RemoveMergeFormat(this.FieldCode, ref empty1), "compare");
    string empty2 = string.Empty;
    string text2;
    try
    {
      text2 = this.UpdateCondition(text1, (List<int>) null, (string) null);
    }
    catch
    {
      text2 = "Error! Unknown op code for conditional.";
    }
    this.UpdateFieldResult(text2);
  }

  private string CompareExpression(double operand1, double operand2, string operation)
  {
    double num = 0.0;
    switch (operation)
    {
      case "=":
        num = operand1 == operand2 ? 1.0 : 0.0;
        break;
      case "<":
        num = operand1 < operand2 ? 1.0 : 0.0;
        break;
      case "<=":
        num = operand1 <= operand2 ? 1.0 : 0.0;
        break;
      case ">":
        num = operand1 > operand2 ? 1.0 : 0.0;
        break;
      case ">=":
        num = operand1 >= operand2 ? 1.0 : 0.0;
        break;
      case "<>":
        num = operand1 != operand2 ? 1.0 : 0.0;
        break;
    }
    return num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
  }

  private void UpdateFormulaField()
  {
    string fieldCode1 = this.FieldCode;
    string formattingString = string.Empty;
    string empty1 = string.Empty;
    if (fieldCode1.Contains("\\*"))
    {
      int startIndex = fieldCode1.IndexOf("\\*");
      string fieldCode2 = fieldCode1.Substring(startIndex);
      fieldCode1 = fieldCode1.Remove(startIndex);
      formattingString = this.RemoveMergeFormat(fieldCode2, ref empty1);
    }
    string text1 = this.RemoveMergeFormat(fieldCode1, ref empty1);
    if (this.StartsWithExt(text1, "="))
      text1 = text1.Substring(1).Trim();
    bool isFieldCodeStartWithCurrencySymbol = false;
    string str = this.RemoveCurrencySymbol(text1, ref isFieldCodeStartWithCurrencySymbol);
    string empty2 = string.Empty;
    string text2;
    if (!double.TryParse(str, out double _))
    {
      try
      {
        text2 = this.UpdateFormula(str, ref isFieldCodeStartWithCurrencySymbol);
      }
      catch (Exception ex)
      {
        text2 = ex.Message;
      }
    }
    else
      text2 = str;
    if (isFieldCodeStartWithCurrencySymbol && string.IsNullOrEmpty(formattingString) && string.IsNullOrEmpty(empty1))
      text2 = "$" + this.UpdateNumberFormat(text2, "0.00");
    this.UpdateFieldResult(!string.IsNullOrEmpty(formattingString) ? (this.FieldCode.IndexOf("\\*") <= this.FieldCode.IndexOf("\\#") ? "Error! Picture switch must be first formatting switch." : this.UpdateTextFormat(text2, formattingString)) : this.UpdateNumberFormat(text2, empty1));
  }

  private string RemoveCurrencySymbol(string text, ref bool isFieldCodeStartWithCurrencySymbol)
  {
    char[] chArray = new char[1]{ '$' };
    foreach (char ch in chArray)
    {
      if (this.StartsWithExt(text, ch.ToString()))
      {
        text = text.Substring(1).Trim();
        isFieldCodeStartWithCurrencySymbol = true;
        break;
      }
    }
    return text;
  }

  internal string UpdateNumberFormat(string text, string numberFormat)
  {
    if (numberFormat.Contains("\\* MERGEFORMAT"))
      numberFormat = numberFormat.Remove(numberFormat.IndexOf("\\* MERGEFORMAT")).Trim();
    else if (numberFormat.Contains("\\* Mergeformat"))
      numberFormat = numberFormat.Remove(numberFormat.IndexOf("\\* Mergeformat")).Trim();
    double result;
    if (double.TryParse(text, out result) && !string.IsNullOrEmpty(numberFormat))
    {
      string numberFormat1 = numberFormat;
      if (result == 0.0 && numberFormat.Contains(";"))
      {
        string text1 = numberFormat;
        int num = numberFormat1.IndexOf(";");
        if (num > 0)
          text1 = text1.Substring(num + 1, numberFormat1.Length - (num + 1));
        if (this.StartsWithExt(text1, '-'.ToString()))
          return string.Empty;
      }
      if (numberFormat1.Contains("#"))
        numberFormat1 = numberFormat1.Replace("#", "0");
      if (numberFormat1 != string.Empty)
      {
        if (numberFormat1.Contains("%"))
          result /= 100.0;
        string numberFormat2 = this.GetNumberFormat(numberFormat1);
        if (this.IsNeedToFormatFieldResult(numberFormat2))
        {
          text = this.GetFormattedString(numberFormat2, text);
          if (string.IsNullOrEmpty(text))
            text = result.ToString(numberFormat2, (IFormatProvider) CultureInfo.CurrentCulture);
        }
        else
          text = result.ToString(numberFormat2, (IFormatProvider) CultureInfo.CurrentCulture);
      }
      if (numberFormat.Contains("#") && !string.IsNullOrEmpty(text))
      {
        string str = numberFormat;
        if (result < 0.0 && str.Contains(";"))
          this.CheckNumberFormatForNegativeValue(ref numberFormat, text);
        int num1 = numberFormat.Length;
        if (numberFormat.Contains(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator))
          num1 = numberFormat.IndexOf(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
        int num2 = numberFormat.IndexOf("#");
        if (this.StartsWithExt(text, '('.ToString()))
          ++num2;
        if (num2 < 0)
          num2 = 0;
        bool flag = false;
        if (this.StartsWithExt(text, '-'.ToString()))
          flag = true;
        for (int index = num2; index < num1 && (!char.IsNumber(text[index]) || text[index] == '0'); ++index)
        {
          if (numberFormat[index] != '0')
          {
            if (text[index] == '0')
            {
              text = text.Remove(index, 1);
              text = text.Insert(index, " ");
            }
            else
            {
              text = text.Remove(index, 1);
              numberFormat = numberFormat.Remove(index, 1);
              --index;
              --num1;
            }
          }
        }
        if (flag && !this.StartsWithExt(text, '-'.ToString()))
          text = '-'.ToString() + text;
      }
    }
    return text;
  }

  private bool IsNeedToFormatFieldResult(string numberFormat)
  {
    return numberFormat.Contains("£") ? this.IsNeedToFormatFieldResult(numberFormat, "£", true) : this.IsNeedToFormatFieldResult(numberFormat, "$", false);
  }

  private bool IsNeedToFormatFieldResult(string numberFormat, string currency, bool checkContains)
  {
    string str = $"{currency}{CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator}0{CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator}00";
    return str == numberFormat || checkContains && numberFormat.Contains(str) || str.Replace(currency, "") == numberFormat;
  }

  private string GetFormattedString(string format, string text)
  {
    char groupSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator[0];
    char decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
    if ((int) groupSeparator == (int) decimalSeparator)
      return text;
    if (groupSeparator == ' ')
      return string.Empty;
    if (format.Contains(";"))
      this.CheckNumberFormatForNegativeValue(ref format, text);
    string empty = string.Empty;
    string fractionalPart = string.Empty;
    int length = text.IndexOf(decimalSeparator);
    string integerPart;
    if (length != -1)
    {
      integerPart = text.Substring(0, length);
      fractionalPart = text.Substring(length + 1, text.Length - integerPart.Length - 1);
    }
    else
      integerPart = text;
    StringBuilder fieldResult = new StringBuilder();
    this.FormatFieldValue(format, groupSeparator, decimalSeparator, integerPart, fractionalPart, fieldResult);
    return fieldResult.ToString();
  }

  private void CheckNumberFormatForNegativeValue(ref string format, string text)
  {
    int length = format.IndexOf(";");
    format = this.StartsWithExt(text, "-") ? format.Substring(length + 1, format.Length - (length + 1)) : format.Substring(0, length);
    if (format.IndexOf(";") <= 0)
      return;
    format = format.Substring(0, format.IndexOf(";"));
  }

  private void FormatFieldValue(
    string format,
    char groupSeparator,
    char decimalSeparator,
    string integerPart,
    string fractionalPart,
    StringBuilder fieldResult)
  {
    string[] strArray = this.SplitNumberFormat(format, groupSeparator, decimalSeparator);
    bool flag = this.StartsWithExt(format, groupSeparator.ToString()) || this.StartsWithExt(format, "0");
    bool insertGroupSeparator = true;
    if (flag || this.HasValidSeperator(format, groupSeparator, decimalSeparator, ref insertGroupSeparator))
    {
      if (this.StartsWithExt(format, "0") && !format.Contains(groupSeparator.ToString()))
        insertGroupSeparator = false;
      this.UpdateIntegeralPart(integerPart, strArray[0], groupSeparator.ToString(), fieldResult, insertGroupSeparator);
      if (!flag && !this.StartsWithExt(fieldResult.ToString(), decimalSeparator.ToString()))
      {
        if (insertGroupSeparator)
        {
          if (this.StartsWithExt(fieldResult.ToString(), '-'.ToString()) && this.StartsWithExt(format, '-'.ToString()))
            fieldResult.Remove(0, 1);
          this.InsertBeforeText(fieldResult, format, groupSeparator);
        }
        else
          this.InsertBeforeText(fieldResult, format, '0');
      }
    }
    if (string.IsNullOrEmpty(strArray[1]))
      return;
    this.UpdateFractionalPart(fractionalPart, strArray[1], groupSeparator, decimalSeparator, fieldResult);
    if (!this.StartsWithExt(fieldResult.ToString(), decimalSeparator.ToString()))
      return;
    this.InsertBeforeText(fieldResult, format, decimalSeparator);
  }

  private void InsertBeforeText(StringBuilder fieldResult, string numberFormat, char separator)
  {
    string[] strArray = numberFormat.Split(separator);
    fieldResult.Insert(0, strArray[0]);
  }

  private void UpdateFractionalPart(
    string fractionalPart,
    string numberFormats,
    char groupSeparator,
    char decimalSeparator,
    StringBuilder fieldResult)
  {
    if (string.IsNullOrEmpty(fractionalPart))
    {
      numberFormats = numberFormats.Replace(groupSeparator.ToString(), "");
      fieldResult.Append(decimalSeparator.ToString() + numberFormats);
    }
    else
    {
      fieldResult.Append(decimalSeparator);
      for (int index = 0; index < numberFormats.Length; ++index)
      {
        char ch = index < fractionalPart.Length ? fractionalPart[index] : '0';
        if ((int) numberFormats[index] == (int) decimalSeparator && index != 0)
          fieldResult.Append(decimalSeparator);
        else if (numberFormats[index] == '%')
          fieldResult.Append('%');
        else if ((int) numberFormats[index] != (int) groupSeparator)
          fieldResult.Append(ch);
      }
    }
  }

  private void UpdateIntegeralPart(
    string integerPart,
    string numberFormats,
    string groupSeparator,
    StringBuilder fieldResult,
    bool insertSeparator)
  {
    if (integerPart.Contains(groupSeparator))
      integerPart = this.UpdateFieldValueBySeparator(integerPart, groupSeparator[0]);
    int num1 = string.IsNullOrEmpty(numberFormats) || numberFormats.Length <= integerPart.Length ? integerPart.Length : numberFormats.Length;
    int length = integerPart.Length;
    int num2 = 0;
    bool flag = false;
    for (int index = num1 - 1; index >= 0; --index)
    {
      string s = length > 0 ? integerPart[length - 1].ToString() : "0";
      fieldResult.Insert(0, s);
      if (insertSeparator && (fieldResult.Length - num2) % 3 == 0 && index != 0 && index != num1 - 1)
      {
        fieldResult.Insert(0, groupSeparator);
        ++num2;
        flag = true;
      }
      else if (flag && char.IsDigit(s, 0))
        flag = false;
      --length;
    }
    if (!flag)
      return;
    fieldResult.Remove(fieldResult.ToString().IndexOf(groupSeparator), 1);
  }

  private string UpdateFieldValueBySeparator(string fieldValue, char separator)
  {
    string[] strArray = fieldValue.Split(separator);
    if (strArray.Length - 1 != 1)
      return this.AddFieldValues(this.SplitFieldValue(fieldValue, separator));
    int num = fieldValue.IndexOf(separator);
    return num + 4 <= fieldValue.Length || num == 0 ? fieldValue.Replace(separator.ToString(), "") : (this.ConvertToInteger(strArray[0]) + this.ConvertToInteger(strArray[1])).ToString();
  }

  private string AddFieldValues(List<int> fieldValues)
  {
    int num = 0;
    foreach (int fieldValue in fieldValues)
      num += fieldValue;
    return num.ToString();
  }

  private List<int> SplitFieldValue(string fieldValue, char separator)
  {
    List<int> integerFieldValues = new List<int>();
    StringBuilder integerFieldValue = new StringBuilder();
    bool foundMultipleSeparator = false;
    int lastGroupLegth = 0;
    for (int index = fieldValue.Length - 1; index >= 0; --index)
    {
      if ((int) fieldValue[index] == (int) separator)
      {
        this.SplitBySeparator(ref foundMultipleSeparator, index, fieldValue, separator, integerFieldValue, integerFieldValues, ref lastGroupLegth);
      }
      else
      {
        integerFieldValue.Insert(0, fieldValue[index].ToString());
        if (foundMultipleSeparator)
          foundMultipleSeparator = false;
      }
    }
    integerFieldValues.Add(this.ConvertToInteger(integerFieldValue.ToString()));
    return integerFieldValues;
  }

  private void SplitBySeparator(
    ref bool foundMultipleSeparator,
    int charIndex,
    string fieldValue,
    char separator,
    StringBuilder integerFieldValue,
    List<int> integerFieldValues,
    ref int lastGroupLegth)
  {
    if (foundMultipleSeparator)
      return;
    if (charIndex - 1 < 0 && (int) fieldValue[charIndex - 1] == (int) separator)
    {
      integerFieldValues.Add(this.ConvertToInteger(integerFieldValue.ToString()));
      foundMultipleSeparator = true;
      this.Clear(integerFieldValue);
    }
    else if (integerFieldValue.Length - lastGroupLegth < 3)
    {
      integerFieldValues.Add(this.ConvertToInteger(integerFieldValue.ToString()));
      lastGroupLegth = 0;
      this.Clear(integerFieldValue);
    }
    else
      lastGroupLegth = integerFieldValue.Length;
  }

  private void Clear(StringBuilder stringBuilder)
  {
    stringBuilder.Length = 0;
    stringBuilder.Capacity = 0;
  }

  private int ConvertToInteger(string value)
  {
    int result = 0;
    int.TryParse(value, out result);
    return result;
  }

  private bool IsBeginWithDoubleQuote(int index)
  {
    for (; index >= 0; --index)
    {
      if ((int) this.FieldCode[index] == (int) ControlChar.DoubleQuote)
        return true;
      if (this.FieldCode[index] == '#')
        return false;
    }
    return false;
  }

  private bool HasValidSeperator(
    string numberFormat,
    char seperator,
    char decimalSeparator,
    ref bool insertGroupSeparator)
  {
    int num1 = this.FieldCode.IndexOf(numberFormat);
    int num2;
    if (num1 <= 0)
    {
      num2 = 0;
    }
    else
    {
      int index = num1;
      int num3 = index - 1;
      num2 = this.IsBeginWithDoubleQuote(index) ? 1 : 0;
    }
    bool flag = num2 != 0;
    for (int index = 0; index < numberFormat.Length; ++index)
    {
      if ((int) numberFormat[index] == (int) seperator)
        return true;
      if (numberFormat[index] == '0')
      {
        if (!numberFormat.Contains(seperator.ToString()))
          insertGroupSeparator = false;
        return true;
      }
      if ((int) numberFormat[index] == (int) decimalSeparator || numberFormat[index] == ' ' && !flag)
        return false;
    }
    return false;
  }

  private string[] SplitNumberFormat(
    string numberFormat,
    char groupSeparator,
    char decimalSeparator)
  {
    string[] strArray = new string[2];
    if (numberFormat.Contains(decimalSeparator.ToString()))
    {
      int length = numberFormat.IndexOf(decimalSeparator);
      strArray[0] = numberFormat.Substring(0, length);
      strArray[1] = numberFormat.Substring(strArray[0].Length + 1, numberFormat.Length - strArray[0].Length - 1);
      if (!this.StartsWithExt(strArray[0], 0.ToString()))
      {
        if (strArray[0].Contains(0.ToString()))
        {
          int startIndex = strArray[0].IndexOf('0');
          strArray[0] = strArray[0].Substring(startIndex, strArray[0].Length - startIndex);
        }
        else
          strArray[0] = string.Empty;
      }
    }
    else if (numberFormat.Contains("0"))
    {
      int startIndex = numberFormat.IndexOf('0');
      strArray[0] = numberFormat.Substring(startIndex, numberFormat.Length - startIndex);
      strArray[0] = strArray[0].Replace(groupSeparator.ToString(), "");
      strArray[1] = string.Empty;
    }
    return strArray;
  }

  protected string RemoveMergeFormat(string text)
  {
    if (text.Contains("\\* MERGEFORMAT"))
      text = text.Remove(text.IndexOf("\\* MERGEFORMAT")).Trim();
    else if (text.Contains("\\* Mergeformat"))
      text = text.Remove(text.IndexOf("\\* Mergeformat")).Trim();
    return text.Trim();
  }

  internal string RemoveMergeFormat(string fieldCode, ref string numberFormat)
  {
    if (fieldCode.Contains("\\#"))
    {
      numberFormat = fieldCode.Substring(fieldCode.IndexOf("\\#"));
      if (!numberFormat.Contains("\""))
      {
        int num = numberFormat.IndexOf("\\#");
        if (numberFormat.Length > num + 2)
        {
          numberFormat = numberFormat.Insert(num + 2, "\"");
          numberFormat = numberFormat.Insert(numberFormat.Length, "\"");
        }
      }
      if (numberFormat.IndexOf("\"") != -1)
        numberFormat = numberFormat.Substring(numberFormat.IndexOf("\"") + 1);
      if (numberFormat.LastIndexOf("\"") != -1)
        numberFormat = numberFormat.Remove(numberFormat.LastIndexOf("\""));
      numberFormat = numberFormat.Trim();
      fieldCode = fieldCode.Remove(fieldCode.IndexOf("\\#")).Trim();
    }
    if (fieldCode.Contains("\\* MERGEFORMAT"))
      fieldCode = fieldCode.Remove(fieldCode.IndexOf("\\* MERGEFORMAT")).Trim();
    else if (fieldCode.Contains("\\* Mergeformat"))
      fieldCode = fieldCode.Remove(fieldCode.IndexOf("\\* Mergeformat")).Trim();
    return fieldCode.Trim();
  }

  private string UpdateFormula(string fieldCode, ref bool isFieldCodeStartWithCurrencySymbol)
  {
    if (this.IsFunction(fieldCode.ToLower()))
      return this.UpdateFunction(fieldCode, ref isFieldCodeStartWithCurrencySymbol);
    double result;
    if (this.IsExpression(fieldCode))
    {
      result = this.UpdateExpression(fieldCode, ref isFieldCodeStartWithCurrencySymbol);
    }
    else
    {
      Bookmark byName = this.Document.Bookmarks.FindByName(fieldCode);
      if (byName != null)
      {
        string empty = string.Empty;
        WParagraph ownerParagraph = byName.BookmarkStart.OwnerParagraph;
        if (ownerParagraph == byName.BookmarkEnd.OwnerParagraph)
        {
          for (int index = byName.BookmarkStart.PreviousSibling is WTextFormField ? byName.BookmarkStart.Index - 1 : byName.BookmarkStart.Index + 1; index < byName.BookmarkEnd.Index; ++index)
          {
            Entity childEntity = ownerParagraph.ChildEntities[index];
            switch (childEntity)
            {
              case WField _:
                empty += (childEntity as WField).Text;
                index = (childEntity as WField).FieldEnd.Index;
                break;
              case WTextRange _:
                empty += (childEntity as WTextRange).Text;
                break;
            }
          }
        }
        char[] chArray = new char[24]
        {
          '@',
          '#',
          '%',
          '&',
          ' ',
          '`',
          '~',
          '{',
          '}',
          '[',
          ']',
          ';',
          ':',
          '"',
          '|',
          '<',
          '>',
          ',',
          '?',
          '=',
          '$',
          '\'',
          '\\',
          '£'
        };
        double.TryParse(empty.Trim(chArray), out result);
      }
      else
      {
        if (!fieldCode.Contains(CultureInfo.CurrentCulture.TextInfo.ListSeparator))
          throw new Exception("!Syntax Error, " + fieldCode.ToUpper());
        throw new Exception("!Undefined Bookmark, " + fieldCode.ToUpper());
      }
    }
    return result.ToString((IFormatProvider) CultureInfo.CurrentCulture);
  }

  private string UpdateFunction(string fieldCode, ref bool isFieldCodeStartWithCurrencySymbol)
  {
    string str = fieldCode.ToLower();
    int startIndex = fieldCode.IndexOf('(');
    if (fieldCode.Contains('('.ToString()))
      str = fieldCode.Remove(startIndex).ToLower().Trim();
    List<double> operands = new List<double>();
    int num = fieldCode.LastIndexOf(')');
    if (startIndex + 1 < num)
      operands = this.SplitOperands(fieldCode.Substring(startIndex + 1, num - startIndex - 1), ref isFieldCodeStartWithCurrencySymbol);
    else if (!(str == "true") && !(str == "false"))
      throw new Exception("!Syntax Error, " + (object) ')');
    switch (str)
    {
      case "product":
        return this.Product(operands).ToString((IFormatProvider) CultureInfo.CurrentCulture);
      case "sum":
        return this.Sum(operands).ToString((IFormatProvider) CultureInfo.CurrentCulture);
      case "average":
        return operands.Count <= 1 ? $"!Syntax Error, {operands[0]}" : this.Average(operands).ToString((IFormatProvider) CultureInfo.CurrentCulture);
      case "mod":
        return (operands[0] % operands[1]).ToString((IFormatProvider) CultureInfo.CurrentCulture);
      case "abs":
        return Math.Abs(operands[0]).ToString((IFormatProvider) CultureInfo.CurrentCulture);
      case "int":
        return Math.Floor(operands[0]).ToString((IFormatProvider) CultureInfo.CurrentCulture);
      case "round":
        return this.RoundOf(operands[0], (int) operands[1]).ToString((IFormatProvider) CultureInfo.CurrentCulture);
      case "sign":
        return Math.Sign(operands[0]).ToString();
      case "count":
        return operands.Count.ToString();
      case "defined":
        return this.Defined(operands[0].ToString((IFormatProvider) CultureInfo.InvariantCulture)).ToString((IFormatProvider) CultureInfo.CurrentCulture);
      case "or":
        return ((int) operands[0] | (int) operands[1]) != 0 ? "1" : "0";
      case "and":
        return ((int) operands[0] & (int) operands[1]) != 0 ? "1" : "0";
      case "not":
        return (int) operands[0] != 0 ? "0" : "1";
      case "max":
        operands.Sort();
        return operands[operands.Count - 1].ToString((IFormatProvider) CultureInfo.CurrentCulture);
      case "min":
        operands.Sort();
        return operands[0].ToString((IFormatProvider) CultureInfo.CurrentCulture);
      case "true":
        return "1";
      case "false":
        return "0";
      case "if":
        return operands[0] != 1.0 ? operands[2].ToString((IFormatProvider) CultureInfo.CurrentCulture) : operands[1].ToString((IFormatProvider) CultureInfo.CurrentCulture);
      default:
        throw new NotSupportedException($"The operation{str}is not supported.");
    }
  }

  private double Product(List<double> operands)
  {
    double num = 1.0;
    for (int index = 0; index < operands.Count; ++index)
      num *= operands[index];
    return num;
  }

  private double Sum(List<double> operands)
  {
    double num = 0.0;
    for (int index = 0; index < operands.Count; ++index)
      num += operands[index];
    return num;
  }

  private double Average(List<double> operands) => this.Sum(operands) / (double) operands.Count;

  private double RoundOf(double operand, int decimalPoint)
  {
    if (decimalPoint >= 0)
      return Math.Round(operand, decimalPoint = decimalPoint > 12 ? 12 : decimalPoint);
    decimalPoint = Math.Abs(decimalPoint);
    double num = Math.Pow(10.0, (double) decimalPoint);
    return (double) (long) (operand / num) * num;
  }

  private double Defined(string operand) => double.TryParse(operand, out double _) ? 1.0 : 0.0;

  private bool IsFunction(string text)
  {
    bool flag = false;
    foreach (string function in this.Functions)
    {
      if (this.StartsWithExt(text, function))
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  private bool IsExpression(string text)
  {
    List<string> stringList = new List<string>((IEnumerable<string>) new string[12]
    {
      "+",
      "-",
      "*",
      "/",
      "%",
      "^",
      "=",
      "<",
      "<=",
      ">",
      ">=",
      "<>"
    });
    bool flag = false;
    foreach (string str in stringList)
    {
      if (text.Contains(str))
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  private double UpdateExpression(string text, ref bool isFieldCodeStartWithCurrencySymbol)
  {
    List<string> operators = new List<string>((IEnumerable<string>) new string[12]
    {
      "=",
      "<",
      "<=",
      ">",
      ">=",
      "<>",
      "^",
      "%",
      "/",
      "*",
      "-",
      "+"
    });
    List<string> expression = this.SplitExpression(text, operators, ref isFieldCodeStartWithCurrencySymbol);
    foreach (string operation in operators)
      this.EvaluateExpression(ref expression, operation, operators.IndexOf(operation) > 6);
    return double.Parse(expression[0], (IFormatProvider) CultureInfo.CurrentCulture);
  }

  private void EvaluateExpression(
    ref List<string> expression,
    string operation,
    bool isAritmeticOperation)
  {
    while (expression.Contains(operation))
    {
      int index = !isAritmeticOperation ? expression.LastIndexOf(operation) : expression.IndexOf(operation);
      double num = 0.0;
      double x = double.Parse(expression[index - 1]);
      double y = double.Parse(expression[index + 1]);
      switch (operation)
      {
        case "+":
          num = x + y;
          break;
        case "-":
          num = x - y;
          break;
        case "*":
          num = x * y;
          break;
        case "/":
          num = x / y;
          break;
        case "%":
          num = x % y;
          break;
        case "^":
          num = Math.Pow(x, y);
          break;
        case "=":
          num = x == y ? 1.0 : 0.0;
          break;
        case "<":
          num = x < y ? 1.0 : 0.0;
          break;
        case "<=":
          num = x <= y ? 1.0 : 0.0;
          break;
        case ">":
          num = x > y ? 1.0 : 0.0;
          break;
        case ">=":
          num = x >= y ? 1.0 : 0.0;
          break;
        case "<>":
          num = x != y ? 1.0 : 0.0;
          break;
      }
      expression.RemoveAt(index + 1);
      expression.RemoveAt(index);
      expression.RemoveAt(index - 1);
      expression.Insert(index - 1, num.ToString());
    }
  }

  private List<string> SplitExpression(
    string text,
    List<string> operators,
    ref bool isFieldCodeStartWithCurrencySymbol)
  {
    string str1 = text;
    List<string> stringList = new List<string>();
    int num1 = 0;
    string str2 = string.Empty;
    string empty1 = string.Empty;
    int startIndex = 0;
    bool flag = false;
    while (startIndex < text.Length && text.Substring(startIndex).Contains("("))
    {
      for (int index = startIndex; index < text.Length; ++index)
      {
        if (char.IsLetter(text[index]))
        {
          if (this.IsFunction(text.Substring(index).ToLower()) && index > 0 && text[index - 1] != '(')
          {
            text = text.Insert(index, "(");
            flag = true;
            startIndex = index + 1;
            break;
          }
          break;
        }
      }
      if (flag)
      {
        int num2 = -1;
        for (int index = startIndex; index < text.Length; ++index)
        {
          if (text[index] == ')')
          {
            if (num2 == 0)
            {
              flag = false;
              text = text.Insert(index + 1, ")");
              startIndex = index + 1;
              break;
            }
            --num2;
          }
          else if (text[index] == '(')
            ++num2;
        }
      }
      else
        ++startIndex;
    }
    for (int index = 0; index < text.Length; ++index)
    {
      string empty2 = string.Empty;
      if (operators.Contains(text[index].ToString()))
      {
        empty2 = text[index].ToString();
        if (index != text.Length - 1 && (text[index + 1].ToString() == "=" || text[index + 1].ToString() == ">"))
          empty2 += text[index++].ToString();
      }
      if (index == text.Length - 1 && text[index] != ')' && !operators.Contains(text[index].ToString()))
        str2 += (string) (object) text[index];
      if (text[index] == '(')
        ++num1;
      else if (text[index] == ')')
        --num1;
      if (operators.Contains(empty2) && num1 == 0 || index == text.Length - 1)
      {
        str2 = this.RemoveCurrencySymbol(str2.Trim(), ref isFieldCodeStartWithCurrencySymbol);
        double result;
        if (double.TryParse(str2, out result))
        {
          stringList.Add(str2);
          str2 = string.Empty;
        }
        else if (str1 != str2)
        {
          string s = this.UpdateFormula(str2, ref isFieldCodeStartWithCurrencySymbol);
          if (!double.TryParse(s, out result))
            throw new Exception("!Syntax Error, " + s);
          stringList.Add(s);
          str2 = string.Empty;
        }
        if (index != text.Length - 1 && operators.Contains(empty2))
          stringList.Add(empty2.Trim());
      }
      else if (text[index] == '(' && num1 > 1 || text[index] == ')' && num1 > 0 || text[index] != '(' && text[index] != ')')
        str2 += (string) (object) text[index];
    }
    return stringList;
  }

  private List<double> SplitOperands(string text, ref bool isFieldCodeStartWithCurrencySymbol)
  {
    string listSeparator = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
    if (string.IsNullOrEmpty(listSeparator))
      listSeparator = ','.ToString();
    List<double> doubleList = new List<double>();
    int num = 0;
    string empty = string.Empty;
    for (int index = 0; index < text.Length; ++index)
    {
      if (index == text.Length - 1)
        empty += (string) (object) text[index];
      if (text[index] == '(')
        ++num;
      else if (text[index] == ')')
        --num;
      if (listSeparator.Contains(text[index].ToString()) && num == 0 || index == text.Length - 1)
      {
        string str = this.RemoveCurrencySymbol(empty.Trim(), ref isFieldCodeStartWithCurrencySymbol);
        double result;
        if (double.TryParse(str, out result))
        {
          doubleList.Add(result);
          empty = string.Empty;
        }
        else
        {
          string s = this.UpdateFormula(str, ref isFieldCodeStartWithCurrencySymbol);
          if (!double.TryParse(s, out result))
            throw new Exception("!Syntax Error, " + s);
          doubleList.Add(result);
          empty = string.Empty;
        }
      }
      else
        empty += (string) (object) text[index];
    }
    return doubleList;
  }

  public void Unlink()
  {
    bool isFieldSeperatorReached = false;
    bool isFieldReached = false;
    this.entities = new Stack<Entity>();
    this.nestedFields = new List<WField>();
    this.Range.Items.Clear();
    this.UpdateFieldRange();
    if (this.Range.Items.Count > 0)
      this.RemoveFieldItems(ref isFieldReached, ref isFieldSeperatorReached);
    this.Range.Items.Clear();
    this.entities.Clear();
    this.nestedFields.Clear();
  }

  private void RemoveFieldItems(ref bool isFieldReached, ref bool isFieldSeperatorReached)
  {
    if (this.FieldType == FieldType.FieldIndexEntry)
      return;
    if (this.FieldType == FieldType.FieldSet)
    {
      if (this.OwnerParagraph != null)
      {
        this.RemoveField(this);
        return;
      }
    }
    else if (this.FieldType != FieldType.FieldSet)
    {
      this.entities.Push((Entity) this);
      isFieldReached = true;
      isFieldSeperatorReached = false;
    }
    for (int index = 0; index < this.Range.Items.Count; ++index)
    {
      Entity paraItem = this.Range.Items[index] as Entity;
      switch (paraItem)
      {
        case ParagraphItem _:
          this.ParagraphItems(paraItem as ParagraphItem, ref isFieldReached, ref isFieldSeperatorReached);
          break;
        case TextBodyItem _:
          this.TextBodyItems(paraItem as TextBodyItem, ref isFieldReached, ref isFieldSeperatorReached);
          break;
      }
    }
    WParagraph ownerParagraph = this.OwnerParagraph;
    int index1 = ownerParagraph.ChildEntities.IndexOf((IEntity) this);
    if (this.OwnerParagraph != null)
      this.RemoveField(this);
    this.RemoveNestedFields();
    if (ownerParagraph.Items.Count <= index1 || !(ownerParagraph.ChildEntities[index1] is WField))
      return;
    (ownerParagraph.ChildEntities[index1] as WField).Unlink();
  }

  private void ParagraphItems(
    ParagraphItem paraItem,
    ref bool isFieldReached,
    ref bool isFieldSeperatorReached)
  {
    switch (paraItem)
    {
      case WField _ when (paraItem as WField).FieldType == FieldType.FieldSet:
        if ((paraItem as WField).FieldSeparator != null)
        {
          this.entities.Push((Entity) paraItem);
          isFieldReached = true;
        }
        paraItem.RemoveSelf();
        break;
      case WField _ when (paraItem as WField).FieldType != FieldType.FieldSet:
        this.nestedFields.Add(paraItem as WField);
        this.AddUnlinkNestedFieldStack(ref isFieldReached, ref isFieldSeperatorReached);
        this.entities.Push((Entity) paraItem);
        isFieldReached = true;
        isFieldSeperatorReached = false;
        return;
      case WFieldMark _ when (paraItem as WFieldMark).Type == FieldMarkType.FieldSeparator && isFieldReached:
        if (this.entities.Count > 0)
          this.entities.Pop();
        if (this.entities.Count == 0)
          isFieldSeperatorReached = true;
        paraItem.RemoveSelf();
        break;
      case WFieldMark _ when (paraItem as WFieldMark).Type == FieldMarkType.FieldEnd && isFieldReached:
        if (this.entities.Count == 0)
        {
          isFieldReached = false;
          isFieldSeperatorReached = false;
        }
        paraItem.RemoveSelf();
        if (this.m_unlinkNestedFieldStack.Count > 0)
        {
          this.ResetUnlinkNestedFieldStack(ref isFieldReached, ref isFieldSeperatorReached);
          break;
        }
        break;
      case WTextBox _:
        this.RemoveFieldCodesInTextBody((paraItem as WTextBox).TextBoxBody, ref isFieldReached, ref isFieldSeperatorReached);
        break;
      case Shape _:
        this.RemoveFieldCodesInTextBody((paraItem as Shape).TextBody, ref isFieldReached, ref isFieldSeperatorReached);
        break;
      case InlineContentControl _:
        this.RemoveFieldCodesInParagraph((paraItem as InlineContentControl).ParagraphItems, ref isFieldReached, ref isFieldSeperatorReached);
        break;
    }
    if (!isFieldReached || this.entities.Count == 0)
      return;
    paraItem.RemoveSelf();
  }

  private void TextBodyItems(
    TextBodyItem textbodyItem,
    ref bool isFieldReached,
    ref bool isFieldSeperatorReached)
  {
    switch (textbodyItem.EntityType)
    {
      case EntityType.Paragraph:
        WParagraph wparagraph = textbodyItem as WParagraph;
        this.RemoveFieldCodesInParagraph(wparagraph.Items, ref isFieldReached, ref isFieldSeperatorReached);
        if (wparagraph.Items.Count != 0 || isFieldSeperatorReached || !isFieldReached)
          break;
        wparagraph.RemoveSelf();
        break;
      case EntityType.BlockContentControl:
        BlockContentControl blockContentControl = textbodyItem as BlockContentControl;
        if (isFieldReached && this.entities.Count != 0)
        {
          blockContentControl.RemoveSelf();
          break;
        }
        this.RemoveFieldCodesInTextBody(blockContentControl.TextBody, ref isFieldReached, ref isFieldSeperatorReached);
        break;
      case EntityType.Table:
        if (isFieldReached && this.entities.Count != 0)
        {
          textbodyItem.RemoveSelf();
          break;
        }
        this.RemoveFieldCodesInTable(textbodyItem as WTable, ref isFieldReached, ref isFieldSeperatorReached);
        break;
    }
  }

  private void RemoveFieldCodesInTextBody(
    WTextBody textBody,
    ref bool isFieldReached,
    ref bool isFieldSeperatorReached)
  {
    for (int index = 0; index < textBody.ChildEntities.Count && (isFieldReached || isFieldSeperatorReached); ++index)
    {
      IEntity childEntity = (IEntity) textBody.ChildEntities[index];
      switch (childEntity.EntityType)
      {
        case EntityType.Paragraph:
          WParagraph wparagraph = childEntity as WParagraph;
          this.RemoveFieldCodesInParagraph(wparagraph.Items, ref isFieldReached, ref isFieldSeperatorReached);
          if (wparagraph.Items.Count == 0 && !isFieldSeperatorReached && isFieldReached)
          {
            textBody.ChildEntities.Remove((IEntity) wparagraph);
            --index;
            break;
          }
          break;
        case EntityType.BlockContentControl:
          BlockContentControl blockContentControl = childEntity as BlockContentControl;
          if (isFieldReached && this.entities.Count != 0)
          {
            textBody.ChildEntities.RemoveAt(index);
            --index;
            break;
          }
          this.RemoveFieldCodesInTextBody(blockContentControl.TextBody, ref isFieldReached, ref isFieldSeperatorReached);
          break;
        case EntityType.Table:
          if (isFieldReached && this.entities.Count != 0)
          {
            textBody.ChildEntities.RemoveAt(index);
            --index;
            break;
          }
          this.RemoveFieldCodesInTable(childEntity as WTable, ref isFieldReached, ref isFieldSeperatorReached);
          break;
      }
    }
  }

  private void RemoveFieldCodesInParagraph(
    ParagraphItemCollection paraItems,
    ref bool isFieldReached,
    ref bool isFieldSeperatorReached)
  {
    for (int index = 0; index < paraItems.Count && (isFieldReached || isFieldSeperatorReached); ++index)
    {
      if (paraItems[index] is WField && (paraItems[index] as WField).FieldType == FieldType.FieldSet)
      {
        paraItems.RemoveAt(index);
        --index;
      }
      else if (paraItems[index] is WField && (paraItems[index] as WField).FieldType != FieldType.FieldSet)
      {
        this.nestedFields.Add(paraItems[index] as WField);
        this.AddUnlinkNestedFieldStack(ref isFieldReached, ref isFieldSeperatorReached);
        this.entities.Push((Entity) paraItems[index]);
        isFieldReached = true;
        isFieldSeperatorReached = false;
      }
      else if (paraItems[index] is WFieldMark && (paraItems[index] as WFieldMark).Type == FieldMarkType.FieldSeparator && isFieldReached)
      {
        if (this.entities.Count > 0)
          this.entities.Pop();
        if (this.entities.Count == 0)
          isFieldSeperatorReached = true;
        paraItems.RemoveAt(index);
        --index;
      }
      else if (paraItems[index] is WFieldMark && (paraItems[index] as WFieldMark).Type == FieldMarkType.FieldEnd && isFieldReached)
      {
        if (this.entities.Count == 0)
        {
          isFieldReached = false;
          isFieldSeperatorReached = false;
        }
        paraItems.RemoveAt(index);
        --index;
        if (this.m_unlinkNestedFieldStack.Count > 0)
          this.ResetUnlinkNestedFieldStack(ref isFieldReached, ref isFieldSeperatorReached);
      }
      else
      {
        if (paraItems[index] is WTextBox)
          this.RemoveFieldCodesInTextBody((paraItems[index] as WTextBox).TextBoxBody, ref isFieldReached, ref isFieldSeperatorReached);
        else if (paraItems[index] is Shape)
          this.RemoveFieldCodesInTextBody((paraItems[index] as Shape).TextBody, ref isFieldReached, ref isFieldSeperatorReached);
        else if (paraItems[index] is InlineContentControl)
          this.RemoveFieldCodesInParagraph((paraItems[index] as InlineContentControl).ParagraphItems, ref isFieldReached, ref isFieldSeperatorReached);
        if (isFieldReached && this.entities.Count != 0)
        {
          paraItems.RemoveAt(index);
          --index;
        }
      }
    }
  }

  private void RemoveFieldCodesInTable(
    WTable table,
    ref bool isFieldReached,
    ref bool isFieldSeperatorReached)
  {
    foreach (WTableRow row in (CollectionImpl) table.Rows)
    {
      foreach (WTextBody cell in (CollectionImpl) row.Cells)
        this.RemoveFieldCodesInTextBody(cell, ref isFieldReached, ref isFieldSeperatorReached);
    }
  }

  private void RemoveNestedFields()
  {
    foreach (WField nestedField in this.nestedFields)
    {
      if (nestedField.OwnerParagraph != null)
        this.RemoveField(nestedField);
    }
  }

  private void RemoveField(WField field)
  {
    WParagraph ownerParagraph = field.OwnerParagraph;
    int index = ownerParagraph.ChildEntities.IndexOf((IEntity) field);
    ownerParagraph.Items.RemoveAt(index);
    if (ownerParagraph.Items.Count != 0)
      return;
    ownerParagraph.OwnerTextBody.ChildEntities.Remove((IEntity) ownerParagraph);
  }

  private void AddUnlinkNestedFieldStack(ref bool isFieldReached, ref bool isFieldSeperatorReached)
  {
    this.m_unlinkNestedFieldStack.Push(new Dictionary<string, object>()
    {
      {
        nameof (isFieldReached),
        (object) isFieldReached
      },
      {
        nameof (isFieldSeperatorReached),
        (object) isFieldSeperatorReached
      },
      {
        "entities",
        this.entities == null ? (object) (Stack<Entity>) null : (object) new Stack<Entity>((IEnumerable<Entity>) this.entities.ToArray())
      }
    });
  }

  private void ResetUnlinkNestedFieldStack(
    ref bool isFieldReached,
    ref bool isFieldSeperatorReached)
  {
    Dictionary<string, object> dictionary = this.m_unlinkNestedFieldStack.Pop();
    isFieldReached = (bool) dictionary[nameof (isFieldReached)];
    isFieldSeperatorReached = (bool) dictionary[nameof (isFieldSeperatorReached)];
    this.entities = new Stack<Entity>((IEnumerable<Entity>) (dictionary["entities"] as Stack<Entity>).ToArray());
  }

  internal void UpdateFieldRange()
  {
    try
    {
      if (this.GetOwnerParagraphValue() == null || this.FieldEnd == null)
      {
        this.m_range.Items.Clear();
      }
      else
      {
        if (this.OwnerParagraph == this.FieldEnd.OwnerParagraph)
        {
          if (this.Owner is InlineContentControl && this.Owner == this.FieldEnd.Owner)
          {
            for (int index = this.GetIndexInOwnerCollection() + 1; index < (this.Owner as InlineContentControl).ParagraphItems.Count; ++index)
            {
              this.m_range.Items.Add((object) (this.Owner as InlineContentControl).ParagraphItems[index]);
              if ((this.Owner as InlineContentControl).ParagraphItems[index] == this.FieldEnd)
                break;
            }
          }
          else
          {
            for (int index = this.GetIndexInOwnerCollection() + 1; index < this.OwnerParagraph.Items.Count; ++index)
            {
              if (this.FieldType == FieldType.FieldSet && this.OwnerParagraph.Items[index] is WField && this.m_range.Items[this.m_range.Items.Count - 1] is WTextRange && !(this.m_range.Items[this.m_range.Items.Count - 1] as WTextRange).Text.EndsWith(" "))
                (this.m_range.Items[this.m_range.Items.Count - 1] as WTextRange).Text += " ";
              this.m_range.Items.Add((object) this.OwnerParagraph.Items[index]);
              if (this.OwnerParagraph.Items[index] == this.FieldEnd)
                break;
            }
          }
        }
        else
        {
          for (int index = this.GetIndexInOwnerCollection() + 1; index < this.OwnerParagraph.Items.Count; ++index)
            this.m_range.Items.Add((object) this.OwnerParagraph.Items[index]);
          for (int index = this.OwnerParagraph.GetIndexInOwnerCollection() + 1; index < this.OwnerParagraph.OwnerTextBody.Items.Count; ++index)
          {
            this.m_range.Items.Add((object) this.OwnerParagraph.OwnerTextBody.Items[index]);
            if (this.OwnerParagraph.OwnerTextBody.Items[index] == this.FieldEnd.OwnerParagraph)
              break;
          }
        }
        this.IsFieldRangeUpdated = true;
      }
    }
    catch
    {
      this.m_range.Items.Clear();
    }
  }

  private string UpdateNestedFieldCode(bool isUpdateNestedFields, WMergeField mergeField)
  {
    string str = string.Empty;
    this.IsFieldSeparator = false;
    this.IsSkip = false;
    this.m_nestedFields.Clear();
    for (int index = 0; index < this.Range.Items.Count; ++index)
    {
      Entity entity = this.Range.Items[index] as Entity;
      if (entity != mergeField)
      {
        if (this != null && this.FieldType == FieldType.FieldSet && entity is WField && !(entity as WField).IsUpdated && !(entity as WField).IsSkip && (entity as WField).FieldEnd != null)
          str += " ";
        if (this != null && this.FieldType == FieldType.FieldIf && entity is WField && !(entity as WField).IsSkip)
          str = str + (object) '\u0013' + this.UpdateTextForParagraphItem(entity, isUpdateNestedFields) + (object) '\u0015';
        str = !(entity is ParagraphItem) ? str + this.UpdateTextForTextBodyItem(entity, isUpdateNestedFields, mergeField) : str + this.UpdateTextForParagraphItem(entity, isUpdateNestedFields);
        if (this.IsFieldSeparator)
          break;
      }
      else
        break;
    }
    this.IsFieldSeparator = false;
    this.IsSkip = false;
    this.m_nestedFields.Clear();
    return str;
  }

  protected string UpdateTextForTextBodyItem(Entity entity, bool isUpdateNestedFields)
  {
    return this.UpdateTextForTextBodyItem(entity, isUpdateNestedFields, (WMergeField) null);
  }

  private string UpdateTextForTextBodyItem(
    Entity entity,
    bool isUpdateNestedFields,
    WMergeField mergeField)
  {
    string str = string.Empty;
    switch (entity)
    {
      case WParagraph _:
        if (!this.IsSkip)
          str += "\r";
        for (int index = 0; index < (entity as WParagraph).Items.Count; ++index)
        {
          ParagraphItem paragraphItem = (entity as WParagraph).Items[index];
          if (paragraphItem == mergeField)
          {
            this.IsFieldSeparator = true;
            break;
          }
          str += this.UpdateTextForParagraphItem((Entity) paragraphItem, isUpdateNestedFields);
          if (this.IsFieldSeparator)
            return str;
        }
        break;
      case WTable _:
        if (!this.IsSkip)
        {
          str = str + (object) '\u0013' + this.UpdateTextForTable(entity, isUpdateNestedFields, mergeField) + (object) '\u0015';
          break;
        }
        break;
    }
    return str;
  }

  private string UpdateTextForTable(
    Entity entity,
    bool isUpdateNestedFields,
    WMergeField mergeField)
  {
    string empty = string.Empty;
    for (int index1 = 0; index1 < (entity as WTable).Rows.Count; ++index1)
    {
      WTableRow row = (entity as WTable).Rows[index1];
      for (int index2 = 0; index2 < row.Cells.Count; ++index2)
      {
        WTableCell cell = row.Cells[index2];
        for (int index3 = 0; index3 < cell.Items.Count; ++index3)
        {
          empty += this.UpdateTextForTextBodyItem((Entity) cell.Items[index3], isUpdateNestedFields, mergeField);
          if (this.IsFieldSeparator)
            return empty;
        }
        if (!this.IsSkip)
          empty += "\a";
      }
      if (!this.IsSkip)
        empty += "\r\a";
    }
    return empty;
  }

  protected string UpdateTextForParagraphItem(Entity entity, bool isUpdateNestedFields)
  {
    string str = string.Empty;
    if (this.IsFieldSeparator)
      return str;
    if (entity is WField && !this.IsSkip)
    {
      WField wfield = entity as WField;
      if (wfield.FieldEnd != null)
      {
        if (isUpdateNestedFields)
        {
          if (wfield.FieldType == FieldType.FieldMergeField || wfield.FieldType == FieldType.FieldHyperlink || wfield.FieldType == FieldType.FieldAutoNum)
          {
            str = wfield.Text;
          }
          else
          {
            if (!wfield.IsUpdated)
            {
              wfield.Range.Items.Clear();
              wfield.UpdateFieldRange();
              wfield.IsFieldRangeUpdated = false;
              wfield.Update();
            }
            str = wfield.FieldType != FieldType.FieldUnknown ? wfield.FieldResult : wfield.Text;
          }
        }
        else
          str = wfield.FindFieldResult();
        this.m_nestedFields.Push(wfield);
        if (wfield.FieldType != FieldType.FieldAutoNum)
          this.IsSkip = true;
      }
    }
    else if (this.FieldSeparator == entity)
      this.IsFieldSeparator = true;
    else if (this.IsSkip && entity is WFieldMark && this.m_nestedFields.Peek().FieldEnd == entity)
    {
      this.IsSkip = false;
      this.m_nestedFields.Pop();
    }
    else
    {
      switch (entity)
      {
        case WTextRange _ when !this.IsSkip:
          str = (entity as WTextRange).Text;
          break;
        case InlineContentControl _ when !this.IsSkip:
          for (int index = 0; index < (entity as InlineContentControl).ParagraphItems.Count; ++index)
          {
            Entity paragraphItem = (Entity) (entity as InlineContentControl).ParagraphItems[index];
            str = !(paragraphItem is ParagraphItem) ? str + this.UpdateTextForTextBodyItem(paragraphItem, isUpdateNestedFields) : str + this.UpdateTextForParagraphItem(paragraphItem, isUpdateNestedFields);
            if (this.IsFieldSeparator)
              break;
          }
          break;
      }
    }
    return str;
  }

  internal string GetFieldResultValue()
  {
    string fieldResultValue = string.Empty;
    if (this.Owner is WParagraph && this.FieldSeparator != null && this.FieldSeparator.Owner is WParagraph && this.FieldEnd != null && this.FieldEnd.Owner is WParagraph && this.FieldType != FieldType.FieldIncludePicture)
    {
      if (this.Range.Items.Count == 0 && !this.IsFieldRangeUpdated)
        this.UpdateFieldRange();
      int num1 = this.FieldSeparator.OwnerParagraph != this.OwnerParagraph ? this.Range.Items.IndexOf((object) this.FieldSeparator.OwnerParagraph) : this.Range.Items.IndexOf((object) this.FieldSeparator) + 1;
      if (num1 == -1)
        return fieldResultValue;
      for (int index = num1; index < this.Range.Items.Count; ++index)
      {
        Entity entity = this.Range.Items[index] as Entity;
        if (entity is WParagraph)
        {
          int startIndex = 0;
          int endIndex = (entity as WParagraph).Items.Count - 1;
          if (this.FieldSeparator.OwnerParagraph == entity)
            startIndex = this.FieldSeparator.GetIndexInOwnerCollection() + 1;
          if (this.FieldEnd.OwnerParagraph == entity)
            endIndex = this.FieldEnd.GetIndexInOwnerCollection() - 1;
          if (endIndex != -1)
            fieldResultValue += (entity as WParagraph).GetText(startIndex, endIndex);
          if (this.FieldEnd.OwnerParagraph != entity)
            fieldResultValue += ControlChar.ParagraphBreak;
          if (endIndex != -1 && endIndex < (entity as WParagraph).Items.Count - 1)
          {
            fieldResultValue = fieldResultValue + (entity as WParagraph).GetText(endIndex + 1, (entity as WParagraph).Items.Count - 1) + ControlChar.ParagraphBreak;
            this.Document.m_prevClonedEntity = (TextBodyItem) this.FieldEnd.OwnerParagraph;
          }
        }
        else if (entity is WTable)
        {
          fieldResultValue += (entity as WTable).GetTableText();
        }
        else
        {
          if (entity is WField)
          {
            int num2 = this.Range.Items.IndexOf((object) (entity as WField).FieldSeparator);
            if (num2 != -1)
            {
              index = num2;
              continue;
            }
          }
          if (entity is WMergeField)
            fieldResultValue += (entity as WField).Text;
          else if (entity is WTextRange)
            fieldResultValue += (entity as WTextRange).Text;
          else if (entity is Break)
            fieldResultValue += ControlChar.ParagraphBreak;
        }
      }
    }
    return fieldResultValue;
  }

  private void ParseFieldValue(string fieldValue)
  {
    Match match = new Regex("(\\w+)\\s+\"?([^\"]*)\"?").Match(fieldValue.Trim());
    if (match.Groups[2].Length == 0)
      this.m_fieldValue = match.Groups[1].Value;
    else
      this.m_fieldValue = match.Groups[2].Value;
  }

  private static string ClearStringFromOtherCharacters(string value)
  {
    return value.Remove(0, 1).Trim('"');
  }

  internal void RemoveFieldSeparator(WFieldMark fieldMark)
  {
    int inOwnerCollection1 = fieldMark.ParentField.FieldSeparator.GetIndexInOwnerCollection();
    WParagraph ownerParagraph1 = fieldMark.OwnerParagraph;
    BookmarkStart bookmarkStart = new BookmarkStart((IWordDocument) this.m_doc, "_fieldBookmark");
    BookmarkEnd bookmarkEnd = new BookmarkEnd((IWordDocument) this.m_doc, "_fieldBookmark");
    ownerParagraph1.ChildEntities.Insert(inOwnerCollection1, (IEntity) bookmarkStart);
    this.EnsureBookmarkStart(bookmarkStart);
    WParagraph ownerParagraph2 = fieldMark.OwnerParagraph;
    int inOwnerCollection2 = fieldMark.GetIndexInOwnerCollection();
    ownerParagraph2.Items.Insert(inOwnerCollection2, (IEntity) bookmarkEnd);
    this.EnsureBookmarkStart(bookmarkEnd);
    BookmarksNavigator bookmarksNavigator = new BookmarksNavigator((IWordDocument) this.m_doc);
    bookmarksNavigator.MoveToBookmark("_fieldBookmark");
    bookmarksNavigator.DeleteBookmarkContent(false);
    if (ownerParagraph1.Items.Contains((IEntity) bookmarkStart))
      ownerParagraph1.Items.Remove((IEntity) bookmarkStart);
    if (!ownerParagraph2.Items.Contains((IEntity) bookmarkEnd))
      return;
    ownerParagraph2.Items.Remove((IEntity) bookmarkEnd);
  }

  protected void ParseField(string fieldCode)
  {
    char[] chArray = new char[1]{ '\\' };
    string[] fieldValues = fieldCode.Split(chArray);
    this.ParseFieldValue(fieldValues[0]);
    this.ParseFieldFormat(fieldValues);
  }

  protected void ParseFieldFormat(string[] fieldValues)
  {
    for (int index = 1; index < fieldValues.Length; ++index)
    {
      string fieldValue = fieldValues[index];
      if (fieldValue.Length > 0)
      {
        string str = WField.ClearStringFromOtherCharacters(fieldValue).Trim();
        switch (fieldValue[0])
        {
          case '*':
            switch (str)
            {
              case "Upper":
                this.m_textFormat = TextFormat.Uppercase;
                this.SetTextFormatSwitchString();
                continue;
              case "Lower":
                this.m_textFormat = TextFormat.Lowercase;
                this.SetTextFormatSwitchString();
                continue;
              case "Caps":
                this.m_textFormat = TextFormat.Titlecase;
                this.SetTextFormatSwitchString();
                continue;
              case "FirstCap":
                this.m_textFormat = TextFormat.FirstCapital;
                this.SetTextFormatSwitchString();
                continue;
              default:
                WField wfield1 = this;
                wfield1.m_formattingString = $"{wfield1.m_formattingString} \\{fieldValue}";
                continue;
            }
          default:
            WField wfield2 = this;
            wfield2.m_formattingString = $"{wfield2.m_formattingString} \\{fieldValue}";
            continue;
        }
      }
    }
  }

  private void ParseLocalRef(string fieldCode, int startPos)
  {
    startPos += 2;
    if (fieldCode.Length <= startPos)
      return;
    string str = fieldCode.Substring(startPos, fieldCode.Length - startPos).Trim();
    int startIndex = str.IndexOf("\"");
    if (startIndex == -1)
      return;
    int num = str.IndexOf("\"", startIndex + 1);
    if (num == -1)
      return;
    this.m_localRef = str.Substring(startIndex, num + 1 - startIndex);
  }

  private string ParseSwitches(string text, int index)
  {
    text = text.Remove(0, index + 2).Trim();
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    if (this.IsStartWithValidChar(text, ref empty1) && !string.IsNullOrEmpty(empty1) && this.IsEndWithValidChar(text, ref empty2) && !string.IsNullOrEmpty(empty2))
    {
      text = text.Remove(0, text.IndexOf(empty1) + 1);
      text = text.Remove(text.LastIndexOf(empty2));
    }
    return text;
  }

  private bool IsStartWithValidChar(string text, ref string startCharacter)
  {
    startCharacter = string.Empty;
    string[] strArray = new string[2]
    {
      "\"",
      '“'.ToString()
    };
    foreach (string str in strArray)
    {
      if (this.StartsWithExt(text, str))
      {
        startCharacter = str;
        return true;
      }
    }
    return false;
  }

  private bool IsEndWithValidChar(string text, ref string endCharacter)
  {
    endCharacter = string.Empty;
    string[] strArray = new string[2]
    {
      "\"",
      '”'.ToString()
    };
    foreach (string str in strArray)
    {
      if (text.EndsWith(str))
      {
        endCharacter = str;
        return true;
      }
    }
    return false;
  }

  private string RemoveMeridiem(string text, out bool isMeridiemDefined)
  {
    isMeridiemDefined = false;
    string lower = text.ToLower();
    if (lower.Contains("am/pm"))
    {
      text = text.Remove(lower.IndexOf("am/pm"), lower.Length - lower.IndexOf("am/pm"));
      isMeridiemDefined = true;
    }
    return text;
  }

  private string UpdateMeridiem(string text, DateTime currentDateTime)
  {
    CultureInfo cultureInfo = !this.CharacterFormat.HasValue(73) ? CultureInfo.CurrentCulture : this.GetCulture((LocaleIDs) this.CharacterFormat.LocaleIdASCII);
    text = currentDateTime.Hour >= 12 ? text + cultureInfo.DateTimeFormat.PMDesignator : text + cultureInfo.DateTimeFormat.AMDesignator;
    return text;
  }

  internal string UpdateDateValue(string text, DateTime currentDateTime)
  {
    int count = 0;
    bool flag = false;
    string dateValue = string.Empty;
    int index = 0;
    while (index < text.Length)
    {
      if (flag)
      {
        switch (text[index])
        {
          case '\'':
            flag = false;
            ++index;
            continue;
          case '\\':
            ++index;
            continue;
          default:
            dateValue += (string) (object) text[index];
            ++index;
            continue;
        }
      }
      else
      {
        switch (text[index])
        {
          case '\'':
            flag = true;
            ++index;
            continue;
          case 'D':
          case 'd':
            while (index < text.Length && (text[index] == 'd' || text[index] == 'D'))
            {
              ++index;
              ++count;
            }
            dateValue = this.UpdateDay(dateValue, currentDateTime, count);
            count = 0;
            continue;
          case 'H':
          case 'h':
            bool is12HoursFormat = false;
            while (index < text.Length && (text[index] == 'h' || text[index] == 'H'))
            {
              if (text[index] == 'h')
                is12HoursFormat = true;
              ++index;
              ++count;
            }
            dateValue = this.UpdateHour(dateValue, currentDateTime, count, is12HoursFormat);
            count = 0;
            continue;
          case 'M':
            while (index < text.Length && text[index] == 'M')
            {
              ++index;
              ++count;
            }
            dateValue = this.UpdateMonth(dateValue, currentDateTime, count);
            count = 0;
            continue;
          case 'S':
          case 's':
            while (index < text.Length && (text[index] == 's' || text[index] == 'S'))
            {
              ++index;
              ++count;
            }
            dateValue = this.UpdateSecond(dateValue, currentDateTime, count);
            count = 0;
            continue;
          case 'Y':
          case 'y':
            while (index < text.Length && (text[index] == 'y' || text[index] == 'Y'))
            {
              ++index;
              ++count;
            }
            dateValue = this.UpdateYear(dateValue, currentDateTime, count);
            count = 0;
            continue;
          case '\\':
            ++index;
            continue;
          case 'm':
            while (index < text.Length && text[index] == 'm')
            {
              ++index;
              ++count;
            }
            dateValue = this.UpdateMinute(dateValue, currentDateTime, count);
            count = 0;
            continue;
          default:
            dateValue += (string) (object) text[index];
            ++index;
            continue;
        }
      }
    }
    return dateValue;
  }

  private string UpdateDay(string dateValue, DateTime currentDateTime, int count)
  {
    string empty = string.Empty;
    CultureInfo cultureInfo = !this.CharacterFormat.HasValue(73) ? CultureInfo.CurrentCulture : this.GetCulture((LocaleIDs) this.CharacterFormat.LocaleIdASCII);
    string str;
    switch (count)
    {
      case 1:
        str = currentDateTime.Day.ToString();
        break;
      case 2:
        str = Convert.ToInt16(currentDateTime.Day.ToString()) >= (short) 10 ? currentDateTime.Day.ToString() : "0" + currentDateTime.Day.ToString();
        break;
      case 3:
        str = cultureInfo.DateTimeFormat.DayNames[(int) currentDateTime.DayOfWeek].Substring(0, 3);
        break;
      default:
        str = cultureInfo.DateTimeFormat.DayNames[(int) currentDateTime.DayOfWeek];
        break;
    }
    dateValue += str;
    return dateValue;
  }

  internal CultureInfo GetCulture(LocaleIDs localID)
  {
    string name = string.Empty;
    if (Enum.IsDefined(typeof (LocaleIDs), (object) localID))
    {
      name = localID != LocaleIDs.es_ES_tradnl ? localID.ToString().Replace('_', '-') : "es-ES_tradnl";
    }
    else
    {
      switch ((short) localID)
      {
        case 1:
          name = "ar-SA";
          break;
        case 2:
          name = "bg-BG";
          break;
        case 3:
          name = "ca-ES";
          break;
        case 4:
          name = "zh-TW";
          break;
        case 5:
          name = "cs-CZ";
          break;
        case 6:
          name = "da-DK";
          break;
        case 7:
          name = "de-DE";
          break;
        case 8:
          name = "el-GR";
          break;
        case 9:
          name = "en-US";
          break;
        case 10:
          name = "es-ES_tradnl";
          break;
        case 11:
          name = "fi-FI";
          break;
        case 12:
          name = "fr-FR";
          break;
        case 13:
          name = "he-IL";
          break;
        case 14:
          name = "hu-HU";
          break;
        case 15:
          name = "is-IS";
          break;
        case 16 /*0x10*/:
          name = "it-IT";
          break;
        case 17:
          name = "ja-JP";
          break;
        case 18:
          name = "ko-KR";
          break;
        case 19:
          name = "nl-NL";
          break;
        case 20:
          name = "nb-NO";
          break;
        case 21:
          name = "pl-PL";
          break;
        case 22:
          name = "pt-BR";
          break;
        case 23:
          name = "rm-CH";
          break;
        case 24:
          name = "ro-RO";
          break;
        case 25:
          name = "ru-RU";
          break;
        case 26:
          name = "hr-HR";
          break;
        case 27:
          name = "sk-SK";
          break;
        case 28:
          name = "sq-AL";
          break;
        case 29:
          name = "sv-SE";
          break;
        case 30:
          name = "th-TH";
          break;
        case 31 /*0x1F*/:
          name = "tr-TR";
          break;
        case 32 /*0x20*/:
          name = "ur-PK";
          break;
        case 33:
          name = "id-ID";
          break;
        case 34:
          name = "uk-UA";
          break;
        case 35:
          name = "be-BY";
          break;
        case 36:
          name = "sl-SI";
          break;
        case 37:
          name = "et-EE";
          break;
        case 38:
          name = "lv-LV";
          break;
        case 39:
          name = "lt-LT";
          break;
        case 40:
          name = "tg-Cyrl-TJ";
          break;
        case 41:
          name = "fa-IR";
          break;
        case 42:
          name = "vi-VN";
          break;
        case 43:
          name = "hy-AM";
          break;
        case 44:
          name = "az-Latn-AZ";
          break;
        case 45:
          name = "eu-Es";
          break;
        case 46:
          name = "hsb-DE";
          break;
        case 47:
          name = "mk-MK";
          break;
        case 50:
          name = "tn-ZA";
          break;
        case 52:
          name = "xh-ZA";
          break;
        case 53:
          name = "zu-ZA";
          break;
        case 54:
          name = "af-ZA";
          break;
        case 55:
          name = "ka-GE";
          break;
        case 56:
          name = "fo-FO";
          break;
        case 57:
          name = "hi-IN";
          break;
        case 58:
          name = "mt-MT";
          break;
        case 59:
          name = "se-NO";
          break;
        case 62:
          name = "ms-MY";
          break;
        case 63 /*0x3F*/:
          name = "kk-KZ";
          break;
        case 64 /*0x40*/:
          name = "ky-KG";
          break;
        case 65:
          name = "sw-KE";
          break;
        case 66:
          name = "tk-TM";
          break;
        case 67:
          name = "uz-Latn-UZ";
          break;
        case 68:
          name = "tt-RU";
          break;
        case 69:
          name = "bn-IN";
          break;
        case 70:
          name = "pa-IN";
          break;
        case 71:
          name = "gu-IN";
          break;
        case 72:
          name = "or-IN";
          break;
        case 73:
          name = "ta-IN";
          break;
        case 74:
          name = "te-IN";
          break;
        case 75:
          name = "kn-IN";
          break;
        case 76:
          name = "ml-IN";
          break;
        case 78:
          name = "mr-IN";
          break;
        case 79:
          name = "sa-IN";
          break;
        case 80 /*0x50*/:
          name = "mn-MN";
          break;
        case 81:
          name = "bo-CN";
          break;
        case 82:
          name = "cy-GB";
          break;
        case 84:
          name = "lo-LA";
          break;
        case 86:
          name = "gl-ES";
          break;
        case 87:
          name = "kok-IN";
          break;
        case 90:
          name = "syr-SY";
          break;
        case 91:
          name = "si-LK";
          break;
        case 92:
          name = "chr-US";
          break;
        case 93:
          name = "iu-Cans-CA";
          break;
        case 94:
          name = "am-ET";
          break;
        case 97:
          name = "ne-NP";
          break;
        case 98:
          name = "fy-NL";
          break;
        case 99:
          name = "ps-AF";
          break;
        case 100:
          name = "fil-PH";
          break;
        case 101:
          name = "dv-MV";
          break;
        case 103:
          name = "ff-NG";
          break;
        case 104:
          name = "ha-Latn-NG";
          break;
        case 107:
          name = "quz-BO";
          break;
        case 108:
          name = "nso-ZA";
          break;
        case 109:
          name = "ba-RU";
          break;
        case 110:
          name = "lb-LU";
          break;
        case 111:
          name = "kl-GL";
          break;
        case 112 /*0x70*/:
          name = "ig-NG";
          break;
        case 115:
          name = "ti-ET";
          break;
        case 117:
          name = "haw-US";
          break;
        case 120:
          name = "ii-CN";
          break;
        case 122:
          name = "arn-CL";
          break;
        case 126:
          name = "br-FR";
          break;
        case 128 /*0x80*/:
          name = "ug-CN";
          break;
        case 129:
          name = "mi-NZ";
          break;
        case 130:
          name = "oc-FR";
          break;
        case 131:
          name = "co-FR";
          break;
        case 132:
          name = "gsw-FR";
          break;
        case 133:
          name = "sah-RU";
          break;
        case 134:
          name = "qut-GT";
          break;
        case 135:
          name = "rw-Rw";
          break;
        case 140:
          name = "prs-AF";
          break;
      }
    }
    return !string.IsNullOrEmpty(name) ? new CultureInfo(name) : CultureInfo.CurrentCulture;
  }

  private string UpdateMonth(string dateValue, DateTime currentDateTime, int count)
  {
    string empty = string.Empty;
    CultureInfo cultureInfo = !this.CharacterFormat.HasValue(73) ? CultureInfo.CurrentCulture : this.GetCulture((LocaleIDs) this.CharacterFormat.LocaleIdASCII);
    string str;
    switch (count)
    {
      case 1:
        str = currentDateTime.Month.ToString();
        break;
      case 2:
        str = Convert.ToInt16(currentDateTime.Month.ToString()) >= (short) 10 ? currentDateTime.Month.ToString() : "0" + currentDateTime.Month.ToString();
        break;
      case 3:
        str = cultureInfo.DateTimeFormat.MonthNames[currentDateTime.Month - 1].Substring(0, 3);
        break;
      default:
        str = cultureInfo.DateTimeFormat.MonthNames[currentDateTime.Month - 1];
        break;
    }
    dateValue += str;
    return dateValue;
  }

  private string UpdateYear(string dateValue, DateTime currentDateTime, int count)
  {
    string empty = string.Empty;
    string str;
    switch (count)
    {
      case 1:
        str = currentDateTime.Year.ToString().Remove(0, 2);
        break;
      case 2:
        str = currentDateTime.Year.ToString().Remove(0, 2);
        break;
      case 4:
        str = currentDateTime.Year.ToString();
        break;
      default:
        str = currentDateTime.Year.ToString();
        break;
    }
    dateValue += str;
    return dateValue;
  }

  private string UpdateHour(
    string dateValue,
    DateTime currentDateTime,
    int count,
    bool is12HoursFormat)
  {
    int hour = currentDateTime.Hour;
    string str = hour.ToString();
    if (is12HoursFormat && hour > 12)
    {
      hour -= 12;
      str = hour.ToString();
    }
    if (count == 2 && hour < 10)
      str = "0" + str;
    dateValue += str;
    return dateValue;
  }

  private string UpdateMinute(string dateValue, DateTime currentDateTime, int count)
  {
    string empty = string.Empty;
    string str;
    switch (count)
    {
      case 1:
        str = currentDateTime.Minute.ToString();
        break;
      case 2:
        str = Convert.ToInt16(currentDateTime.Minute.ToString()) >= (short) 10 ? currentDateTime.Minute.ToString() : "0" + currentDateTime.Minute.ToString();
        break;
      default:
        str = currentDateTime.Minute.ToString();
        break;
    }
    dateValue += str;
    return dateValue;
  }

  private string UpdateSecond(string dateValue, DateTime currentDateTime, int count)
  {
    string empty = string.Empty;
    string str;
    switch (count)
    {
      case 1:
        str = currentDateTime.Second.ToString();
        break;
      case 2:
        str = Convert.ToInt16(currentDateTime.Second.ToString()) >= (short) 10 ? currentDateTime.Second.ToString() : "0" + currentDateTime.Second.ToString();
        break;
      default:
        str = currentDateTime.Second.ToString();
        break;
    }
    dateValue += str;
    return dateValue;
  }

  internal void CheckFieldSeparator()
  {
    if (this.FieldSeparator == null)
    {
      WFieldMark wfieldMark = new WFieldMark((IWordDocument) this.Document, FieldMarkType.FieldSeparator);
      this.FieldEnd.OwnerParagraph.Items.Insert(this.FieldEnd.GetIndexInOwnerCollection(), (IEntity) wfieldMark);
      this.FieldSeparator = wfieldMark;
      this.IsFieldRangeUpdated = false;
    }
    else
    {
      if (this.FieldType == FieldType.FieldDate || this.FieldSeparator.NextSibling == null)
        return;
      this.m_resultFormat = (this.FieldSeparator.NextSibling as ParagraphItem).ParaItemCharFormat.CloneInt() as WCharacterFormat;
    }
  }

  private void GetFormattingForHyperLink()
  {
    if (this.FieldEnd.PreviousSibling == null)
      this.m_resultFormat = this.FieldEnd.ParaItemCharFormat.CloneInt() as WCharacterFormat;
    else
      this.m_resultFormat = (this.FieldEnd.PreviousSibling as ParagraphItem).ParaItemCharFormat.CloneInt() as WCharacterFormat;
  }

  protected Entity GetClonedTable(Entity entity)
  {
    Entity clonedTable = entity.Clone();
    WTable wtable1 = clonedTable as WTable;
    WTable wtable2 = entity as WTable;
    for (int index1 = 0; index1 < wtable2.Rows.Count; ++index1)
    {
      for (int index2 = 0; index2 < wtable2.Rows[index1].Cells.Count; ++index2)
        this.UpdateClonedTextBodyItem((WTextBody) wtable2.Rows[index1].Cells[index2], (WTextBody) wtable1.Rows[index1].Cells[index2]);
    }
    return clonedTable;
  }

  private Entity GetClonedContentControl(Entity entity)
  {
    Entity clonedContentControl = entity.Clone();
    BlockContentControl blockContentControl1 = clonedContentControl as BlockContentControl;
    BlockContentControl blockContentControl2 = entity as BlockContentControl;
    for (int index = 0; index < blockContentControl2.TextBody.Count; ++index)
      this.UpdateClonedTextBodyItem(blockContentControl2.TextBody, blockContentControl1.TextBody);
    return clonedContentControl;
  }

  private void UpdateClonedTextBodyItem(WTextBody source, WTextBody destination)
  {
    destination.Items.Clear();
    int itemIndex = 0;
    for (int index = 0; index < source.Items.Count; ++index)
    {
      Entity nextItem = (Entity) null;
      if (source.Items[index] is WParagraph)
      {
        foreach (Entity entity in this.GetClonedParagraph((Entity) source.Items[index], (string) null, ref nextItem, ref itemIndex))
          destination.Items.Add((IEntity) entity);
        if (nextItem != null)
        {
          index = source.Items.IndexOf((IEntity) nextItem);
          if (itemIndex > 0)
            --index;
          this.m_nestedFields.Clear();
          this.IsSkip = false;
        }
      }
      else if (source.Items[index] is BlockContentControl)
        destination.Items.Add((IEntity) this.GetClonedContentControl((Entity) source.Items[index]));
      else
        destination.Items.Add((IEntity) this.GetClonedTable((Entity) source.Items[index]));
    }
  }

  private List<Entity> GetClonedParagraph(
    Entity entity,
    string txt,
    ref Entity nextItem,
    ref int itemIndex)
  {
    List<Entity> itemsToUpdate = new List<Entity>();
    WParagraph paragraph = entity.Clone() as WParagraph;
    paragraph.ClearItems();
    if (txt != null && txt.EndsWith("\r"))
      txt = txt.Remove(txt.Length - 1);
    string empty = string.Empty;
    string str = txt;
    itemIndex = this.GetParagraphItemIndex(entity, txt, itemIndex);
    bool flag1 = false;
    for (int index = itemIndex; index < (entity as WParagraph).Items.Count; ++index)
    {
      ParagraphItem field = (entity as WParagraph).Items[index];
      Entity entity1 = field.Clone();
      bool isFieldSeparator = this.IsFieldSeparator;
      this.IsFieldSeparator = false;
      string text = this.UpdateTextForParagraphItem((Entity) (entity as WParagraph).Items[index], true);
      this.IsFieldSeparator = isFieldSeparator;
      bool flag2 = false;
      if ((field is WField && ((field as WField).FieldSeparator != null || (field as WField).FieldType == FieldType.FieldAutoNum) && (field as WField).FieldEnd != null || field is WIfField && (field as WField).FieldEnd != null) && (field as WField).FieldType != FieldType.FieldHyperlink)
      {
        bool isResultFound = false;
        if ((field as WField).FieldSeparator == null && (field as WField).FieldType == FieldType.FieldAutoNum)
        {
          WTextRange wtextRange = new WTextRange((IWordDocument) field.Document);
          wtextRange.Text = text;
          wtextRange.CharacterFormat.ImportContainer((FormatBase) (field as WField).CharacterFormat);
          wtextRange.CharacterFormat.CopyProperties((FormatBase) (field as WField).CharacterFormat);
          if (wtextRange.CharacterFormat.HasValue(106))
            wtextRange.CharacterFormat.PropertiesHash.Remove(106);
          paragraph.Items.Add((IEntity) wtextRange);
        }
        else if (field is WDropDownFormField)
        {
          WTextRange wtextRange = new WTextRange((IWordDocument) field.Document);
          wtextRange.Text = (field as WDropDownFormField).DropDownValue;
          wtextRange.CharacterFormat.ImportContainer((FormatBase) (field as WDropDownFormField).CharacterFormat);
          paragraph.Items.Add((IEntity) wtextRange);
        }
        else
          flag2 = this.UpdateFieldItems(field, entity1, ref paragraph, ref itemsToUpdate, ref itemIndex, ref isResultFound);
        nextItem = (field as WField).Range.Items[(field as WField).Range.Items.Count - 1] as Entity;
        if (nextItem is WParagraph && flag2 && this.IsSkipToAddEmptyPara(paragraph, field as WField, isResultFound))
          flag1 = true;
      }
      else if (txt != null && entity1 is WTextRange)
      {
        if (index == itemIndex && text.Contains(txt) && !this.StartsWithExt(text, txt))
          text = txt;
        if (text.Contains(str) && !str.Contains(text))
          text = str;
        (entity1 as WTextRange).Text = text;
      }
      if (!string.IsNullOrEmpty(str))
        str = str.Substring(text.Length);
      if (!flag2)
      {
        switch (entity1)
        {
          case WField _:
          case BookmarkStart _:
          case BookmarkEnd _:
          case WFieldMark _:
label_24:
            if (str == null || !(str == string.Empty))
            {
              if (nextItem != null)
              {
                index = (entity as WParagraph).Items.IndexOf((IEntity) nextItem);
                this.m_nestedFields.Clear();
                this.IsSkip = false;
                nextItem = (Entity) null;
                continue;
              }
              continue;
            }
            goto label_29;
          default:
            paragraph.Items.Add((IEntity) entity1);
            goto label_24;
        }
      }
      else
        break;
    }
label_29:
    if (((entity as WParagraph).Items.Count == 0 || (entity as WParagraph).Items.Count > 0) && !flag1)
      itemsToUpdate.Add((Entity) paragraph);
    return itemsToUpdate;
  }

  private bool UpdateFieldItems(
    ParagraphItem item,
    Entity entity,
    ref WParagraph paragraph,
    ref List<Entity> itemsToUpdate,
    ref int itemIndex,
    ref bool isResultFound)
  {
    bool flag = false;
    int num = 0;
    WField field = item as WField;
    if (field.Range.Items.Contains((object) field.FieldSeparator))
      num = field.Range.Items.IndexOf((object) field.FieldSeparator) + 1;
    else if (field.Range.Items.Contains((object) field.FieldSeparator.OwnerParagraph))
      num = field.Range.Items.IndexOf((object) field.FieldSeparator.OwnerParagraph);
    for (int index = num; index < field.Range.Items.Count; ++index)
    {
      if (field.Range.Items[index] == field.FieldSeparator.OwnerParagraph)
      {
        WParagraph wparagraph = field.Range.Items[index] as WParagraph;
        if (wparagraph.LastItem != field.FieldSeparator)
        {
          if (index != field.Range.Items.Count - 1 || wparagraph.Items.Count <= 0 || wparagraph.Items[0] != field.FieldEnd)
          {
            for (Entity nextSibling = field.FieldSeparator.NextSibling as Entity; nextSibling != null; nextSibling = nextSibling.NextSibling as Entity)
            {
              if (nextSibling == field.FieldEnd)
              {
                itemIndex = field.FieldEnd.Index < field.FieldEnd.OwnerParagraph.Items.Count - 1 ? field.FieldEnd.Index + 1 : 0;
                break;
              }
              isResultFound = true;
              paragraph.Items.Add((IEntity) nextSibling.Clone());
            }
            entity = (Entity) paragraph;
          }
          else
            break;
        }
        else
          continue;
      }
      else if (field.Range.Items[index] is WParagraph && field.Range.Items[index] == field.FieldEnd.OwnerParagraph)
      {
        if (field.FieldSeparator.OwnerParagraph != field.FieldEnd.OwnerParagraph)
        {
          if (paragraph.ChildEntities.Count > 0)
            itemsToUpdate.Add((Entity) paragraph);
          flag = true;
          paragraph = field.FieldEnd.OwnerParagraph.Clone() as WParagraph;
          paragraph.ClearItems();
        }
        int count = paragraph.ChildEntities.Count > 0 ? paragraph.ChildEntities.Count : 0;
        Entity previousSibling = field.FieldEnd.PreviousSibling as Entity;
        itemIndex = field.FieldEnd.Index < field.FieldEnd.OwnerParagraph.Items.Count - 1 ? field.FieldEnd.Index + 1 : 0;
        for (; previousSibling != null && previousSibling != field.FieldSeparator; previousSibling = previousSibling.PreviousSibling as Entity)
        {
          isResultFound = true;
          paragraph.Items.Insert(count, (IEntity) previousSibling.Clone());
        }
        entity = (Entity) paragraph;
      }
      else
        entity = (field.Range.Items[index] as Entity).Clone();
      switch (entity)
      {
        case TextBodyItem _:
          flag = true;
          if (!(entity is WParagraph) || entity is WParagraph && !this.IsSkipToAddEmptyPara(entity as WParagraph, field, isResultFound))
          {
            itemsToUpdate.Add(entity);
            break;
          }
          break;
        case ParagraphItem _:
          paragraph.Items.Add((IEntity) entity);
          break;
      }
      if (index == field.Range.Items.Count - 2 && field.Range.Items[field.Range.Items.Count - 1] is WFieldMark && (field.Range.Items[field.Range.Items.Count - 1] as WFieldMark).Type == FieldMarkType.FieldEnd)
        break;
    }
    return flag;
  }

  private bool IsSkipToAddEmptyPara(WParagraph paragraph, WField field, bool isResultFound)
  {
    return paragraph != null && paragraph.ChildEntities.Count == 0 && field.Index != 0 && !isResultFound;
  }

  private int GetParagraphItemIndex(Entity entity, string txt, int index)
  {
    int paragraphItemIndex = index;
    string empty = string.Empty;
    if (txt != null)
    {
      bool isFieldSeparator = this.IsFieldSeparator;
      this.IsFieldSeparator = false;
      string str = this.UpdateTextForTextBodyItem(entity, true);
      this.IsFieldSeparator = isFieldSeparator;
      if (str.EndsWith("\r"))
        str = str.Remove(str.Length - 1);
      int startIndex = str.IndexOf(txt);
      if (startIndex > 0)
      {
        for (int index1 = 0; index1 < (entity as WParagraph).Items.Count; ++index1)
        {
          empty += this.UpdateTextForParagraphItem((Entity) (entity as WParagraph).Items[index1], true);
          if (startIndex == empty.Length)
          {
            paragraphItemIndex = index1 + 1;
            break;
          }
          if (startIndex < empty.Length)
          {
            txt = empty.Substring(startIndex);
            paragraphItemIndex = index1;
            break;
          }
        }
      }
    }
    return paragraphItemIndex;
  }

  private int GetStartItemIndex(int index, ref string text)
  {
    string empty1 = string.Empty;
    this.IsFieldSeparator = false;
    this.m_nestedFields.Clear();
    this.IsSkip = false;
    string empty2 = string.Empty;
    for (int index1 = 0; index1 < this.Range.Items.Count; ++index1)
    {
      Entity entity = this.Range.Items[index1] as Entity;
      string str = !(entity is ParagraphItem) ? this.UpdateTextForTextBodyItem(entity, true) : this.UpdateTextForParagraphItem(entity, true);
      empty1 += str;
      if (!this.IsFieldSeparator)
      {
        if (index == empty1.Length)
        {
          index = index1 + 1;
          break;
        }
        if (index < empty1.Length)
        {
          if (this.IsSkip)
          {
            WField wfield = this.m_nestedFields.Pop();
            if (wfield.FieldSeparator.GetIndexInOwnerCollection() == wfield.FieldSeparator.OwnerParagraph.Items.Count - 1)
            {
              index = this.Range.Items.IndexOf((object) wfield.FieldSeparator.OwnerParagraph) + 1;
              break;
            }
            text = empty1.Substring(index);
            index = this.Range.Items.IndexOf((object) wfield.FieldSeparator.OwnerParagraph);
            break;
          }
          text = empty1.Substring(index);
          index = index1;
          break;
        }
      }
      else
        break;
    }
    this.IsFieldSeparator = false;
    this.m_nestedFields.Clear();
    this.IsSkip = false;
    return index;
  }

  private void MergeFieldMarkParagraphs()
  {
    WParagraph ownerParagraph = this.FieldEnd.OwnerParagraph;
    int count = this.OwnerParagraph.ChildEntities.Count;
    this.OwnerParagraph.ParagraphFormat.CopyFormat((FormatBase) ownerParagraph.ParagraphFormat);
    this.OwnerParagraph.BreakCharacterFormat.CopyFormat((FormatBase) ownerParagraph.BreakCharacterFormat);
    this.OwnerParagraph.ListFormat.CopyFormat((FormatBase) ownerParagraph.ListFormat);
    for (int index = ownerParagraph.ChildEntities.Count - 1; index >= 0; --index)
    {
      this.OwnerParagraph.ChildEntities.Insert(count, (IEntity) ownerParagraph.ChildEntities[index]);
      if (this.OwnerParagraph.ChildEntities[count] == this.FieldEnd)
        this.FieldEnd = this.OwnerParagraph.ChildEntities[count] as WFieldMark;
    }
    ownerParagraph.RemoveSelf();
  }

  internal void RemoveFieldResult() => this.RemovePreviousResult();

  protected void RemovePreviousResult()
  {
    for (int index = this.Range.Count - 1; index >= 0; --index)
    {
      Entity paragraph = this.Range.Items[index] as Entity;
      if (paragraph != this.FieldEnd)
      {
        if (paragraph != this.FieldSeparator)
        {
          switch (paragraph)
          {
            case ParagraphItem _:
              this.Range.Items.Remove((object) paragraph);
              this.OwnerParagraph.Items.Remove((IEntity) paragraph);
              continue;
            case WParagraph _:
              WTextBody ownerTextBody1 = (paragraph as WParagraph).OwnerTextBody;
              if (!this.IsFieldSeparator)
                this.CheckPragragh(paragraph as WParagraph);
              if (!this.IsFieldSeparator)
              {
                if ((paragraph as WParagraph).Items.Count == 0)
                {
                  this.Range.Items.Remove((object) paragraph);
                  ownerTextBody1.Items.Remove((IEntity) paragraph);
                  continue;
                }
                continue;
              }
              goto label_14;
            case WTable _:
              WTextBody ownerTextBody2 = (paragraph as WTable).OwnerTextBody;
              if (!this.IsFieldSeparator)
              {
                this.Range.Items.Remove((object) paragraph);
                ownerTextBody2.Items.Remove((IEntity) paragraph);
                continue;
              }
              continue;
            default:
              continue;
          }
        }
        else
          break;
      }
    }
label_14:
    this.IsFieldRangeUpdated = false;
  }

  private void CheckPragragh(WParagraph paragraph)
  {
    for (int index = paragraph.Items.Count - 1; index >= 0; --index)
    {
      ParagraphItem paragraphItem = paragraph.Items[index];
      if (this.FieldEnd.OwnerParagraph == paragraph && index > paragraph.Items.IndexOf((IEntity) this.FieldEnd))
        index = paragraph.Items.IndexOf((IEntity) this.FieldEnd);
      else if (paragraphItem != this.FieldEnd)
      {
        if (paragraphItem == this.FieldSeparator)
        {
          this.IsFieldSeparator = true;
          break;
        }
        if (!this.IsFieldSeparator)
          paragraph.Items.Remove((IEntity) paragraphItem);
      }
    }
  }

  private void UpdateParagraphText(WParagraph paragraph, bool isLastItem, ref string result)
  {
    bool isFieldSeparator = this.IsFieldSeparator;
    this.IsFieldSeparator = false;
    result = this.UpdateTextForTextBodyItem((Entity) paragraph, true) + result;
    this.IsFieldSeparator = isFieldSeparator;
    if (!isLastItem)
      return;
    result = result.TrimEnd('\r');
  }

  private string GetParagraphItemText(ParagraphItem item)
  {
    string paragraphItemText = string.Empty;
    switch (item)
    {
      case WField _:
        if ((item as WField).FieldType == FieldType.FieldMergeField || (item as WField).FieldType == FieldType.FieldHyperlink)
        {
          paragraphItemText = (item as WField).Text;
          break;
        }
        if (!(item as WField).IsUpdated && item != this)
          (item as WField).Update();
        paragraphItemText = (item as WField).FieldResult;
        break;
      case WTextRange _:
        paragraphItemText = (item as WTextRange).Text;
        break;
    }
    return paragraphItemText;
  }

  internal void UpdateFieldResult(string text) => this.UpdateFieldResult(text, false);

  internal void UpdateFieldResult(string text, bool isFromHyperLink)
  {
    if (this.IsDeleteRevision)
      return;
    this.FieldResult = text;
    if (!(this.Owner is WParagraph) || this.FieldEnd == null || !(this.FieldEnd.Owner is WParagraph))
      return;
    this.CheckFieldSeparator();
    if (isFromHyperLink)
      this.GetFormattingForHyperLink();
    this.RemovePreviousResult();
    if (isFromHyperLink && this.OwnerParagraph != this.FieldEnd.OwnerParagraph)
      this.MergeFieldMarkParagraphs();
    text = text.Replace(ControlChar.CrLf, ControlChar.ParagraphBreak);
    text = text.Replace(ControlChar.LineFeedChar, '\r');
    if (this.OwnerParagraph == this.FieldEnd.OwnerParagraph)
    {
      int num = text.IndexOf('\r');
      if (num != -1)
      {
        string text1 = text.Substring(num);
        text = text.Substring(0, num);
        this.OwnerParagraph.Items.Insert(this.FieldEnd.GetIndexInOwnerCollection(), (IEntity) this.GetTextRange(text));
        this.OwnerParagraph.Items.Insert(this.FieldEnd.GetIndexInOwnerCollection(), (IEntity) this.GetTextRange(text1));
      }
      else
      {
        WTextRange textRange = this.GetTextRange(text);
        int count = textRange.RevisionsInternal.Count;
        this.OwnerParagraph.Items.Insert(this.FieldEnd.GetIndexInOwnerCollection(), (IEntity) textRange);
        if (textRange.RevisionsInternal.Count - count > 0 && this.IsInsertRevision)
        {
          for (int index = 0; index < textRange.RevisionsInternal.Count; ++index)
          {
            if (textRange.RevisionsInternal[index].Range.Items.IndexOf((object) textRange) != -1)
            {
              textRange.RevisionsInternal[index].Range.Items.Remove((object) textRange);
              Revision revision = textRange.RevisionsInternal[index];
              foreach (object obj in (IEnumerable) revision.Range.Items)
              {
                if (obj is Entity entity && entity.EntityType == EntityType.FieldMark && (entity as WFieldMark).Type == FieldMarkType.FieldEnd && entity as WFieldMark == this.FieldEnd)
                {
                  revision.Range.InnerList.Insert(revision.Range.InnerList.IndexOf((object) entity), (object) textRange);
                  break;
                }
              }
            }
          }
        }
      }
    }
    else
    {
      string[] strArray = text.Split('\r');
      for (int index = 0; index < strArray.Length; ++index)
      {
        WTextRange wtextRange = new WTextRange((IWordDocument) this.Document);
        if (this.m_resultFormat != null)
        {
          wtextRange.CharacterFormat.ImportContainer((FormatBase) this.m_resultFormat);
          wtextRange.CharacterFormat.CopyProperties((FormatBase) this.m_resultFormat);
          this.m_resultFormat = (WCharacterFormat) null;
        }
        else
        {
          WCharacterFormat format = this.FieldType == FieldType.FieldDate || this.FieldType == FieldType.FieldTime ? this.GetFirstFieldCodeItem().CharacterFormat : this.CharacterFormat;
          wtextRange.CharacterFormat.ImportContainer((FormatBase) format);
          wtextRange.CharacterFormat.CopyProperties((FormatBase) format);
        }
        if (wtextRange.CharacterFormat.HasValue(106))
          wtextRange.CharacterFormat.PropertiesHash.Remove(106);
        wtextRange.Text = strArray[index];
        if (index == 0)
          this.FieldSeparator.OwnerParagraph.Items.Add((IEntity) wtextRange);
        else if (index == strArray.Length - 1)
        {
          this.FieldEnd.OwnerParagraph.Items.Insert(this.FieldEnd.GetIndexInOwnerCollection(), (IEntity) wtextRange);
        }
        else
        {
          WParagraph wparagraph = this.FieldEnd.OwnerParagraph.Clone() as WParagraph;
          wparagraph.ClearItems();
          this.FieldEnd.OwnerParagraph.OwnerTextBody.Items.Insert(this.FieldEnd.OwnerParagraph.GetIndexInOwnerCollection(), (IEntity) wparagraph);
          wparagraph.Items.Add((IEntity) wtextRange);
        }
      }
    }
    this.IsFieldRangeUpdated = false;
  }

  protected WTextRange GetTextRange(string text)
  {
    WTextRange textRange = new WTextRange((IWordDocument) this.Document);
    if (this.m_resultFormat != null)
    {
      textRange.CharacterFormat.ImportContainer((FormatBase) this.m_resultFormat);
      textRange.CharacterFormat.CopyProperties((FormatBase) this.m_resultFormat);
      this.m_resultFormat = (WCharacterFormat) null;
    }
    else
    {
      WCharacterFormat format = this.FieldType == FieldType.FieldDate || this.FieldType == FieldType.FieldTime ? this.GetFirstFieldCodeItem().CharacterFormat : this.CharacterFormat;
      textRange.CharacterFormat.ImportContainer((FormatBase) format);
      textRange.CharacterFormat.CopyProperties((FormatBase) format);
    }
    if (textRange.CharacterFormat.HasValue(106))
      textRange.CharacterFormat.PropertiesHash.Remove(106);
    textRange.Text = text;
    return textRange;
  }

  internal void SkipLayoutingOfFieldCode()
  {
    this.IsFieldSeparator = false;
    for (int index1 = 0; index1 < this.Range.Items.Count; ++index1)
    {
      Entity entity = this.Range.Items[index1] as Entity;
      if (entity is ParagraphItem)
      {
        if (entity is WField wfield && DocumentLayouter.IsLayoutingHeaderFooter)
        {
          if (this.FieldType == FieldType.FieldIf || this.FieldType == FieldType.FieldCompare || this.FieldType == FieldType.FieldFormula)
          {
            if (wfield.FieldType == FieldType.FieldNumPages)
            {
              this.IsNumPagesInsideExpressionField = true;
              wfield.IsNumPageUsedForEvaluation = true;
            }
            else if (wfield.FieldType == FieldType.FieldPage)
              wfield.IsNumPageUsedForEvaluation = true;
          }
          else if (this.FieldType == FieldType.FieldUnknown)
            wfield.IsFieldInsideUnknownField = true;
        }
        this.SkipLayoutingOfParagraphItem(entity);
      }
      else
        this.SkipLayoutingOfTextBodyItem(entity);
      if (!this.IsFieldSeparator || this.FieldType == FieldType.FieldExpression)
      {
        switch (entity)
        {
          case ParagraphItem _ when this.Owner is WParagraph && (this.Owner as WParagraph).LastItem == entity && entity != this.FieldEnd && ((IWidget) (this.Owner as WParagraph).LastItem).LayoutInfo.IsSkip && !(this.Owner as IWidget).LayoutInfo.IsSkip:
            bool flag1 = !this.IsNumPagesInsideExpressionField && DocumentLayouter.IsLayoutingHeaderFooter && (this.FieldType == FieldType.FieldIf || this.FieldType == FieldType.FieldCompare || this.FieldType == FieldType.FieldFormula);
            int inOwnerCollection = this.GetIndexInOwnerCollection();
            if (inOwnerCollection == 0)
            {
              if (flag1)
              {
                (this.Owner as IWidget).LayoutInfo.IsSkip = true;
                break;
              }
              (this.Owner as WParagraph).IsNeedToSkip = true;
              break;
            }
            bool flag2 = true;
            for (int index2 = 0; index2 < inOwnerCollection; ++index2)
            {
              if (!((IWidget) this.OwnerParagraph.Items[index2]).LayoutInfo.IsSkip)
              {
                flag2 = false;
                break;
              }
            }
            if (flag2)
            {
              if (flag1)
              {
                (this.Owner as IWidget).LayoutInfo.IsSkip = true;
                break;
              }
              (this.Owner as WParagraph).IsNeedToSkip = true;
              break;
            }
            break;
          case ParagraphItem _ when !(entity is WFieldMark) && this.FieldType == FieldType.FieldExpression:
            (entity as IWidget).LayoutInfo.IsSkip = true;
            break;
        }
      }
      else
        break;
    }
    this.IsFieldSeparator = false;
  }

  private void SkipMacroButtonFieldCode()
  {
    string empty = string.Empty;
    string str1 = "MACROBUTTON";
    for (int index = 0; index < this.Range.Items.Count; ++index)
    {
      Entity entity = this.Range.Items[index] as Entity;
      if (entity is WTextRange)
      {
        empty += (entity as WTextRange).Text;
        if (empty.ToUpper().Contains(str1))
        {
          string str2 = this.SplitByWhiteSpace(empty);
          if (string.IsNullOrEmpty(str2))
          {
            (entity as IWidget).LayoutInfo.IsSkip = true;
          }
          else
          {
            if ((entity as WTextRange).Text.Trim() == str2)
              break;
            WTextRange wtextRange = new WTextRange((IWordDocument) this.m_doc);
            wtextRange.ApplyCharacterFormat(this.CharacterFormat);
            wtextRange.Text = str2;
            (entity as IWidget).LayoutInfo.IsSkip = true;
            this.OwnerParagraph.Items.Insert(this.Index + 1, (IEntity) wtextRange);
            break;
          }
        }
        else
          (entity as IWidget).LayoutInfo.IsSkip = true;
      }
    }
  }

  private string SplitByWhiteSpace(string macroText)
  {
    string empty = string.Empty;
    bool flag = false;
    macroText = macroText.TrimStart();
    for (int index = 0; index < macroText.Length; ++index)
    {
      if (macroText[index].Equals(ControlChar.SpaceChar))
      {
        if (empty.Length != 0)
        {
          if (flag)
          {
            flag = false;
            macroText = macroText.Remove(0, index + 1);
            break;
          }
          if (empty.ToUpper() == "MACROBUTTON")
            flag = true;
          empty = string.Empty;
        }
      }
      else
        empty += macroText[index].ToString();
    }
    return !string.IsNullOrEmpty(macroText) && (macroText.Trim().ToUpper() == "MACROBUTTON" || flag) ? string.Empty : macroText;
  }

  private void SkipLayoutingOfParagraphItem(Entity entity)
  {
    if (entity is InlineContentControl inlineContentControl)
    {
      for (int index = 0; index <= inlineContentControl.ParagraphItems.Count - 1; ++index)
        this.SkipLayoutingOfParagraphItem((Entity) inlineContentControl.ParagraphItems[index]);
    }
    else
    {
      if (this.IsFieldSeparator)
        return;
      if (this.FieldSeparator == entity || this.FieldEnd == entity)
      {
        this.IsFieldSeparator = true;
      }
      else
      {
        if (entity is WField && (((WField) entity).FieldType == FieldType.FieldPage || ((WField) entity).FieldType == FieldType.FieldNumPages || ((WField) entity).FieldType == FieldType.FieldSectionPages || ((WField) entity).FieldType == FieldType.FieldPageRef) && (DocumentLayouter.m_UpdatingPageFields || DocumentLayouter.IsLayoutingHeaderFooter))
          return;
        if (entity is IWidget widget)
          widget.LayoutInfo.IsSkip = true;
        if (!(entity is WField) || (entity as WField).FieldType != FieldType.FieldIf && (entity as WField).FieldType != FieldType.FieldCompare && (entity as WField).FieldType != FieldType.FieldFormula || !DocumentLayouter.IsLayoutingHeaderFooter)
          return;
        this.IsFieldRangeUpdated = false;
      }
    }
  }

  private void SkipLayoutingOfTextBodyItem(Entity entity)
  {
    switch (entity)
    {
      case WParagraph _:
        if ((entity as WParagraph).ChildEntities.Contains((IEntity) this.FieldSeparator) || (entity as WParagraph).ChildEntities.Contains((IEntity) this.FieldEnd))
        {
          for (int index = 0; index < (entity as WParagraph).Items.Count; ++index)
          {
            this.SkipLayoutingOfParagraphItem((Entity) (entity as WParagraph).Items[index]);
            if (this.IsFieldSeparator)
              break;
          }
          break;
        }
        (entity as IWidget).LayoutInfo.IsSkip = true;
        break;
      case WTable _:
        this.SkipLayoutingOfTable(entity);
        break;
    }
  }

  private void SkipLayoutingOfTable(Entity entity)
  {
    for (int index1 = 0; index1 < (entity as WTable).Rows.Count; ++index1)
    {
      WTableRow row = (entity as WTable).Rows[index1];
      for (int index2 = 0; index2 < row.Cells.Count; ++index2)
      {
        WTableCell cell = row.Cells[index2];
        for (int index3 = 0; index3 < cell.Items.Count; ++index3)
        {
          this.SkipLayoutingOfTextBodyItem((Entity) cell.Items[index3]);
          if (this.IsFieldSeparator)
            return;
        }
      }
    }
    if (this.IsFieldSeparator)
      return;
    (entity as IWidget).LayoutInfo.IsSkip = true;
  }

  private bool SetSkip(int startIndex, EntityCollection items, bool isFieldCode, int itemsCount)
  {
    IEntity entity1 = isFieldCode ? (IEntity) this.FieldSeparator : (IEntity) this.FieldEnd;
    for (int index = startIndex; index < itemsCount; ++index)
    {
      Entity entity2 = items[index];
      switch (entity2)
      {
        case InlineContentControl _ when entity2 == entity1.Owner:
          return this.SetSkip(0, (EntityCollection) (entity2 as InlineContentControl).ParagraphItems, isFieldCode, (entity2 as InlineContentControl).ParagraphItems.Count);
        case WParagraph _ when entity2 == (entity1 as ParagraphItem).OwnerParagraph:
          return this.SetSkip(0, (EntityCollection) (entity2 as WParagraph).Items, isFieldCode, (entity2 as WParagraph).Items.Count);
        case BlockContentControl _:
          if (this.SetSkip(0, (EntityCollection) (entity2 as BlockContentControl).TextBody.Items, isFieldCode, (entity2 as BlockContentControl).TextBody.Items.Count))
            return true;
          break;
        default:
          (entity2 as IWidget).LayoutInfo.IsSkip = true;
          if (entity2 == entity1)
            return true;
          break;
      }
    }
    return false;
  }

  private IEntity GetNextSibling()
  {
    if (this.NextSibling != null)
      return this.NextSibling;
    if (!(this.Owner is InlineContentControl) || this.Owner.NextSibling == null)
      return (IEntity) null;
    return this.Owner.NextSibling is InlineContentControl && (this.Owner.NextSibling as InlineContentControl).ParagraphItems.Count > 0 ? (IEntity) (this.Owner.NextSibling as InlineContentControl).ParagraphItems[0] : this.Owner.NextSibling;
  }

  internal void SkipLayoutingFieldItems(bool isFieldCode)
  {
    Entity entity = (isFieldCode ? this.GetNextSibling() : (IEntity) this.FieldSeparator) as Entity;
    int startIndex = entity.Index;
    if (entity.Owner is InlineContentControl)
    {
      if (this.SetSkip(startIndex, (EntityCollection) (entity.Owner as InlineContentControl).ParagraphItems, isFieldCode, (entity.Owner as InlineContentControl).ParagraphItems.Count))
        return;
      startIndex = entity.Owner.Index + 1;
    }
    WParagraph ownerParagraph = (entity as ParagraphItem).OwnerParagraph;
    if (this.SetSkip(startIndex, (EntityCollection) ownerParagraph.Items, isFieldCode, ownerParagraph.Items.Count))
      return;
    WTextBody ownerTextBody = ownerParagraph.OwnerTextBody;
    int itemsCount = ownerTextBody.Items.Count;
    if (this.FieldType == FieldType.FieldUnknown && this.FieldEnd != null && this.FieldEnd.OwnerParagraph.Owner is WTableCell && (!(this.OwnerParagraph.Owner is WTableCell) || this.FieldEnd.OwnerParagraph.Owner != this.OwnerParagraph.Owner))
    {
      WTable table = this.GetOwnerTable((Entity) this.FieldEnd) as WTable;
      while (table.IsInCell && this.GetOwnerTable((Entity) table) is WTable ownerTable)
        table = ownerTable;
      if (ownerTextBody.Items.Contains((IEntity) table))
        itemsCount = table.Index;
      this.SkipTableItems(table, this.FieldEnd.OwnerParagraph.Owner as WTableCell);
    }
    this.SetSkip(ownerParagraph.Index + 1, (EntityCollection) ownerTextBody.Items, isFieldCode, itemsCount);
  }

  private bool SkipTableItems(WTable table, WTableCell wTableCell)
  {
    foreach (WTableRow row in (CollectionImpl) table.Rows)
    {
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
      {
        bool flag1 = false;
        bool flag2 = false;
        foreach (Entity childEntity in (CollectionImpl) cell.ChildEntities)
        {
          if (!flag1 && childEntity is WParagraph)
            flag1 = true;
          else if (childEntity is WParagraph)
          {
            flag2 = (childEntity as WParagraph).Items.Contains((IEntity) this.FieldEnd);
            if (flag2)
              this.SkipParaItems((childEntity as WParagraph).Items);
            else
              (childEntity as IWidget).LayoutInfo.IsSkip = true;
          }
          else if (childEntity is WTable)
            flag2 = this.SkipTableItems(childEntity as WTable, cell);
        }
        if (flag2)
          return true;
      }
    }
    return false;
  }

  private void SkipParaItems(ParagraphItemCollection items)
  {
    foreach (Entity entity in (CollectionImpl) items)
    {
      if (entity != this.FieldEnd)
        (entity as IWidget).LayoutInfo.IsSkip = true;
    }
  }

  SizeF ILeafWidget.Measure(DrawingContext dc)
  {
    SizeF sizeF = SizeF.Empty;
    WCharacterFormat charFormat = this.GetCharFormat();
    Font fontToRender = charFormat.GetFontToRender(this.ScriptType);
    switch (this.FieldType)
    {
      case FieldType.FieldNumPages:
        string text = string.Empty;
        WCharacterFormat characterFormatValue = this.GetCharacterFormatValue();
        if (DocumentLayouter.IsFirstLayouting)
          this.Text = "";
        else
          text = this.FieldResult;
        sizeF = dc.MeasureString(text, characterFormatValue.GetFontToRender(this.ScriptType), (StringFormat) null, characterFormatValue, false);
        if (text != string.Empty)
        {
          ((IWidget) this).LayoutInfo.Size = sizeF;
          break;
        }
        break;
      case FieldType.FieldExpression:
        sizeF = this.GetEQFieldSize(this.ScriptType, dc, charFormat);
        ((IWidget) this).LayoutInfo.Size = sizeF;
        break;
      case FieldType.FieldDocVariable:
        if (this.FieldSeparator == null)
        {
          string variable = this.Document.Variables[this.FieldValue];
          sizeF = dc.MeasureString(variable, fontToRender, (StringFormat) null, charFormat, false);
          ((IWidget) this).LayoutInfo.Size = sizeF;
          break;
        }
        ((IWidget) this).LayoutInfo.IsSkip = true;
        break;
    }
    return sizeF;
  }

  private SizeF GetEQFieldSize(
    FontScriptType fontScriptType,
    DrawingContext dc,
    WCharacterFormat charFormat)
  {
    EquationField equationField = new EquationField();
    equationField.EQFieldEntity = this;
    equationField.LayouttedEQField = this.LayoutEQfileCode(fontScriptType, this.FieldCode, dc, charFormat, 0.0f, 0.0f);
    DocumentLayouter.EquationFields.Add(equationField);
    return equationField.LayouttedEQField.Bounds.Size;
  }

  private LayoutedEQFields LayoutEQfileCode(
    FontScriptType fontScriptType,
    string fieldCode,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    fieldCode = this.RemoveEQText(fieldCode);
    List<string> stringList = new List<string>();
    LayoutedEQFields layoutedEqFields = new LayoutedEQFields();
    if (this.IsValidEqFieldCode(stringList, fieldCode))
    {
      this.LayoutEquationFieldCode(fontScriptType, layoutedEqFields, stringList, dc, charFormat, xPosition, yPosition);
      this.UpdateltEqFieldsBounds(layoutedEqFields);
    }
    else
      dc.GenerateErrorFieldCode(layoutedEqFields, xPosition, yPosition, charFormat);
    return layoutedEqFields;
  }

  private bool IsValidEqFieldCode(List<string> splittedFieldCodes, string fieldCode)
  {
    this.GetSplittedFieldCode(splittedFieldCodes, fieldCode);
    foreach (string splittedFieldCode in splittedFieldCodes)
    {
      if (!this.IsValidSwitch(splittedFieldCode))
        return false;
    }
    return true;
  }

  private bool IsValidSwitch(string fieldCode)
  {
    switch (this.GetFirstOccurenceEqSwitch(fieldCode).ToLower())
    {
      case "\\a":
        return this.IsValidArraySwitch(fieldCode);
      case "\\f":
        return this.IsValidFractionSwitch(fieldCode);
      case "\\s":
        return this.IsValidSuperscriptSwitch(fieldCode);
      case "\\l":
        return this.IsValidListSwitch(fieldCode);
      case "\\r":
        return this.IsValidRadicalSwitch(fieldCode);
      case "\\i":
        return this.IsValidIntegralSwitch(fieldCode);
      case "\\b":
        return this.IsValidBracketSwitch(fieldCode);
      case "\\x":
        return this.IsValidBoxSwitch(fieldCode);
      case "\\o":
        return this.IsValidOverstrikeSwitch(fieldCode);
      case "\\d":
        return this.IsValidDisplaceSwitch(fieldCode);
      default:
        return !this.HasSlashError(fieldCode) && !fieldCode.Contains("\\ ");
    }
  }

  private LayoutedEQFields LayoutSwitch(
    FontScriptType scriptType,
    string fieldCode,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    switch (this.GetFirstOccurenceEqSwitch(fieldCode).ToLower())
    {
      case "\\a":
        return this.LayoutArraySwitch(scriptType, fieldCode, dc, charFormat, xPosition, yPosition);
      case "\\f":
        return this.LayoutFractionSwitch(scriptType, fieldCode, dc, charFormat, xPosition, yPosition);
      case "\\s":
        return this.LayoutSuperscriptSwitch(scriptType, fieldCode, dc, charFormat, xPosition, yPosition);
      case "\\l":
        return this.LayoutListSwitch(scriptType, fieldCode, dc, charFormat, xPosition, yPosition);
      case "\\r":
        return this.LayoutRadicalSwitch(scriptType, fieldCode, dc, charFormat, xPosition, yPosition);
      case "\\b":
        return this.LayoutBracketSwitch(scriptType, fieldCode, dc, charFormat, xPosition, yPosition);
      case "\\x":
        return this.LayoutBoxSwitch(scriptType, fieldCode, dc, charFormat, xPosition, yPosition);
      case "\\i":
        return this.LayoutIntegralSwitch(scriptType, fieldCode, dc, charFormat, xPosition, yPosition);
      case "\\o":
        return this.LayoutOverstrikeSwitch(scriptType, fieldCode, dc, charFormat, xPosition, yPosition);
      case "\\d":
        return this.LayoutDisplaceSwitch(scriptType, fieldCode, dc, charFormat, xPosition, yPosition);
      default:
        if (!(fieldCode != ""))
          return (LayoutedEQFields) null;
        TextEQField textEqField = new TextEQField();
        this.GenerateTextEQField(textEqField, fieldCode, dc, charFormat.GetFontToRender(scriptType), charFormat, xPosition, yPosition);
        return (LayoutedEQFields) textEqField;
    }
  }

  private void GenerateSwitch(
    FontScriptType fontScriptType,
    LayoutedEQFields ltEqField,
    string fieldCode,
    DrawingContext dc,
    Font font,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    if (this.HasAnySwitch(fieldCode))
    {
      this.GenerateNestedSwitch(fontScriptType, ltEqField, fieldCode, dc, charFormat, xPosition, yPosition);
    }
    else
    {
      TextEQField textEqField = new TextEQField();
      this.GenerateTextEQField(textEqField, fieldCode, dc, font, charFormat, xPosition, yPosition);
      ltEqField.ChildEQFileds.Add((LayoutedEQFields) textEqField);
      this.UpdateltEqFieldsBounds(ltEqField);
    }
  }

  private void GenerateNestedSwitch(
    FontScriptType fontScriptType,
    LayoutedEQFields ltEqField,
    string fieldCode,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    LayoutedEQFields layouttedEqField = new LayoutedEQFields();
    List<string> splittedFieldCodeSwitch = new List<string>();
    this.GetSplittedFieldCode(splittedFieldCodeSwitch, fieldCode);
    this.LayoutEquationFieldCode(fontScriptType, layouttedEqField, splittedFieldCodeSwitch, dc, charFormat, xPosition, yPosition);
    ltEqField.ChildEQFileds.Add(layouttedEqField);
    this.UpdateltEqFieldsBounds(ltEqField);
  }

  private void GenerateTextEQField(
    TextEQField textEqField,
    string fieldCode,
    DrawingContext dc,
    Font font,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    textEqField.Text = this.ReplaceSymbols(fieldCode);
    string text = textEqField.Text;
    if (fieldCode == "Õ".ToString() || fieldCode == "å")
      textEqField.Font = font;
    textEqField.Bounds = new RectangleF(new PointF(xPosition, yPosition), dc.MeasureString(text, font, (StringFormat) null, charFormat, false));
    float ascent = dc.GetAscent(font);
    dc.ShiftEqFieldYPosition((LayoutedEQFields) textEqField, -ascent);
  }

  private LayoutedEQFields LayoutRadicalSwitch(
    FontScriptType fontScriptType,
    string fieldCode,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    string[] elements = this.SplitElementsByComma(fieldCode.Substring(fieldCode.IndexOf("\\r", StringComparison.CurrentCultureIgnoreCase) + 2));
    LayoutedEQFields radicalSwitch = new LayoutedEQFields();
    radicalSwitch.SwitchType = LayoutedEQFields.EQSwitchType.Radical;
    this.GenerateRadicalSwitch(fontScriptType, radicalSwitch, elements, dc, charFormat, xPosition, yPosition);
    return radicalSwitch;
  }

  private void GenerateRadicalSwitch(
    FontScriptType scriptType,
    LayoutedEQFields radicalSwitch,
    string[] elements,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    float outerElementRight = 0.0f;
    float outerElementY = 0.0f;
    if (elements.Length == 1)
    {
      this.GenerateSwitch(scriptType, radicalSwitch, elements[0], dc, charFormat.GetFontToRender(scriptType), charFormat, xPosition, yPosition);
      this.GenerateRadicalSymbol(radicalSwitch, radicalSwitch.ChildEQFileds[0], xPosition, ref outerElementRight, ref outerElementY, charFormat.FontSize);
      radicalSwitch.Bounds = new RectangleF(radicalSwitch.Bounds.X, radicalSwitch.Bounds.Y - 0.7f, radicalSwitch.Bounds.Width, radicalSwitch.Bounds.Height + 0.7f);
    }
    else
    {
      this.GenerateSwitch(scriptType, radicalSwitch, elements[1], dc, charFormat.GetFontToRender(scriptType), charFormat, xPosition, yPosition);
      RectangleF bounds = radicalSwitch.ChildEQFileds[radicalSwitch.ChildEQFileds.Count - 1].Bounds;
      this.GenerateRadicalSymbol(radicalSwitch, radicalSwitch.ChildEQFileds[radicalSwitch.ChildEQFileds.Count - 1], xPosition, ref outerElementRight, ref outerElementY, charFormat.FontSize);
      this.GenerateRadicalOuterElement(scriptType, radicalSwitch, elements[0], dc, charFormat, xPosition, yPosition, outerElementY, outerElementRight, bounds);
    }
  }

  private void GenerateRadicalOuterElement(
    FontScriptType fontScriptType,
    LayoutedEQFields radicalSwitch,
    string element,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition,
    float outerELementY,
    float outerElementRight,
    RectangleF innerElementBounds)
  {
    LayoutedEQFields layoutedEqFields = new LayoutedEQFields();
    this.GenerateSwitch(fontScriptType, layoutedEqFields, element, dc, charFormat.GetFontToRender(fontScriptType), charFormat, xPosition, yPosition);
    float num1 = (float) dc.FontMetric.Descent(charFormat.GetFontToRender(fontScriptType));
    if ((double) innerElementBounds.Height <= (double) layoutedEqFields.Bounds.Height)
      num1 = layoutedEqFields.Bounds.Bottom;
    dc.ShiftEqFieldYPosition(layoutedEqFields, outerELementY);
    outerElementRight -= layoutedEqFields.Bounds.Width;
    this.ShiftEqFieldXPosition(layoutedEqFields, outerElementRight - layoutedEqFields.Bounds.X);
    radicalSwitch.ChildEQFileds.Add(layoutedEqFields);
    float num2 = (double) radicalSwitch.Bounds.Top < (double) layoutedEqFields.Bounds.Top ? radicalSwitch.Bounds.Height : radicalSwitch.Bounds.Height + (radicalSwitch.Bounds.Y - layoutedEqFields.Bounds.Y);
    this.UpdateRadicalBounds(radicalSwitch);
    if ((double) radicalSwitch.Bounds.X < (double) xPosition)
      this.ShiftEqFieldXPosition(radicalSwitch, xPosition - radicalSwitch.Bounds.X);
    this.ShiftRadicalSymbolYPosition(radicalSwitch, charFormat.GetFontToRender(fontScriptType).Size / 24f);
    radicalSwitch.Bounds = new RectangleF(radicalSwitch.Bounds.X, radicalSwitch.Bounds.Y - num1, radicalSwitch.Bounds.Width, num2 + num1);
  }

  private void ShiftRadicalSymbolYPosition(LayoutedEQFields ltEQField, float shiftPosition)
  {
    for (int index = 0; index < ltEQField.ChildEQFileds.Count; ++index)
    {
      if (ltEQField.ChildEQFileds[index] is LineEQField)
      {
        LineEQField childEqFiled = ltEQField.ChildEQFileds[index] as LineEQField;
        childEqFiled.Point1 = new PointF(childEqFiled.Point1.X, childEqFiled.Point1.Y + shiftPosition);
        childEqFiled.Point2 = new PointF(childEqFiled.Point2.X, childEqFiled.Point2.Y + shiftPosition);
      }
    }
  }

  private void UpdateRadicalBounds(LayoutedEQFields ltEQField)
  {
    float x = this.GetLeftMostX(ltEQField);
    if ((double) x > 0.0)
      x = 0.0f;
    float num = -this.GetTopMostY(ltEQField);
    float maximumBottom = this.GetMaximumBottom(ltEQField);
    ltEQField.Bounds = new RectangleF(x, -num, -x + this.GetMaximumRight(ltEQField), num + maximumBottom);
  }

  private void GenerateRadicalSymbol(
    LayoutedEQFields radicalSwitch,
    LayoutedEQFields innerElement,
    float xPosition,
    ref float outerElementRight,
    ref float outerElementY,
    float fontSize)
  {
    float lineThickness = 0.7f;
    float height = 0.0f;
    LineEQField upwardLine = new LineEQField();
    this.GenerateUpwardLine(innerElement, upwardLine, lineThickness);
    LineEQField downwardLine = new LineEQField();
    PointF downwardStart = new PointF(xPosition, 0.0f);
    this.GenerateRadicalDownwardLine(innerElement, upwardLine, downwardLine, lineThickness, ref downwardStart, ref height);
    LineEQField topHorizontalLine = new LineEQField();
    this.GenerateRadicalTopHorizontalLine(innerElement, upwardLine, topHorizontalLine, lineThickness);
    LineEQField rootHook = new LineEQField();
    this.GenerateRadicalHook(innerElement, rootHook, lineThickness, ref height, ref downwardStart);
    radicalSwitch.ChildEQFileds.Add((LayoutedEQFields) rootHook);
    radicalSwitch.ChildEQFileds.Add((LayoutedEQFields) downwardLine);
    radicalSwitch.ChildEQFileds.Add((LayoutedEQFields) upwardLine);
    radicalSwitch.ChildEQFileds.Add((LayoutedEQFields) topHorizontalLine);
    this.UpdateRadicalSwitchBounds(radicalSwitch, innerElement, rootHook, upwardLine, topHorizontalLine, xPosition, lineThickness);
    this.GetOuterElementPositions(rootHook, upwardLine, topHorizontalLine, lineThickness, ref outerElementRight, ref outerElementY, fontSize);
  }

  private void UpdateRadicalSwitchBounds(
    LayoutedEQFields radicalSwitch,
    LayoutedEQFields innerElement,
    LineEQField rootHook,
    LineEQField upwardLine,
    LineEQField topHorizontalLine,
    float xPosition,
    float lineThickness)
  {
    this.ShiftEqFieldXPosition(innerElement, lineThickness);
    float num = rootHook.Point1.X - lineThickness / 4f;
    this.ShiftEqFieldXPosition(radicalSwitch, xPosition - num);
    float x = xPosition;
    float y = topHorizontalLine.Point1.Y - lineThickness / 2f;
    float height = (float) ((double) upwardLine.Point1.Y + -(double) y + (double) lineThickness / 2.0);
    float width = innerElement.Bounds.Right - x;
    radicalSwitch.Bounds = new RectangleF(x, y, width, height);
  }

  private void GenerateUpwardLine(
    LayoutedEQFields innerElement,
    LineEQField upwardLine,
    float lineThickness)
  {
    float widthFromAngle = this.GetWidthFromAngle(innerElement.Bounds.Height, this.DegreeIntoRadians(80.3856f), this.DegreeIntoRadians(9.6143f));
    float x1 = (float) ((double) innerElement.Bounds.Left - (double) widthFromAngle - (double) lineThickness / 2.0);
    float y1 = innerElement.Bounds.Bottom + lineThickness / 2f;
    upwardLine.Point1 = new PointF(x1, y1);
    float x2 = innerElement.Bounds.Left - lineThickness / 2f;
    float y2 = innerElement.Bounds.Top + lineThickness / 4f;
    upwardLine.Point2 = new PointF(x2, y2);
  }

  private void GenerateRadicalDownwardLine(
    LayoutedEQFields innerElement,
    LineEQField upwardLine,
    LineEQField downwardLine,
    float lineThickness,
    ref PointF downwardStart,
    ref float height)
  {
    height = innerElement.Bounds.Height * 0.558f;
    float widthFromAngle = this.GetWidthFromAngle(height, this.DegreeIntoRadians(64.3483f), this.DegreeIntoRadians(25.6516f));
    float x = upwardLine.Point1.X - widthFromAngle;
    float y = innerElement.Bounds.Bottom - innerElement.Bounds.Height * 0.558f;
    downwardStart = new PointF(x, y);
    downwardLine.Point1 = new PointF(x - lineThickness / 4f, y + lineThickness / 4f);
    downwardLine.Point2 = new PointF(upwardLine.Point1.X, upwardLine.Point1.Y);
  }

  private void GenerateRadicalTopHorizontalLine(
    LayoutedEQFields innerElement,
    LineEQField upwardLine,
    LineEQField topHorizontalLine,
    float lineThickness)
  {
    float y = innerElement.Bounds.Top + lineThickness / 4f;
    topHorizontalLine.Point1 = new PointF(upwardLine.Point2.X, y);
    float x = topHorizontalLine.Point1.X + innerElement.Bounds.Width;
    topHorizontalLine.Point2 = new PointF(x, topHorizontalLine.Point1.Y);
  }

  private void GenerateRadicalHook(
    LayoutedEQFields innerElement,
    LineEQField rootHook,
    float lineThickness,
    ref float height,
    ref PointF downwardStart)
  {
    height -= innerElement.Bounds.Height * 0.485f;
    float widthFromAngle = this.GetWidthFromAngle(height, this.DegreeIntoRadians(32.2825f), this.DegreeIntoRadians(57.7174f));
    float x1 = downwardStart.X - widthFromAngle;
    float y1 = innerElement.Bounds.Bottom - innerElement.Bounds.Height * 0.485f;
    rootHook.Point1 = new PointF(x1, y1);
    float x2 = downwardStart.X - lineThickness / 4f;
    float y2 = downwardStart.Y + lineThickness / 4f;
    rootHook.Point2 = new PointF(x2, y2);
  }

  private void GetOuterElementPositions(
    LineEQField rootHook,
    LineEQField upwardLine,
    LineEQField topHorizontalLine,
    float lineThickness,
    ref float outerElementRight,
    ref float outerElementY,
    float fontSize)
  {
    float num1 = (double) fontSize > 24.0 ? 0.5f : 0.6121f;
    float num2 = (upwardLine.Point2.X - (rootHook.Point1.X - lineThickness / 4f)) * num1;
    outerElementRight = rootHook.Point1.X + num2;
    lineThickness *= 1.5f;
    float num3 = (float) (-(double) topHorizontalLine.Point1.Y - -(double) rootHook.Point1.Y) + lineThickness;
    outerElementY = rootHook.Point1.Y - num3 / 2f;
  }

  private float GetWidthFromAngle(float height, double angle1, double angle2)
  {
    return (float) ((double) height / Math.Sin(angle1) * Math.Sin(angle2));
  }

  private double DegreeIntoRadians(float angle) => Math.PI * (double) angle / 180.0;

  private bool IsValidRadicalSwitch(string fieldCode)
  {
    if (this.HasParenthesisError(fieldCode))
      return false;
    string str = fieldCode.Substring(fieldCode.IndexOf("\\r", StringComparison.CurrentCultureIgnoreCase) + 2);
    return this.StartsWithExt(str.TrimStart(), "(") && this.SplitElementsByComma(str).Length <= 2 && this.IsValidEqFieldCode(new List<string>(), str);
  }

  private bool IsValidListSwitch(string fieldCode)
  {
    if (this.HasParenthesisError(fieldCode))
      return false;
    string str = fieldCode.Substring(fieldCode.IndexOf("\\l", StringComparison.CurrentCultureIgnoreCase) + 2);
    return this.StartsWithExt(str.TrimStart(), "(") && this.IsValidEqFieldCode(new List<string>(), str.Substring(1, str.Length - 2));
  }

  private LayoutedEQFields LayoutListSwitch(
    FontScriptType fontScriptType,
    string fieldCode,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    LayoutedEQFields listSwitch = new LayoutedEQFields();
    listSwitch.SwitchType = LayoutedEQFields.EQSwitchType.List;
    this.GenerateListSwitch(fontScriptType, listSwitch, fieldCode, dc, charFormat, xPosition, yPosition);
    return listSwitch;
  }

  private void GenerateListSwitch(
    FontScriptType fontScriptType,
    LayoutedEQFields listSwitch,
    string fieldCode,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    string str = fieldCode.Substring(fieldCode.IndexOf("\\l", StringComparison.CurrentCultureIgnoreCase) + 2);
    string fieldCode1 = str.Substring(1, str.Length - 2);
    this.GenerateSwitch(fontScriptType, listSwitch, fieldCode1, dc, charFormat.GetFontToRender(fontScriptType), charFormat, xPosition, yPosition);
  }

  private bool IsValidSuperscriptSwitch(string fieldCode)
  {
    if (this.HasParenthesisError(fieldCode))
      return false;
    string substringTill = this.GetSubstringTill(fieldCode, '(');
    string fieldCode1 = fieldCode.Substring(fieldCode.IndexOf(substringTill, StringComparison.Ordinal) + substringTill.Length);
    return this.IsCorrectSuperscriptSwitchSequence(substringTill) && this.IsValidEqFieldCode(new List<string>(), fieldCode1);
  }

  private bool IsCorrectSuperscriptSwitchSequence(string superscriptEqCode)
  {
    if (superscriptEqCode.Contains("\\ "))
      return false;
    string[] strArray = superscriptEqCode.Split('\\');
    for (int index = 1; index < strArray.Length; ++index)
    {
      if (strArray[index].StartsWith("up", StringComparison.OrdinalIgnoreCase) || strArray[index].StartsWith("do", StringComparison.OrdinalIgnoreCase))
      {
        if (!this.IsCorrectCodeFormat(strArray[index].Trim(), true))
          return false;
      }
      else if (strArray[index].StartsWith("ai", StringComparison.OrdinalIgnoreCase) || strArray[index].StartsWith("di", StringComparison.OrdinalIgnoreCase))
      {
        if (!this.IsCorrectCodeFormat(strArray[index].Trim(), false))
          return false;
      }
      else if (!strArray[index].TrimEnd().Equals("s", StringComparison.OrdinalIgnoreCase) || index != 1)
        return false;
    }
    return true;
  }

  private bool HasPositiveValue(string inputText)
  {
    int num = inputText.IndexOfAny("0123456789".ToCharArray());
    return num == -1 || inputText[num - 1] != '-';
  }

  private LayoutedEQFields LayoutSuperscriptSwitch(
    FontScriptType fontScriptType,
    string fieldCode,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    string substringTill = this.GetSubstringTill(fieldCode, '(');
    string[] elements = this.SplitElementsByComma(fieldCode.Substring(fieldCode.IndexOf(substringTill, StringComparison.Ordinal) + substringTill.Length));
    LayoutedEQFields scriptSwitch = new LayoutedEQFields();
    scriptSwitch.SwitchType = LayoutedEQFields.EQSwitchType.Superscript;
    this.GenerateSuperscriptSwitch(fontScriptType, scriptSwitch, elements, dc, charFormat, xPosition, yPosition);
    if (elements.Length == 1)
      this.ApplySuperscriptProperties(scriptSwitch, substringTill, dc);
    return scriptSwitch;
  }

  private void GenerateSuperscriptSwitch(
    FontScriptType fontScriptType,
    LayoutedEQFields scriptSwitch,
    string[] elements,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    if (elements.Length == 1)
    {
      this.GenerateSwitch(fontScriptType, scriptSwitch, elements[0], dc, charFormat.GetFontToRender(fontScriptType), charFormat, xPosition, yPosition);
    }
    else
    {
      foreach (string element in elements)
        this.GenerateSwitch(fontScriptType, scriptSwitch, element, dc, charFormat.GetFontToRender(fontScriptType), charFormat, xPosition, yPosition);
      this.AlignColumnWise(scriptSwitch, dc);
      this.UpdateltEqFieldsBounds(scriptSwitch);
    }
  }

  private void AddSpaceBelowLine(LayoutedEQFields layoutedEqFields, int spaceBelowLineValue)
  {
    layoutedEqFields.Bounds = new RectangleF(layoutedEqFields.Bounds.X, layoutedEqFields.Bounds.Y, layoutedEqFields.Bounds.Width, layoutedEqFields.Bounds.Height + (float) spaceBelowLineValue);
  }

  private void AddSpaceAboveLine(LayoutedEQFields layoutedEqFields, int spaceAboveLineValue)
  {
    layoutedEqFields.Bounds = new RectangleF(layoutedEqFields.Bounds.X, layoutedEqFields.Bounds.Y - (float) spaceAboveLineValue, layoutedEqFields.Bounds.Width, layoutedEqFields.Bounds.Height + (float) spaceAboveLineValue);
  }

  private int GetSpaceBelowLineValue(string fieldCode)
  {
    string[] strArray = fieldCode.Split('\\');
    for (int index = strArray.Length - 1; index > 0; --index)
    {
      if (strArray[index].Trim().StartsWith("di", StringComparison.CurrentCultureIgnoreCase))
        return this.GetValueFromstring(strArray[index].Trim());
    }
    return 0;
  }

  private int GetSpaceAboveLineValue(string fieldCode)
  {
    string[] strArray = fieldCode.Split('\\');
    for (int index = strArray.Length - 1; index > 0; --index)
    {
      if (strArray[index].Trim().StartsWith("ai", StringComparison.CurrentCultureIgnoreCase))
        return this.GetValueFromstring(strArray[index].Trim());
    }
    return 0;
  }

  private void AlignColumnWise(LayoutedEQFields ltEqField, DrawingContext dc)
  {
    for (int index = 1; index < ltEqField.ChildEQFileds.Count; ++index)
    {
      float yPosition = ltEqField.ChildEQFileds[index - 1].Bounds.Bottom - ltEqField.ChildEQFileds[index].Bounds.Y;
      dc.ShiftEqFieldYPosition(ltEqField.ChildEQFileds[index], yPosition);
    }
    this.UpdateltEqFieldsBounds(ltEqField);
    float num = ltEqField.Bounds.Y - ltEqField.Bounds.Height / 2f;
    dc.ShiftEqFieldYPosition(ltEqField, num - ltEqField.Bounds.Y);
  }

  private void ApplySuperscriptProperties(
    LayoutedEQFields scriptSwitch,
    string superscriptEqCode,
    DrawingContext dc)
  {
    int upValue = 2;
    int downValue = 2;
    bool isUpValue = true;
    bool isDownValue = false;
    this.GetsSuperOrSubscriptPropertiesValue(superscriptEqCode, ref upValue, ref downValue, ref isUpValue, ref isDownValue);
    int spaceAboveLineValue = this.GetSpaceAboveLineValue(superscriptEqCode);
    int spaceBelowLineValue = this.GetSpaceBelowLineValue(superscriptEqCode);
    if (isUpValue)
      this.AlignAsSuperScriptSwitch(dc, scriptSwitch, upValue);
    else if (downValue != 0)
      this.AlignAsSubscriptSwitch(dc, scriptSwitch, downValue);
    if (spaceAboveLineValue > 0)
      this.AddSpaceAboveLine(scriptSwitch, spaceAboveLineValue);
    if (spaceBelowLineValue <= 0)
      return;
    this.AddSpaceBelowLine(scriptSwitch, spaceBelowLineValue);
  }

  private void AlignAsSuperScriptSwitch(
    DrawingContext dc,
    LayoutedEQFields scriptSwitch,
    int upValue)
  {
    LayoutedEQFields childEqFiled = scriptSwitch.ChildEQFileds[0];
    dc.ShiftEqFieldYPosition(childEqFiled, -(float) upValue);
    if (upValue > 0)
    {
      scriptSwitch.Bounds = new RectangleF(scriptSwitch.Bounds.X, scriptSwitch.Bounds.Y - (float) upValue, scriptSwitch.Bounds.Width, scriptSwitch.Bounds.Height + (float) upValue);
    }
    else
    {
      scriptSwitch.Bounds = new RectangleF(scriptSwitch.Bounds.X, scriptSwitch.Bounds.Y, scriptSwitch.Bounds.Width, scriptSwitch.Bounds.Height - (float) upValue);
      scriptSwitch.SwitchType = LayoutedEQFields.EQSwitchType.Subscript;
    }
  }

  private void AlignAsSubscriptSwitch(
    DrawingContext dc,
    LayoutedEQFields scriptSwitch,
    int downValue)
  {
    LayoutedEQFields childEqFiled = scriptSwitch.ChildEQFileds[0];
    dc.ShiftEqFieldYPosition(childEqFiled, (float) downValue);
    if (downValue > 0)
    {
      scriptSwitch.Bounds = new RectangleF(scriptSwitch.Bounds.X, scriptSwitch.Bounds.Y, scriptSwitch.Bounds.Width, scriptSwitch.Bounds.Height + (float) downValue);
      scriptSwitch.SwitchType = LayoutedEQFields.EQSwitchType.Subscript;
    }
    else
    {
      scriptSwitch.Bounds = new RectangleF(scriptSwitch.Bounds.X, scriptSwitch.Bounds.Y + (float) downValue, scriptSwitch.Bounds.Width, scriptSwitch.Bounds.Height - (float) downValue);
      scriptSwitch.SwitchType = LayoutedEQFields.EQSwitchType.Superscript;
    }
  }

  private void GetsSuperOrSubscriptPropertiesValue(
    string fieldCode,
    ref int upValue,
    ref int downValue,
    ref bool isUpValue,
    ref bool isDownValue)
  {
    string[] strArray = fieldCode.Split('\\');
    for (int index = strArray.Length - 1; index > 0; --index)
    {
      if (strArray[index].Trim().StartsWith("up", StringComparison.CurrentCultureIgnoreCase))
      {
        upValue = this.GetValueFromstring(strArray[index].Trim());
        if (upValue == int.MinValue)
          upValue = 2;
        downValue = 0;
        break;
      }
      if (strArray[index].Trim().StartsWith("do", StringComparison.CurrentCultureIgnoreCase))
      {
        downValue = this.GetValueFromstring(strArray[index].Trim());
        if (downValue == int.MinValue)
          downValue = 2;
        upValue = 0;
        isDownValue = true;
        isUpValue = false;
        break;
      }
    }
  }

  private int GetValueFromstring(string inputText)
  {
    string s = "";
    for (int index = 0; index < inputText.Length; ++index)
    {
      if (char.IsDigit(inputText[index]))
        s += (string) (object) inputText[index];
    }
    if (!this.HasPositiveValue(inputText))
      return -(s != "" ? int.Parse(s) : int.MinValue);
    return !(s != "") ? int.MinValue : int.Parse(s);
  }

  private float GetMaximumBottom(LayoutedEQFields ltEqFields)
  {
    float bottom = ltEqFields.Bounds.Bottom;
    for (int index = 0; index < ltEqFields.ChildEQFileds.Count; ++index)
    {
      if (index == 0)
        bottom = ltEqFields.ChildEQFileds[index].Bounds.Bottom;
      else if ((double) ltEqFields.ChildEQFileds[index].Bounds.Bottom >= (double) bottom)
        bottom = ltEqFields.ChildEQFileds[index].Bounds.Bottom;
    }
    return bottom;
  }

  private LayoutedEQFields LayoutFractionSwitch(
    FontScriptType fontScriptType,
    string fieldCode,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    string[] elements = this.SplitElementsByComma(fieldCode.Substring(fieldCode.IndexOf("\\f", StringComparison.CurrentCultureIgnoreCase) + 2));
    LayoutedEQFields fraction = new LayoutedEQFields();
    fraction.SwitchType = LayoutedEQFields.EQSwitchType.Fraction;
    this.GenerateFractionSwitch(fontScriptType, fraction, elements, dc, charFormat, xPosition, yPosition);
    return fraction;
  }

  private void GenerateFractionSwitch(
    FontScriptType fontScriptType,
    LayoutedEQFields fraction,
    string[] elements,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    for (int index = 0; index <= elements.Length - 1; ++index)
      this.GenerateSwitch(fontScriptType, fraction, elements[index], dc, charFormat.GetFontToRender(fontScriptType), charFormat, xPosition, yPosition);
    this.InsertFractionLine(fontScriptType, fraction, fraction.ChildEQFileds[0], fraction.ChildEQFileds[1], xPosition, yPosition, dc, charFormat);
  }

  private void InsertFractionLine(
    FontScriptType fontScriptType,
    LayoutedEQFields fraction,
    LayoutedEQFields numerator,
    LayoutedEQFields denominator,
    float xPosition,
    float yPosition,
    DrawingContext dc,
    WCharacterFormat charFormat)
  {
    LineEQField fractionLine = new LineEQField();
    fractionLine.Point1 = new PointF(xPosition, yPosition - 1f);
    this.SetXForFractionelements(fractionLine, numerator, denominator, xPosition, yPosition);
    this.SetYForFractionElements(numerator, denominator, dc, xPosition, yPosition);
    fraction.ChildEQFileds.Insert(1, (LayoutedEQFields) fractionLine);
    float num = dc.MeasureString(" ", charFormat.GetFontToRender(fontScriptType), (StringFormat) null, charFormat, false).Height - dc.GetAscent(charFormat.GetFontToRender(fontScriptType));
    dc.ShiftEqFieldYPosition(fraction, -num);
    this.UpdateltEqFieldsBounds(fraction);
  }

  private void SetXForFractionelements(
    LineEQField fractionLine,
    LayoutedEQFields numerator,
    LayoutedEQFields denominator,
    float xPosition,
    float yPosition)
  {
    if ((double) numerator.Bounds.Width > (double) denominator.Bounds.Width)
    {
      fractionLine.Point2 = new PointF(fractionLine.Point1.X + numerator.Bounds.Width, fractionLine.Point1.Y);
      fractionLine.Bounds = new RectangleF(xPosition, yPosition, numerator.Bounds.Width, 0.5f);
      this.ShiftEqFieldXPosition(denominator, this.CenterAlign(fractionLine, denominator.Bounds.Width));
    }
    else
    {
      fractionLine.Point2 = new PointF(fractionLine.Point1.X + denominator.Bounds.Width, fractionLine.Point1.Y);
      fractionLine.Bounds = new RectangleF(xPosition, yPosition, denominator.Bounds.Width, 0.5f);
      this.ShiftEqFieldXPosition(numerator, this.CenterAlign(fractionLine, numerator.Bounds.Width));
    }
  }

  private void SetYForFractionElements(
    LayoutedEQFields numerator,
    LayoutedEQFields denominator,
    DrawingContext dc,
    float xPosition,
    float yPosition)
  {
    float yPosition1 = yPosition - (1f + this.GetTopMostY(denominator));
    dc.ShiftEqFieldYPosition(denominator, yPosition1);
    float yPosition2 = yPosition - (numerator.Bounds.Bottom + 1f);
    dc.ShiftEqFieldYPosition(numerator, yPosition2);
  }

  private float CenterAlign(LineEQField lineEqField, float textWidth)
  {
    return (float) (((double) lineEqField.Point2.X - (double) lineEqField.Point1.X - (double) textWidth) / 2.0);
  }

  private bool IsValidFractionSwitch(string fieldCode)
  {
    if (this.HasParenthesisError(fieldCode))
      return false;
    string str = fieldCode.Substring(fieldCode.IndexOf("\\f", StringComparison.CurrentCultureIgnoreCase) + 2);
    return this.StartsWithExt(str.TrimStart(), "(") && this.SplitElementsByComma(str).Length == 2 && this.IsValidEqFieldCode(new List<string>(), str);
  }

  private void LayoutEquationFieldCode(
    FontScriptType fontScriptType,
    LayoutedEQFields layouttedEqField,
    List<string> splittedFieldCodeSwitch,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    for (int index = 0; index < splittedFieldCodeSwitch.Count; ++index)
    {
      LayoutedEQFields layoutedEqFields = this.LayoutSwitch(fontScriptType, splittedFieldCodeSwitch[index], dc, charFormat, xPosition, yPosition);
      layouttedEqField.ChildEQFileds.Add(layoutedEqFields);
      xPosition = layoutedEqFields.Bounds.Right;
    }
    this.UpdateltEqFieldsBounds(layouttedEqField);
  }

  private LayoutedEQFields LayoutBracketSwitch(
    FontScriptType fontScriptType,
    string fieldCode,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    string substringTill = this.GetSubstringTill(fieldCode, '(');
    string str1 = fieldCode.Substring(fieldCode.IndexOf(substringTill, StringComparison.Ordinal) + substringTill.Length);
    string str2 = str1.Substring(1, str1.Length - 2);
    char openingChar = char.MinValue;
    char closingChar = char.MinValue;
    this.GetBracketsType(substringTill, ref openingChar, ref closingChar);
    if (openingChar == char.MinValue && closingChar == char.MinValue)
    {
      openingChar = '(';
      closingChar = ')';
    }
    LayoutedEQFields layoutedEqFields = new LayoutedEQFields();
    layoutedEqFields.SwitchType = LayoutedEQFields.EQSwitchType.Bracket;
    if (!string.IsNullOrEmpty(str2))
      this.GenerateBracketSwitch(fontScriptType, layoutedEqFields, str2, openingChar, closingChar, dc, charFormat, xPosition, yPosition);
    else
      this.GenerateSwitch(fontScriptType, layoutedEqFields, str2, dc, charFormat.GetFontToRender(fontScriptType), charFormat, xPosition, yPosition);
    return layoutedEqFields;
  }

  private void GetBracketsType(string bracketFieldCode, ref char openingChar, ref char closingChar)
  {
    string[] strArray = bracketFieldCode.Split('\\');
    for (int index = 0; index < strArray.Length; ++index)
    {
      if (index < strArray.Length - 1)
      {
        char ch = strArray[index + 1][0];
        if (strArray[index].StartsWith("bc", StringComparison.CurrentCultureIgnoreCase))
        {
          openingChar = ch;
          closingChar = this.GetCorrespondingCloseCharacter(openingChar);
        }
        else if (strArray[index].StartsWith("lc", StringComparison.CurrentCultureIgnoreCase))
          openingChar = ch;
        else if (strArray[index].StartsWith("rc", StringComparison.CurrentCultureIgnoreCase))
          closingChar = ch;
      }
    }
  }

  private char GetCorrespondingCloseCharacter(char openingCharacter)
  {
    switch (openingCharacter)
    {
      case '(':
        return ')';
      case '<':
        return '>';
      case '[':
        return ']';
      case '{':
        return '}';
      default:
        return openingCharacter;
    }
  }

  private void GenerateBracketSwitch(
    FontScriptType fontScriptType,
    LayoutedEQFields bracketSwitch,
    string element,
    char openBracket,
    char closeBracket,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    this.GenerateSwitch(fontScriptType, bracketSwitch, element, dc, charFormat.GetFontToRender(fontScriptType), charFormat, xPosition, yPosition);
    float extraYPosition = dc.GetAscent(charFormat.GetFontToRender(fontScriptType)) / 2f;
    float extraWidth = 0.0f;
    bool isBracketContainsMinimumHeight = this.IsBracketContainsMinimumHeight(fontScriptType, openBracket, charFormat, dc, bracketSwitch.Bounds.Height);
    if (!isBracketContainsMinimumHeight && (openBracket == '[' || closeBracket == ']'))
      extraWidth = 1f;
    float maxHeight = bracketSwitch.Bounds.Height - extraYPosition / 2f;
    float yPosition1 = bracketSwitch.Bounds.Y + extraYPosition;
    this.GenerateopeningBracket(fontScriptType, bracketSwitch, openBracket, dc, charFormat, isBracketContainsMinimumHeight, xPosition, yPosition1, maxHeight, extraWidth, extraYPosition);
    this.GenerateClosingBracket(fontScriptType, bracketSwitch, closeBracket, dc, charFormat, isBracketContainsMinimumHeight, bracketSwitch.Bounds.Right, yPosition1, maxHeight, extraWidth, extraYPosition);
    this.UpdateltEqFieldsBounds(bracketSwitch);
  }

  private void GenerateopeningBracket(
    FontScriptType fontScriptType,
    LayoutedEQFields bracketSwitch,
    char openBracket,
    DrawingContext dc,
    WCharacterFormat charFormat,
    bool isBracketContainsMinimumHeight,
    float xPosition,
    float yPosition,
    float maxHeight,
    float extraWidth,
    float extraYPosition)
  {
    LayoutedEQFields layoutedCharacter = new LayoutedEQFields();
    if (isBracketContainsMinimumHeight)
      this.AdjustFontSizeOfCharacter(fontScriptType, layoutedCharacter, openBracket, dc, charFormat, xPosition, bracketSwitch.Bounds.Height);
    else
      this.GenerateCharacter(fontScriptType, layoutedCharacter, openBracket, dc, charFormat, xPosition, yPosition, maxHeight);
    layoutedCharacter.Bounds = new RectangleF(layoutedCharacter.Bounds.X, layoutedCharacter.Bounds.Y, layoutedCharacter.Bounds.Width + extraWidth, layoutedCharacter.Bounds.Height - extraYPosition / 2f);
    this.ShiftEqFieldXPosition(bracketSwitch, layoutedCharacter.Bounds.Width + extraWidth);
    bracketSwitch.ChildEQFileds.Add(layoutedCharacter);
  }

  private void GenerateClosingBracket(
    FontScriptType fontScriptType,
    LayoutedEQFields bracketSwitch,
    char closeBracket,
    DrawingContext dc,
    WCharacterFormat charFormat,
    bool isBracketContainsMinimumHeight,
    float xPosition,
    float yPosition,
    float maxHeight,
    float extraWidth,
    float extraYPosition)
  {
    LayoutedEQFields layoutedEqFields = new LayoutedEQFields();
    isBracketContainsMinimumHeight = this.IsBracketContainsMinimumHeight(fontScriptType, closeBracket, charFormat, dc, bracketSwitch.Bounds.Height);
    if (isBracketContainsMinimumHeight)
    {
      this.AdjustFontSizeOfCharacter(fontScriptType, layoutedEqFields, closeBracket, dc, charFormat, xPosition, bracketSwitch.Bounds.Height);
    }
    else
    {
      this.GenerateCharacter(fontScriptType, layoutedEqFields, closeBracket, dc, charFormat, xPosition, yPosition, maxHeight);
      layoutedEqFields.Bounds = new RectangleF(layoutedEqFields.Bounds.X, layoutedEqFields.Bounds.Y, layoutedEqFields.Bounds.Width + extraWidth, layoutedEqFields.Bounds.Height - extraYPosition / 2f);
      this.ShiftEqFieldXPosition(layoutedEqFields, extraWidth);
    }
    bracketSwitch.ChildEQFileds.Add(layoutedEqFields);
  }

  private bool IsBracketContainsMinimumHeight(
    FontScriptType fontScriptType,
    char inputCharacter,
    WCharacterFormat charFormat,
    DrawingContext dc,
    float maxHeight)
  {
    float num1 = 23f;
    float num2 = 14f;
    float height = dc.MeasureString(inputCharacter.ToString(), charFormat.GetFontToRender(fontScriptType), (StringFormat) null, charFormat, false).Height;
    switch (inputCharacter)
    {
      case '(':
      case ')':
        if ((double) num2 >= (double) maxHeight)
          return true;
        break;
      case '[':
      case ']':
        if ((double) height >= (double) maxHeight)
          return true;
        break;
      case '{':
      case '}':
        if ((double) num1 >= (double) maxHeight)
          return true;
        break;
    }
    return false;
  }

  private void GenerateCharacter(
    FontScriptType fontScriptType,
    LayoutedEQFields layoutedCharacter,
    char inputCharacter,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition,
    float maxHeight)
  {
    switch (inputCharacter)
    {
      case '(':
        this.GenerateParenthesisFromUnicodes(layoutedCharacter, '⎧', '⎪', '⎩', dc, charFormat, xPosition, yPosition, maxHeight);
        break;
      case ')':
        this.GenerateParenthesisFromUnicodes(layoutedCharacter, '⎫', '⎪', '⎭', dc, charFormat, xPosition, yPosition, maxHeight);
        break;
      case '[':
        this.GenerateSquareBracketFromUnicodes(layoutedCharacter, '⎡', '⎢', '⎣', dc, charFormat, xPosition, yPosition, maxHeight);
        break;
      case ']':
        this.GenerateSquareBracketFromUnicodes(layoutedCharacter, '⎤', '⎥', '⎦', dc, charFormat, xPosition, yPosition, maxHeight);
        break;
      case '{':
        this.GeneratCurlyBraceFromUnicodes(layoutedCharacter, '⎧', '⎨', '⎩', '⎪', dc, charFormat, xPosition, yPosition, maxHeight);
        break;
      case '}':
        this.GeneratCurlyBraceFromUnicodes(layoutedCharacter, '⎫', '⎬', '⎭', '⎪', dc, charFormat, xPosition, yPosition, maxHeight);
        break;
      default:
        this.AdjustFontSizeOfCharacter(fontScriptType, layoutedCharacter, inputCharacter, dc, charFormat, xPosition, maxHeight);
        break;
    }
  }

  private void AdjustFontSizeOfCharacter(
    FontScriptType fontScriptType,
    LayoutedEQFields layoutedCharacter,
    char inputCharacter,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float maxHeight)
  {
    if ((double) maxHeight <= 0.0)
      maxHeight = charFormat.GetFontToRender(fontScriptType).Size;
    TextEQField textEqField = new TextEQField();
    textEqField.Text = inputCharacter.ToString();
    textEqField.Font = this.Document.FontSettings.GetFont(charFormat.GetFontToRender(fontScriptType).FontFamily.Name, maxHeight, charFormat.GetFontToRender(fontScriptType).Style);
    textEqField.Bounds = new RectangleF(new PointF(xPosition, 0.0f), dc.MeasureString(textEqField.Text, textEqField.Font, (StringFormat) null, charFormat, false));
    layoutedCharacter.ChildEQFileds.Add((LayoutedEQFields) textEqField);
    float ascent = dc.GetAscent(textEqField.Font);
    dc.ShiftEqFieldYPosition(layoutedCharacter, -ascent);
    this.UpdateltEqFieldsBounds(layoutedCharacter);
  }

  private void GenerateParenthesisFromUnicodes(
    LayoutedEQFields charField,
    char upperPartUnicode,
    char middlePartUnicode,
    char lowerPartUnicode,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition,
    float maxHeight)
  {
    TextEQField upperPart = new TextEQField();
    this.GenerateUpperPartOfBracket(charField, upperPartUnicode, upperPart, dc, charFormat, xPosition, yPosition);
    TextEQField lowerPart = new TextEQField();
    this.GenerateLowerPartOfBracket(charField, lowerPartUnicode, lowerPart, dc, charFormat, xPosition, yPosition + maxHeight);
    float maxHeight1 = lowerPart.Bounds.Top - upperPart.Bounds.Bottom;
    if ((double) maxHeight1 > 0.0)
    {
      LayoutedEQFields middlePart = new LayoutedEQFields();
      this.GenerateMiddlePartParenthesis(charField, middlePartUnicode, middlePart, lowerPart, dc, charFormat, xPosition, upperPart.Bounds.Bottom, maxHeight1);
    }
    this.UpdateltEqFieldsBounds(charField);
  }

  private void GenerateMiddlePartParenthesis(
    LayoutedEQFields charField,
    char middlePartUnicode,
    LayoutedEQFields middlePart,
    TextEQField lowerPart,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition,
    float maxHeight)
  {
    this.GenerateRepeatedCharacter(middlePart, middlePartUnicode, dc, charFormat, xPosition, yPosition, maxHeight, 0.0f);
    float num = middlePart.Bounds.Height - (maxHeight + lowerPart.Font.Size / 2f);
    if ((double) num > 0.0)
      dc.ShiftEqFieldYPosition(middlePart, -num);
    charField.ChildEQFileds.Add(middlePart);
  }

  private void GenerateSquareBracketFromUnicodes(
    LayoutedEQFields charField,
    char upperPartUnicode,
    char middlePartUnicode,
    char lowerPartUnicode,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition,
    float maxHeight)
  {
    TextEQField upperPart = new TextEQField();
    this.GenerateUpperPartOfBracket(charField, upperPartUnicode, upperPart, dc, charFormat, xPosition, yPosition);
    TextEQField lowerPart = new TextEQField();
    this.GenerateLowerPartOfBracket(charField, lowerPartUnicode, lowerPart, dc, charFormat, xPosition, yPosition + maxHeight);
    float maxHeight1 = lowerPart.Bounds.Top - upperPart.Bounds.Bottom;
    if ((double) maxHeight1 > 0.0)
    {
      LayoutedEQFields extensionPart = new LayoutedEQFields();
      this.GenerateRepeatedCharacter(extensionPart, middlePartUnicode, dc, charFormat, xPosition, upperPart.Bounds.Bottom, maxHeight1, 0.0f);
      charField.ChildEQFileds.Add(extensionPart);
    }
    this.UpdateltEqFieldsBounds(charField);
  }

  private void GeneratCurlyBraceFromUnicodes(
    LayoutedEQFields charField,
    char upperPartUnicode,
    char middlePartUnicode,
    char lowerPartUnicode,
    char extensionPartUnicode,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition,
    float maxHeight)
  {
    TextEQField upperPart = new TextEQField();
    this.GenerateUpperPartOfBracket(charField, upperPartUnicode, upperPart, dc, charFormat, xPosition, yPosition);
    TextEQField lowerPart = new TextEQField();
    this.GenerateLowerPartOfBracket(charField, lowerPartUnicode, lowerPart, dc, charFormat, xPosition, yPosition + maxHeight);
    float yPosition1 = yPosition + maxHeight / 2f;
    TextEQField textEqField = new TextEQField();
    this.GenerateUpperPartOfBracket(charField, middlePartUnicode, textEqField, dc, charFormat, xPosition, yPosition1);
    this.UpdateltEqFieldsBounds(charField);
    float maxHeight1 = textEqField.Bounds.Top - textEqField.Font.Size - upperPart.Bounds.Bottom;
    if ((double) maxHeight1 > 0.0)
      this.GenerateUpperPartExtension(charField, extensionPartUnicode, textEqField, dc, charFormat, xPosition, upperPart.Bounds.Bottom, maxHeight1);
    float maxHeight2 = lowerPart.Bounds.Top - textEqField.Bounds.Bottom;
    if ((double) maxHeight2 > 0.0)
      this.GenerateLowerPartExtension(charField, extensionPartUnicode, lowerPart, dc, charFormat, xPosition, textEqField.Bounds.Bottom, maxHeight2);
    this.UpdateltEqFieldsBounds(charField);
  }

  private void GenerateUpperPartExtension(
    LayoutedEQFields charField,
    char extensionPartUnicode,
    TextEQField middlePart,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition,
    float maxHeight)
  {
    LayoutedEQFields layoutedEqFields = new LayoutedEQFields();
    this.GenerateRepeatedCharacter(layoutedEqFields, extensionPartUnicode, dc, charFormat, xPosition, yPosition, maxHeight, 0.0f);
    if ((double) layoutedEqFields.Bounds.Height > (double) maxHeight)
    {
      float num = layoutedEqFields.Bounds.Height - (maxHeight + middlePart.Font.Size * 0.25f);
      if ((double) num > 0.0)
        dc.ShiftEqFieldYPosition(layoutedEqFields, -num);
    }
    charField.ChildEQFileds.Add(layoutedEqFields);
  }

  private void GenerateLowerPartExtension(
    LayoutedEQFields charField,
    char extensionPartUnicode,
    TextEQField lowerPart,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition,
    float maxHeight)
  {
    LayoutedEQFields layoutedEqFields = new LayoutedEQFields();
    this.GenerateRepeatedCharacter(layoutedEqFields, extensionPartUnicode, dc, charFormat, xPosition, yPosition, maxHeight, 0.0f);
    if ((double) layoutedEqFields.Bounds.Height > (double) maxHeight)
    {
      float num = layoutedEqFields.Bounds.Height - (maxHeight + lowerPart.Font.Size / 2f);
      if ((double) num > 0.0)
        dc.ShiftEqFieldYPosition(layoutedEqFields, -num);
    }
    charField.ChildEQFileds.Add(layoutedEqFields);
    this.UpdateltEqFieldsBounds(charField);
  }

  private void GenerateUpperPartOfBracket(
    LayoutedEQFields charField,
    char upperPartUnicode,
    TextEQField upperPart,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    upperPart.Text = upperPartUnicode.ToString();
    upperPart.Font = this.Document.FontSettings.GetFont("Cambria Math", 8f, FontStyle.Regular);
    upperPart.Bounds = new RectangleF(new PointF(xPosition, yPosition), dc.MeasureString(upperPart.Text, upperPart.Font, (StringFormat) null, charFormat, false));
    charField.ChildEQFileds.Add((LayoutedEQFields) upperPart);
  }

  private void GenerateLowerPartOfBracket(
    LayoutedEQFields charField,
    char lowerPartUnicode,
    TextEQField lowerPart,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    lowerPart.Text = lowerPartUnicode.ToString();
    lowerPart.Font = this.Document.FontSettings.GetFont("Cambria Math", 8f, FontStyle.Regular);
    float y = yPosition - dc.MeasureString(lowerPart.Text, lowerPart.Font, (StringFormat) null, charFormat, false).Height;
    lowerPart.Bounds = new RectangleF(new PointF(xPosition, y), dc.MeasureString(lowerPart.Text, lowerPart.Font, (StringFormat) null, charFormat, false));
    charField.ChildEQFileds.Add((LayoutedEQFields) lowerPart);
  }

  private void GenerateRepeatedCharacter(
    LayoutedEQFields extensionPart,
    char extensionUnicode,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition,
    float maxHeight,
    float fontSize)
  {
    while ((double) extensionPart.Bounds.Height < (double) maxHeight)
    {
      TextEQField textEqField = new TextEQField()
      {
        Text = extensionUnicode.ToString()
      };
      textEqField.Font = textEqField.Text.Equals('ô'.ToString()) ? this.Document.FontSettings.GetFont("Symbol", fontSize, FontStyle.Regular) : this.Document.FontSettings.GetFont("Cambria Math", 8f, FontStyle.Regular);
      textEqField.Bounds = new RectangleF(new PointF(xPosition, yPosition), dc.MeasureString(textEqField.Text, textEqField.Font, (StringFormat) null, charFormat, false));
      if (textEqField.Text.Equals('ô'.ToString()) && extensionPart.ChildEQFileds.Count > 0)
        dc.ShiftEqFieldYPosition((LayoutedEQFields) textEqField, -(float) dc.FontMetric.Descent(textEqField.Font));
      yPosition = textEqField.Bounds.Bottom;
      extensionPart.ChildEQFileds.Add((LayoutedEQFields) textEqField);
      this.UpdateltEqFieldsBounds(extensionPart);
    }
    this.UpdateltEqFieldsBounds(extensionPart);
  }

  private bool IsValidBracketSwitch(string fieldCode)
  {
    if (this.HasParenthesisError(fieldCode))
      return false;
    string substringTill = this.GetSubstringTill(fieldCode, '(');
    string str = fieldCode.Substring(fieldCode.IndexOf(substringTill, StringComparison.Ordinal) + substringTill.Length);
    if (this.SplitElementsByComma(str).Length != 1)
      return false;
    return !this.HasInCorrectBracketSwitchSequence(substringTill.Split('\\')) && this.IsValidEqFieldCode(new List<string>(), str);
  }

  private bool HasInCorrectBracketSwitchSequence(string[] fieldCodes)
  {
    for (int index = 1; index < fieldCodes.Length; ++index)
    {
      if (!fieldCodes[index].TrimEnd().Equals("b", StringComparison.CurrentCultureIgnoreCase) || index != 1)
      {
        if (!fieldCodes[index].StartsWith("bc", StringComparison.CurrentCultureIgnoreCase) && !fieldCodes[index].StartsWith("lc", StringComparison.CurrentCultureIgnoreCase) && !fieldCodes[index].StartsWith("rc", StringComparison.CurrentCultureIgnoreCase) || !this.IsOnlyAlphabets(fieldCodes[index].Trim()) || index != fieldCodes.Length - 1 && (!(fieldCodes[index + 1] != "") || this.HasManyCharacters(fieldCodes[index + 1])))
          return true;
        ++index;
      }
    }
    return false;
  }

  private bool IsValidBoxSwitch(string fieldCode)
  {
    if (this.HasParenthesisError(fieldCode))
      return false;
    string substringTill = this.GetSubstringTill(fieldCode, '(');
    string str = fieldCode.Substring(fieldCode.IndexOf(substringTill, StringComparison.Ordinal) + substringTill.Length);
    return this.StartsWithExt(str.TrimStart(), "(") && this.SplitElementsByComma(str).Length == 1 && this.IsCorrectBoxSwitchSequence(substringTill) && this.IsValidEqFieldCode(new List<string>(), str);
  }

  private bool IsCorrectBoxSwitchSequence(string boxSwitchEqCode)
  {
    if (boxSwitchEqCode.Contains("\\ "))
      return false;
    string[] strArray = boxSwitchEqCode.Split('\\');
    for (int index = 1; index < strArray.Length; ++index)
    {
      if (strArray[index].TrimEnd().Contains(" ") || !this.IsOnlyAlphabets(strArray[index].Trim()) || !strArray[index].StartsWith("le", StringComparison.CurrentCultureIgnoreCase) && !strArray[index].StartsWith("ri", StringComparison.CurrentCultureIgnoreCase) && !strArray[index].StartsWith("to", StringComparison.CurrentCultureIgnoreCase) && !strArray[index].StartsWith("bo", StringComparison.CurrentCultureIgnoreCase) && (!strArray[index].TrimEnd().Equals("x", StringComparison.OrdinalIgnoreCase) || index != 1))
        return false;
    }
    return true;
  }

  private LayoutedEQFields LayoutBoxSwitch(
    FontScriptType fontScriptType,
    string fieldCode,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    string substringTill = this.GetSubstringTill(fieldCode, '(');
    string element = fieldCode.Substring(fieldCode.IndexOf(substringTill, StringComparison.Ordinal) + substringTill.Length);
    bool hasLeftLine = false;
    bool hasTopLine = false;
    bool hasRightLine = false;
    bool hasBottomLine = false;
    this.GetBoxSwitchProperties(substringTill, ref hasLeftLine, ref hasRightLine, ref hasBottomLine, ref hasTopLine);
    LayoutedEQFields layoutedEqFields = new LayoutedEQFields();
    layoutedEqFields.SwitchType = LayoutedEQFields.EQSwitchType.Box;
    this.GenerateBoxSwitch(fontScriptType, layoutedEqFields, element, dc, charFormat, xPosition, yPosition);
    float extraBoxWidth = dc.MeasureString(" ", charFormat.GetFontToRender(fontScriptType), (StringFormat) null, charFormat, false).Width * 1.5f;
    this.InsertBox(layoutedEqFields, hasLeftLine, hasTopLine, hasRightLine, hasBottomLine, extraBoxWidth);
    this.UpdateltEqFieldsBounds(layoutedEqFields);
    return layoutedEqFields;
  }

  private void GetBoxSwitchProperties(
    string boxSwitchEqCode,
    ref bool hasLeftLine,
    ref bool hasRightLine,
    ref bool hasBottomLine,
    ref bool hasTopLine)
  {
    string[] strArray = boxSwitchEqCode.Split('\\');
    for (int index = 2; index < strArray.Length; ++index)
    {
      switch (strArray[index].Substring(0, 2).ToLower())
      {
        case "le":
          hasLeftLine = true;
          break;
        case "ri":
          hasRightLine = true;
          break;
        case "to":
          hasTopLine = true;
          break;
        case "bo":
          hasBottomLine = true;
          break;
      }
    }
  }

  private void InsertBox(
    LayoutedEQFields boxSwitch,
    bool hasLeftLine,
    bool hasTopLine,
    bool hasRightLine,
    bool hasBottomLine,
    float extraBoxWidth)
  {
    RectangleF bounds = boxSwitch.Bounds;
    bool flag = !hasLeftLine && !hasRightLine && !hasTopLine && !hasBottomLine;
    if (hasLeftLine || flag)
      this.AddLineEQChild(boxSwitch, bounds.X + 1f, bounds.Y + extraBoxWidth, bounds.X + 1f, bounds.Y + bounds.Height - extraBoxWidth, "leftLine");
    if (hasRightLine || flag)
      this.AddLineEQChild(boxSwitch, (float) ((double) bounds.X + (double) bounds.Width - 2.0), bounds.Y + extraBoxWidth, (float) ((double) bounds.X + (double) bounds.Width - 2.0), bounds.Y + bounds.Height - extraBoxWidth, "rightLine");
    if (hasTopLine || flag)
      this.AddLineEQChild(boxSwitch, bounds.X + 1f, bounds.Y + extraBoxWidth, (float) ((double) bounds.X + (double) bounds.Width - 2.0), bounds.Y + extraBoxWidth, "topLine");
    if (!hasBottomLine && !flag)
      return;
    this.AddLineEQChild(boxSwitch, bounds.X + 1f, bounds.Y + bounds.Height - extraBoxWidth, (float) ((double) bounds.X + (double) bounds.Width - 2.0), bounds.Y + bounds.Height - extraBoxWidth, "bottomLine");
  }

  private void AddLineEQChild(
    LayoutedEQFields ltEqFields,
    float x1,
    float y1,
    float x2,
    float y2,
    string line)
  {
    LineEQField lineEqField = new LineEQField();
    lineEqField.Point1 = new PointF(x1, y1);
    lineEqField.Point2 = new PointF(x2, y2);
    switch (line)
    {
      case "leftLine":
      case "rightLine":
        lineEqField.Bounds = new RectangleF(x1, y1, 1f, y2 - y1);
        break;
      case "topLine":
      case "bottomLine":
        lineEqField.Bounds = new RectangleF(x1, y1, x2 - x1, 1f);
        break;
    }
    ltEqFields.ChildEQFileds.Add((LayoutedEQFields) lineEqField);
  }

  private void GenerateBoxSwitch(
    FontScriptType fontScriptType,
    LayoutedEQFields boxSwitch,
    string element,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    element = element.Substring(1, element.Length - 2);
    this.GenerateSwitch(fontScriptType, boxSwitch, element, dc, charFormat.GetFontToRender(fontScriptType), charFormat, xPosition, yPosition);
    float shiftX = dc.MeasureString(" ", charFormat.GetFontToRender(fontScriptType), (StringFormat) null, charFormat, false).Width * 1.5f;
    RectangleF bounds = boxSwitch.Bounds;
    boxSwitch.Bounds = new RectangleF(bounds.X - shiftX, bounds.Y - shiftX, bounds.Width + shiftX * 2f, bounds.Height + shiftX * 2f);
    this.ShiftEqFieldXPosition(boxSwitch, shiftX);
  }

  private LayoutedEQFields LayoutIntegralSwitch(
    FontScriptType fontScriptType,
    string fieldCode,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    string substringTill = this.GetSubstringTill(fieldCode, '(');
    string element = fieldCode.Substring(fieldCode.IndexOf(substringTill, StringComparison.Ordinal) + substringTill.Length);
    bool isInline = false;
    bool isVaribleSize = false;
    string symbol = "I";
    this.GetIntegralProperties(substringTill, ref isInline, ref symbol, ref isVaribleSize);
    LayoutedEQFields integralSwitch = new LayoutedEQFields();
    integralSwitch.SwitchType = LayoutedEQFields.EQSwitchType.Integral;
    this.GenerateIntegralSwitch(fontScriptType, integralSwitch, element, symbol, isInline, isVaribleSize, dc, charFormat, xPosition, yPosition);
    return integralSwitch;
  }

  private void GetIntegralProperties(
    string integralEqCode,
    ref bool isInline,
    ref string symbol,
    ref bool isVaribleSize)
  {
    string[] strArray = integralEqCode.Split('\\');
    for (int index = 0; index < strArray.Length; ++index)
    {
      if (strArray[index].StartsWith("su", StringComparison.CurrentCultureIgnoreCase))
        symbol = "S";
      else if (strArray[index].StartsWith("pr", StringComparison.CurrentCultureIgnoreCase))
        symbol = "P";
      else if (strArray[index].StartsWith("fc", StringComparison.CurrentCultureIgnoreCase) && index < strArray.Length - 1)
      {
        symbol = strArray[index + 1][0].ToString();
        isVaribleSize = false;
      }
      else if (strArray[index].StartsWith("vc", StringComparison.CurrentCultureIgnoreCase) && index < strArray.Length - 1)
      {
        symbol = strArray[index + 1][0].ToString();
        isVaribleSize = true;
      }
      else if (strArray[index].StartsWith("in", StringComparison.CurrentCultureIgnoreCase))
        isInline = true;
    }
  }

  private void GenerateIntegralSwitch(
    FontScriptType fontScriptType,
    LayoutedEQFields integralSwitch,
    string element,
    string symbol,
    bool isInline,
    bool isVariableSize,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    string[] integralElements = this.SplitElementsByComma(element);
    float maxWidth1 = 0.0f;
    LayoutedEQFields upperLimit = new LayoutedEQFields();
    LayoutedEQFields lowerLimit = new LayoutedEQFields();
    LayoutedEQFields integrand = new LayoutedEQFields();
    this.GenerateIntegralSwitchElements(fontScriptType, upperLimit, lowerLimit, integrand, ref maxWidth1, integralElements, dc, charFormat, xPosition, yPosition);
    float symbolheight = 0.0f;
    this.GetHeightOfSymbol(upperLimit, lowerLimit, integrand, isInline, ref symbolheight);
    LayoutedEQFields layoutedEqFields = new LayoutedEQFields();
    if (symbol == "I" || integralElements[0] != "" || integralElements[1] != "" || integralElements[2] != "")
      this.GenerateSymbolForIntegralSwitch(fontScriptType, integrand, layoutedEqFields, symbol, isVariableSize, dc, charFormat, xPosition, yPosition, symbolheight);
    float maxWidth2 = (double) maxWidth1 > (double) layoutedEqFields.Bounds.Width ? maxWidth1 : layoutedEqFields.Bounds.Width;
    if (layoutedEqFields.ChildEQFileds.Count <= 0)
      return;
    this.SetIntegralElementsPosition(fontScriptType, isInline, upperLimit, lowerLimit, layoutedEqFields, integrand, integralSwitch, symbol, dc, charFormat, xPosition, yPosition, maxWidth2);
  }

  private void GenerateIntegralSwitchElements(
    FontScriptType fontScriptType,
    LayoutedEQFields upperLimit,
    LayoutedEQFields lowerLimit,
    LayoutedEQFields integrand,
    ref float maxWidth,
    string[] integralElements,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    this.GenerateSwitch(fontScriptType, upperLimit, integralElements[1], dc, charFormat.GetFontToRender(fontScriptType), charFormat, xPosition, yPosition);
    maxWidth = upperLimit.Bounds.Width;
    this.GenerateSwitch(fontScriptType, lowerLimit, integralElements[0], dc, charFormat.GetFontToRender(fontScriptType), charFormat, xPosition, yPosition);
    maxWidth = (double) maxWidth > (double) lowerLimit.Bounds.Width ? maxWidth : lowerLimit.Bounds.Width;
    this.GenerateSwitch(fontScriptType, integrand, integralElements[2], dc, charFormat.GetFontToRender(fontScriptType), charFormat, xPosition, yPosition);
  }

  private void GetHeightOfSymbol(
    LayoutedEQFields upperLimit,
    LayoutedEQFields lowerLimit,
    LayoutedEQFields integrand,
    bool isInline,
    ref float symbolheight)
  {
    if (isInline)
      symbolheight = ((double) upperLimit.Bounds.Height + (double) lowerLimit.Bounds.Height) / 2.0 > (double) integrand.Bounds.Height ? (float) (((double) upperLimit.Bounds.Height + (double) lowerLimit.Bounds.Height) / 2.0) : integrand.Bounds.Height;
    else
      symbolheight = integrand.Bounds.Height;
  }

  private void SetIntegralElementsPosition(
    FontScriptType fontScriptType,
    bool isInline,
    LayoutedEQFields upperLimit,
    LayoutedEQFields lowerLimit,
    LayoutedEQFields integralSymbol,
    LayoutedEQFields integrand,
    LayoutedEQFields integralSwitch,
    string symbol,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition,
    float maxWidth)
  {
    if (isInline)
      this.AlignInlineIntegralSwitch(fontScriptType, upperLimit, lowerLimit, integralSymbol, integrand, integralSwitch, symbol, dc, charFormat, xPosition, yPosition);
    else
      this.AlignNotInlineIntegralSwitch(fontScriptType, upperLimit, lowerLimit, integralSymbol, integrand, integralSwitch, dc, charFormat, maxWidth);
    float num = integralSwitch.ChildEQFileds.Count > 2 ? integralSwitch.ChildEQFileds[1].Bounds.Right : integralSwitch.Bounds.Right;
    this.ShiftEqFieldXPosition(integrand, num - integrand.Bounds.Left);
    integralSwitch.ChildEQFileds.Add(integrand);
    this.UpdateltEqFieldsBounds(integralSwitch);
  }

  private void AlignInlineIntegralSwitch(
    FontScriptType fontScriptType,
    LayoutedEQFields upperLimit,
    LayoutedEQFields lowerLimit,
    LayoutedEQFields integralSymbol,
    LayoutedEQFields integrand,
    LayoutedEQFields integralSwitch,
    string symbol,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    float yPosition1 = integrand.Bounds.Top - upperLimit.Bounds.Top;
    dc.ShiftEqFieldYPosition(upperLimit, yPosition1);
    float yPosition2 = integrand.Bounds.Bottom - lowerLimit.Bounds.Bottom;
    dc.ShiftEqFieldYPosition(lowerLimit, yPosition2);
    float num = upperLimit.Bounds.Bottom - lowerLimit.Bounds.Top;
    if ((double) num > 0.0)
    {
      dc.ShiftEqFieldYPosition(upperLimit, (float) -((double) num / 2.0));
      dc.ShiftEqFieldYPosition(lowerLimit, num / 2f);
    }
    float symbolSize = lowerLimit.Bounds.Bottom - upperLimit.Bounds.Top;
    integralSymbol.ChildEQFileds.Clear();
    this.GenerateSymbolForIntegralSwitch(fontScriptType, integrand, integralSymbol, symbol, false, dc, charFormat, xPosition, yPosition, symbolSize);
    this.EQFieldVerticalAlignment(integralSymbol, integrand, dc);
    float shiftX1 = integralSymbol.Bounds.Right - upperLimit.Bounds.Left;
    this.ShiftEqFieldXPosition(upperLimit, shiftX1);
    float shiftX2 = integralSymbol.Bounds.Right - lowerLimit.Bounds.Left;
    this.ShiftEqFieldXPosition(lowerLimit, shiftX2);
    integralSwitch.ChildEQFileds.Add(upperLimit);
    integralSwitch.ChildEQFileds.Add(lowerLimit);
    integralSwitch.ChildEQFileds.Add(integralSymbol);
    this.UpdateltEqFieldsBounds(integralSwitch);
  }

  private void AlignNotInlineIntegralSwitch(
    FontScriptType fontScriptType,
    LayoutedEQFields upperLimit,
    LayoutedEQFields lowerLimit,
    LayoutedEQFields integralSymbol,
    LayoutedEQFields integrand,
    LayoutedEQFields integralSwitch,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float maxWidth)
  {
    if (integralSymbol.ChildEQFileds.Count > 1)
      this.AlignIntegralSymbol(fontScriptType, upperLimit, lowerLimit, integralSymbol, integrand, dc, charFormat);
    else
      this.AlignPiOrSummationSymbol(upperLimit, lowerLimit, integralSymbol, integrand, dc);
    this.ShiftEqFieldXPosition(upperLimit, this.GetCenterAlignSpace(maxWidth, upperLimit.Bounds.Width));
    this.ShiftEqFieldXPosition(integralSymbol, this.GetCenterAlignSpace(maxWidth, integralSymbol.Bounds.Width));
    this.ShiftEqFieldXPosition(lowerLimit, this.GetCenterAlignSpace(maxWidth, lowerLimit.Bounds.Width));
    integralSwitch.ChildEQFileds.Add(upperLimit);
    integralSwitch.ChildEQFileds.Add(integralSymbol);
    integralSwitch.ChildEQFileds.Add(lowerLimit);
    this.UpdateltEqFieldsBounds(integralSwitch);
  }

  private void AlignPiOrSummationSymbol(
    LayoutedEQFields upperLimit,
    LayoutedEQFields lowerLimit,
    LayoutedEQFields integralSymbol,
    LayoutedEQFields integrand,
    DrawingContext dc)
  {
    float yPosition1 = integrand.Bounds.Top - upperLimit.Bounds.Bottom;
    dc.ShiftEqFieldYPosition(upperLimit, yPosition1);
    float yPosition2 = integrand.Bounds.Bottom - lowerLimit.Bounds.Top + (float) dc.FontMetric.Descent((integralSymbol.ChildEQFileds[0] as TextEQField).Font) / 2f;
    dc.ShiftEqFieldYPosition(lowerLimit, yPosition2);
    if ((double) lowerLimit.Bounds.Height == 0.0 || (double) upperLimit.Bounds.Height == 0.0)
    {
      this.EQFieldVerticalAlignment(integralSymbol, integrand, dc);
    }
    else
    {
      float num1 = lowerLimit.Bounds.Bottom - upperLimit.Bounds.Top;
      float num2 = (float) (((double) num1 - (double) integralSymbol.Bounds.Height) / 2.0);
      float yPosition3 = upperLimit.Bounds.Top + num2 - integralSymbol.Bounds.Y;
      dc.ShiftEqFieldYPosition(integralSymbol, yPosition3);
      float num3 = (float) (((double) num1 - (double) integrand.Bounds.Height) / 2.0);
      float yPosition4 = upperLimit.Bounds.Top + num3 - integrand.Bounds.Y;
      dc.ShiftEqFieldYPosition(integrand, yPosition4);
    }
  }

  private void AlignIntegralSymbol(
    FontScriptType fontScriptType,
    LayoutedEQFields upperLimit,
    LayoutedEQFields lowerLimit,
    LayoutedEQFields integralSymbol,
    LayoutedEQFields integrand,
    DrawingContext dc,
    WCharacterFormat charFormat)
  {
    float yPosition1 = integrand.Bounds.Top - integralSymbol.Bounds.Top;
    dc.ShiftEqFieldYPosition(integralSymbol, yPosition1);
    float ofIntegralSymbol = this.GetExtraHeightOfIntegralSymbol(integrand.Bounds.Height, (float) dc.FontMetric.Descent(charFormat.GetFontToRender(fontScriptType)));
    if ((double) upperLimit.Bounds.Height != 0.0)
    {
      float yPosition2 = integrand.Bounds.Top - upperLimit.Bounds.Bottom - (float) dc.FontMetric.Descent(charFormat.GetFontToRender(fontScriptType)) / 2f;
      dc.ShiftEqFieldYPosition(upperLimit, yPosition2);
    }
    if ((double) lowerLimit.Bounds.Height != 0.0)
    {
      float yPosition3 = integrand.Bounds.Bottom - lowerLimit.Bounds.Top + this.GetExtraValueForLowerLimit(fontScriptType, integrand.Bounds.Height, dc, charFormat);
      dc.ShiftEqFieldYPosition(lowerLimit, yPosition3);
    }
    if ((double) upperLimit.Bounds.Height == 0.0)
    {
      upperLimit.Bounds = new RectangleF(upperLimit.Bounds.X, upperLimit.Bounds.Y, upperLimit.Bounds.Width, upperLimit.Bounds.Height + ofIntegralSymbol);
      float yPosition4 = integrand.Bounds.Top - upperLimit.Bounds.Bottom;
      dc.ShiftEqFieldYPosition(upperLimit, yPosition4);
    }
    if ((double) lowerLimit.Bounds.Height != 0.0)
      return;
    lowerLimit.Bounds = new RectangleF(lowerLimit.Bounds.X, lowerLimit.Bounds.Y, lowerLimit.Bounds.Width, lowerLimit.Bounds.Height + ofIntegralSymbol);
    float yPosition5 = integrand.Bounds.Bottom - lowerLimit.Bounds.Top;
    dc.ShiftEqFieldYPosition(lowerLimit, yPosition5);
  }

  private float GetExtraValueForLowerLimit(
    FontScriptType fontScriptType,
    float integrandHeight,
    DrawingContext dc,
    WCharacterFormat charFormat)
  {
    float num = (float) dc.FontMetric.Descent(charFormat.GetFontToRender(fontScriptType));
    if ((double) charFormat.GetFontToRender(fontScriptType).Size >= 12.0)
      return 0.0f;
    if ((double) integrandHeight > 14.0 && (double) integrandHeight <= 21.0)
      return num / 3f;
    return (double) integrandHeight > 21.0 ? -num : num;
  }

  private float GetExtraHeightOfIntegralSymbol(float integrandHeight, float descent)
  {
    if ((double) integrandHeight >= 48.0 && (double) integrandHeight < 73.0)
      return descent / 2f;
    return (double) integrandHeight >= 73.0 ? descent / 4f : descent;
  }

  private float GetCenterAlignSpace(float maxSize, float elementSize)
  {
    return (maxSize - elementSize) / 2f;
  }

  private void GenerateSymbolForIntegralSwitch(
    FontScriptType fontScriptType,
    LayoutedEQFields integrand,
    LayoutedEQFields layoutedSymbol,
    string inputCharacter,
    bool isVariableSize,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition,
    float symbolSize)
  {
    switch (inputCharacter)
    {
      case "S":
        Font font1 = this.Document.FontSettings.GetFont("Symbol", this.GetFontSizeFromHeight("å", ref symbolSize, dc, charFormat), FontStyle.Regular);
        this.GenerateSwitch(fontScriptType, layoutedSymbol, "å", dc, font1, charFormat, xPosition, yPosition);
        layoutedSymbol.Bounds = new RectangleF(layoutedSymbol.Bounds.X, layoutedSymbol.Bounds.Y, layoutedSymbol.Bounds.Width, symbolSize);
        break;
      case "I":
        float integralSymbolFontSize = this.GetIntegralSymbolFontSize(integrand);
        if ((double) symbolSize <= 17.72607421875)
        {
          this.GenerateTwoPartsOfIntegral(layoutedSymbol, dc, charFormat, xPosition, yPosition, integralSymbolFontSize);
          break;
        }
        this.GenerateIntegralFromUnicodes(layoutedSymbol, dc, charFormat, xPosition, yPosition, symbolSize, integralSymbolFontSize);
        break;
      case "P":
        Font font2 = this.Document.FontSettings.GetFont("Symbol", this.GetFontSizeFromHeight("Õ", ref symbolSize, dc, charFormat), FontStyle.Regular);
        this.GenerateSwitch(fontScriptType, layoutedSymbol, "Õ", dc, font2, charFormat, xPosition, yPosition);
        break;
      default:
        TextEQField textEqField = new TextEQField();
        textEqField.Text = inputCharacter;
        textEqField.Font = !isVariableSize ? charFormat.GetFontToRender(fontScriptType) : this.Document.FontSettings.GetFont(charFormat.GetFontToRender(fontScriptType).Name, symbolSize, charFormat.GetFontToRender(fontScriptType).Style);
        textEqField.Bounds = new RectangleF(new PointF(xPosition, yPosition), dc.MeasureString(textEqField.Text, textEqField.Font, (StringFormat) null, charFormat, false));
        layoutedSymbol.ChildEQFileds.Add((LayoutedEQFields) textEqField);
        this.UpdateltEqFieldsBounds(layoutedSymbol);
        break;
    }
  }

  private float GetIntegralSymbolFontSize(LayoutedEQFields integrand)
  {
    float integralSymbolFontSize = 12f;
    if ((double) integrand.Bounds.Height >= 48.0 && (double) integrand.Bounds.Height < 73.0)
      integralSymbolFontSize = 18f;
    else if ((double) integrand.Bounds.Height >= 73.0)
      integralSymbolFontSize = 24f;
    return integralSymbolFontSize;
  }

  private void GenerateTwoPartsOfIntegral(
    LayoutedEQFields layoutedSymbol,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition,
    float fontSize)
  {
    TextEQField upperPart = new TextEQField();
    this.GenerateUpperPartOfIntegral(layoutedSymbol, upperPart, dc, charFormat, xPosition, yPosition, fontSize);
    TextEQField lowerPart = new TextEQField();
    this.GenerateLowerPartOfIntegral(layoutedSymbol, lowerPart, dc, charFormat, xPosition, yPosition + 17.7260742f, fontSize);
    this.UpdateltEqFieldsBounds(layoutedSymbol);
  }

  private void GenerateIntegralFromUnicodes(
    LayoutedEQFields layoutedSymbol,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition,
    float maxHeight,
    float fontSize)
  {
    maxHeight -= 0.5f;
    TextEQField upperPart = new TextEQField();
    this.GenerateUpperPartOfIntegral(layoutedSymbol, upperPart, dc, charFormat, xPosition, yPosition, fontSize);
    TextEQField lowerPart = new TextEQField();
    this.GenerateLowerPartOfIntegral(layoutedSymbol, lowerPart, dc, charFormat, xPosition, yPosition + maxHeight, fontSize);
    float num = (float) dc.FontMetric.Descent(upperPart.Font);
    float maxHeight1 = (float) ((double) lowerPart.Bounds.Top + (double) num - ((double) upperPart.Bounds.Bottom - (double) num));
    if ((double) maxHeight1 > 0.0)
    {
      LayoutedEQFields middlePart = new LayoutedEQFields();
      this.GenerateMiddlePartOfIntegral(layoutedSymbol, middlePart, lowerPart, dc, charFormat, xPosition, upperPart.Bounds.Bottom - num, maxHeight1, fontSize);
    }
    this.UpdateltEqFieldsBounds(layoutedSymbol);
  }

  private void GenerateUpperPartOfIntegral(
    LayoutedEQFields layoutedSymbol,
    TextEQField upperPart,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition,
    float fontSize)
  {
    upperPart.Text = 'ó'.ToString();
    upperPart.Font = this.Document.FontSettings.GetFont("Symbol", fontSize, FontStyle.Regular);
    upperPart.Bounds = new RectangleF(new PointF(xPosition, yPosition), dc.MeasureString(upperPart.Text, upperPart.Font, (StringFormat) null, charFormat, false));
    layoutedSymbol.ChildEQFileds.Add((LayoutedEQFields) upperPart);
  }

  private void GenerateLowerPartOfIntegral(
    LayoutedEQFields layoutedSymbol,
    TextEQField lowerPart,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition,
    float fontSize)
  {
    lowerPart.Text = 'õ'.ToString();
    lowerPart.Font = this.Document.FontSettings.GetFont("Symbol", fontSize, FontStyle.Regular);
    float y = yPosition - dc.MeasureString(lowerPart.Text, lowerPart.Font, (StringFormat) null, charFormat, false).Height;
    lowerPart.Bounds = new RectangleF(new PointF(xPosition, y), dc.MeasureString(lowerPart.Text, lowerPart.Font, (StringFormat) null, charFormat, false));
    layoutedSymbol.ChildEQFileds.Add((LayoutedEQFields) lowerPart);
  }

  private void GenerateMiddlePartOfIntegral(
    LayoutedEQFields layoutedSymbol,
    LayoutedEQFields middlePart,
    TextEQField lowerPart,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition,
    float maxHeight,
    float fontSize)
  {
    this.GenerateRepeatedCharacter(middlePart, 'ô', dc, charFormat, xPosition, yPosition, maxHeight, fontSize);
    float num1 = (float) dc.FontMetric.Descent((middlePart.ChildEQFileds[0] as TextEQField).Font);
    float num2 = middlePart.Bounds.Bottom - (lowerPart.Bounds.Bottom - num1 * 2f);
    if ((double) num2 > 0.0)
      dc.ShiftEqFieldYPosition(middlePart.ChildEQFileds[middlePart.ChildEQFileds.Count - 1], -num2);
    layoutedSymbol.ChildEQFileds.Add(middlePart);
  }

  private bool IsValidIntegralSwitch(string fieldCode)
  {
    if (this.HasParenthesisError(fieldCode))
      return false;
    string substringTill = this.GetSubstringTill(fieldCode, '(');
    string str = fieldCode.Substring(fieldCode.IndexOf(substringTill, StringComparison.Ordinal) + substringTill.Length);
    if (this.SplitElementsByComma(str).Length != 3)
      return false;
    return !this.HasInCorrectIntegralSequence(substringTill.Split('\\')) && this.IsValidEqFieldCode(new List<string>(), str);
  }

  private bool HasInCorrectIntegralSequence(string[] fieldCodes)
  {
    for (int index = 1; index < fieldCodes.Length; ++index)
    {
      if (!fieldCodes[index].TrimEnd().Equals("i", StringComparison.CurrentCultureIgnoreCase) || index != 1)
      {
        if (fieldCodes[index].StartsWith("su", StringComparison.CurrentCultureIgnoreCase) || fieldCodes[index].StartsWith("pr", StringComparison.CurrentCultureIgnoreCase) || fieldCodes[index].StartsWith("in", StringComparison.CurrentCultureIgnoreCase))
        {
          if (!this.IsOnlyAlphabets(fieldCodes[index].TrimEnd()))
            return true;
        }
        else
        {
          if (!fieldCodes[index].StartsWith("fc", StringComparison.CurrentCultureIgnoreCase) && !fieldCodes[index].StartsWith("vc", StringComparison.CurrentCultureIgnoreCase) || !this.IsOnlyAlphabets(fieldCodes[index].TrimEnd()) || index != fieldCodes.Length - 1 && (!(fieldCodes[index + 1] != "") || this.HasManyCharacters(fieldCodes[index + 1])))
            return true;
          ++index;
        }
      }
    }
    return false;
  }

  private bool IsValidOverstrikeSwitch(string fieldCode)
  {
    if (this.HasParenthesisError(fieldCode))
      return false;
    string substringTill = this.GetSubstringTill(fieldCode, '(');
    string fieldCode1 = fieldCode.Substring(fieldCode.IndexOf(substringTill, StringComparison.Ordinal) + substringTill.Length);
    return this.IsCorrectOverstrikeSwitchSequence(substringTill) && this.IsValidEqFieldCode(new List<string>(), fieldCode1);
  }

  private bool IsCorrectOverstrikeSwitchSequence(string overstrikeSwitchEqCode)
  {
    if (overstrikeSwitchEqCode.Contains("\\ "))
      return false;
    string[] strArray = overstrikeSwitchEqCode.Split('\\');
    for (int index = 1; index < strArray.Length; ++index)
    {
      if (strArray[index].TrimEnd().Contains(" ") || !this.IsOnlyAlphabets(strArray[index].Trim()) || !strArray[index].StartsWith("al", StringComparison.CurrentCultureIgnoreCase) && !strArray[index].StartsWith("ar", StringComparison.CurrentCultureIgnoreCase) && !strArray[index].StartsWith("ac", StringComparison.CurrentCultureIgnoreCase) && !strArray[index].StartsWith("ad", StringComparison.CurrentCultureIgnoreCase) && (!strArray[index].TrimEnd().Equals("o", StringComparison.OrdinalIgnoreCase) || index != 1))
        return false;
    }
    return true;
  }

  private LayoutedEQFields LayoutOverstrikeSwitch(
    FontScriptType fontScriptType,
    string fieldCode,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    string substringTill = this.GetSubstringTill(fieldCode, '(');
    string overstrikeElement = fieldCode.Substring(fieldCode.IndexOf(substringTill, StringComparison.Ordinal) + substringTill.Length);
    string alignment = "center";
    this.GetAlignmentProperty(substringTill, ref alignment);
    LayoutedEQFields overstrikeSwitch = new LayoutedEQFields();
    overstrikeSwitch.SwitchType = LayoutedEQFields.EQSwitchType.Overstrike;
    this.GenerateOverstrikeSwitch(fontScriptType, overstrikeSwitch, overstrikeElement, alignment, dc, charFormat, xPosition, yPosition);
    return overstrikeSwitch;
  }

  private void GenerateOverstrikeSwitch(
    FontScriptType fontScriptType,
    LayoutedEQFields overstrikeSwitch,
    string overstrikeElement,
    string alignment,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    foreach (string fieldCode in this.SplitElementsByComma(overstrikeElement))
      this.GenerateSwitch(fontScriptType, overstrikeSwitch, fieldCode, dc, charFormat.GetFontToRender(fontScriptType), charFormat, xPosition, yPosition);
    this.UpdateltEqFieldsBounds(overstrikeSwitch);
    if (!(alignment == "center") && !(alignment == "right"))
      return;
    this.AlignOverstrikeElements(overstrikeSwitch, alignment);
  }

  private void AlignOverstrikeElements(LayoutedEQFields overstrikeSwitch, string alignment)
  {
    switch (alignment)
    {
      case "right":
        for (int index = 0; index < overstrikeSwitch.ChildEQFileds.Count; ++index)
        {
          float shiftX = overstrikeSwitch.Bounds.Width - overstrikeSwitch.ChildEQFileds[index].Bounds.Width;
          if ((double) shiftX > 0.0)
            this.ShiftEqFieldXPosition(overstrikeSwitch.ChildEQFileds[index], shiftX);
        }
        break;
      case "center":
        for (int index = 0; index < overstrikeSwitch.ChildEQFileds.Count; ++index)
        {
          float num = overstrikeSwitch.Bounds.Width - overstrikeSwitch.ChildEQFileds[index].Bounds.Width;
          if ((double) num > 0.0)
            this.ShiftEqFieldXPosition(overstrikeSwitch.ChildEQFileds[index], num / 2f);
        }
        break;
    }
    this.UpdateltEqFieldsBounds(overstrikeSwitch);
  }

  private void GetAlignmentProperty(string overstrikeSwitchEqCode, ref string alignment)
  {
    string[] strArray = overstrikeSwitchEqCode.Split('\\');
    for (int index = 0; index < strArray.Length; ++index)
    {
      if (strArray[index].StartsWith("al", StringComparison.CurrentCultureIgnoreCase) || strArray[index].StartsWith("ad", StringComparison.CurrentCultureIgnoreCase))
        alignment = "left";
      else if (strArray[index].StartsWith("ar", StringComparison.CurrentCultureIgnoreCase))
        alignment = "right";
      else if (strArray[index].StartsWith("ac", StringComparison.CurrentCultureIgnoreCase))
        alignment = "center";
    }
  }

  private bool IsValidArraySwitch(string fieldCode)
  {
    if (this.HasParenthesisError(fieldCode))
      return false;
    string substringTill = this.GetSubstringTill(fieldCode, '(');
    string fieldCode1 = fieldCode.Substring(fieldCode.IndexOf(substringTill, StringComparison.Ordinal) + substringTill.Length);
    return this.IsCorrectArraySwitchSequence(substringTill) && this.IsValidEqFieldCode(new List<string>(), fieldCode1);
  }

  private bool IsCorrectArraySwitchSequence(string arrayEqCode)
  {
    if (arrayEqCode.Contains("\\ "))
      return false;
    string[] strArray = arrayEqCode.Split('\\');
    for (int index = 1; index < strArray.Length; ++index)
    {
      if (!strArray[index].StartsWith("al", StringComparison.OrdinalIgnoreCase) && !strArray[index].StartsWith("ar", StringComparison.OrdinalIgnoreCase) && !strArray[index].StartsWith("ac", StringComparison.OrdinalIgnoreCase) || !this.IsOnlyAlphabets(strArray[index].Trim()))
      {
        if (strArray[index].StartsWith("co", StringComparison.OrdinalIgnoreCase))
        {
          if (!this.IsCorrectCodeFormat(strArray[index].Trim(), true))
            return false;
        }
        else if (strArray[index].StartsWith("hs", StringComparison.OrdinalIgnoreCase) || strArray[index].StartsWith("vs", StringComparison.OrdinalIgnoreCase))
        {
          if (!this.IsCorrectCodeFormat(strArray[index].Trim(), false))
            return false;
        }
        else if (!strArray[index].TrimEnd().Equals("a", StringComparison.OrdinalIgnoreCase) || index != 1)
          return false;
      }
    }
    return true;
  }

  private LayoutedEQFields LayoutArraySwitch(
    FontScriptType fontScriptType,
    string fieldCode,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    string substringTill = this.GetSubstringTill(fieldCode, '(');
    string arrayElement = fieldCode.Substring(fieldCode.IndexOf(substringTill, StringComparison.Ordinal) + substringTill.Length);
    int numberOfColumns = 1;
    int verticalSpace = 0;
    int horizontalSpace = 0;
    StringAlignment alignment = StringAlignment.Center;
    this.GetArraySwitchProperties(substringTill, ref numberOfColumns, ref verticalSpace, ref horizontalSpace, ref alignment);
    LayoutedEQFields arraySwitch = new LayoutedEQFields();
    arraySwitch.SwitchType = LayoutedEQFields.EQSwitchType.Array;
    arraySwitch.Alignment = alignment;
    this.GenerateArraySwitch(fontScriptType, arraySwitch, arrayElement, numberOfColumns, verticalSpace, horizontalSpace, alignment, dc, charFormat, xPosition, yPosition);
    return arraySwitch;
  }

  private void GetArraySwitchProperties(
    string arraySwitchEqCode,
    ref int numberOfColumns,
    ref int verticalSpace,
    ref int horizontalSpace,
    ref StringAlignment alignment)
  {
    string[] strArray = arraySwitchEqCode.Split('\\');
    for (int index = 1; index < strArray.Length; ++index)
    {
      if (strArray[index].Trim().StartsWith("co", StringComparison.CurrentCultureIgnoreCase))
      {
        numberOfColumns = this.GetValueFromstring(strArray[index].Trim());
        if (numberOfColumns <= 0)
          numberOfColumns = 1;
      }
      else if (strArray[index].Trim().StartsWith("vs", StringComparison.CurrentCultureIgnoreCase))
      {
        verticalSpace = this.GetValueFromstring(strArray[index].Trim());
        if (verticalSpace == int.MinValue)
          verticalSpace = 0;
      }
      else if (strArray[index].Trim().StartsWith("hs", StringComparison.CurrentCultureIgnoreCase))
      {
        horizontalSpace = this.GetValueFromstring(strArray[index].Trim());
        if (horizontalSpace == int.MinValue)
          horizontalSpace = 0;
      }
      else if (strArray[index].Trim().StartsWith("al", StringComparison.CurrentCultureIgnoreCase) || strArray[index].Trim().StartsWith("ar", StringComparison.CurrentCultureIgnoreCase))
        this.GetAlignmentForArraySwitch(strArray[index].Trim(), ref alignment);
    }
  }

  private void GetAlignmentForArraySwitch(string alignmentCode, ref StringAlignment alignment)
  {
    switch (alignmentCode.ToLower())
    {
      case "al":
        alignment = StringAlignment.Near;
        break;
      case "ar":
        alignment = StringAlignment.Far;
        break;
    }
  }

  private void GenerateArraySwitch(
    FontScriptType fontScriptType,
    LayoutedEQFields arraySwitch,
    string arrayElement,
    int numberOfColumns,
    int verticalSpace,
    int horizontalSpace,
    StringAlignment alignment,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    this.GenerateArraySwitchElements(fontScriptType, arraySwitch, arrayElement, numberOfColumns, verticalSpace, horizontalSpace, alignment, dc, charFormat, xPosition, yPosition);
    int rows = 0;
    int columns = 0;
    this.GetRowsAndColumnsCount(arraySwitch, ref rows, ref columns);
    this.SetColumnWidth(arraySwitch, rows, columns);
    this.SetYForArrayElements(arraySwitch, dc, rows, columns);
    this.UpdateltEqFieldsBounds(arraySwitch);
    this.AlignArraySwitch(arraySwitch, dc, charFormat.GetFontToRender(fontScriptType));
  }

  private void GenerateArraySwitchElements(
    FontScriptType fontScriptType,
    LayoutedEQFields arraySwitch,
    string arrayElement,
    int numberOfColumns,
    int verticalSpace,
    int horizontalSpace,
    StringAlignment alignment,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    string[] strArray = this.SplitElementsByComma(arrayElement);
    int num = 0;
    int index1 = 0;
    LayoutedEQFields ltEqField = new LayoutedEQFields();
    ltEqField.SwitchType = LayoutedEQFields.EQSwitchType.Array;
    ltEqField.Alignment = alignment;
    for (int index2 = 0; index2 < strArray.Length; ++index2)
    {
      this.GenerateSwitch(fontScriptType, ltEqField, strArray[index2], dc, charFormat.GetFontToRender(fontScriptType), charFormat, xPosition, yPosition);
      RectangleF bounds = ltEqField.ChildEQFileds[index1].Bounds;
      ltEqField.ChildEQFileds[index1].Bounds = new RectangleF(bounds.X, bounds.Y, bounds.Width + (float) horizontalSpace, bounds.Height + (float) verticalSpace);
      ++index1;
      if (index1 % numberOfColumns == 0 || index2 == strArray.Length - 1)
      {
        arraySwitch.ChildEQFileds.Add(ltEqField);
        if (index2 + 1 < strArray.Length)
        {
          ++num;
          index1 = 0;
          ltEqField = new LayoutedEQFields();
        }
      }
    }
    this.UpdateltEqFieldsBounds(arraySwitch);
  }

  private void AlignArraySwitch(LayoutedEQFields arraySwitch, DrawingContext dc, Font font)
  {
    float num = (float) (-((double) dc.GetAscent(font) / 2.0) - (double) arraySwitch.Bounds.Height / 2.0);
    dc.ShiftEqFieldYPosition(arraySwitch, num - arraySwitch.Bounds.Y);
  }

  private bool HasElementInArraySwitch(LayoutedEQFields arraySwitch, int row, int column)
  {
    return row < arraySwitch.ChildEQFileds.Count && column < arraySwitch.ChildEQFileds[row].ChildEQFileds.Count;
  }

  private void GetRowsAndColumnsCount(LayoutedEQFields arraySwitch, ref int rows, ref int columns)
  {
    rows = arraySwitch.ChildEQFileds.Count;
    columns = 1;
    if (rows <= 0)
      return;
    columns = arraySwitch.ChildEQFileds[0].ChildEQFileds.Count;
  }

  private void SetYForArrayElements(
    LayoutedEQFields arraySwitch,
    DrawingContext dc,
    int rows,
    int columns)
  {
    for (int index1 = 1; index1 < rows; ++index1)
    {
      float maximumBottom = this.GetMaximumBottom(arraySwitch.ChildEQFileds[index1 - 1]);
      for (int index2 = 0; index2 < columns; ++index2)
      {
        if (this.HasElementInArraySwitch(arraySwitch, index1, index2))
        {
          LayoutedEQFields childEqFiled = arraySwitch.ChildEQFileds[index1].ChildEQFileds[index2];
          float num1 = -this.GetTopMostY(childEqFiled);
          float num2 = maximumBottom - num1;
          dc.ShiftEqFieldYPosition(childEqFiled, num2 - childEqFiled.Bounds.Y);
          float yPosition = arraySwitch.ChildEQFileds[index1 - 1].ChildEQFileds[index2].Bounds.Bottom - childEqFiled.Bounds.Y;
          if ((double) yPosition > 0.0)
            dc.ShiftEqFieldYPosition(arraySwitch.ChildEQFileds[index1], yPosition);
          this.UpdateltEqFieldsBounds(arraySwitch.ChildEQFileds[index1]);
        }
      }
    }
  }

  private void SetColumnWidth(LayoutedEQFields arraySwitch, int rows, int columns)
  {
    for (int index1 = 0; index1 < columns; ++index1)
    {
      float maximumColumnWidth = this.GetMaximumColumnWidth(arraySwitch, index1);
      for (int index2 = 0; index2 < rows; ++index2)
      {
        if (this.HasElementInArraySwitch(arraySwitch, index2, index1))
        {
          RectangleF bounds = arraySwitch.ChildEQFileds[index2].ChildEQFileds[index1].Bounds;
          float num = maximumColumnWidth - bounds.Width;
          if ((double) num > 0.0)
            arraySwitch.ChildEQFileds[index2].ChildEQFileds[index1].Bounds = new RectangleF(bounds.X, bounds.Y, bounds.Width + num, bounds.Height);
        }
      }
      if (index1 > 0 && this.HasElementInArraySwitch(arraySwitch, 0, index1))
        this.SetXforElementsInColumn(arraySwitch, index1, rows, arraySwitch.ChildEQFileds[0].ChildEQFileds[index1 - 1].Bounds.Right);
    }
    this.UpdateltEqFieldsBounds(arraySwitch);
  }

  private void SetXforElementsInColumn(
    LayoutedEQFields arraySwitch,
    int columnIndex,
    int rows,
    float xposition)
  {
    for (int index = 0; index < rows; ++index)
    {
      if (this.HasElementInArraySwitch(arraySwitch, index, columnIndex))
      {
        LayoutedEQFields childEqFiled = arraySwitch.ChildEQFileds[index].ChildEQFileds[columnIndex];
        this.ShiftEqFieldXPosition(childEqFiled, xposition - childEqFiled.Bounds.X);
        this.UpdateltEqFieldsBounds(arraySwitch.ChildEQFileds[index]);
      }
    }
  }

  private float GetMaximumColumnWidth(LayoutedEQFields arraySwitch, int columnIndex)
  {
    float maximumColumnWidth = 0.0f;
    int rows = 0;
    int columns = 0;
    this.GetRowsAndColumnsCount(arraySwitch, ref rows, ref columns);
    for (int index = 0; index < rows; ++index)
    {
      if (this.HasElementInArraySwitch(arraySwitch, index, columnIndex))
      {
        float width = arraySwitch.ChildEQFileds[index].ChildEQFileds[columnIndex].Bounds.Width;
        maximumColumnWidth = (double) maximumColumnWidth < (double) width ? width : maximumColumnWidth;
      }
    }
    return maximumColumnWidth;
  }

  private bool IsValidDisplaceSwitch(string fieldCode)
  {
    if (this.HasParenthesisError(fieldCode))
      return false;
    string substringTill = this.GetSubstringTill(fieldCode, '(');
    string fieldCode1 = fieldCode.Substring(fieldCode.IndexOf(substringTill, StringComparison.Ordinal) + substringTill.Length);
    if (substringTill.Contains("\\ "))
      return false;
    string[] strArray = substringTill.Split('\\');
    for (int index = 1; index < strArray.Length; ++index)
    {
      if (!strArray[index].StartsWith("li", StringComparison.OrdinalIgnoreCase) || !this.IsOnlyAlphabets(strArray[index].Trim()))
      {
        if (strArray[index].StartsWith("fo", StringComparison.OrdinalIgnoreCase) || strArray[index].StartsWith("ba", StringComparison.OrdinalIgnoreCase))
        {
          if (!this.IsCorrectCodeFormat(strArray[index].Trim(), true))
            return false;
        }
        else if (!strArray[index].TrimEnd().Equals("d", StringComparison.OrdinalIgnoreCase) || index != 1)
          return false;
      }
    }
    return this.IsValidEqFieldCode(new List<string>(), fieldCode1);
  }

  private LayoutedEQFields LayoutDisplaceSwitch(
    FontScriptType fontScriptType,
    string fieldCode,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    string substringTill = this.GetSubstringTill(fieldCode, '(');
    string displaceElement = fieldCode.Substring(fieldCode.IndexOf(substringTill, StringComparison.Ordinal) + substringTill.Length);
    int shiftValue = 0;
    bool hasLine = false;
    this.GetDisplaceSwitchProperties(substringTill, ref shiftValue, ref hasLine);
    LayoutedEQFields displaceSwitch = new LayoutedEQFields();
    displaceSwitch.SwitchType = LayoutedEQFields.EQSwitchType.Displace;
    this.GenerateDisplaceSwitch(fontScriptType, displaceSwitch, displaceElement, shiftValue, hasLine, dc, charFormat, xPosition, yPosition);
    return displaceSwitch;
  }

  private void GenerateDisplaceSwitch(
    FontScriptType fontScriptType,
    LayoutedEQFields displaceSwitch,
    string displaceElement,
    int shiftValue,
    bool hasLine,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    foreach (string fieldCode in this.SplitElementsByComma(displaceElement))
    {
      this.GenerateSwitch(fontScriptType, displaceSwitch, fieldCode, dc, charFormat.GetFontToRender(fontScriptType), charFormat, xPosition, yPosition);
      xPosition = displaceSwitch.Bounds.Right;
    }
    this.ShiftDisplaceSwitch(fontScriptType, displaceSwitch, shiftValue, hasLine, dc, charFormat, xPosition, yPosition);
  }

  private void ShiftDisplaceSwitch(
    FontScriptType fontScriptType,
    LayoutedEQFields displaceSwitch,
    int shiftValue,
    bool hasLine,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    if (hasLine)
      this.GenerateLineInDisplaceSwitch(fontScriptType, displaceSwitch, dc, charFormat, displaceSwitch.Bounds.Right, displaceSwitch.Bounds.Bottom, -shiftValue);
    if (shiftValue > 0)
    {
      TextEQField ltEqField = new TextEQField();
      this.GenerateSpaceForWidth(fontScriptType, ltEqField, (float) shiftValue, dc, charFormat, xPosition, yPosition);
      this.ShiftEqFieldXPosition(displaceSwitch, (float) shiftValue);
      this.SetZeroWidth(displaceSwitch);
      displaceSwitch.ChildEQFileds.Add((LayoutedEQFields) ltEqField);
      this.UpdateltEqFieldsBounds(displaceSwitch);
      displaceSwitch.Bounds = new RectangleF(displaceSwitch.Bounds.X, displaceSwitch.Bounds.Y, (float) shiftValue, displaceSwitch.Bounds.Height);
    }
    else
    {
      this.ShiftEqFieldXPosition(displaceSwitch, (float) shiftValue);
      this.SetZeroWidth(displaceSwitch);
    }
  }

  private void GenerateLineInDisplaceSwitch(
    FontScriptType fontScriptType,
    LayoutedEQFields displaceSwitch,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition,
    int length)
  {
    LineEQField lineEqField = new LineEQField();
    float num = dc.MeasureString(" ", charFormat.GetFontToRender(fontScriptType), (StringFormat) null, charFormat, false).Height - dc.GetAscent(charFormat.GetFontToRender(fontScriptType));
    lineEqField.Point1 = new PointF(xPosition, yPosition - num);
    lineEqField.Point2 = new PointF(lineEqField.Point1.X + (float) length, lineEqField.Point1.Y);
    displaceSwitch.ChildEQFileds.Add((LayoutedEQFields) lineEqField);
  }

  private void SetZeroWidth(LayoutedEQFields ltEqFields)
  {
    switch (ltEqFields)
    {
      case TextEQField _:
      case LineEQField _:
        ltEqFields.Bounds = new RectangleF(ltEqFields.Bounds.X, ltEqFields.Bounds.Y, 0.0f, ltEqFields.Bounds.Height);
        break;
      case null:
        break;
      default:
        ltEqFields.Bounds = new RectangleF(ltEqFields.Bounds.X, ltEqFields.Bounds.Y, 0.0f, ltEqFields.Bounds.Height);
        for (int index = 0; index < ltEqFields.ChildEQFileds.Count; ++index)
          this.SetZeroWidth(ltEqFields.ChildEQFileds[index]);
        break;
    }
  }

  private void GenerateSpaceForWidth(
    FontScriptType fontScriptType,
    TextEQField ltEqField,
    float maxWidth,
    DrawingContext dc,
    WCharacterFormat charFormat,
    float xPosition,
    float yPosition)
  {
    string str = "";
    while ((double) maxWidth > (double) ltEqField.Bounds.Width)
    {
      ltEqField.Text = str;
      ltEqField.Bounds = new RectangleF(xPosition, yPosition, dc.MeasureString(ltEqField.Text, charFormat.GetFontToRender(fontScriptType), (StringFormat) null, charFormat, false).Width, dc.MeasureString(ltEqField.Text, charFormat.GetFontToRender(fontScriptType), (StringFormat) null, charFormat, false).Height);
      str += " ";
    }
    float ascent = dc.GetAscent(charFormat.GetFontToRender(fontScriptType));
    dc.ShiftEqFieldYPosition((LayoutedEQFields) ltEqField, -ascent);
  }

  private void GetDisplaceSwitchProperties(
    string displaceSwitchEqCode,
    ref int shiftValue,
    ref bool hasLine)
  {
    string[] strArray = displaceSwitchEqCode.Split('\\');
    for (int index = 1; index < strArray.Length; ++index)
    {
      if (strArray[index].Trim().StartsWith("fo", StringComparison.CurrentCultureIgnoreCase))
      {
        shiftValue = this.GetValueFromstring(strArray[index].Trim());
        if (shiftValue == int.MinValue)
          shiftValue = 0;
      }
      else if (strArray[index].Trim().StartsWith("ba", StringComparison.CurrentCultureIgnoreCase))
      {
        shiftValue = this.GetValueFromstring(strArray[index].Trim());
        if (shiftValue == int.MinValue)
          shiftValue = 0;
        shiftValue = -shiftValue;
      }
      else if (strArray[index].Trim().StartsWith("li", StringComparison.CurrentCultureIgnoreCase))
        hasLine = true;
    }
  }

  private bool IsCorrectCodeFormat(string eqCode, bool isAlsoNegative)
  {
    eqCode = eqCode.Trim();
    string valuePart = "";
    this.SplitTextAndInteger(eqCode, ref valuePart);
    int result;
    return !(valuePart != "") || int.TryParse(valuePart, out result) && (isAlsoNegative || result >= 0) && (result <= 0 || !valuePart.Trim().Contains("+"));
  }

  private void SplitTextAndInteger(string originalString, ref string valuePart)
  {
    string str = "";
    foreach (char c in originalString)
    {
      if (char.IsLetter(c))
      {
        str += (string) (object) c;
      }
      else
      {
        valuePart = originalString.Substring(originalString.IndexOf(str, StringComparison.Ordinal) + str.Length).TrimStart();
        break;
      }
    }
  }

  private float GetFontSizeFromHeight(
    string text,
    ref float maxHeight,
    DrawingContext dc,
    WCharacterFormat charFormat)
  {
    maxHeight += maxHeight * 0.25f;
    float fontSize = 11f;
    float num = 0.0f;
    while ((double) num < (double) maxHeight)
    {
      num = dc.MeasureString(text, this.Document.FontSettings.GetFont("Symbol", fontSize, FontStyle.Regular), (StringFormat) null, charFormat, false).Height;
      if ((double) num < (double) maxHeight)
        fontSize += 0.1f;
    }
    return fontSize;
  }

  private void EQFieldVerticalAlignment(
    LayoutedEQFields movableEqField,
    LayoutedEQFields standardEqField,
    DrawingContext dc)
  {
    float yPosition = standardEqField.Bounds.Top - movableEqField.Bounds.Top;
    if ((double) yPosition != 0.0)
      dc.ShiftEqFieldYPosition(movableEqField, yPosition);
    float num = movableEqField.Bounds.Height - standardEqField.Bounds.Height;
    dc.ShiftEqFieldYPosition(movableEqField, (float) -((double) num / 2.0));
  }

  private char GetSeparator() => CultureInfo.CurrentCulture.TextInfo.ListSeparator[0];

  private string[] SplitElementsByComma(string element)
  {
    element = element.Trim();
    int num = 1;
    List<string> stringList = new List<string>();
    List<int> indexOfBackSlashes = this.GetIndexOfBackSlashes(element);
    string str = "";
    for (int index = 1; index <= element.Length - 2; ++index)
    {
      if (element[index] == '(' && !indexOfBackSlashes.Contains(index - 1))
        ++num;
      else if (element[index] == ')' && !indexOfBackSlashes.Contains(index - 1))
        --num;
      if ((int) element[index] == (int) this.GetSeparator() && !indexOfBackSlashes.Contains(index - 1) && num == 1)
      {
        stringList.Add(str);
        str = "";
      }
      else
        str += (string) (object) element[index];
    }
    stringList.Add(str);
    return stringList.ToArray();
  }

  private void UpdateltEqFieldsBounds(LayoutedEQFields ltEQfields)
  {
    float num = -this.GetTopMostY(ltEQfields);
    float maximumBottom = this.GetMaximumBottom(ltEQfields);
    float leftMostX = this.GetLeftMostX(ltEQfields);
    ltEQfields.Bounds = new RectangleF(leftMostX, -num, -leftMostX + this.GetMaximumRight(ltEQfields), num + maximumBottom);
  }

  private void ShiftEqFieldXPosition(LayoutedEQFields layoutedEqFields, float shiftX)
  {
    switch (layoutedEqFields)
    {
      case TextEQField _:
        TextEQField textEqField = layoutedEqFields as TextEQField;
        textEqField.Bounds = new RectangleF(textEqField.Bounds.X + shiftX, textEqField.Bounds.Y, textEqField.Bounds.Width, textEqField.Bounds.Height);
        break;
      case LineEQField _:
        LineEQField lineEqField = layoutedEqFields as LineEQField;
        lineEqField.Bounds = new RectangleF(lineEqField.Bounds.X + shiftX, lineEqField.Bounds.Y, lineEqField.Bounds.Width, lineEqField.Bounds.Height);
        lineEqField.Point1 = new PointF(lineEqField.Point1.X + shiftX, lineEqField.Point1.Y);
        lineEqField.Point2 = new PointF(lineEqField.Point2.X + shiftX, lineEqField.Point2.Y);
        break;
      case null:
        break;
      default:
        layoutedEqFields.Bounds = new RectangleF(layoutedEqFields.Bounds.X + shiftX, layoutedEqFields.Bounds.Y, layoutedEqFields.Bounds.Width, layoutedEqFields.Bounds.Height);
        using (List<LayoutedEQFields>.Enumerator enumerator = layoutedEqFields.ChildEQFileds.GetEnumerator())
        {
          while (enumerator.MoveNext())
            this.ShiftEqFieldXPosition(enumerator.Current, shiftX);
          break;
        }
    }
  }

  private bool HasAnySwitch(string fieldCode)
  {
    string[] strArray = new string[10]
    {
      "\\a",
      "\\b",
      "\\d",
      "\\f",
      "\\i",
      "\\o",
      "\\r",
      "\\s",
      "\\l",
      "\\x"
    };
    foreach (string str in strArray)
    {
      if (fieldCode.ToLower().Contains(str))
        return true;
    }
    return false;
  }

  private string ExtractSwitch(string fieldCode)
  {
    int num1 = 0;
    int num2 = 0;
    fieldCode = fieldCode.TrimStart();
    List<int> indexOfBackSlashes = this.GetIndexOfBackSlashes(fieldCode);
    for (int index = 0; index < fieldCode.Length; ++index)
    {
      if (fieldCode[index] == '(' && !indexOfBackSlashes.Contains(index - 1))
        ++num1;
      else if (fieldCode[index] == ')' && !indexOfBackSlashes.Contains(index - 1))
        ++num2;
      if (num1 == num2 && num1 != 0 && num2 != 0)
        return fieldCode.Substring(0, index + 1);
    }
    return fieldCode;
  }

  private void GetSplittedFieldCode(List<string> splittedFieldCodeSwitch, string fieldCode)
  {
    string switchBeforeText = this.GetSwitchBeforeText(fieldCode);
    if (switchBeforeText != "")
    {
      fieldCode = fieldCode.Substring(switchBeforeText.Length);
      splittedFieldCodeSwitch.Add(switchBeforeText);
    }
    string str = this.ExtractSwitch(fieldCode);
    string fieldCode1 = fieldCode.Substring(str.Length);
    if (str != "")
      splittedFieldCodeSwitch.Add(str);
    if (fieldCode1 != "" && this.HasAnySwitch(fieldCode1))
    {
      this.GetSplittedFieldCode(splittedFieldCodeSwitch, fieldCode1);
    }
    else
    {
      if (!(fieldCode1 != ""))
        return;
      splittedFieldCodeSwitch.Add(fieldCode1);
    }
  }

  private float GetTopMostY(LayoutedEQFields ltEqFields)
  {
    float y = ltEqFields.Bounds.Y;
    for (int index = 0; index < ltEqFields.ChildEQFileds.Count; ++index)
    {
      if (index == 0)
        y = ltEqFields.ChildEQFileds[index].Bounds.Y;
      else if ((double) ltEqFields.ChildEQFileds[index].Bounds.Y < (double) y)
        y = ltEqFields.ChildEQFileds[index].Bounds.Y;
    }
    return y;
  }

  private float GetLeftMostX(LayoutedEQFields ltEqFields)
  {
    float x = ltEqFields.Bounds.X;
    for (int index = 0; index < ltEqFields.ChildEQFileds.Count; ++index)
    {
      if (index == 0)
        x = ltEqFields.ChildEQFileds[index].Bounds.X;
      else if ((double) ltEqFields.ChildEQFileds[index].Bounds.X < (double) x)
        x = ltEqFields.ChildEQFileds[index].Bounds.X;
    }
    return x;
  }

  private float GetMaximumRight(LayoutedEQFields ltEqFields)
  {
    float right = ltEqFields.Bounds.Right;
    for (int index = 0; index < ltEqFields.ChildEQFileds.Count; ++index)
    {
      if (index == 0)
        right = ltEqFields.ChildEQFileds[index].Bounds.Right;
      else if ((double) ltEqFields.ChildEQFileds[index].Bounds.Right > (double) right)
        right = ltEqFields.ChildEQFileds[index].Bounds.Right;
    }
    return right;
  }

  private bool HasManyCharacters(string fieldcode)
  {
    return this.StartsWithExt(fieldcode, " ") ? !(fieldcode.Trim() == "") : fieldcode.TrimEnd().Length > 1;
  }

  private string GetSubstringTill(string originalString, char delimeter)
  {
    string substringTill = "";
    for (int index = 0; index < originalString.Length && ((int) originalString[index] != (int) delimeter || (index <= 0 || originalString[index - 1] == '\\') && (index <= 1 || originalString[index - 1] != '\\' || originalString[index - 2] != '\\')); ++index)
      substringTill += (string) (object) originalString[index];
    return substringTill;
  }

  private bool HasSlashError(string fieldCode)
  {
    List<int> indexOfBackSlashes = this.GetIndexOfBackSlashes(fieldCode);
    char[] charArray = new char[4]
    {
      this.GetSeparator(),
      '(',
      ')',
      '\\'
    };
    foreach (int num in indexOfBackSlashes)
    {
      char c = fieldCode[num + 1];
      if (!this.IsExistInArray(charArray, char.ToLower(c)))
        return true;
    }
    return false;
  }

  private List<int> GetIndexOfBackSlashes(string fieldCode)
  {
    List<int> indexOfBackSlashes = new List<int>();
    for (int index = fieldCode.IndexOf('\\'); index > -1; index = fieldCode.IndexOf('\\', index + 1))
    {
      if (indexOfBackSlashes.Count == 0)
        indexOfBackSlashes.Add(index);
      else if (indexOfBackSlashes[indexOfBackSlashes.Count - 1] != index - 1)
        indexOfBackSlashes.Add(index);
    }
    return indexOfBackSlashes;
  }

  private bool HasParenthesisError(string element)
  {
    int num1 = 0;
    int num2 = 0;
    List<int> indexOfBackSlashes = this.GetIndexOfBackSlashes(element);
    for (int index = 0; index < element.Length; ++index)
    {
      if (element[index] == '(' && index == 0)
        ++num1;
      else if (element[index] == '(' && !indexOfBackSlashes.Contains(index - 1))
        ++num1;
      else if (element[index] == ')' && !indexOfBackSlashes.Contains(index - 1))
        ++num2;
    }
    return num1 > num2;
  }

  private bool IsOnlyAlphabets(string inputText)
  {
    foreach (char c in inputText)
    {
      if (!char.IsLetter(c))
        return false;
    }
    return true;
  }

  private string GetSwitchBeforeText(string eqFieldCode)
  {
    if (!eqFieldCode.Contains("\\"))
      return eqFieldCode;
    List<int> indexOfBackSlashes = this.GetIndexOfBackSlashes(eqFieldCode);
    char[] charArray = new char[10]
    {
      'a',
      'b',
      'd',
      'f',
      'i',
      'o',
      'r',
      's',
      'l',
      'x'
    };
    foreach (int length in indexOfBackSlashes)
    {
      char c = eqFieldCode[length + 1];
      if (this.IsExistInArray(charArray, char.ToLower(c)))
        return eqFieldCode.Substring(0, length);
    }
    return "";
  }

  private string GetFirstOccurenceEqSwitch(string eqInstruction)
  {
    string[] strArray = new string[10]
    {
      "\\a",
      "\\b",
      "\\d",
      "\\f",
      "\\i",
      "\\o",
      "\\r",
      "\\s",
      "\\l",
      "\\x"
    };
    string str1 = "";
    for (int index = 0; index < eqInstruction.Length; ++index)
    {
      str1 += (string) (object) eqInstruction[index];
      int num = str1.LastIndexOf('\\');
      if (num == str1.Length - 2 && num != -1)
      {
        foreach (string occurenceEqSwitch in strArray)
        {
          string str2 = str1.Substring(0, str1.LastIndexOf('\\'));
          if (str1.ToLower().Contains(occurenceEqSwitch) && !str2.EndsWith("\\"))
            return occurenceEqSwitch;
        }
      }
    }
    return "";
  }

  private string RemoveEQText(string fieldCode)
  {
    string eqFieldCode = fieldCode;
    if (fieldCode.TrimStart().StartsWith("EQ", StringComparison.OrdinalIgnoreCase))
      eqFieldCode = fieldCode.Substring(fieldCode.IndexOf("EQ", StringComparison.Ordinal) + "EQ".Length).TrimStart();
    if (this.StartsWithExt(eqFieldCode.Trim(), "\\*"))
    {
      string switchBeforeText = this.GetSwitchBeforeText(eqFieldCode);
      eqFieldCode = eqFieldCode.Substring(switchBeforeText.Length);
    }
    return eqFieldCode;
  }

  private string ReplaceSymbols(string text)
  {
    List<int> indexOfBackSlashes = this.GetIndexOfBackSlashes(text);
    string str = "";
    for (int index = 0; index < text.Length; ++index)
    {
      if (!indexOfBackSlashes.Contains(index))
        str += (string) (object) text[index];
    }
    return str;
  }

  private bool IsExistInArray(char[] charArray, char searchingChar)
  {
    foreach (int num in charArray)
    {
      if (num == (int) searchingChar)
        return true;
    }
    return false;
  }

  internal WCharacterFormat GetCharacterFormatValue()
  {
    if (this.FormattingString.ToUpper().Contains("\\* MERGEFORMAT"))
    {
      if (this.FieldSeparator != null && this.FieldSeparator.NextSibling is WTextRange)
        return (this.FieldSeparator.NextSibling as WTextRange).CharacterFormat;
    }
    else if (this.FieldType == FieldType.FieldNumPages || this.FieldType == FieldType.FieldSectionPages || this.FieldType == FieldType.FieldPage)
      return this.GetFirstFieldCodeItem().CharacterFormat;
    return this.CharacterFormat;
  }

  internal WCharacterFormat GetCharacterFormatValue(int paraItemIndex)
  {
    int index = this.Range.Items.IndexOf((object) this.FieldSeparator) + paraItemIndex;
    if (index < this.Range.Items.IndexOf((object) this.FieldEnd))
    {
      if (this.FieldSeparator != null && this.Range.Count >= index && this.Range.InnerList[index] is WTextRange)
        return (this.Range.InnerList[index] as WTextRange).CharacterFormat;
      if (this.FieldSeparator != null && this.Range.InnerList[index] is WFieldMark && (this.Range.InnerList[index] as WFieldMark).Type == FieldMarkType.FieldEnd)
        return (this.Range.InnerList[index - 1] as WTextRange).CharacterFormat;
    }
    else if (this.FieldEnd != null && this.FieldEnd.PreviousSibling is WTextRange)
      return (this.FieldEnd.PreviousSibling as WTextRange).CharacterFormat;
    return this.CharacterFormat;
  }

  internal FontScriptType GetFontScriptType()
  {
    if (this.FormattingString.ToUpper().Contains("\\* MERGEFORMAT"))
    {
      if (this.FieldSeparator != null && this.FieldSeparator.NextSibling is WTextRange)
        return (this.FieldSeparator.NextSibling as WTextRange).ScriptType;
    }
    else if (this.FieldType == FieldType.FieldNumPages || this.FieldType == FieldType.FieldSectionPages || this.FieldType == FieldType.FieldPage)
      return this.GetFirstFieldCodeItem().ScriptType;
    return FontScriptType.English;
  }

  private WTextRange GetFirstFieldCodeItem()
  {
    for (IEntity nextSibling = this.NextSibling; nextSibling != null; nextSibling = nextSibling.NextSibling)
    {
      if (nextSibling is WTextRange && !string.IsNullOrEmpty((nextSibling as WTextRange).Text))
        return this.StartsWithExt((nextSibling as WTextRange).Text, " ") || this.GetOwnerTextBody((Entity) this) is HeaderFooter && (this.FieldType == FieldType.FieldPage || this.FieldType == FieldType.FieldNumPages) ? nextSibling as WTextRange : (WTextRange) this;
      if (nextSibling == this.FieldSeparator || nextSibling == this.FieldEnd)
        return (WTextRange) this;
    }
    return (WTextRange) this;
  }

  ILayoutInfo IWidget.LayoutInfo
  {
    get
    {
      if (this.m_layoutInfo == null)
        this.CreateLayoutInfo();
      return this.m_layoutInfo;
    }
  }

  internal bool IsBookmarkCrossRefField(ref string bkName)
  {
    string[] strArray = this.InternalFieldCode.Split(new char[1]
    {
      ' '
    }, StringSplitOptions.RemoveEmptyEntries);
    if (strArray.Length > 2 && this.InternalFieldCode.Contains("\\h"))
    {
      Bookmark byName = this.Document.Bookmarks.FindByName(strArray[1]);
      if (byName != null && byName.BookmarkStart != null && byName.BookmarkEnd != null)
      {
        bkName = byName.Name;
        return true;
      }
    }
    return false;
  }

  void IWidget.InitLayoutInfo() => this.m_layoutInfo = (ILayoutInfo) null;

  void IWidget.InitLayoutInfo(IWidget widget)
  {
  }

  internal WTextRange GetCurrentTextRange()
  {
    if (this.FormattingString.ToUpper().Contains("\\* MERGEFORMAT"))
    {
      if (this.FieldSeparator != null && this.FieldSeparator.NextSibling is WTextRange)
        return this.FieldSeparator.NextSibling as WTextRange;
    }
    else if (this.FieldType == FieldType.FieldPage || this.FieldType == FieldType.FieldNumPages || this.FieldType == FieldType.FieldSectionPages)
      return this.GetFirstFieldCodeItem();
    return (WTextRange) this;
  }

  internal BookmarkStart GetBookmarkOfCrossRefField(ref bool isHiddenBookmark)
  {
    string fieldCode = this.FieldCode;
    string[] separator1 = new string[1]{ "\\*" };
    foreach (string str in fieldCode.Split(separator1, StringSplitOptions.RemoveEmptyEntries))
    {
      char[] separator2 = new char[1]{ ' ' };
      string[] strArray = str.Split(separator2, StringSplitOptions.RemoveEmptyEntries);
      for (int index = 1; index < strArray.Length; ++index)
      {
        if (!strArray[index].Contains("\\"))
        {
          Bookmark byName = this.Document.Bookmarks.FindByName(strArray[index]);
          if (byName == null)
            return (BookmarkStart) null;
          isHiddenBookmark = this.StartsWithExt(byName.Name, "_");
          if (!isHiddenBookmark && byName.BookmarkStart != null && byName.BookmarkEnd != null)
            return byName.BookmarkStart;
        }
      }
    }
    return (BookmarkStart) null;
  }

  internal bool StartsWithExt(string text, string value) => text.StartsWithExt(value);

  internal enum Month
  {
    January = 1,
    February = 2,
    March = 3,
    April = 4,
    May = 5,
    June = 6,
    July = 7,
    August = 8,
    September = 9,
    October = 10, // 0x0000000A
    November = 11, // 0x0000000B
    December = 12, // 0x0000000C
  }
}
