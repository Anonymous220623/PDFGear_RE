// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.RectangularArrays
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf;

internal class RectangularArrays
{
  internal int[][] ReturnRectangularIntArray(int Size1, int Size2)
  {
    int[][] numArray = new int[Size1][];
    for (int index = 0; index < Size1; ++index)
      numArray[index] = new int[Size2];
    return numArray;
  }

  internal string[][] ReturnRectangularStringArray(int Size1, int Size2)
  {
    string[][] strArray = new string[Size1][];
    for (int index = 0; index < Size1; ++index)
      strArray[index] = new string[Size2];
    return strArray;
  }

  internal float[][] ReturnRectangularFloatArray(int Size1, int Size2)
  {
    float[][] numArray = new float[Size1][];
    for (int index = 0; index < Size1; ++index)
      numArray[index] = new float[Size2];
    return numArray;
  }

  internal short[][] ReturnRectangularShortArray(int Size1, int Size2)
  {
    short[][] numArray = new short[Size1][];
    for (int index = 0; index < Size1; ++index)
      numArray[index] = new short[Size2];
    return numArray;
  }

  internal byte[][] ReturnRectangularSbyteArray(int Size1, int Size2)
  {
    byte[][] numArray = new byte[Size1][];
    for (int index = 0; index < Size1; ++index)
      numArray[index] = new byte[Size2];
    return numArray;
  }
}
