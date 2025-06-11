// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.VbaDataProcess
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Office;

internal class VbaDataProcess
{
  internal static Stream Compress(Stream decompressed)
  {
    byte[] buffer = new byte[decompressed.Length];
    decompressed.Position = 0L;
    decompressed.Read(buffer, 0, (int) decompressed.Length);
    MemoryStream memoryStream = new MemoryStream();
    int index1 = 0;
    int length = (int) decompressed.Length;
    int num1 = 0;
    memoryStream.WriteByte((byte) 1);
    int num2 = num1 + 1;
    while (index1 < length)
    {
      int num3 = num2;
      int num4 = index1;
      int num5 = num3 + 4098;
      num2 = num3 + 2;
      int num6 = Math.Min(num4 + 4096 /*0x1000*/, length);
      while (index1 < num6 && num2 < num5)
      {
        int num7 = num2;
        byte num8 = 0;
        ++num2;
        for (int index2 = 0; index2 <= 7; ++index2)
        {
          if (index1 < num6 && num2 < num5)
          {
            int num9 = index1 - 1;
            int val1 = 0;
            int num10 = 0;
            for (; num9 >= num4; --num9)
            {
              int index3 = num9;
              int index4 = index1;
              int num11 = 0;
              for (; index4 < num6 && (int) buffer[index3] == (int) buffer[index4]; ++index4)
              {
                ++num11;
                ++index3;
              }
              if (num11 > val1)
              {
                val1 = num11;
                num10 = num9;
              }
            }
            ushort num12;
            ushort num13;
            if (val1 >= 3)
            {
              double d = Math.Log((double) (index1 - num4), 2.0);
              ushort val2 = (ushort) ((uint) (ushort) ((uint) ushort.MaxValue >> (int) Math.Max(d % 1.0 == 0.0 ? (ushort) d : (ushort) (double) (ushort) (Math.Floor(d) + 1.0), (ushort) 4)) + 3U);
              num12 = (ushort) Math.Min(val1, (int) val2);
              num13 = (ushort) (index1 - num10);
            }
            else
            {
              num12 = (ushort) 0;
              num13 = (ushort) 0;
            }
            if (num13 != (ushort) 0)
            {
              if (num2 + 1 < num5)
              {
                double d = Math.Log((double) (index1 - num4), 2.0);
                ushort num14 = Math.Max(d % 1.0 == 0.0 ? (ushort) d : (ushort) (double) (ushort) (Math.Floor(d) + 1.0), (ushort) 4);
                ushort num15 = (ushort) ((uint) ushort.MaxValue >> (int) num14);
                byte[] bytes = BitConverter.GetBytes((ushort) ((uint) (ushort) ((uint) num13 - 1U) << (int) (ushort) (16U /*0x10*/ - (uint) num14) | (uint) (ushort) ((uint) num12 - 3U)));
                memoryStream.Position = (long) num2;
                memoryStream.Write(bytes, 0, 2);
                num8 = (byte) ((int) num8 & ~(1 << index2) | 1 << index2);
                num2 += 2;
                index1 += (int) num12;
              }
              else
                num2 = num5;
            }
            else if (num2 < num5)
            {
              memoryStream.Position = (long) num2;
              memoryStream.WriteByte(buffer[index1]);
              ++num2;
              ++index1;
            }
            else
              num2 = num5;
          }
        }
        memoryStream.Position = (long) num7;
        memoryStream.WriteByte(num8);
      }
      int num16;
      if (index1 < num6)
      {
        int num17 = num6 - 1;
        num2 = num3 + 2;
        index1 = num4;
        int num18 = 4096 /*0x1000*/;
        for (int index5 = num4; index5 <= num17; ++index5)
        {
          memoryStream.Position = (long) num2;
          memoryStream.WriteByte(buffer[index5]);
          ++num2;
          ++index1;
          --num18;
        }
        for (int index6 = 1; index6 <= num18; ++index6)
        {
          memoryStream.Position = (long) num2;
          memoryStream.WriteByte((byte) 0);
          ++num2;
        }
        num16 = 0;
      }
      else
        num16 = 1;
      ushort num19 = (ushort) ((int) (ushort) ((int) (ushort) (0 & 61440 /*0xF000*/ | num2 - num3 - 3) & (int) short.MaxValue | num16 << 15) & 36863 /*0x8FFF*/ | 12288 /*0x3000*/);
      memoryStream.Position = (long) num3;
      byte[] bytes1 = BitConverter.GetBytes(num19);
      memoryStream.Write(bytes1, 0, bytes1.Length);
      memoryStream.Position = (long) num2;
    }
    return (Stream) memoryStream;
  }

