// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.PermissionHelper
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class PermissionHelper
{
  private static bool? m_unmanagedCodePermission;

  public static bool HasUnmanagedCodePermission
  {
    get
    {
      if (!PermissionHelper.m_unmanagedCodePermission.HasValue)
        PermissionHelper.m_unmanagedCodePermission = PermissionHelper.HasSecurityPermissionFlag(SecurityPermissionFlag.UnmanagedCode);
      return PermissionHelper.m_unmanagedCodePermission.Value;
    }
  }

  public static Point GetSafePointToScreen(Visual visual, Point point)
  {
    try
    {
      return visual is HwndHost ? (visual == null || PresentationSource.FromVisual(visual) == null ? point : visual.PointToScreen(point)) : visual.PointToScreen(point);
    }
    catch
    {
      try
      {
        return visual == null || PresentationSource.FromVisual(visual) == null ? point : visual.PointToScreen(point);
      }
      catch
      {
        return (visual as FrameworkElement).Parent == null || PresentationSource.FromVisual((visual as FrameworkElement).Parent as Visual) == null ? point : ((visual as FrameworkElement).Parent as Visual).PointToScreen(point);
      }
    }
  }

  private static bool? HasSecurityPermissionFlag(SecurityPermissionFlag flag)
  {
    bool? nullable = new bool?(true);
    try
    {
      new SecurityPermission(flag).Demand();
    }
    catch (Exception ex)
    {
      nullable = new bool?(false);
    }
    return nullable;
  }
}
