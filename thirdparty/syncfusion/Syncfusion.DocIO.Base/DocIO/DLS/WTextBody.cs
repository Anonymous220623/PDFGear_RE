// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WTextBody
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Layouting;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WTextBody : 
  WidgetContainer,
  ITextBody,
  ICompositeEntity,
  IEntity,
  IWidgetContainer,
  IWidget
{
  protected BodyItemCollection m_bodyItems;
  private WParagraphCollection m_paragraphs;
  private WTableCollection m_tables;
  private FormFieldCollection m_formFields;
  private List<AlternateChunk> m_alternateChunkCollection;

  public override EntityType EntityType => EntityType.TextBody;

  public IWParagraphCollection Paragraphs => (IWParagraphCollection) this.m_paragraphs;

  public IWTableCollection Tables => (IWTableCollection) this.m_tables;

  public FormFieldCollection FormFields
  {
    get
    {
      if (this.m_formFields == null)
        this.m_formFields = new FormFieldCollection(this);
      return this.m_formFields;
    }
  }

  internal List<AlternateChunk> AlternateChunkCollection
  {
    get
    {
      if (this.m_alternateChunkCollection == null)
        this.m_alternateChunkCollection = new List<AlternateChunk>();
      return this.m_alternateChunkCollection;
    }
  }

  public IWParagraph LastParagraph
  {
    get
    {
      return this.Paragraphs.Count <= 0 ? (IWParagraph) null : (IWParagraph) this.Paragraphs[this.Paragraphs.Count - 1];
    }
  }

  internal bool IsFormFieldsCreated => this.m_formFields != null;

  internal BodyItemCollection Items => this.m_bodyItems;

  public EntityCollection ChildEntities => (EntityCollection) this.m_bodyItems;

  internal WTextBody(WordDocument doc, Entity owner)
    : base(doc, owner)
  {
    this.m_bodyItems = new BodyItemCollection(this);
    this.m_paragraphs = new WParagraphCollection(this.m_bodyItems);
    this.m_tables = new WTableCollection(this.m_bodyItems);
  }

  internal WTextBody(WSection sec)
    : this(sec.Document, (Entity) sec)
  {
  }

  public IWParagraph AddParagraph()
  {
    return this.m_bodyItems[this.m_bodyItems.Add((IEntity) new WParagraph((IWordDocument) this.Document))] as IWParagraph;
  }

  public IWTable AddTable()
  {
    IWTable wtable = (IWTable) new WTable((IWordDocument) this.Document);
    this.m_bodyItems.Add((IEntity) wtable);
    return wtable;
  }

  public IBlockContentControl AddBlockContentControl(ContentControlType controlType)
  {
    switch (controlType)
    {
      case ContentControlType.BuildingBlockGallery:
      case ContentControlType.Group:
      case ContentControlType.RepeatingSection:
        throw new NotImplementedException($"Creating a content control for the {(object) controlType}type is not implemented");
      default:
        return this.m_bodyItems[this.m_bodyItems.Add((IEntity) new BlockContentControl(this.Document)
        {
          ContentControlProperties = {
            Type = controlType
          }
        })] as IBlockContentControl;
    }
  }

  internal IBlockContentControl AddStructureDocumentTag()
  {
    IBlockContentControl blockContentControl = (IBlockContentControl) new BlockContentControl(this.m_doc);
    this.m_bodyItems.Add((IEntity) blockContentControl);
    return blockContentControl;
  }

  internal AlternateChunk AddAlternateChunk()
  {
    AlternateChunk alternateChunk = new AlternateChunk(this.m_doc);
    this.m_bodyItems.Add((IEntity) alternateChunk);
    return alternateChunk;
  }

  internal AlternateChunk AddAltChunk(AlternateChunk altChunk)
  {
    this.m_bodyItems.Add((IEntity) altChunk);
    return altChunk;
  }

  public void InsertXHTML(string html) => this.InsertXHTML(html, this.Paragraphs.Count);

  public void InsertXHTML(string html, int paragraphIndex)
  {
    this.Paragraphs.Insert(paragraphIndex, (IWParagraph) new WParagraph((IWordDocument) this.Document));
    this.InsertXHTML(html, paragraphIndex, 0);
  }

  public void InsertXHTML(string html, int paragraphIndex, int paragraphItemIndex)
  {
    IHtmlConverter instance = HtmlConverterFactory.GetInstance();
    (instance as HTMLConverterImpl).HtmlImportSettings = this.Document.HTMLImportSettings;
    instance.AppendToTextBody((ITextBody) this, html, paragraphIndex, paragraphItemIndex);
  }

  public bool IsValidXHTML(string html, XHTMLValidationType type)
  {
    IHtmlConverter instance = HtmlConverterFactory.GetInstance();
    (instance as HTMLConverterImpl).HtmlImportSettings = this.Document.HTMLImportSettings;
    return instance.IsValid(html, type);
  }

  public bool IsValidXHTML(string html, XHTMLValidationType type, out string exceptionMessage)
  {
    IHtmlConverter instance = HtmlConverterFactory.GetInstance();
    (instance as HTMLConverterImpl).HtmlImportSettings = this.Document.HTMLImportSettings;
    return instance.IsValid(html, type, out exceptionMessage);
  }

  public void EnsureMinimum()
  {
    if (this.Paragraphs.Count != 0)
      return;
    this.AddParagraph();
  }

  internal TextSelection Find(Regex pattern)
  {
    foreach (TextBodyItem textBodyItem in (CollectionImpl) this.Items)
    {
      TextSelection textSelection = textBodyItem.Find(pattern);
      if (textSelection != null && textSelection.Count > 0)
        return textSelection;
    }
    return (TextSelection) null;
  }

  internal TextSelectionList FindAll(Regex pattern)
  {
    TextSelectionList all1 = (TextSelectionList) null;
    foreach (TextBodyItem textBodyItem in (CollectionImpl) this.Items)
    {
      TextSelectionList all2 = textBodyItem.FindAll(pattern);
      if (all2 != null && all2.Count > 0)
      {
        if (all1 == null)
          all1 = new TextSelectionList();
        all1.AddRange((IEnumerable<TextSelection>) all2);
      }
    }
    return all1;
  }

  internal int Replace(Regex pattern, string replace)
  {
    int num = 0;
    foreach (TextBodyItem textBodyItem in (CollectionImpl) this.Items)
    {
      num += textBodyItem.Replace(pattern, replace);
      if (this.Document.ReplaceFirst && num > 0)
        return num;
    }
    return num;
  }

  internal int Replace(Regex pattern, TextSelection textSelection, bool saveFormatting)
  {
    int num = 0;
    foreach (TextBodyItem textBodyItem in (CollectionImpl) this.Items)
    {
      num += textBodyItem.Replace(pattern, textSelection, saveFormatting);
      if (this.Document.ReplaceFirst && num > 0)
        return num;
    }
    return num;
  }

  internal int Replace(Regex pattern, TextBodyPart textPart, bool saveFormatting)
  {
    TextSelectionList textSelectionList = !FindUtils.IsPatternEmpty(pattern) ? this.FindAll(pattern) : throw new ArgumentException("Search string cannot be empty");
    int num1 = 0;
    if (textSelectionList == null)
      return 0;
    foreach (TextSelection textSelection in (List<TextSelection>) textSelectionList)
    {
      WCharacterFormat wcharacterFormat = (WCharacterFormat) null;
      if (saveFormatting)
        wcharacterFormat = textSelection.StartTextRange.CharacterFormat;
      InlineContentControl owner = textSelection.StartTextRange.Owner as InlineContentControl;
      int num2 = textSelection.SplitAndErase();
      if (owner != null)
      {
        textPart.PasteAt(owner, num2, wcharacterFormat, saveFormatting);
      }
      else
      {
        WParagraph ownerParagraph = textSelection.OwnerParagraph;
        textPart.PasteAt((ITextBody) ownerParagraph.OwnerTextBody, ownerParagraph.GetIndexInOwnerCollection(), num2, wcharacterFormat, saveFormatting);
      }
      ++num1;
      if (this.Document.ReplaceFirst)
        break;
    }
    return num1;
  }

  internal int Replace(Regex pattern, IWordDocument replaceDoc, bool saveFormatting)
  {
    if (FindUtils.IsPatternEmpty(pattern))
      throw new ArgumentException("Search string cannot be empty");
    WCharacterFormat wcharacterFormat = (WCharacterFormat) null;
    TextSelectionList all = this.FindAll(pattern);
    int num1 = 0;
    if (all == null)
      return 0;
    foreach (TextSelection sel in (List<TextSelection>) all)
    {
      if (saveFormatting)
        wcharacterFormat = this.GetSrcCharacterFormat(sel);
      InlineContentControl owner = sel.StartTextRange.Owner as InlineContentControl;
      int num2 = sel.SplitAndErase();
      WParagraph ownerParagraph = sel.OwnerParagraph;
      for (int index = replaceDoc.Sections.Count - 1; index >= 0; --index)
      {
        IWSection section = (IWSection) replaceDoc.Sections[index];
        TextBodyPart textBodyPart = new TextBodyPart(this.Document);
        textBodyPart.Copy(section.Body, false);
        if (owner != null)
          textBodyPart.PasteAt(owner, num2, wcharacterFormat, saveFormatting);
        else
          textBodyPart.PasteAt((ITextBody) ownerParagraph.OwnerTextBody, ownerParagraph.GetIndexInOwnerCollection(), num2, wcharacterFormat, saveFormatting);
      }
      ++num1;
      if (this.Document.ReplaceFirst)
        break;
    }
    return num1;
  }

  private WCharacterFormat GetSrcCharacterFormat(TextSelection sel)
  {
    WCharacterFormat format = new WCharacterFormat((IWordDocument) this.Document);
    sel.StartTextRange.CharacterFormat.UpdateSourceFormatting(format);
    return format;
  }

  internal override void AddSelf()
  {
    foreach (Entity childEntity in (CollectionImpl) this.ChildEntities)
      childEntity.AddSelf();
  }

  protected override object CloneImpl()
  {
    WTextBody body = (WTextBody) base.CloneImpl();
    body.m_bodyItems = new BodyItemCollection(body);
    this.ChildEntities.CloneTo((EntityCollection) body.m_bodyItems);
    body.m_paragraphs = new WParagraphCollection(body.m_bodyItems);
    body.m_tables = new WTableCollection(body.m_bodyItems);
    return (object) body;
  }

  private Entity GetTextBodyBaseEntity(Entity entity)
  {
    Entity textBodyBaseEntity = entity;
    while (textBodyBaseEntity.Owner != null)
    {
      textBodyBaseEntity = textBodyBaseEntity.Owner;
      if (textBodyBaseEntity is WSection)
        return textBodyBaseEntity;
    }
    return textBodyBaseEntity;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    int index = 0;
    for (int count = this.ChildEntities.Count; index < count; ++index)
    {
      Entity childEntity = this.ChildEntities[index];
      Entity textBodyBaseEntity = this.GetTextBodyBaseEntity(childEntity);
      if (childEntity is AlternateChunk && textBodyBaseEntity is WSection)
        (textBodyBaseEntity as WSection).Body.AlternateChunkCollection.Add(childEntity as AlternateChunk);
      childEntity.CloneRelationsTo(doc, nextOwner);
    }
  }

  internal override void Close()
  {
    if (this.m_bodyItems != null && this.m_bodyItems.InnerList != null && this.m_bodyItems.Count > 0)
    {
      TextBodyItem textBodyItem = (TextBodyItem) null;
      for (int index = 0; index < this.m_bodyItems.Count; ++index)
      {
        this.m_bodyItems[index].Close();
        textBodyItem = (TextBodyItem) null;
      }
      this.m_bodyItems.Close();
      this.m_bodyItems = (BodyItemCollection) null;
    }
    if (this.m_paragraphs != null)
    {
      this.m_paragraphs.Close();
      this.m_paragraphs = (WParagraphCollection) null;
    }
    if (this.m_tables != null)
    {
      this.m_tables.Close();
      this.m_tables = (WTableCollection) null;
    }
    if (this.m_formFields != null)
    {
      this.m_formFields.Close();
      this.m_formFields = (FormFieldCollection) null;
    }
    if (this.m_alternateChunkCollection != null)
    {
      this.m_alternateChunkCollection.Clear();
      this.m_alternateChunkCollection = (List<AlternateChunk>) null;
    }
    base.Close();
  }

  internal void MakeChanges(bool acceptChanges)
  {
    for (int itemIndex = 0; itemIndex < this.m_bodyItems.Count; ++itemIndex)
    {
      TextBodyItem bodyItem = this.m_bodyItems[itemIndex];
      if (!this.RemoveChangedItem(bodyItem, acceptChanges, ref itemIndex))
      {
        bool next = this.CheckMoveToNext(bodyItem, acceptChanges);
        if (bodyItem is WTable)
        {
          WTable ownerTable = bodyItem as WTable;
          if (acceptChanges)
          {
            ownerTable.m_trackTblFormat = (XmlTableFormat) null;
            ownerTable.m_trackTableGrid = (WTableColumnCollection) null;
            if (ownerTable.DocxTableFormat.Format.OldPropertiesHash.Count > 0)
              ownerTable.DocxTableFormat.Format.OldPropertiesHash.Clear();
          }
          else if (ownerTable.m_trackTblFormat != null)
          {
            ownerTable.DocxTableFormat.Format.ClearFormatting();
            ownerTable.DocxTableFormat = ownerTable.TrackTblFormat.Clone(ownerTable);
            ownerTable.FirstRow.RowFormat.ClearFormatting();
            ownerTable.FirstRow.RowFormat.ImportContainer((FormatBase) ownerTable.TrackTblFormat.Format);
            ownerTable.m_trackTblFormat = (XmlTableFormat) null;
          }
          if (!acceptChanges && ownerTable.m_trackTableGrid != null)
            ownerTable.ChangeTrackTableGrid();
        }
        if (!acceptChanges)
          this.RemoveChangedFormat(bodyItem);
        if (bodyItem.IsInsertRevision || bodyItem.IsDeleteRevision || bodyItem.IsChangedCFormat)
          bodyItem.AcceptCChanges();
        if (bodyItem.IsChangedPFormat)
          bodyItem.AcceptPChanges();
        bodyItem.MakeChanges(acceptChanges);
        if (next && this.MoveToNextPara(bodyItem))
        {
          this.ChildEntities.RemoveAt(itemIndex);
          --itemIndex;
        }
      }
    }
  }

  private bool RemoveChangedItem(TextBodyItem item, bool acceptChanges, ref int itemIndex)
  {
    if (item.IsInsertRevision && !acceptChanges || item.IsDeleteRevision && acceptChanges)
    {
      bool flag = true;
      if (item is WTable)
        flag = (item as WTable).RemoveChangedTable(acceptChanges);
      else if (item is WParagraph)
        flag = (item as WParagraph).CheckOnRemove();
      if (flag)
      {
        this.ChildEntities.RemoveAt(itemIndex);
        --itemIndex;
        return true;
      }
    }
    return false;
  }

  internal bool HasTrackedChanges()
  {
    foreach (TextBodyItem bodyItem in (CollectionImpl) this.m_bodyItems)
    {
      if (bodyItem.HasTrackedChanges())
        return true;
    }
    return false;
  }

  private void RemoveChangedFormat(TextBodyItem item)
  {
    if (item.IsChangedCFormat)
      item.RemoveCFormatChanges();
    if (!item.IsChangedPFormat)
      return;
    item.RemovePFormatChanges();
  }

  private bool CheckMoveToNext(TextBodyItem item, bool acceptChanges)
  {
    bool next = false;
    if (item is WParagraph && item.NextSibling is WParagraph && (item.IsInsertRevision && !acceptChanges || item.IsDeleteRevision && acceptChanges))
      next = true;
    return next;
  }

  private bool MoveToNextPara(TextBodyItem item)
  {
    if (item is WParagraph wparagraph)
    {
      int num = wparagraph.Items.Count - 1;
      if (!(item.NextSibling is WParagraph nextSibling))
        return false;
      for (int index = num; index >= 0; --index)
        nextSibling.Items.Insert(0, (IEntity) wparagraph.Items[index]);
    }
    return true;
  }

  protected override void InitXDLSHolder()
  {
    this.XDLSHolder.AddElement("paragraphs", (object) this.m_bodyItems);
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutInfo(ChildrenLayoutDirection.Vertical);
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this.m_bodyItems == null || this.m_bodyItems.InnerList == null || this.m_bodyItems.Count <= 0)
      return;
    for (int index = 0; index < this.m_bodyItems.Count; ++index)
    {
      this.m_bodyItems[index].InitLayoutInfo(entity, ref isLastTOCEntry);
      if (isLastTOCEntry)
        break;
    }
  }

  protected override IEntityCollectionBase WidgetCollection
  {
    get => (IEntityCollectionBase) this.m_bodyItems;
  }
}
