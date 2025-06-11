// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerSet
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerSet : Asn1Set
{
  internal static readonly DerSet Empty = new DerSet();

  internal DerSet()
    : base(0)
  {
  }

  internal DerSet(params Syncfusion.Pdf.Security.Asn1Encode[] collection)
    : base(collection.Length)
  {
    foreach (Syncfusion.Pdf.Security.Asn1Encode asn1Encode in collection)
      this.AddObject(asn1Encode);
    this.SortObjects();
  }

  internal DerSet(Asn1EncodeCollection collection)
    : this(collection, true)
  {
  }

  internal DerSet(Asn1EncodeCollection collection, bool isSort)
    : base(collection.Count)
  {
    foreach (Syncfusion.Pdf.Security.Asn1Encode asn1Encode in collection)
      this.AddObject(asn1Encode);
    if (!isSort)
      return;
    this.SortObjects();
  }

  internal override void Encode(DerStream outputStream)
  {
    MemoryStream memoryStream = new MemoryStream();
    DerStream derStream = new DerStream((Stream) memoryStream);
    foreach (Syncfusion.Pdf.Security.Asn1Encode asn1Encode in (Asn1Set) this)
      derStream.WriteObject((object) asn1Encode);
    derStream.m_stream.Close();
    byte[] array = memoryStream.ToArray();
    outputStream.WriteEncoded(49, array);
  }

  internal static DerSet FromCollection(Asn1EncodeCollection collection)
  {
    return collection.Count >= 1 ? new DerSet(collection) : DerSet.Empty;
  }

  internal static DerSet FromCollection(Asn1EncodeCollection collection, bool isSort)
  {
    return collection.Count >= 1 ? new DerSet(collection, isSort) : DerSet.Empty;
  }
}
