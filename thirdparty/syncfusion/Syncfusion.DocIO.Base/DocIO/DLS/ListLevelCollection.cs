// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ListLevelCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS.XML;
using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class ListLevelCollection : XDLSSerializableCollection
{
  public WListLevel this[int index] => (WListLevel) this.InnerList[index];

  internal ListLevelCollection(ListStyle owner)
    : base(owner.Document, (OwnerHolder) owner)
  {
  }

  internal int Add(WListLevel level)
  {
    if (level == null)
      throw new ArgumentNullException(nameof (level));
    level.SetOwner(this.OwnerBase);
    return this.InnerList.Add((object) level);
  }

  internal int IndexOf(WListLevel level) => this.InnerList.IndexOf((object) level);

  internal void Clear() => this.InnerList.Clear();

  protected override OwnerHolder CreateItem(IXDLSContentReader reader)
  {
    return (OwnerHolder) new WListLevel(this.OwnerBase as ListStyle);
  }

  protected override string GetTagItemName() => "level";

  internal bool Compare(ListLevelCollection listLevels)
  {
    if (this.Count != listLevels.Count)
      return false;
    for (int index = 0; index < listLevels.Count; ++index)
    {
      if (!listLevels[index].Compare(this[index]))
        return false;
    }
    return true;
  }
}
