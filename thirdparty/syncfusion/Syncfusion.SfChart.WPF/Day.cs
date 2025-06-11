// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.Day
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

[Flags]
public enum Day
{
  Sunday = 1,
  Monday = 2,
  Tuesday = 4,
  Wednesday = 8,
  Thursday = 16, // 0x00000010
  Friday = 32, // 0x00000020
  Saturday = 64, // 0x00000040
}
