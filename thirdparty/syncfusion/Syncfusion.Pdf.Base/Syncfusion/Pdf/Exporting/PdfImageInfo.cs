// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Exporting.PdfImageInfo
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.IO;
using Syncfusion.Pdf.Xmp;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Exporting;

public class PdfImageInfo
{
  private RectangleF m_bounds;
  private Image m_image;
  private int m_index;
  private string m_name;
  private PdfMatrix m_matrix;
  private bool m_maskImage;
  private bool m_bisImageExtracted;
  internal bool m_isImageMasked;
  internal bool m_isImageInterpolated;
  internal bool m_isSoftMasked;
  private XmpMetadata m_metadata;

  internal PdfImageInfo()
  {
  }

  internal PdfImageInfo(RectangleF bounds, Image image, int index)
  {
    this.m_bounds = bounds;
    this.m_image = image;
    this.m_index = index;
  }

  internal PdfImageInfo(RectangleF bounds, Image image, int index, string name)
  {
    this.m_bounds = bounds;
    this.m_image = image;
    this.m_index = index;
    this.m_name = name;
  }

  public RectangleF Bounds
  {
    get => this.m_bounds;
    internal set => this.m_bounds = value;
  }

  public Image Image
  {
    get => this.m_image;
    internal set => this.m_image = value;
  }

  public int Index
  {
    get => this.m_index;
    internal set => this.m_index = value;
  }

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  internal PdfMatrix Matrix
  {
    get => this.m_matrix;
    set => this.m_matrix = value;
  }

  internal bool MaskImage
  {
    get => this.m_maskImage;
    set => this.m_maskImage = value;
  }

  public bool IsSoftMasked => this.m_isSoftMasked;

  public bool IsImageMasked => this.m_isImageMasked;

  public bool IsImageInterpolated => this.m_isImageInterpolated;

  internal bool IsImageExtracted
  {
    get => this.m_bisImageExtracted;
    set => this.m_bisImageExtracted = value;
  }

  public XmpMetadata Metadata
  {
    get => this.m_metadata;
    internal set => this.m_metadata = value;
  }
}
