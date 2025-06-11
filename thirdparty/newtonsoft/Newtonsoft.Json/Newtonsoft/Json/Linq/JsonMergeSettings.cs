// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonMergeSettings
// Assembly: Newtonsoft.Json, Version=13.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: F7499511-9334-484F-B6DB-6825BE2EAC0D
// Assembly location: D:\PDFGear\bin\Newtonsoft.Json.dll

using System;

#nullable disable
namespace Newtonsoft.Json.Linq;

public class JsonMergeSettings
{
  private MergeArrayHandling _mergeArrayHandling;
  private MergeNullValueHandling _mergeNullValueHandling;
  private StringComparison _propertyNameComparison;

  public JsonMergeSettings() => this._propertyNameComparison = StringComparison.Ordinal;

  public MergeArrayHandling MergeArrayHandling
  {
    get => this._mergeArrayHandling;
    set
    {
      this._mergeArrayHandling = value >= MergeArrayHandling.Concat && value <= MergeArrayHandling.Merge ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }
  }

  public MergeNullValueHandling MergeNullValueHandling
  {
    get => this._mergeNullValueHandling;
    set
    {
      this._mergeNullValueHandling = value >= MergeNullValueHandling.Ignore && value <= MergeNullValueHandling.Merge ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }
  }

  public StringComparison PropertyNameComparison
  {
    get => this._propertyNameComparison;
    set
    {
      this._propertyNameComparison = value >= StringComparison.CurrentCulture && value <= StringComparison.OrdinalIgnoreCase ? value : throw new ArgumentOutOfRangeException(nameof (value));
    }
  }
}
