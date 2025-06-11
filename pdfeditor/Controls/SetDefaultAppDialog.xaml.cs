// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.SetDefaultAppDialog
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls;

public partial class SetDefaultAppDialog : Window, IComponentConnector
{
  internal CheckBox DontShowAgainCheckbox;
  private bool _contentLoaded;

  public SetDefaultAppDialog() => this.InitializeComponent();

  private void OKButton_Click(object sender, RoutedEventArgs e)
  {
    this.DialogResult = new bool?(true);
  }

  public string Action { get; private set; }

  protected override void OnClosed(EventArgs e)
  {
    StringBuilder stringBuilder = new StringBuilder();
    if (this.DontShowAgainCheckbox.IsChecked.GetValueOrDefault())
      stringBuilder.Append("Silence_");
    if (this.DialogResult.GetValueOrDefault())
      stringBuilder.Append("SetDefault");
    else
      stringBuilder.Append("Exit");
    this.Action = stringBuilder.ToString();
    GAManager.SendEvent("ExtDefaultAppDialog", this.Action, "Count", 1L);
    base.OnClosed(e);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/setdefaultappdialog.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 1)
    {
      if (connectionId == 2)
        ((ButtonBase) target).Click += new RoutedEventHandler(this.OKButton_Click);
      else
        this._contentLoaded = true;
    }
    else
      this.DontShowAgainCheckbox = (CheckBox) target;
  }
}
