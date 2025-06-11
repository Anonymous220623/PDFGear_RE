// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Extension.DependencyObjectExtension
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Tools.Extension;

public static class DependencyObjectExtension
{
  public static DependencyObject GetVisualOrLogicalParent(this DependencyObject sourceElement)
  {
    return sourceElement == null ? (DependencyObject) null : (sourceElement is Visual ? VisualTreeHelper.GetParent(sourceElement) ?? LogicalTreeHelper.GetParent(sourceElement) : LogicalTreeHelper.GetParent(sourceElement));
  }
}
