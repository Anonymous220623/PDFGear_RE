// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.ReferenceRecordsCollection
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

internal class ReferenceRecordsCollection : CollectionBase<ReferenceRecord>
{
  private VbaProject m_parent;

  internal ReferenceRecord Add(VbaReferenceType type)
  {
    ReferenceRecord referenceRecord = (ReferenceRecord) null;
    switch (type)
    {
      case VbaReferenceType.ReferenceRegister:
        referenceRecord = (ReferenceRecord) new ReferenceRegisterRecord();
        break;
      case VbaReferenceType.ReferenceProject:
        referenceRecord = (ReferenceRecord) new ReferenceProjectRecord();
        break;
      case VbaReferenceType.ReferenceControl:
        referenceRecord = (ReferenceRecord) new ReferenceControlRecord();
        break;
      case VbaReferenceType.ReferenceOriginal:
        referenceRecord = (ReferenceRecord) new ReferenceOriginalRecord();
        break;
    }
    this.Add(referenceRecord);
    return referenceRecord;
  }

  internal void Dispose() => this.Clear();

  internal ReferenceRecordsCollection Clone(VbaProject parent)
  {
    ReferenceRecordsCollection recordsCollection = (ReferenceRecordsCollection) this.Clone();
    recordsCollection.m_parent = parent;
    for (int index = 0; index < this.InnerList.Count; ++index)
    {
      ReferenceRecord inner = this.InnerList[index];
      recordsCollection.Add(inner.Clone());
    }
    return recordsCollection;
  }
}
