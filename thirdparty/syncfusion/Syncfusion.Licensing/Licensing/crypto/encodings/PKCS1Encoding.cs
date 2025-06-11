// Decompiled with JetBrains decompiler
// Type: Syncfusion.Licensing.crypto.encodings.PKCS1Encoding
// Assembly: Syncfusion.Licensing, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=632609b4d040f6b4
// MVID: 46253E3A-52AF-49F3-BF00-D811A33B7BC6
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Licensing.dll

using Syncfusion.Licensing.crypto.parameters;
using Syncfusion.Licensing.security;
using System;
using System.ComponentModel;

#nullable disable
namespace Syncfusion.Licensing.crypto.encodings;

[EditorBrowsable(EditorBrowsableState.Never)]
public class PKCS1Encoding : AsymmetricBlockCipher
{
  private static int HEADER_LENGTH = 10;
  private SecureRandom random;
  private AsymmetricBlockCipher engine;
  private bool forEncryption;
  private bool forPrivateKey;

  public PKCS1Encoding(AsymmetricBlockCipher cipher) => this.engine = cipher;

  public void init(bool forEncryption, CipherParameters param)
  {
    AsymmetricKeyParameter asymmetricKeyParameter;
    if (typeof (ParametersWithRandom).IsInstanceOfType((object) param))
    {
      ParametersWithRandom parametersWithRandom = (ParametersWithRandom) param;
      this.random = parametersWithRandom.getRandom();
      asymmetricKeyParameter = (AsymmetricKeyParameter) parametersWithRandom.getParameters();
    }
    else
    {
      this.random = new SecureRandom();
      asymmetricKeyParameter = (AsymmetricKeyParameter) param;
    }
    this.engine.init(forEncryption, (CipherParameters) asymmetricKeyParameter);
    this.forPrivateKey = asymmetricKeyParameter.isPrivate();
    this.forEncryption = forEncryption;
  }

  public int getInputBlockSize()
  {
    int inputBlockSize = this.engine.getInputBlockSize();
    return this.forEncryption ? inputBlockSize - PKCS1Encoding.HEADER_LENGTH : inputBlockSize;
  }

  public int getOutputBlockSize()
  {
    int outputBlockSize = this.engine.getOutputBlockSize();
    return this.forEncryption ? outputBlockSize : outputBlockSize - PKCS1Encoding.HEADER_LENGTH;
  }

  public byte[] processBlock(byte[] inBytes, int inOff, int inLen)
  {
    return this.forEncryption ? this.encodeBlock(inBytes, inOff, inLen) : this.decodeBlock(inBytes, inOff, inLen);
  }

  private byte[] encodeBlock(byte[] inBytes, int inOff, int inLen)
  {
    byte[] numArray = new byte[this.engine.getInputBlockSize()];
    if (this.forPrivateKey)
    {
      numArray[0] = (byte) 1;
      for (int index = 1; index != numArray.Length - inLen - 1; ++index)
        numArray[index] = byte.MaxValue;
    }
    else
    {
      this.random.nextBytes(numArray);
      numArray[0] = (byte) 2;
      for (int index = 1; index != numArray.Length - inLen - 1; ++index)
      {
        while (numArray[index] == (byte) 0)
          numArray[index] = (byte) this.random.nextInt();
      }
    }
    numArray[numArray.Length - inLen - 1] = (byte) 0;
    Array.Copy((Array) inBytes, inOff, (Array) numArray, numArray.Length - inLen, inLen);
    return this.engine.processBlock(numArray, 0, numArray.Length);
  }

  private byte[] decodeBlock(byte[] inBytes, int inOff, int inLen)
  {
    byte[] sourceArray = this.engine.processBlock(inBytes, inOff, inLen);
    if (sourceArray.Length < this.getOutputBlockSize())
      throw new InvalidCipherTextException("block truncated");
    if (sourceArray[0] != (byte) 1 && sourceArray[0] != (byte) 2)
      throw new InvalidCipherTextException("unknown block type");
    int index = 1;
    while (index != sourceArray.Length && sourceArray[index] != (byte) 0)
      ++index;
    int sourceIndex = index + 1;
    if (sourceIndex >= sourceArray.Length || sourceIndex < PKCS1Encoding.HEADER_LENGTH)
      throw new InvalidCipherTextException("no data in block");
    byte[] destinationArray = new byte[sourceArray.Length - sourceIndex];
    Array.Copy((Array) sourceArray, sourceIndex, (Array) destinationArray, 0, destinationArray.Length);
    return destinationArray;
  }
}
