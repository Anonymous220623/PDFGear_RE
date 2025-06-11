// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarSettings.FontSizePicker
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Models.Menus.ToolbarSettings;
using pdfeditor.Utils.Behaviors;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.Menus.ToolbarSettings;

public partial class FontSizePicker : UserControl, IComponentConnector
{
  private ObservableCollection<string> itemSource;
  public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof (Model), typeof (ToolbarSettingItemFontSizeModel), typeof (FontSizePicker), new PropertyMetadata((object) null, new PropertyChangedCallback(FontSizePicker.OnModelPropertyChanged)));
  internal Border border;
  internal Image fontSizeIcon;
  internal TextBox textBox;
  internal TextBoxEditBehavior _TextBoxEditBehavior;
  internal Button comboBoxDropButton;
  internal ComboBox comboBox;
  private bool _contentLoaded;

  public FontSizePicker()
  {
    this.InitializeComponent();
    this.textBox.IsUndoEnabled = false;
  }

  public ToolbarSettingItemFontSizeModel Model
  {
    get => (ToolbarSettingItemFontSizeModel) this.GetValue(FontSizePicker.ModelProperty);
    set => this.SetValue(FontSizePicker.ModelProperty, (object) value);
  }

  private static void OnModelPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is FontSizePicker fontSizePicker))
      return;
    if (e.OldValue != null)
    {
      WeakEventManager<ToolbarSettingItemFontSizeModel, EventArgs>.RemoveHandler((ToolbarSettingItemFontSizeModel) e.OldValue, "SelectedValueChanged", new EventHandler<EventArgs>(fontSizePicker.Model_SelectedValueChanged));
      WeakEventManager<ToolbarSettingItemFontSizeModel, EventArgs>.RemoveHandler((ToolbarSettingItemFontSizeModel) e.OldValue, "StandardItemsChanged", new EventHandler<EventArgs>(fontSizePicker.Model_StandardItemsChanged));
    }
    if (e.NewValue != null)
    {
      WeakEventManager<ToolbarSettingItemFontSizeModel, EventArgs>.AddHandler((ToolbarSettingItemFontSizeModel) e.NewValue, "SelectedValueChanged", new EventHandler<EventArgs>(fontSizePicker.Model_SelectedValueChanged));
      WeakEventManager<ToolbarSettingItemFontSizeModel, EventArgs>.AddHandler((ToolbarSettingItemFontSizeModel) e.NewValue, "StandardItemsChanged", new EventHandler<EventArgs>(fontSizePicker.Model_StandardItemsChanged));
    }
    fontSizePicker.UpdatePickerComboBoxItems();
    fontSizePicker.UpdateSelectedValue();
  }

  private void Model_SelectedValueChanged(object sender, EventArgs e) => this.UpdateSelectedValue();

  private void Model_StandardItemsChanged(object sender, EventArgs e)
  {
    this.UpdatePickerComboBoxItems();
  }

  private void UpdateSelectedValue()
  {
    if (this.Model?.SelectedValue is float selectedValue)
      this._TextBoxEditBehavior.Text = $"{selectedValue} pt";
    else
      this._TextBoxEditBehavior.Text = (string) null;
  }

  private void UpdatePickerComboBoxItems()
  {
    this.comboBox.SelectionChanged -= new SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
    this.itemSource = (ObservableCollection<string>) null;
    if (this.Model?.StandardItems == null || this.Model.StandardItems.Count == 0)
    {
      this.comboBox.Visibility = Visibility.Collapsed;
      this.comboBoxDropButton.Visibility = Visibility.Collapsed;
      this.textBox.Padding = new Thickness(0.0);
    }
    else
    {
      this.comboBox.Visibility = Visibility.Visible;
      this.comboBoxDropButton.Visibility = Visibility.Visible;
      this.textBox.Padding = new Thickness(0.0, 0.0, 20.0, 0.0);
      this.itemSource = new ObservableCollection<string>(this.Model.StandardItems.Select<float, string>((Func<float, string>) (c => $"{c} pt")));
    }
    this.comboBox.ItemsSource = (IEnumerable) this.itemSource;
    this.comboBox.SelectionChanged += new SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
  }

  private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (this.Model == null || e.AddedItems.Count <= 0)
      return;
    this._TextBoxEditBehavior.Text = (string) e.AddedItems[0];
    ((Selector) sender).SelectedItem = (object) null;
  }

  private void FontSizeIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    this.comboBox.IsDropDownOpen = true;
  }

  private void _TextBoxEditBehavior_TextChanged(object sender, EventArgs e)
  {
    float? nullable = new float?();
    if (this.Model.SelectedValue is float selectedValue)
      nullable = new float?(selectedValue);
    float num;
    if (this.TryParseFontSize(this._TextBoxEditBehavior.Text, out num))
    {
      if (nullable.HasValue && (double) nullable.Value == (double) num)
        return;
      this.Model.SelectedValue = (object) num;
      this.Model.ExecuteCommand();
    }
    else if (nullable.HasValue)
      this._TextBoxEditBehavior.Text = $"{nullable.Value} pt";
    else
      this._TextBoxEditBehavior.Text = "";
  }

  private bool TryParseFontSize(string text, out float value)
  {
    value = 0.0f;
    text = text?.Trim();
    if (string.IsNullOrEmpty(text))
      return false;
    if (text.Length > 2 && (text[text.Length - 2] == 'p' || text[text.Length - 2] == 'P') && (text[text.Length - 1] == 't' || text[text.Length - 1] == 'T'))
      text = text.Substring(0, text.Length - 2).Trim();
    return float.TryParse(text, NumberStyles.Number, (IFormatProvider) NumberFormatInfo.CurrentInfo, out value) || float.TryParse(text, NumberStyles.Number, (IFormatProvider) CultureInfo.InvariantCulture, out value);
  }

  private void comboBoxDropButton_Click(object sender, RoutedEventArgs e)
  {
    this.comboBox.IsDropDownOpen = true;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/menus/toolbarsettings/fontsizepicker.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.border = (Border) target;
        break;
      case 2:
        this.fontSizeIcon = (Image) target;
        this.fontSizeIcon.MouseLeftButtonDown += new MouseButtonEventHandler(this.FontSizeIcon_MouseLeftButtonDown);
        break;
      case 3:
        this.textBox = (TextBox) target;
        break;
      case 4:
        this._TextBoxEditBehavior = (TextBoxEditBehavior) target;
        break;
      case 5:
        this.comboBoxDropButton = (Button) target;
        this.comboBoxDropButton.Click += new RoutedEventHandler(this.comboBoxDropButton_Click);
        break;
      case 6:
        this.comboBox = (ComboBox) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
