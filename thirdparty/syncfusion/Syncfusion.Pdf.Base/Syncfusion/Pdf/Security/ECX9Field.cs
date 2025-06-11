// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECX9Field
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ECX9Field : Asn1Encode
{
  private ECx9FieldObject m_fieldID;
  private EllipticCurves m_curve;
  private EllipticPoint m_point;
  private Number m_num1;
  private Number m_num2;
  private byte[] m_seed;

  public ECX9Field(Asn1Sequence sequence)
  {
    if (!(sequence[0] is DerInteger) || !((DerInteger) sequence[0]).Value.Equals((object) Number.One))
      throw new ArgumentException("bad version in ECX9Field");
    ECx9Curve ecx9Curve = !(sequence[2] is ECx9Curve) ? new ECx9Curve(new ECx9FieldObject((Asn1Sequence) sequence[1]), (Asn1Sequence) sequence[2]) : (ECx9Curve) sequence[2];
    this.m_curve = ecx9Curve.Curve;
    this.m_point = !(sequence[3] is ECx9Point) ? new ECx9Point(this.m_curve, (Asn1Octet) sequence[3]).Point : ((ECx9Point) sequence[3]).Point;
    this.m_num1 = ((DerInteger) sequence[4]).Value;
    this.m_seed = ecx9Curve.GetSeed();
    if (sequence.Count != 6)
      return;
    this.m_num2 = ((DerInteger) sequence[5]).Value;
  }

  public ECX9Field(EllipticCurves curve, EllipticPoint point, Number num, Number num1)
    : this(curve, point, num, num1, (byte[]) null)
  {
  }

  public ECX9Field(
    EllipticCurves curve,
    EllipticPoint point,
    Number num,
    Number num1,
    byte[] seed)
  {
    this.m_curve = curve;
    this.m_point = point;
    this.m_num1 = num;
    this.m_num2 = num1;
    this.m_seed = seed;
    switch (curve)
    {
      case FiniteCurves _:
        this.m_fieldID = new ECx9FieldObject(((FiniteCurves) curve).PointQ);
        break;
      case Field2MCurves _:
        Field2MCurves field2Mcurves = (Field2MCurves) curve;
        this.m_fieldID = new ECx9FieldObject(field2Mcurves.PointM, field2Mcurves.ElementX, field2Mcurves.ElementY, field2Mcurves.ElementZ);
        break;
    }
  }

  public EllipticCurves Curve => this.m_curve;

  public EllipticPoint PointG => this.m_point;

  public Number NumberX => this.m_num1;

  public Number NumberY => this.m_num2 == null ? Number.One : this.m_num2;

  public byte[] Seed() => this.m_seed;

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[5]
    {
      (Asn1Encode) new DerInteger(1),
      (Asn1Encode) this.m_fieldID,
      (Asn1Encode) new ECx9Curve(this.m_curve, this.m_seed),
      (Asn1Encode) new ECx9Point(this.m_point),
      (Asn1Encode) new DerInteger(this.m_num1)
    });
    if (this.m_num2 != null)
      collection.Add((Asn1Encode) new DerInteger(this.m_num2));
    return (Asn1) new DerSequence(collection);
  }
}
