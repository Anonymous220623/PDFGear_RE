// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.TextBodyPart
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class TextBodyPart
{
  internal WTextBody m_textPart;
  private WTextBody m_body;
  private int m_itemIndex;
  private int m_pItemIndex;
  private WCharacterFormat m_srcFormat;
  private bool m_saveFormatting;

  public BodyItemCollection BodyItems
  {
    get => this.m_textPart == null ? (BodyItemCollection) null : this.m_textPart.Items;
  }

  public TextBodyPart()
  {
  }

  public TextBodyPart(TextBodySelection textBodySelection) => this.Copy(textBodySelection);

  public TextBodyPart(TextSelection textSelection) => this.Copy(textSelection);

  public TextBodyPart(WordDocument doc) => this.EnsureTextBody(doc);

  public void Clear() => this.m_textPart.Items.Clear();

  public void Copy(TextSelection textSel)
  {
    this.EnsureTextBody((textSel.OwnerParagraph != null || !(textSel.StartTextRange.Owner is InlineContentControl) && !(textSel.StartTextRange.Owner is Break) ? (OwnerHolder) textSel.OwnerParagraph : (OwnerHolder) textSel.StartTextRange.GetOwnerParagraphValue()).Document);
    WTextRange[] ranges = textSel.GetRanges();
    WParagraph wparagraph = new WParagraph((IWordDocument) this.m_textPart.Document);
    this.m_textPart.Items.Add((IEntity) wparagraph);
    int index = 0;
    for (int length = ranges.Length; index < length; ++index)
      wparagraph.Items.Add((IEntity) ranges[index].Clone());
  }

  private void Copy(TextBodySelection textSel, bool isFindField)
  {
    this.EnsureTextBody(textSel.TextBody.Document);
    int itemStartIndex = textSel.ItemStartIndex;
    int itemEndIndex = textSel.ItemEndIndex;
    for (int index1 = itemStartIndex; index1 <= itemEndIndex; ++index1)
    {
      TextBodyItem textBodyItem = (TextBodyItem) textSel.TextBody.Items[index1].Clone();
      if ((index1 == itemStartIndex || index1 == itemEndIndex) && textBodyItem.EntityType == EntityType.Paragraph)
      {
        WParagraph wparagraph = textBodyItem as WParagraph;
        if (index1 == itemEndIndex)
        {
          int index2 = textSel.ParagraphItemEndIndex + 1;
          if (isFindField)
          {
            for (int paragraphItemStartIndex = itemStartIndex == itemEndIndex ? textSel.ParagraphItemStartIndex : 0; paragraphItemStartIndex < index2; ++paragraphItemStartIndex)
            {
              ParagraphItem paragraphItem = wparagraph.Items[paragraphItemStartIndex];
              if (paragraphItem is WField)
              {
                WField wfield = paragraphItem as WField;
                if (wfield.FieldEnd.Index > index2 && wfield.FieldEnd.OwnerParagraph.Equals((object) wparagraph))
                {
                  wparagraph.Items.RemoveFromInnerList(index2);
                  index2 = wfield.FieldEnd.Index + 1;
                  break;
                }
              }
            }
          }
          while (index2 < wparagraph.Items.Count)
            wparagraph.Items.RemoveFromInnerList(wparagraph.Items.Count - 1);
        }
        if (index1 == itemStartIndex)
        {
          int num1 = textSel.ParagraphItemStartIndex;
          if (isFindField)
          {
            int num2 = itemStartIndex == itemEndIndex ? textSel.ParagraphItemEndIndex + 1 : wparagraph.Items.Count;
            for (int index3 = num1; index3 < num2; ++index3)
            {
              ParagraphItem paragraphItem = wparagraph.Items[index3];
              WField wfield = paragraphItem is WField ? paragraphItem as WField : (WField) null;
              if (paragraphItem is WFieldMark && (paragraphItem as WFieldMark).Type == FieldMarkType.FieldEnd && (paragraphItem as WFieldMark).ParentField != null && (paragraphItem as WFieldMark).ParentField != wfield && (paragraphItem as WFieldMark).ParentField.Index < num1 - 1 && (paragraphItem as WFieldMark).ParentField.OwnerParagraph.Equals((object) wparagraph))
              {
                wparagraph.Items.RemoveFromInnerList(num1 - 1);
                num1 = (paragraphItem as WFieldMark).ParentField.Index;
                break;
              }
            }
          }
          for (; num1 > 0; --num1)
            wparagraph.Items.RemoveFromInnerList(0);
        }
      }
      this.m_textPart.Items.Add((IEntity) textBodyItem);
    }
  }

  public void Copy(TextBodySelection textSel) => this.Copy(textSel, false);

  public void Copy(TextBodyItem bodyItem, bool clone)
  {
    if (clone)
      bodyItem = (TextBodyItem) bodyItem.Clone();
    this.EnsureTextBody(bodyItem.Document);
    this.m_textPart.Items.Add((IEntity) bodyItem);
  }

  public void Copy(ParagraphItem pItem, bool clone)
  {
    if (clone)
      pItem = (ParagraphItem) pItem.Clone();
    this.EnsureTextBody(pItem.Document);
    this.m_textPart.AddParagraph().Items.Add((IEntity) pItem);
  }

  public WordDocument GetAsWordDocument()
  {
    WordDocument asWordDocument = new WordDocument();
    IWSection wsection = asWordDocument.AddSection();
    foreach (TextBodyItem textBodyItem in (CollectionImpl) this.m_textPart.Items)
      wsection.Body.Items.Add((IEntity) textBodyItem.Clone());
    return asWordDocument;
  }

  public void Close()
  {
    if (this.m_textPart.Items == null || this.m_textPart.Items.Count <= 0)
      return;
    TextBodyItem textBodyItem = (TextBodyItem) null;
    for (int index = 0; index < this.m_textPart.Items.Count; ++index)
    {
      this.m_textPart.Items[index].Close();
      textBodyItem = (TextBodyItem) null;
    }
    this.m_textPart.Items.Clear();
    this.m_textPart = (WTextBody) null;
  }

  internal void Copy(WTextBody textBody, bool clone)
  {
    this.EnsureTextBody(textBody.Document);
    if (clone)
      this.m_textPart = (WTextBody) textBody.Clone();
    else
      this.m_textPart = textBody;
  }

  public void PasteAfter(TextBodyItem bodyItem)
  {
    int inOwnerCollection = bodyItem.GetIndexInOwnerCollection();
    this.PasteAt((ITextBody) bodyItem.OwnerTextBody, inOwnerCollection + 1);
  }

  public void PasteAfter(ParagraphItem paragraphItem)
  {
    TextBodyItem owner = paragraphItem.Owner as TextBodyItem;
    int inOwnerCollection1 = owner.GetIndexInOwnerCollection();
    int inOwnerCollection2 = paragraphItem.GetIndexInOwnerCollection();
    this.PasteAt((ITextBody) owner.OwnerTextBody, inOwnerCollection1, inOwnerCollection2 + 1);
  }

  public void PasteAt(ITextBody textBody, int itemIndex) => this.PasteAt(textBody, itemIndex, 0);

  internal void PasteAt(
    ITextBody textBody,
    int itemIndex,
    int pItemIndex,
    WCharacterFormat srcFormat,
    bool saveFormatting)
  {
    bool flag = false;
    if (saveFormatting && textBody != null && textBody.Document != null)
    {
      flag = textBody.Document.ImportStyles;
      textBody.Document.ImportStyles = false;
    }
    this.m_srcFormat = srcFormat;
    this.m_saveFormatting = saveFormatting;
    this.PasteAt(textBody, itemIndex, pItemIndex);
    if (!saveFormatting || textBody == null || textBody.Document == null)
      return;
    textBody.Document.ImportStyles = flag;
  }

  public void PasteAt(ITextBody textBody, int itemIndex, int pItemIndex)
  {
    this.PasteAt(textBody, itemIndex, pItemIndex, false);
  }

  public void PasteAtEnd(ITextBody textBody)
  {
    this.PasteAt(textBody, ((WTextBody) textBody).Items.Count);
  }

  internal void PasteAt(ITextBody textBody, int itemIndex, int pItemIndex, bool isBkmkReplace)
  {
    if (this.m_textPart.Items.Count == 0)
      return;
    this.m_body = textBody as WTextBody;
    this.m_itemIndex = itemIndex;
    this.m_pItemIndex = pItemIndex;
    this.ValidateArgs();
    WParagraph srcParagraph = this.m_textPart.Items[0] as WParagraph;
    WParagraph srcLastParagraph = this.m_textPart.Items[this.m_textPart.Count - 1] as WParagraph;
    WParagraph wparagraph1 = itemIndex < this.m_body.Items.Count ? this.m_body.Items[itemIndex] as WParagraph : (WParagraph) null;
    string name1 = (string) null;
    if (isBkmkReplace && wparagraph1 != null && wparagraph1.ChildEntities.Count > pItemIndex && wparagraph1.ChildEntities[pItemIndex] is BookmarkEnd)
      name1 = (wparagraph1.ChildEntities[pItemIndex] as BookmarkEnd).Name;
    WParagraph wparagraph2 = (WParagraph) null;
    int num1 = 0;
    int num2 = 0;
    int num3 = this.m_textPart.Items.Count - 1;
    bool flag = num3 > 0 || srcParagraph == null;
    int count1 = this.m_body.Document.ClonedFields.Count;
    if (flag && wparagraph1 != null && this.m_pItemIndex >= 0)
    {
      wparagraph2 = this.SplitParagraph(wparagraph1, srcLastParagraph);
      if (wparagraph1.Items.Count > 0)
        num1 = 1;
      if (srcLastParagraph != null)
        --num3;
    }
    if (srcParagraph != null && wparagraph1 != null && this.m_saveFormatting)
    {
      int num4 = wparagraph1.Items.Count - this.m_pItemIndex;
      int index1 = wparagraph1.Items.Count - num4;
      int index2 = 0;
      for (int count2 = srcParagraph.Items.Count; index2 < count2; ++index2)
      {
        Entity entity = srcParagraph.Items[index2].Clone();
        wparagraph1.Items.Insert(index1, (IEntity) entity);
        wparagraph1.ListFormat.ImportListFormat(srcParagraph.ListFormat);
        if (entity is WTextRange && this.m_saveFormatting && this.m_srcFormat != null)
          this.m_srcFormat.UpdateSourceFormatting((entity as WTextRange).CharacterFormat);
        index1 = wparagraph1.Items.Count - num4;
      }
      num2 = 1;
      num1 = 1;
    }
    else if (srcParagraph != null && wparagraph1 != null && !this.m_saveFormatting)
    {
      wparagraph1.ImportStyle(srcParagraph.ParaStyle);
      srcParagraph.ParagraphFormat.UpdateSourceFormatting(wparagraph1.ParagraphFormat);
      wparagraph1.ListFormat.ImportListFormat(srcParagraph.ListFormat);
      srcParagraph.BreakCharacterFormat.UpdateSourceFormatting(wparagraph1.BreakCharacterFormat);
      this.CopyParagraphItems(srcParagraph, wparagraph1);
      num2 = 1;
      num1 = 1;
    }
    itemIndex += num1 - num2;
    int index3 = num2;
    for (int index4 = num3; index3 <= index4; ++index3)
    {
      Entity entity = this.m_textPart.Items[index3].Clone();
      this.m_body.Items.Insert(itemIndex + index3, (IEntity) entity);
      switch (entity)
      {
        case WParagraph _:
          (entity as WParagraph).ApplyStyle((entity as WParagraph).StyleName, false);
          if (this.m_saveFormatting)
          {
            this.ApplySrcFormat(entity as WParagraph);
            break;
          }
          this.UpdateFormatting(entity as WParagraph, this.m_textPart.Items[index3] as WParagraph);
          break;
        case WTable _:
          this.ApplySrcFormat(entity as WTable, this.m_textPart.Items[index3] as WTable);
          break;
      }
    }
    if (this.m_body.Document.ClonedFields.Count > count1 && wparagraph2 != null)
    {
      for (int index5 = 0; index5 < wparagraph2.ChildEntities.Count && this.m_body.Document.ClonedFields.Count > 0; ++index5)
      {
        if (wparagraph2.ChildEntities[index5] is WFieldMark && (wparagraph2.ChildEntities[index5] as WFieldMark).Type == FieldMarkType.FieldEnd)
          this.m_body.Document.ClonedFields.Pop().FieldEnd = wparagraph2.ChildEntities[index5] as WFieldMark;
      }
    }
    if (itemIndex > 0 && this.m_body.Items[itemIndex - 1] is WParagraph && (this.m_body.Items[itemIndex - 1] as WParagraph).Items.Count == 1 && (this.m_body.Items[itemIndex - 1] as WParagraph).Items[0] is BookmarkStart && this.m_textPart.Items[0] is WTable)
    {
      WParagraph wparagraph3 = this.m_body.Items[itemIndex - 1] as WParagraph;
      WTable wtable = this.m_body.Items[itemIndex] as WTable;
      string name2 = (wparagraph3.Items[0].Clone() as BookmarkStart).Name;
      WordDocument document = this.m_body.Document;
      document.Bookmarks.Remove(document.Bookmarks[name2]);
      wparagraph3.RemoveSelf();
      if (wtable.FirstRow != null && wtable.FirstRow.Cells.Count > 0)
      {
        if (wtable.FirstRow.Cells[0].Items.Count == 0)
          wtable.FirstRow.Cells[0].Items.Add((IEntity) new WParagraph((IWordDocument) document));
        if (wtable.FirstRow.Cells[0].Items[0] is WParagraph)
          (wtable.FirstRow.Cells[0].Items[0] as WParagraph).Items.Insert(0, (IEntity) new BookmarkStart((IWordDocument) document, name2));
      }
      if (this.m_textPart.Items.Count == 1)
      {
        (this.m_body.Items[itemIndex] as WParagraph).Items.Insert(0, (IEntity) new BookmarkEnd((IWordDocument) document, name2));
      }
      else
      {
        if (!(this.m_body.Items[itemIndex + this.m_textPart.Items.Count - 2] is WParagraph wparagraph4))
        {
          wparagraph4 = new WParagraph((IWordDocument) document);
          this.m_body.Items.Add((IEntity) wparagraph4);
        }
        wparagraph4.Items.Add((IEntity) new BookmarkEnd((IWordDocument) document, name2));
      }
    }
    else
    {
      if (!isBkmkReplace || name1 == null)
        return;
      Bookmark byName = this.m_body.Document.Bookmarks.FindByName(name1);
      if (byName.BookmarkEnd != null)
        return;
      WParagraph wparagraph5 = (WParagraph) null;
      if (wparagraph1 != null && wparagraph2 == null)
        wparagraph5 = wparagraph1;
      else if (wparagraph2 != null)
        wparagraph5 = wparagraph2;
      if (wparagraph5 == null)
        return;
      for (int index6 = 0; index6 < wparagraph5.ChildEntities.Count; ++index6)
      {
        if (wparagraph5.ChildEntities[index6] is BookmarkEnd && (wparagraph5.ChildEntities[index6] as BookmarkEnd).Name == name1)
        {
          byName.SetEnd(wparagraph5.ChildEntities[index6] as BookmarkEnd);
          break;
        }
      }
    }
  }

  internal void PasteAt(
    InlineContentControl inlineContentControl,
    int index,
    WCharacterFormat sourceFormat,
    bool saveFormatting)
  {
    this.m_body = inlineContentControl.OwnerParagraph.OwnerTextBody;
    this.m_pItemIndex = index;
    this.m_srcFormat = sourceFormat;
    this.m_saveFormatting = saveFormatting;
    this.PasteAt(inlineContentControl, this.m_textPart);
  }

  private void PasteAt(InlineContentControl inlineContentControl, WTextBody textBody)
  {
    if (textBody.Items.Count == 0)
      return;
    for (int index = 0; index < textBody.Items.Count; ++index)
    {
      if (textBody.Items[index] is WParagraph)
      {
        this.PasteParagraphAtInlineContentControl(inlineContentControl, textBody.Items[index] as WParagraph);
        if (inlineContentControl.ContentControlProperties.Type == ContentControlType.Text && inlineContentControl.ContentControlProperties.Multiline && index < textBody.Items.Count - 1)
        {
          Break @break = new Break((IWordDocument) this.m_body.Document, BreakType.LineBreak);
          inlineContentControl.ParagraphItems.Insert(this.m_pItemIndex, (IEntity) @break);
          ++this.m_pItemIndex;
        }
      }
      else if (textBody.Items[index] is WTable)
        this.PasteTableAtInlineContentControl(inlineContentControl, textBody.Items[index] as WTable);
      else if (textBody.Items[index] is BlockContentControl)
      {
        BlockContentControl blockContentControl = textBody.Items[index] as BlockContentControl;
        if (inlineContentControl.ContentControlProperties.Type == ContentControlType.RichText)
        {
          InlineContentControl inlineContentControl1 = new InlineContentControl(this.m_body.Document, ContentControlType.RichText);
          inlineContentControl.ParagraphItems.Insert(this.m_pItemIndex, (IEntity) inlineContentControl1);
          this.m_pItemIndex = 0;
          this.PasteAt(inlineContentControl1, blockContentControl.TextBody);
          this.m_pItemIndex = inlineContentControl1.Index;
        }
        else if (inlineContentControl.ContentControlProperties.Type == ContentControlType.Text)
          this.PasteAt(inlineContentControl, blockContentControl.TextBody);
      }
    }
  }

  private void PasteParagraphAtInlineContentControl(
    InlineContentControl inlineContentControl,
    WParagraph paragraph)
  {
    for (int index = 0; index < paragraph.Items.Count; ++index)
    {
      ParagraphItem paragraphItem1 = paragraph.Items[index];
      switch (paragraphItem1)
      {
        case WTextRange _:
          this.PasteTextRangeAtInlineContentControl(inlineContentControl, paragraphItem1 as WTextRange);
          break;
        case InlineContentControl _ when inlineContentControl.ContentControlProperties.Type == ContentControlType.Text:
          this.PasteContentControlAsPlainText(inlineContentControl, paragraphItem1 as InlineContentControl);
          break;
        case Break _ when (paragraphItem1 as Break).BreakType == BreakType.LineBreak && (inlineContentControl.ContentControlProperties.Type == ContentControlType.RichText || inlineContentControl.ContentControlProperties.Type == ContentControlType.Text && inlineContentControl.ContentControlProperties.Multiline):
          ParagraphItem paragraphItem2 = paragraphItem1.Clone() as ParagraphItem;
          inlineContentControl.ParagraphItems.Insert(this.m_pItemIndex, (IEntity) paragraphItem2);
          ++this.m_pItemIndex;
          break;
        default:
          if (inlineContentControl.ContentControlProperties.Type == ContentControlType.RichText)
          {
            ParagraphItem paragraphItem3 = paragraphItem1.Clone() as ParagraphItem;
            inlineContentControl.ParagraphItems.Insert(this.m_pItemIndex, (IEntity) paragraphItem3);
            ++this.m_pItemIndex;
            break;
          }
          break;
      }
    }
    if (paragraph.Items.Count != 0 || inlineContentControl.ContentControlProperties.Type != ContentControlType.RichText)
      return;
    this.PasteTextRangeAtInlineContentControl(inlineContentControl, new WTextRange((IWordDocument) this.m_body.Document)
    {
      Text = " "
    });
  }

  private void PasteContentControlAsPlainText(
    InlineContentControl sourceContentControl,
    InlineContentControl nestedContentControl)
  {
    foreach (ParagraphItem paragraphItem in (CollectionImpl) nestedContentControl.ParagraphItems)
    {
      if (paragraphItem is WTextRange)
        this.PasteTextRangeAtInlineContentControl(sourceContentControl, paragraphItem as WTextRange);
      else if (paragraphItem is InlineContentControl)
        this.PasteContentControlAsPlainText(sourceContentControl, paragraphItem as InlineContentControl);
    }
  }

  private void PasteTableAtInlineContentControl(
    InlineContentControl inlineContentControl,
    WTable table)
  {
    foreach (WTableRow row in (CollectionImpl) table.Rows)
    {
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
      {
        for (int index = 0; index < cell.Items.Count; ++index)
        {
          if (cell.Items[index] is WParagraph)
            this.PasteParagraphAtInlineContentControl(inlineContentControl, cell.Items[index] as WParagraph);
          else if (cell.Items[index] is WTable)
            this.PasteTableAtInlineContentControl(inlineContentControl, cell.Items[index] as WTable);
        }
        if (inlineContentControl.ContentControlProperties.Type == ContentControlType.Text && cell.Index < row.Cells.Count - 1)
          this.PasteTextRangeAtInlineContentControl(inlineContentControl, new WTextRange((IWordDocument) this.m_body.Document)
          {
            Text = '\t'.ToString()
          });
      }
      if (inlineContentControl.ContentControlProperties.Type == ContentControlType.Text && inlineContentControl.ContentControlProperties.Multiline && row.Index < table.Rows.Count - 1)
      {
        Break @break = new Break((IWordDocument) this.m_body.Document, BreakType.LineBreak);
        inlineContentControl.ParagraphItems.Insert(this.m_pItemIndex, (IEntity) @break);
        ++this.m_pItemIndex;
      }
    }
  }

  private void PasteTextRangeAtInlineContentControl(
    InlineContentControl inlineContentControl,
    WTextRange textRange)
  {
    ParagraphItem paragraphItem = textRange.Clone() as ParagraphItem;
    inlineContentControl.ParagraphItems.Insert(this.m_pItemIndex, (IEntity) paragraphItem);
    ++this.m_pItemIndex;
    if (this.m_saveFormatting && this.m_srcFormat != null)
    {
      this.m_srcFormat.UpdateSourceFormatting((paragraphItem as WTextRange).CharacterFormat);
    }
    else
    {
      if (this.m_saveFormatting)
        return;
      textRange.ParaItemCharFormat.UpdateSourceFormatting((paragraphItem as WTextRange).CharacterFormat);
    }
  }

  private void CopyParagraphItems(WParagraph srcParagraph, WParagraph destParagraph)
  {
    int num = 0;
    for (int index = 0; index < srcParagraph.Items.Count; ++index)
    {
      ParagraphItem paragraphItem1 = srcParagraph.Items[index];
      ParagraphItem paragraphItem2 = paragraphItem1.Clone() as ParagraphItem;
      int count = destParagraph.Items.Count;
      destParagraph.Items.Insert(this.m_pItemIndex + index - num, (IEntity) paragraphItem2);
      if (paragraphItem1 is WTextRange)
        paragraphItem1.ParaItemCharFormat.UpdateSourceFormatting((paragraphItem2 as WTextRange).CharacterFormat);
      if (count == destParagraph.Items.Count)
        ++num;
    }
  }

  internal static void SplitParagraph(
    WParagraph paragraph,
    int nextpItemIndex,
    WParagraph paragraphToInsert)
  {
    int inOwnerCollection = paragraph.GetIndexInOwnerCollection();
    paragraph.OwnerTextBody.Items.Insert(inOwnerCollection + 1, (IEntity) paragraphToInsert);
    paragraphToInsert.ParagraphFormat.ImportContainer((FormatBase) paragraph.ParagraphFormat);
    paragraphToInsert.BreakCharacterFormat.ImportContainer((FormatBase) paragraph.BreakCharacterFormat);
    while (paragraph.Items.Count > nextpItemIndex)
      paragraphToInsert.Items.Add((IEntity) paragraph.Items[nextpItemIndex]);
  }

  private void ValidateArgs()
  {
    if (this.m_body == null)
      throw new ArgumentNullException("textBody");
    if (this.m_itemIndex < 0 || this.m_itemIndex > this.m_body.Items.Count)
      throw new ArgumentOutOfRangeException("itemIndex", "itemIndex is less than 0 or greater than " + (object) this.m_body.Items.Count);
    if ((this.m_body.Items.Count > this.m_itemIndex ? this.m_body.Items[this.m_itemIndex] : (TextBodyItem) null) is WParagraph wparagraph && (this.m_pItemIndex < 0 || this.m_pItemIndex > wparagraph.Items.Count))
      throw new ArgumentOutOfRangeException("pItemIndex", "pItemIndex is less than 0 or greater than  " + (object) wparagraph.Items.Count);
  }

  private WParagraph SplitParagraph(WParagraph trgFirstParagraph, WParagraph srcLastParagraph)
  {
    WParagraph wparagraph = srcLastParagraph == null ? new WParagraph((IWordDocument) this.m_body.Document) : (WParagraph) srcLastParagraph.Clone();
    if (trgFirstParagraph.Items.Count > this.m_pItemIndex || srcLastParagraph != null)
    {
      this.m_body.Items.Insert(this.m_itemIndex + 1, (IEntity) wparagraph);
      wparagraph.ApplyStyle(trgFirstParagraph.StyleName, false);
      this.ApplySrcFormat(wparagraph);
    }
    while (trgFirstParagraph.Items.Count > this.m_pItemIndex)
      wparagraph.UpdateBookmarkEnd(trgFirstParagraph.Items[this.m_pItemIndex], wparagraph, true);
    return wparagraph;
  }

  private void UpdateFormatting(WParagraph trgParagraph, WParagraph srcParagraph)
  {
    srcParagraph.ParagraphFormat.UpdateSourceFormatting(trgParagraph.ParagraphFormat);
    srcParagraph.BreakCharacterFormat.UpdateSourceFormatting(trgParagraph.BreakCharacterFormat);
    int index = 0;
    for (int count = trgParagraph.Items.Count; index < count; ++index)
    {
      Entity entity1 = (Entity) trgParagraph.Items[index];
      Entity entity2 = (Entity) null;
      if (index < srcParagraph.Items.Count)
        entity2 = (Entity) srcParagraph.Items[index];
      if (entity1 is WTextRange && entity2 is WTextRange)
        (entity2 as WTextRange).CharacterFormat.UpdateSourceFormatting((entity1 as WTextRange).CharacterFormat);
    }
  }

  private void EnsureTextBody(WordDocument doc)
  {
    if (this.m_textPart != null && this.m_textPart.Document == doc)
      this.Clear();
    else
      this.m_textPart = new WTextBody(doc, (Entity) null);
  }

  private void ApplySrcFormat(WParagraph paragraph)
  {
    if (!this.m_saveFormatting || this.m_srcFormat == null || paragraph == null)
      return;
    this.m_srcFormat.UpdateSourceFormatting(paragraph.BreakCharacterFormat);
    foreach (ParagraphItem paragraphItem in (CollectionImpl) paragraph.Items)
    {
      if (paragraphItem is WTextRange)
        this.m_srcFormat.UpdateSourceFormatting(paragraphItem.ParaItemCharFormat);
    }
  }

  private void ApplySrcFormat(WTable table, WTable srcTable)
  {
    if (this.m_saveFormatting && this.m_srcFormat == null || table == null)
      return;
    for (int index1 = 0; index1 < table.Rows.Count; ++index1)
    {
      for (int index2 = 0; index2 < table.Rows[index1].Cells.Count; ++index2)
      {
        for (int index3 = 0; index3 < table.Rows[index1].Cells[index2].ChildEntities.Count; ++index3)
        {
          if (table.Rows[index1].Cells[index2].ChildEntities[index3] is WParagraph)
          {
            WParagraph childEntity = table.Rows[index1].Cells[index2].ChildEntities[index3] as WParagraph;
            childEntity.ApplyStyle(childEntity.StyleName, false);
            if (this.m_saveFormatting)
              this.ApplySrcFormat(childEntity);
            else
              this.UpdateFormatting(childEntity, srcTable.Rows[index1].Cells[index2].ChildEntities[index3] as WParagraph);
          }
          else if (table.Rows[index1].Cells[index2].ChildEntities[index3] is WTable)
            this.ApplySrcFormat(table.Rows[index1].Cells[index2].ChildEntities[index3] as WTable, srcTable.Rows[index1].Cells[index2].ChildEntities[index3] as WTable);
        }
      }
    }
  }

  internal void GetBookmarkContentPart(BookmarkStart bkmkStart, BookmarkEnd bkmkEnd)
  {
    WParagraph owner1 = bkmkStart.Owner as WParagraph;
    WParagraph owner2 = bkmkEnd.Owner as WParagraph;
    if (!owner1.IsInCell && !owner2.IsInCell || owner1.OwnerTextBody == owner2.OwnerTextBody)
      this.GetParagraphBookmarkContent(owner1, owner2, bkmkStart, bkmkEnd);
    else if (owner1.IsInCell && owner2.IsInCell)
      this.GetTableBookmarkContent(owner1, owner2, bkmkStart, bkmkEnd, (WTableCell) null);
    else if (!owner1.IsInCell && owner2.IsInCell)
    {
      this.GetParagraphAfterTableBkmkContent(owner1, owner2, bkmkStart, bkmkEnd);
    }
    else
    {
      if (!owner1.IsInCell || owner2.IsInCell)
        return;
      this.GetTableAfteParagraphBkmkContent(owner1, owner2, bkmkStart, bkmkEnd);
    }
  }

  private Entity GetSection(Entity entity)
  {
    while (!(entity is WSection))
      entity = entity.Owner;
    return entity;
  }

  private void GetTableAfteParagraphBkmkContent(
    WParagraph paragraphStart,
    WParagraph paragraphEnd,
    BookmarkStart bkmkStart,
    BookmarkEnd bkmkEnd)
  {
    WTableCell wtableCell = paragraphStart.GetOwnerEntity() as WTableCell;
    WTableCell bkmkEndCell = (WTableCell) null;
    WTable owner1 = (WTable) wtableCell.Owner.Owner;
    int inOwnerCollection1 = wtableCell.Owner.GetIndexInOwnerCollection();
    if (bkmkStart.ColumnFirst != (short) -1 && bkmkStart.ColumnLast != (short) -1)
    {
      int inOwnerCollection2 = owner1.LastRow.GetIndexInOwnerCollection();
      if ((int) bkmkStart.ColumnFirst < owner1.Rows[inOwnerCollection1].Cells.Count && (int) bkmkStart.ColumnLast < owner1.Rows[inOwnerCollection2].Cells.Count)
      {
        wtableCell = owner1.Rows[inOwnerCollection1].Cells[(int) bkmkStart.ColumnFirst];
        bkmkEndCell = owner1.Rows[inOwnerCollection2].Cells[(int) bkmkStart.ColumnLast];
      }
      wtableCell.GetIndexInOwnerCollection();
      bkmkEndCell.GetIndexInOwnerCollection();
      this.GetTableBookmarkContent(paragraphStart, paragraphEnd, bkmkStart, bkmkEnd, bkmkEndCell);
    }
    else
    {
      this.m_textPart = new WTextBody(owner1.OwnerTextBody.Owner as WSection);
      int inOwnerCollection3 = owner1.GetIndexInOwnerCollection();
      int inOwnerCollection4 = paragraphEnd.GetIndexInOwnerCollection();
      int num = bkmkEnd.GetIndexInOwnerCollection() - 1;
      WTextBody owner2 = (WTextBody) owner1.Owner;
      WTextBody owner3 = (WTextBody) paragraphEnd.Owner;
      int startSectionIndex = this.GetSection((Entity) owner2).GetIndexInOwnerCollection() + 1;
      int inOwnerCollection5 = this.GetSection((Entity) owner3).GetIndexInOwnerCollection();
      bool isInSingleSection = false;
      if (startSectionIndex - 1 == inOwnerCollection5)
        isInSingleSection = true;
      bool IsFirstBkmkEnd = this.IsBkmkEndInFirstItem(paragraphEnd, bkmkEnd, num);
      if (owner2 != owner3 || owner2.ChildEntities[inOwnerCollection3 + 1] != paragraphEnd || !IsFirstBkmkEnd)
      {
        bkmkStart.GetBkmkContentInDiffCell(owner1, inOwnerCollection1, owner1.LastRow.GetIndexInOwnerCollection(), 0, owner1.LastCell.GetIndexInOwnerCollection(), this.m_textPart);
        this.AddTextBodyItems(inOwnerCollection3, inOwnerCollection4, owner1.OwnerTextBody);
        if (IsFirstBkmkEnd && isInSingleSection)
          return;
        this.CopyMultiSectionBodyItems(startSectionIndex, inOwnerCollection5, owner2.Document);
        this.CopyBkmkEndTextBody(inOwnerCollection4, num, owner3, IsFirstBkmkEnd, isInSingleSection);
      }
      else
      {
        WTableCell lastCell = owner1.LastCell;
        this.GetTableBookmarkContent(paragraphStart, paragraphEnd, bkmkStart, bkmkEnd, lastCell);
      }
    }
  }

  private void GetParagraphAfterTableBkmkContent(
    WParagraph paragraphStart,
    WParagraph paragraphEnd,
    BookmarkStart bkmkStart,
    BookmarkEnd bkmkEnd)
  {
    WTextBody owner1 = (WTextBody) paragraphStart.Owner;
    WTableCell ownerEntity = paragraphEnd.GetOwnerEntity() as WTableCell;
    WTable owner2 = (WTable) ownerEntity.Owner.Owner;
    WTextBody owner3 = (WTextBody) owner2.Owner;
    this.m_textPart = new WTextBody(owner1.Owner as WSection);
    bool isInSingleSection = false;
    int inOwnerCollection1 = owner2.GetIndexInOwnerCollection();
    int startSectionIndex = this.GetSection((Entity) owner1).GetIndexInOwnerCollection() + 1;
    int inOwnerCollection2 = this.GetSection((Entity) owner3).GetIndexInOwnerCollection();
    if (startSectionIndex - 1 == inOwnerCollection2)
      isInSingleSection = true;
    int inOwnerCollection3 = ownerEntity.Owner.GetIndexInOwnerCollection();
    int inOwnerCollection4 = ownerEntity.GetIndexInOwnerCollection();
    if (inOwnerCollection4 == 0 && inOwnerCollection3 != 0 && ownerEntity.Paragraphs[0] == bkmkEnd.OwnerParagraph && this.IsBkmkEndInFirstItem(ownerEntity.Paragraphs[0], bkmkEnd, bkmkEnd.GetIndexInOwnerCollection() - 1))
      --inOwnerCollection3;
    if (inOwnerCollection1 >= 0 && inOwnerCollection3 == 0 && inOwnerCollection4 == 0 && ownerEntity.Paragraphs[0] == bkmkEnd.OwnerParagraph)
    {
      if (ownerEntity.ChildEntities[0] is WParagraph && inOwnerCollection1 == 0)
      {
        WParagraph childEntity = ownerEntity.ChildEntities[0] as WParagraph;
        if (childEntity.Items[0] == bkmkEnd)
        {
          WParagraph wparagraph = new WParagraph((IWordDocument) owner3.Document);
          wparagraph.Items.Add((IEntity) childEntity.Items[0]);
          owner3.ChildEntities.Insert(0, (IEntity) wparagraph);
          this.GetParagraphBookmarkContent(paragraphStart, paragraphEnd, bkmkStart, bkmkEnd);
          return;
        }
      }
      else if (inOwnerCollection1 > 0)
      {
        (owner3.Items[inOwnerCollection1 - 1] as WParagraph).Items.Add((IEntity) bkmkEnd);
        this.GetParagraphBookmarkContent(paragraphStart, owner3.Items[inOwnerCollection1 - 1] as WParagraph, bkmkStart, bkmkEnd);
        return;
      }
    }
    int inOwnerCollection5 = paragraphStart.GetIndexInOwnerCollection();
    int bkmkStartNextItemIndex = bkmkStart.GetIndexInOwnerCollection() + 1;
    this.CopyBkmkStartTextBody(inOwnerCollection5, inOwnerCollection1, owner1, bkmkStartNextItemIndex, inOwnerCollection1 - 1, isInSingleSection);
    if (!isInSingleSection)
    {
      this.CopyMultiSectionBodyItems(startSectionIndex, inOwnerCollection2, owner1.Document);
      this.AddTextBodyItems(0, inOwnerCollection1 - 1, owner2.OwnerTextBody);
    }
    bkmkStart.GetBkmkContentInDiffCell(owner2, 0, inOwnerCollection3, 0, owner2.LastCell.GetIndexInOwnerCollection(), this.m_textPart);
  }

  private void GetTableBookmarkContent(
    WParagraph paragraphStart,
    WParagraph paragraphEnd,
    BookmarkStart bkmkStart,
    BookmarkEnd bkmkEnd,
    WTableCell bkmkEndCell)
  {
    WTableCell ownerEntity = paragraphStart.GetOwnerEntity() as WTableCell;
    if (paragraphEnd.IsInCell)
      bkmkEndCell = paragraphEnd.GetOwnerEntity() as WTableCell;
    WTableCell tempBkmkEndCell = bkmkEndCell;
    WTable owner1 = (WTable) ownerEntity.Owner.Owner;
    WTable owner2 = (WTable) bkmkEndCell.Owner.Owner;
    this.m_textPart = new WTextBody(owner1.Owner.Owner as WSection);
    bool flag1 = false;
    int inOwnerCollection1 = ownerEntity.Owner.GetIndexInOwnerCollection();
    int inOwnerCollection2 = bkmkEndCell.Owner.GetIndexInOwnerCollection();
    int inOwnerCollection3 = bkmkEndCell.GetIndexInOwnerCollection();
    int inOwnerCollection4 = ownerEntity.GetIndexInOwnerCollection();
    int startSectionIndex = this.GetSection((Entity) owner1).GetIndexInOwnerCollection() + 1;
    int inOwnerCollection5 = this.GetSection((Entity) owner2).GetIndexInOwnerCollection();
    if (startSectionIndex - 1 == inOwnerCollection5)
      flag1 = true;
    bkmkStart.GetBookmarkStartAndEndCell(ownerEntity, bkmkEndCell, tempBkmkEndCell, owner1, owner2, bkmkStart, bkmkEnd, inOwnerCollection1, ref inOwnerCollection2, ref inOwnerCollection4, ref inOwnerCollection3);
    if (owner1 == owner2)
    {
      bkmkStart.GetBkmkContentInDiffCell(owner1, inOwnerCollection1, inOwnerCollection2, inOwnerCollection4, inOwnerCollection3, this.m_textPart);
    }
    else
    {
      WTable bkmkEndTable = owner2;
      int bkmkEndRowIndex = inOwnerCollection2;
      bool flag2 = flag1 && owner2.IsInCell && bkmkStart.IsBookmarkEndAtSameCell(ownerEntity, owner1, ref bkmkEndTable, ref bkmkEndRowIndex);
      if (!flag2)
        bkmkStart.GetBkmkContentInDiffCell(owner1, inOwnerCollection1, owner1.Rows.Count - 1, 0, owner1.LastCell.GetIndexInOwnerCollection(), this.m_textPart);
      int inOwnerCollection6 = owner1.GetIndexInOwnerCollection();
      int inOwnerCollection7 = owner2.GetIndexInOwnerCollection();
      if (flag1)
      {
        if (flag2)
          this.AddTextBodyItemsOfNestedTable(bkmkStart, bkmkEndTable, bkmkEndRowIndex, ownerEntity);
        else
          this.AddTextBodyItems(inOwnerCollection6, inOwnerCollection7, owner1.OwnerTextBody);
      }
      else
      {
        this.AddTextBodyItems(inOwnerCollection6, owner1.OwnerTextBody.ChildEntities.Count, owner1.OwnerTextBody);
        this.CopyMultiSectionBodyItems(startSectionIndex, inOwnerCollection5, owner2.Document);
        this.AddTextBodyItems(0, inOwnerCollection7, owner2.OwnerTextBody);
      }
      if (flag2)
        bkmkStart.GetBkmkContentInDiffCell(bkmkEndTable, 0, bkmkEndRowIndex, 0, bkmkEndTable.LastCell.GetIndexInOwnerCollection(), this.m_textPart);
      else
        bkmkStart.GetBkmkContentInDiffCell(owner2, 0, inOwnerCollection2, 0, owner2.LastCell.GetIndexInOwnerCollection(), this.m_textPart);
    }
  }

  private void AddTextBodyItemsOfNestedTable(
    BookmarkStart bkmrkStart,
    WTable bkrmkEndTable,
    int endRowIndex,
    WTableCell bkmrkStartCell)
  {
    int index1 = bkmrkStart.OwnerParagraph.Index;
    int index2 = bkrmkEndTable.Index;
    for (int index3 = index1; index3 < index2; ++index3)
    {
      TextBodyItem textBodyItem = (TextBodyItem) bkmrkStartCell.Items[index3].Clone();
      if (index3 == index1 && textBodyItem.EntityType == EntityType.Paragraph)
      {
        WParagraph wparagraph = textBodyItem as WParagraph;
        for (int index4 = bkmrkStart.Index; index4 > 0; --index4)
          wparagraph.Items.RemoveFromInnerList(0);
      }
      this.m_textPart.Items.Add((IEntity) textBodyItem);
    }
  }

  private void AddTextBodyItems(int startItemIndex, int endItemIndex, WTextBody textBody)
  {
    for (int index = startItemIndex + 1; index < endItemIndex; ++index)
      this.m_textPart.ChildEntities.Add((IEntity) textBody.ChildEntities[index].CloneInt());
  }

  private void GetParagraphBookmarkContent(
    WParagraph paragraphStart,
    WParagraph paragraphEnd,
    BookmarkStart bkmkStart,
    BookmarkEnd bkmkEnd)
  {
    WTextBody ownerTextBody1 = paragraphStart.OwnerTextBody;
    WTextBody ownerTextBody2 = paragraphEnd.OwnerTextBody;
    this.m_textPart = new WTextBody(this.GetSection((Entity) ownerTextBody1) as WSection);
    bool isInSingleSection = false;
    int inOwnerCollection1 = paragraphStart.GetIndexInOwnerCollection();
    int bkmkStartNextItemIndex = bkmkStart.GetIndexInOwnerCollection() + 1;
    int inOwnerCollection2 = paragraphEnd.GetIndexInOwnerCollection();
    int num = bkmkEnd.GetIndexInOwnerCollection() - 1;
    if (ownerTextBody1 == ownerTextBody2)
      isInSingleSection = true;
    if (isInSingleSection)
    {
      TextBodySelection textSel = new TextBodySelection((ParagraphItem) bkmkStart, (ParagraphItem) bkmkEnd);
      ++textSel.ParagraphItemStartIndex;
      --textSel.ParagraphItemEndIndex;
      this.Copy(textSel, true);
    }
    else
    {
      this.CopyBkmkStartTextBody(inOwnerCollection1, inOwnerCollection2, ownerTextBody1, bkmkStartNextItemIndex, num, isInSingleSection);
      if (!isInSingleSection)
        this.CopyMultiSectionBodyItems(this.GetSection((Entity) ownerTextBody1).GetIndexInOwnerCollection() + 1, this.GetSection((Entity) ownerTextBody2).GetIndexInOwnerCollection(), ownerTextBody1.Document);
      bool IsFirstBkmkEnd = this.IsBkmkEndInFirstItem(paragraphEnd, bkmkEnd, num);
      if (IsFirstBkmkEnd && isInSingleSection)
        return;
      this.CopyBkmkEndTextBody(inOwnerCollection2, num, ownerTextBody2, IsFirstBkmkEnd, isInSingleSection);
    }
  }

  private void CopyBkmkStartTextBody(
    int startParagraphIndex,
    int endParagraphIndex,
    WTextBody startTextBody,
    int bkmkStartNextItemIndex,
    int bkmkEndPrevItemIndex,
    bool isInSingleSection)
  {
    int num = isInSingleSection ? endParagraphIndex : startTextBody.ChildEntities.Count;
    for (int index = startParagraphIndex; index < num; ++index)
    {
      TextBodyItem childEntity = startTextBody.ChildEntities[index] as TextBodyItem;
      if (startParagraphIndex == index && bkmkStartNextItemIndex > 0)
      {
        WParagraph wparagraph = (childEntity as WParagraph).Clone() as WParagraph;
        for (; bkmkStartNextItemIndex > 0; --bkmkStartNextItemIndex)
          wparagraph.Items.RemoveFromInnerList(0);
        if (wparagraph.ChildEntities.Count > 0)
          this.m_textPart.ChildEntities.Add((IEntity) wparagraph);
      }
      else
        this.m_textPart.ChildEntities.Add((IEntity) childEntity.Clone());
    }
  }

  private void CopyBkmkEndTextBody(
    int endParagraphIndex,
    int bkmkEndPreviosItemIndex,
    WTextBody endTextBody,
    bool IsFirstBkmkEnd,
    bool isInSingleSection)
  {
    if (!isInSingleSection)
      this.CopyBkmkStartTextBody(0, endParagraphIndex, endTextBody, 0, -1, true);
    if (IsFirstBkmkEnd)
      return;
    WParagraph wparagraph = (endTextBody.Items[endParagraphIndex] as WParagraph).Clone() as WParagraph;
    while (bkmkEndPreviosItemIndex + 1 < wparagraph.Items.Count)
      wparagraph.Items.RemoveFromInnerList(bkmkEndPreviosItemIndex + 1);
    this.m_textPart.ChildEntities.Add((IEntity) wparagraph);
  }

  private bool IsBkmkEndInFirstItem(
    WParagraph paragraph,
    BookmarkEnd bkmkEnd,
    int bkmkEndPreItemIndex)
  {
    for (int index = 0; index <= bkmkEndPreItemIndex; ++index)
    {
      if (!(paragraph.Items[index] is BookmarkStart) && !(paragraph.Items[index] is BookmarkEnd))
        return false;
    }
    return true;
  }

  private void CopyMultiSectionBodyItems(
    int startSectionIndex,
    int endSectionIndex,
    WordDocument Document)
  {
    for (int index = startSectionIndex; index < endSectionIndex; ++index)
    {
      foreach (Entity childEntity in (CollectionImpl) Document.Sections[index].Body.ChildEntities)
        this.m_textPart.ChildEntities.Add((IEntity) childEntity.Clone());
    }
  }
}
