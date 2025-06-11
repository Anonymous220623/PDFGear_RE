// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RevocationListRequest
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RevocationListRequest : Asn1Encode
{
  private OcspRequestCollection m_requests;

  public RevocationListRequest(OcspRequestCollection requests)
  {
    this.m_requests = requests != null ? requests : throw new ArgumentNullException(nameof (requests));
  }

  public override Asn1 GetAsn1()
  {
    return (Asn1) new DerSequence(new Asn1EncodeCollection(new Asn1Encode[1]
    {
      (Asn1Encode) this.m_requests
    }));
  }
}
