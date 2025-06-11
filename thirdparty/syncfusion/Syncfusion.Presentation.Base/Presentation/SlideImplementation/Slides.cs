// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideImplementation.Slides
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.SlideImplementation;

internal class Slides : ISlides, IEnumerable<ISlide>, IEnumerable
{
  private List<ISlide> _list;
  private Syncfusion.Presentation.Presentation _presentation;
  private Section _sectionParent;

  internal Slides(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
    this._list = new List<ISlide>();
  }

  internal Slides(Section section)
  {
    this._sectionParent = section;
    this._presentation = section.Sections.Presentation;
    this._list = new List<ISlide>();
  }

  public int Count => this._list.Count;

  public ISlide this[int index]
  {
    get
    {
      if (this._list.Count <= index)
        throw new IndexOutOfRangeException("Index was out of range,value should be greater than slide count");
      return this._list[index];
    }
  }

  internal bool IsSectionParent => this._sectionParent != null;

  public IEnumerator<ISlide> GetEnumerator() => (IEnumerator<ISlide>) this._list.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._list.GetEnumerator();

  public ISlide Add()
  {
    if (this.IsSectionParent)
    {
      ISlide slide = this._sectionParent.Sections.Presentation.Slides.Add();
      this._sectionParent.AddSlide(slide as Slide);
      return slide;
    }
    Slide slide1 = new Slide(this._presentation, Helper.GenerateSlideId(this._presentation.SlideList));
    int count = this._presentation.SlideList.Count;
    SlideLayoutType ppSlideLayout = SlideLayoutType.Title;
    if (count != 0)
    {
      Slide slide2 = this._presentation.Slides[count - 1] as Slide;
      char[] charArray = Helper.GetFileNameWithoutExtension(slide2.LayoutTarget).ToCharArray();
      int num1 = int.Parse(charArray[charArray.Length - 1].ToString());
      if (count == 1)
      {
        int num2;
        slide1.AddRelationToSlide(num2 = num1 + 1);
        this._presentation.DataHolder.GetSlideLayout(slide1);
        ppSlideLayout = slide1.LayoutSlide.LayoutType;
      }
      else
      {
        slide1.AddRelationToSlide(num1);
        ppSlideLayout = slide2.LayoutSlide.LayoutType;
      }
    }
    slide1.AddShapesFromLayout(ppSlideLayout, slide1);
    this.Add(slide1);
    return (ISlide) slide1;
  }

  public ISlide Add(ILayoutSlide layoutSlide)
  {
    if (this.IsSectionParent)
    {
      ISlide slide = this._sectionParent.Sections.Presentation.Slides.Add(layoutSlide);
      this._sectionParent.AddSlide(slide as Slide);
      return slide;
    }
    Slide slide1 = new Slide(this._presentation, Helper.GenerateSlideId(this._presentation.SlideList));
    foreach (IShape shape in (IEnumerable<ISlideItem>) layoutSlide.Shapes)
    {
      if (shape.PlaceholderFormat != null)
        slide1.AddShapesFromPlaceholderType(shape, layoutSlide);
    }
    foreach (KeyValuePair<string, string> layout in ((MasterSlide) layoutSlide.MasterSlide).LayoutList)
    {
      if (layout.Value == ((LayoutSlide) layoutSlide).LayoutId)
      {
        string[] strArray = ((BaseSlide) layoutSlide.MasterSlide).TopRelation.GetTargetByRelationId(layout.Key).Split('/');
        int int32 = Convert.ToInt32(strArray[2].Substring(strArray[2].IndexOf('.') - 1, 1));
        slide1.AddRelationToSlide(int32);
        break;
      }
    }
    this.Add(slide1);
    return (ISlide) slide1;
  }

  internal void AddPreservedElements(Syncfusion.Presentation.Presentation presentation)
  {
    if (presentation.PreservedElements == null)
      return;
    foreach (string key in presentation.PreservedElements.Keys)
    {
      if (!this._presentation.PreservedElements.ContainsKey(key))
      {
        Stream preservedElement = presentation.PreservedElements[key];
        Stream stream = (Stream) new MemoryStream();
        byte[] buffer = new byte[preservedElement.Length];
        preservedElement.Position = 0L;
        int count;
        while ((count = preservedElement.Read(buffer, 0, buffer.Length)) > 0)
          stream.Write(buffer, 0, count);
        this._presentation.PreservedElements.Add(key, stream);
      }
    }
  }

