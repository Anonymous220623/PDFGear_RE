// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.Presentation.Net.ItemNamesComparer
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.CompoundFile.Presentation.Net;

internal class ItemNamesComparer : IComparer, IComparer<string>
{
  public int Compare(object x, object y)
  {
    if (x == null && y == null)
      return 0;
    if (y == null)
      return 1;
    if (x == null)
      return -1;
    string x1 = x.ToString();
    string y1 = y.ToString();
    int num = x1.Length - y1.Length;
    if (num == 0)
      num = StringComparer.Ordinal.Compare(x1, y1);
    return num;
  }

  public int Compare(string x, string y)
  {
    if (x == null && y == null)
      return 0;
    if (y == null)
      return 1;
    if (x == null)
      return -1;
    int num = x.Length - y.Length;
    if (num == 0)
      num = StringComparer.Ordinal.Compare(x.ToUpper(), y.ToUpper());
    return num;
  }
}
