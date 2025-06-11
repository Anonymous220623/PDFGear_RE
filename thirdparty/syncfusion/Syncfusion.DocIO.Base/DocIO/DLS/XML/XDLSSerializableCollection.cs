// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.XML.XDLSSerializableCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.DocIO.DLS.XML;

public abstract class XDLSSerializableCollection(WordDocument doc, OwnerHolder owner) : 
  CollectionImpl(doc, owner),
  IXDLSSerializableCollection,
  IEnumerable
{
  IXDLSSerializable IXDLSSerializableCollection.AddNewItem(IXDLSContentReader reader)
  {
    OwnerHolder ownerHolder = this.CreateItem(reader);
    if (ownerHolder != null)
    {
      this.InnerList.Add((object) ownerHolder);
      ownerHolder.SetOwner(this.OwnerBase);
    }
    return ownerHolder as IXDLSSerializable;
  }

  string IXDLSSerializableCollection.TagItemName => this.GetTagItemName();

  internal virtual void CloneToImpl(CollectionImpl coll)
  {
    foreach (XDLSSerializableBase inner in (IEnumerable) this.InnerList)
    {
      object obj = inner.CloneInt();
      coll.InnerList.Add(obj);
      if (obj is OwnerHolder)
        (obj as OwnerHolder).SetOwner(coll.OwnerBase);
    }
  }

  protected abstract string GetTagItemName();

  protected abstract OwnerHolder CreateItem(IXDLSContentReader reader);
}
