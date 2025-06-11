// Decompiled with JetBrains decompiler
// Type: PDFLauncher.PurchasedWindow
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

#nullable disable
namespace PDFLauncher;

public partial class PurchasedWindow : Window, IComponentConnector
{
  internal Image uImg;
  private bool _contentLoaded;

  public PurchasedWindow() => this.InitializeComponent();

  private void Button_Click(object sender, RoutedEventArgs e) => this.Close();

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/PDFLauncher;component/purchasedwindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId != 1)
    {
      if (connectionId == 2)
        ((ButtonBase) target).Click += new RoutedEventHandler(this.Button_Click);
      else
        this._contentLoaded = true;
    }
    else
      this.uImg = (Image) target;
  }
}
