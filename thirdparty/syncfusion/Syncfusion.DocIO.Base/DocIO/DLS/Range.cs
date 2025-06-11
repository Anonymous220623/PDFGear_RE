// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Range
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class Range : CollectionImpl
{
  internal IList Items => this.InnerList;

  internal Range(WordDocument doc, OwnerHolder owner)
    : base(doc, owner)
  {
  }

  internal Range()
  {
  }

  internal void CloneItemsTo(Range range)
  {
    int index = 0;
    for (int count = this.Count; index < count; ++index)
    {
      if (this.InnerList[index] is Entity inner)
        range.Items.Add((object) inner);
    }
  }

  internal bool ContainTextBodyItems()
  {
    bool flag = false;
    foreach (Entity entity in (IEnumerable) this.Items)
    {
      if (entity is WParagraph || entity is WTable)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  internal int GetLastParagraphItemIndex()
  {
    int paragraphItemIndex = 0;
    foreach (Entity entity in (IEnumerable) this.Items)
    {
      switch (entity)
      {
        case WParagraph _:
        case WTable _:
          goto label_8;
        default:
          paragraphItemIndex = this.Items.IndexOf((object) entity);
          continue;
      }
    }
label_8:
    return paragraphItemIndex;
  }
}
