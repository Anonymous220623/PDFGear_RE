// Decompiled with JetBrains decompiler
// Type: pdfeditor.Models.PageContents.ImageMatrix
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

#nullable disable
namespace pdfeditor.Models.PageContents;

public class ImageMatrix
{
  public float a;
  public float b;
  public float c;
  public float d;
  public float e;
  public float f;

  public ImageMatrix()
  {
  }

  public ImageMatrix(float a, float b, float c, float d, float e, float f)
  {
    this.a = a;
    this.b = b;
    this.c = c;
    this.d = d;
    this.e = e;
    this.f = f;
  }
}
