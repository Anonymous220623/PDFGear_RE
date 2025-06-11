// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.Animation.AnimationConstant
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Animation.Internal;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Presentation.Animation;

internal class AnimationConstant
{
  internal static Dictionary<EffectType, Dictionary<float, EffectSubtype>> GetSubTypeDictionary()
  {
    return new Dictionary<EffectType, Dictionary<float, EffectSubtype>>()
    {
      {
        EffectType.Appear,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.CurveUpDown,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Ascend,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Blast,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Blinds,
        new Dictionary<float, EffectSubtype>()
        {
          {
            10f,
            EffectSubtype.Horizontal
          },
          {
            5f,
            EffectSubtype.Vertical
          }
        }
      },
      {
        EffectType.Blink,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Boomerang,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Bounce,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.BoldFlash,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.BoldReveal,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.BrushOnColor,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.BrushOnUnderline,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Box,
        new Dictionary<float, EffectSubtype>()
        {
          {
            16f,
            EffectSubtype.In
          },
          {
            32f,
            EffectSubtype.Out
          }
        }
      },
      {
        EffectType.CenterRevolve,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.ChangeFillColor,
        new Dictionary<float, EffectSubtype>()
        {
          {
            6f,
            EffectSubtype.GradualAndCycleClockwise
          },
          {
            10f,
            EffectSubtype.GradualAndCycleCounterClockwise
          },
          {
            2f,
            EffectSubtype.Gradual
          },
          {
            1f,
            EffectSubtype.Instant
          }
        }
      },
      {
        EffectType.ChangeFont,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.ChangeFontColor,
        new Dictionary<float, EffectSubtype>()
        {
          {
            6f,
            EffectSubtype.GradualAndCycleClockwise
          },
          {
            10f,
            EffectSubtype.GradualAndCycleCounterClockwise
          },
          {
            2f,
            EffectSubtype.Gradual
          },
          {
            1f,
            EffectSubtype.Instant
          }
        }
      },
      {
        EffectType.ChangeFontSize,
        new Dictionary<float, EffectSubtype>()
        {
          {
            2f,
            EffectSubtype.Gradual
          },
          {
            1f,
            EffectSubtype.Instant
          }
        }
      },
      {
        EffectType.ChangeFontStyle,
        new Dictionary<float, EffectSubtype>()
        {
          {
            1f,
            EffectSubtype.FontBold
          },
          {
            0.0f,
            EffectSubtype.FontItalic
          },
          {
            2f,
            EffectSubtype.FontUnderline
          }
        }
      },
      {
        EffectType.ChangeLineColor,
        new Dictionary<float, EffectSubtype>()
        {
          {
            6f,
            EffectSubtype.GradualAndCycleClockwise
          },
          {
            10f,
            EffectSubtype.GradualAndCycleCounterClockwise
          },
          {
            2f,
            EffectSubtype.Gradual
          },
          {
            1f,
            EffectSubtype.Instant
          }
        }
      },
      {
        EffectType.Checkerboard,
        new Dictionary<float, EffectSubtype>()
        {
          {
            10f,
            EffectSubtype.Across
          },
          {
            5f,
            EffectSubtype.Vertical
          }
        }
      },
      {
        EffectType.Circle,
        new Dictionary<float, EffectSubtype>()
        {
          {
            16f,
            EffectSubtype.In
          },
          {
            32f,
            EffectSubtype.Out
          }
        }
      },
      {
        EffectType.ColorBlend,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.ColorTypewriter,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.ColorWave,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.ComplementaryColor,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.ComplementaryColor2,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Compress,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.ContrastingColor,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Crawl,
        new Dictionary<float, EffectSubtype>()
        {
          {
            4f,
            EffectSubtype.Bottom
          },
          {
            8f,
            EffectSubtype.Left
          },
          {
            2f,
            EffectSubtype.Right
          },
          {
            1f,
            EffectSubtype.Top
          }
        }
      },
      {
        EffectType.Credits,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Darken,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Desaturate,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Descend,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Diamond,
        new Dictionary<float, EffectSubtype>()
        {
          {
            16f,
            EffectSubtype.In
          },
          {
            32f,
            EffectSubtype.Out
          }
        }
      },
      {
        EffectType.Dissolve,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.EaseInOut,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Expand,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Fade,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.FadedSwivel,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.FadedZoom,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          },
          {
            528f,
            EffectSubtype.Center
          }
        }
      },
      {
        EffectType.FlashBulb,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.FlashOnce,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Flicker,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Flip,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Float,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Fly,
        new Dictionary<float, EffectSubtype>()
        {
          {
            4f,
            EffectSubtype.Bottom
          },
          {
            12f,
            EffectSubtype.BottomLeft
          },
          {
            6f,
            EffectSubtype.BottomRight
          },
          {
            8f,
            EffectSubtype.Left
          },
          {
            2f,
            EffectSubtype.Right
          },
          {
            1f,
            EffectSubtype.Top
          },
          {
            9f,
            EffectSubtype.TopLeft
          },
          {
            3f,
            EffectSubtype.TopRight
          }
        }
      },
      {
        EffectType.Fold,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Glide,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.GrowAndTurn,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.GrowShrink,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.GrowWithColor,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Lighten,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.LightSpeed,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Peek,
        new Dictionary<float, EffectSubtype>()
        {
          {
            4f,
            EffectSubtype.Bottom
          },
          {
            8f,
            EffectSubtype.Left
          },
          {
            2f,
            EffectSubtype.Right
          },
          {
            1f,
            EffectSubtype.Top
          }
        }
      },
      {
        EffectType.Pinwheel,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Plus,
        new Dictionary<float, EffectSubtype>()
        {
          {
            16f,
            EffectSubtype.In
          },
          {
            32f,
            EffectSubtype.Out
          }
        }
      },
      {
        EffectType.Path4PointStar,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.Path5PointStar,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.Path6PointStar,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.Path8PointStar,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathArcDown,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathArcLeft,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathArcRight,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathArcUp,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathBean,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathBounceLeft,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathBounceRight,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathBuzzsaw,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathCircle,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathCrescentMoon,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathCurvedSquare,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathCurvedX,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathCurvyLeft,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathCurvyRight,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathCurvyStar,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathDecayingWave,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathDiagonalDownRight,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathDiagonalUpRight,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathDiamond,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathDown,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathEqualTriangle,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathFigure8Four,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathFootball,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathFunnel,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathHeart,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathHeartbeat,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathHexagon,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathHorizontalFigure8,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathInvertedSquare,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathInvertedTriangle,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathLeft,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathLoopdeLoop,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathNeutron,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathOctagon,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathParallelogram,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathPeanut,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathPentagon,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathPlus,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathPointyStar,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathRight,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathRightTriangle,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathSCurve1,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathSCurve2,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathSineWave,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathSpiralLeft,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathSpiralRight,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathSpring,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathSquare,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathStairsDown,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathSwoosh,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathTeardrop,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathTrapezoid,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathTurnDown,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathTurnRight,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathTurnUp,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathTurnUpRight,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathUp,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathUser,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathVerticalFigure8,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathWave,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.PathZigzag,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.Custom,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.RandomEffects,
        new Dictionary<float, EffectSubtype>()
      },
      {
        EffectType.RandomBars,
        new Dictionary<float, EffectSubtype>()
        {
          {
            10f,
            EffectSubtype.Horizontal
          },
          {
            5f,
            EffectSubtype.Vertical
          }
        }
      },
      {
        EffectType.RiseUp,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Shimmer,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Sling,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Spin,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Spinner,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Spiral,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Split,
        new Dictionary<float, EffectSubtype>()
        {
          {
            26f,
            EffectSubtype.HorizontalIn
          },
          {
            42f,
            EffectSubtype.HorizontalOut
          },
          {
            21f,
            EffectSubtype.VerticalIn
          },
          {
            37f,
            EffectSubtype.VerticalOut
          }
        }
      },
      {
        EffectType.Stretch,
        new Dictionary<float, EffectSubtype>()
        {
          {
            10f,
            EffectSubtype.Across
          },
          {
            4f,
            EffectSubtype.Bottom
          },
          {
            8f,
            EffectSubtype.Left
          },
          {
            2f,
            EffectSubtype.Right
          },
          {
            1f,
            EffectSubtype.Top
          }
        }
      },
      {
        EffectType.Strips,
        new Dictionary<float, EffectSubtype>()
        {
          {
            12f,
            EffectSubtype.DownLeft
          },
          {
            6f,
            EffectSubtype.DownRight
          },
          {
            9f,
            EffectSubtype.UpLeft
          },
          {
            3f,
            EffectSubtype.UpRight
          }
        }
      },
      {
        EffectType.StyleEmphasis,
        new Dictionary<float, EffectSubtype>()
        {
          {
            12f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Swish,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Swivel,
        new Dictionary<float, EffectSubtype>()
        {
          {
            10f,
            EffectSubtype.Horizontal
          },
          {
            5f,
            EffectSubtype.Vertical
          }
        }
      },
      {
        EffectType.Teeter,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Thread,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Transparency,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Unfold,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.VerticalGrow,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Wave,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Wedge,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Wheel,
        new Dictionary<float, EffectSubtype>()
        {
          {
            1f,
            EffectSubtype.Wheel1
          },
          {
            2f,
            EffectSubtype.Wheel2
          },
          {
            3f,
            EffectSubtype.Wheel3
          },
          {
            4f,
            EffectSubtype.Wheel4
          },
          {
            8f,
            EffectSubtype.Wheel8
          }
        }
      },
      {
        EffectType.Whip,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Wipe,
        new Dictionary<float, EffectSubtype>()
        {
          {
            4f,
            EffectSubtype.Bottom
          },
          {
            8f,
            EffectSubtype.Left
          },
          {
            2f,
            EffectSubtype.Right
          },
          {
            1f,
            EffectSubtype.Top
          }
        }
      },
      {
        EffectType.Magnify,
        new Dictionary<float, EffectSubtype>()
        {
          {
            0.0f,
            EffectSubtype.None
          }
        }
      },
      {
        EffectType.Zoom,
        new Dictionary<float, EffectSubtype>()
        {
          {
            16f,
            EffectSubtype.In
          },
          {
            20f,
            EffectSubtype.InBottom
          },
          {
            528f,
            EffectSubtype.InCenter
          },
          {
            272f,
            EffectSubtype.InSlightly
          },
          {
            32f,
            EffectSubtype.Out
          },
          {
            36f,
            EffectSubtype.OutBottom
          },
          {
            544f,
            EffectSubtype.OutCenter
          },
          {
            288f,
            EffectSubtype.OutSlightly
          }
        }
      }
    };
  }

