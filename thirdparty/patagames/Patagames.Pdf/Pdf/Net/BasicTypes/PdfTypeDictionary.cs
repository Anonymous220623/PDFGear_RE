// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.BasicTypes.PdfTypeDictionary
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

/// <summary>Represents the Dictionary type of objects</summary>
/// <remarks>
/// A dictionary object is an associative table containing pairs of objects, known as the dictionary’s entries.
/// The first element of each entry is the key and the second element is the value.
/// The key must be a name. The value can be any kind of object, including another dictionary.
/// A dictionary entry whose value is null is equivalent to an absent entry.
/// (This differs from PostScript, where null behaves like any other object as the value of a dictionary entry.)
/// The number of entries in a dictionary is subject to an implementation limit;
/// <note type="note">No two entries in the same dictionary should have the same key. If a key does appear more than once, its value is undefined.</note>
/// Dictionary objects are the main building blocks of a PDF document.
/// They are commonly used to collect and tie together the attributes of a complex object,
/// such as a font or a page of the document, with each entry in the dictionary specifying the name
/// and value of an attribute. By convention, the Type entry of such a dictionary identifies the type
/// of object the dictionary describes.
/// In some cases, a Subtype entry (sometimes abbreviated S) is used to further identify a specialized
/// subcategory of the general type. The value of the Type or Subtype entry is always a name.
/// For example, in a font dictionary, the value of the Type entry is always Font, whereas that of the Subtype entry
/// may be Type1, TrueType, or one of several other values.
/// <para>
/// The value of the Type entry can almost always be inferred from context.
/// The operand of the Tf operator, for example, must be a font object;
/// therefore, the Typeentry in a font dictionary serves primarily as documentation and as information for error checking.
/// The Type entry is not required unless so stated in its description;
/// however, if the entry is present, it must have the correct value.
/// In addition, the value of the Type entry in any dictionary, even in private data,
/// must be either a name defined in this book or a registered name.
/// </para>
/// </remarks>
/// <summary>
/// Construct new instance of PdfTypeDictionary class from given Handle
/// </summary>
/// <param name="Handle">A handle to the unmanaged Dictionary object</param>
public class PdfTypeDictionary(IntPtr Handle) : 
  PdfTypeBase(Handle),
  IDictionary<string, PdfTypeBase>,
  ICollection<KeyValuePair<string, PdfTypeBase>>,
  IEnumerable<KeyValuePair<string, PdfTypeBase>>,
  IEnumerable
{
  private MgrIntObj _manager = new MgrIntObj();

  /// <summary>
  /// Determines whether the Dictionary is a signature field.
  /// </summary>
  public bool IsSignature => Pdfium.FPDFDICT_IsSignatureDict(this.Handle);

  /// <summary>Creates new Dictionary object</summary>
  /// <returns>The instance of a newly created object</returns>
  public static PdfTypeDictionary Create()
  {
    IntPtr Handle = Pdfium.FPDFDICT_Create();
    return !(Handle == IntPtr.Zero) ? new PdfTypeDictionary(Handle) : throw Pdfium.ProcessLastError();
  }

  /// <summary>Creates new instance of PdfTypeDictionary class</summary>
  /// <param name="handle">A handle to the unmanaged Dictionary object</param>
  /// <returns>An instance of PdfTypeDictionary</returns>
  public static PdfTypeDictionary Create(IntPtr handle)
  {
    return !(handle == IntPtr.Zero) ? new PdfTypeDictionary(handle) : throw new ArgumentException();
  }

  /// <summary>Gets the element with the specified key.</summary>
  /// <param name="key">The key of the element to get.</param>
  /// <returns>The element with the specified key.</returns>
  /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and key is not found.</exception>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public PdfTypeBase GetBy(string key) => this[key];

  /// <summary>Sets the element at the specified key.</summary>
  /// <param name="key">The key of the element to set.</param>
  /// <param name="value">The element which should be setted at the specified key.</param>
  /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and key is not found.</exception>
  /// <exception cref="T:System.ArgumentNullException">The key or value is null.</exception>
  public void SetAt(string key, PdfTypeBase value) => this[key] = value;

  /// <summary>
  /// Gets the element with the specified key and returns it as a matrix.
  /// </summary>
  /// <param name="key">The key of the element to get.</param>
  /// <returns>The matrix located at the specified key; null if there is no matrix.</returns>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public FS_MATRIX GetMatrixBy(string key)
  {
    return key != null ? Pdfium.FPDFDICT_GetMatrixBy(this.Handle, key) : throw new ArgumentNullException(nameof (key));
  }

  /// <summary>Sets the matrix at the specified key.</summary>
  /// <param name="key">The key of the element to set.</param>
  /// <param name="matrix">The matrix which should be setted at the specified key.</param>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public void SetMatrixAt(string key, FS_MATRIX matrix)
  {
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    Pdfium.FPDFDICT_SetAtMatrix(this.Handle, key, matrix);
  }

  /// <summary>
  /// Gets the element with the specified key and returns it as a rectangle.
  /// </summary>
  /// <param name="key">The key of the element to get.</param>
  /// <returns>The rectangle located at the specified key.</returns>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public FS_RECTF GetRectBy(string key)
  {
    return key != null ? Pdfium.FPDFDICT_GetRectBy(this.Handle, key) : throw new ArgumentNullException(nameof (key));
  }

  /// <summary>Sets the rectangle at the specified key.</summary>
  /// <param name="key">The key of the element to set.</param>
  /// <param name="rect">The rectangle which should be setted at the specified key.</param>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public void SetRectAt(string key, FS_RECTF rect)
  {
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    Pdfium.FPDFDICT_SetAtRect(this.Handle, key, rect);
  }

  /// <summary>
  /// Gets the element with the specified key and returns it as a Real number.
  /// </summary>
  /// <param name="key">The key of the element to get.</param>
  /// <returns>The real number located at the specified key.</returns>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public float GetRealBy(string key)
  {
    return key != null ? Pdfium.FPDFDICT_GetNumberBy(this.Handle, key) : throw new ArgumentNullException(nameof (key));
  }

  /// <summary>Sets the Real number at the specified key.</summary>
  /// <param name="key">The key of the element to set.</param>
  /// <param name="value">The real  number which should be setted at the specified key.</param>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public void SetRealAt(string key, float value)
  {
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    Pdfium.FPDFDICT_SetAtNumber(this.Handle, key, value);
  }

  /// <summary>
  /// Gets the element with the specified key and returns it as an integer value.
  /// </summary>
  /// <param name="key">The key of the element to get.</param>
  /// <returns>The integer value located at the specified key.</returns>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public int GetIntegerBy(string key)
  {
    return key != null ? Pdfium.FPDFDICT_GetIntegerBy(this.Handle, key) : throw new ArgumentNullException(nameof (key));
  }

  /// <summary>Sets the integer value at the specified key.</summary>
  /// <param name="key">The key of the element to set.</param>
  /// <param name="value">The integer value which should be setted at the specified key.</param>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public void SetIntegerAt(string key, int value)
  {
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    Pdfium.FPDFDICT_SetAtInteger(this.Handle, key, value);
  }

  /// <summary>
  /// Gets the element with the specified key and returns it as a boolean value.
  /// </summary>
  /// <param name="key">The key of the element to get</param>
  /// <returns>The boolean value located at the specified key.</returns>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public bool GetBooleanBy(string key)
  {
    return key != null ? Pdfium.FPDFDICT_GetBooleanBy(this.Handle, key) : throw new ArgumentNullException(nameof (key));
  }

  /// <summary>Sets the boolean value at the specified key.</summary>
  /// <param name="key">The key of the element to set.</param>
  /// <param name="value">The boolean value which should be setted at the specified key.</param>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public void SetBooleanAt(string key, bool value)
  {
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    Pdfium.FPDFDICT_SetAtBoolean(this.Handle, key, value);
  }

  /// <summary>
  /// Gets the element with the specified key and returns it as a ansi string.
  /// </summary>
  /// <param name="key">The key of the element to get.</param>
  /// <returns>The string located at the specified key.</returns>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public string GetStringBy(string key)
  {
    return key != null ? Pdfium.FPDFDICT_GetStringBy(this.Handle, key) : throw new ArgumentNullException(nameof (key));
  }

  /// <summary>
  /// Gets the element with the specified key and returns it as an unicode string.
  /// </summary>
  /// <param name="key">The key of the element to get.</param>
  /// <returns>The string located at the specified key.</returns>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public string GetUnicodeBy(string key)
  {
    return key != null ? Pdfium.FPDFDICT_GetUnicodeTextBy(this.Handle, key) : throw new ArgumentNullException(nameof (key));
  }

  /// <summary>Sets the ansi string at the specified key.</summary>
  /// <param name="key">The key of the element to set.</param>
  /// <param name="text">The string which should be setted at the specified key.</param>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public void SetStringAt(string key, string text)
  {
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    Pdfium.FPDFDICT_SetAtString(this.Handle, key, text);
  }

  /// <summary>
  /// Creates a Name object and sets it at the specified key.
  /// </summary>
  /// <param name="key">The key of the element to set.</param>
  /// <param name="name">The name which should be setted at the specified key.</param>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public void SetNameAt(string key, string name)
  {
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    Pdfium.FPDFDICT_SetAtName(this.Handle, key, name);
  }

  /// <summary>
  /// Creates an Indirect object and sets it at the specified key of dictionary.
  /// </summary>
  /// <param name="key">The key contained in the Dictionary.</param>
  /// <param name="list">List of objects</param>
  /// <param name="objectNumber">Object's number</param>
  /// <exception cref="T:System.ArgumentNullException">The key or list is null.</exception>
  public void SetIndirectAt(string key, PdfIndirectList list, int objectNumber)
  {
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    Pdfium.FPDFDICT_SetAtReference(this.Handle, key, list.Handle, objectNumber);
  }

  /// <summary>
  /// Creates an Indirect object and sets it at the specified key of dictionary.
  /// </summary>
  /// <param name="key">The key contained in the Dictionary.</param>
  /// <param name="list">List of objects</param>
  /// <param name="directObject">Handle to PDF object</param>
  /// <exception cref="T:System.ArgumentNullException">The key, list or object is null.</exception>
  public void SetIndirectAt(string key, PdfIndirectList list, PdfTypeBase directObject)
  {
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    if (list == null)
      throw new ArgumentNullException(nameof (list));
    if (directObject == null)
      throw new ArgumentNullException(nameof (directObject));
    Pdfium.FPDFDICT_SetAtReferenceEx(this.Handle, key, list.Handle, directObject.Handle);
  }

  /// <summary>Gets the number of keys contained in the Dictionary.</summary>
  public int Count => Pdfium.FPDFDICT_GetCount(this.Handle);

  /// <summary>Gets or sets the element with the specified key</summary>
  /// <param name="key">The key of the element to get or set.</param>
  /// <returns>The element with the specified key.</returns>
  /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and key is not found.</exception>
  /// <exception cref="T:System.ArgumentNullException">The key or value is null.</exception>
  public PdfTypeBase this[string key]
  {
    get
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      if (!this.ContainsKey(key))
        throw new KeyNotFoundException();
      return this._manager.Create(Pdfium.FPDFDICT_GetObjectBy(this.Handle, key));
    }
    set
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      if (value == null)
        throw new ArgumentNullException(nameof (value));
      Pdfium.FPDFDICT_SetAt(this.Handle, key, value != null ? value.Handle : IntPtr.Zero);
      this._manager.Add(value);
    }
  }

  /// <summary>
  /// Gets a value indicating whether the Dictionary object is read-only.
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>
  /// Gets an ICollection object containing the keys of the IDictionary object.
  /// </summary>
  public ICollection<string> Keys
  {
    get
    {
      List<string> ret = new List<string>();
      Pdfium.FPDFDICT_Enum(this.Handle, (Pdfium.DictEnumCallback) ((handle, Key, value) => ret.Add(Key)));
      return (ICollection<string>) ret;
    }
  }

  /// <summary>
  /// Gets an ICollection object containing the values in the IDictionary object.
  /// </summary>
  public ICollection<PdfTypeBase> Values
  {
    get
    {
      List<PdfTypeBase> ret = new List<PdfTypeBase>();
      Pdfium.FPDFDICT_Enum(this.Handle, (Pdfium.DictEnumCallback) ((handle, Key, value) => ret.Add(this._manager.Create(value))));
      return (ICollection<PdfTypeBase>) ret;
    }
  }

  /// <summary>
  /// Adds an element with the provided key and value to the IDictionary object.
  /// </summary>
  /// <param name="item">The item to be added.</param>
  /// <exception cref="T:System.ArgumentNullException">The key or value is null.</exception>
  public void Add(KeyValuePair<string, PdfTypeBase> item) => this.Add(item.Key, item.Value);

  /// <summary>
  /// Adds an element with the provided key and value to the IDictionary object.
  /// </summary>
  /// <param name="key">The string to use as the key of the element to add.</param>
  /// <param name="value">The <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeBase" /> to use as the value of the element to add.</param>
  /// <exception cref="T:System.ArgumentNullException">The key or value is null.</exception>
  /// <exception cref="T:System.ArgumentException">An element with the same key already exists in the Dictionary.</exception>
  public void Add(string key, PdfTypeBase value)
  {
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    if (this.ContainsKey(key))
      throw new ArgumentException(string.Format(Error.err0030, (object) key), nameof (key));
    this[key] = value;
  }

  /// <summary>Removes all elements from the IDictionary object.</summary>
  public void Clear()
  {
    foreach (string key in (IEnumerable<string>) this.Keys)
      Pdfium.FPDFDICT_RemoveAt(this.Handle, key);
    this._manager.Clear();
  }

  /// <summary>
  /// Determines whether the Dictionary contains an item with the specified key.
  /// </summary>
  /// <param name="item">The object to locate in the Dictionary.</param>
  /// <returns>true if the Dictionary contains an item with the key; otherwise, false.</returns>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public bool Contains(KeyValuePair<string, PdfTypeBase> item) => this.ContainsKey(item.Key);

  /// <summary>
  /// Determines whether the Dictionary contains an element with the specified key.
  /// </summary>
  /// <param name="key">The key to locate in the Dictionary.</param>
  /// <returns>true if the Dictionary contains an element with the key; otherwise, false.</returns>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public bool ContainsKey(string key)
  {
    return key != null ? Pdfium.FPDFDICT_KeyExist(this.Handle, key) : throw new ArgumentNullException(nameof (key));
  }

  /// <summary>
  /// Removes the element with the specified key from the Dictionary.
  /// </summary>
  /// <param name="item">The key of the element to remove.</param>
  /// <returns>Always true</returns>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public bool Remove(KeyValuePair<string, PdfTypeBase> item) => this.Remove(item.Key);

  /// <summary>
  /// Removes the element with the specified key from the Dictionary.
  /// </summary>
  /// <param name="key">The key of the element to remove.</param>
  /// <returns>Always true</returns>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public bool Remove(string key)
  {
    PdfTypeBase pdfTypeBase = key != null ? this[key] : throw new ArgumentNullException(nameof (key));
    if (pdfTypeBase != null)
      this._manager.Remove(pdfTypeBase.Handle);
    Pdfium.FPDFDICT_RemoveAt(this.Handle, key);
    return true;
  }

  /// <summary>Gets the value associated with the specified key.</summary>
  /// <param name="key">The key whose value to get.</param>
  /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
  /// <returns>true if the object that implements Dictionary contains an element with the specified key; otherwise, false.</returns>
  /// <exception cref="T:System.ArgumentNullException">The key is null.</exception>
  public bool TryGetValue(string key, out PdfTypeBase value)
  {
    if (key == null)
      throw new ArgumentNullException(nameof (key));
    value = (PdfTypeBase) null;
    if (!this.ContainsKey(key))
      return false;
    value = this[key];
    return true;
  }

  /// <summary>
  /// Copies the elements of the Dictionary to an Array, starting at a particular Array index
  /// </summary>
  /// <param name="array">The one-dimensional Array that is the destination of the elements copied from Dictionary. The Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  /// <exception cref="T:System.ArgumentNullException">The array is null.</exception>
  /// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
  public void CopyTo(KeyValuePair<string, PdfTypeBase>[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (arrayIndex));
    foreach (KeyValuePair<string, PdfTypeBase> keyValuePair in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = keyValuePair;
    }
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>An enumerator that can be used to iterate through the collection.</returns>
  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>An enumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<KeyValuePair<string, PdfTypeBase>> GetEnumerator()
  {
    return (IEnumerator<KeyValuePair<string, PdfTypeBase>>) new PdfDictionaryEnumerator(this);
  }
}
