// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.BookmarksNavigator
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class BookmarksNavigator
{
  private const string c_DocumentPropertyNotInitialized = "You can not use DocumentNavigator without initializing Document property";
  private const string c_NotFoundSpecifiedBookmark = "Specified bookmark not found";
  private const string c_NotEqualDocumentProperty = " Document property must be equal this Document property";
  private const string c_CurrBookmarkNull = "Current Bookmark didn't select";
  private const string c_NotSupportGettingContent = "Not supported getting content between bookmarks in different paragraphs";
  private const string c_NotSupportDeletingContent = "Not supported deleting content between bookmarks in different paragraphs";
  private WordDocument m_document;
  private int m_currParagraphItemIndex;
  private IWParagraph m_currParagraph;
  private Bookmark m_currBookmark;
  private IParagraphItem m_currBookmarkItem;
  private byte m_flag = 4;

  public IWordDocument Document
  {
    get => (IWordDocument) this.m_document;
    set => this.m_document = (WordDocument) value;
  }

  public Bookmark CurrentBookmark => this.m_currBookmark;

  private bool IsStart
  {
    get => ((int) this.m_flag & 1) != 0;
    set => this.m_flag = (byte) ((int) this.m_flag & 254 | (value ? 1 : 0));
  }

  private bool IsAfter
  {
    get => ((int) this.m_flag & 2) >> 1 != 0;
    set => this.m_flag = (byte) ((int) this.m_flag & 253 | (value ? 1 : 0) << 1);
  }

  internal bool RemoveEmptyParagraph
  {
    get => ((int) this.m_flag & 4) >> 1 != 0;
    set => this.m_flag = (byte) ((int) this.m_flag & 251 | (value ? 1 : 0) << 2);
  }

  public IParagraphItem CurrentBookmarkItem
  {
    get
    {
      this.m_currBookmarkItem = this.IsStart || this.m_currBookmark.BookmarkEnd == null ? (IParagraphItem) this.m_currBookmark.BookmarkStart : (IParagraphItem) this.m_currBookmark.BookmarkEnd;
      return this.m_currBookmarkItem;
    }
  }

  private int CurrentParagraphItemIndex
  {
    get
    {
      if (this.m_currBookmark == null || !(this.CurrentBookmarkItem.Owner is WParagraph))
        throw new ArgumentException("Specified bookmark not found");
      this.m_currParagraphItemIndex = !this.IsAfter ? this.m_currBookmarkItem.OwnerParagraph.ChildEntities.IndexOf((IEntity) this.m_currBookmarkItem) : this.m_currBookmarkItem.OwnerParagraph.ChildEntities.IndexOf((IEntity) this.m_currBookmarkItem) + 1;
      return this.m_currParagraphItemIndex;
    }
  }

  public BookmarksNavigator(IWordDocument doc) => this.m_document = (WordDocument) doc;

  public void MoveToBookmark(string bookmarkName)
  {
    this.MoveToBookmark(bookmarkName, false, false);
  }

  public void MoveToBookmark(string bookmarkName, bool isStart, bool isAfter)
  {
    this.IsStart = isStart;
    this.IsAfter = isAfter;
    string name = bookmarkName.Replace('-', '_');
    if (this.m_document == null)
      throw new InvalidOperationException("You can not use DocumentNavigator without initializing Document property");
    this.m_currBookmark = this.m_document.Bookmarks.FindByName(name);
    if (this.m_currBookmark == null)
      throw new ArgumentException("Specified bookmark not found");
    this.m_currParagraph = (IWParagraph) (this.IsStart || this.m_currBookmark.BookmarkEnd == null ? (IParagraphItem) this.m_currBookmark.BookmarkStart : (IParagraphItem) this.m_currBookmark.BookmarkEnd).OwnerParagraph;
  }

  public IWTextRange InsertText(string text) => this.InsertText(text, true);

  public IWTextRange InsertText(string text, bool saveFormatting)
  {
    return this.InsertText(text, saveFormatting, false);
  }

  public void InsertTable(IWTable table) => this.InsertBodyItem(table as TextBodyItem);

  public IParagraphItem InsertParagraphItem(ParagraphItemType itemType)
  {
    IParagraphItem paragraphItem = (IParagraphItem) this.m_document.CreateParagraphItem(itemType);
    this.m_currParagraph.Items.Insert(this.CurrentParagraphItemIndex, (IEntity) paragraphItem);
    return paragraphItem;
  }

  public void InsertParagraph(IWParagraph paragraph)
  {
    this.InsertBodyItem(paragraph as TextBodyItem);
  }

  public void InsertTextBodyPart(TextBodyPart bodyPart)
  {
    if (this.CurrentBookmarkItem == null)
      return;
    TextBodyItem textBodyItem = this.m_currBookmarkItem.Owner as TextBodyItem;
    int inOwnerCollection = textBodyItem.GetIndexInOwnerCollection();
    int pItemIndex = (this.m_currBookmarkItem as ParagraphItem).GetIndexInOwnerCollection();
    BookmarkEnd currBookmarkItem = this.m_currBookmarkItem is BookmarkEnd ? this.m_currBookmarkItem as BookmarkEnd : (BookmarkEnd) null;
    WParagraph wparagraph = textBodyItem as WParagraph;
    if (currBookmarkItem != null && currBookmarkItem.Name != "_GoBack" && !currBookmarkItem.HasRenderableItemBefore())
    {
      WParagraph ownerParagraph = this.Document.Bookmarks.FindByName(currBookmarkItem.Name).BookmarkStart.OwnerParagraph;
      WParagraph previousSibling = wparagraph.PreviousSibling as WParagraph;
      if (ownerParagraph != wparagraph && previousSibling != null)
      {
        inOwnerCollection = previousSibling.GetIndexInOwnerCollection();
        pItemIndex = previousSibling.ChildEntities.Count;
        textBodyItem = (TextBodyItem) previousSibling;
      }
    }
    if (this.IsStart)
    {
      while (pItemIndex < (textBodyItem as WParagraph).Items.Count)
      {
        ParagraphItem paragraphItem = (textBodyItem as WParagraph).Items[pItemIndex - 1];
        if (paragraphItem is BookmarkEnd && (paragraphItem as BookmarkEnd).Name != this.CurrentBookmark.Name)
        {
          ++pItemIndex;
          if (pItemIndex > (textBodyItem as WParagraph).Items.Count - 1)
            break;
        }
        else
          break;
      }
    }
    else
    {
      while (pItemIndex > 0)
      {
        ParagraphItem paragraphItem = (textBodyItem as WParagraph).Items[pItemIndex - 1];
        if (paragraphItem is BookmarkStart && (paragraphItem as BookmarkStart).Name != this.CurrentBookmark.Name)
        {
          --pItemIndex;
          if (pItemIndex < 1)
            break;
        }
        else
          break;
      }
    }
    bodyPart.PasteAt((ITextBody) textBodyItem.OwnerTextBody, inOwnerCollection, pItemIndex, true);
  }

  public TextBodyPart GetBookmarkContent()
  {
    this.CheckCurrentState();
    TextBodyPart bookmarkContent = new TextBodyPart();
    bookmarkContent.GetBookmarkContentPart(this.CurrentBookmark.BookmarkStart, this.CurrentBookmark.BookmarkEnd);
    return bookmarkContent;
  }

  public WordDocumentPart GetContent()
  {
    this.CheckCurrentState();
    WordDocumentPart content = new WordDocumentPart();
    content.GetWordDocumentPart(this.CurrentBookmark.BookmarkStart, this.CurrentBookmark.BookmarkEnd);
    return content;
  }

  public void DeleteBookmarkContent(bool saveFormatting)
  {
    this.DeleteBookmarkContent(saveFormatting, false);
  }

  [Obsolete("This method will be removed in future version. As a work around to remove bookmarked paragraph, utilize current bookmark property of bookmark navigator to access the current bookmarked paragraph and then remove its index from its owner (Text Body) collection.", false)]
  public void DeleteBookmarkContent(bool saveFormatting, bool removeEmptyParagraph)
  {
    this.DeleteBookmarkContent(saveFormatting, false, false);
  }

  public void ReplaceBookmarkContent(TextBodyPart bodyPart)
  {
    this.CurrentBookmark.BookmarkStart.Document.ImportStyles = false;
    if (bodyPart.BodyItems != null && bodyPart.BodyItems.Document != null)
      bodyPart.BodyItems.Document.IsSkipFieldDetach = true;
    this.DeleteBookmarkContent(true, false, true);
    string name = this.CurrentBookmark.Name;
    int bkmkIndex = this.Document.Bookmarks.InnerList.IndexOf((object) this.CurrentBookmark);
    if (!this.CurrentBookmarkItem.OwnerParagraph.IsInCell)
      this.InsertTextBodyPart(bodyPart);
    else
      this.ReplaceTableBookmarkContent((WordDocumentPart) null, bodyPart);
    this.ReplaceBookmarkIndex(name, bkmkIndex);
    this.CurrentBookmark.BookmarkStart.Document.ImportStyles = true;
    if (bodyPart.BodyItems == null || bodyPart.BodyItems.Document == null)
      return;
    bodyPart.BodyItems.Document.IsSkipFieldDetach = false;
  }

  public void ReplaceContent(WordDocumentPart documentPart)
  {
    this.CurrentBookmark.BookmarkStart.Document.ImportStyles = false;
    this.DeleteBookmarkContent(true, false, true);
    string name = this.CurrentBookmark.Name;
    int bkmkIndex = this.Document.Bookmarks.InnerList.IndexOf((object) this.CurrentBookmark);
    if (!this.CurrentBookmarkItem.OwnerParagraph.IsInCell)
      this.ReplaceParagraphBookmarkContent(documentPart);
    else
      this.ReplaceTableBookmarkContent(documentPart, (TextBodyPart) null);
    this.ReplaceBookmarkIndex(name, bkmkIndex);
    this.CurrentBookmark.BookmarkStart.Document.ImportStyles = true;
  }

  public void ReplaceBookmarkContent(string text, bool saveFormatting)
  {
    this.CurrentBookmark.BookmarkStart.Document.ImportStyles = false;
    this.DeleteBookmarkContent(saveFormatting, true, true);
    this.InsertText(text, saveFormatting, true);
    this.CurrentBookmark.BookmarkStart.Document.ImportStyles = true;
  }

  private void ReplaceBookmarkIndex(string bookmarkName, int bkmkIndex)
  {
    this.m_currBookmark = this.Document.Bookmarks.FindByName(bookmarkName);
    WParagraph ownerParagraph1 = this.m_currBookmark.BookmarkStart.OwnerParagraph;
    WParagraph ownerParagraph2 = this.m_currBookmark.BookmarkEnd.OwnerParagraph;
    int inOwnerCollection1 = this.m_currBookmark.BookmarkStart.GetIndexInOwnerCollection();
    int inOwnerCollection2 = this.m_currBookmark.BookmarkEnd.GetIndexInOwnerCollection();
    this.Document.Bookmarks.Remove(this.m_currBookmark);
    BookmarkStart start = new BookmarkStart(this.Document, bookmarkName);
    ownerParagraph1.Items.Insert(inOwnerCollection1, (IEntity) start);
    BookmarkEnd end = new BookmarkEnd(this.Document, bookmarkName);
    ownerParagraph2.Items.Insert(inOwnerCollection2, (IEntity) end);
    this.CurrentBookmark.SetStart(start);
    this.CurrentBookmark.SetEnd(end);
    this.Document.Bookmarks.InnerList.Insert(bkmkIndex, (object) this.CurrentBookmark);
    this.Document.Bookmarks.InnerList.RemoveAt(this.Document.Bookmarks.Count - 1);
    this.m_currBookmark = this.Document.Bookmarks.FindByName(bookmarkName);
  }

  private void ReplaceParagraphBookmarkContent(WordDocumentPart documentPart)
  {
    if (documentPart.Sections.Count == 1)
    {
      this.InsertTextBodyPart(new TextBodyPart()
      {
        m_textPart = documentPart.Sections[0].Body
      });
    }
    else
    {
      if (documentPart.Sections.Count <= 1)
        return;
      int inOwnerCollection1 = this.GetSection(this.CurrentBookmark.BookmarkStart.Owner).GetIndexInOwnerCollection();
      int index1 = 0;
      while (index1 < documentPart.Sections.Count)
      {
        if (index1 == 0)
        {
          WSection wsection = documentPart.Sections[0].Clone();
          WParagraph ownerParagraph = this.CurrentBookmark.BookmarkStart.OwnerParagraph;
          WTextBody ownerTextBody = ownerParagraph.OwnerTextBody;
          int inOwnerCollection2 = ownerParagraph.GetIndexInOwnerCollection();
          int inOwnerCollection3 = this.CurrentBookmark.BookmarkStart.GetIndexInOwnerCollection();
          if (wsection.Body.Items[0] is WParagraph)
          {
            WParagraph wparagraph = wsection.Body.Items[0] as WParagraph;
            for (; inOwnerCollection3 >= 0; --inOwnerCollection3)
            {
              wparagraph.Items.Insert(0, (IEntity) ownerParagraph.Items[inOwnerCollection3].Clone());
              ownerParagraph.Items[inOwnerCollection3].RemoveSelf();
            }
            --inOwnerCollection2;
          }
          else if (this.CurrentBookmark.BookmarkStart.OwnerParagraph == this.CurrentBookmark.BookmarkEnd.OwnerParagraph)
          {
            WParagraph wparagraph = ownerParagraph.CloneWithoutItems();
            for (; inOwnerCollection3 >= 0; --inOwnerCollection3)
            {
              wparagraph.Items.Insert(0, (IEntity) ownerParagraph.Items[inOwnerCollection3].Clone());
              ownerParagraph.Items[inOwnerCollection3].RemoveSelf();
            }
            wsection.Body.Items.Insert(0, (IEntity) wparagraph);
            --inOwnerCollection2;
          }
          for (; inOwnerCollection2 > -1; --inOwnerCollection2)
          {
            wsection.Body.Items.Insert(0, (IEntity) ownerTextBody.Items[inOwnerCollection2].Clone());
            ownerTextBody.Items[inOwnerCollection2].RemoveSelf();
          }
          this.Document.Sections.Insert(inOwnerCollection1, (IEntity) wsection);
        }
        else if (index1 == documentPart.Sections.Count - 1)
        {
          documentPart.Sections[documentPart.Sections.Count - 1].BreakCode = SectionBreakCode.NewPage;
          WSection wsection = documentPart.Sections[documentPart.Sections.Count - 1].Clone();
          WParagraph ownerParagraph = this.CurrentBookmark.BookmarkEnd.OwnerParagraph;
          WTextBody ownerTextBody = ownerParagraph.OwnerTextBody;
          this.CurrentBookmark.BookmarkEnd.GetIndexInOwnerCollection();
          int index2 = documentPart.Sections[documentPart.Sections.Count - 1].Body.Items.Count - 1;
          for (int index3 = index2; index3 >= 0; --index3)
          {
            if (index3 == index2 && wsection.Body.Items[index2] is WParagraph)
            {
              WParagraph wparagraph = wsection.Body.Items[index2] as WParagraph;
              while (wparagraph.Items.Count != 0)
                ownerParagraph.Items.Insert(0, (IEntity) wparagraph.Items[wparagraph.Items.Count - 1]);
            }
            else
              ownerTextBody.Items.Insert(0, (IEntity) wsection.Body.Items[index3]);
          }
          this.m_currBookmark = this.Document.Bookmarks.FindByName(this.CurrentBookmark.Name);
          for (int index4 = 0; index4 < ownerParagraph.ChildEntities.Count; ++index4)
          {
            if (ownerParagraph.ChildEntities[index4] is BookmarkEnd && (ownerParagraph.ChildEntities[index4] as BookmarkEnd).Name == this.CurrentBookmark.Name)
            {
              this.m_currBookmark.SetEnd(ownerParagraph.ChildEntities[index4] as BookmarkEnd);
              break;
            }
          }
        }
        else
          this.Document.Sections.Insert(inOwnerCollection1, (IEntity) documentPart.Sections[index1].Clone());
        ++index1;
        ++inOwnerCollection1;
      }
    }
  }

  private void DeleteBookmarkContent(
    bool saveFormatting,
    bool removeEmptyParagrph,
    bool isReplaceBkmkContent)
  {
    this.m_document.IsDeletingBookmarkContent = true;
    BookmarkStart bookmarkStart = this.CurrentBookmark.BookmarkStart;
    BookmarkEnd bookmarkEnd = this.CurrentBookmark.BookmarkEnd;
    this.IsStart = false;
    this.IsAfter = false;
    if (this.CurrentBookmark == null)
      throw new InvalidOperationException();
    if (bookmarkEnd != null)
    {
      WParagraph ownerParagraph1 = bookmarkStart.OwnerParagraph;
      WParagraph ownerParagraph2 = bookmarkEnd.OwnerParagraph;
      if (ownerParagraph1.Owner != ownerParagraph2.Owner)
      {
        if (!ownerParagraph1.IsInCell && !ownerParagraph2.IsInCell)
          this.DeleteBookmarkTextBody(saveFormatting, isReplaceBkmkContent);
        else if (ownerParagraph1.IsInCell && ownerParagraph2.IsInCell)
          this.DeleteBkmkContentInDiffCell(ownerParagraph1, ownerParagraph2, bookmarkStart, bookmarkEnd, (WTableCell) null, isReplaceBkmkContent);
        else if (ownerParagraph1.IsInCell && !ownerParagraph2.IsInCell)
          this.DeleteBkmkContentInTableAfterParagraph(ownerParagraph1, ownerParagraph2, bookmarkStart, bookmarkEnd, isReplaceBkmkContent);
        else if (!ownerParagraph1.IsInCell && ownerParagraph2.IsInCell)
          this.DeleteBkmkContentInParagraphAftertable(ownerParagraph1, ownerParagraph2, bookmarkStart, bookmarkEnd, isReplaceBkmkContent);
      }
      else if (bookmarkEnd.IsAfterCellMark || bookmarkEnd.IsAfterRowMark || bookmarkEnd.IsAfterTableMark)
        this.DeleteBkmkContentInDiffCell(ownerParagraph1, ownerParagraph2, bookmarkStart, bookmarkEnd, (WTableCell) null, isReplaceBkmkContent);
      else
        this.DeleteBookmarkTextBody(saveFormatting, isReplaceBkmkContent);
    }
    if (this.m_document.Bookmarks.InnerList.Contains((object) this.CurrentBookmark))
      this.MoveToBookmark(this.CurrentBookmark.Name, this.IsStart, this.IsAfter);
    this.m_document.IsDeletingBookmarkContent = false;
  }

  private void DeleteInBetweenSections(int startSectiontionIndex, int endSectiontionIndex)
  {
    for (int index = startSectiontionIndex; index < endSectiontionIndex; ++index)
      this.Document.Sections.RemoveAt(startSectiontionIndex);
  }

  private void DeleteBkmkContentInDiffCell(
    WParagraph paragraphStart,
    WParagraph paragraphEnd,
    BookmarkStart bkmkStart,
    BookmarkEnd bkmkEnd,
    WTableCell bkmkEndCell,
    bool isReplaceContent)
  {
    WTableCell ownerEntity = paragraphStart.GetOwnerEntity() as WTableCell;
    if (paragraphEnd.IsInCell)
      bkmkEndCell = paragraphEnd.GetOwnerEntity() as WTableCell;
    WTableCell tempBkmkEndCell = bkmkEndCell;
    WTable owner1 = (WTable) ownerEntity.Owner.Owner;
    WTable owner2 = (WTable) bkmkEndCell.Owner.Owner;
    int inOwnerCollection1 = ownerEntity.Owner.GetIndexInOwnerCollection();
    int inOwnerCollection2 = bkmkEndCell.Owner.GetIndexInOwnerCollection();
    int inOwnerCollection3 = bkmkEndCell.GetIndexInOwnerCollection();
    int inOwnerCollection4 = ownerEntity.GetIndexInOwnerCollection();
    bkmkStart.GetBookmarkStartAndEndCell(ownerEntity, bkmkEndCell, tempBkmkEndCell, owner1, owner2, bkmkStart, bkmkEnd, inOwnerCollection1, ref inOwnerCollection2, ref inOwnerCollection4, ref inOwnerCollection3);
    if (owner1 != owner2)
    {
      if (isReplaceContent && owner2.Rows.Count - 1 != inOwnerCollection2)
        throw new InvalidOperationException("You cannot replace bookmark content when the bookmark starts in one table and ends in another table partially");
      if (owner2.IsInCell && bkmkStart.IsBookmarkEndAtSameCell(ownerEntity, owner1, ref owner2, ref inOwnerCollection2))
        this.DeleteNestedTableBookmarkTextBodyInSameCell(bkmkStart, ownerEntity, owner2, inOwnerCollection2);
      else
        this.DeleteTableBookmarkTextBody(owner1, owner2, inOwnerCollection1, inOwnerCollection2, isReplaceContent, paragraphEnd, bkmkEnd);
    }
    else
    {
      bool flag = this.IsBookmarkEnclosedTable(owner1, owner2, bkmkStart, bkmkEnd);
      this.DeleteTableBookmarkTextBody(owner1, inOwnerCollection1, inOwnerCollection2, inOwnerCollection4, inOwnerCollection3, bkmkStart, bkmkEnd);
      if (flag)
      {
        int bkmkIndex = this.Document.Bookmarks.InnerList.IndexOf((object) this.CurrentBookmark);
        WTextBody ownerTextBody = owner1.OwnerTextBody;
        int inOwnerCollection5 = owner1.GetIndexInOwnerCollection();
        if (owner1.NextSibling is WParagraph)
          this.ReplaceCurrentBookmark(owner1.NextSibling as WParagraph, 0, 1, bkmkIndex);
        else if (owner1.PreviousSibling is WParagraph)
        {
          WParagraph previousSibling = owner1.PreviousSibling as WParagraph;
          this.ReplaceCurrentBookmark(previousSibling, previousSibling.Items.Count, previousSibling.Items.Count + 1, bkmkIndex);
        }
        else
        {
          WParagraph paragraph = new WParagraph(this.Document);
          ownerTextBody.ChildEntities.Insert(inOwnerCollection5, (IEntity) paragraph);
          this.ReplaceCurrentBookmark(paragraph, 0, 1, bkmkIndex);
        }
        owner1.RemoveSelf();
      }
    }
    this.m_currBookmark = this.Document.Bookmarks.FindByName(bkmkEnd.Name);
    if (this.m_currBookmark.BookmarkEnd != null)
      return;
    this.m_currBookmark.SetEnd(bkmkEnd);
  }

  private void CreateBookmark(
    IWParagraph paragraphStart,
    IWParagraph paragraphEnd,
    int bkmkIndex,
    BookmarkStart bkmkStart,
    int columnFirst,
    int columnLast)
  {
    if (this.Document.Bookmarks.FindByName(this.CurrentBookmark.Name) != null)
      this.Document.Bookmarks.Remove(this.CurrentBookmark);
    BookmarkStart start = new BookmarkStart(this.Document, this.CurrentBookmark.Name);
    paragraphStart.ChildEntities.Add((IEntity) start);
    BookmarkEnd end = new BookmarkEnd(this.Document, this.CurrentBookmark.Name);
    paragraphEnd.ChildEntities.Add((IEntity) end);
    this.CurrentBookmark.SetStart(start);
    this.CurrentBookmark.SetEnd(end);
    if (bkmkIndex < this.Document.Bookmarks.Count)
    {
      this.Document.Bookmarks.InnerList.Insert(bkmkIndex, (object) this.CurrentBookmark);
      this.Document.Bookmarks.InnerList.RemoveAt(this.Document.Bookmarks.Count - 1);
    }
    this.m_currBookmark = this.Document.Bookmarks.FindByName(bkmkStart.Name);
    this.m_currBookmark.BookmarkStart.ColumnFirst = (short) columnFirst;
    this.m_currBookmark.BookmarkStart.ColumnLast = (short) columnLast;
  }

  private bool IsBookmarkEnclosedTable(
    WTable bkmkStartTable,
    WTable bkmkEndTable,
    BookmarkStart bookmarkStart,
    BookmarkEnd bookmarkEnd)
  {
    int inOwnerCollection1 = bookmarkStart.GetIndexInOwnerCollection();
    int inOwnerCollection2 = bookmarkEnd.GetIndexInOwnerCollection();
    bool flag1 = inOwnerCollection1 == 0 && bookmarkStart.OwnerParagraph == bkmkStartTable.FirstRow.Cells[0].Paragraphs[0];
    WParagraph nextSibling = bkmkEndTable.NextSibling as WParagraph;
    bool flag2 = bookmarkEnd.OwnerParagraph == bkmkEndTable.LastCell.LastParagraph && inOwnerCollection2 == bkmkEndTable.LastCell.LastParagraph.ChildEntities.Count - 1 || nextSibling != null && bookmarkEnd.OwnerParagraph == nextSibling && !bookmarkEnd.HasRenderableItemBefore();
    return flag1 && flag2;
  }

  private void DeleteNestedTableBookmarkTextBodyInSameCell(
    BookmarkStart bkmrkStart,
    WTableCell bkmkStartCell,
    WTable endTable,
    int bkmkEndRowIndex)
  {
    int index1 = this.Document.Bookmarks.InnerList.IndexOf((object) this.CurrentBookmark);
    WParagraph ownerParagraph = bkmrkStart.OwnerParagraph;
    int index2 = ownerParagraph.Index;
    int index3 = endTable.Index;
    for (int index4 = ownerParagraph.Items.Count - 1; index4 >= bkmrkStart.Index + 1; --index4)
    {
      if (!(ownerParagraph.Items[index4] is BookmarkStart) && !(ownerParagraph.Items[index4] is BookmarkEnd))
        ownerParagraph.Items.RemoveAt(index4);
    }
    for (int index5 = index2; index5 <= index3 && index5 < bkmkStartCell.Items.Count; ++index5)
    {
      if (bkmkStartCell.Items[ownerParagraph.Index + 1] is WParagraph)
      {
        WParagraph paragraph = bkmkStartCell.Items[index2 + 1] as WParagraph;
        this.DeleteParagraphItemsInCell(paragraph);
        paragraph.RemoveSelf();
      }
      else if (bkmkStartCell.Items[index2 + 1] is WTable)
      {
        WTable bkmkTable = bkmkStartCell.Items[index2 + 1] as WTable;
        if (bkmkTable == endTable)
        {
          this.DeleteTableRows(0, bkmkEndRowIndex, bkmkTable);
          break;
        }
        bkmkTable.RemoveSelf();
      }
    }
    if (this.Document.Bookmarks.FindByName(this.CurrentBookmark.Name) != null)
      this.Document.Bookmarks.Remove(this.CurrentBookmark);
    BookmarkStart start = new BookmarkStart(this.Document, this.CurrentBookmark.Name);
    ownerParagraph.ChildEntities.Add((IEntity) start);
    BookmarkEnd end = new BookmarkEnd(this.Document, this.CurrentBookmark.Name);
    ownerParagraph.ChildEntities.Add((IEntity) end);
    this.CurrentBookmark.SetStart(start);
    this.CurrentBookmark.SetEnd(end);
    if (index1 < this.Document.Bookmarks.Count)
    {
      this.Document.Bookmarks.InnerList.Insert(index1, (object) this.CurrentBookmark);
      this.Document.Bookmarks.InnerList.RemoveAt(this.Document.Bookmarks.Count - 1);
    }
    this.m_currBookmark = this.Document.Bookmarks.FindByName(start.Name);
  }

  private void DeleteTableBookmarkTextBody(
    WTable bkmkStartTable,
    WTable bkmkEndTable,
    int bkmkStartRowIndex,
    int bkmkEndRowIndex,
    bool isReplaceContent,
    WParagraph paragraphEnd,
    BookmarkEnd bkmkEnd)
  {
    int bkmkIndex = this.Document.Bookmarks.InnerList.IndexOf((object) this.CurrentBookmark);
    WTextBody owner1 = (WTextBody) bkmkStartTable.Owner;
    WTextBody owner2 = (WTextBody) bkmkEndTable.Owner;
    int num1 = this.GetSection((Entity) bkmkStartTable).GetIndexInOwnerCollection() + 1;
    int inOwnerCollection1 = this.GetSection((Entity) bkmkEndTable).GetIndexInOwnerCollection();
    int num2 = bkmkStartTable.GetIndexInOwnerCollection() + 1;
    int inOwnerCollection2 = bkmkEndTable.GetIndexInOwnerCollection();
    bool isInSingleSection = false;
    if (num1 - 1 == inOwnerCollection1)
      isInSingleSection = true;
    WTable wtable = (WTable) null;
    if (bkmkStartTable.IsInCell)
    {
      wtable = owner1.Owner.Owner as WTable;
      while (wtable != null && wtable.IsInCell)
        wtable = wtable.Owner.Owner.Owner as WTable;
    }
    if (wtable != null && wtable == bkmkEndTable)
    {
      this.DeleteTableRows(bkmkStartRowIndex, bkmkStartTable.Rows.Count - 1, bkmkStartTable);
      WTable owner3 = owner1.Owner.Owner as WTable;
      int index = owner1.Owner.Index;
      while (owner3 != null && owner3 != bkmkEndTable)
      {
        this.DeleteTableRows(index, owner3.Rows.Count - 1, owner3);
        owner3 = owner3.Owner.Owner.Owner as WTable;
        index = owner3.Owner.Owner.Index;
      }
    }
    else
    {
      this.DeleteTableRows(bkmkStartRowIndex, bkmkStartTable.Rows.Count - 1, bkmkStartTable);
      this.DeleteFirstSectionItemsFromDocument(num2, ref inOwnerCollection2, owner1, -1, isInSingleSection);
    }
    if (!isInSingleSection)
      this.DeleteInBetweenSections(inOwnerCollection1, inOwnerCollection1);
    this.DeleteLastSectionItemsFromDocument(inOwnerCollection2, owner2, true, isInSingleSection);
    if (this.IsBkmkEndFirstItemInTable(bkmkEndRowIndex, bkmkEndTable, paragraphEnd, bkmkEnd))
      bkmkEnd.RemoveSelf();
    else
      this.DeleteTableRows(0, bkmkEndRowIndex, bkmkEndTable, paragraphEnd, bkmkEnd);
    if (bkmkEndTable.Rows.Count == 0)
      bkmkEndTable.RemoveSelf();
    if (bkmkStartTable.Rows.Count == 0)
    {
      bkmkStartTable.RemoveSelf();
      --num2;
    }
    if (!isReplaceContent && this.CheckTwoTableProperties(bkmkStartTable, bkmkEndTable))
    {
      if (this.RemoveEmptyParagraph)
        this.SetCurrentBookmarkPosition(bkmkStartTable, bkmkEndTable, owner1, owner2, bkmkIndex);
      if (bkmkStartTable.Rows.Count != 0 && bkmkEndTable.Rows.Count != 0)
      {
        while (bkmkEndTable.Rows.Count != 0)
          bkmkStartTable.Rows.Add(bkmkEndTable.Rows[0]);
        bkmkStartTable.UpdateTableGrid(false, true);
      }
    }
    else
    {
      WParagraph paragraph = new WParagraph(this.Document);
      this.ReplaceCurrentBookmark(paragraph, 0, 1, bkmkIndex);
      owner1.Items.Insert(num2, (IEntity) paragraph);
    }
    if (isInSingleSection)
      return;
    this.MergeMultiSectionBodyItems(owner1, owner2);
  }

  private bool CheckTwoTableProperties(WTable bkmkStartTable, WTable bkmkEndTable)
  {
    bool flag = false;
    if (bkmkStartTable.TableFormat.WrapTextAround && bkmkEndTable.TableFormat.WrapTextAround)
    {
      if (bkmkStartTable.TableFormat.HorizontalAlignment != bkmkEndTable.TableFormat.HorizontalAlignment || bkmkStartTable.TableFormat.Positioning.AllowOverlap != bkmkEndTable.TableFormat.Positioning.AllowOverlap || (double) bkmkStartTable.TableFormat.Positioning.DistanceFromBottom != (double) bkmkEndTable.TableFormat.Positioning.DistanceFromBottom || (double) bkmkStartTable.TableFormat.Positioning.DistanceFromLeft != (double) bkmkStartTable.TableFormat.Positioning.DistanceFromLeft || (double) bkmkStartTable.TableFormat.Positioning.DistanceFromRight != (double) bkmkStartTable.TableFormat.Positioning.DistanceFromRight || (double) bkmkStartTable.TableFormat.Positioning.DistanceFromTop != (double) bkmkEndTable.TableFormat.Positioning.DistanceFromTop || bkmkStartTable.TableFormat.Positioning.VertRelationTo != bkmkEndTable.TableFormat.Positioning.VertRelationTo || (double) bkmkStartTable.TableFormat.Positioning.VertPosition != (double) bkmkEndTable.TableFormat.Positioning.VertPosition || bkmkStartTable.TableFormat.Positioning.VertPositionAbs != bkmkEndTable.TableFormat.Positioning.VertPositionAbs || (double) bkmkStartTable.TableFormat.Positioning.HorizPosition != (double) bkmkEndTable.TableFormat.Positioning.HorizPosition || bkmkStartTable.TableFormat.Positioning.HorizPositionAbs != bkmkEndTable.TableFormat.Positioning.HorizPositionAbs || bkmkStartTable.TableFormat.Positioning.HorizRelationTo != bkmkEndTable.TableFormat.Positioning.HorizRelationTo)
        return false;
      flag = true;
    }
    else if (!bkmkStartTable.TableFormat.WrapTextAround && !bkmkEndTable.TableFormat.WrapTextAround)
      flag = true;
    if (bkmkStartTable.StyleName != null || bkmkEndTable.StyleName != null)
      flag = bkmkStartTable.StyleName == bkmkEndTable.StyleName;
    return flag;
  }

  private void DeleteBkmkContentInTableAfterParagraph(
    WParagraph paragraphStart,
    WParagraph paragraphEnd,
    BookmarkStart bkmkStart,
    BookmarkEnd bkmkEnd,
    bool isReplaceContent)
  {
    int bkmkIndex = this.Document.Bookmarks.InnerList.IndexOf((object) this.CurrentBookmark);
    WTableCell wtableCell1 = paragraphStart.GetOwnerEntity() as WTableCell;
    WTableCell wtableCell2 = (WTableCell) null;
    WTable owner1 = (WTable) wtableCell1.Owner.Owner;
    int inOwnerCollection1 = wtableCell1.Owner.GetIndexInOwnerCollection();
    if (bkmkStart.ColumnFirst != (short) -1 && bkmkStart.ColumnLast != (short) -1)
    {
      int num = owner1.Rows.Count - 1;
      if ((int) bkmkStart.ColumnFirst < owner1.Rows[inOwnerCollection1].Cells.Count && (int) bkmkStart.ColumnLast < owner1.Rows[num].Cells.Count)
      {
        wtableCell1 = owner1.Rows[inOwnerCollection1].Cells[(int) bkmkStart.ColumnFirst];
        wtableCell2 = owner1.Rows[num].Cells[(int) bkmkStart.ColumnLast];
      }
      int inOwnerCollection2 = wtableCell1.GetIndexInOwnerCollection();
      int inOwnerCollection3 = wtableCell2.GetIndexInOwnerCollection();
      this.DeleteTableBookmarkTextBody(owner1, inOwnerCollection1, num, inOwnerCollection2, inOwnerCollection3, bkmkStart, bkmkEnd);
    }
    else
    {
      int inOwnerCollection4 = owner1.GetIndexInOwnerCollection();
      int inOwnerCollection5 = paragraphEnd.GetIndexInOwnerCollection();
      int bkmkEndPreviosItemIndex = bkmkEnd.GetIndexInOwnerCollection() - 1;
      WTextBody owner2 = (WTextBody) owner1.Owner;
      WTextBody owner3 = (WTextBody) paragraphEnd.Owner;
      bool isInSingleSection = false;
      int startSectiontionIndex = this.GetSection((Entity) owner2).GetIndexInOwnerCollection() + 1;
      int inOwnerCollection6 = this.GetSection((Entity) owner3).GetIndexInOwnerCollection();
      if (startSectiontionIndex - 1 == inOwnerCollection6)
        isInSingleSection = true;
      bool IsFirstBkmkEnd = this.IsBkmkEndInFirstItem(paragraphEnd, (ParagraphItem) bkmkEnd, bkmkEndPreviosItemIndex);
      if (!isInSingleSection || owner2.ChildEntities[inOwnerCollection4 + 1] != paragraphEnd || !IsFirstBkmkEnd)
      {
        this.DeleteTableRows(inOwnerCollection1, owner1.Rows.Count - 1, owner1);
        if (owner1.Rows.Count == 0)
        {
          owner1.RemoveSelf();
          --inOwnerCollection5;
          --inOwnerCollection4;
        }
        this.DeleteFirstSectionItemsFromDocument(inOwnerCollection4 + 1, ref inOwnerCollection5, owner2, -1, isInSingleSection);
        if (IsFirstBkmkEnd)
          --inOwnerCollection5;
        if (!isInSingleSection || !IsFirstBkmkEnd)
        {
          this.DeleteInBetweenSections(startSectiontionIndex, inOwnerCollection6);
          if (inOwnerCollection5 >= 0)
            this.DeleteLastSectionItemsFromDocument(inOwnerCollection5, owner3, IsFirstBkmkEnd, isInSingleSection);
        }
        WParagraph toInsertBookmark = this.GetParagraphToInsertBookmark(owner2, owner3, paragraphEnd, inOwnerCollection5, bkmkStart, bkmkEnd, isInSingleSection, false);
        if (toInsertBookmark != null)
        {
          this.ReplaceCurrentBookmark(toInsertBookmark, 0, 1, bkmkIndex);
          if (toInsertBookmark != paragraphEnd)
          {
            this.MoveNestedBookmark(paragraphEnd, toInsertBookmark);
            paragraphEnd.RemoveSelf();
          }
        }
        else
          this.ReplaceCurrentBookmark(paragraphEnd, 0, 1, bkmkIndex);
        if (isInSingleSection)
          return;
        this.MergeMultiSectionBodyItems(owner2, owner3);
      }
      else
        this.DeleteBkmkContentInDiffCell(paragraphStart, paragraphEnd, bkmkStart, bkmkEnd, owner1.LastCell, isReplaceContent);
    }
  }

  private void DeleteTableRows(int startRowIndex, int endRowIndex, WTable bkmkTable)
  {
    if (bkmkTable.PreviousSibling is Entity previousSibling)
    {
      Stack<Entity> bookmarks = new Stack<Entity>();
      this.UpdateBookmark((TextBodyItem) bkmkTable, bookmarks);
      WParagraph wparagraph = previousSibling as WParagraph;
      if (previousSibling is WTable)
        wparagraph = (previousSibling as WTable).LastCell.LastParagraph as WParagraph;
      int count = wparagraph.Items.Count;
      while (bookmarks.Count > 0)
      {
        Entity entity = bookmarks.Pop();
        if (startRowIndex == 0)
          wparagraph.Items.Insert(count, (IEntity) entity);
      }
    }
    for (int index = startRowIndex; index <= endRowIndex; ++index)
      bkmkTable.Rows[startRowIndex].RemoveSelf();
  }

  private void UpdateBookmark(TextBodyItem textBodyItem, Stack<Entity> bookmarks)
  {
    if (textBodyItem is WTable)
    {
      foreach (WTableRow row in (CollectionImpl) (textBodyItem as WTable).Rows)
      {
        foreach (WTextBody cell in (CollectionImpl) row.Cells)
        {
          foreach (TextBodyItem textBodyItem1 in (CollectionImpl) cell.Items)
            this.UpdateBookmark(textBodyItem1, bookmarks);
        }
      }
    }
    if (!(textBodyItem is WParagraph))
      return;
    for (int index = 0; index < (textBodyItem as WParagraph).Items.Count; ++index)
    {
      ParagraphItem paragraphItem = (textBodyItem as WParagraph).Items[index];
      if (paragraphItem is BookmarkStart && (paragraphItem as BookmarkStart).Name != this.CurrentBookmark.Name || paragraphItem is BookmarkEnd && (paragraphItem as BookmarkEnd).Name != this.CurrentBookmark.Name)
      {
        BookmarkStart bookmarkStart = paragraphItem is BookmarkStart ? paragraphItem as BookmarkStart : (this.Document.Bookmarks.FindByName((paragraphItem as BookmarkEnd).Name) != null ? this.Document.Bookmarks.FindByName((paragraphItem as BookmarkEnd).Name).BookmarkStart : (BookmarkStart) null);
        BookmarkEnd bookmarkEnd = paragraphItem is BookmarkEnd ? paragraphItem as BookmarkEnd : (this.Document.Bookmarks.FindByName((paragraphItem as BookmarkStart).Name) != null ? this.Document.Bookmarks.FindByName((paragraphItem as BookmarkStart).Name).BookmarkEnd : (BookmarkEnd) null);
        if (bookmarkStart != null && bookmarkEnd != null && this.GetOwnerEntity((Entity) bookmarkEnd) != this.GetOwnerEntity((Entity) bookmarkStart))
          bookmarks.Push((Entity) paragraphItem);
      }
    }
  }

  private Entity GetOwnerEntity(Entity entity)
  {
    Entity owner = entity.Owner;
    while (!(owner is WTable) && (!(owner is WParagraph) || !((owner as WParagraph).OwnerTextBody.Owner is WSection)) && owner.Owner != null)
      owner = owner.Owner;
    return owner;
  }

  private void DeleteTableRows(
    int startRowIndex,
    int endRowIndex,
    WTable bkmkTable,
    WParagraph paragraphEnd,
    BookmarkEnd bkmkEnd)
  {
    int inOwnerCollection = (paragraphEnd.GetOwnerEntity() as WTableCell).GetIndexInOwnerCollection();
    for (int index1 = startRowIndex; index1 <= endRowIndex; ++index1)
    {
      if (index1 == endRowIndex)
      {
        WTableRow row = bkmkTable.Rows[index1];
        for (int index2 = 0; index2 <= inOwnerCollection; ++index2)
        {
          if (index2 == inOwnerCollection)
            this.DeleteBookmarkEndRow(paragraphEnd, bkmkEnd, index2, row);
          else
            row.Cells[index2].ChildEntities.Clear();
        }
      }
      else
        bkmkTable.Rows[startRowIndex].RemoveSelf();
    }
  }

  private void DeleteBookmarkEndRow(
    WParagraph paragraphEnd,
    BookmarkEnd bkmkEnd,
    int j,
    WTableRow endRow)
  {
    int inOwnerCollection = paragraphEnd.GetIndexInOwnerCollection();
    for (int index1 = 0; index1 <= inOwnerCollection; ++index1)
    {
      if (index1 == inOwnerCollection)
      {
        int index2 = 0;
        while (index2 < bkmkEnd.GetIndexInOwnerCollection())
        {
          ParagraphItem childEntity = paragraphEnd.ChildEntities[index2] as ParagraphItem;
          switch (childEntity)
          {
            case BookmarkEnd _:
              Bookmark byName1 = this.Document.Bookmarks.FindByName((childEntity as BookmarkEnd).Name);
              if (byName1 != null && byName1.BookmarkStart != null && byName1.BookmarkStart.IsDetached)
              {
                childEntity.RemoveSelf();
                continue;
              }
              ++index2;
              continue;
            case BookmarkStart _:
              Bookmark byName2 = this.Document.Bookmarks.FindByName((childEntity as BookmarkStart).Name);
              WParagraph ownerParagraph = byName2.BookmarkEnd.OwnerParagraph;
              if (this.CheckBookmarkEnd(childEntity.OwnerParagraph, ownerParagraph, byName2, bkmkEnd.GetIndexInOwnerCollection()))
              {
                childEntity.RemoveSelf();
                continue;
              }
              ++index2;
              continue;
            default:
              childEntity.RemoveSelf();
              continue;
          }
        }
        bkmkEnd.RemoveSelf();
      }
      else
        endRow.Cells[j].ChildEntities.RemoveAt(index1);
    }
  }

  private bool CheckBookmarkEnd(
    WParagraph bkmkStartPara,
    WParagraph bkmkEndPara,
    Bookmark bkmk,
    int endIndex)
  {
    return bkmkStartPara == bkmkEndPara ? bkmk.BookmarkEnd.GetIndexInOwnerCollection() < endIndex : bkmkStartPara.OwnerTextBody == bkmkEndPara.OwnerTextBody && bkmkStartPara.Index < bkmkEndPara.Index;
  }

  private void DeleteBkmkContentInParagraphAftertable(
    WParagraph paragraphStart,
    WParagraph paragraphEnd,
    BookmarkStart bkmkStart,
    BookmarkEnd bkmkEnd,
    bool isReplaceContent)
  {
    int bkmkIndex = this.Document.Bookmarks.InnerList.IndexOf((object) this.CurrentBookmark);
    WTextBody ownerTextBody1 = paragraphStart.OwnerTextBody;
    WTableCell ownerEntity = paragraphEnd.GetOwnerEntity() as WTableCell;
    WTable owner = (WTable) ownerEntity.Owner.Owner;
    WTextBody ownerTextBody2 = owner.OwnerTextBody;
    int inOwnerCollection1 = owner.GetIndexInOwnerCollection();
    int startSectiontionIndex = this.GetSection((Entity) ownerTextBody1).GetIndexInOwnerCollection() + 1;
    int inOwnerCollection2 = this.GetSection((Entity) ownerTextBody2).GetIndexInOwnerCollection();
    bool isInSingleSection = false;
    if (startSectiontionIndex - 1 == inOwnerCollection2)
      isInSingleSection = true;
    int inOwnerCollection3 = ownerEntity.Owner.GetIndexInOwnerCollection();
    int inOwnerCollection4 = ownerEntity.GetIndexInOwnerCollection();
    if (inOwnerCollection4 == 0 && inOwnerCollection3 != 0 && ownerEntity.Paragraphs[0] == bkmkEnd.OwnerParagraph && this.IsBkmkEndInFirstItem(ownerEntity.Paragraphs[0], (ParagraphItem) bkmkEnd, bkmkEnd.GetIndexInOwnerCollection() - 1))
      --inOwnerCollection3;
    int inOwnerCollection5 = paragraphStart.GetIndexInOwnerCollection();
    int num = bkmkStart.GetIndexInOwnerCollection() + 1;
    if (inOwnerCollection1 >= 0 && inOwnerCollection3 == 0 && inOwnerCollection4 == 0 && ownerEntity.Paragraphs[0] == bkmkEnd.OwnerParagraph)
    {
      --inOwnerCollection1;
      if (ownerEntity.ChildEntities[0] is WParagraph && inOwnerCollection1 > 0)
      {
        (ownerTextBody2.Items[inOwnerCollection1] as WParagraph).Items.Add((IEntity) bkmkEnd);
        this.DeleteBookmarkTextBody(false, isReplaceContent);
        return;
      }
    }
    if (isReplaceContent && owner.Rows.Count - 1 != inOwnerCollection3)
      throw new InvalidOperationException("You cannot replace bookmark content when the bookmark starts before the table and ends in table partially");
    this.DeleteFirstSectionItemsFromDocument(inOwnerCollection5, ref inOwnerCollection1, ownerTextBody1, num, isInSingleSection);
    this.DeleteInBetweenSections(startSectiontionIndex, inOwnerCollection2);
    this.DeleteLastSectionItemsFromDocument(inOwnerCollection1 - 1, ownerTextBody2, true, isInSingleSection);
    if (inOwnerCollection1 < 0)
      return;
    this.DeleteTableRows(0, inOwnerCollection3, owner);
    if (owner.Rows.Count == 0)
    {
      bool flag = this.IsBkmkEndInFirstItem(paragraphStart, (ParagraphItem) bkmkStart, paragraphStart.Items.Count - 1);
      WParagraph wparagraph = paragraphStart;
      if (flag)
        wparagraph = this.GetParagraphToInsertBookmark(ownerTextBody1, ownerTextBody2, (WParagraph) null, inOwnerCollection1, bkmkStart, bkmkEnd, isInSingleSection, false);
      if (wparagraph == paragraphStart)
      {
        this.ReplaceCurrentBookmark(wparagraph, num - 1, num, bkmkIndex);
      }
      else
      {
        this.ReplaceCurrentBookmark(wparagraph, 0, 1, bkmkIndex);
        this.MoveNestedBookmark(paragraphStart, wparagraph);
        paragraphStart.RemoveSelf();
      }
      owner.RemoveSelf();
    }
    else
    {
      WParagraph toInsertBookmark = this.GetParagraphToInsertBookmark(ownerTextBody1, ownerTextBody2, paragraphStart, inOwnerCollection5, bkmkStart, bkmkEnd, isInSingleSection, true);
      if (toInsertBookmark == paragraphStart)
      {
        this.ReplaceCurrentBookmark(toInsertBookmark, num - 1, num, bkmkIndex);
      }
      else
      {
        this.ReplaceCurrentBookmark(toInsertBookmark, 0, 1, bkmkIndex);
        paragraphStart.RemoveSelf();
      }
    }
    if (owner.Rows.Count != 0 && owner.Rows[0].Cells.Count > 0 && owner.Rows[0].Cells[0].Paragraphs.Count > 0)
    {
      WParagraph paragraph = owner.Rows[0].Cells[0].Paragraphs[0];
      this.ReplaceCurrentBookmark(paragraph, 0, 1, bkmkIndex);
      this.MoveNestedBookmark(paragraphStart, paragraph);
      paragraphStart.RemoveSelf();
    }
    if (isInSingleSection)
      return;
    this.MergeMultiSectionBodyItems(ownerTextBody1, ownerTextBody2);
  }

  private WParagraph GetParagraphToInsertBookmark(
    WTextBody startTextBody,
    WTextBody endTextBody,
    WParagraph paragraph,
    int bkmkItemIndex,
    BookmarkStart bkmkStart,
    BookmarkEnd bkmkEnd,
    bool isInSingleSection,
    bool isParaAfterTable)
  {
    WTextBody wtextBody = isInSingleSection ? startTextBody : (isParaAfterTable ? startTextBody : endTextBody);
    if (paragraph != null && !this.IsBkmkEndInFirstItem(paragraph, (ParagraphItem) bkmkStart, paragraph.ChildEntities.Count - 1))
      return paragraph;
    if (wtextBody.ChildEntities.Count > bkmkItemIndex + 1)
    {
      if (wtextBody.ChildEntities[bkmkItemIndex + 1] is WParagraph)
        return wtextBody.ChildEntities[bkmkItemIndex + 1] as WParagraph;
      if (wtextBody.ChildEntities[bkmkItemIndex + 1] is WTable)
        return (wtextBody.ChildEntities[bkmkItemIndex + 1] as WTable).FirstRow.Cells[0].Paragraphs[0];
    }
    return (WParagraph) null;
  }

  private void DeleteTableBookmarkTextBody(
    WTable bkmkTable,
    int startRowIndex,
    int endRowIndex,
    int startCellIndex,
    int endCellIndex,
    BookmarkStart bkmkStart,
    BookmarkEnd bkmkEnd)
  {
    int bkmkIndex = this.Document.Bookmarks.InnerList.IndexOf((object) this.CurrentBookmark);
    Entity owner1 = bkmkStart.Owner;
    Entity owner2 = bkmkEnd.Owner;
    int columnFirst = (int) bkmkStart.ColumnFirst;
    int columnLast = (int) bkmkStart.ColumnLast;
    bool flag = bkmkStart.ColumnFirst == (short) -1 && bkmkStart.ColumnLast == (short) -1;
    for (int index1 = startRowIndex; index1 <= endRowIndex; ++index1)
    {
      WTableRow row = bkmkTable.Rows[index1];
      if (flag)
        endCellIndex = row.Cells.Count - 1;
      for (int index2 = startCellIndex; index2 <= endCellIndex && index2 <= row.Cells.Count - 1; ++index2)
      {
        WTableCell cell = bkmkTable.Rows[index1].Cells[index2];
        if (cell.ChildEntities.Count != 0)
        {
          WParagraph lastParagraph = cell.LastParagraph as WParagraph;
          lastParagraph.GetIndexInOwnerCollection();
          while (cell.Paragraphs.Count > 1)
          {
            WParagraph paragraph = cell.Paragraphs[0];
            this.DeleteParagraphItemsInCell(paragraph);
            while (paragraph.Items.Count != 0)
              lastParagraph.Items.Add((IEntity) paragraph.Items[0]);
            paragraph.RemoveSelf();
          }
          int count = cell.Tables.Count;
          while (cell.Tables.Count != 0)
            (cell.Tables[0] as WTable).RemoveSelf();
          this.DeleteParagraphItemsInCell(lastParagraph);
        }
      }
    }
    this.CreateBookmark(bkmkTable.Rows[startRowIndex].Cells[startCellIndex].LastParagraph, bkmkTable.Rows[endRowIndex].Cells[endCellIndex].LastParagraph, bkmkIndex, bkmkStart, columnFirst, columnLast);
    this.m_currBookmark.BookmarkEnd.IsAfterRowMark = true;
  }

  private void DeleteParagraphItemsInCell(WParagraph paragraph)
  {
    for (int index = paragraph.Items.Count - 1; index >= 0; --index)
    {
      if (!(paragraph.Items[index] is BookmarkStart) && !(paragraph.Items[index] is BookmarkEnd))
        paragraph.Items.RemoveAt(index);
    }
  }

  private void DeleteBookmarkTextBody(bool saveFormatting, bool isReplaceBkmkContent)
  {
    int bkmkIndex = this.Document.Bookmarks.InnerList.IndexOf((object) this.CurrentBookmark);
    BookmarkStart bookmarkStart = this.CurrentBookmark.BookmarkStart;
    BookmarkEnd bookmarkEnd = this.CurrentBookmark.BookmarkEnd;
    WParagraph ownerParagraph1 = bookmarkStart.OwnerParagraph;
    WParagraph ownerParagraph2 = bookmarkEnd.OwnerParagraph;
    WTextBody ownerTextBody1 = ownerParagraph1.OwnerTextBody;
    WTextBody ownerTextBody2 = ownerParagraph2.OwnerTextBody;
    int inOwnerCollection1 = ownerParagraph1.GetIndexInOwnerCollection();
    int bkmkStartNextItemIndex = bookmarkStart.GetIndexInOwnerCollection() + 1;
    int bkmkEndPreviosItemIndex = bookmarkEnd.GetIndexInOwnerCollection() - 1;
    bool isInSingleSection = false;
    int inOwnerCollection2;
    int startSectiontionIndex;
    if (ownerParagraph1.IsInCell && ownerParagraph2.IsInCell && ownerTextBody1 == ownerTextBody2)
    {
      isInSingleSection = true;
      startSectiontionIndex = inOwnerCollection2 = ownerTextBody1.GetIndexInOwnerCollection();
    }
    else
    {
      startSectiontionIndex = this.GetSection((Entity) ownerTextBody1).GetIndexInOwnerCollection() + 1;
      inOwnerCollection2 = this.GetSection((Entity) ownerTextBody2).GetIndexInOwnerCollection();
      if (startSectiontionIndex - 1 == inOwnerCollection2)
        isInSingleSection = true;
    }
    bool flag = this.IsBkmkEndInFirstItem(ownerParagraph1, (ParagraphItem) bookmarkStart, bkmkStartNextItemIndex);
    bool IsFirstBkmkEnd = this.IsBkmkEndInFirstItem(ownerParagraph2, (ParagraphItem) bookmarkEnd, bkmkEndPreviosItemIndex);
    int inOwnerCollection3 = ownerParagraph2.GetIndexInOwnerCollection();
    if (saveFormatting && ownerParagraph1.Items.Count > bkmkStartNextItemIndex)
    {
      int index = bkmkStartNextItemIndex;
      while (index < ownerParagraph1.Items.Count - 1 && !(ownerParagraph1.Items[index] is WTextRange))
        ++index;
      if (ownerParagraph1.Items[index] is WTextRange wtextRange1)
      {
        WTextRange wtextRange = new WTextRange(this.Document);
        wtextRange.CharacterFormat.ImportContainer((FormatBase) wtextRange1.CharacterFormat);
        if (wtextRange.CharacterFormat.PropertiesHash.ContainsKey(106))
          wtextRange.CharacterFormat.PropertiesHash.Remove(106);
        ownerParagraph1.ChildEntities.Insert(bkmkStartNextItemIndex, (IEntity) wtextRange);
        ++bkmkStartNextItemIndex;
      }
    }
    if (isInSingleSection && inOwnerCollection1 == inOwnerCollection3)
    {
      while (bkmkStartNextItemIndex < ownerParagraph1.ChildEntities.Count && (!(ownerParagraph1.ChildEntities[bkmkStartNextItemIndex] is BookmarkEnd) || !((ownerParagraph1.ChildEntities[bkmkStartNextItemIndex] as BookmarkEnd).Name == bookmarkEnd.Name)))
      {
        ParagraphItem paragraphItem = ownerParagraph2.Items[bkmkStartNextItemIndex];
        switch (paragraphItem)
        {
          case WField _:
          case WFieldMark _ when !(this.CurrentBookmark.Name.ToLower() == "_fieldbookmark"):
            this.CheckFieldWithinBookmark(ownerParagraph2, ref bkmkStartNextItemIndex);
            continue;
          case BookmarkStart _:
          case BookmarkEnd _:
            if (paragraphItem is BookmarkStart && (paragraphItem as BookmarkStart).Name == "_GoBack")
            {
              ownerParagraph2.Items.RemoveAt(bkmkStartNextItemIndex);
              continue;
            }
            ++bkmkStartNextItemIndex;
            continue;
          default:
            ownerParagraph2.Items.RemoveAt(bkmkStartNextItemIndex);
            continue;
        }
      }
      if (bookmarkEnd.IsAfterParagraphMark && (IsFirstBkmkEnd || flag))
      {
        WParagraph toInsertBookmark = this.GetParagraphToInsertBookmark(ownerTextBody1, ownerTextBody2, ownerParagraph1, inOwnerCollection1, bookmarkStart, bookmarkEnd, true, true);
        if (toInsertBookmark != null && toInsertBookmark != ownerParagraph1)
        {
          this.ReplaceCurrentBookmark(toInsertBookmark, 0, 1, bkmkIndex);
          this.MoveNestedBookmark(ownerParagraph1, toInsertBookmark);
          ownerParagraph1.RemoveSelf();
        }
      }
      List<Bookmark> bookmarkList = new List<Bookmark>();
      List<string> stringList1 = new List<string>();
      List<string> stringList2 = new List<string>();
      previousSibling = this.CurrentBookmark.BookmarkStart.PreviousSibling as Entity;
      nextSibling = this.CurrentBookmark.BookmarkEnd.NextSibling as Entity;
      if (previousSibling != null)
      {
        while (previousSibling.EntityType == EntityType.BookmarkStart)
        {
          if ((previousSibling as BookmarkStart).Name.IndexOf("_") != 0)
            stringList1.Add((previousSibling as BookmarkStart).Name);
          if (!(previousSibling.PreviousSibling is Entity previousSibling))
            break;
        }
      }
      if (nextSibling != null)
      {
        while (nextSibling.EntityType == EntityType.BookmarkEnd)
        {
          if ((nextSibling as BookmarkEnd).Name.IndexOf("_") != 0)
            stringList2.Add((nextSibling as BookmarkEnd).Name);
          if (!(nextSibling.NextSibling is Entity nextSibling))
            break;
        }
      }
      foreach (ParagraphItem paragraphItem in (CollectionImpl) ownerParagraph2.Items)
      {
        if (paragraphItem is BookmarkStart)
        {
          Bookmark byName = this.Document.Bookmarks.FindByName((paragraphItem as BookmarkStart).Name);
          if (byName.BookmarkEnd != null && this.CurrentBookmark.BookmarkStart != null && this.CurrentBookmark.BookmarkEnd != null)
          {
            if (byName.Name.IndexOf('_') != 0 && paragraphItem.Index > this.CurrentBookmark.BookmarkStart.Index && byName.BookmarkEnd.OwnerParagraph == this.CurrentBookmark.BookmarkEnd.OwnerParagraph && byName.BookmarkEnd.Index < this.CurrentBookmark.BookmarkEnd.Index)
              bookmarkList.Add(byName);
            else if (stringList1.Contains(byName.Name) && byName.BookmarkEnd.OwnerParagraph == this.CurrentBookmark.BookmarkEnd.OwnerParagraph && byName.BookmarkEnd.Index < this.CurrentBookmark.BookmarkEnd.Index)
              bookmarkList.Add(byName);
            else if (stringList2.Contains(byName.Name) && byName.BookmarkStart.Index > this.CurrentBookmark.BookmarkStart.Index)
              bookmarkList.Add(byName);
          }
        }
      }
      if (bookmarkList.Count == 0)
        return;
      foreach (Bookmark bookmark in bookmarkList)
        this.Document.Bookmarks.Remove(bookmark);
    }
    else
    {
      this.DeleteFirstSectionItemsFromDocument(inOwnerCollection1, ref inOwnerCollection3, ownerTextBody1, bkmkStartNextItemIndex, isInSingleSection);
      if (!IsFirstBkmkEnd || !isInSingleSection)
        this.DeleteLastSectionItemsFromDocument(inOwnerCollection3, ownerTextBody2, IsFirstBkmkEnd, isInSingleSection);
      if (IsFirstBkmkEnd)
        --inOwnerCollection3;
      bool isFirstItemBkmkEnd = inOwnerCollection3 < 0 && IsFirstBkmkEnd && !flag;
      if (!this.RemoveEmptyParagraph)
        return;
      this.SetCurrentBookmarkPosition(ownerParagraph1, ownerParagraph2, ownerTextBody1, ownerTextBody2, bookmarkStart, bookmarkEnd, inOwnerCollection3, isFirstItemBkmkEnd, isInSingleSection, isReplaceBkmkContent, bkmkIndex);
      if (isInSingleSection)
        return;
      this.DeleteInBetweenSections(startSectiontionIndex, inOwnerCollection2);
      if (isFirstItemBkmkEnd)
        return;
      this.MergeMultiSectionBodyItems(ownerTextBody1, ownerTextBody2);
    }
  }

  private void CheckFieldWithinBookmark(WParagraph paragraphEnd, ref int bkmkStartNextItemIndex)
  {
    int num1 = -1;
    int bkmkIndex = this.Document.Bookmarks.InnerList.IndexOf((object) this.CurrentBookmark);
    ParagraphItem paragraphItem = paragraphEnd.Items[bkmkStartNextItemIndex];
    bool flag = this.CurrentBookmark.BookmarkStart.GetIndexInOwnerCollection() < bkmkStartNextItemIndex;
    int index = -1;
    int num2 = -1;
    if (paragraphItem is WField)
    {
      index = bkmkStartNextItemIndex;
      num2 = (paragraphItem as WField).FieldEnd.GetIndexInOwnerCollection();
    }
    else if (paragraphItem is WFieldMark)
    {
      index = (paragraphItem as WFieldMark).ParentField.GetIndexInOwnerCollection();
      num2 = (paragraphItem as WFieldMark).Type == FieldMarkType.FieldEnd ? paragraphItem.GetIndexInOwnerCollection() : (paragraphItem as WFieldMark).ParentField.FieldEnd.GetIndexInOwnerCollection();
    }
    while (index < paragraphEnd.Items.Count && index <= num2)
    {
      if ((!(paragraphEnd.Items[index] is BookmarkEnd) || !((paragraphEnd.Items[index] as BookmarkEnd).Name == this.CurrentBookmark.Name)) && (!(paragraphEnd.Items[index] is BookmarkStart) || !((paragraphEnd.Items[index] as BookmarkStart).Name == this.CurrentBookmark.Name)))
      {
        int count = paragraphEnd.Items.Count;
        paragraphEnd.Items.RemoveAt(index);
        num2 -= count - paragraphEnd.Items.Count;
      }
      else
      {
        if (paragraphEnd.Items[index] is BookmarkEnd)
          num1 = index;
        ++index;
      }
    }
    if (this.CurrentBookmark.BookmarkEnd == null)
    {
      int inOwnerCollection = this.CurrentBookmark.BookmarkStart.GetIndexInOwnerCollection();
      this.CurrentBookmark.BookmarkStart.RemoveSelf();
      this.ReplaceCurrentBookmark(paragraphEnd, inOwnerCollection, flag ? bkmkStartNextItemIndex : inOwnerCollection + 1, bkmkIndex);
    }
    if (this.CurrentBookmark.BookmarkStart == null || this.CurrentBookmark.BookmarkStart.IsDetached)
    {
      int inOwnerCollection = this.CurrentBookmark.BookmarkEnd.GetIndexInOwnerCollection();
      this.CurrentBookmark.BookmarkEnd.RemoveSelf();
      this.ReplaceCurrentBookmark(paragraphEnd, inOwnerCollection, inOwnerCollection + 1, bkmkIndex);
      bkmkStartNextItemIndex = inOwnerCollection + 1;
    }
    if (num1 == -1)
      return;
    bkmkStartNextItemIndex = num1;
  }

  private void MergeMultiSectionBodyItems(WTextBody startTextBody, WTextBody endTextBody)
  {
    while (endTextBody.ChildEntities.Count != 0 && endTextBody.ChildEntities.Count > 0)
      startTextBody.ChildEntities.Add((IEntity) endTextBody.ChildEntities[0]);
    this.Document.Sections.RemoveAt((this.GetSection((Entity) endTextBody) as WSection).GetIndexInOwnerCollection());
  }

  private void DeleteFirstSectionItemsFromDocument(
    int startParagraphIndex,
    ref int endParagraphIndex,
    WTextBody startTextBody,
    int bkmkStartNextItemIndex,
    bool isInSingleSection)
  {
    int num1 = isInSingleSection ? endParagraphIndex : startTextBody.ChildEntities.Count;
    int num2 = startParagraphIndex;
    for (int index = startParagraphIndex; index < num1; ++index)
    {
      if (startParagraphIndex == index)
      {
        if (bkmkStartNextItemIndex > 0)
        {
          WParagraph childEntity = startTextBody.ChildEntities[index] as WParagraph;
          while (childEntity.ChildEntities.Count > bkmkStartNextItemIndex)
          {
            if (childEntity.Items[bkmkStartNextItemIndex] is BookmarkEnd)
            {
              BookmarkEnd bookmarkEnd = childEntity.Items[bkmkStartNextItemIndex] as BookmarkEnd;
              if (bookmarkEnd.Name != "_GoBack" && bookmarkEnd.Name != this.CurrentBookmark.Name)
              {
                Bookmark byName = this.Document.Bookmarks.FindByName(bookmarkEnd.Name);
                if (this.IsDeleteBkmk(byName, startParagraphIndex))
                {
                  childEntity.ChildEntities.RemoveAt(byName.BookmarkStart.Index);
                  --bkmkStartNextItemIndex;
                }
                else if (byName != null && byName.BookmarkStart != null && (!byName.BookmarkStart.IsDetached || num2 > this.GetOwnerEntity((Entity) byName.BookmarkStart).Index || endParagraphIndex < this.GetOwnerEntity((Entity) byName.BookmarkEnd).Index))
                {
                  ++bkmkStartNextItemIndex;
                  continue;
                }
              }
            }
            else if (childEntity.Items[bkmkStartNextItemIndex] is BookmarkStart)
            {
              BookmarkStart bookmarkStart = childEntity.Items[bkmkStartNextItemIndex] as BookmarkStart;
              if (bookmarkStart.Name != "_GoBack" && bookmarkStart.Name != this.CurrentBookmark.Name)
              {
                Bookmark byName = this.Document.Bookmarks.FindByName(bookmarkStart.Name);
                if (byName != null && byName.BookmarkStart != null && (num2 > this.GetOwnerEntity((Entity) byName.BookmarkStart).Index || endParagraphIndex < this.GetOwnerEntity((Entity) byName.BookmarkEnd).Index))
                {
                  ++bkmkStartNextItemIndex;
                  continue;
                }
              }
            }
            else if (!(this.CurrentBookmark.Name.ToLower() == "_fieldbookmark") && childEntity.Items[bkmkStartNextItemIndex] is WField && (childEntity.Items[bkmkStartNextItemIndex] as WField).FieldEnd.OwnerParagraph != null && childEntity != (childEntity.Items[bkmkStartNextItemIndex] as WField).FieldEnd.OwnerParagraph)
              throw new InvalidOperationException("Bookmark content not replaced properly while replacing with another bookmark content");
            childEntity.ChildEntities.RemoveAt(bkmkStartNextItemIndex);
          }
          ++startParagraphIndex;
          bkmkStartNextItemIndex = -1;
        }
        else
        {
          if (startTextBody.ChildEntities[startParagraphIndex] is WParagraph)
          {
            this.DeleteBkmkFromParagraph(startTextBody.ChildEntities[startParagraphIndex] as WParagraph, num2, endParagraphIndex);
            if ((startTextBody.ChildEntities[startParagraphIndex] as WParagraph).ChildEntities.Count == 0)
              startTextBody.ChildEntities.RemoveAt(startParagraphIndex);
            else
              ++startParagraphIndex;
          }
          else
          {
            Stack<Entity> bookmarks = new Stack<Entity>();
            this.UpdateBookmark(startTextBody.ChildEntities[startParagraphIndex] as TextBodyItem, bookmarks);
            if (bookmarks.Count != 0)
            {
              WParagraph wparagraph = new WParagraph(this.Document);
              int num3 = 0;
              while (bookmarks.Count > num3)
              {
                Entity entity = bookmarks.Pop();
                if (!this.IsBkMkInsideCurrBkMkRegion(entity, num2, endParagraphIndex))
                  wparagraph.Items.Add((IEntity) entity);
                else
                  ++num3;
              }
              if (wparagraph.ChildEntities.Count > 0)
              {
                startTextBody.ChildEntities.Insert(startParagraphIndex, (IEntity) wparagraph);
                ++startParagraphIndex;
              }
            }
            startTextBody.ChildEntities.RemoveAt(startParagraphIndex);
          }
          if (isInSingleSection)
            --endParagraphIndex;
        }
      }
      else
      {
        if (startTextBody.ChildEntities.Count > startParagraphIndex)
        {
          if (startTextBody.ChildEntities[startParagraphIndex] is WParagraph)
          {
            this.DeleteBkmkFromParagraph(startTextBody.ChildEntities[startParagraphIndex] as WParagraph, num2, endParagraphIndex);
            if ((startTextBody.ChildEntities[startParagraphIndex] as WParagraph).ChildEntities.Count == 0)
              startTextBody.ChildEntities.RemoveAt(startParagraphIndex);
            else
              ++startParagraphIndex;
          }
          else
          {
            Stack<Entity> bookmarks = new Stack<Entity>();
            this.UpdateBookmark(startTextBody.ChildEntities[startParagraphIndex] as TextBodyItem, bookmarks);
            if (bookmarks.Count != 0)
            {
              WParagraph wparagraph = new WParagraph(this.Document);
              int num4 = 0;
              while (bookmarks.Count > num4)
              {
                Entity entity = bookmarks.Pop();
                if (!this.IsBkMkInsideCurrBkMkRegion(entity, num2, endParagraphIndex))
                  wparagraph.Items.Add((IEntity) entity);
                else
                  ++num4;
              }
              if (wparagraph.ChildEntities.Count > 0)
              {
                startTextBody.ChildEntities.Insert(startParagraphIndex, (IEntity) wparagraph);
                ++startParagraphIndex;
              }
            }
            startTextBody.ChildEntities.RemoveAt(startParagraphIndex);
          }
        }
        if (isInSingleSection)
          --endParagraphIndex;
      }
    }
  }

  private bool IsDeleteBkmk(Bookmark bkmkStart, int startParagraphIndex)
  {
    bool flag = false;
    if (bkmkStart != null && bkmkStart.BookmarkStart != null && bkmkStart.BookmarkStart.OwnerParagraph.Index == startParagraphIndex)
    {
      for (int index = bkmkStart.BookmarkStart.Index + 1; index <= this.CurrentBookmark.BookmarkStart.Index; ++index)
      {
        if (bkmkStart.BookmarkStart.OwnerParagraph.Items[index] is BookmarkStart || bkmkStart.BookmarkStart.OwnerParagraph.Items[index] is BookmarkEnd)
        {
          flag = true;
        }
        else
        {
          flag = false;
          break;
        }
      }
    }
    return flag;
  }

  private bool IsBkMkInsideCurrBkMkRegion(Entity item, int startParaIndex, int endParaIndex)
  {
    bool flag = true;
    switch (item)
    {
      case BookmarkEnd _:
        BookmarkEnd bookmarkEnd = item as BookmarkEnd;
        if (bookmarkEnd.Name != "_GoBack" && bookmarkEnd.Name != this.CurrentBookmark.Name)
        {
          Bookmark byName = this.Document.Bookmarks.FindByName(bookmarkEnd.Name);
          if (byName != null && byName.BookmarkStart != null)
          {
            flag = startParaIndex <= this.GetOwnerEntity((Entity) byName.BookmarkStart).Index && endParaIndex >= this.GetOwnerEntity((Entity) byName.BookmarkEnd).Index;
            break;
          }
          break;
        }
        break;
      case BookmarkStart _:
        BookmarkStart bookmarkStart = item as BookmarkStart;
        if (bookmarkStart.Name != "_GoBack" && bookmarkStart.Name != this.CurrentBookmark.Name)
        {
          Bookmark byName = this.Document.Bookmarks.FindByName(bookmarkStart.Name);
          if (byName != null && byName.BookmarkStart != null)
          {
            flag = startParaIndex <= this.GetOwnerEntity((Entity) byName.BookmarkStart).Index && endParaIndex >= this.GetOwnerEntity((Entity) byName.BookmarkEnd).Index;
            break;
          }
          break;
        }
        break;
    }
    return flag;
  }

  private WParagraph DeleteBkmkFromParagraph(
    WParagraph paragraph,
    int startBkmkIndex,
    int endBkmkIndex)
  {
    int index = 0;
    while (paragraph.ChildEntities.Count > index)
    {
      if (this.IsBkMkInsideCurrBkMkRegion(paragraph.ChildEntities[index], startBkmkIndex, endBkmkIndex))
        paragraph.ChildEntities.RemoveAt(index);
      else
        ++index;
    }
    return paragraph;
  }

  private void DeleteLastSectionItemsFromDocument(
    int endParagraphIndex,
    WTextBody endTextBody,
    bool IsFirstBkmkEnd,
    bool isInSingleSection)
  {
    if (!isInSingleSection)
      this.DeleteFirstSectionItemsFromDocument(0, ref endParagraphIndex, endTextBody, 0, true);
    if (IsFirstBkmkEnd)
      return;
    this.DeletePreviousItemsInOwnerParagraphgrah(endTextBody.ChildEntities[endParagraphIndex] as WParagraph, this.CurrentBookmark.BookmarkEnd);
  }

  private Entity GetSection(Entity entity)
  {
    while (!(entity is WSection))
      entity = entity.Owner;
    return entity;
  }

  private bool IsBkmkEndInFirstItem(
    WParagraph paragraph,
    ParagraphItem bkmkEnd,
    int bkmkEndPreviosItemIndex)
  {
    for (int index = 0; index <= bkmkEndPreviosItemIndex && bkmkEndPreviosItemIndex < paragraph.ChildEntities.Count; ++index)
    {
      if ((!(paragraph.Items[index] is BookmarkStart) || (bkmkEnd is BookmarkEnd ? ((paragraph.Items[index] as BookmarkStart).Name != (bkmkEnd as BookmarkEnd).Name ? 1 : 0) : 1) == 0) && !(paragraph.Items[index] is BookmarkEnd))
        return false;
    }
    return true;
  }

  private bool IsBkmkEndFirstItemInTable(
    int bkmkEndRowIndex,
    WTable bkmkEndTable,
    WParagraph paragraphEnd,
    BookmarkEnd bkmkEnd)
  {
    return bkmkEndRowIndex <= 0 && bkmkEnd.GetIndexInOwnerCollection() <= 0 && (paragraphEnd.GetOwnerEntity() as WTableCell).GetIndexInOwnerCollection() <= 0;
  }

  private void SetCurrentBookmarkPosition(
    WParagraph paragraphStart,
    WParagraph paragraphEnd,
    WTextBody textBodyStart,
    WTextBody textBodyEnd,
    BookmarkStart bkmkStart,
    BookmarkEnd bkmkEnd,
    int startParagraphNextIndex,
    bool isFirstItemBkmkEnd,
    bool isInSingleSection,
    bool isReplaceOperation,
    int bkmkIndex)
  {
    bool flag1 = this.IsBkmkEndInFirstItem(paragraphStart, (ParagraphItem) bkmkStart, paragraphStart.ChildEntities.Count - 1);
    bool flag2 = this.IsBkmkEndInFirstItem(paragraphEnd, (ParagraphItem) bkmkEnd, paragraphEnd.ChildEntities.Count - 1);
    if (isFirstItemBkmkEnd && !flag1)
      return;
    if (!flag1 && !flag2)
    {
      if (paragraphEnd.PreviousSibling != paragraphStart || bkmkEnd.HasRenderableItemBefore())
        return;
      WParagraph toInsertBookmark = this.GetParagraphToInsertBookmark(textBodyStart, textBodyEnd, paragraphEnd, startParagraphNextIndex, bkmkStart, bkmkEnd, isInSingleSection, false);
      if (toInsertBookmark == null)
        return;
      int index = bkmkStart.Index + 1;
      WTextRange childEntity = paragraphStart.ChildEntities.Count > index ? paragraphStart.ChildEntities[index] as WTextRange : (WTextRange) null;
      int num = bkmkEnd.PreviousSibling is BookmarkStart || bkmkEnd.PreviousSibling is BookmarkEnd ? bkmkEnd.Index + 1 : 1;
      this.ReplaceCurrentBookmark(toInsertBookmark, 0, num, bkmkIndex);
      if (childEntity != null)
        toInsertBookmark.Items.Insert(num, (IEntity) childEntity);
      this.MoveNestedBookmark(paragraphStart, toInsertBookmark);
      paragraphStart.RemoveSelf();
      if (toInsertBookmark == paragraphEnd)
        return;
      paragraphEnd.RemoveSelf();
    }
    else if (flag1 && !flag2)
    {
      this.ReplaceCurrentBookmark(paragraphEnd, 0, 1, bkmkIndex);
      this.MoveNestedBookmark(paragraphStart, paragraphEnd);
      paragraphStart.RemoveSelf();
    }
    else if (!flag1 && flag2)
    {
      while (paragraphEnd.Items.Count != 0)
        paragraphStart.UpdateBookmarkEnd(paragraphEnd.Items[0], paragraphStart, true);
      paragraphEnd.RemoveSelf();
    }
    else
    {
      if (!flag1 || !flag2)
        return;
      if (!isReplaceOperation)
      {
        WParagraph toInsertBookmark = this.GetParagraphToInsertBookmark(textBodyStart, textBodyEnd, paragraphEnd, startParagraphNextIndex, bkmkStart, bkmkEnd, isInSingleSection, false);
        if (toInsertBookmark == null)
          return;
        this.ReplaceCurrentBookmark(toInsertBookmark, 0, 1, bkmkIndex);
        this.MoveNestedBookmark(paragraphStart, toInsertBookmark);
        paragraphStart.RemoveSelf();
        if (toInsertBookmark == paragraphEnd)
          return;
        paragraphEnd.RemoveSelf();
      }
      else
      {
        while (paragraphEnd.Items.Count != 0)
          paragraphStart.UpdateBookmarkEnd(paragraphEnd.Items[0], paragraphStart, true);
        paragraphEnd.RemoveSelf();
      }
    }
  }

  private void SetCurrentBookmarkPosition(
    WTable bkmkStartTable,
    WTable bkmkEndTable,
    WTextBody startTableTextBody,
    WTextBody endTableTextBody,
    int bkmkIndex)
  {
    if (bkmkEndTable.Rows.Count != 0 && bkmkEndTable.Rows[0].Cells.Count > 0)
    {
      WTableCell cell = bkmkEndTable.Rows[0].Cells[0];
      if (cell.ChildEntities.Count != 0)
        this.ReplaceCurrentBookmark(cell.Paragraphs[0], 0, 1, bkmkIndex);
      else
        this.ReplaceCurrentBookmark(cell.AddParagraph() as WParagraph, 0, 1, bkmkIndex);
    }
    else
    {
      WParagraph paragraph = startTableTextBody != endTableTextBody ? this.GetParagraphToInsertBookmark(startTableTextBody, endTableTextBody, (WParagraph) null, bkmkEndTable.GetIndexInOwnerCollection(), (BookmarkStart) null, (BookmarkEnd) null, false, false) : this.GetParagraphToInsertBookmark(startTableTextBody, endTableTextBody, (WParagraph) null, bkmkEndTable.GetIndexInOwnerCollection(), (BookmarkStart) null, (BookmarkEnd) null, true, false);
      if (paragraph == null)
        return;
      this.ReplaceCurrentBookmark(paragraph, 0, 1, bkmkIndex);
    }
  }

  private void DeletePreviousItemsInOwnerParagraphgrah(WParagraph paragraphEnd, BookmarkEnd bkmkEnd)
  {
    int index = 0;
    while (paragraphEnd.Items.Count > 0 && index < paragraphEnd.Items.Count && paragraphEnd.Items[index] != bkmkEnd)
    {
      if (!(paragraphEnd.Items[index] is BookmarkStart) && !(paragraphEnd.Items[index] is BookmarkEnd))
        paragraphEnd.Items.RemoveAt(index);
      else if (paragraphEnd.Items[index] is BookmarkStart && (paragraphEnd.Items[index] as BookmarkStart).Name == "_GoBack")
        paragraphEnd.Items.RemoveAt(index);
      else
        ++index;
    }
  }

  private void MoveNestedBookmark(WParagraph sourceParagraph, WParagraph destinationParagrah)
  {
    if (sourceParagraph.ChildEntities.Count == 0)
      return;
    while (sourceParagraph.Items.Count != 0)
      destinationParagrah.UpdateBookmarkEnd(sourceParagraph.LastItem, destinationParagrah, false);
  }

  private void ReplaceCurrentBookmark(
    WParagraph paragraph,
    int bkmkStartIndex,
    int bkmkEndIndex,
    int bkmkIndex)
  {
    string name = this.CurrentBookmark.Name;
    if (this.Document.Bookmarks.FindByName(name) != null)
      this.Document.Bookmarks.Remove(this.CurrentBookmark);
    BookmarkStart start = new BookmarkStart(this.Document, name);
    paragraph.Items.Insert(bkmkStartIndex, (IEntity) start);
    BookmarkEnd end = new BookmarkEnd(this.Document, name);
    paragraph.Items.Insert(bkmkEndIndex, (IEntity) end);
    this.CurrentBookmark.SetStart(start);
    this.CurrentBookmark.SetEnd(end);
    if (bkmkIndex < this.Document.Bookmarks.Count)
    {
      this.Document.Bookmarks.InnerList.Insert(bkmkIndex, (object) this.CurrentBookmark);
      this.Document.Bookmarks.InnerList.RemoveAt(this.Document.Bookmarks.Count - 1);
    }
    this.m_currBookmark = this.Document.Bookmarks.FindByName(name);
  }

  private void ReplaceTableBookmarkContent(WordDocumentPart documentPart, TextBodyPart textPart)
  {
    if (documentPart != null && documentPart.Sections.Count > 1)
      throw new InvalidOperationException("You cannot replace bookmark content with multiple sections when bookmark starts and ends within the same table");
    if ((documentPart == null || documentPart.Sections.Count != 1) && textPart == null)
      return;
    WTableCell ownerEntity1 = this.CurrentBookmark.BookmarkStart.OwnerParagraph.GetOwnerEntity() as WTableCell;
    WTableCell ownerEntity2 = this.CurrentBookmark.BookmarkEnd.OwnerParagraph.GetOwnerEntity() as WTableCell;
    WTable ownerTable = ownerEntity1.OwnerRow.OwnerTable;
    int inOwnerCollection1 = ownerEntity1.GetIndexInOwnerCollection();
    int num = ownerEntity2.GetIndexInOwnerCollection();
    int inOwnerCollection2 = ownerEntity1.OwnerRow.GetIndexInOwnerCollection();
    int inOwnerCollection3 = ownerEntity2.OwnerRow.GetIndexInOwnerCollection();
    if (inOwnerCollection2 == inOwnerCollection3 && inOwnerCollection1 == num)
    {
      if (documentPart != null)
      {
        this.ReplaceParagraphBookmarkContent(documentPart);
      }
      else
      {
        if (textPart == null)
          return;
        this.InsertTextBodyPart(textPart);
      }
    }
    else
    {
      WTextBody wtextBody = (WTextBody) null;
      if (documentPart != null)
        wtextBody = documentPart.Sections[0].Body;
      else if (textPart != null)
        wtextBody = textPart.m_textPart;
      bool flag = this.CurrentBookmark.BookmarkStart.ColumnFirst == (short) -1 && this.CurrentBookmark.BookmarkStart.ColumnLast == (short) -1;
      for (int index1 = inOwnerCollection2; index1 <= inOwnerCollection3; ++index1)
      {
        if (flag)
          num = ownerTable.Rows[index1].Cells.Count - 1;
        for (int index2 = inOwnerCollection1; index2 <= num; ++index2)
        {
          ownerTable.Rows[index1].Cells[index2].Items.Clear();
          wtextBody.Items.CloneTo((EntityCollection) ownerTable.Rows[index1].Cells[index2].Items);
          if (index1 == inOwnerCollection2 && index2 == inOwnerCollection1)
          {
            if (ownerTable.Rows[index1].Cells[index2].Paragraphs.Count != 0)
              ownerTable.Rows[index1].Cells[index2].Paragraphs[0].Items.Insert(0, (IEntity) this.CurrentBookmark.BookmarkStart);
            else
              ownerTable.Rows[index1].Cells[index2].AddParagraph().Items.Add((IEntity) this.CurrentBookmark.BookmarkStart);
          }
          if (index1 == inOwnerCollection3 && index2 == num)
          {
            if (ownerTable.Rows[index1].Cells[index2].Paragraphs.Count != 0)
              ownerTable.Rows[index1].Cells[index2].LastParagraph.Items.Add((IEntity) this.CurrentBookmark.BookmarkEnd);
            else
              ownerTable.Rows[index1].Cells[index2].AddParagraph().Items.Add((IEntity) this.CurrentBookmark.BookmarkEnd);
          }
        }
      }
    }
  }

  private void CheckCurrentState()
  {
    if (this.m_document == null)
      throw new InvalidOperationException("You can not use DocumentNavigator without initializing Document property");
    if (this.m_currBookmark == null || this.m_currParagraph == null || this.m_currParagraphItemIndex < 0)
      throw new InvalidOperationException("Current Bookmark didn't select");
  }

  private IWTextRange InsertText(string text, bool saveFormatting, bool isReplaceContent)
  {
    this.CheckCurrentState();
    IWTextRange wtextRange = (IWTextRange) null;
    if (saveFormatting)
      wtextRange = !this.IsStart ? this.m_currBookmark.BookmarkEnd.PreviousSibling as IWTextRange : this.m_currBookmark.BookmarkStart.PreviousSibling as IWTextRange;
    IWTextRange textRange = this.InsertParagraphItem(ParagraphItemType.TextRange) as IWTextRange;
    textRange.Text = text;
    if (saveFormatting)
    {
      if (wtextRange != null)
      {
        WCharacterFormat characterFormat = wtextRange.CharacterFormat;
        textRange.CharacterFormat.ImportContainer((FormatBase) characterFormat);
        if (isReplaceContent)
          textRange.OwnerParagraph.ChildEntities.Remove((IEntity) wtextRange);
      }
      else
        this.ApplyParagraphFormatting(textRange);
    }
    return textRange;
  }

  private void ApplyParagraphFormatting(IWTextRange textRange)
  {
    if (!(this.m_currBookmark.BookmarkStart.Owner is WParagraph owner))
      return;
    textRange.CharacterFormat.ImportContainer((FormatBase) owner.BreakCharacterFormat);
  }

  private void InsertBodyItem(TextBodyItem item)
  {
    if (this.CurrentBookmarkItem == null)
      return;
    WParagraph ownerParagraph = this.m_currBookmarkItem.OwnerParagraph;
    int inOwnerCollection = ownerParagraph.GetIndexInOwnerCollection();
    WParagraph wparagraph1 = new WParagraph((IWordDocument) ownerParagraph.Document);
    if (this.CurrentParagraphItemIndex != 0)
    {
      if (this.m_currParagraphItemIndex < ownerParagraph.Items.Count)
        TextBodyPart.SplitParagraph(ownerParagraph, this.m_currParagraphItemIndex, new WParagraph((IWordDocument) ownerParagraph.Document));
      else if (item is WTable && ownerParagraph.NextSibling is WParagraph || item is WParagraph)
      {
        ownerParagraph.OwnerTextBody.Items.Insert(inOwnerCollection + 1, (IEntity) wparagraph1);
        wparagraph1.BreakCharacterFormat.ImportContainer((FormatBase) ownerParagraph.BreakCharacterFormat);
        wparagraph1.ParagraphFormat.ImportContainer((FormatBase) ownerParagraph.ParagraphFormat);
      }
      ++inOwnerCollection;
      if (item is WParagraph)
      {
        WParagraph wparagraph2 = item as WParagraph;
        while (wparagraph2.Items.Count > 0)
          ownerParagraph.Items.Add((IEntity) wparagraph2.Items[0]);
        ownerParagraph.ParagraphFormat.ImportContainer((FormatBase) wparagraph2.ParagraphFormat);
        ownerParagraph.BreakCharacterFormat.ImportContainer((FormatBase) wparagraph2.BreakCharacterFormat);
        ownerParagraph.ListFormat.ImportContainer((FormatBase) wparagraph2.ListFormat);
      }
    }
    if (!(item is WTable) && this.m_currParagraphItemIndex > 0)
      return;
    ownerParagraph.OwnerTextBody.Items.Insert(inOwnerCollection, (IEntity) item);
  }
}