  internal static Dictionary<EffectPresetClassType, Dictionary<float, EffectType>> GetEffectTypeDictionary()
  {
    return new Dictionary<EffectPresetClassType, Dictionary<float, EffectType>>()
    {
      {
        EffectPresetClassType.Entrance,
        new Dictionary<float, EffectType>()
        {
          {
            1f,
            EffectType.Appear
          },
          {
            52f,
            EffectType.CurveUpDown
          },
          {
            42f,
            EffectType.Ascend
          },
          {
            3f,
            EffectType.Blinds
          },
          {
            25f,
            EffectType.Boomerang
          },
          {
            26f,
            EffectType.Bounce
          },
          {
            4f,
            EffectType.Box
          },
          {
            43f,
            EffectType.CenterRevolve
          },
          {
            5f,
            EffectType.Checkerboard
          },
          {
            6f,
            EffectType.Circle
          },
          {
            27f,
            EffectType.ColorTypewriter
          },
          {
            50f,
            EffectType.Compress
          },
          {
            7f,
            EffectType.Crawl
          },
          {
            28f,
            EffectType.Credits
          },
          {
            47f,
            EffectType.Descend
          },
          {
            8f,
            EffectType.Diamond
          },
          {
            9f,
            EffectType.Dissolve
          },
          {
            29f,
            EffectType.EaseInOut
          },
          {
            55f,
            EffectType.Expand
          },
          {
            10f,
            EffectType.Fade
          },
          {
            45f,
            EffectType.FadedSwivel
          },
          {
            53f,
            EffectType.FadedZoom
          },
          {
            11f,
            EffectType.FlashOnce
          },
          {
            56f,
            EffectType.Flip
          },
          {
            30f,
            EffectType.Float
          },
          {
            2f,
            EffectType.Fly
          },
          {
            58f,
            EffectType.Fold
          },
          {
            54f,
            EffectType.Glide
          },
          {
            31f,
            EffectType.GrowAndTurn
          },
          {
            34f,
            EffectType.LightSpeed
          },
          {
            12f,
            EffectType.Peek
          },
          {
            35f,
            EffectType.Pinwheel
          },
          {
            13f,
            EffectType.Plus
          },
          {
            14f,
            EffectType.RandomBars
          },
          {
            24f,
            EffectType.RandomEffects
          },
          {
            37f,
            EffectType.RiseUp
          },
          {
            48f,
            EffectType.Sling
          },
          {
            49f,
            EffectType.Spinner
          },
          {
            15f,
            EffectType.Spiral
          },
          {
            16f,
            EffectType.Split
          },
          {
            17f,
            EffectType.Stretch
          },
          {
            18f,
            EffectType.Strips
          },
          {
            38f,
            EffectType.Swish
          },
          {
            19f,
            EffectType.Swivel
          },
          {
            39f,
            EffectType.Thread
          },
          {
            40f,
            EffectType.Unfold
          },
          {
            20f,
            EffectType.Wedge
          },
          {
            21f,
            EffectType.Wheel
          },
          {
            41f,
            EffectType.Whip
          },
          {
            22f,
            EffectType.Wipe
          },
          {
            23f,
            EffectType.Zoom
          },
          {
            51f,
            EffectType.Magnify
          },
          {
            0.0f,
            EffectType.Appear
          }
        }
      },
      {
        EffectPresetClassType.Emphasis,
        new Dictionary<float, EffectType>()
        {
          {
            1f,
            EffectType.ChangeFillColor
          },
          {
            2f,
            EffectType.ChangeFont
          },
          {
            3f,
            EffectType.ChangeFontColor
          },
          {
            4f,
            EffectType.ChangeFontSize
          },
          {
            5f,
            EffectType.ChangeFontStyle
          },
          {
            6f,
            EffectType.GrowShrink
          },
          {
            7f,
            EffectType.ChangeLineColor
          },
          {
            8f,
            EffectType.Spin
          },
          {
            9f,
            EffectType.Transparency
          },
          {
            10f,
            EffectType.BoldFlash
          },
          {
            14f,
            EffectType.Blast
          },
          {
            15f,
            EffectType.BoldReveal
          },
          {
            16f,
            EffectType.BrushOnColor
          },
          {
            18f,
            EffectType.BrushOnUnderline
          },
          {
            19f,
            EffectType.ColorBlend
          },
          {
            20f,
            EffectType.ColorWave
          },
          {
            21f,
            EffectType.ComplementaryColor
          },
          {
            22f,
            EffectType.ComplementaryColor2
          },
          {
            23f,
            EffectType.ContrastingColor
          },
          {
            24f,
            EffectType.Darken
          },
          {
            25f,
            EffectType.Desaturate
          },
          {
            26f,
            EffectType.FlashBulb
          },
          {
            27f,
            EffectType.Flicker
          },
          {
            28f,
            EffectType.GrowWithColor
          },
          {
            30f,
            EffectType.Lighten
          },
          {
            31f,
            EffectType.StyleEmphasis
          },
          {
            32f,
            EffectType.Teeter
          },
          {
            33f,
            EffectType.VerticalGrow
          },
          {
            34f,
            EffectType.Wave
          },
          {
            35f,
            EffectType.Blink
          },
          {
            36f,
            EffectType.Shimmer
          },
          {
            0.0f,
            EffectType.Custom
          }
        }
      },
      {
        EffectPresetClassType.Path,
        new Dictionary<float, EffectType>()
        {
          {
            16f,
            EffectType.Path4PointStar
          },
          {
            5f,
            EffectType.Path5PointStar
          },
          {
            11f,
            EffectType.Path6PointStar
          },
          {
            17f,
            EffectType.Path8PointStar
          },
          {
            37f,
            EffectType.PathArcDown
          },
          {
            51f,
            EffectType.PathArcLeft
          },
          {
            58f,
            EffectType.PathArcRight
          },
          {
            44f,
            EffectType.PathArcUp
          },
          {
            31f,
            EffectType.PathBean
          },
          {
            41f,
            EffectType.PathBounceLeft
          },
          {
            54f,
            EffectType.PathBounceRight
          },
          {
            25f,
            EffectType.PathBuzzsaw
          },
          {
            1f,
            EffectType.PathCircle
          },
          {
            6f,
            EffectType.PathCrescentMoon
          },
          {
            20f,
            EffectType.PathCurvedSquare
          },
          {
            21f,
            EffectType.PathCurvedX
          },
          {
            48f,
            EffectType.PathCurvyLeft
          },
          {
            61f,
            EffectType.PathCurvyRight
          },
          {
            23f,
            EffectType.PathCurvyStar
          },
          {
            60f,
            EffectType.PathDecayingWave
          },
          {
            49f,
            EffectType.PathDiagonalDownRight
          },
          {
            56f,
            EffectType.PathDiagonalUpRight
          },
          {
            3f,
            EffectType.PathDiamond
          },
          {
            42f,
            EffectType.PathDown
          },
          {
            13f,
            EffectType.PathEqualTriangle
          },
          {
            28f,
            EffectType.PathFigure8Four
          },
          {
            12f,
            EffectType.PathFootball
          },
          {
            52f,
            EffectType.PathFunnel
          },
          {
            9f,
            EffectType.PathHeart
          },
          {
            45f,
            EffectType.PathHeartbeat
          },
          {
            4f,
            EffectType.PathHexagon
          },
          {
            26f,
            EffectType.PathHorizontalFigure8
          },
          {
            34f,
            EffectType.PathInvertedSquare
          },
          {
            33f,
            EffectType.PathInvertedTriangle
          },
          {
            35f,
            EffectType.PathLeft
          },
          {
            24f,
            EffectType.PathLoopdeLoop
          },
          {
            29f,
            EffectType.PathNeutron
          },
          {
            10f,
            EffectType.PathOctagon
          },
          {
            14f,
            EffectType.PathParallelogram
          },
          {
            27f,
            EffectType.PathPeanut
          },
          {
            15f,
            EffectType.PathPentagon
          },
          {
            32f,
            EffectType.PathPlus
          },
          {
            19f,
            EffectType.PathPointyStar
          },
          {
            63f,
            EffectType.PathRight
          },
          {
            2f,
            EffectType.PathRightTriangle
          },
          {
            59f,
            EffectType.PathSCurve1
          },
          {
            39f,
            EffectType.PathSCurve2
          },
          {
            40f,
            EffectType.PathSineWave
          },
          {
            55f,
            EffectType.PathSpiralLeft
          },
          {
            46f,
            EffectType.PathSpiralRight
          },
          {
            53f,
            EffectType.PathSpring
          },
          {
            7f,
            EffectType.PathSquare
          },
          {
            62f,
            EffectType.PathStairsDown
          },
          {
            30f,
            EffectType.PathSwoosh
          },
          {
            18f,
            EffectType.PathTeardrop
          },
          {
            8f,
            EffectType.PathTrapezoid
          },
          {
            50f,
            EffectType.PathTurnDown
          },
          {
            36f,
            EffectType.PathTurnRight
          },
          {
            43f,
            EffectType.PathTurnUp
          },
          {
            57f,
            EffectType.PathTurnUpRight
          },
          {
            64f,
            EffectType.PathUp
          },
          {
            0.0f,
            EffectType.PathUser
          },
          {
            22f,
            EffectType.PathVerticalFigure8
          },
          {
            47f,
            EffectType.PathWave
          },
          {
            38f,
            EffectType.PathZigzag
          }
        }
      },
      {
        EffectPresetClassType.Exit,
        new Dictionary<float, EffectType>()
        {
          {
            1f,
            EffectType.Appear
          },
          {
            52f,
            EffectType.CurveUpDown
          },
          {
            42f,
            EffectType.Ascend
          },
          {
            3f,
            EffectType.Blinds
          },
          {
            25f,
            EffectType.Boomerang
          },
          {
            26f,
            EffectType.Bounce
          },
          {
            4f,
            EffectType.Box
          },
          {
            43f,
            EffectType.CenterRevolve
          },
          {
            5f,
            EffectType.Checkerboard
          },
          {
            6f,
            EffectType.Circle
          },
          {
            27f,
            EffectType.ColorTypewriter
          },
          {
            50f,
            EffectType.Compress
          },
          {
            7f,
            EffectType.Crawl
          },
          {
            28f,
            EffectType.Credits
          },
          {
            47f,
            EffectType.Descend
          },
          {
            8f,
            EffectType.Diamond
          },
          {
            9f,
            EffectType.Dissolve
          },
          {
            29f,
            EffectType.EaseInOut
          },
          {
            55f,
            EffectType.Expand
          },
          {
            10f,
            EffectType.Fade
          },
          {
            45f,
            EffectType.FadedSwivel
          },
          {
            53f,
            EffectType.FadedZoom
          },
          {
            11f,
            EffectType.FlashOnce
          },
          {
            56f,
            EffectType.Flip
          },
          {
            30f,
            EffectType.Float
          },
          {
            2f,
            EffectType.Fly
          },
          {
            58f,
            EffectType.Fold
          },
          {
            54f,
            EffectType.Glide
          },
          {
            31f,
            EffectType.GrowAndTurn
          },
          {
            34f,
            EffectType.LightSpeed
          },
          {
            12f,
            EffectType.Peek
          },
          {
            35f,
            EffectType.Pinwheel
          },
          {
            13f,
            EffectType.Plus
          },
          {
            14f,
            EffectType.RandomBars
          },
          {
            24f,
            EffectType.RandomEffects
          },
          {
            37f,
            EffectType.RiseUp
          },
          {
            48f,
            EffectType.Sling
          },
          {
            49f,
            EffectType.Spinner
          },
          {
            15f,
            EffectType.Spiral
          },
          {
            16f,
            EffectType.Split
          },
          {
            17f,
            EffectType.Stretch
          },
          {
            18f,
            EffectType.Strips
          },
          {
            38f,
            EffectType.Swish
          },
          {
            19f,
            EffectType.Swivel
          },
          {
            39f,
            EffectType.Thread
          },
          {
            40f,
            EffectType.Unfold
          },
          {
            20f,
            EffectType.Wedge
          },
          {
            21f,
            EffectType.Wheel
          },
          {
            41f,
            EffectType.Whip
          },
          {
            22f,
            EffectType.Wipe
          },
          {
            23f,
            EffectType.Zoom
          },
          {
            51f,
            EffectType.Magnify
          },
          {
            0.0f,
            EffectType.Appear
          }
        }
      }
    };
  }

  internal static bool IsNotNullOrEmpty(string val) => !string.IsNullOrEmpty(val);

