// Decompiled with JetBrains decompiler
// Type: RichTextReader
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO;
using Syncfusion.XlsIO.Implementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
public class RichTextReader
{
  private const string GroupStart = "{";
  private const string GroupEnd = "}";
  private const string ControlStart = "\\";
  private const string Space = " ";
  private const string CarriegeReturn = "\r";
  private const string NewLine = "\n";
  private const string SemiColon = ";";
  public const string CrLf = "\r\n";
  internal const string Tab = "\t";
  internal const string WindowsCodePage = "Windows-1252";
  internal const char NonBreakingHyphen = '\u001E';
  internal const char SoftHyphen = '\u001F';
  internal const char NonBreakingSpace = ' ';
  private string m_rtfText;
  private WorksheetImpl m_sheet;
  private IRange m_range;
  private RichTextString m_rtf;
  private IApplication m_application;
  private WorkbookImpl m_book;
  private RtfLexer m_lexer;
  private RtfReader m_rtfReader;
  private string m_token;
  private string m_previousToken;
  private string m_previousTokenKey;
  private string m_previousTokenValue;
  private string m_previousControlString;
  private string m_fontName;
  private RtfTableType m_currentTableType = RtfTableType.None;
  private Dictionary<string, IFont> m_fontTable = new Dictionary<string, IFont>();
  private Dictionary<int, RichTextReader.RtfColor> m_colorTable = new Dictionary<int, RichTextReader.RtfColor>();
  private Stack<string> m_stack = new Stack<string>();
  private Stack<string> m_pictureStack = new Stack<string>();
  private Stack<string> m_destStack = new Stack<string>();
  private Stack<string> m_headerFooterStack = new Stack<string>();
  private IFont m_rtfFont;
  private IFont m_baseFont;
  private int m_fontNumber;
  private int m_fontChar;
  private string m_fontID;
  private RtfTokenType m_tokenType;
  private RtfTokenType m_prevTokenType;
  private RichTextReader.RtfColor m_rtfColorTable;
  private bool m_rtfBold;
  private bool m_rtfItalic;
  private ExcelUnderline m_rtfUnderline;
  private bool m_rtfStrikethrough;
  private bool m_rtfSubscript;
  private bool m_rtfSuperscript;
  private int m_emptyParaCount;
  private bool m_bIsPicture;
  private bool m_bIsDocumentInfo;
  private bool m_bIsShapePicture;
  private bool m_bIsListText;
  private bool m_bIsBookmarkStart;
  private bool m_bIsBookmarkEnd;
  private bool m_bIsHeader;
  private bool m_bIsFooter;
  private bool m_bIsCustomProperties;
  private string m_currStyleID;
  private Dictionary<int, RichTextReader.TabFormat> m_tabCollection = new Dictionary<int, RichTextReader.TabFormat>();
  private int m_tabCount;
  private bool m_bIsAccentChar;
  private Stack<string> m_backgroundCollectionStack = new Stack<string>();
  private bool m_bIsBackgroundCollection;
  private string m_styleName;
  private Stack<string> m_listLevelStack = new Stack<string>();
  private bool m_bIsListLevel;
  private Stack<int> m_unicodeCountStack = new Stack<int>();
  private int m_unicodeCount;
  private int m_currColorIndex = -1;
  private Stack<Dictionary<int, RichTextReader.TabFormat>> m_tabFormatStack = new Stack<Dictionary<int, RichTextReader.TabFormat>>();
  private Stack<string> m_rtfCollectionStack = new Stack<string>();
  private Stack<string> m_shapeInstructionStack = new Stack<string>();
  private bool m_bIsShapeInstruction;
  private Stack<string> m_objectStack = new Stack<string>();
  private bool m_bIsObject;
  private string m_drawingFieldName;
  private string m_drawingFieldValue;
  private Stack<int> m_fieldResultGroupStack = new Stack<int>();
  private Stack<int> m_fieldInstructionGroupStack = new Stack<int>();
  private Stack<int> m_fieldGroupStack = new Stack<int>();
  private Stack<string> m_formFieldDataStack = new Stack<string>();
  private string m_currentFieldGroupData;
  private string m_defaultCodePage;
  private bool isWrapPolygon;
  private bool m_isCommentRangeStart;
  private bool m_isCommentReference;
  private Stack<string> m_commentGroupCollection = new Stack<string>();
  private RichTextReader.Tokens token;
  private RichTextReader.Groups m_group;
  private List<RichTextReader.Groups> m_groupOrder;

  internal bool IsDestinationControlWord => this.m_destStack.Count > 0;

  internal bool IsFormFieldGroup => this.m_formFieldDataStack.Count > 0;

  internal bool IsFieldGroup => this.m_fieldGroupStack.Count > 0;

  internal string DefaultCodePage
  {
    get
    {
      if (this.m_defaultCodePage != null)
        return this.m_defaultCodePage;
      return this.IsSupportedCodePage(CultureInfo.CurrentCulture.TextInfo.ANSICodePage) ? this.GetSupportedCodePage(CultureInfo.CurrentCulture.TextInfo.ANSICodePage) : "windows-1252";
    }
    set => this.m_defaultCodePage = value;
  }

  public RichTextReader(IWorksheet parentSheet)
  {
    this.m_sheet = parentSheet as WorksheetImpl;
    this.m_book = parentSheet.Workbook as WorkbookImpl;
    this.m_application = parentSheet.Application;
  }

  internal void ParseToken()
  {
    this.m_rtfReader = new RtfReader((Stream) new MemoryStream(Encoding.UTF8.GetBytes(this.m_rtfText)));
    this.m_lexer = new RtfLexer(this.m_rtfReader);
    int index1 = 0;
    this.m_groupOrder = new List<RichTextReader.Groups>();
    this.m_rtfFont = (IFont) null;
    this.m_token = this.m_lexer.ReadNextToken(this.m_previousTokenKey);
    int num1 = 0;
    while ((long) this.m_rtfReader.Position <= this.m_rtfReader.Length)
    {
      if (this.m_token == "{")
      {
        this.m_group = new RichTextReader.Groups();
        this.m_groupOrder.Add(this.m_group);
        ++index1;
        this.m_tokenType = RtfTokenType.GroupStart;
        this.ParseGroupStart();
        if (this.m_isCommentReference)
          this.m_commentGroupCollection.Push(this.m_token);
      }
      else if (this.m_token == "}")
      {
        --index1;
        if (this.m_previousToken == "par")
        {
          this.IsPnListStyleDefined(this.m_groupOrder[this.m_groupOrder.Count - 1]);
          this.m_groupOrder.RemoveRange(index1, 1);
        }
        else if (index1 != 0)
        {
          for (int index2 = 0; index2 < this.m_groupOrder[index1].ChildElements.Count; ++index2)
          {
            if (this.m_groupOrder[index1].ChildElements[index2] is RichTextReader.Tokens childElement && childElement.TokenName == "cf")
              this.m_rtfColorTable = (RichTextReader.RtfColor) null;
          }
          this.m_groupOrder[index1 - 1].ChildElements.Add(this.m_groupOrder[index1]);
          this.m_groupOrder.RemoveRange(index1, 1);
        }
        this.m_tokenType = RtfTokenType.GroupEnd;
        this.ParseGroupEnd();
        if (index1 <= 1)
          this.m_rtfFont = (IFont) null;
        if (this.m_rtfCollectionStack.Count == 0)
          break;
      }
      else if (this.m_token == ";")
      {
        if (this.m_previousToken == "colortbl")
          this.m_currColorIndex = 0;
        this.m_tokenType = RtfTokenType.Unknown;
        this.m_lexer.CurrRtfTokenType = RtfTokenType.Unknown;
        if (this.m_currentTableType == RtfTableType.ColorTable && this.m_previousTokenKey == "blue")
          this.AddColorTableEntry();
        else if (this.m_currentTableType == RtfTableType.FontTable)
          this.AddFontTableEntry();
        else if (this.m_currentTableType != RtfTableType.StyleSheet)
          this.ParseDocumentElement(this.m_token);
      }
      else if (this.m_token.StartsWith("\\"))
      {
        this.m_group = this.m_groupOrder[this.m_groupOrder.Count - 1];
        this.m_tokenType = RtfTokenType.ControlWord;
        this.ParseControlStart();
      }
      else if (this.m_token == "\r" || this.m_token == "\n" || this.m_token == string.Empty)
        this.m_tokenType = RtfTokenType.Unknown;
      else
        this.ParseDocumentElement(this.m_token);
      if (this.m_rtf.Text != null && this.m_rtf.Text.Length > 0)
      {
        string str = this.m_rtf.Text.Substring(num1, this.m_rtf.Text.Length - num1);
        if (str.Contains("�") && str.Length > 1)
        {
          int num2 = str.IndexOf('�');
          num1 += num2;
          if (this.m_token == "par")
          {
            IFont font1 = this.m_rtf.GetFont(num1);
            IFont font2;
            if (font1 == null && this.m_baseFont != null)
            {
              font2 = this.m_book.CreateFont(this.m_baseFont);
              (font2 as FontImpl).CharSet = (byte) (this.m_baseFont as FontWrapper).CharSet;
            }
            else
              font2 = font1 != null ? this.m_book.CreateFont(font1) : this.m_book.CreateFont();
            font2.RGBColor = this.m_rtfFont.RGBColor;
            font2.Bold = this.m_rtfFont.Bold;
            font2.Italic = this.m_rtfFont.Italic;
            font2.Underline = ExcelUnderline.None;
            font2.Strikethrough = this.m_rtfFont.Strikethrough;
            font2.Subscript = this.m_rtfFont.Subscript;
            font2.Superscript = this.m_rtfFont.Superscript;
            this.m_rtf.SetFont(num1, num1, font2);
            ++num1;
          }
        }
      }
      if (this.m_token != null && this.m_token != "\r" && this.m_token != "\n")
        this.m_previousControlString = this.m_token;
      if (this.m_token != null && this.m_tokenType == RtfTokenType.ControlWord && this.m_token != " ")
        this.m_previousToken = this.m_token;
      if (this.m_token == "emdash" || this.m_token == "endash")
        this.m_prevTokenType = RtfTokenType.Text;
      else if (this.m_prevTokenType != RtfTokenType.Text || !(this.m_token == "\r") && !(this.m_token == "\n"))
        this.m_prevTokenType = this.m_tokenType;
      this.m_token = this.m_lexer.ReadNextToken(this.m_previousTokenKey);
      this.SetParsedElementFlag(this.m_token);
      if (this.m_token.Contains("\\colortbl") && this.m_colorTable.Count > 0)
        this.SkipGroup();
      if (this.m_token.Contains("\\macpict"))
        this.SkipGroup();
      if (this.m_token.Contains("\\footnote"))
        this.SkipGroup();
      if (this.m_token.Contains("\\txfieldtext") && this.m_previousControlString == "*")
        this.SkipGroup();
      if (this.m_token.Contains("\\formfield"))
        this.m_formFieldDataStack.Push("{");
      if (this.m_token.StartsWith("\\jpegblip") || this.m_token.StartsWith("\\wmetafile") || this.m_token.StartsWith("\\blipuid") || this.m_token.StartsWith("\\pngblip") || this.m_token.StartsWith("\\emfblip") || this.m_token.StartsWith("\\macpict") || this.m_token.StartsWith("\\objdata"))
        this.m_lexer.IsImageBytes = true;
    }
    if (this.m_previousToken != "sv" && this.isWrapPolygon)
      this.isWrapPolygon = false;
    this.m_lexer.Close();
    this.Close();
  }

