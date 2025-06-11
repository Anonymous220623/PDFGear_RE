// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ObjectReflectionCache
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

#nullable disable
namespace NLog.Internal;

internal class ObjectReflectionCache : IObjectTypeTransformer
{
  private MruCache<Type, ObjectReflectionCache.ObjectPropertyInfos> _objectTypeCache;
  private IObjectTypeTransformer _objectTypeTransformation;
  private const BindingFlags PublicProperties = BindingFlags.Instance | BindingFlags.Public;

  private MruCache<Type, ObjectReflectionCache.ObjectPropertyInfos> ObjectTypeCache
  {
    get
    {
      return this._objectTypeCache ?? Interlocked.CompareExchange<MruCache<Type, ObjectReflectionCache.ObjectPropertyInfos>>(ref this._objectTypeCache, new MruCache<Type, ObjectReflectionCache.ObjectPropertyInfos>(10000), (MruCache<Type, ObjectReflectionCache.ObjectPropertyInfos>) null) ?? this._objectTypeCache;
    }
  }

  private IObjectTypeTransformer ObjectTypeTransformation
  {
    get
    {
      return this._objectTypeTransformation ?? (this._objectTypeTransformation = ConfigurationItemFactory.Default.ObjectTypeTransformer);
    }
  }

  public static IObjectTypeTransformer Instance { get; } = (IObjectTypeTransformer) new ObjectReflectionCache();

  object IObjectTypeTransformer.TryTransformObject(object obj) => (object) null;

  public ObjectReflectionCache.ObjectPropertyList LookupObjectProperties(object value)
  {
    ObjectReflectionCache.ObjectPropertyList objectPropertyList;
    if (this.TryLookupExpandoObject(value, out objectPropertyList))
      return objectPropertyList;
    if (this.ObjectTypeTransformation != ObjectReflectionCache.Instance)
    {
      object obj = this.ObjectTypeTransformation.TryTransformObject(value);
      if (obj != null)
      {
        if (obj is IConvertible)
          return new ObjectReflectionCache.ObjectPropertyList(obj, ObjectReflectionCache.ObjectPropertyInfos.SimpleToString.Properties, ObjectReflectionCache.ObjectPropertyInfos.SimpleToString.FastLookup);
        if (this.TryLookupExpandoObject(obj, out objectPropertyList))
          return objectPropertyList;
        value = obj;
      }
    }
    Type type = value.GetType();
    ObjectReflectionCache.ObjectPropertyInfos objectPropertyInfos = ObjectReflectionCache.BuildObjectPropertyInfos(value, type);
    this.ObjectTypeCache.TryAddValue(type, objectPropertyInfos);
    return new ObjectReflectionCache.ObjectPropertyList(value, objectPropertyInfos.Properties, objectPropertyInfos.FastLookup);
  }

  public bool TryLookupExpandoObject(
    object value,
    out ObjectReflectionCache.ObjectPropertyList objectPropertyList)
  {
    switch (value)
    {
      case IDictionary<string, object> dictionary:
        objectPropertyList = new ObjectReflectionCache.ObjectPropertyList(dictionary);
        return true;
      case DynamicObject d:
        Dictionary<string, object> dict = ObjectReflectionCache.DynamicObjectToDict(d);
        objectPropertyList = new ObjectReflectionCache.ObjectPropertyList((IDictionary<string, object>) dict);
        return true;
      default:
        Type type = value.GetType();
        ObjectReflectionCache.ObjectPropertyInfos propertyInfos;
        if (this.ObjectTypeCache.TryGetValue(type, out propertyInfos))
        {
          if (!propertyInfos.HasFastLookup)
          {
            ObjectReflectionCache.FastPropertyLookup[] fastLookup = ObjectReflectionCache.BuildFastLookup(propertyInfos.Properties, false);
            propertyInfos = new ObjectReflectionCache.ObjectPropertyInfos(propertyInfos.Properties, fastLookup);
            this.ObjectTypeCache.TryAddValue(type, propertyInfos);
          }
          objectPropertyList = new ObjectReflectionCache.ObjectPropertyList(value, propertyInfos.Properties, propertyInfos.FastLookup);
          return true;
        }
        if (ObjectReflectionCache.TryExtractExpandoObject(type, out propertyInfos))
        {
          this.ObjectTypeCache.TryAddValue(type, propertyInfos);
          objectPropertyList = new ObjectReflectionCache.ObjectPropertyList(value, propertyInfos.Properties, propertyInfos.FastLookup);
          return true;
        }
        objectPropertyList = new ObjectReflectionCache.ObjectPropertyList();
        return false;
    }
  }

