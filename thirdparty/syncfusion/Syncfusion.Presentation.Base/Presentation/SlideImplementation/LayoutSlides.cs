// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideImplementation.LayoutSlides
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.SlideImplementation;

internal class LayoutSlides : ILayoutSlides, IEnumerable<ILayoutSlide>, IEnumerable
{
  private List<ILayoutSlide> _list;
  private object _parent;

  internal LayoutSlides(object parent)
  {
    this._parent = parent;
    this._list = new List<ILayoutSlide>();
  }

  internal object Parent => this._parent;

  public int Count => this._list.Count;

  public ILayoutSlide this[int index] => this._list[index];

  public IEnumerator GetEnumerator() => (IEnumerator) this._list.GetEnumerator();

  IEnumerator<ILayoutSlide> IEnumerable<ILayoutSlide>.GetEnumerator()
  {
    return (IEnumerator<ILayoutSlide>) this._list.GetEnumerator();
  }

  public void Add(ILayoutSlide layoutSlide) => this._list.Add(layoutSlide);

  public ILayoutSlide Add(SlideLayoutType layoutType, string layoutName)
  {
    MasterSlide parent = (MasterSlide) this.Parent;
    string layoutId = Helper.GenerateLayoutId(parent.LayoutList, parent.MasterId);
    string relationIdentifier = Helper.GenerateRelationIdentifier(parent.TopRelation);
    LayoutSlide layoutSlide = new LayoutSlide(parent.Presentation, parent, layoutId.ToString());
    int number = int.Parse(relationIdentifier.Remove(0, 3));
    layoutSlide.TopRelation = layoutSlide.AddRelationToLayoutSlide(number, parent);
    layoutSlide.SetType(layoutType);
    layoutSlide.Name = layoutName;
    layoutSlide.AddShapesFromLayoutType(layoutType);
    parent.LayoutSlides.Add((ILayoutSlide) layoutSlide);
    parent.LayoutList.Add(relationIdentifier, layoutId.ToString());
    return (ILayoutSlide) layoutSlide;
  }

  public void Insert(int index, ILayoutSlide value) => this._list.Insert(index, value);

  public void RemoveAt(int index) => this.Remove(this._list[index]);

  public void Remove(ILayoutSlide value)
  {
    this.RemoveLayoutSlideRelations(value as LayoutSlide);
    this._list.Remove(value);
  }

  public int IndexOf(ILayoutSlide value) => this._list.IndexOf(value);

  public void Clear()
  {
    foreach (ILayoutSlide layoutSlide in this._list)
      this.RemoveLayoutSlideRelations(layoutSlide as LayoutSlide);
    this._list.Clear();
  }

  public ILayoutSlide GetByType(SlideLayoutType type)
  {
    List<ILayoutSlide> layoutSlidesByType = this.GetLayoutSlidesByType(type);
    return layoutSlidesByType.Count <= 0 ? (ILayoutSlide) null : layoutSlidesByType[0];
  }

  private void RemoveLayoutSlideRelations(LayoutSlide layoutSlide)
  {
    string layoutIndex = layoutSlide.Presentation.DataHolder.GetLayoutIndex(layoutSlide);
    MasterSlide masterSlide = layoutSlide.MasterSlide as MasterSlide;
    if (masterSlide.LayoutList.ContainsKey(layoutIndex))
      masterSlide.LayoutList.Remove(layoutIndex);
    string partName = "/" + Helper.FormatPathForZipArchive(masterSlide.TopRelation.GetItemPathByRelation(layoutIndex));
    masterSlide.Presentation.RemoveOverrideContentType(partName);
    masterSlide.TopRelation.RemoveRelation(layoutIndex);
  }

  private List<ILayoutSlide> GetLayoutSlidesByType(SlideLayoutType type)
  {
    List<ILayoutSlide> layoutSlidesByType = new List<ILayoutSlide>();
    for (int index = 0; index < this._list.Count; ++index)
    {
      if (this[index].LayoutType == type)
        layoutSlidesByType.Add(this[index]);
    }
    return layoutSlidesByType;
  }

