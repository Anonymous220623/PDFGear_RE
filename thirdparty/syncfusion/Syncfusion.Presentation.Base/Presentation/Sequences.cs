// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Sequences
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation;

internal class Sequences : ISequences, IEnumerable<ISequence>, IEnumerable
{
  private List<ISequence> sequenceList = new List<ISequence>();
  private BaseSlide baseSlide;

  internal Sequences(BaseSlide slide) => this.baseSlide = slide;

  public ISequence this[int index]
  {
    get
    {
      if (this.sequenceList.Count <= index)
        throw new IndexOutOfRangeException("Index was out of range, value should be less than Sequences count");
      return this.sequenceList[index];
    }
  }

  public int Count => this.sequenceList.Count;

  public ISequence Add(IShape shapeTrigger)
  {
    ISequence sequence = (ISequence) new Sequence(this.baseSlide);
    sequence.TriggerShape = shapeTrigger;
    this.sequenceList.Add(sequence);
    return sequence;
  }

  public void Clear()
  {
    if ((this.baseSlide.Timeline as Timeline).GetMainSequence().Count == 0)
      this.baseSlide.IsAnimated = false;
    this.sequenceList.Clear();
  }

  public void Remove(ISequence item)
  {
    if (item == null)
      return;
    this.sequenceList.Remove(item);
  }

  public void RemoveAt(int index)
  {
    if (index >= this.sequenceList.Count)
      throw new Exception("Index must be less than Sequences count");
    this.sequenceList.RemoveAt(index);
  }

  public IEnumerator<ISequence> GetEnumerator()
  {
    return (IEnumerator<ISequence>) this.sequenceList.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.sequenceList.GetEnumerator();

  internal void Add(Sequence sequence) => this.sequenceList.Add((ISequence) sequence);

  internal Sequences Clone(BaseSlide newBaseSlide)
  {
    Sequences sequences = (Sequences) this.MemberwiseClone();
    sequences.baseSlide = newBaseSlide;
    sequences.sequenceList = this.CloneSequenceList(newBaseSlide);
    return sequences;
  }

  private List<ISequence> CloneSequenceList(BaseSlide newBaseSlide)
  {
    List<ISequence> sequenceList = new List<ISequence>();
    foreach (Sequence sequence1 in this.sequenceList)
    {
      Sequence sequence2 = sequence1.Clone(newBaseSlide);
      sequenceList.Add((ISequence) sequence2);
    }
    return sequenceList;
  }
}
