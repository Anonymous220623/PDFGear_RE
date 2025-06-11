// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.DerBitString
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class DerBitString : DerString
{
  private static readonly char[] m_table = new char[16 /*0x10*/]
  {
    '0',
    '1',
    '2',
    '3',
    '4',
    '5',
    '6',
    '7',
    '8',
    '9',
    'A',
    'B',
    'C',
    'D',
    'E',
    'F'
  };
  private byte[] m_data;
  private int m_extra;

  internal int ExtraBits => this.m_extra;

  internal DerBitString(byte data, int pad)
  {
    this.m_data = new byte[1]{ data };
    this.m_extra = pad;
  }

  internal DerBitString(byte[] data, int pad)
  {
    this.m_data = data;
    this.m_extra = pad;
  }

  internal DerBitString(byte[] data) => this.m_data = data;

  internal DerBitString(Syncfusion.Pdf.Security.Asn1Encode asn1)
  {
    this.m_data = asn1.GetDerEncoded();
  }

  public byte[] GetBytes() => this.m_data;

  public int Value
  {
    get
    {
      int num = 0;
      for (int index = 0; index != this.m_data.Length && index != 4; ++index)
        num |= ((int) this.m_data[index] & (int) byte.MaxValue) << 8 * index;
      return num;
    }
  }

  internal override void Encode(DerStream stream)
  {
    byte[] numArray = new byte[this.GetBytes().Length + 1];
    numArray[0] = (byte) this.m_extra;
    Array.Copy((Array) this.GetBytes(), 0, (Array) numArray, 1, numArray.Length - 1);
    stream.WriteEncoded(3, numArray);
  }

  public override int GetHashCode()
  {
    return this.m_extra.GetHashCode() ^ Asn1Constants.GetHashCode(this.m_data);
  }

  protected override bool IsEquals(Asn1 asn1)
  {
    return asn1 is DerBitString derBitString && this.m_extra == derBitString.m_extra && Asn1Constants.AreEqual(this.m_data, derBitString.m_data);
  }

  public override string GetString()
  {
    StringBuilder stringBuilder = new StringBuilder("#");
    byte[] derEncoded = this.GetDerEncoded();
    for (int index = 0; index != derEncoded.Length; ++index)
    {
      uint num = (uint) derEncoded[index];
      stringBuilder.Append(DerBitString.m_table[(IntPtr) (num >> 4 & 15U)]);
      stringBuilder.Append(DerBitString.m_table[(int) derEncoded[index] & 15]);
    }
    return stringBuilder.ToString();
  }

  internal static DerBitString FromAsn1Octets(byte[] bytes)
  {
    int pad = (int) bytes[0];
    byte[] numArray = new byte[bytes.Length - 1];
    Array.Copy((Array) bytes, 1, (Array) numArray, 0, numArray.Length);
    return new DerBitString(numArray, pad);
  }

  internal static DerBitString GetString(object obj)
  {
    switch (obj)
    {
      case null:
      case DerBitString _:
        return (DerBitString) obj;
      default:
        throw new ArgumentException("Invalid entry");
    }
  }

  internal static DerBitString GetString(Asn1Tag tag, bool isExplicit)
  {
    Asn1 asn1 = tag.GetObject();
    return isExplicit || asn1 is DerBitString ? DerBitString.GetString((object) asn1) : DerBitString.FromAsn1Octets(((Asn1Octet) asn1).GetOctets());
  }
}
