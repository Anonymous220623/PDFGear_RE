// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.IOfficeDataRange
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

#nullable disable
namespace Syncfusion.OfficeChart;

public interface IOfficeDataRange
{
  int FirstRow { get; }

  int LastRow { get; }

  int FirstColumn { get; }

  int LastColumn { get; }

  void SetValue(int rowIndex, int columnIndex, int value);

  void SetValue(int rowIndex, int columnIndex, double value);

  void SetValue(int rowIndex, int columnIndex, string value);

  void SetValue(int rowIndex, int columnIndex, object value);

  object GetValue(int rowIndex, int columnIndex);
}
