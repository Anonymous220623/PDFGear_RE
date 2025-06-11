// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.ICCProfile
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class ICCProfile
{
  private int m_n;
  private Colorspace m_alternateColorspace;
  private PdfArray m_iccProfileValue;
  private PdfDictionary m_alternateColorspaceDictionary;

  internal Colorspace AlternateColorspace
  {
    get
    {
      if (this.m_alternateColorspace == null)
        this.m_alternateColorspace = this.GetAlternateColorSpace();
      return this.m_alternateColorspace;
    }
  }

  internal int N
  {
    get => this.m_n;
    set => this.m_n = value;
  }

  public ICCProfile()
  {
  }

  public ICCProfile(PdfArray array)
  {
    this.m_iccProfileValue = array;
    this.SetValue(array[1] as PdfReferenceHolder);
  }

  private Colorspace GetAlternateColorSpace()
  {
    switch (this.N)
    {
      case 1:
        return (Colorspace) new DeviceGray();
      case 3:
        return (Colorspace) new DeviceRGB();
      case 4:
        return (Colorspace) new DeviceCMYK();
      default:
        throw new NotSupportedException();
    }
  }

  private void SetValue(PdfReferenceHolder pdfReference)
  {
    if (!(pdfReference != (PdfReferenceHolder) null) || !(pdfReference.Object is PdfDictionary))
      return;
    this.m_alternateColorspaceDictionary = pdfReference.Object as PdfDictionary;
    if (!this.m_alternateColorspaceDictionary.ContainsKey("N"))
      return;
    this.m_n = (this.m_alternateColorspaceDictionary["N"] as PdfNumber).IntValue;
  }
}
