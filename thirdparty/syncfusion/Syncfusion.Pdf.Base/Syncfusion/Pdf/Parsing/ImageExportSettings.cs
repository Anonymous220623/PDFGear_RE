// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.ImageExportSettings
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class ImageExportSettings
{
  private SizeF m_customSize;
  private float m_dpiX;
  private float m_dpiY;
  private bool m_keepAspectRatio;

  public event ImageExportSettings.FontNotFoundEventHandler FontNotFound;

  internal void OnFontNotFounded(FontNotFoundEventArgs e)
  {
    if (this.FontNotFound == null)
      return;
    this.FontNotFound((object) this, e);
  }

  public SizeF CustomSize
  {
    get => this.m_customSize;
    set => this.m_customSize = value;
  }

  public float DpiX
  {
    get => this.m_dpiX;
    set => this.m_dpiX = value;
  }

  public float DpiY
  {
    get => this.m_dpiY;
    set => this.m_dpiY = value;
  }

  public bool KeepAspectRatio
  {
    get => this.m_keepAspectRatio;
    set => this.m_keepAspectRatio = value;
  }

  public delegate void FontNotFoundEventHandler(object sender, FontNotFoundEventArgs args);
}
