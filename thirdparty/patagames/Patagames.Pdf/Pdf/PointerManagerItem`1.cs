// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.PointerManagerItem`1
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

internal class PointerManagerItem<T> : IDisposable, IPointerManagerItem
{
  protected Type _object1Type;
  protected IntPtr _object1;
  protected T _obj;

  public IntPtr Key { get; set; }

  public bool IsDisposed { get; private set; }

  public virtual IntPtr this[int index]
  {
    get
    {
      if (this.IsDisposed)
        throw new ObjectDisposedException(nameof (PointerManagerItem<T>));
      if (index == 0)
        return this._object1;
      throw new IndexOutOfRangeException();
    }
  }

  public T Obj1 => this._obj;

  public PointerManagerItem(T structure)
  {
    this._obj = structure;
    this.IsNeedCheckForMemoryLeaks = true;
    this.Key = IntPtr.Zero;
    this._object1Type = structure.GetType();
    this._object1 = Marshal.AllocHGlobal(Marshal.SizeOf<T>(structure));
    Marshal.StructureToPtr<T>(structure, this._object1, false);
  }

  public void Dispose() => this.Dispose(true);

  /// <summary>Releases all resources used by the PdfImageObject.</summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !Pdfium.IsInitialize)
      return;
    Marshal.DestroyStructure(this._object1, this._object1Type);
    Marshal.FreeHGlobal(this._object1);
    this.IsDisposed = true;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  /// <summary>Finalize object</summary>
  ~PointerManagerItem()
  {
    int num = this.IsNeedCheckForMemoryLeaks ? 1 : 0;
  }

  public bool IsNeedCheckForMemoryLeaks { get; set; }
}
