// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.Annotations.PdfBorderStyleModel
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Patagames.Pdf.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace pdfeditor.Models.Annotations;

public class PdfBorderStyleModel : IEquatable<PdfBorderStyleModel>
{
  public PdfBorderStyleModel(float width, BorderStyles style, float[] dashPattern)
  {
    this.Width = width;
    this.Style = style;
    float[] numArray = dashPattern != null ? ((IEnumerable<float>) dashPattern).ToArray<float>() : (float[]) null;
    if (numArray == null)
      numArray = new float[2]{ 2f, 4f };
    this.DashPattern = (IReadOnlyList<float>) numArray;
  }

  public float Width { get; }

  public BorderStyles Style { get; }

  public IReadOnlyList<float> DashPattern { get; }

  public bool Equals(PdfBorderStyleModel other)
  {
    return other != null && (double) this.Width == (double) other.Width && this.Style == other.Style && BaseAnnotation.CollectionEqual<float>(this.DashPattern, this.DashPattern);
  }
}
