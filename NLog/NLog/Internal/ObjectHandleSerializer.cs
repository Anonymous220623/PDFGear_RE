// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ObjectHandleSerializer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

#nullable disable
namespace NLog.Internal;

[Serializable]
internal class ObjectHandleSerializer : ISerializable
{
  [NonSerialized]
  private readonly object _wrapped;

  public ObjectHandleSerializer()
  {
  }

  public ObjectHandleSerializer(object wrapped) => this._wrapped = wrapped;

  protected ObjectHandleSerializer(SerializationInfo info, StreamingContext context)
  {
    Type type = (Type) null;
    try
    {
      type = (Type) info.GetValue("wrappedtype", typeof (Type));
      this._wrapped = info.GetValue("wrappedvalue", type);
    }
    catch (Exception ex)
    {
      this._wrapped = (object) string.Empty;
      object[] objArray = new object[1]{ (object) type };
      InternalLogger.Debug(ex, "ObjectHandleSerializer failed to deserialize object: {0}", objArray);
    }
  }

  [SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
  public void GetObjectData(SerializationInfo info, StreamingContext context)
  {
    try
    {
      if (this._wrapped is ISerializable || this._wrapped.GetType().IsSerializable)
      {
        info.AddValue("wrappedtype", (object) this._wrapped.GetType());
        info.AddValue("wrappedvalue", this._wrapped);
      }
      else
      {
        info.AddValue("wrappedtype", (object) typeof (string));
        string empty = string.Empty;
        try
        {
          empty = this._wrapped?.ToString();
        }
        finally
        {
          info.AddValue("wrappedvalue", (object) (empty ?? string.Empty));
        }
      }
    }
    catch (Exception ex)
    {
      object[] objArray = new object[1]
      {
        (object) this._wrapped?.GetType()
      };
      InternalLogger.Debug(ex, "ObjectHandleSerializer failed to serialize object: {0}", objArray);
    }
  }

  public object Unwrap() => this._wrapped ?? (object) string.Empty;
}
