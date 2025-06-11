// Decompiled with JetBrains decompiler
// Type: Patagames.Pdf.Enums.KeyboardModifiers
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Pdf.Enums;

/// <summary>Keyboard modifiers</summary>
public enum KeyboardModifiers
{
  ShiftKey = 1,
  ControlKey = 2,
  AltKey = 4,
  MetaKey = 8,
  KeyPad = 16, // 0x00000010
  AutoRepeat = 32, // 0x00000020
  LeftButtonDown = 64, // 0x00000040
  MiddleButtonDown = 128, // 0x00000080
  RightButtonDown = 256, // 0x00000100
}
