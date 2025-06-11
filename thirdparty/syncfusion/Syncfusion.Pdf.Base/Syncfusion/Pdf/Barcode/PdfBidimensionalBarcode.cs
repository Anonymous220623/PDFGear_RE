// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Barcode.PdfBidimensionalBarcode
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Barcode;

public abstract class PdfBidimensionalBarcode
{
  private string text;
  private PdfColor backColor;
  private PointF location;
  private PdfBarcodeQuietZones quietZone;
  private float xDimension;
  private SizeF size;
  private PdfColor foreColor;

  public PdfBidimensionalBarcode() => this.quietZone = new PdfBarcodeQuietZones();

  public virtual SizeF Size
  {
    get => this.size;
    set => this.size = value;
  }

  public string Text
  {
    get => this.text;
    set => this.text = value;
  }

  public PdfColor BackColor
  {
    get => this.backColor;
    set => this.backColor = value;
  }

  public PdfBarcodeQuietZones QuietZone
  {
    get => this.quietZone;
    set => this.quietZone = value;
  }

  public float XDimension
  {
    get => this.xDimension;
    set => this.xDimension = value;
  }

  public PointF Location
  {
    get => this.location;
    set => this.location = value;
  }

  public PdfColor ForeColor
  {
    get => this.foreColor;
    set => this.foreColor = value;
  }

  internal byte[] GetData() => Encoding.Default.GetBytes(this.text);

  public abstract void Draw(PdfPageBase page, PointF location);

  public abstract void Draw(PdfPageBase page);

  public abstract Image ToImage();
}
