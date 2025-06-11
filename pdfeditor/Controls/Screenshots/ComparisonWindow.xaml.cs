// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Screenshots.ComparisonWindow
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Net;
using pdfeditor.Utils;
using pdfeditor.Views;
using PDFKit;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace pdfeditor.Controls.Screenshots;

public partial class ComparisonWindow : Window, IComponentConnector
{
  private ScreenshotDialogResult result;
  private PdfViewer viewer;
  public static readonly DependencyProperty IsThumbnailProperty = DependencyProperty.Register(nameof (IsThumbnail), typeof (bool), typeof (ComparisonWindow), new PropertyMetadata((object) true, new PropertyChangedCallback(ComparisonWindow.OnIsThumbnailPropertyChanged)));
  internal Border LayoutRoot;
  internal Button CopyButton;
  internal Button ScaleButton;
  internal Button DeleteButton;
  internal System.Windows.Controls.Image ContentImage;
  private bool _contentLoaded;

  public ComparisonWindow()
  {
    this.InitializeComponent();
    this.Width = 1.0;
    this.Height = 1.0;
    this.Left = 0.0;
    this.Top = 0.0;
    this.LayoutRoot.Opacity = 0.0;
    this.Loaded += new RoutedEventHandler(this.ComparisonWindow_Loaded);
    this.SourceInitialized += new EventHandler(this.ComparisonWindow_SourceInitialized);
    this.MouseLeftButtonDown += new MouseButtonEventHandler(this.ComparisonWindow_MouseLeftButtonDown);
  }

