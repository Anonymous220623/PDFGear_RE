// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.OcspStatus
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class OcspStatus : Asn1Encode
{
  private int m_tagNumber;
  private Asn1Encode m_value;

  internal OcspStatus(Asn1Tag choice)
  {
    this.m_tagNumber = choice.TagNumber;
    switch (choice.TagNumber)
    {
      case 0:
      case 2:
        this.m_value = (Asn1Encode) new DerNull(0);
        break;
    }
  }

  internal OcspStatus()
  {
  }

  internal OcspStatus GetStatus(object obj)
  {
    switch (obj)
    {
      case null:
      case OcspStatus _:
        return (OcspStatus) obj;
      case Asn1Tag _:
        return new OcspStatus((Asn1Tag) obj);
      default:
        throw new ArgumentException("Invalid entry in sequence");
    }
  }

  internal int TagNumber => this.m_tagNumber;

  public override Asn1 GetAsn1() => (Asn1) new DerTag(false, this.m_tagNumber, this.m_value);
}
