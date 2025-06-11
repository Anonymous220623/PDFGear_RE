// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECPointBase
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal abstract class ECPointBase : EllipticPoint
{
  protected internal ECPointBase(
    EllipticCurves curve,
    EllipticCurveElements pointX,
    EllipticCurveElements pointY,
    bool isCompress)
    : base(curve, pointX, pointY, isCompress)
  {
  }

  protected internal abstract bool BasePoint { get; }

  internal override byte[] Encoded(bool compressed)
  {
    if (this.IsInfinity)
      return new byte[1];
    int byteLength = ECConvertPoint.GetByteLength(this.pointX);
    byte[] numArray1 = ECConvertPoint.ConvetByte(this.PointX.ToIntValue(), byteLength);
    byte[] numArray2;
    if (compressed)
    {
      numArray2 = new byte[1 + numArray1.Length];
      numArray2[0] = this.BasePoint ? (byte) 3 : (byte) 2;
    }
    else
    {
      byte[] numArray3 = ECConvertPoint.ConvetByte(this.PointY.ToIntValue(), byteLength);
      numArray2 = new byte[1 + numArray1.Length + numArray3.Length];
      numArray2[0] = (byte) 4;
      numArray3.CopyTo((Array) numArray2, 1 + numArray1.Length);
    }
    numArray1.CopyTo((Array) numArray2, 1);
    return numArray2;
  }

  internal override EllipticPoint Multiply(Number number)
  {
    if (number.SignValue < 0)
      throw new ArgumentException("number cannot be negative");
    if (this.IsInfinity)
      return (EllipticPoint) this;
    if (number.SignValue == 0)
      return this.curve.IsInfinity;
    this.CheckMultiplier();
    return this.multiplier.Multiply((EllipticPoint) this, number, this.preInfo);
  }
}
