// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.PdfFontCollection
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net;

/// <summary>
/// Represents the collection of fonts in an acroform resource dictionary.
/// </summary>
public class PdfFontCollection : IReadOnlyList<PdfFont>, IEnumerable<PdfFont>, IEnumerable
{
  private PdfInteractiveForms _forms;
  private ListObjectManager<IntPtr, PdfFont> _mgr = new ListObjectManager<IntPtr, PdfFont>();

  /// <summary>Initialize the collection.</summary>
  /// <param name="forms">Interactive forms.</param>
  public PdfFontCollection(PdfInteractiveForms forms) => this._forms = forms;

  /// <summary>
  /// Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire System.Collections.Generic.List`1.
  /// </summary>
  /// <param name="match">The System.Predicate`1 delegate that defines the conditions of the element to search for.</param>
  /// <returns>The first element that matches the conditions defined by the specified predicate, if found; otherwise, null.</returns>
  /// <exception cref="T:System.ArgumentNullException">match is null.</exception>
  public PdfFont Find(Predicate<PdfFont> match)
  {
    if (match == null)
      throw new ArgumentNullException(nameof (match));
    foreach (PdfFont pdfFont in this)
    {
      if (match(pdfFont))
        return pdfFont;
    }
    return (PdfFont) null;
  }

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.Net.PdfFont" /> at the specified index
  /// </summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.Net.PdfFont" /> at the specified index.</returns>
  public PdfFont this[int index]
  {
    get
    {
      if (index < 0 || index >= this.Count)
        throw new ArgumentOutOfRangeException();
      IntPtr formFont = Pdfium.FPDFInterForm_GetFormFont(this._forms.Handle, index, false, out string _);
      if (formFont == IntPtr.Zero)
        throw Pdfium.ProcessLastError();
      if (this._mgr.Contains(formFont))
        return this._mgr.Get(formFont);
      PdfFont fromHandle = PdfFont.CreateFromHandle(formFont);
      this._mgr.Add(formFont, fromHandle);
      return fromHandle;
    }
  }

  /// <summary>
  /// Gets the number of <see cref="T:Patagames.Pdf.Net.PdfFont" /> contained in the collection.
  /// </summary>
  public int Count => Pdfium.FPDFInterForm_CountFormFonts(this._forms.Handle);

  /// <summary>
  /// Gets a value indicating whether the collection is read-only.
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>
  /// Determines whether the collection contains a specific <see cref="T:Patagames.Pdf.Net.PdfFont" />.
  /// </summary>
  /// <param name="item">The <see cref="T:Patagames.Pdf.Net.PdfFont" /> to locate in the collection.</param>
  /// <returns>true if <see cref="T:Patagames.Pdf.Net.PdfFont" /> is found in the collection; otherwise, false.</returns>
  public bool Contains(PdfFont item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Determines the index of a specific  <see cref="T:Patagames.Pdf.Net.PdfFont" /> in the collection.
  /// </summary>
  /// <param name="item">The  <see cref="T:Patagames.Pdf.Net.PdfFont" /> to locate in the collection</param>
  /// <returns>The index of  <see cref="T:Patagames.Pdf.Net.PdfFont" /> if found in the collection; otherwise, -1.</returns>
  public int IndexOf(PdfFont item)
  {
    for (int index = 0; index < this.Count; ++index)
    {
      if (this[index].Handle == item.Handle)
        return index;
    }
    return -1;
  }

  /// <summary>
  /// Copies the elements of the collections to an System.Array, starting at a particular System.Array index.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from collection. The System.Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  public void CopyTo(PdfFont[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException();
    foreach (PdfFont pdfFont in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = pdfFont;
    }
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<PdfFont> GetEnumerator()
  {
    return (IEnumerator<PdfFont>) new CollectionEnumerator<PdfFont>((IReadOnlyList<PdfFont>) this);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  /// <summary>
  /// Adds the <paramref name="font" /> to the <see cref="T:Patagames.Pdf.Net.PdfFontCollection" />.
  /// </summary>
  /// <param name="font">The <see cref="T:Patagames.Pdf.Net.PdfFont" /> to add to the <see cref="T:Patagames.Pdf.Net.PdfFontCollection" />.</param>
  /// <param name="tagName">An optional tag name under which the <paramref name="font" /> will be added to the collection. Four characters max.</param>
  /// <returns>The tag name under which the <paramref name="font" /> was actually added to the collection.</returns>
  public string Add(PdfFont font, string tagName = null)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    return Pdfium.FPDFInterForm_AddFormFont(this._forms.Handle, font.Handle, tagName);
  }

  /// <summary>
  /// Removes the first occurrence of a specific <paramref name="font" /> from the <see cref="T:Patagames.Pdf.Net.PdfFontCollection" />.
  /// </summary>
  /// <param name="font">The <see cref="T:Patagames.Pdf.Net.PdfFont" /> to remove from the <see cref="T:Patagames.Pdf.Net.PdfFontCollection" />.</param>
  public void Remove(PdfFont font)
  {
    if (font == null)
      throw new ArgumentNullException(nameof (font));
    Pdfium.FPDFInterForm_RemoveFormFont(this._forms.Handle, font.Handle);
  }

  /// <summary>
  /// Removes the first occurrence of a specific <paramref name="tagName" /> from the <see cref="T:Patagames.Pdf.Net.PdfFontCollection" />.
  /// </summary>
  /// <param name="tagName">The font's tag name to remove from the <see cref="T:Patagames.Pdf.Net.PdfFontCollection" />.</param>
  public void Remove(string tagName)
  {
    if (tagName == null)
      throw new ArgumentNullException(nameof (tagName));
    Pdfium.FPDFInterForm_RemoveFormFont(this._forms.Handle, tagName);
  }

  /// <summary>
  /// Gets the <see cref="T:Patagames.Pdf.Net.PdfFont" /> with the specified <paramref name="tagName" />
  /// </summary>
  /// <param name="tagName">The font's tag name.</param>
  /// <returns>The <see cref="T:Patagames.Pdf.Net.PdfFont" /> at the specified <paramref name="tagName" />.</returns>
  public PdfFont this[string tagName]
  {
    get
    {
      if (tagName == null)
        throw new ArgumentNullException(tagName);
      IntPtr formFont = Pdfium.FPDFInterForm_GetFormFont(this._forms.Handle, tagName);
      if (formFont == IntPtr.Zero)
        return (PdfFont) null;
      if (this._mgr.Contains(formFont))
        return this._mgr.Get(formFont);
      PdfFont fromHandle = PdfFont.CreateFromHandle(formFont);
      this._mgr.Add(formFont, fromHandle);
      return fromHandle;
    }
  }

  /// <summary>Gets the tag name at the specified index</summary>
  /// <param name="index">The zero-based index of the element to get.</param>
  /// <returns>The tag name at the specified index.</returns>
  public string GetTagName(int index)
  {
    if (index < 0 || index >= this.Count)
      throw new ArgumentOutOfRangeException();
    string tagName;
    if (Pdfium.FPDFInterForm_GetFormFont(this._forms.Handle, index, true, out tagName) == IntPtr.Zero)
      throw Pdfium.ProcessLastError();
    return tagName;
  }
}
