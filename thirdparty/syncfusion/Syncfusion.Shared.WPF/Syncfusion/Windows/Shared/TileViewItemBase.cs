// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TileViewItemBase
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class TileViewItemBase : TileViewItemAnimationBase
{
  private const string FloatPanelArea = "FloatPanelArea";
  private static int currZIndex = 1;
  private Point dragCompletedPoint;
  private bool dragging;
  public UIElement UIElementObj;
  internal static readonly DependencyProperty IsMovableProperty = DependencyProperty.Register(nameof (IsMovable), typeof (bool), typeof (TileViewItemBase), new PropertyMetadata((object) true));

  public event TileViewDragEventHandler DragStartedEvent;

  public event TileViewDragEventHandler DragMouseMoveEvent;

  public event TileViewDragEventHandler DragCompletedEvent;

  public event TileViewDragEventHandler PanelFocused;

  internal bool IsMovable
  {
    get => (bool) this.GetValue(TileViewItemBase.IsMovableProperty);
    set => this.SetValue(TileViewItemBase.IsMovableProperty, (object) value);
  }

  internal static int CurrentZIndex
  {
    get => TileViewItemBase.currZIndex;
    set => TileViewItemBase.currZIndex = value;
  }

  internal Point DragCompletedPoint
  {
    get => this.dragCompletedPoint;
    set => this.dragCompletedPoint = value;
  }

  internal TileViewItemBase()
  {
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.IsMovable = this.IsMovable;
    bool flag = true;
    if (this.ParentTileViewControl != null)
      flag = this.ParentTileViewControl.AllowItemRepositioning;
    if (!flag || !(this.GetTemplateChild("FloatPanelArea") is FrameworkElement templateChild))
      return;
    templateChild.MouseLeftButtonDown += new MouseButtonEventHandler(this.FloatPanelArea_MouseLeftButtonDown);
    templateChild.MouseMove += new MouseEventHandler(this.FloatPanelArea_MouseMove);
    templateChild.MouseLeftButtonUp += new MouseButtonEventHandler(this.FloatPanelArea_MouseLeftButtonUp);
  }

  public virtual void UpdateCoordinate(Point Pt)
  {
    Canvas.SetLeft(this.UIElementObj, Math.Max(0.0, Pt.X));
    Canvas.SetTop(this.UIElementObj, Math.Max(0.0, Pt.Y));
  }

  public virtual void UpdateFloatPanelSize(double Width, double Height)
  {
    Width = Math.Max(this.MinWidth, Width);
    Height = Math.Max(this.MinHeight, Height);
  }

  public void FloatPanelArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    bool flag = true;
    if (this.ParentTileViewControl != null)
      flag = this.ParentTileViewControl.AllowItemRepositioning;
    TileViewItem tileViewItem = (TileViewItem) null;
    if (flag && this.IsMovable)
      tileViewItem = VisualUtils.FindAncestor(sender as Visual, typeof (TileViewItem)) as TileViewItem;
    if (tileViewItem == null || tileViewItem.TileViewItemState == TileViewItemState.Maximized)
      return;
    ((UIElement) sender).CaptureMouse();
    this.dragCompletedPoint = e.GetPosition((IInputElement) (sender as UIElement));
    this.dragging = true;
    if (this.DragStartedEvent == null)
      return;
    this.DragStartedEvent((object) this, new TileViewDragEventArgs(0.0, 0.0, (MouseEventArgs) e, string.Empty));
  }

  public void FloatPanelArea_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    ((UIElement) sender).ReleaseMouseCapture();
    this.dragging = false;
    Point position = e.GetPosition((IInputElement) (sender as UIElement));
    TileViewDragEventArgs args = new TileViewDragEventArgs(position.X - this.dragCompletedPoint.X, position.Y - this.dragCompletedPoint.Y, (MouseEventArgs) e, string.Empty);
    if (this.ParentTileViewControl.IsVirtualizing && this.ParentTileViewControl.maximizedItem == null)
    {
      this.ParentTileViewControl.UpdateTileViewLayout();
      (this.ParentTileViewControl.itemsPanel as TileViewVirtualizingPanel).InvalidateMeasure();
    }
    if (this.DragCompletedEvent == null)
      return;
    this.DragCompletedEvent((object) this, args);
  }

  public void FloatPanelArea_MouseMove(object sender, MouseEventArgs e)
  {
    if (!this.dragging || !this.IsMovable || this.ParentTileViewControl.IsVirtualizing && this.ParentTileViewControl.maximizedItem != null)
      return;
    Point position = e.GetPosition((IInputElement) (sender as UIElement));
    double x = Convert.ToDouble(Canvas.GetLeft((UIElement) this) + position.X - this.dragCompletedPoint.X);
    double y = Canvas.GetTop((UIElement) this) + position.Y - this.dragCompletedPoint.Y;
    if (Math.Abs(position.X - this.dragCompletedPoint.X) <= 0.0 && Math.Abs(position.Y - this.dragCompletedPoint.Y) <= 0.0)
      return;
    this.UpdateCoordinate(new Point(x, y));
    if (this.ParentTileViewControl.IsVirtualizing)
    {
      this.ParentTileViewControl.Timer.Start();
      (this.ParentTileViewControl.itemsPanel as TileViewVirtualizingPanel).InvalidateMeasure();
    }
    if (this.DragMouseMoveEvent == null)
      return;
    this.DragMouseMoveEvent((object) this, new TileViewDragEventArgs(position.X - this.dragCompletedPoint.X, position.Y - this.dragCompletedPoint.Y, e, string.Empty));
  }

  protected override void OnGotFocus(RoutedEventArgs e)
  {
    base.OnGotFocus(e);
    Panel.SetZIndex((UIElement) this, TileViewItemBase.CurrentZIndex++);
    if (this.PanelFocused == null)
      return;
    this.PanelFocused((object) this, (TileViewDragEventArgs) null);
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (this.dragging)
    {
      base.OnMouseLeftButtonDown(e);
      Panel.SetZIndex((UIElement) this, TileViewItemBase.CurrentZIndex++);
      if (this.PanelFocused == null)
        return;
      this.PanelFocused((object) this, (TileViewDragEventArgs) null);
    }
    else
      e.Handled = true;
  }
}
