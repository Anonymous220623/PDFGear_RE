// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Operations.OperationManager
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Nito.AsyncEx;
using Patagames.Pdf.Net;
using pdfeditor.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace pdfeditor.Models.Operations;

public class OperationManager : ObservableObject, IDisposable
{
  private bool disposedValue;
  private 
  #nullable disable
  PdfDocument pdfDocument;
  private List<OperationManager.OperationItem> operations = new List<OperationManager.OperationItem>();
  private int currentIndex = -1;
  private bool canGoBack;
  private bool canGoForward;
  private string version = string.Empty;
  private AsyncLock asyncLocker = new AsyncLock();

  public OperationManager(PdfDocument pdfDocument) => this.pdfDocument = pdfDocument;

  public bool CanGoBack
  {
    get => this.canGoBack;
    private set
    {
      if (!this.SetProperty<bool>(ref this.canGoBack, value, nameof (CanGoBack)))
        return;
      EventHandler stateChanged = this.StateChanged;
      if (stateChanged == null)
        return;
      stateChanged((object) this, EventArgs.Empty);
    }
  }

  public bool CanGoForward
  {
    get => this.canGoForward;
    private set
    {
      if (!this.SetProperty<bool>(ref this.canGoForward, value, nameof (CanGoForward)))
        return;
      EventHandler stateChanged = this.StateChanged;
      if (stateChanged == null)
        return;
      stateChanged((object) this, EventArgs.Empty);
    }
  }

  public string Version
  {
    get => this.version;
    private set
    {
      if (!this.SetProperty<string>(ref this.version, value, nameof (Version)))
        return;
      EventHandler stateChanged = this.StateChanged;
      if (stateChanged == null)
        return;
      stateChanged((object) this, EventArgs.Empty);
    }
  }

  private async Task AddOperationCoreAsync(
    Func<PdfDocument, Task> goback,
    Func<PdfDocument, Task> goforward,
    string tag = "")
  {
    this.ThrowIfDisposed();
    using (await this.asyncLocker.LockAsync())
    {
      this.TryRemoveEndOfQueue();
      this.operations.Add(new OperationManager.OperationItem(goback, goforward, tag));
      this.currentIndex = this.operations.Count - 1;
      this.UpdateCanState();
    }
  }

  public async Task AddOperationAsync(
    Func<PdfDocument, Task> goback,
    Func<PdfDocument, Task> goforward,
    string tag = "")
  {
    await this.AddOperationCoreAsync(goback, goforward, tag).ConfigureAwait(false);
  }

  public async Task AddOperationAsync(
    Action<PdfDocument> goback,
    Func<PdfDocument, Task> goforward,
    string tag = "")
  {
    await this.AddOperationCoreAsync(goback != null ? (Func<PdfDocument, Task>) (d =>
    {
      goback(d);
      return Task.CompletedTask;
    }) : (Func<PdfDocument, Task>) null, goforward, tag).ConfigureAwait(false);
  }

  public async Task AddOperationAsync(
    Func<PdfDocument, Task> goback,
    Action<PdfDocument> goforward,
    string tag = "")
  {
    await this.AddOperationCoreAsync(goback, goforward != null ? (Func<PdfDocument, Task>) (d =>
    {
      goforward(d);
      return Task.CompletedTask;
    }) : (Func<PdfDocument, Task>) null, tag).ConfigureAwait(false);
  }

  public async Task AddOperationAsync(
    Action<PdfDocument> goback,
    Action<PdfDocument> goforward,
    string tag = "")
  {
    await this.AddOperationCoreAsync(goback != null ? (Func<PdfDocument, Task>) (d =>
    {
      goback(d);
      return Task.CompletedTask;
    }) : (Func<PdfDocument, Task>) null, goforward != null ? (Func<PdfDocument, Task>) (d =>
    {
      goforward(d);
      return Task.CompletedTask;
    }) : (Func<PdfDocument, Task>) null, tag).ConfigureAwait(false);
  }

  public async Task ClearAsync()
  {
    OperationManager operationManager = this;
    using (await operationManager.asyncLocker.LockAsync().ConfigureAwait(false))
    {
      operationManager.operations.Clear();
      operationManager.currentIndex = -1;
      // ISSUE: reference to a compiler-generated method
      DispatcherHelper.UIDispatcher.Invoke(new Action(operationManager.\u003CClearAsync\u003Eb__23_0));
    }
  }

