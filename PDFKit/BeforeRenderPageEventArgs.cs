// Decompiled with JetBrains decompiler
// Type: PDFKit.BeforeRenderPageEventArgs
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Enums;
using Patagames.Pdf.Net;
using System;
using System.Drawing;

#nullable disable
namespace PDFKit;

public class BeforeRenderPageEventArgs : EventArgs
{
  public Graphics Graphics { get; private set; }

  public PdfPage Page { get; private set; }

  public int X { get; set; }

  public int Y { get; set; }

  public int Width { get; set; }

  public int Height { get; set; }

  public PageRotate Rotation { get; private set; }

  public BeforeRenderPageEventArgs(
    Graphics g,
    PdfPage page,
    int x,
    int y,
    int width,
    int height,
    PageRotate rotation)
  {
    this.Graphics = g;
    this.Page = page;
    this.X = x;
    this.Y = y;
    this.Width = width;
    this.Height = height;
    this.Rotation = rotation;
  }
}
