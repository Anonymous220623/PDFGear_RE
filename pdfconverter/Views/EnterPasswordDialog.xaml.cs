// Decompiled with JetBrains decompiler
// Type: pdfconverter.Views.EnterPasswordDialog
// Assembly: pdfconverter, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 691C26A2-7651-46C3-AA3C-2839EDA9722B
// Assembly location: C:\Program Files\PDFgear\pdfconverter.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace pdfconverter.Views;

public partial class EnterPasswordDialog : Window, IComponentConnector
{
  internal TextBlock PwdTip;
  internal PasswordBox PasswordBox;
  internal TextBox PasswordTextBox;
  internal Button ShowPwdBth;
  internal Button OkBtn;
  private bool _contentLoaded;

  public EnterPasswordDialog() => this.InitializeComponent();

  public EnterPasswordDialog(string fileName)
    : this()
  {
    if (string.IsNullOrEmpty(fileName))
      return;
    this.PwdTip.Text = pdfconverter.Properties.Resources.WinPwdEnterTipContentFa.Replace("XXX", fileName);
  }

  public string Password { get; private set; }

  private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
  {
    string password = this.PasswordBox.Password;
    if (!(this.PasswordTextBox.Text != password))
      return;
    this.PasswordTextBox.Text = password;
  }

  private void PasswordTextBox_TextChanged(object sender, TextChangedEventArgs e)
  {
    string password = this.PasswordBox.Password;
    if (this.PasswordTextBox.Text != password)
      this.PasswordBox.Password = this.PasswordTextBox.Text;
    this.OkBtn.IsEnabled = !string.IsNullOrEmpty(password);
  }

  private void CancelButton_Click(object sender, RoutedEventArgs e) => this.Close();

  private void OKButton_Click(object sender, RoutedEventArgs e)
  {
    this.Password = this.PasswordTextBox.Text;
    this.DialogResult = new bool?(true);
  }

  private void ShowPwdBth_MouseDown(object sender, MouseButtonEventArgs e)
  {
    if (e.LeftButton != MouseButtonState.Pressed)
      return;
    ((UIElement) sender).CaptureMouse();
    this.PasswordBox.Visibility = Visibility.Collapsed;
    this.PasswordTextBox.Visibility = Visibility.Visible;
  }

  private void ShowPwdBth_MouseUp(object sender, MouseButtonEventArgs e)
  {
    if (e.LeftButton != MouseButtonState.Released)
      return;
    ((UIElement) sender).ReleaseMouseCapture();
    this.PasswordBox.Visibility = Visibility.Visible;
    this.PasswordTextBox.Visibility = Visibility.Collapsed;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfconverter;component/views/enterpassworddialog.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        this.PwdTip = (TextBlock) target;
        break;
      case 2:
        this.PasswordBox = (PasswordBox) target;
        this.PasswordBox.PasswordChanged += new RoutedEventHandler(this.PasswordBox_PasswordChanged);
        break;
      case 3:
        this.PasswordTextBox = (TextBox) target;
        this.PasswordTextBox.TextChanged += new TextChangedEventHandler(this.PasswordTextBox_TextChanged);
        break;
      case 4:
        this.ShowPwdBth = (Button) target;
        this.ShowPwdBth.PreviewMouseDown += new MouseButtonEventHandler(this.ShowPwdBth_MouseDown);
        this.ShowPwdBth.PreviewMouseUp += new MouseButtonEventHandler(this.ShowPwdBth_MouseUp);
        break;
      case 5:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.CancelButton_Click);
        break;
      case 6:
        this.OkBtn = (Button) target;
        this.OkBtn.Click += new RoutedEventHandler(this.OKButton_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
