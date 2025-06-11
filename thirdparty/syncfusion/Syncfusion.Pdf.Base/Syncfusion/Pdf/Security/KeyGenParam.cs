// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.KeyGenParam
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class KeyGenParam
{
  private SecureRandomAlgorithm randomNumber;
  private int strength;

  internal KeyGenParam(SecureRandomAlgorithm randomNumber, int strength)
  {
    if (randomNumber == null)
      throw new ArgumentNullException(nameof (randomNumber));
    if (strength < 1)
      throw new ArgumentException("Must be a positive value");
    this.randomNumber = randomNumber;
    this.strength = strength;
  }

  internal SecureRandomAlgorithm Random => this.randomNumber;

  internal int Strength => this.strength;
}
