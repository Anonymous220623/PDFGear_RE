// Decompiled with JetBrains decompiler
// Type: RegExt.FileAssociations.RegisterExtensions
// Assembly: RegExt, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: DBF16820-DB7E-4C29-8C11-DD0B94851B7F
// Assembly location: C:\Program Files\PDFgear\RegExt.exe

using Microsoft.Win32;

#nullable disable
namespace RegExt.FileAssociations;

internal static class RegisterExtensions
{
  internal static void Write32(this RegistryHive hive, string subKey, string value)
  {
    hive.Write32(subKey, "", value);
  }

  internal static void Write32(this RegistryHive hive, string subKey, string key, string value)
  {
    using (RegistryKey registryKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default))
    {
      using (RegistryKey subKey1 = registryKey.CreateSubKey(subKey, true))
        subKey1.SetValue(key, (object) value);
    }
  }
}
