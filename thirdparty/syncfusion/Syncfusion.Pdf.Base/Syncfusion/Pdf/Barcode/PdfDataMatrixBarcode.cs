// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfDataMatrixBarcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public class PdfDataMatrixBarcode : PdfBidimensionalBarcode
{
  private const int dpi = 96 /*0x60*/;
  private PdfDataMatrixEncoding dataMatrixEncoding;
  private PdfDataMatrixSize size;
  internal byte[,] dataMatrixArray;
  private PdfDataMatrixSymbolAttribute[] symbolAttributes;
  private PdfDataMatrixSymbolAttribute symbolAttribute;
  private int[] log;
  private int[] aLog;
  private byte[] polynomial;
  private int blockLength;
  private int m_quiteZoneLeft = 1;
  private int m_quiteZoneRight = 1;
  private int m_quiteZoneTop = 1;
  private int m_quiteZoneBottom = 1;

  public PdfDataMatrixBarcode() => this.Initialize();

  public PdfDataMatrixBarcode(string text)
    : this()
  {
    this.Text = text;
  }

  public PdfDataMatrixEncoding Encoding
  {
    get => this.dataMatrixEncoding;
    set => this.dataMatrixEncoding = value;
  }

  public PdfDataMatrixSize Size
  {
    get => this.size;
    set => this.size = value;
  }

  internal int ActualRows
  {
    get => this.symbolAttribute.SymbolRow + (this.m_quiteZoneTop + this.m_quiteZoneBottom);
  }

  internal int ActualColumns
  {
    get => this.symbolAttribute.SymbolColumn + (this.m_quiteZoneLeft + this.m_quiteZoneRight);
  }

  private void Initialize()
  {
    this.QuietZone.All = 1f;
    this.Encoding = PdfDataMatrixEncoding.Auto;
    this.Size = PdfDataMatrixSize.Auto;
    this.XDimension = 0.86f;
    this.symbolAttributes = new PdfDataMatrixSymbolAttribute[30]
    {
      new PdfDataMatrixSymbolAttribute(10, 10, 1, 1, 3, 5, 1, 3),
      new PdfDataMatrixSymbolAttribute(12, 12, 1, 1, 5, 7, 1, 5),
      new PdfDataMatrixSymbolAttribute(14, 14, 1, 1, 8, 10, 1, 8),
      new PdfDataMatrixSymbolAttribute(16 /*0x10*/, 16 /*0x10*/, 1, 1, 12, 12, 1, 12),
      new PdfDataMatrixSymbolAttribute(18, 18, 1, 1, 18, 14, 1, 18),
      new PdfDataMatrixSymbolAttribute(20, 20, 1, 1, 22, 18, 1, 22),
      new PdfDataMatrixSymbolAttribute(22, 22, 1, 1, 30, 20, 1, 30),
      new PdfDataMatrixSymbolAttribute(24, 24, 1, 1, 36, 24, 1, 36),
      new PdfDataMatrixSymbolAttribute(26, 26, 1, 1, 44, 28, 1, 44),
      new PdfDataMatrixSymbolAttribute(32 /*0x20*/, 32 /*0x20*/, 2, 2, 62, 36, 1, 62),
      new PdfDataMatrixSymbolAttribute(36, 36, 2, 2, 86, 42, 1, 86),
      new PdfDataMatrixSymbolAttribute(40, 40, 2, 2, 114, 48 /*0x30*/, 1, 114),
      new PdfDataMatrixSymbolAttribute(44, 44, 2, 2, 144 /*0x90*/, 56, 1, 144 /*0x90*/),
      new PdfDataMatrixSymbolAttribute(48 /*0x30*/, 48 /*0x30*/, 2, 2, 174, 68, 1, 174),
      new PdfDataMatrixSymbolAttribute(52, 52, 2, 2, 204, 84, 2, 102),
      new PdfDataMatrixSymbolAttribute(64 /*0x40*/, 64 /*0x40*/, 4, 4, 280, 112 /*0x70*/, 2, 140),
      new PdfDataMatrixSymbolAttribute(72, 72, 4, 4, 368, 144 /*0x90*/, 4, 92),
      new PdfDataMatrixSymbolAttribute(80 /*0x50*/, 80 /*0x50*/, 4, 4, 456, 192 /*0xC0*/, 4, 114),
      new PdfDataMatrixSymbolAttribute(88, 88, 4, 4, 576, 224 /*0xE0*/, 4, 144 /*0x90*/),
      new PdfDataMatrixSymbolAttribute(96 /*0x60*/, 96 /*0x60*/, 4, 4, 696, 272, 4, 174),
      new PdfDataMatrixSymbolAttribute(104, 104, 4, 4, 816, 336, 6, 136),
      new PdfDataMatrixSymbolAttribute(120, 120, 6, 6, 1050, 408, 6, 175),
      new PdfDataMatrixSymbolAttribute(132, 132, 6, 6, 1304, 496, 8, 163),
      new PdfDataMatrixSymbolAttribute(144 /*0x90*/, 144 /*0x90*/, 6, 6, 1558, 620, 10, 156),
      new PdfDataMatrixSymbolAttribute(8, 18, 1, 1, 5, 7, 1, 5),
      new PdfDataMatrixSymbolAttribute(8, 32 /*0x20*/, 2, 1, 10, 11, 1, 10),
      new PdfDataMatrixSymbolAttribute(12, 26, 1, 1, 16 /*0x10*/, 14, 1, 16 /*0x10*/),
      new PdfDataMatrixSymbolAttribute(12, 36, 2, 1, 22, 18, 1, 22),
      new PdfDataMatrixSymbolAttribute(16 /*0x10*/, 36, 2, 1, 32 /*0x20*/, 24, 1, 32 /*0x20*/),
      new PdfDataMatrixSymbolAttribute(16 /*0x10*/, 48 /*0x30*/, 2, 1, 49, 28, 1, 49)
    };
    this.CreateLogArrays();
  }

  private void CreateLogArrays()
  {
    this.log = new int[256 /*0x0100*/];
    this.aLog = new int[256 /*0x0100*/];
    this.log[0] = -255;
    this.aLog[0] = 1;
    for (int index = 1; index <= (int) byte.MaxValue; ++index)
    {
      this.aLog[index] = this.aLog[index - 1] * 2;
      if (this.aLog[index] >= 256 /*0x0100*/)
        this.aLog[index] = this.aLog[index] ^ 301;
      this.log[this.aLog[index]] = index;
    }
  }

  private void CreatePolynomial(int step)
  {
    this.blockLength = 69;
    this.polynomial = new byte[this.blockLength];
    int num = this.symbolAttribute.CorrectionCodewords / step;
    for (int index = 0; index < this.polynomial.Length; ++index)
      this.polynomial[index] = (byte) 1;
    for (int j = 1; j <= num; ++j)
    {
      for (int index = j - 1; index >= 0; --index)
      {
        this.polynomial[index] = this.ErrorCorrectionCodeDoublify(this.polynomial[index], j);
        if (index > 0)
          this.polynomial[index] = PdfDataMatrixBarcode.ErrorCorrectionCodeSum(this.polynomial[index], this.polynomial[index - 1]);
      }
    }
  }

  private void CreateMatrix(int[] codeword)
  {
    int symbolColumn1 = this.symbolAttribute.SymbolColumn;
    int symbolRow1 = this.symbolAttribute.SymbolRow;
    int num1 = symbolColumn1 / this.symbolAttribute.HoriDataRegion;
    int num2 = symbolRow1 / this.symbolAttribute.VertDataRegion;
    int numColumn = symbolColumn1 - 2 * (symbolColumn1 / num1);
    int numRow = symbolRow1 - 2 * (symbolRow1 / num2);
    int[] array = new int[numColumn * numRow];
    this.ErrorCorrectingCode200Placement(array, numRow, numColumn);
    byte[] numArray1 = new byte[symbolColumn1 * symbolRow1];
    for (int index1 = 0; index1 < symbolRow1; index1 += num2)
    {
      for (int index2 = 0; index2 < symbolColumn1; ++index2)
        numArray1[index1 * symbolColumn1 + index2] = (byte) 1;
      for (int index3 = 0; index3 < symbolColumn1; index3 += 2)
        numArray1[(index1 + num2 - 1) * symbolColumn1 + index3] = (byte) 1;
    }
    for (int index4 = 0; index4 < symbolColumn1; index4 += num1)
    {
      for (int index5 = 0; index5 < symbolRow1; ++index5)
        numArray1[index5 * symbolColumn1 + index4] = (byte) 1;
      for (int index6 = 0; index6 < symbolRow1; index6 += 2)
        numArray1[index6 * symbolColumn1 + index4 + num1 - 1] = (byte) 1;
    }
    for (int index7 = 0; index7 < numRow; ++index7)
    {
      for (int index8 = 0; index8 < numColumn; ++index8)
      {
        int num3 = array[(numRow - index7 - 1) * numColumn + index8];
        if (num3 == 1 || num3 > 7 && (codeword[(num3 >> 3) - 1] & 1 << (num3 & 7)) != 0)
          numArray1[(1 + index7 + 2 * (index7 / (num2 - 2))) * symbolColumn1 + 1 + index8 + 2 * (index8 / (num1 - 2))] = (byte) 1;
      }
    }
    int symbolColumn2 = this.symbolAttribute.SymbolColumn;
    int symbolRow2 = this.symbolAttribute.SymbolRow;
    byte[,] numArray2 = new byte[symbolColumn2, symbolRow2];
    for (int index9 = 0; index9 < symbolColumn2; ++index9)
    {
      for (int index10 = 0; index10 < symbolRow2; ++index10)
        numArray2[index9, index10] = numArray1[symbolColumn2 * index10 + index9];
    }
    byte[,] dataMatrix = new byte[symbolRow2, symbolColumn2];
    for (int index11 = 0; index11 < symbolRow2; ++index11)
    {
      for (int index12 = 0; index12 < symbolColumn2; ++index12)
        dataMatrix[symbolRow2 - 1 - index11, index12] = numArray2[index12, index11];
    }
    this.AddQuiteZone(dataMatrix);
  }

  private void ErrorCorrectingCode200Placement(int[] array, int numRow, int numColumn)
  {
    for (int index1 = 0; index1 < numRow; ++index1)
    {
      for (int index2 = 0; index2 < numColumn; ++index2)
        array[index1 * numColumn + index2] = 0;
    }
    int num = 1;
    int row1 = 4;
    int column1 = 0;
    do
    {
      if (row1 == numRow && column1 == 0)
        this.ErrorCorrectingCode200PlacementCornerA(array, numRow, numColumn, num++);
      if (row1 == numRow - 2 && column1 == 0 && numColumn % 4 != 0)
        this.ErrorCorrectingCode200PlacementCornerB(array, numRow, numColumn, num++);
      if (row1 == numRow - 2 && column1 == 0 && numColumn % 8 == 4)
        this.ErrorCorrectingCode200PlacementCornerC(array, numRow, numColumn, num++);
      if (row1 == numRow + 4 && column1 == 2 && numColumn % 8 == 0)
        this.ErrorCorrectingCode200PlacementCornerD(array, numRow, numColumn, num++);
      do
      {
        if (row1 < numRow && column1 >= 0 && array[row1 * numColumn + column1] == 0)
          this.ErrorCorrectingCode200PlacementBlock(array, numRow, numColumn, row1, column1, num++);
        row1 -= 2;
        column1 += 2;
      }
      while (row1 >= 0 && column1 < numColumn);
      int row2 = row1 + 1;
      int column2 = column1 + 3;
      do
      {
        if (row2 >= 0 && column2 < numColumn && array[row2 * numColumn + column2] == 0)
          this.ErrorCorrectingCode200PlacementBlock(array, numRow, numColumn, row2, column2, num++);
        row2 += 2;
        column2 -= 2;
      }
      while (row2 < numRow && column2 >= 0);
      row1 = row2 + 3;
      column1 = column2 + 1;
    }
    while (row1 < numRow || column1 < numColumn);
    if (array[numRow * numColumn - 1] != 0)
      return;
    array[numRow * numColumn - 1] = array[numRow * numColumn - numColumn - 2] = 1;
  }

  private void ErrorCorrectingCode200PlacementCornerA(
    int[] array,
    int numRow,
    int numColumn,
    int place)
  {
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, numRow - 1, 0, place, '\a');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, numRow - 1, 1, place, '\u0006');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, numRow - 1, 2, place, '\u0005');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 0, numColumn - 2, place, '\u0004');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 0, numColumn - 1, place, '\u0003');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 1, numColumn - 1, place, '\u0002');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 2, numColumn - 1, place, '\u0001');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 3, numColumn - 1, place, char.MinValue);
  }

  private void ErrorCorrectingCode200PlacementCornerB(
    int[] array,
    int numRow,
    int numColumn,
    int place)
  {
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, numRow - 3, 0, place, '\a');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, numRow - 2, 0, place, '\u0006');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, numRow - 1, 0, place, '\u0005');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 0, numColumn - 4, place, '\u0004');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 0, numColumn - 3, place, '\u0003');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 0, numColumn - 2, place, '\u0002');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 0, numColumn - 1, place, '\u0001');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 1, numColumn - 1, place, char.MinValue);
  }

  private void ErrorCorrectingCode200PlacementCornerC(
    int[] array,
    int numRow,
    int numColumn,
    int place)
  {
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, numRow - 3, 0, place, '\a');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, numRow - 2, 0, place, '\u0006');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, numRow - 1, 0, place, '\u0005');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 0, numColumn - 2, place, '\u0004');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 0, numColumn - 1, place, '\u0003');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 1, numColumn - 1, place, '\u0002');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 2, numColumn - 1, place, '\u0001');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 3, numColumn - 1, place, char.MinValue);
  }

  private void ErrorCorrectingCode200PlacementCornerD(
    int[] array,
    int numRow,
    int numColumn,
    int place)
  {
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, numRow - 1, 0, place, '\a');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, numRow - 1, numColumn - 1, place, '\u0006');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 0, numColumn - 3, place, '\u0005');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 0, numColumn - 2, place, '\u0004');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 0, numColumn - 1, place, '\u0003');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 1, numColumn - 3, place, '\u0002');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 1, numColumn - 2, place, '\u0001');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, 1, numColumn - 1, place, char.MinValue);
  }

  private void ErrorCorrectingCode200PlacementBlock(
    int[] array,
    int numRow,
    int numColumn,
    int row,
    int column,
    int place)
  {
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, row - 2, column - 2, place, '\a');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, row - 2, column - 1, place, '\u0006');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, row - 1, column - 2, place, '\u0005');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, row - 1, column - 1, place, '\u0004');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, row - 1, column, place, '\u0003');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, row, column - 2, place, '\u0002');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, row, column - 1, place, '\u0001');
    this.ErrorCorrectingCode200PlacementBit(array, numRow, numColumn, row, column, place, char.MinValue);
  }

  private void ErrorCorrectingCode200PlacementBit(
    int[] array,
    int numRow,
    int numColumn,
    int row,
    int column,
    int place,
    char character)
  {
    if (row < 0)
    {
      row += numRow;
      column += 4 - (numRow + 4) % 8;
    }
    if (column < 0)
    {
      column += numColumn;
      row += 4 - (numColumn + 4) % 8;
    }
    array[row * numColumn + column] = (place << 3) + (int) character;
  }

  internal void BuildDataMatrix() => this.CreateMatrix(this.PrepareCodeword(this.GetData()));

  private int[] PrepareCodeword(byte[] dataCodeword)
  {
    byte[] codeword = this.PrepareDataCodeword(dataCodeword);
    int[] errorCorrection = this.ComputeErrorCorrection(ref codeword);
    int[] numArray = new int[codeword.Length + errorCorrection.Length];
    codeword.CopyTo((Array) numArray, 0);
    errorCorrection.CopyTo((Array) numArray, codeword.Length);
    return numArray;
  }

  private byte[] DataMatrixBaseEncoder(byte[] dataCodeword)
  {
    int num = 1;
    if (dataCodeword.Length > 249)
      ++num;
    byte[] destinationArray = new byte[1 + num + dataCodeword.Length];
    destinationArray[0] = (byte) 231;
    if (dataCodeword.Length <= 249)
    {
      destinationArray[1] = (byte) dataCodeword.Length;
    }
    else
    {
      destinationArray[1] = (byte) (dataCodeword.Length / 250 + 249);
      destinationArray[2] = (byte) (dataCodeword.Length % 250);
    }
    Array.Copy((Array) dataCodeword, 0, (Array) destinationArray, 1 + num, dataCodeword.Length);
    for (int index = 1; index < destinationArray.Length; ++index)
      destinationArray[index] = this.ComputeBase256Codeword((int) destinationArray[index], index);
    return destinationArray;
  }

  private byte ComputeBase256Codeword(int codeWordValue, int index)
  {
    int num1 = 149 * (index + 1) % (int) byte.MaxValue + 1;
    int num2 = codeWordValue + num1;
    return num2 <= (int) byte.MaxValue ? (byte) num2 : (byte) (num2 - 256 /*0x0100*/);
  }

  private byte[] DataMatrixASCIINumericEncoder(byte[] dataCodeword)
  {
    byte[] destinationArray = dataCodeword;
    bool flag = true;
    if (destinationArray.Length % 2 == 1)
    {
      flag = false;
      destinationArray = new byte[dataCodeword.Length + 1];
      Array.Copy((Array) dataCodeword, 0, (Array) destinationArray, 0, dataCodeword.Length);
    }
    byte[] numArray = new byte[destinationArray.Length / 2];
    for (int index = 0; index < numArray.Length; ++index)
      numArray[index] = flag || index != numArray.Length - 1 ? (byte) (((int) destinationArray[2 * index] - 48 /*0x30*/) * 10 + ((int) destinationArray[2 * index + 1] - 48 /*0x30*/) + 130) : (byte) ((uint) destinationArray[2 * index] + 1U);
    return numArray;
  }

  private byte[] DataMatrixASCIIEncoder(byte[] dataCodeword)
  {
    byte[] sourceArray = new byte[dataCodeword.Length];
    int length = 0;
    for (int index1 = 0; index1 < dataCodeword.Length; ++index1)
    {
      if (dataCodeword[index1] >= (byte) 48 /*0x30*/ && dataCodeword[index1] <= (byte) 57)
      {
        int index2 = 0;
        if (index1 != 0)
          index2 = length - 1;
        byte num1 = (byte) ((uint) sourceArray[index2] - 1U);
        byte num2 = 0;
        if (index1 != 0 && length != 1)
          num2 = sourceArray[index2 - 1];
        if (num2 != (byte) 235 && num1 >= (byte) 48 /*0x30*/ && num1 <= (byte) 57)
          sourceArray[index2] = (byte) (10 * ((int) num1 - 48 /*0x30*/) + ((int) dataCodeword[index1] - 48 /*0x30*/) + 130);
        else
          sourceArray[length++] = (byte) ((uint) dataCodeword[index1] + 1U);
      }
      else if (dataCodeword[index1] < (byte) 127 /*0x7F*/)
      {
        sourceArray[length++] = (byte) ((uint) dataCodeword[index1] + 1U);
      }
      else
      {
        sourceArray[length] = (byte) 235;
        sourceArray[length++] = (byte) ((uint) dataCodeword[index1] - (uint) sbyte.MaxValue);
      }
    }
    byte[] destinationArray = new byte[length];
    Array.Copy((Array) sourceArray, (Array) destinationArray, length);
    return destinationArray;
  }

  private int[] ComputeErrorCorrection(ref byte[] codeword)
  {
    int length1 = codeword.Length;
    this.symbolAttribute = new PdfDataMatrixSymbolAttribute();
    if (this.Size == PdfDataMatrixSize.Auto)
    {
      foreach (PdfDataMatrixSymbolAttribute symbolAttribute in this.symbolAttributes)
      {
        if (symbolAttribute.DataCodewords >= length1)
        {
          this.symbolAttribute = symbolAttribute;
          break;
        }
      }
    }
    else
      this.symbolAttribute = this.symbolAttributes[(int) (this.Size - 1)];
    if (this.symbolAttribute.DataCodewords > length1)
    {
      byte[] codeword1;
      this.PadCodewords(this.symbolAttribute.DataCodewords, codeword, out codeword1);
      codeword = new byte[codeword1.Length];
      codeword1.CopyTo((Array) codeword, 0);
      int length2 = codeword.Length;
    }
    else
    {
      if (this.symbolAttribute.DataCodewords == 0)
        throw new PdfBarcodeException("Data cannot be encoded as barcode");
      if (this.symbolAttribute.DataCodewords < length1)
        throw new PdfBarcodeException($"Data too long for {this.symbolAttribute.SymbolRow.ToString()}x{this.symbolAttribute.SymbolColumn.ToString()} barcode.");
    }
    int correctionCodewords = this.symbolAttribute.CorrectionCodewords;
    int[] errorCorrection = new int[correctionCodewords + this.symbolAttribute.DataCodewords];
    int interleavedBlock = this.symbolAttribute.InterleavedBlock;
    int dataCodewords = this.symbolAttribute.DataCodewords;
    int num1 = this.symbolAttribute.CorrectionCodewords / interleavedBlock;
    int num2 = dataCodewords + num1 * interleavedBlock;
    this.CreatePolynomial(interleavedBlock);
    this.blockLength = 68;
    byte[] numArray1 = new byte[this.blockLength];
    for (int index1 = 0; index1 < interleavedBlock; ++index1)
    {
      for (int index2 = 0; index2 < numArray1.Length; ++index2)
        numArray1[index2] = (byte) 0;
      for (int index3 = index1; index3 < dataCodewords; index3 += interleavedBlock)
      {
        int j = (int) PdfDataMatrixBarcode.ErrorCorrectionCodeSum(numArray1[num1 - 1], codeword[index3]);
        for (int index4 = num1 - 1; index4 > 0; --index4)
          numArray1[index4] = PdfDataMatrixBarcode.ErrorCorrectionCodeSum(numArray1[index4 - 1], this.ErrorCorrectionCodeProduct(this.polynomial[index4], j));
        numArray1[0] = this.ErrorCorrectionCodeProduct(this.polynomial[0], j);
      }
      int num3 = index1 < 8 || this.Size != PdfDataMatrixSize.Size144x144 ? this.symbolAttribute.InterleavedDataBlock : this.symbolAttribute.DataCodewords / interleavedBlock;
      int num4 = num1;
      for (int index5 = index1 + interleavedBlock * num3; index5 < num2; index5 += interleavedBlock)
        errorCorrection[index5] = (int) numArray1[--num4];
      if (num4 != 0)
        throw new Exception("Error in error correction code generation!");
    }
    if (errorCorrection.Length > correctionCodewords)
    {
      int[] numArray2 = errorCorrection;
      errorCorrection = new int[correctionCodewords];
      int num5 = 0;
      for (int length3 = numArray2.Length; length3 > this.symbolAttribute.DataCodewords; --length3)
        errorCorrection[num5++] = numArray2[length3 - 1];
    }
    Array.Reverse((Array) errorCorrection);
    return errorCorrection;
  }

  private byte ErrorCorrectionCodeDoublify(byte i, int j)
  {
    if (i == (byte) 0)
      return 0;
    return j == 0 ? i : (byte) this.aLog[(this.log[(int) i] + j) % (int) byte.MaxValue];
  }

  private byte ErrorCorrectionCodeProduct(byte i, int j)
  {
    return i == (byte) 0 || j == 0 ? (byte) 0 : (byte) this.aLog[(this.log[(int) i] + this.log[j]) % (int) byte.MaxValue];
  }

  private static byte ErrorCorrectionCodeSum(byte i, byte j) => (byte) ((uint) i ^ (uint) j);

  private void PadCodewords(int dataCodeWordLength, byte[] temp, out byte[] codeword)
  {
    int length1 = temp.Length;
    using (MemoryStream memoryStream = new MemoryStream())
    {
      for (int index = 0; index < length1; ++index)
        memoryStream.WriteByte(temp[index]);
      if (length1 < dataCodeWordLength)
        memoryStream.WriteByte((byte) 129);
      int length2;
      for (length2 = (int) memoryStream.Length; length2 < dataCodeWordLength; length2 = (int) memoryStream.Length)
      {
        int num = 129 + (length2 + 1) * 149 % 253 + 1;
        if (num > 254)
          num -= 254;
        memoryStream.WriteByte((byte) num);
      }
      codeword = new byte[memoryStream.Length];
      Array.Copy((Array) memoryStream.ToArray(), (Array) codeword, length2);
    }
  }

  private byte[] PrepareDataCodeword(byte[] dataCodeword)
  {
    if (this.Encoding == PdfDataMatrixEncoding.Auto || this.Encoding == PdfDataMatrixEncoding.ASCIINumeric)
    {
      bool flag1 = true;
      bool flag2 = false;
      int num = 0;
      byte[] numArray = dataCodeword;
      PdfDataMatrixEncoding dataMatrixEncoding = PdfDataMatrixEncoding.ASCII;
      for (int index = 0; index < numArray.Length; ++index)
      {
        if (numArray[index] < (byte) 48 /*0x30*/ || numArray[index] > (byte) 57)
          flag1 = false;
        else if (numArray[index] > (byte) 127 /*0x7F*/)
        {
          ++num;
          if (num > 3)
          {
            flag2 = true;
            break;
          }
        }
      }
      if (flag1)
        dataMatrixEncoding = PdfDataMatrixEncoding.ASCIINumeric;
      if (flag2)
        dataMatrixEncoding = PdfDataMatrixEncoding.Base256;
      this.Encoding = this.Encoding != PdfDataMatrixEncoding.ASCIINumeric || this.Encoding == dataMatrixEncoding ? dataMatrixEncoding : throw new PdfBarcodeException("Data contains invalid characters and cannot be encoded as ASCIINumeric.");
    }
    byte[] numArray1 = (byte[]) null;
    switch (this.Encoding)
    {
      case PdfDataMatrixEncoding.ASCII:
        numArray1 = this.DataMatrixASCIIEncoder(dataCodeword);
        break;
      case PdfDataMatrixEncoding.ASCIINumeric:
        numArray1 = this.DataMatrixASCIINumericEncoder(dataCodeword);
        break;
      case PdfDataMatrixEncoding.Base256:
        numArray1 = this.DataMatrixBaseEncoder(dataCodeword);
        break;
    }
    return numArray1;
  }

  private void AddQuiteZone(byte[,] dataMatrix)
  {
    int quiteZone = this.GetQuiteZone();
    int actualRows = this.ActualRows;
    int actualColumns = this.ActualColumns;
    this.dataMatrixArray = new byte[actualRows, actualColumns];
    for (int index = 0; index < actualColumns; ++index)
      this.dataMatrixArray[0, index] = (byte) 0;
    for (int index1 = quiteZone; index1 < actualRows - quiteZone; ++index1)
    {
      this.dataMatrixArray[index1, 0] = (byte) 0;
      for (int index2 = quiteZone; index2 < actualColumns - quiteZone; ++index2)
        this.dataMatrixArray[index1, index2] = dataMatrix[index1 - quiteZone, index2 - quiteZone];
      this.dataMatrixArray[index1, actualColumns - quiteZone] = (byte) 0;
    }
    for (int index = 0; index < actualColumns; ++index)
      this.dataMatrixArray[actualRows - quiteZone, index] = (byte) 0;
  }

  private int GetQuiteZone()
  {
    int quiteZone = 1;
    if (this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0)
    {
      this.m_quiteZoneLeft = this.m_quiteZoneRight = this.m_quiteZoneTop = this.m_quiteZoneBottom = (int) this.QuietZone.All;
      quiteZone = (int) this.QuietZone.All;
    }
    return quiteZone;
  }

  public override Image ToImage()
  {
    this.BuildDataMatrix();
    int pixels = (int) new PdfUnitConvertor().ConvertToPixels(this.XDimension, PdfGraphicsUnit.Point);
    int num1 = this.ActualColumns * pixels;
    int num2 = this.ActualRows * pixels;
    int num3 = 0;
    int num4 = 0;
    float num5 = (float) (num3 + (this.QuietZone.IsAll || (int) this.QuietZone.Left <= 0 ? 0 : (int) this.QuietZone.Left));
    float num6 = (float) (num4 + (this.QuietZone.IsAll || (int) this.QuietZone.Top <= 0 ? 0 : (int) this.QuietZone.Top));
    Bitmap image = new Bitmap(num1 + (int) num5, num2 + (int) num6, PixelFormat.Format32bppRgb);
    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) image))
    {
      graphics.Clear(Color.White);
      Brush brush1 = Brushes.White;
      Brush brush2 = Brushes.Black;
      int actualRows = this.ActualRows;
      int actualColumns = this.ActualColumns;
      int y = num4 + (this.QuietZone.IsAll || (int) this.QuietZone.Top <= 0 ? 0 : (int) this.QuietZone.Top);
      for (int index1 = 0; index1 < actualRows; ++index1)
      {
        int x = num3 + (this.QuietZone.IsAll || (int) this.QuietZone.Left <= 0 ? 0 : (int) this.QuietZone.Left);
        for (int index2 = 0; index2 < actualColumns; ++index2)
        {
          if (this.BackColor.A != (byte) 0)
          {
            Color color = Color.FromArgb(this.BackColor.ToArgb());
            if (color != Color.White)
              brush1 = (Brush) new SolidBrush(color);
          }
          if (this.ForeColor.A != (byte) 0)
          {
            Color color = Color.FromArgb(this.ForeColor.ToArgb());
            if (color != Color.Black)
              brush2 = (Brush) new SolidBrush(color);
          }
          Brush brush3 = this.dataMatrixArray[index1, index2] != (byte) 1 ? brush1 : brush2;
          graphics.FillRectangle(brush3, new Rectangle(x, y, pixels, pixels));
          x += pixels;
        }
        y += pixels;
        num3 = 0;
      }
    }
    return (Image) image;
  }

  public Image ToImage(SizeF size)
  {
    bool flag = !(size == SizeF.Empty);
    this.BuildDataMatrix();
    float xdimension = this.XDimension;
    byte[,] dataMatrixArray = this.dataMatrixArray;
    int actualRows = this.ActualRows;
    int actualColumns = this.ActualColumns;
    PdfColor backColor = this.BackColor;
    PdfBarcodeQuietZones quietZone = this.QuietZone;
    PdfColor foreColor = this.ForeColor;
    int pixels = (int) new PdfUnitConvertor().ConvertToPixels(xdimension, PdfGraphicsUnit.Point);
    float num1 = 0.0f;
    float num2 = 0.0f;
    int num3 = 0;
    int y1 = 0;
    float width = 0.0f;
    float height = 0.0f;
    float num4 = num1 + (quietZone.IsAll || (int) quietZone.Left <= 0 ? 0.0f : (float) (int) quietZone.Left);
    float num5 = num2 + (quietZone.IsAll || (int) quietZone.Top <= 0 ? 0.0f : (float) (int) quietZone.Top);
    Bitmap image = new Bitmap((int) size.Width + (int) num4, (int) size.Height + (int) num5, PixelFormat.Format32bppRgb);
    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) image))
    {
      graphics.Clear(Color.White);
      Brush brush1 = Brushes.White;
      Brush brush2 = Brushes.Black;
      int num6 = actualRows;
      int num7 = actualColumns;
      float num8 = 0.0f;
      if (flag)
      {
        width = size.Width / (float) actualColumns;
        height = size.Height / (float) num6;
        graphics.FillRectangle(Brushes.White, 0.0f, 0.0f, size.Width, size.Height);
        if ((double) height > (double) width)
        {
          height = width;
          float num9 = width * (float) actualColumns;
          float num10 = height * (float) num6;
          num8 = (float) ((double) size.Width / 2.0 - (double) num9 / 2.0);
          num2 = (float) ((double) size.Height / 2.0 - (double) num10 / 2.0);
        }
        else if ((double) width > (double) height)
        {
          width = height;
          float num11 = width * (float) actualColumns;
          float num12 = height * (float) num6;
          num8 = (float) ((double) size.Width / 2.0 - (double) num11 / 2.0);
          num2 = (float) ((double) size.Height / 2.0 - (double) num12 / 2.0);
        }
      }
      float y2 = num2 + (quietZone.IsAll || (int) quietZone.Top <= 0 ? 0.0f : (float) (int) quietZone.Top);
      for (int index1 = 0; index1 < num6; ++index1)
      {
        float x1 = num8 + (quietZone.IsAll || (int) quietZone.Left <= 0 ? 0.0f : (float) (int) quietZone.Left);
        num3 = 0;
        int x2 = 0;
        for (int index2 = 0; index2 < num7; ++index2)
        {
          if (backColor.A != (byte) 0)
          {
            Color color = Color.FromArgb(backColor.ToArgb());
            if (color != Color.White)
              brush1 = (Brush) new SolidBrush(color);
          }
          if (foreColor.A != (byte) 0)
          {
            Color color = Color.FromArgb(foreColor.ToArgb());
            if (color != Color.Black)
              brush2 = (Brush) new SolidBrush(color);
          }
          Brush brush3 = dataMatrixArray[index1, index2] != (byte) 1 ? brush1 : brush2;
          if (flag)
          {
            graphics.FillRectangle(brush3, x1, y2, width, height);
            x1 += width;
          }
          else
          {
            graphics.FillRectangle(brush3, new Rectangle(x2, y1, pixels, pixels));
            x2 += pixels;
          }
        }
        if (flag)
          y2 += height;
        else
          y1 += pixels;
      }
    }
    return (Image) image;
  }

  public override void Draw(PdfPageBase page, PointF location)
  {
    this.BuildDataMatrix();
    bool flag = false;
    if (page is PdfPage && (page as PdfPage).Document != null)
      flag = (page as PdfPage).Document.AutoTag;
    PdfBrush pdfBrush1 = PdfBrushes.Black;
    PdfBrush pdfBrush2 = PdfBrushes.White;
    float x1 = location.X;
    float y1 = location.Y;
    int actualRows = this.ActualRows;
    int actualColumns = this.ActualColumns;
    float y2 = location.Y + (this.QuietZone.IsAll || (int) this.QuietZone.Top <= 0 ? 0.0f : (float) (int) this.QuietZone.Top);
    PdfTemplate template = (PdfTemplate) null;
    if (flag)
      template = new PdfTemplate(new SizeF((float) actualColumns * this.XDimension, (float) actualRows * this.XDimension));
    for (int index1 = 0; index1 < actualRows; ++index1)
    {
      float x2 = location.X + (this.QuietZone.IsAll || (int) this.QuietZone.Left <= 0 ? 0.0f : (float) (int) this.QuietZone.Left);
      for (int index2 = 0; index2 < actualColumns; ++index2)
      {
        if (this.BackColor.A != (byte) 0)
        {
          Color color = Color.FromArgb(this.BackColor.ToArgb());
          if (color != Color.White)
            pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) color);
        }
        if (this.ForeColor.A != (byte) 0)
        {
          Color color = Color.FromArgb(this.ForeColor.ToArgb());
          if (color != Color.Black)
            pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) color);
        }
        PdfBrush brush = this.dataMatrixArray[index1, index2] != (byte) 1 ? pdfBrush2 : pdfBrush1;
        if (flag)
          template.Graphics.DrawRectangle(brush, x2, y2, this.XDimension, this.XDimension);
        else
          page.Graphics.DrawRectangle(brush, x2, y2, this.XDimension, this.XDimension);
        x2 += this.XDimension;
      }
      y2 += this.XDimension;
    }
    if (!flag)
      return;
    page.Graphics.DrawPdfTemplate(template, location);
  }

  public void Draw(PdfPageBase page, PointF location, SizeF Size)
  {
    this.BuildDataMatrix();
    bool flag = false;
    if (page is PdfPage && (page as PdfPage).Document != null)
      flag = (page as PdfPage).Document.AutoTag;
    PdfBrush pdfBrush1 = PdfBrushes.Black;
    PdfBrush pdfBrush2 = PdfBrushes.White;
    float x1 = location.X;
    float y = location.Y;
    int actualRows = this.ActualRows;
    int actualColumns = this.ActualColumns;
    float width = Size.Width / (float) actualRows + this.XDimension;
    float height = Size.Height / (float) actualColumns + this.XDimension;
    PdfTemplate template = (PdfTemplate) null;
    if (flag)
      template = new PdfTemplate(Size);
    for (int index1 = 0; index1 < actualRows; ++index1)
    {
      float x2 = location.X;
      for (int index2 = 0; index2 < actualColumns; ++index2)
      {
        if (this.BackColor.A != (byte) 0)
        {
          Color color = Color.FromArgb(this.BackColor.ToArgb());
          if (color != Color.White)
            pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) color);
        }
        if (this.ForeColor.A != (byte) 0)
        {
          Color color = Color.FromArgb(this.ForeColor.ToArgb());
          if (color != Color.Black)
            pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) color);
        }
        PdfBrush brush = this.dataMatrixArray[index1, index2] != (byte) 1 ? pdfBrush2 : pdfBrush1;
        if (flag)
          template.Graphics.DrawRectangle(brush, x2, y, width, height);
        else
          page.Graphics.DrawRectangle(brush, x2, y, width, height);
        x2 += width;
      }
      y += height;
    }
    if (!flag)
      return;
    page.Graphics.DrawPdfTemplate(template, location);
  }

  public void Draw(PdfPageBase page, RectangleF Rectangle)
  {
    this.BuildDataMatrix();
    bool flag = false;
    if (page is PdfPage && (page as PdfPage).Document != null)
      flag = (page as PdfPage).Document.AutoTag;
    PdfBrush pdfBrush1 = PdfBrushes.Black;
    PdfBrush pdfBrush2 = PdfBrushes.White;
    float x1 = Rectangle.X;
    float y = Rectangle.Y;
    int actualRows = this.ActualRows;
    int actualColumns = this.ActualColumns;
    float width = Rectangle.Width / (float) actualRows + this.XDimension;
    float height = Rectangle.Height / (float) actualColumns + this.XDimension;
    PdfTemplate template = (PdfTemplate) null;
    if (flag)
      template = new PdfTemplate(new SizeF(Rectangle.Width, Rectangle.Height));
    for (int index1 = 0; index1 < actualRows; ++index1)
    {
      float x2 = Rectangle.X;
      for (int index2 = 0; index2 < actualColumns; ++index2)
      {
        if (this.BackColor.A != (byte) 0)
        {
          Color color = Color.FromArgb(this.BackColor.ToArgb());
          if (color != Color.White)
            pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) color);
        }
        if (this.ForeColor.A != (byte) 0)
        {
          Color color = Color.FromArgb(this.ForeColor.ToArgb());
          if (color != Color.Black)
            pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) color);
        }
        PdfBrush brush = this.dataMatrixArray[index1, index2] != (byte) 1 ? pdfBrush2 : pdfBrush1;
        if (flag)
          template.Graphics.DrawRectangle(brush, x2, y, width, height);
        else
          page.Graphics.DrawRectangle(brush, x2, y, width, height);
        x2 += width;
      }
      y += height;
    }
    if (!flag)
      return;
    page.Graphics.DrawPdfTemplate(template, new PointF(Rectangle.X, Rectangle.Y));
  }

  public void Draw(PdfPageBase page, float x, float y, float Width, float Height)
  {
    this.BuildDataMatrix();
    bool flag = false;
    if (page is PdfPage && (page as PdfPage).Document != null)
      flag = (page as PdfPage).Document.AutoTag;
    PdfBrush pdfBrush1 = PdfBrushes.Black;
    PdfBrush pdfBrush2 = PdfBrushes.White;
    int actualRows = this.ActualRows;
    int actualColumns = this.ActualColumns;
    float width = Width / (float) actualRows + this.XDimension;
    float height = Height / (float) actualColumns + this.XDimension;
    float y1 = y + (this.QuietZone.IsAll || (int) this.QuietZone.Top <= 0 ? 0.0f : (float) (int) this.QuietZone.Top);
    PdfTemplate template = (PdfTemplate) null;
    if (flag)
      template = new PdfTemplate(new SizeF(Width, Height));
    for (int index1 = 0; index1 < actualRows; ++index1)
    {
      float x1 = x + (this.QuietZone.IsAll || (int) this.QuietZone.Left <= 0 ? 0.0f : (float) (int) this.QuietZone.Left);
      for (int index2 = 0; index2 < actualColumns; ++index2)
      {
        if (this.BackColor.A != (byte) 0)
        {
          Color color = Color.FromArgb(this.BackColor.ToArgb());
          if (color != Color.White)
            pdfBrush2 = (PdfBrush) new PdfSolidBrush((PdfColor) color);
        }
        if (this.ForeColor.A != (byte) 0)
        {
          Color color = Color.FromArgb(this.ForeColor.ToArgb());
          if (color != Color.Black)
            pdfBrush1 = (PdfBrush) new PdfSolidBrush((PdfColor) color);
        }
        PdfBrush brush = this.dataMatrixArray[index1, index2] != (byte) 1 ? pdfBrush2 : pdfBrush1;
        if (flag)
          template.Graphics.DrawRectangle(brush, x1, y1, width, height);
        else
          page.Graphics.DrawRectangle(brush, x1, y1, width, height);
        x1 += width;
      }
      y1 += height;
    }
    if (!flag)
      return;
    page.Graphics.DrawPdfTemplate(template, new PointF(x, y));
  }

  public override void Draw(PdfPageBase page) => this.Draw(page, this.Location);
}
