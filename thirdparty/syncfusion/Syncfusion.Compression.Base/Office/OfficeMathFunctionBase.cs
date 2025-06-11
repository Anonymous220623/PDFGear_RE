// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.OfficeMathFunctionBase
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

internal abstract class OfficeMathFunctionBase : 
  OwnerHolder,
  IOfficeMathFunctionBase,
  IOfficeMathEntity
{
  internal MathFunctionType m_type;

  public MathFunctionType Type => this.m_type;

  internal OfficeMathFunctionBase(IOfficeMathEntity owner)
    : base(owner)
  {
    this.m_type = (MathFunctionType) 0;
  }

  internal override void Close() => base.Close();

  internal abstract OfficeMathFunctionBase Clone(IOfficeMathEntity owner);

  internal IOfficeRunFormat GetDefaultControlProperties()
  {
    return this.GetBaseMathParagraph(this).DefaultMathCharacterFormat.Clone();
  }

  private OfficeMathParagraph GetBaseMathParagraph(OfficeMathFunctionBase mathFunction)
  {
    IOfficeMathEntity ownerMathEntity = mathFunction.OwnerMathEntity;
    while (true)
    {
      switch (ownerMathEntity)
      {
        case IOfficeMathParagraph _:
        case null:
          goto label_3;
        default:
          ownerMathEntity = ownerMathEntity.OwnerMathEntity;
          continue;
      }
    }
label_3:
    return ownerMathEntity as OfficeMathParagraph;
  }
}
