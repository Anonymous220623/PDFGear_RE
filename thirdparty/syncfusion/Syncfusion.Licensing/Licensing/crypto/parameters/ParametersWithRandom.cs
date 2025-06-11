// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.crypto.parameters.ParametersWithRandom
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using Syncfusion.Licensing.security;
using System.ComponentModel;

#nullable disable
namespace Syncfusion.Licensing.crypto.parameters;

[EditorBrowsable(EditorBrowsableState.Never)]
public class ParametersWithRandom : CipherParameters
{
  private SecureRandom random;
  private CipherParameters parameters;

  public ParametersWithRandom(CipherParameters parameters, SecureRandom random)
  {
    this.random = random;
    this.parameters = parameters;
  }

  public ParametersWithRandom(CipherParameters parameters)
  {
    this.random = (SecureRandom) null;
    this.parameters = parameters;
  }

  public SecureRandom getRandom()
  {
    if (this.random == null)
      this.random = new SecureRandom();
    return this.random;
  }

  public CipherParameters getParameters() => this.parameters;
}
