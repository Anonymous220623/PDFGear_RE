// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.ILineFormat
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation;

public interface ILineFormat
{
  ArrowheadLength BeginArrowheadLength { get; set; }

  ArrowheadStyle BeginArrowheadStyle { get; set; }

  ArrowheadWidth BeginArrowheadWidth { get; set; }

  LineCapStyle CapStyle { get; set; }

  LineDashStyle DashStyle { get; set; }

  ArrowheadLength EndArrowheadLength { get; set; }

  ArrowheadStyle EndArrowheadStyle { get; set; }

  ArrowheadWidth EndArrowheadWidth { get; set; }

  IFill Fill { get; }

  LineJoinType LineJoinType { get; set; }

  LineStyle Style { get; set; }

  double Weight { get; set; }
}
