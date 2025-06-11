// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Properties;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.Wrappers;

/// <summary>Represents a document’s name tree as a flat list.</summary>
public class PdfNameTreeCollection : PdfWrapper, IEnumerable<PdfTypeBase>, IEnumerable
{
  private const int nMaxRecursion = 999;
  private string _name;

  /// <summary>
  /// Gets the document with which the collection is associated.
  /// </summary>
  public PdfDocument Document { get; private set; }

  /// <summary>Creates an empty document's name tree.</summary>
  /// <param name="doc">The Pdf document.</param>
  public PdfNameTreeCollection(PdfDocument doc)
  {
    this.Document = doc != null ? doc : throw new ArgumentNullException();
  }

  /// <summary>
  /// Initializes an instance of the <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" /> with the specified root node of the document's name tree.
  /// </summary>
  /// <param name="doc">The PDF document.</param>
  /// <param name="nameTreeRoot">The root node of the document's name tree.</param>
  public PdfNameTreeCollection(PdfDocument doc, PdfTypeDictionary nameTreeRoot)
    : base((PdfTypeBase) nameTreeRoot)
  {
    this.Document = doc != null ? doc : throw new ArgumentNullException(nameof (doc));
  }

  /// <summary>
  /// Initializes an instance of the <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" /> with the specified document's name tree.
  /// </summary>
  /// <param name="doc">the PDF document.</param>
  /// <param name="name">The name of the document's name tree.</param>
  public PdfNameTreeCollection(PdfDocument doc, string name)
    : base(PdfNameTreeCollection.GetDict(doc, name))
  {
    this.Document = doc;
    if (!doc.Root.ContainsKey("Names"))
    {
      this._name = name;
    }
    else
    {
      if (doc.Root["Names"].As<PdfTypeDictionary>().ContainsKey(name))
        return;
      this._name = name;
    }
  }

  private static PdfTypeBase GetDict(PdfDocument doc, string name)
  {
    if (doc == null)
      throw new ArgumentNullException(nameof (doc));
    if ((name ?? "").Trim() == "")
      throw new ArgumentException(string.Format(Error.err0060, (object) nameof (name)));
    if (!doc.Root.ContainsKey("Names"))
      return (PdfTypeBase) PdfTypeDictionary.Create();
    PdfTypeDictionary pdfTypeDictionary = doc.Root["Names"].As<PdfTypeDictionary>();
    if (!pdfTypeDictionary.ContainsKey(name))
      return (PdfTypeBase) PdfTypeDictionary.Create();
    return pdfTypeDictionary[name].Is<PdfTypeIndirect>() && pdfTypeDictionary[name].As<PdfTypeIndirect>().Direct == null && PdfIndirectList.FromPdfDocument(doc)[pdfTypeDictionary[name].As<PdfTypeIndirect>().Number] == null ? (PdfTypeBase) PdfTypeDictionary.Create() : (PdfTypeBase) pdfTypeDictionary[name].As<PdfTypeDictionary>();
  }

