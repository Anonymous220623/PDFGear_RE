// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.WindowChromeWorker
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Standard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace Syncfusion.Windows;

internal class WindowChromeWorker : DependencyObject
{
  private const SWP _SwpFlags = SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER;
  private readonly List<KeyValuePair<WM, MessageHandler>> _messageTable;
  private Window _window;
  private IntPtr _hwnd;
  private HwndSource _hwndSource;
  private bool _isHooked;
  private bool _isFixedUp;
  private bool _isUserResizing;
  private bool _hasUserMovedWindow;
  private Point _windowPosAtStartOfUserMove = new Point();
  private int _blackGlassFixupAttemptCount;
  private WindowChrome _chromeInfo;
  private WindowState _lastRoundingState;
  private WindowState _lastMenuState;
  private bool _isGlassEnabled;
  public static readonly DependencyProperty WindowChromeWorkerProperty = DependencyProperty.RegisterAttached(nameof (WindowChromeWorker), typeof (WindowChromeWorker), typeof (WindowChromeWorker), new PropertyMetadata((object) null, new PropertyChangedCallback(WindowChromeWorker._OnChromeWorkerChanged)));
  private static readonly HT[,] _HitTestBorders = new HT[3, 3]
  {
    {
      HT.TOPLEFT,
      HT.TOP,
      HT.TOPRIGHT
    },
    {
      HT.LEFT,
      HT.CLIENT,
      HT.RIGHT
    },
    {
      HT.BOTTOMLEFT,
      HT.BOTTOM,
      HT.BOTTOMRIGHT
    }
  };

  public WindowChromeWorker()
  {
    this._messageTable = new List<KeyValuePair<WM, MessageHandler>>()
    {
      new KeyValuePair<WM, MessageHandler>(WM.SETTEXT, new MessageHandler(this._HandleSetTextOrIcon)),
      new KeyValuePair<WM, MessageHandler>(WM.SETICON, new MessageHandler(this._HandleSetTextOrIcon)),
      new KeyValuePair<WM, MessageHandler>(WM.NCACTIVATE, new MessageHandler(this._HandleNCActivate)),
      new KeyValuePair<WM, MessageHandler>(WM.NCCALCSIZE, new MessageHandler(this._HandleNCCalcSize)),
      new KeyValuePair<WM, MessageHandler>(WM.NCHITTEST, new MessageHandler(this._HandleNCHitTest)),
      new KeyValuePair<WM, MessageHandler>(WM.NCRBUTTONUP, new MessageHandler(this._HandleNCRButtonUp)),
      new KeyValuePair<WM, MessageHandler>(WM.SIZE, new MessageHandler(this._HandleSize)),
      new KeyValuePair<WM, MessageHandler>(WM.WINDOWPOSCHANGED, new MessageHandler(this._HandleWindowPosChanged)),
      new KeyValuePair<WM, MessageHandler>(WM.DWMCOMPOSITIONCHANGED, new MessageHandler(this._HandleDwmCompositionChanged))
    };
    if (!Utility.IsPresentationFrameworkVersionLessThan4)
      return;
    this._messageTable.AddRange((IEnumerable<KeyValuePair<WM, MessageHandler>>) new KeyValuePair<WM, MessageHandler>[4]
    {
      new KeyValuePair<WM, MessageHandler>(WM.WININICHANGE, new MessageHandler(this._HandleSettingChange)),
      new KeyValuePair<WM, MessageHandler>(WM.ENTERSIZEMOVE, new MessageHandler(this._HandleEnterSizeMove)),
      new KeyValuePair<WM, MessageHandler>(WM.EXITSIZEMOVE, new MessageHandler(this._HandleExitSizeMove)),
      new KeyValuePair<WM, MessageHandler>(WM.MOVE, new MessageHandler(this._HandleMove))
    });
  }

  public void SetWindowChrome(WindowChrome newChrome)
  {
    this.VerifyAccess();
    if (newChrome == this._chromeInfo)
      return;
    if (this._chromeInfo != null)
      this._chromeInfo.PropertyChangedThatRequiresRepaint -= new EventHandler(this._OnChromePropertyChangedThatRequiresRepaint);
    this._chromeInfo = newChrome;
    if (this._chromeInfo != null)
      this._chromeInfo.PropertyChangedThatRequiresRepaint += new EventHandler(this._OnChromePropertyChangedThatRequiresRepaint);
    this._ApplyNewCustomChrome();
  }

  private void _OnChromePropertyChangedThatRequiresRepaint(object sender, EventArgs e)
  {
    this._UpdateFrameState(true);
  }

