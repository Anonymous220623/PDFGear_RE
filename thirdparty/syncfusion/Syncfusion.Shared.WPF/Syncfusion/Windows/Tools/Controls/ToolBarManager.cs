// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.ToolBarManager
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (ToolBarManager), XamlResource = "/Syncfusion.Shared.Wpf;component/Controls/ToolBarAdv/Themes/Default/DefaultToolBarManagerStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Office2007Blue/Office2007BlueToolBarManagerStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Metro/MetroToolBarManagerStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Transparent/TransparentToolBarManagerStyle.xaml")]
[ToolboxItem(false)]
[DesignTimeVisible(false)]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Office2007Silver/Office2007SilverToolBarManagerStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Office2007Black/Office2007BlackToolBarManagerStyle.xaml")]
[System.Windows.Markup.ContentProperty("Content")]
[StyleTypedProperty(Property = "FloatingToolBarStyle", StyleTargetType = typeof (FloatingToolBar))]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Blend/BlendToolBarManagerStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/VS2010/VS2010ToolBarManagerStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Office2010Blue/Office2010BlueToolBarManagerStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Office2010Black/Office2010BlackToolBarManagerStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (ToolBarManager), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ToolBarAdv/Themes/Office2010Silver/Office2010SilverToolBarManagerStyle.xaml")]
public class ToolBarManager : Control
{
  internal ContentControl content;
  private ToolBarManagerPanel panel;
  private bool needToInvalidate = true;
  private Size currentSize;
  internal List<FloatingToolBar> FloatingToolBars;
  public static readonly DependencyProperty LeftToolBarTrayProperty = DependencyProperty.Register(nameof (LeftToolBarTray), typeof (ToolBarTrayAdv), typeof (ToolBarManager), new PropertyMetadata(new PropertyChangedCallback(ToolBarManager.OnLeftTrayChanged)));
  public static readonly DependencyProperty RightToolBarTrayProperty = DependencyProperty.Register(nameof (RightToolBarTray), typeof (ToolBarTrayAdv), typeof (ToolBarManager), new PropertyMetadata(new PropertyChangedCallback(ToolBarManager.OnRightTrayChanged)));
  public static readonly DependencyProperty TopToolBarTrayProperty = DependencyProperty.Register(nameof (TopToolBarTray), typeof (ToolBarTrayAdv), typeof (ToolBarManager), new PropertyMetadata(new PropertyChangedCallback(ToolBarManager.OnTopTrayChanged)));
  public static readonly DependencyProperty BottomToolBarTrayProperty = DependencyProperty.Register(nameof (BottomToolBarTray), typeof (ToolBarTrayAdv), typeof (ToolBarManager), new PropertyMetadata(new PropertyChangedCallback(ToolBarManager.OnBottomTrayChanged)));
  public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof (Content), typeof (UIElement), typeof (ToolBarManager), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty CanDockAtTopProperty = DependencyProperty.Register(nameof (CanDockAtTop), typeof (bool), typeof (ToolBarManager), new PropertyMetadata((object) true));
  public static readonly DependencyProperty CanDockAtBottomProperty = DependencyProperty.Register(nameof (CanDockAtBottom), typeof (bool), typeof (ToolBarManager), new PropertyMetadata((object) true));
  public static readonly DependencyProperty CanDockAtLeftProperty = DependencyProperty.Register(nameof (CanDockAtLeft), typeof (bool), typeof (ToolBarManager), new PropertyMetadata((object) true));
  public static readonly DependencyProperty CanDockAtRightProperty = DependencyProperty.Register(nameof (CanDockAtRight), typeof (bool), typeof (ToolBarManager), new PropertyMetadata((object) true));
  internal static readonly DependencyProperty DockAreaProperty = DependencyProperty.RegisterAttached("DockArea", typeof (DockArea), typeof (ToolBarManager), new PropertyMetadata((object) DockArea.Top));
  public static readonly DependencyProperty ToolBarStateProperty = DependencyProperty.RegisterAttached("ToolBarState", typeof (ToolBarState), typeof (ToolBarManager), new PropertyMetadata((object) ToolBarState.Docking, new PropertyChangedCallback(ToolBarManager.OnToolBarStateChanged)));
  public static readonly DependencyProperty FloatingToolBarStyleProperty = DependencyProperty.Register(nameof (FloatingToolBarStyle), typeof (Style), typeof (ToolBarManager), new PropertyMetadata((object) null, new PropertyChangedCallback(ToolBarManager.OnFloatingToolBarStyleChanged)));
  private Window wn;

