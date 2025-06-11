// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.WriteProtection
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Syncfusion.Presentation;

internal class WriteProtection
{
  private const int SpinCount = 100000;
  private Dictionary<string, string> m_attributes;

  internal Dictionary<string, string> Attributes
  {
    get
    {
      if (this.m_attributes == null)
        this.m_attributes = new Dictionary<string, string>();
      return this.m_attributes;
    }
  }

  internal bool IsWriteProtected => this.m_attributes != null && this.m_attributes.Count != 0;

  internal void SetWriteProtection(string password)
  {
    this.RemoveWriteProtection();
    this.Attributes.Add("cryptProviderType", "rsaAES");
    this.Attributes.Add("cryptAlgorithmClass", "hash");
    this.Attributes.Add("cryptAlgorithmType", "typeAny");
    this.Attributes.Add("cryptAlgorithmSid", "14");
    this.Attributes.Add("spinCount", 100000.ToString());
    byte[] salt = this.CreateSalt(16 /*0x10*/);
    this.Attributes.Add("saltData", Convert.ToBase64String(salt));
    this.Attributes.Add("hashData", Convert.ToBase64String(this.ComputeHash(salt, password)));
  }

  internal void RemoveWriteProtection()
  {
    if (this.m_attributes == null)
      return;
    this.m_attributes.Clear();
  }

  private byte[] CreateSalt(int length)
  {
    byte[] salt = length > 0 ? new byte[length] : throw new ArgumentOutOfRangeException(nameof (length));
    Random random = new Random((int) DateTime.Now.Ticks);
    int maxValue = 256 /*0x0100*/;
    for (int index = 0; index < length; ++index)
      salt[index] = (byte) random.Next(maxValue);
    return salt;
  }

  private byte[] ComputeHash(byte[] salt, string password)
  {
    if (password.Length > 15)
      password = password.Substring(0, 15);
    byte[] bytes1 = Encoding.Unicode.GetBytes(password);
    for (int index = 0; index < bytes1.Length; ++index)
    {
      if (bytes1[index] > byte.MaxValue)
        bytes1[index] = byte.MaxValue;
    }
    password = Encoding.Unicode.GetString(bytes1);
    password = password.Trim().Trim('\uFEFF');
    byte[] bytes2 = Encoding.Unicode.GetBytes(password);
    byte[] buffer1 = this.CombineByteArrays(salt, bytes2);
    SHA512 shA512 = (SHA512) new SHA512CryptoServiceProvider();
    byte[] hash = shA512.ComputeHash(buffer1);
    byte[] buffer2 = new byte[68];
    for (int index1 = 0; index1 < 100000; ++index1)
    {
      hash.CopyTo((Array) buffer2, 0);
      int num = index1;
      for (int index2 = 0; index2 < 4; ++index2)
      {
        buffer2[hash.Length + index2] = (byte) num;
        num >>= 8;
      }
      hash = shA512.ComputeHash(buffer2);
    }
    return hash;
  }

  private byte[] CombineByteArrays(byte[] array1, byte[] array2)
  {
    byte[] dst = new byte[array1.Length + array2.Length];
    Buffer.BlockCopy((Array) array1, 0, (Array) dst, 0, array1.Length);
    Buffer.BlockCopy((Array) array2, 0, (Array) dst, array1.Length, array2.Length);
    return dst;
  }
}
