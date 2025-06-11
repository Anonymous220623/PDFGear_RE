// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.ProgressWindow.InternalProgressWindow
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Controls.ProgressWindow;

public partial class InternalProgressWindow : Window, IComponentConnector
{
  private CancellationTokenSource showDialogCts;
  public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(nameof (IsIndeterminate), typeof (bool), typeof (InternalProgressWindow), new PropertyMetadata((object) false));
  public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof (Value), typeof (double), typeof (InternalProgressWindow), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty ProgressModeProperty = DependencyProperty.Register(nameof (ProgressMode), typeof (InternalProgressMode), typeof (InternalProgressWindow), new PropertyMetadata((object) InternalProgressMode.ProgressBar, new PropertyChangedCallback(InternalProgressWindow.OnProgressModePropertyChanged)));
  public static readonly DependencyProperty IsCancellableProperty = DependencyProperty.Register(nameof (IsCancellable), typeof (bool), typeof (InternalProgressWindow), new PropertyMetadata((object) true, new PropertyChangedCallback(InternalProgressWindow.OnIsCancellablePropertyChanged)));
  public static readonly DependencyProperty IsCompletedProperty;
  public static readonly DependencyPropertyKey IsCompletedPropertyKey = DependencyProperty.RegisterReadOnly(nameof (IsCompleted), typeof (bool), typeof (InternalProgressWindow), new PropertyMetadata((object) false));
  private bool _contentLoaded;

  static InternalProgressWindow()
  {
    InternalProgressWindow.IsCompletedProperty = InternalProgressWindow.IsCompletedPropertyKey.DependencyProperty;
  }

  public InternalProgressWindow()
  {
    this.InitializeComponent();
    this.showDialogCts = new CancellationTokenSource();
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.UpdateProgressModeState();
    this.UpdateCancelStates();
  }

  public bool IsIndeterminate
  {
    get => (bool) this.GetValue(InternalProgressWindow.IsIndeterminateProperty);
    set => this.SetValue(InternalProgressWindow.IsIndeterminateProperty, (object) value);
  }

  public double Value
  {
    get => (double) this.GetValue(InternalProgressWindow.ValueProperty);
    set => this.SetValue(InternalProgressWindow.ValueProperty, (object) value);
  }

  public InternalProgressMode ProgressMode
  {
    get => (InternalProgressMode) this.GetValue(InternalProgressWindow.ProgressModeProperty);
    set => this.SetValue(InternalProgressWindow.ProgressModeProperty, (object) value);
  }

  private static void OnProgressModePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (object.Equals(e.NewValue, e.OldValue) || !(d is InternalProgressWindow internalProgressWindow))
      return;
    internalProgressWindow.UpdateProgressModeState();
  }

  private void UpdateProgressModeState()
  {
    if (this.ProgressMode == InternalProgressMode.ProgressRing)
      VisualStateManager.GoToState((FrameworkElement) this, "RingMode", true);
    else
      VisualStateManager.GoToState((FrameworkElement) this, "BarMode", true);
  }

  public bool IsCancellable
  {
    get => (bool) this.GetValue(InternalProgressWindow.IsCancellableProperty);
    set => this.SetValue(InternalProgressWindow.IsCancellableProperty, (object) value);
  }

  private static void OnIsCancellablePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (object.Equals(e.NewValue, e.OldValue) || !(d is InternalProgressWindow internalProgressWindow))
      return;
    internalProgressWindow.UpdateCancelStates();
  }

  private void UpdateCancelStates()
  {
    if (this.IsCancellable)
      VisualStateManager.GoToState((FrameworkElement) this, "CanCancel", true);
    else
      VisualStateManager.GoToState((FrameworkElement) this, "CannotCancel", true);
  }

  public bool IsCompleted
  {
    get => (bool) this.GetValue(InternalProgressWindow.IsCompletedProperty);
    private set => this.SetValue(InternalProgressWindow.IsCompletedPropertyKey, (object) value);
  }

  public void Complete()
  {
    this.IsCompleted = true;
    this.showDialogCts.Cancel();
    if (!this.IsVisible)
      return;
    this.Close();
  }

  protected override void OnClosing(CancelEventArgs e)
  {
    if (!this.IsCompleted && !this.IsCancellable)
      e.Cancel = true;
    this.showDialogCts.Cancel();
    base.OnClosing(e);
  }

  public bool? ShowDialog(int millisecondsDelay)
  {
    if (millisecondsDelay <= 0)
      return this.ShowDialog();
    DispatcherFrame frame = new DispatcherFrame(true)
    {
      Continue = true
    };
    RunCore();
    Dispatcher.PushFrame(frame);
    return this.showDialogCts.IsCancellationRequested ? new bool?() : this.ShowDialog();

    async void RunCore()
    {
      try
      {
        await Task.Delay(millisecondsDelay, this.showDialogCts.Token);
      }
      catch
      {
      }
      frame.Continue = false;
    }
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  public void InitializeComponent()
  {
    if (this._contentLoaded)
      return;
    this._contentLoaded = true;
    Application.LoadComponent((object) this, new Uri("/pdfeditor;component/controls/progresswindow/internalprogresswindow.xaml", UriKind.Relative));
  }

  [DebuggerNonUserCode]
  [GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
  [EditorBrowsable(EditorBrowsableState.Never)]
  void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;
}
