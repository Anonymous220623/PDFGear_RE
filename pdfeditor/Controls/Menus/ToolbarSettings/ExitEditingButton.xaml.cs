// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Menus.ToolbarSettings.ExitEditingButton
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

public partial class ExitEditingButton : UserControl, IComponentConnector
{
  public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(nameof (Model), typeof (ToolbarSettingItemExitModel), typeof (ExitEditingButton), new PropertyMetadata((PropertyChangedCallback) null));
  internal Button btnExit;
  private bool _contentLoaded;

  public ToolbarSettingItemExitModel Model
  {
    get => (ToolbarSettingItemExitModel) this.GetValue(ExitEditingButton.ModelProperty);
    set => this.SetValue(ExitEditingButton.ModelProperty, (object) value);
  }

  public ExitEditingButton() => this.InitializeComponent();

  private void btnExit_Click(object sender, RoutedEventArgs e) => this.Model?.ExecuteCommand();

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/menus/toolbarsettings/exiteditingbutton.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId == 1)
    {
      this.btnExit = (Button) target;
      this.btnExit.Click += new RoutedEventHandler(this.btnExit_Click);
    }
    else
      this._contentLoaded = true;
  }
}
