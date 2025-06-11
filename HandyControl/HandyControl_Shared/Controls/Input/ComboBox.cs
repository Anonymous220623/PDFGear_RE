// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.ComboBox
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using HandyControl.Interactivity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Controls;

[TemplatePart(Name = "PART_AutoCompletePanel", Type = typeof (Panel))]
[TemplatePart(Name = "PART_EditableTextBox", Type = typeof (System.Windows.Controls.TextBox))]
[TemplatePart(Name = "PART_Popup_AutoComplete", Type = typeof (Popup))]
public class ComboBox : System.Windows.Controls.ComboBox
{
  private bool _isAutoCompleteAction = true;
  private Panel _autoCompletePanel;
  private System.Windows.Controls.TextBox _editableTextBox;
  private Popup _autoPopupAutoComplete;
  private const string AutoCompletePanel = "PART_AutoCompletePanel";
  private const string AutoPopupAutoComplete = "PART_Popup_AutoComplete";
  private const string EditableTextBox = "PART_EditableTextBox";
  public static readonly DependencyProperty AutoCompleteProperty = DependencyProperty.Register(nameof (AutoComplete), typeof (bool), typeof (ComboBox), new PropertyMetadata(ValueBoxes.FalseBox, new PropertyChangedCallback(ComboBox.OnAutoCompleteChanged)));
  internal static readonly DependencyProperty SearchTextProperty = DependencyProperty.Register(nameof (SearchText), typeof (string), typeof (ComboBox), new PropertyMetadata((object) null, new PropertyChangedCallback(ComboBox.OnSearchTextChanged)));
  public static readonly DependencyProperty SelectionBrushProperty = TextBoxBase.SelectionBrushProperty.AddOwner(typeof (ComboBox));
  public static readonly DependencyProperty SelectionOpacityProperty = TextBoxBase.SelectionOpacityProperty.AddOwner(typeof (ComboBox));
  public static readonly DependencyProperty CaretBrushProperty = TextBoxBase.CaretBrushProperty.AddOwner(typeof (ComboBox));

  public ComboBox()
  {
    this.CommandBindings.Add(new CommandBinding((ICommand) ControlCommands.Clear, (ExecutedRoutedEventHandler) ((s, e) =>
    {
      if (this.IsReadOnly)
        return;
      this.SetCurrentValue(Selector.SelectedValueProperty, (object) null);
      this.SetCurrentValue(Selector.SelectedItemProperty, (object) null);
      this.SetCurrentValue(Selector.SelectedIndexProperty, (object) -1);
      this.SetCurrentValue(System.Windows.Controls.ComboBox.TextProperty, (object) "");
    })));
  }

  public override void OnApplyTemplate()
  {
    if (this._editableTextBox != null)
    {
      BindingOperations.ClearBinding((DependencyObject) this._editableTextBox, System.Windows.Controls.TextBox.TextProperty);
      this._editableTextBox.GotFocus -= new RoutedEventHandler(this.EditableTextBox_GotFocus);
      this._editableTextBox.LostFocus -= new RoutedEventHandler(this.EditableTextBox_LostFocus);
    }
    base.OnApplyTemplate();
    if (!this.IsEditable)
      return;
    this._editableTextBox = this.GetTemplateChild("PART_EditableTextBox") as System.Windows.Controls.TextBox;
    if (this._editableTextBox == null)
      return;
    this._editableTextBox.SetBinding(ComboBox.SelectionBrushProperty, (BindingBase) new Binding(ComboBox.SelectionBrushProperty.Name)
    {
      Source = (object) this
    });
    this._editableTextBox.SetBinding(ComboBox.SelectionOpacityProperty, (BindingBase) new Binding(ComboBox.SelectionOpacityProperty.Name)
    {
      Source = (object) this
    });
    this._editableTextBox.SetBinding(ComboBox.CaretBrushProperty, (BindingBase) new Binding(ComboBox.CaretBrushProperty.Name)
    {
      Source = (object) this
    });
    if (!this.AutoComplete)
      return;
    this._autoPopupAutoComplete = this.GetTemplateChild("PART_Popup_AutoComplete") as Popup;
    this._autoCompletePanel = this.GetTemplateChild("PART_AutoCompletePanel") as Panel;
    System.Windows.Controls.TextBox editableTextBox = this._editableTextBox;
    DependencyProperty textProperty = System.Windows.Controls.TextBox.TextProperty;
    Binding binding = new Binding(ComboBox.SearchTextProperty.Name);
    binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
    binding.Mode = BindingMode.OneWayToSource;
    binding.Delay = 500;
    binding.Source = (object) this;
    editableTextBox.SetBinding(textProperty, (BindingBase) binding);
    this._editableTextBox.GotFocus += new RoutedEventHandler(this.EditableTextBox_GotFocus);
    this._editableTextBox.LostFocus += new RoutedEventHandler(this.EditableTextBox_LostFocus);
  }

