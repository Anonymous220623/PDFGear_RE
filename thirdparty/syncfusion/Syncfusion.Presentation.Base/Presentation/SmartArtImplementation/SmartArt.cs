// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SmartArtImplementation.SmartArt
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.SmartArtImplementation;

internal class SmartArt : Shape, ISmartArt
{
  private SmartArtNodes _nodeCollection;
  private DataModel _dataModel;
  private bool _is2005Layout;
  [ThreadStatic]
  private static bool _created;
  private bool _createdSet;
  private SmartArtLayout _layoutDefinition;
  private string _layoutRelationId;
  private string _colorsRelationId;
  private string _quickstyleRelationId;

  internal SmartArt(BaseSlide baseSlide)
    : base(ShapeType.GraphicFrame, baseSlide)
  {
    this.DrawingType = DrawingType.SmartArt;
    this._nodeCollection = new SmartArtNodes((object) this);
    this._dataModel = new DataModel(this);
  }

  internal bool Is2005Layout => this._is2005Layout;

  internal SmartArtLayout LayoutDefinition
  {
    get => this._layoutDefinition ?? (this._layoutDefinition = new SmartArtLayout(this));
  }

  internal bool CreatedSmartArt
  {
    get => SmartArt._created;
    set
    {
      if (this._createdSet)
        return;
      SmartArt._created = value;
      this._createdSet = true;
    }
  }

  internal DataModel DataModel => this._dataModel;

  internal string LayoutRelationId
  {
    get => this._layoutRelationId;
    set => this._layoutRelationId = value;
  }

  internal string ColorsRelationId
  {
    get => this._colorsRelationId;
    set => this._colorsRelationId = value;
  }

  internal string QuickStyleRelationId
  {
    get => this._quickstyleRelationId;
    set => this._quickstyleRelationId = value;
  }

  public IFill Background => this.Fill;

  public SmartArtType Layout => this._dataModel.SmartArtType;

  public ISmartArtNodes Nodes => (ISmartArtNodes) this._nodeCollection;

  internal void SetSmartArtType(SmartArtType smartArtType)
  {
    this._dataModel.SmartArtType = smartArtType;
    this._nodeCollection.AddDocumentPoint();
    this._nodeCollection.AddNodes();
    foreach (SmartArtNode node in this._nodeCollection)
      (node.ChildNodes as SmartArtNodes).AddChildNodes(this._dataModel.ChildNodeCount);
    switch (smartArtType)
    {
      case SmartArtType.TableHierarchy:
      case SmartArtType.CirclePictureHierarchy:
      case SmartArtType.Hierarchy:
      case SmartArtType.HorizontalHierarchy:
        (((SmartArtNode) this._nodeCollection[0].ChildNodes[0]).ChildNodes as SmartArtNodes).AddChildNodes(2U);
        (((SmartArtNode) this._nodeCollection[0].ChildNodes[1]).ChildNodes as SmartArtNodes).AddChildNodes(1U);
        break;
      case SmartArtType.SubStepProcess:
        (((SmartArtNode) this._nodeCollection[0]).ChildNodes as SmartArtNodes).AddChildNodes(2U);
        break;
      case SmartArtType.PhasedProcess:
        (((SmartArtNode) this._nodeCollection[0]).ChildNodes as SmartArtNodes).AddChildNodes(3U);
        (((SmartArtNode) this._nodeCollection[1]).ChildNodes as SmartArtNodes).AddChildNodes(2U);
        (((SmartArtNode) this._nodeCollection[2]).ChildNodes as SmartArtNodes).AddChildNodes(1U);
        break;
      case SmartArtType.OrganizationChart:
      case SmartArtType.NameAndTitleOrganizationChart:
      case SmartArtType.HalfCircleOrganizationChart:
      case SmartArtType.HorizontalOrganizationChart:
        ((SmartArtNode) this._nodeCollection[0].ChildNodes[0]).BasePoint.Type = SmartArtPointType.AssistantElement;
        break;
      case SmartArtType.LabeledHierarchy:
      case SmartArtType.HorizontalLabeledHierarchy:
        (((SmartArtNode) this._nodeCollection[0]).ChildNodes as SmartArtNodes).AddChildNodes(2U);
        (((SmartArtNode) this._nodeCollection[0].ChildNodes[0]).ChildNodes as SmartArtNodes).AddChildNodes(2U);
        (((SmartArtNode) this._nodeCollection[0].ChildNodes[1]).ChildNodes as SmartArtNodes).AddChildNodes(1U);
        break;
      case SmartArtType.Balance:
        (((SmartArtNode) this._nodeCollection[0]).ChildNodes as SmartArtNodes).AddChildNodes(2U);
        (((SmartArtNode) this._nodeCollection[1]).ChildNodes as SmartArtNodes).AddChildNodes(3U);
        break;
    }
  }

