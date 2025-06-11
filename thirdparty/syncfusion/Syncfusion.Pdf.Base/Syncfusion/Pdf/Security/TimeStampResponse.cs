// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.TimeStampResponse
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class TimeStampResponse
{
  private Asn1 m_encodedObject;
  private Asn1Integer m_pkiStatusInfo;
  private Asn1 m_timeStampToken;
  private Asn1Identifier m_contentType;

  internal Asn1 Object => this.m_encodedObject;

  internal TimeStampResponse(byte[] bytes) => this.ReadTimeStampResponse(new Asn1Stream(bytes));

  internal byte[] GetEncoded(Asn1 encodedObject) => this.ReadTimeStampToken(encodedObject);

  private void ReadTimeStampResponse(Asn1Stream stream) => this.m_encodedObject = stream.ReadAsn1();

  private byte[] ReadTimeStampToken(Asn1 encodedObject)
  {
    if (encodedObject is Asn1Sequence)
    {
      this.m_pkiStatusInfo = new Asn1Integer(((encodedObject as Asn1Sequence)[0] as DerObjectID).GetBytes());
      this.m_timeStampToken = encodedObject;
    }
    return this.ReadContentInfo();
  }

  private byte[] ReadContentInfo()
  {
    this.m_contentType = (this.m_timeStampToken as Asn1Sequence)[0] as Asn1Identifier;
    return this.ReadTimeStampContent();
  }

  private byte[] ReadTimeStampContent()
  {
    return new Asn1DerStream((Stream) new MemoryStream()).ParseTimeStamp(this.m_timeStampToken);
  }
}
