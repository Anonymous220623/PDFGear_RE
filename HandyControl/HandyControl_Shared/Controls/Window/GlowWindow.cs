// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.GlowWindow
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Expression.Drawing;
using HandyControl.Tools;
using HandyControl.Tools.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace HandyControl.Controls;

public class GlowWindow : Window
{
  internal int DeferGlowChangesCount;
  private readonly GlowEdge[] _glowEdges = new GlowEdge[4];
  private DispatcherTimer _makeGlowVisibleTimer;
  private bool _isGlowVisible;
  private bool _useLogicalSizeForRestore;
  private bool _updatingZOrder;
  private Rect _logicalSizeForRestore = Rect.Empty;
  public static readonly DependencyProperty ActiveGlowColorProperty = DependencyProperty.Register(nameof (ActiveGlowColor), typeof (Color), typeof (GlowWindow), new PropertyMetadata((object) new Color(), new PropertyChangedCallback(GlowWindow.OnGlowColorChanged)));
  public static readonly DependencyProperty InactiveGlowColorProperty = DependencyProperty.Register(nameof (InactiveGlowColor), typeof (Color), typeof (GlowWindow), new PropertyMetadata((object) new Color(), new PropertyChangedCallback(GlowWindow.OnGlowColorChanged)));

  public Color ActiveGlowColor
  {
    get => (Color) this.GetValue(GlowWindow.ActiveGlowColorProperty);
    set => this.SetValue(GlowWindow.ActiveGlowColorProperty, (object) value);
  }

  public Color InactiveGlowColor
  {
    get => (Color) this.GetValue(GlowWindow.InactiveGlowColorProperty);
    set => this.SetValue(GlowWindow.InactiveGlowColorProperty, (object) value);
  }

  internal void EndDeferGlowChanges()
  {
    foreach (GlowEdge loadedGlowWindow in this.LoadedGlowWindows)
      loadedGlowWindow.CommitChanges();
  }

  protected virtual bool ShouldShowGlow
  {
    get
    {
      IntPtr handle = WindowHelper.GetHandle(this);
      return InteropMethods.IsWindowVisible(handle) && !InteropMethods.IsIconic(handle) && !InteropMethods.IsZoomed(handle) && this.ResizeMode != 0;
    }
  }

  private IntPtr HwndSourceHook(
    IntPtr hWnd,
    int msg,
    IntPtr wParam,
    IntPtr lParam,
    ref bool handled)
  {
    if (msg <= 71)
    {
      if (msg == 6)
        return IntPtr.Zero;
      if (msg != 18)
      {
        if (msg != 70)
        {
          if (msg != 71)
            return IntPtr.Zero;
          this.WmWindowPosChanged(lParam);
          return IntPtr.Zero;
        }
        this.WmWindowPosChanging(lParam);
        return IntPtr.Zero;
      }
    }
    else if (msg <= 166)
    {
      switch (msg - 128 /*0x80*/)
      {
        case 0:
          break;
        case 1:
        case 2:
          return IntPtr.Zero;
        case 6:
          handled = true;
          return this.WmNcActivate(hWnd, wParam);
        default:
          if ((uint) (msg - 164) > 2U)
            return IntPtr.Zero;
          handled = true;
          return IntPtr.Zero;
      }
    }
    else
    {
      if ((uint) (msg - 174) <= 1U)
      {
        handled = true;
        return IntPtr.Zero;
      }
      if (msg != 274)
        return IntPtr.Zero;
      this.WmSysCommand(hWnd, wParam);
      return IntPtr.Zero;
    }
    handled = true;
    return this.CallDefWindowProcWithoutVisibleStyle(hWnd, msg, wParam, lParam);
  }

  protected override void OnActivated(EventArgs e)
  {
    this.UpdateGlowActiveState();
    base.OnActivated(e);
  }

  protected override void OnDeactivated(EventArgs e)
  {
    this.UpdateGlowActiveState();
    base.OnDeactivated(e);
  }

  protected override void OnClosed(EventArgs e)
  {
    this.StopTimer();
    this.DestroyGlowWindows();
    base.OnClosed(e);
  }

  protected override void OnSourceInitialized(EventArgs e)
  {
    base.OnSourceInitialized(e);
    HwndSource hwndSource = this.GetHwndSource();
    if (hwndSource == null)
      return;
    hwndSource.AddHook(new System.Windows.Interop.HwndSourceHook(this.HwndSourceHook));
    this.CreateGlowWindowHandles();
  }

