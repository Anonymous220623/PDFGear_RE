// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.AnWTFilterSpec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf;

internal class AnWTFilterSpec : ModuleSpec
{
  private const string REV_FILTER_STR = "w5x3";
  private const string NON_REV_FILTER_STR = "w9x7";

  internal AnWTFilterSpec(int nt, int nc, byte type, QuantTypeSpec qts, JPXParameters pl)
    : base(nt, nc, type)
  {
    pl.checkList('F', JPXParameters.toNameArray(AnWTFilter.ParameterInfo));
    string parameter = pl.getParameter("Ffilters");
    bool flag = true;
    if (parameter == null)
    {
      if (pl.getBooleanParameter("lossless"))
      {
        this.setDefault((object) this.parseFilters("w5x3"));
      }
      else
      {
        for (int t = nt - 1; t >= 0; --t)
        {
          for (int c = nc - 1; c >= 0; --c)
          {
            switch (qts.getSpecValType(t, c))
            {
              case 0:
                if (this.getDefault() == null)
                {
                  if (pl.getBooleanParameter("lossless"))
                    this.setDefault((object) this.parseFilters("w5x3"));
                  if (((string) qts.getDefault()).Equals("reversible"))
                    this.setDefault((object) this.parseFilters("w5x3"));
                  else
                    this.setDefault((object) this.parseFilters("w9x7"));
                }
                this.specValType[t][c] = (byte) 0;
                break;
              case 1:
                if (!this.isCompSpecified(c))
                {
                  if (((string) qts.getCompDef(c)).Equals("reversible"))
                    this.setCompDef(c, (object) this.parseFilters("w5x3"));
                  else
                    this.setCompDef(c, (object) this.parseFilters("w9x7"));
                }
                this.specValType[t][c] = (byte) 1;
                break;
              case 2:
                if (!this.isTileSpecified(t))
                {
                  if (((string) qts.getTileDef(t)).Equals("reversible"))
                    this.setTileDef(t, (object) this.parseFilters("w5x3"));
                  else
                    this.setTileDef(t, (object) this.parseFilters("w9x7"));
                }
                this.specValType[t][c] = (byte) 2;
                break;
              case 3:
                if (!this.isTileCompSpecified(t, c))
                {
                  if (((string) qts.getTileCompVal(t, c)).Equals("reversible"))
                    this.setTileCompVal(t, c, (object) this.parseFilters("w5x3"));
                  else
                    this.setTileCompVal(t, c, (object) this.parseFilters("w9x7"));
                }
                this.specValType[t][c] = (byte) 3;
                break;
              default:
                throw new ArgumentException("Unsupported specification type");
            }
          }
        }
      }
    }
    else
    {
      SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(parameter);
      byte num1 = 0;
      bool[] flagArray1 = (bool[]) null;
      bool[] flagArray2 = (bool[]) null;
      while (tokenizer.HasMoreTokens())
      {
        string word = tokenizer.NextToken();
        switch (word[0])
        {
          case 'C':
          case 'c':
            flagArray2 = ModuleSpec.parseIdx(word, this.nComp);
            num1 = num1 != (byte) 2 ? (byte) 1 : (byte) 3;
            continue;
          case 'T':
          case 't':
            flagArray1 = ModuleSpec.parseIdx(word, this.nTiles);
            num1 = num1 != (byte) 1 ? (byte) 2 : (byte) 3;
            continue;
          case 'W':
          case 'w':
            AnWTFilter[][] value_Renamed = !pl.getBooleanParameter("lossless") || !word.ToUpper().Equals("w9x7".ToUpper()) ? this.parseFilters(word) : throw new ArgumentException("Cannot use non reversible wavelet transform with '-lossless' option");
            switch (num1)
            {
              case 0:
                this.setDefault((object) value_Renamed);
                break;
              case 1:
                for (int c = flagArray2.Length - 1; c >= 0; --c)
                {
                  if (flagArray2[c])
                    this.setCompDef(c, (object) value_Renamed);
                }
                break;
              case 2:
                for (int t = flagArray1.Length - 1; t >= 0; --t)
                {
                  if (flagArray1[t])
                    this.setTileDef(t, (object) value_Renamed);
                }
                break;
              default:
                for (int t = flagArray1.Length - 1; t >= 0; --t)
                {
                  for (int c = flagArray2.Length - 1; c >= 0; --c)
                  {
                    if (flagArray1[t] && flagArray2[c])
                      this.setTileCompVal(t, c, (object) value_Renamed);
                  }
                }
                break;
            }
            num1 = (byte) 0;
            flagArray1 = (bool[]) null;
            flagArray2 = (bool[]) null;
            continue;
          default:
            throw new ArgumentException("Bad construction for parameter: " + word);
        }
      }
      if (this.getDefault() == null)
      {
        int num2 = 0;
        for (int index1 = nt - 1; index1 >= 0; --index1)
        {
          for (int index2 = nc - 1; index2 >= 0; --index2)
          {
            if (this.specValType[index1][index2] == (byte) 0)
              ++num2;
          }
        }
        if (num2 != 0)
        {
          if (((string) qts.getDefault()).Equals("reversible"))
            this.setDefault((object) this.parseFilters("w5x3"));
          else
            this.setDefault((object) this.parseFilters("w9x7"));
        }
        else
        {
          this.setDefault(this.getTileCompVal(0, 0));
          switch (this.specValType[0][0])
          {
            case 1:
              for (int index = nt - 1; index >= 0; --index)
              {
                if (this.specValType[index][0] == (byte) 1)
                  this.specValType[index][0] = (byte) 0;
              }
              this.compDef[0] = (object) null;
              break;
            case 2:
              for (int index = nc - 1; index >= 0; --index)
              {
                if (this.specValType[0][index] == (byte) 2)
                  this.specValType[0][index] = (byte) 0;
              }
              this.tileDef[0] = (object) null;
              break;
            case 3:
              this.specValType[0][0] = (byte) 0;
              this.tileCompVal[(object) "t0c0"] = (object) null;
              break;
          }
        }
      }
      for (int t = nt - 1; t >= 0; --t)
      {
        for (int c = nc - 1; c >= 0; --c)
        {
          if (((string) qts.getTileCompVal(t, c)).Equals("reversible"))
          {
            if (!this.isReversible(t, c))
            {
              if (!flag)
                this.setTileCompVal(t, c, (object) this.parseFilters("w5x3"));
              else
                throw new ArgumentException($"Filter of tile-component ({(object) t},{(object) c}) does not allow reversible quantization. Specify '-Qtype expounded' or '-Qtype derived'in the command line.");
            }
          }
          else if (this.isReversible(t, c))
          {
            if (!flag)
              this.setTileCompVal(t, c, (object) this.parseFilters("w9x7"));
            else
              throw new ArgumentException($"Filter of tile-component ({(object) t},{(object) c}) does not allow non-reversible quantization. Specify '-Qtype reversible' in the command line");
          }
        }
      }
    }
  }

