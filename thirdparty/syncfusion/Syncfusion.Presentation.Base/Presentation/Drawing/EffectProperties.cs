// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Drawing.EffectProperties
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.Drawing;

internal class EffectProperties
{
  private EffectList _effectList;

  public EffectProperties(Syncfusion.Presentation.Presentation presentation)
  {
    this._effectList = new EffectList(presentation);
  }

  internal EffectList EffectList
  {
    get => this._effectList;
    set => this._effectList = value;
  }

  internal void ClearAll()
  {
    if (this._effectList == null)
      return;
    this._effectList.Close();
    this._effectList = (EffectList) null;
  }

  public EffectProperties Clone()
  {
    EffectProperties effectProperties = (EffectProperties) this.MemberwiseClone();
    effectProperties._effectList = this._effectList.Clone();
    return effectProperties;
  }

  internal void SetParent(Syncfusion.Presentation.Presentation newParent)
  {
    this._effectList.SetParent(newParent);
  }
}
