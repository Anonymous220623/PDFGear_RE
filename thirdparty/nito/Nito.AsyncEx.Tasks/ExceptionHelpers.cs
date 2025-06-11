// Decompiled with JetBrains decompiler
// Type: ExceptionHelpers
// Assembly: Nito.AsyncEx.Tasks, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A9818263-2EB2-45AD-B24E-66ECAA8EEAB5
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Tasks.dll

using System;
using System.Runtime.ExceptionServices;

#nullable enable
internal static class ExceptionHelpers
{
  public static Exception PrepareForRethrow(Exception exception)
  {
    ExceptionDispatchInfo.Capture(exception).Throw();
    return exception;
  }
}
