// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.RegistryLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using Microsoft.Win32;
using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("registry")]
public class RegistryLayoutRenderer : LayoutRenderer
{
  private static readonly Dictionary<string, RegistryHive> HiveAliases = new Dictionary<string, RegistryHive>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
  {
    {
      "HKEY_LOCAL_MACHINE",
      RegistryHive.LocalMachine
    },
    {
      "HKLM",
      RegistryHive.LocalMachine
    },
    {
      "HKEY_CURRENT_USER",
      RegistryHive.CurrentUser
    },
    {
      "HKCU",
      RegistryHive.CurrentUser
    },
    {
      "HKEY_CLASSES_ROOT",
      RegistryHive.ClassesRoot
    },
    {
      "HKEY_USERS",
      RegistryHive.Users
    },
    {
      "HKEY_CURRENT_CONFIG",
      RegistryHive.CurrentConfig
    },
    {
      "HKEY_DYN_DATA",
      RegistryHive.DynData
    },
    {
      "HKEY_PERFORMANCE_DATA",
      RegistryHive.PerformanceData
    }
  };

  public RegistryLayoutRenderer() => this.RequireEscapingSlashesInDefaultValue = true;

  public Layout Value { get; set; }

  public Layout DefaultValue { get; set; }

  [System.ComponentModel.DefaultValue(true)]
  public bool RequireEscapingSlashesInDefaultValue { get; set; }

  [System.ComponentModel.DefaultValue("Default")]
  public RegistryView View { get; set; }

  [RequiredParameter]
  public Layout Key { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    object obj = (object) null;
    string name = this.Value?.Render(logEvent);
    RegistryLayoutRenderer.ParseResult key = RegistryLayoutRenderer.ParseKey(this.Key.Render(logEvent));
    try
    {
      using (RegistryKey registryKey1 = RegistryKey.OpenBaseKey(key.Hive, this.View))
      {
        if (key.HasSubKey)
        {
          using (RegistryKey registryKey2 = registryKey1.OpenSubKey(key.SubKey))
          {
            if (registryKey2 != null)
              obj = registryKey2.GetValue(name);
          }
        }
        else
          obj = registryKey1.GetValue(name);
      }
    }
    catch (Exception ex)
    {
      InternalLogger.Error("Error when writing to registry");
      if (ex.MustBeRethrown())
        throw;
    }
    string str = (string) null;
    if (obj != null)
      str = Convert.ToString(obj, (IFormatProvider) CultureInfo.InvariantCulture);
    else if (this.DefaultValue != null)
    {
      str = this.DefaultValue.Render(logEvent);
      if (this.RequireEscapingSlashesInDefaultValue)
        str = str.Replace("\\\\", "\\");
    }
    builder.Append(str);
  }

  private static RegistryLayoutRenderer.ParseResult ParseKey(string key)
  {
    int length = key.IndexOfAny(new char[2]{ '\\', '/' });
    string str = (string) null;
    string hiveName1;
    if (length >= 0)
    {
      hiveName1 = key.Substring(0, length);
      str = key.Substring(length + 1).Replace('/', '\\').TrimStart('\\').Replace("\\\\", "\\");
    }
    else
      hiveName1 = key;
    RegistryHive hiveName2 = RegistryLayoutRenderer.ParseHiveName(hiveName1);
    return new RegistryLayoutRenderer.ParseResult()
    {
      SubKey = str,
      Hive = hiveName2
    };
  }

  private static RegistryHive ParseHiveName(string hiveName)
  {
    RegistryHive hiveName1;
    if (RegistryLayoutRenderer.HiveAliases.TryGetValue(hiveName, out hiveName1))
      return hiveName1;
    throw new ArgumentException($"Key name is not supported. Root hive '{hiveName}' not recognized.");
  }

  private class ParseResult
  {
    public string SubKey { get; set; }

    public RegistryHive Hive { get; set; }

    public bool HasSubKey => !string.IsNullOrEmpty(this.SubKey);
  }
}
