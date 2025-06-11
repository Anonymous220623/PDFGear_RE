// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.BaseConstraints
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class BaseConstraints : Asn1Encode
{
  private DerBoolean m_isCertificate;
  private DerInteger m_pathLength;

  internal Number PathLenConstraint
  {
    get => this.m_pathLength != null ? this.m_pathLength.Value : (Number) null;
  }

  internal bool IsCertificate => this.m_isCertificate != null && this.m_isCertificate.IsTrue;

  internal static BaseConstraints GetConstraints(object obj)
  {
    switch (obj)
    {
      case null:
      case BaseConstraints _:
        return (BaseConstraints) obj;
      case Asn1Sequence _:
        return new BaseConstraints((Asn1Sequence) obj);
      case X509Extension _:
        return BaseConstraints.GetConstraints((object) X509Extension.ConvertValueToObject((X509Extension) obj));
      default:
        throw new ArgumentException("Invalid entry");
    }
  }

  private BaseConstraints(Asn1Sequence sequence)
  {
    if (sequence.Count <= 0)
      return;
    if (sequence[0] is DerBoolean)
      this.m_isCertificate = DerBoolean.GetBoolean((object) sequence[0]);
    else
      this.m_pathLength = DerInteger.GetNumber((object) sequence[0]);
    if (sequence.Count <= 1)
      return;
    if (this.m_isCertificate == null)
      throw new ArgumentException("Invalid length in sequence");
    this.m_pathLength = DerInteger.GetNumber((object) sequence[1]);
  }

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[0]);
    if (this.m_isCertificate != null)
      collection.Add((Asn1Encode) this.m_isCertificate);
    if (this.m_pathLength != null)
      collection.Add((Asn1Encode) this.m_pathLength);
    return (Asn1) new DerSequence(collection);
  }

  public override string ToString()
  {
    if (this.m_pathLength == null)
      return $"BasicConstraints: isCa({(object) this.IsCertificate})";
    return $"BasicConstraints: isCa({(object) this.IsCertificate}), pathLenConstraint = {(object) this.m_pathLength.Value}";
  }
}
