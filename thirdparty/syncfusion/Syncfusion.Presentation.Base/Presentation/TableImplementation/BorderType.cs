// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.TableImplementation.BorderType
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;

#nullable disable
namespace Syncfusion.Presentation.TableImplementation;

[Flags]
internal enum BorderType : sbyte
{
  None = 0,
  Left = 1,
  Top = 2,
  Right = 4,
  Bottom = 8,
  DiagonalUp = 16, // 0x10
  DiagonalDown = 32, // 0x20
}