  internal static PointF[] CheckIsValidCmdPath(MotionCommandPathType type, PointF[] points)
  {
    switch (type)
    {
      case MotionCommandPathType.MoveTo:
        if (points.Length != 1 && points.Length <= 1)
          throw new Exception($"One point needed to make the {(object) type} as possible");
        if (points.Length > 1)
        {
          List<PointF> pointFList = new List<PointF>((IEnumerable<PointF>) points);
          pointFList.RemoveRange(1, points.Length);
          points = pointFList.ToArray();
          break;
        }
        break;
      case MotionCommandPathType.LineTo:
        if (points.Length != 1 && points.Length <= 1)
          throw new Exception($"One point needed to make the {(object) type} as possible");
        if (points.Length > 1)
        {
          List<PointF> pointFList = new List<PointF>((IEnumerable<PointF>) points);
          pointFList.RemoveRange(1, points.Length - 1);
          points = pointFList.ToArray();
          break;
        }
        break;
      case MotionCommandPathType.CurveTo:
        if (points.Length != 3 && points.Length <= 3)
          throw new Exception($"Three points needed to make the {(object) type} as possible");
        if (points.Length > 3)
        {
          List<PointF> pointFList = new List<PointF>((IEnumerable<PointF>) points);
          pointFList.RemoveRange(3, points.Length - 3);
          points = pointFList.ToArray();
          break;
        }
        break;
      case MotionCommandPathType.CloseLoop:
        points = (PointF[]) null;
        break;
      case MotionCommandPathType.End:
        points = (PointF[]) null;
        break;
    }
    return points;
  }

  internal static BehaviorAccumulateType GetAccumulate(bool? nullableBool)
  {
    ref bool? local = ref nullableBool;
    bool valueOrDefault = local.GetValueOrDefault();
    if (local.HasValue)
    {
      switch (valueOrDefault)
      {
        case true:
          return BehaviorAccumulateType.Always;
      }
    }
    return BehaviorAccumulateType.None;
  }

  internal static bool? GetNullableBoolValue(BehaviorAccumulateType accumulateType)
  {
    switch (accumulateType)
    {
      case BehaviorAccumulateType.Always:
        return new bool?(true);
      default:
        return new bool?();
    }
  }

  internal static MotionPathEditMode GetMotionPathEditMode(string motionPath)
  {
    switch (motionPath)
    {
      case "relative":
        return MotionPathEditMode.Relative;
      case "fixed":
        return MotionPathEditMode.Fixed;
      default:
        return MotionPathEditMode.NotDefined;
    }
  }

  internal static char GetMotionPathPointsTypesString(MotionPathPointsType value)
  {
    switch (value)
    {
      case MotionPathPointsType.Auto:
        return 'A';
      case MotionPathPointsType.Corner:
        return 'F';
      case MotionPathPointsType.Straight:
        return 'T';
      case MotionPathPointsType.Smooth:
        return 'S';
      case MotionPathPointsType.CurveAuto:
        return 'a';
      case MotionPathPointsType.CurveCorner:
        return 'f';
      case MotionPathPointsType.CurveStraight:
        return 't';
      case MotionPathPointsType.CurveSmooth:
        return 's';
      default:
        return ' ';
    }
  }

  internal static MotionPathPointsType GetMotionPathPointsTypes(char value)
  {
    switch (value)
    {
      case 'A':
        return MotionPathPointsType.Auto;
      case 'F':
        return MotionPathPointsType.Corner;
      case 'S':
        return MotionPathPointsType.Smooth;
      case 'T':
        return MotionPathPointsType.Straight;
      case 'a':
        return MotionPathPointsType.CurveAuto;
      case 'f':
        return MotionPathPointsType.CurveCorner;
      case 's':
        return MotionPathPointsType.CurveSmooth;
      case 't':
        return MotionPathPointsType.CurveStraight;
      default:
        return MotionPathPointsType.None;
    }
  }

  internal static MotionOriginType GetMotionOriginType(string motionOrigin)
  {
    switch (motionOrigin)
    {
      case "parent":
        return MotionOriginType.Parent;
      case "layout":
        return MotionOriginType.Layout;
      default:
        return MotionOriginType.NotDefined;
    }
  }

  internal static FilterEffectType GetFilterEffectType(string filterType)
  {
    if (filterType.Contains("fade"))
      return FilterEffectType.Fade;
    if (filterType.Contains("blinds"))
      return FilterEffectType.Blinds;
    if (filterType.Contains("box"))
      return FilterEffectType.Box;
    if (filterType.Contains("checkerboard"))
      return FilterEffectType.Checkerboard;
    if (filterType.Contains("circle"))
      return FilterEffectType.Circle;
    if (filterType.Contains("diamond"))
      return FilterEffectType.Diamond;
    if (filterType.Contains("dissolve"))
      return FilterEffectType.Dissolve;
    if (filterType.Contains("slide"))
      return FilterEffectType.Slide;
    if (filterType.Contains("plus"))
      return FilterEffectType.Plus;
    if (filterType.Contains("barn"))
      return FilterEffectType.Barn;
    if (filterType.Contains("randombar"))
      return FilterEffectType.RandomBar;
    if (filterType.Contains("strips"))
      return FilterEffectType.Strips;
    if (filterType.Contains("wedge"))
      return FilterEffectType.Wedge;
    if (filterType.Contains("wheel"))
      return FilterEffectType.Wheel;
    if (filterType.Contains("wipe"))
      return FilterEffectType.Wipe;
    if (filterType.Contains("image"))
      return FilterEffectType.Image;
    if (filterType.Contains("pixelate"))
      return FilterEffectType.Pixelate;
    return filterType.Contains("stretch") ? FilterEffectType.Stretch : FilterEffectType.None;
  }

  internal static string GetFilterEffectSubTypeString(SubtypeFilterEffect subtypeFilterEffect)
  {
    switch (subtypeFilterEffect)
    {
      case SubtypeFilterEffect.Across:
        return "across";
      case SubtypeFilterEffect.Down:
        return "down";
      case SubtypeFilterEffect.DownLeft:
        return "downLeft";
      case SubtypeFilterEffect.DownRight:
        return "downRight";
      case SubtypeFilterEffect.FromBottom:
        return "fromBottom";
      case SubtypeFilterEffect.FromLeft:
        return "fromLeft";
      case SubtypeFilterEffect.FromRight:
        return "fromRight";
      case SubtypeFilterEffect.FromTop:
        return "fromTop";
      case SubtypeFilterEffect.Horizontal:
        return "horizontal";
      case SubtypeFilterEffect.In:
        return "in";
      case SubtypeFilterEffect.InHorizontal:
        return "inHorizontal";
      case SubtypeFilterEffect.InVertical:
        return "inVertical";
      case SubtypeFilterEffect.Left:
        return "left";
      case SubtypeFilterEffect.Out:
        return "out";
      case SubtypeFilterEffect.OutHorizontal:
        return "outHorizontal";
      case SubtypeFilterEffect.OutVertical:
        return "outVertical";
      case SubtypeFilterEffect.Right:
        return "right";
      case SubtypeFilterEffect.Spokes1:
        return "1";
      case SubtypeFilterEffect.Spokes2:
        return "2";
      case SubtypeFilterEffect.Spokes3:
        return "3";
      case SubtypeFilterEffect.Spokes4:
        return "4";
      case SubtypeFilterEffect.Spokes8:
        return "8";
      case SubtypeFilterEffect.Up:
        return "up";
      case SubtypeFilterEffect.UpLeft:
        return "upLeft";
      case SubtypeFilterEffect.UpRight:
        return "upRight";
      case SubtypeFilterEffect.Vertical:
        return "vertical";
      default:
        return "";
    }
  }

  internal static void SetFilterEffectSubType(string value, FilterEffect filterEffect)
  {
    int num1 = value.LastIndexOf('(');
    int num2 = value.LastIndexOf(')');
    value = value.Substring(num1 + 1, num2 - num1 - 1);
    switch (value)
    {
      case "horizontal":
        filterEffect.Subtype = SubtypeFilterEffect.Horizontal;
        break;
      case "vertical":
        filterEffect.Subtype = SubtypeFilterEffect.Vertical;
        break;
      case "in":
        filterEffect.Subtype = SubtypeFilterEffect.In;
        break;
      case "out":
        filterEffect.Subtype = SubtypeFilterEffect.Out;
        break;
      case "across":
        filterEffect.Subtype = SubtypeFilterEffect.Across;
        break;
      case "down":
        filterEffect.Subtype = SubtypeFilterEffect.Down;
        break;
      case "fromTop":
        filterEffect.Subtype = SubtypeFilterEffect.FromTop;
        break;
      case "fromBottom":
        filterEffect.Subtype = SubtypeFilterEffect.FromBottom;
        break;
      case "fromLeft":
        filterEffect.Subtype = SubtypeFilterEffect.FromLeft;
        break;
      case "fromRight":
        filterEffect.Subtype = SubtypeFilterEffect.FromRight;
        break;
      case "inVertical":
        filterEffect.Subtype = SubtypeFilterEffect.InVertical;
        break;
      case "inHorizontal":
        filterEffect.Subtype = SubtypeFilterEffect.InHorizontal;
        break;
      case "outVertical":
        filterEffect.Subtype = SubtypeFilterEffect.OutVertical;
        break;
      case "outHorizontal":
        filterEffect.Subtype = SubtypeFilterEffect.OutHorizontal;
        break;
      case "downLeft":
        filterEffect.Subtype = SubtypeFilterEffect.DownLeft;
        break;
      case "downRight":
        filterEffect.Subtype = SubtypeFilterEffect.DownRight;
        break;
      case "upRight":
        filterEffect.Subtype = SubtypeFilterEffect.UpRight;
        break;
      case "upLeft":
        filterEffect.Subtype = SubtypeFilterEffect.UpLeft;
        break;
      case "1":
        filterEffect.Subtype = SubtypeFilterEffect.Spokes1;
        break;
      case "2":
        filterEffect.Subtype = SubtypeFilterEffect.Spokes2;
        break;
      case "3":
        filterEffect.Subtype = SubtypeFilterEffect.Spokes3;
        break;
      case "4":
        filterEffect.Subtype = SubtypeFilterEffect.Spokes4;
        break;
      case "8":
        filterEffect.Subtype = SubtypeFilterEffect.Spokes8;
        break;
      case "right":
        filterEffect.Subtype = SubtypeFilterEffect.Right;
        break;
      case "left":
        filterEffect.Subtype = SubtypeFilterEffect.Left;
        break;
      case "up":
        filterEffect.Subtype = SubtypeFilterEffect.Up;
        break;
      default:
        filterEffect.Subtype = SubtypeFilterEffect.None;
        break;
    }
  }

  internal static FilterEffectRevealType GetRevealTypeFilterEffect(string filterType)
  {
    switch (filterType)
    {
      case "in":
        return FilterEffectRevealType.In;
      case "out":
        return FilterEffectRevealType.Out;
      default:
        return FilterEffectRevealType.None;
    }
  }

  internal static string GetRevealTypeFilterEffectString(FilterEffectRevealType filterType)
  {
    switch (filterType)
    {
      case FilterEffectRevealType.In:
        return "in";
      case FilterEffectRevealType.Out:
        return "out";
      default:
        return "";
    }
  }

  internal static ColorSpace GetColorSpaceValue(string colorValue)
  {
    switch (colorValue)
    {
      case "hsl":
        return ColorSpace.HSL;
      case "rgb":
        return ColorSpace.RGB;
      default:
        return ColorSpace.NotDefined;
    }
  }

