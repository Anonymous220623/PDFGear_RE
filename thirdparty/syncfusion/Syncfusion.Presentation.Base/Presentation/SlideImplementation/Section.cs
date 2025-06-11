// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideImplementation.Section
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.SlideImplementation;

internal class Section : ISection
{
  private Sections _sections;
  private string _name;
  private List<string> _indexList;
  private string _id;
  private ISlides _slides;

  internal Section(Sections sections)
  {
    this._sections = sections;
    this._slides = (ISlides) new Syncfusion.Presentation.SlideImplementation.Slides(this);
  }

  public string Name
  {
    get => this._name;
    set => this._name = value;
  }

  internal Sections Sections => this._sections;

  internal List<string> SlideIdList => this._indexList ?? (this._indexList = new List<string>());

  public int SlidesCount => this._indexList.Count;

  public ISlides Slides => this._slides;

  internal string ID
  {
    get => this._id;
    set => this._id = value;
  }

  public void InsertSlide(int slideIndex, ISlide slide)
  {
    if (slideIndex < 0 || slideIndex > this._sections.Presentation.Slides.Count - 1)
      throw new ArgumentOutOfRangeException("Index was out of range. Must be non-negative and less than the size of the collection.");
    this._sections.Presentation.Slides.Insert(slideIndex, slide);
    this.AddSlide(slide as Slide);
  }

  public ISlides Clone() => (ISlides) ((Syncfusion.Presentation.SlideImplementation.Slides) this._slides).Clone();

  public ISlide AddSlide(SlideLayoutType slideLayoutType)
  {
    Slide slide = (Slide) this._sections.Presentation.Slides.Add(slideLayoutType);
    this.AddSlide(slide);
    return (ISlide) slide;
  }

  public void Move(int toPosition)
  {
    if (toPosition <= 0 || toPosition > this._sections.Count)
      throw new ArgumentOutOfRangeException("Index was out of range. Must ranges from one and less than or equal to the size of the collection.");
    int index = this._sections.IndexOf(this);
    if (index == toPosition - 1)
      return;
    Section section1 = (Section) this._sections[index];
    Section section2 = (Section) this._sections[toPosition - 1];
    string slideId1 = section2.SlideIdList[0];
    Dictionary<string, string> newSlideIdList = new Dictionary<string, string>();
    Dictionary<string, string> dictionary = new Dictionary<string, string>(this._sections.Presentation.SlideList.Count);
    foreach (KeyValuePair<string, string> slide in this._sections.Presentation.SlideList)
      dictionary.Add(slide.Value, slide.Key);
    bool flag = true;
    foreach (KeyValuePair<string, string> keyValuePair in dictionary)
    {
      if (!section1.SlideIdList.Contains(keyValuePair.Key) && keyValuePair.Key != slideId1 && !newSlideIdList.ContainsKey(keyValuePair.Value))
        newSlideIdList.Add(keyValuePair.Value, keyValuePair.Key);
      else if (keyValuePair.Key == slideId1 && flag && toPosition == this._sections.Count)
      {
        foreach (string slideId2 in section2.SlideIdList)
          newSlideIdList.Add(dictionary[slideId2], slideId2);
        foreach (string slideId3 in section1.SlideIdList)
          newSlideIdList.Add(dictionary[slideId3], slideId3);
        flag = false;
      }
      else if (keyValuePair.Key == slideId1 && flag)
      {
        foreach (string slideId4 in section1.SlideIdList)
          newSlideIdList.Add(dictionary[slideId4], slideId4);
        if (!newSlideIdList.ContainsKey(keyValuePair.Value))
          newSlideIdList.Add(keyValuePair.Value, keyValuePair.Key);
        flag = false;
      }
    }
    if (newSlideIdList.Count == this._sections.Presentation.SlideList.Count)
      this._sections.Presentation.SetSlideListValue(newSlideIdList);
    this._sections.RemoveAt(index);
    if (toPosition > index)
      --toPosition;
    this._sections.Insert(toPosition, section1);
  }

  internal Section CloneSection()
  {
    Section section = (Section) this.MemberwiseClone();
    if (this._indexList != null)
      section._indexList = Helper.CloneList(this._indexList);
    section._slides = (ISlides) ((Syncfusion.Presentation.SlideImplementation.Slides) this._slides).Clone();
    return section;
  }

  internal void AddSlide(Slide slide)
  {
    slide.SectionId = this._id;
    this.SlideIdList.Add(slide.SlideID.ToString());
    (this._slides as Syncfusion.Presentation.SlideImplementation.Slides).AddSlide(slide);
  }

  internal void RemoveSlide(Slide slide)
  {
    slide.SectionId = (string) null;
    this.SlideIdList.Remove(slide.SlideID.ToString());
    (this._slides as Syncfusion.Presentation.SlideImplementation.Slides).RemoveSlide(slide);
  }

  internal void SetParent(Sections sections) => this._sections = sections;

  internal void Close()
  {
    if (this._indexList != null)
    {
      this._indexList.Clear();
      this._indexList = (List<string>) null;
    }
    ((Syncfusion.Presentation.SlideImplementation.Slides) this._slides).Close();
    this._slides = (ISlides) null;
    this._sections = (Sections) null;
  }
}
