// Decompiled with JetBrains decompiler
// Type: NLog.SetupSerializationBuilderExtensions
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.MessageTemplates;
using NLog.Targets;
using System;

#nullable disable
namespace NLog;

public static class SetupSerializationBuilderExtensions
{
  public static ISetupSerializationBuilder RegisterJsonConverter(
    this ISetupSerializationBuilder setupBuilder,
    IJsonConverter jsonConverter)
  {
    ConfigurationItemFactory.Default.JsonConverter = jsonConverter ?? (IJsonConverter) DefaultJsonSerializer.Instance;
    return setupBuilder;
  }

  public static ISetupSerializationBuilder RegisterValueFormatter(
    this ISetupSerializationBuilder setupBuilder,
    IValueFormatter valueFormatter)
  {
    ConfigurationItemFactory.Default.ValueFormatter = valueFormatter ?? ValueFormatter.Instance;
    return setupBuilder;
  }

  public static ISetupSerializationBuilder RegisterObjectTransformation<T>(
    this ISetupSerializationBuilder setupBuilder,
    Func<T, object> transformer)
  {
    if (transformer == null)
      throw new ArgumentNullException(nameof (transformer));
    IObjectTypeTransformer objectTypeTransformer = ConfigurationItemFactory.Default.ObjectTypeTransformer;
    ConfigurationItemFactory.Default.ObjectTypeTransformer = (IObjectTypeTransformer) new SetupSerializationBuilderExtensions.ObjectTypeTransformation<T>(transformer, objectTypeTransformer);
    return setupBuilder;
  }

  public static ISetupSerializationBuilder RegisterObjectTransformation(
    this ISetupSerializationBuilder setupBuilder,
    Type objectType,
    Func<object, object> transformer)
  {
    if (objectType == (Type) null)
      throw new ArgumentNullException(nameof (objectType));
    if (transformer == null)
      throw new ArgumentNullException(nameof (transformer));
    IObjectTypeTransformer objectTypeTransformer = ConfigurationItemFactory.Default.ObjectTypeTransformer;
    ConfigurationItemFactory.Default.ObjectTypeTransformer = (IObjectTypeTransformer) new SetupSerializationBuilderExtensions.ObjectTypeTransformation(objectType, transformer, objectTypeTransformer);
    return setupBuilder;
  }

  private class ObjectTypeTransformation<T> : IObjectTypeTransformer
  {
    private readonly IObjectTypeTransformer _original;
    private readonly Func<T, object> _transformer;

    public ObjectTypeTransformation(Func<T, object> transformer, IObjectTypeTransformer original)
    {
      this._original = original;
      this._transformer = transformer;
    }

    public object TryTransformObject(object obj)
    {
      if (obj is T obj2)
      {
        object obj1 = this._transformer(obj2);
        if (obj1 != null)
          return obj1;
      }
      return this._original?.TryTransformObject(obj);
    }
  }

  private class ObjectTypeTransformation : IObjectTypeTransformer
  {
    private readonly IObjectTypeTransformer _original;
    private readonly Func<object, object> _transformer;
    private readonly Type _objectType;

    public ObjectTypeTransformation(
      Type objecType,
      Func<object, object> transformer,
      IObjectTypeTransformer original)
    {
      this._original = original;
      this._transformer = transformer;
      this._objectType = objecType;
    }

    public object TryTransformObject(object obj)
    {
      if (this._objectType.IsAssignableFrom(obj.GetType()))
        return this._transformer(obj);
      return this._original?.TryTransformObject(obj);
    }
  }
}
