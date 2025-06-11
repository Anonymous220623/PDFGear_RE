// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.StringComparer
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class StringComparer : IComparer
{
  public int Compare(object x, object y)
  {
    string strA = x as string;
    string strB = y as string;
    return strA != null && strB != null ? string.CompareOrdinal(strA, strB) : 0;
  }
}
