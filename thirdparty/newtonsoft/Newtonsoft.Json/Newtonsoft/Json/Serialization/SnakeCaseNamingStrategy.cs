// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.SnakeCaseNamingStrategy
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;

#nullable enable
namespace Newtonsoft.Json.Serialization;

public class SnakeCaseNamingStrategy : NamingStrategy
{
  public SnakeCaseNamingStrategy(bool processDictionaryKeys, bool overrideSpecifiedNames)
  {
    this.ProcessDictionaryKeys = processDictionaryKeys;
    this.OverrideSpecifiedNames = overrideSpecifiedNames;
  }

  public SnakeCaseNamingStrategy(
    bool processDictionaryKeys,
    bool overrideSpecifiedNames,
    bool processExtensionDataNames)
    : this(processDictionaryKeys, overrideSpecifiedNames)
  {
    this.ProcessExtensionDataNames = processExtensionDataNames;
  }

  public SnakeCaseNamingStrategy()
  {
  }

  protected override string ResolvePropertyName(string name) => StringUtils.ToSnakeCase(name);
}
