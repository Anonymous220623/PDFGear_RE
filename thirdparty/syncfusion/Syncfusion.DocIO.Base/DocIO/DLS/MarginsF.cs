// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.MarginsF
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public sealed class MarginsF : FormatBase
{
  internal const int LeftKey = 1;
  internal const int RightKey = 2;
  internal const int TopKey = 3;
  internal const int BottomKey = 4;
  internal const int GutterKey = 5;

  public float All
  {
    get => !this.IsAll ? 0.0f : this.Left;
    set
    {
      if ((double) this.Left == (double) value && this.IsAll)
        return;
      this.Left = this.Right = this.Top = this.Bottom = value;
    }
  }

  public float Left
  {
    get => (float) this.GetPropertyValue(1);
    set
    {
      if ((double) value == (double) this.Left)
        return;
      this.SetPropertyValue(1, (object) value);
    }
  }

  public float Right
  {
    get => (float) this.GetPropertyValue(2);
    set
    {
      if ((double) value == (double) this.Right)
        return;
      this.SetPropertyValue(2, (object) value);
    }
  }

  public float Top
  {
    get => (float) this.GetPropertyValue(3);
    set
    {
      if ((double) value == (double) this.Top)
        return;
      this.SetPropertyValue(3, (object) value);
    }
  }

  public float Bottom
  {
    get => (float) this.GetPropertyValue(4);
    set
    {
      if ((double) value == (double) this.Bottom)
        return;
      this.SetPropertyValue(4, (object) value);
    }
  }

  private bool IsAll
  {
    get
    {
      return (double) this.Left == (double) this.Right && (double) this.Right == (double) this.Top && (double) this.Top == (double) this.Bottom;
    }
  }

  internal float Gutter
  {
    get => (float) this.GetPropertyValue(5);
    set
    {
      if ((double) value == (double) this.Gutter)
        return;
      this.SetPropertyValue(5, (object) value);
    }
  }

  public MarginsF()
  {
  }

  public MarginsF(float left, float top, float right, float bottom)
  {
    this.Left = left;
    this.Top = top;
    this.Right = right;
    this.Bottom = bottom;
  }

  public MarginsF Clone() => new MarginsF(this.Left, this.Top, this.Right, this.Bottom);

  protected override object GetDefValue(int key)
  {
    switch (key)
    {
      case 1:
      case 2:
      case 3:
      case 4:
        return (object) 20f;
      case 5:
        return (object) 0.0f;
      default:
        throw new ArgumentNullException("key not found");
    }
  }

  internal void SetOldPropertyHashMarginValues(
    float left,
    float top,
    float right,
    float bottom,
    float gutter)
  {
    this.SetPropertyValue(1, (object) left);
    this.SetPropertyValue(2, (object) right);
    this.SetPropertyValue(3, (object) top);
    this.SetPropertyValue(4, (object) bottom);
    this.SetPropertyValue(5, (object) gutter);
  }

  internal object GetPropertyValue(int propKey) => this[propKey];

  internal void SetPropertyValue(int propKey, object value) => this[propKey] = value;

  internal bool Compare(MarginsF marginsF)
  {
    return this.Compare(3, (FormatBase) marginsF) && this.Compare(4, (FormatBase) marginsF) && this.Compare(1, (FormatBase) marginsF) && this.Compare(2, (FormatBase) marginsF) && this.Compare(5, (FormatBase) marginsF);
  }
}
