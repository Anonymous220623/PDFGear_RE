// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.IOutline
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
internal interface IOutline
{
  ushort OutlineLevel { get; set; }

  bool IsCollapsed { get; set; }

  bool IsHidden { get; set; }

  ushort ExtendedFormatIndex { get; set; }

  ushort Index { get; set; }
}
