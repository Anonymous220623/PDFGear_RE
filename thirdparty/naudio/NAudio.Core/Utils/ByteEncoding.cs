// Decompiled with JetBrains decompiler
// Type: NAudio.Utils.ByteEncoding
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System.Text;

#nullable disable
namespace NAudio.Utils;

public class ByteEncoding : Encoding
{
  public static readonly ByteEncoding Instance = new ByteEncoding();

  private ByteEncoding()
  {
  }

  public override int GetByteCount(char[] chars, int index, int count) => count;

  public override int GetBytes(
    char[] chars,
    int charIndex,
    int charCount,
    byte[] bytes,
    int byteIndex)
  {
    for (int index = 0; index < charCount; ++index)
      bytes[byteIndex + index] = (byte) chars[charIndex + index];
    return charCount;
  }

  public override int GetCharCount(byte[] bytes, int index, int count)
  {
    for (int charCount = 0; charCount < count; ++charCount)
    {
      if (bytes[index + charCount] == (byte) 0)
        return charCount;
    }
    return count;
  }

  public override int GetChars(
    byte[] bytes,
    int byteIndex,
    int byteCount,
    char[] chars,
    int charIndex)
  {
    for (int chars1 = 0; chars1 < byteCount; ++chars1)
    {
      byte num = bytes[byteIndex + chars1];
      if (num == (byte) 0)
        return chars1;
      chars[charIndex + chars1] = (char) num;
    }
    return byteCount;
  }

  public override int GetMaxCharCount(int byteCount) => byteCount;

  public override int GetMaxByteCount(int charCount) => charCount;
}
