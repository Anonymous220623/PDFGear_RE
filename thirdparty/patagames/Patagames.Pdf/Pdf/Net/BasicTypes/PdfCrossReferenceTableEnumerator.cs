// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.BasicTypes.PdfCrossReferenceTableEnumerator
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.BasicTypes;

internal class PdfCrossReferenceTableEnumerator : 
  IEnumerator<PdfCrossRefItem>,
  IDisposable,
  IEnumerator
{
  private PdfCrossReferenceTable _table;
  private IntPtr _it = IntPtr.Zero;
  private PdfCrossRefItem _currentInfo;

  public PdfCrossReferenceTableEnumerator(PdfCrossReferenceTable table)
  {
    this._table = table;
    this._it = Pdfium.FPDFCROSSREF_GetItemIterator(this._table.Handle);
  }

  public void Dispose() => this.Dispose(true);

  protected virtual void Dispose(bool disposing)
  {
    if (this._it != IntPtr.Zero)
      Pdfium.FPDFCROSSREF_ReleaseItemIterator(this._it);
    this._it = IntPtr.Zero;
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  public PdfCrossRefItem Current => this._currentInfo;

  object IEnumerator.Current => (object) this.Current;

  public bool MoveNext()
  {
    int objNum;
    int position;
    int type;
    int gennum;
    if (Pdfium.FPDFCROSSREF_GetItem(this._it, out objNum, out position, out type, out gennum))
    {
      this._currentInfo = new PdfCrossRefItem()
      {
        GenerationNumber = gennum,
        ObjectNumber = objNum,
        Position = position,
        ObjectType = type
      };
      return true;
    }
    this._currentInfo = new PdfCrossRefItem();
    return false;
  }

  public void Reset()
  {
    this._currentInfo = new PdfCrossRefItem();
    this.Dispose();
  }
}
