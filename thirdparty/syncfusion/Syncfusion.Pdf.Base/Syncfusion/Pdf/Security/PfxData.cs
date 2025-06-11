// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PfxData
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class PfxData : Asn1Encode
{
  private ContentInformation m_contentInformation;
  private MacInformation m_macInformation;

  internal PfxData(Asn1Sequence sequence)
  {
    Number number = ((DerInteger) sequence[0]).Value;
    this.m_contentInformation = ContentInformation.GetInformation((object) sequence[1]);
    if (sequence.Count != 3)
      return;
    this.m_macInformation = MacInformation.GetInformation((object) sequence[2]);
  }

  internal ContentInformation ContentInformation => this.m_contentInformation;

  public override Asn1 GetAsn1()
  {
    Asn1EncodeCollection collection = new Asn1EncodeCollection(new Asn1Encode[2]
    {
      (Asn1Encode) new DerInteger(3),
      (Asn1Encode) this.m_contentInformation
    });
    if (this.m_macInformation != null)
      collection.Add((Asn1Encode) this.m_macInformation);
    return (Asn1) new BerSequence(collection);
  }
}
