// Decompiled with JetBrains decompiler
// Type: FileWatcher.Views.NotifyView
// Assembly: FileWatcher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 60845E26-CA8C-4ACF-A930-D757CD9B4993
// Assembly location: C:\Program Files\PDFgear\FileWatcher.exe

using CommomLib.Commom;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Threading;

#nullable disable
namespace FileWatcher.Views;

public partial class NotifyView : Window, IComponentConnector
{
  private string filePath = "";
  private bool closing;
  private bool closed;
  private DispatcherTimer closetimer;
  public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius", typeof (CornerRadius), typeof (NotifyView), new PropertyMetadata((object) new CornerRadius()));
  internal Grid LayoutRoot;
  internal System.Windows.Shapes.Rectangle BackgroundRect;
  internal System.Windows.Controls.Image LeftImage;
  internal TextBlock FileName;
  internal TextBlock FilePath;
  internal TextBlock FileSize;
  internal System.Windows.Controls.Button Open;
  internal System.Windows.Controls.CheckBox DisableFileWatcherCheckBox;
  private bool _contentLoaded;

  public NotifyView(string filePath)
  {
    this.InitializeComponent();
    this.filePath = filePath;
    this.Loaded += new RoutedEventHandler(this.NotifyView_Loaded);
    this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.NotifyView_IsVisibleChanged);
    this.LayoutRoot.Opacity = 0.0;
    this.FileName.Text = System.IO.Path.GetFileName(filePath);
    this.FilePath.Text = System.IO.Path.GetFullPath(filePath);
    long length = new FileInfo(filePath).Length;
    this.FileSize.Text = length >= 102L ? (length >= 102400L /*0x019000*/ ? (length >= 104857600L /*0x06400000*/ ? $"{(double) length / 1024.0 / 1024.0 / 1024.0:0.##}GB" : $"{(double) length / 1024.0 / 1024.0:0.##}MB") : $"{(double) length / 1024.0:0.##}KB") : $"{length:0.##}B";
    this.closetimer = new DispatcherTimer()
    {
      Interval = TimeSpan.FromSeconds(10.0)
    };
    this.closetimer.Tick += new EventHandler(this.Closetimer_Tick);
  }

  private void NotifyView_Loaded(object sender, RoutedEventArgs e)
  {
  }

  protected override void OnSourceInitialized(EventArgs e)
  {
    base.OnSourceInitialized(e);
    IntPtr handle = new WindowInteropHelper((Window) this).Handle;
    NotifyView.NativeMethods.SetWindowLongPtr(handle, -20, new IntPtr((long) NotifyView.NativeMethods.GetWindowLongPtr(handle, -20) | 134217856L /*0x08000080*/));
    GAManager2.SendEvent("FileWatcher", "NotifyShow", "Count", 1L);
  }

  private void NotifyView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue is bool newValue && !newValue)
      return;
    foreach (NotifyView notifyView in App.Current.Windows.OfType<NotifyView>().ToList<NotifyView>())
    {
      if (notifyView != this)
      {
        notifyView.closed = true;
        notifyView.Close();
      }
    }
    Screen primaryScreen = Screen.PrimaryScreen;
    System.Drawing.Rectangle bounds = primaryScreen.Bounds;
    uint dpiX;
    int dpiForMonitor = (int) NotifyView.NativeMethods.GetDpiForMonitor(NotifyView.NativeMethods.MonitorFromPoint(new System.Drawing.Point(bounds.Left + bounds.Width / 2, bounds.Top + bounds.Height / 2), NotifyView.NativeMethods.MonitorOptions.MONITOR_DEFAULTTOPRIMARY), out dpiX, out uint _);
    double num = (double) dpiX / 96.0;
    double width = this.Width;
    double height = this.Height;
    int cx = (int) Math.Ceiling(width * num);
    int cy = (int) Math.Ceiling(height * num);
    System.Drawing.Rectangle workingArea = primaryScreen.WorkingArea;
    int X = workingArea.Right - cx;
    int Y = workingArea.Bottom - cy;
    NotifyView.NativeMethods.SetWindowPos(new WindowInteropHelper((Window) this).Handle, IntPtr.Zero, X, Y, cx, cy, NotifyView.NativeMethods.SetWindowPosFlags.SWP_NOACTIVATE | NotifyView.NativeMethods.SetWindowPosFlags.SWP_NOZORDER);
    this.Width = width;
    this.Height = height;
    ((Storyboard) this.Resources[(object) "ShowWindow"]).Begin();
    this.LayoutRoot.Opacity = 1.0;
    this.closetimer.Start();
  }

  protected override void OnMouseDown(MouseButtonEventArgs e)
  {
    base.OnMouseDown(e);
    this.OpenFile();
  }

  private void OpenFile()
  {
    GAManager2.SendEvent("FileWatcher", nameof (OpenFile), "Count", 1L);
    string lpFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PDFLauncher.exe");
    if (!this.filePath.StartsWith("\""))
      this.filePath = $"\"{this.filePath}\"";
    NotifyView.NativeMethods.ShellExecute(IntPtr.Zero, "open", lpFile, this.filePath, "", 1);
    this.Close();
  }

  private void DisableFileWatcherCheckBox_Click(object sender, RoutedEventArgs e)
  {
    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() =>
    {
      int num = !this.DisableFileWatcherCheckBox.IsChecked.GetValueOrDefault() ? 1 : 0;
      GAManager2.SendEvent("FileWatcher", "NotifyCheck", this.IsEnabled.ToString(), 1L);
      SettingsHelper.SetIsEnabled(num != 0);
    }));
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    base.OnClosing(e);
    this.closetimer.Stop();
    if (this.closed)
      return;
    e.Cancel = true;
    if (this.closing)
      return;
    this.closing = true;
    ((Timeline) this.Resources[(object) "HideWindow"]).Completed += new EventHandler(this.NotifyView_Completed);
    ((Storyboard) this.Resources[(object) "HideWindow"]).Begin();
  }

  private void Closetimer_Tick(object sender, EventArgs e) => this.Close();

  private void NotifyView_Completed(object sender, EventArgs e)
  {
    int num = !this.DisableFileWatcherCheckBox.IsChecked.GetValueOrDefault() ? 1 : 0;
    this.closed = true;
    this.Close();
    SettingsHelper.SetIsEnabled(num != 0);
    if (SettingsHelper.IsEnabled)
      return;
    App.Current.Shutdown();
  }

  private void Close_Click(object sender, RoutedEventArgs e) => this.Close();

  private void OpenButton_Click(object sender, RoutedEventArgs e) => this.OpenFile();

  public static CornerRadius GetCornerRadius(DependencyObject obj)
  {
    return (CornerRadius) obj.GetValue(NotifyView.CornerRadiusProperty);
  }

  public static void SetCornerRadius(DependencyObject obj, CornerRadius value)
  {
    obj.SetValue(NotifyView.CornerRadiusProperty, (object) value);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    System.Windows.Application.LoadComponent((object) this, new Uri("/FileWatcher;component/views/notifyview.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.LayoutRoot = (Grid) target;
        break;
      case 2:
        this.BackgroundRect = (System.Windows.Shapes.Rectangle) target;
        break;
      case 3:
        this.LeftImage = (System.Windows.Controls.Image) target;
        break;
      case 4:
        this.FileName = (TextBlock) target;
        break;
      case 5:
        this.FilePath = (TextBlock) target;
        break;
      case 6:
        this.FileSize = (TextBlock) target;
        break;
      case 7:
        this.Open = (System.Windows.Controls.Button) target;
        this.Open.Click += new RoutedEventHandler(this.OpenButton_Click);
        break;
      case 8:
        this.DisableFileWatcherCheckBox = (System.Windows.Controls.CheckBox) target;
        this.DisableFileWatcherCheckBox.Click += new RoutedEventHandler(this.DisableFileWatcherCheckBox_Click);
        break;
      case 9:
        ((System.Windows.Controls.Primitives.ButtonBase) target).Click += new RoutedEventHandler(this.Close_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }

  private class NativeMethods
  {
    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
    private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
    private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
    {
      return IntPtr.Size == 8 ? NotifyView.NativeMethods.GetWindowLongPtr64(hWnd, nIndex) : NotifyView.NativeMethods.GetWindowLongPtr32(hWnd, nIndex);
    }

    public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
    {
      return IntPtr.Size == 8 ? NotifyView.NativeMethods.SetWindowLongPtr64(hWnd, nIndex, dwNewLong) : new IntPtr(NotifyView.NativeMethods.SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
    }

    [DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr MonitorFromPoint(
      NotifyView.NativeMethods.POINT pt,
      NotifyView.NativeMethods.MonitorOptions dwFlags);

    public static IntPtr MonitorFromPoint(System.Drawing.Point pt, NotifyView.NativeMethods.MonitorOptions dwFlags)
    {
      return NotifyView.NativeMethods.MonitorFromPoint(new NotifyView.NativeMethods.POINT(pt.X, pt.Y), dwFlags);
    }

    [DllImport("shcore.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern uint GetDpiForMonitor(
      IntPtr hMonitor,
      int dpiType,
      out uint dpiX,
      out uint dpiY);

    public static uint GetDpiForMonitor(IntPtr hMonitor, out uint dpiX, out uint dpiY)
    {
      return NotifyView.NativeMethods.GetDpiForMonitor(hMonitor, 0, out dpiX, out dpiY);
    }

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetWindowPos(
      IntPtr hWnd,
      IntPtr hWndInsertAfter,
      int X,
      int Y,
      int cx,
      int cy,
      NotifyView.NativeMethods.SetWindowPosFlags uFlags);

    [DllImport("Shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr ShellExecute(
      IntPtr hwnd,
      string lpOperation,
      string lpFile,
      string lpParameters,
      string lpDirectory,
      int nShowCmd);

    public enum MonitorOptions : uint
    {
      MONITOR_DEFAULTTONULL,
      MONITOR_DEFAULTTOPRIMARY,
      MONITOR_DEFAULTTONEAREST,
    }

    public struct POINT(int x, int y)
    {
      public int X = x;
      public int Y = y;
    }

    [Flags]
    public enum SetWindowPosFlags : uint
    {
      SWP_ASYNCWINDOWPOS = 16384, // 0x00004000
      SWP_DEFERERASE = 8192, // 0x00002000
      SWP_DRAWFRAME = 32, // 0x00000020
      SWP_FRAMECHANGED = SWP_DRAWFRAME, // 0x00000020
      SWP_HIDEWINDOW = 128, // 0x00000080
      SWP_NOACTIVATE = 16, // 0x00000010
      SWP_NOCOPYBITS = 256, // 0x00000100
      SWP_NOMOVE = 2,
      SWP_NOOWNERZORDER = 512, // 0x00000200
      SWP_NOREDRAW = 8,
      SWP_NOREPOSITION = SWP_NOOWNERZORDER, // 0x00000200
      SWP_NOSENDCHANGING = 1024, // 0x00000400
      SWP_NOSIZE = 1,
      SWP_NOZORDER = 4,
      SWP_SHOWWINDOW = 64, // 0x00000040
    }
  }
}
