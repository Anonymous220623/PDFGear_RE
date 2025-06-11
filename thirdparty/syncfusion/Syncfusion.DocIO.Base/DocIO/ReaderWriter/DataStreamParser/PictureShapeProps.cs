// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.PictureShapeProps
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using System;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser;

[CLSCompliant(false)]
internal class PictureShapeProps : BaseProps
{
  private float m_brightness = 50f;
  private float m_contrast = 50f;
  private PictureColor m_color;
  private string m_altText;
  private string m_name;

  internal PictureShapeProps()
  {
  }

  internal float PictureBrightness
  {
    get => this.m_brightness;
    set => this.m_brightness = value;
  }

  internal float PictureContrast
  {
    get => this.m_contrast;
    set => this.m_contrast = value;
  }

  internal PictureColor PictureColor
  {
    get => this.m_color;
    set => this.m_color = value;
  }

  internal string AlternativeText
  {
    get => this.m_altText;
    set => this.m_altText = value;
  }

  internal string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }
}
