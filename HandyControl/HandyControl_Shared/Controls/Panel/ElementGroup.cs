// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ElementGroup
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

public class ElementGroup : ItemsControl
{
  private static readonly Dictionary<Orientation, Dictionary<ElementGroup.ChildLocation, CornerRadius>> CornerRadiusDict;
  public static readonly DependencyProperty OrientationProperty = StackPanel.OrientationProperty.AddOwner(typeof (ElementGroup), (PropertyMetadata) new FrameworkPropertyMetadata(ValueBoxes.HorizontalBox, FrameworkPropertyMetadataOptions.AffectsMeasure));
  public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(nameof (Layout), typeof (LinearLayout), typeof (ElementGroup), (PropertyMetadata) new FrameworkPropertyMetadata((object) LinearLayout.Uniform, FrameworkPropertyMetadataOptions.AffectsMeasure));

  public Orientation Orientation
  {
    get => (Orientation) this.GetValue(ElementGroup.OrientationProperty);
    set => this.SetValue(ElementGroup.OrientationProperty, ValueBoxes.OrientationBox(value));
  }

  public LinearLayout Layout
  {
    get => (LinearLayout) this.GetValue(ElementGroup.LayoutProperty);
    set => this.SetValue(ElementGroup.LayoutProperty, (object) value);
  }

  static ElementGroup()
  {
    CornerRadius resourceInternal = HandyControl.Tools.ResourceHelper.GetResourceInternal<CornerRadius>("DefaultCornerRadius");
    ElementGroup.CornerRadiusDict = new Dictionary<Orientation, Dictionary<ElementGroup.ChildLocation, CornerRadius>>()
    {
      [Orientation.Horizontal] = new Dictionary<ElementGroup.ChildLocation, CornerRadius>()
      {
        [ElementGroup.ChildLocation.Single] = resourceInternal,
        [ElementGroup.ChildLocation.First] = new CornerRadius(resourceInternal.TopLeft, 0.0, 0.0, resourceInternal.BottomLeft),
        [ElementGroup.ChildLocation.Middle] = new CornerRadius(),
        [ElementGroup.ChildLocation.Last] = new CornerRadius(0.0, resourceInternal.TopRight, resourceInternal.BottomRight, 0.0)
      },
      [Orientation.Vertical] = new Dictionary<ElementGroup.ChildLocation, CornerRadius>()
      {
        [ElementGroup.ChildLocation.Single] = resourceInternal,
        [ElementGroup.ChildLocation.First] = new CornerRadius(resourceInternal.TopLeft, resourceInternal.TopRight, 0.0, 0.0),
        [ElementGroup.ChildLocation.Middle] = new CornerRadius(),
        [ElementGroup.ChildLocation.Last] = new CornerRadius(0.0, 0.0, resourceInternal.BottomRight, resourceInternal.BottomLeft)
      }
    };
  }

  protected override void OnVisualChildrenChanged(
    DependencyObject visualAdded,
    DependencyObject visualRemoved)
  {
    base.OnVisualChildrenChanged(visualAdded, visualRemoved);
    if (visualAdded is UIElement uiElement1)
    {
      uiElement1.GotFocus += new RoutedEventHandler(this.ElementAdded_GotFocus);
      uiElement1.LostFocus += new RoutedEventHandler(this.ElementAdded_LostFocus);
      uiElement1.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.ElementAdded_IsVisibleChanged);
    }
    if (!(visualRemoved is UIElement uiElement2))
      return;
    uiElement2.GotFocus -= new RoutedEventHandler(this.ElementAdded_GotFocus);
    uiElement2.LostFocus -= new RoutedEventHandler(this.ElementAdded_LostFocus);
    uiElement2.IsVisibleChanged -= new DependencyPropertyChangedEventHandler(this.ElementAdded_IsVisibleChanged);
  }

  protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
  {
    this.UpdateChildrenCornerRadius();
  }

  protected override void OnRender(DrawingContext drawingContext)
  {
    this.UpdateChildrenCornerRadius();
  }

  private void UpdateChildrenCornerRadius()
  {
    List<UIElement> visibleChildrenCount = this.GetVisibleChildrenCount();
    int count = visibleChildrenCount.Count;
    if (count == 0)
      return;
    Orientation orientation = this.Orientation;
    if (count == 1)
    {
      ElementGroup.UpdateChildCornerRadius((DependencyObject) visibleChildrenCount[0], ElementGroup.CornerRadiusDict[orientation][ElementGroup.ChildLocation.Single]);
    }
    else
    {
      ElementGroup.UpdateChildCornerRadius((DependencyObject) visibleChildrenCount[0], ElementGroup.CornerRadiusDict[orientation][ElementGroup.ChildLocation.First]);
      Thickness thickness = orientation == Orientation.Horizontal ? new Thickness(-1.0, 0.0, 0.0, 0.0) : new Thickness(0.0, -1.0, 0.0, 0.0);
      for (int index = 1; index < count; ++index)
      {
        UIElement child = visibleChildrenCount[index];
        child.SetCurrentValue(FrameworkElement.MarginProperty, (object) thickness);
        ElementGroup.UpdateChildCornerRadius((DependencyObject) child, ElementGroup.CornerRadiusDict[orientation][ElementGroup.ChildLocation.Middle]);
      }
      UIElement child1 = visibleChildrenCount[count - 1];
      child1.SetCurrentValue(FrameworkElement.MarginProperty, (object) thickness);
      ElementGroup.UpdateChildCornerRadius((DependencyObject) child1, ElementGroup.CornerRadiusDict[orientation][ElementGroup.ChildLocation.Last]);
    }
  }

  private List<UIElement> GetVisibleChildrenCount()
  {
    return this.Items.OfType<UIElement>().Where<UIElement>((Func<UIElement, bool>) (element => element.Visibility == Visibility.Visible)).ToList<UIElement>();
  }

  private static void UpdateChildCornerRadius(DependencyObject child, CornerRadius cornerRadius)
  {
    if (child is Border border)
      border.SetCurrentValue(Border.CornerRadiusProperty, (object) cornerRadius);
    else
      BorderElement.SetCornerRadius(child, cornerRadius);
  }

  private void ElementAdded_LostFocus(object sender, RoutedEventArgs e)
  {
    ElementGroup.ResetElementZIndex(e.OriginalSource);
  }

  private void ElementAdded_GotFocus(object sender, RoutedEventArgs e)
  {
    ElementGroup.MaximizeElementZIndex(e.OriginalSource);
  }

  private static void ResetElementZIndex(object element) => Panel.SetZIndex((UIElement) element, 0);

  private static void MaximizeElementZIndex(object element)
  {
    Panel.SetZIndex((UIElement) element, int.MaxValue);
  }

  private void ElementAdded_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    this.UpdateChildrenCornerRadius();
  }

  private enum ChildLocation
  {
    Single,
    First,
    Middle,
    Last,
  }
}
