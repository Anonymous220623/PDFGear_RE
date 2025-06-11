// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Metafiles.CustomLineCapArrowData
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Metafiles;

internal struct CustomLineCapArrowData
{
  internal float width;
  internal float height;
  internal float middleInset;
  internal int fillState;
  internal int lineStartCap;
  internal int lineEndCap;
  internal int lineJoin;
  internal float lineMitterLimit;
  internal float widthScale;

  internal void Reset()
  {
    this.width = 0.0f;
    this.height = 0.0f;
    this.middleInset = 0.0f;
    this.fillState = 0;
    this.fillState = 0;
    this.lineEndCap = 0;
    this.lineEndCap = 0;
    this.lineJoin = 0;
    this.lineMitterLimit = 0.0f;
    this.widthScale = 0.0f;
  }
}
