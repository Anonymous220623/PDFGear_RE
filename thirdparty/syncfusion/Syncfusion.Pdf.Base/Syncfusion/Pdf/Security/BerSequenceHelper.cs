// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.BerSequenceHelper
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class BerSequenceHelper : IAsn1Collection, IAsn1
{
  private Asn1Parser m_helper;

  internal BerSequenceHelper(Asn1Parser helper) => this.m_helper = helper;

  public IAsn1 ReadObject() => this.m_helper.ReadObject();

  public Asn1 GetAsn1() => (Asn1) new BerSequence(this.m_helper.ReadCollection());
}
