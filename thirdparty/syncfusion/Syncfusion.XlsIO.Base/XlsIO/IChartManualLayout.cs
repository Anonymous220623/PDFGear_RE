// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IChartManualLayout
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public interface IChartManualLayout : IParentApplication
{
  LayoutTargets LayoutTarget { get; set; }

  LayoutModes LeftMode { get; set; }

  LayoutModes TopMode { get; set; }

  double Left { get; set; }

  double Top { get; set; }

  LayoutModes WidthMode { get; set; }

  LayoutModes HeightMode { get; set; }

  double Width { get; set; }

  double Height { get; set; }
}
