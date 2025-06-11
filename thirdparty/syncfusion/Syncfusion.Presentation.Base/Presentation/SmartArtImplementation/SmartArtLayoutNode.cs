// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SmartArtImplementation.SmartArtLayoutNode
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.SmartArtImplementation;

internal class SmartArtLayoutNode
{
  private SmartArtLayout _smartArtLayout;
  private string _name;
  private string _styleLabel;
  private SmartArtChildOrderType _childOrder;
  private string _moveWith;
  private SmartArtAlgorithmType _algorithmType;
  private uint _algorithmRevision;
  private string _shapeType;
  private SmartArtAxisType _axisType;
  private SmartArtPointType _pointType;
  private List<SmartArtConstraint> _constraintList;
  private List<SmartArtRule> _ruleList;
  private SmartArtLayoutNode _forEach;
  private SmartArtLayoutNode _layoutNode;
  private SmartArtLayoutNode _if;
  private SmartArtLayoutNode _else;
  private Dictionary<SmartArtParameterId, object> _parameterList;

  internal SmartArtLayoutNode(SmartArtLayout smartArtLayout)
  {
    this._smartArtLayout = smartArtLayout;
  }

  internal string Name
  {
    get => this._name;
    set => this._name = value;
  }

  internal string StyleLabel
  {
    get => this._styleLabel;
    set => this._styleLabel = value;
  }

  internal SmartArtChildOrderType ChildOrder
  {
    get => this._childOrder;
    set => this._childOrder = value;
  }

  internal string MoveWith
  {
    get => this._moveWith;
    set => this._moveWith = value;
  }

  internal uint AlgorithmRevision
  {
    get => this._algorithmRevision;
    set => this._algorithmRevision = value;
  }

  internal SmartArtAlgorithmType AlgorithmType
  {
    get => this._algorithmType;
    set => this._algorithmType = value;
  }

  internal Dictionary<SmartArtParameterId, object> ParameterList
  {
    get
    {
      return this._parameterList ?? (this._parameterList = new Dictionary<SmartArtParameterId, object>());
    }
  }

  internal string ShapeType
  {
    get => this._shapeType;
    set => this._shapeType = value;
  }

  internal SmartArtAxisType AxisType
  {
    get => this._axisType;
    set => this._axisType = value;
  }

  internal SmartArtPointType PointType
  {
    get => this._pointType;
    set => this._pointType = value;
  }

  internal List<SmartArtConstraint> ConstraintList
  {
    get => this._constraintList;
    set => this._constraintList = value;
  }

  internal List<SmartArtRule> RuleList
  {
    get => this._ruleList;
    set => this._ruleList = value;
  }

  internal SmartArtLayoutNode ForEach
  {
    get => this._forEach ?? (this._forEach = new SmartArtLayoutNode(this._smartArtLayout));
  }

  internal SmartArtLayoutNode LayoutNode
  {
    get => this._layoutNode ?? (this._layoutNode = new SmartArtLayoutNode(this._smartArtLayout));
  }

  internal SmartArtLayoutNode If
  {
    get => this._if ?? (this._if = new SmartArtLayoutNode(this._smartArtLayout));
  }

  internal SmartArtLayoutNode Else
  {
    get => this._else ?? (this._else = new SmartArtLayoutNode(this._smartArtLayout));
  }
}
