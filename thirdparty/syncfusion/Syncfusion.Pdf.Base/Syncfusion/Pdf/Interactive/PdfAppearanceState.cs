// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfAppearanceState
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfAppearanceState : IPdfWrapper
{
  private PdfTemplate m_on;
  private PdfTemplate m_off;
  private PdfDictionary m_dictionary = new PdfDictionary();
  private string m_onMappingName = "Yes";
  private string m_offMappingName = nameof (Off);

  public PdfTemplate On
  {
    get => this.m_on;
    set
    {
      if (this.m_on == value)
        return;
      this.m_on = value;
    }
  }

  public PdfTemplate Off
  {
    get => this.m_off;
    set
    {
      if (this.m_off == value)
        return;
      this.m_off = value;
    }
  }

  public string OnMappingName
  {
    get => this.m_onMappingName;
    set
    {
      this.m_onMappingName = value != null ? value : throw new ArgumentNullException(nameof (OnMappingName));
    }
  }

  public string OffMappingName
  {
    get => this.m_offMappingName;
    set
    {
      this.m_offMappingName = value != null ? value : throw new ArgumentNullException(nameof (OffMappingName));
    }
  }

  public PdfAppearanceState()
  {
    this.m_dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    if (this.m_on != null)
      this.m_dictionary[PdfName.EncodeName(this.m_onMappingName)] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_on);
    if (this.m_off == null)
      return;
    this.m_dictionary[PdfName.EncodeName(this.m_offMappingName)] = (IPdfPrimitive) new PdfReferenceHolder((IPdfWrapper) this.m_off);
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
