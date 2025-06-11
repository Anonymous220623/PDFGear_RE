// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.GlowEdge
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Controls;
using HandyControl.Tools.Interop;
using System;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Data;

internal class GlowEdge : HwndWrapper
{
  private const string GlowEdgeClassName = "HandyControlGlowEdge";
  private const int GlowDepth = 9;
  private const int CornerGripThickness = 18;
  private static ushort _sharedWindowClassAtom;
  private static InteropValues.WndProc _sharedWndProc;
  private readonly GlowBitmap[] _activeGlowBitmaps = new GlowBitmap[16 /*0x10*/];
  private readonly GlowBitmap[] _inactiveGlowBitmaps = new GlowBitmap[16 /*0x10*/];
  private readonly Dock _orientation;
  private readonly GlowWindow _targetWindow;
  private Color _activeGlowColor = Colors.Transparent;
  private int _height;
  private Color _inactiveGlowColor = Colors.Transparent;
  private GlowEdge.FieldInvalidationTypes _invalidatedValues;
  private bool _isActive;
  private bool _isVisible;
  private int _left;
  private bool _pendingDelayRender;
  private int _top;
  private int _width;

  internal static long CreatedGlowEdges { get; private set; }

  internal static long DisposedGlowEdges { get; private set; }

  internal GlowEdge(GlowWindow owner, Dock orientation)
  {
    this._targetWindow = owner ?? throw new ArgumentNullException(nameof (owner));
    this._orientation = orientation;
    ++GlowEdge.CreatedGlowEdges;
  }

  private bool IsDeferringChanges => this._targetWindow.DeferGlowChangesCount > 0;

  private static ushort SharedWindowClassAtom
  {
    get
    {
      if (GlowEdge._sharedWindowClassAtom == (ushort) 0)
        GlowEdge._sharedWindowClassAtom = InteropMethods.RegisterClass(ref new InteropValues.WNDCLASS()
        {
          cbClsExtra = 0,
          cbWndExtra = 0,
          hbrBackground = IntPtr.Zero,
          hCursor = IntPtr.Zero,
          hIcon = IntPtr.Zero,
          lpfnWndProc = (Delegate) (GlowEdge._sharedWndProc = GlowEdge.\u003C\u003EO.\u003C0\u003E__DefWindowProc ?? (GlowEdge.\u003C\u003EO.\u003C0\u003E__DefWindowProc = new InteropValues.WndProc(InteropMethods.DefWindowProc))),
          lpszClassName = "HandyControlGlowEdge",
          lpszMenuName = (string) null,
          style = 0U
        });
      return GlowEdge._sharedWindowClassAtom;
    }
  }

  internal bool IsVisible
  {
    get => this._isVisible;
    set
    {
      this.UpdateProperty<bool>(ref this._isVisible, value, GlowEdge.FieldInvalidationTypes.Render | GlowEdge.FieldInvalidationTypes.Visibility);
    }
  }

  internal int Left
  {
    get => this._left;
    set
    {
      this.UpdateProperty<int>(ref this._left, value, GlowEdge.FieldInvalidationTypes.Location);
    }
  }

  internal int Top
  {
    get => this._top;
    set => this.UpdateProperty<int>(ref this._top, value, GlowEdge.FieldInvalidationTypes.Location);
  }

  internal int Width
  {
    get => this._width;
    set
    {
      this.UpdateProperty<int>(ref this._width, value, GlowEdge.FieldInvalidationTypes.Size | GlowEdge.FieldInvalidationTypes.Render);
    }
  }

  internal int Height
  {
    get => this._height;
    set
    {
      this.UpdateProperty<int>(ref this._height, value, GlowEdge.FieldInvalidationTypes.Size | GlowEdge.FieldInvalidationTypes.Render);
    }
  }

  internal bool IsActive
  {
    get => this._isActive;
    set
    {
      this.UpdateProperty<bool>(ref this._isActive, value, GlowEdge.FieldInvalidationTypes.Render);
    }
  }

