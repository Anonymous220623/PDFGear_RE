// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WSection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.Layouting;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WSection : 
  WidgetContainer,
  IWSection,
  ICompositeEntity,
  IEntity,
  IWidgetContainer,
  IWidget
{
  private const float DEF_DISTANCE_BETWEEN_COLUMNS = 36f;
  private WTextBody m_body;
  internal WSectionFormat m_sectionFormat;
  internal WHeadersFooters m_headersFooters;
  private EntityCollection m_childEntities;
  protected internal byte[] m_internalData;
  private short m_previousHeaderCount;
  private short m_previousFooterCount;
  private byte m_bFlags = 1;

  internal short PreviousHeaderCount
  {
    get => this.m_previousHeaderCount;
    set => this.m_previousHeaderCount = value;
  }

  internal short PreviousFooterCount
  {
    get => this.m_previousFooterCount;
    set => this.m_previousFooterCount = value;
  }

  public WTextBody Body => this.m_body;

  public WHeadersFooters HeadersFooters => this.m_headersFooters;

  public WPageSetup PageSetup
  {
    get => this.SectionFormat.PageSetup;
    internal set => this.SectionFormat.PageSetup = value;
  }

  internal WSectionFormat SectionFormat => this.m_sectionFormat;

  public ColumnCollection Columns => this.SectionFormat.Columns;

  public SectionBreakCode BreakCode
  {
    get => this.SectionFormat.BreakCode;
    set => this.SectionFormat.BreakCode = value;
  }

  internal byte[] DataArray
  {
    get => this.m_internalData;
    set => this.m_internalData = value;
  }

  public override EntityType EntityType => EntityType.Section;

  public EntityCollection ChildEntities
  {
    get
    {
      if (this.m_childEntities == null)
      {
        this.m_childEntities = (EntityCollection) new WSection.SectionChildEntities();
        this.m_childEntities.AddToInnerList((Entity) this.m_body);
        this.m_body.SetOwner((OwnerHolder) this);
        for (int index = 0; index < 6; ++index)
        {
          this.m_childEntities.AddToInnerList((Entity) this.m_headersFooters[index]);
          this.m_headersFooters[index].SetOwner((OwnerHolder) this);
        }
      }
      return this.m_childEntities;
    }
  }

  public IWParagraphCollection Paragraphs => this.Body.Paragraphs;

  public IWTableCollection Tables => this.Body.Tables;

  internal DocTextDirection TextDirection
  {
    get => this.SectionFormat.TextDirection;
    set => this.SectionFormat.TextDirection = value;
  }

  public bool ProtectForm
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsSectionFitInSamePage
  {
    get => ((int) this.m_bFlags & 4) >> 2 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 251 | (value ? 1 : 0) << 2);
  }

  public WSection(IWordDocument doc)
    : base((WordDocument) doc, (Entity) null)
  {
    this.m_sectionFormat = new WSectionFormat(this);
    this.m_body = new WTextBody(this);
    this.SectionFormat.m_columns = new ColumnCollection(this);
    this.m_headersFooters = new WHeadersFooters(this);
    this.m_headersFooters.SetOwner((OwnerHolder) this);
    this.PageSetup = new WPageSetup(this);
    this.PageSetup.SetOwner((OwnerHolder) this);
  }

  public Column AddColumn(float width, float spacing) => this.AddColumn(width, spacing, false);

  internal Column AddColumn(float width, float spacing, bool isOpening)
  {
    Column column = new Column((IWordDocument) this.Document);
    column.Width = width;
    column.Space = spacing;
    this.Columns.Add(column, isOpening);
    return column;
  }

  public void MakeColumnsEqual()
  {
    if (this.Columns.Count <= 0)
      return;
    float num = ((float) ((double) this.PageSetup.PageSize.Width - ((double) this.PageSetup.Margins.Left != -0.05000000074505806 ? (double) this.PageSetup.Margins.Left : 0.0) - ((double) this.PageSetup.Margins.Right != -0.05000000074505806 ? (double) this.PageSetup.Margins.Right : 0.0)) - (float) (this.Columns.Count - 1) * 36f) / (float) this.Columns.Count;
    foreach (Column column in (CollectionImpl) this.Columns)
    {
      column.Width = num;
      column.Space = 36f;
    }
  }

  public WSection Clone() => (WSection) base.Clone();

  public IWParagraph AddParagraph() => this.Body.AddParagraph();

  public IWTable AddTable() => this.Body.AddTable();

  internal IBlockContentControl AddStructureDocumentTag() => this.Body.AddStructureDocumentTag();

  internal AlternateChunk AddAlternateChunk() => this.Body.AddAlternateChunk();

  internal override void AddSelf()
  {
    this.Body.AddSelf();
    for (int index = 0; index < 6; ++index)
      this.HeadersFooters[index].AddSelf();
  }

  internal string GetText(WParagraph lastParagraph)
  {
    string empty = string.Empty;
    for (int index = 0; index < this.Body.ChildEntities.Count; ++index)
    {
      Entity childEntity = this.Body.ChildEntities[index];
      if (childEntity is WParagraph)
        empty += (childEntity as WParagraph).GetParagraphText(lastParagraph == childEntity);
      else if (childEntity is WTable)
        empty += (childEntity as WTable).GetTableText();
      if (this.Document.m_prevClonedEntity != null)
      {
        index = this.Document.m_prevClonedEntity.GetIndexInOwnerCollection();
        this.Document.m_prevClonedEntity = (TextBodyItem) null;
      }
    }
    return empty;
  }

  internal void AddEmptyParagraph()
  {
    bool flag = false;
    if (this.NextSibling == null)
      flag = true;
    if (this.Body.ChildEntities.Count == 0 && !flag)
      this.AddParagraph();
    if (!(this.Body.ChildEntities.LastItem is WTable))
      return;
    this.AddParagraph();
  }

  internal bool LineNumbersEnabled()
  {
    return this.PageSetup.LineNumberingMode != LineNumberingMode.None && (double) this.PageSetup.Margins.Left > 0.0 && this.PageSetup.LineNumberingStep > 0;
  }

  internal WSection CloneWithoutBodyItems()
  {
    bool isCloning = this.Document.IsCloning;
    this.Document.IsCloning = true;
    WSection wsection = new WSection((IWordDocument) this.Document);
    if (this.m_sectionFormat != null)
    {
      wsection.m_sectionFormat = this.m_sectionFormat.Clone();
      wsection.m_sectionFormat.SetOwner((OwnerHolder) wsection);
      wsection.PageSetup = this.PageSetup.Clone();
      wsection.PageSetup.SetOwner((OwnerHolder) wsection);
      wsection.SectionFormat.m_columns = new ColumnCollection(wsection);
      this.m_sectionFormat.m_columns.CloneTo(wsection.SectionFormat.m_columns);
    }
    wsection.m_headersFooters = this.m_headersFooters.Clone();
    wsection.m_headersFooters.SetOwner((OwnerHolder) wsection);
    for (int index = 0; index < 6; ++index)
      wsection.m_headersFooters[index].SetOwner((OwnerHolder) wsection);
    this.Document.IsCloning = isCloning;
    return wsection;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    this.Body.CloneRelationsTo(doc, nextOwner);
    ImportOptions importOptions = doc.ImportOptions;
    bool importStyles = doc.ImportStyles;
    if ((doc.ImportOptions & ImportOptions.UseDestinationStyles) == (ImportOptions) 0)
    {
      doc.ImportOptions = ImportOptions.UseDestinationStyles;
      doc.ImportStyles = false;
    }
    for (int index = 0; index <= 5; ++index)
      this.m_headersFooters[index].CloneRelationsTo(doc, nextOwner);
    if (doc.ImportOptions == importOptions)
      return;
    doc.ImportOptions = importOptions;
    doc.ImportStyles = importStyles;
  }

  protected override object CloneImpl()
  {
    bool isCloning = this.Document.IsCloning;
    this.Document.IsCloning = true;
    WSection wsection = (WSection) base.CloneImpl();
    wsection.m_childEntities = (EntityCollection) null;
    if (this.m_sectionFormat != null)
    {
      wsection.m_sectionFormat = this.m_sectionFormat.Clone();
      wsection.m_sectionFormat.SetOwner((OwnerHolder) wsection);
      wsection.PageSetup = this.PageSetup.Clone();
      wsection.PageSetup.SetOwner((OwnerHolder) wsection);
      wsection.SectionFormat.m_columns = new ColumnCollection(wsection);
      this.m_sectionFormat.m_columns.CloneTo(wsection.SectionFormat.m_columns);
    }
    wsection.m_body = (WTextBody) this.m_body.Clone();
    wsection.m_body.SetOwner((OwnerHolder) wsection);
    wsection.m_headersFooters = this.m_headersFooters.Clone();
    wsection.m_headersFooters.SetOwner((OwnerHolder) wsection);
    for (int index = 0; index < 6; ++index)
    {
      wsection.m_headersFooters[index].SetOwner((OwnerHolder) wsection);
      if (this.m_headersFooters[index].Watermark != null && this.m_headersFooters[index].Watermark.Type != WatermarkType.NoWatermark)
      {
        wsection.m_headersFooters[index].Watermark = (Watermark) this.m_headersFooters[index].Watermark.Clone();
        wsection.m_headersFooters[index].Watermark.SetOwner((OwnerHolder) wsection.m_headersFooters[index]);
      }
    }
    this.Document.IsCloning = isCloning;
    return (object) wsection;
  }

  internal void MakeChanges(bool acceptChanges)
  {
    this.m_body.MakeChanges(acceptChanges);
    if (this.m_internalData != null && this.m_internalData.Length < 300 && this.m_internalData.Length > 0)
    {
      SinglePropertyModifierArray propertyModifierArray = new SinglePropertyModifierArray(this.m_internalData, 0);
      SinglePropertyModifierRecord sprm = propertyModifierArray.TryGetSprm(12857);
      if (sprm != null)
      {
        int num = propertyModifierArray.Modifiers.IndexOf(sprm) + 1;
        if (num < propertyModifierArray.Count)
        {
          List<SinglePropertyModifierRecord> propertyModifierRecordList = new List<SinglePropertyModifierRecord>();
          int sprmIndex = num;
          for (int count = propertyModifierArray.Count; sprmIndex < count; ++sprmIndex)
            propertyModifierRecordList.Add(propertyModifierArray.GetSprmByIndex(sprmIndex));
          foreach (SinglePropertyModifierRecord modifier in propertyModifierRecordList)
          {
            propertyModifierArray.RemoveValue((int) modifier.Options);
            propertyModifierArray.Add(modifier);
          }
        }
        propertyModifierArray.RemoveValue(12857);
      }
      this.m_internalData = new byte[this.m_internalData.Length];
      propertyModifierArray.Save(this.m_internalData, 0);
    }
    for (int index = 0; index < 6; ++index)
      this.HeadersFooters[index].MakeChanges(acceptChanges);
  }

  internal bool HasTrackedChanges()
  {
    if (this.m_body.HasTrackedChanges())
      return true;
    for (int index = 0; index < 6; ++index)
    {
      if (this.HeadersFooters[index].HasTrackedChanges())
        return true;
    }
    return false;
  }

  internal override void Close()
  {
    if (this.m_body != null)
    {
      this.m_body.Close();
      this.m_body = (WTextBody) null;
    }
    if (this.m_headersFooters != null)
    {
      this.m_headersFooters.Close();
      this.m_headersFooters = (WHeadersFooters) null;
    }
    if (this.PageSetup != null)
    {
      this.PageSetup.Close();
      this.PageSetup = (WPageSetup) null;
    }
    if (this.Columns != null)
    {
      this.Columns.Close();
      this.SectionFormat.m_columns = (ColumnCollection) null;
    }
    if (this.m_sectionFormat != null)
    {
      this.m_sectionFormat.Close();
      this.m_sectionFormat = (WSectionFormat) null;
    }
    if (this.m_internalData != null)
      this.m_internalData = (byte[]) null;
    base.Close();
  }

  internal WParagraph GetFirstParagraph()
  {
    IEntity childEntity = this.Body.ChildEntities.Count > 0 ? (IEntity) this.Body.ChildEntities[0] : (IEntity) null;
    switch (childEntity)
    {
      case null:
        return (WParagraph) null;
      case WParagraph _:
        return this.Body.ChildEntities[0] as WParagraph;
      case BlockContentControl _:
        return (childEntity as BlockContentControl).ChildEntities.Count <= 0 ? (WParagraph) null : (childEntity as BlockContentControl).ChildEntities[0] as WParagraph;
      default:
        return childEntity as WParagraph;
    }
  }

  protected override void InitXDLSHolder()
  {
    base.InitXDLSHolder();
    this.XDLSHolder.AddElement("body", (object) this.m_body);
    this.XDLSHolder.AddElement("page-setup", (object) this.PageSetup);
    this.XDLSHolder.AddElement("columns", (object) this.Columns);
    this.XDLSHolder.AddElement("headers-footers", (object) this.m_headersFooters);
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("BreakCode", (Enum) this.BreakCode);
  }

  protected override void ReadXmlAttributes(IXDLSAttributeReader reader)
  {
    base.ReadXmlAttributes(reader);
    if (!reader.HasAttribute("BreakCode"))
      return;
    this.BreakCode = (SectionBreakCode) reader.ReadEnum("BreakCode", typeof (SectionBreakCode));
  }

  protected override void WriteXmlContent(IXDLSContentWriter writer)
  {
    base.WriteXmlContent(writer);
    if (this.DataArray == null)
      return;
    writer.WriteChildBinaryElement("internal-data", this.DataArray);
  }

  protected override bool ReadXmlContent(IXDLSContentReader reader)
  {
    bool flag = base.ReadXmlContent(reader);
    if (reader.TagName == "internal-data")
    {
      this.DataArray = reader.ReadChildBinaryElement();
      flag = true;
    }
    return flag;
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutInfo(ChildrenLayoutDirection.Vertical);
    if (this.Body.Items.Count != 0)
      return;
    this.m_layoutInfo.IsSkip = true;
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    if (this.m_headersFooters != null)
    {
      this.m_headersFooters.InitLayoutInfo(entity, ref isLastTOCEntry);
      if (isLastTOCEntry)
        return;
    }
    if (this.m_body == null)
      return;
    this.m_layoutInfo = (ILayoutInfo) null;
    this.m_body.InitLayoutInfo(entity, ref isLastTOCEntry);
    int num = isLastTOCEntry ? 1 : 0;
  }

  protected override IEntityCollectionBase WidgetCollection
  {
    get
    {
      this.AddEmptyParagraph();
      IEntity previousSibling;
      if (this.m_body.Items.Count <= 0 || !(this.m_body.Items.LastItem is WParagraph) || !(this.m_body.Items.LastItem as WParagraph).SectionEndMark || !((previousSibling = (this.m_body.Items.LastItem as WParagraph).PreviousSibling) is WParagraph) || (previousSibling as WParagraph).ChildEntities.Count <= 0 || !((previousSibling as WParagraph).LastItem is Break) || ((previousSibling as WParagraph).LastItem as Break).BreakType != BreakType.PageBreak || this.Document.Settings.CompatibilityOptions[CompatibilityOption.SplitPgBreakAndParaMark])
        return (IEntityCollectionBase) this.m_body.Items;
      BodyItemCollection widgetCollection = new BodyItemCollection(this.m_body);
      for (int index = 0; index < this.m_body.Items.Count; ++index)
        widgetCollection.AddToInnerList((Entity) this.m_body.Items[index]);
      widgetCollection.RemoveFromInnerList(widgetCollection.IndexOf((IEntity) widgetCollection.LastItem));
      return (IEntityCollectionBase) widgetCollection;
    }
  }

  internal class SectionChildEntities : EntityCollection
  {
    internal SectionChildEntities()
      : base((WordDocument) null)
    {
    }

    protected override string GetTagItemName() => throw new Exception();

    protected override OwnerHolder CreateItem(IXDLSContentReader reader) => throw new Exception();

    protected override Type[] TypesOfElement
    {
      get => throw new Exception("Cannot insert an object to SectionChildEntities collection.");
    }
  }
}
