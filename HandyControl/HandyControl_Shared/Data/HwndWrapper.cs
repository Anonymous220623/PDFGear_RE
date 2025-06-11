// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.HwndWrapper
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Tools.Interop;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace HandyControl.Data;

public abstract class HwndWrapper : DisposableObject
{
  private IntPtr _handle;
  private bool _isHandleCreationAllowed = true;
  private ushort _wndClassAtom;
  private Delegate _wndProc;

  protected ushort WindowClassAtom
  {
    get
    {
      if (this._wndClassAtom == (ushort) 0)
        this._wndClassAtom = this.CreateWindowClassCore();
      return this._wndClassAtom;
    }
  }

  public IntPtr Handle
  {
    get
    {
      this.EnsureHandle();
      return this._handle;
    }
  }

  protected virtual bool IsWindowSubclassed => false;

  protected virtual ushort CreateWindowClassCore() => this.RegisterClass(Guid.NewGuid().ToString());

  protected virtual void DestroyWindowClassCore()
  {
    if (this._wndClassAtom == (ushort) 0)
      return;
    InteropMethods.UnregisterClass(new IntPtr((int) this._wndClassAtom), InteropMethods.GetModuleHandle((string) null));
    this._wndClassAtom = (ushort) 0;
  }

  protected ushort RegisterClass(string className)
  {
    return InteropMethods.RegisterClass(ref new InteropValues.WNDCLASS()
    {
      cbClsExtra = 0,
      cbWndExtra = 0,
      hbrBackground = IntPtr.Zero,
      hCursor = IntPtr.Zero,
      hIcon = IntPtr.Zero,
      lpfnWndProc = this._wndProc = (Delegate) new InteropValues.WndProc(this.WndProc),
      lpszClassName = className,
      lpszMenuName = (string) null,
      style = 0U
    });
  }

  private void SubclassWndProc()
  {
    this._wndProc = (Delegate) new InteropValues.WndProc(this.WndProc);
    InteropMethods.SetWindowLong(this._handle, -4, Marshal.GetFunctionPointerForDelegate(this._wndProc));
  }

  protected abstract IntPtr CreateWindowCore();

  protected virtual void DestroyWindowCore()
  {
    if (!(this._handle != IntPtr.Zero))
      return;
    InteropMethods.DestroyWindow(this._handle);
    this._handle = IntPtr.Zero;
  }

  protected virtual IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
  {
    return InteropMethods.DefWindowProc(hwnd, msg, wParam, lParam);
  }

  public void EnsureHandle()
  {
    if (!(this._handle == IntPtr.Zero) || !this._isHandleCreationAllowed)
      return;
    this._isHandleCreationAllowed = false;
    this._handle = this.CreateWindowCore();
    if (!this.IsWindowSubclassed)
      return;
    this.SubclassWndProc();
  }

  protected override void DisposeNativeResources()
  {
    this._isHandleCreationAllowed = false;
    this.DestroyWindowCore();
    this.DestroyWindowClassCore();
  }
}
