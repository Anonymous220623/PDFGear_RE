// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfQRBarcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public class PdfQRBarcode : PdfBidimensionalBarcode
{
  private const int dpi = 96 /*0x60*/;
  private QRCodeVersion version = QRCodeVersion.Version01;
  internal int noOfModules = 21;
  internal ModuleValue[,] moduleValue;
  internal ModuleValue[,] dataAllocationValues;
  private bool isMixMode;
  private bool mixVersionERC = true;
  private string mixExecutablePart;
  private string mixRemainingPart;
  private int totalBits;
  private int mixDataCount;
  private List<string> text = new List<string>();
  private List<InputMode> mode = new List<InputMode>();
  internal bool isXdimension;
  private InputMode inputMode;
  private PdfErrorCorrectionLevel errorCorrectionLevel = PdfErrorCorrectionLevel.Low;
  private int dataBits;
  private int[] blocks;
  private Bitmap image;
  private bool isUserMentionedMode;
  private bool isUserMentionedVersion;
  private bool isUserMentionedErrorCorrectionLevel;
  private bool isEci;
  private int eciAssignmentNumber = 3;
  private PdfQRBarcodeValues qrBarcodeValues;
  private int defaultQuiteZone = 2;
  private bool chooseDefaultMode;

  public PdfQRBarcode()
  {
    this.XDimension = 1f;
    this.QuietZone.All = 2f;
  }

  public QRCodeVersion Version
  {
    get => this.version;
    set
    {
      this.version = value;
      this.noOfModules = (int) (this.version - 1) * 4 + 21;
      if (value == QRCodeVersion.Auto)
        return;
      this.isUserMentionedVersion = true;
    }
  }

  public PdfErrorCorrectionLevel ErrorCorrectionLevel
  {
    get => this.errorCorrectionLevel;
    set
    {
      this.errorCorrectionLevel = value;
      this.isUserMentionedErrorCorrectionLevel = true;
    }
  }

  public InputMode InputMode
  {
    get => this.inputMode;
    set
    {
      this.inputMode = value;
      this.isUserMentionedMode = true;
    }
  }

  public override SizeF Size
  {
    get => base.Size.IsEmpty ? this.GetBarcodeSize() : base.Size;
    set => base.Size = value;
  }

  public override Image ToImage()
  {
    this.GenerateValues();
    float num1 = new PdfUnitConvertor().ConvertToPixels(this.XDimension, PdfGraphicsUnit.Point);
    int quiteZone = this.GetQuiteZone();
    float num2 = 0.0f;
    float num3 = 0.0f;
    SizeF sizeF = SizeF.Empty;
    float num4;
    float num5;
    if (base.Size != SizeF.Empty)
    {
      sizeF = this.DrawBarcode(this.Size);
      num4 = this.Size.Width;
      num5 = this.Size.Height;
      if ((double) num4 <= (double) num5)
      {
        num1 = num4 / (float) (this.noOfModules + 2 * quiteZone);
        num2 = 0.0f;
        num3 = (float) (((double) num5 - (double) sizeF.Height) / 2.0);
      }
      else
      {
        num1 = num5 / (float) (this.noOfModules + 2 * quiteZone);
        num2 = (float) (((double) num4 - (double) sizeF.Width) / 2.0);
        num3 = 0.0f;
      }
    }
    else
    {
      this.isXdimension = true;
      num4 = (float) (this.noOfModules + 2 * quiteZone) * this.XDimension;
      num5 = (float) (this.noOfModules + 2 * quiteZone) * this.XDimension;
    }
    float num6 = num2 + (this.QuietZone.IsAll || (int) this.QuietZone.Left <= 0 ? 0.0f : (float) (int) this.QuietZone.Left);
    float num7 = num3 + (this.QuietZone.IsAll || (int) this.QuietZone.Top <= 0 ? 0.0f : (float) (int) this.QuietZone.Top);
    Bitmap image = new Bitmap((int) num4 + (int) num6, (int) num5 + (int) num7, PixelFormat.Format32bppRgb);
    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) image))
    {
      graphics.Clear(Color.White);
      Brush brush1 = Brushes.White;
      Brush brush2 = Brushes.Black;
      int num8 = this.noOfModules + 2 * quiteZone;
      int num9 = this.noOfModules + 2 * quiteZone;
      float y = num3 + (this.QuietZone.IsAll || (int) this.QuietZone.Top <= 0 ? 0.0f : (float) (int) this.QuietZone.Top);
      for (int index1 = 0; index1 < num8; ++index1)
      {
        float x = num2 + (this.QuietZone.IsAll || (int) this.QuietZone.Left <= 0 ? 0.0f : (float) (int) this.QuietZone.Left);
        for (int index2 = 0; index2 < num9; ++index2)
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
          Brush brush3 = !this.moduleValue[index1, index2].IsBlack ? brush1 : brush2;
          if (this.dataAllocationValues[index2, index1].IsFilled && this.dataAllocationValues[index2, index1].IsBlack)
            brush3 = brush2;
          graphics.FillRectangle(brush3, x, y, num1, num1);
          if (this.isXdimension)
            x += this.XDimension;
          else
            x += num1;
        }
        if (this.isXdimension)
          y += this.XDimension;
        else
          y += num1;
        num2 = !(base.Size != SizeF.Empty) ? 0.0f : ((double) num4 > (double) num5 ? (float) (((double) num4 - (double) sizeF.Width) / 2.0) : 0.0f);
      }
    }
    return (Image) image;
  }

  public Image ToImage(SizeF size)
  {
    bool flag = !(size == SizeF.Empty);
    this.GenerateValues();
    float xdimension = this.XDimension;
    PdfBarcodeQuietZones quietZone = this.QuietZone;
    int noOfModules = this.noOfModules;
    ModuleValue[,] moduleValue = this.moduleValue;
    PdfColor backColor = this.BackColor;
    ModuleValue[,] allocationValues = this.dataAllocationValues;
    bool isXdimension = this.isXdimension;
    PdfColor foreColor = this.ForeColor;
    float num1 = new PdfUnitConvertor().ConvertToPixels(xdimension, PdfGraphicsUnit.Point);
    int quiteZone = this.GetQuiteZone();
    float num2 = 0.0f;
    float num3 = 0.0f;
    float num4;
    float num5;
    if (flag)
    {
      num4 = size.Width;
      num5 = size.Height;
      num1 = (double) num4 > (double) num5 ? num5 / (float) (noOfModules + 2 * quiteZone) : num4 / (float) (noOfModules + 2 * quiteZone);
    }
    else
    {
      num4 = (float) (noOfModules + 2 * quiteZone) * num1;
      num5 = (float) (noOfModules + 2 * quiteZone) * num1;
    }
    float num6 = num2 + (quietZone.IsAll || (int) quietZone.Left <= 0 ? 0.0f : (float) (int) quietZone.Left);
    float num7 = num3 + (quietZone.IsAll || (int) quietZone.Top <= 0 ? 0.0f : (float) (int) quietZone.Top);
    Bitmap image = new Bitmap((int) num4 + (int) num6, (int) num5 + (int) num7, PixelFormat.Format32bppRgb);
    using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) image))
    {
      graphics.Clear(Color.White);
      Brush brush1 = Brushes.White;
      Brush brush2 = Brushes.Black;
      float y = num3 + (quietZone.IsAll || (int) quietZone.Top <= 0 ? 0.0f : (float) (int) quietZone.Top);
      int num8 = noOfModules + 2 * quiteZone;
      int num9 = noOfModules + 2 * quiteZone;
      float width = num4 / (float) num8;
      float height = num5 / (float) num9;
      for (int index1 = 0; index1 < num8; ++index1)
      {
        float x = num2 + (quietZone.IsAll || (int) quietZone.Left <= 0 ? 0.0f : (float) (int) quietZone.Left);
        for (int index2 = 0; index2 < num9; ++index2)
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
          Brush brush3 = !moduleValue[index1, index2].IsBlack ? brush1 : brush2;
          if (allocationValues[index2, index1].IsFilled && allocationValues[index2, index1].IsBlack)
            brush3 = brush2;
          if (flag)
            graphics.FillRectangle(brush3, x, y, width, height);
          else
            graphics.FillRectangle(brush3, x, y, num1, num1);
          if (isXdimension)
            x += xdimension;
          else if (flag)
            x += width;
          else
            x += num1;
        }
        if (isXdimension)
          y += xdimension;
        else if (flag)
          y += height;
        else
          y += num1;
        num2 = 0.0f;
      }
    }
    return (Image) image;
  }

  private SizeF DrawBarcode(SizeF Size)
  {
    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage((Image) new Bitmap((int) Size.Width, (int) Size.Height));
    this.GenerateValues();
    int quiteZone = this.GetQuiteZone();
    Brush brush1 = Brushes.Black;
    Brush brush2 = Brushes.White;
    if (this.BackColor.A != (byte) 0)
    {
      Color color = Color.FromArgb(this.BackColor.ToArgb());
      if (color != Color.White)
        brush2 = (Brush) new SolidBrush(color);
    }
    if (this.ForeColor.A != (byte) 0)
    {
      Color color = Color.FromArgb(this.ForeColor.ToArgb());
      if (color != Color.Black)
        brush1 = (Brush) new SolidBrush(color);
    }
    float num1 = 0.0f;
    float num2 = 0.0f;
    float width = Size.Width;
    float height = Size.Height;
    float num3 = (double) width > (double) height ? height / (float) (this.noOfModules + 2 * quiteZone) : width / (float) (this.noOfModules + 2 * quiteZone);
    int num4 = this.noOfModules + 2 * quiteZone;
    int num5 = this.noOfModules + 2 * quiteZone;
    for (int index1 = 0; index1 < num4; ++index1)
    {
      num1 = 0.0f;
      for (int index2 = 0; index2 < num5; ++index2)
      {
        Brush brush3 = !this.moduleValue[index1, index2].IsBlack ? brush2 : brush1;
        if (this.dataAllocationValues[index2, index1].IsFilled && this.dataAllocationValues[index2, index1].IsBlack)
          brush3 = brush1;
        graphics.FillRectangle(brush3, num1, num2, num3, num3);
        num1 += num3;
      }
      num2 += num3;
    }
    return new SizeF(num1, num2);
  }

  public override void Draw(PdfPageBase page, PointF location)
  {
    this.GenerateValues();
    int quiteZone = this.GetQuiteZone();
    bool flag = false;
    if (page is PdfPage && (page as PdfPage).Document != null)
      flag = (page as PdfPage).Document.AutoTag;
    PdfBrush pdfBrush1 = PdfBrushes.Black;
    PdfBrush pdfBrush2 = PdfBrushes.White;
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
    float x1 = location.X;
    float y1 = location.Y;
    PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor();
    float num1 = this.XDimension;
    float width;
    float height;
    if (base.Size != SizeF.Empty)
    {
      width = this.Size.Width;
      height = this.Size.Height;
      num1 = (double) width > (double) height ? height / (float) (this.noOfModules + 2 * this.defaultQuiteZone) : width / (float) (this.noOfModules + 2 * this.defaultQuiteZone);
    }
    else
    {
      width = (float) (this.noOfModules + 2 * quiteZone) * num1;
      height = (float) (this.noOfModules + 2 * quiteZone) * num1;
    }
    int num2 = this.noOfModules + 2 * quiteZone;
    int num3 = this.noOfModules + 2 * quiteZone;
    float y2 = location.Y + (this.QuietZone.IsAll || (int) this.QuietZone.Top <= 0 ? 0.0f : (float) (int) this.QuietZone.Top);
    PdfTemplate template = (PdfTemplate) null;
    if (flag)
      template = new PdfTemplate(new SizeF(width, height));
    for (int index1 = 0; index1 < num2; ++index1)
    {
      float x2 = location.X + (this.QuietZone.IsAll || (int) this.QuietZone.Left <= 0 ? 0.0f : (float) (int) this.QuietZone.Left);
      for (int index2 = 0; index2 < num3; ++index2)
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
        PdfBrush brush = !this.moduleValue[index1, index2].IsBlack ? pdfBrush2 : pdfBrush1;
        if (this.dataAllocationValues[index2, index1].IsFilled && this.dataAllocationValues[index2, index1].IsBlack)
          brush = pdfBrush1;
        if (flag)
          template.Graphics.DrawRectangle(brush, x2, y2, num1, num1);
        else
          page.Graphics.DrawRectangle(brush, x2, y2, num1, num1);
        x2 += num1;
      }
      y2 += num1;
    }
    if (!flag)
      return;
    page.Graphics.DrawPdfTemplate(template, location);
  }

  public void Draw(PdfPageBase page, PointF location, SizeF size)
  {
    this.Draw(page, location.X, location.Y, size.Width, size.Height);
  }

  public void Draw(PdfPageBase page, RectangleF rectangle)
  {
    this.Draw(page, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
  }

  public void Draw(PdfPageBase page, float a, float b, float width, float height)
  {
    this.GenerateValues();
    int quiteZone = this.GetQuiteZone();
    bool flag = false;
    if (page is PdfPage && (page as PdfPage).Document != null)
      flag = (page as PdfPage).Document.AutoTag;
    PdfBrush pdfBrush1 = PdfBrushes.Black;
    PdfBrush pdfBrush2 = PdfBrushes.White;
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
    PdfUnitConvertor pdfUnitConvertor = new PdfUnitConvertor();
    double xdimension = (double) this.XDimension;
    int num1 = this.noOfModules + 2 * quiteZone;
    int num2 = this.noOfModules + 2 * quiteZone;
    float width1 = width / (float) (this.noOfModules + 2 * this.defaultQuiteZone);
    float height1 = height / (float) (this.noOfModules + 2 * this.defaultQuiteZone);
    float y = b + (this.QuietZone.IsAll || (int) this.QuietZone.Top <= 0 ? 0.0f : (float) (int) this.QuietZone.Top);
    PdfTemplate template = (PdfTemplate) null;
    if (flag)
      template = new PdfTemplate(new SizeF(width, height));
    for (int index1 = 0; index1 < num1; ++index1)
    {
      float x = a + (this.QuietZone.IsAll || (int) this.QuietZone.Left <= 0 ? 0.0f : (float) (int) this.QuietZone.Left);
      for (int index2 = 0; index2 < num2; ++index2)
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
        PdfBrush brush = !this.moduleValue[index1, index2].IsBlack ? pdfBrush2 : pdfBrush1;
        if (this.dataAllocationValues[index2, index1].IsFilled && this.dataAllocationValues[index2, index1].IsBlack)
          brush = pdfBrush1;
        if (flag)
          template.Graphics.DrawRectangle(brush, x, y, width1, height1);
        else
          page.Graphics.DrawRectangle(brush, x, y, width1, height1);
        x += width1;
      }
      y += height1;
    }
    if (!flag)
      return;
    page.Graphics.DrawPdfTemplate(template, new PointF(a, b));
  }

  public override void Draw(PdfPageBase page) => this.Draw(page, this.Location);

  internal void GenerateValues()
  {
    if (this.inputMode == InputMode.MixingMode)
    {
      this.isMixMode = true;
      this.mixVersionERC = false;
    }
    this.Initialize();
    if (this.isMixMode)
    {
      int num1 = 4;
      int num2 = 10;
      int num3 = 9;
      int num4 = 8;
      int num5 = 10;
      int num6 = 11;
      int num7 = 8;
      while (this.mixExecutablePart != null)
      {
        if (this.inputMode == InputMode.NumericMode)
        {
          if (this.mixExecutablePart.Length % 3 == 0)
            this.totalBits = this.totalBits + num1 + num2 + num5 * (this.mixExecutablePart.Length / 3);
          else if (this.mixExecutablePart.Length % 3 == 1)
            this.totalBits = this.totalBits + num1 + num2 + num5 * (this.mixExecutablePart.Length / 3) + 4;
          else if (this.mixExecutablePart.Length % 3 == 2)
            this.totalBits = this.totalBits + num1 + num2 + num5 * (this.mixExecutablePart.Length / 3) + 7;
        }
        if (this.inputMode == InputMode.AlphaNumericMode)
        {
          if (this.mixExecutablePart.Length % 2 == 0)
            this.totalBits = this.totalBits + num1 + num3 + num6 * (this.mixExecutablePart.Length / 2);
          else if (this.mixExecutablePart.Length % 2 == 1)
            this.totalBits = this.totalBits + num1 + num3 + num6 * (this.mixExecutablePart.Length / 2) + 6;
        }
        if (this.inputMode == InputMode.BinaryMode)
          this.totalBits = this.totalBits + num1 + num4 + num7 * this.mixExecutablePart.Length;
        this.text.Add(this.mixExecutablePart);
        this.mode.Add(this.inputMode);
        if (this.mixRemainingPart == null)
        {
          this.Text = "";
          this.mixExecutablePart = (string) null;
          this.inputMode = InputMode.MixingMode;
          this.mixVersionERC = true;
        }
        else
          this.Text = this.mixRemainingPart;
        this.Initialize();
      }
    }
    this.qrBarcodeValues = new PdfQRBarcodeValues(this.version, this.errorCorrectionLevel);
    this.moduleValue = new ModuleValue[this.noOfModules, this.noOfModules];
    this.DrawPDP(0, 0);
    this.DrawPDP(this.noOfModules - 7, 0);
    this.DrawPDP(0, this.noOfModules - 7);
    this.DrawTimingPattern();
    if (this.version != QRCodeVersion.Version01)
    {
      int[] patternCoOrdinates = this.GetAlignmentPatternCoOrdinates();
      foreach (int x in patternCoOrdinates)
      {
        foreach (int y in patternCoOrdinates)
        {
          if (!this.moduleValue[x, y].IsPDP)
            this.DrawAlignmentPattern(x, y);
        }
      }
    }
    this.AllocateFormatAndVersionInformation();
    bool[] data;
    if (this.isMixMode)
    {
      List<bool> boolList = new List<bool>();
      this.mixDataCount = this.text.Count;
      int index = 0;
      foreach (string str in this.text)
      {
        --this.mixDataCount;
        this.Text = str;
        this.inputMode = this.mode[index];
        foreach (bool flag in this.EncodeData())
          boolList.Add(flag);
        ++index;
      }
      data = boolList.ToArray();
    }
    else
      data = this.EncodeData();
    this.DataAllocationAndMasking(data);
    this.DrawFormatInformation();
    this.AddQuietZone();
  }

  private unsafe void AddQuietZone()
  {
    int quiteZone = this.GetQuiteZone();
    int length1 = this.noOfModules + 2 * quiteZone;
    int length2 = this.noOfModules + 2 * quiteZone;
    ModuleValue[,] moduleValueArray1 = new ModuleValue[length1, length2];
    ModuleValue[,] moduleValueArray2 = new ModuleValue[length1, length2];
    for (int index = 0; index < length2; ++index)
    {
      *(ModuleValue*) ref moduleValueArray1.Address(0, index) = new ModuleValue();
      moduleValueArray1[0, index].IsBlack = false;
      moduleValueArray1[0, index].IsFilled = false;
      moduleValueArray1[0, index].IsPDP = false;
      *(ModuleValue*) ref moduleValueArray2.Address(0, index) = new ModuleValue();
      moduleValueArray2[0, index].IsBlack = false;
      moduleValueArray2[0, index].IsFilled = false;
      moduleValueArray2[0, index].IsPDP = false;
    }
    for (int index1 = quiteZone; index1 < length1 - quiteZone; ++index1)
    {
      *(ModuleValue*) ref moduleValueArray1.Address(index1, 0) = new ModuleValue();
      moduleValueArray1[index1, 0].IsBlack = false;
      moduleValueArray1[index1, 0].IsFilled = false;
      moduleValueArray1[index1, 0].IsPDP = false;
      *(ModuleValue*) ref moduleValueArray2.Address(index1, 0) = new ModuleValue();
      moduleValueArray2[index1, 0].IsBlack = false;
      moduleValueArray2[index1, 0].IsFilled = false;
      moduleValueArray2[index1, 0].IsPDP = false;
      for (int index2 = quiteZone; index2 < length2 - quiteZone; ++index2)
      {
        moduleValueArray1[index1, index2] = this.moduleValue[index1 - quiteZone, index2 - quiteZone];
        moduleValueArray2[index1, index2] = this.dataAllocationValues[index1 - quiteZone, index2 - quiteZone];
      }
      *(ModuleValue*) ref moduleValueArray1.Address(index1, length2 - quiteZone) = new ModuleValue();
      moduleValueArray1[index1, length2 - quiteZone].IsBlack = false;
      moduleValueArray1[index1, length2 - quiteZone].IsFilled = false;
      moduleValueArray1[index1, length2 - quiteZone].IsPDP = false;
      *(ModuleValue*) ref moduleValueArray2.Address(index1, length2 - quiteZone) = new ModuleValue();
      moduleValueArray2[index1, length2 - quiteZone].IsBlack = false;
      moduleValueArray2[index1, length2 - quiteZone].IsFilled = false;
      moduleValueArray2[index1, length2 - quiteZone].IsPDP = false;
    }
    for (int index = 0; index < length2; ++index)
    {
      *(ModuleValue*) ref moduleValueArray1.Address(length1 - quiteZone, index) = new ModuleValue();
      moduleValueArray1[length1 - quiteZone, index].IsBlack = false;
      moduleValueArray1[length1 - quiteZone, index].IsFilled = false;
      moduleValueArray1[length1 - quiteZone, index].IsPDP = false;
      *(ModuleValue*) ref moduleValueArray2.Address(length1 - quiteZone, index) = new ModuleValue();
      moduleValueArray2[length1 - quiteZone, index].IsBlack = false;
      moduleValueArray2[length1 - quiteZone, index].IsFilled = false;
      moduleValueArray2[length1 - quiteZone, index].IsPDP = false;
    }
    this.moduleValue = moduleValueArray1;
    this.dataAllocationValues = moduleValueArray2;
  }

  private void DrawPDP(int x, int y)
  {
    int index1 = x;
    int index2 = y;
    while (index1 < x + 7)
    {
      this.moduleValue[index1, y].IsBlack = true;
      this.moduleValue[index1, y].IsFilled = true;
      this.moduleValue[index1, y].IsPDP = true;
      this.moduleValue[index1, y + 6].IsBlack = true;
      this.moduleValue[index1, y + 6].IsFilled = true;
      this.moduleValue[index1, y + 6].IsPDP = true;
      if (y + 7 < this.noOfModules)
      {
        this.moduleValue[index1, y + 7].IsBlack = false;
        this.moduleValue[index1, y + 7].IsFilled = true;
        this.moduleValue[index1, y + 7].IsPDP = true;
      }
      else if (y - 1 >= 0)
      {
        this.moduleValue[index1, y - 1].IsBlack = false;
        this.moduleValue[index1, y - 1].IsFilled = true;
        this.moduleValue[index1, y - 1].IsPDP = true;
      }
      this.moduleValue[x, index2].IsBlack = true;
      this.moduleValue[x, index2].IsFilled = true;
      this.moduleValue[x, index2].IsPDP = true;
      this.moduleValue[x + 6, index2].IsBlack = true;
      this.moduleValue[x + 6, index2].IsFilled = true;
      this.moduleValue[x + 6, index2].IsPDP = true;
      if (x + 7 < this.noOfModules)
      {
        this.moduleValue[x + 7, index2].IsBlack = false;
        this.moduleValue[x + 7, index2].IsFilled = true;
        this.moduleValue[x + 7, index2].IsPDP = true;
      }
      else if (x - 1 >= 0)
      {
        this.moduleValue[x - 1, index2].IsBlack = false;
        this.moduleValue[x - 1, index2].IsFilled = true;
        this.moduleValue[x - 1, index2].IsPDP = true;
      }
      ++index1;
      ++index2;
    }
    if (x + 7 < this.noOfModules && y + 7 < this.noOfModules)
    {
      this.moduleValue[x + 7, y + 7].IsBlack = false;
      this.moduleValue[x + 7, y + 7].IsFilled = true;
      this.moduleValue[x + 7, y + 7].IsPDP = true;
    }
    else if (x + 7 < this.noOfModules && y + 7 >= this.noOfModules)
    {
      this.moduleValue[x + 7, y - 1].IsBlack = false;
      this.moduleValue[x + 7, y - 1].IsFilled = true;
      this.moduleValue[x + 7, y - 1].IsPDP = true;
    }
    else if (x + 7 >= this.noOfModules && y + 7 < this.noOfModules)
    {
      this.moduleValue[x - 1, y + 7].IsBlack = false;
      this.moduleValue[x - 1, y + 7].IsFilled = true;
      this.moduleValue[x - 1, y + 7].IsPDP = true;
    }
    ++x;
    ++y;
    int index3 = x;
    int index4 = y;
    while (index3 < x + 5)
    {
      this.moduleValue[index3, y].IsBlack = false;
      this.moduleValue[index3, y].IsFilled = true;
      this.moduleValue[index3, y].IsPDP = true;
      this.moduleValue[index3, y + 4].IsBlack = false;
      this.moduleValue[index3, y + 4].IsFilled = true;
      this.moduleValue[index3, y + 4].IsPDP = true;
      this.moduleValue[x, index4].IsBlack = false;
      this.moduleValue[x, index4].IsFilled = true;
      this.moduleValue[x, index4].IsPDP = true;
      this.moduleValue[x + 4, index4].IsBlack = false;
      this.moduleValue[x + 4, index4].IsFilled = true;
      this.moduleValue[x + 4, index4].IsPDP = true;
      ++index3;
      ++index4;
    }
    ++x;
    ++y;
    int index5 = x;
    int index6 = y;
    while (index5 < x + 3)
    {
      this.moduleValue[index5, y].IsBlack = true;
      this.moduleValue[index5, y].IsFilled = true;
      this.moduleValue[index5, y].IsPDP = true;
      this.moduleValue[index5, y + 2].IsBlack = true;
      this.moduleValue[index5, y + 2].IsFilled = true;
      this.moduleValue[index5, y + 2].IsPDP = true;
      this.moduleValue[x, index6].IsBlack = true;
      this.moduleValue[x, index6].IsFilled = true;
      this.moduleValue[x, index6].IsPDP = true;
      this.moduleValue[x + 2, index6].IsBlack = true;
      this.moduleValue[x + 2, index6].IsFilled = true;
      this.moduleValue[x + 2, index6].IsPDP = true;
      ++index5;
      ++index6;
    }
    this.moduleValue[x + 1, y + 1].IsBlack = true;
    this.moduleValue[x + 1, y + 1].IsFilled = true;
    this.moduleValue[x + 1, y + 1].IsPDP = true;
  }

  private void DrawTimingPattern()
  {
    for (int index = 8; index < this.noOfModules - 8; index += 2)
    {
      this.moduleValue[index, 6].IsBlack = true;
      this.moduleValue[index, 6].IsFilled = true;
      this.moduleValue[index + 1, 6].IsBlack = false;
      this.moduleValue[index + 1, 6].IsFilled = true;
      this.moduleValue[6, index].IsBlack = true;
      this.moduleValue[6, index].IsFilled = true;
      this.moduleValue[6, index + 1].IsBlack = false;
      this.moduleValue[6, index + 1].IsFilled = true;
    }
    this.moduleValue[this.noOfModules - 8, 8].IsBlack = true;
    this.moduleValue[this.noOfModules - 8, 8].IsFilled = true;
  }

  private void DrawAlignmentPattern(int x, int y)
  {
    int index1 = x - 2;
    int index2 = y - 2;
    while (index1 < x + 3)
    {
      this.moduleValue[index1, y - 2].IsBlack = true;
      this.moduleValue[index1, y - 2].IsFilled = true;
      this.moduleValue[index1, y + 2].IsBlack = true;
      this.moduleValue[index1, y + 2].IsFilled = true;
      this.moduleValue[x - 2, index2].IsBlack = true;
      this.moduleValue[x - 2, index2].IsFilled = true;
      this.moduleValue[x + 2, index2].IsBlack = true;
      this.moduleValue[x + 2, index2].IsFilled = true;
      ++index1;
      ++index2;
    }
    int index3 = x - 1;
    int index4 = y - 1;
    while (index3 < x + 2)
    {
      this.moduleValue[index3, y - 1].IsBlack = false;
      this.moduleValue[index3, y - 1].IsFilled = true;
      this.moduleValue[index3, y + 1].IsBlack = false;
      this.moduleValue[index3, y + 1].IsFilled = true;
      this.moduleValue[x - 1, index4].IsBlack = false;
      this.moduleValue[x - 1, index4].IsFilled = true;
      this.moduleValue[x + 1, index4].IsBlack = false;
      this.moduleValue[x + 1, index4].IsFilled = true;
      ++index3;
      ++index4;
    }
    this.moduleValue[x, y].IsBlack = true;
    this.moduleValue[x, y].IsFilled = true;
  }

  private bool[] EncodeData()
  {
    List<bool> boolList = new List<bool>();
    switch (this.inputMode)
    {
      case InputMode.NumericMode:
        boolList.Add(false);
        boolList.Add(false);
        boolList.Add(false);
        boolList.Add(true);
        break;
      case InputMode.AlphaNumericMode:
        boolList.Add(false);
        boolList.Add(false);
        boolList.Add(true);
        boolList.Add(false);
        break;
      case InputMode.BinaryMode:
        if (this.isEci)
        {
          boolList.Add(false);
          boolList.Add(true);
          boolList.Add(true);
          boolList.Add(true);
          foreach (bool flag in this.StringToBoolArray(this.eciAssignmentNumber.ToString(), 8))
            boolList.Add(flag);
        }
        boolList.Add(false);
        boolList.Add(true);
        boolList.Add(false);
        boolList.Add(false);
        break;
    }
    int noOfBits = 0;
    if (this.version < QRCodeVersion.Version10)
    {
      switch (this.inputMode)
      {
        case InputMode.NumericMode:
          noOfBits = 10;
          break;
        case InputMode.AlphaNumericMode:
          noOfBits = 9;
          break;
        case InputMode.BinaryMode:
          noOfBits = 8;
          break;
      }
    }
    else if (this.version < QRCodeVersion.Version27)
    {
      switch (this.inputMode)
      {
        case InputMode.NumericMode:
          noOfBits = 12;
          break;
        case InputMode.AlphaNumericMode:
          noOfBits = 11;
          break;
        case InputMode.BinaryMode:
          noOfBits = 16 /*0x10*/;
          break;
      }
    }
    else
    {
      switch (this.inputMode)
      {
        case InputMode.NumericMode:
          noOfBits = 14;
          break;
        case InputMode.AlphaNumericMode:
          noOfBits = 13;
          break;
        case InputMode.BinaryMode:
          noOfBits = 16 /*0x10*/;
          break;
      }
    }
    bool[] boolArray1 = this.IntToBoolArray(Encoding.UTF8.GetBytes(this.Text).Length, noOfBits);
    for (int index = 0; index < noOfBits; ++index)
      boolList.Add(boolArray1[index]);
    if (this.inputMode == InputMode.NumericMode)
    {
      char[] charArray = this.Text.ToCharArray();
      string numberInString = "";
      for (int index = 0; index < charArray.Length; ++index)
      {
        numberInString += (string) (object) charArray[index];
        if (index % 3 == 2 && index != 0 || index == charArray.Length - 1)
        {
          bool[] flagArray = numberInString.ToString().Length != 3 ? (numberInString.ToString().Length != 2 ? this.StringToBoolArray(numberInString, 4) : this.StringToBoolArray(numberInString, 7)) : this.StringToBoolArray(numberInString, 10);
          numberInString = "";
          foreach (bool flag in flagArray)
            boolList.Add(flag);
        }
      }
    }
    else if (this.inputMode == InputMode.AlphaNumericMode)
    {
      char[] charArray = this.Text.ToCharArray();
      string str = "";
      int num = 0;
      for (int index = 0; index < charArray.Length; ++index)
      {
        str += (string) (object) charArray[index];
        if (index % 2 == 0 && index + 1 != charArray.Length)
          num = 45 * this.qrBarcodeValues.GetAlphanumericvalues(charArray[index]);
        if (index % 2 == 1 && index != 0)
        {
          bool[] boolArray2 = this.IntToBoolArray(num + this.qrBarcodeValues.GetAlphanumericvalues(charArray[index]), 11);
          num = 0;
          foreach (bool flag in boolArray2)
            boolList.Add(flag);
          str = (string) null;
        }
        if (index != 1 && str != null && index + 1 == charArray.Length && str.Length == 1)
        {
          bool[] boolArray3 = this.IntToBoolArray(this.qrBarcodeValues.GetAlphanumericvalues(charArray[index]), 6);
          num = 0;
          foreach (bool flag in boolArray3)
            boolList.Add(flag);
        }
      }
    }
    else if (this.inputMode == InputMode.BinaryMode)
    {
      char[] charArray = this.Text.ToCharArray();
      for (int index = 0; index < charArray.Length; ++index)
      {
        int number1 = 0;
        bool flag1 = false;
        if (this.Text[index] >= ' ' && this.Text[index] <= '~' || this.Text[index] >= '¡' && this.Text[index] <= 'ÿ' || this.Text[index] == '\n' || this.Text[index] == '\r')
          number1 = (int) charArray[index];
        else if (this.Text[index] >= '｡' && this.Text[index] <= 'ﾟ')
          number1 = (int) charArray[index] - 65216;
        else if (this.Text[index] >= 'Ё' && this.Text[index] <= 'џ')
          number1 = (int) charArray[index] - 864;
        else if (this.Text[index] >= 'Ą' && this.Text[index] <= 'ž')
          number1 = (int) Encoding.GetEncoding("ISO-8859-2").GetBytes(this.Text[index].ToString())[0];
        else if (this.Text[index] < 'd')
        {
          number1 = (int) Encoding.GetEncoding("ISO-8859-1").GetBytes(this.Text[index].ToString())[0];
        }
        else
        {
          this.Text[index].ToString();
          flag1 = true;
          foreach (int number2 in Encoding.UTF8.GetBytes(this.Text[index].ToString()))
          {
            foreach (bool flag2 in this.IntToBoolArray(number2, 8))
              boolList.Add(flag2);
          }
        }
        if (!flag1)
        {
          foreach (bool flag3 in this.IntToBoolArray(number1, 8))
            boolList.Add(flag3);
        }
      }
    }
    if (this.isMixMode && this.mixDataCount == 0 || !this.isMixMode)
    {
      for (int index = 0; index < 4 && boolList.Count / 8 != this.qrBarcodeValues.NumberOfDataCodeWord; ++index)
        boolList.Add(false);
      while (boolList.Count % 8 != 0)
        boolList.Add(false);
      while (boolList.Count / 8 != this.qrBarcodeValues.NumberOfDataCodeWord)
      {
        boolList.Add(true);
        boolList.Add(true);
        boolList.Add(true);
        boolList.Add(false);
        boolList.Add(true);
        boolList.Add(true);
        boolList.Add(false);
        boolList.Add(false);
        if (boolList.Count / 8 != this.qrBarcodeValues.NumberOfDataCodeWord)
        {
          boolList.Add(false);
          boolList.Add(false);
          boolList.Add(false);
          boolList.Add(true);
          boolList.Add(false);
          boolList.Add(false);
          boolList.Add(false);
          boolList.Add(true);
        }
        else
          break;
      }
      this.dataBits = this.qrBarcodeValues.NumberOfDataCodeWord;
      this.blocks = this.qrBarcodeValues.NumberOfErrorCorrectionBlocks;
      int length = this.blocks[0];
      if (this.blocks.Length == 6)
        length = this.blocks[0] + this.blocks[3];
      string[][] strArray1 = new string[length][];
      List<bool> encodeData1 = boolList;
      if (this.blocks.Length == 6)
      {
        int num = this.blocks[0] * this.blocks[2] * 8;
        encodeData1 = new List<bool>();
        for (int index = 0; index < num; ++index)
          encodeData1.Add(boolList[index]);
      }
      string[,] strArray2 = new string[this.blocks[0], encodeData1.Count / 8 / this.blocks[0]];
      string[,] blocks1 = this.CreateBlocks(encodeData1, this.blocks[0]);
      for (int block = 0; block < this.blocks[0]; ++block)
        strArray1[block] = this.SplitCodeWord(blocks1, block, encodeData1.Count / 8 / this.blocks[0]);
      if (this.blocks.Length == 6)
      {
        List<bool> encodeData2 = new List<bool>();
        for (int index = this.blocks[0] * this.blocks[2] * 8; index < boolList.Count; ++index)
          encodeData2.Add(boolList[index]);
        string[,] strArray3 = new string[this.blocks[0], encodeData2.Count / 8 / this.blocks[3]];
        string[,] blocks2 = this.CreateBlocks(encodeData2, this.blocks[3]);
        int block = this.blocks[0];
        int num = 0;
        for (; block < length; ++block)
          strArray1[block] = this.SplitCodeWord(blocks2, num++, encodeData2.Count / 8 / this.blocks[3]);
      }
      boolList = new List<bool>();
      for (int index1 = 0; index1 < 125; ++index1)
      {
        for (int index2 = 0; index2 < length; ++index2)
        {
          for (int index3 = 0; index3 < 8; ++index3)
          {
            if (index1 < strArray1[index2].Length)
              boolList.Add(strArray1[index2][index1][index3] == '1');
          }
        }
      }
      PdfErrorCorrectionCodewords correctionCodewords = new PdfErrorCorrectionCodewords(this.version, this.errorCorrectionLevel);
      this.dataBits = this.qrBarcodeValues.NumberOfDataCodeWord;
      int correctingCodeWords = this.qrBarcodeValues.NumberOfErrorCorrectingCodeWords;
      this.blocks = this.qrBarcodeValues.NumberOfErrorCorrectionBlocks;
      correctionCodewords.DataBits = this.blocks.Length != 6 ? this.dataBits / this.blocks[0] : (this.dataBits - this.blocks[3] * this.blocks[5]) / this.blocks[0];
      correctionCodewords.ECCW = correctingCodeWords / length;
      string[][] strArray4 = new string[length][];
      int index4 = 0;
      for (int index5 = 0; index5 < this.blocks[0]; ++index5)
      {
        correctionCodewords.DC = strArray1[index4];
        strArray4[index4++] = correctionCodewords.GetERCW();
      }
      if (this.blocks.Length == 6)
      {
        correctionCodewords.DataBits = (this.dataBits - this.blocks[0] * this.blocks[2]) / this.blocks[3];
        for (int index6 = 0; index6 < this.blocks[3]; ++index6)
        {
          correctionCodewords.DC = strArray1[index4];
          strArray4[index4++] = correctionCodewords.GetERCW();
        }
      }
      if (this.blocks.Length != 6)
      {
        for (int index7 = 0; index7 < strArray4[0].Length; ++index7)
        {
          for (int index8 = 0; index8 < this.blocks[0]; ++index8)
          {
            for (int index9 = 0; index9 < 8; ++index9)
            {
              if (index7 < strArray4[index8].Length)
                boolList.Add(strArray4[index8][index7][index9] == '1');
            }
          }
        }
      }
      else
      {
        for (int index10 = 0; index10 < strArray4[0].Length; ++index10)
        {
          for (int index11 = 0; index11 < length; ++index11)
          {
            for (int index12 = 0; index12 < 8; ++index12)
            {
              if (index10 < strArray4[index11].Length)
                boolList.Add(strArray4[index11][index10][index12] == '1');
            }
          }
        }
      }
    }
    return boolList.ToArray();
  }

  private void DataAllocationAndMasking(bool[] data)
  {
    this.dataAllocationValues = new ModuleValue[this.noOfModules, this.noOfModules];
    int num = 0;
    int index1;
    for (int index2 = this.noOfModules - 1; index2 >= 0; index2 = index1 - 2)
    {
      for (int index3 = this.noOfModules - 1; index3 >= 0; --index3)
      {
        if (!this.moduleValue[index2, index3].IsFilled || !this.moduleValue[index2 - 1, index3].IsFilled)
        {
          if (!this.moduleValue[index2, index3].IsFilled)
          {
            if (num + 1 < data.Length)
              this.dataAllocationValues[index2, index3].IsBlack = data[num++];
            if ((index2 + index3) % 3 == 0)
            {
              if (this.dataAllocationValues[index2, index3].IsBlack)
                this.dataAllocationValues[index2, index3].IsBlack = true;
              else
                this.dataAllocationValues[index2, index3].IsBlack = false;
            }
            else if (this.dataAllocationValues[index2, index3].IsBlack)
              this.dataAllocationValues[index2, index3].IsBlack = false;
            else
              this.dataAllocationValues[index2, index3].IsBlack = true;
            this.dataAllocationValues[index2, index3].IsFilled = true;
          }
          if (!this.moduleValue[index2 - 1, index3].IsFilled)
          {
            if (num + 1 < data.Length)
              this.dataAllocationValues[index2 - 1, index3].IsBlack = data[num++];
            if ((index2 - 1 + index3) % 3 == 0)
            {
              if (this.dataAllocationValues[index2 - 1, index3].IsBlack)
                this.dataAllocationValues[index2 - 1, index3].IsBlack = true;
              else
                this.dataAllocationValues[index2 - 1, index3].IsBlack = false;
            }
            else if (this.dataAllocationValues[index2 - 1, index3].IsBlack)
              this.dataAllocationValues[index2 - 1, index3].IsBlack = false;
            else
              this.dataAllocationValues[index2 - 1, index3].IsBlack = true;
            this.dataAllocationValues[index2 - 1, index3].IsFilled = true;
          }
        }
      }
      index1 = index2 - 2;
      if (index1 == 6)
        --index1;
      for (int index4 = 0; index4 < this.noOfModules; ++index4)
      {
        if (!this.moduleValue[index1, index4].IsFilled || !this.moduleValue[index1 - 1, index4].IsFilled)
        {
          if (!this.moduleValue[index1, index4].IsFilled)
          {
            if (num + 1 < data.Length)
              this.dataAllocationValues[index1, index4].IsBlack = data[num++];
            if ((index1 + index4) % 3 == 0)
            {
              if (this.dataAllocationValues[index1, index4].IsBlack)
                this.dataAllocationValues[index1, index4].IsBlack = true;
              else
                this.dataAllocationValues[index1, index4].IsBlack = false;
            }
            else if (this.dataAllocationValues[index1, index4].IsBlack)
              this.dataAllocationValues[index1, index4].IsBlack = false;
            else
              this.dataAllocationValues[index1, index4].IsBlack = true;
            this.dataAllocationValues[index1, index4].IsFilled = true;
          }
          if (!this.moduleValue[index1 - 1, index4].IsFilled)
          {
            if (num + 1 < data.Length)
              this.dataAllocationValues[index1 - 1, index4].IsBlack = data[num++];
            if ((index1 - 1 + index4) % 3 == 0)
            {
              if (this.dataAllocationValues[index1 - 1, index4].IsBlack)
                this.dataAllocationValues[index1 - 1, index4].IsBlack = true;
              else
                this.dataAllocationValues[index1 - 1, index4].IsBlack = false;
            }
            else if (this.dataAllocationValues[index1 - 1, index4].IsBlack)
              this.dataAllocationValues[index1 - 1, index4].IsBlack = false;
            else
              this.dataAllocationValues[index1 - 1, index4].IsBlack = true;
            this.dataAllocationValues[index1 - 1, index4].IsFilled = true;
          }
        }
      }
    }
    for (int index5 = 0; index5 < this.noOfModules; ++index5)
    {
      for (int index6 = 0; index6 < this.noOfModules; ++index6)
      {
        if (!this.moduleValue[index5, index6].IsFilled)
        {
          if (this.dataAllocationValues[index5, index6].IsBlack)
            this.dataAllocationValues[index5, index6].IsBlack = false;
          else
            this.dataAllocationValues[index5, index6].IsBlack = true;
        }
      }
    }
  }

  private void DrawFormatInformation()
  {
    byte[] formatInformation = this.qrBarcodeValues.FormatInformation;
    int index1 = 0;
    for (int index2 = 0; index2 < 7; ++index2)
    {
      if (index2 == 6)
        this.moduleValue[index2 + 1, 8].IsBlack = formatInformation[index1] == (byte) 1;
      else
        this.moduleValue[index2, 8].IsBlack = formatInformation[index1] == (byte) 1;
      this.moduleValue[8, this.noOfModules - index2 - 1].IsBlack = formatInformation[index1++] == (byte) 1;
    }
    int index3 = 14;
    for (int index4 = 0; index4 < 7; ++index4)
    {
      if (index4 == 6)
        this.moduleValue[8, index4 + 1].IsBlack = formatInformation[index3] == (byte) 1;
      else
        this.moduleValue[8, index4].IsBlack = formatInformation[index3] == (byte) 1;
      this.moduleValue[this.noOfModules - index4 - 1, 8].IsBlack = formatInformation[index3--] == (byte) 1;
    }
    this.moduleValue[8, 8].IsBlack = formatInformation[7] == (byte) 1;
    this.moduleValue[8, this.noOfModules - 8].IsBlack = formatInformation[7] == (byte) 1;
  }

  private SizeF GetBarcodeSize()
  {
    int num = 2;
    if (this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0)
      num = (int) this.QuietZone.All;
    return new SizeF((float) (this.noOfModules + 2 * num) * this.XDimension, (float) (this.noOfModules + 2 * num) * this.XDimension);
  }

  private void Initialize()
  {
    if (!this.isUserMentionedMode)
      this.chooseDefaultMode = true;
    InputMode inputMode = InputMode.NumericMode;
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int num4;
    int num5;
    for (int index = 0; index < this.Text.Length; ++index)
    {
      if (this.Text[index] < ':' && this.Text[index] > '/')
      {
        if (this.isMixMode)
        {
          ++num2;
          ++num1;
          if (num1 >= 7 && inputMode == InputMode.BinaryMode)
          {
            num4 = 0;
            num5 = 0;
            break;
          }
          if ((inputMode == InputMode.NumericMode || index == this.Text.Length - 1 && num2 < 10) && inputMode != InputMode.BinaryMode)
          {
            this.mixExecutablePart = this.Text.Substring(0, index + 1);
            this.mixRemainingPart = this.Text.Length == 1 || index == this.Text.Length - 1 ? (string) null : this.Text.Substring(index + 1, this.Text.Length - (index + 1));
          }
        }
      }
      else if (this.Text[index] < '[' && this.Text[index] > '@' || this.Text[index] == '$' || this.Text[index] == '%' || this.Text[index] == '*' || this.Text[index] == '+' || this.Text[index] == '-' || this.Text[index] == '.' || this.Text[index] == '/' || this.Text[index] == ':' || this.Text[index] == ' ')
      {
        if (this.isMixMode)
        {
          if (num2 >= 10)
          {
            num5 = 0;
            num4 = 0;
            break;
          }
          num2 = 0;
          ++num1;
          if (inputMode != InputMode.BinaryMode)
          {
            this.mixExecutablePart = this.Text.Substring(0, index + 1);
            this.mixRemainingPart = this.Text.Length == 1 || index == this.Text.Length - 1 ? (string) null : this.Text.Substring(index + 1, this.Text.Length - (index + 1));
            inputMode = InputMode.AlphaNumericMode;
          }
        }
        else
          inputMode = InputMode.AlphaNumericMode;
      }
      else if (this.Text[index] >= '｡' && this.Text[index] <= 'ﾟ' || this.Text[index] >= 'a' && this.Text[index] <= 'z')
      {
        if (this.isMixMode)
        {
          if (num2 >= 5)
          {
            num5 = 0;
            break;
          }
          if (num1 >= 7)
          {
            num4 = 0;
            this.mixExecutablePart = this.Text.Substring(0, index);
            this.mixRemainingPart = this.Text.Length == 0 || index == this.Text.Length ? (string) null : this.Text.Substring(index, this.Text.Length - index);
            break;
          }
          num2 = 0;
          num1 = 0;
          ++num3;
          this.mixExecutablePart = this.Text.Substring(0, index + 1);
          this.mixRemainingPart = this.Text.Length == 1 || index == this.Text.Length - 1 ? (string) null : this.Text.Substring(index + 1, this.Text.Length - (index + 1));
        }
        inputMode = InputMode.BinaryMode;
        if (!this.isMixMode)
          break;
      }
      else
      {
        if (this.isMixMode)
        {
          if (num2 >= 5)
          {
            num5 = 0;
            break;
          }
          if (num1 >= 7)
          {
            num4 = 0;
            this.mixExecutablePart = this.Text.Substring(0, index);
            this.mixRemainingPart = this.Text.Length == 0 || index == this.Text.Length ? (string) null : this.Text.Substring(index, this.Text.Length - index);
            break;
          }
          num2 = 0;
          num1 = 0;
          ++num3;
          this.mixExecutablePart = this.Text.Substring(0, index + 1);
          this.mixRemainingPart = this.Text.Length == 1 || index == this.Text.Length - 1 ? (string) null : this.Text.Substring(index + 1, this.Text.Length - (index + 1));
        }
        inputMode = InputMode.BinaryMode;
        if (!this.isMixMode)
          break;
      }
    }
    if (this.isUserMentionedMode && !this.isMixMode && inputMode != this.inputMode && ((inputMode == InputMode.AlphaNumericMode || inputMode == InputMode.BinaryMode) && this.inputMode == InputMode.NumericMode || inputMode == InputMode.BinaryMode && this.inputMode == InputMode.AlphaNumericMode))
      throw new PdfBarcodeException("Mode Conflict: Default mode that supports your data is :" + (object) inputMode);
    this.InputMode = inputMode;
    if (this.isEci)
    {
      for (int index = 0; index < this.Text.Length; ++index)
      {
        if (this.Text[index] < ' ' || this.Text[index] > 'ÿ')
        {
          if (this.IsCP437Character(this.Text[index]))
          {
            this.eciAssignmentNumber = 2;
            break;
          }
          if (this.IsISO8859_2Character(this.Text[index]))
          {
            this.eciAssignmentNumber = 4;
            break;
          }
          if (this.IsISO8859_3Character(this.Text[index]))
          {
            this.eciAssignmentNumber = 5;
            break;
          }
          if (this.IsISO8859_4Character(this.Text[index]))
          {
            this.eciAssignmentNumber = 6;
            break;
          }
          if (this.IsISO8859_5Character(this.Text[index]))
          {
            this.eciAssignmentNumber = 7;
            break;
          }
          if (this.IsISO8859_6Character(this.Text[index]))
          {
            this.eciAssignmentNumber = 8;
            break;
          }
          if (this.IsISO8859_7Character(this.Text[index]))
          {
            this.eciAssignmentNumber = 9;
            break;
          }
          if (this.IsISO8859_8Character(this.Text[index]))
          {
            this.eciAssignmentNumber = 10;
            break;
          }
          if (this.IsISO8859_11Character(this.Text[index]))
          {
            this.eciAssignmentNumber = 13;
            break;
          }
          if (this.IsWindows1250Character(this.Text[index]))
          {
            this.eciAssignmentNumber = 21;
            break;
          }
          if (this.IsWindows1251Character(this.Text[index]))
          {
            this.eciAssignmentNumber = 22;
            break;
          }
          if (this.IsWindows1252Character(this.Text[index]))
          {
            this.eciAssignmentNumber = 23;
            break;
          }
          if (this.IsWindows1256Character(this.Text[index]))
          {
            this.eciAssignmentNumber = 24;
            break;
          }
        }
      }
    }
    if (!this.mixVersionERC)
      return;
    if (this.isMixMode)
      this.inputMode = InputMode.MixingMode;
    if (!this.isUserMentionedVersion || this.version == QRCodeVersion.Auto)
    {
      int[] numArray = (int[]) null;
      if (this.isUserMentionedErrorCorrectionLevel)
      {
        switch (this.inputMode)
        {
          case InputMode.NumericMode:
            switch (this.errorCorrectionLevel)
            {
              case PdfErrorCorrectionLevel.Low:
                numArray = PdfQRBarcodeValues.numericDataCapacityLow;
                break;
              case PdfErrorCorrectionLevel.Medium:
                numArray = PdfQRBarcodeValues.numericDataCapacityMedium;
                break;
              case PdfErrorCorrectionLevel.Quartile:
                numArray = PdfQRBarcodeValues.numericDataCapacityQuartile;
                break;
              case PdfErrorCorrectionLevel.High:
                numArray = PdfQRBarcodeValues.numericDataCapacityHigh;
                break;
            }
            break;
          case InputMode.AlphaNumericMode:
            switch (this.errorCorrectionLevel)
            {
              case PdfErrorCorrectionLevel.Low:
                numArray = PdfQRBarcodeValues.alphanumericDataCapacityLow;
                break;
              case PdfErrorCorrectionLevel.Medium:
                numArray = PdfQRBarcodeValues.alphanumericDataCapacityMedium;
                break;
              case PdfErrorCorrectionLevel.Quartile:
                numArray = PdfQRBarcodeValues.alphanumericDataCapacityQuartile;
                break;
              case PdfErrorCorrectionLevel.High:
                numArray = PdfQRBarcodeValues.alphanumericDataCapacityHigh;
                break;
            }
            break;
          case InputMode.BinaryMode:
            switch (this.errorCorrectionLevel)
            {
              case PdfErrorCorrectionLevel.Low:
                numArray = PdfQRBarcodeValues.binaryDataCapacityLow;
                break;
              case PdfErrorCorrectionLevel.Medium:
                numArray = PdfQRBarcodeValues.binaryDataCapacityMedium;
                break;
              case PdfErrorCorrectionLevel.Quartile:
                numArray = PdfQRBarcodeValues.binaryDataCapacityQuartile;
                break;
              case PdfErrorCorrectionLevel.High:
                numArray = PdfQRBarcodeValues.binaryDataCapacityHigh;
                break;
            }
            break;
          case InputMode.MixingMode:
            switch (this.errorCorrectionLevel)
            {
              case PdfErrorCorrectionLevel.Low:
                numArray = PdfQRBarcodeValues.mixedDataCapacityLow;
                break;
              case PdfErrorCorrectionLevel.Medium:
                numArray = PdfQRBarcodeValues.mixedDataCapacityMedium;
                break;
              case PdfErrorCorrectionLevel.Quartile:
                numArray = PdfQRBarcodeValues.mixedDataCapacityQuartile;
                break;
              case PdfErrorCorrectionLevel.High:
                numArray = PdfQRBarcodeValues.mixedDataCapacityHigh;
                break;
            }
            break;
        }
      }
      else
      {
        this.errorCorrectionLevel = PdfErrorCorrectionLevel.Medium;
        switch (this.inputMode)
        {
          case InputMode.NumericMode:
            numArray = PdfQRBarcodeValues.numericDataCapacityMedium;
            break;
          case InputMode.AlphaNumericMode:
            numArray = PdfQRBarcodeValues.alphanumericDataCapacityMedium;
            break;
          case InputMode.BinaryMode:
            numArray = PdfQRBarcodeValues.binaryDataCapacityMedium;
            break;
          case InputMode.MixingMode:
            numArray = PdfQRBarcodeValues.mixedDataCapacityMedium;
            break;
        }
      }
      byte[] bytes = Encoding.UTF8.GetBytes(this.Text);
      int index;
      if (!this.isMixMode)
      {
        index = 0;
        while (index < numArray.Length && numArray[index] < bytes.Length)
          ++index;
      }
      else
      {
        index = 0;
        while (index < numArray.Length && numArray[index] <= this.totalBits)
          ++index;
      }
      this.Version = (QRCodeVersion) (index + 1);
      if (this.Version > QRCodeVersion.Version40)
        throw new PdfBarcodeException("Text length is greater than the data capacity of error correction level");
    }
    else
    {
      if (!this.isUserMentionedVersion)
        return;
      if (this.isUserMentionedErrorCorrectionLevel)
      {
        int num6 = 0;
        if (this.inputMode == InputMode.AlphaNumericMode)
          num6 = PdfQRBarcodeValues.GetAlphanumericDataCapacity(this.version, this.errorCorrectionLevel);
        else if (this.inputMode == InputMode.NumericMode)
          num6 = PdfQRBarcodeValues.GetNumericDataCapacity(this.version, this.errorCorrectionLevel);
        if (this.inputMode == InputMode.BinaryMode)
          num6 = PdfQRBarcodeValues.GetBinaryDataCapacity(this.version, this.errorCorrectionLevel);
        if (this.inputMode == InputMode.MixingMode)
          num6 = PdfQRBarcodeValues.GetMixedDataCapacity(this.version, this.errorCorrectionLevel);
        if (num6 < this.Text.Length && this.inputMode != InputMode.MixingMode)
        {
          if (!this.chooseDefaultMode)
            throw new PdfBarcodeException("Text length is greater than the version capacity");
          this.inputMode = InputMode.MixingMode;
          this.isMixMode = true;
          this.mixVersionERC = false;
          this.Initialize();
        }
        else if (num6 < this.totalBits && this.inputMode == InputMode.MixingMode)
          throw new PdfBarcodeException("Text length is greater than the version capacity");
      }
      else
      {
        int num7 = 0;
        int num8 = 0;
        int num9 = 0;
        int num10 = 0;
        if (this.inputMode == InputMode.AlphaNumericMode)
        {
          num7 = PdfQRBarcodeValues.GetAlphanumericDataCapacity(this.version, PdfErrorCorrectionLevel.Low);
          num8 = PdfQRBarcodeValues.GetAlphanumericDataCapacity(this.version, PdfErrorCorrectionLevel.Medium);
          num9 = PdfQRBarcodeValues.GetAlphanumericDataCapacity(this.version, PdfErrorCorrectionLevel.Quartile);
          num10 = PdfQRBarcodeValues.GetAlphanumericDataCapacity(this.version, PdfErrorCorrectionLevel.High);
        }
        else if (this.inputMode == InputMode.NumericMode)
        {
          num7 = PdfQRBarcodeValues.GetNumericDataCapacity(this.version, PdfErrorCorrectionLevel.Low);
          num8 = PdfQRBarcodeValues.GetNumericDataCapacity(this.version, PdfErrorCorrectionLevel.Medium);
          num9 = PdfQRBarcodeValues.GetNumericDataCapacity(this.version, PdfErrorCorrectionLevel.Quartile);
          num10 = PdfQRBarcodeValues.GetNumericDataCapacity(this.version, PdfErrorCorrectionLevel.High);
        }
        else if (this.inputMode == InputMode.BinaryMode)
        {
          num7 = PdfQRBarcodeValues.GetBinaryDataCapacity(this.version, PdfErrorCorrectionLevel.Low);
          num8 = PdfQRBarcodeValues.GetBinaryDataCapacity(this.version, PdfErrorCorrectionLevel.Medium);
          num9 = PdfQRBarcodeValues.GetBinaryDataCapacity(this.version, PdfErrorCorrectionLevel.Quartile);
          num10 = PdfQRBarcodeValues.GetBinaryDataCapacity(this.version, PdfErrorCorrectionLevel.High);
        }
        else if (this.inputMode == InputMode.MixingMode)
        {
          num7 = PdfQRBarcodeValues.GetMixedDataCapacity(this.version, PdfErrorCorrectionLevel.Low);
          num8 = PdfQRBarcodeValues.GetMixedDataCapacity(this.version, PdfErrorCorrectionLevel.Medium);
          num9 = PdfQRBarcodeValues.GetMixedDataCapacity(this.version, PdfErrorCorrectionLevel.Quartile);
          num10 = PdfQRBarcodeValues.GetMixedDataCapacity(this.version, PdfErrorCorrectionLevel.High);
        }
        if (num10 > this.Text.Length)
          this.errorCorrectionLevel = PdfErrorCorrectionLevel.High;
        else if (num9 > this.Text.Length)
          this.errorCorrectionLevel = PdfErrorCorrectionLevel.Quartile;
        else if (num8 > this.Text.Length)
        {
          this.errorCorrectionLevel = PdfErrorCorrectionLevel.Medium;
        }
        else
        {
          if (num7 <= this.Text.Length)
            throw new PdfBarcodeException("Text length is greater than the version capacity");
          this.errorCorrectionLevel = PdfErrorCorrectionLevel.Low;
        }
      }
    }
  }

  private string[] SplitCodeWord(string[,] encodeData, int block, int count)
  {
    string[] strArray = new string[count];
    for (int index = 0; index < count; ++index)
      strArray[index] = encodeData[block, index];
    return strArray;
  }

  private string[,] CreateBlocks(List<bool> encodeData, int noOfBlocks)
  {
    string[,] blocks = new string[noOfBlocks, encodeData.Count / 8 / noOfBlocks];
    string str = (string) null;
    int index1 = 0;
    int index2 = 0;
    int index3 = 0;
    for (; index1 < encodeData.Count; ++index1)
    {
      if (index1 % 8 == 0 && index1 != 0)
      {
        blocks[index3, index2] = str;
        str = (string) null;
        ++index2;
        if (index2 == encodeData.Count / noOfBlocks / 8)
        {
          ++index3;
          index2 = 0;
        }
      }
      str += (string) (object) (encodeData[index1] ? 1 : 0);
    }
    blocks[index3, index2] = str;
    return blocks;
  }

  private bool[] IntToBoolArray(int number, int noOfBits)
  {
    bool[] boolArray = new bool[noOfBits];
    for (int index = 0; index < noOfBits; ++index)
      boolArray[noOfBits - index - 1] = (number >> index & 1) == 1;
    return boolArray;
  }

  private bool[] StringToBoolArray(string numberInString, int noOfBits)
  {
    bool[] boolArray = new bool[noOfBits];
    char[] charArray = numberInString.ToCharArray();
    int num = 0;
    for (int index = 0; index < charArray.Length; ++index)
      num = num * 10 + (int) charArray[index] - 48 /*0x30*/;
    for (int index = 0; index < noOfBits; ++index)
      boolArray[noOfBits - index - 1] = (num >> index & 1) == 1;
    return boolArray;
  }

  private int[] GetAlignmentPatternCoOrdinates()
  {
    int[] patternCoOrdinates = (int[]) null;
    switch (this.version)
    {
      case QRCodeVersion.Version02:
        patternCoOrdinates = new int[2]{ 6, 18 };
        break;
      case QRCodeVersion.Version03:
        patternCoOrdinates = new int[2]{ 6, 22 };
        break;
      case QRCodeVersion.Version04:
        patternCoOrdinates = new int[2]{ 6, 26 };
        break;
      case QRCodeVersion.Version05:
        patternCoOrdinates = new int[2]{ 6, 30 };
        break;
      case QRCodeVersion.Version06:
        patternCoOrdinates = new int[2]{ 6, 34 };
        break;
      case QRCodeVersion.Version07:
        patternCoOrdinates = new int[3]{ 6, 22, 38 };
        break;
      case QRCodeVersion.Version08:
        patternCoOrdinates = new int[3]{ 6, 24, 42 };
        break;
      case QRCodeVersion.Version09:
        patternCoOrdinates = new int[3]{ 6, 26, 46 };
        break;
      case QRCodeVersion.Version10:
        patternCoOrdinates = new int[3]{ 6, 28, 50 };
        break;
      case QRCodeVersion.Version11:
        patternCoOrdinates = new int[3]{ 6, 30, 54 };
        break;
      case QRCodeVersion.Version12:
        patternCoOrdinates = new int[3]
        {
          6,
          32 /*0x20*/,
          58
        };
        break;
      case QRCodeVersion.Version13:
        patternCoOrdinates = new int[3]{ 6, 34, 62 };
        break;
      case QRCodeVersion.Version14:
        patternCoOrdinates = new int[4]{ 6, 26, 46, 66 };
        break;
      case QRCodeVersion.Version15:
        patternCoOrdinates = new int[4]
        {
          6,
          26,
          48 /*0x30*/,
          70
        };
        break;
      case QRCodeVersion.Version16:
        patternCoOrdinates = new int[4]{ 6, 26, 50, 74 };
        break;
      case QRCodeVersion.Version17:
        patternCoOrdinates = new int[4]{ 6, 30, 54, 78 };
        break;
      case QRCodeVersion.Version18:
        patternCoOrdinates = new int[4]{ 6, 30, 56, 82 };
        break;
      case QRCodeVersion.Version19:
        patternCoOrdinates = new int[4]{ 6, 30, 58, 86 };
        break;
      case QRCodeVersion.Version20:
        patternCoOrdinates = new int[4]{ 6, 34, 62, 90 };
        break;
      case QRCodeVersion.Version21:
        patternCoOrdinates = new int[5]{ 6, 28, 50, 72, 94 };
        break;
      case QRCodeVersion.Version22:
        patternCoOrdinates = new int[5]{ 6, 26, 50, 74, 98 };
        break;
      case QRCodeVersion.Version23:
        patternCoOrdinates = new int[5]
        {
          6,
          30,
          54,
          78,
          102
        };
        break;
      case QRCodeVersion.Version24:
        patternCoOrdinates = new int[5]
        {
          6,
          28,
          54,
          80 /*0x50*/,
          106
        };
        break;
      case QRCodeVersion.Version25:
        patternCoOrdinates = new int[5]
        {
          6,
          32 /*0x20*/,
          58,
          84,
          110
        };
        break;
      case QRCodeVersion.Version26:
        patternCoOrdinates = new int[5]
        {
          6,
          30,
          58,
          86,
          114
        };
        break;
      case QRCodeVersion.Version27:
        patternCoOrdinates = new int[5]
        {
          6,
          34,
          62,
          90,
          118
        };
        break;
      case QRCodeVersion.Version28:
        patternCoOrdinates = new int[6]
        {
          6,
          26,
          50,
          74,
          98,
          122
        };
        break;
      case QRCodeVersion.Version29:
        patternCoOrdinates = new int[6]
        {
          6,
          30,
          54,
          78,
          102,
          126
        };
        break;
      case QRCodeVersion.Version30:
        patternCoOrdinates = new int[6]
        {
          6,
          26,
          52,
          78,
          104,
          130
        };
        break;
      case QRCodeVersion.Version31:
        patternCoOrdinates = new int[6]
        {
          6,
          30,
          56,
          82,
          108,
          134
        };
        break;
      case QRCodeVersion.Version32:
        patternCoOrdinates = new int[6]
        {
          6,
          34,
          60,
          86,
          112 /*0x70*/,
          138
        };
        break;
      case QRCodeVersion.Version33:
        patternCoOrdinates = new int[6]
        {
          6,
          30,
          58,
          86,
          114,
          142
        };
        break;
      case QRCodeVersion.Version34:
        patternCoOrdinates = new int[6]
        {
          6,
          34,
          62,
          90,
          118,
          146
        };
        break;
      case QRCodeVersion.Version35:
        patternCoOrdinates = new int[7]
        {
          6,
          30,
          54,
          78,
          102,
          126,
          150
        };
        break;
      case QRCodeVersion.Version36:
        patternCoOrdinates = new int[7]
        {
          6,
          24,
          50,
          76,
          102,
          128 /*0x80*/,
          154
        };
        break;
      case QRCodeVersion.Version37:
        patternCoOrdinates = new int[7]
        {
          6,
          28,
          54,
          80 /*0x50*/,
          106,
          132,
          158
        };
        break;
      case QRCodeVersion.Version38:
        patternCoOrdinates = new int[7]
        {
          6,
          32 /*0x20*/,
          58,
          84,
          110,
          136,
          162
        };
        break;
      case QRCodeVersion.Version39:
        patternCoOrdinates = new int[7]
        {
          6,
          26,
          54,
          82,
          110,
          138,
          166
        };
        break;
      case QRCodeVersion.Version40:
        patternCoOrdinates = new int[7]
        {
          6,
          30,
          58,
          86,
          114,
          142,
          170
        };
        break;
    }
    return patternCoOrdinates;
  }

  private void AllocateFormatAndVersionInformation()
  {
    for (int index = 0; index < 9; ++index)
    {
      this.moduleValue[8, index].IsFilled = true;
      this.moduleValue[index, 8].IsFilled = true;
    }
    for (int index = this.noOfModules - 8; index < this.noOfModules; ++index)
    {
      this.moduleValue[8, index].IsFilled = true;
      this.moduleValue[index, 8].IsFilled = true;
    }
    if (this.version <= QRCodeVersion.Version06)
      return;
    byte[] versionInformation = this.qrBarcodeValues.VersionInformation;
    int index1 = 0;
    for (int index2 = 0; index2 < 6; ++index2)
    {
      for (int index3 = 2; index3 >= 0; --index3)
      {
        this.moduleValue[index2, this.noOfModules - 9 - index3].IsBlack = versionInformation[index1] == (byte) 1;
        this.moduleValue[index2, this.noOfModules - 9 - index3].IsFilled = true;
        this.moduleValue[this.noOfModules - 9 - index3, index2].IsBlack = versionInformation[index1++] == (byte) 1;
        this.moduleValue[this.noOfModules - 9 - index3, index2].IsFilled = true;
      }
    }
  }

  internal int GetQuiteZone()
  {
    int quiteZone = 2;
    if (this.QuietZone.IsAll && (double) this.QuietZone.All > 0.0)
      quiteZone = (int) this.QuietZone.All;
    return quiteZone;
  }

  private bool IsCP437Character(char inputChar)
  {
    return Array.IndexOf<string>(new string[49]
    {
      "2591",
      "2592",
      "2593",
      "2502",
      "2524",
      "2561",
      "2562",
      "2556",
      "2555",
      "2563",
      "2551",
      "2557",
      "255D",
      "255C",
      "255B",
      "2510",
      "2514",
      "2534",
      "252C",
      "251C",
      "2500",
      "253C",
      "255E",
      "255F",
      "255A",
      "2554",
      "2569",
      "2566",
      "2560",
      "2550",
      "256C",
      "2567",
      "2568",
      "2564",
      "2565",
      "2559",
      "2558",
      "2552",
      "2553",
      "256B",
      "256A",
      "2518",
      "250C",
      "2588",
      "2584",
      "258C",
      "2590",
      "2580",
      "25A0"
    }, ((int) inputChar).ToString("X")) > -1;
  }

  private bool IsISO8859_2Character(char inputChar)
  {
    return Array.IndexOf<string>(new string[57]
    {
      "104",
      "2D8",
      "141",
      "13D",
      "15A",
      "160",
      "15E",
      "164",
      "179",
      "17D",
      "17B",
      "105",
      "2DB",
      "142",
      "13E",
      "15B",
      "2C7",
      "161",
      "15F",
      "165",
      "17A",
      "2DD",
      "17E",
      "17C",
      "154",
      "102",
      "139",
      "106",
      "10C",
      "118",
      "11A",
      "10E",
      "110",
      "143",
      "147",
      "150",
      "158",
      "16E",
      "170",
      "162",
      "155",
      "103",
      "13A",
      "107",
      "10D",
      "119",
      "11B",
      "10F",
      "111",
      "144",
      "148",
      "151",
      "159",
      "16F",
      "171",
      "163",
      "2D9"
    }, ((int) inputChar).ToString("X")) > -1;
  }

  private bool IsISO8859_3Character(char inputChar)
  {
    return Array.IndexOf<string>(new string[26]
    {
      "126",
      "124",
      "130",
      "15E",
      "11E",
      "134",
      "17B",
      "127",
      "125",
      "131",
      "15F",
      "11F",
      "135",
      "17C",
      "10A",
      "108",
      "120",
      "11C",
      "16C",
      "15C",
      "10B",
      "109",
      "121",
      "11D",
      "16D",
      "15D"
    }, ((int) inputChar).ToString("X")) > -1;
  }

  private bool IsISO8859_4Character(char inputChar)
  {
    return Array.IndexOf<string>(new string[49]
    {
      "104",
      "138",
      "156",
      "128",
      "13B",
      "160",
      "112",
      "122",
      "166",
      "17D",
      "105",
      "2DB",
      "157",
      "129",
      "13C",
      "2C7",
      "161",
      "113",
      "123",
      "167",
      "14A",
      "17E",
      "14B",
      "100",
      "12E",
      "10C",
      "118",
      "116",
      "12A",
      "110",
      "145",
      "14C",
      "136",
      "172",
      "168",
      "16A",
      "101",
      "12F",
      "10D",
      "119",
      "117",
      "12B",
      "111",
      "146",
      "14D",
      "137",
      "173",
      "169",
      "16B"
    }, ((int) inputChar).ToString("X")) > -1;
  }

  private bool IsISO8859_5Character(char inputChar)
  {
    return inputChar >= 'Ё' && inputChar <= 'џ' && inputChar != 'Ѝ' && inputChar != 'ѐ' && inputChar != 'ѝ';
  }

  private bool IsISO8859_6Character(char inputChar)
  {
    return inputChar >= 'ء' && inputChar <= 'غ' || inputChar >= 'ـ' && inputChar <= 'ْ' || inputChar == '؟' || inputChar == '؛' || inputChar == '،';
  }

  private bool IsISO8859_7Character(char inputChar)
  {
    return inputChar >= '΄' && inputChar <= 'ώ' || inputChar == 'ͺ';
  }

  private bool IsISO8859_8Character(char inputChar) => inputChar >= 'א' && inputChar <= 'ת';

  private bool IsISO8859_11Character(char inputChar) => inputChar >= 'ก' && inputChar <= '๛';

  private bool IsWindows1250Character(char inputChar)
  {
    return Array.IndexOf<string>(new string[10]
    {
      "141",
      "104",
      "15E",
      "17B",
      "142",
      "105",
      "15F",
      "13D",
      "13E",
      "17C"
    }, ((int) inputChar).ToString("X")) > -1 || inputChar >= 'ء' && inputChar <= 'ي';
  }

  private bool IsWindows1251Character(char inputChar)
  {
    return Array.IndexOf<string>(new string[30]
    {
      "402",
      "403",
      "453",
      "409",
      "40A",
      "40C",
      "40B",
      "40F",
      "452",
      "459",
      "45A",
      "45C",
      "45B",
      "45F",
      "40E",
      "45E",
      "408",
      "490",
      "401",
      "404",
      "407",
      "406",
      "456",
      "491",
      "451",
      "454",
      "458",
      "405",
      "455",
      "457"
    }, ((int) inputChar).ToString("X")) > -1 || inputChar >= 'А' && inputChar <= 'я';
  }

  private bool IsWindows1252Character(char inputChar)
  {
    return Array.IndexOf<string>(new string[27]
    {
      "20AC",
      "201A",
      "192",
      "201E",
      "2026",
      "2020",
      "2021",
      "2C6",
      "2030",
      "160",
      "2039",
      "152",
      "17D",
      "2018",
      "2019",
      "201C",
      "201D",
      "2022",
      "2013",
      "2014",
      "2DC",
      "2122",
      "161",
      "203A",
      "153",
      "17E",
      "178"
    }, ((int) inputChar).ToString("X")) > -1;
  }

  private bool IsWindows1256Character(char inputChar)
  {
    return Array.IndexOf<string>(new string[13]
    {
      "67E",
      "679",
      "152",
      "686",
      "698",
      "688",
      "6AF",
      "6A9",
      "691",
      "153",
      "6BA",
      "6BE",
      "6C1"
    }, ((int) inputChar).ToString("X")) > -1 || inputChar >= 'ء' && inputChar <= 'ي';
  }
}