  internal static ColorDirection GetColorDirectionValue(string colordirection)
  {
    switch (colordirection)
    {
      case "ccw":
        return ColorDirection.CounterClockWise;
      case "cw":
        return ColorDirection.ClockWise;
      default:
        return ColorDirection.NotDefined;
    }
  }

  internal static string GetColorDirectionString(ColorDirection colordirection)
  {
    switch (colordirection)
    {
      case ColorDirection.ClockWise:
        return "cw";
      case ColorDirection.CounterClockWise:
        return "ccw";
      default:
        return "";
    }
  }

  internal static PropertyCalcModeType GetCalcPropertyType(string calcMode)
  {
    switch (calcMode)
    {
      case "discrete":
        return PropertyCalcModeType.Discrete;
      case "fmla":
        return PropertyCalcModeType.Formula;
      case "lin":
        return PropertyCalcModeType.Linear;
      default:
        return PropertyCalcModeType.NotDefined;
    }
  }

  internal static string GetCalcPropertyTypeString(PropertyCalcModeType calcMode)
  {
    switch (calcMode)
    {
      case PropertyCalcModeType.Discrete:
        return "discrete";
      case PropertyCalcModeType.Linear:
        return "lin";
      case PropertyCalcModeType.Formula:
        return "fmla";
      default:
        return "";
    }
  }

  internal static PropertyValueType GetPropertyValue(string propertyValue)
  {
    switch (propertyValue)
    {
      case "clr":
        return PropertyValueType.Color;
      case "num":
        return PropertyValueType.Number;
      case "str":
        return PropertyValueType.String;
      default:
        return PropertyValueType.NotDefined;
    }
  }

  internal static string GetPropertyValueString(PropertyValueType propertyValue)
  {
    switch (propertyValue)
    {
      case PropertyValueType.String:
        return "str";
      case PropertyValueType.Number:
        return "num";
      case PropertyValueType.Color:
        return "clr";
      default:
        return "";
    }
  }

  internal static NextAction GetAction(string actionValue)
  {
    switch (actionValue)
    {
      case "seek":
        return NextAction.Seek;
      default:
        return NextAction.None;
    }
  }

  internal static PreviousAction GetPreviousAction(string actionValue)
  {
    switch (actionValue)
    {
      case "skipTimed":
        return PreviousAction.SkipTimed;
      default:
        return PreviousAction.None;
    }
  }

  internal static string GetPreviousActionString(PreviousAction actionValue)
  {
    switch (actionValue)
    {
      case PreviousAction.None:
        return "none";
      case PreviousAction.SkipTimed:
        return "skipTimed";
      default:
        return "";
    }
  }

  internal static TriggerEvent GetTriggerEvent(string triggerEvent)
  {
    switch (triggerEvent)
    {
      case "begin":
        return TriggerEvent.Begin;
      case "end":
        return TriggerEvent.End;
      case "onBegin":
        return TriggerEvent.OnBegin;
      case "onClick":
        return TriggerEvent.OnClick;
      case "onDblClick":
        return TriggerEvent.OnDblClick;
      case "onEnd":
        return TriggerEvent.OnEnd;
      case "onMouseOut":
        return TriggerEvent.OnMouseOut;
      case "onMouseOver":
        return TriggerEvent.OnMouseOver;
      case "onNext":
        return TriggerEvent.OnNext;
      case "onPrev":
        return TriggerEvent.OnPrev;
      case "onStopAudio":
        return TriggerEvent.OnStopAudio;
      default:
        return TriggerEvent.None;
    }
  }

  internal static string GetTriggerEventString(TriggerEvent triggerEvent)
  {
    switch (triggerEvent)
    {
      case TriggerEvent.Begin:
        return "begin";
      case TriggerEvent.End:
        return "end";
      case TriggerEvent.OnBegin:
        return "onBegin";
      case TriggerEvent.OnClick:
        return "onClick";
      case TriggerEvent.OnDblClick:
        return "onDblClick";
      case TriggerEvent.OnEnd:
        return "onEnd";
      case TriggerEvent.OnMouseOut:
        return "onMouseOut";
      case TriggerEvent.OnMouseOver:
        return "onMouseOver";
      case TriggerEvent.OnNext:
        return "onNext";
      case TriggerEvent.OnPrev:
        return "onPrev";
      case TriggerEvent.OnStopAudio:
        return "onStopAudio";
      default:
        return "";
    }
  }

  internal static TimeNodeSync GetTimeNodeSyncValue(string syncValue)
  {
    switch (syncValue)
    {
      case "canSlip":
        return TimeNodeSync.CanSlip;
      case "locked":
        return TimeNodeSync.Locked;
      default:
        return TimeNodeSync.None;
    }
  }

  internal static EffectRestartType GetRestartType(string restartValue)
  {
    switch (restartValue)
    {
      case "always":
        return EffectRestartType.Always;
      case "never":
        return EffectRestartType.Never;
      case "whenNotActive":
        return EffectRestartType.WhenNotActive;
      default:
        return EffectRestartType.NotDefined;
    }
  }

  internal static string GetRestartTypeString(EffectRestartType restartValue)
  {
    switch (restartValue)
    {
      case EffectRestartType.Always:
        return "always";
      case EffectRestartType.WhenNotActive:
        return "whenNotActive";
      case EffectRestartType.Never:
        return "never";
      default:
        return "";
    }
  }

  internal static EffectPresetClassType GetPresetClassType(string presetClassType)
  {
    switch (presetClassType)
    {
      case "emph":
        return EffectPresetClassType.Emphasis;
      case "entr":
        return EffectPresetClassType.Entrance;
      case "exit":
        return EffectPresetClassType.Exit;
      case "path":
        return EffectPresetClassType.Path;
      default:
        return EffectPresetClassType.None;
    }
  }

  internal static string GetPresetClassString(EffectPresetClassType effectPresetClassType)
  {
    switch (effectPresetClassType)
    {
      case EffectPresetClassType.Entrance:
        return "entr";
      case EffectPresetClassType.Exit:
        return "exit";
      case EffectPresetClassType.Emphasis:
        return "emph";
      case EffectPresetClassType.Path:
        return "path";
      default:
        return "";
    }
  }

  internal static AnimationPropertyType GetPropertyType(string typeName)
  {
    switch (typeName)
    {
      case "style.opacity":
        return AnimationPropertyType.Opacity;
      case "style.rotation":
        return AnimationPropertyType.Rotation;
      case "style.visibility":
        return AnimationPropertyType.Visibility;
      case "style.color":
        return AnimationPropertyType.Color;
      case "style.fontSize":
        return AnimationPropertyType.TextFontSize;
      case "style.fontWeight":
        return AnimationPropertyType.TextFontWeight;
      case "style.fontStyle":
        return AnimationPropertyType.TextFontStyle;
      case "style.fontFamily":
        return AnimationPropertyType.TextFontName;
      case "style.textEffectEmboss":
        return AnimationPropertyType.TextFontEmboss;
      case "style.textShadow":
        return AnimationPropertyType.TextFontShadow;
      case "style.textDecorationUnderline":
        return AnimationPropertyType.TextFontUnderline;
      case "style.textDecorationLineThrough":
        return AnimationPropertyType.TextFontStrikeThrough;
      case "imageData.gain":
        return AnimationPropertyType.ShapePictureBrightness;
      case "imageData.blacklevel":
        return AnimationPropertyType.ShapePictureContrast;
      case "imageData.gamma":
        return AnimationPropertyType.ShapePictureGamma;
      case "imageData.grayscale":
        return AnimationPropertyType.ShapePictureGrayscale;
      case "fillon":
      case "fill.on":
        return AnimationPropertyType.ShapeFillOn;
      case "filltype":
      case "fill.type":
        return AnimationPropertyType.ShapeFillType;
      case "fillcolor":
      case "fill.color":
        return AnimationPropertyType.ShapeFillColor;
      case "fillopacity":
      case "fill.opacity":
        return AnimationPropertyType.ShapeFillOpacity;
      case "fillcolor2":
      case "fill.color2":
        return AnimationPropertyType.ShapeFillColor2;
      case "stroke.on":
        return AnimationPropertyType.StrokeOn;
      case "stroke.color":
        return AnimationPropertyType.ShapeStrokeColor;
      case "shadow.on":
        return AnimationPropertyType.ShapeShadowOn;
      case "shadow.type":
        return AnimationPropertyType.ShapeShadowType;
      case "shadow.color":
        return AnimationPropertyType.ShapeShadowColor;
      case "shadow.opacity":
        return AnimationPropertyType.ShapeShadowOpacity;
      case "shadow.offset.x":
        return AnimationPropertyType.ShapeShadowOffsetX;
      case "shadow.offset.y":
        return AnimationPropertyType.ShapeShadowOffsetY;
      case "ppt_x":
        return AnimationPropertyType.PptX;
      case "ppt_y":
        return AnimationPropertyType.PptY;
      case "ppt_x_ppt_y":
        return AnimationPropertyType.PptXPptY;
      case "ppt_h":
        return AnimationPropertyType.PptHeight;
      case "ppt_w":
        return AnimationPropertyType.PptWidth;
      case "r":
        return AnimationPropertyType.RotationPPT;
      default:
        return AnimationPropertyType.NotDefined;
    }
  }

  internal static TimeNodeType GetTimeNode(string timeNodeValue)
  {
    switch (timeNodeValue)
    {
      case "afterEffect":
        return TimeNodeType.AfterEffect;
      case "afterGroup":
        return TimeNodeType.AfterGroup;
      case "clickEffect":
        return TimeNodeType.ClickEffect;
      case "clickPar":
        return TimeNodeType.ClickPar;
      case "interactiveSeq":
        return TimeNodeType.InteractiveSeq;
      case "mainSeq":
        return TimeNodeType.MainSeq;
      case "tmRoot":
        return TimeNodeType.TmRoot;
      case "withEffect":
        return TimeNodeType.WithEffect;
      case "withGroup":
        return TimeNodeType.WithGroup;
      default:
        return TimeNodeType.None;
    }
  }

  internal static string GetTimeNodeTypeString(TimeNodeType timeNodeType)
  {
    switch (timeNodeType)
    {
      case TimeNodeType.ClickEffect:
        return "clickEffect";
      case TimeNodeType.WithEffect:
        return "withEffect";
      case TimeNodeType.AfterEffect:
        return "afterEffect";
      case TimeNodeType.MainSeq:
        return "mainSeq";
      case TimeNodeType.InteractiveSeq:
        return "interactiveSeq";
      case TimeNodeType.ClickPar:
        return "clickPar";
      case TimeNodeType.WithGroup:
        return "withGroup";
      case TimeNodeType.AfterGroup:
        return "afterGroup";
      case TimeNodeType.TmRoot:
        return "tmRoot";
      default:
        return "";
    }
  }

  internal static TimeNodeFill GetTimeNodeFillValue(string timeNodeFill)
  {
    switch (timeNodeFill)
    {
      case "freeze":
        return TimeNodeFill.Freeze;
      case "hold":
        return TimeNodeFill.Hold;
      case "remove":
        return TimeNodeFill.Remove;
      case "transition":
        return TimeNodeFill.Trasnistion;
      default:
        return TimeNodeFill.None;
    }
  }

  internal static string GetTimeNodeFillString(TimeNodeFill timeNodeFill)
  {
    switch (timeNodeFill)
    {
      case TimeNodeFill.Remove:
        return "remove";
      case TimeNodeFill.Freeze:
        return "freeze";
      case TimeNodeFill.Hold:
        return "hold";
      case TimeNodeFill.Trasnistion:
        return "transition";
      default:
        return "";
    }
  }

