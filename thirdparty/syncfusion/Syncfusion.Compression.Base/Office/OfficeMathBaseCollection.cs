// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathBaseCollection
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

internal class OfficeMathBaseCollection : 
  CollectionImpl,
  IOfficeMathBaseCollection,
  ICollectionBase,
  IOfficeMathEntity
{
  public IOfficeMathFunctionBase this[int index] => (IOfficeMathFunctionBase) this.InnerList[index];

  internal OfficeMathBaseCollection(OfficeMath owner)
    : base((IOfficeMathEntity) owner)
  {
  }

  internal OfficeMathFunctionBase CreateFunction(MathFunctionType Type)
  {
    switch (Type)
    {
      case MathFunctionType.Accent:
        return (OfficeMathFunctionBase) new OfficeMathAccent(this.OwnerMathEntity);
      case MathFunctionType.Bar:
        return (OfficeMathFunctionBase) new OfficeMathBar(this.OwnerMathEntity);
      case MathFunctionType.BorderBox:
        return (OfficeMathFunctionBase) new OfficeMathBorderBox(this.OwnerMathEntity);
      case MathFunctionType.Box:
        return (OfficeMathFunctionBase) new OfficeMathBox(this.OwnerMathEntity);
      case MathFunctionType.Delimiter:
        return (OfficeMathFunctionBase) new OfficeMathDelimiter(this.OwnerMathEntity);
      case MathFunctionType.EquationArray:
        return (OfficeMathFunctionBase) new OfficeMathEquationArray(this.OwnerMathEntity);
      case MathFunctionType.Fraction:
        return (OfficeMathFunctionBase) new OfficeMathFraction(this.OwnerMathEntity);
      case MathFunctionType.Function:
        return (OfficeMathFunctionBase) new OfficeMathFunction(this.OwnerMathEntity);
      case MathFunctionType.GroupCharacter:
        return (OfficeMathFunctionBase) new OfficeMathGroupCharacter(this.OwnerMathEntity);
      case MathFunctionType.Limit:
        return (OfficeMathFunctionBase) new OfficeMathLimit(this.OwnerMathEntity);
      case MathFunctionType.Matrix:
        return (OfficeMathFunctionBase) new OfficeMathMatrix(this.OwnerMathEntity);
      case MathFunctionType.NArray:
        return (OfficeMathFunctionBase) new OfficeMathNArray(this.OwnerMathEntity);
      case MathFunctionType.Phantom:
        return (OfficeMathFunctionBase) new OfficeMathPhantom(this.OwnerMathEntity);
      case MathFunctionType.Radical:
        return (OfficeMathFunctionBase) new OfficeMathRadical(this.OwnerMathEntity);
      case MathFunctionType.LeftSubSuperscript:
        return (OfficeMathFunctionBase) new OfficeMathLeftScript(this.OwnerMathEntity);
      case MathFunctionType.SubSuperscript:
        return (OfficeMathFunctionBase) new OfficeMathScript(this.OwnerMathEntity);
      case MathFunctionType.RightSubSuperscript:
        return (OfficeMathFunctionBase) new OfficeMathRightScript(this.OwnerMathEntity);
      case MathFunctionType.RunElement:
        return (OfficeMathFunctionBase) new OfficeMathRunElement(this.OwnerMathEntity);
      default:
        return (OfficeMathFunctionBase) null;
    }
  }

  public IOfficeMathFunctionBase Add(int index, MathFunctionType mathFunctionType)
  {
    OfficeMathFunctionBase function = this.CreateFunction(mathFunctionType);
    this.m_innerList.Insert(index, (object) function);
    return (IOfficeMathFunctionBase) function;
  }

  public IOfficeMathFunctionBase Add(MathFunctionType mathFunctionType)
  {
    OfficeMathFunctionBase function = this.CreateFunction(mathFunctionType);
    this.m_innerList.Add((object) function);
    return (IOfficeMathFunctionBase) function;
  }

  internal void CloneItemsTo(OfficeMathBaseCollection items)
  {
    this.CloneItemsTo(items, 0, this.Count - 1);
  }

  internal void CloneItemsTo(OfficeMathBaseCollection items, int startIndex, int endIndex)
  {
    for (int index = startIndex; index <= endIndex; ++index)
    {
      OfficeMathFunctionBase mathFunctionBase = (this[index] as OfficeMathFunctionBase).Clone(items.OwnerMathEntity);
      items.Add((object) mathFunctionBase);
    }
  }

  internal override void Close()
  {
    if (this.m_innerList != null)
    {
      for (int index = 0; index < this.m_innerList.Count; ++index)
        (this.m_innerList[index] as OfficeMathFunctionBase).Close();
    }
    base.Close();
  }
}
