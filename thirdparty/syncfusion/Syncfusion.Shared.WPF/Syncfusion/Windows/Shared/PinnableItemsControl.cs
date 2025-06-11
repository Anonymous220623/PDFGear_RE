// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.PinnableItemsControl
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class PinnableItemsControl : ItemsControl
{
  internal PinnableListBox pinnableListBox;

  public bool IsPinnedContainer { get; set; }

  protected override bool IsItemItsOwnContainerOverride(object item) => item is PinnableListBoxItem;

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new PinnableListBoxItem();
  }

  protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
  {
    if (element is PinnableListBoxItem pinnableListBoxItem)
    {
      pinnableListBoxItem.pinnableListBox = this.pinnableListBox;
      if (pinnableListBoxItem.pinnableListBox.pinnableItem != null)
        pinnableListBoxItem.pinnableListBox.pinnableItem = pinnableListBoxItem;
      if (pinnableListBoxItem.pinnableListBox.isCalledByUpdateItems)
        pinnableListBoxItem.IsPinned = this.IsPinnedContainer;
    }
    base.PrepareContainerForItemOverride(element, item);
  }
}
