// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ImagePreRenderEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf;

public class ImagePreRenderEventArgs : EventArgs
{
  internal Stream m_imageStream;
  internal float m_height;
  internal float m_width;
  internal string[] m_filter;

  public Stream ImageStream
  {
    get => this.m_imageStream;
    set => this.m_imageStream = value;
  }

  public float Height
  {
    get => this.m_height;
    set => this.m_height = value;
  }

  public float Width
  {
    get => this.m_width;
    set => this.m_width = value;
  }

  public string[] Filter => this.m_filter;

  internal ImagePreRenderEventArgs(ImageStructure structure)
  {
    this.m_imageStream = structure.ImageStream;
    this.m_height = structure.Height;
    this.m_width = structure.Width;
    this.m_filter = structure.ImageFilter;
  }
}
