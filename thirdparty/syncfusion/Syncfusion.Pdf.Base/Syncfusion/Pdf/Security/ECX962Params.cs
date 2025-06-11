// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECX962Params
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ECX962Params : Asn1Encode, IAsn1Choice
{
  private readonly Asn1 m_parameters;

  public ECX962Params(ECX9Field ecParameters) => this.m_parameters = ecParameters.GetAsn1();

  public ECX962Params(DerObjectID namedCurve) => this.m_parameters = (Asn1) namedCurve;

  public ECX962Params(Asn1 obj) => this.m_parameters = obj;

  public bool IsNamedCurve => this.m_parameters is DerObjectID;

  public Asn1 Parameters => this.m_parameters;

  public override Asn1 GetAsn1() => this.m_parameters;
}
