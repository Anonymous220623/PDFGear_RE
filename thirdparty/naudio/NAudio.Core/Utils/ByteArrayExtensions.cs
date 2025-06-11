// Decompiled with JetBrains decompiler
// Type: NAudio.Utils.ByteArrayExtensions
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;
using System.Text;

#nullable disable
namespace NAudio.Utils;

public static class ByteArrayExtensions
{
  public static bool IsEntirelyNull(byte[] buffer)
  {
    foreach (byte num in buffer)
    {
      if (num != (byte) 0)
        return false;
    }
    return true;
  }

  public static string DescribeAsHex(byte[] buffer, string separator, int bytesPerLine)
  {
    StringBuilder stringBuilder = new StringBuilder();
    int num1 = 0;
    foreach (byte num2 in buffer)
    {
      stringBuilder.AppendFormat("{0:X2}{1}", (object) num2, (object) separator);
      if (++num1 % bytesPerLine == 0)
        stringBuilder.Append("\r\n");
    }
    stringBuilder.Append("\r\n");
    return stringBuilder.ToString();
  }

  public static string DecodeAsString(byte[] buffer, int offset, int length, Encoding encoding)
  {
    for (int index = 0; index < length; ++index)
    {
      if (buffer[offset + index] == (byte) 0)
        length = index;
    }
    return encoding.GetString(buffer, offset, length);
  }

  public static byte[] Concat(params byte[][] byteArrays)
  {
    int length = 0;
    foreach (byte[] byteArray in byteArrays)
      length += byteArray.Length;
    if (length <= 0)
      return new byte[0];
    byte[] destinationArray = new byte[length];
    int destinationIndex = 0;
    foreach (byte[] byteArray in byteArrays)
    {
      Array.Copy((Array) byteArray, 0, (Array) destinationArray, destinationIndex, byteArray.Length);
      destinationIndex += byteArray.Length;
    }
    return destinationArray;
  }
}
