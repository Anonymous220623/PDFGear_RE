// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfNamedDestination
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfNamedDestination : IPdfWrapper
{
  private PdfDestination m_destination;
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfCrossTable m_crossTable = new PdfCrossTable();

  public PdfNamedDestination(string title)
  {
    this.Title = title != null ? title : throw new ArgumentNullException("The title can't be null");
    this.Initialize();
  }

  internal PdfNamedDestination(PdfDictionary dictionary, PdfCrossTable crossTable)
  {
    this.m_dictionary = dictionary;
    this.m_crossTable = crossTable;
  }

  public virtual PdfDestination Destination
  {
    get => this.m_destination;
    set
    {
      if (value == null)
        throw new ArgumentNullException("The destination value can't be null");
      if (value == this.m_destination)
        return;
      this.m_destination = value;
      this.Dictionary.SetProperty("D", (IPdfWrapper) this.m_destination);
    }
  }

  public virtual string Title
  {
    get
    {
      PdfString pdfString = this.Dictionary[nameof (Title)] as PdfString;
      string title = (string) null;
      if (pdfString != null)
        title = pdfString.Value;
      return title;
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException("The title can't be null");
      this.Dictionary.SetString(nameof (Title), value);
    }
  }

  internal PdfDictionary Dictionary => this.m_dictionary;

  internal PdfCrossTable CrossTable => this.m_crossTable;

  internal void Initialize()
  {
    this.Dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    this.Dictionary.SetProperty("S", (IPdfPrimitive) new PdfName("GoTo"));
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    this.Dictionary.SetProperty("D", (IPdfWrapper) this.m_destination);
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
