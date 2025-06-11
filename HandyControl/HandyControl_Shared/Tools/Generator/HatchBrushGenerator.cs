// Decompiled with JetBrains decompiler
// Type: HandyControl.Tools.HatchBrushGenerator
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using HandyControl.Data;
using System;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace HandyControl.Tools;

public class HatchBrushGenerator
{
  private static readonly byte[][] HatchBrushes = new byte[53][]
  {
    new byte[8]
    {
      (byte) 0,
      (byte) 0,
      (byte) 0,
      byte.MaxValue,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    },
    new byte[8]
    {
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8
    },
    new byte[8]
    {
      (byte) 1,
      (byte) 2,
      (byte) 4,
      (byte) 8,
      (byte) 16 /*0x10*/,
      (byte) 32 /*0x20*/,
      (byte) 64 /*0x40*/,
      (byte) 128 /*0x80*/
    },
    new byte[8]
    {
      (byte) 128 /*0x80*/,
      (byte) 64 /*0x40*/,
      (byte) 32 /*0x20*/,
      (byte) 16 /*0x10*/,
      (byte) 8,
      (byte) 4,
      (byte) 2,
      (byte) 1
    },
    new byte[8]
    {
      (byte) 8,
      (byte) 8,
      (byte) 8,
      byte.MaxValue,
      (byte) 8,
      (byte) 8,
      (byte) 8,
      (byte) 8
    },
    new byte[8]
    {
      (byte) 129,
      (byte) 66,
      (byte) 36,
      (byte) 24,
      (byte) 24,
      (byte) 36,
      (byte) 66,
      (byte) 129
    },
    new byte[8]
    {
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 8,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 128 /*0x80*/
    },
    new byte[8]
    {
      (byte) 0,
      (byte) 2,
      (byte) 0,
      (byte) 136,
      (byte) 0,
      (byte) 32 /*0x20*/,
      (byte) 0,
      (byte) 136
    },
    new byte[8]
    {
      (byte) 0,
      (byte) 34,
      (byte) 0,
      (byte) 204,
      (byte) 0,
      (byte) 34,
      (byte) 0,
      (byte) 204
    },
    new byte[8]
    {
      (byte) 0,
      (byte) 204,
      (byte) 0,
      (byte) 204,
      (byte) 0,
      (byte) 204,
      (byte) 0,
      (byte) 204
    },
    new byte[8]
    {
      (byte) 0,
      (byte) 204,
      (byte) 4,
      (byte) 204,
      (byte) 0,
      (byte) 204,
      (byte) 64 /*0x40*/,
      (byte) 204
    },
    new byte[8]
    {
      (byte) 68,
      (byte) 204,
      (byte) 34,
      (byte) 204,
      (byte) 68,
      (byte) 204,
      (byte) 34,
      (byte) 204
    },
    new byte[8]
    {
      (byte) 85,
      (byte) 204,
      (byte) 85,
      (byte) 204,
      (byte) 85,
      (byte) 204,
      (byte) 85,
      (byte) 204
    },
    new byte[8]
    {
      (byte) 85,
      (byte) 205,
      (byte) 85,
      (byte) 238,
      (byte) 85,
      (byte) 220,
      (byte) 85,
      (byte) 238
    },
    new byte[8]
    {
      (byte) 85,
      (byte) 221,
      (byte) 85,
      byte.MaxValue,
      (byte) 85,
      (byte) 221,
      (byte) 85,
      byte.MaxValue
    },
    new byte[8]
    {
      (byte) 85,
      byte.MaxValue,
      (byte) 85,
      byte.MaxValue,
      (byte) 85,
      byte.MaxValue,
      (byte) 85,
      byte.MaxValue
    },
    new byte[8]
    {
      (byte) 85,
      byte.MaxValue,
      (byte) 89,
      byte.MaxValue,
      (byte) 85,
      byte.MaxValue,
      (byte) 153,
      byte.MaxValue
    },
    new byte[8]
    {
      (byte) 119,
      byte.MaxValue,
      (byte) 221,
      byte.MaxValue,
      (byte) 119,
      byte.MaxValue,
      (byte) 253,
      byte.MaxValue
    },
    new byte[8]
    {
      (byte) 17,
      (byte) 34,
      (byte) 68,
      (byte) 136,
      (byte) 17,
      (byte) 34,
      (byte) 68,
      (byte) 136
    },
    new byte[8]
    {
      (byte) 136,
      (byte) 68,
      (byte) 34,
      (byte) 17,
      (byte) 136,
      (byte) 68,
      (byte) 34,
      (byte) 17
    },
    new byte[8]
    {
      (byte) 153,
      (byte) 51,
      (byte) 102,
      (byte) 204,
      (byte) 153,
      (byte) 51,
      (byte) 102,
      (byte) 204
    },
    new byte[8]
    {
      (byte) 204,
      (byte) 102,
      (byte) 51,
      (byte) 153,
      (byte) 204,
      (byte) 102,
      (byte) 51,
      (byte) 153
    },
    new byte[8]
    {
      (byte) 193,
      (byte) 131,
      (byte) 7,
      (byte) 14,
      (byte) 28,
      (byte) 56,
      (byte) 112 /*0x70*/,
      (byte) 224 /*0xE0*/
    },
    new byte[8]
    {
      (byte) 224 /*0xE0*/,
      (byte) 112 /*0x70*/,
      (byte) 56,
      (byte) 28,
      (byte) 14,
      (byte) 7,
      (byte) 131,
      (byte) 193
    },
    new byte[8]
    {
      (byte) 136,
      (byte) 136,
      (byte) 136,
      (byte) 136,
      (byte) 136,
      (byte) 136,
      (byte) 136,
      (byte) 136
    },
    new byte[8]
    {
      (byte) 0,
      (byte) 0,
      (byte) 0,
      byte.MaxValue,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      byte.MaxValue
    },
    new byte[8]
    {
      (byte) 170,
      (byte) 170,
      (byte) 170,
      (byte) 170,
      (byte) 170,
      (byte) 170,
      (byte) 170,
      (byte) 170
    },
    new byte[8]
    {
      (byte) 0,
      byte.MaxValue,
      (byte) 0,
      byte.MaxValue,
      (byte) 0,
      byte.MaxValue,
      (byte) 0,
      byte.MaxValue
    },
    new byte[8]
    {
      (byte) 204,
      (byte) 204,
      (byte) 204,
      (byte) 204,
      (byte) 204,
      (byte) 204,
      (byte) 204,
      (byte) 204
    },
    new byte[8]
    {
      (byte) 0,
      (byte) 0,
      byte.MaxValue,
      byte.MaxValue,
      (byte) 0,
      (byte) 0,
      byte.MaxValue,
      byte.MaxValue
    },
    new byte[8]
    {
      (byte) 17,
      (byte) 34,
      (byte) 68,
      (byte) 136,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    },
    new byte[8]
    {
      (byte) 136,
      (byte) 68,
      (byte) 34,
      (byte) 17,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 0
    },
    new byte[8]
    {
      (byte) 15,
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 240 /*0xF0*/,
      (byte) 0,
      (byte) 0,
      (byte) 0
    },
    new byte[8]
    {
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      (byte) 16 /*0x10*/,
      (byte) 16 /*0x10*/,
      (byte) 16 /*0x10*/,
      (byte) 16 /*0x10*/
    },
    new byte[8]
    {
      (byte) 1,
      (byte) 8,
      (byte) 128 /*0x80*/,
      (byte) 16 /*0x10*/,
      (byte) 2,
      (byte) 64 /*0x40*/,
      (byte) 4,
      (byte) 32 /*0x20*/
    },
    new byte[8]
    {
      (byte) 3,
      (byte) 99,
      (byte) 108,
      (byte) 12,
      (byte) 192 /*0xC0*/,
      (byte) 198,
      (byte) 54,
      (byte) 48 /*0x30*/
    },
    new byte[8]
    {
      (byte) 3,
      (byte) 132,
      (byte) 72,
      (byte) 48 /*0x30*/,
      (byte) 3,
      (byte) 132,
      (byte) 72,
      (byte) 48 /*0x30*/
    },
    new byte[8]
    {
      (byte) 48 /*0x30*/,
      (byte) 73,
      (byte) 6,
      (byte) 0,
      (byte) 48 /*0x30*/,
      (byte) 73,
      (byte) 6,
      (byte) 0
    },
    new byte[8]
    {
      (byte) 129,
      (byte) 66,
      (byte) 36,
      (byte) 24,
      (byte) 8,
      (byte) 4,
      (byte) 2,
      (byte) 1
    },
    new byte[8]
    {
      byte.MaxValue,
      (byte) 1,
      (byte) 1,
      (byte) 1,
      byte.MaxValue,
      (byte) 16 /*0x10*/,
      (byte) 16 /*0x10*/,
      (byte) 16 /*0x10*/
    },
    new byte[8]
    {
      (byte) 17,
      (byte) 130,
      (byte) 68,
      (byte) 168,
      (byte) 17,
      (byte) 162,
      (byte) 68,
      (byte) 42
    },
    new byte[8]
    {
      (byte) 85,
      (byte) 170,
      (byte) 85,
      (byte) 170,
      (byte) 15,
      (byte) 15,
      (byte) 15,
      (byte) 15
    },
    new byte[8]
    {
      (byte) 2,
      (byte) 1,
      (byte) 2,
      (byte) 0,
      (byte) 16 /*0x10*/,
      (byte) 32 /*0x20*/,
      (byte) 16 /*0x10*/,
      (byte) 0
    },
    new byte[8]
    {
      (byte) 85,
      (byte) 0,
      (byte) 1,
      (byte) 0,
      (byte) 1,
      (byte) 0,
      (byte) 1,
      (byte) 0
    },
    new byte[8]
    {
      (byte) 17,
      (byte) 0,
      (byte) 4,
      (byte) 0,
      (byte) 17,
      (byte) 0,
      (byte) 64 /*0x40*/,
      (byte) 0
    },
    new byte[8]
    {
      (byte) 3,
      (byte) 12,
      (byte) 16 /*0x10*/,
      (byte) 32 /*0x20*/,
      (byte) 32 /*0x20*/,
      (byte) 48 /*0x30*/,
      (byte) 72,
      (byte) 132
    },
    new byte[8]
    {
      byte.MaxValue,
      (byte) 51,
      byte.MaxValue,
      (byte) 204,
      byte.MaxValue,
      (byte) 51,
      byte.MaxValue,
      (byte) 204
    },
    new byte[8]
    {
      (byte) 238,
      (byte) 25,
      (byte) 31 /*0x1F*/,
      (byte) 31 /*0x1F*/,
      (byte) 238,
      (byte) 145,
      (byte) 241,
      (byte) 241
    },
    new byte[8]
    {
      byte.MaxValue,
      (byte) 17,
      (byte) 17,
      (byte) 17,
      byte.MaxValue,
      (byte) 17,
      (byte) 17,
      (byte) 17
    },
    new byte[8]
    {
      (byte) 51,
      (byte) 51,
      (byte) 204,
      (byte) 204,
      (byte) 51,
      (byte) 51,
      (byte) 204,
      (byte) 204
    },
    new byte[8]
    {
      (byte) 15,
      (byte) 15,
      (byte) 15,
      (byte) 15,
      (byte) 240 /*0xF0*/,
      (byte) 240 /*0xF0*/,
      (byte) 240 /*0xF0*/,
      (byte) 240 /*0xF0*/
    },
    new byte[8]
    {
      (byte) 1,
      (byte) 130,
      (byte) 68,
      (byte) 40,
      (byte) 16 /*0x10*/,
      (byte) 40,
      (byte) 68,
      (byte) 130
    },
    new byte[8]
    {
      (byte) 8,
      (byte) 28,
      (byte) 62,
      (byte) 127 /*0x7F*/,
      (byte) 62,
      (byte) 28,
      (byte) 8,
      (byte) 0
    }
  };

