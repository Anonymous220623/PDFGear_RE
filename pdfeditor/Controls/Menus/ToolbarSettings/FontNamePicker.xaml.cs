// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarSettings.FontNamePicker
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Models.Menus.ToolbarSettings;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.Menus.ToolbarSettings;

public partial class FontNamePicker : UserControl, IComponentConnector
{
  private ObservableCollection<string> itemSource;
  public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof (Model), typeof (ToolbarSettingItemFontNameModel), typeof (FontNamePicker), new PropertyMetadata((object) null, new PropertyChangedCallback(FontNamePicker.OnModelPropertyChanged)));
  internal ComboBox comboBox;
  private bool _contentLoaded;

  public FontNamePicker() => this.InitializeComponent();

  public ToolbarSettingItemFontNameModel Model
  {
    get => (ToolbarSettingItemFontNameModel) this.GetValue(FontNamePicker.ModelProperty);
    set => this.SetValue(FontNamePicker.ModelProperty, (object) value);
  }

  private static void OnModelPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is FontNamePicker fontNamePicker))
      return;
    if (e.OldValue != null)
      WeakEventManager<ToolbarSettingItemFontNameModel, EventArgs>.RemoveHandler((ToolbarSettingItemFontNameModel) e.OldValue, "SelectedValueChanged", new EventHandler<EventArgs>(fontNamePicker.Model_SelectedValueChanged));
    if (e.NewValue != null)
      WeakEventManager<ToolbarSettingItemFontNameModel, EventArgs>.AddHandler((ToolbarSettingItemFontNameModel) e.NewValue, "SelectedValueChanged", new EventHandler<EventArgs>(fontNamePicker.Model_SelectedValueChanged));
    fontNamePicker.UpdatePickerComboBoxItems();
    fontNamePicker.UpdateSelectedValue();
  }

  private void Model_SelectedValueChanged(object sender, EventArgs e) => this.UpdateSelectedValue();

  private void UpdateSelectedValue()
  {
    this.comboBox.SelectionChanged -= new SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
    if (this.Model == null)
      this.comboBox.SelectedIndex = -1;
    else if (this.itemSource != null && this.Model.SelectedValue is string selectedValue)
    {
      if (this.itemSource.Count != this.Model.AllFonts.Count)
        this.itemSource.RemoveAt(0);
      if (!this.Model.AllFonts.Contains<string>(selectedValue))
        this.itemSource.Insert(0, selectedValue);
      this.comboBox.SelectedItem = (object) selectedValue;
    }
    this.comboBox.SelectionChanged += new SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
  }

  private void UpdatePickerComboBoxItems()
  {
    this.comboBox.SelectionChanged -= new SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
    if (this.Model == null)
    {
      this.comboBox.ItemsSource = (IEnumerable) null;
      this.itemSource = (ObservableCollection<string>) null;
    }
    else
    {
      this.itemSource = new ObservableCollection<string>((IEnumerable<string>) this.Model.AllFonts);
      this.comboBox.ItemsSource = (IEnumerable) this.itemSource;
    }
    this.comboBox.SelectionChanged += new SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
  }

  private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (this.Model == null)
      return;
    string str = e.AddedItems.OfType<string>().FirstOrDefault<string>();
    if (str == null)
      return;
    this.Model.SelectedValue = (object) str;
    this.Model.ExecuteCommand();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/menus/toolbarsettings/fontnamepicker.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId == 1)
      this.comboBox = (ComboBox) target;
    else
      this._contentLoaded = true;
  }
}
