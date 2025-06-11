// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideImplementation.BaseSlide
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Animation;
using Syncfusion.Presentation.Animation.Internal;
using Syncfusion.Presentation.Charts;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.Interfaces;
using Syncfusion.Presentation.SlideTransition;
using Syncfusion.Presentation.SlideTransition.Internal;
using Syncfusion.Presentation.Themes;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Presentation.SlideImplementation;

internal class BaseSlide : IBaseSlide
{
  private Syncfusion.Presentation.Background _background;
  private string _name;
  private Syncfusion.Presentation.Presentation _presentation;
  private Syncfusion.Presentation.Drawing.Shapes _shapeCollection;
  private RelationCollection _topRelation;
  private bool _isDisposed;
  private bool _isColorMapChanged;
  private Dictionary<string, Stream> _bgPreserve;
  internal Dictionary<string, string> ColorMap;
  private IAnimationTimeline timeLine;
  private InternaTimeLine timing;
  private bool isAnimated;
  private ISlideShowTransition slideTransition;
  private InternalSlideTransition internal_SlideTransition;
  private bool isAlternateContent;
  private bool isSlideTransition;
  private ushort _lastShapeId;
  private bool? _hasOLEObject;
  private RelationCollection _vmlRelation;
  private IHeadersFooters _headersFooters;

  internal BaseSlide(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
    this._bgPreserve = new Dictionary<string, Stream>();
    this.ColorMap = new Dictionary<string, string>();
    this.timeLine = (IAnimationTimeline) new Syncfusion.Presentation.Timeline(this);
    this.timing = new InternaTimeLine(this);
    this.slideTransition = (ISlideShowTransition) new SlideShowTransition(this);
    this.internal_SlideTransition = new InternalSlideTransition();
    this._lastShapeId = (ushort) 1;
    this._headersFooters = (IHeadersFooters) new Syncfusion.Presentation.HeaderFooter.HeadersFooters((IBaseSlide) this);
  }

  public IHeadersFooters HeadersFooters => this._headersFooters;

  public ISlideSize SlideSize => this._presentation.SlideSize;

  internal InternaTimeLine Timing
  {
    get => this.timing;
    set => this.timing = value;
  }

  internal ushort LastShapeId
  {
    get => this._lastShapeId;
    set => this._lastShapeId = value;
  }

  internal bool IsAnimated
  {
    get => this.isAnimated;
    set => this.isAnimated = value;
  }

  public IAnimationTimeline Timeline
  {
    get
    {
      if (this.IsAnimated && (this.timeLine as Syncfusion.Presentation.Timeline).GetMainSequence().Count == 0 && (this.timeLine as Syncfusion.Presentation.Timeline).GetInteractiveSequences().Count == 0 && this.timing.TimeNodeList != null)
        this.UpdateSlideTimeLine();
      return this.timeLine;
    }
  }

  internal bool IsAlternateContent
  {
    get => this.isAlternateContent;
    set => this.isAlternateContent = value;
  }

  internal bool IsSlideTransition
  {
    get => this.isSlideTransition;
    set => this.isSlideTransition = value;
  }

  public ISlideShowTransition SlideTransition => this.slideTransition;

  public InternalSlideTransition InternalTransition
  {
    get => this.internal_SlideTransition;
    set => this.internal_SlideTransition = value;
  }

  public string Name
  {
    get => this._name;
    set => this._name = value;
  }

  internal Dictionary<string, Stream> SlidePrsvedElts
  {
    get => this._bgPreserve;
    set => this._bgPreserve = value;
  }

  public IShapes Shapes
  {
    get => (IShapes) this._shapeCollection ?? (IShapes) (this._shapeCollection = new Syncfusion.Presentation.Drawing.Shapes(this));
    internal set => this._shapeCollection = value as Syncfusion.Presentation.Drawing.Shapes;
  }

  public IGroupShapes GroupShapes => (IGroupShapes) new GroupShapesCollection(this);

  public ITables Tables => (ITables) new TableCollection(this);

  public IPictures Pictures => (IPictures) new PicturesCollection(this);

  public IPresentationCharts Charts => (IPresentationCharts) new ChartCollection(this);

  public IBackground Background
  {
    get
    {
      return (IBackground) this._background ?? (IBackground) (this._background = new Syncfusion.Presentation.Background(this));
    }
    internal set => this._background = (Syncfusion.Presentation.Background) value;
  }

