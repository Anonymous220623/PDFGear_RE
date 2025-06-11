// Decompiled with JetBrains decompiler
// Type: WpfToolkit.Controls.GridView
// Assembly: VirtualizingWrapPanel, Version=1.5.4.0, Culture=neutral, PublicKeyToken=null
// MVID: E61E2A8E-A00C-4FB4-9D6E-5B7404CFB214
// Assembly location: D:\PDFGear\bin\VirtualizingWrapPanel.dll

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

#nullable enable
namespace WpfToolkit.Controls;

public class GridView : ListView
{
  public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof (Orientation), typeof (Orientation), typeof (GridView), (PropertyMetadata) new FrameworkPropertyMetadata((object) Orientation.Vertical));
  public static readonly DependencyProperty SpacingModeProperty = DependencyProperty.Register(nameof (SpacingMode), typeof (SpacingMode), typeof (GridView), (PropertyMetadata) new FrameworkPropertyMetadata((object) SpacingMode.Uniform));
  public static readonly DependencyProperty StretchItemsProperty = DependencyProperty.Register(nameof (StretchItems), typeof (bool), typeof (GridView), (PropertyMetadata) new FrameworkPropertyMetadata((object) false));

  public Orientation Orientation
  {
    get => (Orientation) this.GetValue(GridView.OrientationProperty);
    set => this.SetValue(GridView.OrientationProperty, (object) value);
  }

  public SpacingMode SpacingMode
  {
    get => (SpacingMode) this.GetValue(GridView.SpacingModeProperty);
    set => this.SetValue(GridView.SpacingModeProperty, (object) value);
  }

  public bool StretchItems
  {
    get => (bool) this.GetValue(GridView.StretchItemsProperty);
    set => this.SetValue(GridView.StretchItemsProperty, (object) value);
  }

  static GridView()
  {
    DependencyProperty containerStyleProperty = ItemsControl.ItemContainerStyleProperty;
    Type forType = typeof (GridView);
    Style defaultValue = new Style();
    defaultValue.Setters.Add((SetterBase) new Setter()
    {
      Property = FrameworkElement.MarginProperty,
      Value = (object) new Thickness(0.0)
    });
    defaultValue.Setters.Add((SetterBase) new Setter()
    {
      Property = Control.PaddingProperty,
      Value = (object) new Thickness(4.0)
    });
    defaultValue.Setters.Add((SetterBase) new Setter()
    {
      Property = Control.HorizontalContentAlignmentProperty,
      Value = (object) HorizontalAlignment.Stretch
    });
    defaultValue.Setters.Add((SetterBase) new Setter()
    {
      Property = Control.VerticalContentAlignmentProperty,
      Value = (object) VerticalAlignment.Stretch
    });
    FrameworkPropertyMetadata typeMetadata = new FrameworkPropertyMetadata((object) defaultValue);
    containerStyleProperty.OverrideMetadata(forType, (PropertyMetadata) typeMetadata);
  }

  public GridView()
  {
    FrameworkElementFactory root = new FrameworkElementFactory(typeof (VirtualizingWrapPanel));
    root.SetBinding(VirtualizingWrapPanel.OrientationProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath(nameof (Orientation), new object[0]),
      Mode = BindingMode.OneWay
    });
    root.SetBinding(VirtualizingWrapPanel.SpacingModeProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath(nameof (SpacingMode), new object[0]),
      Mode = BindingMode.OneWay
    });
    root.SetBinding(VirtualizingWrapPanel.StretchItemsProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath(nameof (StretchItems), new object[0]),
      Mode = BindingMode.OneWay
    });
    this.ItemsPanel = new ItemsPanelTemplate(root);
    VirtualizingPanel.SetCacheLengthUnit((DependencyObject) this, VirtualizationCacheLengthUnit.Page);
    VirtualizingPanel.SetCacheLength((DependencyObject) this, new VirtualizationCacheLength(1.0));
    VirtualizingPanel.SetIsVirtualizingWhenGrouping((DependencyObject) this, true);
  }
}