  private AnWTFilter[][] parseFilters(string word)
  {
    AnWTFilter[][] filters = new AnWTFilter[2][];
    for (int index = 0; index < 2; ++index)
      filters[index] = new AnWTFilter[1];
    if (word.ToUpper().Equals("w5x3".ToUpper()))
    {
      filters[0][0] = (AnWTFilter) new AnWTFilterIntLift5x3();
      filters[1][0] = (AnWTFilter) new AnWTFilterIntLift5x3();
      return filters;
    }
    if (!word.ToUpper().Equals("w9x7".ToUpper()))
      throw new ArgumentException("Non JPEG 2000 part I filter: " + word);
    filters[0][0] = (AnWTFilter) new AnWTFilterFloatLift9x7();
    filters[1][0] = (AnWTFilter) new AnWTFilterFloatLift9x7();
    return filters;
  }

  public virtual int getWTDataType(int t, int c)
  {
    return ((AnWTFilter[][]) this.getSpec(t, c))[0][0].DataType;
  }

  public virtual AnWTFilter[] getHFilters(int t, int c) => ((AnWTFilter[][]) this.getSpec(t, c))[0];

  public virtual AnWTFilter[] getVFilters(int t, int c) => ((AnWTFilter[][]) this.getSpec(t, c))[1];

  public override string ToString()
  {
    string str1 = $"nTiles={(object) this.nTiles}\nnComp={(object) this.nComp}\n\n";
    for (int t = 0; t < this.nTiles; ++t)
    {
      for (int c = 0; c < this.nComp; ++c)
      {
        AnWTFilter[][] spec = (AnWTFilter[][]) this.getSpec(t, c);
        string str2 = $"{str1}(t:{(object) t},c:{(object) c})\n" + "\tH:";
        for (int index = 0; index < spec[0].Length; ++index)
          str2 = $"{str2} {(object) spec[0][index]}";
        string str3 = str2 + "\n\tV:";
        for (int index = 0; index < spec[1].Length; ++index)
          str3 = $"{str3} {(object) spec[1][index]}";
        str1 = str3 + "\n";
      }
    }
    return str1;
  }

  public virtual bool isReversible(int t, int c)
  {
    AnWTFilter[] hfilters = this.getHFilters(t, c);
    AnWTFilter[] vfilters = this.getVFilters(t, c);
    for (int index = hfilters.Length - 1; index >= 0; --index)
    {
      if (!hfilters[index].Reversible || !vfilters[index].Reversible)
        return false;
    }
    return true;
  }
}
