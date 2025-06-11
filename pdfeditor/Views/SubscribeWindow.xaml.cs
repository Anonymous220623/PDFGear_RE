// Decompiled with JetBrains decompiler
// Type: pdfeditor.Views.SubscribeWindow
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
using System.Windows.Input;
using System.Windows.Markup;

#nullable disable
namespace pdfeditor.Views;

public partial class SubscribeWindow : Window, IComponentConnector
{
  internal Image LogoImage;
  private bool _contentLoaded;

  public SubscribeWindow() => this.InitializeComponent();

  protected override void OnSourceInitialized(EventArgs e)
  {
    base.OnSourceInitialized(e);
    GAManager.SendEvent("YTSub", "Show", "Count", 1L);
  }

  private void OpenChannel()
  {
    GAManager.SendEvent("YTSub", "Subscribe", "Count", 1L);
    try
    {
      Process.Start("https://www.youtube.com/@pdfgear?sub_confirmation=1");
    }
    catch
    {
    }
  }

  private void OpenTwitterChannel()
  {
    GAManager.SendEvent("TwitterSub", "Subscribe", "Count", 1L);
    try
    {
      Process.Start("https://twitter.com/intent/user?screen_name=PDFgear");
    }
    catch
    {
    }
  }

  private void OpenTrustpliot()
  {
    GAManager.SendEvent("TrustpliotSub", "Subscribe", "Count", 1L);
    try
    {
      Process.Start("https://www.trustpilot.com/evaluate/pdfgear.com");
    }
    catch
    {
    }
  }

  private void Button_Click(object sender, RoutedEventArgs e) => this.OpenChannel();

  private void TrustpliotButton_Click(object sender, RoutedEventArgs e) => this.OpenTrustpliot();

  private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e) => this.OpenChannel();

  private void Button_Click_1(object sender, RoutedEventArgs e) => this.OpenTwitterChannel();

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/views/subscribewindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  internal Delegate _CreateDelegate(Type delegateType, string handler)
  {
    return Delegate.CreateDelegate(delegateType, (object) this, handler);
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    switch (connectionId)
    {
      case 1:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.TrustpliotButton_Click);
        break;
      case 2:
        this.LogoImage = (Image) target;
        break;
      case 3:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.Button_Click);
        break;
      case 4:
        ((ButtonBase) target).Click += new RoutedEventHandler(this.Button_Click_1);
        break;
      default:
        this._contentLoaded = true;
        break;
    }
  }
}
