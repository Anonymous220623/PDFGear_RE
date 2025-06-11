// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Drawing;
using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.Rendering;
using Syncfusion.Presentation.SlideImplementation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Xml;

#nullable disable
namespace Syncfusion.Presentation.SlideToImageConverter;

internal class SlideToImageConverter
{
  private static bool? m_isAzureCompatible;

  internal SlideToImageConverter()
  {
  }

  internal static bool IsAzureCompatible
  {
    get
    {
      if (!Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.m_isAzureCompatible.HasValue)
        Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.m_isAzureCompatible = new bool?(Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.IsEMFAzureCompatible());
      return Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.m_isAzureCompatible.Value;
    }
  }

  private static bool IsEMFAzureCompatible()
  {
    try
    {
      Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.CreateImage();
      return false;
    }
    catch
    {
      return true;
    }
  }

  private static void CreateImage()
  {
    int width = Syncfusion.Presentation.Drawing.Helper.PointToPixel(540.0);
    int height = Syncfusion.Presentation.Drawing.Helper.PointToPixel(720.0);
    using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
    {
      width = (int) ((double) width / 96.0 * (double) graphics.DpiX);
      height = (int) ((double) height / 96.0 * (double) graphics.DpiY);
    }
    MemoryStream memoryStream = new MemoryStream();
    using (Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppPArgb))
    {
      using (Graphics graphics = Graphics.FromImage((System.Drawing.Image) bitmap))
      {
        IntPtr hdc = graphics.GetHdc();
        RectangleF frameRect = new RectangleF(0.0f, 0.0f, (float) width, (float) height);
        System.Drawing.Image image = (System.Drawing.Image) new Metafile((Stream) memoryStream, hdc, frameRect, MetafileFrameUnit.Pixel, EmfType.EmfPlusDual);
        graphics.ReleaseHdc(hdc);
        graphics.Dispose();
        image.Dispose();
      }
    }
    memoryStream.Dispose();
  }

  internal System.Drawing.Image ConvertToImage(Slide slide, ImageType imageType)
  {
    MemoryStream stream = new MemoryStream();
    System.Drawing.Image image = this.CreateImage(slide, imageType, stream);
    using (Graphics graphics = slide.Presentation.Graphics = Graphics.FromImage(image))
    {
      Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.SetGraphicsProperties(graphics);
      GDIRenderer gdiRenderer = new GDIRenderer();
      slide.Presentation.Renderer = (RendererBase) gdiRenderer;
      slide.Layout();
      gdiRenderer.DrawSlide(slide);
    }
    return image;
  }

  internal Stream ConvertToImage(Slide slide, Syncfusion.Drawing.ImageFormat imageFormat)
  {
    MemoryStream stream = new MemoryStream();
    System.Drawing.Image image = this.CreateImage(slide, ImageType.Metafile, stream);
    using (Graphics graphics = slide.Presentation.Graphics = Graphics.FromImage(image))
    {
      Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.SetGraphicsProperties(graphics);
      GDIRenderer gdiRenderer = new GDIRenderer();
      slide.Presentation.Renderer = (RendererBase) gdiRenderer;
      slide.Layout();
      gdiRenderer.DrawSlide(slide);
    }
    stream.Position = 0L;
    return (Stream) stream;
  }

  internal Stream ConvertToImage(NotesSlide slide, Syncfusion.Drawing.ImageFormat imageFormat)
  {
    MemoryStream stream = new MemoryStream();
    System.Drawing.Image image = this.CreateImage(slide, ImageType.Metafile, stream);
    using (Graphics graphics = slide.Presentation.Graphics = Graphics.FromImage(image))
    {
      Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.SetGraphicsProperties(graphics);
      GDIRenderer gdiRenderer = new GDIRenderer();
      slide.Presentation.Renderer = (RendererBase) gdiRenderer;
      slide.Layout();
      gdiRenderer.DrawSlide(slide);
    }
    stream.Position = 0L;
    return (Stream) stream;
  }

  internal static void SetGraphicsProperties(Graphics graphics)
  {
    graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
    graphics.SmoothingMode = SmoothingMode.AntiAlias;
    graphics.CompositingQuality = CompositingQuality.GammaCorrected;
    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
    graphics.PageUnit = GraphicsUnit.Point;
  }

  internal System.Drawing.Image CreateImage(Slide slide, ImageType imageType, MemoryStream stream)
  {
    int num1 = Syncfusion.Presentation.Drawing.Helper.PointToPixel(slide.Presentation.SlideSize.Width);
    int num2 = Syncfusion.Presentation.Drawing.Helper.PointToPixel(slide.Presentation.SlideSize.Height);
    if (Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.IsAzureCompatible)
      imageType = ImageType.Bitmap;
    using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
    {
      num1 = (int) ((double) num1 / 96.0 * (double) graphics.DpiX);
      num2 = (int) ((double) num2 / 96.0 * (double) graphics.DpiY);
    }
    System.Drawing.Image image = (System.Drawing.Image) null;
    if (slide.IsPortableRendering)
    {
      image = (slide.Background as Background).Fill.FillType != FillType.Automatic || (slide.LayoutSlide.Background as Background).Fill.FillType != FillType.Automatic ? (System.Drawing.Image) new Bitmap(num1, num2, PixelFormat.Format32bppPArgb) : this.DrawMasterBackground(slide, num1, num2, image, imageType);
      using (Graphics graphics = Graphics.FromImage(image))
        (image as Bitmap).SetResolution(graphics.DpiX, graphics.DpiY);
    }
    else
    {
      switch (imageType)
      {
        case ImageType.Metafile:
          if ((slide.Background as Background).Fill.FillType == FillType.Automatic && (slide.LayoutSlide.Background as Background).Fill.FillType == FillType.Automatic)
            image = this.DrawMasterBackground(slide, num1, num2, image, imageType);
          using (Bitmap bitmap = new Bitmap(num1, num2))
          {
            using (Graphics graphics = Graphics.FromImage((System.Drawing.Image) bitmap))
            {
              bitmap.SetResolution(graphics.DpiX, graphics.DpiY);
              IntPtr hdc = graphics.GetHdc();
              Rectangle frameRect = new Rectangle(0, 0, num1, num2);
              image = (System.Drawing.Image) new Metafile((Stream) stream, hdc, frameRect, MetafileFrameUnit.Pixel, EmfType.EmfPlusOnly);
              graphics.ReleaseHdc();
              break;
            }
          }
        case ImageType.Bitmap:
          image = (slide.Background as Background).Fill.FillType != FillType.Automatic || (slide.LayoutSlide.Background as Background).Fill.FillType != FillType.Automatic ? (System.Drawing.Image) new Bitmap(num1, num2, PixelFormat.Format32bppPArgb) : this.DrawMasterBackground(slide, num1, num2, image, imageType);
          using (Graphics graphics = Graphics.FromImage(image))
          {
            (image as Bitmap).SetResolution(graphics.DpiX, graphics.DpiY);
            break;
          }
      }
    }
    return image;
  }

  internal System.Drawing.Image CreateImage(
    NotesSlide slide,
    ImageType imageType,
    MemoryStream stream)
  {
    int width = Syncfusion.Presentation.Drawing.Helper.PointToPixel(Syncfusion.Presentation.Drawing.Helper.EmuToPoint(((NotesSize) slide.Presentation.NotesSize).CX));
    int height = Syncfusion.Presentation.Drawing.Helper.PointToPixel(Syncfusion.Presentation.Drawing.Helper.EmuToPoint(((NotesSize) slide.Presentation.NotesSize).CY));
    if (Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.IsAzureCompatible)
      imageType = ImageType.Bitmap;
    using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
    {
      width = (int) ((double) width / 96.0 * (double) graphics.DpiX);
      height = (int) ((double) height / 96.0 * (double) graphics.DpiY);
    }
    System.Drawing.Image image = (System.Drawing.Image) null;
    if (slide.IsPortableRendering)
    {
      image = (System.Drawing.Image) new Bitmap(width, height, PixelFormat.Format32bppPArgb);
      using (Graphics graphics = Graphics.FromImage(image))
        (image as Bitmap).SetResolution(graphics.DpiX, graphics.DpiY);
    }
    else
    {
      switch (imageType)
      {
        case ImageType.Metafile:
          using (Bitmap bitmap = new Bitmap(width, height))
          {
            using (Graphics graphics = Graphics.FromImage((System.Drawing.Image) bitmap))
            {
              bitmap.SetResolution(graphics.DpiX, graphics.DpiY);
              IntPtr hdc = graphics.GetHdc();
              Rectangle frameRect = new Rectangle(0, 0, width, height);
              image = (System.Drawing.Image) new Metafile((Stream) stream, hdc, frameRect, MetafileFrameUnit.Pixel, EmfType.EmfPlusOnly);
              graphics.ReleaseHdc();
              break;
            }
          }
        case ImageType.Bitmap:
          image = (System.Drawing.Image) new Bitmap(width, height, PixelFormat.Format32bppPArgb);
          using (Graphics graphics = Graphics.FromImage(image))
          {
            (image as Bitmap).SetResolution(graphics.DpiX, graphics.DpiY);
            break;
          }
      }
    }
    return image;
  }

  private System.Drawing.Image DrawMasterBackground(
    Slide slide,
    int slideWidth,
    int slideHeight,
    System.Drawing.Image result,
    ImageType imageType)
  {
    switch (imageType)
    {
      case ImageType.Metafile:
        using (Bitmap bitmap = new Bitmap(slideWidth, slideHeight))
        {
          using (Graphics graphics = Graphics.FromImage((System.Drawing.Image) bitmap))
          {
            Stream stream = (Stream) new MemoryStream();
            bitmap.SetResolution(graphics.DpiX, graphics.DpiY);
            IntPtr hdc = graphics.GetHdc();
            Rectangle frameRect = new Rectangle(0, 0, slideWidth, slideHeight);
            result = (System.Drawing.Image) new Metafile(stream, hdc, frameRect, MetafileFrameUnit.Pixel, EmfType.EmfPlusOnly);
            graphics.ReleaseHdc();
          }
        }
        GDIRenderer gdiRenderer1 = new GDIRenderer((ISlide) slide);
        Graphics graphics1 = Graphics.FromImage(result);
        gdiRenderer1.Graphics = graphics1;
        Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.SetGraphicsProperties(graphics1);
        GraphicsPath path1 = new GraphicsPath();
        path1.AddRectangle(new RectangleF(0.0f, 0.0f, (float) slide.Presentation.SlideSize.Width, (float) slide.Presentation.SlideSize.Height));
        gdiRenderer1.FillBackground((IShape) null, path1, (slide.LayoutSlide.MasterSlide.Background as Background).Fill);
        graphics1.Dispose();
        return result;
      case ImageType.Bitmap:
        result = (System.Drawing.Image) new Bitmap(slideWidth, slideHeight, PixelFormat.Format32bppPArgb);
        GDIRenderer gdiRenderer2 = new GDIRenderer((ISlide) slide);
        Graphics graphics2 = Graphics.FromImage(result);
        gdiRenderer2.Graphics = graphics2;
        Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.SetGraphicsProperties(graphics2);
        GraphicsPath path2 = new GraphicsPath();
        path2.AddRectangle(new RectangleF(0.0f, 0.0f, (float) slide.Presentation.SlideSize.Width, (float) slide.Presentation.SlideSize.Height));
        gdiRenderer2.FillBackground((IShape) null, path2, (slide.LayoutSlide.MasterSlide.Background as Background).Fill);
        return result;
      default:
        return (System.Drawing.Image) null;
    }
  }

  internal static GraphicsPath[] GetGraphicsPath(
    Shape shape,
    RectangleF bounds,
    ref Pen pen,
    GDIRenderer gDIRenderer)
  {
    GraphicsPath[] graphicsPath;
    if (shape.GetCustomGeometry())
    {
      graphicsPath = Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetCustomGeomentryPath(bounds, shape);
      if (graphicsPath.Length == 0)
        graphicsPath = new GraphicsPath[1]
        {
          new GraphicsPath()
        };
    }
    else
      graphicsPath = new GraphicsPath[1]
      {
        Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetPresetGeomentryPath(shape, bounds, ref pen, gDIRenderer)
      };
    return graphicsPath;
  }

  private static GraphicsPath GetPresetGeomentryPath(
    Shape shape,
    RectangleF bounds,
    ref Pen pen,
    GDIRenderer gDIRenderer)
  {
    ShapePath shapePath = new ShapePath(bounds, shape.ShapeGuide);
    GraphicsPath presetGeomentryPath = new GraphicsPath();
    switch (shape.GetAutoShapeType())
    {
      case AutoShapeType.Rectangle:
      case AutoShapeType.FlowChartProcess:
        presetGeomentryPath.AddRectangle(bounds);
        return presetGeomentryPath;
      case AutoShapeType.Parallelogram:
      case AutoShapeType.FlowChartData:
        return shapePath.GetParallelogramPath();
      case AutoShapeType.Trapezoid:
        return shapePath.GetTrapezoidPath();
      case AutoShapeType.Diamond:
      case AutoShapeType.FlowChartDecision:
        PointF[] points1 = new PointF[4]
        {
          new PointF(bounds.X, bounds.Y + bounds.Height / 2f),
          new PointF(bounds.X + bounds.Width / 2f, bounds.Y),
          new PointF(bounds.Right, bounds.Y + bounds.Height / 2f),
          new PointF(bounds.X + bounds.Width / 2f, bounds.Bottom)
        };
        presetGeomentryPath.AddLines(points1);
        presetGeomentryPath.CloseFigure();
        break;
      case AutoShapeType.RoundedRectangle:
        return shapePath.GetRoundedRectanglePath();
      case AutoShapeType.Octagon:
        return shapePath.GetOctagonPath();
      case AutoShapeType.IsoscelesTriangle:
        return shapePath.GetTrianglePath();
      case AutoShapeType.RightTriangle:
        PointF[] points2 = new PointF[3]
        {
          new PointF(bounds.X, bounds.Bottom),
          new PointF(bounds.X, bounds.Y),
          new PointF(bounds.Right, bounds.Bottom)
        };
        presetGeomentryPath.AddLines(points2);
        presetGeomentryPath.CloseFigure();
        return presetGeomentryPath;
      case AutoShapeType.Oval:
        presetGeomentryPath.AddEllipse(bounds);
        return presetGeomentryPath;
      case AutoShapeType.Hexagon:
        return shapePath.GetHexagonPath();
      case AutoShapeType.Cross:
        return shapePath.GetCrossPath();
      case AutoShapeType.RegularPentagon:
        return shapePath.GetRegularPentagonPath();
      case AutoShapeType.Can:
        return shapePath.GetCanPath();
      case AutoShapeType.Cube:
        return shapePath.GetCubePath();
      case AutoShapeType.Bevel:
        return shapePath.GetBevelPath();
      case AutoShapeType.FoldedCorner:
        return shapePath.GetFoldedCornerPath();
      case AutoShapeType.SmileyFace:
        if (pen != null)
        {
          GraphicsPath[] smileyFacePath = shapePath.GetSmileyFacePath();
          foreach (GraphicsPath path in smileyFacePath)
          {
            path.FillMode = FillMode.Winding;
            gDIRenderer.FillBackground((IShape) shape, path, shape.GetDefaultFillFormat());
            gDIRenderer.Graphics.DrawPath(pen, path);
          }
          if (shape is Picture)
          {
            presetGeomentryPath = smileyFacePath[0];
            break;
          }
          break;
        }
        break;
      case AutoShapeType.Donut:
        return shapePath.GetDonutPath((shape.LineFormat as LineFormat).GetDefaultWidth());
      case AutoShapeType.NoSymbol:
        return shapePath.GetNoSymbolPath();
      case AutoShapeType.BlockArc:
        return shapePath.GetBlockArcPath();
      case AutoShapeType.Heart:
        return shapePath.GetHeartPath();
      case AutoShapeType.LightningBolt:
        return shapePath.GetLightningBoltPath();
      case AutoShapeType.Sun:
        return shapePath.GetSunPath();
      case AutoShapeType.Moon:
        return shapePath.GetMoonPath();
      case AutoShapeType.Arc:
        if (pen != null)
        {
          GraphicsPath[] arcPath = shapePath.GetArcPath();
          gDIRenderer.FillBackground((IShape) shape, arcPath[1], shape.GetDefaultFillFormat());
          gDIRenderer.Graphics.DrawPath(pen, arcPath[0]);
          if (shape is Picture)
          {
            presetGeomentryPath = arcPath[1];
            break;
          }
          break;
        }
        break;
      case AutoShapeType.DoubleBracket:
        return shapePath.GetDoubleBracketPath();
      case AutoShapeType.DoubleBrace:
        return shapePath.GetDoubleBracePath();
      case AutoShapeType.Plaque:
        return shapePath.GetPlaquePath();
      case AutoShapeType.LeftBracket:
        return shapePath.GetLeftBracketPath();
      case AutoShapeType.RightBracket:
        return shapePath.GetRightBracketPath();
      case AutoShapeType.LeftBrace:
        return shapePath.GetLeftBracePath();
      case AutoShapeType.RightBrace:
        return shapePath.GetRightBracePath();
      case AutoShapeType.RightArrow:
        return shapePath.GetRightArrowPath();
      case AutoShapeType.LeftArrow:
        return shapePath.GetLeftArrowPath();
      case AutoShapeType.UpArrow:
        return shapePath.GetUpArrowPath();
      case AutoShapeType.DownArrow:
        return shapePath.GetDownArrowPath();
      case AutoShapeType.LeftRightArrow:
        return shapePath.GetLeftRightArrowPath();
      case AutoShapeType.UpDownArrow:
        return shapePath.GetUpDownArrowPath();
      case AutoShapeType.QuadArrow:
        return shapePath.GetQuadArrowPath();
      case AutoShapeType.LeftRightUpArrow:
        return shapePath.GetLeftRightUpArrowPath();
      case AutoShapeType.BentArrow:
        return shapePath.GetBentArrowPath();
      case AutoShapeType.UTurnArrow:
        return shapePath.GetUTrunArrowPath();
      case AutoShapeType.LeftUpArrow:
        return shapePath.GetLeftUpArrowPath();
      case AutoShapeType.BentUpArrow:
        return shapePath.GetBentUpArrowPath();
      case AutoShapeType.CurvedRightArrow:
        return shapePath.GetCurvedRightArrowPath();
      case AutoShapeType.CurvedLeftArrow:
        return shapePath.GetCurvedLeftArrowPath();
      case AutoShapeType.CurvedUpArrow:
        return shapePath.GetCurvedUpArrowPath();
      case AutoShapeType.CurvedDownArrow:
        return shapePath.GetCurvedDownArrowPath();
      case AutoShapeType.StripedRightArrow:
        return shapePath.GetStripedRightArrowPath();
      case AutoShapeType.NotchedRightArrow:
        return shapePath.GetNotchedRightArrowPath();
      case AutoShapeType.Pentagon:
        return shapePath.GetPentagonPath();
      case AutoShapeType.Chevron:
        return shapePath.GetChevronPath();
      case AutoShapeType.RightArrowCallout:
        return shapePath.GetRightArrowCalloutPath();
      case AutoShapeType.LeftArrowCallout:
        return shapePath.GetLeftArrowCalloutPath();
      case AutoShapeType.UpArrowCallout:
        return shapePath.GetUpArrowCalloutPath();
      case AutoShapeType.DownArrowCallout:
        return shapePath.GetDownArrowCalloutPath();
      case AutoShapeType.LeftRightArrowCallout:
        return shapePath.GetLeftRightArrowCalloutPath();
      case AutoShapeType.QuadArrowCallout:
        return shapePath.GetQuadArrowCalloutPath();
      case AutoShapeType.CircularArrow:
        return shapePath.GetCircularArrowPath();
      case AutoShapeType.FlowChartAlternateProcess:
        return shapePath.GetFlowChartAlternateProcessPath();
      case AutoShapeType.FlowChartPredefinedProcess:
        return shapePath.GetFlowChartPredefinedProcessPath();
      case AutoShapeType.FlowChartInternalStorage:
        return shapePath.GetFlowChartInternalStoragePath();
      case AutoShapeType.FlowChartDocument:
        return shapePath.GetFlowChartDocumentPath();
      case AutoShapeType.FlowChartMultiDocument:
        return shapePath.GetFlowChartMultiDocumentPath();
      case AutoShapeType.FlowChartTerminator:
        return shapePath.GetFlowChartTerminatorPath();
      case AutoShapeType.FlowChartPreparation:
        return shapePath.GetFlowChartPreparationPath();
      case AutoShapeType.FlowChartManualInput:
        return shapePath.GetFlowChartManualInputPath();
      case AutoShapeType.FlowChartManualOperation:
        return shapePath.GetFlowChartManualOperationPath();
      case AutoShapeType.FlowChartConnector:
        return shapePath.GetFlowChartConnectorPath();
      case AutoShapeType.FlowChartOffPageConnector:
        return shapePath.GetFlowChartOffPageConnectorPath();
      case AutoShapeType.FlowChartCard:
        return shapePath.GetFlowChartCardPath();
      case AutoShapeType.FlowChartPunchedTape:
        return shapePath.GetFlowChartPunchedTapePath();
      case AutoShapeType.FlowChartSummingJunction:
        return shapePath.GetFlowChartSummingJunctionPath();
      case AutoShapeType.FlowChartOr:
        return shapePath.GetFlowChartOrPath();
      case AutoShapeType.FlowChartCollate:
        return shapePath.GetFlowChartCollatePath();
      case AutoShapeType.FlowChartSort:
        return shapePath.GetFlowChartSortPath();
      case AutoShapeType.FlowChartExtract:
        return shapePath.GetFlowChartExtractPath();
      case AutoShapeType.FlowChartMerge:
        return shapePath.GetFlowChartMergePath();
      case AutoShapeType.FlowChartStoredData:
        return shapePath.GetFlowChartOnlineStoragePath();
      case AutoShapeType.FlowChartDelay:
        return shapePath.GetFlowChartDelayPath();
      case AutoShapeType.FlowChartSequentialAccessStorage:
        return shapePath.GetFlowChartSequentialAccessStoragePath();
      case AutoShapeType.FlowChartMagneticDisk:
        return shapePath.GetFlowChartMagneticDiskPath();
      case AutoShapeType.FlowChartDirectAccessStorage:
        return shapePath.GetFlowChartDirectAccessStoragePath();
      case AutoShapeType.FlowChartDisplay:
        return shapePath.GetFlowChartDisplayPath();
      case AutoShapeType.Explosion1:
        return shapePath.GetExplosion1();
      case AutoShapeType.Explosion2:
        return shapePath.GetExplosion2();
      case AutoShapeType.Star4Point:
        return shapePath.GetStar4Point();
      case AutoShapeType.Star5Point:
        return shapePath.GetStar5Point();
      case AutoShapeType.Star8Point:
        return shapePath.GetStar8Point();
      case AutoShapeType.Star16Point:
        return shapePath.GetStar16Point();
      case AutoShapeType.Star24Point:
        return shapePath.GetStar24Point();
      case AutoShapeType.Star32Point:
        return shapePath.GetStar32Point();
      case AutoShapeType.UpRibbon:
        return shapePath.GetUpRibbon();
      case AutoShapeType.DownRibbon:
        return shapePath.GetDownRibbon();
      case AutoShapeType.CurvedUpRibbon:
        return shapePath.GetCurvedUpRibbon();
      case AutoShapeType.CurvedDownRibbon:
        return shapePath.GetCurvedDownRibbon();
      case AutoShapeType.VerticalScroll:
        return shapePath.GetVerticalScroll();
      case AutoShapeType.HorizontalScroll:
        if (pen != null)
        {
          GraphicsPath[] horizontalScroll = shapePath.GetHorizontalScroll();
          foreach (GraphicsPath path in horizontalScroll)
          {
            gDIRenderer.FillBackground((IShape) shape, path, shape.GetDefaultFillFormat());
            gDIRenderer.Graphics.DrawPath(pen, path);
          }
          if (shape is Picture)
          {
            presetGeomentryPath = horizontalScroll[0];
            break;
          }
          break;
        }
        break;
      case AutoShapeType.Wave:
        return shapePath.GetWave();
      case AutoShapeType.DoubleWave:
        return shapePath.GetDoubleWave();
      case AutoShapeType.RectangularCallout:
        return shapePath.GetRectangularCalloutPath();
      case AutoShapeType.RoundedRectangularCallout:
        return shapePath.GetRoundedRectangularCalloutPath();
      case AutoShapeType.OvalCallout:
        return shapePath.GetOvalCalloutPath();
      case AutoShapeType.CloudCallout:
        return shapePath.GetCloudCalloutPath();
      case AutoShapeType.LineCallout1:
      case AutoShapeType.LineCallout1NoBorder:
        return shapePath.GetLineCallout1Path();
      case AutoShapeType.LineCallout2:
      case AutoShapeType.LineCallout2NoBorder:
        return shapePath.GetLineCallout2Path();
      case AutoShapeType.LineCallout3:
      case AutoShapeType.LineCallout3NoBorder:
        return shapePath.GetLineCallout3Path();
      case AutoShapeType.LineCallout1AccentBar:
      case AutoShapeType.LineCallout1BorderAndAccentBar:
        return shapePath.GetLineCallout1AccentBarPath();
      case AutoShapeType.LineCallout2AccentBar:
      case AutoShapeType.LineCallout2BorderAndAccentBar:
        return shapePath.GetLineCallout2AccentBarPath();
      case AutoShapeType.LineCallout3AccentBar:
      case AutoShapeType.LineCallout3BorderAndAccentBar:
        return shapePath.GetLineCallout3AccentBarPath();
      case AutoShapeType.LeftRightRibbon:
        return shapePath.GetLeftRightRibbonPath();
      case AutoShapeType.DiagonalStripe:
        return shapePath.GetDiagonalStripePath();
      case AutoShapeType.Pie:
        return shapePath.GetPiePath();
      case AutoShapeType.Decagon:
        return shapePath.GetDecagonPath();
      case AutoShapeType.Heptagon:
        return shapePath.GetHeptagonPath();
      case AutoShapeType.Dodecagon:
        return shapePath.GetDodecagonPath();
      case AutoShapeType.Star6Point:
        return shapePath.GetStar6Point();
      case AutoShapeType.Star7Point:
        return shapePath.GetStar7Point();
      case AutoShapeType.Star10Point:
        return shapePath.GetStar10Point();
      case AutoShapeType.Star12Point:
        return shapePath.GetStar12Point();
      case AutoShapeType.RoundSingleCornerRectangle:
        return shapePath.GetRoundSingleCornerRectanglePath();
      case AutoShapeType.RoundSameSideCornerRectangle:
        return shapePath.GetRoundSameSideCornerRectanglePath();
      case AutoShapeType.RoundDiagonalCornerRectangle:
        return shapePath.GetRoundDiagonalCornerRectanglePath();
      case AutoShapeType.SnipAndRoundSingleCornerRectangle:
        return shapePath.GetSnipAndRoundSingleCornerRectanglePath();
      case AutoShapeType.SnipSingleCornerRectangle:
        return shapePath.GetSnipSingleCornerRectanglePath();
      case AutoShapeType.SnipSameSideCornerRectangle:
        return shapePath.GetSnipSameSideCornerRectanglePath();
      case AutoShapeType.SnipDiagonalCornerRectangle:
        return shapePath.GetSnipDiagonalCornerRectanglePath();
      case AutoShapeType.Frame:
        return shapePath.GetFramePath();
      case AutoShapeType.HalfFrame:
        return shapePath.GetHalfFramePath();
      case AutoShapeType.Teardrop:
        return shapePath.GetTearDropPath();
      case AutoShapeType.Chord:
        return shapePath.GetChordPath();
      case AutoShapeType.Corner:
        return shapePath.GetL_ShapePath();
      case AutoShapeType.MathPlus:
        return shapePath.GetMathPlusPath();
      case AutoShapeType.MathMinus:
        return shapePath.GetMathMinusPath();
      case AutoShapeType.MathMultiply:
        return shapePath.GetMathMultiplyPath();
      case AutoShapeType.MathDivision:
        return shapePath.GetMathDivisionPath();
      case AutoShapeType.MathEqual:
        return shapePath.GetMathEqualPath();
      case AutoShapeType.MathNotEqual:
        return shapePath.GetMathNotEqualPath();
      case AutoShapeType.PieWedge:
        presetGeomentryPath.AddPie(bounds.X, bounds.Y, bounds.Width * 2f, bounds.Height * 2f, 180f, 90f);
        return presetGeomentryPath;
      case AutoShapeType.Gear6:
        Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.SetCustomGeometry("<pathLst><path w=\"2167466\" h=\"2167466\"><moveTo><pt x=\"1621800\" y=\"548964\"/></moveTo><lnTo><pt x=\"1941574\" y=\"452590\"/></lnTo><lnTo><pt x=\"2059240\" y=\"656392\"/></lnTo><lnTo><pt x=\"1815890\" y=\"885138\"/></lnTo><cubicBezTo><pt x=\"1851165\" y=\"1015185\"/><pt x=\"1851165\" y=\"1152281\"/><pt x=\"1815890\" y=\"1282328\"/></cubicBezTo><lnTo><pt x=\"2059240\" y=\"1511074\"/></lnTo><lnTo><pt x=\"1941574\" y=\"1714876\"/></lnTo><lnTo><pt x=\"1621800\" y=\"1618502\"/></lnTo><cubicBezTo><pt x=\"1526813\" y=\"1714075\"/><pt x=\"1408085\" y=\"1782623\"/><pt x=\"1277823\" y=\"1817097\"/></cubicBezTo><lnTo><pt x=\"1201398\" y=\"2142217\"/></lnTo><lnTo><pt x=\"966068\" y=\"2142217\"/></lnTo><lnTo><pt x=\"889643\" y=\"1817097\"/></lnTo><cubicBezTo><pt x=\"759381\" y=\"1782622\"/><pt x=\"640653\" y=\"1714074\"/><pt x=\"545666\" y=\"1618502\"/></cubicBezTo><lnTo><pt x=\"225892\" y=\"1714876\"/></lnTo><lnTo><pt x=\"108226\" y=\"1511074\"/></lnTo><lnTo><pt x=\"351576\" y=\"1282328\"/></lnTo><cubicBezTo><pt x=\"316301\" y=\"1152281\"/><pt x=\"316301\" y=\"1015185\"/><pt x=\"351576\" y=\"885138\"/></cubicBezTo><lnTo><pt x=\"108226\" y=\"656392\"/></lnTo><lnTo><pt x=\"225892\" y=\"452590\"/></lnTo><lnTo><pt x=\"545666\" y=\"548964\"/></lnTo><cubicBezTo><pt x=\"640653\" y=\"453391\"/><pt x=\"759381\" y=\"384843\"/><pt x=\"889643\" y=\"350369\"/></cubicBezTo><lnTo><pt x=\"966068\" y=\"25249\"/></lnTo><lnTo><pt x=\"1201398\" y=\"25249\"/></lnTo><lnTo><pt x=\"1277823\" y=\"350369\"/></lnTo><cubicBezTo><pt x=\"1408085\" y=\"384844\"/><pt x=\"1526813\" y=\"453392\"/><pt x=\"1621800\" y=\"548964\"/></cubicBezTo><close/></path></pathLst>", shape);
        return Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetCustomGeomentryPath(bounds, shape)[0];
      case AutoShapeType.Gear9:
        Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.SetCustomGeometry("<pathLst><path w=\"2980266\" h=\"2980266\"><moveTo><pt x=\"2115406\" y=\"475169\"/></moveTo><lnTo><pt x=\"2347223\" y=\"280641\"/></lnTo><lnTo><pt x=\"2532418\" y=\"436038\"/></lnTo><lnTo><pt x=\"2381100\" y=\"698113\"/></lnTo><cubicBezTo><pt x=\"2488696\" y=\"819151\"/><pt x=\"2570502\" y=\"960843\"/><pt x=\"2621526\" y=\"1114543\"/></cubicBezTo><lnTo><pt x=\"2924149\" y=\"1114535\"/></lnTo><lnTo><pt x=\"2966129\" y=\"1352617\"/></lnTo><lnTo><pt x=\"2681754\" y=\"1456113\"/></lnTo><cubicBezTo><pt x=\"2686376\" y=\"1617995\"/><pt x=\"2657965\" y=\"1779121\"/><pt x=\"2598255\" y=\"1929659\"/></cubicBezTo><lnTo><pt x=\"2830082\" y=\"2124176\"/></lnTo><lnTo><pt x=\"2709205\" y=\"2333542\"/></lnTo><lnTo><pt x=\"2424835\" y=\"2230031\"/></lnTo><cubicBezTo><pt x=\"2324320\" y=\"2357010\"/><pt x=\"2198986\" y=\"2462178\"/><pt x=\"2056481\" y=\"2539116\"/></cubicBezTo><lnTo><pt x=\"2109039\" y=\"2837141\"/></lnTo><lnTo><pt x=\"1881863\" y=\"2919826\"/></lnTo><lnTo><pt x=\"1730559\" y=\"2657743\"/></lnTo><cubicBezTo><pt x=\"1571939\" y=\"2690405\"/><pt x=\"1408327\" y=\"2690405\"/><pt x=\"1249707\" y=\"2657743\"/></cubicBezTo><lnTo><pt x=\"1098403\" y=\"2919826\"/></lnTo><lnTo><pt x=\"871227\" y=\"2837141\"/></lnTo><lnTo><pt x=\"923785\" y=\"2539117\"/></lnTo><cubicBezTo><pt x=\"781280\" y=\"2462179\"/><pt x=\"655947\" y=\"2357011\"/><pt x=\"555431\" y=\"2230032\"/></cubicBezTo><lnTo><pt x=\"271061\" y=\"2333542\"/></lnTo><lnTo><pt x=\"150184\" y=\"2124176\"/></lnTo><lnTo><pt x=\"382011\" y=\"1929660\"/></lnTo><cubicBezTo><pt x=\"322301\" y=\"1779122\"/><pt x=\"293890\" y=\"1617995\"/><pt x=\"298512\" y=\"1456114\"/></cubicBezTo><lnTo><pt x=\"14137\" y=\"1352617\"/></lnTo><lnTo><pt x=\"56117\" y=\"1114535\"/></lnTo><lnTo><pt x=\"358740\" y=\"1114543\"/></lnTo><cubicBezTo><pt x=\"409764\" y=\"960843\"/><pt x=\"491570\" y=\"819151\"/><pt x=\"599166\" y=\"698113\"/></cubicBezTo><lnTo><pt x=\"447848\" y=\"436038\"/></lnTo><lnTo><pt x=\"633043\" y=\"280641\"/></lnTo><lnTo><pt x=\"864860\" y=\"475169\"/></lnTo><cubicBezTo><pt x=\"1002743\" y=\"390226\"/><pt x=\"1156488\" y=\"334267\"/><pt x=\"1316713\" y=\"310708\"/></cubicBezTo><lnTo><pt x=\"1369255\" y=\"12681\"/></lnTo><lnTo><pt x=\"1611011\" y=\"12681\"/></lnTo><lnTo><pt x=\"1663553\" y=\"310708\"/></lnTo><cubicBezTo><pt x=\"1823778\" y=\"334267\"/><pt x=\"1977523\" y=\"390226\"/><pt x=\"2115406\" y=\"475169\"/></cubicBezTo><close/></path></pathLst>", shape);
        return Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetCustomGeomentryPath(bounds, shape)[0];
      case AutoShapeType.Funnel:
        return shapePath.GetFunnelPath();
      case AutoShapeType.LeftCircularArrow:
        return shapePath.GetLeftCircularArrowPath();
      case AutoShapeType.Cloud:
        return shapePath.GetCloudPath();
      case AutoShapeType.SwooshArrow:
        return shapePath.GetSwooshArrowPath();
      case AutoShapeType.Line:
      case AutoShapeType.StraightConnector:
        presetGeomentryPath.AddLine(bounds.X, bounds.Y, bounds.Right, bounds.Bottom);
        return presetGeomentryPath;
      case AutoShapeType.ElbowConnector:
        return shapePath.GetBentConnector3Path();
      case AutoShapeType.CurvedConnector:
        return shapePath.GetCurvedConnector3Path();
      case AutoShapeType.BentConnector2:
        return shapePath.GetBentConnector2Path();
      case AutoShapeType.BentConnector4:
        return shapePath.GetBentConnector4Path();
      case AutoShapeType.BentConnector5:
        return shapePath.GetBentConnector5Path();
      case AutoShapeType.CurvedConnector2:
        return shapePath.GetCurvedConnector2Path();
      case AutoShapeType.CurvedConnector4:
        return shapePath.GetCurvedConnector4Path();
      case AutoShapeType.CurvedConnector5:
        return shapePath.GetCurvedConnector5Path();
      default:
        if (shape.GetPresetGeometry() || shape.DrawingType == DrawingType.PlaceHolder)
        {
          presetGeomentryPath.AddRectangle(bounds);
          break;
        }
        break;
    }
    return presetGeomentryPath;
  }

  private static void SetCustomGeometry(string pathList, Shape shape)
  {
    Stream input = (Stream) new MemoryStream(Encoding.UTF8.GetBytes(pathList.Replace('\'', ' ')));
    input.Position = 0L;
    XmlReader reader = XmlReader.Create(input);
    shape.Path2DList = new List<Path2D>();
    Syncfusion.Presentation.Drawing.DrawingParser.ParsePath2D(reader, shape, (Dictionary<string, string>) null);
  }

  private static GraphicsPath[] GetCustomGeomentryPath(RectangleF bounds, Shape shape)
  {
    List<Path2D> path2Dlist = shape.GetPath2DList();
    bool flag = false;
    if (path2Dlist.Count > 1)
    {
      IFill fill = !shape.IsBgFill ? shape.GetDefaultFillFormat() : ((Background) shape.BaseSlide.Background).GetDefaultFillFormat();
      if (fill != null && fill.FillType == FillType.Solid)
        flag = true;
    }
    GraphicsPath[] customGeomentryPath;
    if (flag)
    {
      customGeomentryPath = new GraphicsPath[path2Dlist.Count];
      for (int index = 0; index < path2Dlist.Count; ++index)
      {
        Path2D path2D = path2Dlist[index];
        double width = path2D.Width;
        double height = path2D.Height;
        GraphicsPath path = new GraphicsPath();
        Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGeomentryPath(path, path2D.PathElements, width, height, bounds);
        customGeomentryPath[index] = path;
      }
    }
    else
    {
      customGeomentryPath = new GraphicsPath[1];
      GraphicsPath path = new GraphicsPath();
      foreach (Path2D path2D in shape.GetPath2DList())
      {
        double width = path2D.Width;
        double height = path2D.Height;
        Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGeomentryPath(path, path2D.PathElements, width, height, bounds);
      }
      customGeomentryPath[0] = path;
    }
    return customGeomentryPath;
  }

  private static float GetGeomentryPathYValue(double pathHeight, double y, RectangleF bounds)
  {
    if (pathHeight == 0.0)
      return bounds.Y + (float) Syncfusion.Presentation.Drawing.Helper.EmuToPoint(y);
    double num = y * 100.0 / pathHeight;
    return (float) ((double) bounds.Height * num / 100.0) + bounds.Y;
  }

  private static float GetGeomentryPathXValue(double pathWidth, double x, RectangleF bounds)
  {
    if (pathWidth == 0.0)
      return bounds.X + (float) Syncfusion.Presentation.Drawing.Helper.EmuToPoint(x);
    double num = x * 100.0 / pathWidth;
    return (float) ((double) bounds.Width * num / 100.0) + bounds.X;
  }

  private static void GetGeomentryPath(
    GraphicsPath path,
    List<double> pathElements,
    double pathWidth,
    double pathHeight,
    RectangleF bounds)
  {
    PointF pt1 = (PointF) Point.Empty;
    double num = 0.0;
    for (int index = 0; index < pathElements.Count; index = index + ((int) num + 1) + 1)
    {
      switch ((ushort) pathElements[index])
      {
        case 1:
          path.CloseFigure();
          pt1 = (PointF) Point.Empty;
          num = 0.0;
          break;
        case 2:
          path.CloseFigure();
          num = pathElements[index + 1] * 2.0;
          pt1 = new PointF(Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds));
          break;
        case 3:
          num = pathElements[index + 1] * 2.0;
          PointF pt2 = new PointF(Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds));
          path.AddLine(pt1, pt2);
          pt1 = pt2;
          break;
        case 4:
          num = pathElements[index + 1] * 2.0;
          RectangleF rect = new RectangleF();
          rect.X = bounds.X;
          rect.Y = bounds.Y;
          rect.Width = (float) Syncfusion.Presentation.Drawing.Helper.EmuToPoint(pathElements[index + 2]) * 2f;
          rect.Height = (float) Syncfusion.Presentation.Drawing.Helper.EmuToPoint(pathElements[index + 3]) * 2f;
          float startAngle = (float) pathElements[index + 4] / 60000f;
          float sweepAngle = (float) pathElements[index + 5] / 60000f;
          path.AddArc(rect, startAngle, sweepAngle);
          pt1 = path.PathPoints[path.PathPoints.Length - 1];
          break;
        case 5:
          num = pathElements[index + 1] * 2.0;
          PointF[] points1 = new PointF[3]
          {
            pt1,
            new PointF(Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds)),
            new PointF(Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGeomentryPathXValue(pathWidth, pathElements[index + 4], bounds), Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGeomentryPathYValue(pathHeight, pathElements[index + 5], bounds))
          };
          path.AddBeziers(points1);
          pt1 = points1[2];
          break;
        case 6:
          num = pathElements[index + 1] * 2.0;
          PointF[] points2 = new PointF[4]
          {
            pt1,
            new PointF(Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGeomentryPathXValue(pathWidth, pathElements[index + 2], bounds), Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGeomentryPathYValue(pathHeight, pathElements[index + 3], bounds)),
            new PointF(Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGeomentryPathXValue(pathWidth, pathElements[index + 4], bounds), Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGeomentryPathYValue(pathHeight, pathElements[index + 5], bounds)),
            new PointF(Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGeomentryPathXValue(pathWidth, pathElements[index + 6], bounds), Syncfusion.Presentation.SlideToImageConverter.SlideToImageConverter.GetGeomentryPathYValue(pathHeight, pathElements[index + 7], bounds))
          };
          path.AddBeziers(points2);
          pt1 = points2[3];
          break;
      }
    }
  }
}
