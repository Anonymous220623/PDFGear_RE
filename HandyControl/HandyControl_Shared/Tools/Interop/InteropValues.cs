// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Interop.InteropValues
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;

#nullable disable
namespace HandyControl.Tools.Interop;

internal class InteropValues
{
  internal const int BITSPIXEL = 12;
  internal const int PLANES = 14;
  internal const int BI_RGB = 0;
  internal const int DIB_RGB_COLORS = 0;
  internal const int E_FAIL = -2147467259 /*0x80004005*/;
  internal const int HWND_TOP = 0;
  internal const int GWL_STYLE = -16;
  internal const int NIF_MESSAGE = 1;
  internal const int NIF_ICON = 2;
  internal const int NIF_TIP = 4;
  internal const int NIF_INFO = 16 /*0x10*/;
  internal const int NIM_ADD = 0;
  internal const int NIM_MODIFY = 1;
  internal const int NIM_DELETE = 2;
  internal const int NIIF_NONE = 0;
  internal const int NIIF_INFO = 1;
  internal const int NIIF_WARNING = 2;
  internal const int NIIF_ERROR = 3;
  internal const int WM_ACTIVATE = 6;
  internal const int WM_QUIT = 18;
  internal const int WM_GETMINMAXINFO = 36;
  internal const int WM_WINDOWPOSCHANGING = 70;
  internal const int WM_WINDOWPOSCHANGED = 71;
  internal const int WM_SETICON = 128 /*0x80*/;
  internal const int WM_NCCREATE = 129;
  internal const int WM_NCDESTROY = 130;
  internal const int WM_NCHITTEST = 132;
  internal const int WM_NCACTIVATE = 134;
  internal const int WM_NCRBUTTONDOWN = 164;
  internal const int WM_NCRBUTTONUP = 165;
  internal const int WM_NCRBUTTONDBLCLK = 166;
  internal const int WM_NCUAHDRAWCAPTION = 174;
  internal const int WM_NCUAHDRAWFRAME = 175;
  internal const int WM_KEYDOWN = 256 /*0x0100*/;
  internal const int WM_KEYUP = 257;
  internal const int WM_SYSKEYDOWN = 260;
  internal const int WM_SYSKEYUP = 261;
  internal const int WM_SYSCOMMAND = 274;
  internal const int WM_MOUSEMOVE = 512 /*0x0200*/;
  internal const int WM_LBUTTONUP = 514;
  internal const int WM_LBUTTONDBLCLK = 515;
  internal const int WM_RBUTTONUP = 517;
  internal const int WM_ENTERSIZEMOVE = 561;
  internal const int WM_EXITSIZEMOVE = 562;
  internal const int WM_CLIPBOARDUPDATE = 797;
  internal const int WM_USER = 1024 /*0x0400*/;
  internal const int WS_VISIBLE = 268435456 /*0x10000000*/;
  internal const int MF_BYCOMMAND = 0;
  internal const int MF_BYPOSITION = 1024 /*0x0400*/;
  internal const int MF_GRAYED = 1;
  internal const int MF_SEPARATOR = 2048 /*0x0800*/;
  internal const int TB_GETBUTTON = 1047;
  internal const int TB_BUTTONCOUNT = 1048;
  internal const int TB_GETITEMRECT = 1053;
  internal const int VERTRES = 10;
  internal const int DESKTOPVERTRES = 117;
  internal const int LOGPIXELSX = 88;
  internal const int LOGPIXELSY = 90;
  internal const int SC_CLOSE = 61536;
  internal const int SC_SIZE = 61440 /*0xF000*/;
  internal const int SC_MOVE = 61456;
  internal const int SC_MINIMIZE = 61472;
  internal const int SC_MAXIMIZE = 61488;
  internal const int SC_RESTORE = 61728;
  internal const int SRCCOPY = 13369376;
  internal const int MONITOR_DEFAULTTOPRIMARY = 1;
  internal const int MONITOR_DEFAULTTONEAREST = 2;

  internal static class ExternDll
  {
    public const string User32 = "user32.dll";
    public const string Gdi32 = "gdi32.dll";
    public const string GdiPlus = "gdiplus.dll";
    public const string Kernel32 = "kernel32.dll";
    public const string Shell32 = "shell32.dll";
    public const string MsImg = "msimg32.dll";
    public const string NTdll = "ntdll.dll";
    public const string DwmApi = "dwmapi.dll";
  }

