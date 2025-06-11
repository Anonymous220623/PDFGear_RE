// Decompiled with JetBrains decompiler
// Type: System.Diagnostics.CodeAnalysis.NotNullWhenAttribute
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

#nullable disable
namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
internal sealed class NotNullWhenAttribute : Attribute
{
  public NotNullWhenAttribute(bool returnValue) => this.ReturnValue = returnValue;

  public bool ReturnValue { get; }
}
