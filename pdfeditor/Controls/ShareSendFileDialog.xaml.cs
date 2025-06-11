// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.ShareSendFileDialog
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Utils;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls;

public partial class ShareSendFileDialog : Window, IComponentConnector
{
  private readonly string filePath;
  private bool _contentLoaded;

  public ShareSendFileDialog(string filePath)
  {
    this.InitializeComponent();
    this.filePath = filePath;
  }

  private void Button_Click()
  {
  }

  private async void Button_Click(object sender, RoutedEventArgs e)
  {
    await ShareUtils.ShowInFolderAsync(this.filePath);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/sharesendfiledialog.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId == 1)
      ((ButtonBase) target).Click += new RoutedEventHandler(this.Button_Click);
    else
      this._contentLoaded = true;
  }
}
