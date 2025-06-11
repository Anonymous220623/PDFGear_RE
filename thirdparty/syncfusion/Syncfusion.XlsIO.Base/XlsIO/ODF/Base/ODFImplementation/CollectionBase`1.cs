// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.CollectionBase`1
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Globalization;

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class CollectionBase<T>
{
  internal static string GenerateDefaultName(string strStart, params ICollection[] arrCollections)
  {
    int val2 = 1;
    strStart = strStart.ToUpper();
    int length1 = strStart.Length;
    int index = 0;
    for (int length2 = arrCollections.Length; index < length2; ++index)
    {
      foreach (object obj in (IEnumerable) arrCollections[index])
      {
        string str = !(obj is INamedObject) ? obj.ToString() : (obj as INamedObject).Name;
        if (str.StartsWith(strStart))
        {
          string s = str.Substring(length1, str.Length - length1);
          double result;
          if (double.TryParse(s, NumberStyles.Integer, (IFormatProvider) null, out result))
            val2 = Math.Max((int) result + 1, val2);
          else if (s == "")
            ++val2;
        }
      }
    }
    return strStart + val2.ToString();
  }
}
