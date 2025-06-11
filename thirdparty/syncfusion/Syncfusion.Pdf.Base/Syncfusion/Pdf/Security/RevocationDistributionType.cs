// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RevocationDistributionType
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RevocationDistributionType : Asn1Encode
{
  public const int FullName = 0;
  public const int NameRelativeToCrlIssuer = 1;
  private Asn1Encode m_name;
  private int m_type;

  internal RevocationDistributionType GetDistributionType(Asn1Tag tag, bool isExplicit)
  {
    return this.GetDistributionType((object) Asn1Tag.GetTag(tag, true));
  }

  internal RevocationDistributionType GetDistributionType(object obj)
  {
    switch (obj)
    {
      case null:
      case RevocationDistributionType _:
        return (RevocationDistributionType) obj;
      case Asn1Tag _:
        return new RevocationDistributionType((Asn1Tag) obj);
      default:
        throw new ArgumentException("Invalid entry in sequence");
    }
  }

  internal int PointType => this.m_type;

  internal Asn1Encode Name => this.m_name;

  internal RevocationDistributionType(Asn1Tag tag)
  {
    this.m_type = tag.TagNumber;
    if (this.m_type == 0)
      this.m_name = (Asn1Encode) new RevocationName().GetCrlName(tag, false);
    else
      this.m_name = (Asn1Encode) Asn1Set.GetAsn1Set(tag, false);
  }

  internal RevocationDistributionType()
  {
  }

  public override Asn1 GetAsn1() => (Asn1) new DerTag(false, this.m_type, this.m_name);
}
