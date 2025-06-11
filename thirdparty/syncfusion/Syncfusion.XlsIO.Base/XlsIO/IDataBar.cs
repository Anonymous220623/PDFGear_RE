// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IDataBar
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IDataBar
{
  IConditionValue MinPoint { get; }

  IConditionValue MaxPoint { get; }

  Color BarColor { get; set; }

  int PercentMax { get; set; }

  int PercentMin { get; set; }

  bool ShowValue { get; set; }

  Color BarAxisColor { get; set; }

  Color BorderColor { get; set; }

  bool HasBorder { get; }

  bool HasGradientFill { get; set; }

  DataBarDirection DataBarDirection { get; set; }

  Color NegativeBorderColor { get; set; }

  Color NegativeFillColor { get; set; }

  DataBarAxisPosition DataBarAxisPosition { get; set; }
}