  private bool IsPnListStyleDefined(RichTextReader.Groups group)
  {
    bool flag = false;
    for (int index = 0; index < group.ChildElements.Count; ++index)
    {
      RichTextReader.Groups childElement1 = group.ChildElements[index];
      if (flag)
        return flag;
      foreach (object childElement2 in childElement1.ChildElements)
      {
        if (childElement2 is RichTextReader.Tokens)
        {
          RichTextReader.Tokens tokens = childElement2 as RichTextReader.Tokens;
          if (tokens.TokenName == "pnlvlbody" || tokens.TokenName == "pnlvlcont" || tokens.TokenName == "pnlvlblt")
            flag = true;
        }
      }
    }
    return flag;
  }

  internal void Close()
  {
    this.m_lexer = (RtfLexer) null;
    this.m_rtfReader = (RtfReader) null;
    this.m_previousToken = (string) null;
    this.m_previousTokenKey = (string) null;
    this.m_previousTokenValue = (string) null;
    this.m_fontTable.Clear();
    this.m_colorTable.Clear();
    this.m_fontTable = (Dictionary<string, IFont>) null;
    this.m_colorTable = (Dictionary<int, RichTextReader.RtfColor>) null;
    this.m_tabFormatStack.Clear();
    this.m_tabFormatStack = (Stack<Dictionary<int, RichTextReader.TabFormat>>) null;
    this.m_stack.Clear();
    this.m_stack = (Stack<string>) null;
    this.m_unicodeCountStack.Clear();
    this.m_unicodeCountStack = (Stack<int>) null;
    this.m_shapeInstructionStack.Clear();
    this.m_shapeInstructionStack = (Stack<string>) null;
    this.m_rtfCollectionStack.Clear();
    this.m_rtfCollectionStack = (Stack<string>) null;
    this.m_pictureStack.Clear();
    this.m_pictureStack = (Stack<string>) null;
    this.m_listLevelStack.Clear();
    this.m_listLevelStack = (Stack<string>) null;
    this.m_headerFooterStack.Clear();
    this.m_headerFooterStack = (Stack<string>) null;
    this.m_formFieldDataStack.Clear();
    this.m_formFieldDataStack = (Stack<string>) null;
    this.m_fieldResultGroupStack.Clear();
    this.m_fieldResultGroupStack = (Stack<int>) null;
    this.m_fieldGroupStack.Clear();
    this.m_fieldGroupStack = (Stack<int>) null;
    this.m_destStack.Clear();
    this.m_destStack = (Stack<string>) null;
    this.m_groupOrder.Clear();
    this.m_groupOrder = (List<RichTextReader.Groups>) null;
  }

  private void AddFontTableEntry()
  {
    if (this.m_fontName != null && this.m_fontID != null && !this.m_fontTable.ContainsKey(this.m_fontID))
    {
      IFont font = this.m_book.CreateFont();
      font.FontName = this.m_fontName.Trim();
      (font as FontWrapper).CharSet = this.m_fontChar;
      this.m_fontTable.Add(this.m_fontID, font);
      this.m_fontName = (string) null;
      this.m_fontID = (string) null;
      this.m_fontChar = 0;
      this.m_fontNumber = 0;
    }
    using (Dictionary<string, IFont>.Enumerator enumerator = this.m_fontTable.GetEnumerator())
    {
      if (!enumerator.MoveNext())
        return;
      this.m_baseFont = enumerator.Current.Value;
    }
  }

  private void AddColorTableEntry()
  {
    this.m_colorTable.Add(++this.m_currColorIndex, this.m_rtfColorTable);
    this.m_rtfColorTable = new RichTextReader.RtfColor();
  }

  private void ParseControlStart()
  {
    this.m_bIsAccentChar = false;
    this.m_lexer.CurrRtfTokenType = RtfTokenType.ControlWord;
    if (this.m_token.EndsWith("?"))
      this.m_token = this.m_token.TrimEnd('?');
    this.m_token = this.m_token.Trim();
    this.m_token = this.m_token.Substring(1);
    if (this.m_token == "\\" || this.m_token == "{" || this.m_token == "}")
      this.ParseDocumentElement(this.m_token);
    if (this.m_token == "*" && !this.IsDestinationControlWord)
      this.m_destStack.Push("{");
    string[] strArray = this.SeperateToken(this.m_token);
    this.token = new RichTextReader.Tokens();
    this.token.TokenName = strArray[0];
    this.m_group.ChildElements.Add((RichTextReader.Groups) this.token);
    if (strArray[0] != null && (strArray[0].StartsWith("atnid") || strArray[0].StartsWith("atnauthor")))
      strArray = this.SeparateAnnotationToken(strArray);
    if (strArray[0] != null && strArray[0].StartsWith("atnparent"))
      strArray[0] = "atnparent";
    this.ParseControlWords(this.m_token, strArray[0], strArray[1], strArray[2]);
    this.m_previousTokenKey = strArray[0];
    this.m_previousTokenValue = strArray[1];
  }

  private string[] SeparateAnnotationToken(string[] value)
  {
    string str = string.Empty;
    if (value[0].StartsWith("atnid"))
      str = value[0].Substring(0, "atnid".Length);
    if (value[0].StartsWith("atnauthor"))
      str = value[0].Substring(0, "atnauthor".Length);
    int length1 = value[0].Length - 1 - (str.Length - 1);
    int length2 = str.Length;
    value[1] = value[0].Substring(length2, length1);
    value[0] = str;
    return value;
  }

  private bool IsNestedGroup()
  {
    return this.m_currentTableType != RtfTableType.None || this.m_bIsListText || this.m_bIsDocumentInfo || this.m_bIsCustomProperties;
  }

