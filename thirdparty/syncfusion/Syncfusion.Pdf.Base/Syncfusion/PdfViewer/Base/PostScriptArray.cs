// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.PostScriptArray
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class PostScriptArray : 
  PostScriptObj,
  IList<object>,
  ICollection<object>,
  IEnumerable<object>,
  IEnumerable
{
  private readonly List<object> store;

  public static PostScriptArray MatrixIdentity
  {
    get
    {
      return new PostScriptArray(new object[6]
      {
        (object) 1,
        (object) 0,
        (object) 0,
        (object) 1,
        (object) 0,
        (object) 0
      });
    }
  }

  public object this[int index]
  {
    get => this.store[index];
    set => this.store[index] = value;
  }

  public int Count => this.store.Count;

  public bool IsReadOnly => false;

  public PostScriptArray() => this.store = new List<object>();

  public PostScriptArray(int capacity)
  {
    this.store = new List<object>(capacity);
    for (int index = 0; index < capacity; ++index)
      this.store.Add((object) null);
  }

  public PostScriptArray(object[] initialValue)
  {
    this.store = new List<object>((IEnumerable<object>) initialValue);
  }

  public Matrix ToMatrix()
  {
    double res1;
    Helper.ParseReal(this.store[0], out res1);
    double res2;
    Helper.ParseReal(this.store[1], out res2);
    double res3;
    Helper.ParseReal(this.store[2], out res3);
    double res4;
    Helper.ParseReal(this.store[3], out res4);
    double res5;
    Helper.ParseReal(this.store[4], out res5);
    double res6;
    Helper.ParseReal(this.store[5], out res6);
    return new Matrix(res1, res2, res3, res4, res5, res6);
  }

  public int IndexOf(object item) => this.store.IndexOf(item);

  public void Insert(int index, object item) => this.store.Insert(index, item);

  public void RemoveAt(int index) => this.store.RemoveAt(index);

  public void Add(object item) => this.store.Add(item);

  public void Clear() => this.store.Clear();

  public bool Contains(object item) => this.store.Contains(item);

  public void CopyTo(object[] array, int arrayIndex) => this.store.CopyTo(array, arrayIndex);

  public bool Remove(object item) => this.store.Remove(item);

  public IEnumerator<object> GetEnumerator() => (IEnumerator<object>) this.store.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.store.GetEnumerator();

  public void Load(object[] content)
  {
    for (int index = 0; index < content.Length; ++index)
      this.store.Add(content[index]);
  }

  public T GetElementAs<T>(int index) => (T) this.store[index];

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    foreach (object obj in this.store)
    {
      stringBuilder.Append(" ");
      stringBuilder.Append(obj);
    }
    stringBuilder.Remove(0, 1);
    stringBuilder.Append("]");
    stringBuilder.Insert(0, "[");
    return stringBuilder.ToString();
  }
}
