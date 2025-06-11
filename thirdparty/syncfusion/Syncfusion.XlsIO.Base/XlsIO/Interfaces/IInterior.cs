// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Interfaces.IInterior
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Interfaces;

public interface IInterior
{
  ExcelKnownColors PatternColorIndex { get; set; }

  Color PatternColor { get; set; }

  ExcelKnownColors ColorIndex { get; set; }

  Color Color { get; set; }

  IGradient Gradient { get; }

  ExcelPattern FillPattern { get; set; }
}
