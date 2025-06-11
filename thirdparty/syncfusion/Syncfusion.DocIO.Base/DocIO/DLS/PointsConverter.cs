// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.PointsConverter
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.Layouting;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public sealed class PointsConverter
{
  public static float FromCm(float centimeter)
  {
    return (float) UnitsConvertor.Instance.ConvertUnits((double) centimeter, PrintUnits.Centimeter, PrintUnits.Point);
  }

  public static float FromInch(float inch)
  {
    return (float) UnitsConvertor.Instance.ConvertUnits((double) inch, PrintUnits.Inch, PrintUnits.Point);
  }

  public static float FromPixel(float px)
  {
    return (float) UnitsConvertor.Instance.ConvertUnits((double) px, PrintUnits.Pixel, PrintUnits.Point);
  }
}
