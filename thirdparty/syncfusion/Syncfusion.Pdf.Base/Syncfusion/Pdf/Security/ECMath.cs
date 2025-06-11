// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECMath
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ECMath
{
  internal static EllipticPoint AddCurve(
    EllipticPoint PCurve,
    Number number,
    EllipticPoint QCurve,
    Number number1)
  {
    EllipticCurves curve = PCurve.Curve;
    if (!curve.Equals((object) QCurve.Curve))
      throw new ArgumentException("PCurve and QCurve must be on same curve");
    return curve is Field2MCurves && ((Field2MCurves) curve).IsKOBLITZ ? PCurve.Multiply(number).SumValue(QCurve.Multiply(number1)) : ECMath.BlockFunction(PCurve, number, QCurve, number1);
  }

  private static EllipticPoint BlockFunction(
    EllipticPoint PCurve,
    Number number1,
    EllipticPoint QCurve,
    Number number2)
  {
    int num = Math.Max(number1.BitLength, number2.BitLength);
    EllipticPoint ellipticPoint1 = PCurve.SumValue(QCurve);
    EllipticPoint ellipticPoint2 = PCurve.Curve.IsInfinity;
    for (int index = num - 1; index >= 0; --index)
    {
      ellipticPoint2 = ellipticPoint2.Twice();
      if (number1.TestBit(index))
        ellipticPoint2 = !number2.TestBit(index) ? ellipticPoint2.SumValue(PCurve) : ellipticPoint2.SumValue(ellipticPoint1);
      else if (number2.TestBit(index))
        ellipticPoint2 = ellipticPoint2.SumValue(QCurve);
    }
    return ellipticPoint2;
  }
}
