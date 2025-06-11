// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RevocationDistribution
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RevocationDistribution : Asn1Encode
{
  private RevocationDistributionType m_distributionPoint;
  private RevocationName m_issuer;

  private RevocationDistribution(Asn1Sequence sequence)
  {
    for (int index = 0; index != sequence.Count; ++index)
    {
      Asn1Tag tag = Asn1Tag.GetTag((object) sequence[index]);
      switch (tag.TagNumber)
      {
        case 0:
          this.m_distributionPoint = new RevocationDistributionType().GetDistributionType(tag, true);
          break;
        case 2:
          this.m_issuer = new RevocationName().GetCrlName(tag, false);
          break;
      }
    }
  }

  internal RevocationDistribution()
  {
  }

  public RevocationDistribution GetCrlDistribution(object obj)
  {
    switch (obj)
    {
      case null:
      case RevocationDistribution _:
        return (RevocationDistribution) obj;
      case Asn1Sequence _:
        return new RevocationDistribution((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry in CRL distribution point");
    }
  }

  internal RevocationDistributionType DistributionPointName => this.m_distributionPoint;

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[0]);
    if (this.m_distributionPoint != null)
      collection.Add((Asn1Encode) new DerTag(0, (Asn1Encode) this.m_distributionPoint));
    if (this.m_issuer != null)
      collection.Add((Asn1Encode) new DerTag(false, 2, (Asn1Encode) this.m_issuer));
    return (Asn1) new DerSequence(collection);
  }
}
