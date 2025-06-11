// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.SecureParamNumber
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class SecureParamNumber : ICipherParam
{
  private readonly ICipherParam m_parameters;
  private readonly SecureRandomAlgorithm m_random;

  public SecureParamNumber(ICipherParam parameters, SecureRandomAlgorithm random)
  {
    if (parameters == null)
      throw new ArgumentNullException(nameof (parameters));
    if (random == null)
      throw new ArgumentNullException(nameof (random));
    this.m_parameters = parameters;
    this.m_random = random;
  }

  public SecureParamNumber(ICipherParam parameters)
    : this(parameters, new SecureRandomAlgorithm())
  {
  }

  public SecureRandomAlgorithm Random => this.m_random;

  public ICipherParam Parameters => this.m_parameters;
}
