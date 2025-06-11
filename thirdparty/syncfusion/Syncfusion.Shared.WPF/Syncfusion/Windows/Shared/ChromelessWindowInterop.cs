// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ChromelessWindowInterop
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class ChromelessWindowInterop
{
  internal const int WM_SYSCOMMAND = 274;
  internal const int WM_LBUTTONUP = 514;
  internal const int TPM_RETURNCMD = 256 /*0x0100*/;
  internal const int MONITOR_DEFAULTTONEAREST = 2;

  [DllImport("user32")]
  internal static extern int SetWindowPos(
    IntPtr hwnd,
    int hwndInsertAfter,
    int x,
    int y,
    int cx,
    int cy,
    int wFlags);

  [DllImport("SHELL32", CallingConvention = CallingConvention.StdCall)]
  internal static extern uint SHAppBarMessage(
    int dwMessage,
    ref ChromelessWindowInterop.APPBARDATA pData);

  [DllImport("user32", SetLastError = true)]
  internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

  [DllImport("user32")]
  internal static extern bool GetWindowRect(IntPtr hWnd, ref ChromelessWindowInterop.RECT r);

  [DllImport("dwmapi.dll", CharSet = CharSet.Auto)]
  internal static extern void DwmExtendFrameIntoClientArea(
    IntPtr hWnd,
    ref ChromelessWindowInterop.MARGINS pMarInset);

  [DllImport("dwmapi.dll", CharSet = CharSet.Auto)]
  private static extern void DwmIsCompositionEnabled(ref bool pfEnabled);

  [DllImport("User32.dll", CharSet = CharSet.Auto)]
  internal static extern bool GetMonitorInfo(
    IntPtr hMonitor,
    ChromelessWindowInterop.MONITORINFO lpmi);

  [DllImport("User32.dll", CharSet = CharSet.Auto)]
  private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

  [DllImport("User32.dll", CharSet = CharSet.Auto)]
  internal static extern IntPtr MonitorFromWindow(IntPtr hwnd, int dwFlags);

  [DllImport("user32.dll", CharSet = CharSet.Auto)]
  internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

  [CLSCompliant(false)]
  [DllImport("User32.dll", CharSet = CharSet.Auto)]
  public static extern int TrackPopupMenu(
    IntPtr hMenu,
    uint uFlags,
    int x,
    int y,
    int nReserved,
    IntPtr hWnd,
    IntPtr prcRect);

  [DllImport("gdi32.dll")]
  internal static extern IntPtr CreateRoundRectRgn(
    int x1,
    int y1,
    int x2,
    int y2,
    int cx,
    int cy);

  [DllImport("user32.dll")]
  internal static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

  [DllImport("user32")]
  internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

  [DllImport("user32")]
  internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

  [DllImport("user32")]
  internal static extern bool SetWindowPos(
    IntPtr hWnd,
    IntPtr hWndInsertAfter,
    int x,
    int y,
    int cx,
    int cy,
    int uFlags);

  [DllImport("user32.dll")]
  internal static extern bool GetCursorPos(out ChromelessWindowInterop.POINT lpPoint);

  [DllImport("User32.dll", CharSet = CharSet.Auto)]
  internal static extern bool AdjustWindowRectEx(
    ref ChromelessWindowInterop.RECT lpRECT,
    int dwStyle,
    bool bMenu,
    int dwExStyle);

  [DllImport("dwmapi.dll", CharSet = CharSet.Auto)]
  internal static extern int DwmDefWindowProc(
    IntPtr hwnd,
    int msg,
    IntPtr wParam,
    IntPtr lParam,
    out IntPtr plResult);

  internal static bool IsVista() => Environment.OSVersion.Version.Major >= 6;

  internal static short GetX(int point) => (short) (point & (int) ushort.MaxValue);

  internal static short GetY(int point) => (short) (point >> 16 /*0x10*/ & (int) ushort.MaxValue);

  internal static int GetX(IntPtr point) => (int) ChromelessWindowInterop.GetX((int) point);

  internal static int GetY(IntPtr point) => (int) ChromelessWindowInterop.GetX((int) point);

  internal static void ShowSystemMenu(IntPtr handle, System.Windows.Point point)
  {
    int wParam = ChromelessWindowInterop.TrackPopupMenu(ChromelessWindowInterop.GetSystemMenu(handle, false), 256U /*0x0100*/, (int) point.X, (int) point.Y, 0, handle, IntPtr.Zero);
    if (wParam == 0)
      return;
    ChromelessWindowInterop.SendMessage(handle, 274, wParam, 0);
  }

  internal static void HandleMinMax(ChromelessWindow window, IntPtr hwnd, IntPtr lParam)
  {
    ChromelessWindowInterop.MINMAXINFO structure = (ChromelessWindowInterop.MINMAXINFO) Marshal.PtrToStructure(lParam, typeof (ChromelessWindowInterop.MINMAXINFO));
    IntPtr hMonitor = ChromelessWindowInterop.MonitorFromWindow(hwnd, 2);
    if (hMonitor != IntPtr.Zero)
    {
      int num = 3;
      ChromelessWindowInterop.MONITORINFO lpmi = new ChromelessWindowInterop.MONITORINFO();
      ChromelessWindowInterop.GetMonitorInfo(hMonitor, lpmi);
      ChromelessWindowInterop.RECT rcWork = lpmi.rcWork;
      ChromelessWindowInterop.RECT rcMonitor = lpmi.rcMonitor;
      structure.ptMaxPosition.x = Math.Abs(rcWork.left - rcMonitor.left) - num;
      structure.ptMaxPosition.y = Math.Abs(rcWork.top - rcMonitor.top) - num;
      structure.ptMaxSize.x = Math.Abs(rcWork.right - rcWork.left) + 2 * num;
      structure.ptMaxSize.y = Math.Abs(rcWork.bottom - rcWork.top) + 2 * num;
      structure.ptMinTrackSize.x = 160 /*0xA0*/;
      structure.ptMinTrackSize.y = 38;
    }
    Marshal.StructureToPtr<ChromelessWindowInterop.MINMAXINFO>(structure, lParam, true);
  }

  internal static bool CanEnableDwm()
  {
    if (!ChromelessWindowInterop.IsVista() || BrowserInteropHelper.IsBrowserHosted)
      return false;
    bool pfEnabled = false;
    ChromelessWindowInterop.DwmIsCompositionEnabled(ref pfEnabled);
    return pfEnabled;
  }

  internal static System.Windows.Point GetTransformedPoint(Visual visual)
  {
    PresentationSource presentationSource = PresentationSource.FromVisual(visual);
    System.Windows.Point transformedPoint = new System.Windows.Point(120.0, 120.0);
    if (presentationSource != null)
    {
      MatrixTransform matrixTransform = new MatrixTransform(presentationSource.CompositionTarget.TransformToDevice);
      System.Windows.Point point1 = new System.Windows.Point(0.0, 0.0);
      System.Windows.Point point2 = matrixTransform.Transform(point1);
      System.Windows.Point point3 = new System.Windows.Point(96.0, 96.0);
      point3 = matrixTransform.Transform(point3);
      transformedPoint.X = point3.X - point2.X;
      transformedPoint.Y = point3.Y - point2.Y;
    }
    return transformedPoint;
  }

  internal static void ExtendWindow(IntPtr hWnd, int size)
  {
    ChromelessWindowInterop.DwmExtendFrameIntoClientArea(hWnd, ref new ChromelessWindowInterop.MARGINS()
    {
      cxLeftWidth = 0,
      cxRightWidth = 0,
      cyTopHeight = size,
      cyBottomHeight = 0
    });
  }

  internal struct POINT
  {
    internal int x;
    internal int y;

    internal POINT(int x1, int y1)
    {
      this.x = x1;
      this.y = y1;
    }

    internal System.Windows.Point Location => new System.Windows.Point((double) this.x, (double) this.y);
  }

  internal struct RECT
  {
    internal int left;
    internal int top;
    internal int right;
    internal int bottom;

    internal RECT(int l, int t, int r, int b)
    {
      this.left = l;
      this.top = t;
      this.right = r;
      this.bottom = b;
    }

    internal int Height => this.bottom - this.top;

    internal int Width => this.right - this.left;

    internal Rect ToRectangle()
    {
      return new Rect((double) this.left, (double) this.top, (double) (this.right - this.left), (double) (this.bottom - this.top));
    }

    internal System.Drawing.Point Location => new System.Drawing.Point(this.left, this.top);

    internal static ChromelessWindowInterop.RECT FromRectangle(Rect r)
    {
      return new ChromelessWindowInterop.RECT((int) Math.Ceiling(r.Left), (int) Math.Ceiling(r.Top), (int) Math.Ceiling(r.Right), (int) Math.Ceiling(r.Bottom));
    }

    internal static Rect GetExtendedRect(
      Rect rect,
      Thickness thickness,
      Thickness padding,
      Thickness Margin)
    {
      return new Rect(rect.Left + thickness.Left + padding.Left + Margin.Left, rect.Top + thickness.Top + padding.Top + Margin.Top, Math.Max(0.0, rect.Width - (thickness.Left + padding.Left) - (thickness.Right + padding.Right) - (Margin.Left + Margin.Right)), Math.Max(0.0, rect.Height - (thickness.Top + padding.Top) - (thickness.Bottom + padding.Bottom)) - (Margin.Top + Margin.Bottom));
    }
  }

  internal struct APPBARDATA
  {
    public int cbSize;
    public IntPtr hWnd;
    public int uCallbackMessage;
    public int uEdge;
    public ChromelessWindowInterop.RECT rc;
    public IntPtr lParam;
  }

  internal struct MARGINS
  {
    internal int cxLeftWidth;
    internal int cxRightWidth;
    internal int cyTopHeight;
    internal int cyBottomHeight;

    internal MARGINS(Thickness thickness)
    {
      this.cxLeftWidth = (int) thickness.Left;
      this.cxRightWidth = (int) thickness.Right;
      this.cyTopHeight = (int) thickness.Top;
      this.cyBottomHeight = (int) thickness.Bottom;
    }
  }

  [StructLayout(LayoutKind.Sequential)]
  internal class MONITORINFO
  {
    internal int cbSize = Marshal.SizeOf(typeof (ChromelessWindowInterop.MONITORINFO));
    internal ChromelessWindowInterop.RECT rcMonitor;
    internal ChromelessWindowInterop.RECT rcWork;
    internal int dwFlags;
  }

  internal struct MINMAXINFO
  {
    internal ChromelessWindowInterop.POINT ptReserved;
    internal ChromelessWindowInterop.POINT ptMaxSize;
    internal ChromelessWindowInterop.POINT ptMaxPosition;
    internal ChromelessWindowInterop.POINT ptMinTrackSize;
    internal ChromelessWindowInterop.POINT ptMaxTrackSize;
  }

  internal struct WINDOWPOS
  {
    internal IntPtr hwnd;
    internal IntPtr hwndInsertAfter;
    internal int x;
    internal int y;
    internal int cx;
    internal int cy;
    internal int flags;
  }

  internal struct NCCALCSIZE_PARAMS
  {
    internal ChromelessWindowInterop.RECT rgrc0;
    internal ChromelessWindowInterop.RECT rgrc1;
    internal ChromelessWindowInterop.RECT rgrc2;
    internal IntPtr lppos;
  }

  public enum SizingDirection
  {
    None,
    West,
    East,
    North,
    NorthWest,
    NorthEast,
    South,
    SouthWest,
    SouthEast,
  }
}
