// Decompiled with JetBrains decompiler
// Type: PDFKit.HighlightInfo
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf;
using System.Windows.Media;

#nullable disable
namespace PDFKit;

public struct HighlightInfo
{
  public int CharIndex;
  public int CharsCount;
  public Color Color;

  public FS_RECTF Inflate { get; set; }
}
