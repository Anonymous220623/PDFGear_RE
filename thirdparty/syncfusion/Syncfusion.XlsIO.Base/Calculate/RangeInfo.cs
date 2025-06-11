// Decompiled with JetBrains decompiler
// Type: Syncfusion.Calculate.RangeInfo
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.Calculate;

public class RangeInfo
{
  private int top;
  private int left;
  private int right;
  private int bottom;

  public RangeInfo(int top, int left, int bottom, int right)
  {
    this.top = top;
    this.bottom = bottom;
    this.left = left;
    this.right = right;
  }

  public int Bottom
  {
    get => this.bottom;
    set => this.bottom = value;
  }

  public int Left
  {
    get => this.left;
    set => this.left = value;
  }

  public int Right
  {
    get => this.right;
    set => this.right = value;
  }

  public int Top
  {
    get => this.top;
    set => this.top = value;
  }

  public static RangeInfo Cells(int top, int left, int bottom, int right)
  {
    return new RangeInfo(top, left, bottom, right);
  }

  public static string GetAlphaLabel(int col)
  {
    char[] chArray1 = new char[10];
    int length;
    for (length = 0; col > 0 && length < 9; ++length)
    {
      --col;
      chArray1[length] = (char) (col % 26 + 65);
      col /= 26;
    }
    char[] chArray2 = new char[length];
    for (int index = 0; index < length; ++index)
      chArray2[length - index - 1] = chArray1[index];
    return new string(chArray2);
  }
}
