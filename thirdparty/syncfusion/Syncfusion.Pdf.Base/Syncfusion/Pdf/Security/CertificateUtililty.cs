// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.CertificateUtililty
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class CertificateUtililty
{
  internal string GetCrlUrl(X509Certificate certificate)
  {
    try
    {
      Asn1 extensionValue = this.GetExtensionValue(certificate, X509Extensions.CrlDistributionPoints.ID);
      if (extensionValue == null)
        return (string) null;
      foreach (RevocationDistribution distributionPoint in new RevocationPointList().GetCrlPointList((object) extensionValue).GetDistributionPoints())
      {
        RevocationDistributionType distributionPointName = distributionPoint.DistributionPointName;
        if (distributionPointName.PointType == 0)
        {
          foreach (OcspTag name in ((RevocationName) distributionPointName.Name).Names)
          {
            if (name.TagNumber == 6)
              return DerAsciiString.GetAsciiString((Asn1Tag) name.GetAsn1(), false).GetString();
          }
        }
      }
    }
    catch
    {
    }
    return (string) null;
  }

  internal string GetOcspUrl(X509Certificate certificate)
  {
    try
    {
      Asn1 extensionValue = this.GetExtensionValue(certificate, X509Extensions.AuthorityInfoAccess.ID);
      if (extensionValue == null)
        return (string) null;
      Asn1Sequence asn1Sequence1 = (Asn1Sequence) extensionValue;
      for (int index = 0; index < asn1Sequence1.Count; ++index)
      {
        Asn1Sequence asn1Sequence2 = (Asn1Sequence) asn1Sequence1[index];
        if (asn1Sequence2.Count == 2 && asn1Sequence2[0] is DerObjectID && ((DerObjectID) asn1Sequence2[0]).ID.Equals("1.3.6.1.5.5.7.48.1"))
          return this.GetStringFromGeneralName((Asn1) asn1Sequence2[1]) ?? "";
      }
    }
    catch
    {
    }
    return (string) null;
  }

  private Asn1 GetExtensionValue(X509Certificate certificate, string id)
  {
    byte[] buffer = (byte[]) null;
    Asn1Octet extension = certificate.GetExtension(new DerObjectID(id));
    if (extension != null)
      buffer = extension.GetDerEncoded();
    return buffer == null ? (Asn1) null : new Asn1Stream((Stream) new MemoryStream(((Asn1Octet) new Asn1Stream((Stream) new MemoryStream(buffer)).ReadAsn1()).GetOctets())).ReadAsn1();
  }

  private string GetStringFromGeneralName(Asn1 names)
  {
    return Encoding.UTF8.GetString(Asn1Octet.GetOctetString((Asn1Tag) names, false).GetOctets());
  }
}
