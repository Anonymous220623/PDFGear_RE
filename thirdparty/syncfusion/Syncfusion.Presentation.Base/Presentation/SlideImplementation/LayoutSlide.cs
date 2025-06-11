// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideImplementation.LayoutSlide
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.SlideImplementation;

internal class LayoutSlide : BaseSlide, ILayoutSlide, IBaseSlide
{
  private string _layoutId;
  private Syncfusion.Presentation.SlideImplementation.MasterSlide _masterSlide;
  private SlideLayoutType _ppSlideLayout;
  private bool _showMasterShape;
  private bool _isChanged;
  private Dictionary<string, bool> _headerFooter;

  internal LayoutSlide(Syncfusion.Presentation.Presentation presentation, Syncfusion.Presentation.SlideImplementation.MasterSlide masterSlide, string layoutId)
    : base(presentation)
  {
    this._masterSlide = masterSlide;
    this._layoutId = layoutId;
    this._showMasterShape = true;
  }

  public SlideLayoutType LayoutType => this._ppSlideLayout;

  public IMasterSlide MasterSlide => (IMasterSlide) this._masterSlide;

  internal Dictionary<string, bool> HeaderFooter
  {
    get => this._headerFooter;
    set => this._headerFooter = value;
  }

  internal bool ShowMasterShape
  {
    get => this._showMasterShape;
    set => this._showMasterShape = value;
  }

  internal string LayoutId
  {
    get => this._layoutId;
    set => this._layoutId = value;
  }

  internal void IsChanged(bool value) => this._isChanged = value;

  internal bool IsChanged() => this._isChanged;

  internal Syncfusion.Presentation.SlideImplementation.MasterSlide GetMasterSlide(
    Syncfusion.Presentation.Presentation newParent)
  {
    foreach (Syncfusion.Presentation.SlideImplementation.MasterSlide master in (IEnumerable<IMasterSlide>) newParent.Masters)
    {
      if (this._masterSlide.MasterId == master.MasterId)
        return master;
    }
    return (Syncfusion.Presentation.SlideImplementation.MasterSlide) null;
  }

  internal RelationCollection AddRelationToLayoutSlide(int number, Syncfusion.Presentation.SlideImplementation.MasterSlide masterSlide)
  {
    Relation relation1 = new Relation("rId" + (object) number, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/slideLayout", $"../slideLayouts/slideLayout{(object) number}.xml", (string) null);
    masterSlide.TopRelation.Add(relation1.Id, relation1);
    string itemByContentType = masterSlide.Presentation.TopRelation.GetItemByContentType("http://schemas.openxmlformats.org/officeDocument/2006/relationships/slideMaster");
    string idByTarget = masterSlide.Presentation.TopRelation.GetIdByTarget(itemByContentType);
    if (!masterSlide.Presentation.OverrideContentType.ContainsKey($"/ppt/slideLayouts/slideLayout{(object) number}.xml"))
      masterSlide.Presentation.OverrideContentType.Add($"/ppt/slideLayouts/slideLayout{(object) number}.xml", "application/vnd.openxmlformats-officedocument.presentationml.slideLayout+xml");
    Relation relation2 = new Relation(idByTarget, "http://schemas.openxmlformats.org/officeDocument/2006/relationships/slideMaster", "../" + itemByContentType, (string) null);
    RelationCollection layoutSlide = new RelationCollection();
    layoutSlide.Add(idByTarget, relation2);
    return layoutSlide;
  }

  internal void AddShapesFromSourceLayout(LayoutSlide sourceLayoutSlide)
  {
    this.Shapes = (IShapes) (sourceLayoutSlide.Shapes as Syncfusion.Presentation.Drawing.Shapes).Clone();
    (this.Shapes as Syncfusion.Presentation.Drawing.Shapes).SetParent((BaseSlide) this);
  }

  internal void AddShapesFromLayoutType(SlideLayoutType ppSlideLayout)
  {
    Syncfusion.Presentation.Drawing.Shapes shapes = (Syncfusion.Presentation.Drawing.Shapes) this.Shapes;
    switch (ppSlideLayout)
    {
      case SlideLayoutType.Custom:
      case SlideLayoutType.TitleOnly:
        shapes.AddLayoutTitleOnly();
        break;
      case SlideLayoutType.Title:
        shapes.AddLayoutTitle();
        break;
      case SlideLayoutType.TitleAndContent:
        shapes.AddLayoutObject();
        break;
      case SlideLayoutType.SectionHeader:
        shapes.AddLayoutSectionHeader();
        break;
      case SlideLayoutType.TwoContent:
        shapes.AddLayoutTwoObjects();
        break;
      case SlideLayoutType.Comparison:
        shapes.AddLayoutComparison();
        break;
      case SlideLayoutType.Blank:
        shapes.AddLayoutBlank();
        break;
      case SlideLayoutType.ContentWithCaption:
        shapes.AddLayoutContentWithCaption();
        break;
      case SlideLayoutType.PictureWithCaption:
        shapes.AddLayoutPictureWithCaption();
        break;
      case SlideLayoutType.TitleAndVerticalText:
        shapes.AddLayoutVerticalText();
        break;
      case SlideLayoutType.VerticalTitleAndText:
        shapes.AddLayoutVerticalTitleAndText();
        break;
    }
  }

  internal void SetType(SlideLayoutType layoutType) => this._ppSlideLayout = layoutType;

  internal override void Close()
  {
    this._masterSlide = (Syncfusion.Presentation.SlideImplementation.MasterSlide) null;
    base.Close();
  }

  public LayoutSlide Clone()
  {
    LayoutSlide newParent = (LayoutSlide) this.MemberwiseClone();
    this.Clone((BaseSlide) newParent);
    return newParent;
  }

  internal void SetMaster(Syncfusion.Presentation.SlideImplementation.MasterSlide masterSlide)
  {
    this._masterSlide = masterSlide;
  }

  internal override void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    base.SetParent(presentation);
  }

  internal void SetParent(Syncfusion.Presentation.SlideImplementation.MasterSlide masterSlide)
  {
    base.SetParent(masterSlide.Presentation);
    this._masterSlide = masterSlide;
  }
}
