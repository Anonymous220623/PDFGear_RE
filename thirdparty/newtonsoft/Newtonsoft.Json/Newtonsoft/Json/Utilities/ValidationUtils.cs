// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.ValidationUtils
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace Newtonsoft.Json.Utilities;

internal static class ValidationUtils
{
  public static void ArgumentNotNull([NotNull] object? value, string parameterName)
  {
    if (value == null)
      throw new ArgumentNullException(parameterName);
  }
}
