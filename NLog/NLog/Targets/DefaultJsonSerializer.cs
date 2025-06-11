// Decompiled with JetBrains decompiler
// Type: NLog.Targets.DefaultJsonSerializer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.Targets;

public class DefaultJsonSerializer : IJsonConverter, IJsonSerializer
{
  private readonly ObjectReflectionCache _objectReflectionCache = new ObjectReflectionCache();
  private readonly MruCache<Enum, string> _enumCache = new MruCache<Enum, string>(2000);
  private readonly JsonSerializeOptions _serializeOptions = new JsonSerializeOptions();
  private readonly JsonSerializeOptions _exceptionSerializeOptions = new JsonSerializeOptions()
  {
    SanitizeDictionaryKeys = true
  };
  private readonly IFormatProvider _defaultFormatProvider = (IFormatProvider) DefaultJsonSerializer.CreateFormatProvider();
  private const int MaxJsonLength = 524288 /*0x080000*/;
  private static readonly IEqualityComparer<object> _referenceEqualsComparer = (IEqualityComparer<object>) SingleItemOptimizedHashSet<object>.ReferenceEqualityComparer.Default;

  public static DefaultJsonSerializer Instance { get; } = new DefaultJsonSerializer();

  internal DefaultJsonSerializer()
  {
  }

  public string SerializeObject(object value)
  {
    return this.SerializeObject(value, this._serializeOptions);
  }

  public string SerializeObject(object value, JsonSerializeOptions options)
  {
    int num;
    switch (value)
    {
      case null:
        return "null";
      case string text:
        for (int index = 0; index < text.Length; ++index)
        {
          if (DefaultJsonSerializer.RequiresJsonEscape(text[index], options))
          {
            StringBuilder destination = new StringBuilder(text.Length + 4);
            destination.Append('"');
            DefaultJsonSerializer.AppendStringEscape(destination, text, options);
            destination.Append('"');
            return destination.ToString();
          }
        }
        return DefaultJsonSerializer.QuoteValue(text);
      case IConvertible convertible:
        num = (int) convertible.GetTypeCode();
        break;
      default:
        num = 1;
        break;
    }
    TypeCode objTypeCode = (TypeCode) num;
    switch (objTypeCode)
    {
      case TypeCode.Object:
      case TypeCode.Char:
        StringBuilder destination1 = new StringBuilder();
        return !this.SerializeObject(value, destination1, options) ? (string) null : destination1.ToString();
      default:
        if (StringHelpers.IsNullOrWhiteSpace(options.Format) && options.FormatProvider == null)
        {
          if (!options.EnumAsInteger && DefaultJsonSerializer.IsNumericTypeCode(objTypeCode, false) && value is Enum @enum)
            return DefaultJsonSerializer.QuoteValue(this.EnumAsString(@enum));
          string str = XmlHelper.XmlConvertToString(convertible, objTypeCode);
          return DefaultJsonSerializer.SkipQuotes(convertible, objTypeCode) ? str : DefaultJsonSerializer.QuoteValue(str);
        }
        goto case TypeCode.Object;
    }
  }

  public bool SerializeObject(object value, StringBuilder destination)
  {
    return this.SerializeObject(value, destination, this._serializeOptions);
  }

  public bool SerializeObject(
    object value,
    StringBuilder destination,
    JsonSerializeOptions options)
  {
    return this.SerializeObject(value, destination, options, new SingleItemOptimizedHashSet<object>(), 0);
  }

  private bool SerializeObject(
    object value,
    StringBuilder destination,
    JsonSerializeOptions options,
    SingleItemOptimizedHashSet<object> objectsInPath,
    int depth)
  {
    int length = destination.Length;
    try
    {
      return this.SerializeSimpleObjectValue(value, destination, options) || this.SerializeObjectWithReflection(value, destination, options, ref objectsInPath, depth);
    }
    catch
    {
      destination.Length = length;
      return false;
    }
  }

