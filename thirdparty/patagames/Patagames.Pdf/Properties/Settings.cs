// Decompiled with JetBrains decompiler
// Type: Patagames.Properties.Settings
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable disable
namespace Patagames.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.3.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
  private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

  public static Settings Default => Settings.defaultInstance;

  [ApplicationScopedSetting]
  [DebuggerNonUserCode]
  [DefaultSettingValue("0")]
  public uint LogLevel => (uint) this[nameof (LogLevel)];
}
