// Decompiled with JetBrains decompiler
// Type: CommomLib.Config.ConfigUtils
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace CommomLib.Config;

public static class ConfigUtils
{
  public static bool TryGet<T>(string key, out T value)
  {
    value = default (T);
    string str;
    if (string.IsNullOrEmpty(key) || !SqliteUtils.TryGet(key, out str))
      return false;
    if (str == "")
      return true;
    try
    {
      value = JsonConvert.DeserializeObject<T>(str);
      return true;
    }
    catch
    {
    }
    return false;
  }

  public static async Task<(bool result, T value)> TryGetAsync<T>(
    string key,
    CancellationToken cancellationToken)
  {
    if (string.IsNullOrEmpty(key))
      return ();
    (bool flag, string str) = await SqliteUtils.TryGetAsync(key, cancellationToken).ConfigureAwait(false);
    if (flag)
    {
      if (str == "")
        return (true, default (T));
      try
      {
        return (true, JsonConvert.DeserializeObject<T>(str));
      }
      catch
      {
      }
    }
    return ();
  }

  public static bool TrySet<T>(string key, T value)
  {
    if (string.IsNullOrEmpty(key))
      return false;
    string str = (object) value != null ? JsonConvert.SerializeObject((object) value, Formatting.Indented) : string.Empty;
    return SqliteUtils.TrySet(key, str);
  }

  public static async Task<bool> TrySetAsync<T>(
    string key,
    T value,
    CancellationToken cancellationToken)
  {
    return !string.IsNullOrEmpty(key) && await SqliteUtils.TrySetAsync(key, (object) (T) value != null ? JsonConvert.SerializeObject((object) (T) value, Formatting.Indented) : string.Empty, cancellationToken).ConfigureAwait(false);
  }
}
