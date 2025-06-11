// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.ILayoutInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal interface ILayoutInfo
{
  bool IsClipped { get; set; }

  bool IsSkip { get; set; }

  bool IsSkipBottomAlign { get; set; }

  bool IsLineContainer { get; }

  ChildrenLayoutDirection ChildrenLayoutDirection { get; }

  bool IsLineBreak { get; set; }

  bool TextWrap { get; set; }

  bool IsPageBreakItem { get; set; }

  bool IsVerticalText { get; set; }

  bool IsFirstItemInPage { get; set; }

  bool IsKeepWithNext { get; set; }

  bool IsHiddenRow { get; set; }

  SizeF Size { get; set; }

  SyncFont Font { get; set; }
}
