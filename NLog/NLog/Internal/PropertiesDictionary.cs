// Decompiled with JetBrains decompiler
// Type: NLog.Internal.PropertiesDictionary
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.MessageTemplates;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

#nullable disable
namespace NLog.Internal;

[DebuggerDisplay("Count = {Count}")]
internal sealed class PropertiesDictionary : 
  IDictionary<object, object>,
  ICollection<KeyValuePair<object, object>>,
  IEnumerable<KeyValuePair<object, object>>,
  IEnumerable,
  IEnumerable<MessageTemplateParameter>
{
  private Dictionary<object, PropertiesDictionary.PropertyValue> _eventProperties;
  private IList<MessageTemplateParameter> _messageProperties;
  private PropertiesDictionary.DictionaryCollection _keyCollection;
  private PropertiesDictionary.DictionaryCollection _valueCollection;
  private IDictionary _eventContextAdapter;
  private static readonly PropertiesDictionary.DictionaryCollection EmptyKeyCollection = new PropertiesDictionary.DictionaryCollection(new PropertiesDictionary(), true);
  private static readonly PropertiesDictionary.DictionaryCollection EmptyValueCollection = new PropertiesDictionary.DictionaryCollection(new PropertiesDictionary(), false);

  public PropertiesDictionary(IList<MessageTemplateParameter> messageParameters = null)
  {
    if (messageParameters == null || messageParameters.Count <= 0)
      return;
    this.MessageProperties = messageParameters;
  }

  public PropertiesDictionary(
    IReadOnlyList<KeyValuePair<object, object>> eventProperties)
  {
    int count = eventProperties.Count;
    if (count <= 0)
      return;
    this._eventProperties = new Dictionary<object, PropertiesDictionary.PropertyValue>(count);
    for (int index = 0; index < count; ++index)
    {
      KeyValuePair<object, object> eventProperty = eventProperties[index];
      this._eventProperties[eventProperty.Key] = new PropertiesDictionary.PropertyValue(eventProperty.Value, false);
    }
  }

  private bool IsEmpty
  {
    get
    {
      if (this._eventProperties != null && this._eventProperties.Count != 0)
        return false;
      return this._messageProperties == null || this._messageProperties.Count == 0;
    }
  }

  public IDictionary EventContext
  {
    get
    {
      return this._eventContextAdapter ?? (this._eventContextAdapter = (IDictionary) new DictionaryAdapter<object, object>((IDictionary<object, object>) this));
    }
  }

  private Dictionary<object, PropertiesDictionary.PropertyValue> EventProperties
  {
    get
    {
      if (this._eventProperties == null)
        Interlocked.CompareExchange<Dictionary<object, PropertiesDictionary.PropertyValue>>(ref this._eventProperties, PropertiesDictionary.BuildEventProperties(this._messageProperties), (Dictionary<object, PropertiesDictionary.PropertyValue>) null);
      return this._eventProperties;
    }
  }

  public IList<MessageTemplateParameter> MessageProperties
  {
    get
    {
      return this._messageProperties ?? (IList<MessageTemplateParameter>) ArrayHelper.Empty<MessageTemplateParameter>();
    }
    internal set
    {
      this._messageProperties = this.SetMessageProperties(value, this._messageProperties);
    }
  }

  private IList<MessageTemplateParameter> SetMessageProperties(
    IList<MessageTemplateParameter> newMessageProperties,
    IList<MessageTemplateParameter> oldMessageProperties)
  {
    if (this._eventProperties == null && PropertiesDictionary.VerifyUniqueMessageTemplateParametersFast(newMessageProperties))
      return newMessageProperties;
    if (this._eventProperties == null)
      this._eventProperties = new Dictionary<object, PropertiesDictionary.PropertyValue>(newMessageProperties.Count);
    if (oldMessageProperties != null && this._eventProperties.Count > 0)
      this.RemoveOldMessageProperties(oldMessageProperties);
    return newMessageProperties != null && (this._eventProperties.Count > 0 || !PropertiesDictionary.InsertMessagePropertiesIntoEmptyDictionary(newMessageProperties, this._eventProperties)) ? PropertiesDictionary.CreateUniqueMessagePropertiesListSlow(newMessageProperties, this._eventProperties) : newMessageProperties;
  }

  private void RemoveOldMessageProperties(
    IList<MessageTemplateParameter> oldMessageProperties)
  {
    for (int index = 0; index < oldMessageProperties.Count; ++index)
    {
      PropertiesDictionary.PropertyValue propertyValue;
      if (this._eventProperties.TryGetValue((object) oldMessageProperties[index].Name, out propertyValue) && propertyValue.IsMessageProperty)
        this._eventProperties.Remove((object) oldMessageProperties[index].Name);
    }
  }

  private static Dictionary<object, PropertiesDictionary.PropertyValue> BuildEventProperties(
    IList<MessageTemplateParameter> messageProperties)
  {
    if (messageProperties == null || messageProperties.Count <= 0)
      return new Dictionary<object, PropertiesDictionary.PropertyValue>();
    Dictionary<object, PropertiesDictionary.PropertyValue> eventProperties = new Dictionary<object, PropertiesDictionary.PropertyValue>(messageProperties.Count);
    if (!PropertiesDictionary.InsertMessagePropertiesIntoEmptyDictionary(messageProperties, eventProperties))
      PropertiesDictionary.CreateUniqueMessagePropertiesListSlow(messageProperties, eventProperties);
    return eventProperties;
  }

  public object this[object key]
  {
    get
    {
      PropertiesDictionary.PropertyValue propertyValue;
      if (!this.IsEmpty && this.EventProperties.TryGetValue(key, out propertyValue))
        return propertyValue.Value;
      throw new KeyNotFoundException();
    }
    set => this.EventProperties[key] = new PropertiesDictionary.PropertyValue(value, false);
  }

  public ICollection<object> Keys => (ICollection<object>) this.KeyCollection;

  public ICollection<object> Values => (ICollection<object>) this.ValueCollection;

  private PropertiesDictionary.DictionaryCollection KeyCollection
  {
    get
    {
      if (this._keyCollection != null)
        return this._keyCollection;
      return this.IsEmpty ? PropertiesDictionary.EmptyKeyCollection : this._keyCollection ?? (this._keyCollection = new PropertiesDictionary.DictionaryCollection(this, true));
    }
  }

  private PropertiesDictionary.DictionaryCollection ValueCollection
  {
    get
    {
      if (this._valueCollection != null)
        return this._valueCollection;
      return this.IsEmpty ? PropertiesDictionary.EmptyValueCollection : this._valueCollection ?? (this._valueCollection = new PropertiesDictionary.DictionaryCollection(this, false));
    }
  }

  public int Count
  {
    get
    {
      Dictionary<object, PropertiesDictionary.PropertyValue> eventProperties = this._eventProperties;
      if (eventProperties != null)
        return __nonvirtual (eventProperties.Count);
      IList<MessageTemplateParameter> messageProperties = this._messageProperties;
      return messageProperties == null ? 0 : messageProperties.Count;
    }
  }

  public bool IsReadOnly => false;

  public void Add(object key, object value)
  {
    this.EventProperties.Add(key, new PropertiesDictionary.PropertyValue(value, false));
  }

  public void Add(KeyValuePair<object, object> item) => this.Add(item.Key, item.Value);

  public void Clear()
  {
    this._eventProperties?.Clear();
    if (this._messageProperties == null)
      return;
    this._messageProperties = (IList<MessageTemplateParameter>) ArrayHelper.Empty<MessageTemplateParameter>();
  }

  public bool Contains(KeyValuePair<object, object> item)
  {
    return !this.IsEmpty && (((ICollection<KeyValuePair<object, PropertiesDictionary.PropertyValue>>) this.EventProperties).Contains(new KeyValuePair<object, PropertiesDictionary.PropertyValue>(item.Key, new PropertiesDictionary.PropertyValue(item.Value, false))) || ((ICollection<KeyValuePair<object, PropertiesDictionary.PropertyValue>>) this.EventProperties).Contains(new KeyValuePair<object, PropertiesDictionary.PropertyValue>(item.Key, new PropertiesDictionary.PropertyValue(item.Value, true))));
  }

  public bool ContainsKey(object key) => !this.IsEmpty && this.EventProperties.ContainsKey(key);

  public void CopyTo(KeyValuePair<object, object>[] array, int arrayIndex)
  {
    if (array == null)
      throw new ArgumentNullException(nameof (array));
    if (arrayIndex < 0)
      throw new ArgumentOutOfRangeException(nameof (arrayIndex));
    if (this.IsEmpty)
      return;
    foreach (KeyValuePair<object, object> keyValuePair in this)
      array[arrayIndex++] = keyValuePair;
  }

  public IEnumerator<KeyValuePair<object, object>> GetEnumerator()
  {
    return this.IsEmpty ? Enumerable.Empty<KeyValuePair<object, object>>().GetEnumerator() : (IEnumerator<KeyValuePair<object, object>>) new PropertiesDictionary.DictionaryEnumerator(this);
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return this.IsEmpty ? ArrayHelper.Empty<KeyValuePair<object, object>>().GetEnumerator() : (IEnumerator) new PropertiesDictionary.DictionaryEnumerator(this);
  }

  public bool Remove(object key) => !this.IsEmpty && this.EventProperties.Remove(key);

  public bool Remove(KeyValuePair<object, object> item)
  {
    return !this.IsEmpty && (this.EventProperties.Remove((object) new KeyValuePair<object, PropertiesDictionary.PropertyValue>(item.Key, new PropertiesDictionary.PropertyValue(item.Value, false))) || this.EventProperties.Remove((object) new KeyValuePair<object, PropertiesDictionary.PropertyValue>(item.Key, new PropertiesDictionary.PropertyValue(item.Value, true))));
  }

  public bool TryGetValue(object key, out object value)
  {
    if (!this.IsEmpty)
    {
      if (this._eventProperties == null && key is string str)
      {
        IList<MessageTemplateParameter> messageProperties = this._messageProperties;
        if ((messageProperties != null ? (messageProperties.Count < 5 ? 1 : 0) : 0) != 0)
        {
          for (int index = 0; index < this._messageProperties.Count; ++index)
          {
            string name = this._messageProperties[index].Name;
            if (str.Equals(name, StringComparison.Ordinal))
            {
              value = this._messageProperties[index].Value;
              return true;
            }
          }
          goto label_10;
        }
      }
      PropertiesDictionary.PropertyValue propertyValue;
      if (this.EventProperties.TryGetValue(key, out propertyValue))
      {
        value = propertyValue.Value;
        return true;
      }
    }
label_10:
    value = (object) null;
    return false;
  }

  private static bool VerifyUniqueMessageTemplateParametersFast(
    IList<MessageTemplateParameter> parameterList)
  {
    if (parameterList == null || parameterList.Count == 0)
      return true;
    if (parameterList.Count > 10)
      return false;
    for (int index1 = 0; index1 < parameterList.Count - 1; ++index1)
    {
      for (int index2 = index1 + 1; index2 < parameterList.Count; ++index2)
      {
        MessageTemplateParameter parameter = parameterList[index1];
        string name1 = parameter.Name;
        parameter = parameterList[index2];
        string name2 = parameter.Name;
        if (name1 == name2)
          return false;
      }
    }
    return true;
  }

  private static bool InsertMessagePropertiesIntoEmptyDictionary(
    IList<MessageTemplateParameter> messageProperties,
    Dictionary<object, PropertiesDictionary.PropertyValue> eventProperties)
  {
    try
    {
      for (int index = 0; index < messageProperties.Count; ++index)
        eventProperties.Add((object) messageProperties[index].Name, new PropertiesDictionary.PropertyValue(messageProperties[index].Value, true));
      return true;
    }
    catch (ArgumentException ex)
    {
      for (int index = 0; index < messageProperties.Count; ++index)
        eventProperties.Remove((object) messageProperties[index].Name);
      return false;
    }
  }

  private static IList<MessageTemplateParameter> CreateUniqueMessagePropertiesListSlow(
    IList<MessageTemplateParameter> messageProperties,
    Dictionary<object, PropertiesDictionary.PropertyValue> eventProperties)
  {
    List<MessageTemplateParameter> templateParameterList = (List<MessageTemplateParameter>) null;
    for (int index1 = 0; index1 < messageProperties.Count; ++index1)
    {
      Dictionary<object, PropertiesDictionary.PropertyValue> dictionary1 = eventProperties;
      MessageTemplateParameter messageProperty = messageProperties[index1];
      string name1 = messageProperty.Name;
      PropertiesDictionary.PropertyValue propertyValue1;
      ref PropertiesDictionary.PropertyValue local = ref propertyValue1;
      if (dictionary1.TryGetValue((object) name1, out local) && propertyValue1.IsMessageProperty)
      {
        if (templateParameterList == null)
        {
          templateParameterList = new List<MessageTemplateParameter>(messageProperties.Count);
          for (int index2 = 0; index2 < index1; ++index2)
            templateParameterList.Add(messageProperties[index2]);
        }
      }
      else
      {
        Dictionary<object, PropertiesDictionary.PropertyValue> dictionary2 = eventProperties;
        messageProperty = messageProperties[index1];
        string name2 = messageProperty.Name;
        messageProperty = messageProperties[index1];
        PropertiesDictionary.PropertyValue propertyValue2 = new PropertiesDictionary.PropertyValue(messageProperty.Value, true);
        dictionary2[(object) name2] = propertyValue2;
        templateParameterList?.Add(messageProperties[index1]);
      }
    }
    return (IList<MessageTemplateParameter>) templateParameterList ?? messageProperties;
  }

  IEnumerator<MessageTemplateParameter> IEnumerable<MessageTemplateParameter>.GetEnumerator()
  {
    return (IEnumerator<MessageTemplateParameter>) new PropertiesDictionary.ParameterEnumerator(this);
  }

  private struct PropertyValue(object value, bool isMessageProperty)
  {
    public readonly object Value = value;
    public readonly bool IsMessageProperty = isMessageProperty;
  }

  private abstract class DictionaryEnumeratorBase : IDisposable
  {
    private readonly PropertiesDictionary _dictionary;
    private int? _messagePropertiesEnumerator;
    private bool _eventEnumeratorCreated;
    private Dictionary<object, PropertiesDictionary.PropertyValue>.Enumerator _eventEnumerator;

    protected DictionaryEnumeratorBase(PropertiesDictionary dictionary)
    {
      this._dictionary = dictionary;
    }

    protected KeyValuePair<object, object> CurrentProperty
    {
      get
      {
        if (this._messagePropertiesEnumerator.HasValue)
        {
          MessageTemplateParameter messageProperty = this._dictionary._messageProperties[this._messagePropertiesEnumerator.Value];
          return new KeyValuePair<object, object>((object) messageProperty.Name, messageProperty.Value);
        }
        if (!this._eventEnumeratorCreated)
          throw new InvalidOperationException();
        KeyValuePair<object, PropertiesDictionary.PropertyValue> current = this._eventEnumerator.Current;
        object key = current.Key;
        current = this._eventEnumerator.Current;
        object obj = current.Value.Value;
        return new KeyValuePair<object, object>(key, obj);
      }
    }

    protected MessageTemplateParameter CurrentParameter
    {
      get
      {
        if (this._messagePropertiesEnumerator.HasValue)
          return this._dictionary._messageProperties[this._messagePropertiesEnumerator.Value];
        if (!this._eventEnumeratorCreated)
          throw new InvalidOperationException();
        KeyValuePair<object, PropertiesDictionary.PropertyValue> current = this._eventEnumerator.Current;
        string name = XmlHelper.XmlConvertToString(current.Key ?? (object) string.Empty) ?? string.Empty;
        current = this._eventEnumerator.Current;
        return new MessageTemplateParameter(name, current.Value.Value, (string) null, CaptureType.Unknown);
      }
    }

    public bool MoveNext()
    {
      if (this._messagePropertiesEnumerator.HasValue)
      {
        if (this._messagePropertiesEnumerator.Value + 1 < this._dictionary._messageProperties.Count)
        {
          this._messagePropertiesEnumerator = this.FindNextValidMessagePropertyIndex(this._messagePropertiesEnumerator.Value + 1);
          if (this._messagePropertiesEnumerator.HasValue)
            return true;
          this._messagePropertiesEnumerator = new int?(this._dictionary._eventProperties.Count - 1);
        }
        if (!PropertiesDictionary.DictionaryEnumeratorBase.HasEventProperties(this._dictionary))
          return false;
        this._messagePropertiesEnumerator = new int?();
        this._eventEnumerator = this._dictionary._eventProperties.GetEnumerator();
        this._eventEnumeratorCreated = true;
        return this.MoveNextValidEventProperty();
      }
      if (this._eventEnumeratorCreated)
        return this.MoveNextValidEventProperty();
      if (PropertiesDictionary.DictionaryEnumeratorBase.HasMessageProperties(this._dictionary))
      {
        this._messagePropertiesEnumerator = this.FindNextValidMessagePropertyIndex(0);
        if (this._messagePropertiesEnumerator.HasValue)
          return true;
      }
      if (!PropertiesDictionary.DictionaryEnumeratorBase.HasEventProperties(this._dictionary))
        return false;
      this._eventEnumerator = this._dictionary._eventProperties.GetEnumerator();
      this._eventEnumeratorCreated = true;
      return this.MoveNextValidEventProperty();
    }

    private static bool HasMessageProperties(PropertiesDictionary propertiesDictionary)
    {
      return propertiesDictionary._messageProperties != null && propertiesDictionary._messageProperties.Count > 0;
    }

    private static bool HasEventProperties(PropertiesDictionary propertiesDictionary)
    {
      return propertiesDictionary._eventProperties != null && propertiesDictionary._eventProperties.Count > 0;
    }

    private bool MoveNextValidEventProperty()
    {
      while (this._eventEnumerator.MoveNext())
      {
        if (!this._eventEnumerator.Current.Value.IsMessageProperty)
          return true;
      }
      return false;
    }

    private int? FindNextValidMessagePropertyIndex(int startIndex)
    {
      if (this._dictionary._eventProperties == null)
        return new int?(startIndex);
      for (int index = startIndex; index < this._dictionary._messageProperties.Count; ++index)
      {
        PropertiesDictionary.PropertyValue propertyValue;
        if (this._dictionary._eventProperties.TryGetValue((object) this._dictionary._messageProperties[index].Name, out propertyValue) && propertyValue.IsMessageProperty)
          return new int?(index);
      }
      return new int?();
    }

    public void Dispose()
    {
    }

    public void Reset()
    {
      this._messagePropertiesEnumerator = new int?();
      this._eventEnumeratorCreated = false;
      this._eventEnumerator = new Dictionary<object, PropertiesDictionary.PropertyValue>.Enumerator();
    }
  }

  private class ParameterEnumerator(PropertiesDictionary dictionary) : 
    PropertiesDictionary.DictionaryEnumeratorBase(dictionary),
    IEnumerator<MessageTemplateParameter>,
    IDisposable,
    IEnumerator
  {
    public MessageTemplateParameter Current => this.CurrentParameter;

    object IEnumerator.Current => (object) this.CurrentParameter;
  }

  private class DictionaryEnumerator(PropertiesDictionary dictionary) : 
    PropertiesDictionary.DictionaryEnumeratorBase(dictionary),
    IEnumerator<KeyValuePair<object, object>>,
    IDisposable,
    IEnumerator
  {
    public KeyValuePair<object, object> Current => this.CurrentProperty;

    object IEnumerator.Current => (object) this.CurrentProperty;
  }

  [DebuggerDisplay("Count = {Count}")]
  private class DictionaryCollection : ICollection<object>, IEnumerable<object>, IEnumerable
  {
    private readonly PropertiesDictionary _dictionary;
    private readonly bool _keyCollection;

    public DictionaryCollection(PropertiesDictionary dictionary, bool keyCollection)
    {
      this._dictionary = dictionary;
      this._keyCollection = keyCollection;
    }

    public int Count => this._dictionary.Count;

    public bool IsReadOnly => true;

    public void Add(object item) => throw new NotSupportedException();

    public void Clear() => throw new NotSupportedException();

    public bool Remove(object item) => throw new NotSupportedException();

    public bool Contains(object item)
    {
      if (this._keyCollection)
        return this._dictionary.ContainsKey(item);
      return !this._dictionary.IsEmpty && (this._dictionary.EventProperties.ContainsValue(new PropertiesDictionary.PropertyValue(item, false)) || this._dictionary.EventProperties.ContainsValue(new PropertiesDictionary.PropertyValue(item, true)));
    }

    public void CopyTo(object[] array, int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException(nameof (array));
      if (arrayIndex < 0)
        throw new ArgumentOutOfRangeException(nameof (arrayIndex));
      if (this._dictionary.IsEmpty)
        return;
      foreach (KeyValuePair<object, object> keyValuePair in this._dictionary)
        array[arrayIndex++] = this._keyCollection ? keyValuePair.Key : keyValuePair.Value;
    }

    public IEnumerator<object> GetEnumerator()
    {
      return (IEnumerator<object>) new PropertiesDictionary.DictionaryCollection.DictionaryCollectionEnumerator(this._dictionary, this._keyCollection);
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    private class DictionaryCollectionEnumerator : 
      PropertiesDictionary.DictionaryEnumeratorBase,
      IEnumerator<object>,
      IDisposable,
      IEnumerator
    {
      private readonly bool _keyCollection;

      public DictionaryCollectionEnumerator(PropertiesDictionary dictionary, bool keyCollection)
        : base(dictionary)
      {
        this._keyCollection = keyCollection;
      }

      public object Current
      {
        get => !this._keyCollection ? this.CurrentProperty.Value : this.CurrentProperty.Key;
      }
    }
  }
}
