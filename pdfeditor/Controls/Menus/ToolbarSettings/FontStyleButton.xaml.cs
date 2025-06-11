// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarSettings.FontStyleButton
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
using System.Windows.Media;

#nullable disable
namespace pdfeditor.Controls.Menus.ToolbarSettings;

public partial class FontStyleButton : UserControl, IComponentConnector
{
  public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof (Model), typeof (ToolbarSettingItemModel), typeof (FontStyleButton), new PropertyMetadata((object) null, new PropertyChangedCallback(FontStyleButton.OnModelPropertyChanged)));
  internal ToggleButton Button;
  private bool _contentLoaded;

  public FontStyleButton() => this.InitializeComponent();

  public ToolbarSettingItemModel Model
  {
    get => (ToolbarSettingItemModel) this.GetValue(FontStyleButton.ModelProperty);
    set => this.SetValue(FontStyleButton.ModelProperty, (object) value);
  }

  private static void OnModelPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == e.OldValue || !(d is FontStyleButton fontStyleButton))
      return;
    if (e.OldValue != null)
      WeakEventManager<ToolbarSettingItemModel, EventArgs>.RemoveHandler((ToolbarSettingItemModel) e.OldValue, "SelectedValueChanged", new EventHandler<EventArgs>(fontStyleButton.Model_SelectedValueChanged));
    if (e.NewValue != null)
      WeakEventManager<ToolbarSettingItemModel, EventArgs>.AddHandler((ToolbarSettingItemModel) e.NewValue, "SelectedValueChanged", new EventHandler<EventArgs>(fontStyleButton.Model_SelectedValueChanged));
    fontStyleButton.UpdateModel();
  }

  private void Model_SelectedValueChanged(object sender, EventArgs e)
  {
    this.Button.IsChecked = new bool?(this.Model?.SelectedValue is bool selectedValue && selectedValue);
  }

  private void UpdateModel()
  {
    if (this.Model is ToolbarSettingItemBoldModel)
    {
      this.Button.Content = (object) "B";
      this.Button.FontStyle = FontStyles.Normal;
      this.Button.FontWeight = FontWeights.Bold;
      this.Button.Padding = new Thickness();
      this.Button.FontFamily = new FontFamily("Arial");
    }
    else
    {
      if (!(this.Model is ToolbarSettingItemItalicModel))
        return;
      this.Button.Content = (object) "I";
      this.Button.FontStyle = FontStyles.Italic;
      this.Button.FontWeight = FontWeights.Normal;
      this.Button.Padding = new Thickness(0.0, 0.0, 2.0, 0.0);
      this.Button.FontFamily = new FontFamily("Times New Roman");
    }
  }

  private void Button_Click(object sender, RoutedEventArgs e)
  {
    if (this.Model.SelectedValue is bool selectedValue && selectedValue)
    {
      this.Model.SelectedValue = (object) false;
      this.Model.ExecuteCommand();
    }
    else
    {
      this.Model.SelectedValue = (object) true;
      this.Model.ExecuteCommand();
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/menus/toolbarsettings/fontstylebutton.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId == 1)
    {
      this.Button = (ToggleButton) target;
      this.Button.Click += new RoutedEventHandler(this.Button_Click);
    }
    else
      this._contentLoaded = true;
  }
}
