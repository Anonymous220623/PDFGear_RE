// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Annotations.PdfBorderEffectModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Enums;
using System;

#nullable disable
namespace pdfeditor.Models.Annotations;

public class PdfBorderEffectModel : IEquatable<PdfBorderEffectModel>
{
  public PdfBorderEffectModel(BorderEffects effect, int intensity)
  {
    this.Effect = effect;
    this.Intensity = intensity;
  }

  public BorderEffects Effect { get; }

  public int Intensity { get; }

  public bool Equals(PdfBorderEffectModel other)
  {
    return other != null && this.Effect == other.Effect && this.Intensity == other.Intensity;
  }
}
