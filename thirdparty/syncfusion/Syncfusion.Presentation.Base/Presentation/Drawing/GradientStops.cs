// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.GradientStops
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class GradientStops : IGradientStops, IEnumerable<IGradientStop>, IEnumerable
{
  private GradientFill _gradientFill;
  private List<IGradientStop> _list;

  internal GradientStops(GradientFill gradientFill)
  {
    this._gradientFill = gradientFill;
    this._list = new List<IGradientStop>();
  }

  public int Count => this._list.Count;

  public IGradientStop this[int index] => this._list[index];

  public IEnumerator<IGradientStop> GetEnumerator()
  {
    return (IEnumerator<IGradientStop>) this._list.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();

  public IGradientStop Add()
  {
    GradientStop gradientStop = new GradientStop(this);
    this._list.Add((IGradientStop) gradientStop);
    if (this._gradientFill.FillFormat.Parent is Background)
    {
      Background parent = this._gradientFill.FillFormat.Parent as Background;
      if (parent.GetFillFormat().FillType == FillType.Automatic)
        parent.SetFill((IFill) this._gradientFill.FillFormat);
    }
    return (IGradientStop) gradientStop;
  }

  public int Add(IGradientStop gradientStop)
  {
    this._list.Add(gradientStop);
    if (this._gradientFill.FillFormat.Parent is Background)
    {
      Background parent = this._gradientFill.FillFormat.Parent as Background;
      if (parent.GetFillFormat().FillType == FillType.Automatic)
        parent.SetFill((IFill) this._gradientFill.FillFormat);
    }
    return this._list.IndexOf(gradientStop);
  }

  public void Insert(int index, IGradientStop item) => this._list.Insert(index, item);

  public void RemoveAt(int index) => this._list.RemoveAt(index);

  public void Remove(IGradientStop item) => this._list.Remove(item);

  public int IndexOf(IGradientStop item) => this._list.IndexOf(item);

  public void Clear() => this._list.Clear();

  public void Add(IColor rgb, float position)
  {
    this.Add(new GradientStop(this)
    {
      Color = rgb,
      Position = position
    });
  }

  public void Insert(int index, IColor rgb, float position)
  {
    this._list.Insert(index, (IGradientStop) new GradientStop(this)
    {
      Color = rgb,
      Position = position
    });
  }

  internal Syncfusion.Presentation.Presentation Presentation
  {
    get => this._gradientFill.FillFormat.Presentation;
  }

  internal BaseSlide BaseSlide => this._gradientFill.FillFormat.BaseSlide;

  internal GradientFill GetGradientFill() => this._gradientFill;

  internal void Add(GradientStop gradientStop) => this._list.Add((IGradientStop) gradientStop);

  internal void Close() => this.CloseAll();

  private void CloseAll()
  {
    if (this._list != null)
    {
      foreach (GradientStop gradientStop in this._list)
        gradientStop.Close();
      this._list.Clear();
      this._list = (List<IGradientStop>) null;
    }
    this._gradientFill = (GradientFill) null;
  }

  public GradientStops Clone()
  {
    GradientStops newParent = (GradientStops) this.MemberwiseClone();
    newParent._list = this.CloneGradientStopList(newParent);
    return newParent;
  }

  private List<IGradientStop> CloneGradientStopList(GradientStops newParent)
  {
    List<IGradientStop> gradientStopList = new List<IGradientStop>();
    foreach (GradientStop gradientStop1 in this._list)
    {
      GradientStop gradientStop2 = gradientStop1.Clone();
      gradientStop2.SetParent(newParent);
      gradientStopList.Add((IGradientStop) gradientStop2);
    }
    return gradientStopList;
  }

  internal void SetParent(GradientFill gradientFill) => this._gradientFill = gradientFill;
}
