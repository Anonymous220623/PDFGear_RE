// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Interactive.Pdf3DAnimation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Interactive;

public class Pdf3DAnimation : IPdfWrapper
{
  private PDF3DAnimationType m_type;
  private int m_playCount;
  private float m_timeMultiplier;
  private PdfDictionary m_dictionary = new PdfDictionary();

  public PDF3DAnimationType Type
  {
    get => this.m_type;
    set => this.m_type = value;
  }

  public int PlayCount
  {
    get => this.m_playCount;
    set => this.m_playCount = value;
  }

  public float TimeMultiplier
  {
    get => this.m_timeMultiplier;
    set => this.m_timeMultiplier = value;
  }

  internal PdfDictionary Dictionary => this.m_dictionary;

  public Pdf3DAnimation() => this.Initialize();

  public Pdf3DAnimation(PDF3DAnimationType type)
    : this()
  {
    this.m_type = type;
  }

  protected virtual void Initialize()
  {
    this.m_dictionary.BeginSave += new SavePdfPrimitiveEventHandler(this.Dictionary_BeginSave);
    this.m_dictionary.SetProperty("Type", (IPdfPrimitive) new PdfName("3DAnimationStyle"));
  }

  private void Dictionary_BeginSave(object sender, SavePdfPrimitiveEventArgs ars) => this.Save();

  protected virtual void Save()
  {
    this.Dictionary["Subtype"] = (IPdfPrimitive) new PdfName((Enum) this.m_type);
    this.Dictionary.SetProperty("PC", (IPdfPrimitive) new PdfNumber(this.m_playCount));
    this.Dictionary.SetProperty("TM", (IPdfPrimitive) new PdfNumber(this.m_timeMultiplier));
  }

  IPdfPrimitive IPdfWrapper.Element => (IPdfPrimitive) this.m_dictionary;
}