  public void Add(ISlide clonedSlide, PasteOptions pasteOptions, IPresentation sourcePresentation)
  {
    if (this.IsSectionParent)
    {
      this._sectionParent.Sections.Presentation.Slides.Add(clonedSlide, pasteOptions, sourcePresentation);
      this._sectionParent.AddSlide(clonedSlide as Slide);
    }
    else
    {
      Slide slide = (Slide) clonedSlide;
      Syncfusion.Presentation.Presentation presentation = (Syncfusion.Presentation.Presentation) sourcePresentation;
      presentation.IsSourcePresentation = true;
      if (this._presentation != sourcePresentation)
      {
        switch (pasteOptions)
        {
          case PasteOptions.UseDestinationTheme:
            this.ApplyDestinationThemeToPresentation(slide, presentation);
            slide.Presentation.DataHolder.SetBackgroundColor(slide);
            break;
          case PasteOptions.SourceFormatting:
            bool isMerged = false;
            MasterSlide masterFromSlideClone = this.GetSourceMasterFromSlideClone(slide, presentation, ref isMerged);
            this._presentation.DataHolder.AddLayoutsToArchive(presentation, masterFromSlideClone, slide, isMerged);
            if (!isMerged || this._presentation.MasterList.Count == 1)
            {
              string idsAndGetMasterId = this.GenerateLayoutIdsAndGetMasterId(masterFromSlideClone);
              string relationId = Helper.GenerateRelationId(this._presentation.TopRelation);
              this._presentation.TopRelation.Add(relationId, new Relation(relationId, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/slideMaster", $"slideMasters/slideMaster{(object) (this._presentation.MasterList.Count + 1)}.xml", (string) null));
              this._presentation.MasterList.Add(relationId, idsAndGetMasterId);
              MasterSlide masterSlide = (MasterSlide) slide.GetLayoutSlideFromPresentation(presentation).MasterSlide;
              masterSlide.MergedMasterId = idsAndGetMasterId;
              masterFromSlideClone.MergedMasterId = idsAndGetMasterId;
              masterFromSlideClone.MasterId = idsAndGetMasterId;
              this.AllocateSourceTheme(masterFromSlideClone);
              this.MergeSourceLayoutsToCurrentPresentation(presentation, masterFromSlideClone);
              this.AddPreservedElements(presentation);
              slide.SetParent(this._presentation);
              if (slide.NotesSlide != null)
                slide.InitializeNotesMasterSlide();
              this._presentation.Masters.Add((IMasterSlide) masterFromSlideClone);
              masterSlide.Merged = true;
              break;
            }
            break;
        }
      }
      this.UpdateSmartArtItems(slide);
      this.Add(slide);
    }
  }

  private void UpdateSmartArtItems(Slide slideClone)
  {
    int dataCountFromSlides = this.GenerateDataCountFromSlides();
    foreach (Relation relation in slideClone.TopRelation.GetRelationList())
    {
      if (relation.Type.Equals("http://schemas.openxmlformats.org/officeDocument/2006/relationships/diagramData"))
      {
        relation.Target = $"../diagrams/data{dataCountFromSlides.ToString()}.xml";
        this._presentation.AddOverrideContentType($"/ppt/diagrams/data{dataCountFromSlides.ToString()}.xml", "application/vnd.openxmlformats-officedocument.drawingml.diagramData+xml");
        ++dataCountFromSlides;
      }
    }
  }

  private void UpdateOLEItems(Slide slideClone)
  {
    int vmlCountFromSlides = this.GenerateVMLCountFromSlides();
    foreach (Relation relation in slideClone.TopRelation.GetRelationList())
    {
      if (relation.Type.Equals("http://schemas.openxmlformats.org/officeDocument/2006/relationships/vmlDrawing"))
      {
        relation.Target = $"../diagrams/vmlDrawing{vmlCountFromSlides.ToString()}.xml";
        this._presentation.AddDefaultContentType("vml", "application/vnd.openxmlformats-officedocument.vmlDrawing");
        ++vmlCountFromSlides;
      }
    }
  }

  private int GenerateVMLCountFromSlides()
  {
    List<string> stringList = new List<string>();
    foreach (BaseSlide slide in (IEnumerable<ISlide>) this._presentation.Slides)
    {
      foreach (Relation relation in slide.TopRelation.GetRelationList())
      {
        if (relation.Type.Equals("http://schemas.openxmlformats.org/officeDocument/2006/relationships/vmlDrawing"))
          stringList.Add(Helper.GetFileNameWithoutExtension(relation.Target));
      }
    }
    if (stringList.Count > 0)
    {
      for (int vmlCountFromSlides = 1; vmlCountFromSlides < int.MaxValue; vmlCountFromSlides = vmlCountFromSlides + 1 + 1)
      {
        if (!stringList.Contains("data" + vmlCountFromSlides.ToString()))
          return vmlCountFromSlides;
      }
    }
    return 1;
  }

  private int GenerateDataCountFromSlides()
  {
    List<string> stringList = new List<string>();
    foreach (BaseSlide slide in (IEnumerable<ISlide>) this._presentation.Slides)
    {
      foreach (Relation relation in slide.TopRelation.GetRelationList())
      {
        if (relation.Type.Equals("http://schemas.openxmlformats.org/officeDocument/2006/relationships/diagramData"))
          stringList.Add(Helper.GetFileNameWithoutExtension(relation.Target));
      }
    }
    if (stringList.Count > 0)
    {
      for (int dataCountFromSlides = 1; dataCountFromSlides < int.MaxValue; dataCountFromSlides = dataCountFromSlides + 1 + 1)
      {
        if (!stringList.Contains("data" + dataCountFromSlides.ToString()))
          return dataCountFromSlides;
      }
    }
    return 1;
  }

  private void MergeSourceLayoutsToCurrentPresentation(
    Syncfusion.Presentation.Presentation presentation,
    MasterSlide masterSlide)
  {
    foreach (LayoutSlide layoutSlide in (IEnumerable<ILayoutSlide>) masterSlide.LayoutSlides)
    {
      if (this._presentation.MasterList.ContainsValue(masterSlide.MergedMasterId))
      {
        foreach (Relation relation in layoutSlide.TopRelation.GetRelationList())
        {
          string target = relation.Target;
          if (target.Contains("slideMasters"))
            relation.Target = $"../slideMasters/slideMaster{(this._presentation.Masters.Count + 1).ToString()}.xml";
          if (this.NeedToRemoveRelation(target) && !relation.Type.Contains("hyperlink"))
            layoutSlide.TopRelation.RemoveRelation(relation.Id);
        }
        this._presentation.DataHolder.GetLayoutIndex(presentation, layoutSlide);
      }
    }
  }

  private void AllocateSourceTheme(MasterSlide masterSlide)
  {
    Relation relationByContentType = masterSlide.TopRelation.GetRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/theme");
    int num1 = 0;
    if (!this._presentation.Created)
      num1 = this._presentation.DataHolder.GetUnParsedThemeCount();
    int num2 = 0;
    if (this._presentation.NotesList != null)
      num2 = this._presentation.NotesList.Count;
    if (this._presentation.HandoutList != null)
      num2 += this._presentation.HandoutList.Count;
    int num3 = this._presentation.Masters.Count + num2 + num1;
    int num4;
    relationByContentType.Target = $"../theme/theme{(num4 = num3 + 1).ToString()}.xml";
    this._presentation.AddOverrideContentType($"/ppt/theme/theme{num4.ToString()}.xml", "application/vnd.openxmlformats-officedocument.theme+xml");
  }

  private string GenerateLayoutIdsAndGetMasterId(MasterSlide masterSlide)
  {
    int count = this._presentation.MasterList.Count;
    long num1 = 0;
    foreach (MasterSlide master in (IEnumerable<IMasterSlide>) this._presentation.Masters)
    {
      if (master.LayoutList != null)
      {
        count += master.LayoutList.Count;
        string[] strArray = new string[master.LayoutList.Count];
        master.LayoutList.Values.CopyTo(strArray, 0);
        long num2 = this.SortList(new List<string>((IEnumerable<string>) strArray));
        if (num2 > num1)
          num1 = num2;
      }
    }
    int num3 = count - this._presentation.MasterList.Count;
    masterSlide.OldLayoutList = Helper.CloneDictionary(masterSlide.LayoutList);
    masterSlide.LayoutList.Clear();
    long num4 = num1 + 1L;
    int num5 = masterSlide.TopRelation.RemoveLayoutRelation();
    for (int index = 0; index < num5; ++index)
    {
      string relationId = Helper.GenerateRelationId(masterSlide.TopRelation);
      masterSlide.TopRelation.Add(relationId, new Relation(relationId, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/slideLayout", $"../slideLayouts/slideLayout{(++num3).ToString()}.xml", (string) null));
      masterSlide.LayoutList.Add(relationId, (++num4).ToString());
    }
    this.SortLayoutList(masterSlide.LayoutList);
    int num6 = count + masterSlide.LayoutList.Count + 1;
    long num7;
    return (num7 = num4 + 1L).ToString();
  }

  private MasterSlide GetSourceMasterFromSlideClone(
    Slide slideClone,
    Syncfusion.Presentation.Presentation presentation,
    ref bool isMerged)
  {
    MasterSlide masterSlide1 = (MasterSlide) slideClone.GetLayoutSlideFromPresentation(presentation).MasterSlide;
    if (!masterSlide1.Merged)
    {
      masterSlide1.Merged = true;
    }
    else
    {
      isMerged = true;
      foreach (IMasterSlide master in (IEnumerable<IMasterSlide>) this._presentation.Masters)
      {
        if (masterSlide1.MergedMasterId == ((MasterSlide) master).MergedMasterId)
          return master as MasterSlide;
      }
    }
    MasterSlide masterSlide2 = masterSlide1.Clone();
    masterSlide2.SetParent(this._presentation);
    ((LayoutSlides) masterSlide2.LayoutSlides).SetParent(masterSlide2);
    return masterSlide2;
  }

  private void ApplyDestinationThemeToPresentation(Slide slideClone, Syncfusion.Presentation.Presentation presentation)
  {
    if (!this._presentation.IsSourcePresentation)
      this._presentation.DataHolder.AddSlideContentToArchive(slideClone, presentation);
    int masterLayoutCount = this.GetTotalMasterLayoutCount(this._presentation);
    foreach (Relation relation in slideClone.TopRelation.GetRelationList())
    {
      if (relation.Target.Contains("slideLayout"))
      {
        int num1 = int.Parse(Helper.GetFileNameWithoutExtension(relation.Target).Remove(0, 11));
        if (slideClone.Presentation != null && slideClone.Presentation.Masters != null && slideClone.LayoutSlide != null && this._presentation.Masters.Count > 0)
        {
          MasterSlide master = this._presentation.Masters[0] as MasterSlide;
          if ((master.LayoutSlides as LayoutSlides).GetEquivalentLayoutSlide(slideClone.LayoutSlide as LayoutSlide) is LayoutSlide equivalentLayoutSlide)
          {
            string layoutId = equivalentLayoutSlide.LayoutId;
            string keyFromDictionary = Helper.GetKeyFromDictionary(master.LayoutList, layoutId);
            if (keyFromDictionary != string.Empty)
            {
              int num2 = int.Parse(Helper.GetFileNameWithoutExtension((this._presentation.Masters[0] as MasterSlide).TopRelation.GetItemPathByRelation(keyFromDictionary)).Remove(0, 11));
              if (num1 != num2 && num2 <= masterLayoutCount)
              {
                relation.Target = $"../slideLayouts/slideLayout{(object) num2}.xml";
                break;
              }
              if (num1 > masterLayoutCount)
              {
                relation.Target = "../slideLayouts/slideLayout1.xml";
                break;
              }
              break;
            }
            if (num1 > masterLayoutCount)
            {
              relation.Target = "../slideLayouts/slideLayout1.xml";
              break;
            }
            break;
          }
          if (this._presentation.Masters.Count == 1)
          {
            int num3 = int.Parse(Helper.GenerateRelationIdentifier(master.TopRelation).Remove(0, 3));
            (master.LayoutSlides.Add(SlideLayoutType.Blank, slideClone.LayoutSlide.Name) as LayoutSlide).AddShapesFromSourceLayout(slideClone.LayoutSlide as LayoutSlide);
            relation.Target = $"../slideLayouts/slideLayout{(object) num3}.xml";
            break;
          }
          relation.Target = "../slideLayouts/slideLayout1.xml";
          break;
        }
        if (num1 > masterLayoutCount)
        {
          relation.Target = "../slideLayouts/slideLayout1.xml";
          break;
        }
        break;
      }
      if (relation.Target.Contains("comment"))
        this._presentation.DataHolder.AddCommentArchiveItem(relation, presentation.DataHolder.Archive);
    }
    this.AddPreservedElements(presentation);
    slideClone.SetParent(this._presentation);
    this.UpdateSmartArtItems(slideClone);
  }

  private bool NeedToRemoveRelation(string target)
  {
    return !target.Contains("slideMaster") && !target.Contains("image") && !target.Contains("hdphoto") && !target.Contains("themeOverride") && !target.Contains("ink") && !target.Contains("oleObject") && !target.Contains("vmlDrawing") && !target.Contains("printerSettings") && !target.Contains("tag");
  }

  private int GetTotalMasterLayoutCount(Syncfusion.Presentation.Presentation presentation)
  {
    int masterLayoutCount = 0;
    if (presentation.Masters.Count != 0)
    {
      foreach (IMasterSlide master in (IEnumerable<IMasterSlide>) presentation.Masters)
        masterLayoutCount += ((MasterSlide) master).LayoutList.Count;
    }
    return masterLayoutCount;
  }

  private long SortList(List<string> list)
  {
    long num1 = 0;
    foreach (string s in list)
    {
      if (s != null)
      {
        long num2 = long.Parse(s);
        if (num1 < num2)
          num1 = num2;
      }
    }
    return num1;
  }

  private void SortLayoutList(Dictionary<string, string> layoutList)
  {
    SortedDictionary<int, string> sortedDictionary = new SortedDictionary<int, string>();
    foreach (KeyValuePair<string, string> layout in layoutList)
    {
      int key = int.Parse(layout.Key.Remove(0, 3));
      sortedDictionary.Add(key, layout.Value);
    }
    layoutList.Clear();
    foreach (KeyValuePair<int, string> keyValuePair in sortedDictionary)
      layoutList.Add("rId" + keyValuePair.Key.ToString(), keyValuePair.Value);
  }

  public int Add(ISlide value)
  {
    if (this.IsSectionParent)
    {
      Slide slide = this._sectionParent.Sections.Presentation.Slides[this._sectionParent.Sections.Presentation.Slides.Add(value)] as Slide;
      this._sectionParent.AddSlide(slide);
      return this._sectionParent.Slides.IndexOf((ISlide) slide);
    }
    Slide clonedSlide = (Slide) value;
    if (clonedSlide.Presentation != null)
    {
      clonedSlide = clonedSlide.Clone() as Slide;
      this.Add((ISlide) clonedSlide, PasteOptions.UseDestinationTheme, (IPresentation) clonedSlide.Presentation);
    }
    else
      this.Add(clonedSlide);
    return this._list.IndexOf((ISlide) clonedSlide);
  }

  internal void Add(Slide value)
  {
    this.UpdateSlideList(this._presentation.Slides.Count, value);
    this._list.Add((ISlide) value);
  }

  public void Insert(int index, ISlide value)
  {
    if (this.IsSectionParent)
    {
      this._sectionParent.Sections.Presentation.Slides.Insert(index, value);
      this._sectionParent.AddSlide(value as Slide);
    }
    else
    {
      Slide slide = value as Slide;
      if (slide.Presentation == null || slide.Presentation != this._presentation)
      {
        this.UpdateSlideList(index, slide);
        this.ApplyDestinationThemeToPresentation((Slide) value, this._presentation);
        slide.Presentation.DataHolder.GetSlideLayout(slide);
      }
      else
        this.UpdateSlideList(index, slide);
      this._list.Insert(index, value);
    }
  }

  public void Insert(
    int index,
    ISlide clonedSlide,
    PasteOptions pasteOptions,
    IPresentation sourcePresentation)
  {
    if (this.IsSectionParent)
    {
      this._sectionParent.Sections.Presentation.Slides.Insert(index, clonedSlide, pasteOptions, sourcePresentation);
      this._sectionParent.AddSlide(clonedSlide as Slide);
    }
    else
    {
      Slide slide = (Slide) clonedSlide;
      Syncfusion.Presentation.Presentation presentation = (Syncfusion.Presentation.Presentation) sourcePresentation;
      presentation.IsSourcePresentation = true;
      switch (pasteOptions)
      {
        case PasteOptions.UseDestinationTheme:
          if (this._presentation != sourcePresentation)
          {
            this.UpdateSlideList(index, slide);
            this.ApplyDestinationThemeToPresentation(slide, presentation);
            slide.Presentation.DataHolder.SetBackgroundColor(slide);
            break;
          }
          this.UpdateSlideList(index, slide);
          this.UpdateSmartArtItems(slide);
          break;
        case PasteOptions.SourceFormatting:
          if (this._presentation != sourcePresentation)
          {
            bool isMerged = false;
            MasterSlide masterFromSlideClone = this.GetSourceMasterFromSlideClone(slide, presentation, ref isMerged);
            this._presentation.DataHolder.AddLayoutsToArchive(presentation, masterFromSlideClone, slide, isMerged);
            if (!isMerged || this._presentation.MasterList.Count == 1)
            {
              string idsAndGetMasterId = this.GenerateLayoutIdsAndGetMasterId(masterFromSlideClone);
              string relationId = Helper.GenerateRelationId(this._presentation.TopRelation);
              this._presentation.TopRelation.Add(relationId, new Relation(relationId, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/slideMaster", $"slideMasters/slideMaster{(object) (this._presentation.MasterList.Count + 1)}.xml", (string) null));
              this._presentation.MasterList.Add(relationId, idsAndGetMasterId);
              MasterSlide masterSlide = (MasterSlide) slide.GetLayoutSlideFromPresentation(presentation).MasterSlide;
              masterSlide.MergedMasterId = idsAndGetMasterId;
              masterFromSlideClone.MergedMasterId = idsAndGetMasterId;
              masterFromSlideClone.MasterId = idsAndGetMasterId;
              this.AllocateSourceTheme(masterFromSlideClone);
              this.MergeSourceLayoutsToCurrentPresentation(presentation, masterFromSlideClone);
              this.AddPreservedElements(presentation);
              slide.SetParent(this._presentation);
              if (slide.NotesSlide != null)
                slide.InitializeNotesMasterSlide();
              this._presentation.Masters.Add((IMasterSlide) masterFromSlideClone);
              masterSlide.Merged = true;
              this.UpdateSmartArtItems(slide);
              this.UpdateSlideList(index, slide);
              break;
            }
            this.UpdateSlideList(index, slide);
            slide.SetParent(this._presentation);
            this.UpdateSmartArtItems(slide);
            break;
          }
          this.UpdateSlideList(index, slide);
          this.UpdateSmartArtItems(slide);
          break;
      }
      this._list.Insert(index, (ISlide) slide);
    }
  }

  private void UpdateSlideList(int index, Slide slide)
  {
    string slideId = Helper.GenerateSlideId(this._presentation.SlideList);
    slide.SlideID = Helper.ToUInt(slideId);
    string relationId = Helper.GenerateRelationId(this._presentation.TopRelation);
    string relationTarget = Helper.GenerateRelationTarget(this._presentation);
    string[] strArray = relationTarget.Split('/');
    string key1 = strArray[strArray.Length - 1];
    if (this._presentation.SlidesFromInputFile.ContainsKey(key1))
      this._presentation.SlidesFromInputFile[key1] = (ISlide) slide;
    else
      this._presentation.SlidesFromInputFile.Add(key1, (ISlide) slide);
    this._presentation.TopRelation.Add(relationId, new Relation(relationId, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/slide", relationTarget, (string) null));
    Dictionary<string, string> dictionary = new Dictionary<string, string>();
    foreach (string key2 in this._presentation.SlideList.Keys)
    {
      if (dictionary.Count == index)
        dictionary.Add(relationId, slideId);
      dictionary.Add(key2, this._presentation.SlideList[key2]);
    }
    if (!dictionary.ContainsKey(relationId))
      dictionary.Add(relationId, slideId);
    this._presentation.SlideList.Clear();
    this._presentation.SlideList = dictionary;
    slide.Presentation = this._presentation;
    slide.Presentation.DataHolder.GetSlideLayout(slide);
    this.UpdateNotesSlideRelation(slide, relationTarget);
  }

  internal void UpdateNotesSlideRelation(Slide slide, string slidePath)
  {
    if (!(slide.NotesSlide is NotesSlide notesSlide))
      return;
    Relation relationByContentType = notesSlide.TopRelation.GetRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/slide");
    if (relationByContentType != null)
    {
      Relation relation = relationByContentType.Clone();
      relation.Target = "../" + slidePath;
      notesSlide.TopRelation.Update(relation);
    }
    string str = $"../notesSlides/notesSlide{(++this._presentation.NotesSlideCount).ToString()}.xml";
    slide.TopRelation.GetRelationByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/notesSlide").Target = str;
    slide.InitializeNotesMasterSlide();
    this._presentation.AddOverrideContentType("/" + Helper.FormatPathForZipArchive(slide.TopRelation.GetItemPathByKeyword("notesSlide")), "application/vnd.openxmlformats-officedocument.presentationml.notesSlide+xml");
  }

  public void RemoveAt(int index) => this.Remove(this._list[index]);

  public void Remove(ISlide value)
  {
    if (this.IsSectionParent)
    {
      this._sectionParent.Sections.Presentation.Slides.Remove(value);
      this._sectionParent.RemoveSlide(value as Slide);
    }
    else
    {
      Slide slide1 = (Slide) value;
      string str = (string) null;
      foreach (KeyValuePair<string, string> slide2 in slide1.Presentation.SlideList)
      {
        if (slide2.Value == slide1.SlideID.ToString())
        {
          str = slide2.Key;
          break;
        }
      }
      string partName = "/ppt/" + slide1.Presentation.TopRelation.GetItemPathByRelation(str);
      slide1.Presentation.RemoveOverrideContentType(partName);
      slide1.Presentation.TopRelation.RemoveRelation(str);
      slide1.Presentation.SlideList.Remove(str);
      slide1.TopRelation.Clear();
      foreach (Section section in ((Sections) slide1.Presentation.Sections).GetSectionList())
      {
        if (section.SlideIdList.Contains(slide1.SlideID.ToString()))
        {
          section.SlideIdList.Remove(slide1.SlideID.ToString());
          break;
        }
      }
      this._list.Remove(value);
    }
  }

  public int IndexOf(ISlide value) => this._list.IndexOf(value);

  public void Clear()
  {
    while (this._list.Count != 0)
      this.Remove(this._list[0]);
  }

  public ISlide Add(SlideLayoutType ppSlideLayout)
  {
    if (this.IsSectionParent)
    {
      ISlide slide = this._sectionParent.Sections.Presentation.Slides.Add(ppSlideLayout);
      this._sectionParent.AddSlide(slide as Slide);
      return slide;
    }
    if (this._sectionParent != null)
      return this._sectionParent.AddSlide(ppSlideLayout);
    Slide slide1 = new Slide(this._presentation, Helper.GenerateSlideId(this._presentation.SlideList));
    slide1.AddShapesFromLayout(ppSlideLayout, slide1);
    this.Add(slide1);
    return (ISlide) slide1;
  }

  internal void AddSlide(Slide slide) => this._list.Add((ISlide) slide);

  internal void RemoveSlide(Slide slide) => this._list.Remove((ISlide) slide);

  internal void Close() => this.ClearAll();

  private void ClearAll()
  {
    if (this._list != null)
    {
      foreach (BaseSlide baseSlide in this._list)
        baseSlide.Close();
      this._list.Clear();
      this._list = (List<ISlide>) null;
    }
    this._presentation = (Syncfusion.Presentation.Presentation) null;
  }

  public Slides Clone()
  {
    Slides slides = (Slides) this.MemberwiseClone();
    slides._list = this.CloneSlideList();
    return slides;
  }

  private List<ISlide> CloneSlideList()
  {
    List<ISlide> slideList = new List<ISlide>();
    foreach (ISlide slide1 in this._list)
    {
      Slide slide2 = slide1.Clone() as Slide;
      slideList.Add((ISlide) slide2);
    }
    return slideList;
  }

  internal void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
    foreach (BaseSlide baseSlide in this._list)
      baseSlide.SetParent(presentation);
  }
}
