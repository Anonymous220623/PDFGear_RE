// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.FloatingToolBar
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Shared;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

[CLSCompliant(true)]
public class FloatingToolBar : ContentControl
{
  internal WrapPanel panel;
  private double resizeOffset = double.NaN;
  private Thumb topThumb;
  private Thumb bottomThumb;
  private Thumb leftThumb;
  private Popup PART_DropPopUp;
  private Thumb rightThumb;
  private Thumb titleThumb;
  private FrameworkElement client;
  private ToolBarAdv toolBar;
  private bool forceDrag;
  private double maxLineSize;
  private double minLineSize;
  private bool isDragging;
  private Button closbutton;
  public static readonly DependencyProperty ControlsResourceDictionaryProperty = DependencyProperty.Register(nameof (ControlsResourceDictionary), typeof (ResourceDictionary), typeof (FloatingToolBar), new PropertyMetadata((object) new ResourceDictionary()
  {
    Source = new Uri("/Syncfusion.Shared.Wpf;component/Controls/ToolBarAdv/Themes/ToolBarResources.xaml", UriKind.RelativeOrAbsolute)
  }, new PropertyChangedCallback(FloatingToolBar.OnControlsResourceDictionaryPropertyChanged)));
  public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof (Title), typeof (string), typeof (FloatingToolBar), (PropertyMetadata) new UIPropertyMetadata((object) string.Empty));
  internal Popup popup;

  internal ToolBarManager Manager { get; set; }

  public ToolBarAdv ToolBar
  {
    get => this.toolBar;
    set => this.toolBar = value;
  }

  public ObservableCollection<ToolBarIteminfo> ToolBarItemInfoCollection
  {
    get => this.toolBar.ToolBarItemInfoCollection;
    set => this.toolBar.ToolBarItemInfoCollection = value;
  }

  public ResourceDictionary ControlsResourceDictionary
  {
    get => (ResourceDictionary) this.GetValue(FloatingToolBar.ControlsResourceDictionaryProperty);
    set => this.SetValue(FloatingToolBar.ControlsResourceDictionaryProperty, (object) value);
  }

  internal bool ForceDrag
  {
    get => this.forceDrag;
    set
    {
      this.forceDrag = value;
      if (this.titleThumb == null)
        return;
      if (this.forceDrag)
        this.titleThumb.CaptureMouse();
      else
        this.titleThumb.ReleaseMouseCapture();
    }
  }

  public string Title
  {
    get => (string) this.GetValue(FloatingToolBar.TitleProperty);
    set => this.SetValue(FloatingToolBar.TitleProperty, (object) value);
  }

  public FloatingToolBar()
  {
    this.DefaultStyleKey = (object) typeof (FloatingToolBar);
    this.panel = new WrapPanel();
    this.Content = (object) this.panel;
    this.Unloaded += new RoutedEventHandler(this.FloatingToolBar_Unloaded);
  }

  public override void OnApplyTemplate()
  {
    this.GetTemplateChild();
    base.OnApplyTemplate();
    if (!((this.ToolBar.Foreground as SolidColorBrush).Color != (Control.ForegroundProperty.DefaultMetadata.DefaultValue as SolidColorBrush).Color))
      return;
    this.Foreground = this.ToolBar.Foreground;
  }

  private void GetTemplateChild()
  {
    if (this.closbutton != null)
      this.closbutton.Click -= new RoutedEventHandler(this.closbutton_Click);
    if (this.topThumb != null)
      this.topThumb.DragDelta -= new DragDeltaEventHandler(this.ResizeDragDelta);
    if (this.bottomThumb != null)
      this.bottomThumb.DragDelta -= new DragDeltaEventHandler(this.ResizeDragDelta);
    if (this.leftThumb != null)
      this.leftThumb.DragDelta -= new DragDeltaEventHandler(this.ResizeDragDelta);
    if (this.rightThumb != null)
      this.rightThumb.DragDelta -= new DragDeltaEventHandler(this.ResizeDragDelta);
    if (this.titleThumb != null)
    {
      this.titleThumb.DragDelta -= new DragDeltaEventHandler(this.titleThumb_DragDelta);
      this.titleThumb.DragStarted -= new DragStartedEventHandler(this.titleThumb_DragStarted);
      this.titleThumb.MouseMove -= new MouseEventHandler(this.titleThumb_MouseMove);
      this.titleThumb.MouseLeftButtonUp -= new MouseButtonEventHandler(this.titleThumb_MouseLeftButtonUp);
      this.titleThumb.DragCompleted -= new DragCompletedEventHandler(this.titleThumb_DragCompleted);
    }
    this.PART_DropPopUp = this.GetTemplateChild("PART_DropPopUp") as Popup;
    this.topThumb = this.GetTemplateChild("PART_TopThumb") as Thumb;
    this.bottomThumb = this.GetTemplateChild("PART_BottomThumb") as Thumb;
    this.leftThumb = this.GetTemplateChild("PART_LeftThumb") as Thumb;
    this.rightThumb = this.GetTemplateChild("PART_RightThumb") as Thumb;
    this.titleThumb = this.GetTemplateChild("PART_TitleFloatingThumb") as Thumb;
    this.client = this.GetTemplateChild("PART_Client") as FrameworkElement;
    this.closbutton = this.GetTemplateChild("PART_CloseButton") as Button;
    if (this.closbutton != null)
      this.closbutton.Click += new RoutedEventHandler(this.closbutton_Click);
    if (this.topThumb != null)
      this.topThumb.DragDelta += new DragDeltaEventHandler(this.ResizeDragDelta);
    if (this.bottomThumb != null)
      this.bottomThumb.DragDelta += new DragDeltaEventHandler(this.ResizeDragDelta);
    if (this.leftThumb != null)
      this.leftThumb.DragDelta += new DragDeltaEventHandler(this.ResizeDragDelta);
    if (this.rightThumb != null)
      this.rightThumb.DragDelta += new DragDeltaEventHandler(this.ResizeDragDelta);
    if (this.titleThumb != null)
    {
      this.titleThumb.DragDelta += new DragDeltaEventHandler(this.titleThumb_DragDelta);
      this.titleThumb.DragStarted += new DragStartedEventHandler(this.titleThumb_DragStarted);
      this.titleThumb.MouseMove += new MouseEventHandler(this.titleThumb_MouseMove);
      this.titleThumb.MouseLeftButtonUp += new MouseButtonEventHandler(this.titleThumb_MouseLeftButtonUp);
      this.titleThumb.DragCompleted += new DragCompletedEventHandler(this.titleThumb_DragCompleted);
    }
    if (this.titleThumb == null)
      return;
    if (this.ForceDrag)
      this.titleThumb.CaptureMouse();
    else
      this.titleThumb.ReleaseMouseCapture();
  }

  private void titleThumb_DragCompleted(object sender, DragCompletedEventArgs e)
  {
    this.isDragging = false;
  }

  private void closbutton_Click(object sender, RoutedEventArgs e)
  {
    if (this.popup != null)
      this.popup.IsOpen = false;
    this.ToolBar.ChangeStateInternally(ToolBarState.Hidden);
  }

  private void OnMouseDown(object sender, MouseButtonEventArgs args)
  {
    if (this.PART_DropPopUp == null)
      return;
    this.PART_DropPopUp.IsOpen = false;
  }

  private static void OnControlsResourceDictionaryPropertyChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    (obj as FloatingToolBar).OnControlsResourceDictionaryPropertyChanged(args);
  }

  private void OnControlsResourceDictionaryPropertyChanged(DependencyPropertyChangedEventArgs args)
  {
    if (this.ToolBar != null && ToolBarManager.GetToolBarState(this.ToolBar) == ToolBarState.Floating)
      this.InsertItems(this.ToolBar);
    this.ApplyStyleForControls();
  }

  internal void ApplyStyleForControls()
  {
    ResourceDictionary resourceDictionary = this.ControlsResourceDictionary;
    if (this.ToolBar == null || this.ToolBar.generatedConatiner == null)
      return;
    foreach (FrameworkElement frameworkElement in this.ToolBar.generatedConatiner.Values)
    {
      if (frameworkElement.GetType().Name == "ButtonAdv" || frameworkElement.GetType().Name == "DropDownButtonAdv" || frameworkElement.GetType().Name == "SplitButtonAdv")
      {
        if (resourceDictionary.Contains((object) $"SyncfusionToolBar{frameworkElement.GetType().Name}Style"))
          frameworkElement.Style = resourceDictionary[(object) $"SyncfusionToolBar{frameworkElement.GetType().Name}Style"] as Style;
      }
      else if (resourceDictionary.Contains((object) $"ToolBar{frameworkElement.GetType().Name}Style"))
        frameworkElement.Style = resourceDictionary[(object) $"ToolBar{frameworkElement.GetType().Name}Style"] as Style;
    }
  }

  private void titleThumb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    this.ForceDrag = false;
  }

  private void titleThumb_MouseMove(object sender, MouseEventArgs e)
  {
    Point position = e.GetPosition((IInputElement) this.Manager);
    if (this.ForceDrag && this.popup != null)
    {
      WindowInterop.POINT lpPoint = new WindowInterop.POINT();
      WindowInterop.GetCursorPos(out lpPoint);
      this.popup.HorizontalOffset = (double) (lpPoint.x - 10);
      this.popup.VerticalOffset = (double) (lpPoint.y - 10);
      this.ToolBar.FloatingBarLocation = new Point(this.popup.HorizontalOffset, this.popup.VerticalOffset);
    }
    if (!this.ForceDrag && !this.isDragging)
      return;
    this.FindDockArea(position);
  }

  private void titleThumb_DragStarted(object sender, DragStartedEventArgs e)
  {
    this.isDragging = true;
    this.client.Focus();
  }

  private void titleThumb_DragDelta(object sender, DragDeltaEventArgs e)
  {
    this.MoveWindow(e.HorizontalChange, e.VerticalChange);
  }

  internal void MoveWindow(double horizontalChange, double verticalChange)
  {
    if (this.popup == null)
      return;
    this.popup.HorizontalOffset += horizontalChange;
    this.popup.VerticalOffset += verticalChange;
    this.ToolBar.FloatingBarLocation = new Point(this.popup.HorizontalOffset, this.popup.VerticalOffset);
  }

  private void FindDockArea(Point point)
  {
    DockArea dockArea = this.Manager.FindDockArea(point);
    ToolBarTrayAdv toolBarTray = this.Manager.GetToolBarTray(dockArea);
    if (toolBarTray != null && (toolBarTray.IsLocked || !this.Manager.CanDock(dockArea)) || dockArea == DockArea.None)
      return;
    this.Visibility = Visibility.Collapsed;
    if (this.popup != null)
      this.popup.IsOpen = false;
    this.panel.Children.Clear();
    if (this.Manager.FloatingToolBars.Contains(this.ToolBar.floatingToolBar))
      this.Manager.FloatingToolBars.Remove(this.ToolBar.floatingToolBar);
    this.ToolBar.floatingToolBar = (FloatingToolBar) null;
    this.Manager.DockToolBar(this.ToolBar, dockArea);
    this.ForceDrag = false;
    this.ToolBar.IsDragging = true;
    this.ToolBar.ChangeStateInternally(ToolBarState.Docking);
  }

  private void ResizeDragDelta(object sender, DragDeltaEventArgs e)
  {
    Thumb thumb = sender as Thumb;
    Size desiredSize = this.panel.DesiredSize;
    switch (thumb.Name)
    {
      case "PART_TopThumb":
        this.Resize(e.VerticalChange);
        if (this.popup == null)
          break;
        this.popup.VerticalOffset += desiredSize.Height - this.panel.DesiredSize.Height;
        break;
      case "PART_BottomThumb":
        this.Resize(-e.VerticalChange);
        break;
      case "PART_LeftThumb":
        this.Resize(-e.HorizontalChange);
        if (this.popup == null)
          break;
        this.popup.HorizontalOffset += desiredSize.Width - this.panel.DesiredSize.Width;
        break;
      case "PART_RightThumb":
        this.Resize(e.HorizontalChange);
        break;
    }
  }

  private void Resize(double change)
  {
    Size size = new Size(this.panel.ActualWidth, this.panel.ActualHeight);
    if (size.Width + change <= this.minLineSize)
      return;
    double num = change;
    this.panel.Width = double.NaN;
    if (!double.IsNaN(this.resizeOffset))
      num = this.resizeOffset;
    this.panel.Measure(new Size(this.panel.ActualWidth + num, double.PositiveInfinity));
    if (this.panel.DesiredSize.Width == size.Width && this.panel.DesiredSize.Width < this.maxLineSize)
    {
      if (double.IsNaN(this.resizeOffset))
        this.resizeOffset = 0.0;
      this.resizeOffset += change;
    }
    else
      this.resizeOffset = double.NaN;
    this.panel.Width = this.panel.DesiredSize.Width;
    this.InvalidateMeasure();
  }

  protected override Size ArrangeOverride(Size finalSize) => base.ArrangeOverride(finalSize);

  protected override Size MeasureOverride(Size availableSize)
  {
    Size size = base.MeasureOverride(availableSize);
    this.MeasureLineSizes();
    return size;
  }

  private double GetMaxLineSize()
  {
    double maxLineSize = 0.0;
    foreach (UIElement child in this.panel.Children)
      maxLineSize += child.DesiredSize.Width;
    return maxLineSize;
  }

  private double GetMinLineSize()
  {
    double val1 = 0.0;
    foreach (UIElement child in this.panel.Children)
      val1 = Math.Max(val1, child.DesiredSize.Width);
    return val1;
  }

  private void MeasureLineSizes()
  {
    this.maxLineSize = this.GetMaxLineSize();
    this.minLineSize = this.GetMinLineSize();
  }

  internal void InsertItems(ToolBarAdv toolBar)
  {
    this.panel.Children.Clear();
    foreach (object obj in (IEnumerable) toolBar.Items)
      toolBar.InsertItemToPanel((Panel) this.panel, obj);
  }

  internal void ReleaseResources()
  {
    if (this.topThumb != null)
      this.topThumb.DragDelta -= new DragDeltaEventHandler(this.ResizeDragDelta);
    if (this.bottomThumb != null)
      this.bottomThumb.DragDelta -= new DragDeltaEventHandler(this.ResizeDragDelta);
    if (this.leftThumb != null)
      this.leftThumb.DragDelta -= new DragDeltaEventHandler(this.ResizeDragDelta);
    if (this.rightThumb != null)
      this.rightThumb.DragDelta -= new DragDeltaEventHandler(this.ResizeDragDelta);
    if (this.titleThumb != null)
    {
      this.titleThumb.DragDelta -= new DragDeltaEventHandler(this.titleThumb_DragDelta);
      this.titleThumb.DragStarted -= new DragStartedEventHandler(this.titleThumb_DragStarted);
      this.titleThumb.MouseMove -= new MouseEventHandler(this.titleThumb_MouseMove);
      this.titleThumb.MouseLeftButtonUp -= new MouseButtonEventHandler(this.titleThumb_MouseLeftButtonUp);
    }
    this.Unloaded -= new RoutedEventHandler(this.FloatingToolBar_Unloaded);
  }

  private void FloatingToolBar_Unloaded(object sender, RoutedEventArgs e)
  {
    this.ReleaseResources();
  }
}
