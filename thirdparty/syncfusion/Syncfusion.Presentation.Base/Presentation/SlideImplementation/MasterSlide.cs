// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideImplementation.MasterSlide
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.RichText;
using Syncfusion.Presentation.Themes;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.SlideImplementation;

internal class MasterSlide : BaseSlide, IMasterSlide, IBaseSlide
{
  private Dictionary<string, string> _layoutList;
  private Dictionary<string, bool> _headerFooter;
  private Syncfusion.Presentation.SlideImplementation.LayoutSlides _layoutSlideCollection;
  private string _masterId;
  private Theme _theme;
  private TextBody _titleStyle;
  private TextBody _bodyStyle;
  private TextBody _otherStyle;
  private bool _merged;
  private Dictionary<string, string> _oldLayoutList;
  private string _mergedMasterId;

  internal MasterSlide(Syncfusion.Presentation.Presentation presentation, string masterId)
    : base(presentation)
  {
    this._masterId = masterId;
    this._theme = new Theme(this);
    this._layoutSlideCollection = new Syncfusion.Presentation.SlideImplementation.LayoutSlides((object) this);
  }

  public ILayoutSlides LayoutSlides => (ILayoutSlides) this._layoutSlideCollection;

  internal string MasterId
  {
    get => this._masterId;
    set => this._masterId = value;
  }

  internal Dictionary<string, string> OldLayoutList
  {
    get => this._oldLayoutList;
    set => this._oldLayoutList = value;
  }

  internal Dictionary<string, bool> HeaderFooter
  {
    get => this._headerFooter;
    set => this._headerFooter = value;
  }

  internal Dictionary<string, string> LayoutList
  {
    get => this._layoutList ?? (this._layoutList = new Dictionary<string, string>());
    set => this._layoutList = value;
  }

  internal bool Merged
  {
    get => this._merged;
    set => this._merged = value;
  }

  internal string MergedMasterId
  {
    get => this._mergedMasterId;
    set => this._mergedMasterId = value;
  }

  internal Theme Theme
  {
    get => this._theme;
    set => this._theme = value;
  }

  internal TextBody TitleStyle
  {
    get => this._titleStyle ?? (this._titleStyle = new TextBody((BaseSlide) this));
  }

  internal TextBody BodyStyle
  {
    get => this._bodyStyle ?? (this._bodyStyle = new TextBody((BaseSlide) this));
  }

  internal TextBody OtherStyle
  {
    get => this._otherStyle ?? (this._otherStyle = new TextBody((BaseSlide) this));
  }

  internal override void Close()
  {
    base.Close();
    if (this._layoutSlideCollection != null)
    {
      this._layoutSlideCollection.Close();
      this._layoutSlideCollection = (Syncfusion.Presentation.SlideImplementation.LayoutSlides) null;
    }
    if (this._layoutList != null)
    {
      this._layoutList.Clear();
      this._layoutList = (Dictionary<string, string>) null;
    }
    if (this._theme != null)
    {
      this._theme.Close();
      this._theme = (Theme) null;
    }
    if (this._titleStyle != null)
    {
      this._titleStyle.Close();
      this._titleStyle = (TextBody) null;
    }
    if (this._bodyStyle != null)
    {
      this._bodyStyle.Close();
      this._bodyStyle = (TextBody) null;
    }
    if (this._otherStyle == null)
      return;
    this._otherStyle.Close();
    this._otherStyle = (TextBody) null;
  }

  public MasterSlide Clone()
  {
    MasterSlide masterSlide = (MasterSlide) this.MemberwiseClone();
    this.Clone((BaseSlide) masterSlide);
    masterSlide.ColorMap = Helper.CloneDictionary(this.ColorMap);
    masterSlide._bodyStyle = this._bodyStyle.Clone();
    masterSlide._bodyStyle.SetParent((BaseSlide) masterSlide);
    masterSlide._layoutList = Helper.CloneDictionary(this._layoutList);
    masterSlide._titleStyle = this._titleStyle.Clone();
    masterSlide._titleStyle.SetParent((BaseSlide) masterSlide);
    masterSlide._otherStyle = this._otherStyle.Clone();
    masterSlide._otherStyle.SetParent((BaseSlide) masterSlide);
    masterSlide._theme = this._theme.Clone();
    masterSlide._theme.SetParent((BaseSlide) masterSlide);
    masterSlide._layoutSlideCollection = this._layoutSlideCollection.Clone();
    masterSlide._layoutSlideCollection.SetParent(masterSlide);
    return masterSlide;
  }

  internal override void SetParent(Syncfusion.Presentation.Presentation presentation)
  {
    base.SetParent(presentation);
    this._bodyStyle.SetParent(presentation);
    this._titleStyle.SetParent(presentation);
    this._otherStyle.SetParent(presentation);
    this._theme.SetParent(presentation);
    this._layoutSlideCollection.SetParent(presentation);
  }
}
