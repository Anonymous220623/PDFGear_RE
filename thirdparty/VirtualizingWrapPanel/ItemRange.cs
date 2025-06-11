// Decompiled with JetBrains decompiler
// Type: WpfToolkit.Controls.ItemRange
// Assembly: VirtualizingWrapPanel, Version=1.5.4.0, Culture=neutral, PublicKeyToken=null
// MVID: E61E2A8E-A00C-4FB4-9D6E-5B7404CFB214
// Assembly location: D:\PDFGear\bin\VirtualizingWrapPanel.dll

#nullable disable
namespace WpfToolkit.Controls;

public struct ItemRange
{
  public int StartIndex { get; }

  public int EndIndex { get; }

  public ItemRange(int startIndex, int endIndex)
    : this()
  {
    this.StartIndex = startIndex;
    this.EndIndex = endIndex;
  }

  public bool Contains(int itemIndex) => itemIndex >= this.StartIndex && itemIndex <= this.EndIndex;
}
