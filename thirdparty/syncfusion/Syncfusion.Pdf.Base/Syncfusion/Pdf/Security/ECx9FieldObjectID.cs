// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECx9FieldObjectID
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ECx9FieldObjectID : Asn1Encode
{
  private EllipticCurveElements m_field;

  public ECx9FieldObjectID(EllipticCurveElements field) => this.m_field = field;

  public ECx9FieldObjectID(Number point, Asn1Octet sequence)
    : this((EllipticCurveElements) new FinitePFieldObject(point, new Number(1, sequence.GetOctets())))
  {
  }

  public ECx9FieldObjectID(int num, int num1, int num2, int num3, Asn1Octet sequence)
    : this((EllipticCurveElements) new Finite2MFieldObject(num, num1, num2, num3, new Number(1, sequence.GetOctets())))
  {
  }

  public EllipticCurveElements Value => this.m_field;

  public override Asn1 GetAsn1()
  {
    int byteLength = ECConvertPoint.GetByteLength(this.m_field);
    return (Asn1) new DerOctet(ECConvertPoint.ConvetByte(this.m_field.ToIntValue(), byteLength));
  }
}