  private void ComparisonWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.DragMove();
  }

  private void ComparisonWindow_SourceInitialized(object sender, EventArgs e)
  {
    this.SetWindowActivable(new WindowInteropHelper((Window) this).Handle, false);
  }

  private void ComparisonWindow_Loaded(object sender, RoutedEventArgs e)
  {
    this.UpdateWindowState();
  }

  public void SetContent(PdfDocument document, ScreenshotDialogResult result)
  {
    this.result = result;
    if (result == null)
    {
      this.Hide();
      this.ContentImage.Source = (ImageSource) null;
      this.viewer = (PdfViewer) null;
    }
    else
    {
      if (document != null)
        this.viewer = PDFKit.PdfControl.GetPdfControl(document)?.Viewer;
      this.IsThumbnail = true;
      this.Show();
      this.UpdateWindowState();
      this.SetThumbnailLocation();
      this.ContentImage.Source = (ImageSource) (result.RotatedImage ?? result.Image);
    }
  }

  private void SetThumbnailLocation()
  {
    int dpi = 96 /*0x60*/;
    double num1;
    double num2;
    if (this.viewer != null)
    {
      Window window = Window.GetWindow((DependencyObject) this.viewer);
      if (window != null)
      {
        int pixelsPerInchX = (int) VisualTreeHelper.GetDpi((Visual) this.viewer).PixelsPerInchX;
        System.Windows.Point point = this.viewer.TranslatePoint(new System.Windows.Point(), (UIElement) window);
        int titlebarHeight = ComparisonWindow.GetTitlebarHeight();
        num1 = window.Left + 20.0;
        num2 = window.Top + point.Y + (double) titlebarHeight * 96.0 / (double) pixelsPerInchX + 20.0;
        goto label_4;
      }
    }
    Rect scaledWorkArea = this.GetScaledWorkArea(out dpi);
    num1 = scaledWorkArea.Left + 100.0;
    num2 = scaledWorkArea.Top + 100.0;
label_4:
    this.Left = num1;
    this.Top = num2;
  }

  public bool IsThumbnail
  {
    get => (bool) this.GetValue(ComparisonWindow.IsThumbnailProperty);
    set => this.SetValue(ComparisonWindow.IsThumbnailProperty, (object) value);
  }

  private static void OnIsThumbnailPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if ((bool) e.NewValue == (bool) e.OldValue || !(d is ComparisonWindow comparisonWindow))
      return;
    comparisonWindow.UpdateWindowState();
  }

  private void UpdateWindowState()
  {
    if (!this.IsVisible || this.result == null)
      return;
    this.LayoutRoot.Opacity = 1.0;
    Rect scaledWorkArea = this.GetScaledWorkArea(out int _);
    string str1;
    string str2;
    double num1;
    double num2;
    if (this.IsThumbnail)
    {
      str1 = "\uE1D9";
      str2 = pdfeditor.Properties.Resources.WinScreenshotToolbarZoomInContent;
      num1 = 140.0;
      num2 = 140.0;
    }
    else
    {
      str1 = "\uE1D8";
      str2 = pdfeditor.Properties.Resources.WinScreenshotToolbarZoomOutContent;
      System.Windows.Size size = this.result.SelectedClientRect.Size;
      if (size.Width > scaledWorkArea.Width)
        size = new System.Windows.Size(scaledWorkArea.Width, size.Height * scaledWorkArea.Width / size.Width);
      if (size.Height > scaledWorkArea.Height)
        size = new System.Windows.Size(size.Width * scaledWorkArea.Height / size.Height, scaledWorkArea.Height);
      num1 = size.Width;
      num2 = size.Height;
    }
    this.ScaleButton.Content = (object) str1;
    ToolTipService.SetToolTip((DependencyObject) this.ScaleButton, (object) str2);
    this.Width = num1;
    this.Height = num2;
    this.ContentImage.Width = num1;
    this.ContentImage.Height = num2;
  }

  private Rect GetScaledWorkArea(out int dpi)
  {
    HandleRef? nullable = new HandleRef?();
    dpi = 96 /*0x60*/;
    if (ComparisonWindow.MultiMonitorSupport())
    {
      MainView mainView = Application.Current.Windows.OfType<MainView>().FirstOrDefault<MainView>();
      HandleRef hMonitor;
      uint dpiX;
      if (mainView != null && ScreenUtils.TryGetMonitorForWindow((Window) mainView, out hMonitor) && ScreenUtils.TryGetDpiForMonitor(hMonitor, out dpiX, out uint _))
      {
        nullable = new HandleRef?(hMonitor);
        dpi = (int) dpiX;
      }
    }
    Int32Rect workArea;
    if (nullable.HasValue && ScreenUtils.GetMonitorInfo(nullable.Value.Handle, out Int32Rect _, out workArea, out bool _))
    {
      float num = (float) dpi / 96f;
      return new Rect((double) workArea.X * (double) num, (double) workArea.Y * (double) num, (double) workArea.Width * (double) num, (double) workArea.Height * (double) num);
    }
    using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
      dpi = (int) graphics.DpiX;
    return SystemParameters.WorkArea;
  }

  public static int GetTitlebarHeight() => ComparisonWindow.GetSystemMetrics(4);

  private static bool MultiMonitorSupport() => ComparisonWindow.GetSystemMetrics(80 /*0x50*/) != 0;

  private void SetWindowActivable(IntPtr hwnd, bool value)
  {
    int windowLongW = ComparisonWindow.GetWindowLongW(hwnd, -20);
    int dwNewLong = !value ? windowLongW & -134217729 : windowLongW | 134217728 /*0x08000000*/;
    ComparisonWindow.SetWindowLongW(hwnd, -20, dwNewLong);
  }

  [DllImport("user32", SetLastError = true)]
  private static extern int GetWindowLongW(IntPtr hWnd, int nIndex);

  [DllImport("user32", SetLastError = true)]
  private static extern int SetWindowLongW(IntPtr hWnd, int nIndex, int dwNewLong);

  [DllImport("user32")]
  private static extern int GetSystemMetrics(int nIndex);

  private async void CopyButton_Click(object sender, RoutedEventArgs e)
  {
    if (this.result == null)
      return;
    ((UIElement) sender).IsEnabled = false;
    try
    {
      Clipboard.SetImage((BitmapSource) (this.result.RotatedImage ?? this.result.Image));
    }
    catch
    {
    }
    await Task.Delay(300);
    ((UIElement) sender).IsEnabled = true;
    CommomLib.Commom.GAManager.SendEvent("Screenshot", "Copy2", "Count", 1L);
  }

  private void DeleteButton_Click(object sender, RoutedEventArgs e)
  {
    this.SetContent((PdfDocument) null, (ScreenshotDialogResult) null);
  }

  private void ScaleButton_Click(object sender, RoutedEventArgs e)
  {
    this.IsThumbnail = !this.IsThumbnail;
  }

  private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
  {
    this.IsThumbnail = !this.IsThumbnail;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/screenshots/comparisonwindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        ((Control) target).MouseDoubleClick += new MouseButtonEventHandler(this.Window_MouseDoubleClick);
        break;
      case 2:
        this.LayoutRoot = (Border) target;
        break;
      case 3:
        this.CopyButton = (Button) target;
        this.CopyButton.Click += new RoutedEventHandler(this.CopyButton_Click);
        break;
      case 4:
        this.ScaleButton = (Button) target;
        this.ScaleButton.Click += new RoutedEventHandler(this.ScaleButton_Click);
        break;
      case 5:
        this.DeleteButton = (Button) target;
        this.DeleteButton.Click += new RoutedEventHandler(this.DeleteButton_Click);
        break;
      case 6:
        this.ContentImage = (System.Windows.Controls.Image) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
