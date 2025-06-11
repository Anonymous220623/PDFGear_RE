// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ContentControlListItems
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class ContentControlListItems : IEnumerable
{
  private string m_lastValue;
  private List<ContentControlListItem> m_listItems = new List<ContentControlListItem>();

  internal string LastValue
  {
    get => this.m_lastValue;
    set => this.m_lastValue = value;
  }

  public ContentControlListItem this[int index]
  {
    get
    {
      if (this.m_listItems == null)
        this.m_listItems = new List<ContentControlListItem>();
      return this.m_listItems[index];
    }
  }

  public void Add(ContentControlListItem item)
  {
    if (this.m_listItems == null)
      return;
    this.m_listItems.Add(item);
  }

  public void Insert(int index, ContentControlListItem item)
  {
    if (this.m_listItems == null || index >= this.m_listItems.Count - 1)
      return;
    this.m_listItems.Insert(index, item);
  }

  public void Remove(ContentControlListItem item)
  {
    if (this.m_listItems == null)
      return;
    this.m_listItems.Remove(item);
  }

  public void RemoveAt(int index)
  {
    if (this.m_listItems == null)
      return;
    this.m_listItems.RemoveAt(index);
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.m_listItems.GetEnumerator();

  public int Count => this.m_listItems != null ? this.m_listItems.Count : 0;

  internal void Close()
  {
    if (this.m_listItems == null)
      return;
    this.m_listItems.Clear();
    this.m_listItems = (List<ContentControlListItem>) null;
  }

  internal ContentControlListItems Clone()
  {
    ContentControlListItems controlListItems = (ContentControlListItems) this.MemberwiseClone();
    controlListItems.m_listItems = new List<ContentControlListItem>();
    foreach (ContentControlListItem listItem in this.m_listItems)
      controlListItems.m_listItems.Add(listItem);
    return controlListItems;
  }
}
