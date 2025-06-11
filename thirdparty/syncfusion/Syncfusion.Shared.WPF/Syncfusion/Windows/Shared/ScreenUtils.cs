// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.ScreenUtils
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Collections;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class ScreenUtils
{
  public static Rect FixByScreenBounds(Rect rectWindow)
  {
    if (rectWindow.IsEmpty || SystemParameters.PrimaryScreenWidth != SystemParameters.VirtualScreenWidth)
      return rectWindow;
    Rect workArea = SystemParameters.WorkArea;
    if (workArea.Bottom <= rectWindow.Y)
      rectWindow.Y = workArea.Bottom - rectWindow.Height;
    if (rectWindow.X >= workArea.Right)
      rectWindow.X = workArea.Right - rectWindow.Width;
    else if (rectWindow.X <= workArea.Left)
      rectWindow.X = workArea.Left;
    return rectWindow;
  }

  public static FrameworkElement GetElementFromPoint(ItemsControl parentItemsControl, Point point)
  {
    foreach (object obj in (IEnumerable) parentItemsControl.Items)
    {
      UIElement elementFromPoint = !(obj is UIElement) ? parentItemsControl.ItemContainerGenerator.ContainerFromItem(obj) as UIElement : obj as UIElement;
      if (elementFromPoint != null && elementFromPoint.IsVisible && new Rect(elementFromPoint.PointToScreen(new Point(0.0, 0.0)), elementFromPoint.PointToScreen(new Point(elementFromPoint.RenderSize.Width, elementFromPoint.RenderSize.Height))).Contains(point))
        return elementFromPoint as FrameworkElement;
    }
    return (FrameworkElement) null;
  }
}
