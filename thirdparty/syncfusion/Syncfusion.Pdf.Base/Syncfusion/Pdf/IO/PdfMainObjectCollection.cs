// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IO.PdfMainObjectCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.IO;

internal class PdfMainObjectCollection
{
  private List<ObjectInfo> m_objectCollection = new List<ObjectInfo>();
  internal Dictionary<long, ObjectInfo> mainObjectCollection = new Dictionary<long, ObjectInfo>();
  private Dictionary<IPdfPrimitive, long> m_primitiveObjectCollection = new Dictionary<IPdfPrimitive, long>();
  private HashSet<IPdfPrimitive> m_primitiveObjects = new HashSet<IPdfPrimitive>();
  private int m_index;
  internal int m_maximumReferenceObjNumber;
  protected static object s_syncObject = new object();

  internal ObjectInfo this[int index]
  {
    get
    {
      if (index < 0 || index > this.m_objectCollection.Count)
        throw new ArgumentOutOfRangeException(nameof (index));
      return this.m_objectCollection[index];
    }
  }

  internal int Count => this.m_objectCollection.Count;

  internal PdfMainObjectCollection()
  {
  }

  internal void Add(IPdfPrimitive element)
  {
    if (element == null)
      throw new ArgumentNullException(nameof (element));
    if (element is IPdfWrapper)
      element = (element as IPdfWrapper).Element;
    this.m_objectCollection.Add(new ObjectInfo(element));
    this.m_primitiveObjects.Add(element);
    if (!this.m_primitiveObjectCollection.ContainsKey(element))
      this.m_primitiveObjectCollection.Add(element, (long) (this.m_objectCollection.Count - 1));
    element.Position = this.m_index = this.m_objectCollection.Count - 1;
    element.Status = ObjectStatus.Registered;
  }

  internal void Add(IPdfPrimitive obj, PdfReference reference)
  {
    if (obj == null)
      throw new ArgumentNullException("element");
    if (reference == (PdfReference) null)
      throw new ArgumentNullException(nameof (reference));
    if (obj is IPdfWrapper)
      obj = (obj as IPdfWrapper).Element;
    lock (PdfMainObjectCollection.s_syncObject)
    {
      ObjectInfo objectInfo = new ObjectInfo(obj, reference);
      if ((long) this.m_maximumReferenceObjNumber < reference.ObjNum)
        this.m_maximumReferenceObjNumber = (int) reference.ObjNum;
      this.m_objectCollection.Add(objectInfo);
      this.m_primitiveObjects.Add(objectInfo.Object);
      if (!this.m_primitiveObjectCollection.ContainsKey(objectInfo.Object))
        this.m_primitiveObjectCollection.Add(objectInfo.Object, (long) (this.m_objectCollection.Count - 1));
      this.mainObjectCollection.Add(reference.ObjNum, objectInfo);
    }
    obj.Position = reference.Position = this.m_objectCollection.Count - 1;
  }

  internal void Remove(int index)
  {
    lock (PdfMainObjectCollection.s_syncObject)
    {
      if (this.m_objectCollection.Count > index)
      {
        IPdfPrimitive pdfPrimitive = this.m_objectCollection[index].Object;
        if (this.m_primitiveObjects.Contains(pdfPrimitive))
          this.m_primitiveObjects.Remove(pdfPrimitive);
      }
      if (this.m_objectCollection.Count <= index)
        return;
      this.m_objectCollection.RemoveAt(index);
    }
  }

  internal bool Contains(IPdfPrimitive element) => this.LookFor(element) >= 0;

  internal bool ContainsReference(PdfReference reference)
  {
    return this.mainObjectCollection.ContainsKey(reference.ObjNum);
  }

  internal PdfReference GetReference(int index) => this.m_objectCollection[index].Reference;

  internal PdfReference GetReference(IPdfPrimitive obj, out bool isNew)
  {
    this.m_index = this.LookFor(obj);
    PdfReference reference;
    if (this.m_index < 0 || this.m_index > this.Count)
    {
      isNew = true;
      reference = (PdfReference) null;
    }
    else
    {
      isNew = false;
      reference = this.m_objectCollection[this.m_index].Reference;
    }
    return reference;
  }

  internal IPdfPrimitive GetObject(int index) => this.m_objectCollection[index].Object;

  internal IPdfPrimitive GetObject(PdfReference reference)
  {
    try
    {
      return this.mainObjectCollection[reference.ObjNum].Object;
    }
    catch
    {
      return (IPdfPrimitive) null;
    }
  }

