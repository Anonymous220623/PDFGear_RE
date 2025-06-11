// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarSettings.StrokeThicknessPicker
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using pdfeditor.Models.Menus.ToolbarSettings;
using pdfeditor.ViewModels;
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

#nullable disable
namespace pdfeditor.Controls.Menus.ToolbarSettings;

public partial class StrokeThicknessPicker : UserControl, IComponentConnector
{
  public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof (Model), typeof (ToolbarSettingItemStrokeThicknessModel), typeof (StrokeThicknessPicker), new PropertyMetadata((object) null, new PropertyChangedCallback(StrokeThicknessPicker.OnModelPropertyChanged)));
  internal Image thicknessIcon;
  internal ComboBox comboBox;
  private bool _contentLoaded;

  public StrokeThicknessPicker() => this.InitializeComponent();

  public ToolbarSettingItemStrokeThicknessModel Model
  {
    get
    {
      return (ToolbarSettingItemStrokeThicknessModel) this.GetValue(StrokeThicknessPicker.ModelProperty);
    }
    set => this.SetValue(StrokeThicknessPicker.ModelProperty, (object) value);
  }

  private static void OnModelPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is StrokeThicknessPicker strokeThicknessPicker))
      return;
    if (e.OldValue != null)
    {
      WeakEventManager<ToolbarSettingItemStrokeThicknessModel, EventArgs>.RemoveHandler((ToolbarSettingItemStrokeThicknessModel) e.OldValue, "SelectedValueChanged", new EventHandler<EventArgs>(strokeThicknessPicker.Model_SelectedValueChanged));
      WeakEventManager<ToolbarSettingItemStrokeThicknessModel, EventArgs>.RemoveHandler((ToolbarSettingItemStrokeThicknessModel) e.OldValue, "StandardItemsChanged", new EventHandler<EventArgs>(strokeThicknessPicker.Model_StandardItemsChanged));
      WeakEventManager<ToolbarSettingItemStrokeThicknessModel, EventArgs>.RemoveHandler((ToolbarSettingItemStrokeThicknessModel) e.OldValue, "TransientItemChanged", new EventHandler<EventArgs>(strokeThicknessPicker.Model_TransientItemChanged));
    }
    if (e.NewValue != null)
    {
      WeakEventManager<ToolbarSettingItemStrokeThicknessModel, EventArgs>.AddHandler((ToolbarSettingItemStrokeThicknessModel) e.NewValue, "SelectedValueChanged", new EventHandler<EventArgs>(strokeThicknessPicker.Model_SelectedValueChanged));
      WeakEventManager<ToolbarSettingItemStrokeThicknessModel, EventArgs>.AddHandler((ToolbarSettingItemStrokeThicknessModel) e.NewValue, "StandardItemsChanged", new EventHandler<EventArgs>(strokeThicknessPicker.Model_StandardItemsChanged));
      WeakEventManager<ToolbarSettingItemStrokeThicknessModel, EventArgs>.AddHandler((ToolbarSettingItemStrokeThicknessModel) e.NewValue, "TransientItemChanged", new EventHandler<EventArgs>(strokeThicknessPicker.Model_TransientItemChanged));
    }
    strokeThicknessPicker.UpdatePickerComboBoxItems();
    strokeThicknessPicker.UpdateSelectedValue();
  }

  private void Model_SelectedValueChanged(object sender, EventArgs e)
  {
    this.UpdateSelectedValue();
    if (this.Model.Id.AnnotationMode != AnnotationMode.Ink)
      return;
    (this.Model.Parent[3] as ToolbarSettingInkEraserModel).IsChecked = false;
  }

  private void Model_StandardItemsChanged(object sender, EventArgs e)
  {
    this.UpdatePickerComboBoxItems();
  }

  private void Model_TransientItemChanged(object sender, EventArgs e)
  {
    this.UpdatePickerComboBoxItems();
  }

  private void UpdateSelectedValue()
  {
    this.comboBox.SelectionChanged -= new SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
    if (this.Model == null)
      this.comboBox.SelectedIndex = -1;
    else if (this.comboBox.ItemsSource is List<StrokeThicknessPicker.NestedModel> itemsSource && this.Model.SelectedValue is float selectedValue)
    {
      foreach (StrokeThicknessPicker.NestedModel nestedModel in itemsSource)
      {
        if ((double) nestedModel.Value == (double) selectedValue)
        {
          this.comboBox.SelectedItem = (object) nestedModel;
          break;
        }
      }
    }
    this.comboBox.SelectionChanged += new SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
  }

  private void UpdatePickerComboBoxItems()
  {
    this.comboBox.SelectionChanged -= new SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
    if (this.Model == null)
    {
      this.comboBox.ItemsSource = (IEnumerable) null;
    }
    else
    {
      ObservableCollection<float> standardItems = this.Model.StandardItems;
      List<StrokeThicknessPicker.NestedModel> nestedModelList = (standardItems != null ? standardItems.Select<float, StrokeThicknessPicker.NestedModel>((Func<float, StrokeThicknessPicker.NestedModel>) (c => new StrokeThicknessPicker.NestedModel(c, this.Model))).ToList<StrokeThicknessPicker.NestedModel>() : (List<StrokeThicknessPicker.NestedModel>) null) ?? new List<StrokeThicknessPicker.NestedModel>();
      if (this.Model.TransientItem.HasValue && nestedModelList.Find((Predicate<StrokeThicknessPicker.NestedModel>) (c =>
      {
        float? transientItem = c.Model.TransientItem;
        double num1 = (double) transientItem.Value;
        transientItem = this.Model.TransientItem;
        double num2 = (double) transientItem.Value;
        return num1 == num2;
      })) == null)
        nestedModelList.Add(new StrokeThicknessPicker.NestedModel(this.Model.TransientItem.Value, this.Model));
      this.comboBox.ItemsSource = (IEnumerable) nestedModelList;
    }
    this.comboBox.SelectionChanged += new SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
  }

  private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    if (this.Model == null)
      return;
    StrokeThicknessPicker.NestedModel nestedModel = e.AddedItems.OfType<StrokeThicknessPicker.NestedModel>().FirstOrDefault<StrokeThicknessPicker.NestedModel>();
    if (nestedModel == null)
      return;
    this.Model.SelectedValue = (object) nestedModel.Value;
    this.Model.ExecuteCommand();
  }

  private void ThicknessIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/menus/toolbarsettings/strokethicknesspicker.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 1)
    {
      if (connectionId == 2)
        this.comboBox = (ComboBox) target;
      else
        this._contentLoaded = true;
    }
    else
    {
      this.thicknessIcon = (Image) target;
      this.thicknessIcon.MouseLeftButtonDown += new MouseButtonEventHandler(this.ThicknessIcon_MouseLeftButtonDown);
    }
  }

  private class NestedModel : ObservableObject
  {
    public NestedModel(float value, ToolbarSettingItemStrokeThicknessModel model)
    {
      this.Value = value;
      this.Model = model;
    }

    public float Value { get; }

    public string Caption => $"{this.Value} pt";

    public ToolbarSettingItemStrokeThicknessModel Model { get; }
  }
}
