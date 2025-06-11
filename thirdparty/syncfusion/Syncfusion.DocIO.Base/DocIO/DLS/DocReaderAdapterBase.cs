// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.DocReaderAdapterBase
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class DocReaderAdapterBase
{
  private const int DEF_WMFPLACEABLEHEADER_KEY = -1698247209;
  protected List<WPicture> m_listPic;
  protected WTextBody m_textBody;
  protected ITextBodyItem m_currParagraph;
  private bool m_cellFinished;
  private bool m_rowFinished;
  private WTable m_currTable;
  private Stack<WTable> m_tablesNested = new Stack<WTable>();
  protected Stack<WTextBody> m_nestedTextBodies = new Stack<WTextBody>();
  private WField m_currField;
  private Stack<WField> m_fieldStack = new Stack<WField>();
  private bool m_isPostFixBkmkStart;
  protected bool m_finalize;
  private WordChunkType m_prevChunkType;
  private BookmarkInfo m_bookmarkInfo;
  private List<WTable> nestedTable = new List<WTable>();
  protected WordDocument DocumentEx;

  protected WParagraph CurrentParagraph
  {
    get
    {
      if (this.m_currParagraph == null)
        this.m_currParagraph = (ITextBodyItem) this.m_textBody.AddParagraph();
      return this.m_currParagraph as WParagraph;
    }
  }

  protected WField CurrentField
  {
    get
    {
      this.m_currField = this.m_fieldStack.Count <= 0 ? (WField) null : this.m_fieldStack.Peek();
      return this.m_currField;
    }
  }

  internal List<WPicture> ListPictures
  {
    get
    {
      if (this.m_listPic == null)
        this.m_listPic = new List<WPicture>();
      return this.m_listPic;
    }
  }

  internal void Init(WordDocument doc)
  {
    this.DocumentEx = doc;
    this.m_textBody = (WTextBody) null;
    this.m_finalize = true;
  }

  protected void ReadTextBody(WordReaderBase reader, WTextBody textBody)
  {
    this.m_textBody = textBody;
    HeaderFooter headerFooter = textBody is HeaderFooter ? textBody as HeaderFooter : (HeaderFooter) null;
    this.m_currParagraph = (ITextBodyItem) null;
    while (!this.EndOfTextBody(reader, reader.ReadChunk()))
    {
      this.Preparation(reader);
      this.ProcessChunk(reader, headerFooter);
    }
    if (!this.m_finalize)
      return;
    this.Finalize(reader);
  }

  protected virtual bool EndOfTextBody(WordReaderBase reader, WordChunkType chunkType)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  protected virtual void Preparation(WordReaderBase reader)
  {
    int prevLevel = this.m_nestedTextBodies.Count == 0 ? 0 : this.m_tablesNested.Count + 1;
    if (this.m_prevChunkType != WordChunkType.TableRow && prevLevel > reader.PAPXSprms.GetInt(26185, 1))
    {
      if (prevLevel > 0)
        reader.PAPXSprms.SetIntValue(26185, prevLevel);
      else
        reader.PAPXSprms.RemoveValue(26185);
    }
    DocReaderAdapterBase.PrepareTableInfo prepti = new DocReaderAdapterBase.PrepareTableInfo(reader, prevLevel);
    this.PrepareParagraph(reader, ref prepti);
    this.PrepareTable((IWordReaderBase) reader, ref prepti);
    this.SetPostfixBkmks();
  }

  private void ProcessChunk(WordReaderBase reader, HeaderFooter headerFooter)
  {
    this.ProcessBookmarkAfterParaEnd(reader);
    switch (reader.ChunkType)
    {
      case WordChunkType.Text:
        if (reader is IWordReader)
        {
          IWordReader wordReader = reader as IWordReader;
          if (wordReader.IsEndnote || wordReader.IsFootnote)
          {
            int splittedTextLength = (reader as WordReader).CustomFnSplittedTextLength;
            if (splittedTextLength > -1)
            {
              this.ReadCustomFootnote(reader, splittedTextLength, 0);
              break;
            }
            this.ReadFootnote(reader);
            break;
          }
        }
        this.ReadText(reader);
        break;
      case WordChunkType.ParagraphEnd:
        this.ReadParagraphEnd(reader);
        break;
      case WordChunkType.PageBreak:
        this.ReadBreak(reader, BreakType.PageBreak);
        break;
      case WordChunkType.ColumnBreak:
        this.ReadBreak(reader, BreakType.ColumnBreak);
        break;
      case WordChunkType.DocumentEnd:
        this.ReadDocumentEnd(reader);
        break;
      case WordChunkType.Image:
        WField formField1 = this.m_fieldStack.Count > 0 ? this.m_fieldStack.Peek() : (WField) null;
        if (formField1 != null && FieldTypeDefiner.IsFormField(formField1.FieldType) && reader.CHPXSprms.GetBoolean(2054, false))
        {
          FormField formField2 = reader.GetFormField(formField1.FieldType);
          FormFieldPropertiesConverter.ReadFormFieldProperties(formField1 as WFormField, formField2);
          (formField1 as WFormField).HasFFData = true;
          break;
        }
        this.ReadImage(reader);
        break;
      case WordChunkType.Shape:
        this.ReadShape(reader, headerFooter);
        break;
      case WordChunkType.Table:
        this.ReadTable(reader);
        this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_2, 11);
        break;
      case WordChunkType.TableRow:
        this.ReadTableRow(reader);
        this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_2, 16 /*0x10*/);
        break;
      case WordChunkType.TableCell:
        this.ReadTableCell(reader);
        this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_2, 10);
        break;
      case WordChunkType.Footnote:
        switch (reader)
        {
          case WordFootnoteReader _:
          case WordEndnoteReader _:
            this.ReadFootnoteMarker(reader);
            break;
          case IWordReader _:
            this.ReadFootnote(reader);
            break;
        }
        break;
      case WordChunkType.FieldBeginMark:
        if (reader.CurrentBookmark != null)
          this.AppendBookmark(reader.CurrentBookmark, reader.IsBookmarkStart);
        this.ReadFldBeginMark(reader);
        break;
      case WordChunkType.FieldSeparator:
        this.InsertFldSeparator(reader);
        break;
      case WordChunkType.FieldEndMark:
        this.InsertFldEndMark(reader);
        break;
      case WordChunkType.Tab:
        this.ReadTab(reader);
        this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_2, 9);
        break;
      case WordChunkType.Annotation:
        this.ReadAnnotation(reader);
        this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 4);
        break;
      case WordChunkType.LineBreak:
        this.ReadBreak(reader, BreakType.LineBreak);
        break;
      case WordChunkType.Symbol:
        if (reader is IWordReader)
        {
          IWordReader wordReader = reader as IWordReader;
          if (wordReader.IsEndnote || wordReader.IsFootnote)
          {
            this.ReadFootnote(reader);
            break;
          }
        }
        this.ReadSymbol(reader);
        break;
      case WordChunkType.CurrentPageNumber:
        this.ReadCurrentPageNumber(reader);
        break;
      default:
        throw new InvalidOperationException("Unsupported WordChunkType occured");
    }
    this.ProcessCommText(reader, this.m_currParagraph as WParagraph);
    this.ProcessBookmarks(reader);
    this.m_prevChunkType = reader.ChunkType;
  }

  private void ReadCustomFootnote(WordReaderBase reader, int splittedTextLength, int startPos)
  {
    this.Addtext(reader, reader.TextChunk.Substring(startPos, splittedTextLength), true);
    this.ReadFootnote(reader);
    int num = 0;
    if (this.CurrentParagraph != null && this.CurrentParagraph.ChildEntities.LastItem is WFootnote)
    {
      ++reader.m_docInfo.TablesData.Footnotes.m_footEndnoteRefIndex;
      num = (this.CurrentParagraph.ChildEntities.LastItem as WFootnote).m_strCustomMarker.Length;
    }
    startPos += splittedTextLength + num;
    int length = reader.TextChunk.Length - startPos;
    if (reader.TextChunk.Length > startPos && startPos + length <= reader.TextChunk.Length)
    {
      string text = reader.TextChunk.Substring(startPos, length);
      IWordReader wordReader = reader as IWordReader;
      if (wordReader.IsEndnote || wordReader.IsFootnote)
      {
        splittedTextLength = (reader as WordReader).CustomFnSplittedTextLength;
        (reader as WordReader).CustomFnSplittedTextLength -= startPos;
        splittedTextLength = (reader as WordReader).CustomFnSplittedTextLength;
        if (splittedTextLength > -1)
          this.ReadCustomFootnote(reader, splittedTextLength, startPos);
        else
          this.ReadFootnote(reader);
      }
      else
        this.Addtext(reader, text, true);
    }
    (reader as WordReader).CustomFnSplittedTextLength = -1;
  }

  private void ProcessBookmarkAfterParaEnd(WordReaderBase reader)
  {
    if (reader.BookmarkAfterParaEnd == null)
      return;
    this.AppendBookmark(reader.BookmarkAfterParaEnd, reader.IsBKMKStartAfterParaEnd);
    reader.BookmarkAfterParaEnd = (BookmarkInfo) null;
  }

  private void ReadFootnoteMarker(WordReaderBase reader)
  {
    IWTextRange wtextRange = this.CurrentParagraph.AppendText(reader.TextChunk);
    this.ReadCharacterFormat(reader, wtextRange.CharacterFormat);
  }

  private void PrepareParagraph(
    WordReaderBase reader,
    ref DocReaderAdapterBase.PrepareTableInfo prepti)
  {
  }

  private void PrepareTable(
    IWordReaderBase reader,
    ref DocReaderAdapterBase.PrepareTableInfo prepti)
  {
    if (prepti.InTable)
    {
      if (this.m_cellFinished && reader.ChunkType != WordChunkType.TableRow)
        this.AppendTableCell(ref prepti);
      else if (this.m_rowFinished && prepti.State != PrepareTableState.LeaveTable)
        this.AppendTableRow();
    }
    switch (prepti.State)
    {
      case PrepareTableState.EnterTable:
        if (prepti.PrevLevel == 0)
          this.m_nestedTextBodies.Push(this.m_textBody);
        this.EnsureUpperTable(prepti.Level, reader);
        break;
      case PrepareTableState.LeaveTable:
        this.EnsureLowerTable(prepti.Level, reader);
        break;
    }
    if (this.m_cellFinished)
      this.m_cellFinished = false;
    if (!this.m_rowFinished)
      return;
    this.m_currParagraph = (ITextBodyItem) null;
    this.m_rowFinished = false;
  }

  private void EnsureLowerTable(int level, IWordReaderBase reader)
  {
    while (this.m_tablesNested.Count > level)
      this.m_tablesNested.Pop();
    if (this.m_currTable == null)
      return;
    if (level == 0)
    {
      this.m_textBody = this.m_nestedTextBodies.Pop();
      if (this.m_currTable.Owner == null)
        this.m_textBody.Items.Add((IEntity) this.m_currTable);
      foreach (WTable table in this.nestedTable)
      {
        if (table.PreferredTableWidth.WidthType == FtsWidth.Auto)
          table.UpdateGridSpan();
        else
          table.UpdateGridSpan(table);
      }
      if (this.m_currTable.PreferredTableWidth.WidthType == FtsWidth.Auto)
        this.m_currTable.UpdateGridSpan();
      else
        this.m_currTable.UpdateGridSpan(this.m_currTable);
      if (this.nestedTable.Count > 0)
        this.nestedTable.Clear();
      this.UpdateTableGridAfterValue(this.m_currTable, reader);
      this.m_currParagraph = (ITextBodyItem) null;
      this.m_currTable = (WTable) null;
    }
    else
    {
      WTable currTable = this.m_currTable;
      this.m_currTable = this.m_tablesNested.Pop();
      this.m_textBody = (WTextBody) this.m_currTable.LastCell;
      if (this.m_currTable.Owner == null)
        this.m_textBody.Items.Add((IEntity) currTable);
      this.UpdateTableGridAfterValue(currTable, reader);
      this.nestedTable.Add(currTable);
      this.m_currParagraph = (ITextBodyItem) null;
    }
  }

  private void EnsureUpperTable(int level, IWordReaderBase reader)
  {
    do
    {
      if (this.m_currTable != null)
        this.m_tablesNested.Push(this.m_currTable);
      this.m_currTable = new WTable((IWordDocument) this.DocumentEx);
      reader.TableRowWidthStack.Push(new Dictionary<WTableRow, short>());
      reader.MaximumTableRowWidth.Add((short) 0);
      this.AppendTableRow();
    }
    while (this.m_tablesNested.Count < level - 1);
  }

  private void UpdateTableGridAfterValue(WTable table, IWordReaderBase reader)
  {
    if (reader.TableRowWidthStack.Count <= 0 || reader.MaximumTableRowWidth.Count <= 0)
      return;
    Dictionary<WTableRow, short> dictionary = reader.TableRowWidthStack.Pop();
    if (dictionary.Count != table.Rows.Count)
      return;
    short num = reader.MaximumTableRowWidth[reader.MaximumTableRowWidth.Count - 1];
    foreach (WTableRow key in dictionary.Keys)
    {
      if ((int) num > (int) dictionary[key])
        key.RowFormat.AfterWidth = (float) ((int) num - (int) dictionary[key]) / 20f;
    }
    reader.MaximumTableRowWidth.RemoveAt(reader.MaximumTableRowWidth.Count - 1);
  }

  private void AppendTableRow()
  {
    this.m_textBody = (WTextBody) this.m_currTable.AddRow(false, false).AddCell(false);
  }

  private void AppendTableCell(ref DocReaderAdapterBase.PrepareTableInfo prepti)
  {
    this.m_textBody = (WTextBody) this.m_currTable.LastRow.AddCell(false);
  }

  protected virtual void Finalize(WordReaderBase reader)
  {
    int prevLevel = this.m_nestedTextBodies.Count == 0 ? 0 : this.m_tablesNested.Count + 1;
    if (!(reader is WordSubdocumentReader) || !(reader as WordSubdocumentReader).IsNextItemPos)
    {
      DocReaderAdapterBase.PrepareTableInfo prepti = new DocReaderAdapterBase.PrepareTableInfo(reader, prevLevel);
      this.PrepareTable((IWordReaderBase) reader, ref prepti);
    }
    if (reader is WordTextBoxReader || reader.ChunkType == WordChunkType.SectionEnd)
    {
      reader.RestoreBookmark();
      this.m_bookmarkInfo = (BookmarkInfo) null;
    }
    else
      this.SetPostfixBkmks();
  }

  private void SetPostfixBkmks()
  {
    if (this.m_bookmarkInfo == null)
      return;
    this.AppendBookmark(this.m_bookmarkInfo, this.m_isPostFixBkmkStart);
    this.m_bookmarkInfo = (BookmarkInfo) null;
  }

  private void ProcessBookmarks(WordReaderBase reader)
  {
    if (reader.CurrentBookmark == null || !(reader.CurrentBookmark.Name != "_PictureBullets"))
      return;
    bool cellGroupBookmark = reader.CurrentBookmark.IsCellGroupBookmark;
    if (reader.ChunkType == WordChunkType.ParagraphEnd && this.IsParagraphBefore(reader) || reader.ChunkType == WordChunkType.TableCell || reader.ChunkType == WordChunkType.TableRow)
    {
      if (reader is WordTextBoxReader && reader.ChunkType == WordChunkType.ParagraphEnd || reader.CurrentBookmark.IsCellGroupBookmark && !reader.IsBookmarkStart && reader.CurrentBookmark.EndPos > reader.EndTextPos)
        reader.RestoreBookmark();
      else if (reader.CurrentBookmark.IsCellGroupBookmark && !reader.IsBookmarkStart && (reader.ChunkType == WordChunkType.TableCell || reader.ChunkType == WordChunkType.TableRow))
      {
        WTableRow lastRow = this.m_currTable.LastRow;
        WTableCell wtableCell = lastRow.Cells.Count <= (int) reader.CurrentBookmark.EndCellIndex ? lastRow.Cells[lastRow.Cells.Count - 1] : lastRow.Cells[(int) reader.CurrentBookmark.EndCellIndex];
        BookmarkEnd bookmarkEnd = (wtableCell.LastParagraph == null ? wtableCell.AddParagraph() : wtableCell.LastParagraph).AppendBookmarkEnd(reader.CurrentBookmark.Name);
        bookmarkEnd.IsCellGroupBkmk = cellGroupBookmark;
        if (reader.ChunkType == WordChunkType.TableRow)
          bookmarkEnd.IsAfterRowMark = true;
      }
      else
      {
        this.m_bookmarkInfo = reader.CurrentBookmark;
        this.m_isPostFixBkmkStart = reader.IsBookmarkStart;
      }
    }
    else if (reader.ChunkType != WordChunkType.FieldBeginMark)
    {
      if (reader.ChunkType != WordChunkType.ParagraphEnd)
      {
        this.AppendBookmark(reader.CurrentBookmark, reader.IsBookmarkStart);
        reader.BookmarkAfterParaEnd = (BookmarkInfo) null;
      }
      else
      {
        reader.BookmarkAfterParaEnd = reader.CurrentBookmark;
        reader.IsBKMKStartAfterParaEnd = reader.IsBookmarkStart;
      }
    }
    reader.CurrentBookmark = (BookmarkInfo) null;
  }

  private bool IsParagraphBefore(WordReaderBase reader)
  {
    if (reader.IsBookmarkStart)
    {
      string textChunk = reader.TextChunk;
      int num = reader.CurrentTextPosition - textChunk.Length;
      if (textChunk.Substring(0, reader.CurrentBookmark.StartPos - num) == ControlChar.CarriegeReturn)
        return true;
    }
    return false;
  }

  private void AppendBookmark(BookmarkInfo bkmrInfo, bool isBookmarkStart)
  {
    if (isBookmarkStart)
    {
      BookmarkStart bookmarkStart = this.CurrentParagraph.AppendBookmarkStart(bkmrInfo.Name);
      this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_1, 5);
      bookmarkStart.ColumnFirst = bkmrInfo.StartCellIndex;
      bookmarkStart.ColumnLast = bkmrInfo.EndCellIndex;
      bookmarkStart.IsCellGroupBkmk = bkmrInfo.IsCellGroupBookmark;
    }
    else
    {
      BookmarkEnd bookmarkEnd = this.CurrentParagraph.AppendBookmarkEnd(bkmrInfo.Name);
      this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_1, 4);
      bookmarkEnd.IsCellGroupBkmk = bkmrInfo.IsCellGroupBookmark;
    }
  }

  private void ReadText(WordReaderBase reader)
  {
    if (reader.CHPXSprms.GetBoolean(2133, false) && reader.TextChunk.Length == 1)
    {
      switch (reader.TextChunk[0])
      {
        case '\u0006':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 18);
          break;
        case '\n':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 0);
          break;
        case '\v':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 29);
          break;
        case '\f':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 7);
          break;
        case '\u000E':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 1);
          break;
        case '\u000F':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 12);
          break;
        case '\u0010':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 14);
          break;
        case '\u0016':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 17);
          break;
        case '\u0017':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 16 /*0x10*/);
          break;
        case '\u0018':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 20);
          break;
        case '\u0019':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 21);
          break;
        case '\u001A':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 27);
          break;
        case '\u001B':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 3);
          break;
        case '\u001C':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 9);
          break;
        case '\u001D':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 11);
          break;
        case '\u001E':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 13);
          break;
        case '!':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 23);
          break;
        case '"':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_1, 0);
          break;
        case '#':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_1, 1);
          break;
        case '$':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 2);
          break;
        case '%':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 22);
          break;
        case '&':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 8);
          break;
        case ')':
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 26);
          break;
      }
    }
    this.Addtext(reader, reader.TextChunk, false);
  }

  internal void Addtext(WordReaderBase reader, string text, bool isFromFootNoteSplittedText)
  {
    string text1 = isFromFootNoteSplittedText ? text : reader.TextChunk;
    switch (text1)
    {
      case null:
        break;
      case "":
        break;
      default:
        if (text1 == ControlChar.CarriegeReturn)
        {
          Break @break = new Break((IWordDocument) this.DocumentEx, BreakType.LineBreak);
          @break.TextRange.Text = ControlChar.CarriegeReturn;
          this.AddItem((ParagraphItem) @break, (IWParagraph) this.CurrentParagraph);
          this.ReadCharacterFormat(reader, @break.CharacterFormat);
          this.CheckTrackChanges((ParagraphItem) @break, reader);
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_1, 7);
          break;
        }
        if (text1.Contains('\u001F'.ToString()))
        {
          text = text1;
          int num = text.IndexOf('\u001F');
          while (true)
          {
            do
            {
              switch (num)
              {
                case -1:
                  goto label_11;
                case 0:
                  ++num;
                  break;
              }
              this.AddTextRange(reader, text.Substring(0, num));
              text = text.Substring(num);
              num = text.IndexOf('\u001F');
            }
            while (num != -1 || !(text != string.Empty));
            this.AddTextRange(reader, text);
            this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_2, 7);
          }
label_11:
          break;
        }
        this.AddTextRange(reader, text1);
        break;
    }
  }

  private void AddTextRange(WordReaderBase reader, string text)
  {
    IWTextRange wtextRange = (IWTextRange) new WTextRange((IWordDocument) this.DocumentEx);
    wtextRange.Text = text;
    this.AddItem(wtextRange as ParagraphItem, (IWParagraph) this.CurrentParagraph);
    this.ReadCharacterFormat(reader, wtextRange.CharacterFormat);
    this.CheckTrackChanges(wtextRange as ParagraphItem, reader);
    if (!(this.CurrentField is WTextFormField) || !wtextRange.CharacterFormat.PropertiesHash.ContainsKey(106))
      return;
    wtextRange.CharacterFormat.PropertiesHash.Remove(106);
  }

  private void ReadParagraphEnd(WordReaderBase reader)
  {
    this.ReadListFormat(reader, this.CurrentParagraph.ListFormat);
    this.ReadCharacterFormat(reader, this.CurrentParagraph.BreakCharacterFormat);
    this.ReadParagraphFormat(reader, (IWParagraph) this.CurrentParagraph);
    this.CheckTrackChanges(this.CurrentParagraph, reader);
    this.m_currParagraph = (ITextBodyItem) null;
  }

  private void ReadSymbol(WordReaderBase reader)
  {
    SymbolDescriptor symbolDescriptor = reader.GetSymbolDescriptor();
    WSymbol wsymbol = new WSymbol((IWordDocument) this.DocumentEx);
    this.AddItem((ParagraphItem) wsymbol, (IWParagraph) this.CurrentParagraph);
    wsymbol.CharacterCode = symbolDescriptor.CharCode;
    wsymbol.CharCodeExt = symbolDescriptor.CharCodeExt;
    wsymbol.FontName = reader.StyleSheet.FontNamesList[(int) symbolDescriptor.FontCode];
    this.ReadCharacterFormat(reader, wsymbol.CharacterFormat);
    this.CheckTrackChanges((ParagraphItem) wsymbol, reader);
    for (int index = 1; index < reader.TextChunk.Length; ++index)
      this.AddItem((ParagraphItem) wsymbol.Clone(), (IWParagraph) this.CurrentParagraph);
  }

  private void ReadCurrentPageNumber(WordReaderBase reader)
  {
    if (!reader.CHPXSprms.Contain(2133))
      return;
    this.CurrentParagraph.AppendField("", FieldType.FieldPage);
  }

  private void ReadTab(WordReaderBase reader)
  {
  }

  private void ReadTableCell(WordReaderBase reader)
  {
    this.m_cellFinished = true;
    WTableCell lastCell = this.m_currTable.LastCell;
    if (lastCell.Items.Count == 0)
      this.m_currParagraph = (ITextBodyItem) (lastCell.AddParagraph() as WParagraph);
    if (this.m_currParagraph != null || this.m_prevChunkType == WordChunkType.ParagraphEnd || this.m_prevChunkType == WordChunkType.TableRow || this.m_prevChunkType == WordChunkType.TableCell)
    {
      this.ReadListFormat(reader, this.CurrentParagraph.ListFormat);
      this.ReadParagraphFormat(reader, (IWParagraph) this.CurrentParagraph);
    }
    this.ReadCharacterFormat(reader, lastCell.CharacterFormat);
    this.CurrentParagraph.BreakCharacterFormat.ImportContainer((FormatBase) lastCell.CharacterFormat);
    this.CheckTrackChanges(this.CurrentParagraph, reader);
    this.m_currParagraph = (ITextBodyItem) null;
  }

  private void ReadTableRow(WordReaderBase reader)
  {
    this.m_rowFinished = true;
    if (this.m_prevChunkType != WordChunkType.TableCell && this.m_currTable != null)
    {
      foreach (Entity childEntity1 in (CollectionImpl) this.m_currTable.LastCell.ChildEntities)
      {
        if (childEntity1 is WParagraph)
        {
          for (int index = 0; index < (childEntity1 as WParagraph).ChildEntities.Count; ++index)
          {
            Entity childEntity2 = (childEntity1 as WParagraph).ChildEntities[index];
            switch (childEntity2)
            {
              case BookmarkStart _:
              case BookmarkEnd _:
                (this.m_currTable.LastCell.PreviousSibling as WTableCell).LastParagraph.ChildEntities.Add((IEntity) childEntity2);
                --index;
                break;
            }
          }
        }
      }
      this.m_currTable.LastRow.Cells.RemoveAt(this.m_currTable.LastRow.Cells.Count - 1);
    }
    if (this.m_currTable != null)
    {
      this.ReadTableRowFormat(reader, this.m_currTable);
      if (this.m_currTable.Rows.Count > 1 && this.IsSplitTableRows(reader, this.m_currTable.Rows[this.m_currTable.Rows.Count - 2].RowFormat.m_unParsedSprms, reader.PAPXSprms))
      {
        WTextBody textBody = this.m_textBody;
        if (this.m_textBody == this.m_currTable.LastCell)
        {
          WTextBody wtextBody = reader.PAPXSprms.GetInt(26185, 1) <= 1 ? this.m_nestedTextBodies.Peek() : (WTextBody) this.m_tablesNested.Peek().LastCell;
          WTable wtable = new WTable((IWordDocument) this.m_currTable.Document);
          wtable.Rows.Add(this.m_currTable.LastRow);
          if (this.m_currTable.Owner == null)
          {
            wtextBody.Items.Add((IEntity) this.m_currTable);
            if (reader.PAPXSprms[13837] == null)
            {
              WParagraph wparagraph = new WParagraph((IWordDocument) this.m_currTable.Document);
              wtextBody.Items.Add((IEntity) wparagraph);
              wparagraph.BreakCharacterFormat.Hidden = true;
            }
          }
          this.UpdateTableGridAfterValue(this.m_currTable, (IWordReaderBase) reader);
          this.m_currTable = wtable;
        }
      }
      if (this.m_currTable.LastRow.Cells.Count < 1)
        this.m_currTable.Rows.Remove(this.m_currTable.LastRow);
      else
        this.m_currTable.LastRow.HasTblPrEx = true;
    }
    else
    {
      WSymbol wsymbol = this.CurrentParagraph.AppendSymbol((byte) 7);
      this.ReadCharacterFormat(reader, wsymbol.CharacterFormat);
      this.CheckTrackChanges((ParagraphItem) wsymbol, reader);
      wsymbol.FontName = reader.CHPXSprms.Contain(19038) ? reader.GetFontName(19038) : reader.GetFontName(19023);
      this.m_rowFinished = false;
    }
    SinglePropertyModifierArray propertyModifierArray1 = (SinglePropertyModifierArray) null;
    if (this.m_currTable == null || this.m_currTable.LastRow == null)
      return;
    SinglePropertyModifierRecord propertyModifier = reader.PAPX.PropertyModifiers[25707];
    if (propertyModifier != null && reader.m_streamsManager.DataStream != null)
    {
      if (reader.m_streamsManager.DataStream.Length > (long) propertyModifier.UIntValue)
        reader.m_streamsManager.DataStream.Position = (long) propertyModifier.UIntValue;
      if (reader.m_streamsManager.DataStream.Position + 2L < reader.m_streamsManager.DataStream.Length)
      {
        int iCount = (int) reader.m_streamsManager.DataReader.ReadUInt16();
        if (iCount <= 16290)
        {
          SinglePropertyModifierArray propertyModifierArray2 = new SinglePropertyModifierArray();
          if (reader.m_streamsManager.DataStream.Position + (long) iCount <= reader.m_streamsManager.DataStream.Length)
            propertyModifierArray2.Parse((Stream) reader.m_streamsManager.DataStream, iCount);
          propertyModifierArray1 = propertyModifierArray2;
        }
      }
    }
    if (!reader.PAPX.PropertyModifiers.Contain(54836) && propertyModifierArray1 != null && propertyModifierArray1[54836] != null)
    {
      Spacings source = new Spacings(propertyModifierArray1[54836]);
      Paddings destination = new Paddings();
      if (source != null && !source.IsEmpty)
      {
        TablePropertiesConverter.ExportPaddings(source, destination);
        this.m_currTable.LastRow.RowFormat.SetPropertyValue(3, (object) destination);
      }
      else
      {
        TablePropertiesConverter.ExportDefaultPaddings(destination);
        this.m_currTable.LastRow.RowFormat.SetPropertyValue(3, (object) destination);
      }
    }
    propertyModifierArray1?.Clear();
  }

  private static void ExportDefaultPaddings(Paddings destination)
  {
    destination.Left = 0.0f;
    destination.Right = 0.0f;
    destination.Top = 0.0f;
    destination.Bottom = 0.0f;
  }

  private bool IsSplitTableRows(
    WordReaderBase reader,
    SinglePropertyModifierArray previousRowSprms,
    SinglePropertyModifierArray currentRowSprms)
  {
    bool flag = false;
    SinglePropertyModifierArray propertyModifierArray1 = previousRowSprms?.Clone();
    int[] numArray = new int[3]{ 29801, 22074, 22116 };
    if (propertyModifierArray1 != null && currentRowSprms != null)
    {
      foreach (int option in numArray)
      {
        SinglePropertyModifierRecord newSprm1 = propertyModifierArray1.GetNewSprm(option, 13928);
        SinglePropertyModifierRecord newSprm2 = currentRowSprms.GetNewSprm(option, 13928);
        if (newSprm1 != null && newSprm2 != null)
        {
          if (!this.CompareArray(newSprm1.ByteArray, newSprm2.ByteArray))
          {
            flag = true;
            break;
          }
        }
        else if (newSprm1 != null && newSprm2 == null || newSprm1 == null && newSprm2 != null)
        {
          flag = true;
          break;
        }
      }
    }
    if (!flag)
      flag = this.CompareBidiAndPositioning();
    if (!flag)
    {
      SinglePropertyModifierRecord propertyModifierRecord1 = (SinglePropertyModifierRecord) null;
      SinglePropertyModifierRecord propertyModifierRecord2 = (SinglePropertyModifierRecord) null;
      SinglePropertyModifierRecord previousRowSprm = previousRowSprms?[25707];
      if (previousRowSprm != null && reader.m_streamsManager.DataStream != null)
      {
        if (reader.m_streamsManager.DataStream.Length > (long) previousRowSprm.UIntValue)
          reader.m_streamsManager.DataStream.Position = (long) previousRowSprm.UIntValue;
        if (reader.m_streamsManager.DataStream.Position + 2L < reader.m_streamsManager.DataStream.Length)
        {
          int iCount = (int) reader.m_streamsManager.DataReader.ReadUInt16();
          if (iCount <= 16290)
          {
            SinglePropertyModifierArray propertyModifierArray2 = new SinglePropertyModifierArray();
            if (reader.m_streamsManager.DataStream.Position + (long) iCount < reader.m_streamsManager.DataStream.Length)
              propertyModifierArray2.Parse((Stream) reader.m_streamsManager.DataStream, iCount);
            propertyModifierRecord1 = propertyModifierArray2[22074];
          }
        }
      }
      SinglePropertyModifierRecord currentRowSprm = currentRowSprms[25707];
      if (currentRowSprm != null && reader.m_streamsManager.DataStream != null)
      {
        if (reader.m_streamsManager.DataStream.Length > (long) currentRowSprm.UIntValue)
          reader.m_streamsManager.DataStream.Position = (long) currentRowSprm.UIntValue;
        if (reader.m_streamsManager.DataStream.Position + 2L < reader.m_streamsManager.DataStream.Length)
        {
          int iCount = (int) reader.m_streamsManager.DataReader.ReadUInt16();
          if (iCount <= 16290)
          {
            SinglePropertyModifierArray propertyModifierArray3 = new SinglePropertyModifierArray();
            if (reader.m_streamsManager.DataStream.Position + (long) iCount < reader.m_streamsManager.DataStream.Length)
              propertyModifierArray3.Parse((Stream) reader.m_streamsManager.DataStream, iCount);
            propertyModifierRecord2 = propertyModifierArray3[22074];
          }
        }
      }
      if (propertyModifierRecord1 != null && propertyModifierRecord2 != null)
      {
        if (!this.CompareArray(propertyModifierRecord1.ByteArray, propertyModifierRecord2.ByteArray))
          flag = true;
        else if (propertyModifierRecord1 != null && propertyModifierRecord2 == null || propertyModifierRecord1 == null && propertyModifierRecord2 != null)
          flag = true;
      }
    }
    return flag;
  }

  private bool CompareBidiAndPositioning()
  {
    RowFormat rowFormat1 = this.m_currTable.Rows[this.m_currTable.Rows.Count - 2].RowFormat;
    RowFormat rowFormat2 = this.m_currTable.LastRow.RowFormat;
    if (rowFormat1.Bidi != rowFormat2.Bidi)
      return true;
    RowFormat.TablePositioning positioning1 = rowFormat1.Positioning;
    RowFormat.TablePositioning positioning2 = rowFormat2.Positioning;
    return positioning1.AllowOverlap != positioning2.AllowOverlap || (double) positioning1.DistanceFromBottom != (double) positioning2.DistanceFromBottom || (double) positioning1.DistanceFromLeft != (double) positioning2.DistanceFromLeft || (double) positioning1.DistanceFromRight != (double) positioning2.DistanceFromRight || (double) positioning1.DistanceFromTop != (double) positioning2.DistanceFromTop || (double) positioning1.HorizPosition != (double) positioning2.HorizPosition || positioning1.HorizPositionAbs != positioning2.HorizPositionAbs || positioning1.HorizRelationTo != positioning2.HorizRelationTo || (double) positioning1.VertPosition != (double) positioning2.VertPosition || positioning1.VertPositionAbs != positioning2.VertPositionAbs || positioning1.VertRelationTo != positioning2.VertRelationTo;
  }

  private bool CompareArray(byte[] buffer1, byte[] buffer2)
  {
    bool flag = true;
    for (int index = 0; index < buffer1.Length; ++index)
    {
      if ((int) buffer1[index] != (int) buffer2[index])
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  private void ReadTable(WordReaderBase reader) => this.ReadTableRow(reader);

  protected virtual void ReadAnnotation(WordReaderBase reader)
  {
  }

  protected virtual void ReadFootnote(WordReaderBase reader)
  {
    throw new Exception("The method or operation is not implemented.");
  }

  private void ReadBreak(WordReaderBase reader, BreakType breakType)
  {
    Break @break;
    if (breakType == BreakType.LineBreak)
    {
      @break = new Break((IWordDocument) this.DocumentEx, BreakType.LineBreak);
      @break.TextRange.Text = reader.TextChunk;
    }
    else
      @break = new Break((IWordDocument) this.DocumentEx, breakType);
    this.AddItem((ParagraphItem) @break, (IWParagraph) this.CurrentParagraph);
    this.ReadCharacterFormat(reader, @break.CharacterFormat);
    this.CheckTrackChanges((ParagraphItem) @break, reader);
  }

  protected virtual void ReadDocumentEnd(WordReaderBase reader)
  {
  }

  protected virtual void ReadShape(WordReaderBase reader, HeaderFooter headerFooter)
  {
    if (reader.ReadWatermark(this.DocumentEx, (WTextBody) headerFooter))
    {
      if (headerFooter == null)
        return;
      headerFooter.WriteWatermark = true;
    }
    else
    {
      FileShapeAddress fspa = reader.GetFSPA();
      if (fspa == null)
        return;
      fspa.IsHeaderShape = reader is WordHeaderFooterReader;
      MsofbtSpContainer shapeContainer = (MsofbtSpContainer) null;
      if (this.DocumentEx.Escher.Containers.ContainsKey(fspa.Spid))
        shapeContainer = this.DocumentEx.Escher.Containers[fspa.Spid] as MsofbtSpContainer;
      if (shapeContainer != null && (shapeContainer.Shape.ShapeType == EscherShapeType.msosptTextBox || shapeContainer.Shape.ShapeType == EscherShapeType.msosptRectangle && shapeContainer.FindContainerByMsofbt(MSOFBT.msofbtClientTextbox) != null))
        this.ReadTextBox(reader, fspa);
      else if (shapeContainer != null && shapeContainer.Shape.ShapeType == EscherShapeType.msosptPictureFrame)
      {
        Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ShapeBase drawingObject = reader.GetDrawingObject();
        if (!(drawingObject is PictureShape))
          return;
        this.ReadPictureShape(reader, drawingObject, shapeContainer);
      }
      else
        this.ReadAutoShape(reader);
    }
  }

  protected virtual bool ReadWatermark(WordReaderBase reader) => false;

  private bool IsShapeFieldResult(IEntity endItem)
  {
    if (this.CurrentField == null)
      return false;
    IEntity nextSibling = this.CurrentField.NextSibling;
    string empty = string.Empty;
    for (; nextSibling != null && nextSibling != endItem; nextSibling = nextSibling.NextSibling)
    {
      if (nextSibling is WTextRange)
      {
        empty += (nextSibling as WTextRange).Text;
        if (FieldTypeDefiner.GetFieldType(empty) == FieldType.FieldShape)
          return true;
      }
    }
    return false;
  }

  protected virtual void ReadTextBox(WordReaderBase reader, FileShapeAddress fspa)
  {
  }

  private void ReadPictureShape(
    WordReaderBase reader,
    Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ShapeBase shape,
    MsofbtSpContainer shapeContainer)
  {
    PictureShape pictureShape = shape as PictureShape;
    if (pictureShape.ImageRecord == null)
      return;
    WPicture picture = new WPicture((IWordDocument) this.DocumentEx);
    this.CurrentParagraph.LoadPicture(picture, pictureShape.ImageRecord);
    this.AddItem((ParagraphItem) picture, (IWParagraph) this.CurrentParagraph);
    BaseProps shapeProps = pictureShape.ShapeProps;
    float height = (float) shapeProps.Height / 20f;
    float width = (float) shapeProps.Width / 20f;
    float num1 = (float) shapeProps.YaTop / 20f;
    float num2 = (float) shapeProps.XaLeft / 20f;
    SizeF sizeF = new SizeF(width, height);
    picture.Size = sizeF;
    picture.HeightScale = 100f;
    picture.WidthScale = 100f;
    picture.VerticalOrigin = pictureShape.ShapeProps.RelVrtPos;
    picture.HorizontalOrigin = pictureShape.ShapeProps.RelHrzPos;
    picture.VerticalPosition = num1;
    picture.HorizontalPosition = num2;
    if (picture.PreviousSibling is WFieldMark && (picture.PreviousSibling as WFieldMark).Type == FieldMarkType.FieldSeparator && this.IsShapeFieldResult(picture.PreviousSibling))
    {
      picture.TextWrappingStyle = TextWrappingStyle.Inline;
      shapeContainer.Bse.IsInlineBlip = true;
      shapeContainer.Bse.IsPictureInShapeField = true;
    }
    else
      picture.TextWrappingStyle = pictureShape.ShapeProps.TextWrappingStyle;
    picture.TextWrappingType = pictureShape.ShapeProps.TextWrappingType;
    picture.IsBelowText = pictureShape.ShapeProps.IsBelowText;
    picture.HorizontalAlignment = pictureShape.ShapeProps.HorizontalAlignment;
    picture.VerticalAlignment = pictureShape.ShapeProps.VerticalAlignment;
    picture.ShapeId = pictureShape.ShapeProps.Spid;
    picture.IsHeaderPicture = pictureShape.ShapeProps.IsHeaderShape;
    picture.AlternativeText = pictureShape.PictureProps.AlternativeText;
    picture.Name = pictureShape.PictureProps.Name;
    if (shapeContainer.ShapePosition != null)
    {
      picture.LayoutInCell = shapeContainer.ShapePosition.AllowInTableCell;
      picture.Visible = shapeContainer.ShapeOptions.Visible;
    }
    if (shapeContainer.ShapeOptions.Properties.ContainsKey(263))
      picture.ChromaKeyColor = WordColor.ConvertRGBToColor(shapeContainer.ShapeOptions.GetPropertyValue(263));
    if (shapeContainer.ShapeOptions != null)
    {
      this.UpdateImageCroppingPostion(picture, shapeContainer);
      picture.DistanceFromBottom = (float) Math.Round((double) shapeContainer.ShapeOptions.DistanceFromBottom / 12700.0, 2);
      picture.DistanceFromLeft = (float) Math.Round((double) shapeContainer.ShapeOptions.DistanceFromLeft / 12700.0, 2);
      picture.DistanceFromRight = (float) Math.Round((double) shapeContainer.ShapeOptions.DistanceFromRight / 12700.0, 2);
      picture.DistanceFromTop = (float) Math.Round((double) shapeContainer.ShapeOptions.DistanceFromTop / 12700.0, 2);
      picture.Rotation = (float) ((int) shapeContainer.ShapeOptions.Roation / 65536 /*0x010000*/);
    }
    if (!picture.IsBelowText && picture.TextWrappingStyle == TextWrappingStyle.Behind)
      picture.TextWrappingStyle = TextWrappingStyle.InFrontOfText;
    if ((picture.TextWrappingStyle == TextWrappingStyle.Through || picture.TextWrappingStyle == TextWrappingStyle.Tight) && shapeContainer.ShapeOptions != null && shapeContainer.ShapeOptions.Properties.Contains(899))
    {
      picture.WrapPolygon = new WrapPolygon();
      picture.WrapPolygon.Edited = false;
      for (int index = 0; index < shapeContainer.ShapeOptions.WrapPolygonVertices.Coords.Count; ++index)
        picture.WrapPolygon.Vertices.Add(shapeContainer.ShapeOptions.WrapPolygonVertices.Coords[index]);
    }
    this.CheckTextEmbed(shape, picture);
    picture.PictureShape.ShapeContainer = shapeContainer;
  }

  internal void UpdateImageCroppingPostion(WPicture picture, MsofbtSpContainer ShapeContainer)
  {
    int propertyValue1 = (int) ShapeContainer.ShapeOptions.GetPropertyValue(258);
    if (propertyValue1 != -1)
      picture.FillRectangle.LeftOffset = this.GetPictureCropValue(propertyValue1);
    int propertyValue2 = (int) ShapeContainer.ShapeOptions.GetPropertyValue(259);
    if (propertyValue2 != -1)
      picture.FillRectangle.RightOffset = this.GetPictureCropValue(propertyValue2);
    int propertyValue3 = (int) ShapeContainer.ShapeOptions.GetPropertyValue(256 /*0x0100*/);
    if (propertyValue3 != -1)
      picture.FillRectangle.TopOffset = this.GetPictureCropValue(propertyValue3);
    int propertyValue4 = (int) ShapeContainer.ShapeOptions.GetPropertyValue(257);
    if (propertyValue4 == -1)
      return;
    picture.FillRectangle.BottomOffset = this.GetPictureCropValue(propertyValue4);
  }

  private float GetPictureCropValue(int propValue)
  {
    return (float) Math.Round((double) ((float) propValue * 1.5259f) / 1000.0, 3);
  }

  protected virtual void CheckTextEmbed(Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.ShapeBase shape, WPicture picture)
  {
  }

  private void ReadAutoShape(WordReaderBase reader)
  {
    ShapeObject shapeObj = new ShapeObject((IWordDocument) this.DocumentEx);
    shapeObj.FSPA = reader.GetFSPA();
    this.ReadCharacterFormat(reader, shapeObj.CharacterFormat);
    if (reader is WordHeaderFooterReader)
      shapeObj.IsHeaderAutoShape = true;
    if (!this.DocumentEx.Escher.Containers.ContainsKey(shapeObj.FSPA.Spid))
      return;
    this.AddItem((ParagraphItem) shapeObj, (IWParagraph) this.CurrentParagraph);
    this.CheckTrackChanges((ParagraphItem) shapeObj, reader);
    BaseContainer container = this.DocumentEx.Escher.Containers[shapeObj.FSPA.Spid];
    if (container is MsofbtSpgrContainer)
      this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 15);
    else
      this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 25);
    this.ReadAutoShapeText(this.DocumentEx.Escher.Containers[shapeObj.FSPA.Spid], shapeObj);
    if (!(container is MsofbtSpContainer) || (container as MsofbtSpContainer).ShapePosition == null)
      return;
    shapeObj.AllowInCell = (container as MsofbtSpContainer).ShapePosition.AllowInTableCell;
  }

  private bool IsUnsupportedSpType(BaseContainer baseContainer)
  {
    bool flag = false;
    if (baseContainer is MsofbtSpContainer && (baseContainer as MsofbtSpContainer).Shape.ShapeType == EscherShapeType.msosptHostControl)
      flag = true;
    return flag;
  }

  private void ReadAutoShapeText(BaseContainer shapeContainer, ShapeObject shapeObj)
  {
    int index = 0;
    for (int count = shapeContainer.Children.Count; index < count; ++index)
    {
      BaseEscherRecord child = shapeContainer.Children[index] as BaseEscherRecord;
      switch (child)
      {
        case MsofbtSp _:
          MsofbtSp msofbtSp = child as MsofbtSp;
          if (msofbtSp.ShapeType != EscherShapeType.msosptPictureFrame)
          {
            this.ReadAutoShapeTextBox(msofbtSp.ShapeId, shapeObj);
            break;
          }
          break;
        case BaseContainer _:
          this.ReadAutoShapeText(child as BaseContainer, shapeObj);
          break;
      }
    }
  }

  protected virtual void ReadAutoShapeTextBox(int shapeId, ShapeObject shapeObj)
  {
  }

  internal bool StartsWithExt(string text, string value) => text.StartsWithExt(value);

  private void ReadImage(WordReaderBase reader)
  {
    if (this.CurrentField is WMergeField)
      FormFieldPropertiesConverter.ReadFormFieldProperties(this.CurrentField as WFormField, reader.GetFormField(this.CurrentField.FieldType));
    else if (this.CurrentField is WFormField && reader.CHPXSprms.GetBoolean(2054, false))
    {
      FormFieldPropertiesConverter.ReadFormFieldProperties(this.CurrentField as WFormField, reader.GetFormField(this.CurrentField.FieldType));
      (this.CurrentField as WFormField).HasFFData = true;
    }
    else
    {
      if (this.CurrentField != null && (this.CurrentField.FieldType == FieldType.FieldPageRef || this.CurrentField.FieldType == FieldType.FieldRef || this.CurrentField.FieldType == FieldType.FieldHyperlink || this.CurrentField.FieldType == FieldType.FieldNoteRef) && reader.CHPXSprms.GetBoolean(2054, false))
        return;
      WordImageReader imageReader = (WordImageReader) reader.GetImageReader(this.DocumentEx);
      ShapeObject shapeObject = (ShapeObject) new InlineShapeObject((IWordDocument) this.DocumentEx);
      (shapeObject as InlineShapeObject).ShapeContainer = imageReader.InlineShapeContainer;
      (shapeObject as InlineShapeObject).PictureDescriptor = imageReader.PictureDescriptor;
      if (imageReader.UnparsedData != null)
        (shapeObject as InlineShapeObject).UnparsedData = imageReader.UnparsedData;
      CharacterPropertiesConverter.SprmsToFormat((IWordReaderBase) reader, shapeObject.CharacterFormat);
      if (imageReader.ImageRecord == null || imageReader.ImageRecord.m_imageBytes == null)
      {
        SinglePropertyModifierRecord chpxSprm = reader.CHPXSprms[2058];
        if (chpxSprm != null && chpxSprm.BoolValue)
        {
          (shapeObject as InlineShapeObject).IsOLE = true;
          (shapeObject as InlineShapeObject).OLEContainerId = reader.GetPicLocation();
        }
        if (reader.CHPXSprms.GetBoolean(2108, false))
          return;
        this.AddItem((ParagraphItem) shapeObject, (IWParagraph) this.CurrentParagraph);
        this.CheckTrackChanges((ParagraphItem) shapeObject, reader);
        InlineShapeObject inlineShapeObject = shapeObject as InlineShapeObject;
        string text = inlineShapeObject.ShapeContainer == null || inlineShapeObject.ShapeContainer.Shape == null ? (string) null : inlineShapeObject.ShapeContainer.Shape.ShapeType.ToString().TrimStart();
        if (text != null && text != "msosptTextBox" && this.StartsWithExt(text, "msosptText"))
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 31 /*0x1F*/);
        else
          this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_notSupportedElementFlag, 10);
      }
      else
      {
        WPicture picture;
        if (reader.CHPXSprms.GetBoolean(2108, false) && imageReader.ImageRecord.m_imageBytes.Length > 0)
        {
          picture = new WPicture((IWordDocument) this.DocumentEx);
          if (imageReader.InlineShapeContainer.Bse.Blip.Header.Type == MSOFBT.msofbtBlipWMF)
            this.PlaceableMetafileHeader(imageReader);
          picture.LoadImage(imageReader.ImageRecord);
          this.ListPictures.Add(picture);
        }
        else
        {
          picture = new WPicture((IWordDocument) this.DocumentEx);
          if (imageReader.InlineShapeContainer.Bse.Blip.Header.Type == MSOFBT.msofbtBlipWMF)
            this.PlaceableMetafileHeader(imageReader);
          this.CurrentParagraph.LoadPicture(picture, imageReader.ImageRecord);
          this.AddItem((ParagraphItem) picture, (IWParagraph) this.CurrentParagraph);
          picture.IsShape = true;
          if (this.CurrentField != null && (this.CurrentField.FieldType == FieldType.FieldLink || this.CurrentField.FieldType == FieldType.FieldEmbed) && this.CurrentField.Owner is WOleObject)
            (this.CurrentField.Owner as WOleObject).SetOlePicture(picture);
        }
        float height = (float) imageReader.Height / 20f;
        float num1 = (float) imageReader.HeightScale / 10f;
        float width = (float) imageReader.Width / 20f;
        float num2 = (float) imageReader.WidthScale / 10f;
        CharacterPropertiesConverter.SprmsToFormat((IWordReaderBase) reader, picture.CharacterFormat);
        this.CheckTrackChanges((ParagraphItem) picture, reader);
        SizeF sizeF = new SizeF(width, height);
        picture.Size = sizeF;
        picture.SetHeightScaleValue(num1);
        picture.SetWidthScaleValue(num2);
        picture.PictureShape = shapeObject as InlineShapeObject;
        picture.AlternativeText = imageReader.AlternativeText;
        picture.Name = imageReader.Name;
        if (picture.PictureShape != null && picture.PictureShape.ShapeContainer != null && picture.PictureShape.ShapeContainer.ShapeOptions != null)
          picture.Rotation = (float) (picture.PictureShape.ShapeContainer.ShapeOptions.Roation / 65536U /*0x010000*/);
        if (picture.PictureShape != null && picture.PictureShape.ShapeContainer != null && picture.PictureShape.ShapeContainer.ShapePosition != null)
        {
          picture.LayoutInCell = picture.PictureShape.ShapeContainer.ShapePosition.AllowInTableCell;
          picture.Visible = picture.PictureShape.ShapeContainer.ShapePosition.Visible;
        }
        if (picture.PictureShape != null && picture.PictureShape.ShapeContainer != null && picture.PictureShape.ShapeContainer.ShapeOptions != null)
          this.UpdateImageCroppingPostion(picture, picture.PictureShape.ShapeContainer);
        if (picture.PictureShape == null || picture.PictureShape.ShapeContainer == null || picture.PictureShape.ShapeContainer.ShapeOptions == null || !picture.PictureShape.ShapeContainer.ShapeOptions.Properties.ContainsKey(263))
          return;
        picture.ChromaKeyColor = WordColor.ConvertRGBToColor(picture.PictureShape.ShapeContainer.ShapeOptions.GetPropertyValue(263));
      }
    }
  }

  private void PlaceableMetafileHeader(WordImageReader imageReader)
  {
    MsofbtMetaFile blip = imageReader.InlineShapeContainer.Bse.Blip as MsofbtMetaFile;
    float num1 = (float) blip.m_rectWidth / 12700f;
    double num2 = (double) blip.m_rectHeight / 12700.0;
    byte[] bytes1 = BitConverter.GetBytes((short) blip.m_rectLeft);
    byte[] bytes2 = BitConverter.GetBytes((short) blip.m_rectTop);
    byte[] bytes3 = BitConverter.GetBytes((short) blip.m_rectRight);
    byte[] bytes4 = BitConverter.GetBytes((short) blip.m_rectBottom);
    short Inch = (short) ((double) (short) blip.m_rectRight * 72.0 / (double) num1);
    byte[] bytes5 = BitConverter.GetBytes(Inch);
    byte[] bytes6 = BitConverter.GetBytes(this.CalculateCheckSum((short) 0, (short) blip.m_rectLeft, (short) blip.m_rectTop, (short) blip.m_rectRight, (short) blip.m_rectBottom, Inch, (short) 0));
    byte[] src = new byte[22]
    {
      (byte) 215,
      (byte) 205,
      (byte) 198,
      (byte) 154,
      (byte) 0,
      (byte) 0,
      bytes1[0],
      bytes1[1],
      bytes2[0],
      bytes2[1],
      bytes3[0],
      bytes3[1],
      bytes4[0],
      bytes4[1],
      bytes5[0],
      bytes5[1],
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      bytes6[0],
      bytes6[1]
    };
    byte[] imageBytes = imageReader.ImageRecord.ImageBytes;
    byte[] dst = new byte[imageReader.ImageRecord.ImageBytes.Length + 22];
    Buffer.BlockCopy((Array) src, 0, (Array) dst, 0, 22);
    Buffer.BlockCopy((Array) imageBytes, 0, (Array) dst, src.Length, imageBytes.Length);
    imageReader.ImageRecord.ImageBytes = dst;
  }

  private short CalculateCheckSum(
    short Handle,
    short left,
    short top,
    short right,
    short bottom,
    short Inch,
    short reserved)
  {
    return (short) (0 ^ 52695 ^ 39622 ^ (int) Handle ^ (int) left ^ (int) top ^ (int) right ^ (int) bottom ^ (int) Inch ^ (int) reserved & (int) ushort.MaxValue ^ (int) ((long) reserved & 4294901760L) >> 16 /*0x10*/);
  }

  private void AddItem(ParagraphItem item, IWParagraph para)
  {
    para.Items.Add((IEntity) item);
    switch (item.EntityType)
    {
      case EntityType.TextRange:
        this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_2, 12);
        break;
      case EntityType.Picture:
        this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_1, 31 /*0x1F*/);
        break;
      case EntityType.Field:
        this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_1, 18);
        if ((item as WField).FieldType != FieldType.FieldHyperlink)
          break;
        this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_1, 23);
        break;
      case EntityType.Shape:
        this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_2, 6);
        break;
      case EntityType.Break:
        this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_1, 6);
        break;
      case EntityType.Symbol:
        this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_2, 8);
        break;
      case EntityType.OleObject:
        this.DocumentEx.SetTriggerElement(ref this.DocumentEx.m_supportedElementFlag_1, 29);
        break;
    }
  }

  private void ReadFldBeginMark(WordReaderBase reader)
  {
    FieldDescriptor fld = reader.GetFld();
    this.m_currField = new WField((IWordDocument) this.DocumentEx);
    this.m_currField.FieldType = FieldType.FieldUnknown;
    if (fld != null)
    {
      if (Enum.IsDefined(typeof (FieldType), (object) fld.Type))
        this.m_currField = this.m_currField.CreateFieldByType(string.Empty, fld.Type);
      else if (fld != null)
        this.m_currField.SourceFieldType = (short) fld.Type;
    }
    if (reader.CHPX != null)
      CharacterPropertiesConverter.SprmsToFormat(reader.CHPXSprms, this.m_currField.CharacterFormat, reader.StyleSheet, reader.SttbfRMarkAuthorNames, true);
    this.InsertStartField(reader);
  }

  private void InsertFldSeparator(WordReaderBase reader)
  {
    WFieldMark wfieldMark = new WFieldMark((IWordDocument) this.DocumentEx, FieldMarkType.FieldSeparator);
    this.AddItem((ParagraphItem) wfieldMark, (IWParagraph) this.CurrentParagraph);
    CharacterPropertiesConverter.SprmsToFormat((IWordReaderBase) reader, wfieldMark.CharacterFormat);
    this.CheckTrackChanges((ParagraphItem) wfieldMark, reader);
    WField currentField = this.CurrentField;
    if (currentField == null)
      return;
    currentField.FieldSeparator = wfieldMark;
    if (currentField is WEmbedField)
    {
      (currentField as WEmbedField).StoragePicLocation = reader.GetPicLocation();
    }
    else
    {
      if (!(currentField is WControlField))
        return;
      int picLocation = reader.GetPicLocation();
      WControlField wcontrolField = currentField as WControlField;
      if (picLocation <= 0)
        return;
      wcontrolField.StoragePicLocation = picLocation;
      if (reader.m_streamsManager.ObjectPoolStream == null)
        return;
      wcontrolField.OleObject.ParseObjectPool((Stream) reader.m_streamsManager.ObjectPoolStream, wcontrolField.StoragePicLocation.ToString(), this.DocumentEx.OleObjectCollection);
    }
  }

  private void InsertFldEndMark(WordReaderBase reader)
  {
    if (this.m_fieldStack.Count <= 0)
      return;
    WField currentField = this.CurrentField;
    if (currentField.FieldType == FieldType.FieldFillIn && this.CurrentParagraph.Items.Count > 0 && this.CurrentField.FieldSeparator == null)
      this.InsertFldSeparator(reader);
    WFieldMark endMark = new WFieldMark((IWordDocument) this.DocumentEx, FieldMarkType.FieldEnd);
    this.AddItem((ParagraphItem) endMark, (IWParagraph) this.CurrentParagraph);
    currentField.FieldEnd = endMark;
    CharacterPropertiesConverter.SprmsToFormat((IWordReaderBase) reader, endMark.CharacterFormat);
    this.CheckTrackChanges((ParagraphItem) endMark, reader);
    this.DocumentEx.UpdateFieldRevision(currentField);
    if (!currentField.IsFormField())
      this.UpdateFieldType(reader, currentField, endMark);
    if (this.m_fieldStack == null || this.m_fieldStack.Count <= 0)
      return;
    WField wfield = this.m_fieldStack.Pop();
    if (wfield.FieldType != FieldType.FieldTOC)
      wfield.UpdateFieldCode(wfield.GetFieldCodeForUnknownFieldType());
    if (wfield.FieldType != FieldType.FieldDate && wfield.FieldType != FieldType.FieldTime)
      return;
    wfield.Update();
  }

  private void UpdateFieldType(WordReaderBase reader, WField field, WFieldMark endMark)
  {
    if (field.FieldType == FieldType.FieldUnknown)
    {
      field.SetUnknownFieldType();
      if (field.FieldType == FieldType.FieldUnknown)
        return;
    }
    FieldType fieldType = field.FieldType;
    switch (fieldType)
    {
      case FieldType.FieldIf:
      case FieldType.FieldMergeField:
        field = field.ReplaceValidField();
        this.m_fieldStack.Pop();
        this.m_fieldStack.Push(field);
        break;
      case FieldType.FieldTOC:
        field.ReplaceAsTOCField();
        break;
      case FieldType.FieldLink:
      case FieldType.FieldEmbed:
        this.ReadOleObject(reader, fieldType);
        break;
      default:
        if (!field.IsFormField())
          break;
        goto case FieldType.FieldIf;
    }
  }

  private void InsertStartField(WordReaderBase reader)
  {
    this.m_fieldStack.Push(this.m_currField);
    this.AddItem((ParagraphItem) this.CurrentField, (IWParagraph) this.CurrentParagraph);
    CharacterPropertiesConverter.SprmsToFormat((IWordReaderBase) reader, this.m_currField.CharacterFormat);
    this.CheckTrackChanges((ParagraphItem) this.CurrentField, reader);
  }

  private void ReadOleObject(WordReaderBase reader, FieldType type)
  {
    WOleObject owner = new WOleObject(this.DocumentEx);
    WField wfield = this.m_fieldStack.Pop();
    wfield.UpdateFieldCode(wfield.GetFieldCodeForUnknownFieldType());
    owner.OleStorageName = !(wfield is WEmbedField) ? reader.GetPicLocation().ToString() : (wfield as WEmbedField).StoragePicLocation.ToString();
    if (type == FieldType.FieldEmbed)
      owner.SetLinkType(OleLinkType.Embed);
    else
      owner.SetLinkType(OleLinkType.Link);
    if ((!this.DocumentEx.Settings.PreserveOleImageAsImage || owner.LinkType != OleLinkType.Embed || !(wfield.FieldValue == "Paint.Picture")) && reader.m_streamsManager.ObjectPoolStream != null && reader.m_streamsManager.ObjectPoolStream.Length != 0L)
      owner.ParseObjectPool((Stream) reader.m_streamsManager.ObjectPoolStream);
    if (wfield.FieldSeparator != null && wfield.FieldSeparator.NextSibling is WPicture)
      owner.SetOlePicture(wfield.FieldSeparator.NextSibling as WPicture);
    if (this.DocumentEx.Settings.PreserveOleImageAsImage && owner.LinkType == OleLinkType.Embed && wfield.FieldValue == "Paint.Picture")
    {
      ParagraphItem paragraphItem = (ParagraphItem) (owner.OlePicture.Clone() as WPicture);
      int index = wfield.Index;
      if (this.CurrentParagraph.Items.InnerList[index] is WField)
        (this.CurrentParagraph.Items.InnerList[index] as WField).RemoveSelf();
      this.CurrentParagraph.Items.Insert(index, (IEntity) paragraphItem);
    }
    else
    {
      int index = wfield.Index;
      this.DocumentEx.IsSkipFieldDetach = true;
      this.CurrentParagraph.Items.RemoveAt(index);
      this.DocumentEx.IsSkipFieldDetach = false;
      this.CurrentParagraph.Items.Insert(index, (IEntity) owner);
      wfield.SetOwner((OwnerHolder) owner);
      owner.Field = wfield;
    }
  }

  protected void ReadListFormat(WordReaderBase reader, WListFormat listFormat)
  {
    if (!reader.HasList())
      return;
    ListInfo listInfo = reader.ListInfo;
    ListPropertiesConverter.Export(listFormat, reader.PAPXSprms, reader);
  }

  protected void AddUsedFonts(WCharacterFormat charFormat)
  {
    string fontName = charFormat.GetFontName((short) 68);
    FontStyle fontStyle = FontStyle.Regular;
    if (charFormat.HasValue(4) && charFormat.Bold)
      fontStyle |= FontStyle.Bold;
    if (charFormat.HasValue(5) && charFormat.Italic)
      fontStyle |= FontStyle.Italic;
    if (charFormat.HasValue(7) && charFormat.UnderlineStyle != UnderlineStyle.None)
      fontStyle |= FontStyle.Underline;
    if (charFormat.HasValue(6) && charFormat.Strikeout)
      fontStyle |= FontStyle.Strikeout;
    Font font;
    try
    {
      font = charFormat.Document.FontSettings.GetFont(fontName, 11f, fontStyle);
    }
    catch (Exception ex)
    {
      FontFamily fontFamily = new FontFamily(fontName);
      if (fontFamily.IsStyleAvailable(FontStyle.Bold))
        fontStyle |= FontStyle.Bold;
      if (fontFamily.IsStyleAvailable(FontStyle.Italic))
        fontStyle |= FontStyle.Italic;
      if (fontFamily.IsStyleAvailable(FontStyle.Underline))
        fontStyle |= FontStyle.Underline;
      if (fontFamily.IsStyleAvailable(FontStyle.Strikeout))
        fontStyle |= FontStyle.Strikeout;
      font = charFormat.Document.FontSettings.GetFont(fontName, 11f, fontStyle);
    }
    if (this.DocumentEx.UsedFontNames.Contains(font))
      return;
    this.DocumentEx.UsedFontNames.Add(font);
  }

  protected void ReadCharacterFormat(WordReaderBase reader, WCharacterFormat charFormat)
  {
    CharacterPropertiesConverter.SprmsToFormat((IWordReaderBase) reader, charFormat);
    this.AddUsedFonts(charFormat);
  }

  protected void ReadParagraphFormat(WordReaderBase reader, IWParagraph paragraph)
  {
    ParagraphPropertiesConverter.SprmsToFormat(reader.PAPXSprms, paragraph.ParagraphFormat, reader.SttbfRMarkAuthorNames, reader.StyleSheet);
    this.UpdateParagraphStyle(paragraph, reader);
    if (reader.PAPXSprms == null)
      return;
    ParagraphPropertiesConverter.UpdateDirectParagraphFormatting(paragraph.ParagraphFormat, reader.PAPXSprms);
  }

  protected void UpdateParagraphStyle(IWParagraph paragraph, WordReaderBase reader)
  {
    int num = reader.CurrentStyleIndex;
    WordStyle styleByIndex = reader.StyleSheet.GetStyleByIndex(num);
    bool flag = false;
    SinglePropertyModifierRecord newSprm1 = reader.PAPX.PropertyModifiers.GetNewSprm(50689, 9828);
    SinglePropertyModifierRecord newSprm2 = reader.PAPX.PropertyModifiers.GetNewSprm(17920, 9828);
    if (newSprm1 != null && newSprm1.ByteArray[0] == (byte) 0 && newSprm2 != null)
    {
      ushort ushortValue = newSprm2.UshortValue;
      ushort uint16_1 = BitConverter.ToUInt16(newSprm1.ByteArray, 1);
      ushort uint16_2 = BitConverter.ToUInt16(newSprm1.ByteArray, 3);
      if ((int) uint16_2 >= (int) uint16_1 && (int) ushortValue >= (int) uint16_1 && (int) ushortValue <= (int) uint16_2)
      {
        int startIndex = 5;
        ushort length = (ushort) ((int) uint16_2 - (int) uint16_1 + 1);
        ushort[] numArray = new ushort[(int) length];
        for (int index = 0; index < (int) length && startIndex + 1 < newSprm1.ByteArray.Length; ++index)
        {
          numArray[index] = BitConverter.ToUInt16(newSprm1.ByteArray, startIndex);
          startIndex += 2;
        }
        ushort index1 = numArray[(int) ushortValue - (int) uint16_1];
        if (reader.StyleSheet.GetStyleByIndex((int) index1) != null)
        {
          styleByIndex = reader.StyleSheet.GetStyleByIndex((int) index1);
          num = (int) index1;
          flag = true;
        }
      }
    }
    if (!flag && newSprm2 != null && reader.StyleSheet.GetStyleByIndex((int) newSprm2.ShortValue) != null)
    {
      styleByIndex = reader.StyleSheet.GetStyleByIndex((int) newSprm2.ShortValue);
      num = (int) newSprm2.ShortValue;
    }
    IWParagraphStyle style = !styleByIndex.IsCharacterStyle ? this.DocumentEx.Styles.FindByName(!reader.StyleSheet.StyleNames.ContainsKey(num) ? styleByIndex.Name : reader.StyleSheet.StyleNames[num], StyleType.ParagraphStyle) as IWParagraphStyle : this.DocumentEx.Styles.FindByName("Normal") as IWParagraphStyle;
    if (style != null)
    {
      (paragraph as WParagraph).ApplyStyle(style, false);
    }
    else
    {
      if (paragraph.ParagraphFormat.m_unParsedSprms == null)
        paragraph.ParagraphFormat.m_unParsedSprms = new SinglePropertyModifierArray();
      paragraph.ParagraphFormat.m_unParsedSprms.SetShortValue(17920, (short) num);
    }
  }

  private void ReadTableRowFormat(WordReaderBase reader, WTable table)
  {
    TablePropertiesConverter.SprmsToFormat((IWordReaderBase) reader, table.LastRow.RowFormat);
    WTableRow lastRow = table.LastRow;
    this.ReadCharacterFormat(reader, lastRow.CharacterFormat);
    if (lastRow.RowFormat.HasKey(122))
    {
      Revision newRevision = this.DocumentEx.CreateNewRevision(RevisionType.Formatting, lastRow.RowFormat.FormatChangeAuthorName, lastRow.RowFormat.FormatChangeDateTime, (string) null);
      lastRow.RowFormat.Revisions.Add(newRevision);
      newRevision.Range.Items.Add((object) lastRow.RowFormat);
      this.DocumentEx.UpdateTableFormatRevision(lastRow);
    }
    if (lastRow.CharacterFormat.IsInsertRevision)
    {
      this.DocumentEx.TableRowRevision(RevisionType.Insertions, lastRow, reader);
      this.DocumentEx.UpdateTableRowRevision(lastRow);
    }
    if (!lastRow.CharacterFormat.IsDeleteRevision)
      return;
    this.DocumentEx.TableRowRevision(RevisionType.Deletions, lastRow, reader);
    this.DocumentEx.UpdateTableRowRevision(lastRow);
  }

  internal void CheckTrackChanges(ParagraphItem item, WordReaderBase reader)
  {
    WCharacterFormat charFormat = item.GetCharFormat();
    if (charFormat.HasKey(105))
      this.DocumentEx.CharacterFormatChange(charFormat, item, reader);
    if (charFormat.IsInsertRevision)
    {
      string authorName = this.DocumentEx.GetAuthorName(reader, true);
      DateTime dateTime = this.DocumentEx.GetDateTime(reader, true, charFormat);
      this.DocumentEx.ParagraphItemRevision(item, RevisionType.Insertions, authorName, dateTime, (string) null, true, (Revision) null, (Revision) null, (Stack<Revision>) null);
    }
    if (!charFormat.IsDeleteRevision)
      return;
    string authorName1 = this.DocumentEx.GetAuthorName(reader, false);
    DateTime dateTime1 = this.DocumentEx.GetDateTime(reader, false, charFormat);
    this.DocumentEx.ParagraphItemRevision(item, RevisionType.Deletions, authorName1, dateTime1, (string) null, true, (Revision) null, (Revision) null, (Stack<Revision>) null);
  }

  internal void CheckTrackChanges(WParagraph paragraph, WordReaderBase reader)
  {
    this.DocumentEx.CharacterFormatChange(paragraph.BreakCharacterFormat, (ParagraphItem) null, reader);
    this.DocumentEx.ParagraphFormatChange(paragraph.ParagraphFormat);
    this.DocumentEx.UpdateLastItemRevision((IWParagraph) paragraph, paragraph.Items);
  }

  protected virtual void ProcessCommText(WordReaderBase reader, WParagraph para)
  {
  }

  internal virtual void Close()
  {
    if (this.m_listPic != null)
    {
      this.m_listPic.Clear();
      this.m_listPic = (List<WPicture>) null;
    }
    this.m_textBody = (WTextBody) null;
    this.m_currParagraph = (ITextBodyItem) null;
    this.m_currTable = (WTable) null;
    if (this.m_tablesNested != null)
    {
      this.m_tablesNested.Clear();
      this.m_tablesNested = (Stack<WTable>) null;
    }
    if (this.m_nestedTextBodies != null)
    {
      this.m_nestedTextBodies.Clear();
      this.m_nestedTextBodies = (Stack<WTextBody>) null;
    }
    this.m_currField = (WField) null;
    if (this.m_fieldStack != null)
    {
      this.m_fieldStack.Clear();
      this.m_fieldStack = (Stack<WField>) null;
    }
    this.m_bookmarkInfo = (BookmarkInfo) null;
    this.DocumentEx = (WordDocument) null;
  }

  internal struct PrepareTableInfo
  {
    internal bool InTable;
    internal int Level;
    internal int PrevLevel;
    internal PrepareTableState State;

    internal PrepareTableInfo(WordReaderBase reader, int prevLevel)
    {
      this.InTable = reader.HasTableBody;
      this.PrevLevel = prevLevel;
      this.Level = this.InTable ? reader.PAPXSprms.GetInt(26185, 1) : 0;
      if (this.Level > this.PrevLevel)
        this.State = PrepareTableState.EnterTable;
      else if (this.Level < this.PrevLevel)
        this.State = PrepareTableState.LeaveTable;
      else
        this.State = PrepareTableState.NoChange;
    }
  }

  internal struct ComplexTable
  {
    internal WTable Table;
    internal WTable OneRowTable;

    internal ComplexTable(WTable oneRowTable)
    {
      this.Table = (WTable) null;
      this.OneRowTable = oneRowTable;
    }

    internal void SetNull()
    {
      this.Table = (WTable) null;
      this.OneRowTable = (WTable) null;
    }

    internal void AppendOneRowToTable()
    {
      this.Table = this.OneRowTable;
      this.OneRowTable = (WTable) null;
    }

    internal void CreateOneRowTable(WordDocument docEx)
    {
      this.OneRowTable = new WTable((IWordDocument) docEx);
      this.OneRowTable.ResetCells(1, 1);
      this.OneRowTable.TableFormat.Borders.BorderType = BorderStyle.None;
    }
  }
}
