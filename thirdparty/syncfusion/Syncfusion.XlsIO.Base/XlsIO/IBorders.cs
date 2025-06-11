// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IBorders
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IBorders : IEnumerable, IParentApplication
{
  ExcelKnownColors Color { get; set; }

  System.Drawing.Color ColorRGB { get; set; }

  int Count { get; }

  IBorder this[ExcelBordersIndex Index] { get; }

  ExcelLineStyle LineStyle { get; set; }

  ExcelLineStyle Value { get; set; }
}
