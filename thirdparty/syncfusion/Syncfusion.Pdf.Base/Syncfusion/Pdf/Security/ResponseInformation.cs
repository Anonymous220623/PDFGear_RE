// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ResponseInformation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ResponseInformation : Asn1Encode
{
  private static readonly DerInteger Version1 = new DerInteger(0);
  private bool m_versionPresent;
  private DerInteger m_version;
  private RevocationResponseIdentifier m_responderIdentifier;
  private GeneralizedTime m_producedTime;
  private Asn1Sequence m_sequence;
  private X509Extensions m_responseExtensions;

  private ResponseInformation(Asn1Sequence sequence)
  {
    int num1 = 0;
    Asn1Encode asn1Encode1 = sequence[0];
    if (asn1Encode1 is Asn1Tag)
    {
      Asn1Tag tag = (Asn1Tag) asn1Encode1;
      if (tag.TagNumber == 0)
      {
        this.m_versionPresent = true;
        this.m_version = DerInteger.GetNumber(tag, true);
        ++num1;
      }
      else
        this.m_version = ResponseInformation.Version1;
    }
    else
      this.m_version = ResponseInformation.Version1;
    RevocationResponseIdentifier responseIdentifier = new RevocationResponseIdentifier();
    Asn1Sequence asn1Sequence1 = sequence;
    int index1 = num1;
    int num2 = index1 + 1;
    Asn1Encode asn1Encode2 = asn1Sequence1[index1];
    this.m_responderIdentifier = responseIdentifier.GetResponseID((object) asn1Encode2);
    Asn1Sequence asn1Sequence2 = sequence;
    int index2 = num2;
    int num3 = index2 + 1;
    this.m_producedTime = (GeneralizedTime) asn1Sequence2[index2];
    Asn1Sequence asn1Sequence3 = sequence;
    int index3 = num3;
    int index4 = index3 + 1;
    this.m_sequence = (Asn1Sequence) asn1Sequence3[index3];
    if (sequence.Count <= index4)
      return;
    this.m_responseExtensions = X509Extensions.GetInstance((Asn1Tag) sequence[index4], true);
  }

  internal ResponseInformation()
  {
  }

  internal ResponseInformation GetInformation(object obj)
  {
    switch (obj)
    {
      case null:
      case ResponseInformation _:
        return (ResponseInformation) obj;
      case Asn1Sequence _:
        return new ResponseInformation((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry in sequence");
    }
  }

  internal Asn1Sequence Sequence => this.m_sequence;

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[0]);
    if (this.m_versionPresent || !this.m_version.Equals((object) ResponseInformation.Version1))
      collection.Add((Asn1Encode) new DerTag(true, 0, (Asn1Encode) this.m_version));
    collection.Add((Asn1Encode) this.m_responderIdentifier, (Asn1Encode) this.m_producedTime, (Asn1Encode) this.m_sequence);
    if (this.m_responseExtensions != null)
      collection.Add((Asn1Encode) new DerTag(true, 1, (Asn1Encode) this.m_responseExtensions));
    return (Asn1) new DerSequence(collection);
  }
}
