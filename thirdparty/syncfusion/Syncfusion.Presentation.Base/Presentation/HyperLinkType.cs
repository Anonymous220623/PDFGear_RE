// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.HyperLinkType
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation;

public enum HyperLinkType
{
  Unknown = -1, // 0xFFFFFFFF
  NoAction = 0,
  Hyperlink = 1,
  JumpFirstSlide = 2,
  JumpPreviousSlide = 3,
  JumpNextSlide = 4,
  JumpLastSlide = 5,
  JumpEndShow = 6,
  JumpLastViewedSlide = 7,
  JumpSpecificSlide = 8,
  OpenFile = 10, // 0x0000000A
  StartProgram = 14, // 0x0000000E
}