  internal RelationCollection VMLRelation
  {
    get => this._vmlRelation;
    set => this._vmlRelation = value;
  }

  internal bool HasOLEObject
  {
    get
    {
      if (!this._hasOLEObject.HasValue)
      {
        foreach (Relation relation in this.TopRelation.GetRelationList())
        {
          if (relation.Type.Contains("vmlDrawing"))
            this._hasOLEObject = new bool?(true);
        }
      }
      return this._hasOLEObject.HasValue && this._hasOLEObject.Value;
    }
    set => this._hasOLEObject = new bool?(value);
  }

  internal Syncfusion.Presentation.Presentation Presentation
  {
    get => this._presentation;
    set => this._presentation = value;
  }

  internal bool IsDisposed => this._isDisposed;

  internal bool IsColorMapChanged
  {
    get => this._isColorMapChanged;
    set => this._isColorMapChanged = value;
  }

  internal RelationCollection TopRelation
  {
    get => this._topRelation;
    set => this._topRelation = value;
  }

  internal Theme BaseTheme
  {
    get
    {
      switch (this)
      {
        case MasterSlide _:
          return (this as MasterSlide).Theme;
        case LayoutSlide _:
          return ((MasterSlide) __nonvirtual (((LayoutSlide) this).MasterSlide)).Theme;
        case Slide _:
          return ((MasterSlide) ((Slide) this).LayoutSlide.MasterSlide).Theme;
        case NotesMasterSlide _:
          return ((NotesMasterSlide) this).Theme;
        case NotesSlide _:
          return ((MasterSlide) ((NotesSlide) this).ParentSlide.LayoutSlide.MasterSlide).Theme;
        default:
          return (this as HandoutMaster).ThemeCollection;
      }
    }
  }

  internal void RemoveAnimationEffectsOfShape(Shape shape)
  {
    if (this.Timeline.InteractiveSequences.Count > 0)
    {
      for (int index = 0; index < this.Timeline.InteractiveSequences.Count; ++index)
      {
        ISequence interactiveSequence = this.Timeline.InteractiveSequences[index];
        if (interactiveSequence.GetEffectsByShape((IShape) shape).Length > 0)
          this.Timeline.InteractiveSequences.Remove(interactiveSequence);
      }
    }
    if (this.Timeline.MainSequence.Count <= 0)
      return;
    for (int index1 = 0; index1 < this.Timeline.MainSequence.Count; ++index1)
    {
      IEffect[] effectsByShape = this.Timeline.MainSequence.GetEffectsByShape((IShape) shape);
      for (int index2 = 0; index2 < effectsByShape.Length; ++index2)
        this.Timeline.MainSequence.Remove(effectsByShape[index1]);
    }
  }

  internal Shape GetShapeWithId(Syncfusion.Presentation.Drawing.Shapes shapes, int shapeId)
  {
    for (int index = 0; index < shapes.Count; ++index)
    {
      if (shapes[index] is GroupShape)
      {
        if ((shapes[index] as Shape).ShapeId == shapeId)
          return shapes[index] as Shape;
        Shape shapeWithId = this.GetShapeWithId((shapes[index] as GroupShape).Shapes as Syncfusion.Presentation.Drawing.Shapes, shapeId);
        if (shapeWithId != null)
          return shapeWithId;
      }
      else if ((shapes[index] as Shape).ShapeId == shapeId)
        return shapes[index] as Shape;
    }
    return (Shape) null;
  }

  internal IAnimationTimeline GetSlideTimeLine() => this.timeLine;

  private void UpdateSlideTimeLine()
  {
    foreach (object timeNode in this.Timing.TimeNodeList)
    {
      if (timeNode is ParallelTimeNode)
      {
        ParallelTimeNode par = timeNode as ParallelTimeNode;
        if (par.CommonTimeNode.NodeType == TimeNodeType.TmRoot)
          this.UpdateSequences(par);
      }
    }
  }

  private void UpdateSequences(ParallelTimeNode par)
  {
    if (par.CommonTimeNode.ChildTimeNodeList == null)
      return;
    foreach (object childTimeNode in par.CommonTimeNode.ChildTimeNodeList)
    {
      if (childTimeNode is SequenceTimeNode)
      {
        SequenceTimeNode seq = childTimeNode as SequenceTimeNode;
        if (seq.CommonTimeNode.NodeType == TimeNodeType.MainSeq)
          this.UpdateMainSequence(seq);
        if (seq.CommonTimeNode.NodeType == TimeNodeType.InteractiveSeq)
          this.UpdateInteractiveSequence(seq);
      }
    }
  }

