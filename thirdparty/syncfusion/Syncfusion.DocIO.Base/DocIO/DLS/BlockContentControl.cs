// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.BlockContentControl
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Layouting;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class BlockContentControl : 
  TextBodyItem,
  IWidgetContainer,
  IWidget,
  IBlockContentControl,
  ICompositeEntity,
  IEntity
{
  private ContentControlProperties m_controlProperties;
  private WCharacterFormat m_breakCharacterFormat;
  private WTextBody m_textBody;
  private WParagraph m_mappedParagraph;

  internal WParagraph MappedParagraph
  {
    get => this.m_mappedParagraph;
    set => this.m_mappedParagraph = value;
  }

  public WTextBody TextBody => this.m_textBody;

  public ContentControlProperties ContentControlProperties => this.m_controlProperties;

  public EntityCollection ChildEntities => this.m_textBody.ChildEntities;

  internal Entity LastChildEntity
  {
    get
    {
      return this.m_textBody.ChildEntities.Count > 0 ? this.m_textBody.ChildEntities[this.m_textBody.ChildEntities.Count - 1] : (Entity) null;
    }
  }

  public WCharacterFormat BreakCharacterFormat => this.m_breakCharacterFormat;

  public BlockContentControl(WordDocument doc)
    : base(doc)
  {
    this.m_controlProperties = new ContentControlProperties(doc, (Entity) this);
    this.m_textBody = new WTextBody(doc, (Entity) this);
    this.m_breakCharacterFormat = new WCharacterFormat((IWordDocument) doc);
  }

  public BlockContentControl(WordDocument doc, ContentControlType controlType)
    : base(doc)
  {
    this.m_controlProperties = new ContentControlProperties(doc, (Entity) this);
    this.m_controlProperties.Type = controlType;
    this.m_textBody = new WTextBody(doc, (Entity) this);
    this.m_breakCharacterFormat = new WCharacterFormat((IWordDocument) doc);
  }

  internal override void AddSelf() => this.m_textBody.AddSelf();

  internal BlockContentControl Clone() => (BlockContentControl) this.CloneImpl();

  protected override object CloneImpl()
  {
    BlockContentControl owner = (BlockContentControl) base.CloneImpl();
    owner.m_controlProperties = this.m_controlProperties.Clone();
    owner.m_controlProperties.SetOwnerContentControl((Entity) owner);
    owner.m_textBody = (WTextBody) this.m_textBody.Clone();
    owner.m_textBody.SetOwner((OwnerHolder) owner);
    owner.m_breakCharacterFormat = new WCharacterFormat((IWordDocument) this.Document);
    owner.m_breakCharacterFormat.ImportContainer((FormatBase) this.BreakCharacterFormat);
    owner.m_breakCharacterFormat.CopyProperties((FormatBase) this.BreakCharacterFormat);
    return (object) owner;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    this.m_textBody.CloneRelationsTo(doc, nextOwner);
    this.ContentControlProperties.CloneRelationsTo(doc, nextOwner);
    this.ContentControlProperties.SetOwner((OwnerHolder) this);
    if (this.m_breakCharacterFormat == null)
      return;
    this.m_breakCharacterFormat.CloneRelationsTo(doc, nextOwner);
  }

  internal override TextBodyItem GetNextTextBodyItemValue()
  {
    if (this.NextSibling != null)
      return this.NextSibling as TextBodyItem;
    if (this.Owner is WTableCell)
      return (this.Owner as WTableCell).GetNextTextBodyItem();
    if (this.Owner is WTextBody)
    {
      if (this.OwnerTextBody.Owner is WTextBox)
        return (this.OwnerTextBody.Owner as WTextBox).GetNextTextBodyItem();
      if (this.OwnerTextBody.Owner is WSection)
        return this.GetNextInSection(this.OwnerTextBody.Owner as WSection);
    }
    return (TextBodyItem) null;
  }

  internal bool IsHiddenParagraphMarkIsInLastItemOfSDTContent()
  {
    BodyItemCollection items = this.TextBody.Items;
    return items != null && items.Count > 0 && items[items.Count - 1] is WParagraph && (items[items.Count - 1] as WParagraph).BreakCharacterFormat != null && (items[items.Count - 1] as WParagraph).BreakCharacterFormat.Hidden;
  }

  internal bool IsDeletionParagraphMarkIsInLastItemOfSDTContent()
  {
    BodyItemCollection items = this.TextBody.Items;
    return items != null && items.Count > 0 && items[items.Count - 1] is WParagraph && (items[items.Count - 1] as WParagraph).BreakCharacterFormat != null && (items[items.Count - 1] as WParagraph).BreakCharacterFormat.IsDeleteRevision;
  }

  internal bool ContainsParagraph()
  {
    BodyItemCollection items = this.TextBody.Items;
    for (int index = 0; index < items.Count; ++index)
    {
      if (items[index] is WParagraph)
        return true;
      if (items[index] is BlockContentControl)
        return (items[index] as BlockContentControl).ContainsParagraph();
    }
    return false;
  }

  internal WParagraph GetFirstParagraphOfSDTContent()
  {
    BodyItemCollection items = this.TextBody.Items;
    return items.Count > 0 && items[0] is WParagraph ? (WParagraph) items[0] : (WParagraph) null;
  }

  internal WParagraph GetLastParagraphOfSDTContent()
  {
    BodyItemCollection items = this.TextBody.Items;
    return items.Count > 0 && items[items.Count - 1] is WParagraph ? items[items.Count - 1] as WParagraph : (WParagraph) null;
  }

  internal override bool CheckDeleteRev()
  {
    return this.m_breakCharacterFormat != null && this.m_breakCharacterFormat.IsDeleteRevision;
  }

  internal override void SetChangedPFormat(bool check)
  {
  }

  internal override void SetChangedCFormat(bool check)
  {
  }

  internal override void SetDeleteRev(bool check)
  {
    if (this.m_breakCharacterFormat == null)
      return;
    this.m_breakCharacterFormat.IsDeleteRevision = check;
  }

  internal override void SetInsertRev(bool check)
  {
    if (this.m_breakCharacterFormat == null)
      return;
    this.m_breakCharacterFormat.IsInsertRevision = check;
  }

  internal override bool HasTrackedChanges() => false;

  public override int Replace(Regex pattern, string replace)
  {
    int num = 0;
    foreach (TextBodyItem textBodyItem in (CollectionImpl) this.TextBody.Items)
    {
      num += textBodyItem.Replace(pattern, replace);
      if (this.Document.ReplaceFirst && num > 0)
        return num;
    }
    return num;
  }

  internal int Replace(Regex pattern, TextBodyPart textPart, bool saveFormatting)
  {
    TextSelectionList textSelectionList = !FindUtils.IsPatternEmpty(pattern) ? this.FindAll(pattern) : throw new ArgumentException("Search string cannot be empty");
    if (textSelectionList == null)
      return 0;
    foreach (TextSelection textSelection in (List<TextSelection>) textSelectionList)
    {
      WCharacterFormat srcFormat = (WCharacterFormat) null;
      if (saveFormatting)
        srcFormat = textSelection.StartTextRange.CharacterFormat;
      int pItemIndex = textSelection.SplitAndErase();
      WParagraph ownerParagraph = textSelection.OwnerParagraph;
      textPart.PasteAt((ITextBody) ownerParagraph.OwnerTextBody, ownerParagraph.GetIndexInOwnerCollection(), pItemIndex, srcFormat, saveFormatting);
      if (this.Document.ReplaceFirst)
        break;
    }
    return textSelectionList.Count;
  }

  public override int Replace(string given, string replace, bool caseSensitive, bool wholeWord)
  {
    return this.Replace(FindUtils.StringToRegex(given, caseSensitive, wholeWord), replace);
  }

  public override int Replace(Regex pattern, TextSelection textSelection)
  {
    return this.Replace(pattern, textSelection, false);
  }

  public override int Replace(Regex pattern, TextSelection textSelection, bool saveFormatting)
  {
    int num = 0;
    foreach (TextBodyItem textBodyItem in (CollectionImpl) this.TextBody.Items)
    {
      num += textBodyItem.Replace(pattern, textSelection, saveFormatting);
      if (this.Document.ReplaceFirst && num > 0)
        return num;
    }
    return num;
  }

  public int Replace(
    string given,
    TextSelection textSelection,
    bool caseSensitive,
    bool wholeWord)
  {
    return this.Replace(FindUtils.StringToRegex(given, caseSensitive, wholeWord), textSelection, false);
  }

  public int Replace(
    string given,
    TextSelection textSelection,
    bool caseSensitive,
    bool wholeWord,
    bool saveFormatting)
  {
    return this.Replace(FindUtils.StringToRegex(given, caseSensitive, wholeWord), textSelection, saveFormatting);
  }

  internal int ReplaceFirst(string given, string replace, bool caseSensitive, bool wholeWord)
  {
    return this.ReplaceFirst(FindUtils.StringToRegex(given, caseSensitive, wholeWord), replace);
  }

  internal int ReplaceFirst(Regex pattern, string replace)
  {
    if (FindUtils.IsPatternEmpty(pattern))
      throw new ArgumentException("Search string cannot be empty");
    TextReplacer instance = TextReplacer.Instance;
    bool replaceFirst = this.Document.ReplaceFirst;
    this.Document.ReplaceFirst = true;
    int num = 0;
    for (int index = 0; index < this.TextBody.Items.Count; ++index)
    {
      if (this.TextBody.Items[index] is WParagraph)
        num += instance.Replace((WParagraph) this.TextBody.Items[index], pattern, replace);
      else if (this.TextBody.Items[index] is BlockContentControl)
        num += this.ReplaceFirst(pattern, replace);
    }
    this.Document.ReplaceFirst = replaceFirst;
    return num;
  }

  protected override void CreateLayoutInfo()
  {
    this.m_layoutInfo = (ILayoutInfo) new LayoutInfo(ChildrenLayoutDirection.Vertical);
  }

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this.m_textBody == null)
      return;
    this.m_textBody.InitLayoutInfo(entity, ref isLastTOCEntry);
    int num = isLastTOCEntry ? 1 : 0;
  }

  internal override void RemoveCFormatChanges()
  {
  }

  internal override void RemovePFormatChanges()
  {
  }

  internal override void AcceptCChanges()
  {
  }

  internal override void AcceptPChanges()
  {
  }

  internal override bool CheckChangedCFormat() => false;

  internal override bool CheckInsertRev() => false;

  public override TextSelection Find(Regex pattern)
  {
    foreach (TextBodyItem textBodyItem in (CollectionImpl) this.TextBody.Items)
    {
      TextSelection textSelection = textBodyItem.Find(pattern);
      if (textSelection != null && textSelection.Count > 0)
        return textSelection;
    }
    return (TextSelection) null;
  }

  public TextSelection Find(string given, bool caseSensitive, bool wholeWord)
  {
    return this.Find(FindUtils.StringToRegex(given, caseSensitive, wholeWord));
  }

  internal override void MakeChanges(bool acceptChanges)
  {
  }

  internal override TextSelectionList FindAll(Regex pattern)
  {
    TextSelectionList all1 = (TextSelectionList) null;
    foreach (TextBodyItem textBodyItem in (CollectionImpl) this.TextBody.Items)
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

  private WCharacterFormat GetSrcCharacterFormat(TextSelection sel)
  {
    WCharacterFormat format = new WCharacterFormat((IWordDocument) this.Document);
    sel.StartTextRange.CharacterFormat.UpdateSourceFormatting(format);
    return format;
  }

  internal int Replace(Regex pattern, IWordDocument replaceDoc, bool saveFormatting)
  {
    if (FindUtils.IsPatternEmpty(pattern))
      throw new ArgumentException("Search string cannot be empty");
    WCharacterFormat srcFormat = (WCharacterFormat) null;
    TextSelectionList all = this.FindAll(pattern);
    if (all == null)
      return 0;
    foreach (TextSelection sel in (List<TextSelection>) all)
    {
      if (saveFormatting)
        srcFormat = this.GetSrcCharacterFormat(sel);
      int pItemIndex = sel.SplitAndErase();
      WParagraph ownerParagraph = sel.OwnerParagraph;
      for (int index = replaceDoc.Sections.Count - 1; index >= 0; --index)
      {
        IWSection section = (IWSection) replaceDoc.Sections[index];
        TextBodyPart textBodyPart = new TextBodyPart(this.Document);
        textBodyPart.Copy(section.Body, false);
        textBodyPart.PasteAt((ITextBody) ownerParagraph.OwnerTextBody, ownerParagraph.GetIndexInOwnerCollection(), pItemIndex, srcFormat, saveFormatting);
      }
      if (this.Document.ReplaceFirst)
        break;
    }
    return all.Count;
  }

  internal override void Close()
  {
    if (this.m_textBody != null)
    {
      this.m_textBody.Close();
      this.m_textBody = (WTextBody) null;
    }
    if (this.m_controlProperties != null)
    {
      this.m_controlProperties.Close();
      this.m_controlProperties = (ContentControlProperties) null;
    }
    if (this.m_breakCharacterFormat != null)
    {
      this.m_breakCharacterFormat.Close();
      this.m_breakCharacterFormat = (WCharacterFormat) null;
    }
    if (this.m_mappedParagraph != null)
    {
      this.m_mappedParagraph.Close();
      this.m_mappedParagraph = (WParagraph) null;
    }
    base.Close();
  }

  internal override bool CheckChangedPFormat() => false;

  public override EntityType EntityType => EntityType.BlockContentControl;

  int IWidgetContainer.Count => this.WidgetCollection.Count;

  IWidget IWidgetContainer.this[int index] => this.WidgetCollection[index] as IWidget;

  protected IEntityCollectionBase WidgetCollection => (IEntityCollectionBase) this.TextBody.Items;

  public EntityCollection WidgetInnerCollection => this.WidgetCollection as EntityCollection;
}