  private void EditableTextBox_LostFocus(object sender, RoutedEventArgs e)
  {
    if (this._autoPopupAutoComplete == null)
      return;
    this._autoPopupAutoComplete.IsOpen = false;
  }

  protected override void OnDropDownClosed(EventArgs e)
  {
    base.OnDropDownClosed(e);
    this._isAutoCompleteAction = false;
  }

  private void EditableTextBox_GotFocus(object sender, RoutedEventArgs e)
  {
    if (this._autoPopupAutoComplete == null || this._editableTextBox == null || string.IsNullOrEmpty(this._editableTextBox.Text))
      return;
    this._autoPopupAutoComplete.IsOpen = true;
  }

  protected override void OnSelectionChanged(SelectionChangedEventArgs e)
  {
    this._isAutoCompleteAction = false;
    base.OnSelectionChanged(e);
  }

  public Func<ItemCollection, object, IEnumerable<object>> SearchFunc { get; set; }

  private static void OnAutoCompleteChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ComboBox comboBox = (ComboBox) d;
    if (comboBox._editableTextBox == null)
      return;
    comboBox.UpdateSearchItems(comboBox._editableTextBox.Text);
  }

  public bool AutoComplete
  {
    get => (bool) this.GetValue(ComboBox.AutoCompleteProperty);
    set => this.SetValue(ComboBox.AutoCompleteProperty, ValueBoxes.BooleanBox(value));
  }

  private static void OnSearchTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ComboBox comboBox = (ComboBox) d;
    if (comboBox._isAutoCompleteAction)
      comboBox.UpdateSearchItems(e.NewValue as string);
    comboBox._isAutoCompleteAction = true;
  }

  internal string SearchText
  {
    get => (string) this.GetValue(ComboBox.SearchTextProperty);
    set => this.SetValue(ComboBox.SearchTextProperty, (object) value);
  }

  public Brush SelectionBrush
  {
    get => (Brush) this.GetValue(ComboBox.SelectionBrushProperty);
    set => this.SetValue(ComboBox.SelectionBrushProperty, (object) value);
  }

  public double SelectionOpacity
  {
    get => (double) this.GetValue(ComboBox.SelectionOpacityProperty);
    set => this.SetValue(ComboBox.SelectionOpacityProperty, (object) value);
  }

  public Brush CaretBrush
  {
    get => (Brush) this.GetValue(ComboBox.CaretBrushProperty);
    set => this.SetValue(ComboBox.CaretBrushProperty, (object) value);
  }

  private void UpdateSearchItems(string key)
  {
    if (this._editableTextBox == null || this._autoPopupAutoComplete == null)
      return;
    this._autoPopupAutoComplete.IsOpen = !string.IsNullOrEmpty(key);
    this._autoCompletePanel.Children.Clear();
    if (this.SearchFunc == null)
    {
      if (string.IsNullOrEmpty(key))
        return;
      foreach (object content in (IEnumerable) this.Items)
      {
        string str = content?.ToString();
        if (str != null && str.Contains(key))
          this._autoCompletePanel.Children.Add((UIElement) this.CreateSearchItem(content));
      }
    }
    else
    {
      foreach (object content in this.SearchFunc(this.Items, (object) key))
        this._autoCompletePanel.Children.Add((UIElement) this.CreateSearchItem(content));
    }
  }

  private ComboBoxItem CreateSearchItem(object content)
  {
    ComboBoxItem searchItem = new ComboBoxItem();
    searchItem.Content = content;
    searchItem.Style = this.ItemContainerStyle;
    searchItem.ContentTemplate = this.ItemTemplate;
    searchItem.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(this.AutoCompleteItem_PreviewMouseLeftButtonDown);
    return searchItem;
  }

  private void AutoCompleteItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (!(sender is ComboBoxItem comboBoxItem))
      return;
    if (this._autoPopupAutoComplete != null)
      this._autoPopupAutoComplete.IsOpen = false;
    this._isAutoCompleteAction = false;
    this.SelectedValue = comboBoxItem.Content;
  }
}
