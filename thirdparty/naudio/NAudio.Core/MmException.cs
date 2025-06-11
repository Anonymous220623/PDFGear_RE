// Decompiled with JetBrains decompiler
// Type: NAudio.MmException
// Assembly: NAudio.Core, Version=2.2.1.0, Culture=neutral, PublicKeyToken=e279aa5131008a41
// MVID: 1DE6B66D-E24A-4618-BD87-23DB1CFE545D
// Assembly location: D:\PDFGear\bin\NAudio.Core.dll

using System;

#nullable disable
namespace NAudio;

public class MmException : Exception
{
  public MmException(MmResult result, string function)
    : base(MmException.ErrorMessage(result, function))
  {
    this.Result = result;
    this.Function = function;
  }

  private static string ErrorMessage(MmResult result, string function)
  {
    return $"{result} calling {function}";
  }

  public static void Try(MmResult result, string function)
  {
    if (result != MmResult.NoError)
      throw new MmException(result, function);
  }

  public MmResult Result { get; }

  public string Function { get; }
}