  private List<ILayoutSlide> GetLayoutSlidesByName(string layoutName)
  {
    List<ILayoutSlide> layoutSlidesByName = new List<ILayoutSlide>();
    for (int index = 0; index < this._list.Count; ++index)
    {
      if ((this[index] as LayoutSlide).Name == layoutName)
        layoutSlidesByName.Add(this[index]);
    }
    return layoutSlidesByName;
  }

  private void FilterLayoutSlidesByPlaceHolderType(
    List<PlaceholderType> sourcePlaceHolderTypes,
    List<ILayoutSlide> list)
  {
    for (int index = 0; index < list.Count; ++index)
    {
      List<PlaceholderType> placeholderTypes = ((list[index] as LayoutSlide).Shapes as Shapes).GetPlaceholderTypes();
      if (!Helper.ComparePlaceholderType(sourcePlaceHolderTypes, placeholderTypes))
      {
        list.RemoveAt(index);
        --index;
      }
      placeholderTypes.Clear();
    }
  }

  internal ILayoutSlide GetEquivalentLayoutSlide(LayoutSlide sourceLayoutSlide)
  {
    ILayoutSlide equivalentLayoutSlide = (ILayoutSlide) null;
    List<ILayoutSlide> list = this.GetLayoutSlidesByType(sourceLayoutSlide.LayoutType);
    List<PlaceholderType> placeholderTypes = (sourceLayoutSlide.Shapes as Shapes).GetPlaceholderTypes();
    if (list.Count > 0)
    {
      this.FilterLayoutSlidesByPlaceHolderType(placeholderTypes, list);
      for (int index = 0; index < list.Count; ++index)
      {
        LayoutSlide layoutSlide = list[index] as LayoutSlide;
        if (layoutSlide.Name == sourceLayoutSlide.Name)
        {
          equivalentLayoutSlide = (ILayoutSlide) layoutSlide;
          break;
        }
      }
      if (equivalentLayoutSlide == null && list.Count > 0 && sourceLayoutSlide.LayoutType != SlideLayoutType.Custom)
        equivalentLayoutSlide = list[0];
    }
    else
    {
      list = this.GetLayoutSlidesByName(sourceLayoutSlide.Name);
      this.FilterLayoutSlidesByPlaceHolderType(placeholderTypes, list);
      equivalentLayoutSlide = list.Count > 0 ? list[0] : (ILayoutSlide) null;
    }
    placeholderTypes.Clear();
    list.Clear();
    return equivalentLayoutSlide;
  }

  internal void Add(LayoutSlide layoutSlide) => this._list.Add((ILayoutSlide) layoutSlide);

  internal void Close()
  {
    if (this._list != null)
    {
      foreach (BaseSlide baseSlide in this._list)
        baseSlide.Close();
      this._list.Clear();
      this._list = (List<ILayoutSlide>) null;
    }
    this._parent = (object) null;
  }

  public LayoutSlides Clone()
  {
    LayoutSlides layoutSlides = (LayoutSlides) this.MemberwiseClone();
    layoutSlides._list = this.CloneLayoutSlides();
    return layoutSlides;
  }

  private List<ILayoutSlide> CloneLayoutSlides()
  {
    List<ILayoutSlide> layoutSlideList = new List<ILayoutSlide>();
    foreach (LayoutSlide layoutSlide1 in this._list)
    {
      LayoutSlide layoutSlide2 = layoutSlide1.Clone();
      layoutSlideList.Add((ILayoutSlide) layoutSlide2);
    }
    return layoutSlideList;
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    this._parent = (object) presentation;
    foreach (LayoutSlide layoutSlide in this._list)
    {
      layoutSlide.SetParent(presentation);
      layoutSlide.SetMaster(layoutSlide.GetMasterSlide(presentation));
    }
  }

  internal void SetParent(MasterSlide masterSlide)
  {
    this._parent = (object) masterSlide.Presentation;
    foreach (LayoutSlide layoutSlide in this._list)
      layoutSlide.SetParent(masterSlide);
  }
}
