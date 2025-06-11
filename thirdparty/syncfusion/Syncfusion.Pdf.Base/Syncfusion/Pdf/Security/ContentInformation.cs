// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ContentInformation
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ContentInformation : Asn1Encode
{
  private DerObjectID m_contentType;
  private Asn1Encode m_content;

  internal static ContentInformation GetInformation(object obj)
  {
    switch (obj)
    {
      case null:
      case ContentInformation _:
        return (ContentInformation) obj;
      case Asn1Sequence _:
        return new ContentInformation((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry");
    }
  }

  private ContentInformation(Asn1Sequence sequence)
  {
    this.m_contentType = sequence.Count >= 1 && sequence.Count <= 2 ? (DerObjectID) sequence[0] : throw new ArgumentException("Invalid length in sequence");
    if (sequence.Count <= 1)
      return;
    Asn1Tag asn1Tag = (Asn1Tag) sequence[1];
    if (!asn1Tag.IsExplicit || asn1Tag.TagNumber != 0)
      throw new ArgumentException("Invalid tag");
    this.m_content = (Asn1Encode) asn1Tag.GetObject();
  }

  internal DerObjectID ContentType => this.m_contentType;

  internal Asn1Encode Content => this.m_content;

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[1]
    {
      (Asn1Encode) this.m_contentType
    });
    if (this.m_content != null)
      collection.Add((Asn1Encode) new BerTag(0, this.m_content));
    return (Asn1) new BerSequence(collection);
  }
}
