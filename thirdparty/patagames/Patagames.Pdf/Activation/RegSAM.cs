// Decompiled with JetBrains decompiler
// Type: Patagames.Activation.RegSAM
// Assembly: Patagames.Pdf, Version=9.68.38.462, Culture=neutral, PublicKeyToken=60fd6cf9b15941cf
// MVID: 6A4C4B5C-8C74-4D04-A848-779C64CD849D
// Assembly location: D:\PDFGear\bin\Patagames.Pdf.dll
// XML documentation location: D:\PDFGear\bin\Patagames.Pdf.xml

#nullable disable
namespace Patagames.Activation;

internal enum RegSAM
{
  QueryValue = 1,
  SetValue = 2,
  CreateSubKey = 4,
  EnumerateSubKeys = 8,
  Notify = 16, // 0x00000010
  CreateLink = 32, // 0x00000020
  WOW64_64Key = 256, // 0x00000100
  WOW64_32Key = 512, // 0x00000200
  WOW64_Res = 768, // 0x00000300
  Write = 131078, // 0x00020006
  Execute = 131097, // 0x00020019
  Read = 131097, // 0x00020019
  AllAccess = 983103, // 0x000F003F
}
