// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Signature.SignatureClearConfirmWin
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.Signature;

public partial class SignatureClearConfirmWin : Window, IComponentConnector
{
  internal Button btnCancel;
  internal Button btnOk;
  private bool _contentLoaded;

  public SignatureClearConfirmWin() => this.InitializeComponent();

  private void btnCancel_Click(object sender, RoutedEventArgs e)
  {
    this.DialogResult = new bool?(false);
    this.Close();
  }

  private void btnOk_Click(object sender, RoutedEventArgs e) => this.DialogResult = new bool?(true);

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/signature/signatureclearconfirmwin.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 1)
    {
      if (connectionId == 2)
      {
        this.btnOk = (Button) target;
        this.btnOk.Click += new RoutedEventHandler(this.btnOk_Click);
      }
      else
        this._contentLoaded = true;
    }
    else
    {
      this.btnCancel = (Button) target;
      this.btnCancel.Click += new RoutedEventHandler(this.btnCancel_Click);
    }
  }
}
