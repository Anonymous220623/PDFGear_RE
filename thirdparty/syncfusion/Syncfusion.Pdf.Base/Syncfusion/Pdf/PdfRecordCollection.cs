// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PdfRecordCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

internal class PdfRecordCollection : IEnumerable
{
  private List<PdfRecord> m_recordCollection;

  internal List<PdfRecord> RecordCollection
  {
    get => this.m_recordCollection;
    set => this.m_recordCollection = value;
  }

  internal PdfRecordCollection() => this.m_recordCollection = new List<PdfRecord>();

  public void Add(PdfRecord record) => this.m_recordCollection.Add(record);

  internal void Remove(PdfRecord record) => this.m_recordCollection.Remove(record);

  public IEnumerator GetEnumerator() => (IEnumerator) this.m_recordCollection.GetEnumerator();
}
