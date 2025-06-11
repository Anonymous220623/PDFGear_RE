// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECx9FieldObject
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ECx9FieldObject : Asn1Encode
{
  private readonly DerObjectID m_identifier;
  private readonly Asn1 m_param;

  public ECx9FieldObject(Number primePNum)
  {
    this.m_identifier = ECDSAOIDs.X90UniqueID;
    this.m_param = (Asn1) new DerInteger(primePNum);
  }

  public ECx9FieldObject(int num, int num1, int num2, int num3)
  {
    this.m_identifier = ECDSAOIDs.X90RecordID;
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[1]
    {
      (Asn1Encode) new DerInteger(num)
    });
    if (num2 == 0)
      collection.Add((Asn1Encode) ECDSAOIDs.X90TNObjID, (Asn1Encode) new DerInteger(num1));
    else
      collection.Add((Asn1Encode) ECDSAOIDs.X90PPObjID, (Asn1Encode) new DerSequence(new Asn1Encode[3]
      {
        (Asn1Encode) new DerInteger(num1),
        (Asn1Encode) new DerInteger(num2),
        (Asn1Encode) new DerInteger(num3)
      }));
    this.m_param = (Asn1) new DerSequence(collection);
  }

  internal ECx9FieldObject(Asn1Sequence sequence)
  {
    this.m_identifier = (DerObjectID) sequence[0];
    this.m_param = (Asn1) sequence[1];
  }

  public DerObjectID Identifier => this.m_identifier;

  public Asn1 Parameters => this.m_param;

  public override Asn1 GetAsn1()
  {
    return (Asn1) new DerSequence(new Asn1Encode[2]
    {
      (Asn1Encode) this.m_identifier,
      (Asn1Encode) this.m_param
    });
  }
}
