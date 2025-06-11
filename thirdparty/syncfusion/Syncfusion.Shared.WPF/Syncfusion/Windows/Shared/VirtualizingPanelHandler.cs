// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.VirtualizingPanelHandler
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal abstract class VirtualizingPanelHandler
{
  private VisibleItemsHandler initialPathPositionStates;
  private bool isInitialized;
  private CarouselPathHelper carouselPathHelper;
  private TimeSpan duration = new TimeSpan(0, 0, 0, 0, 300);
  private TimeSpan lastRender = new TimeSpan(0, 0, 0);
  private TimeSpan totalRunningTime = new TimeSpan(0, 0, 0);
  private ItemMovementState state;

  public CarouselPathHelper CarouselPathHelper
  {
    get => this.carouselPathHelper;
    protected set => this.carouselPathHelper = value;
  }

  public TimeSpan Duration
  {
    get => this.duration;
    set
    {
      this.duration = value;
      if (!(this.duration <= this.totalRunningTime) || this.state != ItemMovementState.Started)
        return;
      this.EndItemMovement();
      this.state = ItemMovementState.Finished;
    }
  }

  public VisibleItemsHandler InitialPathPositionStates
  {
    get => this.initialPathPositionStates;
    set => this.initialPathPositionStates = value;
  }

  public bool IsInitialized => this.isInitialized;

  public TimeSpan LastRender
  {
    get => this.lastRender;
    protected set => this.lastRender = value;
  }

  public ItemMovementState State
  {
    get => this.state;
    protected internal set => this.state = value;
  }

  public TimeSpan TotalRunningTime
  {
    get => this.totalRunningTime;
    protected set => this.totalRunningTime = value;
  }

  public abstract void CalculateItemsToAdd(
    out VisibleRangeAction action,
    out LinkedList<VisiblePanelItem> itemsToAdd,
    bool isLooping,
    int selectedIndex);

  public abstract void AddItemToMove(
    VisiblePanelItem item,
    bool isLooping,
    int selectedIndex,
    bool isNextPage,
    bool isPreviousPage);

  protected abstract void Animate(double percentageDone);

  public abstract void EndItemMovement();

  public abstract void Reverse();

  public abstract IList<VisiblePanelItem> GetItemsToRemoveEndofArrangeOverride();

  public virtual void BeginItemMovement(TimeSpan beginTime)
  {
    if (!this.isInitialized || this.state != ItemMovementState.NotStarted)
      return;
    this.lastRender = beginTime;
    this.state = ItemMovementState.Started;
  }

  public void Initialize(CarouselPathHelper PathHelper, VisibleItemsHandler positionStates)
  {
    if (PathHelper == null)
      throw new ArgumentNullException("path");
    if (positionStates == null)
      throw new ArgumentNullException(nameof (positionStates));
    this.CarouselPathHelper = PathHelper;
    if (this.initialPathPositionStates == null)
      this.initialPathPositionStates = positionStates;
    this.Initialize();
    this.isInitialized = true;
  }

  protected virtual void Initialize()
  {
  }

  private static Point CalculateNewPosition(UIElement item, CarouselPathHelper carouselPathHelper)
  {
    double pathFraction = CustomPathCarouselPanel.GetPathFraction(item);
    try
    {
      Point point;
      carouselPathHelper.Geometry.GetPointAtFractionLength(pathFraction, out point, out Point _);
      return point;
    }
    catch
    {
      return new Point();
    }
  }

  public static MatrixTransform RecalculateItemPosition(
    UIElement item,
    CarouselPathHelper carouselPathHelper)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    if (carouselPathHelper == null)
      throw new ArgumentNullException("animationPath");
    Matrix identity = Matrix.Identity;
    Point newPosition = VirtualizingPanelHandler.CalculateNewPosition(item, carouselPathHelper);
    identity.Translate(newPosition.X, newPosition.Y);
    VirtualizingPanelHandler.CustomPathCenterItem(item, ref identity);
    return new MatrixTransform(identity);
  }

  private static void CustomPathCenterItem(UIElement item, ref Matrix itemTransform)
  {
    Size renderSize = item.RenderSize;
    itemTransform.Translate(-(renderSize.Width / 2.0), -(renderSize.Height / 2.0));
  }

  protected static void SetPathFractionManagerForItem(
    UIElement item,
    double startPathFraction,
    double endPathFraction)
  {
    if (startPathFraction == 0.0 && endPathFraction == 1.0 || startPathFraction == 1.0 && endPathFraction == 0.0)
      endPathFraction = startPathFraction;
    PathFractionManager pathFractionManager = new PathFractionManager(endPathFraction, startPathFraction);
    if (pathFractionManager == null)
      return;
    CustomPathCarouselPanel.SetPathFractionManager(item, pathFractionManager);
  }

  protected static void EndItemMovement(
    List<VisiblePanelItem> collection,
    CarouselPathHelper animationPath)
  {
    foreach (VisiblePanelItem visiblePanelItem in collection)
    {
      PathFractionManager pathFractionManager = CustomPathCarouselPanel.GetPathFractionManager(visiblePanelItem.Child);
      if (pathFractionManager != null)
        CustomPathCarouselPanel.SetPathFraction(visiblePanelItem.Child, pathFractionManager.NewPathFraction);
      VirtualizingPanelHandler.RecalculateItemPosition(visiblePanelItem.Child, animationPath);
    }
  }

  public void Update(TimeSpan currentTime)
  {
    if (this.state != ItemMovementState.Started)
      return;
    this.totalRunningTime = this.totalRunningTime.Add(currentTime.Subtract(this.lastRender));
    this.lastRender = currentTime;
    if (this.totalRunningTime < this.duration)
    {
      this.Animate(this.GetPercentageDone());
    }
    else
    {
      this.EndItemMovement();
      this.state = ItemMovementState.Finished;
    }
  }

  public double GetPercentageDone()
  {
    return this.totalRunningTime.TotalSeconds / this.duration.TotalSeconds;
  }

  public TimeSpan GetTimeLeft()
  {
    return this.duration <= this.totalRunningTime ? TimeSpan.Zero : this.duration.Subtract(this.totalRunningTime);
  }

  protected static void ReverseAnimationdata(List<VisiblePanelItem> collection)
  {
    foreach (VisiblePanelItem visiblePanelItem in collection)
    {
      PathFractionManager pathFractionManager1 = CustomPathCarouselPanel.GetPathFractionManager(visiblePanelItem.Child);
      if (pathFractionManager1 != null)
      {
        PathFractionManager pathFractionManager2 = new PathFractionManager(pathFractionManager1.CurrentPathFraction, pathFractionManager1.NewPathFraction);
        CustomPathCarouselPanel.SetPathFractionManager(visiblePanelItem.Child, pathFractionManager2);
      }
    }
  }

  protected static void UpdateCustomPathItemFraction(UIElement item, double animationPercentageDone)
  {
    PathFractionManager pathFractionManager = CustomPathCarouselPanel.GetPathFractionManager(item);
    if (pathFractionManager == null)
      return;
    double num = pathFractionManager.NewPathFraction - pathFractionManager.CurrentPathFraction;
    CustomPathCarouselPanel.SetPathFraction(item, pathFractionManager.CurrentPathFraction + num * animationPercentageDone);
  }

  protected void UpdateItemTransformation(UIElement item)
  {
    Matrix matrix = VirtualizingPanelHandler.RecalculateItemPosition(item, this.CarouselPathHelper).Matrix;
    matrix.Translate(0.0, 0.0);
    item.RenderTransform = (Transform) new MatrixTransform(matrix);
  }
}
