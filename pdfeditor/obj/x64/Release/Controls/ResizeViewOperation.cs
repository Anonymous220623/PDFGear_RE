// Decompiled with JetBrains decompiler
// Type: pdfeditor.Controls.ResizeViewOperation
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;

#nullable disable
namespace pdfeditor.Controls;

[Flags]
public enum ResizeViewOperation
{
  None = 0,
  Move = 1,
  LeftTop = 2,
  CenterTop = 4,
  RightTop = 8,
  LeftCenter = 16, // 0x00000010
  RightCenter = 32, // 0x00000020
  LeftBottom = 64, // 0x00000040
  CenterBottom = 128, // 0x00000080
  RightBottom = 256, // 0x00000100
  ResizeCorner = RightBottom | LeftBottom | RightTop | LeftTop, // 0x0000014A
  ResizeCornerAndMove = ResizeCorner | Move, // 0x0000014B
  ResizeAll = ResizeCorner | CenterBottom | RightCenter | LeftCenter | CenterTop, // 0x000001FE
  All = ResizeAll | Move, // 0x000001FF
}
