// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.DocWriterAdapter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.ReaderWriter;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser;
using Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;
using Syncfusion.Layouting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal sealed class DocWriterAdapter
{
  private const string LINK_STRING = "OLE_LINK";
  private int m_listID = 1720085641;
  private int m_secNumber;
  private int m_tableNestingLevel;
  private string m_prevStyleName = string.Empty;
  private WParagraph m_lastParagarph;
  private WordWriter m_mainWriter;
  private IWordWriterBase m_currWriter;
  private WordDocument m_document;
  private IWSection m_currSection;
  private WTextBoxCollection m_txbxItems;
  private WTextBoxCollection m_hfTxbxItems;
  private List<WComment> m_commentCollection;
  private List<WFootnote> m_footnoteCollection;
  private List<WFootnote> m_endnoteCollection;
  private Dictionary<string, int> m_charStylesHash = new Dictionary<string, int>();
  private Dictionary<string, ListData> m_listData = new Dictionary<string, ListData>();
  private List<string> m_bookmarksAfterCell = new List<string>();
  private Stack<WField> m_fieldStack = new Stack<WField>();
  private Dictionary<string, DictionaryEntry> m_commOffsets;
  private List<WPicture> m_listPicture;
  private List<WOleObject> m_oleObjects;
  private List<Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.OLEObject> m_OLEObjects;

  private IWordWriterBase CurrentWriter
  {
    get => this.m_currWriter;
    set => this.m_currWriter = value;
  }

  private WField CurrentField
  {
    get => this.m_fieldStack.Count <= 0 ? (WField) null : this.m_fieldStack.Peek();
  }

  private List<WPicture> ListPicture
  {
    get
    {
      if (this.m_listPicture == null)
        this.m_listPicture = new List<WPicture>();
      return this.m_listPicture;
    }
  }

  private WParagraph LastParagraph => this.m_lastParagarph;

  private List<WComment> CommentCollection
  {
    get
    {
      if (this.m_commentCollection == null)
        this.m_commentCollection = new List<WComment>();
      return this.m_commentCollection;
    }
  }

  private List<WFootnote> FootnoteCollection
  {
    get
    {
      if (this.m_footnoteCollection == null)
        this.m_footnoteCollection = new List<WFootnote>();
      return this.m_footnoteCollection;
    }
  }

  private List<WFootnote> EndnoteCollection
  {
    get
    {
      if (this.m_endnoteCollection == null)
        this.m_endnoteCollection = new List<WFootnote>();
      return this.m_endnoteCollection;
    }
  }

  private WTextBoxCollection HFTextBoxCollection
  {
    get
    {
      if (this.m_hfTxbxItems == null)
        this.m_hfTxbxItems = new WTextBoxCollection((IWordDocument) this.m_document);
      return this.m_hfTxbxItems;
    }
  }

  private WTextBoxCollection TextBoxCollection
  {
    get
    {
      if (this.m_txbxItems == null)
        this.m_txbxItems = new WTextBoxCollection((IWordDocument) this.m_document);
      return this.m_txbxItems;
    }
  }

  private Dictionary<string, DictionaryEntry> CommentOffsets
  {
    get
    {
      if (this.m_commOffsets == null)
        this.m_commOffsets = new Dictionary<string, DictionaryEntry>();
      return this.m_commOffsets;
    }
  }

  internal DocWriterAdapter() => AdapterListIDHolder.Instance.ListStyleIDtoName.Clear();

  public void Write(WordWriter writer, WordDocument document)
  {
    this.m_document = document;
    this.Init(writer);
    this.m_mainWriter.WriteDocumentHeader();
    this.WriteBody();
    this.m_mainWriter.WriteDocumentEnd(this.m_document.Password, this.m_document.BuiltinDocumentProperties.Author, this.m_document.FIBVersion, this.m_document.OleObjectCollection);
    writer.Close();
    this.Close();
  }

  private void Init(WordWriter writer)
  {
    this.ResetLists();
    this.m_secNumber = 0;
    this.CurrentWriter = (IWordWriterBase) writer;
    this.m_mainWriter = writer;
    writer.StyleSheet.FontSubstitutionTable = this.m_document.FontSubstitutionTable;
    writer.m_docInfo.TablesData.FFNStringTable = this.m_document.FFNStringTable;
    this.m_lastParagarph = this.m_document.LastParagraph;
    this.m_mainWriter.CHPXStickProperties = false;
    this.m_mainWriter.PAPXStickProperties = false;
    this.m_mainWriter.SectionProperties.StickProperties = false;
    this.WriteStyleSheet((IWordWriter) writer);
    this.AddListPictures();
    SectionPropertiesConverter.Import(writer.SectionProperties, this.m_document.Sections[0]);
  }

  private void WriteBody()
  {
    this.m_secNumber = 0;
    this.WriteBackground();
    this.WriteDocumentEscher();
    this.WriteMainBody();
    this.WriteFootnotesBody();
    this.WriteHFBody();
    this.WriteAnnotationsBody();
    this.WriteEndnotesBody();
    this.WriteTextBoxes();
    this.WriteDocumentProperties();
  }

  private void WriteMainBody()
  {
    int index = 0;
    for (int count = this.m_document.Sections.Count; index < count; ++index)
    {
      WSection section = this.m_document.Sections[index];
      this.WriteSectionEnd((IWSection) section);
      if (section.Body.Items.Count > 0)
      {
        this.WriteParagraphs(section.Body.Items, false);
      }
      else
      {
        this.m_mainWriter.PAPX.PropertyModifiers.Clear();
        this.m_mainWriter.CurrentStyleIndex = 0;
      }
    }
  }

  private void WriteHFBody()
  {
    bool flag1 = false;
    bool flag2 = true;
    if (this.m_document.Watermark != null && this.m_document.Watermark.Type != WatermarkType.NoWatermark)
      this.WriteWatermarkParagraphs();
    int index1 = 0;
    for (int count = this.m_document.Sections.Count; index1 < count; ++index1)
    {
      if (!this.m_document.Sections[index1].HeadersFooters.IsEmpty)
      {
        flag2 = false;
        break;
      }
    }
    if (this.FootnoteCollection.Count <= 0 && this.EndnoteCollection.Count <= 0 && flag2)
      return;
    WordHeaderFooterWriter subdocumentWriter = this.m_mainWriter.GetSubdocumentWriter(WordSubdocument.HeaderFooter) as WordHeaderFooterWriter;
    subdocumentWriter.CHPXStickProperties = false;
    this.CurrentWriter = (IWordWriterBase) subdocumentWriter;
    subdocumentWriter.PAPXStickProperties = false;
    this.WriteSeparatorStories();
    int index2 = 0;
    for (int count = this.m_document.Sections.Count; index2 < count; ++index2)
    {
      WSection section = this.m_document.Sections[index2];
      if (flag1)
        subdocumentWriter.WriteSectionEnd();
      for (int index3 = 0; index3 < 6; ++index3)
      {
        if (section.HeadersFooters[index3].WriteWatermark)
          this.InsertWatermark((WTextBody) section.HeadersFooters[index3], (HeaderType) index3);
        this.WriteHeaderFooter(subdocumentWriter, section.HeadersFooters[index3].ChildEntities as BodyItemCollection, (HeaderType) index3);
      }
      flag1 = true;
    }
    subdocumentWriter?.WriteDocumentEnd();
  }

  private void WriteSeparatorStories()
  {
    this.WriteSeparatorStory(this.m_document.Footnotes.Separator);
    this.WriteSeparatorStory(this.m_document.Footnotes.ContinuationSeparator);
    this.WriteSeparatorStory(this.m_document.Footnotes.ContinuationNotice);
    this.WriteSeparatorStory(this.m_document.Endnotes.Separator);
    this.WriteSeparatorStory(this.m_document.Endnotes.ContinuationSeparator);
    this.WriteSeparatorStory(this.m_document.Endnotes.ContinuationNotice);
  }

  private void WriteSeparatorStory(WTextBody body)
  {
    this.WriteParagraphs((BodyItemCollection) body.ChildEntities, false);
    if (body.ChildEntities.Count >= 1 && !(body.ChildEntities[body.ChildEntities.Count - 1] is WTable))
      (this.CurrentWriter as WordHeaderFooterWriter).WriteMarker(WordChunkType.ParagraphEnd);
    (this.CurrentWriter as WordHeaderFooterWriter).ClosePrevSeparator();
  }

  private void InsertWatermark(WTextBody textBody, HeaderType headerType)
  {
    if (headerType != HeaderType.EvenHeader && headerType != HeaderType.OddHeader && headerType != HeaderType.FirstPageHeader)
      return;
    WParagraph wparagraph = this.GetFirstPara(textBody);
    if (wparagraph == null)
    {
      WSection ownerBase = textBody.OwnerBase as WSection;
      wparagraph = new WParagraph((IWordDocument) ownerBase.Document);
      ownerBase.HeadersFooters[(int) headerType].Items.Insert(0, (IEntity) wparagraph);
    }
    wparagraph.Items.Insert(0, (IEntity) (textBody as HeaderFooter).Watermark);
  }

  private WParagraph GetFirstPara(WTextBody textBody)
  {
    if (textBody != null && textBody.Items[0] is WParagraph)
      return textBody.Items[0] as WParagraph;
    if (textBody != null && textBody.Items[0] is WTable)
      return this.GetFirstTblPara(textBody.Items[0] as WTable);
    return textBody != null && textBody.Items[0] is BlockContentControl ? this.GetFirstPara((textBody.Items[0] as BlockContentControl).TextBody) : (WParagraph) null;
  }

  private WParagraph GetFirstTblPara(WTable table)
  {
    foreach (WTableRow row in (CollectionImpl) table.Rows)
    {
      foreach (WTextBody cell in (CollectionImpl) row.Cells)
      {
        foreach (TextBodyItem firstTblPara in (CollectionImpl) cell.Items)
        {
          if (firstTblPara is WParagraph)
            return firstTblPara as WParagraph;
        }
      }
    }
    return table.Rows.Count > 0 && table.Rows[0].Cells.Count > 0 ? table.Rows[0].Cells[0].AddParagraph() as WParagraph : (WParagraph) null;
  }

  private void WriteTextBoxBody(WTextBoxCollection txbxCollection, WordSubdocument txBxType)
  {
    if (txbxCollection == null || txbxCollection.Count <= 0)
      return;
    IWordSubdocumentWriter subdocumentWriter = this.m_mainWriter.GetSubdocumentWriter(txBxType);
    subdocumentWriter.CHPXStickProperties = false;
    subdocumentWriter.PAPXStickProperties = false;
    this.CurrentWriter = (IWordWriterBase) subdocumentWriter;
    int count = txbxCollection.Count;
    for (int index = 0; index < count; ++index)
      this.WriteTextBoxText(subdocumentWriter, txbxCollection[index] as WTextBox);
    subdocumentWriter.WriteDocumentEnd();
  }

  private void WriteTextBoxText(IWordSubdocumentWriter txbxWriter, WTextBox textBox)
  {
    this.WriteParagraphs((BodyItemCollection) textBox.TextBoxBody.ChildEntities, false);
    if (txbxWriter is WordHFTextBoxWriter)
      ((WordHFTextBoxWriter) txbxWriter).WriteHFTextBoxEnd(textBox.TextBoxSpid);
    else
      ((WordTextBoxWriter) txbxWriter).WriteTextBoxEnd(textBox.TextBoxSpid);
  }

  private void WriteFootnotesBody()
  {
    if (this.m_footnoteCollection == null || this.m_footnoteCollection.Count <= 0)
      return;
    int count = this.m_footnoteCollection.Count;
    IWordSubdocumentWriter subdocumentWriter = this.m_mainWriter.GetSubdocumentWriter(WordSubdocument.Footnote);
    subdocumentWriter.CHPXStickProperties = false;
    subdocumentWriter.PAPXStickProperties = false;
    this.CurrentWriter = (IWordWriterBase) subdocumentWriter;
    for (int index = 0; index < count; ++index)
    {
      WFootnote footnote = this.m_footnoteCollection[index];
      this.WriteSubDocumentText(subdocumentWriter, footnote.TextBody);
    }
    subdocumentWriter.WriteDocumentEnd();
  }

  private void WriteAnnotationsBody()
  {
    if (this.m_commentCollection == null || this.m_commentCollection.Count <= 0)
      return;
    int count = this.m_commentCollection.Count;
    IWordSubdocumentWriter subdocumentWriter = this.m_mainWriter.GetSubdocumentWriter(WordSubdocument.Annotation);
    subdocumentWriter.CHPXStickProperties = false;
    subdocumentWriter.PAPXStickProperties = false;
    this.CurrentWriter = (IWordWriterBase) subdocumentWriter;
    for (int index = 0; index < count; ++index)
    {
      WComment comment = this.m_commentCollection[index];
      this.WriteSubDocumentText(subdocumentWriter, comment.TextBody);
    }
    subdocumentWriter.WriteDocumentEnd();
  }

  private void WriteEndnotesBody()
  {
    if (this.m_endnoteCollection == null || this.m_endnoteCollection.Count <= 0)
      return;
    int count = this.m_endnoteCollection.Count;
    IWordSubdocumentWriter subdocumentWriter = this.m_mainWriter.GetSubdocumentWriter(WordSubdocument.Endnote);
    subdocumentWriter.CHPXStickProperties = false;
    subdocumentWriter.PAPXStickProperties = false;
    this.CurrentWriter = (IWordWriterBase) subdocumentWriter;
    for (int index = 0; index < count; ++index)
    {
      WFootnote endnote = this.m_endnoteCollection[index];
      this.WriteSubDocumentText(subdocumentWriter, endnote.TextBody);
    }
    subdocumentWriter.WriteDocumentEnd();
  }

  private void WriteTextBoxes()
  {
    this.WriteTextBoxBody(this.m_txbxItems, WordSubdocument.TextBox);
    this.WriteTextBoxBody(this.m_hfTxbxItems, WordSubdocument.HeaderTextBox);
  }

  private void WriteHeaderFooter(
    WordHeaderFooterWriter hfWriter,
    BodyItemCollection collection,
    HeaderType hType)
  {
    hfWriter.HeaderType = hType;
    this.WriteParagraphs(collection, false);
    if (collection.Count < 1 || collection[collection.Count - 1] is WTable)
      return;
    hfWriter.WriteMarker(WordChunkType.ParagraphEnd);
  }

  private void WriteSubDocumentText(IWordSubdocumentWriter writer, WTextBody body)
  {
    writer.WriteItemStart();
    this.WriteParagraphs((BodyItemCollection) body.ChildEntities, false);
    writer.WriteItemEnd();
  }

  private void WriteParagraphs(BodyItemCollection paragraphs, bool isTableBody)
  {
    IEntity entity = (IEntity) null;
    IWordWriterBase currentWriter = this.CurrentWriter;
    for (int index = 0; index < paragraphs.Count; ++index)
    {
      if (index != 0)
      {
        if (isTableBody)
          this.SetTableNestingLevel(currentWriter.PAPX.PropertyModifiers, this.m_tableNestingLevel);
        if (entity is IWParagraph)
        {
          WParagraph wparagraph = entity as WParagraph;
          if (index == paragraphs.Count - 1 && this.m_currSection.NextSibling == null && paragraphs[index] is WParagraph && ((WParagraph) paragraphs[index]).RemoveEmpty)
            break;
          if (!wparagraph.RemoveEmpty || !(wparagraph.Text == string.Empty))
            currentWriter.WriteMarker(WordChunkType.ParagraphEnd);
        }
      }
      if (isTableBody)
      {
        this.SetCellMark(currentWriter.PAPX.PropertyModifiers, true);
        this.SetTableNestingLevel(currentWriter.PAPX.PropertyModifiers, this.m_tableNestingLevel);
      }
      entity = (IEntity) paragraphs[index];
      switch (entity)
      {
        case IWParagraph _:
          WParagraph wparagraph1 = entity as WParagraph;
          if (!wparagraph1.RemoveEmpty || !(wparagraph1.Text == string.Empty))
          {
            if (wparagraph1.ParagraphFormat.PageBreakAfter && !this.IsPageBreakNeedToBeSkipped((Entity) wparagraph1))
              wparagraph1.InsertBreak(BreakType.PageBreak);
            if (wparagraph1.ParagraphFormat.ColumnBreakAfter && !this.IsPageBreakNeedToBeSkipped((Entity) wparagraph1))
              wparagraph1.InsertBreak(BreakType.ColumnBreak);
            this.WriteParagraph(entity as IWParagraph);
            break;
          }
          break;
        case IWTable _:
          this.WriteTable(entity as IWTable);
          break;
        case BlockContentControl _:
          bool flag = this.WriteSDTBlock(entity as BlockContentControl, isTableBody);
          if (entity.NextSibling != null && flag)
          {
            currentWriter.WriteMarker(WordChunkType.ParagraphEnd);
            break;
          }
          break;
      }
    }
  }

  private bool WriteSDTBlock(BlockContentControl sdtBlock, bool isTableBody)
  {
    BodyItemCollection items = sdtBlock.TextBody.Items;
    bool flag = items.LastItem is WParagraph;
    IEntity entity = (IEntity) null;
    IWordWriterBase currentWriter = this.CurrentWriter;
    for (int index = 0; index < items.Count; ++index)
    {
      if (index != 0)
      {
        if (isTableBody)
          this.SetTableNestingLevel(currentWriter.PAPX.PropertyModifiers, this.m_tableNestingLevel);
        if (entity is IWParagraph)
        {
          WParagraph wparagraph = entity as WParagraph;
          if (!wparagraph.RemoveEmpty || !(wparagraph.Text == string.Empty))
            currentWriter.WriteMarker(WordChunkType.ParagraphEnd);
        }
      }
      if (isTableBody)
      {
        this.SetCellMark(currentWriter.PAPX.PropertyModifiers, true);
        this.SetTableNestingLevel(currentWriter.PAPX.PropertyModifiers, this.m_tableNestingLevel);
      }
      entity = (IEntity) items[index];
      switch (entity)
      {
        case IWParagraph _:
          WParagraph wparagraph1 = entity as WParagraph;
          if (!wparagraph1.RemoveEmpty || !(wparagraph1.Text == string.Empty))
          {
            if (wparagraph1.ParagraphFormat.PageBreakAfter && !this.IsPageBreakNeedToBeSkipped((Entity) wparagraph1))
              wparagraph1.InsertBreak(BreakType.PageBreak);
            if (wparagraph1.ParagraphFormat.ColumnBreakAfter && !this.IsPageBreakNeedToBeSkipped((Entity) wparagraph1))
              wparagraph1.InsertBreak(BreakType.ColumnBreak);
            this.WriteParagraph(entity as IWParagraph);
            break;
          }
          break;
        case IWTable _:
          this.WriteTable(entity as IWTable);
          break;
        case BlockContentControl _:
          flag = this.WriteSDTBlock(entity as BlockContentControl, isTableBody);
          if (entity.NextSibling != null && flag)
          {
            currentWriter.WriteMarker(WordChunkType.ParagraphEnd);
            flag = false;
            break;
          }
          break;
      }
    }
    return flag;
  }

  private bool CheckCurItemInTable(bool isTableBody, BodyItemCollection paragraphs, int itemIndex)
  {
    bool flag = false;
    if (isTableBody)
    {
      IEntity paragraph = (IEntity) paragraphs[itemIndex];
      if (paragraph is WParagraph && (paragraph as WParagraph).Items.Count == 1)
      {
        WParagraph wparagraph = paragraph as WParagraph;
        if (wparagraph.Items[0] is BookmarkEnd && (wparagraph.Items[0] as BookmarkEnd).IsCellGroupBkmk)
        {
          this.m_bookmarksAfterCell.Add((wparagraph.Items[0] as BookmarkEnd).Name);
          flag = true;
        }
      }
    }
    return flag;
  }

  private void WriteParagraph(IWParagraph paragraph)
  {
    (paragraph as WParagraph).SplitTextRange();
    bool flag1 = paragraph == this.LastParagraph;
    this.WriteParagraphProperties(paragraph);
    bool flag2 = false;
    for (int index = 0; index < paragraph.Items.Count; ++index)
    {
      ParagraphItem paragraphItem = paragraph[index];
      bool flag3 = flag2 ? paragraphItem.GetCharFormat().BiDirectionalOverride != BiDirectionalOverride.None : this.SerializeDirectionalOverride(paragraphItem);
      if (!flag2 && flag3)
        flag2 = flag3;
      if (flag2 && !flag3)
      {
        this.WriteTextChunks('\u202C'.ToString(), true);
        flag2 = false;
      }
      this.WriteParaItem(paragraphItem, paragraph);
    }
    if (flag2)
      this.WriteTextChunks('\u202C'.ToString(), true);
    if (flag1 && this.ListPicture.Count > 0)
    {
      this.WriteBookmarkStart(new BookmarkStart((IWordDocument) this.m_document, "_PictureBullets"));
      this.WriteListPictures();
      this.WriteBookmarkEnd(new BookmarkEnd((IWordDocument) this.m_document, "_PictureBullets"));
    }
    if (!paragraph.IsInCell || !(paragraph as WParagraph).HasNoRenderableItem() || (paragraph.Owner as WTableCell).Count != 1 || ((paragraph.Owner as WTableCell).CharacterFormat.IsInsertRevision || (paragraph.Owner as WTableCell).CharacterFormat.IsDeleteRevision || !paragraph.BreakCharacterFormat.IsDeleteRevision) && !paragraph.BreakCharacterFormat.IsInsertRevision)
      return;
    if (paragraph.BreakCharacterFormat.IsDeleteRevision)
      (paragraph.Owner as WTableCell).CharacterFormat.IsDeleteRevision = true;
    if (paragraph.BreakCharacterFormat.IsInsertRevision)
      (paragraph.Owner as WTableCell).CharacterFormat.IsInsertRevision = true;
    (paragraph.Owner as WTableCell).CharacterFormat.AuthorName = paragraph.BreakCharacterFormat.AuthorName;
    (paragraph.Owner as WTableCell).CharacterFormat.RevDateTime = paragraph.BreakCharacterFormat.RevDateTime;
    foreach (Revision revision in paragraph.BreakCharacterFormat.Revisions)
      (paragraph.Owner as WTableCell).CharacterFormat.Revisions.Add(revision);
  }

  private bool SerializeDirectionalOverride(ParagraphItem item)
  {
    switch (item.GetCharFormat().BiDirectionalOverride)
    {
      case BiDirectionalOverride.None:
        return false;
      case BiDirectionalOverride.LTR:
        this.WriteTextChunks('\u202D'.ToString(), true);
        break;
      case BiDirectionalOverride.RTL:
        this.WriteTextChunks('\u202E'.ToString(), true);
        break;
    }
    return true;
  }

  private void WriteListPictures()
  {
    int index = 0;
    for (int count = this.ListPicture.Count; index < count; ++index)
    {
      WPicture picture = this.ListPicture[index];
      picture.CharacterFormat.Hidden = true;
      this.WriteImage((IWPicture) picture);
    }
  }

  private void WriteParaItem(ParagraphItem item, IWParagraph paragraph)
  {
    WTextRange text = item as WTextRange;
    XmlParagraphItem xmlParagraphItem = item as XmlParagraphItem;
    if (item is WFormField)
      this.WriteFormField(item as WFormField);
    else if (item is IWField)
      this.WriteBeginField(item as WField);
    else if (text != null)
    {
      this.WriteText(text);
    }
    else
    {
      switch (item)
      {
        case WPicture _ when (item as WPicture).ImageRecord != null:
          this.WriteImage((IWPicture) (item as WPicture));
          break;
        case BookmarkStart _:
          this.WriteBookmarkStart(item as BookmarkStart);
          break;
        case BookmarkEnd _:
          this.WriteBookmarkEnd(item as BookmarkEnd);
          break;
        case WSymbol _:
          this.WriteSymbol(item as WSymbol);
          break;
        case IWTextBox _:
          this.WriteTextBoxShape(item as WTextBox);
          break;
        case ShapeObject _:
          if (!item.IsNotFieldShape())
            break;
          this.WriteShapeObject(item as ShapeObject);
          break;
        case WFieldMark _:
          this.WriteFieldMarkAndText(item as WFieldMark);
          break;
        case WComment _:
          this.WriteComment(item as WComment);
          break;
        case WFootnote _:
          this.WriteFootnote(item as WFootnote);
          break;
        case Break _:
          this.WriteBreak(item as Break, (WParagraph) paragraph);
          break;
        case Watermark _:
          this.WriteWatermark(item as Watermark);
          break;
        case TableOfContent _:
          this.WriteTOC(item as TableOfContent);
          break;
        case WCommentMark _:
          this.WriteCommMark(item as WCommentMark);
          break;
        case WOleObject _:
          this.WriteOleObject(item as WOleObject);
          break;
        case WAbsoluteTab _:
          this.WriteAbsoluteTab(item as WAbsoluteTab);
          break;
        case InlineContentControl _:
          ParagraphItemCollection paragraphItems = (item as InlineContentControl).ParagraphItems;
          for (int index = 0; index < paragraphItems.Count; ++index)
            this.WriteParaItem(paragraphItems[index], paragraph);
          break;
        default:
          if (xmlParagraphItem == null || xmlParagraphItem.MathParaItemsCollection == null || xmlParagraphItem.MathParaItemsCollection.Count <= 0)
            break;
          IEnumerator enumerator = xmlParagraphItem.MathParaItemsCollection.GetEnumerator();
          try
          {
            while (enumerator.MoveNext())
              this.WriteParaItem((ParagraphItem) enumerator.Current, paragraph);
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

  private void WriteDocumentEscher()
  {
    EscherClass escher = this.m_document.Escher;
    if (escher == null)
      return;
    (this.m_currWriter as WordWriterBase).Escher = escher;
  }

  private void WriteAbsoluteTab(WAbsoluteTab absoluteTab)
  {
    WTextRange text = new WTextRange((IWordDocument) this.m_document);
    text.Text = absoluteTab.Text;
    text.ApplyCharacterFormat(absoluteTab.CharacterFormat);
    this.WriteText(text);
  }

  private void WriteWatermarkParagraphs()
  {
    if (this.m_document.Sections.Count == 0)
      this.m_document.AddSection();
    foreach (WSection section in (CollectionImpl) this.m_document.Sections)
    {
      for (int index = 0; index < 6; ++index)
      {
        if (section.HeadersFooters[index].ChildEntities.Count == 0 && section.HeadersFooters[index].WriteWatermark)
        {
          WParagraph wparagraph = new WParagraph((IWordDocument) this.m_document);
          section.HeadersFooters[index].ChildEntities.Add((IEntity) wparagraph);
        }
      }
    }
  }

  private void WriteText(WTextRange text)
  {
    this.UpdateCharStyleIndex(text.CharacterFormat.CharStyleName, false);
    CharacterPropertiesConverter.FormatToSprms(text.CharacterFormat, this.CurrentWriter.CHPX.PropertyModifiers, this.CurrentWriter.StyleSheet);
    if (text.Text != SpecialCharacters.FootnoteAsciiStr)
    {
      string text1 = text.Text;
      if (this.CurrentField is WTextFormField)
        text1 = FormFieldPropertiesConverter.FormatText(this.CurrentField.TextFormat, text1);
      else if (this.CurrentField != null && this.CurrentField.IsFieldWithoutSeparator && this.CurrentWriter.CHPX.PropertyModifiers[2050] == null)
        this.CurrentWriter.CHPX.PropertyModifiers.SetBoolValue(2050, true);
      this.WriteTextChunks(text1, text.SafeText);
    }
    else
      this.CurrentWriter.WriteMarker(WordChunkType.Footnote);
    this.CurrentWriter.CHPX.PropertyModifiers.Clear();
  }

  private void WriteTextChunks(string text, bool safeText)
  {
    if (safeText)
      this.CurrentWriter.WriteSafeChunk(text);
    else
      this.CurrentWriter.WriteChunk(text);
  }

  private void WriteBeginField(WField field)
  {
    if (field.FieldEnd == null && field.FieldType == FieldType.FieldUnknown)
      return;
    switch (field)
    {
      case WMergeField _:
        (field as WMergeField).UpdateFieldMarks();
        break;
      case WIfField _:
        (field as WIfField).UpdateExpString();
        break;
      case WSeqField _:
        (field as WSeqField).UpdateFieldMarks();
        break;
    }
    this.m_fieldStack.Push(field);
    this.UpdateCharStyleIndex(field.CharacterFormat.CharStyleName, false);
    CharacterPropertiesConverter.FormatToSprms(field.CharacterFormat, this.CurrentWriter.CHPX.PropertyModifiers, this.CurrentWriter.StyleSheet);
    string str = field.IsFormField() ? FieldTypeDefiner.GetFieldCode(field.FieldType) : string.Empty;
    if (field.IsFieldWithoutSeparator)
    {
      if (field.FieldSeparator != null && field.FieldEnd != null)
      {
        field.RemoveFieldSeparator(field.FieldEnd);
        field.FieldSeparator = (WFieldMark) null;
      }
      this.CurrentWriter.CHPXStickProperties = true;
      if (this.CurrentWriter.CHPX.PropertyModifiers[2050] == null)
        this.CurrentWriter.CHPX.PropertyModifiers.SetBoolValue(2050, true);
      this.CurrentWriter.WriteMarker(WordChunkType.FieldBeginMark);
      if (!string.IsNullOrEmpty(str))
        this.CurrentWriter.WriteSafeChunk(str);
      this.CurrentWriter.CHPXStickProperties = false;
    }
    else
      this.CurrentWriter.InsertStartField(str, field, false);
  }

  private void WriteOleObjectCharProps(WField field)
  {
    if (!(field.Owner is WOleObject))
      return;
    WOleObject owner = field.Owner as WOleObject;
    int result = 0;
    if (int.TryParse(owner.OleStorageName, out result))
      this.CurrentWriter.CHPX.PropertyModifiers.SetIntValue(27139, result);
    this.CurrentWriter.CHPX.PropertyModifiers.SetBoolValue(2058, true);
  }

  private void WriteFormField(WFormField field)
  {
    this.m_fieldStack.Push((WField) field);
    FormField formField = (FormField) null;
    if (field.HasFFData)
    {
      formField = new FormField(field.FieldType);
      FormFieldPropertiesConverter.WriteFormFieldProperties(formField, field);
    }
    CharacterPropertiesConverter.FormatToSprms(field.CharacterFormat, this.CurrentWriter.CHPX.PropertyModifiers, this.CurrentWriter.StyleSheet);
    this.CurrentWriter.InsertFormField(string.Empty, formField, field);
    if (formField == null || field.FormFieldType != FormFieldType.TextInput || (field as WTextFormField).TextRange.Text.Length != 0 || formField.DefaultTextInputValue.Length <= 0)
      return;
    (field as WTextFormField).TextRange.Text = formField.DefaultTextInputValue;
  }

  private void WriteTable(IWTable table)
  {
    this.CurrentWriter.PAPX.PropertyModifiers.Clear();
    if (table != null)
    {
      (table as WTable).UpdateGridSpan();
      int index1 = 0;
      for (int count1 = table.Rows.Count; index1 < count1; ++index1)
      {
        ++this.m_tableNestingLevel;
        WTableRow row = table.Rows[index1];
        int count2 = row.Cells.Count;
        for (int index2 = 0; index2 < count2; ++index2)
        {
          WTableCell cell = row.Cells[index2];
          this.CurrentWriter.CHPX.PropertyModifiers.Clear();
          this.SetTableNestingLevel(this.CurrentWriter.PAPX.PropertyModifiers, this.m_tableNestingLevel);
          this.WriteParagraphs((BodyItemCollection) cell.ChildEntities, true);
          if (cell.Items.Count > 0 && cell.Items.LastItem is WParagraph && this.m_document.ActualFormatType == FormatType.Html)
            cell.CharacterFormat.ImportContainer((FormatBase) cell.LastParagraph.BreakCharacterFormat);
          this.UpdateCharStyleIndex(cell.CharacterFormat.CharStyleName, false);
          CharacterPropertiesConverter.FormatToSprms(cell.CharacterFormat, this.CurrentWriter.CHPX.PropertyModifiers, this.CurrentWriter.StyleSheet);
          this.SetTableNestingLevel(this.CurrentWriter.PAPX.PropertyModifiers, this.m_tableNestingLevel);
          this.CurrentWriter.WriteCellMark(this.m_tableNestingLevel);
          int index3 = 0;
          for (int count3 = this.m_bookmarksAfterCell.Count; index3 < count3; ++index3)
            this.CurrentWriter.InsertBookmarkEnd(this.m_bookmarksAfterCell[index3]);
          this.m_bookmarksAfterCell.Clear();
        }
        if (this.m_tableNestingLevel == 1)
        {
          this.SetCellMark(this.CurrentWriter.PAPX.PropertyModifiers, true);
          this.CurrentWriter.PAPX.PropertyModifiers.SetBoolValue(9239, true);
          this.SetTableNestingLevel(this.CurrentWriter.PAPX.PropertyModifiers, this.m_tableNestingLevel);
        }
        else
        {
          this.SetCellMark(this.CurrentWriter.PAPX.PropertyModifiers, true);
          this.SetTableNestingLevel(this.CurrentWriter.PAPX.PropertyModifiers, this.m_tableNestingLevel);
          this.CurrentWriter.PAPX.PropertyModifiers.SetBoolValue(9291, true);
          this.CurrentWriter.PAPX.PropertyModifiers.SetBoolValue(9292, true);
        }
        this.WriteTableProps(this.CurrentWriter, row, table);
        this.CurrentWriter.WriteRowMark(this.m_tableNestingLevel, count2);
        --this.m_tableNestingLevel;
      }
    }
    this.CurrentWriter.PAPX.PropertyModifiers.Clear();
  }

  private void SetTableNestingLevel(SinglePropertyModifierArray sprms, int value)
  {
    if (value > 0)
      sprms.SetIntValue(26185, value);
    else
      sprms.RemoveValue(26185);
  }

  private void SetCellMark(SinglePropertyModifierArray sprms, bool value)
  {
    if (value)
      sprms.SetBoolValue(9238, value);
    else
      sprms.RemoveValue(9238);
  }

  private void WriteTableProps(IWordWriterBase writer, WTableRow row, IWTable table)
  {
    TablePropertiesConverter.FormatToSprms(row, this.CurrentWriter.PAPX.PropertyModifiers, writer.StyleSheet);
    CharacterPropertiesConverter.FormatToSprms(row.CharacterFormat, writer.CHPX.PropertyModifiers, writer.StyleSheet);
  }

  private void WriteImage(IWPicture picture)
  {
    WPicture wPict = picture as WPicture;
    wPict.IsHeaderPicture = this.CurrentWriter is WordHeaderFooterWriter || this.CurrentWriter is WordHFTextBoxWriter;
    int height = (int) Math.Round((double) wPict.Size.Height * 20.0);
    int width = (int) Math.Round((double) wPict.Size.Width * 20.0);
    if (wPict.TextWrappingStyle == TextWrappingStyle.Inline)
      this.WriteInlinePicture(wPict, height, width);
    else
      this.WritePictureShape(wPict, height, width);
  }

  private void WritePictureShape(WPicture wPict, int height, int width)
  {
    this.CheckShapeForCloning((ParagraphItem) wPict);
    if (wPict.CharacterFormat != null && this.CurrentWriter != null && this.CurrentWriter.CHPX != null)
      CharacterPropertiesConverter.FormatToSprms(wPict.CharacterFormat, this.CurrentWriter.CHPX.PropertyModifiers, this.CurrentWriter.StyleSheet);
    int num1 = (int) Math.Round((double) wPict.VerticalPosition * 20.0);
    int num2 = (int) Math.Round((double) wPict.HorizontalPosition * 20.0);
    PictureShapeProps pictProps = new PictureShapeProps();
    if (wPict.HorizontalOrigin == HorizontalOrigin.LeftMargin || wPict.HorizontalOrigin == HorizontalOrigin.RightMargin || wPict.HorizontalOrigin == HorizontalOrigin.InsideMargin || wPict.HorizontalOrigin == HorizontalOrigin.OutsideMargin)
      pictProps.RelHrzPos = HorizontalOrigin.Margin;
    else
      pictProps.RelHrzPos = wPict.HorizontalOrigin;
    pictProps.RelVrtPos = wPict.VerticalOrigin;
    pictProps.XaLeft = num2;
    pictProps.YaTop = num1;
    pictProps.Width = (int) ((double) width / 100.0 * (double) wPict.WidthScale);
    pictProps.Height = (int) ((double) height / 100.0 * (double) wPict.HeightScale);
    pictProps.TextWrappingType = wPict.TextWrappingType;
    if (wPict.TextWrappingStyle == TextWrappingStyle.Behind)
      pictProps.IsBelowText = true;
    else
      pictProps.IsBelowText = wPict.IsBelowText;
    pictProps.TextWrappingStyle = wPict.TextWrappingStyle;
    pictProps.HorizontalAlignment = wPict.HorizontalAlignment;
    pictProps.VerticalAlignment = wPict.VerticalAlignment;
    pictProps.Spid = wPict.ShapeId;
    pictProps.TxbxCount = 0;
    pictProps.AlternativeText = wPict.AlternativeText;
    pictProps.Name = wPict.Name;
    this.CurrentWriter.InsertShape(wPict, pictProps);
    if (wPict.EmbedBody == null)
      return;
    this.WriteEmbedBody(wPict.EmbedBody, pictProps.Spid);
  }

  private void WriteEmbedBody(WTextBody text, int shapeId)
  {
    WTextBox textBoxItem = new WTextBox((IWordDocument) this.m_document);
    textBoxItem.SetTextBody(text);
    for (int index1 = 0; index1 < text.Items.Count; ++index1)
    {
      TextBodyItem textBodyItem = text.Items[index1];
      if (textBodyItem is WParagraph)
      {
        WParagraph wparagraph = textBodyItem as WParagraph;
        for (int index2 = wparagraph.Items.Count - 1; index2 >= 0; --index2)
        {
          ParagraphItem paragraphItem = wparagraph.Items[index2];
          if (paragraphItem is BookmarkStart && !(paragraphItem as BookmarkStart).Name.StartsWithExt("OLE_LINK") || paragraphItem is BookmarkEnd && !(paragraphItem as BookmarkEnd).Name.StartsWithExt("OLE_LINK"))
            wparagraph.Items.Remove((IEntity) paragraphItem);
        }
      }
    }
    textBoxItem.TextBoxSpid = shapeId;
    textBoxItem.TextBoxFormat.TextBoxIdentificator = (float) (this.m_currWriter as WordWriterBase).NextTextId;
    this.PrepareTextBoxColl(textBoxItem);
    if (!(this.m_document.Escher.FindContainerBySpid(shapeId) is MsofbtSpContainer containerBySpid))
      return;
    if (containerBySpid.ShapeOptions.Properties[267] is FOPTEBid property)
      property.Value = (uint) textBoxItem.TextBoxFormat.TextBoxIdentificator;
    else
      containerBySpid.ShapeOptions.Properties.Add((FOPTEBase) new FOPTEBid(267, false, (uint) textBoxItem.TextBoxFormat.TextBoxIdentificator));
  }

  private void WriteInlinePicture(WPicture wPict, int height, int width)
  {
    this.CheckShapeForCloning((ParagraphItem) wPict);
    CharacterPropertiesConverter.FormatToSprms(wPict.CharacterFormat, this.CurrentWriter.CHPX.PropertyModifiers, this.CurrentWriter.StyleSheet);
    if (wPict.Document.Settings.CompatibilityMode != CompatibilityMode.Word2003 && wPict.PictureShape.ShapeContainer != null && wPict.PictureShape.ShapeContainer.ShapeOptions != null)
      wPict.PictureShape = this.ConvertToInlineShape(wPict.PictureShape);
    if (wPict.PictureShape.ShapeContainer == null || wPict.PictureShape.ShapeContainer.Shape == null || wPict.PictureShape.ShapeContainer != null && wPict.PictureShape.ShapeContainer.Bse.Blip.IsDib || wPict.IsMetaFile && wPict.PictureShape.ShapeContainer.Bse.Blip is MsofbtImage || wPict.Document.IsReadOnly)
    {
      this.CurrentWriter.InsertImage(wPict, height, width);
    }
    else
    {
      wPict.PictureShape.ShapeContainer.CheckOptContainer();
      wPict.PictureShape.ShapeContainer.WritePictureOptions(new PictureShapeProps()
      {
        AlternativeText = wPict.AlternativeText,
        Name = wPict.Name
      }, wPict);
      wPict.PictureShape.PictureDescriptor.SetBasePictureOptions(height, width, wPict.HeightScale, wPict.WidthScale);
      (this.m_currWriter as WordWriterBase).InsertInlineShapeObject(wPict.PictureShape);
      if (!wPict.PictureShape.ShapeContainer.Bse.IsPictureInShapeField || !this.IsInShapeField(wPict))
        return;
      wPict.PictureShape.ShapeContainer.Bse.IsInlineBlip = false;
    }
  }

  private bool IsInShapeField(WPicture picture)
  {
    for (IEntity previousSibling = picture.PreviousSibling; previousSibling != null; previousSibling = previousSibling.PreviousSibling)
    {
      if (previousSibling is WFieldMark)
      {
        WFieldMark wfieldMark = previousSibling as WFieldMark;
        return wfieldMark.Type == FieldMarkType.FieldSeparator && wfieldMark.ParentField != null && wfieldMark.ParentField.FieldType == FieldType.FieldShape;
      }
    }
    return false;
  }

  private InlineShapeObject ConvertToInlineShape(InlineShapeObject pictureShape)
  {
    InlineShapeObject inlineShape = pictureShape;
    uint num1 = 0;
    if (inlineShape.ShapeContainer.ShapeOptions.Properties.ContainsKey(459))
      num1 = inlineShape.ShapeContainer.GetPropertyValue(459);
    uint num2 = (uint) Math.Round((double) num1 / 12700.0 * 8.0);
    inlineShape.PictureDescriptor.BorderLeft.LineWidth = (byte) num2;
    inlineShape.PictureDescriptor.BorderTop.LineWidth = (byte) num2;
    inlineShape.PictureDescriptor.BorderRight.LineWidth = (byte) num2;
    inlineShape.PictureDescriptor.BorderBottom.LineWidth = (byte) num2;
    BorderStyle borderStyle = BorderStyle.None;
    if (inlineShape.ShapeContainer.ShapeOptions.Properties.ContainsKey(461))
    {
      TextBoxLineStyle propertyValue = (TextBoxLineStyle) inlineShape.ShapeContainer.GetPropertyValue(461);
      borderStyle = inlineShape.GetBorderStyle(LineDashing.Solid, propertyValue);
      if (propertyValue == TextBoxLineStyle.Simple)
        borderStyle = BorderStyle.Single;
    }
    if (inlineShape.ShapeContainer.ShapeOptions.Properties.ContainsKey(462))
    {
      LineDashing propertyValue = (LineDashing) inlineShape.ShapeContainer.GetPropertyValue(462);
      borderStyle = inlineShape.GetBorderStyle(propertyValue, TextBoxLineStyle.Simple);
      if (propertyValue == LineDashing.Solid && borderStyle == BorderStyle.None)
        borderStyle = BorderStyle.Single;
    }
    if (borderStyle != BorderStyle.None)
    {
      inlineShape.PictureDescriptor.BorderLeft.BorderType = (byte) borderStyle;
      inlineShape.PictureDescriptor.BorderTop.BorderType = (byte) borderStyle;
      inlineShape.PictureDescriptor.BorderRight.BorderType = (byte) borderStyle;
      inlineShape.PictureDescriptor.BorderBottom.BorderType = (byte) borderStyle;
    }
    if (inlineShape.ShapeContainer.ShapeOptions.Properties.ContainsKey(448))
    {
      uint propertyValue = inlineShape.ShapeContainer.GetPropertyValue(448);
      int id = WordColor.ConvertColorToId(WordColor.ConvertRGBToColor(propertyValue));
      inlineShape.PictureDescriptor.BorderLeft.LineColor = (byte) id;
      inlineShape.PictureDescriptor.BorderTop.LineColor = (byte) id;
      inlineShape.PictureDescriptor.BorderRight.LineColor = (byte) id;
      inlineShape.PictureDescriptor.BorderBottom.LineColor = (byte) id;
      inlineShape.ShapeContainer.ShapePosition.SetPropertyValue(924, propertyValue);
      inlineShape.ShapeContainer.ShapePosition.SetPropertyValue(923, propertyValue);
      inlineShape.ShapeContainer.ShapePosition.SetPropertyValue(926, propertyValue);
      inlineShape.ShapeContainer.ShapePosition.SetPropertyValue(925, propertyValue);
    }
    return inlineShape;
  }

  private void WriteShapeObject(ShapeObject shapeObject)
  {
    this.CheckShapeForCloning((ParagraphItem) shapeObject);
    CharacterPropertiesConverter.FormatToSprms(shapeObject.CharacterFormat, this.CurrentWriter.CHPX.PropertyModifiers, this.CurrentWriter.StyleSheet);
    if (shapeObject is InlineShapeObject)
    {
      (this.m_currWriter as WordWriterBase).InsertInlineShapeObject(shapeObject as InlineShapeObject);
    }
    else
    {
      (this.m_currWriter as WordWriterBase).InsertShapeObject(shapeObject);
      this.WriteShapeObjTextBody(shapeObject);
    }
  }

  private void WriteShapeObjTextBody(ShapeObject shapeObject)
  {
    if (this.m_currWriter is WordHeaderFooterWriter)
    {
      int index = 0;
      for (int count = shapeObject.AutoShapeTextCollection.Count; index < count; ++index)
        this.HFTextBoxCollection.Add((IWTextBox) (shapeObject.AutoShapeTextCollection[index] as WTextBox));
    }
    else
    {
      int index = 0;
      for (int count = shapeObject.AutoShapeTextCollection.Count; index < count; ++index)
        this.TextBoxCollection.Add((IWTextBox) (shapeObject.AutoShapeTextCollection[index] as WTextBox));
    }
  }

  private void WriteSectionEnd(IWSection section)
  {
    if (!(this.CurrentWriter is WordWriter))
      return;
    if (this.m_secNumber != 0)
    {
      SectionPropertiesConverter.Import(this.m_mainWriter.SectionProperties, section as WSection);
      this.m_mainWriter.WriteMarker(WordChunkType.SectionEnd);
    }
    this.m_currSection = section;
    ++this.m_secNumber;
  }

  private void WriteBookmarkStart(BookmarkStart start)
  {
    this.m_mainWriter.InsertBookmarkStart(start.Name, start);
  }

  private void WriteBookmarkEnd(BookmarkEnd end) => this.m_mainWriter.InsertBookmarkEnd(end.Name);

  private void WriteBreak(Break docBreak, WParagraph paragraph)
  {
    CharacterPropertiesConverter.FormatToSprms(docBreak.CharacterFormat, this.CurrentWriter.CHPX.PropertyModifiers, this.CurrentWriter.StyleSheet);
    if (docBreak.BreakType == BreakType.ColumnBreak)
      this.m_mainWriter.WriteMarker(WordChunkType.ColumnBreak);
    else if (docBreak.BreakType == BreakType.PageBreak)
    {
      if (!(this.CurrentWriter is WordWriter))
        return;
      (this.CurrentWriter as WordWriter).InsertPageBreak();
    }
    else if (docBreak.BreakType == BreakType.LineBreak && docBreak.TextRange.Text == ControlChar.CarriegeReturn)
      this.WriteText(docBreak.TextRange);
    else
      this.CurrentWriter.WriteMarker(WordChunkType.LineBreak);
  }

  private void WriteSymbol(WSymbol symbol)
  {
    if (this.CurrentWriter.StyleSheet.FontNameToIndex(symbol.FontName) == -1)
      this.CurrentWriter.StyleSheet.UpdateFontName(symbol.FontName);
    this.UpdateCharStyleIndex(symbol.CharacterFormat.CharStyleName, false);
    CharacterPropertiesConverter.FormatToSprms(symbol.CharacterFormat, this.CurrentWriter.CHPX.PropertyModifiers, this.CurrentWriter.StyleSheet);
    this.CurrentWriter.CHPX.PropertyModifiers.SetByteArrayValue(27145, new SymbolDescriptor()
    {
      CharCode = symbol.CharacterCode,
      CharCodeExt = symbol.CharCodeExt,
      FontCode = ((short) this.CurrentWriter.StyleSheet.FontNameToIndex(symbol.FontName))
    }.Save());
    this.CurrentWriter.WriteMarker(WordChunkType.Symbol);
  }

  private void WriteTextBoxShape(WTextBox textBoxItem)
  {
    textBoxItem.TextBoxFormat.IsHeaderTextBox = this.CurrentWriter is WordHeaderFooterWriter;
    this.CheckShapeForCloning((ParagraphItem) textBoxItem);
    if (textBoxItem.CharacterFormat != null && this.CurrentWriter != null && this.CurrentWriter.CHPX != null)
      CharacterPropertiesConverter.FormatToSprms(textBoxItem.CharacterFormat, this.CurrentWriter.CHPX.PropertyModifiers, this.CurrentWriter.StyleSheet);
    TextBoxProps textBoxProps = new TextBoxProps();
    textBoxItem.TextBoxSpid = this.CurrentWriter.InsertTextBox(textBoxItem.Visible, textBoxItem.TextBoxFormat);
    this.PrepareTextBoxColl(textBoxItem);
  }

  private void PrepareTextBoxColl(WTextBox textBoxItem)
  {
    if (this.m_currWriter is WordHeaderFooterWriter)
      this.HFTextBoxCollection.Add((IWTextBox) textBoxItem);
    else
      this.TextBoxCollection.Add((IWTextBox) textBoxItem);
  }

  private void CheckShapeForCloning(ParagraphItem shapeItem)
  {
    if (!shapeItem.IsCloned)
      return;
    this.m_document.CloneShapeEscher(this.m_document, (IParagraphItem) shapeItem);
  }

  private void WriteFieldMarkAndText(WFieldMark fldMark)
  {
    if (this.CurrentField == null)
      return;
    if (fldMark.Type == FieldMarkType.FieldEnd && this.CurrentField != null && this.CurrentField.FieldType == FieldType.FieldDocVariable && fldMark.PreviousSibling is WField && this.m_document.UpdateFields)
    {
      bool chpxStickProperties = this.CurrentWriter.CHPXStickProperties;
      this.CurrentWriter.CHPXStickProperties = true;
      this.WriteFieldSeparator();
      this.CurrentWriter.CHPXStickProperties = chpxStickProperties;
    }
    CharacterPropertiesConverter.FormatToSprms(this.CurrentField is WFormField ? this.CurrentField.CharacterFormat : fldMark.CharacterFormat, this.CurrentWriter.CHPX.PropertyModifiers, this.CurrentWriter.StyleSheet);
    if (fldMark.Type == FieldMarkType.FieldSeparator)
      this.WriteFieldSeparator();
    else if (this.CurrentField.IsFieldWithoutSeparator)
    {
      if (this.CurrentWriter.CHPX.PropertyModifiers[2050] == null)
        this.CurrentWriter.CHPX.PropertyModifiers.SetBoolValue(2050, true);
      this.CurrentWriter.WriteMarker(WordChunkType.FieldEndMark);
      this.m_fieldStack.Pop();
    }
    else
      this.WriteFieldEnd();
  }

  private void WriteFieldSeparator()
  {
    if (this.CurrentField != null)
    {
      if (this.CurrentField.FieldType == FieldType.FieldEmbed || this.CurrentField.FieldType == FieldType.FieldLink)
        this.WriteOleObjectCharProps(this.CurrentField);
      else if (this.CurrentField.FieldType == FieldType.FieldOCX)
        this.CurrentWriter.CHPX.PropertyModifiers.SetIntValue(27139, (this.CurrentField as WControlField).StoragePicLocation);
    }
    this.CurrentWriter.InsertFieldSeparator();
  }

  private void WriteFieldEnd()
  {
    this.CurrentWriter.InsertEndField();
    if (this.m_fieldStack.Count <= 0)
      return;
    this.m_fieldStack.Pop();
  }

  private void WriteComment(WComment comment)
  {
    if (comment.AppendItems)
      this.WriteCommItems(comment);
    else
      this.CountCommOffset(comment);
    this.CommentCollection.Add(comment);
    (this.CurrentWriter as WordWriter).InsertComment(comment.Format);
  }

  private void WriteFootnote(WFootnote footnote)
  {
    footnote.EnsureFtnMarker();
    if (footnote.FootnoteType == FootnoteType.Footnote)
      this.FootnoteCollection.Add(footnote);
    else
      this.EndnoteCollection.Add(footnote);
    this.UpdateCharStyleIndex(footnote.MarkerCharacterFormat.CharStyleName, false);
    CharacterPropertiesConverter.FormatToSprms(footnote.MarkerCharacterFormat, this.CurrentWriter.CHPX.PropertyModifiers, this.CurrentWriter.StyleSheet);
    (this.CurrentWriter as WordWriter).InsertFootnote(footnote);
  }

  private void WriteWatermark(Watermark watermark)
  {
    if (watermark is PictureWatermark && (watermark as PictureWatermark).Picture == null)
      return;
    SizeF pageSize = this.m_document.LastSection.PageSetup.PageSize;
    MarginsF margins = this.m_document.LastSection.PageSetup.Margins;
    float maxWidth = pageSize.Width - margins.Left - margins.Right;
    this.CurrentWriter.InsertWatermark(watermark, UnitsConvertor.Instance, maxWidth);
  }

  private void WriteTOC(TableOfContent toc) => this.WriteBeginField(toc.TOCField);

  private void WriteFieldWithoutSeparator(string fieldCode, WField field)
  {
    if (field.FieldEnd != null)
    {
      this.CurrentWriter.CHPXStickProperties = true;
      if (this.CurrentWriter.CHPX.PropertyModifiers[2050] == null)
        this.CurrentWriter.CHPX.PropertyModifiers.SetBoolValue(2050, true);
      this.CurrentWriter.WriteMarker(WordChunkType.FieldBeginMark);
      this.CurrentWriter.WriteSafeChunk(fieldCode);
      this.CurrentWriter.CHPXStickProperties = false;
      this.CurrentWriter.WriteMarker(WordChunkType.FieldEndMark);
    }
    else
    {
      if (string.IsNullOrEmpty(fieldCode))
        return;
      this.CurrentWriter.WriteSafeChunk(fieldCode);
    }
  }

  private void WriteCommMark(WCommentMark commMark)
  {
    if (commMark.Type == CommentMarkType.CommentStart)
    {
      DictionaryEntry dictionaryEntry = new DictionaryEntry((object) (this.CurrentWriter as WordWriter).GetTextPos(), (object) 0);
      if (this.CommentOffsets.ContainsKey(commMark.CommentId))
        return;
      this.CommentOffsets.Add(commMark.CommentId, dictionaryEntry);
    }
    else
    {
      if (!this.CommentOffsets.ContainsKey(commMark.CommentId))
        return;
      DictionaryEntry commentOffset = this.CommentOffsets[commMark.CommentId] with
      {
        Value = (object) (this.CurrentWriter as WordWriter).GetTextPos()
      };
      this.CommentOffsets[commMark.CommentId] = commentOffset;
    }
  }

  private void WriteOleObject(WOleObject oleObject) => this.WriteBeginField(oleObject.Field);

  private void AddListPictures()
  {
    foreach (ListStyle listStyle in (CollectionImpl) this.m_document.ListStyles)
    {
      if (listStyle == null || listStyle.Levels == null)
        break;
      int index = 0;
      for (int count = listStyle.Levels.Count; index < count; ++index)
      {
        WListLevel level = listStyle.Levels[index];
        if (level != null && level.PicBullet != null)
        {
          this.ListPicture.Add(level.PicBullet);
          int num = this.ListPicture.Count - 1;
          level.CharacterFormat.ListPictureIndex = num;
          level.CharacterFormat.ListHasPicture = true;
        }
      }
    }
  }

  private void AddPictures(WListFormat listFormat)
  {
  }

  private void WriteParagraphProperties(IWParagraph paragraph)
  {
    List<SinglePropertyModifierRecord> newSprms = new List<SinglePropertyModifierRecord>();
    List<SinglePropertyModifierRecord> oldSprms = new List<SinglePropertyModifierRecord>();
    this.WriteListProperties(paragraph, oldSprms, newSprms);
    if (paragraph.IsInCell && this.CurrentWriter.PAPX.PropertyModifiers.GetInt(26185, 1) > 1 && paragraph != (paragraph.Owner as WTableCell).LastParagraph)
      this.CurrentWriter.PAPX.PropertyModifiers.RemoveValue(9291);
    ParagraphPropertiesConverter.FormatToSprms(paragraph.ParagraphFormat, this.CurrentWriter.PAPX.PropertyModifiers, this.CurrentWriter.StyleSheet);
    if (paragraph.ParagraphFormat.m_unParsedSprms != null && paragraph.ParagraphFormat.m_unParsedSprms.Count > 0)
    {
      foreach (SinglePropertyModifierRecord unParsedSprm in paragraph.ParagraphFormat.m_unParsedSprms)
        this.CurrentWriter.PAPX.PropertyModifiers.InsertAt(unParsedSprm.Clone(), 0);
    }
    if (oldSprms.Count > 0)
    {
      foreach (SinglePropertyModifierRecord propertyModifierRecord in oldSprms)
        this.CurrentWriter.PAPX.PropertyModifiers.InsertAt(propertyModifierRecord.Clone(), 0);
    }
    if (newSprms.Count > 0)
    {
      foreach (SinglePropertyModifierRecord propertyModifierRecord in newSprms)
        this.CurrentWriter.PAPX.PropertyModifiers.Add(propertyModifierRecord.Clone());
    }
    this.WriteParagraphStyle(this.CurrentWriter, paragraph);
    this.CurrentWriter.PAPX.PropertyModifiers.SortSprms();
    this.CurrentWriter.BreakCHPX.PropertyModifiers.Clear();
    this.UpdateCharStyleIndex(paragraph.BreakCharacterFormat.CharStyleName, true);
    CharacterPropertiesConverter.FormatToSprms(paragraph.BreakCharacterFormat, this.CurrentWriter.BreakCHPX.PropertyModifiers, this.CurrentWriter.StyleSheet);
  }

  private void WriteParagraphStyle(IWordWriterBase writer, IWParagraph paragraph)
  {
    string styleName = paragraph.StyleName;
    if (styleName != null)
    {
      int num = writer.StyleSheet.StyleNameToIndex(styleName, false);
      if (num <= -1)
        return;
      if (num == 0 && paragraph.ParagraphFormat.m_unParsedSprms != null && paragraph.ParagraphFormat.m_unParsedSprms.Contain(17920))
        num = (int) paragraph.ParagraphFormat.m_unParsedSprms[17920].ShortValue;
      writer.CurrentStyleIndex = num;
      writer.PAPX.PropertyModifiers.SetByteArrayValue(17920, BitConverter.GetBytes((ushort) num));
    }
    else
      writer.CurrentStyleIndex = 0;
  }

  private void WriteStyleSheet(IWordWriter writer)
  {
    WordStyleSheet styleSheet = writer.StyleSheet;
    this.UpdateDefFormat();
    List<string> stringList = new List<string>();
    List<int> intList = new List<int>();
    int index1 = 0;
    for (int count = this.m_document.Styles.Count; index1 < count; ++index1)
    {
      Style style1 = this.m_document.Styles[index1] as Style;
      if (!(style1 is WTableStyle))
      {
        WParagraphStyle style2 = style1 as WParagraphStyle;
        WCharacterStyle wcharacterStyle = style1 as WCharacterStyle;
        bool flag = style1.StyleType == StyleType.CharacterStyle;
        int num = 0;
        int index2 = stringList.Contains(style1.Name) ? -1 : styleSheet.StyleNameToIndex(style1.Name, flag);
        stringList.Add(style1.Name);
        WordStyle style3;
        if ((style1.StyleId > 0 && style1.StyleId < 10 || style1.StyleId == 105 || style1.StyleId == 107) && !intList.Contains(style1.StyleId))
        {
          style3 = new WordStyle(styleSheet, style1.Name);
          style3.ID = style1.StyleId;
          if (style1.StyleId > 0 && style1.StyleId < 10 && !intList.Contains(style1.StyleId))
          {
            styleSheet.RemoveStyleByIndex(style3.ID);
            styleSheet.InsertStyle(style3.ID, style3);
          }
          else if (style1.StyleId == 105 && !intList.Contains(style1.StyleId))
          {
            styleSheet.RemoveStyleByIndex(11);
            styleSheet.InsertStyle(11, style3);
          }
          else if (style1.StyleId == 107 && !intList.Contains(style1.StyleId))
          {
            styleSheet.RemoveStyleByIndex(12);
            styleSheet.InsertStyle(12, style3);
          }
          intList.Add(style1.StyleId);
        }
        else if (index2 < 0)
        {
          index2 = styleSheet.StylesCount;
          switch (index2)
          {
            case 13:
              if (this.m_document.Styles.FixedIndex13HasStyle && this.m_document.Styles.FixedIndex13StyleName != null && this.m_document.Styles.FixedIndex13StyleName != string.Empty)
              {
                style3 = styleSheet.CreateStyle(this.m_document.Styles.FixedIndex13StyleName, flag);
                style3.ID = style1.StyleId;
                break;
              }
              WordStyle empty1 = WordStyle.Empty;
              styleSheet.InsertStyle(13, empty1);
              num = styleSheet.StylesCount;
              if (!this.m_document.Styles.FixedIndex14HasStyle || this.m_document.Styles.FixedIndex14StyleName == null || !(this.m_document.Styles.FixedIndex14StyleName != string.Empty))
              {
                WordStyle empty2 = WordStyle.Empty;
                styleSheet.InsertStyle(14, empty2);
                index2 = styleSheet.StylesCount;
                style3 = styleSheet.CreateStyle(style1.Name, flag);
                style3.ID = style1.StyleId;
                break;
              }
              continue;
            case 14:
              if (this.m_document.Styles.FixedIndex14HasStyle && this.m_document.Styles.FixedIndex14StyleName != null && this.m_document.Styles.FixedIndex14StyleName != string.Empty)
              {
                style3 = styleSheet.CreateStyle(this.m_document.Styles.FixedIndex14StyleName, flag);
                style3.ID = style1.StyleId;
                if (this.m_document.Styles.FixedIndex14StyleName != style1.Name)
                {
                  --index1;
                  break;
                }
                break;
              }
              WordStyle empty3 = WordStyle.Empty;
              styleSheet.InsertStyle(14, empty3);
              index2 = styleSheet.StylesCount;
              style3 = styleSheet.CreateStyle(style1.Name, flag);
              style3.ID = style1.StyleId;
              break;
            default:
              index2 = styleSheet.StylesCount;
              style3 = styleSheet.CreateStyle(style1.Name, flag);
              style3.ID = style1.StyleId;
              break;
          }
        }
        else
          style3 = styleSheet.GetStyleByIndex(index2);
        if (style2 != null)
        {
          ParagraphPropertiesConverter.FormatToSprms(style2.ParagraphFormat, style3.PAPX.PropertyModifiers, styleSheet);
          this.UpdateListInStyle((IWordWriterBase) writer, style2, style3.PAPX.PropertyModifiers);
          style3.PAPX.PropertyModifiers.SortSprms();
        }
        if (style1.CharacterFormat.HasKey(68))
          style3.CHPX.PropertyModifiers.RemoveValue(19023);
        CharacterPropertiesConverter.FormatToSprms(style1.CharacterFormat, style3.CHPX.PropertyModifiers, styleSheet);
        style3.IsPrimary = style1.IsPrimaryStyle;
        style3.IsSemiHidden = style1.IsSemiHidden;
        style3.UnhideWhenUsed = style1.UnhideWhenUsed;
        style3.TypeCode = style1.TypeCode;
        if (style1.TableStyleData != null && style1.TypeCode == WordStyleType.TableStyle)
        {
          style3.TableStyleData = new byte[style1.TableStyleData.Length];
          Buffer.BlockCopy((Array) style1.TableStyleData, 0, (Array) style3.TableStyleData, 0, style1.TableStyleData.Length);
        }
        if (wcharacterStyle != null)
        {
          try
          {
            this.m_charStylesHash.Add(style1.Name, index2);
          }
          catch
          {
          }
        }
      }
    }
    int index3 = 0;
    for (int count = this.m_document.Styles.Count; index3 < count; ++index3)
    {
      Style style = this.m_document.Styles[index3] as Style;
      if (!string.IsNullOrEmpty(style.Name))
      {
        bool isCharacter = style.StyleType == StyleType.CharacterStyle;
        int index4 = styleSheet.StyleNameToIndex(style.Name, isCharacter);
        if (style.BaseStyle != null)
        {
          int index5 = styleSheet.StyleNameToIndex(style.BaseStyle.Name, isCharacter);
          styleSheet.GetStyleByIndex(index4).BaseStyleIndex = index5;
        }
        if (style.NextStyle != null)
        {
          int index6 = styleSheet.StyleNameToIndex(style.NextStyle, isCharacter);
          styleSheet.GetStyleByIndex(index4).NextStyleIndex = index6;
        }
        if (!string.IsNullOrEmpty(style.LinkStyle))
        {
          int index7 = styleSheet.StyleNameToIndex(style.LinkStyle);
          styleSheet.GetStyleByIndex(index4).LinkStyleIndex = index7;
        }
      }
    }
  }

  private void UpdateDefFormat()
  {
    if (!((this.m_document.Styles as StyleCollection).FindFirstStyleByName("Normal") is WParagraphStyle firstStyleByName))
      return;
    if (this.m_document.m_defParaFormat != null)
    {
      if ((firstStyleByName.ParagraphFormat.PropertiesHash == null || firstStyleByName.ParagraphFormat.PropertiesHash.Count == 0) && firstStyleByName.ParagraphFormat.IsDefault)
        firstStyleByName.ParagraphFormat.ImportContainer((FormatBase) this.m_document.m_defParaFormat);
      else if (this.m_document.m_defParaFormat.PropertiesHash.Count > 0)
      {
        foreach (KeyValuePair<int, object> keyValuePair in this.m_document.m_defParaFormat.PropertiesHash)
        {
          if (keyValuePair.Key == 20 && firstStyleByName.ParagraphFormat.PropertiesHash.ContainsKey(20))
            ParagraphPropertiesConverter.CopyBorders((Borders) keyValuePair.Value, firstStyleByName.ParagraphFormat);
          if (!firstStyleByName.ParagraphFormat.PropertiesHash.ContainsKey(keyValuePair.Key))
            firstStyleByName.ParagraphFormat.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
        }
      }
    }
    if (this.m_document.DefCharFormat == null || this.m_document.DefCharFormat.PropertiesHash == null)
      return;
    if ((firstStyleByName.CharacterFormat.PropertiesHash == null || firstStyleByName.CharacterFormat.PropertiesHash.Count == 0) && firstStyleByName.CharacterFormat.IsDefault)
    {
      firstStyleByName.CharacterFormat.ImportContainer((FormatBase) this.m_document.DefCharFormat);
    }
    else
    {
      foreach (KeyValuePair<int, object> keyValuePair in this.m_document.DefCharFormat.PropertiesHash)
      {
        if (keyValuePair.Key == 67 && firstStyleByName.CharacterFormat.PropertiesHash.ContainsKey(67))
        {
          Border border = firstStyleByName.CharacterFormat.Border;
          Border destBorder = (Border) keyValuePair.Value;
          if (destBorder.IsBorderDefined && !border.IsBorderDefined)
            ParagraphPropertiesConverter.ExportBorder(border, destBorder);
        }
        if (!firstStyleByName.CharacterFormat.PropertiesHash.ContainsKey(keyValuePair.Key))
          firstStyleByName.CharacterFormat.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
      }
      if (firstStyleByName.CharacterFormat.CharStyleName == null)
        firstStyleByName.CharacterFormat.CharStyleName = this.m_document.DefCharFormat.CharStyleName;
      if (!firstStyleByName.CharacterFormat.HasKey(68))
      {
        string fontName = this.m_document.DefCharFormat.FontNameAscii;
        if (this.m_document.DefCharFormat.IsThemeFont(fontName))
          fontName = this.m_document.DefCharFormat.FontName;
        firstStyleByName.CharacterFormat.FontNameAscii = fontName;
      }
      if (!firstStyleByName.CharacterFormat.HasKey(61))
      {
        string fontName = this.m_document.DefCharFormat.FontNameBidi;
        if (this.m_document.DefCharFormat.IsThemeFont(fontName))
          fontName = this.m_document.DefCharFormat.FontName;
        firstStyleByName.CharacterFormat.FontNameBidi = fontName;
      }
      if (!firstStyleByName.CharacterFormat.HasKey(69))
      {
        string fontName = this.m_document.DefCharFormat.FontNameFarEast;
        if (this.m_document.DefCharFormat.IsThemeFont(fontName))
          fontName = this.m_document.DefCharFormat.FontName;
        firstStyleByName.CharacterFormat.FontNameFarEast = fontName;
      }
      if (firstStyleByName.CharacterFormat.HasKey(70))
        return;
      string fontName1 = this.m_document.DefCharFormat.FontNameNonFarEast;
      if (this.m_document.DefCharFormat.IsThemeFont(fontName1))
        fontName1 = this.m_document.DefCharFormat.FontName;
      firstStyleByName.CharacterFormat.FontNameNonFarEast = fontName1;
    }
  }

  private void WriteDocumentProperties()
  {
    if (this.m_document.BuiltinDocumentProperties != null)
      this.m_mainWriter.BuiltinDocumentProperties = this.m_document.BuiltinDocumentProperties.Clone();
    if (this.m_document.CustomDocumentProperties != null)
      this.m_mainWriter.CustomDocumentProperties = this.m_document.CustomDocumentProperties.Clone();
    this.m_mainWriter.WriteProtected = this.m_document.WriteProtected;
    this.m_mainWriter.HasPicture = this.m_document.HasPicture;
    this.m_mainWriter.SttbfRMark = this.m_document.SttbfRMark;
    if (this.m_document.MacrosData != null)
      this.m_mainWriter.MacrosStream = new MemoryStream(this.m_document.MacrosData);
    if (this.m_document.MacroCommands != null)
      this.m_mainWriter.MacroCommands = this.m_document.MacroCommands;
    if (this.m_document.GrammarSpellingData != null)
      this.m_mainWriter.GrammarSpellingData = this.m_document.GrammarSpellingData;
    if (this.m_document.DOP != null)
    {
      this.m_mainWriter.DOP = this.m_document.DOP;
      this.m_mainWriter.DOP.OddAndEvenPagesHeaderFooter = this.m_document.DifferentOddAndEvenPages;
      this.m_mainWriter.DOP.DefaultTabWidth = (ushort) Math.Round((double) this.m_document.DefaultTabWidth * 20.0);
      this.m_mainWriter.DOP.UpdateDateTime(this.m_mainWriter.BuiltinDocumentProperties);
    }
    if (this.m_document.AssociatedStrings != null)
      this.m_mainWriter.AssociatedStrings = this.m_document.AssociatedStrings.GetAssociatedStrings();
    this.WriteDocumentDefaultFont();
    this.m_mainWriter.DOP.ViewType = (byte) this.m_document.ViewSetup.DocumentViewType;
    if (this.m_document.ViewSetup.ZoomType != ZoomType.None)
      this.m_mainWriter.DOP.ZoomType = (byte) this.m_document.ViewSetup.ZoomType;
    if (this.m_document.ViewSetup.ZoomPercent != 100)
      this.m_mainWriter.DOP.ZoomPercent = (ushort) this.m_document.ViewSetup.ZoomPercent;
    if (this.m_document.Variables.Count <= 0)
      return;
    this.m_mainWriter.Variables = this.m_document.Variables.ToByteArray();
  }

  private void WriteDocumentDefaultFont()
  {
    WCharacterFormat defCharFormat = this.m_document.DefCharFormat;
    string fontName1 = this.m_document.StandardAsciiFont;
    if (string.IsNullOrEmpty(fontName1) && defCharFormat != null && defCharFormat.HasValue(68))
    {
      fontName1 = defCharFormat.FontNameAscii;
      if (defCharFormat.IsThemeFont(fontName1))
        fontName1 = defCharFormat.FontName;
    }
    this.m_mainWriter.StandardAsciiFont = fontName1;
    string fontName2 = this.m_document.StandardFarEastFont;
    if (string.IsNullOrEmpty(fontName2) && defCharFormat != null && defCharFormat.HasValue(69))
    {
      fontName2 = defCharFormat.FontNameFarEast;
      if (defCharFormat.IsThemeFont(fontName2))
        fontName2 = defCharFormat.FontName;
    }
    this.m_mainWriter.StandardFarEastFont = fontName2;
    string fontName3 = this.m_document.StandardNonFarEastFont;
    if (string.IsNullOrEmpty(fontName3) && defCharFormat != null && defCharFormat.HasValue(70))
    {
      fontName3 = defCharFormat.FontNameNonFarEast;
      if (defCharFormat.IsThemeFont(fontName3))
        fontName3 = defCharFormat.FontName;
    }
    this.m_mainWriter.StandardNonFarEastFont = fontName3;
    string fontName4 = this.m_document.StandardBidiFont;
    if (string.IsNullOrEmpty(fontName4) && defCharFormat != null && defCharFormat.HasValue(61))
    {
      fontName4 = defCharFormat.FontNameBidi;
      if (defCharFormat.IsThemeFont(fontName4))
        fontName4 = defCharFormat.FontName;
    }
    this.m_mainWriter.StandardBidiFont = fontName4;
  }

  private void WriteBackground()
  {
    Background background = this.m_document.Background;
    if (background.Type == BackgroundType.NoBackground)
      return;
    this.CheckEscher();
    EscherClass escher = this.m_document.Escher;
    MsofbtSpContainer msofbtSpContainer = new MsofbtSpContainer(this.m_document);
    msofbtSpContainer.CreateRectangleContainer();
    MsofbtSpContainer backgroundContainerValue = escher.GetBackgroundContainerValue();
    int shapeId = backgroundContainerValue.Shape.ShapeId;
    msofbtSpContainer.UpdateBackground(this.m_document, background);
    MsofbtDgContainer containerForSubDocType = escher.FindDgContainerForSubDocType(ShapeDocType.Main);
    containerForSubDocType.Children.Remove((object) backgroundContainerValue);
    escher.Containers.Remove(shapeId);
    containerForSubDocType.Children.Add((object) msofbtSpContainer);
    escher.Containers.Add(shapeId, (BaseContainer) msofbtSpContainer);
  }

  private void WritePictureBackground(
    MsofbtSpContainer backContainer,
    MsofbtSpContainer oldBackContainer,
    Background background,
    EscherClass escher)
  {
    MsofbtBSE bse = new MsofbtBSE(this.m_document);
    bse.Initialize(background.ImageRecord);
    backContainer.Bse = bse;
    uint num = backContainer.GetPropertyValue(390);
    if (num != uint.MaxValue)
    {
      escher.ModifyBStoreByPid((int) num, bse);
    }
    else
    {
      escher.m_msofbtDggContainer.BstoreContainer.Children.Add((object) bse);
      num = (uint) escher.m_msofbtDggContainer.BstoreContainer.Children.Count;
    }
    backContainer.UpdateFillPicture(background, (int) num);
  }

  private void CheckEscher()
  {
    EscherClass escher = this.m_document.Escher;
    if (escher != null && escher.m_dgContainers.Count == 0 || escher == null)
    {
      EscherClass escherClass = new EscherClass(this.m_document);
      escherClass.CreateDgForSubDocuments();
      this.m_document.Escher = escherClass;
    }
    else
    {
      if (escher.m_msofbtDggContainer.BstoreContainer != null)
        return;
      escher.m_msofbtDggContainer.Children.Add((object) new MsofbtBstoreContainer(this.m_document));
    }
  }

  private void WriteCommItems(WComment comment)
  {
    int num = 0;
    if (comment.CommentedBodyPart != null)
    {
      this.WriteParagraphProperties((IWParagraph) comment.OwnerParagraph);
      this.CurrentWriter.WriteMarker(WordChunkType.ParagraphEnd);
      num = (this.CurrentWriter as WordWriter).GetTextPos();
      this.WriteParagraphs(comment.CommentedBodyPart.BodyItems, false);
    }
    else if (comment.CommentedItems.Count > 0)
    {
      num = (this.CurrentWriter as WordWriter).GetTextPos();
      foreach (ParagraphItem commentedItem in (CollectionImpl) comment.CommentedItems)
        this.WriteParaItem(commentedItem, (IWParagraph) comment.OwnerParagraph);
    }
    int textPos = (this.CurrentWriter as WordWriter).GetTextPos();
    comment.Format.BookmarkStartOffset = textPos - num;
    comment.Format.BookmarkEndOffset = 0;
  }

  private void CountCommOffset(WComment comment)
  {
    if (!this.CommentOffsets.ContainsKey(comment.Format.TagBkmk))
      return;
    DictionaryEntry commentOffset = this.CommentOffsets[comment.Format.TagBkmk];
    int key = (int) commentOffset.Key;
    int num = (int) commentOffset.Value;
    if (num == 0)
      return;
    comment.Format.BookmarkStartOffset = num - key;
    if (comment.Format.BookmarkStartOffset == 0)
      comment.Format.BookmarkEndOffset = 1;
    else
      comment.Format.BookmarkEndOffset = 0;
  }

  internal void Close()
  {
    this.m_document = (WordDocument) null;
    if (this.m_txbxItems != null)
      this.m_txbxItems = (WTextBoxCollection) null;
    if (this.m_hfTxbxItems != null)
      this.m_hfTxbxItems = (WTextBoxCollection) null;
    if (this.m_commentCollection != null)
      this.m_commentCollection = (List<WComment>) null;
    if (this.m_footnoteCollection != null)
      this.m_footnoteCollection = (List<WFootnote>) null;
    if (this.m_endnoteCollection != null)
      this.m_endnoteCollection = (List<WFootnote>) null;
    if (this.m_charStylesHash != null)
    {
      this.m_charStylesHash.Clear();
      this.m_charStylesHash = (Dictionary<string, int>) null;
    }
    if (this.m_listData != null)
    {
      this.m_listData.Clear();
      this.m_listData = (Dictionary<string, ListData>) null;
    }
    if (this.m_bookmarksAfterCell != null)
    {
      this.m_bookmarksAfterCell.Clear();
      this.m_bookmarksAfterCell = (List<string>) null;
    }
    if (this.m_fieldStack != null)
    {
      this.m_fieldStack.Clear();
      this.m_fieldStack = (Stack<WField>) null;
    }
    if (this.m_commOffsets != null)
    {
      this.m_commOffsets.Clear();
      this.m_commOffsets = (Dictionary<string, DictionaryEntry>) null;
    }
    if (this.m_listPicture != null)
    {
      this.m_listPicture.Clear();
      this.m_listPicture = (List<WPicture>) null;
    }
    if (this.m_oleObjects != null)
    {
      this.m_oleObjects.Clear();
      this.m_oleObjects = (List<WOleObject>) null;
    }
    if (this.m_OLEObjects != null)
    {
      this.m_OLEObjects.Clear();
      this.m_OLEObjects = (List<Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.OLEObject>) null;
    }
    CharacterPropertiesConverter.Close();
  }

  private void UpdateCharStyleIndex(string charStyleName, bool isParaBreak)
  {
    if (charStyleName == null || !this.m_charStylesHash.ContainsKey(charStyleName))
      return;
    ushort num = (ushort) this.m_charStylesHash[charStyleName];
    if (num == (ushort) 0)
      return;
    if (isParaBreak)
      this.CurrentWriter.BreakCHPX.PropertyModifiers.SetUShortValue(18992, num);
    else
      this.CurrentWriter.CHPX.PropertyModifiers.SetUShortValue(18992, num);
  }

  private void WriteBreakAfter(WParagraph curPara, BreakType type)
  {
    if (curPara.NextSibling is WParagraph nextSibling)
      this.WriteParagraphProperties((IWParagraph) nextSibling);
    if (type == BreakType.PageBreak)
    {
      this.m_mainWriter.WriteMarker(WordChunkType.PageBreak);
    }
    else
    {
      if (type != BreakType.ColumnBreak)
        return;
      this.m_mainWriter.WriteMarker(WordChunkType.ColumnBreak);
    }
  }

  private bool IsPageBreakNeedToBeSkipped(Entity entity)
  {
    Entity entity1 = entity;
    while (entity1.Owner != null)
    {
      entity1 = entity1.Owner;
      switch (entity1)
      {
        case WTextBox _:
        case WFootnote _:
        case HeaderFooter _:
          return true;
        default:
          continue;
      }
    }
    return false;
  }

  private void WriteListProperties(
    IWParagraph paragraph,
    List<SinglePropertyModifierRecord> oldSprms,
    List<SinglePropertyModifierRecord> newSprms)
  {
    this.WriteListProperties(paragraph.ListFormat, (IWordWriterBase) (this.CurrentWriter as WordWriterBase));
    Dictionary<int, object> dictionary = new Dictionary<int, object>((IDictionary<int, object>) paragraph.ListFormat.PropertiesHash);
    if (this.CurrentWriter.PAPX.PropertyModifiers.Count > 0)
    {
      foreach (SinglePropertyModifierRecord propertyModifier in this.CurrentWriter.PAPX.PropertyModifiers)
        newSprms.Add(propertyModifier.Clone());
    }
    paragraph.ListFormat.PropertiesHash.Clear();
    foreach (KeyValuePair<int, object> keyValuePair in paragraph.ListFormat.OldPropertiesHash)
      paragraph.ListFormat.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
    this.CurrentWriter.PAPX.PropertyModifiers.Clear();
    this.WriteListProperties(paragraph.ListFormat, (IWordWriterBase) (this.CurrentWriter as WordWriterBase));
    if (this.CurrentWriter.PAPX.PropertyModifiers.Count > 0)
    {
      foreach (SinglePropertyModifierRecord propertyModifier in this.CurrentWriter.PAPX.PropertyModifiers)
        oldSprms.Add(propertyModifier.Clone());
    }
    paragraph.ListFormat.PropertiesHash.Clear();
    foreach (KeyValuePair<int, object> keyValuePair in dictionary)
      paragraph.ListFormat.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
  }

  private void WriteListProperties(WListFormat listFormat, IWordWriterBase writer)
  {
    if (listFormat.ListType == ListType.NoList && listFormat.ListLevelNumber < 1)
      this.ProcessEmptyList(listFormat, writer);
    else if (listFormat.CustomStyleName != string.Empty)
      this.ProcessList(listFormat, writer);
    else if (listFormat.ListLevelNumber > 0)
    {
      writer.PAPX.PropertyModifiers.SetByteValue(9738, (byte) listFormat.ListLevelNumber);
    }
    else
    {
      if (listFormat.ListLevelNumber <= 0)
        return;
      writer.PAPX.PropertyModifiers.SetByteValue(9738, (byte) listFormat.ListLevelNumber);
    }
  }

  private void ProcessEmptyList(WListFormat listFormat, IWordWriterBase writer)
  {
    if (listFormat.IsListRemoved)
    {
      this.RemoveListSprms(writer.PAPX.PropertyModifiers);
    }
    else
    {
      if (!listFormat.IsEmptyList)
        return;
      this.WriteEmptyList(writer.PAPX.PropertyModifiers);
    }
  }

  private void ProcessList(WListFormat listFormat, IWordWriterBase writer)
  {
    bool isBuiltInStyle = listFormat.CurrentListStyle.IsBuiltInStyle;
    if (isBuiltInStyle)
      this.RemoveListSprms(writer.PAPX.PropertyModifiers);
    else if (this.m_prevStyleName != listFormat.CustomStyleName)
    {
      if (AdapterListIDHolder.Instance.ContainsListName(listFormat.CurrentListStyle.Name))
        this.ContinueCurrentList(writer, listFormat);
      else
        this.ApplyStyle(writer, listFormat, isBuiltInStyle);
    }
    else
      this.ContinueCurrentList(writer, listFormat);
  }

  internal void RemoveListSprms(SinglePropertyModifierArray Sprms)
  {
    int count = Sprms.Modifiers.Count;
    for (int index = 0; index < count; ++index)
    {
      SinglePropertyModifierRecord modifier = Sprms.Modifiers[index];
      if (modifier.TypedOptions == 17931 || modifier.TypedOptions == 9738)
      {
        Sprms.Modifiers.Remove(modifier);
        --count;
      }
    }
  }

  internal void WriteEmptyList(SinglePropertyModifierArray Sprms)
  {
    SinglePropertyModifierRecord propertyModifierRecord1 = Sprms[17931];
    if (propertyModifierRecord1 == null)
    {
      propertyModifierRecord1 = new SinglePropertyModifierRecord(17931);
      Sprms.Modifiers.Add(propertyModifierRecord1);
    }
    propertyModifierRecord1.ShortValue = (short) 0;
    SinglePropertyModifierRecord propertyModifierRecord2 = Sprms[9738];
    if (propertyModifierRecord2 == null)
    {
      propertyModifierRecord2 = new SinglePropertyModifierRecord(9738);
      Sprms.Modifiers.Add(propertyModifierRecord2);
    }
    propertyModifierRecord2.ShortValue = (short) 0;
  }

  private void ContinueCurrentList(IWordWriterBase writer, WListFormat listFormat)
  {
    ListData listData = this.m_listData[listFormat.CustomStyleName];
    writer.ListProperties.ContinueCurrentList(listData, listFormat, writer.StyleSheet);
    this.m_prevStyleName = listFormat.CustomStyleName;
  }

  private void ApplyStyle(IWordWriterBase writer, WListFormat listFormat, bool useBaseStyle)
  {
    ListStyle byName = this.m_document.ListStyles.FindByName(listFormat.CustomStyleName);
    if (byName == null)
      return;
    ListData listData = this.CreateListData(byName, writer.StyleSheet, listFormat);
    if (!this.m_listData.ContainsKey(listFormat.CustomStyleName))
      this.m_listData.Add(listFormat.CustomStyleName, listData);
    if (useBaseStyle)
      this.ModifyBaseStyles((int) writer.ListProperties.StyleListIndexes[listFormat.CustomStyleName], writer);
    writer.ListProperties.ApplyList(listData, listFormat, writer.StyleSheet, true);
    this.m_prevStyleName = listFormat.CustomStyleName;
  }

  private void ModifyBaseStyles(int listFormatIndex, IWordWriterBase writer)
  {
    int currentStyleIndex = writer.CurrentStyleIndex;
    WordStyle styleByIndex = writer.StyleSheet.GetStyleByIndex(currentStyleIndex);
    short num = styleByIndex.PAPX.PropertyModifiers.GetShort(17931, (short) -1);
    if ((int) num == listFormatIndex || num == (short) -1)
      return;
    styleByIndex.PAPX.PropertyModifiers.SetShortValue(17931, (short) listFormatIndex);
  }

  private ListData CreateListData(
    ListStyle listStyle,
    WordStyleSheet styleSheet,
    WListFormat lstFormat)
  {
    ListData listFormat = new ListData(this.m_listID, listStyle.IsHybrid, listStyle.IsSimple);
    AdapterListIDHolder.Instance.ListStyleIDtoName.Add(this.m_listID, listStyle.Name);
    ListPropertiesConverter.Import(listStyle, listFormat, styleSheet);
    ++this.m_listID;
    return listFormat;
  }

  private void UpdateListInStyle(
    IWordWriterBase writer,
    WParagraphStyle style,
    SinglePropertyModifierArray sprms)
  {
    if (!style.ListFormat.IsEmptyList && !style.ListFormat.IsListRemoved && style.ListFormat.ListLevelNumber < 1 && (style.ListFormat.ListType == ListType.NoList || style.ListFormat.CurrentListStyle == null))
      return;
    if (style.ListFormat.IsEmptyList)
      this.WriteEmptyList(sprms);
    else if (style.ListFormat.IsListRemoved)
      this.RemoveListSprms(sprms);
    else if (style.ListFormat.CurrentListStyle == null && style.ListFormat.ListLevelNumber > 0)
      sprms.SetByteValue(9738, (byte) style.ListFormat.ListLevelNumber);
    else if (style.ListFormat.CurrentListStyle == null && style.ListFormat.ListLevelNumber > 0)
    {
      sprms.SetByteValue(9738, (byte) style.ListFormat.ListLevelNumber);
    }
    else
    {
      string name = style.ListFormat.CurrentListStyle.Name;
      if (AdapterListIDHolder.Instance.ContainsListName(name))
      {
        short styleListIndex = writer.ListProperties.StyleListIndexes[name];
        if (style.ListFormat.ListLevelNumber != -1)
          sprms.SetByteValue(9738, (byte) style.ListFormat.ListLevelNumber);
        if (sprms[17931] == null)
          sprms.Add(new SinglePropertyModifierRecord(17931));
        sprms[17931].ShortValue = styleListIndex;
      }
      else
      {
        ListStyle currentListStyle = style.ListFormat.CurrentListStyle;
        WListFormat listFormat = style.ListFormat;
        ListData listData = this.CreateListData(currentListStyle, writer.StyleSheet, listFormat);
        this.m_listData.Add(listFormat.CustomStyleName, listData);
        int num = writer.ListProperties.ApplyList(listData, listFormat, writer.StyleSheet, false);
        if (listFormat.ListLevelNumber != -1)
          sprms.SetByteValue(9738, (byte) listFormat.ListLevelNumber);
        if (sprms[17931] == null)
          sprms.Add(new SinglePropertyModifierRecord(17931));
        sprms[17931].ShortValue = (short) num;
      }
    }
  }

  private void ResetLists()
  {
    this.m_prevStyleName = (string) null;
    AdapterListIDHolder.Instance.ListStyleIDtoName.Clear();
    if (this.m_listData == null || this.m_listData.Count <= 0)
      return;
    this.m_listData.Clear();
  }
}
