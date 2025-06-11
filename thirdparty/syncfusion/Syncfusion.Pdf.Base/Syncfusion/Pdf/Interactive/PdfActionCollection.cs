// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfActionCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfActionCollection : PdfCollection
{
  private PdfArray m_actions = new PdfArray();

  private PdfAction this[int index] => (PdfAction) this.List[index];

  public int Add(PdfAction action)
  {
    return action != null ? this.DoAdd(action) : throw new ArgumentNullException(nameof (action));
  }

  public void Insert(int index, PdfAction action)
  {
    if (action == null)
      throw new ArgumentNullException(nameof (action));
    this.DoInsert(index, action);
  }

  public int IndexOf(PdfAction action)
  {
    return action != null ? this.List.IndexOf((object) action) : throw new ArgumentNullException(nameof (action));
  }

  public bool Contains(PdfAction action)
  {
    return action != null ? this.List.Contains((object) action) : throw new ArgumentNullException(nameof (action));
  }

  public void Clear() => this.DoClear();

  public void Remove(PdfAction action)
  {
    if (action == null)
      throw new ArgumentNullException(nameof (action));
    this.DoRemove(action);
  }

  public void RemoveAt(int index) => this.DoRemoveAt(index);

  private int DoAdd(PdfAction action)
  {
    this.m_actions.Add((IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) action));
    this.List.Add((object) action);
    return this.List.Count - 1;
  }

  private void DoInsert(int index, PdfAction action)
  {
    this.m_actions.Insert(index, (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) action));
    this.List.Insert(index, (object) action);
  }

  private new void DoClear()
  {
    this.m_actions.Clear();
    this.List.Clear();
  }

  private void DoRemove(PdfAction action)
  {
    this.m_actions.RemoveAt(this.List.IndexOf((object) action));
    this.List.Remove((object) action);
  }

  private void DoRemoveAt(int index)
  {
    this.m_actions.RemoveAt(index);
    this.List.RemoveAt(index);
  }
}
