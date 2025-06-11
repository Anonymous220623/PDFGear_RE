// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SmartArtImplementation.SmartArtConnections
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.SmartArtImplementation;

internal class SmartArtConnections
{
  private DataModel _dataModel;
  private System.Collections.Generic.Dictionary<Guid, SmartArtConnection> _list;

  internal SmartArtConnections(DataModel dataModel)
  {
    this._dataModel = dataModel;
    this._list = new System.Collections.Generic.Dictionary<Guid, SmartArtConnection>();
  }

  internal System.Collections.Generic.Dictionary<Guid, SmartArtConnection> Dictionary => this._list;

  internal SmartArtConnection this[int index]
  {
    get
    {
      if (index < 0 && index >= this._list.Count)
        return (SmartArtConnection) null;
      Guid[] array = new Guid[this._list.Count];
      this._list.Keys.CopyTo(array, 0);
      return this._list[array[index]];
    }
  }

  internal SmartArtConnection this[Guid key] => this._list[key];

  internal void Add(Guid modelId, SmartArtConnection connection)
  {
    this._list.Add(modelId, connection);
  }

  internal Guid GetDestinationId(Guid guid)
  {
    foreach (KeyValuePair<Guid, SmartArtConnection> keyValuePair in this._list)
    {
      SmartArtConnection smartArtConnection = keyValuePair.Value;
      if (smartArtConnection.Type == SmartArtConnectionType.PresentationOf && smartArtConnection.SourceId == guid)
        return smartArtConnection.DestinationId;
    }
    return Guid.Empty;
  }

  internal List<SmartArtConnection> GetConnectionList()
  {
    SmartArtConnection[] smartArtConnectionArray = new SmartArtConnection[this._list.Count];
    this._list.Values.CopyTo(smartArtConnectionArray, 0);
    return new List<SmartArtConnection>((IEnumerable<SmartArtConnection>) smartArtConnectionArray);
  }

  internal void Remove(Guid key) => this._list.Remove(key);

  internal void Clear() => this._list.Clear();

  internal void RemoveByConnectionType(SmartArtConnectionType smartArtConnectionType)
  {
    SmartArtConnection[] array = new SmartArtConnection[this._list.Count];
    this._list.Values.CopyTo(array, 0);
    foreach (SmartArtConnection smartArtConnection in array)
    {
      if (smartArtConnection.Type == smartArtConnectionType)
        this._list.Remove(smartArtConnection.ModelId);
    }
  }

  internal void Close()
  {
    this._dataModel = (DataModel) null;
    foreach (KeyValuePair<Guid, SmartArtConnection> keyValuePair in this._list)
      keyValuePair.Value.Close();
    this._list.Clear();
  }

  public SmartArtConnections Clone()
  {
    SmartArtConnections smartArtConnections = (SmartArtConnections) this.MemberwiseClone();
    smartArtConnections._list = this.CloneConnectionDictionary();
    return smartArtConnections;
  }

  private System.Collections.Generic.Dictionary<Guid, SmartArtConnection> CloneConnectionDictionary()
  {
    System.Collections.Generic.Dictionary<Guid, SmartArtConnection> dictionary = new System.Collections.Generic.Dictionary<Guid, SmartArtConnection>();
    foreach (KeyValuePair<Guid, SmartArtConnection> keyValuePair in this._list)
    {
      SmartArtConnection smartArtConnection = keyValuePair.Value.Clone();
      dictionary.Add(keyValuePair.Key, smartArtConnection);
    }
    return dictionary;
  }

  internal void SetParent(DataModel dataModel)
  {
    this._dataModel = dataModel;
    foreach (KeyValuePair<Guid, SmartArtConnection> keyValuePair in this._list)
      keyValuePair.Value.SetParent(dataModel);
  }
}