  private bool SerializeObjectWithReflection(
    object value,
    StringBuilder destination,
    JsonSerializeOptions options,
    ref SingleItemOptimizedHashSet<object> objectsInPath,
    int depth)
  {
    if (destination.Length > 524288 /*0x080000*/ || objectsInPath.Contains(value))
      return false;
    switch (value)
    {
      case IDictionary dictionary:
        using (DefaultJsonSerializer.StartCollectionScope(ref objectsInPath, (object) dictionary))
        {
          this.SerializeDictionaryObject(dictionary, destination, options, objectsInPath, depth);
          return true;
        }
      case IEnumerable enumerable:
        ObjectReflectionCache.ObjectPropertyList objectPropertyList;
        if (this._objectReflectionCache.TryLookupExpandoObject(value, out objectPropertyList))
        {
          if (objectPropertyList.ConvertToString || depth >= options.MaxRecursionLimit)
            return this.SerializeObjectAsString(value, destination, options);
          using (new SingleItemOptimizedHashSet<object>.SingleItemScopedInsert(value, ref objectsInPath, false, DefaultJsonSerializer._referenceEqualsComparer))
            return this.SerializeObjectProperties(objectPropertyList, destination, options, objectsInPath, depth);
        }
        using (DefaultJsonSerializer.StartCollectionScope(ref objectsInPath, value))
        {
          this.SerializeCollectionObject(enumerable, destination, options, objectsInPath, depth);
          return true;
        }
      default:
        return this.SerializeObjectWithProperties(value, destination, options, ref objectsInPath, depth);
    }
  }

  private bool SerializeSimpleObjectValue(
    object value,
    StringBuilder destination,
    JsonSerializeOptions options,
    bool forceToString = false)
  {
    IConvertible convertible = value as IConvertible;
    TypeCode objTypeCode = value == null ? TypeCode.Empty : (convertible != null ? convertible.GetTypeCode() : TypeCode.Object);
    if (objTypeCode != TypeCode.Object)
    {
      this.SerializeSimpleTypeCodeValue(convertible, objTypeCode, destination, options, forceToString);
      return true;
    }
    switch (value)
    {
      case DateTimeOffset dateTimeOffset:
        DefaultJsonSerializer.QuoteValue(destination, dateTimeOffset.ToString("yyyy-MM-dd HH:mm:ss zzz", (IFormatProvider) CultureInfo.InvariantCulture));
        return true;
      case IFormattable formattable:
        bool hasFormat = !StringHelpers.IsNullOrWhiteSpace(options.Format);
        this.SerializeWithFormatProvider(formattable, true, destination, options, hasFormat);
        return true;
      default:
        return false;
    }
  }

  private static SingleItemOptimizedHashSet<object>.SingleItemScopedInsert StartCollectionScope(
    ref SingleItemOptimizedHashSet<object> objectsInPath,
    object value)
  {
    return new SingleItemOptimizedHashSet<object>.SingleItemScopedInsert(value, ref objectsInPath, true, DefaultJsonSerializer._referenceEqualsComparer);
  }

  private void SerializeWithFormatProvider(
    IFormattable formattable,
    bool includeQuotes,
    StringBuilder destination,
    JsonSerializeOptions options,
    bool hasFormat)
  {
    if (includeQuotes)
      destination.Append('"');
    IFormatProvider formatProvider = options.FormatProvider ?? (hasFormat ? this._defaultFormatProvider : (IFormatProvider) null);
    string text = formattable.ToString(hasFormat ? options.Format : "", formatProvider);
    if (includeQuotes)
      DefaultJsonSerializer.AppendStringEscape(destination, text, options);
    else
      destination.Append(text);
    if (!includeQuotes)
      return;
    destination.Append('"');
  }

  private void SerializeDictionaryObject(
    IDictionary dictionary,
    StringBuilder destination,
    JsonSerializeOptions options,
    SingleItemOptimizedHashSet<object> objectsInPath,
    int depth)
  {
    bool flag = true;
    int depth1 = objectsInPath.Count <= 1 ? depth : depth + 1;
    if (depth1 > options.MaxRecursionLimit)
    {
      destination.Append("{}");
    }
    else
    {
      destination.Append('{');
      foreach (DictionaryEntry dictionaryEntry in new DictionaryEntryEnumerable(dictionary))
      {
        int length = destination.Length;
        if (length <= 524288 /*0x080000*/)
        {
          if (!flag)
            destination.Append(',');
          object key = dictionaryEntry.Key;
          if (options.QuoteKeys)
          {
            if (!this.SerializeObjectAsString(key, destination, options))
            {
              destination.Length = length;
              continue;
            }
          }
          else if (!this.SerializeObject(key, destination, options, objectsInPath, depth1))
          {
            destination.Length = length;
            continue;
          }
          if (options.SanitizeDictionaryKeys)
          {
            int num1 = options.QuoteKeys ? 1 : 0;
            int num2 = destination.Length - num1;
            int keyStartIndex = length + (flag ? 0 : 1) + num1;
            if (!DefaultJsonSerializer.SanitizeDictionaryKey(destination, keyStartIndex, num2 - keyStartIndex))
            {
              destination.Length = length;
              continue;
            }
          }
          destination.Append(':');
          if (!this.SerializeObject(dictionaryEntry.Value, destination, options, objectsInPath, depth1))
            destination.Length = length;
          else
            flag = false;
        }
        else
          break;
      }
      destination.Append('}');
    }
  }

