// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.FSharpFunction
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

#nullable enable
namespace Newtonsoft.Json.Utilities;

internal class FSharpFunction
{
  private readonly object? _instance;
  private readonly MethodCall<object?, object> _invoker;

  public FSharpFunction(object? instance, MethodCall<object?, object> invoker)
  {
    this._instance = instance;
    this._invoker = invoker;
  }

  public object Invoke(params object[] args) => this._invoker(this._instance, args);
}
