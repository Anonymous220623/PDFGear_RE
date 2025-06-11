// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.BarcodeSymbolTable
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Barcode;

internal class BarcodeSymbolTable
{
  private char symbol;
  private int checkDigit;
  private byte[] bars;

  public BarcodeSymbolTable()
  {
  }

  public BarcodeSymbolTable(char symbol, int checkDigit, byte[] bars)
  {
    this.symbol = symbol;
    this.checkDigit = checkDigit;
    this.bars = bars;
  }

  public BarcodeSymbolTable(int checkDigit, byte[] bars)
  {
    this.checkDigit = checkDigit;
    this.bars = bars;
  }

  public char Symbol
  {
    get => this.symbol;
    set => this.symbol = value;
  }

  public int CheckDigit
  {
    get => this.checkDigit;
    set => this.checkDigit = value;
  }

  public byte[] Bars
  {
    get => this.bars;
    set => this.bars = value;
  }
}
