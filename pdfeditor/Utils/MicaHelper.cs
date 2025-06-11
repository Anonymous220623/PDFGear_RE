// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.MicaHelper
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Shell;

#nullable disable
namespace pdfeditor.Utils;

internal class MicaHelper
{
  private static readonly Version MinSupportedVersion = new Version(10, 0, 22000, 0);
  private readonly Window window;
  private WindowChrome windowChrome;
  private bool isMicaEnabled;
  private FrameworkElement titlebarPlaceholder;

  public static bool IsSupported => false;

  private MicaHelper(Window window)
  {
    this.window = window ?? throw new ArgumentNullException(nameof (window));
    window.StateChanged += new EventHandler(this.Window_StateChanged);
    window.Closed += new EventHandler(this.Window_Closed);
    this.windowChrome = new WindowChrome()
    {
      CornerRadius = new CornerRadius(0.0),
      GlassFrameThickness = new Thickness(-1.0),
      UseAeroCaptionButtons = true,
      ResizeBorderThickness = new Thickness(6.0),
      CaptionHeight = SystemParameters.CaptionHeight,
      NonClientFrameEdges = NonClientFrameEdges.Left | NonClientFrameEdges.Right | NonClientFrameEdges.Bottom
    };
    WindowChrome.SetWindowChrome(window, this.windowChrome);
    SystemParameters.StaticPropertyChanged += new PropertyChangedEventHandler(this.SystemParameters_StaticPropertyChanged);
  }

  private void Window_Closed(object sender, EventArgs e)
  {
    SystemParameters.StaticPropertyChanged -= new PropertyChangedEventHandler(this.SystemParameters_StaticPropertyChanged);
  }

  private void SystemParameters_StaticPropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (!(e.PropertyName == "CaptionHeight"))
      return;
    this.UpdateTitlebar();
  }

  public bool IsMicaEnabled
  {
    get => MicaHelper.IsSupported && this.isMicaEnabled;
    set
    {
      if (this.isMicaEnabled == value)
        return;
      this.isMicaEnabled = value;
      MicaHelper.SetMicaState(this.window, MicaHelper.IsSupported && this.isMicaEnabled, false);
    }
  }

  public FrameworkElement TitlebarPlaceholder
  {
    get => this.titlebarPlaceholder;
    set
    {
      if (this.titlebarPlaceholder == value)
        return;
      this.titlebarPlaceholder = value;
      this.UpdateTitlebar();
    }
  }

  private void Window_StateChanged(object sender, EventArgs e) => this.UpdateTitlebar();

  private void UpdateTitlebar()
  {
    WindowState windowState = this.window.WindowState;
    if (windowState == WindowState.Minimized)
      return;
    double captionHeight = SystemParameters.CaptionHeight;
    Thickness resizeBorderThickness = this.windowChrome.ResizeBorderThickness;
    double top1 = resizeBorderThickness.Top;
    this.windowChrome.CaptionHeight = captionHeight;
    if (this.TitlebarPlaceholder == null)
      return;
    if (windowState == WindowState.Maximized)
    {
      this.TitlebarPlaceholder.Height = captionHeight;
      if (!(this.window.Content is FrameworkElement content))
        return;
      resizeBorderThickness = this.windowChrome.ResizeBorderThickness;
      double left = resizeBorderThickness.Left + 2.0;
      resizeBorderThickness = this.windowChrome.ResizeBorderThickness;
      double top2 = resizeBorderThickness.Top + 2.0;
      resizeBorderThickness = this.windowChrome.ResizeBorderThickness;
      double right = resizeBorderThickness.Right + 2.0;
      resizeBorderThickness = this.windowChrome.ResizeBorderThickness;
      double bottom = resizeBorderThickness.Bottom + 2.0;
      Thickness thickness = new Thickness(left, top2, right, bottom);
      content.Margin = thickness;
    }
    else
    {
      this.TitlebarPlaceholder.Height = captionHeight + top1;
      if (!(this.window.Content is FrameworkElement content))
        return;
      resizeBorderThickness = this.windowChrome.ResizeBorderThickness;
      double left = resizeBorderThickness.Left - 2.0;
      resizeBorderThickness = this.windowChrome.ResizeBorderThickness;
      double top3 = resizeBorderThickness.Top - 2.0;
      resizeBorderThickness = this.windowChrome.ResizeBorderThickness;
      double right = resizeBorderThickness.Right - 2.0;
      resizeBorderThickness = this.windowChrome.ResizeBorderThickness;
      double bottom = resizeBorderThickness.Bottom - 2.0;
      Thickness thickness = new Thickness(left, top3, right, bottom);
      content.Margin = thickness;
    }
  }

  public static MicaHelper Create(Window window)
  {
    return MicaHelper.IsSupported ? new MicaHelper(window) : (MicaHelper) null;
  }

  private static void SetMicaState(Window window, bool isEnabled, bool darkThemeEnabled)
  {
    int pvAttribute1 = 1;
    int pvAttribute2 = 0;
    IntPtr hwnd = new WindowInteropHelper(window).EnsureHandle();
    if (isEnabled & darkThemeEnabled)
      MicaHelper.DwmSetWindowAttribute(hwnd, MicaHelper.DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, ref pvAttribute1, Marshal.SizeOf(typeof (int)));
    else
      MicaHelper.DwmSetWindowAttribute(hwnd, MicaHelper.DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, ref pvAttribute2, Marshal.SizeOf(typeof (int)));
    if (isEnabled)
      MicaHelper.DwmSetWindowAttribute(hwnd, MicaHelper.DwmWindowAttribute.DWMWA_MICA_EFFECT, ref pvAttribute1, Marshal.SizeOf(typeof (int)));
    else
      MicaHelper.DwmSetWindowAttribute(hwnd, MicaHelper.DwmWindowAttribute.DWMWA_MICA_EFFECT, ref pvAttribute2, Marshal.SizeOf(typeof (int)));
  }

  [DllImport("dwmapi.dll")]
  private static extern int DwmSetWindowAttribute(
    IntPtr hwnd,
    MicaHelper.DwmWindowAttribute dwAttribute,
    ref int pvAttribute,
    int cbAttribute);

  [Flags]
  public enum DwmWindowAttribute : uint
  {
    DWMWA_USE_HOSTBACKDROPBRUSH = 17, // 0x00000011
    DWMWA_USE_IMMERSIVE_DARK_MODE = 20, // 0x00000014
    DWMWA_MICA_EFFECT = 1029, // 0x00000405
  }
}
