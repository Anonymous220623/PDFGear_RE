// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.RevocationName
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class RevocationName : Asn1Encode
{
  private OcspTag[] m_names;

  internal OcspTag[] Names => (OcspTag[]) this.m_names.Clone();

  private RevocationName(Asn1Sequence sequence)
  {
    this.m_names = new OcspTag[sequence.Count];
    for (int index = 0; index != sequence.Count; ++index)
    {
      OcspTag ocspTag = new OcspTag();
      this.m_names[index] = ocspTag.GetOcspName((object) sequence[index]);
    }
  }

  internal RevocationName()
  {
  }

  public RevocationName GetCrlName(object obj)
  {
    switch (obj)
    {
      case null:
      case RevocationName _:
        return (RevocationName) obj;
      case Asn1Sequence _:
        return new RevocationName((Asn1Sequence) obj);
      default:
        throw new ArgumentException("Invalid entry in sequence");
    }
  }

  public RevocationName GetCrlName(Asn1Tag tag, bool isExplicit)
  {
    return this.GetCrlName((object) Asn1Sequence.GetSequence(tag, isExplicit));
  }

  public override Asn1 GetAsn1() => (Asn1) new DerSequence((Asn1Encode[]) this.m_names);
}