  private void ParseGroupStart()
  {
    this.m_lexer.CurrRtfTokenType = RtfTokenType.GroupStart;
    if (this.IsNestedGroup())
      this.m_stack.Push("{");
    if (this.m_bIsPicture)
      this.m_pictureStack.Push("{");
    if (this.IsDestinationControlWord)
      this.m_destStack.Push("{");
    if (this.m_bIsHeader || this.m_bIsFooter)
      this.m_headerFooterStack.Push("{");
    if (this.IsFieldGroup)
    {
      if (this.m_fieldResultGroupStack.Count > 0)
        this.m_fieldResultGroupStack.Push(this.m_fieldResultGroupStack.Pop() + 1);
      if (this.m_fieldInstructionGroupStack.Count > 0)
        this.m_fieldInstructionGroupStack.Push(this.m_fieldInstructionGroupStack.Pop() + 1);
      if (this.m_fieldGroupStack.Count > 0)
        this.m_fieldGroupStack.Push(this.m_fieldGroupStack.Pop() + 1);
    }
    if (this.m_bIsListLevel)
      this.m_listLevelStack.Push("\\");
    if (this.m_bIsBackgroundCollection)
      this.m_backgroundCollectionStack.Push("\\");
    if (this.m_currentTableType == RtfTableType.None)
    {
      this.m_tabFormatStack.Push(new Dictionary<int, RichTextReader.TabFormat>((IDictionary<int, RichTextReader.TabFormat>) this.m_tabCollection));
      this.m_tabCollection = this.m_tabFormatStack.Peek();
    }
    if (this.m_rtfCollectionStack.Count > 0)
      this.m_rtfCollectionStack.Push("\\");
    if (this.m_bIsShapeInstruction)
      this.m_shapeInstructionStack.Push("{");
    if (this.m_bIsObject)
      this.m_objectStack.Push("{");
    if (!this.IsFormFieldGroup)
      return;
    this.m_formFieldDataStack.Push("{");
  }

  private void ParseGroupEnd()
  {
    this.m_lexer.CurrRtfTokenType = RtfTokenType.GroupEnd;
    if (this.m_unicodeCountStack.Count > 0)
      this.m_unicodeCountStack.Pop();
    this.m_unicodeCount = 0;
    if (this.IsNestedGroup())
    {
      this.m_stack.Pop();
      if (this.m_stack.Count == 0)
      {
        if (this.m_bIsListText)
          this.m_bIsListText = false;
        this.m_bIsDocumentInfo = false;
        this.m_bIsCustomProperties = false;
        this.m_currentTableType = RtfTableType.None;
        this.m_lexer.CurrRtfTableType = RtfTableType.None;
      }
    }
    if (this.IsDestinationControlWord)
      this.m_destStack.Pop();
    if (this.m_currentTableType == RtfTableType.StyleSheet)
    {
      this.m_tabCollection.Clear();
      this.m_tabCount = 0;
      this.m_tabFormatStack.Clear();
    }
    if (this.m_bIsHeader || this.m_bIsFooter)
    {
      this.m_headerFooterStack.Pop();
      if (this.m_headerFooterStack.Count == 0)
      {
        this.m_bIsHeader = false;
        this.m_bIsFooter = false;
        this.m_tabFormatStack.Clear();
      }
    }
    int num = this.IsFieldGroup ? 1 : 0;
    if (this.m_bIsListLevel)
    {
      if (this.m_listLevelStack.Count > 0)
        this.m_listLevelStack.Pop();
      if (this.m_listLevelStack.Count == 0)
        this.m_bIsListLevel = false;
    }
    if (this.m_bIsPicture)
    {
      if (this.m_pictureStack.Count > 0)
        this.m_pictureStack.Pop();
      if (this.m_pictureStack.Count == 0)
      {
        this.m_bIsPicture = false;
        this.m_lexer.IsImageBytes = false;
      }
    }
    if (this.m_bIsBackgroundCollection)
    {
      if (this.m_backgroundCollectionStack.Count > 0)
        this.m_backgroundCollectionStack.Pop();
      if (this.m_backgroundCollectionStack.Count == 0)
        this.m_bIsBackgroundCollection = false;
    }
    if (this.m_currentTableType == RtfTableType.None)
    {
      if (this.m_tabFormatStack.Count > 1)
        this.m_tabFormatStack.Pop();
      if (this.m_tabFormatStack.Count > 0)
        this.m_tabCollection = this.m_tabFormatStack.Peek();
    }
    if (this.m_drawingFieldName != null && this.m_drawingFieldValue != null)
    {
      this.m_bIsShapePicture = (!(this.m_drawingFieldName == "fHorizRule") || !(this.m_drawingFieldValue == "1")) && this.m_bIsShapePicture;
      if ((!this.m_bIsPicture || !this.m_bIsShapePicture) && !string.IsNullOrEmpty(this.m_drawingFieldName) && !string.IsNullOrEmpty(this.m_drawingFieldValue) && this.m_drawingFieldName.ToLower() == "shapetype")
        int.TryParse(this.m_drawingFieldValue, out int _);
      this.m_drawingFieldValue = (string) null;
      this.m_drawingFieldName = (string) null;
    }
    if (this.m_rtfCollectionStack.Count > 0)
      this.m_rtfCollectionStack.Pop();
    if (this.m_bIsShapeInstruction)
    {
      this.m_shapeInstructionStack.Pop();
      if (this.m_shapeInstructionStack.Count == 0)
        this.m_bIsShapeInstruction = false;
    }
    if (this.m_bIsObject)
    {
      this.m_objectStack.Pop();
      if (this.m_objectStack.Count == 0)
        this.m_bIsObject = false;
    }
    if (!this.IsFormFieldGroup)
      return;
    this.m_formFieldDataStack.Pop();
  }

  private string RemoveDelimiterSpace(string token)
  {
    if ((!this.m_previousControlString.StartsWith("u") || this.m_previousControlString.Length <= 1 || !char.IsNumber(this.m_previousControlString[1]) || !(this.m_previousTokenKey == "u")) && (this.m_previousControlString == "}" || this.m_lexer.CurrRtfTokenType == RtfTokenType.Text || this.m_token.StartsWith("u") || this.m_tokenType == RtfTokenType.Unknown && !this.m_bIsListText && token != null && !this.m_bIsBackgroundCollection))
      return token;
    if (this.m_token.Length <= 1)
      return (string) null;
    if (this.m_tokenType != RtfTokenType.GroupStart)
      token = token.Substring(1, token.Length - 1);
    return token;
  }

  private void ParseDocumentElement(string m_token)
  {
    if (m_token.StartsWith(" "))
      m_token = this.RemoveDelimiterSpace(m_token);
    int length = this.m_rtf.Text != null ? this.m_rtf.Text.Length : 0;
    if (!string.IsNullOrEmpty(m_token) && this.m_previousControlString.StartsWith("u") && (this.m_previousControlString.Length > 1 && this.m_previousTokenKey == "u" && char.IsNumber(this.m_previousControlString[1]) || this.m_previousControlString.Length > 2 && this.m_previousTokenKey == "u-" && char.IsNumber(this.m_previousControlString[2])))
    {
      if (this.m_unicodeCountStack.Count > 0)
      {
        if (m_token.Length >= this.m_unicodeCount)
        {
          m_token = m_token.Substring(this.m_unicodeCount);
          this.m_unicodeCount = 0;
        }
        else
        {
          m_token = string.Empty;
          this.m_unicodeCount -= m_token.Length;
        }
      }
      else
        m_token = m_token.Substring(1);
    }
    if (m_token == null || this.m_bIsBackgroundCollection)
      return;
    this.m_tokenType = RtfTokenType.Text;
    this.m_lexer.CurrRtfTokenType = RtfTokenType.Text;
    if (this.m_rtfColorTable != null)
      this.ApplyColorTable(Color.FromArgb(this.m_rtfColorTable.RedN, this.m_rtfColorTable.GreenN, this.m_rtfColorTable.BlueN));
    if (this.m_rtfBold || this.m_rtfItalic || this.m_rtfUnderline != ExcelUnderline.None || this.m_rtfStrikethrough || this.m_rtfSubscript || this.m_rtfSuperscript)
    {
      if (this.m_rtfFont == null && this.m_baseFont != null)
      {
        this.m_rtfFont = this.m_book.CreateFont(this.m_baseFont);
        (this.m_rtfFont as FontImpl).CharSet = (byte) (this.m_baseFont as FontWrapper).CharSet;
      }
      else
        this.m_rtfFont = this.m_rtfFont != null ? this.m_book.CreateFont(this.m_rtfFont) : this.m_book.CreateFont();
      this.m_rtfFont.Bold = this.m_rtfBold;
      this.m_rtfFont.Italic = this.m_rtfItalic;
      this.m_rtfFont.Underline = this.m_rtfUnderline;
      this.m_rtfFont.Strikethrough = this.m_rtfStrikethrough;
      this.m_rtfFont.Subscript = this.m_rtfSubscript;
      this.m_rtfFont.Superscript = this.m_rtfSuperscript;
    }
    if (this.m_bIsBookmarkStart)
      this.m_bIsBookmarkStart = false;
    else if (this.m_bIsBookmarkEnd)
      this.m_bIsBookmarkEnd = false;
    else if (this.IsFieldGroup && !this.m_bIsPicture)
      this.m_currentFieldGroupData += m_token;
    else if (this.m_isCommentRangeStart && !this.m_bIsPicture && !this.m_lexer.IsImageBytes)
    {
      this.AppendRTF(m_token);
      this.m_emptyParaCount = 0;
    }
    else if (this.m_isCommentReference && !this.m_bIsPicture && !this.m_bIsDocumentInfo)
    {
      this.AppendRTF(m_token);
      this.m_emptyParaCount = 0;
    }
    else if (this.m_currentTableType == RtfTableType.None && !this.IsDestinationControlWord && !this.m_bIsDocumentInfo && !this.m_bIsPicture)
    {
      if (this.m_previousToken.StartsWith("'") && this.m_bIsAccentChar)
      {
        m_token = " " + m_token;
        this.m_bIsAccentChar = false;
      }
      this.AppendRTF(m_token);
      this.m_emptyParaCount = 0;
    }
    else if (this.m_currentTableType == RtfTableType.FontTable && !this.IsDestinationControlWord)
      this.m_fontName += m_token;
    else if (this.m_currentTableType == RtfTableType.StyleSheet)
      this.m_styleName += m_token;
    else if (this.m_previousToken == "sn" && this.m_bIsPicture && this.m_bIsShapePicture)
    {
      this.m_drawingFieldName = m_token;
      if (m_token == "pWrapPolygonVertices")
        this.isWrapPolygon = true;
    }
    else if (this.m_previousToken == "sv" && this.m_drawingFieldName != null && this.m_bIsPicture && this.m_bIsShapePicture)
    {
      if (this.isWrapPolygon)
        this.m_drawingFieldValue += m_token;
      else
        this.m_drawingFieldValue = m_token;
    }
    int iEndPos = this.m_rtf.Text != null ? this.m_rtf.Text.Length - 1 : 0;
    if (length > iEndPos || length <= -1)
      return;
    if (this.m_rtfFont == null && this.m_baseFont != null)
    {
      this.m_rtfFont = this.m_book.CreateFont(this.m_baseFont);
      (this.m_rtfFont as FontImpl).CharSet = (byte) (this.m_baseFont as FontWrapper).CharSet;
    }
    else
      this.m_rtfFont = this.m_rtfFont != null ? this.m_book.CreateFont(this.m_rtfFont) : this.m_book.CreateFont();
    this.m_rtf.SetFont(length, iEndPos, this.m_rtfFont);
  }

