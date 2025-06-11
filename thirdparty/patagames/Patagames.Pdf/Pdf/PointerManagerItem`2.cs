// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.PointerManagerItem`2
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

internal class PointerManagerItem<T1, T2> : PointerManagerItem<T1>
{
  protected Type _object2Type;
  protected IntPtr _object2;
  protected T2 _obj2;

  public override IntPtr this[int index]
  {
    get
    {
      if (this.IsDisposed)
        throw new ObjectDisposedException(nameof (PointerManagerItem<T1, T2>));
      if (index == 0)
        return this._object1;
      if (index == 1)
        return this._object2;
      throw new IndexOutOfRangeException();
    }
  }

  public T2 Obj2 => this._obj2;

  public PointerManagerItem(T1 structure1, T2 structure2)
    : base(structure1)
  {
    this._obj2 = structure2;
    this._object2Type = structure2.GetType();
    this._object2 = Marshal.AllocHGlobal(Marshal.SizeOf<T2>(structure2));
    Marshal.StructureToPtr<T2>(structure2, this._object2, false);
  }

  /// <summary>Releases all resources used by the PdfImageObject.</summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected override void Dispose(bool disposing)
  {
    if (this.IsDisposed || !Pdfium.IsInitialize)
      return;
    Marshal.DestroyStructure(this._object2, this._object2Type);
    Marshal.FreeHGlobal(this._object2);
    base.Dispose(disposing);
  }
}
