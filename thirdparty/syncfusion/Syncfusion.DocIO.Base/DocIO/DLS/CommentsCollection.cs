// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.CommentsCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class CommentsCollection(WordDocument doc) : CollectionImpl(doc, (OwnerHolder) doc)
{
  public WComment this[int index] => this.InnerList[index] as WComment;

  public int Counts() => this.InnerList.Count;

  public void RemoveAt(int index)
  {
    WComment inner1 = this.InnerList[index] as WComment;
    string paraId = (inner1.ChildEntities.LastItem as WParagraph).ParaId;
    List<string> stringList = new List<string>();
    if (this.m_doc.CommentsEx.Count > 0)
    {
      foreach (WCommentExtended wcommentExtended in (CollectionImpl) this.m_doc.CommentsEx)
      {
        if (wcommentExtended.ParentParaId == paraId)
          stringList.Add(wcommentExtended.ParaId);
      }
    }
    this.InnerList.Remove((object) inner1);
    if (inner1.Owner is WParagraph)
      inner1.OwnerParagraph.Items.Remove((IEntity) inner1);
    if (stringList.Count <= 0)
      return;
    for (int index1 = 0; index1 < this.InnerList.Count; ++index1)
    {
      foreach (string str in stringList)
      {
        if (((this.InnerList[index1] as WComment).ChildEntities.LastItem as WParagraph).ParaId == str)
        {
          WComment inner2 = this.InnerList[index1] as WComment;
          this.InnerList.Remove((object) inner2);
          if (inner2.Owner is WParagraph)
            inner2.OwnerParagraph.Items.Remove((IEntity) inner2);
          --index1;
          break;
        }
      }
    }
  }

  public void Clear()
  {
    while (this.InnerList.Count > 0)
      this.RemoveAt(this.InnerList.Count - 1);
  }

  internal void Add(WComment comment) => this.InnerList.Add((object) comment);

  public void Remove(WComment comment)
  {
    this.InnerList.Remove((object) comment);
    comment.OwnerParagraph.Items.Remove((IEntity) comment);
    string paraId = (comment.ChildEntities.LastItem as WParagraph).ParaId;
    List<string> stringList = new List<string>();
    if (this.m_doc.CommentsEx.Count > 0)
    {
      foreach (WCommentExtended wcommentExtended in (CollectionImpl) this.m_doc.CommentsEx)
      {
        if (wcommentExtended.ParentParaId == paraId)
          stringList.Add(wcommentExtended.ParaId);
      }
    }
    if (stringList.Count <= 0)
      return;
    for (int index = 0; index < this.InnerList.Count; ++index)
    {
      foreach (string str in stringList)
      {
        if (((this.InnerList[index] as WComment).ChildEntities.LastItem as WParagraph).ParaId == str)
        {
          WComment inner = this.InnerList[index] as WComment;
          this.InnerList.Remove((object) inner);
          if (inner.Owner is WParagraph)
            inner.OwnerParagraph.Items.Remove((IEntity) inner);
          --index;
          break;
        }
      }
    }
  }

  internal void SetParentParaIDAndIsResolved()
  {
    if (this.Count <= 0)
      return;
    List<string> paraIdOfComments = new List<string>();
    foreach (WComment wcomment in (CollectionImpl) this)
    {
      if (wcomment.ChildEntities.LastItem is WParagraph)
        paraIdOfComments.Add((wcomment.ChildEntities.LastItem as WParagraph).ParaId);
    }
    foreach (WComment wcomment in (CollectionImpl) this)
      wcomment.ParentParaId = wcomment.SetParentParaIdAndIsResolved(paraIdOfComments);
  }
}
