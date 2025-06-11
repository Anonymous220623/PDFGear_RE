// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideImplementation.MasterSlides
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.SlideImplementation;

internal class MasterSlides : IMasterSlides, IEnumerable<IMasterSlide>, IEnumerable
{
  private List<IMasterSlide> _list;
  private Syncfusion.Presentation.Presentation _presentation;

  internal MasterSlides(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
    this._list = new List<IMasterSlide>();
  }

  internal Syncfusion.Presentation.Presentation Presentation => this._presentation;

  public int Count => this._list.Count;

  public IMasterSlide this[int index]
  {
    get => index < this._list.Count ? this._list[index] : (IMasterSlide) null;
  }

  public IEnumerator GetEnumerator() => (IEnumerator) this._list.GetEnumerator();

  IEnumerator<IMasterSlide> IEnumerable<IMasterSlide>.GetEnumerator()
  {
    return (IEnumerator<IMasterSlide>) this._list.GetEnumerator();
  }

  public void Add(IMasterSlide slide) => this._list.Add(slide);

  public void Insert(int index, IMasterSlide slide) => this._list.Insert(index, slide);

  public void Remove(IMasterSlide value) => this.RemoveMasterSlide(value as MasterSlide);

  public void RemoveAt(int index) => this.Remove(this._list[index]);

  public int IndexOf(IMasterSlide slide) => this._list.IndexOf(slide);

  public void Clear() => this.ClearAll();

  internal void Add(MasterSlide masterSlide) => this._list.Add((IMasterSlide) masterSlide);

  private void RemoveMasterSlide(MasterSlide masterSlide)
  {
    string str = (string) null;
    foreach (KeyValuePair<string, string> master in masterSlide.Presentation.MasterList)
    {
      if (master.Value == masterSlide.MasterId.ToString())
      {
        str = master.Key;
        break;
      }
    }
    string partName = "/ppt/" + masterSlide.Presentation.TopRelation.GetItemPathByRelation(str);
    masterSlide.Presentation.RemoveOverrideContentType(partName);
    masterSlide.Presentation.TopRelation.RemoveRelation(str);
    masterSlide.Presentation.MasterList.Remove(str);
    this._list.Remove((IMasterSlide) masterSlide);
  }

  internal void Close() => this.ClearAll();

  private void ClearAll()
  {
    if (this._list != null)
    {
      foreach (BaseSlide baseSlide in this._list)
        baseSlide.Close();
      this._list.Clear();
      this._list = (List<IMasterSlide>) null;
    }
    this._presentation = (Syncfusion.Presentation.Presentation) null;
  }

  public MasterSlides Clone()
  {
    MasterSlides masterSlides = (MasterSlides) this.MemberwiseClone();
    masterSlides._list = this.CloneMasterList(masterSlides._presentation);
    return masterSlides;
  }

  private List<IMasterSlide> CloneMasterList(Syncfusion.Presentation.Presentation newParent)
  {
    List<IMasterSlide> masterSlideList = new List<IMasterSlide>();
    foreach (MasterSlide masterSlide1 in this._list)
    {
      MasterSlide masterSlide2 = masterSlide1.Clone();
      masterSlide2.Presentation = newParent;
      masterSlideList.Add((IMasterSlide) masterSlide2);
    }
    return masterSlideList;
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
    foreach (BaseSlide baseSlide in this._list)
      baseSlide.SetParent(presentation);
  }
}
