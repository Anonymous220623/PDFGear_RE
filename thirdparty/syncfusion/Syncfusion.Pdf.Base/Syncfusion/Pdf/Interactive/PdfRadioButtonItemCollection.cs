// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfRadioButtonItemCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfRadioButtonItemCollection : PdfCollection, IPdfWrapper
{
  private PdfArray m_array = new PdfArray();
  private PdfRadioButtonListField m_field;

  public PdfRadioButtonItemCollection(PdfRadioButtonListField field) => this.m_field = field;

  public int Add(PdfRadioButtonListItem item)
  {
    return item != null ? this.DoAdd(item) : throw new ArgumentNullException(nameof (item));
  }

  public void Insert(int index, PdfRadioButtonListItem item)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    this.DoInsert(index, item);
  }

  public void Remove(PdfRadioButtonListItem item)
  {
    if (item == null)
      throw new ArgumentNullException(nameof (item));
    this.DoRemove(item);
  }

  public void RemoveAt(int index)
  {
    if (index < 0 || index >= this.List.Count)
      throw new ArgumentOutOfRangeException(nameof (index));
    PdfRadioButtonListItem radioButtonListItem = (PdfRadioButtonListItem) this.List[index];
    this.m_array.RemoveAt(index);
    this.List.RemoveAt(index);
  }

  public int IndexOf(PdfRadioButtonListItem item) => this.List.IndexOf((object) item);

  public bool Contains(PdfRadioButtonListItem item) => this.List.Contains((object) item);

  public void Clear() => this.DoClear();

  public PdfRadioButtonListItem this[int index] => (PdfRadioButtonListItem) this.List[index];

  private int DoAdd(PdfRadioButtonListItem item)
  {
    this.m_array.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) item));
    item.SetField(this.m_field);
    this.List.Add((object) item);
    return this.List.Count - 1;
  }

  private void DoInsert(int index, PdfRadioButtonListItem item)
  {
    this.m_array.Insert(index, (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) item));
    item.SetField(this.m_field);
    this.List.Insert(index, (object) item);
  }

  private void DoRemove(PdfRadioButtonListItem item)
  {
    if (!this.List.Contains((object) item))
      return;
    int index = this.List.IndexOf((object) item);
    this.m_array.RemoveAt(index);
    item.SetField((PdfRadioButtonListField) null);
    this.List.RemoveAt(index);
  }

  private new void DoClear()
  {
    foreach (PdfRadioButtonListItem radioButtonListItem in this.List)
      radioButtonListItem.SetField((PdfRadioButtonListField) null);
    this.m_array.Clear();
    this.List.Clear();
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_array;
}
