// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.X509CertificateParser
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class X509CertificateParser
{
  private Asn1Set m_sData;
  private int m_sDataObjectCount;
  private Stream m_currentStream;

  private X509Certificate ReadDerCertificate(Asn1Stream dIn)
  {
    Asn1Sequence asn1Sequence = (Asn1Sequence) dIn.ReadAsn1();
    if (asn1Sequence.Count <= 1 || !(asn1Sequence[0] is DerObjectID) || !asn1Sequence[0].Equals((object) PKCSOIDs.SignedData))
      return this.CreateX509Certificate(X509CertificateStructure.GetInstance((object) asn1Sequence));
    if (asn1Sequence.Count >= 2)
    {
      foreach (Asn1 asn1 in Asn1Sequence.GetSequence((Asn1Tag) asn1Sequence[1], true))
      {
        if (asn1 is Asn1Tag)
        {
          Asn1Tag taggedObject = (Asn1Tag) asn1;
          if (taggedObject.TagNumber == 0)
          {
            this.m_sData = Asn1Set.GetAsn1Set(taggedObject, false);
            break;
          }
        }
      }
    }
    return this.GetCertificate();
  }

  private X509Certificate GetCertificate()
  {
    if (this.m_sData != null)
    {
      while (this.m_sDataObjectCount < this.m_sData.Count)
      {
        object obj = (object) this.m_sData[this.m_sDataObjectCount++];
        if (obj is Asn1Sequence)
          return this.CreateX509Certificate(X509CertificateStructure.GetInstance(obj));
      }
    }
    return (X509Certificate) null;
  }

  protected virtual X509Certificate CreateX509Certificate(X509CertificateStructure c)
  {
    return new X509Certificate(c);
  }

  internal X509Certificate ReadCertificate(byte[] input)
  {
    return this.ReadCertificate((Stream) new MemoryStream(input, false));
  }

  internal ICollection ReadCertificates(byte[] input)
  {
    return this.ReadCertificates((Stream) new MemoryStream(input, false));
  }

  internal X509Certificate ReadCertificate(Stream inStream)
  {
    if (inStream == null)
      throw new ArgumentNullException(nameof (inStream));
    if (!inStream.CanRead)
      throw new ArgumentException("inStream must be read-able", nameof (inStream));
    if (this.m_currentStream == null)
    {
      this.m_currentStream = inStream;
      this.m_sData = (Asn1Set) null;
      this.m_sDataObjectCount = 0;
    }
    else if (this.m_currentStream != inStream)
    {
      this.m_currentStream = inStream;
      this.m_sData = (Asn1Set) null;
      this.m_sDataObjectCount = 0;
    }
    try
    {
      if (this.m_sData != null)
      {
        if (this.m_sDataObjectCount != this.m_sData.Count)
          return this.GetCertificate();
        this.m_sData = (Asn1Set) null;
        this.m_sDataObjectCount = 0;
        return (X509Certificate) null;
      }
      PushStream pushStream = new PushStream(inStream);
      int b = pushStream.ReadByte();
      if (b < 0)
        return (X509Certificate) null;
      pushStream.Unread(b);
      return this.ReadDerCertificate(new Asn1Stream((Stream) pushStream));
    }
    catch (Exception ex)
    {
      throw new Exception("Failed to read certificate", ex);
    }
  }

  internal ICollection ReadCertificates(Stream inStream)
  {
    IList list = (IList) new ArrayList();
    X509Certificate x509Certificate;
    while ((x509Certificate = this.ReadCertificate(inStream)) != null)
      list.Add((object) x509Certificate);
    return (ICollection) list;
  }
}
