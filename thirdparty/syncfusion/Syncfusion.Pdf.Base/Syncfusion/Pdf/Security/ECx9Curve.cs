// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECx9Curve
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ECx9Curve : Asn1Encode
{
  private readonly EllipticCurves m_curve;
  private readonly byte[] m_seed;
  private readonly DerObjectID m_fieldID;

  public ECx9Curve(EllipticCurves curve, byte[] seed)
  {
    this.m_curve = curve != null ? curve : throw new ArgumentNullException(nameof (curve));
    this.m_seed = Asn1Constants.Clone(seed);
    if (curve is FiniteCurves)
    {
      this.m_fieldID = ECDSAOIDs.X90UniqueID;
    }
    else
    {
      if (!(curve is Field2MCurves))
        throw new ArgumentException("EllipticCurves is not implemented");
      this.m_fieldID = ECDSAOIDs.X90RecordID;
    }
  }

  public ECx9Curve(ECx9FieldObject fieldID, Asn1Sequence sequence)
  {
    if (fieldID == null)
      throw new ArgumentNullException(nameof (fieldID));
    if (sequence == null)
      throw new ArgumentNullException(nameof (sequence));
    this.m_fieldID = fieldID.Identifier;
    if (this.m_fieldID.Equals((object) ECDSAOIDs.X90UniqueID))
    {
      Number number = ((DerInteger) fieldID.Parameters).Value;
      ECx9FieldObjectID ecx9FieldObjectId1 = new ECx9FieldObjectID(number, (Asn1Octet) sequence[0]);
      ECx9FieldObjectID ecx9FieldObjectId2 = new ECx9FieldObjectID(number, (Asn1Octet) sequence[1]);
      this.m_curve = (EllipticCurves) new FiniteCurves(number, ecx9FieldObjectId1.Value.ToIntValue(), ecx9FieldObjectId2.Value.ToIntValue());
    }
    else if (this.m_fieldID.Equals((object) ECDSAOIDs.X90RecordID))
    {
      DerSequence parameters = (DerSequence) fieldID.Parameters;
      int intValue1 = ((DerInteger) parameters[0]).Value.IntValue;
      DerObjectID derObjectId = (DerObjectID) parameters[1];
      int num1 = 0;
      int num2 = 0;
      int intValue2;
      if (derObjectId.Equals((object) ECDSAOIDs.X90TNObjID))
      {
        intValue2 = ((DerInteger) parameters[2]).Value.IntValue;
      }
      else
      {
        DerSequence derSequence = (DerSequence) parameters[2];
        intValue2 = ((DerInteger) derSequence[0]).Value.IntValue;
        num1 = ((DerInteger) derSequence[1]).Value.IntValue;
        num2 = ((DerInteger) derSequence[2]).Value.IntValue;
      }
      ECx9FieldObjectID ecx9FieldObjectId3 = new ECx9FieldObjectID(intValue1, intValue2, num1, num2, (Asn1Octet) sequence[0]);
      ECx9FieldObjectID ecx9FieldObjectId4 = new ECx9FieldObjectID(intValue1, intValue2, num1, num2, (Asn1Octet) sequence[1]);
      this.m_curve = (EllipticCurves) new Field2MCurves(intValue1, intValue2, num1, num2, ecx9FieldObjectId3.Value.ToIntValue(), ecx9FieldObjectId4.Value.ToIntValue());
    }
    if (sequence.Count != 3)
      return;
    this.m_seed = ((DerBitString) sequence[2]).GetBytes();
  }

  public EllipticCurves Curve => this.m_curve;

  public byte[] GetSeed() => Asn1Constants.Clone(this.m_seed);

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[0]);
    if (this.m_fieldID.Equals((object) ECDSAOIDs.X90UniqueID) || this.m_fieldID.Equals((object) ECDSAOIDs.X90RecordID))
    {
      collection.Add((Asn1Encode) new ECx9FieldObjectID(this.m_curve.ElementA).GetAsn1());
      collection.Add((Asn1Encode) new ECx9FieldObjectID(this.m_curve.ElementB).GetAsn1());
    }
    if (this.m_seed != null)
      collection.Add((Asn1Encode) new DerBitString(this.m_seed));
    return (Asn1) new DerSequence(collection);
  }
}