  internal static CommandEffectType GetCommandEffectType(string command)
  {
    switch (command)
    {
      case "call":
        return CommandEffectType.Call;
      case "evt":
        return CommandEffectType.Event;
      case "verb":
        return CommandEffectType.Verb;
      default:
        return CommandEffectType.NotDefined;
    }
  }

  internal static bool? GetNullableBoolValue(string value)
  {
    switch (value)
    {
      case "0":
        return new bool?(false);
      case "1":
        return new bool?(true);
      default:
        return new bool?();
    }
  }

  internal static string GetNullableBoolString(bool? value)
  {
    ref bool? local = ref value;
    bool valueOrDefault = local.GetValueOrDefault();
    if (local.HasValue)
    {
      switch (valueOrDefault)
      {
        case false:
          return "0";
        case true:
          return "1";
      }
    }
    return "";
  }

  internal static BehaviorAccumulateType GetBehaviorType(string accumulateType)
  {
    switch (accumulateType)
    {
      case "always":
        return BehaviorAccumulateType.Always;
      default:
        return BehaviorAccumulateType.None;
    }
  }

  internal static string GetBehaviorTypeString(BehaviorAccumulateType accumulateType)
  {
    return accumulateType == BehaviorAccumulateType.Always ? "always" : "";
  }

  internal static BehaviourOverrideType GetBehaviorOverrideType(string overrideType)
  {
    switch (overrideType)
    {
      case "normal":
        return BehaviourOverrideType.Normal;
      case "childStyle":
        return BehaviourOverrideType.ChildStyle;
      default:
        return BehaviourOverrideType.None;
    }
  }

  internal static string GetBehaviorOverrideTypeString(BehaviourOverrideType overrideType)
  {
    switch (overrideType)
    {
      case BehaviourOverrideType.None:
        return "none";
      case BehaviourOverrideType.ChildStyle:
        return "childStyle";
      case BehaviourOverrideType.Normal:
        return "normal";
      default:
        return "";
    }
  }

  internal static BehaviorTransformType GetBehaviorTransformType(string transformType)
  {
    switch (transformType)
    {
      case "img":
        return BehaviorTransformType.Image;
      case "pt":
        return BehaviorTransformType.Point;
      default:
        return BehaviorTransformType.None;
    }
  }

  internal static string GetBehaviorTransformTypeString(BehaviorTransformType transformType)
  {
    switch (transformType)
    {
      case BehaviorTransformType.Image:
        return "img";
      case BehaviorTransformType.Point:
        return "pt";
      default:
        return "none";
    }
  }

  internal static EffectTriggerType GetTriggerType(TimeNodeType timeNodeType)
  {
    if (timeNodeType == TimeNodeType.ClickEffect)
      return EffectTriggerType.OnClick;
    return timeNodeType == TimeNodeType.WithEffect ? EffectTriggerType.WithPrevious : EffectTriggerType.AfterPrevious;
  }

  internal static TimeNodeType GetTimeNodeType(EffectTriggerType effectTriggerType)
  {
    switch (effectTriggerType)
    {
      case EffectTriggerType.AfterPrevious:
        return TimeNodeType.AfterEffect;
      case EffectTriggerType.OnClick:
        return TimeNodeType.ClickEffect;
      case EffectTriggerType.WithPrevious:
        return TimeNodeType.WithEffect;
      default:
        return TimeNodeType.None;
    }
  }

  internal static BehaviorAdditiveType GetBehaviorAdditiveType(string additiveType)
  {
    switch (additiveType)
    {
      case "base":
        return BehaviorAdditiveType.Base;
      case "mult":
        return BehaviorAdditiveType.Multiply;
      case "none":
        return BehaviorAdditiveType.None;
      case "repl":
        return BehaviorAdditiveType.Replace;
      case "sum":
        return BehaviorAdditiveType.Sum;
      default:
        return BehaviorAdditiveType.NotDefined;
    }
  }

  internal static string GetBehaviorAdditiveString(BehaviorAdditiveType additiveType)
  {
    switch (additiveType)
    {
      case BehaviorAdditiveType.None:
        return "none";
      case BehaviorAdditiveType.Base:
        return "base";
      case BehaviorAdditiveType.Sum:
        return "sum";
      case BehaviorAdditiveType.Replace:
        return "repl";
      case BehaviorAdditiveType.Multiply:
        return "mult";
      default:
        return "";
    }
  }

  internal static TriggerRuntimeNode GetTriggerRuntimeNode(string triggerType)
  {
    switch (triggerType)
    {
      case "all":
        return TriggerRuntimeNode.All;
      case "first":
        return TriggerRuntimeNode.First;
      case "last":
        return TriggerRuntimeNode.Last;
      default:
        return TriggerRuntimeNode.None;
    }
  }

  internal static ParagraphBuildType GetParagraphBuildType(string buildType)
  {
    switch (buildType)
    {
      case "allAtOnce":
        return ParagraphBuildType.AllAtOnce;
      case "cust":
        return ParagraphBuildType.Custom;
      case "p":
        return ParagraphBuildType.Paragraph;
      case "whole":
        return ParagraphBuildType.Whole;
      default:
        return ParagraphBuildType.None;
    }
  }

  internal static string GetParagraphBuildTypeString(ParagraphBuildType buildType)
  {
    switch (buildType)
    {
      case ParagraphBuildType.None:
        return "none";
      case ParagraphBuildType.AllAtOnce:
        return "allAtOnce";
      case ParagraphBuildType.Custom:
        return "cust";
      case ParagraphBuildType.Paragraph:
        return "p";
      case ParagraphBuildType.Whole:
        return "whole";
      default:
        return "";
    }
  }

  internal static DiagramBuildType GetDiagramBuildType(string diagramBuild)
  {
    switch (diagramBuild)
    {
      case "allAtOnce":
        return DiagramBuildType.AllAtOnce;
      case "breadthByLvl":
        return DiagramBuildType.BreadthByLvl;
      case "breadthByNode":
        return DiagramBuildType.BreadthByNode;
      case "ccw":
        return DiagramBuildType.Ccw;
      case "ccwIn":
        return DiagramBuildType.CcwIn;
      case "ccwOut":
        return DiagramBuildType.CcwOut;
      case "cust":
        return DiagramBuildType.Cust;
      case "cw":
        return DiagramBuildType.Cw;
      case "cwIn":
        return DiagramBuildType.CwIn;
      case "cwOut":
        return DiagramBuildType.CwOut;
      case "depthByBranch":
        return DiagramBuildType.DepthByBranch;
      case "depthByNode":
        return DiagramBuildType.DepthByNode;
      case "down":
        return DiagramBuildType.Down;
      case "inByRing":
        return DiagramBuildType.InByRing;
      case "outByRing":
        return DiagramBuildType.OutByRing;
      case "up":
        return DiagramBuildType.Up;
      case "whole":
        return DiagramBuildType.Whole;
      default:
        return DiagramBuildType.None;
    }
  }

  internal static string GetDiagramBuildTypeString(DiagramBuildType diagramBuild)
  {
    switch (diagramBuild)
    {
      case DiagramBuildType.AllAtOnce:
        return "allAtOnce";
      case DiagramBuildType.BreadthByLvl:
        return "breadthByLvl";
      case DiagramBuildType.BreadthByNode:
        return "breadthByNode";
      case DiagramBuildType.Ccw:
        return "ccw";
      case DiagramBuildType.CcwIn:
        return "ccwIn";
      case DiagramBuildType.CcwOut:
        return "ccwOut";
      case DiagramBuildType.Cust:
        return "cust";
      case DiagramBuildType.Cw:
        return "cw";
      case DiagramBuildType.CwIn:
        return "cwIn";
      case DiagramBuildType.CwOut:
        return "cwOut";
      case DiagramBuildType.DepthByBranch:
        return "depthByBranch";
      case DiagramBuildType.DepthByNode:
        return "depthByNode";
      case DiagramBuildType.Down:
        return "down";
      case DiagramBuildType.InByRing:
        return "inByRing";
      case DiagramBuildType.OutByRing:
        return "outByRing";
      case DiagramBuildType.Up:
        return "up";
      case DiagramBuildType.Whole:
        return "whole";
      default:
        return "";
    }
  }

  internal static OleChartBuildType GetOleChartBuildType(string chartBuildType)
  {
    switch (chartBuildType)
    {
      case "allAtOnce":
        return OleChartBuildType.AllAtOnce;
      case "category":
        return OleChartBuildType.Category;
      case "categoryEl":
        return OleChartBuildType.CategoryEl;
      case "series":
        return OleChartBuildType.Series;
      case "seriesEl":
        return OleChartBuildType.SeriesEl;
      default:
        return OleChartBuildType.None;
    }
  }

  internal static string GetOleChartBuildTypeString(OleChartBuildType chartBuildType)
  {
    switch (chartBuildType)
    {
      case OleChartBuildType.AllAtOnce:
        return "allAtOnce";
      case OleChartBuildType.Category:
        return "category";
      case OleChartBuildType.CategoryEl:
        return "categoryEl";
      case OleChartBuildType.Series:
        return "series";
      case OleChartBuildType.SeriesEl:
        return "seriesEl";
      default:
        return "";
    }
  }

  internal static AnimationBuildType GetAnimationDiagramBuildType(string buildtype)
  {
    switch (buildtype)
    {
      case "allAtOnce":
        return AnimationBuildType.AllAtOnce;
      case "lvlAtOnce":
        return AnimationBuildType.LvlAtOnce;
      case "lvlOne":
        return AnimationBuildType.LvlOne;
      case "one":
        return AnimationBuildType.One;
      default:
        return AnimationBuildType.None;
    }
  }

  internal static string GetAnimationDiagramBuildTypeString(AnimationBuildType buildtype)
  {
    switch (buildtype)
    {
      case AnimationBuildType.AllAtOnce:
        return "allAtOnce";
      case AnimationBuildType.LvlAtOnce:
        return "lvlAtOnce";
      case AnimationBuildType.LvlOne:
        return "lvlOne";
      case AnimationBuildType.One:
        return "one";
      default:
        return "";
    }
  }

  internal static AnimationBuildStep GetTargetChartAnimationBuildStep(string buildStep)
  {
    switch (buildStep)
    {
      case "allPts":
        return AnimationBuildStep.AllPts;
      case "category":
        return AnimationBuildStep.Category;
      case "gridLegend":
        return AnimationBuildStep.GridLegend;
      case "ptInCategory":
        return AnimationBuildStep.PtInCategory;
      case "ptInSeries":
        return AnimationBuildStep.PtInSeries;
      case "series":
        return AnimationBuildStep.Series;
      default:
        return AnimationBuildStep.None;
    }
  }

  internal static string GetTargetChartAnimationBuildStepString(AnimationBuildStep buildStep)
  {
    switch (buildStep)
    {
      case AnimationBuildStep.AllPts:
        return "allPts";
      case AnimationBuildStep.Category:
        return "category";
      case AnimationBuildStep.GridLegend:
        return "gridLegend";
      case AnimationBuildStep.PtInCategory:
        return "ptInCategory";
      case AnimationBuildStep.PtInSeries:
        return "ptInSeries";
      case AnimationBuildStep.Series:
        return "series";
      default:
        return "";
    }
  }

  internal static AnimationBuildStepDiagram GetTargetDiagramBuildStep(string buildStep)
  {
    switch (buildStep)
    {
      case "bg":
        return AnimationBuildStepDiagram.Bg;
      case "sp":
        return AnimationBuildStepDiagram.Sp;
      default:
        return AnimationBuildStepDiagram.None;
    }
  }

