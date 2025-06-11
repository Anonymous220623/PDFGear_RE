// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfDataMatrixSymbolAttribute
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Barcode;

internal struct PdfDataMatrixSymbolAttribute
{
  internal readonly int SymbolRow;
  internal readonly int SymbolColumn;
  internal readonly int HoriDataRegion;
  internal readonly int VertDataRegion;
  internal readonly int DataCodewords;
  internal readonly int CorrectionCodewords;
  internal readonly int InterleavedBlock;
  internal readonly int InterleavedDataBlock;

  internal PdfDataMatrixSymbolAttribute(
    int m_SymbolRow,
    int m_SymbolColumn,
    int m_horiDataRegions,
    int m_vertDataRegions,
    int m_dataCodewords,
    int m_correctionCodewords,
    int m_interleavedBlock,
    int m_interleavedDataBlock)
  {
    this.SymbolRow = m_SymbolRow;
    this.SymbolColumn = m_SymbolColumn;
    this.HoriDataRegion = m_horiDataRegions;
    this.VertDataRegion = m_vertDataRegions;
    this.DataCodewords = m_dataCodewords;
    this.CorrectionCodewords = m_correctionCodewords;
    this.InterleavedBlock = m_interleavedBlock;
    this.InterleavedDataBlock = m_interleavedDataBlock;
  }
}
