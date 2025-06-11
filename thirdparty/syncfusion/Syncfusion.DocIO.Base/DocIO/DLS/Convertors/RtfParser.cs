// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Convertors.RtfParser
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.DLS.Convertors;

internal class RtfParser
{
  private const string c_groupStart = "{";
  private const string c_groupEnd = "}";
  private const string c_controlStart = "\\";
  private const string c_space = " ";
  private const string c_carriegeReturn = "\r";
  private const string c_newLine = "\n";
  private const string c_semiColon = ";";
  private RtfLexer m_lexer;
  private RtfReader m_rtfReader;
  private string m_token;
  private string m_previousToken;
  private string m_previousTokenKey;
  private string m_previousTokenValue;
  private string m_previousControlString;
  private bool m_bIsContinousList;
  private bool m_bIsPreviousList;
  private RtfTableType m_currentTableType = RtfTableType.None;
  private Dictionary<string, RtfFont> m_fontTable = new Dictionary<string, RtfFont>();
  private Dictionary<int, RtfColor> m_colorTable = new Dictionary<int, RtfColor>();
  private Dictionary<int, CellFormat> m_cellFormatTable = new Dictionary<int, CellFormat>();
  private IWParagraph m_currParagraph;
  private IWSection m_currSection;
  private WordDocument m_document;
  private RtfParser.TextFormat m_currTextFormat;
  private RtfParser.ShapeFormat m_currShapeFormat;
  private RtfParser.PictureFormat m_picFormat;
  private Stack<string> m_stack = new Stack<string>();
  private Stack<string> m_pictureOrShapeStack = new Stack<string>();
  private Stack<string> m_groupShapeStack = new Stack<string>();
  private Stack<string> m_destStack = new Stack<string>();
  private Stack<string> m_headerFooterStack = new Stack<string>();
  private Stack<WCommentMark> commentstack = new Stack<WCommentMark>();
  private Stack<Dictionary<int, CellFormat>> m_CellFormatStack = new Stack<Dictionary<int, CellFormat>>();
  private Stack<Dictionary<int, CellFormat>> m_prevCellFormatStack = new Stack<Dictionary<int, CellFormat>>();
  private RtfFont m_rtfFont;
  private RtfTokenType m_tokenType;
  private RtfTokenType m_prevTokenType;
  private RtfColor m_rtfColorTable;
  private bool m_bIsBorderTop;
  private bool m_bIsBorderBottom;
  private bool m_bIsBorderLeft;
  private bool m_bIsBorderRight;
  private bool m_bIsBorderDiagonalDown;
  private bool m_bIsBorderDiagonalUp;
  private bool m_bIsPictureOrShape;
  private bool m_bIsShape;
  private bool m_bIsHorizontalBorder;
  private bool m_bIsVerticalBorder;
  private bool m_bIsFallBackImage;
  private bool m_bIsShapeResult;
  private int m_bShapeResultStackCount;
  private IWPicture m_currPicture;
  private Shape m_currShape;
  private WTextBox m_currTextBox;
  private IWTextRange tr;
  private bool m_bIsDocumentInfo;
  private bool m_bIsShapePicture;
  private RtfParser.SecionFormat m_secFormat;
  private ListStyle m_currListStyle;
  private WListLevel m_currListLevel;
  private int m_currLevelIndex = -1;
  private Dictionary<string, ListStyle> m_listTable = new Dictionary<string, ListStyle>();
  private Dictionary<string, string> m_listOverrideTable = new Dictionary<string, string>();
  private Dictionary<string, IWParagraphStyle> m_styleTable = new Dictionary<string, IWParagraphStyle>();
  private Dictionary<string, WCharacterStyle> m_charStyleTable = new Dictionary<string, WCharacterStyle>();
  private string m_currStyleName;
  private bool m_bIsListText;
  private bool m_bIsList;
  private bool isPnStartUpdate;
  private IWTable m_currTable;
  private WTableRow m_currRow;
  private WTableCell m_currCell;
  private int m_currCellFormatIndex = -1;
  private bool m_bIsRow;
  private CellFormat m_currCellFormat;
  private RowFormat m_currRowFormat;
  private bool m_bIsGroupShape;
  private bool m_bIsBookmarkStart;
  private bool m_bIsBookmarkEnd;
  private bool m_bIsHeader;
  private bool m_bIsFooter;
  private bool m_bIsLevelText;
  private WTextBody m_textBody;
  private int m_previousLevel;
  private int m_currentLevel;
  private bool m_bIsCustomProperties;
  private string m_currPropertyName;
  private object m_currPropertyValue;
  private Syncfusion.CompoundFile.DocIO.PropertyType m_currPropertyType;
  private bool m_bInTable;
  private Stack<WTextBody> m_nestedTextBody = new Stack<WTextBody>();
  private Stack<WTable> m_nestedTable = new Stack<WTable>();
  private bool m_bCellFinished;
  private Column m_currColumn;
  private IWParagraphStyle m_currStyle;
  private WCharacterStyle m_currCharStyle;
  private string m_currStyleID;
  private int m_secCount;
  private Dictionary<int, TabFormat> m_tabCollection = new Dictionary<int, TabFormat>();
  private TabFormat m_currTabFormat;
  private int m_tabCount;
  private bool m_bIsLinespacingRule;
  private bool m_bIsAccentChar;
  private int m_currCellBoundary;
  private int m_currRowLeftIndent;
  private Stack<RowFormat> m_currRowFormatStack = new Stack<RowFormat>();
  private Stack<RowFormat> m_prevRowFormatStack = new Stack<RowFormat>();
  private Stack<string> m_backgroundCollectionStack = new Stack<string>();
  private bool m_bIsBackgroundCollection;
  private bool m_bIsDefaultSectionFormat;
  private RtfParser.SecionFormat m_defaultSectionFormat;
  private string m_styleName;
  private Stack<string> m_listLevelStack = new Stack<string>();
  private bool m_bIsListLevel;
  private IWParagraph m_prevParagraph;
  private RtfParser.TextFormat m_prevTextFormat;
  private WParagraphFormat m_listLevelParaFormat;
  private WCharacterFormat m_listLevelCharFormat;
  private int m_pnLevelNumber = -1;
  private FormFieldData m_currentFormField;
  private Stack<int> m_unicodeCountStack = new Stack<int>();
  private int m_unicodeCount;
  private int m_currColorIndex = -1;
  private Stack<Dictionary<int, TabFormat>> m_tabFormatStack = new Stack<Dictionary<int, TabFormat>>();
  private Stack<RtfParser.TextFormat> m_textFormatStack = new Stack<RtfParser.TextFormat>();
  private Stack<WParagraphFormat> m_paragraphFormatStack = new Stack<WParagraphFormat>();
  private WParagraphFormat m_currParagraphFormat;
  private Stack<string> m_rtfCollectionStack = new Stack<string>();
  private Stack<string> m_shapeInstructionStack = new Stack<string>();
  private Stack<string> m_shapeTextStack = new Stack<string>();
  private Stack<WTextBody> m_shapeTextBody = new Stack<WTextBody>();
  private Stack<Dictionary<string, object>> m_ownerShapeTextbodyStack = new Stack<Dictionary<string, object>>();
  private Stack<WParagraph> m_shapeParagraph = new Stack<WParagraph>();
  private Stack<Dictionary<string, bool>> m_shapeFlagStack = new Stack<Dictionary<string, bool>>();
  private Stack<Dictionary<string, object>> m_shapeTextbodyStack = new Stack<Dictionary<string, object>>();
  private List<RtfParser.TempShapeProperty> m_drawingFields = new List<RtfParser.TempShapeProperty>();
  private bool m_bIsShapeInstruction;
  private bool m_bIsShapeText;
  private bool m_bIsShapePictureAdded;
  private Stack<string> m_objectStack = new Stack<string>();
  private bool m_bIsObject;
  private bool m_bIsStandardPictureSizeNeedToBePreserved;
  private string m_drawingFieldName;
  private string m_drawingFieldValue;
  private Stack<int> m_fieldResultGroupStack = new Stack<int>();
  private Stack<int> m_fieldInstructionGroupStack = new Stack<int>();
  private Stack<int> m_fieldGroupStack = new Stack<int>();
  private Stack<WField> m_fieldCollectionStack = new Stack<WField>();
  private Stack<string> m_formFieldDataStack = new Stack<string>();
  private string m_currentFieldGroupData;
  private Stack<FieldGroupType> m_fieldGroupTypeStack = new Stack<FieldGroupType>();
  private string m_defaultCodePage;
  private int m_defaultFontIndex;
  private bool m_bIsRowBorderTop;
  private bool m_bIsRowBorderBottom;
  private bool m_bIsRowBorderLeft;
  private bool m_bIsRowBorderRight;
  private float m_leftcellspace;
  private float m_rightcellspace;
  private float m_bottomcellspace;
  private float m_topcellspace;
  private bool m_bIsWord97StylePadding;
  private bool isWrapPolygon;
  private int m_currenttrleft;
  private Dictionary<string, WComment> m_comments;
  private List<string> m_commRangeStartId;
  private WComment m_currComment;
  private string m_commAtnText;
  private bool m_isCommentRangeStart;
  private bool m_isCommentReference;
  private bool m_isCommentOwnerParaIsCell;
  private Stack<string> m_commentGroupCollection = new Stack<string>();
  private bool m_isLevelTextLengthRead;
  private bool m_isFirstPlaceHolderRead;
  private bool m_isPibName;
  private bool m_isPibFlags;
  private string m_href = "";
  private string m_externalLink = "";
  private bool m_isImageHyperlink;
  private string m_linkType = "";
  private string m_uniqueStyleID;
  private Tokens token;
  private Groups group;
  private List<Groups> groupOrder;
  private bool isNested;
  private bool isSpecialCharacter;
  private bool isPlainTagPresent;
  private bool isPardTagpresent;
  private bool m_isDistFromLeft;
  private string m_DistFromLeftVal;
  private RtfFont m_previousRtfFont;
  private byte m_bFlags;

  private bool IsStyleSheet
  {
    get => ((int) this.m_bFlags & 16 /*0x10*/) >> 4 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 239 | (value ? 1 : 0) << 4);
  }

  private bool m_isPnNextList
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  private bool IsSectionBreak
  {
    get => ((int) this.m_bFlags & 8) >> 3 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 247 | (value ? 1 : 0) << 3);
  }

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

  internal int DefaultFontIndex
  {
    get => this.m_defaultFontIndex;
    set => this.m_defaultFontIndex = value;
  }

  public bool IsDestinationControlWord => this.m_destStack.Count > 0;

  public bool IsFormFieldGroup => this.m_formFieldDataStack.Count > 0;

  public bool IsFieldGroup => this.m_fieldGroupStack.Count > 0;

  public int CurrentLevel => this.m_currentLevel;

  public int PreviousLevel => this.m_previousLevel;

  public WTextBody CurrentTextBody => this.m_textBody;

  protected IWParagraph CurrentPara
  {
    get
    {
      if (this.m_currParagraph == null)
        this.m_currParagraph = (IWParagraph) new WParagraph((IWordDocument) this.m_document);
      return this.m_currParagraph;
    }
    set => this.m_currParagraph = value;
  }

  protected Column CurrColumn
  {
    get
    {
      if (this.m_currColumn == null)
        this.m_currColumn = (this.CurrentSection as WSection).AddColumn(1f, 1f, true);
      return this.m_currColumn;
    }
    set => this.m_currColumn = value;
  }

  protected IWSection CurrentSection
  {
    get
    {
      if (this.m_currSection == null)
      {
        this.m_currSection = this.m_document.AddSection();
        this.m_currSection.PageSetup.EqualColumnWidth = true;
        this.m_textBody = this.m_currSection.Body;
      }
      return this.m_currSection;
    }
  }

  protected ListStyle CurrListStyle
  {
    get => this.m_currListStyle;
    set => this.m_currListStyle = value;
  }

  private WComment CurrentComment
  {
    get => this.m_currComment;
    set => this.m_currComment = value;
  }

  private List<string> CommentStartIdList
  {
    get
    {
      if (this.m_commRangeStartId == null)
        this.m_commRangeStartId = new List<string>();
      return this.m_commRangeStartId;
    }
  }

  private string CommentLinkText
  {
    get => this.m_commAtnText;
    set => this.m_commAtnText = value;
  }

  private Dictionary<string, WComment> Comments
  {
    get
    {
      if (this.m_comments == null)
        this.m_comments = new Dictionary<string, WComment>();
      return this.m_comments;
    }
  }

  protected WListLevel CurrListLevel
  {
    get => this.m_currListLevel;
    set => this.m_currListLevel = value;
  }

  private bool IsLevelTextLengthRead
  {
    get => this.m_isLevelTextLengthRead;
    set => this.m_isLevelTextLengthRead = value;
  }

  public RtfFont CurrRtfFont
  {
    get => this.m_rtfFont;
    set => this.m_rtfFont = value;
  }

  public RtfColor CurrColorTable
  {
    get => this.m_rtfColorTable;
    set => this.m_rtfColorTable = value;
  }

  public IWTable CurrTable
  {
    get => this.m_currTable;
    set => this.m_currTable = value;
  }

  public WTableRow CurrRow
  {
    get => this.m_currRow;
    set => this.m_currRow = value;
  }

  public WTableCell CurrCell
  {
    get => this.m_currCell;
    set => this.m_currCell = value;
  }

  public CellFormat CurrCellFormat
  {
    get
    {
      if (this.m_currCellFormat == null)
        this.m_currCellFormat = new CellFormat();
      return this.m_currCellFormat;
    }
    set => this.m_currCellFormat = value;
  }

  public RowFormat CurrRowFormat
  {
    get
    {
      if (this.m_currRowFormat == null)
        this.m_currRowFormat = new RowFormat((IWordDocument) this.m_document);
      return this.m_currRowFormat;
    }
    set => this.m_currRowFormat = value;
  }

  public TabFormat CurrTabFormat
  {
    get
    {
      if (this.m_currTabFormat == null)
        this.m_currTabFormat = new TabFormat();
      return this.m_currTabFormat;
    }
    set => this.m_currTabFormat = value;
  }

  public RtfParser(WordDocument document, Stream stream)
  {
    this.m_rtfReader = new RtfReader(stream);
    this.m_lexer = new RtfLexer(this.m_rtfReader);
    this.m_document = document;
    this.m_currTextFormat = new RtfParser.TextFormat();
    this.m_currParagraphFormat = new WParagraphFormat((IWordDocument) this.m_document);
    this.m_secFormat = new RtfParser.SecionFormat();
    this.CurrTable = (IWTable) null;
    this.m_currRow = (WTableRow) null;
    this.m_currCell = (WTableCell) null;
  }

  public void ParseToken()
  {
    int index = 0;
    this.groupOrder = new List<Groups>();
    this.InitDefaultCompatibilityOptions();
    this.m_token = this.m_lexer.ReadNextToken(this.m_previousTokenKey, this.m_bIsLevelText);
    while ((long) this.m_rtfReader.Position <= this.m_rtfReader.Length)
    {
      if (this.m_token == "{")
      {
        this.group = new Groups();
        this.groupOrder.Add(this.group);
        ++index;
        this.m_tokenType = RtfTokenType.GroupStart;
        this.ParseGroupStart();
        if (this.m_isCommentReference)
          this.m_commentGroupCollection.Push(this.m_token);
      }
      else if (this.m_token == "}")
      {
        --index;
        if (this.m_previousToken == "par")
        {
          if (!this.IsPnListStyleDefined(this.groupOrder[this.groupOrder.Count - 1]))
            this.m_isPnNextList = true;
          else
            this.m_bIsList = false;
          if (this.isNested)
            this.PlainCount(this.groupOrder[index]);
          this.groupOrder.RemoveRange(index, 1);
        }
        else if (index != 0)
        {
          this.groupOrder[index - 1].ChildElements.Add(this.groupOrder[index]);
          this.groupOrder.RemoveRange(index, 1);
        }
        this.m_tokenType = RtfTokenType.GroupEnd;
        this.ParseGroupEnd();
        if (this.m_isCommentReference)
        {
          if (this.m_commentGroupCollection.Count != 0)
          {
            this.m_commentGroupCollection.Pop();
          }
          else
          {
            this.m_isCommentReference = false;
            if (!this.m_textBody.ChildEntities.Contains((IEntity) this.CurrentPara))
              this.m_textBody.ChildEntities.Add((IEntity) this.CurrentPara);
            this.m_textBody = !this.m_isCommentOwnerParaIsCell ? this.CurrentSection.Body : (WTextBody) this.m_currCell;
            this.CurrentPara = this.m_textBody.LastParagraph;
            this.CurrentPara.ChildEntities.Add((IEntity) this.CurrentComment);
            if (this.m_textFormatStack.Count > 1)
              this.m_textFormatStack.Pop();
            this.CurrentComment = (WComment) null;
          }
        }
        if (this.m_rtfCollectionStack.Count == 0)
          break;
      }
      else if (this.m_token == ";")
      {
        if (this.m_previousToken == "colortbl")
          this.m_currColorIndex = 0;
        if (this.m_bIsLevelText)
        {
          this.m_bIsLevelText = false;
          this.IsLevelTextLengthRead = false;
          this.m_isFirstPlaceHolderRead = false;
        }
        this.m_tokenType = RtfTokenType.Unknown;
        this.m_lexer.CurrRtfTokenType = RtfTokenType.Unknown;
        if (this.m_currentTableType == RtfTableType.ColorTable && this.m_previousTokenKey == "blue")
          this.AddColorTableEntry();
        else if (this.m_currentTableType == RtfTableType.FontTable)
          this.AddFontTableEntry();
        else if (this.m_currentTableType == RtfTableType.StyleSheet)
          this.AddStyleSheetEntry();
        else
          this.ParseDocumentElement(this.m_token);
      }
      else if (this.StartsWithExt(this.m_token, "\\"))
      {
        this.group = this.groupOrder[this.groupOrder.Count - 1];
        this.m_tokenType = RtfTokenType.ControlWord;
        this.ParseControlStart();
      }
      else if (this.m_token == "\r" || this.m_token == "\n" || this.m_token == string.Empty)
        this.m_tokenType = RtfTokenType.Unknown;
      else
        this.ParseDocumentElement(this.m_token);
      if (this.m_previousToken == string.Empty && this.m_prevTokenType == RtfTokenType.ControlWord && (this.m_token == "\r" || this.m_token == "\n"))
        this.ParseParagraphEnd();
      if (this.m_token != null && this.m_token != "\r" && this.m_token != "\n" && this.m_token != " ")
        this.m_previousControlString = this.m_token;
      if (this.m_token != null && this.m_tokenType == RtfTokenType.ControlWord && this.m_token != " ")
        this.m_previousToken = this.m_token;
      if (this.m_token == "emdash" || this.m_token == "endash")
        this.m_prevTokenType = RtfTokenType.Text;
      else if (this.m_prevTokenType != RtfTokenType.Text || !(this.m_token == "\r") && !(this.m_token == "\n"))
        this.m_prevTokenType = this.m_tokenType;
      this.m_token = this.m_lexer.ReadNextToken(this.m_previousTokenKey, this.m_bIsLevelText);
      this.SetParsedElementFlag(this.m_token);
      if (this.m_token.Contains("\\stylesheet") && this.m_document.Styles.Count > 0 && this.IsStyleSheet)
        this.SkipGroup();
      if (this.m_token.Contains("\\colortbl") && this.m_colorTable.Count > 0)
        this.SkipGroup();
      if (this.m_token.Contains("\\macpict"))
        this.SkipGroup();
      if (this.m_token.Contains("\\footnote"))
        this.SkipGroup();
      if (this.m_token.Contains("\\txfieldtext") && this.m_previousControlString == "*")
        this.SkipGroup();
      if (this.m_token.Contains("\\shprslt") && (this.m_currShape != null && this.m_currShape.AutoShapeType != AutoShapeType.Unknown || this.m_currTextBox != null))
      {
        this.SkipGroup();
        if (this.m_currShape != null)
          this.AddAdjustValues();
        if (this.m_currShape != null && this.m_currShape.EffectList.Count != 0 && this.m_currShape.EffectList[0].IsShadowEffect)
          this.AddShadowDirectionandDistance();
        if (this.m_currTextBox != null && this.m_currTextBox.Shape != null)
          this.SetDefaultValuesForShapeTextBox();
      }
      if (this.m_token.Contains("\\shpgrp"))
        this.SkipGroup();
      if (this.m_token.Contains("\\formfield"))
      {
        this.m_currentFormField = new FormFieldData();
        this.m_formFieldDataStack.Push("{");
      }
      if (this.StartsWithExt(this.m_token, "\\jpegblip") || this.StartsWithExt(this.m_token, "\\wmetafile") || this.StartsWithExt(this.m_token, "\\pngblip") || this.StartsWithExt(this.m_token, "\\emfblip") || this.StartsWithExt(this.m_token, "\\macpict") || this.StartsWithExt(this.m_token, "\\objdata"))
        this.m_lexer.IsImageBytes = true;
      if (this.StartsWithExt(this.m_token, "\\dibitmap"))
      {
        this.m_lexer.IsImageBytes = true;
        this.SkipGroup();
        this.m_lexer.IsImageBytes = false;
      }
      if (this.StartsWithExt(this.m_token, "\\wmetafile"))
        this.m_bIsStandardPictureSizeNeedToBePreserved = true;
    }
    if (this.m_previousToken != "sv" && this.isWrapPolygon)
      this.isWrapPolygon = false;
    this.m_lexer.Close();
    this.m_currentLevel = 0;
    this.m_bInTable = false;
    if (this.m_textBody != null && this.m_textBody.Items.LastItem != this.CurrentSection.Body.Items.LastItem)
      this.ParseParagraphEnd();
    if (this.CurrentPara.Items.Count > 0)
    {
      this.CurrentSection.Paragraphs.Add(this.CurrentPara);
      if (this.m_paragraphFormatStack.Count > 0)
        this.CopyParagraphFormatting(this.m_currParagraphFormat, this.CurrentPara.ParagraphFormat);
    }
    if (this.CurrentSection.Body.ChildEntities.Count > 0)
    {
      this.AddNewSection(this.CurrentSection);
      this.ApplySectionFormatting();
    }
    this.Close();
  }

  private void PlainCount(Groups group)
  {
    int num = 0;
    foreach (object childElement in group.ChildElements)
    {
      if (childElement is Tokens)
      {
        Tokens tokens = childElement as Tokens;
        if (tokens.TokenName == "plain" && !this.m_bIsListText)
          ++num;
        else if (tokens.TokenName == "atrfend")
          --num;
      }
    }
    for (; num > 0 && this.m_textFormatStack.Count > 1; --num)
      this.m_textFormatStack.Pop();
    if (this.m_textFormatStack.Count <= 0)
      return;
    this.m_currTextFormat = this.m_textFormatStack.Peek();
  }

  private bool IsPnListStyleDefined(Groups group)
  {
    bool flag = false;
    for (int index = 0; index < group.ChildElements.Count; ++index)
    {
      Groups childElement1 = group.ChildElements[index];
      if (flag)
        return flag;
      foreach (object childElement2 in childElement1.ChildElements)
      {
        if (childElement2 is Tokens)
        {
          Tokens tokens = childElement2 as Tokens;
          if (tokens.TokenName == "pnlvlbody" || tokens.TokenName == "pnlvlcont" || tokens.TokenName == "pnlvlblt")
            flag = true;
        }
      }
    }
    return flag;
  }

  private bool IsPnListStyleDefinedExisting(Groups group)
  {
    bool flag = false;
    int num1 = 0;
    int num2 = 0;
    for (int index = group.ChildElements.Count - 1; index > 0 && num1 <= 1 && num2 <= 0; --index)
    {
      Groups childElement1 = group.ChildElements[index];
      if (!flag)
      {
        if (group.ChildElements[index] is Tokens)
        {
          Tokens childElement2 = group.ChildElements[index] as Tokens;
          if (childElement2.TokenName == "ilvl" || childElement2.TokenName == "ls")
          {
            flag = false;
            break;
          }
          if (childElement2.TokenName == "pnlvlbody")
          {
            flag = true;
            break;
          }
          if (childElement2.TokenName == "par")
            ++num1;
        }
        foreach (object childElement3 in childElement1.ChildElements)
        {
          if (childElement3 is Tokens)
          {
            Tokens tokens = childElement3 as Tokens;
            if (tokens.TokenName == "ilvl" || tokens.TokenName == "ls")
            {
              flag = false;
              ++num2;
              break;
            }
            if (tokens.TokenName == "pnlvlbody")
            {
              flag = true;
              ++num2;
              break;
            }
            if (tokens.TokenName == "par")
              ++num1;
          }
        }
      }
    }
    return flag;
  }

  private void InitDefaultCompatibilityOptions()
  {
    this.m_document.Settings.SetCompatibilityModeValue(CompatibilityMode.Word2007);
    this.m_document.DOP.Dop2000.Copts.DontUseHTMLParagraphAutoSpacing = true;
    this.m_document.DOP.Dop2000.Copts.DontBreakWrappedTables = true;
    this.m_document.DOP.Dop2000.Copts.Copts80.Copts60.NoSpaceRaiseLower = false;
  }

  public void Close()
  {
    this.m_lexer = (RtfLexer) null;
    this.m_rtfReader = (RtfReader) null;
    this.m_previousToken = (string) null;
    this.m_previousTokenKey = (string) null;
    this.m_previousTokenValue = (string) null;
    this.m_fontTable.Clear();
    this.m_listOverrideTable.Clear();
    this.m_listTable.Clear();
    this.m_colorTable.Clear();
    this.m_styleTable.Clear();
    this.m_fontTable = (Dictionary<string, RtfFont>) null;
    this.m_listOverrideTable = (Dictionary<string, string>) null;
    this.m_listTable = (Dictionary<string, ListStyle>) null;
    this.m_colorTable = (Dictionary<int, RtfColor>) null;
    this.m_styleTable = (Dictionary<string, IWParagraphStyle>) null;
    this.m_tabFormatStack.Clear();
    this.m_tabFormatStack = (Stack<Dictionary<int, TabFormat>>) null;
    this.m_stack.Clear();
    this.m_stack = (Stack<string>) null;
    this.m_textFormatStack.Clear();
    this.m_textFormatStack = (Stack<RtfParser.TextFormat>) null;
    this.m_unicodeCountStack.Clear();
    this.m_unicodeCountStack = (Stack<int>) null;
    this.m_shapeInstructionStack.Clear();
    this.m_shapeInstructionStack = (Stack<string>) null;
    this.m_shapeTextStack.Clear();
    this.m_shapeTextStack = (Stack<string>) null;
    this.m_shapeTextBody.Clear();
    this.m_shapeTextBody = (Stack<WTextBody>) null;
    this.m_shapeParagraph.Clear();
    this.m_shapeParagraph = (Stack<WParagraph>) null;
    this.m_rtfCollectionStack.Clear();
    this.m_rtfCollectionStack = (Stack<string>) null;
    this.m_prevRowFormatStack.Clear();
    this.m_prevRowFormatStack = (Stack<RowFormat>) null;
    this.m_prevCellFormatStack.Clear();
    this.m_prevCellFormatStack = (Stack<Dictionary<int, CellFormat>>) null;
    this.m_pictureOrShapeStack.Clear();
    this.m_pictureOrShapeStack = (Stack<string>) null;
    this.m_groupShapeStack.Clear();
    this.m_groupShapeStack = (Stack<string>) null;
    this.m_shapeFlagStack.Clear();
    this.m_shapeFlagStack = (Stack<Dictionary<string, bool>>) null;
    this.m_listLevelStack.Clear();
    this.m_listLevelStack = (Stack<string>) null;
    this.m_headerFooterStack.Clear();
    this.m_headerFooterStack = (Stack<string>) null;
    this.m_formFieldDataStack.Clear();
    this.m_formFieldDataStack = (Stack<string>) null;
    this.m_fieldResultGroupStack.Clear();
    this.m_fieldResultGroupStack = (Stack<int>) null;
    this.m_fieldGroupTypeStack.Clear();
    this.m_fieldGroupTypeStack = (Stack<FieldGroupType>) null;
    this.m_fieldGroupStack.Clear();
    this.m_fieldGroupStack = (Stack<int>) null;
    this.m_fieldCollectionStack.Clear();
    this.m_fieldCollectionStack = (Stack<WField>) null;
    this.m_destStack.Clear();
    this.m_destStack = (Stack<string>) null;
    this.m_currRowFormatStack.Clear();
    this.m_currRowFormatStack = (Stack<RowFormat>) null;
    this.m_CellFormatStack.Clear();
    this.m_CellFormatStack = (Stack<Dictionary<int, CellFormat>>) null;
    this.groupOrder.Clear();
    this.groupOrder = (List<Groups>) null;
    this.m_drawingFields.Clear();
    this.m_drawingFields = (List<RtfParser.TempShapeProperty>) null;
    this.m_shapeTextbodyStack.Clear();
    this.m_shapeTextbodyStack = (Stack<Dictionary<string, object>>) null;
    this.m_ownerShapeTextbodyStack.Clear();
    this.m_ownerShapeTextbodyStack = (Stack<Dictionary<string, object>>) null;
  }

  private void AddFontTableEntry()
  {
    bool flag = false;
    foreach (KeyValuePair<string, RtfFont> keyValuePair in this.m_fontTable)
    {
      if (keyValuePair.Key == this.m_rtfFont.FontID)
        flag = true;
    }
    if (this.m_rtfFont.FontName == null || this.m_rtfFont.FontID == null)
      return;
    this.m_rtfFont.FontName = this.m_rtfFont.FontName.Trim();
    if (flag)
      this.m_fontTable[this.m_rtfFont.FontID].FontName = this.m_rtfFont.FontName;
    else
      this.m_fontTable.Add(this.m_rtfFont.FontID, this.m_rtfFont);
    if (this.m_rtfFont.AlternateFontName == null)
      return;
    if (this.m_document.FontSubstitutionTable.ContainsKey(this.m_rtfFont.FontName))
      this.m_document.FontSubstitutionTable[this.m_rtfFont.FontName] = this.m_rtfFont.AlternateFontName;
    else
      this.m_document.FontSubstitutionTable.Add(this.m_rtfFont.FontName, this.m_rtfFont.AlternateFontName);
  }

  private void AddColorTableEntry()
  {
    this.m_colorTable.Add(++this.m_currColorIndex, this.m_rtfColorTable);
    this.m_rtfColorTable = new RtfColor();
  }

  private void AddStyleSheetEntry()
  {
    if (this.m_styleName == null || this.m_styleName != null && this.m_styleName.Length == 0)
      this.m_styleName = "Style" + Guid.NewGuid().ToString();
    if (this.m_currStyle == null && this.m_currStyleID == null && this.m_currParagraphFormat != null)
    {
      this.m_currStyleID = string.Empty;
      this.m_currStyle = (IWParagraphStyle) new WParagraphStyle((IWordDocument) this.m_document);
    }
    if (this.m_currStyle != null)
    {
      Dictionary<string, string> builtinStyles = (this.m_currStyle as Style).GetBuiltinStyles();
      if (builtinStyles.ContainsKey(this.m_styleName.ToLower()))
        this.m_styleName = builtinStyles[this.m_styleName.ToLower()];
      (this.m_currStyle as Style).SetStyleName(this.m_styleName);
      Style style = this.IsStylePresent(this.m_currStyle.Name, StyleType.ParagraphStyle);
      if (style == null)
      {
        IWParagraphStyle wparagraphStyle = this.m_document.AddParagraphStyle(this.m_currStyle.Name);
        this.CopyParagraphFormatting(this.m_currParagraphFormat, wparagraphStyle.ParagraphFormat);
        this.UpdateTabsCollection(wparagraphStyle.ParagraphFormat);
        this.CopyTextFormatToCharFormat(wparagraphStyle.CharacterFormat, this.m_currTextFormat);
        if (!this.m_styleTable.ContainsKey(this.m_currStyleID))
          this.m_styleTable.Add(this.m_currStyleID, this.m_currStyle);
      }
      else
      {
        this.m_currStyle = style as IWParagraphStyle;
        this.CopyParagraphFormatting(this.m_currParagraphFormat, this.m_currStyle.ParagraphFormat);
        this.UpdateTabsCollection(this.m_currStyle.ParagraphFormat);
        this.CopyTextFormatToCharFormat(this.m_currStyle.CharacterFormat, this.m_currTextFormat);
      }
    }
    if (this.m_currCharStyle != null)
    {
      this.m_currCharStyle.SetStyleName(this.m_styleName);
      this.CopyTextFormatToCharFormat(this.m_currCharStyle.CharacterFormat, this.m_currTextFormat);
      if (this.IsStylePresent(this.m_currCharStyle.Name, StyleType.CharacterStyle) == null)
      {
        this.m_document.Styles.Add((IStyle) this.m_currCharStyle);
        if (!this.m_charStyleTable.ContainsKey(this.m_currStyleID))
          this.m_charStyleTable.Add(this.m_currStyleID, this.m_currCharStyle);
      }
    }
    this.m_styleName = string.Empty;
    this.m_currStyle = (IWParagraphStyle) null;
    this.m_currCharStyle = (WCharacterStyle) null;
  }

  private Style IsStylePresent(string styleName, StyleType styleType)
  {
    foreach (Style style in (IEnumerable) this.m_document.Styles)
    {
      if (style.Name == styleName && style.StyleType == styleType)
        return style;
    }
    return (Style) null;
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
    this.token = new Tokens();
    this.token.TokenName = strArray[0];
    this.token.TokenValue = strArray[1];
    this.group.ChildElements.Add((Groups) this.token);
    if (strArray[0] != null && (this.StartsWithExt(strArray[0], "atnid") || this.StartsWithExt(strArray[0], "atnauthor")))
      strArray = this.SeparateAnnotationToken(strArray);
    if (strArray[0] != null && this.StartsWithExt(strArray[0], "atnparent"))
      strArray[0] = "atnparent";
    if (this.StartsWithExt(this.m_token, "cellx"))
    {
      if (!this.m_bIsBorderLeft && this.CurrCellFormat.Borders.Left.BorderType == BorderStyle.None)
        this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.Cleared;
      if (!this.m_bIsBorderRight && this.CurrCellFormat.Borders.Right.BorderType == BorderStyle.None)
        this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.Cleared;
      if (!this.m_bIsBorderTop && this.CurrCellFormat.Borders.Top.BorderType == BorderStyle.None)
        this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.Cleared;
      if (!this.m_bIsBorderBottom && this.CurrCellFormat.Borders.Bottom.BorderType == BorderStyle.None)
        this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.Cleared;
    }
    if (this.m_token == "defshp")
      this.m_bIsFallBackImage = true;
    this.ParseControlWords(this.m_token, strArray[0], strArray[1], strArray[2]);
    this.m_previousTokenKey = strArray[0];
    this.m_previousTokenValue = strArray[1];
  }

  private string[] SeparateAnnotationToken(string[] value)
  {
    string str = string.Empty;
    if (this.StartsWithExt(value[0], "atnid"))
      str = value[0].Substring(0, "atnid".Length);
    if (this.StartsWithExt(value[0], "atnauthor"))
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
    if (this.m_bIsPictureOrShape)
      this.m_pictureOrShapeStack.Push("{");
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
      this.m_textFormatStack.Push(this.m_currTextFormat.Clone());
      this.m_currTextFormat = this.m_textFormatStack.Peek();
      this.m_tabFormatStack.Push(new Dictionary<int, TabFormat>((IDictionary<int, TabFormat>) this.m_tabCollection));
      this.m_tabCollection = this.m_tabFormatStack.Peek();
      WParagraphFormat destParaFormat = new WParagraphFormat((IWordDocument) this.m_document);
      if (this.m_paragraphFormatStack.Count > 0)
        this.CopyParagraphFormatting(this.m_currParagraphFormat, destParaFormat);
      this.m_paragraphFormatStack.Push(destParaFormat);
      this.m_currParagraphFormat = this.m_paragraphFormatStack.Peek();
    }
    if (this.m_rtfCollectionStack.Count > 0)
      this.m_rtfCollectionStack.Push("\\");
    if (this.m_bIsShapeInstruction)
      this.m_shapeInstructionStack.Push("{");
    if (this.m_bIsShapeText)
      this.m_shapeTextStack.Push("{");
    if (this.m_bIsGroupShape)
      this.m_groupShapeStack.Push("{");
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
        {
          this.m_bIsListText = false;
          this.CopyParagraphFormatting(this.CurrentPara.ParagraphFormat, this.m_listLevelParaFormat);
          this.CopyTextFormatToCharFormat(this.m_listLevelCharFormat, this.m_currTextFormat);
          if (this.m_currTextFormat.FontFamily == string.Empty)
            this.ResetListFontName(this.m_listLevelCharFormat);
          this.CurrentPara = this.m_prevParagraph;
          this.m_currTextFormat = this.m_prevTextFormat;
          if (this.m_previousRtfFont != null)
            this.CurrRtfFont = this.m_previousRtfFont;
          this.m_previousRtfFont = (RtfFont) null;
        }
        this.m_bIsDocumentInfo = false;
        this.m_bIsCustomProperties = false;
        if (this.m_currentTableType == RtfTableType.FontTable && this.m_rtfFont != null && this.m_rtfFont.FontID != null && !this.m_fontTable.ContainsKey(this.m_rtfFont.FontID))
          this.AddFontTableEntry();
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
      this.m_tabFormatStack.Push(new Dictionary<int, TabFormat>((IDictionary<int, TabFormat>) this.m_tabCollection));
    }
    if (this.m_currentTableType == RtfTableType.FontTable && this.m_rtfFont != null && this.m_rtfFont.FontID != null && !this.m_fontTable.ContainsKey(this.m_rtfFont.FontID))
      this.AddFontTableEntry();
    if (this.m_bIsHeader || this.m_bIsFooter)
    {
      this.m_headerFooterStack.Pop();
      if (this.m_headerFooterStack.Count == 0)
      {
        if (this.m_currParagraph != null && this.m_currParagraph.ChildEntities.Count != 0)
          this.ParseParagraphEnd();
        if (this.m_currTable != null)
          this.ProcessTableInfo(false);
        this.m_bIsHeader = false;
        this.m_bIsFooter = false;
        this.m_textBody = (WTextBody) null;
        this.m_currTextFormat = new RtfParser.TextFormat();
        this.m_textFormatStack.Clear();
        this.m_textFormatStack.Push(this.m_currTextFormat);
        this.m_tabFormatStack.Clear();
        this.m_paragraphFormatStack.Clear();
      }
    }
    if (this.IsFieldGroup)
      this.ParseGroupEndWithinFieldGroup();
    if (this.m_bIsListLevel)
    {
      if (this.m_listLevelStack.Count > 0)
        this.m_listLevelStack.Pop();
      if (this.m_listLevelStack.Count == 0)
      {
        this.CopyParagraphFormatting(this.CurrentPara.ParagraphFormat, this.CurrListLevel.ParagraphFormat);
        this.CopyTextFormatToCharFormat(this.CurrListLevel.CharacterFormat, this.m_currTextFormat);
        this.CurrentPara = (IWParagraph) null;
        this.m_currTextFormat = new RtfParser.TextFormat();
        this.m_bIsListLevel = false;
      }
      if (this.m_bIsLevelText)
      {
        this.m_bIsLevelText = false;
        this.IsLevelTextLengthRead = false;
        this.m_isFirstPlaceHolderRead = false;
      }
    }
    if (this.m_bIsShapeText)
    {
      this.m_shapeTextStack.Pop();
      if (this.m_shapeTextStack.Count == 0)
      {
        if (this.m_currParagraph.ChildEntities.Count > 0)
        {
          if (this.IsFieldGroup && this.m_currentFieldGroupData != string.Empty)
          {
            this.ParseFieldGroupData(this.m_currentFieldGroupData);
            this.m_currentFieldGroupData = string.Empty;
          }
          this.ParseParagraphEnd();
          this.isPlainTagPresent = false;
          this.isPardTagpresent = false;
        }
        this.m_bIsShapeText = false;
        if (this.IsFieldGroup && this.m_currentFieldGroupData != string.Empty)
        {
          this.ParseFieldGroupData(this.m_currentFieldGroupData);
          this.m_currentFieldGroupData = string.Empty;
        }
        this.ResetParagraphFormat(this.m_currParagraphFormat);
        this.ResetCharacterFormat(this.CurrentPara.BreakCharacterFormat);
        this.CopyParagraphFormatting(this.m_currParagraphFormat, this.CurrentPara.ParagraphFormat);
        this.ProcessTableInfo(true);
        if (this.CurrCell != null)
          this.CopyTextFormatToCharFormat(this.CurrCell.CharacterFormat, this.m_currTextFormat);
        this.m_bCellFinished = true;
        this.m_currParagraph = (IWParagraph) new WParagraph((IWordDocument) this.m_document);
        this.ParseRowEnd(true);
        IWTable currTable = this.m_currTable;
        bool flag1 = this.m_currTable != null;
        int count = this.m_nestedTable.Count;
        this.ProcessTableInfo(true);
        bool flag2 = flag1 && this.m_currTable == null;
        this.ResetShapeTextbodyStack();
        if (flag2 || count > this.m_nestedTable.Count)
          this.m_previousLevel = this.m_currentLevel;
        WTextBody lastCell = this.m_textBody == null || !(this.m_textBody.ChildEntities.LastItem is WTable) ? (WTextBody) null : (WTextBody) (this.m_textBody.ChildEntities.LastItem as WTable).LastCell;
        if (currTable != null && this.m_textBody != null && currTable == this.m_textBody.ChildEntities.LastItem && this.m_textBody.ChildEntities.LastItem is WTable && (this.m_textBody.ChildEntities.LastItem as WTable).Rows.Count > 1)
        {
          this.MoveItemsToShape(lastCell);
          (this.m_textBody.ChildEntities.LastItem as WTable).LastRow.RemoveSelf();
        }
        else if (lastCell != null)
        {
          this.MoveItemsToShape(lastCell);
          this.m_textBody.ChildEntities.LastItem.RemoveSelf();
        }
      }
    }
    if (this.m_bIsPictureOrShape)
    {
      if (this.m_pictureOrShapeStack.Count > 0)
      {
        this.m_pictureOrShapeStack.Pop();
        this.m_bIsShapeResult = this.m_bIsShapeResult && this.m_bShapeResultStackCount <= this.m_pictureOrShapeStack.Count;
      }
      if (this.m_pictureOrShapeStack.Count == 0)
      {
        this.m_bIsPictureOrShape = false;
        this.m_lexer.IsImageBytes = false;
        this.m_bIsFallBackImage = false;
        if (this.m_ownerShapeTextbodyStack.Count > 0)
          this.ResetOwnerShapeStack();
      }
    }
    if (this.m_bIsGroupShape)
    {
      if (this.m_groupShapeStack.Count > 0)
        this.m_groupShapeStack.Pop();
      if (this.m_groupShapeStack.Count == 0)
        this.m_bIsGroupShape = false;
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
      if (this.m_textFormatStack.Count > 1)
        this.m_textFormatStack.Pop();
      if (this.m_textFormatStack.Count > 0)
        this.m_currTextFormat = this.m_textFormatStack.Peek();
      if (this.m_tabFormatStack.Count > 1)
        this.m_tabFormatStack.Pop();
      if (this.m_tabFormatStack.Count > 0)
        this.m_tabCollection = this.m_tabFormatStack.Peek();
      if (this.m_paragraphFormatStack.Count > 1)
        this.m_paragraphFormatStack.Pop();
      if (this.m_paragraphFormatStack.Count > 0)
        this.m_currParagraphFormat = this.m_paragraphFormatStack.Peek();
    }
    if (this.m_drawingFieldName != null && this.m_drawingFieldValue != null)
    {
      if (this.m_drawingFieldName == "shapeType" && this.m_drawingFieldValue != "75" && this.m_drawingFieldValue != "202" && !this.m_bIsGroupShape)
      {
        this.m_currShape = this.CurrentPara.AppendShape(this.GetAutoShapeType(this.m_drawingFieldValue), this.m_currShapeFormat.m_width, this.m_currShapeFormat.m_height);
        this.m_bIsShape = true;
        this.m_currShape.WrapFormat.AllowOverlap = true;
        this.ApplyShapeFormatting(this.m_currShape, this.m_picFormat, this.m_currShapeFormat);
        this.ParseDrawingFields();
      }
      if (this.m_drawingFieldName == "shapeType" && this.m_drawingFieldValue == "202" && !this.m_bIsGroupShape)
      {
        this.m_currTextBox = this.CurrentPara.AppendTextBox(this.m_currShapeFormat.m_width, this.m_currShapeFormat.m_height) as WTextBox;
        this.m_currTextBox.TextBoxFormat.AllowOverlap = true;
        this.m_currTextBox.Shape = new Shape((IWordDocument) this.m_document, AutoShapeType.Rectangle);
        this.m_currTextBox.Shape.Height = this.m_currShapeFormat.m_height;
        this.m_currTextBox.Shape.Width = this.m_currShapeFormat.m_width;
        this.m_currTextBox.Shape.WrapFormat.AllowOverlap = true;
        this.ApplyShapeFormatting(this.m_currTextBox.Shape, this.m_picFormat, this.m_currShapeFormat);
        this.ApplyTextBoxFormatting(this.m_currTextBox, this.m_picFormat, this.m_currShapeFormat);
        this.ParseDrawingFields();
      }
      if (this.m_currShape == null)
        this.m_bIsShapePicture = (!(this.m_drawingFieldName == "fHorizRule") || !(this.m_drawingFieldValue == "1")) && this.m_bIsShapePicture;
      if (this.m_currPicture != null && this.m_bIsPictureOrShape && this.m_bIsShapePicture || (this.m_currShape != null || this.m_currTextBox != null) && this.m_bIsShape)
      {
        this.ParseShapeToken(this.m_drawingFieldName, this.m_drawingFieldName, this.m_drawingFieldValue);
        this.ApplyTextBoxFormatsToShape();
      }
      else if (!string.IsNullOrEmpty(this.m_drawingFieldName) && !string.IsNullOrEmpty(this.m_drawingFieldValue) && this.m_drawingFieldName.ToLower() == "shapetype")
      {
        int result;
        if (int.TryParse(this.m_drawingFieldValue, out result))
          this.SetShapeElementsFlag(result);
      }
      else if (this.m_currShape == null && this.m_currTextBox == null && this.m_currPicture == null)
        this.m_drawingFields.Add(new RtfParser.TempShapeProperty(this.m_drawingFieldName, this.m_drawingFieldValue));
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
    if (this.m_formFieldDataStack.Count != 0)
      return;
    this.WriteFormFieldProperties();
  }

  private void ParseDrawingFields()
  {
    foreach (RtfParser.TempShapeProperty drawingField in this.m_drawingFields)
    {
      this.ParseShapeToken(drawingField.m_drawingFieldName, drawingField.m_drawingFieldName, drawingField.m_drawingFieldValue);
      this.ApplyTextBoxFormatsToShape();
    }
    this.m_drawingFields.Clear();
  }

  private void ParsePictureDrawingFields()
  {
    foreach (RtfParser.TempShapeProperty drawingField in this.m_drawingFields)
      this.ParsePictureToken(drawingField.m_drawingFieldName, drawingField.m_drawingFieldName, drawingField.m_drawingFieldValue);
    this.m_drawingFields.Clear();
  }

  private void MoveItemsToShape(WTextBody textBody)
  {
    if (this.m_currShape != null)
    {
      foreach (Entity childEntity in (CollectionImpl) textBody.ChildEntities)
        this.m_currShape.TextBody.ChildEntities.Add((IEntity) childEntity.Clone());
    }
    else
    {
      if (this.m_currTextBox == null)
        return;
      foreach (Entity childEntity in (CollectionImpl) textBody.ChildEntities)
        this.m_currTextBox.TextBoxBody.ChildEntities.Add((IEntity) childEntity.Clone());
    }
  }

  private void ParseGroupEndWithinFieldGroup()
  {
    if (this.m_currentFieldGroupData != string.Empty)
    {
      this.ParseFieldGroupData(this.m_currentFieldGroupData);
      this.m_currentFieldGroupData = string.Empty;
    }
    this.EnsureFieldSubGroupEnd(FieldGroupType.FieldResult);
    this.EnsureFieldSubGroupEnd(FieldGroupType.FieldInstruction);
    this.EnsureFieldGroupEnd();
  }

  private void EnsureFieldSubGroupEnd(FieldGroupType fieldGroupType)
  {
    switch (fieldGroupType)
    {
      case FieldGroupType.FieldInstruction:
        if (this.m_fieldInstructionGroupStack.Count <= 0)
          break;
        int num1 = this.m_fieldInstructionGroupStack.Pop() - 1;
        if (num1 == 0)
        {
          if (this.m_fieldGroupTypeStack.Count <= 0 || this.m_fieldGroupTypeStack.Peek() != fieldGroupType)
            break;
          int num2 = (int) this.m_fieldGroupTypeStack.Pop();
          break;
        }
        this.m_fieldInstructionGroupStack.Push(num1);
        break;
      case FieldGroupType.FieldResult:
        if (this.m_fieldResultGroupStack.Count <= 0)
          break;
        int num3 = this.m_fieldResultGroupStack.Pop() - 1;
        if (num3 == 0)
        {
          if (this.m_fieldGroupTypeStack.Count <= 0 || this.m_fieldGroupTypeStack.Peek() != fieldGroupType)
            break;
          int num4 = (int) this.m_fieldGroupTypeStack.Pop();
          break;
        }
        this.m_fieldResultGroupStack.Push(num3);
        break;
    }
  }

  private void EnsureFieldGroupEnd()
  {
    int num = this.m_fieldGroupStack.Pop() - 1;
    if (num == 0)
    {
      if (this.m_fieldCollectionStack.Count > 0)
      {
        int count = this.CurrentPara.Items.Count;
        WFieldMark wfieldMark = new WFieldMark((IWordDocument) this.m_document);
        wfieldMark.Type = FieldMarkType.FieldEnd;
        this.CurrentPara.ChildEntities.Add((IEntity) wfieldMark);
        this.m_fieldCollectionStack.Peek().FieldEnd = wfieldMark;
      }
      if (this.m_fieldCollectionStack.Count <= 0)
        return;
      WField wfield = this.m_fieldCollectionStack.Pop();
      if (wfield.FieldType == FieldType.FieldUnknown)
      {
        wfield.UpdateUnknownFieldType((WCharacterFormat) null);
      }
      else
      {
        if (wfield.FieldType == FieldType.FieldTOC)
          return;
        string unknownFieldType = wfield.GetFieldCodeForUnknownFieldType();
        wfield.UpdateFieldCode(unknownFieldType);
      }
    }
    else
      this.m_fieldGroupStack.Push(num);
  }

  private void WriteFormFieldProperties()
  {
    WFormField wformField = (WFormField) null;
    if (this.m_fieldCollectionStack.Count > 0)
      wformField = this.m_fieldCollectionStack.Peek() as WFormField;
    if (wformField == null)
      return;
    this.ApplyFormFieldProperties(wformField);
    switch (wformField.FormFieldType)
    {
      case FormFieldType.TextInput:
        this.ApplyTextFormFieldProperties(wformField as WTextFormField);
        break;
      case FormFieldType.CheckBox:
        this.ApplyCheckboxPorperties(wformField as WCheckBox);
        break;
      case FormFieldType.DropDown:
        this.ApplyDropDownFormFieldProperties(wformField as WDropDownFormField);
        break;
    }
  }

  private string RemoveDelimiterSpace(string token)
  {
    if ((!this.StartsWithExt(this.m_previousControlString, "u") || this.m_previousControlString.Length <= 1 || !char.IsNumber(this.m_previousControlString[1]) || !(this.m_previousTokenKey == "u")) && (this.m_previousControlString == "}" || this.m_lexer.CurrRtfTokenType == RtfTokenType.Text || this.StartsWithExt(this.m_token, "u") || this.m_tokenType == RtfTokenType.Unknown && !this.m_bIsListText && token != null && !this.m_bIsBackgroundCollection))
      return token;
    if (this.m_token.Length <= 1)
      return (string) null;
    if (this.m_tokenType != RtfTokenType.GroupStart)
      token = token.Substring(1, token.Length - 1);
    return token;
  }

  private bool IsPictureToken()
  {
    return this.m_bIsPictureOrShape && this.m_bIsShapePicture && this.m_lexer.IsImageBytes;
  }

  private void ParseDocumentElement(string m_token)
  {
    if (this.StartsWithExt(m_token, " "))
      m_token = this.RemoveDelimiterSpace(m_token);
    if (!string.IsNullOrEmpty(m_token) && !string.IsNullOrEmpty(this.m_previousControlString) && this.StartsWithExt(this.m_previousControlString, "u") && (this.m_previousControlString.Length > 1 && this.m_previousTokenKey == "u" && char.IsNumber(this.m_previousControlString[1]) || this.m_previousControlString.Length > 2 && this.m_previousTokenKey == "u-" && char.IsNumber(this.m_previousControlString[2])))
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
    if (this.m_bIsBackgroundCollection && this.IsPictureToken() && !this.m_bIsGroupShape)
      this.ParseImageBytes();
    if (this.m_bIsListText || m_token == null || this.m_bIsBackgroundCollection)
      return;
    this.m_tokenType = RtfTokenType.Text;
    this.m_lexer.CurrRtfTokenType = RtfTokenType.Text;
    if (this.IsPictureToken() && !this.m_bIsGroupShape)
      this.ParseImageBytes();
    else if (this.m_bIsBookmarkStart)
    {
      this.CurrentPara.AppendBookmarkStart(m_token);
      this.m_bIsBookmarkStart = false;
    }
    else if (this.m_bIsBookmarkEnd)
    {
      if (!(this.CurrentPara.ChildEntities.LastItem is BookmarkEnd) || !((this.CurrentPara.ChildEntities.LastItem as BookmarkEnd).Name == m_token))
        this.CurrentPara.AppendBookmarkEnd(m_token);
      this.m_bIsBookmarkEnd = false;
    }
    else if (this.m_bIsCustomProperties)
      this.ParseCustomDocumentProperties(m_token);
    else if (this.m_bIsDocumentInfo)
      this.ParseBuiltInDocumentProperties(m_token);
    else if (this.IsFormFieldGroup && this.m_currentFormField != null)
      this.ParseFormFieldDestinationWords(m_token);
    else if (this.IsFieldGroup && !this.m_bIsPictureOrShape && this.m_previousToken != "sn" && this.m_previousToken != "sv")
      this.m_currentFieldGroupData += m_token;
    else if (this.m_isCommentRangeStart && !this.m_bIsPictureOrShape && !this.m_lexer.IsImageBytes)
    {
      this.CommentLinkText = m_token;
      if (!this.isSpecialCharacter)
        m_token = this.GetEncodedString(m_token);
      else
        this.isSpecialCharacter = false;
      this.tr = this.CurrentPara.AppendText(m_token);
      this.CopyTextFormatToCharFormat(this.tr.CharacterFormat, this.m_currTextFormat);
      if (!this.isPlainTagPresent && this.m_document.LastParagraph != null && this.IsDefaultTextFormat(this.m_currTextFormat))
      {
        if (this.m_document.LastParagraph.BreakCharacterFormat.HasValue(106))
          this.m_document.LastParagraph.BreakCharacterFormat.PropertiesHash.Remove(106);
        this.CurrentPara.BreakCharacterFormat.CopyFormat((FormatBase) this.m_document.LastParagraph.BreakCharacterFormat);
        this.tr.CharacterFormat.CopyProperties((FormatBase) this.m_document.LastParagraph.BreakCharacterFormat);
      }
      this.CurrentPara.Items.Add((IEntity) this.tr);
    }
    else if (this.m_isCommentReference && !this.m_bIsPictureOrShape && !this.m_bIsDocumentInfo)
    {
      if (this.m_bIsList && this.CurrentPara.ListFormat != null && this.CurrentPara.ListFormat.CurrentListLevel == null && this.CurrentPara.ListFormat.CurrentListStyle == null)
        this.CurrentPara.ListFormat.ContinueListNumbering();
      if (!this.isSpecialCharacter)
        m_token = this.GetEncodedString(m_token);
      else
        this.isSpecialCharacter = false;
      this.tr = this.CurrentPara.AppendText(m_token);
      this.CopyTextFormatToCharFormat(this.tr.CharacterFormat, this.m_currTextFormat);
      if (this.isPlainTagPresent || this.m_document.LastParagraph == null || !this.IsDefaultTextFormat(this.m_currTextFormat))
        return;
      if (this.m_document.LastParagraph.BreakCharacterFormat.HasValue(106))
        this.m_document.LastParagraph.BreakCharacterFormat.PropertiesHash.Remove(106);
      this.CurrentPara.BreakCharacterFormat.CopyFormat((FormatBase) this.m_document.LastParagraph.BreakCharacterFormat);
      this.tr.CharacterFormat.CopyProperties((FormatBase) this.m_document.LastParagraph.BreakCharacterFormat);
    }
    else if (this.m_currentTableType == RtfTableType.None && (!this.IsDestinationControlWord || this.m_bIsShapeText && this.m_previousTokenKey != "pntxta" && this.m_previousTokenKey != "pntxtb") && !this.m_bIsDocumentInfo && !this.m_bIsPictureOrShape)
    {
      if (!string.IsNullOrEmpty(this.m_previousToken) && this.StartsWithExt(this.m_previousToken, "'") && this.m_bIsAccentChar)
      {
        m_token = " " + m_token;
        this.m_bIsAccentChar = false;
      }
      if (this.m_prevTokenType == RtfTokenType.Text && this.tr != null)
      {
        if (!this.isSpecialCharacter)
          m_token = this.GetEncodedString(m_token);
        else
          this.isSpecialCharacter = false;
        this.tr.Text += m_token;
      }
      else
      {
        if (this.m_bIsList && this.CurrentPara.ListFormat != null && this.CurrentPara.ListFormat.CurrentListLevel == null && this.CurrentPara.ListFormat.CurrentListStyle == null)
          this.CurrentPara.ListFormat.ContinueListNumbering();
        if (!this.isSpecialCharacter)
          m_token = this.GetEncodedString(m_token);
        else
          this.isSpecialCharacter = false;
        this.tr = this.CurrentPara.AppendText(m_token);
        if (this.tr.CharacterFormat.BaseFormat != null && this.tr.CharacterFormat.BaseFormat.OwnerBase is WParagraphStyle)
          this.ResetTextBackgroundColor(this.tr.CharacterFormat);
        this.CopyTextFormatToCharFormat(this.tr.CharacterFormat, this.m_currTextFormat);
        if (!this.isPlainTagPresent && this.m_document.LastParagraph != null && this.IsDefaultTextFormat(this.m_currTextFormat))
        {
          if (this.m_document.LastParagraph.BreakCharacterFormat.HasValue(106))
            this.m_document.LastParagraph.BreakCharacterFormat.PropertiesHash.Remove(106);
          this.CurrentPara.BreakCharacterFormat.CopyFormat((FormatBase) this.m_document.LastParagraph.BreakCharacterFormat);
          this.tr.CharacterFormat.CopyProperties((FormatBase) this.m_document.LastParagraph.BreakCharacterFormat);
        }
      }
      this.m_bIsBookmarkEnd = false;
    }
    else if (this.IsDestinationControlWord && this.m_previousTokenKey == "pntxta" && this.CurrentPara.ListFormat != null && this.CurrentPara.ListFormat.CurrentListLevel != null)
      this.CurrentPara.ListFormat.CurrentListLevel.NumberSuffix = m_token;
    else if (this.IsDestinationControlWord && this.m_previousTokenKey == "pntxtb" && this.CurrentPara.ListFormat != null && this.CurrentPara.ListFormat.CurrentListLevel != null)
      this.CurrentPara.ListFormat.CurrentListLevel.NumberPrefix = m_token;
    else if (this.m_currentTableType == RtfTableType.FontTable && !this.IsDestinationControlWord)
      this.m_rtfFont.FontName += m_token;
    else if (this.m_currentTableType == RtfTableType.FontTable && this.m_previousTokenKey == "falt")
      this.m_rtfFont.AlternateFontName += m_token;
    else if (this.m_currentTableType == RtfTableType.StyleSheet)
    {
      if (string.IsNullOrEmpty(this.m_styleName))
        this.m_styleName += m_token.Trim();
      else
        this.m_styleName += m_token;
    }
    else if (this.m_previousToken == "sn" && this.m_bIsPictureOrShape && this.m_bIsShapePicture)
    {
      this.m_drawingFieldName = m_token;
      if (m_token == "pWrapPolygonVertices")
        this.isWrapPolygon = true;
      if (m_token == "pibName")
        this.m_isPibName = true;
      if (m_token == "pibFlags")
        this.m_isPibFlags = true;
      if (!(m_token == "dxWrapDistLeft"))
        return;
      this.m_isDistFromLeft = true;
    }
    else if (this.m_previousToken == "sv" && this.m_drawingFieldName != null && this.m_bIsPictureOrShape && this.m_bIsShapePicture)
    {
      if (this.isWrapPolygon)
        this.m_drawingFieldValue += m_token;
      else
        this.m_drawingFieldValue = m_token;
      if (this.m_isPibName)
      {
        this.m_externalLink = m_token;
        this.m_isPibName = false;
      }
      if (this.m_isPibFlags)
      {
        this.m_linkType = m_token;
        this.m_isPibFlags = false;
      }
      if (!this.m_isDistFromLeft)
        return;
      this.m_DistFromLeftVal = m_token;
      this.m_isDistFromLeft = false;
    }
    else
    {
      if (!this.m_isImageHyperlink || !(this.m_drawingFieldName == "pihlShape") || !this.m_bIsPictureOrShape || !this.m_bIsShapePicture || !(this.m_previousToken == "hlfr") && !(this.m_previousToken == "hlsrc"))
        return;
      this.m_href = m_token;
      this.m_isImageHyperlink = false;
    }
  }

  private bool IsDefaultTextFormat(RtfParser.TextFormat m_currTextFormat)
  {
    RtfParser.TextFormat textFormat = new RtfParser.TextFormat();
    return m_currTextFormat.AllCaps == textFormat.AllCaps && m_currTextFormat.BackColor == textFormat.BackColor && m_currTextFormat.Bidi == textFormat.Bidi && m_currTextFormat.Bold == textFormat.Bold && (double) m_currTextFormat.CharacterSpacing == (double) textFormat.CharacterSpacing && m_currTextFormat.CharacterStyleName == textFormat.CharacterStyleName && m_currTextFormat.DoubleStrike == textFormat.DoubleStrike && m_currTextFormat.Emboss == textFormat.Emboss && m_currTextFormat.Engrave == textFormat.Engrave && m_currTextFormat.FontColor == textFormat.FontColor && m_currTextFormat.FontFamily == textFormat.FontFamily && (double) m_currTextFormat.FontSize == (double) textFormat.FontSize && m_currTextFormat.ForeColor == textFormat.ForeColor && m_currTextFormat.HighlightColor == textFormat.HighlightColor && m_currTextFormat.Italic == textFormat.Italic && (int) m_currTextFormat.LidBi == (int) textFormat.LidBi && (int) m_currTextFormat.LocalIdASCII == (int) textFormat.LocalIdASCII && (int) m_currTextFormat.LocalIdForEast == (int) textFormat.LocalIdForEast && m_currTextFormat.m_subSuperScript == textFormat.m_subSuperScript && m_currTextFormat.m_underlineStyle == textFormat.m_underlineStyle && (double) m_currTextFormat.Position == (double) textFormat.Position && (double) m_currTextFormat.Scaling == (double) textFormat.Scaling && m_currTextFormat.Shadow == textFormat.Shadow && m_currTextFormat.SmallCaps == textFormat.SmallCaps && m_currTextFormat.SpecVanish == textFormat.SpecVanish && m_currTextFormat.Strike == textFormat.Strike && m_currTextFormat.Style == textFormat.Style && m_currTextFormat.TextAlign == textFormat.TextAlign && m_currTextFormat.Underline == textFormat.Underline;
  }

  private string GetEncodedString(string m_token)
  {
    m_token.ToCharArray();
    Encoding encoding = this.GetEncoding();
    string str1 = (string) null;
    if (encoding.WebName == "gb2312")
    {
      byte[] bytes = this.GetEncoding("Windows-1252").GetBytes(m_token);
      str1 = encoding.GetString(bytes, 0, bytes.Length);
    }
    else
    {
      for (int index = 0; index < m_token.Length; ++index)
      {
        byte[] bytes = BitConverter.GetBytes((int) m_token[index]);
        string str2 = encoding.GetString(bytes, 0, bytes.Length).Replace("\0", "");
        str1 += str2;
      }
    }
    return str1 != null ? (m_token = str1) : m_token;
  }

  private Encoding GetEncoding() => Encoding.GetEncoding(this.GetCodePage());

  private Encoding GetEncoding(string codePage) => Encoding.GetEncoding(codePage);

  private void ResetTextBackgroundColor(WCharacterFormat sourceFormat)
  {
    if (!(sourceFormat.TextBackgroundColor != Color.Empty) || sourceFormat.HasKey(9))
      return;
    sourceFormat.TextBackgroundColor = Color.Empty;
  }

  private WTextRange GetFieldCodeTextRange(WField field, string fieldCode)
  {
    WTextRange fieldCodeTextRange = new WTextRange((IWordDocument) this.m_document);
    fieldCodeTextRange.Text = fieldCode;
    fieldCodeTextRange.ApplyCharacterFormat(field.CharacterFormat);
    return fieldCodeTextRange;
  }

  private void ParseFieldGroupData(string token)
  {
    FieldType fieldType = FieldType.FieldUnknown;
    if (this.m_fieldGroupTypeStack.Count > 0 && this.m_fieldGroupTypeStack.Peek() == FieldGroupType.FieldInstruction && this.m_fieldCollectionStack.Count < this.m_fieldGroupStack.Count)
      fieldType = this.GetFieldType(token.Trim());
    switch (fieldType)
    {
      case FieldType.FieldIf:
        this.ApplyFieldProperties((WField) new WIfField((IWordDocument) this.m_document), token, fieldType);
        break;
      case FieldType.FieldSequence:
        this.ApplyFieldProperties((WField) new WSeqField((IWordDocument) this.m_document), token, fieldType);
        break;
      case FieldType.FieldTOC:
        this.ParseTOCField(token, fieldType);
        break;
      case FieldType.FieldMergeField:
        this.ApplyFieldProperties((WField) new WMergeField((IWordDocument) this.m_document), token, fieldType);
        break;
      case FieldType.FieldFormTextInput:
        this.ApplyFieldProperties((WField) new WTextFormField((IWordDocument) this.m_document), token, fieldType);
        this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_2, 14);
        break;
      case FieldType.FieldFormCheckBox:
        this.ApplyFieldProperties((WField) new WCheckBox((IWordDocument) this.m_document), token, fieldType);
        this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_1, 9);
        break;
      case FieldType.FieldFormDropDown:
        this.ApplyFieldProperties((WField) new WDropDownFormField((IWordDocument) this.m_document), token, fieldType);
        this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_1, 15);
        break;
      case FieldType.FieldShape:
        this.ApplyFieldProperties(new WField((IWordDocument) this.m_document), token, fieldType);
        break;
      case FieldType.FieldUnknown:
        this.ParseUnknownField(token, fieldType);
        break;
      default:
        this.ApplyFieldProperties(new WField((IWordDocument) this.m_document), token, fieldType);
        if (fieldType != FieldType.FieldHyperlink)
          break;
        this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_1, 23);
        break;
    }
  }

  private void ParseTOCField(string token, FieldType fieldType)
  {
    TableOfContent tableOfContent = new TableOfContent((IWordDocument) this.m_document, token);
    this.m_fieldCollectionStack.Push(tableOfContent.TOCField);
    this.m_document.TOC.Add(tableOfContent.TOCField, tableOfContent);
    this.CurrentPara.ChildEntities.Add((IEntity) tableOfContent);
    this.CurrentPara.ChildEntities.Add((IEntity) this.GetFieldCodeTextRange(tableOfContent.TOCField, token));
    this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_2, 15);
  }

  private void ParseUnknownField(string token, FieldType fieldType)
  {
    if (!(this.m_previousToken != "datafield") || this.m_fieldGroupTypeStack.Count == 0)
      return;
    switch (this.m_fieldGroupTypeStack.Peek())
    {
      case FieldGroupType.FieldInstruction:
        if (this.m_fieldCollectionStack.Count < this.m_fieldGroupStack.Count)
        {
          if (token == null || token == string.Empty)
            break;
          this.ApplyFieldProperties(new WField((IWordDocument) this.m_document), token, fieldType);
          break;
        }
        if (this.m_fieldCollectionStack.Count <= 0 || this.m_bIsGroupShape)
          break;
        this.AppendTextRange(token);
        break;
      case FieldGroupType.FieldResult:
        if (this.m_bIsGroupShape)
          break;
        this.AppendTextRange(token);
        break;
    }
  }

  private void AppendTextRange(string token)
  {
    WTextRange wtextRange = new WTextRange((IWordDocument) this.m_document);
    wtextRange.Text = token;
    this.CurrentPara.ChildEntities.Add((IEntity) wtextRange);
    this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_2, 12);
    this.CopyTextFormatToCharFormat(wtextRange.CharacterFormat, this.m_currTextFormat);
  }

  private void ReplaceWfieldWithWMergeFieldObject()
  {
    WMergeField wmergeField = new WMergeField((IWordDocument) this.m_document);
    string internalFieldCode = this.m_fieldCollectionStack.Peek().InternalFieldCode;
    wmergeField.FieldType = FieldType.FieldMergeField;
    this.CopyTextFormatToCharFormat(wmergeField.CharacterFormat, this.m_currTextFormat);
    if (this.m_currParagraph.Items.LastItem.EntityType != EntityType.Field || (this.m_currParagraph.Items.LastItem as WField).FieldType != FieldType.FieldMergeField)
      return;
    this.m_currParagraph.Items.Remove((IEntity) this.m_currParagraph.Items.LastItem);
    this.m_currParagraph.Items.Add((IEntity) wmergeField);
    wmergeField.FieldCode = internalFieldCode;
    this.m_fieldCollectionStack.Pop();
    this.m_fieldCollectionStack.Push((WField) wmergeField);
  }

  private void ApplyFieldProperties(WField field, string token, FieldType fieldType)
  {
    field.FieldType = fieldType;
    this.CopyTextFormatToCharFormat(field.CharacterFormat, this.m_currTextFormat);
    this.CurrentPara.ChildEntities.Add((IEntity) field);
    this.CurrentPara.ChildEntities.Add((IEntity) this.GetFieldCodeTextRange(field, token));
    this.m_fieldCollectionStack.Push(field);
  }

  private void ParseFormFieldDestinationWords(string token)
  {
    this.m_token = this.m_token.TrimStart();
    switch (this.m_previousToken)
    {
      case "ffname":
        this.m_currentFormField.Name = token;
        break;
      case "ffdeftext":
        this.m_currentFormField.DefaultText = token;
        break;
      case "ffformat":
        this.m_currentFormField.StringFormat = token;
        break;
      case "ffhelptext":
        this.m_currentFormField.HelpText = token;
        break;
      case "ffstattext":
        this.m_currentFormField.StatusHelpText = token;
        break;
      case "ffentrymcr":
        this.m_currentFormField.MarcoOnStart = token;
        break;
      case "ffexitmcr":
        this.m_currentFormField.MacroOnExit = token;
        break;
      case "ffl":
        this.m_currentFormField.DropDownItems.Add(token);
        break;
    }
  }

  private void ParseImageBytes()
  {
    if (this.StartsWithExt(this.m_previousToken, "blipuid") || this.StartsWithExt(this.m_previousToken, "dximageuri"))
    {
      if (!(this.m_previousControlString == "}"))
        return;
      this.AppendPictureToParagraph(this.m_token);
    }
    else
      this.AppendPictureToParagraph(this.m_token);
  }

  private void ParseCustomDocumentProperties(string m_token)
  {
    switch (this.m_previousToken)
    {
      case "propname":
        this.m_currPropertyName = m_token;
        break;
      case "staticval":
        switch (this.m_currPropertyType)
        {
          case Syncfusion.CompoundFile.DocIO.PropertyType.Int16:
          case Syncfusion.CompoundFile.DocIO.PropertyType.Int32:
          case Syncfusion.CompoundFile.DocIO.PropertyType.Int:
            this.m_currPropertyValue = (object) Convert.ToInt32(m_token);
            break;
          case Syncfusion.CompoundFile.DocIO.PropertyType.Double:
            this.m_currPropertyValue = (object) Convert.ToDouble(m_token);
            break;
          case Syncfusion.CompoundFile.DocIO.PropertyType.Bool:
            this.m_currPropertyValue = (object) Convert.ToBoolean(Convert.ToInt32(m_token));
            break;
          case Syncfusion.CompoundFile.DocIO.PropertyType.DateTime:
            this.m_currPropertyValue = (object) Convert.ToDateTime(m_token, (IFormatProvider) CultureInfo.InvariantCulture);
            break;
          default:
            this.m_currPropertyValue = (object) m_token;
            break;
        }
        if (this.m_currPropertyName != null && this.m_currPropertyValue != null)
        {
          this.m_document.CustomDocumentProperties.Add(this.m_currPropertyName, this.m_currPropertyValue);
          this.m_document.CustomDocumentProperties[this.m_currPropertyName].PropertyType = this.m_currPropertyType;
        }
        this.m_currPropertyName = (string) null;
        this.m_currPropertyValue = (object) null;
        break;
    }
  }

  private void ParseBuiltInDocumentProperties(string m_token)
  {
    switch (this.m_previousToken)
    {
      case "title":
        this.m_document.BuiltinDocumentProperties.Title += m_token;
        break;
      case "category":
        this.m_document.BuiltinDocumentProperties.Category += m_token;
        break;
      case "doccomm":
        this.m_document.BuiltinDocumentProperties.Comments += m_token;
        break;
      case "operator":
        this.m_document.BuiltinDocumentProperties.Author += m_token;
        break;
      case "manager":
        this.m_document.BuiltinDocumentProperties.Manager += m_token;
        break;
      case "company":
        this.m_document.BuiltinDocumentProperties.Company += m_token;
        break;
      case "keywords":
        this.m_document.BuiltinDocumentProperties.Keywords += m_token;
        break;
      case "subject":
        this.m_document.BuiltinDocumentProperties.Subject += m_token;
        break;
    }
  }

  private FieldType GetFieldType(string token)
  {
    string str;
    if (token.Contains(" "))
    {
      int length = token.IndexOf(" ");
      str = token.Substring(0, length);
    }
    else
      str = token;
    return FieldTypeDefiner.GetFieldType(str.Trim());
  }

  private string GetFormattingString(string fieldInstruction, string fieldTypeString)
  {
    string empty = string.Empty;
    return fieldInstruction.Replace(fieldTypeString, string.Empty).Trim();
  }

  private void ApplyDropDownFormFieldProperties(WDropDownFormField dropDownFormField)
  {
    dropDownFormField.DefaultDropDownValue = this.m_currentFormField.Ffdefres;
    for (int index = 0; index < this.m_currentFormField.DropDownItems.Count; ++index)
    {
      if (this.m_currentFormField.DropDownItems[index].Text != null && this.m_currentFormField.DropDownItems[index].Text != string.Empty)
        dropDownFormField.DropDownItems.Add(this.m_currentFormField.DropDownItems[index].Text);
    }
  }

  private void ApplyTextFormFieldProperties(WTextFormField textField)
  {
    textField.DefaultText = this.m_currentFormField.DefaultText != null || this.m_currentFormField.MaxLength >= "     ".Length || this.m_currentFormField.MaxLength == 0 ? (this.m_currentFormField.DefaultText != null ? this.m_currentFormField.DefaultText : "     ") : string.Empty;
    if (this.m_currentFormField.MaxLength > 0)
      textField.MaximumLength = this.m_currentFormField.MaxLength;
    textField.StringFormat = this.m_currentFormField.StringFormat != null ? this.m_currentFormField.StringFormat : string.Empty;
  }

  private void ApplyCheckboxPorperties(WCheckBox checkbox)
  {
    checkbox.SizeType = this.m_currentFormField.CheckboxSizeType;
    checkbox.SetCheckBoxSizeValue(this.m_currentFormField.CheckboxSize);
    checkbox.DefaultCheckBoxValue = this.m_currentFormField.Ffdefres == 1;
    checkbox.Checked = this.m_currentFormField.Ffres == 1 || this.m_currentFormField.Ffres == 25 && checkbox.DefaultCheckBoxValue;
  }

  private void ApplyFormFieldProperties(WFormField formField)
  {
    if (this.m_currentFormField.Name != null && this.m_document.Bookmarks[this.m_currentFormField.Name] == null)
      formField.Name = this.m_currentFormField.Name;
    formField.Help = this.m_currentFormField.HelpText;
    formField.StatusBarHelp = this.m_currentFormField.StatusHelpText;
    formField.MacroOnStart = this.m_currentFormField.MarcoOnStart;
    formField.MacroOnEnd = this.m_currentFormField.MacroOnExit;
    formField.Enabled = this.m_currentFormField.Enabled;
    formField.CalculateOnExit = this.m_currentFormField.CalculateOnExit;
  }

  private bool IsSupportedPicture()
  {
    return !this.m_bIsObject && !this.m_bIsGroupShape && !this.m_bIsFallBackImage && (!this.m_bIsShape || !this.m_bIsShapePictureAdded);
  }

  private void AppendPictureToParagraph(string token)
  {
    byte[] imageByteArray = this.GetImageByteArray(token);
    if (this.m_bIsBackgroundCollection)
    {
      this.m_document.Background.ImageBytes = imageByteArray;
      this.m_document.Background.Type = BackgroundType.Picture;
    }
    else
    {
      if (this.IsSupportedPicture())
      {
        this.m_currPicture = this.CurrentPara.AppendPicture(imageByteArray);
        this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_1, 31 /*0x1F*/);
        this.ParsePictureDrawingFields();
        this.ApplyPictureFormatting(this.m_currPicture, this.m_picFormat);
        this.m_bIsShapePictureAdded = true;
        if (!string.IsNullOrEmpty(this.m_href))
          (this.m_currPicture as WPicture).Href = this.m_href;
        if (!string.IsNullOrEmpty(this.m_externalLink))
        {
          (this.m_currPicture as WPicture).ExternalLink = this.m_externalLink;
          Image imageStream = new DocxParser().DownloadImage(this.m_externalLink);
          if (imageStream != null)
            this.m_currPicture.LoadImage(imageStream);
          if (imageByteArray != null)
            (this.m_currPicture as WPicture).HasImageRecordReference = true;
        }
        if (!string.IsNullOrEmpty(this.m_linkType))
          (this.m_currPicture as WPicture).LinkType = this.m_linkType;
        if (!string.IsNullOrEmpty(this.m_DistFromLeftVal))
        {
          (this.m_currPicture as WPicture).DistanceFromLeft = (float) this.GetIntValue(this.m_DistFromLeftVal) / 12700f;
          this.m_DistFromLeftVal = (string) null;
        }
      }
      else if (!this.m_bIsGroupShape && this.m_bIsFallBackImage && (this.m_currShape != null || this.m_currTextBox != null))
      {
        WPicture currPicture = new WPicture((IWordDocument) this.m_document);
        currPicture.LoadImage(imageByteArray);
        this.ApplyPictureFormatting((IWPicture) currPicture, this.m_picFormat);
        if (!string.IsNullOrEmpty(this.m_DistFromLeftVal))
        {
          currPicture.DistanceFromLeft = (float) this.GetIntValue(this.m_DistFromLeftVal) / 12700f;
          this.m_DistFromLeftVal = (string) null;
        }
        if (this.m_currShape != null)
        {
          this.m_currShape.FallbackPic = currPicture;
        }
        else
        {
          this.m_currTextBox.IsShape = true;
          this.m_currTextBox.Shape.FallbackPic = currPicture;
        }
      }
      this.m_bIsStandardPictureSizeNeedToBePreserved = false;
    }
  }

  private float GetRotationAngle(float rotation)
  {
    if ((double) rotation >= 360.0 || (double) rotation <= -360.0)
      rotation %= 360f;
    if ((double) rotation < 0.0)
      rotation = 360f + rotation;
    return rotation;
  }

  private int GetBufferSize(int bufferSize)
  {
    byte[] rtfData = this.m_rtfReader.RtfData;
    bool flag = false;
    for (char ch = (char) rtfData[this.m_rtfReader.Position + bufferSize]; Array.IndexOf<char>(this.m_lexer.m_delimeters, ch) != -1; ch = (char) rtfData[this.m_rtfReader.Position + bufferSize])
    {
      flag = true;
      --bufferSize;
    }
    return !flag ? bufferSize : bufferSize + 1;
  }

  private byte[] GetImageByteArray(string token)
  {
    if (this.StartsWithExt(token, "bin") || this.StartsWithExt(this.m_previousToken, "bin") && this.m_previousToken != "bin")
    {
      string[] strArray = this.SeperateToken(this.m_token);
      if (this.StartsWithExt(this.m_previousToken, "bin"))
        strArray = this.SeperateToken(this.m_previousToken);
      int int32 = Convert.ToInt32(strArray[1]);
      byte[] destinationArray = new byte[int32];
      Array.Copy((Array) this.m_rtfReader.RtfData, !this.StartsWithExt(token, "bin") || this.m_lexer.m_prevChar != ' ' ? this.m_rtfReader.Position - 1 : this.m_rtfReader.Position, (Array) destinationArray, 0, int32);
      this.m_rtfReader.Position += this.GetBufferSize(int32);
      return destinationArray;
    }
    token = token.Replace(ControlChar.CarriegeReturn, "");
    token = token.Replace(ControlChar.LineFeed, "");
    token = token.Replace(" ", "");
    token = token.ToUpper();
    byte[] bytes = this.m_rtfReader.Encoding.GetBytes(token);
    byte[] imageByteArray = new byte[bytes.Length / 2];
    int index = 0;
    int num1 = 0;
    while (num1 < bytes.Length / 2)
    {
      int num2 = bytes[index] > (byte) 57 ? (int) bytes[index] - 55 : (int) bytes[index] - 48 /*0x30*/;
      int num3 = bytes[index + 1] > (byte) 57 ? (int) bytes[index + 1] - 55 : (int) bytes[index + 1] - 48 /*0x30*/;
      imageByteArray[num1++] = (byte) (num2 << 4 | num3);
      index += 2;
    }
    this.m_lexer.IsImageBytes = false;
    return imageByteArray;
  }

  private void ApplyPictureFormatting(IWPicture currPicture, RtfParser.PictureFormat pictureFormat)
  {
    if ((double) pictureFormat.HeightScale <= 0.0)
      pictureFormat.HeightScale = 100f;
    if ((double) pictureFormat.WidthScale <= 0.0)
      pictureFormat.WidthScale = 100f;
    if (this.m_bIsStandardPictureSizeNeedToBePreserved)
    {
      if (this.m_bIsShape && this.m_currShapeFormat.Size != new SizeF())
      {
        SizeF size = this.m_currShapeFormat.Size;
        pictureFormat.Height = size.Height;
        pictureFormat.Width = size.Width;
      }
      else
      {
        if ((double) pictureFormat.Height <= 0.0 || (double) pictureFormat.Height > 1584.0)
          pictureFormat.Height = 216f;
        if ((double) pictureFormat.Width <= 0.0 || (double) pictureFormat.Width > 1584.0)
          pictureFormat.Width = 216f;
      }
    }
    if (!this.m_bIsShape || this.m_currShapeFormat.Size == new SizeF())
    {
      if ((double) pictureFormat.Height > 0.0 && (double) pictureFormat.Height <= 1584.0)
        (currPicture as WPicture).m_size.Height = pictureFormat.Height;
      if ((double) pictureFormat.Width > 0.0 && (double) pictureFormat.Width <= 1584.0)
        (currPicture as WPicture).m_size.Width = pictureFormat.Width;
      if ((double) pictureFormat.HeightScale > 0.0)
        (currPicture as WPicture).SetHeightScaleValue(pictureFormat.HeightScale);
      if ((double) pictureFormat.WidthScale > 0.0)
        (currPicture as WPicture).SetWidthScaleValue(pictureFormat.WidthScale);
    }
    else
    {
      if ((double) pictureFormat.Height > 0.0 && (double) pictureFormat.Height <= 1584.0)
        (currPicture as WPicture).m_size.Height = this.m_currShapeFormat.Size.Height;
      if ((double) pictureFormat.Width > 0.0 && (double) pictureFormat.Width <= 1584.0)
        (currPicture as WPicture).m_size.Width = this.m_currShapeFormat.Size.Width;
    }
    if (this.m_bIsShapePicture)
    {
      currPicture.HorizontalOrigin = this.m_currShapeFormat.m_horizOrgin;
      currPicture.VerticalOrigin = this.m_currShapeFormat.m_vertOrgin;
      currPicture.HorizontalAlignment = this.m_currShapeFormat.m_horizAlignment;
      (currPicture as WPicture).SetTextWrappingStyleValue(this.m_currShapeFormat.m_textWrappingStyle);
      currPicture.TextWrappingType = this.m_currShapeFormat.m_textWrappingType;
      currPicture.VerticalPosition = this.m_currShapeFormat.m_vertPosition;
      currPicture.HorizontalPosition = this.m_currShapeFormat.m_horizPosition;
      currPicture.IsBelowText = this.m_currShapeFormat.m_isBelowText;
      (currPicture as WPicture).OrderIndex = this.m_picFormat.Zorder * 1024 /*0x0400*/;
    }
    if (string.IsNullOrEmpty(this.m_picFormat.Rotation))
      return;
    float rotation = float.Parse(this.m_picFormat.Rotation) / 65536f;
    WPicture currPicture1 = this.m_currPicture as WPicture;
    currPicture1.Rotation = this.GetRotationAngle(rotation);
    if ((double) currPicture1.Rotation > 44.0 && (double) currPicture1.Rotation < 135.0 || (double) currPicture1.Rotation > 224.0 && (double) currPicture1.Rotation < 315.0)
    {
      float height = currPicture1.Height;
      currPicture1.Height = currPicture1.Width;
      currPicture1.Width = height;
      float num = Math.Abs(currPicture1.Height - currPicture1.Width) / 2f;
      if ((double) currPicture1.Height > (double) currPicture1.Width)
      {
        currPicture1.HorizontalPosition += num;
        currPicture1.VerticalPosition -= num;
      }
      if ((double) currPicture1.Height < (double) currPicture1.Width)
      {
        currPicture1.VerticalPosition += num;
        currPicture1.HorizontalPosition -= num;
      }
    }
    this.m_picFormat.Rotation = (string) null;
  }

  private AutoShapeType GetAutoShapeType(string shapeValue)
  {
    switch (shapeValue)
    {
      case "1":
        return AutoShapeType.Rectangle;
      case "2":
        return AutoShapeType.RoundedRectangle;
      case "3":
        return AutoShapeType.Oval;
      case "4":
        return AutoShapeType.Diamond;
      case "5":
        return AutoShapeType.IsoscelesTriangle;
      case "6":
        return AutoShapeType.RightTriangle;
      case "7":
        return AutoShapeType.Parallelogram;
      case "8":
        return AutoShapeType.Trapezoid;
      case "9":
        return AutoShapeType.Hexagon;
      case "10":
        return AutoShapeType.Octagon;
      case "11":
        return AutoShapeType.Cross;
      case "12":
        return AutoShapeType.Star5Point;
      case "13":
        return AutoShapeType.RightArrow;
      case "14":
        return AutoShapeType.RightArrow;
      case "15":
        return AutoShapeType.Pentagon;
      case "16":
        return AutoShapeType.Cube;
      case "17":
        return AutoShapeType.RoundedRectangularCallout;
      case "18":
        return AutoShapeType.Star16Point;
      case "19":
        return AutoShapeType.Arc;
      case "20":
        return AutoShapeType.Line;
      case "21":
        return AutoShapeType.Plaque;
      case "22":
        return AutoShapeType.Can;
      case "23":
        return AutoShapeType.Donut;
      case "32":
        return AutoShapeType.StraightConnector;
      case "33":
        return AutoShapeType.BentConnector2;
      case "34":
        return AutoShapeType.ElbowConnector;
      case "35":
        return AutoShapeType.BentConnector4;
      case "36":
        return AutoShapeType.BentConnector5;
      case "37":
        return AutoShapeType.CurvedConnector2;
      case "38":
        return AutoShapeType.CurvedConnector;
      case "39":
        return AutoShapeType.CurvedConnector4;
      case "40":
        return AutoShapeType.CurvedConnector5;
      case "41":
        return AutoShapeType.LineCallout1NoBorder;
      case "42":
        return AutoShapeType.LineCallout2NoBorder;
      case "43":
        return AutoShapeType.LineCallout3NoBorder;
      case "44":
        return AutoShapeType.LineCallout1AccentBar;
      case "45":
        return AutoShapeType.LineCallout2AccentBar;
      case "46":
        return AutoShapeType.LineCallout3AccentBar;
      case "47":
        return AutoShapeType.LineCallout1;
      case "48":
        return AutoShapeType.LineCallout2;
      case "49":
        return AutoShapeType.LineCallout3;
      case "50":
        return AutoShapeType.LineCallout1BorderAndAccentBar;
      case "51":
        return AutoShapeType.LineCallout2BorderAndAccentBar;
      case "52":
        return AutoShapeType.LineCallout3BorderAndAccentBar;
      case "53":
        return AutoShapeType.DownRibbon;
      case "54":
        return AutoShapeType.UpRibbon;
      case "55":
        return AutoShapeType.Chevron;
      case "56":
        return AutoShapeType.RegularPentagon;
      case "57":
        return AutoShapeType.NoSymbol;
      case "58":
        return AutoShapeType.Star8Point;
      case "59":
        return AutoShapeType.Star16Point;
      case "60":
        return AutoShapeType.Star32Point;
      case "61":
        return AutoShapeType.RectangularCallout;
      case "62":
        return AutoShapeType.RoundedRectangularCallout;
      case "63":
        return AutoShapeType.OvalCallout;
      case "64":
        return AutoShapeType.Wave;
      case "65":
        return AutoShapeType.FoldedCorner;
      case "66":
        return AutoShapeType.LeftArrow;
      case "67":
        return AutoShapeType.DownArrow;
      case "68":
        return AutoShapeType.UpArrow;
      case "69":
        return AutoShapeType.LeftRightArrow;
      case "70":
        return AutoShapeType.UpDownArrow;
      case "71":
        return AutoShapeType.Explosion1;
      case "72":
        return AutoShapeType.Explosion2;
      case "73":
        return AutoShapeType.LightningBolt;
      case "74":
        return AutoShapeType.Heart;
      case "76":
        return AutoShapeType.QuadArrow;
      case "77":
        return AutoShapeType.LeftArrowCallout;
      case "78":
        return AutoShapeType.RightArrowCallout;
      case "79":
        return AutoShapeType.UpDownArrowCallout;
      case "80":
        return AutoShapeType.DownArrowCallout;
      case "81":
        return AutoShapeType.LeftRightArrowCallout;
      case "82":
        return AutoShapeType.UpDownArrowCallout;
      case "83":
        return AutoShapeType.QuadArrowCallout;
      case "84":
        return AutoShapeType.Bevel;
      case "85":
        return AutoShapeType.LeftBracket;
      case "86":
        return AutoShapeType.RightBracket;
      case "87":
        return AutoShapeType.LeftBrace;
      case "88":
        return AutoShapeType.RightBrace;
      case "89":
        return AutoShapeType.LeftUpArrow;
      case "90":
        return AutoShapeType.BentUpArrow;
      case "91":
        return AutoShapeType.BentArrow;
      case "93":
        return AutoShapeType.StripedRightArrow;
      case "94":
        return AutoShapeType.NotchedRightArrow;
      case "95":
        return AutoShapeType.BlockArc;
      case "92":
        return AutoShapeType.Star24Point;
      case "96":
        return AutoShapeType.SmileyFace;
      case "97":
        return AutoShapeType.VerticalScroll;
      case "98":
        return AutoShapeType.HorizontalScroll;
      case "99":
        return AutoShapeType.CircularArrow;
      case "100":
        return AutoShapeType.CircularArrow;
      case "101":
        return AutoShapeType.UTurnArrow;
      case "102":
        return AutoShapeType.CurvedRightArrow;
      case "103":
        return AutoShapeType.CurvedLeftArrow;
      case "104":
        return AutoShapeType.CurvedUpArrow;
      case "105":
        return AutoShapeType.CurvedDownArrow;
      case "106":
        return AutoShapeType.CloudCallout;
      case "107":
        return AutoShapeType.CurvedDownRibbon;
      case "108":
        return AutoShapeType.CurvedUpRibbon;
      case "109":
        return AutoShapeType.FlowChartProcess;
      case "110":
        return AutoShapeType.FlowChartDecision;
      case "111":
        return AutoShapeType.FlowChartData;
      case "112":
        return AutoShapeType.FlowChartPredefinedProcess;
      case "113":
        return AutoShapeType.FlowChartInternalStorage;
      case "114":
        return AutoShapeType.FlowChartDocument;
      case "115":
        return AutoShapeType.FlowChartMultiDocument;
      case "116":
        return AutoShapeType.FlowChartTerminator;
      case "117":
        return AutoShapeType.FlowChartPreparation;
      case "118":
        return AutoShapeType.FlowChartManualInput;
      case "119":
        return AutoShapeType.FlowChartManualOperation;
      case "120":
        return AutoShapeType.FlowChartConnector;
      case "121":
        return AutoShapeType.FlowChartCard;
      case "122":
        return AutoShapeType.FlowChartPunchedTape;
      case "123":
        return AutoShapeType.FlowChartSummingJunction;
      case "124":
        return AutoShapeType.FlowChartOr;
      case "125":
        return AutoShapeType.FlowChartCollate;
      case "126":
        return AutoShapeType.FlowChartSort;
      case "127":
        return AutoShapeType.FlowChartExtract;
      case "128":
        return AutoShapeType.FlowChartMerge;
      case "130":
        return AutoShapeType.FlowChartStoredData;
      case "131":
        return AutoShapeType.FlowChartSequentialAccessStorage;
      case "132":
        return AutoShapeType.FlowChartMagneticDisk;
      case "133":
        return AutoShapeType.FlowChartDirectAccessStorage;
      case "134":
        return AutoShapeType.FlowChartDisplay;
      case "135":
        return AutoShapeType.FlowChartDelay;
      case "176":
        return AutoShapeType.FlowChartAlternateProcess;
      case "177":
        return AutoShapeType.FlowChartOffPageConnector;
      case "178":
        return AutoShapeType.LineCallout1NoBorder;
      case "179":
        return AutoShapeType.LineCallout1AccentBar;
      case "180":
        return AutoShapeType.LineCallout1;
      case "181":
        return AutoShapeType.LineCallout1BorderAndAccentBar;
      case "182":
        return AutoShapeType.LeftRightUpArrow;
      case "183":
        return AutoShapeType.Sun;
      case "184":
        return AutoShapeType.Moon;
      case "185":
        return AutoShapeType.DoubleBracket;
      case "186":
        return AutoShapeType.DoubleBrace;
      case "187":
        return AutoShapeType.Star4Point;
      case "188":
        return AutoShapeType.DoubleWave;
      default:
        return AutoShapeType.Unknown;
    }
  }

  private void ApplyShapeFormatting(
    Shape currShape,
    RtfParser.PictureFormat pictureFormat,
    RtfParser.ShapeFormat shapeFormat)
  {
    if ((double) pictureFormat.HeightScale <= 0.0)
      pictureFormat.HeightScale = 100f;
    if ((double) pictureFormat.WidthScale <= 0.0)
      pictureFormat.WidthScale = 100f;
    if (this.m_bIsStandardPictureSizeNeedToBePreserved)
    {
      if (this.m_bIsShape && this.m_currShapeFormat.Size != new SizeF())
      {
        SizeF size = this.m_currShapeFormat.Size;
        shapeFormat.m_height = size.Height;
        shapeFormat.m_width = size.Width;
      }
      else
      {
        if ((double) shapeFormat.m_height <= 0.0 || (double) shapeFormat.m_height > 1584.0)
          shapeFormat.m_height = 216f;
        if ((double) shapeFormat.m_width <= 0.0 || (double) shapeFormat.m_width > 1584.0)
          shapeFormat.m_width = 216f;
      }
    }
    if (!this.m_bIsShape || this.m_currShapeFormat.Size == new SizeF())
    {
      if ((double) shapeFormat.m_height > 0.0 && (double) shapeFormat.m_height <= 1584.0)
        currShape.Height = shapeFormat.m_height;
      if ((double) shapeFormat.m_width > 0.0 && (double) shapeFormat.m_width <= 1584.0)
        currShape.Width = shapeFormat.m_width;
      if ((double) pictureFormat.HeightScale > 0.0)
        currShape.HeightScale = pictureFormat.HeightScale;
      if ((double) pictureFormat.WidthScale > 0.0)
        currShape.WidthScale = pictureFormat.WidthScale;
    }
    else
    {
      if ((double) shapeFormat.m_height > 0.0 && (double) shapeFormat.m_height <= 1584.0)
        currShape.Height = this.m_currShapeFormat.Size.Height;
      if ((double) shapeFormat.m_width > 0.0 && (double) shapeFormat.m_width <= 1584.0)
        currShape.Width = this.m_currShapeFormat.Size.Width;
    }
    if (!this.m_bIsShapePicture)
      return;
    currShape.LeftEdgeExtent = this.m_currShapeFormat.m_left;
    currShape.RightEdgeExtent = this.m_currShapeFormat.m_right;
    currShape.TopEdgeExtent = this.m_currShapeFormat.m_top;
    currShape.BottomEdgeExtent = this.m_currShapeFormat.m_bottom;
    currShape.HorizontalOrigin = this.m_currShapeFormat.m_horizOrgin;
    currShape.VerticalOrigin = this.m_currShapeFormat.m_vertOrgin;
    currShape.HorizontalAlignment = this.m_currShapeFormat.m_horizAlignment;
    currShape.WrapFormat.SetTextWrappingStyleValue(this.m_currShapeFormat.m_textWrappingStyle);
    currShape.WrapFormat.TextWrappingType = this.m_currShapeFormat.m_textWrappingType;
    currShape.VerticalPosition = this.m_currShapeFormat.m_vertPosition;
    currShape.HorizontalPosition = this.m_currShapeFormat.m_horizPosition;
    currShape.IsBelowText = this.m_currShapeFormat.m_isBelowText;
    currShape.LockAnchor = this.m_currShapeFormat.m_isLockAnchor;
    currShape.ZOrderPosition = this.m_currShapeFormat.m_zOrder;
    currShape.ShapeID = (long) this.m_currShapeFormat.m_uniqueId;
    currShape.IsLineStyleInline = true;
    if (currShape.FillFormat.Color != Color.Empty)
    {
      currShape.IsFillStyleInline = true;
      currShape.FillFormat.Fill = true;
    }
    if (currShape.WrapFormat.TextWrappingStyle != TextWrappingStyle.Square && currShape.WrapFormat.TextWrappingStyle != TextWrappingStyle.Tight)
      return;
    currShape.WrapFormat.DistanceLeft = 9f;
    currShape.WrapFormat.DistanceRight = 9f;
  }

  private void ApplyTextBoxFormatting(
    WTextBox textBox,
    RtfParser.PictureFormat pictureFormat,
    RtfParser.ShapeFormat shapeFormat)
  {
    if ((double) pictureFormat.HeightScale <= 0.0)
      pictureFormat.HeightScale = 100f;
    if ((double) pictureFormat.WidthScale <= 0.0)
      pictureFormat.WidthScale = 100f;
    if (this.m_bIsStandardPictureSizeNeedToBePreserved)
    {
      if (this.m_bIsShape && this.m_currShapeFormat.Size != new SizeF())
      {
        SizeF size = this.m_currShapeFormat.Size;
        shapeFormat.m_height = size.Height;
        shapeFormat.m_width = size.Width;
      }
      else
      {
        if ((double) shapeFormat.m_height <= 0.0 || (double) shapeFormat.m_height > 1584.0)
          shapeFormat.m_height = 216f;
        if ((double) shapeFormat.m_width <= 0.0 || (double) shapeFormat.m_width > 1584.0)
          shapeFormat.m_width = 216f;
      }
    }
    if (!this.m_bIsShape || this.m_currShapeFormat.Size == new SizeF())
    {
      if ((double) shapeFormat.m_height > 0.0 && (double) shapeFormat.m_height <= 1584.0)
        textBox.TextBoxFormat.Height = shapeFormat.m_height;
      if ((double) shapeFormat.m_width > 0.0 && (double) shapeFormat.m_width <= 1584.0)
        textBox.TextBoxFormat.Width = shapeFormat.m_width;
    }
    else
    {
      if ((double) shapeFormat.m_height > 0.0 && (double) shapeFormat.m_height <= 1584.0)
        textBox.TextBoxFormat.Height = this.m_currShapeFormat.Size.Height;
      if ((double) shapeFormat.m_width > 0.0 && (double) shapeFormat.m_width <= 1584.0)
        textBox.TextBoxFormat.Width = this.m_currShapeFormat.Size.Width;
    }
    if (!this.m_bIsShapePicture)
      return;
    textBox.TextBoxFormat.HorizontalOrigin = this.m_currShapeFormat.m_horizOrgin;
    textBox.TextBoxFormat.VerticalOrigin = this.m_currShapeFormat.m_vertOrgin;
    textBox.TextBoxFormat.HorizontalAlignment = this.m_currShapeFormat.m_horizAlignment;
    textBox.TextBoxFormat.SetTextWrappingStyleValue(this.m_currShapeFormat.m_textWrappingStyle);
    textBox.TextBoxFormat.TextWrappingType = this.m_currShapeFormat.m_textWrappingType;
    textBox.TextBoxFormat.VerticalPosition = this.m_currShapeFormat.m_vertPosition;
    textBox.TextBoxFormat.HorizontalPosition = this.m_currShapeFormat.m_horizPosition;
    textBox.TextBoxFormat.IsBelowText = this.m_currShapeFormat.m_isBelowText;
    textBox.TextBoxFormat.TextBoxShapeID = this.m_currShapeFormat.m_uniqueId;
    textBox.TextBoxFormat.OrderIndex = this.m_currShapeFormat.m_zOrder;
    if (textBox.TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Square || textBox.TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Tight || textBox.TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Through)
    {
      textBox.TextBoxFormat.WrapDistanceLeft = 9f;
      textBox.TextBoxFormat.WrapDistanceRight = 9f;
    }
    if (textBox.Shape != null && (textBox.TextBoxFormat.TextWrappingStyle == TextWrappingStyle.Behind || textBox.Shape.LockAnchor))
      textBox.IsShape = true;
    textBox.TextBoxFormat.InternalMargin.Left = 7.2f;
    textBox.TextBoxFormat.InternalMargin.Right = 7.2f;
    textBox.TextBoxFormat.InternalMargin.Top = 3.6f;
    textBox.TextBoxFormat.InternalMargin.Bottom = 3.6f;
  }

  private void ApplyTextBoxFormatsToShape()
  {
    if (this.m_currTextBox == null || !this.m_currTextBox.IsShape || this.m_currTextBox.Shape == null)
      return;
    Shape shape = this.m_currTextBox.Shape;
    if (!this.m_currTextBox.TextBoxFormat.AutoFit)
      return;
    shape.TextFrame.ShapeAutoFit = true;
    shape.TextFrame.NoAutoFit = false;
    shape.TextFrame.NormalAutoFit = false;
  }

  private void CopyParagraphFormatting(
    WParagraphFormat sourceParaFormat,
    WParagraphFormat destParaFormat)
  {
    destParaFormat.ImportContainer((FormatBase) sourceParaFormat);
    destParaFormat.CopyFormat((FormatBase) sourceParaFormat);
    if (!this.m_bIsLinespacingRule)
      return;
    destParaFormat.SetPropertyValue(52, (object) sourceParaFormat.LineSpacing);
    destParaFormat.LineSpacingRule = sourceParaFormat.LineSpacingRule;
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
        case RtfTableType.ListTable:
        case RtfTableType.ListOverrideTable:
          this.ParseListTable(this.m_token, tokenKey, tokenValue, tokenValue2);
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
          if (this.m_rtfCollectionStack.Count < 1)
          {
            this.m_rtfCollectionStack.Push("\\");
            break;
          }
          this.isNested = true;
          break;
        case "fonttbl":
          this.m_rtfFont = new RtfFont();
          this.m_currentTableType = RtfTableType.FontTable;
          this.m_lexer.CurrRtfTableType = RtfTableType.FontTable;
          this.m_stack.Push("{");
          break;
        case "stylesheet":
          this.IsStyleSheet = true;
          this.m_document.HasStyleSheets = true;
          this.m_currentTableType = RtfTableType.StyleSheet;
          this.m_lexer.CurrRtfTableType = RtfTableType.StyleSheet;
          this.m_document.DefCharFormat = new WCharacterFormat((IWordDocument) this.m_document);
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
          this.m_rtfColorTable = new RtfColor();
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
        case "deff":
        case "adeff":
          this.DefaultFontIndex = Convert.ToInt32(tokenValue);
          break;
        case "htmautsp":
          this.m_document.DOP.Dop2000.Copts.DontUseHTMLParagraphAutoSpacing = false;
          break;
        case "spltpgpar":
          this.m_document.Settings.CompatibilityOptions[CompatibilityOption.SplitPgBreakAndParaMark] = false;
          break;
        case "nobrkwrptbl":
          this.m_document.DOP.Dop2000.Copts.DontBreakWrappedTables = false;
          break;
        case "noextrasprl":
          this.m_document.DOP.Dop2000.Copts.Copts80.Copts60.NoSpaceRaiseLower = true;
          break;
        case "hyphauto":
          this.m_document.DOP.AutoHyphen = tokenValue == "1";
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
      this.m_token = this.m_lexer.ReadNextToken(this.m_previousTokenKey, this.m_bIsLevelText);
      if (this.m_token == "{")
        stringStack.Push("{");
      if (this.m_token == "}")
        stringStack.Pop();
    }
    this.m_token = "}";
  }

  private string ParseBulletChar(string token)
  {
    string bulletChar = string.Empty;
    string s = token.Replace("'", string.Empty);
    if (token.Length >= 3)
    {
      string oldValue = token.Substring(0, 3);
      switch (oldValue)
      {
        case "'00":
        case "'01":
        case "'02":
        case "'03":
        case "'04":
        case "'05":
        case "'06":
        case "'07":
        case "'08":
          bulletChar = s = token.Replace(oldValue, string.Empty).Replace("'", string.Empty);
          break;
      }
    }
    int result;
    if (int.TryParse(s, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out result))
      bulletChar = result != 149 ? ((char) result).ToString() : '•'.ToString();
    return bulletChar;
  }

  private void ParseListTable(
    string token,
    string tokenKey,
    string tokenValue,
    string tokenValue2)
  {
    if (this.m_bIsLevelText && (this.CurrListLevel.PatternType == ListPatternType.Arabic || this.CurrListLevel.PatternType == ListPatternType.LowLetter))
    {
      string str = "";
      if (this.StartsWithExt(this.m_previousToken, "leveltemplateid") || this.m_previousToken == "leveltext" && !this.StartsWithExt(token, "leveltemplateid"))
      {
        this.IsLevelTextLengthRead = true;
        this.CurrListLevel.NumberPrefix = string.Empty;
        this.CurrListLevel.NumberSuffix = string.Empty;
        if (token.Length > 3)
          this.CurrListLevel.NumberPrefix = token.Substring(3, token.Length - 3);
      }
      else if (this.IsLevelTextLengthRead)
      {
        if (this.m_isFirstPlaceHolderRead)
        {
          if (this.m_previousToken.Length > 3)
            str = this.m_previousToken.Substring(3, this.m_previousToken.Length - 3);
          if (this.StartsWithExt(token, "'01") && this.StartsWithExt(this.m_previousToken, "'00"))
          {
            WListLevel currListLevel = this.CurrListLevel;
            currListLevel.NumberPrefix = currListLevel.NumberPrefix + char.MinValue.ToString() + str;
          }
          else if (this.StartsWithExt(token, "'02") && this.StartsWithExt(this.m_previousToken, "'01"))
          {
            WListLevel currListLevel = this.CurrListLevel;
            currListLevel.NumberPrefix = currListLevel.NumberPrefix + '\u0001'.ToString() + str;
          }
          else if (this.StartsWithExt(token, "'03") && this.StartsWithExt(this.m_previousToken, "'02"))
          {
            WListLevel currListLevel = this.CurrListLevel;
            currListLevel.NumberPrefix = currListLevel.NumberPrefix + '\u0002'.ToString() + str;
          }
          else if (this.StartsWithExt(token, "'04") && this.StartsWithExt(this.m_previousToken, "'03"))
          {
            WListLevel currListLevel = this.CurrListLevel;
            currListLevel.NumberPrefix = currListLevel.NumberPrefix + '\u0003'.ToString() + str;
          }
          else if (this.StartsWithExt(token, "'05") && this.StartsWithExt(this.m_previousToken, "'04"))
          {
            WListLevel currListLevel = this.CurrListLevel;
            currListLevel.NumberPrefix = currListLevel.NumberPrefix + '\u0004'.ToString() + str;
          }
          else if (this.StartsWithExt(token, "'06") && this.StartsWithExt(this.m_previousToken, "'05"))
          {
            WListLevel currListLevel = this.CurrListLevel;
            currListLevel.NumberPrefix = currListLevel.NumberPrefix + '\u0005'.ToString() + str;
          }
          else if (this.StartsWithExt(token, "'07") && this.StartsWithExt(this.m_previousToken, "'06"))
          {
            WListLevel currListLevel = this.CurrListLevel;
            currListLevel.NumberPrefix = currListLevel.NumberPrefix + '\u0006'.ToString() + str;
          }
          else if (this.StartsWithExt(token, "'08") && this.StartsWithExt(this.m_previousToken, "'07"))
          {
            WListLevel currListLevel = this.CurrListLevel;
            currListLevel.NumberPrefix = currListLevel.NumberPrefix + '\a'.ToString() + str;
          }
        }
        this.m_isFirstPlaceHolderRead = true;
        this.CurrListLevel.NumberSuffix = token.Length <= 3 ? string.Empty : token.Substring(3, token.Length - 3);
      }
    }
    else if (this.m_bIsLevelText && this.CurrListLevel != null && this.CurrListLevel.PatternType == ListPatternType.Bullet)
    {
      string bulletChar = this.ParseBulletChar(token);
      if (!string.IsNullOrEmpty(bulletChar))
        this.CurrListLevel.BulletCharacter = bulletChar;
    }
    switch (tokenKey)
    {
      case "list":
        this.CurrListStyle = new ListStyle(this.m_document);
        string str1 = "ListStyle" + Guid.NewGuid().ToString();
        this.m_currStyleName = str1;
        this.CurrListStyle.Name = str1;
        this.m_currLevelIndex = -1;
        break;
      case "ls":
        if (this.m_currentTableType != RtfTableType.ListOverrideTable)
          break;
        this.m_listOverrideTable.Add(token, this.CurrListStyle.Name);
        break;
      case "listlevel":
        this.ParselistLevelStart();
        break;
      case "levelfollow":
        switch (Convert.ToInt32(tokenValue))
        {
          case 0:
            this.CurrListLevel.FollowCharacter = FollowCharacterType.Tab;
            return;
          case 1:
            this.CurrListLevel.FollowCharacter = FollowCharacterType.Space;
            return;
          case 2:
            return;
          case 3:
            this.CurrListLevel.FollowCharacter = FollowCharacterType.Nothing;
            return;
          default:
            return;
        }
      case "levelstartat":
        this.CurrListLevel.StartAt = Convert.ToInt32(tokenValue);
        break;
      case "levelnfcn":
      case "levelnfc":
        int int32 = Convert.ToInt32(tokenValue);
        if (int32 == 23)
        {
          if (this.CurrListLevel.LevelNumber == 0)
            this.CurrListStyle.ListType = ListType.Bulleted;
          this.CurrListLevel.PatternType = ListPatternType.Bullet;
          break;
        }
        if (this.CurrListLevel.LevelNumber == 0)
          this.CurrListStyle.ListType = ListType.Numbered;
        switch (int32)
        {
          case 0:
            this.CurrListLevel.PatternType = ListPatternType.Arabic;
            return;
          case 1:
            this.CurrListLevel.PatternType = ListPatternType.UpRoman;
            return;
          case 2:
            this.CurrListLevel.PatternType = ListPatternType.LowRoman;
            return;
          case 3:
            this.CurrListLevel.PatternType = ListPatternType.UpLetter;
            return;
          case 4:
            this.CurrListLevel.PatternType = ListPatternType.LowLetter;
            return;
          default:
            this.CurrListLevel.PatternType = ListPatternType.Arabic;
            return;
        }
      case "leveljc":
        switch (Convert.ToInt32(tokenValue))
        {
          case 0:
            this.CurrListLevel.NumberAlignment = ListNumberAlignment.Left;
            return;
          case 1:
            this.CurrListLevel.NumberAlignment = ListNumberAlignment.Center;
            return;
          case 2:
            this.CurrListLevel.NumberAlignment = ListNumberAlignment.Right;
            return;
          default:
            return;
        }
      case "levelnorestart":
        switch (Convert.ToInt32(tokenValue))
        {
          case 0:
            this.CurrListLevel.NoRestartByHigher = false;
            return;
          case 1:
            this.CurrListLevel.NoRestartByHigher = true;
            return;
          default:
            return;
        }
      case "lin-":
      case "li-":
        float twipsValue1 = this.ExtractTwipsValue(tokenValue);
        this.CurrListLevel.ParagraphFormat.SetPropertyValue(2, (object) (float) -(double) twipsValue1);
        this.CurrListLevel.TextPosition = -twipsValue1;
        break;
      case "lin":
      case "li":
        float twipsValue2 = this.ExtractTwipsValue(tokenValue);
        this.CurrListLevel.ParagraphFormat.SetPropertyValue(2, (object) twipsValue2);
        this.CurrListLevel.TextPosition = twipsValue2;
        break;
      case "fi":
        this.CurrListLevel.ParagraphFormat.SetPropertyValue(5, (object) this.ExtractTwipsValue(tokenValue));
        break;
      case "fi-":
        this.CurrListLevel.ParagraphFormat.SetPropertyValue(5, (object) (float) -(double) this.ExtractTwipsValue(tokenValue));
        break;
      case "listid-":
      case "listid":
        if (this.m_currentTableType == RtfTableType.ListTable)
        {
          this.m_listTable.Add(token, this.CurrListStyle);
          break;
        }
        if (this.m_currentTableType != RtfTableType.ListOverrideTable)
          break;
        this.m_currLevelIndex = -1;
        using (Dictionary<string, ListStyle>.Enumerator enumerator = this.m_listTable.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<string, ListStyle> current = enumerator.Current;
            if (current.Key == token)
            {
              string styleName = "LfoStyle_" + Guid.NewGuid().ToString();
              this.CurrListStyle = this.m_document.AddListStyle(current.Value.ListType, styleName);
              this.CopyListStyle(current.Value, this.CurrListStyle);
              this.CurrListStyle.Name = styleName;
            }
          }
          break;
        }
      case "levelold":
        this.CurrListLevel.Word6Legacy = tokenValue != "0";
        break;
      case "levelspace":
        float twipsValue3 = this.ExtractTwipsValue(tokenValue);
        if (!this.CurrListLevel.Word6Legacy)
          break;
        this.CurrListLevel.TabSpaceAfter = twipsValue3;
        break;
      case "tx":
        this.CurrListLevel.TabSpaceAfter = this.ExtractTwipsValue(tokenValue);
        break;
      case "f":
      case "af":
        using (Dictionary<string, RtfFont>.Enumerator enumerator = this.m_fontTable.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<string, RtfFont> current = enumerator.Current;
            if (this.SeperateToken(current.Key)[1] == tokenValue)
            {
              this.CurrRtfFont = current.Value;
              if (this.CurrListStyle.ListType == ListType.Bulleted)
              {
                if (this.m_previousTokenKey == "hich")
                  this.CurrListLevel.CharacterFormat.FontNameNonFarEast = this.CurrRtfFont.FontName;
                else if (this.m_previousTokenKey == "dbch")
                  this.CurrListLevel.CharacterFormat.FontNameFarEast = this.CurrRtfFont.FontName;
                else
                  this.CurrListLevel.CharacterFormat.FontName = this.CurrRtfFont.FontName;
              }
              else
                this.CurrListLevel.CharacterFormat.FontName = this.CurrRtfFont.FontName;
            }
          }
          break;
        }
      case "fs":
        if (tokenValue == null)
          break;
        this.CurrListLevel.CharacterFormat.SetPropertyValue(3, (object) (float) ((double) float.Parse(tokenValue, (IFormatProvider) CultureInfo.InvariantCulture) / 2.0));
        break;
      case "b":
        if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
        {
          this.CurrListLevel.CharacterFormat.Bold = false;
          this.m_currTextFormat.Bold = RtfParser.ThreeState.False;
          break;
        }
        this.CurrListLevel.CharacterFormat.Bold = true;
        this.m_currTextFormat.Bold = RtfParser.ThreeState.True;
        break;
      case "u-":
        this.CurrListLevel.BulletCharacter = ((char) (65536 /*0x010000*/ - Convert.ToInt32(tokenValue))).ToString();
        break;
      case "leveltext":
        this.m_bIsLevelText = true;
        break;
      case "u":
        this.CurrListLevel.BulletCharacter = ((char) Convert.ToInt32(tokenValue)).ToString();
        break;
      default:
        this.ParseFormattingToken(token, tokenKey, tokenValue, tokenValue2);
        break;
    }
  }

  private void ParselistLevelStart()
  {
    this.m_listLevelStack.Push("{");
    this.m_bIsListLevel = true;
    ++this.m_currLevelIndex;
    if (this.m_currentTableType == RtfTableType.ListTable)
    {
      this.CurrListLevel = new WListLevel(this.CurrListStyle);
      this.m_currLevelIndex = this.CurrListStyle.Levels.Add(this.CurrListLevel);
    }
    else if (this.m_currentTableType == RtfTableType.ListOverrideTable)
      this.CurrListLevel = this.CurrListStyle.Levels[this.m_currLevelIndex];
    this.m_bIsLevelText = false;
    this.IsLevelTextLengthRead = false;
    this.m_isFirstPlaceHolderRead = false;
    this.CurrentPara = (IWParagraph) new WParagraph((IWordDocument) this.m_document);
    this.m_currTextFormat = new RtfParser.TextFormat();
  }

  private void CopyCharacterFormatting(WCharacterFormat sourceFormat, WCharacterFormat destFormat)
  {
    if (sourceFormat.HasValue(3))
      destFormat.SetPropertyValue(3, (object) sourceFormat.FontSize);
    if (sourceFormat.HasValue(1))
      destFormat.TextColor = sourceFormat.TextColor;
    if (sourceFormat.HasValue((int) sbyte.MaxValue))
      destFormat.Scaling = sourceFormat.Scaling;
    if (sourceFormat.HasValue(2) && sourceFormat.FontName != "Times New Roman")
    {
      destFormat.FontName = sourceFormat.FontName;
      if (sourceFormat.FontName == "Monotype Corsiva" || sourceFormat.FontName == "Brush Script MT")
        destFormat.Italic = true;
    }
    if (sourceFormat.HasValue(68) && sourceFormat.FontNameAscii != "Times New Roman")
      destFormat.FontNameAscii = sourceFormat.FontNameAscii;
    if (sourceFormat.HasValue(61) && sourceFormat.FontNameBidi != "Times New Roman")
      destFormat.FontNameBidi = sourceFormat.FontNameBidi;
    if (sourceFormat.HasValue(69) && sourceFormat.FontNameFarEast != "Times New Roman")
      destFormat.FontNameFarEast = sourceFormat.FontNameFarEast;
    if (sourceFormat.HasValue(70) && sourceFormat.FontNameNonFarEast != "Times New Roman")
      destFormat.FontNameNonFarEast = sourceFormat.FontNameNonFarEast;
    if (sourceFormat.HasValue(4))
      destFormat.Bold = sourceFormat.Bold;
    if (sourceFormat.HasValue(5))
      destFormat.Italic = sourceFormat.Italic;
    if (sourceFormat.HasValue(7) && sourceFormat.UnderlineStyle != UnderlineStyle.None)
      destFormat.UnderlineStyle = sourceFormat.UnderlineStyle;
    if (sourceFormat.HasValue(63 /*0x3F*/))
      destFormat.HighlightColor = sourceFormat.HighlightColor;
    if (sourceFormat.HasValue(50))
      destFormat.Shadow = sourceFormat.Shadow;
    if (sourceFormat.HasValue(18))
      destFormat.SetPropertyValue(18, (object) sourceFormat.CharacterSpacing);
    if (sourceFormat.HasValue(14))
      destFormat.DoubleStrike = sourceFormat.DoubleStrike;
    if (sourceFormat.HasValue(51))
      destFormat.Emboss = sourceFormat.Emboss;
    if (sourceFormat.HasValue(52))
      destFormat.Engrave = sourceFormat.Engrave;
    if (sourceFormat.HasValue(10))
      destFormat.SubSuperScript = sourceFormat.SubSuperScript;
    if (sourceFormat.HasValue(9))
      destFormat.TextBackgroundColor = sourceFormat.TextBackgroundColor;
    if (sourceFormat.HasValue(77))
      destFormat.ForeColor = sourceFormat.ForeColor;
    if (sourceFormat.HasValue(54))
      destFormat.AllCaps = sourceFormat.AllCaps;
    if (sourceFormat.Bidi)
    {
      destFormat.Bidi = true;
      destFormat.FontNameBidi = sourceFormat.FontNameBidi;
      destFormat.SetPropertyValue(62, (object) sourceFormat.FontSizeBidi);
    }
    if (sourceFormat.HasValue(59))
      destFormat.BoldBidi = sourceFormat.BoldBidi;
    if (sourceFormat.HasValue(109))
      destFormat.FieldVanish = sourceFormat.FieldVanish;
    if (sourceFormat.HasValue(53))
      destFormat.Hidden = sourceFormat.Hidden;
    if (sourceFormat.HasValue(24))
      destFormat.SpecVanish = sourceFormat.SpecVanish;
    if (!sourceFormat.HasValue(55))
      return;
    destFormat.SmallCaps = sourceFormat.SmallCaps;
  }

  private void CopyListStyle(ListStyle sourceListStyle, ListStyle destListStyle)
  {
    destListStyle.ListType = sourceListStyle.ListType;
    destListStyle.Name = sourceListStyle.Name;
    for (int index = 0; index < sourceListStyle.Levels.Count; ++index)
    {
      destListStyle.Levels[index].ParagraphFormat.SetPropertyValue(2, (object) sourceListStyle.Levels[index].ParagraphFormat.LeftIndent);
      destListStyle.Levels[index].ParagraphFormat.SetPropertyValue(5, (object) sourceListStyle.Levels[index].ParagraphFormat.FirstLineIndent);
      WCharacterFormat characterFormat1 = destListStyle.Levels[index].CharacterFormat;
      WCharacterFormat characterFormat2 = sourceListStyle.Levels[index].CharacterFormat;
      if (characterFormat2.HasValue(2))
        characterFormat1.FontName = characterFormat2.FontName;
      if (characterFormat2.HasValue(68))
        characterFormat1.FontNameAscii = characterFormat2.FontNameAscii;
      if (characterFormat2.HasValue(61))
        characterFormat1.FontNameBidi = characterFormat2.FontNameBidi;
      if (characterFormat2.HasValue(69))
        characterFormat1.FontNameFarEast = characterFormat2.FontNameFarEast;
      if (characterFormat2.HasValue(70))
        characterFormat1.FontNameNonFarEast = characterFormat2.FontNameNonFarEast;
      if (characterFormat2.HasValue(3))
        characterFormat1.SetPropertyValue(3, (object) characterFormat2.FontSize);
      if (characterFormat2.HasValue(4))
        characterFormat1.Bold = characterFormat2.Bold;
      if (sourceListStyle.Levels[index].BulletCharacter != null)
        destListStyle.Levels[index].BulletCharacter = sourceListStyle.Levels[index].BulletCharacter;
      destListStyle.Levels[index].FollowCharacter = sourceListStyle.Levels[index].FollowCharacter;
      if (sourceListStyle.Levels[index].NoLevelText)
        destListStyle.Levels[index].NoLevelText = sourceListStyle.Levels[index].NoLevelText;
      if (sourceListStyle.Levels[index].NoRestartByHigher)
        destListStyle.Levels[index].NoRestartByHigher = sourceListStyle.Levels[index].NoRestartByHigher;
      destListStyle.Levels[index].NumberAlignment = sourceListStyle.Levels[index].NumberAlignment;
      destListStyle.Levels[index].NumberPosition = sourceListStyle.Levels[index].NumberPosition;
      if (sourceListStyle.Levels[index].NumberPrefix != null)
        destListStyle.Levels[index].NumberPrefix = sourceListStyle.Levels[index].NumberPrefix;
      if (sourceListStyle.Levels[index].NumberSuffix != null)
        destListStyle.Levels[index].NumberSuffix = sourceListStyle.Levels[index].NumberSuffix;
      destListStyle.Levels[index].PatternType = sourceListStyle.Levels[index].PatternType;
      destListStyle.Levels[index].StartAt = sourceListStyle.Levels[index].StartAt;
      destListStyle.Levels[index].TabSpaceAfter = sourceListStyle.Levels[index].TabSpaceAfter;
      destListStyle.Levels[index].TextPosition = sourceListStyle.Levels[index].TextPosition;
    }
  }

  private void ParsePageNumberingToken(string token, string tokenKey, string tokenValue)
  {
    switch (tokenKey)
    {
      case "pgnstarts":
        this.CurrentSection.PageSetup.PageStartingNumber = Convert.ToInt32(tokenValue);
        break;
      case "pgnrestart":
        this.CurrentSection.PageSetup.RestartPageNumbering = true;
        this.CurrentSection.PageSetup.PageStartingNumber = 1;
        break;
      case "pgndec":
        this.CurrentSection.PageSetup.PageNumberStyle = PageNumberStyle.Arabic;
        break;
      case "pgnucrm":
        this.CurrentSection.PageSetup.PageNumberStyle = PageNumberStyle.RomanUpper;
        break;
      case "pgnlcrm":
        this.CurrentSection.PageSetup.PageNumberStyle = PageNumberStyle.RomanLower;
        break;
      case "pgnucltr":
        this.CurrentSection.PageSetup.PageNumberStyle = PageNumberStyle.LetterUpper;
        break;
      case "pgnlcltr":
        this.CurrentSection.PageSetup.PageNumberStyle = PageNumberStyle.LetterLower;
        break;
      default:
        this.CurrentSection.PageSetup.PageNumberStyle = PageNumberStyle.Arabic;
        break;
    }
  }

  private void ParseLineNumberingToken(string token, string tokenKey, string tokenValue)
  {
    switch (tokenKey)
    {
      case "linex":
        this.CurrentSection.PageSetup.SetPageSetupProperty("LineNumberingDistanceFromText", (object) Convert.ToSingle(tokenValue));
        break;
      case "linestarts":
        this.CurrentSection.PageSetup.SetPageSetupProperty("LineNumberingStartValue", (object) Convert.ToInt32(tokenValue));
        break;
      case "lineppage":
        this.CurrentSection.PageSetup.SetPageSetupProperty("LineNumberingMode", (object) LineNumberingMode.RestartPage);
        break;
      case "linecont":
        this.CurrentSection.PageSetup.SetPageSetupProperty("LineNumberingMode", (object) LineNumberingMode.Continuous);
        break;
      case "linerestart":
        this.CurrentSection.PageSetup.SetPageSetupProperty("LineNumberingMode", (object) LineNumberingMode.RestartSection);
        break;
      case "linemod":
        this.CurrentSection.PageSetup.SetPageSetupProperty("LineNumberingStep", (object) Convert.ToInt32(tokenValue));
        break;
      case "line":
        this.CopyTextFormatToCharFormat(this.CurrentPara.AppendBreak(BreakType.LineBreak).CharacterFormat, this.m_currTextFormat);
        break;
    }
  }

  private void ParseFontTable(string token, string tokenKey, string tokenValue)
  {
    switch (tokenKey)
    {
      case "f":
      case "af":
        if (this.m_rtfFont != null && this.m_rtfFont.FontID != null && !this.m_fontTable.ContainsKey(this.m_rtfFont.FontID))
          this.AddFontTableEntry();
        this.m_rtfFont = new RtfFont();
        this.m_rtfFont.FontID = token;
        this.m_rtfFont.FontNumber = Convert.ToInt32(tokenValue);
        return;
      case "fcharset":
        if (this.m_rtfFont != null)
        {
          this.m_rtfFont.FontCharSet = Convert.ToInt16(tokenValue);
          return;
        }
        break;
    }
    if (!this.StartsWithExt(token, "'"))
      return;
    this.ParseSpecialCharacters(token);
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

  private void ResetListFontName(WCharacterFormat listCharFormat)
  {
    listCharFormat.FontName = "Times New Roman";
    listCharFormat.FontNameAscii = "Times New Roman";
    listCharFormat.FontNameBidi = "Times New Roman";
    listCharFormat.FontNameFarEast = "Times New Roman";
    listCharFormat.FontNameNonFarEast = "Times New Roman";
  }

  private bool IsParaInShapeResult(string token) => token != "par" || this.m_bIsShapeResult;

  private void ParseFormattingToken(
    string token,
    string tokenKey,
    string tokenValue,
    string tokenValue2)
  {
    if (this.m_bIsPictureOrShape && !this.m_bIsShapePicture)
      this.ParsePictureToken(token, tokenKey, tokenValue);
    if (this.m_bIsPictureOrShape && this.m_bIsShapePicture && this.IsParaInShapeResult(token))
    {
      if (token.Contains("pic") || this.StartsWithExt(token, "bin"))
        this.ParsePictureToken(token, tokenKey, tokenValue);
      else
        this.ParseShapeToken(token, tokenKey, tokenValue);
    }
    else if (tokenKey == "bin" && !this.m_bIsPictureOrShape && !this.m_bIsShapePicture && !string.IsNullOrEmpty(tokenValue))
      this.m_rtfReader.Position += int.Parse(tokenValue);
    else if (this.StartsWithExt(token, "line"))
      this.ParseLineNumberingToken(token, tokenKey, tokenValue);
    else if (this.StartsWithExt(token, "chpgn"))
    {
      WField wfield = this.m_currParagraph.AppendField("", FieldType.FieldPage) as WField;
      this.CopyTextFormatToCharFormat(wfield.CharacterFormat, this.m_currTextFormat);
      if (!(wfield.NextSibling is WTextRange))
        return;
      (wfield.NextSibling as WTextRange).ApplyCharacterFormat(wfield.CharacterFormat);
    }
    else if (this.StartsWithExt(token, "pgn"))
      this.ParsePageNumberingToken(token, tokenKey, tokenValue);
    else if (this.StartsWithExt(token, "brdr"))
      this.ParseParagraphBorders(token, tokenKey, tokenValue);
    else if (this.StartsWithExt(token, "vert"))
      this.ParsePageVerticalAlignment(token, tokenKey, tokenValue);
    else if (this.m_bIsCustomProperties)
    {
      if (!this.StartsWithExt(this.m_token, "proptype"))
        return;
      this.m_currPropertyType = (Syncfusion.CompoundFile.DocIO.PropertyType) Convert.ToInt64(tokenValue);
    }
    else if (this.m_previousTokenKey == "pntxtb" && this.CurrentPara != null && this.CurrentPara.ListFormat != null && this.CurrentPara.ListFormat.CurrentListLevel != null)
    {
      string bulletChar = this.ParseBulletChar(token);
      if (string.IsNullOrEmpty(bulletChar))
        return;
      this.CurrentPara.ListFormat.CurrentListLevel.BulletCharacter = bulletChar;
    }
    else
    {
      switch (tokenKey)
      {
        case "s":
          if (this.m_currentTableType == RtfTableType.StyleSheet)
          {
            this.m_currStyleID = this.m_token;
            this.m_currStyle = (IWParagraphStyle) new WParagraphStyle((IWordDocument) this.m_document);
            this.m_currParagraph = (IWParagraph) new WParagraph((IWordDocument) this.m_document);
            this.m_currParagraphFormat = new WParagraphFormat((IWordDocument) this.m_document);
            this.m_currTextFormat = new RtfParser.TextFormat();
            this.m_styleName = string.Empty;
            break;
          }
          using (Dictionary<string, IWParagraphStyle>.Enumerator enumerator = this.m_styleTable.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<string, IWParagraphStyle> current = enumerator.Current;
              if (current.Key == this.m_token)
              {
                if (!this.m_bIsListText)
                {
                  this.ResetParagraphFormat(this.m_currParagraphFormat);
                  this.ResetCharacterFormat(this.CurrentPara.BreakCharacterFormat);
                }
                (this.CurrentPara as WParagraph).ApplyStyle(current.Value.Name, false);
              }
            }
            break;
          }
        case "cs":
          if (this.m_currentTableType == RtfTableType.StyleSheet)
          {
            this.m_currStyleID = this.m_token;
            this.m_currCharStyle = new WCharacterStyle(this.m_document);
            this.m_currTextFormat = new RtfParser.TextFormat();
            this.m_styleName = string.Empty;
            break;
          }
          using (Dictionary<string, WCharacterStyle>.Enumerator enumerator = this.m_charStyleTable.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<string, WCharacterStyle> current = enumerator.Current;
              if (current.Key == this.m_token)
                this.m_currTextFormat.CharacterStyleName = current.Value.Name;
            }
            break;
          }
        case "cols":
          float equalColumnWidth = this.GetEqualColumnWidth(Convert.ToInt32(tokenValue));
          for (int index = 0; index < Convert.ToInt32(tokenValue); ++index)
            (this.CurrentSection as WSection).AddColumn(equalColumnWidth, 36f, true);
          if (this.CurrentSection.Columns.Count <= 0)
            break;
          this.CurrColumn = this.CurrentSection.Columns[0];
          break;
        case "colno":
          if (this.CurrentSection.Columns.Count < Convert.ToInt32(tokenValue))
            break;
          this.CurrColumn = this.CurrentSection.Columns[Convert.ToInt32(tokenValue) - 1];
          break;
        case "colsx":
        case "colsr":
          this.CurrColumn.Space = this.ExtractTwipsValue(tokenValue);
          break;
        case "colw":
          this.CurrentSection.PageSetup.EqualColumnWidth = false;
          this.CurrColumn.Width = this.ExtractTwipsValue(tokenValue);
          break;
        case "viewkind":
          switch (Convert.ToInt32(tokenValue))
          {
            case 0:
              this.m_document.ViewSetup.DocumentViewType = DocumentViewType.None;
              return;
            case 1:
              this.m_document.ViewSetup.DocumentViewType = DocumentViewType.NormalLayout;
              return;
            case 2:
              this.m_document.ViewSetup.DocumentViewType = DocumentViewType.OutlineLayout;
              return;
            case 3:
              this.m_document.ViewSetup.DocumentViewType = DocumentViewType.PrintLayout;
              return;
            case 4:
              this.m_document.ViewSetup.DocumentViewType = DocumentViewType.WebLayout;
              return;
            case 5:
              this.m_document.ViewSetup.DocumentViewType = DocumentViewType.OutlineLayout;
              return;
            default:
              return;
          }
        case "viewscale":
          this.m_document.ViewSetup.SetZoomPercentValue(Convert.ToInt32(tokenValue));
          break;
        case "viewzk":
          switch (Convert.ToInt32(tokenValue))
          {
            case 0:
              this.m_document.ViewSetup.ZoomType = ZoomType.None;
              return;
            case 1:
              this.m_document.ViewSetup.ZoomType = ZoomType.FullPage;
              return;
            case 2:
              this.m_document.ViewSetup.ZoomType = ZoomType.PageWidth;
              return;
            case 3:
              this.m_document.ViewSetup.ZoomType = ZoomType.TextFit;
              return;
            default:
              return;
          }
        case "viewbksp":
          this.m_document.Settings.DisplayBackgrounds = Convert.ToInt32(tokenValue) == 1;
          break;
        case "facingp":
          this.m_secFormat.DifferentOddAndEvenPage = true;
          break;
        case "lndscpsxn":
          this.m_secFormat.m_pageOrientation = PageOrientation.Landscape;
          break;
        case "titlepg":
          this.CurrentSection.PageSetup.DifferentFirstPage = true;
          this.CurrentSection.HeadersFooters.LinkToPrevious = true;
          break;
        case "header":
        case "headerr":
          this.m_bIsHeader = true;
          this.m_headerFooterStack.Push("{");
          this.m_textBody = (WTextBody) this.CurrentSection.HeadersFooters.OddHeader;
          this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_1, 22);
          this.m_textBody.Items.Clear();
          break;
        case "headerl":
          this.m_bIsHeader = true;
          this.m_headerFooterStack.Push("{");
          this.m_textBody = (WTextBody) this.CurrentSection.HeadersFooters.EvenHeader;
          this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_1, 22);
          this.m_textBody.Items.Clear();
          break;
        case "headerf":
          this.m_bIsHeader = true;
          this.m_headerFooterStack.Push("{");
          this.m_textBody = (WTextBody) this.CurrentSection.HeadersFooters.FirstPageHeader;
          this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_1, 22);
          this.m_textBody.Items.Clear();
          break;
        case "footerl":
          if (this.m_currTable == null)
          {
            this.m_bIsFooter = true;
            this.m_headerFooterStack.Push("{");
            this.m_textBody = (WTextBody) this.CurrentSection.HeadersFooters.EvenFooter;
            this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_1, 20);
            this.m_textBody.Items.Clear();
            break;
          }
          this.SkipGroup();
          if (!(this.m_token == "}"))
            break;
          this.m_tokenType = RtfTokenType.GroupEnd;
          this.ParseGroupEnd();
          break;
        case "footerf":
          if (this.m_currTable == null)
          {
            this.m_bIsFooter = true;
            this.m_headerFooterStack.Push("{");
            this.m_textBody = (WTextBody) this.CurrentSection.HeadersFooters.FirstPageFooter;
            this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_1, 20);
            this.m_textBody.Items.Clear();
            break;
          }
          this.SkipGroup();
          if (!(this.m_token == "}"))
            break;
          this.m_tokenType = RtfTokenType.GroupEnd;
          this.ParseGroupEnd();
          break;
        case "footer":
        case "footerr":
          if (this.m_currTable == null)
          {
            this.m_bIsFooter = true;
            this.m_headerFooterStack.Push("{");
            this.m_textBody = (WTextBody) this.CurrentSection.HeadersFooters.OddFooter;
            this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_1, 20);
            this.m_textBody.Items.Clear();
            break;
          }
          this.SkipGroup();
          if (!(this.m_token == "}"))
            break;
          this.m_tokenType = RtfTokenType.GroupEnd;
          this.ParseGroupEnd();
          break;
        case "atrfstart":
          if (tokenValue == null)
            break;
          string commentId1 = tokenValue;
          WCommentMark wcommentMark1 = new WCommentMark(this.m_document, commentId1);
          wcommentMark1.Type = CommentMarkType.CommentStart;
          wcommentMark1.CommentId = commentId1;
          this.CommentStartIdList.Add(commentId1);
          if (this.CurrentPara != null)
            this.CurrentPara.Items.Add((IEntity) wcommentMark1);
          this.m_isCommentRangeStart = true;
          this.commentstack.Push(wcommentMark1);
          break;
        case "atrfend":
          if (tokenValue == null)
            break;
          string commentId2 = tokenValue;
          if (this.CurrentPara != null && this.CommentLinkText != string.Empty && this.m_textFormatStack.Count > 0)
            this.m_currTextFormat = this.m_textFormatStack.Pop();
          this.CommentLinkText = string.Empty;
          if (this.CommentStartIdList != null && this.CommentStartIdList.Contains(commentId2))
          {
            WCommentMark wcommentMark2 = new WCommentMark(this.m_document, commentId2, CommentMarkType.CommentEnd);
            wcommentMark2.CommentId = commentId2;
            if (this.CurrentPara != null)
              this.CurrentPara.Items.Add((IEntity) wcommentMark2);
            this.commentstack.Push(wcommentMark2);
          }
          this.m_isCommentRangeStart = false;
          break;
        case "atnid":
          if (this.CurrentComment == null)
            this.CurrentComment = new WComment((IWordDocument) this.m_document);
          if (tokenValue == null)
            break;
          string str1 = tokenValue;
          if (str1 == null)
            break;
          this.CurrentComment.Format.UserInitials = str1;
          break;
        case "atnauthor":
          if (tokenValue == null)
            break;
          if (this.CurrentComment == null)
            this.CurrentComment = new WComment((IWordDocument) this.m_document);
          string str2 = tokenValue;
          if (str2 == null)
            break;
          this.CurrentComment.Format.User = str2;
          break;
        case "annotation":
          if (tokenValue != null)
          {
            if (this.CurrentComment == null)
              this.CurrentComment = new WComment((IWordDocument) this.m_document);
            this.CurrentComment.Format.TagBkmk = tokenValue;
            this.m_textBody.Items.Add((IEntity) this.CurrentPara);
            this.m_document.SetTriggerElement(ref this.m_document.m_notSupportedElementFlag, 4);
            this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_1, 10);
            this.m_isCommentOwnerParaIsCell = this.m_textBody == this.m_currCell;
            this.m_textBody = this.CurrentComment.TextBody;
            this.CurrentPara = (IWParagraph) new WParagraph((IWordDocument) this.m_document);
            if (this.commentstack.Count > 0)
            {
              WCommentMark wcommentMark3 = this.commentstack.Peek();
              if (wcommentMark3.Type == CommentMarkType.CommentEnd)
              {
                this.CurrentComment.CommentRangeEnd = wcommentMark3;
                wcommentMark3.Comment = this.CurrentComment;
                this.commentstack.Pop();
              }
            }
            if (this.commentstack.Count > 0)
            {
              WCommentMark wcommentMark4 = this.commentstack.Peek();
              if (wcommentMark4.Type == CommentMarkType.CommentStart)
              {
                this.CurrentComment.CommentRangeStart = wcommentMark4;
                wcommentMark4.Comment = this.CurrentComment;
                this.commentstack.Pop();
              }
            }
          }
          this.m_isCommentReference = true;
          break;
        case "atnref":
          if (tokenValue == null)
            break;
          this.CurrentComment.Format.TagBkmk = tokenValue;
          this.m_isCommentReference = true;
          this.CommentLinkText = string.Empty;
          break;
        case "atnparent":
          break;
        case "pard":
          this.isPardTagpresent = true;
          if (this.m_bIsListText)
            break;
          this.ParseParagraphStart();
          this.m_isPnNextList = true;
          break;
        case "phmrg":
          this.m_currParagraphFormat.FrameHorizontalPos = (byte) 1;
          break;
        case "phcol":
          this.m_currParagraphFormat.FrameHorizontalPos = (byte) 0;
          break;
        case "phpg":
          this.m_currParagraphFormat.FrameHorizontalPos = (byte) 2;
          break;
        case "pvmrg":
          this.m_currParagraphFormat.FrameVerticalPos = (byte) 0;
          break;
        case "pvpg":
          this.m_currParagraphFormat.FrameVerticalPos = (byte) 1;
          break;
        case "pvpara":
          this.m_currParagraphFormat.FrameVerticalPos = (byte) 2;
          break;
        case "absw":
          if (tokenValue == null)
            break;
          this.m_currParagraphFormat.FrameWidth = this.ExtractTwipsValue(tokenValue);
          break;
        case "absh-":
          if (tokenValue == null)
            break;
          this.m_currParagraphFormat.FrameHeight = this.ExtractTwipsValue(tokenValue);
          break;
        case "absh":
          if (tokenValue == null)
            break;
          float num1 = float.Parse(tokenValue, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture);
          if ((double) num1 == 0.0)
            break;
          this.m_currParagraphFormat.FrameHeight = (float) ((int) (short) num1 | 32768 /*0x8000*/) / 20f;
          break;
        case "posnegx-":
          if (tokenValue == null)
            break;
          this.m_currParagraphFormat.FrameX = this.SetFramePositions(tokenValue, true, true);
          break;
        case "posx":
          if (tokenValue == null)
            break;
          this.m_currParagraphFormat.FrameX = this.SetFramePositions(tokenValue, false, true);
          break;
        case "posxc":
          this.m_currParagraphFormat.FrameX = -4f;
          break;
        case "posxi":
          this.m_currParagraphFormat.FrameX = -12f;
          break;
        case "posxo":
          this.m_currParagraphFormat.FrameX = -16f;
          break;
        case "posxr":
          this.m_currParagraphFormat.FrameX = -8f;
          break;
        case "posxl":
          this.m_currParagraphFormat.FrameX = 0.0f;
          break;
        case "posnegy-":
          if (tokenValue == null)
            break;
          this.m_currParagraphFormat.FrameY = this.SetFramePositions(tokenValue, true, false);
          break;
        case "posy":
          if (tokenValue == null)
            break;
          this.m_currParagraphFormat.FrameY = this.SetFramePositions(tokenValue, false, false);
          break;
        case "posyil":
          this.m_currParagraphFormat.FrameY = 0.0f;
          break;
        case "posyt":
          this.m_currParagraphFormat.FrameY = -4f;
          break;
        case "posyc":
          this.m_currParagraphFormat.FrameY = -8f;
          break;
        case "posyb":
          this.m_currParagraphFormat.FrameY = -12f;
          break;
        case "posyin":
          this.m_currParagraphFormat.FrameY = -16f;
          break;
        case "posyout":
          this.m_currParagraphFormat.FrameY = -20f;
          break;
        case "dfrmtxtx":
          if (tokenValue == null)
            break;
          this.m_currParagraphFormat.FrameHorizontalDistanceFromText = this.ExtractTwipsValue(tokenValue);
          break;
        case "dfrmtxty":
          if (tokenValue == null)
            break;
          this.m_currParagraphFormat.FrameVerticalDistanceFromText = this.ExtractTwipsValue(tokenValue);
          break;
        case "nowrap":
          this.m_currParagraphFormat.WrapFrameAround = FrameWrapMode.None;
          break;
        case "wrapdefault":
          this.m_currParagraphFormat.WrapFrameAround = FrameWrapMode.Auto;
          break;
        case "wraptight":
          this.m_currParagraphFormat.WrapFrameAround = FrameWrapMode.Tight;
          break;
        case "wrapthrough":
          this.m_currParagraphFormat.WrapFrameAround = FrameWrapMode.Through;
          break;
        case "wraparound":
          this.m_currParagraphFormat.WrapFrameAround = FrameWrapMode.Around;
          break;
        case "plain":
          this.isPlainTagPresent = true;
          this.m_currTextFormat = new RtfParser.TextFormat();
          if (this.m_bIsListText)
            break;
          this.m_textFormatStack.Push(this.m_currTextFormat);
          break;
        case "par":
          if (this.IsFieldGroup && this.m_currentFieldGroupData != string.Empty)
          {
            this.ParseFieldGroupData(this.m_currentFieldGroupData);
            this.m_currentFieldGroupData = string.Empty;
          }
          this.ParseParagraphEnd();
          this.isPlainTagPresent = false;
          this.isPardTagpresent = false;
          break;
        case "rtlch":
          this.m_currTextFormat.Bidi = RtfParser.ThreeState.True;
          break;
        case "ltrch":
          this.m_currTextFormat.Bidi = RtfParser.ThreeState.False;
          break;
        case "expnd":
        case "expndtw":
          if (tokenValue == null)
            break;
          this.m_currTextFormat.CharacterSpacing = tokenKey == "expnd" ? this.ExtractQuaterPointsValue(tokenValue) : this.ExtractTwipsValue(tokenValue);
          break;
        case "expnd-":
        case "expndtw-":
          if (tokenValue == null)
            break;
          this.m_currTextFormat.CharacterSpacing = tokenKey == "expnd-" ? -this.ExtractQuaterPointsValue(tokenValue) : -this.ExtractTwipsValue(tokenValue);
          break;
        case "shad":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.Shadow = false;
            break;
          }
          this.m_currTextFormat.Shadow = true;
          break;
        case "sa":
          float num2 = 0.0f;
          if (tokenValue != null)
            num2 = this.ExtractTwipsValue(tokenValue);
          this.m_currParagraphFormat.SetPropertyValue(9, (object) num2);
          break;
        case "saauto":
          if (Convert.ToInt32(tokenValue) != 1)
            break;
          this.m_currParagraphFormat.SpaceAfterAuto = true;
          break;
        case "sb":
          this.m_currParagraphFormat.SetPropertyValue(8, (object) this.ExtractTwipsValue(tokenValue));
          break;
        case "sbauto":
          if (Convert.ToInt32(tokenValue) != 1)
            break;
          this.m_currParagraphFormat.SpaceBeforeAuto = true;
          break;
        case "fs":
          float num3 = 0.0f;
          if (tokenValue != null)
            num3 = float.Parse(tokenValue, (IFormatProvider) CultureInfo.InvariantCulture) / 2f;
          this.m_currTextFormat.FontSize = num3;
          if (tokenValue2 == null)
            break;
          this.m_token = tokenValue2;
          this.ParseDocumentElement(tokenValue2);
          break;
        case "sl":
          this.m_bIsLinespacingRule = true;
          float twipsValue1 = this.ExtractTwipsValue(tokenValue);
          if ((double) twipsValue1 != 0.0)
          {
            this.m_currParagraphFormat.SetPropertyValue(52, (object) twipsValue1);
            this.m_currParagraphFormat.LineSpacingRule = LineSpacingRule.AtLeast;
            break;
          }
          this.m_currParagraphFormat.SetPropertyValue(52, (object) 12f);
          this.m_currParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
          break;
        case "sl-":
          this.m_bIsLinespacingRule = true;
          float twipsValue2 = this.ExtractTwipsValue(tokenValue);
          if ((double) twipsValue2 == 0.0)
            break;
          this.m_currParagraphFormat.SetPropertyValue(52, (object) (float) -(double) twipsValue2);
          this.m_currParagraphFormat.LineSpacingRule = LineSpacingRule.Exactly;
          break;
        case "slmult":
          this.m_bIsLinespacingRule = true;
          int int32_1 = Convert.ToInt32(tokenValue);
          if ((double) this.m_currParagraphFormat.LineSpacing < 0.0)
          {
            this.m_currParagraphFormat.LineSpacingRule = LineSpacingRule.Exactly;
            this.m_currParagraphFormat.SetPropertyValue(52, (object) (float) -(double) this.m_currParagraphFormat.LineSpacing);
            break;
          }
          if (int32_1 == 1)
          {
            this.m_currParagraphFormat.LineSpacingRule = LineSpacingRule.Multiple;
            break;
          }
          if (int32_1 != 0)
            break;
          this.m_currParagraphFormat.LineSpacingRule = LineSpacingRule.AtLeast;
          break;
        case "f":
          foreach (KeyValuePair<string, RtfFont> keyValuePair in this.m_fontTable)
          {
            if (tokenValue2 != null)
              token = tokenKey + tokenValue;
            if (keyValuePair.Key == token)
            {
              this.CurrRtfFont = keyValuePair.Value;
              this.m_currTextFormat.FontFamily = this.CurrRtfFont.FontName.Trim();
            }
          }
          if (tokenValue2 == null)
            break;
          this.m_token = tokenValue2;
          this.ParseDocumentElement(tokenValue2);
          break;
        case "cf":
          if (Convert.ToInt64(tokenValue) == 0L)
          {
            this.m_currTextFormat.FontColor = Color.Black;
          }
          else
          {
            long int64 = Convert.ToInt64(tokenValue);
            this.CurrColorTable = new RtfColor();
            foreach (KeyValuePair<int, RtfColor> keyValuePair in this.m_colorTable)
            {
              if ((long) keyValuePair.Key == int64)
                this.CurrColorTable = keyValuePair.Value;
            }
            this.ApplyColorTable(this.CurrColorTable);
          }
          if (tokenValue2 == null)
            break;
          this.m_token = tokenValue2;
          this.ParseDocumentElement(tokenValue2);
          break;
        case "u":
          if (this.m_unicodeCountStack.Count > 0)
            this.m_unicodeCount = this.m_unicodeCountStack.Peek();
          string m_token1 = ((char) Convert.ToInt32(tokenValue)).ToString();
          this.isSpecialCharacter = true;
          this.ParseDocumentElement(m_token1);
          break;
        case "bullet":
          this.tr = this.CurrentPara.AppendText('•'.ToString());
          this.CopyTextFormatToCharFormat(this.tr.CharacterFormat, this.m_currTextFormat);
          break;
        case "u*":
          Encoding encoding = this.GetEncoding();
          byte[] bytes = BitConverter.GetBytes(Convert.ToInt16(tokenValue));
          string m_token2 = encoding.GetString(bytes, 0, bytes.Length).Replace("\0", "");
          this.isSpecialCharacter = true;
          this.ParseDocumentElement(m_token2);
          break;
        case "u-":
          if (this.m_unicodeCountStack.Count > 0)
            this.m_unicodeCount = this.m_unicodeCountStack.Peek();
          string m_token3 = ((char) (65536 /*0x010000*/ - Convert.ToInt32(tokenValue))).ToString();
          this.isSpecialCharacter = true;
          this.ParseDocumentElement(m_token3);
          break;
        case "uc":
          this.m_unicodeCountStack.Push(Convert.ToInt32(tokenValue));
          break;
        case "ql":
          this.m_currParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
          break;
        case "qc":
          this.m_currParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
          break;
        case "qr":
          this.m_currParagraphFormat.HorizontalAlignment = HorizontalAlignment.Right;
          break;
        case "qj":
          this.m_currParagraphFormat.HorizontalAlignment = HorizontalAlignment.Justify;
          break;
        case "qd":
          this.m_currParagraphFormat.HorizontalAlignment = HorizontalAlignment.Distribute;
          break;
        case "qk":
          switch (Convert.ToByte(tokenValue))
          {
            case 0:
              this.m_currParagraphFormat.HorizontalAlignment = HorizontalAlignment.JustifyLow;
              return;
            case 10:
              this.m_currParagraphFormat.HorizontalAlignment = HorizontalAlignment.JustifyMedium;
              return;
            default:
              this.m_currParagraphFormat.HorizontalAlignment = HorizontalAlignment.JustifyHigh;
              return;
          }
        case "qt":
          this.m_currParagraphFormat.HorizontalAlignment = HorizontalAlignment.ThaiJustify;
          break;
        case "tx":
          if (this.m_bIsListLevel || this.m_bIsListText)
            break;
          this.CurrTabFormat.TabPosition = this.ExtractTwipsValue(tokenValue);
          this.m_tabCollection.Add(this.m_tabCollection.Count + 1, this.m_currTabFormat);
          this.CurrTabFormat = new TabFormat();
          break;
        case "noline":
          this.m_currParagraphFormat.SuppressLineNumbers = true;
          break;
        case "tqc":
          if (this.m_bIsListText || this.m_bIsListLevel)
            break;
          this.CurrTabFormat.TabJustification = TabJustification.Centered;
          break;
        case "tqr":
          if (this.m_bIsListText || this.m_bIsListLevel)
            break;
          this.CurrTabFormat.TabJustification = TabJustification.Right;
          break;
        case "tqdec":
          if (this.m_bIsListText || this.m_bIsListLevel)
            break;
          this.CurrTabFormat.TabJustification = TabJustification.Decimal;
          break;
        case "tb":
          if (this.m_bIsListText || this.m_bIsListLevel)
            break;
          this.CurrTabFormat.TabJustification = TabJustification.Bar;
          this.CurrTabFormat.TabPosition = this.ExtractTwipsValue(tokenValue);
          break;
        case "tlmdot":
        case "tldot":
          if (this.m_bIsListText || this.m_bIsListLevel)
            break;
          this.CurrTabFormat.TabLeader = TabLeader.Dotted;
          break;
        case "tleq":
        case "tlhyph":
          if (this.m_bIsListText || this.m_bIsListLevel)
            break;
          this.CurrTabFormat.TabLeader = TabLeader.Hyphenated;
          break;
        case "tlul":
          if (this.m_bIsListText || this.m_bIsListLevel)
            break;
          this.CurrTabFormat.TabLeader = TabLeader.Single;
          break;
        case "tlth":
          if (this.m_bIsListText || this.m_bIsListLevel)
            break;
          this.CurrTabFormat.TabLeader = TabLeader.Heavy;
          break;
        case "tab":
          if (this.IsFieldGroup && this.m_currentFieldGroupData != string.Empty)
          {
            this.ParseFieldGroupData(this.m_currentFieldGroupData);
            this.m_currentFieldGroupData = string.Empty;
          }
          if (this.m_currentTableType == RtfTableType.StyleSheet || this.m_bIsListText || this.m_bIsListLevel)
            break;
          ++this.m_tabCount;
          if (this.m_tabCollection.Count == 0 || this.m_tabCount > this.m_tabCollection.Count)
          {
            this.tr = this.CurrentPara.AppendText(ControlChar.Tab);
            this.CopyTextFormatToCharFormat(this.tr.CharacterFormat, this.m_currTextFormat);
            break;
          }
          this.SortTabCollection();
          using (Dictionary<int, TabFormat>.Enumerator enumerator = this.m_tabCollection.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<int, TabFormat> current = enumerator.Current;
              if (current.Key == this.m_tabCount)
              {
                this.CurrentPara.ParagraphFormat.Tabs.AddTab(current.Value.TabPosition, current.Value.TabJustification, current.Value.TabLeader);
                this.tr = this.CurrentPara.AppendText(ControlChar.Tab);
                this.CopyTextFormatToCharFormat(this.tr.CharacterFormat, this.m_currTextFormat);
              }
            }
            break;
          }
        case "fi-":
          this.m_currParagraphFormat.SetPropertyValue(5, (object) (float) -(double) this.ExtractTwipsValue(tokenValue));
          break;
        case "fi":
          this.m_currParagraphFormat.SetPropertyValue(5, (object) this.ExtractTwipsValue(tokenValue));
          break;
        case "cufi":
          break;
        case "li":
          float twipsValue3 = this.ExtractTwipsValue(tokenValue);
          if (this.m_currParagraphFormat.Bidi)
          {
            this.m_currParagraphFormat.SetPropertyValue(3, (object) twipsValue3);
            break;
          }
          this.m_currParagraphFormat.SetPropertyValue(2, (object) twipsValue3);
          break;
        case "li-":
          this.m_currParagraphFormat.SetPropertyValue(2, (object) (float) -(double) this.ExtractTwipsValue(tokenValue));
          break;
        case "culi":
          break;
        case "ri":
          float twipsValue4 = this.ExtractTwipsValue(tokenValue);
          if (this.m_currParagraphFormat.Bidi)
          {
            this.m_currParagraphFormat.SetPropertyValue(2, (object) twipsValue4);
            break;
          }
          this.m_currParagraphFormat.SetPropertyValue(3, (object) twipsValue4);
          break;
        case "ri-":
          this.m_currParagraphFormat.SetPropertyValue(3, (object) (float) -(double) this.ExtractTwipsValue(tokenValue));
          break;
        case "curi":
          break;
        case "indmirror":
          this.m_currParagraphFormat.MirrorIndents = true;
          break;
        case "hyphpar":
          if (this.GetIntValue(tokenValue) != 0)
            break;
          this.m_currParagraphFormat.SuppressAutoHyphens = true;
          break;
        case "keep":
          this.m_currParagraphFormat.Keep = true;
          break;
        case "keepn":
          this.m_currParagraphFormat.KeepFollow = true;
          break;
        case "outlinelevel":
        case "level":
          this.ParseOutLineLevel(token, tokenKey, tokenValue);
          break;
        case "pagebb":
          this.m_currParagraphFormat.PageBreakBefore = true;
          break;
        case "contextualspace":
          this.m_currParagraphFormat.ContextualSpacing = true;
          break;
        case "widctlpar":
          this.m_currParagraphFormat.WidowControl = true;
          break;
        case "nowidctlpar":
          this.m_currParagraphFormat.WidowControl = false;
          break;
        case "nowwrap":
          this.m_currParagraphFormat.WordWrap = false;
          break;
        case "page":
          this.CurrentPara.AppendBreak(BreakType.PageBreak);
          break;
        case "column":
          this.CurrentPara.AppendBreak(BreakType.ColumnBreak);
          break;
        case "shading":
          this.m_currParagraphFormat.TextureStyle = this.GetTextureStyle(Convert.ToInt32(tokenValue));
          break;
        case "bgcross":
          this.m_currParagraphFormat.TextureStyle = TextureStyle.TextureCross;
          break;
        case "bgdkcross":
          this.m_currParagraphFormat.TextureStyle = TextureStyle.TextureDarkCross;
          break;
        case "bgdkdcross":
          this.m_currParagraphFormat.TextureStyle = TextureStyle.TextureDarkDiagonalCross;
          break;
        case "bgdkbdiag":
          this.m_currParagraphFormat.TextureStyle = TextureStyle.TextureDarkDiagonalDown;
          break;
        case "bgdkfdiag":
          this.m_currParagraphFormat.TextureStyle = TextureStyle.TextureDarkDiagonalUp;
          break;
        case "bgdkhoriz":
          this.m_currParagraphFormat.TextureStyle = TextureStyle.TextureDarkHorizontal;
          break;
        case "bgdkvert":
          this.m_currParagraphFormat.TextureStyle = TextureStyle.TextureDarkVertical;
          break;
        case "bgdcross":
          this.m_currParagraphFormat.TextureStyle = TextureStyle.TextureDiagonalCross;
          break;
        case "bgbdiag":
          this.m_currParagraphFormat.TextureStyle = TextureStyle.TextureDiagonalDown;
          break;
        case "bgfdiag":
          this.m_currParagraphFormat.TextureStyle = TextureStyle.TextureDiagonalUp;
          break;
        case "bghoriz":
          this.m_currParagraphFormat.TextureStyle = TextureStyle.TextureHorizontal;
          break;
        case "bgvert":
          this.m_currParagraphFormat.TextureStyle = TextureStyle.TextureVertical;
          break;
        case "caps":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.AllCaps = RtfParser.ThreeState.False;
            break;
          }
          this.m_currTextFormat.AllCaps = RtfParser.ThreeState.True;
          break;
        case "scaps":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.SmallCaps = RtfParser.ThreeState.False;
            break;
          }
          this.m_currTextFormat.SmallCaps = RtfParser.ThreeState.True;
          break;
        case "charscalex":
          if (tokenValue == null)
            break;
          this.m_currTextFormat.Scaling = (float) Convert.ToInt32(tokenValue);
          break;
        case "b":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.Bold = RtfParser.ThreeState.False;
            break;
          }
          this.m_currTextFormat.Bold = RtfParser.ThreeState.True;
          break;
        case "i":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.Italic = RtfParser.ThreeState.False;
            break;
          }
          this.m_currTextFormat.Italic = RtfParser.ThreeState.True;
          break;
        case "ul":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.Underline = RtfParser.ThreeState.False;
            this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
            break;
          }
          this.m_currTextFormat.Underline = RtfParser.ThreeState.True;
          this.m_currTextFormat.m_underlineStyle = UnderlineStyle.Single;
          break;
        case "ulnone":
          this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
          this.m_currTextFormat.Underline = RtfParser.ThreeState.False;
          break;
        case "uldb":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
            break;
          }
          this.m_currTextFormat.m_underlineStyle = UnderlineStyle.Double;
          break;
        case "uld":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
            break;
          }
          this.m_currTextFormat.m_underlineStyle = UnderlineStyle.Dotted;
          break;
        case "uldash":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
            break;
          }
          this.m_currTextFormat.m_underlineStyle = UnderlineStyle.Dash;
          break;
        case "uldashd":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
            break;
          }
          this.m_currTextFormat.m_underlineStyle = UnderlineStyle.DotDash;
          break;
        case "uldashdd":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
            break;
          }
          this.m_currTextFormat.m_underlineStyle = UnderlineStyle.DotDotDash;
          break;
        case "ulwave":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
            break;
          }
          this.m_currTextFormat.m_underlineStyle = UnderlineStyle.Wavy;
          break;
        case "ulhwave":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
            break;
          }
          this.m_currTextFormat.m_underlineStyle = UnderlineStyle.WavyHeavy;
          break;
        case "ulldash":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
            break;
          }
          this.m_currTextFormat.m_underlineStyle = UnderlineStyle.DashLong;
          break;
        case "ulth":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
            break;
          }
          this.m_currTextFormat.m_underlineStyle = UnderlineStyle.Thick;
          break;
        case "ulthd":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
            break;
          }
          this.m_currTextFormat.m_underlineStyle = UnderlineStyle.DottedHeavy;
          break;
        case "ululdbwave":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
            break;
          }
          this.m_currTextFormat.m_underlineStyle = UnderlineStyle.WavyDouble;
          break;
        case "ulw":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
            break;
          }
          this.m_currTextFormat.m_underlineStyle = UnderlineStyle.Words;
          break;
        case "ulthldash":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
            break;
          }
          this.m_currTextFormat.m_underlineStyle = UnderlineStyle.DashLongHeavy;
          break;
        case "ulthdashdd":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
            break;
          }
          this.m_currTextFormat.m_underlineStyle = UnderlineStyle.DotDotDashHeavy;
          break;
        case "ulthdashd":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
            break;
          }
          this.m_currTextFormat.m_underlineStyle = UnderlineStyle.DotDashHeavy;
          break;
        case "ulthdash":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
            break;
          }
          this.m_currTextFormat.m_underlineStyle = UnderlineStyle.DashHeavy;
          break;
        case "sub":
          this.m_currTextFormat.m_subSuperScript = SubSuperScript.SubScript;
          break;
        case "super":
          this.m_currTextFormat.m_subSuperScript = SubSuperScript.SuperScript;
          break;
        case "nosupersub":
          this.m_currTextFormat.m_subSuperScript = SubSuperScript.None;
          break;
        case "up":
          if (tokenValue != null)
          {
            this.m_currTextFormat.Position = (float) Convert.ToInt32(tokenValue) / 2f;
            break;
          }
          this.m_currTextFormat.Position = 3f;
          break;
        case "lang":
          if (tokenValue == null)
            break;
          this.m_currTextFormat.LocalIdASCII = Convert.ToInt16(tokenValue);
          break;
        case "langfenp":
          if (tokenValue == null)
            break;
          this.m_currTextFormat.LocalIdForEast = Convert.ToInt16(tokenValue);
          break;
        case "langfe":
          if (tokenValue == null)
            break;
          this.m_currTextFormat.LidBi = Convert.ToInt16(tokenValue);
          break;
        case "dn":
          if (tokenValue != null)
          {
            this.m_currTextFormat.Position = (float) -((double) Convert.ToInt32(tokenValue) / 2.0);
            break;
          }
          this.m_currTextFormat.Position = -3f;
          break;
        case "strike":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.Strike = RtfParser.ThreeState.False;
            break;
          }
          this.m_currTextFormat.Strike = RtfParser.ThreeState.True;
          break;
        case "striked":
          switch (Convert.ToInt32(tokenValue))
          {
            case 0:
              this.m_currTextFormat.DoubleStrike = RtfParser.ThreeState.False;
              return;
            case 1:
              this.m_currTextFormat.DoubleStrike = RtfParser.ThreeState.True;
              return;
            default:
              return;
          }
        case "embo":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.Emboss = RtfParser.ThreeState.False;
            break;
          }
          this.m_currTextFormat.Emboss = RtfParser.ThreeState.True;
          break;
        case "impr":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.Engrave = RtfParser.ThreeState.False;
            break;
          }
          this.m_currTextFormat.Engrave = RtfParser.ThreeState.True;
          break;
        case "sectd":
          this.ParseSectionStart();
          this.IsSectionBreak = true;
          break;
        case "sbknone":
          this.CurrentSection.BreakCode = SectionBreakCode.NoBreak;
          this.IsSectionBreak = true;
          break;
        case "sbkcol":
          this.CurrentSection.BreakCode = SectionBreakCode.NewColumn;
          this.IsSectionBreak = true;
          break;
        case "sbkpage":
          this.CurrentSection.BreakCode = SectionBreakCode.NewPage;
          this.IsSectionBreak = true;
          break;
        case "sbkeven":
          this.CurrentSection.BreakCode = SectionBreakCode.EvenPage;
          this.IsSectionBreak = true;
          break;
        case "sbkodd":
          this.CurrentSection.BreakCode = SectionBreakCode.Oddpage;
          this.IsSectionBreak = true;
          break;
        case "sect":
          this.ResetParagraphFormat(this.m_currParagraphFormat);
          this.ResetCharacterFormat(this.CurrentPara.BreakCharacterFormat);
          this.CopyParagraphFormatting(this.m_currParagraphFormat, this.CurrentPara.ParagraphFormat);
          this.ProcessTableInfo(false);
          this.AddNewParagraph(this.CurrentPara);
          this.AddNewSection(this.CurrentSection);
          this.ApplySectionFormatting();
          this.m_currSection = (IWSection) new WSection((IWordDocument) this.m_document);
          this.m_textBody = this.m_currSection.Body;
          if (this.m_paragraphFormatStack.Count > 0)
          {
            this.m_paragraphFormatStack.Clear();
            this.m_currParagraphFormat = new WParagraphFormat((IWordDocument) this.m_document);
            this.m_paragraphFormatStack.Push(this.m_currParagraphFormat);
          }
          this.isPlainTagPresent = false;
          this.isPardTagpresent = false;
          break;
        case "binfsxn":
          this.m_secFormat.FirstPageTray = (int) Math.Floor((double) float.Parse(tokenValue, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case "binsxn":
          this.m_secFormat.OtherPagesTray = (int) Math.Floor((double) float.Parse(tokenValue, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case "marglsxn":
        case "margl":
          this.m_secFormat.LeftMargin = this.ExtractTwipsValue(tokenValue);
          break;
        case "margtsxn":
        case "margt":
          this.m_secFormat.TopMargin = this.ExtractTwipsValue(tokenValue);
          break;
        case "margrsxn":
        case "margr":
          this.m_secFormat.RightMargin = this.ExtractTwipsValue(tokenValue);
          break;
        case "margbsxn":
        case "margb":
          this.m_secFormat.BottomMargin = this.ExtractTwipsValue(tokenValue);
          break;
        case "gutter":
          this.CurrentSection.PageSetup.Margins.Gutter = this.ExtractTwipsValue(tokenValue);
          break;
        case "gutterprl":
          this.m_document.DOP.GutterAtTop = true;
          break;
        case "margmirror":
          this.m_document.MultiplePage = MultiplePage.MirrorMargins;
          break;
        case "bookfold":
          this.m_document.MultiplePage = MultiplePage.BookFold;
          break;
        case "bookfoldrev":
          this.m_document.MultiplePage = MultiplePage.ReverseBookFold;
          break;
        case "twoonone":
          this.m_document.MultiplePage = MultiplePage.TwoPagesPerSheet;
          break;
        case "bookfoldsheets":
          this.m_document.SheetsPerBooklet = (int) this.ExtractTwipsValue(tokenValue);
          break;
        case "pgwsxn":
        case "paperw":
          this.m_secFormat.pageSize.Width = this.ExtractTwipsValue(tokenValue);
          break;
        case "pghsxn":
        case "paperh":
          this.m_secFormat.pageSize.Height = this.ExtractTwipsValue(tokenValue);
          break;
        case "headery":
          this.m_secFormat.HeaderDistance = this.ExtractTwipsValue(tokenValue);
          break;
        case "footery":
          this.m_secFormat.FooterDistance = this.ExtractTwipsValue(tokenValue);
          break;
        case "deftab":
          this.m_secFormat.DefaultTabWidth = this.ExtractTwipsValue(tokenValue);
          break;
        case "lquote":
          char ch1 = '‘';
          if (this.m_prevTokenType == RtfTokenType.Text && this.tr != null)
          {
            this.tr.Text += ch1.ToString();
            break;
          }
          this.tr = this.CurrentPara.AppendText(ch1.ToString());
          this.CopyTextFormatToCharFormat(this.tr.CharacterFormat, this.m_currTextFormat);
          break;
        case "rquote":
          char ch2 = '’';
          if (this.m_prevTokenType == RtfTokenType.Text && this.tr != null && !this.IsFieldGroup)
          {
            this.tr.Text += ch2.ToString();
            break;
          }
          if (this.IsFieldGroup)
          {
            this.m_currentFieldGroupData += ch2.ToString();
            break;
          }
          this.tr = this.CurrentPara.AppendText(ch2.ToString());
          this.CopyTextFormatToCharFormat(this.tr.CharacterFormat, this.m_currTextFormat);
          break;
        case "rdblquote":
        case "ldblquote":
          this.tr = this.CurrentPara.AppendText("\"");
          this.CopyTextFormatToCharFormat(this.tr.CharacterFormat, this.m_currTextFormat);
          break;
        case "endash":
          char ch3 = '–';
          if (this.m_prevTokenType == RtfTokenType.Text && this.tr != null)
          {
            this.tr.Text += ch3.ToString();
            break;
          }
          this.tr = this.CurrentPara.AppendText(ch3.ToString());
          this.CopyTextFormatToCharFormat(this.tr.CharacterFormat, this.m_currTextFormat);
          break;
        case "emdash":
          char ch4 = '—';
          if (this.m_prevTokenType == RtfTokenType.Text && this.tr != null)
          {
            this.tr.Text += ch4.ToString();
            break;
          }
          this.tr = this.CurrentPara.AppendText(ch4.ToString());
          this.CopyTextFormatToCharFormat(this.tr.CharacterFormat, this.m_currTextFormat);
          break;
        case "cb":
        case "chcbpat":
          int int32_2 = Convert.ToInt32(tokenValue);
          this.CurrColorTable = new RtfColor();
          foreach (KeyValuePair<int, RtfColor> keyValuePair in this.m_colorTable)
          {
            if (keyValuePair.Key == int32_2)
            {
              this.CurrColorTable = keyValuePair.Value;
              this.m_currTextFormat.BackColor = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
            }
          }
          if (tokenValue2 == null)
            break;
          this.m_token = tokenValue2;
          this.ParseDocumentElement(tokenValue2);
          break;
        case "chcfpat":
          int int32_3 = Convert.ToInt32(tokenValue);
          this.CurrColorTable = new RtfColor();
          foreach (KeyValuePair<int, RtfColor> keyValuePair in this.m_colorTable)
          {
            if (keyValuePair.Key == int32_3)
            {
              this.CurrColorTable = keyValuePair.Value;
              this.m_currTextFormat.ForeColor = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
            }
          }
          if (tokenValue2 == null)
            break;
          this.m_token = tokenValue2;
          this.ParseDocumentElement(tokenValue2);
          break;
        case "cbpat":
          int int32_4 = Convert.ToInt32(tokenValue);
          this.CurrColorTable = new RtfColor();
          using (Dictionary<int, RtfColor>.Enumerator enumerator = this.m_colorTable.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<int, RtfColor> current = enumerator.Current;
              if (current.Key == int32_4)
              {
                this.CurrColorTable = current.Value;
                this.m_currParagraphFormat.BackColor = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
              }
            }
            break;
          }
        case "cfpat":
          int int32_5 = Convert.ToInt32(tokenValue);
          this.CurrColorTable = new RtfColor();
          using (Dictionary<int, RtfColor>.Enumerator enumerator = this.m_colorTable.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<int, RtfColor> current = enumerator.Current;
              if (current.Key == int32_5)
              {
                this.CurrColorTable = current.Value;
                this.m_currParagraphFormat.ForeColor = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
              }
            }
            break;
          }
        case "highlight":
          int int32_6 = Convert.ToInt32(tokenValue);
          this.CurrColorTable = new RtfColor();
          if (this.m_colorTable.ContainsKey(int32_6))
          {
            this.CurrColorTable = this.m_colorTable[int32_6];
            this.m_currTextFormat.HighlightColor = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
            Color color = this.m_currTextFormat.HighlightColor;
            int argb1 = color.ToArgb();
            color = Color.Green;
            int argb2 = color.ToArgb();
            if (argb1 != argb2)
              break;
            this.m_currTextFormat.HighlightColor = Color.DarkGreen;
            break;
          }
          this.m_currTextFormat.HighlightColor = Color.Empty;
          break;
        case "nonshppict":
          this.m_bIsShapePicture = false;
          this.m_bIsShape = false;
          if (this.m_bIsPictureOrShape)
            break;
          this.m_pictureOrShapeStack.Push("{");
          this.m_bIsPictureOrShape = true;
          this.m_bIsShapePictureAdded = false;
          this.m_currPicture = (IWPicture) null;
          this.m_picFormat = new RtfParser.PictureFormat();
          break;
        case "shppict":
          this.m_bIsShapePicture = true;
          this.m_currShapeFormat = new RtfParser.ShapeFormat();
          bool flag = false;
          if (this.m_bIsPictureOrShape)
            break;
          if (this.m_pictureOrShapeStack.Count > 0)
          {
            this.AddOwnerShapeTextStack();
            flag = true;
          }
          this.m_pictureOrShapeStack.Push("{");
          this.m_bIsPictureOrShape = true;
          this.m_bIsShapePictureAdded = false;
          this.m_currPicture = (IWPicture) null;
          this.m_picFormat = new RtfParser.PictureFormat();
          if (this.m_currShape == null && this.m_currTextBox == null || flag || this.m_bIsFallBackImage)
            break;
          this.m_currShape = (Shape) null;
          this.m_currTextBox = (WTextBox) null;
          break;
        case "shp":
          this.m_pictureOrShapeStack.Push("{");
          this.m_bIsShape = true;
          this.m_bIsShapePictureAdded = false;
          this.m_bIsShapePicture = true;
          this.m_currShapeFormat = new RtfParser.ShapeFormat();
          this.m_currPicture = (IWPicture) null;
          this.m_bIsPictureOrShape = true;
          this.m_picFormat = new RtfParser.PictureFormat();
          this.m_currShape = (Shape) null;
          this.m_currTextBox = (WTextBox) null;
          break;
        case "pict":
          if (this.m_bIsPictureOrShape)
            break;
          this.m_bIsShapePicture = true;
          this.m_bIsShape = false;
          this.m_currShapeFormat = new RtfParser.ShapeFormat();
          this.m_currPicture = (IWPicture) null;
          this.m_pictureOrShapeStack.Push("{");
          this.m_bIsPictureOrShape = true;
          this.m_picFormat = new RtfParser.PictureFormat();
          this.m_bIsFallBackImage = false;
          break;
        case "shpgrp":
          this.m_bIsGroupShape = true;
          this.m_groupShapeStack.Push("{");
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
          this.ParseListTextStart();
          break;
        case "ls":
          if (!this.IsDestinationControlWord || this.m_destStack.Count == 2 && this.m_bIsShapeText && this.m_shapeTextStack.Count > 0)
          {
            this.ApplyListFormatting(token, tokenKey, tokenValue, this.CurrentPara.ListFormat);
            if (this.m_currentTableType == RtfTableType.None)
              this.m_bIsList = true;
          }
          this.m_bIsContinousList = false;
          break;
        case "ltrpar":
          this.m_currParagraphFormat.Bidi = false;
          break;
        case "rtlpar":
          this.m_currParagraphFormat.Bidi = true;
          break;
        case "lin":
          this.m_currParagraphFormat.SetPropertyValue(2, (object) this.ExtractTwipsValue(tokenValue));
          break;
        case "rin":
          this.m_currParagraphFormat.SetPropertyValue(3, (object) this.ExtractTwipsValue(tokenValue));
          break;
        case "ilvl":
          if (this.IsDestinationControlWord || Convert.ToInt32(tokenValue) >= 9 && Convert.ToInt32(tokenValue) <= 12)
            break;
          this.CurrentPara.ListFormat.ListLevelNumber = Convert.ToInt32(tokenValue);
          break;
        case "pn":
          this.m_pnLevelNumber = 0;
          break;
        case "pnlvlblt":
          if (this.m_currentTableType == RtfTableType.None)
            this.m_bIsList = true;
          this.CurrentPara.ListFormat.ApplyDefBulletStyle();
          this.CurrentPara.ListFormat.ListLevelNumber = this.m_pnLevelNumber;
          this.CurrentPara.ListFormat.CurrentListLevel.PatternType = ListPatternType.Bullet;
          this.CurrentPara.ListFormat.CurrentListLevel.Word6Legacy = true;
          this.ResetListFontName(this.CurrentPara.ListFormat.CurrentListLevel.CharacterFormat);
          this.m_bIsContinousList = false;
          break;
        case "pnf":
          using (Dictionary<string, RtfFont>.Enumerator enumerator = this.m_fontTable.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<string, RtfFont> current = enumerator.Current;
              if (current.Key == token)
              {
                this.CurrRtfFont = current.Value;
                this.CurrentPara.ListFormat.CurrentListLevel.CharacterFormat.FontName = this.CurrRtfFont.FontName;
              }
            }
            break;
          }
        case "pnlvlbody":
          if (this.m_document.ListStyles.Count > 0 && this.m_isPnNextList)
            this.m_isPnNextList = !this.IsPnListStyleDefinedExisting(this.groupOrder[0]);
          if (this.m_isPnNextList)
          {
            this.m_uniqueStyleID = Guid.NewGuid().ToString();
            this.m_isPnNextList = false;
            string styleName = "Numbered" + this.m_uniqueStyleID;
            ListStyle listStyle = new ListStyle((IWordDocument) this.m_document, ListType.Numbered);
            this.isPnStartUpdate = true;
            this.m_document.AddListStyle(listStyle.ListType, styleName);
          }
          if (this.m_currentTableType == RtfTableType.None)
            this.m_bIsList = true;
          if (this.m_bIsPreviousList && this.m_bIsContinousList || !this.m_bIsContinousList)
          {
            if (this.m_uniqueStyleID != null)
              this.CurrentPara.ListFormat.ApplyStyle("Numbered" + this.m_uniqueStyleID);
            else
              this.CurrentPara.ListFormat.ApplyDefNumberedStyle();
            this.CurrentPara.ListFormat.ListLevelNumber = this.m_pnLevelNumber;
            this.CurrentPara.ListFormat.CurrentListLevel.NumberSuffix = string.Empty;
            this.CurrentPara.ListFormat.CurrentListLevel.NumberAlignment = ListNumberAlignment.Left;
            this.CurrentPara.ListFormat.CurrentListLevel.Word6Legacy = true;
            this.m_bIsContinousList = false;
            break;
          }
          if (!this.m_bIsContinousList)
            break;
          this.CurrentPara.ListFormat.ContinueListNumbering();
          this.m_bIsContinousList = false;
          break;
        case "pnlvl":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.ListLevelNumber = Convert.ToInt32(tokenValue);
          break;
        case "pnstart":
          if (this.isPnStartUpdate && this.CurrentPara.ListFormat != null && this.CurrentPara.ListFormat.CurrentListLevel != null)
            this.CurrentPara.ListFormat.CurrentListLevel.StartAt = Convert.ToInt32(tokenValue);
          this.isPnStartUpdate = false;
          break;
        case "pndec":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.PatternType = ListPatternType.Arabic;
          break;
        case "pnlcrm":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.PatternType = ListPatternType.LowRoman;
          break;
        case "pnucrm":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.PatternType = ListPatternType.UpRoman;
          break;
        case "pnucltr":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.PatternType = ListPatternType.UpLetter;
          break;
        case "pnlcltr":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.PatternType = ListPatternType.LowLetter;
          break;
        case "pnord":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.PatternType = ListPatternType.Ordinal;
          break;
        case "pnordt":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.PatternType = ListPatternType.OrdinalText;
          break;
        case "pntxta.":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.NumberSuffix = ".";
          break;
        case "pnindent":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.ParagraphFormat.SetPropertyValue(2, (object) this.ExtractTwipsValue(tokenValue));
          this.CurrentPara.ListFormat.CurrentListLevel.TextPosition = this.ExtractTwipsValue(tokenValue);
          break;
        case "pnsp":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.TabSpaceAfter = this.ExtractTwipsValue(tokenValue);
          break;
        case "pnb*":
        case "pnb":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.CharacterFormat.Bold = true;
          break;
        case "pni*":
        case "pni":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.CharacterFormat.Italic = true;
          break;
        case "pncaps*":
        case "pncaps":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.CharacterFormat.AllCaps = true;
          break;
        case "pnul*":
        case "pnul":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.CharacterFormat.UnderlineStyle = UnderlineStyle.Single;
          break;
        case "pnuld*":
        case "pnuld":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.CharacterFormat.UnderlineStyle = UnderlineStyle.Dotted;
          break;
        case "pnuldash*":
        case "pnuldash":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.CharacterFormat.UnderlineStyle = UnderlineStyle.Dash;
          break;
        case "pnulwave*":
        case "pnulwave":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.CharacterFormat.UnderlineStyle = UnderlineStyle.Wavy;
          break;
        case "pnuldb*":
        case "pnuldb":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.CharacterFormat.UnderlineStyle = UnderlineStyle.Double;
          break;
        case "pnulth*":
        case "pnulth":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.CharacterFormat.UnderlineStyle = UnderlineStyle.Thick;
          break;
        case "pnulnone*":
        case "pnulnone":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.CharacterFormat.UnderlineStyle = UnderlineStyle.None;
          break;
        case "pnfs":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.CharacterFormat.SetPropertyValue(3, (object) (float) ((double) Convert.ToInt32(tokenValue) / 2.0));
          break;
        case "pnqc":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.NumberAlignment = ListNumberAlignment.Center;
          break;
        case "pnql":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.NumberAlignment = ListNumberAlignment.Left;
          break;
        case "pnqr":
          if (this.CurrentPara.ListFormat == null || this.CurrentPara.ListFormat.CurrentListLevel == null)
            break;
          this.CurrentPara.ListFormat.CurrentListLevel.NumberAlignment = ListNumberAlignment.Right;
          break;
        case "pnlvlcont":
          if (this.CurrentPara.ListFormat != null && this.CurrentPara.ListFormat.CurrentListLevel != null)
          {
            this.CurrentPara.ListFormat.ContinueListNumbering();
            this.CurrentPara.ListFormat.CurrentListLevel.NoLevelText = true;
          }
          if (!this.m_bIsPreviousList)
            break;
          this.m_bIsContinousList = true;
          break;
        case "brsp":
          float twipsValue5 = this.ExtractTwipsValue(tokenValue);
          if (this.m_bIsRow)
            break;
          if (this.m_bIsBorderBottom)
            this.m_currParagraphFormat.Borders.Bottom.Space = twipsValue5;
          if (this.m_bIsBorderLeft)
            this.m_currParagraphFormat.Borders.Left.Space = twipsValue5;
          if (this.m_bIsBorderTop)
            this.m_currParagraphFormat.Borders.Top.Space = twipsValue5;
          if (!this.m_bIsBorderRight)
            break;
          this.m_currParagraphFormat.Borders.Right.Space = twipsValue5;
          break;
        case "background":
          this.m_bIsBackgroundCollection = true;
          this.m_backgroundCollectionStack.Push("{");
          break;
        case "trowd":
          this.ParseRowStart(false);
          break;
        case "trql":
          this.CurrRowFormat.HorizontalAlignment = RowAlignment.Left;
          break;
        case "trqr":
          this.CurrRowFormat.HorizontalAlignment = RowAlignment.Right;
          break;
        case "trqc":
          this.CurrRowFormat.HorizontalAlignment = RowAlignment.Center;
          break;
        case "ltrrow":
          this.m_bIsRow = true;
          break;
        case "rtlrow":
          this.CurrRowFormat.Bidi = true;
          break;
        case "tsrowd":
          this.CurrRowFormat.Bidi = false;
          break;
        case "row":
        case "nestrow":
          this.ParseRowEnd(false);
          break;
        case "clmgf":
          this.CurrCellFormat.HorizontalMerge = CellMerge.Start;
          break;
        case "clmrg":
          this.CurrCellFormat.HorizontalMerge = CellMerge.Continue;
          break;
        case "cellx":
        case "cellx-":
          this.ParseCellBoundary(token, tokenKey, tokenValue);
          break;
        case "cell":
        case "nestcell":
          if (this.IsFieldGroup && this.m_currentFieldGroupData != string.Empty)
          {
            this.ParseFieldGroupData(this.m_currentFieldGroupData);
            this.m_currentFieldGroupData = string.Empty;
          }
          this.ResetParagraphFormat(this.m_currParagraphFormat);
          this.ResetCharacterFormat(this.CurrentPara.BreakCharacterFormat);
          this.CopyParagraphFormatting(this.m_currParagraphFormat, this.CurrentPara.ParagraphFormat);
          this.ProcessTableInfo(false);
          this.AddNewParagraph(this.CurrentPara);
          if (this.CurrCell != null)
            this.CopyTextFormatToCharFormat(this.CurrCell.CharacterFormat, this.m_currTextFormat);
          this.m_bCellFinished = true;
          this.m_currParagraph = (IWParagraph) new WParagraph((IWordDocument) this.m_document);
          break;
        case "intbl":
          this.m_bInTable = true;
          this.m_bIsRow = true;
          this.m_currentLevel = 1;
          if (!this.m_bIsShape || this.m_shapeTextStack.Count <= 0)
            break;
          this.m_currentLevel = (int) this.m_shapeTextbodyStack.Peek()["m_currentLevel"] + 2;
          break;
        case "itap":
          if (!this.m_bIsShape || this.m_shapeTextStack.Count <= 0)
          {
            this.m_currentLevel = Convert.ToInt32(tokenValue);
            break;
          }
          if (this.m_currentLevel > Convert.ToInt32(tokenValue))
            break;
          this.m_currentLevel = Convert.ToInt32(tokenValue) + 1;
          break;
        case "tdfrmtxtLeft":
          this.CurrRowFormat.Positioning.DistanceFromLeft = this.ExtractTwipsValue(tokenValue);
          break;
        case "tdfrmtxtRight":
          this.CurrRowFormat.Positioning.DistanceFromRight = this.ExtractTwipsValue(tokenValue);
          break;
        case "tdfrmtxtTop":
          this.CurrRowFormat.Positioning.DistanceFromTop = this.ExtractTwipsValue(tokenValue);
          break;
        case "tdfrmtxtBottom":
          this.CurrRowFormat.Positioning.DistanceFromBottom = this.ExtractTwipsValue(tokenValue);
          break;
        case "tphcol":
          this.CurrRowFormat.Positioning.HorizRelationTo = HorizontalRelation.Column;
          break;
        case "tphmrg":
          this.CurrRowFormat.Positioning.HorizRelationTo = HorizontalRelation.Margin;
          break;
        case "tphpg":
          this.CurrRowFormat.Positioning.HorizRelationTo = HorizontalRelation.Page;
          break;
        case "tposnegx-":
        case "tposx":
          float twipsValue6 = this.ExtractTwipsValue(tokenValue);
          this.CurrRowFormat.Positioning.HorizPosition = tokenKey == "tposx" ? twipsValue6 : -twipsValue6;
          break;
        case "tposxc":
          this.CurrRowFormat.Positioning.HorizPositionAbs = HorizontalPosition.Center;
          break;
        case "tposxi":
          this.CurrRowFormat.Positioning.HorizPositionAbs = HorizontalPosition.Inside;
          break;
        case "tposxl":
          this.CurrRowFormat.Positioning.HorizPositionAbs = HorizontalPosition.Left;
          break;
        case "tposxo":
          this.CurrRowFormat.Positioning.HorizPositionAbs = HorizontalPosition.Outside;
          break;
        case "tposxr":
          this.CurrRowFormat.Positioning.HorizPositionAbs = HorizontalPosition.Right;
          break;
        case "tposnegy-":
        case "tposy":
          float twipsValue7 = this.ExtractTwipsValue(tokenValue);
          this.CurrRowFormat.Positioning.VertPosition = tokenKey == "tposy" ? twipsValue7 : -twipsValue7;
          break;
        case "tposyb":
          this.CurrRowFormat.Positioning.VertPositionAbs = VerticalPosition.Bottom;
          break;
        case "tposyc":
          this.CurrRowFormat.Positioning.VertPositionAbs = VerticalPosition.Center;
          break;
        case "tposyin":
          this.CurrRowFormat.Positioning.VertPositionAbs = VerticalPosition.Inside;
          break;
        case "tposyout":
          this.CurrRowFormat.Positioning.VertPositionAbs = VerticalPosition.Outside;
          break;
        case "tposyt":
          this.CurrRowFormat.Positioning.VertPositionAbs = VerticalPosition.Top;
          break;
        case "tpvmrg":
          this.CurrRowFormat.Positioning.VertRelationTo = VerticalRelation.Margin;
          break;
        case "tpvpara":
          this.CurrRowFormat.Positioning.VertRelationTo = VerticalRelation.Paragraph;
          break;
        case "tpvpg":
          this.CurrRowFormat.Positioning.VertRelationTo = VerticalRelation.Page;
          break;
        case "taprtl":
          this.CurrRowFormat.Bidi = true;
          break;
        case "trhdr":
          this.CurrRowFormat.IsHeaderRow = true;
          break;
        case "trkeep":
          this.CurrRowFormat.IsBreakAcrossPages = false;
          break;
        case "trrh":
          this.CurrRowFormat.Height = this.ExtractTwipsValue(tokenValue);
          break;
        case "trrh-":
          this.CurrRowFormat.Height = -this.ExtractTwipsValue(tokenValue);
          break;
        case "trpaddb":
          this.CurrRowFormat.Paddings.Bottom = this.ExtractTwipsValue(tokenValue);
          break;
        case "trpaddl":
          this.CurrRowFormat.Paddings.Left = this.ExtractTwipsValue(tokenValue);
          break;
        case "trpaddr":
          this.CurrRowFormat.Paddings.Right = this.ExtractTwipsValue(tokenValue);
          break;
        case "trpaddt":
          this.CurrRowFormat.Paddings.Top = this.ExtractTwipsValue(tokenValue);
          break;
        case "trpaddfb":
          if (Convert.ToInt32(tokenValue) != 0)
            break;
          this.CurrRowFormat.Paddings.Bottom = 0.0f;
          break;
        case "trpaddft":
          if (Convert.ToInt32(tokenValue) != 0)
            break;
          this.CurrRowFormat.Paddings.Top = 0.0f;
          break;
        case "trpaddfr":
          if (Convert.ToInt32(tokenValue) != 0)
            break;
          this.CurrRowFormat.Paddings.Right = 0.0f;
          break;
        case "trpaddfl":
          if (Convert.ToInt32(tokenValue) != 0)
            break;
          this.CurrRowFormat.Paddings.Left = 0.0f;
          break;
        case "trspdb":
          this.m_bottomcellspace = this.ExtractTwipsValue(tokenValue);
          break;
        case "trspdl":
          this.m_leftcellspace = this.ExtractTwipsValue(tokenValue);
          break;
        case "trspdr":
          this.m_rightcellspace = this.ExtractTwipsValue(tokenValue);
          break;
        case "trspdt":
          this.m_topcellspace = this.ExtractTwipsValue(tokenValue);
          break;
        case "trspdfb":
          if (Convert.ToInt32(tokenValue) != 3)
            break;
          this.CurrRowFormat.CellSpacing = this.m_bottomcellspace;
          break;
        case "trspdft":
          if (Convert.ToInt32(tokenValue) != 3)
            break;
          this.CurrRowFormat.CellSpacing = this.m_topcellspace;
          break;
        case "trspdfl":
          if (Convert.ToInt32(tokenValue) != 3)
            break;
          this.CurrRowFormat.CellSpacing = this.m_leftcellspace;
          break;
        case "trspdfr":
          if (Convert.ToInt32(tokenValue) != 3)
            break;
          this.CurrRowFormat.CellSpacing = this.m_rightcellspace;
          break;
        case "tabsnoovrlp":
          if (!this.CurrRowFormat.WrapTextAround || Convert.ToInt32(tokenValue) != 1)
            break;
          this.CurrRowFormat.Positioning.AllowOverlap = false;
          break;
        case "trgaph":
          this.CurrRowFormat.Paddings.Left = this.CurrRowFormat.Paddings.Right = this.ExtractTwipsValue(tokenValue);
          this.m_bIsWord97StylePadding = true;
          break;
        case "trleft":
          this.m_currenttrleft = Convert.ToInt32(tokenValue);
          if (this.m_bIsWord97StylePadding)
          {
            float twipsValue8 = this.ExtractTwipsValue(tokenValue);
            this.CurrRowFormat.LeftIndent = this.CurrRowFormat.Paddings.Left + twipsValue8;
            this.m_currRowLeftIndent = (int) ((double) this.CurrRowFormat.LeftIndent * 20.0);
            this.CurrRowFormat.BeforeWidth = twipsValue8;
          }
          this.m_bIsWord97StylePadding = false;
          this.m_currRowFormat.IsLeftIndentDefined = true;
          break;
        case "trleft-":
          this.m_currenttrleft = -Convert.ToInt32(tokenValue);
          if (this.m_bIsWord97StylePadding)
          {
            this.CurrRowFormat.LeftIndent = this.CurrRowFormat.Paddings.Left + -this.ExtractTwipsValue(tokenValue);
            this.m_currRowLeftIndent = (int) ((double) this.CurrRowFormat.LeftIndent * 20.0);
          }
          this.m_bIsWord97StylePadding = false;
          break;
        case "tblind":
          float twipsValue9 = this.ExtractTwipsValue(tokenValue);
          this.m_currRowLeftIndent = Convert.ToInt32(tokenValue);
          this.CurrRowFormat.LeftIndent = twipsValue9;
          this.m_currRowFormat.IsLeftIndentDefined = true;
          break;
        case "tblind-":
          float twipsValue10 = this.ExtractTwipsValue(tokenValue);
          this.m_currRowLeftIndent = -Convert.ToInt32(tokenValue);
          this.CurrRowFormat.LeftIndent = -twipsValue10;
          this.m_currRowFormat.IsLeftIndentDefined = true;
          break;
        case "trautofit":
          if (!(tokenValue == "1"))
            break;
          this.CurrRowFormat.IsAutoResized = true;
          break;
        case "trftsWidth":
          this.CurrRowFormat.PreferredWidth.WidthType = (FtsWidth) Convert.ToInt32(tokenValue);
          if (this.CurrRowFormat.PreferredWidth.WidthType == FtsWidth.Auto)
            this.CurrRowFormat.PreferredWidth.Width = 0.0f;
          if (this.CurrRowFormat.PreferredWidth.WidthType == FtsWidth.Percentage)
            this.CurrRowFormat.PreferredWidth.Width = 0.0f;
          if (!(this.m_previousTokenKey == "trwWidth") || this.CurrRowFormat.PreferredWidth.WidthType != FtsWidth.Percentage)
            break;
          this.CurrRowFormat.PreferredWidth.Width = (float) this.GetIntValue(this.m_previousTokenValue) / 50f;
          break;
        case "trwWidth":
          if (this.CurrRowFormat.PreferredWidth.WidthType == FtsWidth.Percentage)
          {
            this.CurrRowFormat.PreferredWidth.Width = (float) this.GetIntValue(tokenValue) / 50f;
            break;
          }
          if (this.CurrRowFormat.PreferredWidth.WidthType != FtsWidth.Point)
            break;
          this.CurrRowFormat.PreferredWidth.Width = (float) this.GetIntValue(tokenValue) / 20f;
          break;
        case "trftsWidthB":
          this.CurrRowFormat.GridBeforeWidth.WidthType = (FtsWidth) Convert.ToInt32(tokenValue);
          break;
        case "trwWidthB":
          if (this.CurrRowFormat.GridBeforeWidth.WidthType == FtsWidth.Percentage)
          {
            this.CurrRowFormat.GridBeforeWidth.Width = (float) this.GetIntValue(tokenValue) / 50f;
            break;
          }
          if (this.CurrRowFormat.GridBeforeWidth.WidthType != FtsWidth.Point)
            break;
          this.CurrRowFormat.GridBeforeWidth.Width = (float) this.GetIntValue(tokenValue) / 20f;
          break;
        case "trftsWidthA":
          this.CurrRowFormat.GridAfterWidth.WidthType = (FtsWidth) Convert.ToInt32(tokenValue);
          break;
        case "trwWidthA":
          if (this.CurrRowFormat.GridAfterWidth.WidthType == FtsWidth.Percentage)
          {
            this.CurrRowFormat.GridAfterWidth.Width = (float) this.GetIntValue(tokenValue) / 50f;
            break;
          }
          if (this.CurrRowFormat.GridAfterWidth.WidthType != FtsWidth.Point)
            break;
          this.CurrRowFormat.GridAfterWidth.Width = (float) this.GetIntValue(tokenValue) / 20f;
          break;
        case "clFitText":
          this.CurrCellFormat.FitText = true;
          break;
        case "clNoWrap":
          this.CurrCellFormat.TextWrap = false;
          break;
        case "clpadt":
          this.CurrCellFormat.Paddings.Left = this.ExtractTwipsValue(tokenValue);
          break;
        case "clpadl":
          this.CurrCellFormat.Paddings.Top = this.ExtractTwipsValue(tokenValue);
          break;
        case "clpadb":
          this.CurrCellFormat.Paddings.Bottom = this.ExtractTwipsValue(tokenValue);
          break;
        case "clpadr":
          this.CurrCellFormat.Paddings.Right = this.ExtractTwipsValue(tokenValue);
          break;
        case "clpadfl":
          break;
        case "clpadft":
          break;
        case "clpadfb":
          break;
        case "clpadfr":
          break;
        case "clhidemark":
          this.CurrCellFormat.HideMark = true;
          break;
        case "clftsWidth":
          this.CurrCellFormat.PreferredWidth.WidthType = (FtsWidth) Convert.ToInt32(tokenValue);
          if (this.CurrCellFormat.PreferredWidth.WidthType != FtsWidth.Percentage || !(this.m_previousTokenKey == "clwWidth"))
            break;
          this.CurrCellFormat.PreferredWidth.Width = (float) this.GetIntValue(this.m_previousTokenValue) / 50f;
          break;
        case "clwWidth":
          if (Convert.ToInt32(this.m_previousTokenValue) == 3)
          {
            this.CurrCellFormat.PreferredWidth.Width = this.ExtractTwipsValue(tokenValue);
            this.CurrCellFormat.PreferredWidth.WidthType = FtsWidth.Point;
            break;
          }
          if (this.CurrCellFormat.PreferredWidth.WidthType != FtsWidth.Percentage)
            break;
          this.CurrCellFormat.PreferredWidth.Width = (float) this.GetIntValue(tokenValue) / 50f;
          break;
        case "clvertalt":
          this.CurrCellFormat.VerticalAlignment = VerticalAlignment.Top;
          break;
        case "clvertalc":
          this.CurrCellFormat.VerticalAlignment = VerticalAlignment.Middle;
          break;
        case "clvertalb":
          this.CurrCellFormat.VerticalAlignment = VerticalAlignment.Bottom;
          break;
        case "cltxlrtb":
          this.CurrCellFormat.TextDirection = TextDirection.Horizontal;
          break;
        case "cltxtbrl":
          this.CurrCellFormat.TextDirection = TextDirection.VerticalTopToBottom;
          break;
        case "cltxbtlr":
          this.CurrCellFormat.TextDirection = TextDirection.VerticalBottomToTop;
          break;
        case "cltxlrtbv":
          this.CurrCellFormat.TextDirection = TextDirection.HorizontalFarEast;
          break;
        case "cltxtbrlv":
          this.CurrCellFormat.TextDirection = TextDirection.VerticalFarEast;
          break;
        case "clbrdrt":
          this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.Cleared;
          this.m_bIsBorderTop = true;
          this.m_bIsBorderBottom = false;
          this.m_bIsBorderLeft = false;
          this.m_bIsBorderRight = false;
          this.m_bIsBorderDiagonalUp = false;
          this.m_bIsBorderDiagonalDown = false;
          this.m_bIsRowBorderBottom = false;
          this.m_bIsRowBorderLeft = false;
          this.m_bIsRowBorderRight = false;
          this.m_bIsRowBorderTop = false;
          break;
        case "clbrdrr":
          this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.Cleared;
          this.m_bIsBorderRight = true;
          this.m_bIsBorderTop = false;
          this.m_bIsBorderBottom = false;
          this.m_bIsBorderLeft = false;
          this.m_bIsBorderDiagonalUp = false;
          this.m_bIsBorderDiagonalDown = false;
          this.m_bIsRowBorderBottom = false;
          this.m_bIsRowBorderLeft = false;
          this.m_bIsRowBorderRight = false;
          this.m_bIsRowBorderTop = false;
          break;
        case "clbrdrl":
          this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.Cleared;
          this.m_bIsBorderLeft = true;
          this.m_bIsBorderRight = false;
          this.m_bIsBorderTop = false;
          this.m_bIsBorderBottom = false;
          this.m_bIsBorderDiagonalUp = false;
          this.m_bIsBorderDiagonalDown = false;
          this.m_bIsRowBorderBottom = false;
          this.m_bIsRowBorderLeft = false;
          this.m_bIsRowBorderRight = false;
          this.m_bIsRowBorderTop = false;
          break;
        case "clbrdrb":
          this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.Cleared;
          this.m_bIsBorderBottom = true;
          this.m_bIsBorderLeft = false;
          this.m_bIsBorderRight = false;
          this.m_bIsBorderTop = false;
          this.m_bIsBorderDiagonalDown = false;
          this.m_bIsBorderDiagonalUp = false;
          this.m_bIsRowBorderBottom = false;
          this.m_bIsRowBorderLeft = false;
          this.m_bIsRowBorderRight = false;
          this.m_bIsRowBorderTop = false;
          break;
        case "cldglu":
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.Cleared;
          this.m_bIsBorderDiagonalDown = true;
          this.m_bIsBorderDiagonalUp = false;
          this.m_bIsBorderLeft = false;
          this.m_bIsBorderRight = false;
          this.m_bIsBorderTop = false;
          this.m_bIsBorderBottom = false;
          this.m_bIsRowBorderBottom = false;
          this.m_bIsRowBorderLeft = false;
          this.m_bIsRowBorderRight = false;
          this.m_bIsRowBorderTop = false;
          break;
        case "cldgll":
          this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.Cleared;
          this.m_bIsBorderDiagonalUp = true;
          this.m_bIsBorderDiagonalDown = true;
          this.m_bIsBorderLeft = false;
          this.m_bIsBorderRight = false;
          this.m_bIsBorderTop = false;
          this.m_bIsBorderBottom = false;
          this.m_bIsRowBorderBottom = false;
          this.m_bIsRowBorderLeft = false;
          this.m_bIsRowBorderRight = false;
          this.m_bIsRowBorderTop = false;
          break;
        case "trbrdrt":
          this.m_bIsRowBorderTop = true;
          this.m_bIsRowBorderBottom = false;
          this.m_bIsRowBorderLeft = false;
          this.m_bIsRowBorderRight = false;
          this.m_bIsBorderBottom = false;
          this.m_bIsBorderLeft = false;
          this.m_bIsBorderRight = false;
          this.m_bIsBorderTop = false;
          this.m_bIsBorderDiagonalDown = false;
          this.m_bIsBorderDiagonalUp = false;
          break;
        case "trbrdrr":
          this.m_bIsRowBorderRight = true;
          this.m_bIsRowBorderTop = false;
          this.m_bIsRowBorderBottom = false;
          this.m_bIsRowBorderLeft = false;
          this.m_bIsBorderBottom = false;
          this.m_bIsBorderLeft = false;
          this.m_bIsBorderRight = false;
          this.m_bIsBorderTop = false;
          this.m_bIsBorderDiagonalDown = false;
          this.m_bIsBorderDiagonalUp = false;
          break;
        case "trbrdrl":
          this.m_bIsRowBorderLeft = true;
          this.m_bIsRowBorderRight = false;
          this.m_bIsRowBorderTop = false;
          this.m_bIsRowBorderBottom = false;
          this.m_bIsBorderBottom = false;
          this.m_bIsBorderLeft = false;
          this.m_bIsBorderRight = false;
          this.m_bIsBorderTop = false;
          this.m_bIsBorderDiagonalDown = false;
          this.m_bIsBorderDiagonalUp = false;
          break;
        case "trbrdrb":
          this.m_bIsRowBorderBottom = true;
          this.m_bIsRowBorderLeft = false;
          this.m_bIsRowBorderRight = false;
          this.m_bIsRowBorderTop = false;
          this.m_bIsBorderBottom = false;
          this.m_bIsBorderLeft = false;
          this.m_bIsBorderRight = false;
          this.m_bIsBorderTop = false;
          this.m_bIsBorderDiagonalDown = false;
          this.m_bIsBorderDiagonalUp = false;
          break;
        case "trbrdrh":
          this.m_bIsHorizontalBorder = true;
          this.m_bIsVerticalBorder = false;
          break;
        case "trbrdrv":
          this.m_bIsVerticalBorder = true;
          this.m_bIsHorizontalBorder = false;
          this.m_bIsRowBorderRight = false;
          break;
        case "clcbpat":
        case "clcbpatraw":
          int int32_7 = Convert.ToInt32(tokenValue);
          this.CurrColorTable = new RtfColor();
          using (Dictionary<int, RtfColor>.Enumerator enumerator = this.m_colorTable.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<int, RtfColor> current = enumerator.Current;
              if (current.Key == int32_7)
              {
                this.CurrColorTable = current.Value;
                this.CurrCellFormat.BackColor = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
              }
            }
            break;
          }
        case "clcfpat":
        case "clcfpatraw":
          int int32_8 = Convert.ToInt32(tokenValue);
          using (Dictionary<int, RtfColor>.Enumerator enumerator = this.m_colorTable.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<int, RtfColor> current = enumerator.Current;
              if (current.Key == int32_8)
              {
                this.CurrColorTable = current.Value;
                this.CurrCellFormat.ForeColor = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
              }
            }
            break;
          }
        case "clshdngraw":
        case "clshdng":
          this.CurrCellFormat.TextureStyle = this.GetTextureStyle(Convert.ToInt32(tokenValue));
          break;
        case "clbghoriz":
        case "rawclbghoriz":
          this.CurrCellFormat.TextureStyle = TextureStyle.TextureHorizontal;
          break;
        case "clbgvert":
        case "rawclbgvert":
          this.CurrCellFormat.TextureStyle = TextureStyle.TextureVertical;
          break;
        case "clbgfdiag":
        case "rawclbgfdiag":
          this.CurrCellFormat.TextureStyle = TextureStyle.TextureDiagonalDown;
          break;
        case "clbgbdiag":
        case "rawclbgbdiag":
          this.CurrCellFormat.TextureStyle = TextureStyle.TextureDiagonalUp;
          break;
        case "clbgcross":
        case "rawclbgcross":
          this.CurrCellFormat.TextureStyle = TextureStyle.TextureCross;
          break;
        case "clbgdcross":
        case "rawclbgdcross":
          this.CurrCellFormat.TextureStyle = TextureStyle.TextureDiagonalCross;
          break;
        case "clbgdkhor":
        case "rawclbgdkhor":
          this.CurrCellFormat.TextureStyle = TextureStyle.TextureDarkHorizontal;
          break;
        case "clbgdkvert":
        case "rawclbgdkvert":
          this.CurrCellFormat.TextureStyle = TextureStyle.TextureDarkVertical;
          break;
        case "clbgdkfdiag":
        case "rawclbgdkfdiag":
          this.CurrCellFormat.TextureStyle = TextureStyle.TextureDarkDiagonalDown;
          break;
        case "clbgdkbdiag":
        case "rawclbgdkbdiag":
          this.CurrCellFormat.TextureStyle = TextureStyle.TextureDarkDiagonalUp;
          break;
        case "clbgdkcross":
        case "rawclbgdkcross":
          this.CurrCellFormat.TextureStyle = TextureStyle.TextureDarkCross;
          break;
        case "clbgdkdcross":
        case "rawclbgdkdcross":
          this.CurrCellFormat.TextureStyle = TextureStyle.TextureDarkDiagonalCross;
          break;
        case "clvmgf":
          this.CurrCellFormat.VerticalMerge = CellMerge.Start;
          break;
        case "clvmrg":
          this.CurrCellFormat.VerticalMerge = CellMerge.Continue;
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
        case "fldinst":
          if (this.m_fieldInstructionGroupStack.Count > 0)
            this.m_fieldInstructionGroupStack.Push(this.m_fieldInstructionGroupStack.Pop() - 1);
          this.m_fieldInstructionGroupStack.Push(1);
          this.m_fieldGroupTypeStack.Push(FieldGroupType.FieldInstruction);
          break;
        case "fldrslt":
          if (this.m_fieldCollectionStack.Count > 0)
          {
            WFieldMark wfieldMark = new WFieldMark((IWordDocument) this.m_document);
            wfieldMark.Type = FieldMarkType.FieldSeparator;
            this.CurrentPara.ChildEntities.Add((IEntity) wfieldMark);
            this.m_fieldCollectionStack.Peek().FieldSeparator = wfieldMark;
          }
          if (this.m_fieldResultGroupStack.Count > 0)
            this.m_fieldResultGroupStack.Push(this.m_fieldResultGroupStack.Pop() - 1);
          this.m_fieldResultGroupStack.Push(1);
          this.m_fieldGroupTypeStack.Push(FieldGroupType.FieldResult);
          break;
        case "wpfldparam":
        case "fldtitle":
          this.m_fieldGroupTypeStack.Push(FieldGroupType.FieldInvalid);
          break;
        case "v":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.IsHiddenText = false;
            break;
          }
          this.m_currTextFormat.IsHiddenText = true;
          break;
        case "spv":
          if (tokenValue != null && Convert.ToInt32(tokenValue) == 0)
          {
            this.m_currTextFormat.SpecVanish = false;
            break;
          }
          this.m_currTextFormat.SpecVanish = true;
          break;
        case "fftype":
          if (this.m_currentFormField == null)
            break;
          switch (Convert.ToInt32(tokenValue))
          {
            case 0:
              this.m_currentFormField.FormFieldType = FormFieldType.TextInput;
              return;
            case 1:
              this.m_currentFormField.FormFieldType = FormFieldType.CheckBox;
              return;
            case 2:
              this.m_currentFormField.FormFieldType = FormFieldType.DropDown;
              return;
            default:
              return;
          }
        case "ffprot":
          if (this.m_currentFormField == null || !(tokenValue == "1"))
            break;
          this.m_currentFormField.Enabled = false;
          break;
        case "ffsize":
          if (this.m_currentFormField == null)
            break;
          if (tokenValue == null || tokenValue != null && tokenValue == "1")
          {
            this.m_currentFormField.CheckboxSizeType = CheckBoxSizeType.Exactly;
            break;
          }
          this.m_currentFormField.CheckboxSizeType = CheckBoxSizeType.Auto;
          break;
        case "ffrecalc":
          if (this.m_currentFormField == null)
            break;
          if (tokenValue == null || tokenValue != null && tokenValue == "1")
          {
            this.m_currentFormField.CalculateOnExit = true;
            break;
          }
          this.m_currentFormField.CalculateOnExit = false;
          break;
        case "ffhaslistbox":
          if (this.m_currentFormField == null)
            break;
          if (tokenValue == null || tokenValue != null && tokenValue == "1")
          {
            this.m_currentFormField.IsListBox = true;
            this.m_currentFormField.DropDownItems = new WDropDownCollection(this.m_document);
            break;
          }
          this.m_currentFormField.IsListBox = false;
          break;
        case "ffmaxlen":
          if (this.m_currentFormField == null || tokenValue == null || !(tokenValue != string.Empty))
            break;
          this.m_currentFormField.MaxLength = Convert.ToInt32(tokenValue);
          break;
        case "ffhps":
          if (this.m_currentFormField == null || tokenValue == null || !(tokenValue != string.Empty) || this.m_currentFormField.CheckboxSizeType != CheckBoxSizeType.Exactly)
            break;
          this.m_currentFormField.CheckboxSize = Convert.ToInt32(tokenValue) / 2;
          break;
        case "ffres":
          if (this.m_currentFormField == null || tokenValue == null || !(tokenValue != string.Empty))
            break;
          this.m_currentFormField.Ffres = Convert.ToInt32(tokenValue);
          break;
        case "ffdefres":
          if (this.m_currentFormField == null || tokenValue == null || !(tokenValue != string.Empty))
            break;
          this.m_currentFormField.Ffdefres = Convert.ToInt32(tokenValue);
          break;
        case "title":
          this.m_document.BuiltinDocumentProperties.Title = "";
          break;
        case "category":
          this.m_document.BuiltinDocumentProperties.Category = "";
          break;
        case "doccomm":
          this.m_document.BuiltinDocumentProperties.Comments = "";
          break;
        case "operator":
          this.m_document.BuiltinDocumentProperties.Author = "";
          break;
        case "manager":
          this.m_document.BuiltinDocumentProperties.Manager = "";
          break;
        case "company":
          this.m_document.BuiltinDocumentProperties.Company = "";
          break;
        case "keywords":
          this.m_document.BuiltinDocumentProperties.Keywords = "";
          break;
        case "subject":
          this.m_document.BuiltinDocumentProperties.Subject = "";
          break;
        default:
          this.ParseSpecialCharacters(token);
          break;
      }
    }
  }

  private float SetFramePositions(string tokenValue, bool isNeg, bool isXValue)
  {
    float num1 = isNeg ? -this.ExtractTwipsValue(tokenValue) : this.ExtractTwipsValue(tokenValue);
    float num2 = isNeg ? (float) -this.GetIntValue(tokenValue) : (float) this.GetIntValue(tokenValue);
    if (isXValue)
    {
      if (this.m_currParagraphFormat.IsFrameXAlign(num2))
        num1 += 0.05f;
    }
    else if (this.m_currParagraphFormat.IsFrameYAlign(num2))
      num1 += 0.05f;
    return num1;
  }

  private void ResetParagraphFormat(WParagraphFormat sourceParaFormat)
  {
    if (!sourceParaFormat.HasKey(8))
      sourceParaFormat.SetPropertyValue(8, (object) 0.0f);
    if (!sourceParaFormat.HasKey(9))
      sourceParaFormat.SetPropertyValue(9, (object) 0.0f);
    if (!sourceParaFormat.HasKey(31 /*0x1F*/))
      sourceParaFormat.Bidi = false;
    if (!sourceParaFormat.HasKey(22))
      sourceParaFormat.ColumnBreakAfter = false;
    if (!sourceParaFormat.HasKey(92))
      sourceParaFormat.ContextualSpacing = false;
    if (!sourceParaFormat.HasKey(5))
      sourceParaFormat.SetPropertyValue(5, (object) 0.0f);
    if (!sourceParaFormat.HasKey(0))
      sourceParaFormat.HorizontalAlignment = HorizontalAlignment.Left;
    if (!sourceParaFormat.HasKey(6))
      sourceParaFormat.Keep = false;
    if (!sourceParaFormat.HasKey(10))
      sourceParaFormat.KeepFollow = false;
    if (!sourceParaFormat.HasKey(2))
      sourceParaFormat.SetPropertyValue(2, (object) 0.0f);
    if (!sourceParaFormat.HasKey(52))
      sourceParaFormat.SetPropertyValue(52, (object) 12f);
    if (!sourceParaFormat.HasKey(53))
      sourceParaFormat.LineSpacingRule = LineSpacingRule.Multiple;
    if (!sourceParaFormat.HasKey(56))
      sourceParaFormat.OutlineLevel = OutlineLevel.BodyText;
    if (!sourceParaFormat.HasKey(13))
      sourceParaFormat.PageBreakAfter = false;
    if (!sourceParaFormat.HasKey(12))
      sourceParaFormat.PageBreakBefore = false;
    if (!sourceParaFormat.HasKey(3))
      sourceParaFormat.SetPropertyValue(3, (object) 0.0f);
    if (!sourceParaFormat.HasKey(55))
      sourceParaFormat.SpaceAfterAuto = false;
    if (!sourceParaFormat.HasKey(54))
      sourceParaFormat.SpaceBeforeAuto = false;
    if (!sourceParaFormat.HasKey(11))
      sourceParaFormat.WidowControl = false;
    if (!sourceParaFormat.HasKey(89))
      sourceParaFormat.WordWrap = true;
    if (!sourceParaFormat.HasKey(83))
      sourceParaFormat.FrameHorizontalDistanceFromText = 0.0f;
    if (!sourceParaFormat.HasKey(84))
      sourceParaFormat.FrameVerticalDistanceFromText = 0.0f;
    if (!sourceParaFormat.HasKey(76))
      sourceParaFormat.FrameWidth = 0.0f;
    if (!sourceParaFormat.HasKey(77))
      sourceParaFormat.FrameHeight = 0.0f;
    if (!sourceParaFormat.HasKey(74))
      sourceParaFormat.FrameY = 0.0f;
    if (!sourceParaFormat.HasKey(73))
      sourceParaFormat.FrameX = 0.0f;
    if (!sourceParaFormat.HasKey(72))
      sourceParaFormat.FrameVerticalPos = (byte) 0;
    if (!sourceParaFormat.HasKey(71))
      sourceParaFormat.FrameHorizontalPos = (byte) 0;
    if (!sourceParaFormat.HasKey(88))
      sourceParaFormat.WrapFrameAround = FrameWrapMode.Auto;
    this.ResetBorders(sourceParaFormat);
  }

  private void ResetBorders(WParagraphFormat sourceParaFormat)
  {
    Borders borders = sourceParaFormat.Borders;
    if (!borders.Top.IsBorderDefined)
      this.ResetBorder(sourceParaFormat.Borders.Top);
    if (!borders.Left.IsBorderDefined)
      this.ResetBorder(sourceParaFormat.Borders.Left);
    if (!borders.Bottom.IsBorderDefined)
      this.ResetBorder(sourceParaFormat.Borders.Bottom);
    if (borders.Right.IsBorderDefined)
      return;
    this.ResetBorder(sourceParaFormat.Borders.Right);
  }

  private void ResetBorder(Border border)
  {
    border.LineWidth = 0.0f;
    border.Color = Color.Empty;
    border.BorderType = BorderStyle.None;
    border.Shadow = false;
    border.Space = 0.0f;
  }

  private void ResetCharacterFormat(WCharacterFormat sourceFormat)
  {
    if ((double) sourceFormat.FontSize != 10.0 && !sourceFormat.HasKey(3))
      sourceFormat.SetPropertyValue(3, (object) 10f);
    if ((double) sourceFormat.Scaling != 100.0 && !sourceFormat.HasKey((int) sbyte.MaxValue))
      sourceFormat.Scaling = 100f;
    if (sourceFormat.TextColor != Color.Empty && !sourceFormat.HasKey(1))
      sourceFormat.TextColor = Color.Empty;
    if (sourceFormat.FontName != "Times New Roman" && !sourceFormat.HasKey(2))
      sourceFormat.FontName = "Times New Roman";
    if (sourceFormat.Bold && !sourceFormat.HasKey(4))
      sourceFormat.Bold = false;
    if (sourceFormat.Italic && !sourceFormat.HasKey(5))
      sourceFormat.Italic = false;
    if (sourceFormat.UnderlineStyle != UnderlineStyle.None && !sourceFormat.HasKey(7))
      sourceFormat.UnderlineStyle = UnderlineStyle.None;
    if (sourceFormat.HighlightColor != Color.Empty && !sourceFormat.HasKey(63 /*0x3F*/))
      sourceFormat.HighlightColor = Color.Empty;
    if (sourceFormat.Shadow && !sourceFormat.HasKey(50))
      sourceFormat.Shadow = false;
    if ((double) sourceFormat.CharacterSpacing != 0.0 && !sourceFormat.HasKey(18))
      sourceFormat.SetPropertyValue(18, (object) 0.0f);
    if (sourceFormat.DoubleStrike && !sourceFormat.HasKey(14))
      sourceFormat.DoubleStrike = false;
    if (sourceFormat.Emboss && !sourceFormat.HasKey(51))
      sourceFormat.Emboss = false;
    if (sourceFormat.Engrave && !sourceFormat.HasKey(52))
      sourceFormat.Engrave = false;
    if (sourceFormat.SubSuperScript != SubSuperScript.None && !sourceFormat.HasKey(10))
      sourceFormat.SubSuperScript = SubSuperScript.None;
    if (sourceFormat.TextBackgroundColor != Color.Empty && !sourceFormat.HasKey(9))
      sourceFormat.TextBackgroundColor = Color.Empty;
    if (sourceFormat.ForeColor != Color.Empty && !sourceFormat.HasKey(77))
      sourceFormat.ForeColor = Color.Empty;
    if (sourceFormat.AllCaps && !sourceFormat.HasKey(54))
      sourceFormat.AllCaps = false;
    if (sourceFormat.BoldBidi && !sourceFormat.HasKey(59))
      sourceFormat.BoldBidi = false;
    if (sourceFormat.FieldVanish && !sourceFormat.HasKey(109))
      sourceFormat.FieldVanish = false;
    if (sourceFormat.Hidden && !sourceFormat.HasKey(53))
      sourceFormat.Hidden = false;
    if (sourceFormat.SpecVanish && !sourceFormat.HasKey(24))
      sourceFormat.SpecVanish = false;
    if (!sourceFormat.SmallCaps || sourceFormat.HasKey(55))
      return;
    sourceFormat.SmallCaps = false;
  }

  private float GetEqualColumnWidth(int columnCount)
  {
    return (this.m_secFormat.pageSize.Width - (this.m_secFormat.LeftMargin + this.m_secFormat.RightMargin) - (float) (36 * (columnCount - 1))) / (float) columnCount;
  }

  private void ParseSpecialCharacters(string token)
  {
    string m_token = (string) null;
    if (!this.IsDestinationControlWord || this.IsFieldGroup || this.m_bIsShapeText)
    {
      if (this.StartsWithExt(token, "'") && !this.IsAccentCharacterNeedToBeOmitted())
      {
        this.m_bIsAccentChar = true;
        m_token = this.GetAccentCharacter(token);
        if (m_token == " ")
        {
          this.m_tokenType = RtfTokenType.Text;
          this.m_lexer.CurrRtfTokenType = RtfTokenType.Text;
        }
      }
      else if (this.StartsWithExt(token, "_"))
        m_token = token.Replace("_", '\u001E'.ToString());
      else if (this.StartsWithExt(token, "~"))
        m_token = token.Replace("~", ' '.ToString());
      else if (this.StartsWithExt(token, "-"))
        m_token = token.Replace("-", '\u001F'.ToString());
      else if (this.StartsWithExt(token, ":"))
        m_token = token;
      else if (this.StartsWithExt(token, "zw") && this.m_tokenType == RtfTokenType.ControlWord)
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
    {
      this.isSpecialCharacter = true;
      this.ParseDocumentElement(m_token);
    }
    if (!this.StartsWithExt(token, "zw") || this.m_tokenType != RtfTokenType.Text)
      return;
    this.m_tokenType = RtfTokenType.ControlWord;
    this.m_lexer.CurrRtfTokenType = RtfTokenType.ControlWord;
  }

  private bool IsAccentCharacterNeedToBeOmitted()
  {
    return this.m_unicodeCount > 0 && --this.m_unicodeCount >= 0;
  }

  private TextureStyle GetTextureStyle(int textureValue)
  {
    switch (textureValue)
    {
      case 250:
        return TextureStyle.Texture2Pt5Percent;
      case 500:
        return TextureStyle.Texture5Percent;
      case 750:
        return TextureStyle.Texture7Pt5Percent;
      case 1000:
        return TextureStyle.Texture10Percent;
      case 1250:
        return TextureStyle.Texture12Pt5Percent;
      case 1500:
        return TextureStyle.Texture15Percent;
      case 1750:
        return TextureStyle.Texture17Pt5Percent;
      case 2000:
        return TextureStyle.Texture20Percent;
      case 2250:
        return TextureStyle.Texture22Pt5Percent;
      case 2500:
        return TextureStyle.Texture25Percent;
      case 2750:
        return TextureStyle.Texture27Pt5Percent;
      case 3000:
        return TextureStyle.Texture30Percent;
      case 3250:
        return TextureStyle.Texture32Pt5Percent;
      case 3500:
        return TextureStyle.Texture35Percent;
      case 3750:
        return TextureStyle.Texture37Pt5Percent;
      case 4000:
        return TextureStyle.Texture40Percent;
      case 4250:
        return TextureStyle.Texture42Pt5Percent;
      case 4500:
        return TextureStyle.Texture45Percent;
      case 4750:
        return TextureStyle.Texture47Pt5Percent;
      case 5000:
        return TextureStyle.Texture50Percent;
      case 5250:
        return TextureStyle.Texture52Pt5Percent;
      case 5500:
        return TextureStyle.Texture55Percent;
      case 5750:
        return TextureStyle.Texture57Pt5Percent;
      case 6000:
        return TextureStyle.Texture60Percent;
      case 6250:
        return TextureStyle.Texture62Pt5Percent;
      case 6500:
        return TextureStyle.Texture65Percent;
      case 6750:
        return TextureStyle.Texture67Pt5Percent;
      case 7000:
        return TextureStyle.Texture70Percent;
      case 7250:
        return TextureStyle.Texture72Pt5Percent;
      case 7500:
        return TextureStyle.Texture75Percent;
      case 7750:
        return TextureStyle.Texture77Pt5Percent;
      case 8000:
        return TextureStyle.Texture80Percent;
      case 8250:
        return TextureStyle.Texture82Pt5Percent;
      case 8500:
        return TextureStyle.Texture85Percent;
      case 8750:
        return TextureStyle.Texture87Pt5Percent;
      case 9000:
        return TextureStyle.Texture90Percent;
      case 9250:
        return TextureStyle.Texture92Pt5Percent;
      case 9500:
        return TextureStyle.Texture95Percent;
      case 9750:
        return TextureStyle.Texture97Pt5Percent;
      case 10000:
        return TextureStyle.TextureSolid;
      default:
        return TextureStyle.TextureNone;
    }
  }

  private void ParseListTextStart()
  {
    this.m_stack.Push("{");
    if (this.m_bIsList)
    {
      this.CurrentPara.ListFormat.ContinueListNumbering();
      this.CopyParagraphFormatting(this.m_currParagraphFormat, this.CurrentPara.ParagraphFormat);
    }
    this.m_previousRtfFont = this.CurrRtfFont.Clone();
    this.m_prevParagraph = this.CurrentPara;
    this.m_prevTextFormat = this.m_currTextFormat.Clone();
    this.m_currTextFormat.Underline = RtfParser.ThreeState.Unknown;
    this.m_currTextFormat.m_underlineStyle = UnderlineStyle.None;
    this.m_currTextFormat.FontFamily = string.Empty;
    this.CurrentPara = (IWParagraph) new WParagraph((IWordDocument) this.m_document);
    this.m_listLevelCharFormat = new WCharacterFormat((IWordDocument) this.m_document);
    this.m_listLevelParaFormat = new WParagraphFormat((IWordDocument) this.m_document);
    this.m_bIsListText = true;
  }

  private void ParseParagraphEnd()
  {
    if (this.CurrentPara.ListFormat.CurrentListStyle != null && this.m_bIsList)
    {
      if (this.m_listLevelParaFormat != null)
        this.CopyParagraphFormatting(this.m_listLevelParaFormat, this.CurrentPara.ListFormat.CurrentListLevel.ParagraphFormat);
      if (this.m_listLevelCharFormat != null)
        this.CopyCharacterFormatting(this.m_listLevelCharFormat, this.CurrentPara.ListFormat.CurrentListLevel.CharacterFormat);
    }
    this.ResetParagraphFormat(this.m_currParagraphFormat);
    this.ResetCharacterFormat(this.CurrentPara.BreakCharacterFormat);
    this.CopyParagraphFormatting(this.m_currParagraphFormat, this.CurrentPara.ParagraphFormat);
    if (!this.isPardTagpresent && (this.m_textBody == null || this.m_textBody.Count == 0) && this.m_document.LastParagraph != null && this.m_secCount > 0)
    {
      this.CurrentPara.ParagraphFormat.CopyFormat((FormatBase) this.m_document.LastParagraph.ParagraphFormat);
      this.CurrentPara.BreakCharacterFormat.CopyFormat((FormatBase) this.m_document.LastParagraph.BreakCharacterFormat);
    }
    this.m_tabCount = 0;
    IWParagraph currentPara = this.CurrentPara;
    if (!this.IsFieldGroup && (!this.m_bIsShapeText || !(this.m_previousToken != "nonesttables")) && (this.IsDestinationControlWord || !(this.m_previousToken != "nonesttables")))
      return;
    if (this.m_previousToken == "row" && this.m_previousLevel == this.m_currentLevel && this.m_bInTable && this.m_currentLevel <= 1)
    {
      this.m_bInTable = false;
      this.m_currentLevel = 0;
    }
    this.ProcessTableInfo(false);
    this.AddNewParagraph(this.CurrentPara);
    this.m_currParagraph = (IWParagraph) new WParagraph((IWordDocument) this.m_document);
    if (currentPara == null || currentPara.StyleName == null)
      return;
    if (!this.m_bIsListText)
    {
      this.CurrentPara.ParagraphFormat.SetDefaultProperties();
      this.CurrentPara.BreakCharacterFormat.SetDefaultProperties();
    }
    (this.m_currParagraph as WParagraph).ApplyStyle(currentPara.StyleName, false);
  }

  private void ParseParagraphStart()
  {
    this.m_bIsLinespacingRule = false;
    this.m_tabCount = 0;
    this.m_tabCollection.Clear();
    this.CurrTabFormat = new TabFormat();
    this.m_bIsPreviousList = this.m_bIsList;
    this.m_bIsList = false;
    if (this.CurrentPara.Items.Count > 0)
    {
      List<Entity> entityList = new List<Entity>();
      foreach (Entity entity in (CollectionImpl) this.CurrentPara.Items)
        entityList.Add(entity);
      this.CurrentPara = (IWParagraph) new WParagraph((IWordDocument) this.m_document);
      foreach (IEntity entity in entityList)
        this.CurrentPara.Items.Add(entity);
      entityList.Clear();
    }
    else
      this.CurrentPara = (IWParagraph) new WParagraph((IWordDocument) this.m_document);
    this.m_currParagraphFormat = new WParagraphFormat((IWordDocument) this.m_document);
    if (this.m_paragraphFormatStack.Count > 0)
      this.m_paragraphFormatStack.Pop();
    this.m_paragraphFormatStack.Push(this.m_currParagraphFormat);
    this.m_bIsBorderTop = false;
    this.m_bIsBorderBottom = false;
    this.m_bIsBorderLeft = false;
    this.m_bIsBorderRight = false;
    if (this.m_bIsShape && this.m_shapeTextStack.Count > 0)
      return;
    this.m_currentLevel = 0;
  }

  private void ParseSectionStart()
  {
    if (!this.m_bIsDefaultSectionFormat)
    {
      this.m_bIsDefaultSectionFormat = true;
      this.m_secFormat.HeaderDistance = Convert.ToSingle(36);
      this.m_secFormat.FooterDistance = Convert.ToSingle(36);
      this.m_defaultSectionFormat = new RtfParser.SecionFormat();
      this.CopySectionFormat(this.m_secFormat, this.m_defaultSectionFormat);
    }
    if (this.m_bIsHeader || this.m_bIsFooter || this.m_bIsRow)
      return;
    if (this.m_previousToken == "sect")
    {
      ++this.m_secCount;
      this.m_currSection = (IWSection) new WSection((IWordDocument) this.m_document);
      this.m_textBody = this.m_currSection.Body;
      this.m_secFormat = new RtfParser.SecionFormat();
      if (this.m_bIsDefaultSectionFormat)
        this.CopySectionFormat(this.m_defaultSectionFormat, this.m_secFormat);
      this.CurrColumn = (Column) null;
    }
    else
    {
      this.CurrColumn = (Column) null;
      this.CurrentSection.Columns.InnerList.Clear();
      this.CurrentSection.BreakCode = SectionBreakCode.NewPage;
    }
    this.CurrentSection.PageSetup.EqualColumnWidth = true;
  }

  private void CopySectionFormat(
    RtfParser.SecionFormat sourceFormat,
    RtfParser.SecionFormat destFormat)
  {
    destFormat.BottomMargin = sourceFormat.BottomMargin;
    destFormat.DefaultTabWidth = sourceFormat.DefaultTabWidth;
    destFormat.DifferentFirstPage = sourceFormat.DifferentFirstPage;
    destFormat.DifferentOddAndEvenPage = sourceFormat.DifferentOddAndEvenPage;
    destFormat.FooterDistance = sourceFormat.FooterDistance;
    destFormat.HeaderDistance = sourceFormat.HeaderDistance;
    destFormat.IsFrontPageBorder = sourceFormat.IsFrontPageBorder;
    destFormat.LeftMargin = sourceFormat.LeftMargin;
    destFormat.m_pageOrientation = sourceFormat.m_pageOrientation;
    destFormat.pageSize = sourceFormat.pageSize;
    destFormat.RightMargin = sourceFormat.RightMargin;
    destFormat.TopMargin = sourceFormat.TopMargin;
    destFormat.VertAlignment = sourceFormat.VertAlignment;
    destFormat.FirstPageTray = sourceFormat.FirstPageTray;
    destFormat.OtherPagesTray = sourceFormat.OtherPagesTray;
  }

  private void ParseRowStart(bool isFromShape)
  {
    this.m_bIsRow = true;
    this.m_bInTable = true;
    this.m_currCellFormatIndex = -1;
    this.CurrCellFormat = new CellFormat();
    if (isFromShape && this.m_currentLevel > 0)
      ++this.m_currentLevel;
    if (this.m_currentLevel <= 1)
    {
      this.m_CellFormatStack.Clear();
      this.m_currRowFormatStack.Clear();
    }
    if (this.m_currentLevel > 1 && this.m_currentLevel <= this.m_CellFormatStack.Count)
      this.m_CellFormatStack.Pop();
    if (this.m_currentLevel > 1 && this.m_currentLevel <= this.m_currRowFormatStack.Count)
      this.m_currRowFormatStack.Pop();
    this.m_cellFormatTable = new Dictionary<int, CellFormat>();
    this.m_CellFormatStack.Push(this.m_cellFormatTable);
    this.CurrRowFormat = new RowFormat((IWordDocument) this.m_document);
    this.m_currRowFormatStack.Push(this.CurrRowFormat);
  }

  private void ParseRowEnd(bool isShapeTextEnd)
  {
    this.m_bIsRow = false;
    this.m_bInTable = false;
    int index = 0;
    this.m_prevCellFormatStack = new Stack<Dictionary<int, CellFormat>>((IEnumerable<Dictionary<int, CellFormat>>) this.m_CellFormatStack.ToArray());
    this.m_prevRowFormatStack = new Stack<RowFormat>((IEnumerable<RowFormat>) this.m_currRowFormatStack.ToArray());
    if (this.m_currRowFormatStack.Count > 0)
      this.CurrRowFormat = this.m_currRowFormatStack.Pop();
    if (this.m_currTable != null)
    {
      this.ApplyRowFormatting(this.m_currTable.LastRow, this.CurrRowFormat);
      this.CopyTextFormatToCharFormat(this.m_currTable.LastRow.CharacterFormat, this.m_currTextFormat);
    }
    this.m_cellFormatTable = this.m_CellFormatStack.Pop();
    if (!isShapeTextEnd)
    {
      foreach (KeyValuePair<int, CellFormat> keyValuePair in this.m_cellFormatTable)
      {
        if (this.m_currTable != null)
          this.ApplyCellFormatting(this.m_currTable.LastRow.Cells[index], keyValuePair.Value);
        ++index;
      }
    }
    this.CurrRowFormat = new RowFormat((IWordDocument) this.m_document);
    if (this.m_currParagraph != null)
      this.SetDefaultValue(this.m_currParagraph.ParagraphFormat);
    this.m_currCellFormatIndex = -1;
    this.m_currRow = (WTableRow) null;
    this.m_leftcellspace = 0.0f;
    this.m_rightcellspace = 0.0f;
    this.m_bottomcellspace = 0.0f;
    this.m_topcellspace = 0.0f;
    this.m_CellFormatStack = new Stack<Dictionary<int, CellFormat>>((IEnumerable<Dictionary<int, CellFormat>>) this.m_prevCellFormatStack.ToArray());
    this.m_currRowFormatStack = new Stack<RowFormat>((IEnumerable<RowFormat>) this.m_prevRowFormatStack.ToArray());
    if (this.m_currentLevel > 1)
    {
      this.m_currRowFormatStack.Pop();
      this.m_CellFormatStack.Pop();
      if (this.m_nestedTable.Count > 0)
        --this.m_currentLevel;
      else
        this.m_currentLevel = 0;
    }
    if (this.PreviousLevel != this.CurrentLevel)
      return;
    this.m_currentLevel = 0;
  }

  private void ParseCellBoundary(string token, string tokenKey, string tokenValue)
  {
    int currCellBoundary = this.m_currCellBoundary;
    this.m_currCellBoundary = !tokenKey.EndsWith("-") ? Convert.ToInt32(tokenValue) : -Convert.ToInt32(tokenValue);
    ++this.m_currCellFormatIndex;
    this.m_cellFormatTable = this.m_CellFormatStack.Pop();
    this.CurrCellFormat.CellWidth = this.m_cellFormatTable.Count <= 0 ? this.ExtractTwipsValue((this.m_currCellBoundary - this.m_currenttrleft).ToString()) : this.ExtractTwipsValue((this.m_currCellBoundary - currCellBoundary).ToString());
    this.m_cellFormatTable.Add(this.m_currCellFormatIndex, this.CurrCellFormat);
    this.m_CellFormatStack.Push(this.m_cellFormatTable);
    this.CurrCellFormat = new CellFormat();
    this.m_bIsBorderTop = false;
    this.m_bIsBorderRight = false;
    this.m_bIsBorderLeft = false;
    this.m_bIsBorderBottom = false;
  }

  private bool GetTextWrapAround(RowFormat.TablePositioning positioning)
  {
    if (positioning.HasValue(64 /*0x40*/))
      return true;
    if (positioning.HasValue(62))
      return (double) positioning.HorizPosition != 0.0;
    if (positioning.HasValue(65))
    {
      if (positioning.VertRelationTo == VerticalRelation.Paragraph)
        return true;
      if (positioning.HasValue(63 /*0x3F*/))
        return (double) positioning.VertPosition != 0.0;
    }
    else if (positioning.HasValue(63 /*0x3F*/))
      return (double) positioning.VertPosition != 0.0;
    return false;
  }

  private string GetAccentCharacter(string token)
  {
    int result = 0;
    string str1 = "";
    int length = token.Length;
    string s1 = length > 2 ? token.Substring(1, 2) : token.Substring(1, 1);
    string str2 = string.Empty;
    if (s1 != "3f")
    {
      bool flag = false;
      if (this.m_currTextFormat.FontFamily.Length == 0 && this.m_currentTableType != RtfTableType.FontTable)
      {
        foreach (KeyValuePair<string, RtfFont> keyValuePair in this.m_fontTable)
        {
          string[] strArray = keyValuePair.Key.Split('f');
          if (strArray[strArray.Length - 1] == this.DefaultFontIndex.ToString())
          {
            flag = true;
            this.CurrRtfFont = keyValuePair.Value;
            this.m_currTextFormat.FontFamily = this.CurrRtfFont.FontName.Trim();
            break;
          }
        }
      }
      int.TryParse(s1, NumberStyles.HexNumber, (IFormatProvider) CultureInfo.InvariantCulture, out result);
      Encoding encoding = this.GetEncoding();
      if (!this.IsSingleByte())
      {
        string token1 = this.m_lexer.ReadNextToken(this.m_previousTokenKey, this.m_bIsLevelText);
        while (token1 == "\n" || token1 == "\r")
          token1 = this.m_lexer.ReadNextToken(this.m_previousTokenKey, this.m_bIsLevelText);
        string[] strArray = this.SeperateToken(token1);
        this.m_previousTokenKey = strArray[0];
        this.m_previousTokenValue = strArray[1];
        string str3 = token1.Trim().Substring(1);
        string s2 = str3.Substring(1, 2) + s1;
        str2 = str3.Substring(3);
        result = int.Parse(s2, NumberStyles.HexNumber);
      }
      byte[] bytes = BitConverter.GetBytes((short) result);
      str1 = encoding.GetString(bytes, 0, bytes.Length).Replace("\0", "");
      if (flag)
        this.m_currTextFormat.FontFamily = string.Empty;
    }
    else if (this.m_previousTokenKey == "u" || this.m_previousTokenKey == "u-")
      str1 = ((char) Convert.ToInt32(this.m_previousTokenValue)).ToString();
    if (length > 3)
      str1 += token.Substring(3, length - 3);
    return str1 + str2;
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
    if (this.m_currentTableType == RtfTableType.FontTable && this.m_lexer.CurrRtfTableType == RtfTableType.FontTable)
    {
      int fontCharSet = (int) this.CurrRtfFont.FontCharSet;
      return (int) this.CurrRtfFont.FontCharSet;
    }
    return (this.CurrListLevel == null || !(this.CurrListLevel.CharacterFormat.FontName == this.CurrRtfFont.FontName)) && (this.m_currTextFormat == null || this.CurrRtfFont == null || !(this.m_currTextFormat.FontFamily == this.CurrRtfFont.FontName)) ? 1 : (int) this.CurrRtfFont.FontCharSet;
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

  private void SetDefaultValue(WParagraphFormat paragraphFormat)
  {
    paragraphFormat.SetPropertyValue(9, (object) 0.0f);
    paragraphFormat.SetPropertyValue(8, (object) 0.0f);
    paragraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
    paragraphFormat.SetPropertyValue(2, (object) 0.0f);
    paragraphFormat.SetPropertyValue(3, (object) 0.0f);
    paragraphFormat.BackColor = Color.Empty;
    paragraphFormat.ForeColor = Color.Empty;
    paragraphFormat.SetPropertyValue(52, (object) 0.0f);
    paragraphFormat.LineSpacingRule = LineSpacingRule.AtLeast;
    paragraphFormat.TextureStyle = TextureStyle.TextureNone;
  }

  private void ProcessTableInfo(bool isShapeTextEnd)
  {
    if (this.m_bInTable && this.m_currentLevel == 0)
      this.m_currentLevel = 1;
    PrepareTableInfo prepareTableInfo = new PrepareTableInfo(this.m_bInTable, this.m_currentLevel, this.m_previousLevel);
    WTextBody wtextBody = this.m_textBody != null ? this.m_textBody : this.CurrentSection.Body;
    if (prepareTableInfo.InTable && prepareTableInfo.State != PrepareTableState.LeaveTable)
    {
      if (this.m_currRow == null)
      {
        if (this.m_currTable == null)
          this.m_currTable = (IWTable) new WTable((IWordDocument) this.m_document);
        this.m_currRow = this.m_currTable.AddRow(false, false);
        this.m_currCell = this.m_currRow.AddCell(false);
        this.m_textBody = (WTextBody) this.m_currCell;
      }
      else if (this.m_bCellFinished)
      {
        this.m_currCell = this.m_currTable.LastRow.AddCell();
        this.m_textBody = (WTextBody) this.m_currCell;
      }
    }
    if (this.m_bCellFinished)
      this.m_bCellFinished = false;
    switch (prepareTableInfo.State)
    {
      case PrepareTableState.EnterTable:
        if (prepareTableInfo.PrevLevel == 0)
          this.m_nestedTextBody.Push(wtextBody);
        this.EnsureUpperTable(prepareTableInfo.Level);
        break;
      case PrepareTableState.LeaveTable:
        this.EnsureLowerTable(prepareTableInfo.Level, isShapeTextEnd);
        break;
    }
  }

  private void EnsureLowerTable(int level, bool isShapeTextEnd)
  {
    if (this.m_currTable != null)
    {
      float beforeWidth1 = (this.m_currTable.ChildEntities[0] as WTableRow).RowFormat.BeforeWidth;
      bool flag = false;
      for (int index = 0; index < this.m_currTable.ChildEntities.Count; ++index)
      {
        float beforeWidth2 = (this.CurrTable.ChildEntities[index] as WTableRow).RowFormat.BeforeWidth;
        if ((double) beforeWidth1 == (double) beforeWidth2)
        {
          flag = true;
        }
        else
        {
          flag = false;
          break;
        }
      }
      if (flag)
      {
        for (int index = 0; index < this.m_currTable.ChildEntities.Count; ++index)
          (this.CurrTable.ChildEntities[index] as WTableRow).RowFormat.BeforeWidth = 0.0f;
      }
    }
    while (this.m_nestedTable.Count > level)
      this.m_nestedTable.Pop();
    if (!this.m_currTable.FirstRow.RowFormat.IsLeftIndentDefined)
      this.m_currTable.TableFormat.LeftIndent = 0.0f;
    else if ((double) this.m_currTable.TableFormat.LeftIndent != (double) this.m_currTable.FirstRow.RowFormat.LeftIndent)
      this.m_currTable.TableFormat.LeftIndent = this.m_currTable.FirstRow.RowFormat.LeftIndent;
    if (level == 0)
    {
      this.m_textBody = this.m_nestedTextBody.Pop();
      for (int index1 = 0; index1 < this.m_currTable.Rows.Count; ++index1)
      {
        for (int index2 = 0; index2 < this.m_currTable.Rows[index1].Cells.Count; ++index2)
        {
          WTableCell cell = this.m_currTable.Rows[index1].Cells[index2];
          if (cell.CellFormat.HorizontalMerge == CellMerge.Start && (double) cell.Width < (double) cell.PreferredWidth.Width && cell.NextSibling is WTableCell && (cell.NextSibling as WTableCell).CellFormat.HorizontalMerge == CellMerge.Continue && (double) cell.PreferredWidth.Width == (double) cell.Width + (double) (cell.NextSibling as WTableCell).Width)
            this.m_currTable.Rows[index1].Cells.RemoveAt(index2 + 1);
        }
      }
      if (this.m_currTable != null && !isShapeTextEnd)
      {
        if ((double) this.CurrentSection.PageSetup.Margins.Bottom != (double) this.m_secFormat.BottomMargin)
          this.ApplySectionFormatting();
        if (!this.m_currTable.TableFormat.IsAutoResized && this.m_currTable.FirstRow.RowFormat.IsAutoResized)
          this.m_currTable.TableFormat.IsAutoResized = this.m_currTable.FirstRow.RowFormat.IsAutoResized;
        (this.m_currTable as WTable).IsTableGridUpdated = false;
        (this.m_currTable as WTable).UpdateGridSpan();
      }
      this.m_textBody.Items.Add((IEntity) this.m_currTable);
      this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_2, 11);
      this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_2, 10);
      this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_2, 16 /*0x10*/);
      this.m_currTable = (IWTable) null;
      this.m_currRow = (WTableRow) null;
      this.m_currCell = (WTableCell) null;
      this.m_previousLevel = level;
    }
    else
    {
      WTable currTable = this.m_currTable as WTable;
      this.m_currTable = (IWTable) this.m_nestedTable.Pop();
      this.m_currRow = this.m_currTable.LastRow;
      this.m_currCell = this.m_currTable.LastCell;
      this.m_textBody = (WTextBody) this.m_currTable.LastCell;
      this.m_textBody.Items.Add((IEntity) currTable);
    }
    if (this.m_currTable == null || isShapeTextEnd)
      return;
    (this.m_currTable as WTable).UpdateGridSpan();
  }

  private void EnsureUpperTable(int level)
  {
    while (this.m_nestedTable.Count < level - 1)
    {
      if (this.m_currTable != null)
        this.m_nestedTable.Push(this.m_currTable as WTable);
      this.m_currTable = (IWTable) new WTable((IWordDocument) this.m_document);
      this.m_currRow = this.m_currTable.AddRow(false, false);
      this.m_currCell = this.m_currRow.AddCell(false);
      this.m_textBody = (WTextBody) this.m_currCell;
    }
  }

  private void ApplyListFormatting(
    string token,
    string tokenKey,
    string tokenValue,
    WListFormat listFormat)
  {
    foreach (KeyValuePair<string, string> keyValuePair in this.m_listOverrideTable)
    {
      if (keyValuePair.Key == token)
      {
        string styleName = keyValuePair.Value;
        listFormat.ApplyStyle(styleName);
        listFormat.ListLevelNumber = 0;
        break;
      }
    }
  }

  private void ApplySectionFormatting()
  {
    if ((double) this.m_secFormat.BottomMargin >= 0.0)
      this.CurrentSection.PageSetup.Margins.Bottom = this.m_secFormat.BottomMargin;
    if ((double) this.m_secFormat.LeftMargin >= 0.0)
      this.CurrentSection.PageSetup.Margins.Left = this.m_secFormat.LeftMargin;
    if ((double) this.m_secFormat.RightMargin >= 0.0)
      this.CurrentSection.PageSetup.Margins.Right = this.m_secFormat.RightMargin;
    if ((double) this.m_secFormat.TopMargin >= 0.0)
      this.CurrentSection.PageSetup.Margins.Top = this.m_secFormat.TopMargin;
    SizeF pageSize = this.m_secFormat.pageSize;
    if ((double) this.m_secFormat.pageSize.Width > 0.0 && (double) this.m_secFormat.pageSize.Height > 0.0)
      this.CurrentSection.PageSetup.PageSize = this.m_secFormat.pageSize;
    this.CurrentSection.PageSetup.SetPageSetupProperty("HeaderDistance", (object) this.m_secFormat.HeaderDistance);
    this.CurrentSection.PageSetup.SetPageSetupProperty("FooterDistance", (object) this.m_secFormat.FooterDistance);
    this.m_document.DefaultTabWidth = this.m_secFormat.DefaultTabWidth;
    this.CurrentSection.PageSetup.VerticalAlignment = this.m_secFormat.VertAlignment;
    if (this.m_secFormat.m_pageOrientation != PageOrientation.Portrait)
      this.CurrentSection.PageSetup.Orientation = this.m_secFormat.m_pageOrientation;
    this.m_document.DifferentOddAndEvenPages = this.m_secFormat.DifferentOddAndEvenPage;
    if (this.m_secFormat.FirstPageTray > 0)
      this.CurrentSection.PageSetup.FirstPageTray = (PrinterPaperTray) this.m_secFormat.FirstPageTray;
    if (this.m_secFormat.OtherPagesTray <= 0)
      return;
    this.CurrentSection.PageSetup.OtherPagesTray = (PrinterPaperTray) this.m_secFormat.OtherPagesTray;
  }

  private void ParseShapeToken(string token, string tokenKey, string tokenValue)
  {
    if (!this.m_bIsShapePicture || this.m_bIsGroupShape)
      return;
    switch (tokenKey)
    {
      case "posh":
        switch (tokenValue)
        {
          case null:
            return;
          case "1":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.HorizontalAlignment = ShapeHorizontalAlignment.Left;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.HorizontalAlignment = ShapeHorizontalAlignment.Left;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HorizontalAlignment = ShapeHorizontalAlignment.Left;
            return;
          case "2":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.HorizontalAlignment = ShapeHorizontalAlignment.Center;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.HorizontalAlignment = ShapeHorizontalAlignment.Center;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HorizontalAlignment = ShapeHorizontalAlignment.Center;
            return;
          case "3":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.HorizontalAlignment = ShapeHorizontalAlignment.Right;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.HorizontalAlignment = ShapeHorizontalAlignment.Right;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HorizontalAlignment = ShapeHorizontalAlignment.Right;
            return;
          case "4":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.HorizontalAlignment = ShapeHorizontalAlignment.Inside;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.HorizontalAlignment = ShapeHorizontalAlignment.Inside;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HorizontalAlignment = ShapeHorizontalAlignment.Inside;
            return;
          case "5":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.HorizontalAlignment = ShapeHorizontalAlignment.Outside;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.HorizontalAlignment = ShapeHorizontalAlignment.Outside;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HorizontalAlignment = ShapeHorizontalAlignment.Outside;
            return;
          default:
            return;
        }
      case "posv":
        switch (tokenValue)
        {
          case null:
            return;
          case "1":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.VerticalAlignment = ShapeVerticalAlignment.Top;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.VerticalAlignment = ShapeVerticalAlignment.Top;
              return;
            }
            this.m_currTextBox.TextBoxFormat.VerticalAlignment = ShapeVerticalAlignment.Top;
            return;
          case "2":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.VerticalAlignment = ShapeVerticalAlignment.Center;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.VerticalAlignment = ShapeVerticalAlignment.Center;
              return;
            }
            this.m_currTextBox.TextBoxFormat.VerticalAlignment = ShapeVerticalAlignment.Center;
            return;
          case "3":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.VerticalAlignment = ShapeVerticalAlignment.Bottom;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.VerticalAlignment = ShapeVerticalAlignment.Bottom;
              return;
            }
            this.m_currTextBox.TextBoxFormat.VerticalAlignment = ShapeVerticalAlignment.Bottom;
            return;
          case "4":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.VerticalAlignment = ShapeVerticalAlignment.Inside;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.VerticalAlignment = ShapeVerticalAlignment.Inside;
              return;
            }
            this.m_currTextBox.TextBoxFormat.VerticalAlignment = ShapeVerticalAlignment.Inside;
            return;
          case "5":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.VerticalAlignment = ShapeVerticalAlignment.Outside;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.VerticalAlignment = ShapeVerticalAlignment.Outside;
              return;
            }
            this.m_currTextBox.TextBoxFormat.VerticalAlignment = ShapeVerticalAlignment.Outside;
            return;
          default:
            return;
        }
      case "posrelh":
        switch (tokenValue)
        {
          case "0":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.HorizontalOrigin = HorizontalOrigin.Margin;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.HorizontalOrigin = HorizontalOrigin.Margin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HorizontalOrigin = HorizontalOrigin.Margin;
            return;
          case "1":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.HorizontalOrigin = HorizontalOrigin.Page;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.HorizontalOrigin = HorizontalOrigin.Page;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HorizontalOrigin = HorizontalOrigin.Page;
            return;
          case "2":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.HorizontalOrigin = HorizontalOrigin.Column;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.HorizontalOrigin = HorizontalOrigin.Column;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HorizontalOrigin = HorizontalOrigin.Column;
            return;
          case "3":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.HorizontalOrigin = HorizontalOrigin.Character;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.HorizontalOrigin = HorizontalOrigin.Character;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HorizontalOrigin = HorizontalOrigin.Character;
            return;
          case "4":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.HorizontalOrigin = HorizontalOrigin.LeftMargin;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.HorizontalOrigin = HorizontalOrigin.LeftMargin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HorizontalOrigin = HorizontalOrigin.LeftMargin;
            return;
          case "5":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.HorizontalOrigin = HorizontalOrigin.RightMargin;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.HorizontalOrigin = HorizontalOrigin.RightMargin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HorizontalOrigin = HorizontalOrigin.RightMargin;
            return;
          case "6":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.HorizontalOrigin = HorizontalOrigin.InsideMargin;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.HorizontalOrigin = HorizontalOrigin.InsideMargin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HorizontalOrigin = HorizontalOrigin.InsideMargin;
            return;
          case "7":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.HorizontalOrigin = HorizontalOrigin.OutsideMargin;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.HorizontalOrigin = HorizontalOrigin.OutsideMargin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HorizontalOrigin = HorizontalOrigin.OutsideMargin;
            return;
          case null:
            return;
          default:
            return;
        }
      case "posrelv":
        switch (tokenValue)
        {
          case "0":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.VerticalOrigin = VerticalOrigin.Margin;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.VerticalOrigin = VerticalOrigin.Margin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.VerticalOrigin = VerticalOrigin.Margin;
            return;
          case "1":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.VerticalOrigin = VerticalOrigin.Page;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.VerticalOrigin = VerticalOrigin.Page;
              return;
            }
            this.m_currTextBox.TextBoxFormat.VerticalOrigin = VerticalOrigin.Page;
            return;
          case "2":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.VerticalOrigin = VerticalOrigin.Paragraph;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.VerticalOrigin = VerticalOrigin.Paragraph;
              return;
            }
            this.m_currTextBox.TextBoxFormat.VerticalOrigin = VerticalOrigin.Paragraph;
            return;
          case "3":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.VerticalOrigin = VerticalOrigin.Line;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.VerticalOrigin = VerticalOrigin.Line;
              return;
            }
            this.m_currTextBox.TextBoxFormat.VerticalOrigin = VerticalOrigin.Line;
            return;
          case "4":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.VerticalOrigin = VerticalOrigin.TopMargin;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.VerticalOrigin = VerticalOrigin.TopMargin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.VerticalOrigin = VerticalOrigin.TopMargin;
            return;
          case "5":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.VerticalOrigin = VerticalOrigin.BottomMargin;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.VerticalOrigin = VerticalOrigin.BottomMargin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.VerticalOrigin = VerticalOrigin.BottomMargin;
            return;
          case "6":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.VerticalOrigin = VerticalOrigin.InsideMargin;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.VerticalOrigin = VerticalOrigin.InsideMargin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.VerticalOrigin = VerticalOrigin.InsideMargin;
            return;
          case "7":
            if (this.m_currPicture != null)
            {
              this.m_currPicture.VerticalOrigin = VerticalOrigin.OutsideMargin;
              return;
            }
            if (this.m_currShape != null)
            {
              this.m_currShape.VerticalOrigin = VerticalOrigin.OutsideMargin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.VerticalOrigin = VerticalOrigin.OutsideMargin;
            return;
          case null:
            return;
          default:
            return;
        }
      case "fLayoutInCell":
        if (tokenValue == "1")
        {
          if (this.m_currPicture != null)
          {
            (this.m_currPicture as WPicture).LayoutInCell = true;
            break;
          }
          if (this.m_currShape != null)
          {
            this.m_currShape.LayoutInCell = true;
            break;
          }
          this.m_currTextBox.TextBoxFormat.AllowInCell = true;
          break;
        }
        if (this.m_currPicture != null)
        {
          (this.m_currPicture as WPicture).LayoutInCell = false;
          break;
        }
        if (this.m_currShape != null)
        {
          this.m_currShape.LayoutInCell = false;
          break;
        }
        this.m_currTextBox.TextBoxFormat.AllowInCell = false;
        break;
      case "shprslt":
        this.m_bIsShapeResult = true;
        this.m_bShapeResultStackCount = this.m_pictureOrShapeStack.Count - 1;
        this.m_bIsShapePicture = true;
        break;
      case "nonshppict":
        this.m_bIsShapePicture = false;
        break;
      case "shppict":
        this.m_bIsShapePicture = true;
        break;
      case "shpwr":
        switch (Convert.ToInt32(tokenValue))
        {
          case 1:
            this.m_currShapeFormat.m_textWrappingStyle = TextWrappingStyle.TopAndBottom;
            return;
          case 2:
            this.m_currShapeFormat.m_textWrappingStyle = TextWrappingStyle.Square;
            return;
          case 3:
            this.m_currShapeFormat.m_textWrappingStyle = TextWrappingStyle.InFrontOfText;
            return;
          case 4:
            this.m_currShapeFormat.m_textWrappingStyle = TextWrappingStyle.Tight;
            return;
          case 5:
            this.m_currShapeFormat.m_textWrappingStyle = TextWrappingStyle.Through;
            return;
          case 6:
            this.m_currShapeFormat.m_textWrappingStyle = TextWrappingStyle.Behind;
            return;
          default:
            return;
        }
      case "shpwrk":
        switch (Convert.ToInt32(tokenValue))
        {
          case 0:
            this.m_currShapeFormat.m_textWrappingType = TextWrappingType.Both;
            return;
          case 1:
            this.m_currShapeFormat.m_textWrappingType = TextWrappingType.Left;
            return;
          case 2:
            this.m_currShapeFormat.m_textWrappingType = TextWrappingType.Right;
            return;
          case 3:
            this.m_currShapeFormat.m_textWrappingType = TextWrappingType.Largest;
            return;
          default:
            return;
        }
      case "shpbypara":
        this.m_currShapeFormat.m_vertOrgin = VerticalOrigin.Paragraph;
        break;
      case "shpbymargin":
        this.m_currShapeFormat.m_vertOrgin = VerticalOrigin.Margin;
        break;
      case "shpbypage":
        this.m_currShapeFormat.m_vertOrgin = VerticalOrigin.Page;
        break;
      case "shpbxpage":
        this.m_currShapeFormat.m_horizOrgin = HorizontalOrigin.Page;
        break;
      case "shpbxmargin":
        this.m_currShapeFormat.m_horizOrgin = HorizontalOrigin.Margin;
        break;
      case "shpbxcolumn":
        this.m_currShapeFormat.m_horizOrgin = HorizontalOrigin.Column;
        break;
      case "shpleft":
        this.m_currShapeFormat.m_horizPosition = this.ExtractTwipsValue(tokenValue);
        this.m_currShapeFormat.m_left = (float) this.GetIntValue(tokenValue);
        break;
      case "shpleft-":
        this.m_currShapeFormat.m_horizPosition = -this.ExtractTwipsValue(tokenValue);
        this.m_currShapeFormat.m_left = -(float) this.GetIntValue(tokenValue);
        break;
      case "shptop":
        this.m_currShapeFormat.m_vertPosition = this.ExtractTwipsValue(tokenValue);
        this.m_currShapeFormat.m_top = (float) this.GetIntValue(tokenValue);
        break;
      case "shptop-":
        this.m_currShapeFormat.m_vertPosition = -this.ExtractTwipsValue(tokenValue);
        this.m_currShapeFormat.m_top = -(float) this.GetIntValue(tokenValue);
        break;
      case "shpright":
        this.m_currShapeFormat.m_right = (float) this.GetIntValue(tokenValue);
        break;
      case "shpright-":
        this.m_currShapeFormat.m_right = -(float) this.GetIntValue(tokenValue);
        break;
      case "shpbottom":
        this.m_currShapeFormat.m_bottom = (float) this.GetIntValue(tokenValue);
        break;
      case "shpbottom-":
        this.m_currShapeFormat.m_bottom = -(float) this.GetIntValue(tokenValue);
        break;
      case "shpfblwtxt":
        if (Convert.ToInt32(tokenValue) == 1)
        {
          this.m_currShapeFormat.m_isBelowText = true;
          if (this.m_currShapeFormat.m_textWrappingStyle != TextWrappingStyle.Inline)
            break;
          this.m_currShapeFormat.m_textWrappingStyle = TextWrappingStyle.Behind;
          break;
        }
        this.m_currShapeFormat.m_isBelowText = false;
        if (this.m_currShapeFormat.m_textWrappingStyle != TextWrappingStyle.Inline)
          break;
        this.m_currShapeFormat.m_textWrappingStyle = TextWrappingStyle.InFrontOfText;
        break;
      case "shpfhdr":
        if (Convert.ToInt32(tokenValue) == 1)
        {
          this.m_currShapeFormat.m_isInHeader = true;
          break;
        }
        this.m_currShapeFormat.m_isInHeader = false;
        break;
      case "shplockanchor":
        this.m_currShapeFormat.m_isLockAnchor = true;
        break;
      case "shpz":
        this.m_currShapeFormat.m_zOrder = this.GetIntValue(tokenValue);
        this.m_picFormat.Zorder = this.GetIntValue(tokenValue);
        break;
      case "shplid":
        this.m_currShapeFormat.m_uniqueId = this.GetIntValue(tokenValue);
        break;
      case "shpinst":
        this.m_bIsShapeInstruction = true;
        this.m_shapeInstructionStack.Push("{");
        break;
      case "pWrapPolygonVertices":
        if (this.m_currPicture != null)
          (this.m_currPicture as WPicture).WrapPolygon.Vertices.Clear();
        else if (this.m_currShape != null)
          this.m_currShape.WrapFormat.WrapPolygon.Vertices.Clear();
        else
          this.m_currTextBox.TextBoxFormat.WrapPolygon.Vertices.Clear();
        string drawingFieldValue = this.m_drawingFieldValue;
        char[] chArray = new char[2]{ '(', ')' };
        foreach (string str in drawingFieldValue.Split(chArray))
        {
          if (str.Contains(","))
          {
            string[] strArray = str.Split(',');
            float x = float.Parse(strArray[0], (IFormatProvider) CultureInfo.InvariantCulture);
            float y = float.Parse(strArray[1], (IFormatProvider) CultureInfo.InvariantCulture);
            if (this.m_currPicture != null)
              (this.m_currPicture as WPicture).WrapPolygon.Vertices.Add(new PointF(x, y));
            else if (this.m_currShape != null)
              this.m_currShape.WrapFormat.WrapPolygon.Vertices.Add(new PointF(x, y));
            else
              this.m_currTextBox.TextBoxFormat.WrapPolygon.Vertices.Add(new PointF(x, y));
          }
        }
        this.m_drawingFieldValue = string.Empty;
        break;
      case "object":
        this.m_bIsObject = true;
        this.m_objectStack.Push("\\");
        break;
      case "hlfr":
      case "hlsrc":
        this.m_isImageHyperlink = true;
        break;
      case "fAllowOverlap":
        if (tokenValue == "1")
        {
          if (this.m_currPicture != null)
          {
            (this.m_currPicture as WPicture).AllowOverlap = true;
            break;
          }
          if (this.m_currShape != null)
          {
            this.m_currShape.WrapFormat.AllowOverlap = true;
            break;
          }
          this.m_currTextBox.TextBoxFormat.AllowOverlap = true;
          break;
        }
        if (this.m_currPicture != null)
        {
          (this.m_currPicture as WPicture).AllowOverlap = false;
          break;
        }
        if (this.m_currShape != null)
        {
          this.m_currShape.WrapFormat.AllowOverlap = false;
          break;
        }
        this.m_currTextBox.TextBoxFormat.AllowOverlap = false;
        break;
      case "shptxt":
        this.AddShapeTextbodyStack();
        this.ParseRowStart(true);
        this.ClearPreviousTextbody();
        this.m_bIsShapeText = true;
        this.m_bIsPictureOrShape = false;
        this.m_shapeTextStack.Push("{");
        break;
      default:
        this.ParseShapeToken(token, tokenValue);
        break;
    }
  }

  private void ParseShapeToken(string token, string tokenValue)
  {
    if ((token.ToLower().Contains("shadow") || token.Contains("3D")) && (this.m_currShape != null && this.m_currShape.EffectList.Count == 0 || this.m_currTextBox != null && this.m_currTextBox.Shape != null && this.m_currTextBox.Shape.EffectList.Count == 0))
    {
      EffectFormat effectFormat = new EffectFormat(this.m_currShape);
      effectFormat.IsEffectListItem = true;
      if (tokenValue != "0")
        effectFormat.IsShadowEffect = true;
      effectFormat.ShadowFormat.m_type = "outerShdw";
      effectFormat.ShadowFormat.ShadowOffset2X = 0.0f;
      effectFormat.ShadowFormat.ShadowOffset2Y = 0.0f;
      effectFormat.ShadowFormat.Color = Color.FromArgb((int) byte.MaxValue, 128 /*0x80*/, 128 /*0x80*/, 128 /*0x80*/);
      if (token.Contains("3D"))
        effectFormat.IsShadowEffect = false;
      if (this.m_currShape != null)
        this.m_currShape.EffectList.Add(effectFormat);
      else if (this.m_currTextBox.Shape != null)
        this.m_currTextBox.Shape.EffectList.Add(effectFormat);
    }
    if (token.Contains("3D") && (this.m_currShape != null && this.m_currShape.EffectList.Count == 1 || this.m_currTextBox != null && this.m_currTextBox.Shape != null && this.m_currTextBox.Shape.EffectList.Count == 1))
    {
      Shape shape = this.m_currShape == null ? this.m_currTextBox.Shape : this.m_currShape;
      EffectFormat effectFormat = new EffectFormat(shape);
      effectFormat.IsEffectListItem = true;
      effectFormat.IsShapeProperties = true;
      effectFormat.IsSceneProperties = true;
      if (tokenValue != "0")
      {
        shape.IsShapePropertiesInline = true;
        shape.IsScenePropertiesInline = true;
      }
      shape.EffectList.Add(effectFormat);
    }
    if (this.m_currShape == null && this.m_currTextBox == null)
      return;
    switch (token)
    {
      case "fPseudoInline":
        if (!(tokenValue == "1"))
          break;
        if (this.m_currShape != null)
        {
          this.m_currShape.WrapFormat.TextWrappingStyle = TextWrappingStyle.Inline;
          break;
        }
        this.m_currTextBox.TextBoxFormat.TextWrappingStyle = TextWrappingStyle.Inline;
        break;
      case "rotation":
        this.SetRotationValue(tokenValue);
        break;
      case "fFlipV":
        if (this.m_currShape != null)
        {
          this.m_currShape.FlipVertical = tokenValue == "1";
          break;
        }
        this.m_currTextBox.TextBoxFormat.FlipVertical = tokenValue == "1";
        break;
      case "fFlipH":
        if (this.m_currShape != null)
        {
          this.m_currShape.FlipHorizontal = tokenValue == "1";
          break;
        }
        this.m_currTextBox.TextBoxFormat.FlipHorizontal = tokenValue == "1";
        break;
      case "dxWrapDistLeft":
        double result1;
        double.TryParse(tokenValue, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result1);
        double num1 = Math.Round(result1 / 12700.0, 2);
        if (this.m_currShape != null)
        {
          this.m_currShape.WrapFormat.DistanceLeft = (float) num1;
          break;
        }
        this.m_currTextBox.TextBoxFormat.WrapDistanceLeft = (float) num1;
        break;
      case "dxWrapDistRight":
        double result2;
        double.TryParse(tokenValue, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result2);
        double num2 = Math.Round(result2 / 12700.0, 2);
        if (this.m_currShape != null)
        {
          this.m_currShape.WrapFormat.DistanceRight = (float) num2;
          break;
        }
        this.m_currTextBox.TextBoxFormat.WrapDistanceRight = (float) num2;
        break;
      case "dyWrapDistTop":
        double result3;
        double.TryParse(tokenValue, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result3);
        double num3 = Math.Round(result3 / 12700.0, 2);
        if (this.m_currShape != null)
        {
          this.m_currShape.WrapFormat.DistanceTop = (float) num3;
          break;
        }
        this.m_currTextBox.TextBoxFormat.WrapDistanceTop = (float) num3;
        break;
      case "dyWrapDistBottom":
        double result4;
        double.TryParse(tokenValue, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result4);
        double num4 = Math.Round(result4 / 12700.0, 2);
        if (this.m_currShape != null)
        {
          this.m_currShape.WrapFormat.DistanceBottom = (float) num4;
          break;
        }
        this.m_currTextBox.TextBoxFormat.WrapDistanceBottom = (float) num4;
        break;
      case "fBehindDocument":
        if (!(tokenValue == "1"))
          break;
        if (this.m_currShape != null && this.m_currShape.WrapFormat.TextWrappingStyle == TextWrappingStyle.InFrontOfText)
        {
          this.m_currShape.WrapFormat.TextWrappingStyle = TextWrappingStyle.Behind;
          break;
        }
        if (this.m_currTextBox == null || this.m_currTextBox.TextBoxFormat.TextWrappingStyle != TextWrappingStyle.InFrontOfText)
          break;
        this.m_currTextBox.TextBoxFormat.TextWrappingStyle = TextWrappingStyle.Behind;
        break;
      case "fEditedWrap":
        if (this.m_currShape != null)
        {
          this.m_currShape.WrapFormat.WrapPolygon.Edited = tokenValue == "1";
          break;
        }
        this.m_currTextBox.TextBoxFormat.WrapPolygon.Edited = tokenValue == "1";
        break;
      case "fHidden":
        if (!(tokenValue == "1"))
          break;
        if (this.m_currShape != null)
        {
          this.m_currShape.Visible = false;
          break;
        }
        this.m_currTextBox.Visible = false;
        break;
      case "pctVert":
        float result5 = float.MaxValue;
        float.TryParse(tokenValue, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result5);
        if ((double) result5 == 3.4028234663852886E+38)
          break;
        if (this.m_currShape != null)
        {
          this.m_currShape.IsRelativeHeight = true;
          this.m_currShape.RelativeHeight = (float) Math.Round((double) result5 / 10.0, 2);
          this.m_currShape.RelativeHeightVerticalOrigin = VerticalOrigin.Page;
          break;
        }
        this.m_currTextBox.TextBoxFormat.HeightRelativePercent = (float) Math.Round((double) result5 / 10.0, 2);
        this.m_currTextBox.TextBoxFormat.HeightOrigin = HeightOrigin.Page;
        break;
      case "pctHoriz":
        float result6 = float.MaxValue;
        float.TryParse(tokenValue, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result6);
        if ((double) result6 == 3.4028234663852886E+38)
          break;
        if (this.m_currShape != null)
        {
          this.m_currShape.IsRelativeWidth = true;
          this.m_currShape.RelativeWidth = (float) Math.Round((double) result6 / 10.0, 2);
          this.m_currShape.RelativeWidthHorizontalOrigin = HorizontalOrigin.Page;
          break;
        }
        this.m_currTextBox.TextBoxFormat.WidthRelativePercent = (float) Math.Round((double) result6 / 10.0, 2);
        this.m_currTextBox.TextBoxFormat.WidthOrigin = WidthOrigin.Page;
        break;
      case "sizerelh":
        if (this.m_currShape != null)
          this.m_currShape.IsRelativeWidth = true;
        switch (tokenValue)
        {
          case null:
            return;
          case "0":
            if (this.m_currShape != null)
            {
              this.m_currShape.RelativeWidthHorizontalOrigin = HorizontalOrigin.Margin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.WidthOrigin = WidthOrigin.Margin;
            return;
          case "1":
            if (this.m_currShape != null)
            {
              this.m_currShape.RelativeWidthHorizontalOrigin = HorizontalOrigin.Page;
              return;
            }
            this.m_currTextBox.TextBoxFormat.WidthOrigin = WidthOrigin.Margin;
            return;
          case "2":
            if (this.m_currShape != null)
            {
              this.m_currShape.RelativeWidthHorizontalOrigin = HorizontalOrigin.LeftMargin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.WidthOrigin = WidthOrigin.LeftMargin;
            return;
          case "3":
            if (this.m_currShape != null)
            {
              this.m_currShape.RelativeWidthHorizontalOrigin = HorizontalOrigin.RightMargin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.WidthOrigin = WidthOrigin.RightMargin;
            return;
          case "4":
            if (this.m_currShape != null)
            {
              this.m_currShape.RelativeWidthHorizontalOrigin = HorizontalOrigin.InsideMargin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.WidthOrigin = WidthOrigin.InsideMargin;
            return;
          case "5":
            if (this.m_currShape != null)
            {
              this.m_currShape.RelativeWidthHorizontalOrigin = HorizontalOrigin.OutsideMargin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.WidthOrigin = WidthOrigin.OutsideMargin;
            return;
          default:
            return;
        }
      case "sizerelv":
        if (this.m_currShape != null)
          this.m_currShape.IsRelativeHeight = true;
        switch (tokenValue)
        {
          case null:
            return;
          case "0":
            if (this.m_currShape != null)
            {
              this.m_currShape.RelativeHeightVerticalOrigin = VerticalOrigin.Margin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HeightOrigin = HeightOrigin.Margin;
            return;
          case "1":
            if (this.m_currShape != null)
            {
              this.m_currShape.RelativeHeightVerticalOrigin = VerticalOrigin.Page;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HeightOrigin = HeightOrigin.Page;
            return;
          case "2":
            if (this.m_currShape != null)
            {
              this.m_currShape.RelativeHeightVerticalOrigin = VerticalOrigin.TopMargin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HeightOrigin = HeightOrigin.TopMargin;
            return;
          case "3":
            if (this.m_currShape != null)
            {
              this.m_currShape.RelativeHeightVerticalOrigin = VerticalOrigin.BottomMargin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HeightOrigin = HeightOrigin.BottomMargin;
            return;
          case "4":
            if (this.m_currShape != null)
            {
              this.m_currShape.RelativeHeightVerticalOrigin = VerticalOrigin.InsideMargin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HeightOrigin = HeightOrigin.InsideMargin;
            return;
          case "5":
            if (this.m_currShape != null)
            {
              this.m_currShape.RelativeHeightVerticalOrigin = VerticalOrigin.OutsideMargin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HeightOrigin = HeightOrigin.OutsideMargin;
            return;
          default:
            return;
        }
      case "pctHorizPos":
        float result7 = float.MaxValue;
        float.TryParse(tokenValue, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result7);
        if (this.m_currShape != null)
        {
          this.m_currShape.IsRelativeHorizontalPosition = true;
          if ((double) result7 != 3.4028234663852886E+38)
            this.m_currShape.RelativeHorizontalPosition = (float) Math.Round((double) result7 / 10.0, 2);
          this.m_currShape.RelativeHorizontalOrigin = this.m_currShape.HorizontalOrigin;
          break;
        }
        if (this.m_currTextBox == null && this.m_currTextBox.Shape == null)
          break;
        this.m_currTextBox.IsShape = true;
        this.m_currTextBox.Shape.IsRelativeHorizontalPosition = true;
        if ((double) result7 != 3.4028234663852886E+38)
          this.m_currTextBox.Shape.RelativeHorizontalPosition = (float) Math.Round((double) result7 / 10.0, 2);
        this.m_currTextBox.Shape.RelativeHorizontalOrigin = this.m_currTextBox.TextBoxFormat.HorizontalOrigin;
        break;
      case "pctVertPos":
        float result8 = float.MaxValue;
        float.TryParse(tokenValue, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result8);
        if (this.m_currShape != null)
        {
          this.m_currShape.IsRelativeVerticalPosition = true;
          if ((double) result8 != 3.4028234663852886E+38)
            this.m_currShape.RelativeVerticalPosition = (float) Math.Round((double) result8 / 10.0, 2);
          this.m_currShape.RelativeVerticalOrigin = this.m_currShape.VerticalOrigin;
          break;
        }
        this.m_currTextBox.IsShape = true;
        this.m_currTextBox.Shape.IsRelativeVerticalPosition = true;
        if ((double) result8 != 3.4028234663852886E+38)
          this.m_currTextBox.Shape.RelativeVerticalPosition = (float) Math.Round((double) result8 / 10.0, 2);
        this.m_currTextBox.Shape.RelativeVerticalOrigin = this.m_currTextBox.TextBoxFormat.VerticalOrigin;
        break;
      case "lineEndCapStyle":
        if (this.m_currTextBox != null && this.m_currTextBox.Shape != null)
          this.m_currTextBox.IsShape = true;
        switch (tokenValue)
        {
          case null:
            return;
          case "0":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.LineCap = LineCap.Round;
              return;
            }
            if (this.m_currTextBox.Shape == null)
              return;
            this.m_currTextBox.Shape.LineFormat.LineCap = LineCap.Round;
            return;
          case "1":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.LineCap = LineCap.Square;
              return;
            }
            if (this.m_currTextBox.Shape == null)
              return;
            this.m_currTextBox.Shape.LineFormat.LineCap = LineCap.Square;
            return;
          case "2":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.LineCap = LineCap.Flat;
              return;
            }
            if (this.m_currTextBox.Shape == null)
              return;
            this.m_currTextBox.Shape.LineFormat.LineCap = LineCap.Flat;
            return;
          default:
            return;
        }
      case "lineEndArrowWidth":
        if (this.m_currShape == null)
          break;
        switch (tokenValue)
        {
          case null:
            return;
          case "0":
            this.m_currShape.LineFormat.EndArrowheadWidth = LineEndWidth.NarrowArrow;
            return;
          case "1":
            this.m_currShape.LineFormat.EndArrowheadWidth = LineEndWidth.MediumWidthArrow;
            return;
          case "2":
            this.m_currShape.LineFormat.EndArrowheadWidth = LineEndWidth.WideArrow;
            return;
          default:
            return;
        }
      case "lineEndArrowLength":
        if (this.m_currShape == null)
          break;
        switch (tokenValue)
        {
          case null:
            return;
          case "0":
            this.m_currShape.LineFormat.EndArrowheadLength = LineEndLength.ShortArrow;
            return;
          case "1":
            this.m_currShape.LineFormat.EndArrowheadLength = LineEndLength.MediumLenArrow;
            return;
          case "2":
            this.m_currShape.LineFormat.EndArrowheadLength = LineEndLength.LongArrow;
            return;
          default:
            return;
        }
      case "lineStartArrowhead":
        if (this.m_currShape == null)
          break;
        switch (tokenValue)
        {
          case null:
            return;
          case "0":
            this.m_currShape.LineFormat.BeginArrowheadStyle = ArrowheadStyle.ArrowheadNone;
            return;
          case "1":
            this.m_currShape.LineFormat.BeginArrowheadStyle = ArrowheadStyle.ArrowheadTriangle;
            return;
          case "2":
            this.m_currShape.LineFormat.BeginArrowheadStyle = ArrowheadStyle.ArrowheadStealth;
            return;
          case "3":
            this.m_currShape.LineFormat.BeginArrowheadStyle = ArrowheadStyle.ArrowheadDiamond;
            return;
          case "4":
            this.m_currShape.LineFormat.BeginArrowheadStyle = ArrowheadStyle.ArrowheadOval;
            return;
          case "5":
            this.m_currShape.LineFormat.BeginArrowheadStyle = ArrowheadStyle.ArrowheadOpen;
            return;
          default:
            return;
        }
      case "lineEndArrowhead":
        if (this.m_currShape == null)
          break;
        switch (tokenValue)
        {
          case null:
            return;
          case "0":
            this.m_currShape.LineFormat.EndArrowheadStyle = ArrowheadStyle.ArrowheadNone;
            return;
          case "1":
            this.m_currShape.LineFormat.EndArrowheadStyle = ArrowheadStyle.ArrowheadTriangle;
            return;
          case "2":
            this.m_currShape.LineFormat.EndArrowheadStyle = ArrowheadStyle.ArrowheadStealth;
            return;
          case "3":
            this.m_currShape.LineFormat.EndArrowheadStyle = ArrowheadStyle.ArrowheadDiamond;
            return;
          case "4":
            this.m_currShape.LineFormat.EndArrowheadStyle = ArrowheadStyle.ArrowheadOval;
            return;
          case "5":
            this.m_currShape.LineFormat.EndArrowheadStyle = ArrowheadStyle.ArrowheadOpen;
            return;
          default:
            return;
        }
      case "lineStartArrowWidth":
        switch (tokenValue)
        {
          case null:
            return;
          case "0":
            this.m_currShape.LineFormat.BeginArrowheadWidth = LineEndWidth.NarrowArrow;
            return;
          case "1":
            this.m_currShape.LineFormat.BeginArrowheadWidth = LineEndWidth.MediumWidthArrow;
            return;
          case "2":
            this.m_currShape.LineFormat.BeginArrowheadWidth = LineEndWidth.WideArrow;
            return;
          default:
            return;
        }
      case "lineStartArrowLength":
        if (this.m_currShape == null)
          break;
        switch (tokenValue)
        {
          case null:
            return;
          case "0":
            this.m_currShape.LineFormat.BeginArrowheadLength = LineEndLength.ShortArrow;
            return;
          case "1":
            this.m_currShape.LineFormat.BeginArrowheadLength = LineEndLength.MediumLenArrow;
            return;
          case "2":
            this.m_currShape.LineFormat.BeginArrowheadLength = LineEndLength.LongArrow;
            return;
          default:
            return;
        }
      case "alignHR":
        switch (tokenValue)
        {
          case null:
            return;
          case "0":
            if (this.m_currShape != null)
            {
              this.m_currShape.HorizontalAlignment = ShapeHorizontalAlignment.Left;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HorizontalAlignment = ShapeHorizontalAlignment.Left;
            return;
          case "1":
            if (this.m_currShape != null)
            {
              this.m_currShape.HorizontalAlignment = ShapeHorizontalAlignment.Center;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HorizontalAlignment = ShapeHorizontalAlignment.Center;
            return;
          case "2":
            if (this.m_currShape != null)
            {
              this.m_currShape.HorizontalAlignment = ShapeHorizontalAlignment.Right;
              return;
            }
            this.m_currTextBox.TextBoxFormat.HorizontalAlignment = ShapeHorizontalAlignment.Right;
            return;
          default:
            return;
        }
      case "fHorizRule":
        if (this.m_currShape != null)
        {
          this.m_currShape.IsHorizontalRule = tokenValue == "1";
          break;
        }
        this.m_currTextBox.Shape.IsHorizontalRule = tokenValue == "1";
        break;
      case "fStandardHR":
        if (this.m_currShape != null)
        {
          this.m_currShape.UseStandardColorHR = tokenValue == "1";
          break;
        }
        this.m_currTextBox.Shape.UseStandardColorHR = tokenValue == "1";
        break;
      case "fNoShadeHR":
        if (this.m_currShape != null)
        {
          this.m_currShape.UseNoShadeHR = tokenValue == "1";
          break;
        }
        this.m_currTextBox.Shape.UseNoShadeHR = tokenValue == "1";
        break;
      case "pctHR":
        float num5 = (float) Math.Round(double.Parse(tokenValue) / 10.0);
        if ((double) num5 == 0.0)
          break;
        if (this.m_currShape != null)
        {
          this.m_currShape.WidthScale = num5;
          break;
        }
        this.m_currTextBox.Shape.WidthScale = num5;
        break;
      case "fillType":
        if (this.m_currShape != null)
          this.m_currShape.FillFormat.Fill = true;
        else
          this.m_currTextBox.Shape.FillFormat.Fill = true;
        switch (tokenValue)
        {
          case "0":
            if (this.m_currShape != null)
            {
              this.m_currShape.FillFormat.FillType = FillType.FillSolid;
              break;
            }
            this.m_currTextBox.Shape.FillFormat.FillType = FillType.FillSolid;
            break;
          case "1":
            if (this.m_currShape != null)
            {
              this.m_currShape.FillFormat.FillType = FillType.FillPatterned;
              break;
            }
            this.m_currTextBox.Shape.FillFormat.FillType = FillType.FillPatterned;
            break;
          case "2":
            if (this.m_currShape != null)
            {
              this.m_currShape.FillFormat.FillType = FillType.FillTextured;
              break;
            }
            this.m_currTextBox.Shape.FillFormat.FillType = FillType.FillTextured;
            break;
          case "3":
            if (this.m_currShape != null)
            {
              this.m_currShape.FillFormat.FillType = FillType.FillPicture;
              break;
            }
            this.m_currTextBox.Shape.FillFormat.FillType = FillType.FillPicture;
            break;
          case "5":
            if (this.m_currShape != null)
            {
              this.m_currShape.FillFormat.FillType = FillType.FillGradient;
              break;
            }
            this.m_currTextBox.Shape.FillFormat.FillType = FillType.FillGradient;
            break;
          case "6":
            if (this.m_currShape != null)
            {
              this.m_currShape.FillFormat.FillType = FillType.FillGradient;
              break;
            }
            this.m_currTextBox.Shape.FillFormat.FillType = FillType.FillGradient;
            break;
          case "7":
            if (this.m_currShape != null)
            {
              this.m_currShape.FillFormat.FillType = FillType.FillGradient;
              break;
            }
            this.m_currTextBox.Shape.FillFormat.FillType = FillType.FillGradient;
            break;
          case "9":
            if (this.m_currShape != null)
            {
              this.m_currShape.FillFormat.FillType = FillType.FillBackground;
              break;
            }
            this.m_currTextBox.Shape.FillFormat.FillType = FillType.FillBackground;
            break;
        }
        if (this.m_currShape != null)
        {
          this.m_currShape.FillFormat.FillType = (FillType) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.FillFormat.FillType = (FillType) this.GetIntValue(tokenValue);
        break;
      case "fillColor":
        if (this.m_currShape != null)
        {
          this.m_currShape.FillFormat.Fill = true;
          this.m_currShape.FillFormat.IsDefaultFill = false;
          this.m_currShape.IsFillStyleInline = true;
          this.m_currShape.FillFormat.Color = Color.FromArgb(int.Parse(tokenValue));
          this.m_currShape.FillFormat.Color = Color.FromArgb((int) this.m_currShape.FillFormat.Color.B, (int) this.m_currShape.FillFormat.Color.G, (int) this.m_currShape.FillFormat.Color.R);
          break;
        }
        this.m_currTextBox.TextBoxFormat.FillColor = Color.FromArgb(int.Parse(tokenValue));
        this.m_currTextBox.TextBoxFormat.FillColor = Color.FromArgb((int) this.m_currTextBox.TextBoxFormat.FillColor.B, (int) this.m_currTextBox.TextBoxFormat.FillColor.G, (int) this.m_currTextBox.TextBoxFormat.FillColor.R);
        break;
      case "fillOpacity":
        if (this.m_currShape != null)
        {
          this.m_currShape.FillFormat.Transparency = (float) Math.Round(1.0 - (double) float.Parse(tokenValue) / 65536.0, 2) * 100f;
          break;
        }
        if (this.m_currTextBox == null || this.m_currTextBox.Shape == null)
          break;
        this.m_currTextBox.IsShape = true;
        this.m_currTextBox.Shape.FillFormat.Transparency = (float) Math.Round(1.0 - (double) float.Parse(tokenValue) / 65536.0, 2) * 100f;
        break;
      case "fillBackOpacity":
        if (this.m_currShape != null)
        {
          this.m_currShape.FillFormat.SecondaryOpacity = (float) Math.Round(1.0 - (double) float.Parse(tokenValue) / 65536.0, 2) * 100f;
          break;
        }
        this.m_currTextBox.Shape.FillFormat.SecondaryOpacity = (float) Math.Round(1.0 - (double) float.Parse(tokenValue) / 65536.0, 2) * 100f;
        break;
      case "fillBackColor":
        if (this.m_currShape != null)
        {
          this.m_currShape.FillFormat.ForeColor = Color.FromArgb(this.GetIntValue(tokenValue));
          break;
        }
        this.m_currTextBox.Shape.FillFormat.ForeColor = Color.FromArgb(this.GetIntValue(tokenValue));
        break;
      case "lineStyle":
        switch (tokenValue)
        {
          case null:
            return;
          case "0":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.Style = LineStyle.Single;
              return;
            }
            this.m_currTextBox.TextBoxFormat.LineStyle = TextBoxLineStyle.Simple;
            return;
          case "1":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.Style = LineStyle.ThinThin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.LineStyle = TextBoxLineStyle.Double;
            return;
          case "2":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.Style = LineStyle.ThickThin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.LineStyle = TextBoxLineStyle.ThickThin;
            return;
          case "3":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.Style = LineStyle.ThinThick;
              return;
            }
            this.m_currTextBox.TextBoxFormat.LineStyle = TextBoxLineStyle.ThinThick;
            return;
          case "4":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.Style = LineStyle.ThickBetweenThin;
              return;
            }
            this.m_currTextBox.TextBoxFormat.LineStyle = TextBoxLineStyle.Triple;
            return;
          default:
            return;
        }
      case "lineDashing":
        switch (tokenValue)
        {
          case "0":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.DashStyle = LineDashing.Solid;
              return;
            }
            this.m_currTextBox.TextBoxFormat.LineDashing = LineDashing.Solid;
            return;
          case "1":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.DashStyle = LineDashing.Dash;
              return;
            }
            this.m_currTextBox.TextBoxFormat.LineDashing = LineDashing.Dash;
            return;
          case "2":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.DashStyle = LineDashing.Dot;
              return;
            }
            this.m_currTextBox.IsShape = true;
            this.m_currTextBox.TextBoxFormat.LineDashing = LineDashing.Dot;
            return;
          case "3":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.DashStyle = LineDashing.DashDotGEL;
              return;
            }
            this.m_currTextBox.TextBoxFormat.LineDashing = LineDashing.DashDotGEL;
            return;
          case "4":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.DashStyle = LineDashing.DashDotDot;
              return;
            }
            this.m_currTextBox.TextBoxFormat.LineDashing = LineDashing.DashDotDot;
            return;
          case "5":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.DashStyle = LineDashing.DotGEL;
              return;
            }
            this.m_currTextBox.TextBoxFormat.LineDashing = LineDashing.DotGEL;
            return;
          case "6":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.DashStyle = LineDashing.DashGEL;
              return;
            }
            this.m_currTextBox.TextBoxFormat.LineDashing = LineDashing.DashGEL;
            return;
          case "7":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.DashStyle = LineDashing.LongDashGEL;
              return;
            }
            this.m_currTextBox.TextBoxFormat.LineDashing = LineDashing.LongDashGEL;
            return;
          case "8":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.DashStyle = LineDashing.DashDotGEL;
              return;
            }
            this.m_currTextBox.TextBoxFormat.LineDashing = LineDashing.DashDotGEL;
            return;
          case "9":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.DashStyle = LineDashing.LongDashDotGEL;
              return;
            }
            this.m_currTextBox.TextBoxFormat.LineDashing = LineDashing.LongDashDotGEL;
            return;
          case "10":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.DashStyle = LineDashing.LongDashDotDotGEL;
              return;
            }
            this.m_currTextBox.TextBoxFormat.LineDashing = LineDashing.LongDashDotDotGEL;
            return;
          case null:
            return;
          default:
            return;
        }
      case "fLine":
        if (this.m_currShape != null)
        {
          this.m_currShape.LineFormat.Line = tokenValue == "1";
          if (!this.m_currShape.LineFormat.Line)
            break;
          this.m_currShape.LineFormat.Color = Color.Empty;
          break;
        }
        this.m_currTextBox.TextBoxFormat.NoLine = !(tokenValue == "1");
        if (!this.m_currTextBox.TextBoxFormat.NoLine || this.m_currTextBox.Shape == null)
          break;
        this.m_currTextBox.IsShape = true;
        this.m_currTextBox.TextBoxFormat.LineColor = Color.Empty;
        this.m_currTextBox.Shape.LineFormat.Color = Color.Empty;
        break;
      case "lineBackColor":
        if (this.m_currShape != null)
        {
          this.m_currShape.LineFormat.ForeColor = Color.FromArgb(this.GetIntValue(tokenValue));
          break;
        }
        this.m_currTextBox.Shape.LineFormat.ForeColor = Color.FromArgb(this.GetIntValue(tokenValue));
        break;
      case "lineColor":
        if (this.m_currShape != null)
        {
          this.m_currShape.LineFormat.Color = Color.FromArgb(int.Parse(tokenValue));
          this.m_currShape.LineFormat.Color = Color.FromArgb((int) this.m_currShape.LineFormat.Color.B, (int) this.m_currShape.LineFormat.Color.G, (int) this.m_currShape.LineFormat.Color.R);
          break;
        }
        this.m_currTextBox.TextBoxFormat.LineColor = Color.FromArgb(this.GetIntValue(tokenValue));
        this.m_currTextBox.TextBoxFormat.LineColor = Color.FromArgb((int) this.m_currTextBox.TextBoxFormat.LineColor.B, (int) this.m_currTextBox.TextBoxFormat.LineColor.G, (int) this.m_currTextBox.TextBoxFormat.LineColor.R);
        break;
      case "lineType":
        switch (tokenValue)
        {
          case null:
            return;
          case "0":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.LineFormatType = LineFormatType.Solid;
              return;
            }
            this.m_currTextBox.Shape.LineFormat.LineFormatType = LineFormatType.Solid;
            return;
          case "1":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.LineFormatType = LineFormatType.Patterned;
              return;
            }
            this.m_currTextBox.Shape.LineFormat.LineFormatType = LineFormatType.Patterned;
            return;
          case "2":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.LineFormatType = LineFormatType.Gradient;
              return;
            }
            this.m_currTextBox.Shape.LineFormat.LineFormatType = LineFormatType.Gradient;
            return;
          case "3":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.LineFormatType = LineFormatType.Gradient;
              return;
            }
            this.m_currTextBox.Shape.LineFormat.LineFormatType = LineFormatType.Gradient;
            return;
          default:
            return;
        }
      case "lineWidth":
        double result9;
        double.TryParse(tokenValue, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result9);
        double num6 = Math.Round(result9 / 12700.0, 2);
        if (this.m_currShape != null)
        {
          this.m_currShape.LineFormat.Weight = (float) num6;
          if (this.m_currShape.LineFormat.LineFormatType != ~(LineFormatType.None | LineFormatType.Solid))
            break;
          this.m_currShape.LineFormat.LineFormatType = LineFormatType.Solid;
          break;
        }
        this.m_currTextBox.TextBoxFormat.LineWidth = (float) num6;
        break;
      case "lineJoinStyle":
        switch (tokenValue)
        {
          case null:
            return;
          case "0":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.LineJoin = LineJoin.Bevel;
              return;
            }
            this.m_currTextBox.Shape.LineFormat.LineJoin = LineJoin.Bevel;
            return;
          case "1":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.LineJoin = LineJoin.Miter;
              return;
            }
            this.m_currTextBox.Shape.LineFormat.LineJoin = LineJoin.Miter;
            return;
          case "2":
            if (this.m_currShape != null)
            {
              this.m_currShape.LineFormat.LineJoin = LineJoin.Round;
              return;
            }
            this.m_currTextBox.Shape.LineFormat.LineJoin = LineJoin.Round;
            return;
          default:
            return;
        }
      case "lineMiterLimit":
        if (this.m_currShape != null)
        {
          this.m_currShape.LineFormat.MiterJoinLimit = tokenValue;
          break;
        }
        this.m_currTextBox.Shape.LineFormat.MiterJoinLimit = tokenValue;
        break;
      case "lineOpacity":
        if (this.m_currShape == null)
          break;
        this.m_currShape.LineFormat.Transparency = (float) this.GetIntValue(tokenValue);
        break;
      case "fillFocus":
        if (this.m_currShape != null)
        {
          this.m_currShape.FillFormat.Focus = float.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.FillFormat.Focus = float.Parse(tokenValue);
        break;
      case "shadowType":
        switch (tokenValue)
        {
          case "1":
            if (this.m_currShape != null)
            {
              this.m_currShape.EffectList[0].ShadowFormat.ShadowType = ShadowType.Double;
              return;
            }
            this.m_currTextBox.Shape.EffectList[0].ShadowFormat.ShadowType = ShadowType.Double;
            return;
          case "2":
            if (this.m_currShape != null)
            {
              this.m_currShape.EffectList[0].ShadowFormat.ShadowType = ShadowType.Perspective;
              return;
            }
            this.m_currTextBox.Shape.EffectList[0].ShadowFormat.ShadowType = ShadowType.Perspective;
            return;
          case "3":
            if (this.m_currShape != null)
            {
              this.m_currShape.EffectList[0].ShadowFormat.ShadowType = ShadowType.ShapeRelative;
              return;
            }
            this.m_currTextBox.Shape.EffectList[0].ShadowFormat.ShadowType = ShadowType.ShapeRelative;
            return;
          case "4":
            if (this.m_currShape != null)
            {
              this.m_currShape.EffectList[0].ShadowFormat.ShadowType = ShadowType.DrawingRelative;
              return;
            }
            this.m_currTextBox.Shape.EffectList[0].ShadowFormat.ShadowType = ShadowType.DrawingRelative;
            return;
          case "5":
            if (this.m_currShape != null)
            {
              this.m_currShape.EffectList[0].ShadowFormat.ShadowType = ShadowType.Emboss;
              return;
            }
            this.m_currTextBox.Shape.EffectList[0].ShadowFormat.ShadowType = ShadowType.Emboss;
            return;
          default:
            if (this.m_currShape != null)
            {
              this.m_currShape.EffectList[0].ShadowFormat.ShadowType = ShadowType.Single;
              return;
            }
            this.m_currTextBox.Shape.EffectList[0].ShadowFormat.ShadowType = ShadowType.Single;
            return;
        }
      case "shadowColor":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[0].ShadowFormat.Color = Color.FromArgb(int.Parse(tokenValue));
          this.m_currShape.EffectList[0].ShadowFormat.Color = Color.FromArgb((int) this.m_currShape.EffectList[0].ShadowFormat.Color.B, (int) this.m_currShape.EffectList[0].ShadowFormat.Color.G, (int) this.m_currShape.EffectList[0].ShadowFormat.Color.R);
          break;
        }
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.Color = Color.FromArgb(int.Parse(tokenValue));
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.Color = Color.FromArgb((int) this.m_currShape.EffectList[0].ShadowFormat.Color.B, (int) this.m_currShape.EffectList[0].ShadowFormat.Color.G, (int) this.m_currShape.EffectList[0].ShadowFormat.Color.R);
        break;
      case "shadowHighlight":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[0].ShadowFormat.Color2 = Color.FromArgb(int.Parse(tokenValue));
          this.m_currShape.EffectList[0].ShadowFormat.Color2 = Color.FromArgb((int) this.m_currShape.EffectList[0].ShadowFormat.Color2.B, (int) this.m_currShape.EffectList[0].ShadowFormat.Color2.G, (int) this.m_currShape.EffectList[0].ShadowFormat.Color2.R);
          break;
        }
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.Color2 = Color.FromArgb(int.Parse(tokenValue));
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.Color2 = Color.FromArgb((int) this.m_currShape.EffectList[0].ShadowFormat.Color2.B, (int) this.m_currShape.EffectList[0].ShadowFormat.Color2.G, (int) this.m_currShape.EffectList[0].ShadowFormat.Color2.R);
        break;
      case "shadowOpacity":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[0].ShadowFormat.Transparency = 0.5f;
          break;
        }
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.Transparency = 0.5f;
        break;
      case "shadowOffsetX":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[0].ShadowFormat.ShadowOffsetX = (float) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.ShadowOffsetX = (float) this.GetIntValue(tokenValue);
        break;
      case "shadowOffsetY":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[0].ShadowFormat.ShadowOffsetY = (float) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.ShadowOffsetY = (float) this.GetIntValue(tokenValue);
        break;
      case "shadowSecondOffsetX":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[0].ShadowFormat.ShadowOffset2X = (float) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.ShadowOffset2X = (float) this.GetIntValue(tokenValue);
        break;
      case "shadowSecondOffsetY":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[0].ShadowFormat.ShadowOffset2Y = (float) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.ShadowOffset2Y = (float) this.GetIntValue(tokenValue);
        break;
      case "shadowOriginX":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[0].ShadowFormat.OriginX = (float) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.OriginX = (float) this.GetIntValue(tokenValue);
        break;
      case "shadowOriginY":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[0].ShadowFormat.OriginY = (float) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.OriginY = (float) this.GetIntValue(tokenValue);
        break;
      case "fShadow":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[0].ShadowFormat.Visible = tokenValue == "1";
          break;
        }
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.Visible = tokenValue == "1";
        break;
      case "fshadowObscured":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[0].ShadowFormat.Obscured = tokenValue == "1";
          break;
        }
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.Obscured = tokenValue == "1";
        break;
      case "fillRectRight":
        float result10 = 0.0f;
        float.TryParse(tokenValue, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result10);
        if (this.m_currShape != null)
        {
          this.m_currShape.FillFormat.FillRectangle.RightOffset = result10;
          break;
        }
        this.m_currTextBox.Shape.FillFormat.FillRectangle.RightOffset = result10;
        break;
      case "fillRectBottom":
        float result11 = 0.0f;
        float.TryParse(tokenValue, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result11);
        if (this.m_currShape != null)
        {
          this.m_currShape.FillFormat.FillRectangle.BottomOffset = result11;
          break;
        }
        this.m_currTextBox.Shape.FillFormat.FillRectangle.BottomOffset = result11;
        break;
      case "fillRectLeft":
        float result12 = 0.0f;
        float.TryParse(tokenValue, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result12);
        if (this.m_currShape != null)
        {
          this.m_currShape.FillFormat.FillRectangle.LeftOffset = result12;
          break;
        }
        this.m_currTextBox.Shape.FillFormat.FillRectangle.LeftOffset = result12;
        break;
      case "fillRectTop":
        float result13 = 0.0f;
        float.TryParse(tokenValue, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out result13);
        if (this.m_currShape != null)
        {
          this.m_currShape.FillFormat.FillRectangle.TopOffset = result13;
          break;
        }
        this.m_currTextBox.Shape.FillFormat.FillRectangle.TopOffset = result13;
        break;
      case "fRecolorFillAsPicture":
        if (this.m_currShape != null)
        {
          this.m_currShape.FillFormat.ReColor = tokenValue == "1";
          break;
        }
        this.m_currTextBox.Shape.FillFormat.ReColor = tokenValue == "1";
        break;
      case "dxTextLeft":
        if (this.m_currTextBox == null)
          break;
        this.m_currTextBox.TextBoxFormat.InternalMargin.Left = (float) Math.Round((double) float.Parse(tokenValue) / 12700.0, 2);
        break;
      case "dyTextTop":
        if (this.m_currTextBox == null)
          break;
        this.m_currTextBox.TextBoxFormat.InternalMargin.Top = (float) Math.Round((double) float.Parse(tokenValue) / 12700.0, 2);
        break;
      case "dxTextRight":
        if (this.m_currTextBox == null)
          break;
        this.m_currTextBox.TextBoxFormat.InternalMargin.Right = (float) Math.Round((double) float.Parse(tokenValue) / 12700.0, 2);
        break;
      case "dyTextBottom":
        if (this.m_currTextBox == null)
          break;
        this.m_currTextBox.TextBoxFormat.InternalMargin.Bottom = (float) Math.Round((double) float.Parse(tokenValue) / 12700.0, 2);
        break;
      case "WrapText":
        if (this.m_currTextBox == null)
          break;
        switch (tokenValue)
        {
          case null:
            return;
          case "0":
            this.m_currTextBox.TextBoxFormat.TextWrappingStyle = TextWrappingStyle.Square;
            return;
          case "1":
            this.m_currTextBox.TextBoxFormat.TextWrappingStyle = TextWrappingStyle.Tight;
            return;
          case "2":
            if (this.m_currTextBox.Shape == null)
              return;
            this.m_currTextBox.IsShape = true;
            this.m_currTextBox.Shape.TextFrame.NoWrap = true;
            return;
          case "3":
            this.m_currTextBox.TextBoxFormat.TextWrappingStyle = TextWrappingStyle.TopAndBottom;
            return;
          case "4":
            this.m_currTextBox.TextBoxFormat.TextWrappingStyle = TextWrappingStyle.Through;
            return;
          default:
            return;
        }
      case "c3DSpecularAmt":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.Specularity = (float) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.Specularity = (float) this.GetIntValue(tokenValue);
        break;
      case "c3DDiffuseAmt":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.Diffusity = (float) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.Diffusity = (float) this.GetIntValue(tokenValue);
        break;
      case "c3DShininess":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.Shininess = (float) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.Shininess = (float) this.GetIntValue(tokenValue);
        break;
      case "c3DExtrudeForward":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.ForeDepth = (float) this.GetIntValue(tokenValue) / 12700f;
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.ForeDepth = (float) this.GetIntValue(tokenValue) / 12700f;
        break;
      case "c3DExtrudeBackward":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.BackDepth = (float) this.GetIntValue(tokenValue) / 12700f;
          this.m_currShape.EffectList[1].ThreeDFormat.ExtrusionHeight = this.m_currShape.EffectList[1].ThreeDFormat.BackDepth;
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.BackDepth = (float) this.GetIntValue(tokenValue) / 12700f;
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.ExtrusionHeight = this.m_currShape.EffectList[1].ThreeDFormat.BackDepth;
        break;
      case "c3DExtrudePlane":
        if (string.IsNullOrEmpty(tokenValue) || tokenValue.Length <= 2 || !Enum.IsDefined(typeof (ExtrusionPlane), (object) (char.ToUpper(tokenValue[0]).ToString() + tokenValue.Substring(1))))
          break;
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.ExtrusionPlane = (ExtrusionPlane) Enum.Parse(typeof (ExtrusionPlane), tokenValue, true);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.ExtrusionPlane = (ExtrusionPlane) Enum.Parse(typeof (ExtrusionPlane), tokenValue, true);
        break;
      case "c3DExtrusionColor":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.HasExtrusionColor = true;
          this.m_currShape.EffectList[1].ThreeDFormat.ExtrusionColor = Color.FromArgb(int.Parse(tokenValue));
          this.m_currShape.EffectList[1].ThreeDFormat.ExtrusionColor = Color.FromArgb((int) this.m_currShape.EffectList[1].ThreeDFormat.ExtrusionColor.B, (int) this.m_currShape.EffectList[1].ThreeDFormat.ExtrusionColor.G, (int) this.m_currShape.EffectList[1].ThreeDFormat.ExtrusionColor.R);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.HasExtrusionColor = true;
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.ExtrusionColor = Color.FromArgb(int.Parse(tokenValue));
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.ExtrusionColor = Color.FromArgb((int) this.m_currShape.EffectList[1].ThreeDFormat.ExtrusionColor.B, (int) this.m_currShape.EffectList[1].ThreeDFormat.ExtrusionColor.G, (int) this.m_currShape.EffectList[1].ThreeDFormat.ExtrusionColor.R);
        break;
      case "c3DExtrusionColorExtMod":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.ColorMode = tokenValue;
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.ColorMode = tokenValue;
        break;
      case "f3D":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.Visible = this.GetIntValue(tokenValue) == 1;
          this.m_currShape.EffectList[1].ThreeDFormat.HasBevelTop = true;
          this.m_currShape.EffectList[1].ThreeDFormat.HasBevelBottom = true;
          this.m_currShape.EffectList[1].ThreeDFormat.BevelBottomHeight = 1f;
          this.m_currShape.EffectList[1].ThreeDFormat.BevelBottomWidth = 1f;
          this.m_currShape.EffectList[1].ThreeDFormat.BevelTopHeight = 1f;
          this.m_currShape.EffectList[1].ThreeDFormat.BevelTopWidth = 1f;
          this.m_currShape.EffectList[1].ThreeDFormat.PresetMaterialType = PresetMaterialType.LegacyMatte;
          this.m_currShape.EffectList[1].ThreeDFormat.ExtrusionHeight = 36f;
          this.m_currShape.EffectList[1].ThreeDFormat.HasLightRigEffect = true;
          this.m_currShape.EffectList[1].ThreeDFormat.LightRigType = LightRigType.LegacyFlat3;
          this.m_currShape.EffectList[1].ThreeDFormat.LightRigDirection = LightRigDirection.T;
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.Visible = this.GetIntValue(tokenValue) == 1;
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.HasBevelTop = true;
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.HasBevelBottom = true;
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.BevelBottomHeight = (float) Math.Abs(1);
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.BevelBottomWidth = 1f;
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.BevelTopHeight = 1f;
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.BevelTopWidth = 1f;
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.PresetMaterialType = PresetMaterialType.LegacyMatte;
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.ExtrusionHeight = 36f;
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.HasLightRigEffect = true;
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.LightRigType = LightRigType.LegacyFlat3;
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.LightRigDirection = LightRigDirection.T;
        break;
      case "fc3DMetallic":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.Metal = this.GetIntValue(tokenValue) == 1;
          this.m_currShape.EffectList[1].ThreeDFormat.PresetMaterialType = tokenValue == "1" ? PresetMaterialType.Metal : PresetMaterialType.LegacyMatte;
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.Metal = this.GetIntValue(tokenValue) == 1;
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.PresetMaterialType = tokenValue == "1" ? PresetMaterialType.Metal : PresetMaterialType.LegacyMatte;
        break;
      case "fc3DUseExtrusionColor":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.HasExtrusionColor = this.GetIntValue(tokenValue) == 1;
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.HasExtrusionColor = this.GetIntValue(tokenValue) == 1;
        break;
      case "fc3DLightFace":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.HasLightRigEffect = this.GetIntValue(tokenValue) == 1;
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.HasLightRigEffect = this.GetIntValue(tokenValue) == 1;
        break;
      case "c3DYRotationAngle":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.RotationAngleY = (float) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.RotationAngleY = (float) this.GetIntValue(tokenValue);
        break;
      case "c3DXRotationAngle":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.RotationAngleX = (float) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.RotationAngleX = (float) this.GetIntValue(tokenValue);
        break;
      case "c3DRotationAxisX":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.RotationX = (float) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.RotationX = (float) this.GetIntValue(tokenValue);
        break;
      case "c3DRotationAxisY":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.RotationY = (float) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.RotationY = (float) this.GetIntValue(tokenValue);
        break;
      case "c3DRotationAxisZ":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.RotationZ = (float) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.RotationZ = (float) this.GetIntValue(tokenValue);
        break;
      case "c3DRotationAngle":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.OrientationAngle = (float) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.OrientationAngle = (float) this.GetIntValue(tokenValue);
        break;
      case "c3DRotationCenterX":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.RotationCenterX = (float) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.RotationCenterX = (float) this.GetIntValue(tokenValue);
        break;
      case "c3DRotationCenterY":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.RotationCenterY = (float) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.RotationCenterY = (float) this.GetIntValue(tokenValue);
        break;
      case "c3DRotationCenterZ":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.RotationCenterZ = (float) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.RotationCenterZ = (float) this.GetIntValue(tokenValue);
        break;
      case "fc3DRotationCenterAuto":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.AutoRotationCenter = this.GetIntValue(tokenValue) == 1;
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.AutoRotationCenter = this.GetIntValue(tokenValue) == 1;
        break;
      case "c3DEdgeThickness":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.Edge = (float) this.GetIntValue(tokenValue) / 12700f;
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.Edge = (float) this.GetIntValue(tokenValue) / 12700f;
        break;
      case "txflTextFlow":
        if (this.m_currTextBox == null)
          break;
        switch (tokenValue)
        {
          case "1":
            this.m_currTextBox.TextBoxFormat.TextDirection = TextDirection.VerticalFarEast;
            return;
          case "2":
            this.m_currTextBox.TextBoxFormat.TextDirection = TextDirection.VerticalBottomToTop;
            return;
          case "3":
            this.m_currTextBox.TextBoxFormat.TextDirection = TextDirection.VerticalTopToBottom;
            return;
          case "4":
            this.m_currTextBox.TextBoxFormat.TextDirection = TextDirection.HorizontalFarEast;
            return;
          case "5":
            this.m_currTextBox.TextBoxFormat.TextDirection = TextDirection.Vertical;
            return;
          default:
            this.m_currTextBox.TextBoxFormat.TextDirection = TextDirection.Horizontal;
            return;
        }
      case "scaleText":
        if (this.m_currTextBox == null)
          break;
        this.m_currTextBox.CharacterFormat.Scaling = float.Parse(tokenValue);
        break;
      case "fFitShapeToText":
        if (this.m_currTextBox == null)
          break;
        this.m_currTextBox.TextBoxFormat.AutoFit = tokenValue == "1";
        break;
      case "c3DRenderMode":
        switch (tokenValue)
        {
          case null:
            return;
          case "0":
            if (this.m_currShape != null)
            {
              this.m_currShape.EffectList[1].ThreeDFormat.ExtrusionRenderMode = ExtrusionRenderMode.Solid;
              return;
            }
            this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.ExtrusionRenderMode = ExtrusionRenderMode.Solid;
            return;
          case "1":
            if (this.m_currShape != null)
            {
              this.m_currShape.EffectList[1].ThreeDFormat.ExtrusionRenderMode = ExtrusionRenderMode.Wireframe;
              return;
            }
            this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.ExtrusionRenderMode = ExtrusionRenderMode.Wireframe;
            return;
          case "2":
            if (this.m_currShape != null)
            {
              this.m_currShape.EffectList[1].ThreeDFormat.ExtrusionRenderMode = ExtrusionRenderMode.BoundingCube;
              return;
            }
            this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.ExtrusionRenderMode = ExtrusionRenderMode.BoundingCube;
            return;
          default:
            return;
        }
      case "c3DXViewpoint":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.ViewPointX = float.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.ViewPointX = float.Parse(tokenValue);
        break;
      case "c3DYViewpoint":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.ViewPointY = float.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.ViewPointY = float.Parse(tokenValue);
        break;
      case "c3DZViewpoint":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.ViewPointZ = float.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.ViewPointZ = float.Parse(tokenValue);
        break;
      case "c3DOriginX":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.ViewPointOriginX = float.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.ViewPointOriginX = float.Parse(tokenValue);
        break;
      case "c3DOriginY":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.ViewPointOriginY = float.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.ViewPointOriginY = float.Parse(tokenValue);
        break;
      case "c3DSkewAngle":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.SkewAngle = float.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.SkewAngle = float.Parse(tokenValue);
        break;
      case "c3DSkewAmount":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.SkewAmount = float.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.SkewAmount = float.Parse(tokenValue);
        break;
      case "c3DAmbientIntensity":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.Brightness = float.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.Brightness = float.Parse(tokenValue);
        break;
      case "c3DKeyIntensity":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.LightLevel = float.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.LightLevel = float.Parse(tokenValue);
        break;
      case "c3DKeyX":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.LightRigRotationX = float.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.LightRigRotationX = float.Parse(tokenValue);
        break;
      case "c3DKeyY":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.LightRigRotationY = float.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.LightRigRotationY = float.Parse(tokenValue);
        break;
      case "c3DKeyZ":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.LightRigRotationZ = float.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.LightRigRotationZ = float.Parse(tokenValue);
        break;
      case "c3DFillIntensity":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.LightLevel2 = float.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.LightLevel2 = float.Parse(tokenValue);
        break;
      case "c3DFillX":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.LightRigRotation2X = float.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.LightRigRotation2X = float.Parse(tokenValue);
        break;
      case "c3DFillY":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.LightRigRotation2Y = float.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.LightRigRotation2Y = float.Parse(tokenValue);
        break;
      case "c3DFillZ":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.LightRigRotation2Z = float.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.LightRigRotation2Z = float.Parse(tokenValue);
        break;
      case "fc3DKeyHarsh":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.LightHarsh = this.GetIntValue(tokenValue) == 1;
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.LightHarsh = this.GetIntValue(tokenValue) == 1;
        break;
      case "fc3DFillHarsh":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.LightHarsh2 = this.GetIntValue(tokenValue) == 1;
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.LightHarsh2 = this.GetIntValue(tokenValue) == 1;
        break;
      case "fillAngle":
        if (this.m_currShape != null)
        {
          this.m_currShape.FillFormat.TextureHorizontalScale = (double) float.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.FillFormat.TextureHorizontalScale = (double) float.Parse(tokenValue);
        break;
      case "adjustValue":
      case "adjust2Value":
      case "adjust3Value":
      case "adjust4Value":
      case "adjust5Value":
      case "adjust6Value":
      case "adjust7Value":
      case "adjust8Value":
      case "adjust9Value":
      case "adjust10Value":
        if (this.m_currShape.ShapeGuide.ContainsKey(token))
        {
          this.m_currShape.ShapeGuide[token] = tokenValue;
          break;
        }
        this.m_currShape.ShapeGuide.Add(token, tokenValue);
        break;
      case "fInnerShadow":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[0].ShadowFormat.m_type = tokenValue == "0" ? "outerShdw" : "innerShdw";
          break;
        }
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.m_type = tokenValue == "0" ? "outerShdw" : "innerShdw";
        break;
      case "shadowScaleYToX":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[0].ShadowFormat.HorizontalSkewAngle = (short) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.HorizontalSkewAngle = (short) this.GetIntValue(tokenValue);
        break;
      case "shadowScaleXToY":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[0].ShadowFormat.VerticalSkewAngle = (short) this.GetIntValue(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.VerticalSkewAngle = (short) this.GetIntValue(tokenValue);
        break;
      case "shadowScaleYToY":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[0].ShadowFormat.VerticalScalingFactor = double.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.VerticalScalingFactor = double.Parse(tokenValue);
        break;
      case "shadowScaleXToX":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[0].ShadowFormat.HorizontalScalingFactor = double.Parse(tokenValue);
          break;
        }
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.HorizontalScalingFactor = double.Parse(tokenValue);
        break;
      case "shadowPerspectiveX":
      case "shadowPerspectiveY":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[0].ShadowFormat.ShadowPerspectiveMatrix = tokenValue;
          break;
        }
        this.m_currTextBox.Shape.EffectList[0].ShadowFormat.ShadowPerspectiveMatrix = tokenValue;
        break;
      case "fc3DParallel":
        if (this.m_currShape != null)
        {
          this.m_currShape.EffectList[1].ThreeDFormat.HasCameraEffect = true;
          this.m_currShape.EffectList[1].ThreeDFormat.CameraPresetType = tokenValue == "1" ? CameraPresetType.LegacyObliqueLeft : CameraPresetType.PerspectiveBelow;
          break;
        }
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.HasCameraEffect = true;
        this.m_currTextBox.Shape.EffectList[1].ThreeDFormat.CameraPresetType = tokenValue == "1" ? CameraPresetType.LegacyObliqueLeft : CameraPresetType.PerspectiveBelow;
        break;
      case "fFilled":
        if (!(tokenValue == "0"))
          break;
        if (this.m_currTextBox != null)
        {
          this.m_currTextBox.Shape.FillFormat.Fill = false;
          this.m_currTextBox.TextBoxFormat.FillColor = Color.Empty;
          break;
        }
        this.m_currShape.FillFormat.Fill = false;
        this.m_currShape.FillFormat.Color = Color.Empty;
        break;
      case "anchorText":
        if (this.m_currTextBox == null || this.m_currTextBox.Shape == null)
          break;
        this.m_currTextBox.IsShape = true;
        switch (tokenValue)
        {
          case null:
            return;
          case "0":
            this.m_currTextBox.Shape.TextFrame.TextVerticalAlignment = VerticalAlignment.Top;
            return;
          case "1":
            this.m_currTextBox.Shape.TextFrame.TextVerticalAlignment = VerticalAlignment.Middle;
            return;
          case "2":
            this.m_currTextBox.Shape.TextFrame.TextVerticalAlignment = VerticalAlignment.Bottom;
            return;
          default:
            return;
        }
    }
  }

  private void SetDefaultValuesForShapeTextBox()
  {
    this.m_currTextBox.Shape.LineFormat.Color = this.m_currTextBox.TextBoxFormat.LineColor;
    this.m_currTextBox.Shape.LineFormat.Weight = this.m_currTextBox.TextBoxFormat.LineWidth;
    this.m_currTextBox.Shape.LineFormat.DashStyle = this.m_currTextBox.TextBoxFormat.LineDashing;
    this.m_currTextBox.Shape.LineFormat.Style = this.GetLineStyle();
    this.m_currTextBox.Shape.FillFormat.Color = this.m_currTextBox.TextBoxFormat.FillColor;
    if (this.m_currTextBox.Shape.LineFormat.LineCap != LineCap.Round)
      this.m_currTextBox.Shape.LineFormat.LineCap = LineCap.Flat;
    this.m_currTextBox.Shape.TextFrame.InternalMargin.Right = this.m_currTextBox.TextBoxFormat.InternalMargin.Right;
    this.m_currTextBox.Shape.TextFrame.InternalMargin.Left = this.m_currTextBox.TextBoxFormat.InternalMargin.Left;
    this.m_currTextBox.Shape.TextFrame.InternalMargin.Top = this.m_currTextBox.TextBoxFormat.InternalMargin.Top;
    this.m_currTextBox.Shape.TextFrame.InternalMargin.Bottom = this.m_currTextBox.TextBoxFormat.InternalMargin.Bottom;
  }

  private LineStyle GetLineStyle()
  {
    switch (this.m_currTextBox.TextBoxFormat.LineStyle)
    {
      case TextBoxLineStyle.Simple:
        return LineStyle.Single;
      case TextBoxLineStyle.Double:
        return LineStyle.StyleMixed;
      case TextBoxLineStyle.ThickThin:
        return LineStyle.ThickThin;
      case TextBoxLineStyle.ThinThick:
        return LineStyle.ThinThick;
      case TextBoxLineStyle.Triple:
        return LineStyle.ThickBetweenThin;
      default:
        return LineStyle.Single;
    }
  }

  private void SetRotationValue(string rotationValue)
  {
    if (this.m_currShape != null)
    {
      this.m_currShape.Rotation = float.Parse(rotationValue) / 65536f;
      if (this.m_currShape.AutoShapeType == AutoShapeType.ElbowConnector || this.m_currShape.AutoShapeType == AutoShapeType.CurvedConnector)
        this.m_currShape.Rotation = Math.Abs(this.m_currShape.Rotation);
      if (((double) this.m_currShape.Rotation < 44.0 || (double) this.m_currShape.Rotation > 134.0) && ((double) this.m_currShape.Rotation < 225.0 || (double) this.m_currShape.Rotation > 314.0))
        return;
      float height = this.m_currShape.Height;
      this.m_currShape.Height = this.m_currShape.Width;
      this.m_currShape.Width = height;
      float num = Math.Abs(this.m_currShape.Height - this.m_currShape.Width) / 2f;
      if ((double) this.m_currShape.Height > (double) this.m_currShape.Width)
      {
        this.m_currShape.HorizontalPosition += num;
        this.m_currShape.VerticalPosition -= num;
      }
      if ((double) this.m_currShape.Height >= (double) this.m_currShape.Width)
        return;
      this.m_currShape.VerticalPosition += num;
      this.m_currShape.HorizontalPosition -= num;
    }
    else
      this.m_currTextBox.TextBoxFormat.Rotation = float.Parse(rotationValue) / 65536f;
  }

  private void AddShadowDirectionandDistance()
  {
    double shadowOffsetX = (double) this.m_currShape.EffectList[0].ShadowFormat.ShadowOffsetX;
    double shadowOffsetY = (double) this.m_currShape.EffectList[0].ShadowFormat.ShadowOffsetY;
    double num1 = shadowOffsetX / 12700.0;
    double num2 = shadowOffsetY / 12700.0;
    this.m_currShape.EffectList[0].ShadowFormat.Distance = Math.Sqrt(Math.Pow(Math.Abs(num1), 2.0) + Math.Pow(Math.Abs(num2), 2.0));
    double x = 0.0 - num1;
    double num3 = Math.Atan2(0.0 - num2, x) * (180.0 / Math.PI);
    if (shadowOffsetX < 0.0 && shadowOffsetY < 0.0)
      this.m_currShape.EffectList[0].ShadowFormat.Direction = Math.Round(180.0 + Math.Abs(num3));
    else if (shadowOffsetX > 0.0 && shadowOffsetY < 0.0)
      this.m_currShape.EffectList[0].ShadowFormat.Direction = Math.Round(num3 <= 0.0 ? 360.0 - Math.Abs(num3) : 180.0 + Math.Abs(num3));
    else if (shadowOffsetX < 0.0 && shadowOffsetY > 0.0)
    {
      this.m_currShape.EffectList[0].ShadowFormat.Direction = Math.Round(num3 <= 0.0 ? 180.0 - Math.Abs(num3) : Math.Abs(num3));
    }
    else
    {
      if (shadowOffsetX <= 0.0 || shadowOffsetY <= 0.0)
        return;
      this.m_currShape.EffectList[0].ShadowFormat.Direction = Math.Round(num3 <= 0.0 ? 180.0 - Math.Abs(num3) : Math.Abs(num3));
    }
  }

  private void AddAdjustValues()
  {
    switch (this.m_currShape.AutoShapeType)
    {
      case AutoShapeType.Parallelogram:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          if ((double) this.m_currShape.Width > (double) this.m_currShape.Height)
          {
            using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
            {
              if (!enumerator.MoveNext())
                break;
              double num = Math.Round((4.62962963 / (double) this.m_currShape.Height * (double) (this.m_currShape.Width - this.m_currShape.Height) + 4.62962963) * (double) int.Parse(enumerator.Current.Value));
              this.m_currShape.ShapeGuide.Clear();
              this.m_currShape.ShapeGuide.Add("adj", "val " + num.ToString());
              break;
            }
          }
          using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            double num = Math.Round((double) int.Parse(enumerator.Current.Value) * 4.62962963);
            this.m_currShape.ShapeGuide.Clear();
            this.m_currShape.ShapeGuide.Add("adj", "val " + num.ToString());
            break;
          }
        }
        if ((double) this.m_currShape.Width > (double) this.m_currShape.Height)
        {
          this.m_currShape.ShapeGuide.Add("adj", "val " + Math.Round((double) (25000f / this.m_currShape.Height) * (double) (this.m_currShape.Width - this.m_currShape.Height) + 25000.0).ToString());
          break;
        }
        this.m_currShape.ShapeGuide.Add("adj", "val 25000");
        break;
      case AutoShapeType.RoundedRectangle:
      case AutoShapeType.DoubleBracket:
      case AutoShapeType.Plaque:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            double num1 = (double) int.Parse(enumerator.Current.Value);
            double num2 = Math.Round(Math.Abs(num1) * 4.62962963);
            if (num1 < 0.0)
              num2 = 0.0;
            this.m_currShape.ShapeGuide.Clear();
            this.m_currShape.ShapeGuide.Add("adj", "val " + num2.ToString());
            break;
          }
        }
        this.m_currShape.ShapeGuide.Add("adj", "val 16667");
        break;
      case AutoShapeType.Octagon:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            double num3 = (double) int.Parse(enumerator.Current.Value);
            double num4 = Math.Round(Math.Abs(num3) * 4.62962963);
            if (num3 < 0.0)
              num4 = 0.0;
            this.m_currShape.ShapeGuide.Clear();
            this.m_currShape.ShapeGuide.Add("adj", "val " + num4.ToString());
            break;
          }
        }
        this.m_currShape.ShapeGuide.Add("adj", "val 29287");
        break;
      case AutoShapeType.IsoscelesTriangle:
      case AutoShapeType.Moon:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            double num5 = (double) int.Parse(enumerator.Current.Value);
            double num6 = Math.Round(Math.Abs(num5) * 4.62962963);
            if (num5 < 0.0)
              num6 = 0.0;
            this.m_currShape.ShapeGuide.Clear();
            this.m_currShape.ShapeGuide.Add("adj", "val " + num6.ToString());
            break;
          }
        }
        this.m_currShape.ShapeGuide.Add("adj", "val 50000");
        break;
      case AutoShapeType.Hexagon:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          if ((double) this.m_currShape.Width > (double) this.m_currShape.Height)
          {
            using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
            {
              if (enumerator.MoveNext())
              {
                double num = Math.Round((4.62962963 / (double) this.m_currShape.Height * (double) (this.m_currShape.Width - this.m_currShape.Height) + 4.62962963) * (double) int.Parse(enumerator.Current.Value));
                this.m_currShape.ShapeGuide.Clear();
                this.m_currShape.ShapeGuide.Add("adj", "val " + num.ToString());
              }
            }
          }
          else
          {
            using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
            {
              if (enumerator.MoveNext())
              {
                double num = Math.Round((double) int.Parse(enumerator.Current.Value) * 4.62962963);
                this.m_currShape.ShapeGuide.Clear();
                this.m_currShape.ShapeGuide.Add("adj", "val " + num.ToString());
              }
            }
          }
        }
        else if ((double) this.m_currShape.Width > (double) this.m_currShape.Height)
          this.m_currShape.ShapeGuide.Add("adj", "val " + Math.Round((double) (25000f / this.m_currShape.Height) * (double) (this.m_currShape.Width - this.m_currShape.Height) + 25000.0).ToString());
        else
          this.m_currShape.ShapeGuide.Add("adj", "val 25000");
        this.m_currShape.ShapeGuide.Add("vf", "val 115470");
        break;
      case AutoShapeType.Cross:
      case AutoShapeType.Cube:
      case AutoShapeType.Sun:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            double num7 = (double) int.Parse(enumerator.Current.Value);
            double num8 = Math.Round(Math.Abs(num7) * 4.62962963);
            if (num7 < 0.0)
              num8 = 0.0;
            this.m_currShape.ShapeGuide.Clear();
            this.m_currShape.ShapeGuide.Add("adj", "val " + num8.ToString());
            break;
          }
        }
        this.m_currShape.ShapeGuide.Add("adj", "val 25000");
        break;
      case AutoShapeType.Can:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          if ((double) this.m_currShape.Height > (double) this.m_currShape.Width)
          {
            using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
            {
              if (!enumerator.MoveNext())
                break;
              double num = Math.Round((4.62962963 / (double) this.m_currShape.Width * (double) (this.m_currShape.Height - this.m_currShape.Width) + 4.62962963) * (double) int.Parse(enumerator.Current.Value));
              this.m_currShape.ShapeGuide.Clear();
              this.m_currShape.ShapeGuide.Add("adj", "val " + num.ToString());
              break;
            }
          }
          using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            double num = Math.Round((double) int.Parse(enumerator.Current.Value) * 4.62962963);
            this.m_currShape.ShapeGuide.Clear();
            this.m_currShape.ShapeGuide.Add("adj", "val " + num.ToString());
            break;
          }
        }
        if ((double) this.m_currShape.Height > (double) this.m_currShape.Width)
        {
          this.m_currShape.ShapeGuide.Add("adj", "val " + Math.Round((double) (25000f / this.m_currShape.Width) * (double) (this.m_currShape.Height - this.m_currShape.Width) + 25000.0).ToString());
          break;
        }
        this.m_currShape.ShapeGuide.Add("adj", "val 25000");
        break;
      case AutoShapeType.Bevel:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            double num9 = (double) int.Parse(enumerator.Current.Value);
            double num10 = Math.Round(Math.Abs(num9) * 4.62962963);
            if (num9 < 0.0)
              num10 = 0.0;
            this.m_currShape.ShapeGuide.Clear();
            this.m_currShape.ShapeGuide.Add("adj", "val " + num10.ToString());
            break;
          }
        }
        this.m_currShape.ShapeGuide.Add("adj", "val 12500");
        break;
      case AutoShapeType.FoldedCorner:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            double num = Math.Round(Math.Abs(21600.0 - (double) Math.Abs(int.Parse(enumerator.Current.Value))) * 4.62962963);
            this.m_currShape.ShapeGuide.Clear();
            this.m_currShape.ShapeGuide.Add("adj", "val " + num.ToString());
            break;
          }
        }
        this.m_currShape.ShapeGuide.Add("adj", "val 12500");
        break;
      case AutoShapeType.SmileyFace:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            double num = Math.Round(4653.0 - (17520.0 - (double) Math.Abs(int.Parse(enumerator.Current.Value))) * 4.62962963);
            this.m_currShape.ShapeGuide.Clear();
            this.m_currShape.ShapeGuide.Add("adj", "val " + num.ToString());
            break;
          }
        }
        this.m_currShape.ShapeGuide.Add("adj", "val 4653");
        break;
      case AutoShapeType.Donut:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          if ((double) this.m_currShape.Height != (double) this.m_currShape.Width)
            break;
          using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            double num = Math.Round((double) int.Parse(enumerator.Current.Value) * 4.63);
            this.m_currShape.ShapeGuide.Clear();
            this.m_currShape.ShapeGuide.Add("adj", "val " + num.ToString());
            break;
          }
        }
        if ((double) this.m_currShape.Height != (double) this.m_currShape.Width)
          break;
        this.m_currShape.ShapeGuide.Add("adj", "val 25000");
        break;
      case AutoShapeType.NoSymbol:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          if ((double) this.m_currShape.Height != (double) this.m_currShape.Width)
            break;
          using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            double num = Math.Round((double) int.Parse(enumerator.Current.Value) * 4.63);
            this.m_currShape.ShapeGuide.Clear();
            this.m_currShape.ShapeGuide.Add("adj", "val " + num.ToString());
            break;
          }
        }
        if ((double) this.m_currShape.Height != (double) this.m_currShape.Width)
          break;
        this.m_currShape.ShapeGuide.Add("adj", "val 12500");
        break;
      case AutoShapeType.DoubleBrace:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            double num11 = (double) int.Parse(enumerator.Current.Value);
            double num12 = Math.Round(Math.Abs(num11) * 4.62962963);
            if (num11 < 0.0)
              num12 = 0.0;
            this.m_currShape.ShapeGuide.Clear();
            this.m_currShape.ShapeGuide.Add("adj", "val " + num12.ToString());
            break;
          }
        }
        this.m_currShape.ShapeGuide.Add("adj", "val 8333");
        break;
      case AutoShapeType.LeftBracket:
      case AutoShapeType.RightBracket:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          if ((double) this.m_currShape.Height > (double) this.m_currShape.Width)
          {
            using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
            {
              if (!enumerator.MoveNext())
                break;
              double num = Math.Round((4.62962963 / (double) this.m_currShape.Width * (double) (this.m_currShape.Height - this.m_currShape.Width) + 4.62962963) * (double) int.Parse(enumerator.Current.Value));
              this.m_currShape.ShapeGuide.Clear();
              this.m_currShape.ShapeGuide.Add("adj", "val " + num.ToString());
              break;
            }
          }
          using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            double num = Math.Round((double) int.Parse(enumerator.Current.Value) * 4.62962963);
            this.m_currShape.ShapeGuide.Clear();
            this.m_currShape.ShapeGuide.Add("adj", "val " + num.ToString());
            break;
          }
        }
        if ((double) this.m_currShape.Height > (double) this.m_currShape.Width)
        {
          this.m_currShape.ShapeGuide.Add("adj", "val " + Math.Round((double) (8333f / this.m_currShape.Width) * (double) (this.m_currShape.Height - this.m_currShape.Width) + 8333.0).ToString());
          break;
        }
        this.m_currShape.ShapeGuide.Add("adj", "val 8333");
        break;
      case AutoShapeType.LeftBrace:
      case AutoShapeType.RightBrace:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          double num13 = 0.0;
          double num14 = 50000.0;
          if ((double) this.m_currShape.Height > (double) this.m_currShape.Width)
          {
            foreach (KeyValuePair<string, string> keyValuePair in this.m_currShape.ShapeGuide)
            {
              if (keyValuePair.Key == "adjustValue")
                num13 = Math.Round((4.62962963 / (double) this.m_currShape.Width * (double) (this.m_currShape.Height - this.m_currShape.Width) + 4.62962963) * (double) int.Parse(keyValuePair.Value));
              else
                num14 = Math.Round((double) int.Parse(keyValuePair.Value) * 4.62962963);
            }
          }
          else
          {
            foreach (KeyValuePair<string, string> keyValuePair in this.m_currShape.ShapeGuide)
            {
              double num15 = Math.Round((double) int.Parse(keyValuePair.Value) * 4.62962963);
              if (keyValuePair.Key == "adjustValue")
                num13 = num15;
              else
                num14 = num15;
            }
          }
          this.m_currShape.ShapeGuide.Clear();
          this.m_currShape.ShapeGuide.Add("adj1", "val " + num13.ToString());
          this.m_currShape.ShapeGuide.Add("adj2", "val " + num14.ToString());
          break;
        }
        if ((double) this.m_currShape.Height > (double) this.m_currShape.Width)
          this.m_currShape.ShapeGuide.Add("adj1", "val " + Math.Round((double) (8333f / this.m_currShape.Width) * (double) (this.m_currShape.Height - this.m_currShape.Width) + 8333.0).ToString());
        else
          this.m_currShape.ShapeGuide.Add("adj1", "val 8333");
        this.m_currShape.ShapeGuide.Add("adj2", "val 50000");
        break;
      case AutoShapeType.RightArrow:
      case AutoShapeType.DownArrow:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          double num16 = 50000.0;
          double num17 = 25000.0;
          foreach (KeyValuePair<string, string> keyValuePair in this.m_currShape.ShapeGuide)
          {
            float num18 = (float) int.Parse(keyValuePair.Value);
            if ((double) this.m_currShape.Width > (double) this.m_currShape.Height)
            {
              if (keyValuePair.Key == "adjustValue")
              {
                double width = (double) this.m_currShape.Width;
                double height = (double) this.m_currShape.Height;
                num17 = Math.Round(100000.0 + 100000.0 / (double) this.m_currShape.Height + (double) num18 * 4.62962963);
              }
              else
                num16 = Math.Round(100000.0 - (double) num18 * 9.25925924);
            }
            else if (keyValuePair.Key == "adjustValue")
              num17 = Math.Round(100000.0 - (double) num18 * 4.62962963);
            else
              num16 = Math.Round(100000.0 - (double) num18 * 9.25925924);
          }
          this.m_currShape.ShapeGuide.Clear();
          this.m_currShape.ShapeGuide.Add("adj1", "val " + num16.ToString());
          this.m_currShape.ShapeGuide.Add("adj2", "val " + num17.ToString());
          break;
        }
        double num19 = 50000.0;
        double num20 = 25000.0;
        if ((double) this.m_currShape.Width > (double) this.m_currShape.Height)
          num20 = Math.Round((double) (25000f / this.m_currShape.Height) * (double) (this.m_currShape.Width - this.m_currShape.Height) + 25000.0);
        this.m_currShape.ShapeGuide.Add("adj1", "val " + num19.ToString());
        this.m_currShape.ShapeGuide.Add("adj2", "val " + num20.ToString());
        break;
      case AutoShapeType.Star4Point:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            double num21 = 50000.0 - Math.Round((double) int.Parse(enumerator.Current.Value) * 4.62962963);
            this.m_currShape.ShapeGuide.Clear();
            this.m_currShape.ShapeGuide.Add("adj", "val " + num21.ToString());
            break;
          }
        }
        this.m_currShape.ShapeGuide.Add("adj", "val 12500");
        break;
      case AutoShapeType.Star8Point:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            double num22 = 50000.0 - Math.Round((double) int.Parse(enumerator.Current.Value) * 4.62962963);
            this.m_currShape.ShapeGuide.Clear();
            this.m_currShape.ShapeGuide.Add("adj", "val " + num22.ToString());
            break;
          }
        }
        this.m_currShape.ShapeGuide.Add("adj", "val 38250");
        break;
      case AutoShapeType.Star16Point:
      case AutoShapeType.Star24Point:
      case AutoShapeType.Star32Point:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            double num23 = 50000.0 - Math.Round((double) int.Parse(enumerator.Current.Value) * 4.62962963);
            this.m_currShape.ShapeGuide.Clear();
            this.m_currShape.ShapeGuide.Add("adj", "val " + num23.ToString());
            break;
          }
        }
        this.m_currShape.ShapeGuide.Add("adj", "val 37500");
        break;
      case AutoShapeType.UpRibbon:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          double num24 = 12500.0;
          double num25 = 50000.0;
          foreach (KeyValuePair<string, string> keyValuePair in this.m_currShape.ShapeGuide)
          {
            float num26 = (float) int.Parse(keyValuePair.Value);
            if (keyValuePair.Key == "adjustValue")
              num25 = Math.Round(75000.0 - (double) Math.Abs(2700f - num26) * 9.25925926);
            else
              num24 = Math.Round((21600.0 - (double) num26) * 4.62962963);
          }
          this.m_currShape.ShapeGuide.Clear();
          this.m_currShape.ShapeGuide.Add("adj1", "val " + num24.ToString());
          this.m_currShape.ShapeGuide.Add("adj2", "val " + num25.ToString());
          break;
        }
        this.m_currShape.ShapeGuide.Add("adj1", "val 12500");
        this.m_currShape.ShapeGuide.Add("adj2", "val 50000");
        break;
      case AutoShapeType.DownRibbon:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          double num27 = 12500.0;
          double num28 = 50000.0;
          foreach (KeyValuePair<string, string> keyValuePair in this.m_currShape.ShapeGuide)
          {
            double num29 = Math.Round(75000.0 - (double) Math.Abs(2700f - (float) int.Parse(keyValuePair.Value)) * 9.25925926);
            if (keyValuePair.Key == "adjustValue")
              num28 = num29;
            else
              num27 = num29;
          }
          this.m_currShape.ShapeGuide.Clear();
          this.m_currShape.ShapeGuide.Add("adj1", "val " + num27.ToString());
          this.m_currShape.ShapeGuide.Add("adj2", "val " + num28.ToString());
          break;
        }
        this.m_currShape.ShapeGuide.Add("adj1", "val 12500");
        this.m_currShape.ShapeGuide.Add("adj2", "val 50000");
        break;
      case AutoShapeType.CurvedUpRibbon:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          double num30 = 25000.0;
          double num31 = 50000.0;
          double num32 = 12500.0;
          foreach (KeyValuePair<string, string> keyValuePair in this.m_currShape.ShapeGuide)
          {
            float num33 = (float) int.Parse(keyValuePair.Value);
            if (keyValuePair.Key == "adjustValue")
              num31 = Math.Round(75000.0 - (double) Math.Abs(2700f - num33) * 9.25925926);
            else if (keyValuePair.Key == "adjust2Value")
              num30 = Math.Round(41667.0 - ((double) num33 - 12600.0) * 4.62962963);
            else
              num32 = Math.Round((double) num33 * 4.62962963);
          }
          this.m_currShape.ShapeGuide.Clear();
          this.m_currShape.ShapeGuide.Add("adj1", "val " + num30.ToString());
          this.m_currShape.ShapeGuide.Add("adj2", "val " + num31.ToString());
          this.m_currShape.ShapeGuide.Add("adj3", "val " + num32.ToString());
          break;
        }
        this.m_currShape.ShapeGuide.Add("adj1", "val 25000");
        this.m_currShape.ShapeGuide.Add("adj2", "val 50000");
        this.m_currShape.ShapeGuide.Add("adj3", "val 12500");
        break;
      case AutoShapeType.CurvedDownRibbon:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          double num34 = 25000.0;
          double num35 = 50000.0;
          double num36 = 12500.0;
          foreach (KeyValuePair<string, string> keyValuePair in this.m_currShape.ShapeGuide)
          {
            float num37 = (float) int.Parse(keyValuePair.Value);
            if (keyValuePair.Key == "adjustValue")
              num35 = Math.Round(75000.0 - (double) Math.Abs(2700f - num37) * 9.25925926);
            else if (keyValuePair.Key == "adjust2Value")
              num34 = Math.Round(41667.0 - (9000.0 - (double) num37) * 4.62962963);
            else
              num36 = Math.Round(3125.0 + (20925.0 - (double) num37) * 4.62962963);
          }
          this.m_currShape.ShapeGuide.Clear();
          this.m_currShape.ShapeGuide.Add("adj1", "val " + num34.ToString());
          this.m_currShape.ShapeGuide.Add("adj2", "val " + num35.ToString());
          this.m_currShape.ShapeGuide.Add("adj3", "val " + num36.ToString());
          break;
        }
        this.m_currShape.ShapeGuide.Add("adj1", "val 25000");
        this.m_currShape.ShapeGuide.Add("adj2", "val 50000");
        this.m_currShape.ShapeGuide.Add("adj3", "val 12500");
        break;
      case AutoShapeType.VerticalScroll:
      case AutoShapeType.HorizontalScroll:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
          {
            if (!enumerator.MoveNext())
              break;
            double num38 = Math.Round((double) int.Parse(enumerator.Current.Value) * 4.62962963);
            this.m_currShape.ShapeGuide.Clear();
            this.m_currShape.ShapeGuide.Add("adj", "val " + num38.ToString());
            break;
          }
        }
        this.m_currShape.ShapeGuide.Add("adj", "val 12500");
        break;
      case AutoShapeType.Wave:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          double num39 = 13005.0;
          double num40 = 0.0;
          foreach (KeyValuePair<string, string> keyValuePair in this.m_currShape.ShapeGuide)
          {
            float num41 = (float) int.Parse(keyValuePair.Value);
            if (keyValuePair.Key == "adjustValue")
            {
              num39 = Math.Round((double) num41 * 4.62962963);
            }
            else
            {
              num40 = Math.Round((double) Math.Abs(10800f - num41) * 4.62962963);
              if ((double) num41 < 10800.0)
                num40 = -num40;
            }
          }
          this.m_currShape.ShapeGuide.Clear();
          this.m_currShape.ShapeGuide.Add("adj1", "val " + num39.ToString());
          this.m_currShape.ShapeGuide.Add("adj2", "val " + num40.ToString());
          break;
        }
        this.m_currShape.ShapeGuide.Add("adj1", "val 13005");
        this.m_currShape.ShapeGuide.Add("adj2", "val 0");
        break;
      case AutoShapeType.DoubleWave:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          double num42 = 13005.0;
          double num43 = 0.0;
          foreach (KeyValuePair<string, string> keyValuePair in this.m_currShape.ShapeGuide)
          {
            float num44 = (float) int.Parse(keyValuePair.Value);
            if (keyValuePair.Key == "adjustValue")
            {
              num42 = Math.Round((double) num44 * 4.62962963);
            }
            else
            {
              num43 = Math.Round((double) Math.Abs(10800f - num44) * 4.62962963);
              if ((double) num44 < 10800.0)
                num43 = -num43;
            }
          }
          this.m_currShape.ShapeGuide.Clear();
          this.m_currShape.ShapeGuide.Add("adj1", "val " + num42.ToString());
          this.m_currShape.ShapeGuide.Add("adj2", "val " + num43.ToString());
          break;
        }
        this.m_currShape.ShapeGuide.Add("adj1", "val 6500");
        this.m_currShape.ShapeGuide.Add("adj2", "val 0");
        break;
      case AutoShapeType.RectangularCallout:
      case AutoShapeType.OvalCallout:
      case AutoShapeType.CloudCallout:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          double num45 = -43570.0;
          double num46 = 70000.0;
          foreach (KeyValuePair<string, string> keyValuePair in this.m_currShape.ShapeGuide)
          {
            double num47 = Math.Round((double) int.Parse(keyValuePair.Value) * 4.62962963) - 50000.0;
            if (keyValuePair.Key == "adjustValue")
              num45 = num47;
            else
              num46 = num47;
          }
          this.m_currShape.ShapeGuide.Clear();
          this.m_currShape.ShapeGuide.Add("adj1", "val " + num45.ToString());
          this.m_currShape.ShapeGuide.Add("adj2", "val " + num46.ToString());
          break;
        }
        this.m_currShape.ShapeGuide.Add("adj1", "val -43750");
        this.m_currShape.ShapeGuide.Add("adj2", "val 70000");
        break;
      case AutoShapeType.RoundedRectangularCallout:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          double num48 = -43570.0;
          double num49 = 70000.0;
          foreach (KeyValuePair<string, string> keyValuePair in this.m_currShape.ShapeGuide)
          {
            double num50 = Math.Round((double) int.Parse(keyValuePair.Value) * 4.62962963) - 50000.0;
            if (keyValuePair.Key == "adjustValue")
              num48 = num50;
            else
              num49 = num50;
          }
          this.m_currShape.ShapeGuide.Clear();
          this.m_currShape.ShapeGuide.Add("adj1", "val " + num48.ToString());
          this.m_currShape.ShapeGuide.Add("adj2", "val " + num49.ToString());
        }
        else
        {
          this.m_currShape.ShapeGuide.Add("adj1", "val -43750");
          this.m_currShape.ShapeGuide.Add("adj2", "val 70000");
        }
        this.m_currShape.ShapeGuide.Add("adj3", "val 16667");
        break;
      case AutoShapeType.LineCallout1:
      case AutoShapeType.LineCallout1NoBorder:
      case AutoShapeType.LineCallout1AccentBar:
      case AutoShapeType.LineCallout1BorderAndAccentBar:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          double num51 = 0.0;
          double num52 = -8333.0;
          double num53 = 0.0;
          double num54 = -8333.0;
          foreach (KeyValuePair<string, string> keyValuePair in this.m_currShape.ShapeGuide)
          {
            double num55 = Math.Round((double) int.Parse(keyValuePair.Value) * 4.62962963);
            if (keyValuePair.Key == "adjustValue")
              num54 = num55;
            else if (keyValuePair.Key == "adjust2Value")
              num53 = num55;
            else if (keyValuePair.Key == "adjust3Value")
              num52 = num55;
            else if (keyValuePair.Key == "adjust4Value")
              num51 = num55;
          }
          this.m_currShape.ShapeGuide.Clear();
          this.m_currShape.ShapeGuide.Add("adj1", "val " + num51.ToString());
          this.m_currShape.ShapeGuide.Add("adj2", "val " + num52.ToString());
          this.m_currShape.ShapeGuide.Add("adj3", "val " + num53.ToString());
          this.m_currShape.ShapeGuide.Add("adj4", "val " + num54.ToString());
          break;
        }
        this.m_currShape.ShapeGuide.Add("adj1", "val 18750");
        this.m_currShape.ShapeGuide.Add("adj2", "val -8333");
        this.m_currShape.ShapeGuide.Add("adj3", "val 112500");
        this.m_currShape.ShapeGuide.Add("adj4", "val -38333");
        break;
      case AutoShapeType.LineCallout2:
      case AutoShapeType.LineCallout2AccentBar:
      case AutoShapeType.LineCallout2NoBorder:
      case AutoShapeType.LineCallout2BorderAndAccentBar:
        if (this.m_currShape.ShapeGuide.Count != 0)
        {
          double num56 = 18750.0;
          double num57 = -8333.0;
          double num58 = 18750.0;
          double num59 = -16667.0;
          double num60 = 112500.0;
          double num61 = -46667.0;
          foreach (KeyValuePair<string, string> keyValuePair in this.m_currShape.ShapeGuide)
          {
            double num62 = Math.Round((double) int.Parse(keyValuePair.Value) * 4.62962963);
            if (keyValuePair.Key == "adjustValue")
              num61 = num62;
            else if (keyValuePair.Key == "adjust2Value")
              num60 = num62;
            else if (keyValuePair.Key == "adjust3Value")
              num59 = num62;
            else if (keyValuePair.Key == "adjust4Value")
              num58 = num62;
            else if (keyValuePair.Key == "adjust5Value")
              num57 = num62;
            else if (keyValuePair.Key == "adjust6Value")
              num56 = num62;
          }
          this.m_currShape.ShapeGuide.Clear();
          this.m_currShape.ShapeGuide.Add("adj1", "val " + num56.ToString());
          this.m_currShape.ShapeGuide.Add("adj2", "val " + num57.ToString());
          this.m_currShape.ShapeGuide.Add("adj3", "val " + num58.ToString());
          this.m_currShape.ShapeGuide.Add("adj4", "val " + num59.ToString());
          this.m_currShape.ShapeGuide.Add("adj5", "val " + num60.ToString());
          this.m_currShape.ShapeGuide.Add("adj6", "val " + num61.ToString());
          break;
        }
        this.m_currShape.ShapeGuide.Add("adj1", "val 18750");
        this.m_currShape.ShapeGuide.Add("adj2", "val -8333");
        this.m_currShape.ShapeGuide.Add("adj3", "val 18750");
        this.m_currShape.ShapeGuide.Add("adj4", "val -16667");
        this.m_currShape.ShapeGuide.Add("adj5", "val 112500");
        this.m_currShape.ShapeGuide.Add("adj6", "val -46667");
        break;
      case AutoShapeType.LineCallout3:
      case AutoShapeType.LineCallout3AccentBar:
      case AutoShapeType.LineCallout3NoBorder:
      case AutoShapeType.LineCallout3BorderAndAccentBar:
        if (this.m_currShape.ShapeGuide.Count == 0)
          break;
        double num63 = 18750.0;
        double num64 = 108333.0;
        double num65 = 18750.0;
        double num66 = 116667.0;
        double num67 = 100000.0;
        double num68 = 116667.0;
        double num69 = 112917.0;
        double num70 = 108333.0;
        foreach (KeyValuePair<string, string> keyValuePair in this.m_currShape.ShapeGuide)
        {
          double num71 = Math.Round((double) int.Parse(keyValuePair.Value) * 4.62962963);
          if (keyValuePair.Key == "adjustValue")
            num70 = num71;
          else if (keyValuePair.Key == "adjust2Value")
            num69 = num71;
          else if (keyValuePair.Key == "adjust3Value")
            num68 = num71;
          else if (keyValuePair.Key == "adjust4Value")
            num67 = num71;
          else if (keyValuePair.Key == "adjust5Value")
            num66 = num71;
          else if (keyValuePair.Key == "adjust6Value")
            num65 = num71;
          else if (keyValuePair.Key == "adjust7Value")
            num64 = num71;
          else if (keyValuePair.Key == "adjust8Value")
            num63 = num71;
        }
        this.m_currShape.ShapeGuide.Clear();
        this.m_currShape.ShapeGuide.Add("adj1", "val " + num63.ToString());
        this.m_currShape.ShapeGuide.Add("adj2", "val " + num64.ToString());
        this.m_currShape.ShapeGuide.Add("adj3", "val " + num65.ToString());
        this.m_currShape.ShapeGuide.Add("adj4", "val " + num66.ToString());
        this.m_currShape.ShapeGuide.Add("adj5", "val " + num67.ToString());
        this.m_currShape.ShapeGuide.Add("adj6", "val " + num68.ToString());
        this.m_currShape.ShapeGuide.Add("adj7", "val " + num69.ToString());
        this.m_currShape.ShapeGuide.Add("adj8", "val " + num70.ToString());
        break;
      case AutoShapeType.ElbowConnector:
      case AutoShapeType.CurvedConnector:
        using (Dictionary<string, string>.Enumerator enumerator = this.m_currShape.ShapeGuide.GetEnumerator())
        {
          if (!enumerator.MoveNext())
            break;
          KeyValuePair<string, string> current = enumerator.Current;
          double num72 = !current.Key.Contains("adjust") ? (double) int.Parse(current.Value.Substring(4)) : (double) int.Parse(current.Value);
          double num73 = Math.Round(Math.Abs(num72) * 4.62962963);
          if (num72 < 0.0)
            num73 = -num73;
          if (this.m_currShape.ShapeGuide.Count > 1 && current.Key.Contains("adjustValue"))
          {
            this.m_currShape.ShapeGuide.Clear();
            this.m_currShape.ShapeGuide.Add("adj1", "val " + num73.ToString());
            break;
          }
          this.m_currShape.ShapeGuide.Clear();
          this.m_currShape.ShapeGuide.Add("adj1", "val 50000");
          break;
        }
    }
  }

  private void AddOwnerShapeTextStack()
  {
    Dictionary<string, object> dictionary = new Dictionary<string, object>();
    dictionary.Add("m_currShape", (object) this.m_currShape);
    dictionary.Add("m_currTextBox", (object) this.m_currTextBox);
    Stack<string> stringStack = new Stack<string>();
    string[] array = new string[this.m_pictureOrShapeStack.Count];
    this.m_pictureOrShapeStack.CopyTo(array, 0);
    for (int index = array.Length - 1; index >= 0; --index)
      stringStack.Push(array[index]);
    dictionary.Add("m_pictureOrShapeStack", (object) stringStack);
    this.m_ownerShapeTextbodyStack.Push(dictionary);
    this.m_pictureOrShapeStack.Clear();
  }

  private void AddShapeTextbodyStack()
  {
    this.m_shapeTextbodyStack.Push(new Dictionary<string, object>()
    {
      {
        "m_currParagraph",
        this.m_currParagraph == null ? (object) (IWParagraph) null : (object) this.m_currParagraph
      },
      {
        "isPardTagpresent",
        (object) this.isPardTagpresent
      },
      {
        "m_bIsPictureOrShape",
        (object) this.m_bIsPictureOrShape
      },
      {
        "m_bInTable",
        (object) this.m_bInTable
      },
      {
        "m_previousLevel",
        (object) this.m_previousLevel
      },
      {
        "m_currentLevel",
        (object) this.m_currentLevel
      },
      {
        "m_bIsRow",
        (object) this.m_bIsRow
      },
      {
        "m_textFormatStack",
        this.m_textFormatStack == null ? (object) (Stack<RtfParser.TextFormat>) null : (object) new Stack<RtfParser.TextFormat>((IEnumerable<RtfParser.TextFormat>) this.m_textFormatStack.ToArray())
      }
    });
  }

  private void ClearPreviousTextbody()
  {
    this.m_currParagraph = (IWParagraph) null;
    this.isPardTagpresent = false;
    this.m_textFormatStack.Clear();
  }

  private void ResetOwnerShapeStack()
  {
    Dictionary<string, object> dictionary = this.m_ownerShapeTextbodyStack.Pop();
    this.m_currShape = dictionary["m_currShape"] as Shape;
    this.m_currTextBox = dictionary["m_currTextBox"] as WTextBox;
    Stack<string> stringStack = dictionary["m_pictureOrShapeStack"] as Stack<string>;
    string[] array = new string[stringStack.Count];
    stringStack.CopyTo(array, 0);
    for (int index = array.Length - 1; index >= 0; --index)
      this.m_pictureOrShapeStack.Push(array[index]);
    stringStack.Clear();
  }

  private void ResetShapeTextbodyStack()
  {
    Dictionary<string, object> dictionary = this.m_shapeTextbodyStack.Pop();
    this.m_currParagraph = dictionary["m_currParagraph"] as IWParagraph;
    this.isPardTagpresent = (bool) dictionary["isPardTagpresent"];
    this.m_bIsPictureOrShape = (bool) dictionary["m_bIsPictureOrShape"];
    this.m_previousLevel = (int) dictionary["m_previousLevel"];
    this.m_currentLevel = (int) dictionary["m_currentLevel"];
    this.m_bInTable = (bool) dictionary["m_bInTable"];
    this.m_bIsRow = (bool) dictionary["m_bIsRow"];
    this.m_textFormatStack = new Stack<RtfParser.TextFormat>((IEnumerable<RtfParser.TextFormat>) (dictionary["m_textFormatStack"] as Stack<RtfParser.TextFormat>).ToArray());
  }

  private void ParsePictureToken(string token, string tokenKey, string tokenValue)
  {
    if (!this.m_bIsShapePicture || this.m_bIsGroupShape)
      return;
    switch (tokenKey)
    {
      case "picscalex":
        this.m_picFormat.WidthScale = Convert.ToSingle(tokenValue);
        break;
      case "picscaley":
        this.m_picFormat.HeightScale = Convert.ToSingle(tokenValue);
        break;
      case "picwgoal":
        this.m_picFormat.Width = this.ExtractTwipsValue(tokenValue);
        break;
      case "pichgoal":
        this.m_picFormat.Height = this.ExtractTwipsValue(tokenValue);
        break;
      case "picw":
        this.m_picFormat.PicW = this.GetIntValue(tokenValue);
        break;
      case "pich":
        this.m_picFormat.picH = this.GetIntValue(tokenValue);
        break;
      case "rotation":
        this.m_picFormat.Rotation = tokenValue;
        break;
      case "object":
        this.m_bIsObject = true;
        this.m_objectStack.Push("\\");
        break;
    }
    if (!(tokenKey == "bin") && !(this.m_previousTokenKey == "bin") || tokenValue == null)
      return;
    this.AppendPictureToParagraph(token);
  }

  private int GetIntValue(string tokenValue)
  {
    int result;
    int.TryParse(tokenValue, out result);
    return result;
  }

  private void ParsePageVerticalAlignment(string token, string tokenKey, string tokenValue)
  {
    switch (tokenKey)
    {
      case "vertal":
      case "vertalb":
        this.m_secFormat.VertAlignment = PageAlignment.Bottom;
        break;
      case "vertalt":
        this.m_secFormat.VertAlignment = PageAlignment.Top;
        break;
      case "vertalc":
        this.m_secFormat.VertAlignment = PageAlignment.Middle;
        break;
      case "vertalj":
        this.m_secFormat.VertAlignment = PageAlignment.Justified;
        break;
    }
  }

  private void ParseOutLineLevel(string token, string tokenKey, string tokenValue)
  {
    switch (Convert.ToInt32(tokenValue))
    {
      case 0:
        this.m_currParagraphFormat.OutlineLevel = OutlineLevel.Level1;
        break;
      case 1:
        this.m_currParagraphFormat.OutlineLevel = OutlineLevel.Level2;
        break;
      case 2:
        this.m_currParagraphFormat.OutlineLevel = OutlineLevel.Level3;
        break;
      case 3:
        this.m_currParagraphFormat.OutlineLevel = OutlineLevel.Level4;
        break;
      case 4:
        this.m_currParagraphFormat.OutlineLevel = OutlineLevel.Level5;
        break;
      case 5:
        this.m_currParagraphFormat.OutlineLevel = OutlineLevel.Level6;
        break;
      case 6:
        this.m_currParagraphFormat.OutlineLevel = OutlineLevel.Level7;
        break;
      case 7:
        this.m_currParagraphFormat.OutlineLevel = OutlineLevel.Level8;
        break;
      case 8:
        this.m_currParagraphFormat.OutlineLevel = OutlineLevel.Level9;
        break;
      default:
        this.m_currParagraphFormat.OutlineLevel = OutlineLevel.BodyText;
        break;
    }
  }

  private void ParseParagraphBorders(string token, string tokenKey, string tokenValue)
  {
    switch (tokenKey)
    {
      case "brdrtbl":
        if (!this.m_bIsRow)
          break;
        if (this.m_bIsBorderBottom)
          this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.Cleared;
        if (this.m_bIsBorderLeft)
          this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.Cleared;
        if (this.m_bIsBorderTop)
          this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.Cleared;
        if (this.m_bIsBorderRight)
          this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.Cleared;
        if (this.m_bIsBorderDiagonalDown)
          this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.Cleared;
        if (this.m_bIsBorderDiagonalUp)
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.Cleared;
        if (this.m_bIsRowBorderBottom)
          this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.Cleared;
        if (this.m_bIsRowBorderLeft)
          this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.Cleared;
        if (this.m_bIsRowBorderTop)
          this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.Cleared;
        if (!this.m_bIsRowBorderRight)
          break;
        this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.Cleared;
        break;
      case "brdrt":
        this.m_bIsBorderTop = true;
        this.m_bIsBorderBottom = false;
        this.m_bIsBorderLeft = false;
        this.m_bIsBorderRight = false;
        break;
      case "brdrb":
        this.m_bIsBorderBottom = true;
        this.m_bIsBorderLeft = false;
        this.m_bIsBorderRight = false;
        this.m_bIsBorderTop = false;
        break;
      case "brdrl":
        this.m_bIsBorderLeft = true;
        this.m_bIsBorderRight = false;
        this.m_bIsBorderTop = false;
        this.m_bIsBorderBottom = false;
        break;
      case "brdrr":
        this.m_bIsBorderRight = true;
        this.m_bIsBorderBottom = false;
        this.m_bIsBorderLeft = false;
        this.m_bIsBorderTop = false;
        break;
      case "box":
        this.m_bIsBorderTop = true;
        this.m_bIsBorderBottom = true;
        this.m_bIsBorderLeft = true;
        this.m_bIsBorderRight = true;
        break;
      case "brdrsh":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.Shadow = true;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.Shadow = true;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.Shadow = true;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.Shadow = true;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.Shadow = true;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.Shadow = true;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.Shadow = true;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.Shadow = true;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.Shadow = true;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.Shadow = true;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.Shadow = true;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.Shadow = true;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.Shadow = true;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.Shadow = true;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.Shadow = true;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.Shadow = true;
        break;
      case "brdrs":
        if (this.m_bIsRow && !this.m_previousToken.StartsWith("brdrt") && !this.m_previousToken.StartsWith("brdrr") && !this.m_previousToken.StartsWith("brdrl") && !this.m_previousToken.StartsWith("brdrb"))
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.Single;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.Single;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.Single;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.Single;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.Single;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.Single;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.Single;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.Single;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.Single;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.Single;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.Single;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.Single;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.Single;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.Single;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.Single;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.Single;
        break;
      case "brdrth":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.Thick;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.Thick;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.Thick;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.Thick;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.Thick;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.Thick;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.Thick;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.Thick;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.Thick;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.Thick;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.Thick;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.Thick;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.Thick;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.Thick;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.Thick;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.Thick;
        break;
      case "brdrdb":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.Double;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.Double;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.Double;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.Double;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.Double;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.Double;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.Double;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.Double;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.Double;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.Double;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.Double;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.Double;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.Double;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.Double;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.Double;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.Double;
        break;
      case "brdrdot":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.Dot;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.Dot;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.Dot;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.Dot;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.Dot;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.Dot;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.Dot;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.Dot;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.Dot;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.Dot;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.Dot;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.Dot;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.Dot;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.Dot;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.Dot;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.Dot;
        break;
      case "brdrdashsm":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.DashSmallGap;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.DashSmallGap;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.DashSmallGap;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.DashSmallGap;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.DashSmallGap;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.DashSmallGap;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.DashSmallGap;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.DashSmallGap;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.DashSmallGap;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.DashSmallGap;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.DashSmallGap;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.DashSmallGap;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.DashSmallGap;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.DashSmallGap;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.DashSmallGap;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.DashSmallGap;
        break;
      case "brdrdash":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.DashLargeGap;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.DashLargeGap;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.DashLargeGap;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.DashLargeGap;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.DashLargeGap;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.DashLargeGap;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.DashLargeGap;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.DashLargeGap;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.DashLargeGap;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.DashLargeGap;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.DashLargeGap;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.DashLargeGap;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.DashLargeGap;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.DashLargeGap;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.DashLargeGap;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.DashLargeGap;
        break;
      case "brdrdashdd":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.DotDotDash;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.DotDotDash;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.DotDotDash;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.DotDotDash;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.DotDotDash;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.DotDotDash;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.DotDotDash;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.DotDotDash;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.DotDotDash;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.DotDotDash;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.DotDotDash;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.DotDotDash;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.DotDotDash;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.DotDotDash;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.DotDotDash;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.DotDotDash;
        break;
      case "brdrtnthmg":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.ThickThinMediumGap;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.ThickThinMediumGap;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.ThickThinMediumGap;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.ThickThinMediumGap;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.ThickThinMediumGap;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.ThickThinMediumGap;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.ThickThinMediumGap;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.ThickThinMediumGap;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.ThickThinMediumGap;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.ThickThinMediumGap;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.ThickThinMediumGap;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.ThickThinMediumGap;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.ThickThinMediumGap;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.ThickThinMediumGap;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.ThickThinMediumGap;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.ThickThinMediumGap;
        break;
      case "brdrtnthsg":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.ThinThinSmallGap;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.ThinThinSmallGap;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.ThinThinSmallGap;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.ThinThinSmallGap;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.ThinThinSmallGap;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.ThinThinSmallGap;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.ThinThinSmallGap;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.ThinThinSmallGap;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.ThinThinSmallGap;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.ThinThinSmallGap;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.ThinThinSmallGap;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.ThinThinSmallGap;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.ThinThinSmallGap;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.ThinThinSmallGap;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.ThinThinSmallGap;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.ThinThinSmallGap;
        break;
      case "brdrtnthtnsg":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.ThinThickThinSmallGap;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.ThinThickThinSmallGap;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.ThinThickThinSmallGap;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.ThinThickThinSmallGap;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.ThinThickThinSmallGap;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.ThinThickThinSmallGap;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.ThinThickThinSmallGap;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.ThinThickThinSmallGap;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.ThinThickThinSmallGap;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.ThinThickThinSmallGap;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.ThinThickThinSmallGap;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.ThinThickThinSmallGap;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.ThinThickThinSmallGap;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.ThinThickThinSmallGap;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.ThinThickThinSmallGap;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.ThinThickThinSmallGap;
        break;
      case "brdrthtnmg":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.ThickThinMediumGap;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.ThickThinMediumGap;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.ThinThickMediumGap;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.ThinThickMediumGap;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.ThinThickMediumGap;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.ThinThickMediumGap;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.ThinThickMediumGap;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.ThinThickMediumGap;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.ThinThickMediumGap;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.ThinThickMediumGap;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.ThinThickMediumGap;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.ThinThickMediumGap;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.ThinThickMediumGap;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.ThinThickMediumGap;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.ThinThickMediumGap;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.ThinThickMediumGap;
        break;
      case "brdrtnthlg":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.ThickThinLargeGap;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.ThickThinLargeGap;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.ThickThinLargeGap;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.ThickThinLargeGap;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.ThickThinLargeGap;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.ThickThinLargeGap;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.ThickThinLargeGap;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.ThickThinLargeGap;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.ThickThinLargeGap;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.ThickThinLargeGap;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.ThickThinLargeGap;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.ThickThinLargeGap;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.ThickThinLargeGap;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.ThickThinLargeGap;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.ThickThinLargeGap;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.ThickThinLargeGap;
        break;
      case "brdrthtnlg":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.ThinThickLargeGap;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.ThinThickLargeGap;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.ThinThickLargeGap;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.ThinThickLargeGap;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.ThinThickLargeGap;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.ThinThickLargeGap;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.ThinThickLargeGap;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.ThinThickLargeGap;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.ThinThickLargeGap;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.ThinThickLargeGap;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.ThinThickLargeGap;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.ThinThickLargeGap;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.ThinThickLargeGap;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.ThinThickLargeGap;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.ThinThickLargeGap;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.ThinThickLargeGap;
        break;
      case "brdremboss":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.Emboss3D;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.Emboss3D;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.Emboss3D;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.Emboss3D;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.Emboss3D;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.Emboss3D;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.Emboss3D;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.Emboss3D;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.Emboss3D;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.Emboss3D;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.Emboss3D;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.Emboss3D;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.Emboss3D;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.Emboss3D;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.Emboss3D;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.Emboss3D;
        break;
      case "brdrengrave":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.Engrave3D;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.Engrave3D;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.Engrave3D;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.Engrave3D;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.Engrave3D;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.Engrave3D;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.Engrave3D;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.Engrave3D;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.Engrave3D;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.Engrave3D;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.Engrave3D;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.Engrave3D;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.Engrave3D;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.Engrave3D;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.Engrave3D;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.Engrave3D;
        break;
      case "brdrnone":
      case "brdrnil":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.Cleared;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.Cleared;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.Cleared;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.Cleared;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.Cleared;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.Cleared;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.Cleared;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.Cleared;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.Cleared;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.Cleared;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.Cleared;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.Cleared;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.None;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.None;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.None;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.None;
        break;
      case "brdrtnthtnmg":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.ThickThickThinMediumGap;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.ThickThickThinMediumGap;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.ThickThickThinMediumGap;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.ThickThickThinMediumGap;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.ThickThickThinMediumGap;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.ThickThickThinMediumGap;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.ThickThickThinMediumGap;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.ThickThickThinMediumGap;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.ThickThickThinMediumGap;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.ThickThickThinMediumGap;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.ThickThickThinMediumGap;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.ThickThickThinMediumGap;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.ThickThickThinMediumGap;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.ThickThickThinMediumGap;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.ThickThickThinMediumGap;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.ThickThickThinMediumGap;
        break;
      case "brdrhair":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.Hairline;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.Hairline;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.Hairline;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.Hairline;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.Hairline;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.Hairline;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.Hairline;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.Hairline;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.Hairline;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.Hairline;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.Hairline;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.Hairline;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.Hairline;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.Hairline;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.Hairline;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.Hairline;
        break;
      case "brdrdashd":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.DotDash;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.DotDash;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.DotDash;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.DotDash;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.DotDash;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.DotDash;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.DotDash;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.DotDash;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.DotDash;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.DotDash;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.DotDash;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.DotDash;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.DotDash;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.DotDash;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.DotDash;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.DotDash;
        break;
      case "brdrtriple":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.Triple;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.Triple;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.Triple;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.Triple;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.Triple;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.Triple;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.Triple;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.Triple;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.Triple;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.Triple;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.Triple;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.Triple;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.Triple;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.Triple;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.Triple;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.Triple;
        break;
      case "brdrthtnsg":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.ThinThickSmallGap;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.ThinThickSmallGap;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.ThinThickSmallGap;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.ThinThickSmallGap;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.ThinThickSmallGap;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.ThinThickSmallGap;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.ThinThickSmallGap;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.ThinThickSmallGap;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.ThinThickSmallGap;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.ThinThickSmallGap;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.ThinThickSmallGap;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.ThinThickSmallGap;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.ThinThickSmallGap;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.ThinThickSmallGap;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.ThinThickSmallGap;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.ThinThickSmallGap;
        break;
      case "brdrtnthtnlg":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.ThinThickThinLargeGap;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.ThinThickThinLargeGap;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.ThinThickThinLargeGap;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.ThinThickThinLargeGap;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.ThinThickSmallGap;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.ThinThickSmallGap;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.ThinThickThinLargeGap;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.ThinThickThinLargeGap;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.ThinThickThinLargeGap;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.ThinThickThinLargeGap;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.ThinThickThinLargeGap;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.ThinThickThinLargeGap;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.ThinThickThinLargeGap;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.ThinThickThinLargeGap;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.ThinThickThinLargeGap;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.ThinThickThinLargeGap;
        break;
      case "brdrwavy":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.Wave;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.Wave;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.Wave;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.Wave;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.Wave;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.Wave;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.Wave;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.Wave;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.Wave;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.Wave;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.Wave;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.Wave;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.Wave;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.Wave;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.Wave;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.Wave;
        break;
      case "brdrwavydb":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.DoubleWave;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.DoubleWave;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.DoubleWave;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.DoubleWave;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.DoubleWave;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.DoubleWave;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.DoubleWave;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.DoubleWave;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.DoubleWave;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.DoubleWave;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.DoubleWave;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.DoubleWave;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.DoubleWave;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.DoubleWave;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.DoubleWave;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.DoubleWave;
        break;
      case "brdrdashdotstr":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.DashDotStroker;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.DashDotStroker;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.DashDotStroker;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.DashDotStroker;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.DashDotStroker;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.DashDotStroker;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.DashDotStroker;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.DashDotStroker;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.DashDotStroker;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.DashDotStroker;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.DashDotStroker;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.DashDotStroker;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.DashDotStroker;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.DashDotStroker;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.DashDotStroker;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.DashDotStroker;
        break;
      case "brdrinset":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.Inset;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.Inset;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.Inset;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.Inset;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.Inset;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.Inset;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.Inset;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.Inset;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.Inset;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.Inset;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.Inset;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.Inset;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.Inset;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.Inset;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.Inset;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.Inset;
        break;
      case "brdroutset":
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.BorderType = BorderStyle.Outset;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.BorderType = BorderStyle.Outset;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.BorderType = BorderStyle.Outset;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.BorderType = BorderStyle.Outset;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.BorderType = BorderStyle.Outset;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.BorderType = BorderStyle.Outset;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.BorderType = BorderStyle.Outset;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.BorderType = BorderStyle.Outset;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.BorderType = BorderStyle.Outset;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.BorderType = BorderStyle.Outset;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.BorderType = BorderStyle.Outset;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.BorderType = BorderStyle.Outset;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.BorderType = BorderStyle.Outset;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.BorderType = BorderStyle.Outset;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.BorderType = BorderStyle.Outset;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.BorderType = BorderStyle.Outset;
        break;
      case "brdrw":
        float twipsValue = this.ExtractTwipsValue(tokenValue);
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.LineWidth = twipsValue;
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.LineWidth = twipsValue;
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.LineWidth = twipsValue;
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.LineWidth = twipsValue;
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.LineWidth = twipsValue;
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.LineWidth = twipsValue;
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.LineWidth = twipsValue;
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.LineWidth = twipsValue;
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.LineWidth = twipsValue;
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.LineWidth = twipsValue;
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.LineWidth = twipsValue;
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.LineWidth = twipsValue;
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.LineWidth = twipsValue;
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.LineWidth = twipsValue;
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.LineWidth = twipsValue;
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.LineWidth = twipsValue;
        break;
      case "brdrcf":
        int int32 = Convert.ToInt32(tokenValue);
        this.CurrColorTable = new RtfColor();
        foreach (KeyValuePair<int, RtfColor> keyValuePair in this.m_colorTable)
        {
          if (keyValuePair.Key == int32)
            this.CurrColorTable = keyValuePair.Value;
        }
        if (this.m_bIsRow)
        {
          if (this.m_bIsHorizontalBorder)
            this.CurrRowFormat.Borders.Horizontal.Color = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
          if (this.m_bIsVerticalBorder)
            this.CurrRowFormat.Borders.Vertical.Color = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
          if (this.m_bIsRowBorderBottom)
            this.CurrRowFormat.Borders.Bottom.Color = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
          if (this.m_bIsRowBorderLeft)
            this.CurrRowFormat.Borders.Left.Color = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
          if (this.m_bIsRowBorderTop)
            this.CurrRowFormat.Borders.Top.Color = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
          if (this.m_bIsRowBorderRight)
            this.CurrRowFormat.Borders.Right.Color = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
          if (this.m_bIsBorderBottom)
            this.CurrCellFormat.Borders.Bottom.Color = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
          if (this.m_bIsBorderLeft)
            this.CurrCellFormat.Borders.Left.Color = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
          if (this.m_bIsBorderTop)
            this.CurrCellFormat.Borders.Top.Color = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
          if (this.m_bIsBorderRight)
            this.CurrCellFormat.Borders.Right.Color = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
          if (this.m_bIsBorderDiagonalDown)
            this.CurrCellFormat.Borders.DiagonalDown.Color = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
          if (!this.m_bIsBorderDiagonalUp)
            break;
          this.CurrCellFormat.Borders.DiagonalUp.Color = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
          break;
        }
        if (this.m_bIsBorderBottom)
          this.m_currParagraphFormat.Borders.Bottom.Color = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
        if (this.m_bIsBorderLeft)
          this.m_currParagraphFormat.Borders.Left.Color = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
        if (this.m_bIsBorderTop)
          this.m_currParagraphFormat.Borders.Top.Color = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
        if (!this.m_bIsBorderRight)
          break;
        this.m_currParagraphFormat.Borders.Right.Color = Color.FromArgb(this.CurrColorTable.RedN, this.CurrColorTable.GreenN, this.CurrColorTable.BlueN);
        break;
    }
  }

  private void ApplyCellFormatting(WTableCell cell, CellFormat cellFormat)
  {
    cell.CellFormat.SamePaddingsAsTable = !cellFormat.Paddings.HasKey(2) && !cellFormat.Paddings.HasKey(1) && !cellFormat.Paddings.HasKey(4) && !cellFormat.Paddings.HasKey(3);
    cell.CellFormat.BackColor = cellFormat.BackColor;
    this.ApplyBorder(cell.CellFormat.Borders.Left, cellFormat.Borders.Left);
    this.ApplyBorder(cell.CellFormat.Borders.Right, cellFormat.Borders.Right);
    this.ApplyBorder(cell.CellFormat.Borders.Top, cellFormat.Borders.Top);
    this.ApplyBorder(cell.CellFormat.Borders.Bottom, cellFormat.Borders.Bottom);
    this.ApplyBorder(cell.CellFormat.Borders.DiagonalDown, cellFormat.Borders.DiagonalDown);
    this.ApplyBorder(cell.CellFormat.Borders.DiagonalUp, cellFormat.Borders.DiagonalUp);
    cell.CellFormat.CellWidth = cellFormat.CellWidth;
    cell.CellFormat.FitText = cellFormat.FitText;
    cell.CellFormat.ForeColor = cellFormat.ForeColor;
    cell.CellFormat.PreferredWidth.WidthType = cellFormat.PreferredWidth.WidthType;
    if (cellFormat.PropertiesHash.ContainsKey(13))
    {
      cell.CellFormat.PreferredWidth.Width = cellFormat.PreferredWidth.Width;
    }
    else
    {
      cell.CellFormat.PreferredWidth.Width = cellFormat.CellWidth;
      cell.CellFormat.PreferredWidth.WidthType = FtsWidth.Point;
    }
    if (cellFormat.Paddings.HasKey(3))
      cell.CellFormat.Paddings.Bottom = cellFormat.Paddings.Bottom;
    if (cellFormat.Paddings.HasKey(4))
      cell.CellFormat.Paddings.Right = cellFormat.Paddings.Right;
    if (cellFormat.Paddings.HasKey(1))
      cell.CellFormat.Paddings.Left = cellFormat.Paddings.Left;
    if (cellFormat.Paddings.HasKey(2))
      cell.CellFormat.Paddings.Top = cellFormat.Paddings.Top;
    cell.CellFormat.TextDirection = cellFormat.TextDirection;
    cell.CellFormat.TextWrap = cellFormat.TextWrap;
    cell.CellFormat.VerticalAlignment = cellFormat.VerticalAlignment;
    cell.CellFormat.VerticalMerge = cellFormat.VerticalMerge;
    cell.CellFormat.HorizontalMerge = cellFormat.HorizontalMerge;
    cell.CellFormat.TextureStyle = cellFormat.TextureStyle;
    cell.CellFormat.HideMark = cellFormat.HideMark;
  }

  private void ApplyRowFormatting(WTableRow row, RowFormat rowFormat)
  {
    row.RowFormat.IsAutoResized = rowFormat.HasValue(103) && rowFormat.IsAutoResized;
    if (rowFormat.HasValue(121))
      row.RowFormat.Hidden = rowFormat.Hidden;
    if (rowFormat.HasValue(108))
      row.RowFormat.BackColor = rowFormat.BackColor;
    if (rowFormat.HasValue(104))
      row.RowFormat.Bidi = rowFormat.Bidi;
    if (rowFormat.HasValue(52))
      row.RowFormat.CellSpacing = rowFormat.CellSpacing;
    if (rowFormat.HasValue(105))
    {
      RowAlignment rowAlignment = rowFormat.HorizontalAlignment;
      if (row.RowFormat.Bidi)
      {
        if (rowFormat.HorizontalAlignment == RowAlignment.Left)
          rowAlignment = RowAlignment.Right;
        else if (rowAlignment == RowAlignment.Right)
          rowAlignment = RowAlignment.Left;
      }
      row.RowFormat.HorizontalAlignment = rowAlignment;
    }
    if (rowFormat.HasValue(2))
    {
      if ((double) rowFormat.Height < 0.0)
      {
        row.RowFormat.Height = -rowFormat.Height;
        row.HeightType = TableRowHeightType.Exactly;
      }
      else
        row.RowFormat.Height = rowFormat.Height;
    }
    if (rowFormat.HasValue(107))
      row.RowFormat.IsHeaderRow = rowFormat.IsHeaderRow;
    if (rowFormat.HasValue(106))
      row.RowFormat.IsBreakAcrossPages = rowFormat.IsBreakAcrossPages;
    if (rowFormat.HasValue(53))
      row.RowFormat.LeftIndent = (float) Math.Round((double) rowFormat.LeftIndent, 2);
    if (rowFormat.HasValue(110))
      row.RowFormat.TextureStyle = rowFormat.TextureStyle;
    if (rowFormat.HasValue(68))
      row.RowFormat.Positioning.DistanceFromLeft = rowFormat.Positioning.DistanceFromLeft;
    if (rowFormat.HasValue(69))
      row.RowFormat.Positioning.DistanceFromRight = rowFormat.Positioning.DistanceFromRight;
    if (rowFormat.HasValue(66))
      row.RowFormat.Positioning.DistanceFromTop = rowFormat.Positioning.DistanceFromTop;
    if (rowFormat.HasValue(67))
      row.RowFormat.Positioning.DistanceFromTop = rowFormat.Positioning.DistanceFromBottom;
    if (rowFormat.HasValue(62))
      row.RowFormat.Positioning.HorizPosition = rowFormat.Positioning.HorizPosition;
    if (rowFormat.HasValue(64 /*0x40*/))
      row.RowFormat.Positioning.HorizRelationTo = rowFormat.Positioning.HorizRelationTo;
    if (rowFormat.HasValue(63 /*0x3F*/))
      row.RowFormat.Positioning.VertPosition = rowFormat.Positioning.VertPosition;
    if (rowFormat.HasValue(65))
      row.RowFormat.Positioning.VertRelationTo = rowFormat.Positioning.VertRelationTo;
    if (rowFormat.HasValue(70))
      row.RowFormat.Positioning.AllowOverlap = rowFormat.Positioning.AllowOverlap;
    row.RowFormat.Paddings.Left = rowFormat.Paddings.Left;
    row.RowFormat.Paddings.Right = rowFormat.Paddings.Right;
    row.RowFormat.Paddings.Top = rowFormat.Paddings.Top;
    row.RowFormat.Paddings.Bottom = rowFormat.Paddings.Bottom;
    if (rowFormat.HasValue(14))
      row.RowFormat.GridBeforeWidth.Width = rowFormat.GridBeforeWidth.Width;
    if (rowFormat.HasValue(13))
      row.RowFormat.GridBeforeWidth.WidthType = rowFormat.GridBeforeWidth.WidthType;
    if (rowFormat.HasValue(16 /*0x10*/))
      row.RowFormat.GridAfterWidth.Width = rowFormat.GridAfterWidth.Width;
    if (rowFormat.HasValue(15))
      row.RowFormat.GridAfterWidth.WidthType = rowFormat.GridAfterWidth.WidthType;
    if (rowFormat.HasValue(12))
      this.CurrTable.TableFormat.PreferredWidth.Width = rowFormat.PreferredWidth.Width;
    if (rowFormat.HasValue(11))
      this.CurrTable.TableFormat.PreferredWidth.WidthType = rowFormat.PreferredWidth.WidthType;
    if (this.CurrTable.FirstRow == row && rowFormat.IsLeftIndentDefined)
      row.RowFormat.IsLeftIndentDefined = true;
    this.ApplyBorder(row.RowFormat.Borders.Left, rowFormat.Borders.Left);
    this.ApplyBorder(row.RowFormat.Borders.Right, rowFormat.Borders.Right);
    this.ApplyBorder(row.RowFormat.Borders.Top, rowFormat.Borders.Top);
    this.ApplyBorder(row.RowFormat.Borders.Bottom, rowFormat.Borders.Bottom);
    this.ApplyBorder(row.RowFormat.Borders.Horizontal, rowFormat.Borders.Horizontal);
    this.ApplyBorder(row.RowFormat.Borders.Vertical, rowFormat.Borders.Vertical);
    row.RowFormat.BeforeWidth = rowFormat.BeforeWidth;
  }

  private void ApplyBorder(Border destBorder, Border sourceBorder)
  {
    destBorder.IsRead = true;
    if (sourceBorder.BorderType == BorderStyle.Cleared || sourceBorder.BorderType == BorderStyle.None)
    {
      destBorder.BorderType = sourceBorder.BorderType;
    }
    else
    {
      destBorder.BorderType = sourceBorder.BorderType;
      destBorder.Color = sourceBorder.Color;
      destBorder.LineWidth = sourceBorder.LineWidth;
    }
    destBorder.IsRead = false;
  }

  private void AddNewParagraph(IWParagraph newParagraph)
  {
    this.UpdateTabsCollection(newParagraph.ParagraphFormat);
    if (newParagraph.ListFormat.ListType != ListType.NoList && !newParagraph.ListFormat.IsEmptyList && newParagraph.ListFormat.CurrentListLevel != null)
    {
      float tabSpaceAfter = newParagraph.ListFormat.CurrentListLevel.TabSpaceAfter;
      if ((double) tabSpaceAfter != 0.0 && !newParagraph.ParagraphFormat.Tabs.HasTabPosition(tabSpaceAfter))
      {
        newParagraph.ParagraphFormat.Tabs.AddTab(new Tab((IWordDocument) this.m_document)
        {
          DeletePosition = tabSpaceAfter * 20f
        });
        newParagraph.ParagraphFormat.Tabs.SortTabs();
      }
    }
    if (this.m_currSection == null)
    {
      this.m_currSection = this.m_document.AddSection();
      this.m_textBody = this.m_currSection.Body;
    }
    if (this.m_textBody == null)
      this.m_textBody = this.m_currSection.Body;
    this.m_textBody.Items.Add((IEntity) newParagraph);
    this.m_previousLevel = this.m_currentLevel;
    this.CopyTextFormatToCharFormat(newParagraph.BreakCharacterFormat, this.m_currTextFormat);
    this.m_currParagraph = (IWParagraph) null;
  }

  private void UpdateTabsCollection(WParagraphFormat paraFormat)
  {
    if (this.m_tabCollection.Count > 0 && paraFormat.Tabs.Count == 0)
    {
      foreach (KeyValuePair<int, TabFormat> tab in this.m_tabCollection)
        paraFormat.Tabs.AddTab(tab.Value.TabPosition, tab.Value.TabJustification, tab.Value.TabLeader);
    }
    else if (this.m_tabCollection.Count > paraFormat.Tabs.Count)
    {
      for (int key = paraFormat.Tabs.Count + 1; key <= this.m_tabCollection.Count; ++key)
      {
        if (this.m_tabCollection.ContainsKey(key))
          paraFormat.Tabs.AddTab(this.m_tabCollection[key].TabPosition, this.m_tabCollection[key].TabJustification, this.m_tabCollection[key].TabLeader);
      }
    }
    if (this.m_currentTableType != RtfTableType.None)
      return;
    this.UpdateDeleteTabsCollection(paraFormat, paraFormat.BaseFormat as WParagraphFormat);
  }

  private void UpdateDeleteTabsCollection(WParagraphFormat destFormat, WParagraphFormat baseFormat)
  {
    bool flag = false;
    for (; baseFormat != null; baseFormat = baseFormat.BaseFormat as WParagraphFormat)
    {
      for (int index1 = 0; index1 < baseFormat.Tabs.Count; ++index1)
      {
        for (int index2 = 0; index2 < destFormat.Tabs.Count; ++index2)
        {
          if ((double) baseFormat.Tabs[index1].Position == (double) destFormat.Tabs[index2].Position)
            flag = true;
        }
        if (!flag)
          destFormat.Tabs.AddTab().DeletePosition = baseFormat.Tabs[index1].Position * 20f;
        flag = false;
      }
    }
  }

  private void AddNewSection(IWSection newSection)
  {
    if (newSection.Columns.Count == 0)
    {
      this.CurrColumn = new Column((IWordDocument) this.m_document);
      this.CurrColumn.Space = 36f;
      newSection.Columns.Add(this.CurrColumn, true);
    }
    if (!this.IsSectionBreak)
      newSection.BreakCode = this.m_document.LastSection.BreakCode;
    this.IsSectionBreak = false;
    if (newSection.Owner != null)
      return;
    this.m_document.ChildEntities.Add((IEntity) newSection);
  }

  private float ExtractTwipsValue(string nValue)
  {
    return Convert.ToSingle((double) this.GetIntValue(nValue) / 20.0);
  }

  private float ExtractQuaterPointsValue(string nValue)
  {
    return Convert.ToSingle((double) this.GetIntValue(nValue) / 4.0);
  }

  private void SortTabCollection()
  {
    for (int key1 = 1; key1 < this.m_tabCollection.Count; ++key1)
    {
      for (int key2 = key1 + 1; key2 < this.m_tabCollection.Count + 1; ++key2)
      {
        if ((double) this.m_tabCollection[key1].TabPosition > (double) this.m_tabCollection[key2].TabPosition)
        {
          TabFormat tab = this.m_tabCollection[key1];
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

  private void CopyTextFormatToCharFormat(
    WCharacterFormat charFormat,
    RtfParser.TextFormat textFormat)
  {
    if ((double) textFormat.FontSize > 0.0)
    {
      charFormat.SetPropertyValue(3, (object) textFormat.FontSize);
      charFormat.SetPropertyValue(62, (object) textFormat.FontSize);
    }
    charFormat.TextColor = textFormat.FontColor;
    string tokenValue = this.DefaultFontIndex.ToString();
    if (this.m_currTable != null && textFormat.FontFamily.Length <= 0 && this.m_currentTableType == RtfTableType.None && charFormat.OwnerBase is WTextRange && !string.IsNullOrEmpty((charFormat.OwnerBase as WTextRange).Text) && this.groupOrder.Count >= 2 && this.groupOrder[0].ChildElements.Count > 0 && this.groupOrder[0].ChildElements[0] is Tokens && (this.groupOrder[0].ChildElements[0] as Tokens).TokenName == "rtf")
    {
      int num = -1;
      for (int index = this.groupOrder[0].ChildElements.Count - 1; index > 0; --index)
      {
        if (this.groupOrder[0].ChildElements[index] is Tokens)
        {
          Tokens childElement = this.groupOrder[0].ChildElements[index] as Tokens;
          if (childElement.TokenName == "hich" && childElement.TokenValue == null)
            num = index;
          else if (childElement.TokenName == "rtlch" && childElement.TokenValue == null)
            break;
        }
      }
      if (num != -1 && this.groupOrder[0].ChildElements[num - 1] is Tokens && (this.groupOrder[0].ChildElements[num - 1] as Tokens).TokenName == "af" && this.groupOrder[0].ChildElements[num - 2] is Tokens && (this.groupOrder[0].ChildElements[num - 2] as Tokens).TokenName == "loch" && (this.groupOrder[0].ChildElements[num - 2] as Tokens).TokenValue == null)
        tokenValue = (this.groupOrder[0].ChildElements[num - 1] as Tokens).TokenValue;
    }
    if (textFormat.FontFamily.Length > 0)
      charFormat.FontName = charFormat.FontNameAscii = charFormat.FontNameFarEast = charFormat.FontNameNonFarEast = charFormat.FontNameBidi = textFormat.FontFamily;
    else if (this.m_currentTableType == RtfTableType.None)
    {
      foreach (KeyValuePair<string, RtfFont> keyValuePair in this.m_fontTable)
      {
        string[] strArray = keyValuePair.Key.Split('f');
        if (strArray[strArray.Length - 1] == tokenValue)
          charFormat.FontName = keyValuePair.Value.FontName;
      }
    }
    if (textFormat.CharacterStyleName != string.Empty)
      charFormat.CharStyleName = textFormat.CharacterStyleName;
    charFormat.LocaleIdASCII = textFormat.LocalIdASCII;
    charFormat.LocaleIdFarEast = textFormat.LocalIdForEast;
    charFormat.LocaleIdBidi = textFormat.LidBi;
    charFormat.SetPropertyValue(17, (object) textFormat.Position);
    charFormat.Scaling = textFormat.Scaling;
    if (textFormat.Bold != RtfParser.ThreeState.Unknown)
      charFormat.Bold = textFormat.Bold == RtfParser.ThreeState.True;
    if (textFormat.Bidi != RtfParser.ThreeState.Unknown)
      charFormat.Bidi = textFormat.Bidi == RtfParser.ThreeState.True;
    if ((double) textFormat.CharacterSpacing != 0.0)
      charFormat.CharacterSpacing = textFormat.CharacterSpacing;
    if (textFormat.Italic != RtfParser.ThreeState.Unknown)
      charFormat.Italic = textFormat.Italic == RtfParser.ThreeState.True;
    if (textFormat.Underline != RtfParser.ThreeState.Unknown)
      charFormat.UnderlineStyle = textFormat.Underline != RtfParser.ThreeState.True ? UnderlineStyle.None : UnderlineStyle.Single;
    if (textFormat.BackColor != Color.Empty)
      charFormat.TextBackgroundColor = textFormat.BackColor;
    if (textFormat.ForeColor != Color.Empty)
      charFormat.ForeColor = textFormat.ForeColor;
    if (textFormat.HighlightColor != Color.Empty)
      charFormat.HighlightColor = textFormat.HighlightColor;
    charFormat.UnderlineStyle = textFormat.m_underlineStyle;
    if (textFormat.Shadow)
      charFormat.Shadow = textFormat.Shadow;
    if (textFormat.IsHiddenText)
      charFormat.Hidden = true;
    if (textFormat.SpecVanish)
      charFormat.SpecVanish = true;
    if (textFormat.m_subSuperScript != SubSuperScript.None)
      charFormat.SubSuperScript = textFormat.m_subSuperScript;
    if (textFormat.Strike != RtfParser.ThreeState.Unknown)
      charFormat.Strikeout = textFormat.Strike == RtfParser.ThreeState.True;
    if (textFormat.DoubleStrike != RtfParser.ThreeState.Unknown)
      charFormat.DoubleStrike = textFormat.DoubleStrike == RtfParser.ThreeState.True;
    if (textFormat.Emboss != RtfParser.ThreeState.Unknown)
      charFormat.Emboss = textFormat.Emboss == RtfParser.ThreeState.True;
    if (textFormat.Engrave != RtfParser.ThreeState.Unknown)
      charFormat.Engrave = textFormat.Engrave == RtfParser.ThreeState.True;
    if (textFormat.AllCaps != RtfParser.ThreeState.Unknown)
    {
      if (textFormat.AllCaps == RtfParser.ThreeState.True)
        charFormat.AllCaps = true;
      else if (textFormat.AllCaps == RtfParser.ThreeState.False)
        charFormat.AllCaps = false;
    }
    if (textFormat.SmallCaps == RtfParser.ThreeState.Unknown)
      return;
    if (textFormat.SmallCaps == RtfParser.ThreeState.True)
    {
      charFormat.SmallCaps = true;
    }
    else
    {
      if (textFormat.SmallCaps != RtfParser.ThreeState.False)
        return;
      charFormat.SmallCaps = false;
    }
  }

  private void ApplyParagraphFont(RtfFont rtfFontTable)
  {
    this.m_currTextFormat.FontFamily = rtfFontTable.FontName;
  }

  private void ApplyColorTable(RtfColor rtfColor)
  {
    this.m_currTextFormat.FontColor = Color.FromArgb(rtfColor.RedN, rtfColor.GreenN, rtfColor.BlueN);
  }

  internal bool StartsWithExt(string text, string value) => text.StartsWithExt(value);

  private void SetParsedElementFlag(string token)
  {
    token = Regex.Replace(token, "[\\d+$]", string.Empty);
    switch (token.Trim().ToLower())
    {
      case "\\chdpa":
        this.m_document.SetTriggerElement(ref this.m_document.m_notSupportedElementFlag, 0);
        break;
      case "\\chtime":
        this.m_document.SetTriggerElement(ref this.m_document.m_notSupportedElementFlag, 3);
        break;
      case "\\sectnum":
        this.m_document.SetTriggerElement(ref this.m_document.m_notSupportedElementFlag, 7);
        break;
      case "\\chdate":
        this.m_document.SetTriggerElement(ref this.m_document.m_notSupportedElementFlag, 11);
        break;
      case "\\chdpl":
        this.m_document.SetTriggerElement(ref this.m_document.m_notSupportedElementFlag, 13);
        break;
      case "\\linemod":
        this.m_document.SetTriggerElement(ref this.m_document.m_notSupportedElementFlag, 18);
        break;
      case "\\mmath":
        this.m_document.SetTriggerElement(ref this.m_document.m_notSupportedElementFlag, 19);
        break;
      case "\\revtbl":
        this.m_document.SetTriggerElement(ref this.m_document.m_notSupportedElementFlag, 30);
        break;
      case "\\bkmkend":
        this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_1, 4);
        break;
      case "\\bkmkstart":
        this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_1, 5);
        break;
      case "\\line":
        this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_1, 6);
        break;
      case "\\object":
        this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_1, 29);
        break;
      case "\\tb":
      case "\\tx":
        this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_2, 1);
        break;
      case "\\tab":
        this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_2, 9);
        break;
    }
  }

  private void SetShapeElementsFlag(int shapeTypeValue)
  {
    switch (shapeTypeValue)
    {
      case 136:
        this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_2, 17);
        break;
      case 202:
        this.m_document.SetTriggerElement(ref this.m_document.m_supportedElementFlag_2, 13);
        break;
    }
    if (shapeTypeValue >= 24 && shapeTypeValue <= 31 /*0x1F*/ || shapeTypeValue >= 137 && shapeTypeValue <= 165)
      this.m_document.SetTriggerElement(ref this.m_document.m_notSupportedElementFlag, 31 /*0x1F*/);
    else
      this.m_document.SetTriggerElement(ref this.m_document.m_notSupportedElementFlag, 25);
  }

  internal class TempShapeProperty
  {
    internal string m_drawingFieldName;
    internal string m_drawingFieldValue;

    internal TempShapeProperty(string drawingFieldName, string drawingFieldValue)
    {
      this.m_drawingFieldName = drawingFieldName;
      this.m_drawingFieldValue = drawingFieldValue;
    }
  }

  internal class TextFormat
  {
    private float m_position;
    private float m_scaling;
    private float m_charcterSpacing;
    internal RtfParser.ThreeState Bold;
    internal RtfParser.ThreeState Italic;
    internal RtfParser.ThreeState Underline;
    internal RtfParser.ThreeState Strike;
    internal RtfParser.ThreeState DoubleStrike;
    internal RtfParser.ThreeState Emboss;
    internal RtfParser.ThreeState Engrave;
    internal SubSuperScript m_subSuperScript;
    internal Color FontColor;
    internal Color BackColor;
    internal Color ForeColor;
    private Color m_highlightColor;
    internal string FontFamily;
    internal float FontSize;
    internal HorizontalAlignment TextAlign;
    internal BuiltinStyle Style;
    internal RtfParser.ThreeState Bidi;
    internal RtfParser.ThreeState AllCaps;
    internal RtfParser.ThreeState SmallCaps;
    internal UnderlineStyle m_underlineStyle;
    internal bool Shadow;
    internal bool IsHiddenText;
    internal bool SpecVanish;
    internal string CharacterStyleName;
    private short m_localIdASCII;
    private short m_localIdForEast;
    private short m_lidBi;

    internal short LocalIdASCII
    {
      get => this.m_localIdASCII;
      set => this.m_localIdASCII = value;
    }

    internal short LocalIdForEast
    {
      get => this.m_localIdForEast;
      set => this.m_localIdForEast = value;
    }

    internal float CharacterSpacing
    {
      get => this.m_charcterSpacing;
      set => this.m_charcterSpacing = value;
    }

    internal short LidBi
    {
      get => this.m_lidBi;
      set => this.m_lidBi = value;
    }

    internal float Position
    {
      get => this.m_position;
      set => this.m_position = value;
    }

    internal float Scaling
    {
      get => this.m_scaling;
      set => this.m_scaling = value;
    }

    internal Color HighlightColor
    {
      get => this.m_highlightColor;
      set => this.m_highlightColor = value;
    }

    internal TextFormat()
    {
      this.FontSize = 0.0f;
      this.Bold = RtfParser.ThreeState.False;
      this.Italic = RtfParser.ThreeState.False;
      this.Underline = RtfParser.ThreeState.False;
      this.Strike = RtfParser.ThreeState.False;
      this.DoubleStrike = RtfParser.ThreeState.False;
      this.Emboss = RtfParser.ThreeState.False;
      this.Engrave = RtfParser.ThreeState.False;
      this.FontColor = Color.Empty;
      this.BackColor = Color.Empty;
      this.ForeColor = Color.Empty;
      this.HighlightColor = Color.Empty;
      this.FontFamily = string.Empty;
      this.TextAlign = HorizontalAlignment.Left;
      this.Style = BuiltinStyle.Normal;
      this.Bidi = RtfParser.ThreeState.Unknown;
      this.m_underlineStyle = UnderlineStyle.None;
      this.m_subSuperScript = SubSuperScript.None;
      this.AllCaps = RtfParser.ThreeState.False;
      this.SmallCaps = RtfParser.ThreeState.False;
      this.CharacterStyleName = string.Empty;
      this.Position = 0.0f;
      this.LocalIdASCII = (short) 1033;
      this.LocalIdForEast = (short) 1033;
      this.LidBi = (short) 1025;
      this.Scaling = 100f;
    }

    public RtfParser.TextFormat Clone() => (RtfParser.TextFormat) this.MemberwiseClone();
  }

  internal class SecionFormat
  {
    private float m_leftMargin = 72f;
    private float m_rightMargin = 72f;
    private float m_topMargin = 72f;
    private float m_bottomMargin = 72f;
    internal float HeaderDistance = Convert.ToSingle(36);
    internal float FooterDistance = Convert.ToSingle(36);
    internal bool DifferentFirstPage;
    internal bool DifferentOddAndEvenPage;
    internal bool IsFrontPageBorder;
    internal float DefaultTabWidth = Convert.ToSingle(36);
    internal PageAlignment VertAlignment;
    internal SizeF pageSize = PageSize.Letter;
    internal PageOrientation m_pageOrientation;
    private int m_firstPageTray;
    private int m_otherPagesTray;

    internal float LeftMargin
    {
      get => this.m_leftMargin;
      set => this.m_leftMargin = value;
    }

    internal float RightMargin
    {
      get => this.m_rightMargin;
      set => this.m_rightMargin = value;
    }

    internal float TopMargin
    {
      get => this.m_topMargin;
      set => this.m_topMargin = value;
    }

    internal float BottomMargin
    {
      get => this.m_bottomMargin;
      set => this.m_bottomMargin = value;
    }

    internal int FirstPageTray
    {
      get => this.m_firstPageTray;
      set => this.m_firstPageTray = value;
    }

    internal int OtherPagesTray
    {
      get => this.m_otherPagesTray;
      set => this.m_otherPagesTray = value;
    }
  }

  internal class PictureFormat
  {
    internal float Height;
    internal float Width;
    internal float HeightScale;
    internal float WidthScale;
    internal int PicW;
    internal int picH;
    internal int Zorder;
    internal string Rotation = string.Empty;
  }

  internal class ShapeFormat
  {
    internal float m_width;
    internal float m_height;
    internal float m_left;
    internal float m_right;
    internal float m_top;
    internal float m_bottom;
    internal float m_horizPosition;
    internal float m_vertPosition;
    internal int m_uniqueId;
    internal int m_zOrder;
    internal bool m_isBelowText;
    internal bool m_isInHeader;
    internal bool m_isLockAnchor;
    internal ShapeHorizontalAlignment m_horizAlignment;
    internal TextWrappingType m_textWrappingType;
    internal TextWrappingStyle m_textWrappingStyle;
    internal VerticalOrigin m_vertOrgin;
    internal HorizontalOrigin m_horizOrgin;
    private SizeF m_size;

    internal SizeF Size => this.GetSizeValue();

    private SizeF GetSizeValue()
    {
      this.m_width = (float) (((double) this.m_right - (double) this.m_left) / 20.0);
      this.m_height = (float) (((double) this.m_bottom - (double) this.m_top) / 20.0);
      this.m_size = new SizeF(this.m_width, this.m_height);
      return this.m_size;
    }
  }

  internal enum ThreeState
  {
    False,
    True,
    Unknown,
  }
}
