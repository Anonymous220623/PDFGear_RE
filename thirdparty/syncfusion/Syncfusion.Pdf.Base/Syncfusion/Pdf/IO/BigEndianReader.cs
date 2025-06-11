// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.IO.BigEndianReader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.IO;

internal class BigEndianReader
{
  internal const int Int32Size = 4;
  internal const int Int16Size = 2;
  internal const int Int64Size = 8;
  private const float c_fraction = 16384f;
  private readonly Encoding c_encoding = Encoding.GetEncoding(1252);
  private BinaryReader m_reader;

  public BinaryReader Reader
  {
    get => this.m_reader;
    set => this.m_reader = value;
  }

  public Stream BaseStream => this.m_reader.BaseStream;

  public BigEndianReader(BinaryReader reader)
  {
    this.m_reader = reader != null ? reader : throw new ArgumentNullException(nameof (reader));
  }

  public void Close()
  {
    if (this.m_reader == null)
      return;
    if (this.m_reader.BaseStream != null)
      this.m_reader.BaseStream.Close();
    this.m_reader.Close();
    this.m_reader = (BinaryReader) null;
  }

  public void Seek(long position)
  {
    if (!this.m_reader.BaseStream.CanSeek || position > (long) int.MaxValue)
      return;
    this.m_reader.BaseStream.Position = position;
  }

  public void Skip(long numBytes) => this.Seek(this.m_reader.BaseStream.Position + numBytes);

  public byte[] Reverse(byte[] buffer)
  {
    if (buffer == null)
      throw new ArgumentNullException(nameof (buffer));
    Array.Reverse((Array) buffer);
    return buffer;
  }

  public long ReadInt64()
  {
    byte[] numArray = this.Reverse(this.m_reader.ReadBytes(8));
    long num = 0;
    return numArray.Length < 8 ? num : BitConverter.ToInt64(numArray, 0);
  }

  public ulong ReadUInt64()
  {
    byte[] numArray = this.Reverse(this.m_reader.ReadBytes(8));
    ulong num = 0;
    return numArray.Length < 8 ? num : BitConverter.ToUInt64(numArray, 0);
  }

  public int ReadInt32()
  {
    byte[] numArray = this.Reverse(this.m_reader.ReadBytes(4));
    int num = 0;
    return numArray.Length < 4 ? num : BitConverter.ToInt32(numArray, 0);
  }

  public uint ReadUInt32()
  {
    byte[] numArray = this.Reverse(this.m_reader.ReadBytes(4));
    uint num = 0;
    return numArray.Length < 4 ? num : BitConverter.ToUInt32(numArray, 0);
  }

  public short ReadInt16()
  {
    byte[] numArray = this.Reverse(this.m_reader.ReadBytes(2));
    short num = 0;
    return numArray.Length < 2 ? num : BitConverter.ToInt16(numArray, 0);
  }

  public ushort ReadUInt16()
  {
    byte[] numArray = this.Reverse(this.m_reader.ReadBytes(2));
    ushort num = 0;
    return numArray.Length < 2 ? num : BitConverter.ToUInt16(numArray, 0);
  }

  public byte ReadByte() => this.m_reader.ReadByte();

  public float ReadFixed()
  {
    return (float) BitConverter.ToInt16(this.Reverse(this.m_reader.ReadBytes(2)), 0) + (float) BitConverter.ToInt16(this.Reverse(this.m_reader.ReadBytes(2)), 0) / 16384f;
  }

  public byte[] ReadBytes(int count) => this.m_reader.ReadBytes(count);

  public string ReadString(int len) => this.ReadString(len, false);

  public string ReadString(int len, bool unicode)
  {
    string str;
    if (unicode)
    {
      byte[] bytes = this.ReadBytes(len);
      str = Encoding.BigEndianUnicode.GetString(bytes, 0, bytes.Length);
    }
    else
    {
      byte[] bytes = this.ReadBytes(len);
      str = this.c_encoding.GetString(bytes, 0, bytes.Length);
    }
    return str;
  }

  public string ReadUtf8String(int len)
  {
    byte[] bytes = this.ReadBytes(len);
    return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
  }

  public int Read(byte[] buffer, int index, int count)
  {
    if (buffer == null)
      throw new ArgumentNullException(nameof (buffer));
    int num1 = 0;
    do
    {
      int num2 = this.m_reader.Read(buffer, index + num1, count - num1);
      num1 += num2;
    }
    while (num1 < count);
    return num1;
  }
}
