// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.Protection.PasswordSaveTipWindow
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using CommomLib.Commom;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Controls.Protection;

public partial class PasswordSaveTipWindow : Window, IComponentConnector
{
  internal Grid grid_root;
  internal CheckBox cboxNoMorPrompt;
  private bool _contentLoaded;

  public PasswordSaveTipWindow() => this.InitializeComponent();

  private void Button_Click(object sender, RoutedEventArgs e)
  {
    if (this.cboxNoMorPrompt.IsChecked.GetValueOrDefault())
      ConfigManager.SetPasswordSaveNoMorePromptFlag(true);
    this.Close();
  }

  private void Window_Loaded(object sender, RoutedEventArgs e)
  {
    this.Height = this.grid_root.ActualHeight;
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/protect/passwordsavetipwindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
        break;
      case 2:
        this.grid_root = (Grid) target;
        break;
      case 3:
        this.cboxNoMorPrompt = (CheckBox) target;
        break;
      case 4:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.Button_Click);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
