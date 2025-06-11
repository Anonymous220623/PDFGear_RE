// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChartFillBorder
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChartFillBorder
{
  bool HasInterior { get; }

  bool HasLineProperties { get; }

  bool Has3dProperties { get; }

  bool HasShadowProperties { get; }

  IChartBorder LineProperties { get; }

  IChartInterior Interior { get; }

  IFill Fill { get; }

  IThreeDFormat ThreeD { get; }

  IShadow Shadow { get; }
}
