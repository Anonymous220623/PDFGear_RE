// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ConditionValueType
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO;

public enum ConditionValueType
{
  None = -1, // 0xFFFFFFFF
  Number = 1,
  LowestValue = 2,
  HighestValue = 3,
  Percent = 4,
  Percentile = 5,
  Formula = 6,
  Automatic = 7,
}
