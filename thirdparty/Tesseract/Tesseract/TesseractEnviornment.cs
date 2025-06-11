// Decompiled with JetBrains decompiler
// Type: Tesseract.TesseractEnviornment
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

using InteropDotNet;

#nullable disable
namespace Tesseract;

public static class TesseractEnviornment
{
  public static string CustomSearchPath
  {
    get => LibraryLoader.Instance.CustomSearchPath;
    set => LibraryLoader.Instance.CustomSearchPath = value;
  }
}
