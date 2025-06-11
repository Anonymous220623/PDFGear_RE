// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Protection.EncryptWindow
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using pdfeditor.Models;
using pdfeditor.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.Protection;

public partial class EncryptWindow : Window, IComponentConnector
{
  private DocumentWrapper Pdf;
  internal EncryptWindow window;
  internal RowDefinition cboxGridRow;
  internal TextBlock tbDocmentOpenText;
  internal TextBlock tbDocmentConfirmText;
  internal PasswordBox tbOpenpwd;
  internal TextBox PasswordTextBox;
  internal Button ShowPwdBth;
  internal PasswordBox tbOpenpwdConfirm;
  internal CheckBox ckboxRetainpwd;
  internal TextBlock tbpasswordNotMatch;
  internal Button btnCancel;
  internal Button btnOK;
  private bool _contentLoaded;

  public EncryptWindow(DocumentWrapper pdf)
  {
    this.InitializeComponent();
    this.Pdf = pdf;
    if (!string.IsNullOrWhiteSpace(this.Pdf.EncryptManage.UserPassword))
    {
      this.tbOpenpwd.Password = this.Pdf.EncryptManage.UserPassword;
      this.tbOpenpwdConfirm.Password = this.Pdf.EncryptManage.UserPassword;
    }
    else
    {
      this.tbOpenpwd.Password = this.Pdf.EncryptManage.OwerPassword;
      this.tbOpenpwdConfirm.Password = this.Pdf.EncryptManage.OwerPassword;
    }
    if (!this.Pdf.EncryptManage.IsHaveOwerPassword || pdf.EncryptManage.IsChangedPassword)
    {
      this.Height -= 32.0;
      this.ckboxRetainpwd.Visibility = Visibility.Collapsed;
      this.cboxGridRow.Height = new GridLength(0.0);
    }
    if (this.Pdf.EncryptManage.IsRequiredOwerPassword && this.Pdf.EncryptManage.IsHaveOwerPassword)
      this.ckboxRetainpwd.IsChecked = new bool?(false);
    GAManager.SendEvent("Password", "AddPasswordShow", "Count", 1L);
  }

  private void Encrypt_Click(object sender, RoutedEventArgs e)
  {
    try
    {
      if (!this.VarifyOpenpwd() || this.Pdf == null)
        return;
      string owerpassword = !this.ckboxRetainpwd.IsChecked.GetValueOrDefault() || this.ckboxRetainpwd.Visibility != Visibility.Visible ? this.tbOpenpwdConfirm.Password.Trim() : this.Pdf.EncryptManage.OwerPassword;
      this.Pdf?.EncryptManage.SetPassword(this.tbOpenpwdConfirm.Password.Trim(), owerpassword);
      Ioc.Default.GetRequiredService<MainViewModel>().SetCanSaveFlag("Password", false);
      if (!ConfigManager.GetPasswordSaveNoMorePromptFlag())
        new PasswordSaveTipWindow().ShowDialog();
      GAManager.SendEvent("Password", "AddPassword", "Count", 1L);
      this.Close();
    }
    catch
    {
    }
  }

  private bool VarifyOpenpwd()
  {
    try
    {
      if (string.IsNullOrWhiteSpace(this.tbOpenpwd.Password))
      {
        this.tbpasswordNotMatch.Visibility = Visibility.Visible;
        this.tbpasswordNotMatch.Text = pdfeditor.Properties.Resources.WinPwdPasswordCheckEmptyContent;
        return false;
      }
      if (this.tbOpenpwd.Password != this.tbOpenpwdConfirm.Password)
      {
        this.tbpasswordNotMatch.Visibility = Visibility.Visible;
        this.tbpasswordNotMatch.Text = pdfeditor.Properties.Resources.WinPwdPasswordCheckMatchContent;
        return false;
      }
      if (this.tbOpenpwd.Password.Length < 6)
      {
        this.tbpasswordNotMatch.Visibility = Visibility.Visible;
        this.tbpasswordNotMatch.Text = pdfeditor.Properties.Resources.WinPwdMinCharacterCheckContent;
        return false;
      }
      if (this.tbOpenpwd.Password.Length > 32 /*0x20*/)
      {
        this.tbpasswordNotMatch.Visibility = Visibility.Visible;
        this.tbpasswordNotMatch.Text = pdfeditor.Properties.Resources.WinPwdMaxCharacterCheckContent;
        return false;
      }
      if (!this.VerifyIsANSIString(this.tbOpenpwd.Password))
      {
        this.tbpasswordNotMatch.Visibility = Visibility.Visible;
        this.tbpasswordNotMatch.Text = pdfeditor.Properties.Resources.WinPwdillegalsymbolsCheckContent;
        return false;
      }
    }
    catch
    {
      return false;
    }
    this.tbpasswordNotMatch.Visibility = Visibility.Hidden;
    return true;
  }

