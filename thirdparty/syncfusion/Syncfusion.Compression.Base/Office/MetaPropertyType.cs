// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.MetaPropertyType
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

#nullable disable
namespace Syncfusion.Office;

public enum MetaPropertyType
{
  Unknown = 0,
  Boolean = 1,
  Choice = 2,
  Currency = 5,
  DateTime = 6,
  Lookup = 10, // 0x0000000A
  Note = 14, // 0x0000000E
  Number = 15, // 0x0000000F
  Text = 16, // 0x00000010
  Url = 17, // 0x00000011
  User = 18, // 0x00000012
}
