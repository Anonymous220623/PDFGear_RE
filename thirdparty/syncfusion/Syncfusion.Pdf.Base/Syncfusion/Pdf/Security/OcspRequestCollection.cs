// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.OcspRequestCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class OcspRequestCollection : Asn1Encode
{
  private static readonly DerInteger m_integer = new DerInteger(0);
  private readonly DerInteger m_version;
  private readonly OcspTag m_requestorName;
  private readonly Asn1Sequence m_requestList;
  private readonly X509Extensions m_requestExtensions;
  private bool m_versionSet;

  public OcspRequestCollection(
    OcspTag requestorName,
    Asn1Sequence requestList,
    X509Extensions requestExtensions)
  {
    this.m_version = OcspRequestCollection.m_integer;
    this.m_requestorName = requestorName;
    this.m_requestList = requestList;
    this.m_requestExtensions = requestExtensions;
  }

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[0]);
    if (!this.m_version.Equals((object) OcspRequestCollection.m_integer) || this.m_versionSet)
      collection.Add((Asn1Encode) new DerTag(true, 0, (Asn1Encode) this.m_version));
    if (this.m_requestorName != null)
      collection.Add((Asn1Encode) new DerTag(true, 1, (Asn1Encode) this.m_requestorName));
    collection.Add((Asn1Encode) this.m_requestList);
    if (this.m_requestExtensions != null)
      collection.Add((Asn1Encode) new DerTag(true, 2, (Asn1Encode) this.m_requestExtensions));
    return (Asn1) new DerSequence(collection);
  }
}