  internal static ChartSubelementType GetTargetOleChartSubElement(string subElement)
  {
    switch (subElement)
    {
      case "category":
        return ChartSubelementType.Category;
      case "gridLegend":
        return ChartSubelementType.GridLegend;
      case "ptInCategory":
        return ChartSubelementType.PtInCategory;
      case "ptInSeries":
        return ChartSubelementType.PtInSeries;
      case "series":
        return ChartSubelementType.Series;
      default:
        return ChartSubelementType.None;
    }
  }

  internal static string GetTargetOleChartSubElementString(ChartSubelementType subElement)
  {
    switch (subElement)
    {
      case ChartSubelementType.Category:
        return "category";
      case ChartSubelementType.GridLegend:
        return "gridLegend";
      case ChartSubelementType.PtInCategory:
        return "ptInCategory";
      case ChartSubelementType.PtInSeries:
        return "ptInSeries";
      case ChartSubelementType.Series:
        return "series";
      default:
        return "";
    }
  }

  internal static IterateType GetIterateType(string iteratevalue)
  {
    switch (iteratevalue)
    {
      case "el":
        return IterateType.El;
      case "lt":
        return IterateType.Lt;
      case "wd":
        return IterateType.Wd;
      default:
        return IterateType.None;
    }
  }

  internal static EffectSubtype GetSubTypeWhileNone(EffectType effectType)
  {
    Dictionary<float, EffectSubtype> subType = AnimationConstant.GetSubTypeDictionary()[effectType];
    if (subType.ContainsValue(EffectSubtype.None))
      return EffectSubtype.None;
    EffectSubtype subTypeWhileNone = EffectSubtype.None;
    using (Dictionary<float, EffectSubtype>.Enumerator enumerator = subType.GetEnumerator())
    {
      if (enumerator.MoveNext())
        subTypeWhileNone = enumerator.Current.Value;
    }
    return subTypeWhileNone;
  }

  internal static EffectPresetClassType GetEffectPresetClassType(EffectType effectType)
  {
    Dictionary<EffectPresetClassType, Dictionary<float, EffectType>> effectTypeDictionary = AnimationConstant.GetEffectTypeDictionary();
    foreach (EffectPresetClassType key in effectTypeDictionary.Keys)
    {
      if (effectTypeDictionary[key].ContainsValue(effectType))
        return key;
    }
    return EffectPresetClassType.None;
  }

  internal static bool IsValidSubtype(EffectType effectType, EffectSubtype subType)
  {
    return AnimationConstant.GetSubTypeDictionary()[effectType].ContainsValue(subType);
  }

  internal static EffectSubtype GetEffectSubType(float id, EffectType effectType)
  {
    Dictionary<float, EffectSubtype> subType = AnimationConstant.GetSubTypeDictionary()[effectType];
    return subType.Count > 0 && subType.ContainsKey(id) ? subType[id] : EffectSubtype.None;
  }

  internal static EffectType GetEffectType(float id, EffectPresetClassType presetClassType)
  {
    Dictionary<float, EffectType> effectType = AnimationConstant.GetEffectTypeDictionary()[presetClassType];
    return effectType.ContainsKey(id) ? effectType[id] : EffectType.Custom;
  }

  internal static void UpdateSequenceShape(CommonTimeNode cTn, Sequence sequence)
  {
    if (cTn.StartConditionList == null)
      return;
    foreach (Condition startCondition in cTn.StartConditionList)
    {
      if (startCondition.Event == TriggerEvent.OnClick && startCondition.Target != null && startCondition.Target.ShapeTarget != null)
      {
        int shapeId = int.Parse(startCondition.Target.ShapeTarget.ShapeId);
        IShape shapeWithId = (IShape) sequence.BaseSlide.GetShapeWithId(sequence.BaseSlide.Shapes as Shapes, shapeId);
        sequence.TriggerShape = shapeWithId;
      }
    }
  }

  internal static void UpdateEffects(CommonTimeNode cTn, Sequence sequence, Effect effect)
  {
    IBehaviors behaviors = (IBehaviors) null;
    if (cTn.PresetClass != EffectPresetClassType.None)
    {
      if (effect == null)
      {
        effect = new Effect(sequence.Count);
        sequence.AddEffect((IEffect) effect);
      }
      effect.SetPresetClassType(cTn.PresetClass);
      effect.Sequence = (ISequence) sequence;
      effect.SeType(AnimationConstant.GetEffectType(cTn.PresetId, effect.PresetClassType));
      effect.SetSubType(AnimationConstant.GetEffectSubType(cTn.PresetSubtype, effect.Type));
      if (cTn.Iterate != null)
        effect.Iterate = cTn.Iterate;
      AnimationConstant.UpdateTiming(effect.Timing, cTn);
      if (effect.Behaviors.Count > 0)
        (effect.Behaviors as Behaviors).Clear();
      behaviors = effect.Behaviors;
    }
    foreach (object childTimeNode in cTn.ChildTimeNodeList)
    {
      switch (childTimeNode)
      {
        case ParallelTimeNode _:
          AnimationConstant.UpdateEffects((childTimeNode as ParallelTimeNode).CommonTimeNode, sequence, (Effect) null);
          continue;
        case SetEffect _:
          if (behaviors != null)
          {
            SetEffect setEffect = childTimeNode as SetEffect;
            setEffect.Accumulate = AnimationConstant.GetNullableBoolValue(setEffect.CommonBehavior.Accumulate);
            setEffect.Additive = setEffect.CommonBehavior.Additive == BehaviorAdditiveType.None ? BehaviorAdditiveType.NotDefined : setEffect.CommonBehavior.Additive;
            setEffect.TargetElement = setEffect.CommonBehavior.Target;
            AnimationConstant.UpdateProperties(setEffect.Properties, setEffect.CommonBehavior);
            AnimationConstant.UpdateTiming(setEffect.Timing, setEffect.CommonBehavior.CommonTimeNode);
            (behaviors as Behaviors).Add((IBehavior) setEffect);
            continue;
          }
          continue;
        case FilterEffect _:
          if (behaviors != null)
          {
            FilterEffect filterEffect = childTimeNode as FilterEffect;
            filterEffect.Accumulate = AnimationConstant.GetNullableBoolValue(filterEffect.CommonBehavior.Accumulate);
            filterEffect.Additive = filterEffect.CommonBehavior.Additive == BehaviorAdditiveType.None ? BehaviorAdditiveType.NotDefined : filterEffect.CommonBehavior.Additive;
            filterEffect.TargetElement = filterEffect.CommonBehavior.Target;
            if (filterEffect.CommonBehavior.AttributeNameList != null && filterEffect.CommonBehavior.AttributeNameList.Count > 0)
              AnimationConstant.UpdateProperties(filterEffect.Properties, filterEffect.CommonBehavior);
            AnimationConstant.UpdateTiming(filterEffect.Timing, filterEffect.CommonBehavior.CommonTimeNode);
            (behaviors as Behaviors).Add((IBehavior) filterEffect);
            continue;
          }
          continue;
        case PropertyEffect _:
          if (behaviors != null)
          {
            PropertyEffect propertyEffect = childTimeNode as PropertyEffect;
            propertyEffect.Accumulate = AnimationConstant.GetNullableBoolValue(propertyEffect.CommonBehavior.Accumulate);
            propertyEffect.Additive = propertyEffect.CommonBehavior.Additive == BehaviorAdditiveType.None ? BehaviorAdditiveType.NotDefined : propertyEffect.CommonBehavior.Additive;
            propertyEffect.TargetElement = propertyEffect.CommonBehavior.Target;
            AnimationConstant.UpdatePropertyEffectPoints(propertyEffect);
            if (propertyEffect.CommonBehavior.AttributeNameList != null && propertyEffect.CommonBehavior.AttributeNameList.Count > 0)
              AnimationConstant.UpdateProperties(propertyEffect.Properties, propertyEffect.CommonBehavior);
            AnimationConstant.UpdateTiming(propertyEffect.Timing, propertyEffect.CommonBehavior.CommonTimeNode);
            (behaviors as Behaviors).Add((IBehavior) propertyEffect);
            continue;
          }
          continue;
        case ScaleEffect _:
          if (behaviors != null)
          {
            ScaleEffect scaleEffect = childTimeNode as ScaleEffect;
            scaleEffect.Accumulate = AnimationConstant.GetNullableBoolValue(scaleEffect.CommonBehavior.Accumulate);
            scaleEffect.Additive = scaleEffect.CommonBehavior.Additive == BehaviorAdditiveType.None ? BehaviorAdditiveType.NotDefined : scaleEffect.CommonBehavior.Additive;
            scaleEffect.TargetElement = scaleEffect.CommonBehavior.Target;
            if (scaleEffect.CommonBehavior.AttributeNameList != null && scaleEffect.CommonBehavior.AttributeNameList.Count > 0)
              AnimationConstant.UpdateProperties(scaleEffect.Properties, scaleEffect.CommonBehavior);
            AnimationConstant.UpdateTiming(scaleEffect.Timing, scaleEffect.CommonBehavior.CommonTimeNode);
            (behaviors as Behaviors).Add((IBehavior) scaleEffect);
            continue;
          }
          continue;
        case MotionEffect _:
          if (behaviors != null)
          {
            MotionEffect motionEffect = childTimeNode as MotionEffect;
            motionEffect.Accumulate = AnimationConstant.GetNullableBoolValue(motionEffect.CommonBehavior.Accumulate);
            motionEffect.Additive = motionEffect.CommonBehavior.Additive == BehaviorAdditiveType.None ? BehaviorAdditiveType.NotDefined : motionEffect.CommonBehavior.Additive;
            motionEffect.TargetElement = motionEffect.CommonBehavior.Target;
            if (motionEffect.CommonBehavior.AttributeNameList != null && motionEffect.CommonBehavior.AttributeNameList.Count > 0)
              AnimationConstant.UpdateProperties(motionEffect.Properties, motionEffect.CommonBehavior);
            AnimationConstant.UpdateTiming(motionEffect.Timing, motionEffect.CommonBehavior.CommonTimeNode);
            (behaviors as Behaviors).Add((IBehavior) motionEffect);
            continue;
          }
          continue;
        case RotationEffect _:
          if (behaviors != null)
          {
            RotationEffect rotationEffect = childTimeNode as RotationEffect;
            rotationEffect.Accumulate = AnimationConstant.GetNullableBoolValue(rotationEffect.CommonBehavior.Accumulate);
            rotationEffect.Additive = rotationEffect.CommonBehavior.Additive == BehaviorAdditiveType.None ? BehaviorAdditiveType.NotDefined : rotationEffect.CommonBehavior.Additive;
            rotationEffect.TargetElement = rotationEffect.CommonBehavior.Target;
            if (rotationEffect.CommonBehavior.AttributeNameList != null && rotationEffect.CommonBehavior.AttributeNameList.Count > 0)
              AnimationConstant.UpdateProperties(rotationEffect.Properties, rotationEffect.CommonBehavior);
            AnimationConstant.UpdateTiming(rotationEffect.Timing, rotationEffect.CommonBehavior.CommonTimeNode);
            (behaviors as Behaviors).Add((IBehavior) rotationEffect);
            continue;
          }
          continue;
        case ColorEffect _:
          if (behaviors != null)
          {
            ColorEffect colorEffect = childTimeNode as ColorEffect;
            colorEffect.Accumulate = AnimationConstant.GetNullableBoolValue(colorEffect.CommonBehavior.Accumulate);
            colorEffect.Additive = colorEffect.CommonBehavior.Additive == BehaviorAdditiveType.None ? BehaviorAdditiveType.NotDefined : colorEffect.CommonBehavior.Additive;
            colorEffect.TargetElement = colorEffect.CommonBehavior.Target;
            if (colorEffect.CommonBehavior.AttributeNameList != null && colorEffect.CommonBehavior.AttributeNameList.Count > 0)
              AnimationConstant.UpdateProperties(colorEffect.Properties, colorEffect.CommonBehavior);
            AnimationConstant.UpdateTiming(colorEffect.Timing, colorEffect.CommonBehavior.CommonTimeNode);
            (behaviors as Behaviors).Add((IBehavior) colorEffect);
            continue;
          }
          continue;
        case CommandEffect _:
          if (behaviors != null)
          {
            CommandEffect commandEffect = childTimeNode as CommandEffect;
            commandEffect.Accumulate = AnimationConstant.GetNullableBoolValue(commandEffect.CommonBehavior.Accumulate);
            commandEffect.Additive = commandEffect.CommonBehavior.Additive == BehaviorAdditiveType.None ? BehaviorAdditiveType.NotDefined : commandEffect.CommonBehavior.Additive;
            commandEffect.TargetElement = commandEffect.CommonBehavior.Target;
            if (commandEffect.CommonBehavior.AttributeNameList != null && commandEffect.CommonBehavior.AttributeNameList.Count > 0)
              AnimationConstant.UpdateProperties(commandEffect.Properties, commandEffect.CommonBehavior);
            AnimationConstant.UpdateTiming(commandEffect.Timing, commandEffect.CommonBehavior.CommonTimeNode);
            (behaviors as Behaviors).Add((IBehavior) commandEffect);
            continue;
          }
          continue;
        default:
          continue;
      }
    }
  }

