// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarSettings.EraserPicker
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Models.Menus.ToolbarSettings;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.Menus.ToolbarSettings;

public partial class EraserPicker : UserControl, IComponentConnector
{
  public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof (Model), typeof (ToolbarSettingInkEraserModel), typeof (EraserPicker), new PropertyMetadata((PropertyChangedCallback) null));
  internal ToggleButton Eraser;
  internal ToggleButton EraserBar;
  internal Popup EraserBarPopup;
  internal Slider EraserSizeSlider;
  private bool _contentLoaded;

  public ToolbarSettingInkEraserModel Model
  {
    get => (ToolbarSettingInkEraserModel) this.GetValue(EraserPicker.ModelProperty);
    set => this.SetValue(EraserPicker.ModelProperty, (object) value);
  }

  public EraserPicker() => this.InitializeComponent();

  private void ToggleButton_Click(object sender, RoutedEventArgs e)
  {
    this.EraserBarPopup.IsOpen = false;
    this.Model.IsChecked = true;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/menus/toolbarsettings/eraserpicker.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.Eraser = (ToggleButton) target;
        break;
      case 2:
        this.EraserBar = (ToggleButton) target;
        break;
      case 3:
        this.EraserBarPopup = (Popup) target;
        break;
      case 4:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.ToggleButton_Click);
        break;
      case 5:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.ToggleButton_Click);
        break;
      case 6:
        this.EraserSizeSlider = (Slider) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