  internal static Stream Decompress(Stream compressed)
  {
    byte[] buffer = new byte[compressed.Length];
    compressed.Position = 0L;
    compressed.Read(buffer, 0, (int) compressed.Length);
    MemoryStream memoryStream1 = new MemoryStream(4096 /*0x1000*/);
    int length = buffer.Length;
    int num1 = 0;
    long position = memoryStream1.Position;
    if (buffer[0] != (byte) 1)
      throw new Exception("Stream is corrupted");
    int startIndex1 = num1 + 1;
    while (startIndex1 < length)
    {
      int startIndex2 = startIndex1;
      int uint16_1 = (int) BitConverter.ToUInt16(buffer, startIndex2);
      int num2 = (uint16_1 & 4095 /*0x0FFF*/) + 3;
      int num3 = (uint16_1 & 32768 /*0x8000*/) >> 15;
      int num4 = (int) position;
      int num5 = Math.Min(length, startIndex2 + num2);
      startIndex1 = startIndex2 + 2;
      if (num3 == 1)
      {
        while (startIndex1 < num5)
        {
          byte num6 = buffer[startIndex1];
          ++startIndex1;
          if (startIndex1 < num5)
          {
            for (int index1 = 0; index1 <= 7; ++index1)
            {
              if (startIndex1 < num5)
              {
                if (((int) num6 >> index1 & 1) == 0)
                {
                  memoryStream1.Position = position;
                  memoryStream1.WriteByte(buffer[startIndex1]);
                  ++position;
                  ++startIndex1;
                }
                else
                {
                  int uint16_2 = (int) BitConverter.ToUInt16(buffer, startIndex1);
                  double d = Math.Log((double) ((int) position - num4), 2.0);
                  ushort num7 = Math.Max(d % 1.0 == 0.0 ? (ushort) d : (ushort) (double) (ushort) (Math.Floor(d) + 1.0), (ushort) 4);
                  ushort num8 = (ushort) ((uint) ushort.MaxValue >> (int) num7);
                  ushort num9 = ~num8;
                  ushort num10 = (ushort) ((uint16_2 & (int) num8) + 3);
                  ushort num11 = (ushort) (((uint16_2 & (int) num9) >> 16 /*0x10*/ - (int) num7) + 1);
                  int index2 = (int) position - (int) num11;
                  int num12 = (int) position;
                  for (int index3 = 1; index3 <= (int) num10; ++index3)
                  {
                    memoryStream1.Position = (long) num12;
                    memoryStream1.WriteByte(memoryStream1.ToArray()[index2]);
                    ++num12;
                    ++index2;
                  }
                  position += (long) num10;
                  startIndex1 += 2;
                }
              }
            }
          }
        }
      }
      else
      {
        MemoryStream memoryStream2 = new MemoryStream(4096 /*0x1000*/);
        memoryStream2.Write(buffer, 0, buffer.Length);
        memoryStream2.WriteTo((Stream) memoryStream1);
        position += 4096L /*0x1000*/;
        startIndex1 += 4096 /*0x1000*/;
      }
    }
    return (Stream) memoryStream1;
  }

