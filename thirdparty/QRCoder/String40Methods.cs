// Decompiled with JetBrains decompiler
// Type: QRCoder.String40Methods
// Assembly: QRCoder, Version=1.4.3.0, Culture=neutral, PublicKeyToken=c4ed5b9ae8358a28
// MVID: 5D0AD632-49D4-4E68-92A0-261964209740
// Assembly location: D:\PDFGear\bin\QRCoder.dll

#nullable disable
namespace QRCoder;

internal static class String40Methods
{
  public static bool IsNullOrWhiteSpace(string value)
  {
    if (value == null)
      return true;
    for (int index = 0; index < value.Length; ++index)
    {
      if (!char.IsWhiteSpace(value[index]))
        return false;
    }
    return true;
  }

  public static string ReverseString(string str)
  {
    char[] charArray = str.ToCharArray();
    char[] chArray = new char[charArray.Length];
    int index1 = 0;
    int index2 = str.Length - 1;
    while (index1 < str.Length)
    {
      chArray[index1] = charArray[index2];
      ++index1;
      --index2;
    }
    return new string(chArray);
  }

  public static bool IsAllDigit(string str)
  {
    foreach (char c in str)
    {
      if (!char.IsDigit(c))
        return false;
    }
    return true;
  }
}