  public Brush GetHatchBrush(HatchStyle hatchStyle, Color foreColor, Color backColor)
  {
    byte[] hatchData = this.GetHatchData(hatchStyle);
    GeometryGroup geometryGroup = new GeometryGroup();
    for (int y = 0; y < 8; ++y)
    {
      for (int x = 0; x < 8; ++x)
      {
        if (((int) hatchData[y] & 128 /*0x80*/ >> x) > 0)
          geometryGroup.Children.Add((Geometry) new RectangleGeometry(new Rect((double) x, (double) y, 1.0, 1.0)));
      }
    }
    DrawingBrush target = new DrawingBrush();
    target.Viewport = new Rect(0.0, 0.0, 8.0, 8.0);
    target.ViewportUnits = BrushMappingMode.Absolute;
    target.Stretch = Stretch.None;
    target.TileMode = TileMode.Tile;
    target.Drawing = (Drawing) new DrawingGroup()
    {
      Children = {
        (Drawing) new GeometryDrawing()
        {
          Brush = (Brush) new SolidColorBrush(backColor),
          Geometry = (Geometry) new RectangleGeometry(new Rect(0.0, 0.0, 8.0, 8.0))
        },
        (Drawing) new GeometryDrawing()
        {
          Brush = (Brush) new SolidColorBrush(foreColor),
          Geometry = (Geometry) geometryGroup
        }
      }
    };
    RenderOptions.SetCachingHint((DependencyObject) target, CachingHint.Cache);
    return (Brush) target;
  }

  private byte[] GetHatchData(HatchStyle hatchStyle)
  {
    return hatchStyle >= HatchStyle.Horizontal && hatchStyle <= HatchStyle.SolidDiamond ? HatchBrushGenerator.HatchBrushes[(int) hatchStyle] : throw new ArgumentOutOfRangeException(nameof (hatchStyle));
  }
}
