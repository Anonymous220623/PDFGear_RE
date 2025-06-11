// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.ExternNamesCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Parser;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class ExternNamesCollection : CollectionBaseEx<ExternNameImpl>
{
  private ExternWorkbookImpl m_externBook;
  private System.Collections.Generic.List<ExternNameImpl> m_hashNames = new System.Collections.Generic.List<ExternNameImpl>();
  private SortedList<int, object> m_lstToRemove = new SortedList<int, object>();

  public ExternNamesCollection(IApplication application, object parent)
    : base(application, parent)
  {
    this.SetParents();
    this.Removed += new CollectionBaseEx<ExternNameImpl>.CollectionChange(this.ExternNamesCollection_Removed);
    this.Inserted += new CollectionBaseEx<ExternNameImpl>.CollectionChange(this.ExternNamesCollection_Inserted);
  }

  private void SetParents()
  {
    this.m_externBook = this.FindParent(typeof (ExternWorkbookImpl)) as ExternWorkbookImpl;
    if (this.m_externBook == null)
      throw new ArgumentNullException("Can't find parent workbook.");
  }

  public new ExternNameImpl this[int index]
  {
    get
    {
      return index >= 0 && index <= this.Count ? this.List[index] : throw new ArgumentOutOfRangeException(nameof (index), "Value cannot be less than 0 and greater than Count");
    }
  }

  public ExternNameImpl this[string name]
  {
    get
    {
      int nameIndex = this.GetNameIndex(name);
      return nameIndex >= 0 && nameIndex <= this.Count ? this.List[nameIndex] : (ExternNameImpl) null;
    }
  }

  public ExternWorkbookImpl ParentWorkbook => this.m_externBook;

  [CLSCompliant(false)]
  public int Add(ExternNameRecord name)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    WorkbookImpl workbook = this.m_externBook.Workbook;
    ExternNameImpl externNameImpl = new ExternNameImpl(this.Application, (object) this, name, this.List.Count);
    base.Add(externNameImpl);
    if (!this.m_hashNames.Contains(externNameImpl))
      this.m_hashNames.Add(externNameImpl);
    return this.Count - 1;
  }

  public int Add(string name)
  {
    switch (name)
    {
      case null:
        throw new ArgumentNullException(nameof (name));
      case "":
        throw new ArgumentException("name - string cannot be empty");
      default:
        ExternNameRecord record = (ExternNameRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExternName);
        record.Name = name;
        return this.Add(record);
    }
  }

  public int Add(string name, bool isAddIn)
  {
    switch (name)
    {
      case null:
        throw new ArgumentNullException(nameof (name));
      case "":
        throw new ArgumentException("name - string cannot be empty");
      default:
        ExternNameRecord record = (ExternNameRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ExternName);
        record.Name = name;
        if (isAddIn)
          record.IsAddIn = true;
        return this.Add(record);
    }
  }

  public bool Contains(string name)
  {
    for (int index = 0; index < this.m_hashNames.Count; ++index)
    {
      if (this.m_hashNames[index].Name == name)
        return true;
    }
    return false;
  }

  [CLSCompliant(false)]
  public void Serialize(OffsetArrayList records)
  {
    if (records == null)
      throw new ArgumentNullException(nameof (records));
    int index = 0;
    for (int count = this.Count; index < count; ++index)
      this[index].Serialize(records);
  }

  public int GetNameIndex(string strName)
  {
    for (int index = 0; index < this.m_hashNames.Count; ++index)
    {
      if (this.m_hashNames[index].Name == strName)
        return this.m_hashNames[index].Index;
    }
    return -1;
  }

  public int GetNewIndex(int iNameIndex)
  {
    int num = this.m_lstToRemove.IndexOfKey(iNameIndex);
    return num != -1 ? iNameIndex - num - 1 : iNameIndex;
  }

  public override object Clone(object parent)
  {
    ExternNamesCollection externNamesCollection = (ExternNamesCollection) base.Clone(parent);
    IList<int> keys = this.m_lstToRemove.Keys;
    int index = 0;
    for (int count = this.m_lstToRemove.Count; index < count; ++index)
      this.m_lstToRemove.Add(keys[index], (object) null);
    return (object) externNamesCollection;
  }

  private int Add(ExternNameImpl name)
  {
    base.Add(name);
    return this.Count - 1;
  }

  private void ExternNamesCollection_Removed(
    object sender,
    CollectionChangeEventArgs<ExternNameImpl> args)
  {
    int index = args.Index;
    for (int count = this.Count; index < count; ++index)
      this[index].Index = index;
    ExternNameImpl externNameImpl = args.Value;
    if (externNameImpl.Record.NeedDataArray)
      return;
    this.m_hashNames.Remove(externNameImpl);
  }

  private void ExternNamesCollection_Inserted(
    object sender,
    CollectionChangeEventArgs<ExternNameImpl> args)
  {
    ExternNameImpl externNameImpl = args.Value;
    if (externNameImpl.Record.NeedDataArray)
      return;
    this.m_hashNames.Add(externNameImpl);
  }
}
