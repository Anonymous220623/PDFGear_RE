// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Speech.SpeechMessage
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
namespace pdfeditor.Controls.Speech;

public partial class SpeechMessage : Window, IComponentConnector
{
  internal TextBlock Textblock;
  internal Button btnOk;
  internal Button btnCancel;
  private bool _contentLoaded;

  public SpeechMessage(string cul, int pageindex)
  {
    this.InitializeComponent();
    this.Textblock.Text = this.Textblock.Text.Replace("xxxx", pageindex.ToString()).Replace("xxx", cul);
    this.InitMenu();
  }

  private void InitMenu()
  {
    this.btnOk.Click += (RoutedEventHandler) ((o, e) =>
    {
      this.DialogResult = new bool?(true);
      this.Close();
    });
    this.btnCancel.Click += (RoutedEventHandler) ((o, e) =>
    {
      this.DialogResult = new bool?(false);
      this.Close();
    });
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/speech/speechmessage.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.Textblock = (TextBlock) target;
        break;
      case 2:
        this.btnOk = (Button) target;
        break;
      case 3:
        this.btnCancel = (Button) target;
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
