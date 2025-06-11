// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IO.Pair
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Primitives;

#nullable disable
namespace Syncfusion.Pdf.IO;

internal struct Pair(PdfName name, IPdfPrimitive value)
{
  public static readonly Pair Empty = new Pair((PdfName) null, (IPdfPrimitive) null);
  public PdfName Name = name;
  public IPdfPrimitive Value = value;

  public static bool operator ==(Pair pair, object obj)
  {
    return obj != null && obj is Pair pair1 && pair1.Name == pair.Name && pair.Value == pair1.Value;
  }

  public static bool operator !=(Pair pair, object obj) => !(pair == obj);

  public override bool Equals(object obj) => this == obj;

  public override int GetHashCode() => base.GetHashCode();
}
