// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.BasicTypes.PdfTypeBase
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.Exceptions;
using System;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.BasicTypes;

/// <summary>Represents the base class for PDF types</summary>
/// <remarks>
/// PDF supports eight basic types of objects:
/// <list type="bullet">
/// <item>Boolean values</item>
/// <item>Integer and real numbers</item>
/// <item>Strings</item>
/// <item>Names</item>
/// <item>Arrays</item>
/// <item>Dictionaries</item>
/// <item>Streams</item>
/// <item>The null object</item>
/// </list>
/// Objects may be labeled so that they can be referred to by other objects.
/// A labeled object is called an indirect object.
/// </remarks>
public abstract class PdfTypeBase : IDisposable
{
  private IntPtr _handle;
  private static Pdfium.InternalObjectDestroyCallback _onDestroyCallback = new Pdfium.InternalObjectDestroyCallback(PdfTypeBase.OnDestroyCalback);
  private static Dictionary<IntPtr, List<PdfTypeBase>> _mgrCallbacks = new Dictionary<IntPtr, List<PdfTypeBase>>();
  private static object _lockMgr = new object();

  /// <summary>
  /// Gets the Pdfium SDK handle that the object is bound to
  /// </summary>
  public IntPtr Handle
  {
    get
    {
      if (this.IsDisposed)
        throw new ObjectDisposedException((string) null);
      return this._handle;
    }
  }

  /// <summary>
  /// Gets a value indicating whether the object has been disposed of.
  /// </summary>
  /// <value>true if object has been disposed of; otherwise, false.</value>
  public bool IsDisposed { get; private set; }

  /// <summary>Gets the type of a PDF object</summary>
  public IndirectObjectTypes ObjectType => Pdfium.FPDFOBJ_GetType(this.Handle);

  /// <summary>
  /// Get a positive integer object number. Indirect objects are often numbered sequentially within a PDF file, but this is not required; object numbers may be assigned in any arbitrary order.
  /// </summary>
  public int ObjectNumber => Pdfium.FPDFOBJ_GetObjNum(this.Handle);

  /// <summary>
  /// Get a non-negative integer generation number. In a newly created file, all indirect objects have generation numbers of 0. Nonzero generation numbers may be introduced when the file is later updated.
  /// </summary>
  public int GenerationNumber => Pdfium.FPDFOBJ_GetGenNum(this.Handle);

  /// <summary>
  /// Construct new instance of IntObjBase class from given Handle
  /// </summary>
  /// <param name="Handle">A handle to an unmanaged object.</param>
  protected PdfTypeBase(IntPtr Handle)
  {
    this._handle = Handle;
    this.SubscribeOnDestroy();
  }

  /// <summary>
  /// Identifies the type of the specified object and creates an instance of this type.
  /// </summary>
  /// <param name="handle">A handle to an unmanaged object.</param>
  /// <returns>The instance of a newly created object.</returns>
  public static PdfTypeBase Create(IntPtr handle)
  {
    if (handle == IntPtr.Zero)
      return (PdfTypeBase) null;
    switch (Pdfium.FPDFOBJ_GetType(handle))
    {
      case IndirectObjectTypes.Boolean:
        return (PdfTypeBase) new PdfTypeBoolean(handle);
      case IndirectObjectTypes.Number:
        return (PdfTypeBase) new PdfTypeNumber(handle);
      case IndirectObjectTypes.String:
        return (PdfTypeBase) new PdfTypeString(handle);
      case IndirectObjectTypes.Name:
        return (PdfTypeBase) new PdfTypeName(handle);
      case IndirectObjectTypes.Array:
        return (PdfTypeBase) new PdfTypeArray(handle);
      case IndirectObjectTypes.Dictionary:
        return (PdfTypeBase) new PdfTypeDictionary(handle);
      case IndirectObjectTypes.Stream:
        return (PdfTypeBase) new PdfTypeStream(handle);
      case IndirectObjectTypes.Null:
        return (PdfTypeBase) new PdfTypeNull(handle);
      case IndirectObjectTypes.Reference:
        return (PdfTypeBase) new PdfTypeIndirect(handle);
      default:
        return (PdfTypeBase) new PdfTypeUnknown(handle);
    }
  }

  /// <summary>
  /// Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
  /// </summary>
  ~PdfTypeBase() => this.Dispose(false);

  /// <summary>Releases all resources used by the object.</summary>
  public void Dispose() => this.Dispose(true);

  /// <summary>Releases all resources used by the object.</summary>
  /// <param name="disposing">true for SuppressFinalize</param>
  protected virtual void Dispose(bool disposing)
  {
    if (this.IsDisposed || !PdfCommon.IsInitialize || this._handle == IntPtr.Zero || Pdfium.FPDFOBJ_GetParentObj(this._handle) != IntPtr.Zero)
      return;
    Pdfium.FPDFOBJ_Release(this._handle);
    if (!disposing)
      return;
    GC.SuppressFinalize((object) this);
  }

