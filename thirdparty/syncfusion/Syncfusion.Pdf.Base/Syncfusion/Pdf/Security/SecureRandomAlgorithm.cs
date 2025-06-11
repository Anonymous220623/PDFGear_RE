// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.SecureRandomAlgorithm
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class SecureRandomAlgorithm : Random
{
  private static readonly IAlgorithmGenerator m_sha1Generator = (IAlgorithmGenerator) new DigestAlgorithmGenerator((IMessageDigest) new SHA1MessageDigest());
  private static readonly IAlgorithmGenerator m_sha256Generator = (IAlgorithmGenerator) new DigestAlgorithmGenerator((IMessageDigest) new SHA256MessageDigest());
  private SecureRandomAlgorithm[] m_master = new SecureRandomAlgorithm[1];
  private double DoubleScale = Math.Pow(2.0, 64.0);
  protected IAlgorithmGenerator m_generator;

  private SecureRandomAlgorithm Algorithm
  {
    get
    {
      if (this.m_master[0] == null)
      {
        SecureRandomAlgorithm secureRandomAlgorithm = this.m_master[0] = new SecureRandomAlgorithm((IAlgorithmGenerator) new RWAlgorithmGenerator(SecureRandomAlgorithm.m_sha256Generator, 32 /*0x20*/));
        secureRandomAlgorithm.SetBytes(DateTime.Now.Ticks);
        secureRandomAlgorithm.GenerateBytes(1 + secureRandomAlgorithm.Next(32 /*0x20*/));
      }
      return this.m_master[0];
    }
  }

  internal byte[] GetBytes(int length) => this.Algorithm.GenerateBytes(length);

  internal SecureRandomAlgorithm()
    : this(SecureRandomAlgorithm.m_sha1Generator)
  {
    this.SetBytes(this.GetBytes(8));
  }

  internal SecureRandomAlgorithm(IAlgorithmGenerator generator)
    : base(0)
  {
    this.m_generator = generator;
  }

  public virtual byte[] GenerateBytes(int length)
  {
    this.SetBytes(DateTime.Now.Ticks);
    byte[] buffer = new byte[length];
    this.NextBytes(buffer);
    return buffer;
  }

  public virtual void SetBytes(byte[] bytes) => this.m_generator.AddMaterial(bytes);

  public virtual void SetBytes(long value) => this.m_generator.AddMaterial(value);

  public override int Next()
  {
    int num;
    do
    {
      num = this.NextInt() & int.MaxValue;
    }
    while (num == int.MaxValue);
    return num;
  }

  public override int Next(int maxValue)
  {
    if (maxValue < 2)
    {
      if (maxValue < 0)
        throw new ArgumentOutOfRangeException(nameof (maxValue));
      return 0;
    }
    if ((maxValue & -maxValue) == maxValue)
    {
      int num = this.NextInt() & int.MaxValue;
      return (int) ((long) maxValue * (long) num >> 31 /*0x1F*/);
    }
    int num1;
    int num2;
    do
    {
      num1 = this.NextInt() & int.MaxValue;
      num2 = num1 % maxValue;
    }
    while (num1 - num2 + (maxValue - 1) < 0);
    return num2;
  }

  public override int Next(int minValue, int maxValue)
  {
    if (maxValue <= minValue)
      return maxValue == minValue ? minValue : throw new ArgumentException("Invalid max value");
    int maxValue1 = maxValue - minValue;
    if (maxValue1 > 0)
      return minValue + this.Next(maxValue1);
    int num;
    do
    {
      num = this.NextInt();
    }
    while (num < minValue || num >= maxValue);
    return num;
  }

  public override void NextBytes(byte[] buffer) => this.m_generator.FillNextBytes(buffer);

  public virtual void NextBytes(byte[] buffer, int start, int length)
  {
    this.m_generator.FillNextBytes(buffer, start, length);
  }

  public override double NextDouble()
  {
    return Convert.ToDouble((ulong) this.NextLong()) / this.DoubleScale;
  }

  public virtual int NextInt()
  {
    byte[] buffer = new byte[4];
    this.NextBytes(buffer);
    int num = 0;
    for (int index = 0; index < 4; ++index)
      num = (num << 8) + ((int) buffer[index] & (int) byte.MaxValue);
    return num;
  }

  public virtual long NextLong()
  {
    return (long) (uint) this.NextInt() << 32 /*0x20*/ | (long) (uint) this.NextInt();
  }
}
