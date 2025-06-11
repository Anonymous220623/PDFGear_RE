// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIOBaseAssembly
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Reflection;

#nullable disable
namespace Syncfusion;

public class XlsIOBaseAssembly
{
  public static readonly string Name;
  public static readonly Assembly Assembly;
  public static readonly string RootNamespace = "Syncfusion.OfficeChart";

  static XlsIOBaseAssembly()
  {
    XlsIOBaseAssembly.Assembly = typeof (XlsIOBaseAssembly).Assembly;
    string fullName = XlsIOBaseAssembly.Assembly.FullName;
    int length = fullName.IndexOf(",");
    XlsIOBaseAssembly.Name = fullName.Substring(0, length);
  }
}
