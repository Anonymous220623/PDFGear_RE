// Decompiled with JetBrains decompiler
// Type: NLog.Config.MethodFactory
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace NLog.Config;

internal class MethodFactory : 
  INamedItemFactory<MethodInfo, MethodInfo>,
  INamedItemFactory<ReflectionHelpers.LateBoundMethod, MethodInfo>,
  IFactory
{
  private readonly Dictionary<string, MethodInfo> _nameToMethodInfo = new Dictionary<string, MethodInfo>();
  private readonly Dictionary<string, ReflectionHelpers.LateBoundMethod> _nameToLateBoundMethod = new Dictionary<string, ReflectionHelpers.LateBoundMethod>();
  private readonly Func<Type, IList<KeyValuePair<string, MethodInfo>>> _methodExtractor;

  public MethodFactory(
    Func<Type, IList<KeyValuePair<string, MethodInfo>>> methodExtractor)
  {
    this._methodExtractor = methodExtractor;
  }

  public void ScanTypes(Type[] types, string prefix)
  {
    foreach (Type type in types)
    {
      try
      {
        if (!type.IsClass())
        {
          if (!type.IsAbstract())
            continue;
        }
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
    IList<KeyValuePair<string, MethodInfo>> keyValuePairList = this._methodExtractor(type);
    if (keyValuePairList == null || keyValuePairList.Count <= 0)
      return;
    for (int index = 0; index < keyValuePairList.Count; ++index)
    {
      string str = itemNamePrefix;
      KeyValuePair<string, MethodInfo> keyValuePair = keyValuePairList[index];
      string key = keyValuePair.Key;
      string itemName = str + key;
      keyValuePair = keyValuePairList[index];
      MethodInfo itemDefinition = keyValuePair.Value;
      this.RegisterDefinition(itemName, itemDefinition);
    }
  }

  public static IList<KeyValuePair<string, MethodInfo>> ExtractClassMethods<TClassAttributeType, TMethodAttributeType>(
    Type type)
    where TClassAttributeType : Attribute
    where TMethodAttributeType : NameBaseAttribute
  {
    if (!type.IsDefined(typeof (TClassAttributeType), false))
      return (IList<KeyValuePair<string, MethodInfo>>) ArrayHelper.Empty<KeyValuePair<string, MethodInfo>>();
    List<KeyValuePair<string, MethodInfo>> classMethods = new List<KeyValuePair<string, MethodInfo>>();
    foreach (MethodInfo method in type.GetMethods())
    {
      foreach (TMethodAttributeType customAttribute in (TMethodAttributeType[]) method.GetCustomAttributes(typeof (TMethodAttributeType), false))
        classMethods.Add(new KeyValuePair<string, MethodInfo>(customAttribute.Name, method));
    }
    return (IList<KeyValuePair<string, MethodInfo>>) classMethods;
  }

  public void Clear()
  {
    this._nameToMethodInfo.Clear();
    lock (this._nameToLateBoundMethod)
      this._nameToLateBoundMethod.Clear();
  }

  public void RegisterDefinition(string itemName, MethodInfo itemDefinition)
  {
    this._nameToMethodInfo[itemName] = itemDefinition;
    lock (this._nameToLateBoundMethod)
      this._nameToLateBoundMethod.Remove(itemName);
  }

  internal void RegisterDefinition(
    string itemName,
    MethodInfo itemDefinition,
    ReflectionHelpers.LateBoundMethod lateBoundMethod)
  {
    this._nameToMethodInfo[itemName] = itemDefinition;
    lock (this._nameToLateBoundMethod)
      this._nameToLateBoundMethod[itemName] = lateBoundMethod;
  }

  public bool TryCreateInstance(string itemName, out MethodInfo result)
  {
    return this._nameToMethodInfo.TryGetValue(itemName, out result);
  }

  public bool TryCreateInstance(string itemName, out ReflectionHelpers.LateBoundMethod result)
  {
    lock (this._nameToLateBoundMethod)
    {
      if (this._nameToLateBoundMethod.TryGetValue(itemName, out result))
        return true;
    }
    MethodInfo methodInfo;
    if (!this._nameToMethodInfo.TryGetValue(itemName, out methodInfo))
      return false;
    result = ReflectionHelpers.CreateLateBoundMethod(methodInfo);
    lock (this._nameToLateBoundMethod)
      this._nameToLateBoundMethod[itemName] = result;
    return true;
  }

  MethodInfo INamedItemFactory<MethodInfo, MethodInfo>.CreateInstance(string itemName)
  {
    MethodInfo result;
    if (this.TryCreateInstance(itemName, out result))
      return result;
    throw new NLogConfigurationException($"Unknown function: '{itemName}'");
  }

  public ReflectionHelpers.LateBoundMethod CreateInstance(string itemName)
  {
    ReflectionHelpers.LateBoundMethod result;
    if (this.TryCreateInstance(itemName, out result))
      return result;
    throw new NLogConfigurationException($"Unknown function: '{itemName}'");
  }

  public bool TryGetDefinition(string itemName, out MethodInfo result)
  {
    return this._nameToMethodInfo.TryGetValue(itemName, out result);
  }
}
