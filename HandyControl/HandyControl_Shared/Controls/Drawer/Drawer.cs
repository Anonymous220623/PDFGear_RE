// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Drawer
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace HandyControl.Controls;

[System.Windows.Markup.ContentProperty("Content")]
public class Drawer : FrameworkElement
{
  private Storyboard _storyboard;
  private AdornerContainer _container;
  private ContentControl _animationControl;
  private TranslateTransform _translateTransform;
  private double _animationLength;
  private string _animationPropertyName;
  private FrameworkElement _maskElement;
  private AdornerLayer _layer;
  private UIElement _contentElement;
  private Point _contentRenderTransformOrigin;
  public static readonly RoutedEvent OpenedEvent = EventManager.RegisterRoutedEvent("Opened", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (Drawer));
  public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent("Closed", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (Drawer));
  public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof (IsOpen), typeof (bool), typeof (Drawer), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(Drawer.OnIsOpenChanged)));
  public static readonly DependencyProperty MaskCanCloseProperty = DependencyProperty.Register(nameof (MaskCanClose), typeof (bool), typeof (Drawer), new PropertyMetadata(ValueBoxes.TrueBox));
  public static readonly DependencyProperty ShowMaskProperty = DependencyProperty.Register(nameof (ShowMask), typeof (bool), typeof (Drawer), new PropertyMetadata(ValueBoxes.TrueBox));
  public static readonly DependencyProperty DockProperty = DependencyProperty.Register(nameof (Dock), typeof (Dock), typeof (Drawer), new PropertyMetadata((object) Dock.Left));
  public static readonly DependencyProperty ShowModeProperty = DependencyProperty.Register(nameof (ShowMode), typeof (DrawerShowMode), typeof (Drawer), new PropertyMetadata((object) DrawerShowMode.Cover));
  public static readonly DependencyProperty MaskBrushProperty = DependencyProperty.Register(nameof (MaskBrush), typeof (Brush), typeof (Drawer), new PropertyMetadata((object) null));
  public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof (Content), typeof (object), typeof (Drawer), new PropertyMetadata((object) null));

  static Drawer()
  {
    FrameworkElement.DataContextProperty.OverrideMetadata(typeof (Drawer), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(Drawer.DataContextPropertyChanged)));
  }

  public Drawer() => this.Loaded += new RoutedEventHandler(this.Drawer_Loaded);

  private void Drawer_Loaded(object sender, RoutedEventArgs e)
  {
    if (this.IsOpen)
      this.OnIsOpenChanged(true);
    this.Loaded -= new RoutedEventHandler(this.Drawer_Loaded);
  }

  private static void DataContextPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Drawer) d).OnDataContextPropertyChanged(e);
  }

  private void OnDataContextPropertyChanged(DependencyPropertyChangedEventArgs e)
  {
    this.UpdateDataContext((FrameworkElement) this._animationControl, e.OldValue, e.NewValue);
  }

  public event RoutedEventHandler Opened
  {
    add => this.AddHandler(Drawer.OpenedEvent, (Delegate) value);
    remove => this.RemoveHandler(Drawer.OpenedEvent, (Delegate) value);
  }

  public event RoutedEventHandler Closed
  {
    add => this.AddHandler(Drawer.ClosedEvent, (Delegate) value);
    remove => this.RemoveHandler(Drawer.ClosedEvent, (Delegate) value);
  }

  private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((Drawer) d).OnIsOpenChanged((bool) e.NewValue);
  }

  public bool IsOpen
  {
    get => (bool) this.GetValue(Drawer.IsOpenProperty);
    set => this.SetValue(Drawer.IsOpenProperty, ValueBoxes.BooleanBox(value));
  }

  public bool MaskCanClose
  {
    get => (bool) this.GetValue(Drawer.MaskCanCloseProperty);
    set => this.SetValue(Drawer.MaskCanCloseProperty, ValueBoxes.BooleanBox(value));
  }

  public bool ShowMask
  {
    get => (bool) this.GetValue(Drawer.ShowMaskProperty);
    set => this.SetValue(Drawer.ShowMaskProperty, ValueBoxes.BooleanBox(value));
  }

  public Dock Dock
  {
    get => (Dock) this.GetValue(Drawer.DockProperty);
    set => this.SetValue(Drawer.DockProperty, (object) value);
  }

  public DrawerShowMode ShowMode
  {
    get => (DrawerShowMode) this.GetValue(Drawer.ShowModeProperty);
    set => this.SetValue(Drawer.ShowModeProperty, (object) value);
  }

  public Brush MaskBrush
  {
    get => (Brush) this.GetValue(Drawer.MaskBrushProperty);
    set => this.SetValue(Drawer.MaskBrushProperty, (object) value);
  }

  public object Content
  {
    get => this.GetValue(Drawer.ContentProperty);
    set => this.SetValue(Drawer.ContentProperty, value);
  }

  private void CreateContainer()
  {
    this._storyboard = new Storyboard();
    this._storyboard.Completed += new EventHandler(this.Storyboard_Completed);
    this._translateTransform = new TranslateTransform();
    ContentControl contentControl = new ContentControl();
    contentControl.Content = this.Content;
    contentControl.RenderTransform = (Transform) this._translateTransform;
    contentControl.DataContext = (object) this;
    this._animationControl = contentControl;
    SimplePanel simplePanel1 = new SimplePanel();
    simplePanel1.ClipToBounds = true;
    SimplePanel simplePanel2 = simplePanel1;
    if (this.ShowMask)
    {
      Border border = new Border();
      border.Background = this.MaskBrush;
      border.Opacity = 0.0;
      this._maskElement = (FrameworkElement) border;
      this._maskElement.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.MaskElement_PreviewMouseLeftButtonDown);
      simplePanel2.Children.Add((UIElement) this._maskElement);
    }
    this._animationControl.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    Size desiredSize = this._animationControl.DesiredSize;
    switch (this.Dock)
    {
      case Dock.Left:
        this._animationControl.HorizontalAlignment = HorizontalAlignment.Left;
        this._animationControl.VerticalAlignment = VerticalAlignment.Stretch;
        this._translateTransform.X = -desiredSize.Width;
        this._animationLength = -desiredSize.Width;
        this._animationPropertyName = "(UIElement.RenderTransform).(TranslateTransform.X)";
        break;
      case Dock.Top:
        this._animationControl.HorizontalAlignment = HorizontalAlignment.Stretch;
        this._animationControl.VerticalAlignment = VerticalAlignment.Top;
        this._translateTransform.Y = -desiredSize.Height;
        this._animationLength = -desiredSize.Height;
        this._animationPropertyName = "(UIElement.RenderTransform).(TranslateTransform.Y)";
        break;
      case Dock.Right:
        this._animationControl.HorizontalAlignment = HorizontalAlignment.Right;
        this._animationControl.VerticalAlignment = VerticalAlignment.Stretch;
        this._translateTransform.X = desiredSize.Width;
        this._animationLength = desiredSize.Width;
        this._animationPropertyName = "(UIElement.RenderTransform).(TranslateTransform.X)";
        break;
      case Dock.Bottom:
        this._animationControl.HorizontalAlignment = HorizontalAlignment.Stretch;
        this._animationControl.VerticalAlignment = VerticalAlignment.Bottom;
        this._translateTransform.Y = desiredSize.Height;
        this._animationLength = desiredSize.Height;
        this._animationPropertyName = "(UIElement.RenderTransform).(TranslateTransform.Y)";
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    this._animationControl.DataContext = this.DataContext;
    this._animationControl.CommandBindings.Clear();
    this._animationControl.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Close, (ExecutedRoutedEventHandler) ((s, e) => this.SetCurrentValue(Drawer.IsOpenProperty, ValueBoxes.FalseBox))));
    simplePanel2.Children.Add((UIElement) this._animationControl);
    AdornerContainer adornerContainer = new AdornerContainer((UIElement) this._layer);
    adornerContainer.Child = (UIElement) simplePanel2;
    adornerContainer.ClipToBounds = true;
    this._container = adornerContainer;
  }

  private void Storyboard_Completed(object sender, EventArgs e)
  {
    if (!this.IsOpen)
    {
      this._contentElement.SetCurrentValue(UIElement.RenderTransformOriginProperty, (object) this._contentRenderTransformOrigin);
      this._layer.Remove((Adorner) this._container);
      this.RaiseEvent(new RoutedEventArgs(Drawer.ClosedEvent, (object) this));
    }
    else
      this.RaiseEvent(new RoutedEventArgs(Drawer.OpenedEvent, (object) this));
  }

  private void OnIsOpenChanged(bool isOpen)
  {
    if (this.Content == null || DesignerHelper.IsInDesignMode)
      return;
    DrawerContainer parent = VisualHelper.GetParent<DrawerContainer>((DependencyObject) this);
    AdornerDecorator adornerDecorator;
    if (parent != null)
    {
      this._contentElement = parent.Child;
      adornerDecorator = (AdornerDecorator) parent;
    }
    else
    {
      System.Windows.Window activeWindow = WindowHelper.GetActiveWindow();
      if (activeWindow == null)
        return;
      adornerDecorator = VisualHelper.GetChild<AdornerDecorator>((DependencyObject) activeWindow);
      this._contentElement = activeWindow.Content as UIElement;
    }
    if (this._contentElement == null)
      return;
    this._layer = adornerDecorator?.AdornerLayer;
    if (this._layer == null)
      return;
    this._contentRenderTransformOrigin = this._contentElement.RenderTransformOrigin;
    if (this._container == null)
      this.CreateContainer();
    switch (this.ShowMode)
    {
      case DrawerShowMode.Push:
        this.ShowByPush(isOpen);
        break;
      case DrawerShowMode.Press:
        this._contentElement.SetCurrentValue(UIElement.RenderTransformOriginProperty, (object) new Point(0.5, 0.5));
        this.ShowByPress(isOpen);
        break;
    }
    if (isOpen)
    {
      if (this._maskElement != null)
      {
        DoubleAnimation animation = AnimationHelper.CreateAnimation(1.0);
        Storyboard.SetTarget((DependencyObject) animation, (DependencyObject) this._maskElement);
        Storyboard.SetTargetProperty((DependencyObject) animation, new PropertyPath(UIElement.OpacityProperty.Name, Array.Empty<object>()));
        this._storyboard.Children.Add((Timeline) animation);
      }
      DoubleAnimation animation1 = AnimationHelper.CreateAnimation(0.0);
      Storyboard.SetTarget((DependencyObject) animation1, (DependencyObject) this._animationControl);
      Storyboard.SetTargetProperty((DependencyObject) animation1, new PropertyPath(this._animationPropertyName, Array.Empty<object>()));
      this._storyboard.Children.Add((Timeline) animation1);
      this._layer.Remove((Adorner) this._container);
      this._layer.Add((Adorner) this._container);
    }
    else
    {
      if (this._maskElement != null)
      {
        DoubleAnimation animation = AnimationHelper.CreateAnimation(0.0);
        Storyboard.SetTarget((DependencyObject) animation, (DependencyObject) this._maskElement);
        Storyboard.SetTargetProperty((DependencyObject) animation, new PropertyPath(UIElement.OpacityProperty.Name, Array.Empty<object>()));
        this._storyboard.Children.Add((Timeline) animation);
      }
      DoubleAnimation animation2 = AnimationHelper.CreateAnimation(this._animationLength);
      Storyboard.SetTarget((DependencyObject) animation2, (DependencyObject) this._animationControl);
      Storyboard.SetTargetProperty((DependencyObject) animation2, new PropertyPath(this._animationPropertyName, Array.Empty<object>()));
      this._storyboard.Children.Add((Timeline) animation2);
    }
    this._storyboard.Begin();
  }

  private void ShowByPush(bool isOpen)
  {
    string path;
    switch (this.Dock)
    {
      case Dock.Left:
      case Dock.Right:
        path = "(UIElement.RenderTransform).(TranslateTransform.X)";
        this._contentElement.RenderTransform = (Transform) new TranslateTransform()
        {
          X = (isOpen ? 0.0 : -this._animationLength)
        };
        break;
      case Dock.Top:
      case Dock.Bottom:
        path = "(UIElement.RenderTransform).(TranslateTransform.Y)";
        this._contentElement.RenderTransform = (Transform) new TranslateTransform()
        {
          Y = (isOpen ? 0.0 : -this._animationLength)
        };
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    DoubleAnimation element = isOpen ? AnimationHelper.CreateAnimation(-this._animationLength) : AnimationHelper.CreateAnimation(0.0);
    Storyboard.SetTarget((DependencyObject) element, (DependencyObject) this._contentElement);
    Storyboard.SetTargetProperty((DependencyObject) element, new PropertyPath(path, Array.Empty<object>()));
    this._storyboard.Children.Add((Timeline) element);
  }

  private void ShowByPress(bool isOpen)
  {
    UIElement contentElement = this._contentElement;
    ScaleTransform scaleTransform;
    if (!isOpen)
      scaleTransform = new ScaleTransform()
      {
        ScaleX = 0.9,
        ScaleY = 0.9
      };
    else
      scaleTransform = new ScaleTransform();
    contentElement.RenderTransform = (Transform) scaleTransform;
    DoubleAnimation element1 = isOpen ? AnimationHelper.CreateAnimation(0.9) : AnimationHelper.CreateAnimation(1.0);
    Storyboard.SetTarget((DependencyObject) element1, (DependencyObject) this._contentElement);
    Storyboard.SetTargetProperty((DependencyObject) element1, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)", Array.Empty<object>()));
    this._storyboard.Children.Add((Timeline) element1);
    DoubleAnimation element2 = isOpen ? AnimationHelper.CreateAnimation(0.9) : AnimationHelper.CreateAnimation(1.0);
    Storyboard.SetTarget((DependencyObject) element2, (DependencyObject) this._contentElement);
    Storyboard.SetTargetProperty((DependencyObject) element2, new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)", Array.Empty<object>()));
    this._storyboard.Children.Add((Timeline) element2);
  }

  private void MaskElement_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (!this.MaskCanClose)
      return;
    this.SetCurrentValue(Drawer.IsOpenProperty, ValueBoxes.FalseBox);
  }

  private void UpdateDataContext(FrameworkElement target, object oldValue, object newValue)
  {
    if (target == null || BindingOperations.GetBindingExpression((DependencyObject) target, FrameworkElement.DataContextProperty) != null || this != target.DataContext && !object.Equals(oldValue, target.DataContext))
      return;
    target.DataContext = newValue ?? (object) this;
  }
}
