// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.XFORM
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Native;

internal struct XFORM
{
  public float eM11;
  public float eM12;
  public float eM21;
  public float eM22;
  public float eDx;
  public float eDy;

  public override string ToString()
  {
    return $"{(object) this.eM11} {(object) this.eM12} {(object) this.eM21} {(object) this.eM22} {(object) this.eDx} {(object) this.eDy}";
  }
}
