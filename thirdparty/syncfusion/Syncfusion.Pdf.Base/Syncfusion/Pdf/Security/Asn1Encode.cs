// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1Encode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal abstract class Asn1Encode : IAsn1
{
  public abstract Asn1 GetAsn1();

  internal byte[] GetEncoded()
  {
    MemoryStream memoryStream = new MemoryStream();
    new Asn1DerStream((Stream) memoryStream).WriteObject((object) this);
    return memoryStream.ToArray();
  }

  internal byte[] GetEncoded(string encoding)
  {
    if (!encoding.Equals("DER"))
      return this.GetEncoded();
    MemoryStream memoryStream = new MemoryStream();
    new DerStream((Stream) memoryStream).WriteObject((object) this);
    return memoryStream.ToArray();
  }

  public byte[] GetDerEncoded()
  {
    try
    {
      return this.GetEncoded("DER");
    }
    catch (IOException ex)
    {
      return (byte[]) null;
    }
  }

  public sealed override int GetHashCode() => this.GetAsn1().GetAsn1Hash();

  public sealed override bool Equals(object obj)
  {
    if (obj == this)
      return true;
    if (!(obj is IAsn1 asn1_1))
      return false;
    Asn1 asn1_2 = this.GetAsn1();
    Asn1 asn1_3 = asn1_1.GetAsn1();
    return asn1_2 == asn1_3 || asn1_2.Asn1Equals(asn1_3);
  }
}
