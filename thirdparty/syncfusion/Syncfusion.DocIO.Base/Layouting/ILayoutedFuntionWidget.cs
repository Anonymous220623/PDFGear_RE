// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.ILayoutedFuntionWidget
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Office;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal interface ILayoutedFuntionWidget
{
  IOfficeMathFunctionBase Widget { get; }

  RectangleF Bounds { get; set; }

  LayoutedOMathWidget Owner { get; set; }

  void ShiftXYPosition(float xPosition, float yPosition);

  void Dispose();
}
