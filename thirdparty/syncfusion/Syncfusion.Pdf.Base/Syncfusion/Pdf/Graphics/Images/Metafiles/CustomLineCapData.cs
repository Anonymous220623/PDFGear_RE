// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Metafiles.CustomLineCapData
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Metafiles;

internal struct CustomLineCapData
{
  internal int baseCap;
  internal int baseInset;
  internal int strokeStartCap;
  internal int strokeEndCap;
  internal int strokeJoin;
  internal float strokeMitterLimit;
  internal float widthScale;

  internal void Reset()
  {
    this.baseCap = 0;
    this.baseInset = 0;
    this.strokeStartCap = 0;
    this.strokeEndCap = 0;
    this.strokeJoin = 0;
    this.strokeMitterLimit = 0.0f;
    this.widthScale = 0.0f;
  }
}
