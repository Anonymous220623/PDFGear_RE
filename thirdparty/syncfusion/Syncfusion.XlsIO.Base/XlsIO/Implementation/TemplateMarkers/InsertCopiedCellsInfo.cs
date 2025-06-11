// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.TemplateMarkers.InsertCopiedCellsInfo
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

#nullable disable
namespace Syncfusion.XlsIO.Implementation.TemplateMarkers;

internal struct InsertCopiedCellsInfo
{
  private bool isStyleCopied;
  private int insertedCellsCount;

  internal bool IsStyleCopied => this.isStyleCopied;

  internal int InsertedCellsCount => this.insertedCellsCount;

  internal InsertCopiedCellsInfo(bool isStyleCopied, int cellsCount)
  {
    this.isStyleCopied = isStyleCopied;
    this.insertedCellsCount = cellsCount;
  }
}
