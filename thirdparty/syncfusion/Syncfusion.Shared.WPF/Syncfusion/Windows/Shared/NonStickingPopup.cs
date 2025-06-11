// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.NonStickingPopup
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Interop;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class NonStickingPopup : Popup
{
  private bool m_Shift = true;
  private int m_Shiftcount;

  static NonStickingPopup()
  {
    Popup.PlacementRectangleProperty.OverrideMetadata(typeof (NonStickingPopup), (PropertyMetadata) new FrameworkPropertyMetadata((object) new Rect(0.0, 0.0, 0.0, 0.0), new PropertyChangedCallback(NonStickingPopup.OnPlacementRectangleChanged), (CoerceValueCallback) null));
  }

  public void Removehandle()
  {
  }

  private static void OnPlacementRectangleChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    (d as NonStickingPopup).OnPlacementRectangleChanged(args);
  }

  private void OnPlacementRectangleChanged(DependencyPropertyChangedEventArgs args)
  {
    this.MaxWidth = 0.87 * SystemParameters.PrimaryScreenWidth;
    this.MaxHeight = 0.75 * SystemParameters.PrimaryScreenHeight;
    if (this.Child == null)
      return;
    HwndSource hwndSource = (HwndSource) PresentationSource.FromVisual((Visual) this.Child);
    if (hwndSource == null)
      return;
    NativeMethods.SetWindowPos(hwndSource.Handle, 0, 0, 0, 0, 0, 51);
  }

  private IntPtr PositioningHook(
    IntPtr hwnd,
    int msg,
    IntPtr wParam,
    IntPtr lParam,
    ref bool handled)
  {
    if (msg != 70)
      return IntPtr.Zero;
    WINDOWPOS structure = (WINDOWPOS) Marshal.PtrToStructure(lParam, typeof (WINDOWPOS));
    HwndSource hwndSource = HwndSource.FromHwnd(structure.hwnd);
    if (this.m_Shift && this.ChangeDefault())
    {
      this.MaxWidth = 0.87 * SystemParameters.PrimaryScreenWidth;
      this.MaxHeight = 0.75 * SystemParameters.PrimaryScreenHeight;
      if (this.m_Shiftcount > 2)
        this.m_Shift = false;
      ++this.m_Shiftcount;
    }
    if (hwndSource != null)
    {
      Matrix transformFromDevice = hwndSource.CompositionTarget.TransformFromDevice;
      Matrix transformToDevice = hwndSource.CompositionTarget.TransformToDevice;
      Point point1 = new Point(this.PlacementRectangle.X, this.PlacementRectangle.Y);
      Point point2 = new Point(this.PlacementRectangle.X + this.PlacementRectangle.Width, this.PlacementRectangle.Y + this.PlacementRectangle.Height);
      Point point3 = transformToDevice.Transform(point1);
      point2 = transformToDevice.Transform(point2);
      structure.x = (int) point3.X;
      structure.y = (int) point3.Y;
      structure.cx = (int) (point2.X - point3.X);
      structure.cy = (int) (point2.Y - point3.Y);
      structure.flags &= -4;
      structure.flags |= 32 /*0x20*/;
      Marshal.StructureToPtr<WINDOWPOS>(structure, lParam, true);
    }
    else
      Marshal.DestroyStructure(lParam, typeof (WINDOWPOS));
    return IntPtr.Zero;
  }

  protected override void OnOpened(EventArgs e)
  {
    if (this.TemplatedParent != null && this.TemplatedParent.GetType().Name == "NotifyIcon" && (int) typeof (SystemParameters).GetProperty("DpiX", BindingFlags.Static | BindingFlags.NonPublic).GetValue((object) null, (object[]) null) > 96 /*0x60*/)
    {
      base.OnOpened(e);
    }
    else
    {
      if (this.Child != null && PermissionHelper.HasUnmanagedCodePermission)
        this.OnOpenedSecure();
      base.OnOpened(e);
    }
  }

  private bool ChangeDefault() => this.Height != 100.0 && this.Width != 150.0;

  private void OnOpenedSecure()
  {
    HwndSource hwndSource = (HwndSource) PresentationSource.FromVisual((Visual) this.Child);
    HwndSourceHook hook = new HwndSourceHook(this.PositioningHook);
    hwndSource.RemoveHook(hook);
    hwndSource.AddHook(hook);
  }
}
