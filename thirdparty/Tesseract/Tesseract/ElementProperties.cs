// Decompiled with JetBrains decompiler
// Type: Tesseract.ElementProperties
// Assembly: Tesseract, Version=4.1.1.0, Culture=neutral, PublicKeyToken=null
// MVID: C5D5562D-D917-402B-968F-9F8B28C3D951
// Assembly location: D:\PDFGear\bin\Tesseract.dll

#nullable disable
namespace Tesseract;

public struct ElementProperties(
  Orientation orientation,
  TextLineOrder textLineOrder,
  WritingDirection writingDirection,
  float deskewAngle)
{
  private Orientation orientation = orientation;
  private TextLineOrder textLineOrder = textLineOrder;
  private WritingDirection writingDirection = writingDirection;
  private float deskewAngle = deskewAngle;

  public Orientation Orientation => this.orientation;

  public TextLineOrder TextLineOrder => this.textLineOrder;

  public WritingDirection WritingDirection => this.writingDirection;

  public float DeskewAngle => this.deskewAngle;
}
