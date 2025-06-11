// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.AutoCompleteTextBox
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Tools.Helper;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_SearchTextBox", Type = typeof (System.Windows.Controls.TextBox))]
public class AutoCompleteTextBox : ComboBox
{
  private const string SearchTextBox = "PART_SearchTextBox";
  private bool ignoreTextChanging;
  private System.Windows.Controls.TextBox _searchTextBox;
  private object _selectedItem;

  static AutoCompleteTextBox()
  {
    System.Windows.Controls.ComboBox.TextProperty.OverrideMetadata(typeof (AutoCompleteTextBox), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
  }

  public override void OnApplyTemplate()
  {
    if (this._searchTextBox != null)
    {
      this._searchTextBox.GotFocus -= new RoutedEventHandler(this.SearchTextBoxGotFocus);
      this._searchTextBox.KeyDown -= new KeyEventHandler(this.SearchTextBoxKeyDown);
      this._searchTextBox.TextChanged -= new TextChangedEventHandler(this.SearchTextBoxTextChanged);
    }
    base.OnApplyTemplate();
    this._searchTextBox = this.GetTemplateChild("PART_SearchTextBox") as System.Windows.Controls.TextBox;
    if (this._searchTextBox != null)
    {
      this._searchTextBox.GotFocus += new RoutedEventHandler(this.SearchTextBoxGotFocus);
      this._searchTextBox.PreviewKeyDown += new KeyEventHandler(this.SearchTextBoxKeyDown);
      this._searchTextBox.TextChanged += new TextChangedEventHandler(this.SearchTextBoxTextChanged);
    }
    this.UpdateTextBoxBySelectedItem(this._selectedItem);
  }

  protected override void OnSelectionChanged(SelectionChangedEventArgs e)
  {
    base.OnSelectionChanged(e);
    if (e.AddedItems.Count <= 0)
      return;
    this._selectedItem = e.AddedItems[0];
    this.UpdateTextBoxBySelectedItem(this._selectedItem);
  }

  protected override bool IsItemItsOwnContainerOverride(object item)
  {
    return item is AutoCompleteTextBoxItem;
  }

  protected override DependencyObject GetContainerForItemOverride()
  {
    return (DependencyObject) new AutoCompleteTextBoxItem();
  }

  private void SearchTextBoxTextChanged(object sender, TextChangedEventArgs e)
  {
    this._selectedItem = (object) null;
    this.SelectedIndex = -1;
    if (this.ignoreTextChanging)
    {
      this.ignoreTextChanging = false;
    }
    else
    {
      this.Text = this._searchTextBox.Text;
      if (string.IsNullOrEmpty(this.Text))
      {
        this.SetCurrentValue(System.Windows.Controls.ComboBox.IsDropDownOpenProperty, ValueBoxes.FalseBox);
        this._searchTextBox.Focus();
      }
      else
      {
        if (!this._searchTextBox.IsFocused)
          return;
        this.SetCurrentValue(System.Windows.Controls.ComboBox.IsDropDownOpenProperty, ValueBoxes.TrueBox);
      }
    }
  }

  private void SearchTextBoxKeyDown(object sender, KeyEventArgs e)
  {
    if (e.Key == Key.Up)
    {
      int selectedIndex = this.SelectedIndex - 1;
      if (selectedIndex < 0)
        selectedIndex = this.Items.Count - 1;
      this.UpdateTextBoxBySelectedIndex(selectedIndex);
    }
    else if (e.Key == Key.Down)
    {
      int selectedIndex = this.SelectedIndex + 1;
      if (selectedIndex >= this.Items.Count)
        selectedIndex = 0;
      this.UpdateTextBoxBySelectedIndex(selectedIndex);
    }
    else
    {
      if (e.Key != Key.Return)
        return;
      this.SetCurrentValue(System.Windows.Controls.ComboBox.IsDropDownOpenProperty, ValueBoxes.FalseBox);
      e.Handled = true;
    }
  }

  private void UpdateTextBoxBySelectedIndex(int selectedIndex)
  {
    if (this._searchTextBox == null)
      return;
    this.ignoreTextChanging = true;
    if (!(this.ItemContainerGenerator.ContainerFromIndex(selectedIndex) is AutoCompleteTextBoxItem completeTextBoxItem))
      return;
    this._searchTextBox.Text = BindingHelper.GetString(completeTextBoxItem.Content, this.DisplayMemberPath);
    this._searchTextBox.CaretIndex = this._searchTextBox.Text.Length;
    this.SelectedIndex = selectedIndex;
  }

  private void UpdateTextBoxBySelectedItem(object selectedItem)
  {
    if (this._searchTextBox == null)
      return;
    this.ignoreTextChanging = true;
    this._searchTextBox.Text = BindingHelper.GetString(selectedItem, this.DisplayMemberPath);
    this._searchTextBox.CaretIndex = this._searchTextBox.Text.Length;
    this.ignoreTextChanging = true;
    this.Text = this._searchTextBox.Text;
    this.ignoreTextChanging = false;
  }

  private void SearchTextBoxGotFocus(object sender, RoutedEventArgs e)
  {
    if (string.IsNullOrEmpty(this.Text))
      return;
    this.SetCurrentValue(System.Windows.Controls.ComboBox.IsDropDownOpenProperty, ValueBoxes.TrueBox);
  }
}
