// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.VirtualizingPanelItemMoveHandler
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal class VirtualizingPanelItemMoveHandler : VirtualizingPanelHandler
{
  private VirtualizingItemsCollection _VirtualizingItemsCollection;
  private List<VisiblePanelItem> newItemsToExit;
  private List<VisiblePanelItem> newItemsToStay;
  private List<VisiblePanelItem> oldItemsToExit;
  private List<VisiblePanelItem> oldItemsToStay;
  private int pathDisplacement;

  public VirtualizingPanelItemMoveHandler()
  {
  }

  public VirtualizingPanelItemMoveHandler(int displacement, CarouselPanelHelper infoProvider)
  {
    this.pathDisplacement = displacement;
    this.oldItemsToExit = new List<VisiblePanelItem>();
    this.newItemsToExit = new List<VisiblePanelItem>();
    this.oldItemsToStay = new List<VisiblePanelItem>();
    this.newItemsToStay = new List<VisiblePanelItem>();
    this.Collection = new VirtualizingItemsCollection(infoProvider.PageSize, infoProvider.ItemsCount, infoProvider.Position);
  }

  public VirtualizingItemsCollection Collection
  {
    get => this._VirtualizingItemsCollection;
    set => this._VirtualizingItemsCollection = value;
  }

  public int PathDisplacement => this.pathDisplacement;

  public override void AddItemToMove(
    VisiblePanelItem item,
    bool isLooping,
    int selectedIndex,
    bool isNextPage,
    bool isPreviousPage)
  {
    if (item == null)
      return;
    if (CustomPathCarouselPanel.GetPathFraction(item.Child) < 0.0)
      this.AddNewItem(item, isLooping, selectedIndex, isNextPage, isPreviousPage);
    else
      this.AddExistingItem(item, isLooping, selectedIndex, isNextPage, isPreviousPage);
  }

  protected override void Initialize() => this.Collection.Move(this.pathDisplacement);

  private void AddExistingItem(
    VisiblePanelItem pair,
    bool isLooping,
    int selectedIndex,
    bool isNextPage,
    bool isPreviousPage)
  {
    UIElement child = pair.Child;
    double startPathFraction = CustomPathCarouselPanel.GetPathFraction(child);
    double nextPathFraction = this.CalculateNextPathFraction(pair.Index, isLooping, selectedIndex);
    if (isLooping && this.Collection.ItemsCount - this.Collection.ItemsPerPage < Math.Abs(this.PathDisplacement))
    {
      double num1 = this.Collection.ItemsPerPage % 2 == 0 ? Math.Round(1.0 / (double) (this.Collection.ItemsPerPage + 2), 3) : Math.Round(1.0 / (double) (this.Collection.ItemsPerPage + 1), 3);
      double num2 = Math.Round(Math.Abs(nextPathFraction - startPathFraction), 3);
      double num3 = Math.Round((double) Math.Abs(this.PathDisplacement) * num1, 3);
      if (startPathFraction > 0.0 && startPathFraction < 1.0 && nextPathFraction > 0.0 && nextPathFraction < 1.0 && (num2 == num3 && (isNextPage || isPreviousPage) || num2 != num3 && num2 > num1 && (num2 > num3 || num2 % num1 == 0.0 || isNextPage || isPreviousPage)))
        startPathFraction = this.PathDisplacement < 0 ? 1.0 : 0.0;
    }
    VirtualizingPanelHandler.SetPathFractionManagerForItem(child, startPathFraction, nextPathFraction);
    CustomPathCarouselPanel.SetPathFraction(child, nextPathFraction);
    switch (nextPathFraction.ToString())
    {
      case "0":
      case "1":
        this.oldItemsToExit.Add(pair);
        break;
      default:
        if (pair.Child is CarouselItem)
        {
          if ((pair.Child as CarouselItem).DataContext == null)
            break;
          if ((pair.Child as CarouselItem).DataContext.ToString() == "{DisconnectedItem}")
          {
            this.oldItemsToExit.Add(pair);
            break;
          }
          this.oldItemsToStay.Add(pair);
          break;
        }
        this.oldItemsToStay.Add(pair);
        break;
    }
  }

  public double CalculateNextPathFraction(int index, bool isLooping, int selectedIndex)
  {
    int position = this.Collection.GetPosition(index, isLooping, selectedIndex);
    return position == -1 ? this.GetExitPathFraction() : this.CarouselPathHelper.GetVisiblePathFraction(position).PathFraction;
  }

  public double GetExitPathFraction() => this.PathDisplacement >= 0 ? 1.0 : 0.0;

  private void AddNewItem(
    VisiblePanelItem pair,
    bool isLooping,
    int selectedIndex,
    bool isNextPage,
    bool isPreviousPage)
  {
    this.SetStartingPathFraction(pair.Child);
    this.CalculateNewItemAnimation(pair, isLooping, selectedIndex, isNextPage, isPreviousPage);
    if (pair.Child is CarouselItem)
    {
      if ((pair.Child as CarouselItem).DataContext == null)
        return;
      if ((pair.Child as CarouselItem).DataContext.ToString() == "{DisconnectedItem}")
        this.newItemsToExit.Add(pair);
      else
        this.newItemsToStay.Add(pair);
    }
    else
      this.newItemsToStay.Add(pair);
  }

  private void SetStartingPathFraction(UIElement item)
  {
    double num = -1.0;
    if (this.pathDisplacement <= 0)
      num = 1.0;
    else if (this.pathDisplacement > 0)
      num = 0.0;
    CustomPathCarouselPanel.SetPathFraction(item, num);
  }

  private void CalculateNewItemAnimation(
    VisiblePanelItem pair,
    bool isLooping,
    int selectedIndex,
    bool isNextPage,
    bool isPreviousPage)
  {
    double startPathFraction = CustomPathCarouselPanel.GetPathFraction(pair.Child);
    double nextPathFraction = this.CalculateNextPathFraction(pair.Index, isLooping, selectedIndex);
    if (startPathFraction == 0.0 && !isNextPage && !isPreviousPage)
    {
      double num = Math.Round((this.Collection.ItemsPerPage % 2 == 0 ? Math.Round(1.0 / (double) (this.Collection.ItemsPerPage + 2), 3) : Math.Round(1.0 / (double) (this.Collection.ItemsPerPage + 1), 3)) * (double) (this.Collection.ItemsPerPage / 2 + 1), 3);
      startPathFraction = nextPathFraction <= num ? 0.0 : 1.0;
    }
    VirtualizingPanelHandler.SetPathFractionManagerForItem(pair.Child, startPathFraction, nextPathFraction);
    CustomPathCarouselPanel.SetPathFraction(pair.Child, nextPathFraction);
  }

  public override void CalculateItemsToAdd(
    out VisibleRangeAction action,
    out LinkedList<VisiblePanelItem> itemsToAdd,
    bool isLooping,
    int selectedIndex)
  {
    itemsToAdd = new LinkedList<VisiblePanelItem>();
    action = this.PathDisplacement >= 0 ? VisibleRangeAction.AddFromEnd : VisibleRangeAction.AddFromStart;
    if (isLooping)
    {
      int num1 = this.Collection.ItemsPerPage % 2 == 0 ? this.Collection.ItemsPerPage / 2 - 1 : this.Collection.ItemsPerPage / 2;
      int num2 = selectedIndex >= 0 ? selectedIndex - num1 : 0;
      for (int index = 0; index < this.Collection.ItemsPerPage && this.Collection.ItemsCount > 0; ++index)
      {
        int num3 = num2 >= 0 ? num2 : this.Collection.ItemsCount - 1 - index;
        ++num2;
        VisiblePanelItem visiblePanelItem = new VisiblePanelItem((UIElement) null, num3 < this.Collection.ItemsCount ? num3 : num3 % this.Collection.ItemsCount);
        itemsToAdd.AddFirst(visiblePanelItem);
      }
    }
    else
    {
      for (int index = 0; index < this.Collection.VisibleItemsCount; ++index)
      {
        VisiblePanelItem visiblePanelItem = new VisiblePanelItem((UIElement) null, this.Collection.FirstVisibleIndex + index);
        itemsToAdd.AddFirst(visiblePanelItem);
      }
    }
  }

  public override IList<VisiblePanelItem> GetItemsToRemoveEndofArrangeOverride()
  {
    return (IList<VisiblePanelItem>) this.oldItemsToExit;
  }

  protected override void Animate(double percentageDone)
  {
    this.Animate(this.oldItemsToExit, percentageDone);
    this.Animate(this.oldItemsToStay, percentageDone);
    this.Animate(this.newItemsToExit, percentageDone);
    this.Animate(this.newItemsToStay, percentageDone);
  }

  private void Animate(List<VisiblePanelItem> collection, double percentageDone)
  {
    foreach (VisiblePanelItem visiblePanelItem in collection)
    {
      VirtualizingPanelHandler.UpdateCustomPathItemFraction(visiblePanelItem.Child, percentageDone);
      this.UpdateItemTransformation(visiblePanelItem.Child);
    }
  }

  public override void EndItemMovement()
  {
    VirtualizingPanelHandler.EndItemMovement(this.oldItemsToExit, this.CarouselPathHelper);
    VirtualizingPanelHandler.EndItemMovement(this.oldItemsToStay, this.CarouselPathHelper);
    VirtualizingPanelHandler.EndItemMovement(this.newItemsToExit, this.CarouselPathHelper);
    VirtualizingPanelHandler.EndItemMovement(this.newItemsToStay, this.CarouselPathHelper);
  }

  public bool IsOpposite(int displacement) => this.PathDisplacement == -displacement;

  public override void Reverse()
  {
    this.pathDisplacement = -this.pathDisplacement;
    this.TotalRunningTime = this.Duration.Subtract(this.TotalRunningTime);
    VirtualizingPanelHandler.ReverseAnimationdata(this.oldItemsToExit);
    VirtualizingPanelHandler.ReverseAnimationdata(this.oldItemsToStay);
    VirtualizingPanelHandler.ReverseAnimationdata(this.newItemsToExit);
    VirtualizingPanelHandler.ReverseAnimationdata(this.newItemsToStay);
    this.ReverseOldAndNewItems();
  }

  private void ReverseOldAndNewItems()
  {
    List<VisiblePanelItem> oldItemsToExit = this.oldItemsToExit;
    this.oldItemsToExit = new List<VisiblePanelItem>((IEnumerable<VisiblePanelItem>) this.newItemsToExit);
    this.oldItemsToExit.AddRange((IEnumerable<VisiblePanelItem>) this.newItemsToStay);
    this.newItemsToExit.Clear();
    this.newItemsToStay = new List<VisiblePanelItem>((IEnumerable<VisiblePanelItem>) oldItemsToExit);
  }
}
