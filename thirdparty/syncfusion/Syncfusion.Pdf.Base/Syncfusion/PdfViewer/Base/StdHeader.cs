// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.StdHeader
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class StdHeader
{
  private readonly long offset;
  private readonly int headerLength;

  public long Offset => this.offset;

  public int HeaderLength => this.headerLength;

  public uint NextHeaderOffset { get; private set; }

  public StdHeader(long offset, int headerLength)
  {
    this.offset = offset;
    this.headerLength = headerLength;
  }

  public void Read(StdFontReader reader)
  {
    int num1 = (int) reader.Read();
    int num2 = (int) reader.Read();
    this.NextHeaderOffset = reader.ReadUInt();
  }

  public bool IsPositionInside(long position)
  {
    return this.Offset <= position && position < this.Offset + (long) this.HeaderLength;
  }
}
