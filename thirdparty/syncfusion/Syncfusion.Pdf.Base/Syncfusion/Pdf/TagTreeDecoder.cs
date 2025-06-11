// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.TagTreeDecoder
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class TagTreeDecoder
{
  internal int w;
  internal int h;
  internal int lvls;
  internal int[][] treeV;
  internal int[][] treeS;

  public virtual int Width => this.w;

  public virtual int Height => this.h;

  public TagTreeDecoder(int h, int w)
  {
    this.w = w >= 0 && h >= 0 ? w : throw new ArgumentException();
    this.h = h;
    if (w == 0 || h == 0)
    {
      this.lvls = 0;
    }
    else
    {
      this.lvls = 1;
      while (h != 1 || w != 1)
      {
        w = w + 1 >> 1;
        h = h + 1 >> 1;
        ++this.lvls;
      }
    }
    this.treeV = new int[this.lvls][];
    this.treeS = new int[this.lvls][];
    w = this.w;
    h = this.h;
    for (int index = 0; index < this.lvls; ++index)
    {
      this.treeV[index] = new int[h * w];
      ArrayUtil.intArraySet(this.treeV[index], int.MaxValue);
      this.treeS[index] = new int[h * w];
      w = w + 1 >> 1;
      h = h + 1 >> 1;
    }
  }

  public virtual int update(int m, int n, int t, PktHeaderBitReader in_Renamed)
  {
    if (m >= this.h || n >= this.w || t < 0)
      throw new ArgumentException();
    int index1 = this.lvls - 1;
    int num1 = this.treeS[index1][0];
    int index2 = (m >> index1) * (this.w + (1 << index1) - 1 >> index1) + (n >> index1);
    int num2;
    while (true)
    {
      int num3 = this.treeS[index1][index2];
      num2 = this.treeV[index1][index2];
      if (num3 < num1)
        num3 = num1;
      while (t > num3)
      {
        if (num2 >= num3)
        {
          if (in_Renamed.readBit() == 0)
            ++num3;
          else
            num2 = num3++;
        }
        else
        {
          num3 = t;
          break;
        }
      }
      this.treeS[index1][index2] = num3;
      this.treeV[index1][index2] = num2;
      if (index1 > 0)
      {
        num1 = num3 < num2 ? num3 : num2;
        --index1;
        index2 = (m >> index1) * (this.w + (1 << index1) - 1 >> index1) + (n >> index1);
      }
      else
        break;
    }
    return num2;
  }

  public virtual int getValue(int m, int n)
  {
    if (m >= this.h || n >= this.w)
      throw new ArgumentException();
    return this.treeV[0][m * this.w + n];
  }
}