  public ToolBarTrayAdv LeftToolBarTray
  {
    get => (ToolBarTrayAdv) this.GetValue(ToolBarManager.LeftToolBarTrayProperty);
    set => this.SetValue(ToolBarManager.LeftToolBarTrayProperty, (object) value);
  }

  public ToolBarTrayAdv RightToolBarTray
  {
    get => (ToolBarTrayAdv) this.GetValue(ToolBarManager.RightToolBarTrayProperty);
    set => this.SetValue(ToolBarManager.RightToolBarTrayProperty, (object) value);
  }

  public ToolBarTrayAdv TopToolBarTray
  {
    get => (ToolBarTrayAdv) this.GetValue(ToolBarManager.TopToolBarTrayProperty);
    set => this.SetValue(ToolBarManager.TopToolBarTrayProperty, (object) value);
  }

  public ToolBarTrayAdv BottomToolBarTray
  {
    get => (ToolBarTrayAdv) this.GetValue(ToolBarManager.BottomToolBarTrayProperty);
    set => this.SetValue(ToolBarManager.BottomToolBarTrayProperty, (object) value);
  }

  public UIElement Content
  {
    get => (UIElement) this.GetValue(ToolBarManager.ContentProperty);
    set => this.SetValue(ToolBarManager.ContentProperty, (object) value);
  }

  public bool CanDockAtTop
  {
    get => (bool) this.GetValue(ToolBarManager.CanDockAtTopProperty);
    set => this.SetValue(ToolBarManager.CanDockAtTopProperty, (object) value);
  }

  public bool CanDockAtBottom
  {
    get => (bool) this.GetValue(ToolBarManager.CanDockAtBottomProperty);
    set => this.SetValue(ToolBarManager.CanDockAtBottomProperty, (object) value);
  }

  public bool CanDockAtLeft
  {
    get => (bool) this.GetValue(ToolBarManager.CanDockAtLeftProperty);
    set => this.SetValue(ToolBarManager.CanDockAtLeftProperty, (object) value);
  }

  public bool CanDockAtRight
  {
    get => (bool) this.GetValue(ToolBarManager.CanDockAtRightProperty);
    set => this.SetValue(ToolBarManager.CanDockAtRightProperty, (object) value);
  }

  internal static DockArea GetDockArea(DependencyObject obj)
  {
    return (DockArea) obj.GetValue(ToolBarManager.DockAreaProperty);
  }

  internal static void SetDockArea(DependencyObject obj, DockArea value)
  {
    obj.SetValue(ToolBarManager.DockAreaProperty, (object) value);
  }

  public static ToolBarState GetToolBarState(ToolBarAdv obj)
  {
    return (ToolBarState) obj.GetValue(ToolBarManager.ToolBarStateProperty);
  }

  public static void SetToolBarState(ToolBarAdv obj, ToolBarState value)
  {
    obj.SetValue(ToolBarManager.ToolBarStateProperty, (object) value);
  }

  public Style FloatingToolBarStyle
  {
    get => (Style) this.GetValue(ToolBarManager.FloatingToolBarStyleProperty);
    set => this.SetValue(ToolBarManager.FloatingToolBarStyleProperty, (object) value);
  }

  public ToolBarManager()
  {
    this.DefaultStyleKey = (object) typeof (ToolBarManager);
    this.FloatingToolBars = new List<FloatingToolBar>();
    this.Loaded += new RoutedEventHandler(this.ToolBarManager_Loaded);
    this.Unloaded += new RoutedEventHandler(this.ToolBarManager_Unloaded);
  }

