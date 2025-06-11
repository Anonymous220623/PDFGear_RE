// Decompiled with JetBrains decompiler
// Type: CommomLib.AppTheme.WindowThemeHelper
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Commom;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace CommomLib.AppTheme;

public static class WindowThemeHelper
{
  private static bool installed;
  private static Dispatcher dispatcher;
  private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
  private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
  private static int Attribute = -1;
  private static bool NeedRedrawFrame = false;
  private static object locker = new object();

  public static bool Initialize()
  {
    if (WindowThemeHelper.installed)
      return true;
    lock (WindowThemeHelper.locker)
    {
      if (WindowThemeHelper.dispatcher == null)
        WindowThemeHelper.dispatcher = Application.Current.Dispatcher;
      if (!WindowThemeHelper.dispatcher.CheckAccess())
        return false;
      ThemeResourceDictionary.MainThemeResourceDictionaryHelper.MainThemeResourceDictionaryThemeChanged += new EventHandler(WindowThemeHelper.OnThemeChanged);
      Application.Current.GetWindowLifetimeListener().WindowCreated += new EventHandler<WindowLifetimeListenerEventArgs>(WindowThemeHelper.Listener_WindowCreated);
      WindowThemeHelper.installed = true;
      return true;
    }
  }

  public static bool Uninitialize()
  {
    if (!WindowThemeHelper.installed)
      return true;
    lock (WindowThemeHelper.locker)
    {
      if (!WindowThemeHelper.dispatcher.CheckAccess())
        return false;
      ThemeResourceDictionary.MainThemeResourceDictionaryHelper.MainThemeResourceDictionaryThemeChanged -= new EventHandler(WindowThemeHelper.OnThemeChanged);
      Application.Current.GetWindowLifetimeListener().WindowCreated -= new EventHandler<WindowLifetimeListenerEventArgs>(WindowThemeHelper.Listener_WindowCreated);
      WindowThemeHelper.installed = false;
      return true;
    }
  }

  private static void Listener_WindowCreated(object sender, WindowLifetimeListenerEventArgs e)
  {
    WindowThemeHelper.SetWindowTheme(e.HwndSource.Handle, WindowThemeHelper.GetCurrentTheme());
  }

  private static SystemThemeListener.ActualTheme GetCurrentTheme()
  {
    ThemeResourceDictionary forCurrentApp = ThemeResourceDictionary.GetForCurrentApp();
    return forCurrentApp != null && !(forCurrentApp.Theme != "Dark") ? SystemThemeListener.ActualTheme.Dark : SystemThemeListener.ActualTheme.Light;
  }

  private static void OnThemeChanged(object sender, EventArgs e)
  {
    lock (WindowThemeHelper.locker)
    {
      foreach (Window window in Application.Current.Windows.OfType<Window>())
        WindowThemeHelper.SetWindowTheme(window, WindowThemeHelper.GetCurrentTheme());
    }
  }

  public static bool SetWindowTheme(Window window, SystemThemeListener.ActualTheme theme)
  {
    return window != null && PresentationSource.FromVisual((Visual) window) is HwndSource hwndSource && WindowThemeHelper.SetWindowTheme(hwndSource.Handle, theme);
  }

  public static bool SetWindowTheme(IntPtr hwnd, SystemThemeListener.ActualTheme theme)
  {
    if (hwnd == IntPtr.Zero)
      return false;
    if (WindowThemeHelper.Attribute == -1)
    {
      lock (WindowThemeHelper.locker)
      {
        if (WindowThemeHelper.Attribute == -1)
        {
          Version version = Environment.OSVersion.Version;
          WindowThemeHelper.Attribute = !(version >= new Version(10, 0, 18985, 0)) ? (!(version >= new Version(10, 0, 17763, 0)) ? 0 : 19) : 20;
          if (version < new Version(10, 0, 22000, 0))
            WindowThemeHelper.NeedRedrawFrame = true;
        }
      }
    }
    if (WindowThemeHelper.Attribute == 0)
      return false;
    int attrValue = theme == SystemThemeListener.ActualTheme.Dark ? 1 : 0;
    int num = WindowThemeHelper.DwmSetWindowAttribute(hwnd, WindowThemeHelper.Attribute, ref attrValue, 4) == 0 ? 1 : 0;
    if (num == 0)
      return num != 0;
    if (!WindowThemeHelper.NeedRedrawFrame)
      return num != 0;
    WindowThemeHelper.DwmExtendFrameIntoClientArea(hwnd, new WindowThemeHelper.MARGINS(32 /*0x20*/, 8, 8, 8));
    WindowThemeHelper.DwmExtendFrameIntoClientArea(hwnd, new WindowThemeHelper.MARGINS(0, 0, 0, 0));
    WindowThemeHelper.SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, 55U);
    return num != 0;
  }

  [DllImport("dwmapi.dll")]
  private static extern int DwmSetWindowAttribute(
    IntPtr hwnd,
    int attr,
    ref int attrValue,
    int attrSize);

  [DllImport("user32.dll")]
  private static extern bool SetWindowPos(
    IntPtr hWnd,
    IntPtr hWndInsertAfter,
    int X,
    int Y,
    int cx,
    int cy,
    uint uFlags);

  [DllImport("user32.dll")]
  private static extern bool GetWindowRect(IntPtr hWnd, out WindowThemeHelper.RECT lpRect);

  [DllImport("user32.dll")]
  private static extern bool FlashWindow(IntPtr hWnd, bool bInvert);

  [DllImport("dwmapi.dll")]
  private static extern int DwmExtendFrameIntoClientArea(
    IntPtr hWnd,
    WindowThemeHelper.MARGINS pMarInset);

  private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

  private struct MARGINS(int cxLeftWidth, int cxRightWidth, int cyTopHeight, int cyBottomHeight)
  {
    public int cxLeftWidth = cxLeftWidth;
    public int cxRightWidth = cxRightWidth;
    public int cyTopHeight = cyTopHeight;
    public int cyBottomHeight = cyBottomHeight;
  }

  private struct RECT : IEquatable<WindowThemeHelper.RECT>
  {
    public int left;
    public int top;
    public int right;
    public int bottom;

    public override bool Equals(object obj)
    {
      return obj is WindowThemeHelper.RECT other && this.Equals(other);
    }

    public bool Equals(WindowThemeHelper.RECT other)
    {
      return other.left == this.left && other.right == this.right && other.top == this.top && other.bottom == this.bottom;
    }

    public override int GetHashCode() => this.left + this.right + this.top + this.bottom;

    public static bool operator ==(WindowThemeHelper.RECT left, WindowThemeHelper.RECT right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(WindowThemeHelper.RECT left, WindowThemeHelper.RECT right)
    {
      return !left.Equals(right);
    }
  }
}
