// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PKCS12PasswordParameter
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class PKCS12PasswordParameter : Asn1Encode
{
  private DerInteger m_iterations;
  private Asn1Octet m_octet;

  internal PKCS12PasswordParameter(byte[] bytes, int iteration)
  {
    this.m_octet = (Asn1Octet) new DerOctet(bytes);
    this.m_iterations = new DerInteger(iteration);
  }

  private PKCS12PasswordParameter(Asn1Sequence sequence)
  {
    this.m_octet = sequence.Count == 2 ? Asn1Octet.GetOctetString((object) sequence[0]) : throw new ArgumentException("Invalid length in sequence");
    this.m_iterations = DerInteger.GetNumber((object) sequence[1]);
  }

  internal static PKCS12PasswordParameter GetPBEParameter(object obj)
  {
    switch (obj)
    {
      case PKCS12PasswordParameter _:
        return (PKCS12PasswordParameter) obj;
      case Asn1Sequence _:
        return new PKCS12PasswordParameter((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry");
    }
  }

  internal Number Iterations => this.m_iterations.Value;

  internal byte[] Octets => this.m_octet.GetOctets();

  public override Asn1 GetAsn1()
  {
    return (Asn1) new DerSequence(new Asn1Encode[2]
    {
      (Asn1Encode) this.m_octet,
      (Asn1Encode) this.m_iterations
    });
  }
}
