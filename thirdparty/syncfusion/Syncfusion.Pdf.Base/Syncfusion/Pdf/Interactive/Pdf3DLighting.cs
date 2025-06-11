// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.Pdf3DLighting
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class Pdf3DLighting : IPdfWrapper
{
  private Pdf3DLightingStyle m_lightingStyle;
  private PdfDictionary m_dictionary = new PdfDictionary();

  public Pdf3DLightingStyle Style
  {
    get => this.m_lightingStyle;
    set => this.m_lightingStyle = value;
  }

  internal PdfDictionary Dictionary => this.m_dictionary;

  public Pdf3DLighting() => this.Initialize();

  public Pdf3DLighting(Pdf3DLightingStyle style)
    : this()
  {
    this.m_lightingStyle = style;
  }

  protected virtual void Initialize()
  {
    this.m_dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    this.m_dictionary.SetProperty("Type", (IPdfPrimitive) new PdfName("3DLightingScheme"));
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars) => this.Save();

  protected virtual void Save()
  {
    this.Dictionary.SetProperty("Subtype", (IPdfPrimitive) new PdfName((Enum) this.m_lightingStyle));
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
