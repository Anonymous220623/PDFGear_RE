// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.WidthTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;
using System;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal abstract class WidthTable : ICloneable
{
  public abstract int this[int index] { get; }

  public abstract WidthTable Clone();

  object ICloneable.Clone() => (object) this.Clone();

  internal abstract PdfArray ToArray();
}