  private bool VerifyIsANSIString(string pwd)
  {
    bool flag = true;
    foreach (char ch in pwd)
    {
      if (ch < '!' || ch > '~')
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  private void Cacel_Click(object sender, RoutedEventArgs e)
  {
    this.DialogResult = new bool?(false);
    this.Close();
  }

  private void ckboxRetainpwd_Checked(object sender, RoutedEventArgs e)
  {
    if (this.Pdf == null || !this.Pdf.EncryptManage.IsRequiredOwerPassword)
      return;
    bool? nullable = new OwerPasswordCheckWindow(this.Pdf).ShowDialog();
    bool flag = false;
    if (!(nullable.GetValueOrDefault() == flag & nullable.HasValue))
      return;
    this.ckboxRetainpwd.IsChecked = new bool?(false);
  }

  private void ShowPwdBth_MouseDown(object sender, MouseButtonEventArgs e)
  {
    if (e.LeftButton != MouseButtonState.Pressed)
      return;
    ((UIElement) sender).CaptureMouse();
    this.tbOpenpwd.Visibility = Visibility.Collapsed;
    this.PasswordTextBox.Visibility = Visibility.Visible;
  }

  private void ShowPwdBth_MouseUp(object sender, MouseButtonEventArgs e)
  {
    if (e.LeftButton != MouseButtonState.Released)
      return;
    ((UIElement) sender).ReleaseMouseCapture();
    this.tbOpenpwd.Visibility = Visibility.Visible;
    this.PasswordTextBox.Visibility = Visibility.Collapsed;
  }

  private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
  {
    string password = this.tbOpenpwd.Password;
    if (!(this.PasswordTextBox.Text != password))
      return;
    this.PasswordTextBox.Text = password;
  }

  private void PasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
  {
    string password = this.tbOpenpwd.Password;
    if (this.PasswordTextBox.Text != password)
      this.tbOpenpwd.Password = this.PasswordTextBox.Text;
    this.btnOK.IsEnabled = !string.IsNullOrEmpty(password);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/protect/encryptwindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.window = (EncryptWindow) target;
        break;
      case 2:
        this.cboxGridRow = (RowDefinition) target;
        break;
      case 3:
        this.tbDocmentOpenText = (TextBlock) target;
        break;
      case 4:
        this.tbDocmentConfirmText = (TextBlock) target;
        break;
      case 5:
        this.tbOpenpwd = (PasswordBox) target;
        this.tbOpenpwd.PasswordChanged += new RoutedEventHandler(this.PasswordBox_PasswordChanged);
        break;
      case 6:
        this.PasswordTextBox = (TextBox) target;
        this.PasswordTextBox.TextChanged += new TextChangedEventHandler(this.PasswordTextBox_TextChanged);
        break;
      case 7:
        this.ShowPwdBth = (Button) target;
        this.ShowPwdBth.PreviewMouseDown += new MouseButtonEventHandler(this.ShowPwdBth_MouseDown);
        this.ShowPwdBth.PreviewMouseUp += new MouseButtonEventHandler(this.ShowPwdBth_MouseUp);
        break;
      case 8:
        this.tbOpenpwdConfirm = (PasswordBox) target;
        break;
      case 9:
        this.ckboxRetainpwd = (CheckBox) target;
        this.ckboxRetainpwd.Checked += new RoutedEventHandler(this.ckboxRetainpwd_Checked);
        break;
      case 10:
        this.tbpasswordNotMatch = (TextBlock) target;
        break;
      case 11:
        this.btnCancel = (Button) target;
        this.btnCancel.Click += new RoutedEventHandler(this.Cacel_Click);
        break;
      case 12:
        this.btnOK = (Button) target;
        this.btnOK.Click += new RoutedEventHandler(this.Encrypt_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
