// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.EllipticTNMuliplier
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class EllipticTNMuliplier : EllipticMultiplier
{
  public EllipticPoint Multiply(EllipticPoint pointP, Number number, EllipticComp preInfo)
  {
    Finite2MPoint pointP1 = pointP is Finite2MPoint ? (Finite2MPoint) pointP : throw new ArgumentException("Finite2MPoint");
    Field2MCurves curve = (Field2MCurves) pointP1.Curve;
    int pointM = curve.PointM;
    sbyte intValue = (sbyte) curve.ElementA.ToIntValue().IntValue;
    sbyte num = curve.MU();
    Number[] numberS = curve.SI();
    ITanuElement lambdaValue = ECTanFunction.MODFun(number, pointM, intValue, numberS, num, (sbyte) 10);
    return (EllipticPoint) this.MultiplyValue(pointP1, lambdaValue, preInfo, intValue, num);
  }

  private Finite2MPoint MultiplyValue(
    Finite2MPoint pointP,
    ITanuElement lambdaValue,
    EllipticComp preInfo,
    sbyte bitA,
    sbyte pointMU)
  {
    ITanuElement[] alpha = bitA != (sbyte) 0 ? ECTanFunction.AlphaOne : ECTanFunction.AlphaZeo;
    Number tw = ECTanFunction.FindTW(pointMU, 4);
    sbyte[] tadic = ECTanFunction.GetTAdic(pointMU, lambdaValue, (sbyte) 4, Number.ValueOf(16L /*0x10*/), tw, alpha);
    return EllipticTNMuliplier.MultiplyTFunction(pointP, tadic, preInfo);
  }

  private static Finite2MPoint MultiplyTFunction(
    Finite2MPoint pointP,
    sbyte[] byteU,
    EllipticComp preInfo)
  {
    sbyte intValue = (sbyte) pointP.Curve.ElementA.ToIntValue().IntValue;
    Finite2MPoint[] comp;
    if (preInfo == null || !(preInfo is FiniteCompField))
    {
      comp = ECTanFunction.FindComp(pointP, intValue);
      pointP.SetInfo((EllipticComp) new FiniteCompField(comp));
    }
    else
      comp = ((FiniteCompField) preInfo).FindComp();
    Finite2MPoint pointP1 = (Finite2MPoint) pointP.Curve.IsInfinity;
    for (int index = byteU.Length - 1; index >= 0; --index)
    {
      pointP1 = ECTanFunction.GetTanU(pointP1);
      if (byteU[index] != (sbyte) 0)
        pointP1 = byteU[index] <= (sbyte) 0 ? pointP1.SubtractSimple(comp[(int) -byteU[index]]) : pointP1.AddSimple(comp[(int) byteU[index]]);
    }
    return pointP1;
  }
}
