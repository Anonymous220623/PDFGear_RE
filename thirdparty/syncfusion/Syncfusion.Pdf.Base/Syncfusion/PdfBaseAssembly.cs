// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfBaseAssembly
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Reflection;

#nullable disable
namespace Syncfusion;

public class PdfBaseAssembly
{
  public const string RootNamespace = "Syncfusion.Pdf";
  public static readonly string Name;
  public static readonly Assembly Assembly = typeof (PdfBaseAssembly).Assembly;

  static PdfBaseAssembly()
  {
    string fullName = PdfBaseAssembly.Assembly.FullName;
    int length = fullName.IndexOf(",");
    PdfBaseAssembly.Name = fullName.Substring(0, length);
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
