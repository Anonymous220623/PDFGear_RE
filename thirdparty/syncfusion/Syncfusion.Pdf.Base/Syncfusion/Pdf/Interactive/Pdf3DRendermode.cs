// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.Pdf3DRendermode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class Pdf3DRendermode : IPdfWrapper
{
  private Pdf3DRenderStyle m_style;
  private PdfColor m_faceColor;
  private PdfColor m_auxilaryColor;
  private float m_opacity;
  private float m_creaseValue;
  private PdfDictionary m_dictionary = new PdfDictionary();

  public Pdf3DRenderStyle Style
  {
    get => this.m_style;
    set => this.m_style = value;
  }

  public PdfColor AuxilaryColor
  {
    get => this.m_auxilaryColor;
    set => this.m_auxilaryColor = value;
  }

  public PdfColor FaceColor
  {
    get => this.m_faceColor;
    set => this.m_faceColor = value;
  }

  public float CreaseValue
  {
    get => this.m_creaseValue;
    set => this.m_creaseValue = value;
  }

  public float Opacity
  {
    get => this.m_opacity;
    set => this.m_opacity = value;
  }

  internal PdfDictionary Dictionary => this.m_dictionary;

  public Pdf3DRendermode() => this.Initialize();

  public Pdf3DRendermode(Pdf3DRenderStyle style)
    : this()
  {
    this.m_style = style;
  }

  protected virtual void Initialize()
  {
    this.m_dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    this.m_dictionary.SetProperty("Type", (IPdfPrimitive) new PdfName("3DRenderMode"));
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars) => this.Save();

  protected virtual void Save()
  {
    this.Dictionary["Subtype"] = (IPdfPrimitive) new PdfName((Enum) this.m_style);
    PdfArray array1 = new PdfArray();
    array1.Insert(0, (IPdfPrimitive) new PdfName("DeviceRGB"));
    array1.Insert(1, (IPdfPrimitive) new PdfNumber((float) this.m_auxilaryColor.R / (float) byte.MaxValue));
    array1.Insert(2, (IPdfPrimitive) new PdfNumber((float) this.m_auxilaryColor.G / (float) byte.MaxValue));
    array1.Insert(3, (IPdfPrimitive) new PdfNumber((float) this.m_auxilaryColor.B / (float) byte.MaxValue));
    if (!this.m_auxilaryColor.IsEmpty)
      this.Dictionary["AC"] = (IPdfPrimitive) new PdfArray(array1);
    PdfArray array2 = new PdfArray();
    array2.Insert(0, (IPdfPrimitive) new PdfName("DeviceRGB"));
    array2.Insert(1, (IPdfPrimitive) new PdfNumber((float) this.m_faceColor.R / (float) byte.MaxValue));
    array2.Insert(2, (IPdfPrimitive) new PdfNumber((float) this.m_faceColor.G / (float) byte.MaxValue));
    array2.Insert(3, (IPdfPrimitive) new PdfNumber((float) this.m_faceColor.B / (float) byte.MaxValue));
    if (!this.m_faceColor.IsEmpty)
      this.Dictionary["FC"] = (IPdfPrimitive) new PdfArray(array2);
    if ((double) this.m_opacity != 0.0)
      this.Dictionary.SetProperty("O", (IPdfPrimitive) new PdfNumber(this.m_opacity));
    if ((double) this.m_creaseValue == 0.0)
      return;
    this.Dictionary.SetProperty("CV", (IPdfPrimitive) new PdfNumber(this.m_creaseValue));
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
