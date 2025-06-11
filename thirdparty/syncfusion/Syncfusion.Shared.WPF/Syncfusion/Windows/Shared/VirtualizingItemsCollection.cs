// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.VirtualizingItemsCollection
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal class VirtualizingItemsCollection
{
  private int _ItemsCount;
  private int _ItemsPerPage;
  private int firstVisibleIndex;
  private int lastVisibleIndex;
  private int offset;

  public VirtualizingItemsCollection(int itemsPerPage, int itemsCount)
  {
    this.Offset = 0;
    this.ItemsPerPage = itemsPerPage;
    this.ItemsCount = itemsCount;
  }

  public VirtualizingItemsCollection(int itemsPerPage, int itemsCount, int offset)
  {
    this.ItemsPerPage = itemsPerPage;
    this.ItemsCount = itemsCount;
    this.Offset = offset;
  }

  public int CountAfter
  {
    get
    {
      if (this.LastVisibleIndex >= 0)
        return this.ItemsCount - this.LastVisibleIndex - 1;
      return this.Offset == 0 ? this.ItemsCount : 0;
    }
  }

  public int CountBefore
  {
    get
    {
      if (this.FirstVisibleIndex > 0)
        return this.FirstVisibleIndex;
      return this.OffsetIsMaximum ? this.ItemsPerPage : 0;
    }
  }

  public int FirstVisibleIndex => this.firstVisibleIndex;

  public int FreePosition => this.ItemsPerPage - this.VisibleItemsCount;

  public bool HasMoreItems => this.VisibleItemsCount < this.ItemsCount;

  public bool HasReachedEnd => this.Offset >= this.ItemsCount + this.ItemsPerPage;

  public bool HasVisibleItems => this.LastVisibleIndex >= 0 && this.FirstVisibleIndex >= 0;

  public int ItemsCount
  {
    get => this._ItemsCount;
    private set => this._ItemsCount = value;
  }

  public int ItemsPerPage
  {
    get => this._ItemsPerPage;
    private set => this._ItemsPerPage = value;
  }

  public int LastVisibleIndex => this.lastVisibleIndex;

  public int Offset
  {
    get => this.offset;
    private set
    {
      this.offset = value;
      this.firstVisibleIndex = this.GetFirstVisibleIndex();
      this.lastVisibleIndex = this.GetLastVisibleIndex();
    }
  }

  public bool OffsetIsMaximum => this.Offset == this.ItemsCount + this.ItemsPerPage;

  public bool PageFull => this.VisibleItemsCount == this.ItemsPerPage;

  public int VisibleItemsCount
  {
    get => this.HasVisibleItems ? this.LastVisibleIndex - this.FirstVisibleIndex + 1 : 0;
  }

  public void ModifyOffsetAfterItemAdded(int newIndexPosition, int count)
  {
    if (newIndexPosition <= this.GetFirstVisibleIndex())
      this.Offset += count;
    this.ItemsCount += count;
    this.Offset = this.Offset;
  }

  public void ModifyOffsetAfterItemRemoved(int index, int count)
  {
    if (index <= this.GetFirstVisibleIndex())
      this.Offset -= count;
    this.ItemsCount -= count;
    this.Offset = this.Offset;
  }

  private int GetFirstVisibleIndex()
  {
    for (int itemsPerPage = this.ItemsPerPage; itemsPerPage > 0; --itemsPerPage)
    {
      int firstVisibleIndex = this.Offset - itemsPerPage;
      if (firstVisibleIndex >= 0 && firstVisibleIndex < this.ItemsCount)
        return firstVisibleIndex;
    }
    return -1;
  }

  private int GetLastVisibleIndex()
  {
    for (int index = 1; index <= this.ItemsPerPage; ++index)
    {
      int lastVisibleIndex = this.Offset - index;
      if (lastVisibleIndex < this.ItemsCount)
        return lastVisibleIndex;
    }
    return -1;
  }

  public int GetPosition(int index, bool isLooping, int selectedIndex)
  {
    if (isLooping)
    {
      int num1 = this.ItemsPerPage % 2 == 0 ? this.ItemsPerPage / 2 - 1 : this.ItemsPerPage / 2;
      int num2 = this.ItemsPerPage % 2 == 0 ? num1 + 1 : num1;
      int min1 = selectedIndex >= 0 ? selectedIndex - num1 : 0;
      if (min1 >= 0)
      {
        int num3 = selectedIndex + num2 - (this.ItemsCount - 1);
        if (CarouselPanelHelperMethods.IsInRange(index, min1, min1 + this.ItemsPerPage - 1))
          return Math.Abs(this.ItemsPerPage - (index - min1) - 1);
        if (CarouselPanelHelperMethods.IsInRange(index, 0, num3 - 1))
          return Math.Abs(num3 - index - 1);
      }
      else
      {
        int min2 = this.ItemsCount + min1;
        int max = min1 >= 0 ? this.ItemsPerPage - 1 : this.ItemsPerPage + min1 - 1;
        if (CarouselPanelHelperMethods.IsInRange(index, 0, max))
          return Math.Abs(this.ItemsPerPage - (index - min1) - 1);
        if (CarouselPanelHelperMethods.IsInRange(index, min2, this.ItemsCount - 1))
          return Math.Abs(this.ItemsPerPage - (index - min2) - 1);
      }
    }
    else if (index >= 0 && CarouselPanelHelperMethods.IsInRange(index, this.FirstVisibleIndex, this.LastVisibleIndex))
      return Math.Abs(index - (this.Offset - 1));
    return -1;
  }

  private bool IsBeforeVisibleRange(int index)
  {
    int num = this.Offset - this.ItemsPerPage;
    return index < num;
  }

  public void Move(int displacement)
  {
    if (displacement > 0)
      this.MoveRight(displacement);
    else
      this.MoveLeft(displacement);
  }

  public void MoveLeft(int displacement)
  {
    this.Offset = CarouselPanelHelperMethods.CoerceRangeValues(this.Offset - Math.Abs(displacement), 0, this.ItemsPerPage + this.ItemsCount);
  }

  public void MoveRight(int displacement)
  {
    Math.Abs(displacement);
    this.Offset = CarouselPanelHelperMethods.CoerceRangeValues(this.Offset + displacement, 0, this.ItemsPerPage + this.ItemsCount);
  }

  public void TryFill()
  {
    if (this.PageFull)
      return;
    int countBefore = this.CountBefore;
    int countAfter = this.CountAfter;
    if (countAfter >= countBefore)
      this.Offset += Math.Min(Math.Max(this.FreePosition, countAfter), this.FreePosition);
    else
      this.Offset -= Math.Min(Math.Max(this.FreePosition, countBefore), this.FreePosition);
  }
}
