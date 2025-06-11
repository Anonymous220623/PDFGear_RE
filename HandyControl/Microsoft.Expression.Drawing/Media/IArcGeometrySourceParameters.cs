// Decompiled with JetBrains decompiler
// Type: HandyControl.Expression.Media.IArcGeometrySourceParameters
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

#nullable disable
namespace HandyControl.Expression.Media;

public interface IArcGeometrySourceParameters : IGeometrySourceParameters
{
  double ArcThickness { get; }

  UnitType ArcThicknessUnit { get; }

  double EndAngle { get; }

  double StartAngle { get; }
}
