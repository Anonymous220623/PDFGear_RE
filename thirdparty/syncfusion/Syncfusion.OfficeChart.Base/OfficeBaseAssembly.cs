// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeBaseAssembly
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Reflection;

#nullable disable
namespace Syncfusion;

internal class OfficeBaseAssembly
{
  public static readonly string Name;
  public static readonly Assembly Assembly;
  public static readonly string RootNamespace = "Syncfusion.OfficeChart";

  static OfficeBaseAssembly()
  {
    OfficeBaseAssembly.Assembly = typeof (OfficeBaseAssembly).Assembly;
    string fullName = OfficeBaseAssembly.Assembly.FullName;
    int length = fullName.IndexOf(",");
    OfficeBaseAssembly.Name = fullName.Substring(0, length);
  }
}
