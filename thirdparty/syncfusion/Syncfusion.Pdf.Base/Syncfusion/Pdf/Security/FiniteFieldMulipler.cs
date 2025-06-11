// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.FiniteFieldMulipler
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class FiniteFieldMulipler : EllipticMultiplier
{
  public EllipticPoint Multiply(EllipticPoint pointP, Number number, EllipticComp preInfo)
  {
    Number number1 = number;
    Number number2 = number1.Multiply(Number.Three);
    EllipticPoint ellipticPoint1 = pointP.Negate();
    EllipticPoint ellipticPoint2 = pointP;
    for (int index = number2.BitLength - 2; index > 0; --index)
    {
      ellipticPoint2 = ellipticPoint2.Twice();
      bool flag1 = number2.TestBit(index);
      bool flag2 = number1.TestBit(index);
      if (flag1 != flag2)
        ellipticPoint2 = ellipticPoint2.SumValue(flag1 ? pointP : ellipticPoint1);
    }
    return ellipticPoint2;
  }
}
