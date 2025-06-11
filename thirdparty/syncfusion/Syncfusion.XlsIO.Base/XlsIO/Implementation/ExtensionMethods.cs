// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ExtensionMethods
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal static class ExtensionMethods
{
  internal static void CustomAdd(
    this Dictionary<int, List<GroupPoint>> dictionary,
    int level,
    List<GroupPoint> value)
  {
    if (dictionary.Count == 0)
    {
      dictionary.Add(level, value);
    }
    else
    {
      int num1 = 0;
      bool flag = true;
      foreach (int key in dictionary.Keys)
      {
        if (level < key)
        {
          flag = false;
          break;
        }
        ++num1;
      }
      if (flag)
      {
        dictionary.Add(level, value);
      }
      else
      {
        Dictionary<int, List<GroupPoint>> dictionary1 = new Dictionary<int, List<GroupPoint>>();
        int num2 = 0;
        foreach (KeyValuePair<int, List<GroupPoint>> keyValuePair in dictionary)
        {
          if (num2 == num1)
            dictionary1.Add(level, value);
          dictionary1.Add(keyValuePair.Key, keyValuePair.Value);
          ++num2;
        }
        dictionary.Clear();
        foreach (KeyValuePair<int, List<GroupPoint>> keyValuePair in dictionary1)
          dictionary.Add(keyValuePair.Key, keyValuePair.Value);
        dictionary1.Clear();
        if (dictionary1 == null)
          ;
      }
    }
  }
}
