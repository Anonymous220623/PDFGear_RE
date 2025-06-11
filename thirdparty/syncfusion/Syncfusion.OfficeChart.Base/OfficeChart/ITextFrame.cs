// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.ITextFrame
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface ITextFrame
{
  bool IsTextOverFlow { get; set; }

  bool WrapTextInShape { get; set; }

  bool IsAutoSize { get; set; }

  int MarginLeftPt { get; set; }

  int TopMarginPt { get; set; }

  int RightMarginPt { get; set; }

  int BottomMarginPt { get; set; }

  bool IsAutoMargins { get; set; }

  TextVertOverflowType TextVertOverflowType { get; set; }

  TextHorzOverflowType TextHorzOverflowType { get; set; }

  OfficeHorizontalAlignment HorizontalAlignment { get; set; }

  OfficeVerticalAlignment VerticalAlignment { get; set; }

  TextDirection TextDirection { get; set; }

  ITextRange TextRange { get; }
}
