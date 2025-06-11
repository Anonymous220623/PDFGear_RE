// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.BasicTypes.PdfTypeIndirect
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using System;

#nullable disable
namespace Patagames.Pdf.Net.BasicTypes;

/// <summary>Represents the indirect objects</summary>
/// <remarks>
/// Any object in a PDF file may be labeled as an indirect object.
/// This gives the object a unique object identifier by which other objects can refer to it
/// (for example, as an element of an array or as the value of a dictionary entry).
/// The object identifier consists of two parts:
/// <list type="bullet">
/// <item>A positive integer object number. Indirect objects are often numbered sequentially within a PDF file,
/// but this is not required; object numbers may be assigned in any arbitrary order.</item>
/// <item>A non-negative integer generation number. In a newly created file, all indirect objects have generation numbers of 0.
/// Nonzero generation numbers may be introduced when the file is later updated.</item>
/// </list>
/// Together, the combination of an object number and a generation number uniquely identifies an indirect object.
/// The object retains the same object number and generation number throughout its existence, even if its value is modified.
/// <note type="note">In the data structures that make up a PDF document, certain values are required to be specified as
/// indirect object references. Except where this is explicitly called out, any object (other than a stream) may be specified
/// either directly or as an indirect object reference; the semantics are entirely equivalent.
/// Note in particular that content streams, which define the visible contents of the document,
/// may not contain indirect references.</note>
/// </remarks>
/// <summary>
/// Construct new instance of PdfTypeIndirect class from given Handle
/// </summary>
/// <param name="Handle">A handle to an unmanaged indirect object.</param>
public class PdfTypeIndirect(IntPtr Handle) : PdfTypeBase(Handle)
{
  private PdfTypeBase _direct;
  private IndirectObjectTypes _directType;
  private PdfIndirectList _list;

  /// <summary>Gets the direct object for this indirect object</summary>
  public PdfTypeBase Direct
  {
    get
    {
      IntPtr direct = Pdfium.FPDFOBJ_GetDirect(this.Handle);
      if (direct == IntPtr.Zero)
      {
        this._direct = (PdfTypeBase) null;
        return (PdfTypeBase) null;
      }
      IndirectObjectTypes type = Pdfium.FPDFOBJ_GetType(direct);
      if (this._direct != null && this._direct.Handle == direct && this._directType == type)
        return this._direct;
      this._direct = PdfTypeBase.Create(direct);
      this._directType = type;
      return this._direct;
    }
  }

  /// <summary>
  /// Gets a list of objects in which is linked with the specified reference object.
  /// </summary>
  public PdfIndirectList List
  {
    get
    {
      IntPtr objList = Pdfium.FPDFREF_GetObjList(this.Handle);
      if (objList == IntPtr.Zero)
      {
        this._list = (PdfIndirectList) null;
        return (PdfIndirectList) null;
      }
      if (this._list != null && this._list.Handle == objList)
        return this._list;
      this._list = new PdfIndirectList(objList);
      return this._list;
    }
  }

  /// <summary>
  /// Gets an object number which is linked with the specified reference object.
  /// </summary>
  public int Number => Pdfium.FPDFREF_GetRefObjNum(this.Handle);

  /// <summary>Create an instance of an indirect object.</summary>
  /// <param name="list">The <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfIndirectList" /> which contains this object for which the indiret object is created.</param>
  /// <param name="num">Object number for which the indiret object is created.</param>
  /// <returns>The instance of a newly created object</returns>
  public static PdfTypeIndirect Create(PdfIndirectList list, int num)
  {
    IntPtr Handle = Pdfium.FPDFREF_Create(list != null ? list.Handle : IntPtr.Zero, num);
    return !(Handle == IntPtr.Zero) ? new PdfTypeIndirect(Handle)
    {
      _list = list
    } : throw Pdfium.ProcessLastError();
  }

  /// <summary>Creates new instance of PdfTypeIndirect class</summary>
  /// <param name="handle">A handle to an unmanaged indirect object.</param>
  /// <returns>An instance of PdfTypeIndirect</returns>
  public static PdfTypeIndirect Create(IntPtr handle)
  {
    return !(handle == IntPtr.Zero) ? new PdfTypeIndirect(handle) : throw new ArgumentException();
  }

  /// <summary>
  /// Changes a link of Reference object to the specified object in the specified list.
  /// </summary>
  /// <param name="list">List of indirect objects</param>
  /// <param name="num">Object's number in the specified list</param>
  public void SetDirect(PdfIndirectList list, int num)
  {
    Pdfium.FPDFREF_SetRef(this.Handle, list != null ? list.Handle : IntPtr.Zero, num);
    this._list = list;
  }
}
