// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RevocationPointList
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RevocationPointList : Asn1Encode
{
  private readonly Asn1Sequence m_sequence;

  internal RevocationPointList GetCrlPointList(object obj)
  {
    switch (obj)
    {
      case RevocationPointList _:
      case null:
        return (RevocationPointList) obj;
      case Asn1Sequence _:
        return new RevocationPointList((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry in sequence");
    }
  }

  internal RevocationPointList()
  {
  }

  private RevocationPointList(Asn1Sequence sequence) => this.m_sequence = sequence;

  internal RevocationDistribution[] GetDistributionPoints()
  {
    RevocationDistribution[] distributionPoints = new RevocationDistribution[this.m_sequence.Count];
    RevocationDistribution revocationDistribution = new RevocationDistribution();
    for (int index = 0; index != this.m_sequence.Count; ++index)
      distributionPoints[index] = revocationDistribution.GetCrlDistribution((object) this.m_sequence[index]);
    return distributionPoints;
  }

  public override Asn1 GetAsn1() => (Asn1) this.m_sequence;
}
