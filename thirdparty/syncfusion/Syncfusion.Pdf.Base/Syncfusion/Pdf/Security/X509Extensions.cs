// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.X509Extensions
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class X509Extensions : Asn1Encode
{
  internal static readonly DerObjectID SubjectDirectoryAttributes = new DerObjectID("2.5.29.9");
  internal static readonly DerObjectID SubjectKeyIdentifier = new DerObjectID("2.5.29.14");
  internal static readonly DerObjectID KeyUsage = new DerObjectID("2.5.29.15");
  internal static readonly DerObjectID PrivateKeyUsagePeriod = new DerObjectID("2.5.29.16");
  internal static readonly DerObjectID SubjectAlternativeName = new DerObjectID("2.5.29.17");
  internal static readonly DerObjectID IssuerAlternativeName = new DerObjectID("2.5.29.18");
  internal static readonly DerObjectID BasicConstraints = new DerObjectID("2.5.29.19");
  internal static readonly DerObjectID CrlNumber = new DerObjectID("2.5.29.20");
  internal static readonly DerObjectID ReasonCode = new DerObjectID("2.5.29.21");
  internal static readonly DerObjectID InstructionCode = new DerObjectID("2.5.29.23");
  internal static readonly DerObjectID InvalidityDate = new DerObjectID("2.5.29.24");
  internal static readonly DerObjectID DeltaCrlIndicator = new DerObjectID("2.5.29.27");
  internal static readonly DerObjectID IssuingDistributionPoint = new DerObjectID("2.5.29.28");
  internal static readonly DerObjectID CertificateIssuer = new DerObjectID("2.5.29.29");
  internal static readonly DerObjectID NameConstraints = new DerObjectID("2.5.29.30");
  internal static readonly DerObjectID CrlDistributionPoints = new DerObjectID("2.5.29.31");
  internal static readonly DerObjectID CertificatePolicies = new DerObjectID("2.5.29.32");
  internal static readonly DerObjectID PolicyMappings = new DerObjectID("2.5.29.33");
  internal static readonly DerObjectID AuthorityKeyIdentifier = new DerObjectID("2.5.29.35");
  internal static readonly DerObjectID PolicyConstraints = new DerObjectID("2.5.29.36");
  internal static readonly DerObjectID ExtendedKeyUsage = new DerObjectID("2.5.29.37");
  internal static readonly DerObjectID FreshestCrl = new DerObjectID("2.5.29.46");
  internal static readonly DerObjectID InhibitAnyPolicy = new DerObjectID("2.5.29.54");
  internal static readonly DerObjectID AuthorityInfoAccess = new DerObjectID("1.3.6.1.5.5.7.1.1");
  internal static readonly DerObjectID SubjectInfoAccess = new DerObjectID("1.3.6.1.5.5.7.1.11");
  internal static readonly DerObjectID LogoType = new DerObjectID("1.3.6.1.5.5.7.1.12");
  internal static readonly DerObjectID BiometricInfo = new DerObjectID("1.3.6.1.5.5.7.1.2");
  internal static readonly DerObjectID QCStatements = new DerObjectID("1.3.6.1.5.5.7.1.3");
  internal static readonly DerObjectID AuditIdentity = new DerObjectID("1.3.6.1.5.5.7.1.4");
  internal static readonly DerObjectID NoRevAvail = new DerObjectID("2.5.29.56");
  internal static readonly DerObjectID TargetInformation = new DerObjectID("2.5.29.55");
  private readonly IDictionary m_extensions = (IDictionary) new Hashtable();
  private readonly IList m_ordering;

  internal static X509Extensions GetInstance(Asn1Tag obj, bool explicitly)
  {
    return X509Extensions.GetInstance((object) Asn1Sequence.GetSequence(obj, explicitly));
  }

  internal static X509Extensions GetInstance(object obj)
  {
    switch (obj)
    {
      case null:
      case X509Extensions _:
        return (X509Extensions) obj;
      case Asn1Sequence _:
        return new X509Extensions(obj as Asn1Sequence);
      case Asn1Tag _:
        return X509Extensions.GetInstance((object) ((Asn1Tag) obj).GetObject());
      default:
        throw new ArgumentException("unknown object in factory: " + obj.GetType().Name, nameof (obj));
    }
  }

  private X509Extensions(Asn1Sequence seq)
  {
    this.m_ordering = (IList) new ArrayList();
    foreach (Asn1Encode asn1Encode in seq)
    {
      Asn1Sequence sequence = Asn1Sequence.GetSequence((object) asn1Encode.GetAsn1());
      DerObjectID key = sequence.Count >= 2 && sequence.Count <= 3 ? DerObjectID.GetID((object) sequence[0].GetAsn1()) : throw new ArgumentException("Bad sequence size: " + (object) sequence.Count);
      bool critical = sequence.Count == 3 && DerBoolean.GetBoolean((object) sequence[1].GetAsn1()).IsTrue;
      Asn1Octet octetString = Asn1Octet.GetOctetString((object) sequence[sequence.Count - 1].GetAsn1());
      this.m_extensions.Add((object) key, (object) new X509Extension(critical, octetString));
      this.m_ordering.Add((object) key);
    }
  }

  internal IEnumerable ExtensionOids
  {
    get => (IEnumerable) new EnumerableProxy((IEnumerable) this.m_ordering);
  }

  internal X509Extension GetExtension(DerObjectID oid)
  {
    return (X509Extension) this.m_extensions[(object) oid];
  }

  internal X509Extensions(IDictionary extensions)
    : this((IList) null, extensions)
  {
  }

  internal X509Extensions(IList ordering, IDictionary extensions)
  {
    if (ordering == null)
    {
      List<object> objectList = new List<object>();
      foreach (object key in (IEnumerable) extensions.Keys)
        objectList.Add(key);
      this.m_ordering = (IList) objectList;
    }
    else
      this.m_ordering = ordering;
    foreach (DerObjectID key in (IEnumerable) this.m_ordering)
      this.m_extensions.Add((object) key, (object) (X509Extension) extensions[(object) key]);
  }

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection1 = new Asn1EncodeCollection(new Asn1Encode[0]);
    foreach (DerObjectID key in (IEnumerable) this.m_ordering)
    {
      X509Extension extension = (X509Extension) this.m_extensions[(object) key];
      Asn1EncodeCollection collection2 = new Asn1EncodeCollection(new Asn1Encode[1]
      {
        (Asn1Encode) key
      });
      if (extension.IsCritical)
        collection2.Add((Asn1Encode) DerBoolean.True);
      collection2.Add((Asn1Encode) extension.Value);
      collection1.Add((Asn1Encode) new DerSequence(collection2));
    }
    return (Asn1) new DerSequence(collection1);
  }
}
