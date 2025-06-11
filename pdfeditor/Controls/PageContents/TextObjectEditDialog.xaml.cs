// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.PageContents.TextObjectEditDialog
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.PageContents;

public partial class TextObjectEditDialog : Window, IComponentConnector
{
  public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof (Text), typeof (string), typeof (TextObjectEditDialog), new PropertyMetadata((object) ""));
  private bool _contentLoaded;

  public TextObjectEditDialog() => this.InitializeComponent();

  public string Text
  {
    get => (string) this.GetValue(TextObjectEditDialog.TextProperty);
    set => this.SetValue(TextObjectEditDialog.TextProperty, (object) value);
  }

  private void OKButton_Click(object sender, RoutedEventArgs e)
  {
    this.DialogResult = new bool?(true);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/pagecontents/textobjecteditdialog.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId == 1)
      ((ButtonBase) target).Click += new RoutedEventHandler(this.OKButton_Click);
    else
      this._contentLoaded = true;
  }
}
