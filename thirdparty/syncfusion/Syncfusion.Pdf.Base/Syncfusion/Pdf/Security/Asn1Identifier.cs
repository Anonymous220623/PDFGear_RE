// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Asn1Identifier
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Asn1Identifier : Asn1
{
  private char m_tokenSeparator = '.';
  private string m_id;
  private byte[] m_body;
  private MemoryStream m_stream;

  public Asn1Identifier(string id)
    : base(Asn1UniversalTags.ObjectIdentifier)
  {
    this.m_id = !string.IsNullOrEmpty(id) ? id : throw new ArgumentNullException("Oid");
  }

  public Asn1Identifier(byte[] bytes)
    : base(Asn1UniversalTags.ObjectIdentifier)
  {
    this.m_id = this.CreateIdentifier(bytes);
    this.m_body = bytes == null ? (byte[]) null : (byte[]) bytes.Clone();
  }

  public Asn1Identifier GetIdentifier(object obj) => (Asn1Identifier) obj;

  internal byte[] Body => this.m_body;

  private byte[] ToArray()
  {
    string[] strArray = this.m_id.Split(this.m_tokenSeparator);
    int num1 = int.Parse(strArray[0]);
    int num2 = int.Parse(strArray[1]);
    MemoryStream memoryStream = new MemoryStream();
    this.AppendField((long) (num1 * 40 + num2), (Stream) memoryStream);
    for (int index = 2; index < strArray.Length; ++index)
    {
      string s = strArray[index];
      if (s.Length < 18)
        this.AppendField((long) int.Parse(s), (Stream) memoryStream);
      else
        this.AppendField(s, (Stream) memoryStream);
    }
    byte[] array = memoryStream.ToArray();
    memoryStream.Dispose();
    return array;
  }

  private void AppendField(long value, Stream stream)
  {
    if (value >= 128L /*0x80*/)
    {
      if (value >= 16384L /*0x4000*/)
      {
        if (value >= 2097152L /*0x200000*/)
        {
          if (value >= 268435456L /*0x10000000*/)
          {
            if (value >= 34359738368L /*0x0800000000*/)
            {
              if (value >= 4398046511104L /*0x040000000000*/)
              {
                if (value >= 562949953421312L /*0x02000000000000*/)
                {
                  if (value >= 72057594037927936L /*0x0100000000000000*/)
                    stream.WriteByte((byte) ((ulong) (value >> 56) | 128UL /*0x80*/));
                  stream.WriteByte((byte) ((ulong) (value >> 49) | 128UL /*0x80*/));
                }
                stream.WriteByte((byte) ((ulong) (value >> 42) | 128UL /*0x80*/));
              }
              stream.WriteByte((byte) ((ulong) (value >> 35) | 128UL /*0x80*/));
            }
            stream.WriteByte((byte) ((ulong) (value >> 28) | 128UL /*0x80*/));
          }
          stream.WriteByte((byte) ((ulong) (value >> 21) | 128UL /*0x80*/));
        }
        stream.WriteByte((byte) ((ulong) (value >> 14) | 128UL /*0x80*/));
      }
      stream.WriteByte((byte) ((ulong) (value >> 7) | 128UL /*0x80*/));
    }
    stream.WriteByte((byte) ((ulong) value & (ulong) sbyte.MaxValue));
  }

  private void AppendField(string value, Stream stream)
  {
    int length = (Encoding.ASCII.GetBytes(value).Length + 6) / 7;
    if (length == 0)
    {
      stream.WriteByte((byte) 0);
    }
    else
    {
      int int16 = (int) Convert.ToInt16(value);
      byte[] buffer = new byte[length];
      for (int index = length - 1; index >= 0; --index)
      {
        buffer[index] = (byte) (int16 & (int) sbyte.MaxValue | 128 /*0x80*/);
        int16 >>= 7;
      }
      buffer[length - 1] &= (byte) 127 /*0x7F*/;
      stream.Write(buffer, 0, buffer.Length);
    }
  }

  internal byte[] Asn1Encode() => this.Asn1Encode(this.ToArray());

  protected override bool IsEquals(Asn1 asn1Object) => throw new NotImplementedException();

  public override int GetHashCode() => throw new NotImplementedException();

  internal override void Encode(DerStream derStr) => derStr.WriteEncoded(6, this.ToArray());

  private string CreateIdentifier(byte[] bytes)
  {
    StringBuilder stringBuilder = new StringBuilder();
    long num1 = 0;
    bool flag = true;
    for (int index = 0; index != bytes.Length; ++index)
    {
      int num2 = (int) bytes[index];
      if (num1 < 36028797018963968L /*0x80000000000000*/)
      {
        num1 = num1 * 128L /*0x80*/ + (long) (num2 & (int) sbyte.MaxValue);
        if ((num2 & 128 /*0x80*/) == 0)
        {
          if (flag)
          {
            switch ((int) num1 / 40)
            {
              case 0:
                stringBuilder.Append('0');
                break;
              case 1:
                stringBuilder.Append('1');
                num1 -= 40L;
                break;
              default:
                stringBuilder.Append('2');
                num1 -= 80L /*0x50*/;
                break;
            }
            flag = false;
          }
          stringBuilder.Append('.');
          stringBuilder.Append(num1);
          num1 = 0L;
        }
      }
    }
    return stringBuilder.ToString();
  }
}
