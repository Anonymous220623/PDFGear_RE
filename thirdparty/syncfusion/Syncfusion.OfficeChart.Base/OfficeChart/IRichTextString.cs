// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IRichTextString
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

internal interface IRichTextString : IParentApplication, IOptimizedUpdate
{
  IOfficeFont GetFont(int iPosition);

  void SetFont(int iStartPos, int iEndPos, IOfficeFont font);

  void ClearFormatting();

  void Clear();

  void Append(string text, IOfficeFont font);

  string Text { get; set; }

  string RtfText { get; set; }

  bool IsFormatted { get; }
}
