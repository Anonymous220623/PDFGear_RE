// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.VisualHelper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools.Interop;
using System;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Tools;

public static class VisualHelper
{
  internal static VisualStateGroup TryGetVisualStateGroup(DependencyObject d, string groupName)
  {
    FrameworkElement implementationRoot = VisualHelper.GetImplementationRoot(d);
    if (implementationRoot == null)
      return (VisualStateGroup) null;
    IList visualStateGroups = VisualStateManager.GetVisualStateGroups(implementationRoot);
    return visualStateGroups == null ? (VisualStateGroup) null : visualStateGroups.OfType<VisualStateGroup>().FirstOrDefault<VisualStateGroup>((Func<VisualStateGroup, bool>) (group => string.CompareOrdinal(groupName, group.Name) == 0));
  }

  internal static FrameworkElement GetImplementationRoot(DependencyObject d)
  {
    return 1 != VisualTreeHelper.GetChildrenCount(d) ? (FrameworkElement) null : VisualTreeHelper.GetChild(d, 0) as FrameworkElement;
  }

  public static T GetChild<T>(DependencyObject d) where T : DependencyObject
  {
    if (d == null)
      return default (T);
    if (d is T child1)
      return child1;
    for (int childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(d); ++childIndex)
    {
      T child2 = VisualHelper.GetChild<T>(VisualTreeHelper.GetChild(d, childIndex));
      if ((object) child2 != null)
        return child2;
    }
    return default (T);
  }

  public static T GetParent<T>(DependencyObject d) where T : DependencyObject
  {
    T parent;
    switch (d)
    {
      case null:
        parent = default (T);
        break;
      case T obj:
        parent = obj;
        break;
      case Window _:
        parent = default (T);
        break;
      default:
        parent = VisualHelper.GetParent<T>(VisualTreeHelper.GetParent(d));
        break;
    }
    return parent;
  }

  public static IntPtr GetHandle(this Visual visual)
  {
    // ISSUE: explicit non-virtual call
    return !(PresentationSource.FromVisual(visual) is HwndSource hwndSource) ? IntPtr.Zero : __nonvirtual (hwndSource.Handle);
  }

  internal static void HitTestVisibleElements(
    Visual visual,
    HitTestResultCallback resultCallback,
    HitTestParameters parameters)
  {
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    VisualTreeHelper.HitTest(visual, VisualHelper.\u003C\u003EO.\u003C0\u003E__ExcludeNonVisualElements ?? (VisualHelper.\u003C\u003EO.\u003C0\u003E__ExcludeNonVisualElements = new HitTestFilterCallback(VisualHelper.ExcludeNonVisualElements)), resultCallback, parameters);
  }

  private static HitTestFilterBehavior ExcludeNonVisualElements(
    DependencyObject potentialHitTestTarget)
  {
    return !(potentialHitTestTarget is Visual) || potentialHitTestTarget is UIElement uiElement && (!uiElement.IsVisible || !uiElement.IsEnabled) ? HitTestFilterBehavior.ContinueSkipSelfAndChildren : HitTestFilterBehavior.Continue;
  }

  internal static bool ModifyStyle(IntPtr hWnd, int styleToRemove, int styleToAdd)
  {
    int windowLong = InteropMethods.GetWindowLong(hWnd, InteropValues.GWL.STYLE);
    int dwNewLong = windowLong & ~styleToRemove | styleToAdd;
    if (dwNewLong == windowLong)
      return false;
    InteropMethods.SetWindowLong(hWnd, InteropValues.GWL.STYLE, dwNewLong);
    return true;
  }
}
