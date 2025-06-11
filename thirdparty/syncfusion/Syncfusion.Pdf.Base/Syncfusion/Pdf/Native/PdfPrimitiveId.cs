// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Native.PdfPrimitiveId
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Pdf.Native;

[StructLayout(LayoutKind.Sequential, Size = 1)]
internal struct PdfPrimitiveId
{
  internal byte[] Null => new byte[1];

  internal byte[] Integer => new byte[1]{ (byte) 1 };

  internal byte[] Real => new byte[1]{ (byte) 2 };

  internal byte[] Boolean => new byte[1]{ (byte) 3 };

  internal byte[] Name => new byte[1]{ (byte) 4 };

  internal byte[] String => new byte[1]{ (byte) 5 };

  internal byte[] Dictionary => new byte[1]{ (byte) 6 };

  internal byte[] Array => new byte[1]{ (byte) 7 };

  internal byte[] Stream => new byte[1]{ (byte) 8 };

  internal byte[] True => new byte[1]{ (byte) 1 };

  internal byte[] False => new byte[1];

  internal byte[] Visited
  {
    get
    {
      return new byte[4]
      {
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue,
        byte.MaxValue
      };
    }
  }
}
