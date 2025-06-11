// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.PKCS12AlgorithmGenerator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class PKCS12AlgorithmGenerator : PasswordGenerator
{
  private const int m_keyMaterial = 1;
  private const int m_invaidMaterial = 2;
  private const int m_macMaterial = 3;
  private IMessageDigest m_digest;
  private int m_size;
  private int m_length;

  internal PKCS12AlgorithmGenerator(IMessageDigest digest)
  {
    this.m_digest = digest;
    this.m_size = digest.MessageDigestSize;
    this.m_length = digest.ByteLength;
  }

  private void Adjust(byte[] a, int offset, byte[] b)
  {
    int num1 = ((int) b[b.Length - 1] & (int) byte.MaxValue) + ((int) a[offset + b.Length - 1] & (int) byte.MaxValue) + 1;
    a[offset + b.Length - 1] = (byte) num1;
    int num2 = num1 >>> 8;
    for (int index = b.Length - 2; index >= 0; --index)
    {
      int num3 = num2 + (((int) b[index] & (int) byte.MaxValue) + ((int) a[offset + index] & (int) byte.MaxValue));
      a[offset + index] = (byte) num3;
      num2 = num3 >>> 8;
    }
  }

  private byte[] GenerateDerivedKey(int id, int length)
  {
    byte[] bytes = new byte[this.m_length];
    byte[] destinationArray = new byte[length];
    for (int index = 0; index != bytes.Length; ++index)
      bytes[index] = (byte) id;
    byte[] sourceArray1;
    if (this.m_value != null && this.m_value.Length != 0)
    {
      sourceArray1 = new byte[this.m_length * ((this.m_value.Length + this.m_length - 1) / this.m_length)];
      for (int index = 0; index != sourceArray1.Length; ++index)
        sourceArray1[index] = this.m_value[index % this.m_value.Length];
    }
    else
      sourceArray1 = new byte[0];
    byte[] sourceArray2;
    if (this.m_password != null && this.m_password.Length != 0)
    {
      sourceArray2 = new byte[this.m_length * ((this.m_password.Length + this.m_length - 1) / this.m_length)];
      for (int index = 0; index != sourceArray2.Length; ++index)
        sourceArray2[index] = this.m_password[index % this.m_password.Length];
    }
    else
      sourceArray2 = new byte[0];
    byte[] numArray1 = new byte[sourceArray1.Length + sourceArray2.Length];
    Array.Copy((Array) sourceArray1, 0, (Array) numArray1, 0, sourceArray1.Length);
    Array.Copy((Array) sourceArray2, 0, (Array) numArray1, sourceArray1.Length, sourceArray2.Length);
    byte[] b = new byte[this.m_length];
    int num = (length + this.m_size - 1) / this.m_size;
    byte[] numArray2 = new byte[this.m_size];
    for (int index1 = 1; index1 <= num; ++index1)
    {
      this.m_digest.Update(bytes, 0, bytes.Length);
      this.m_digest.Update(numArray1, 0, numArray1.Length);
      this.m_digest.DoFinal(numArray2, 0);
      for (int index2 = 1; index2 != this.m_count; ++index2)
      {
        this.m_digest.Update(numArray2, 0, numArray2.Length);
        this.m_digest.DoFinal(numArray2, 0);
      }
      for (int index3 = 0; index3 != b.Length; ++index3)
        b[index3] = numArray2[index3 % numArray2.Length];
      for (int index4 = 0; index4 != numArray1.Length / this.m_length; ++index4)
        this.Adjust(numArray1, index4 * this.m_length, b);
      if (index1 == num)
        Array.Copy((Array) numArray2, 0, (Array) destinationArray, (index1 - 1) * this.m_size, destinationArray.Length - (index1 - 1) * this.m_size);
      else
        Array.Copy((Array) numArray2, 0, (Array) destinationArray, (index1 - 1) * this.m_size, numArray2.Length);
    }
    return destinationArray;
  }

  internal override ICipherParam GenerateParam(string algorithm, int keySize)
  {
    keySize /= 8;
    byte[] derivedKey = this.GenerateDerivedKey(1, keySize);
    return (ICipherParam) new ParamUtility().CreateKeyParameter(algorithm, derivedKey, 0, keySize);
  }

  internal override ICipherParam GenerateParam(string algorithm, int keySize, int ivSize)
  {
    keySize /= 8;
    ivSize /= 8;
    byte[] derivedKey = this.GenerateDerivedKey(1, keySize);
    return (ICipherParam) new InvalidParameter((ICipherParam) new ParamUtility().CreateKeyParameter(algorithm, derivedKey, 0, keySize), this.GenerateDerivedKey(2, ivSize), 0, ivSize);
  }

  internal override ICipherParam GenerateParam(int keySize)
  {
    keySize /= 8;
    return (ICipherParam) new KeyParameter(this.GenerateDerivedKey(3, keySize), 0, keySize);
  }
}
