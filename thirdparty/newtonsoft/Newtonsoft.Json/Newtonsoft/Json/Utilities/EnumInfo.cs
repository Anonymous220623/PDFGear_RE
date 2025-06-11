// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.EnumInfo
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

#nullable enable
namespace Newtonsoft.Json.Utilities;

internal class EnumInfo
{
  public readonly bool IsFlags;
  public readonly ulong[] Values;
  public readonly string[] Names;
  public readonly string[] ResolvedNames;

  public EnumInfo(bool isFlags, ulong[] values, string[] names, string[] resolvedNames)
  {
    this.IsFlags = isFlags;
    this.Values = values;
    this.Names = names;
    this.ResolvedNames = resolvedNames;
  }
}
