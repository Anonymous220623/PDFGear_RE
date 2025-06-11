// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.FieldFilter
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable enable
namespace Newtonsoft.Json.Linq.JsonPath;

internal class FieldFilter : PathFilter
{
  internal string? Name;

  public FieldFilter(string? name) => this.Name = name;

  public override IEnumerable<JToken> ExecuteFilter(
    JToken root,
    IEnumerable<JToken> current,
    JsonSelectSettings? settings)
  {
    foreach (JToken jtoken1 in current)
    {
      if (jtoken1 is JObject jobject)
      {
        if (this.Name != null)
        {
          JToken jtoken2 = jobject[this.Name];
          if (jtoken2 != null)
          {
            yield return jtoken2;
          }
          else
          {
            JsonSelectSettings jsonSelectSettings = settings;
            if ((jsonSelectSettings != null ? (jsonSelectSettings.ErrorWhenNoMatch ? 1 : 0) : 0) != 0)
              throw new JsonException("Property '{0}' does not exist on JObject.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this.Name));
          }
        }
        else
        {
          foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
            yield return keyValuePair.Value;
        }
      }
      else
      {
        JsonSelectSettings jsonSelectSettings = settings;
        if ((jsonSelectSettings != null ? (jsonSelectSettings.ErrorWhenNoMatch ? 1 : 0) : 0) != 0)
          throw new JsonException("Property '{0}' not valid on {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) (this.Name ?? "*"), (object) jtoken1.GetType().Name));
      }
    }
  }
}
