// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.MgrIntObj
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net;

internal class MgrIntObj
{
  private Dictionary<IntPtr, MgrIntObjVal> _list = new Dictionary<IntPtr, MgrIntObjVal>();

  public PdfTypeBase Create(IntPtr Handle)
  {
    if (Handle == IntPtr.Zero)
      return (PdfTypeBase) null;
    IndirectObjectTypes type = Pdfium.FPDFOBJ_GetType(Handle);
    if (this._list.ContainsKey(Handle))
    {
      MgrIntObjVal mgrIntObjVal = this._list[Handle];
      if (!mgrIntObjVal.obj.IsDisposed && mgrIntObjVal.objType == type)
        return mgrIntObjVal.obj;
      this._list.Remove(Handle);
    }
    MgrIntObjVal mgrIntObjVal1 = new MgrIntObjVal()
    {
      obj = PdfTypeBase.Create(Handle),
      objType = type
    };
    this._list.Add(Handle, mgrIntObjVal1);
    return mgrIntObjVal1.obj;
  }

  internal void Add(PdfTypeBase item)
  {
    if (item == null || this._list.ContainsKey(item.Handle))
      return;
    MgrIntObjVal mgrIntObjVal = new MgrIntObjVal()
    {
      obj = item,
      objType = item.ObjectType
    };
    this._list.Add(item.Handle, mgrIntObjVal);
  }

  internal void Remove(IntPtr handle)
  {
    if (!this._list.ContainsKey(handle))
      return;
    this._list.Remove(handle);
  }

  internal void Clear() => this._list.Clear();
}
