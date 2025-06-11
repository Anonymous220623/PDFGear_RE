// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECx9Point
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ECx9Point : Asn1Encode
{
  private readonly EllipticPoint m_point;

  public ECx9Point(EllipticPoint point) => this.m_point = point;

  public ECx9Point(EllipticCurves curve, Asn1Octet sequence)
  {
    this.m_point = curve.GetDecodedECPoint(sequence.GetOctets());
  }

  public EllipticPoint Point => this.m_point;

  public override Asn1 GetAsn1() => (Asn1) new DerOctet(this.m_point.Encoded());
}
