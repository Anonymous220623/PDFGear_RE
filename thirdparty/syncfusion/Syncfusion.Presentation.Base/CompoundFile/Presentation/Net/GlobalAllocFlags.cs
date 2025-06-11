// Decompiled with JetBrains decompiler
// Type: Syncfusion.CompoundFile.Presentation.Net.GlobalAllocFlags
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;

#nullable disable
namespace Syncfusion.CompoundFile.Presentation.Net;

[Flags]
internal enum GlobalAllocFlags
{
  GMEM_FIXED = 0,
  GMEM_MOVEABLE = 2,
  GMEM_ZEROINIT = 64, // 0x00000040
  GMEM_NODISCARD = 32, // 0x00000020
}
