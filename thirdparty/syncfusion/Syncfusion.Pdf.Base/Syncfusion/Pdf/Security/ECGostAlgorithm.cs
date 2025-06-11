// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECGostAlgorithm
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ECGostAlgorithm : Asn1Encode
{
  private DerObjectID m_publicKey;
  private DerObjectID m_digestParam;
  private DerObjectID m_encryptParam;

  public ECGostAlgorithm(DerObjectID m_publicKey, DerObjectID m_digestParam)
    : this(m_publicKey, m_digestParam, (DerObjectID) null)
  {
  }

  public ECGostAlgorithm(
    DerObjectID m_publicKey,
    DerObjectID m_digestParam,
    DerObjectID m_encryptParam)
  {
    if (m_publicKey == null)
      throw new ArgumentNullException("publicKey");
    if (m_digestParam == null)
      throw new ArgumentNullException("digestParam");
    this.m_publicKey = m_publicKey;
    this.m_digestParam = m_digestParam;
    this.m_encryptParam = m_encryptParam;
  }

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[2]
    {
      (Asn1Encode) this.m_publicKey,
      (Asn1Encode) this.m_digestParam
    });
    if (this.m_encryptParam != null)
      collection.Add((Asn1Encode) this.m_encryptParam);
    return (Asn1) new DerSequence(collection);
  }
}
