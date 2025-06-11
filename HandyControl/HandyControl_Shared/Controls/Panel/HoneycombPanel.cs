// Decompiled with JetBrains decompiler
// Type: HandyControl.Controls.HoneycombPanel
// Assembly: HandyControl, Version=3.5.1.0, Culture=neutral, PublicKeyToken=45be8712787a1e5b
// MVID: 037726F1-D572-4656-BADB-F5293B9C5CB8
// Assembly location: D:\PDFGear\bin\HandyControl.dll

using System;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace HandyControl.Controls;

public class HoneycombPanel : Panel
{
  private double _unitLength;
  private HoneycombPanel.HoneycombStuffer _stuffer;

  private static int GetXCount(int count)
  {
    if (count == 0)
      return 0;
    --count;
    int num1 = (int) Math.Floor(Math.Pow((12.0 * (double) count + 25.0) / 36.0, 0.5) - 5.0 / 6.0);
    int num2 = 3 * num1 * num1 + 5 * num1;
    int num3 = num2 + 2;
    if (count >= num3)
      return 4 * num1 + 6;
    return count <= num2 ? 4 * num1 + 2 : 4 * num1 + 4;
  }

  private static int GetYCount(int count)
  {
    if (count == 0)
      return 0;
    --count;
    int num1 = (int) Math.Floor(Math.Pow((double) count / 3.0 + 0.25, 0.5) - 0.5);
    int num2 = 3 * num1 * num1 + 3 * num1;
    return count <= num2 ? 2 * num1 : 2 * num1 + 2;
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    Size size = new Size();
    foreach (UIElement internalChild in this.InternalChildren)
    {
      if (internalChild != null)
      {
        internalChild.Measure(availableSize);
        size.Width = Math.Max(size.Width, internalChild.DesiredSize.Width);
        size.Height = Math.Max(size.Height, internalChild.DesiredSize.Height);
      }
    }
    this._unitLength = Math.Max(size.Width, size.Height) / 2.0;
    return new Size((double) HoneycombPanel.GetXCount(this.InternalChildren.Count) * this._unitLength, (double) HoneycombPanel.GetYCount(this.InternalChildren.Count) * Math.Pow(3.0, 0.5) * this._unitLength + this._unitLength * 2.0);
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    double num = this._unitLength * 2.0;
    this._stuffer = new HoneycombPanel.HoneycombStuffer(new Rect(finalSize.Width / 2.0 - this._unitLength, finalSize.Height / 2.0 - this._unitLength, num, num));
    foreach (UIElement internalChild in this.InternalChildren)
      internalChild.Arrange(this._stuffer.Move());
    return finalSize;
  }

  private class HoneycombStuffer
  {
    private int _turns;
    private int _maxIndex;
    private int _currentIndex = -1;
    private readonly double _offsetX;
    private readonly double _offsetY;
    private Rect _childBounds;
    private readonly double[] _offsetXArr;
    private readonly double[] _offsetYArr;

    public HoneycombStuffer(Rect childBounds)
    {
      this._childBounds = childBounds;
      this._offsetX = childBounds.Width / 2.0;
      this._offsetY = Math.Pow(3.0, 0.5) * this._offsetX;
      this._offsetXArr = new double[6]
      {
        2.0 * this._offsetX,
        this._offsetX,
        -this._offsetX,
        -2.0 * this._offsetX,
        -this._offsetX,
        this._offsetX
      };
      this._offsetYArr = new double[6]
      {
        0.0,
        this._offsetY,
        this._offsetY,
        0.0,
        -this._offsetY,
        -this._offsetY
      };
    }

    public Rect Move()
    {
      ++this._currentIndex;
      if (this._currentIndex > this._maxIndex)
      {
        ++this._turns;
        this._maxIndex = this._turns * 6 - 1;
        this._currentIndex = 0;
        this._childBounds.Offset(this._offsetX, -this._offsetY);
        return this._childBounds;
      }
      if (this._turns > 0)
      {
        int index = this._currentIndex / this._turns;
        this._childBounds.Offset(this._offsetXArr[index], this._offsetYArr[index]);
      }
      return this._childBounds;
    }
  }
}
