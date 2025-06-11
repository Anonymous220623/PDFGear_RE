// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.IWPicture
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public interface IWPicture : IParagraphItem, IEntity
{
  float Height { get; set; }

  float Width { get; set; }

  float HeightScale { get; set; }

  float WidthScale { get; set; }

  float Rotation { get; set; }

  bool FlipHorizontal { get; set; }

  bool FlipVertical { get; set; }

  void LoadImage(Image imageStream);

  Image Image { get; }

  void LoadImage(byte[] imageBytes);

  void LoadImage(byte[] svgData, byte[] imageBytes);

  byte[] SvgData { get; }

  byte[] ImageBytes { get; }

  IWParagraph AddCaption(
    string name,
    CaptionNumberingFormat format,
    CaptionPosition captionPosition);

  HorizontalOrigin HorizontalOrigin { get; set; }

  VerticalOrigin VerticalOrigin { get; set; }

  float HorizontalPosition { get; set; }

  float VerticalPosition { get; set; }

  TextWrappingStyle TextWrappingStyle { get; set; }

  TextWrappingType TextWrappingType { get; set; }

  bool IsBelowText { get; set; }

  ShapeHorizontalAlignment HorizontalAlignment { get; set; }

  ShapeVerticalAlignment VerticalAlignment { get; set; }

  string AlternativeText { get; set; }

  string Name { get; set; }

  string Title { get; set; }

  bool Visible { get; set; }

  WCharacterFormat CharacterFormat { get; }
}