  private static bool SanitizeDictionaryKey(
    StringBuilder destination,
    int keyStartIndex,
    int keyLength)
  {
    if (keyLength == 0)
      return false;
    int num = keyStartIndex + keyLength;
    for (int index = keyStartIndex; index < num; ++index)
    {
      char c = destination[index];
      if (c != '_' && !char.IsLetterOrDigit(c))
        destination[index] = '_';
    }
    return true;
  }

  private void SerializeCollectionObject(
    IEnumerable value,
    StringBuilder destination,
    JsonSerializeOptions options,
    SingleItemOptimizedHashSet<object> objectsInPath,
    int depth)
  {
    bool flag = true;
    int depth1 = objectsInPath.Count <= 1 ? depth : depth + 1;
    if (depth1 > options.MaxRecursionLimit)
    {
      destination.Append("[]");
    }
    else
    {
      destination.Append('[');
      foreach (object obj in value)
      {
        int length = destination.Length;
        if (length <= 524288 /*0x080000*/)
        {
          if (!flag)
            destination.Append(',');
          if (!this.SerializeObject(obj, destination, options, objectsInPath, depth1))
            destination.Length = length;
          else
            flag = false;
        }
        else
          break;
      }
      destination.Append(']');
    }
  }

  private bool SerializeObjectWithProperties(
    object value,
    StringBuilder destination,
    JsonSerializeOptions options,
    ref SingleItemOptimizedHashSet<object> objectsInPath,
    int depth)
  {
    if (depth < options.MaxRecursionLimit)
    {
      ObjectReflectionCache.ObjectPropertyList objectPropertyList = this._objectReflectionCache.LookupObjectProperties(value);
      if (!objectPropertyList.ConvertToString)
      {
        if (options == DefaultJsonSerializer.Instance._serializeOptions && value is Exception)
          options = DefaultJsonSerializer.Instance._exceptionSerializeOptions;
        using (new SingleItemOptimizedHashSet<object>.SingleItemScopedInsert(value, ref objectsInPath, false, DefaultJsonSerializer._referenceEqualsComparer))
          return this.SerializeObjectProperties(objectPropertyList, destination, options, objectsInPath, depth);
      }
    }
    return this.SerializeObjectAsString(value, destination, options);
  }

  private void SerializeSimpleTypeCodeValue(
    IConvertible value,
    TypeCode objTypeCode,
    StringBuilder destination,
    JsonSerializeOptions options,
    bool forceToString = false)
  {
    if (objTypeCode == TypeCode.Empty || value == null)
      destination.Append(forceToString ? "\"\"" : "null");
    else if (objTypeCode == TypeCode.String || objTypeCode == TypeCode.Char)
    {
      destination.Append('"');
      DefaultJsonSerializer.AppendStringEscape(destination, value.ToString(), options);
      destination.Append('"');
    }
    else
    {
      bool hasFormat = !StringHelpers.IsNullOrWhiteSpace(options.Format);
      if (options.FormatProvider != null | hasFormat && value is IFormattable formattable)
      {
        bool includeQuotes = forceToString || objTypeCode == TypeCode.Object || !DefaultJsonSerializer.SkipQuotes(value, objTypeCode);
        this.SerializeWithFormatProvider(formattable, includeQuotes, destination, options, hasFormat);
      }
      else
        this.SerializeSimpleTypeCodeValueNoEscape(value, objTypeCode, destination, options, forceToString);
    }
  }

