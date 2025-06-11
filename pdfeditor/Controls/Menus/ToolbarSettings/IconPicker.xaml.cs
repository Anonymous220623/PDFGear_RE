// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarSettings.IconPicker
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
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.Menus.ToolbarSettings;

public partial class IconPicker : UserControl, IComponentConnector
{
  public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof (Model), typeof (ToolbarSettingItemIconModel), typeof (IconPicker), new PropertyMetadata((object) null, new PropertyChangedCallback(IconPicker.OnModelPropertyChanged)));
  private bool _contentLoaded;

  public IconPicker() => this.InitializeComponent();

  public ToolbarSettingItemIconModel Model
  {
    get => (ToolbarSettingItemIconModel) this.GetValue(IconPicker.ModelProperty);
    set => this.SetValue(IconPicker.ModelProperty, (object) value);
  }

  private static void OnModelPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/menus/toolbarsettings/iconpicker.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;
}
