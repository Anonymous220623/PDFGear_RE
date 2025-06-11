// Decompiled with JetBrains decompiler
// Type: Tesseract.Internal.Guard
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

using System;
using System.Diagnostics;

#nullable disable
namespace Tesseract.Internal;

internal static class Guard
{
  [DebuggerHidden]
  public static void Require(string paramName, bool condition)
  {
    if (!condition)
      throw new ArgumentException(string.Empty, paramName);
  }

  [DebuggerHidden]
  public static void Require(string paramName, bool condition, string message)
  {
    if (!condition)
      throw new ArgumentException(message, paramName);
  }

  [DebuggerHidden]
  public static void Require(
    string paramName,
    bool condition,
    string message,
    params object[] args)
  {
    if (!condition)
      throw new ArgumentException(string.Format(message, args), paramName);
  }

  [DebuggerHidden]
  public static void RequireNotNull(string argName, object value)
  {
    if (value == null)
      throw new ArgumentException($"Argument \"{value}\" must not be null.");
  }

  [DebuggerHidden]
  public static void RequireNotNullOrEmpty(string paramName, string value)
  {
    Guard.RequireNotNull(paramName, (object) value);
    if (value.Length == 0)
      throw new ArgumentException(paramName, $"The argument \"{paramName}\" must not be null or empty.");
  }

  [DebuggerHidden]
  public static void Verify(bool condition, string message, params object[] args)
  {
    if (!condition)
      throw new InvalidOperationException(string.Format(message, args));
  }
}
