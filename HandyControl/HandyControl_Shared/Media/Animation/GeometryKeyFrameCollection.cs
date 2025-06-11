// Decompiled with JetBrains decompiler
// Type: HandyControl.Media.Animation.GeometryKeyFrameCollection
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

#nullable disable
namespace HandyControl.Media.Animation;

public class GeometryKeyFrameCollection : Freezable, IList, ICollection, IEnumerable
{
  private List<GeometryKeyFrame> _keyFrames;
  private static GeometryKeyFrameCollection s_emptyCollection;

  public GeometryKeyFrameCollection() => this._keyFrames = new List<GeometryKeyFrame>(2);

  public static GeometryKeyFrameCollection Empty
  {
    get
    {
      if (GeometryKeyFrameCollection.s_emptyCollection == null)
      {
        GeometryKeyFrameCollection keyFrameCollection = new GeometryKeyFrameCollection();
        keyFrameCollection._keyFrames = new List<GeometryKeyFrame>(0);
        keyFrameCollection.Freeze();
        GeometryKeyFrameCollection.s_emptyCollection = keyFrameCollection;
      }
      return GeometryKeyFrameCollection.s_emptyCollection;
    }
  }

  public GeometryKeyFrameCollection Clone() => (GeometryKeyFrameCollection) base.Clone();

  protected override Freezable CreateInstanceCore() => (Freezable) new GeometryKeyFrameCollection();

  protected override void CloneCore(Freezable sourceFreezable)
  {
    GeometryKeyFrameCollection keyFrameCollection = (GeometryKeyFrameCollection) sourceFreezable;
    base.CloneCore(sourceFreezable);
    int count = keyFrameCollection._keyFrames.Count;
    this._keyFrames = new List<GeometryKeyFrame>(count);
    for (int index = 0; index < count; ++index)
    {
      GeometryKeyFrame newValue = (GeometryKeyFrame) keyFrameCollection._keyFrames[index].Clone();
      this._keyFrames.Add(newValue);
      this.OnFreezablePropertyChanged((DependencyObject) null, (DependencyObject) newValue);
    }
  }

  protected override void CloneCurrentValueCore(Freezable sourceFreezable)
  {
    GeometryKeyFrameCollection keyFrameCollection = (GeometryKeyFrameCollection) sourceFreezable;
    base.CloneCurrentValueCore(sourceFreezable);
    int count = keyFrameCollection._keyFrames.Count;
    this._keyFrames = new List<GeometryKeyFrame>(count);
    for (int index = 0; index < count; ++index)
    {
      GeometryKeyFrame newValue = (GeometryKeyFrame) keyFrameCollection._keyFrames[index].CloneCurrentValue();
      this._keyFrames.Add(newValue);
      this.OnFreezablePropertyChanged((DependencyObject) null, (DependencyObject) newValue);
    }
  }

  protected override void GetAsFrozenCore(Freezable sourceFreezable)
  {
    GeometryKeyFrameCollection keyFrameCollection = (GeometryKeyFrameCollection) sourceFreezable;
    base.GetAsFrozenCore(sourceFreezable);
    int count = keyFrameCollection._keyFrames.Count;
    this._keyFrames = new List<GeometryKeyFrame>(count);
    for (int index = 0; index < count; ++index)
    {
      GeometryKeyFrame asFrozen = (GeometryKeyFrame) keyFrameCollection._keyFrames[index].GetAsFrozen();
      this._keyFrames.Add(asFrozen);
      this.OnFreezablePropertyChanged((DependencyObject) null, (DependencyObject) asFrozen);
    }
  }

  protected override void GetCurrentValueAsFrozenCore(Freezable sourceFreezable)
  {
    GeometryKeyFrameCollection keyFrameCollection = (GeometryKeyFrameCollection) sourceFreezable;
    base.GetCurrentValueAsFrozenCore(sourceFreezable);
    int count = keyFrameCollection._keyFrames.Count;
    this._keyFrames = new List<GeometryKeyFrame>(count);
    for (int index = 0; index < count; ++index)
    {
      GeometryKeyFrame currentValueAsFrozen = (GeometryKeyFrame) keyFrameCollection._keyFrames[index].GetCurrentValueAsFrozen();
      this._keyFrames.Add(currentValueAsFrozen);
      this.OnFreezablePropertyChanged((DependencyObject) null, (DependencyObject) currentValueAsFrozen);
    }
  }

  protected override bool FreezeCore(bool isChecking)
  {
    bool flag = base.FreezeCore(isChecking);
    for (int index = 0; index < this._keyFrames.Count & flag; ++index)
      flag &= Freezable.Freeze((Freezable) this._keyFrames[index], isChecking);
    return flag;
  }

