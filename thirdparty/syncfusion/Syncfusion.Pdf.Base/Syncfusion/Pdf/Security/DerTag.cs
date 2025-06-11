// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerTag
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerTag : Asn1Tag
{
  internal DerTag(int tagNumber, Syncfusion.Pdf.Security.Asn1Encode asn1)
    : base(tagNumber, asn1)
  {
  }

  internal DerTag(bool isExplicit, int tagNumber, Syncfusion.Pdf.Security.Asn1Encode asn1)
    : base(isExplicit, tagNumber, asn1)
  {
  }

  internal DerTag(int tagNumber)
    : base(false, tagNumber, (Syncfusion.Pdf.Security.Asn1Encode) DerSequence.Empty)
  {
  }

  internal override void Encode(DerStream stream)
  {
    if (!this.IsEmpty)
    {
      byte[] derEncoded = this.m_object.GetDerEncoded();
      if (this.m_isExplicit)
      {
        stream.WriteEncoded(160 /*0xA0*/, this.m_tagNumber, derEncoded);
      }
      else
      {
        int flag = (int) derEncoded[0] & 32 /*0x20*/ | 128 /*0x80*/;
        stream.WriteTag(flag, this.m_tagNumber);
        stream.m_stream.Write(derEncoded, 1, derEncoded.Length - 1);
      }
    }
    else
      stream.WriteEncoded(160 /*0xA0*/, this.m_tagNumber, new byte[0]);
  }
}
