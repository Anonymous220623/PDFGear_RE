// Decompiled with JetBrains decompiler
// Type: PDFKit.DispatcherISyncInvoke
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;

#nullable disable
namespace PDFKit;

internal class DispatcherISyncInvoke : ISynchronizeInvoke
{
  private readonly Dispatcher _dispatcher;

  public DispatcherISyncInvoke(Dispatcher dispatcher) => this._dispatcher = dispatcher;

  public IAsyncResult BeginInvoke(Delegate method, object[] args)
  {
    return (IAsyncResult) new DispatcherISyncInvoke.DispatcherOperationAsync(this._dispatcher.BeginInvoke(method, args));
  }

  public object EndInvoke(IAsyncResult result)
  {
    result.AsyncWaitHandle.WaitOne();
    return result is DispatcherISyncInvoke.DispatcherOperationAsync ? ((DispatcherISyncInvoke.DispatcherOperationAsync) result).Result : (object) null;
  }

  public object Invoke(Delegate method, object[] args)
  {
    return this.InvokeRequired ? this.EndInvoke(this.BeginInvoke(method, args)) : method.DynamicInvoke(args);
  }

  public bool InvokeRequired => !this._dispatcher.CheckAccess();

  private class DispatcherOperationAsync : IAsyncResult, IDisposable
  {
    private readonly DispatcherOperation _dop;
    private ManualResetEvent _handle = new ManualResetEvent(false);

    public DispatcherOperationAsync(DispatcherOperation dispatcherOperation)
    {
      this._dop = dispatcherOperation;
      this._dop.Aborted += new EventHandler(this.DopAborted);
      this._dop.Completed += new EventHandler(this.DopCompleted);
    }

    public object Result
    {
      get
      {
        if (!this.IsCompleted)
          throw new InvalidAsynchronousStateException("Not Completed");
        return this._dop.Result;
      }
    }

    private void DopCompleted(object sender, EventArgs e) => this._handle.Set();

    private void DopAborted(object sender, EventArgs e) => this._handle.Set();

    public bool IsCompleted => this._dop.Status == DispatcherOperationStatus.Completed;

    public WaitHandle AsyncWaitHandle => (WaitHandle) this._handle;

    public object AsyncState => (object) null;

    public bool CompletedSynchronously => false;

    public void Dispose()
    {
      if (this._handle == null)
        return;
      this._handle.Dispose();
      this._handle = (ManualResetEvent) null;
    }
  }
}