  public IEnumerator GetEnumerator()
  {
    this.ReadPreamble();
    return (IEnumerator) this._keyFrames.GetEnumerator();
  }

  void ICollection.CopyTo(Array array, int index)
  {
    this.ReadPreamble();
    ((ICollection) this._keyFrames).CopyTo(array, index);
  }

  public void CopyTo(GeometryKeyFrame[] array, int index)
  {
    this.ReadPreamble();
    this._keyFrames.CopyTo(array, index);
  }

  public int Count
  {
    get
    {
      this.ReadPreamble();
      return this._keyFrames.Count;
    }
  }

  public object SyncRoot
  {
    get
    {
      this.ReadPreamble();
      return ((ICollection) this._keyFrames).SyncRoot;
    }
  }

  public bool IsSynchronized
  {
    get
    {
      this.ReadPreamble();
      return this.IsFrozen || this.Dispatcher != null;
    }
  }

  int IList.Add(object keyFrame) => this.Add((GeometryKeyFrame) keyFrame);

  public int Add(GeometryKeyFrame keyFrame)
  {
    if (keyFrame == null)
      throw new ArgumentNullException(nameof (keyFrame));
    this.WritePreamble();
    this.OnFreezablePropertyChanged((DependencyObject) null, (DependencyObject) keyFrame);
    this._keyFrames.Add(keyFrame);
    this.WritePostscript();
    return this._keyFrames.Count - 1;
  }

  public void Clear()
  {
    this.WritePreamble();
    if (this._keyFrames.Count <= 0)
      return;
    foreach (DependencyObject keyFrame in this._keyFrames)
      this.OnFreezablePropertyChanged(keyFrame, (DependencyObject) null);
    this._keyFrames.Clear();
    this.WritePostscript();
  }

  bool IList.Contains(object keyFrame) => this.Contains((GeometryKeyFrame) keyFrame);

  public bool Contains(GeometryKeyFrame keyFrame)
  {
    this.ReadPreamble();
    return this._keyFrames.Contains(keyFrame);
  }

  int IList.IndexOf(object keyFrame) => this.IndexOf((GeometryKeyFrame) keyFrame);

  public int IndexOf(GeometryKeyFrame keyFrame)
  {
    this.ReadPreamble();
    return this._keyFrames.IndexOf(keyFrame);
  }

  void IList.Insert(int index, object keyFrame) => this.Insert(index, (GeometryKeyFrame) keyFrame);

  public void Insert(int index, GeometryKeyFrame keyFrame)
  {
    if (keyFrame == null)
      throw new ArgumentNullException(nameof (keyFrame));
    this.WritePreamble();
    this.OnFreezablePropertyChanged((DependencyObject) null, (DependencyObject) keyFrame);
    this._keyFrames.Insert(index, keyFrame);
    this.WritePostscript();
  }

  void IList.Remove(object keyFrame) => this.Remove((GeometryKeyFrame) keyFrame);

  public void Remove(GeometryKeyFrame keyFrame)
  {
    this.WritePreamble();
    if (!this._keyFrames.Contains(keyFrame))
      return;
    this.OnFreezablePropertyChanged((DependencyObject) keyFrame, (DependencyObject) null);
    this._keyFrames.Remove(keyFrame);
    this.WritePostscript();
  }

  public void RemoveAt(int index)
  {
    this.WritePreamble();
    this.OnFreezablePropertyChanged((DependencyObject) this._keyFrames[index], (DependencyObject) null);
    this._keyFrames.RemoveAt(index);
    this.WritePostscript();
  }

  object IList.this[int index]
  {
    get => (object) this[index];
    set => this[index] = (GeometryKeyFrame) value;
  }

  public GeometryKeyFrame this[int index]
  {
    get
    {
      this.ReadPreamble();
      return this._keyFrames[index];
    }
    set
    {
      if (value == null)
        throw new ArgumentNullException(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "DoubleKeyFrameCollection[{0}]", (object) index));
      this.WritePreamble();
      if (value == this._keyFrames[index])
        return;
      this.OnFreezablePropertyChanged((DependencyObject) this._keyFrames[index], (DependencyObject) value);
      this._keyFrames[index] = value;
      this.WritePostscript();
    }
  }

  public bool IsReadOnly
  {
    get
    {
      this.ReadPreamble();
      return this.IsFrozen;
    }
  }

  public bool IsFixedSize
  {
    get
    {
      this.ReadPreamble();
      return this.IsFrozen;
    }
  }
}
