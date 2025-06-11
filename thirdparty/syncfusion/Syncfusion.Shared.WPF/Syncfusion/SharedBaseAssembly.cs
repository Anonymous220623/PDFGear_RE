// Decompiled with JetBrains decompiler
// Type: Syncfusion.SharedBaseAssembly
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Reflection;

#nullable disable
namespace Syncfusion;

public class SharedBaseAssembly
{
  public static readonly string Name;
  public static readonly Assembly Assembly;
  public static readonly string RootNamespace = "Syncfusion.Windows.Shared";

  static SharedBaseAssembly()
  {
    SharedBaseAssembly.Assembly = typeof (SharedBaseAssembly).Assembly;
    string fullName = SharedBaseAssembly.Assembly.FullName;
    int length = fullName.IndexOf(",");
    SharedBaseAssembly.Name = fullName.Substring(0, length);
  }

  public static Assembly AssemblyResolver(object sender, ResolveEventArgs e)
  {
    if (e.Name.StartsWith(SharedBaseAssembly.Name))
      return SharedBaseAssembly.Assembly;
    string lower = e.Name.ToLower();
    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
    for (int index = 0; index < assemblies.Length; ++index)
    {
      if (assemblies[index].GetName().Name.ToLower() == lower)
        return assemblies[index];
    }
    return (Assembly) null;
  }
}