  private void ParseControlWords(
    string token,
    string tokenKey,
    string tokenValue,
    string tokenValue2)
  {
    if (this.m_currentTableType != RtfTableType.None)
    {
      switch (this.m_currentTableType)
      {
        case RtfTableType.FontTable:
          this.ParseFontTable(this.m_token, tokenKey, tokenValue);
          break;
        case RtfTableType.ColorTable:
          this.ParseColorTable(this.m_token, tokenKey, tokenValue);
          break;
        case RtfTableType.StyleSheet:
          this.ParseFormattingToken(token, tokenKey, tokenValue, tokenValue2);
          break;
      }
    }
    else
    {
      switch (tokenKey)
      {
        case "rtf":
          this.m_rtfCollectionStack.Push("\\");
          break;
        case "fonttbl":
          this.m_baseFont = this.m_book.CreateFont();
          this.m_currentTableType = RtfTableType.FontTable;
          this.m_lexer.CurrRtfTableType = RtfTableType.FontTable;
          this.m_stack.Push("{");
          break;
        case "stylesheet":
          this.m_currentTableType = RtfTableType.StyleSheet;
          this.m_lexer.CurrRtfTableType = RtfTableType.StyleSheet;
          this.m_stack.Push("{");
          break;
        case "listtable":
          this.m_currentTableType = RtfTableType.ListTable;
          this.m_lexer.CurrRtfTableType = RtfTableType.ListTable;
          this.m_stack.Push("{");
          break;
        case "listoverridetable":
          this.m_currentTableType = RtfTableType.ListOverrideTable;
          this.m_lexer.CurrRtfTableType = RtfTableType.ListOverrideTable;
          this.m_stack.Push("{");
          break;
        case "colortbl":
          this.m_currentTableType = RtfTableType.ColorTable;
          this.m_lexer.CurrRtfTableType = RtfTableType.ColorTable;
          this.m_rtfColorTable = new RichTextReader.RtfColor();
          this.m_stack.Push("{");
          break;
        case "info":
          this.m_bIsDocumentInfo = true;
          this.m_stack.Push("{");
          break;
        case "userprops":
          this.m_bIsCustomProperties = true;
          this.m_stack.Push("{");
          break;
        case "ansicpg":
          if (!this.IsSupportedCodePage(Convert.ToInt32(tokenValue)))
            break;
          this.DefaultCodePage = this.GetSupportedCodePage(Convert.ToInt32(tokenValue));
          break;
        default:
          this.ParseFormattingToken(token, tokenKey, tokenValue, tokenValue2);
          break;
      }
    }
  }

  private void SkipGroup()
  {
    Stack<string> stringStack = new Stack<string>();
    stringStack.Push("{");
    while (stringStack.Count > 0)
    {
      this.m_token = this.m_lexer.ReadNextToken(this.m_previousTokenKey);
      if (this.m_token == "{")
        stringStack.Push("{");
      if (this.m_token == "}")
        stringStack.Pop();
    }
    this.m_token = "}";
  }

  private void ParseFontTable(string token, string tokenKey, string tokenValue)
  {
    if (tokenKey == "f" || tokenKey == "af")
    {
      this.m_fontID = token;
      this.m_fontNumber = Convert.ToInt32(tokenValue);
    }
    if (!(tokenKey == "fcharset"))
      return;
    this.m_fontChar = (int) Convert.ToInt16(tokenValue);
  }

  private void ParseColorTable(string token, string tokenKey, string tokenValue)
  {
    switch (tokenKey)
    {
      case "red":
        this.m_rtfColorTable.RedN = Convert.ToInt32(tokenValue);
        break;
      case "green":
        this.m_rtfColorTable.GreenN = Convert.ToInt32(tokenValue);
        break;
      case "blue":
        this.m_rtfColorTable.BlueN = Convert.ToInt32(tokenValue);
        break;
    }
  }

