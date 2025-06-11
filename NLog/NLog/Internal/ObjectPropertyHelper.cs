// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ObjectPropertyHelper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

#nullable disable
namespace NLog.Internal;

internal class ObjectPropertyHelper
{
  private string[] _objectPropertyPath;
  private ObjectReflectionCache _objectReflectionCache;

  private ObjectReflectionCache ObjectReflectionCache
  {
    get
    {
      return this._objectReflectionCache ?? (this._objectReflectionCache = new ObjectReflectionCache());
    }
  }

  public string ObjectPath
  {
    get
    {
      string[] objectPropertyPath = this._objectPropertyPath;
      return (objectPropertyPath != null ? (objectPropertyPath.Length != 0 ? 1 : 0) : 0) == 0 ? (string) null : string.Join(".", this._objectPropertyPath);
    }
    set
    {
      this._objectPropertyPath = StringHelpers.IsNullOrWhiteSpace(value) ? (string[]) null : value.SplitAndTrimTokens('.');
    }
  }

  public bool TryGetObjectProperty(object value, out object foundValue)
  {
    foundValue = (object) null;
    if (this._objectPropertyPath == null)
      return false;
    ObjectReflectionCache objectReflectionCache = this.ObjectReflectionCache;
    for (int index = 0; index < this._objectPropertyPath.Length; ++index)
    {
      if (value == null)
      {
        foundValue = (object) null;
        return true;
      }
      ObjectReflectionCache.ObjectPropertyList.PropertyValue propertyValue;
      if (objectReflectionCache.LookupObjectProperties(value).TryGetPropertyValue(this._objectPropertyPath[index], out propertyValue))
      {
        value = propertyValue.Value;
      }
      else
      {
        foundValue = (object) null;
        return false;
      }
    }
    foundValue = value;
    return true;
  }
}
