// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.Latin1Converter
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System.Text;

#nullable disable
namespace XmpCore.Impl;

public static class Latin1Converter
{
  private const int StateStart = 0;
  private const int StateUtf8Char = 11;

  public static ByteBuffer Convert(ByteBuffer buffer)
  {
    if (buffer.GetEncoding() != Encoding.UTF8)
      return buffer;
    byte[] bytes = new byte[8];
    int len = 0;
    int num1 = 0;
    ByteBuffer byteBuffer = new ByteBuffer(buffer.Length * 4 / 3);
    int num2 = 0;
    for (int index1 = 0; index1 < buffer.Length; ++index1)
    {
      int num3 = buffer.CharAt(index1);
      if (num2 == 0 || num2 != 11)
      {
        if (num3 < (int) sbyte.MaxValue)
          byteBuffer.Append((byte) num3);
        else if (num3 >= 192 /*0xC0*/)
        {
          num1 = -1;
          for (int index2 = num3; num1 < 8 && (index2 & 128 /*0x80*/) == 128 /*0x80*/; index2 <<= 1)
            ++num1;
          bytes[len++] = (byte) num3;
          num2 = 11;
        }
        else
        {
          byte[] utf8 = Latin1Converter.ConvertToUtf8((byte) num3);
          byteBuffer.Append(utf8);
        }
      }
      else if (num1 > 0 && (num3 & 192 /*0xC0*/) == 128 /*0x80*/)
      {
        bytes[len++] = (byte) num3;
        --num1;
        if (num1 == 0)
        {
          byteBuffer.Append(bytes, 0, len);
          len = 0;
          num2 = 0;
        }
      }
      else
      {
        byte[] utf8 = Latin1Converter.ConvertToUtf8(bytes[0]);
        byteBuffer.Append(utf8);
        index1 -= len;
        len = 0;
        num2 = 0;
      }
    }
    if (num2 == 11)
    {
      for (int index = 0; index < len; ++index)
      {
        byte[] utf8 = Latin1Converter.ConvertToUtf8(bytes[index]);
        byteBuffer.Append(utf8);
      }
    }
    return byteBuffer;
  }

  private static byte[] ConvertToUtf8(byte ch)
  {
    int num = (int) ch & (int) byte.MaxValue;
    return num < 128 /*0x80*/ ? new byte[1]{ ch } : (num == 129 || num == 141 || num == 143 || num == 144 /*0x90*/ || num == 157 ? new byte[1]
    {
      (byte) 32 /*0x20*/
    } : Encoding.UTF8.GetBytes(Encoding.GetEncoding("windows-1252").GetString(new byte[1]
    {
      ch
    }, 0, 1)));
  }
}