  private void UpdateMainSequence(SequenceTimeNode seq)
  {
    Sequence mainSequence = (this.timeLine as Syncfusion.Presentation.Timeline).GetMainSequence() as Sequence;
    AnimationConstant.UpdateEffects(seq.CommonTimeNode, mainSequence, (Effect) null);
  }

  private void UpdateInteractiveSequence(SequenceTimeNode seq)
  {
    ISequences interactiveSequences = (this.timeLine as Syncfusion.Presentation.Timeline).GetInteractiveSequences();
    CommonTimeNode commonTimeNode = seq.CommonTimeNode;
    Sequence sequence = new Sequence(this);
    AnimationConstant.UpdateSequenceShape(commonTimeNode, sequence);
    AnimationConstant.UpdateEffects(commonTimeNode, sequence, (Effect) null);
    (interactiveSequences as Sequences).Add(sequence);
  }

  internal bool IsImageReferenceExists(string picturePath)
  {
    if ((this.Background as Syncfusion.Presentation.Background).GetFillFormat().FillType == FillType.Picture && ((this.Background as Syncfusion.Presentation.Background).GetFillFormat().PictureFill as TextureFill).GetImagePath() == picturePath)
      return true;
    foreach (Picture picture in (IEnumerable<IPicture>) this.Pictures)
    {
      if (picture.GetImagePath() == picturePath)
        return true;
    }
    return false;
  }

  internal virtual void Close() => this.DisposeAll();

  internal void DisposeAll()
  {
    if (this._isDisposed)
      return;
    this.ClearAll();
    this._isDisposed = true;
  }

  private void ClearAll()
  {
    if (this._background != null)
    {
      this._background.Close();
      this._background = (Syncfusion.Presentation.Background) null;
    }
    if (this._shapeCollection != null)
    {
      this._shapeCollection.Close();
      this._shapeCollection = (Syncfusion.Presentation.Drawing.Shapes) null;
    }
    if (this._topRelation != null)
    {
      this._topRelation.Close();
      this._topRelation = (RelationCollection) null;
    }
    if (this.ColorMap != null)
    {
      this.ColorMap.Clear();
      this.ColorMap = (Dictionary<string, string>) null;
    }
    if (this._bgPreserve != null)
    {
      foreach (KeyValuePair<string, Stream> keyValuePair in this._bgPreserve)
        keyValuePair.Value.Dispose();
      this._bgPreserve.Clear();
      this._bgPreserve = (Dictionary<string, Stream>) null;
    }
    if (this._headersFooters != null)
    {
      (this._headersFooters as Syncfusion.Presentation.HeaderFooter.HeadersFooters).Dispose();
      this._headersFooters = (IHeadersFooters) null;
    }
    this._presentation = (Syncfusion.Presentation.Presentation) null;
  }

  internal BaseSlide Clone(BaseSlide newParent)
  {
    if (this._background != null)
    {
      newParent._background = this._background.Clone();
      newParent._background.SetParent(newParent);
    }
    newParent._bgPreserve = Helper.CloneDictionary(this._bgPreserve);
    newParent._isColorMapChanged = this._isColorMapChanged;
    newParent._isDisposed = this._isDisposed;
    newParent._name = this._name;
    newParent._topRelation = this._topRelation.Clone();
    newParent._shapeCollection = this._shapeCollection.Clone();
    newParent._shapeCollection.SetParent(newParent);
    newParent._headersFooters = (this._headersFooters as Syncfusion.Presentation.HeaderFooter.HeadersFooters).Clone();
    (newParent._headersFooters as Syncfusion.Presentation.HeaderFooter.HeadersFooters).SetParent((IBaseSlide) newParent);
    if (this.isAnimated)
    {
      newParent.timing = this.timing.Clone(newParent);
      newParent.timeLine = (IAnimationTimeline) (this.timeLine as Syncfusion.Presentation.Timeline).Clone(newParent);
    }
    return newParent;
  }

  internal virtual void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    this._presentation = presentation;
    this._shapeCollection.SetParent(presentation);
  }
}
