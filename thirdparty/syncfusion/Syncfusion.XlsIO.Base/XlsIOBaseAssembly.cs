// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIOBaseAssembly
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Reflection;

#nullable disable
namespace Syncfusion;

public class XlsIOBaseAssembly
{
  public static readonly string Name;
  public static readonly Assembly Assembly;
  public static readonly string RootNamespace = "Syncfusion.XlsIO";

  static XlsIOBaseAssembly()
  {
    XlsIOBaseAssembly.Assembly = typeof (XlsIOBaseAssembly).Assembly;
    string fullName = XlsIOBaseAssembly.Assembly.FullName;
    int length = fullName.IndexOf(",");
    XlsIOBaseAssembly.Name = fullName.Substring(0, length);
  }

  public static Assembly AssemblyResolver(object sender, ResolveEventArgs e)
  {
    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
    for (int index = 0; index < assemblies.Length; ++index)
    {
      if (assemblies[index].GetName().Name == e.Name)
        return assemblies[index];
    }
    return (Assembly) null;
  }
}
