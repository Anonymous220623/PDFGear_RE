// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.crypto.engines.RSAEngine
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using Syncfusion.Licensing.crypto.parameters;
using Syncfusion.Licensing.math;
using System;
using System.ComponentModel;

#nullable disable
namespace Syncfusion.Licensing.crypto.engines;

[EditorBrowsable(EditorBrowsableState.Never)]
public class RSAEngine : AsymmetricBlockCipher
{
  private RSAKeyParameters key;
  private bool forEncryption;

  public string getAlgorithmName() => "RSA";

  public void init(bool forEncryption, CipherParameters param)
  {
    this.key = (RSAKeyParameters) param;
    this.forEncryption = forEncryption;
  }

  public int getInputBlockSize()
  {
    int num = this.key.getModulus().bitLength();
    return this.forEncryption ? (num + 7) / 8 - 1 : (num + 7) / 8;
  }

  public int getOutputBlockSize()
  {
    int num = this.key.getModulus().bitLength();
    return this.forEncryption ? (num + 7) / 8 : (num + 7) / 8 - 1;
  }

  public byte[] processBlock(byte[] inBytes, int inOff, int inLen)
  {
    if (inLen > this.getInputBlockSize() + 1)
      throw new DataLengthException("input too large for RSA cipher.\n");
    if (inLen == this.getInputBlockSize() + 1 && ((int) inBytes[inOff] & 128 /*0x80*/) != 0)
      throw new DataLengthException("input too large for RSA cipher.\n");
    byte[] numArray;
    if (inOff != 0 || inLen != inBytes.Length)
    {
      numArray = new byte[inLen];
      Array.Copy((Array) inBytes, inOff, (Array) numArray, 0, inLen);
    }
    else
      numArray = inBytes;
    BigInteger bigInteger1 = new BigInteger(1, numArray);
    byte[] byteArray;
    if (typeof (RSAPrivateCrtKeyParameters).IsInstanceOfType((object) this.key))
    {
      RSAPrivateCrtKeyParameters key = (RSAPrivateCrtKeyParameters) this.key;
      BigInteger p = key.getP();
      BigInteger q = key.getQ();
      BigInteger dp = key.getDP();
      BigInteger dq = key.getDQ();
      BigInteger qinv = key.getQInv();
      BigInteger bigInteger2 = bigInteger1.remainder(p).modPow(dp, p);
      BigInteger val = bigInteger1.remainder(q).modPow(dq, q);
      byteArray = bigInteger2.subtract(val).multiply(qinv).mod(p).multiply(q).add(val).toByteArray();
    }
    else
      byteArray = bigInteger1.modPow(this.key.getExponent(), this.key.getModulus()).toByteArray();
    if (this.forEncryption)
    {
      if (byteArray[0] == (byte) 0 && byteArray.Length > this.getOutputBlockSize())
      {
        byte[] destinationArray = new byte[byteArray.Length - 1];
        Array.Copy((Array) byteArray, 1, (Array) destinationArray, 0, destinationArray.Length);
        return destinationArray;
      }
      if (byteArray.Length < this.getOutputBlockSize())
      {
        byte[] destinationArray = new byte[this.getOutputBlockSize()];
        Array.Copy((Array) byteArray, 0, (Array) destinationArray, destinationArray.Length - byteArray.Length, byteArray.Length);
        return destinationArray;
      }
    }
    else if (byteArray[0] == (byte) 0)
    {
      byte[] destinationArray = new byte[byteArray.Length - 1];
      Array.Copy((Array) byteArray, 1, (Array) destinationArray, 0, destinationArray.Length);
      return destinationArray;
    }
    return byteArray;
  }
}
