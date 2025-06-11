// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Pkcs7Padding
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class Pkcs7Padding : IPadding
{
  public void Initialize(SecureRandomAlgorithm random)
  {
  }

  public string PaddingName => "PKCS7";

  public int AddPadding(byte[] bytes, int offset)
  {
    byte num = (byte) (bytes.Length - offset);
    for (; offset < bytes.Length; ++offset)
      bytes[offset] = num;
    return (int) num;
  }

  public int Count(byte[] input)
  {
    int num = (int) input[input.Length - 1];
    if (num < 1 || num > input.Length)
      throw new Exception("Invalid pad");
    for (int index = 1; index <= num; ++index)
    {
      if ((int) input[input.Length - index] != num)
        throw new Exception("Invalid pad");
    }
    return num;
  }
}
