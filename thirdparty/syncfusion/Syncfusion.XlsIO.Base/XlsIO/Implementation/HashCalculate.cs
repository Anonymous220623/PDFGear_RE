// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.HashCalculate
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

public sealed class HashCalculate
{
  private static int m_level;

  private HashCalculate()
  {
  }

  public static int CalculateHash(object value, string[] skipProps)
  {
    HashCalculate.m_level = 0;
    StringBuilder builder = new StringBuilder(8192 /*0x2000*/);
    List<string> toSkip = new List<string>((IEnumerable<string>) skipProps);
    toSkip.Sort();
    HashCalculate.ObjectToString(builder, value, toSkip);
    return builder.ToString().GetHashCode();
  }

  private static void ObjectToString(StringBuilder builder, object value, List<string> toSkip)
  {
    ++HashCalculate.m_level;
    Type type1 = value.GetType();
    if (!type1.IsPrimitive)
    {
      PropertyInfo[] properties = type1.GetProperties();
      if (properties.Length > 0)
      {
        for (int index = 0; index < properties.Length; ++index)
        {
          if (properties[index].CanRead)
          {
            if (toSkip.BinarySearch(properties[index].Name) < 0)
            {
              try
              {
                object obj = properties[index].GetValue(value, new object[0]);
                if (obj != null)
                {
                  Type type2 = obj.GetType();
                  if (obj is string)
                    builder.Append(obj.ToString());
                  else if (obj is ICollection)
                  {
                    IEnumerator enumerator = ((IEnumerable) obj).GetEnumerator();
                    enumerator.Reset();
                    int num = 0;
                    while (enumerator.MoveNext())
                    {
                      HashCalculate.ObjectToString(builder, enumerator.Current, toSkip);
                      ++num;
                    }
                  }
                  else if (!type2.IsPrimitive)
                    HashCalculate.ObjectToString(builder, obj, toSkip);
                  else
                    builder.Append(obj.ToString());
                }
                else
                  builder.Append("null");
              }
              catch (Exception ex)
              {
              }
            }
          }
        }
      }
      else
        builder.Append(value.ToString());
    }
    else
      builder.Append(value.ToString());
    --HashCalculate.m_level;
  }
}
