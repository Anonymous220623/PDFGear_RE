// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.MgrPageObj
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using System;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net;

internal class MgrPageObj
{
  private Dictionary<IntPtr, MgrPageObjVal> _list = new Dictionary<IntPtr, MgrPageObjVal>();

  public PdfPageObject Create(IntPtr Handle)
  {
    if (Handle == IntPtr.Zero)
      return (PdfPageObject) null;
    PageObjectTypes type = Pdfium.FPDFPageObj_GetType(Handle);
    if (this._list.ContainsKey(Handle))
    {
      MgrPageObjVal mgrPageObjVal = this._list[Handle];
      if (mgrPageObjVal.objType == type)
        return mgrPageObjVal.obj;
      this._list.Remove(Handle);
    }
    MgrPageObjVal mgrPageObjVal1 = new MgrPageObjVal()
    {
      obj = PdfPageObject.Create(Handle),
      objType = type
    };
    this._list.Add(Handle, mgrPageObjVal1);
    return mgrPageObjVal1.obj;
  }

  internal void Add(PdfPageObject item)
  {
    if (item == null || this._list.ContainsKey(item.Handle))
      return;
    MgrPageObjVal mgrPageObjVal = new MgrPageObjVal()
    {
      obj = item,
      objType = item.ObjectType
    };
    this._list.Add(item.Handle, mgrPageObjVal);
  }

  internal void Remove(IntPtr handle)
  {
    if (!this._list.ContainsKey(handle))
      return;
    this._list.Remove(handle);
  }

  internal void Clear() => this._list.Clear();

  internal PdfPageObject Get(IntPtr handle)
  {
    return this._list.ContainsKey(handle) ? this._list[handle].obj : (PdfPageObject) null;
  }
}