  private static bool TryExtractExpandoObject(
    Type objectType,
    out ObjectReflectionCache.ObjectPropertyInfos propertyInfos)
  {
    foreach (Type interfaceType in objectType.GetInterfaces())
    {
      if (ObjectReflectionCache.IsGenericDictionaryEnumeratorType(interfaceType))
      {
        ObjectReflectionCache.IDictionaryEnumerator dictionaryEnumerator = (ObjectReflectionCache.IDictionaryEnumerator) Activator.CreateInstance(typeof (ObjectReflectionCache.DictionaryEnumerator<,>).MakeGenericType(interfaceType.GetGenericArguments()));
        propertyInfos = new ObjectReflectionCache.ObjectPropertyInfos((PropertyInfo[]) null, new ObjectReflectionCache.FastPropertyLookup[1]
        {
          new ObjectReflectionCache.FastPropertyLookup(string.Empty, TypeCode.Object, (ReflectionHelpers.LateBoundMethod) ((o, p) => (object) dictionaryEnumerator.GetEnumerator(o)))
        });
        return true;
      }
    }
    propertyInfos = new ObjectReflectionCache.ObjectPropertyInfos();
    return false;
  }

  private static ObjectReflectionCache.ObjectPropertyInfos BuildObjectPropertyInfos(
    object value,
    Type objectType)
  {
    ObjectReflectionCache.ObjectPropertyInfos objectPropertyInfos;
    if (ObjectReflectionCache.ConvertSimpleToString(objectType))
    {
      objectPropertyInfos = ObjectReflectionCache.ObjectPropertyInfos.SimpleToString;
    }
    else
    {
      PropertyInfo[] publicProperties = ObjectReflectionCache.GetPublicProperties(objectType);
      if (value is Exception)
      {
        ObjectReflectionCache.FastPropertyLookup[] fastLookup = ObjectReflectionCache.BuildFastLookup(publicProperties, true);
        objectPropertyInfos = new ObjectReflectionCache.ObjectPropertyInfos(publicProperties, fastLookup);
      }
      else
        objectPropertyInfos = publicProperties.Length != 0 ? new ObjectReflectionCache.ObjectPropertyInfos(publicProperties, (ObjectReflectionCache.FastPropertyLookup[]) null) : ObjectReflectionCache.ObjectPropertyInfos.SimpleToString;
    }
    return objectPropertyInfos;
  }

  private static bool ConvertSimpleToString(Type objectType)
  {
    return typeof (IFormattable).IsAssignableFrom(objectType) || typeof (Uri).IsAssignableFrom(objectType) || typeof (MemberInfo).IsAssignableFrom(objectType) || typeof (Assembly).IsAssignableFrom(objectType) || typeof (Module).IsAssignableFrom(objectType) || typeof (Stream).IsAssignableFrom(objectType);
  }

  private static PropertyInfo[] GetPublicProperties(Type type)
  {
    PropertyInfo[] source = (PropertyInfo[]) null;
    try
    {
      source = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
    }
    catch (Exception ex)
    {
      object[] objArray = new object[1]{ (object) type };
      InternalLogger.Warn(ex, "Failed to get object properties for type: {0}", objArray);
    }
    if (source != null)
    {
      foreach (PropertyInfo p1 in source)
      {
        if (!p1.IsValidPublicProperty())
        {
          source = ((IEnumerable<PropertyInfo>) source).Where<PropertyInfo>((Func<PropertyInfo, bool>) (p => p.IsValidPublicProperty())).ToArray<PropertyInfo>();
          break;
        }
      }
    }
    return source ?? ArrayHelper.Empty<PropertyInfo>();
  }

