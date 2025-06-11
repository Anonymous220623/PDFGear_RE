// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.CarouselPanelHelper
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal class CarouselPanelHelper
{
  private CustomPathCarouselPanel carouselPanel;

  public CarouselPanelHelper(CustomPathCarouselPanel panel) => this.carouselPanel = panel;

  public int ItemsCount => this.ItemCount();

  public int PageSize => this.carouselPanel.ItemsPerPage;

  public int Position => this.carouselPanel.PanelOffset;

  public int ItemCount()
  {
    int count = this.carouselPanel.Children.Count;
    if (this.carouselPanel.IsItemsHost)
      count = this.ParentItem().Items.Count;
    return count;
  }

  private ItemsControl ParentItem()
  {
    ItemsControl itemsControl = (ItemsControl) null;
    if (this.carouselPanel.IsItemsHost)
      itemsControl = ItemsControl.GetItemsOwner((DependencyObject) this.carouselPanel);
    return itemsControl;
  }
}
