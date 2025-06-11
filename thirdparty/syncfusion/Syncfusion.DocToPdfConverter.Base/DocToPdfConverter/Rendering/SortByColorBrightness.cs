// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocToPdfConverter.Rendering.SortByColorBrightness
// Assembly: Syncfusion.DocToPDFConverter.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 84EFC094-D348-494C-A410-44F5807BB0D3
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocToPdfConverter.Base.dll

using Syncfusion.DocIO.DLS;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocToPdfConverter.Rendering;

internal class SortByColorBrightness : IComparer<Border>
{
  public int Compare(Border firstBorder, Border secondBorder)
  {
    if (firstBorder == null || secondBorder == null)
      return 0;
    Color color1 = firstBorder.Color;
    Color color2 = secondBorder.Color;
    int num1 = (int) color1.R + (int) color1.B + 2 * (int) color1.G;
    int num2 = (int) color2.R + (int) color2.B + 2 * (int) color2.G;
    if (num1 < num2)
      return 1;
    return num1 > num2 ? -1 : 0;
  }
}
