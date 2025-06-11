// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Net.Actions.PdfActionCollection
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Pdf.Net.BasicTypes;
using Patagames.Pdf.Net.Exceptions;
using Patagames.Pdf.Properties;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Patagames.Pdf.Net.Actions;

/// <summary>
/// Represents the sequence of actions to be performed after the <see cref="P:Patagames.Pdf.Net.Actions.PdfActionCollection.Parent" /> action.
/// </summary>
public class PdfActionCollection : 
  IList<PdfAction>,
  ICollection<PdfAction>,
  IEnumerable<PdfAction>,
  IEnumerable
{
  private PdfIndirectList _list;
  private Dictionary<IntPtr, PdfAction> _mgr = new Dictionary<IntPtr, PdfAction>();

  /// <summary>Gets the parent action.</summary>
  public PdfAction Parent { get; private set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="T:Patagames.Pdf.Net.Actions.PdfActionCollection" />.
  /// </summary>
  /// <param name="action">The action that will be associated with this class.</param>
  internal PdfActionCollection(PdfAction action)
  {
    this.Parent = action != null ? action : throw new ArgumentNullException(nameof (action));
    this._list = PdfIndirectList.FromPdfDocument(action.Document);
  }

  /// <summary>
  /// Gets the number of actions contained in the collection.
  /// </summary>
  public int Count => this.CountInHost();

  /// <summary>Gets or sets the action at the specified index</summary>
  /// <param name="index">The zero-based index of the element to get or set.</param>
  /// <returns>The action at the specified index.</returns>
  public PdfAction this[int index]
  {
    get
    {
      return index >= 0 && index < this.Count ? this.MgrCreateAction(index) : throw new IndexOutOfRangeException();
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException();
      if (index < 0 || index > this.Count - 1)
        throw new IndexOutOfRangeException();
      if (value.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(value.Dictionary.Handle) != IntPtr.Zero)
        throw new ArgumentException(string.Format(Error.err0067, (object) "action", (object) "object"));
      this.AddToHost(index, value, PdfActionCollection.Modes.Replace);
      this.MgrAdd(value);
    }
  }

  /// <summary>
  /// Gets a value indicating whether the collection is read-only.
  /// </summary>
  public bool IsReadOnly => false;

  /// <summary>Adds an action to the collection</summary>
  /// <param name="item">The action to add to the collection</param>
  public void Add(PdfAction item)
  {
    if (item == null)
      throw new ArgumentNullException();
    this.AddToHost(this.Count, item, PdfActionCollection.Modes.Add);
    this.MgrAdd(item);
  }

  /// <summary>Removes all the actions from the collection</summary>
  public void Clear()
  {
    this.ClearHost();
    this.MgrClear();
  }

  /// <summary>
  /// Determines whether the collection contains a specific action.
  /// </summary>
  /// <param name="item">The action to locate in the collection.</param>
  /// <returns>true if action is found in the collection; otherwise, false.</returns>
  public bool Contains(PdfAction item) => this.IndexOf(item) >= 0;

  /// <summary>
  /// Determines the index of a specific action in the collection.
  /// </summary>
  /// <param name="item">The action to locate in the collection</param>
  /// <returns>The index of the action if found in the collection; otherwise, -1.</returns>
  public int IndexOf(PdfAction item)
  {
    return item != null ? this.GetIndexInHost(item) : throw new ArgumentNullException(nameof (item));
  }

  /// <summary>
  /// Inserts an action to the collection at the specified index.
  /// </summary>
  /// <param name="index">The zero-based index at which action should be inserted.</param>
  /// <param name="item">The action to insert into the collection.</param>
  public void Insert(int index, PdfAction item)
  {
    if (item == null)
      throw new ArgumentNullException();
    if (index < 0 || index > this.Count)
      throw new IndexOutOfRangeException();
    this.AddToHost(index, item, PdfActionCollection.Modes.Insert);
    this.MgrAdd(item);
  }

  /// <summary>
  /// Removes the first occurrence of a specific action from the collection.
  /// </summary>
  /// <param name="item">The action to remove from the collection.</param>
  /// <returns>
  /// true if action was successfully removed from the collection;
  /// otherwise, false. This method also returns false if action is not found in the
  /// original collection.
  /// </returns>
  public bool Remove(PdfAction item)
  {
    int index = this.IndexOf(item);
    if (index < 0)
      return false;
    this.RemoveAt(index);
    return true;
  }

  /// <summary>Removes the action at the specified index.</summary>
  /// <param name="index">The zero-based index of the action to remove.</param>
  public void RemoveAt(int index)
  {
    if (index < 0 || index > this.Count - 1)
      throw new IndexOutOfRangeException();
    this.MgrRemove(index);
    this.RemoveFromHost(index);
  }

  /// <summary>
  /// Copies the elements of the collections to an System.Array, starting at a particular System.Array index.
  /// </summary>
  /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied from collection. The System.Array must have zero-based indexing.</param>
  /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
  public void CopyTo(PdfAction[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (arrayIndex));
    foreach (PdfAction pdfAction in this)
    {
      if (arrayIndex > array.Length - 1)
        break;
      array[arrayIndex++] = pdfAction;
    }
  }

  /// <summary>
  /// Returns an enumerator that iterates through the collection.
  /// </summary>
  /// <returns>A System.Collections.Generic.IEnumerator that can be used to iterate through the collection.</returns>
  public IEnumerator<PdfAction> GetEnumerator()
  {
    return (IEnumerator<PdfAction>) new CollectionEnumerator<PdfAction>((IList<PdfAction>) this);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

  /// <summary>
  /// Determines whether the collection contains a specific action.
  /// </summary>
  /// <param name="dictionary">The action dictionary to locate in the collection.</param>
  /// <returns>The action if found in the collection; otherwise, null</returns>
  public PdfAction GetByDictionary(PdfTypeBase dictionary)
  {
    if (dictionary == null || dictionary.IsDisposed)
      return (PdfAction) null;
    if (dictionary is PdfTypeIndirect)
      dictionary = (dictionary as PdfTypeIndirect).Direct;
    if (dictionary == null || dictionary.IsDisposed)
      return (PdfAction) null;
    foreach (PdfAction byDictionary in this)
    {
      if (!byDictionary.Dictionary.IsDisposed && byDictionary.Dictionary.Handle == dictionary.Handle)
        return byDictionary;
    }
    return (PdfAction) null;
  }

  private bool IsEqualTypes(PdfTypeBase dict1, PdfTypeBase dict2)
  {
    if (dict1 == null || dict2 == null || !dict1.Is<PdfTypeDictionary>() || !dict2.Is<PdfTypeDictionary>())
      return false;
    PdfTypeDictionary pdfTypeDictionary1 = dict1.As<PdfTypeDictionary>();
    PdfTypeDictionary pdfTypeDictionary2 = dict2.As<PdfTypeDictionary>();
    return pdfTypeDictionary1.ContainsKey("Type") && pdfTypeDictionary2.ContainsKey("Type") && (pdfTypeDictionary1["Type"] as PdfTypeName).Value == (pdfTypeDictionary2["Type"] as PdfTypeName).Value;
  }

  private PdfAction MgrGetAction(int index)
  {
    PdfTypeDictionary fromHost = this.GetFromHost(index);
    if (fromHost != null && this._mgr.ContainsKey(fromHost.Handle))
    {
      PdfAction action = this._mgr[fromHost.Handle];
      if (!action.Dictionary.IsDisposed)
        return action;
      this._mgr.Remove(fromHost.Handle);
    }
    return (PdfAction) null;
  }

  private PdfAction MgrCreateAction(int index)
  {
    PdfAction action1 = this.MgrGetAction(index);
    PdfTypeDictionary fromHost = this.GetFromHost(index);
    if (action1 != null && this.IsEqualTypes((PdfTypeBase) action1.Dictionary, (PdfTypeBase) fromHost))
      return action1;
    if (fromHost == null)
      return (PdfAction) null;
    PdfAction action2 = PdfAction.FromHandle(this.Parent.Document, fromHost.Handle);
    this._mgr.Add(action2.Dictionary.Handle, action2);
    return action2;
  }

  private void MgrAdd(PdfAction item)
  {
    if (this._mgr.ContainsKey(item.Dictionary.Handle))
      return;
    this._mgr.Add(item.Dictionary.Handle, item);
  }

  private void MgrRemove(int index)
  {
    PdfAction action = this.MgrGetAction(index);
    if (action == null)
      return;
    this._mgr.Remove(action.Dictionary.Handle);
  }

  private void MgrClear() => this._mgr.Clear();

  private int CountInHost()
  {
    if (!this.Parent.Dictionary.ContainsKey("Next"))
      return 0;
    if (this.Parent.Dictionary["Next"].Is<PdfTypeDictionary>())
      return 1;
    if (this.Parent.Dictionary["Next"].Is<PdfTypeArray>())
      return this.Parent.Dictionary["Next"].As<PdfTypeArray>().Count;
    throw new PdfParserException(string.Format(Error.err0045, (object) "Next"));
  }

  private void AddToHost(int index, PdfAction value, PdfActionCollection.Modes mode)
  {
    if (value.Dictionary.ObjectNumber == 0 && Pdfium.FPDFOBJ_GetParentObj(value.Dictionary.Handle) != IntPtr.Zero)
      throw new ArgumentException(string.Format(Error.err0067, (object) "action", (object) "object"));
    this._list.Add((PdfTypeBase) value.Dictionary);
    if (!this.Parent.Dictionary.ContainsKey("Next"))
      this.Parent.Dictionary.SetIndirectAt("Next", this._list, (PdfTypeBase) value.Dictionary);
    else if (this.Parent.Dictionary["Next"].Is<PdfTypeDictionary>() && index == 0)
    {
      this.MgrRemove(0);
      this.Parent.Dictionary.SetIndirectAt("Next", this._list, (PdfTypeBase) value.Dictionary);
    }
    else if (this.Parent.Dictionary["Next"].Is<PdfTypeDictionary>() && index == 1)
    {
      PdfTypeArray pdfTypeArray = PdfTypeArray.Create();
      PdfTypeBase pdfTypeBase = this.Parent.Dictionary["Next"].As<PdfTypeDictionary>().ObjectNumber == 0 ? this.Parent.Dictionary["Next"].As<PdfTypeDictionary>().Clone() : (PdfTypeBase) this.Parent.Dictionary["Next"].As<PdfTypeDictionary>();
      this._list.Add(pdfTypeBase);
      pdfTypeArray.AddIndirect(this._list, pdfTypeBase);
      pdfTypeArray.AddIndirect(this._list, (PdfTypeBase) value.Dictionary);
      this.MgrRemove(0);
      this.Parent.Dictionary["Next"] = (PdfTypeBase) pdfTypeArray;
    }
    else if (this.Parent.Dictionary["Next"].Is<PdfTypeArray>() && mode == PdfActionCollection.Modes.Replace)
    {
      this.MgrRemove(index);
      this.Parent.Dictionary["Next"].As<PdfTypeArray>().SetAt(index, (PdfTypeBase) value.Dictionary, this._list);
    }
    else if (this.Parent.Dictionary["Next"].Is<PdfTypeArray>() && mode == PdfActionCollection.Modes.Insert)
    {
      this.Parent.Dictionary["Next"].As<PdfTypeArray>().Insert(index, (PdfTypeBase) value.Dictionary, this._list);
    }
    else
    {
      if (!this.Parent.Dictionary["Next"].Is<PdfTypeArray>() || mode != PdfActionCollection.Modes.Add)
        throw new PdfParserException(string.Format(Error.err0045, (object) "Next"));
      this.Parent.Dictionary["Next"].As<PdfTypeArray>().Add((PdfTypeBase) value.Dictionary, this._list);
    }
  }

  private void ClearHost()
  {
    if (!this.Parent.Dictionary.ContainsKey("Next"))
      return;
    this.Parent.Dictionary.Remove("Next");
  }

  private int GetIndexInHost(PdfAction item)
  {
    if (!this.Parent.Dictionary.ContainsKey("Next"))
      return -1;
    if (this.Parent.Dictionary["Next"].Is<PdfTypeDictionary>() && this.IsEqualTypes(this.Parent.Dictionary["Next"], (PdfTypeBase) item.Dictionary))
      return 0;
    if (this.Parent.Dictionary["Next"].Is<PdfTypeArray>())
    {
      int indexInHost = 0;
      foreach (PdfTypeBase a in this.Parent.Dictionary["Next"].As<PdfTypeArray>())
      {
        if (a.Is<PdfTypeDictionary>() && this.IsEqualTypes((PdfTypeBase) a.As<PdfTypeDictionary>(), (PdfTypeBase) item.Dictionary))
          return indexInHost;
        ++indexInHost;
      }
    }
    return -1;
  }

  private void RemoveFromHost(int index)
  {
    if (!this.Parent.Dictionary.ContainsKey("Next"))
      return;
    if (this.Parent.Dictionary["Next"].Is<PdfTypeDictionary>())
    {
      this.Parent.Dictionary.Remove("Next");
    }
    else
    {
      if (!this.Parent.Dictionary["Next"].Is<PdfTypeArray>())
        return;
      this.Parent.Dictionary["Next"].As<PdfTypeArray>().RemoveAt(index);
      if (this.Parent.Dictionary["Next"].As<PdfTypeArray>().Count != 0)
        return;
      this.Parent.Dictionary.Remove("Next");
    }
  }

  private PdfTypeDictionary GetFromHost(int index)
  {
    if (!this.Parent.Dictionary.ContainsKey("Next"))
      return (PdfTypeDictionary) null;
    if (this.Parent.Dictionary["Next"].Is<PdfTypeDictionary>())
      return this.Parent.Dictionary["Next"].As<PdfTypeDictionary>();
    return this.Parent.Dictionary["Next"].Is<PdfTypeArray>() ? this.Parent.Dictionary["Next"].As<PdfTypeArray>()[index].As<PdfTypeDictionary>() : (PdfTypeDictionary) null;
  }

  private enum Modes
  {
    Replace,
    Insert,
    Add,
  }
}
