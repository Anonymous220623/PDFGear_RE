// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.ErrorContext
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;

#nullable enable
namespace Newtonsoft.Json.Serialization;

public class ErrorContext
{
  internal ErrorContext(object? originalObject, object? member, string path, Exception error)
  {
    this.OriginalObject = originalObject;
    this.Member = member;
    this.Error = error;
    this.Path = path;
  }

  internal bool Traced { get; set; }

  public Exception Error { get; }

  public object? OriginalObject { get; }

  public object? Member { get; }

  public string Path { get; }

  public bool Handled { get; set; }
}