  private IEnumerable<GlowEdge> LoadedGlowWindows
  {
    get
    {
      return ((IEnumerable<GlowEdge>) this._glowEdges).Where<GlowEdge>((Func<GlowEdge, bool>) (w => w != null));
    }
  }

  private bool IsGlowVisible
  {
    get => this._isGlowVisible;
    set
    {
      if (this._isGlowVisible == value)
        return;
      this._isGlowVisible = value;
      for (int direction = 0; direction < this._glowEdges.Length; ++direction)
        this.GetOrCreateGlowWindow(direction).IsVisible = value;
    }
  }

  private GlowEdge GetOrCreateGlowWindow(int direction)
  {
    GlowEdge glowEdge1 = this._glowEdges[direction];
    if (glowEdge1 != null)
      return glowEdge1;
    GlowEdge[] glowEdges = this._glowEdges;
    int index = direction;
    GlowEdge glowEdge2 = new GlowEdge(this, (Dock) direction);
    glowEdge2.ActiveGlowColor = this.ActiveGlowColor;
    glowEdge2.InactiveGlowColor = this.InactiveGlowColor;
    glowEdge2.IsActive = this.IsActive;
    GlowEdge glowWindow = glowEdge2;
    glowEdges[index] = glowEdge2;
    return glowWindow;
  }

