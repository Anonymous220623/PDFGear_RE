// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.PathFilter
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

#nullable enable
namespace Newtonsoft.Json.Linq.JsonPath;

internal abstract class PathFilter
{
  public abstract IEnumerable<JToken> ExecuteFilter(
    JToken root,
    IEnumerable<JToken> current,
    JsonSelectSettings? settings);

  protected static JToken? GetTokenIndex(JToken t, JsonSelectSettings? settings, int index)
  {
    if (t is JArray jarray)
    {
      if (jarray.Count > index)
        return jarray[index];
      if (settings != null && settings.ErrorWhenNoMatch)
        throw new JsonException("Index {0} outside the bounds of JArray.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) index));
      return (JToken) null;
    }
    if (t is JConstructor jconstructor)
    {
      if (jconstructor.Count > index)
        return jconstructor[(object) index];
      if (settings != null && settings.ErrorWhenNoMatch)
        throw new JsonException("Index {0} outside the bounds of JConstructor.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) index));
      return (JToken) null;
    }
    if (settings != null && settings.ErrorWhenNoMatch)
      throw new JsonException("Index {0} not valid on {1}.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) index, (object) t.GetType().Name));
    return (JToken) null;
  }

  protected static JToken? GetNextScanValue(JToken originalParent, JToken? container, JToken? value)
  {
    if (container != null && container.HasValues)
    {
      value = container.First;
    }
    else
    {
      while (value != null && value != originalParent && value == value.Parent.Last)
        value = (JToken) value.Parent;
      if (value == null || value == originalParent)
        return (JToken) null;
      value = value.Next;
    }
    return value;
  }
}
