// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.EnumBindingObject`1
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using PDFKit.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable disable
namespace pdfeditor.Models;

public class EnumBindingObject<T> : DynamicObject, INotifyPropertyChanged where T : struct, Enum
{
  private static IReadOnlyCollection<string> _enumNames;
  private static object locker = new object();
  private static Dictionary<string, long> _enumValues;
  private T value;
  private readonly T defaultValue;

  public EnumBindingObject(T defaultValue)
  {
    if (EnumBindingObject<T>._enumNames == null)
    {
      lock (EnumBindingObject<T>.locker)
      {
        if (EnumBindingObject<T>._enumNames == null)
        {
          EnumBindingObject<T>._enumNames = (IReadOnlyCollection<string>) EnumHelper<T>.NameValueDict.Keys.ToArray<string>();
          EnumBindingObject<T>._enumValues = EnumHelper<T>.NameValueDict.ToDictionary<KeyValuePair<string, T>, string, long>((Func<KeyValuePair<string, T>, string>) (c => c.Key), (Func<KeyValuePair<string, T>, long>) (c => Convert.ToInt64((object) c.Value)));
        }
      }
    }
    this.defaultValue = defaultValue;
    this.value = defaultValue;
  }

  public T Value
  {
    get => this.value;
    set
    {
      if (object.Equals((object) this.value, (object) value))
        return;
      long int64 = Convert.ToInt64((object) this.value);
      this.value = value;
      this.OnPropertyChanged(nameof (Value));
      this.RaiseValueChanged(int64);
      this.RaiseValueChanged(Convert.ToInt64((object) this.value));
    }
  }

  public override bool TryGetMember(GetMemberBinder binder, out object result)
  {
    result = (object) null;
    if (binder.Name == "Value" && (binder.ReturnType == typeof (T) || binder.ReturnType == typeof (object)))
    {
      result = (object) this.Value;
      return true;
    }
    long num;
    if (!(binder.ReturnType == typeof (bool)) && !(binder.ReturnType == typeof (object)) || !EnumBindingObject<T>._enumValues.TryGetValue(binder.Name, out num))
      return false;
    result = (object) (Convert.ToInt64((object) this.Value) == num);
    return true;
  }

  public override bool TrySetMember(SetMemberBinder binder, object value)
  {
    if (binder.Name == "Value")
    {
      if (!(value is T obj))
        throw new ArgumentException(nameof (value));
      Convert.ToInt64((object) this.Value);
      this.Value = obj;
      return true;
    }
    long num;
    if (!(value is bool flag) || !EnumBindingObject<T>._enumValues.TryGetValue(binder.Name, out num))
      return false;
    if (Convert.ToInt64((object) this.Value) == num)
    {
      if (!flag)
        this.Value = this.defaultValue;
    }
    else
    {
      T result;
      if (flag && Enum.TryParse<T>(binder.Name, out result))
        this.Value = result;
    }
    return true;
  }

  public event PropertyChangedEventHandler PropertyChanged;

  protected void OnPropertyChanged([CallerMemberName] string propName = "")
  {
    PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
    if (propertyChanged == null)
      return;
    propertyChanged((object) this, new PropertyChangedEventArgs(propName));
  }

  protected void RaiseValueChanged(long value)
  {
    foreach (KeyValuePair<string, long> enumValue in EnumBindingObject<T>._enumValues)
    {
      if (enumValue.Value == value)
        this.OnPropertyChanged(enumValue.Key);
    }
  }
}
