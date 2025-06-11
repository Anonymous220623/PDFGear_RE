// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.ProgressionSpec
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections;

#nullable disable
namespace Syncfusion.Pdf;

internal class ProgressionSpec : ModuleSpec
{
  public ProgressionSpec(int nt, int nc, byte type)
    : base(nt, nc, type)
  {
    if (type != (byte) 1)
      throw new ApplicationException("Illegal use of class ProgressionSpec !");
  }

  internal ProgressionSpec(int nt, int nc, int nl, IntegerSpec dls, byte type, JPXParameters pl)
    : base(nt, nc, type)
  {
    string parameter = pl.getParameter("Aptype");
    if (parameter == null)
    {
      int type1 = pl.getParameter("Rroi") != null ? this.checkProgMode("layer") : this.checkProgMode("res");
      if (type1 == -1)
        throw new ArgumentException($"Unknown progression type : '{parameter}'");
      this.setDefault((object) new Progression[1]
      {
        new Progression(type1, 0, nc, 0, dls.Max + 1, nl)
      });
    }
    else
    {
      SupportClass.Tokenizer tokenizer = new SupportClass.Tokenizer(parameter);
      byte num1 = 0;
      bool[] flagArray = (bool[]) null;
      bool flag = false;
      int num2 = 0;
      ArrayList arrayList = ArrayList.Synchronized(new ArrayList(10));
      Progression progression = (Progression) null;
      while (tokenizer.HasMoreTokens())
      {
        string str = tokenizer.NextToken();
        if (str[0] == 't')
        {
          if (arrayList.Count > 0)
          {
            progression.ce = nc;
            progression.lye = nl;
            progression.re = dls.Max + 1;
            Progression[] value_Renamed = new Progression[arrayList.Count];
            arrayList.CopyTo((Array) value_Renamed);
            switch (num1)
            {
              case 0:
                this.setDefault((object) value_Renamed);
                break;
              case 2:
                for (int t = flagArray.Length - 1; t >= 0; --t)
                {
                  if (flagArray[t])
                    this.setTileDef(t, (object) value_Renamed);
                }
                break;
            }
          }
          arrayList.Clear();
          num2 = -1;
          flag = false;
          flagArray = ModuleSpec.parseIdx(str, this.nTiles);
          num1 = (byte) 2;
        }
        else if (flag)
        {
          int num3;
          try
          {
            num3 = int.Parse(str);
          }
          catch (FormatException ex)
          {
            throw new ArgumentException("Progression order specification has missing parameters: " + parameter);
          }
          switch (num2)
          {
            case 0:
              if (num3 < 0 || num3 > dls.Max + 1)
                throw new ArgumentException("Invalid res_start in '-Aptype' option: " + (object) num3);
              progression.rs = num3;
              break;
            case 1:
              progression.cs = num3 >= 0 && num3 <= nc ? num3 : throw new ArgumentException("Invalid comp_start in '-Aptype' option: " + (object) num3);
              break;
            case 2:
              if (num3 < 0)
                throw new ArgumentException("Invalid layer_end in '-Aptype' option: " + (object) num3);
              if (num3 > nl)
                num3 = nl;
              progression.lye = num3;
              break;
            case 3:
              if (num3 < 0)
                throw new ArgumentException("Invalid res_end in '-Aptype' option: " + (object) num3);
              if (num3 > dls.Max + 1)
                num3 = dls.Max + 1;
              progression.re = num3;
              break;
            case 4:
              if (num3 < 0)
                throw new ArgumentException("Invalid comp_end in '-Aptype' option: " + (object) num3);
              if (num3 > nc)
                num3 = nc;
              progression.ce = num3;
              break;
          }
          if (num2 < 4)
          {
            ++num2;
            flag = true;
          }
          else
          {
            num2 = num2 == 4 ? 0 : throw new ApplicationException("Error in usage of 'Aptype' option: " + parameter);
            flag = false;
          }
        }
        else if (!flag)
        {
          int type2 = this.checkProgMode(str);
          if (type2 == -1)
            throw new ArgumentException($"Unknown progression type : '{str}'");
          flag = true;
          num2 = 0;
          progression = arrayList.Count != 0 ? new Progression(type2, 0, nc, 0, dls.Max + 1, nl) : new Progression(type2, 0, nc, 0, dls.Max + 1, nl);
          arrayList.Add((object) progression);
        }
      }
      if (arrayList.Count == 0)
      {
        int type3 = pl.getParameter("Rroi") != null ? this.checkProgMode("layer") : this.checkProgMode("res");
        if (type3 == -1)
          throw new ArgumentException($"Unknown progression type : '{parameter}'");
        this.setDefault((object) new Progression[1]
        {
          new Progression(type3, 0, nc, 0, dls.Max + 1, nl)
        });
      }
      else
      {
        progression.ce = nc;
        progression.lye = nl;
        progression.re = dls.Max + 1;
        Progression[] value_Renamed = new Progression[arrayList.Count];
        arrayList.CopyTo((Array) value_Renamed);
        switch (num1)
        {
          case 0:
            this.setDefault((object) value_Renamed);
            break;
          case 2:
            for (int t = flagArray.Length - 1; t >= 0; --t)
            {
              if (flagArray[t])
                this.setTileDef(t, (object) value_Renamed);
            }
            break;
        }
        if (this.getDefault() != null)
          return;
        int num4 = 0;
        for (int index1 = nt - 1; index1 >= 0; --index1)
        {
          for (int index2 = nc - 1; index2 >= 0; --index2)
          {
            if (this.specValType[index1][index2] == (byte) 0)
              ++num4;
          }
        }
        if (num4 != 0)
        {
          int type4 = pl.getParameter("Rroi") != null ? this.checkProgMode("layer") : this.checkProgMode("res");
          if (type4 == -1)
            throw new ArgumentException($"Unknown progression type : '{parameter}'");
          this.setDefault((object) new Progression[1]
          {
            new Progression(type4, 0, nc, 0, dls.Max + 1, nl)
          });
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
    }
  }

  private int checkProgMode(string mode)
  {
    switch (mode)
    {
      case "res":
        return 1;
      case "layer":
        return 0;
      case "pos-comp":
        return 3;
      case "comp-pos":
        return 4;
      case "res-pos":
        return 2;
      default:
        return -1;
    }
  }
}
