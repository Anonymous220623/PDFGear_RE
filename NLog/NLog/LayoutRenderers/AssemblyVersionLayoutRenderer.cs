// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.AssemblyVersionLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("assembly-version")]
[ThreadAgnostic]
[ThreadSafe]
public class AssemblyVersionLayoutRenderer : LayoutRenderer
{
  private const string DefaultFormat = "major.minor.build.revision";
  private string _format;
  private string _assemblyVersion;

  public AssemblyVersionLayoutRenderer()
  {
    this.Type = AssemblyVersionType.Assembly;
    this.Format = "major.minor.build.revision";
  }

  [DefaultParameter]
  public string Name { get; set; }

  [DefaultValue("Assembly")]
  public AssemblyVersionType Type { get; set; }

  [DefaultValue("major.minor.build.revision")]
  public string Format
  {
    get => this._format;
    set => this._format = value?.ToLowerInvariant();
  }

  protected override void InitializeLayoutRenderer()
  {
    this._assemblyVersion = (string) null;
    base.InitializeLayoutRenderer();
  }

  protected override void CloseLayoutRenderer()
  {
    this._assemblyVersion = (string) null;
    base.CloseLayoutRenderer();
  }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    string str = this._assemblyVersion ?? (this._assemblyVersion = this.ApplyFormatToVersion(this.GetVersion()));
    if (string.IsNullOrEmpty(str))
      str = $"Could not find value for {(string.IsNullOrEmpty(this.Name) ? (object) "entry" : (object) this.Name)} assembly and version type {this.Type}";
    builder.Append(str);
  }

  private string ApplyFormatToVersion(string version)
  {
    if (this.Format.Equals("major.minor.build.revision", StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(version))
      return version;
    string[] strArray = version.SplitAndTrimTokens('.');
    version = this.Format.Replace("major", strArray[0]).Replace("minor", strArray.Length > 1 ? strArray[1] : "0").Replace("build", strArray.Length > 2 ? strArray[2] : "0").Replace("revision", strArray.Length > 3 ? strArray[3] : "0");
    return version;
  }

  private string GetVersion() => this.GetVersion(this.GetAssembly());

  protected virtual Assembly GetAssembly()
  {
    return string.IsNullOrEmpty(this.Name) ? Assembly.GetEntryAssembly() : Assembly.Load(new AssemblyName(this.Name));
  }

  private string GetVersion(Assembly assembly)
  {
    switch (this.Type)
    {
      case AssemblyVersionType.File:
        if ((object) assembly == null)
          return (string) null;
        return ReflectionHelpers.GetCustomAttribute<AssemblyFileVersionAttribute>(assembly)?.Version;
      case AssemblyVersionType.Informational:
        if ((object) assembly == null)
          return (string) null;
        return ReflectionHelpers.GetCustomAttribute<AssemblyInformationalVersionAttribute>(assembly)?.InformationalVersion;
      default:
        return assembly?.GetName().Version?.ToString();
    }
  }
}
