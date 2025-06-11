// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.EllipticCurves
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal abstract class EllipticCurves
{
  internal EllipticCurveElements elementA;
  internal EllipticCurveElements elementB;

  public abstract int Size { get; }

  public abstract EllipticCurveElements ECNumber(Number number);

  public abstract EllipticPoint ECPoints(Number pointX, Number pointY, bool isCompress);

  public abstract EllipticPoint IsInfinity { get; }

  public EllipticCurveElements ElementA => this.elementA;

  public EllipticCurveElements ElementB => this.elementB;

  public override bool Equals(object element)
  {
    if (element == this)
      return true;
    return element is EllipticCurves element1 && this.Equals(element1);
  }

  protected bool Equals(EllipticCurves element)
  {
    return this.elementA.Equals((object) element.elementA) && this.elementB.Equals((object) element.elementB);
  }

  public override int GetHashCode() => this.elementA.GetHashCode() ^ this.elementB.GetHashCode();

  protected abstract EllipticPoint GetDecompressECPoint(int point, Number number);

  public virtual EllipticPoint GetDecodedECPoint(byte[] encodedPoints)
  {
    int length = (this.Size + 7) / 8;
    switch (encodedPoints[0])
    {
      case 0:
        if (encodedPoints.Length != 1)
          throw new ArgumentException("Invalid range for encodedPoints");
        return this.IsInfinity;
      case 2:
      case 3:
        if (encodedPoints.Length != length + 1)
          throw new ArgumentException("Invalid range for compressed encodedPoints");
        return this.GetDecompressECPoint((int) encodedPoints[0] & 1, new Number(1, encodedPoints, 1, length));
      case 4:
      case 6:
      case 7:
        if (encodedPoints.Length != 2 * length + 1)
          throw new ArgumentException("Invalid range for uncompressed encodedPoints");
        return this.ECPoints(new Number(1, encodedPoints, 1, length), new Number(1, encodedPoints, 1 + length, length), false);
      default:
        throw new FormatException("Invalid encoding " + (object) encodedPoints[0]);
    }
  }
}
