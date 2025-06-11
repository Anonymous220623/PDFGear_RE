// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.AcroForms.ExportValuesCollection
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.AcroForms;

/// <summary>
/// Represents a collection which holds an array of text strings representing the export value of each <see cref="T:Patagames.Pdf.Net.PdfControl" /> in the field.
/// </summary>
public class ExportValuesCollection : 
  IList<string>,
  ICollection<string>,
  IEnumerable<string>,
  IEnumerable
{
  private PdfField _field;

  private PdfTypeArray _array
  {
    get
    {
      PdfTypeBase fieldAttribute = this._field.GetFieldAttribute("Opt");
      return fieldAttribute != null && fieldAttribute.Is<PdfTypeArray>() ? fieldAttribute.As<PdfTypeArray>() : (PdfTypeArray) null;
    }
  }

  /// <summary>
  /// Initialize new instance of <see cref="T:Patagames.Pdf.Net.AcroForms.ExportValuesCollection" /> class.
  /// </summary>
  /// <param name="field">The <see cref="T:Patagames.Pdf.Net.PdfField" />.</param>
  public ExportValuesCollection(PdfField field) => this._field = field;

  private void RemoveFromField()
  {
    if (this._array.Count != 0 || !this._field.Dictionary.ContainsKey("Opt"))
      return;
    this._field.Dictionary.Remove("Opt");
  }

  private void AddToField()
  {
    if (this._array != null)
      return;
    this._field.Dictionary["Opt"] = (PdfTypeBase) PdfTypeArray.Create();
  }

  /// <summary>
  /// Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire System.Collections.Generic.List`1.
  /// </summary>
  /// <param name="match">The System.Predicate`1 delegate that defines the conditions of the element to search for.</param>
  /// <returns>The first element that matches the conditions defined by the specified predicate, if found; otherwise, null.</returns>
  /// <exception cref="T:System.ArgumentNullException">match is null.</exception>
  public string Find(Predicate<string> match)
  {
    if (match == null)
      throw new ArgumentNullException(nameof (match));
    if (this._array == null)
      return (string) null;
    foreach (string str in this)
    {
      if (match(str))
        return str;
    }
    return (string) null;
  }

  /// <summary>Gets or sets the element at the specified index.</summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The element at the specified index.</returns>
  /// <exception cref="T:System.IndexOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1" />.</exception>
  public string this[int index]
  {
    get
    {
      if (index < 0 || index >= this.Count)
        throw new ArgumentOutOfRangeException();
      return this._array[index].As<PdfTypeString>().UnicodeString;
    }
    set
    {
      if (index < 0 || index >= this.Count)
        throw new ArgumentOutOfRangeException();
      this._array[index] = (PdfTypeBase) PdfTypeString.Create(value);
    }
  }

  /// <summary>
  ///  Gets the number of elements contained in the <see cref="T:System.Collections.Generic.IList`1" />.
  /// </summary>
  public int Count => this._array != null ? this._array.Count : 0;

  /// <summary>
  ///  Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
  /// </summary>
  /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
  /// <returns>The index of item if found in the list; otherwise, -1.</returns>
  public int IndexOf(string item)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index] == item)
        return index;
    }
    return -1;
  }

  /// <summary>
  /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.IList`1" /> is read-only.
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>
  /// Determines whether the <see cref="T:System.Collections.Generic.IList`1" /> contains a specific value.
  /// </summary>
  /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
  /// <returns>rue if item is found in the <see cref="T:System.Collections.Generic.IList`1" />; otherwise, false.</returns>
  public bool Contains(string item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Copies the elements of the <see cref="T:System.Collections.Generic.IList`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
  /// </summary>
  /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.IList`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  /// <exception cref="T:System.ArgumentNullException"><paramref name="array" /> is null.</exception>
  /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex" /> less than 0.</exception>
  /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.IList`1" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.</exception>
  public void CopyTo(string[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException();
    foreach (string str in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = str;
    }
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<string> GetEnumerator()
  {
    return (IEnumerator<string>) new CollectionEnumerator<string>((IList<string>) this);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  /// <summary>
  ///  Inserts an item to the <see cref="T:System.Collections.Generic.IList`1" /> at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index at which item should be inserted.</param>
  /// <param name="item">  The object to insert into the <see cref="T:System.Collections.Generic.IList`1" />.</param>
  public void Insert(int index, string item)
  {
    if (index < 0 || index > this.Count)
      throw new ArgumentOutOfRangeException();
    this.AddToField();
    this._array.Insert(index, (PdfTypeBase) PdfTypeString.Create(item, true));
  }

  /// <summary>
  /// Removes the <see cref="T:System.Collections.Generic.IList`1" /> item at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index of the item to remove.</param>
  public void RemoveAt(int index)
  {
    if (index < 0 || index >= this.Count)
      throw new ArgumentOutOfRangeException();
    this._array.RemoveAt(index);
    this.RemoveFromField();
  }

  /// <summary>
  /// Adds an item to the <see cref="T:System.Collections.Generic.IList`1" />.
  /// </summary>
  /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.IList`1" />.</param>
  public void Add(string item)
  {
    this.AddToField();
    this._array.Add((PdfTypeBase) PdfTypeString.Create(item, true));
  }

  /// <summary>
  /// Removes all items from the <see cref="T:System.Collections.Generic.IList`1" />.
  /// </summary>
  public void Clear()
  {
    if (this._array != null)
      this._array.Clear();
    this.RemoveFromField();
  }

  /// <summary>
  /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.IList`1" />.
  /// </summary>
  /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.IList`1" />.</param>
  /// <returns>
  /// true if item was successfully removed from the <see cref="T:System.Collections.Generic.IList`1" />;
  /// otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.IList`1" />.</returns>
  public bool Remove(string item)
  {
    int index = this.IndexOf(item);
    if (index < 0)
      return false;
    this.RemoveAt(index);
    return true;
  }
}
