// Decompiled with JetBrains decompiler
// Type: Syncfusion.Layouting.ITextMeasurable
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.Rendering;
using System.Drawing;

#nullable disable
namespace Syncfusion.Layouting;

internal interface ITextMeasurable
{
  SizeF Measure(string text);

  SizeF Measure(DrawingContext dc, string text);
}
