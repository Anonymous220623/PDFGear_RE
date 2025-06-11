// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.BasicTypes.PdfTypeArray
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Properties;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.BasicTypes;

/// <summary>Represents the Array type of objects</summary>
/// <remarks>
/// An array object is a one-dimensional collection of objects arranged sequentially.
/// Unlike arrays in many other computer languages, PDF arrays may be heterogeneous;
/// that is, an array’s elements may be any combination of numbers, strings, dictionaries,
/// or any other objects, including other arrays.
/// The number of elements in an array is subject to an implementation limit;
/// </remarks>
/// <summary>
/// Construct new instance of PdfTypeArray class from given Handle
/// </summary>
/// <param name="Handle">A handle to the unmanaged Array object.</param>
public class PdfTypeArray(IntPtr Handle) : 
  PdfTypeBase(Handle),
  IList<PdfTypeBase>,
  ICollection<PdfTypeBase>,
  IEnumerable<PdfTypeBase>,
  IEnumerable
{
  private MgrIntObj _manager = new MgrIntObj();

  /// <summary>Creates new Array object</summary>
  /// <returns>The instance of a newly created object</returns>
  public static PdfTypeArray Create()
  {
    IntPtr Handle = Pdfium.FPDFARRAY_Create();
    return !(Handle == IntPtr.Zero) ? new PdfTypeArray(Handle) : throw Pdfium.ProcessLastError();
  }

  /// <summary>Creates new instance of PdfTypeArray class</summary>
  /// <param name="handle">A handle to the unmanaged Array object.</param>
  /// <returns>An instance of PdfTypeArray</returns>
  public static PdfTypeArray Create(IntPtr handle)
  {
    return !(handle == IntPtr.Zero) ? new PdfTypeArray(handle) : throw new ArgumentException();
  }

  /// <summary>Gets the element at the specified index</summary>
  /// <param name="index">The zero-based index of the element to get or set.</param>
  /// <returns>The element at the specified index.</returns>
  /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the Array</exception>
  public PdfTypeBase GetAt(int index)
  {
    if (index < 0 || index > this.Count - 1)
      throw new ArgumentOutOfRangeException();
    return this._manager.Create(Pdfium.FPDFARRAY_GetObjectAt(this.Handle, index));
  }

  /// <summary>
  /// Gets the element with the specified index and returns it as a matrix.
  /// </summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The matrix located at the specified index; null if there is no matrix.</returns>
  public FS_MATRIX GetMatrixAt(int index)
  {
    IntPtr arrayAt = Pdfium.FPDFARRAY_GetArrayAt(this.Handle, index);
    return arrayAt == IntPtr.Zero ? (FS_MATRIX) null : Pdfium.FPDFARRAY_GetMatrix(arrayAt);
  }

  /// <summary>
  /// Gets the element with the specified index and returns it as a rectangle.
  /// </summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The rectangle located at the specified index.</returns>
  public FS_RECTF GetRectAt(int index)
  {
    IntPtr arrayAt = Pdfium.FPDFARRAY_GetArrayAt(this.Handle, index);
    return arrayAt == IntPtr.Zero ? new FS_RECTF() : Pdfium.FPDFARRAY_GetRect(arrayAt);
  }

  /// <summary>
  /// Gets the element with the specified index and returns it as a Real number.
  /// </summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The real number located at the specified index.</returns>
  public float GetRealAt(int index) => Pdfium.FPDFARRAY_GetNumberAt(this.Handle, index);

  /// <summary>
  /// Gets the element with the specified index and returns it as an integer value.
  /// </summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The integer value located at the specified index.</returns>
  public int GetIntegerAt(int index) => Pdfium.FPDFARRAY_GetIntegerAt(this.Handle, index);

  /// <summary>
  /// Gets the element with the specified index and returns it as a boolean value.
  /// </summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The boolean value located at the specified index.</returns>
  public bool GetBooleanAt(int index) => Pdfium.FPDFARRAY_GetIntegerAt(this.Handle, index) == 1;

  /// <summary>
  /// Gets the element with the specified index and returns it as a ansi string.
  /// </summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The string located at the specified index.</returns>
  public string GetStringAt(int index) => Pdfium.FPDFARRAY_GetStringAt(this.Handle, index);

  /// <summary>Adds a real value to the Array.</summary>
  /// <param name="value">The real number which should be added at the end of Array.</param>
  public void AddReal(float value) => Pdfium.FPDFARRAY_AddNumber(this.Handle, value);

  /// <summary>Adds the integer value to the Array.</summary>
  /// <param name="value">The integer number which should be added at the end of Array.</param>
  public void AddInteger(int value) => Pdfium.FPDFARRAY_AddInteger(this.Handle, value);

  /// <summary>Adds the ansi string to the Array.</summary>
  /// <param name="text">The string which should be added at the end of Array.</param>
  public void AddString(string text) => Pdfium.FPDFARRAY_AddString(this.Handle, text);

  /// <summary>Creates a Name object and adds it to the Array.</summary>
  /// <param name="name">The name which should be added at the end of Array.</param>
  public void AddName(string name) => Pdfium.FPDFARRAY_AddName(this.Handle, name);

  /// <summary>Creates an Indirect object and adds it to the Array.</summary>
  /// <param name="list">List of objects</param>
  /// <param name="objectNumber">Object's number</param>
  /// <exception cref="T:System.ArgumentNullException">list is null.</exception>
  public void AddIndirect(PdfIndirectList list, int objectNumber)
  {
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    Pdfium.FPDFARRAY_AddReference(this.Handle, list.Handle, objectNumber);
  }

  /// <summary>Creates an Indirect object and adds it to the Array.</summary>
  /// <param name="list">List of objects</param>
  /// <param name="directObject">A PDF object</param>
  /// <exception cref="T:System.ArgumentNullException">list or object is null.</exception>
  public void AddIndirect(PdfIndirectList list, PdfTypeBase directObject)
  {
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    if (directObject == null)
      throw new ArgumentNullException(nameof (directObject));
    Pdfium.FPDFARRAY_AddReferenceEx(this.Handle, list.Handle, directObject.Handle);
  }

  /// <summary>Sets the element at the specified index</summary>
  /// <param name="index">The zero-based index of the element to get or set.</param>
  /// <param name="value">The element which should be setted at the specified index.</param>
  /// <returns>The element at the specified index.</returns>
  /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the Array</exception>
  /// <exception cref="T:System.ArgumentNullException">value is null.</exception>
  public void SetAt(int index, PdfTypeBase value)
  {
    if (value == null)
      throw new ArgumentNullException();
    if (index < 0 || index > this.Count - 1)
      throw new ArgumentOutOfRangeException();
    if (value.ObjectNumber > 0)
      throw new NotSupportedException(Error.err0033);
    Pdfium.FPDFARRAY_SetAt(this.Handle, index, value.Handle, IntPtr.Zero);
    this._manager.Add(value);
  }

  /// <summary>Sets the element at the specified index</summary>
  /// <param name="index">The zero-based index of the element to get or set.</param>
  /// <param name="value">The element which should be setted at the specified index.</param>
  /// <param name="list">The list of objects.</param>
  /// <returns>The element at the specified index.</returns>
  /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the Array</exception>
  /// <exception cref="T:System.ArgumentNullException">value or list is null.</exception>
  public void SetAt(int index, PdfTypeBase value, PdfIndirectList list)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    if (index < 0 || index > this.Count - 1)
      throw new ArgumentOutOfRangeException();
    Pdfium.FPDFARRAY_SetAt(this.Handle, index, value.Handle, list.Handle);
    this._manager.Add(value);
  }

  /// <summary>Adds an item to the Array</summary>
  /// <param name="item">The object to add to the Array</param>
  /// <param name="list">The list of objects.</param>
  /// <exception cref="T:System.ArgumentNullException">item or list is null.</exception>
  public void Add(PdfTypeBase item, PdfIndirectList list)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    Pdfium.FPDFARRAY_Add(this.Handle, item.Handle, list.Handle);
    this._manager.Add(item);
  }

  /// <summary>Inserts an item to the Array at the specified index.</summary>
  /// <param name="index">The zero-based index at which item should be inserted.</param>
  /// <param name="item">The object to insert into the Array.</param>
  /// <param name="list">The list of objects.</param>
  /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the Array.</exception>
  /// <exception cref="T:System.ArgumentNullException">item or list is null.</exception>
  public void Insert(int index, PdfTypeBase item, PdfIndirectList list)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    if (index < 0 || index > this.Count)
      throw new ArgumentOutOfRangeException();
    Pdfium.FPDFARRAY_InsertAt(this.Handle, index, item.Handle, list.Handle);
    this._manager.Add(item);
  }

  /// <summary>Gets the number of elements contained in the Array.</summary>
  public int Count => Pdfium.FPDFARRAY_GetCount(this.Handle);

  /// <summary>Gets or sets the element at the specified index</summary>
  /// <param name="index">The zero-based index of the element to get or set.</param>
  /// <returns>The element at the specified index.</returns>
  /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the Array</exception>
  /// <exception cref="T:System.ArgumentNullException">value is null.</exception>
  public PdfTypeBase this[int index]
  {
    get => this.GetAt(index);
    set => this.SetAt(index, value);
  }

  /// <summary>
  /// Gets a value indicating whether the Array is read-only.
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>Determines the index of a specific item in the Array.</summary>
  /// <param name="item">The object to locate in the Array</param>
  /// <returns>The index of item if found in the list; otherwise, -1.</returns>
  /// <exception cref="T:System.ArgumentNullException">item is null.</exception>
  public int IndexOf(PdfTypeBase item)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    int num = 0;
    foreach (PdfTypeBase pdfTypeBase in this)
    {
      if (pdfTypeBase.Handle == item.Handle)
        return num;
      ++num;
    }
    return -1;
  }

  /// <summary>
  /// Removes the System.Collections.Generic.IList`1 item at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index of the item to remove.</param>
  /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the Array.</exception>
  public void RemoveAt(int index)
  {
    IntPtr handle = index >= 0 && index <= this.Count - 1 ? Pdfium.FPDFARRAY_GetObjectAt(this.Handle, index) : throw new ArgumentOutOfRangeException();
    if (!(handle != IntPtr.Zero))
      return;
    this._manager.Remove(handle);
    Pdfium.FPDFARRAY_RemoveAt(this.Handle, index);
  }

  /// <summary>Adds an item to the Array</summary>
  /// <param name="item">The object to add to the Array</param>
  /// <exception cref="T:System.ArgumentNullException">item is null.</exception>
  public void Add(PdfTypeBase item)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    if (item.ObjectNumber > 0)
      throw new NotSupportedException(Error.err0031);
    Pdfium.FPDFARRAY_Add(this.Handle, item.Handle, IntPtr.Zero);
    this._manager.Add(item);
  }

  /// <summary>Inserts an item to the Array at the specified index.</summary>
  /// <param name="index">The zero-based index at which item should be inserted.</param>
  /// <param name="item">The object to insert into the Array.</param>
  /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the Array.</exception>
  /// <exception cref="T:System.ArgumentNullException">item is null.</exception>
  public void Insert(int index, PdfTypeBase item)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    if (index < 0 || index > this.Count)
      throw new ArgumentOutOfRangeException();
    if (item.ObjectNumber > 0)
      throw new NotSupportedException(Error.err0032);
    Pdfium.FPDFARRAY_InsertAt(this.Handle, index, item.Handle, IntPtr.Zero);
    this._manager.Add(item);
  }

  /// <summary>Removes all items from the Array</summary>
  public void Clear()
  {
    Pdfium.FPDFARRAY_RemoveAt(this.Handle, 0, this.Count);
    this._manager.Clear();
  }

  /// <summary>
  /// Determines whether the Array contains a specific value.
  /// </summary>
  /// <param name="item">The object to locate in the Array.</param>
  /// <returns>true if item is found in the Array; otherwise, false.</returns>
  /// <exception cref="T:System.ArgumentNullException">item is null.</exception>
  public bool Contains(PdfTypeBase item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Copies the elements of the Array to an System.Array, starting at a particular System.Array index.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from Array. The System.Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  /// <exception cref="T:System.ArgumentNullException">array is null.</exception>
  /// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
  public void CopyTo(PdfTypeBase[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (arrayIndex));
    foreach (PdfTypeBase pdfTypeBase in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = pdfTypeBase;
    }
  }

  /// <summary>
  /// Removes the first occurrence of a specific object from the Array.
  /// </summary>
  /// <param name="item">The object to remove from the Array.</param>
  /// <returns>
  /// true if item was successfully removed from the Array;
  /// otherwise, false. This method also returns false if item is not found in the
  /// original Array.
  /// </returns>
  /// <exception cref="T:System.ArgumentNullException">item is null.</exception>
  public bool Remove(PdfTypeBase item)
  {
    int index = item != null ? this.IndexOf(item) : throw new ArgumentNullException(nameof (item));
    if (index < 0)
      return false;
    this.RemoveAt(index);
    return true;
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the Array.</returns>
  public IEnumerator<PdfTypeBase> GetEnumerator()
  {
    return (IEnumerator<PdfTypeBase>) new CollectionEnumerator<PdfTypeBase>((IList<PdfTypeBase>) this);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
}
