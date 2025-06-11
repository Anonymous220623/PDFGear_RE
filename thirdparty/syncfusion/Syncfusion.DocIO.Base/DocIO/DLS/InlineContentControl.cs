// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.InlineContentControl
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Layouting;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class InlineContentControl : ParagraphItem, IInlineContentControl, IParagraphItem, IEntity
{
  private ContentControlProperties m_controlProperties;
  private ParagraphItemCollection m_paragraphItemCollection;
  private ParagraphItem m_mappedItem;
  internal bool IsMappedItem;

  internal ParagraphItem MappedItem
  {
    get => this.m_mappedItem;
    set => this.m_mappedItem = value;
  }

  public ParagraphItemCollection ParagraphItems
  {
    get
    {
      if (this.m_paragraphItemCollection == null)
        this.m_paragraphItemCollection = new ParagraphItemCollection(this.m_doc);
      return this.m_paragraphItemCollection;
    }
  }

  public ContentControlProperties ContentControlProperties => this.m_controlProperties;

  public WCharacterFormat BreakCharacterFormat => this.ParaItemCharFormat;

  public override EntityType EntityType => EntityType.InlineContentControl;

  internal override int EndPos
  {
    get
    {
      return this.ParagraphItems.Count > 0 ? this.ParagraphItems[this.ParagraphItems.Count - 1].EndPos : this.StartPos;
    }
  }

  internal InlineContentControl(WordDocument doc)
    : base(doc)
  {
    this.m_controlProperties = new ContentControlProperties(doc, (Entity) this);
    this.m_paragraphItemCollection = new ParagraphItemCollection(doc);
    this.m_paragraphItemCollection.SetOwner((OwnerHolder) this);
    this.m_charFormat = new WCharacterFormat((IWordDocument) doc, (Entity) this);
  }

  public InlineContentControl(WordDocument doc, ContentControlType controlType)
    : base(doc)
  {
    this.m_controlProperties = new ContentControlProperties(doc, (Entity) this);
    this.m_paragraphItemCollection = new ParagraphItemCollection(doc);
    this.m_paragraphItemCollection.SetOwner((OwnerHolder) this);
    this.m_charFormat = new WCharacterFormat((IWordDocument) doc, (Entity) this);
    this.m_controlProperties.Type = controlType;
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutInfo(ChildrenLayoutDirection.Vertical);
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this.m_paragraphItemCollection != null)
    {
      foreach (Entity paragraphItem in (CollectionImpl) this.m_paragraphItemCollection)
      {
        paragraphItem.InitLayoutInfo(entity, ref isLastTOCEntry);
        if (isLastTOCEntry)
          return;
      }
    }
    int num = isLastTOCEntry ? 1 : 0;
  }

  internal bool IsHidden()
  {
    for (int index = 0; index < this.ParagraphItems.Count; ++index)
    {
      if (!this.ParagraphItems[index].ParaItemCharFormat.Hidden)
        return false;
    }
    return this.ParagraphItems.Count > 0;
  }

  internal bool IsDeletion()
  {
    for (int index = 0; index < this.ParagraphItems.Count; ++index)
    {
      if (!this.ParagraphItems[index].ParaItemCharFormat.IsDeleteRevision)
        return false;
    }
    return this.ParagraphItems.Count > 0;
  }

  internal InlineContentControl Clone() => (InlineContentControl) this.CloneImpl();

  internal override void AddSelf()
  {
    foreach (Entity paragraphItem in (CollectionImpl) this.m_paragraphItemCollection)
      paragraphItem.AddSelf();
  }

  protected override object CloneImpl()
  {
    InlineContentControl owner = (InlineContentControl) base.CloneImpl();
    owner.m_paragraphItemCollection = new ParagraphItemCollection(owner);
    this.m_paragraphItemCollection.CloneItemsTo(owner.m_paragraphItemCollection);
    if (this.m_mappedItem != null)
      owner.m_mappedItem = this.m_mappedItem.Clone() as ParagraphItem;
    owner.m_controlProperties = this.m_controlProperties.Clone();
    owner.m_controlProperties.SetOwnerContentControl((Entity) owner);
    owner.m_charFormat = new WCharacterFormat((IWordDocument) this.Document, (Entity) owner);
    owner.m_charFormat.ImportContainer((FormatBase) this.BreakCharacterFormat);
    owner.m_charFormat.CopyProperties((FormatBase) this.BreakCharacterFormat);
    return (object) owner;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    int index = 0;
    for (int count = this.ParagraphItems.Count; index < count; ++index)
      this.ParagraphItems[index].CloneRelationsTo(doc, nextOwner);
    this.ContentControlProperties.CloneRelationsTo(doc, nextOwner);
    this.ContentControlProperties.SetOwner((OwnerHolder) this);
  }

  internal void ApplyBaseFormat()
  {
    IWParagraphStyle style = this.OwnerParagraph.GetStyle();
    if (style == null)
      return;
    this.ParaItemCharFormat.ApplyBase((FormatBase) style.CharacterFormat);
    for (int index = 0; index < this.ParagraphItems.Count; ++index)
      this.ParagraphItems[index].ParaItemCharFormat.ApplyBase((FormatBase) style.CharacterFormat);
    if (this.MappedItem == null)
      return;
    this.MappedItem.ParaItemCharFormat.ApplyBase((FormatBase) style.CharacterFormat);
  }

  internal void ApplyBaseFormatForCharacterStyle(IWCharacterStyle style)
  {
    for (int index = 0; index < this.ParagraphItems.Count; ++index)
      this.ParagraphItems[index].ParaItemCharFormat.CharStyleName = style.Name;
  }

  internal override void Close()
  {
    if (this.m_paragraphItemCollection != null)
    {
      this.m_paragraphItemCollection.Close();
      this.m_paragraphItemCollection = (ParagraphItemCollection) null;
    }
    if (this.m_controlProperties != null)
    {
      this.m_controlProperties.Close();
      this.m_controlProperties = (ContentControlProperties) null;
    }
    if (this.m_mappedItem != null)
    {
      this.m_mappedItem.Close();
      this.m_mappedItem = (ParagraphItem) null;
    }
    base.Close();
  }

  internal void CopyItemsTo(ParagraphItemCollection paraItems)
  {
    for (int index1 = 0; index1 < this.ParagraphItems.Count; ++index1)
    {
      XmlParagraphItem paragraphItem = this.ParagraphItems[index1] as XmlParagraphItem;
      if (this.ParagraphItems[index1] is InlineContentControl)
        ((InlineContentControl) this.ParagraphItems[index1]).CopyItemsTo(paraItems);
      else if (paragraphItem != null && paragraphItem.MathParaItemsCollection != null)
      {
        for (int index2 = 0; index2 < paragraphItem.MathParaItemsCollection.Count; ++index2)
        {
          if (paragraphItem.MathParaItemsCollection[index2] is InlineContentControl)
            ((InlineContentControl) paragraphItem.MathParaItemsCollection[index2]).CopyItemsTo(paraItems);
          else
            paraItems.InnerList.Add((object) paragraphItem.MathParaItemsCollection[index2]);
        }
      }
      else
        paraItems.InnerList.Add((object) this.ParagraphItems[index1]);
    }
  }
}
