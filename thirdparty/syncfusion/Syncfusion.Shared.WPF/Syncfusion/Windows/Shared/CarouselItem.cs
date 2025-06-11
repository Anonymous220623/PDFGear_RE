// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.CarouselItem
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Shared;

public class CarouselItem : ContentControl
{
  public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(nameof (IsSelected), typeof (bool), typeof (CarouselItem), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(CarouselItem.OnIsSelectedChanged)));
  public static readonly DependencyProperty OwnerProperty = DependencyProperty.Register(nameof (Owner), typeof (Carousel), typeof (CarouselItem), new PropertyMetadata((PropertyChangedCallback) null));

  public CarouselItem() => this.DefaultStyleKey = (object) typeof (CarouselItem);

  public bool IsSelected
  {
    get => (bool) this.GetValue(CarouselItem.IsSelectedProperty);
    set => this.SetValue(CarouselItem.IsSelectedProperty, (object) value);
  }

  public Carousel Owner
  {
    get => (Carousel) this.GetValue(CarouselItem.OwnerProperty);
    set => this.SetValue(CarouselItem.OwnerProperty, (object) value);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.IsManipulationEnabled = true;
    if (this.IsSelected)
      this.Owner.SelectedItem = (object) this;
    if (this.Content == null)
      return;
    if (this.Owner.SelectedItem != null)
      this.Owner.SelectedIndex = this.Owner.Items.IndexOf(this.Owner.SelectedItem);
    if (this.Owner.SelectedIndex < 0 || !this.Owner.Items.Contains(this.Content) || this.Owner.SelectedIndex != this.Owner.Items.IndexOf(this.Content))
      return;
    this.IsSelected = true;
  }

  protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
  {
    if (this.Owner != null && this.Owner.previousSelected != null)
    {
      this.Owner.previousSelected.IsSelected = false;
      this.Owner.previousSelected = this;
    }
    else if (this.Owner != null)
      this.Owner.previousSelected = this;
    this.IsSelected = true;
  }

  protected override void OnTouchMove(TouchEventArgs e)
  {
    if (this.Owner != null && this.Owner.previousSelected != null)
    {
      this.Owner.previousSelected.IsSelected = false;
      this.Owner.previousSelected = this;
    }
    else if (this.Owner != null)
      this.Owner.previousSelected = this;
    this.IsSelected = true;
  }

  private static void OnIsSelectedChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(sender is CarouselItem carouselItem1) || carouselItem1.Owner == null)
      return;
    int num = carouselItem1.Owner.Items.IndexOf(carouselItem1.Content);
    object content = carouselItem1.Content;
    if (num == -1)
      num = carouselItem1.Owner.Items.IndexOf((object) carouselItem1);
    if (!(bool) args.NewValue)
      return;
    if (carouselItem1.Owner.SelectedIndex != num)
      carouselItem1.Owner.SelectedIndex = num;
    foreach (object obj in (IEnumerable) carouselItem1.Owner.Items)
    {
      if (carouselItem1.Owner.ItemContainerGenerator.ContainerFromItem(obj) is CarouselItem carouselItem2 && carouselItem2 != null && carouselItem2 != carouselItem1)
        carouselItem2.IsSelected = false;
    }
  }
}
