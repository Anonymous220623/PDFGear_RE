// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.OverviewExtension
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal static class OverviewExtension
{
  internal static double GetScaleRatio(this double source, double target) => target / source;

  private static Size GetScaleRatioSize(this Size source, Size target)
  {
    return new Size(target.Width / source.Width, target.Height / source.Height);
  }

  internal static double GetScaleRatioDouble(this Size source, Size target)
  {
    Size scaleRatioSize = source.GetScaleRatioSize(target);
    return scaleRatioSize.Width >= scaleRatioSize.Height ? scaleRatioSize.Width : scaleRatioSize.Height;
  }

  internal static Size GetUniformSize(this Size source, Size target)
  {
    double scaleRatioDouble = source.GetScaleRatioDouble(target);
    Size size = new Size(target.Width / scaleRatioDouble, target.Height / scaleRatioDouble);
    return new Size(double.IsNaN(size.Width) ? 0.0 : size.Width, double.IsNaN(size.Height) ? 0.0 : size.Height);
  }

  internal static bool IsMouseWheelUp(this MouseWheelEventArgs args, OverviewContentHolder och)
  {
    return (ZoomGesture.MouseWheelUp & och.ZoomInGesture) != ZoomGesture.None && args.Delta > 0;
  }

  internal static bool CanZoom_Keys_Or(this OverviewContentHolder och, ZoomGesture gesture)
  {
    ZoomGesture zoomGesture1 = gesture & (ZoomGesture.Ctrl | ZoomGesture.Shift | ZoomGesture.Alt);
    ModifierKeys modifierKeys = Keyboard.Modifiers & (ModifierKeys.Alt | ModifierKeys.Control | ModifierKeys.Shift);
    ZoomGesture zoomGesture2 = ZoomGesture.Ctrl | ZoomGesture.Shift;
    ZoomGesture zoomGesture3 = ZoomGesture.Shift | ZoomGesture.Alt;
    ZoomGesture zoomGesture4 = ZoomGesture.Ctrl | ZoomGesture.Alt;
    ZoomGesture zoomGesture5 = ZoomGesture.Ctrl | ZoomGesture.Shift | ZoomGesture.Alt;
    return (zoomGesture5 & zoomGesture1) == ZoomGesture.None || ZoomGesture.Ctrl == zoomGesture1 && (ModifierKeys.Control & modifierKeys) != ModifierKeys.None || ZoomGesture.Shift == zoomGesture1 && (ModifierKeys.Shift & modifierKeys) != ModifierKeys.None || ZoomGesture.Alt == zoomGesture1 && (ModifierKeys.Alt & modifierKeys) != ModifierKeys.None || zoomGesture2 == zoomGesture1 && ((ModifierKeys.Control | ModifierKeys.Shift) & modifierKeys) != ModifierKeys.None || zoomGesture3 == zoomGesture1 && ((ModifierKeys.Alt | ModifierKeys.Shift) & modifierKeys) != ModifierKeys.None || zoomGesture4 == zoomGesture1 && ((ModifierKeys.Alt | ModifierKeys.Control) & modifierKeys) != ModifierKeys.None || zoomGesture5 == zoomGesture1 && ((ModifierKeys.Alt | ModifierKeys.Control | ModifierKeys.Shift) & modifierKeys) != ModifierKeys.None;
  }

  internal static bool CanZoom_Keys_And(this OverviewContentHolder och, ZoomGesture gesture)
  {
    ZoomGesture zoomGesture1 = gesture & (ZoomGesture.Ctrl | ZoomGesture.Shift | ZoomGesture.Alt);
    ModifierKeys modifierKeys1 = Keyboard.Modifiers & ModifierKeys.Control;
    ModifierKeys modifierKeys2 = Keyboard.Modifiers & ModifierKeys.Shift;
    ModifierKeys modifierKeys3 = Keyboard.Modifiers & ModifierKeys.Alt;
    ModifierKeys modifierKeys4 = Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift);
    ModifierKeys modifierKeys5 = Keyboard.Modifiers & (ModifierKeys.Alt | ModifierKeys.Shift);
    ModifierKeys modifierKeys6 = Keyboard.Modifiers & (ModifierKeys.Alt | ModifierKeys.Control);
    ModifierKeys modifierKeys7 = Keyboard.Modifiers & (ModifierKeys.Alt | ModifierKeys.Control | ModifierKeys.Shift);
    ZoomGesture zoomGesture2 = ZoomGesture.Ctrl | ZoomGesture.Shift;
    ZoomGesture zoomGesture3 = ZoomGesture.Shift | ZoomGesture.Alt;
    ZoomGesture zoomGesture4 = ZoomGesture.Ctrl | ZoomGesture.Alt;
    ZoomGesture zoomGesture5 = ZoomGesture.Ctrl | ZoomGesture.Shift | ZoomGesture.Alt;
    return (zoomGesture5 & zoomGesture1) == ZoomGesture.None || ZoomGesture.Ctrl == zoomGesture1 && ModifierKeys.Control == modifierKeys1 || ZoomGesture.Shift == zoomGesture1 && ModifierKeys.Shift == modifierKeys2 || ZoomGesture.Alt == zoomGesture1 && ModifierKeys.Alt == modifierKeys3 || zoomGesture2 == zoomGesture1 && (ModifierKeys.Control | ModifierKeys.Shift) == modifierKeys4 || zoomGesture3 == zoomGesture1 && (ModifierKeys.Alt | ModifierKeys.Shift) == modifierKeys5 || zoomGesture4 == zoomGesture1 && (ModifierKeys.Alt | ModifierKeys.Control) == modifierKeys6 || zoomGesture5 == zoomGesture1 && (ModifierKeys.Alt | ModifierKeys.Control | ModifierKeys.Shift) == modifierKeys7;
  }

  internal static bool CanZoom(
    this MouseWheelEventArgs args,
    OverviewContentHolder och,
    ZoomGesture gesture)
  {
    if (((gesture & ZoomGesture.MouseWheelUp) == ZoomGesture.None || args.Delta <= 0) && ((gesture & ZoomGesture.MouseWheelDown) == ZoomGesture.None || args.Delta >= 0))
      return false;
    return (gesture & ZoomGesture.And) != ZoomGesture.None ? och.CanZoom_Keys_And(gesture) : och.CanZoom_Keys_Or(gesture);
  }

  internal static bool CanZoom(
    this MouseButtonEventArgs args,
    OverviewContentHolder och,
    ZoomGesture gesture)
  {
    if (((gesture & ZoomGesture.LeftClick) == ZoomGesture.None || och.m_MouseState != OverviewMouseState.LeftClick && och.m_MouseState != OverviewMouseState.LeftDoubleClick) && ((gesture & ZoomGesture.RightClick) == ZoomGesture.None || och.m_MouseState != OverviewMouseState.RightClick && och.m_MouseState != OverviewMouseState.RightDoubleClick) && ((gesture & ZoomGesture.LeftDoubleClick) == ZoomGesture.None || och.m_MouseState != OverviewMouseState.LeftDoubleClick) && ((gesture & ZoomGesture.RightDoubleClick) == ZoomGesture.None || och.m_MouseState != OverviewMouseState.RightDoubleClick))
      return false;
    return (gesture & ZoomGesture.And) != ZoomGesture.None ? och.CanZoom_Keys_And(gesture) : och.CanZoom_Keys_Or(gesture);
  }

  internal static bool CanZoomIn(this MouseWheelEventArgs args, OverviewContentHolder och)
  {
    return och.IsZoomInEnabled && args.CanZoom(och, och.ZoomInGesture);
  }

  internal static bool CanZoomOut(this MouseWheelEventArgs args, OverviewContentHolder och)
  {
    return och.IsZoomOutEnabled && args.CanZoom(och, och.ZoomOutGesture);
  }

  internal static bool CanZoomIn(this MouseButtonEventArgs args, OverviewContentHolder och)
  {
    return och.IsZoomInEnabled && args.CanZoom(och, och.ZoomInGesture);
  }

  internal static bool CanZoomOut(this MouseButtonEventArgs args, OverviewContentHolder och)
  {
    return och.IsZoomOutEnabled && args.CanZoom(och, och.ZoomOutGesture);
  }

  internal static bool CanZoomIn(this MouseEventArgs args, OverviewContentHolder och)
  {
    switch (args)
    {
      case MouseWheelEventArgs _:
        return OverviewExtension.CanZoomIn(args as MouseWheelEventArgs, och);
      case MouseButtonEventArgs _:
        return OverviewExtension.CanZoomIn(args as MouseButtonEventArgs, och);
      default:
        return false;
    }
  }

  internal static bool CanZoomOut(this MouseEventArgs args, OverviewContentHolder och)
  {
    switch (args)
    {
      case MouseWheelEventArgs _:
        return OverviewExtension.CanZoomOut(args as MouseWheelEventArgs, och);
      case MouseButtonEventArgs _:
        return OverviewExtension.CanZoomOut(args as MouseButtonEventArgs, och);
      default:
        return false;
    }
  }
}
