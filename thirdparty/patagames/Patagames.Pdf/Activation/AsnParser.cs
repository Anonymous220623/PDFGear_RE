// Decompiled with JetBrains decompiler
// Type: Patagames.Activation.AsnParser
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace Patagames.Activation;

internal class AsnParser
{
  private List<byte> octets;
  private int initialCount;

  internal AsnParser(byte[] values)
  {
    this.octets = new List<byte>(values.Length);
    this.octets.AddRange((IEnumerable<byte>) values);
    this.initialCount = this.octets.Count;
  }

  internal int CurrentPosition() => this.initialCount - this.octets.Count;

  internal int RemainingBytes() => this.octets.Count;

  private int GetLength()
  {
    int length = 0;
    int position = this.CurrentPosition();
    try
    {
      byte nextOctet = this.GetNextOctet();
      if ((int) nextOctet == ((int) nextOctet & (int) sbyte.MaxValue))
        return (int) nextOctet;
      int num = (int) nextOctet & (int) sbyte.MaxValue;
      if (num > 4)
      {
        StringBuilder stringBuilder = new StringBuilder("Invalid Length Encoding. ");
        stringBuilder.AppendFormat("Length uses {0} octets", (object) num.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        throw new BerDecodeException(stringBuilder.ToString(), position);
      }
      while (num-- != 0)
        length = length << 8 | (int) this.GetNextOctet();
    }
    catch (ArgumentOutOfRangeException ex)
    {
      throw new BerDecodeException("Error Parsing Key", position, (Exception) ex);
    }
    return length;
  }

  internal byte[] Next()
  {
    int position = this.CurrentPosition();
    try
    {
      int nextOctet = (int) this.GetNextOctet();
      int length = this.GetLength();
      if (length > this.RemainingBytes())
      {
        StringBuilder stringBuilder = new StringBuilder("Incorrect Size. ");
        stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) length.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
        throw new BerDecodeException(stringBuilder.ToString(), position);
      }
      return this.GetOctets(length);
    }
    catch (ArgumentOutOfRangeException ex)
    {
      throw new BerDecodeException("Error Parsing Key", position, (Exception) ex);
    }
  }

