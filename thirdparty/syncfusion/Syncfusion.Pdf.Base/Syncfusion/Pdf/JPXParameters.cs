// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.JPXParameters
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

internal class JPXParameters : Dictionary<string, string>
{
  private JPXParameters defaults;

  public JPXParameters DefaultParameterList => this.defaults;

  public JPXParameters()
  {
  }

  public JPXParameters(JPXParameters def) => this.defaults = def;

  public virtual string getParameter(string pname)
  {
    string parameter;
    if (this.ContainsKey(pname))
      parameter = this[pname];
    else
      this.defaults.TryGetValue(pname, out parameter);
    return parameter;
  }

  public virtual bool getBooleanParameter(string pname)
  {
    switch (this.getParameter(pname))
    {
      case null:
        throw new ArgumentException("No parameter with name " + pname);
      case "on":
        return true;
      case "off":
        return false;
      default:
        throw new Exception();
    }
  }

  public virtual int getIntParameter(string pname)
  {
    string parameter = this.getParameter(pname);
    if (parameter == null)
      throw new ArgumentException("No parameter with name " + pname);
    try
    {
      return int.Parse(parameter);
    }
    catch (FormatException ex)
    {
      throw new FormatException($"Parameter \"{pname}\" is not integer: {ex.Message}");
    }
  }

  public virtual float getFloatParameter(string pname)
  {
    string parameter = this.getParameter(pname);
    if (parameter == null)
      throw new ArgumentException("No parameter with name " + pname);
    try
    {
      return float.Parse(parameter);
    }
    catch (FormatException ex)
    {
      throw new FormatException($"Parameter \"{pname}\" is not floating-point: {ex.Message}");
    }
  }

  public virtual void checkList(char prfx, string[] plist)
  {
    IEnumerator enumerator = (IEnumerator) this.Keys.GetEnumerator();
    while (enumerator.MoveNext())
    {
      string current = (string) enumerator.Current;
      if (current.Length > 0 && (int) current[0] == (int) prfx)
      {
        bool flag = false;
        if (plist != null)
        {
          for (int index = plist.Length - 1; index >= 0; --index)
          {
            if (current.Equals(plist[index]))
            {
              flag = true;
              break;
            }
          }
        }
        if (!flag)
          throw new ArgumentException($"Option '{current}' is not a valid one.");
      }
    }
  }

  public static string[] toNameArray(string[][] pinfo)
  {
    if (pinfo == null)
      return (string[]) null;
    string[] nameArray = new string[pinfo.Length];
    for (int index = pinfo.Length - 1; index >= 0; --index)
      nameArray[index] = pinfo[index][0];
    return nameArray;
  }
}
