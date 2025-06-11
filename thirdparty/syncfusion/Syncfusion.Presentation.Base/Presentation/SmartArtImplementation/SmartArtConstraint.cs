// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SmartArtImplementation.SmartArtConstraint
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.SmartArtImplementation;

internal class SmartArtConstraint
{
  private SmartArtLayoutNode _smartArtLayoutNode;
  private SmartArtConstraintType _type;
  private SmartArtConstraintRelationShip _constraintRelationShip;
  private string _forName;
  private SmartArtPointType _pointType;

  internal SmartArtConstraint(SmartArtLayoutNode smartArtLayoutNode)
  {
    this._smartArtLayoutNode = smartArtLayoutNode;
  }

  internal SmartArtConstraintType Type
  {
    get => this._type;
    set => this._type = value;
  }

  internal SmartArtConstraintRelationShip ConstraintRelationShip
  {
    get => this._constraintRelationShip;
    set => this._constraintRelationShip = value;
  }

  internal string ForName
  {
    get => this._forName;
    set => this._forName = value;
  }

  internal SmartArtPointType PointType
  {
    get => this._pointType;
    set => this._pointType = value;
  }
}
