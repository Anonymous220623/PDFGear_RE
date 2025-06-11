// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarButtonContextMenuItem
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Controls.ColorPickers;
using pdfeditor.Models.Menus;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Menus;

public class ToolbarButtonContextMenuItem : MenuItem
{
  public static readonly DependencyProperty IsToggleEnabledProperty = DependencyProperty.Register(nameof (IsToggleEnabled), typeof (bool), typeof (ToolbarButtonContextMenuItem), new PropertyMetadata((object) false));

  static ToolbarButtonContextMenuItem()
  {
    FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (ToolbarButtonContextMenuItem), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (ToolbarButtonContextMenuItem)));
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    if (!(this.DataContext is ContextMenuItemModel dataContext) || !object.Equals(dataContext.TagData?.MenuItemValue, (object) "ColorPicker"))
      return;
    ColorPicker templateChild = this.GetTemplateChild("ColorPicker") as ColorPicker;
    templateChild.ItemClick += new EventHandler<ColorPickerItemClickEventArgs>(this.ColorPicker_ItemClick);
    if (!(dataContext.Parent is ColorMoreItemContextMenuItemModel parent))
      return;
    templateChild.RecentColorsKey = parent.RecentColorsKey;
    templateChild.DefaultColor = parent.DefaultColor;
    templateChild.Tag = this.DataContext;
  }

  private void ColorPicker_ItemClick(object sender, ColorPickerItemClickEventArgs e)
  {
    if (!(((FrameworkElement) sender).Tag is ContextMenuItemModel tag) || !(tag.Parent is ColorMoreItemContextMenuItemModel parent1))
      return;
    parent1.TagData.MenuItemValue = (object) $"#{e.Item.Color.A:X2}{e.Item.Color.R:X2}{e.Item.Color.G:X2}{e.Item.Color.B:X2}";
    parent1.Command.Execute((object) parent1);
    FrameworkElement reference = (FrameworkElement) this;
    while (reference != null)
    {
      if (!(reference.Parent is FrameworkElement parent2))
        parent2 = VisualTreeHelper.GetParent((DependencyObject) reference) as FrameworkElement;
      reference = parent2;
      if (reference is ContextMenu contextMenu)
      {
        contextMenu.IsOpen = false;
        break;
      }
    }
  }

  public bool IsToggleEnabled
  {
    get => (bool) this.GetValue(ToolbarButtonContextMenuItem.IsToggleEnabledProperty);
    set => this.SetValue(ToolbarButtonContextMenuItem.IsToggleEnabledProperty, (object) value);
  }

  protected override void OnClick()
  {
    bool isChecked = this.IsChecked;
    base.OnClick();
    if (((this.IsToggleEnabled ? 0 : (this.IsCheckable ? 1 : 0)) & (isChecked ? 1 : 0)) == 0 || this.IsChecked)
      return;
    this.IsChecked = true;
  }

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new ToolbarButtonContextMenuItem();
  }

  protected override bool IsItemItsOwnContainerOverride(object item)
  {
    return item is ToolbarButtonContextMenuItem;
  }
}
