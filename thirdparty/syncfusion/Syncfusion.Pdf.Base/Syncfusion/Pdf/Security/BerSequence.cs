// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.BerSequence
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class BerSequence : DerSequence
{
  public static readonly BerSequence Empty = new BerSequence();

  internal BerSequence()
  {
  }

  internal BerSequence(Syncfusion.Pdf.Security.Asn1Encode asn1)
    : base(asn1)
  {
  }

  internal BerSequence(params Syncfusion.Pdf.Security.Asn1Encode[] collection)
    : base(collection)
  {
  }

  internal BerSequence(Asn1EncodeCollection collection)
    : base(collection)
  {
  }

  public static BerSequence FromCollection(Asn1EncodeCollection collection)
  {
    return collection.Count >= 1 ? new BerSequence(collection) : BerSequence.Empty;
  }

  internal override void Encode(DerStream stream)
  {
    if (stream is Asn1DerStream)
    {
      stream.m_stream.WriteByte((byte) 48 /*0x30*/);
      stream.m_stream.WriteByte((byte) 128 /*0x80*/);
      foreach (Syncfusion.Pdf.Security.Asn1Encode asn1Encode in (Asn1Sequence) this)
        stream.WriteObject((object) asn1Encode);
      stream.m_stream.WriteByte((byte) 0);
      stream.m_stream.WriteByte((byte) 0);
    }
    else
      base.Encode(stream);
  }
}
