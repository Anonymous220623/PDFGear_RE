// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfAttachmentCollection
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Wrappers;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>Represents a collection of document attachments.</summary>
public class PdfAttachmentCollection : 
  ICollection<PdfAttachment>,
  IEnumerable<PdfAttachment>,
  IEnumerable
{
  private PdfDocument _doc;
  private ListObjectManager<IntPtr, PdfAttachment> _mgr = new ListObjectManager<IntPtr, PdfAttachment>();
  private PdfNameTreeCollection _nameTree;

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" /> class.
  /// </summary>
  /// <param name="document">Document which contains this collection of attachments.</param>
  public PdfAttachmentCollection(PdfDocument document)
  {
    this._doc = document;
    this._nameTree = new PdfNameTreeCollection(this._doc, "EmbeddedFiles");
  }

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.Net.PdfAttachment" /> by its name.
  /// </summary>
  /// <param name="name">The name of an attachment.</param>
  /// <returns>The value associated with the <paramref name="name" /> parameter in the <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" /> object, if name is found; otherwise, null.</returns>
  /// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> or value is null.</exception>
  /// <exception cref="T:System.ObjectDisposedException">The attachment has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnexpectedTypeException">The object with the given name is not an attachment.</exception>
  public PdfAttachment this[string name]
  {
    get
    {
      PdfTypeBase pdfTypeBase = this._nameTree[name];
      if (pdfTypeBase == null)
        return (PdfAttachment) null;
      PdfTypeDictionary pdfTypeDictionary = pdfTypeBase.As<PdfTypeDictionary>();
      IntPtr handle = pdfTypeDictionary.Handle;
      if (handle == IntPtr.Zero)
        return (PdfAttachment) null;
      if (this._mgr.Contains(handle))
        return this._mgr.Get(handle);
      PdfAttachment pdfAttachment = new PdfAttachment(name, new PdfFileSpecification(this._doc, (PdfTypeBase) pdfTypeDictionary.As<PdfTypeDictionary>()));
      this._mgr.Add(handle, pdfAttachment);
      return pdfAttachment;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException();
      IntPtr key = IntPtr.Zero;
      PdfTypeBase pdfTypeBase = this._nameTree[name];
      if (pdfTypeBase != null)
        key = pdfTypeBase.As<PdfTypeDictionary>().Handle;
      this._nameTree[name] = (PdfTypeBase) value.FileSpecification.Dictionary;
      IntPtr handle = value.FileSpecification.Dictionary.Handle;
      this._mgr.Add(handle, value);
      if (!(key != IntPtr.Zero) || !(handle != key))
        return;
      this._mgr.Remove(key);
    }
  }

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.Net.PdfAttachment" /> at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The value at the specified <paramref name="index" /> in the <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" /> object.</returns>
  /// <exception cref="T:System.IndexOutOfRangeException"><paramref name="index" /> is not a valid index in the collection.</exception>
  /// <exception cref="T:System.ArgumentNullException">The value or name of the attachment at specified index is null.</exception>
  /// <exception cref="T:System.ObjectDisposedException">The attachment has been disposed.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.UnexpectedTypeException">The object at the given index is not an attachment.</exception>
  public PdfAttachment this[int index]
  {
    get => this[this._nameTree.NameOf(index)];
    set => this[this._nameTree.NameOf(index)] = value;
  }

  /// <summary>
  /// Removes the <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" /> item at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index of the item to remove.</param>
  /// <exception cref="T:System.IndexOutOfRangeException"><paramref name="index" /> is not a valid index in the collection.</exception>
  public void RemoveAt(int index)
  {
    IntPtr key = IntPtr.Zero;
    PdfTypeBase pdfTypeBase = this._nameTree[index];
    if (pdfTypeBase != null)
      key = pdfTypeBase.As<PdfTypeDictionary>().Handle;
    this._nameTree.Remove(index);
    if (!(key != IntPtr.Zero))
      return;
    this._mgr.Remove(key);
  }

  /// <summary>
  /// Gets the number of elements contained in the <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" />.
  /// </summary>
  public int Count => this._nameTree.Count;

  /// <summary>
  /// Gets a value indicating whether the <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" /> is read-only.
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>
  /// Adds an item to the <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" />.
  /// </summary>
  /// <param name="item">The object to add to the <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" />.</param>
  /// <exception cref="T:System.ArgumentNullException">The <paramref name="item" /> is null.</exception>
  /// <exception cref="T:Patagames.Pdf.Net.Exceptions.PdfParserException">The <paramref name="item" /> could not be added due to a malformed document's name tree.</exception>
  /// <exception cref="T:System.ArgumentException">An element with the same name already exists in the <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" />.</exception>
  /// <exception cref="T:System.ObjectDisposedException">The attachment has been disposed.</exception>
  public void Add(PdfAttachment item)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    this._nameTree.Add(item.Name, (PdfTypeBase) item.FileSpecification.Dictionary);
    this._mgr.Add(item.FileSpecification.Dictionary.Handle, item);
  }

  /// <summary>
  ///  Removes all items from the <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" />.
  /// </summary>
  public void Clear()
  {
    this._nameTree.Clear();
    this._mgr.Clear();
  }

  /// <summary>
  /// Determines whether the <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" /> contains a specific value.
  /// </summary>
  /// <param name="item">The object to locate in the <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" />.</param>
  /// <returns>true if <paramref name="item" /> is found in the <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" />; otherwise, false.</returns>
  /// <exception cref="T:System.ArgumentNullException">The <paramref name="item" /> is null.</exception>
  public bool Contains(PdfAttachment item)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    return this._nameTree.Contains(item.Name);
  }

  /// <summary>
  /// Copies the elements of the <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
  /// </summary>
  /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
  /// <exception cref="T:System.ArgumentNullException"><paramref name="array" /> is null.</exception>
  /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex" /> is less than 0.</exception>
  public void CopyTo(PdfAttachment[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException();
    foreach (PdfAttachment pdfAttachment in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = pdfAttachment;
    }
  }

  /// <summary>
  /// Removes the <paramref name="item" /> from the <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" />.
  /// </summary>
  /// <param name="item">The object to remove from the <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" />.</param>
  /// <returns>true if item was successfully removed from the <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" />; otherwise, false.
  /// This method also returns false if item is not found in the original <see cref="T:Patagames.Pdf.Net.PdfAttachmentCollection" />.</returns>
  /// <exception cref="T:System.ArgumentNullException">The <paramref name="item" /> is null.</exception>
  public bool Remove(PdfAttachment item)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    int num = this._nameTree.Remove(item.Name) ? 1 : 0;
    if (num == 0)
      return num != 0;
    this._mgr.Remove(item.FileSpecification.Dictionary.Handle);
    return num != 0;
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
  public IEnumerator<PdfAttachment> GetEnumerator()
  {
    return (IEnumerator<PdfAttachment>) new CollectionEnumerator<PdfAttachment>(this);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
}
