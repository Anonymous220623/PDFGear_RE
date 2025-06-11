// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.OffsetArrayList
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[CLSCompliant(false)]
public class OffsetArrayList : 
  IList,
  IList<IBiffStorage>,
  ICollection<IBiffStorage>,
  IEnumerable<IBiffStorage>,
  ICollection,
  IEnumerable
{
  private List<IBiffStorage> m_list = new List<IBiffStorage>();

  public bool IsFixedSize => ((IList) this.m_list).IsFixedSize;

  public bool IsReadOnly => ((IList) this.m_list).IsReadOnly;

  public IBiffStorage this[int index]
  {
    get => this.m_list[index];
    set => this.m_list[index] = value;
  }

  object IList.this[int index]
  {
    get => (object) this.m_list[index];
    set
    {
      if (!(value is IBiffStorage biffStorage))
        return;
      this.m_list[index] = biffStorage;
    }
  }

  public void RemoveAt(int index) => this.m_list.RemoveAt(index);

  public void Insert(int index, IBiffStorage value) => this.m_list.Insert(index, value);

  public bool Remove(IBiffStorage value)
  {
    int index = this.m_list.IndexOf(value);
    if (index >= 0)
      this.m_list.RemoveAt(index);
    return index >= 0;
  }

  public bool Contains(IBiffStorage value) => this.m_list.Contains(value);

  public void Clear() => this.m_list.Clear();

  public int IndexOf(IBiffStorage value) => this.m_list.IndexOf(value);

  public int Add(IBiffStorage value)
  {
    this.m_list.Add(value);
    return this.m_list.Count - 1;
  }

  void ICollection<IBiffStorage>.Add(IBiffStorage value) => this.m_list.Add(value);

  public void AddList(IList value)
  {
    int index = 0;
    for (int count = value.Count; index < count; ++index)
      this.Add(value[index] as IBiffStorage);
  }

  public void AddRange(ICollection value)
  {
    foreach (IBiffStorage biffStorage in (IEnumerable) value)
      this.Add(biffStorage);
  }

  public void AddRange(ICollection<IBiffStorage> value)
  {
    this.m_list.AddRange((IEnumerable<IBiffStorage>) value);
  }

  public void Insert(int index, object value)
  {
    if (!(value is BiffRecordRaw biffRecordRaw))
      return;
    this.Insert(index, (IBiffStorage) biffRecordRaw);
  }

  public void Remove(object value) => this.Remove((IBiffStorage) (value as BiffRecordRaw));

  public bool Contains(object value)
  {
    return value is BiffRecordRaw biffRecordRaw && this.Contains((IBiffStorage) biffRecordRaw);
  }

  public int IndexOf(object value)
  {
    return !(value is BiffRecordRaw biffRecordRaw) ? -1 : this.IndexOf((IBiffStorage) biffRecordRaw);
  }

  public int Add(object value)
  {
    return !(value is BiffRecordRaw biffRecordRaw) ? -1 : this.Add((IBiffStorage) biffRecordRaw);
  }

  public bool IsSynchronized => ((ICollection) this.m_list).IsSynchronized;

  public int Count => this.m_list.Count;

  public void CopyTo(Array array, int index) => ((ICollection) this.m_list).CopyTo(array, index);

  public object SyncRoot => ((ICollection) this.m_list).SyncRoot;

  public IEnumerator GetEnumerator() => (IEnumerator) this.m_list.GetEnumerator();

  IEnumerator<IBiffStorage> IEnumerable<IBiffStorage>.GetEnumerator()
  {
    return (IEnumerator<IBiffStorage>) this.m_list.GetEnumerator();
  }

  public void UpdateBiffRecordsOffsets() => this.CalculateRecordsStreamPos();

  protected void CalculateRecordsStreamPos()
  {
    int num = 0;
    int index = 0;
    for (int count = this.m_list.Count; index < count; ++index)
    {
      IBiffStorage biffStorage = this.m_list[index];
      if (biffStorage != null)
      {
        biffStorage.StreamPos = (long) num;
        num += 4 + biffStorage.GetStoreSize(ExcelVersion.Excel97to2003);
      }
    }
  }

  public void CopyTo(IBiffStorage[] array, int arrayIndex) => this.m_list.CopyTo(array, arrayIndex);
}
