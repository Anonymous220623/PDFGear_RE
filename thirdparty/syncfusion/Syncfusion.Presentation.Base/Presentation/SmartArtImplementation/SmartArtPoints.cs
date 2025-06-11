// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SmartArtImplementation.SmartArtPoints
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.SmartArtImplementation;

internal class SmartArtPoints
{
  private DataModel _dataModel;
  private Dictionary<Guid, SmartArtPoint> _list;

  internal SmartArtPoints(DataModel dataModel)
  {
    this._dataModel = dataModel;
    this._list = new Dictionary<Guid, SmartArtPoint>();
  }

  internal SmartArtPoint this[int index]
  {
    get
    {
      if (index < 0 && index >= this._list.Count)
        return (SmartArtPoint) null;
      Guid[] array = new Guid[this._list.Count];
      this._list.Keys.CopyTo(array, 0);
      return this._list[array[index]];
    }
  }

  internal SmartArtPoint this[Guid key] => this._list[key];

  internal void Add(Guid modelId, SmartArtPoint point) => this._list.Add(modelId, point);

  internal bool TryGetValue(Guid key, out SmartArtPoint value)
  {
    return this._list.TryGetValue(key, out value);
  }

  internal SmartArtPoint GetPointByElementId(Guid elementId)
  {
    foreach (KeyValuePair<Guid, SmartArtPoint> keyValuePair in this._list)
    {
      SmartArtPoint pointByElementId = keyValuePair.Value;
      if (pointByElementId.Type == SmartArtPointType.Node && pointByElementId.ModelId == elementId)
        return pointByElementId;
    }
    return (SmartArtPoint) null;
  }

  private Guid GetElementId(Guid sourceId)
  {
    foreach (KeyValuePair<Guid, SmartArtPoint> keyValuePair in this._list)
    {
      SmartArtPoint smartArtPoint = keyValuePair.Value;
      if (smartArtPoint.Type == SmartArtPointType.Presentation && smartArtPoint.ModelId == sourceId)
        return smartArtPoint.PresentationElementId;
    }
    return Guid.Empty;
  }

  internal List<SmartArtPoint> GetPointList()
  {
    SmartArtPoint[] smartArtPointArray = new SmartArtPoint[this._list.Count];
    this._list.Values.CopyTo(smartArtPointArray, 0);
    return new List<SmartArtPoint>((IEnumerable<SmartArtPoint>) smartArtPointArray);
  }

  internal void Remove(Guid key) => this._list.Remove(key);

  internal int IndexOf(Guid key)
  {
    Guid[] guidArray = new Guid[this._list.Count];
    this._list.Keys.CopyTo(guidArray, 0);
    return new List<Guid>((IEnumerable<Guid>) guidArray).IndexOf(key);
  }

  internal Guid GetKeyByIndex(int index)
  {
    Guid[] guidArray = new Guid[this._list.Count];
    this._list.Keys.CopyTo(guidArray, 0);
    return new List<Guid>((IEnumerable<Guid>) guidArray)[index];
  }

  internal void Clear()
  {
    SmartArtPoint[] array = new SmartArtPoint[this._list.Count];
    this._list.Values.CopyTo(array, 0);
    foreach (SmartArtPoint smartArtPoint in array)
    {
      if (smartArtPoint.Type != SmartArtPointType.Document)
        this._list.Remove(smartArtPoint.ModelId);
    }
  }

  internal void RemoveByPointType(SmartArtPointType smartArtPointType)
  {
    SmartArtPoint[] array = new SmartArtPoint[this._list.Count];
    this._list.Values.CopyTo(array, 0);
    foreach (SmartArtPoint smartArtPoint in array)
    {
      if (smartArtPoint.Type == smartArtPointType)
        this._list.Remove(smartArtPoint.ModelId);
    }
  }

  internal void Close()
  {
    this._dataModel = (DataModel) null;
    foreach (KeyValuePair<Guid, SmartArtPoint> keyValuePair in this._list)
      keyValuePair.Value.Close();
    this._list.Clear();
  }

  public SmartArtPoints Clone()
  {
    SmartArtPoints smartArtPoints = (SmartArtPoints) this.MemberwiseClone();
    smartArtPoints._list = this.ClonePointList();
    return smartArtPoints;
  }

  private Dictionary<Guid, SmartArtPoint> ClonePointList()
  {
    Dictionary<Guid, SmartArtPoint> dictionary = new Dictionary<Guid, SmartArtPoint>();
    foreach (KeyValuePair<Guid, SmartArtPoint> keyValuePair in this._list)
    {
      SmartArtPoint smartArtPoint = keyValuePair.Value.Clone();
      dictionary.Add(keyValuePair.Key, smartArtPoint);
    }
    return dictionary;
  }

  internal void SetParent(DataModel dataModel)
  {
    this._dataModel = dataModel;
    foreach (KeyValuePair<Guid, SmartArtPoint> keyValuePair in this._list)
      keyValuePair.Value.SetParent(dataModel);
  }

  internal void SetPointTextBodyParent(BaseSlide baseSlide)
  {
    foreach (KeyValuePair<Guid, SmartArtPoint> keyValuePair in this._list)
    {
      if (keyValuePair.Value.TextBody != null)
        keyValuePair.Value.TextBody.SetParent(baseSlide);
    }
  }
}
