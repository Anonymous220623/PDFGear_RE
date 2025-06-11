// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.TabItemCapsuleStyleSelector
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Tools;

public class TabItemCapsuleStyleSelector : StyleSelector
{
  public override Style SelectStyle(object item, DependencyObject container)
  {
    if (container is TabItem tabItem)
    {
      TabControl parent = VisualHelper.GetParent<TabControl>((DependencyObject) tabItem);
      if (parent != null)
      {
        int count = parent.Items.Count;
        if (count == 1)
          return ResourceHelper.GetResourceInternal<Style>("TabItemCapsuleSingle");
        int num = parent.ItemContainerGenerator.IndexFromContainer((DependencyObject) tabItem);
        return num != 0 ? ResourceHelper.GetResourceInternal<Style>(num == count - 1 ? (parent.TabStripPlacement == Dock.Top || parent.TabStripPlacement == Dock.Bottom ? "TabItemCapsuleHorizontalLast" : "TabItemCapsuleVerticalLast") : "TabItemCapsuleDefault") : ResourceHelper.GetResourceInternal<Style>(parent.TabStripPlacement == Dock.Top || parent.TabStripPlacement == Dock.Bottom ? "TabItemCapsuleHorizontalFirst" : "TabItemCapsuleVerticalFirst");
      }
    }
    return (Style) null;
  }
}
