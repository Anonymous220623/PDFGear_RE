// Decompiled with JetBrains decompiler
// Type: HandyControl.Data.ColorRange
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System.Windows.Media;

#nullable disable
namespace HandyControl.Data;

public class ColorRange : IValueRange<Color>
{
  private Color _start;
  private Color _end;
  private readonly int[] _subColorArr = new int[4];

  public Color Start
  {
    get => this._start;
    set
    {
      this._start = value;
      this.Update();
    }
  }

  public Color End
  {
    get => this._end;
    set
    {
      this._end = value;
      this.Update();
    }
  }

  private void Update()
  {
    this._subColorArr[0] = (int) this._start.A - (int) this._end.A;
    this._subColorArr[1] = (int) this._start.R - (int) this._end.R;
    this._subColorArr[2] = (int) this._start.G - (int) this._end.G;
    this._subColorArr[3] = (int) this._start.B - (int) this._end.B;
  }

  public Color GetColor(double range)
  {
    return range < 0.0 || range > 1.0 ? new Color() : Color.FromArgb((byte) ((double) this._start.A - (double) this._subColorArr[0] * range), (byte) ((double) this._start.R - (double) this._subColorArr[1] * range), (byte) ((double) this._start.G - (double) this._subColorArr[2] * range), (byte) ((double) this._start.B - (double) this._subColorArr[3] * range));
  }
}
