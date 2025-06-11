// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.Utf8Checker
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class Utf8Checker
{
  internal static bool IsUtf8(Stream stream)
  {
    byte[] buffer = new byte[stream.Length > 4L ? new IntPtr(4) : checked ((IntPtr) stream.Length)];
    stream.Position = 0L;
    stream.Read(buffer, 0, buffer.Length);
    stream.Position = 0L;
    if (buffer.Length >= 2 && (buffer[0] == byte.MaxValue && buffer[1] == (byte) 254 || buffer[0] == (byte) 254 && buffer[1] == byte.MaxValue))
      return false;
    if (buffer.Length >= 3)
    {
      if (buffer[0] == (byte) 239 && buffer[1] == (byte) 187 && buffer[2] == (byte) 191)
        return true;
      if (buffer[0] == (byte) 43 && buffer[1] == (byte) 47 && buffer[2] == (byte) 118)
        return false;
    }
    if (buffer.Length == 4 && (buffer[0] == byte.MaxValue && buffer[1] == (byte) 254 && buffer[2] == (byte) 0 && buffer[3] == (byte) 0 || buffer[0] == (byte) 0 && buffer[1] == (byte) 0 && buffer[2] == (byte) 254 && buffer[3] == byte.MaxValue))
      return false;
    bool flag = !Utf8Checker.HasExtendedASCIICharacter(stream);
    stream.Position = 0L;
    return flag;
  }

  private static bool HasExtendedASCIICharacter(Stream stream)
  {
    stream.Position = 0L;
    while (stream.Position < stream.Length)
    {
      int num1 = stream.ReadByte();
      if (num1 != -1)
      {
        if (num1 > (int) sbyte.MaxValue)
        {
          if (num1 >= 192 /*0xC0*/ && num1 <= 223)
          {
            int num2 = stream.ReadByte();
            if (num2 < 128 /*0x80*/ || num2 > 191)
              return true;
          }
          else if (num1 >= 224 /*0xE0*/ && num1 <= 239)
          {
            int num3 = 0;
            while (num3 < 2)
            {
              int num4 = stream.ReadByte();
              ++num3;
              if (num4 < 128 /*0x80*/ || num4 > 191)
                return true;
            }
          }
          else
          {
            if (num1 < 240 /*0xF0*/ || num1 > 247)
              return true;
            int num5 = 0;
            while (num5 < 3)
            {
              int num6 = stream.ReadByte();
              ++num5;
              if (num6 < 128 /*0x80*/ || num6 > 191)
                return true;
            }
          }
        }
      }
      else
        break;
    }
    return false;
  }
}
