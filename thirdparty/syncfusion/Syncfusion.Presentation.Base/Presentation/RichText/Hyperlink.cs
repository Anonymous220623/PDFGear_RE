// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.RichText.Hyperlink
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using Syncfusion.Presentation.SmartArtImplementation;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.RichText;

internal class Hyperlink : IHyperLink
{
  private string _relationId;
  private string _actionString;
  private string _screenTip;
  private HyperLinkType _actionType;
  private HyperLinkColor _hyperLinkColorType;
  private ISlide _targetSlide;
  private string _link;
  private object _parent;
  private bool _highlightClick;

  internal Hyperlink(TextPart textPart) => this._parent = (object) textPart;

  internal Hyperlink(Shape shape) => this._parent = (object) shape;

  internal Hyperlink(SmartArtPoint smartArtPoint) => this._parent = (object) smartArtPoint;

  public ISlide TargetSlide
  {
    get
    {
      this.EnsureTargetSlide();
      return this._targetSlide;
    }
  }

  internal bool HighlightClick
  {
    get => this._highlightClick;
    set => this._highlightClick = value;
  }

  internal IColor Color
  {
    get
    {
      Syncfusion.Presentation.Presentation presentation = !(this._parent is Shape) ? ((TextPart) this._parent).Paragraph.BaseSlide.Presentation : ((Shape) this._parent).BaseSlide.Presentation;
      ColorObject color = new ColorObject(true);
      color.SetColor(ColorType.Theme, "hlink");
      color.UpdateColorObject((object) presentation);
      return (IColor) color;
    }
  }

  public HyperLinkType Action
  {
    get
    {
      if (this._actionType == HyperLinkType.Unknown)
        this._actionType = Helper.GetActionType(this._actionString);
      return this._actionType;
    }
    internal set
    {
      this._actionType = value;
      this._actionString = Helper.GetActionString(value, this.Url);
    }
  }

  public string Url
  {
    get
    {
      return this._actionType == HyperLinkType.Hyperlink || this._actionType == HyperLinkType.OpenFile ? this._link : (string) null;
    }
    set
    {
      this._link = value;
      this.SetLink(value);
    }
  }

  internal string RelationId
  {
    get => this._relationId;
    set
    {
      this._relationId = value;
      if (string.IsNullOrEmpty(value))
        return;
      RelationCollection relationCollection = (RelationCollection) null;
      if (this._parent is TextPart)
      {
        TextPart parent = (TextPart) this._parent;
        if (parent.Paragraph != null)
        {
          Paragraph paragraph = parent.Paragraph;
          relationCollection = paragraph.BaseShape == null || !(paragraph.BaseShape is SmartArt) && !(paragraph.BaseShape is SmartArtShape) ? paragraph.BaseSlide.TopRelation : (!(paragraph.BaseShape is SmartArt) ? (paragraph.BaseShape as SmartArtShape).ParentSmartArt.DataModel.DrawingRelation : (paragraph.BaseShape as SmartArt).DataModel.TopRelation);
        }
      }
      else if (this._parent is Shape)
        relationCollection = ((Shape) this._parent).BaseSlide.TopRelation;
      if (relationCollection == null)
        return;
      this._link = relationCollection.GetTargetByRelationId(value);
    }
  }

  internal string ActionString
  {
    get => this._actionString;
    set
    {
      this._actionString = value;
      this._actionType = Helper.GetActionType(value);
    }
  }

  internal HyperLinkColor HyperLinkColorType
  {
    get => this._hyperLinkColorType;
    set => this._hyperLinkColorType = value;
  }

  public string ScreenTip
  {
    get => this._screenTip;
    set => this._screenTip = value;
  }

