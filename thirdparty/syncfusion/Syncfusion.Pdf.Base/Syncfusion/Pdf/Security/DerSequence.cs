// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerSequence
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerSequence : Asn1Sequence
{
  internal static readonly DerSequence Empty = new DerSequence();

  internal static DerSequence FromCollection(Asn1EncodeCollection collection)
  {
    return collection.Count >= 1 ? new DerSequence(collection) : DerSequence.Empty;
  }

  internal DerSequence()
    : base(0)
  {
  }

  internal DerSequence(Syncfusion.Pdf.Security.Asn1Encode asn1)
    : base(1)
  {
    this.AddObject(asn1);
  }

  internal DerSequence(params Syncfusion.Pdf.Security.Asn1Encode[] collection)
    : base(collection.Length)
  {
    foreach (Syncfusion.Pdf.Security.Asn1Encode asn1Encode in collection)
      this.AddObject(asn1Encode);
  }

  internal DerSequence(Asn1EncodeCollection collection)
    : base(collection.Count)
  {
    foreach (Syncfusion.Pdf.Security.Asn1Encode asn1Encode in collection)
      this.AddObject(asn1Encode);
  }

  internal override void Encode(DerStream outputStream)
  {
    MemoryStream memoryStream = new MemoryStream();
    DerStream derStream = new DerStream((Stream) memoryStream);
    foreach (Syncfusion.Pdf.Security.Asn1Encode asn1Encode in (Asn1Sequence) this)
      derStream.WriteObject((object) asn1Encode);
    derStream.m_stream.Close();
    byte[] array = memoryStream.ToArray();
    outputStream.WriteEncoded(48 /*0x30*/, array);
  }
}
