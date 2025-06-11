// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.ECCipherKeyParam
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class ECCipherKeyParam
{
  private readonly CipherParameter m_publicKeyParameter;
  private readonly CipherParameter m_privateKeyParameter;

  internal ECCipherKeyParam(CipherParameter publicKeyParameter, CipherParameter privateKeyParameter)
  {
    if (publicKeyParameter.IsPrivate)
      throw new ArgumentException("Expected a public key", "publicParameter");
    if (!privateKeyParameter.IsPrivate)
      throw new ArgumentException("Expected a private key", "privateParameter");
    this.m_publicKeyParameter = publicKeyParameter;
    this.m_privateKeyParameter = privateKeyParameter;
  }
}