  private static void _OnChromeWorkerChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    Window window = (Window) d;
    ((WindowChromeWorker) e.NewValue)._SetWindow(window);
  }

  private void _SetWindow(Window window)
  {
    this._window = window;
    this._hwnd = new WindowInteropHelper(this._window).Handle;
    if (Utility.IsPresentationFrameworkVersionLessThan4)
    {
      Utility.AddDependencyPropertyChangeListener((object) this._window, Control.TemplateProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
      Utility.AddDependencyPropertyChangeListener((object) this._window, FrameworkElement.FlowDirectionProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
    }
    this._window.Closed += new EventHandler(this._UnsetWindow);
    if (IntPtr.Zero != this._hwnd)
    {
      this._hwndSource = HwndSource.FromHwnd(this._hwnd);
      this._window.ApplyTemplate();
      if (this._chromeInfo == null)
        return;
      this._ApplyNewCustomChrome();
    }
    else
      this._window.SourceInitialized += (EventHandler) ((sender, e) =>
      {
        this._hwnd = new WindowInteropHelper(this._window).Handle;
        this._hwndSource = HwndSource.FromHwnd(this._hwnd);
        if (this._chromeInfo == null)
          return;
        this._ApplyNewCustomChrome();
      });
  }

  private void _UnsetWindow(object sender, EventArgs e)
  {
    if (Utility.IsPresentationFrameworkVersionLessThan4)
    {
      Utility.RemoveDependencyPropertyChangeListener((object) this._window, Control.TemplateProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
      Utility.RemoveDependencyPropertyChangeListener((object) this._window, FrameworkElement.FlowDirectionProperty, new EventHandler(this._OnWindowPropertyChangedThatRequiresTemplateFixup));
    }
    if (this._chromeInfo != null)
      this._chromeInfo.PropertyChangedThatRequiresRepaint -= new EventHandler(this._OnChromePropertyChangedThatRequiresRepaint);
    this._RestoreStandardChromeState(true);
  }

  public static WindowChromeWorker GetWindowChromeWorker(Window window)
  {
    Verify.IsNotNull<Window>(window, nameof (window));
    return (WindowChromeWorker) window.GetValue(WindowChromeWorker.WindowChromeWorkerProperty);
  }

  public static void SetWindowChromeWorker(Window window, WindowChromeWorker chrome)
  {
    Verify.IsNotNull<Window>(window, nameof (window));
    window.SetValue(WindowChromeWorker.WindowChromeWorkerProperty, (object) chrome);
  }

  private void _OnWindowPropertyChangedThatRequiresTemplateFixup(object sender, EventArgs e)
  {
    if (this._chromeInfo == null || !(this._hwnd != IntPtr.Zero))
      return;
    this._window.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) new WindowChromeWorker._Action(this._FixupTemplateIssues));
  }

  private void _ApplyNewCustomChrome()
  {
    if (this._hwnd == IntPtr.Zero)
      return;
    if (this._chromeInfo == null)
    {
      this._RestoreStandardChromeState(false);
    }
    else
    {
      if (!this._isHooked)
      {
        this._hwndSource.AddHook(new HwndSourceHook(this._WndProc));
        this._isHooked = true;
      }
      this._FixupTemplateIssues();
      this._UpdateSystemMenu(new WindowState?(this._window.WindowState));
      this._UpdateFrameState(true);
      try
      {
        Standard.NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
      }
      catch (Exception ex)
      {
        using (StreamWriter streamWriter = new StreamWriter(".\\\\Error.txt", true))
        {
          streamWriter.WriteLine($"Message :{ex.Message}<br/>{Environment.NewLine}StackTrace :{ex.StackTrace}{Environment.NewLine}Date :{DateTime.Now.ToString()}");
          streamWriter.WriteLine($"{Environment.NewLine}-----------------------------------------------------------------------------{Environment.NewLine}");
        }
      }
    }
  }

  private void _FixupTemplateIssues()
  {
    if (this._window.Template == null)
      return;
    if (VisualTreeHelper.GetChildrenCount((DependencyObject) this._window) == 0)
    {
      this._window.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) new WindowChromeWorker._Action(this._FixupTemplateIssues));
    }
    else
    {
      Thickness margin = new Thickness();
      FrameworkElement child = (FrameworkElement) VisualTreeHelper.GetChild((DependencyObject) this._window, 0);
      if (this._chromeInfo.NonClientFrameEdges != NonClientFrameEdges.None)
      {
        if (Utility.IsFlagSet((int) this._chromeInfo.NonClientFrameEdges, 2))
          margin.Top -= SystemParameters2.Current.WindowResizeBorderThickness.Top;
        if (Utility.IsFlagSet((int) this._chromeInfo.NonClientFrameEdges, 1))
          margin.Left -= SystemParameters2.Current.WindowResizeBorderThickness.Left;
        if (Utility.IsFlagSet((int) this._chromeInfo.NonClientFrameEdges, 8))
          margin.Bottom -= SystemParameters2.Current.WindowResizeBorderThickness.Bottom;
        if (Utility.IsFlagSet((int) this._chromeInfo.NonClientFrameEdges, 4))
          margin.Right -= SystemParameters2.Current.WindowResizeBorderThickness.Right;
      }
      if (Utility.IsPresentationFrameworkVersionLessThan4)
      {
        RECT windowRect = Standard.NativeMethods.GetWindowRect(this._hwnd);
        RECT adjustedWindowRect = this._GetAdjustedWindowRect(windowRect);
        Rect logical1 = DpiHelper.DeviceRectToLogical(new Rect((double) windowRect.Left, (double) windowRect.Top, (double) windowRect.Width, (double) windowRect.Height));
        Rect logical2 = DpiHelper.DeviceRectToLogical(new Rect((double) adjustedWindowRect.Left, (double) adjustedWindowRect.Top, (double) adjustedWindowRect.Width, (double) adjustedWindowRect.Height));
        if (!Utility.IsFlagSet((int) this._chromeInfo.NonClientFrameEdges, 1))
          margin.Right -= logical1.Left - logical2.Left;
        if (!Utility.IsFlagSet((int) this._chromeInfo.NonClientFrameEdges, 4))
          margin.Right += logical1.Right - logical2.Right;
        if (!Utility.IsFlagSet((int) this._chromeInfo.NonClientFrameEdges, 2))
          margin.Bottom -= logical1.Top - logical2.Top;
        if (!Utility.IsFlagSet((int) this._chromeInfo.NonClientFrameEdges, 8))
          margin.Bottom += logical1.Bottom - logical2.Bottom;
        Transform transform;
        if (this._window.FlowDirection == FlowDirection.RightToLeft)
        {
          Thickness thickness = new Thickness(logical1.Left - logical2.Left, logical1.Top - logical2.Top, logical2.Right - logical1.Right, logical2.Bottom - logical1.Bottom);
          transform = (Transform) new MatrixTransform(1.0, 0.0, 0.0, 1.0, -(thickness.Left + thickness.Right), 0.0);
        }
        else
          transform = (Transform) null;
        child.RenderTransform = transform;
      }
      child.Margin = margin;
      this._chromeInfo.OnMarginChanged(this._chromeInfo, new MarginChangedEventArgs(margin));
      if (!Utility.IsPresentationFrameworkVersionLessThan4 || this._isFixedUp)
        return;
      this._hasUserMovedWindow = false;
      this._window.StateChanged += new EventHandler(this._FixupRestoreBounds);
      this._isFixedUp = true;
    }
  }

  private void _FixupWindows7Issues()
  {
    if (this._blackGlassFixupAttemptCount > 5 || !Utility.IsOSWindows7OrNewer || !Standard.NativeMethods.DwmIsCompositionEnabled())
      return;
    ++this._blackGlassFixupAttemptCount;
    bool flag = false;
    try
    {
      flag = Standard.NativeMethods.DwmGetCompositionTimingInfo(this._hwnd).HasValue;
    }
    catch (Exception ex)
    {
    }
    if (!flag)
      this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) new WindowChromeWorker._Action(this._FixupWindows7Issues));
    else
      this._blackGlassFixupAttemptCount = 0;
  }

  private void _FixupRestoreBounds(object sender, EventArgs e)
  {
    if (this._window.WindowState != WindowState.Maximized && this._window.WindowState != WindowState.Minimized || !this._hasUserMovedWindow)
      return;
    this._hasUserMovedWindow = false;
    WINDOWPLACEMENT windowPlacement = Standard.NativeMethods.GetWindowPlacement(this._hwnd);
    RECT adjustedWindowRect = this._GetAdjustedWindowRect(new RECT()
    {
      Bottom = 100,
      Right = 100
    });
    Point logical = DpiHelper.DevicePixelsToLogical(new Point((double) (windowPlacement.rcNormalPosition.Left - adjustedWindowRect.Left), (double) (windowPlacement.rcNormalPosition.Top - adjustedWindowRect.Top)));
    this._window.Top = logical.Y;
    this._window.Left = logical.X;
  }

  private RECT _GetAdjustedWindowRect(RECT rcWindow)
  {
    WS windowLongPtr1 = (WS) (int) Standard.NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE);
    WS_EX windowLongPtr2 = (WS_EX) (int) Standard.NativeMethods.GetWindowLongPtr(this._hwnd, GWL.EXSTYLE);
    return Standard.NativeMethods.AdjustWindowRectEx(rcWindow, windowLongPtr1, false, windowLongPtr2);
  }

  private HT _GetHTFromResizeGripDirection(ResizeGripDirection direction)
  {
    bool flag = this._window.FlowDirection == FlowDirection.RightToLeft;
    switch (direction)
    {
      case ResizeGripDirection.TopLeft:
        return !flag ? HT.TOPLEFT : HT.TOPRIGHT;
      case ResizeGripDirection.Top:
        return HT.TOP;
      case ResizeGripDirection.TopRight:
        return !flag ? HT.TOPRIGHT : HT.TOPLEFT;
      case ResizeGripDirection.Right:
        return !flag ? HT.RIGHT : HT.LEFT;
      case ResizeGripDirection.BottomRight:
        return !flag ? HT.BOTTOMRIGHT : HT.BOTTOMLEFT;
      case ResizeGripDirection.Bottom:
        return HT.BOTTOM;
      case ResizeGripDirection.BottomLeft:
        return !flag ? HT.BOTTOMLEFT : HT.BOTTOMRIGHT;
      case ResizeGripDirection.Left:
        return !flag ? HT.LEFT : HT.RIGHT;
      default:
        return HT.NOWHERE;
    }
  }

  private bool _IsWindowDocked
  {
    get
    {
      if (this._window.WindowState != WindowState.Normal)
        return false;
      RECT adjustedWindowRect = this._GetAdjustedWindowRect(new RECT()
      {
        Bottom = 100,
        Right = 100
      });
      return this._window.RestoreBounds.Location != new Point(this._window.Left, this._window.Top) - (Vector) DpiHelper.DevicePixelsToLogical(new Point((double) adjustedWindowRect.Left, (double) adjustedWindowRect.Top));
    }
  }

  private IntPtr _WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
  {
    WM uMsg = (WM) msg;
    foreach (KeyValuePair<WM, MessageHandler> keyValuePair in this._messageTable)
    {
      if (keyValuePair.Key == uMsg)
        return keyValuePair.Value(uMsg, wParam, lParam, out handled);
    }
    return IntPtr.Zero;
  }

  private IntPtr _HandleSetTextOrIcon(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
  {
    bool flag = this._ModifyStyle(WS.VISIBLE, WS.OVERLAPPED);
    IntPtr num = Standard.NativeMethods.DefWindowProc(this._hwnd, uMsg, wParam, lParam);
    if (flag)
      this._ModifyStyle(WS.OVERLAPPED, WS.VISIBLE);
    handled = true;
    return num;
  }

  private IntPtr _HandleNCActivate(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
  {
    IntPtr num = Standard.NativeMethods.DefWindowProc(this._hwnd, WM.NCACTIVATE, wParam, new IntPtr(-1));
    handled = true;
    return num;
  }

  private IntPtr _HandleNCCalcSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
  {
    if (this._chromeInfo.NonClientFrameEdges != NonClientFrameEdges.None)
    {
      Thickness thickness = DpiHelper.LogicalThicknessToDevice(SystemParameters2.Current.WindowResizeBorderThickness);
      if (thickness.Right <= 4.0)
        thickness = new Thickness(SystemParameters2.Current.WindowResizeBorderThickness.Left + 4.0, SystemParameters2.Current.WindowResizeBorderThickness.Top + 4.0, SystemParameters2.Current.WindowResizeBorderThickness.Right + 4.0, SystemParameters.ResizeFrameHorizontalBorderHeight + 4.0);
      RECT structure = (RECT) Marshal.PtrToStructure(lParam, typeof (RECT));
      if (Utility.IsFlagSet((int) this._chromeInfo.NonClientFrameEdges, 2))
        structure.Top += (int) thickness.Top;
      if (Utility.IsFlagSet((int) this._chromeInfo.NonClientFrameEdges, 1))
        structure.Left += (int) thickness.Left;
      if (Utility.IsFlagSet((int) this._chromeInfo.NonClientFrameEdges, 8))
        structure.Bottom -= (int) thickness.Bottom;
      if (Utility.IsFlagSet((int) this._chromeInfo.NonClientFrameEdges, 4))
        structure.Right -= (int) thickness.Right;
      Marshal.StructureToPtr<RECT>(structure, lParam, false);
    }
    handled = true;
    return new IntPtr(768 /*0x0300*/);
  }

  private IntPtr _HandleNCHitTest(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
  {
    Point devicePoint1 = new Point((double) Utility.GET_X_LPARAM(lParam), (double) Utility.GET_Y_LPARAM(lParam));
    Rect windowRect = this._GetWindowRect();
    Point devicePoint2 = devicePoint1;
    devicePoint2.Offset(-windowRect.X, -windowRect.Y);
    IInputElement inputElement = this._window.InputHitTest(DpiHelper.DevicePixelsToLogical(devicePoint2));
    if (inputElement != null)
    {
      if (WindowChrome.GetIsHitTestVisibleInChrome(inputElement))
      {
        handled = true;
        return new IntPtr(1);
      }
      ResizeGripDirection resizeGripDirection = WindowChrome.GetResizeGripDirection(inputElement);
      if (resizeGripDirection != ResizeGripDirection.None)
      {
        handled = true;
        return new IntPtr((int) this._GetHTFromResizeGripDirection(resizeGripDirection));
      }
    }
    if (this._chromeInfo.UseAeroCaptionButtons && Utility.IsOSVistaOrNewer && this._chromeInfo.GlassFrameThickness != new Thickness() && this._isGlassEnabled)
    {
      IntPtr plResult;
      handled = Standard.NativeMethods.DwmDefWindowProc(this._hwnd, uMsg, wParam, lParam, out plResult);
      if (IntPtr.Zero != plResult)
        return plResult;
    }
    HT ht = this._HitTestNca(DpiHelper.DeviceRectToLogical(windowRect), DpiHelper.DevicePixelsToLogical(devicePoint1));
    handled = true;
    return new IntPtr((int) ht);
  }

  private IntPtr _HandleNCRButtonUp(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
  {
    if (2 == (int) wParam.ToInt64())
      SystemCommands.ShowSystemMenuPhysicalCoordinates(this._window, new Point((double) Utility.GET_X_LPARAM(lParam), (double) Utility.GET_Y_LPARAM(lParam)));
    handled = false;
    return IntPtr.Zero;
  }

  private IntPtr _HandleSize(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
  {
    WindowState? assumeState = new WindowState?();
    if (wParam.ToInt64() == 2L)
      assumeState = new WindowState?(WindowState.Maximized);
    this._UpdateSystemMenu(assumeState);
    handled = false;
    return IntPtr.Zero;
  }

  private IntPtr _HandleWindowPosChanged(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
  {
    this._UpdateSystemMenu(new WindowState?());
    if (!this._isGlassEnabled)
      this._SetRoundingRegion(new WINDOWPOS?((WINDOWPOS) Marshal.PtrToStructure(lParam, typeof (WINDOWPOS))));
    handled = false;
    return IntPtr.Zero;
  }

  private IntPtr _HandleDwmCompositionChanged(
    WM uMsg,
    IntPtr wParam,
    IntPtr lParam,
    out bool handled)
  {
    this._UpdateFrameState(false);
    handled = false;
    return IntPtr.Zero;
  }

  private IntPtr _HandleSettingChange(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
  {
    this._FixupTemplateIssues();
    handled = false;
    return IntPtr.Zero;
  }

  private IntPtr _HandleEnterSizeMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
  {
    this._isUserResizing = true;
    if (this._window.WindowState != WindowState.Maximized && !this._IsWindowDocked)
      this._windowPosAtStartOfUserMove = new Point(this._window.Left, this._window.Top);
    handled = false;
    return IntPtr.Zero;
  }

  private IntPtr _HandleExitSizeMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
  {
    this._isUserResizing = false;
    if (this._window.WindowState == WindowState.Maximized)
    {
      this._window.Top = this._windowPosAtStartOfUserMove.Y;
      this._window.Left = this._windowPosAtStartOfUserMove.X;
    }
    handled = false;
    return IntPtr.Zero;
  }

  private IntPtr _HandleMove(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled)
  {
    if (this._isUserResizing)
      this._hasUserMovedWindow = true;
    handled = false;
    return IntPtr.Zero;
  }

  private bool _ModifyStyle(WS removeStyle, WS addStyle)
  {
    WS ws1 = IntPtr.Size == 4 ? (WS) Standard.NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE).ToInt32() : (WS) Standard.NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE).ToInt64();
    WS ws2 = ws1 & ~removeStyle | addStyle;
    if (ws1 == ws2)
      return false;
    Standard.NativeMethods.SetWindowLongPtr(this._hwnd, GWL.STYLE, new IntPtr((int) ws2));
    return true;
  }

  private WindowState _GetHwndState()
  {
    switch (Standard.NativeMethods.GetWindowPlacement(this._hwnd).showCmd)
    {
      case SW.SHOWMINIMIZED:
        return WindowState.Minimized;
      case SW.SHOWMAXIMIZED:
        return WindowState.Maximized;
      default:
        return WindowState.Normal;
    }
  }

  private Rect _GetWindowRect()
  {
    RECT windowRect = Standard.NativeMethods.GetWindowRect(this._hwnd);
    return new Rect((double) windowRect.Left, (double) windowRect.Top, (double) windowRect.Width, (double) windowRect.Height);
  }

  private void _UpdateSystemMenu(WindowState? assumeState)
  {
    WindowState windowState = (WindowState) ((int) assumeState ?? (int) this._GetHwndState());
    if (!assumeState.HasValue && this._lastMenuState == windowState)
      return;
    this._lastMenuState = windowState;
    bool flag1 = this._ModifyStyle(WS.VISIBLE, WS.OVERLAPPED);
    IntPtr systemMenu = Standard.NativeMethods.GetSystemMenu(this._hwnd, false);
    if (IntPtr.Zero != systemMenu)
    {
      WS ws = IntPtr.Size == 4 ? (WS) Standard.NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE).ToInt32() : (WS) Standard.NativeMethods.GetWindowLongPtr(this._hwnd, GWL.STYLE).ToInt64();
      bool flag2 = Utility.IsFlagSet((int) ws, 131072 /*0x020000*/);
      bool flag3 = Utility.IsFlagSet((int) ws, 65536 /*0x010000*/);
      bool flag4 = Utility.IsFlagSet((int) ws, 262144 /*0x040000*/);
      switch (windowState)
      {
        case WindowState.Minimized:
          int num1 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.ENABLED);
          int num2 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.GRAYED | MF.DISABLED);
          int num3 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, MF.GRAYED | MF.DISABLED);
          int num4 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, MF.GRAYED | MF.DISABLED);
          int num5 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, flag3 ? MF.ENABLED : MF.GRAYED | MF.DISABLED);
          break;
        case WindowState.Maximized:
          int num6 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.ENABLED);
          int num7 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.GRAYED | MF.DISABLED);
          int num8 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, MF.GRAYED | MF.DISABLED);
          int num9 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, flag2 ? MF.ENABLED : MF.GRAYED | MF.DISABLED);
          int num10 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, MF.GRAYED | MF.DISABLED);
          break;
        default:
          int num11 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.RESTORE, MF.GRAYED | MF.DISABLED);
          int num12 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MOVE, MF.ENABLED);
          int num13 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.SIZE, flag4 ? MF.ENABLED : MF.GRAYED | MF.DISABLED);
          int num14 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MINIMIZE, flag2 ? MF.ENABLED : MF.GRAYED | MF.DISABLED);
          int num15 = (int) Standard.NativeMethods.EnableMenuItem(systemMenu, SC.MAXIMIZE, flag3 ? MF.ENABLED : MF.GRAYED | MF.DISABLED);
          break;
      }
    }
    if (!flag1)
      return;
    this._ModifyStyle(WS.OVERLAPPED, WS.VISIBLE);
  }

  private void _UpdateFrameState(bool force)
  {
    if (IntPtr.Zero == this._hwnd)
      return;
    bool flag = Standard.NativeMethods.DwmIsCompositionEnabled();
    if (!force && flag == this._isGlassEnabled)
      return;
    this._isGlassEnabled = flag && this._chromeInfo.GlassFrameThickness != new Thickness();
    if (!this._isGlassEnabled)
    {
      this._SetRoundingRegion(new WINDOWPOS?());
    }
    else
    {
      this._ClearRoundingRegion();
      this._ExtendGlassFrame();
    }
    try
    {
      Standard.NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
    }
    catch (Exception ex)
    {
      using (StreamWriter streamWriter = new StreamWriter(".\\\\Error.txt", true))
      {
        streamWriter.WriteLine($"Message :{ex.Message}<br/>{Environment.NewLine}StackTrace :{ex.StackTrace}{Environment.NewLine}Date :{DateTime.Now.ToString()}");
        streamWriter.WriteLine($"{Environment.NewLine}-----------------------------------------------------------------------------{Environment.NewLine}");
      }
    }
  }

  private void _ClearRoundingRegion()
  {
    try
    {
      Standard.NativeMethods.SetWindowRgn(this._hwnd, IntPtr.Zero, Standard.NativeMethods.IsWindowVisible(this._hwnd));
    }
    catch (Exception ex)
    {
      using (StreamWriter streamWriter = new StreamWriter(".\\\\Error.txt", true))
      {
        streamWriter.WriteLine($"Message :{ex.Message}<br/>{Environment.NewLine}StackTrace :{ex.StackTrace}{Environment.NewLine}Date :{DateTime.Now.ToString()}");
        streamWriter.WriteLine($"{Environment.NewLine}-----------------------------------------------------------------------------{Environment.NewLine}");
      }
    }
  }

  private void _SetRoundingRegion(WINDOWPOS? wp)
  {
    if (Standard.NativeMethods.GetWindowPlacement(this._hwnd).showCmd == SW.SHOWMAXIMIZED)
    {
      int num1;
      int num2;
      if (wp.HasValue)
      {
        num1 = wp.Value.x;
        num2 = wp.Value.y;
      }
      else
      {
        Rect windowRect = this._GetWindowRect();
        num1 = (int) windowRect.Left;
        num2 = (int) windowRect.Top;
      }
      RECT rcWork = Standard.NativeMethods.GetMonitorInfo(Standard.NativeMethods.MonitorFromWindow(this._hwnd, 2U)).rcWork;
      rcWork.Offset(-num1, -num2);
      IntPtr gdiObject = IntPtr.Zero;
      try
      {
        gdiObject = Standard.NativeMethods.CreateRectRgnIndirect(rcWork);
        Standard.NativeMethods.SetWindowRgn(this._hwnd, gdiObject, Standard.NativeMethods.IsWindowVisible(this._hwnd));
        gdiObject = IntPtr.Zero;
      }
      finally
      {
        Utility.SafeDeleteObject(ref gdiObject);
      }
    }
    else
    {
      Size size;
      if (wp.HasValue && !Utility.IsFlagSet(wp.Value.flags, 1))
      {
        size = new Size((double) wp.Value.cx, (double) wp.Value.cy);
      }
      else
      {
        if (wp.HasValue && this._lastRoundingState == this._window.WindowState)
          return;
        size = this._GetWindowRect().Size;
      }
      this._lastRoundingState = this._window.WindowState;
      IntPtr gdiObject = IntPtr.Zero;
      try
      {
        double num = Math.Min(size.Width, size.Height);
        double radius1 = Math.Min(DpiHelper.LogicalPixelsToDevice(new Point(this._chromeInfo.CornerRadius.TopLeft, 0.0)).X, num / 2.0);
        if (WindowChromeWorker._IsUniform(this._chromeInfo.CornerRadius))
        {
          if (!this._chromeInfo.IsSealed)
            this._chromeInfo.CornerRadius = new CornerRadius(0.0);
          gdiObject = WindowChromeWorker._CreateRoundRectRgn(new Rect(size), radius1);
        }
        else
        {
          gdiObject = WindowChromeWorker._CreateRoundRectRgn(new Rect(0.0, 0.0, size.Width / 2.0 + radius1, size.Height / 2.0 + radius1), radius1);
          double radius2 = Math.Min(DpiHelper.LogicalPixelsToDevice(new Point(this._chromeInfo.CornerRadius.TopRight, 0.0)).X, num / 2.0);
          Rect region1 = new Rect(0.0, 0.0, size.Width / 2.0 + radius2, size.Height / 2.0 + radius2);
          region1.Offset(size.Width / 2.0 - radius2, 0.0);
          WindowChromeWorker._CreateAndCombineRoundRectRgn(gdiObject, region1, radius2);
          double radius3 = Math.Min(DpiHelper.LogicalPixelsToDevice(new Point(this._chromeInfo.CornerRadius.BottomLeft, 0.0)).X, num / 2.0);
          Rect region2 = new Rect(0.0, 0.0, size.Width / 2.0 + radius3, size.Height / 2.0 + radius3);
          region2.Offset(0.0, size.Height / 2.0 - radius3);
          WindowChromeWorker._CreateAndCombineRoundRectRgn(gdiObject, region2, radius3);
          double radius4 = Math.Min(DpiHelper.LogicalPixelsToDevice(new Point(this._chromeInfo.CornerRadius.BottomRight, 0.0)).X, num / 2.0);
          Rect region3 = new Rect(0.0, 0.0, size.Width / 2.0 + radius4, size.Height / 2.0 + radius4);
          region3.Offset(size.Width / 2.0 - radius4, size.Height / 2.0 - radius4);
          WindowChromeWorker._CreateAndCombineRoundRectRgn(gdiObject, region3, radius4);
        }
        Standard.NativeMethods.SetWindowRgn(this._hwnd, gdiObject, Standard.NativeMethods.IsWindowVisible(this._hwnd));
        gdiObject = IntPtr.Zero;
      }
      finally
      {
        Utility.SafeDeleteObject(ref gdiObject);
      }
    }
  }

  private static IntPtr _CreateRoundRectRgn(Rect region, double radius)
  {
    return DoubleUtilities.AreClose(0.0, radius) ? Standard.NativeMethods.CreateRectRgn((int) Math.Floor(region.Left), (int) Math.Floor(region.Top), (int) Math.Ceiling(region.Right), (int) Math.Ceiling(region.Bottom)) : Standard.NativeMethods.CreateRoundRectRgn((int) Math.Floor(region.Left), (int) Math.Floor(region.Top), (int) Math.Ceiling(region.Right) + 1, (int) Math.Ceiling(region.Bottom) + 1, (int) Math.Ceiling(radius), (int) Math.Ceiling(radius));
  }

  private static void _CreateAndCombineRoundRectRgn(IntPtr hrgnSource, Rect region, double radius)
  {
    IntPtr gdiObject = IntPtr.Zero;
    try
    {
      gdiObject = WindowChromeWorker._CreateRoundRectRgn(region, radius);
      if (Standard.NativeMethods.CombineRgn(hrgnSource, hrgnSource, gdiObject, RGN.OR) == CombineRgnResult.ERROR)
        throw new InvalidOperationException("Unable to combine two HRGNs.");
    }
    catch
    {
      Utility.SafeDeleteObject(ref gdiObject);
      throw;
    }
  }

  private static bool _IsUniform(CornerRadius cornerRadius)
  {
    return DoubleUtilities.AreClose(cornerRadius.BottomLeft, cornerRadius.BottomRight) && DoubleUtilities.AreClose(cornerRadius.TopLeft, cornerRadius.TopRight) && DoubleUtilities.AreClose(cornerRadius.BottomLeft, cornerRadius.TopRight);
  }

  private void _ExtendGlassFrame()
  {
    if (!Utility.IsOSVistaOrNewer || IntPtr.Zero == this._hwnd)
      return;
    if (!Standard.NativeMethods.DwmIsCompositionEnabled())
    {
      this._hwndSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;
    }
    else
    {
      if (this._hwndSource != null && this._hwndSource.CompositionTarget != null)
        this._hwndSource.CompositionTarget.BackgroundColor = Colors.Transparent;
      Thickness device1 = DpiHelper.LogicalThicknessToDevice(this._chromeInfo.GlassFrameThickness);
      if (this._chromeInfo.NonClientFrameEdges != NonClientFrameEdges.None)
      {
        Thickness device2 = DpiHelper.LogicalThicknessToDevice(SystemParameters2.Current.WindowResizeBorderThickness);
        if (Utility.IsFlagSet((int) this._chromeInfo.NonClientFrameEdges, 2))
        {
          device1.Top -= device2.Top;
          device1.Top = Math.Max(0.0, device1.Top);
        }
        if (Utility.IsFlagSet((int) this._chromeInfo.NonClientFrameEdges, 1))
        {
          device1.Left -= device2.Left;
          device1.Left = Math.Max(0.0, device1.Left);
        }
        if (Utility.IsFlagSet((int) this._chromeInfo.NonClientFrameEdges, 8))
        {
          device1.Bottom -= device2.Bottom;
          device1.Bottom = Math.Max(0.0, device1.Bottom);
        }
        if (Utility.IsFlagSet((int) this._chromeInfo.NonClientFrameEdges, 4))
        {
          device1.Right -= device2.Right;
          device1.Right = Math.Max(0.0, device1.Right);
        }
      }
      MARGINS pMarInset = new MARGINS()
      {
        cxLeftWidth = (int) Math.Ceiling(device1.Left),
        cxRightWidth = (int) Math.Ceiling(device1.Right),
        cyTopHeight = (int) Math.Ceiling(device1.Top),
        cyBottomHeight = (int) Math.Ceiling(device1.Bottom)
      };
      try
      {
        Standard.NativeMethods.DwmExtendFrameIntoClientArea(this._hwnd, ref pMarInset);
      }
      catch
      {
      }
    }
  }

  private HT _HitTestNca(Rect windowPosition, Point mousePosition)
  {
    int index1 = 1;
    int index2 = 1;
    bool flag = false;
    if (mousePosition.Y >= windowPosition.Top && mousePosition.Y < windowPosition.Top + this._chromeInfo.ResizeBorderThickness.Top + this._chromeInfo.CaptionHeight)
    {
      flag = mousePosition.Y < windowPosition.Top + this._chromeInfo.ResizeBorderThickness.Top;
      index1 = 0;
    }
    else if (mousePosition.Y < windowPosition.Bottom && mousePosition.Y >= windowPosition.Bottom - (double) (int) this._chromeInfo.ResizeBorderThickness.Bottom)
      index1 = 2;
    if (mousePosition.X >= windowPosition.Left && mousePosition.X < windowPosition.Left + (double) (int) this._chromeInfo.ResizeBorderThickness.Left)
      index2 = 0;
    else if (mousePosition.X < windowPosition.Right && mousePosition.X >= windowPosition.Right - this._chromeInfo.ResizeBorderThickness.Right)
      index2 = 2;
    if (index1 == 0 && index2 != 1 && !flag)
      index1 = 1;
    HT ht = WindowChromeWorker._HitTestBorders[index1, index2];
    if (ht == HT.TOP && !flag)
      ht = HT.CAPTION;
    return ht;
  }

  private void _RestoreStandardChromeState(bool isClosing)
  {
    this.VerifyAccess();
    this._UnhookCustomChrome();
    if (isClosing)
      return;
    this._RestoreFrameworkIssueFixups();
    this._RestoreGlassFrame();
    this._RestoreHrgn();
    this._window.InvalidateMeasure();
  }

  private void _UnhookCustomChrome()
  {
    if (!this._isHooked)
      return;
    this._hwndSource.RemoveHook(new HwndSourceHook(this._WndProc));
    this._isHooked = false;
  }

  private void _RestoreFrameworkIssueFixups()
  {
    ((FrameworkElement) VisualTreeHelper.GetChild((DependencyObject) this._window, 0)).Margin = new Thickness();
    if (!Utility.IsPresentationFrameworkVersionLessThan4)
      return;
    this._window.StateChanged -= new EventHandler(this._FixupRestoreBounds);
    this._isFixedUp = false;
  }

  private void _RestoreGlassFrame()
  {
    if (!Utility.IsOSVistaOrNewer || this._hwnd == IntPtr.Zero)
      return;
    this._hwndSource.CompositionTarget.BackgroundColor = SystemColors.WindowColor;
    if (!Standard.NativeMethods.DwmIsCompositionEnabled())
      return;
    MARGINS pMarInset = new MARGINS();
    try
    {
      Standard.NativeMethods.DwmExtendFrameIntoClientArea(this._hwnd, ref pMarInset);
    }
    catch
    {
    }
  }

  private void _RestoreHrgn()
  {
    this._ClearRoundingRegion();
    Standard.NativeMethods.SetWindowPos(this._hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP.DRAWFRAME | SWP.NOACTIVATE | SWP.NOMOVE | SWP.NOOWNERZORDER | SWP.NOSIZE | SWP.NOZORDER);
  }

  private delegate void _Action();
}
