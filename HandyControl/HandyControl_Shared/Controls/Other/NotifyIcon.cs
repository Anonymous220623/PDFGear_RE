// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.NotifyIcon
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools;
using HandyControl.Tools.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

#nullable disable
namespace HandyControl.Controls;

public class NotifyIcon : FrameworkElement, IDisposable
{
  private bool _isMouseOver;
  private bool _added;
  private readonly object _syncObj = new object();
  private readonly int _id;
  private ImageSource _icon;
  private IntPtr _iconCurrentHandle;
  private IntPtr _iconDefaultHandle;
  private IconHandle _iconHandle;
  private const int WmTrayMouseMessage = 2048 /*0x0800*/;
  private string _windowClassName;
  private int _wmTaskbarCreated;
  private IntPtr _messageWindowHandle;
  private readonly InteropValues.WndProc _callback;
  private Popup _contextContent;
  private bool _doubleClick;
  private DispatcherTimer _dispatcherTimerBlink;
  private DispatcherTimer _dispatcherTimerPos;
  private bool _isTransparent;
  private bool _isDisposed;
  private static int NextId;
  private static readonly Dictionary<string, NotifyIcon> NotifyIconDic = new Dictionary<string, NotifyIcon>();
  public static readonly DependencyProperty TokenProperty = DependencyProperty.Register(nameof (Token), typeof (string), typeof (NotifyIcon), new PropertyMetadata((object) null, new PropertyChangedCallback(NotifyIcon.OnTokenChanged)));
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (NotifyIcon), new PropertyMetadata((object) null));
  public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof (Icon), typeof (ImageSource), typeof (NotifyIcon), new PropertyMetadata((object) null, new PropertyChangedCallback(NotifyIcon.OnIconChanged)));
  public static readonly DependencyProperty ContextContentProperty = DependencyProperty.Register(nameof (ContextContent), typeof (object), typeof (NotifyIcon), new PropertyMetadata((object) null));
  public static readonly DependencyProperty BlinkIntervalProperty = DependencyProperty.Register(nameof (BlinkInterval), typeof (TimeSpan), typeof (NotifyIcon), new PropertyMetadata((object) TimeSpan.FromMilliseconds(500.0), new PropertyChangedCallback(NotifyIcon.OnBlinkIntervalChanged)));
  public static readonly DependencyProperty IsBlinkProperty = DependencyProperty.Register(nameof (IsBlink), typeof (bool), typeof (NotifyIcon), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(NotifyIcon.OnIsBlinkChanged)));
  public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (NotifyIcon));
  public static readonly RoutedEvent MouseDoubleClickEvent = EventManager.RegisterRoutedEvent("MouseDoubleClick", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (NotifyIcon));

  static NotifyIcon()
  {
    UIElement.VisibilityProperty.OverrideMetadata(typeof (NotifyIcon), new PropertyMetadata((object) Visibility.Visible, new PropertyChangedCallback(NotifyIcon.OnVisibilityChanged)));
    FrameworkElement.DataContextProperty.OverrideMetadata(typeof (NotifyIcon), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(NotifyIcon.DataContextPropertyChanged)));
    FrameworkElement.ContextMenuProperty.OverrideMetadata(typeof (NotifyIcon), (PropertyMetadata) new FrameworkPropertyMetadata(new PropertyChangedCallback(NotifyIcon.ContextMenuPropertyChanged)));
  }

  private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    NotifyIcon notifyIcon = (NotifyIcon) d;
    if ((Visibility) e.NewValue == Visibility.Visible)
    {
      if (notifyIcon._iconCurrentHandle == IntPtr.Zero)
        notifyIcon.OnIconChanged();
      notifyIcon.UpdateIcon(true);
    }
    else
    {
      if (!(notifyIcon._iconCurrentHandle != IntPtr.Zero))
        return;
      notifyIcon.UpdateIcon(false);
    }
  }

  private static void DataContextPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((NotifyIcon) d).OnDataContextPropertyChanged(e);
  }

  private void OnDataContextPropertyChanged(DependencyPropertyChangedEventArgs e)
  {
    this.UpdateDataContext((FrameworkElement) this._contextContent, e.OldValue, e.NewValue);
    this.UpdateDataContext((FrameworkElement) this.ContextMenu, e.OldValue, e.NewValue);
  }

  private static void ContextMenuPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ((NotifyIcon) d).OnContextMenuPropertyChanged(e);
  }

  private void OnContextMenuPropertyChanged(DependencyPropertyChangedEventArgs e)
  {
    this.UpdateDataContext((FrameworkElement) e.NewValue, (object) null, this.DataContext);
  }

  public NotifyIcon()
  {
    this._id = ++NotifyIcon.NextId;
    this._callback = new InteropValues.WndProc(this.Callback);
    this.Loaded += (RoutedEventHandler) ((s, e) => this.Init());
    if (Application.Current == null)
      return;
    Application.Current.Exit += (ExitEventHandler) ((s, e) => this.Dispose());
  }

  ~NotifyIcon() => this.Dispose(false);

  public void Init()
  {
    this.RegisterClass();
    if (this.Visibility == Visibility.Visible)
    {
      this.OnIconChanged();
      this.UpdateIcon(true);
    }
    this._dispatcherTimerPos = new DispatcherTimer()
    {
      Interval = TimeSpan.FromMilliseconds(200.0)
    };
    this._dispatcherTimerPos.Tick += new EventHandler(this.DispatcherTimerPos_Tick);
  }

  public static void Register(string token, NotifyIcon notifyIcon)
  {
    if (string.IsNullOrEmpty(token) || notifyIcon == null)
      return;
    NotifyIcon.NotifyIconDic[token] = notifyIcon;
  }

  public static void Unregister(string token, NotifyIcon notifyIcon)
  {
    if (string.IsNullOrEmpty(token) || notifyIcon == null || !NotifyIcon.NotifyIconDic.ContainsKey(token) || NotifyIcon.NotifyIconDic[token] != notifyIcon)
      return;
    NotifyIcon.NotifyIconDic.Remove(token);
  }

  public static void Unregister(NotifyIcon notifyIcon)
  {
    if (notifyIcon == null)
      return;
    KeyValuePair<string, NotifyIcon> keyValuePair = NotifyIcon.NotifyIconDic.FirstOrDefault<KeyValuePair<string, NotifyIcon>>((Func<KeyValuePair<string, NotifyIcon>, bool>) (item => notifyIcon == item.Value));
    if (string.IsNullOrEmpty(keyValuePair.Key))
      return;
    NotifyIcon.NotifyIconDic.Remove(keyValuePair.Key);
  }

  public static void Unregister(string token)
  {
    if (string.IsNullOrEmpty(token) || !NotifyIcon.NotifyIconDic.ContainsKey(token))
      return;
    NotifyIcon.NotifyIconDic.Remove(token);
  }

  public static void ShowBalloonTip(
    string title,
    string content,
    NotifyIconInfoType infoType,
    string token)
  {
    NotifyIcon notifyIcon;
    if (!NotifyIcon.NotifyIconDic.TryGetValue(token, out notifyIcon))
      return;
    notifyIcon.ShowBalloonTip(title, content, infoType);
  }

  public void ShowBalloonTip(string title, string content, NotifyIconInfoType infoType)
  {
    if (!this._added || DesignerHelper.IsInDesignMode)
      return;
    InteropValues.NOTIFYICONDATA pnid = new InteropValues.NOTIFYICONDATA()
    {
      uFlags = 16 /*0x10*/,
      hWnd = this._messageWindowHandle,
      uID = this._id,
      szInfoTitle = title ?? string.Empty,
      szInfo = content ?? string.Empty
    };
    InteropValues.NOTIFYICONDATA notifyicondata = pnid;
    int num;
    switch (infoType)
    {
      case NotifyIconInfoType.None:
        num = 0;
        break;
      case NotifyIconInfoType.Info:
        num = 1;
        break;
      case NotifyIconInfoType.Warning:
        num = 2;
        break;
      case NotifyIconInfoType.Error:
        num = 3;
        break;
      default:
        num = pnid.dwInfoFlags;
        break;
    }
    notifyicondata.dwInfoFlags = num;
    InteropMethods.Shell_NotifyIcon(1, pnid);
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  public void CloseContextControl()
  {
    if (this._contextContent != null)
    {
      this._contextContent.IsOpen = false;
    }
    else
    {
      if (this.ContextMenu == null)
        return;
      this.ContextMenu.IsOpen = false;
    }
  }

  private static void OnTokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is NotifyIcon notifyIcon))
      return;
    if (e.NewValue == null)
      NotifyIcon.Unregister(notifyIcon);
    else
      NotifyIcon.Register(e.NewValue.ToString(), notifyIcon);
  }

  public string Token
  {
    get => (string) this.GetValue(NotifyIcon.TokenProperty);
    set => this.SetValue(NotifyIcon.TokenProperty, (object) value);
  }

  public string Text
  {
    get => (string) this.GetValue(NotifyIcon.TextProperty);
    set => this.SetValue(NotifyIcon.TextProperty, (object) value);
  }

  private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    NotifyIcon notifyIcon = (NotifyIcon) d;
    notifyIcon._icon = (ImageSource) e.NewValue;
    notifyIcon.OnIconChanged();
    if (string.IsNullOrEmpty(notifyIcon._windowClassName) || notifyIcon.IsBlink || notifyIcon.Visibility != Visibility.Visible)
      return;
    notifyIcon.UpdateIcon(true);
  }

  public ImageSource Icon
  {
    get => (ImageSource) this.GetValue(NotifyIcon.IconProperty);
    set => this.SetValue(NotifyIcon.IconProperty, (object) value);
  }

  public object ContextContent
  {
    get => this.GetValue(NotifyIcon.ContextContentProperty);
    set => this.SetValue(NotifyIcon.ContextContentProperty, value);
  }

  private static void OnBlinkIntervalChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    NotifyIcon notifyIcon = (NotifyIcon) d;
    if (notifyIcon._dispatcherTimerBlink == null)
      return;
    notifyIcon._dispatcherTimerBlink.Interval = (TimeSpan) e.NewValue;
  }

  public TimeSpan BlinkInterval
  {
    get => (TimeSpan) this.GetValue(NotifyIcon.BlinkIntervalProperty);
    set => this.SetValue(NotifyIcon.BlinkIntervalProperty, (object) value);
  }

  private static void OnIsBlinkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    NotifyIcon notifyIcon = (NotifyIcon) d;
    if (notifyIcon.Visibility != Visibility.Visible)
      return;
    if ((bool) e.NewValue)
    {
      if (notifyIcon._dispatcherTimerBlink == null)
      {
        notifyIcon._dispatcherTimerBlink = new DispatcherTimer()
        {
          Interval = notifyIcon.BlinkInterval
        };
        notifyIcon._dispatcherTimerBlink.Tick += new EventHandler(notifyIcon.DispatcherTimerBlinkTick);
      }
      notifyIcon._dispatcherTimerBlink.Start();
    }
    else
    {
      notifyIcon._dispatcherTimerBlink?.Stop();
      notifyIcon._dispatcherTimerBlink = (DispatcherTimer) null;
      notifyIcon.UpdateIcon(true);
    }
  }

  public bool IsBlink
  {
    get => (bool) this.GetValue(NotifyIcon.IsBlinkProperty);
    set => this.SetValue(NotifyIcon.IsBlinkProperty, ValueBoxes.BooleanBox(value));
  }

  private void DispatcherTimerBlinkTick(object sender, EventArgs e)
  {
    if (this.Visibility != Visibility.Visible || this._iconCurrentHandle == IntPtr.Zero)
      return;
    this.UpdateIcon(true, !this._isTransparent);
  }

  private bool CheckMouseIsEnter()
  {
    List<InteropValues.RECT> rectList;
    if (!this.FindNotifyIcon(out rectList))
      return false;
    InteropValues.POINT point;
    InteropMethods.GetCursorPos(out point);
    return rectList.Any<InteropValues.RECT>((Func<InteropValues.RECT, bool>) (rectNotify => point.X >= rectNotify.Left && point.X <= rectNotify.Right && point.Y >= rectNotify.Top && point.Y <= rectNotify.Bottom));
  }

  private void DispatcherTimerPos_Tick(object sender, EventArgs e)
  {
    if (this.CheckMouseIsEnter())
    {
      if (this._isMouseOver)
        return;
      this._isMouseOver = true;
      MouseEventArgs e1 = new MouseEventArgs(Mouse.PrimaryDevice, Environment.TickCount);
      e1.RoutedEvent = UIElement.MouseEnterEvent;
      this.RaiseEvent((RoutedEventArgs) e1);
      this._dispatcherTimerPos.Interval = TimeSpan.FromMilliseconds(500.0);
    }
    else
    {
      this._dispatcherTimerPos.Stop();
      this._isMouseOver = false;
      MouseEventArgs e2 = new MouseEventArgs(Mouse.PrimaryDevice, Environment.TickCount);
      e2.RoutedEvent = UIElement.MouseLeaveEvent;
      this.RaiseEvent((RoutedEventArgs) e2);
    }
  }

  private IntPtr FindTrayToolbarWindow()
  {
    IntPtr hwndParent = InteropMethods.FindWindow("Shell_TrayWnd", (string) null);
    if (hwndParent != IntPtr.Zero)
    {
      hwndParent = InteropMethods.FindWindowEx(hwndParent, IntPtr.Zero, "TrayNotifyWnd", (string) null);
      if (hwndParent != IntPtr.Zero)
      {
        hwndParent = InteropMethods.FindWindowEx(hwndParent, IntPtr.Zero, "SysPager", (string) null);
        if (hwndParent != IntPtr.Zero)
          hwndParent = InteropMethods.FindWindowEx(hwndParent, IntPtr.Zero, "ToolbarWindow32", (string) null);
      }
    }
    return hwndParent;
  }

  private IntPtr FindTrayToolbarOverFlowWindow()
  {
    IntPtr hwndParent = InteropMethods.FindWindow("NotifyIconOverflowWindow", (string) null);
    if (hwndParent != IntPtr.Zero)
      hwndParent = InteropMethods.FindWindowEx(hwndParent, IntPtr.Zero, "ToolbarWindow32", (string) null);
    return hwndParent;
  }

  private bool FindNotifyIcon(out List<InteropValues.RECT> rectList)
  {
    List<InteropValues.RECT> rectNotifyList = new List<InteropValues.RECT>();
    bool notifyIcon = this.FindNotifyIcon(this.FindTrayToolbarWindow(), ref rectNotifyList);
    if (!notifyIcon)
      notifyIcon = this.FindNotifyIcon(this.FindTrayToolbarOverFlowWindow(), ref rectNotifyList);
    rectList = rectNotifyList;
    return notifyIcon;
  }

  private bool FindNotifyIcon(IntPtr hTrayWnd, ref List<InteropValues.RECT> rectNotifyList)
  {
    InteropValues.RECT lpRect;
    InteropMethods.GetWindowRect(hTrayWnd, out lpRect);
    int num1 = (int) InteropMethods.SendMessage(hTrayWnd, 1048U, 0U, IntPtr.Zero);
    bool notifyIcon = false;
    if (num1 > 0)
    {
      uint lpdwProcessId1;
      int windowThreadProcessId1 = (int) InteropMethods.GetWindowThreadProcessId(hTrayWnd, out lpdwProcessId1);
      IntPtr num2 = InteropMethods.OpenProcess(InteropValues.ProcessAccess.VMOperation | InteropValues.ProcessAccess.VMRead | InteropValues.ProcessAccess.VMWrite, false, lpdwProcessId1);
      IntPtr num3 = InteropMethods.VirtualAllocEx(num2, IntPtr.Zero, 1024 /*0x0400*/, InteropValues.AllocationType.Commit, InteropValues.MemoryProtection.ReadWrite);
      InteropValues.TBBUTTON lpBuffer1 = new InteropValues.TBBUTTON();
      InteropValues.TRAYDATA lpBuffer2 = new InteropValues.TRAYDATA();
      int id = Process.GetCurrentProcess().Id;
      for (uint wParam = 0; (long) wParam < (long) num1; ++wParam)
      {
        int num4 = (int) InteropMethods.SendMessage(hTrayWnd, 1047U, wParam, num3);
        int lpNumberOfBytesRead;
        if (InteropMethods.ReadProcessMemory(num2, num3, out lpBuffer1, Marshal.SizeOf<InteropValues.TBBUTTON>(lpBuffer1), out lpNumberOfBytesRead))
        {
          if (lpBuffer1.dwData == IntPtr.Zero)
            lpBuffer1.dwData = lpBuffer1.iString;
          InteropMethods.ReadProcessMemory(num2, lpBuffer1.dwData, out lpBuffer2, Marshal.SizeOf<InteropValues.TRAYDATA>(lpBuffer2), out lpNumberOfBytesRead);
          uint lpdwProcessId2;
          int windowThreadProcessId2 = (int) InteropMethods.GetWindowThreadProcessId(lpBuffer2.hwnd, out lpdwProcessId2);
          if ((int) lpdwProcessId2 == id)
          {
            InteropValues.RECT lpBuffer3 = new InteropValues.RECT();
            IntPtr num5 = InteropMethods.VirtualAllocEx(num2, IntPtr.Zero, Marshal.SizeOf(typeof (Rect)), InteropValues.AllocationType.Commit, InteropValues.MemoryProtection.ReadWrite);
            int num6 = (int) InteropMethods.SendMessage(hTrayWnd, 1053U, wParam, num5);
            InteropMethods.ReadProcessMemory(num2, num5, out lpBuffer3, Marshal.SizeOf<InteropValues.RECT>(lpBuffer3), out lpNumberOfBytesRead);
            InteropMethods.VirtualFreeEx(num2, num5, Marshal.SizeOf<InteropValues.RECT>(lpBuffer3), InteropValues.FreeType.Decommit);
            InteropMethods.VirtualFreeEx(num2, num5, 0, InteropValues.FreeType.Release);
            int num7 = lpRect.Left + lpBuffer3.Left;
            int num8 = lpRect.Top + lpBuffer3.Top;
            int num9 = lpRect.Top + lpBuffer3.Bottom;
            int num10 = lpRect.Left + lpBuffer3.Right;
            rectNotifyList.Add(new InteropValues.RECT()
            {
              Left = num7,
              Right = num10,
              Top = num8,
              Bottom = num9
            });
            notifyIcon = true;
          }
        }
      }
      InteropMethods.VirtualFreeEx(num2, num3, 16534, InteropValues.FreeType.Decommit);
      InteropMethods.VirtualFreeEx(num2, num3, 0, InteropValues.FreeType.Release);
      InteropMethods.CloseHandle(num2);
    }
    return notifyIcon;
  }

  private void OnIconChanged()
  {
    if (this._windowClassName == null)
      return;
    IconHandle largeIconHandle;
    if (this._icon != null)
    {
      IconHelper.GetIconHandlesFromImageSource(this._icon, out largeIconHandle, out this._iconHandle);
      this._iconCurrentHandle = this._iconHandle.CriticalGetHandle();
    }
    else
    {
      if (this._iconDefaultHandle == IntPtr.Zero)
      {
        IconHelper.GetDefaultIconHandles(out largeIconHandle, out this._iconHandle);
        this._iconDefaultHandle = this._iconHandle.CriticalGetHandle();
      }
      this._iconCurrentHandle = this._iconDefaultHandle;
    }
  }

  private void UpdateIcon(bool showIconInTray, bool isTransparent = false)
  {
    lock (this._syncObj)
    {
      if (DesignerHelper.IsInDesignMode)
        return;
      this._isTransparent = isTransparent;
      InteropValues.NOTIFYICONDATA pnid = new InteropValues.NOTIFYICONDATA()
      {
        uCallbackMessage = 2048 /*0x0800*/,
        uFlags = 7,
        hWnd = this._messageWindowHandle,
        uID = this._id,
        dwInfoFlags = 4,
        hIcon = isTransparent ? IntPtr.Zero : this._iconCurrentHandle,
        szTip = this.Text
      };
      if (showIconInTray)
      {
        if (!this._added)
        {
          InteropMethods.Shell_NotifyIcon(0, pnid);
          this._added = true;
        }
        else
          InteropMethods.Shell_NotifyIcon(1, pnid);
      }
      else
      {
        if (!this._added)
          return;
        InteropMethods.Shell_NotifyIcon(2, pnid);
        this._added = false;
      }
    }
  }

  private void RegisterClass()
  {
    this._windowClassName = $"HandyControl.Controls.NotifyIcon{Guid.NewGuid()}";
    int num = (int) InteropMethods.RegisterClass(new InteropValues.WNDCLASS4ICON()
    {
      style = 0,
      lpfnWndProc = this._callback,
      cbClsExtra = 0,
      cbWndExtra = 0,
      hInstance = IntPtr.Zero,
      hIcon = IntPtr.Zero,
      hCursor = IntPtr.Zero,
      hbrBackground = IntPtr.Zero,
      lpszMenuName = "",
      lpszClassName = this._windowClassName
    });
    this._wmTaskbarCreated = InteropMethods.RegisterWindowMessage("TaskbarCreated");
    this._messageWindowHandle = InteropMethods.CreateWindowEx(0, this._windowClassName, "", 0, 0, 0, 1, 1, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
  }

  private IntPtr Callback(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
  {
    if (msg == this._wmTaskbarCreated)
    {
      if (this._messageWindowHandle == hWnd && this.Visibility == Visibility.Visible)
      {
        this.UpdateIcon(false);
        this.UpdateIcon(true);
      }
    }
    else
    {
      switch (lparam.ToInt64() - 512L /*0x0200*/)
      {
        case 0:
          if (!this._dispatcherTimerPos.IsEnabled)
          {
            this._dispatcherTimerPos.Interval = TimeSpan.FromMilliseconds(200.0);
            this._dispatcherTimerPos.Start();
            break;
          }
          break;
        case 2:
          this.WmMouseUp(MouseButton.Left);
          break;
        case 3:
          this.WmMouseDown(MouseButton.Left, 2);
          break;
        case 5:
          this.ShowContextMenu();
          this.WmMouseUp(MouseButton.Right);
          break;
      }
    }
    return InteropMethods.DefWindowProc(hWnd, msg, wparam, lparam);
  }

  private void WmMouseDown(MouseButton button, int clicks)
  {
    if (clicks != 2)
      return;
    MouseButtonEventArgs e = new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, button);
    e.RoutedEvent = NotifyIcon.MouseDoubleClickEvent;
    this.RaiseEvent((RoutedEventArgs) e);
    this._doubleClick = true;
  }

  private void WmMouseUp(MouseButton button)
  {
    if (!this._doubleClick && button == MouseButton.Left)
    {
      MouseButtonEventArgs e = new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, button);
      e.RoutedEvent = NotifyIcon.ClickEvent;
      this.RaiseEvent((RoutedEventArgs) e);
    }
    this._doubleClick = false;
  }

  private void ShowContextMenu()
  {
    if (this.ContextContent != null)
    {
      if (this._contextContent == null)
      {
        Popup popup = new Popup();
        popup.Placement = PlacementMode.Mouse;
        popup.AllowsTransparency = true;
        popup.StaysOpen = false;
        popup.UseLayoutRounding = true;
        popup.SnapsToDevicePixels = true;
        this._contextContent = popup;
      }
      this._contextContent.Child = (UIElement) new ContentControl()
      {
        Content = this.ContextContent
      };
      this._contextContent.IsOpen = true;
      InteropMethods.SetForegroundWindow(this._contextContent.Child.GetHandle());
    }
    else
    {
      if (this.ContextMenu == null || this.ContextMenu.Items.Count == 0)
        return;
      this.ContextMenu.InvalidateProperty(FrameworkElement.StyleProperty);
      foreach (object obj in (IEnumerable) this.ContextMenu.Items)
      {
        if (obj is MenuItem menuItem2)
          menuItem2.InvalidateProperty(FrameworkElement.StyleProperty);
        else if (this.ContextMenu.ItemContainerGenerator.ContainerFromItem(obj) is MenuItem menuItem1)
          menuItem1.InvalidateProperty(FrameworkElement.StyleProperty);
      }
      this.ContextMenu.Placement = PlacementMode.Mouse;
      this.ContextMenu.IsOpen = true;
      InteropMethods.SetForegroundWindow(this.ContextMenu.GetHandle());
    }
  }

  public event RoutedEventHandler Click
  {
    add => this.AddHandler(NotifyIcon.ClickEvent, (Delegate) value);
    remove => this.RemoveHandler(NotifyIcon.ClickEvent, (Delegate) value);
  }

  public event RoutedEventHandler MouseDoubleClick
  {
    add => this.AddHandler(NotifyIcon.MouseDoubleClickEvent, (Delegate) value);
    remove => this.RemoveHandler(NotifyIcon.MouseDoubleClickEvent, (Delegate) value);
  }

  private void UpdateDataContext(FrameworkElement target, object oldValue, object newValue)
  {
    if (target == null || BindingOperations.GetBindingExpression((DependencyObject) target, FrameworkElement.DataContextProperty) != null || this != target.DataContext && !object.Equals(oldValue, target.DataContext))
      return;
    target.DataContext = newValue ?? (object) this;
  }

  private void Dispose(bool disposing)
  {
    if (this._isDisposed)
      return;
    if (disposing)
    {
      if (this._dispatcherTimerBlink != null && this.IsBlink)
        this._dispatcherTimerBlink.Stop();
      this.UpdateIcon(false);
    }
    this._isDisposed = true;
  }
}
