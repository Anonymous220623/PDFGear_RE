// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.Window
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Extension;
using HandyControl.Tools.Interop;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_NonClientArea", Type = typeof (UIElement))]
public class Window : System.Windows.Window
{
  private const string ElementNonClientArea = "PART_NonClientArea";
  private bool _isFullScreen;
  private Thickness _actualBorderThickness;
  private readonly Thickness _commonPadding;
  private bool _showNonClientArea = true;
  private double _tempNonClientAreaHeight;
  private WindowState _tempWindowState;
  private WindowStyle _tempWindowStyle;
  private ResizeMode _tempResizeMode;
  private UIElement _nonClientArea;
  public static readonly DependencyProperty NonClientAreaContentProperty = DependencyProperty.Register(nameof (NonClientAreaContent), typeof (object), typeof (Window), new PropertyMetadata((object) null));
  public static readonly DependencyProperty CloseButtonHoverBackgroundProperty = DependencyProperty.Register(nameof (CloseButtonHoverBackground), typeof (Brush), typeof (Window), new PropertyMetadata((object) null));
  public static readonly DependencyProperty CloseButtonHoverForegroundProperty = DependencyProperty.Register(nameof (CloseButtonHoverForeground), typeof (Brush), typeof (Window), new PropertyMetadata((object) null));
  public static readonly DependencyProperty CloseButtonBackgroundProperty = DependencyProperty.Register(nameof (CloseButtonBackground), typeof (Brush), typeof (Window), new PropertyMetadata((object) Brushes.Transparent));
  public static readonly DependencyProperty CloseButtonForegroundProperty = DependencyProperty.Register(nameof (CloseButtonForeground), typeof (Brush), typeof (Window), new PropertyMetadata((object) Brushes.White));
  public static readonly DependencyProperty OtherButtonBackgroundProperty = DependencyProperty.Register(nameof (OtherButtonBackground), typeof (Brush), typeof (Window), new PropertyMetadata((object) Brushes.Transparent));
  public static readonly DependencyProperty OtherButtonForegroundProperty = DependencyProperty.Register(nameof (OtherButtonForeground), typeof (Brush), typeof (Window), new PropertyMetadata((object) Brushes.White));
  public static readonly DependencyProperty OtherButtonHoverBackgroundProperty = DependencyProperty.Register(nameof (OtherButtonHoverBackground), typeof (Brush), typeof (Window), new PropertyMetadata((object) null));
  public static readonly DependencyProperty OtherButtonHoverForegroundProperty = DependencyProperty.Register(nameof (OtherButtonHoverForeground), typeof (Brush), typeof (Window), new PropertyMetadata((object) null));
  public static readonly DependencyProperty NonClientAreaBackgroundProperty = DependencyProperty.Register(nameof (NonClientAreaBackground), typeof (Brush), typeof (Window), new PropertyMetadata((object) null));
  public static readonly DependencyProperty NonClientAreaForegroundProperty = DependencyProperty.Register(nameof (NonClientAreaForeground), typeof (Brush), typeof (Window), new PropertyMetadata((object) null));
  public static readonly DependencyProperty NonClientAreaHeightProperty = DependencyProperty.Register(nameof (NonClientAreaHeight), typeof (double), typeof (Window), new PropertyMetadata((object) 22.0));
  public static readonly DependencyProperty ShowNonClientAreaProperty = DependencyProperty.Register(nameof (ShowNonClientArea), typeof (bool), typeof (Window), new PropertyMetadata(ValueBoxes.TrueBox, new PropertyChangedCallback(Window.OnShowNonClientAreaChanged)));
  public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(nameof (ShowTitle), typeof (bool), typeof (Window), new PropertyMetadata(ValueBoxes.TrueBox));
  public static readonly DependencyProperty IsFullScreenProperty = DependencyProperty.Register(nameof (IsFullScreen), typeof (bool), typeof (Window), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(Window.OnIsFullScreenChanged)));
  public static readonly DependencyProperty ShowIconProperty = DependencyProperty.Register(nameof (ShowIcon), typeof (bool), typeof (Window), new PropertyMetadata(ValueBoxes.TrueBox));

  static Window()
  {
    FrameworkElement.StyleProperty.OverrideMetadata(typeof (Window), (PropertyMetadata) new FrameworkPropertyMetadata((object) HandyControl.Tools.ResourceHelper.GetResourceInternal<Style>("WindowWin10")));
  }

  public Window()
  {
    WindowChrome windowChrome = new WindowChrome()
    {
      CornerRadius = new CornerRadius(),
      GlassFrameThickness = new Thickness(0.0, 0.0, 0.0, 1.0),
      UseAeroCaptionButtons = false
    };
    BindingOperations.SetBinding((DependencyObject) windowChrome, WindowChrome.CaptionHeightProperty, (BindingBase) new Binding(Window.NonClientAreaHeightProperty.Name)
    {
      Source = (object) this
    });
    WindowChrome.SetWindowChrome((System.Windows.Window) this, windowChrome);
    this._commonPadding = this.Padding;
    this.Loaded += (RoutedEventHandler) ((s, e) => this.OnLoaded(e));
  }

  public object NonClientAreaContent
  {
    get => this.GetValue(Window.NonClientAreaContentProperty);
    set => this.SetValue(Window.NonClientAreaContentProperty, value);
  }

  public Brush CloseButtonHoverBackground
  {
    get => (Brush) this.GetValue(Window.CloseButtonHoverBackgroundProperty);
    set => this.SetValue(Window.CloseButtonHoverBackgroundProperty, (object) value);
  }

  public Brush CloseButtonHoverForeground
  {
    get => (Brush) this.GetValue(Window.CloseButtonHoverForegroundProperty);
    set => this.SetValue(Window.CloseButtonHoverForegroundProperty, (object) value);
  }

  public Brush CloseButtonBackground
  {
    get => (Brush) this.GetValue(Window.CloseButtonBackgroundProperty);
    set => this.SetValue(Window.CloseButtonBackgroundProperty, (object) value);
  }

  public Brush CloseButtonForeground
  {
    get => (Brush) this.GetValue(Window.CloseButtonForegroundProperty);
    set => this.SetValue(Window.CloseButtonForegroundProperty, (object) value);
  }

  public Brush OtherButtonBackground
  {
    get => (Brush) this.GetValue(Window.OtherButtonBackgroundProperty);
    set => this.SetValue(Window.OtherButtonBackgroundProperty, (object) value);
  }

  public Brush OtherButtonForeground
  {
    get => (Brush) this.GetValue(Window.OtherButtonForegroundProperty);
    set => this.SetValue(Window.OtherButtonForegroundProperty, (object) value);
  }

  public Brush OtherButtonHoverBackground
  {
    get => (Brush) this.GetValue(Window.OtherButtonHoverBackgroundProperty);
    set => this.SetValue(Window.OtherButtonHoverBackgroundProperty, (object) value);
  }

  public Brush OtherButtonHoverForeground
  {
    get => (Brush) this.GetValue(Window.OtherButtonHoverForegroundProperty);
    set => this.SetValue(Window.OtherButtonHoverForegroundProperty, (object) value);
  }

  public Brush NonClientAreaBackground
  {
    get => (Brush) this.GetValue(Window.NonClientAreaBackgroundProperty);
    set => this.SetValue(Window.NonClientAreaBackgroundProperty, (object) value);
  }

  public Brush NonClientAreaForeground
  {
    get => (Brush) this.GetValue(Window.NonClientAreaForegroundProperty);
    set => this.SetValue(Window.NonClientAreaForegroundProperty, (object) value);
  }

  public double NonClientAreaHeight
  {
    get => (double) this.GetValue(Window.NonClientAreaHeightProperty);
    set => this.SetValue(Window.NonClientAreaHeightProperty, (object) value);
  }

  public bool ShowNonClientArea
  {
    get => (bool) this.GetValue(Window.ShowNonClientAreaProperty);
    set => this.SetValue(Window.ShowNonClientAreaProperty, ValueBoxes.BooleanBox(value));
  }

  public bool ShowTitle
  {
    get => (bool) this.GetValue(Window.ShowTitleProperty);
    set => this.SetValue(Window.ShowTitleProperty, ValueBoxes.BooleanBox(value));
  }

  public bool IsFullScreen
  {
    get => (bool) this.GetValue(Window.IsFullScreenProperty);
    set => this.SetValue(Window.IsFullScreenProperty, ValueBoxes.BooleanBox(value));
  }

  public bool ShowIcon
  {
    get => (bool) this.GetValue(Window.ShowIconProperty);
    set => this.SetValue(Window.ShowIconProperty, (object) value);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this._nonClientArea = this.GetTemplateChild("PART_NonClientArea") as UIElement;
  }

  protected override void OnSourceInitialized(EventArgs e)
  {
    base.OnSourceInitialized(e);
    this.GetHwndSource()?.AddHook(new System.Windows.Interop.HwndSourceHook(this.HwndSourceHook));
  }

  protected override void OnStateChanged(EventArgs e)
  {
    base.OnStateChanged(e);
    if (this.WindowState == WindowState.Maximized)
    {
      this.BorderThickness = new Thickness();
      this._tempNonClientAreaHeight = this.NonClientAreaHeight;
      this.NonClientAreaHeight += 8.0;
    }
    else
    {
      this.BorderThickness = this._actualBorderThickness;
      this.NonClientAreaHeight = this._tempNonClientAreaHeight;
    }
  }

  protected new void OnLoaded(RoutedEventArgs args)
  {
    this._actualBorderThickness = this.BorderThickness;
    this._tempNonClientAreaHeight = this.NonClientAreaHeight;
    if (this.WindowState == WindowState.Maximized)
    {
      this.BorderThickness = new Thickness();
      this._tempNonClientAreaHeight += 8.0;
    }
    this.CommandBindings.Add(new CommandBinding((ICommand) SystemCommands.MinimizeWindowCommand, (ExecutedRoutedEventHandler) ((s, e) => this.WindowState = WindowState.Minimized)));
    this.CommandBindings.Add(new CommandBinding((ICommand) SystemCommands.MaximizeWindowCommand, (ExecutedRoutedEventHandler) ((s, e) => this.WindowState = WindowState.Maximized)));
    this.CommandBindings.Add(new CommandBinding((ICommand) SystemCommands.RestoreWindowCommand, (ExecutedRoutedEventHandler) ((s, e) => this.WindowState = WindowState.Normal)));
    this.CommandBindings.Add(new CommandBinding((ICommand) SystemCommands.CloseWindowCommand, (ExecutedRoutedEventHandler) ((s, e) => this.Close())));
    this.CommandBindings.Add(new CommandBinding((ICommand) SystemCommands.ShowSystemMenuCommand, new ExecutedRoutedEventHandler(this.ShowSystemMenu)));
    this._tempWindowState = this.WindowState;
    this._tempWindowStyle = this.WindowStyle;
    this._tempResizeMode = this.ResizeMode;
    this.SwitchIsFullScreen(this._isFullScreen);
    this.SwitchShowNonClientArea(this._showNonClientArea);
    if (this.WindowState == WindowState.Maximized)
      this._tempNonClientAreaHeight -= 8.0;
    if (this.SizeToContent != SizeToContent.WidthAndHeight)
      return;
    this.SizeToContent = SizeToContent.Height;
    this.Dispatcher.BeginInvoke((Delegate) (() => this.SizeToContent = SizeToContent.WidthAndHeight));
  }

  protected override void OnContentRendered(EventArgs e)
  {
    base.OnContentRendered(e);
    if (this.SizeToContent != SizeToContent.WidthAndHeight)
      return;
    this.InvalidateMeasure();
  }

  private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
  {
    InteropValues.MINMAXINFO structure = (InteropValues.MINMAXINFO) Marshal.PtrToStructure(lParam, typeof (InteropValues.MINMAXINFO));
    IntPtr hMonitor = InteropMethods.MonitorFromWindow(hwnd, 2);
    if (hMonitor != IntPtr.Zero && structure != null)
    {
      InteropValues.APPBARDATA pData = new InteropValues.APPBARDATA();
      if (InteropMethods.SHAppBarMessage(4, ref pData) > 0U)
      {
        InteropValues.MONITORINFO monitorInfo = new InteropValues.MONITORINFO();
        monitorInfo.cbSize = (uint) Marshal.SizeOf(typeof (InteropValues.MONITORINFO));
        InteropMethods.GetMonitorInfo(hMonitor, ref monitorInfo);
        InteropValues.RECT rcWork = monitorInfo.rcWork;
        InteropValues.RECT rcMonitor = monitorInfo.rcMonitor;
        structure.ptMaxPosition.X = Math.Abs(rcWork.Left - rcMonitor.Left);
        structure.ptMaxPosition.Y = Math.Abs(rcWork.Top - rcMonitor.Top);
        structure.ptMaxSize.X = Math.Abs(rcWork.Right - rcWork.Left);
        structure.ptMaxSize.Y = Math.Abs(rcWork.Bottom - rcWork.Top - 1);
      }
    }
    Marshal.StructureToPtr<InteropValues.MINMAXINFO>(structure, lParam, true);
  }

  private IntPtr HwndSourceHook(
    IntPtr hwnd,
    int msg,
    IntPtr wparam,
    IntPtr lparam,
    ref bool handled)
  {
    switch (msg)
    {
      case 36:
        this.WmGetMinMaxInfo(hwnd, lparam);
        this.Padding = this.WindowState == WindowState.Maximized ? WindowHelper.WindowMaximizedPadding : this._commonPadding;
        break;
      case 71:
        this.Padding = this.WindowState == WindowState.Maximized ? WindowHelper.WindowMaximizedPadding : this._commonPadding;
        break;
      case 132:
        try
        {
          lparam.ToInt32();
          break;
        }
        catch (OverflowException ex)
        {
          handled = true;
          break;
        }
    }
    return IntPtr.Zero;
  }

  private static void OnShowNonClientAreaChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Window) d).SwitchShowNonClientArea((bool) e.NewValue);
  }

  private void SwitchShowNonClientArea(bool showNonClientArea)
  {
    if (this._nonClientArea == null)
      this._showNonClientArea = showNonClientArea;
    else if (showNonClientArea)
    {
      if (this.IsFullScreen)
      {
        this._nonClientArea.Show(false);
        this._tempNonClientAreaHeight = this.NonClientAreaHeight;
        this.NonClientAreaHeight = 0.0;
      }
      else
      {
        this._nonClientArea.Show(true);
        this.NonClientAreaHeight = this._tempNonClientAreaHeight;
      }
    }
    else
    {
      this._nonClientArea.Show(false);
      this._tempNonClientAreaHeight = this.NonClientAreaHeight;
      this.NonClientAreaHeight = 0.0;
    }
  }

  private static void OnIsFullScreenChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((Window) d).SwitchIsFullScreen((bool) e.NewValue);
  }

  private void SwitchIsFullScreen(bool isFullScreen)
  {
    if (this._nonClientArea == null)
      this._isFullScreen = isFullScreen;
    else if (isFullScreen)
    {
      this._nonClientArea.Show(false);
      this._tempNonClientAreaHeight = this.NonClientAreaHeight;
      this.NonClientAreaHeight = 0.0;
      this._tempWindowState = this.WindowState;
      this._tempWindowStyle = this.WindowStyle;
      this._tempResizeMode = this.ResizeMode;
      this.WindowStyle = WindowStyle.None;
      this.WindowState = WindowState.Maximized;
      this.WindowState = WindowState.Minimized;
      this.WindowState = WindowState.Maximized;
    }
    else
    {
      if (this.ShowNonClientArea)
      {
        this._nonClientArea.Show(true);
        this.NonClientAreaHeight = this._tempNonClientAreaHeight;
      }
      else
      {
        this._nonClientArea.Show(false);
        this._tempNonClientAreaHeight = this.NonClientAreaHeight;
        this.NonClientAreaHeight = 0.0;
      }
      this.WindowState = this._tempWindowState;
      this.WindowStyle = this._tempWindowStyle;
      this.ResizeMode = this._tempResizeMode;
    }
  }

  private void ShowSystemMenu(object sender, ExecutedRoutedEventArgs e)
  {
    SystemCommands.ShowSystemMenu((System.Windows.Window) this, this.WindowState == WindowState.Maximized ? new Point(0.0, this.NonClientAreaHeight) : new Point(this.Left, this.Top + this.NonClientAreaHeight));
  }
}