  public async Task GoBackAsync()
  {
    OperationManager sender = this;
    if (!sender.CanGoBack)
      throw new ArgumentException("CanGoBack");
    EventHandler operationInvoked1 = OperationManager.BeforeOperationInvoked;
    if (operationInvoked1 != null)
      operationInvoked1((object) sender, EventArgs.Empty);
    try
    {
      using (await sender.asyncLocker.LockAsync())
      {
        await RunCore(sender.operations[sender.currentIndex].GoBack, sender.pdfDocument);
        --sender.currentIndex;
        sender.UpdateCanState();
      }
    }
    finally
    {
      EventHandler operationInvoked2 = OperationManager.AfterOperationInvoked;
      if (operationInvoked2 != null)
        operationInvoked2((object) sender, EventArgs.Empty);
    }

    static async Task RunCore(Func<PdfDocument, Task> func, PdfDocument doc)
    {
      await func(doc).ConfigureAwait(false);
    }
  }

  public async Task GoForwardAsync()
  {
    OperationManager sender = this;
    if (!sender.CanGoForward)
      throw new ArgumentException("CanGoForward");
    EventHandler operationInvoked1 = OperationManager.BeforeOperationInvoked;
    if (operationInvoked1 != null)
      operationInvoked1((object) sender, EventArgs.Empty);
    try
    {
      using (await sender.asyncLocker.LockAsync())
      {
        int idx = sender.currentIndex + 1;
        await RunCore(sender.operations[idx].GoForward, sender.pdfDocument);
        sender.currentIndex = idx;
        sender.UpdateCanState();
      }
    }
    finally
    {
      EventHandler operationInvoked2 = OperationManager.AfterOperationInvoked;
      if (operationInvoked2 != null)
        operationInvoked2((object) sender, EventArgs.Empty);
    }

    static async Task RunCore(Func<PdfDocument, Task> func, PdfDocument doc)
    {
      await func(doc).ConfigureAwait(false);
    }
  }

  private void UpdateCanState()
  {
    string newValue = string.Empty;
    if (this.operations != null && this.currentIndex > -1)
      newValue = this.operations[this.currentIndex].Version;
    if (!(this.SetProperty<bool>(ref this.canGoBack, this.currentIndex > -1, "CanGoBack") | this.SetProperty<bool>(ref this.canGoForward, this.operations != null && this.currentIndex < this.operations.Count - 1, "CanGoForward") | this.SetProperty<string>(ref this.version, newValue, "Version")))
      return;
    EventHandler stateChanged = this.StateChanged;
    if (stateChanged == null)
      return;
    stateChanged((object) this, EventArgs.Empty);
  }

  public event EventHandler StateChanged;

  public static event EventHandler BeforeOperationInvoked;

  public static event EventHandler AfterOperationInvoked;

  private void TryRemoveEndOfQueue()
  {
    if (this.currentIndex >= this.operations.Count - 1)
      return;
    this.operations.RemoveRange(this.currentIndex + 1, this.operations.Count - this.currentIndex - 1);
  }

  private void ThrowIfDisposed()
  {
    if (this.disposedValue)
      throw new ObjectDisposedException(nameof (OperationManager));
  }

  protected virtual void Dispose(bool disposing)
  {
    if (this.disposedValue)
      return;
    if (disposing)
      this.pdfDocument = (PdfDocument) null;
    this.disposedValue = true;
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  public class OperationItem
  {
    public OperationItem(
      Func<PdfDocument, Task> goback,
      Func<PdfDocument, Task> goforward,
      string tag)
    {
      this.GoBack = goback;
      this.GoForward = goforward;
      this.Tag = tag;
      this.Version = Guid.NewGuid().ToString();
    }

    public string Version { get; }

    public string Tag { get; }

    public Func<PdfDocument, Task> GoBack { get; }

    public Func<PdfDocument, Task> GoForward { get; }

    public void Deconstruct(
      out Func<PdfDocument, Task> goback,
      out Func<PdfDocument, Task> goforward)
    {
      goback = this.GoBack;
      goforward = this.GoForward;
    }
  }
}
