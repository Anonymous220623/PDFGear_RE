// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.BerTag
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class BerTag : DerTag
{
  internal BerTag(int tagNumber, Syncfusion.Pdf.Security.Asn1Encode asn1)
    : base(tagNumber, asn1)
  {
  }

  internal BerTag(bool IsExplicit, int tagNumber, Syncfusion.Pdf.Security.Asn1Encode asn1)
    : base(IsExplicit, tagNumber, asn1)
  {
  }

  internal BerTag(int tagNumber)
    : base(false, tagNumber, (Syncfusion.Pdf.Security.Asn1Encode) BerSequence.Empty)
  {
  }

  internal new void Encode(DerStream stream)
  {
    if (stream is Asn1DerStream)
    {
      stream.WriteTag(160 /*0xA0*/, this.m_tagNumber);
      stream.m_stream.WriteByte((byte) 128 /*0x80*/);
      if (!this.IsEmpty)
      {
        if (!this.m_isExplicit)
        {
          IEnumerable enumerable;
          if (this.m_object is Asn1Octet)
            enumerable = !(this.m_object is BerOctet) ? (IEnumerable) new BerOctet(((Asn1Octet) this.m_object).GetOctets()) : (IEnumerable) this.m_object;
          else if (this.m_object is Asn1Sequence)
            enumerable = (IEnumerable) this.m_object;
          else
            enumerable = this.m_object is Asn1Set ? (IEnumerable) this.m_object : throw new Exception(this.m_object.GetType().Name);
          foreach (Syncfusion.Pdf.Security.Asn1Encode asn1Encode in enumerable)
            stream.WriteObject((object) asn1Encode);
        }
        else
          stream.WriteObject((object) this.m_object);
      }
      stream.m_stream.WriteByte((byte) 0);
      stream.m_stream.WriteByte((byte) 0);
    }
    else
      base.Encode(stream);
  }
}
