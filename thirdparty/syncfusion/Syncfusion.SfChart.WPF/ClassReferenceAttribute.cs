// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ClassReferenceAttribute
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Reflection;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public sealed class ClassReferenceAttribute : Attribute
{
  private bool isReviewed;
  private bool shouldInclude = true;

  public bool IsReviewed
  {
    get => this.isReviewed;
    set
    {
      if (this.isReviewed == value)
        return;
      this.isReviewed = value;
    }
  }

  public bool ShouldInclude
  {
    get => this.shouldInclude;
    set
    {
      if (this.shouldInclude == value)
        return;
      this.shouldInclude = value;
    }
  }

  public static Assembly AssemblyResolver(object sender, ResolveEventArgs e)
  {
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
