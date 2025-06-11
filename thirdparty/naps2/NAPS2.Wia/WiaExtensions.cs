// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.WiaExtensions
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace NAPS2.Wia;

public static class WiaExtensions
{
  public static string Id(this IWiaDeviceProps device) => device.Properties[2].Value.ToString();

  public static string Name(this IWiaDeviceProps device) => device.Properties[7].Value.ToString();

  public static string Name(this WiaItem item) => item.Properties[4098].Value.ToString();

  public static string FullName(this WiaItem item) => item.Properties[4099].Value.ToString();

  public static bool SupportsFlatbed(this WiaDevice device)
  {
    return ((int) device.Properties[3086].Value & 2) != 0;
  }

  public static bool SupportsFeeder(this WiaDevice device)
  {
    return ((int) device.Properties[3086].Value & 1) != 0;
  }

  public static bool SupportsDuplex(this WiaDevice device)
  {
    return ((int) device.Properties[3086].Value & 4) != 0;
  }

  public static bool FeederReady(this WiaDevice device)
  {
    return ((int) device.Properties[3087].Value & 1) != 0;
  }

  public static void SetProperty(this WiaItemBase item, int propId, int value)
  {
    WiaProperty orNull = item.Properties.GetOrNull(propId);
    if (orNull == null)
      return;
    orNull.Value = (object) value;
  }

  public static void SetPropertyClosest(this WiaItemBase item, int propId, ref int value)
  {
    WiaProperty orNull = item.Properties.GetOrNull(propId);
    if (orNull == null)
      return;
    if (orNull.Attributes.Flags.HasFlag((Enum) WiaPropertyFlags.List))
    {
      int value2 = value;
      int? nullable = orNull.Attributes.Values.OfType<int>().OrderBy<int, int>((Func<int, int>) (x => Math.Abs(x - value2))).Cast<int?>().FirstOrDefault<int?>();
      if (!nullable.HasValue)
        return;
      orNull.Value = (object) nullable.Value;
      value = nullable.Value;
    }
    else if (orNull.Attributes.Flags.HasFlag((Enum) WiaPropertyFlags.Range))
    {
      value = Math.Min(Math.Max(value, orNull.Attributes.Min), orNull.Attributes.Max);
      if (orNull.Attributes.Step != 0)
        value -= (value - orNull.Attributes.Min) % orNull.Attributes.Step;
      orNull.Value = (object) value;
    }
    else
      orNull.Value = (object) value;
  }

  public static void SetPropertyRange(
    this WiaItemBase item,
    int propId,
    int value,
    int expectedMin,
    int expectedMax)
  {
    WiaProperty orNull = item.Properties.GetOrNull(propId);
    if (orNull == null)
      return;
    if (orNull.Attributes.Flags.HasFlag((Enum) WiaPropertyFlags.Range))
    {
      int num1 = value - expectedMin;
      int num2 = expectedMax - expectedMin;
      int num3 = orNull.Attributes.Max - orNull.Attributes.Min;
      int val1 = num1 * num3 / num2 + orNull.Attributes.Min;
      if (orNull.Attributes.Step != 0)
        val1 -= (val1 - orNull.Attributes.Min) % orNull.Attributes.Step;
      int num4 = Math.Max(Math.Min(val1, orNull.Attributes.Max), orNull.Attributes.Min);
      orNull.Value = (object) num4;
    }
    else
      orNull.Value = (object) value;
  }

  public static Dictionary<int, object> SerializeEditable(this WiaPropertyCollection props)
  {
    return props.Where<WiaProperty>((Func<WiaProperty, bool>) (x => x.Type == (ushort) 3)).ToDictionary<WiaProperty, int, object>((Func<WiaProperty, int>) (x => x.Id), (Func<WiaProperty, object>) (x => x.Value));
  }

  public static Dictionary<int, object> Delta(
    this WiaPropertyCollection props,
    Dictionary<int, object> target)
  {
    Dictionary<int, object> dictionary1 = props.SerializeEditable();
    Dictionary<int, object> dictionary2 = new Dictionary<int, object>();
    foreach (KeyValuePair<int, object> keyValuePair in target)
    {
      if (dictionary1.ContainsKey(keyValuePair.Key) && !object.Equals(dictionary1[keyValuePair.Key], keyValuePair.Value))
        dictionary2.Add(keyValuePair.Key, keyValuePair.Value);
    }
    return dictionary2;
  }

  public static void DeserializeEditable(
    this WiaPropertyCollection props,
    Dictionary<int, object> values)
  {
    foreach (KeyValuePair<int, object> keyValuePair in values)
    {
      WiaProperty orNull = props.GetOrNull(keyValuePair.Key);
      if (orNull != null)
      {
        try
        {
          orNull.Value = keyValuePair.Value;
        }
        catch (ArgumentException ex)
        {
        }
        catch (WiaException ex)
        {
        }
      }
    }
  }
}
