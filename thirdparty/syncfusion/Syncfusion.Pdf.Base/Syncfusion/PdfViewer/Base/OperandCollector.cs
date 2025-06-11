// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.OperandCollector
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class OperandCollector
{
  private readonly LinkedList<object> store;

  public int Count => this.store.Count;

  public object First => Countable.First<object>((IEnumerable<object>) this.store);

  public object Last => Countable.Last<object>((IEnumerable<object>) this.store);

  public OperandCollector() => this.store = new LinkedList<object>();

  public object GetElementAt(Origin origin, int index)
  {
    if (origin == Origin.Begin)
    {
      LinkedListNode<object> linkedListNode = this.store.First;
      for (int index1 = 0; index1 < index; ++index1)
        linkedListNode = linkedListNode.Next;
      return linkedListNode.Value;
    }
    LinkedListNode<object> linkedListNode1 = this.store.Last;
    for (int index2 = 0; index2 < index; ++index2)
      linkedListNode1 = linkedListNode1.Previous;
    return linkedListNode1.Value;
  }

  public void AddLast(object obj) => this.store.AddLast(obj);

  public void AddFirst(object obj) => this.store.AddFirst(obj);

  public object GetLast()
  {
    object last = this.store.Last.Value;
    this.store.RemoveLast();
    return last;
  }

  public T GetLastAs<T>() => (T) this.GetLast();

  public int GetLastAsInt()
  {
    int res;
    Helper.ParseInteger(this.GetLast(), out res);
    return res;
  }

  public double GetLastAsReal()
  {
    double res;
    Helper.ParseReal(this.GetLast(), out res);
    return res;
  }

  public object GetFirst()
  {
    object first = this.store.First.Value;
    this.store.RemoveFirst();
    return first;
  }

  public T GetFirstAs<T>() => (T) this.GetFirst();

  public int GetFirstAsInt()
  {
    int res;
    Helper.ParseInteger(this.GetFirst(), out res);
    return res;
  }

  public double GetFirstAsReal()
  {
    double res;
    Helper.ParseReal(this.GetFirst(), out res);
    return res;
  }

  public void Clear() => this.store.Clear();
}
