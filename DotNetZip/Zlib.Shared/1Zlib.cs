// Decompiled with JetBrains decompiler
// Type: Ionic.Zlib.SharedUtils
// Assembly: DotNetZip, Version=1.16.0.0, Culture=neutral, PublicKeyToken=6583c7c814667745
// MVID: 1BA35737-6015-47E9-B884-84E39CCBF397
// Assembly location: D:\PDFGear\bin\DotNetZip.dll

using System.IO;
using System.Text;

#nullable disable
namespace Ionic.Zlib;

internal class SharedUtils
{
  public static int URShift(int number, int bits) => number >>> bits;

  public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
  {
    if (target.Length == 0)
      return 0;
    char[] buffer = new char[target.Length];
    int num = sourceTextReader.Read(buffer, start, count);
    if (num == 0)
      return -1;
    for (int index = start; index < start + num; ++index)
      target[index] = (byte) buffer[index];
    return num;
  }

  internal static byte[] ToByteArray(string sourceString) => Encoding.UTF8.GetBytes(sourceString);

  internal static char[] ToCharArray(byte[] byteArray) => Encoding.UTF8.GetChars(byteArray);
}
