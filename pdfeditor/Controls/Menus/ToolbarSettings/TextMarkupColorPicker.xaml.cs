// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarSettings.TextMarkupColorPicker
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using pdfeditor.Controls.ColorPickers;
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
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Menus.ToolbarSettings;

public partial class TextMarkupColorPicker : UserControl, IComponentConnector
{
  private ObservableCollection<TextMarkupColorPicker.NestedItemsControlModel> standardColors;
  private ObservableCollection<TextMarkupColorPicker.NestedItemsControlModel> recentColors;
  public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof (Model), typeof (ToolbarSettingItemColorModel), typeof (TextMarkupColorPicker), new PropertyMetadata((object) null, new PropertyChangedCallback(TextMarkupColorPicker.OnModelPropertyChanged)));
  internal ItemsControl StandardColorItemsControl;
  internal ItemsControl RecentColorItemsControl;
  private bool _contentLoaded;

  public TextMarkupColorPicker()
  {
    this.InitializeComponent();
    this.standardColors = new ObservableCollection<TextMarkupColorPicker.NestedItemsControlModel>();
    this.recentColors = new ObservableCollection<TextMarkupColorPicker.NestedItemsControlModel>();
  }

  public ToolbarSettingItemColorModel Model
  {
    get => (ToolbarSettingItemColorModel) this.GetValue(TextMarkupColorPicker.ModelProperty);
    set => this.SetValue(TextMarkupColorPicker.ModelProperty, (object) value);
  }

  private static void OnModelPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is TextMarkupColorPicker markupColorPicker))
      return;
    if (e.OldValue != null)
    {
      WeakEventManager<ToolbarSettingItemColorModel, EventArgs>.RemoveHandler((ToolbarSettingItemColorModel) e.OldValue, "SelectedValueChanged", new EventHandler<EventArgs>(markupColorPicker.Model_SelectedValueChanged));
      WeakEventManager<ToolbarSettingItemColorModel, EventArgs>.RemoveHandler((ToolbarSettingItemColorModel) e.OldValue, "ColorsChanged", new EventHandler<EventArgs>(markupColorPicker.Model_ColorsChanged));
    }
    if (e.NewValue != null)
    {
      WeakEventManager<ToolbarSettingItemColorModel, EventArgs>.AddHandler((ToolbarSettingItemColorModel) e.NewValue, "SelectedValueChanged", new EventHandler<EventArgs>(markupColorPicker.Model_SelectedValueChanged));
      WeakEventManager<ToolbarSettingItemColorModel, EventArgs>.AddHandler((ToolbarSettingItemColorModel) e.NewValue, "ColorsChanged", new EventHandler<EventArgs>(markupColorPicker.Model_ColorsChanged));
    }
    markupColorPicker.UpdateColorCollection();
  }

  private void Model_SelectedValueChanged(object sender, EventArgs e)
  {
    this.UpdateRadioButtonCheckedState();
  }

  private void Model_ColorsChanged(object sender, EventArgs e) => this.UpdateColorCollection();

  private void ColorPickerButton_ItemClick(object sender, ColorPickerButtonItemClickEventArgs e)
  {
    if (this.Model == null)
      return;
    Color color = e.Item.Color;
    this.Model.SelectedValue = (object) $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
    this.Model.ExecuteCommand();
  }

  private void UpdateColorCollection()
  {
    if (this.Model != null)
    {
      this.StandardColorItemsControl.ItemsSource = (IEnumerable) null;
      this.RecentColorItemsControl.ItemsSource = (IEnumerable) null;
      this.standardColors.Clear();
      this.recentColors.Clear();
      if (this.Model.StandardColors != null)
      {
        foreach (string standardColor in (Collection<string>) this.Model.StandardColors)
          this.standardColors.Add(new TextMarkupColorPicker.NestedItemsControlModel(standardColor, this.Model));
      }
      if (this.Model.RecentColors != null)
      {
        foreach (string recentColor in (Collection<string>) this.Model.RecentColors)
          this.recentColors.Add(new TextMarkupColorPicker.NestedItemsControlModel(recentColor, this.Model));
      }
      this.StandardColorItemsControl.ItemsSource = (IEnumerable) this.standardColors;
      this.RecentColorItemsControl.ItemsSource = (IEnumerable) this.recentColors;
    }
    this.UpdateRadioButtonCheckedState();
  }

  private void UpdateRadioButtonCheckedState()
  {
    string selectedValue = this.Model?.SelectedValue as string;
    Color color1;
    try
    {
      if (string.IsNullOrEmpty(selectedValue))
        return;
      color1 = (Color) ColorConverter.ConvertFromString(selectedValue);
    }
    catch
    {
      return;
    }
    foreach (TextMarkupColorPicker.NestedItemsControlModel itemsControlModel in this.standardColors.Concat<TextMarkupColorPicker.NestedItemsControlModel>((IEnumerable<TextMarkupColorPicker.NestedItemsControlModel>) this.recentColors))
    {
      try
      {
        Color color2 = (Color) ColorConverter.ConvertFromString(itemsControlModel.Value);
        itemsControlModel.IsChecked = color2 == color1;
      }
      catch
      {
        itemsControlModel.IsChecked = false;
      }
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/menus/toolbarsettings/textmarkupcolorpicker.xaml", UriKind.Relative));
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
    if (connectionId != 1)
    {
      if (connectionId == 2)
        this.RecentColorItemsControl = (ItemsControl) target;
      else
        this._contentLoaded = true;
    }
    else
      this.StandardColorItemsControl = (ItemsControl) target;
  }

  private class NestedItemsControlModel : ObservableObject
  {
    private bool isChecked;

    public NestedItemsControlModel(string value, ToolbarSettingItemColorModel model)
    {
      this.Value = value;
      this.Model = model;
      this.Command = (ICommand) new RelayCommand((Action) (() =>
      {
        this.Model.SelectedValue = (object) this.Value;
        this.Model.ExecuteCommand();
      }));
    }

    public string Value { get; }

    public ToolbarSettingItemColorModel Model { get; }

    public ICommand Command { get; }

    public bool IsChecked
    {
      get => this.isChecked;
      set => this.SetProperty<bool>(ref this.isChecked, value, nameof (IsChecked));
    }
  }
}