  internal Color ActiveGlowColor
  {
    get => this._activeGlowColor;
    set
    {
      this.UpdateProperty<Color>(ref this._activeGlowColor, value, GlowEdge.FieldInvalidationTypes.ActiveColor | GlowEdge.FieldInvalidationTypes.Render);
    }
  }

  internal Color InactiveGlowColor
  {
    get => this._inactiveGlowColor;
    set
    {
      this.UpdateProperty<Color>(ref this._inactiveGlowColor, value, GlowEdge.FieldInvalidationTypes.InactiveColor | GlowEdge.FieldInvalidationTypes.Render);
    }
  }

  private IntPtr TargetWindowHandle => new WindowInteropHelper((System.Windows.Window) this._targetWindow).Handle;

  protected override bool IsWindowSubclassed => true;

  private bool IsPositionValid
  {
    get
    {
      return (this._invalidatedValues & (GlowEdge.FieldInvalidationTypes.Location | GlowEdge.FieldInvalidationTypes.Size | GlowEdge.FieldInvalidationTypes.Visibility)) == GlowEdge.FieldInvalidationTypes.None;
    }
  }

  private void UpdateProperty<T>(
    ref T field,
    T value,
    GlowEdge.FieldInvalidationTypes invalidatedValues)
    where T : struct
  {
    if (field.Equals((object) value))
      return;
    field = value;
    this._invalidatedValues |= invalidatedValues;
    if (this.IsDeferringChanges)
      return;
    this.CommitChanges();
  }

  protected override ushort CreateWindowClassCore() => GlowEdge.SharedWindowClassAtom;

  protected override void DestroyWindowClassCore()
  {
  }

