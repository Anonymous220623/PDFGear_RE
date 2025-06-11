// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.LogicalUtils
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

public sealed class LogicalUtils
{
  public static DependencyObject GetParent(DependencyObject current)
  {
    return LogicalTreeHelper.GetParent(current);
  }

  public static DependencyObject GetRootParent(DependencyObject current)
  {
    DependencyObject rootParent = current;
    for (; current != null; current = LogicalUtils.GetParent(current))
      rootParent = current;
    return rootParent;
  }

  public static DependencyObject GetParentOfType(DependencyObject current, Type type)
  {
    while (current != null && !type.IsInstanceOfType((object) current))
      current = LogicalUtils.GetParent(current);
    return current;
  }
}
