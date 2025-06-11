// Decompiled with JetBrains decompiler
// Type: Patagames.Activation.AsnKeyParser
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace Patagames.Activation;

internal class AsnKeyParser
{
  private AsnParser parser;

  internal AsnKeyParser(string pathname)
  {
    using (BinaryReader binaryReader = new BinaryReader((Stream) new FileStream(pathname, FileMode.Open, FileAccess.Read)))
    {
      FileInfo fileInfo = new FileInfo(pathname);
      this.parser = new AsnParser(binaryReader.ReadBytes((int) fileInfo.Length));
    }
  }

  internal AsnKeyParser(byte[] keydata) => this.parser = new AsnParser(keydata);

  internal static byte[] TrimLeadingZero(byte[] values)
  {
    byte[] destinationArray;
    if (values[0] == (byte) 0 && values.Length > 1)
    {
      destinationArray = new byte[values.Length - 1];
      Array.Copy((Array) values, 1, (Array) destinationArray, 0, values.Length - 1);
    }
    else
    {
      destinationArray = new byte[values.Length];
      Array.Copy((Array) values, (Array) destinationArray, values.Length);
    }
    return destinationArray;
  }

  internal static bool EqualOid(byte[] first, byte[] second)
  {
    if (first.Length != second.Length)
      return false;
    for (int index = 0; index < first.Length; ++index)
    {
      if ((int) first[index] != (int) second[index])
        return false;
    }
    return true;
  }

