// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.Pdf417Barcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public class Pdf417Barcode : PdfBidimensionalBarcode
{
  private const int SwitchToTextMode = 900;
  private const int SwitchToByteMode = 901;
  private const int SwitchToNumericMode = 902;
  private const int ShiftToByteMode = 913;
  private const int SwitchToByteModeForSix = 924;
  private const int MaxCodeWords = 929;
  private const int DataRowsMax = 90;
  private const int DataColumnsMax = 30;
  private const int CodeWordLength = 17;
  private Pdf417ErrorCorrectionLevel m_errorCorrectionLevel = Pdf417ErrorCorrectionLevel.Auto;
  private int dataRows;
  private int dataColums;
  private byte[] inputBinaryData;
  private int inputDataLength;
  private int textDataPosition;
  private List<int> codeWordsList;
  private EncodingMode encodingMode;
  private TextEncodingMode textEncodingMode;
  private EncodingControl encodingControl;
  private float barWidthPixel = 2f;
  private float rowHeightPixel = 6f;
  private float quietZonePixel = 4f;
  private bool[,] pdf417Matrix;
  private int errorCorrectionLength;
  private int[] errorCorrectionCodewords;
  private int defaultDataColumns = 3;
  private readonly bool[] StartPattern = new bool[17]
  {
    true,
    true,
    true,
    true,
    true,
    true,
    true,
    true,
    false,
    true,
    false,
    true,
    false,
    true,
    false,
    false,
    false
  };
  private readonly bool[] StopPattern = new bool[18]
  {
    true,
    true,
    true,
    true,
    true,
    true,
    true,
    false,
    true,
    false,
    false,
    false,
    true,
    false,
    true,
    false,
    false,
    true
  };

  public Pdf417Barcode()
  {
    this.XDimension = 1f;
    this.QuietZone.All = 0.0f;
  }

  public override SizeF Size
  {
    get => base.Size.IsEmpty ? this.GetBarcodeSize() : base.Size;
    set => base.Size = value;
  }

  internal int DataRows
  {
    get => this.dataRows;
    set => this.dataRows = value;
  }

  internal int DataColumns
  {
    get => this.dataColums;
    set => this.dataColums = value;
  }

  internal int BarcodeColumns => 17 * (this.DataColumns + 4) + 1;

  internal float BarcodeWidth
  {
    get
    {
      return (float) ((double) this.barWidthPixel * (double) this.BarcodeColumns + 2.0 * (double) this.QuietZoneNew + (this.QuietZone.IsAll || (double) this.QuietZone.Left <= 0.0 ? 0.0 : (double) this.QuietZone.Left) + (this.QuietZone.IsAll || (double) this.QuietZone.Right <= 0.0 ? 0.0 : (double) this.QuietZone.Right));
    }
  }

  internal float BarcodeHeight
  {
    get
    {
      return (float) ((double) this.rowHeightPixel * (double) this.DataRows + 2.0 * (double) this.QuietZoneNew + (this.QuietZone.IsAll || (double) this.QuietZone.Top <= 0.0 ? 0.0 : (double) this.QuietZone.Top) + (this.QuietZone.IsAll || (double) this.QuietZone.Bottom <= 0.0 ? 0.0 : (double) this.QuietZone.Bottom));
    }
  }

  internal float RowHeight
  {
    get => this.rowHeightPixel;
    set => this.rowHeightPixel = value;
  }

  internal float QuietZoneNew
  {
    get => this.quietZonePixel;
    set => this.quietZonePixel = value;
  }

  public Pdf417ErrorCorrectionLevel ErrorCorrectionLevel
  {
    get => this.m_errorCorrectionLevel;
    set => this.m_errorCorrectionLevel = value;
  }

  public override void Draw(PdfPageBase page) => this.Draw(page, this.Location);

  public override void Draw(PdfPageBase page, PointF location)
  {
    if (string.IsNullOrEmpty(this.Text))
      throw new PdfBarcodeException("Input data is empty");
    bool flag = false;
    if (page is PdfPage && (page as PdfPage).Document != null)
      flag = (page as PdfPage).Document.AutoTag;
    byte[] bytes = Encoding.UTF8.GetBytes(this.Text);
    this.QuietZoneNew = this.GetQuiteZone();
    this.barWidthPixel = (float) ((double) this.barWidthPixel * (double) this.XDimension / 2.0);
    this.rowHeightPixel = (float) ((double) this.rowHeightPixel * (double) this.XDimension / 2.0);
    this.EncodeTextData(bytes);
    if (this.pdf417Matrix == null)
      this.pdf417Matrix = this.Create417BarcodeMatrix();
    double pixels = (double) new PdfUnitConvertor(96f).ConvertToPixels(this.XDimension, PdfGraphicsUnit.Point);
    float num1 = 1f;
    float num2 = 1f;
    if (base.Size != SizeF.Empty)
    {
      num1 = base.Size.Width / this.BarcodeWidth;
      num2 = base.Size.Height / this.BarcodeHeight;
    }
    int barcodeColumns = this.BarcodeColumns;
    int dataRows = this.DataRows;
    PdfBrush brush1 = PdfBrushes.White;
    PdfBrush brush2 = PdfBrushes.Black;
    if (this.BackColor.A != (byte) 0)
    {
      Color color = Color.FromArgb(this.BackColor.ToArgb());
      if (color != Color.White)
        brush1 = (PdfBrush) new PdfSolidBrush((PdfColor) color);
    }
    if (this.ForeColor.A != (byte) 0)
    {
      Color color = Color.FromArgb(this.ForeColor.ToArgb());
      if (color != Color.Black)
        brush2 = (PdfBrush) new PdfSolidBrush((PdfColor) color);
    }
    PdfGraphics graphics = page.Graphics;
    float x = 0.0f;
    float y = 0.0f;
    PdfTemplate template = (PdfTemplate) null;
    if (flag)
    {
      template = !(this.Size != SizeF.Empty) ? new PdfTemplate(new SizeF(this.BarcodeWidth * num1, this.BarcodeHeight * num2)) : new PdfTemplate(this.Size);
      template.Graphics.DrawRectangle(brush1, x, y, this.BarcodeWidth * num1, this.BarcodeHeight * num2);
    }
    else
      graphics.DrawRectangle(brush1, x + location.X, y + location.Y, this.BarcodeWidth * num1, this.BarcodeHeight * num2);
    float num3 = y + (this.QuietZone.IsAll || (double) this.QuietZone.Top <= 0.0 ? 0.0f : this.QuietZone.Top);
    float num4 = x + (this.QuietZone.IsAll || (double) this.QuietZone.Left <= 0.0 ? 0.0f : this.QuietZone.Left);
    for (int index1 = 0; index1 < dataRows; ++index1)
    {
      for (int index2 = 0; index2 < barcodeColumns; ++index2)
      {
        if (this.pdf417Matrix[index1, index2])
        {
          if (flag)
            template.Graphics.DrawRectangle(brush2, num4 + this.QuietZoneNew, num3 + this.QuietZoneNew, this.barWidthPixel * num1, this.RowHeight * num2);
          else
            graphics.DrawRectangle(brush2, num4 + location.X + this.QuietZoneNew, num3 + location.Y + this.QuietZoneNew, this.barWidthPixel * num1, this.RowHeight * num2);
        }
        num4 += this.barWidthPixel * num1;
      }
      num4 = this.QuietZone.IsAll || (double) this.QuietZone.Left <= 0.0 ? 0.0f : this.QuietZone.Left;
      num3 += this.RowHeight * num2;
    }
    if (!flag)
      return;
    page.Graphics.DrawPdfTemplate(template, location);
  }

  public void Draw(PdfPageBase page, float x, float y, float width, float height)
  {
    this.Size = new SizeF(width, height);
    this.Location = new PointF(x, y);
    this.Draw(page);
  }

  public void Draw(PdfPageBase page, PointF location, SizeF size)
  {
    this.Draw(page, location.X, location.Y, size.Width, size.Height);
  }

  public void Draw(PdfPageBase page, RectangleF rectangle)
  {
    this.Draw(page, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
  }

  public override Image ToImage()
  {
    byte[] byteData = !string.IsNullOrEmpty(this.Text) ? Encoding.UTF8.GetBytes(this.Text) : throw new PdfBarcodeException("Input data is empty");
    this.QuietZoneNew = this.GetQuiteZone();
    this.EncodeTextData(byteData);
    if (this.pdf417Matrix == null)
      this.pdf417Matrix = this.Create417BarcodeMatrix();
    int barcodeColumns = this.BarcodeColumns;
    int dataRows = this.DataRows;
    Brush brush1 = Brushes.White;
    Brush brush2 = Brushes.Black;
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
    float num1 = 1f;
    float num2 = 1f;
    if (base.Size != SizeF.Empty)
    {
      num1 = base.Size.Width / this.BarcodeWidth;
      num2 = base.Size.Height / this.BarcodeHeight;
    }
    Bitmap image = new Bitmap((int) ((double) this.BarcodeWidth * (double) num1), (int) ((double) this.BarcodeHeight * (double) num2));
    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) image);
    graphics.FillRectangle(brush1, 0.0f, 0.0f, this.BarcodeWidth * num1, this.BarcodeHeight * num2);
    float num3 = 0.0f;
    float num4 = (float) (0.0 + (this.QuietZone.IsAll || (double) this.QuietZone.Top <= 0.0 ? 0.0 : (double) this.QuietZone.Top));
    float num5 = num3 + (this.QuietZone.IsAll || (double) this.QuietZone.Left <= 0.0 ? 0.0f : this.QuietZone.Left);
    for (int index1 = 0; index1 < dataRows; ++index1)
    {
      for (int index2 = 0; index2 < barcodeColumns; ++index2)
      {
        if (this.pdf417Matrix[index1, index2])
          graphics.FillRectangle(brush2, num5 + this.QuietZoneNew, num4 + this.QuietZoneNew, this.barWidthPixel * num1, this.RowHeight * num2);
        num5 += this.barWidthPixel * num1;
      }
      num5 = this.QuietZone.IsAll || (double) this.QuietZone.Left <= 0.0 ? 0.0f : this.QuietZone.Left;
      num4 += this.RowHeight * num2;
    }
    return (Image) image;
  }

  public Image ToImage(SizeF size)
  {
    this.Size = size;
    return this.ToImage();
  }

  private SizeF GetBarcodeSize()
  {
    byte[] bytes = Encoding.UTF8.GetBytes(this.Text);
    float quiteZone = this.GetQuiteZone();
    float num1 = (float) ((double) this.barWidthPixel * (double) (int) this.XDimension / 2.0);
    float num2 = (float) ((double) this.rowHeightPixel * (double) (int) this.XDimension / 2.0);
    this.EncodeTextData(bytes);
    return new SizeF((float) ((double) num1 * (double) this.BarcodeColumns + 2.0 * (double) quiteZone + (this.QuietZone.IsAll || (int) this.QuietZone.Left <= 0 ? 0.0 : (double) (int) this.QuietZone.Left) + (this.QuietZone.IsAll || (int) this.QuietZone.Right <= 0 ? 0.0 : (double) (int) this.QuietZone.Right)), (float) ((double) num2 * (double) this.DataRows + 2.0 * (double) quiteZone + (this.QuietZone.IsAll || (int) this.QuietZone.Top <= 0 ? 0.0 : (double) (int) this.QuietZone.Top) + (this.QuietZone.IsAll || (int) this.QuietZone.Bottom <= 0 ? 0.0 : (double) (int) this.QuietZone.Bottom)));
  }

  internal float GetQuiteZone()
  {
    float quiteZone = 2f;
    if (this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0)
      quiteZone = this.QuietZone.All;
    return quiteZone;
  }

  internal bool[,] Create417BarcodeMatrix()
  {
    int[] Codewords = new int[this.DataRows * this.DataColumns];
    int count = this.codeWordsList.Count;
    for (int index = 0; index < count; ++index)
      Codewords[index] = this.codeWordsList[index];
    int num1 = this.DataRows * this.DataColumns - this.codeWordsList.Count - this.errorCorrectionLength;
    for (int index = 0; index < num1; ++index)
      Codewords[count++] = 900;
    Codewords[0] = count;
    this.CalculateErrorCorrection(Codewords);
    bool[,] Matrix = new bool[this.DataRows, this.BarcodeColumns];
    int num2 = this.DataRows - 1;
    int num3 = this.BarcodeColumns - this.StopPattern.Length;
    int Col = num3 - 17;
    for (int Row = 0; Row < this.DataRows; ++Row)
    {
      for (int index = 0; index < this.StartPattern.Length; ++index)
        Matrix[Row, index] = this.StartPattern[index];
      for (int index = 0; index < this.StopPattern.Length; ++index)
        Matrix[Row, num3 + index] = this.StopPattern[index];
      int num4 = 30 * (Row / 3);
      int num5 = num4;
      int Codeword1;
      int Codeword2;
      switch (Row % 3)
      {
        case 0:
          Codeword1 = num4 + num2 / 3;
          Codeword2 = num5 + (this.DataColumns - 1);
          break;
        case 1:
          Codeword1 = num4 + ((int) this.ErrorCorrectionLevel * 3 + num2 % 3);
          Codeword2 = num5 + num2 / 3;
          break;
        default:
          Codeword1 = num4 + (this.DataColumns - 1);
          Codeword2 = num5 + ((int) this.ErrorCorrectionLevel * 3 + num2 % 3);
          break;
      }
      this.CodewordToModules(Row, this.StartPattern.Length, Codeword1, Matrix);
      this.CodewordToModules(Row, Col, Codeword2, Matrix);
      for (int index1 = 0; index1 < this.DataColumns; ++index1)
      {
        int index2 = this.DataColumns * Row + index1;
        this.CodewordToModules(Row, 17 * (index1 + 2), Codewords[index2], Matrix);
      }
    }
    this.pdf417Matrix = Matrix;
    return Matrix;
  }

  internal void CodewordToModules(int Row, int Col, int Codeword, bool[,] Matrix)
  {
    Matrix[Row, Col] = true;
    int num1 = (int) Pdf417Tables.CodewordTable[Row % 3, Codeword];
    int num2 = 16384 /*0x4000*/;
    for (int index = 1; index < 17; ++index)
    {
      if ((num1 & num2) != 0)
        Matrix[Row, Col + index] = true;
      num2 >>= 1;
    }
  }

  internal void CalculateErrorCorrection(int[] Codewords)
  {
    int[] errorCorrectionTable = Pdf417Tables.ErrorCorrectionTables[(int) this.ErrorCorrectionLevel];
    this.errorCorrectionCodewords = new int[this.errorCorrectionLength];
    int index1 = this.errorCorrectionLength - 1;
    int codeword = Codewords[0];
    for (int index2 = 0; index2 < codeword; ++index2)
    {
      int num = (Codewords[index2] + this.errorCorrectionCodewords[index1]) % 929;
      for (int index3 = index1; index3 > 0; --index3)
        this.errorCorrectionCodewords[index3] = (929 + this.errorCorrectionCodewords[index3 - 1] - num * errorCorrectionTable[index3]) % 929;
      this.errorCorrectionCodewords[0] = (929 - num * errorCorrectionTable[0]) % 929;
    }
    for (int index4 = index1; index4 >= 0; --index4)
      this.errorCorrectionCodewords[index4] = (929 - this.errorCorrectionCodewords[index4]) % 929;
    for (int index5 = 0; index5 < this.errorCorrectionLength; ++index5)
      Codewords[codeword + index5] = this.errorCorrectionCodewords[index1 - index5];
  }

  internal void EncodeTextData(byte[] byteData)
  {
    this.pdf417Matrix = (bool[,]) null;
    this.inputBinaryData = byteData;
    this.DataEncoding();
    this.DetermineCorrectionLevel();
    int num1 = this.defaultDataColumns;
    int num2 = (this.codeWordsList.Count + this.errorCorrectionLength + num1 - 1) / num1;
    if (num2 > 90)
    {
      num2 = 90;
      num1 = (this.codeWordsList.Count + this.errorCorrectionLength + num2 - 1) / num2;
      if (num1 > 30)
        throw new PdfBarcodeException("Data overflow for PDF417Barcode");
    }
    this.DataRows = num2;
    this.DataColumns = num1;
  }

  internal void DetermineCorrectionLevel()
  {
    if (this.ErrorCorrectionLevel >= Pdf417ErrorCorrectionLevel.Level0 && this.ErrorCorrectionLevel <= Pdf417ErrorCorrectionLevel.Level8)
    {
      this.ErrorCorrectionLevel = this.m_errorCorrectionLevel;
    }
    else
    {
      int count = this.codeWordsList.Count;
      this.ErrorCorrectionLevel = count > 40 ? (count > 160 /*0xA0*/ ? (count > 320 ? (count > 863 ? Pdf417ErrorCorrectionLevel.Level6 : Pdf417ErrorCorrectionLevel.Level5) : Pdf417ErrorCorrectionLevel.Level4) : Pdf417ErrorCorrectionLevel.Level3) : Pdf417ErrorCorrectionLevel.Level2;
    }
    this.errorCorrectionLength = 1 << (int) (this.ErrorCorrectionLevel + 1 & (Pdf417ErrorCorrectionLevel) 31 /*0x1F*/);
  }

  private int CountText()
  {
    int num1 = 0;
    int textDataPosition;
    for (textDataPosition = this.textDataPosition; textDataPosition < this.inputDataLength; ++textDataPosition)
    {
      int num2 = (int) this.inputBinaryData[textDataPosition];
      if ((num2 >= 32 /*0x20*/ || num2 == 13 || num2 == 10 || num2 == 9) && num2 <= 126)
        ++num1;
      else
        break;
    }
    return textDataPosition - this.textDataPosition;
  }

  private int CountPunctuation(int CurrentTextCount)
  {
    int num = 0;
    while (CurrentTextCount > 0)
    {
      int index = (int) this.inputBinaryData[this.textDataPosition + num];
      if (Pdf417Tables.TextToPunctuation[index] == (byte) 127 /*0x7F*/)
        return 0;
      ++num;
      if (num == 3)
        return 3;
    }
    return 0;
  }

  private int CountBytes()
  {
    int num1 = 0;
    int textDataPosition;
    for (textDataPosition = this.textDataPosition; textDataPosition < this.inputDataLength; ++textDataPosition)
    {
      int num2 = (int) this.inputBinaryData[textDataPosition];
      if (num2 < 32 /*0x20*/ && num2 != 13 && num2 != 10 && num2 != 9 || num2 > 126)
      {
        num1 = 0;
      }
      else
      {
        ++num1;
        if (num1 >= 5)
        {
          textDataPosition -= 4;
          break;
        }
      }
    }
    return textDataPosition - this.textDataPosition;
  }

  private void EncodeTextSegment(int TotalCount)
  {
    if (this.encodingMode != EncodingMode.Text)
    {
      this.codeWordsList.Add(900);
      this.encodingMode = EncodingMode.Text;
      this.textEncodingMode = TextEncodingMode.Upper;
    }
    List<int> intList = new List<int>();
    while (TotalCount > 0)
    {
      int index = (int) this.inputBinaryData[this.textDataPosition++];
      --TotalCount;
      switch (this.textEncodingMode)
      {
        case TextEncodingMode.Upper:
          int num1 = (int) Pdf417Tables.TextToUpper[index];
          if (num1 != (int) sbyte.MaxValue)
          {
            intList.Add(num1);
            continue;
          }
          int num2 = (int) Pdf417Tables.TextToLower[index];
          if (num2 != (int) sbyte.MaxValue)
          {
            intList.Add(27);
            intList.Add(num2);
            this.textEncodingMode = TextEncodingMode.Lower;
            continue;
          }
          int num3 = (int) Pdf417Tables.TextToMixed[index];
          if (num3 != (int) sbyte.MaxValue)
          {
            intList.Add(28);
            intList.Add(num3);
            this.textEncodingMode = TextEncodingMode.Mixed;
            continue;
          }
          int num4 = (int) Pdf417Tables.TextToPunctuation[index];
          if (num4 != (int) sbyte.MaxValue)
          {
            if (this.CountPunctuation(TotalCount) > 0)
            {
              intList.Add(28);
              intList.Add(25);
              intList.Add(num4);
              this.textEncodingMode = TextEncodingMode.Punctuation;
              continue;
            }
            intList.Add(29);
            intList.Add(num4);
            continue;
          }
          continue;
        case TextEncodingMode.Lower:
          int num5 = (int) Pdf417Tables.TextToLower[index];
          if (num5 != (int) sbyte.MaxValue)
          {
            intList.Add(num5);
            continue;
          }
          int num6 = (int) Pdf417Tables.TextToUpper[index];
          if (num6 != (int) sbyte.MaxValue)
          {
            intList.Add(27);
            intList.Add(num6);
            continue;
          }
          int num7 = (int) Pdf417Tables.TextToMixed[index];
          if (num7 != (int) sbyte.MaxValue)
          {
            intList.Add(28);
            intList.Add(num7);
            this.textEncodingMode = TextEncodingMode.Mixed;
            continue;
          }
          int num8 = (int) Pdf417Tables.TextToPunctuation[index];
          if (num8 != (int) sbyte.MaxValue)
          {
            if (this.CountPunctuation(TotalCount) > 0)
            {
              intList.Add(28);
              intList.Add(25);
              intList.Add(num8);
              this.textEncodingMode = TextEncodingMode.Punctuation;
              continue;
            }
            intList.Add(29);
            intList.Add(num8);
            continue;
          }
          continue;
        case TextEncodingMode.Mixed:
          int num9 = (int) Pdf417Tables.TextToMixed[index];
          if (num9 != (int) sbyte.MaxValue)
          {
            intList.Add(num9);
            continue;
          }
          int num10 = (int) Pdf417Tables.TextToLower[index];
          if (num10 != (int) sbyte.MaxValue)
          {
            intList.Add(27);
            intList.Add(num10);
            this.textEncodingMode = TextEncodingMode.Lower;
            continue;
          }
          int num11 = (int) Pdf417Tables.TextToUpper[index];
          if (num11 != (int) sbyte.MaxValue)
          {
            intList.Add(28);
            intList.Add(num11);
            this.textEncodingMode = TextEncodingMode.Upper;
            continue;
          }
          int num12 = (int) Pdf417Tables.TextToPunctuation[index];
          if (num12 != (int) sbyte.MaxValue)
          {
            if (this.CountPunctuation(TotalCount) > 0)
            {
              intList.Add(25);
              intList.Add(num12);
              this.textEncodingMode = TextEncodingMode.Punctuation;
              continue;
            }
            intList.Add(29);
            intList.Add(num12);
            continue;
          }
          continue;
        case TextEncodingMode.Punctuation:
          int num13 = (int) Pdf417Tables.TextToPunctuation[index];
          if (num13 != (int) sbyte.MaxValue)
          {
            intList.Add(num13);
            continue;
          }
          intList.Add(29);
          this.textEncodingMode = TextEncodingMode.Upper;
          goto case TextEncodingMode.Upper;
        default:
          continue;
      }
    }
    int index1 = intList.Count & -2;
    for (int index2 = 0; index2 < index1; index2 += 2)
      this.codeWordsList.Add(30 * intList[index2] + intList[index2 + 1]);
    if ((intList.Count & 1) == 0)
      return;
    this.codeWordsList.Add(30 * intList[index1] + 29);
  }

  private void EncodeByteSegment(int Count)
  {
    if (Count == 1 && this.encodingMode == EncodingMode.Text)
    {
      this.codeWordsList.Add(913);
      this.codeWordsList.Add((int) this.inputBinaryData[this.textDataPosition++]);
    }
    else
    {
      this.codeWordsList.Add(Count % 6 == 0 ? 924 : 901);
      this.encodingMode = EncodingMode.Byte;
      int num = this.textDataPosition + Count;
      if (Count >= 6)
      {
        while (num - this.textDataPosition >= 6)
        {
          long result = (long) this.inputBinaryData[this.textDataPosition++] << 40 | (long) this.inputBinaryData[this.textDataPosition++] << 32 /*0x20*/ | (long) this.inputBinaryData[this.textDataPosition++] << 24 | (long) this.inputBinaryData[this.textDataPosition++] << 16 /*0x10*/ | (long) this.inputBinaryData[this.textDataPosition++] << 8 | (long) this.inputBinaryData[this.textDataPosition++];
          for (int index = 4; index > 0; --index)
            this.codeWordsList.Add((int) Math.DivRem(result, Pdf417Tables.Factorial[index], out result));
          this.codeWordsList.Add((int) result);
        }
      }
      while (this.textDataPosition < num)
        this.codeWordsList.Add((int) this.inputBinaryData[this.textDataPosition++]);
    }
  }

  private void DataEncoding()
  {
    this.codeWordsList = new List<int>();
    this.codeWordsList.Add(0);
    this.textDataPosition = 0;
    this.inputDataLength = this.inputBinaryData.Length;
    this.encodingMode = EncodingMode.Text;
    this.textEncodingMode = TextEncodingMode.Upper;
    while (this.textDataPosition < this.inputDataLength)
    {
      int TotalCount = this.CountText();
      if (TotalCount >= 5)
        this.EncodeTextSegment(TotalCount);
      else
        this.EncodeByteSegment(this.CountBytes());
    }
  }
}
