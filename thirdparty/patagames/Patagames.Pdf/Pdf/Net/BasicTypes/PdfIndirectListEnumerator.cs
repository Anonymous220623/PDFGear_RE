// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.BasicTypes.PdfIndirectListEnumerator
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.BasicTypes;

internal class PdfIndirectListEnumerator : IEnumerator<PdfTypeBase>, IDisposable, IEnumerator
{
  private PdfIndirectList _list;
  private IntPtr _it = IntPtr.Zero;
  private int _currentKey;

  public PdfIndirectListEnumerator(PdfIndirectList list)
  {
    this._list = list;
    this._it = Pdfium.FPDFHOLDER_GetItemIterator(list.Handle);
  }

  public void Dispose() => this.Dispose(true);

  protected virtual void Dispose(bool disposing)
  {
    if (this._it != IntPtr.Zero)
      Pdfium.FPDFHOLDER_ReleaseItemIterator(this._it);
    this._it = IntPtr.Zero;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  public PdfTypeBase Current => this._list[this._currentKey];

  object IEnumerator.Current => (object) this.Current;

  public bool MoveNext() => Pdfium.FPDFHOLDER_GetItem(this._it, out this._currentKey, out IntPtr _);

  public void Reset()
  {
    this._currentKey = 0;
    this.Dispose();
  }
}
