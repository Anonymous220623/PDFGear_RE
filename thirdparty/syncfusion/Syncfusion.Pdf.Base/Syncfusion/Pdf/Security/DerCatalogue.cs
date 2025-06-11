// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerCatalogue
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerCatalogue : Asn1
{
  private readonly byte[] m_bytes;

  internal DerCatalogue GetEnumeration(object obj)
  {
    switch (obj)
    {
      case null:
      case DerCatalogue _:
        return (DerCatalogue) obj;
      default:
        throw new ArgumentException("Invalid entry" + obj.GetType().Name);
    }
  }

  internal DerCatalogue(int value) => this.m_bytes = Number.ValueOf((long) value).ToByteArray();

  internal DerCatalogue()
  {
  }

  internal DerCatalogue(byte[] bytes) => this.m_bytes = bytes;

  internal Number Value => new Number(this.m_bytes);

  internal override void Encode(DerStream stream) => stream.WriteEncoded(10, this.m_bytes);

  protected override bool IsEquals(Asn1 asn1)
  {
    return asn1 is DerCatalogue derCatalogue && Asn1Constants.AreEqual(this.m_bytes, derCatalogue.m_bytes);
  }

  public override int GetHashCode() => Asn1Constants.GetHashCode(this.m_bytes);
}