  private void ParseFormattingToken(
    string token,
    string tokenKey,
    string tokenValue,
    string tokenValue2)
  {
    if (tokenKey == "bin" && !this.m_bIsPicture && !this.m_bIsShapePicture && !string.IsNullOrEmpty(tokenValue))
      this.m_rtfReader.Position += int.Parse(tokenValue);
    else if (token.StartsWith("line"))
    {
      if (!(tokenKey == "line"))
        return;
      this.AppendRTF("\r\n");
      ++this.m_emptyParaCount;
    }
    else
    {
      switch (tokenKey)
      {
        case "s":
          if (this.m_currentTableType != RtfTableType.StyleSheet)
            break;
          this.m_currStyleID = this.m_token;
          this.m_styleName = string.Empty;
          break;
        case "cs":
          if (this.m_currentTableType != RtfTableType.StyleSheet)
            break;
          this.m_currStyleID = this.m_token;
          this.m_styleName = string.Empty;
          break;
        case "par":
          this.AppendRTF("\r\n");
          ++this.m_emptyParaCount;
          if (this.m_rtfFont == null && this.m_baseFont != null)
          {
            this.m_rtfFont = this.m_book.CreateFont(this.m_baseFont);
            break;
          }
          if (this.m_rtfFont == null)
          {
            this.m_rtfFont = this.m_book.CreateFont();
            break;
          }
          this.m_rtfFont = this.m_book.CreateFont(this.m_rtfFont);
          break;
        case "header":
        case "headerr":
          this.m_bIsHeader = true;
          this.m_headerFooterStack.Push("{");
          break;
        case "headerl":
          this.m_bIsHeader = true;
          this.m_headerFooterStack.Push("{");
          break;
        case "headerf":
          this.m_bIsHeader = true;
          this.m_headerFooterStack.Push("{");
          break;
        case "footerl":
          this.m_bIsFooter = true;
          this.m_headerFooterStack.Push("{");
          break;
        case "footerf":
          this.m_bIsFooter = true;
          this.m_headerFooterStack.Push("{");
          break;
        case "footer":
        case "footerr":
          this.m_bIsFooter = true;
          this.m_headerFooterStack.Push("{");
          break;
        case "atrfstart":
          if (tokenValue == null)
            break;
          this.m_isCommentRangeStart = true;
          break;
        case "annotation":
          this.m_isCommentReference = true;
          break;
        case "atnref":
          if (tokenValue == null)
            break;
          this.m_isCommentReference = true;
          break;
        case "pard":
          if (this.m_bIsListText)
            break;
          this.m_rtfFont = this.m_rtfFont != null || this.m_baseFont == null ? (this.m_rtfFont != null ? this.m_book.CreateFont(this.m_rtfFont) : this.m_book.CreateFont()) : this.m_book.CreateFont(this.m_baseFont);
          if (!(this.m_rtfFont is FontImpl))
          {
            (this.m_rtfFont as FontWrapper).Font.ParaAlign = Excel2007CommentHAlign.l;
            break;
          }
          (this.m_rtfFont as FontImpl).ParaAlign = Excel2007CommentHAlign.l;
          (this.m_rtfFont as FontImpl).Font.HasParagrapAlign = true;
          break;
        case "qc":
          if (this.m_bIsListText)
            break;
          this.m_rtfFont = this.m_rtfFont != null || this.m_baseFont == null ? (this.m_rtfFont != null ? this.m_book.CreateFont(this.m_rtfFont) : this.m_book.CreateFont()) : this.m_book.CreateFont(this.m_baseFont);
          if (!(this.m_rtfFont is FontImpl))
          {
            (this.m_rtfFont as FontWrapper).Font.ParaAlign = Excel2007CommentHAlign.ctr;
            (this.m_rtfFont as FontWrapper).Font.HasParagrapAlign = true;
            break;
          }
          (this.m_rtfFont as FontImpl).ParaAlign = Excel2007CommentHAlign.ctr;
          (this.m_rtfFont as FontImpl).HasParagrapAlign = true;
          break;
        case "qj":
        case "ql":
          if (this.m_bIsListText)
            break;
          this.m_rtfFont = this.m_rtfFont != null || this.m_baseFont == null ? (this.m_rtfFont != null ? this.m_book.CreateFont(this.m_rtfFont) : this.m_book.CreateFont()) : this.m_book.CreateFont(this.m_baseFont);
          if (!(this.m_rtfFont is FontImpl))
          {
            (this.m_rtfFont as FontWrapper).Font.ParaAlign = Excel2007CommentHAlign.l;
            (this.m_rtfFont as FontWrapper).Font.HasParagrapAlign = true;
            break;
          }
          (this.m_rtfFont as FontImpl).ParaAlign = Excel2007CommentHAlign.l;
          (this.m_rtfFont as FontImpl).HasParagrapAlign = true;
          break;
        case "qr":
          if (this.m_bIsListText)
            break;
          this.m_rtfFont = this.m_rtfFont != null || this.m_baseFont == null ? (this.m_rtfFont != null ? this.m_book.CreateFont(this.m_rtfFont) : this.m_book.CreateFont()) : this.m_book.CreateFont(this.m_baseFont);
          if (!(this.m_rtfFont is FontImpl))
          {
            (this.m_rtfFont as FontWrapper).Font.ParaAlign = Excel2007CommentHAlign.r;
            (this.m_rtfFont as FontWrapper).Font.HasParagrapAlign = true;
            break;
          }
          (this.m_rtfFont as FontImpl).ParaAlign = Excel2007CommentHAlign.r;
          (this.m_rtfFont as FontImpl).HasParagrapAlign = true;
          break;
        case "fs":
          float num = 0.0f;
          if (this.m_rtfFont == null && this.m_baseFont != null)
          {
            this.m_rtfFont = this.m_book.CreateFont(this.m_baseFont);
            (this.m_rtfFont as FontImpl).CharSet = (byte) (this.m_baseFont as FontWrapper).CharSet;
          }
          else
            this.m_rtfFont = this.m_rtfFont != null ? this.m_book.CreateFont(this.m_rtfFont) : this.m_book.CreateFont();
          if (tokenValue != null)
            num = float.Parse(tokenValue, (IFormatProvider) CultureInfo.InvariantCulture) / 2f;
          this.m_rtfFont.Size = (double) num;
          if (tokenValue2 == null)
            break;
          this.m_token = tokenValue2;
          this.ParseDocumentElement(tokenValue2);
          break;
        case "f":
          if (tokenValue2 != null)
            token = tokenKey + tokenValue;
          if (this.m_fontTable.ContainsKey(token))
          {
            if (this.m_rtfFont == null)
            {
              this.m_rtfFont = this.m_book.CreateFont(this.m_fontTable[token]);
              (this.m_rtfFont as FontImpl).CharSet = (byte) (this.m_fontTable[token] as FontWrapper).CharSet;
            }
            else
            {
              this.m_rtfFont.FontName = this.m_fontTable[token].FontName;
              (this.m_rtfFont as FontImpl).CharSet = (byte) (this.m_fontTable[token] as FontWrapper).CharSet;
            }
          }
          if (tokenValue2 == null)
            break;
          this.m_token = tokenValue2;
          this.ParseDocumentElement(tokenValue2);
          break;
        case "cf":
          if (Convert.ToInt32(tokenValue) == 0)
          {
            this.m_rtfColorTable = (RichTextReader.RtfColor) null;
            this.ApplyColorTable(Color.Black);
          }
          else
          {
            int int32 = Convert.ToInt32(tokenValue);
            if (this.m_colorTable.ContainsKey(int32))
              this.m_rtfColorTable = this.m_colorTable[int32];
            this.ApplyColorTable(Color.FromArgb(this.m_rtfColorTable.RedN, this.m_rtfColorTable.GreenN, this.m_rtfColorTable.BlueN));
          }
          if (tokenValue2 == null)
            break;
          this.m_token = tokenValue2;
          this.ParseDocumentElement(tokenValue2);
          break;
        case "u":
          if (this.m_unicodeCountStack.Count > 0)
            this.m_unicodeCount = this.m_unicodeCountStack.Peek();
          this.ParseDocumentElement(((char) Convert.ToInt32(tokenValue)).ToString());
          break;
        case "bullet":
          this.AppendRTF('•'.ToString());
          break;
        case "u*":
          Encoding encoding = Encoding.GetEncoding(this.GetCodePage());
          byte[] bytes = BitConverter.GetBytes(Convert.ToInt16(tokenValue));
          this.ParseDocumentElement(encoding.GetString(bytes, 0, bytes.Length).Replace("\0", ""));
          break;
        case "u-":
          if (this.m_unicodeCountStack.Count > 0)
            this.m_unicodeCount = this.m_unicodeCountStack.Peek();
          this.ParseDocumentElement(((char) (65536 /*0x010000*/ - Convert.ToInt32(tokenValue))).ToString());
          break;
        case "uc":
          this.m_unicodeCountStack.Push(Convert.ToInt32(tokenValue));
          break;
        case "tab":
          if (this.m_currentTableType == RtfTableType.StyleSheet || this.m_bIsListText || this.m_bIsListLevel)
            break;
          ++this.m_tabCount;
          if (this.m_tabCollection.Count == 0 || this.m_tabCount > this.m_tabCollection.Count)
          {
            this.AppendRTF("\t");
            break;
          }
          this.SortTabCollection();
          if (!this.m_tabCollection.ContainsKey(this.m_tabCount))
            break;
          this.AppendRTF("\t");
          break;
        case "b":
          if (this.m_rtfFont == null && this.m_baseFont != null)
          {
            this.m_rtfFont = this.m_book.CreateFont(this.m_baseFont);
            (this.m_rtfFont as FontImpl).CharSet = (byte) (this.m_baseFont as FontWrapper).CharSet;
          }
          else
            this.m_rtfFont = this.m_rtfFont != null ? this.m_book.CreateFont(this.m_rtfFont) : this.m_book.CreateFont();
          this.m_rtfFont.Bold = tokenValue == null || Convert.ToInt32(tokenValue) != 0;
          this.m_rtfBold = this.m_rtfFont.Bold;
          break;
        case "i":
          if (this.m_rtfFont == null && this.m_baseFont != null)
          {
            this.m_rtfFont = this.m_book.CreateFont(this.m_baseFont);
            (this.m_rtfFont as FontImpl).CharSet = (byte) (this.m_baseFont as FontWrapper).CharSet;
          }
          else
            this.m_rtfFont = this.m_rtfFont != null ? this.m_book.CreateFont(this.m_rtfFont) : this.m_book.CreateFont();
          this.m_rtfFont.Italic = tokenValue == null || Convert.ToInt32(tokenValue) != 0;
          this.m_rtfItalic = this.m_rtfFont.Italic;
          break;
        case "ul":
          if (this.m_rtfFont == null && this.m_baseFont != null)
          {
            this.m_rtfFont = this.m_book.CreateFont(this.m_baseFont);
            (this.m_rtfFont as FontImpl).CharSet = (byte) (this.m_baseFont as FontWrapper).CharSet;
          }
          else
            this.m_rtfFont = this.m_rtfFont != null ? this.m_book.CreateFont(this.m_rtfFont) : this.m_book.CreateFont();
          this.m_rtfFont.Underline = tokenValue == null || Convert.ToInt32(tokenValue) != 0 ? ExcelUnderline.Single : ExcelUnderline.None;
          this.m_rtfUnderline = this.m_rtfFont.Underline;
          break;
        case "ulnone":
          if (this.m_rtfFont == null && this.m_baseFont != null)
          {
            this.m_rtfFont = this.m_book.CreateFont(this.m_baseFont);
            (this.m_rtfFont as FontImpl).CharSet = (byte) (this.m_baseFont as FontWrapper).CharSet;
          }
          else
            this.m_rtfFont = this.m_rtfFont != null ? this.m_book.CreateFont(this.m_rtfFont) : this.m_book.CreateFont();
          this.m_rtfFont.Underline = ExcelUnderline.None;
          this.m_rtfUnderline = this.m_rtfFont.Underline;
          break;
        case "uldb":
          if (this.m_rtfFont == null && this.m_baseFont != null)
          {
            this.m_rtfFont = this.m_book.CreateFont(this.m_baseFont);
            (this.m_rtfFont as FontImpl).CharSet = (byte) (this.m_baseFont as FontWrapper).CharSet;
          }
          else
            this.m_rtfFont = this.m_rtfFont != null ? this.m_book.CreateFont(this.m_rtfFont) : this.m_book.CreateFont();
          this.m_rtfFont.Underline = tokenValue == null || Convert.ToInt32(tokenValue) != 0 ? ExcelUnderline.Double : ExcelUnderline.None;
          this.m_rtfUnderline = this.m_rtfFont.Underline;
          break;
        case "strike":
          if (this.m_rtfFont == null && this.m_baseFont != null)
          {
            this.m_rtfFont = this.m_book.CreateFont(this.m_baseFont);
            (this.m_rtfFont as FontImpl).CharSet = (byte) (this.m_baseFont as FontWrapper).CharSet;
          }
          else
            this.m_rtfFont = this.m_rtfFont != null ? this.m_book.CreateFont(this.m_rtfFont) : this.m_book.CreateFont();
          this.m_rtfFont.Strikethrough = tokenValue == null || Convert.ToInt32(tokenValue) != 0;
          this.m_rtfStrikethrough = this.m_rtfFont.Strikethrough;
          break;
        case "sub":
          if (this.m_rtfFont == null && this.m_baseFont != null)
          {
            this.m_rtfFont = this.m_book.CreateFont(this.m_baseFont);
            (this.m_rtfFont as FontImpl).CharSet = (byte) (this.m_baseFont as FontWrapper).CharSet;
          }
          else
            this.m_rtfFont = this.m_rtfFont != null ? this.m_book.CreateFont(this.m_rtfFont) : this.m_book.CreateFont();
          this.m_rtfFont.Subscript = tokenValue == null || Convert.ToInt32(tokenValue) != 0;
          this.m_rtfSubscript = this.m_rtfFont.Subscript;
          break;
        case "super":
          if (this.m_rtfFont == null && this.m_baseFont != null)
          {
            this.m_rtfFont = this.m_book.CreateFont(this.m_baseFont);
            (this.m_rtfFont as FontImpl).CharSet = (byte) (this.m_baseFont as FontWrapper).CharSet;
          }
          else
            this.m_rtfFont = this.m_rtfFont != null ? this.m_book.CreateFont(this.m_rtfFont) : this.m_book.CreateFont();
          this.m_rtfFont.Superscript = tokenValue == null || Convert.ToInt32(tokenValue) != 0;
          this.m_rtfSuperscript = this.m_rtfFont.Superscript;
          break;
        case "nosupersub":
          if (this.m_rtfFont == null && this.m_baseFont != null)
          {
            this.m_rtfFont = this.m_book.CreateFont(this.m_baseFont);
            (this.m_rtfFont as FontImpl).CharSet = (byte) (this.m_baseFont as FontWrapper).CharSet;
          }
          else
            this.m_rtfFont = this.m_rtfFont != null ? this.m_book.CreateFont(this.m_rtfFont) : this.m_book.CreateFont();
          this.m_rtfFont.Superscript = false;
          this.m_rtfFont.Subscript = false;
          this.m_rtfSuperscript = this.m_rtfFont.Superscript;
          this.m_rtfSubscript = this.m_rtfFont.Subscript;
          break;
        case "lquote":
          char ch = '‘';
          if (this.m_prevTokenType == RtfTokenType.Text)
            break;
          this.AppendRTF(ch.ToString());
          break;
        case "rquote":
          this.AppendRTF('’'.ToString());
          break;
        case "rdblquote":
        case "ldblquote":
          this.AppendRTF("\"");
          break;
        case "endash":
          this.AppendRTF('–'.ToString());
          break;
        case "emdash":
          this.AppendRTF('—'.ToString());
          break;
        case "chcfpat":
          int int32_1 = Convert.ToInt32(tokenValue);
          if (this.m_colorTable.ContainsKey(int32_1))
            this.m_rtfColorTable = this.m_colorTable[int32_1];
          this.ApplyColorTable(Color.FromArgb(this.m_rtfColorTable.RedN, this.m_rtfColorTable.GreenN, this.m_rtfColorTable.BlueN));
          if (tokenValue2 == null)
            break;
          this.m_token = tokenValue2;
          this.ParseDocumentElement(tokenValue2);
          break;
        case "object":
          this.m_bIsObject = true;
          this.m_objectStack.Push("{");
          break;
        case "bkmkstart":
          this.m_bIsBookmarkStart = true;
          break;
        case "bkmkend":
          this.m_bIsBookmarkEnd = true;
          break;
        case "pntext":
        case "listtext":
          this.m_stack.Push("{");
          this.m_bIsListText = true;
          break;
        case "pnf":
          if (!this.m_fontTable.ContainsKey(token))
            break;
          this.m_rtfFont = this.m_book.CreateFont(this.m_fontTable[token]);
          (this.m_rtfFont as FontImpl).CharSet = (byte) (this.m_fontTable[token] as FontWrapper).CharSet;
          break;
        case "clcfpat":
        case "clcfpatraw":
          int int32_2 = Convert.ToInt32(tokenValue);
          if (this.m_colorTable.ContainsKey(int32_2))
            this.m_rtfColorTable = this.m_colorTable[int32_2];
          this.ApplyColorTable(Color.FromArgb(this.m_rtfColorTable.RedN, this.m_rtfColorTable.GreenN, this.m_rtfColorTable.BlueN));
          break;
        case "field":
          if (!this.IsFieldGroup)
          {
            this.m_currentFieldGroupData = string.Empty;
            this.m_fieldInstructionGroupStack = new Stack<int>();
            this.m_fieldResultGroupStack = new Stack<int>();
            this.m_fieldGroupStack = new Stack<int>();
          }
          if (this.m_fieldGroupStack.Count > 0)
            this.m_fieldGroupStack.Push(this.m_fieldGroupStack.Pop() - 1);
          this.m_fieldGroupStack.Push(1);
          break;
        default:
          this.ParseSpecialCharacters(token);
          break;
      }
    }
  }

