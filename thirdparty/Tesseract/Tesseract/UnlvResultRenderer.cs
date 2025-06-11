// Decompiled with JetBrains decompiler
// Type: Tesseract.UnlvResultRenderer
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

using Tesseract.Interop;

#nullable disable
namespace Tesseract;

public sealed class UnlvResultRenderer : ResultRenderer
{
  public UnlvResultRenderer(string outputFilename)
  {
    this.Initialise(TessApi.Native.UnlvRendererCreate(outputFilename));
  }
}
