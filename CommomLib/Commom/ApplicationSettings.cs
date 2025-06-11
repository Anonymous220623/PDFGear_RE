// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.ApplicationSettings
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using CommomLib.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#nullable disable
namespace CommomLib.Commom;

public class ApplicationSettings
{
  public ApplicationSettings()
  {
    this.Values = (IDictionary<string, string>) new ApplicationSettings.SqliteDictionary();
  }

  public IDictionary<string, string> Values { get; }

  private class SqliteDictionary : 
    IDictionary<string, string>,
    ICollection<KeyValuePair<string, string>>,
    IEnumerable<KeyValuePair<string, string>>,
    IEnumerable
  {
    public string this[string key]
    {
      get
      {
        string str;
        return SqliteUtils.TryGet(key, out str) ? str : string.Empty;
      }
      set => SqliteUtils.TrySet(key, value ?? "");
    }

    public ICollection<string> Keys
    {
      get
      {
        return (ICollection<string>) SqliteUtils.GetAllKeysAsync(new CancellationToken()).GetAwaiter().GetResult().ToArray<string>();
      }
    }

    public ICollection<string> Values
    {
      get
      {
        return (ICollection<string>) this.Keys.Select<string, string>((Func<string, string>) (c => this[c])).ToArray<string>();
      }
    }

    public int Count => SqliteUtils.GetCountAsync(new CancellationToken()).GetAwaiter().GetResult();

    public bool IsReadOnly => false;

    public void Add(string key, string value) => this[key] = value;

    public void Add(KeyValuePair<string, string> item) => this.Add(item.Key, item.Value);

    public void Clear() => throw new NotImplementedException();

    public bool Contains(KeyValuePair<string, string> item) => this[item.Key] == item.Value;

    public bool ContainsKey(string key) => !string.IsNullOrEmpty(this[key]);

    public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
    {
      IEnumerator<KeyValuePair<string, string>> enumerator = this.GetEnumerator();
      int num = 0;
      while (enumerator.MoveNext())
      {
        array[num + arrayIndex] = enumerator.Current;
        ++num;
      }
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
      List<string> list = this.Keys.ToList<string>();
      int i = 0;
      foreach (string key in list)
      {
        string str = this[key];
        if (!string.IsNullOrEmpty(str))
        {
          yield return new KeyValuePair<string, string>(key, str);
          ++i;
        }
      }
    }

    public bool Remove(string key)
    {
      if (!this.ContainsKey(key))
        return false;
      this[key] = string.Empty;
      return true;
    }

    public bool Remove(KeyValuePair<string, string> item)
    {
      return this.Contains(item) && this.Remove(item.Key);
    }

    public bool TryGetValue(string key, out string value)
    {
      value = this[key];
      return !string.IsNullOrEmpty(value);
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();
  }
}
