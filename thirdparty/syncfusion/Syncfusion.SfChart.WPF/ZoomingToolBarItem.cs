// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ZoomingToolBarItem
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ZoomingToolBarItem : ContentControl, INotifyPropertyChanged
{
  public static readonly DependencyProperty IconBackgroundProperty = DependencyProperty.Register(nameof (IconBackground), typeof (Color), typeof (ZoomingToolBarItem), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty EnableColorProperty = DependencyProperty.Register(nameof (EnableColor), typeof (Color), typeof (ZoomingToolBarItem), new PropertyMetadata((object) Color.FromArgb((byte) 250, (byte) 12, (byte) 158, (byte) 239)));
  public static readonly DependencyProperty DisableColorProperty = DependencyProperty.Register(nameof (DisableColor), typeof (Color), typeof (ZoomingToolBarItem), new PropertyMetadata((object) Color.FromArgb((byte) 250, (byte) 179, (byte) 179, (byte) 179)));
  public static readonly DependencyProperty ToolBarIconHeightProperty = DependencyProperty.Register(nameof (ToolBarIconHeight), typeof (double), typeof (ZoomingToolBarItem), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ZoomingToolBarItem.OnSizeChanged)));
  public static readonly DependencyProperty ToolBarIconWidthProperty = DependencyProperty.Register(nameof (ToolBarIconWidth), typeof (double), typeof (ZoomingToolBarItem), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ZoomingToolBarItem.OnSizeChanged)));
  public static readonly DependencyProperty ToolBarIconMarginProperty = DependencyProperty.Register(nameof (ToolBarIconMargin), typeof (Thickness), typeof (ZoomingToolBarItem), new PropertyMetadata((object) new Thickness().GetThickness(0.0, 0.0, 0.0, 0.0), new PropertyChangedCallback(ZoomingToolBarItem.OnSizeChanged)));
  private ChartZoomPanBehavior source;

  public event PropertyChangedEventHandler PropertyChanged;

  public Color IconBackground
  {
    get => (Color) this.GetValue(ZoomingToolBarItem.IconBackgroundProperty);
    set => this.SetValue(ZoomingToolBarItem.IconBackgroundProperty, (object) value);
  }

  public Color EnableColor
  {
    get => (Color) this.GetValue(ZoomingToolBarItem.EnableColorProperty);
    set => this.SetValue(ZoomingToolBarItem.EnableColorProperty, (object) value);
  }

  public Color DisableColor
  {
    get => (Color) this.GetValue(ZoomingToolBarItem.DisableColorProperty);
    set => this.SetValue(ZoomingToolBarItem.DisableColorProperty, (object) value);
  }

  public double ToolBarIconHeight
  {
    get => (double) this.GetValue(ZoomingToolBarItem.ToolBarIconHeightProperty);
    set => this.SetValue(ZoomingToolBarItem.ToolBarIconHeightProperty, (object) value);
  }

  public double ToolBarIconWidth
  {
    get => (double) this.GetValue(ZoomingToolBarItem.ToolBarIconWidthProperty);
    set => this.SetValue(ZoomingToolBarItem.ToolBarIconWidthProperty, (object) value);
  }

  public Thickness ToolBarIconMargin
  {
    get => (Thickness) this.GetValue(ZoomingToolBarItem.ToolBarIconMarginProperty);
    set => this.SetValue(ZoomingToolBarItem.ToolBarIconMarginProperty, (object) value);
  }

  internal ChartZoomPanBehavior Source
  {
    get => this.source;
    set
    {
      this.source = value;
      if (this.source == null)
        return;
      this.BindingToolbarItems(this.source);
    }
  }

  internal void SetPressedState()
  {
    VisualStateManager.GoToState((FrameworkElement) this, "Pressed", true);
  }

  internal void Dispose()
  {
    this.source = (ChartZoomPanBehavior) null;
    if (this.PropertyChanged != null)
    {
      foreach (Delegate invocation in this.PropertyChanged.GetInvocationList())
        this.PropertyChanged -= invocation as PropertyChangedEventHandler;
      this.PropertyChanged = (PropertyChangedEventHandler) null;
    }
    this.Content = (object) null;
    this.DataContext = (object) null;
  }

  internal void OnPropertyChanged(string propertyName)
  {
    if (this.PropertyChanged == null)
      return;
    this.PropertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
  }

  protected override AutomationPeer OnCreateAutomationPeer()
  {
    return (AutomationPeer) new ChartZoomingToolBarAutomationPeer(this);
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    VisualStateManager.GoToState((FrameworkElement) this, "PointerOver", true);
  }

  protected override void OnMouseLeave(MouseEventArgs e)
  {
    VisualStateManager.GoToState((FrameworkElement) this, "Normal", true);
  }

  protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
  {
    VisualStateManager.GoToState((FrameworkElement) this, "PointerOver", true);
  }

  private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ZoomingToolBarItem).ScheduleUpdate();
  }

  private void BindingToolbarItems(ChartZoomPanBehavior dataSource)
  {
    BindingOperations.SetBinding((DependencyObject) this, ZoomingToolBarItem.ToolBarIconHeightProperty, (BindingBase) new Binding()
    {
      Source = (object) dataSource,
      Path = new PropertyPath("ToolBarItemHeight", new object[0])
    });
    BindingOperations.SetBinding((DependencyObject) this, ZoomingToolBarItem.ToolBarIconWidthProperty, (BindingBase) new Binding()
    {
      Source = (object) dataSource,
      Path = new PropertyPath("ToolBarItemWidth", new object[0])
    });
    BindingOperations.SetBinding((DependencyObject) this, ZoomingToolBarItem.ToolBarIconMarginProperty, (BindingBase) new Binding()
    {
      Source = (object) dataSource,
      Path = new PropertyPath("ToolBarItemMargin", new object[0])
    });
  }

  private void ScheduleUpdate()
  {
    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Delegate) new Action(this.UpdateToolBarPosition));
  }

  private void UpdateToolBarPosition()
  {
    this.source.ChartZoomingToolBar.UpdateLayout();
    this.source.OnLayoutUpdated();
  }
}
