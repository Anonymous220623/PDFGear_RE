// Decompiled with JetBrains decompiler
// Type: Patagames.Activation.Log
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using Patagames.Properties;
using System;
using System.Diagnostics;

#nullable disable
namespace Patagames.Activation;

internal class Log
{
  private static uint LogLevel
  {
    get
    {
      try
      {
        return Settings.Default.LogLevel;
      }
      catch (Exception ex)
      {
        Trace.TraceError(ex.Message);
        return 0;
      }
    }
  }

  public static void Info(string message, params object[] args)
  {
    if (Log.LogLevel == 0U)
      return;
    Trace.TraceInformation(message, args);
  }

  public static void Error(string message, params object[] args)
  {
    if (Log.LogLevel == 0U)
      return;
    Trace.TraceError(message, args);
  }
}
