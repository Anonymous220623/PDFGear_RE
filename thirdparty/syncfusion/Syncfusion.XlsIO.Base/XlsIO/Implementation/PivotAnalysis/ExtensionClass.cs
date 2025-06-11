// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.ExtensionClass
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public static class ExtensionClass
{
  public static bool Has(this List<HiddenGroup> t, HiddenGroup hiddenGroup)
  {
    bool flag = false;
    foreach (HiddenGroup hiddenGroup1 in t)
    {
      if (hiddenGroup1.From == hiddenGroup.From && hiddenGroup1.To == hiddenGroup.To && hiddenGroup1.GroupName == hiddenGroup.GroupName && hiddenGroup1.Level == hiddenGroup.Level)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }
}
