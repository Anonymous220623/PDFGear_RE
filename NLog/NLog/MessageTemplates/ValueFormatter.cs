// Decompiled with JetBrains decompiler
// Type: NLog.MessageTemplates.ValueFormatter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.MessageTemplates;

internal class ValueFormatter : IValueFormatter
{
  private static IValueFormatter _instance;
  private static readonly IEqualityComparer<object> _referenceEqualsComparer = (IEqualityComparer<object>) SingleItemOptimizedHashSet<object>.ReferenceEqualityComparer.Default;
  private const int MaxRecursionDepth = 2;
  private const int MaxValueLength = 524288 /*0x080000*/;
  private const string LiteralFormatSymbol = "l";
  private readonly MruCache<Enum, string> _enumCache = new MruCache<Enum, string>(2000);
  public const string FormatAsJson = "@";
  public const string FormatAsString = "$";

  public static IValueFormatter Instance
  {
    get
    {
      return ValueFormatter._instance ?? (ValueFormatter._instance = (IValueFormatter) new ValueFormatter());
    }
    set => ValueFormatter._instance = value ?? (IValueFormatter) new ValueFormatter();
  }

  private ValueFormatter()
  {
  }

  public bool FormatValue(
    object value,
    string format,
    CaptureType captureType,
    IFormatProvider formatProvider,
    StringBuilder builder)
  {
    if (captureType == CaptureType.Serialize)
      return ConfigurationItemFactory.Default.JsonConverter.SerializeObject(value, builder);
    if (captureType != CaptureType.Stringify)
      return this.FormatObject(value, format, formatProvider, builder);
    builder.Append('"');
    ValueFormatter.FormatToString(value, (string) null, formatProvider, builder);
    builder.Append('"');
    return true;
  }

  public bool FormatObject(
    object value,
    string format,
    IFormatProvider formatProvider,
    StringBuilder builder)
  {
    if (this.SerializeSimpleObject(value, format, formatProvider, builder, false))
      return true;
    if (value is IEnumerable collection)
    {
      string format1 = format;
      IFormatProvider formatProvider1 = formatProvider;
      StringBuilder builder1 = builder;
      SingleItemOptimizedHashSet<object> objectsInPath = new SingleItemOptimizedHashSet<object>();
      return this.SerializeWithoutCyclicLoop(collection, format1, formatProvider1, builder1, objectsInPath, 0);
    }
    ValueFormatter.SerializeConvertToString(value, formatProvider, builder);
    return true;
  }

  private bool SerializeSimpleObject(
    object value,
    string format,
    IFormatProvider formatProvider,
    StringBuilder builder,
    bool convertToString = true)
  {
    if (value is string stringValue)
    {
      ValueFormatter.SerializeStringObject(stringValue, format, builder);
      return true;
    }
    if (value == null)
    {
      builder.Append("NULL");
      return true;
    }
    if (value is IConvertible convertible)
    {
      this.SerializeConvertibleObject(convertible, format, formatProvider, builder);
      return true;
    }
    if (!string.IsNullOrEmpty(format) && value is IFormattable formattable)
    {
      builder.Append(formattable.ToString(format, formatProvider));
      return true;
    }
    if (!convertToString)
      return false;
    ValueFormatter.SerializeConvertToString(value, formatProvider, builder);
    return true;
  }

  private void SerializeConvertibleObject(
    IConvertible value,
    string format,
    IFormatProvider formatProvider,
    StringBuilder builder)
  {
    TypeCode typeCode = value.GetTypeCode();
    if (typeCode == TypeCode.String)
      ValueFormatter.SerializeStringObject(value.ToString(), format, builder);
    else if (!string.IsNullOrEmpty(format) && value is IFormattable formattable)
    {
      builder.Append(formattable.ToString(format, formatProvider));
    }
    else
    {
      switch (typeCode)
      {
        case TypeCode.Boolean:
          builder.Append(value.ToBoolean((IFormatProvider) CultureInfo.InvariantCulture) ? "true" : "false");
          break;
        case TypeCode.Char:
          int num = format != "l" ? 1 : 0;
          if (num != 0)
            builder.Append('"');
          builder.Append(value.ToChar((IFormatProvider) CultureInfo.InvariantCulture));
          if (num == 0)
            break;
          builder.Append('"');
          break;
        case TypeCode.SByte:
        case TypeCode.Byte:
        case TypeCode.Int16:
        case TypeCode.UInt16:
        case TypeCode.Int32:
        case TypeCode.UInt32:
        case TypeCode.Int64:
        case TypeCode.UInt64:
          if (value is Enum @enum)
          {
            this.AppendEnumAsString(builder, @enum);
            break;
          }
          builder.AppendIntegerAsString(value, typeCode);
          break;
        default:
          ValueFormatter.SerializeConvertToString((object) value, formatProvider, builder);
          break;
      }
    }
  }

