// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1Tag
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Asn1Tag : Asn1, IAsn1Tag, IAsn1
{
  internal int m_tagNumber;
  internal bool m_isExplicit = true;
  internal Syncfusion.Pdf.Security.Asn1Encode m_object;

  public int TagNumber => this.m_tagNumber;

  internal bool IsExplicit => this.m_isExplicit;

  internal bool IsEmpty => false;

  protected Asn1Tag(int tagNumber, Syncfusion.Pdf.Security.Asn1Encode asn1Encode)
  {
    this.m_isExplicit = true;
    this.m_tagNumber = tagNumber;
    this.m_object = asn1Encode;
  }

  protected Asn1Tag(bool isExplicit, int tagNumber, Syncfusion.Pdf.Security.Asn1Encode asn1Encode)
  {
    this.m_isExplicit = isExplicit;
    this.m_tagNumber = tagNumber;
    this.m_object = asn1Encode;
  }

  internal static Asn1Tag GetTag(Asn1Tag tag, bool isExplicit)
  {
    if (isExplicit)
      return (Asn1Tag) tag.GetObject();
    throw new ArgumentException("Explicit tag is not used");
  }

  internal static Asn1Tag GetTag(object obj)
  {
    switch (obj)
    {
      case null:
      case Asn1Tag _:
        return (Asn1Tag) obj;
      default:
        throw new ArgumentException("Invalid entry in sequence");
    }
  }

  protected override bool IsEquals(Asn1 asn1Object)
  {
    return asn1Object is Asn1Tag asn1Tag && this.m_tagNumber == asn1Tag.m_tagNumber && this.m_isExplicit == asn1Tag.m_isExplicit && object.Equals((object) this.GetObject(), (object) asn1Tag.GetObject());
  }

  public override int GetHashCode()
  {
    int hashCode = this.m_tagNumber.GetHashCode();
    if (this.m_object != null)
      hashCode ^= this.m_object.GetHashCode();
    return hashCode;
  }

  internal Asn1 GetObject() => this.m_object != null ? this.m_object.GetAsn1() : (Asn1) null;

  public IAsn1 GetParser(int tagNumber, bool isExplicit)
  {
    switch (tagNumber)
    {
      case 4:
        return (IAsn1) Asn1Octet.GetOctetString(this, isExplicit).Parser;
      case 16 /*0x10*/:
        return (IAsn1) Asn1Sequence.GetSequence(this, isExplicit).Parser;
      case 17:
        return (IAsn1) Asn1Set.GetAsn1Set(this, isExplicit).Parser;
      default:
        if (isExplicit)
          return (IAsn1) this.GetObject();
        throw new Exception("Implicit tagging is not supported : " + (object) tagNumber);
    }
  }

  public override string ToString() => $"[{(object) this.m_tagNumber}]{(object) this.m_object}";

  internal override void Encode(DerStream stream) => throw new NotImplementedException();
}
