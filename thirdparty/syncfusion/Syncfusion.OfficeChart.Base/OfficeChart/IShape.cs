// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IShape
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface IShape : IParentApplication
{
  int Height { get; set; }

  int Id { get; }

  int Left { get; set; }

  string Name { get; set; }

  int Top { get; set; }

  int Width { get; set; }

  OfficeShapeType ShapeType { get; }

  bool IsShapeVisible { get; set; }

  string AlternativeText { get; set; }

  bool IsMoveWithCell { get; set; }

  bool IsSizeWithCell { get; set; }

  IOfficeFill Fill { get; }

  IShapeLineFormat Line { get; }

  string OnAction { get; set; }

  IShadow Shadow { get; }

  IThreeDFormat ThreeD { get; }

  int ShapeRotation { get; set; }

  void Remove();

  void Scale(int scaleWidth, int scaleHeight);
}
