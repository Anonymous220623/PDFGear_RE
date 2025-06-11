// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Tools.Controls.DropDownMenuGroup
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using Syncfusion.Windows.Controls.Tools.AutomationPeer;
using Syncfusion.Windows.Shared;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

#nullable disable
namespace Syncfusion.Windows.Tools.Controls;

[SkinType(SkinVisualStyle = Skin.Transparent, Type = typeof (DropDownMenuGroup), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/TransparentStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Black, Type = typeof (DropDownMenuGroup), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/Office2007BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Blue, Type = typeof (DropDownMenuGroup), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/Office2010BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Black, Type = typeof (DropDownMenuGroup), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/Office2010BlackStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2010Silver, Type = typeof (DropDownMenuGroup), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/Office2010SilverStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Blend, Type = typeof (DropDownMenuGroup), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/BlendStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.VS2010, Type = typeof (DropDownMenuGroup), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/VS2010Style.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyRed, Type = typeof (DropDownMenuGroup), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/ShinyRedStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.ShinyBlue, Type = typeof (DropDownMenuGroup), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/ShinyBlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Default, Type = typeof (DropDownMenuGroup), XamlResource = "/Syncfusion.Shared.WPF;component/Controls/ButtonControls/DropDownButton/Themes/DropDownButton.xaml")]
[SkinType(SkinVisualStyle = Skin.Metro, Type = typeof (DropDownMenuGroup), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/MetroStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Blue, Type = typeof (DropDownMenuGroup), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/Office2007BlueStyle.xaml")]
[SkinType(SkinVisualStyle = Skin.Office2007Silver, Type = typeof (DropDownMenuGroup), XamlResource = "pack://application:,,,/Syncfusion.Shared.Wpf.Classic;component/Controls/ButtonControls/DropDownButton/Themes/Office2007SilverStyle.xaml")]
public class DropDownMenuGroup : HeaderedItemsControl, IDisposable
{
  internal const int iconPadding = 10;
  private Thumb _resizethumb;
  internal Border iconContainer;
  internal Border moreItemTrayBar;
  internal double maximumTrayBarWidth = 16.0;
  public static readonly DependencyProperty IconBarEnabledProperty = DependencyProperty.Register(nameof (IconBarEnabled), typeof (bool), typeof (DropDownMenuGroup), new PropertyMetadata((object) false));
  public static readonly DependencyProperty IsMoreItemsIconTrayEnabledProperty = DependencyProperty.Register(nameof (IsMoreItemsIconTrayEnabled), typeof (bool), typeof (DropDownMenuGroup), new PropertyMetadata((object) true));
  public static readonly DependencyProperty ScrollBarVisibilityProperty = DependencyProperty.Register(nameof (ScrollBarVisibility), typeof (ScrollBarVisibility), typeof (DropDownMenuGroup), new PropertyMetadata((object) ScrollBarVisibility.Disabled));
  public static readonly DependencyProperty MoreItemsProperty = DependencyProperty.Register(nameof (MoreItems), typeof (ObservableCollection<UIElement>), typeof (DropDownMenuGroup), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty IsResizableProperty = DependencyProperty.Register(nameof (IsResizable), typeof (bool), typeof (DropDownMenuGroup), new PropertyMetadata((object) false));

  public DropDownMenuGroup()
  {
    this.DefaultStyleKey = (object) typeof (DropDownMenuGroup);
    this.MoreItems = new ObservableCollection<UIElement>();
    this.Loaded += new RoutedEventHandler(this.DropDownMenuGroup_Loaded);
    this.KeyDown += new KeyEventHandler(this.DropDownMenuGroup_KeyDown);
  }

  [Category("Appearance")]
  public bool IconBarEnabled
  {
    get => (bool) this.GetValue(DropDownMenuGroup.IconBarEnabledProperty);
    set => this.SetValue(DropDownMenuGroup.IconBarEnabledProperty, (object) value);
  }

  [Category("Apperance")]
  public bool IsMoreItemsIconTrayEnabled
  {
    get => (bool) this.GetValue(DropDownMenuGroup.IsMoreItemsIconTrayEnabledProperty);
    set => this.SetValue(DropDownMenuGroup.IsMoreItemsIconTrayEnabledProperty, (object) value);
  }

  [Category("Apperance")]
  public ScrollBarVisibility ScrollBarVisibility
  {
    get => (ScrollBarVisibility) this.GetValue(DropDownMenuGroup.ScrollBarVisibilityProperty);
    set => this.SetValue(DropDownMenuGroup.ScrollBarVisibilityProperty, (object) value);
  }

  [Category("Common Properties")]
  public ObservableCollection<UIElement> MoreItems
  {
    get => (ObservableCollection<UIElement>) this.GetValue(DropDownMenuGroup.MoreItemsProperty);
    set => this.SetValue(DropDownMenuGroup.MoreItemsProperty, (object) value);
  }

  [Category("Layout")]
  public bool IsResizable
  {
    get => (bool) this.GetValue(DropDownMenuGroup.IsResizableProperty);
    set => this.SetValue(DropDownMenuGroup.IsResizableProperty, (object) value);
  }

  public override void OnApplyTemplate()
  {
    this.iconContainer = this.GetTemplateChild("IconTray") as Border;
    this.moreItemTrayBar = this.GetTemplateChild("MoreitemBar") as Border;
    this._resizethumb = this.GetTemplateChild("PART_ResizeThumb") as Thumb;
    if (this._resizethumb != null)
      this._resizethumb.DragDelta += new DragDeltaEventHandler(this._resizethumb_DragDelta);
    base.OnApplyTemplate();
  }

  protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
  {
    if (e.Property == ItemsControl.ItemsSourceProperty || e.Property == DropDownMenuGroup.MoreItemsProperty)
      this.ValidateIconBarWidth();
    base.OnPropertyChanged(e);
  }

  private void _resizethumb_DragDelta(object sender, DragDeltaEventArgs e)
  {
    if (double.IsNaN(this.Height))
    {
      if (this.ActualHeight + e.VerticalChange <= 0.0)
        return;
      this.Height = this.ActualHeight + e.VerticalChange;
    }
    else
    {
      if (this.Height + e.VerticalChange <= 0.0)
        return;
      this.Height += e.VerticalChange;
    }
  }

  private void DropDownMenuGroup_KeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key != Key.Escape)
      return;
    if (this.Parent is DropDownButtonAdv)
    {
      ((DropDownButtonAdv) this.Parent).IsDropDownOpen = false;
    }
    else
    {
      if (!(this.Parent is Popup))
        return;
      ((Popup) this.Parent).IsOpen = false;
    }
  }

  private void DropDownMenuGroup_Loaded(object sender, RoutedEventArgs e)
  {
    this.ValidateIconBarWidth();
  }

  private void ValidateIconBarWidth()
  {
    if (this.Items != null)
    {
      for (int index = 0; index < this.Items.Count; ++index)
      {
        if (this.Items[index] is DropDownMenuItem dropDownMenuItem && dropDownMenuItem.IconSize.Width > this.maximumTrayBarWidth)
          this.maximumTrayBarWidth = dropDownMenuItem.IconSize.Width;
      }
    }
    if (this.MoreItems != null)
    {
      for (int index = 0; index < this.MoreItems.Count; ++index)
      {
        if (this.MoreItems[index] is DropDownMenuItem moreItem && moreItem.IconSize.Width > this.maximumTrayBarWidth)
          this.maximumTrayBarWidth = moreItem.IconSize.Width;
      }
    }
    if (this.moreItemTrayBar == null || this.iconContainer == null)
      return;
    this.moreItemTrayBar.Width = this.maximumTrayBarWidth + 10.0;
    this.iconContainer.Width = this.maximumTrayBarWidth + 10.0;
  }

  public void Dispose()
  {
    this.Loaded -= new RoutedEventHandler(this.DropDownMenuGroup_Loaded);
    this.KeyDown -= new KeyEventHandler(this.DropDownMenuGroup_KeyDown);
    if (this._resizethumb != null)
    {
      this._resizethumb.DragDelta -= new DragDeltaEventHandler(this._resizethumb_DragDelta);
      this._resizethumb = (Thumb) null;
    }
    this.MoreItems = (ObservableCollection<UIElement>) null;
    this.ItemsSource = (IEnumerable) null;
    this.Items.Clear();
  }

  protected override System.Windows.Automation.Peers.AutomationPeer OnCreateAutomationPeer()
  {
    return (System.Windows.Automation.Peers.AutomationPeer) new DropDownMenuGroupAutomationPeer(this);
  }
}