  private static ObjectReflectionCache.FastPropertyLookup[] BuildFastLookup(
    PropertyInfo[] properties,
    bool includeType)
  {
    int num = includeType ? 1 : 0;
    ObjectReflectionCache.FastPropertyLookup[] fastPropertyLookupArray = new ObjectReflectionCache.FastPropertyLookup[properties.Length + num];
    if (includeType)
      fastPropertyLookupArray[0] = new ObjectReflectionCache.FastPropertyLookup("Type", TypeCode.String, (ReflectionHelpers.LateBoundMethod) ((o, p) => (object) o.GetType().ToString()));
    foreach (PropertyInfo property in properties)
    {
      MethodInfo getMethod = property.GetGetMethod();
      Type returnType = getMethod.ReturnType;
      ReflectionHelpers.LateBoundMethod lateBoundMethod = ReflectionHelpers.CreateLateBoundMethod(getMethod);
      TypeCode typeCode = Type.GetTypeCode(returnType);
      fastPropertyLookupArray[num++] = new ObjectReflectionCache.FastPropertyLookup(property.Name, typeCode, lateBoundMethod);
    }
    return fastPropertyLookupArray;
  }

  private static Dictionary<string, object> DynamicObjectToDict(DynamicObject d)
  {
    Dictionary<string, object> dict = new Dictionary<string, object>();
    foreach (string dynamicMemberName in d.GetDynamicMemberNames())
    {
      object result;
      if (d.TryGetMember((GetMemberBinder) new ObjectReflectionCache.GetBinderAdapter(dynamicMemberName), out result))
        dict[dynamicMemberName] = result;
    }
    return dict;
  }

  private static bool IsGenericDictionaryEnumeratorType(Type interfaceType)
  {
    return interfaceType.IsGenericType() && (interfaceType.GetGenericTypeDefinition() == typeof (IDictionary<,>) || interfaceType.GetGenericTypeDefinition() == typeof (IReadOnlyDictionary<,>)) && interfaceType.GetGenericArguments()[0] == typeof (string);
  }

