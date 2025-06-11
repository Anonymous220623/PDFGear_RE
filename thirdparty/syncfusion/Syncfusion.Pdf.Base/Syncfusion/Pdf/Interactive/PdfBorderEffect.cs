// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.PdfBorderEffect
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class PdfBorderEffect : IPdfWrapper
{
  private PdfDictionary m_dictionary = new PdfDictionary();
  private PdfBorderEffectStyle m_style;
  private float m_intensity;

  public PdfBorderEffectStyle Style
  {
    get => this.m_style;
    set
    {
      this.m_style = value;
      this.Dictionary.SetProperty("S", (IPdfPrimitive) new PdfName(this.StyleToEffect(this.m_style)));
    }
  }

  public float Intensity
  {
    get => this.m_intensity;
    set
    {
      this.m_intensity = (double) value >= 0.0 && (double) value <= 2.0 ? value : throw new Exception("Intensity range only 0 to 2");
      this.Dictionary.SetNumber("I", this.m_intensity);
    }
  }

  internal PdfDictionary Dictionary => this.m_dictionary;

  public PdfBorderEffect() => this.Initialize();

  internal PdfBorderEffect(PdfDictionary dictionary)
  {
    if (!dictionary.ContainsKey("BE") || !(PdfCrossTable.Dereference(dictionary["BE"]) is PdfDictionary pdfDictionary))
      return;
    if (pdfDictionary.ContainsKey("I"))
    {
      IPdfPrimitive pdfPrimitive = pdfDictionary["I"];
      if (pdfPrimitive is PdfNumber)
        this.m_intensity = (pdfPrimitive as PdfNumber).FloatValue;
    }
    if (!pdfDictionary.ContainsKey("S"))
      return;
    IPdfPrimitive pdfPrimitive1 = pdfDictionary["S"];
    if (pdfPrimitive1.ToString() == null)
      return;
    this.m_style = this.GetBorderEffect(pdfPrimitive1.ToString());
  }

  protected void Initialize()
  {
    this.Dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars)
  {
    this.Dictionary.SetProperty("S", (IPdfPrimitive) new PdfName(this.StyleToEffect(this.m_style)));
    this.Dictionary.SetNumber("I", this.m_intensity);
  }

  private string StyleToEffect(PdfBorderEffectStyle effect)
  {
    return effect == PdfBorderEffectStyle.Cloudy ? "C" : "S";
  }

  private PdfBorderEffectStyle GetBorderEffect(string beffect)
  {
    return beffect == "/C" ? PdfBorderEffectStyle.Cloudy : PdfBorderEffectStyle.Solid;
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
