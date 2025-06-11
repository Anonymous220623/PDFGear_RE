// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfCollection : IEnumerable
{
  private System.Collections.Generic.List<object> m_list;

  public PdfCollection() => this.m_list = new System.Collections.Generic.List<object>();

  public int Count => this.m_list.Count;

  protected System.Collections.Generic.List<object> List => this.m_list;

  internal void CopyTo(IPdfWrapper[] array, int index)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (index < 0)
      throw new ArgumentOutOfRangeException(nameof (index));
    this.m_list.CopyTo((object[]) array, index);
  }

  public IEnumerator GetEnumerator() => (IEnumerator) this.m_list.GetEnumerator();

  internal void DoClear() => this.List.Clear();
}
