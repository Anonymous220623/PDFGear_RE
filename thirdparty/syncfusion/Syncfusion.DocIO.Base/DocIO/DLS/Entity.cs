// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Entity
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public abstract class Entity(WordDocument doc, Entity owner) : XDLSSerializableBase(doc, owner), IEntity
{
  internal int Index = -1;
  internal List<Revision> m_revisions;

  public Entity Owner => (Entity) this.OwnerBase;

  public abstract EntityType EntityType { get; }

  public IEntity NextSibling
  {
    get
    {
      if (this.Owner is ICompositeEntity owner)
        return (IEntity) owner.ChildEntities.NextSibling(this);
      if (this.Owner is InlineContentControl)
        return (IEntity) (this.Owner as InlineContentControl).ParagraphItems.NextSibling(this);
      return !(this.Owner is XmlParagraphItem) || ((XmlParagraphItem) this.Owner).MathParaItemsCollection == null ? (IEntity) null : (IEntity) ((XmlParagraphItem) this.Owner).MathParaItemsCollection.NextSibling(this);
    }
  }

  public IEntity PreviousSibling
  {
    get
    {
      if (this.Owner is ICompositeEntity owner)
        return (IEntity) owner.ChildEntities.PreviousSibling(this);
      if (this.Owner is InlineContentControl)
        return (IEntity) (this.Owner as InlineContentControl).ParagraphItems.PreviousSibling(this);
      return !(this.Owner is XmlParagraphItem) || ((XmlParagraphItem) this.Owner).MathParaItemsCollection == null ? (IEntity) null : (IEntity) ((XmlParagraphItem) this.Owner).MathParaItemsCollection.NextSibling(this);
    }
  }

  public bool IsComposite => this is ICompositeEntity;

  internal bool DeepDetached
  {
    get
    {
      if (this.EntityType == EntityType.WordDocument)
        return false;
      return this.Owner == null || this.Owner.DeepDetached;
    }
  }

  internal bool IsFloatingItem(bool isTextWrapAround)
  {
    TextWrappingStyle textWrappingStyle = TextWrappingStyle.Inline;
    switch (this)
    {
      case WPicture _:
        textWrappingStyle = (this as WPicture).TextWrappingStyle;
        break;
      case Shape _:
        textWrappingStyle = (this as Shape).WrapFormat.TextWrappingStyle;
        break;
      case WTextBox _:
        textWrappingStyle = (this as WTextBox).TextBoxFormat.TextWrappingStyle;
        break;
      case GroupShape _:
        textWrappingStyle = (this as GroupShape).WrapFormat.TextWrappingStyle;
        break;
      case WChart _:
        textWrappingStyle = (this as WChart).WrapFormat.TextWrappingStyle;
        break;
    }
    if (!isTextWrapAround)
      return textWrappingStyle != TextWrappingStyle.Inline;
    return textWrappingStyle != TextWrappingStyle.Inline && textWrappingStyle != TextWrappingStyle.Behind && textWrappingStyle != TextWrappingStyle.InFrontOfText;
  }

  internal bool IsBuiltInCharacterStyle(BuiltinStyle builtInStyle)
  {
    return builtInStyle == BuiltinStyle.FootnoteReference || builtInStyle == BuiltinStyle.CommentReference || builtInStyle == BuiltinStyle.LineNumber || builtInStyle == BuiltinStyle.PageNumber || builtInStyle == BuiltinStyle.EndnoteReference || builtInStyle == BuiltinStyle.EndnoteText || builtInStyle == BuiltinStyle.Hyperlink || builtInStyle == BuiltinStyle.FollowedHyperlink || builtInStyle == BuiltinStyle.Strong || builtInStyle == BuiltinStyle.Emphasis || builtInStyle == BuiltinStyle.HtmlAcronym || builtInStyle == BuiltinStyle.HtmlCite || builtInStyle == BuiltinStyle.HtmlCode || builtInStyle == BuiltinStyle.HtmlDefinition || builtInStyle == BuiltinStyle.HtmlKeyboard || builtInStyle == BuiltinStyle.HtmlSample || builtInStyle == BuiltinStyle.HtmlTypewriter || builtInStyle == BuiltinStyle.HtmlVariable;
  }

  internal List<Revision> RevisionsInternal
  {
    get
    {
      if (this.m_revisions == null)
        this.m_revisions = new List<Revision>();
      return this.m_revisions;
    }
  }

  public Entity Clone() => (Entity) this.CloneImpl();

  internal static bool IsVerticalTextDirection(TextDirection textDirection)
  {
    return textDirection == TextDirection.VerticalTopToBottom || textDirection == TextDirection.VerticalFarEast || textDirection == TextDirection.VerticalBottomToTop;
  }

  internal virtual void AddSelf()
  {
  }

  internal virtual void AttachToDocument()
  {
    if (!(this is ICompositeEntity compositeEntity))
      return;
    EntityCollection childEntities = compositeEntity.ChildEntities;
    int index = 0;
    for (int count = childEntities.Count; index < count; ++index)
    {
      childEntities[index].AttachToDocument();
      if (childEntities.Count < count)
      {
        --index;
        --count;
      }
    }
  }

  internal virtual void RemoveSelf()
  {
    if (this.Owner is ICompositeEntity owner)
    {
      owner.ChildEntities.Remove((IEntity) this);
    }
    else
    {
      if (!(this.Owner is InlineContentControl))
        return;
      (this.Owner as InlineContentControl).ParagraphItems.Remove((IEntity) this);
    }
  }

  internal int GetIndexInOwnerCollection()
  {
    if (this.Owner is ICompositeEntity owner)
      return owner.ChildEntities.IndexOf((IEntity) this);
    return this.Owner is InlineContentControl ? (this.Owner as InlineContentControl).ParagraphItems.IndexOf((IEntity) this) : -1;
  }

  internal bool IsParentOf(Entity entity)
  {
    bool flag = false;
    for (OwnerHolder ownerBase = entity.OwnerBase; ownerBase != null; ownerBase = ownerBase.OwnerBase)
    {
      if (ownerBase == this)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  internal virtual void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
  }

  internal Entity GetOwnerTextBody(Entity entity)
  {
    Entity ownerTextBody = entity;
    while (true)
    {
      switch (ownerTextBody)
      {
        case WSection _:
        case HeaderFooter _:
        case WComment _:
        case WFootnote _:
          goto label_5;
        default:
          if (ownerTextBody.Owner != null)
          {
            ownerTextBody = ownerTextBody.Owner;
            continue;
          }
          goto label_2;
      }
    }
label_2:
    return ownerTextBody;
label_5:
    return ownerTextBody;
  }

  internal Entity GetOwnerShape(Entity entity)
  {
    Entity ownerShape = entity;
    while (ownerShape.Owner != null)
    {
      ownerShape = ownerShape.Owner;
      if (ownerShape is WTextBox || ownerShape is Shape)
        return ownerShape;
    }
    return (Entity) null;
  }

  internal Entity GetBaseEntity(Entity entity)
  {
    Entity baseEntity = entity;
    while (baseEntity.Owner != null)
    {
      baseEntity = baseEntity.Owner;
      if (baseEntity is WSection || baseEntity is HeaderFooter)
        return baseEntity;
    }
    return baseEntity;
  }

  internal string GetHierarchicalIndex(string hierarchicalIndex)
  {
    if (this.Owner is ICompositeEntity)
    {
      if (this is HeaderFooter)
        return (string) null;
      hierarchicalIndex = $"{this.GetIndexInOwnerCollection().ToString()};{hierarchicalIndex}";
      if (!(this.Owner is WordDocument))
        return this.Owner.GetHierarchicalIndex(hierarchicalIndex);
    }
    return hierarchicalIndex;
  }

  internal int GetZOrder()
  {
    switch (this.EntityType)
    {
      case EntityType.Picture:
        return (this as WPicture).OrderIndex;
      case EntityType.Shape:
      case EntityType.AutoShape:
        return (this as Shape).ZOrderPosition;
      case EntityType.TextBox:
        return (this as WTextBox).TextBoxFormat.OrderIndex;
      case EntityType.XmlParaItem:
        return (this as XmlParagraphItem).ZOrderIndex;
      case EntityType.Chart:
        return (this as WChart).ZOrderPosition;
      case EntityType.OleObject:
        if ((this as WOleObject).OlePicture != null)
          return (this as WOleObject).OlePicture.OrderIndex;
        break;
      case EntityType.GroupShape:
        return (this as GroupShape).ZOrderPosition;
    }
    return -1;
  }

  internal bool IsNeedToSortByItsPosition(Entity secondfloatingItem)
  {
    if (this.Owner == null || secondfloatingItem.Owner == null)
      return false;
    if (this.Owner == secondfloatingItem.Owner)
      return this.GetIndexInOwnerCollection() < secondfloatingItem.GetIndexInOwnerCollection();
    if (this.Owner.Owner == null || secondfloatingItem.Owner.Owner == null)
      return false;
    if (this.Owner.Owner == secondfloatingItem.Owner.Owner)
      return this.Owner.GetIndexInOwnerCollection() < secondfloatingItem.Owner.GetIndexInOwnerCollection();
    WParagraph owner1 = this.Owner as WParagraph;
    WParagraph owner2 = secondfloatingItem.Owner as WParagraph;
    WSection ownerSection1 = this.GetOwnerSection((Entity) owner1) as WSection;
    WSection ownerSection2 = this.GetOwnerSection((Entity) owner2) as WSection;
    if (ownerSection1 == null || ownerSection2 == null)
      return false;
    if (ownerSection1 != ownerSection2)
      return ownerSection1.GetIndexInOwnerCollection() < ownerSection2.GetIndexInOwnerCollection();
    if (!owner1.IsInCell && owner2.IsInCell || owner1.IsInCell && !owner2.IsInCell)
      return this.IsNeedToSortByItsPosition(owner1, owner2);
    return owner1.IsInCell && owner2.IsInCell && this.IsNeedToSortByItsPosition(owner1.GetOwnerEntity() as WTableCell, owner2.GetOwnerEntity() as WTableCell);
  }

  private bool IsNeedToSortByItsPosition(
    WParagraph firstFloatingItem,
    WParagraph secondfloatingItem)
  {
    WTable wtable = firstFloatingItem.IsInCell ? this.GetOwnerTable((Entity) firstFloatingItem) as WTable : this.GetOwnerTable((Entity) secondfloatingItem) as WTable;
    return wtable != null && (firstFloatingItem.IsInCell ? wtable.GetIndexInOwnerCollection() : firstFloatingItem.GetIndexInOwnerCollection()) < (secondfloatingItem.IsInCell ? wtable.GetIndexInOwnerCollection() : secondfloatingItem.GetIndexInOwnerCollection());
  }

  private bool IsNeedToSortByItsPosition(
    WTableCell firstItemOwnerCell,
    WTableCell secondItemOwnerCell)
  {
    if (firstItemOwnerCell.OwnerRow == secondItemOwnerCell.OwnerRow)
      return firstItemOwnerCell.GetIndexInOwnerCollection() < secondItemOwnerCell.GetIndexInOwnerCollection();
    WTable ownerTable1 = this.GetOwnerTable((Entity) firstItemOwnerCell.OwnerRow) as WTable;
    WTable ownerTable2 = this.GetOwnerTable((Entity) secondItemOwnerCell.OwnerRow) as WTable;
    if (ownerTable1 == null || ownerTable2 == null)
      return false;
    if (ownerTable1 != ownerTable2)
      return ownerTable1.GetIndexInOwnerCollection() < ownerTable2.GetIndexInOwnerCollection();
    if (firstItemOwnerCell.OwnerRow.OwnerTable == secondItemOwnerCell.OwnerRow.OwnerTable)
      return firstItemOwnerCell.OwnerRow.GetIndexInOwnerCollection() < secondItemOwnerCell.OwnerRow.GetIndexInOwnerCollection();
    if (firstItemOwnerCell.OwnerRow.OwnerTable.IsInCell && !secondItemOwnerCell.OwnerRow.OwnerTable.IsInCell)
      return this.IsNeedToSortByItsPosition(firstItemOwnerCell.OwnerRow.OwnerTable.GetOwnerTableCell(), secondItemOwnerCell);
    if (!firstItemOwnerCell.OwnerRow.OwnerTable.IsInCell && secondItemOwnerCell.OwnerRow.OwnerTable.IsInCell)
      return this.IsNeedToSortByItsPosition(firstItemOwnerCell, secondItemOwnerCell.OwnerRow.OwnerTable.GetOwnerTableCell());
    return firstItemOwnerCell.OwnerRow.OwnerTable.IsInCell && secondItemOwnerCell.OwnerRow.OwnerTable.IsInCell && this.IsNeedToSortByItsPosition(firstItemOwnerCell.OwnerRow.OwnerTable.GetOwnerTableCell(), secondItemOwnerCell.OwnerRow.OwnerTable.GetOwnerTableCell());
  }

  internal Entity GetOwnerSection(Entity entity)
  {
    while (true)
    {
      switch (entity)
      {
        case null:
        case WSection _:
          goto label_3;
        default:
          entity = entity.Owner;
          continue;
      }
    }
label_3:
    return !(entity is WSection) ? (Entity) null : entity;
  }

  internal Entity GetOwnerTable(Entity entity)
  {
    while (entity != null && !(entity is WTable))
    {
      entity = entity.Owner;
      if (entity is WTable && (entity as WTable).IsInCell)
        entity = entity.Owner;
    }
    return !(entity is WTable) ? (Entity) null : entity;
  }

  internal override void Close()
  {
    base.Close();
    if (this.m_revisions == null)
      return;
    this.m_revisions.Clear();
    this.m_revisions = (List<Revision>) null;
  }
}
