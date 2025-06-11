// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Pkcs1Encoding
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Pkcs1Encoding : ICipherBlock
{
  private const string m_enableSequrity = "Syncfusion.Pdf.Security";
  private const int m_length = 10;
  private static readonly bool[] m_securityLengthEnabled;
  private SecureRandomAlgorithm m_random;
  private ICipherBlock m_cipher;
  private bool m_isEncryption;
  private bool m_isPrivateKey;
  private bool m_useSecurityLength;

  internal bool SecurityLengthEnabled
  {
    get => Pkcs1Encoding.m_securityLengthEnabled[0];
    set => Pkcs1Encoding.m_securityLengthEnabled[0] = value;
  }

  public string AlgorithmName => this.m_cipher.AlgorithmName + "/PKCS1Padding";

  static Pkcs1Encoding()
  {
    string environmentVariable = Environment.GetEnvironmentVariable("Syncfusion.Pdf.Security");
    Pkcs1Encoding.m_securityLengthEnabled = new bool[1]
    {
      environmentVariable == null || environmentVariable.Equals("true")
    };
  }

  internal Pkcs1Encoding(ICipherBlock cipher)
  {
    this.m_cipher = cipher;
    this.m_useSecurityLength = this.SecurityLengthEnabled;
  }

  public ICipherBlock GetUnderlyingCipher() => this.m_cipher;

  public void Initialize(bool forEncryption, ICipherParam parameters)
  {
    this.m_random = new SecureRandomAlgorithm();
    CipherParameter cipherParameter = (CipherParameter) parameters;
    this.m_cipher.Initialize(forEncryption, parameters);
    this.m_isPrivateKey = cipherParameter.IsPrivate;
    this.m_isEncryption = forEncryption;
  }

  public int InputBlock
  {
    get
    {
      int inputBlock = this.m_cipher.InputBlock;
      return !this.m_isEncryption ? inputBlock : inputBlock - 10;
    }
  }

  public int OutputBlock
  {
    get
    {
      int outputBlock = this.m_cipher.OutputBlock;
      return !this.m_isEncryption ? outputBlock - 10 : outputBlock;
    }
  }

  public byte[] ProcessBlock(byte[] input, int inOff, int length)
  {
    return !this.m_isEncryption ? this.DecodeBlock(input, inOff, length) : this.EncodeBlock(input, inOff, length);
  }

  private byte[] EncodeBlock(byte[] input, int inOff, int inLen)
  {
    if (inLen > this.InputBlock)
      throw new ArgumentException("Input data too large");
    byte[] numArray = new byte[this.m_cipher.InputBlock];
    if (this.m_isPrivateKey)
    {
      numArray[0] = (byte) 1;
      for (int index = 1; index != numArray.Length - inLen - 1; ++index)
        numArray[index] = byte.MaxValue;
    }
    else
    {
      this.m_random.NextBytes(numArray);
      numArray[0] = (byte) 2;
      for (int index = 1; index != numArray.Length - inLen - 1; ++index)
      {
        while (numArray[index] == (byte) 0)
          numArray[index] = (byte) this.m_random.NextInt();
      }
    }
    numArray[numArray.Length - inLen - 1] = (byte) 0;
    Array.Copy((Array) input, inOff, (Array) numArray, numArray.Length - inLen, inLen);
    return this.m_cipher.ProcessBlock(numArray, 0, numArray.Length);
  }

  private byte[] DecodeBlock(byte[] input, int inOff, int inLen)
  {
    byte[] sourceArray = this.m_cipher.ProcessBlock(input, inOff, inLen);
    if (sourceArray.Length < this.OutputBlock)
      throw new Exception("Invalid block. Block truncated");
    byte num1 = sourceArray[0];
    switch (num1)
    {
      case 1:
      case 2:
        if (this.m_useSecurityLength && sourceArray.Length != this.m_cipher.OutputBlock)
          throw new Exception("Invalid size");
        int index;
        for (index = 1; index != sourceArray.Length; ++index)
        {
          byte num2 = sourceArray[index];
          if (num2 != (byte) 0)
          {
            if (num1 == (byte) 1 && num2 != byte.MaxValue)
              throw new Exception("Invalid block padding");
          }
          else
            break;
        }
        int sourceIndex = index + 1;
        if (sourceIndex > sourceArray.Length || sourceIndex < 10)
          throw new Exception("no data in block");
        byte[] destinationArray = new byte[sourceArray.Length - sourceIndex];
        Array.Copy((Array) sourceArray, sourceIndex, (Array) destinationArray, 0, destinationArray.Length);
        return destinationArray;
      default:
        throw new Exception("Invalid block type");
    }
  }
}
