// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.VisibleItemsHandler
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Globalization;
using System.Text;

#nullable disable
namespace Syncfusion.Windows.Shared;

internal class VisibleItemsHandler
{
  private const int AvailablePositionDigit = -1;
  private VisiblePanelItem[] positions;

  public VisibleItemsHandler(int visiblePositions)
  {
    this.positions = new VisiblePanelItem[visiblePositions];
  }

  public int Count => this.positions.Length;

  public VisiblePanelItem this[int positionIndex]
  {
    get => this.positions[positionIndex];
    set => this.positions.SetValue((object) value, positionIndex);
  }

  public int GetFreePositionsLeft()
  {
    int freePositionsLeft = 0;
    for (int positionIndex = 0; positionIndex < this.Count && this[positionIndex] == null; ++positionIndex)
      ++freePositionsLeft;
    return freePositionsLeft;
  }

  public int GetFreePositionsRight()
  {
    int freePositionsRight = 0;
    for (int positionIndex = this.Count - 1; positionIndex >= 0 && this[positionIndex] == null; --positionIndex)
      ++freePositionsRight;
    return freePositionsRight;
  }

  public VisiblePanelItem GetItemAtPosition(int positionIndex) => this.positions[positionIndex];

  public int GetLargestItemIndex()
  {
    int largestItemIndex = -1;
    for (int positionIndex = 0; positionIndex < this.Count; ++positionIndex)
    {
      if (this[positionIndex] != null && this[positionIndex].Index > largestItemIndex)
        largestItemIndex = this[positionIndex].Index;
    }
    return largestItemIndex;
  }

  public int GetUsedPositions()
  {
    int usedPositions = 0;
    for (int positionIndex = 0; positionIndex < this.Count; ++positionIndex)
    {
      if (this[positionIndex] != null)
        ++usedPositions;
    }
    return usedPositions;
  }

  public void SetItemAtPosition(int positionIndex, VisiblePanelItem item)
  {
    this.positions.SetValue((object) item, positionIndex);
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder(this.Count);
    for (int positionIndex = 0; positionIndex < this.Count; ++positionIndex)
    {
      if (this[positionIndex] == null)
        stringBuilder.Append(-1.ToString((IFormatProvider) CultureInfo.InstalledUICulture));
      else
        stringBuilder.Append(this[positionIndex].Index);
    }
    return stringBuilder.ToString();
  }
}
