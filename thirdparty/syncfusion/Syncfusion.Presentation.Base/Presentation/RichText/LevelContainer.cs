// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.RichText.LevelContainer
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.RichText;

internal struct LevelContainer
{
  private int _number;

  internal LevelContainer(int number) => this._number = number;

  public LevelContainer(LevelContainer levelContainer) => this._number = levelContainer._number;

  internal int Number
  {
    get => this._number;
    set => this._number = value;
  }

  internal LevelContainer Update()
  {
    ++this._number;
    return this;
  }

  internal LevelContainer Clear()
  {
    this._number = 0;
    return this;
  }

  public LevelContainer Clone() => new LevelContainer(this);
}
