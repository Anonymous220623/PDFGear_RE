// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.PathFractionRangeHandler
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal class PathFractionRangeHandler : IEnumerable<VisiblePanelItem>, IEnumerable
{
  private const int MinimumIndexValue = -1;
  private LinkedList<VisiblePanelItem> _ChildIndexPair;
  private int firstVisibleItemIndex;
  private int lastVisibleItemIndex;
  private LinkedList<VisiblePanelItem> visibleItems;

  public PathFractionRangeHandler()
  {
    this.firstVisibleItemIndex = -1;
    this.lastVisibleItemIndex = -1;
    this.visibleItems = new LinkedList<VisiblePanelItem>();
    this.ToCleanUp = new LinkedList<VisiblePanelItem>();
  }

  internal PathFractionRangeHandler(LinkedList<VisiblePanelItem> visibleItems)
    : this()
  {
    this.visibleItems = visibleItems;
  }

  public int Count => this.visibleItems.Count;

  public VisiblePanelItem First
  {
    get
    {
      return this.visibleItems.First != null ? this.visibleItems.First.Value : (VisiblePanelItem) null;
    }
  }

  public int FirstVisibleItemIndex => this.First == null ? -1 : this.First.Index;

  public bool HasVisibleItems => this.visibleItems.Count > 0;

  public VisiblePanelItem Last
  {
    get => this.visibleItems.Last != null ? this.visibleItems.Last.Value : (VisiblePanelItem) null;
  }

  public int LastVisibleItemIndex => this.Last == null ? -1 : this.Last.Index;

  public LinkedList<VisiblePanelItem> ToCleanUp
  {
    get => this._ChildIndexPair;
    private set => this._ChildIndexPair = value;
  }

  public void AddFirst(LinkedList<VisiblePanelItem> pairs)
  {
    if (pairs == null)
      throw new ArgumentNullException(nameof (pairs));
    if (pairs.Count <= 0)
      return;
    for (LinkedListNode<VisiblePanelItem> linkedListNode = pairs.Last; linkedListNode != null; linkedListNode = linkedListNode.Previous)
      this.AddFirst(linkedListNode.Value);
  }

  public void AddFirst(VisiblePanelItem pair)
  {
    if (pair == null)
      throw new ArgumentNullException(nameof (pair));
    this.visibleItems.AddFirst(pair);
  }

  public void AddLast(LinkedList<VisiblePanelItem> pairs)
  {
    if (pairs == null)
      throw new ArgumentNullException(nameof (pairs));
    if (pairs.Count <= 0)
      return;
    foreach (VisiblePanelItem pair in pairs)
      this.AddLast(pair);
  }

  public void AddLast(VisiblePanelItem pair)
  {
    if (pair == null)
      throw new ArgumentNullException(nameof (pair));
    this.visibleItems.AddLast(pair);
  }

  public void Clear()
  {
    this.visibleItems.Clear();
    this.ToCleanUp.Clear();
  }

  public void ClearCleanUp() => this.ToCleanUp.Clear();

  public IEnumerator<VisiblePanelItem> GetEnumerator()
  {
    return (IEnumerator<VisiblePanelItem>) this.visibleItems.GetEnumerator();
  }

  public int GetVisibleItemsCount()
  {
    return this.HasVisibleItems ? this.lastVisibleItemIndex - this.firstVisibleItemIndex + 1 : 0;
  }

  public bool IsInVisibleRange(int index)
  {
    return index >= 0 && this.HasVisibleItems && index >= this.firstVisibleItemIndex && index <= this.lastVisibleItemIndex;
  }

  public void Remove(VisiblePanelItem pair) => this.visibleItems.Remove(pair);

  public void ScheduleClean(IList<VisiblePanelItem> pairs)
  {
    foreach (VisiblePanelItem pair in (IEnumerable<VisiblePanelItem>) pairs)
    {
      if (this.visibleItems.Remove(pair))
        this.ToCleanUp.AddFirst(pair);
    }
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.visibleItems.GetEnumerator();

  public void UpdateVisibleRange(VisibleRangeAction action, LinkedList<VisiblePanelItem> pairs)
  {
    LinkedList<VisiblePanelItem> pairs1 = new LinkedList<VisiblePanelItem>();
    foreach (VisiblePanelItem pair in pairs)
    {
      if (!this.visibleItems.Contains(pair))
        pairs1.AddFirst(pair);
    }
    if (action == VisibleRangeAction.AddFromEnd)
      this.AddLast(pairs1);
    if (action == VisibleRangeAction.AddFromStart)
      this.AddFirst(pairs1);
    if (action == VisibleRangeAction.RemoveFromEnd || action == VisibleRangeAction.RemoveFromStart)
      throw new NotImplementedException();
  }
}