  /// <summary>
  /// Gets and sets the value associated with a specific name in a <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" /> object.
  /// </summary>
  /// <param name="name">The name associated with the value to get or set.</param>
  /// <returns>The value associated with the name parameter in the <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" /> object, if name is found; otherwise, null.</returns>
  /// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> or value is null.</exception>
  public PdfTypeBase this[string name]
  {
    get
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      int nIndex = 0;
      return this.SearchNameNode(this.Dictionary, name, ref nIndex, out PdfTypeArray _, out int _);
    }
    set
    {
      if (name == null)
        throw new ArgumentNullException(nameof (name));
      if (value == null)
        throw new ArgumentNullException();
      this.AddInternal(name, value, true);
    }
  }

  /// <summary>Gets or sets the element at the specified index.</summary>
  /// <param name="index">The zero-based index of the element to get or set.</param>
  /// <returns>The element at the specified index; or null if element not found.</returns>
  /// <exception cref="T:System.IndexOutOfRangeException"><paramref name="index" /> is not a valid index in the collection.</exception>
  /// <exception cref="T:System.ArgumentNullException">The value is null.</exception>
  public PdfTypeBase this[int index]
  {
    get
    {
      if (index < 0 || index >= this.Count)
        throw new IndexOutOfRangeException();
      int nCurIndex = 0;
      return this.SearchNameNode(this.Dictionary, index, ref nCurIndex, out string _, out PdfTypeArray _, out int _);
    }
    set
    {
      if (index < 0 || index >= this.Count)
        throw new IndexOutOfRangeException();
      this[this.NameOf(index)] = value != null ? value : throw new ArgumentNullException();
    }
  }

  /// <summary>
  /// Adds an element with the specified name and value to a <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" /> object.
  /// </summary>
  /// <param name="name">The name of the element to add.</param>
  /// <param name="item">The value of the element to add.</param>
  /// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> or <paramref name="item" /> is null.</exception>
  public void Add(string name, PdfTypeBase item)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    if (item == null)
      throw new ArgumentNullException();
    this.AddInternal(name, item, false);
  }

  /// <summary>
  /// Gets the number of elements contained in a <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" /> object.
  /// </summary>
  public int Count => this.CountNames(this.Dictionary);

  /// <summary>
  /// Removes all elements from a <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" /> object.
  /// </summary>
  public void Clear() => this.Dictionary.Clear();

  /// <summary>
  /// Determines whether a <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" /> object contains a specific name.
  /// </summary>
  /// <param name="name">The name to locate in the <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" /> object.</param>
  /// <returns>true if the <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" /> object contains an element with the specified name; otherwise, false.</returns>
  /// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> is null.</exception>
  public bool Contains(string name) => this.IndexOf(name) >= 0;

  /// <summary>
  /// Copies <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" /> elements to a one-dimensional System.Array object, starting at the specified index in the array.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array object that is the destination of the <see cref="T:Patagames.Pdf.Net.BasicTypes.PdfTypeBase" /> objects copied from <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" />. The System.Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  /// <exception cref="T:System.ArgumentNullException"><paramref name="array" /> is null.</exception>
  /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex" /> is less than zero.</exception>
  public void CopyTo(PdfTypeBase[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException();
    foreach (PdfTypeBase pdfTypeBase in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = pdfTypeBase;
    }
  }

  /// <summary>
  ///  Returns the zero-based index of the specified name in a <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" /> object.
  /// </summary>
  /// <param name="name">The name to locate in the <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" /> object.</param>
  /// <returns>The zero-based index of the name parameter, if name is found in the <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" /> object; otherwise, -1.</returns>
  /// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> is null.</exception>
  public int IndexOf(string name)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    int nIndex = 0;
    return this.SearchNameNode(this.Dictionary, name, ref nIndex, out PdfTypeArray _, out int _) != null ? nIndex : -1;
  }

  /// <summary>
  ///  Returns the name at the specified <paramref name="index" /> in a <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" /> object.
  /// </summary>
  /// <param name="index">The zero-based index.</param>
  /// <returns>The name.</returns>
  /// <exception cref="T:System.IndexOutOfRangeException"><paramref name="index" /> is not a valid index in the collection.</exception>
  public string NameOf(int index)
  {
    if (index < 0 || index >= this.Count)
      throw new IndexOutOfRangeException();
    int nCurIndex = 0;
    string name;
    return this.SearchNameNode(this.Dictionary, index, ref nCurIndex, out name, out PdfTypeArray _, out int _) == null ? (string) null : name;
  }

  /// <summary>
  ///  Removes the element with the specified name from a <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" /> object.
  /// </summary>
  /// <param name="name">The name of the element to remove.</param>
  /// <returns>true if the element is successfully removed; otherwise, false. This method also returns false if key was not found in the <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" />.</returns>
  /// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> is null.</exception>
  public bool Remove(string name)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    int nIndex = 0;
    PdfTypeArray ppFind;
    int ppIndex;
    if (this.SearchNameNode(this.Dictionary, name, ref nIndex, out ppFind, out ppIndex) == null)
      return false;
    ppFind.RemoveAt(ppIndex + 1);
    ppFind.RemoveAt(ppIndex);
    return true;
  }

  /// <summary>
  /// Removes the element at the specified index of a <see cref="T:Patagames.Pdf.Net.Wrappers.PdfNameTreeCollection" /> object.
  /// </summary>
  /// <param name="index">The zero-based index of the element to remove.</param>
  /// <returns>true if the element is successfully removed; otherwise, false.</returns>
  /// <exception cref="T:System.IndexOutOfRangeException"><paramref name="index" /> is not a valid index in the collection.</exception>
  public bool Remove(int index)
  {
    if (index < 0 || index >= this.Count)
      throw new IndexOutOfRangeException();
    int nCurIndex = 0;
    PdfTypeArray ppFind;
    int ppIndex;
    if (this.SearchNameNode(this.Dictionary, index, ref nCurIndex, out string _, out ppFind, out ppIndex) == null)
      return false;
    ppFind.RemoveAt(ppIndex + 1);
    ppFind.RemoveAt(ppIndex);
    return true;
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
  public IEnumerator<PdfTypeBase> GetEnumerator()
  {
    return (IEnumerator<PdfTypeBase>) new CollectionEnumerator<PdfTypeBase>(this);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  private int CountNames(PdfTypeDictionary pNode, int nLevel = 0)
  {
    if (nLevel > 999 || pNode == null)
      return 0;
    if (pNode.ContainsKey("Names"))
      return pNode["Names"].As<PdfTypeArray>().Count / 2;
    if (!pNode.ContainsKey("Kids"))
      return 0;
    PdfTypeArray pdfTypeArray = pNode["Kids"].As<PdfTypeArray>();
    int num = 0;
    for (int index = 0; index < pdfTypeArray.Count; ++index)
    {
      PdfTypeBase pdfTypeBase = pdfTypeArray[index];
      if (pdfTypeBase != null && pdfTypeBase.Is<PdfTypeDictionary>())
        num += this.CountNames(pdfTypeBase.As<PdfTypeDictionary>(), nLevel + 1);
    }
    return num;
  }

  private PdfTypeBase SearchNameNode(
    PdfTypeDictionary pNode,
    int nIndex,
    ref int nCurIndex,
    out string name,
    out PdfTypeArray ppFind,
    out int ppIndex,
    int nLevel = 0)
  {
    name = (string) null;
    ppFind = (PdfTypeArray) null;
    ppIndex = 0;
    if (nLevel > 999)
      return (PdfTypeBase) null;
    if (pNode.ContainsKey("Names"))
    {
      PdfTypeArray pdfTypeArray = pNode["Names"].As<PdfTypeArray>();
      int num = pdfTypeArray.Count / 2;
      if (nIndex >= nCurIndex + num)
      {
        nCurIndex += num;
        return (PdfTypeBase) null;
      }
      ppFind = pdfTypeArray;
      ppIndex = (nIndex - nCurIndex) * 2;
      name = pdfTypeArray[(nIndex - nCurIndex) * 2].As<PdfTypeString>().UnicodeString;
      return pdfTypeArray[(nIndex - nCurIndex) * 2 + 1];
    }
    if (!pNode.ContainsKey("Kids"))
      return (PdfTypeBase) null;
    PdfTypeArray pdfTypeArray1 = pNode["Kids"].As<PdfTypeArray>();
    for (int index = 0; index < pdfTypeArray1.Count; ++index)
    {
      PdfTypeBase pdfTypeBase1 = pdfTypeArray1[index];
      if (pdfTypeBase1 != null && pdfTypeBase1.Is<PdfTypeDictionary>())
      {
        PdfTypeBase pdfTypeBase2 = this.SearchNameNode(pdfTypeBase1.As<PdfTypeDictionary>(), nIndex, ref nCurIndex, out name, out ppFind, out ppIndex, nLevel + 1);
        if (pdfTypeBase2 != null)
          return pdfTypeBase2;
      }
    }
    return (PdfTypeBase) null;
  }

  private PdfTypeBase SearchNameNode(
    PdfTypeDictionary pNode,
    string name,
    ref int nIndex,
    out PdfTypeArray ppFind,
    out int ppIndex,
    int nLevel = 0)
  {
    ppFind = (PdfTypeArray) null;
    ppIndex = 0;
    if (nLevel > 999)
      return (PdfTypeBase) null;
    if (pNode.ContainsKey("Limits"))
    {
      PdfTypeArray pdfTypeArray = pNode["Limits"].As<PdfTypeArray>();
      string str1 = pdfTypeArray[0].As<PdfTypeString>().UnicodeString;
      string strB = pdfTypeArray[1].As<PdfTypeString>().UnicodeString;
      if (string.Compare(str1, strB) > 0)
      {
        string str2 = strB;
        strB = str1;
        str1 = str2;
      }
      if (string.Compare(name, str1) < 0 || string.Compare(name, strB) > 0)
        return (PdfTypeBase) null;
    }
    if (pNode.ContainsKey("Names"))
    {
      PdfTypeArray pdfTypeArray = pNode["Names"].As<PdfTypeArray>();
      int num1 = pdfTypeArray.Count / 2;
      for (int index = 0; index < num1; ++index)
      {
        int num2 = string.Compare(pdfTypeArray[index * 2].As<PdfTypeString>().UnicodeString, name);
        if (num2 <= 0)
        {
          ppFind = pdfTypeArray;
          if (num2 >= 0)
          {
            nIndex += index;
            ppIndex = index * 2;
            return pdfTypeArray[index * 2 + 1];
          }
        }
      }
      nIndex += num1;
      return (PdfTypeBase) null;
    }
    if (!pNode.ContainsKey("Kids"))
      return (PdfTypeBase) null;
    PdfTypeArray pdfTypeArray1 = pNode["Kids"].As<PdfTypeArray>();
    for (int index = 0; index < pdfTypeArray1.Count; ++index)
    {
      PdfTypeBase pdfTypeBase1 = pdfTypeArray1[index];
      if (pdfTypeBase1 != null && pdfTypeBase1.Is<PdfTypeDictionary>())
      {
        PdfTypeBase pdfTypeBase2 = this.SearchNameNode(pdfTypeBase1.As<PdfTypeDictionary>(), name, ref nIndex, out ppFind, out ppIndex, nLevel + 1);
        if (pdfTypeBase2 != null)
          return pdfTypeBase2;
      }
    }
    return (PdfTypeBase) null;
  }

  private PdfTypeBase AddNameNode(
    PdfTypeDictionary pNode,
    string name,
    PdfTypeBase value,
    ref int nIndex,
    out PdfTypeArray ppFind,
    out int ppIndex,
    bool canReplace,
    int nLevel = 0)
  {
    ppFind = (PdfTypeArray) null;
    ppIndex = 0;
    if (nLevel > 999)
      return (PdfTypeBase) null;
    if (pNode.ContainsKey("Limits"))
    {
      PdfTypeArray pdfTypeArray = pNode["Limits"].As<PdfTypeArray>();
      string str1 = pdfTypeArray[0].As<PdfTypeString>().UnicodeString;
      string strB = pdfTypeArray[1].As<PdfTypeString>().UnicodeString;
      if (string.Compare(str1, strB) > 0)
      {
        string str2 = strB;
        strB = str1;
        str1 = str2;
      }
      if (string.Compare(name, str1) < 0 || string.Compare(name, strB) > 0)
        return (PdfTypeBase) null;
    }
    if (!pNode.ContainsKey("Kids") && !pNode.ContainsKey("Names"))
      pNode["Names"] = (PdfTypeBase) PdfTypeArray.Create();
    if (pNode.ContainsKey("Names"))
    {
      PdfTypeArray pdfTypeArray = pNode["Names"].As<PdfTypeArray>();
      int num1 = pdfTypeArray.Count / 2;
      for (int index = 0; index < num1; ++index)
      {
        int num2 = string.Compare(pdfTypeArray[index * 2].As<PdfTypeString>().UnicodeString, name);
        if (num2 <= 0)
        {
          ppFind = pdfTypeArray;
          if (num2 >= 0)
          {
            if (!canReplace)
              throw new ArgumentException(Error.err0061);
            nIndex += index;
            ppIndex = index * 2;
            pdfTypeArray[index * 2 + 1] = value;
            return pdfTypeArray[index * 2 + 1];
          }
        }
        else
        {
          ppFind = pdfTypeArray;
          pdfTypeArray.Insert(index * 2, (PdfTypeBase) PdfTypeString.Create(name, true, true));
          pdfTypeArray.Insert(index * 2 + 1, value);
          nIndex += index;
          ppIndex = index * 2;
          return pdfTypeArray[index * 2 + 1];
        }
      }
      pdfTypeArray.Add((PdfTypeBase) PdfTypeString.Create(name, true, true));
      pdfTypeArray.Add(value);
      nIndex += num1;
      ppIndex = num1 * 2;
      ppFind = pdfTypeArray;
      return pdfTypeArray[num1 * 2 + 1];
    }
    if (!pNode.ContainsKey("Kids"))
      return (PdfTypeBase) null;
    PdfTypeArray pdfTypeArray1 = pNode["Kids"].As<PdfTypeArray>();
    for (int index = 0; index < pdfTypeArray1.Count; ++index)
    {
      PdfTypeBase pdfTypeBase1 = pdfTypeArray1[index];
      if (pdfTypeBase1 != null && pdfTypeBase1.Is<PdfTypeDictionary>())
      {
        PdfTypeBase pdfTypeBase2 = this.AddNameNode(pdfTypeBase1.As<PdfTypeDictionary>(), name, value, ref nIndex, out ppFind, out ppIndex, canReplace, nLevel + 1);
        if (pdfTypeBase2 != null)
          return pdfTypeBase2;
      }
    }
    return (PdfTypeBase) null;
  }

  private void AddInternal(string name, PdfTypeBase item, bool canReplace)
  {
    int nIndex = 0;
    bool flag;
    switch (item.ObjectType)
    {
      case IndirectObjectTypes.String:
      case IndirectObjectTypes.Array:
      case IndirectObjectTypes.Dictionary:
      case IndirectObjectTypes.Stream:
        flag = true;
        break;
      default:
        flag = item.ObjectNumber != 0;
        break;
    }
    if (flag)
    {
      PdfIndirectList list = PdfIndirectList.FromPdfDocument(this.Document);
      item = (PdfTypeBase) PdfTypeIndirect.Create(list, list.Add(item));
    }
    PdfTypeArray ppFind;
    int ppIndex;
    if (this.AddNameNode(this.Dictionary, name, item, ref nIndex, out ppFind, out ppIndex, canReplace) == null)
    {
      if (this.Dictionary.ContainsKey("Kids"))
      {
        PdfIndirectList list = PdfIndirectList.FromPdfDocument(this.Document);
        PdfTypeDictionary pdfTypeDictionary = PdfTypeDictionary.Create();
        int objectNumber = list.Add((PdfTypeBase) pdfTypeDictionary);
        this.Dictionary["Kids"].As<PdfTypeArray>().AddIndirect(list, objectNumber);
        pdfTypeDictionary["Limits"] = (PdfTypeBase) PdfTypeArray.Create();
        pdfTypeDictionary["Limits"].As<PdfTypeArray>().Add((PdfTypeBase) PdfTypeString.Create(name, true));
        pdfTypeDictionary["Limits"].As<PdfTypeArray>().Add((PdfTypeBase) PdfTypeString.Create(name, true));
        if (this.AddNameNode(pdfTypeDictionary, name, item, ref nIndex, out ppFind, out ppIndex, canReplace) != null)
        {
          this.InsertIntoDoc();
          return;
        }
      }
      throw new PdfParserException(Error.err0062);
    }
    this.InsertIntoDoc();
  }

  private void InsertIntoDoc()
  {
    if (this._name == null || this._name.Trim() == "")
      return;
    PdfIndirectList list = PdfIndirectList.FromPdfDocument(this.Document);
    PdfTypeDictionary pdfTypeDictionary;
    if (this.Document.Root.ContainsKey("Names"))
    {
      pdfTypeDictionary = this.Document.Root["Names"].As<PdfTypeDictionary>();
    }
    else
    {
      pdfTypeDictionary = PdfTypeDictionary.Create();
      list.Add((PdfTypeBase) pdfTypeDictionary);
      this.Document.Root.SetIndirectAt("Names", list, (PdfTypeBase) pdfTypeDictionary);
    }
    list.Add((PdfTypeBase) this.Dictionary);
    pdfTypeDictionary.SetIndirectAt(this._name, list, (PdfTypeBase) this.Dictionary);
    this._name = (string) null;
  }
}
