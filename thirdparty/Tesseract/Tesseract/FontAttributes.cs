// Decompiled with JetBrains decompiler
// Type: Tesseract.FontAttributes
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

#nullable disable
namespace Tesseract;

public class FontAttributes
{
  public FontInfo FontInfo { get; private set; }

  public bool IsUnderlined { get; private set; }

  public bool IsSmallCaps { get; private set; }

  public int PointSize { get; private set; }

  public FontAttributes(FontInfo fontInfo, bool isUnderlined, bool isSmallCaps, int pointSize)
  {
    this.FontInfo = fontInfo;
    this.IsUnderlined = isUnderlined;
    this.IsSmallCaps = isSmallCaps;
    this.PointSize = pointSize;
  }
}
