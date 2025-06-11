// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DesEdeAlogorithm
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DesEdeAlogorithm : DataEncryption
{
  private int[] m_key1;
  private int[] m_Key2;
  private int[] m_Key3;
  private bool m_isEncryption;

  public override void Initialize(bool forEncryption, ICipherParam parameters)
  {
    byte[] sourceArray = parameters is KeyParameter ? ((KeyParameter) parameters).Keys : throw new ArgumentException("Invalid parameter");
    if (sourceArray.Length != 24 && sourceArray.Length != 16 /*0x10*/)
      throw new ArgumentException("Invalid key size. Size must be 16 or 24 bytes.");
    this.m_isEncryption = forEncryption;
    byte[] numArray1 = new byte[8];
    Array.Copy((Array) sourceArray, 0, (Array) numArray1, 0, numArray1.Length);
    this.m_key1 = DataEncryption.GenerateWorkingKey(forEncryption, numArray1);
    byte[] numArray2 = new byte[8];
    Array.Copy((Array) sourceArray, 8, (Array) numArray2, 0, numArray2.Length);
    this.m_Key2 = DataEncryption.GenerateWorkingKey(!forEncryption, numArray2);
    if (sourceArray.Length == 24)
    {
      byte[] numArray3 = new byte[8];
      Array.Copy((Array) sourceArray, 16 /*0x10*/, (Array) numArray3, 0, numArray3.Length);
      this.m_Key3 = DataEncryption.GenerateWorkingKey(forEncryption, numArray3);
    }
    else
      this.m_Key3 = this.m_key1;
  }

  public override int ProcessBlock(
    byte[] inputBytes,
    int inOffset,
    byte[] outputBytes,
    int outOffset)
  {
    if (this.m_key1 == null)
      throw new InvalidOperationException("Data Encryption Standard - Encrypt Decrypt Encrypt not initialised");
    if (inOffset + 8 > inputBytes.Length)
      throw new Exception("Invalid length in input bytes");
    if (outOffset + 8 > outputBytes.Length)
      throw new Exception("Invalid length in output bytes");
    byte[] numArray = new byte[8];
    if (this.m_isEncryption)
    {
      DataEncryption.EncryptData(this.m_key1, inputBytes, inOffset, numArray, 0);
      DataEncryption.EncryptData(this.m_Key2, numArray, 0, numArray, 0);
      DataEncryption.EncryptData(this.m_Key3, numArray, 0, outputBytes, outOffset);
    }
    else
    {
      DataEncryption.EncryptData(this.m_Key3, inputBytes, inOffset, numArray, 0);
      DataEncryption.EncryptData(this.m_Key2, numArray, 0, numArray, 0);
      DataEncryption.EncryptData(this.m_key1, numArray, 0, outputBytes, outOffset);
    }
    return 8;
  }

  public override void Reset()
  {
  }

  public override int BlockSize => 8;

  public override string AlgorithmName => "DESede";
}
