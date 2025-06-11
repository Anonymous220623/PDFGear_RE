// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SmartArtImplementation.SmartArtLayout
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;

#nullable disable
namespace Syncfusion.Presentation.SmartArtImplementation;

internal class SmartArtLayout
{
  private SmartArt _smartArt;
  private SmartArtLayoutNode _layoutNode;

  internal SmartArtLayout(SmartArt smartArt) => this._smartArt = smartArt;

  internal SmartArtLayoutNode LayoutNode
  {
    get => this._layoutNode ?? (this._layoutNode = new SmartArtLayoutNode(this));
  }

  internal void BasicBlockListLayouting()
  {
    SmartArtNodes nodes = this._smartArt.Nodes as SmartArtNodes;
    double num1 = Math.Round(this._smartArt.ShapeFrame.GetDefaultWidth() * 12700.0);
    double num2 = Math.Round(this._smartArt.ShapeFrame.GetDefaultHeight() * 12700.0);
    double num3 = 0.0;
    double num4 = 0.0;
    double num5 = 0.0;
    double num6 = 0.0;
    double num7 = 0.0;
    double num8 = 0.6;
    double num9 = 0.1;
    double num10 = 0.0;
    double num11 = 0.0;
    int num12 = 1;
    int num13 = 1;
    int num14 = 0;
    bool flag = true;
    int num15 = 0;
    for (int index = 0; index < nodes.Count; ++index)
    {
      ++num14;
      if (num10 == 0.0 && num11 == 0.0)
      {
        num3 = num1;
        num4 = num3 * 0.6;
        num5 = num3 * 0.1;
        if (num4 > num2)
        {
          num4 = num2;
          num3 = num4 / num8;
          num5 = num3 * num9;
          num7 = 0.0;
          num6 = (num1 - num3) / 2.0;
        }
        else
        {
          num6 = 0.0;
          num7 = (num2 - num4) / 2.0;
        }
        num10 = num3;
        num11 = num4;
        flag = false;
      }
      else if (num10 == num1 && num11 < num2)
      {
        ++num12;
        num3 -= num3 / (double) num12;
        num4 = num3 * num8;
        num5 = num3 * num9;
        double num16 = num4 * (double) num12 + num5 * (double) (num12 - 1);
        while (num16 != num2)
        {
          double num17 = num2 - num16;
          num3 = (num4 + num17 / (double) num12) / num8;
          num4 = num3 * num8;
          num5 = num3 * num9;
          num11 = Math.Round(num4 * (double) num12 + num5 * (double) (num12 - 1), 4);
          num16 = num11;
          flag = true;
        }
        if (num3 > num1)
        {
          num3 = num1;
          num4 = num3 * num8;
          num5 = num3 * num9;
          num11 = Math.Round(num4 * (double) num12 + num5 * (double) (num12 - 1), 4);
          flag = false;
        }
        num15 = num14 % num13;
        num10 = Math.Round(num15 != 0 ? num3 * (double) num15 + num5 * (double) (num15 - 1) : num3 * (double) num13 + num5 * (double) (num13 - 1));
      }
      else if (num11 == num2 && num10 < num1)
      {
        if (num1 >= num10 + num3 + num5)
        {
          flag = true;
        }
        else
        {
          ++num13;
          for (double num18 = num3 * (double) num13 + num5 * (double) (num13 - 1); num18 != num1; num18 = Math.Round(num3 * (double) num13 + num5 * (double) (num13 - 1), 4))
          {
            double num19 = num1 - num18;
            num3 += num19 / (double) num13;
            num4 = num3 * num8;
            num5 = num3 * num9;
          }
          flag = false;
        }
        num11 = Math.Round(num4 * (double) num12 + num5 * (double) (num12 - 1), 4);
        num15 = num14 % num13;
        num10 = Math.Round(num15 != 0 ? num3 * (double) num15 + num5 * (double) (num15 - 1) : num3 * (double) num13 + num5 * (double) (num13 - 1), 4);
      }
      else if (num11 < num2 && num10 < num1)
      {
        double num20 = num4 * (double) num12 + num5 * (double) (num12 - 1);
        num15 = num14 % num13;
        double num21 = num15 != 0 ? num3 * (double) num15 + num5 * (double) (num15 - 1) : num3 * (double) num13 + num5 * (double) (num13 - 1);
        num11 = Math.Round(num20, 4);
        num10 = Math.Round(num21, 4);
        flag = false;
      }
    }
    double num22 = num3 / 12700.0;
    double num23 = num4 / 12700.0;
    double num24 = num5 / 12700.0;
    double num25 = num2 / 12700.0;
    double num26 = num1 / 12700.0;
    if (flag)
    {
      double num27 = (num26 - (num22 * (double) num13 + num24 * (double) (num13 - 1))) / 2.0;
      double num28 = num26 - num27;
      int num29 = 0;
      int num30 = 0;
      double num31 = 0.0;
      for (int index = 0; index < nodes.Count - num15; ++index)
      {
        nodes[index].Shapes[0].Width = num22;
        nodes[index].Shapes[0].Height = num23;
        double num32 = num27 + num22 * (double) num30 + num24 * (double) num30;
        if (num32 >= num28)
        {
          nodes[index].Shapes[0].Left = num27;
          ++num29;
          num31 = 0.0 + num23 * (double) num29 + num24 * (double) num29;
          nodes[index].Shapes[0].Top = num31;
          num30 = 1;
        }
        else
        {
          nodes[index].Shapes[0].Left = num32;
          nodes[index].Shapes[0].Top = num31;
          ++num30;
        }
      }
      int num33 = 0;
      int num34 = num29 + 1;
      for (int index = nodes.Count - num15; index < nodes.Count; ++index)
      {
        double num35 = (num26 - (num22 * (double) num15 + num24 * (double) (num15 - 1))) / 2.0;
        nodes[index].Shapes[0].Width = num22;
        nodes[index].Shapes[0].Height = num23;
        double num36 = num35 + num22 * (double) num33 + num24 * (double) num33;
        double num37 = 0.0 + (num23 * (double) num34 + num24 * (double) num34);
        nodes[index].Shapes[0].Left = num36;
        nodes[index].Shapes[0].Top = num37;
        ++num33;
      }
    }
    else
    {
      double num38 = (num25 - (num23 * (double) num12 + num24 * (double) (num12 - 1))) / 2.0;
      double num39 = num38;
      int num40 = 0;
      int num41 = 0;
      double num42 = 0.0;
      for (int index = 0; index < nodes.Count - num15; ++index)
      {
        nodes[index].Shapes[0].Width = num22;
        nodes[index].Shapes[0].Height = num23;
        double num43 = num42 + num22 * (double) num41 + num24 * (double) num41;
        double num44 = num38;
        if (num43 >= num26)
        {
          num42 = 0.0;
          nodes[index].Shapes[0].Left = num42;
          ++num40;
          double num45 = num39 + num23 * (double) num40 + num24 * (double) num40;
          nodes[index].Shapes[0].Top = num45;
          num38 = num45;
          num41 = 1;
        }
        else
        {
          nodes[index].Shapes[0].Left = num43;
          num42 = 0.0;
          nodes[index].Shapes[0].Top = num44;
          ++num41;
        }
      }
      int num46 = 0;
      int num47 = num40 + 1;
      for (int index = nodes.Count - num15; index < nodes.Count; ++index)
      {
        double num48 = (num26 - (num22 * (double) num15 + num24 * (double) (num15 - 1))) / 2.0;
        nodes[index].Shapes[0].Width = num22;
        nodes[index].Shapes[0].Height = num23;
        double num49 = num48 + num22 * (double) num46 + num24 * (double) num46;
        double num50 = num39 + (num23 * (double) num47 + num24 * (double) num47);
        nodes[index].Shapes[0].Left = num49;
        nodes[index].Shapes[0].Top = num50;
        ++num46;
      }
    }
  }
}
