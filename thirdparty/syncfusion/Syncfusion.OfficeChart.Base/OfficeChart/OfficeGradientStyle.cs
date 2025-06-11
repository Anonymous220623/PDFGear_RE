// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.OfficeGradientStyle
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart;

public enum OfficeGradientStyle
{
  Horizontal = 0,
  Vertical = 1,
  DiagonalUp = 2,
  [Obsolete("This enumeration option has been deprecated. You can use DiagonalUp instead of Diagonl_Up.")] Diagonl_Up = 2,
  DiagonalDown = 3,
  [Obsolete("This enumeration option has been deprecated. You can use DiagonalDown instead of Diagonl_Down.")] Diagonl_Down = 3,
  FromCorner = 4,
  [Obsolete("This enumeration option has been deprecated. You can use FromCorner instead of From_Corner.")] From_Corner = 4,
  FromCenter = 5,
  [Obsolete("This enumeration option has been deprecated. You can use FromCenter instead of From_Center.")] From_Center = 5,
}
