// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Protection.OwerPasswordCheckWindow
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Models;
using pdfeditor.Utils;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.Protection;

public partial class OwerPasswordCheckWindow : Window, IComponentConnector
{
  private DocumentWrapper Pdf;
  internal PasswordBox passwordBox;
  internal TextBlock tbError;
  internal Button btnCancel;
  internal Button btnOK;
  private bool _contentLoaded;

  public OwerPasswordCheckWindow(DocumentWrapper document)
  {
    this.InitializeComponent();
    this.Pdf = document;
  }

  private void OK_Click(object sender, RoutedEventArgs e)
  {
    if (string.IsNullOrWhiteSpace(this.passwordBox.Password))
    {
      this.tbError.Visibility = Visibility.Visible;
      this.tbError.Text = pdfeditor.Properties.Resources.WinPwdPasswordCheckEmptyContent;
    }
    else
    {
      try
      {
        if (EncryptUtils.VerifyOwerpassword(this.Pdf.DocumentPath, this.passwordBox.Password))
        {
          this.DialogResult = new bool?(true);
          this.Pdf.EncryptManage.UpdateOwerPassword(this.passwordBox.Password);
          this.Pdf.EncryptManage.IsRequiredOwerPassword = false;
          this.Close();
        }
        else
        {
          this.tbError.Text = pdfeditor.Properties.Resources.WinPwdWrongCheckContent;
          this.tbError.Visibility = Visibility.Visible;
        }
      }
      catch
      {
      }
    }
  }

  private void Cancel_Click(object sender, RoutedEventArgs e)
  {
    this.DialogResult = new bool?(false);
    this.Close();
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/protect/owerpasswordcheckwindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.passwordBox = (PasswordBox) target;
        break;
      case 2:
        this.tbError = (TextBlock) target;
        break;
      case 3:
        this.btnCancel = (Button) target;
        this.btnCancel.Click += new RoutedEventHandler(this.Cancel_Click);
        break;
      case 4:
        this.btnOK = (Button) target;
        this.btnOK.Click += new RoutedEventHandler(this.OK_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
