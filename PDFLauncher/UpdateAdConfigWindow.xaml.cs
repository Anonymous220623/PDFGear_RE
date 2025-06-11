// Decompiled with JetBrains decompiler
// Type: PDFLauncher.UpdateAdConfigWindow
// Assembly: PDFLauncher, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 94FE2CE7-EB91-4B8B-872D-74808A04C1E6
// Assembly location: C:\Program Files\PDFgear\PDFLauncher.exe

using CommomLib.Commom;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

#nullable disable
namespace PDFLauncher;

public partial class UpdateAdConfigWindow : Window, IComponentConnector
{
  private bool _contentLoaded;

  public UpdateAdConfigWindow()
  {
    this.InitializeComponent();
    this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(this.UpdateAdConfigWindow_IsVisibleChanged);
  }

  private void UpdateAdConfigWindow_IsVisibleChanged(
    object sender,
    DependencyPropertyChangedEventArgs e)
  {
    if (!this.IsVisible)
      return;
    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() => this.Hide()));
  }

  private void Window_Loaded(object sender, RoutedEventArgs e)
  {
    Task.Run(TaskExceptionHelper.ExceptionBoundary((Func<Task>) (async () => Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (async () =>
    {
      UpdateAdConfigWindow updateAdConfigWindow = this;
      await Task.Delay(10000);
      updateAdConfigWindow.Close();
    })))));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/PDFLauncher;component/updateadconfigwindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target)
  {
    if (connectionId == 1)
      ((FrameworkElement) target).Loaded += new RoutedEventHandler(this.Window_Loaded);
    else
      this._contentLoaded = true;
  }
}
