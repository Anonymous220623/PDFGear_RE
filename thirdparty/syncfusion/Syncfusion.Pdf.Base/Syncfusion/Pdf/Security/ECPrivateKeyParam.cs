// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECPrivateKeyParam
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ECPrivateKeyParam : Asn1Encode
{
  private readonly Asn1Sequence sequence;

  public ECPrivateKeyParam(Asn1Sequence sequence)
  {
    this.sequence = sequence != null ? sequence : throw new ArgumentNullException(nameof (sequence));
  }

  public Number GetKey() => new Number(1, ((Asn1Octet) this.sequence[1]).GetOctets());

  public override Asn1 GetAsn1() => (Asn1) this.sequence;
}