  internal void SetLink(string link)
  {
    this._link = link;
    if (this._actionType == HyperLinkType.Hyperlink)
    {
      if (link.StartsWith("\\"))
        this._link = "file:" + link;
      else if (!link.StartsWith("file:///"))
        this._link = "file:///" + link;
    }
    RelationCollection topRelation = (RelationCollection) null;
    if (this._parent is TextPart)
      topRelation = ((TextPart) this._parent).Paragraph.BaseSlide.TopRelation;
    else if (this._parent is Shape)
      topRelation = ((Shape) this._parent).BaseSlide.TopRelation;
    else if (this._parent is SmartArtPoint)
      topRelation = ((SmartArtPoint) this._parent).TextBody.BaseSlide.TopRelation;
    if (topRelation == null)
      return;
    this._relationId = Helper.GenerateRelationId(topRelation);
    Relation relation = new Relation();
    relation.Id = this._relationId;
    relation.Target = link;
    switch (this._actionType)
    {
      case HyperLinkType.Hyperlink:
      case HyperLinkType.OpenFile:
      case HyperLinkType.StartProgram:
        relation.TargetMode = "External";
        break;
    }
    relation.Type = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink";
    topRelation.Add(this._relationId, relation);
  }

  internal void SetTargetSlide(ISlide slide) => this._targetSlide = slide;

  internal void SetTargetSlideRelation()
  {
    RelationCollection topRelation = (RelationCollection) null;
    string str = (string) null;
    if (this._parent is TextPart)
    {
      TextPart parent = (TextPart) this._parent;
      topRelation = parent.Paragraph.BaseSlide.TopRelation;
      str = this.GetTargetString(parent.Paragraph.BaseSlide.Presentation);
    }
    else if (this._parent is Shape)
    {
      Shape parent = (Shape) this._parent;
      topRelation = parent.BaseSlide.TopRelation;
      str = this.GetTargetString(parent.BaseSlide.Presentation);
    }
    if (str == null || topRelation == null)
      return;
    this._relationId = Helper.GenerateRelationId(topRelation);
    Relation relation = new Relation();
    relation.Id = this._relationId;
    string[] strArray = str.Split('/');
    relation.Target = strArray[strArray.Length - 1];
    switch (this._actionType)
    {
      case HyperLinkType.Hyperlink:
      case HyperLinkType.OpenFile:
      case HyperLinkType.StartProgram:
        relation.TargetMode = "External";
        break;
    }
    relation.Type = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/slide";
    topRelation.Add(this._relationId, relation);
  }

  private string GetTargetString(Syncfusion.Presentation.Presentation presentation)
  {
    if (this._targetSlide == null)
      return (string) null;
    string str = ((Slide) this._targetSlide).SlideID.ToString();
    foreach (KeyValuePair<string, string> slide in presentation.SlideList)
    {
      if (str == slide.Value)
        return presentation.TopRelation.GetItemPathByRelation(slide.Key);
    }
    return (string) null;
  }

  internal void EnsureTargetSlide()
  {
    if (this._targetSlide != null || this._actionType != HyperLinkType.JumpSpecificSlide || this._link == null)
      return;
    BaseSlide baseSlide = (BaseSlide) null;
    if (this._parent is Shape)
      baseSlide = ((Shape) this._parent).BaseSlide;
    else if (this._parent is TextPart && ((TextPart) this._parent).Paragraph != null)
      baseSlide = ((TextPart) this._parent).Paragraph.BaseSlide;
    else if (this._parent is SmartArtPoint)
      baseSlide = ((SmartArtPoint) this._parent).TextBody.BaseSlide;
    if (baseSlide == null || baseSlide.Presentation == null || !baseSlide.Presentation.SlidesFromInputFile.ContainsKey(this._link))
      return;
    this._targetSlide = baseSlide.Presentation.SlidesFromInputFile[this._link];
    this._link = (string) null;
    string[] strArray = this.GetTargetString(baseSlide.Presentation).Split('/');
    foreach (Relation relation in baseSlide.TopRelation.GetRelationList())
    {
      if (relation.Id == this.RelationId)
      {
        relation.Target = strArray[strArray.Length - 1];
        break;
      }
    }
  }

  internal string GetLink() => this.TargetSlide != null ? (string) null : this._link;

  public Hyperlink Clone()
  {
    Hyperlink hyperlink = (Hyperlink) this.MemberwiseClone();
    if (this._targetSlide != null)
      hyperlink._targetSlide = this._targetSlide.Clone();
    return hyperlink;
  }

  internal void SetParent(Shape shape) => this._parent = (object) shape;

  internal void SetParent(SmartArtPoint point) => this._parent = (object) point;

  internal void Close()
  {
    if (this._targetSlide != null)
      this._targetSlide = (ISlide) null;
    this._parent = (object) null;
  }
}
