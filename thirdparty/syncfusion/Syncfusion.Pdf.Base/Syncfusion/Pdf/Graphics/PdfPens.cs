// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfPens
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public sealed class PdfPens
{
  private static Dictionary<object, object> s_pens = new Dictionary<object, object>();

  public static PdfPen AliceBlue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen aliceBlue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.AliceBlue))
          aliceBlue = PdfPens.s_pens[(object) KnownColor.AliceBlue] as PdfPen;
        if (aliceBlue == null)
          aliceBlue = PdfPens.GetPen(KnownColor.AliceBlue);
        return aliceBlue;
      }
    }
  }

  public static PdfPen AntiqueWhite
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen antiqueWhite = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.AntiqueWhite))
          antiqueWhite = PdfPens.s_pens[(object) KnownColor.AntiqueWhite] as PdfPen;
        if (antiqueWhite == null)
          antiqueWhite = PdfPens.GetPen(KnownColor.AntiqueWhite);
        return antiqueWhite;
      }
    }
  }

  public static PdfPen Aqua
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen aqua = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Aqua))
          aqua = PdfPens.s_pens[(object) KnownColor.Aqua] as PdfPen;
        if (aqua == null)
          aqua = PdfPens.GetPen(KnownColor.Aqua);
        return aqua;
      }
    }
  }

  public static PdfPen Aquamarine
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen aquamarine = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Aquamarine))
          aquamarine = PdfPens.s_pens[(object) KnownColor.Aquamarine] as PdfPen;
        if (aquamarine == null)
          aquamarine = PdfPens.GetPen(KnownColor.Aquamarine);
        return aquamarine;
      }
    }
  }

  public static PdfPen Azure
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen azure = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Azure))
          azure = PdfPens.s_pens[(object) KnownColor.Azure] as PdfPen;
        if (azure == null)
          azure = PdfPens.GetPen(KnownColor.Azure);
        return azure;
      }
    }
  }

  public static PdfPen Beige
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen beige = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Beige))
          beige = PdfPens.s_pens[(object) KnownColor.Beige] as PdfPen;
        if (beige == null)
          beige = PdfPens.GetPen(KnownColor.Beige);
        return beige;
      }
    }
  }

  public static PdfPen Bisque
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen bisque = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Bisque))
          bisque = PdfPens.s_pens[(object) KnownColor.Bisque] as PdfPen;
        if (bisque == null)
          bisque = PdfPens.GetPen(KnownColor.Bisque);
        return bisque;
      }
    }
  }

  public static PdfPen Black
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen black = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Black))
          black = PdfPens.s_pens[(object) KnownColor.Black] as PdfPen;
        if (black == null)
          black = PdfPens.GetPen(KnownColor.Black);
        return black;
      }
    }
  }

  public static PdfPen BlanchedAlmond
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen blanchedAlmond = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.BlanchedAlmond))
          blanchedAlmond = PdfPens.s_pens[(object) KnownColor.BlanchedAlmond] as PdfPen;
        if (blanchedAlmond == null)
          blanchedAlmond = PdfPens.GetPen(KnownColor.BlanchedAlmond);
        return blanchedAlmond;
      }
    }
  }

  public static PdfPen Blue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen blue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Blue))
          blue = PdfPens.s_pens[(object) KnownColor.Blue] as PdfPen;
        if (blue == null)
          blue = PdfPens.GetPen(KnownColor.Blue);
        return blue;
      }
    }
  }

  public static PdfPen BlueViolet
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen blueViolet = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.BlueViolet))
          blueViolet = PdfPens.s_pens[(object) KnownColor.BlueViolet] as PdfPen;
        if (blueViolet == null)
          blueViolet = PdfPens.GetPen(KnownColor.BlueViolet);
        return blueViolet;
      }
    }
  }

  public static PdfPen Brown
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen brown = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Brown))
          brown = PdfPens.s_pens[(object) KnownColor.Brown] as PdfPen;
        if (brown == null)
          brown = PdfPens.GetPen(KnownColor.Brown);
        return brown;
      }
    }
  }

  public static PdfPen BurlyWood
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen burlyWood = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.BurlyWood))
          burlyWood = PdfPens.s_pens[(object) KnownColor.BurlyWood] as PdfPen;
        if (burlyWood == null)
          burlyWood = PdfPens.GetPen(KnownColor.BurlyWood);
        return burlyWood;
      }
    }
  }

  public static PdfPen CadetBlue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen cadetBlue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.CadetBlue))
          cadetBlue = PdfPens.s_pens[(object) KnownColor.CadetBlue] as PdfPen;
        if (cadetBlue == null)
          cadetBlue = PdfPens.GetPen(KnownColor.CadetBlue);
        return cadetBlue;
      }
    }
  }

  public static PdfPen Chartreuse
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen chartreuse = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Chartreuse))
          chartreuse = PdfPens.s_pens[(object) KnownColor.Chartreuse] as PdfPen;
        if (chartreuse == null)
          chartreuse = PdfPens.GetPen(KnownColor.Chartreuse);
        return chartreuse;
      }
    }
  }

  public static PdfPen Chocolate
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen chocolate = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Chocolate))
          chocolate = PdfPens.s_pens[(object) KnownColor.Chocolate] as PdfPen;
        if (chocolate == null)
          chocolate = PdfPens.GetPen(KnownColor.Chocolate);
        return chocolate;
      }
    }
  }

  public static PdfPen Coral
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen coral = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Coral))
          coral = PdfPens.s_pens[(object) KnownColor.Coral] as PdfPen;
        if (coral == null)
          coral = PdfPens.GetPen(KnownColor.Coral);
        return coral;
      }
    }
  }

  public static PdfPen CornflowerBlue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen cornflowerBlue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.CornflowerBlue))
          cornflowerBlue = PdfPens.s_pens[(object) KnownColor.CornflowerBlue] as PdfPen;
        if (cornflowerBlue == null)
          cornflowerBlue = PdfPens.GetPen(KnownColor.CornflowerBlue);
        return cornflowerBlue;
      }
    }
  }

  public static PdfPen Cornsilk
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen cornsilk = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Cornsilk))
          cornsilk = PdfPens.s_pens[(object) KnownColor.Cornsilk] as PdfPen;
        if (cornsilk == null)
          cornsilk = PdfPens.GetPen(KnownColor.Cornsilk);
        return cornsilk;
      }
    }
  }

  public static PdfPen Crimson
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen crimson = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Crimson))
          crimson = PdfPens.s_pens[(object) KnownColor.Crimson] as PdfPen;
        if (crimson == null)
          crimson = PdfPens.GetPen(KnownColor.Crimson);
        return crimson;
      }
    }
  }

  public static PdfPen Cyan
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen cyan = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Cyan))
          cyan = PdfPens.s_pens[(object) KnownColor.Cyan] as PdfPen;
        if (cyan == null)
          cyan = PdfPens.GetPen(KnownColor.Cyan);
        return cyan;
      }
    }
  }

  public static PdfPen DarkBlue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen darkBlue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DarkBlue))
          darkBlue = PdfPens.s_pens[(object) KnownColor.DarkBlue] as PdfPen;
        if (darkBlue == null)
          darkBlue = PdfPens.GetPen(KnownColor.DarkBlue);
        return darkBlue;
      }
    }
  }

  public static PdfPen DarkCyan
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen darkCyan = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DarkCyan))
          darkCyan = PdfPens.s_pens[(object) KnownColor.DarkCyan] as PdfPen;
        if (darkCyan == null)
          darkCyan = PdfPens.GetPen(KnownColor.DarkCyan);
        return darkCyan;
      }
    }
  }

  public static PdfPen DarkGoldenrod
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen darkGoldenrod = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DarkGoldenrod))
          darkGoldenrod = PdfPens.s_pens[(object) KnownColor.DarkGoldenrod] as PdfPen;
        if (darkGoldenrod == null)
          darkGoldenrod = PdfPens.GetPen(KnownColor.DarkGoldenrod);
        return darkGoldenrod;
      }
    }
  }

  public static PdfPen DarkGray
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen darkGray = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DarkGray))
          darkGray = PdfPens.s_pens[(object) KnownColor.DarkGray] as PdfPen;
        if (darkGray == null)
          darkGray = PdfPens.GetPen(KnownColor.DarkGray);
        return darkGray;
      }
    }
  }

  public static PdfPen DarkGreen
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen darkGreen = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DarkGreen))
          darkGreen = PdfPens.s_pens[(object) KnownColor.DarkGreen] as PdfPen;
        if (darkGreen == null)
          darkGreen = PdfPens.GetPen(KnownColor.DarkGreen);
        return darkGreen;
      }
    }
  }

  public static PdfPen DarkKhaki
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen darkKhaki = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DarkKhaki))
          darkKhaki = PdfPens.s_pens[(object) KnownColor.DarkKhaki] as PdfPen;
        if (darkKhaki == null)
          darkKhaki = PdfPens.GetPen(KnownColor.DarkKhaki);
        return darkKhaki;
      }
    }
  }

  public static PdfPen DarkMagenta
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen darkMagenta = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DarkMagenta))
          darkMagenta = PdfPens.s_pens[(object) KnownColor.DarkMagenta] as PdfPen;
        if (darkMagenta == null)
          darkMagenta = PdfPens.GetPen(KnownColor.DarkMagenta);
        return darkMagenta;
      }
    }
  }

  public static PdfPen DarkOliveGreen
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen darkOliveGreen = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DarkOliveGreen))
          darkOliveGreen = PdfPens.s_pens[(object) KnownColor.DarkOliveGreen] as PdfPen;
        if (darkOliveGreen == null)
          darkOliveGreen = PdfPens.GetPen(KnownColor.DarkOliveGreen);
        return darkOliveGreen;
      }
    }
  }

  public static PdfPen DarkOrange
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen darkOrange = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DarkOrange))
          darkOrange = PdfPens.s_pens[(object) KnownColor.DarkOrange] as PdfPen;
        if (darkOrange == null)
          darkOrange = PdfPens.GetPen(KnownColor.DarkOrange);
        return darkOrange;
      }
    }
  }

  public static PdfPen DarkOrchid
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen darkOrchid = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DarkOrchid))
          darkOrchid = PdfPens.s_pens[(object) KnownColor.DarkOrchid] as PdfPen;
        if (darkOrchid == null)
          darkOrchid = PdfPens.GetPen(KnownColor.DarkOrchid);
        return darkOrchid;
      }
    }
  }

  public static PdfPen DarkRed
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen darkRed = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DarkRed))
          darkRed = PdfPens.s_pens[(object) KnownColor.DarkRed] as PdfPen;
        if (darkRed == null)
          darkRed = PdfPens.GetPen(KnownColor.DarkRed);
        return darkRed;
      }
    }
  }

  public static PdfPen DarkSalmon
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen darkSalmon = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DarkSalmon))
          darkSalmon = PdfPens.s_pens[(object) KnownColor.DarkSalmon] as PdfPen;
        if (darkSalmon == null)
          darkSalmon = PdfPens.GetPen(KnownColor.DarkSalmon);
        return darkSalmon;
      }
    }
  }

  public static PdfPen DarkSeaGreen
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen darkSeaGreen = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DarkSeaGreen))
          darkSeaGreen = PdfPens.s_pens[(object) KnownColor.DarkSeaGreen] as PdfPen;
        if (darkSeaGreen == null)
          darkSeaGreen = PdfPens.GetPen(KnownColor.DarkSeaGreen);
        return darkSeaGreen;
      }
    }
  }

  public static PdfPen DarkSlateBlue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen darkSlateBlue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DarkSlateBlue))
          darkSlateBlue = PdfPens.s_pens[(object) KnownColor.DarkSlateBlue] as PdfPen;
        if (darkSlateBlue == null)
          darkSlateBlue = PdfPens.GetPen(KnownColor.DarkSlateBlue);
        return darkSlateBlue;
      }
    }
  }

  public static PdfPen DarkSlateGray
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen darkSlateGray = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DarkSlateGray))
          darkSlateGray = PdfPens.s_pens[(object) KnownColor.DarkSlateGray] as PdfPen;
        if (darkSlateGray == null)
          darkSlateGray = PdfPens.GetPen(KnownColor.DarkSlateGray);
        return darkSlateGray;
      }
    }
  }

  public static PdfPen DarkTurquoise
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen darkTurquoise = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DarkTurquoise))
          darkTurquoise = PdfPens.s_pens[(object) KnownColor.DarkTurquoise] as PdfPen;
        if (darkTurquoise == null)
          darkTurquoise = PdfPens.GetPen(KnownColor.DarkTurquoise);
        return darkTurquoise;
      }
    }
  }

  public static PdfPen DarkViolet
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen darkViolet = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DarkViolet))
          darkViolet = PdfPens.s_pens[(object) KnownColor.DarkViolet] as PdfPen;
        if (darkViolet == null)
          darkViolet = PdfPens.GetPen(KnownColor.DarkViolet);
        return darkViolet;
      }
    }
  }

  public static PdfPen DeepPink
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen deepPink = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DeepPink))
          deepPink = PdfPens.s_pens[(object) KnownColor.DeepPink] as PdfPen;
        if (deepPink == null)
          deepPink = PdfPens.GetPen(KnownColor.DeepPink);
        return deepPink;
      }
    }
  }

  public static PdfPen DeepSkyBlue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen deepSkyBlue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DeepSkyBlue))
          deepSkyBlue = PdfPens.s_pens[(object) KnownColor.DeepSkyBlue] as PdfPen;
        if (deepSkyBlue == null)
          deepSkyBlue = PdfPens.GetPen(KnownColor.DeepSkyBlue);
        return deepSkyBlue;
      }
    }
  }

  public static PdfPen DimGray
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen dimGray = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DimGray))
          dimGray = PdfPens.s_pens[(object) KnownColor.DimGray] as PdfPen;
        if (dimGray == null)
          dimGray = PdfPens.GetPen(KnownColor.DimGray);
        return dimGray;
      }
    }
  }

  public static PdfPen DodgerBlue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen dodgerBlue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.DodgerBlue))
          dodgerBlue = PdfPens.s_pens[(object) KnownColor.DodgerBlue] as PdfPen;
        if (dodgerBlue == null)
          dodgerBlue = PdfPens.GetPen(KnownColor.DodgerBlue);
        return dodgerBlue;
      }
    }
  }

  public static PdfPen Firebrick
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen firebrick = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Firebrick))
          firebrick = PdfPens.s_pens[(object) KnownColor.Firebrick] as PdfPen;
        if (firebrick == null)
          firebrick = PdfPens.GetPen(KnownColor.Firebrick);
        return firebrick;
      }
    }
  }

  public static PdfPen FloralWhite
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen floralWhite = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.FloralWhite))
          floralWhite = PdfPens.s_pens[(object) KnownColor.FloralWhite] as PdfPen;
        if (floralWhite == null)
          floralWhite = PdfPens.GetPen(KnownColor.FloralWhite);
        return floralWhite;
      }
    }
  }

  public static PdfPen ForestGreen
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen forestGreen = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.ForestGreen))
          forestGreen = PdfPens.s_pens[(object) KnownColor.ForestGreen] as PdfPen;
        if (forestGreen == null)
          forestGreen = PdfPens.GetPen(KnownColor.ForestGreen);
        return forestGreen;
      }
    }
  }

  public static PdfPen Fuchsia
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen fuchsia = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Fuchsia))
          fuchsia = PdfPens.s_pens[(object) KnownColor.Fuchsia] as PdfPen;
        if (fuchsia == null)
          fuchsia = PdfPens.GetPen(KnownColor.Fuchsia);
        return fuchsia;
      }
    }
  }

  public static PdfPen Gainsboro
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen gainsboro = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Gainsboro))
          gainsboro = PdfPens.s_pens[(object) KnownColor.Gainsboro] as PdfPen;
        if (gainsboro == null)
          gainsboro = PdfPens.GetPen(KnownColor.Gainsboro);
        return gainsboro;
      }
    }
  }

  public static PdfPen GhostWhite
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen ghostWhite = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.GhostWhite))
          ghostWhite = PdfPens.s_pens[(object) KnownColor.GhostWhite] as PdfPen;
        if (ghostWhite == null)
          ghostWhite = PdfPens.GetPen(KnownColor.GhostWhite);
        return ghostWhite;
      }
    }
  }

  public static PdfPen Gold
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen gold = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Gold))
          gold = PdfPens.s_pens[(object) KnownColor.Gold] as PdfPen;
        if (gold == null)
          gold = PdfPens.GetPen(KnownColor.Gold);
        return gold;
      }
    }
  }

  public static PdfPen Goldenrod
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen goldenrod = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Goldenrod))
          goldenrod = PdfPens.s_pens[(object) KnownColor.Goldenrod] as PdfPen;
        if (goldenrod == null)
          goldenrod = PdfPens.GetPen(KnownColor.Goldenrod);
        return goldenrod;
      }
    }
  }

  public static PdfPen Gray
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen gray = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Gray))
          gray = PdfPens.s_pens[(object) KnownColor.Gray] as PdfPen;
        if (gray == null)
          gray = PdfPens.GetPen(KnownColor.Gray);
        return gray;
      }
    }
  }

  public static PdfPen Green
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen green = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Green))
          green = PdfPens.s_pens[(object) KnownColor.Green] as PdfPen;
        if (green == null)
          green = PdfPens.GetPen(KnownColor.Green);
        return green;
      }
    }
  }

  public static PdfPen GreenYellow
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen greenYellow = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.GreenYellow))
          greenYellow = PdfPens.s_pens[(object) KnownColor.GreenYellow] as PdfPen;
        if (greenYellow == null)
          greenYellow = PdfPens.GetPen(KnownColor.GreenYellow);
        return greenYellow;
      }
    }
  }

  public static PdfPen Honeydew
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen honeydew = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Honeydew))
          honeydew = PdfPens.s_pens[(object) KnownColor.Honeydew] as PdfPen;
        if (honeydew == null)
          honeydew = PdfPens.GetPen(KnownColor.Honeydew);
        return honeydew;
      }
    }
  }

  public static PdfPen HotPink
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen hotPink = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.HotPink))
          hotPink = PdfPens.s_pens[(object) KnownColor.HotPink] as PdfPen;
        if (hotPink == null)
          hotPink = PdfPens.GetPen(KnownColor.HotPink);
        return hotPink;
      }
    }
  }

  public static PdfPen IndianRed
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen indianRed = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.IndianRed))
          indianRed = PdfPens.s_pens[(object) KnownColor.IndianRed] as PdfPen;
        if (indianRed == null)
          indianRed = PdfPens.GetPen(KnownColor.IndianRed);
        return indianRed;
      }
    }
  }

  public static PdfPen Indigo
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen indigo = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Indigo))
          indigo = PdfPens.s_pens[(object) KnownColor.Indigo] as PdfPen;
        if (indigo == null)
          indigo = PdfPens.GetPen(KnownColor.Indigo);
        return indigo;
      }
    }
  }

  public static PdfPen Ivory
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen ivory = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Ivory))
          ivory = PdfPens.s_pens[(object) KnownColor.Ivory] as PdfPen;
        if (ivory == null)
          ivory = PdfPens.GetPen(KnownColor.Ivory);
        return ivory;
      }
    }
  }

  public static PdfPen Khaki
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen khaki = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Khaki))
          khaki = PdfPens.s_pens[(object) KnownColor.Khaki] as PdfPen;
        if (khaki == null)
          khaki = PdfPens.GetPen(KnownColor.Khaki);
        return khaki;
      }
    }
  }

  public static PdfPen Lavender
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen lavender = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Lavender))
          lavender = PdfPens.s_pens[(object) KnownColor.Lavender] as PdfPen;
        if (lavender == null)
          lavender = PdfPens.GetPen(KnownColor.Lavender);
        return lavender;
      }
    }
  }

  public static PdfPen LavenderBlush
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen lavenderBlush = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.LavenderBlush))
          lavenderBlush = PdfPens.s_pens[(object) KnownColor.LavenderBlush] as PdfPen;
        if (lavenderBlush == null)
          lavenderBlush = PdfPens.GetPen(KnownColor.LavenderBlush);
        return lavenderBlush;
      }
    }
  }

  public static PdfPen LawnGreen
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen lawnGreen = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.LawnGreen))
          lawnGreen = PdfPens.s_pens[(object) KnownColor.LawnGreen] as PdfPen;
        if (lawnGreen == null)
          lawnGreen = PdfPens.GetPen(KnownColor.LawnGreen);
        return lawnGreen;
      }
    }
  }

  public static PdfPen LemonChiffon
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen lemonChiffon = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.LemonChiffon))
          lemonChiffon = PdfPens.s_pens[(object) KnownColor.LemonChiffon] as PdfPen;
        if (lemonChiffon == null)
          lemonChiffon = PdfPens.GetPen(KnownColor.LemonChiffon);
        return lemonChiffon;
      }
    }
  }

  public static PdfPen LightBlue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen lightBlue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.LightBlue))
          lightBlue = PdfPens.s_pens[(object) KnownColor.LightBlue] as PdfPen;
        if (lightBlue == null)
          lightBlue = PdfPens.GetPen(KnownColor.LightBlue);
        return lightBlue;
      }
    }
  }

  public static PdfPen LightCoral
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen lightCoral = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.LightCoral))
          lightCoral = PdfPens.s_pens[(object) KnownColor.LightCoral] as PdfPen;
        if (lightCoral == null)
          lightCoral = PdfPens.GetPen(KnownColor.LightCoral);
        return lightCoral;
      }
    }
  }

  public static PdfPen LightCyan
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen lightCyan = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.LightCyan))
          lightCyan = PdfPens.s_pens[(object) KnownColor.LightCyan] as PdfPen;
        if (lightCyan == null)
          lightCyan = PdfPens.GetPen(KnownColor.LightCyan);
        return lightCyan;
      }
    }
  }

  public static PdfPen LightGoldenrodYellow
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen lightGoldenrodYellow = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.LightGoldenrodYellow))
          lightGoldenrodYellow = PdfPens.s_pens[(object) KnownColor.LightGoldenrodYellow] as PdfPen;
        if (lightGoldenrodYellow == null)
          lightGoldenrodYellow = PdfPens.GetPen(KnownColor.LightGoldenrodYellow);
        return lightGoldenrodYellow;
      }
    }
  }

  public static PdfPen LightGray
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen lightGray = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.LightGray))
          lightGray = PdfPens.s_pens[(object) KnownColor.LightGray] as PdfPen;
        if (lightGray == null)
          lightGray = PdfPens.GetPen(KnownColor.LightGray);
        return lightGray;
      }
    }
  }

  public static PdfPen LightGreen
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen lightGreen = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.LightGreen))
          lightGreen = PdfPens.s_pens[(object) KnownColor.LightGreen] as PdfPen;
        if (lightGreen == null)
          lightGreen = PdfPens.GetPen(KnownColor.LightGreen);
        return lightGreen;
      }
    }
  }

  public static PdfPen LightPink
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen lightPink = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.LightPink))
          lightPink = PdfPens.s_pens[(object) KnownColor.LightPink] as PdfPen;
        if (lightPink == null)
          lightPink = PdfPens.GetPen(KnownColor.LightPink);
        return lightPink;
      }
    }
  }

  public static PdfPen LightSalmon
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen lightSalmon = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.LightSalmon))
          lightSalmon = PdfPens.s_pens[(object) KnownColor.LightSalmon] as PdfPen;
        if (lightSalmon == null)
          lightSalmon = PdfPens.GetPen(KnownColor.LightSalmon);
        return lightSalmon;
      }
    }
  }

  public static PdfPen LightSeaGreen
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen lightSeaGreen = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.LightSeaGreen))
          lightSeaGreen = PdfPens.s_pens[(object) KnownColor.LightSeaGreen] as PdfPen;
        if (lightSeaGreen == null)
          lightSeaGreen = PdfPens.GetPen(KnownColor.LightSeaGreen);
        return lightSeaGreen;
      }
    }
  }

  public static PdfPen LightSkyBlue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen lightSkyBlue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.LightSkyBlue))
          lightSkyBlue = PdfPens.s_pens[(object) KnownColor.LightSkyBlue] as PdfPen;
        if (lightSkyBlue == null)
          lightSkyBlue = PdfPens.GetPen(KnownColor.LightSkyBlue);
        return lightSkyBlue;
      }
    }
  }

  public static PdfPen LightSlateGray
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen lightSlateGray = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.LightSlateGray))
          lightSlateGray = PdfPens.s_pens[(object) KnownColor.LightSlateGray] as PdfPen;
        if (lightSlateGray == null)
          lightSlateGray = PdfPens.GetPen(KnownColor.LightSlateGray);
        return lightSlateGray;
      }
    }
  }

  public static PdfPen LightSteelBlue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen lightSteelBlue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.LightSteelBlue))
          lightSteelBlue = PdfPens.s_pens[(object) KnownColor.LightSteelBlue] as PdfPen;
        if (lightSteelBlue == null)
          lightSteelBlue = PdfPens.GetPen(KnownColor.LightSteelBlue);
        return lightSteelBlue;
      }
    }
  }

  public static PdfPen LightYellow
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen lightYellow = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.LightYellow))
          lightYellow = PdfPens.s_pens[(object) KnownColor.LightYellow] as PdfPen;
        if (lightYellow == null)
          lightYellow = PdfPens.GetPen(KnownColor.LightYellow);
        return lightYellow;
      }
    }
  }

  public static PdfPen Lime
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen lime = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Lime))
          lime = PdfPens.s_pens[(object) KnownColor.Lime] as PdfPen;
        if (lime == null)
          lime = PdfPens.GetPen(KnownColor.Lime);
        return lime;
      }
    }
  }

  public static PdfPen LimeGreen
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen limeGreen = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.LimeGreen))
          limeGreen = PdfPens.s_pens[(object) KnownColor.LimeGreen] as PdfPen;
        if (limeGreen == null)
          limeGreen = PdfPens.GetPen(KnownColor.LimeGreen);
        return limeGreen;
      }
    }
  }

  public static PdfPen Linen
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen linen = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Linen))
          linen = PdfPens.s_pens[(object) KnownColor.Linen] as PdfPen;
        if (linen == null)
          linen = PdfPens.GetPen(KnownColor.Linen);
        return linen;
      }
    }
  }

  public static PdfPen Magenta
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen magenta = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Magenta))
          magenta = PdfPens.s_pens[(object) KnownColor.Magenta] as PdfPen;
        if (magenta == null)
          magenta = PdfPens.GetPen(KnownColor.Magenta);
        return magenta;
      }
    }
  }

  public static PdfPen Maroon
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen maroon = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Maroon))
          maroon = PdfPens.s_pens[(object) KnownColor.Maroon] as PdfPen;
        if (maroon == null)
          maroon = PdfPens.GetPen(KnownColor.Maroon);
        return maroon;
      }
    }
  }

  public static PdfPen MediumAquamarine
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen mediumAquamarine = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.MediumAquamarine))
          mediumAquamarine = PdfPens.s_pens[(object) KnownColor.MediumAquamarine] as PdfPen;
        if (mediumAquamarine == null)
          mediumAquamarine = PdfPens.GetPen(KnownColor.MediumAquamarine);
        return mediumAquamarine;
      }
    }
  }

  public static PdfPen MediumBlue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen mediumBlue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.MediumBlue))
          mediumBlue = PdfPens.s_pens[(object) KnownColor.MediumBlue] as PdfPen;
        if (mediumBlue == null)
          mediumBlue = PdfPens.GetPen(KnownColor.MediumBlue);
        return mediumBlue;
      }
    }
  }

  public static PdfPen MediumOrchid
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen mediumOrchid = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.MediumOrchid))
          mediumOrchid = PdfPens.s_pens[(object) KnownColor.MediumOrchid] as PdfPen;
        if (mediumOrchid == null)
          mediumOrchid = PdfPens.GetPen(KnownColor.MediumOrchid);
        return mediumOrchid;
      }
    }
  }

  public static PdfPen MediumPurple
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen mediumPurple = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.MediumPurple))
          mediumPurple = PdfPens.s_pens[(object) KnownColor.MediumPurple] as PdfPen;
        if (mediumPurple == null)
          mediumPurple = PdfPens.GetPen(KnownColor.MediumPurple);
        return mediumPurple;
      }
    }
  }

  public static PdfPen MediumSeaGreen
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen mediumSeaGreen = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.MediumSeaGreen))
          mediumSeaGreen = PdfPens.s_pens[(object) KnownColor.MediumSeaGreen] as PdfPen;
        if (mediumSeaGreen == null)
          mediumSeaGreen = PdfPens.GetPen(KnownColor.MediumSeaGreen);
        return mediumSeaGreen;
      }
    }
  }

  public static PdfPen MediumSlateBlue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen mediumSlateBlue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.MediumSlateBlue))
          mediumSlateBlue = PdfPens.s_pens[(object) KnownColor.MediumSlateBlue] as PdfPen;
        if (mediumSlateBlue == null)
          mediumSlateBlue = PdfPens.GetPen(KnownColor.MediumSlateBlue);
        return mediumSlateBlue;
      }
    }
  }

  public static PdfPen MediumSpringGreen
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen mediumSpringGreen = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.MediumSpringGreen))
          mediumSpringGreen = PdfPens.s_pens[(object) KnownColor.MediumSpringGreen] as PdfPen;
        if (mediumSpringGreen == null)
          mediumSpringGreen = PdfPens.GetPen(KnownColor.MediumSpringGreen);
        return mediumSpringGreen;
      }
    }
  }

  public static PdfPen MediumTurquoise
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen mediumTurquoise = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.MediumTurquoise))
          mediumTurquoise = PdfPens.s_pens[(object) KnownColor.MediumTurquoise] as PdfPen;
        if (mediumTurquoise == null)
          mediumTurquoise = PdfPens.GetPen(KnownColor.MediumTurquoise);
        return mediumTurquoise;
      }
    }
  }

  public static PdfPen MediumVioletRed
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen mediumVioletRed = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.MediumVioletRed))
          mediumVioletRed = PdfPens.s_pens[(object) KnownColor.MediumVioletRed] as PdfPen;
        if (mediumVioletRed == null)
          mediumVioletRed = PdfPens.GetPen(KnownColor.MediumVioletRed);
        return mediumVioletRed;
      }
    }
  }

  public static PdfPen MidnightBlue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen midnightBlue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.MidnightBlue))
          midnightBlue = PdfPens.s_pens[(object) KnownColor.MidnightBlue] as PdfPen;
        if (midnightBlue == null)
          midnightBlue = PdfPens.GetPen(KnownColor.MidnightBlue);
        return midnightBlue;
      }
    }
  }

  public static PdfPen MintCream
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen mintCream = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.MintCream))
          mintCream = PdfPens.s_pens[(object) KnownColor.MintCream] as PdfPen;
        if (mintCream == null)
          mintCream = PdfPens.GetPen(KnownColor.MintCream);
        return mintCream;
      }
    }
  }

  public static PdfPen MistyRose
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen mistyRose = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.MistyRose))
          mistyRose = PdfPens.s_pens[(object) KnownColor.MistyRose] as PdfPen;
        if (mistyRose == null)
          mistyRose = PdfPens.GetPen(KnownColor.MistyRose);
        return mistyRose;
      }
    }
  }

  public static PdfPen Moccasin
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen moccasin = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Moccasin))
          moccasin = PdfPens.s_pens[(object) KnownColor.Moccasin] as PdfPen;
        if (moccasin == null)
          moccasin = PdfPens.GetPen(KnownColor.Moccasin);
        return moccasin;
      }
    }
  }

  public static PdfPen NavajoWhite
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen navajoWhite = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.NavajoWhite))
          navajoWhite = PdfPens.s_pens[(object) KnownColor.NavajoWhite] as PdfPen;
        if (navajoWhite == null)
          navajoWhite = PdfPens.GetPen(KnownColor.NavajoWhite);
        return navajoWhite;
      }
    }
  }

  public static PdfPen Navy
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen navy = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Navy))
          navy = PdfPens.s_pens[(object) KnownColor.Navy] as PdfPen;
        if (navy == null)
          navy = PdfPens.GetPen(KnownColor.Navy);
        return navy;
      }
    }
  }

  public static PdfPen OldLace
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen oldLace = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.OldLace))
          oldLace = PdfPens.s_pens[(object) KnownColor.OldLace] as PdfPen;
        if (oldLace == null)
          oldLace = PdfPens.GetPen(KnownColor.OldLace);
        return oldLace;
      }
    }
  }

  public static PdfPen Olive
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen olive = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Olive))
          olive = PdfPens.s_pens[(object) KnownColor.Olive] as PdfPen;
        if (olive == null)
          olive = PdfPens.GetPen(KnownColor.Olive);
        return olive;
      }
    }
  }

  public static PdfPen OliveDrab
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen oliveDrab = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.OliveDrab))
          oliveDrab = PdfPens.s_pens[(object) KnownColor.OliveDrab] as PdfPen;
        if (oliveDrab == null)
          oliveDrab = PdfPens.GetPen(KnownColor.OliveDrab);
        return oliveDrab;
      }
    }
  }

  public static PdfPen Orange
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen orange = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Orange))
          orange = PdfPens.s_pens[(object) KnownColor.Orange] as PdfPen;
        if (orange == null)
          orange = PdfPens.GetPen(KnownColor.Orange);
        return orange;
      }
    }
  }

  public static PdfPen OrangeRed
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen orangeRed = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.OrangeRed))
          orangeRed = PdfPens.s_pens[(object) KnownColor.OrangeRed] as PdfPen;
        if (orangeRed == null)
          orangeRed = PdfPens.GetPen(KnownColor.OrangeRed);
        return orangeRed;
      }
    }
  }

  public static PdfPen Orchid
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen orchid = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Orchid))
          orchid = PdfPens.s_pens[(object) KnownColor.Orchid] as PdfPen;
        if (orchid == null)
          orchid = PdfPens.GetPen(KnownColor.Orchid);
        return orchid;
      }
    }
  }

  public static PdfPen PaleGoldenrod
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen paleGoldenrod = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.PaleGoldenrod))
          paleGoldenrod = PdfPens.s_pens[(object) KnownColor.PaleGoldenrod] as PdfPen;
        if (paleGoldenrod == null)
          paleGoldenrod = PdfPens.GetPen(KnownColor.PaleGoldenrod);
        return paleGoldenrod;
      }
    }
  }

  public static PdfPen PaleGreen
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen paleGreen = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.PaleGreen))
          paleGreen = PdfPens.s_pens[(object) KnownColor.PaleGreen] as PdfPen;
        if (paleGreen == null)
          paleGreen = PdfPens.GetPen(KnownColor.PaleGreen);
        return paleGreen;
      }
    }
  }

  public static PdfPen PaleTurquoise
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen paleTurquoise = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.PaleTurquoise))
          paleTurquoise = PdfPens.s_pens[(object) KnownColor.PaleTurquoise] as PdfPen;
        if (paleTurquoise == null)
          paleTurquoise = PdfPens.GetPen(KnownColor.PaleTurquoise);
        return paleTurquoise;
      }
    }
  }

  public static PdfPen PaleVioletRed
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen paleVioletRed = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.PaleVioletRed))
          paleVioletRed = PdfPens.s_pens[(object) KnownColor.PaleVioletRed] as PdfPen;
        if (paleVioletRed == null)
          paleVioletRed = PdfPens.GetPen(KnownColor.PaleVioletRed);
        return paleVioletRed;
      }
    }
  }

  public static PdfPen PapayaWhip
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen papayaWhip = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.PapayaWhip))
          papayaWhip = PdfPens.s_pens[(object) KnownColor.PapayaWhip] as PdfPen;
        if (papayaWhip == null)
          papayaWhip = PdfPens.GetPen(KnownColor.PapayaWhip);
        return papayaWhip;
      }
    }
  }

  public static PdfPen PeachPuff
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen peachPuff = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.PeachPuff))
          peachPuff = PdfPens.s_pens[(object) KnownColor.PeachPuff] as PdfPen;
        if (peachPuff == null)
          peachPuff = PdfPens.GetPen(KnownColor.PeachPuff);
        return peachPuff;
      }
    }
  }

  public static PdfPen Peru
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen peru = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Peru))
          peru = PdfPens.s_pens[(object) KnownColor.Peru] as PdfPen;
        if (peru == null)
          peru = PdfPens.GetPen(KnownColor.Peru);
        return peru;
      }
    }
  }

  public static PdfPen Pink
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen pink = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Pink))
          pink = PdfPens.s_pens[(object) KnownColor.Pink] as PdfPen;
        if (pink == null)
          pink = PdfPens.GetPen(KnownColor.Pink);
        return pink;
      }
    }
  }

  public static PdfPen Plum
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen plum = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Plum))
          plum = PdfPens.s_pens[(object) KnownColor.Plum] as PdfPen;
        if (plum == null)
          plum = PdfPens.GetPen(KnownColor.Plum);
        return plum;
      }
    }
  }

  public static PdfPen PowderBlue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen powderBlue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.PowderBlue))
          powderBlue = PdfPens.s_pens[(object) KnownColor.PowderBlue] as PdfPen;
        if (powderBlue == null)
          powderBlue = PdfPens.GetPen(KnownColor.PowderBlue);
        return powderBlue;
      }
    }
  }

  public static PdfPen Purple
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen purple = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Purple))
          purple = PdfPens.s_pens[(object) KnownColor.Purple] as PdfPen;
        if (purple == null)
          purple = PdfPens.GetPen(KnownColor.Purple);
        return purple;
      }
    }
  }

  public static PdfPen Red
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen red = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Red))
          red = PdfPens.s_pens[(object) KnownColor.Red] as PdfPen;
        if (red == null)
          red = PdfPens.GetPen(KnownColor.Red);
        return red;
      }
    }
  }

  public static PdfPen RosyBrown
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen rosyBrown = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.RosyBrown))
          rosyBrown = PdfPens.s_pens[(object) KnownColor.RosyBrown] as PdfPen;
        if (rosyBrown == null)
          rosyBrown = PdfPens.GetPen(KnownColor.RosyBrown);
        return rosyBrown;
      }
    }
  }

  public static PdfPen RoyalBlue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen royalBlue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.RoyalBlue))
          royalBlue = PdfPens.s_pens[(object) KnownColor.RoyalBlue] as PdfPen;
        if (royalBlue == null)
          royalBlue = PdfPens.GetPen(KnownColor.RoyalBlue);
        return royalBlue;
      }
    }
  }

  public static PdfPen SaddleBrown
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen saddleBrown = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.SaddleBrown))
          saddleBrown = PdfPens.s_pens[(object) KnownColor.SaddleBrown] as PdfPen;
        if (saddleBrown == null)
          saddleBrown = PdfPens.GetPen(KnownColor.SaddleBrown);
        return saddleBrown;
      }
    }
  }

  public static PdfPen Salmon
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen salmon = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Salmon))
          salmon = PdfPens.s_pens[(object) KnownColor.Salmon] as PdfPen;
        if (salmon == null)
          salmon = PdfPens.GetPen(KnownColor.Salmon);
        return salmon;
      }
    }
  }

  public static PdfPen SandyBrown
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen sandyBrown = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.SandyBrown))
          sandyBrown = PdfPens.s_pens[(object) KnownColor.SandyBrown] as PdfPen;
        if (sandyBrown == null)
          sandyBrown = PdfPens.GetPen(KnownColor.SandyBrown);
        return sandyBrown;
      }
    }
  }

  public static PdfPen SeaGreen
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen seaGreen = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.SeaGreen))
          seaGreen = PdfPens.s_pens[(object) KnownColor.SeaGreen] as PdfPen;
        if (seaGreen == null)
          seaGreen = PdfPens.GetPen(KnownColor.SeaGreen);
        return seaGreen;
      }
    }
  }

  public static PdfPen SeaShell
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen seaShell = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.SeaShell))
          seaShell = PdfPens.s_pens[(object) KnownColor.SeaShell] as PdfPen;
        if (seaShell == null)
          seaShell = PdfPens.GetPen(KnownColor.SeaShell);
        return seaShell;
      }
    }
  }

  public static PdfPen Sienna
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen sienna = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Sienna))
          sienna = PdfPens.s_pens[(object) KnownColor.Sienna] as PdfPen;
        if (sienna == null)
          sienna = PdfPens.GetPen(KnownColor.Sienna);
        return sienna;
      }
    }
  }

  public static PdfPen Silver
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen silver = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Silver))
          silver = PdfPens.s_pens[(object) KnownColor.Silver] as PdfPen;
        if (silver == null)
          silver = PdfPens.GetPen(KnownColor.Silver);
        return silver;
      }
    }
  }

  public static PdfPen SkyBlue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen skyBlue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.SkyBlue))
          skyBlue = PdfPens.s_pens[(object) KnownColor.SkyBlue] as PdfPen;
        if (skyBlue == null)
          skyBlue = PdfPens.GetPen(KnownColor.SkyBlue);
        return skyBlue;
      }
    }
  }

  public static PdfPen SlateBlue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen slateBlue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.SlateBlue))
          slateBlue = PdfPens.s_pens[(object) KnownColor.SlateBlue] as PdfPen;
        if (slateBlue == null)
          slateBlue = PdfPens.GetPen(KnownColor.SlateBlue);
        return slateBlue;
      }
    }
  }

  public static PdfPen SlateGray
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen slateGray = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.SlateGray))
          slateGray = PdfPens.s_pens[(object) KnownColor.SlateGray] as PdfPen;
        if (slateGray == null)
          slateGray = PdfPens.GetPen(KnownColor.SlateGray);
        return slateGray;
      }
    }
  }

  public static PdfPen Snow
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen snow = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Snow))
          snow = PdfPens.s_pens[(object) KnownColor.Snow] as PdfPen;
        if (snow == null)
          snow = PdfPens.GetPen(KnownColor.Snow);
        return snow;
      }
    }
  }

  public static PdfPen SpringGreen
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen springGreen = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.SpringGreen))
          springGreen = PdfPens.s_pens[(object) KnownColor.SpringGreen] as PdfPen;
        if (springGreen == null)
          springGreen = PdfPens.GetPen(KnownColor.SpringGreen);
        return springGreen;
      }
    }
  }

  public static PdfPen SteelBlue
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen steelBlue = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.SteelBlue))
          steelBlue = PdfPens.s_pens[(object) KnownColor.SteelBlue] as PdfPen;
        if (steelBlue == null)
          steelBlue = PdfPens.GetPen(KnownColor.SteelBlue);
        return steelBlue;
      }
    }
  }

  public static PdfPen Tan
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen tan = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Tan))
          tan = PdfPens.s_pens[(object) KnownColor.Tan] as PdfPen;
        if (tan == null)
          tan = PdfPens.GetPen(KnownColor.Tan);
        return tan;
      }
    }
  }

  public static PdfPen Teal
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen teal = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Teal))
          teal = PdfPens.s_pens[(object) KnownColor.Teal] as PdfPen;
        if (teal == null)
          teal = PdfPens.GetPen(KnownColor.Teal);
        return teal;
      }
    }
  }

  public static PdfPen Thistle
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen thistle = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Thistle))
          thistle = PdfPens.s_pens[(object) KnownColor.Thistle] as PdfPen;
        if (thistle == null)
          thistle = PdfPens.GetPen(KnownColor.Thistle);
        return thistle;
      }
    }
  }

  public static PdfPen Tomato
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen tomato = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Tomato))
          tomato = PdfPens.s_pens[(object) KnownColor.Tomato] as PdfPen;
        if (tomato == null)
          tomato = PdfPens.GetPen(KnownColor.Tomato);
        return tomato;
      }
    }
  }

  public static PdfPen Transparent
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen transparent = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Transparent))
          transparent = PdfPens.s_pens[(object) KnownColor.Transparent] as PdfPen;
        if (transparent == null)
          transparent = PdfPens.GetPen(KnownColor.Transparent);
        return transparent;
      }
    }
  }

  public static PdfPen Turquoise
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen turquoise = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Turquoise))
          turquoise = PdfPens.s_pens[(object) KnownColor.Turquoise] as PdfPen;
        if (turquoise == null)
          turquoise = PdfPens.GetPen(KnownColor.Turquoise);
        return turquoise;
      }
    }
  }

  public static PdfPen Violet
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen violet = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Violet))
          violet = PdfPens.s_pens[(object) KnownColor.Violet] as PdfPen;
        if (violet == null)
          violet = PdfPens.GetPen(KnownColor.Violet);
        return violet;
      }
    }
  }

  public static PdfPen Wheat
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen wheat = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Wheat))
          wheat = PdfPens.s_pens[(object) KnownColor.Wheat] as PdfPen;
        if (wheat == null)
          wheat = PdfPens.GetPen(KnownColor.Wheat);
        return wheat;
      }
    }
  }

  public static PdfPen White
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen white = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.White))
          white = PdfPens.s_pens[(object) KnownColor.White] as PdfPen;
        if (white == null)
          white = PdfPens.GetPen(KnownColor.White);
        return white;
      }
    }
  }

  public static PdfPen WhiteSmoke
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen whiteSmoke = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.WhiteSmoke))
          whiteSmoke = PdfPens.s_pens[(object) KnownColor.WhiteSmoke] as PdfPen;
        if (whiteSmoke == null)
          whiteSmoke = PdfPens.GetPen(KnownColor.WhiteSmoke);
        return whiteSmoke;
      }
    }
  }

  public static PdfPen Yellow
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen yellow = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.Yellow))
          yellow = PdfPens.s_pens[(object) KnownColor.Yellow] as PdfPen;
        if (yellow == null)
          yellow = PdfPens.GetPen(KnownColor.Yellow);
        return yellow;
      }
    }
  }

  public static PdfPen YellowGreen
  {
    get
    {
      lock (PdfPens.s_pens)
      {
        PdfPen yellowGreen = (PdfPen) null;
        if (PdfPens.s_pens.ContainsKey((object) KnownColor.YellowGreen))
          yellowGreen = PdfPens.s_pens[(object) KnownColor.YellowGreen] as PdfPen;
        if (yellowGreen == null)
          yellowGreen = PdfPens.GetPen(KnownColor.YellowGreen);
        return yellowGreen;
      }
    }
  }

  private static PdfPen GetPen(KnownColor colorName)
  {
    Color color = Color.FromKnownColor(colorName);
    PdfColor pdfColor = new PdfColor(color);
    PdfPen pen = new PdfPen((PdfColor) color, true);
    PdfPens.s_pens[(object) colorName] = (object) pen;
    return pen;
  }

  private PdfPens()
  {
  }
}
