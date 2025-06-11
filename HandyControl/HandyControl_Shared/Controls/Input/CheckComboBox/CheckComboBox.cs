// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.CheckComboBox
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_Panel", Type = typeof (Panel))]
[TemplatePart(Name = "PART_SelectAll", Type = typeof (CheckComboBoxItem))]
public class CheckComboBox : ListBox
{
  private const string ElementPanel = "PART_Panel";
  private const string ElementSelectAll = "PART_SelectAll";
  private Panel _panel;
  private CheckComboBoxItem _selectAllItem;
  private bool _isInternalAction;
  public static readonly DependencyProperty MaxDropDownHeightProperty = System.Windows.Controls.ComboBox.MaxDropDownHeightProperty.AddOwner(typeof (CheckComboBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) (SystemParameters.PrimaryScreenHeight / 3.0)));
  public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(nameof (IsDropDownOpen), typeof (bool), typeof (CheckComboBox), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(CheckComboBox.OnIsDropDownOpenChanged)));
  public static readonly DependencyProperty TagStyleProperty = DependencyProperty.Register(nameof (TagStyle), typeof (Style), typeof (CheckComboBox), new PropertyMetadata((object) null));
  public static readonly DependencyProperty TagSpacingProperty = DependencyProperty.Register(nameof (TagSpacing), typeof (double), typeof (CheckComboBox), new PropertyMetadata(ValueBoxes.Double0Box));
  public static readonly DependencyProperty ShowSelectAllButtonProperty = DependencyProperty.Register(nameof (ShowSelectAllButton), typeof (bool), typeof (CheckComboBox), new PropertyMetadata(ValueBoxes.FalseBox));

  [Bindable(true)]
  [Category("Layout")]
  [TypeConverter(typeof (LengthConverter))]
  public double MaxDropDownHeight
  {
    get => (double) this.GetValue(CheckComboBox.MaxDropDownHeightProperty);
    set => this.SetValue(CheckComboBox.MaxDropDownHeightProperty, (object) value);
  }

  private static void OnIsDropDownOpenChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    CheckComboBox checkComboBox = (CheckComboBox) d;
    if ((bool) e.NewValue)
      return;
    checkComboBox.Dispatcher.BeginInvoke((Delegate) (() => Mouse.Capture((IInputElement) null)), DispatcherPriority.Send);
  }

  public bool IsDropDownOpen
  {
    get => (bool) this.GetValue(CheckComboBox.IsDropDownOpenProperty);
    set => this.SetValue(CheckComboBox.IsDropDownOpenProperty, ValueBoxes.BooleanBox(value));
  }

  public Style TagStyle
  {
    get => (Style) this.GetValue(CheckComboBox.TagStyleProperty);
    set => this.SetValue(CheckComboBox.TagStyleProperty, (object) value);
  }

  public double TagSpacing
  {
    get => (double) this.GetValue(CheckComboBox.TagSpacingProperty);
    set => this.SetValue(CheckComboBox.TagSpacingProperty, (object) value);
  }

  public bool ShowSelectAllButton
  {
    get => (bool) this.GetValue(CheckComboBox.ShowSelectAllButtonProperty);
    set => this.SetValue(CheckComboBox.ShowSelectAllButtonProperty, ValueBoxes.BooleanBox(value));
  }

  public CheckComboBox()
  {
    this.AddHandler(HandyControl.Controls.Tag.ClosedEvent, (Delegate) new RoutedEventHandler(this.Tags_OnClosed));
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Clear, (ExecutedRoutedEventHandler) ((s, e) =>
    {
      this.SetCurrentValue(Selector.SelectedValueProperty, (object) null);
      this.SetCurrentValue(Selector.SelectedItemProperty, (object) null);
      this.SetCurrentValue(Selector.SelectedIndexProperty, (object) -1);
      this.SelectedItems.Clear();
    })));
  }

  public override void OnApplyTemplate()
  {
    if (this._selectAllItem != null)
    {
      this._selectAllItem.Selected -= new RoutedEventHandler(this.SelectAllItem_Selected);
      this._selectAllItem.Unselected -= new RoutedEventHandler(this.SelectAllItem_Unselected);
    }
    base.OnApplyTemplate();
    this._panel = this.GetTemplateChild("PART_Panel") as Panel;
    this._selectAllItem = this.GetTemplateChild("PART_SelectAll") as CheckComboBoxItem;
    if (this._selectAllItem != null)
    {
      this._selectAllItem.Selected += new RoutedEventHandler(this.SelectAllItem_Selected);
      this._selectAllItem.Unselected += new RoutedEventHandler(this.SelectAllItem_Unselected);
    }
    this.UpdateTags();
  }

  protected override void OnSelectionChanged(SelectionChangedEventArgs e)
  {
    this.UpdateTags();
    base.OnSelectionChanged(e);
  }

  protected override bool IsItemItsOwnContainerOverride(object item) => item is CheckComboBoxItem;

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new CheckComboBoxItem();
  }

  protected override void OnDisplayMemberPathChanged(
    string oldDisplayMemberPath,
    string newDisplayMemberPath)
  {
    this.UpdateTags();
  }

  private void Tags_OnClosed(object sender, RoutedEventArgs e)
  {
    if (!(e.OriginalSource is HandyControl.Controls.Tag originalSource))
      return;
    this.SelectedItems.Remove(originalSource.Tag);
    this._panel.Children.Remove((UIElement) originalSource);
  }

  private void SwitchAllItems(bool selected)
  {
    if (this._isInternalAction)
      return;
    this._isInternalAction = true;
    if (!selected)
      this.UnselectAll();
    else
      this.SelectAll();
    this._isInternalAction = false;
    this.UpdateTags();
  }

  private void SelectAllItem_Selected(object sender, RoutedEventArgs e)
  {
    this.SwitchAllItems(true);
  }

  private void SelectAllItem_Unselected(object sender, RoutedEventArgs e)
  {
    this.SwitchAllItems(false);
  }

  private void UpdateTags()
  {
    if (this._panel == null || this._isInternalAction)
      return;
    if (this._selectAllItem != null)
    {
      this._isInternalAction = true;
      this._selectAllItem.SetCurrentValue(Selector.IsSelectedProperty, (object) (bool) (this.Items.Count <= 0 ? 0 : (this.SelectedItems.Count == this.Items.Count ? 1 : 0)));
      this._isInternalAction = false;
    }
    this._panel.Children.Clear();
    foreach (object selectedItem in (IEnumerable) this.SelectedItems)
    {
      HandyControl.Controls.Tag tag = new HandyControl.Controls.Tag();
      tag.Style = this.TagStyle;
      tag.Tag = selectedItem;
      HandyControl.Controls.Tag element = tag;
      if (this.ItemsSource != null)
        element.SetBinding(ContentControl.ContentProperty, (BindingBase) new Binding(this.DisplayMemberPath)
        {
          Source = selectedItem
        });
      else
        element.Content = this.IsItemItsOwnContainerOverride(selectedItem) ? ((ContentControl) selectedItem).Content : selectedItem;
      this._panel.Children.Add((UIElement) element);
    }
  }
}