  internal static byte[] Decrypt(byte[] data)
  {
    byte num1 = data[0];
    byte num2 = data[1];
    byte num3 = data[2];
    byte num4 = (byte) ((uint) num1 ^ (uint) num3);
    byte num5 = num3;
    byte num6 = num2;
    int length = ((int) num1 & 6) / 2;
    byte[] numArray1 = new byte[length];
    int index1 = 0;
    int index2 = 3;
    while (index2 < length + 3)
    {
      numArray1[index1] = data[index2];
      ++index2;
      ++index1;
    }
    foreach (byte num7 in numArray1)
    {
      byte num8 = (byte) ((uint) num7 ^ (uint) num6 + (uint) num4);
      num6 = num5;
      num5 = num7;
      num4 = num8;
    }
    byte[] numArray2 = new byte[4];
    int index3 = 0;
    int index4 = length + 3;
    while (index4 < length + 7)
    {
      numArray2[index3] = data[index4];
      ++index4;
      ++index3;
    }
    byte y = 0;
    int num9 = 0;
    foreach (byte num10 in numArray2)
    {
      byte num11 = (byte) ((uint) num10 ^ (uint) num6 + (uint) num4);
      double num12 = Math.Pow(256.0, (double) y) * (double) num11;
      num9 += (int) num12;
      num6 = num5;
      num5 = num10;
      num4 = num11;
      ++y;
    }
    byte[] numArray3 = new byte[data.Length - (length + 7)];
    int index5 = 0;
    int index6 = length + 7;
    while (index6 < data.Length)
    {
      numArray3[index5] = data[index6];
      ++index6;
      ++index5;
    }
    MemoryStream memoryStream = new MemoryStream();
    foreach (byte num13 in numArray3)
    {
      byte num14 = (byte) ((uint) num13 ^ (uint) num6 + (uint) num4);
      memoryStream.WriteByte(num14);
      num6 = num5;
      num5 = num13;
      num4 = num14;
    }
    return memoryStream.ToArray();
  }

  internal static byte[] Encrypt(byte[] data, string clsID)
  {
    byte num1 = 2;
    int length1 = data.Length;
    byte num2 = 3;
    byte num3 = (byte) ((uint) num2 ^ (uint) num1);
    byte num4 = 0;
    foreach (char ch in clsID)
      num4 += (byte) ch;
    byte num5 = (byte) ((uint) num2 ^ (uint) num4);
    byte num6 = num4;
    byte num7 = num5;
    byte num8 = num3;
    int length2 = ((int) num2 & 6) / 2;
    byte[] buffer1 = new byte[length2];
    for (int index = 1; index <= length2; ++index)
    {
      byte num9 = 1;
      byte num10 = (byte) ((uint) num9 ^ (uint) num8 + (uint) num6);
      buffer1[index - 1] = num10;
      num8 = num7;
      num7 = num10;
      num6 = num9;
    }
    byte[] bytes = BitConverter.GetBytes(length1);
    byte[] buffer2 = new byte[4];
    int index1 = 3;
    int index2 = 0;
    while (index1 >= 0)
    {
      byte num11 = bytes[index1];
      byte num12 = (byte) ((uint) num11 ^ (uint) num8 + (uint) num6);
      buffer2[index2] = num12;
      num8 = num7;
      num7 = num12;
      num6 = num11;
      --index1;
      ++index2;
    }
    MemoryStream memoryStream1 = new MemoryStream();
    for (int index3 = 0; index3 < data.Length; ++index3)
    {
      byte num13 = data[index3];
      byte num14 = (byte) ((uint) num13 ^ (uint) num8 + (uint) num6);
      memoryStream1.WriteByte(num14);
      num8 = num7;
      num7 = num14;
      num6 = num13;
    }
    byte[] array = memoryStream1.ToArray();
    MemoryStream memoryStream2 = new MemoryStream();
    memoryStream2.WriteByte(num2);
    memoryStream2.WriteByte(num3);
    memoryStream2.WriteByte(num5);
    memoryStream2.Write(buffer1, 0, buffer1.Length);
    memoryStream2.Write(buffer2, 0, 4);
    memoryStream2.Write(array, 0, array.Length);
    return memoryStream2.ToArray();
  }
}
