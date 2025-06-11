// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfSectionPageCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf;

public class PdfSectionPageCollection : IEnumerable
{
  private PdfSection m_section;

  public PdfPage this[int index]
  {
    get
    {
      return index >= 0 || index <= this.Count ? this.m_section[index] : throw new ArgumentOutOfRangeException(nameof (index));
    }
  }

  public int Count => this.m_section.Count;

  private PdfSectionPageCollection()
  {
  }

  internal PdfSectionPageCollection(PdfSection section)
  {
    this.m_section = section != null ? section : throw new ArgumentNullException(nameof (section));
  }

  public PdfPage Add() => this.m_section.Add();

  public void Add(PdfPage page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    this.m_section.Add(page);
  }

  public void Insert(int index, PdfPage page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    if (index < 0 && index > this.Count)
      throw new ArgumentOutOfRangeException(nameof (index));
    this.m_section.Insert(index, page);
  }

  public int IndexOf(PdfPage page)
  {
    return page != null ? this.m_section.IndexOf(page) : throw new ArgumentNullException(nameof (page));
  }

  public bool Contains(PdfPage page)
  {
    return page != null ? this.m_section.Contains(page) : throw new ArgumentNullException(nameof (page));
  }

  public void Remove(PdfPage page)
  {
    if (page == null)
      throw new ArgumentNullException(nameof (page));
    this.m_section.Remove(page);
  }

  public void RemoveAt(int index)
  {
    if (index < 0 && index > this.Count)
      throw new ArgumentOutOfRangeException(nameof (index));
    this.m_section.RemoveAt(index);
  }

  public void Clear() => this.m_section = (PdfSection) null;

  IEnumerator IEnumerable.GetEnumerator() => this.m_section.GetEnumerator();
}
