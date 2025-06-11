// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.REFOBJ
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net;
using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Patagames.Pdf;

/// <summary>Represents the object that referred by other object</summary>
public class REFOBJ
{
  private PdfDocument _doc;
  private static int __unsafeSize;
  private PdfIndirectList _list;

  internal static int _unsafeSize
  {
    get
    {
      if (REFOBJ.__unsafeSize <= 0)
        REFOBJ.__unsafeSize = Marshal.SizeOf<REFOBJ.REFOBJInternal>(new REFOBJ.REFOBJInternal());
      return REFOBJ.__unsafeSize;
    }
  }

  /// <summary>
  /// Gets the number of the object which is referred by <see cref="P:Patagames.Pdf.REFOBJ.ReferredBy" /> objects.
  /// </summary>
  public int ObjectNumber { get; private set; }

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeBase" /> object that is reffered by <see cref="P:Patagames.Pdf.REFOBJ.ReferredBy" /> objects or null if such object does not exists.
  /// </summary>
  public PdfTypeBase ReferTo
  {
    get
    {
      if (this._list == null)
        this._list = PdfIndirectList.FromPdfDocument(this._doc);
      return this._list[this.ObjectNumber];
    }
  }

  /// <summary>
  /// Gets an array of objects which are referred to <see cref="P:Patagames.Pdf.REFOBJ.ObjectNumber" />/<see cref="P:Patagames.Pdf.REFOBJ.ReferTo" />.
  /// </summary>
  public PdfTypeBase[] ReferredBy { get; private set; }

  internal REFOBJ(PdfDocument doc, IntPtr handle)
  {
    this._doc = doc;
    REFOBJ.REFOBJInternal structure = Marshal.PtrToStructure<REFOBJ.REFOBJInternal>(handle);
    this.ReferredBy = new PdfTypeBase[structure.objCount];
    this.ObjectNumber = structure.objNum;
    for (int index = 0; index < structure.objCount; ++index)
    {
      IntPtr handle1 = Marshal.ReadIntPtr(new IntPtr(structure.objects.ToInt64() + (long) (IntPtr.Size * index)));
      if (handle1 != IntPtr.Zero)
        this.ReferredBy[index] = PdfTypeBase.Create(handle1);
    }
  }

  private struct REFOBJInternal
  {
    public int objNum;
    public int objCount;
    public IntPtr objects;
  }
}
