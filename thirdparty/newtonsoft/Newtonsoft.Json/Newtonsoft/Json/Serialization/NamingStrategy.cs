// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.NamingStrategy
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

#nullable enable
namespace Newtonsoft.Json.Serialization;

public abstract class NamingStrategy
{
  public bool ProcessDictionaryKeys { get; set; }

  public bool ProcessExtensionDataNames { get; set; }

  public bool OverrideSpecifiedNames { get; set; }

  public virtual string GetPropertyName(string name, bool hasSpecifiedName)
  {
    return hasSpecifiedName && !this.OverrideSpecifiedNames ? name : this.ResolvePropertyName(name);
  }

  public virtual string GetExtensionDataName(string name)
  {
    return !this.ProcessExtensionDataNames ? name : this.ResolvePropertyName(name);
  }

  public virtual string GetDictionaryKey(string key)
  {
    return !this.ProcessDictionaryKeys ? key : this.ResolvePropertyName(key);
  }

  protected abstract string ResolvePropertyName(string name);

  public override int GetHashCode()
  {
    return ((this.GetType().GetHashCode() * 397 ^ this.ProcessDictionaryKeys.GetHashCode()) * 397 ^ this.ProcessExtensionDataNames.GetHashCode()) * 397 ^ this.OverrideSpecifiedNames.GetHashCode();
  }

  public override bool Equals(object obj) => this.Equals(obj as NamingStrategy);

  protected bool Equals(NamingStrategy? other)
  {
    return other != null && this.GetType() == other.GetType() && this.ProcessDictionaryKeys == other.ProcessDictionaryKeys && this.ProcessExtensionDataNames == other.ProcessExtensionDataNames && this.OverrideSpecifiedNames == other.OverrideSpecifiedNames;
  }
}
