// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideImplementation.Sections
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.SlideImplementation;

internal class Sections : ISections, IEnumerable<ISection>, IEnumerable
{
  private Syncfusion.Presentation.Presentation _presentation;
  private List<ISection> _sections;

  internal Sections(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
    this._sections = new List<ISection>();
  }

  internal void Add(Section section) => this._sections.Add((ISection) section);

  public ISection Add()
  {
    Section section = new Section(this);
    section.Name = "";
    section.ID = Serializator.GetGuidString(Guid.NewGuid());
    this._sections.Add((ISection) section);
    return (ISection) section;
  }

  internal Syncfusion.Presentation.Presentation Presentation => this._presentation;

  public ISection this[int index]
  {
    get
    {
      if (index < 0 || index >= this._sections.Count)
        throw new IndexOutOfRangeException("Index was out of range. Must be non-negative and less than the size of the collection.");
      return this._sections[index];
    }
  }

  public IEnumerator<ISection> GetEnumerator()
  {
    return (IEnumerator<ISection>) this._sections.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._sections.GetEnumerator();

  internal int AddSection(string sectionName)
  {
    Section section = new Section(this);
    section.Name = sectionName;
    section.ID = Serializator.GetGuidString(Guid.NewGuid());
    this._sections.Add((ISection) section);
    return this._sections.IndexOf((ISection) section);
  }

  public void Clear() => this._sections.Clear();

  public void Insert(int index, ISection section)
  {
    if (index < 0 || index > this._sections.Count)
      throw new ArgumentOutOfRangeException("Index was out of range. Must be non-negative and less than the size of the collection.");
    this._sections.Insert(index, section);
  }

  public int Count => this._sections.Count;

  internal void Delete(int sectionIndex, bool deleteSlides)
  {
    if (deleteSlides)
    {
      foreach (string slideId in ((Section) this._sections[sectionIndex]).SlideIdList)
      {
        foreach (Slide slide in (IEnumerable<ISlide>) this._presentation.Slides)
        {
          if ((int) slide.SlideID == (int) Convert.ToUInt32(slideId))
          {
            this._presentation.Slides.Remove((ISlide) slide);
            break;
          }
        }
      }
    }
    this._sections.RemoveAt(sectionIndex);
  }

  internal ISection GetSectionByID(string sectionId)
  {
    foreach (ISection section in this._sections)
    {
      if (((Section) section).ID == sectionId)
        return section;
    }
    return (ISection) null;
  }

  internal List<ISection> GetSectionList() => this._sections;

  public void Remove(ISection section) => this._sections.Remove(section);

  public void RemoveAt(int index)
  {
    if (index < 0 || index >= this._sections.Count)
      throw new ArgumentOutOfRangeException("Index was out of range. Must be non-negative and less than the size of the collection.");
    this._sections.RemoveAt(index);
  }

  internal void Insert(int toPosition, Section section)
  {
    this._sections.Insert(toPosition, (ISection) section);
  }

  internal int IndexOf(Section section) => this._sections.IndexOf((ISection) section);

  public Sections Clone()
  {
    Sections sections = (Sections) this.MemberwiseClone();
    sections._sections = this.CloneSections();
    return sections;
  }

  private List<ISection> CloneSections()
  {
    List<ISection> sectionList = new List<ISection>();
    foreach (Section section1 in this._sections)
    {
      Section section2 = section1.CloneSection();
      sectionList.Add((ISection) section2);
    }
    return sectionList;
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
    foreach (Section section in this._sections)
      section.SetParent(this);
  }

  internal void Close()
  {
    this.ClearSectionList();
    this._presentation = (Syncfusion.Presentation.Presentation) null;
  }

  private void ClearSectionList()
  {
    foreach (Section section in this._sections)
      section.Close();
    this._sections.Clear();
    this._sections = (List<ISection>) null;
  }
}
