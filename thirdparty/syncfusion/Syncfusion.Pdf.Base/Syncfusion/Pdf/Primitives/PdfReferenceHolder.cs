// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Primitives.PdfReferenceHolder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using System;

#nullable disable
namespace Syncfusion.Pdf.Primitives;

internal class PdfReferenceHolder : IPdfPrimitive
{
  private IPdfPrimitive m_object;
  private PdfCrossTable m_crossTable;
  private PdfReference m_reference;
  private int m_objectIndex = -1;
  private ObjectStatus m_status;
  private bool m_isSaving;
  private int m_index;
  private int m_position = -1;
  private static object m_lock = new object();

  internal IPdfPrimitive Object
  {
    get
    {
      if (this.m_reference != (PdfReference) null || this.m_object == null)
        this.m_object = this.ObtainObject();
      return this.m_object;
    }
  }

  internal int Index
  {
    get
    {
      PdfMainObjectCollection pdfObjects = this.m_crossTable.PdfObjects;
      this.m_objectIndex = pdfObjects.GetObjectIndex(this.m_reference);
      if (this.m_objectIndex < 0)
      {
        lock (PdfReferenceHolder.m_lock)
        {
          this.m_crossTable.GetObject((IPdfPrimitive) this.m_reference);
          this.m_objectIndex = pdfObjects.Count - 1;
        }
      }
      return this.m_objectIndex;
    }
  }

  public PdfReference Reference => this.m_reference;

  public ObjectStatus Status
  {
    get => this.m_status;
    set => this.m_status = value;
  }

  public bool IsSaving
  {
    get => this.m_isSaving;
    set => this.m_isSaving = value;
  }

  public int ObjectCollectionIndex
  {
    get => this.m_index;
    set => this.m_index = value;
  }

  public int Position
  {
    get => this.m_position;
    set => this.m_position = value;
  }

  public IPdfPrimitive ClonedObject => (IPdfPrimitive) null;

  public PdfReferenceHolder(IPdfWrapper wrapper)
    : this(wrapper.Element)
  {
  }

  public PdfReferenceHolder(IPdfPrimitive obj)
  {
    this.m_object = obj != null ? obj : throw new ArgumentNullException(nameof (obj));
  }

  internal PdfReferenceHolder(PdfReference reference, PdfCrossTable crossTable)
  {
    if (crossTable == null)
      throw new ArgumentNullException(nameof (crossTable));
    if (reference == (PdfReference) null)
      throw new ArgumentNullException(nameof (reference));
    this.m_crossTable = crossTable;
    this.m_reference = reference;
  }

  public void Save(IPdfWriter writer)
  {
    long num = writer != null ? writer.Position : throw new ArgumentNullException(nameof (writer));
    PdfCrossTable crossTable = writer.Document.CrossTable;
    if (crossTable.Document is PdfDocument)
      this.Object.IsSaving = true;
    PdfReference pdfReference = !writer.Document.FileStructure.IncrementalUpdate || !writer.Document.m_isStreamCopied ? crossTable.GetReference(this.Object) : (!(this.m_reference == (PdfReference) null) ? this.m_reference : crossTable.GetReference(this.Object));
    if (writer.Position != num)
      writer.Position = num;
    pdfReference.Save(writer);
  }

  public IPdfPrimitive Clone(PdfCrossTable crossTable)
  {
    string empty = string.Empty;
    if (this.Reference != (PdfReference) null && this.m_crossTable != null && this.m_crossTable.PageCorrespondance.ContainsKey((IPdfPrimitive) this.Reference))
      return (IPdfPrimitive) new PdfReferenceHolder(this.m_crossTable.PageCorrespondance[(IPdfPrimitive) this.Reference] as PdfReference, crossTable);
    IPdfPrimitive pdfPrimitive1;
    if (this.m_crossTable != null && this.m_crossTable.PageCorrespondance.ContainsKey(this.Object))
    {
      if (!(this.m_crossTable.PageCorrespondance[this.Object] is PdfPageBase pdfPageBase) || pdfPageBase.Dictionary == null)
        return (IPdfPrimitive) new PdfNull();
      pdfPrimitive1 = (IPdfPrimitive) pdfPageBase.Dictionary;
    }
    else
    {
      if (this.Object is PdfNumber)
        return (IPdfPrimitive) new PdfNumber((this.Object as PdfNumber).FloatValue);
      if (this.Object is PdfDictionary)
      {
        PdfName key = new PdfName("Type");
        PdfDictionary pdfDictionary = this.Object as PdfDictionary;
        if (pdfDictionary.ContainsKey(key))
        {
          PdfName pdfName = pdfDictionary[key] as PdfName;
          if (pdfName != (PdfName) null && pdfName.Value == "Page")
            return (IPdfPrimitive) new PdfNull();
        }
      }
      if ((object) (this.Object as PdfName) != null)
        return (IPdfPrimitive) new PdfName((this.Object as PdfName).Value);
      if (crossTable.PrevReference != null && crossTable.PrevReference.Contains(this.Reference))
      {
        IPdfPrimitive pdfPrimitive2 = crossTable.Document == null || crossTable.Document.EnableMemoryOptimization ? this.m_crossTable.GetObject((IPdfPrimitive) this.Reference).ClonedObject : this.m_crossTable.GetObject((IPdfPrimitive) this.Reference);
        return pdfPrimitive2 != null ? (IPdfPrimitive) new PdfReferenceHolder(crossTable.GetReference(pdfPrimitive2), crossTable) : (IPdfPrimitive) new PdfNull();
      }
      if (this.Reference != (PdfReference) null)
        crossTable.PrevReference.Add(this.Reference);
      pdfPrimitive1 = this.Object is PdfCatalog ? (IPdfPrimitive) crossTable.Document.Catalog : this.Object.Clone(crossTable);
    }
    return (IPdfPrimitive) new PdfReferenceHolder(crossTable.GetReference(pdfPrimitive1), crossTable);
  }

  public override bool Equals(object obj)
  {
    PdfReferenceHolder pdfReferenceHolder = obj as PdfReferenceHolder;
    bool flag = pdfReferenceHolder != (PdfReferenceHolder) null;
    if (flag)
    {
      if (this.m_reference != (PdfReference) null && pdfReferenceHolder.m_reference != (PdfReference) null)
        flag &= pdfReferenceHolder.m_reference == this.m_reference;
      else
        flag &= pdfReferenceHolder.Object == this.Object;
    }
    return flag;
  }

  public override int GetHashCode() => this.Object.GetHashCode();

  public static bool operator ==(PdfReferenceHolder rh1, PdfReferenceHolder rh2)
  {
    object obj1 = (object) rh1;
    object obj2 = (object) rh2;
    return obj1 == null || obj2 == null ? obj1 == obj2 : rh1.Equals((object) rh2);
  }

  public static bool operator !=(PdfReferenceHolder rh1, PdfReferenceHolder rh2) => !(rh1 == rh2);

  private IPdfPrimitive ObtainObject()
  {
    IPdfPrimitive pdfPrimitive = (IPdfPrimitive) null;
    if (this.m_reference != (PdfReference) null && this.m_crossTable.PdfObjects != null)
    {
      if (this.Index >= 0)
        pdfPrimitive = this.m_crossTable.PdfObjects.GetObject(this.m_reference);
    }
    else if (this.m_object != null)
      pdfPrimitive = this.m_object;
    return pdfPrimitive;
  }
}
