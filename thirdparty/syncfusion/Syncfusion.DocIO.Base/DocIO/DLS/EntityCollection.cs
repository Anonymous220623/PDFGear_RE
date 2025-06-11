// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.EntityCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.ReaderWriter;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public abstract class EntityCollection : 
  XDLSSerializableCollection,
  IEntityCollectionBase,
  ICollectionBase,
  IEnumerable
{
  internal EntityCollection.ChangeItemsHandlerList ChangeItemsHandlers = new EntityCollection.ChangeItemsHandlerList();
  private byte m_bFlags;

  public Entity this[int index] => this.InnerList[index] as Entity;

  public Entity FirstItem => this.Count <= 0 ? (Entity) null : this[0];

  public Entity LastItem => this.Count <= 0 ? (Entity) null : this[this.Count - 1];

  internal bool Joined => this.OwnerBase != null;

  internal bool IsNewEntityHasField
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal Entity Owner => (Entity) this.OwnerBase;

  protected abstract Type[] TypesOfElement { get; }

  internal EntityCollection(WordDocument doc)
    : this(doc, (Entity) null)
  {
  }

  internal EntityCollection(WordDocument doc, Entity owner)
    : base(doc, (OwnerHolder) owner)
  {
  }

  public int Add(IEntity entity)
  {
    switch (entity)
    {
      case null:
        throw new ArgumentNullException(nameof (entity));
      case BookmarkStart _:
      case BookmarkEnd _:
        if (this.Owner is InlineContentControl && !this.IsContentControlAllowBookmark(entity))
          throw new Exception($"Cannot add an object of type {entity.EntityType} into the {(this.Owner as InlineContentControl).ContentControlProperties.Type} Content Control");
        break;
    }
    int count = this.Count;
    if (!this.IsContentControlAllowParagraph() && entity is IWParagraph)
      throw new InvalidOperationException("Paragraph can't be added for CheckBox and Picture content control.");
    int index;
    if (this.m_doc != null && !this.m_doc.IsOpening && this.Owner is WParagraph && this.InnerList.Count > 0 && this.InnerList[count - 1] is BookmarkEnd)
    {
      index = this.GetIndexOfLastBookMarkEnd();
      this.Insert(index, entity);
    }
    else
    {
      this.OnInsert(count, (Entity) entity);
      index = this.OnInsertField(this.Count, (Entity) entity);
      this.AddToInnerList((Entity) entity);
      this.OnInsertComplete(index, (Entity) entity);
      this.UpdateParagraphTextForInlineControl(index, (Entity) entity);
      if (this.Document != null && !this.Document.IsFieldRangeAdding && !this.Document.IsOpening && !this.Document.IsMailMerge && !this.Document.IsCloning && this.IsNewEntityHasField)
      {
        this.UpdateFieldSeparatorAndEnd((Entity) entity);
        this.IsNewEntityHasField = false;
      }
      this.OnInsertFieldComplete(this.Count - 1, (Entity) entity);
      if (this.Document != null && this.Document.TrackChanges && !this.Document.IsOpening && !this.Document.IsMailMerge && !this.Document.IsCloning && !this.Document.IsClosing)
        this.UpdateTrackRevision(entity);
    }
    return index;
  }

  private bool IsContentControlAllowBookmark(IEntity entity)
  {
    if (!(this.Owner is InlineContentControl owner))
      return false;
    return owner.ContentControlProperties.Type == ContentControlType.RichText || owner.ContentControlProperties.Type == ContentControlType.Text || owner.ContentControlProperties.Type == ContentControlType.BuildingBlockGallery || owner.ContentControlProperties.Type == ContentControlType.RepeatingSection;
  }

  private bool IsContentControlAllowParagraph()
  {
    bool flag = false;
    if (this.Owner is IInlineContentControl)
    {
      ContentControlType type = (this.Owner as IInlineContentControl).ContentControlProperties.Type;
      flag = type == ContentControlType.CheckBox || type == ContentControlType.Picture;
    }
    else if (this.Owner is IBlockContentControl)
    {
      ContentControlType type = (this.Owner as IBlockContentControl).ContentControlProperties.Type;
      flag = type == ContentControlType.CheckBox || type == ContentControlType.Picture;
    }
    else if (this.Owner is ICellContentControl)
    {
      ContentControlType type = (this.Owner as ICellContentControl).ContentControlProperties.Type;
      flag = type == ContentControlType.CheckBox || type == ContentControlType.Picture;
    }
    return !flag;
  }

  private int GetIndexOfLastBookMarkEnd()
  {
    int count = this.InnerList.Count;
    while (count > 0 && this.InnerList[count - 1] is BookmarkEnd && this.InnerList[count - 1] is BookmarkEnd inner && (inner.IsAfterParagraphMark || inner.IsAfterCellMark || inner.IsAfterTableMark || inner.IsAfterRowMark))
      --count;
    return count;
  }

  public void Clear()
  {
    if (this.InnerList.Count > 0 && this[0].Owner is WSection)
      throw new ArgumentException("Cannot Clear objects from WSection.");
    this.OnClear();
    this.InnerList.Clear();
  }

  public bool Contains(IEntity entity)
  {
    if (entity == null)
      return false;
    int index = (entity as Entity).Index;
    return this.InnerList.Count > index && index >= 0 && this.InnerList[index] == entity;
  }

  public int IndexOf(IEntity entity) => this.Contains(entity) ? (entity as Entity).Index : -1;

  public void Insert(int index, IEntity entity)
  {
    switch (entity)
    {
      case null:
        throw new ArgumentNullException(nameof (entity));
      case BookmarkStart _:
      case BookmarkEnd _:
        if (this.Owner is InlineContentControl && !this.IsContentControlAllowBookmark(entity))
          throw new Exception($"Cannot insert an object of type {entity.EntityType} into the {(this.Owner as InlineContentControl).ContentControlProperties.Type} Content Control");
        break;
    }
    this.OnInsert(index, (Entity) entity);
    index = this.OnInsertField(index, (Entity) entity);
    this.InsertToInnerList(index, entity);
    this.OnInsertComplete(index, (Entity) entity);
    this.UpdateParagraphTextForInlineControl(index, (Entity) entity);
    if (this.Document != null && !this.Document.IsFieldRangeAdding && !this.Document.IsOpening && !this.Document.IsMailMerge && !this.Document.IsCloning && this.IsNewEntityHasField)
    {
      this.UpdateFieldSeparatorAndEnd((Entity) entity);
      this.IsNewEntityHasField = false;
    }
    this.OnInsertFieldComplete(index, (Entity) entity);
    if (this.Document == null || !this.Document.TrackChanges || this.Document.IsOpening || this.Document.IsMailMerge || this.Document.IsCloning || this.Document.IsClosing)
      return;
    this.UpdateTrackRevision(entity);
  }

  internal void UpdatePositionForGroupShape(Entity entity)
  {
    if (this.Document == null || this.Document.IsOpening || !(entity is GroupShape))
      return;
    (entity as GroupShape).UpdatePositionForGroupShapeAndChildShape();
  }

  internal void UpdateParagraphTextForInlineControl(int index, Entity entity)
  {
    if (index > this.Count - 1 || !(entity is InlineContentControl))
      return;
    this.UpdateTextFromInlineControl((entity as InlineContentControl).GetOwnerParagraphValue(), entity as InlineContentControl);
  }

  internal void InsertToInnerList(int index, IEntity entity)
  {
    this.InnerList.Insert(index, (object) entity);
    (entity as Entity).Index = index;
    this.UpdateIndex(index + 1, true);
  }

  internal void RemoveFromInnerList(int index)
  {
    this.InnerList.RemoveAt(index);
    this.UpdateIndex(index, false);
  }

  internal void AddToInnerList(Entity entity)
  {
    this.InnerList.Add((object) entity);
    entity.Index = this.InnerList.Count - 1;
  }

  internal void UpdateIndexForDuplicateEntity(int startIndex, bool isAdd)
  {
    int count = this.InnerList.Count;
    for (int index = startIndex; index < count; ++index)
    {
      if (this.InnerList[index] is Entity && (index <= 0 || this.InnerList[index] != this.InnerList[index - 1] || (this.InnerList[index - 1] as Entity).Index != index - 1))
      {
        if (isAdd)
          ++(this.InnerList[index] as Entity).Index;
        else
          --(this.InnerList[index] as Entity).Index;
      }
    }
  }

  internal void UpdateIndex(int startIndex, bool isAdd)
  {
    int count = this.InnerList.Count;
    for (int index = startIndex; index < count; ++index)
    {
      if (this.InnerList[index] is Entity)
      {
        if (isAdd)
          ++(this.InnerList[index] as Entity).Index;
        else
          --(this.InnerList[index] as Entity).Index;
      }
    }
  }

  public void Remove(IEntity entity)
  {
    if (entity.Owner is WSection)
      throw new ArgumentException($"Cannot remove an object of type {(object) entity.EntityType} from the {(object) entity.Owner.EntityType}");
    this.UpdateDocumentCollection(entity);
    this.OnRemove((entity as Entity).Index);
    this.RemoveFromInnerList((entity as Entity).Index);
  }

  public void RemoveAt(int index)
  {
    if ((this.InnerList[index] as IEntity).Owner is WSection)
      throw new ArgumentException($"Cannot remove an object of type {(object) (this.InnerList[index] as IEntity).EntityType} from the {(object) (this.InnerList[index] as IEntity).Owner.EntityType}");
    this.UpdateDocumentCollection(this.InnerList[index] as IEntity);
    this.OnRemove(index);
    this.RemoveFromInnerList(index);
  }

  private void UpdateDocumentCollection(IEntity entity)
  {
    if (this.Document == null)
      return;
    if (entity is ParagraphItem && (entity as ParagraphItem).GetCharFormat() != null)
      this.UpdateRevisionCollection((FormatBase) (entity as ParagraphItem).GetCharFormat());
    this.UpdateRevisionCollection(entity as Entity);
    if (entity is WParagraph)
    {
      WParagraph wparagraph = entity as WParagraph;
      this.UpdateRevisionCollection((FormatBase) wparagraph.ParagraphFormat);
      this.UpdateRevisionCollection((FormatBase) wparagraph.BreakCharacterFormat);
      if ((entity as WParagraph).Items == null)
        return;
      for (int index = 0; index < (entity as WParagraph).Items.Count; ++index)
      {
        ParagraphItem paragraphItem = (entity as WParagraph).Items[index];
        if (paragraphItem.GetCharFormat() != null)
          this.UpdateRevisionCollection((FormatBase) paragraphItem.GetCharFormat());
        this.UpdateRevisionCollection((Entity) paragraphItem);
        switch (paragraphItem)
        {
          case BookmarkStart _:
            Bookmark byName = this.Document.Bookmarks.FindByName((paragraphItem as BookmarkStart).Name);
            if (byName != null && !(paragraphItem as BookmarkStart).IsDetached)
            {
              if (byName.BookmarkStart != null)
                byName.BookmarkStart.IsDetached = true;
              if (byName.BookmarkEnd != null)
                byName.BookmarkEnd.IsDetached = true;
              this.Document.Bookmarks.InnerList.Remove((object) byName);
              break;
            }
            break;
          case WField _:
            this.Document.Fields.Remove(paragraphItem as WField);
            break;
          case WTextBox _:
            foreach (IEntity entity1 in (CollectionImpl) (paragraphItem as WTextBox).TextBoxBody.Items)
              this.UpdateDocumentCollection(entity1);
            this.Document.TextBoxes.Remove(paragraphItem as WTextBox);
            this.Document.FloatingItems.Remove((Entity) (paragraphItem as WTextBox));
            break;
          case Shape _:
            foreach (IEntity entity2 in (CollectionImpl) (paragraphItem as Shape).TextBody.Items)
              this.UpdateDocumentCollection(entity2);
            this.Document.AutoShapeCollection.Remove(paragraphItem as Shape);
            this.Document.FloatingItems.Remove((Entity) (paragraphItem as Shape));
            break;
          case GroupShape _:
            this.UpdateGroupShapeCollection(paragraphItem as GroupShape);
            break;
          case WPicture _:
            (paragraphItem as WPicture).RemoveImageInCollection();
            break;
          case WComment _:
            (paragraphItem as WComment).IsDetached = true;
            this.Document.Comments.InnerList.Remove((object) (paragraphItem as WComment));
            break;
        }
      }
    }
    if (entity is Shape)
    {
      foreach (IEntity entity3 in (CollectionImpl) (entity as Shape).TextBody.Items)
        this.UpdateDocumentCollection(entity3);
      this.Document.AutoShapeCollection.Remove(entity as Shape);
      this.Document.FloatingItems.Remove((Entity) (entity as Shape));
    }
    if (entity is GroupShape)
      this.UpdateGroupShapeCollection(entity as GroupShape);
    else if (entity is WTextBox)
    {
      if ((entity as WTextBox).TextBoxBody != null)
      {
        foreach (IEntity entity4 in (CollectionImpl) (entity as WTextBox).TextBoxBody.Items)
          this.UpdateDocumentCollection(entity4);
      }
      this.Document.TextBoxes.Remove(entity as WTextBox);
      this.Document.FloatingItems.Remove((Entity) (entity as WTextBox));
    }
    else if (entity is WTableCell)
    {
      this.UpdateRevisionCollection((FormatBase) (entity as WTableCell).CellFormat);
      foreach (IEntity entity5 in (CollectionImpl) ((WTextBody) entity).Items)
        this.UpdateDocumentCollection(entity5);
    }
    else if (entity is WTableRow)
    {
      this.UpdateRevisionCollection((FormatBase) (entity as WTableRow).RowFormat);
      foreach (WTableCell cell in (CollectionImpl) ((WTableRow) entity).Cells)
      {
        this.UpdateRevisionCollection((FormatBase) cell.CellFormat);
        foreach (IEntity entity6 in (CollectionImpl) cell.Items)
          this.UpdateDocumentCollection(entity6);
      }
    }
    else if (entity is WTable)
    {
      this.UpdateRevisionCollection((FormatBase) (entity as WTable).DocxTableFormat.Format);
      foreach (WTableRow row in (CollectionImpl) (entity as WTable).Rows)
      {
        this.UpdateRevisionCollection((FormatBase) row.RowFormat);
        foreach (WTableCell cell in (CollectionImpl) row.Cells)
        {
          this.UpdateRevisionCollection((FormatBase) cell.CellFormat);
          foreach (IEntity entity7 in (CollectionImpl) cell.Items)
            this.UpdateDocumentCollection(entity7);
        }
      }
    }
    else if (entity is BookmarkStart)
    {
      Bookmark byName = this.Document.Bookmarks.FindByName((entity as BookmarkStart).Name);
      if (byName == null || (entity as BookmarkStart).IsDetached)
        return;
      if (byName.BookmarkStart != null)
        byName.BookmarkStart.IsDetached = true;
      if (byName.BookmarkEnd != null)
        byName.BookmarkEnd.IsDetached = true;
      this.Document.Bookmarks.InnerList.Remove((object) byName);
    }
    else
    {
      if (!(entity is WSection))
        return;
      this.UpdateRevisionCollection((FormatBase) (entity as WSection).SectionFormat);
      foreach (WTextBody childEntity in (CollectionImpl) ((WSection) entity).ChildEntities)
      {
        foreach (IEntity entity8 in (CollectionImpl) childEntity.Items)
          this.UpdateDocumentCollection(entity8);
      }
    }
  }

  private void UpdateGroupShapeCollection(GroupShape groupShape)
  {
    foreach (ParagraphItem childShape in (CollectionImpl) groupShape.ChildShapes)
    {
      switch (childShape)
      {
        case ChildGroupShape _:
          IEnumerator enumerator1 = (childShape as ChildGroupShape).ChildShapes.GetEnumerator();
          try
          {
            while (enumerator1.MoveNext())
            {
              ChildShape current = (ChildShape) enumerator1.Current;
              if (current is ChildGroupShape)
              {
                foreach (IEntity entity in (CollectionImpl) (current as ChildGroupShape).TextBody.Items)
                  this.UpdateDocumentCollection(entity);
              }
            }
            break;
          }
          finally
          {
            if (enumerator1 is IDisposable disposable)
              disposable.Dispose();
          }
        case ChildShape _:
          IEnumerator enumerator2 = (childShape as ChildShape).TextBody.Items.GetEnumerator();
          try
          {
            while (enumerator2.MoveNext())
              this.UpdateDocumentCollection((IEntity) enumerator2.Current);
            break;
          }
          finally
          {
            if (enumerator2 is IDisposable disposable)
              disposable.Dispose();
          }
      }
      this.Document.FloatingItems.Remove((Entity) groupShape);
    }
  }

  private void UpdateRevisionCollection(Entity entity)
  {
    if (this.Document == null || !this.Document.TrackChanges || this.Document.IsOpening || this.Document.IsMailMerge || this.Document.IsCloning || this.Document.IsClosing)
      return;
    for (int index = 0; index < entity.RevisionsInternal.Count; ++index)
    {
      Revision revision = entity.RevisionsInternal[index];
      if (revision.Range.Count > 0 && revision.Range.InnerList.Contains((object) entity))
      {
        revision.Range.InnerList.Remove((object) entity);
        entity.RevisionsInternal.Remove(revision);
        --index;
      }
      if (revision.Range.Count == 0)
        revision.RemoveSelf();
    }
  }

  private void UpdateRevisionCollection(FormatBase formatBase)
  {
    if (this.Document == null || !this.Document.TrackChanges || this.Document.IsOpening || this.Document.IsMailMerge || this.Document.IsCloning || this.Document.IsClosing)
      return;
    for (int index = 0; index < formatBase.Revisions.Count; ++index)
    {
      Revision revision = formatBase.Revisions[index];
      if (revision.Range.Count > 0 && revision.Range.InnerList.Contains((object) formatBase))
      {
        revision.Range.InnerList.Remove((object) formatBase);
        formatBase.Revisions.Remove(revision);
        --index;
      }
      if (revision.Range.Count == 0)
        revision.RemoveSelf();
    }
  }

  internal void UpdateTrackRevision(IEntity entity)
  {
    switch (entity)
    {
      case ParagraphItem _:
        this.UpdateTrackRevision(entity as ParagraphItem);
        break;
      case TextBodyItem _:
        this.UpdateTrackRevision(entity as TextBodyItem);
        break;
      case WSection _:
        this.UpdateTrackRevision(entity as WSection);
        break;
      case WordDocument _:
        IEnumerator enumerator = ((WordDocument) entity).ChildEntities.GetEnumerator();
        try
        {
          while (enumerator.MoveNext())
            this.UpdateTrackRevision((WSection) enumerator.Current);
          break;
        }
        finally
        {
          if (enumerator is IDisposable disposable)
            disposable.Dispose();
        }
      case WTableCell _:
        if (((WTableCell) entity).CellFormat.HasKey(15))
          this.Document.UpdateCellFormatRevision((WTableCell) entity);
        this.UpdateTrackRevision((WTextBody) entity);
        break;
      case WTableRow _:
        this.UpdateRowRevision(entity as WTableRow);
        foreach (WTableCell cell in (CollectionImpl) ((WTableRow) entity).Cells)
        {
          if (cell.CellFormat.HasKey(15))
            this.Document.UpdateCellFormatRevision(cell);
          this.UpdateTrackRevision((WTextBody) cell);
        }
        this.Document.UpdateTableFormatRevision(entity as WTableRow);
        break;
    }
  }

  private void UpdateTrackRevision(WSection section)
  {
    this.Document.SectionFormatChange(section);
    foreach (WTextBody childEntity in (CollectionImpl) section.ChildEntities)
    {
      foreach (TextBodyItem bodyItemEntity in (CollectionImpl) childEntity.Items)
        this.UpdateTrackRevision(bodyItemEntity);
    }
  }

  private void UpdateTrackRevision(TextBodyItem bodyItemEntity)
  {
    switch (bodyItemEntity.EntityType)
    {
      case EntityType.Paragraph:
        this.UpdateParagraphRevision(bodyItemEntity as WParagraph);
        break;
      case EntityType.BlockContentControl:
        this.UpdateTrackRevision((bodyItemEntity as BlockContentControl).TextBody);
        break;
      case EntityType.Table:
        this.UpdateTrackRevision(bodyItemEntity as WTable);
        break;
    }
  }

  private void UpdateTrackRevision(ParagraphItem paraItem)
  {
    this.UpdateParaItemRevision(paraItem);
    switch (paraItem)
    {
      case InlineContentControl _:
        this.UpdateTrackRevision((paraItem as InlineContentControl).ParagraphItems);
        break;
      case WTextBox _:
        this.UpdateTrackRevision((paraItem as WTextBox).TextBoxBody);
        break;
      case Shape _:
        this.UpdateTrackRevision((paraItem as Shape).TextBody);
        break;
    }
  }

  private void UpdateTrackRevision(ParagraphItemCollection paraItems)
  {
    for (int index = 0; index < paraItems.Count; ++index)
      this.UpdateTrackRevision(paraItems[index]);
  }

  private void UpdateTrackRevision(WTextBody textBody)
  {
    for (int index = 0; index < textBody.ChildEntities.Count; ++index)
      this.UpdateTrackRevision(textBody.ChildEntities[index] as TextBodyItem);
  }

  private void UpdateTrackRevision(WTable table)
  {
    if (table.DocxTableFormat.Format.HasKey(15))
      this.Document.UpdateTableRevision(table);
    foreach (WTableRow row in (CollectionImpl) table.Rows)
    {
      this.UpdateRowRevision(row);
      foreach (WTableCell cell in (CollectionImpl) row.Cells)
      {
        if (cell.CellFormat.HasKey(15))
          this.Document.UpdateCellFormatRevision(cell);
        this.UpdateTrackRevision((WTextBody) cell);
      }
      this.Document.UpdateTableFormatRevision(row);
    }
  }

  private void UpdateParaItemRevision(ParagraphItem paraItem)
  {
    WCharacterFormat charFormat = paraItem.GetCharFormat();
    if (charFormat == null)
      return;
    if (charFormat.HasKey(105))
      this.Document.CharacterFormatChange(charFormat, paraItem, (WordReaderBase) null);
    if (charFormat.IsInsertRevision)
      this.Document.ParagraphItemRevision(paraItem, RevisionType.Insertions, charFormat.AuthorName, charFormat.RevDateTime, (string) null, true, (Revision) null, (Revision) null, (Stack<Revision>) null);
    if (!charFormat.IsDeleteRevision)
      return;
    this.Document.ParagraphItemRevision(paraItem, RevisionType.Deletions, charFormat.AuthorName, charFormat.RevDateTime, (string) null, true, (Revision) null, (Revision) null, (Stack<Revision>) null);
  }

  private void UpdateParagraphRevision(WParagraph paragraph)
  {
    this.Document.ParagraphFormatChange(paragraph.ParagraphFormat);
    this.Document.CharacterFormatChange(paragraph.BreakCharacterFormat, (ParagraphItem) null, (WordReaderBase) null);
    this.UpdateTrackRevision(paragraph.Items);
    this.Document.UpdateLastItemRevision((IWParagraph) paragraph, paragraph.Items);
  }

  private void UpdateRowRevision(WTableRow row)
  {
    if (row.RowFormat.HasKey(122))
      this.Document.UpdateRowFormatRevision(row.RowFormat);
    if (row.IsInsertRevision)
      this.Document.TableRowRevision(RevisionType.Insertions, row, (WordReaderBase) null);
    if (!row.IsDeleteRevision)
      return;
    this.Document.TableRowRevision(RevisionType.Deletions, row, (WordReaderBase) null);
  }

  internal Entity NextSibling(Entity entity)
  {
    int index = entity.Index;
    return index < 0 || index > this.Count - 2 ? (Entity) null : this[index + 1];
  }

  internal Entity PreviousSibling(Entity entity)
  {
    int index = entity.Index;
    return index < 1 || index > this.Count - 1 ? (Entity) null : this[index - 1];
  }

  internal int GetNextOrPrevIndex(int index, EntityType type, bool next)
  {
    do
    {
      index += next ? 1 : -1;
      if (index > this.InnerList.Count - 1 || index < 0)
        return -1;
    }
    while ((this.InnerList[index] as Entity).EntityType != type);
    return index;
  }

  internal void InternalClearBy(EntityType type)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      Entity entity = this[index];
      if (entity.EntityType == type)
      {
        entity.SetOwner((OwnerHolder) null);
        this.RemoveFromInnerList(index);
        --index;
      }
    }
  }

  internal void CloneTo(EntityCollection destColl)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      destColl.Add((IEntity) this[index].Clone());
  }

  protected virtual void OnClear()
  {
    for (int index = 0; index < this.Count; ++index)
    {
      Entity entity = this[index];
      this.UpdateDocumentCollection((IEntity) entity);
      entity.SetOwner((OwnerHolder) null);
      entity.Index = -1;
    }
    this.ChangeItemsHandlers.Send(EntityCollection.ChangeItemsType.Clear, (Entity) null);
  }

  protected virtual void OnInsert(int index, Entity entity)
  {
    if (!this.IsCorrectElementType(entity))
      throw new ArgumentException($"Cannot insert an object of type {entity.EntityType} into the {this.Owner.EntityType}");
    if (this.Joined)
    {
      bool flag = entity.Owner == null;
      int num = this.Owner.DeepDetached ? 1 : 0;
      WordDocument document = entity.Document;
      if (this.Document != document)
      {
        if (!flag && !entity.DeepDetached)
          throw new InvalidOperationException("You can not add no clonned entity from other document");
        entity.CloneRelationsTo(this.Document, (OwnerHolder) this.Owner);
        if (!string.IsNullOrEmpty(this.Document.Settings.DuplicateListStyleNames) && !this.Document.Settings.MaintainImportedListCache && this.Document.IsCloning)
          this.Document.Settings.DuplicateListStyleNames = string.Empty;
      }
      else if (this.Document.IsMailMerge && entity is WParagraph && (this.Document.ImportOptions & ImportOptions.ListRestartNumbering) != (ImportOptions) 0)
        (entity as WParagraph).ListFormat.CloneListRelationsTo(this.Document, (string) null);
      if (!flag)
      {
        if (!this.Document.Settings.DisableMovingEntireField)
          this.UpdateFieldRange(entity);
        this.Document.IsSkipFieldDetach = true;
        entity.RemoveSelf();
        this.Document.IsSkipFieldDetach = false;
      }
      else if (!this.Document.IsOpening && !this.Document.IsCloning && this.Document == document)
        entity.AddSelf();
      if (entity.EntityType == EntityType.TextBox && (entity as WTextBox).Shape != null)
        (entity as WTextBox).Shape.SetOwner((OwnerHolder) this.Owner);
      entity.SetOwner((OwnerHolder) this.Owner);
    }
    this.ChangeItemsHandlers.Send(EntityCollection.ChangeItemsType.Add, entity);
  }

  private void UpdateFieldRange(Entity entity)
  {
    switch (entity)
    {
      case WParagraph _:
        for (int index = 0; index < (entity as WParagraph).Items.Count; ++index)
        {
          ParagraphItem paragraphItem = (entity as WParagraph).Items[index];
          if (paragraphItem is WField && (paragraphItem as WField).FieldEnd != null && (paragraphItem as WField).FieldEnd.OwnerParagraph != entity)
          {
            WField wfield = paragraphItem as WField;
            wfield.Range.Items.Clear();
            wfield.IsFieldRangeUpdated = false;
            wfield.UpdateFieldRange();
            this.IsNewEntityHasField = true;
            break;
          }
        }
        break;
      case WField _:
        if ((entity as WField).FieldEnd == null || (entity as WField).FieldEnd.OwnerParagraph == this.Owner)
          break;
        WField wfield1 = entity as WField;
        wfield1.Range.Items.Clear();
        wfield1.IsFieldRangeUpdated = false;
        wfield1.UpdateFieldRange();
        this.IsNewEntityHasField = true;
        break;
    }
  }

  private void UpdateFieldSeparatorAndEnd(Entity entity)
  {
    switch (entity)
    {
      case WParagraph _:
        for (int index = 0; index < (entity as WParagraph).Items.Count; ++index)
        {
          ParagraphItem field = (entity as WParagraph).Items[index];
          if (field is WField && (field as WField).FieldEnd != null && (field as WField).FieldEnd.OwnerParagraph != entity)
            this.InsertFieldRange(field as WField, (entity as WParagraph).GetIndexInOwnerCollection() + 1, index + 1, true);
        }
        break;
      case WField _:
        if ((entity as WField).FieldEnd == null || (entity as WField).FieldEnd.OwnerParagraph == this.Owner)
          break;
        if (this.Owner is WParagraph)
        {
          this.InsertFieldRange(entity as WField, (this.Owner as WParagraph).GetIndexInOwnerCollection() + 1, (entity as WField).GetIndexInOwnerCollection() + 1, false);
          break;
        }
        if (!(this.Owner is InlineContentControl) || (this.Owner as InlineContentControl).GetOwnerParagraphValue() == null)
          break;
        this.InsertFieldRange(entity as WField, (this.Owner as InlineContentControl).GetOwnerParagraphValue().GetIndexInOwnerCollection() + 1, (entity as WField).GetIndexInOwnerCollection() + 1, false);
        break;
    }
  }

  private void InsertFieldRange(
    WField field,
    int bodyItemIndex,
    int paraItemIndex,
    bool isSkipParaItems)
  {
    this.Document.IsFieldRangeAdding = true;
    WParagraph ownerParagraph = field.OwnerParagraph;
    for (int index1 = 0; index1 < field.Range.Items.Count; ++index1)
    {
      Entity entity = field.Range.Items[index1] as Entity;
      switch (entity)
      {
        case ParagraphItem _ when this.Owner is WParagraph && !isSkipParaItems:
          (this.Owner as WParagraph).ChildEntities.Insert(paraItemIndex, (IEntity) entity);
          ++paraItemIndex;
          break;
        case TextBodyItem _:
          if (index1 == field.Range.Items.Count - 1)
          {
            if (this.Owner is WParagraph && paraItemIndex < (this.Owner as WParagraph).ChildEntities.Count)
            {
              WParagraph wparagraph1 = new WParagraph((IWordDocument) this.Document);
              WParagraph wparagraph2 = entity as WParagraph;
              int inOwnerCollection = field.FieldEnd.GetIndexInOwnerCollection();
              for (int index2 = 0; index2 <= inOwnerCollection; ++index2)
                wparagraph1.ChildEntities.Add((IEntity) wparagraph2.ChildEntities[0]);
              ownerParagraph.OwnerTextBody.ChildEntities.Insert(bodyItemIndex, (IEntity) wparagraph1);
              int count = (this.Owner as WParagraph).ChildEntities.Count;
              for (int index3 = paraItemIndex; index3 < count; ++index3)
                wparagraph1.ChildEntities.Add((IEntity) (this.Owner as WParagraph).ChildEntities[paraItemIndex]);
              break;
            }
            if (ownerParagraph.OwnerTextBody.ChildEntities.Count > 0 && ownerParagraph.OwnerTextBody.ChildEntities.Count > bodyItemIndex && ownerParagraph.OwnerTextBody.ChildEntities[bodyItemIndex] is WParagraph)
            {
              WParagraph childEntity = ownerParagraph.OwnerTextBody.ChildEntities[bodyItemIndex] as WParagraph;
              WParagraph wparagraph = entity as WParagraph;
              int inOwnerCollection = field.FieldEnd.GetIndexInOwnerCollection();
              for (int index4 = 0; index4 <= inOwnerCollection; ++index4)
                childEntity.ChildEntities.Insert(index4, (IEntity) wparagraph.ChildEntities[0]);
              break;
            }
            WParagraph wparagraph3 = new WParagraph((IWordDocument) this.Document);
            WParagraph wparagraph4 = entity as WParagraph;
            int inOwnerCollection1 = field.FieldEnd.GetIndexInOwnerCollection();
            for (int index5 = 0; index5 <= inOwnerCollection1; ++index5)
              wparagraph3.ChildEntities.Add((IEntity) wparagraph4.ChildEntities[0]);
            ownerParagraph.OwnerTextBody.ChildEntities.Insert(bodyItemIndex, (IEntity) wparagraph3);
            break;
          }
          if (ownerParagraph.OwnerTextBody.ChildEntities.Contains((IEntity) entity) && ownerParagraph.OwnerTextBody.ChildEntities.IndexOf((IEntity) entity) < bodyItemIndex)
            --bodyItemIndex;
          ownerParagraph.OwnerTextBody.ChildEntities.Insert(bodyItemIndex, (IEntity) entity);
          ++bodyItemIndex;
          break;
      }
    }
    this.Document.IsFieldRangeAdding = false;
  }

  protected virtual void OnInsertComplete(int index, Entity entity)
  {
    if (!this.Joined || this.Owner.DeepDetached)
      return;
    if (entity.Owner is WParagraph)
    {
      switch (entity)
      {
        case BookmarkStart _:
          return;
        case BookmarkEnd _:
          return;
        case EditableRangeStart _:
          return;
        case EditableRangeEnd _:
          return;
      }
    }
    entity.AttachToDocument();
  }

  protected virtual void OnRemove(int index)
  {
    Entity entity = this[index];
    entity.SetOwner((OwnerHolder) null);
    this.ChangeItemsHandlers.Send(EntityCollection.ChangeItemsType.Remove, entity);
  }

  private bool IsCorrectElementType(Entity entity)
  {
    bool flag = false;
    foreach (Type type in this.TypesOfElement)
    {
      flag = type.IsInstanceOfType((object) entity);
      if (flag)
        break;
    }
    return flag;
  }

  private int OnInsertField(int index, Entity entity)
  {
    if (this.m_doc != null && !this.m_doc.IsOpening)
    {
      switch (entity)
      {
        case WFormField _ when !this.m_doc.IsHTMLImport && !this.m_doc.IsSkipFieldDetach:
          index = this.OnInsertFormField(index, entity);
          if ((entity as WField).FieldEnd != null)
          {
            this.Document.ClonedFields.Push(entity as WField);
            break;
          }
          break;
        case WField _ when (entity as WField).FieldEnd != null:
          this.Document.ClonedFields.Push(entity as WField);
          break;
      }
    }
    return index;
  }

  private void OnInsertFieldComplete(int index, Entity entity)
  {
    if (this.m_doc == null || this.m_doc.IsOpening)
      return;
    if (entity is WFormField && (entity as WFormField).FieldEnd == null && !this.m_doc.IsHTMLImport && !this.m_doc.IsSkipFieldDetach)
      this.OnInsertFormFieldComplete(index, entity);
    else if (this.IsMergeFieldNeedToBeUpdated(entity))
    {
      this.OnMergeFieldComplete(index, entity);
    }
    else
    {
      switch (entity)
      {
        case WField _ when !string.IsNullOrEmpty((entity as WField).m_detachedFieldCode) && !this.m_doc.IsInternalManipulation():
          WTextRange wtextRange = new WTextRange((IWordDocument) this.m_doc);
          wtextRange.Text = (entity as WField).m_detachedFieldCode;
          (entity as WField).m_detachedFieldCode = string.Empty;
          wtextRange.ApplyCharacterFormat((entity as WField).CharacterFormat);
          this.Insert(++index, (IEntity) wtextRange);
          break;
        case WFieldMark _ when this.Document.ClonedFields.Count > 0:
          WField wfield = this.Document.ClonedFields.Peek();
          if ((entity as WFieldMark).Type == FieldMarkType.FieldSeparator)
          {
            wfield.FieldSeparator = entity as WFieldMark;
            break;
          }
          this.Document.ClonedFields.Pop().FieldEnd = entity as WFieldMark;
          break;
      }
    }
  }

  private bool IsMergeFieldNeedToBeUpdated(Entity entity)
  {
    return entity is WMergeField && ((entity as WField).FieldEnd == null || !((entity as WField).FieldEnd.Owner is WParagraph)) && !this.m_doc.IsMailMerge && !this.m_doc.IsCloning && !this.m_doc.IsHTMLImport && !this.m_doc.IsSkipFieldDetach;
  }

  private void OnMergeFieldComplete(int fieldIndex, Entity entity)
  {
    WMergeField wmergeField = entity as WMergeField;
    WFieldMark wfieldMark1 = new WFieldMark((IWordDocument) this.m_doc, FieldMarkType.FieldSeparator);
    WFieldMark wfieldMark2 = new WFieldMark((IWordDocument) this.m_doc, FieldMarkType.FieldEnd);
    WTextRange wtextRange = new WTextRange((IWordDocument) this.m_doc);
    if (string.IsNullOrEmpty(wmergeField.m_detachedFieldCode))
    {
      wtextRange.Text = wmergeField.FindFieldCode();
    }
    else
    {
      wtextRange.Text = wmergeField.m_detachedFieldCode;
      wmergeField.m_detachedFieldCode = string.Empty;
    }
    (this.Owner as WParagraph).Items.Insert(++fieldIndex, (IEntity) wtextRange);
    if (wmergeField.FieldSeparator == null || !(wmergeField.FieldSeparator.Owner is WParagraph))
    {
      wmergeField.FieldSeparator = wfieldMark1;
      (this.Owner as WParagraph).Items.Insert(fieldIndex + 1, (IEntity) wfieldMark1);
    }
    fieldIndex = wmergeField.FieldSeparator.GetIndexInOwnerCollection();
    wmergeField.FieldEnd = wfieldMark2;
    (this.Owner as WParagraph).Items.Insert(fieldIndex + 1, (IEntity) wfieldMark2);
    wmergeField.UpdateMergeFieldResult();
  }

  private int OnInsertFormField(int index, Entity entity)
  {
    switch ((entity as WFormField).FormFieldType)
    {
      case FormFieldType.TextInput:
        WTextFormField wtextFormField = entity as WTextFormField;
        if (wtextFormField.Name == null || wtextFormField.Name == string.Empty)
        {
          string str = "Text_" + Guid.NewGuid().ToString().Replace("-", "_");
          wtextFormField.Name = str.Substring(0, 20);
        }
        if (wtextFormField.DefaultText == null || wtextFormField.DefaultText == string.Empty)
        {
          wtextFormField.DefaultText = "     ";
          break;
        }
        break;
      case FormFieldType.CheckBox:
        WCheckBox wcheckBox = entity as WCheckBox;
        if (wcheckBox.Name == null || wcheckBox.Name == string.Empty)
        {
          string str = "Check_" + Guid.NewGuid().ToString().Replace("-", "_");
          wcheckBox.Name = str.Substring(0, 20);
          break;
        }
        break;
      case FormFieldType.DropDown:
        WDropDownFormField wdropDownFormField = entity as WDropDownFormField;
        if (wdropDownFormField.Name == null || wdropDownFormField.Name == string.Empty)
        {
          string str = "Drop_" + Guid.NewGuid().ToString().Replace("-", "_");
          wdropDownFormField.Name = str.Substring(0, 20);
          break;
        }
        break;
    }
    if ((this.Owner as WParagraph).Items.Count > 0 && ((this.Owner as WParagraph).LastItem is BookmarkStart && ((this.Owner as WParagraph).LastItem as BookmarkStart).Name == (entity as WFormField).Name || index < (this.Owner as WParagraph).Items.Count && index > 0 && (this.Owner as WParagraph).Items[index - 1] is BookmarkStart && ((this.Owner as WParagraph).Items[index - 1] as BookmarkStart).Name == (entity as WFormField).Name))
      return index;
    (this.Owner as WParagraph).CheckFormFieldName((entity as WFormField).Name);
    index = index < this.InnerList.Count ? index : this.InnerList.Count;
    (this.Owner as WParagraph).Items.Insert(index, (IEntity) new BookmarkStart((IWordDocument) this.Document, (entity as WFormField).Name));
    ++index;
    return index;
  }

  internal void OnInsertFormFieldComplete(int index, Entity entity)
  {
    WFieldMark wfieldMark1 = new WFieldMark((IWordDocument) this.m_doc, FieldMarkType.FieldSeparator);
    WFieldMark wfieldMark2 = new WFieldMark((IWordDocument) this.m_doc, FieldMarkType.FieldEnd);
    WParagraph owner = this.Owner as WParagraph;
    WTextRange wtextRange = new WTextRange((IWordDocument) this.m_doc);
    wtextRange.Text = (entity as WFormField).FieldCode;
    wtextRange.ApplyCharacterFormat((entity as WFormField).CharacterFormat);
    owner.Items.Insert(++index, (IEntity) wtextRange);
    if (entity is WTextFormField)
    {
      (entity as WTextFormField).FieldSeparator = wfieldMark1;
      owner.Items.Insert(++index, (IEntity) wfieldMark1);
      if ((entity as WTextFormField).TextRange.Owner == null)
        owner.Items.Insert(++index, (IEntity) (entity as WTextFormField).TextRange);
      (entity as WTextFormField).FieldEnd = wfieldMark2;
      owner.Items.Insert(++index, (IEntity) wfieldMark2);
      owner.Items.Insert(++index, (IEntity) new BookmarkEnd((IWordDocument) this.Document, (entity as WFormField).Name));
    }
    else
    {
      (entity as WFormField).FieldEnd = wfieldMark2;
      owner.Items.Insert(++index, (IEntity) wfieldMark2);
      owner.Items.Insert(++index, (IEntity) new BookmarkEnd((IWordDocument) this.Document, (entity as WFormField).Name));
    }
  }

  private void UpdateTextFromInlineControl(
    WParagraph ownerPara,
    InlineContentControl inlineContentControl)
  {
    foreach (ParagraphItem paragraphItem in (CollectionImpl) inlineContentControl.ParagraphItems)
    {
      if (paragraphItem is WTextRange)
        (paragraphItem as WTextRange).InsertTextInParagraphText(ownerPara);
      else if (paragraphItem is InlineContentControl)
        this.UpdateTextFromInlineControl(paragraphItem.GetOwnerParagraphValue(), paragraphItem as InlineContentControl);
    }
  }

  public enum ChangeItemsType
  {
    Add,
    Remove,
    Clear,
  }

  public delegate void ChangeItems(EntityCollection.ChangeItemsType type, Entity entity);

  internal class ChangeItemsHandlerList : IEnumerable
  {
    private List<EntityCollection.ChangeItems> m_list = new List<EntityCollection.ChangeItems>();

    public void Add(EntityCollection.ChangeItems handler)
    {
      if (this.m_list.Contains(handler))
        throw new ArgumentException("handler already exists");
      this.m_list.Add(handler);
    }

    public void Remove(EntityCollection.ChangeItems handler)
    {
      if (!this.m_list.Contains(handler))
        throw new ArgumentException("handler not exists");
      this.m_list.Remove(handler);
    }

    public IEnumerator GetEnumerator() => (IEnumerator) this.m_list.GetEnumerator();

    public void Send(EntityCollection.ChangeItemsType type, Entity entity)
    {
      foreach (Delegate @delegate in this.m_list)
        @delegate.DynamicInvoke((object) type, (object) entity);
    }
  }
}
