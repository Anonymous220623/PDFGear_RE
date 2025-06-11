// Decompiled with JetBrains decompiler
// Type: NLog.Config.Factory`2
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal;
using System;
using System.Collections.Generic;

#nullable disable
namespace NLog.Config;

internal class Factory<TBaseType, TAttributeType> : INamedItemFactory<TBaseType, Type>, IFactory
  where TBaseType : class
  where TAttributeType : NameBaseAttribute
{
  private readonly Dictionary<string, Factory<TBaseType, TAttributeType>.GetTypeDelegate> _items = new Dictionary<string, Factory<TBaseType, TAttributeType>.GetTypeDelegate>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase);
  private readonly ConfigurationItemFactory _parentFactory;

  internal Factory(ConfigurationItemFactory parentFactory) => this._parentFactory = parentFactory;

  public void ScanTypes(Type[] types, string prefix)
  {
    foreach (Type type in types)
    {
      try
      {
        this.RegisterType(type, prefix);
      }
      catch (Exception ex)
      {
        InternalLogger.Error(ex, "Failed to add type '{0}'.", (object) type.FullName);
        if (ex.MustBeRethrown())
          throw;
      }
    }
  }

  public void RegisterType(Type type, string itemNamePrefix)
  {
    if (!typeof (TBaseType).IsAssignableFrom(type))
      return;
    IEnumerable<TAttributeType> customAttributes = type.GetCustomAttributes<TAttributeType>(false);
    if (customAttributes == null)
      return;
    foreach (TAttributeType attributeType in customAttributes)
      this.RegisterDefinition(itemNamePrefix + attributeType.Name, type);
  }

  public void RegisterNamedType(string itemName, string typeName)
  {
    this._items[itemName] = (Factory<TBaseType, TAttributeType>.GetTypeDelegate) (() => Type.GetType(typeName, false));
  }

  public void Clear() => this._items.Clear();

  public void RegisterDefinition(string itemName, Type itemDefinition)
  {
    this._items[itemName] = (Factory<TBaseType, TAttributeType>.GetTypeDelegate) (() => itemDefinition);
  }

  public bool TryGetDefinition(string itemName, out Type result)
  {
    Factory<TBaseType, TAttributeType>.GetTypeDelegate getTypeDelegate;
    if (!this._items.TryGetValue(itemName, out getTypeDelegate))
    {
      result = (Type) null;
      return false;
    }
    try
    {
      result = getTypeDelegate();
      return result != (Type) null;
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrown())
        throw;
      result = (Type) null;
      return false;
    }
  }

  public virtual bool TryCreateInstance(string itemName, out TBaseType result)
  {
    Type result1;
    if (!this.TryGetDefinition(itemName, out result1))
    {
      result = default (TBaseType);
      return false;
    }
    result = (TBaseType) this._parentFactory.CreateInstance(result1);
    return true;
  }

  public virtual TBaseType CreateInstance(string itemName)
  {
    TBaseType result;
    if (this.TryCreateInstance(itemName, out result))
      return result;
    string message = $"{typeof (TBaseType).Name} cannot be found: '{itemName}'";
    if (itemName != null && (itemName.StartsWith("aspnet", StringComparison.OrdinalIgnoreCase) || itemName.StartsWith("iis", StringComparison.OrdinalIgnoreCase)))
      message += ". Is NLog.Web not included?";
    throw new ArgumentException(message);
  }

  private delegate Type GetTypeDelegate()
    where TBaseType : class
    where TAttributeType : NameBaseAttribute;
}
