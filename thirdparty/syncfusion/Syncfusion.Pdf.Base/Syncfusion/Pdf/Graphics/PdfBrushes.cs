// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.PdfBrushes
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics;

public sealed class PdfBrushes
{
  private static Dictionary<object, object> s_brushes = new Dictionary<object, object>();

  private PdfBrushes()
  {
  }

  public static PdfBrush AliceBlue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush aliceBlue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.AliceBlue))
          aliceBlue = PdfBrushes.s_brushes[(object) KnownColor.AliceBlue] as PdfBrush;
        if (aliceBlue == null)
          aliceBlue = PdfBrushes.GetBrush(KnownColor.AliceBlue);
        return aliceBlue;
      }
    }
  }

  public static PdfBrush AntiqueWhite
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush antiqueWhite = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.AntiqueWhite))
          antiqueWhite = PdfBrushes.s_brushes[(object) KnownColor.AntiqueWhite] as PdfBrush;
        if (antiqueWhite == null)
          antiqueWhite = PdfBrushes.GetBrush(KnownColor.AntiqueWhite);
        return antiqueWhite;
      }
    }
  }

  public static PdfBrush Aqua
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush aqua = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Aqua))
          aqua = PdfBrushes.s_brushes[(object) KnownColor.Aqua] as PdfBrush;
        if (aqua == null)
          aqua = PdfBrushes.GetBrush(KnownColor.Aqua);
        return aqua;
      }
    }
  }

  public static PdfBrush Aquamarine
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush aquamarine = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Aquamarine))
          aquamarine = PdfBrushes.s_brushes[(object) KnownColor.Aquamarine] as PdfBrush;
        if (aquamarine == null)
          aquamarine = PdfBrushes.GetBrush(KnownColor.Aquamarine);
        return aquamarine;
      }
    }
  }

  public static PdfBrush Azure
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush azure = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Azure))
          azure = PdfBrushes.s_brushes[(object) KnownColor.Azure] as PdfBrush;
        if (azure == null)
          azure = PdfBrushes.GetBrush(KnownColor.Azure);
        return azure;
      }
    }
  }

  public static PdfBrush Beige
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush beige = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Beige))
          beige = PdfBrushes.s_brushes[(object) KnownColor.Beige] as PdfBrush;
        if (beige == null)
          beige = PdfBrushes.GetBrush(KnownColor.Beige);
        return beige;
      }
    }
  }

  public static PdfBrush Bisque
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush bisque = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Bisque))
          bisque = PdfBrushes.s_brushes[(object) KnownColor.Bisque] as PdfBrush;
        if (bisque == null)
          bisque = PdfBrushes.GetBrush(KnownColor.Bisque);
        return bisque;
      }
    }
  }

  public static PdfBrush Black
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush black = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Black))
          black = PdfBrushes.s_brushes[(object) KnownColor.Black] as PdfBrush;
        if (black == null)
          black = PdfBrushes.GetBrush(KnownColor.Black);
        return black;
      }
    }
  }

  public static PdfBrush BlanchedAlmond
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush blanchedAlmond = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.BlanchedAlmond))
          blanchedAlmond = PdfBrushes.s_brushes[(object) KnownColor.BlanchedAlmond] as PdfBrush;
        if (blanchedAlmond == null)
          blanchedAlmond = PdfBrushes.GetBrush(KnownColor.BlanchedAlmond);
        return blanchedAlmond;
      }
    }
  }

  public static PdfBrush Blue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush blue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Blue))
          blue = PdfBrushes.s_brushes[(object) KnownColor.Blue] as PdfBrush;
        if (blue == null)
          blue = PdfBrushes.GetBrush(KnownColor.Blue);
        return blue;
      }
    }
  }

  public static PdfBrush BlueViolet
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush blueViolet = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.BlueViolet))
          blueViolet = PdfBrushes.s_brushes[(object) KnownColor.BlueViolet] as PdfBrush;
        if (blueViolet == null)
          blueViolet = PdfBrushes.GetBrush(KnownColor.BlueViolet);
        return blueViolet;
      }
    }
  }

  public static PdfBrush Brown
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush brown = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Brown))
          brown = PdfBrushes.s_brushes[(object) KnownColor.Brown] as PdfBrush;
        if (brown == null)
          brown = PdfBrushes.GetBrush(KnownColor.Brown);
        return brown;
      }
    }
  }

  public static PdfBrush BurlyWood
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush burlyWood = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.BurlyWood))
          burlyWood = PdfBrushes.s_brushes[(object) KnownColor.BurlyWood] as PdfBrush;
        if (burlyWood == null)
          burlyWood = PdfBrushes.GetBrush(KnownColor.BurlyWood);
        return burlyWood;
      }
    }
  }

  public static PdfBrush CadetBlue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush cadetBlue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.CadetBlue))
          cadetBlue = PdfBrushes.s_brushes[(object) KnownColor.CadetBlue] as PdfBrush;
        if (cadetBlue == null)
          cadetBlue = PdfBrushes.GetBrush(KnownColor.CadetBlue);
        return cadetBlue;
      }
    }
  }

  public static PdfBrush Chartreuse
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush chartreuse = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Chartreuse))
          chartreuse = PdfBrushes.s_brushes[(object) KnownColor.Chartreuse] as PdfBrush;
        if (chartreuse == null)
          chartreuse = PdfBrushes.GetBrush(KnownColor.Chartreuse);
        return chartreuse;
      }
    }
  }

  public static PdfBrush Chocolate
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush chocolate = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Chocolate))
          chocolate = PdfBrushes.s_brushes[(object) KnownColor.Chocolate] as PdfBrush;
        if (chocolate == null)
          chocolate = PdfBrushes.GetBrush(KnownColor.Chocolate);
        return chocolate;
      }
    }
  }

  public static PdfBrush Coral
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush coral = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Coral))
          coral = PdfBrushes.s_brushes[(object) KnownColor.Coral] as PdfBrush;
        if (coral == null)
          coral = PdfBrushes.GetBrush(KnownColor.Coral);
        return coral;
      }
    }
  }

  public static PdfBrush CornflowerBlue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush cornflowerBlue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.CornflowerBlue))
          cornflowerBlue = PdfBrushes.s_brushes[(object) KnownColor.CornflowerBlue] as PdfBrush;
        if (cornflowerBlue == null)
          cornflowerBlue = PdfBrushes.GetBrush(KnownColor.CornflowerBlue);
        return cornflowerBlue;
      }
    }
  }

  public static PdfBrush Cornsilk
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush cornsilk = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Cornsilk))
          cornsilk = PdfBrushes.s_brushes[(object) KnownColor.Cornsilk] as PdfBrush;
        if (cornsilk == null)
          cornsilk = PdfBrushes.GetBrush(KnownColor.Cornsilk);
        return cornsilk;
      }
    }
  }

  public static PdfBrush Crimson
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush crimson = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Crimson))
          crimson = PdfBrushes.s_brushes[(object) KnownColor.Crimson] as PdfBrush;
        if (crimson == null)
          crimson = PdfBrushes.GetBrush(KnownColor.Crimson);
        return crimson;
      }
    }
  }

  public static PdfBrush Cyan
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush cyan = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Cyan))
          cyan = PdfBrushes.s_brushes[(object) KnownColor.Cyan] as PdfBrush;
        if (cyan == null)
          cyan = PdfBrushes.GetBrush(KnownColor.Cyan);
        return cyan;
      }
    }
  }

  public static PdfBrush DarkBlue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush darkBlue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DarkBlue))
          darkBlue = PdfBrushes.s_brushes[(object) KnownColor.DarkBlue] as PdfBrush;
        if (darkBlue == null)
          darkBlue = PdfBrushes.GetBrush(KnownColor.DarkBlue);
        return darkBlue;
      }
    }
  }

  public static PdfBrush DarkCyan
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush darkCyan = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DarkCyan))
          darkCyan = PdfBrushes.s_brushes[(object) KnownColor.DarkCyan] as PdfBrush;
        if (darkCyan == null)
          darkCyan = PdfBrushes.GetBrush(KnownColor.DarkCyan);
        return darkCyan;
      }
    }
  }

  public static PdfBrush DarkGoldenrod
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush darkGoldenrod = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DarkGoldenrod))
          darkGoldenrod = PdfBrushes.s_brushes[(object) KnownColor.DarkGoldenrod] as PdfBrush;
        if (darkGoldenrod == null)
          darkGoldenrod = PdfBrushes.GetBrush(KnownColor.DarkGoldenrod);
        return darkGoldenrod;
      }
    }
  }

  public static PdfBrush DarkGray
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush darkGray = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DarkGray))
          darkGray = PdfBrushes.s_brushes[(object) KnownColor.DarkGray] as PdfBrush;
        if (darkGray == null)
          darkGray = PdfBrushes.GetBrush(KnownColor.DarkGray);
        return darkGray;
      }
    }
  }

  public static PdfBrush DarkGreen
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush darkGreen = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DarkGreen))
          darkGreen = PdfBrushes.s_brushes[(object) KnownColor.DarkGreen] as PdfBrush;
        if (darkGreen == null)
          darkGreen = PdfBrushes.GetBrush(KnownColor.DarkGreen);
        return darkGreen;
      }
    }
  }

  public static PdfBrush DarkKhaki
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush darkKhaki = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DarkKhaki))
          darkKhaki = PdfBrushes.s_brushes[(object) KnownColor.DarkKhaki] as PdfBrush;
        if (darkKhaki == null)
          darkKhaki = PdfBrushes.GetBrush(KnownColor.DarkKhaki);
        return darkKhaki;
      }
    }
  }

  public static PdfBrush DarkMagenta
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush darkMagenta = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DarkMagenta))
          darkMagenta = PdfBrushes.s_brushes[(object) KnownColor.DarkMagenta] as PdfBrush;
        if (darkMagenta == null)
          darkMagenta = PdfBrushes.GetBrush(KnownColor.DarkMagenta);
        return darkMagenta;
      }
    }
  }

  public static PdfBrush DarkOliveGreen
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush darkOliveGreen = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DarkOliveGreen))
          darkOliveGreen = PdfBrushes.s_brushes[(object) KnownColor.DarkOliveGreen] as PdfBrush;
        if (darkOliveGreen == null)
          darkOliveGreen = PdfBrushes.GetBrush(KnownColor.DarkOliveGreen);
        return darkOliveGreen;
      }
    }
  }

  public static PdfBrush DarkOrange
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush darkOrange = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DarkOrange))
          darkOrange = PdfBrushes.s_brushes[(object) KnownColor.DarkOrange] as PdfBrush;
        if (darkOrange == null)
          darkOrange = PdfBrushes.GetBrush(KnownColor.DarkOrange);
        return darkOrange;
      }
    }
  }

  public static PdfBrush DarkOrchid
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush darkOrchid = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DarkOrchid))
          darkOrchid = PdfBrushes.s_brushes[(object) KnownColor.DarkOrchid] as PdfBrush;
        if (darkOrchid == null)
          darkOrchid = PdfBrushes.GetBrush(KnownColor.DarkOrchid);
        return darkOrchid;
      }
    }
  }

  public static PdfBrush DarkRed
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush darkRed = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DarkRed))
          darkRed = PdfBrushes.s_brushes[(object) KnownColor.DarkRed] as PdfBrush;
        if (darkRed == null)
          darkRed = PdfBrushes.GetBrush(KnownColor.DarkRed);
        return darkRed;
      }
    }
  }

  public static PdfBrush DarkSalmon
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush darkSalmon = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DarkSalmon))
          darkSalmon = PdfBrushes.s_brushes[(object) KnownColor.DarkSalmon] as PdfBrush;
        if (darkSalmon == null)
          darkSalmon = PdfBrushes.GetBrush(KnownColor.DarkSalmon);
        return darkSalmon;
      }
    }
  }

  public static PdfBrush DarkSeaGreen
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush darkSeaGreen = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DarkSeaGreen))
          darkSeaGreen = PdfBrushes.s_brushes[(object) KnownColor.DarkSeaGreen] as PdfBrush;
        if (darkSeaGreen == null)
          darkSeaGreen = PdfBrushes.GetBrush(KnownColor.DarkSeaGreen);
        return darkSeaGreen;
      }
    }
  }

  public static PdfBrush DarkSlateBlue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush darkSlateBlue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DarkSlateBlue))
          darkSlateBlue = PdfBrushes.s_brushes[(object) KnownColor.DarkSlateBlue] as PdfBrush;
        if (darkSlateBlue == null)
          darkSlateBlue = PdfBrushes.GetBrush(KnownColor.DarkSlateBlue);
        return darkSlateBlue;
      }
    }
  }

  public static PdfBrush DarkSlateGray
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush darkSlateGray = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DarkSlateGray))
          darkSlateGray = PdfBrushes.s_brushes[(object) KnownColor.DarkSlateGray] as PdfBrush;
        if (darkSlateGray == null)
          darkSlateGray = PdfBrushes.GetBrush(KnownColor.DarkSlateGray);
        return darkSlateGray;
      }
    }
  }

  public static PdfBrush DarkTurquoise
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush darkTurquoise = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DarkTurquoise))
          darkTurquoise = PdfBrushes.s_brushes[(object) KnownColor.DarkTurquoise] as PdfBrush;
        if (darkTurquoise == null)
          darkTurquoise = PdfBrushes.GetBrush(KnownColor.DarkTurquoise);
        return darkTurquoise;
      }
    }
  }

  public static PdfBrush DarkViolet
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush darkViolet = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DarkViolet))
          darkViolet = PdfBrushes.s_brushes[(object) KnownColor.DarkViolet] as PdfBrush;
        if (darkViolet == null)
          darkViolet = PdfBrushes.GetBrush(KnownColor.DarkViolet);
        return darkViolet;
      }
    }
  }

  public static PdfBrush DeepPink
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush deepPink = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DeepPink))
          deepPink = PdfBrushes.s_brushes[(object) KnownColor.DeepPink] as PdfBrush;
        if (deepPink == null)
          deepPink = PdfBrushes.GetBrush(KnownColor.DeepPink);
        return deepPink;
      }
    }
  }

  public static PdfBrush DeepSkyBlue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush deepSkyBlue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DeepSkyBlue))
          deepSkyBlue = PdfBrushes.s_brushes[(object) KnownColor.DeepSkyBlue] as PdfBrush;
        if (deepSkyBlue == null)
          deepSkyBlue = PdfBrushes.GetBrush(KnownColor.DeepSkyBlue);
        return deepSkyBlue;
      }
    }
  }

  public static PdfBrush DimGray
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush dimGray = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DimGray))
          dimGray = PdfBrushes.s_brushes[(object) KnownColor.DimGray] as PdfBrush;
        if (dimGray == null)
          dimGray = PdfBrushes.GetBrush(KnownColor.DimGray);
        return dimGray;
      }
    }
  }

  public static PdfBrush DodgerBlue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush dodgerBlue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.DodgerBlue))
          dodgerBlue = PdfBrushes.s_brushes[(object) KnownColor.DodgerBlue] as PdfBrush;
        if (dodgerBlue == null)
          dodgerBlue = PdfBrushes.GetBrush(KnownColor.DodgerBlue);
        return dodgerBlue;
      }
    }
  }

  public static PdfBrush Firebrick
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush firebrick = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Firebrick))
          firebrick = PdfBrushes.s_brushes[(object) KnownColor.Firebrick] as PdfBrush;
        if (firebrick == null)
          firebrick = PdfBrushes.GetBrush(KnownColor.Firebrick);
        return firebrick;
      }
    }
  }

  public static PdfBrush FloralWhite
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush floralWhite = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.FloralWhite))
          floralWhite = PdfBrushes.s_brushes[(object) KnownColor.FloralWhite] as PdfBrush;
        if (floralWhite == null)
          floralWhite = PdfBrushes.GetBrush(KnownColor.FloralWhite);
        return floralWhite;
      }
    }
  }

  public static PdfBrush ForestGreen
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush forestGreen = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.ForestGreen))
          forestGreen = PdfBrushes.s_brushes[(object) KnownColor.ForestGreen] as PdfBrush;
        if (forestGreen == null)
          forestGreen = PdfBrushes.GetBrush(KnownColor.ForestGreen);
        return forestGreen;
      }
    }
  }

  public static PdfBrush Fuchsia
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush fuchsia = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Fuchsia))
          fuchsia = PdfBrushes.s_brushes[(object) KnownColor.Fuchsia] as PdfBrush;
        if (fuchsia == null)
          fuchsia = PdfBrushes.GetBrush(KnownColor.Fuchsia);
        return fuchsia;
      }
    }
  }

  public static PdfBrush Gainsboro
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush gainsboro = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Gainsboro))
          gainsboro = PdfBrushes.s_brushes[(object) KnownColor.Gainsboro] as PdfBrush;
        if (gainsboro == null)
          gainsboro = PdfBrushes.GetBrush(KnownColor.Gainsboro);
        return gainsboro;
      }
    }
  }

  public static PdfBrush GhostWhite
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush ghostWhite = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.GhostWhite))
          ghostWhite = PdfBrushes.s_brushes[(object) KnownColor.GhostWhite] as PdfBrush;
        if (ghostWhite == null)
          ghostWhite = PdfBrushes.GetBrush(KnownColor.GhostWhite);
        return ghostWhite;
      }
    }
  }

  public static PdfBrush Gold
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush gold = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Gold))
          gold = PdfBrushes.s_brushes[(object) KnownColor.Gold] as PdfBrush;
        if (gold == null)
          gold = PdfBrushes.GetBrush(KnownColor.Gold);
        return gold;
      }
    }
  }

  public static PdfBrush Goldenrod
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush goldenrod = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Goldenrod))
          goldenrod = PdfBrushes.s_brushes[(object) KnownColor.Goldenrod] as PdfBrush;
        if (goldenrod == null)
          goldenrod = PdfBrushes.GetBrush(KnownColor.Goldenrod);
        return goldenrod;
      }
    }
  }

  public static PdfBrush Gray
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush gray = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Gray))
          gray = PdfBrushes.s_brushes[(object) KnownColor.Gray] as PdfBrush;
        if (gray == null)
          gray = PdfBrushes.GetBrush(KnownColor.Gray);
        return gray;
      }
    }
  }

  public static PdfBrush Green
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush green = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Green))
          green = PdfBrushes.s_brushes[(object) KnownColor.Green] as PdfBrush;
        if (green == null)
          green = PdfBrushes.GetBrush(KnownColor.Green);
        return green;
      }
    }
  }

  public static PdfBrush GreenYellow
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush greenYellow = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.GreenYellow))
          greenYellow = PdfBrushes.s_brushes[(object) KnownColor.GreenYellow] as PdfBrush;
        if (greenYellow == null)
          greenYellow = PdfBrushes.GetBrush(KnownColor.GreenYellow);
        return greenYellow;
      }
    }
  }

  public static PdfBrush Honeydew
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush honeydew = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Honeydew))
          honeydew = PdfBrushes.s_brushes[(object) KnownColor.Honeydew] as PdfBrush;
        if (honeydew == null)
          honeydew = PdfBrushes.GetBrush(KnownColor.Honeydew);
        return honeydew;
      }
    }
  }

  public static PdfBrush HotPink
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush hotPink = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.HotPink))
          hotPink = PdfBrushes.s_brushes[(object) KnownColor.HotPink] as PdfBrush;
        if (hotPink == null)
          hotPink = PdfBrushes.GetBrush(KnownColor.HotPink);
        return hotPink;
      }
    }
  }

  public static PdfBrush IndianRed
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush indianRed = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.IndianRed))
          indianRed = PdfBrushes.s_brushes[(object) KnownColor.IndianRed] as PdfBrush;
        if (indianRed == null)
          indianRed = PdfBrushes.GetBrush(KnownColor.IndianRed);
        return indianRed;
      }
    }
  }

  public static PdfBrush Indigo
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush indigo = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Indigo))
          indigo = PdfBrushes.s_brushes[(object) KnownColor.Indigo] as PdfBrush;
        if (indigo == null)
          indigo = PdfBrushes.GetBrush(KnownColor.Indigo);
        return indigo;
      }
    }
  }

  public static PdfBrush Ivory
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush ivory = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Ivory))
          ivory = PdfBrushes.s_brushes[(object) KnownColor.Ivory] as PdfBrush;
        if (ivory == null)
          ivory = PdfBrushes.GetBrush(KnownColor.Ivory);
        return ivory;
      }
    }
  }

  public static PdfBrush Khaki
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush khaki = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Khaki))
          khaki = PdfBrushes.s_brushes[(object) KnownColor.Khaki] as PdfBrush;
        if (khaki == null)
          khaki = PdfBrushes.GetBrush(KnownColor.Khaki);
        return khaki;
      }
    }
  }

  public static PdfBrush Lavender
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush lavender = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Lavender))
          lavender = PdfBrushes.s_brushes[(object) KnownColor.Lavender] as PdfBrush;
        if (lavender == null)
          lavender = PdfBrushes.GetBrush(KnownColor.Lavender);
        return lavender;
      }
    }
  }

  public static PdfBrush LavenderBlush
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush lavenderBlush = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.LavenderBlush))
          lavenderBlush = PdfBrushes.s_brushes[(object) KnownColor.LavenderBlush] as PdfBrush;
        if (lavenderBlush == null)
          lavenderBlush = PdfBrushes.GetBrush(KnownColor.LavenderBlush);
        return lavenderBlush;
      }
    }
  }

  public static PdfBrush LawnGreen
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush lawnGreen = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.LawnGreen))
          lawnGreen = PdfBrushes.s_brushes[(object) KnownColor.LawnGreen] as PdfBrush;
        if (lawnGreen == null)
          lawnGreen = PdfBrushes.GetBrush(KnownColor.LawnGreen);
        return lawnGreen;
      }
    }
  }

  public static PdfBrush LemonChiffon
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush lemonChiffon = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.LemonChiffon))
          lemonChiffon = PdfBrushes.s_brushes[(object) KnownColor.LemonChiffon] as PdfBrush;
        if (lemonChiffon == null)
          lemonChiffon = PdfBrushes.GetBrush(KnownColor.LemonChiffon);
        return lemonChiffon;
      }
    }
  }

  public static PdfBrush LightBlue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush lightBlue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.LightBlue))
          lightBlue = PdfBrushes.s_brushes[(object) KnownColor.LightBlue] as PdfBrush;
        if (lightBlue == null)
          lightBlue = PdfBrushes.GetBrush(KnownColor.LightBlue);
        return lightBlue;
      }
    }
  }

  public static PdfBrush LightCoral
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush lightCoral = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.LightCoral))
          lightCoral = PdfBrushes.s_brushes[(object) KnownColor.LightCoral] as PdfBrush;
        if (lightCoral == null)
          lightCoral = PdfBrushes.GetBrush(KnownColor.LightCoral);
        return lightCoral;
      }
    }
  }

  public static PdfBrush LightCyan
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush lightCyan = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.LightCyan))
          lightCyan = PdfBrushes.s_brushes[(object) KnownColor.LightCyan] as PdfBrush;
        if (lightCyan == null)
          lightCyan = PdfBrushes.GetBrush(KnownColor.LightCyan);
        return lightCyan;
      }
    }
  }

  public static PdfBrush LightGoldenrodYellow
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush lightGoldenrodYellow = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.LightGoldenrodYellow))
          lightGoldenrodYellow = PdfBrushes.s_brushes[(object) KnownColor.LightGoldenrodYellow] as PdfBrush;
        if (lightGoldenrodYellow == null)
          lightGoldenrodYellow = PdfBrushes.GetBrush(KnownColor.LightGoldenrodYellow);
        return lightGoldenrodYellow;
      }
    }
  }

  public static PdfBrush LightGray
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush lightGray = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.LightGray))
          lightGray = PdfBrushes.s_brushes[(object) KnownColor.LightGray] as PdfBrush;
        if (lightGray == null)
          lightGray = PdfBrushes.GetBrush(KnownColor.LightGray);
        return lightGray;
      }
    }
  }

  public static PdfBrush LightGreen
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush lightGreen = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.LightGreen))
          lightGreen = PdfBrushes.s_brushes[(object) KnownColor.LightGreen] as PdfBrush;
        if (lightGreen == null)
          lightGreen = PdfBrushes.GetBrush(KnownColor.LightGreen);
        return lightGreen;
      }
    }
  }

  public static PdfBrush LightPink
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush lightPink = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.LightPink))
          lightPink = PdfBrushes.s_brushes[(object) KnownColor.LightPink] as PdfBrush;
        if (lightPink == null)
          lightPink = PdfBrushes.GetBrush(KnownColor.LightPink);
        return lightPink;
      }
    }
  }

  public static PdfBrush LightSalmon
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush lightSalmon = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.LightSalmon))
          lightSalmon = PdfBrushes.s_brushes[(object) KnownColor.LightSalmon] as PdfBrush;
        if (lightSalmon == null)
          lightSalmon = PdfBrushes.GetBrush(KnownColor.LightSalmon);
        return lightSalmon;
      }
    }
  }

  public static PdfBrush LightSeaGreen
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush lightSeaGreen = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.LightSeaGreen))
          lightSeaGreen = PdfBrushes.s_brushes[(object) KnownColor.LightSeaGreen] as PdfBrush;
        if (lightSeaGreen == null)
          lightSeaGreen = PdfBrushes.GetBrush(KnownColor.LightSeaGreen);
        return lightSeaGreen;
      }
    }
  }

  public static PdfBrush LightSkyBlue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush lightSkyBlue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.LightSkyBlue))
          lightSkyBlue = PdfBrushes.s_brushes[(object) KnownColor.LightSkyBlue] as PdfBrush;
        if (lightSkyBlue == null)
          lightSkyBlue = PdfBrushes.GetBrush(KnownColor.LightSkyBlue);
        return lightSkyBlue;
      }
    }
  }

  public static PdfBrush LightSlateGray
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush lightSlateGray = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.LightSlateGray))
          lightSlateGray = PdfBrushes.s_brushes[(object) KnownColor.LightSlateGray] as PdfBrush;
        if (lightSlateGray == null)
          lightSlateGray = PdfBrushes.GetBrush(KnownColor.LightSlateGray);
        return lightSlateGray;
      }
    }
  }

  public static PdfBrush LightSteelBlue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush lightSteelBlue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.LightSteelBlue))
          lightSteelBlue = PdfBrushes.s_brushes[(object) KnownColor.LightSteelBlue] as PdfBrush;
        if (lightSteelBlue == null)
          lightSteelBlue = PdfBrushes.GetBrush(KnownColor.LightSteelBlue);
        return lightSteelBlue;
      }
    }
  }

  public static PdfBrush LightYellow
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush lightYellow = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.LightYellow))
          lightYellow = PdfBrushes.s_brushes[(object) KnownColor.LightYellow] as PdfBrush;
        if (lightYellow == null)
          lightYellow = PdfBrushes.GetBrush(KnownColor.LightYellow);
        return lightYellow;
      }
    }
  }

  public static PdfBrush Lime
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush lime = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Lime))
          lime = PdfBrushes.s_brushes[(object) KnownColor.Lime] as PdfBrush;
        if (lime == null)
          lime = PdfBrushes.GetBrush(KnownColor.Lime);
        return lime;
      }
    }
  }

  public static PdfBrush LimeGreen
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush limeGreen = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.LimeGreen))
          limeGreen = PdfBrushes.s_brushes[(object) KnownColor.LimeGreen] as PdfBrush;
        if (limeGreen == null)
          limeGreen = PdfBrushes.GetBrush(KnownColor.LimeGreen);
        return limeGreen;
      }
    }
  }

  public static PdfBrush Linen
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush linen = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Linen))
          linen = PdfBrushes.s_brushes[(object) KnownColor.Linen] as PdfBrush;
        if (linen == null)
          linen = PdfBrushes.GetBrush(KnownColor.Linen);
        return linen;
      }
    }
  }

  public static PdfBrush Magenta
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush magenta = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Magenta))
          magenta = PdfBrushes.s_brushes[(object) KnownColor.Magenta] as PdfBrush;
        if (magenta == null)
          magenta = PdfBrushes.GetBrush(KnownColor.Magenta);
        return magenta;
      }
    }
  }

  public static PdfBrush Maroon
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush maroon = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Maroon))
          maroon = PdfBrushes.s_brushes[(object) KnownColor.Maroon] as PdfBrush;
        if (maroon == null)
          maroon = PdfBrushes.GetBrush(KnownColor.Maroon);
        return maroon;
      }
    }
  }

  public static PdfBrush MediumAquamarine
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush mediumAquamarine = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.MediumAquamarine))
          mediumAquamarine = PdfBrushes.s_brushes[(object) KnownColor.MediumAquamarine] as PdfBrush;
        if (mediumAquamarine == null)
          mediumAquamarine = PdfBrushes.GetBrush(KnownColor.MediumAquamarine);
        return mediumAquamarine;
      }
    }
  }

  public static PdfBrush MediumBlue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush mediumBlue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.MediumBlue))
          mediumBlue = PdfBrushes.s_brushes[(object) KnownColor.MediumBlue] as PdfBrush;
        if (mediumBlue == null)
          mediumBlue = PdfBrushes.GetBrush(KnownColor.MediumBlue);
        return mediumBlue;
      }
    }
  }

  public static PdfBrush MediumOrchid
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush mediumOrchid = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.MediumOrchid))
          mediumOrchid = PdfBrushes.s_brushes[(object) KnownColor.MediumOrchid] as PdfBrush;
        if (mediumOrchid == null)
          mediumOrchid = PdfBrushes.GetBrush(KnownColor.MediumOrchid);
        return mediumOrchid;
      }
    }
  }

  public static PdfBrush MediumPurple
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush mediumPurple = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.MediumPurple))
          mediumPurple = PdfBrushes.s_brushes[(object) KnownColor.MediumPurple] as PdfBrush;
        if (mediumPurple == null)
          mediumPurple = PdfBrushes.GetBrush(KnownColor.MediumPurple);
        return mediumPurple;
      }
    }
  }

  public static PdfBrush MediumSeaGreen
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush mediumSeaGreen = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.MediumSeaGreen))
          mediumSeaGreen = PdfBrushes.s_brushes[(object) KnownColor.MediumSeaGreen] as PdfBrush;
        if (mediumSeaGreen == null)
          mediumSeaGreen = PdfBrushes.GetBrush(KnownColor.MediumSeaGreen);
        return mediumSeaGreen;
      }
    }
  }

  public static PdfBrush MediumSlateBlue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush mediumSlateBlue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.MediumSlateBlue))
          mediumSlateBlue = PdfBrushes.s_brushes[(object) KnownColor.MediumSlateBlue] as PdfBrush;
        if (mediumSlateBlue == null)
          mediumSlateBlue = PdfBrushes.GetBrush(KnownColor.MediumSlateBlue);
        return mediumSlateBlue;
      }
    }
  }

  public static PdfBrush MediumSpringGreen
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush mediumSpringGreen = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.MediumSpringGreen))
          mediumSpringGreen = PdfBrushes.s_brushes[(object) KnownColor.MediumSpringGreen] as PdfBrush;
        if (mediumSpringGreen == null)
          mediumSpringGreen = PdfBrushes.GetBrush(KnownColor.MediumSpringGreen);
        return mediumSpringGreen;
      }
    }
  }

  public static PdfBrush MediumTurquoise
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush mediumTurquoise = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.MediumTurquoise))
          mediumTurquoise = PdfBrushes.s_brushes[(object) KnownColor.MediumTurquoise] as PdfBrush;
        if (mediumTurquoise == null)
          mediumTurquoise = PdfBrushes.GetBrush(KnownColor.MediumTurquoise);
        return mediumTurquoise;
      }
    }
  }

  public static PdfBrush MediumVioletRed
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush mediumVioletRed = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.MediumVioletRed))
          mediumVioletRed = PdfBrushes.s_brushes[(object) KnownColor.MediumVioletRed] as PdfBrush;
        if (mediumVioletRed == null)
          mediumVioletRed = PdfBrushes.GetBrush(KnownColor.MediumVioletRed);
        return mediumVioletRed;
      }
    }
  }

  public static PdfBrush MidnightBlue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush midnightBlue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.MidnightBlue))
          midnightBlue = PdfBrushes.s_brushes[(object) KnownColor.MidnightBlue] as PdfBrush;
        if (midnightBlue == null)
          midnightBlue = PdfBrushes.GetBrush(KnownColor.MidnightBlue);
        return midnightBlue;
      }
    }
  }

  public static PdfBrush MintCream
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush mintCream = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.MintCream))
          mintCream = PdfBrushes.s_brushes[(object) KnownColor.MintCream] as PdfBrush;
        if (mintCream == null)
          mintCream = PdfBrushes.GetBrush(KnownColor.MintCream);
        return mintCream;
      }
    }
  }

  public static PdfBrush MistyRose
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush mistyRose = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.MistyRose))
          mistyRose = PdfBrushes.s_brushes[(object) KnownColor.MistyRose] as PdfBrush;
        if (mistyRose == null)
          mistyRose = PdfBrushes.GetBrush(KnownColor.MistyRose);
        return mistyRose;
      }
    }
  }

  public static PdfBrush Moccasin
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush moccasin = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Moccasin))
          moccasin = PdfBrushes.s_brushes[(object) KnownColor.Moccasin] as PdfBrush;
        if (moccasin == null)
          moccasin = PdfBrushes.GetBrush(KnownColor.Moccasin);
        return moccasin;
      }
    }
  }

  public static PdfBrush NavajoWhite
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush navajoWhite = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.NavajoWhite))
          navajoWhite = PdfBrushes.s_brushes[(object) KnownColor.NavajoWhite] as PdfBrush;
        if (navajoWhite == null)
          navajoWhite = PdfBrushes.GetBrush(KnownColor.NavajoWhite);
        return navajoWhite;
      }
    }
  }

  public static PdfBrush Navy
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush navy = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Navy))
          navy = PdfBrushes.s_brushes[(object) KnownColor.Navy] as PdfBrush;
        if (navy == null)
          navy = PdfBrushes.GetBrush(KnownColor.Navy);
        return navy;
      }
    }
  }

  public static PdfBrush OldLace
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush oldLace = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.OldLace))
          oldLace = PdfBrushes.s_brushes[(object) KnownColor.OldLace] as PdfBrush;
        if (oldLace == null)
          oldLace = PdfBrushes.GetBrush(KnownColor.OldLace);
        return oldLace;
      }
    }
  }

  public static PdfBrush Olive
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush olive = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Olive))
          olive = PdfBrushes.s_brushes[(object) KnownColor.Olive] as PdfBrush;
        if (olive == null)
          olive = PdfBrushes.GetBrush(KnownColor.Olive);
        return olive;
      }
    }
  }

  public static PdfBrush OliveDrab
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush oliveDrab = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.OliveDrab))
          oliveDrab = PdfBrushes.s_brushes[(object) KnownColor.OliveDrab] as PdfBrush;
        if (oliveDrab == null)
          oliveDrab = PdfBrushes.GetBrush(KnownColor.OliveDrab);
        return oliveDrab;
      }
    }
  }

  public static PdfBrush Orange
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush orange = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Orange))
          orange = PdfBrushes.s_brushes[(object) KnownColor.Orange] as PdfBrush;
        if (orange == null)
          orange = PdfBrushes.GetBrush(KnownColor.Orange);
        return orange;
      }
    }
  }

  public static PdfBrush OrangeRed
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush orangeRed = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.OrangeRed))
          orangeRed = PdfBrushes.s_brushes[(object) KnownColor.OrangeRed] as PdfBrush;
        if (orangeRed == null)
          orangeRed = PdfBrushes.GetBrush(KnownColor.OrangeRed);
        return orangeRed;
      }
    }
  }

  public static PdfBrush Orchid
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush orchid = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Orchid))
          orchid = PdfBrushes.s_brushes[(object) KnownColor.Orchid] as PdfBrush;
        if (orchid == null)
          orchid = PdfBrushes.GetBrush(KnownColor.Orchid);
        return orchid;
      }
    }
  }

  public static PdfBrush PaleGoldenrod
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush paleGoldenrod = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.PaleGoldenrod))
          paleGoldenrod = PdfBrushes.s_brushes[(object) KnownColor.PaleGoldenrod] as PdfBrush;
        if (paleGoldenrod == null)
          paleGoldenrod = PdfBrushes.GetBrush(KnownColor.PaleGoldenrod);
        return paleGoldenrod;
      }
    }
  }

  public static PdfBrush PaleGreen
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush paleGreen = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.PaleGreen))
          paleGreen = PdfBrushes.s_brushes[(object) KnownColor.PaleGreen] as PdfBrush;
        if (paleGreen == null)
          paleGreen = PdfBrushes.GetBrush(KnownColor.PaleGreen);
        return paleGreen;
      }
    }
  }

  public static PdfBrush PaleTurquoise
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush paleTurquoise = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.PaleTurquoise))
          paleTurquoise = PdfBrushes.s_brushes[(object) KnownColor.PaleTurquoise] as PdfBrush;
        if (paleTurquoise == null)
          paleTurquoise = PdfBrushes.GetBrush(KnownColor.PaleTurquoise);
        return paleTurquoise;
      }
    }
  }

  public static PdfBrush PaleVioletRed
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush paleVioletRed = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.PaleVioletRed))
          paleVioletRed = PdfBrushes.s_brushes[(object) KnownColor.PaleVioletRed] as PdfBrush;
        if (paleVioletRed == null)
          paleVioletRed = PdfBrushes.GetBrush(KnownColor.PaleVioletRed);
        return paleVioletRed;
      }
    }
  }

  public static PdfBrush PapayaWhip
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush papayaWhip = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.PapayaWhip))
          papayaWhip = PdfBrushes.s_brushes[(object) KnownColor.PapayaWhip] as PdfBrush;
        if (papayaWhip == null)
          papayaWhip = PdfBrushes.GetBrush(KnownColor.PapayaWhip);
        return papayaWhip;
      }
    }
  }

  public static PdfBrush PeachPuff
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush peachPuff = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.PeachPuff))
          peachPuff = PdfBrushes.s_brushes[(object) KnownColor.PeachPuff] as PdfBrush;
        if (peachPuff == null)
          peachPuff = PdfBrushes.GetBrush(KnownColor.PeachPuff);
        return peachPuff;
      }
    }
  }

  public static PdfBrush Peru
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush peru = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Peru))
          peru = PdfBrushes.s_brushes[(object) KnownColor.Peru] as PdfBrush;
        if (peru == null)
          peru = PdfBrushes.GetBrush(KnownColor.Peru);
        return peru;
      }
    }
  }

  public static PdfBrush Pink
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush pink = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Pink))
          pink = PdfBrushes.s_brushes[(object) KnownColor.Pink] as PdfBrush;
        if (pink == null)
          pink = PdfBrushes.GetBrush(KnownColor.Pink);
        return pink;
      }
    }
  }

  public static PdfBrush Plum
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush plum = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Plum))
          plum = PdfBrushes.s_brushes[(object) KnownColor.Plum] as PdfBrush;
        if (plum == null)
          plum = PdfBrushes.GetBrush(KnownColor.Plum);
        return plum;
      }
    }
  }

  public static PdfBrush PowderBlue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush powderBlue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.PowderBlue))
          powderBlue = PdfBrushes.s_brushes[(object) KnownColor.PowderBlue] as PdfBrush;
        if (powderBlue == null)
          powderBlue = PdfBrushes.GetBrush(KnownColor.PowderBlue);
        return powderBlue;
      }
    }
  }

  public static PdfBrush Purple
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush purple = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Purple))
          purple = PdfBrushes.s_brushes[(object) KnownColor.Purple] as PdfBrush;
        if (purple == null)
          purple = PdfBrushes.GetBrush(KnownColor.Purple);
        return purple;
      }
    }
  }

  public static PdfBrush Red
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush red = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Red))
          red = PdfBrushes.s_brushes[(object) KnownColor.Red] as PdfBrush;
        if (red == null)
          red = PdfBrushes.GetBrush(KnownColor.Red);
        return red;
      }
    }
  }

  public static PdfBrush RosyBrown
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush rosyBrown = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.RosyBrown))
          rosyBrown = PdfBrushes.s_brushes[(object) KnownColor.RosyBrown] as PdfBrush;
        if (rosyBrown == null)
          rosyBrown = PdfBrushes.GetBrush(KnownColor.RosyBrown);
        return rosyBrown;
      }
    }
  }

  public static PdfBrush RoyalBlue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush royalBlue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.RoyalBlue))
          royalBlue = PdfBrushes.s_brushes[(object) KnownColor.RoyalBlue] as PdfBrush;
        if (royalBlue == null)
          royalBlue = PdfBrushes.GetBrush(KnownColor.RoyalBlue);
        return royalBlue;
      }
    }
  }

  public static PdfBrush SaddleBrown
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush saddleBrown = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.SaddleBrown))
          saddleBrown = PdfBrushes.s_brushes[(object) KnownColor.SaddleBrown] as PdfBrush;
        if (saddleBrown == null)
          saddleBrown = PdfBrushes.GetBrush(KnownColor.SaddleBrown);
        return saddleBrown;
      }
    }
  }

  public static PdfBrush Salmon
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush salmon = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Salmon))
          salmon = PdfBrushes.s_brushes[(object) KnownColor.Salmon] as PdfBrush;
        if (salmon == null)
          salmon = PdfBrushes.GetBrush(KnownColor.Salmon);
        return salmon;
      }
    }
  }

  public static PdfBrush SandyBrown
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush sandyBrown = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.SandyBrown))
          sandyBrown = PdfBrushes.s_brushes[(object) KnownColor.SandyBrown] as PdfBrush;
        if (sandyBrown == null)
          sandyBrown = PdfBrushes.GetBrush(KnownColor.SandyBrown);
        return sandyBrown;
      }
    }
  }

  public static PdfBrush SeaGreen
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush seaGreen = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.SeaGreen))
          seaGreen = PdfBrushes.s_brushes[(object) KnownColor.SeaGreen] as PdfBrush;
        if (seaGreen == null)
          seaGreen = PdfBrushes.GetBrush(KnownColor.SeaGreen);
        return seaGreen;
      }
    }
  }

  public static PdfBrush SeaShell
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush seaShell = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.SeaShell))
          seaShell = PdfBrushes.s_brushes[(object) KnownColor.SeaShell] as PdfBrush;
        if (seaShell == null)
          seaShell = PdfBrushes.GetBrush(KnownColor.SeaShell);
        return seaShell;
      }
    }
  }

  public static PdfBrush Sienna
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush sienna = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Sienna))
          sienna = PdfBrushes.s_brushes[(object) KnownColor.Sienna] as PdfBrush;
        if (sienna == null)
          sienna = PdfBrushes.GetBrush(KnownColor.Sienna);
        return sienna;
      }
    }
  }

  public static PdfBrush Silver
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush silver = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Silver))
          silver = PdfBrushes.s_brushes[(object) KnownColor.Silver] as PdfBrush;
        if (silver == null)
          silver = PdfBrushes.GetBrush(KnownColor.Silver);
        return silver;
      }
    }
  }

  public static PdfBrush SkyBlue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush skyBlue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.SkyBlue))
          skyBlue = PdfBrushes.s_brushes[(object) KnownColor.SkyBlue] as PdfBrush;
        if (skyBlue == null)
          skyBlue = PdfBrushes.GetBrush(KnownColor.SkyBlue);
        return skyBlue;
      }
    }
  }

  public static PdfBrush SlateBlue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush slateBlue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.SlateBlue))
          slateBlue = PdfBrushes.s_brushes[(object) KnownColor.SlateBlue] as PdfBrush;
        if (slateBlue == null)
          slateBlue = PdfBrushes.GetBrush(KnownColor.SlateBlue);
        return slateBlue;
      }
    }
  }

  public static PdfBrush SlateGray
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush slateGray = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.SlateGray))
          slateGray = PdfBrushes.s_brushes[(object) KnownColor.SlateGray] as PdfBrush;
        if (slateGray == null)
          slateGray = PdfBrushes.GetBrush(KnownColor.SlateGray);
        return slateGray;
      }
    }
  }

  public static PdfBrush Snow
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush snow = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Snow))
          snow = PdfBrushes.s_brushes[(object) KnownColor.Snow] as PdfBrush;
        if (snow == null)
          snow = PdfBrushes.GetBrush(KnownColor.Snow);
        return snow;
      }
    }
  }

  public static PdfBrush SpringGreen
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush springGreen = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.SpringGreen))
          springGreen = PdfBrushes.s_brushes[(object) KnownColor.SpringGreen] as PdfBrush;
        if (springGreen == null)
          springGreen = PdfBrushes.GetBrush(KnownColor.SpringGreen);
        return springGreen;
      }
    }
  }

  public static PdfBrush SteelBlue
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush steelBlue = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.SteelBlue))
          steelBlue = PdfBrushes.s_brushes[(object) KnownColor.SteelBlue] as PdfBrush;
        if (steelBlue == null)
          steelBlue = PdfBrushes.GetBrush(KnownColor.SteelBlue);
        return steelBlue;
      }
    }
  }

  public static PdfBrush Tan
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush tan = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Tan))
          tan = PdfBrushes.s_brushes[(object) KnownColor.Tan] as PdfBrush;
        if (tan == null)
          tan = PdfBrushes.GetBrush(KnownColor.Tan);
        return tan;
      }
    }
  }

  public static PdfBrush Teal
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush teal = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Teal))
          teal = PdfBrushes.s_brushes[(object) KnownColor.Teal] as PdfBrush;
        if (teal == null)
          teal = PdfBrushes.GetBrush(KnownColor.Teal);
        return teal;
      }
    }
  }

  public static PdfBrush Thistle
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush thistle = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Thistle))
          thistle = PdfBrushes.s_brushes[(object) KnownColor.Thistle] as PdfBrush;
        if (thistle == null)
          thistle = PdfBrushes.GetBrush(KnownColor.Thistle);
        return thistle;
      }
    }
  }

  public static PdfBrush Tomato
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush tomato = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Tomato))
          tomato = PdfBrushes.s_brushes[(object) KnownColor.Tomato] as PdfBrush;
        if (tomato == null)
          tomato = PdfBrushes.GetBrush(KnownColor.Tomato);
        return tomato;
      }
    }
  }

  public static PdfBrush Transparent
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush transparent = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Transparent))
          transparent = PdfBrushes.s_brushes[(object) KnownColor.Transparent] as PdfBrush;
        if (transparent == null)
          transparent = PdfBrushes.GetBrush(KnownColor.Transparent);
        return transparent;
      }
    }
  }

  public static PdfBrush Turquoise
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush turquoise = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Turquoise))
          turquoise = PdfBrushes.s_brushes[(object) KnownColor.Turquoise] as PdfBrush;
        if (turquoise == null)
          turquoise = PdfBrushes.GetBrush(KnownColor.Turquoise);
        return turquoise;
      }
    }
  }

  public static PdfBrush Violet
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush violet = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Violet))
          violet = PdfBrushes.s_brushes[(object) KnownColor.Violet] as PdfBrush;
        if (violet == null)
          violet = PdfBrushes.GetBrush(KnownColor.Violet);
        return violet;
      }
    }
  }

  public static PdfBrush Wheat
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush wheat = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Wheat))
          wheat = PdfBrushes.s_brushes[(object) KnownColor.Wheat] as PdfBrush;
        if (wheat == null)
          wheat = PdfBrushes.GetBrush(KnownColor.Wheat);
        return wheat;
      }
    }
  }

  public static PdfBrush White
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush white = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.White))
          white = PdfBrushes.s_brushes[(object) KnownColor.White] as PdfBrush;
        if (white == null)
          white = PdfBrushes.GetBrush(KnownColor.White);
        return white;
      }
    }
  }

  public static PdfBrush WhiteSmoke
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush whiteSmoke = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.WhiteSmoke))
          whiteSmoke = PdfBrushes.s_brushes[(object) KnownColor.WhiteSmoke] as PdfBrush;
        if (whiteSmoke == null)
          whiteSmoke = PdfBrushes.GetBrush(KnownColor.WhiteSmoke);
        return whiteSmoke;
      }
    }
  }

  public static PdfBrush Yellow
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush yellow = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.Yellow))
          yellow = PdfBrushes.s_brushes[(object) KnownColor.Yellow] as PdfBrush;
        if (yellow == null)
          yellow = PdfBrushes.GetBrush(KnownColor.Yellow);
        return yellow;
      }
    }
  }

  public static PdfBrush YellowGreen
  {
    get
    {
      lock (PdfBrushes.s_brushes)
      {
        PdfBrush yellowGreen = (PdfBrush) null;
        if (PdfBrushes.s_brushes.ContainsKey((object) KnownColor.YellowGreen))
          yellowGreen = PdfBrushes.s_brushes[(object) KnownColor.YellowGreen] as PdfBrush;
        if (yellowGreen == null)
          yellowGreen = PdfBrushes.GetBrush(KnownColor.YellowGreen);
        return yellowGreen;
      }
    }
  }

  private static PdfBrush GetBrush(KnownColor colorName)
  {
    Color color = Color.FromKnownColor(colorName);
    PdfBrush brush = (PdfBrush) new PdfSolidBrush(new PdfColor(color), true);
    PdfBrushes.s_brushes[(object) color] = (object) brush;
    return brush;
  }
}
