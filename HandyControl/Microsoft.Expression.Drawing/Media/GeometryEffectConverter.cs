// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Media.GeometryEffectConverter
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

#nullable disable
namespace HandyControl.Expression.Media;

public sealed class GeometryEffectConverter : TypeConverter
{
  private static readonly Dictionary<string, GeometryEffect> RegisteredEffects = new Dictionary<string, GeometryEffect>()
  {
    {
      "None",
      GeometryEffect.DefaultGeometryEffect
    },
    {
      "Sketch",
      (GeometryEffect) new SketchGeometryEffect()
    }
  };

  public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
  {
    return typeof (string).IsAssignableFrom(sourceType);
  }

  public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
  {
    return typeof (string).IsAssignableFrom(destinationType);
  }

  public override object ConvertFrom(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value)
  {
    GeometryEffect geometryEffect;
    return value is string key && GeometryEffectConverter.RegisteredEffects.TryGetValue(key, out geometryEffect) ? (object) geometryEffect.CloneCurrentValue() : (object) null;
  }

  public override object ConvertTo(
    ITypeDescriptorContext context,
    CultureInfo culture,
    object value,
    Type destinationType)
  {
    if (typeof (string).IsAssignableFrom(destinationType))
    {
      if (value is string)
        return value;
      foreach (KeyValuePair<string, GeometryEffect> registeredEffect in GeometryEffectConverter.RegisteredEffects)
      {
        GeometryEffect geometryEffect = registeredEffect.Value;
        if ((geometryEffect != null ? (geometryEffect.Equals(value as GeometryEffect) ? 1 : 0) : (value == null ? 1 : 0)) != 0)
          return (object) registeredEffect.Key;
      }
    }
    return (object) null;
  }
}
