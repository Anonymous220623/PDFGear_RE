// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.OcspTag
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class OcspTag : Asn1Encode
{
  internal Asn1Encode m_encode;
  internal int m_tagNumber;

  internal OcspTag(int tag, Asn1Encode encode)
  {
    this.m_encode = encode;
    this.m_tagNumber = tag;
  }

  internal OcspTag()
  {
  }

  internal OcspTag GetOcspName(object obj)
  {
    if (obj == null || obj is OcspTag)
      return (OcspTag) obj;
    if (obj is Asn1Tag)
    {
      Asn1Tag tag = (Asn1Tag) obj;
      int tagNumber = tag.TagNumber;
      switch (tagNumber)
      {
        case 0:
          return new OcspTag(tagNumber, (Asn1Encode) Asn1Sequence.GetSequence(tag, false));
        case 1:
          return new OcspTag(tagNumber, (Asn1Encode) DerAsciiString.GetAsciiString(tag, false));
        case 2:
          return new OcspTag(tagNumber, (Asn1Encode) DerAsciiString.GetAsciiString(tag, false));
        case 3:
          throw new ArgumentException("Invalid tag number specified" + (object) tagNumber);
        case 4:
          return new OcspTag(tagNumber, (Asn1Encode) X509Name.GetName(tag, true));
        case 5:
          return new OcspTag(tagNumber, (Asn1Encode) Asn1Sequence.GetSequence(tag, false));
        case 6:
          return new OcspTag(tagNumber, (Asn1Encode) DerAsciiString.GetAsciiString(tag, false));
        case 7:
          return new OcspTag(tagNumber, (Asn1Encode) Asn1Octet.GetOctetString(tag, false));
        case 8:
          return new OcspTag(tagNumber, (Asn1Encode) DerObjectID.GetID(tag, false));
      }
    }
    if (!(obj is byte[]))
      throw new ArgumentException("Invalid entry in sequence");
    try
    {
      return this.GetOcspName((object) Asn1.FromByteArray((byte[]) obj));
    }
    catch (IOException ex)
    {
      throw new ArgumentException("Invalid OCSP name to parse");
    }
  }

  internal int TagNumber => this.m_tagNumber;

  public override Asn1 GetAsn1()
  {
    return (Asn1) new DerTag(this.m_tagNumber == 4, this.m_tagNumber, this.m_encode);
  }
}