  private void ParseSpecialCharacters(string token)
  {
    string m_token = (string) null;
    if (!this.IsDestinationControlWord || this.IsFieldGroup)
    {
      if (token.StartsWith("'") && !this.IsAccentCharacterNeedToBeOmitted())
      {
        this.m_bIsAccentChar = true;
        m_token = this.GetAccentCharacter(token);
      }
      else if (token.StartsWith("_"))
        m_token = token.Replace("_", '\u001E'.ToString());
      else if (token.StartsWith("~"))
        m_token = token.Replace("~", ' '.ToString());
      else if (token.StartsWith("-"))
        m_token = token.Replace("-", '\u001F'.ToString());
      else if (token.StartsWith(":"))
        m_token = token;
      else if (token.StartsWith("zw") && this.m_tokenType == RtfTokenType.ControlWord)
      {
        switch (token)
        {
          case "zwbo":
          case "zwnj":
            m_token = '\u200C'.ToString();
            break;
          case "zwnbo":
          case "zwj":
            m_token = '\u200D'.ToString();
            break;
        }
      }
    }
    if (m_token != null)
      this.ParseDocumentElement(m_token);
    if (!token.StartsWith("zw") || this.m_tokenType != RtfTokenType.Text)
      return;
    this.m_tokenType = RtfTokenType.ControlWord;
    this.m_lexer.CurrRtfTokenType = RtfTokenType.ControlWord;
  }

  private bool IsAccentCharacterNeedToBeOmitted()
  {
    return this.m_unicodeCount > 0 && --this.m_unicodeCount >= 0;
  }

  private string GetAccentCharacter(string token)
  {
    string accentCharacter = "";
    int length = token.Length;
    string s = token.Substring(1, 2);
    if (s != "3f" && s != ".." && s != ".")
    {
      int num = int.Parse(s, NumberStyles.HexNumber);
      Encoding encoding = Encoding.GetEncoding(this.GetCodePage());
      if (!this.IsSingleByte())
      {
        string token1 = this.m_lexer.ReadNextToken(this.m_previousTokenKey);
        while (token1 == "\n" || token1 == "\r")
          token1 = this.m_lexer.ReadNextToken(this.m_previousTokenKey);
        string[] strArray = this.SeperateToken(token1);
        this.m_previousTokenKey = strArray[0];
        this.m_previousTokenValue = strArray[1];
        num = int.Parse(token1.Trim().Substring(1).Substring(1, 2) + s, NumberStyles.HexNumber);
      }
      byte[] bytes = BitConverter.GetBytes((short) num);
      accentCharacter = encoding.GetString(bytes, 0, bytes.Length).Replace("\0", "");
    }
    else if (this.m_previousTokenKey == "u" || this.m_previousTokenKey == "u-")
      accentCharacter = ((char) Convert.ToInt32(this.m_previousTokenValue)).ToString();
    if (length > 3)
      accentCharacter += token.Substring(3, length - 3);
    if (token == "'.." || token == "'.")
      accentCharacter = "_";
    return accentCharacter;
  }