  public struct ObjectPropertyList : 
    IEnumerable<ObjectReflectionCache.ObjectPropertyList.PropertyValue>,
    IEnumerable
  {
    internal static readonly StringComparer NameComparer = StringComparer.Ordinal;
    private static readonly ObjectReflectionCache.FastPropertyLookup[] CreateIDictionaryEnumerator = new ObjectReflectionCache.FastPropertyLookup[1]
    {
      new ObjectReflectionCache.FastPropertyLookup(string.Empty, TypeCode.Object, (ReflectionHelpers.LateBoundMethod) ((o, p) => (object) ((IEnumerable<KeyValuePair<string, object>>) o).GetEnumerator()))
    };
    private readonly object _object;
    private readonly PropertyInfo[] _properties;
    private readonly ObjectReflectionCache.FastPropertyLookup[] _fastLookup;

    public bool ConvertToString
    {
      get
      {
        PropertyInfo[] properties = this._properties;
        return properties != null && properties.Length == 0;
      }
    }

    internal ObjectPropertyList(
      object value,
      PropertyInfo[] properties,
      ObjectReflectionCache.FastPropertyLookup[] fastLookup)
    {
      this._object = value;
      this._properties = properties;
      this._fastLookup = fastLookup;
    }

    public ObjectPropertyList(IDictionary<string, object> value)
    {
      this._object = (object) value;
      this._properties = (PropertyInfo[]) null;
      this._fastLookup = ObjectReflectionCache.ObjectPropertyList.CreateIDictionaryEnumerator;
    }

    public bool TryGetPropertyValue(
      string name,
      out ObjectReflectionCache.ObjectPropertyList.PropertyValue propertyValue)
    {
      if (this._properties != null)
        return this._fastLookup != null ? this.TryFastLookupPropertyValue(name, out propertyValue) : this.TrySlowLookupPropertyValue(name, out propertyValue);
      if (!(this._object is IDictionary<string, object> dictionary))
        return this.TryListLookupPropertyValue(name, out propertyValue);
      object obj;
      if (dictionary.TryGetValue(name, out obj))
      {
        propertyValue = new ObjectReflectionCache.ObjectPropertyList.PropertyValue(name, obj, TypeCode.Object);
        return true;
      }
      propertyValue = new ObjectReflectionCache.ObjectPropertyList.PropertyValue();
      return false;
    }

    private bool TryFastLookupPropertyValue(
      string name,
      out ObjectReflectionCache.ObjectPropertyList.PropertyValue propertyValue)
    {
      int hashCode = ObjectReflectionCache.ObjectPropertyList.NameComparer.GetHashCode(name);
      foreach (ObjectReflectionCache.FastPropertyLookup fastProperty in this._fastLookup)
      {
        if (fastProperty.NameHashCode == hashCode && ObjectReflectionCache.ObjectPropertyList.NameComparer.Equals(fastProperty.Name, name))
        {
          propertyValue = new ObjectReflectionCache.ObjectPropertyList.PropertyValue(this._object, fastProperty);
          return true;
        }
      }
      propertyValue = new ObjectReflectionCache.ObjectPropertyList.PropertyValue();
      return false;
    }

    private bool TrySlowLookupPropertyValue(
      string name,
      out ObjectReflectionCache.ObjectPropertyList.PropertyValue propertyValue)
    {
      foreach (PropertyInfo property in this._properties)
      {
        if (ObjectReflectionCache.ObjectPropertyList.NameComparer.Equals(property.Name, name))
        {
          propertyValue = new ObjectReflectionCache.ObjectPropertyList.PropertyValue(this._object, property);
          return true;
        }
      }
      propertyValue = new ObjectReflectionCache.ObjectPropertyList.PropertyValue();
      return false;
    }

    private bool TryListLookupPropertyValue(
      string name,
      out ObjectReflectionCache.ObjectPropertyList.PropertyValue propertyValue)
    {
      foreach (ObjectReflectionCache.ObjectPropertyList.PropertyValue propertyValue1 in this)
      {
        if (ObjectReflectionCache.ObjectPropertyList.NameComparer.Equals(propertyValue1.Name, name))
        {
          propertyValue = propertyValue1;
          return true;
        }
      }
      propertyValue = new ObjectReflectionCache.ObjectPropertyList.PropertyValue();
      return false;
    }

    public override string ToString() => this._object?.ToString() ?? "null";

    public ObjectReflectionCache.ObjectPropertyList.Enumerator GetEnumerator()
    {
      return this._properties != null ? new ObjectReflectionCache.ObjectPropertyList.Enumerator(this._object, this._properties, this._fastLookup) : new ObjectReflectionCache.ObjectPropertyList.Enumerator((IEnumerator<KeyValuePair<string, object>>) this._fastLookup[0].ValueLookup(this._object, (object[]) null));
    }

    IEnumerator<ObjectReflectionCache.ObjectPropertyList.PropertyValue> IEnumerable<ObjectReflectionCache.ObjectPropertyList.PropertyValue>.GetEnumerator()
    {
      return (IEnumerator<ObjectReflectionCache.ObjectPropertyList.PropertyValue>) this.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public struct PropertyValue
    {
      public readonly string Name;
      public readonly object Value;
      private readonly TypeCode _typecode;

      public TypeCode TypeCode => this.Value != null ? this._typecode : TypeCode.Empty;

      public PropertyValue(string name, object value, TypeCode typeCode)
      {
        this.Name = name;
        this.Value = value;
        this._typecode = typeCode;
      }

      public PropertyValue(object owner, PropertyInfo propertyInfo)
      {
        this.Name = propertyInfo.Name;
        this.Value = propertyInfo.GetValue(owner, (object[]) null);
        this._typecode = TypeCode.Object;
      }

      public PropertyValue(
        object owner,
        ObjectReflectionCache.FastPropertyLookup fastProperty)
      {
        this.Name = fastProperty.Name;
        this.Value = fastProperty.ValueLookup(owner, (object[]) null);
        this._typecode = fastProperty.TypeCode;
      }
    }

    public struct Enumerator : 
      IEnumerator<ObjectReflectionCache.ObjectPropertyList.PropertyValue>,
      IDisposable,
      IEnumerator
    {
      private readonly object _owner;
      private readonly PropertyInfo[] _properties;
      private readonly ObjectReflectionCache.FastPropertyLookup[] _fastLookup;
      private readonly IEnumerator<KeyValuePair<string, object>> _enumerator;
      private int _index;

      internal Enumerator(
        object owner,
        PropertyInfo[] properties,
        ObjectReflectionCache.FastPropertyLookup[] fastLookup)
      {
        this._owner = owner;
        this._properties = properties;
        this._fastLookup = fastLookup;
        this._index = -1;
        this._enumerator = (IEnumerator<KeyValuePair<string, object>>) null;
      }

      internal Enumerator(
        IEnumerator<KeyValuePair<string, object>> enumerator)
      {
        this._owner = (object) enumerator;
        this._properties = (PropertyInfo[]) null;
        this._fastLookup = (ObjectReflectionCache.FastPropertyLookup[]) null;
        this._index = 0;
        this._enumerator = enumerator;
      }

      public ObjectReflectionCache.ObjectPropertyList.PropertyValue Current
      {
        get
        {
          try
          {
            if (this._fastLookup != null)
              return new ObjectReflectionCache.ObjectPropertyList.PropertyValue(this._owner, this._fastLookup[this._index]);
            if (this._properties != null)
              return new ObjectReflectionCache.ObjectPropertyList.PropertyValue(this._owner, this._properties[this._index]);
            KeyValuePair<string, object> current = this._enumerator.Current;
            string key = current.Key;
            current = this._enumerator.Current;
            object obj = current.Value;
            return new ObjectReflectionCache.ObjectPropertyList.PropertyValue(key, obj, TypeCode.Object);
          }
          catch (Exception ex)
          {
            object[] objArray = new object[1]{ this._owner };
            InternalLogger.Debug(ex, "Failed to get property value for object: {0}", objArray);
            return new ObjectReflectionCache.ObjectPropertyList.PropertyValue();
          }
        }
      }

      object IEnumerator.Current => (object) this.Current;

      public void Dispose() => this._enumerator?.Dispose();

      public bool MoveNext()
      {
        if (this._properties == null)
          return this._enumerator.MoveNext();
        int num1 = ++this._index;
        ObjectReflectionCache.FastPropertyLookup[] fastLookup = this._fastLookup;
        int num2 = fastLookup != null ? fastLookup.Length : this._properties.Length;
        return num1 < num2;
      }

      public void Reset()
      {
        if (this._properties != null)
          this._index = -1;
        else
          this._enumerator.Reset();
      }
    }
  }

  internal struct FastPropertyLookup
  {
    public readonly string Name;
    public readonly ReflectionHelpers.LateBoundMethod ValueLookup;
    public readonly TypeCode TypeCode;
    public readonly int NameHashCode;

    public FastPropertyLookup(
      string name,
      TypeCode typeCode,
      ReflectionHelpers.LateBoundMethod valueLookup)
    {
      this.Name = name;
      this.ValueLookup = valueLookup;
      this.TypeCode = typeCode;
      this.NameHashCode = ObjectReflectionCache.ObjectPropertyList.NameComparer.GetHashCode(name);
    }
  }

  private struct ObjectPropertyInfos(
    PropertyInfo[] properties,
    ObjectReflectionCache.FastPropertyLookup[] fastLookup) : 
    IEquatable<ObjectReflectionCache.ObjectPropertyInfos>
  {
    public readonly PropertyInfo[] Properties = properties;
    public readonly ObjectReflectionCache.FastPropertyLookup[] FastLookup = fastLookup;
    public static readonly ObjectReflectionCache.ObjectPropertyInfos SimpleToString = new ObjectReflectionCache.ObjectPropertyInfos(ArrayHelper.Empty<PropertyInfo>(), ArrayHelper.Empty<ObjectReflectionCache.FastPropertyLookup>());

    public bool HasFastLookup => this.FastLookup != null;

    public bool Equals(ObjectReflectionCache.ObjectPropertyInfos other)
    {
      if (this.Properties != other.Properties)
        return false;
      int? length1 = this.FastLookup?.Length;
      int? length2 = other.FastLookup?.Length;
      return length1.GetValueOrDefault() == length2.GetValueOrDefault() & length1.HasValue == length2.HasValue;
    }
  }

  private sealed class GetBinderAdapter : GetMemberBinder
  {
    internal GetBinderAdapter(string name)
      : base(name, false)
    {
    }

    public override DynamicMetaObject FallbackGetMember(
      DynamicMetaObject target,
      DynamicMetaObject errorSuggestion)
    {
      return target;
    }
  }

  private interface IDictionaryEnumerator
  {
    IEnumerator<KeyValuePair<string, object>> GetEnumerator(object value);
  }

  internal sealed class DictionaryEnumerator<TKey, TValue> : 
    ObjectReflectionCache.IDictionaryEnumerator
  {
    public IEnumerator<KeyValuePair<string, object>> GetEnumerator(object value)
    {
      switch (value)
      {
        case IDictionary<TKey, TValue> dictionary1:
          if (dictionary1.Count > 0)
            return this.YieldEnumerator(dictionary1);
          break;
        case IReadOnlyDictionary<TKey, TValue> dictionary2:
          if (dictionary2.Count > 0)
            return this.YieldEnumerator(dictionary2);
          break;
      }
      return (IEnumerator<KeyValuePair<string, object>>) ObjectReflectionCache.DictionaryEnumerator<TKey, TValue>.EmptyDictionaryEnumerator.Default;
    }

    private IEnumerator<KeyValuePair<string, object>> YieldEnumerator(
      IDictionary<TKey, TValue> dictionary)
    {
      foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>) dictionary)
        yield return new KeyValuePair<string, object>(keyValuePair.Key.ToString(), (object) keyValuePair.Value);
    }

    private IEnumerator<KeyValuePair<string, object>> YieldEnumerator(
      IReadOnlyDictionary<TKey, TValue> dictionary)
    {
      foreach (KeyValuePair<TKey, TValue> keyValuePair in (IEnumerable<KeyValuePair<TKey, TValue>>) dictionary)
        yield return new KeyValuePair<string, object>(keyValuePair.Key.ToString(), (object) keyValuePair.Value);
    }

    private sealed class EmptyDictionaryEnumerator : 
      IEnumerator<KeyValuePair<string, object>>,
      IDisposable,
      IEnumerator
    {
      public static readonly ObjectReflectionCache.DictionaryEnumerator<TKey, TValue>.EmptyDictionaryEnumerator Default = new ObjectReflectionCache.DictionaryEnumerator<TKey, TValue>.EmptyDictionaryEnumerator();

      KeyValuePair<string, object> IEnumerator<KeyValuePair<string, object>>.Current
      {
        get => new KeyValuePair<string, object>();
      }

      object IEnumerator.Current => (object) new KeyValuePair<string, object>();

      bool IEnumerator.MoveNext() => false;

      void IDisposable.Dispose()
      {
      }

      void IEnumerator.Reset()
      {
      }
    }
  }
}
