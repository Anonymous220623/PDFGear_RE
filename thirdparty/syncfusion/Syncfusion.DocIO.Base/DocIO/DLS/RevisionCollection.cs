// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.RevisionCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class RevisionCollection : CollectionImpl, ICollectionBase, IEnumerable
{
  public Revision this[int index] => this.InnerList[index] as Revision;

  public void AcceptAll()
  {
    while (this.Count > 0)
      this[0].Accept();
  }

  public void RejectAll()
  {
    while (this.Count > 0)
      this[0].Reject();
  }

  internal void Add(Revision revision)
  {
    this.InnerList.Add((object) revision);
    revision.Owner = (object) this;
  }

  internal void Remove(Revision revision) => this.InnerList.Remove((object) revision);

  internal void CloneItemsTo(RevisionCollection childRevisions)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      Revision revision = (this.InnerList[index] as Revision).Clone();
      if (revision != null)
        childRevisions.Add(revision);
    }
  }

  internal RevisionCollection(WordDocument doc)
    : base(doc, (OwnerHolder) doc)
  {
  }

  internal override void Close()
  {
    while (this.InnerList.Count > 0)
    {
      int index = this.InnerList.Count - 1;
      this[index].Close();
      this.Remove(this[index]);
    }
    base.Close();
  }
}
