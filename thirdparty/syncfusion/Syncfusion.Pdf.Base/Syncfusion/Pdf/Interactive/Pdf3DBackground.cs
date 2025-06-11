// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.Pdf3DBackground
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class Pdf3DBackground : IPdfWrapper
{
  private const float MaxColourChannelValue = 255f;
  private PdfColor m_backgroundColor;
  private bool m_applyEntire;
  private PdfDictionary m_dictionary = new PdfDictionary();

  public PdfColor Color
  {
    get => this.m_backgroundColor;
    set => this.m_backgroundColor = value;
  }

  public bool ApplyToEntireAnnotation
  {
    get => this.m_applyEntire;
    set => this.m_applyEntire = value;
  }

  internal PdfDictionary Dictionary => this.m_dictionary;

  public Pdf3DBackground() => this.Initialize();

  public Pdf3DBackground(PdfColor color)
    : this()
  {
    this.m_backgroundColor = color;
  }

  protected virtual void Initialize()
  {
    this.m_dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    this.m_dictionary.SetProperty("Type", (IPdfPrimitive) new PdfName("3DBG"));
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars) => this.Save();

  protected virtual void Save()
  {
    this.Dictionary["Subtype"] = (IPdfPrimitive) new PdfName("SC");
    this.Dictionary.SetProperty("CS", (IPdfPrimitive) new PdfName("DeviceRGB"));
    this.Dictionary.SetProperty("EA", (IPdfPrimitive) new PdfBoolean(this.m_applyEntire));
    PdfArray array = new PdfArray();
    array.Insert(0, (IPdfPrimitive) new PdfNumber((float) this.m_backgroundColor.R / (float) byte.MaxValue));
    array.Insert(1, (IPdfPrimitive) new PdfNumber((float) this.m_backgroundColor.G / (float) byte.MaxValue));
    array.Insert(2, (IPdfPrimitive) new PdfNumber((float) this.m_backgroundColor.B / (float) byte.MaxValue));
    this.Dictionary["C"] = (IPdfPrimitive) new PdfArray(array);
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
