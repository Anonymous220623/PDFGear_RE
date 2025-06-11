// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Signature.SignatureEmbedConfirmWin
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

public partial class SignatureEmbedConfirmWin : Window, IComponentConnector
{
  public static readonly DependencyProperty ConfirmTitleProperty = DependencyProperty.Register(nameof (ConfirmTitle), typeof (string), typeof (SignatureEmbedConfirmWin), new PropertyMetadata((object) ""));
  public static readonly DependencyProperty NoteMsgProperty = DependencyProperty.Register(" NoteMsg", typeof (string), typeof (SignatureEmbedConfirmWin), new PropertyMetadata((object) ""));
  internal Button btnCancel;
  internal Button btnOk;
  private bool _contentLoaded;

  public string ConfirmTitle
  {
    get => (string) this.GetValue(SignatureEmbedConfirmWin.ConfirmTitleProperty);
    set => this.SetValue(SignatureEmbedConfirmWin.ConfirmTitleProperty, (object) value);
  }

  public string NoteMsg
  {
    get => (string) this.GetValue(SignatureEmbedConfirmWin.NoteMsgProperty);
    set => this.SetValue(SignatureEmbedConfirmWin.NoteMsgProperty, (object) value);
  }

  public SignatureEmbedConfirmWin(EmbedType type)
  {
    this.InitializeComponent();
    switch (type)
    {
      case EmbedType.Single:
        this.ConfirmTitle = pdfeditor.Properties.Resources.WinSignatureFlattenInBatchQuestion;
        this.NoteMsg = pdfeditor.Properties.Resources.WinSignatureFlattenInBatchNoteMsg;
        break;
      case EmbedType.InBatch:
        this.ConfirmTitle = pdfeditor.Properties.Resources.WinSignatureFlattenInBatchQuestion;
        this.NoteMsg = pdfeditor.Properties.Resources.WinSignatureFlattenInBatchNoteMsg;
        break;
      case EmbedType.All:
        this.ConfirmTitle = pdfeditor.Properties.Resources.WinSaveFilewithUnFlattenSignatureNoteMsg;
        this.NoteMsg = pdfeditor.Properties.Resources.WinSaveFilewithUnFlattenSignatureExplain;
        break;
    }
  }

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
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/signature/signatureembedconfirmwin.xaml", UriKind.Relative));
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
      this.btnCancel = (Button) target;
  }
}