  /// <summary>Create a new PDF object based on this object.</summary>
  /// <param name="bDirect">A flag indicating whether indirect objects should be cloned as direct objects.</param>
  /// <returns>A cloned PDF object.</returns>
  /// <remarks>
  /// <para>bDirect should be <strong>False</strong> to keep indirect objects as is;
  /// <strong>True</strong> to convert indirect objects to direct objects.</para>
  /// </remarks>
  public PdfTypeBase Clone(bool bDirect = false)
  {
    IntPtr handle = Pdfium.FPDFOBJ_Clone(this.Handle, bDirect);
    return handle == IntPtr.Zero ? (PdfTypeBase) null : PdfTypeBase.Create(handle);
  }

  /// <summary>Delete object.</summary>
  [Obsolete("This method is obsolete. Please use Dispose() instead.", false)]
  public void Release() => this.Dispose();

  /// <summary>Converts the current instance to specified type.</summary>
  /// <typeparam name="T">The type the instance should be converted for.</typeparam>
  /// <param name="throwException">If false, then do not throw an exception if the type cannot be converted.</param>
  /// <returns>Returns instance of type of T or throw an exception <see cref="T:Patagames.Pdf.Net.Exceptions.UnexpectedTypeException" /> if type of this istance is not T.</returns>
  /// <remarks>This instance may be a direct or inderect object of type T</remarks>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnexpectedTypeException">Throws if the type connot be converted.</exception>
  public T As<T>(bool throwException = true) where T : PdfTypeBase
  {
    if (this is T)
      return this as T;
    if (this is PdfTypeIndirect)
    {
      PdfTypeBase direct = (this as PdfTypeIndirect).Direct;
      if (direct is T)
        return direct as T;
    }
    if (throwException)
      throw new UnexpectedTypeException(typeof (T), this.GetType());
    return default (T);
  }

  /// <summary>Checks if this instance has the specified type.</summary>
  /// <typeparam name="T">The type for check.</typeparam>
  /// <returns>Returns true if this instance has type T; false otherwise.</returns>
  public bool Is<T>() where T : PdfTypeBase
  {
    switch (this)
    {
      case T _:
        return true;
      case PdfTypeIndirect _:
        if ((this as PdfTypeIndirect).Direct is T)
          return true;
        break;
    }
    return false;
  }

  internal PdfTypeArray AsArrayOf<T>() where T : PdfTypeBase
  {
    if (this is PdfTypeArray)
      return this as PdfTypeArray;
    if (this is PdfTypeIndirect)
    {
      PdfTypeBase direct = (this as PdfTypeIndirect).Direct;
      if (direct is PdfTypeArray)
        return direct as PdfTypeArray;
    }
    if (!(this is T))
      throw new UnexpectedTypeException(typeof (T), this.GetType());
    PdfTypeArray pdfTypeArray = PdfTypeArray.Create();
    if (typeof (T) == typeof (PdfTypeName))
    {
      pdfTypeArray.Add(this.Clone());
    }
    else
    {
      if (!(typeof (T) == typeof (PdfTypeDictionary)))
        throw new NotImplementedException($"Type of {typeof (T).Name} not implemented");
      pdfTypeArray.Add(this.Clone());
    }
    return pdfTypeArray;
  }

  private void SubscribeOnDestroy()
  {
    Pdfium.FPDFOBJ_SetDestroyCallback(this.Handle, PdfTypeBase._onDestroyCallback);
    List<PdfTypeBase> pdfTypeBaseList;
    if (PdfTypeBase._mgrCallbacks.ContainsKey(this._handle))
    {
      pdfTypeBaseList = PdfTypeBase._mgrCallbacks[this._handle];
    }
    else
    {
      pdfTypeBaseList = new List<PdfTypeBase>();
      lock (PdfTypeBase._lockMgr)
        PdfTypeBase._mgrCallbacks.Add(this._handle, pdfTypeBaseList);
    }
    pdfTypeBaseList.Add(this);
  }

  [MonoPInvokeCallback(typeof (Pdfium.InternalObjectDestroyCallback))]
  private static void OnDestroyCalback(IntPtr handle)
  {
    if (!PdfTypeBase._mgrCallbacks.ContainsKey(handle))
      return;
    List<PdfTypeBase> mgrCallback = PdfTypeBase._mgrCallbacks[handle];
    for (int index = mgrCallback.Count - 1; index >= 0; --index)
    {
      mgrCallback[index].OnDestroy();
      mgrCallback[index].IsDisposed = true;
      mgrCallback[index]._handle = IntPtr.Zero;
    }
    mgrCallback.Clear();
    lock (PdfTypeBase._lockMgr)
      PdfTypeBase._mgrCallbacks.Remove(handle);
  }

  /// <summary>Called immediately before the object is destroyed.</summary>
  protected virtual void OnDestroy()
  {
  }
}
