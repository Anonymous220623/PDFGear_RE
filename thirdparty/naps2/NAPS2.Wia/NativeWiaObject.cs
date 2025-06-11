// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.NativeWiaObject
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace NAPS2.Wia;

public abstract class NativeWiaObject : IDisposable
{
  private bool _disposed;
  private IntPtr _handle;

  public static WiaVersion DefaultWiaVersion
  {
    get
    {
      if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        throw new InvalidOperationException("Wia is only supported on Windows.");
      return Environment.OSVersion.Version.Major < 6 ? WiaVersion.Wia10 : WiaVersion.Wia20;
    }
  }

  protected NativeWiaObject(WiaVersion version, IntPtr handle)
  {
    if (version == WiaVersion.Default)
      version = NativeWiaObject.DefaultWiaVersion;
    this.Version = version;
    this.Handle = handle;
  }

  protected NativeWiaObject(WiaVersion version)
    : this(version, IntPtr.Zero)
  {
  }

  protected IntPtr Handle
  {
    get
    {
      this.EnsureNotDisposed();
      return this._handle;
    }
    set => this._handle = value;
  }

  public WiaVersion Version { get; }

  private void EnsureNotDisposed()
  {
    if (this._disposed)
      throw new ObjectDisposedException(this.GetType().FullName);
  }

  protected virtual void Dispose(bool disposing)
  {
    if (this._disposed)
      return;
    if (this.Handle != IntPtr.Zero)
      Marshal.Release(this.Handle);
    this._disposed = true;
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  ~NativeWiaObject() => this.Dispose(false);
}
