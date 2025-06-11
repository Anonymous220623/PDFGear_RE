// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfCodabarBarcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public class PdfCodabarBarcode : PdfUnidimensionalBarcode
{
  public PdfCodabarBarcode() => this.Initialize();

  public PdfCodabarBarcode(string text)
    : this()
  {
    this.Text = text;
  }

  private void Initialize()
  {
    this.StartSymbol = 'A';
    this.StopSymbol = 'B';
    this.ValidatorExpression = "^[\\d\\-\\$\\:\\/\\.\\+]+$";
    this.BarcodeSymbols['0'] = new BarcodeSymbolTable('0', 0, new byte[7]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2
    });
    this.BarcodeSymbols['1'] = new BarcodeSymbolTable('1', 0, new byte[7]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['2'] = new BarcodeSymbolTable('2', 0, new byte[7]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['3'] = new BarcodeSymbolTable('3', 0, new byte[7]
    {
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['4'] = new BarcodeSymbolTable('4', 0, new byte[7]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['5'] = new BarcodeSymbolTable('5', 0, new byte[7]
    {
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['6'] = new BarcodeSymbolTable('6', 0, new byte[7]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['7'] = new BarcodeSymbolTable('7', 0, new byte[7]
    {
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['8'] = new BarcodeSymbolTable('8', 0, new byte[7]
    {
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['9'] = new BarcodeSymbolTable('9', 0, new byte[7]
    {
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['-'] = new BarcodeSymbolTable('-', 0, new byte[7]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['$'] = new BarcodeSymbolTable('$', 0, new byte[7]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols[':'] = new BarcodeSymbolTable(':', 0, new byte[7]
    {
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['/'] = new BarcodeSymbolTable('/', 0, new byte[7]
    {
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['.'] = new BarcodeSymbolTable('.', 0, new byte[7]
    {
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 1
    });
    this.BarcodeSymbols['+'] = new BarcodeSymbolTable('+', 0, new byte[7]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2
    });
    this.BarcodeSymbols['A'] = new BarcodeSymbolTable('A', 0, new byte[7]
    {
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 1
    });
    this.BarcodeSymbols['B'] = new BarcodeSymbolTable('B', 0, new byte[7]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 2
    });
  }
}
