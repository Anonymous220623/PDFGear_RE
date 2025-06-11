// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.DisposableObject
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;

#nullable disable
namespace HandyControl.Data;

public class DisposableObject : IDisposable
{
  private EventHandler _disposing;

  public bool IsDisposed { get; private set; }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  ~DisposableObject() => this.Dispose(false);

  public event EventHandler Disposing
  {
    add
    {
      this.ThrowIfDisposed();
      this._disposing += value;
    }
    remove => this._disposing -= value;
  }

  protected void ThrowIfDisposed()
  {
    if (this.IsDisposed)
      throw new ObjectDisposedException(this.GetType().Name);
  }

  protected void Dispose(bool disposing)
  {
    if (this.IsDisposed)
      return;
    try
    {
      if (disposing)
      {
        EventHandler disposing1 = this._disposing;
        if (disposing1 != null)
          disposing1((object) this, EventArgs.Empty);
        this._disposing = (EventHandler) null;
        this.DisposeManagedResources();
      }
      this.DisposeNativeResources();
    }
    finally
    {
      this.IsDisposed = true;
    }
  }

  protected virtual void DisposeManagedResources()
  {
  }

  protected virtual void DisposeNativeResources()
  {
  }
}
