// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfStampCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfStampCollection : PdfCollection
{
  public PdfPageTemplateElement this[int index] => this.List[index] as PdfPageTemplateElement;

  public int Add(PdfPageTemplateElement template)
  {
    if (template == null)
      throw new ArgumentNullException(nameof (template));
    this.List.Add((object) template);
    return this.List.Count - 1;
  }

  public PdfPageTemplateElement Add(float x, float y, float width, float height)
  {
    PdfPageTemplateElement template = new PdfPageTemplateElement(x, y, width, height);
    this.Add(template);
    return template;
  }

  public bool Contains(PdfPageTemplateElement template)
  {
    return template != null ? this.List.Contains((object) template) : throw new ArgumentNullException(nameof (template));
  }

  public void Insert(int index, PdfPageTemplateElement template)
  {
    if (template == null)
      throw new ArgumentNullException(nameof (template));
    this.List.Insert(index, (object) template);
  }

  public void Remove(PdfPageTemplateElement template)
  {
    if (template == null)
      throw new ArgumentNullException(nameof (template));
    this.List.Remove((object) template);
  }

  public void RemoveAt(int index) => this.List.RemoveAt(index);

  public void Clear() => this.List.Clear();

  public new IEnumerator GetEnumerator()
  {
    return (IEnumerator) new PdfStampCollection.PdfPageTemplateEnumerator(this);
  }

  private struct PdfPageTemplateEnumerator : IEnumerator
  {
    private PdfStampCollection m_stamps;
    private int m_currentIndex;

    internal PdfPageTemplateEnumerator(PdfStampCollection stamps)
    {
      this.m_stamps = stamps != null ? stamps : throw new ArgumentNullException(nameof (stamps));
      this.m_currentIndex = -1;
    }

    public object Current
    {
      get
      {
        this.CheckIndex();
        return (object) this.m_stamps[this.m_currentIndex];
      }
    }

    public bool MoveNext()
    {
      ++this.m_currentIndex;
      return this.m_currentIndex < this.m_stamps.Count;
    }

    public void Reset() => this.m_currentIndex = -1;

    private void CheckIndex()
    {
      if (this.m_currentIndex < 0 || this.m_currentIndex >= this.m_stamps.Count)
        throw new IndexOutOfRangeException();
    }
  }
}
