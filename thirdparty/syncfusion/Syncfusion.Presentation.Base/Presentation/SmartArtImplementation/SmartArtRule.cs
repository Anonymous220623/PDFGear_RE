// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SmartArtImplementation.SmartArtRule
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.SmartArtImplementation;

internal class SmartArtRule
{
  private SmartArtLayoutNode _smartArtLayoutNode;
  private SmartArtConstraint _constraintAttributes;
  private double _factor;
  private double _maxValue;
  private double _value;

  internal SmartArtRule(SmartArtLayoutNode smartArtLayoutNode)
  {
    this._smartArtLayoutNode = smartArtLayoutNode;
  }

  internal SmartArtConstraint ConstraintAttributes
  {
    get
    {
      return this._constraintAttributes ?? (this._constraintAttributes = new SmartArtConstraint(this._smartArtLayoutNode));
    }
  }

  internal double Factor
  {
    get => this._factor;
    set => this._factor = value;
  }

  internal double MaxValue
  {
    get => this._maxValue;
    set => this._maxValue = value;
  }

  internal double Value
  {
    get => this._value;
    set => this._value = value;
  }
}
