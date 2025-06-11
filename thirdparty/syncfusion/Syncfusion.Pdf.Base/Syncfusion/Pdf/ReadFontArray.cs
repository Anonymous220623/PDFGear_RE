// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ReadFontArray
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf;

internal class ReadFontArray
{
  private byte[] nextValue_2 = new byte[2];
  private byte[] nextValue_4 = new byte[4];
  private byte[] m_data;
  private int m_pointer;

  public int Pointer
  {
    get => this.m_pointer;
    set => this.m_pointer = value;
  }

  public byte[] Data
  {
    get => this.m_data;
    set => this.m_data = value;
  }

  public ReadFontArray(byte[] data, int pointer)
  {
    this.m_data = data;
    this.m_pointer = pointer;
  }

  public ReadFontArray(byte[] data) => this.m_data = data;

  public byte getnextbyte()
  {
    byte num = this.Data[this.Pointer];
    ++this.Pointer;
    return num;
  }

  public int getnextUint32()
  {
    int num1 = 0;
    for (int index = 0; index < 4; ++index)
    {
      int num2 = this.Pointer >= this.Data.Length ? 0 : (int) this.Data[this.Pointer] & (int) byte.MaxValue;
      num1 += num2 << 8 * (3 - index);
      ++this.Pointer;
    }
    return num1;
  }

  public int getnextUint64()
  {
    int num1 = 0;
    for (int index = 0; index < 8; ++index)
    {
      int num2 = (int) this.Data[this.Pointer];
      if (num2 < 0)
        num2 = 256 /*0x0100*/ + num2;
      num1 += num2 << 8 * (7 - index);
      ++this.Pointer;
    }
    return num1;
  }

  public string getnextUint32AsTag()
  {
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < 4; ++index)
    {
      char ch = (char) this.Data[this.Pointer];
      stringBuilder.Append(ch);
      ++this.Pointer;
    }
    return stringBuilder.ToString();
  }

  public int getnextUint16()
  {
    int num1 = 0;
    for (int index = 0; index < 2; ++index)
    {
      if (this.Data.Length > 0)
      {
        int num2 = (int) this.Data[this.Pointer] & (int) byte.MaxValue;
        num1 += num2 << 8 * (1 - index);
      }
      ++this.Pointer;
    }
    return num1;
  }

  public ushort getnextUshort()
  {
    this.nextValue_2[0] = (byte) 0;
    this.nextValue_2[1] = (byte) 0;
    for (int index = 1; index >= 0; --index)
    {
      if (this.Data.Length > 0 && this.Pointer < this.Data.Length)
      {
        this.nextValue_2[index] = this.Data[this.Pointer];
        ++this.Pointer;
      }
    }
    return BitConverter.ToUInt16(this.nextValue_2, 0);
  }

  public ulong getnextULong()
  {
    this.nextValue_4[0] = (byte) 0;
    this.nextValue_4[1] = (byte) 0;
    this.nextValue_4[2] = (byte) 0;
    this.nextValue_4[3] = (byte) 0;
    for (int index = 3; index >= 0; --index)
    {
      if (this.Data.Length > 0 && this.Pointer < this.Data.Length)
      {
        this.nextValue_4[index] = this.Data[this.Pointer];
        ++this.Pointer;
      }
    }
    return (ulong) BitConverter.ToUInt32(this.nextValue_4, 0);
  }

  public sbyte ReadChar() => this.Read();

  public long getLongDateTime()
  {
    byte[] numArray = new byte[8];
    for (int index = 7; index >= 0; --index)
    {
      if (this.Data.Length > 0 && this.Pointer < this.Data.Length)
      {
        numArray[index] = this.Data[this.Pointer];
        ++this.Pointer;
      }
    }
    return BitConverter.ToInt64(numArray, 0);
  }

  public float getFixed()
  {
    return (float) this.getnextshort() + (float) ((int) this.getnextUshort() / 65536 /*0x010000*/);
  }

  public sbyte Read()
  {
    sbyte num = (sbyte) this.Data[this.Pointer];
    ++this.Pointer;
    return num;
  }

  public uint getULong()
  {
    this.nextValue_4[0] = (byte) 0;
    this.nextValue_4[1] = (byte) 0;
    this.nextValue_4[2] = (byte) 0;
    this.nextValue_4[3] = (byte) 0;
    for (int index = 3; index >= 0; --index)
    {
      if (this.Data.Length > 0 && this.Pointer < this.Data.Length)
      {
        this.nextValue_4[index] = this.Data[this.Pointer];
        ++this.Pointer;
      }
    }
    return BitConverter.ToUInt32(this.nextValue_4, 0);
  }

  public short getnextshort()
  {
    this.nextValue_2[0] = (byte) 0;
    this.nextValue_2[1] = (byte) 0;
    for (int index = 1; index >= 0; --index)
    {
      if (this.Data.Length > 0 && this.Pointer < this.Data.Length)
      {
        this.nextValue_2[index] = this.Data[this.Pointer];
        ++this.Pointer;
      }
    }
    return BitConverter.ToInt16(this.nextValue_2, 0);
  }

  public float Get2Dot14() => (float) this.getnextshort() / 16384f;
}
