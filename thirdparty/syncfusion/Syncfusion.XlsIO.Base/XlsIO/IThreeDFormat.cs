// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IThreeDFormat
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public interface IThreeDFormat
{
  Excel2007ChartBevelProperties BevelTop { get; set; }

  Excel2007ChartBevelProperties BevelBottom { get; set; }

  Excel2007ChartMaterialProperties Material { get; set; }

  Excel2007ChartLightingProperties Lighting { get; set; }

  int BevelTopHeight { get; set; }

  int BevelTopWidth { get; set; }

  int BevelBottomHeight { get; set; }

  int BevelBottomWidth { get; set; }
}