  internal delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

  internal delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

  [return: MarshalAs(UnmanagedType.Bool)]
  internal delegate bool EnumMonitorsDelegate(
    IntPtr hMonitor,
    IntPtr hdcMonitor,
    ref InteropValues.RECT lprcMonitor,
    IntPtr dwData);

  internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  internal class NOTIFYICONDATA
  {
    public int cbSize = Marshal.SizeOf(typeof (InteropValues.NOTIFYICONDATA));
    public IntPtr hWnd;
    public int uID;
    public int uFlags;
    public int uCallbackMessage;
    public IntPtr hIcon;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128 /*0x80*/)]
    public string szTip = string.Empty;
    public int dwState = 1;
    public int dwStateMask = 1;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256 /*0x0100*/)]
    public string szInfo = string.Empty;
    public int uTimeoutOrVersion;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64 /*0x40*/)]
    public string szInfoTitle = string.Empty;
    public int dwInfoFlags;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  internal struct TBBUTTON
  {
    public int iBitmap;
    public int idCommand;
    public IntPtr fsStateStylePadding;
    public IntPtr dwData;
    public IntPtr iString;
  }

  [Flags]
  internal enum AllocationType
  {
    Commit = 4096, // 0x00001000
    Reserve = 8192, // 0x00002000
    Decommit = 16384, // 0x00004000
    Release = 32768, // 0x00008000
    Reset = 524288, // 0x00080000
    Physical = 4194304, // 0x00400000
    TopDown = 1048576, // 0x00100000
    WriteWatch = 2097152, // 0x00200000
    LargePages = 536870912, // 0x20000000
  }

  [Flags]
  internal enum MemoryProtection
  {
    Execute = 16, // 0x00000010
    ExecuteRead = 32, // 0x00000020
    ExecuteReadWrite = 64, // 0x00000040
    ExecuteWriteCopy = 128, // 0x00000080
    NoAccess = 1,
    ReadOnly = 2,
    ReadWrite = 4,
    WriteCopy = 8,
    GuardModifierflag = 256, // 0x00000100
    NoCacheModifierflag = 512, // 0x00000200
    WriteCombineModifierflag = 1024, // 0x00000400
  }

  internal struct TRAYDATA
  {
    public IntPtr hwnd;
    public uint uID;
    public uint uCallbackMessage;
    public uint bReserved0;
    public uint bReserved1;
    public IntPtr hIcon;
  }

  [Flags]
  internal enum FreeType
  {
    Decommit = 16384, // 0x00004000
    Release = 32768, // 0x00008000
  }

  internal struct POINT(int x, int y)
  {
    public int X = x;
    public int Y = y;
  }

  internal enum HookType
  {
    WH_KEYBOARD_LL = 13, // 0x0000000D
    WH_MOUSE_LL = 14, // 0x0000000E
  }

  internal struct MOUSEHOOKSTRUCT
  {
    public InteropValues.POINT pt;
    public IntPtr hwnd;
    public uint wHitTestCode;
    public IntPtr dwExtraInfo;
  }

  [Flags]
  internal enum ProcessAccess
  {
    AllAccess = 1050235, // 0x0010067B
    CreateThread = 2,
    DuplicateHandle = 64, // 0x00000040
    QueryInformation = 1024, // 0x00000400
    SetInformation = 512, // 0x00000200
    Terminate = 1,
    VMOperation = 8,
    VMRead = 16, // 0x00000010
    VMWrite = 32, // 0x00000020
    Synchronize = 1048576, // 0x00100000
  }

  [Serializable]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  internal struct RECT
  {
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;

    public RECT(int left, int top, int right, int bottom)
    {
      this.Left = left;
      this.Top = top;
      this.Right = right;
      this.Bottom = bottom;
    }

    public RECT(Rect rect)
    {
      this.Left = (int) rect.Left;
      this.Top = (int) rect.Top;
      this.Right = (int) rect.Right;
      this.Bottom = (int) rect.Bottom;
    }

    public Point Position => new Point((double) this.Left, (double) this.Top);

    public Size Size => new Size((double) this.Width, (double) this.Height);

    public int Height
    {
      get => this.Bottom - this.Top;
      set => this.Bottom = this.Top + value;
    }

    public int Width
    {
      get => this.Right - this.Left;
      set => this.Right = this.Left + value;
    }

    public bool Equals(InteropValues.RECT other)
    {
      return this.Left == other.Left && this.Right == other.Right && this.Top == other.Top && this.Bottom == other.Bottom;
    }

    public override bool Equals(object obj)
    {
      return obj is InteropValues.RECT other && this.Equals(other);
    }

    public static bool operator ==(InteropValues.RECT left, InteropValues.RECT right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(InteropValues.RECT left, InteropValues.RECT right)
    {
      return !(left == right);
    }

    public override int GetHashCode()
    {
      return ((this.Left * 397 ^ this.Top) * 397 ^ this.Right) * 397 ^ this.Bottom;
    }
  }

  internal struct BLENDFUNCTION
  {
    public byte BlendOp;
    public byte BlendFlags;
    public byte SourceConstantAlpha;
    public byte AlphaFormat;
  }

  internal enum GWL
  {
    EXSTYLE = -20, // 0xFFFFFFEC
    STYLE = -16, // 0xFFFFFFF0
  }

  internal enum GWLP
  {
    USERDATA = -21, // 0xFFFFFFEB
    ID = -12, // 0xFFFFFFF4
    HWNDPARENT = -8, // 0xFFFFFFF8
    HINSTANCE = -6, // 0xFFFFFFFA
    WNDPROC = -4, // 0xFFFFFFFC
  }

  internal struct BITMAPINFOHEADER
  {
    internal uint biSize;
    internal int biWidth;
    internal int biHeight;
    internal ushort biPlanes;
    internal ushort biBitCount;
    internal uint biCompression;
    internal uint biSizeImage;
    internal int biXPelsPerMeter;
    internal int biYPelsPerMeter;
    internal uint biClrUsed;
    internal uint biClrImportant;
  }

  [Flags]
  internal enum RedrawWindowFlags : uint
  {
    Invalidate = 1,
    InternalPaint = 2,
    Erase = 4,
    Validate = 8,
    NoInternalPaint = 16, // 0x00000010
    NoErase = 32, // 0x00000020
    NoChildren = 64, // 0x00000040
    AllChildren = 128, // 0x00000080
    UpdateNow = 256, // 0x00000100
    EraseNow = 512, // 0x00000200
    Frame = 1024, // 0x00000400
    NoFrame = 2048, // 0x00000800
  }

  [StructLayout(LayoutKind.Sequential)]
  internal class WINDOWPOS
  {
    public IntPtr hwnd;
    public IntPtr hwndInsertAfter;
    public int x;
    public int y;
    public int cx;
    public int cy;
    public uint flags;
  }

  internal struct WINDOWPLACEMENT
  {
    public int length;
    public int flags;
    public InteropValues.SW showCmd;
    public InteropValues.POINT ptMinPosition;
    public InteropValues.POINT ptMaxPosition;
    public InteropValues.RECT rcNormalPosition;

    public static InteropValues.WINDOWPLACEMENT Default
    {
      get
      {
        return new InteropValues.WINDOWPLACEMENT()
        {
          length = Marshal.SizeOf(typeof (InteropValues.WINDOWPLACEMENT))
        };
      }
    }
  }

  [StructLayout(LayoutKind.Sequential, Pack = 4)]
  internal struct SIZE
  {
    [ComAliasName("Microsoft.VisualStudio.OLE.Interop.LONG")]
    public int cx;
    [ComAliasName("Microsoft.VisualStudio.OLE.Interop.LONG")]
    public int cy;
  }

  internal struct MONITORINFO
  {
    public uint cbSize;
    public InteropValues.RECT rcMonitor;
    public InteropValues.RECT rcWork;
    public uint dwFlags;
  }

  internal enum SM
  {
    CXSCREEN = 0,
    CYSCREEN = 1,
    CXVSCROLL = 2,
    CYHSCROLL = 3,
    CYCAPTION = 4,
    CXBORDER = 5,
    CYBORDER = 6,
    CXFIXEDFRAME = 7,
    CYFIXEDFRAME = 8,
    CYVTHUMB = 9,
    CXHTHUMB = 10, // 0x0000000A
    CXICON = 11, // 0x0000000B
    CYICON = 12, // 0x0000000C
    CXCURSOR = 13, // 0x0000000D
    CYCURSOR = 14, // 0x0000000E
    CYMENU = 15, // 0x0000000F
    CXFULLSCREEN = 16, // 0x00000010
    CYFULLSCREEN = 17, // 0x00000011
    CYKANJIWINDOW = 18, // 0x00000012
    MOUSEPRESENT = 19, // 0x00000013
    CYVSCROLL = 20, // 0x00000014
    CXHSCROLL = 21, // 0x00000015
    DEBUG = 22, // 0x00000016
    SWAPBUTTON = 23, // 0x00000017
    CXMIN = 28, // 0x0000001C
    CYMIN = 29, // 0x0000001D
    CXSIZE = 30, // 0x0000001E
    CYSIZE = 31, // 0x0000001F
    CXFRAME = 32, // 0x00000020
    CXSIZEFRAME = 32, // 0x00000020
    CYFRAME = 33, // 0x00000021
    CYSIZEFRAME = 33, // 0x00000021
    CXMINTRACK = 34, // 0x00000022
    CYMINTRACK = 35, // 0x00000023
    CXDOUBLECLK = 36, // 0x00000024
    CYDOUBLECLK = 37, // 0x00000025
    CXICONSPACING = 38, // 0x00000026
    CYICONSPACING = 39, // 0x00000027
    MENUDROPALIGNMENT = 40, // 0x00000028
    PENWINDOWS = 41, // 0x00000029
    DBCSENABLED = 42, // 0x0000002A
    CMOUSEBUTTONS = 43, // 0x0000002B
    SECURE = 44, // 0x0000002C
    CXEDGE = 45, // 0x0000002D
    CYEDGE = 46, // 0x0000002E
    CXMINSPACING = 47, // 0x0000002F
    CYMINSPACING = 48, // 0x00000030
    CXSMICON = 49, // 0x00000031
    CYSMICON = 50, // 0x00000032
    CYSMCAPTION = 51, // 0x00000033
    CXSMSIZE = 52, // 0x00000034
    CYSMSIZE = 53, // 0x00000035
    CXMENUSIZE = 54, // 0x00000036
    CYMENUSIZE = 55, // 0x00000037
    ARRANGE = 56, // 0x00000038
    CXMINIMIZED = 57, // 0x00000039
    CYMINIMIZED = 58, // 0x0000003A
    CXMAXTRACK = 59, // 0x0000003B
    CYMAXTRACK = 60, // 0x0000003C
    CXMAXIMIZED = 61, // 0x0000003D
    CYMAXIMIZED = 62, // 0x0000003E
    NETWORK = 63, // 0x0000003F
    CLEANBOOT = 67, // 0x00000043
    CXDRAG = 68, // 0x00000044
    CYDRAG = 69, // 0x00000045
    SHOWSOUNDS = 70, // 0x00000046
    CXMENUCHECK = 71, // 0x00000047
    CYMENUCHECK = 72, // 0x00000048
    SLOWMACHINE = 73, // 0x00000049
    MIDEASTENABLED = 74, // 0x0000004A
    MOUSEWHEELPRESENT = 75, // 0x0000004B
    XVIRTUALSCREEN = 76, // 0x0000004C
    YVIRTUALSCREEN = 77, // 0x0000004D
    CXVIRTUALSCREEN = 78, // 0x0000004E
    CYVIRTUALSCREEN = 79, // 0x0000004F
    CMONITORS = 80, // 0x00000050
    SAMEDISPLAYFORMAT = 81, // 0x00000051
    IMMENABLED = 82, // 0x00000052
    CXFOCUSBORDER = 83, // 0x00000053
    CYFOCUSBORDER = 84, // 0x00000054
    TABLETPC = 86, // 0x00000056
    MEDIACENTER = 87, // 0x00000057
    REMOTESESSION = 4096, // 0x00001000
    REMOTECONTROL = 8193, // 0x00002001
  }

  internal enum CacheSlot
  {
    DpiX,
    FocusBorderWidth,
    FocusBorderHeight,
    HighContrast,
    MouseVanish,
    DropShadow,
    FlatMenu,
    WorkAreaInternal,
    WorkArea,
    IconMetrics,
    KeyboardCues,
    KeyboardDelay,
    KeyboardPreference,
    KeyboardSpeed,
    SnapToDefaultButton,
    WheelScrollLines,
    MouseHoverTime,
    MouseHoverHeight,
    MouseHoverWidth,
    MenuDropAlignment,
    MenuFade,
    MenuShowDelay,
    ComboBoxAnimation,
    ClientAreaAnimation,
    CursorShadow,
    GradientCaptions,
    HotTracking,
    ListBoxSmoothScrolling,
    MenuAnimation,
    SelectionFade,
    StylusHotTracking,
    ToolTipAnimation,
    ToolTipFade,
    UIEffects,
    MinimizeAnimation,
    Border,
    CaretWidth,
    ForegroundFlashCount,
    DragFullWindows,
    NonClientMetrics,
    ThinHorizontalBorderHeight,
    ThinVerticalBorderWidth,
    CursorWidth,
    CursorHeight,
    ThickHorizontalBorderHeight,
    ThickVerticalBorderWidth,
    MinimumHorizontalDragDistance,
    MinimumVerticalDragDistance,
    FixedFrameHorizontalBorderHeight,
    FixedFrameVerticalBorderWidth,
    FocusHorizontalBorderHeight,
    FocusVerticalBorderWidth,
    FullPrimaryScreenWidth,
    FullPrimaryScreenHeight,
    HorizontalScrollBarButtonWidth,
    HorizontalScrollBarHeight,
    HorizontalScrollBarThumbWidth,
    IconWidth,
    IconHeight,
    IconGridWidth,
    IconGridHeight,
    MaximizedPrimaryScreenWidth,
    MaximizedPrimaryScreenHeight,
    MaximumWindowTrackWidth,
    MaximumWindowTrackHeight,
    MenuCheckmarkWidth,
    MenuCheckmarkHeight,
    MenuButtonWidth,
    MenuButtonHeight,
    MinimumWindowWidth,
    MinimumWindowHeight,
    MinimizedWindowWidth,
    MinimizedWindowHeight,
    MinimizedGridWidth,
    MinimizedGridHeight,
    MinimumWindowTrackWidth,
    MinimumWindowTrackHeight,
    PrimaryScreenWidth,
    PrimaryScreenHeight,
    WindowCaptionButtonWidth,
    WindowCaptionButtonHeight,
    ResizeFrameHorizontalBorderHeight,
    ResizeFrameVerticalBorderWidth,
    SmallIconWidth,
    SmallIconHeight,
    SmallWindowCaptionButtonWidth,
    SmallWindowCaptionButtonHeight,
    VirtualScreenWidth,
    VirtualScreenHeight,
    VerticalScrollBarWidth,
    VerticalScrollBarButtonHeight,
    WindowCaptionHeight,
    KanjiWindowHeight,
    MenuBarHeight,
    VerticalScrollBarThumbHeight,
    IsImmEnabled,
    IsMediaCenter,
    IsMenuDropRightAligned,
    IsMiddleEastEnabled,
    IsMousePresent,
    IsMouseWheelPresent,
    IsPenWindows,
    IsRemotelyControlled,
    IsRemoteSession,
    ShowSounds,
    IsSlowMachine,
    SwapButtons,
    IsTabletPC,
    VirtualScreenLeft,
    VirtualScreenTop,
    PowerLineStatus,
    IsGlassEnabled,
    UxThemeName,
    UxThemeColor,
    WindowCornerRadius,
    WindowGlassColor,
    WindowGlassBrush,
    WindowNonClientFrameThickness,
    WindowResizeBorderThickness,
    NumSlots,
  }

  internal static class Win32Constant
  {
    internal const int MAX_PATH = 260;
    internal const int INFOTIPSIZE = 1024 /*0x0400*/;
    internal const int TRUE = 1;
    internal const int FALSE = 0;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  internal struct WNDCLASS
  {
    public uint style;
    public Delegate lpfnWndProc;
    public int cbClsExtra;
    public int cbWndExtra;
    public IntPtr hInstance;
    public IntPtr hIcon;
    public IntPtr hCursor;
    public IntPtr hbrBackground;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string lpszMenuName;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string lpszClassName;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  internal class WNDCLASS4ICON
  {
    public int style;
    public InteropValues.WndProc lpfnWndProc;
    public int cbClsExtra;
    public int cbWndExtra;
    public IntPtr hInstance;
    public IntPtr hIcon;
    public IntPtr hCursor;
    public IntPtr hbrBackground;
    public string lpszMenuName;
    public string lpszClassName;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 2)]
  internal struct BITMAPINFO(int width, int height, short bpp)
  {
    public int biSize = InteropValues.BITMAPINFO.SizeOf();
    public int biWidth = width;
    public int biHeight = height;
    public short biPlanes = 1;
    public short biBitCount = bpp;
    public int biCompression = 0;
    public int biSizeImage = 0;
    public int biXPelsPerMeter = 0;
    public int biYPelsPerMeter = 0;
    public int biClrUsed = 0;
    public int biClrImportant = 0;

    [SecuritySafeCritical]
    private static int SizeOf() => Marshal.SizeOf(typeof (InteropValues.BITMAPINFO));
  }

  [StructLayout(LayoutKind.Sequential)]
  internal class ICONINFO
  {
    public bool fIcon;
    public int xHotspot;
    public int yHotspot;
    public BitmapHandle hbmMask;
    public BitmapHandle hbmColor;
  }

  internal enum WINDOWCOMPOSITIONATTRIB
  {
    WCA_ACCENT_POLICY = 19, // 0x00000013
  }

  internal struct WINCOMPATTRDATA
  {
    public InteropValues.WINDOWCOMPOSITIONATTRIB Attribute;
    public IntPtr Data;
    public int DataSize;
  }

  internal enum ACCENTSTATE
  {
    ACCENT_DISABLED,
    ACCENT_ENABLE_GRADIENT,
    ACCENT_ENABLE_TRANSPARENTGRADIENT,
    ACCENT_ENABLE_BLURBEHIND,
    ACCENT_ENABLE_ACRYLICBLURBEHIND,
    ACCENT_INVALID_STATE,
  }

  internal struct ACCENTPOLICY
  {
    public InteropValues.ACCENTSTATE AccentState;
    public int AccentFlags;
    public uint GradientColor;
    public int AnimationId;
  }

  [Guid("0000000C-0000-0000-C000-000000000046")]
  [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  [ComImport]
  internal interface IStream
  {
    int Read([In] IntPtr buf, [In] int len);

    int Write([In] IntPtr buf, [In] int len);

    [return: MarshalAs(UnmanagedType.I8)]
    long Seek([MarshalAs(UnmanagedType.I8), In] long dlibMove, [In] int dwOrigin);

    void SetSize([MarshalAs(UnmanagedType.I8), In] long libNewSize);

    [return: MarshalAs(UnmanagedType.I8)]
    long CopyTo([MarshalAs(UnmanagedType.Interface), In] InteropValues.IStream pstm, [MarshalAs(UnmanagedType.I8), In] long cb, [MarshalAs(UnmanagedType.LPArray), Out] long[] pcbRead);

    void Commit([In] int grfCommitFlags);

    void Revert();

    void LockRegion([MarshalAs(UnmanagedType.I8), In] long libOffset, [MarshalAs(UnmanagedType.I8), In] long cb, [In] int dwLockType);

    void UnlockRegion([MarshalAs(UnmanagedType.I8), In] long libOffset, [MarshalAs(UnmanagedType.I8), In] long cb, [In] int dwLockType);

    void Stat([In] IntPtr pStatstg, [In] int grfStatFlag);

    [return: MarshalAs(UnmanagedType.Interface)]
    InteropValues.IStream Clone();
  }

  internal class StreamConsts
  {
    public const int LOCK_WRITE = 1;
    public const int LOCK_EXCLUSIVE = 2;
    public const int LOCK_ONLYONCE = 4;
    public const int STATFLAG_DEFAULT = 0;
    public const int STATFLAG_NONAME = 1;
    public const int STATFLAG_NOOPEN = 2;
    public const int STGC_DEFAULT = 0;
    public const int STGC_OVERWRITE = 1;
    public const int STGC_ONLYIFCURRENT = 2;
    public const int STGC_DANGEROUSLYCOMMITMERELYTODISKCACHE = 4;
    public const int STREAM_SEEK_SET = 0;
    public const int STREAM_SEEK_CUR = 1;
    public const int STREAM_SEEK_END = 2;
  }

  [StructLayout(LayoutKind.Sequential, Pack = 8)]
  internal class ImageCodecInfoPrivate
  {
    [MarshalAs(UnmanagedType.Struct)]
    public Guid Clsid;
    [MarshalAs(UnmanagedType.Struct)]
    public Guid FormatID;
    public IntPtr CodecName = IntPtr.Zero;
    public IntPtr DllName = IntPtr.Zero;
    public IntPtr FormatDescription = IntPtr.Zero;
    public IntPtr FilenameExtension = IntPtr.Zero;
    public IntPtr MimeType = IntPtr.Zero;
    public int Flags;
    public int Version;
    public int SigCount;
    public int SigSize;
    public IntPtr SigPattern = IntPtr.Zero;
    public IntPtr SigMask = IntPtr.Zero;
  }

  internal class ComStreamFromDataStream : InteropValues.IStream
  {
    protected Stream DataStream;
    private long _virtualPosition = -1;

    internal ComStreamFromDataStream(Stream dataStream)
    {
      this.DataStream = dataStream ?? throw new ArgumentNullException(nameof (dataStream));
    }

    private void ActualizeVirtualPosition()
    {
      if (this._virtualPosition == -1L)
        return;
      if (this._virtualPosition > this.DataStream.Length)
        this.DataStream.SetLength(this._virtualPosition);
      this.DataStream.Position = this._virtualPosition;
      this._virtualPosition = -1L;
    }

    public virtual InteropValues.IStream Clone()
    {
      InteropValues.ComStreamFromDataStream.NotImplemented();
      return (InteropValues.IStream) null;
    }

    public virtual void Commit(int grfCommitFlags)
    {
      this.DataStream.Flush();
      this.ActualizeVirtualPosition();
    }

    public virtual long CopyTo(InteropValues.IStream pstm, long cb, long[] pcbRead)
    {
      IntPtr num1 = Marshal.AllocHGlobal(4096 /*0x1000*/);
      if (num1 == IntPtr.Zero)
        throw new OutOfMemoryException();
      long num2 = 0;
      try
      {
        int len;
        for (; num2 < cb; num2 += (long) len)
        {
          int length = 4096 /*0x1000*/;
          if (num2 + (long) length > cb)
            length = (int) (cb - num2);
          len = this.Read(num1, length);
          if (len != 0)
          {
            if (pstm.Write(num1, len) != len)
              throw InteropValues.ComStreamFromDataStream.EFail("Wrote an incorrect number of bytes");
          }
          else
            break;
        }
      }
      finally
      {
        Marshal.FreeHGlobal(num1);
      }
      if (pcbRead != null && pcbRead.Length != 0)
        pcbRead[0] = num2;
      return num2;
    }

    public virtual Stream GetDataStream() => this.DataStream;

    public virtual void LockRegion(long libOffset, long cb, int dwLockType)
    {
    }

    protected static ExternalException EFail(string msg)
    {
      throw new ExternalException(msg, -2147467259 /*0x80004005*/);
    }

    protected static void NotImplemented() => throw new NotImplementedException();

    public virtual int Read(IntPtr buf, int length)
    {
      byte[] numArray = new byte[length];
      int num = this.Read(numArray, length);
      Marshal.Copy(numArray, 0, buf, length);
      return num;
    }

    public virtual int Read(byte[] buffer, int length)
    {
      this.ActualizeVirtualPosition();
      return this.DataStream.Read(buffer, 0, length);
    }

    public virtual void Revert() => InteropValues.ComStreamFromDataStream.NotImplemented();

    public virtual long Seek(long offset, int origin)
    {
      long num = this._virtualPosition;
      if (this._virtualPosition == -1L)
        num = this.DataStream.Position;
      long length = this.DataStream.Length;
      switch (origin)
      {
        case 0:
          if (offset <= length)
          {
            this.DataStream.Position = offset;
            this._virtualPosition = -1L;
            break;
          }
          this._virtualPosition = offset;
          break;
        case 1:
          if (offset + num <= length)
          {
            this.DataStream.Position = num + offset;
            this._virtualPosition = -1L;
            break;
          }
          this._virtualPosition = offset + num;
          break;
        case 2:
          if (offset <= 0L)
          {
            this.DataStream.Position = length + offset;
            this._virtualPosition = -1L;
            break;
          }
          this._virtualPosition = length + offset;
          break;
      }
      return this._virtualPosition == -1L ? this.DataStream.Position : this._virtualPosition;
    }

    public virtual void SetSize(long value) => this.DataStream.SetLength(value);

    public virtual void Stat(IntPtr pstatstg, int grfStatFlag)
    {
      InteropValues.ComStreamFromDataStream.NotImplemented();
    }

    public virtual void UnlockRegion(long libOffset, long cb, int dwLockType)
    {
    }

    public virtual int Write(IntPtr buf, int length)
    {
      byte[] numArray = new byte[length];
      Marshal.Copy(buf, numArray, 0, length);
      return this.Write(numArray, length);
    }

    public virtual int Write(byte[] buffer, int length)
    {
      this.ActualizeVirtualPosition();
      this.DataStream.Write(buffer, 0, length);
      return length;
    }
  }

  [StructLayout(LayoutKind.Sequential)]
  internal class MINMAXINFO
  {
    public InteropValues.POINT ptReserved;
    public InteropValues.POINT ptMaxSize;
    public InteropValues.POINT ptMaxPosition;
    public InteropValues.POINT ptMinTrackSize;
    public InteropValues.POINT ptMaxTrackSize;
  }

  internal struct APPBARDATA
  {
    public int cbSize;
    public IntPtr hWnd;
    public uint uCallbackMessage;
    public uint uEdge;
    public InteropValues.RECT rc;
    public int lParam;
  }

  internal struct RTL_OSVERSIONINFOEX
  {
    internal uint dwOSVersionInfoSize;
    internal uint dwMajorVersion;
    internal uint dwMinorVersion;
    internal uint dwBuildNumber;
    internal uint dwPlatformId;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128 /*0x80*/)]
    internal string szCSDVersion;
  }

  [Flags]
  internal enum WindowPositionFlags
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

  internal struct WindowPosition
  {
    public IntPtr Hwnd;
    public IntPtr HwndZOrderInsertAfter;
    public int X;
    public int Y;
    public int Width;
    public int Height;
    public InteropValues.WindowPositionFlags Flags;
  }

  [Flags]
  internal enum DwmWindowAttribute : uint
  {
    DWMWA_NCRENDERING_ENABLED = 1,
    DWMWA_NCRENDERING_POLICY = 2,
    DWMWA_TRANSITIONS_FORCEDISABLED = DWMWA_NCRENDERING_POLICY | DWMWA_NCRENDERING_ENABLED, // 0x00000003
    DWMWA_ALLOW_NCPAINT = 4,
    DWMWA_CAPTION_BUTTON_BOUNDS = DWMWA_ALLOW_NCPAINT | DWMWA_NCRENDERING_ENABLED, // 0x00000005
    DWMWA_NONCLIENT_RTL_LAYOUT = DWMWA_ALLOW_NCPAINT | DWMWA_NCRENDERING_POLICY, // 0x00000006
    DWMWA_FORCE_ICONIC_REPRESENTATION = DWMWA_NONCLIENT_RTL_LAYOUT | DWMWA_NCRENDERING_ENABLED, // 0x00000007
    DWMWA_FLIP3D_POLICY = 8,
    DWMWA_EXTENDED_FRAME_BOUNDS = DWMWA_FLIP3D_POLICY | DWMWA_NCRENDERING_ENABLED, // 0x00000009
    DWMWA_HAS_ICONIC_BITMAP = DWMWA_FLIP3D_POLICY | DWMWA_NCRENDERING_POLICY, // 0x0000000A
    DWMWA_DISALLOW_PEEK = DWMWA_HAS_ICONIC_BITMAP | DWMWA_NCRENDERING_ENABLED, // 0x0000000B
    DWMWA_EXCLUDED_FROM_PEEK = DWMWA_FLIP3D_POLICY | DWMWA_ALLOW_NCPAINT, // 0x0000000C
    DWMWA_LAST = DWMWA_EXCLUDED_FROM_PEEK | DWMWA_NCRENDERING_ENABLED, // 0x0000000D
  }

  [Flags]
  internal enum WindowStyles
  {
    WS_MAXIMIZE = 16777216, // 0x01000000
    WS_MAXIMIZEBOX = 65536, // 0x00010000
    WS_MINIMIZE = 536870912, // 0x20000000
    WS_THICKFRAME = 262144, // 0x00040000
  }

  internal enum SW
  {
    HIDE = 0,
    NORMAL = 1,
    SHOWNORMAL = 1,
    SHOWMINIMIZED = 2,
    MAXIMIZE = 3,
    SHOWMAXIMIZED = 3,
    SHOWNOACTIVATE = 4,
    SHOW = 5,
    MINIMIZE = 6,
    SHOWMINNOACTIVE = 7,
    SHOWNA = 8,
    RESTORE = 9,
    SHOWDEFAULT = 10, // 0x0000000A
    FORCEMINIMIZE = 11, // 0x0000000B
  }
}
