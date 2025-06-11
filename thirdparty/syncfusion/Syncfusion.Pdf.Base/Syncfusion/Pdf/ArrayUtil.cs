// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ArrayUtil
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class ArrayUtil
{
  public const int MAX_EL_COPYING = 8;
  public const int INIT_EL_COPYING = 4;

  public static void intArraySet(int[] arr, int val)
  {
    int length = arr.Length;
    if (length < 8)
    {
      for (int index = length - 1; index >= 0; --index)
        arr[index] = val;
    }
    else
    {
      int num = length >> 1;
      int index;
      for (index = 0; index < 4; ++index)
        arr[index] = val;
      for (; index <= num; index <<= 1)
        Array.Copy((Array) arr, 0, (Array) arr, index, index);
      if (index >= length)
        return;
      Array.Copy((Array) arr, 0, (Array) arr, index, length - index);
    }
  }

  public static void byteArraySet(byte[] arr, byte val)
  {
    int length = arr.Length;
    if (length < 8)
    {
      for (int index = length - 1; index >= 0; --index)
        arr[index] = val;
    }
    else
    {
      int num = length >> 1;
      int index;
      for (index = 0; index < 4; ++index)
        arr[index] = val;
      for (; index <= num; index <<= 1)
        Array.Copy((Array) arr, 0, (Array) arr, index, index);
      if (index >= length)
        return;
      Array.Copy((Array) arr, 0, (Array) arr, index, length - index);
    }
  }
}