  private string GetCodePage()
  {
    switch (this.GetFontCharSet())
    {
      case 0:
      case 1:
        return "Windows-1252";
      case 77:
        return "macintosh";
      case 78:
        return "x-mac-japanese";
      case 79:
        return "x-mac-korean";
      case 80 /*0x50*/:
        return "x-mac-chinesesimp";
      case 81:
      case 82:
        return "x-mac-chinesetrad";
      case 83:
        return "x-mac-hebrew";
      case 84:
        return "x-mac-arabic";
      case 85:
        return "x-mac-greek";
      case 86:
        return "x-mac-turkish";
      case 87:
        return "x-mac-thai";
      case 88:
        return "x-mac-ce";
      case 89:
        return "x-mac-cyrillic";
      case 128 /*0x80*/:
        return "shift_jis";
      case 129:
        return "ks_c_5601-1987";
      case 130:
        return "Johab";
      case 134:
        return "gb2312";
      case 136:
        return "big5";
      case 161:
        return "windows-1253";
      case 162:
        return "windows-1254";
      case 163:
        return "windows-1258";
      case 177:
        return "windows-1255";
      case 178:
      case 179:
      case 180:
      case 181:
        return "windows-1256";
      case 186:
        return "windows-1257";
      case 204:
        return "windows-1251";
      case 222:
        return "windows-874";
      case 238:
        return "windows-1250";
      case 254:
        return "IBM437";
      case (int) byte.MaxValue:
        return "ibm850";
      default:
        return this.DefaultCodePage;
    }
  }

  private bool IsSingleByte()
  {
    switch (this.GetFontCharSet())
    {
      case 78:
      case 79:
      case 80 /*0x50*/:
      case 81:
      case 82:
      case 128 /*0x80*/:
      case 129:
      case 130:
      case 134:
      case 136:
        return false;
      default:
        return true;
    }
  }

  private int GetFontCharSet()
  {
    if (this.m_rtfFont == null && this.m_baseFont != null)
    {
      this.m_rtfFont = this.m_book.CreateFont(this.m_baseFont);
      (this.m_rtfFont as FontImpl).CharSet = (byte) (this.m_baseFont as FontWrapper).CharSet;
    }
    else if (this.m_rtfFont == null)
      this.m_rtfFont = this.m_book.CreateFont();
    return (int) (this.m_rtfFont as FontImpl).CharSet;
  }

  private bool IsSupportedCodePage(int codePage)
  {
    switch (codePage)
    {
      case 37:
      case 437:
      case 500:
      case 708:
      case 720:
      case 737:
      case 775:
      case 850:
      case 852:
      case 855:
      case 857:
      case 858:
      case 860:
      case 861:
      case 862:
      case 863:
      case 864:
      case 865:
      case 866:
      case 869:
      case 870:
      case 874:
      case 875:
      case 932:
      case 936:
      case 949:
      case 950:
      case 1026:
      case 1047:
      case 1140:
      case 1141:
      case 1142:
      case 1143:
      case 1144:
      case 1145:
      case 1146:
      case 1147:
      case 1148:
      case 1149:
      case 1200:
      case 1201:
      case 1250:
      case 1251:
      case 1252:
      case 1253:
      case 1254:
      case 1255:
      case 1256:
      case 1257:
      case 1258:
      case 1361:
      case 10000:
      case 10001:
      case 10002:
      case 10003:
      case 10004:
      case 10005:
      case 10006:
      case 10007:
      case 10008:
      case 10010:
      case 10017:
      case 10021:
      case 10029:
      case 10079:
      case 10081:
      case 10082:
      case 12000:
      case 12001:
      case 20000:
      case 20001:
      case 20002:
      case 20003:
      case 20004:
      case 20005:
      case 20105:
      case 20106:
      case 20107:
      case 20108:
      case 20127:
      case 20261:
      case 20269:
      case 20273:
      case 20277:
      case 20278:
      case 20280:
      case 20284:
      case 20285:
      case 20290:
      case 20297:
      case 20420:
      case 20423:
      case 20424:
      case 20833:
      case 20838:
      case 20866:
      case 20871:
      case 20880:
      case 20905:
      case 20924:
      case 20932:
      case 20936:
      case 20949:
      case 21025:
      case 21866:
      case 28591:
      case 28592:
      case 28593:
      case 28594:
      case 28595:
      case 28596:
      case 28597:
      case 28598:
      case 28599:
      case 28603:
      case 28605:
      case 29001:
      case 38598:
      case 50220:
      case 50221:
      case 50222:
      case 50225:
      case 50227:
      case 51932:
      case 51936:
      case 51949:
      case 52936:
      case 54936:
      case 57002:
      case 57003:
      case 57004:
      case 57005:
      case 57006:
      case 57007:
      case 57008:
      case 57009:
      case 57010:
      case 57011:
      case 65000:
      case 65001:
        return true;
      default:
        return false;
    }
  }

  private string GetSupportedCodePage(int codePage)
  {
    switch (codePage)
    {
      case 37:
        return "IBM037";
      case 437:
        return "IBM437";
      case 500:
        return "IBM500";
      case 708:
        return "ASMO-708";
      case 720:
        return "DOS-720";
      case 737:
        return "ibm737";
      case 775:
        return "ibm775";
      case 850:
        return "ibm850";
      case 852:
        return "ibm852";
      case 855:
        return "IBM855";
      case 857:
        return "ibm857";
      case 858:
        return "IBM00858";
      case 860:
        return "IBM860";
      case 861:
        return "ibm861";
      case 862:
        return "DOS-862";
      case 863:
        return "IBM863";
      case 864:
        return "IBM864";
      case 865:
        return "IBM865";
      case 866:
        return "cp866";
      case 869:
        return "ibm869";
      case 870:
        return "IBM870";
      case 874:
        return "windows-874";
      case 875:
        return "cp875";
      case 932:
        return "shift_jis";
      case 936:
        return "gb2312";
      case 949:
        return "ks_c_5601-1987";
      case 950:
        return "big5";
      case 1026:
        return "IBM1026";
      case 1047:
        return "IBM01047";
      case 1140:
        return "IBM01140";
      case 1141:
        return "IBM01141";
      case 1142:
        return "IBM01142";
      case 1143:
        return "IBM01143";
      case 1144:
        return "IBM01144";
      case 1145:
        return "IBM01145";
      case 1146:
        return "IBM01146";
      case 1147:
        return "IBM01147";
      case 1148:
        return "IBM01148";
      case 1149:
        return "IBM01149";
      case 1200:
        return "utf-16";
      case 1201:
        return "unicodeFFFE";
      case 1250:
        return "windows-1250";
      case 1251:
        return "windows-1251";
      case 1252:
        return "windows-1252";
      case 1253:
        return "windows-1253";
      case 1254:
        return "windows-1254";
      case 1255:
        return "windows-1255";
      case 1256:
        return "windows-1256";
      case 1257:
        return "windows-1257";
      case 1258:
        return "windows-1258";
      case 1361:
        return "Johab";
      case 10000:
        return "macintosh";
      case 10001:
        return "x-mac-japanese";
      case 10002:
        return "x-mac-chinesetrad";
      case 10003:
        return "x-mac-korean";
      case 10004:
        return "x-mac-arabic";
      case 10005:
        return "x-mac-hebrew";
      case 10006:
        return "x-mac-greek";
      case 10007:
        return "x-mac-cyrillic";
      case 10008:
        return "x-mac-chinesesimp";
      case 10010:
        return "x-mac-romanian";
      case 10017:
        return "x-mac-ukrainian";
      case 10021:
        return "x-mac-thai";
      case 10029:
        return "x-mac-ce";
      case 10079:
        return "x-mac-icelandic";
      case 10081:
        return "x-mac-turkish";
      case 10082:
        return "x-mac-croatian";
      case 12000:
        return "utf-32";
      case 12001:
        return "utf-32BE";
      case 20000:
        return "x-Chinese_CNS";
      case 20001:
        return "x-cp20001";
      case 20002:
        return "x_Chinese-Eten";
      case 20003:
        return "x-cp20003";
      case 20004:
        return "x-cp20004";
      case 20005:
        return "x-cp20005";
      case 20105:
        return "x-IA5";
      case 20106:
        return "x-IA5-German";
      case 20107:
        return "x-IA5-Swedish";
      case 20108:
        return "x-IA5-Norwegian";
      case 20127:
        return "us-ascii";
      case 20261:
        return "x-cp20261";
      case 20269:
        return "x-cp20269";
      case 20273:
        return "IBM273";
      case 20277:
        return "IBM277";
      case 20278:
        return "IBM278";
      case 20280:
        return "IBM280";
      case 20284:
        return "IBM284";
      case 20285:
        return "IBM285";
      case 20290:
        return "IBM290";
      case 20297:
        return "IBM297";
      case 20420:
        return "IBM420";
      case 20423:
        return "IBM423";
      case 20424:
        return "IBM424";
      case 20833:
        return "x-EBCDIC-KoreanExtended";
      case 20838:
        return "IBM-Thai";
      case 20866:
        return "koi8-r";
      case 20871:
        return "IBM871";
      case 20880:
        return "IBM880";
      case 20905:
        return "IBM905";
      case 20924:
        return "IBM00924";
      case 20932:
        return "EUC-JP";
      case 20936:
        return "x-cp20936";
      case 20949:
        return "x-cp20949";
      case 21025:
        return "cp1025";
      case 21866:
        return "koi8-u";
      case 28591:
        return "iso-8859-1";
      case 28592:
        return "iso-8859-2";
      case 28593:
        return "iso-8859-3";
      case 28594:
        return "iso-8859-4";
      case 28595:
        return "iso-8859-5";
      case 28596:
        return "iso-8859-6";
      case 28597:
        return "iso-8859-7";
      case 28598:
        return "iso-8859-8";
      case 28599:
        return "iso-8859-9";
      case 28603:
        return "iso-8859-13";
      case 28605:
        return "iso-8859-15";
      case 29001:
        return "x-Europa";
      case 38598:
        return "iso-8859-8-i";
      case 50220:
        return "iso-2022-jp";
      case 50221:
        return "csISO2022JP";
      case 50222:
        return "iso-2022-jp";
      case 50225:
        return "iso-2022-kr";
      case 50227:
        return "x-cp50227";
      case 51932:
        return "euc-jp";
      case 51936:
        return "EUC-CN";
      case 51949:
        return "euc-kr";
      case 52936:
        return "hz-gb-2312";
      case 54936:
        return "GB18030";
      case 57002:
        return "x-iscii-de";
      case 57003:
        return "x-iscii-be";
      case 57004:
        return "x-iscii-ta";
      case 57005:
        return "x-iscii-te";
      case 57006:
        return "x-iscii-as";
      case 57007:
        return "x-iscii-or";
      case 57008:
        return "x-iscii-ka";
      case 57009:
        return "x-iscii-ma";
      case 57010:
        return "x-iscii-gu";
      case 57011:
        return "x-iscii-pa";
      case 65000:
        return "utf-7";
      case 65001:
        return "utf-8";
      default:
        return "Windows-1252";
    }
  }

