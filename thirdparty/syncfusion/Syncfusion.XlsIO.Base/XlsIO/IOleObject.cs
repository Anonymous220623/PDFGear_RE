// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.IOleObject
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO;

public interface IOleObject
{
  IRange Location { get; set; }

  Size Size { get; set; }

  Image Picture { get; }

  IPictureShape Shape { get; }

  string Name { get; }

  bool DisplayAsIcon { get; set; }
}
