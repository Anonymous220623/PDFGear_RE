// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IShape
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public interface IShape : IParentApplication
{
  int Height { get; set; }

  int Id { get; }

  int Left { get; set; }

  string Name { get; set; }

  int Top { get; set; }

  int Width { get; set; }

  ExcelShapeType ShapeType { get; }

  bool IsShapeVisible { get; set; }

  string AlternativeText { get; set; }

  bool IsMoveWithCell { get; set; }

  bool IsSizeWithCell { get; set; }

  IFill Fill { get; }

  IShapeLineFormat Line { get; }

  string OnAction { get; set; }

  IShadow Shadow { get; }

  IThreeDFormat ThreeD { get; }

  int ShapeRotation { get; set; }

  ITextFrame TextFrame { get; }

  IHyperLink Hyperlink { get; }

  void Remove();

  void Scale(int scaleWidth, int scaleHeight);
}
