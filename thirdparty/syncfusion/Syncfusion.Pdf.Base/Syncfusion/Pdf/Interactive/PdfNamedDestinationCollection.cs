// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfNamedDestinationCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfNamedDestinationCollection : IEnumerable, IPdfWrapper
{
  private List<PdfNamedDestination> namedCollections = new List<PdfNamedDestination>();
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfCrossTable m_crossTable = new PdfCrossTable();
  internal int count;
  private PdfArray m_namedDestination = new PdfArray();
  private System.Collections.Generic.Dictionary<string, PdfNamedDestination> m_destCollection = new System.Collections.Generic.Dictionary<string, PdfNamedDestination>();

  public PdfNamedDestinationCollection() => this.Initialize();

  internal PdfNamedDestinationCollection(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    this.m_dictionary = dictionary;
    if (crossTable != null)
      this.m_crossTable = crossTable;
    if (this.Dictionary != null && this.Dictionary.ContainsKey("Dests"))
    {
      if (PdfCrossTable.Dereference(this.Dictionary["Dests"]) is PdfDictionary namedDictionary && namedDictionary.ContainsKey("Names"))
        this.AddCollection(namedDictionary);
      else if (namedDictionary != null && namedDictionary.ContainsKey("Kids") && PdfCrossTable.Dereference(namedDictionary["Kids"]) is PdfArray pdfArray)
      {
        for (int index = 0; index < pdfArray.Count; ++index)
          this.FindDestination(PdfCrossTable.Dereference(pdfArray[index]) as PdfDictionary);
      }
    }
    this.CrossTable.Document.Catalog.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    this.CrossTable.Document.Catalog.Modify();
  }

  public int Count => this.namedCollections.Count;

  public PdfNamedDestination this[int index]
  {
    get
    {
      return index >= 0 && index <= this.Count - 1 ? this.namedCollections[index] : throw new ArgumentOutOfRangeException(nameof (index), "Index is out of range.");
    }
  }

  internal PdfDictionary Dictionary => this.m_dictionary;

  internal PdfCrossTable CrossTable => this.m_crossTable;

  public void Add(PdfNamedDestination namedDestination)
  {
    if (namedDestination == null)
      throw new ArgumentNullException("The named destination value can't be null");
    this.namedCollections.Add(namedDestination);
  }

  public bool Contains(PdfNamedDestination namedDestination)
  {
    return namedDestination != null ? this.namedCollections.Contains(namedDestination) : throw new ArgumentNullException("The named destination value can't be null");
  }

  public void Remove(string title)
  {
    if (title == null)
      throw new ArgumentNullException("The title can't be null");
    int index1 = -1;
    for (int index2 = 0; index2 < this.namedCollections.Count; ++index2)
    {
      if (this.namedCollections[index2].Title.Equals(title))
      {
        index1 = index2;
        break;
      }
    }
    this.RemoveAt(index1);
  }

  public void RemoveAt(int index)
  {
    if (index >= this.namedCollections.Count)
      throw new PdfException("The index value should not be greater than or equal to the count ");
    this.namedCollections.RemoveAt(index);
  }

  public void Clear() => this.namedCollections.Clear();

  public void Insert(int index, PdfNamedDestination namedDestination)
  {
    if (namedDestination == null)
      throw new ArgumentNullException("The named destination value can't be null");
    if (index < 0 || index > this.Count)
      throw new IndexOutOfRangeException("The index can't be less then zero or greater then Count.");
    this.namedCollections.Insert(index, namedDestination);
  }

  internal void Initialize()
  {
    this.Dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    foreach (PdfNamedDestination namedCollection in this.namedCollections)
    {
      this.m_namedDestination.Add((IPdfPrimitive) new PdfString(namedCollection.Title));
      this.m_namedDestination.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) namedCollection));
      this.m_destCollection[namedCollection.Title] = namedCollection;
    }
    bool flag = true;
    if (ars != null && ars.Writer != null && ars.Writer.Document != null && ars.Writer.Document.Catalog != null)
    {
      PdfCatalogNames names = ars.Writer.Document.Catalog.Names;
      if (names != null && names.m_dictionary.ContainsKey("Dests"))
        flag = false;
    }
    if (this.Dictionary.ContainsKey("Dests"))
    {
      if (PdfCrossTable.Dereference(this.Dictionary["Dests"]) is PdfDictionary pdfDictionary1 && !pdfDictionary1.ContainsKey("Kids"))
      {
        this.ReviseNamedDestinationOrder();
        pdfDictionary1.SetProperty("Names", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_namedDestination));
      }
      else if (pdfDictionary1 != null && pdfDictionary1.ContainsKey("Kids") && pdfDictionary1.ContainsKey("Limits"))
      {
        this.ReviseNamedDestinationOrder();
        pdfDictionary1.Clear();
        pdfDictionary1.SetProperty("Names", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_namedDestination));
      }
    }
    else if (flag)
    {
      this.ReviseNamedDestinationOrder();
      PdfDictionary pdfDictionary2 = new PdfDictionary();
      pdfDictionary2.SetProperty("Names", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_namedDestination));
      this.Dictionary.SetProperty("Dests", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) pdfDictionary2));
    }
    else
    {
      this.ReviseNamedDestinationOrder();
      this.Dictionary.SetProperty("Names", (IPdfPrimitive) new PdfReferenceHolder((IPdfPrimitive) this.m_namedDestination));
    }
    if (this.m_destCollection.Count <= 0)
      return;
    this.m_destCollection.Clear();
  }

  private void ReviseNamedDestinationOrder()
  {
    if (this.m_destCollection == null || this.m_destCollection.Count <= 0)
      return;
    System.StringComparer ordinal = System.StringComparer.Ordinal;
    List<string> stringList = new List<string>((IEnumerable<string>) this.m_destCollection.Keys);
    stringList.Sort((IComparer<string>) ordinal);
    this.m_namedDestination.Clear();
    foreach (string key in stringList)
    {
      this.m_namedDestination.Add((IPdfPrimitive) new PdfString(key));
      this.m_namedDestination.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_destCollection[key]));
    }
    stringList.Clear();
  }

  private void FindDestination(PdfDictionary destination)
  {
    if (destination != null && destination.ContainsKey("Names"))
    {
      this.AddCollection(destination);
    }
    else
    {
      if (destination == null || !destination.ContainsKey("Kids") || !(PdfCrossTable.Dereference(destination["Kids"]) is PdfArray pdfArray))
        return;
      for (int index = 0; index < pdfArray.Count; ++index)
        this.FindDestination(PdfCrossTable.Dereference(pdfArray[index]) as PdfDictionary);
    }
  }

  private void AddCollection(PdfDictionary namedDictionary)
  {
    if (!(PdfCrossTable.Dereference(namedDictionary["Names"]) is PdfArray pdfArray))
      return;
    for (int index = 1; index <= pdfArray.Count; index += 2)
    {
      PdfReferenceHolder pdfReferenceHolder = pdfArray[index] as PdfReferenceHolder;
      PdfDictionary dictionary = (PdfDictionary) null;
      if (pdfReferenceHolder != (PdfReferenceHolder) null && pdfReferenceHolder.Object is PdfArray)
      {
        dictionary = new PdfDictionary();
        dictionary.SetProperty("D", (IPdfPrimitive) new PdfArray(pdfReferenceHolder.Object as PdfArray));
      }
      else if (pdfReferenceHolder == (PdfReferenceHolder) null && pdfArray[index] is PdfArray)
      {
        dictionary = new PdfDictionary();
        PdfArray array = pdfArray[index] as PdfArray;
        dictionary.SetProperty("D", (IPdfPrimitive) new PdfArray(array));
      }
      else if (pdfReferenceHolder != (PdfReferenceHolder) null)
        dictionary = pdfReferenceHolder.Object as PdfDictionary;
      else if (dictionary == null && pdfReferenceHolder == (PdfReferenceHolder) null && pdfArray[index] is PdfString && pdfArray.Count > index + 1 && PdfCrossTable.Dereference(pdfArray[index + 1]) is PdfArray)
      {
        ++index;
        dictionary = new PdfDictionary();
        if (PdfCrossTable.Dereference(pdfArray[index]) is PdfArray array)
          dictionary.SetProperty("D", (IPdfPrimitive) new PdfArray(array));
      }
      if (dictionary != null)
      {
        PdfLoadedNamedDestination namedDestination = new PdfLoadedNamedDestination(dictionary, this.m_crossTable);
        if (pdfArray[index - 1] is PdfString pdfString)
          namedDestination.Title = pdfString.Value;
        this.namedCollections.Add((PdfNamedDestination) namedDestination);
      }
    }
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.namedCollections.GetEnumerator();

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
