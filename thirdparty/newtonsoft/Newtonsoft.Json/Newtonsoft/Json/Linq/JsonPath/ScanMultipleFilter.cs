// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.ScanMultipleFilter
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System.Collections.Generic;

#nullable enable
namespace Newtonsoft.Json.Linq.JsonPath;

internal class ScanMultipleFilter : PathFilter
{
  private List<string> _names;

  public ScanMultipleFilter(List<string> names) => this._names = names;

  public override IEnumerable<JToken> ExecuteFilter(
    JToken root,
    IEnumerable<JToken> current,
    JsonSelectSettings? settings)
  {
    foreach (JToken c in current)
    {
      JToken value = c;
      while (true)
      {
        value = PathFilter.GetNextScanValue(c, (JToken) (value as JContainer), value);
        if (value != null)
        {
          if (value is JProperty property)
          {
            foreach (string name in this._names)
            {
              if (property.Name == name)
                yield return property.Value;
            }
          }
          property = (JProperty) null;
        }
        else
          break;
      }
      value = (JToken) null;
    }
  }
}