  internal static void UpdatePropertyEffectPoints(PropertyEffect propertyEffect)
  {
    AnimationPoints animationPoints = new AnimationPoints();
    if (propertyEffect.TAVList != null && propertyEffect.TAVList.Count > 0)
    {
      foreach (TimeAnimateValue tav in propertyEffect.TAVList)
      {
        AnimationPoint point = new AnimationPoint();
        if (tav.Formula != null)
          point.Formula = tav.Formula;
        if (tav.Time != -1)
          point.Time = (float) (tav.Time / 1000);
        if (tav.TimeValue.Bool.HasValue)
          point.Value = (object) tav.TimeValue.Bool;
        else if (tav.TimeValue.Color != null)
          point.Value = (object) tav.TimeValue.Color;
        else if (tav.TimeValue.Float.HasValue)
          point.Value = (object) tav.TimeValue.Float;
        else if (tav.TimeValue.Int.HasValue)
          point.Value = (object) tav.TimeValue.Int;
        else if (tav.TimeValue.String != null)
          point.Value = (object) tav.TimeValue.String;
        animationPoints.Add(point);
      }
    }
    else
      propertyEffect.TAVList = new List<TimeAnimateValue>();
    propertyEffect.Points = (IAnimationPoints) animationPoints;
    propertyEffect.TAVList.Clear();
  }

  internal static void UpdateProperties(
    IBehaviorProperties behaviorProperties,
    CommonBehavior commonBehavior)
  {
    foreach (AttributeName attributeName in commonBehavior.AttributeNameList)
      (behaviorProperties as BehaviorProperties).Add(AnimationConstant.GetPropertyType(attributeName.Name));
  }

  internal static void UpdateTiming(ITiming timing, CommonTimeNode cTn)
  {
    timing.Accelerate = (float) cTn.Acceleration / 100000f;
    timing.AutoReverse = cTn.AutoReverse;
    timing.Decelerate = (float) cTn.Deceleration / 100000f;
    float result1;
    float.TryParse(cTn.Duration, out result1);
    if ((double) result1 != 0.0)
      timing.Duration = result1 / 1000f;
    else if (cTn.Duration == "indefinite")
      timing.Duration = float.PositiveInfinity;
    if (cTn.RepeatCount == "indefinite")
    {
      timing.RepeatCount = float.PositiveInfinity;
    }
    else
    {
      float result2;
      float.TryParse(cTn.RepeatCount, out result2);
      timing.RepeatCount = (double) result2 == 0.0 ? 1f : result2 / 1000f;
    }
    float result3;
    float.TryParse(cTn.RepeatDuration, out result3);
    if ((double) result3 != 0.0)
      timing.RepeatDuration = result3 / 1000f;
    else if (cTn.RepeatDuration == "indefinite")
      timing.RepeatDuration = float.PositiveInfinity;
    timing.Restart = cTn.Restart;
    timing.Speed = cTn.Speed == 0 ? 1f : (float) (cTn.Speed / 100000);
    timing.TriggerDelayTime = AnimationConstant.GetDelay(cTn.StartConditionList) / 1000f;
    timing.TriggerType = AnimationConstant.GetTriggerType(cTn.NodeType);
    (timing as Timing).Fill = cTn.Fill;
  }

  internal static float GetDelay(List<Condition> conditionList)
  {
    if (conditionList != null)
    {
      using (List<Condition>.Enumerator enumerator = conditionList.GetEnumerator())
      {
        if (enumerator.MoveNext())
          return (float) enumerator.Current.Delay;
      }
    }
    return float.NaN;
  }

  internal static void UpdateMainSequence(CommonTimeNode cTn, ref uint cTnId, BaseSlide baseSlide)
  {
    SequenceTimeNode sequenceTimeNode = new SequenceTimeNode();
    sequenceTimeNode.Concurrent = true;
    sequenceTimeNode.NextAction = NextAction.Seek;
    CommonTimeNode cTn1 = sequenceTimeNode.CommonTimeNode = new CommonTimeNode();
    cTn1.Id = ++cTnId;
    cTn1.Duration = "indefinite";
    cTn1.NodeType = TimeNodeType.MainSeq;
    cTn1.ChildTimeNodeList = new List<object>();
    AnimationConstant.UpdateEffectList(cTn1, ((baseSlide.Timeline as Timeline).GetMainSequence() as Sequence).GetEffectList(), ref cTnId, true);
    sequenceTimeNode.PreviousConditionList = new List<Condition>();
    AnimationConstant.UpdateConditionList(sequenceTimeNode.PreviousConditionList, 0, TriggerEvent.OnPrev, (string) null);
    sequenceTimeNode.NextConditionList = new List<Condition>();
    AnimationConstant.UpdateConditionList(sequenceTimeNode.NextConditionList, 0, TriggerEvent.OnNext, (string) null);
    cTn.ChildTimeNodeList.Add((object) sequenceTimeNode);
  }

  internal static void UpdateInteractiveSequences(
    CommonTimeNode cTn,
    ref uint cTnId,
    BaseSlide baseSlide)
  {
    foreach (Sequence interactiveSequence in (IEnumerable<ISequence>) (baseSlide.Timeline as Timeline).GetInteractiveSequences())
    {
      IShape triggerShape = interactiveSequence.TriggerShape;
      SequenceTimeNode sequenceTimeNode = new SequenceTimeNode();
      sequenceTimeNode.Concurrent = true;
      sequenceTimeNode.NextAction = NextAction.Seek;
      CommonTimeNode cTn1 = sequenceTimeNode.CommonTimeNode = new CommonTimeNode();
      cTn1.Id = ++cTnId;
      cTn1.Restart = EffectRestartType.WhenNotActive;
      cTn1.Fill = TimeNodeFill.Hold;
      cTn1.EventFilter = "cancelBubble";
      cTn1.NodeType = TimeNodeType.InteractiveSeq;
      cTn1.StartConditionList = new List<Condition>();
      AnimationConstant.UpdateConditionList(cTn1.StartConditionList, 0, TriggerEvent.OnClick, (triggerShape as Shape).ShapeId.ToString());
      cTn1.EndSync = new Condition();
      AnimationConstant.UpdateEndSync(cTn1.EndSync, TriggerEvent.End, 0);
      cTn1.ChildTimeNodeList = new List<object>();
      AnimationConstant.UpdateEffectList(cTn1, interactiveSequence.GetEffectList(), ref cTnId, false);
      sequenceTimeNode.NextConditionList = new List<Condition>();
      AnimationConstant.UpdateConditionList(sequenceTimeNode.NextConditionList, 0, TriggerEvent.OnClick, (triggerShape as Shape).ShapeId.ToString());
      cTn.ChildTimeNodeList.Add((object) sequenceTimeNode);
    }
  }

  private static void UpdateEndSync(Condition condition, TriggerEvent triggerEvent, int delay)
  {
    if (triggerEvent != TriggerEvent.End)
      return;
    condition.Event = TriggerEvent.End;
    condition.Delay = delay;
    condition.RunTimeNodeTrigger = new RunTimeNodeTrigger();
    condition.RunTimeNodeTrigger.Val = TriggerRuntimeNode.All;
  }

  private static void UpdateParDelay(IEffect effect, ref int parDelay)
  {
    float num1 = 0.0f;
    float num2 = 0.0f;
    float num3 = 0.0f;
    if (!float.IsNaN(effect.Timing.TriggerDelayTime) && (double) effect.Timing.TriggerDelayTime > 0.0)
      num2 = effect.Timing.TriggerDelayTime;
    if ((double) effect.Timing.Duration != 0.001 && (double) effect.Timing.Duration > 0.0 && !float.IsPositiveInfinity(effect.Timing.Duration))
      num3 = effect.Timing.Duration;
    float num4 = num2 + num3;
    foreach (IBehavior behavior in (IEnumerable<IBehavior>) effect.Behaviors)
    {
      float num5 = 0.0f;
      float num6 = 0.0f;
      if (!float.IsNaN(behavior.Timing.TriggerDelayTime) && (double) behavior.Timing.TriggerDelayTime > 0.0)
        num5 = behavior.Timing.TriggerDelayTime;
      if ((double) behavior.Timing.Duration != 0.001 && (double) behavior.Timing.Duration > 0.0 && !float.IsPositiveInfinity(behavior.Timing.Duration))
        num6 = behavior.Timing.Duration;
      float num7 = num5 + num6;
      if ((double) num1 < (double) num7)
        num1 = num7;
    }
    float num8 = (double) num1 >= (double) num3 ? num1 + num2 : num4;
    parDelay += (int) ((double) num8 * 1000.0);
  }

  internal static void UpdateEffectList(
    CommonTimeNode cTn,
    List<IEffect> effectList,
    ref uint cTnId,
    bool isMainSeq)
  {
    int parDelay = 0;
    for (int index = 0; index < effectList.Count; ++index)
    {
      Effect effect = effectList[index] as Effect;
      if (index == 0 || effect.Timing.TriggerType == EffectTriggerType.OnClick)
      {
        AnimationConstant.UpdateRootTimeNode(cTn.ChildTimeNodeList, effect, ref cTnId, isMainSeq, ref parDelay);
        AnimationConstant.UpdateParDelay((IEffect) effect, ref parDelay);
      }
      else if (effect.Timing.TriggerType == EffectTriggerType.AfterPrevious)
      {
        AnimationConstant.UpdateParentTimeNode((cTn.ChildTimeNodeList[cTn.ChildTimeNodeList.Count - 1] as ParallelTimeNode).CommonTimeNode.ChildTimeNodeList, effect, ref cTnId, true, ref parDelay);
        AnimationConstant.UpdateParDelay((IEffect) effect, ref parDelay);
      }
      else if (effect.Timing.TriggerType == EffectTriggerType.WithPrevious)
      {
        ParallelTimeNode childTimeNode = cTn.ChildTimeNodeList[cTn.ChildTimeNodeList.Count - 1] as ParallelTimeNode;
        AnimationConstant.UpdateChildTimeNode((childTimeNode.CommonTimeNode.ChildTimeNodeList[childTimeNode.CommonTimeNode.ChildTimeNodeList.Count - 1] as ParallelTimeNode).CommonTimeNode.ChildTimeNodeList, effect, ref cTnId);
      }
      isMainSeq = true;
    }
  }

