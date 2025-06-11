// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.FloatApproxComparer
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class FloatApproxComparer : IComparer
{
  public FloatApproxComparer Instance = new FloatApproxComparer();

  public int Compare(object x, object y)
  {
    if ((double) (float) x - (double) (float) y < 9.9999997473787516E-05)
      return 0;
    return (double) (float) x > (double) (float) y ? 1 : -1;
  }
}
