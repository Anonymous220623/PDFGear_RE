// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.EditableRangeCollection
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class EditableRangeCollection : CollectionImpl
{
  internal EditableRange this[string id] => this.FindById(id);

  internal EditableRangeCollection(WordDocument doc)
    : base(doc, (OwnerHolder) doc)
  {
  }

  internal EditableRange FindById(string id)
  {
    for (int index = 0; index < this.InnerList.Count; ++index)
    {
      EditableRange inner = this.InnerList[index] as EditableRange;
      if (inner.Id.Equals(id))
        return inner;
    }
    return (EditableRange) null;
  }

  internal void RemoveAt(int index) => this.Remove(this.InnerList[index] as EditableRange);

  internal void Remove(EditableRange editableRange)
  {
    this.InnerList.Remove((object) editableRange);
    EditableRangeStart editableRangeStart = editableRange.EditableRangeStart;
    EditableRangeEnd editableRangeEnd = editableRange.EditableRangeEnd;
    editableRangeStart?.RemoveSelf();
    editableRangeEnd?.RemoveSelf();
  }

  internal void Add(EditableRange editableRange) => this.InnerList.Add((object) editableRange);

  internal void AttachEditableRangeStart(EditableRangeStart editableRangeStart)
  {
    if (this[editableRangeStart.Id] != null)
    {
      editableRangeStart.SetId(editableRangeStart.Id + Guid.NewGuid().ToString());
      editableRangeStart.RemoveSelf();
    }
    else
      this.Add(new EditableRange(editableRangeStart));
  }

  internal void AttacheEditableRangeEnd(EditableRangeEnd editableRangeEnd)
  {
    EditableRange editableRange = this[editableRangeEnd.Id];
    if (editableRange == null)
      return;
    EditableRangeEnd editableRangeEnd1 = editableRange.EditableRangeEnd;
    if (editableRangeEnd1 != null)
    {
      editableRangeEnd.RemoveSelf();
      if (editableRange.EditableRangeEnd != null)
        return;
      editableRange.SetEnd(editableRangeEnd1);
    }
    else
      editableRange.SetEnd(editableRangeEnd);
  }
}