  internal int GetObjectIndex(PdfReference reference)
  {
    int objectIndex = -1;
    if (reference.Position != -1)
      return reference.Position;
    if (this.mainObjectCollection.Count == 0)
    {
      if (this.m_objectCollection.Count == 0)
        return objectIndex;
      for (int index = 0; index < this.m_objectCollection.Count - 1; ++index)
        this.mainObjectCollection.Add(this.m_objectCollection[index].Reference.ObjNum, this.m_objectCollection[index]);
      return !this.mainObjectCollection.ContainsKey(reference.ObjNum) ? objectIndex : 0;
    }
    return !this.mainObjectCollection.ContainsKey(reference.ObjNum) ? objectIndex : 0;
  }

  internal bool TrySetReference(IPdfPrimitive obj, PdfReference reference, out bool found)
  {
    if (obj == null)
      throw new ArgumentNullException(nameof (obj));
    if (reference == (PdfReference) null)
      throw new ArgumentNullException(nameof (reference));
    bool flag = true;
    found = true;
    this.m_index = this.LookFor(obj);
    if (this.m_index < 0 || this.m_index >= this.m_objectCollection.Count)
    {
      flag = false;
      found = false;
    }
    else
    {
      ObjectInfo objectInfo = this.m_objectCollection[this.m_index];
      if (objectInfo.Reference != (PdfReference) null)
        flag = false;
      else
        objectInfo.SetReference(reference);
    }
    return flag;
  }

  internal int IndexOf(IPdfPrimitive element) => this.LookFor(element);

  internal void ReregisterReference(int oldObjIndex, IPdfPrimitive newObj)
  {
    if (newObj == null)
      throw new ArgumentNullException(nameof (newObj));
    ObjectInfo objectInfo = oldObjIndex >= 0 && oldObjIndex <= this.Count ? this.m_objectCollection[oldObjIndex] : throw new ArgumentOutOfRangeException("oldObjectIndex");
    if (objectInfo.Object != newObj)
    {
      this.m_primitiveObjectCollection.Remove(objectInfo.Object);
      this.m_primitiveObjectCollection.Add(newObj, (long) oldObjIndex);
      this.m_primitiveObjects.Remove(objectInfo.Object);
      this.m_primitiveObjects.Add(newObj);
    }
    objectInfo.Object = newObj;
    newObj.Position = oldObjIndex;
  }

  internal void ReregisterReference(IPdfPrimitive oldObj, IPdfPrimitive newObj)
  {
    if (oldObj == null)
      throw new ArgumentNullException(nameof (oldObj));
    if (newObj == null)
      throw new ArgumentNullException(nameof (newObj));
    int oldObjIndex = this.IndexOf(oldObj);
    if (oldObjIndex < 0)
      throw new ArgumentException("Can't reregister an object.", nameof (oldObj));
    this.ReregisterReference(oldObjIndex, newObj);
  }

  private int LookFor(IPdfPrimitive obj)
  {
    lock (PdfMainObjectCollection.s_syncObject)
    {
      int index1 = -1;
      if (obj.Position != -1)
        return obj.Position;
      if (this.Count == this.m_primitiveObjectCollection.Count)
      {
        if (this.m_primitiveObjectCollection.ContainsKey(obj))
        {
          index1 = (int) this.m_primitiveObjectCollection[obj];
          if (this.Count > index1 && this.m_objectCollection[index1].Object != obj)
          {
            for (int index2 = this.Count - 1; index2 >= 0; --index2)
            {
              if (this.m_objectCollection[index2].Object == obj)
              {
                index1 = index2;
                break;
              }
            }
          }
        }
        else if (obj.ClonedObject != null && this.m_primitiveObjectCollection.ContainsKey(obj.ClonedObject))
          index1 = (int) this.m_primitiveObjectCollection[obj.ClonedObject];
      }
      else if (this.m_primitiveObjects.Contains(obj))
      {
        for (int index3 = this.Count - 1; index3 >= 0; --index3)
        {
          if (this.m_objectCollection[index3].Object == obj)
          {
            index1 = index3;
            break;
          }
        }
      }
      return index1;
    }
  }

  private int LookForReference(PdfReference reference)
  {
    int num = -1;
    if (reference.Position != -1)
      return reference.Position;
    for (int index = this.Count - 1; index >= 0; --index)
    {
      if (this.m_objectCollection[index].Reference == reference)
      {
        num = index;
        break;
      }
    }
    return num;
  }
}
