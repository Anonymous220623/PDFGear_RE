// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.SortPointByX
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal class SortPointByX : IComparer<PointF>
{
  public int Compare(PointF a, PointF b)
  {
    if ((double) a.X > (double) b.X)
      return 1;
    return (double) a.X < (double) b.X ? -1 : 0;
  }
}
