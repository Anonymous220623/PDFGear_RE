// Decompiled with JetBrains decompiler
// Type: Tesseract.Internal.Logger
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

using System.Diagnostics;

#nullable disable
namespace Tesseract.Internal;

internal static class Logger
{
  private static readonly TraceSource trace = new TraceSource("Tesseract");

  public static void TraceInformation(string format, params object[] args)
  {
  }

  public static void TraceError(string format, params object[] args)
  {
  }

  public static void TraceWarning(string format, params object[] args)
  {
  }
}
