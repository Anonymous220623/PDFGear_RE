// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.IScalable
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

internal interface IScalable
{
  bool IsLogScale { get; set; }

  bool IsReversed { get; set; }

  double MaximumValue { get; set; }

  double MinimumValue { get; set; }
}
