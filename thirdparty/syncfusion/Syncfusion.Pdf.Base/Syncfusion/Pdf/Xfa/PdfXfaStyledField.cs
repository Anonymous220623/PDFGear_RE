// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Xfa.PdfXfaStyledField
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Graphics;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Xfa;

public abstract class PdfXfaStyledField : PdfXfaField
{
  private string m_toolTip = string.Empty;
  private PdfFont m_font = (PdfFont) new PdfStandardFont(PdfFontFamily.Helvetica, 10f, PdfFontStyle.Regular);
  private PdfColor m_foreColor;
  private PdfXfaHorizontalAlignment m_hAlign = PdfXfaHorizontalAlignment.Center;
  private PdfXfaVerticalAlignment m_vAlign = PdfXfaVerticalAlignment.Middle;
  private PdfXfaRotateAngle m_rotate;
  private PdfXfaBorder m_border = new PdfXfaBorder();
  private bool m_readOnly;
  private float m_width;
  private float m_height;
  internal PdfXfaForm parent;

  public float Width
  {
    set => this.m_width = value;
    get => this.m_width;
  }

  public float Height
  {
    set => this.m_height = value;
    get => this.m_height;
  }

  public PdfXfaBorder Border
  {
    get => this.m_border;
    set
    {
      if (value == null)
        return;
      this.m_border = value;
    }
  }

  public string ToolTip
  {
    get => this.m_toolTip;
    set
    {
      if (value == null)
        return;
      this.m_toolTip = value;
    }
  }

  public bool ReadOnly
  {
    get => this.m_readOnly;
    set => this.m_readOnly = value;
  }

  public PdfFont Font
  {
    get => this.m_font;
    set => this.m_font = value;
  }

  public PdfColor ForeColor
  {
    get => this.m_foreColor;
    set
    {
      if (!(value != PdfColor.Empty))
        return;
      this.m_foreColor = value;
    }
  }

  public PdfXfaHorizontalAlignment HorizontalAlignment
  {
    get => this.m_hAlign;
    set => this.m_hAlign = value;
  }

  public PdfXfaVerticalAlignment VerticalAlignment
  {
    get => this.m_vAlign;
    set => this.m_vAlign = value;
  }

  public PdfXfaRotateAngle Rotate
  {
    get => this.m_rotate;
    set => this.m_rotate = value;
  }

  internal void SetMFTP(XfaWriter xfaWriter)
  {
    xfaWriter.WriteFontInfo(this.Font, this.ForeColor);
    xfaWriter.WriteMargins(this.Margins);
    if (this.ToolTip != null && this.ToolTip != "")
      xfaWriter.WriteToolTip(this.ToolTip);
    xfaWriter.WritePragraph(this.VerticalAlignment, this.HorizontalAlignment);
  }

  internal SizeF GetSize()
  {
    return this.Rotate == PdfXfaRotateAngle.RotateAngle270 || this.Rotate == PdfXfaRotateAngle.RotateAngle90 ? new SizeF(this.Height, this.Width) : new SizeF(this.Width, this.Height);
  }

  internal int GetRotationAngle()
  {
    int rotationAngle = 0;
    if (this.Rotate != PdfXfaRotateAngle.RotateAngle0)
    {
      switch (this.Rotate)
      {
        case PdfXfaRotateAngle.RotateAngle90:
          rotationAngle = 90;
          break;
        case PdfXfaRotateAngle.RotateAngle180:
          rotationAngle = 180;
          break;
        case PdfXfaRotateAngle.RotateAngle270:
          rotationAngle = 270;
          break;
      }
    }
    return rotationAngle;
  }
}