  private static void OnResizeModeChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs e)
  {
    ((GlowWindow) obj).UpdateGlowVisibility(false);
  }

  private static void OnGlowColorChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    ((GlowWindow) obj).UpdateGlowColors();
  }

  private void UpdateGlowColors()
  {
    using (this.DeferGlowChanges())
    {
      foreach (GlowEdge loadedGlowWindow in this.LoadedGlowWindows)
      {
        loadedGlowWindow.ActiveGlowColor = this.ActiveGlowColor;
        loadedGlowWindow.InactiveGlowColor = this.InactiveGlowColor;
      }
    }
  }

  private void UpdateGlowVisibility(bool delayIfNecessary)
  {
    bool shouldShowGlow = this.ShouldShowGlow;
    if (shouldShowGlow == this.IsGlowVisible)
      return;
    if (SystemParameters.MinimizeAnimation & shouldShowGlow & delayIfNecessary)
    {
      if (this._makeGlowVisibleTimer != null)
      {
        this._makeGlowVisibleTimer.Stop();
      }
      else
      {
        this._makeGlowVisibleTimer = new DispatcherTimer()
        {
          Interval = TimeSpan.FromMilliseconds(200.0)
        };
        this._makeGlowVisibleTimer.Tick += new EventHandler(this.OnDelayedVisibilityTimerTick);
      }
      this._makeGlowVisibleTimer.Start();
    }
    else
    {
      this.StopTimer();
      this.IsGlowVisible = shouldShowGlow;
    }
  }

  private void StopTimer()
  {
    if (this._makeGlowVisibleTimer == null)
      return;
    this._makeGlowVisibleTimer.Stop();
    this._makeGlowVisibleTimer.Tick -= new EventHandler(this.OnDelayedVisibilityTimerTick);
    this._makeGlowVisibleTimer = (DispatcherTimer) null;
  }

  private void OnDelayedVisibilityTimerTick(object sender, EventArgs e)
  {
    this.StopTimer();
    this.UpdateGlowWindowPositions(false);
  }

  private void UpdateGlowWindowPositions(bool delayIfNecessary)
  {
    using (this.DeferGlowChanges())
    {
      this.UpdateGlowVisibility(delayIfNecessary);
      foreach (GlowEdge loadedGlowWindow in this.LoadedGlowWindows)
        loadedGlowWindow.UpdateWindowPos();
    }
  }

  private IDisposable DeferGlowChanges() => (IDisposable) new ChangeScope(this);

  private void UpdateGlowActiveState()
  {
    using (this.DeferGlowChanges())
    {
      foreach (GlowEdge loadedGlowWindow in this.LoadedGlowWindows)
        loadedGlowWindow.IsActive = this.IsActive;
    }
  }

  private void DestroyGlowWindows()
  {
    for (int index = 0; index < this._glowEdges.Length; ++index)
    {
      using (this._glowEdges[index])
        this._glowEdges[index] = (GlowEdge) null;
    }
  }

  private void WmWindowPosChanging(IntPtr lParam)
  {
    InteropValues.WINDOWPOS structure = (InteropValues.WINDOWPOS) Marshal.PtrToStructure(lParam, typeof (InteropValues.WINDOWPOS));
    if (((int) structure.flags & 2) != 0 || ((int) structure.flags & 1) != 0 || structure.cx <= 0 || structure.cy <= 0)
      return;
    Rect floatRect = new Rect((double) structure.x, (double) structure.y, (double) structure.cx, (double) structure.cy).DeviceToLogicalUnits();
    if (this._useLogicalSizeForRestore)
    {
      floatRect = this._logicalSizeForRestore;
      this._logicalSizeForRestore = Rect.Empty;
      this._useLogicalSizeForRestore = false;
    }
    Rect deviceUnits = this.GetOnScreenPosition(floatRect).LogicalToDeviceUnits();
    structure.x = (int) deviceUnits.X;
    structure.y = (int) deviceUnits.Y;
    Marshal.StructureToPtr<InteropValues.WINDOWPOS>(structure, lParam, true);
  }

  private void UpdateZOrderOfOwner(IntPtr hwndOwner)
  {
    IntPtr lastOwnedWindow = IntPtr.Zero;
    InteropMethods.EnumThreadWindows(InteropMethods.GetCurrentThreadId(), (InteropValues.EnumWindowsProc) ((hwnd, lParam) =>
    {
      if (InteropMethods.GetWindow(hwnd, 4) == hwndOwner)
        lastOwnedWindow = hwnd;
      return true;
    }), IntPtr.Zero);
    if (!(lastOwnedWindow != IntPtr.Zero) || !(InteropMethods.GetWindow(hwndOwner, 3) != lastOwnedWindow))
      return;
    InteropMethods.SetWindowPos(hwndOwner, lastOwnedWindow, 0, 0, 0, 0, 19);
  }

  private void UpdateZOrderOfThisAndOwner()
  {
    if (this._updatingZOrder)
      return;
    try
    {
      this._updatingZOrder = true;
      WindowInteropHelper windowInteropHelper = new WindowInteropHelper((System.Windows.Window) this);
      IntPtr handle = windowInteropHelper.Handle;
      foreach (GlowEdge loadedGlowWindow in this.LoadedGlowWindows)
      {
        if (InteropMethods.GetWindow(loadedGlowWindow.Handle, 3) != handle)
          InteropMethods.SetWindowPos(loadedGlowWindow.Handle, handle, 0, 0, 0, 0, 19);
        handle = loadedGlowWindow.Handle;
      }
      IntPtr owner = windowInteropHelper.Owner;
      if (!(owner != IntPtr.Zero))
        return;
      this.UpdateZOrderOfOwner(owner);
    }
    finally
    {
      this._updatingZOrder = false;
    }
  }

  private void WmWindowPosChanged(IntPtr lParam)
  {
    try
    {
      this.UpdateGlowWindowPositions(((int) ((InteropValues.WINDOWPOS) Marshal.PtrToStructure(lParam, typeof (InteropValues.WINDOWPOS))).flags & 64 /*0x40*/) == 0);
      this.UpdateZOrderOfThisAndOwner();
    }
    catch
    {
    }
  }

  private Rect GetOnScreenPosition(Rect floatRect)
  {
    Rect onScreenPosition = floatRect;
    floatRect = floatRect.LogicalToDeviceUnits();
    Rect rect1;
    Rect rect2;
    ScreenHelper.FindMaximumSingleMonitorRectangle(floatRect, out rect1, out rect2);
    if (!floatRect.IntersectsWith(rect2))
    {
      ScreenHelper.FindMonitorRectsFromPoint(InteropMethods.GetCursorPos(), out rect1, out rect2);
      Rect logicalUnits = rect2.DeviceToLogicalUnits();
      if (onScreenPosition.Width > logicalUnits.Width)
        onScreenPosition.Width = logicalUnits.Width;
      if (onScreenPosition.Height > logicalUnits.Height)
        onScreenPosition.Height = logicalUnits.Height;
      if (logicalUnits.Right <= onScreenPosition.X)
        onScreenPosition.X = logicalUnits.Right - onScreenPosition.Width;
      if (logicalUnits.Left > onScreenPosition.X + onScreenPosition.Width)
        onScreenPosition.X = logicalUnits.Left;
      if (logicalUnits.Bottom <= onScreenPosition.Y)
        onScreenPosition.Y = logicalUnits.Bottom - onScreenPosition.Height;
      if (logicalUnits.Top > onScreenPosition.Y + onScreenPosition.Height)
        onScreenPosition.Y = logicalUnits.Top;
    }
    return onScreenPosition;
  }

  private static InteropValues.MONITORINFO MonitorInfoFromWindow(IntPtr hWnd)
  {
    IntPtr hMonitor = InteropMethods.MonitorFromWindow(hWnd, 2);
    InteropValues.MONITORINFO monitorinfo = new InteropValues.MONITORINFO();
    monitorinfo.cbSize = (uint) Marshal.SizeOf(typeof (InteropValues.MONITORINFO));
    ref InteropValues.MONITORINFO local = ref monitorinfo;
    InteropMethods.GetMonitorInfo(hMonitor, ref local);
    return monitorinfo;
  }

  private IntPtr WmNcActivate(IntPtr hWnd, IntPtr wParam)
  {
    return InteropMethods.DefWindowProc(hWnd, 134, wParam, InteropMethods.HRGN_NONE);
  }

  private bool IsAeroSnappedToMonitor(IntPtr hWnd)
  {
    InteropValues.MONITORINFO monitorinfo = GlowWindow.MonitorInfoFromWindow(hWnd);
    Rect deviceUnits = new Rect(this.Left, this.Top, this.Width, this.Height).LogicalToDeviceUnits();
    return MathHelper.AreClose((double) monitorinfo.rcWork.Height, deviceUnits.Height) && MathHelper.AreClose((double) monitorinfo.rcWork.Top, deviceUnits.Top);
  }

  private void WmSysCommand(IntPtr hWnd, IntPtr wParam)
  {
    int scWparam = InteropMethods.GET_SC_WPARAM(wParam);
    if (scWparam == 61456)
      InteropMethods.RedrawWindow(hWnd, IntPtr.Zero, IntPtr.Zero, InteropValues.RedrawWindowFlags.Invalidate | InteropValues.RedrawWindowFlags.NoChildren | InteropValues.RedrawWindowFlags.UpdateNow | InteropValues.RedrawWindowFlags.Frame);
    if ((scWparam == 61488 || scWparam == 61472 || scWparam == 61456 || scWparam == 61440 /*0xF000*/) && this.WindowState == WindowState.Normal && !this.IsAeroSnappedToMonitor(hWnd))
      this._logicalSizeForRestore = new Rect(this.Left, this.Top, this.Width, this.Height);
    if (scWparam == 61456 && this.WindowState == WindowState.Maximized && this._logicalSizeForRestore == Rect.Empty)
      this._logicalSizeForRestore = new Rect(this.Left, this.Top, this.Width, this.Height);
    if (scWparam != 61728 || this.WindowState == WindowState.Minimized || this._logicalSizeForRestore.Width <= 0.0 || this._logicalSizeForRestore.Height <= 0.0)
      return;
    this.Left = this._logicalSizeForRestore.Left;
    this.Top = this._logicalSizeForRestore.Top;
    this.Width = this._logicalSizeForRestore.Width;
    this.Height = this._logicalSizeForRestore.Height;
    this._useLogicalSizeForRestore = true;
  }

  private IntPtr CallDefWindowProcWithoutVisibleStyle(
    IntPtr hWnd,
    int msg,
    IntPtr wParam,
    IntPtr lParam)
  {
    int num1 = VisualHelper.ModifyStyle(hWnd, 268435456 /*0x10000000*/, 0) ? 1 : 0;
    IntPtr num2 = InteropMethods.DefWindowProc(hWnd, msg, wParam, lParam);
    if (num1 != 0)
      VisualHelper.ModifyStyle(hWnd, 0, 268435456 /*0x10000000*/);
    return num2;
  }

  private void CreateGlowWindowHandles()
  {
    for (int direction = 0; direction < this._glowEdges.Length; ++direction)
      this.GetOrCreateGlowWindow(direction).EnsureHandle();
  }
}
