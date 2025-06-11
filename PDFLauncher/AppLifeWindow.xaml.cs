// Decompiled with JetBrains decompiler
// Type: PDFLauncher.AppLifeWindow
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

#nullable disable
namespace PDFLauncher;

public partial class AppLifeWindow : Window, IComponentConnector
{
  private bool _contentLoaded;

  public AppLifeWindow()
  {
    this.InitializeComponent();
    this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.AppLifeWindow_IsVisibleChanged);
  }

  private void AppLifeWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
  {
    if (!this.IsVisible)
      return;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() => this.Hide()));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/PDFLauncher;component/applifewindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;
}