  private void SerializeSimpleTypeCodeValueNoEscape(
    IConvertible value,
    TypeCode objTypeCode,
    StringBuilder destination,
    JsonSerializeOptions options,
    bool forceToString)
  {
    if (DefaultJsonSerializer.IsNumericTypeCode(objTypeCode, false))
      this.SerializeSimpleNumericValue(value, objTypeCode, destination, options, forceToString);
    else if (objTypeCode == TypeCode.DateTime)
    {
      destination.Append('"');
      destination.AppendXmlDateTimeRoundTrip(value.ToDateTime((IFormatProvider) CultureInfo.InvariantCulture));
      destination.Append('"');
    }
    else
    {
      string str = XmlHelper.XmlConvertToString(value, objTypeCode);
      if (!forceToString && DefaultJsonSerializer.SkipQuotes(value, objTypeCode) && !string.IsNullOrEmpty(str))
        destination.Append(str);
      else
        DefaultJsonSerializer.QuoteValue(destination, str);
    }
  }

  private void SerializeSimpleNumericValue(
    IConvertible value,
    TypeCode objTypeCode,
    StringBuilder destination,
    JsonSerializeOptions options,
    bool forceToString)
  {
    if (!options.EnumAsInteger && value is Enum @enum)
    {
      DefaultJsonSerializer.QuoteValue(destination, this.EnumAsString(@enum));
    }
    else
    {
      if (forceToString)
        destination.Append('"');
      destination.AppendIntegerAsString(value, objTypeCode);
      if (!forceToString)
        return;
      destination.Append('"');
    }
  }

  private static CultureInfo CreateFormatProvider()
  {
    CultureInfo formatProvider = (CultureInfo) CultureInfo.InvariantCulture.Clone();
    NumberFormatInfo numberFormat = formatProvider.NumberFormat;
    numberFormat.NumberGroupSeparator = string.Empty;
    numberFormat.NumberDecimalSeparator = ".";
    numberFormat.NumberGroupSizes = new int[1];
    numberFormat.NegativeInfinitySymbol = "Infinity";
    numberFormat.PositiveInfinitySymbol = "Infinity";
    return formatProvider;
  }

  private static string QuoteValue(string value) => $"\"{value}\"";

  private static void QuoteValue(StringBuilder destination, string value)
  {
    destination.Append('"');
    destination.Append(value);
    destination.Append('"');
  }

  private string EnumAsString(Enum value)
  {
    string str;
    if (!this._enumCache.TryGetValue(value, out str))
    {
      str = Convert.ToString((object) value, (IFormatProvider) CultureInfo.InvariantCulture);
      this._enumCache.TryAddValue(value, str);
    }
    return str;
  }

  private static bool SkipQuotes(IConvertible value, TypeCode objTypeCode)
  {
    switch (objTypeCode)
    {
      case TypeCode.Empty:
        return true;
      case TypeCode.Boolean:
        return true;
      case TypeCode.Char:
        return false;
      case TypeCode.Single:
        float single = value.ToSingle((IFormatProvider) CultureInfo.InvariantCulture);
        return !float.IsNaN(single) && !float.IsInfinity(single);
      case TypeCode.Double:
        double d = value.ToDouble((IFormatProvider) CultureInfo.InvariantCulture);
        return !double.IsNaN(d) && !double.IsInfinity(d);
      case TypeCode.Decimal:
        return true;
      case TypeCode.DateTime:
        return false;
      case TypeCode.String:
        return false;
      default:
        return DefaultJsonSerializer.IsNumericTypeCode(objTypeCode, false);
    }
  }

  private static bool IsNumericTypeCode(TypeCode objTypeCode, bool includeDecimals)
  {
    switch (objTypeCode)
    {
      case TypeCode.SByte:
      case TypeCode.Byte:
      case TypeCode.Int16:
      case TypeCode.UInt16:
      case TypeCode.Int32:
      case TypeCode.UInt32:
      case TypeCode.Int64:
      case TypeCode.UInt64:
        return true;
      case TypeCode.Single:
      case TypeCode.Double:
      case TypeCode.Decimal:
        return includeDecimals;
      default:
        return false;
    }
  }

  private static void AppendStringEscape(
    StringBuilder destination,
    string text,
    JsonSerializeOptions options)
  {
    DefaultJsonSerializer.AppendStringEscape(destination, text, options.EscapeUnicode, options.EscapeForwardSlash);
  }