  internal static void UpdateConditionList(
    List<Condition> conditionList,
    int delay,
    TriggerEvent triggerEvent,
    string shapeId)
  {
    switch (triggerEvent)
    {
      case TriggerEvent.None:
        conditionList.Add(new Condition() { Delay = delay });
        break;
      case TriggerEvent.OnBegin:
        Condition condition1 = new Condition()
        {
          Event = TriggerEvent.OnBegin,
          Delay = 0,
          TimeNode = new TimeNode()
        };
        condition1.TimeNode.Val = 2U;
        conditionList.Add(condition1);
        break;
      case TriggerEvent.OnClick:
        Condition condition2 = new Condition()
        {
          Event = TriggerEvent.OnClick,
          Delay = delay,
          Target = new TargetElement()
        };
        condition2.Target.ShapeTarget = new ShapeTarget();
        condition2.Target.ShapeTarget.ShapeId = shapeId;
        conditionList.Add(condition2);
        break;
      case TriggerEvent.OnNext:
        Condition condition3 = new Condition()
        {
          Event = TriggerEvent.OnNext,
          Delay = delay,
          Target = new TargetElement()
        };
        condition3.Target.SlideTarget = new SlideTarget();
        conditionList.Add(condition3);
        break;
      case TriggerEvent.OnPrev:
        Condition condition4 = new Condition()
        {
          Event = TriggerEvent.OnPrev,
          Delay = delay,
          Target = new TargetElement()
        };
        condition4.Target.SlideTarget = new SlideTarget();
        conditionList.Add(condition4);
        break;
    }
  }

  internal static void UpdateRootTimeNode(
    List<object> childTimeNodeList,
    Effect effect,
    ref uint cTnId,
    bool isMainSeq,
    ref int parDelay)
  {
    parDelay = 0;
    ParallelTimeNode parallelTimeNode = new ParallelTimeNode();
    CommonTimeNode commonTimeNode = parallelTimeNode.CommonTimeNode = new CommonTimeNode();
    commonTimeNode.Id = ++cTnId;
    commonTimeNode.Fill = TimeNodeFill.Hold;
    commonTimeNode.StartConditionList = new List<Condition>();
    if (isMainSeq)
    {
      AnimationConstant.UpdateConditionList(commonTimeNode.StartConditionList, -1, TriggerEvent.None, (string) null);
      if (effect.Timing.TriggerType != EffectTriggerType.OnClick)
        AnimationConstant.UpdateConditionList(commonTimeNode.StartConditionList, 0, TriggerEvent.OnBegin, (string) null);
    }
    else
      AnimationConstant.UpdateConditionList(commonTimeNode.StartConditionList, 0, TriggerEvent.None, (string) null);
    commonTimeNode.ChildTimeNodeList = new List<object>();
    AnimationConstant.UpdateParentTimeNode(commonTimeNode.ChildTimeNodeList, effect, ref cTnId, false, ref parDelay);
    childTimeNodeList.Add((object) parallelTimeNode);
  }

  internal static void UpdateParentTimeNode(
    List<object> childTimeNodeList,
    Effect effect,
    ref uint cTnId,
    bool isAfterPrev,
    ref int parDelay)
  {
    ParallelTimeNode parallelTimeNode = new ParallelTimeNode();
    CommonTimeNode commonTimeNode = parallelTimeNode.CommonTimeNode = new CommonTimeNode();
    commonTimeNode.Id = ++cTnId;
    commonTimeNode.Fill = TimeNodeFill.Hold;
    commonTimeNode.StartConditionList = new List<Condition>();
    if (isAfterPrev)
      AnimationConstant.UpdateConditionList(commonTimeNode.StartConditionList, parDelay, TriggerEvent.None, (string) null);
    else
      AnimationConstant.UpdateConditionList(commonTimeNode.StartConditionList, 0, TriggerEvent.None, (string) null);
    commonTimeNode.ChildTimeNodeList = new List<object>();
    AnimationConstant.UpdateChildTimeNode(commonTimeNode.ChildTimeNodeList, effect, ref cTnId);
    childTimeNodeList.Add((object) parallelTimeNode);
  }

  internal static void UpdateChildTimeNode(
    List<object> childTimeNodeList,
    Effect effect,
    ref uint cTnId)
  {
    ParallelTimeNode parallelTimeNode = new ParallelTimeNode();
    CommonTimeNode cTn = parallelTimeNode.CommonTimeNode = new CommonTimeNode();
    cTn.PresetId = AnimationConstant.GetEffectTypeValue(effect.Type, effect.PresetClassType);
    cTn.PresetClass = effect.PresetClassType;
    cTn.PresetSubtype = AnimationConstant.GetSubTypeValue(effect.Subtype, effect.Type);
    AnimationConstant.UpdateCommonTimeNode(cTn, effect.Timing, ref cTnId);
    cTn.NodeType = AnimationConstant.GetTimeNodeType(effect.Timing.TriggerType);
    cTn.GroupId = (float) effect.GroupID;
    if (effect.Iterate != null)
      cTn.Iterate = effect.Iterate;
    cTn.ChildTimeNodeList = new List<object>();
    AnimationConstant.UpdateBehaviors(cTn.ChildTimeNodeList, effect, ref cTnId);
    childTimeNodeList.Add((object) parallelTimeNode);
  }

  internal static void UpdateCommonTimeNode(CommonTimeNode cTn, ITiming timing, ref uint cTnId)
  {
    cTn.Id = ++cTnId;
    cTn.Acceleration = (int) ((double) timing.Accelerate * 100000.0);
    cTn.AutoReverse = timing.AutoReverse;
    cTn.Deceleration = (int) ((double) timing.Decelerate * 100000.0);
    cTn.Duration = !float.IsNaN(timing.Duration) ? ((double) timing.Duration != double.PositiveInfinity ? (timing.Duration * 1000f).ToString() : "indefinite") : (string) null;
    cTn.RepeatCount = (double) timing.RepeatCount != 1.0 ? ((double) timing.RepeatCount != double.PositiveInfinity ? (timing.RepeatCount * 1000f).ToString() : "indefinite") : (string) null;
    cTn.RepeatDuration = !float.IsNaN(timing.RepeatDuration) ? ((double) timing.RepeatDuration != double.PositiveInfinity ? (timing.RepeatDuration * 1000f).ToString() : "indefinite") : (string) null;
    cTn.Restart = timing.Restart;
    cTn.Speed = (double) timing.Speed == 1.0 ? 0 : (int) ((double) timing.Speed * 100000.0);
    if (!float.IsNaN(timing.TriggerDelayTime))
    {
      cTn.StartConditionList = new List<Condition>();
      float delay = timing.TriggerDelayTime * 1000f;
      AnimationConstant.UpdateConditionList(cTn.StartConditionList, (int) delay, TriggerEvent.None, (string) null);
    }
    cTn.NodeType = timing.TriggerType != EffectTriggerType.AfterPrevious ? AnimationConstant.GetTimeNodeType(timing.TriggerType) : TimeNodeType.None;
    cTn.Fill = (timing as Timing).Fill;
  }

  internal static void UpdateCommonBehavior(Behavior behavior, ref uint cTnId)
  {
    CommonBehavior commonBehavior = behavior.CommonBehavior;
    ITiming timing = behavior.Timing;
    commonBehavior.Accumulate = AnimationConstant.GetAccumulate(behavior.Accumulate);
    commonBehavior.Additive = behavior.Additive;
    AnimationConstant.UpdateCommonTimeNode(commonBehavior.CommonTimeNode, timing, ref cTnId);
  }

  private static void UpdatePropertyPoints(PropertyEffect propertyEffect)
  {
    if (propertyEffect.TAVList.Count != 0 || propertyEffect.Points.Count <= 0)
      return;
    foreach (AnimationPoint point in (IEnumerable<IAnimationPoint>) propertyEffect.Points)
    {
      TimeAnimateValue timeAnimateValue = new TimeAnimateValue();
      if (point.Formula != null)
        timeAnimateValue.Formula = point.Formula;
      if ((double) point.Time > -1.0)
        timeAnimateValue.Time = (int) point.Time * 1000;
      if (point.Value != null)
      {
        timeAnimateValue.TimeValue = new Values();
        if (point.Value is bool?)
          timeAnimateValue.TimeValue.Bool = (bool?) point.Value;
        else if (point.Value is bool)
          timeAnimateValue.TimeValue.Bool = new bool?((bool) point.Value);
        else if (point.Value is float)
          timeAnimateValue.TimeValue.Float = new float?((float) point.Value);
        else if (point.Value is int)
          timeAnimateValue.TimeValue.Int = new int?((int) point.Value);
        else if (point.Value is ColorValues)
          timeAnimateValue.TimeValue.Color = (ColorValues) point.Value;
        else
          timeAnimateValue.TimeValue.String = !(point.Value is string) ? point.Value.ToString() : (string) point.Value;
        propertyEffect.TAVList.Add(timeAnimateValue);
      }
    }
    (propertyEffect.Points as AnimationPoints).Clear();
  }

  internal static void UpdateBehaviors(
    List<object> childTimeNodeList,
    Effect effect,
    ref uint cTnId)
  {
    foreach (Behavior behavior in (IEnumerable<IBehavior>) effect.Behaviors)
    {
      if (behavior is PropertyEffect)
        AnimationConstant.UpdatePropertyPoints(behavior as PropertyEffect);
      AnimationConstant.UpdateCommonBehavior(behavior, ref cTnId);
      childTimeNodeList.Add((object) behavior);
    }
  }

  internal static CommonTimeNode CreateRootElements(List<object> timeNodeList, ref uint cTnId)
  {
    ParallelTimeNode parallelTimeNode = new ParallelTimeNode();
    CommonTimeNode rootElements = parallelTimeNode.CommonTimeNode = new CommonTimeNode();
    rootElements.Id = ++cTnId;
    rootElements.Duration = "indefinite";
    rootElements.NodeType = TimeNodeType.TmRoot;
    rootElements.Restart = EffectRestartType.Never;
    rootElements.ChildTimeNodeList = new List<object>();
    timeNodeList.Add((object) parallelTimeNode);
    return rootElements;
  }

  internal static float GetSubTypeValue(EffectSubtype effectSubType, EffectType effectType)
  {
    Dictionary<float, EffectSubtype> subType = AnimationConstant.GetSubTypeDictionary()[effectType];
    if (subType.Count > 0)
    {
      foreach (KeyValuePair<float, EffectSubtype> keyValuePair in subType)
      {
        if (effectSubType == EffectSubtype.None || keyValuePair.Value == effectSubType)
          return keyValuePair.Key;
      }
    }
    return 0.0f;
  }

  internal static float GetEffectTypeValue(
    EffectType effectType,
    EffectPresetClassType presetClassType)
  {
    foreach (KeyValuePair<float, EffectType> keyValuePair in AnimationConstant.GetEffectTypeDictionary()[presetClassType])
    {
      if (keyValuePair.Value == effectType)
        return keyValuePair.Key;
    }
    return 0.0f;
  }
}
