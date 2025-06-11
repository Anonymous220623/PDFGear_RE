// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IFillColor
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation;

#nullable disable
namespace Syncfusion.XlsIO;

internal interface IFillColor
{
  ColorObject ForeGroundColorObject { get; }

  ColorObject BackGroundColorObject { get; }

  ExcelPattern Pattern { get; set; }

  bool IsAutomaticFormat { get; set; }

  IFill Fill { get; }

  bool Visible { get; set; }
}
