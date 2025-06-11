// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.EmbeddedFileParams
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class EmbeddedFileParams : IPdfWrapper
{
  private DateTime m_creationDate = DateTime.Now;
  private DateTime m_modificationDate = DateTime.Now;
  private int m_size;
  private PdfDictionary m_dictionary = new PdfDictionary();

  public EmbeddedFileParams()
  {
    this.CreationDate = DateTime.Now;
    this.ModificationDate = DateTime.Now;
  }

  public DateTime CreationDate
  {
    get => this.m_creationDate;
    set
    {
      this.m_creationDate = value;
      this.m_dictionary.SetDateTime(nameof (CreationDate), value);
    }
  }

  public DateTime ModificationDate
  {
    get => this.m_modificationDate;
    set
    {
      this.m_modificationDate = value;
      this.m_dictionary.SetDateTime("ModDate", value);
    }
  }

  internal int Size
  {
    get => this.m_size;
    set
    {
      if (this.m_size == value)
        return;
      this.m_size = value;
      this.m_dictionary.SetNumber(nameof (Size), this.m_size);
    }
  }

  public IPdfPrimitive Element => (IPdfPrimitive) this.m_dictionary;
}
