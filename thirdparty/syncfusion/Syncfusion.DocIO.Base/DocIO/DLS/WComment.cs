// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WComment
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using Syncfusion.Layouting;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WComment : ParagraphItem, ICompositeEntity, IEntity
{
  protected WTextBody m_textBody;
  protected WCommentFormat m_format;
  private ParagraphItemCollection m_commItems;
  private TextBodyPart m_bodyPart;
  private bool m_appendItems;
  private WCommentMark m_commentRangeStart;
  private WCommentMark m_commentRangeEnd;
  private string m_parentParaId = string.Empty;
  private byte m_bFlags;

  public WCommentMark CommentRangeStart
  {
    get => this.m_commentRangeStart;
    internal set => this.m_commentRangeStart = value;
  }

  public WCommentMark CommentRangeEnd
  {
    get => this.m_commentRangeEnd;
    internal set => this.m_commentRangeEnd = value;
  }

  public EntityCollection ChildEntities => this.m_textBody.ChildEntities;

  public override EntityType EntityType => EntityType.Comment;

  public WTextBody TextBody => this.m_textBody;

  public WCommentFormat Format => this.m_format;

  public ParagraphItemCollection CommentedItems
  {
    get
    {
      if (this.m_commItems == null)
        this.m_commItems = new ParagraphItemCollection(this.m_doc);
      return this.m_commItems;
    }
  }

  internal bool AppendItems => this.m_appendItems;

  internal TextBodyPart CommentedBodyPart => this.m_bodyPart;

  internal string ParentParaId
  {
    get => this.m_parentParaId;
    set => this.m_parentParaId = value;
  }

  public WComment Ancestor => this.GetAncestorComment();

  internal bool IsDetached
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal bool IsResolved
  {
    get => ((int) this.m_bFlags & 2) >> 1 != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 253 | (value ? 1 : 0) << 1);
  }

  public WComment(IWordDocument doc)
    : base((WordDocument) doc)
  {
    this.m_format = new WCommentFormat();
    this.m_format.SetOwner((OwnerHolder) this);
    this.m_textBody = new WTextBody(this.Document, (Entity) this);
  }

  protected override object CloneImpl()
  {
    WComment owner = (WComment) base.CloneImpl();
    owner.m_format = this.Format.Clone((IWordDocument) this.Document);
    owner.m_format.SetOwner((OwnerHolder) owner);
    owner.m_textBody = (WTextBody) this.TextBody.Clone();
    owner.m_textBody.SetOwner((OwnerHolder) owner);
    owner.m_commItems = (ParagraphItemCollection) null;
    this.m_bodyPart = (TextBodyPart) null;
    owner.IsDetached = true;
    return (object) owner;
  }

  public void RemoveCommentedItems()
  {
    if (this.m_commItems == null || this.m_commItems.Count == 0)
      return;
    if (this.m_appendItems)
    {
      this.m_commItems.Clear();
      this.m_bodyPart = (TextBodyPart) null;
    }
    else
    {
      this.RemoveItemsBetween(this.m_commItems.FirstItem as ParagraphItem, this.m_commItems.LastItem as ParagraphItem);
      this.Format.BookmarkStartOffset = 0;
      this.Format.BookmarkEndOffset = 1;
      this.m_commItems.Clear();
      this.m_appendItems = false;
    }
  }

  internal void RemoveItemsBetween(ParagraphItem firstItem, ParagraphItem lastItem)
  {
    if (firstItem.PreviousSibling != null && firstItem.PreviousSibling is WCommentMark)
      firstItem.OwnerParagraph.Items.Remove(firstItem.PreviousSibling);
    if (lastItem.NextSibling != null && lastItem.NextSibling is WCommentMark)
      lastItem.OwnerParagraph.Items.Remove(lastItem.NextSibling);
    if (firstItem != lastItem)
    {
      if (firstItem.OwnerParagraph != lastItem.OwnerParagraph)
      {
        while (firstItem.OwnerParagraph.NextTextBodyItem != lastItem.OwnerParagraph && firstItem.OwnerParagraph.NextTextBodyItem != null && this.CheckTextBody(firstItem.OwnerParagraph.NextTextBodyItem))
          firstItem.OwnerParagraph.NextTextBodyItem.RemoveSelf();
      }
      while (firstItem.NextSibling != null && firstItem.NextSibling != lastItem && !(firstItem.NextSibling is WComment))
        firstItem.OwnerParagraph.Items.Remove(firstItem.NextSibling);
      while (lastItem.PreviousSibling != null && lastItem.PreviousSibling != firstItem && !(firstItem.NextSibling is WComment))
        lastItem.OwnerParagraph.Items.Remove(lastItem.PreviousSibling);
      this.RemoveFirstItem(firstItem, lastItem);
    }
    lastItem.RemoveSelf();
  }

  public void ReplaceCommentedItems(string text)
  {
    string str = this.ModifyText(text);
    WTextRange wtextRange = new WTextRange((IWordDocument) this.m_doc);
    wtextRange.Text = text;
    if (this.Format.TagBkmk == "")
      this.Format.UpdateTagBkmk();
    if (str.IndexOf(ControlChar.CarriegeReturn) != -1)
    {
      this.RemoveCommentedItems();
      this.m_appendItems = false;
      string tagBkmk = this.Format.TagBkmk;
      int inOwnerCollection = this.GetIndexInOwnerCollection();
      WCommentMark wcommentMark1 = new WCommentMark(this.Document, tagBkmk, CommentMarkType.CommentStart);
      WCommentMark wcommentMark2 = new WCommentMark(this.Document, tagBkmk, CommentMarkType.CommentEnd);
      this.OwnerParagraph.Items.Insert(inOwnerCollection, (IEntity) wcommentMark2);
      this.OwnerParagraph.Items.Insert(inOwnerCollection, (IEntity) wtextRange);
      this.OwnerParagraph.Items.Insert(inOwnerCollection, (IEntity) wcommentMark1);
    }
    else
    {
      this.RemoveCommentedItems();
      this.m_appendItems = true;
      this.CommentedItems.InnerList.Add((object) wtextRange);
    }
  }

  public void ReplaceCommentedItems(TextBodyPart textBodyPart)
  {
    this.RemoveCommentedItems();
    this.m_appendItems = true;
    this.m_bodyPart = textBodyPart;
    this.FillCommItems();
  }

  internal override void AddSelf()
  {
  }

  internal override void AttachToParagraph(WParagraph owner, int itemPos)
  {
    base.AttachToParagraph(owner, itemPos);
    this.Document.Comments.Add(this);
    if (this.m_textBody == null)
      return;
    this.m_textBody.AttachToDocument();
  }

  internal override void AttachToDocument()
  {
    if (!this.IsDetached)
      return;
    this.Document.Comments.Add(this);
    if (this.m_textBody == null)
      return;
    this.m_textBody.AttachToDocument();
  }

  internal override void Close()
  {
    if (this.m_textBody != null)
    {
      this.m_textBody.Close();
      this.m_textBody = (WTextBody) null;
    }
    this.m_format = (WCommentFormat) null;
    this.m_bodyPart = (TextBodyPart) null;
    if (this.m_commItems != null)
    {
      this.m_commItems.Clear();
      this.m_commItems = (ParagraphItemCollection) null;
    }
    base.Close();
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    if (this.m_textBody == null)
      return;
    this.m_textBody.CloneRelationsTo(doc, nextOwner);
  }

  public void AddCommentedItem(IParagraphItem paraItem)
  {
    if (!(this.Owner is WParagraph) || this.m_commItems != null && this.m_commItems.Contains((IEntity) paraItem))
      return;
    WParagraph ownerParagraph = this.OwnerParagraph;
    int inOwnerCollection1 = this.GetIndexInOwnerCollection();
    if (this.m_format.TagBkmk == "")
    {
      string commentId = Convert.ToString(TagIdRandomizer.Instance.Next());
      this.m_format.TagBkmk = commentId;
      WCommentMark wcommentMark = new WCommentMark(this.m_doc, commentId);
      wcommentMark.Type = CommentMarkType.CommentStart;
      ownerParagraph.Items.Insert(inOwnerCollection1, (IEntity) new WCommentMark(this.m_doc, commentId)
      {
        Type = CommentMarkType.CommentEnd
      });
      ownerParagraph.Items.Insert(inOwnerCollection1, (IEntity) wcommentMark);
    }
    int inOwnerCollection2 = this.GetIndexInOwnerCollection();
    if (!(ownerParagraph.Items[inOwnerCollection2 - 1] is WCommentMark))
      return;
    string tagBkmk = this.m_format.TagBkmk;
    if (!(paraItem.Owner is WParagraph))
      this.InsertCommItem(ownerParagraph, inOwnerCollection2 - 1, paraItem);
    else if (ownerParagraph.Items.Count > inOwnerCollection2 + 1 && paraItem == ownerParagraph.Items[inOwnerCollection2 + 1])
    {
      ownerParagraph.Items.RemoveAt(inOwnerCollection2 + 1);
      this.InsertCommItem(ownerParagraph, inOwnerCollection2 - 1, paraItem);
    }
    else
    {
      WCommentMark commentStart = this.FindCommentStart(inOwnerCollection2, tagBkmk, ownerParagraph.Items);
      if (commentStart != null && paraItem == ownerParagraph.Items[commentStart.GetIndexInOwnerCollection() - 1])
      {
        int inOwnerCollection3 = commentStart.GetIndexInOwnerCollection();
        ownerParagraph.Items.RemoveAt(inOwnerCollection3 - 1);
        this.InsertCommItem(ownerParagraph, inOwnerCollection3, paraItem);
      }
      else
      {
        ParagraphItem paragraphItem = paraItem.Clone() as ParagraphItem;
        this.InsertCommItem(ownerParagraph, inOwnerCollection2 - 1, (IParagraphItem) paragraphItem);
      }
    }
  }

  protected override void InitXDLSHolder()
  {
    this.XDLSHolder.AddElement("body", (object) this.m_textBody);
    this.XDLSHolder.AddElement("comment-format", (object) this.m_format);
  }

  protected override void WriteXmlAttributes(IXDLSAttributeWriter writer)
  {
    base.WriteXmlAttributes(writer);
    writer.WriteValue("type", (Enum) ParagraphItemType.Comment);
  }

  protected override void CreateLayoutInfo() => this.m_layoutInfo = (ILayoutInfo) new LayoutInfo();

  internal override void InitLayoutInfo(Entity entity, ref bool isLastTOCEntry)
  {
    this.m_layoutInfo = (ILayoutInfo) null;
    if (this.m_textBody != null)
      this.m_textBody.InitLayoutInfo(entity, ref isLastTOCEntry);
    if (this.m_commItems != null && this.m_commItems.Count > 0)
    {
      foreach (Entity commItem in (CollectionImpl) this.m_commItems)
      {
        commItem.InitLayoutInfo(entity, ref isLastTOCEntry);
        if (isLastTOCEntry)
          return;
      }
    }
    if (this != entity)
      return;
    isLastTOCEntry = true;
  }

  private void InsertCommItem(WParagraph para, int index, IParagraphItem item)
  {
    para.Items.Insert(index, (IEntity) item);
    (item as ParagraphItem).SetOwner((OwnerHolder) para);
    this.CommentedItems.InnerList.Add((object) item);
  }

  private WCommentMark FindCommentStart(
    int index,
    string startId,
    ParagraphItemCollection itemCollection)
  {
    WCommentMark commentStart = (WCommentMark) null;
    for (int index1 = index; index1 > 0; --index1)
    {
      ParagraphItem paragraphItem = itemCollection[index1];
      if (paragraphItem is WCommentMark)
      {
        WCommentMark wcommentMark = paragraphItem as WCommentMark;
        if (wcommentMark.Type == CommentMarkType.CommentStart && wcommentMark.CommentId == startId)
        {
          commentStart = wcommentMark;
          break;
        }
      }
    }
    return commentStart;
  }

  private bool CheckTextBody(TextBodyItem item)
  {
    return item is WParagraph ? this.CheckPara(item as WParagraph) : this.CheckTable(item as WTable);
  }

  private bool CheckPara(WParagraph para)
  {
    foreach (ParagraphItem paragraphItem in (CollectionImpl) para.Items)
    {
      if (paragraphItem is WComment)
        return false;
    }
    return true;
  }

  private bool CheckTable(WTable table)
  {
    bool flag = true;
    foreach (WTableRow row in (CollectionImpl) table.Rows)
    {
      foreach (WTextBody cell in (CollectionImpl) row.Cells)
      {
        foreach (IEntity childEntity in (CollectionImpl) cell.ChildEntities)
        {
          flag = !(childEntity is WParagraph) ? this.CheckTable(childEntity as WTable) : this.CheckPara(childEntity as WParagraph);
          if (!flag)
            return false;
        }
      }
    }
    return flag;
  }

  private void RemoveFirstItem(ParagraphItem firstItem, ParagraphItem lastItem)
  {
    WParagraph ownerParagraph1 = firstItem.OwnerParagraph;
    if (ownerParagraph1.GetIndexInOwnerCollection() > 0)
    {
      firstItem.RemoveSelf();
    }
    else
    {
      WTable wtable1 = (WTable) null;
      if (ownerParagraph1.IsInCell)
        wtable1 = (ownerParagraph1.GetOwnerEntity() as WTableCell).OwnerRow.OwnerTable;
      if (wtable1 == null)
      {
        if (ownerParagraph1.Items.Count > 1)
          firstItem.RemoveSelf();
        else
          ownerParagraph1.RemoveSelf();
      }
      else
      {
        WParagraph ownerParagraph2 = lastItem.OwnerParagraph;
        WTable wtable2 = (WTable) null;
        if (ownerParagraph2.IsInCell)
          wtable2 = (ownerParagraph2.GetOwnerEntity() as WTableCell).OwnerRow.OwnerTable;
        if (wtable1 != wtable2 && ownerParagraph1.Owner == wtable1.FirstRow.Cells[0])
          wtable1.RemoveSelf();
        else
          firstItem.RemoveSelf();
      }
    }
  }

  private void FillCommItems()
  {
    foreach (TextBodyItem bodyItem in (CollectionImpl) this.m_bodyPart.BodyItems)
    {
      if (bodyItem is WParagraph)
        this.FillCommItems(bodyItem as WParagraph);
      else
        this.FillCommItems(bodyItem as WTable);
    }
  }

  private void FillCommItems(WParagraph para)
  {
    foreach (ParagraphItem paragraphItem in (CollectionImpl) para.Items)
      this.CommentedItems.InnerList.Add((object) paragraphItem);
  }

  private void FillCommItems(WTable table)
  {
    foreach (WTableRow row in (CollectionImpl) table.Rows)
    {
      foreach (WTextBody cell in (CollectionImpl) row.Cells)
      {
        foreach (IEntity childEntity in (CollectionImpl) cell.ChildEntities)
        {
          if (childEntity is WParagraph)
            this.FillCommItems(childEntity as WParagraph);
          else
            this.FillCommItems(childEntity as WTable);
        }
      }
    }
  }

  private string ModifyText(string text)
  {
    text = text.Replace(ControlChar.CrLf, ControlChar.CarriegeReturn);
    text = text.Replace(ControlChar.LineFeedChar, '\r');
    return text;
  }

  internal string SetParentParaIdAndIsResolved(List<string> paraIdOfComments)
  {
    foreach (WCommentExtended wcommentExtended in (CollectionImpl) this.m_doc.CommentsEx)
    {
      if (this.ChildEntities.LastItem is WParagraph && wcommentExtended.ParaId == (this.ChildEntities.LastItem as WParagraph).ParaId)
      {
        this.ParentParaId = wcommentExtended.ParentParaId;
        this.IsResolved = wcommentExtended.IsResolved;
        break;
      }
    }
    if (string.IsNullOrEmpty(this.ParentParaId) || !paraIdOfComments.Contains(this.ParentParaId))
      return string.Empty;
    for (int index = this.m_doc.Comments.InnerList.IndexOf((object) this) - 1; index >= 0; --index)
    {
      WComment comment = this.m_doc.Comments[index];
      if (string.IsNullOrEmpty(comment.ParentParaId))
      {
        string paraId = (comment.ChildEntities.LastItem as WParagraph).ParaId;
        return this.m_doc.Comments[index + 1] != null && paraId == this.m_doc.Comments[index + 1].ParentParaId ? paraId : string.Empty;
      }
      if (!paraIdOfComments.Contains(comment.ParentParaId))
        return string.Empty;
    }
    return string.Empty;
  }

  private WComment GetAncestorComment()
  {
    if (!string.IsNullOrEmpty(this.ParentParaId))
    {
      for (int index = this.m_doc.Comments.InnerList.IndexOf((object) this) - 1; index >= 0; --index)
      {
        WComment comment = this.m_doc.Comments[index];
        if (comment.ChildEntities.LastItem is WParagraph && (comment.ChildEntities.LastItem as WParagraph).ParaId == this.ParentParaId)
          return comment;
      }
    }
    return (WComment) null;
  }
}
