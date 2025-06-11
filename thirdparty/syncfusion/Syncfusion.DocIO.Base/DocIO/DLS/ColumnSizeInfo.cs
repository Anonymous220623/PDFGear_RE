// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ColumnSizeInfo
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class ColumnSizeInfo
{
  private float minimumWordWidth;
  private float maximumWordWidth;
  private float minimumWidth;
  private float maxParaWidth;
  private bool hasMinimumWidth;
  private bool hasMinimumWordWidth;
  private bool hasMaximumWordWidth;

  internal bool HasMinimumWidth
  {
    get => this.hasMinimumWidth;
    set => this.hasMinimumWidth = value;
  }

  internal bool HasMinimumMaximumWordWidth
  {
    get => this.hasMinimumWordWidth;
    set => this.hasMinimumWordWidth = value;
  }

  internal bool HasMaximumWordWidth
  {
    get => this.hasMaximumWordWidth;
    set => this.hasMaximumWordWidth = value;
  }

  internal float MinimumWordWidth
  {
    get => this.minimumWordWidth;
    set
    {
      this.minimumWordWidth = value;
      this.HasMinimumMaximumWordWidth = true;
    }
  }

  internal float MaximumWordWidth
  {
    get => this.maximumWordWidth;
    set
    {
      this.maximumWordWidth = value;
      this.HasMinimumMaximumWordWidth = true;
    }
  }

  internal float MinimumWidth
  {
    get => this.minimumWidth;
    set
    {
      this.minimumWidth = value;
      this.HasMinimumWidth = true;
    }
  }

  internal float MaxParaWidth
  {
    get => this.maxParaWidth;
    set => this.maxParaWidth = value;
  }
}
