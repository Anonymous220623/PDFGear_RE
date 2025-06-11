// Decompiled with JetBrains decompiler
// Type: Tesseract.ChoiceIterator
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

using System;
using System.Runtime.InteropServices;
using Tesseract.Interop;

#nullable disable
namespace Tesseract;

public sealed class ChoiceIterator : DisposableBase
{
  private readonly HandleRef _handleRef;

  internal ChoiceIterator(IntPtr handle) => this._handleRef = new HandleRef((object) this, handle);

  public bool Next()
  {
    this.VerifyNotDisposed();
    return !(this._handleRef.Handle == IntPtr.Zero) && TessApi.Native.ChoiceIteratorNext(this._handleRef) != 0;
  }

  public float GetConfidence()
  {
    this.VerifyNotDisposed();
    return this._handleRef.Handle == IntPtr.Zero ? 0.0f : TessApi.Native.ChoiceIteratorGetConfidence(this._handleRef);
  }

  public string GetText()
  {
    this.VerifyNotDisposed();
    return this._handleRef.Handle == IntPtr.Zero ? string.Empty : TessApi.ChoiceIteratorGetUTF8Text(this._handleRef);
  }

  protected override void Dispose(bool disposing)
  {
    if (!(this._handleRef.Handle != IntPtr.Zero))
      return;
    TessApi.Native.ChoiceIteratorDelete(this._handleRef);
  }
}
