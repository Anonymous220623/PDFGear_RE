// Decompiled with JetBrains decompiler
// Type: pdfeditor.Utils.ProgressUtils
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using pdfeditor.Controls.ProgressWindow;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

#nullable disable
namespace pdfeditor.Utils;

public static class ProgressUtils
{
  public static bool ShowProgressRing(
    Func<ProgressUtils.ProgressAction, Task> func,
    string title,
    object content,
    bool isCancellable,
    Window ownerWindow,
    int millisecondsDelay = 0)
  {
    return ProgressUtils.ShowProgressDialogCore(func, title, content, InternalProgressMode.ProgressRing, isCancellable, ownerWindow, millisecondsDelay);
  }

  public static bool ShowProgressBar(
    Func<ProgressUtils.ProgressAction, Task> func,
    string title,
    object content,
    bool isCancellable,
    Window ownerWindow,
    int millisecondsDelay = 0)
  {
    return func != null && ProgressUtils.ShowProgressDialogCore(func, title, content, InternalProgressMode.ProgressBar, isCancellable, ownerWindow, millisecondsDelay);
  }

  public static bool ShowProgressDialog(
    Func<ProgressUtils.ProgressAction, Task> func,
    string title,
    object content,
    InternalProgressMode mode,
    bool isCancellable,
    Window ownerWindow,
    int millisecondsDelay = 0)
  {
    return ProgressUtils.ShowProgressDialogCore(func, title, content, mode, isCancellable, ownerWindow, millisecondsDelay);
  }

  private static bool ShowProgressDialogCore(
    Func<ProgressUtils.ProgressAction, Task> func,
    string title,
    object content,
    InternalProgressMode mode,
    bool isCancelEnabled,
    Window ownerWindow,
    int millisecondsDelay)
  {
    if (func == null)
      return false;
    title = title ?? string.Empty;
    Progress<double> progress = new Progress<double>();
    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    InternalProgressWindow internalProgressWindow = new InternalProgressWindow();
    internalProgressWindow.Title = title;
    internalProgressWindow.Content = content;
    internalProgressWindow.IsCancellable = isCancelEnabled;
    internalProgressWindow.Owner = ownerWindow;
    internalProgressWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
    internalProgressWindow.ProgressMode = mode;
    InternalProgressWindow progressWindow = internalProgressWindow;
    ProgressUtils.ProgressAction data = new ProgressUtils.ProgressAction((Action) (() => progressWindow.Complete()), (Func<bool>) (() => progressWindow.IsIndeterminate), (Action<bool>) (c => progressWindow.IsIndeterminate = c), (IProgress<double>) progress, cancellationTokenSource.Token);
    RunCore(progressWindow.Dispatcher, func, data);
    progress.ProgressChanged += (EventHandler<double>) ((s, a) => progressWindow.Value = a);
    progressWindow.ShowDialog(millisecondsDelay);
    if (!progressWindow.IsCompleted)
    {
      cancellationTokenSource.Cancel();
      cancellationTokenSource.Dispose();
    }
    return progressWindow.IsCompleted;

    static async void RunCore(
      Dispatcher _dispatcher,
      Func<ProgressUtils.ProgressAction, Task> _func,
      ProgressUtils.ProgressAction data)
    {
      await _dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (async () =>
      {
        try
        {
          await _func(data);
        }
        catch (OperationCanceledException ex)
        {
        }
      }));
    }
  }

  public class ProgressAction
  {
    private readonly Action setComplete;
    private readonly Action<bool> setIndeterminate;
    private readonly Func<bool> getIndeterminate;
    private readonly IProgress<double> progress;

    public ProgressAction(
      Action setComplete,
      Func<bool> getIndeterminate,
      Action<bool> setIndeterminate,
      IProgress<double> progress,
      CancellationToken cancellationToken)
    {
      this.setComplete = setComplete;
      this.getIndeterminate = getIndeterminate;
      this.setIndeterminate = setIndeterminate;
      this.progress = progress;
      this.CancellationToken = cancellationToken;
    }

    public void Complete() => this.setComplete();

    public void Report(double progress) => this.progress.Report(progress);

    public bool IsIndeterminate
    {
      get => this.getIndeterminate();
      set => this.setIndeterminate(value);
    }

    public CancellationToken CancellationToken { get; }
  }
}
