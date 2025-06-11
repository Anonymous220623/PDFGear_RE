// Decompiled with JetBrains decompiler
// Type: Tesseract.FontInfo
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

#nullable disable
namespace Tesseract;

public class FontInfo
{
  internal FontInfo(
    string name,
    int id,
    bool isItalic,
    bool isBold,
    bool isFixedPitch,
    bool isSerif,
    bool isFraktur = false)
  {
    this.Name = name;
    this.Id = id;
    this.IsItalic = isItalic;
    this.IsBold = isBold;
    this.IsFixedPitch = isFixedPitch;
    this.IsSerif = isSerif;
    this.IsFraktur = isFraktur;
  }

  public string Name { get; private set; }

  public int Id { get; private set; }

  public bool IsItalic { get; private set; }

  public bool IsBold { get; private set; }

  public bool IsFixedPitch { get; private set; }

  public bool IsSerif { get; private set; }

  public bool IsFraktur { get; private set; }
}
