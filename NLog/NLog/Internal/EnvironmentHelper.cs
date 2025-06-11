// Decompiled with JetBrains decompiler
// Type: NLog.Internal.EnvironmentHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Security;

#nullable disable
namespace NLog.Internal;

internal static class EnvironmentHelper
{
  internal static string NewLine => Environment.NewLine;

  internal static string GetMachineName()
  {
    try
    {
      return Environment.MachineName;
    }
    catch (SecurityException ex)
    {
      return string.Empty;
    }
  }

  internal static string GetSafeEnvironmentVariable(string name)
  {
    try
    {
      string environmentVariable = Environment.GetEnvironmentVariable(name);
      return string.IsNullOrEmpty(environmentVariable) ? (string) null : environmentVariable;
    }
    catch (SecurityException ex)
    {
      return (string) null;
    }
  }
}