  internal static void AppendStringEscape(
    StringBuilder destination,
    string text,
    bool escapeUnicode,
    bool escapeForwardSlash)
  {
    if (string.IsNullOrEmpty(text))
      return;
    StringBuilder stringBuilder = (StringBuilder) null;
    for (int index = 0; index < text.Length; ++index)
    {
      char ch = text[index];
      if (!DefaultJsonSerializer.RequiresJsonEscape(ch, escapeUnicode, escapeForwardSlash))
      {
        stringBuilder?.Append(ch);
      }
      else
      {
        if (stringBuilder == null)
        {
          stringBuilder = destination;
          stringBuilder.Append(text, 0, index);
        }
        switch (ch)
        {
          case '\b':
            stringBuilder.Append("\\b");
            continue;
          case '\t':
            stringBuilder.Append("\\t");
            continue;
          case '\n':
            stringBuilder.Append("\\n");
            continue;
          case '\f':
            stringBuilder.Append("\\f");
            continue;
          case '\r':
            stringBuilder.Append("\\r");
            continue;
          case '"':
            stringBuilder.Append("\\\"");
            continue;
          case '/':
            if (escapeForwardSlash)
            {
              stringBuilder.Append("\\/");
              continue;
            }
            stringBuilder.Append(ch);
            continue;
          case '\\':
            stringBuilder.Append("\\\\");
            continue;
          default:
            if (DefaultJsonSerializer.EscapeChar(ch, escapeUnicode))
            {
              stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "\\u{0:x4}", new object[1]
              {
                (object) (int) ch
              });
              continue;
            }
            stringBuilder.Append(ch);
            continue;
        }
      }
    }
    if (stringBuilder != null)
      return;
    destination.Append(text);
  }

  internal static bool RequiresJsonEscape(char ch, JsonSerializeOptions options)
  {
    return DefaultJsonSerializer.RequiresJsonEscape(ch, options.EscapeUnicode, options.EscapeForwardSlash);
  }

  internal static bool RequiresJsonEscape(char ch, bool escapeUnicode, bool escapeForwardSlash)
  {
    if (DefaultJsonSerializer.EscapeChar(ch, escapeUnicode))
      return true;
    switch (ch)
    {
      case '"':
      case '\\':
        return true;
      case '/':
        return escapeForwardSlash;
      default:
        return false;
    }
  }

  private static bool EscapeChar(char ch, bool escapeUnicode)
  {
    if (ch < ' ')
      return true;
    return escapeUnicode && ch > '\u007F';
  }

  private bool SerializeObjectProperties(
    ObjectReflectionCache.ObjectPropertyList objectPropertyList,
    StringBuilder destination,
    JsonSerializeOptions options,
    SingleItemOptimizedHashSet<object> objectsInPath,
    int depth)
  {
    destination.Append('{');
    bool flag = true;
    foreach (ObjectReflectionCache.ObjectPropertyList.PropertyValue objectProperty in objectPropertyList)
    {
      int length = destination.Length;
      try
      {
        if (DefaultJsonSerializer.HasNameAndValue(objectProperty))
        {
          if (!flag)
            destination.Append(", ");
          if (options.QuoteKeys)
            DefaultJsonSerializer.QuoteValue(destination, objectProperty.Name);
          else
            destination.Append(objectProperty.Name);
          destination.Append(':');
          TypeCode typeCode = objectProperty.TypeCode;
          if (typeCode != TypeCode.Object)
          {
            this.SerializeSimpleTypeCodeValue((IConvertible) objectProperty.Value, typeCode, destination, options);
            flag = false;
          }
          else if (!this.SerializeObject(objectProperty.Value, destination, options, objectsInPath, depth + 1))
            destination.Length = length;
          else
            flag = false;
        }
      }
      catch
      {
        destination.Length = length;
      }
    }
    destination.Append('}');
    return true;
  }

  private static bool HasNameAndValue(
    ObjectReflectionCache.ObjectPropertyList.PropertyValue propertyValue)
  {
    return propertyValue.Name != null && propertyValue.Value != null;
  }

  private bool SerializeObjectAsString(
    object value,
    StringBuilder destination,
    JsonSerializeOptions options)
  {
    int length = destination.Length;
    try
    {
      if (this.SerializeSimpleObjectValue(value, destination, options, true))
        return true;
      bool hasFormat = !StringHelpers.IsNullOrWhiteSpace(options.Format);
      if (options.FormatProvider != null | hasFormat && value is IFormattable formattable)
      {
        this.SerializeWithFormatProvider(formattable, true, destination, options, hasFormat);
        return true;
      }
      string text = Convert.ToString(value, (IFormatProvider) CultureInfo.InvariantCulture);
      destination.Append('"');
      DefaultJsonSerializer.AppendStringEscape(destination, text, options);
      destination.Append('"');
      return true;
    }
    catch
    {
      destination.Length = length;
      return false;
    }
  }
}
