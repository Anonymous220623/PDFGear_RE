// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Parser.Biff_Records.IMultiCellRecord
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;

#nullable disable
namespace Syncfusion.OfficeChart.Parser.Biff_Records;

[CLSCompliant(false)]
internal interface IMultiCellRecord : ICellPositionFormat
{
  int FirstColumn { get; set; }

  int LastColumn { get; set; }

  int SubRecordSize { get; }

  int GetSeparateSubRecordSize(OfficeVersion version);

  TBIFFRecord SubRecordType { get; }

  void Insert(ICellPositionFormat cell);

  ICellPositionFormat[] Split(int iColumnIndex);

  BiffRecordRaw[] Split(bool bIgnoreStyles);
}