  private static void SerializeConvertToString(
    object value,
    IFormatProvider formatProvider,
    StringBuilder builder)
  {
    builder.Append(Convert.ToString(value, formatProvider));
  }

  private static void SerializeStringObject(
    string stringValue,
    string format,
    StringBuilder builder)
  {
    int num = format != "l" ? 1 : 0;
    if (num != 0)
      builder.Append('"');
    builder.Append(stringValue);
    if (num == 0)
      return;
    builder.Append('"');
  }

  private void AppendEnumAsString(StringBuilder sb, Enum value)
  {
    string str;
    if (!this._enumCache.TryGetValue(value, out str))
    {
      str = value.ToString();
      this._enumCache.TryAddValue(value, str);
    }
    sb.Append(str);
  }

  private bool SerializeWithoutCyclicLoop(
    IEnumerable collection,
    string format,
    IFormatProvider formatProvider,
    StringBuilder builder,
    SingleItemOptimizedHashSet<object> objectsInPath,
    int depth)
  {
    if (objectsInPath.Contains((object) collection) || depth > 2)
      return false;
    if (collection is IDictionary dictionary)
    {
      using (new SingleItemOptimizedHashSet<object>.SingleItemScopedInsert((object) dictionary, ref objectsInPath, true, ValueFormatter._referenceEqualsComparer))
        return this.SerializeDictionaryObject(dictionary, format, formatProvider, builder, objectsInPath, depth);
    }
    using (new SingleItemOptimizedHashSet<object>.SingleItemScopedInsert((object) collection, ref objectsInPath, true, ValueFormatter._referenceEqualsComparer))
      return this.SerializeCollectionObject(collection, format, formatProvider, builder, objectsInPath, depth);
  }

  private bool SerializeDictionaryObject(
    IDictionary dictionary,
    string format,
    IFormatProvider formatProvider,
    StringBuilder builder,
    SingleItemOptimizedHashSet<object> objectsInPath,
    int depth)
  {
    bool flag = false;
    foreach (DictionaryEntry dictionaryEntry in new DictionaryEntryEnumerable(dictionary))
    {
      if (builder.Length > 524288 /*0x080000*/)
        return false;
      if (flag)
        builder.Append(", ");
      this.SerializeCollectionItem(dictionaryEntry.Key, format, formatProvider, builder, ref objectsInPath, depth);
      builder.Append("=");
      this.SerializeCollectionItem(dictionaryEntry.Value, format, formatProvider, builder, ref objectsInPath, depth);
      flag = true;
    }
    return true;
  }

  private bool SerializeCollectionObject(
    IEnumerable collection,
    string format,
    IFormatProvider formatProvider,
    StringBuilder builder,
    SingleItemOptimizedHashSet<object> objectsInPath,
    int depth)
  {
    bool flag = false;
    foreach (object obj in collection)
    {
      if (builder.Length > 524288 /*0x080000*/)
        return false;
      if (flag)
        builder.Append(", ");
      this.SerializeCollectionItem(obj, format, formatProvider, builder, ref objectsInPath, depth);
      flag = true;
    }
    return true;
  }

  private void SerializeCollectionItem(
    object item,
    string format,
    IFormatProvider formatProvider,
    StringBuilder builder,
    ref SingleItemOptimizedHashSet<object> objectsInPath,
    int depth)
  {
    switch (item)
    {
      case IConvertible convertible:
        this.SerializeConvertibleObject(convertible, format, formatProvider, builder);
        break;
      case IEnumerable collection:
        this.SerializeWithoutCyclicLoop(collection, format, formatProvider, builder, objectsInPath, depth + 1);
        break;
      default:
        this.SerializeSimpleObject(item, format, formatProvider, builder);
        break;
    }
  }

  public static void FormatToString(
    object value,
    string format,
    IFormatProvider formatProvider,
    StringBuilder builder)
  {
    switch (value)
    {
      case string str:
        builder.Append(str);
        break;
      case IFormattable formattable:
        builder.Append(formattable.ToString(format, formatProvider));
        break;
      default:
        builder.Append(Convert.ToString(value, formatProvider));
        break;
    }
  }
}
