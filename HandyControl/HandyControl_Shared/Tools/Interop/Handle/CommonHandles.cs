// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.Interop.CommonHandles
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

#nullable disable
namespace HandyControl.Tools.Interop;

internal static class CommonHandles
{
  public static readonly int Icon = HandleCollector.RegisterType(nameof (Icon), 20, 500);
  public static readonly int HDC = HandleCollector.RegisterType(nameof (HDC), 100, 2);
  public static readonly int GDI = HandleCollector.RegisterType(nameof (GDI), 50, 500);
  public static readonly int Kernel = HandleCollector.RegisterType(nameof (Kernel), 0, 1000);
}
