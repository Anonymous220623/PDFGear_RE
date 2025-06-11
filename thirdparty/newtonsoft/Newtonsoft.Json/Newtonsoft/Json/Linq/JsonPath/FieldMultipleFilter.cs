// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.FieldMultipleFilter
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#nullable enable
namespace Newtonsoft.Json.Linq.JsonPath;

internal class FieldMultipleFilter : PathFilter
{
  internal List<string> Names;

  public FieldMultipleFilter(List<string> names) => this.Names = names;

  public override IEnumerable<JToken> ExecuteFilter(
    JToken root,
    IEnumerable<JToken> current,
    JsonSelectSettings? settings)
  {
    foreach (JToken jtoken1 in current)
    {
      if (jtoken1 is JObject o)
      {
        foreach (string name in this.Names)
        {
          JToken jtoken2 = o[name];
          if (jtoken2 != null)
            yield return jtoken2;
          JsonSelectSettings jsonSelectSettings = settings;
          if ((jsonSelectSettings != null ? (jsonSelectSettings.ErrorWhenNoMatch ? 1 : 0) : 0) != 0)
            throw new JsonException("Property '{0}' does not exist on JObject.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) name));
        }
      }
      else
      {
        JsonSelectSettings jsonSelectSettings = settings;
        if ((jsonSelectSettings != null ? (jsonSelectSettings.ErrorWhenNoMatch ? 1 : 0) : 0) != 0)
          throw new JsonException("Properties {0} not valid on {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) string.Join(", ", this.Names.Select<string, string>((Func<string, string>) (n => $"'{n}'"))), (object) jtoken1.GetType().Name));
      }
      o = (JObject) null;
    }
  }
}
