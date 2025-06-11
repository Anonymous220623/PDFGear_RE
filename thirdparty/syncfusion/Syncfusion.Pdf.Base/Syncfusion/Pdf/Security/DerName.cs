// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerName
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerName : Asn1Encode
{
  private Asn1Encode m_name;
  private int m_tagNumber;

  internal DerName(int tagNumber, Asn1Encode name)
  {
    this.m_name = name;
    this.m_tagNumber = tagNumber;
  }

  internal static DerName GetDerName(object name)
  {
    if (name == null || name is DerName)
      return (DerName) name;
    if (name is Asn1Tag)
    {
      Asn1Tag tag = (Asn1Tag) name;
      int tagNumber = tag.TagNumber;
      switch (tagNumber)
      {
        case 0:
          return new DerName(tagNumber, (Asn1Encode) Asn1Sequence.GetSequence(tag, false));
        case 1:
          return new DerName(tagNumber, (Asn1Encode) DerAsciiString.GetAsciiString(tag, false));
        case 2:
          return new DerName(tagNumber, (Asn1Encode) DerAsciiString.GetAsciiString(tag, false));
        case 3:
          throw new ArgumentException("Invalid entry in sequence" + (object) tagNumber);
        case 4:
          return new DerName(tagNumber, (Asn1Encode) X509Name.GetName(tag, true));
        case 5:
          return new DerName(tagNumber, (Asn1Encode) Asn1Sequence.GetSequence(tag, false));
        case 6:
          return new DerName(tagNumber, (Asn1Encode) DerAsciiString.GetAsciiString(tag, false));
        case 7:
          return new DerName(tagNumber, (Asn1Encode) Asn1Octet.GetOctetString(tag, false));
        case 8:
          return new DerName(tagNumber, (Asn1Encode) DerObjectID.GetID(tag, false));
      }
    }
    if (!(name is byte[]))
      throw new ArgumentException("Invalid entry in sequence " + name.GetType().FullName, "obj");
    try
    {
      return DerName.GetDerName((object) Asn1.FromByteArray((byte[]) name));
    }
    catch (Exception ex)
    {
      throw new ArgumentException(ex.Message);
    }
  }

  internal static DerName GetDerName(Asn1Tag tag, bool isExplicit)
  {
    return DerName.GetDerName((object) Asn1Tag.GetTag(tag, true));
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this.m_tagNumber);
    stringBuilder.Append(": ");
    switch (this.m_tagNumber)
    {
      case 1:
      case 2:
      case 6:
        stringBuilder.Append(DerAsciiString.GetAsciiString((object) this.m_name).GetString());
        break;
      case 4:
        stringBuilder.Append(X509Name.GetName((object) this.m_name).ToString());
        break;
      default:
        stringBuilder.Append(this.m_name.ToString());
        break;
    }
    return stringBuilder.ToString();
  }

  public override Asn1 GetAsn1()
  {
    return (Asn1) new DerTag(this.m_tagNumber == 4, this.m_tagNumber, this.m_name);
  }
}