  internal byte GetNextOctet()
  {
    int position = this.CurrentPosition();
    if (this.RemainingBytes() == 0)
    {
      StringBuilder stringBuilder = new StringBuilder("Incorrect Size. ");
      int num = 1;
      string str1 = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      num = this.RemainingBytes();
      string str2 = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) str1, (object) str2);
      throw new BerDecodeException(stringBuilder.ToString(), position);
    }
    return this.GetOctets(1)[0];
  }

  internal byte[] GetOctets(int octetCount)
  {
    int position = this.CurrentPosition();
    if (octetCount > this.RemainingBytes())
    {
      StringBuilder stringBuilder = new StringBuilder("Incorrect Size. ");
      stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) octetCount.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
      throw new BerDecodeException(stringBuilder.ToString(), position);
    }
    byte[] array = new byte[octetCount];
    try
    {
      this.octets.CopyTo(0, array, 0, octetCount);
      this.octets.RemoveRange(0, octetCount);
    }
    catch (ArgumentOutOfRangeException ex)
    {
      throw new BerDecodeException("Error Parsing Key", position, (Exception) ex);
    }
    return array;
  }

  internal bool IsNextNull() => (byte) 5 == this.octets[0];

  internal int NextNull()
  {
    int position = this.CurrentPosition();
    try
    {
      byte nextOctet1 = this.GetNextOctet();
      if ((byte) 5 != nextOctet1)
      {
        StringBuilder stringBuilder = new StringBuilder("Expected Null. ");
        stringBuilder.AppendFormat("Specified Identifier: {0}", (object) nextOctet1.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        throw new BerDecodeException(stringBuilder.ToString(), position);
      }
      byte nextOctet2 = this.GetNextOctet();
      if (nextOctet2 != (byte) 0)
      {
        StringBuilder stringBuilder = new StringBuilder("Null has non-zero size. ");
        stringBuilder.AppendFormat("Size: {0}", (object) nextOctet2.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        throw new BerDecodeException(stringBuilder.ToString(), position);
      }
      return 0;
    }
    catch (ArgumentOutOfRangeException ex)
    {
      throw new BerDecodeException("Error Parsing Key", position, (Exception) ex);
    }
  }

  internal bool IsNextSequence() => (byte) 48 /*0x30*/ == this.octets[0];

  internal int NextSequence()
  {
    int position = this.CurrentPosition();
    try
    {
      byte nextOctet = this.GetNextOctet();
      if ((byte) 48 /*0x30*/ != nextOctet)
      {
        StringBuilder stringBuilder = new StringBuilder("Expected Sequence. ");
        stringBuilder.AppendFormat("Specified Identifier: {0}", (object) nextOctet.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        throw new BerDecodeException(stringBuilder.ToString(), position);
      }
      int length = this.GetLength();
      if (length > this.RemainingBytes())
      {
        StringBuilder stringBuilder = new StringBuilder("Incorrect Sequence Size. ");
        stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) length.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
        throw new BerDecodeException(stringBuilder.ToString(), position);
      }
      return length;
    }
    catch (ArgumentOutOfRangeException ex)
    {
      throw new BerDecodeException("Error Parsing Key", position, (Exception) ex);
    }
  }

  internal bool IsNextOctetString() => (byte) 4 == this.octets[0];

  internal int NextOctetString()
  {
    int position = this.CurrentPosition();
    try
    {
      byte nextOctet = this.GetNextOctet();
      if ((byte) 4 != nextOctet)
      {
        StringBuilder stringBuilder = new StringBuilder("Expected Octet String. ");
        stringBuilder.AppendFormat("Specified Identifier: {0}", (object) nextOctet.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        throw new BerDecodeException(stringBuilder.ToString(), position);
      }
      int length = this.GetLength();
      if (length > this.RemainingBytes())
      {
        StringBuilder stringBuilder = new StringBuilder("Incorrect Octet String Size. ");
        stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) length.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
        throw new BerDecodeException(stringBuilder.ToString(), position);
      }
      return length;
    }
    catch (ArgumentOutOfRangeException ex)
    {
      throw new BerDecodeException("Error Parsing Key", position, (Exception) ex);
    }
  }

  internal bool IsNextBitString() => (byte) 3 == this.octets[0];

  internal int NextBitString()
  {
    int position = this.CurrentPosition();
    try
    {
      byte nextOctet = this.GetNextOctet();
      if ((byte) 3 != nextOctet)
      {
        StringBuilder stringBuilder = new StringBuilder("Expected Bit String. ");
        stringBuilder.AppendFormat("Specified Identifier: {0}", (object) nextOctet.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        throw new BerDecodeException(stringBuilder.ToString(), position);
      }
      int length = this.GetLength();
      byte octet = this.octets[0];
      this.octets.RemoveAt(0);
      int num = length - 1;
      if (octet != (byte) 0)
        throw new BerDecodeException("The first octet of BitString must be 0", position);
      return num;
    }
    catch (ArgumentOutOfRangeException ex)
    {
      throw new BerDecodeException("Error Parsing Key", position, (Exception) ex);
    }
  }

  internal bool IsNextInteger() => (byte) 2 == this.octets[0];

  internal byte[] NextInteger()
  {
    int position = this.CurrentPosition();
    try
    {
      byte nextOctet = this.GetNextOctet();
      if ((byte) 2 != nextOctet)
      {
        StringBuilder stringBuilder = new StringBuilder("Expected Integer. ");
        stringBuilder.AppendFormat("Specified Identifier: {0}", (object) nextOctet.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        throw new BerDecodeException(stringBuilder.ToString(), position);
      }
      int length = this.GetLength();
      if (length > this.RemainingBytes())
      {
        StringBuilder stringBuilder = new StringBuilder("Incorrect Integer Size. ");
        stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) length.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
        throw new BerDecodeException(stringBuilder.ToString(), position);
      }
      return this.GetOctets(length);
    }
    catch (ArgumentOutOfRangeException ex)
    {
      throw new BerDecodeException("Error Parsing Key", position, (Exception) ex);
    }
  }

  internal byte[] NextOID()
  {
    int position = this.CurrentPosition();
    try
    {
      byte nextOctet = this.GetNextOctet();
      if ((byte) 6 != nextOctet)
      {
        StringBuilder stringBuilder = new StringBuilder("Expected Object Identifier. ");
        stringBuilder.AppendFormat("Specified Identifier: {0}", (object) nextOctet.ToString((IFormatProvider) CultureInfo.InvariantCulture));
        throw new BerDecodeException(stringBuilder.ToString(), position);
      }
      int length = this.GetLength();
      if (length > this.RemainingBytes())
      {
        StringBuilder stringBuilder = new StringBuilder("Incorrect Object Identifier Size. ");
        stringBuilder.AppendFormat("Specified: {0}, Remaining: {1}", (object) length.ToString((IFormatProvider) CultureInfo.InvariantCulture), (object) this.RemainingBytes().ToString((IFormatProvider) CultureInfo.InvariantCulture));
        throw new BerDecodeException(stringBuilder.ToString(), position);
      }
      byte[] numArray = new byte[length];
      for (int index = 0; index < length; ++index)
      {
        numArray[index] = this.octets[0];
        this.octets.RemoveAt(0);
      }
      return numArray;
    }
    catch (ArgumentOutOfRangeException ex)
    {
      throw new BerDecodeException("Error Parsing Key", position, (Exception) ex);
    }
  }
}