  private void ToolBarManager_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.wn == null)
      return;
    this.wn.StateChanged += new EventHandler(this.wn_StateChanged);
  }

  private void ToolBarManager_Unloaded(object sender, RoutedEventArgs e)
  {
    if (this.wn != null)
      this.wn.StateChanged -= new EventHandler(this.wn_StateChanged);
    this.Loaded -= new RoutedEventHandler(this.ToolBarManager_Loaded);
    this.Unloaded -= new RoutedEventHandler(this.ToolBarManager_Unloaded);
  }

  private void wn_StateChanged(object sender, EventArgs e)
  {
    Window window = sender as Window;
    if (window.WindowState == WindowState.Minimized)
    {
      foreach (UIElement floatingToolBar in this.FloatingToolBars)
        floatingToolBar.Visibility = Visibility.Collapsed;
    }
    else
    {
      if (window.WindowState != WindowState.Maximized && window.WindowState != WindowState.Normal)
        return;
      foreach (UIElement floatingToolBar in this.FloatingToolBars)
        floatingToolBar.Visibility = Visibility.Visible;
    }
  }

  private static void OnTopTrayChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    if (args.NewValue != null)
      (args.NewValue as ToolBarTrayAdv).ToolBarManager = obj as ToolBarManager;
    (obj as ToolBarManager).OnTrayChanged(args);
  }

  private static void OnToolBarStateChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    if (obj == null)
      return;
    (obj as ToolBarAdv).OnToolBarStateChanged((ToolBarState) args.OldValue, (ToolBarState) args.NewValue);
  }

  private static void OnLeftTrayChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    if (args.NewValue != null)
    {
      (args.NewValue as ToolBarTrayAdv).ToolBarManager = obj as ToolBarManager;
      (args.NewValue as ToolBarTrayAdv).Orientation = System.Windows.Controls.Orientation.Vertical;
    }
    (obj as ToolBarManager).OnTrayChanged(args);
  }

  private static void OnRightTrayChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    if (args.NewValue != null)
    {
      (args.NewValue as ToolBarTrayAdv).ToolBarManager = obj as ToolBarManager;
      (args.NewValue as ToolBarTrayAdv).Orientation = System.Windows.Controls.Orientation.Vertical;
    }
    (obj as ToolBarManager).OnTrayChanged(args);
  }

  private static void OnBottomTrayChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    if (args.NewValue != null)
      (args.NewValue as ToolBarTrayAdv).ToolBarManager = obj as ToolBarManager;
    (obj as ToolBarManager).OnTrayChanged(args);
  }

  public override void OnApplyTemplate()
  {
    this.content = this.GetTemplateChild("PART_Content") as ContentControl;
    this.panel = this.GetTemplateChild("PART_Panel") as ToolBarManagerPanel;
    this.InsertTrays();
    base.OnApplyTemplate();
    this.wn = VisualUtils.FindRootVisual((Visual) this) as Window;
  }

  private void OnTrayChanged(DependencyPropertyChangedEventArgs args)
  {
    this.InsertTray(args.OldValue as ToolBarTrayAdv, args.NewValue as ToolBarTrayAdv);
  }

  private void InsertTray(ToolBarTrayAdv oldTray, ToolBarTrayAdv newTray)
  {
    if (this.panel == null)
      return;
    if (oldTray != null && this.panel.Children.Contains((UIElement) oldTray))
      this.panel.Children.Remove((UIElement) oldTray);
    if (newTray == null || this.panel.Children.Contains((UIElement) newTray))
      return;
    this.panel.Children.Add((UIElement) newTray);
  }

  private static void OnFloatingToolBarStyleChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as ToolBarManager).OnFloatingToolBarStyleChanged(args);
  }

  private void OnFloatingToolBarStyleChanged(DependencyPropertyChangedEventArgs args)
  {
    foreach (FrameworkElement floatingToolBar in this.FloatingToolBars)
      floatingToolBar.Style = this.FloatingToolBarStyle;
  }

  private void InsertTrays()
  {
    if (this.panel == null)
      return;
    if (this.TopToolBarTray != null && !this.panel.Children.Contains((UIElement) this.TopToolBarTray))
    {
      this.RemoveFromParent(this.TopToolBarTray);
      this.panel.Children.Add((UIElement) this.TopToolBarTray);
    }
    if (this.BottomToolBarTray != null && !this.panel.Children.Contains((UIElement) this.BottomToolBarTray))
    {
      this.RemoveFromParent(this.BottomToolBarTray);
      this.panel.Children.Add((UIElement) this.BottomToolBarTray);
    }
    if (this.LeftToolBarTray != null && !this.panel.Children.Contains((UIElement) this.LeftToolBarTray))
    {
      this.RemoveFromParent(this.LeftToolBarTray);
      this.panel.Children.Add((UIElement) this.LeftToolBarTray);
    }
    if (this.RightToolBarTray == null || this.panel.Children.Contains((UIElement) this.RightToolBarTray))
      return;
    this.RemoveFromParent(this.RightToolBarTray);
    this.panel.Children.Add((UIElement) this.RightToolBarTray);
  }

  private void RemoveFromParent(ToolBarTrayAdv tray)
  {
    if (tray == null || !(tray.Parent is Panel) || !(tray.Parent as Panel).Children.Contains((UIElement) tray))
      return;
    (tray.Parent as Panel).Children.Remove((UIElement) tray);
  }

  internal bool CanDock(DockArea area)
  {
    switch (area)
    {
      case DockArea.Top:
        return this.CanDockAtTop;
      case DockArea.Left:
        return this.CanDockAtLeft;
      case DockArea.Right:
        return this.CanDockAtRight;
      case DockArea.Bottom:
        return this.CanDockAtBottom;
      default:
        return false;
    }
  }

  internal void Remove(ToolBarTrayAdv tray)
  {
    this.RemoveFromParent(tray);
    if (this.TopToolBarTray == tray)
      this.TopToolBarTray = (ToolBarTrayAdv) null;
    else if (this.LeftToolBarTray == tray)
      this.LeftToolBarTray = (ToolBarTrayAdv) null;
    else if (this.RightToolBarTray == tray)
    {
      this.RightToolBarTray = (ToolBarTrayAdv) null;
    }
    else
    {
      if (this.BottomToolBarTray != tray)
        return;
      this.BottomToolBarTray = (ToolBarTrayAdv) null;
    }
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    this.panel.Arrange(finalSize);
    base.ArrangeOverride(finalSize);
    return finalSize;
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    Size size = availableSize;
    if (double.IsInfinity(availableSize.Width))
      size.Width = double.MaxValue;
    if (double.IsInfinity(availableSize.Height))
      size.Height = double.MaxValue;
    availableSize = size;
    this.currentSize = availableSize;
    this.MeasureCall(availableSize);
    base.MeasureOverride(availableSize);
    return availableSize;
  }

  private Size MeasureCall(Size availableSize)
  {
    double height1 = availableSize.Height;
    double width1 = availableSize.Width;
    if (this.TopToolBarTray != null)
    {
      this.TopToolBarTray.MeasureCall(this.needToInvalidate, availableSize);
      foreach (ToolBarBand band in this.TopToolBarTray.Bands)
        height1 -= band.Size;
    }
    if (this.BottomToolBarTray != null)
    {
      this.BottomToolBarTray.MeasureCall(this.needToInvalidate, availableSize);
      foreach (ToolBarBand band in this.BottomToolBarTray.Bands)
        height1 -= band.Size;
    }
    double height2 = Math.Max(0.0, height1);
    Size availableSize1 = new Size(availableSize.Width, height2);
    if (this.LeftToolBarTray != null)
    {
      this.LeftToolBarTray.MeasureCall(this.needToInvalidate, availableSize1);
      foreach (ToolBarBand band in this.LeftToolBarTray.Bands)
        width1 -= band.Size;
    }
    if (this.RightToolBarTray != null)
    {
      this.RightToolBarTray.MeasureCall(this.needToInvalidate, availableSize1);
      foreach (ToolBarBand band in this.RightToolBarTray.Bands)
        width1 -= band.Size;
    }
    double width2 = Math.Max(0.0, width1);
    if (this.content != null)
      this.content.Measure(new Size(width2, height2));
    return availableSize;
  }

  internal DockArea FindDockArea(Point point)
  {
    Rect rect1 = new Rect(0.0, 0.0, this.ActualWidth, this.GetSize(this.TopToolBarTray, System.Windows.Controls.Orientation.Horizontal));
    if (rect1.Contains(point))
      return DockArea.Top;
    double size1 = this.GetSize(this.BottomToolBarTray, System.Windows.Controls.Orientation.Horizontal);
    Rect rect2 = new Rect(0.0, Math.Max(0.0, this.ActualHeight - size1), this.ActualWidth, size1);
    if (rect2.Contains(point))
      return DockArea.Bottom;
    double num = this.ActualHeight - (rect1.Height + rect2.Height);
    double height = num < 0.0 ? 20.0 : num;
    double size2 = this.GetSize(this.LeftToolBarTray, System.Windows.Controls.Orientation.Vertical);
    if (new Rect(0.0, rect1.Height, size2, height).Contains(point))
      return DockArea.Left;
    double size3 = this.GetSize(this.RightToolBarTray, System.Windows.Controls.Orientation.Vertical);
    return new Rect(Math.Max(0.0, this.ActualWidth - size3), rect1.Height, size3, height).Contains(point) ? DockArea.Right : DockArea.None;
  }

  private double GetSize(ToolBarTrayAdv tray, System.Windows.Controls.Orientation orientation)
  {
    double size = 20.0;
    if (tray == null)
      return size;
    double orientedHeightValue = OrientedValue.GetOrientedHeightValue(new Size(tray.ActualWidth, tray.ActualHeight), orientation);
    return orientedHeightValue == 0.0 ? 20.0 : orientedHeightValue;
  }

  internal ToolBarTrayAdv GetToolBarTray(DockArea area)
  {
    switch (area)
    {
      case DockArea.Top:
        return this.TopToolBarTray;
      case DockArea.Left:
        return this.LeftToolBarTray;
      case DockArea.Right:
        return this.RightToolBarTray;
      default:
        return this.BottomToolBarTray;
    }
  }

  internal DockArea GetDockArea(ToolBarTrayAdv tray)
  {
    if (tray == null)
      return DockArea.None;
    if (this.LeftToolBarTray == tray)
      return DockArea.Left;
    if (this.TopToolBarTray == tray)
      return DockArea.Top;
    if (this.RightToolBarTray == tray)
      return DockArea.Right;
    return this.BottomToolBarTray == tray ? DockArea.Bottom : DockArea.None;
  }

  internal void DockTray(ToolBarTrayAdv tray, DockArea area)
  {
    if (tray == null || area == DockArea.None)
      return;
    tray.ToolBarManager = this;
    switch (area)
    {
      case DockArea.Top:
        tray.Orientation = System.Windows.Controls.Orientation.Horizontal;
        this.TopToolBarTray = tray;
        break;
      case DockArea.Left:
        tray.Orientation = System.Windows.Controls.Orientation.Vertical;
        this.LeftToolBarTray = tray;
        break;
      case DockArea.Right:
        tray.Orientation = System.Windows.Controls.Orientation.Vertical;
        this.RightToolBarTray = tray;
        break;
      default:
        tray.Orientation = System.Windows.Controls.Orientation.Horizontal;
        this.BottomToolBarTray = tray;
        break;
    }
    tray.UpdateVisualState();
    this.needToInvalidate = false;
    this.Invalidate();
  }

  internal void DockToolBar(ToolBarAdv toolBar, DockArea area)
  {
    if (toolBar == null || area == DockArea.None)
      return;
    ToolBarTrayAdv tray = this.GetToolBarTray(area) ?? new ToolBarTrayAdv();
    if (!tray.ToolBars.Contains(toolBar))
      tray.ToolBars.Add(toolBar);
    toolBar.Tray = tray;
    this.needToInvalidate = false;
    if (this.GetDockArea(toolBar.Tray) == DockArea.None)
    {
      this.DockTray(tray, area);
    }
    else
    {
      toolBar.Orientation = tray.Orientation;
      this.Invalidate();
    }
  }

  public void InvalidateLayout()
  {
    this.InvalidateMeasure();
    this.InvalidateArrange();
    this.UpdateLayout();
  }

  internal void Invalidate()
  {
    this.MeasureCall(this.currentSize);
    if (this.panel == null)
      return;
    this.panel.InvalidateArrange();
  }
}
