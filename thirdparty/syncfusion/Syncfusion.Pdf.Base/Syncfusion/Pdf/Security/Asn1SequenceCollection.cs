// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1SequenceCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Asn1SequenceCollection : Asn1Encode
{
  private DerObjectID m_id;
  private Asn1 m_value;
  private Asn1Set m_attributes;

  internal Asn1SequenceCollection(Asn1Sequence sequence)
  {
    this.m_id = DerObjectID.GetID((object) sequence[0]);
    this.m_value = (sequence[1] as Asn1Tag).GetObject();
    if (sequence.Count != 3)
      return;
    this.m_attributes = (Asn1Set) sequence[2];
  }

  internal DerObjectID ID => this.m_id;

  internal Asn1 Value => this.m_value;

  internal Asn1Set Attributes => this.m_attributes;

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[2]
    {
      (Asn1Encode) this.m_id,
      (Asn1Encode) new DerTag(0, (Asn1Encode) this.m_value)
    });
    if (this.m_attributes != null)
      collection.Add((Asn1Encode) this.m_attributes);
    return (Asn1) new DerSequence(collection);
  }
}
