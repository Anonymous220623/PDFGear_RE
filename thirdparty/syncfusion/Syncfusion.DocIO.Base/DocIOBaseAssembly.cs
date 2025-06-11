// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIOBaseAssembly
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Reflection;

#nullable disable
namespace Syncfusion;

public class DocIOBaseAssembly
{
  public static readonly string Name;
  public static readonly Assembly Assembly;
  public static readonly string RootNamespace = "Syncfusion.DocIO";

  static DocIOBaseAssembly()
  {
    DocIOBaseAssembly.Assembly = typeof (DocIOBaseAssembly).Assembly;
    string fullName = DocIOBaseAssembly.Assembly.FullName;
    int length = fullName.IndexOf(",");
    DocIOBaseAssembly.Name = fullName.Substring(0, length);
  }
}