  private void SortTabCollection()
  {
    for (int key1 = 1; key1 < this.m_tabCollection.Count; ++key1)
    {
      for (int key2 = key1 + 1; key2 < this.m_tabCollection.Count + 1; ++key2)
      {
        if ((double) this.m_tabCollection[key1].TabPosition > (double) this.m_tabCollection[key2].TabPosition)
        {
          RichTextReader.TabFormat tab = this.m_tabCollection[key1];
          this.m_tabCollection[key1] = this.m_tabCollection[key2];
          this.m_tabCollection[key2] = tab;
        }
      }
    }
  }

  private string[] SeperateToken(string token)
  {
    string[] strArray1 = new string[3];
    for (int index = 0; index < token.Length; ++index)
    {
      char c = token[index];
      if (char.IsDigit(c) && strArray1[2] == null)
      {
        string[] strArray2;
        (strArray2 = strArray1)[1] = strArray2[1] + (object) c;
      }
      else if (strArray1[1] == null)
      {
        string[] strArray3;
        (strArray3 = strArray1)[0] = strArray3[0] + (object) c;
      }
      else if (strArray1[1] != null)
      {
        string[] strArray4;
        (strArray4 = strArray1)[2] = strArray4[2] + (object) c;
      }
    }
    return strArray1;
  }

  private void ApplyColorTable(Color rtfColor)
  {
    if (this.m_rtfFont == null && this.m_baseFont != null)
    {
      this.m_rtfFont = this.m_book.CreateFont(this.m_baseFont);
      (this.m_rtfFont as FontImpl).CharSet = (byte) (this.m_baseFont as FontWrapper).CharSet;
    }
    else
      this.m_rtfFont = this.m_rtfFont != null ? this.m_book.CreateFont(this.m_rtfFont) : this.m_book.CreateFont();
    this.m_rtfFont.RGBColor = rtfColor;
  }

  private void SetParsedElementFlag(string token)
  {
    token = Regex.Replace(token, "[\\d+$]", string.Empty);
  }

  private void Parse()
  {
    this.ParseToken();
    this.m_rtf.TextObject.RtfText = this.m_rtfText;
  }

  private void AppendRTF(string rtfString)
  {
    rtfString = rtfString.Replace("'..", "_");
    this.m_rtf.SetText(this.m_rtf.Text + rtfString);
  }

  public void SetRTF(int row, int column, string text)
  {
    this.m_rtfText = text != null ? text : throw new ArgumentNullException("RTF Text");
    this.m_range = this.m_sheet[row, column];
    this.m_rtf = this.m_range.RichText as RichTextString;
    this.m_rtf.Text = string.Empty;
    this.Parse();
    this.UpdateText(true);
  }

  public void SetRTF(object shape, string text)
  {
    this.m_rtfText = text != null ? text : throw new ArgumentNullException("RTF Text");
    this.m_rtf = this.CreateRichTextString() as RichTextString;
    this.Parse();
    this.UpdateText(false);
    switch (shape.GetType().Name)
    {
      case "TextBoxShapeImpl":
        (shape as ITextBoxShape).RichText = (IRichTextString) this.m_rtf;
        break;
    }
  }

  private void UpdateText(bool isRange)
  {
    if (this.m_emptyParaCount > 0 || this.m_rtf.Text[0] == '\'')
    {
      Dictionary<int, int> dictionary = new Dictionary<int, int>((IDictionary<int, int>) this.m_rtf.TextObject.FormattingRuns);
      List<int> intList1 = new List<int>();
      List<int> intList2 = new List<int>();
      foreach (KeyValuePair<int, int> formattingRun in this.m_rtf.TextObject.FormattingRuns)
      {
        intList2.Add(formattingRun.Key);
        intList1.Add(formattingRun.Value);
      }
      if (this.m_emptyParaCount > 0)
      {
        bool flag = false;
        this.m_rtf.Text = this.m_rtf.Text.Remove(this.m_rtf.Text.Length - 1 - this.m_emptyParaCount);
        if (this.m_rtf.Text[0] == '\'' && isRange)
        {
          this.m_rtf.Text = (this.m_range as RangeImpl).CheckApostrophe(this.m_rtf.Text);
          flag = true;
        }
        if (dictionary.Count > 1)
        {
          for (int index = 0; index < dictionary.Count; ++index)
          {
            if (index + 1 < dictionary.Count)
              this.m_rtf.SetFont(!flag || index == 0 ? intList2[index] : intList2[index] - 1, intList2[index + 1] - 1, this.m_book.InnerFonts[intList1[index]]);
            else
              this.m_rtf.SetFont(!flag || index == 0 ? intList2[index] : intList2[index] - 1, this.m_rtf.Text.Length - 1, this.m_book.InnerFonts[intList1[index]]);
          }
        }
      }
    }
    if (!isRange || !this.m_rtf.Text.Contains(Environment.NewLine) && !this.m_rtf.Text.Contains("\n") && !this.m_rtf.Text.Contains("_x000a_"))
      return;
    this.m_range.WrapText = true;
  }

  protected IRichTextString CreateRichTextString()
  {
    return (IRichTextString) new RangeRichTextString(this.m_sheet.Application, (object) this.m_sheet, -1, -1);
  }

  internal class Groups
  {
    private List<RichTextReader.Groups> m_childElements;

    internal List<RichTextReader.Groups> ChildElements
    {
      get
      {
        if (this.m_childElements == null)
          this.m_childElements = new List<RichTextReader.Groups>();
        return this.m_childElements;
      }
      set => this.m_childElements = value;
    }
  }

  internal class Tokens : RichTextReader.Groups
  {
    private string m_tokenName;

    internal string TokenName
    {
      get => this.m_tokenName;
      set => this.m_tokenName = value;
    }
  }

  internal class TabFormat
  {
    private float m_tabPosition = 36f;

    internal float TabPosition
    {
      get => this.m_tabPosition;
      set => this.m_tabPosition = value;
    }
  }

  internal class RtfColor
  {
    private int m_redN;
    private int m_greenN;
    private int m_blueN;

    internal int RedN
    {
      get => this.m_redN;
      set => this.m_redN = value;
    }

    internal int GreenN
    {
      get => this.m_greenN;
      set => this.m_greenN = value;
    }

    internal int BlueN
    {
      get => this.m_blueN;
      set => this.m_blueN = value;
    }
  }
}