  internal RSAParameters ParseRSAPublicKey()
  {
    RSAParameters rsaPublicKey = new RSAParameters();
    byte[] numArray = (byte[]) null;
    int position1 = this.parser.CurrentPosition();
    int num1 = this.parser.NextSequence();
    if (num1 != this.parser.RemainingBytes())
    {
      StringBuilder stringBuilder = new StringBuilder("Incorrect Sequence Size. ");
      stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) num1.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.parser.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      throw new BerDecodeException(stringBuilder.ToString(), position1);
    }
    int position2 = this.parser.CurrentPosition();
    int num2 = this.parser.NextSequence();
    if (num2 > this.parser.RemainingBytes())
    {
      StringBuilder stringBuilder = new StringBuilder("Incorrect AlgorithmIdentifier Size. ");
      stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) num2.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.parser.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      throw new BerDecodeException(stringBuilder.ToString(), position2);
    }
    int position3 = this.parser.CurrentPosition();
    if (!AsnKeyParser.EqualOid(this.parser.NextOID(), new byte[9]
    {
      (byte) 42,
      (byte) 134,
      (byte) 72,
      (byte) 134,
      (byte) 247,
      (byte) 13,
      (byte) 1,
      (byte) 1,
      (byte) 1
    }))
      throw new BerDecodeException("Expected OID 1.2.840.113549.1.1.1", position3);
    if (this.parser.IsNextNull())
      this.parser.NextNull();
    else
      numArray = this.parser.Next();
    int position4 = this.parser.CurrentPosition();
    int num3 = this.parser.NextBitString();
    if (num3 > this.parser.RemainingBytes())
    {
      StringBuilder stringBuilder = new StringBuilder("Incorrect PublicKey Size. ");
      stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) num3.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.parser.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      throw new BerDecodeException(stringBuilder.ToString(), position4);
    }
    int position5 = this.parser.CurrentPosition();
    int num4 = this.parser.NextSequence();
    if (num4 < this.parser.RemainingBytes())
    {
      StringBuilder stringBuilder = new StringBuilder("Incorrect RSAPublicKey Size. ");
      stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) num4.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.parser.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      throw new BerDecodeException(stringBuilder.ToString(), position5);
    }
    rsaPublicKey.Modulus = AsnKeyParser.TrimLeadingZero(this.parser.NextInteger());
    rsaPublicKey.Exponent = AsnKeyParser.TrimLeadingZero(this.parser.NextInteger());
    return rsaPublicKey;
  }

  internal RSAParameters ParseRSAPrivateKey()
  {
    RSAParameters rsaPrivateKey = new RSAParameters();
    byte[] numArray = (byte[]) null;
    int position1 = this.parser.CurrentPosition();
    int num1 = this.parser.NextSequence();
    if (num1 != this.parser.RemainingBytes())
    {
      StringBuilder stringBuilder = new StringBuilder("Incorrect Sequence Size. ");
      stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) num1.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.parser.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      throw new BerDecodeException(stringBuilder.ToString(), position1);
    }
    int position2 = this.parser.CurrentPosition();
    byte[] inData1 = this.parser.NextInteger();
    if (inData1[0] != (byte) 0)
    {
      StringBuilder stringBuilder = new StringBuilder("Incorrect PrivateKeyInfo Version. ");
      stringBuilder.AppendFormat("Expected: 0, Specified: {0}", (object) new BigInteger(inData1).ToString(10));
      throw new BerDecodeException(stringBuilder.ToString(), position2);
    }
    int position3 = this.parser.CurrentPosition();
    int num2 = this.parser.NextSequence();
    if (num2 > this.parser.RemainingBytes())
    {
      StringBuilder stringBuilder = new StringBuilder("Incorrect AlgorithmIdentifier Size. ");
      stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) num2.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.parser.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      throw new BerDecodeException(stringBuilder.ToString(), position3);
    }
    int position4 = this.parser.CurrentPosition();
    if (!AsnKeyParser.EqualOid(this.parser.NextOID(), new byte[9]
    {
      (byte) 42,
      (byte) 134,
      (byte) 72,
      (byte) 134,
      (byte) 247,
      (byte) 13,
      (byte) 1,
      (byte) 1,
      (byte) 1
    }))
      throw new BerDecodeException("Expected OID 1.2.840.113549.1.1.1", position4);
    if (this.parser.IsNextNull())
      this.parser.NextNull();
    else
      numArray = this.parser.Next();
    int position5 = this.parser.CurrentPosition();
    int num3 = this.parser.NextOctetString();
    if (num3 > this.parser.RemainingBytes())
    {
      StringBuilder stringBuilder = new StringBuilder("Incorrect PrivateKey Size. ");
      stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) num3.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.parser.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      throw new BerDecodeException(stringBuilder.ToString(), position5);
    }
    int position6 = this.parser.CurrentPosition();
    int num4 = this.parser.NextSequence();
    if (num4 < this.parser.RemainingBytes())
    {
      StringBuilder stringBuilder = new StringBuilder("Incorrect RSAPrivateKey Size. ");
      stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) num4.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.parser.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      throw new BerDecodeException(stringBuilder.ToString(), position6);
    }
    int position7 = this.parser.CurrentPosition();
    byte[] inData2 = this.parser.NextInteger();
    if (inData2[0] != (byte) 0)
    {
      StringBuilder stringBuilder = new StringBuilder("Incorrect RSAPrivateKey Version. ");
      stringBuilder.AppendFormat("Expected: 0, Specified: {0}", (object) new BigInteger(inData2).ToString(10));
      throw new BerDecodeException(stringBuilder.ToString(), position7);
    }
    rsaPrivateKey.Modulus = AsnKeyParser.TrimLeadingZero(this.parser.NextInteger());
    rsaPrivateKey.Exponent = AsnKeyParser.TrimLeadingZero(this.parser.NextInteger());
    rsaPrivateKey.D = AsnKeyParser.TrimLeadingZero(this.parser.NextInteger());
    rsaPrivateKey.P = AsnKeyParser.TrimLeadingZero(this.parser.NextInteger());
    rsaPrivateKey.Q = AsnKeyParser.TrimLeadingZero(this.parser.NextInteger());
    rsaPrivateKey.DP = AsnKeyParser.TrimLeadingZero(this.parser.NextInteger());
    rsaPrivateKey.DQ = AsnKeyParser.TrimLeadingZero(this.parser.NextInteger());
    rsaPrivateKey.InverseQ = AsnKeyParser.TrimLeadingZero(this.parser.NextInteger());
    return rsaPrivateKey;
  }

  internal DSAParameters ParseDSAPublicKey()
  {
    DSAParameters dsaPublicKey = new DSAParameters();
    int position1 = this.parser.CurrentPosition();
    int num1 = this.parser.NextSequence();
    if (num1 != this.parser.RemainingBytes())
    {
      StringBuilder stringBuilder = new StringBuilder("Incorrect Sequence Size. ");
      stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) num1.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.parser.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      throw new BerDecodeException(stringBuilder.ToString(), position1);
    }
    int position2 = this.parser.CurrentPosition();
    int num2 = this.parser.NextSequence();
    if (num2 > this.parser.RemainingBytes())
    {
      StringBuilder stringBuilder = new StringBuilder("Incorrect AlgorithmIdentifier Size. ");
      stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) num2.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.parser.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      throw new BerDecodeException(stringBuilder.ToString(), position2);
    }
    int position3 = this.parser.CurrentPosition();
    int position4 = AsnKeyParser.EqualOid(this.parser.NextOID(), new byte[7]
    {
      (byte) 42,
      (byte) 134,
      (byte) 72,
      (byte) 206,
      (byte) 56,
      (byte) 4,
      (byte) 1
    }) ? this.parser.CurrentPosition() : throw new BerDecodeException("Expected OID 1.2.840.10040.4.1", position3);
    int num3 = this.parser.NextSequence();
    if (num3 > this.parser.RemainingBytes())
    {
      StringBuilder stringBuilder = new StringBuilder("Incorrect DSS-Params Size. ");
      stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) num3.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.parser.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      throw new BerDecodeException(stringBuilder.ToString(), position4);
    }
    dsaPublicKey.P = AsnKeyParser.TrimLeadingZero(this.parser.NextInteger());
    dsaPublicKey.Q = AsnKeyParser.TrimLeadingZero(this.parser.NextInteger());
    dsaPublicKey.G = AsnKeyParser.TrimLeadingZero(this.parser.NextInteger());
    this.parser.NextBitString();
    dsaPublicKey.Y = AsnKeyParser.TrimLeadingZero(this.parser.NextInteger());
    return dsaPublicKey;
  }

  internal DSAParameters ParseDSAPrivateKey()
  {
    DSAParameters dsaPrivateKey = new DSAParameters();
    int position1 = this.parser.CurrentPosition();
    int num1 = this.parser.NextSequence();
    if (num1 != this.parser.RemainingBytes())
    {
      StringBuilder stringBuilder = new StringBuilder("Incorrect Sequence Size. ");
      stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) num1.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.parser.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      throw new BerDecodeException(stringBuilder.ToString(), position1);
    }
    int position2 = this.parser.CurrentPosition();
    int position3 = this.parser.NextInteger()[0] == (byte) 0 ? this.parser.CurrentPosition() : throw new BerDecodeException("Incorrect PrivateKeyInfo Version", position2);
    int num2 = this.parser.NextSequence();
    if (num2 > this.parser.RemainingBytes())
    {
      StringBuilder stringBuilder = new StringBuilder("Incorrect AlgorithmIdentifier Size. ");
      stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) num2.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.parser.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      throw new BerDecodeException(stringBuilder.ToString(), position3);
    }
    int position4 = this.parser.CurrentPosition();
    int position5 = AsnKeyParser.EqualOid(this.parser.NextOID(), new byte[7]
    {
      (byte) 42,
      (byte) 134,
      (byte) 72,
      (byte) 206,
      (byte) 56,
      (byte) 4,
      (byte) 1
    }) ? this.parser.CurrentPosition() : throw new BerDecodeException("Expected OID 1.2.840.10040.4.1", position4);
    int num3 = this.parser.NextSequence();
    if (num3 > this.parser.RemainingBytes())
    {
      StringBuilder stringBuilder = new StringBuilder("Incorrect DSS-Params Size. ");
      stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) num3.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.parser.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      throw new BerDecodeException(stringBuilder.ToString(), position5);
    }
    dsaPrivateKey.P = AsnKeyParser.TrimLeadingZero(this.parser.NextInteger());
    dsaPrivateKey.Q = AsnKeyParser.TrimLeadingZero(this.parser.NextInteger());
    dsaPrivateKey.G = AsnKeyParser.TrimLeadingZero(this.parser.NextInteger());
    this.parser.NextOctetString();
    dsaPrivateKey.X = AsnKeyParser.TrimLeadingZero(this.parser.NextInteger());
    return dsaPrivateKey;
  }
}
