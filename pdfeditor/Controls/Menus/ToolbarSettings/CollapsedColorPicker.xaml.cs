// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarSettings.CollapsedColorPicker
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Controls.ColorPickers;
using pdfeditor.Models.Menus.ToolbarSettings;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Menus.ToolbarSettings;

public partial class CollapsedColorPicker : UserControl, IComponentConnector
{
  public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof (Model), typeof (ToolbarSettingItemColorCollapseModel), typeof (CollapsedColorPicker), new PropertyMetadata((object) null, new PropertyChangedCallback(CollapsedColorPicker.OnModelPropertyChanged)));
  internal ColorPickerButton colorPickerButton;
  private bool _contentLoaded;

  public CollapsedColorPicker() => this.InitializeComponent();

  public ToolbarSettingItemColorCollapseModel Model
  {
    get => (ToolbarSettingItemColorCollapseModel) this.GetValue(CollapsedColorPicker.ModelProperty);
    set => this.SetValue(CollapsedColorPicker.ModelProperty, (object) value);
  }

  private static void OnModelPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is CollapsedColorPicker collapsedColorPicker))
      return;
    if (e.OldValue != null)
    {
      WeakEventManager<ToolbarSettingItemColorCollapseModel, EventArgs>.RemoveHandler((ToolbarSettingItemColorCollapseModel) e.OldValue, "SelectedValueChanged", new EventHandler<EventArgs>(collapsedColorPicker.Model_SelectedValueChanged));
      WeakEventManager<ToolbarSettingItemColorCollapseModel, EventArgs>.RemoveHandler((ToolbarSettingItemColorCollapseModel) e.OldValue, "ColorsChanged", new EventHandler<EventArgs>(collapsedColorPicker.Model_ColorsChanged));
    }
    if (e.NewValue != null)
    {
      WeakEventManager<ToolbarSettingItemColorCollapseModel, EventArgs>.AddHandler((ToolbarSettingItemColorCollapseModel) e.NewValue, "SelectedValueChanged", new EventHandler<EventArgs>(collapsedColorPicker.Model_SelectedValueChanged));
      WeakEventManager<ToolbarSettingItemColorCollapseModel, EventArgs>.AddHandler((ToolbarSettingItemColorCollapseModel) e.NewValue, "ColorsChanged", new EventHandler<EventArgs>(collapsedColorPicker.Model_ColorsChanged));
    }
    collapsedColorPicker.UpdatePickerSelectedColor();
    collapsedColorPicker.UpdatePickerStandardColors();
  }

  private void Model_SelectedValueChanged(object sender, EventArgs e)
  {
    this.UpdatePickerSelectedColor();
  }

  private void Model_ColorsChanged(object sender, EventArgs e) => this.UpdatePickerStandardColors();

  private void UpdatePickerStandardColors()
  {
    if (this.Model == null)
    {
      this.colorPickerButton.StandardColors = (IReadOnlyCollection<Color>) null;
    }
    else
    {
      ColorPickerButton colorPickerButton = this.colorPickerButton;
      ObservableCollection<string> standardColors = this.Model.StandardColors;
      Color[] array = standardColors != null ? standardColors.Select<string, Color?>((Func<string, Color?>) (c =>
      {
        try
        {
          return (Color?) ColorConverter.ConvertFromString(c);
        }
        catch
        {
        }
        return new Color?();
      })).Where<Color?>((Func<Color?, bool>) (c => c.HasValue)).Select<Color?, Color>((Func<Color?, Color>) (c => c.Value)).ToArray<Color>() : (Color[]) null;
      colorPickerButton.StandardColors = (IReadOnlyCollection<Color>) array;
    }
  }

  private void UpdatePickerSelectedColor()
  {
    if (this.Model == null)
    {
      this.colorPickerButton.SelectedColor = Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0);
    }
    else
    {
      try
      {
        this.colorPickerButton.SelectedColor = (Color) ColorConverter.ConvertFromString((string) this.Model.SelectedValue);
      }
      catch
      {
      }
    }
  }

  private void ColorPickerButton_ItemClick(object sender, ColorPickerButtonItemClickEventArgs e)
  {
    if (this.Model == null)
      return;
    Color color = e.Item.Color;
    this.Model.SelectedValue = (object) $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
    this.Model.ExecuteCommand();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/menus/toolbarsettings/collapsedcolorpicker.xaml", UriKind.Relative));
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
    if (connectionId == 1)
      this.colorPickerButton = (ColorPickerButton) target;
    else
      this._contentLoaded = true;
  }
}