  protected override IntPtr CreateWindowCore()
  {
    return InteropMethods.CreateWindowEx(524416 /*0x080080*/, new IntPtr((int) this.WindowClassAtom), string.Empty, -2046820352 /*0x86000000*/, 0, 0, 0, 0, new WindowInteropHelper((System.Windows.Window) this._targetWindow).Owner, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
  }

  internal void ChangeOwner(IntPtr newOwner)
  {
    InteropMethods.SetWindowLongPtr(this.Handle, InteropValues.GWLP.HWNDPARENT, newOwner);
  }

  protected override IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
  {
    switch (msg)
    {
      case 6:
        return IntPtr.Zero;
      case 70:
        InteropValues.WINDOWPOS structure = (InteropValues.WINDOWPOS) Marshal.PtrToStructure(lParam, typeof (InteropValues.WINDOWPOS));
        structure.flags |= 16U /*0x10*/;
        Marshal.StructureToPtr<InteropValues.WINDOWPOS>(structure, lParam, true);
        break;
      case 126:
        if (this.IsVisible)
        {
          this.RenderLayeredWindow();
          break;
        }
        break;
      case 132:
        return new IntPtr(this.WmNcHitTest(lParam));
      case 161:
      case 163:
      case 164:
      case 166:
      case 167:
      case 169:
      case 171:
      case 173:
        IntPtr targetWindowHandle = this.TargetWindowHandle;
        InteropMethods.SendMessage(targetWindowHandle, 6, new IntPtr(2), IntPtr.Zero);
        InteropMethods.SendMessage(targetWindowHandle, msg, wParam, IntPtr.Zero);
        return IntPtr.Zero;
    }
    return base.WndProc(hwnd, msg, wParam, lParam);
  }

  private int WmNcHitTest(IntPtr lParam)
  {
    int xlParam = InteropMethods.GetXLParam(lParam.ToInt32());
    int ylParam = InteropMethods.GetYLParam(lParam.ToInt32());
    InteropValues.RECT lpRect;
    InteropMethods.GetWindowRect(this.Handle, out lpRect);
    switch (this._orientation)
    {
      case Dock.Left:
        if (ylParam - 18 < lpRect.Top)
          return 13;
        return ylParam + 18 > lpRect.Bottom ? 16 /*0x10*/ : 10;
      case Dock.Top:
        if (xlParam - 18 < lpRect.Left)
          return 13;
        return xlParam + 18 > lpRect.Right ? 14 : 12;
      case Dock.Right:
        if (ylParam - 18 < lpRect.Top)
          return 14;
        return ylParam + 18 > lpRect.Bottom ? 17 : 11;
      default:
        if (xlParam - 18 < lpRect.Left)
          return 16 /*0x10*/;
        return xlParam + 18 > lpRect.Right ? 17 : 15;
    }
  }

  internal void CommitChanges()
  {
    this.InvalidateCachedBitmaps();
    this.UpdateWindowPosCore();
    this.UpdateLayeredWindowCore();
    this._invalidatedValues = GlowEdge.FieldInvalidationTypes.None;
  }

  private void InvalidateCachedBitmaps()
  {
    if (this._invalidatedValues.HasFlag((Enum) GlowEdge.FieldInvalidationTypes.ActiveColor))
      this.ClearCache(this._activeGlowBitmaps);
    if (!this._invalidatedValues.HasFlag((Enum) GlowEdge.FieldInvalidationTypes.InactiveColor))
      return;
    this.ClearCache(this._inactiveGlowBitmaps);
  }

  private void UpdateWindowPosCore()
  {
    if (!this._invalidatedValues.HasFlag((Enum) GlowEdge.FieldInvalidationTypes.Location) && !this._invalidatedValues.HasFlag((Enum) GlowEdge.FieldInvalidationTypes.Size) && !this._invalidatedValues.HasFlag((Enum) GlowEdge.FieldInvalidationTypes.Visibility))
      return;
    int flags = 532;
    if (this._invalidatedValues.HasFlag((Enum) GlowEdge.FieldInvalidationTypes.Visibility))
    {
      if (this.IsVisible)
        flags |= 64 /*0x40*/;
      else
        flags |= 131;
    }
    if (!this._invalidatedValues.HasFlag((Enum) GlowEdge.FieldInvalidationTypes.Location))
      flags |= 2;
    if (!this._invalidatedValues.HasFlag((Enum) GlowEdge.FieldInvalidationTypes.Size))
      flags |= 1;
    InteropMethods.SetWindowPos(this.Handle, IntPtr.Zero, this.Left, this.Top, this.Width, this.Height, flags);
  }

  private void UpdateLayeredWindowCore()
  {
    if (!this.IsVisible || !this._invalidatedValues.HasFlag((Enum) GlowEdge.FieldInvalidationTypes.Render))
      return;
    if (this.IsPositionValid)
    {
      this.BeginDelayedRender();
    }
    else
    {
      this.CancelDelayedRender();
      this.RenderLayeredWindow();
    }
  }

  private void BeginDelayedRender()
  {
    if (this._pendingDelayRender)
      return;
    this._pendingDelayRender = true;
    CompositionTarget.Rendering += new EventHandler(this.CommitDelayedRender);
  }

  private void CancelDelayedRender()
  {
    if (!this._pendingDelayRender)
      return;
    this._pendingDelayRender = false;
    CompositionTarget.Rendering -= new EventHandler(this.CommitDelayedRender);
  }

  private void CommitDelayedRender(object sender, EventArgs e)
  {
    this.CancelDelayedRender();
    if (!this.IsVisible)
      return;
    this.RenderLayeredWindow();
  }

  private void RenderLayeredWindow()
  {
    using (GlowDrawingContext drawingContext = new GlowDrawingContext(this.Width, this.Height))
    {
      if (!drawingContext.IsInitialized)
        return;
      switch (this._orientation)
      {
        case Dock.Left:
          this.DrawLeft(drawingContext);
          break;
        case Dock.Top:
          this.DrawTop(drawingContext);
          break;
        case Dock.Right:
          this.DrawRight(drawingContext);
          break;
        default:
          this.DrawBottom(drawingContext);
          break;
      }
      InteropValues.POINT pptDest = new InteropValues.POINT()
      {
        X = this.Left,
        Y = this.Top
      };
      InteropValues.SIZE psize = new InteropValues.SIZE()
      {
        cx = this.Width,
        cy = this.Height
      };
      InteropValues.POINT pptSrc = new InteropValues.POINT()
      {
        X = 0,
        Y = 0
      };
      InteropMethods.UpdateLayeredWindow(this.Handle, drawingContext.ScreenDC, ref pptDest, ref psize, drawingContext.WindowDC, ref pptSrc, 0U, ref drawingContext.Blend, 2U);
    }
  }

  private GlowBitmap GetOrCreateBitmap(GlowDrawingContext drawingContext, GlowBitmapPart bitmapPart)
  {
    GlowBitmap[] glowBitmapArray;
    Color color;
    if (this.IsActive)
    {
      glowBitmapArray = this._activeGlowBitmaps;
      color = this.ActiveGlowColor;
    }
    else
    {
      glowBitmapArray = this._inactiveGlowBitmaps;
      color = this.InactiveGlowColor;
    }
    return glowBitmapArray[(int) bitmapPart] ?? (glowBitmapArray[(int) bitmapPart] = GlowBitmap.Create(drawingContext, bitmapPart, color));
  }

  private void ClearCache(GlowBitmap[] cache)
  {
    for (int index = 0; index < cache.Length; ++index)
    {
      using (cache[index])
        cache[index] = (GlowBitmap) null;
    }
  }

  protected override void DisposeManagedResources()
  {
    this.ClearCache(this._activeGlowBitmaps);
    this.ClearCache(this._inactiveGlowBitmaps);
  }

  protected override void DisposeNativeResources()
  {
    base.DisposeNativeResources();
    ++GlowEdge.DisposedGlowEdges;
  }

  private void DrawLeft(GlowDrawingContext drawingContext)
  {
    GlowBitmap bitmap1 = this.GetOrCreateBitmap(drawingContext, GlowBitmapPart.CornerTopLeft);
    GlowBitmap bitmap2 = this.GetOrCreateBitmap(drawingContext, GlowBitmapPart.LeftTop);
    GlowBitmap bitmap3 = this.GetOrCreateBitmap(drawingContext, GlowBitmapPart.Left);
    GlowBitmap bitmap4 = this.GetOrCreateBitmap(drawingContext, GlowBitmapPart.LeftBottom);
    GlowBitmap bitmap5 = this.GetOrCreateBitmap(drawingContext, GlowBitmapPart.CornerBottomLeft);
    int height = bitmap1.Height;
    int yoriginDest1 = height + bitmap2.Height;
    int yoriginDest2 = drawingContext.Height - bitmap5.Height;
    int yoriginDest3 = yoriginDest2 - bitmap4.Height;
    int hDest = yoriginDest3 - yoriginDest1;
    InteropMethods.SelectObject(drawingContext.BackgroundDC, bitmap1.Handle);
    InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, 0, bitmap1.Width, bitmap1.Height, drawingContext.BackgroundDC, 0, 0, bitmap1.Width, bitmap1.Height, drawingContext.Blend);
    InteropMethods.SelectObject(drawingContext.BackgroundDC, bitmap2.Handle);
    InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, height, bitmap2.Width, bitmap2.Height, drawingContext.BackgroundDC, 0, 0, bitmap2.Width, bitmap2.Height, drawingContext.Blend);
    if (hDest > 0)
    {
      InteropMethods.SelectObject(drawingContext.BackgroundDC, bitmap3.Handle);
      InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, yoriginDest1, bitmap3.Width, hDest, drawingContext.BackgroundDC, 0, 0, bitmap3.Width, bitmap3.Height, drawingContext.Blend);
    }
    InteropMethods.SelectObject(drawingContext.BackgroundDC, bitmap4.Handle);
    InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, yoriginDest3, bitmap4.Width, bitmap4.Height, drawingContext.BackgroundDC, 0, 0, bitmap4.Width, bitmap4.Height, drawingContext.Blend);
    InteropMethods.SelectObject(drawingContext.BackgroundDC, bitmap5.Handle);
    InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, yoriginDest2, bitmap5.Width, bitmap5.Height, drawingContext.BackgroundDC, 0, 0, bitmap5.Width, bitmap5.Height, drawingContext.Blend);
  }

  private void DrawRight(GlowDrawingContext drawingContext)
  {
    GlowBitmap bitmap1 = this.GetOrCreateBitmap(drawingContext, GlowBitmapPart.CornerTopRight);
    GlowBitmap bitmap2 = this.GetOrCreateBitmap(drawingContext, GlowBitmapPart.RightTop);
    GlowBitmap bitmap3 = this.GetOrCreateBitmap(drawingContext, GlowBitmapPart.Right);
    GlowBitmap bitmap4 = this.GetOrCreateBitmap(drawingContext, GlowBitmapPart.RightBottom);
    GlowBitmap bitmap5 = this.GetOrCreateBitmap(drawingContext, GlowBitmapPart.CornerBottomRight);
    int height = bitmap1.Height;
    int yoriginDest1 = height + bitmap2.Height;
    int yoriginDest2 = drawingContext.Height - bitmap5.Height;
    int yoriginDest3 = yoriginDest2 - bitmap4.Height;
    int hDest = yoriginDest3 - yoriginDest1;
    InteropMethods.SelectObject(drawingContext.BackgroundDC, bitmap1.Handle);
    InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, 0, bitmap1.Width, bitmap1.Height, drawingContext.BackgroundDC, 0, 0, bitmap1.Width, bitmap1.Height, drawingContext.Blend);
    InteropMethods.SelectObject(drawingContext.BackgroundDC, bitmap2.Handle);
    InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, height, bitmap2.Width, bitmap2.Height, drawingContext.BackgroundDC, 0, 0, bitmap2.Width, bitmap2.Height, drawingContext.Blend);
    if (hDest > 0)
    {
      InteropMethods.SelectObject(drawingContext.BackgroundDC, bitmap3.Handle);
      InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, yoriginDest1, bitmap3.Width, hDest, drawingContext.BackgroundDC, 0, 0, bitmap3.Width, bitmap3.Height, drawingContext.Blend);
    }
    InteropMethods.SelectObject(drawingContext.BackgroundDC, bitmap4.Handle);
    InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, yoriginDest3, bitmap4.Width, bitmap4.Height, drawingContext.BackgroundDC, 0, 0, bitmap4.Width, bitmap4.Height, drawingContext.Blend);
    InteropMethods.SelectObject(drawingContext.BackgroundDC, bitmap5.Handle);
    InteropMethods.AlphaBlend(drawingContext.WindowDC, 0, yoriginDest2, bitmap5.Width, bitmap5.Height, drawingContext.BackgroundDC, 0, 0, bitmap5.Width, bitmap5.Height, drawingContext.Blend);
  }

  private void DrawTop(GlowDrawingContext drawingContext)
  {
    GlowBitmap bitmap1 = this.GetOrCreateBitmap(drawingContext, GlowBitmapPart.TopLeft);
    GlowBitmap bitmap2 = this.GetOrCreateBitmap(drawingContext, GlowBitmapPart.Top);
    GlowBitmap bitmap3 = this.GetOrCreateBitmap(drawingContext, GlowBitmapPart.TopRight);
    int xoriginDest1 = 9 + bitmap1.Width;
    int xoriginDest2 = drawingContext.Width - 9 - bitmap3.Width;
    int wDest = xoriginDest2 - xoriginDest1;
    InteropMethods.SelectObject(drawingContext.BackgroundDC, bitmap1.Handle);
    InteropMethods.AlphaBlend(drawingContext.WindowDC, 9, 0, bitmap1.Width, bitmap1.Height, drawingContext.BackgroundDC, 0, 0, bitmap1.Width, bitmap1.Height, drawingContext.Blend);
    if (wDest > 0)
    {
      InteropMethods.SelectObject(drawingContext.BackgroundDC, bitmap2.Handle);
      InteropMethods.AlphaBlend(drawingContext.WindowDC, xoriginDest1, 0, wDest, bitmap2.Height, drawingContext.BackgroundDC, 0, 0, bitmap2.Width, bitmap2.Height, drawingContext.Blend);
    }
    InteropMethods.SelectObject(drawingContext.BackgroundDC, bitmap3.Handle);
    InteropMethods.AlphaBlend(drawingContext.WindowDC, xoriginDest2, 0, bitmap3.Width, bitmap3.Height, drawingContext.BackgroundDC, 0, 0, bitmap3.Width, bitmap3.Height, drawingContext.Blend);
  }

  private void DrawBottom(GlowDrawingContext drawingContext)
  {
    GlowBitmap bitmap1 = this.GetOrCreateBitmap(drawingContext, GlowBitmapPart.BottomLeft);
    GlowBitmap bitmap2 = this.GetOrCreateBitmap(drawingContext, GlowBitmapPart.Bottom);
    GlowBitmap bitmap3 = this.GetOrCreateBitmap(drawingContext, GlowBitmapPart.BottomRight);
    int xoriginDest1 = 9 + bitmap1.Width;
    int xoriginDest2 = drawingContext.Width - 9 - bitmap3.Width;
    int wDest = xoriginDest2 - xoriginDest1;
    InteropMethods.SelectObject(drawingContext.BackgroundDC, bitmap1.Handle);
    InteropMethods.AlphaBlend(drawingContext.WindowDC, 9, 0, bitmap1.Width, bitmap1.Height, drawingContext.BackgroundDC, 0, 0, bitmap1.Width, bitmap1.Height, drawingContext.Blend);
    if (wDest > 0)
    {
      InteropMethods.SelectObject(drawingContext.BackgroundDC, bitmap2.Handle);
      InteropMethods.AlphaBlend(drawingContext.WindowDC, xoriginDest1, 0, wDest, bitmap2.Height, drawingContext.BackgroundDC, 0, 0, bitmap2.Width, bitmap2.Height, drawingContext.Blend);
    }
    InteropMethods.SelectObject(drawingContext.BackgroundDC, bitmap3.Handle);
    InteropMethods.AlphaBlend(drawingContext.WindowDC, xoriginDest2, 0, bitmap3.Width, bitmap3.Height, drawingContext.BackgroundDC, 0, 0, bitmap3.Width, bitmap3.Height, drawingContext.Blend);
  }

  internal void UpdateWindowPos()
  {
    IntPtr targetWindowHandle = this.TargetWindowHandle;
    InteropValues.RECT lpRect;
    InteropMethods.GetWindowRect(targetWindowHandle, out lpRect);
    InteropMethods.GetWindowPlacement(targetWindowHandle);
    if (!this.IsVisible)
      return;
    switch (this._orientation)
    {
      case Dock.Left:
        this.Left = lpRect.Left - 9;
        this.Top = lpRect.Top - 9;
        this.Width = 9;
        this.Height = lpRect.Height + 18;
        break;
      case Dock.Top:
        this.Left = lpRect.Left - 9;
        this.Top = lpRect.Top - 9;
        this.Width = lpRect.Width + 18;
        this.Height = 9;
        break;
      case Dock.Right:
        this.Left = lpRect.Right;
        this.Top = lpRect.Top - 9;
        this.Width = 9;
        this.Height = lpRect.Height + 18;
        break;
      default:
        this.Left = lpRect.Left - 9;
        this.Top = lpRect.Bottom;
        this.Width = lpRect.Width + 18;
        this.Height = 9;
        break;
    }
  }

  [Flags]
  private enum FieldInvalidationTypes
  {
    None = 0,
    Location = 1,
    Size = 2,
    ActiveColor = 4,
    InactiveColor = 8,
    Render = 16, // 0x00000010
    Visibility = 32, // 0x00000020
  }
}
