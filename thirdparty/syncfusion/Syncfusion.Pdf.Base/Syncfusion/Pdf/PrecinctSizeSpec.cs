// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.PrecinctSizeSpec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf;

internal class PrecinctSizeSpec : ModuleSpec
{
  private const string optName = "Cpp";
  private IntegerSpec dls;

  public PrecinctSizeSpec(int nt, int nc, byte type, IntegerSpec dls)
    : base(nt, nc, type)
  {
    this.dls = dls;
  }

  internal PrecinctSizeSpec(
    int nt,
    int nc,
    byte type,
    BlockImageDataSource imgsrc,
    IntegerSpec dls,
    JPXParameters pl)
    : base(nt, nc, type)
  {
    this.dls = dls;
    bool flag1 = false;
    string parameter = pl.getParameter("Cpp");
    List<object>[] value_Renamed1 = new List<object>[2]
    {
      new List<object>(10),
      null
    };
    value_Renamed1[0].Add((object) (int) ushort.MaxValue);
    value_Renamed1[1] = new List<object>(10);
    value_Renamed1[1].Add((object) (int) ushort.MaxValue);
    this.setDefault((object) value_Renamed1);
    if (parameter == null)
      return;
    SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(parameter);
    byte num = 0;
    bool[] flagArray1 = (bool[]) null;
    bool[] flagArray2 = (bool[]) null;
    bool flag2 = false;
    string str = (string) null;
    while ((tokenizer.HasMoreTokens() || flag1) && !flag2)
    {
      List<object>[] value_Renamed2 = new List<object>[2];
      if (!flag1)
        str = tokenizer.NextToken();
      flag1 = false;
      switch (str[0])
      {
        case 'c':
          flagArray2 = ModuleSpec.parseIdx(str, this.nComp);
          num = num != (byte) 2 ? (byte) 1 : (byte) 3;
          continue;
        case 't':
          flagArray1 = ModuleSpec.parseIdx(str, this.nTiles);
          num = num != (byte) 1 ? (byte) 2 : (byte) 3;
          continue;
        default:
          if (!char.IsDigit(str[0]))
            throw new ArgumentException("Bad construction for parameter: " + str);
          value_Renamed2[0] = new List<object>(10);
          value_Renamed2[1] = new List<object>(10);
          do
          {
            int x1;
            int x2;
            try
            {
              x1 = int.Parse(str);
              try
              {
                str = tokenizer.NextToken();
              }
              catch (ArgumentOutOfRangeException ex)
              {
                throw new ArgumentException("'Cpp' option : could not parse the precinct's width");
              }
              x2 = int.Parse(str);
              if (x1 == 1 << MathUtil.log2(x1))
              {
                if (x2 == 1 << MathUtil.log2(x2))
                  goto label_18;
              }
              throw new ArgumentException("Precinct dimensions must be powers of 2");
            }
            catch (FormatException ex)
            {
              throw new ArgumentException($"'Cpp' option : the argument '{str}' could not be parsed.");
            }
label_18:
            value_Renamed2[0].Add((object) x1);
            value_Renamed2[1].Add((object) x2);
            if (tokenizer.HasMoreTokens())
              str = tokenizer.NextToken();
            else
              goto label_41;
          }
          while (char.IsDigit(str[0]));
          flag1 = true;
          switch (num)
          {
            case 0:
              this.setDefault((object) value_Renamed2);
              break;
            case 1:
              for (int c = flagArray2.Length - 1; c >= 0; --c)
              {
                if (flagArray2[c])
                  this.setCompDef(c, (object) value_Renamed2);
              }
              break;
            case 2:
              for (int t = flagArray1.Length - 1; t >= 0; --t)
              {
                if (flagArray1[t])
                  this.setTileDef(t, (object) value_Renamed2);
              }
              break;
            default:
              for (int t = flagArray1.Length - 1; t >= 0; --t)
              {
                for (int c = flagArray2.Length - 1; c >= 0; --c)
                {
                  if (flagArray1[t] && flagArray2[c])
                    this.setTileCompVal(t, c, (object) value_Renamed2);
                }
              }
              break;
          }
          num = (byte) 0;
          flagArray1 = (bool[]) null;
          flagArray2 = (bool[]) null;
          continue;
label_41:
          switch (num)
          {
            case 0:
              this.setDefault((object) value_Renamed2);
              break;
            case 1:
              for (int c = flagArray2.Length - 1; c >= 0; --c)
              {
                if (flagArray2[c])
                  this.setCompDef(c, (object) value_Renamed2);
              }
              break;
            case 2:
              for (int t = flagArray1.Length - 1; t >= 0; --t)
              {
                if (flagArray1[t])
                  this.setTileDef(t, (object) value_Renamed2);
              }
              break;
            default:
              for (int t = flagArray1.Length - 1; t >= 0; --t)
              {
                for (int c = flagArray2.Length - 1; c >= 0; --c)
                {
                  if (flagArray1[t] && flagArray2[c])
                    this.setTileCompVal(t, c, (object) value_Renamed2);
                }
              }
              break;
          }
          flag2 = true;
          continue;
      }
    }
  }

  public virtual int getPPX(int t, int c, int rl)
  {
    bool flag1 = t != -1;
    bool flag2 = c != -1;
    int num;
    List<object>[] objectListArray;
    if (flag1 && flag2)
    {
      num = (int) this.dls.getTileCompVal(t, c);
      objectListArray = (List<object>[]) this.getTileCompVal(t, c);
    }
    else if (flag1 && !flag2)
    {
      num = (int) this.dls.getTileDef(t);
      objectListArray = (List<object>[]) this.getTileDef(t);
    }
    else if (!flag1 && flag2)
    {
      num = (int) this.dls.getCompDef(c);
      objectListArray = (List<object>[]) this.getCompDef(c);
    }
    else
    {
      num = (int) this.dls.getDefault();
      objectListArray = (List<object>[]) this.getDefault();
    }
    int index = num - rl;
    return objectListArray[0].Count > index ? (int) objectListArray[0][index] : (int) objectListArray[0][objectListArray[0].Count - 1];
  }

  public virtual int getPPY(int t, int c, int rl)
  {
    bool flag1 = t != -1;
    bool flag2 = c != -1;
    int num;
    List<object>[] objectListArray;
    if (flag1 && flag2)
    {
      num = (int) this.dls.getTileCompVal(t, c);
      objectListArray = (List<object>[]) this.getTileCompVal(t, c);
    }
    else if (flag1 && !flag2)
    {
      num = (int) this.dls.getTileDef(t);
      objectListArray = (List<object>[]) this.getTileDef(t);
    }
    else if (!flag1 && flag2)
    {
      num = (int) this.dls.getCompDef(c);
      objectListArray = (List<object>[]) this.getCompDef(c);
    }
    else
    {
      num = (int) this.dls.getDefault();
      objectListArray = (List<object>[]) this.getDefault();
    }
    int index = num - rl;
    return objectListArray[1].Count > index ? (int) objectListArray[1][index] : (int) objectListArray[1][objectListArray[1].Count - 1];
  }
}
