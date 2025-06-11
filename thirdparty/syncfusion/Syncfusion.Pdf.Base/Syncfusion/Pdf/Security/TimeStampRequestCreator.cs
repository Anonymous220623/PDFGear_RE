// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.TimeStampRequestCreator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class TimeStampRequestCreator : Asn1
{
  private const string c_IdSHA256 = "2.16.840.1.101.3.4.2.1";
  private const string c_IdTimeStampToken = "1.2.840.113549.1.9.16.2.14";
  private Asn1Integer m_version;
  private MessageStamp m_messageImprint;
  private Asn1Boolean m_certReq;

  public TimeStampRequestCreator(bool certReq)
  {
    this.m_certReq = new Asn1Boolean(certReq);
    this.m_version = new Asn1Integer(1L);
  }

  public byte[] GetAsnEncodedTimestampRequest(byte[] hash)
  {
    return new Asn1Sequence(new Asn1EncodeCollection(new Syncfusion.Pdf.Security.Asn1Encode[0])
    {
      new Syncfusion.Pdf.Security.Asn1Encode[1]{ (Syncfusion.Pdf.Security.Asn1Encode) new Asn1Integer(1L) },
      new Syncfusion.Pdf.Security.Asn1Encode[1]
      {
        (Syncfusion.Pdf.Security.Asn1Encode) new Asn1Sequence(new Syncfusion.Pdf.Security.Asn1Encode[2]
        {
          (Syncfusion.Pdf.Security.Asn1Encode) new Algorithms(new Asn1Identifier("2.16.840.1.101.3.4.2.1"), (Asn1) DerNull.Value),
          (Syncfusion.Pdf.Security.Asn1Encode) new Asn1Octet(hash)
        })
      },
      new Syncfusion.Pdf.Security.Asn1Encode[1]{ (Syncfusion.Pdf.Security.Asn1Encode) new Asn1Integer(100L) },
      new Syncfusion.Pdf.Security.Asn1Encode[1]{ (Syncfusion.Pdf.Security.Asn1Encode) new Asn1Boolean(true) }
    }).AsnEncode();
  }

  protected override bool IsEquals(Asn1 asn1Object) => throw new NotImplementedException();

  public override int GetHashCode() => throw new NotImplementedException();

  internal override void Encode(DerStream derOut) => throw new NotImplementedException();
}
