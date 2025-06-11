// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.PublicKeyDecrypt
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using Syncfusion.Licensing.crypto;
using Syncfusion.Licensing.crypto.encodings;
using Syncfusion.Licensing.crypto.engines;
using Syncfusion.Licensing.crypto.parameters;
using Syncfusion.Licensing.math;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace Syncfusion.Licensing;

[EditorBrowsable(EditorBrowsableState.Never)]
public class PublicKeyDecrypt
{
  internal static RSAKeyParameters pubParameters = new RSAKeyParameters(false, new BigInteger("74311111206657030479635290206242922294134948120327310989730425327779900016841"), new BigInteger("17"));

  public static byte[] SyncfusionDecode(string key)
  {
    return PublicKeyDecrypt.DecodeLongBuffer(key.Trim(), PublicKeyDecrypt.pubParameters);
  }

  private static byte[] DecodeLongBuffer(string encryptedLongKey, RSAKeyParameters pubParameters)
  {
    return PublicKeyDecrypt.DecodeLongBuffer(Convert.FromBase64String(encryptedLongKey), pubParameters);
  }

  private static byte[] DecodeLongBuffer(byte[] buffer, RSAKeyParameters pubParameters)
  {
    PKCS1Encoding pkcS1Encoding = new PKCS1Encoding((AsymmetricBlockCipher) new RSAEngine());
    pkcS1Encoding.init(false, (CipherParameters) pubParameters);
    try
    {
      ArrayList arrayList1 = new ArrayList();
      int inputBlockSize = pkcS1Encoding.getInputBlockSize();
      int num = buffer.Length / inputBlockSize;
      ArrayList arrayList2 = new ArrayList();
      for (int index = 0; index < num; ++index)
      {
        int inOff = index * inputBlockSize;
        int inLen = Math.Min(buffer.Length - inOff, inputBlockSize);
        byte[] sourceArray = pkcS1Encoding.processBlock(buffer, inOff, inLen);
        byte[] numArray = new byte[(int) sourceArray[0]];
        Array.Copy((Array) sourceArray, 1, (Array) numArray, 0, numArray.Length);
        arrayList1.AddRange((ICollection) numArray);
      }
      byte[] numArray1 = new byte[arrayList1.Count];
      arrayList1.CopyTo((Array) numArray1);
      return numArray1;
    }
    catch (Exception ex)
    {
      Debug.WriteLine("RSA: failed - exception " + ex.ToString());
      return new byte[0];
    }
  }
}