  internal void SetCategoryByType()
  {
    switch (this._dataModel.SmartArtType)
    {
      case SmartArtType.BasicBlockList:
        this._dataModel.SetSmartArtNodeCount(5U, 0U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.AlternatingHexagons:
        this._dataModel.SetSmartArtNodeCount(3U, 1U);
        this._dataModel.Category = "list";
        this._is2005Layout = false;
        break;
      case SmartArtType.PictureCaptionList:
        this._dataModel.SetSmartArtNodeCount(4U, 0U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.LinedList:
        this._dataModel.SetSmartArtNodeCount(1U, 3U);
        this._dataModel.Category = "list";
        this._is2005Layout = false;
        break;
      case SmartArtType.VerticalBulletList:
        this._dataModel.SetSmartArtNodeCount(2U, 1U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.VerticalBoxList:
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.HorizontalBulletList:
        this._dataModel.SetSmartArtNodeCount(3U, 2U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.SquareAccentList:
        this._dataModel.SetSmartArtNodeCount(2U, 3U);
        this._dataModel.Category = "list";
        this._is2005Layout = false;
        break;
      case SmartArtType.PictureAccentList:
        this._dataModel.Category = "list";
        this._dataModel.SetSmartArtNodeCount(3U, 2U);
        this._is2005Layout = true;
        break;
      case SmartArtType.BendingPictureAccentList:
        this._dataModel.Category = "list";
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._is2005Layout = true;
        break;
      case SmartArtType.StackedList:
        this._dataModel.Category = "list";
        this._dataModel.SetSmartArtNodeCount(2U, 2U);
        this._is2005Layout = true;
        break;
      case SmartArtType.IncreasingCircleProcess:
        this._dataModel.Category = "list";
        this._dataModel.SetSmartArtNodeCount(3U, 1U);
        this._is2005Layout = false;
        break;
      case SmartArtType.PieProcess:
        this._dataModel.SetSmartArtNodeCount(3U, 1U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.DetailedProcess:
        this._dataModel.SetSmartArtNodeCount(3U, 1U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.GroupedList:
        this._dataModel.SetSmartArtNodeCount(3U, 2U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.HorizontalPictureList:
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.ContinuousPictureList:
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.PictureStrips:
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "list";
        this._is2005Layout = false;
        break;
      case SmartArtType.VerticalPictureList:
        this._dataModel.SetSmartArtNodeCount(3U, 2U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.AlternatingPictureBlocks:
        this._dataModel.Category = "list";
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._is2005Layout = false;
        break;
      case SmartArtType.VerticalPictureAccentList:
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.TitledPictureAccentList:
        this._dataModel.SetSmartArtNodeCount(1U, 2U);
        this._dataModel.Category = "list";
        this._is2005Layout = false;
        break;
      case SmartArtType.VerticalBlockList:
        this._dataModel.SetSmartArtNodeCount(3U, 2U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.VerticalChevronList:
        this._dataModel.SetSmartArtNodeCount(3U, 2U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.VerticalAccentList:
        this._dataModel.Category = "list";
        this._dataModel.SetSmartArtNodeCount(3U, 1U);
        this._is2005Layout = false;
        break;
      case SmartArtType.VerticalArrowList:
        this._dataModel.Category = "list";
        this._dataModel.SetSmartArtNodeCount(2U, 2U);
        this._is2005Layout = true;
        break;
      case SmartArtType.TrapezoidList:
        this._dataModel.Category = "list";
        this._dataModel.SetSmartArtNodeCount(3U, 2U);
        this._is2005Layout = true;
        break;
      case SmartArtType.DescendingBlockList:
        this._dataModel.SetSmartArtNodeCount(3U, 2U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.TableList:
        this._dataModel.SetSmartArtNodeCount(1U, 3U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.SegmentedProcess:
        this._dataModel.SetSmartArtNodeCount(3U, 2U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.VerticalCurvedList:
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "list";
        this._is2005Layout = false;
        break;
      case SmartArtType.PyramidList:
        this._dataModel.Category = "list";
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._is2005Layout = true;
        break;
      case SmartArtType.TargetList:
        this._dataModel.SetSmartArtNodeCount(3U, 2U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.VerticalCircleList:
        this._dataModel.SetSmartArtNodeCount(2U, 2U);
        this._dataModel.Category = "list";
        this._is2005Layout = false;
        break;
      case SmartArtType.TableHierarchy:
        this._dataModel.SetSmartArtNodeCount(1U, 2U);
        this._dataModel.Category = "list";
        this._is2005Layout = true;
        break;
      case SmartArtType.BasicProcess:
        this._dataModel.Category = "process";
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._is2005Layout = true;
        break;
      case SmartArtType.StepUpProcess:
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "process";
        this._is2005Layout = true;
        break;
      case SmartArtType.StepDownProcess:
        this._dataModel.SetSmartArtNodeCount(3U, 1U);
        this._dataModel.Category = "process";
        this._is2005Layout = true;
        break;
      case SmartArtType.AccentProcess:
        this._dataModel.Category = "process";
        this._dataModel.SetSmartArtNodeCount(3U, 1U);
        this._is2005Layout = true;
        break;
      case SmartArtType.AlternatingFlow:
        this._dataModel.Category = "process";
        this._dataModel.SetSmartArtNodeCount(3U, 2U);
        this._is2005Layout = true;
        break;
      case SmartArtType.ContinuousBlockProcess:
        this._dataModel.Category = "process";
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._is2005Layout = true;
        break;
      case SmartArtType.IncreasingArrowsProcess:
        this._dataModel.Category = "process";
        this._dataModel.SetSmartArtNodeCount(3U, 1U);
        this._is2005Layout = true;
        break;
      case SmartArtType.ContinuousArrowProcess:
        this._dataModel.Category = "process";
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._is2005Layout = true;
        break;
      case SmartArtType.ProcessArrows:
        this._dataModel.Category = "process";
        this._dataModel.SetSmartArtNodeCount(3U, 2U);
        this._is2005Layout = true;
        break;
      case SmartArtType.CircleAccentTimeLine:
        this._dataModel.Category = "process";
        this._dataModel.SetSmartArtNodeCount(2U, 2U);
        this._is2005Layout = false;
        break;
      case SmartArtType.BasicTimeLine:
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "process";
        this._is2005Layout = true;
        break;
      case SmartArtType.BasicChevronProcess:
        this._dataModel.Category = "process";
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._is2005Layout = true;
        break;
      case SmartArtType.ClosedChevronProcess:
        this._dataModel.Category = "process";
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._is2005Layout = true;
        break;
      case SmartArtType.ChevronList:
        this._dataModel.SetSmartArtNodeCount(3U, 2U);
        this._dataModel.Category = "process";
        this._is2005Layout = true;
        break;
      case SmartArtType.SubStepProcess:
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "process";
        this._is2005Layout = true;
        break;
      case SmartArtType.PhasedProcess:
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "process";
        this._is2005Layout = true;
        break;
      case SmartArtType.RandomToResultProcess:
        this._dataModel.SetSmartArtNodeCount(2U, 1U);
        this._dataModel.Category = "process";
        this._is2005Layout = true;
        break;
      case SmartArtType.StaggeredProcess:
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "process";
        this._is2005Layout = true;
        break;
      case SmartArtType.ProcessList:
        this._dataModel.SetSmartArtNodeCount(2U, 2U);
        this._dataModel.Category = "process";
        this._is2005Layout = true;
        break;
      case SmartArtType.CircleArrowProcess:
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "process";
        this._is2005Layout = true;
        break;
      case SmartArtType.BasicBendingProcess:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(5U, 0U);
        this._dataModel.Category = "process";
        break;
      case SmartArtType.VerticalBendingProcess:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(9U, 0U);
        this._dataModel.Category = "process";
        break;
      case SmartArtType.AscendingPictureAccentprocess:
        this._is2005Layout = false;
        this._dataModel.Category = "process";
        this._dataModel.SetSmartArtNodeCount(2U, 0U);
        break;
      case SmartArtType.UpwardArrow:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "process";
        break;
      case SmartArtType.DescendingProcess:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(5U, 0U);
        this._dataModel.Category = "process";
        break;
      case SmartArtType.CircularBendingProcess:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(9U, 0U);
        this._dataModel.Category = "process";
        break;
      case SmartArtType.Equation:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "process";
        break;
      case SmartArtType.VerticalEquation:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "process";
        break;
      case SmartArtType.Funnel:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(4U, 0U);
        this._dataModel.Category = "process";
        break;
      case SmartArtType.Gear:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "process";
        break;
      case SmartArtType.ArrowRibbon:
        this._is2005Layout = true;
        this._dataModel.Category = "process";
        this._dataModel.SetSmartArtNodeCount(2U, 0U);
        break;
      case SmartArtType.OpposingArrows:
        this._dataModel.SetSmartArtNodeCount(2U, 0U);
        this._is2005Layout = true;
        this._dataModel.Category = "process";
        break;
      case SmartArtType.ConvergingArrows:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(2U, 0U);
        this._dataModel.Category = "process";
        break;
      case SmartArtType.DivergingArrows:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(2U, 0U);
        this._dataModel.Category = "process";
        break;
      case SmartArtType.BasicCycle:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(5U, 0U);
        this._dataModel.Category = "cycle";
        break;
      case SmartArtType.TextCycle:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(5U, 0U);
        this._dataModel.Category = "cycle";
        break;
      case SmartArtType.BlockCycle:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(5U, 0U);
        this._dataModel.Category = "cycle";
        break;
      case SmartArtType.NondirectionalCycle:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(5U, 0U);
        this._dataModel.Category = "cycle";
        break;
      case SmartArtType.ContinuousCycle:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(5U, 0U);
        this._dataModel.Category = "cycle";
        break;
      case SmartArtType.MultiDirectionalCycle:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "cycle";
        break;
      case SmartArtType.SegmentedCycle:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "cycle";
        break;
      case SmartArtType.BasicPie:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "cycle";
        break;
      case SmartArtType.RadialCycle:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(1U, 4U);
        this._dataModel.Category = "cycle";
        break;
      case SmartArtType.BasicRadial:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(1U, 4U);
        this._dataModel.Category = "cycle";
        break;
      case SmartArtType.DivergingRadial:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(1U, 4U);
        this._dataModel.Category = "cycle";
        break;
      case SmartArtType.RadialVenn:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(1U, 4U);
        this._dataModel.Category = "cycle";
        break;
      case SmartArtType.RadialCluster:
        this._is2005Layout = false;
        this._dataModel.SetSmartArtNodeCount(1U, 3U);
        this._dataModel.Category = "cycle";
        break;
      case SmartArtType.OrganizationChart:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(1U, 4U);
        this._dataModel.Category = "hierarchy";
        break;
      case SmartArtType.NameAndTitleOrganizationChart:
        this._is2005Layout = false;
        this._dataModel.SetSmartArtNodeCount(1U, 4U);
        this._dataModel.Category = "hierarchy";
        break;
      case SmartArtType.HalfCircleOrganizationChart:
        this._is2005Layout = false;
        this._dataModel.SetSmartArtNodeCount(1U, 4U);
        this._dataModel.Category = "hierarchy";
        break;
      case SmartArtType.CirclePictureHierarchy:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(1U, 2U);
        this._dataModel.Category = "hierarchy";
        break;
      case SmartArtType.Hierarchy:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(1U, 2U);
        this._dataModel.Category = "hierarchy";
        break;
      case SmartArtType.LabeledHierarchy:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(4U, 0U);
        this._dataModel.Category = "hierarchy";
        break;
      case SmartArtType.HorizontalOrganizationChart:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(1U, 4U);
        this._dataModel.Category = "hierarchy";
        break;
      case SmartArtType.HorizontalMulti_levelHierarchy:
        this._is2005Layout = false;
        this._dataModel.SetSmartArtNodeCount(1U, 3U);
        this._dataModel.Category = "hierarchy";
        break;
      case SmartArtType.HorizontalHierarchy:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(1U, 2U);
        this._dataModel.Category = "hierarchy";
        break;
      case SmartArtType.HorizontalLabeledHierarchy:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(4U, 0U);
        this._dataModel.Category = "hierarchy";
        break;
      case SmartArtType.Balance:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(2U, 0U);
        this._dataModel.Category = "relationship";
        break;
      case SmartArtType.CircleRelationship:
        this._dataModel.SetSmartArtNodeCount(1U, 2U);
        this._is2005Layout = true;
        this._dataModel.Category = "relationship";
        break;
      case SmartArtType.HexagonCluster:
        this._is2005Layout = false;
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "relationship";
        break;
      case SmartArtType.OpposingIdeas:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(2U, 1U);
        this._dataModel.Category = "relationship";
        break;
      case SmartArtType.PlusAndMinus:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(2U, 0U);
        this._dataModel.Category = "relationship";
        break;
      case SmartArtType.ReverseList:
        this._dataModel.SetSmartArtNodeCount(2U, 0U);
        this._is2005Layout = true;
        this._dataModel.Category = "relationship";
        break;
      case SmartArtType.CounterBalanceArrows:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(2U, 0U);
        this._dataModel.Category = "relationship";
        break;
      case SmartArtType.SegmentedPyramid:
        this._dataModel.SetSmartArtNodeCount(4U, 0U);
        this._is2005Layout = true;
        this._dataModel.Category = "relationship";
        break;
      case SmartArtType.NestedTarget:
        this._dataModel.SetSmartArtNodeCount(3U, 2U);
        this._is2005Layout = true;
        this._dataModel.Category = "relationship";
        break;
      case SmartArtType.ConvergingRadial:
        this._dataModel.SetSmartArtNodeCount(1U, 3U);
        this._is2005Layout = true;
        this._dataModel.Category = "relationship";
        break;
      case SmartArtType.RadialList:
        this._dataModel.SetSmartArtNodeCount(3U, 2U);
        this._is2005Layout = true;
        this._dataModel.Category = "relationship";
        break;
      case SmartArtType.BasicTarget:
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._is2005Layout = true;
        this._dataModel.Category = "relationship";
        break;
      case SmartArtType.BasicMatrix:
        this._dataModel.SetSmartArtNodeCount(4U, 0U);
        this._is2005Layout = true;
        this._dataModel.Category = "matrix";
        break;
      case SmartArtType.TitledMatrix:
        this._dataModel.SetSmartArtNodeCount(1U, 4U);
        this._is2005Layout = true;
        this._dataModel.Category = "matrix";
        break;
      case SmartArtType.GridMatrix:
        this._dataModel.SetSmartArtNodeCount(4U, 0U);
        this._is2005Layout = true;
        this._dataModel.Category = "matrix";
        break;
      case SmartArtType.CycleMatrix:
        this._dataModel.SetSmartArtNodeCount(4U, 1U);
        this._is2005Layout = true;
        this._dataModel.Category = "matrix";
        break;
      case SmartArtType.AccentedPicture:
        this._is2005Layout = false;
        this._dataModel.Category = "picture";
        this._dataModel.SetSmartArtNodeCount(4U, 0U);
        break;
      case SmartArtType.CircularPictureCallOut:
        this._is2005Layout = false;
        this._dataModel.SetSmartArtNodeCount(4U, 0U);
        this._dataModel.Category = "picture";
        break;
      case SmartArtType.SnapshotPictureList:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(1U, 1U);
        this._dataModel.Category = "picture";
        break;
      case SmartArtType.SpiralPicture:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(5U, 0U);
        this._dataModel.Category = "picture";
        break;
      case SmartArtType.CaptionedPictures:
        this._is2005Layout = false;
        this._dataModel.SetSmartArtNodeCount(3U, 1U);
        this._dataModel.Category = "picture";
        break;
      case SmartArtType.BendingPictureCaption:
        this._is2005Layout = false;
        this._dataModel.SetSmartArtNodeCount(2U, 0U);
        this._dataModel.Category = "picture";
        break;
      case SmartArtType.BendingPictureSemiTransparentText:
        this._is2005Layout = false;
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "picture";
        break;
      case SmartArtType.BendingPictureBlocks:
        this._is2005Layout = false;
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "picture";
        break;
      case SmartArtType.BendingPictureCaptionList:
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._is2005Layout = false;
        this._dataModel.Category = "picture";
        break;
      case SmartArtType.TitledPictureBlocks:
        this._dataModel.SetSmartArtNodeCount(3U, 1U);
        this._is2005Layout = false;
        this._dataModel.Category = "picture";
        break;
      case SmartArtType.PictureGrid:
        this._dataModel.SetSmartArtNodeCount(4U, 0U);
        this._is2005Layout = false;
        this._dataModel.Category = "picture";
        break;
      case SmartArtType.PictureAccentBlocks:
        this._is2005Layout = false;
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "picture";
        break;
      case SmartArtType.AlternatingPictureCircles:
        this._is2005Layout = false;
        this._dataModel.Category = "picture";
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        break;
      case SmartArtType.TitlePictureLineup:
        this._is2005Layout = false;
        this._dataModel.SetSmartArtNodeCount(3U, 1U);
        this._dataModel.Category = "picture";
        break;
      case SmartArtType.PictureLineUp:
        this._is2005Layout = false;
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "picture";
        break;
      case SmartArtType.FramedTextPicture:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(1U, 0U);
        this._dataModel.Category = "picture";
        break;
      case SmartArtType.BubblePictureList:
        this._is2005Layout = false;
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "picture";
        break;
      case SmartArtType.BasicPyramid:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "pyramid";
        break;
      case SmartArtType.InvertedPyramid:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "pyramid";
        break;
      case SmartArtType.BasicVenn:
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._is2005Layout = true;
        this._dataModel.Category = "relationship";
        break;
      case SmartArtType.LinearVenn:
        this._dataModel.SetSmartArtNodeCount(4U, 0U);
        this._is2005Layout = true;
        this._dataModel.Category = "relationship";
        break;
      case SmartArtType.StackedVenn:
        this._dataModel.SetSmartArtNodeCount(4U, 0U);
        this._is2005Layout = true;
        this._dataModel.Category = "relationship";
        break;
      case SmartArtType.HierarchyList:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(2U, 2U);
        this._dataModel.Category = "relationship";
        break;
      case SmartArtType.PictureAccentProcess:
        this._is2005Layout = true;
        this._dataModel.Category = "picture";
        this._dataModel.SetSmartArtNodeCount(3U, 2U);
        break;
      case SmartArtType.RepeatingBendingProcess:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(5U, 0U);
        this._dataModel.Category = "process";
        break;
      case SmartArtType.VerticalProcess:
        this._is2005Layout = true;
        this._dataModel.SetSmartArtNodeCount(3U, 0U);
        this._dataModel.Category = "process";
        break;
    }
  }

  internal void InitializeDrawingShapes()
  {
    this._dataModel.RefreshSmartArtShapeCollection();
    switch (this._dataModel.SmartArtType)
    {
      case SmartArtType.BasicBlockList:
        SmartArt.InitializeDefaultPropertiesForBasicBlockList(this);
        break;
      case SmartArtType.AlternatingHexagons:
        SmartArt.InitializeDefaultPropertiesForAlternatingHexagons(this);
        break;
      case SmartArtType.PictureCaptionList:
        SmartArt.InitializeDefaultPropertiesForPictureCaptionList(this);
        break;
      case SmartArtType.LinedList:
        SmartArt.InitializeDefaultPropertiesForLinedList(this);
        break;
      case SmartArtType.VerticalBulletList:
        SmartArt.InitializeDefaultPropertiesForVerticalBulletList(this);
        break;
      case SmartArtType.VerticalBoxList:
        SmartArt.InitializeDefaultPropertiesForVerticalBoxList(this);
        break;
      case SmartArtType.HorizontalBulletList:
        SmartArt.InitializeDefaultPropertiesForHorizontalBulletList(this);
        break;
      case SmartArtType.SquareAccentList:
        SmartArt.InitializeDefaultPropertiesForSquareAccentList(this);
        break;
      case SmartArtType.PictureAccentList:
        SmartArt.InitializeDefaultPropertiesForPictureAccentList(this);
        break;
      case SmartArtType.BendingPictureAccentList:
        SmartArt.InitializeDefaultPropertiesForBendingPictureAccentList(this);
        break;
      case SmartArtType.StackedList:
        SmartArt.InitializeDefaultPropertiesForStackedList(this);
        break;
      case SmartArtType.IncreasingCircleProcess:
        SmartArt.InitializeDefaultPropertiesForIncreasingCircleProcess(this);
        break;
      case SmartArtType.PieProcess:
        SmartArt.InitializeDefaultPropertiesForPieProcess(this);
        break;
      case SmartArtType.DetailedProcess:
        SmartArt.InitializeDefaultPropertiesForDetailedProcess(this);
        break;
      case SmartArtType.GroupedList:
        SmartArt.InitializeDefaultPropertiesForGroupedList(this);
        break;
      case SmartArtType.HorizontalPictureList:
        SmartArt.InitializeDefaultPropertiesForHorizontalPictureList(this);
        break;
      case SmartArtType.ContinuousPictureList:
        SmartArt.InitializeDefaultPropertiesForContinuousPictureList(this);
        break;
      case SmartArtType.PictureStrips:
        SmartArt.InitializeDefaultPropertiesForPictureStrips(this);
        break;
      case SmartArtType.VerticalPictureList:
        SmartArt.InitializeDefaultPropertiesForVerticalPictureList(this);
        break;
      case SmartArtType.AlternatingPictureBlocks:
        SmartArt.InitializeDefaultPropertiesForAlternatingPictureBlocks(this);
        break;
      case SmartArtType.VerticalPictureAccentList:
        SmartArt.InitializeDefaultPropertiesForVerticalPictureAccentList(this);
        break;
      case SmartArtType.TitledPictureAccentList:
        SmartArt.InitializeDefaultPropertiesForTitledPictureAccentList(this);
        break;
      case SmartArtType.VerticalBlockList:
        SmartArt.InitializeDefaultPropertiesForVerticalBlockList(this);
        break;
      case SmartArtType.VerticalChevronList:
        SmartArt.InitializeDefaultPropertiesForVerticalChevronList(this);
        break;
      case SmartArtType.VerticalAccentList:
        SmartArt.InitializeDefaultPropertiesForVerticalAccentList(this);
        break;
      case SmartArtType.VerticalArrowList:
        SmartArt.InitializeDefaultPropertiesForVerticalArrowList(this);
        break;
      case SmartArtType.TrapezoidList:
        SmartArt.InitializeDefaultPropertiesForTrapezoidList(this);
        break;
      case SmartArtType.DescendingBlockList:
        SmartArt.InitializeDefaultPropertiesForDescendingBlockList(this);
        break;
      case SmartArtType.TableList:
        SmartArt.InitializeDefaultPropertiesForTableList(this);
        break;
      case SmartArtType.SegmentedProcess:
        SmartArt.InitializeDefaultPropertiesForSegmentedProcess(this);
        break;
      case SmartArtType.VerticalCurvedList:
        SmartArt.InitializeDefaultPropertiesForVerticalCurvedList(this);
        break;
      case SmartArtType.PyramidList:
        SmartArt.InitializeDefaultPropertiesForPyramidList(this);
        break;
      case SmartArtType.TargetList:
        SmartArt.InitializeDefaultPropertiesForTargetList(this);
        break;
      case SmartArtType.VerticalCircleList:
        SmartArt.InitializeDefaultPropertiesForVerticalCircleList(this);
        break;
      case SmartArtType.TableHierarchy:
        SmartArt.InitializeDefaultPropertiesForTableHierarchy(this);
        break;
      case SmartArtType.BasicProcess:
        SmartArt.InitializeDefaultPropertiesForBasicProcess(this);
        break;
      case SmartArtType.StepUpProcess:
        SmartArt.InitializeDefaultPropertiesForStepUpProcess(this);
        break;
      case SmartArtType.StepDownProcess:
        SmartArt.InitializeDefaultPropertiesForStepDownProcess(this);
        break;
      case SmartArtType.AccentProcess:
        SmartArt.InitializeDefaultPropertiesForAccentProcess(this);
        break;
      case SmartArtType.AlternatingFlow:
        SmartArt.InitializeDefaultPropertiesForAlternatingFlow(this);
        break;
      case SmartArtType.ContinuousBlockProcess:
        SmartArt.InitializeDefaultPropertiesForContinuousBlockProcess(this);
        break;
      case SmartArtType.IncreasingArrowsProcess:
        SmartArt.InitializeDefaultPropertiesForIncreasingArrowProcess(this);
        break;
      case SmartArtType.ContinuousArrowProcess:
        SmartArt.InitializeDefaultPropertiesForContinuousArrowProcess(this);
        break;
      case SmartArtType.ProcessArrows:
        SmartArt.InitializeDefaultPropertiesForProcessArrows(this);
        break;
      case SmartArtType.CircleAccentTimeLine:
        SmartArt.InitializeDefaultPropertiesForCircleAccentTimeLine(this);
        break;
      case SmartArtType.BasicTimeLine:
        SmartArt.InitializeDefaultPropertiesForBasicTimeLine(this);
        break;
      case SmartArtType.BasicChevronProcess:
        SmartArt.InitializeDefaultPropertiesForBasicChevronProcess(this);
        break;
      case SmartArtType.ClosedChevronProcess:
        SmartArt.InitializeDefaultPropertiesForClosedChevronProcess(this);
        break;
      case SmartArtType.ChevronList:
        SmartArt.InitializeDefaultPropertiesForChevronList(this);
        break;
      case SmartArtType.SubStepProcess:
        SmartArt.InitializeDefaultPropertiesForSubStepProcess(this);
        break;
      case SmartArtType.PhasedProcess:
        SmartArt.InitializeDefaultPropertiesForPhasedProcess(this);
        break;
      case SmartArtType.RandomToResultProcess:
        SmartArt.InitializeDefaultPropertiesForRandomToResultProcess(this);
        break;
      case SmartArtType.StaggeredProcess:
        SmartArt.InitializeDefaultPropertiesForStaggeredProcess(this);
        break;
      case SmartArtType.ProcessList:
        SmartArt.InitializeDefaultPropertiesForProcessList(this);
        break;
      case SmartArtType.CircleArrowProcess:
        SmartArt.InitializeDefaultPropertiesForCircleArrowProcess(this);
        break;
      case SmartArtType.BasicBendingProcess:
        SmartArt.InitializeDefaultPropertiesForBasicBendingProcess(this);
        break;
      case SmartArtType.VerticalBendingProcess:
        SmartArt.InitializeDefaultPropertiesForVerticalBendingProcess(this);
        break;
      case SmartArtType.AscendingPictureAccentprocess:
        SmartArt.InitializeDefaultPropertiesForAscendingPictureAccentProcess(this);
        break;
      case SmartArtType.UpwardArrow:
        SmartArt.InitializeDefaultPropertiesForUpwardArrow(this);
        break;
      case SmartArtType.DescendingProcess:
        SmartArt.InitializeDefaultPropertiesForDescendingProcess(this);
        break;
      case SmartArtType.CircularBendingProcess:
        SmartArt.InitializeDefaultPropertiesForCircularBendingProcess(this);
        break;
      case SmartArtType.Equation:
        SmartArt.InitializeDefaultPropertiesForEquation(this);
        break;
      case SmartArtType.VerticalEquation:
        SmartArt.InitializeDefaultPropertiesForVerticalEquation(this);
        break;
      case SmartArtType.Funnel:
        SmartArt.InitializeDefaultPropertiesForFunnel(this);
        break;
      case SmartArtType.Gear:
        SmartArt.InitializeDefaultPropertiesForGear(this);
        break;
      case SmartArtType.ArrowRibbon:
        SmartArt.InitializeDefaultPropertiesForArrowRibbon(this);
        break;
      case SmartArtType.OpposingArrows:
        SmartArt.InitializeDefaultPropertiesForOpposingArrows(this);
        break;
      case SmartArtType.ConvergingArrows:
        SmartArt.InitializeDefaultPropertiesForConvergingArrows(this);
        break;
      case SmartArtType.DivergingArrows:
        SmartArt.InitializeDefaultPropertiesForDivergingArrows(this);
        break;
      case SmartArtType.BasicCycle:
        SmartArt.InitializeDefaultPropertiesForBasicCycle(this);
        break;
      case SmartArtType.TextCycle:
        SmartArt.InitializeDefaultPropertiesForTextCycle(this);
        break;
      case SmartArtType.BlockCycle:
        SmartArt.InitializeDefaultPropertiesForBlockCycle(this);
        break;
      case SmartArtType.NondirectionalCycle:
        SmartArt.InitializeDefaultPropertiesForNondirectionalCycle(this);
        break;
      case SmartArtType.ContinuousCycle:
        SmartArt.InitializeDefaultPropertiesForContinuousCycle(this);
        break;
      case SmartArtType.MultiDirectionalCycle:
        SmartArt.InitializeDefaultPropertiesForMultiDirectionalCycle(this);
        break;
      case SmartArtType.SegmentedCycle:
        SmartArt.InitializeDefaultPropertiesForSegmentedCycle(this);
        break;
      case SmartArtType.BasicPie:
        SmartArt.InitializeDefaultPropertiesForBasicPie(this);
        break;
      case SmartArtType.RadialCycle:
        SmartArt.InitializeDefaultPropertiesForRadialCycle(this);
        break;
      case SmartArtType.BasicRadial:
        SmartArt.InitializeDefaultPropertiesForBasicRadial(this);
        break;
      case SmartArtType.DivergingRadial:
        SmartArt.InitializeDefaultPropertiesForDivergingRadial(this);
        break;
      case SmartArtType.RadialVenn:
        SmartArt.InitializeDefaultPropertiesForRadialVenn(this);
        break;
      case SmartArtType.RadialCluster:
        SmartArt.InitializeDefaultPropertiesForRadialCluster(this);
        break;
      case SmartArtType.OrganizationChart:
        SmartArt.InitializeDefaultPropertiesForOrganizationChart(this);
        break;
      case SmartArtType.NameAndTitleOrganizationChart:
        SmartArt.InitializeDefaultPropertiesForNameAndTitleOrganizationChart(this);
        break;
      case SmartArtType.HalfCircleOrganizationChart:
        SmartArt.InitializeDefaultPropertiesForHalfCircleOrganizationChart(this);
        break;
      case SmartArtType.CirclePictureHierarchy:
        SmartArt.InitializeDefaultPropertiesForCirclePictureHierarchy(this);
        break;
      case SmartArtType.Hierarchy:
        SmartArt.InitializeDefaultPropertiesForHierarchy(this);
        break;
      case SmartArtType.LabeledHierarchy:
        SmartArt.InitializeDefaultPropertiesForLabeledHierarchy(this);
        break;
      case SmartArtType.HorizontalOrganizationChart:
        SmartArt.InitializeDefaultPropertiesForHorizontalOrganizationChart(this);
        break;
      case SmartArtType.HorizontalMulti_levelHierarchy:
        SmartArt.InitializeDefaultPropertiesForHorizontalMultiLevelHierarchy(this);
        break;
      case SmartArtType.HorizontalHierarchy:
        SmartArt.InitializeDefaultPropertiesForHorizontalHierarchy(this);
        break;
      case SmartArtType.HorizontalLabeledHierarchy:
        SmartArt.InitializeDefaultPropertiesForHorizontalLabeledHierarchy(this);
        break;
      case SmartArtType.Balance:
        SmartArt.InitializeDefaultPropertiesForBalance(this);
        break;
      case SmartArtType.CircleRelationship:
        SmartArt.InitializeDefaultPropertiesForCircleRelationship(this);
        break;
      case SmartArtType.HexagonCluster:
        SmartArt.InitializeDefaultPropertiesForHexagonCluster(this);
        break;
      case SmartArtType.OpposingIdeas:
        SmartArt.InitializeDefaultPropertiesForOpposingIdeas(this);
        break;
      case SmartArtType.PlusAndMinus:
        SmartArt.InitializeDefaultPropertiesForPlusAndMinus(this);
        break;
      case SmartArtType.ReverseList:
        SmartArt.InitializeDefaultPropertiesForReverseList(this);
        break;
      case SmartArtType.CounterBalanceArrows:
        SmartArt.InitializeDefaultPropertiesForCounterBalanceArrows(this);
        break;
      case SmartArtType.SegmentedPyramid:
        SmartArt.InitializeDefaultPropertiesForSegmentedPyramid(this);
        break;
      case SmartArtType.NestedTarget:
        SmartArt.InitializeDefaultPropertiesForNestedTarget(this);
        break;
      case SmartArtType.ConvergingRadial:
        SmartArt.InitializeDefaultPropertiesForConvergingRadial(this);
        break;
      case SmartArtType.RadialList:
        SmartArt.InitializeDefaultPropertiesForRadialList(this);
        break;
      case SmartArtType.BasicTarget:
        SmartArt.InitializeDefaultPropertiesForBasicTarget(this);
        break;
      case SmartArtType.BasicMatrix:
        SmartArt.InitializeDefaultPropertiesForBasicMartrix(this);
        break;
      case SmartArtType.TitledMatrix:
        SmartArt.InitializeDefaultPropertiesForTitledMatrix(this);
        break;
      case SmartArtType.GridMatrix:
        SmartArt.InitializeDefaultPropertiesForGridMatrix(this);
        break;
      case SmartArtType.CycleMatrix:
        SmartArt.InitializeDefaultPropertiesForCycleMatrix(this);
        break;
      case SmartArtType.AccentedPicture:
        SmartArt.InitializeDefaultPropertiesForAccentedPicture(this);
        break;
      case SmartArtType.CircularPictureCallOut:
        SmartArt.InitializeDefaultPropertiesForCircularPictureCallOut(this);
        break;
      case SmartArtType.SnapshotPictureList:
        SmartArt.InitializeDefaultPropertiesForSnapshotPictureList(this);
        break;
      case SmartArtType.SpiralPicture:
        SmartArt.InitializeDefaultPropertiesForSpiralPicture(this);
        break;
      case SmartArtType.CaptionedPictures:
        SmartArt.InitializeDefaultPropertiesForCaptionedPictues(this);
        break;
      case SmartArtType.BendingPictureCaption:
        SmartArt.InitializeDefaultPropertiesForBendingPictureCaption(this);
        break;
      case SmartArtType.BendingPictureSemiTransparentText:
        SmartArt.InitializeDefaultPropertiesForBendingPictureSemiTransparentText(this);
        break;
      case SmartArtType.BendingPictureBlocks:
        SmartArt.InitializeDefaultPropertiesForBendingPictureBlocks(this);
        break;
      case SmartArtType.BendingPictureCaptionList:
        SmartArt.InitializeDefaultPropertiesForBendingPictureCaptionList(this);
        break;
      case SmartArtType.TitledPictureBlocks:
        SmartArt.InitializeDefaultPropertiesForTitledPictureBlocks(this);
        break;
      case SmartArtType.PictureGrid:
        SmartArt.InitializeDefaultPropertiesForPictureGrid(this);
        break;
      case SmartArtType.PictureAccentBlocks:
        SmartArt.InitializeDefaultPropertiesForPictureAccentBlocks(this);
        break;
      case SmartArtType.AlternatingPictureCircles:
        SmartArt.InitializeDefaultPropertiesForAlternatingPictureCircles(this);
        break;
      case SmartArtType.TitlePictureLineup:
        SmartArt.InitializeDefaultPropertiesForTitlePictureLineup(this);
        break;
      case SmartArtType.PictureLineUp:
        SmartArt.InitializeDefaultPropertiesForPictureLineUp(this);
        break;
      case SmartArtType.FramedTextPicture:
        SmartArt.InitializeDefaultPropertiesForFramedTextPicture(this);
        break;
      case SmartArtType.BubblePictureList:
        SmartArt.InitializeDefaultPropertiesForBubblePictureList(this);
        break;
      case SmartArtType.BasicPyramid:
        SmartArt.InitializeDefaultPropertiesForBasicPyramid(this);
        break;
      case SmartArtType.InvertedPyramid:
        SmartArt.InitializeDefaultPropertiesForInvertedPyramid(this);
        break;
      case SmartArtType.BasicVenn:
        SmartArt.InitializeDefaultPropertiesForBasicVenn(this);
        break;
      case SmartArtType.LinearVenn:
        SmartArt.InitializeDefaultPropertiesForLinearVenn(this);
        break;
      case SmartArtType.StackedVenn:
        SmartArt.InitializeDefaultPropertiesForStackedVenn(this);
        break;
      case SmartArtType.HierarchyList:
        SmartArt.InitializeDefaultPropertiesForHierarchyList(this);
        break;
      case SmartArtType.PictureAccentProcess:
        SmartArt.InitializeDefaultPropertiesForPictureAccentProcess(this);
        break;
      case SmartArtType.RepeatingBendingProcess:
        SmartArt.InitializeDefaultPropertiesForRepeatingBendingProcess(this);
        break;
      case SmartArtType.VerticalProcess:
        SmartArt.InitializeDefaultPropertiesForVerticalProcess(this);
        break;
    }
  }

  private static void InitializeDefaultPropertiesForInvertedPyramid(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForInvertedPyramid(smartArt._nodeCollection[0], AutoShapeType.Trapezoid, 0, 0, 8128000, 1806222, smartArt);
    SmartArt.SetDefaultPropertiesForInvertedPyramid(smartArt._nodeCollection[1], AutoShapeType.Trapezoid, 1354666, 1806222, 5418666, 1806222, smartArt);
    SmartArt.SetDefaultPropertiesForInvertedPyramid(smartArt._nodeCollection[2], AutoShapeType.Trapezoid, 2709333, 3612444, 2709333, 1806222, smartArt);
  }

  private static void SetDefaultPropertiesForInvertedPyramid(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    smartArtShape.GetGuideList().Add("adj", "val 75000");
    SmartArt.SetShapeProperties(smartArtShape, false, false, 10800000, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBasicPyramid(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBasicPyramid(smartArt._nodeCollection[0], AutoShapeType.Trapezoid, 2709333, 0, 2709333, 1806222, smartArt);
    SmartArt.SetDefaultPropertiesForBasicPyramid(smartArt._nodeCollection[1], AutoShapeType.Trapezoid, 1354666, 1806222, 5418666, 1806222, smartArt);
    SmartArt.SetDefaultPropertiesForBasicPyramid(smartArt._nodeCollection[2], AutoShapeType.Trapezoid, 0, 3612444, 8128000, 1806222, smartArt);
  }

  private static void SetDefaultPropertiesForBasicPyramid(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    smartArtShape.GetGuideList().Add("adj", "val 75000");
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBubblePictureList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBubblePictureList(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 0, 1452217, 2887878, 937768, true, smartArt);
    SmartArt.SetDefaultPropertiesForBubblePictureList(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 5240121, 2880666, 2887878, 898229, true, smartArt);
    SmartArt.SetDefaultPropertiesForBubblePictureList(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 5133644, 1452217, 2887878, 1167901, true, smartArt);
    SmartArt.SetDefaultPropertiesForBubblePictureList((ISmartArtNode) null, AutoShapeType.Oval, 1924710, 2452841, 1945843, 1946164, true, smartArt);
    SmartArt.SetDefaultPropertiesForBubblePictureList((ISmartArtNode) null, AutoShapeType.Donut, 3165855, 1019661, 577900, 577530, true, smartArt);
    SmartArt.SetDefaultPropertiesForBubblePictureList((ISmartArtNode) null, AutoShapeType.Oval, 1999487, 2527524, 1797100, 1796797, false, smartArt);
    SmartArt.SetDefaultPropertiesForBubblePictureList((ISmartArtNode) null, AutoShapeType.Oval, 4011980, 2820513, 1018438, 1018196, true, smartArt);
    SmartArt.SetDefaultPropertiesForBubblePictureList((ISmartArtNode) null, AutoShapeType.Oval, 4072127, 2880666, 898144, 898229, false, smartArt);
    SmartArt.SetDefaultPropertiesForBubblePictureList((ISmartArtNode) null, AutoShapeType.Oval, 3613708, 1383278, 1305356, 1305778, true, smartArt);
    SmartArt.SetDefaultPropertiesForBubblePictureList((ISmartArtNode) null, AutoShapeType.Donut, 4705299, 1062578, 427532, 427825, true, smartArt);
    SmartArt.SetDefaultPropertiesForBubblePictureList((ISmartArtNode) null, AutoShapeType.Donut, 5133644, 3843103, 321056, 320699, true, smartArt);
    SmartArt.SetDefaultPropertiesForBubblePictureList((ISmartArtNode) null, AutoShapeType.Oval, 3682796, 1452217, 1167993, 1167901, false, smartArt);
    SmartArt.SetDefaultPropertiesForBubblePictureList((ISmartArtNode) null, AutoShapeType.Oval, 3682796, 1452217, 1167993, 1167901, false, smartArt);
  }

  private static void SetDefaultPropertiesForBubblePictureList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isParent,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties, isParent);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForFramedTextPicture(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForFramedTextPicture((ISmartArtNode) null, AutoShapeType.Rectangle, 0, 31950, 3134156, 2089429, 0, smartArt);
    SmartArt.SetDefaultPropertiesForFramedTextPicture(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 3265017, 2252036, 4440326, 2742711, 0, smartArt);
    SmartArt.SetDefaultPropertiesForFramedTextPicture((ISmartArtNode) null, AutoShapeType.HalfFrame, 2873247, 1860603, 1066393, 1066669, 0, smartArt);
    SmartArt.SetDefaultPropertiesForFramedTextPicture((ISmartArtNode) null, AutoShapeType.HalfFrame, 7061468, 1860740, 1066393, 1066669, 5400000, smartArt);
    SmartArt.SetDefaultPropertiesForFramedTextPicture((ISmartArtNode) null, AutoShapeType.HalfFrame, 2873110, 4320185, 1066393, 1066669, 16200000, smartArt);
    SmartArt.SetDefaultPropertiesForFramedTextPicture((ISmartArtNode) null, AutoShapeType.HalfFrame, 7061606, 4320047, 1066393, 1066669, 10800000, smartArt);
  }

  private static void SetDefaultPropertiesForFramedTextPicture(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle && smartArtNode != null)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    if (autoShapeType == AutoShapeType.HalfFrame)
    {
      smartArtShape.GetGuideList().Add("adj1", "val 25770");
      smartArtShape.GetGuideList().Add("adj2", "val 25770");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForPictureLineUp(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForPictureLineUp((ISmartArtNode) null, AutoShapeType.Rectangle, 3156, 2645, 2706687, 2706687, smartArt);
    SmartArt.SetDefaultPropertiesForPictureLineUp((ISmartArtNode) null, AutoShapeType.Line, 3156, 2645, 270, 5413375, smartArt);
    SmartArt.SetDefaultPropertiesForPictureLineUp((ISmartArtNode) null, AutoShapeType.Rectangle, 2710656, 2645, 2706687, 2706687, smartArt);
    SmartArt.SetDefaultPropertiesForPictureLineUp((ISmartArtNode) null, AutoShapeType.Line, 2710656, 2645, 270, 5413375, smartArt);
    SmartArt.SetDefaultPropertiesForPictureLineUp((ISmartArtNode) null, AutoShapeType.Rectangle, 5418155, 2645, 2706687, 2706687, smartArt);
    SmartArt.SetDefaultPropertiesForPictureLineUp((ISmartArtNode) null, AutoShapeType.Line, 5418155, 2645, 270, 5413375, smartArt);
    SmartArt.SetDefaultPropertiesForPictureLineUp(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 3156, 2709333, 2706687, 2706687, smartArt);
    SmartArt.SetDefaultPropertiesForPictureLineUp(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 2710656, 2709333, 2706687, 2706687, smartArt);
    SmartArt.SetDefaultPropertiesForPictureLineUp(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 5421312, 2711979, 2706687, 2706687, smartArt);
  }

  private static void SetDefaultPropertiesForPictureLineUp(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (smartArtNode != null && autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForTitlePictureLineup(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForTitlePictureLineup((ISmartArtNode) null, AutoShapeType.Line, 3968, 740833, 0, 4429125, false, smartArt);
    SmartArt.SetDefaultPropertiesForTitlePictureLineup((ISmartArtNode) null, AutoShapeType.Rectangle, 126999, 888470, 2329473, 1993106, false, smartArt);
    SmartArt.SetDefaultPropertiesForTitlePictureLineup((ISmartArtNode) null, AutoShapeType.Line, 2833687, 740833, 0, 4429125, false, smartArt);
    SmartArt.SetDefaultPropertiesForTitlePictureLineup(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 3968, 248708, 2460625, 492125, false, smartArt);
    SmartArt.SetDefaultPropertiesForTitlePictureLineup((ISmartArtNode) null, AutoShapeType.Rectangle, 2956718, 888470, 2329473, 1993106, false, smartArt);
    SmartArt.SetDefaultPropertiesForTitlePictureLineup((ISmartArtNode) null, AutoShapeType.Line, 5663406, 740833, 0, 4429125, false, smartArt);
    SmartArt.SetDefaultPropertiesForTitlePictureLineup(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 2833687, 248708, 2460625, 492125, false, smartArt);
    SmartArt.SetDefaultPropertiesForTitlePictureLineup((ISmartArtNode) null, AutoShapeType.Rectangle, 5786437, 888470, 2329473, 1993106, false, smartArt);
    SmartArt.SetDefaultPropertiesForTitlePictureLineup(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 5663406, 248708, 2460625, 492125, false, smartArt);
    SmartArt.SetDefaultPropertiesForTitlePictureLineup(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 126999, 2881577, 2329473, 2288381, true, smartArt);
    SmartArt.SetDefaultPropertiesForTitlePictureLineup(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.Rectangle, 2956718, 2881577, 2329473, 2288381, true, smartArt);
    SmartArt.SetDefaultPropertiesForTitlePictureLineup(smartArt._nodeCollection[2].ChildNodes[0], AutoShapeType.Rectangle, 5786437, 2881577, 2329473, 2288381, true, smartArt);
  }

  private static void SetDefaultPropertiesForTitlePictureLineup(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isChild,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (isChild && autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (!isChild)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 2500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForAlternatingPictureCircles(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForAlternatingPictureCircles((ISmartArtNode) null, AutoShapeType.Donut, 3386655, 3985, 1354688, 1354666, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingPictureCircles((ISmartArtNode) null, AutoShapeType.Rectangle, 2065934, 51397, 1665987, 1259819, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingPictureCircles(smartArt._nodeCollection[0], AutoShapeType.Oval, 3535711 /*0x35F35F*/, 153038, 1056577, 1056560, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingPictureCircles((ISmartArtNode) null, AutoShapeType.FlowChartConnector, 3929330, 1560656, 269339, 269339, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingPictureCircles((ISmartArtNode) null, AutoShapeType.Donut, 3386655, 2032000, 1354688, 1354666, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingPictureCircles((ISmartArtNode) null, AutoShapeType.Rectangle, 4396078, 2079412, 1665987, 1259819, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingPictureCircles(smartArt._nodeCollection[1], AutoShapeType.Oval, 3535711 /*0x35F35F*/, 2181053, 1056577, 1056560, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingPictureCircles((ISmartArtNode) null, AutoShapeType.FlowChartConnector, 3929330, 3588671, 269339, 269339, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingPictureCircles((ISmartArtNode) null, AutoShapeType.Donut, 3386655, 4060015, 1354688, 1354666, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingPictureCircles((ISmartArtNode) null, AutoShapeType.Rectangle, 2065934, 4107427, 1665987, 1259819, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingPictureCircles(smartArt._nodeCollection[2], AutoShapeType.Oval, 3535711 /*0x35F35F*/, 4209068, 1056577, 1056560, smartArt);
  }

  private static void SetDefaultPropertiesForAlternatingPictureCircles(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    if (autoShapeType == AutoShapeType.Donut)
      smartArtShape.GetGuideList().Add("adj", "val 11010");
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForPictureAccentBlocks(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForPictureAccentBlocks((ISmartArtNode) null, AutoShapeType.Rectangle, 2638579, 2923646, 2495020, 2495020, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentBlocks((ISmartArtNode) null, AutoShapeType.Rectangle, 5632979, 2923646, 2495020, 2495020, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentBlocks((ISmartArtNode) null, AutoShapeType.Rectangle, 5632979, 90300, 2495020, 2495020, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentBlocks(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 1141566, 3921654, 2495020, 499004, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentBlocks(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 4135966, 3921654, 2495020, 499004, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentBlocks(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 4135966, 1088308, 2495020, 499004, smartArt);
  }

  private static void SetDefaultPropertiesForPictureAccentBlocks(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    int rotation = 0;
    if (smartArtNode != null && autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
      rotation = 16200000;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForPictureGrid(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForPictureGrid((ISmartArtNode) null, AutoShapeType.Rectangle, 1787143, 438649, 2162556, 2162556, smartArt);
    SmartArt.SetDefaultPropertiesForPictureGrid((ISmartArtNode) null, AutoShapeType.Rectangle, 4178300, 438649, 2162556, 2162556, smartArt);
    SmartArt.SetDefaultPropertiesForPictureGrid((ISmartArtNode) null, AutoShapeType.Rectangle, 1787143, 3206138, 2162556, 2162556, smartArt);
    SmartArt.SetDefaultPropertiesForPictureGrid((ISmartArtNode) null, AutoShapeType.Rectangle, 4178300, 3206138, 2162556, 2162556, smartArt);
    SmartArt.SetDefaultPropertiesForPictureGrid(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 1787143, 49972, 2162556, 324383, smartArt);
    SmartArt.SetDefaultPropertiesForPictureGrid(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 4178300, 49972, 2162556, 324383, smartArt);
    SmartArt.SetDefaultPropertiesForPictureGrid(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 1787143, 2817461, 2162556, 324383, smartArt);
    SmartArt.SetDefaultPropertiesForPictureGrid(smartArt._nodeCollection[3], AutoShapeType.Rectangle, 4178300, 2817461, 2162556, 324383, smartArt);
  }

  private static void SetDefaultPropertiesForPictureGrid(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (smartArtNode != null)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForTitledPictureBlocks(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForTitledPictureBlocks((ISmartArtNode) null, AutoShapeType.Rectangle, 528696, 428484, 2494516, 2113588, false, smartArt);
    SmartArt.SetDefaultPropertiesForTitledPictureBlocks(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.RoundedRectangle, 2691033, 724256, 1182863, 1231127, true, smartArt);
    SmartArt.SetDefaultPropertiesForTitledPictureBlocks(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 528696, 25469, 2494516, 363951, true, smartArt);
    SmartArt.SetDefaultPropertiesForTitledPictureBlocks((ISmartArtNode) null, AutoShapeType.Rectangle, 4254103, 428484, 2494516, 2113588, false, smartArt);
    SmartArt.SetDefaultPropertiesForTitledPictureBlocks(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.RoundedRectangle, 6416440, 724256, 1182863, 1231127, true, smartArt);
    SmartArt.SetDefaultPropertiesForTitledPictureBlocks(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 4254103, 25469, 2494516, 363951, true, smartArt);
    SmartArt.SetDefaultPropertiesForTitledPictureBlocks((ISmartArtNode) null, AutoShapeType.Rectangle, 2391399, 3279609, 2494516, 2113588, false, smartArt);
    SmartArt.SetDefaultPropertiesForTitledPictureBlocks(smartArt._nodeCollection[2].ChildNodes[0], AutoShapeType.RoundedRectangle, 4553737, 3575380, 1182863, 1231127, true, smartArt);
    SmartArt.SetDefaultPropertiesForTitledPictureBlocks(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 2391399, 2876593, 2494516, 363951, true, smartArt);
  }

  private static void SetDefaultPropertiesForTitledPictureBlocks(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isParent,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    if (autoShapeType == AutoShapeType.RoundedRectangle && isParent)
    {
      smartArtShape.GetGuideList().Add("adj", "val 10000");
      SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, "lt1");
    }
    else
      SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, isParent);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (isParent)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Left, true, 3000);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBendingPictureCaptionList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBendingPictureCaptionList((ISmartArtNode) null, AutoShapeType.Rectangle, 1355328, 661, 2579687, 2063750, false, smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureCaptionList(smartArt._nodeCollection[0], AutoShapeType.RectangularCallout, 1587500, 1858036, 2295921, 722312, true, smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureCaptionList((ISmartArtNode) null, AutoShapeType.Rectangle, 4192984, 661, 2579687, 2063750, false, smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureCaptionList(smartArt._nodeCollection[1], AutoShapeType.RectangularCallout, 4425156, 1858036, 2295921, 722312, true, smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureCaptionList((ISmartArtNode) null, AutoShapeType.Rectangle, 2774156, 2838317, 2579687, 2063750, false, smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureCaptionList(smartArt._nodeCollection[2], AutoShapeType.RectangularCallout, 3006328, 4695692, 2295921, 722312, true, smartArt);
  }

  private static void SetDefaultPropertiesForBendingPictureCaptionList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isParent,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    if (autoShapeType == AutoShapeType.RectangularCallout)
    {
      smartArtShape.GetGuideList().Add("adj1", "val 20250");
      smartArtShape.GetGuideList().Add("adj2", "val -60700");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, isParent);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (isParent)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 3300);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBendingPictureBlocks(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBendingPictureBlocks((ISmartArtNode) null, AutoShapeType.Rectangle, 1325561, 264272, 2517355, 2117275, false, smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureBlocks(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 387756, 1154337, 1364316, 1364316, true, smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureBlocks((ISmartArtNode) null, AutoShapeType.Rectangle, 5222888, 264272, 2517355, 2117275, false, smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureBlocks(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 4285083, 1154337, 1364316, 1364316, true, smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureBlocks((ISmartArtNode) null, AutoShapeType.Rectangle, 3274224, 2900013, 2517355, 2117275, false, smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureBlocks(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 2336419, 3790078, 1364316, 1364316, true, smartArt);
  }

  private static void SetDefaultPropertiesForBendingPictureBlocks(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isParent,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, isParent);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (isParent)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 3700);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBendingPictureSemiTransparentText(
    SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBendingPictureSemiTransparentText(AutoShapeType.Rectangle, 929985, 4361, 2981939, 2555875, smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureSemiTransparentText(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 929985, 1793473, 2981939, 613410, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureSemiTransparentText(AutoShapeType.Rectangle, 4216074, 4361, 2981939, 2555875, smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureSemiTransparentText(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 4216074, 1793473, 2981939, 613410, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureSemiTransparentText(AutoShapeType.Rectangle, 2573030, 2858430, 2981939, 2555875, smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureSemiTransparentText(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 2573030, 4647543, 2981939, 613410, "accent1", smartArt);
  }

  private static void SetDefaultPropertiesForBendingPictureSemiTransparentText(
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    smartArt.DataModel.SmartArtShapeCollection.Add(Guid.NewGuid(), smartArtShape);
  }

  private static void SetDefaultPropertiesForBendingPictureSemiTransparentText(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string colorString,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, false, true, colorString);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 3200);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBendingPictureCaption(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBendingPictureCaption(AutoShapeType.Rectangle, 3756, 1243541, 3609951, 2667740, smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureCaption(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 733427, 3427571, 3110703, 747553, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureCaption(AutoShapeType.Rectangle, 4283868, 1243541, 3609951, 2667740, smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureCaption(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 5013539, 3427571, 3110703, 747553, "accent1", smartArt);
  }

  private static void SetDefaultPropertiesForBendingPictureCaption(
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    smartArt.DataModel.SmartArtShapeCollection.Add(Guid.NewGuid(), smartArtShape);
  }

  private static void SetDefaultPropertiesForBendingPictureCaption(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string colorString,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, colorString);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (smartArtNode != null)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 4200);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForCaptionedPictues(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForCaptionedPictures((ISmartArtNode) null, AutoShapeType.Rectangle, 7143, 1285875, 2419879, 2846916, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForCaptionedPictures((ISmartArtNode) null, AutoShapeType.Rectangle, 128137, 1399751, 2177891, 1850495, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForCaptionedPictures(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 128137, 3534962, 2177891, 483953, smartArt);
    SmartArt.SetDefaultPropertiesForCaptionedPictures(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 128137, 3250247, 2177891, 284714, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForCaptionedPictures((ISmartArtNode) null, AutoShapeType.Rectangle, 2854060, 1285875, 2419879, 2846916, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForCaptionedPictures((ISmartArtNode) null, AutoShapeType.Rectangle, 2975054, 1399751, 2177891, 1850495, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForCaptionedPictures(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.Rectangle, 2975054, 3534962, 2177891, 483953, smartArt);
    SmartArt.SetDefaultPropertiesForCaptionedPictures(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 2975054, 3250247, 2177891, 284714, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForCaptionedPictures((ISmartArtNode) null, AutoShapeType.Rectangle, 5700977, 1285875, 2419879, 2846916, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForCaptionedPictures((ISmartArtNode) null, AutoShapeType.Rectangle, 5821971, 1399751, 2177891, 1850495, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForCaptionedPictures(smartArt._nodeCollection[2].ChildNodes[0], AutoShapeType.Rectangle, 5821971, 3534962, 2177891, 483953, smartArt);
    SmartArt.SetDefaultPropertiesForCaptionedPictures(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 5821971, 3250247, 2177891, 284714, "accent1", smartArt);
  }

  private static void SetDefaultPropertiesForCaptionedPictures(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string colorString,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasLineProperties = true;
    bool hasFillProperties = true;
    if (smartArtNode != null)
    {
      hasLineProperties = false;
      hasFillProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties, colorString);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (smartArtNode != null && smartArtNode.Parent is ISmartArtNode)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 2200);
    else if (smartArtNode != null && smartArtNode.Parent is SmartArt)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, false, 1300);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void SetDefaultPropertiesForCaptionedPictures(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForSpiralPicture(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Rectangle, 0, 26825, 4989465, 4989465, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 4817784, 4844610, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Rectangle, 5094405, 26825, 3033594, 3033594, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 7827559, 2888739, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 7956319, 2888739, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Rectangle, 6276908, 3165198, 1851091, 1851091, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 7763178, 4844610, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 7859749, 4748039, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 7956319, 4651469, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Rectangle, 5094405, 3938566, 1077724, 1077724, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 5871688, 4715849, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 6000449, 4715849, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 5871688, 4844610, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 6000449, 4844610, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Rectangle, 5094405, 3165198, 1077724, 663598, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 5807308, 3657117, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 5903878, 3560547, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 6000449, 3463976, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 5807308, 3463976, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 6000449, 3657117, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 1204996, 5161146, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 2460836, 5161146, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 2589597, 5161146, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 3845437, 5257716, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 3942007, 5161146, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 4038578, 5064575, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 5294418, 5096765, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 5423178, 5096765, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 5294418, 5225526, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 5423178, 5225526, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 6679019, 5257716, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 6775589, 5161146, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 6872159, 5064575, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 6679019, 5064575, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 6872159, 5257716, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(AutoShapeType.Oval, 6872159, 5257716, 85840, 85840, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 4789586, 5016290, 470250, 375551, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 5474437, 5016290, 470250, 375551, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 6223668, 5016290, 470250, 375551, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(smartArt._nodeCollection[3], AutoShapeType.Rectangle, 6908519, 5016290, 470250, 375551, smartArt);
    SmartArt.SetDefaultPropertiesForSpiralPicture(smartArt._nodeCollection[4], AutoShapeType.Rectangle, 7657750, 5016290, 470250, 375551, smartArt);
  }

  private static void SetDefaultPropertiesForSpiralPicture(
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    if (autoShapeType == AutoShapeType.Oval)
      SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, "lt1");
    else
      SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    smartArt.DataModel.SmartArtShapeCollection.Add(Guid.NewGuid(), smartArtShape);
  }

  private static void SetDefaultPropertiesForSpiralPicture(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForSnapshotPictureList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForSnapshotPictureList((ISmartArtNode) null, AutoShapeType.Rectangle, 7934553, 1133689, 193446, 3580281, smartArt);
    SmartArt.SetDefaultPropertiesForSnapshotPictureList((ISmartArtNode) null, AutoShapeType.Frame, 193446, 1133689, 5031232, 3580281, smartArt);
    SmartArt.SetDefaultPropertiesForSnapshotPictureList(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 0, 704696, 4837785, 3386633, true, smartArt);
    SmartArt.SetDefaultPropertiesForSnapshotPictureList(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 5429504, 1133689, 2300224, 3580281, smartArt);
  }

  private static void SetDefaultPropertiesForSnapshotPictureList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (smartArtNode != null && autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    if (autoShapeType == AutoShapeType.Frame)
      smartArtShape.GetGuideList().Add("adj1", "val 5450");
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void SetDefaultPropertiesForSnapshotPictureList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isParent,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, isParent);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForCircularPictureCallOut(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForCircularPictureCallOut((ISmartArtNode) null, AutoShapeType.Line, 2664764, 4131733, 4080256, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCircularPictureCallOut((ISmartArtNode) null, AutoShapeType.Line, 2664764, 2709333, 3495039, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCircularPictureCallOut((ISmartArtNode) null, AutoShapeType.Line, 2664764, 1286933, 4080256, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCircularPictureCallOut((ISmartArtNode) null, AutoShapeType.Oval, 632764, 677333, 4064000, 4064000, smartArt);
    SmartArt.SetDefaultPropertiesForCircularPictureCallOut((ISmartArtNode) null, AutoShapeType.Oval, 6135420, 677333, 1219200, 1219200, smartArt);
    SmartArt.SetDefaultPropertiesForCircularPictureCallOut((ISmartArtNode) null, AutoShapeType.Oval, 5550204, 2099733, 1219200, 1219200, smartArt);
    SmartArt.SetDefaultPropertiesForCircularPictureCallOut((ISmartArtNode) null, AutoShapeType.Oval, 6135420, 3522133, 1219200, 1219200, smartArt);
    SmartArt.SetDefaultPropertiesForCircularPictureCallOut(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 968806, 2835317, 2600960, 1341120, smartArt);
    SmartArt.SetDefaultPropertiesForCircularPictureCallOut(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 6959142, 677333, 931570, 1219200, smartArt);
    SmartArt.SetDefaultPropertiesForCircularPictureCallOut(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 6373926, 2099733, 983234, 1219200, smartArt);
    SmartArt.SetDefaultPropertiesForCircularPictureCallOut(smartArt._nodeCollection[3], AutoShapeType.Rectangle, 6959142, 3522133, 931570, 1219200, smartArt);
  }

  private static void SetDefaultPropertiesForCircularPictureCallOut(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (smartArt._nodeCollection.IndexOf(smartArtNode) == 0)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Left, true, 6500);
    else
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Left, false, 500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  public override ISlideItem Clone()
  {
    SmartArt smartArt = (SmartArt) this.MemberwiseClone();
    this.Clone((Shape) smartArt);
    smartArt._dataModel = this._dataModel.Clone();
    smartArt._nodeCollection = this._nodeCollection.Clone();
    return (ISlideItem) smartArt;
  }

  internal override void SetParent(BaseSlide newParent)
  {
    base.SetParent(newParent);
    foreach (SmartArtPoint point in this._dataModel.PointCollection.GetPointList())
      point.TextBody.SetParent(newParent);
    this._dataModel.SetParent(this);
    this._nodeCollection.SetParent(this);
  }

  internal override void Close()
  {
    base.Close();
    this._dataModel.Close();
    this._nodeCollection.Close();
  }

  private static void InitializeDefaultPropertiesForAccentedPicture(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForAccentedPicture((ISmartArtNode) null, AutoShapeType.RoundedRectangle, 1620631, 369688, 3861645, 4925568, smartArt);
    SmartArt.SetDefaultPropertiesForAccentedPicture(smartArt.Nodes[0], AutoShapeType.Rectangle, 1369939, 2142893, 2973467, 2955340, smartArt);
    SmartArt.SetDefaultPropertiesForAccentedPicture((ISmartArtNode) null, AutoShapeType.Oval, 4817325, 123410, 1329903, 1329903, smartArt);
    SmartArt.SetDefaultPropertiesForAccentedPicture(smartArt.Nodes[1], AutoShapeType.Rectangle, 5742070, 123410, 1170455, 1329903, smartArt);
    SmartArt.SetDefaultPropertiesForAccentedPicture((ISmartArtNode) null, AutoShapeType.Oval, 4817325, 1692696, 1329903, 1329903, smartArt);
    SmartArt.SetDefaultPropertiesForAccentedPicture(smartArt.Nodes[2], AutoShapeType.Rectangle, 5742070, 1692696, 1170455, 1329903, smartArt);
    SmartArt.SetDefaultPropertiesForAccentedPicture((ISmartArtNode) null, AutoShapeType.Oval, 4817325, 3261982, 1329903, 1329903, smartArt);
    SmartArt.SetDefaultPropertiesForAccentedPicture(smartArt.Nodes[3], AutoShapeType.Rectangle, 5742070, 3261982, 1170455, 1329903, smartArt);
  }

  private static void SetDefaultPropertiesForAccentedPicture(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (smartArt._nodeCollection.IndexOf(smartArtNode) == 0)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Left, true, 6500);
    else
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Left, true, 1000);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForGridMatrix(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForGridMatrix((ISmartArtNode) null, AutoShapeType.QuadArrow, 1354666, 0, 5418667, 5418667, smartArt);
    SmartArt.SetDefaultPropertiesForGridMatrix(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 1706879, 352213, 2167466, 2167466, smartArt);
    SmartArt.SetDefaultPropertiesForGridMatrix(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 4253653, 352213, 2167466, 2167466, smartArt);
    SmartArt.SetDefaultPropertiesForGridMatrix(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 1706879, 2898986, 2167466, 2167466, smartArt);
    SmartArt.SetDefaultPropertiesForGridMatrix(smartArt._nodeCollection[3], AutoShapeType.RoundedRectangle, 4253653, 2898986, 2167466, 2167466, smartArt);
  }

  private static void SetDefaultPropertiesForGridMatrix(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    if (autoShapeType == AutoShapeType.QuadArrow)
    {
      smartArtShape.GetGuideList().Add("adj1", "val 2000");
      smartArtShape.GetGuideList().Add("adj2", "val 4000");
      smartArtShape.GetGuideList().Add("adj3", "val 5000");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.RoundedRectangle)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 5300);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForTitledMatrix(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForTitleMatrix(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.RoundSingleCornerRectangle, 677333, -677333, 2709333, 4064000, 16200000, smartArt);
    SmartArt.SetDefaultPropertiesForTitleMatrix(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.RoundSingleCornerRectangle, 4064000, 0, 4064000, 2709333, 0, smartArt);
    SmartArt.SetDefaultPropertiesForTitleMatrix(smartArt._nodeCollection[0].ChildNodes[2], AutoShapeType.RoundSingleCornerRectangle, 0, 2709333, 4064000, 2709333, 10800000, smartArt);
    SmartArt.SetDefaultPropertiesForTitleMatrix(smartArt._nodeCollection[0].ChildNodes[3], AutoShapeType.RoundSingleCornerRectangle, 4741333, 2032000, 2709333, 4064000, 5400000, smartArt);
    SmartArt.SetDefaultPropertiesForTitleMatrix(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 2844799, 2032000, 2438400, 1354666, 0, smartArt);
  }

  private static void SetDefaultPropertiesForTitleMatrix(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.RoundSingleCornerRectangle)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6500);
    else
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, false, 5600);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBasicMartrix(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBasicMatrix((ISmartArtNode) null, AutoShapeType.Diamond, 1354666, 0, 5418667, 5418667, smartArt);
    SmartArt.SetDefaultPropertiesForBasicMatrix(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 1869439, 514773, 2113280, 2113280, smartArt);
    SmartArt.SetDefaultPropertiesForBasicMatrix(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 4145280, 514773, 2113280, 2113280, smartArt);
    SmartArt.SetDefaultPropertiesForBasicMatrix(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 1869439, 2790613, 2113280, 2113280, smartArt);
    SmartArt.SetDefaultPropertiesForBasicMatrix(smartArt._nodeCollection[3], AutoShapeType.RoundedRectangle, 4145280, 2790613, 2113280, 2113280, smartArt);
  }

  private static void SetDefaultPropertiesForBasicMatrix(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.RoundedRectangle)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 5200);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForStackedVenn(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForStackedVenn(smartArt._nodeCollection[0], AutoShapeType.Oval, 1354666, 0, 5418667, 5418667, smartArt);
    SmartArt.SetDefaultPropertiesForStackedVenn(smartArt._nodeCollection[1], AutoShapeType.Oval, 1896533, 1083733, 4334933, 4334933, smartArt);
    SmartArt.SetDefaultPropertiesForStackedVenn(smartArt._nodeCollection[2], AutoShapeType.Oval, 2438399, 2167466, 3251200, 3251200, smartArt);
    SmartArt.SetDefaultPropertiesForStackedVenn(smartArt._nodeCollection[3], AutoShapeType.Oval, 2980266, 3251200, 2167466, 2167466, smartArt);
  }

  private static void SetDefaultPropertiesForStackedVenn(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 2800);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForLinearVenn(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForLinearVenn(smartArt._nodeCollection[0], AutoShapeType.Oval, 2381, 1514739, 2389187, 2389187, smartArt);
    SmartArt.SetDefaultPropertiesForLinearVenn(smartArt._nodeCollection[1], AutoShapeType.Oval, 1913731, 1514739, 2389187, 2389187, smartArt);
    SmartArt.SetDefaultPropertiesForLinearVenn(smartArt._nodeCollection[2], AutoShapeType.Oval, 3825081, 1514739, 2389187, 2389187, smartArt);
    SmartArt.SetDefaultPropertiesForLinearVenn(smartArt._nodeCollection[3], AutoShapeType.Oval, 5736431, 1514739, 2389187, 2389187, smartArt);
  }

  private static void SetDefaultPropertiesForLinearVenn(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBasicVenn(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBasicVenn(smartArt._nodeCollection[0], AutoShapeType.Oval, 2438399, 67733, 3251200, 3251200, smartArt);
    SmartArt.SetDefaultPropertiesForBasicVenn(smartArt._nodeCollection[1], AutoShapeType.Oval, 3611541, 2099733, 3251200, 3251200, smartArt);
    SmartArt.SetDefaultPropertiesForBasicVenn(smartArt._nodeCollection[2], AutoShapeType.Oval, 1265258, 2099733, 3251200, 3251200, smartArt);
  }

  private static void SetDefaultPropertiesForBasicVenn(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBasicTarget(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBasicTarget((ISmartArtNode) null, AutoShapeType.Oval, 677333, 1354666, 4064000, 4064000, smartArt);
    SmartArt.SetDefaultPropertiesForBasicTarget((ISmartArtNode) null, AutoShapeType.Oval, 1490133, 2167466, 2438400, 2438400, smartArt);
    SmartArt.SetDefaultPropertiesForBasicTarget((ISmartArtNode) null, AutoShapeType.Oval, 2302933, 2980266, 812800, 812800, smartArt);
    SmartArt.SetDefaultPropertiesForBasicTarget(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 5418666, 0, 2032000, 1185333, smartArt);
    SmartArt.SetDefaultPropertiesForBasicTarget(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 5418666, 1185333, 2032000, 1185333, smartArt);
    SmartArt.SetDefaultPropertiesForBasicTarget(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 5418666, 2370666, 2032000, 1185333, smartArt);
  }

  private static void SetDefaultPropertiesForBasicTarget(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForRadialList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForRadialList((ISmartArtNode) null, AutoShapeType.Oval, 469562, 1407583, 2603499, 2603499, smartArt);
    SmartArt.SetDefaultPropertiesForRadialList(smartArt._nodeCollection[0], AutoShapeType.Oval, 3080318, 86, 1562100, 1562100, smartArt);
    SmartArt.SetDefaultPropertiesForRadialList(smartArt._nodeCollection[1], AutoShapeType.Oval, 3596977, 1928283, 1562100, 1562100, smartArt);
    SmartArt.SetDefaultPropertiesForRadialList(smartArt._nodeCollection[2], AutoShapeType.Oval, 3080318, 3856480, 1562100, 1562100, smartArt);
    SmartArt.SetDefaultPropertiesForRadialList(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 4798628, 86, 2343150, 1562100, smartArt);
    SmartArt.SetDefaultPropertiesForRadialList(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 5315287, 1928283, 2343150, 1562100, smartArt);
    SmartArt.SetDefaultPropertiesForRadialList(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 4798628, 3856480, 2343150, 1562100, smartArt);
  }

  private static void SetDefaultPropertiesForRadialList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    Guid key = Guid.NewGuid();
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (smartArtNode != null)
    {
      SmartArtNode smartArtNode1 = (SmartArtNode) smartArtNode;
      if (autoShapeType == AutoShapeType.Rectangle)
      {
        SmartArtNode childNode1 = (SmartArtNode) smartArtNode1.ChildNodes[0];
        key = childNode1.Id;
        if (childNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
        SmartArtNode childNode2 = (SmartArtNode) smartArtNode1.ChildNodes[1];
        if (childNode2.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[1] as Paragraph).NodeId = childNode2.Id;
        hasFillProperties = false;
        hasLineProperties = false;
      }
      else
      {
        key = smartArtNode1.Id;
        if (smartArtNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
      }
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    if (autoShapeType == AutoShapeType.Oval)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 3600);
    else
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 5100);
    smartArt.DataModel.SmartArtShapeCollection.Add(key, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForConvergingRadial(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForConvergingRadial(smartArt._nodeCollection[0], AutoShapeType.Oval, 2874010, 3036805, 2379980, 2379980, 0, smartArt);
    SmartArt.SetDefaultPropertiesForConvergingRadial((ISmartArtNode) null, AutoShapeType.LeftArrow, 1161933, 2560481, 2013351, 678294, 12900000, smartArt);
    SmartArt.SetDefaultPropertiesForConvergingRadial(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.RoundedRectangle, 213498, 1417830, 2260981, 1808784, 0, smartArt);
    SmartArt.SetDefaultPropertiesForConvergingRadial((ISmartArtNode) null, AutoShapeType.LeftArrow, 3057324, 1573802, 2013351, 678294, 16200000, smartArt);
    SmartArt.SetDefaultPropertiesForConvergingRadial(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.RoundedRectangle, 2933509, 1881, 2260981, 1808784, 0, smartArt);
    SmartArt.SetDefaultPropertiesForConvergingRadial((ISmartArtNode) null, AutoShapeType.LeftArrow, 4952715, 2560481, 2013351, 678294, 19500000, smartArt);
    SmartArt.SetDefaultPropertiesForConvergingRadial(smartArt._nodeCollection[0].ChildNodes[2], AutoShapeType.RoundedRectangle, 5653520, 1417830, 2260981, 1808784, 0, smartArt);
  }

  private static void SetDefaultPropertiesForConvergingRadial(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    switch (autoShapeType)
    {
      case AutoShapeType.RoundedRectangle:
        smartArtShape.GetGuideList().Add("adj", "val 10000");
        break;
      case AutoShapeType.LeftArrow:
        smartArtShape.GetGuideList().Add("adj1", "val 60000");
        smartArtShape.GetGuideList().Add("adj2", "val 50000");
        break;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType != AutoShapeType.LeftArrow)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 5600);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForNestedTarget(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForNestedTarget(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 0, 0, 8128000, 5418667, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForNestedTarget(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.RoundedRectangle, 203200, 1354666, 1219200, 1865048, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForNestedTarget(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.RoundedRectangle, 203200, 3279662, 1219200, 1865048, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForNestedTarget(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 1625600, 1354666, 6299200, 3793066, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForNestedTarget(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.RoundedRectangle, 1783080, 2682240, 1259840, 1060688, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForNestedTarget(smartArt._nodeCollection[1].ChildNodes[1], AutoShapeType.RoundedRectangle, 1783080, 3802221, 1259840, 1060688, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForNestedTarget(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 3210560, 2709333, 4511040, 2167466, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForNestedTarget(smartArt._nodeCollection[2].ChildNodes[0], AutoShapeType.RoundedRectangle, 3323336, 3684693, 2111356, 975360, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForNestedTarget(smartArt._nodeCollection[2].ChildNodes[1], AutoShapeType.RoundedRectangle, 5494759, 3684693, 2111356, 975360, "lt1", smartArt);
  }

  private static void SetDefaultPropertiesForNestedTarget(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string colorString,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    switch (colorString)
    {
      case "accent1":
        smartArtShape.GetGuideList().Add("adj", "val 8500");
        break;
      case "lt1":
        smartArtShape.GetGuideList().Add("adj", "val 10500");
        break;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, colorString);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (colorString == "accent1")
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Left, true, 5300);
    else
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, false, 3100);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForSegmentedPyramid(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForSegmentedPyramid(smartArt._nodeCollection[0], AutoShapeType.IsoscelesTriangle, 2709333, 0, 2709333, 2709333, 0, smartArt);
    SmartArt.SetDefaultPropertiesForSegmentedPyramid(smartArt._nodeCollection[1], AutoShapeType.IsoscelesTriangle, 1354666, 2709333, 2709333, 2709333, 0, smartArt);
    SmartArt.SetDefaultPropertiesForSegmentedPyramid(smartArt._nodeCollection[2], AutoShapeType.IsoscelesTriangle, 2709333, 2709333, 2709333, 2709333, 10800000, smartArt);
    SmartArt.SetDefaultPropertiesForSegmentedPyramid(smartArt._nodeCollection[3], AutoShapeType.IsoscelesTriangle, 4064000, 2709333, 2709333, 2709333, 0, smartArt);
  }

  private static void SetDefaultPropertiesForSegmentedPyramid(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 3700);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForCounterBalanceArrows(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForCounterBalanceArrows((ISmartArtNode) null, AutoShapeType.MathMinus, 24942, 2246800, 8078114, 925066, 21300000, smartArt);
    SmartArt.SetDefaultPropertiesForCounterBalanceArrows((ISmartArtNode) null, AutoShapeType.DownArrow, 975360, 270933, 2438400, 2167466, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCounterBalanceArrows((ISmartArtNode) null, AutoShapeType.UpArrow, 4714239, 2980266, 2438400, 2167466, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCounterBalanceArrows(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 4307840, 0, 2600960, 0, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCounterBalanceArrows(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 1219200, 3142826, 2600960, 2275840, 0, smartArt);
  }

  private static void SetDefaultPropertiesForCounterBalanceArrows(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForReverseList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForReverseList(smartArt._nodeCollection[0], AutoShapeType.RoundSameSideCornerRectangle, 1209392, 1645161, 3483661, 2128886, 16200000, smartArt);
    SmartArt.SetDefaultPropertiesForReverseList(smartArt._nodeCollection[1], AutoShapeType.RoundSameSideCornerRectangle, 3434946, 1645161, 3483661, 2128886, 5400000, smartArt);
    SmartArt.SetDefaultPropertiesForReverseList((ISmartArtNode) null, AutoShapeType.CircularArrow, 2951004, 0, 2225554, 2225446, 0, smartArt);
    SmartArt.SetDefaultPropertiesForReverseList((ISmartArtNode) null, AutoShapeType.CircularArrow, 2951004, 3193220, 2225554, 2225446, 10800000, smartArt);
  }

  private static void SetDefaultPropertiesForReverseList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    switch (autoShapeType)
    {
      case AutoShapeType.CircularArrow:
        smartArtShape.GetGuideList().Add("adj1", "val 12500");
        smartArtShape.GetGuideList().Add("adj2", "val 1142322");
        smartArtShape.GetGuideList().Add("adj3", "val 20457678");
        smartArtShape.GetGuideList().Add("adj4", "val 10800000");
        smartArtShape.GetGuideList().Add("adj5", "val 12500");
        break;
      case AutoShapeType.RoundSameSideCornerRectangle:
        smartArtShape.GetGuideList().Add("adj1", "val 16670");
        smartArtShape.GetGuideList().Add("adj2", "val 0");
        break;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForPlusAndMinus(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForPlusAndMinus((ISmartArtNode) null, AutoShapeType.Rectangle, 731520, 1247782, 7071360, 3654435, smartArt);
    SmartArt.SetDefaultPropertiesForPlusAndMinus((ISmartArtNode) null, AutoShapeType.MathPlus, 0, 516449, 1381760, 1381760, smartArt);
    SmartArt.SetDefaultPropertiesForPlusAndMinus((ISmartArtNode) null, AutoShapeType.Rectangle, 6827520, 1013363, 1300480, 445662, smartArt);
    SmartArt.SetDefaultPropertiesForPlusAndMinus(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 942848, 1675172, 3283712, 3126325, smartArt);
    SmartArt.SetDefaultPropertiesForPlusAndMinus(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 4299712, 1675172, 3283712, 3126325, smartArt);
  }

  private static void SetDefaultPropertiesForPlusAndMinus(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (smartArtNode != null && autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForOpposingIdeas(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForOpposingIdeas((ISmartArtNode) null, AutoShapeType.RoundDiagonalCornerRectangle, 1015999, 1070223, 6096000, 3278220, 0, smartArt);
    SmartArt.SetDefaultPropertiesForOpposingIdeas((ISmartArtNode) null, AutoShapeType.Line, 4063999, 1417913, 812, 2582840, 0, smartArt);
    SmartArt.SetDefaultPropertiesForOpposingIdeas(smartArt._nodeCollection[0], AutoShapeType.RightArrow, -1280120, 1505953, 3576240, 1016000, 16200000, smartArt);
    SmartArt.SetDefaultPropertiesForOpposingIdeas(smartArt._nodeCollection[1], AutoShapeType.RightArrow, 5831879, 2896713, 3576240, 1016000, 5400000, smartArt);
    SmartArt.SetDefaultPropertiesForOpposingIdeas(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 1219199, 1318573, 2641600, 2781520, 0, smartArt);
    SmartArt.SetDefaultPropertiesForOpposingIdeas(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.Rectangle, 4267199, 1318573, 2641600, 2781520, 0, smartArt);
  }

  private static void SetDefaultPropertiesForOpposingIdeas(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    if (autoShapeType == AutoShapeType.RoundDiagonalCornerRectangle)
    {
      smartArtShape.GetGuideList().Add("adj1", "val 0");
      smartArtShape.GetGuideList().Add("adj2", "val 16670");
    }
    else if (autoShapeType == AutoShapeType.RightArrow)
    {
      smartArtShape.GetGuideList().Add("adj1", "val 49830");
      smartArtShape.GetGuideList().Add("adj2", "val 60660");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForHexagonCluster(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForHexagonCluster(smartArt._nodeCollection[0], AutoShapeType.Hexagon, 1952345, 3341447, 2283968, 1969178, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForHexagonCluster((ISmartArtNode) null, AutoShapeType.Hexagon, 2011680, 4210799, 267411, 230474, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForHexagonCluster((ISmartArtNode) null, AutoShapeType.Hexagon, 0, 2283761, 2283968, 1969178, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForHexagonCluster((ISmartArtNode) null, AutoShapeType.Hexagon, 1554886, 3992811, 267411, 230474, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForHexagonCluster(smartArt._nodeCollection[1], AutoShapeType.Hexagon, 3898188, 2260350, 2283968, 1969178, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForHexagonCluster((ISmartArtNode) null, AutoShapeType.Hexagon, 5459577, 3967318, 267411, 230474, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForHexagonCluster((ISmartArtNode) null, AutoShapeType.Hexagon, 5844032, 3341447, 2283968, 1969178, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForHexagonCluster((ISmartArtNode) null, AutoShapeType.Hexagon, 5903366, 4210799, 267411, 230474, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForHexagonCluster(smartArt._nodeCollection[2], AutoShapeType.Hexagon, 1952345, 1183935, 2283968, 1969178, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForHexagonCluster((ISmartArtNode) null, AutoShapeType.Hexagon, 3500729, 1226596, 267411, 230474, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForHexagonCluster((ISmartArtNode) null, AutoShapeType.Hexagon, 3898188, 108040, 2283968, 1969178, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForHexagonCluster((ISmartArtNode) null, AutoShapeType.Hexagon, 3965651, 972710, 267411, 230474, "lt1", smartArt);
  }

  private static void SetDefaultPropertiesForHexagonCluster(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string colorString,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    smartArtShape.GetGuideList().Add("adj", "val 25000");
    smartArtShape.GetGuideList().Add("vf", "val 115470");
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, colorString);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (smartArtNode != null)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 5400);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForCircleRelationship(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForCircleRelationship(smartArt._nodeCollection[0], AutoShapeType.Oval, 1532940, 355685, 4370425, 4370331, smartArt);
    SmartArt.SetDefaultPropertiesForCircleRelationship((ISmartArtNode) null, AutoShapeType.Oval, 4026611, 156569, 486054, 486046, smartArt);
    SmartArt.SetDefaultPropertiesForCircleRelationship((ISmartArtNode) null, AutoShapeType.Oval, 2875686, 4401305, 351942, 352281, smartArt);
    SmartArt.SetDefaultPropertiesForCircleRelationship((ISmartArtNode) null, AutoShapeType.Oval, 6184595, 2129345, 351942, 352281, smartArt);
    SmartArt.SetDefaultPropertiesForCircleRelationship((ISmartArtNode) null, AutoShapeType.Oval, 4500473, 4776051, 486054, 486046, smartArt);
    SmartArt.SetDefaultPropertiesForCircleRelationship((ISmartArtNode) null, AutoShapeType.Oval, 2975660, 847347, 351942, 352281, smartArt);
    SmartArt.SetDefaultPropertiesForCircleRelationship((ISmartArtNode) null, AutoShapeType.Oval, 1866188, 2862499, 351942, 352281, smartArt);
    SmartArt.SetDefaultPropertiesForCircleRelationship(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Oval, 167436, 1144489, 1776780, 1776213, smartArt);
    SmartArt.SetDefaultPropertiesForCircleRelationship((ISmartArtNode) null, AutoShapeType.Oval, 3534867, 862664, 486054, 486046, smartArt);
    SmartArt.SetDefaultPropertiesForCircleRelationship((ISmartArtNode) null, AutoShapeType.Oval, 334060, 3441466, 878636, 878661, smartArt);
    SmartArt.SetDefaultPropertiesForCircleRelationship(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.Oval, 6351219, 308714, 1776780, 1776213, smartArt);
    SmartArt.SetDefaultPropertiesForCircleRelationship((ISmartArtNode) null, AutoShapeType.Oval, 5558739, 1535062, 486054, 486046, smartArt);
    SmartArt.SetDefaultPropertiesForCircleRelationship((ISmartArtNode) null, AutoShapeType.Oval, 0, 4487078, 351942, 352281, smartArt);
    SmartArt.SetDefaultPropertiesForCircleRelationship((ISmartArtNode) null, AutoShapeType.Oval, 3509670, 3985715, 351942, 352281, smartArt);
  }

  private static void SetDefaultPropertiesForCircleRelationship(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (smartArtNode != null)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBalance(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBalance(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 1679786, 0, 1950720, 1083733, false, smartArt);
    SmartArt.SetDefaultPropertiesForBalance(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 4497493, 0, 1950720, 1083733, false, smartArt);
    SmartArt.SetDefaultPropertiesForBalance((ISmartArtNode) null, AutoShapeType.IsoscelesTriangle, 3657599, 4605866, 812800, 812800, false, smartArt);
    SmartArt.SetDefaultPropertiesForBalance((ISmartArtNode) null, AutoShapeType.Rectangle, 1624855, 4257573, 4878289, 341123, false, smartArt);
    SmartArt.SetDefaultPropertiesForBalance(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.RoundedRectangle, 4553844, 3404681, 1946391, 906819, true, smartArt);
    SmartArt.SetDefaultPropertiesForBalance(smartArt._nodeCollection[1].ChildNodes[1], AutoShapeType.RoundedRectangle, 4624286, 2429321, 1946391, 906819, true, smartArt);
    SmartArt.SetDefaultPropertiesForBalance(smartArt._nodeCollection[1].ChildNodes[2], AutoShapeType.RoundedRectangle, 4694729, 1475635, 1946391, 906819, true, smartArt);
    SmartArt.SetDefaultPropertiesForBalance(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.RoundedRectangle, 1763230, 3209609, 1946391, 906819, true, smartArt);
    SmartArt.SetDefaultPropertiesForBalance(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.RoundedRectangle, 1833673, 2234249, 1946391, 906819, true, smartArt);
  }

  private static void SetDefaultPropertiesForBalance(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isChild,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    int rotation = 0;
    if (autoShapeType == AutoShapeType.RoundedRectangle)
    {
      smartArtShape.GetGuideList().Add("adj", "val 10000");
      if (isChild)
        rotation = 240000;
    }
    if (autoShapeType == AutoShapeType.Rectangle)
      rotation = 240000;
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, isChild);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.RoundedRectangle)
    {
      if (isChild)
        SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 3700);
      else
        SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, false, 3700);
    }
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForHorizontalLabeledHierarchy(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForHorizontalLabeledHierarchy(smartArt._nodeCollection[3], AutoShapeType.RoundedRectangle, 5689978, 0, 2435981, 5418667, true, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalLabeledHierarchy(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 2846009, 0, 2435981, 5418667, true, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalLabeledHierarchy(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 2039, 0, 2435981, 5418667, true, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalLabeledHierarchy(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 206033, 3197016, 2039937, 1019968, false, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalLabeledHierarchy(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.RoundedRectangle, 3061946, 2317293, 2039937, 1019968, false, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalLabeledHierarchy(smartArt._nodeCollection[0].ChildNodes[0].ChildNodes[0], AutoShapeType.RoundedRectangle, 5917858, 1730811, 2039937, 1019968, false, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalLabeledHierarchy(smartArt._nodeCollection[0].ChildNodes[0].ChildNodes[1], AutoShapeType.RoundedRectangle, 5917858, 2903775, 2039937, 1019968, false, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalLabeledHierarchy(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.RoundedRectangle, 3061946, 4076739, 2039937, 1019968, false, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalLabeledHierarchy(smartArt._nodeCollection[0].ChildNodes[1].ChildNodes[0], AutoShapeType.RoundedRectangle, 5917858, 4076739, 2039937, 1019968, false, smartArt);
  }

  private static void SetDefaultPropertiesForHorizontalLabeledHierarchy(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isParent,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    smartArtShape.GetGuideList().Add("adj", "val 10000");
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, isParent);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (isParent)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6300);
    else
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, false, 6300);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForHorizontalHierarchy(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForHorizontalHierarchy(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 2116, 2482188, 2137833, 1068916, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalHierarchy(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.RoundedRectangle, 2995083, 1560248, 2137833, 1068916, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalHierarchy(smartArt._nodeCollection[0].ChildNodes[0].ChildNodes[0], AutoShapeType.RoundedRectangle, 5988050, 945620, 2137833, 1068916, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalHierarchy(smartArt._nodeCollection[0].ChildNodes[0].ChildNodes[1], AutoShapeType.RoundedRectangle, 5988050, 2174875, 2137833, 1068916, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalHierarchy(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.RoundedRectangle, 2995083, 3404129, 2137833, 1068916, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalHierarchy(smartArt._nodeCollection[0].ChildNodes[1].ChildNodes[0], AutoShapeType.RoundedRectangle, 5988050, 3404129, 2137833, 1068916, smartArt);
  }

  private static void SetDefaultPropertiesForHorizontalHierarchy(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.RoundedRectangle)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForHorizontalMultiLevelHierarchy(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForHorizontalMultiLevelHierarchy(smartArt._nodeCollection[0], AutoShapeType.Rectangle, -671481, 2194560, 5418667, 1029546, 16200000, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalMultiLevelHierarchy(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 3228008, 907626, 3376913, 1029546, 0, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalMultiLevelHierarchy(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.Rectangle, 3228008, 2194560, 3376913, 1029546, 0, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalMultiLevelHierarchy(smartArt._nodeCollection[0].ChildNodes[2], AutoShapeType.Rectangle, 3228008, 3481493, 3376913, 1029546, 0, smartArt);
  }

  private static void SetDefaultPropertiesForHorizontalMultiLevelHierarchy(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.Rectangle)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForHorizontalOrganizationChart(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForHorizontalOrganizationChart(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 3224, 2345058, 2388691, 728550, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalOrganizationChart(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.Rectangle, 5736083, 1317920, 2388691, 728550, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalOrganizationChart(smartArt._nodeCollection[0].ChildNodes[2], AutoShapeType.Rectangle, 5736083, 2345058, 2388691, 728550, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalOrganizationChart(smartArt._nodeCollection[0].ChildNodes[3], AutoShapeType.Rectangle, 5736083, 3372195, 2388691, 728550, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalOrganizationChart(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 2869654, 1831489, 2388691, 728550, smartArt);
  }

  private static void SetDefaultPropertiesForHorizontalOrganizationChart(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.Rectangle)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 4700);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForLabeledHierarchy(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForLabeledHierarchy(smartArt._nodeCollection[3], AutoShapeType.RoundedRectangle, 0, 3527425, 8128000, 1227137, true, smartArt);
    SmartArt.SetDefaultPropertiesForLabeledHierarchy(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 0, 2095764, 8128000, 1227137, true, smartArt);
    SmartArt.SetDefaultPropertiesForLabeledHierarchy(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 0, 664104, 8128000, 1227137, true, smartArt);
    SmartArt.SetDefaultPropertiesForLabeledHierarchy(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 4933483, 766365, 1533921, 1022614, false, smartArt);
    SmartArt.SetDefaultPropertiesForLabeledHierarchy(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.RoundedRectangle, 3437909, 2198026, 1533921, 1022614, false, smartArt);
    SmartArt.SetDefaultPropertiesForLabeledHierarchy(smartArt._nodeCollection[0].ChildNodes[0].ChildNodes[0], AutoShapeType.RoundedRectangle, 2440860, 3629686, 1533921, 1022614, false, smartArt);
    SmartArt.SetDefaultPropertiesForLabeledHierarchy(smartArt._nodeCollection[0].ChildNodes[0].ChildNodes[1], AutoShapeType.RoundedRectangle, 4434959, 3629686, 1533921, 1022614, false, smartArt);
    SmartArt.SetDefaultPropertiesForLabeledHierarchy(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.RoundedRectangle, 6429057, 2198026, 1533921, 1022614, false, smartArt);
    SmartArt.SetDefaultPropertiesForLabeledHierarchy(smartArt._nodeCollection[0].ChildNodes[1].ChildNodes[0], AutoShapeType.RoundedRectangle, 6429057, 3629686, 1533921, 1022614, false, smartArt);
  }

  private static void SetDefaultPropertiesForLabeledHierarchy(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isParent,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    smartArtShape.GetGuideList().Add("adj", "val 10000");
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, isParent);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (isParent)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 4000);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForHierarchy(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForHierarchy((ISmartArtNode) null, AutoShapeType.RoundedRectangle, 3541613, 1048, 2089546, 1326862, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForHierarchy(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 3773785, 221611, 2089546, 1326862, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForHierarchy((ISmartArtNode) null, AutoShapeType.RoundedRectangle, 1626195, 1935620, 2089546, 1326862, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForHierarchy(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.RoundedRectangle, 1858367, 2156184, 2089546, 1326862, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForHierarchy((ISmartArtNode) null, AutoShapeType.RoundedRectangle, 349250, 3870192, 2089546, 1326862, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForHierarchy(smartArt._nodeCollection[0].ChildNodes[0].ChildNodes[0], AutoShapeType.RoundedRectangle, 581421, 4090756, 2089546, 1326862, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForHierarchy((ISmartArtNode) null, AutoShapeType.RoundedRectangle, 2903140, 3870192, 2089546, 1326862, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForHierarchy(smartArt._nodeCollection[0].ChildNodes[0].ChildNodes[1], AutoShapeType.RoundedRectangle, 3135312, 4090756, 2089546, 1326862, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForHierarchy((ISmartArtNode) null, AutoShapeType.RoundedRectangle, 5457031, 1935620, 2089546, 1326862, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForHierarchy(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.RoundedRectangle, 5689203, 2156184, 2089546, 1326862, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForHierarchy((ISmartArtNode) null, AutoShapeType.RoundedRectangle, 5457031, 3870192, 2089546, 1326862, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForHierarchy(smartArt._nodeCollection[0].ChildNodes[1].ChildNodes[0], AutoShapeType.RoundedRectangle, 5689203, 4090756, 2089546, 1326862, "lt1", smartArt);
  }

  private static void SetDefaultPropertiesForHierarchy(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string colorString,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, colorString);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForCirclePictureHierarchy(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForCirclePictureHierarchy((ISmartArtNode) null, AutoShapeType.Oval, 3492499, 866563, 1016000, 1016000, smartArt);
    SmartArt.SetDefaultPropertiesForCirclePictureHierarchy((ISmartArtNode) null, AutoShapeType.Oval, 1396999, 2202603, 1016000, 1016000, smartArt);
    SmartArt.SetDefaultPropertiesForCirclePictureHierarchy((ISmartArtNode) null, AutoShapeType.Oval, 0, 3538643, 1016000, 1016000, smartArt);
    SmartArt.SetDefaultPropertiesForCirclePictureHierarchy((ISmartArtNode) null, AutoShapeType.Oval, 2793999, 3538643, 1016000, 1016000, smartArt);
    SmartArt.SetDefaultPropertiesForCirclePictureHierarchy((ISmartArtNode) null, AutoShapeType.Oval, 5587999, 2202603, 1016000, 1016000, smartArt);
    SmartArt.SetDefaultPropertiesForCirclePictureHierarchy((ISmartArtNode) null, AutoShapeType.Oval, 5587999, 3538643, 1016000, 1016000, smartArt);
    SmartArt.SetDefaultPropertiesForCirclePictureHierarchy(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 4508499, 864023, 1524000, 1016000, smartArt);
    SmartArt.SetDefaultPropertiesForCirclePictureHierarchy(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 2412999, 2200063, 1524000, 1016000, smartArt);
    SmartArt.SetDefaultPropertiesForCirclePictureHierarchy(smartArt._nodeCollection[0].ChildNodes[0].ChildNodes[0], AutoShapeType.Rectangle, 1015999, 3536103, 1524000, 1016000, smartArt);
    SmartArt.SetDefaultPropertiesForCirclePictureHierarchy(smartArt._nodeCollection[0].ChildNodes[0].ChildNodes[1], AutoShapeType.Rectangle, 3809999, 3536103, 1524000, 1016000, smartArt);
    SmartArt.SetDefaultPropertiesForCirclePictureHierarchy(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.Rectangle, 6604000, 2200063, 1524000, 1016000, smartArt);
    SmartArt.SetDefaultPropertiesForCirclePictureHierarchy(smartArt._nodeCollection[0].ChildNodes[1].ChildNodes[0], AutoShapeType.Rectangle, 6604000, 3536103, 1524000, 1016000, smartArt);
  }

  private static void SetDefaultPropertiesForCirclePictureHierarchy(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForHalfCircleOrganizationChart(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForHalfCircleOrganizationChart((ISmartArtNode) null, AutoShapeType.Arc, 3469927, 428096, 1188144, 1188144, "arc1", smartArt);
    SmartArt.SetDefaultPropertiesForHalfCircleOrganizationChart((ISmartArtNode) null, AutoShapeType.Arc, 3469927, 428096, 1188144, 1188144, "arc2", smartArt);
    SmartArt.SetDefaultPropertiesForHalfCircleOrganizationChart((ISmartArtNode) null, AutoShapeType.Arc, 594617, 3802426, 1188144, 1188144, "arc1", smartArt);
    SmartArt.SetDefaultPropertiesForHalfCircleOrganizationChart((ISmartArtNode) null, AutoShapeType.Arc, 594617, 3802426, 1188144, 1188144, "arc2", smartArt);
    SmartArt.SetDefaultPropertiesForHalfCircleOrganizationChart((ISmartArtNode) null, AutoShapeType.Arc, 3469927, 428096, 1188144, 1188144, "arc1", smartArt);
    SmartArt.SetDefaultPropertiesForHalfCircleOrganizationChart((ISmartArtNode) null, AutoShapeType.Arc, 594617, 3802426, 1188144, 1188144, "arc2", smartArt);
    SmartArt.SetDefaultPropertiesForHalfCircleOrganizationChart((ISmartArtNode) null, AutoShapeType.Arc, 3469927, 3802426, 1188144, 1188144, "arc1", smartArt);
    SmartArt.SetDefaultPropertiesForHalfCircleOrganizationChart((ISmartArtNode) null, AutoShapeType.Arc, 3469927, 3802426, 1188144, 1188144, "arc2", smartArt);
    SmartArt.SetDefaultPropertiesForHalfCircleOrganizationChart((ISmartArtNode) null, AutoShapeType.Arc, 6345237, 3802426, 1188144, 1188144, "arc1", smartArt);
    SmartArt.SetDefaultPropertiesForHalfCircleOrganizationChart((ISmartArtNode) null, AutoShapeType.Arc, 6345237, 3802426, 1188144, 1188144, "arc2", smartArt);
    SmartArt.SetDefaultPropertiesForHalfCircleOrganizationChart((ISmartArtNode) null, AutoShapeType.Arc, 2032272, 2115261, 1188144, 1188144, "arc1", smartArt);
    SmartArt.SetDefaultPropertiesForHalfCircleOrganizationChart((ISmartArtNode) null, AutoShapeType.Arc, 2032272, 2115261, 1188144, 1188144, "arc2", smartArt);
    SmartArt.SetDefaultPropertiesForHalfCircleOrganizationChart(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 2875855, 641962, 2376289, 760412, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForHalfCircleOrganizationChart(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.Rectangle, 545, 4016292, 2376289, 760412, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForHalfCircleOrganizationChart(smartArt._nodeCollection[0].ChildNodes[2], AutoShapeType.Rectangle, 2875855, 4016292, 2376289, 760412, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForHalfCircleOrganizationChart(smartArt._nodeCollection[0].ChildNodes[3], AutoShapeType.Rectangle, 5751165, 4016292, 2376289, 760412, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForHalfCircleOrganizationChart(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 1438200, 2329127, 2376289, 760412, (string) null, smartArt);
  }

  private static void SetDefaultPropertiesForHalfCircleOrganizationChart(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string artName,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
      hasLineProperties = false;
    switch (artName)
    {
      case "arc1":
        smartArtShape.GetGuideList().Add("adj1", "val 13200000");
        smartArtShape.GetGuideList().Add("adj2", "val 19200000");
        break;
      case "arc2":
        smartArtShape.GetGuideList().Add("adj1", "val 2400000");
        smartArtShape.GetGuideList().Add("adj2", "val 8400000");
        break;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, false);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForNameAndTitleOrganizationChart(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForNameAndTitleOrganizationChart(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 2905546, 382852, 2106279, 1090538, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForNameAndTitleOrganizationChart((ISmartArtNode) null, AutoShapeType.Rectangle, 3326802, 1231048, 1895651, 363512, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForNameAndTitleOrganizationChart(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.Rectangle, 79721, 3824105, 2106279, 1090538, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForNameAndTitleOrganizationChart((ISmartArtNode) null, AutoShapeType.Rectangle, 500977, 4672302, 1895651, 363512, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForNameAndTitleOrganizationChart(smartArt._nodeCollection[0].ChildNodes[2], AutoShapeType.Rectangle, 2905546, 3824105, 2106279, 1090538, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForNameAndTitleOrganizationChart((ISmartArtNode) null, AutoShapeType.Rectangle, 3326802, 4672302, 1895651, 363512, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForNameAndTitleOrganizationChart(smartArt._nodeCollection[0].ChildNodes[3], AutoShapeType.Rectangle, 5731371, 3824105, 2106279, 1090538, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForNameAndTitleOrganizationChart((ISmartArtNode) null, AutoShapeType.Rectangle, 6152627, 4672302, 1895651, 363512, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForNameAndTitleOrganizationChart(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 1492633, 2103479, 2106279, 1090538, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForNameAndTitleOrganizationChart((ISmartArtNode) null, AutoShapeType.Rectangle, 1913889, 2951675, 1895651, 363512, "lt1", smartArt);
  }

  private static void SetDefaultPropertiesForNameAndTitleOrganizationChart(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string colorString,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, colorString);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (colorString == "accent1")
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6400);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForOrganizationChart(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForOrganizationChart(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 2875855, 428096, 2376289, 1188144, smartArt);
    SmartArt.SetDefaultPropertiesForOrganizationChart(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.Rectangle, 545, 3802426, 2376289, 1188144, smartArt);
    SmartArt.SetDefaultPropertiesForOrganizationChart(smartArt._nodeCollection[0].ChildNodes[2], AutoShapeType.Rectangle, 2875855, 3802426, 2376289, 1188144, smartArt);
    SmartArt.SetDefaultPropertiesForOrganizationChart(smartArt._nodeCollection[0].ChildNodes[3], AutoShapeType.Rectangle, 5751165, 3802426, 2376289, 1188144, smartArt);
    SmartArt.SetDefaultPropertiesForOrganizationChart(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 1438200, 2115261, 2376289, 1188144, smartArt);
  }

  private static void SetDefaultPropertiesForOrganizationChart(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.Rectangle)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForRadialCluster(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForRadialCluster(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 3251199, 2520950, 1625600, 1625600, smartArt);
    SmartArt.SetDefaultPropertiesForRadialCluster(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.RoundedRectangle, 3519423, 291507, 1089152, 1089152, smartArt);
    SmartArt.SetDefaultPropertiesForRadialCluster(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.RoundedRectangle, 5682466, 4038007, 1089152, 1089152, smartArt);
    SmartArt.SetDefaultPropertiesForRadialCluster(smartArt._nodeCollection[0].ChildNodes[2], AutoShapeType.RoundedRectangle, 1356381, 4038007, 1089152, 1089152, smartArt);
  }

  private static void SetDefaultPropertiesForRadialCluster(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.RoundedRectangle)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 3600);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForCycleMatrix(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForCycleMatrix(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.RoundedRectangle, 4909312, 3684693, 2676821, 1733973, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCycleMatrix(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.RoundedRectangle, 541866, 3684693, 2676821, 1733973, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCycleMatrix(smartArt._nodeCollection[2].ChildNodes[0], AutoShapeType.RoundedRectangle, 4909312, 0, 2676821, 1733973, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCycleMatrix(smartArt._nodeCollection[3].ChildNodes[0], AutoShapeType.RoundedRectangle, 541866, 0, 2676821, 1733973, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCycleMatrix((ISmartArtNode) null, AutoShapeType.CircularArrow, 3658954, 2221653, 810090, 704426, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCycleMatrix((ISmartArtNode) null, AutoShapeType.CircularArrow, 3658954, 2492586, 810090, 704426, 10800000, smartArt);
  }

  private static void SetDefaultPropertiesForCycleMatrix(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    string colorString = "accent1";
    if (autoShapeType == AutoShapeType.RoundedRectangle)
    {
      smartArtShape.GetGuideList().Add("adj", "val 10000");
      colorString = "lt1";
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, colorString);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForRadialVenn(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForRadialVenn(smartArt._nodeCollection[0], AutoShapeType.Oval, 2561166, 1206500, 3005666, 3005666, smartArt);
    SmartArt.SetDefaultPropertiesForRadialVenn(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Oval, 3312583, 536, 1502833, 1502833, smartArt);
    SmartArt.SetDefaultPropertiesForRadialVenn(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.Oval, 5269963, 1957916, 1502833, 1502833, smartArt);
    SmartArt.SetDefaultPropertiesForRadialVenn(smartArt._nodeCollection[0].ChildNodes[2], AutoShapeType.Oval, 3312583, 3915297, 1502833, 1502833, smartArt);
    SmartArt.SetDefaultPropertiesForRadialVenn(smartArt._nodeCollection[0].ChildNodes[3], AutoShapeType.Oval, 1355202, 1957916, 1502833, 1502833, smartArt);
  }

  private static void SetDefaultPropertiesForRadialVenn(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForDivergingRadial(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForDivergingRadial(smartArt._nodeCollection[0], AutoShapeType.Oval, 3351609, 1996942, 1424781, 1424781, 0, smartArt);
    SmartArt.SetDefaultPropertiesForDivergingRadial((ISmartArtNode) null, AutoShapeType.RightArrow, 3913169, 1478682, 301660, 484425, 16200000, smartArt);
    SmartArt.SetDefaultPropertiesForDivergingRadial(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Oval, 3351609, 2990, 1424781, 1424781, 0, smartArt);
    SmartArt.SetDefaultPropertiesForDivergingRadial((ISmartArtNode) null, AutoShapeType.RightArrow, 4901608, 2467120, 301660, 484425, 0, smartArt);
    SmartArt.SetDefaultPropertiesForDivergingRadial(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.Oval, 5345561, 1996942, 1424781, 1424781, 0, smartArt);
    SmartArt.SetDefaultPropertiesForDivergingRadial((ISmartArtNode) null, AutoShapeType.RightArrow, 3913169, 3455559, 301660, 484425, 5400000, smartArt);
    SmartArt.SetDefaultPropertiesForDivergingRadial(smartArt._nodeCollection[0].ChildNodes[2], AutoShapeType.Oval, 3351609, 3990894, 1424781, 1424781, 0, smartArt);
    SmartArt.SetDefaultPropertiesForDivergingRadial((ISmartArtNode) null, AutoShapeType.RightArrow, 2924731, 2467120, 301660, 484425, 10800000, smartArt);
    SmartArt.SetDefaultPropertiesForDivergingRadial(smartArt._nodeCollection[0].ChildNodes[3], AutoShapeType.Oval, 1357657, 1996942, 1424781, 1424781, 0, smartArt);
  }

  private static void SetDefaultPropertiesForDivergingRadial(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    if (autoShapeType == AutoShapeType.RightArrow)
    {
      smartArtShape.GetGuideList().Add("adj1", "val 60000");
      smartArtShape.GetGuideList().Add("adj2", "val 50000");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.Oval)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 3200);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBasicRadial(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBasicRadial(smartArt._nodeCollection[0], AutoShapeType.Oval, 3317797, 1963130, 1492405, 1492405, smartArt);
    SmartArt.SetDefaultPropertiesForBasicRadial(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Oval, 3317797, 19459, 1492405, 1492405, smartArt);
    SmartArt.SetDefaultPropertiesForBasicRadial(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.Oval, 5261469, 1963130, 1492405, 1492405, smartArt);
    SmartArt.SetDefaultPropertiesForBasicRadial(smartArt._nodeCollection[0].ChildNodes[2], AutoShapeType.Oval, 3317797, 3906802, 1492405, 1492405, smartArt);
    SmartArt.SetDefaultPropertiesForBasicRadial(smartArt._nodeCollection[0].ChildNodes[3], AutoShapeType.Oval, 1374125, 1963130, 1492405, 1492405, smartArt);
  }

  private static void SetDefaultPropertiesForBasicRadial(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.Oval)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 3500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForRadialCycle(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForRadialCycle((ISmartArtNode) null, AutoShapeType.BlockArc, 1980380, 625714, 4167238, 4167238, "ba1", smartArt);
    SmartArt.SetDefaultPropertiesForRadialCycle((ISmartArtNode) null, AutoShapeType.BlockArc, 1980380, 625714, 4167238, 4167238, "ba2", smartArt);
    SmartArt.SetDefaultPropertiesForRadialCycle((ISmartArtNode) null, AutoShapeType.BlockArc, 1980380, 625714, 4167238, 4167238, "ba3", smartArt);
    SmartArt.SetDefaultPropertiesForRadialCycle((ISmartArtNode) null, AutoShapeType.BlockArc, 1980380, 625714, 4167238, 4167238, "ba4", smartArt);
    SmartArt.SetDefaultPropertiesForRadialCycle(smartArt._nodeCollection[0], AutoShapeType.Oval, 3104554, 1749888, 1918890, 1918890, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForRadialCycle(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Oval, 3392388, 2458, 1343223, 1343223, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForRadialCycle(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.Oval, 5427651, 2037721, 1343223, 1343223, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForRadialCycle(smartArt._nodeCollection[0].ChildNodes[2], AutoShapeType.Oval, 3392388, 4072985, 1343223, 1343223, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForRadialCycle(smartArt._nodeCollection[0].ChildNodes[3], AutoShapeType.Oval, 1357124, 2037721, 1343223, 1343223, (string) null, smartArt);
  }

  private static void SetDefaultPropertiesForRadialCycle(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string shapeName,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    switch (shapeName)
    {
      case "ba1":
        smartArtShape.GetGuideList().Add("adj1", "val 10800000");
        smartArtShape.GetGuideList().Add("adj2", "val 16200000");
        smartArtShape.GetGuideList().Add("adj3", "val 4642");
        break;
      case "ba2":
        smartArtShape.GetGuideList().Add("adj1", "val 5400000");
        smartArtShape.GetGuideList().Add("adj2", "val 10800000");
        smartArtShape.GetGuideList().Add("adj3", "val 4642");
        break;
      case "ba3":
        smartArtShape.GetGuideList().Add("adj1", "val 0");
        smartArtShape.GetGuideList().Add("adj2", "val 5400000");
        smartArtShape.GetGuideList().Add("adj3", "val 4642");
        break;
      case "ba4":
        smartArtShape.GetGuideList().Add("adj1", "val 16200000");
        smartArtShape.GetGuideList().Add("adj2", "val 0");
        smartArtShape.GetGuideList().Add("adj3", "val 4642");
        break;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.Oval)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 4300);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBasicPie(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBasicPie(smartArt._nodeCollection[0], AutoShapeType.Pie, 1905474, 365760, 4551680, 4551680, "pie1", smartArt);
    SmartArt.SetDefaultPropertiesForBasicPie(smartArt._nodeCollection[1], AutoShapeType.Pie, 1670845, 501226, 4551680, 4551680, "pie2", smartArt);
    SmartArt.SetDefaultPropertiesForBasicPie(smartArt._nodeCollection[2], AutoShapeType.Pie, 1670845, 501226, 4551680, 4551680, "pie3", smartArt);
  }

  private static void SetDefaultPropertiesForBasicPie(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string shapeName,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    switch (shapeName)
    {
      case "pie1":
        smartArtShape.GetGuideList().Add("adj1", "val 16200000");
        smartArtShape.GetGuideList().Add("adj2", "val 1800000");
        break;
      case "pie2":
        smartArtShape.GetGuideList().Add("adj1", "val 1800000");
        smartArtShape.GetGuideList().Add("adj2", "val 9000000");
        break;
      case "pie3":
        smartArtShape.GetGuideList().Add("adj1", "val 9000000");
        smartArtShape.GetGuideList().Add("adj2", "val 16200000");
        break;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForSegmentedCycle(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForSegmentedCycle(AutoShapeType.Pie, 1881902, 352213, 4551680, 4551680, "pie1", smartArt);
    SmartArt.SetDefaultPropertiesForSegmentedCycle(AutoShapeType.Pie, 1788159, 514773, 4551680, 4551680, "pie2", smartArt);
    SmartArt.SetDefaultPropertiesForSegmentedCycle(AutoShapeType.Pie, 1694416, 352213, 4551680, 4551680, "pie3", smartArt);
    SmartArt.SetDefaultPropertiesForSegmentedCycle(AutoShapeType.CircularArrow, 1600507, 70442, 5115221, 5115221, "ca1", smartArt);
    SmartArt.SetDefaultPropertiesForSegmentedCycle(AutoShapeType.CircularArrow, 1506389, 232714, 5115221, 5115221, "ca2", smartArt);
    SmartArt.SetDefaultPropertiesForSegmentedCycle(AutoShapeType.CircularArrow, 1412270, 70442, 5115221, 5115221, "ca3", smartArt);
  }

  private static void SetDefaultPropertiesForSegmentedCycle(
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string shapeName,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    switch (shapeName)
    {
      case "pie1":
        smartArtShape.GetGuideList().Add("adj1", "val 16200000");
        smartArtShape.GetGuideList().Add("adj2", "val 1800000");
        break;
      case "pie2":
        smartArtShape.GetGuideList().Add("adj1", "val 1800000");
        smartArtShape.GetGuideList().Add("adj2", "val 9000000");
        break;
      case "pie3":
        smartArtShape.GetGuideList().Add("adj1", "val 9000000");
        smartArtShape.GetGuideList().Add("adj2", "val 16200000");
        break;
      case "ca1":
        smartArtShape.GetGuideList().Add("adj1", "val 5085");
        smartArtShape.GetGuideList().Add("adj2", "val 327528");
        smartArtShape.GetGuideList().Add("adj3", "val 1472472");
        smartArtShape.GetGuideList().Add("adj4", "val 16199432");
        smartArtShape.GetGuideList().Add("adj5", "val 5932");
        break;
      case "ca2":
        smartArtShape.GetGuideList().Add("adj1", "val 5085");
        smartArtShape.GetGuideList().Add("adj2", "val 327528");
        smartArtShape.GetGuideList().Add("adj3", "val 8671970");
        smartArtShape.GetGuideList().Add("adj4", "val 1800502");
        smartArtShape.GetGuideList().Add("adj5", "val 5932");
        break;
      case "ca3":
        smartArtShape.GetGuideList().Add("adj1", "val 5085");
        smartArtShape.GetGuideList().Add("adj2", "val 327528");
        smartArtShape.GetGuideList().Add("adj3", "val 15873039");
        smartArtShape.GetGuideList().Add("adj4", "val 9000000");
        smartArtShape.GetGuideList().Add("adj5", "val 5932");
        break;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    smartArt.DataModel.SmartArtShapeCollection.Add(Guid.NewGuid(), smartArtShape);
  }

  private static void InitializeDefaultPropertiesForMultiDirectionalCycle(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForMultiDirectionalCycle(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 2661046, 1572, 2805906, 1402953, 0, smartArt);
    SmartArt.SetDefaultPropertiesForMultiDirectionalCycle((ISmartArtNode) null, AutoShapeType.LeftRightArrow, 4491365, 2463816, 1461927, 491033, 3600000, smartArt);
    SmartArt.SetDefaultPropertiesForMultiDirectionalCycle(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 4977704, 4014141, 2805906, 1402953, 0, smartArt);
    SmartArt.SetDefaultPropertiesForMultiDirectionalCycle((ISmartArtNode) null, AutoShapeType.LeftRightArrow, 3333036, 4470101, 1461927, 491033, 10800000, smartArt);
    SmartArt.SetDefaultPropertiesForMultiDirectionalCycle(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 344389, 4014141, 2805906, 1402953, 0, smartArt);
    SmartArt.SetDefaultPropertiesForMultiDirectionalCycle((ISmartArtNode) null, AutoShapeType.LeftRightArrow, 2174707, 2463816, 1461927, 491033, 18000000, smartArt);
  }

  private static void SetDefaultPropertiesForMultiDirectionalCycle(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    if (autoShapeType == AutoShapeType.RoundedRectangle)
    {
      smartArtShape.GetGuideList().Add("adj", "val 10000");
    }
    else
    {
      smartArtShape.GetGuideList().Add("adj1", "val 60000");
      smartArtShape.GetGuideList().Add("adj2", "val 50000");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.RoundedRectangle)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6100);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForContinuousCycle(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForContinuousCycle((ISmartArtNode) null, AutoShapeType.CircularArrow, 1374164, -32039, 5379671, 5379671, smartArt);
    SmartArt.SetDefaultPropertiesForContinuousCycle(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 2799953, 2274, 2528093, 1264046, smartArt);
    SmartArt.SetDefaultPropertiesForContinuousCycle(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 4981774, 1587460, 2528093, 1264046, smartArt);
    SmartArt.SetDefaultPropertiesForContinuousCycle(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 4148393, 4152345, 2528093, 1264046, smartArt);
    SmartArt.SetDefaultPropertiesForContinuousCycle(smartArt._nodeCollection[3], AutoShapeType.RoundedRectangle, 1451513, 4152345, 2528093, 1264046, smartArt);
    SmartArt.SetDefaultPropertiesForContinuousCycle(smartArt._nodeCollection[4], AutoShapeType.RoundedRectangle, 618131, 1587460, 2528093, 1264046, smartArt);
  }

  private static void SetDefaultPropertiesForContinuousCycle(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    if (autoShapeType == AutoShapeType.CircularArrow)
    {
      smartArtShape.GetGuideList().Add("adj1", "val 5544");
      smartArtShape.GetGuideList().Add("adj2", "val 330680");
      smartArtShape.GetGuideList().Add("adj3", "val 13767645");
      smartArtShape.GetGuideList().Add("adj4", "val 17391005");
      smartArtShape.GetGuideList().Add("adj5", "val 5757");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.RoundedRectangle)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 5200);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForNondirectionalCycle(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForNondirectionalCycle(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 3174007, 3160, 1779984, 1156989, smartArt);
    SmartArt.SetDefaultPropertiesForNondirectionalCycle(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 5371068, 1599418, 1779984, 1156989, smartArt);
    SmartArt.SetDefaultPropertiesForNondirectionalCycle(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 4531865, 4182218, 1779984, 1156989, smartArt);
    SmartArt.SetDefaultPropertiesForNondirectionalCycle(smartArt._nodeCollection[3], AutoShapeType.RoundedRectangle, 1816149, 4182218, 1779984, 1156989, smartArt);
    SmartArt.SetDefaultPropertiesForNondirectionalCycle(smartArt._nodeCollection[4], AutoShapeType.RoundedRectangle, 976947, 1599418, 1779984, 1156989, smartArt);
  }

  private static void SetDefaultPropertiesForNondirectionalCycle(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.RoundedRectangle)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 4500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBlockCycle(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBlockCycle(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 3174007, 3160, 1779984, 1156989, smartArt);
    SmartArt.SetDefaultPropertiesForBlockCycle(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 5371068, 1599418, 1779984, 1156989, smartArt);
    SmartArt.SetDefaultPropertiesForBlockCycle(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 4531865, 4182218, 1779984, 1156989, smartArt);
    SmartArt.SetDefaultPropertiesForBlockCycle(smartArt._nodeCollection[3], AutoShapeType.RoundedRectangle, 1816149, 4182218, 1779984, 1156989, smartArt);
    SmartArt.SetDefaultPropertiesForBlockCycle(smartArt._nodeCollection[4], AutoShapeType.RoundedRectangle, 976947, 1599418, 1779984, 1156989, smartArt);
  }

  private static void SetDefaultPropertiesForBlockCycle(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.RoundedRectangle)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 4500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForTextCycle(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForTextCycle(smartArt._nodeCollection[0], AutoShapeType.CircularArrow, 1549560, 385, 5028878, 5028878, "CircularArrow1", smartArt);
    SmartArt.SetDefaultPropertiesForTextCycle(smartArt._nodeCollection[1], AutoShapeType.CircularArrow, 1549560, 385, 5028878, 5028878, "CircularArrow2", smartArt);
    SmartArt.SetDefaultPropertiesForTextCycle(smartArt._nodeCollection[2], AutoShapeType.CircularArrow, 1549560, 385, 5028878, 5028878, "CircularArrow3", smartArt);
    SmartArt.SetDefaultPropertiesForTextCycle(smartArt._nodeCollection[3], AutoShapeType.CircularArrow, 1549560, 385, 5028878, 5028878, "CircularArrow4", smartArt);
    SmartArt.SetDefaultPropertiesForTextCycle(smartArt._nodeCollection[4], AutoShapeType.CircularArrow, 1549560, 385, 5028878, 5028878, "CircularArrow5", smartArt);
  }

  private static void SetDefaultPropertiesForTextCycle(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string circularArrowName,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    switch (circularArrowName)
    {
      case "CircularArrow1":
        smartArtShape.GetGuideList().Add("adj1", "val 5202");
        smartArtShape.GetGuideList().Add("adj2", "val 336015");
        smartArtShape.GetGuideList().Add("adj3", "val 21292825");
        smartArtShape.GetGuideList().Add("adj4", "val 19766604");
        smartArtShape.GetGuideList().Add("adj5", "val 6068");
        break;
      case "CircularArrow2":
        smartArtShape.GetGuideList().Add("adj1", "val 5202");
        smartArtShape.GetGuideList().Add("adj2", "val 336015");
        smartArtShape.GetGuideList().Add("adj3", "val 4014266");
        smartArtShape.GetGuideList().Add("adj4", "val 2253829");
        smartArtShape.GetGuideList().Add("adj5", "val 6068");
        break;
      case "CircularArrow3":
        smartArtShape.GetGuideList().Add("adj1", "val 5202");
        smartArtShape.GetGuideList().Add("adj2", "val 336015");
        smartArtShape.GetGuideList().Add("adj3", "val 8210155");
        smartArtShape.GetGuideList().Add("adj4", "val 6449719");
        smartArtShape.GetGuideList().Add("adj5", "val 6068");
        break;
      case "CircularArrow4":
        smartArtShape.GetGuideList().Add("adj1", "val 5202");
        smartArtShape.GetGuideList().Add("adj2", "val 336015");
        smartArtShape.GetGuideList().Add("adj3", "val 12297380");
        smartArtShape.GetGuideList().Add("adj4", "val 10771160");
        smartArtShape.GetGuideList().Add("adj5", "val 6068");
        break;
      case "CircularArrow5":
        smartArtShape.GetGuideList().Add("adj1", "val 5202");
        smartArtShape.GetGuideList().Add("adj2", "val 336015");
        smartArtShape.GetGuideList().Add("adj3", "val 16865256");
        smartArtShape.GetGuideList().Add("adj4", "val 15198729");
        smartArtShape.GetGuideList().Add("adj5", "val 6068");
        break;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBasicCycle(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBasicCycle(smartArt._nodeCollection[0], AutoShapeType.Oval, 3246437, 534, 1635124, 1635124, 0, smartArt);
    SmartArt.SetDefaultPropertiesForBasicCycle((ISmartArtNode) null, AutoShapeType.RightArrow, 4830234, 1257302, 436123, 551854, 2160000, smartArt);
    SmartArt.SetDefaultPropertiesForBasicCycle(smartArt._nodeCollection[1], AutoShapeType.Oval, 5235001, 1445310, 1635124, 1635124, 0, smartArt);
    SmartArt.SetDefaultPropertiesForBasicCycle((ISmartArtNode) null, AutoShapeType.RightArrow, 5458534, 3144055, 436123, 551854, 6480000, smartArt);
    SmartArt.SetDefaultPropertiesForBasicCycle(smartArt._nodeCollection[2], AutoShapeType.Oval, 4475437, 3783007, 1635124, 1635124, 0, smartArt);
    SmartArt.SetDefaultPropertiesForBasicCycle((ISmartArtNode) null, AutoShapeType.RightArrow, 3858281, 4324642, 436123, 551854, 1080000, smartArt);
    SmartArt.SetDefaultPropertiesForBasicCycle(smartArt._nodeCollection[3], AutoShapeType.Oval, 2017437, 3783007, 1635124, 1635124, 0, smartArt);
    SmartArt.SetDefaultPropertiesForBasicCycle((ISmartArtNode) null, AutoShapeType.RightArrow, 2240970, 3167533, 436123, 551854, 15120000, smartArt);
    SmartArt.SetDefaultPropertiesForBasicCycle(smartArt._nodeCollection[4], AutoShapeType.Oval, 1257873, 1445310, 1635124, 1635124, 0, smartArt);
    SmartArt.SetDefaultPropertiesForBasicCycle((ISmartArtNode) null, AutoShapeType.RightArrow, 2841670, 1271812, 436123, 551854, 19440000, smartArt);
  }

  private static void SetDefaultPropertiesForBasicCycle(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    if (autoShapeType == AutoShapeType.RightArrow)
    {
      smartArtShape.GetGuideList().Add("adj1", "val 60000");
      smartArtShape.GetGuideList().Add("adj2", "val 50000");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (smartArtShape.TextBody.Paragraphs.Count > 0)
    {
      smartArtShape.TextBody.VerticalAlignment = VerticalAlignmentType.Middle;
      Paragraph paragraph = smartArtShape.TextBody.Paragraphs[0] as Paragraph;
      SmartArt.SetParagraphProperties(paragraph, HorizontalAlignment.Center, 90000, 0, 35000, true, 2889250L);
      Font font = new Font(paragraph);
      font.Color = ColorObject.White;
      if (paragraph.Text.Length >= 7)
        SmartArt.SetFontProperties(font, 1200);
      paragraph.SetEndParaProps(font);
      (paragraph.TextParts.Count != 0 ? paragraph.TextParts[0] as TextPart : paragraph.TextParts.Add() as TextPart).SetFont(font);
    }
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForDivergingArrows(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForDivergingArrows(smartArt._nodeCollection[0], AutoShapeType.UpArrow, 337, 774567, 3869531, 3869531, 16200000, smartArt);
    SmartArt.SetDefaultPropertiesForDivergingArrows(smartArt._nodeCollection[1], AutoShapeType.UpArrow, 4258130, 774567, 3869531, 3869531, 5400000, smartArt);
  }

  private static void SetDefaultPropertiesForDivergingArrows(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    smartArtShape.GetGuideList().Add("adj1", "val 50000");
    smartArtShape.GetGuideList().Add("adj2", "val 35000");
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForConvergingArrows(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForConvergingArrows(smartArt._nodeCollection[0], AutoShapeType.DownArrow, 1763, 744802, 3929062, 3929062, 16200000, smartArt);
    SmartArt.SetDefaultPropertiesForConvergingArrows(smartArt._nodeCollection[1], AutoShapeType.DownArrow, 4197174, 744802, 3929062, 3929062, 5400000, smartArt);
  }

  private static void SetDefaultPropertiesForConvergingArrows(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    smartArtShape.GetGuideList().Add("adj1", "val 50000");
    smartArtShape.GetGuideList().Add("adj2", "val 35000");
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForOpposingArrows(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForOpposingArrows((ISmartArtNode) null, AutoShapeType.UpArrow, 4470, 0, 2682240, 2600960, smartArt);
    SmartArt.SetDefaultPropertiesForOpposingArrows((ISmartArtNode) null, AutoShapeType.DownArrow, 809142, 2817706, 2682240, 2600960, smartArt);
    SmartArt.SetDefaultPropertiesForOpposingArrows(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 2767177, 0, 4551680, 2600960, smartArt);
    SmartArt.SetDefaultPropertiesForOpposingArrows(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 3571849, 2817706, 4551680, 2600960, smartArt);
  }

  private static void SetDefaultPropertiesForOpposingArrows(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForArrowRibbon(SmartArt smartArt)
  {
  }

  private static void SetDefaultPropertiesForArrowRibbon(
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    smartArt.DataModel.SmartArtShapeCollection.Add(Guid.NewGuid(), smartArtShape);
  }

  private static void InitializeDefaultPropertiesForGear(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForGear(AutoShapeType.CircularArrow, 3577577, 1980864, 3814741, 3814741, smartArt);
    SmartArt.SetDefaultPropertiesForGear(AutoShapeType.CircularArrow, 2781867, -231776, 2988394, 2988394, smartArt);
  }

  private static void SetDefaultPropertiesForGear(
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    if (autoShapeType == AutoShapeType.CircularArrow)
    {
      smartArtShape.GetGuideList().Add("adj1", "val 4688");
      smartArtShape.GetGuideList().Add("adj2", "val 299029");
      smartArtShape.GetGuideList().Add("adj3", "val 2539295");
      smartArtShape.GetGuideList().Add("adj4", "val 15812321");
      smartArtShape.GetGuideList().Add("adj5", "val 5469");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    smartArt.DataModel.SmartArtShapeCollection.Add(Guid.NewGuid(), smartArtShape);
  }

  private static void InitializeDefaultPropertiesForFunnel(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForFunnel((ISmartArtNode) null, AutoShapeType.Oval, 1872826, 220133, 4368800, 1517226, true, smartArt);
    SmartArt.SetDefaultPropertiesForFunnel((ISmartArtNode) null, AutoShapeType.DownArrow, 3640666, 3935306, 846666, 541866, false, smartArt);
    SmartArt.SetDefaultPropertiesForFunnel(smartArt._nodeCollection[0], AutoShapeType.Oval, 3461173, 1854538, 1524000, 1524000, false, smartArt);
    SmartArt.SetDefaultPropertiesForFunnel(smartArt._nodeCollection[1], AutoShapeType.Oval, 2370666, 711200, 1524000, 1524000, false, smartArt);
    SmartArt.SetDefaultPropertiesForFunnel(smartArt._nodeCollection[2], AutoShapeType.Oval, 3928533, 342730, 1524000, 1524000, false, smartArt);
    SmartArt.SetDefaultPropertiesForFunnel(smartArt._nodeCollection[3], AutoShapeType.Rectangle, 2031999, 4368800, 4064000, 1016000, false, smartArt);
  }

  private static void SetDefaultPropertiesForFunnel(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isFirstShape,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties, isFirstShape);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.Oval)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 3400);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForVerticalEquation(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForVerticalEquation(smartArt.Nodes[0], AutoShapeType.Oval, 509984, 1963, 1974453, 1974453, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalEquation((ISmartArtNode) null, AutoShapeType.MathPlus, 924619, 2136742, 1145182, 1145182, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalEquation(smartArt.Nodes[1], AutoShapeType.Oval, 509984, 3442250, 1974453, 1974453, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalEquation((ISmartArtNode) null, AutoShapeType.RightArrow, 2780605, 2342085, 627876, 734496, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalEquation(smartArt.Nodes[2], AutoShapeType.Oval, 3669109, 734880, 3948906, 3948906, smartArt);
  }

  private static void SetDefaultPropertiesForVerticalEquation(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.Oval)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 4400);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForEquation(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForEquation(smartArt._nodeCollection[0], AutoShapeType.Oval, 1366, 1803466, 1811734, 1811734, smartArt);
    SmartArt.SetDefaultPropertiesForEquation((ISmartArtNode) null, AutoShapeType.MathPlus, 1960214, 2183930, 1050805, 1050805, smartArt);
    SmartArt.SetDefaultPropertiesForEquation(smartArt._nodeCollection[1], AutoShapeType.Oval, 3158132, 1803466, 1811734, 1811734, smartArt);
    SmartArt.SetDefaultPropertiesForEquation((ISmartArtNode) null, AutoShapeType.MathEqual, 5116980, 2183930, 1050805, 1050805, smartArt);
    SmartArt.SetDefaultPropertiesForEquation(smartArt._nodeCollection[2], AutoShapeType.Oval, 6314898, 1803466, 1811734, 1811734, smartArt);
  }

  private static void SetDefaultPropertiesForEquation(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.Oval)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 40);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForCircularBendingProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForCircularBendingProcess(smartArt._nodeCollection[0], AutoShapeType.Oval, 1135062, 66, 1464468, 1464468, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCircularBendingProcess((ISmartArtNode) null, AutoShapeType.IsoscelesTriangle, 1611014, 1653634, 512564, 400890, 10800000, smartArt);
    SmartArt.SetDefaultPropertiesForCircularBendingProcess(smartArt._nodeCollection[1], AutoShapeType.Oval, 1378896, 2220933, 976800, 976800, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCircularBendingProcess((ISmartArtNode) null, AutoShapeType.IsoscelesTriangle, 1611014, 3508750, 512564, 400890, 10800000, smartArt);
    SmartArt.SetDefaultPropertiesForCircularBendingProcess(smartArt._nodeCollection[2], AutoShapeType.Oval, 1378896, 4197965, 976800, 976800, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCircularBendingProcess((ISmartArtNode) null, AutoShapeType.IsoscelesTriangle, 2720712, 4485920, 512564, 400890, 5400000, smartArt);
    SmartArt.SetDefaultPropertiesForCircularBendingProcess(smartArt._nodeCollection[3], AutoShapeType.Oval, 3575599, 4197965, 976800, 976800, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCircularBendingProcess((ISmartArtNode) null, AutoShapeType.IsoscelesTriangle, 3807717, 3486058, 512564, 400890, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCircularBendingProcess(smartArt._nodeCollection[4], AutoShapeType.Oval, 3575599, 2220933, 976800, 976800, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCircularBendingProcess((ISmartArtNode) null, AutoShapeType.IsoscelesTriangle, 3807717, 1509025, 512564, 400890, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCircularBendingProcess(smartArt._nodeCollection[5], AutoShapeType.Oval, 3575599, 243900, 976800, 976800, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCircularBendingProcess((ISmartArtNode) null, AutoShapeType.IsoscelesTriangle, 4917415, 531855, 512564, 400890, 5400000, smartArt);
    SmartArt.SetDefaultPropertiesForCircularBendingProcess(smartArt._nodeCollection[6], AutoShapeType.Oval, 5772302, 243900, 976800, 976800, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCircularBendingProcess((ISmartArtNode) null, AutoShapeType.IsoscelesTriangle, 6004421, 1531717, 512564, 400890, 10800000, smartArt);
    SmartArt.SetDefaultPropertiesForCircularBendingProcess(smartArt._nodeCollection[7], AutoShapeType.Oval, 5772302, 2220933, 976800, 976800, 0, smartArt);
    SmartArt.SetDefaultPropertiesForCircularBendingProcess((ISmartArtNode) null, AutoShapeType.IsoscelesTriangle, 6004421, 3386833, 512564, 400890, 10800000, smartArt);
    SmartArt.SetDefaultPropertiesForCircularBendingProcess(smartArt._nodeCollection[8], AutoShapeType.Oval, 5528468, 3954131, 1464468, 1464468, 0, smartArt);
  }

  private static void SetDefaultPropertiesForCircularBendingProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.Oval)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 3300);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForDescendingProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForDescendingProcess((ISmartArtNode) null, AutoShapeType.SwooshArrow, 1397313, 1078272, 4677714, 3262122, 4396374, smartArt);
    SmartArt.SetDefaultPropertiesForDescendingProcess((ISmartArtNode) null, AutoShapeType.Oval, 3149599, 1504221, 118126, 118126, 0, smartArt);
    SmartArt.SetDefaultPropertiesForDescendingProcess((ISmartArtNode) null, AutoShapeType.Oval, 3958444, 2156629, 118126, 118126, 0, smartArt);
    SmartArt.SetDefaultPropertiesForDescendingProcess((ISmartArtNode) null, AutoShapeType.Oval, 4564630, 2919577, 118126, 118126, 0, smartArt);
    SmartArt.SetDefaultPropertiesForDescendingProcess(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 1083733, 0, 2205397, 866986, 0, smartArt);
    SmartArt.SetDefaultPropertiesForDescendingProcess(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 3825578, 1129792, 3218688, 866986, 0, smartArt);
    SmartArt.SetDefaultPropertiesForDescendingProcess(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 1083733, 1782199, 2563029, 866986, 0, smartArt);
    SmartArt.SetDefaultPropertiesForDescendingProcess(smartArt._nodeCollection[3], AutoShapeType.Rectangle, 5077290, 2545147, 1966976, 866986, 0, smartArt);
    SmartArt.SetDefaultPropertiesForDescendingProcess(smartArt._nodeCollection[4], AutoShapeType.Rectangle, 4064000, 4551680, 2980266, 866986, 0, smartArt);
  }

  private static void SetDefaultPropertiesForDescendingProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    if (autoShapeType == AutoShapeType.SwooshArrow)
    {
      smartArtShape.GetGuideList().Add("adj1", "val 16310");
      smartArtShape.GetGuideList().Add("adj2", "val 31370");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForUpwardArrow(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForUpwardArrow((ISmartArtNode) null, AutoShapeType.SwooshArrow, 0, 169333, 8128000, 5079999, smartArt);
    SmartArt.SetDefaultPropertiesForUpwardArrow((ISmartArtNode) null, AutoShapeType.Oval, 1032256, 3675549, 211328, 211328, smartArt);
    SmartArt.SetDefaultPropertiesForUpwardArrow((ISmartArtNode) null, AutoShapeType.Oval, 2897632, 2294805, 382016, 382016, smartArt);
    SmartArt.SetDefaultPropertiesForUpwardArrow((ISmartArtNode) null, AutoShapeType.Oval, 5140960, 1454573, 528320, 528320, smartArt);
    SmartArt.SetDefaultPropertiesForUpwardArrow(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 1137920, 3781213, 1893824, 1468120, smartArt);
    SmartArt.SetDefaultPropertiesForUpwardArrow(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 3088640, 2485813, 1950720, 2763519, smartArt);
    SmartArt.SetDefaultPropertiesForUpwardArrow(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 5405120, 1718733, 1950720, 3530600, smartArt);
  }

  private static void SetDefaultPropertiesForUpwardArrow(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    if (autoShapeType == AutoShapeType.SwooshArrow)
    {
      smartArtShape.GetGuideList().Add("adj1", "val 25000");
      smartArtShape.GetGuideList().Add("adj2", "val 25000");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForAscendingPictureAccentProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForAscendingPictureAccentProcess((ISmartArtNode) null, AutoShapeType.Oval, 2858087, 3064947, 194126, 194126, true, smartArt);
    SmartArt.SetDefaultPropertiesForAscendingPictureAccentProcess((ISmartArtNode) null, AutoShapeType.Oval, 2688032, 3337468, 194126, 194126, true, smartArt);
    SmartArt.SetDefaultPropertiesForAscendingPictureAccentProcess((ISmartArtNode) null, AutoShapeType.Oval, 2485364, 3573413, 194126, 194126, true, smartArt);
    SmartArt.SetDefaultPropertiesForAscendingPictureAccentProcess((ISmartArtNode) null, AutoShapeType.Oval, 2727634, 322219, 194126, 194126, true, smartArt);
    SmartArt.SetDefaultPropertiesForAscendingPictureAccentProcess((ISmartArtNode) null, AutoShapeType.Oval, 2986987, 167670, 194126, 194126, true, smartArt);
    SmartArt.SetDefaultPropertiesForAscendingPictureAccentProcess((ISmartArtNode) null, AutoShapeType.Oval, 3245563, 13121, 194126, 194126, true, smartArt);
    SmartArt.SetDefaultPropertiesForAscendingPictureAccentProcess((ISmartArtNode) null, AutoShapeType.Oval, 3504139, 167670, 194126, 194126, true, smartArt);
    SmartArt.SetDefaultPropertiesForAscendingPictureAccentProcess((ISmartArtNode) null, AutoShapeType.Oval, 3763492, 322219, 194126, 194126, true, smartArt);
    SmartArt.SetDefaultPropertiesForAscendingPictureAccentProcess((ISmartArtNode) null, AutoShapeType.Oval, 3245563, 339219, 194126, 194126, true, smartArt);
    SmartArt.SetDefaultPropertiesForAscendingPictureAccentProcess((ISmartArtNode) null, AutoShapeType.Oval, 3245563, 665318, 194126, 194126, true, smartArt);
    SmartArt.SetDefaultPropertiesForAscendingPictureAccentProcess(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 1666152, 4282489, 4186915, 1123055, false, smartArt);
    SmartArt.SetDefaultPropertiesForAscendingPictureAccentProcess((ISmartArtNode) null, AutoShapeType.Oval, 505276, 3182101, 1941262, 1941134, false, smartArt);
    SmartArt.SetDefaultPropertiesForAscendingPictureAccentProcess(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 3435807, 2085834, 4186915, 1123055, false, smartArt);
    SmartArt.SetDefaultPropertiesForAscendingPictureAccentProcess((ISmartArtNode) null, AutoShapeType.Oval, 2274932, 985446, 1941262, 1941134, false, smartArt);
  }

  private static void SetDefaultPropertiesForAscendingPictureAccentProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isSmallCircle,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool isColorSet = false;
    if (autoShapeType == AutoShapeType.RoundedRectangle)
      isColorSet = true;
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, isSmallCircle);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, isColorSet, 4600);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForVerticalBendingProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForVerticalBendingProcess((ISmartArtNode) null, AutoShapeType.Rectangle, -374328, 1438591, 1654072, 199667, 5400000, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBendingProcess(smartArt.Nodes[0], AutoShapeType.RoundedRectangle, 4087, 379875, 2218531, 1331118, 0, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBendingProcess((ISmartArtNode) null, AutoShapeType.Rectangle, -374328, 3102489, 1654072, 199667, 5400000, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBendingProcess(smartArt.Nodes[1], AutoShapeType.RoundedRectangle, 4087, 2043774, 2218531, 1331118, 0, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBendingProcess((ISmartArtNode) null, AutoShapeType.Rectangle, 457620, 3934438, 2940820, 199667, 0, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBendingProcess(smartArt.Nodes[2], AutoShapeType.RoundedRectangle, 4087, 3707672, 2218531, 1331118, 0, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBendingProcess((ISmartArtNode) null, AutoShapeType.Rectangle, 2576317, 3102489, 1654072, 199667, 16200000, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBendingProcess(smartArt.Nodes[3], AutoShapeType.RoundedRectangle, 2954734, 3707672, 2218531, 1331118, 0, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBendingProcess((ISmartArtNode) null, AutoShapeType.Rectangle, 2576317, 1438591, 1654072, 199667, 16200000, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBendingProcess(smartArt.Nodes[4], AutoShapeType.RoundedRectangle, 2954734, 2043774, 2218531, 1331118, 0, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBendingProcess((ISmartArtNode) null, AutoShapeType.Rectangle, 3408266, 606641, 2940820, 199667, 0, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBendingProcess(smartArt.Nodes[5], AutoShapeType.RoundedRectangle, 2954734, 379875, 2218531, 1331118, 0, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBendingProcess((ISmartArtNode) null, AutoShapeType.Rectangle, 5526964, 1438591, 1654072, 199667, 5400000, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBendingProcess(smartArt.Nodes[6], AutoShapeType.RoundedRectangle, 5905380, 379875, 2218531, 1331118, 0, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBendingProcess((ISmartArtNode) null, AutoShapeType.Rectangle, 5526964, 3102489, 1654072, 199667, 5400000, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBendingProcess(smartArt.Nodes[7], AutoShapeType.RoundedRectangle, 5905380, 2043774, 2218531, 1331118, 0, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBendingProcess(smartArt.Nodes[8], AutoShapeType.RoundedRectangle, 5905380, 3707672, 2218531, 1331118, 0, smartArt);
  }

  private static void SetDefaultPropertiesForVerticalBendingProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    bool flag = false;
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    if (autoShapeType == AutoShapeType.RoundedRectangle)
    {
      flag = true;
      smartArtShape.GetGuideList().Add("adj", "val 10000");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, flag);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, flag, 5800);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForRepeatingBendingProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForRepeatingBendingProcess(smartArt.Nodes[0], AutoShapeType.Rectangle, 1391205, 582, 2397125, 1438275, smartArt);
    SmartArt.SetDefaultPropertiesForRepeatingBendingProcess(smartArt.Nodes[1], AutoShapeType.Rectangle, 4339669, 582, 2397125, 1438275, smartArt);
    SmartArt.SetDefaultPropertiesForRepeatingBendingProcess(smartArt.Nodes[2], AutoShapeType.Rectangle, 1391205, 1990196, 2397125, 1438275, smartArt);
    SmartArt.SetDefaultPropertiesForRepeatingBendingProcess(smartArt.Nodes[3], AutoShapeType.Rectangle, 4339669, 1990196, 2397125, 1438275, smartArt);
    SmartArt.SetDefaultPropertiesForRepeatingBendingProcess(smartArt.Nodes[4], AutoShapeType.Rectangle, 1391205, 3979809, 2397125, 1438275, smartArt);
  }

  private static void SetDefaultPropertiesForRepeatingBendingProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 5100);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBasicBendingProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBasicBendingProcess(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 7143, 1001183, 2135187, 1281112, 0, smartArt);
    SmartArt.SetDefaultPropertiesForBasicBendingProcess((ISmartArtNode) null, AutoShapeType.RightArrow, 2330227, 1376976, 452659, 529526, 0, smartArt);
    SmartArt.SetDefaultPropertiesForBasicBendingProcess(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 2996406, 1001183, 2135187, 1281112, 0, smartArt);
    SmartArt.SetDefaultPropertiesForBasicBendingProcess((ISmartArtNode) null, AutoShapeType.RightArrow, 5319490, 1376976, 452659, 529526, 0, smartArt);
    SmartArt.SetDefaultPropertiesForBasicBendingProcess(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 5985668, 1001183, 2135187, 1281112, 0, smartArt);
    SmartArt.SetDefaultPropertiesForBasicBendingProcess((ISmartArtNode) null, AutoShapeType.RightArrow, 6826932, 2431759, 452659, 529526, 5400000, smartArt);
    SmartArt.SetDefaultPropertiesForBasicBendingProcess(smartArt._nodeCollection[3], AutoShapeType.RoundedRectangle, 5985668, 3136371, 2135187, 1281112, 0, smartArt);
    SmartArt.SetDefaultPropertiesForBasicBendingProcess((ISmartArtNode) null, AutoShapeType.RightArrow, 5345112, 3512163, 452659, 529526, 10800000, smartArt);
    SmartArt.SetDefaultPropertiesForBasicBendingProcess(smartArt._nodeCollection[4], AutoShapeType.RoundedRectangle, 2996406, 3136370, 2135187, 1281112, 0, smartArt);
  }

  private static void SetDefaultPropertiesForBasicBendingProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool isColorSet = false;
    if (autoShapeType == AutoShapeType.RoundedRectangle)
    {
      smartArtShape.GetGuideList().Add("adj", "val 10000");
      isColorSet = true;
    }
    else
    {
      smartArtShape.GetGuideList().Add("adj1", "val 60000");
      smartArtShape.GetGuideList().Add("adj2", "val 50000");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, isColorSet, 5500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForCircleArrowProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForCircleArrowProcess((ISmartArtNode) null, AutoShapeType.CircularArrow, 3122127, 0, 2608149, 2608546, smartArt);
    SmartArt.SetDefaultPropertiesForCircleArrowProcess((ISmartArtNode) null, AutoShapeType.BlockArc, 3307759, 3176964, 2240804, 2241702, smartArt);
    SmartArt.SetDefaultPropertiesForCircleArrowProcess(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 3698614, 941764, 1449298, 724475, smartArt);
    SmartArt.SetDefaultPropertiesForCircleArrowProcess(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 2977148, 2449237, 1449298, 724475, smartArt);
    SmartArt.SetDefaultPropertiesForCircleArrowProcess(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 3702042, 3958878, 1449298, 724475, smartArt);
  }

  private static void SetDefaultPropertiesForCircleArrowProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    if (autoShapeType == AutoShapeType.CircularArrow)
    {
      smartArtShape.GetGuideList().Add("adj1", "val 10980");
      smartArtShape.GetGuideList().Add("adj2", "val 1142322");
      smartArtShape.GetGuideList().Add("adj3", "val 4500000");
      smartArtShape.GetGuideList().Add("adj4", "val 10800000");
      smartArtShape.GetGuideList().Add("adj5", "val 12500");
    }
    else if (autoShapeType == AutoShapeType.BlockArc)
    {
      smartArtShape.GetGuideList().Add("adj1", "val 13500000");
      smartArtShape.GetGuideList().Add("adj2", "val 10800000");
      smartArtShape.GetGuideList().Add("adj3", "val 12740");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForProcessList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForProcessList(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 4286, 954550, 3794125, 948531, true, smartArt);
    SmartArt.SetDefaultPropertiesForProcessList((ISmartArtNode) null, AutoShapeType.RightArrow, 1818352, 1986078, 165992, 165992, false, smartArt);
    SmartArt.SetDefaultPropertiesForProcessList(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.RoundedRectangle, 4286, 2235067, 3794125, 948531, false, smartArt);
    SmartArt.SetDefaultPropertiesForProcessList((ISmartArtNode) null, AutoShapeType.RightArrow, 1818352, 3266595, 165992, 165992, false, smartArt);
    SmartArt.SetDefaultPropertiesForProcessList(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.RoundedRectangle, 4286, 3515585, 3794125, 948531, false, smartArt);
    SmartArt.SetDefaultPropertiesForProcessList(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 4329588, 954550, 3794125, 948531, true, smartArt);
    SmartArt.SetDefaultPropertiesForProcessList((ISmartArtNode) null, AutoShapeType.RightArrow, 6143654, 1986078, 165992, 165992, false, smartArt);
    SmartArt.SetDefaultPropertiesForProcessList(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.RoundedRectangle, 4329588, 2235067, 3794125, 948531, false, smartArt);
    SmartArt.SetDefaultPropertiesForProcessList((ISmartArtNode) null, AutoShapeType.RightArrow, 6143654, 3266595, 165992, 165992, false, smartArt);
    SmartArt.SetDefaultPropertiesForProcessList(smartArt._nodeCollection[1].ChildNodes[1], AutoShapeType.RoundedRectangle, 4329588, 3515585, 3794125, 948531, false, smartArt);
  }

  private static void SetDefaultPropertiesForProcessList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isParent,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    int rotation = 0;
    bool isColorSet = false;
    if (autoShapeType == AutoShapeType.RoundedRectangle)
    {
      smartArtShape.GetGuideList().Add("adj", "val 10000");
    }
    else
    {
      rotation = 5400000;
      smartArtShape.GetGuideList().Add("adj1", "val 66700");
      smartArtShape.GetGuideList().Add("adj2", "val 50000");
    }
    if (isParent)
      isColorSet = true;
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, isParent);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, isColorSet, 5400);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForStaggeredProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForStaggeredProcess(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 0, 0, 6908800, 1625600, smartArt);
    SmartArt.SetDefaultPropertiesForStaggeredProcess(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 609599, 1896533, 6908800, 1625600, smartArt);
    SmartArt.SetDefaultPropertiesForStaggeredProcess(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 1219199, 3793066, 6908800, 1625600, smartArt);
    SmartArt.SetDefaultPropertiesForStaggeredProcess((ISmartArtNode) null, AutoShapeType.DownArrow, 5852159, 1232746, 1056640, 1056640, smartArt);
    SmartArt.SetDefaultPropertiesForStaggeredProcess((ISmartArtNode) null, AutoShapeType.DownArrow, 6461759, 3118442, 1056640, 1056640, smartArt);
  }

  private static void SetDefaultPropertiesForStaggeredProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    if (autoShapeType == AutoShapeType.RoundedRectangle)
    {
      smartArtShape.GetGuideList().Add("adj", "val 10000");
    }
    else
    {
      smartArtShape.GetGuideList().Add("adj1", "val 55000");
      smartArtShape.GetGuideList().Add("adj2", "val 45000");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.RoundedRectangle)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Left, true, 6500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForVerticalProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForVerticalProcess(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 2844799, 0, 2438400, 1354666, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalProcess((ISmartArtNode) null, AutoShapeType.RightArrow, 3809999, 1388533, 508000, 609600, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalProcess(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 2844799, 2032000, 2438400, 1354666, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalProcess((ISmartArtNode) null, AutoShapeType.RightArrow, 3809999, 3420533, 508000, 609600, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalProcess(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 2844799, 4064000, 2438400, 1354666, smartArt);
  }

  private static void SetDefaultPropertiesForVerticalProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    int rotation = 0;
    if (autoShapeType == AutoShapeType.RoundedRectangle)
    {
      smartArtShape.GetGuideList().Add("adj", "val 10000");
    }
    else
    {
      smartArtShape.GetGuideList().Add("adj1", "val 60000");
      smartArtShape.GetGuideList().Add("adj2", "val 50000");
      rotation = 5400000;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.RoundedRectangle)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 5900);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForRandomToResultProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForRandomProcess(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 195729, 1313340, 2920666, 962492, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 195729, 3342904, 2920666, 1803241, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Oval, 192410, 1020609, 232325, 232325, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Oval, 355038, 695353, 232325, 232325, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Oval, 745345, 760404, 365083, 365083, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Oval, 1070601, 402623, 232325, 232325, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Oval, 1493434, 272520, 232325, 232325, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Oval, 2013843, 500200, 232325, 232325, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Oval, 2339099, 662828, 365083, 365083, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Oval, 2794458, 1020609, 232325, 232325, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Oval, 2989611, 1378391, 232325, 232325, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Oval, 1298280, 695353, 597408, 597408, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Oval, 29782, 1931326, 232325, 232325, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Oval, 224935, 2224056, 365083, 365083, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Oval, 712819, 2484261, 531030, 531030, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Oval, 1395857, 2907094, 232325, 232325, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Oval, 1525959, 2484261, 365083, 365083, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Oval, 1851215, 2939620, 232325, 232325, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Oval, 2143946, 2419210, 531030, 531030, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Oval, 2859509, 2289108, 365083, 365083, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Chevron, 3224592, 759863, 1072197, 2046942, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(AutoShapeType.Chevron, 4101845, 759863, 1072197, 2046942, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(smartArt._nodeCollection[1], AutoShapeType.Oval, 5393355, 614649, 2485548, 2485548, smartArt);
    SmartArt.SetDefaultPropertiesForRandomProcess(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.Rectangle, 5174042, 3342904, 2924175, 1803241, smartArt);
  }

  private static void SetDefaultPropertiesForRandomProcess(
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    if (autoShapeType == AutoShapeType.Chevron)
      smartArtShape.GetGuideList().Add("adj", "val 62310");
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    smartArt.DataModel.SmartArtShapeCollection.Add(Guid.NewGuid(), smartArtShape);
  }

  private static void SetDefaultPropertiesForRandomProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    bool isColorSet = false;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    else if (autoShapeType == AutoShapeType.Oval)
      isColorSet = true;
    if (autoShapeType == AutoShapeType.Chevron)
      smartArtShape.GetGuideList().Add("adj", "val 62310");
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, isColorSet, 6100);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForPhasedProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForPhasedProcess((ISmartArtNode) null, AutoShapeType.BlockArc, 206, 1273896, 2685891, 2686304, 5400000, smartArt);
    SmartArt.SetDefaultPropertiesForPhasedProcess((ISmartArtNode) null, AutoShapeType.BlockArc, 2764539, 1273896, 2685891, 2686304, 16200000, smartArt);
    SmartArt.SetDefaultPropertiesForPhasedProcess((ISmartArtNode) null, AutoShapeType.BlockArc, 2678382, 1273896, 2685891, 2686304, 5400000, smartArt);
    SmartArt.SetDefaultPropertiesForPhasedProcess((ISmartArtNode) null, AutoShapeType.BlockArc, 5441902, 1273896, 2685891, 2686304, 16200000, smartArt);
    SmartArt.SetDefaultPropertiesForPhasedProcess(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.Oval, 3016609, 2044799, 1230611, 1230611, 0, smartArt);
    SmartArt.SetDefaultPropertiesForPhasedProcess(smartArt._nodeCollection[1].ChildNodes[1], AutoShapeType.Oval, 3903537, 2044799, 1230611, 1230611, 0, smartArt);
    SmartArt.SetDefaultPropertiesForPhasedProcess(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Oval, 774737, 1681837, 851048, 851068, 0, smartArt);
    SmartArt.SetDefaultPropertiesForPhasedProcess((ISmartArtNode) null, AutoShapeType.Oval, 460857, 2393368, 418041, 417874, 0, smartArt);
    SmartArt.SetDefaultPropertiesForPhasedProcess((ISmartArtNode) null, AutoShapeType.Oval, 1695625, 1849245, 243242, 243083, 0, smartArt);
    SmartArt.SetDefaultPropertiesForPhasedProcess(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.Oval, 1605233, 2190153, 851048, 851068, 0, smartArt);
    SmartArt.SetDefaultPropertiesForPhasedProcess((ISmartArtNode) null, AutoShapeType.Oval, 1694229, 3093271, 243242, 243083, 0, smartArt);
    SmartArt.SetDefaultPropertiesForPhasedProcess(smartArt._nodeCollection[0].ChildNodes[2], AutoShapeType.Oval, 789903, 2676504, 851048, 851068, 0, smartArt);
    SmartArt.SetDefaultPropertiesForPhasedProcessForLastShape(smartArt._nodeCollection[2].ChildNodes[0], AutoShapeType.Oval, 5795264, 1829249, 1568704, 1568420, 0, smartArt);
    SmartArt.SetDefaultPropertiesForPhasedProcessForLastShape(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 3082137, 3607213, 2039315, 537350, 0, smartArt);
    SmartArt.SetDefaultPropertiesForPhasedProcessForLastShape(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 5563616, 3607213, 2039315, 537350, 0, smartArt);
    SmartArt.SetDefaultPropertiesForPhasedProcessForLastShape(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 504748, 3607213, 2039315, 537350, 0, smartArt);
  }

  private static void SetDefaultPropertiesForPhasedProcessForLastShape(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    bool isColorSet = false;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    else
      isColorSet = true;
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties, "accent1");
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, isColorSet, 3000);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void SetDefaultPropertiesForPhasedProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    int rotation,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    if (autoShapeType == AutoShapeType.BlockArc)
    {
      smartArtShape.GetGuideList().Add("adj1", "val 13500000");
      smartArtShape.GetGuideList().Add("adj2", "val 18900000");
      smartArtShape.GetGuideList().Add("adj3", "val 4960");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForSubStepProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForSubStepProcess(smartArt._nodeCollection[0], AutoShapeType.Oval, 4255, 1827278, 1764109, 1764109, smartArt);
    SmartArt.SetDefaultPropertiesForSubStepProcess((ISmartArtNode) null, AutoShapeType.Line, 2244674, 2270690, 206199, 0, smartArt);
    SmartArt.SetDefaultPropertiesForSubStepProcess((ISmartArtNode) null, AutoShapeType.Line, 3913016, 2270690, 206199, 0, smartArt);
    SmartArt.SetDefaultPropertiesForSubStepProcess((ISmartArtNode) null, AutoShapeType.Line, 2244674, 3147976, 206199, 0, smartArt);
    SmartArt.SetDefaultPropertiesForSubStepProcess((ISmartArtNode) null, AutoShapeType.Line, 3913016, 3147976, 206199, 0, smartArt);
    SmartArt.SetDefaultPropertiesForSubStepProcess(smartArt._nodeCollection[1], AutoShapeType.Oval, 4595526, 1827278, 1764109, 1764109, smartArt);
    SmartArt.SetDefaultPropertiesForSubStepProcess(smartArt._nodeCollection[2], AutoShapeType.Oval, 6359635, 1827278, 1764109, 1764109, smartArt);
    SmartArt.SetDefaultPropertiesForSubStepProcess(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 2450873, 1832047, 1462143, 877285, smartArt);
    SmartArt.SetDefaultPropertiesForSubStepProcess(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.Rectangle, 2450873, 2709333, 1462143, 877285, smartArt);
  }

  private static void SetDefaultPropertiesForSubStepProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    bool isColorSet = false;
    switch (autoShapeType)
    {
      case AutoShapeType.Rectangle:
        hasFillProperties = false;
        hasLineProperties = false;
        break;
      case AutoShapeType.Oval:
        isColorSet = true;
        break;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, isColorSet, 4300);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForChevronList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForChevronList(smartArt._nodeCollection[0], AutoShapeType.Chevron, 2178, 501565, 3365499, 1346199, true, smartArt);
    SmartArt.SetDefaultPropertiesForChevronList(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Chevron, 2930163, 615992, 2793365, 1117346, false, smartArt);
    SmartArt.SetDefaultPropertiesForChevronList(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.Chevron, 5332456, 615992, 2793365, 1117346, false, smartArt);
    SmartArt.SetDefaultPropertiesForChevronList(smartArt._nodeCollection[1], AutoShapeType.Chevron, 2178, 2036233, 3365499, 1346199, true, smartArt);
    SmartArt.SetDefaultPropertiesForChevronList(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.Chevron, 2930163, 2150660, 2793365, 1117346, false, smartArt);
    SmartArt.SetDefaultPropertiesForChevronList(smartArt._nodeCollection[1].ChildNodes[1], AutoShapeType.Chevron, 5332456, 2150660, 2793365, 1117346, false, smartArt);
    SmartArt.SetDefaultPropertiesForChevronList(smartArt._nodeCollection[2], AutoShapeType.Chevron, 2178, 3570901, 3365499, 1346199, true, smartArt);
    SmartArt.SetDefaultPropertiesForChevronList(smartArt._nodeCollection[2].ChildNodes[0], AutoShapeType.Chevron, 2930163, 3685328, 2793365, 1117346, false, smartArt);
    SmartArt.SetDefaultPropertiesForChevronList(smartArt._nodeCollection[2].ChildNodes[1], AutoShapeType.Chevron, 5332456, 3685328, 2793365, 1117346, false, smartArt);
  }

  private static void SetDefaultPropertiesForChevronList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isParent,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, isParent);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (isParent)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6100);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForClosedChevronProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForClosedChevronProcess(smartArt._nodeCollection[0], AutoShapeType.Pentagon, 3571, 2084652, 3123406, 1249362, smartArt);
    SmartArt.SetDefaultPropertiesForClosedChevronProcess(smartArt._nodeCollection[1], AutoShapeType.Chevron, 2502296, 2084652, 3123406, 1249362, smartArt);
    SmartArt.SetDefaultPropertiesForClosedChevronProcess(smartArt._nodeCollection[2], AutoShapeType.Chevron, 5001021, 2084652, 3123406, 1249362, smartArt);
  }

  private static void SetDefaultPropertiesForClosedChevronProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6400);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBasicChevronProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBasicChevronProcess(smartArt._nodeCollection[0], AutoShapeType.Chevron, 2381, 2129102, 2901156, 1160462, smartArt);
    SmartArt.SetDefaultPropertiesForBasicChevronProcess(smartArt._nodeCollection[1], AutoShapeType.Chevron, 2613421, 2129102, 2901156, 1160462, smartArt);
    SmartArt.SetDefaultPropertiesForBasicChevronProcess(smartArt._nodeCollection[2], AutoShapeType.Chevron, 5224462, 2129102, 2901156, 1160462, smartArt);
  }

  private static void SetDefaultPropertiesForBasicChevronProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 5100);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBasicTimeLine(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBasicTimeLine((ISmartArtNode) null, AutoShapeType.NotchedRightArrow, 0, 1625600, 8128000, 2167466, smartArt);
    SmartArt.SetDefaultPropertiesForBasicTimeLine(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 3571, 0, 2357437, 2167466, smartArt);
    SmartArt.SetDefaultPropertiesForBasicTimeLine(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 2478881, 3251200, 2357437, 2167466, smartArt);
    SmartArt.SetDefaultPropertiesForBasicTimeLine(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 4954190, 0, 2357437, 2167466, smartArt);
    SmartArt.SetDefaultPropertiesForBasicTimeLine((ISmartArtNode) null, AutoShapeType.Oval, 911357, 2438400, 541866, 541866, smartArt);
    SmartArt.SetDefaultPropertiesForBasicTimeLine((ISmartArtNode) null, AutoShapeType.Oval, 3386666, 2438400, 541866, 541866, smartArt);
    SmartArt.SetDefaultPropertiesForBasicTimeLine((ISmartArtNode) null, AutoShapeType.Oval, 5861976, 2438400, 541866, 541866, smartArt);
  }

  private static void SetDefaultPropertiesForBasicTimeLine(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasLineProperties = false;
      hasFillProperties = false;
    }
    if (autoShapeType == AutoShapeType.Donut)
      smartArtShape.GetGuideList().Add("adj", "val 20000");
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForCircleAccentTimeLine(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForCircleAccentTimeLine((ISmartArtNode) null, AutoShapeType.Donut, 2841, 2210265, 1627071, 1627071, smartArt);
    SmartArt.SetDefaultPropertiesForCircleAccentTimeLine((ISmartArtNode) null, AutoShapeType.Oval, 1752469, 2601524, 844553, 844553, smartArt);
    SmartArt.SetDefaultPropertiesForCircleAccentTimeLine((ISmartArtNode) null, AutoShapeType.Oval, 2719450, 2601524, 844553, 844553, smartArt);
    SmartArt.SetDefaultPropertiesForCircleAccentTimeLine((ISmartArtNode) null, AutoShapeType.Donut, 3686560, 2210265, 1627071, 1627071, smartArt);
    SmartArt.SetDefaultPropertiesForCircleAccentTimeLine((ISmartArtNode) null, AutoShapeType.Oval, 5436189, 2601524, 844553, 844553, smartArt);
    SmartArt.SetDefaultPropertiesForCircleAccentTimeLine((ISmartArtNode) null, AutoShapeType.Oval, 6403169, 2601524, 844553, 844553, smartArt);
    SmartArt.SetDefaultPropertiesForCircleAccentTimeLine(smartArt.Nodes[0], AutoShapeType.Rectangle, 576147, 883869, 2022631, 974751, smartArt);
    SmartArt.SetDefaultPropertiesForCircleAccentTimeLine(smartArt.Nodes[0].ChildNodes[0], AutoShapeType.Rectangle, 752212, 3777009, 1749671, 843625, smartArt);
    SmartArt.SetDefaultPropertiesForCircleAccentTimeLine(smartArt.Nodes[0].ChildNodes[1], AutoShapeType.Rectangle, 1719192, 3777009, 1749671, 843625, smartArt);
    SmartArt.SetDefaultPropertiesForCircleAccentTimeLine(smartArt.Nodes[1], AutoShapeType.Rectangle, 4259867, 883869, 2022631, 974751, smartArt);
    SmartArt.SetDefaultPropertiesForCircleAccentTimeLine(smartArt.Nodes[1].ChildNodes[0], AutoShapeType.Rectangle, 4435931, 3777009, 1749671, 843625, smartArt);
    SmartArt.SetDefaultPropertiesForCircleAccentTimeLine(smartArt.Nodes[1].ChildNodes[1], AutoShapeType.Rectangle, 5402911, 3777009, 1749671, 843625, smartArt);
  }

  private static void SetDefaultPropertiesForCircleAccentTimeLine(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasLineProperties = true;
    bool hasFillProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasLineProperties = false;
      hasFillProperties = false;
    }
    if (autoShapeType == AutoShapeType.Donut)
      smartArtShape.GetGuideList().Add("adj", "val 20000");
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForProcessArrows(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForProcessArrows(smartArt._nodeCollection[0], AutoShapeType.RightArrow, 527843, 1793468, 2095500, 1831730, smartArt);
    SmartArt.SetDefaultPropertiesForProcessArrows(smartArt._nodeCollection[0], AutoShapeType.Oval, 3968, 2185458, 1047750, 1047750, smartArt);
    SmartArt.SetDefaultPropertiesForProcessArrows(smartArt._nodeCollection[1], AutoShapeType.RightArrow, 3278187, 1793468, 2095500, 1831730, smartArt);
    SmartArt.SetDefaultPropertiesForProcessArrows(smartArt._nodeCollection[1], AutoShapeType.Oval, 2754312, 2185458, 1047750, 1047750, smartArt);
    SmartArt.SetDefaultPropertiesForProcessArrows(smartArt._nodeCollection[2], AutoShapeType.RightArrow, 6028531, 1793468, 2095500, 1831730, smartArt);
    SmartArt.SetDefaultPropertiesForProcessArrows(smartArt._nodeCollection[2], AutoShapeType.Oval, 5504656, 2185458, 1047750, 1047750, smartArt);
  }

  private static void SetDefaultPropertiesForProcessArrows(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    Guid key = Guid.NewGuid();
    bool isColorSet = false;
    if (smartArtNode != null)
    {
      SmartArtNode smartArtNode1 = (SmartArtNode) smartArtNode;
      if (autoShapeType == AutoShapeType.RightArrow)
      {
        SmartArtNode childNode1 = (SmartArtNode) smartArtNode1.ChildNodes[0];
        key = childNode1.Id;
        if (childNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
        SmartArtNode childNode2 = (SmartArtNode) smartArtNode1.ChildNodes[1];
        if (childNode2.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[1] as Paragraph).NodeId = childNode2.Id;
      }
      else
      {
        key = smartArtNode1.Id;
        if (smartArtNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
        isColorSet = true;
      }
    }
    if (autoShapeType == AutoShapeType.RightArrow)
    {
      smartArtShape.GetGuideList().Add("adj1", "val 70000");
      smartArtShape.GetGuideList().Add("adj2", "val 50000");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, isColorSet, 2400);
    smartArt.DataModel.SmartArtShapeCollection.Add(key, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForContinuousArrowProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForContinuousArrowProcess((ISmartArtNode) null, AutoShapeType.RightArrow, 0, 369333, 8128000, 4680000, smartArt);
    SmartArt.SetDefaultPropertiesForContinuousArrowProcess(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 5356621, 1539333, 1958578, 2340000, smartArt);
    SmartArt.SetDefaultPropertiesForContinuousArrowProcess(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 3006328, 1539333, 1958578, 2340000, smartArt);
    SmartArt.SetDefaultPropertiesForContinuousArrowProcess(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 656034, 1539333, 1958578, 2340000, smartArt);
  }

  private static void SetDefaultPropertiesForContinuousArrowProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForIncreasingArrowProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForIncreasingArrowProcess(smartArt._nodeCollection[0], AutoShapeType.RightArrow, 0, 734852, 8128000, 1183746, smartArt);
    SmartArt.SetDefaultPropertiesForIncreasingArrowProcess(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 0, 1647691, 2503424, 2280331, smartArt);
    SmartArt.SetDefaultPropertiesForIncreasingArrowProcess(smartArt._nodeCollection[1], AutoShapeType.RightArrow, 2503423, 1129434, 5624576, 1183746, smartArt);
    SmartArt.SetDefaultPropertiesForIncreasingArrowProcess(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.Rectangle, 2503423, 2042273, 2503424, 2280331, smartArt);
    SmartArt.SetDefaultPropertiesForIncreasingArrowProcess(smartArt._nodeCollection[2], AutoShapeType.RightArrow, 5006848, 1524016, 3121152, 1183746, smartArt);
    SmartArt.SetDefaultPropertiesForIncreasingArrowProcess(smartArt._nodeCollection[2].ChildNodes[0], AutoShapeType.Rectangle, 5006848, 2436855, 2503424, 2246958, smartArt);
  }

  private static void SetDefaultPropertiesForIncreasingArrowProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool isColorSet = false;
    if (autoShapeType == AutoShapeType.RightArrow)
    {
      smartArtShape.GetGuideList().Add("adj1", "val 50000");
      smartArtShape.GetGuideList().Add("adj2", "val 50000");
      isColorSet = true;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Left, isColorSet, 2200);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForContinuousBlockProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForContinuousProcess((ISmartArtNode) null, AutoShapeType.RightArrow, 609599, 0, 6908800, 5418667, smartArt);
    SmartArt.SetDefaultPropertiesForContinuousProcess(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 0, 1625600, 2438400, 2167466, smartArt);
    SmartArt.SetDefaultPropertiesForContinuousProcess(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 2844799, 1625600, 2438400, 2167466, smartArt);
    SmartArt.SetDefaultPropertiesForContinuousProcess(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 5689600, 1625600, 2438400, 2167466, smartArt);
  }

  private static void SetDefaultPropertiesForContinuousProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (smartArtNode != null)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6100);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForAlternatingFlow(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForAlternatingFlow(smartArt.Nodes[0], AutoShapeType.RoundedRectangle, 141, 1774586, 2266626, 1869493, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingFlow((ISmartArtNode) null, AutoShapeType.CircularArrow, 1300946, 2316885, 2356306, 2356306, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingFlow(smartArt.Nodes[0], AutoShapeType.RoundedRectangle, 503836, 3243474, 2014779, 801211, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingFlow(smartArt.Nodes[1], AutoShapeType.RoundedRectangle, 2804762, 1774586, 2266626, 1869493, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingFlow((ISmartArtNode) null, AutoShapeType.CircularArrow, 4086679, 672173, 2645930, 2645930, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingFlow(smartArt.Nodes[1], AutoShapeType.RoundedRectangle, 3308457, 1373981, 2014779, 801211, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingFlow(smartArt.Nodes[2], AutoShapeType.RoundedRectangle, 5609383, 1774586, 2266626, 1869493, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingFlow(smartArt.Nodes[2], AutoShapeType.RoundedRectangle, 6113078, 3243474, 2014779, 801211, "accent1", smartArt);
  }

  private static void SetDefaultPropertiesForAlternatingFlow(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string colorString,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    Guid key = Guid.NewGuid();
    bool isColorSet = false;
    if (smartArtNode != null)
    {
      SmartArtNode smartArtNode1 = (SmartArtNode) smartArtNode;
      if (colorString == "lt1")
      {
        SmartArtNode childNode1 = (SmartArtNode) smartArtNode1.ChildNodes[0];
        key = childNode1.Id;
        if (childNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
        SmartArtNode childNode2 = (SmartArtNode) smartArtNode1.ChildNodes[1];
        if (childNode2.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[1] as Paragraph).NodeId = childNode2.Id;
      }
      else
      {
        key = smartArtNode1.Id;
        if (smartArtNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
        isColorSet = true;
      }
    }
    if (autoShapeType == AutoShapeType.RoundedRectangle)
    {
      smartArtShape.GetGuideList().Add("adj", "val 10000");
    }
    else
    {
      smartArtShape.GetGuideList().Add("adj1", "val 2271");
      smartArtShape.GetGuideList().Add("adj2", "val 273786");
      smartArtShape.GetGuideList().Add("adj3", "val 19550703");
      smartArtShape.GetGuideList().Add("adj4", "val 12575511");
      smartArtShape.GetGuideList().Add("adj5", "val 2650");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, colorString);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, isColorSet, 4500);
    smartArt.DataModel.SmartArtShapeCollection.Add(key, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForPictureAccentProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForPictureAccentProcess((ISmartArtNode) null, AutoShapeType.RoundedRectangle, 4042, 1185714, 1904523, 1904523, true, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentProcess(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 314081, 2328428, 1904523, 1904523, false, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentProcess((ISmartArtNode) null, AutoShapeType.RightArrow, 2275419, 1909161, 366853, 457630, false, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentProcess((ISmartArtNode) null, AutoShapeType.RoundedRectangle, 2956718, 1185714, 1904523, 1904523, true, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentProcess(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 3266757, 2328428, 1904523, 1904523, false, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentProcess((ISmartArtNode) null, AutoShapeType.RightArrow, 5228095, 1909161, 366853, 457630, false, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentProcess((ISmartArtNode) null, AutoShapeType.RoundedRectangle, 5909394, 1185714, 1904523, 1904523, true, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentProcess(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 6219433, 2328428, 1904523, 1904523, false, smartArt);
  }

  private static void SetDefaultPropertiesForPictureAccentProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isPicture,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    Guid key = Guid.NewGuid();
    if (smartArtNode != null)
    {
      SmartArtNode smartArtNode1 = (SmartArtNode) smartArtNode;
      if (!isPicture)
      {
        key = smartArtNode1.Id;
        if (smartArtNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
        SmartArtNode childNode1 = (SmartArtNode) smartArtNode1.ChildNodes[0];
        if (childNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[1] as Paragraph).NodeId = childNode1.Id;
        SmartArtNode childNode2 = (SmartArtNode) smartArtNode1.ChildNodes[1];
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs.Add());
        (smartArtShape.TextBody.Paragraphs[2] as Paragraph).NodeId = childNode2.Id;
      }
      else
      {
        key = smartArtNode1.Id;
        if (smartArtNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
      }
    }
    if (autoShapeType == AutoShapeType.RoundedRectangle)
    {
      smartArtShape.GetGuideList().Add("adj", "val 10000");
    }
    else
    {
      smartArtShape.GetGuideList().Add("adj1", "val 60000");
      smartArtShape.GetGuideList().Add("adj2", "val 50000");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, isPicture);
    if (!isPicture)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Left, true, 3500);
    smartArt.DataModel.SmartArtShapeCollection.Add(key, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForAccentProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForAccentProcess(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 4042, 1506516, 1838086, 1252799, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForAccentProcess(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.RoundedRectangle, 380518, 2241750, 1838086, 1670400, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForAccentProcess((ISmartArtNode) null, AutoShapeType.RightArrow, 2120776, 1645318, 590732, 457630, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForAccentProcess(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 2956718, 1506516, 1838086, 1252799, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForAccentProcess(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.RoundedRectangle, 3333194, 2241750, 1838086, 1670400, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForAccentProcess((ISmartArtNode) null, AutoShapeType.RightArrow, 5073452, 1645318, 590732, 457630, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForAccentProcess(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 5909394, 1506516, 1838086, 1252799, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForAccentProcess(smartArt._nodeCollection[2].ChildNodes[0], AutoShapeType.RoundedRectangle, 6285870, 2241750, 1838086, 1670400, "lt1", smartArt);
  }

  private static void SetDefaultPropertiesForAccentProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string colorString,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool isColorSet = false;
    if (autoShapeType == AutoShapeType.RoundedRectangle)
    {
      isColorSet = true;
      smartArtShape.GetGuideList().Add("adj", "val 10000");
    }
    else
    {
      smartArtShape.GetGuideList().Add("adj1", "val 60000");
      smartArtShape.GetGuideList().Add("adj2", "val 50000");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, colorString);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Left, isColorSet, 2900);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForStepDownProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForStepDownProcess((ISmartArtNode) null, AutoShapeType.BentUpArrow, 445009, 1583167, 1400175, 1594049, smartArt);
    SmartArt.SetDefaultPropertiesForStepDownProcess(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 74048, 31045, 2357070, 1649872, smartArt);
    SmartArt.SetDefaultPropertiesForStepDownProcess((ISmartArtNode) null, AutoShapeType.BentUpArrow, 2399271, 3436519, 1400175, 1594049, smartArt);
    SmartArt.SetDefaultPropertiesForStepDownProcess(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 2028310, 1884397, 2357070, 1649872, smartArt);
    SmartArt.SetDefaultPropertiesForStepDownProcess(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 3982572, 3737748, 2357070, 1649872, smartArt);
    SmartArt.SetDefaultPropertiesForStepDownProcess(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 2431119, 188398, 1714308, 1333500, smartArt);
    SmartArt.SetDefaultPropertiesForStepDownProcess(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 4385381, 2041750, 1714308, 1333500, smartArt);
    SmartArt.SetDefaultPropertiesForStepDownProcess(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 6339643, 3895101, 1714308, 1333500, smartArt);
  }

  private static void SetDefaultPropertiesForStepDownProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    Guid key = Guid.NewGuid();
    if (smartArtNode != null)
    {
      SmartArtNode smartArtNode1 = (SmartArtNode) smartArtNode;
      if (autoShapeType == AutoShapeType.Rectangle)
      {
        SmartArtNode childNode = (SmartArtNode) smartArtNode1.ChildNodes[0];
        key = childNode.Id;
        if (childNode.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
        hasFillProperties = false;
        hasLineProperties = false;
      }
      else
      {
        key = smartArtNode1.Id;
        if (smartArtNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
      }
    }
    int rotation = 0;
    if (autoShapeType == AutoShapeType.BentUpArrow)
    {
      rotation = 5400000;
      smartArtShape.GetGuideList().Add("adj1", "val 32840");
      smartArtShape.GetGuideList().Add("adj2", "val 25000");
      smartArtShape.GetGuideList().Add("adj3", "val 35780");
    }
    else
      smartArtShape.GetGuideList().Add("adj", "val 16670");
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    if (autoShapeType == AutoShapeType.RoundedRectangle)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6000);
    smartArt.DataModel.SmartArtShapeCollection.Add(key, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForStepUpProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForStepUpProcess((ISmartArtNode) null, AutoShapeType.Corner, 507673, 1770520, 1519334, 2528139, smartArt);
    SmartArt.SetDefaultPropertiesForStepUpProcess((ISmartArtNode) null, AutoShapeType.IsoscelesTriangle, 2105832, 1584396, 430644, 430644, smartArt);
    SmartArt.SetDefaultPropertiesForStepUpProcess((ISmartArtNode) null, AutoShapeType.Corner, 3301799, 1079111, 1519334, 2528139, smartArt);
    SmartArt.SetDefaultPropertiesForStepUpProcess((ISmartArtNode) null, AutoShapeType.IsoscelesTriangle, 4899957, 892986, 430644, 430644, smartArt);
    SmartArt.SetDefaultPropertiesForStepUpProcess((ISmartArtNode) null, AutoShapeType.Corner, 6095925, 387702, 1519334, 2528139, smartArt);
    SmartArt.SetDefaultPropertiesForStepUpProcess(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 254058, 2525889, 2282418, 2000673, smartArt);
    SmartArt.SetDefaultPropertiesForStepUpProcess(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 3048184, 1834480, 2282418, 2000673, smartArt);
    SmartArt.SetDefaultPropertiesForStepUpProcess(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 5842310, 1143070, 2282418, 2000673, smartArt);
  }

  private static void SetDefaultPropertiesForStepUpProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    int rotation = 0;
    if (autoShapeType == AutoShapeType.Corner)
    {
      rotation = 5400000;
      smartArtShape.GetGuideList().Add("adj1", "val 16120");
      smartArtShape.GetGuideList().Add("adj2", "val 16110");
    }
    else
      smartArtShape.GetGuideList().Add("adj", "val 100000");
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBasicProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBasicProcess(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 7143, 2068777, 2135187, 1281112, smartArt);
    SmartArt.SetDefaultPropertiesForBasicProcess((ISmartArtNode) null, AutoShapeType.RightArrow, 2355850, 2444570, 452659, 529526, smartArt);
    SmartArt.SetDefaultPropertiesForBasicProcess(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 2996406, 2068777, 2135187, 1281112, smartArt);
    SmartArt.SetDefaultPropertiesForBasicProcess((ISmartArtNode) null, AutoShapeType.RightArrow, 5345112, 2444570, 452659, 529526, smartArt);
    SmartArt.SetDefaultPropertiesForBasicProcess(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 5985668, 2068777, 2135187, 1281112, smartArt);
  }

  private static void SetDefaultPropertiesForBasicProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasLineProperties = true;
    bool isColorSet = false;
    if (autoShapeType == AutoShapeType.RoundedRectangle)
    {
      smartArtShape.GetGuideList().Add("adj", "val 10000");
      isColorSet = true;
    }
    else
    {
      smartArtShape.GetGuideList().Add("adj1", "val 60000");
      smartArtShape.GetGuideList().Add("adj2", "val 50000");
      hasLineProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, isColorSet, 5500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static Guid InitializeParagraphAndGenerateGuid(
    ISmartArtNode smartArtNode,
    SmartArtShape smartArtShape)
  {
    Guid guid = Guid.NewGuid();
    if (smartArtNode != null)
    {
      SmartArtNode smartArtNode1 = (SmartArtNode) smartArtNode;
      guid = smartArtNode1.Id;
      if (smartArtNode1.TextBody.Paragraphs.Count != 0)
      {
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = guid;
      }
      if (smartArtNode.Shapes[0].Fill.FillType != FillType.Automatic)
        smartArtShape.SetFill(smartArtNode.Shapes[0].Fill as Syncfusion.Presentation.Drawing.Fill);
    }
    return guid;
  }

  private static void InitializeDefaultPropertiesForTableHierarchy(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForTableHierarchy(smartArt._nodeCollection[0], 932, 1947, 8126134, 1709208, smartArt);
    SmartArt.SetDefaultPropertiesForTableHierarchy(smartArt._nodeCollection[0].ChildNodes[0], 932, 1854729, 5308242, 1709208, smartArt);
    SmartArt.SetDefaultPropertiesForTableHierarchy(smartArt._nodeCollection[0].ChildNodes[0].ChildNodes[0], 932, 3707511, 2599531, 1709208, smartArt);
    SmartArt.SetDefaultPropertiesForTableHierarchy(smartArt._nodeCollection[0].ChildNodes[0].ChildNodes[1], 2709644, 3707511, 2599531, 1709208, smartArt);
    SmartArt.SetDefaultPropertiesForTableHierarchy(smartArt._nodeCollection[0].ChildNodes[1], 5527536, 1854729, 2599531, 1709208, smartArt);
    SmartArt.SetDefaultPropertiesForTableHierarchy(smartArt._nodeCollection[0].ChildNodes[1].ChildNodes[0], 5527536, 3707511, 2599531, 1709208, smartArt);
  }

  private static void SetDefaultPropertiesForTableHierarchy(
    ISmartArtNode smartArtNode,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    smartArtShape.GetGuideList().Add("adj", "val 10000");
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, AutoShapeType.RoundedRectangle, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForVerticalCircleList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForVerticalCircleList(2498328, 2286, 2846391, 2846391, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalCircleList(2632975, 121835, 512350, 512350, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalCircleList(2889151, 899225, 95142, 95142, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalCircleList(2498328, 2569988, 2846391, 2846391, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalCircleList(2632975, 2689537, 512350, 512350, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalCircleList(2889151, 3461517, 93200, 93200, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalCircleList(smartArt._nodeCollection[0], 2889151, 121835, 2740520, 512350, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalCircleList(smartArt._nodeCollection[0].ChildNodes[0], 2889151, 634185, 2740520, 327580, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalCircleList(smartArt._nodeCollection[0].ChildNodes[1], 2889151, 1060791, 2740520, 327580, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalCircleList(smartArt._nodeCollection[1], 2889151, 2689537, 2740520, 512350, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalCircleList(smartArt._nodeCollection[1].ChildNodes[0], 2889151, 3201887, 2740520, 316777, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalCircleList(smartArt._nodeCollection[1].ChildNodes[1], 2889151, 3614425, 2740520, 316777, smartArt);
  }

  private static void SetDefaultPropertiesForVerticalCircleList(
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, AutoShapeType.Oval, false, true, true);
    smartArt.DataModel.SmartArtShapeCollection.Add(Guid.NewGuid(), smartArtShape);
  }

  private static void SetDefaultPropertiesForVerticalCircleList(
    ISmartArtNode smartArtNode,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, AutoShapeType.Rectangle, false, false, false);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForHierarchyList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForHierarchyList(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 581421, 661, 3095624, 1547812, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForHierarchyList(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.RoundedRectangle, 1200546, 1935427, 2476499, 1547812, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForHierarchyList(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.RoundedRectangle, 1200546, 3870192, 2476499, 1547812, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForHierarchyList(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 4450953, 661, 3095624, 1547812, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForHierarchyList(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.RoundedRectangle, 5070078, 1935427, 2476499, 1547812, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForHierarchyList(smartArt._nodeCollection[1].ChildNodes[1], AutoShapeType.RoundedRectangle, 5070078, 3870192, 2476499, 1547812, "lt1", smartArt);
  }

  private static void SetDefaultPropertiesForHierarchyList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string colorString,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    smartArtShape.GetGuideList().Add("adj", "val 10000");
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, colorString);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (colorString == "accent1")
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6500);
    else
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, false, 6500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForTargetList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForTargetList((ISmartArtNode) null, AutoShapeType.Pie, 0, 270933, 4876800, 4876800, false, smartArt);
    SmartArt.SetDefaultPropertiesForTargetList(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 2438400, 270933, 5689599, 4876800, false, smartArt);
    SmartArt.SetDefaultPropertiesForTargetList((ISmartArtNode) null, AutoShapeType.Pie, 853441, 1733976, 3169916, 3169916, false, smartArt);
    SmartArt.SetDefaultPropertiesForTargetList(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 2438400, 1733976, 5689599, 3169916, false, smartArt);
    SmartArt.SetDefaultPropertiesForTargetList((ISmartArtNode) null, AutoShapeType.Pie, 1706880, 3197014, 1463038, 1463038, false, smartArt);
    SmartArt.SetDefaultPropertiesForTargetList(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 2438400, 3197014, 5689599, 1463038, false, smartArt);
    SmartArt.SetDefaultPropertiesForTargetList(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 5283200, 270933, 2844799, 1463043, true, smartArt);
    SmartArt.SetDefaultPropertiesForTargetList(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 5283200, 1733976, 2844799, 1463038, true, smartArt);
    SmartArt.SetDefaultPropertiesForTargetList(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 5283200, 3197014, 2844799, 1463038, true, smartArt);
  }

  private static void SetDefaultPropertiesForTargetList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isChild,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    Guid key = Guid.NewGuid();
    if (smartArtNode != null)
    {
      SmartArtNode smartArtNode1 = (SmartArtNode) smartArtNode;
      if (isChild)
      {
        SmartArtNode childNode1 = (SmartArtNode) smartArtNode1.ChildNodes[0];
        key = childNode1.Id;
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs.Add());
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
        SmartArtNode childNode2 = (SmartArtNode) smartArtNode1.ChildNodes[1];
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs.Add());
        (smartArtShape.TextBody.Paragraphs[1] as Paragraph).NodeId = childNode2.Id;
        hasFillProperties = false;
        hasLineProperties = false;
      }
      else
      {
        key = smartArtNode1.Id;
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs.Add());
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
      }
    }
    string colorString = "lt1";
    if (autoShapeType == AutoShapeType.Pie)
    {
      smartArtShape.GetGuideList().Add("adj1", "val 5400000");
      smartArtShape.GetGuideList().Add("adj2", "val 16200000");
      colorString = "accent1";
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties, colorString);
    smartArt.DataModel.SmartArtShapeCollection.Add(key, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForPyramidList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForPyramidList((ISmartArtNode) null, AutoShapeType.IsoscelesTriangle, 948266, 0, 5418667, 5418667, smartArt);
    SmartArt.SetDefaultPropertiesForPyramidList(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 3657599, 544777, 3522133, 1282700, smartArt);
    SmartArt.SetDefaultPropertiesForPyramidList(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 3657599, 1987814, 3522133, 1282700, smartArt);
    SmartArt.SetDefaultPropertiesForPyramidList(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 3657599, 3430852, 3522133, 1282700, smartArt);
  }

  private static void SetDefaultPropertiesForPyramidList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    string colorString = "accent1";
    if (autoShapeType == AutoShapeType.RoundedRectangle)
      colorString = "lt1";
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, colorString);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForVerticalCurvedList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForVerticalCurvedList((ISmartArtNode) null, AutoShapeType.BlockArc, -6125176, -937410, 7293488, 7293488, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalCurvedList(smartArt.Nodes[0], AutoShapeType.Rectangle, 752110, 541866, 7301111, 1083733, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalCurvedList((ISmartArtNode) null, AutoShapeType.Oval, 74777, 406400, 1354666, 1354666, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalCurvedList(smartArt.Nodes[1], AutoShapeType.Rectangle, 1146048, 2167466, 6907174, 1083733, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalCurvedList((ISmartArtNode) null, AutoShapeType.Oval, 468714, 2032000, 1354666, 1354666, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalCurvedList(smartArt.Nodes[2], AutoShapeType.Rectangle, 752110, 3793066, 7301111, 1083733, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalCurvedList((ISmartArtNode) null, AutoShapeType.Oval, 74777, 3657600, 1354666, 1354666, smartArt);
  }

  private static void SetDefaultPropertiesForVerticalCurvedList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    string colorString = "accent1";
    switch (autoShapeType)
    {
      case AutoShapeType.Oval:
        colorString = "lt1";
        break;
      case AutoShapeType.BlockArc:
        hasFillProperties = false;
        smartArtShape.GetGuideList().Add("adj1", "val 18900000");
        smartArtShape.GetGuideList().Add("adj2", "val 2700000");
        smartArtShape.GetGuideList().Add("adj3", "val 296");
        colorString = "lt1";
        break;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, hasFillProperties, colorString);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.Rectangle)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Left, true, 5600);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForSegmentedProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForSegmentedProcess(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 0, 4078917, 8128000, 1338791, true, smartArt);
    SmartArt.SetDefaultPropertiesForSegmentedProcess(smartArt._nodeCollection[2].ChildNodes[0], AutoShapeType.Rectangle, 0, 4775089, 4064000, 615844, false, smartArt);
    SmartArt.SetDefaultPropertiesForSegmentedProcess(smartArt._nodeCollection[2].ChildNodes[1], AutoShapeType.Rectangle, 4064000, 4775089, 4064000, 615844, false, smartArt);
    SmartArt.SetDefaultPropertiesForSegmentedProcess(smartArt._nodeCollection[1], AutoShapeType.UpArrowCallout, 0, 2039937, 8128000, 2059061, false, smartArt);
    SmartArt.SetDefaultPropertiesForSegmentedProcess(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.Rectangle, 0, 2762668, 4064000, 615844, false, smartArt);
    SmartArt.SetDefaultPropertiesForSegmentedProcess(smartArt._nodeCollection[1].ChildNodes[1], AutoShapeType.Rectangle, 4064000, 2762668, 4064000, 615844, false, smartArt);
    SmartArt.SetDefaultPropertiesForSegmentedProcess(smartArt._nodeCollection[0], AutoShapeType.UpArrowCallout, 0, 957, 8128000, 2059061, false, smartArt);
    SmartArt.SetDefaultPropertiesForSegmentedProcess(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 0, 723688, 4064000, 615844, false, smartArt);
    SmartArt.SetDefaultPropertiesForSegmentedProcess(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.Rectangle, 4064000, 723688, 4064000, 615659, false, smartArt);
  }

  private static void SetDefaultPropertiesForSegmentedProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isParent,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    int rotation = 0;
    bool isColorSet = false;
    if (autoShapeType == AutoShapeType.UpArrowCallout)
      rotation = 10800000;
    if (isParent || autoShapeType == AutoShapeType.UpArrowCallout)
      isColorSet = true;
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, isParent);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, isColorSet, 2500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForTableList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForTableList(smartArt._nodeCollection[0], 0, 0, 8128000, 1625600, true, smartArt);
    SmartArt.SetDefaultPropertiesForTableList(smartArt._nodeCollection[0].ChildNodes[0], 3968, 1625600, 2706687, 3413760, false, smartArt);
    SmartArt.SetDefaultPropertiesForTableList(smartArt._nodeCollection[0].ChildNodes[1], 2710656, 1625600, 2706687, 3413760, false, smartArt);
    SmartArt.SetDefaultPropertiesForTableList(smartArt._nodeCollection[0].ChildNodes[2], 5417343, 1625600, 2706687, 3413760, false, smartArt);
    SmartArt.SetDefaultPropertiesForTableList((ISmartArtNode) null, 0, 5039360, 8128000, 379306, true, smartArt);
  }

  private static void SetDefaultPropertiesForTableList(
    ISmartArtNode smartArtNode,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isParent,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, AutoShapeType.Rectangle, false, true, true, isParent);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (smartArtNode != null)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForDescendingBlockList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForDescendingBlockList(smartArt._nodeCollection[2], 5326294, 1331366, 2135779, 4063458, true, smartArt);
    SmartArt.SetDefaultPropertiesForDescendingBlockList(smartArt._nodeCollection[1], 2999547, 645905, 2135779, 4746752, true, smartArt);
    SmartArt.SetDefaultPropertiesForDescendingBlockList(smartArt._nodeCollection[0], 665926, 0, 2135779, 5392657, true, smartArt);
    SmartArt.SetDefaultPropertiesForDescendingBlockList(smartArt._nodeCollection[2], 665926, 0, 1516403, 5418667, false, smartArt);
    SmartArt.SetDefaultPropertiesForDescendingBlockList(smartArt._nodeCollection[1], 2999547, 645905, 1516403, 4772761, false, smartArt);
    SmartArt.SetDefaultPropertiesForDescendingBlockList(smartArt._nodeCollection[0], 5326294, 1331366, 1516403, 4087300, false, smartArt);
  }

  private static void SetDefaultPropertiesForDescendingBlockList(
    ISmartArtNode smartArtNode,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isParent,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    Guid.NewGuid();
    Guid key;
    if (!isParent)
    {
      hasFillProperties = false;
      hasLineProperties = false;
      SmartArtNode smartArtNode1 = (SmartArtNode) smartArtNode;
      SmartArtNode childNode1 = (SmartArtNode) smartArtNode1.ChildNodes[0];
      key = childNode1.Id;
      if (childNode1.TextBody.Paragraphs.Count == 0)
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs.Add());
      else
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs[0]);
      (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
      SmartArtNode childNode2 = (SmartArtNode) smartArtNode1.ChildNodes[1];
      if (childNode2.TextBody.Paragraphs.Count == 0)
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs.Add());
      else
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs[0]);
      (smartArtShape.TextBody.Paragraphs[1] as Paragraph).NodeId = childNode2.Id;
    }
    else
      key = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, AutoShapeType.Rectangle, false, hasLineProperties, hasFillProperties, "accent1");
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Right, true, 3600);
    smartArt.DataModel.SmartArtShapeCollection.Add(key, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForTrapezoidList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForTrapezoidList(smartArt._nodeCollection[0], -1418497, 1419489, 5418667, 2579687, smartArt);
    SmartArt.SetDefaultPropertiesForTrapezoidList(smartArt._nodeCollection[1], 1354666, 1419489, 5418667, 2579687, smartArt);
    SmartArt.SetDefaultPropertiesForTrapezoidList(smartArt._nodeCollection[2], 4127830, 1419489, 5418667, 2579687, smartArt);
  }

  private static void SetDefaultPropertiesForTrapezoidList(
    ISmartArtNode smartArtNode,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    Guid key = Guid.NewGuid();
    if (smartArtNode != null)
    {
      SmartArtNode smartArtNode1 = (SmartArtNode) smartArtNode;
      key = smartArtNode1.Id;
      if (smartArtNode1.TextBody.Paragraphs.Count == 0)
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs.Add());
      else
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs[0]);
      (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
      SmartArtNode childNode1 = (SmartArtNode) smartArtNode1.ChildNodes[0];
      if (childNode1.TextBody.Paragraphs.Count == 0)
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs.Add());
      else
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs[0]);
      (smartArtShape.TextBody.Paragraphs[1] as Paragraph).NodeId = key;
      SmartArtNode childNode2 = (SmartArtNode) smartArtNode1.ChildNodes[1];
      if (childNode2.TextBody.Paragraphs.Count == 0)
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs.Add());
      else
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs[0]);
      (smartArtShape.TextBody.Paragraphs[2] as Paragraph).NodeId = childNode2.Id;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 16200000, offsetX, offsetY, offsetCX, offsetCY, AutoShapeType.FlowChartManualOperation, false, true, true, "accent1");
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6200);
    smartArt.DataModel.SmartArtShapeCollection.Add(key, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForVerticalArrowList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForVerticalArrowList(smartArt._nodeCollection[0], AutoShapeType.RightArrow, 3251199, 661, 4876800, 2579687, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalArrowList(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 0, 661, 3251200, 2579687, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalArrowList(smartArt._nodeCollection[1], AutoShapeType.RightArrow, 3251199, 2838317, 4876800, 2579687, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalArrowList(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 0, 2838317, 3251200, 2579687, "accent1", smartArt);
  }

  private static void SetDefaultPropertiesForVerticalArrowList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string colorString,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    Guid key = Guid.NewGuid();
    bool isColorSet = false;
    if (smartArtNode != null)
    {
      SmartArtNode smartArtNode1 = (SmartArtNode) smartArtNode;
      if (autoShapeType == AutoShapeType.RightArrow)
      {
        SmartArtNode childNode1 = (SmartArtNode) smartArtNode1.ChildNodes[0];
        key = childNode1.Id;
        if (childNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
        SmartArtNode childNode2 = (SmartArtNode) smartArtNode1.ChildNodes[1];
        if (childNode2.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[1] as Paragraph).NodeId = childNode2.Id;
      }
      else
      {
        key = smartArtNode1.Id;
        if (smartArtNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
        isColorSet = true;
      }
    }
    if (autoShapeType == AutoShapeType.RightArrow)
    {
      smartArtShape.GetGuideList().Add("adj1", "val 75000");
      smartArtShape.GetGuideList().Add("adj2", "val 50000");
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, colorString);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, isColorSet, 6500);
    smartArt.DataModel.SmartArtShapeCollection.Add(key, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForVerticalAccentList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 661227, 574540, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 1548845, 574540, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 2437166, 574540, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 3324785, 574540, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 4213105, 574540, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 5100724, 574540, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 5989045, 574540, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 661227, 691486, 6397170, 935566, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 661227, 2411653, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 1548845, 2411653, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 2437166, 2411653, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 3324785, 2411653, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 4213105, 2411653, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 5100724, 2411653, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 5989045, 2411653, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.Rectangle, 661227, 2528599, 6397170, 935566, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 661227, 4248765, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 1548845, 4248765, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 2437166, 4248765, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 3324785, 4248765, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 4213105, 4248765, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 5100724, 4248765, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 5989045, 4248765, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(AutoShapeType.Chevron, 661227, 4248765, 1477727, 1169458, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(smartArt._nodeCollection[2].ChildNodes[0], AutoShapeType.Rectangle, 661227, 4365711, 6397170, 935566, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 661227, 442, 6315075, 574097, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 661227, 1837555, 6315075, 574097, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalAccentList(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 661227, 3674668, 6315075, 574097, (string) null, smartArt);
  }

  private static void SetDefaultPropertiesForVerticalAccentList(
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string colorString,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    if (autoShapeType == AutoShapeType.Chevron)
      smartArtShape.GetGuideList().Add("adj", "val 70610");
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, colorString);
    smartArt.DataModel.SmartArtShapeCollection.Add(Guid.NewGuid(), smartArtShape);
  }

  private static void SetDefaultPropertiesForVerticalAccentList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string colorString,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (colorString == null && autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties, colorString);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForVerticalChevronList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForVerticalChevronList(smartArt._nodeCollection[0], AutoShapeType.Chevron, -289718, 292805, 1931458, 1352020, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalChevronList(smartArt._nodeCollection[0], AutoShapeType.RoundSameSideCornerRectangle, 4112286, -2757179, 1255447, 6775979, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalChevronList(smartArt._nodeCollection[1], AutoShapeType.Chevron, -289718, 2033323, 1931458, 1352020, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalChevronList(smartArt._nodeCollection[1], AutoShapeType.RoundSameSideCornerRectangle, 4112286, -1016661, 1255447, 6775979, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalChevronList(smartArt._nodeCollection[2], AutoShapeType.Chevron, -289718, 3773840, 1931458, 1352020, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalChevronList(smartArt._nodeCollection[2], AutoShapeType.RoundSameSideCornerRectangle, 4112286, 723856, 1255447, 6775979, "lt1", smartArt);
  }

  private static void SetDefaultPropertiesForVerticalChevronList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string colorString,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool isColorSet = false;
    Guid key = Guid.NewGuid();
    if (smartArtNode != null)
    {
      SmartArtNode smartArtNode1 = (SmartArtNode) smartArtNode;
      if (autoShapeType == AutoShapeType.RoundSameSideCornerRectangle)
      {
        SmartArtNode childNode1 = (SmartArtNode) smartArtNode1.ChildNodes[0];
        key = childNode1.Id;
        if (childNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
        SmartArtNode childNode2 = (SmartArtNode) smartArtNode1.ChildNodes[1];
        if (childNode2.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[1] as Paragraph).NodeId = childNode2.Id;
      }
      else
      {
        key = smartArtNode1.Id;
        if (smartArtNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
        isColorSet = true;
      }
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 5400000, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, colorString);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, isColorSet, 3800);
    smartArt.DataModel.SmartArtShapeCollection.Add(key, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForVerticalBlockList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForVerticalBlockList(smartArt._nodeCollection[0], AutoShapeType.RoundSameSideCornerRectangle, 4828539, -1725189, 1397000, 5201920, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBlockList(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 0, 2645, 2926080, 1746250, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBlockList(smartArt._nodeCollection[1], AutoShapeType.RoundSameSideCornerRectangle, 4828539, 108373, 1397000, 5201920, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBlockList(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 0, 1836208, 2926080, 1746250, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBlockList(smartArt._nodeCollection[2], AutoShapeType.RoundSameSideCornerRectangle, 4828539, 1941936, 1397000, 5201920, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBlockList(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 0, 3669771, 2926080, 1746250, smartArt);
  }

  private static void SetDefaultPropertiesForVerticalBlockList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    Guid key = Guid.NewGuid();
    bool isColorSet = false;
    if (smartArtNode != null)
    {
      SmartArtNode smartArtNode1 = (SmartArtNode) smartArtNode;
      if (autoShapeType == AutoShapeType.RoundSameSideCornerRectangle)
      {
        SmartArtNode childNode1 = (SmartArtNode) smartArtNode1.ChildNodes[0];
        key = childNode1.Id;
        if (childNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
        SmartArtNode childNode2 = (SmartArtNode) smartArtNode1.ChildNodes[1];
        if (childNode2.TextBody.Paragraphs.Count > 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs[1]);
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs.Add());
        (smartArtShape.TextBody.Paragraphs[1] as Paragraph).NodeId = childNode2.Id;
      }
      else
      {
        key = smartArtNode1.Id;
        if (smartArtNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
        isColorSet = true;
      }
    }
    int rotation = 0;
    if (autoShapeType == AutoShapeType.RoundSameSideCornerRectangle)
      rotation = 5400000;
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, "accent1");
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, isColorSet, 6500);
    smartArt.DataModel.SmartArtShapeCollection.Add(key, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForTitledPictureAccentList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForTitledPictureAccentList(smartArt.Nodes[0], 0, 474133, 8128000, 1354666, smartArt);
    SmartArt.SetDefaultPropertiesForTitledPictureAccentList((ISmartArtNode) null, 0, 2072640, 1354666, 1354666, smartArt);
    SmartArt.SetDefaultPropertiesForTitledPictureAccentList(smartArt.Nodes[0].ChildNodes[0], 1435946, 2072640, 6692053, 1354666, smartArt);
    SmartArt.SetDefaultPropertiesForTitledPictureAccentList((ISmartArtNode) null, 0, 3589866, 1354666, 1354666, smartArt);
    SmartArt.SetDefaultPropertiesForTitledPictureAccentList(smartArt.Nodes[0].ChildNodes[1], 1435946, 3589866, 6692053, 1354666, smartArt);
  }

  private static void SetDefaultPropertiesForTitledPictureAccentList(
    ISmartArtNode smartArtNode,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    int rotation = 0;
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, AutoShapeType.RoundedRectangle, false, true, true, "accent1");
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (smartArtNode != null)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForVerticalPictureAccentList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForVerticalPictureAccentList(smartArt._nodeCollection[0], AutoShapeType.Pentagon, 1737697, 2526, 5405120, 1505029, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalPictureAccentList((ISmartArtNode) null, AutoShapeType.Oval, 985182, 2526, 1505029, 1505029, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalPictureAccentList(smartArt._nodeCollection[1], AutoShapeType.Pentagon, 1737697, 1956818, 5405120, 1505029, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalPictureAccentList((ISmartArtNode) null, AutoShapeType.Oval, 985182, 1956818, 1505029, 1505029, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalPictureAccentList(smartArt._nodeCollection[2], AutoShapeType.Pentagon, 1737697, 3911110, 5405120, 1505029, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalPictureAccentList((ISmartArtNode) null, AutoShapeType.Oval, 985182, 3911110, 1505029, 1505029, smartArt);
  }

  private static void SetDefaultPropertiesForVerticalPictureAccentList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    int rotation = 0;
    if (autoShapeType == AutoShapeType.Pentagon)
      rotation = 10800000;
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, "accent1");
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (smartArtNode != null)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForAlternatingPictureBlocks(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForAlternatingPictureBlocks(smartArt._nodeCollection[0], 3151822, 2068, 3595052, 1625984, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingPictureBlocks((ISmartArtNode) null, 1381124, 2068, 1609725, 1625984, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingPictureBlocks(smartArt._nodeCollection[1], 1381124, 1896341, 3595052, 1625984, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingPictureBlocks((ISmartArtNode) null, 5137150, 1896341, 1609725, 1625984, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingPictureBlocks(smartArt._nodeCollection[2], 3151822, 3790613, 3595052, 1625984, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingPictureBlocks((ISmartArtNode) null, 1381124, 3790613, 1609725, 1625984, smartArt);
  }

  private static void SetDefaultPropertiesForAlternatingPictureBlocks(
    ISmartArtNode smartArtNode,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    smartArtShape.GetGuideList().Add("adj", "val 10000");
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, AutoShapeType.Rectangle, false, true, true, "accent1");
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (smartArtNode != null)
      SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, true, 6500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForVerticalPictureList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForVerticalPictureList(smartArt._nodeCollection[0], 0, 0, 8128000, 1693333, "ParentShape", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalPictureList((ISmartArtNode) null, 169333, 169333, 1625600, 1354666, "PictureShape", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalPictureList(smartArt._nodeCollection[1], 0, 1862666, 8128000, 1693333, "ParentShape", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalPictureList((ISmartArtNode) null, 169333, 2032000, 1625600, 1354666, "PictureShape", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalPictureList(smartArt._nodeCollection[2], 0, 3725333, 8128000, 1693333, "ParentShape", smartArt);
    SmartArt.SetDefaultPropertiesForVerticalPictureList((ISmartArtNode) null, 169333, 3894666, 1625600, 1354666, "PictureShape", smartArt);
  }

  private static void SetDefaultPropertiesForVerticalPictureList(
    ISmartArtNode smartArtNode,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string shapeName,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    Guid key = Guid.NewGuid();
    bool isColorSet = false;
    if (smartArtNode != null)
    {
      SmartArtNode smartArtNode1 = (SmartArtNode) smartArtNode;
      key = smartArtNode1.Id;
      if (smartArtNode1.TextBody.Paragraphs.Count == 0)
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs.Add());
      else
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs[0]);
      (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
      SmartArtNode childNode1 = (SmartArtNode) smartArtNode1.ChildNodes[0];
      if (childNode1.TextBody.Paragraphs.Count > 0)
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs[1]);
      else
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs.Add());
      (smartArtShape.TextBody.Paragraphs[1] as Paragraph).NodeId = childNode1.Id;
      SmartArtNode childNode2 = (SmartArtNode) smartArtNode1.ChildNodes[1];
      if (childNode2.TextBody.Paragraphs.Count > 1)
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs[2]);
      else
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs.Add());
      (smartArtShape.TextBody.Paragraphs[2] as Paragraph).NodeId = childNode2.Id;
      isColorSet = true;
    }
    smartArtShape.GetGuideList().Add("adj", "val 10000");
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, AutoShapeType.RoundedRectangle, false, true, true, "accent1", shapeName);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Left, isColorSet, 3300);
    smartArt.DataModel.SmartArtShapeCollection.Add(key, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForPictureStrips(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForPictureStrips(smartArt._nodeCollection[0], 1924367, 355737, 4465319, 1395412, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForPictureStrips((ISmartArtNode) null, 1738312, 154178, 976788, 1465183, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForPictureStrips(smartArt._nodeCollection[1], 1924367, 2112407, 4465319, 1395412, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForPictureStrips((ISmartArtNode) null, 1738312, 1910847, 976788, 1465183, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForPictureStrips(smartArt._nodeCollection[2], 1924367, 3869076, 4465319, 1395412, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForPictureStrips((ISmartArtNode) null, 1738312, 3667516, 976788, 1465183, "accent1", smartArt);
  }

  private static void SetDefaultPropertiesForPictureStrips(
    ISmartArtNode smartArtNode,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string colorString,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, AutoShapeType.Rectangle, false, true, true, colorString);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForContinuousPictureList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForContinuousPictureList(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 1706, 0, 2655093, 5418667, smartArt);
    SmartArt.SetDefaultPropertiesForContinuousPictureList((ISmartArtNode) null, AutoShapeType.Oval, 427045, 325120, 1804416, 1804416, smartArt);
    SmartArt.SetDefaultPropertiesForContinuousPictureList(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 2736453, 0, 2655093, 5418667, smartArt);
    SmartArt.SetDefaultPropertiesForContinuousPictureList((ISmartArtNode) null, AutoShapeType.Oval, 3161791, 325120, 1804416, 1804416, smartArt);
    SmartArt.SetDefaultPropertiesForContinuousPictureList(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 5471199, 0, 2655093, 5418667, smartArt);
    SmartArt.SetDefaultPropertiesForContinuousPictureList((ISmartArtNode) null, AutoShapeType.Oval, 5896538, 325120, 1804416, 1804416, smartArt);
    SmartArt.SetDefaultPropertiesForContinuousPictureList((ISmartArtNode) null, AutoShapeType.LeftRightArrow, 325119, 4334933, 7477760, 812800, smartArt);
  }

  private static void SetDefaultPropertiesForContinuousPictureList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool isColorSet = false;
    if (autoShapeType == AutoShapeType.RoundedRectangle)
    {
      smartArtShape.GetGuideList().Add("adj", "val 10000");
      isColorSet = true;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, false, true, "accent1");
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, isColorSet, 6100);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForHorizontalPictureList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForHorizontalPictureList((ISmartArtNode) null, AutoShapeType.RoundedRectangle, 0, 0, 8128000, 2438400, true, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalPictureList((ISmartArtNode) null, AutoShapeType.RoundedRectangle, 243839, 325120, 2387600, 1788160, false, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalPictureList(smartArt._nodeCollection[0], AutoShapeType.RoundSameSideCornerRectangle, 243839, 2438400, 2387600, 2980266, false, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalPictureList((ISmartArtNode) null, AutoShapeType.RoundedRectangle, 2870200, 325120, 2387600, 1788160, false, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalPictureList(smartArt._nodeCollection[1], AutoShapeType.RoundSameSideCornerRectangle, 2870200, 2438400, 2387600, 2980266, false, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalPictureList((ISmartArtNode) null, AutoShapeType.RoundedRectangle, 5496559, 325120, 2387600, 1788160, false, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalPictureList(smartArt._nodeCollection[2], AutoShapeType.RoundSameSideCornerRectangle, 5496559, 2438400, 2387600, 2980266, false, smartArt);
  }

  private static void SetDefaultPropertiesForHorizontalPictureList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool isFirstShape,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    string colorString = "accent1";
    bool hasLineProperties = false;
    int rotation = 0;
    bool isColorSet = false;
    if (!isFirstShape && autoShapeType == AutoShapeType.RoundedRectangle)
      hasLineProperties = true;
    else if (autoShapeType == AutoShapeType.RoundSameSideCornerRectangle)
    {
      hasLineProperties = true;
      isColorSet = true;
      rotation = 10800000;
      smartArtShape.GetGuideList().Add("adj1", "val 10500");
      smartArtShape.GetGuideList().Add("adj2", "val 0");
    }
    if (isFirstShape || autoShapeType == AutoShapeType.RoundedRectangle)
      smartArtShape.GetGuideList().Add("adj", "val 10000");
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, true, colorString, isFirstShape);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, isColorSet, 5200);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForGroupedList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForGroupedList(smartArt._nodeCollection[0], 992, 0, 2579687, 5418667, false, smartArt);
    SmartArt.SetDefaultPropertiesForGroupedList(smartArt._nodeCollection[0].ChildNodes[0], 258960, 1627187, 2063749, 1633802, true, smartArt);
    SmartArt.SetDefaultPropertiesForGroupedList(smartArt._nodeCollection[0].ChildNodes[1], 258960, 3512343, 2063749, 1633802, true, smartArt);
    SmartArt.SetDefaultPropertiesForGroupedList(smartArt._nodeCollection[1], 2774156, 0, 2579687, 5418667, false, smartArt);
    SmartArt.SetDefaultPropertiesForGroupedList(smartArt._nodeCollection[1].ChildNodes[0], 3032125, 1627187, 2063749, 1633802, true, smartArt);
    SmartArt.SetDefaultPropertiesForGroupedList(smartArt._nodeCollection[1].ChildNodes[1], 3032125, 3512343, 2063749, 1633802, true, smartArt);
    SmartArt.SetDefaultPropertiesForGroupedList(smartArt._nodeCollection[2], 5547320, 0, 2579687, 5418667, false, smartArt);
    SmartArt.SetDefaultPropertiesForGroupedList(smartArt._nodeCollection[2].ChildNodes[0], 5805289, 1627187, 2063749, 1633802, true, smartArt);
    SmartArt.SetDefaultPropertiesForGroupedList(smartArt._nodeCollection[2].ChildNodes[1], 5805289, 3512343, 2063749, 1633802, true, smartArt);
  }

  private static void SetDefaultPropertiesForGroupedList(
    ISmartArtNode smartArtNode,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    bool hasLineProperties,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool isColorSet = false;
    if (smartArtNode.Parent is ISmartArtNode)
      isColorSet = true;
    string colorString = "accent1";
    smartArtShape.GetGuideList().Add("adj", "val 10000");
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, AutoShapeType.RoundedRectangle, false, hasLineProperties, true, colorString);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, isColorSet, 5800);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForDetailedProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForDetailedProcess(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 615, 1121039, 2647156, 3176587, smartArt);
    SmartArt.SetDefaultPropertiesForDetailedProcess(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 530046, 1121039, 1972131, 3176587, smartArt);
    SmartArt.SetDefaultPropertiesForDetailedProcess(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 2740421, 1121039, 2647156, 3176587, smartArt);
    SmartArt.SetDefaultPropertiesForDetailedProcess(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.Rectangle, 3269853, 1121039, 1972131, 3176587, smartArt);
    SmartArt.SetDefaultPropertiesForDetailedProcess((ISmartArtNode) null, AutoShapeType.FlowChartExtract, 2520299, 3645011, 466715, 397073, smartArt);
    SmartArt.SetDefaultPropertiesForDetailedProcess(smartArt._nodeCollection[2], AutoShapeType.RoundedRectangle, 5480228, 1121039, 2647156, 3176587, smartArt);
    SmartArt.SetDefaultPropertiesForDetailedProcess(smartArt._nodeCollection[2].ChildNodes[0], AutoShapeType.Rectangle, 6009659, 1121039, 1972131, 3176587, smartArt);
    SmartArt.SetDefaultPropertiesForDetailedProcess((ISmartArtNode) null, AutoShapeType.FlowChartExtract, 5260106, 3645011, 466715, 397073, smartArt);
  }

  private static void SetDefaultPropertiesForDetailedProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = false;
    if (autoShapeType == AutoShapeType.Rectangle)
      hasFillProperties = false;
    string colorString = "accent1";
    int rotation = 0;
    bool isColorSet = true;
    if (autoShapeType == AutoShapeType.FlowChartExtract)
    {
      colorString = "lt1";
      hasLineProperties = true;
      rotation = 5400000;
      isColorSet = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties, colorString);
    if (smartArtShape.AutoShapeType == AutoShapeType.RoundedRectangle)
      smartArtShape.GetGuideList().Add("adj", "val 5000");
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Left, isColorSet, 6500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForPieProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForPieProcess((ISmartArtNode) null, AutoShapeType.Chord, 3353, 822854, 943239, 943239, "chord", smartArt);
    SmartArt.SetDefaultPropertiesForPieProcess((ISmartArtNode) null, AutoShapeType.Pie, 97677, 917178, 754591, 754591, "pie1", smartArt);
    SmartArt.SetDefaultPropertiesForPieProcess((ISmartArtNode) null, AutoShapeType.Chord, 2790626, 822854, 943239, 943239, "chord", smartArt);
    SmartArt.SetDefaultPropertiesForPieProcess((ISmartArtNode) null, AutoShapeType.Pie, 2884950, 917178, 754591, 754591, "pie2", smartArt);
    SmartArt.SetDefaultPropertiesForPieProcess((ISmartArtNode) null, AutoShapeType.Chord, 5577899, 822854, 943239, 943239, "chord", smartArt);
    SmartArt.SetDefaultPropertiesForPieProcess((ISmartArtNode) null, AutoShapeType.Pie, 5672223, 917178, 754591, 754591, "pie3", smartArt);
    SmartArt.SetDefaultPropertiesForPieProcess(smartArt._nodeCollection[0], AutoShapeType.Rectangle, -1081372, 2945143, 2735394, 565943, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForPieProcess(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 663621, 822854, 1886479, 3772958, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForPieProcess(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 1705900, 2945143, 2735394, 565943, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForPieProcess(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.Rectangle, 3450894, 822854, 1886479, 3772958, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForPieProcess(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 4493174, 2945143, 2735394, 565943, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForPieProcess(smartArt._nodeCollection[2].ChildNodes[0], AutoShapeType.Rectangle, 6238167, 822854, 1886479, 3772958, (string) null, smartArt);
  }

  private static void SetDefaultPropertiesForPieProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string shapeName,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
      hasFillProperties = false;
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, false, hasFillProperties, "accent1");
    switch (shapeName)
    {
      case "chord":
        smartArtShape.GetGuideList().Add("adj1", "val 4800000");
        smartArtShape.GetGuideList().Add("adj2", "val 16800000");
        break;
      case "pie1":
        smartArtShape.GetGuideList().Add("adj1", "val 12600000");
        smartArtShape.GetGuideList().Add("adj2", "val 16200000");
        break;
      case "pie2":
        smartArtShape.GetGuideList().Add("adj1", "val 9000000");
        smartArtShape.GetGuideList().Add("adj2", "val 16200000");
        break;
      case "pie3":
        smartArtShape.GetGuideList().Add("adj1", "val 5400000");
        smartArtShape.GetGuideList().Add("adj2", "val 16200000");
        break;
    }
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForIncreasingCircleProcess(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForIncreasingCircleProcess((ISmartArtNode) null, AutoShapeType.Oval, 2328, 0, 628904, 628904, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForIncreasingCircleProcess((ISmartArtNode) null, AutoShapeType.Chord, 65218, 62890, 503123, 503123, "chord1", smartArt);
    SmartArt.SetDefaultPropertiesForIncreasingCircleProcess((ISmartArtNode) null, AutoShapeType.Oval, 2753783, 0, 628904, 628904, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForIncreasingCircleProcess((ISmartArtNode) null, AutoShapeType.Chord, 2816673, 62890, 503123, 503123, "chord2", smartArt);
    SmartArt.SetDefaultPropertiesForIncreasingCircleProcess((ISmartArtNode) null, AutoShapeType.Oval, 5505238, 0, 628904, 628904, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForIncreasingCircleProcess((ISmartArtNode) null, AutoShapeType.Chord, 5568128, 62890, 503123, 503123, "chord3", smartArt);
    SmartArt.SetDefaultPropertiesForIncreasingCircleProcess(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 762253, 628904, 1860507, 2646637, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForIncreasingCircleProcess(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 762253, 0, 1860507, 628904, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForIncreasingCircleProcess(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.Rectangle, 3513708, 628904, 1860507, 2646637, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForIncreasingCircleProcess(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 3513708, 0, 1860507, 628904, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForIncreasingCircleProcess(smartArt._nodeCollection[2].ChildNodes[0], AutoShapeType.Rectangle, 6265164, 628904, 1860507, 2646637, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForIncreasingCircleProcess(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 6265164, 0, 1860507, 628904, (string) null, smartArt);
  }

  private static void SetDefaultPropertiesForIncreasingCircleProcess(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string shapeName,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
      hasFillProperties = false;
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasFillProperties, "accent1", shapeName);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForStackedList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForStackedList(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 1355724, 1354349, 2539007, 1693518, smartArt);
    SmartArt.SetDefaultPropertiesForStackedList(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.Rectangle, 1355724, 3047867, 2539007, 1693518, smartArt);
    SmartArt.SetDefaultPropertiesForStackedList(smartArt._nodeCollection[0], AutoShapeType.Oval, 1587, 677280, 1692671, 1692671, smartArt);
    SmartArt.SetDefaultPropertiesForStackedList(smartArt._nodeCollection[1].ChildNodes[0], AutoShapeType.Rectangle, 5587404, 1354349, 2539007, 1693518, smartArt);
    SmartArt.SetDefaultPropertiesForStackedList(smartArt._nodeCollection[1].ChildNodes[1], AutoShapeType.Rectangle, 5587404, 3047867, 2539007, 1693518, smartArt);
    SmartArt.SetDefaultPropertiesForStackedList(smartArt._nodeCollection[1], AutoShapeType.Oval, 4233267, 677280, 1692671, 1692671, smartArt);
  }

  private static void SetDefaultPropertiesForStackedList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool isColorSet = false;
    if (smartArtNode.Parent is ISmartArt)
      isColorSet = true;
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, "accent1");
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Center, isColorSet, 4100);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBendingPictureAccentList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBendingPictureAccentList((ISmartArtNode) null, AutoShapeType.RoundSameSideCornerRectangle, 5493, 1348022, 2372728, 1771191, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureAccentList(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 5493, 3119214, 2372728, 761612, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureAccentList((ISmartArtNode) null, AutoShapeType.Oval, 1743549, 3240189, 830454, 830454, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureAccentList((ISmartArtNode) null, AutoShapeType.RoundSameSideCornerRectangle, 2779744, 1348022, 2372728, 1771191, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureAccentList(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 2779744, 3119214, 2372728, 761612, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureAccentList((ISmartArtNode) null, AutoShapeType.Oval, 4517800, 3240189, 830454, 830454, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureAccentList((ISmartArtNode) null, AutoShapeType.RoundSameSideCornerRectangle, 5553995, 1348022, 2372728, 1771191, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureAccentList(smartArt._nodeCollection[2], AutoShapeType.Rectangle, 5553995, 3119214, 2372728, 761612, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForBendingPictureAccentList((ISmartArtNode) null, AutoShapeType.Oval, 7292051, 3240189, 830454, 830454, "accent1", smartArt);
  }

  private static void SetDefaultPropertiesForBendingPictureAccentList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string colorString,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true, colorString);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Left, true, 4900);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForPictureAccentList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForPictureAccentList(smartArt._nodeCollection[0], 441623, true, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentList((ISmartArtNode) null, 49345, false, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentList(smartArt._nodeCollection[1], 3283159, true, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentList((ISmartArtNode) null, 2890881, false, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentList(smartArt._nodeCollection[2], 6124695, true, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentList((ISmartArtNode) null, 5732418, false, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentList(smartArt._nodeCollection[0], -1867795, 2772097, 4226560, 392277, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentList(smartArt._nodeCollection[1], 973740, 2772097, 4226560, 392277, smartArt);
    SmartArt.SetDefaultPropertiesForPictureAccentList(smartArt._nodeCollection[2], 3815276, 2772097, 4226560, 392277, smartArt);
  }

  private static void SetDefaultPropertiesForPictureAccentList(
    ISmartArtNode smartArtNode,
    int offsetX,
    bool isParent,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    Guid key = Guid.NewGuid();
    if (smartArtNode != null)
    {
      SmartArtNode smartArtNode1 = (SmartArtNode) smartArtNode;
      if (isParent)
      {
        SmartArtNode childNode1 = (SmartArtNode) smartArtNode1.ChildNodes[0];
        key = childNode1.Id;
        if (childNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
        SmartArtNode childNode2 = (SmartArtNode) smartArtNode1.ChildNodes[1];
        if (childNode2.TextBody.Paragraphs.Count > 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs[1]);
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs.Add());
        (smartArtShape.TextBody.Paragraphs[1] as Paragraph).NodeId = childNode2.Id;
      }
      else
      {
        key = smartArtNode1.Id;
        if (smartArtNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs.Add());
        else
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
      }
    }
    bool isColorSet = false;
    int offsetCX;
    int offsetCY;
    int offsetY;
    if (isParent)
    {
      offsetCX = 1953958;
      offsetCY = 4226560;
      offsetY = 854956;
    }
    else
    {
      offsetCX = 784555;
      offsetCY = 784555;
      offsetY = 337150;
      isColorSet = true;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, AutoShapeType.Rectangle, false, true, true, isParent);
    SmartArt.InitializeDefaultFontColor(smartArtShape.TextBody, HorizontalAlignment.Left, isColorSet, 3500);
    smartArt.DataModel.SmartArtShapeCollection.Add(key, smartArtShape);
  }

  private static void SetDefaultPropertiesForPictureAccentList(
    ISmartArtNode smartArtNode,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 16200000, offsetX, offsetY, offsetCX, offsetCY, AutoShapeType.Rectangle, false, false, false);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForSquareAccentList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForSquareAccentList(37, 837943, 3964841, 466451, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(37, 1013124, 291271, 291271, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(37, 0, 3964841, 837943, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(37, 1692069, 291264, 291264, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(277576, 1498232, 3687302, 678938, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(37, 2371007, 291264, 291264, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(277576, 2177170, 3687302, 678938, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(37, 3049945, 291264, 291264, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(277576, 2856108, 3687302, 678938, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(4163121, 837943, 3964841, 466451, "accent1", smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(4163121, 1013124, 291271, 291271, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(4163121, 0, 3964841, 837943, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(4163121, 1692069, 291264, 291264, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(4440659, 1498232, 3687302, 678938, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(4163121, 2371007, 291264, 291264, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(4440659, 2177170, 3687302, 678938, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(4163121, 3049945, 291264, 291264, "lt1", smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(4440659, 2856108, 3687302, 678938, (string) null, smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(smartArt._nodeCollection[0], 37, 0, 3964841, 837943, smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(smartArt._nodeCollection[0].ChildNodes[0], 277576, 1498232, 3687302, 678938, smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(smartArt._nodeCollection[0].ChildNodes[1], 277576, 2177170, 3687302, 678938, smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(smartArt._nodeCollection[0].ChildNodes[2], 277576, 2856108, 3687302, 678938, smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(smartArt._nodeCollection[1], 4163121, 0, 3964841, 837943, smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(smartArt._nodeCollection[1].ChildNodes[0], 4440659, 1498232, 3687302, 678938, smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(smartArt._nodeCollection[1].ChildNodes[1], 4440659, 2177170, 3687302, 678938, smartArt);
    SmartArt.SetDefaultPropertiesForSquareAccentList(smartArt._nodeCollection[1].ChildNodes[2], 4440659, 2856108, 3687302, 678938, smartArt);
  }

  private static void SetDefaultPropertiesForSquareAccentList(
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    string colorString,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    if (colorString == null)
      SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, AutoShapeType.Rectangle, false, false, false);
    else
      SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, AutoShapeType.Rectangle, false, true, true, colorString);
    smartArt.DataModel.SmartArtShapeCollection.Add(Guid.NewGuid(), smartArtShape);
  }

  private static void SetDefaultPropertiesForSquareAccentList(
    ISmartArtNode smartArtNode,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, AutoShapeType.Rectangle, false, false, false);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForHorizontalBulletList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForHorizontalBulletList(smartArt._nodeCollection[0], 2540, 1247793, true, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalBulletList(smartArt._nodeCollection[0], 2540, 2238393, false, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalBulletList(smartArt._nodeCollection[1], 2825750, 1247793, true, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalBulletList(smartArt._nodeCollection[1], 2825750, 2238393, false, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalBulletList(smartArt._nodeCollection[2], 5648960, 1247793, true, smartArt);
    SmartArt.SetDefaultPropertiesForHorizontalBulletList(smartArt._nodeCollection[2], 5648960, 2238393, false, smartArt);
  }

  private static void SetDefaultPropertiesForHorizontalBulletList(
    ISmartArtNode smartArtNode,
    int offsetX,
    int offsetY,
    bool isParent,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    Guid.NewGuid();
    SmartArtNode smartArtNode1 = (SmartArtNode) smartArtNode;
    Guid id;
    if (!isParent)
    {
      SmartArtNode childNode1 = (SmartArtNode) smartArtNode1.ChildNodes[0];
      id = childNode1.Id;
      if (childNode1.TextBody.Paragraphs.Count == 0)
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs.Add());
      else
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode1.TextBody.Paragraphs[0]);
      (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = id;
      SmartArtNode childNode2 = (SmartArtNode) smartArtNode1.ChildNodes[1];
      if (childNode2.TextBody.Paragraphs.Count > 0)
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs[1]);
      else
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode2.TextBody.Paragraphs.Add());
      (smartArtShape.TextBody.Paragraphs[1] as Paragraph).NodeId = childNode2.Id;
    }
    else
    {
      id = smartArtNode1.Id;
      if (smartArtNode1.TextBody.Paragraphs.Count == 0)
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs.Add());
      else
        ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs[0]);
      (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = id;
    }
    bool isColorSet = true;
    int offsetCY;
    if (isParent)
    {
      offsetCY = 990600;
    }
    else
    {
      offsetCY = 1932480;
      isColorSet = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, 2476500, offsetCY, AutoShapeType.Rectangle, false, true, true, isParent);
    SmartArt.InitializeDefaultFontColor((ITextBody) (smartArtShape.TextBody as Syncfusion.Presentation.RichText.TextBody), HorizontalAlignment.Center, isColorSet, 4400);
    smartArt.DataModel.SmartArtShapeCollection.Add(id, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForVerticalBoxList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForVerticalBoxList((ISmartArtNode) null, AutoShapeType.Rectangle, 0, 635553, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBoxList(smartArt.Nodes[0], AutoShapeType.RoundedRectangle, 406400, 30393, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBoxList((ISmartArtNode) null, AutoShapeType.Rectangle, 0, 2495313, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBoxList(smartArt.Nodes[1], AutoShapeType.RoundedRectangle, 406400, 1890153, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBoxList((ISmartArtNode) null, AutoShapeType.Rectangle, 0, 4355073, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBoxList(smartArt.Nodes[2], AutoShapeType.RoundedRectangle, 406400, 3749913, smartArt);
  }

  private static void SetDefaultPropertiesForVerticalBoxList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    int offsetCX;
    int offsetCY;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      offsetCX = 8128000;
      offsetCY = 1033200;
    }
    else
    {
      offsetCX = 5689600;
      offsetCY = 1210320;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, true, true);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor((ITextBody) (smartArtShape.TextBody as Syncfusion.Presentation.RichText.TextBody), HorizontalAlignment.Left, true, 4100);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForVerticalBulletList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForVerticalBulletList(smartArt._nodeCollection[0], AutoShapeType.RoundedRectangle, 0, 451413, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBulletList(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 0, 1649493, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBulletList(smartArt._nodeCollection[1], AutoShapeType.RoundedRectangle, 0, 2709333, smartArt);
    SmartArt.SetDefaultPropertiesForVerticalBulletList(smartArt._nodeCollection[1], AutoShapeType.Rectangle, 0, 3907413, smartArt);
  }

  private static void SetDefaultPropertiesForVerticalBulletList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    Guid key = Guid.NewGuid();
    if (smartArtNode != null)
    {
      SmartArtNode smartArtNode1 = (SmartArtNode) smartArtNode;
      if (autoShapeType == AutoShapeType.Rectangle)
      {
        SmartArtNode childNode = (SmartArtNode) smartArtNode1.ChildNodes[0];
        key = childNode.Id;
        if (smartArtShape.TextBody.Paragraphs.Count == 0 && childNode.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode.TextBody.Paragraphs.Add());
        else if (smartArtShape.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(childNode.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
      }
      else
      {
        key = smartArtNode1.Id;
        if (smartArtShape.TextBody.Paragraphs.Count == 0 && smartArtNode1.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs.Add());
        else if (smartArtShape.TextBody.Paragraphs.Count == 0)
          ((Paragraphs) smartArtShape.TextBody.Paragraphs).Add(smartArtNode1.TextBody.Paragraphs[0]);
        (smartArtShape.TextBody.Paragraphs[0] as Paragraph).NodeId = key;
      }
    }
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    bool isColorSet = false;
    int offsetCX;
    int offsetCY;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
      offsetCX = 8128000;
      offsetCY = 1059840;
    }
    else
    {
      isColorSet = true;
      offsetCX = 8128000;
      offsetCY = 1198080;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    SmartArt.InitializeDefaultFontColor((ITextBody) (smartArtShape.TextBody as Syncfusion.Presentation.RichText.TextBody), HorizontalAlignment.Left, isColorSet, 5000);
    smartArt.DataModel.SmartArtShapeCollection.Add(key, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForLinedList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForLinedList((ISmartArtNode) null, AutoShapeType.Line, 0, 0, 8128000, 0, smartArt);
    SmartArt.SetDefaultPropertiesForLinedList(smartArt._nodeCollection[0], AutoShapeType.Rectangle, 0, 0, 1625600, 5418667, smartArt);
    SmartArt.SetDefaultPropertiesForLinedList(smartArt._nodeCollection[0].ChildNodes[0], AutoShapeType.Rectangle, 1747520, 84666, 6380480, 1693333, smartArt);
    SmartArt.SetDefaultPropertiesForLinedList((ISmartArtNode) null, AutoShapeType.Line, 1625599, 1778000, 6502400, 0, smartArt);
    SmartArt.SetDefaultPropertiesForLinedList(smartArt._nodeCollection[0].ChildNodes[1], AutoShapeType.Rectangle, 1747520, 1862666, 6380480, 1693333, smartArt);
    SmartArt.SetDefaultPropertiesForLinedList((ISmartArtNode) null, AutoShapeType.Line, 1625599, 3556000, 6502400, 0, smartArt);
    SmartArt.SetDefaultPropertiesForLinedList(smartArt._nodeCollection[0].ChildNodes[2], AutoShapeType.Rectangle, 1747520, 3640666, 6380480, 1693333, smartArt);
    SmartArt.SetDefaultPropertiesForLinedList((ISmartArtNode) null, AutoShapeType.Line, 1625599, 5334000, 6502400, 0, smartArt);
  }

  private static void SetDefaultPropertiesForLinedList(
    ISmartArtNode smartArtNode,
    AutoShapeType autoShapeType,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasFillProperties = false;
      hasLineProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForPictureCaptionList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForPictureCaptionList((ISmartArtNode) null, 160287, 1204, AutoShapeType.RoundedRectangle, smartArt);
    SmartArt.SetDefaultPropertiesForPictureCaptionList(smartArt._nodeCollection[0], 160287, 1682196, AutoShapeType.Rectangle, smartArt);
    SmartArt.SetDefaultPropertiesForPictureCaptionList((ISmartArtNode) null, 2844121, 1204, AutoShapeType.RoundedRectangle, smartArt);
    SmartArt.SetDefaultPropertiesForPictureCaptionList(smartArt._nodeCollection[1], 2844121, 1682196, AutoShapeType.Rectangle, smartArt);
    SmartArt.SetDefaultPropertiesForPictureCaptionList((ISmartArtNode) null, 5527956, 1204, AutoShapeType.RoundedRectangle, smartArt);
    SmartArt.SetDefaultPropertiesForPictureCaptionList(smartArt._nodeCollection[2], 5527956, 1682196, AutoShapeType.Rectangle, smartArt);
    SmartArt.SetDefaultPropertiesForPictureCaptionList((ISmartArtNode) null, 2844121, 2831321, AutoShapeType.RoundedRectangle, smartArt);
    SmartArt.SetDefaultPropertiesForPictureCaptionList(smartArt._nodeCollection[3], 2844121, 4512313, AutoShapeType.Rectangle, smartArt);
  }

  private static void SetDefaultPropertiesForPictureCaptionList(
    ISmartArtNode smartArtNode,
    int offsetX,
    int offsetY,
    AutoShapeType autoShapeType,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    int offsetCX = 2439756;
    bool hasFillProperties = true;
    bool hasLineProperties = true;
    int offsetCY;
    if (autoShapeType == AutoShapeType.RoundedRectangle)
    {
      offsetCY = 1680991;
    }
    else
    {
      offsetCY = 905149;
      hasLineProperties = false;
      hasFillProperties = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, false, hasLineProperties, hasFillProperties);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      Syncfusion.Presentation.RichText.TextBody textBody = smartArtShape.TextBody as Syncfusion.Presentation.RichText.TextBody;
      SmartArt.SetTextBodyProperties(textBody, TextDirection.Horizontal, true, VerticalAlignment.Middle, false, AutoMarginType.NoAutoFit, 298704, 298704, 298704, 298704);
      Paragraph paragraph = new Paragraph(textBody.Paragraphs as Paragraphs);
      SmartArt.SetParagraphProperties(paragraph, HorizontalAlignment.Center, 90000, 0, 35000, true, 1866900L);
      Font font = new Font(paragraph);
      SmartArt.SetFontProperties(font, 4200);
      paragraph.SetEndParaProps(font);
      ((Paragraphs) textBody.Paragraphs).Add((IParagraph) paragraph);
    }
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForAlternatingHexagons(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForAlternatingHexagons(smartArt._nodeCollection[0], 3506806, 130656, 2008628, 1747506, AutoShapeType.Hexagon, 125730, 1466850L, HorizontalAlignment.Center, 3300, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingHexagons(smartArt._nodeCollection[0].ChildNodes[0], 5437901, 401821, 2241629, 1205177, AutoShapeType.Rectangle, 137160, 1600200L, HorizontalAlignment.Left, 3600, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingHexagons((ISmartArtNode) null, 1619499, 130656, 2008628, 1747506, AutoShapeType.Hexagon, 0, 1600200L, HorizontalAlignment.Center, 3600, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingHexagons(smartArt._nodeCollection[1], 2559537, 1835580, 2008628, 1747506, AutoShapeType.Hexagon, 125730, 1466850L, HorizontalAlignment.Center, 3300, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingHexagons(smartArt._nodeCollection[1].ChildNodes[0], 448468, 2106744, 2169318, 1205177, AutoShapeType.Rectangle, 137160, 1600200L, HorizontalAlignment.Right, 3600, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingHexagons((ISmartArtNode) null, 4446844, 1835580, 2008628, 1747506, AutoShapeType.Hexagon, 0, 1600200L, HorizontalAlignment.Center, 3600, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingHexagons(smartArt._nodeCollection[2], 3506806, 3540503, 2008628, 1747506, AutoShapeType.Hexagon, 125730, 1466850L, HorizontalAlignment.Center, 3300, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingHexagons(smartArt._nodeCollection[2].ChildNodes[0], 5437901, 3811668, 2241629, 1205177, AutoShapeType.Rectangle, 137160, 1600200L, HorizontalAlignment.Left, 3600, smartArt);
    SmartArt.SetDefaultPropertiesForAlternatingHexagons((ISmartArtNode) null, 1619499, 3540503, 2008628, 1747506, AutoShapeType.Hexagon, 0, 1600200L, HorizontalAlignment.Center, 3600, smartArt);
  }

  private static void SetDefaultPropertiesForAlternatingHexagons(
    ISmartArtNode smartArtNode,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    AutoShapeType autoShapeType,
    int textMargin,
    long defaultTabSize,
    HorizontalAlignment horizontalAlignment,
    int fontSize,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    bool hasLineProperties = true;
    bool hasGuideList = true;
    int rotation = 5400000;
    bool hasFillProperties = true;
    bool isColorSet = true;
    if (autoShapeType == AutoShapeType.Rectangle)
    {
      hasLineProperties = false;
      hasFillProperties = false;
      hasGuideList = false;
      rotation = 0;
      isColorSet = false;
    }
    SmartArt.SetShapeProperties(smartArtShape, false, false, rotation, offsetX, offsetY, offsetCX, offsetCY, autoShapeType, hasGuideList, hasLineProperties, hasFillProperties);
    SmartArt.SetTextBodyProperties(smartArtShape.TextBody as Syncfusion.Presentation.RichText.TextBody, TextDirection.Horizontal, true, VerticalAlignment.Middle, false, AutoMarginType.NoAutoFit, textMargin, textMargin, textMargin, textMargin);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor((ITextBody) (smartArtShape.TextBody as Syncfusion.Presentation.RichText.TextBody), HorizontalAlignment.Center, isColorSet, fontSize);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultPropertiesForBasicBlockList(SmartArt smartArt)
  {
    if (!SmartArt._created)
      return;
    SmartArt.SetDefaultPropertiesForBasicBlockListShape(smartArt._nodeCollection[0], 1221978, 2645, smartArt);
    SmartArt.SetDefaultPropertiesForBasicBlockListShape(smartArt._nodeCollection[1], 4199334, 2645, smartArt);
    SmartArt.SetDefaultPropertiesForBasicBlockListShape(smartArt._nodeCollection[2], 1221978, 1897327, smartArt);
    SmartArt.SetDefaultPropertiesForBasicBlockListShape(smartArt._nodeCollection[3], 4199334, 1897327, smartArt);
    SmartArt.SetDefaultPropertiesForBasicBlockListShape(smartArt._nodeCollection[4], 2710656, 3792008, smartArt);
  }

  private static void SetDefaultPropertiesForBasicBlockListShape(
    ISmartArtNode smartArtNode,
    int offsetX,
    int offsetY,
    SmartArt smartArt)
  {
    SmartArtShape smartArtShape = new SmartArtShape(smartArt);
    SmartArt.SetShapeProperties(smartArtShape, false, false, 0, offsetX, offsetY, 2706687, 1624012, AutoShapeType.Rectangle, false, true, true);
    Syncfusion.Presentation.RichText.TextBody textBody = smartArtShape.TextBody as Syncfusion.Presentation.RichText.TextBody;
    SmartArt.SetTextBodyProperties(textBody, TextDirection.Horizontal, true, VerticalAlignment.Middle, false, AutoMarginType.NoAutoFit, 247650, 247650, 247650, 247650);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, smartArtShape);
    SmartArt.InitializeDefaultFontColor((ITextBody) textBody, HorizontalAlignment.Center, true, 6500);
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, smartArtShape);
  }

  private static void InitializeDefaultFontColor(
    ITextBody textBody,
    HorizontalAlignment horizontalAlignment,
    bool isColorSet,
    int fontSize)
  {
    if (textBody.Paragraphs.Count <= 0)
      return;
    Paragraph paragraph = textBody.Paragraphs[0] as Paragraph;
    SmartArt.SetParagraphProperties(paragraph, horizontalAlignment, 90000, 0, 35000, true, 2889250L);
    Font font = new Font(paragraph);
    if (isColorSet)
      font.Color = ColorObject.White;
    paragraph.SetEndParaProps(font);
    (paragraph.TextParts.Count != 0 ? paragraph.TextParts[0] as TextPart : paragraph.TextParts.Add() as TextPart).SetFont(font);
    SmartArt.SetFontProperties(font, fontSize);
  }

  internal static void SetDefaultPropertiesForBasicBlockListShape(
    ISmartArtNode smartArtNode,
    SmartArt smartArt)
  {
    SmartArtShape shape = smartArtNode.Shapes[0] as SmartArtShape;
    SmartArt.SetShapeProperties(shape, false, false, 0, 0, 0, 2706687, 1624012, AutoShapeType.Rectangle, false, true, true);
    Syncfusion.Presentation.RichText.TextBody textBody = shape.TextBody as Syncfusion.Presentation.RichText.TextBody;
    SmartArt.SetTextBodyProperties(textBody, TextDirection.Horizontal, true, VerticalAlignment.Middle, false, AutoMarginType.NoAutoFit, 247650, 247650, 247650, 247650);
    Guid guid = SmartArt.InitializeParagraphAndGenerateGuid(smartArtNode, shape);
    if (textBody.Paragraphs.Count > 0)
    {
      Paragraph paragraph = textBody.Paragraphs[0] as Paragraph;
      SmartArt.SetParagraphProperties(paragraph, HorizontalAlignment.Center, 90000, 0, 35000, true, 2889250L);
      Font font = new Font(paragraph);
      font.Color = ColorObject.White;
      SmartArt.SetFontProperties(font, 6500);
      paragraph.SetEndParaProps(font);
      (paragraph.TextParts.Add() as TextPart).SetFont(font);
    }
    smartArt.DataModel.SmartArtShapeCollection.Add(guid, shape);
  }

  private static void SetShapeProperties(
    SmartArtShape smartArtShape,
    bool flipVertical,
    bool flipHorizontal,
    int rotation,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    AutoShapeType autoShapeType,
    bool hasGuideList,
    bool hasLineProperties,
    bool hasFillProperties)
  {
    SmartArt.SetShapeNameTypeAndId(smartArtShape);
    smartArtShape.ShapeFrame.SetAnchor(new bool?(flipVertical), new bool?(flipHorizontal), rotation, (long) offsetX, (long) offsetY, (long) offsetCX, (long) offsetCY);
    SmartArt.SetShapeAutoShapeType(smartArtShape, autoShapeType);
    if (hasGuideList)
    {
      smartArtShape.GetGuideList().Add("adj", "val 25000");
      smartArtShape.GetGuideList().Add("vf", "val 115470");
    }
    SmartArt.SetShapeFillAndLineProperties(smartArtShape, hasLineProperties, hasFillProperties);
  }

  private static void SetShapeProperties(
    SmartArtShape smartArtShape,
    bool flipVertical,
    bool flipHorizontal,
    int rotation,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    AutoShapeType autoShapeType,
    bool hasGuideList,
    bool hasLineProperties,
    bool hasFillProperties,
    string colorString)
  {
    SmartArt.SetShapeNameTypeAndId(smartArtShape);
    smartArtShape.ShapeFrame.SetAnchor(new bool?(flipVertical), new bool?(flipHorizontal), rotation, (long) offsetX, (long) offsetY, (long) offsetCX, (long) offsetCY);
    if (autoShapeType == ~AutoShapeType.Unknown)
    {
      smartArtShape.IsPresetGeometry = false;
      if (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.HierarchyList)
      {
        smartArtShape.Path2DList = new List<Path2D>();
        Path2D path2D = new Path2D()
        {
          PathElements = {
            2.0,
            1.0,
            0.0,
            0.0,
            3.0,
            1.0,
            0.0,
            1160859.0,
            3.0,
            1.0,
            309562.0,
            1160859.0
          }
        };
      }
    }
    else
      SmartArt.SetShapeAutoShapeType(smartArtShape, autoShapeType);
    if (hasGuideList)
    {
      smartArtShape.GetGuideList().Add("adj", "val 25000");
      smartArtShape.GetGuideList().Add("vf", "val 115470");
    }
    else if (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BendingPictureAccentList && colorString == "lt1")
    {
      smartArtShape.GetGuideList().Add("adj1", "val 8000");
      smartArtShape.GetGuideList().Add("adj2", "val 0");
    }
    SmartArt.SetShapeFillAndLineProperties(smartArtShape, hasLineProperties, hasFillProperties, colorString);
  }

  private static void SetShapeProperties(
    SmartArtShape smartArtShape,
    bool flipVertical,
    bool flipHorizontal,
    int rotation,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    AutoShapeType autoShapeType,
    bool hasGuideList,
    bool hasLineProperties,
    bool hasFillProperties,
    string colorString,
    string shapeName)
  {
    SmartArt.SetShapeNameTypeAndId(smartArtShape);
    smartArtShape.ShapeFrame.SetAnchor(new bool?(flipVertical), new bool?(flipHorizontal), rotation, (long) offsetX, (long) offsetY, (long) offsetCX, (long) offsetCY);
    SmartArt.SetShapeAutoShapeType(smartArtShape, autoShapeType);
    if (hasGuideList)
    {
      smartArtShape.GetGuideList().Add("adj", "val 25000");
      smartArtShape.GetGuideList().Add("vf", "val 115470");
    }
    else if (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BendingPictureAccentList && colorString == "lt1")
    {
      smartArtShape.GetGuideList().Add("adj1", "val 8000");
      smartArtShape.GetGuideList().Add("adj2", "val 0");
    }
    SmartArt.SetShapeFillAndLineProperties(smartArtShape, hasLineProperties, hasFillProperties, colorString, shapeName);
  }

  private static void SetShapeProperties(
    SmartArtShape smartArtShape,
    bool flipVertical,
    bool flipHorizontal,
    int rotation,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    AutoShapeType autoShapeType,
    bool hasGuideList,
    bool hasLineProperties,
    bool hasFillProperties,
    string colorString,
    bool isFirstShape)
  {
    SmartArt.SetShapeNameTypeAndId(smartArtShape);
    smartArtShape.ShapeFrame.SetAnchor(new bool?(flipVertical), new bool?(flipHorizontal), rotation, (long) offsetX, (long) offsetY, (long) offsetCX, (long) offsetCY);
    SmartArt.SetShapeAutoShapeType(smartArtShape, autoShapeType);
    if (hasGuideList)
    {
      smartArtShape.GetGuideList().Add("adj", "val 25000");
      smartArtShape.GetGuideList().Add("vf", "val 115470");
    }
    else if (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BendingPictureAccentList && colorString == "lt1")
    {
      smartArtShape.GetGuideList().Add("adj1", "val 8000");
      smartArtShape.GetGuideList().Add("adj2", "val 0");
    }
    SmartArt.SetShapeFillAndLineProperties(smartArtShape, hasLineProperties, hasFillProperties, colorString, isFirstShape);
  }

  private static void SetShapeProperties(
    SmartArtShape smartArtShape,
    bool flipVertical,
    bool flipHorizontal,
    int rotation,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    AutoShapeType autoShapeType,
    bool hasLineProperties,
    bool hasFillProperties,
    string colorString,
    string shapeName)
  {
    SmartArt.SetShapeNameTypeAndId(smartArtShape);
    smartArtShape.ShapeFrame.SetAnchor(new bool?(flipVertical), new bool?(flipHorizontal), rotation, (long) offsetX, (long) offsetY, (long) offsetCX, (long) offsetCY);
    SmartArt.SetShapeAutoShapeType(smartArtShape, autoShapeType);
    switch (shapeName)
    {
      case "chord1":
        smartArtShape.GetGuideList().Add("adj1", "val 1168272");
        smartArtShape.GetGuideList().Add("adj2", "val 9631728");
        break;
      case "chord2":
        smartArtShape.GetGuideList().Add("adj1", "val 20431728");
        smartArtShape.GetGuideList().Add("adj2", "val 11968272");
        break;
      case "chord3":
        smartArtShape.GetGuideList().Add("adj1", "val 16200000");
        smartArtShape.GetGuideList().Add("adj2", "val 16200000");
        break;
    }
    SmartArt.SetShapeFillAndLineProperties(smartArtShape, hasLineProperties, hasFillProperties, colorString);
  }

  private static void SetShapeProperties(
    SmartArtShape smartArtShape,
    bool flipVertical,
    bool flipHorizontal,
    int rotation,
    int offsetX,
    int offsetY,
    int offsetCX,
    int offsetCY,
    AutoShapeType autoShapeType,
    bool hasGuideList,
    bool hasLineProperties,
    bool hasFillProperties,
    bool isParent)
  {
    SmartArt.SetShapeNameTypeAndId(smartArtShape);
    smartArtShape.ShapeFrame.SetAnchor(new bool?(flipVertical), new bool?(flipHorizontal), rotation, (long) offsetX, (long) offsetY, (long) offsetCX, (long) offsetCY);
    SmartArt.SetShapeAutoShapeType(smartArtShape, autoShapeType);
    if (hasGuideList)
    {
      smartArtShape.GetGuideList().Add("adj", "val 25000");
      smartArtShape.GetGuideList().Add("vf", "val 115470");
    }
    SmartArt.SetShapeFillAndLineProperties(smartArtShape, hasLineProperties, hasFillProperties, isParent);
  }

  private static void SetFontProperties(Font font, int fontSize)
  {
    if (font == null)
      return;
    font.LanguageID = (short) 1033;
    font.SetFontSize(fontSize);
    font.Kerning = new int?(1200);
  }

  private static void SetParagraphProperties(
    Paragraph paragraph,
    HorizontalAlignment horizontalAlignment,
    int lineSpacing,
    int spaceBefore,
    int spaceAfter,
    bool isLastPara,
    long defaultTabSize)
  {
    paragraph.IndentLevelNumber = 0;
    paragraph.SetAlignmentType(horizontalAlignment);
    paragraph.SetDefaultTabSize(2889250L);
    paragraph.LineSpacingType = SizeType.Percentage;
    paragraph.SetLineSpacing(lineSpacing);
    paragraph.SpaceBeforeType = SizeType.Percentage;
    paragraph.SetSpaceBefore(spaceBefore);
    paragraph.SpaceAfterType = SizeType.Percentage;
    paragraph.SetSpaceAfter(spaceAfter);
    paragraph.SetIsLastPara(isLastPara);
  }

  private static void SetTextBodyProperties(
    Syncfusion.Presentation.RichText.TextBody textBody,
    TextDirection textDirection,
    bool wrapText,
    VerticalAlignment verticalAlignment,
    bool anchorCenter,
    AutoMarginType autoMarginType,
    int leftMargin,
    int topMargin,
    int rightMargin,
    int bottomMargin)
  {
    textBody.SetTextDirection(textDirection);
    textBody.WrapText = wrapText;
    textBody.IsAutoMargins = false;
    textBody.SetMargin(leftMargin, topMargin, rightMargin, bottomMargin);
    textBody.NumberOfColumns = 1;
    textBody.SetVerticalAlign(verticalAlignment);
    textBody.AnchorCenter = anchorCenter;
    textBody.AutoFitType = autoMarginType;
  }

  private static void SetShapeFillAndLineProperties(
    SmartArtShape smartArtShape,
    bool hasLineProperties,
    bool hasFillProperties)
  {
    ColorObject colorObject1 = new ColorObject(true);
    string str1 = "lt1";
    string str2 = "accent1";
    if (smartArtShape.AutoShapeType == AutoShapeType.Line || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.SquareAccentList || smartArtShape.AutoShapeType == AutoShapeType.DownArrow && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.StaggeredProcess || (smartArtShape.AutoShapeType == AutoShapeType.Donut || smartArtShape.AutoShapeType == AutoShapeType.Oval) && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.AlternatingPictureCircles)
      str1 = "accent1";
    else if (smartArtShape.AutoShapeType == AutoShapeType.Rectangle && (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.VerticalBoxList || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.IncreasingArrowsProcess))
    {
      str1 = "accent1";
      str2 = "lt1";
    }
    if (hasFillProperties)
    {
      smartArtShape.Fill.FillType = FillType.Solid;
      if (smartArtShape.AutoShapeType == AutoShapeType.Oval && (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.VerticalCircleList || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.PhasedProcess || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.RadialVenn || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BasicVenn || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.LinearVenn))
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, 50000);
      else if (smartArtShape.AutoShapeType == AutoShapeType.RightArrow && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.ProcessArrows || smartArtShape.AutoShapeType == AutoShapeType.DownArrow && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.StaggeredProcess || smartArtShape.AutoShapeType == AutoShapeType.Oval && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.AlternatingPictureCircles)
      {
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, 90000);
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 40000);
      }
      else if ((smartArtShape.AutoShapeType == AutoShapeType.Rectangle || smartArtShape.AutoShapeType == AutoShapeType.Frame) && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.SnapshotPictureList)
      {
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 50000);
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, 40000);
      }
      else if (smartArtShape.AutoShapeType == AutoShapeType.RightArrow && (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BasicProcess || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.VerticalProcess || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BasicBendingProcess || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.DivergingRadial) || smartArtShape.AutoShapeType == AutoShapeType.Chevron && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.RandomToResultProcess || smartArtShape.AutoShapeType == AutoShapeType.Oval && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.DescendingProcess || smartArtShape.AutoShapeType == AutoShapeType.IsoscelesTriangle && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.CircularBendingProcess || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.Equation && (smartArtShape.AutoShapeType == AutoShapeType.MathPlus || smartArtShape.AutoShapeType == AutoShapeType.MathEqual) || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.VerticalEquation && (smartArtShape.AutoShapeType == AutoShapeType.RightArrow || smartArtShape.AutoShapeType == AutoShapeType.MathPlus) || smartArtShape.AutoShapeType == AutoShapeType.CircularArrow && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.Gear || smartArtShape.AutoShapeType == AutoShapeType.RightArrow && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BasicCycle || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.MultiDirectionalCycle && smartArtShape.AutoShapeType == AutoShapeType.LeftRightArrow || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.SegmentedCycle && smartArtShape.AutoShapeType == AutoShapeType.CircularArrow || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.RadialCycle && smartArtShape.AutoShapeType == AutoShapeType.BlockArc || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.CounterBalanceArrows && smartArtShape.AutoShapeType == AutoShapeType.MathMinus || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.ConvergingRadial && smartArtShape.AutoShapeType == AutoShapeType.LeftArrow || smartArtShape.AutoShapeType == AutoShapeType.RoundedRectangle && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.TitledMatrix)
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 60000);
      else if (smartArtShape.AutoShapeType == AutoShapeType.BentUpArrow && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.StepDownProcess || smartArtShape.AutoShapeType == AutoShapeType.RightArrow && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.OpposingIdeas || smartArtShape.AutoShapeType == AutoShapeType.Rectangle && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.PlusAndMinus || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.ReverseList && smartArtShape.AutoShapeType == AutoShapeType.RoundSameSideCornerRectangle || (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.AccentedPicture || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.CircularPictureCallOut) && (smartArtShape.AutoShapeType == AutoShapeType.RoundedRectangle || smartArtShape.AutoShapeType == AutoShapeType.Oval) || smartArtShape.AutoShapeType == AutoShapeType.Rectangle && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.PictureGrid || smartArtShape.AutoShapeType == AutoShapeType.Rectangle && (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.AlternatingPictureCircles || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.FramedTextPicture))
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 50000);
      else if (smartArtShape.AutoShapeType == AutoShapeType.RightArrow && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.ContinuousBlockProcess || smartArtShape.AutoShapeType == AutoShapeType.NotchedRightArrow && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BasicTimeLine || smartArtShape.AutoShapeType == AutoShapeType.SwooshArrow && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.UpwardArrow || smartArtShape.AutoShapeType == AutoShapeType.CircularArrow && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.ContinuousCycle || smartArtShape.AutoShapeType == AutoShapeType.Diamond && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BasicMatrix || smartArtShape.AutoShapeType == AutoShapeType.QuadArrow && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.GridMatrix || smartArtShape.AutoShapeType == AutoShapeType.Rectangle && (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BendingPictureCaption || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BendingPictureSemiTransparentText))
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 40000);
      colorObject1.SetColor(ColorType.Theme, str2);
      ((SolidFill) smartArtShape.Fill.SolidFill).SetColorObject(colorObject1);
    }
    else
      smartArtShape.Fill.FillType = FillType.None;
    if (hasLineProperties)
    {
      smartArtShape.HasLineProperties = hasLineProperties;
      smartArtShape.LineFormat.Fill.FillType = FillType.Solid;
      ColorObject colorObject2 = new ColorObject(true);
      if (smartArtShape.AutoShapeType == AutoShapeType.BlockArc || smartArtShape.AutoShapeType == ~AutoShapeType.Unknown && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.HierarchyList || smartArtShape.AutoShapeType == AutoShapeType.Arc && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.HalfCircleOrganizationChart || smartArtShape.AutoShapeType == AutoShapeType.Line && (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.CircularPictureCallOut || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.PictureLineUp))
      {
        str1 = "accent1";
        colorObject2.ColorTransFormCollection.AddColorTransForm(ColorMode.Shade, 60000);
      }
      else if (smartArtShape.AutoShapeType == AutoShapeType.RightArrow && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.ProcessArrows || smartArtShape.AutoShapeType == AutoShapeType.DownArrow && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.StaggeredProcess || smartArtShape.AutoShapeType == AutoShapeType.Oval && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.AlternatingPictureCircles)
      {
        colorObject2.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, 90000);
        colorObject2.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 40000);
      }
      else if (smartArtShape.AutoShapeType == AutoShapeType.MathPlus && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.PlusAndMinus)
        str1 = "accent1";
      colorObject2.SetColor(ColorType.Theme, str1);
      ((SolidFill) smartArtShape.LineFormat.Fill.SolidFill).SetColorObject(colorObject2);
      ((Syncfusion.Presentation.Drawing.LineFormat) smartArtShape.LineFormat).SetWidth(12700);
      smartArtShape.LineFormat.DashStyle = LineDashStyle.Solid;
      smartArtShape.LineFormat.LineJoinType = LineJoinType.Miter;
    }
    else
      smartArtShape.LineFormat.Fill.FillType = FillType.None;
  }

  private static void SetShapeFillAndLineProperties(
    SmartArtShape smartArtShape,
    bool hasLineProperties,
    bool hasFillProperties,
    string colorString)
  {
    ColorObject colorObject1 = new ColorObject(true);
    string str1 = "lt1";
    string str2 = colorString;
    if (colorString == "lt1" || smartArtShape.AutoShapeType == AutoShapeType.Chevron && (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.VerticalChevronList || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.VerticalAccentList))
      str1 = "accent1";
    if (hasFillProperties)
    {
      if (smartArtShape.AutoShapeType == AutoShapeType.Oval && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BendingPictureAccentList || smartArtShape.AutoShapeType == AutoShapeType.Rectangle && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.StackedList || smartArtShape.AutoShapeType == AutoShapeType.RoundSameSideCornerRectangle && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.VerticalBlockList || smartArtShape.AutoShapeType == AutoShapeType.RightArrow && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.VerticalArrowList)
      {
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, 90000);
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 40000);
      }
      else if (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BendingPictureSemiTransparentText && smartArtShape.AutoShapeType == AutoShapeType.Rectangle)
      {
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 50000);
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, 40000);
      }
      else if (smartArtShape.AutoShapeType == AutoShapeType.Oval && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.IncreasingCircleProcess || smartArtShape.AutoShapeType == AutoShapeType.Chord && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.PieProcess || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.GroupedList && !hasLineProperties || str2 == "lt1" && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.PictureStrips)
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 40000);
      else if (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.ContinuousPictureList && smartArtShape.AutoShapeType == AutoShapeType.Oval || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.PictureStrips && colorString == "accent1" || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.VerticalPictureAccentList && smartArtShape.AutoShapeType == AutoShapeType.Oval || colorString == "accent1" && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.CaptionedPictures && smartArtShape.AutoShapeType == AutoShapeType.Rectangle)
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 50000);
      else if (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.ContinuousPictureList && smartArtShape.AutoShapeType == AutoShapeType.LeftRightArrow || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.AccentProcess && smartArtShape.AutoShapeType == AutoShapeType.RightArrow || (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.AlternatingFlow || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.CycleMatrix) && smartArtShape.AutoShapeType == AutoShapeType.CircularArrow)
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 60000);
      else if (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.VerticalChevronList && smartArtShape.AutoShapeType == AutoShapeType.RoundSameSideCornerRectangle)
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 90000);
      else if (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.PyramidList && smartArtShape.AutoShapeType == AutoShapeType.RoundedRectangle || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.TargetList && smartArtShape.AutoShapeType == AutoShapeType.Rectangle || colorString == "lt1" && (smartArtShape.AutoShapeType == AutoShapeType.RoundedRectangle && (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.TitledPictureBlocks || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.AccentProcess || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.AlternatingFlow || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.CycleMatrix || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.Hierarchy || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.NestedTarget) || smartArtShape.AutoShapeType == AutoShapeType.Rectangle && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.NameAndTitleOrganizationChart || smartArtShape.AutoShapeType == AutoShapeType.Hexagon && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.HexagonCluster && smartArtShape.ShapeFrame.GetDefaultWidth() == 179.84))
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, 90000);
      else if (colorString == "lt1" && smartArtShape.AutoShapeType == AutoShapeType.Rectangle && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.CaptionedPictures)
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, 40000);
      smartArtShape.Fill.FillType = FillType.Solid;
      colorObject1.SetColor(ColorType.Theme, str2);
      ((SolidFill) smartArtShape.Fill.SolidFill).SetColorObject(colorObject1);
    }
    else
      smartArtShape.Fill.FillType = FillType.None;
    if (hasLineProperties)
    {
      int num = 12700;
      smartArtShape.HasLineProperties = hasLineProperties;
      smartArtShape.LineFormat.Fill.FillType = FillType.Solid;
      ColorObject colorObject2 = new ColorObject(true);
      if (smartArtShape.AutoShapeType == AutoShapeType.Oval && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BendingPictureAccentList || smartArtShape.AutoShapeType == AutoShapeType.RightArrow && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.VerticalArrowList)
      {
        colorObject2.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, 90000);
        colorObject2.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 40000);
        str1 = "accent1";
      }
      else if (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.HierarchyList && smartArtShape.AutoShapeType == AutoShapeType.RoundedRectangle && colorString == "lt1")
        colorObject2.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, 90000);
      colorObject2.SetColor(ColorType.Theme, str1);
      ((SolidFill) smartArtShape.LineFormat.Fill.SolidFill).SetColorObject(colorObject2);
      if (str2 == "lt1" && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.PictureStrips)
        num = 6350;
      ((Syncfusion.Presentation.Drawing.LineFormat) smartArtShape.LineFormat).SetWidth(num);
      smartArtShape.LineFormat.DashStyle = LineDashStyle.Solid;
      smartArtShape.LineFormat.LineJoinType = LineJoinType.Miter;
    }
    else
      smartArtShape.LineFormat.Fill.FillType = FillType.None;
  }

  private static void SetShapeFillAndLineProperties(
    SmartArtShape smartArtShape,
    bool hasLineProperties,
    bool hasFillProperties,
    string colorString,
    string shapeName)
  {
    ColorObject colorObject1 = new ColorObject(true);
    string str1 = "lt1";
    string str2 = colorString;
    if (colorString == "lt1")
      str1 = "accent1";
    if (hasFillProperties)
    {
      if (smartArtShape.AutoShapeType == AutoShapeType.Oval && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BendingPictureAccentList || smartArtShape.AutoShapeType == AutoShapeType.Rectangle && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.StackedList)
      {
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, 90000);
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 40000);
      }
      else if (smartArtShape.AutoShapeType == AutoShapeType.Oval && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.IncreasingCircleProcess || smartArtShape.AutoShapeType == AutoShapeType.Chord && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.PieProcess || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.GroupedList && !hasLineProperties || str2 == "lt1" && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.PictureStrips)
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 40000);
      else if (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.ContinuousPictureList && smartArtShape.AutoShapeType == AutoShapeType.Oval || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.PictureStrips && colorString == "accent1" || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.VerticalPictureList && shapeName == "PictureShape")
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 50000);
      else if (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.ContinuousPictureList && smartArtShape.AutoShapeType == AutoShapeType.LeftRightArrow)
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 60000);
      smartArtShape.Fill.FillType = FillType.Solid;
      colorObject1.SetColor(ColorType.Theme, str2);
      ((SolidFill) smartArtShape.Fill.SolidFill).SetColorObject(colorObject1);
    }
    else
      smartArtShape.Fill.FillType = FillType.None;
    if (hasLineProperties)
    {
      int num = 12700;
      smartArtShape.HasLineProperties = hasLineProperties;
      smartArtShape.LineFormat.Fill.FillType = FillType.Solid;
      ColorObject colorObject2 = new ColorObject(true);
      if (smartArtShape.AutoShapeType == AutoShapeType.Oval && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BendingPictureAccentList)
      {
        colorObject2.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, 90000);
        colorObject2.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 40000);
        str1 = "accent1";
      }
      colorObject2.SetColor(ColorType.Theme, str1);
      ((SolidFill) smartArtShape.LineFormat.Fill.SolidFill).SetColorObject(colorObject2);
      if (str2 == "lt1" && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.PictureStrips)
        num = 6350;
      ((Syncfusion.Presentation.Drawing.LineFormat) smartArtShape.LineFormat).SetWidth(num);
      smartArtShape.LineFormat.DashStyle = LineDashStyle.Solid;
      smartArtShape.LineFormat.LineJoinType = LineJoinType.Miter;
    }
    else
      smartArtShape.LineFormat.Fill.FillType = FillType.None;
  }

  private static void SetShapeFillAndLineProperties(
    SmartArtShape smartArtShape,
    bool hasLineProperties,
    bool hasFillProperties,
    string colorString,
    bool isFirstShape)
  {
    ColorObject colorObject1 = new ColorObject(true);
    string str1 = "lt1";
    string str2 = colorString;
    if (colorString == "lt1" || isFirstShape)
      str1 = "accent1";
    if (hasFillProperties)
    {
      if (isFirstShape)
      {
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, 90000);
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 40000);
      }
      else if (smartArtShape.ShapeFrame.Rotation == 0)
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 50000);
      smartArtShape.Fill.FillType = FillType.Solid;
      colorObject1.SetColor(ColorType.Theme, str2);
      ((SolidFill) smartArtShape.Fill.SolidFill).SetColorObject(colorObject1);
    }
    else
      smartArtShape.Fill.FillType = FillType.None;
    if (hasLineProperties)
    {
      smartArtShape.HasLineProperties = hasLineProperties;
      smartArtShape.LineFormat.Fill.FillType = FillType.Solid;
      ColorObject colorObject2 = new ColorObject(true);
      if (isFirstShape)
      {
        colorObject2.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, 90000);
        colorObject2.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 40000);
        str1 = "accent1";
      }
      colorObject2.SetColor(ColorType.Theme, str1);
      ((SolidFill) smartArtShape.LineFormat.Fill.SolidFill).SetColorObject(colorObject2);
      ((Syncfusion.Presentation.Drawing.LineFormat) smartArtShape.LineFormat).SetWidth(12700);
      smartArtShape.LineFormat.DashStyle = LineDashStyle.Solid;
      smartArtShape.LineFormat.LineJoinType = LineJoinType.Miter;
    }
    else
      smartArtShape.LineFormat.Fill.FillType = FillType.None;
  }

  private static void SetShapeFillAndLineProperties(
    SmartArtShape smartArtShape,
    bool hasLineProperties,
    bool hasFillProperties,
    bool isParent)
  {
    ColorObject colorObject1 = new ColorObject(true);
    string str1 = "lt1";
    string str2 = "accent1";
    if (smartArtShape.AutoShapeType == AutoShapeType.Line || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.SegmentedProcess && smartArtShape.AutoShapeType == AutoShapeType.Rectangle || !isParent && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.ChevronList && smartArtShape.AutoShapeType == AutoShapeType.Chevron || smartArtShape.AutoShapeType == AutoShapeType.RoundedRectangle && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.ProcessList || smartArtShape.AutoShapeType == AutoShapeType.Oval && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.AscendingPictureAccentprocess)
      str1 = "accent1";
    else if (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.VerticalBoxList && smartArtShape.AutoShapeType == AutoShapeType.Rectangle)
    {
      str1 = "accent1";
      str2 = "lt1";
    }
    if (hasFillProperties)
    {
      smartArtShape.Fill.FillType = FillType.Solid;
      colorObject1.SetColor(ColorType.Theme, str2);
      if (!isParent && (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.HorizontalBulletList || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.PictureAccentList || (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.SegmentedProcess || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.Balance) && smartArtShape.AutoShapeType == AutoShapeType.Rectangle || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.ChevronList && smartArtShape.AutoShapeType == AutoShapeType.Chevron || (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.ProcessList || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.Balance) && smartArtShape.AutoShapeType == AutoShapeType.RoundedRectangle || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.Balance && smartArtShape.AutoShapeType == AutoShapeType.IsoscelesTriangle))
      {
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, 90000);
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 40000);
      }
      else if (isParent && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.TableList)
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Shade, 80000);
      else if (isParent && smartArtShape.AutoShapeType == AutoShapeType.RoundedRectangle && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.PictureAccentProcess || smartArtShape.AutoShapeType == AutoShapeType.Rectangle && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.SnapshotPictureList || !isParent && (smartArtShape.AutoShapeType == AutoShapeType.Oval && (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.AscendingPictureAccentprocess || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BubblePictureList) || smartArtShape.AutoShapeType == AutoShapeType.Rectangle && (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BendingPictureBlocks || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.BendingPictureCaptionList || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.TitledPictureBlocks)))
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 50000);
      else if (smartArtShape.AutoShapeType == AutoShapeType.RightArrow && (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.PictureAccentProcess || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.ProcessList) || !isParent && smartArtShape.AutoShapeType == AutoShapeType.Rectangle && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.VerticalBendingProcess || smartArtShape.AutoShapeType == AutoShapeType.DownArrow && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.Funnel)
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 60000);
      else if (isParent && smartArtShape.AutoShapeType == AutoShapeType.Oval && smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.Funnel)
      {
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 50000);
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, 40000);
      }
      else if (isParent && smartArtShape.AutoShapeType == AutoShapeType.RoundedRectangle && (smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.LabeledHierarchy || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.HorizontalLabeledHierarchy))
        colorObject1.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 40000);
      ((SolidFill) smartArtShape.Fill.SolidFill).SetColorObject(colorObject1);
    }
    else
      smartArtShape.Fill.FillType = FillType.None;
    if (hasLineProperties)
    {
      smartArtShape.HasLineProperties = hasLineProperties;
      smartArtShape.LineFormat.Fill.FillType = FillType.Solid;
      ColorObject colorObject2 = new ColorObject(true);
      colorObject2.SetColor(ColorType.Theme, str1);
      if (!isParent || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.SegmentedProcess && smartArtShape.AutoShapeType == AutoShapeType.Rectangle || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.ChevronList && smartArtShape.AutoShapeType == AutoShapeType.Chevron || smartArtShape.ParentSmartArt.DataModel.SmartArtType == SmartArtType.ProcessList && smartArtShape.AutoShapeType == AutoShapeType.RoundedRectangle)
      {
        colorObject2.ColorTransFormCollection.AddColorTransForm(ColorMode.Alpha, 90000);
        colorObject2.ColorTransFormCollection.AddColorTransForm(ColorMode.Tint, 40000);
      }
      ((SolidFill) smartArtShape.LineFormat.Fill.SolidFill).SetColorObject(colorObject2);
      ((Syncfusion.Presentation.Drawing.LineFormat) smartArtShape.LineFormat).SetWidth(12700);
      smartArtShape.LineFormat.DashStyle = LineDashStyle.Solid;
      smartArtShape.LineFormat.LineJoinType = LineJoinType.Miter;
    }
    else
      smartArtShape.LineFormat.Fill.FillType = FillType.None;
  }

  private static void SetShapeAutoShapeType(
    SmartArtShape smartArtShape,
    AutoShapeType autoShapeType)
  {
    smartArtShape.AutoShapeType = autoShapeType;
    smartArtShape.IsCustomGeometry = false;
  }

  private static void SetShapeNameTypeAndId(SmartArtShape smartArtShape)
  {
    smartArtShape.SetSlideItemType(SlideItemType.AutoShape);
    smartArtShape.ShapeId = 0;
    smartArtShape.ShapeName = "";
  }
}
