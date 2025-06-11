// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideTransition.Internal.TransitionInternal
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

#nullable disable
namespace Syncfusion.Presentation.SlideTransition.Internal;

internal class TransitionInternal
{
  private SlideShowTransition _slideShowTransition;
  private string _transitionNameSpace;
  private bool _isAdvOnTime;
  private Blinds _blinds;
  private Checker _checker;
  private Circle _circle;
  private Comb _comb;
  private Cover _cover;
  private Cut _cut;
  private Diamond _diamond;
  private Dissolve _dissolve;
  private Fade _fade;
  private NewsFlash _newsFlash;
  private Plus _plus;
  private Pull _pull;
  private Push _push;
  private Random _random;
  private RandomBar _randomBar;
  private Split _split;
  private Strips _strips;
  private Wedge _wedge;
  private Wheel _wheel;
  private Wipe _wipe;
  private Zoom _zoom;
  private Reveal _reveal;
  private HoneyComb _honeyComb;
  private Ferris _ferris;
  private Switch _switch;
  private Flip _flip;
  private Flash _flash;
  private Shred _shred;
  private Prism _prism;
  private Pan _pan;
  private PrstTrans _prstTrans;
  private Vortex _vortex;
  private Ripple _ripple;
  private WheelReverse _wheelReverse;
  private Glitter _glitter;
  private Gallery _gallery;
  private Conveyor _conveyor;
  private Doors _door;
  private Window _window;
  private FlyThrough _flyThrough;
  private Warp _warp;
  private Morph _morph;

  internal SlideShowTransition SlideShowTransition
  {
    get => this._slideShowTransition;
    set => this._slideShowTransition = value;
  }

  internal string TransitionNameSpace
  {
    get => this._transitionNameSpace;
    set => this._transitionNameSpace = value;
  }

  internal bool IsAdvanceOnTime
  {
    get => this._isAdvOnTime;
    set => this._isAdvOnTime = value;
  }

  internal Blinds Blinds
  {
    get => this._blinds;
    set => this._blinds = value;
  }

  internal Checker Checker
  {
    get => this._checker;
    set => this._checker = value;
  }

  internal Circle Circle
  {
    get => this._circle;
    set => this._circle = value;
  }

  internal Comb Comb
  {
    get => this._comb;
    set => this._comb = value;
  }

  internal Cover Cover
  {
    get => this._cover;
    set => this._cover = value;
  }

  internal Cut Cut
  {
    get => this._cut;
    set => this._cut = value;
  }

  internal Diamond Diamond
  {
    get => this._diamond;
    set => this._diamond = value;
  }

  internal Dissolve Dissolve
  {
    get => this._dissolve;
    set => this._dissolve = value;
  }

  internal Fade Fade
  {
    get => this._fade;
    set => this._fade = value;
  }

  internal NewsFlash NewsFlash
  {
    get => this._newsFlash;
    set => this._newsFlash = value;
  }

  internal Plus Plus
  {
    get => this._plus;
    set => this._plus = value;
  }

  internal Pull Pull
  {
    get => this._pull;
    set => this._pull = value;
  }

  internal Push Push
  {
    get => this._push;
    set => this._push = value;
  }

  internal Random Random
  {
    get => this._random;
    set => this._random = value;
  }

  internal RandomBar RandomBar
  {
    get => this._randomBar;
    set => this._randomBar = value;
  }

  internal Split Split
  {
    get => this._split;
    set => this._split = value;
  }

  internal Strips Strips
  {
    get => this._strips;
    set => this._strips = value;
  }

  internal Wedge Wedge
  {
    get => this._wedge;
    set => this._wedge = value;
  }

  internal Wheel Wheel
  {
    get => this._wheel;
    set => this._wheel = value;
  }

  internal Wipe Wipe
  {
    get => this._wipe;
    set => this._wipe = value;
  }

  internal Zoom Zoom
  {
    get => this._zoom;
    set => this._zoom = value;
  }

  internal Reveal Reveal
  {
    get => this._reveal;
    set => this._reveal = value;
  }

  internal HoneyComb HoneyComb
  {
    get => this._honeyComb;
    set => this._honeyComb = value;
  }

  internal Ferris Ferris
  {
    get => this._ferris;
    set => this._ferris = value;
  }

  internal Switch Switch
  {
    get => this._switch;
    set => this._switch = value;
  }

  internal Flip Flip
  {
    get => this._flip;
    set => this._flip = value;
  }

  internal Flash Flash
  {
    get => this._flash;
    set => this._flash = value;
  }

  internal Shred Shred
  {
    get => this._shred;
    set => this._shred = value;
  }

  internal Prism Prism
  {
    get => this._prism;
    set => this._prism = value;
  }

  internal Pan Pan
  {
    get => this._pan;
    set => this._pan = value;
  }

  internal PrstTrans PrstTrans
  {
    get => this._prstTrans;
    set => this._prstTrans = value;
  }

  internal WheelReverse WheelReverse
  {
    get => this._wheelReverse;
    set => this._wheelReverse = value;
  }

  internal Vortex Vortex
  {
    get => this._vortex;
    set => this._vortex = value;
  }

  internal Ripple Ripple
  {
    get => this._ripple;
    set => this._ripple = value;
  }

  internal Glitter Glitter
  {
    get => this._glitter;
    set => this._glitter = value;
  }

  internal Gallery Gallery
  {
    get => this._gallery;
    set => this._gallery = value;
  }

  internal Conveyor Conveyor
  {
    get => this._conveyor;
    set => this._conveyor = value;
  }

  internal Doors Door
  {
    get => this._door;
    set => this._door = value;
  }

  internal Window Window
  {
    get => this._window;
    set => this._window = value;
  }

  internal Warp Warp
  {
    get => this._warp;
    set => this._warp = value;
  }

  internal FlyThrough Flythrough
  {
    get => this._flyThrough;
    set => this._flyThrough = value;
  }

  internal Morph Morph
  {
    get => this._morph;
    set => this._morph = value;
  }
}
