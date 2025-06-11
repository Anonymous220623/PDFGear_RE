// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WordDocumentPart
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WordDocumentPart
{
  private WSectionCollection m_sections;

  public WSectionCollection Sections => this.m_sections;

  public WordDocumentPart() => this.m_sections = new WSectionCollection((WordDocument) null);

  public WordDocumentPart(WordDocument document)
  {
    this.m_sections = new WSectionCollection((WordDocument) null);
    this.Load(document);
  }

  public void Load(WordDocument document)
  {
    foreach (WSection section in (CollectionImpl) document.Sections)
      this.m_sections.Add((IWSection) section.Clone());
  }

  public WordDocument GetAsWordDocument()
  {
    WordDocument asWordDocument = new WordDocument();
    foreach (WSection section in (CollectionImpl) this.m_sections)
      asWordDocument.Sections.Add((IWSection) section.Clone());
    return asWordDocument;
  }

  public void Close()
  {
    if (this.m_sections == null || this.m_sections.Count <= 0)
      return;
    WSection wsection = (WSection) null;
    for (int index = 0; index < this.m_sections.Count; ++index)
    {
      this.m_sections[index].Close();
      wsection = (WSection) null;
    }
    this.m_sections.Clear();
    this.m_sections = (WSectionCollection) null;
  }

  internal void GetWordDocumentPart(BookmarkStart bkmkStart, BookmarkEnd bkmkEnd)
  {
    WParagraph ownerParagraph1 = bkmkStart.OwnerParagraph;
    WParagraph ownerParagraph2 = bkmkEnd.OwnerParagraph;
    if (!ownerParagraph1.IsInCell && !ownerParagraph2.IsInCell || ownerParagraph1.OwnerTextBody == ownerParagraph2.OwnerTextBody)
      this.GetParagraphDocumentPart(ownerParagraph1, ownerParagraph2, bkmkStart, bkmkEnd);
    else if (ownerParagraph1.IsInCell && ownerParagraph2.IsInCell)
      this.GetTableDocumentPart(ownerParagraph1, ownerParagraph2, bkmkStart, bkmkEnd, (WTableCell) null);
    else if (ownerParagraph1.IsInCell && !ownerParagraph2.IsInCell)
    {
      this.GetTableAfterParagraphDocumentPart(ownerParagraph1, ownerParagraph2, bkmkStart, bkmkEnd);
    }
    else
    {
      if (ownerParagraph1.IsInCell || !ownerParagraph2.IsInCell)
        return;
      this.GetParagraphAfterTableDocumentPart(ownerParagraph1, ownerParagraph2, bkmkStart, bkmkEnd);
    }
  }

  private Entity GetSection(Entity entity)
  {
    while (!(entity is WSection))
      entity = entity.Owner;
    return entity;
  }

  private void GetParagraphAfterTableDocumentPart(
    WParagraph startParagraph,
    WParagraph endParagraph,
    BookmarkStart bkmkStart,
    BookmarkEnd bkmkEnd)
  {
    WTextBody ownerTextBody1 = startParagraph.OwnerTextBody;
    WTableCell ownerEntity = endParagraph.GetOwnerEntity() as WTableCell;
    WTable owner = (WTable) ownerEntity.Owner.Owner;
    WTextBody ownerTextBody2 = owner.OwnerTextBody;
    int inOwnerCollection1 = owner.GetIndexInOwnerCollection();
    int startSectionIndex = this.GetSection((Entity) ownerTextBody1).GetIndexInOwnerCollection() + 1;
    int inOwnerCollection2 = this.GetSection((Entity) ownerTextBody2).GetIndexInOwnerCollection();
    bool isInSingleSection = false;
    if (startSectionIndex - 1 == inOwnerCollection2)
      isInSingleSection = true;
    int inOwnerCollection3 = ownerEntity.Owner.GetIndexInOwnerCollection();
    int inOwnerCollection4 = ownerEntity.GetIndexInOwnerCollection();
    if (inOwnerCollection4 == 0 && inOwnerCollection3 != 0 && ownerEntity.Paragraphs[0] == bkmkEnd.OwnerParagraph && this.IsBkmkEndInFirstItem(ownerEntity.Paragraphs[0], (ParagraphItem) bkmkEnd, bkmkEnd.GetIndexInOwnerCollection() - 1))
      --inOwnerCollection3;
    if (inOwnerCollection1 >= 0 && inOwnerCollection3 == 0 && inOwnerCollection4 == 0 && ownerEntity.Paragraphs[0] == bkmkEnd.OwnerParagraph)
    {
      if (ownerEntity.ChildEntities[0] is WParagraph && inOwnerCollection1 == 0)
      {
        WParagraph childEntity = ownerEntity.ChildEntities[0] as WParagraph;
        if (this.IsBkmkEndInFirstItem(childEntity, (ParagraphItem) bkmkEnd, bkmkEnd.GetIndexInOwnerCollection() - 1))
        {
          WParagraph paraEnd = new WParagraph((IWordDocument) ownerTextBody2.Document);
          paraEnd.Items.Add((IEntity) childEntity.Items[bkmkEnd.GetIndexInOwnerCollection()]);
          ownerTextBody2.ChildEntities.Insert(0, (IEntity) paraEnd);
          this.GetParagraphDocumentPart(startParagraph, paraEnd, bkmkStart, bkmkEnd);
          return;
        }
      }
      else if (inOwnerCollection1 > 0)
      {
        (ownerTextBody2.Items[inOwnerCollection1 - 1] as WParagraph).Items.Add((IEntity) bkmkEnd);
        this.GetParagraphDocumentPart(startParagraph, ownerTextBody2.Items[inOwnerCollection1 - 1] as WParagraph, bkmkStart, bkmkEnd);
        return;
      }
    }
    int inOwnerCollection5 = startParagraph.GetIndexInOwnerCollection();
    int bkmkStartNextItemIndex = bkmkStart.GetIndexInOwnerCollection() + 1;
    this.AddFirstSectionToDocumentPart(inOwnerCollection5, inOwnerCollection1, ownerTextBody1, bkmkStartNextItemIndex, inOwnerCollection1 - 1, isInSingleSection, (ownerTextBody1.Owner as WSection).CloneWithoutBodyItems());
    if (!isInSingleSection)
    {
      this.AddInBetweenSections(startSectionIndex, inOwnerCollection2, ownerTextBody1.Document);
      this.AddLastSectionToDocumentPart(inOwnerCollection1, -1, owner.OwnerTextBody, true, isInSingleSection);
      bkmkStart.GetBkmkContentInDiffCell(owner, 0, inOwnerCollection3, 0, owner.LastCell.GetIndexInOwnerCollection(), this.m_sections[this.m_sections.Count - 1].Body);
    }
    else
      bkmkStart.GetBkmkContentInDiffCell(owner, 0, inOwnerCollection3, 0, owner.LastCell.GetIndexInOwnerCollection(), this.m_sections[0].Body);
  }

  private void GetTableAfterParagraphDocumentPart(
    WParagraph startParagraph,
    WParagraph endParagraph,
    BookmarkStart bkmkStart,
    BookmarkEnd bkmkEnd)
  {
    WTableCell wtableCell = startParagraph.GetOwnerEntity() as WTableCell;
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
      this.GetTableDocumentPart(startParagraph, endParagraph, bkmkStart, bkmkEnd, bkmkEndCell);
    }
    else
    {
      int inOwnerCollection3 = owner1.GetIndexInOwnerCollection();
      int inOwnerCollection4 = endParagraph.GetIndexInOwnerCollection();
      int bkmkEndPreviosItemIndex = bkmkEnd.GetIndexInOwnerCollection() - 1;
      WTextBody owner2 = (WTextBody) owner1.Owner;
      WTextBody owner3 = (WTextBody) endParagraph.Owner;
      bool isInSingleSection = false;
      int startSectionIndex = this.GetSection((Entity) owner2).GetIndexInOwnerCollection() + 1;
      int inOwnerCollection5 = this.GetSection((Entity) owner3).GetIndexInOwnerCollection();
      if (startSectionIndex - 1 == inOwnerCollection5)
        isInSingleSection = true;
      bool IsFirstBkmkEnd = this.IsBkmkEndInFirstItem(endParagraph, (ParagraphItem) bkmkEnd, bkmkEndPreviosItemIndex);
      if (owner2 != owner3 || owner2.ChildEntities[inOwnerCollection3 + 1] != endParagraph || !IsFirstBkmkEnd)
      {
        WSection section = (this.GetSection((Entity) owner1) as WSection).CloneWithoutBodyItems();
        bkmkStart.GetBkmkContentInDiffCell(owner1, inOwnerCollection1, owner1.LastRow.GetIndexInOwnerCollection(), 0, owner1.LastCell.GetIndexInOwnerCollection(), section.Body);
        this.AddFirstSectionToDocumentPart(inOwnerCollection3 + 1, inOwnerCollection4, owner1.OwnerTextBody, -1, -1, isInSingleSection, section);
        if (IsFirstBkmkEnd && isInSingleSection)
          return;
        this.AddInBetweenSections(startSectionIndex, inOwnerCollection5, owner2.Document);
        if (IsFirstBkmkEnd)
          --inOwnerCollection4;
        this.AddLastSectionToDocumentPart(inOwnerCollection4, bkmkEndPreviosItemIndex, endParagraph.OwnerTextBody, IsFirstBkmkEnd, isInSingleSection);
      }
      else
        this.GetTableDocumentPart(startParagraph, endParagraph, bkmkStart, bkmkEnd, bkmkEndCell);
    }
  }

  private void GetTableDocumentPart(
    WParagraph startParagraph,
    WParagraph endParagraph,
    BookmarkStart bkmkStart,
    BookmarkEnd bkmkEnd,
    WTableCell bkmkEndCell)
  {
    WTableCell ownerEntity = startParagraph.GetOwnerEntity() as WTableCell;
    if (endParagraph.IsInCell)
      bkmkEndCell = endParagraph.GetOwnerEntity() as WTableCell;
    else if (endParagraph.PreviousSibling is WTable)
      bkmkEndCell = (endParagraph.PreviousSibling as WTable).LastCell;
    if (ownerEntity == null || bkmkEndCell == null)
      return;
    WTableCell tempBkmkEndCell = bkmkEndCell;
    WTable owner1 = (WTable) ownerEntity.Owner.Owner;
    WTable owner2 = (WTable) bkmkEndCell.Owner.Owner;
    WSection section1 = (owner1.OwnerTextBody.Owner as WSection).CloneWithoutBodyItems();
    int inOwnerCollection1 = ownerEntity.Owner.GetIndexInOwnerCollection();
    int inOwnerCollection2 = bkmkEndCell.Owner.GetIndexInOwnerCollection();
    int inOwnerCollection3 = bkmkEndCell.GetIndexInOwnerCollection();
    int inOwnerCollection4 = ownerEntity.GetIndexInOwnerCollection();
    bool flag = false;
    int startSectionIndex = this.GetSection((Entity) owner1).GetIndexInOwnerCollection() + 1;
    int inOwnerCollection5 = this.GetSection((Entity) owner2).GetIndexInOwnerCollection();
    if (startSectionIndex - 1 == inOwnerCollection5)
      flag = true;
    bkmkStart.GetBookmarkStartAndEndCell(ownerEntity, bkmkEndCell, tempBkmkEndCell, owner1, owner2, bkmkStart, bkmkEnd, inOwnerCollection1, ref inOwnerCollection2, ref inOwnerCollection4, ref inOwnerCollection3);
    if (owner1 == owner2)
    {
      bkmkStart.GetBkmkContentInDiffCell(owner1, inOwnerCollection1, inOwnerCollection2, inOwnerCollection4, inOwnerCollection3, section1.Body);
      this.m_sections.Add((IWSection) section1);
    }
    else
    {
      bkmkStart.GetBkmkContentInDiffCell(owner1, inOwnerCollection1, owner1.Rows.Count - 1, 0, owner1.LastCell.GetIndexInOwnerCollection(), section1.Body);
      int inOwnerCollection6 = owner1.GetIndexInOwnerCollection();
      int inOwnerCollection7 = owner2.GetIndexInOwnerCollection();
      if (flag)
      {
        this.AddSectionBodyItems(inOwnerCollection6 + 1, inOwnerCollection7, owner1.OwnerTextBody, section1);
        bkmkStart.GetBkmkContentInDiffCell(owner2, 0, inOwnerCollection2, 0, owner2.LastCell.GetIndexInOwnerCollection(), section1.Body);
        this.m_sections.Add((IWSection) section1);
      }
      else
      {
        this.AddSectionBodyItems(inOwnerCollection6 + 1, owner1.OwnerTextBody.ChildEntities.Count, owner1.OwnerTextBody, section1);
        this.m_sections.Add((IWSection) section1);
        this.AddInBetweenSections(startSectionIndex, inOwnerCollection5, owner1.Document);
        WSection section2 = (this.GetSection((Entity) owner2) as WSection).CloneWithoutBodyItems();
        this.AddSectionBodyItems(0, inOwnerCollection7, owner2.OwnerTextBody, section2);
        bkmkStart.GetBkmkContentInDiffCell(owner2, 0, inOwnerCollection2, 0, owner2.LastCell.GetIndexInOwnerCollection(), section2.Body);
        this.m_sections.Add((IWSection) section2);
      }
    }
  }

  private void AddSectionBodyItems(
    int startItemIndex,
    int endItemIndex,
    WTextBody textBody,
    WSection section)
  {
    for (int index = startItemIndex + 1; index < endItemIndex; ++index)
    {
      TextBodyItem textBodyItem = (TextBodyItem) textBody.ChildEntities[index].CloneInt();
      section.Body.Items.AddToInnerList((Entity) textBodyItem);
    }
  }

  private void GetParagraphDocumentPart(
    WParagraph paraStart,
    WParagraph paraEnd,
    BookmarkStart bkmkStart,
    BookmarkEnd bkmkEnd)
  {
    WTextBody ownerTextBody1 = paraStart.OwnerTextBody;
    WTextBody ownerTextBody2 = paraEnd.OwnerTextBody;
    int inOwnerCollection1 = paraStart.GetIndexInOwnerCollection();
    int bkmkStartNextItemIndex = bkmkStart.GetIndexInOwnerCollection() + 1;
    int inOwnerCollection2 = paraEnd.GetIndexInOwnerCollection();
    int bkmkEndPreviosItemIndex = bkmkEnd.GetIndexInOwnerCollection() - 1;
    bool isInSingleSection = false;
    int startSectionIndex = this.GetSection((Entity) ownerTextBody1).GetIndexInOwnerCollection() + 1;
    int inOwnerCollection3 = this.GetSection((Entity) ownerTextBody2).GetIndexInOwnerCollection();
    if (startSectionIndex - 1 == inOwnerCollection3)
      isInSingleSection = true;
    if (isInSingleSection)
    {
      if (!(ownerTextBody1.GetOwnerSection((Entity) ownerTextBody1) is WSection ownerSection))
        return;
      WSection section = ownerSection.CloneWithoutBodyItems();
      TextBodySelection textBodySelection = new TextBodySelection((ParagraphItem) bkmkStart, (ParagraphItem) bkmkEnd);
      ++textBodySelection.ParagraphItemStartIndex;
      --textBodySelection.ParagraphItemEndIndex;
      TextBodyPart textBodyPart = new TextBodyPart(textBodySelection);
      while (textBodyPart.BodyItems.Count != 0)
        section.Body.ChildEntities.Add((IEntity) textBodyPart.BodyItems[0]);
      this.m_sections.Add((IWSection) section);
    }
    else
    {
      this.AddFirstSectionToDocumentPart(inOwnerCollection1, inOwnerCollection2, ownerTextBody1, bkmkStartNextItemIndex, bkmkEndPreviosItemIndex, isInSingleSection, (this.GetSection((Entity) ownerTextBody1) as WSection).CloneWithoutBodyItems());
      if (startSectionIndex - 1 != inOwnerCollection3)
        this.AddInBetweenSections(startSectionIndex, inOwnerCollection3, ownerTextBody1.Owner.Owner as WordDocument);
      bool IsFirstBkmkEnd = this.IsBkmkEndInFirstItem(paraEnd, (ParagraphItem) bkmkEnd, bkmkEndPreviosItemIndex);
      if (IsFirstBkmkEnd)
        --inOwnerCollection2;
      if (IsFirstBkmkEnd && isInSingleSection)
        return;
      this.AddLastSectionToDocumentPart(inOwnerCollection2, bkmkEndPreviosItemIndex, ownerTextBody2, IsFirstBkmkEnd, isInSingleSection);
    }
  }

  private void AddLastSectionToDocumentPart(
    int endParagraphIndex,
    int bkmkEndPreviosItemIndex,
    WTextBody endTextBody,
    bool IsFirstBkmkEnd,
    bool isInSingleSection)
  {
    if (!isInSingleSection)
      this.AddFirstSectionToDocumentPart(0, endParagraphIndex, endTextBody, 0, -1, true, (this.GetSection((Entity) endTextBody) as WSection).CloneWithoutBodyItems());
    if (IsFirstBkmkEnd)
      return;
    WSection wsection = isInSingleSection ? this.m_sections[0] : this.m_sections[this.m_sections.Count - 1];
    WParagraph wparagraph = (endTextBody.Items[endParagraphIndex] as WParagraph).Clone() as WParagraph;
    while (bkmkEndPreviosItemIndex + 1 < wparagraph.Items.Count)
      wparagraph.Items.RemoveFromInnerList(bkmkEndPreviosItemIndex + 1);
    wsection.Body.ChildEntities.Add((IEntity) wparagraph);
  }

  private void AddInBetweenSections(
    int startSectionIndex,
    int endSectionIndex,
    WordDocument document)
  {
    for (int index = startSectionIndex; index < endSectionIndex; ++index)
      this.m_sections.Add((IWSection) document.Sections[index].Clone());
  }

  private void AddFirstSectionToDocumentPart(
    int startParagraphIndex,
    int endParagraphIndex,
    WTextBody startTextBody,
    int bkmkStartNextItemIndex,
    int bkmkEndPreviosItemIndex,
    bool isInSingleSection,
    WSection section)
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
          section.Body.ChildEntities.Add((IEntity) wparagraph);
      }
      else
        section.Body.ChildEntities.Add((IEntity) childEntity.Clone());
    }
    this.m_sections.Add((IWSection) section);
  }

  private bool IsBkmkEndInFirstItem(
    WParagraph paragraph,
    ParagraphItem bkmkEnd,
    int bkmkEndPreviosItemIndex)
  {
    for (int index = 0; index <= bkmkEndPreviosItemIndex; ++index)
    {
      if (!(paragraph.Items[index] is BookmarkStart) && !(paragraph.Items[index] is BookmarkEnd))
        return false;
    }
    return true;
  }
}
