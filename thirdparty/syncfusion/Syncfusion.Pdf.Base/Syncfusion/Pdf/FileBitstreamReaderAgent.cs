// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.FileBitstreamReaderAgent
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Pdf;

internal class FileBitstreamReaderAgent : BitstreamReader
{
  private bool isPsotEqualsZero = true;
  internal PktDecoder pktDec;
  private JPXParameters pl;
  private JPXRandomAccessStream in_Renamed;
  private int[][] firstPackOff;
  private int[] nBytes;
  private bool printInfo;
  private int[] baknBytes;
  private int[][] tilePartLen;
  private int[] totTileLen;
  private int[] totTileHeadLen;
  private int firstTilePartHeadLen;
  private double totAllTileLen;
  private int mainHeadLen;
  private int headLen;
  private int[][] tilePartHeadLen;
  private List<object> pktHL;
  private bool isTruncMode;
  private int remainingTileParts;
  private int[] tilePartsRead;
  private int totTilePartsRead;
  private int[] tileParts;
  private int curTilePart;
  private int[][] tilePartNum;
  private bool isEOCFound;
  private HeaderInformation hi;
  private Syncfusion.Pdf.CBlkInfo[][][][][] cbI;
  private int lQuit;
  private bool usePOCQuit;

  public virtual Syncfusion.Pdf.CBlkInfo[][][][][] CBlkInfo => this.cbI;

  public virtual int getNumTileParts(int t)
  {
    if (this.firstPackOff != null)
    {
      int[] numArray = this.firstPackOff[t];
    }
    return this.firstPackOff[t].Length;
  }

  internal FileBitstreamReaderAgent(
    HeaderDecoder hd,
    JPXRandomAccessStream ehs,
    DecodeHelper decSpec,
    JPXParameters pl,
    bool cdstrInfo,
    HeaderInformation hi)
    : base(hd, decSpec)
  {
    this.pl = pl;
    this.printInfo = cdstrInfo;
    this.hi = hi;
    string str1 = "Codestream elements information in bytes (offset, total length, header length):\n\n";
    this.usePOCQuit = pl.getBooleanParameter("poc_quit");
    pl.getBooleanParameter("parsing");
    try
    {
      this.trate = pl.getFloatParameter("rate");
      if ((double) this.trate == -1.0)
        this.trate = float.MaxValue;
    }
    catch (FormatException ex)
    {
    }
    catch (ArgumentException ex)
    {
    }
    try
    {
      this.tnbytes = pl.getIntParameter("nbytes");
    }
    catch (FormatException ex)
    {
    }
    catch (ArgumentException ex)
    {
    }
    if ((double) this.tnbytes != (double) pl.DefaultParameterList.getFloatParameter("nbytes"))
    {
      this.trate = (float) this.tnbytes * 8f / (float) hd.MaxCompImgWidth / (float) hd.MaxCompImgHeight;
    }
    else
    {
      this.tnbytes = (int) ((double) this.trate * (double) hd.MaxCompImgWidth * (double) hd.MaxCompImgHeight) / 8;
      if (this.tnbytes < 0)
        this.tnbytes = int.MaxValue;
    }
    this.isTruncMode = !pl.getBooleanParameter("parsing");
    int maxCB = 0;
    try
    {
      maxCB = pl.getIntParameter("ncb_quit");
    }
    catch (FormatException ex)
    {
    }
    catch (ArgumentException ex)
    {
    }
    if (maxCB != -1)
    {
      int num1 = this.isTruncMode ? 1 : 0;
    }
    try
    {
      this.lQuit = pl.getIntParameter("l_quit");
    }
    catch (FormatException ex)
    {
    }
    catch (ArgumentException ex)
    {
    }
    this.in_Renamed = ehs;
    this.pktDec = new PktDecoder(decSpec, hd, ehs, (BitstreamReader) this, this.isTruncMode, maxCB);
    this.tileParts = new int[this.nt];
    this.totTileLen = new int[this.nt];
    this.tilePartLen = new int[this.nt][];
    this.tilePartNum = new int[this.nt][];
    this.firstPackOff = new int[this.nt][];
    this.tilePartsRead = new int[this.nt];
    this.totTileHeadLen = new int[this.nt];
    this.tilePartHeadLen = new int[this.nt][];
    this.nBytes = new int[this.nt];
    this.baknBytes = new int[this.nt];
    hd.nTileParts = new int[this.nt];
    int index1 = 0;
    int index2 = 0;
    int num2 = 0;
    int mainHeadOff = hd.mainHeadOff;
    this.mainHeadLen = this.in_Renamed.Pos - mainHeadOff;
    this.headLen = this.mainHeadLen;
    if (maxCB == -1)
      this.anbytes = this.mainHeadLen;
    else
      this.anbytes = 0;
    string str2 = $"{str1}Main header length    : {(object) mainHeadOff}, {(object) this.mainHeadLen}, {(object) this.mainHeadLen}\n";
    int anbytes = this.anbytes;
    int tnbytes = this.tnbytes;
    this.totAllTileLen = 0.0;
    this.remainingTileParts = this.nt;
    int nt = this.nt;
    try
    {
      while (this.remainingTileParts != 0)
      {
        int pos1 = this.in_Renamed.Pos;
        try
        {
          index1 = this.readTilePartHeader();
          if (!this.isEOCFound)
          {
            index2 = this.tilePartsRead[index1];
            if (this.isPsotEqualsZero)
              this.tilePartLen[index1][index2] = this.in_Renamed.length() - 2 - pos1;
          }
          else
            break;
        }
        catch (EndOfStreamException ex)
        {
          this.firstPackOff[index1][index2] = this.in_Renamed.length();
          throw ex;
        }
        int pos2 = this.in_Renamed.Pos;
        if (this.isTruncMode && maxCB == -1 && pos2 - mainHeadOff > this.tnbytes)
        {
          this.firstPackOff[index1][index2] = this.in_Renamed.length();
          break;
        }
        this.firstPackOff[index1][index2] = pos2;
        this.tilePartHeadLen[index1][index2] = pos2 - pos1;
        str2 = $"{str2}Tile-part {(object) index2} of tile {(object) index1} : {(object) pos1}, {(object) this.tilePartLen[index1][index2]}, {(object) this.tilePartHeadLen[index1][index2]}\n";
        this.totTileLen[index1] += this.tilePartLen[index1][index2];
        this.totTileHeadLen[index1] += this.tilePartHeadLen[index1][index2];
        this.totAllTileLen += (double) this.tilePartLen[index1][index2];
        if (this.isTruncMode)
        {
          if (this.anbytes + this.tilePartLen[index1][index2] > this.tnbytes)
          {
            this.anbytes += this.tilePartHeadLen[index1][index2];
            this.headLen += this.tilePartHeadLen[index1][index2];
            this.nBytes[index1] += this.tnbytes - this.anbytes;
            break;
          }
          this.anbytes += this.tilePartHeadLen[index1][index2];
          this.headLen += this.tilePartHeadLen[index1][index2];
          this.nBytes[index1] += this.tilePartLen[index1][index2] - this.tilePartHeadLen[index1][index2];
        }
        else if (this.anbytes + this.tilePartHeadLen[index1][index2] <= this.tnbytes)
        {
          this.anbytes += this.tilePartHeadLen[index1][index2];
          this.headLen += this.tilePartHeadLen[index1][index2];
        }
        else
          break;
        if (num2 == 0)
          this.firstTilePartHeadLen = this.tilePartHeadLen[index1][index2];
        ++this.tilePartsRead[index1];
        this.in_Renamed.seek(pos1 + this.tilePartLen[index1][index2]);
        --this.remainingTileParts;
        --nt;
        ++num2;
        if (this.isPsotEqualsZero)
          break;
      }
    }
    catch (EndOfStreamException ex1)
    {
      int num3 = this.in_Renamed.length();
      if (num3 < this.tnbytes)
      {
        this.tnbytes = num3;
        this.trate = (float) this.tnbytes * 8f / (float) hd.MaxCompImgWidth / (float) hd.MaxCompImgHeight;
      }
      if (!this.isTruncMode)
        this.allocateRate();
      if (pl.getParameter("res") == null)
      {
        this.targetRes = decSpec.dls.Min;
      }
      else
      {
        try
        {
          this.targetRes = pl.getIntParameter("res");
          if (this.targetRes < 0)
            throw new ArgumentException("Specified negative resolution level index: " + (object) this.targetRes);
        }
        catch (FormatException ex2)
        {
          throw new ArgumentException("Invalid resolution level index ('-res' option) " + pl.getParameter("res"));
        }
      }
      int min = decSpec.dls.Min;
      if (this.targetRes > min)
        this.targetRes = min;
      for (int index3 = 0; index3 < this.nt; ++index3)
        this.baknBytes[index3] = this.nBytes[index3];
      return;
    }
    this.remainingTileParts = 0;
    if (pl.getParameter("res") == null)
    {
      this.targetRes = decSpec.dls.Min;
    }
    else
    {
      try
      {
        this.targetRes = pl.getIntParameter("res");
        if (this.targetRes < 0)
          throw new ArgumentException("Specified negative resolution level index: " + (object) this.targetRes);
      }
      catch (FormatException ex)
      {
        throw new ArgumentException("Invalid resolution level index ('-res' option) " + pl.getParameter("res"));
      }
    }
    int min1 = decSpec.dls.Min;
    if (this.targetRes > min1)
      this.targetRes = min1;
    if (!this.isEOCFound)
    {
      int num4 = this.isPsotEqualsZero ? 1 : 0;
    }
    if (!this.isTruncMode)
      this.allocateRate();
    else if (this.in_Renamed.Pos >= this.tnbytes)
      this.anbytes += 2;
    for (int index4 = 0; index4 < this.nt; ++index4)
      this.baknBytes[index4] = this.nBytes[index4];
  }

  private void allocateRate()
  {
    int tnbytes = this.tnbytes;
    this.anbytes += 2;
    int anbytes = this.anbytes;
    int num1 = tnbytes - this.anbytes;
    int num2 = num1;
    for (int index = this.nt - 1; index > 0; --index)
      num1 -= this.nBytes[index] = (int) ((double) num2 * ((double) this.totTileLen[index] / this.totAllTileLen));
    this.nBytes[0] = num1;
  }

  private int readTilePartHeader()
  {
    HeaderInformation.SOT newSot = this.hi.NewSOT;
    switch (this.in_Renamed.readShort())
    {
      case -112:
        this.isEOCFound = false;
        int num1 = this.in_Renamed.readUnsignedShort();
        newSot.lsot = num1;
        if (num1 != 10)
          throw new Exception();
        int tileIdx = this.in_Renamed.readUnsignedShort();
        newSot.isot = tileIdx;
        if (tileIdx > 65534)
          throw new Exception();
        int num2 = this.in_Renamed.readInt();
        newSot.psot = num2;
        this.isPsotEqualsZero = num2 == 0;
        if (num2 < 0)
          throw new ArgumentException("Maximum tile length exceeded");
        int index1 = (int) this.in_Renamed.read();
        newSot.tpsot = index1;
        if (index1 != this.tilePartsRead[tileIdx] || index1 < 0 || index1 > 254)
          throw new Exception();
        int length = (int) this.in_Renamed.read();
        newSot.tnsot = length;
        this.hi.sotValue[(object) $"t{(object) tileIdx}_tp{(object) index1}"] = (object) newSot;
        if (length == 0)
        {
          int num3;
          if (this.tileParts[tileIdx] == 0 || this.tileParts[tileIdx] == this.tilePartLen.Length)
          {
            num3 = 2;
            ++this.remainingTileParts;
          }
          else
            num3 = 1;
          this.tileParts[tileIdx] += num3;
          length = this.tileParts[tileIdx];
          int[] numArray1 = this.tilePartLen[tileIdx];
          this.tilePartLen[tileIdx] = new int[length];
          for (int index2 = 0; index2 < length - num3; ++index2)
            this.tilePartLen[tileIdx][index2] = numArray1[index2];
          int[] numArray2 = this.tilePartNum[tileIdx];
          this.tilePartNum[tileIdx] = new int[length];
          for (int index3 = 0; index3 < length - num3; ++index3)
            this.tilePartNum[tileIdx][index3] = numArray2[index3];
          int[] numArray3 = this.firstPackOff[tileIdx];
          this.firstPackOff[tileIdx] = new int[length];
          for (int index4 = 0; index4 < length - num3; ++index4)
            this.firstPackOff[tileIdx][index4] = numArray3[index4];
          int[] numArray4 = this.tilePartHeadLen[tileIdx];
          this.tilePartHeadLen[tileIdx] = new int[length];
          for (int index5 = 0; index5 < length - num3; ++index5)
            this.tilePartHeadLen[tileIdx][index5] = numArray4[index5];
        }
        else if (this.tileParts[tileIdx] == 0)
        {
          this.remainingTileParts += length - 1;
          this.tileParts[tileIdx] = length;
          this.tilePartLen[tileIdx] = new int[length];
          this.tilePartNum[tileIdx] = new int[length];
          this.firstPackOff[tileIdx] = new int[length];
          this.tilePartHeadLen[tileIdx] = new int[length];
        }
        else
        {
          if (this.tileParts[tileIdx] > length)
            throw new Exception();
          this.remainingTileParts += length - this.tileParts[tileIdx];
          if (this.tileParts[tileIdx] != length)
          {
            int[] numArray5 = this.tilePartLen[tileIdx];
            this.tilePartLen[tileIdx] = new int[length];
            for (int index6 = 0; index6 < this.tileParts[tileIdx] - 1; ++index6)
              this.tilePartLen[tileIdx][index6] = numArray5[index6];
            int[] numArray6 = this.tilePartNum[tileIdx];
            this.tilePartNum[tileIdx] = new int[length];
            for (int index7 = 0; index7 < this.tileParts[tileIdx] - 1; ++index7)
              this.tilePartNum[tileIdx][index7] = numArray6[index7];
            int[] numArray7 = this.firstPackOff[tileIdx];
            this.firstPackOff[tileIdx] = new int[length];
            for (int index8 = 0; index8 < this.tileParts[tileIdx] - 1; ++index8)
              this.firstPackOff[tileIdx][index8] = numArray7[index8];
            int[] numArray8 = this.tilePartHeadLen[tileIdx];
            this.tilePartHeadLen[tileIdx] = new int[length];
            for (int index9 = 0; index9 < this.tileParts[tileIdx] - 1; ++index9)
              this.tilePartHeadLen[tileIdx][index9] = numArray8[index9];
          }
        }
        this.hd.resetHeaderMarkers();
        this.hd.nTileParts[tileIdx] = length;
        do
        {
          this.hd.extractTilePartMarkSeg(this.in_Renamed.readShort(), this.in_Renamed, tileIdx, index1);
        }
        while ((this.hd.NumFoundMarkSeg & 8192 /*0x2000*/) == 0);
        this.hd.readFoundTilePartMarkSeg(tileIdx, index1);
        this.tilePartLen[tileIdx][index1] = num2;
        this.tilePartNum[tileIdx][index1] = this.totTilePartsRead;
        ++this.totTilePartsRead;
        this.hd.TileOfTileParts = tileIdx;
        return tileIdx;
      case -39:
        this.isEOCFound = true;
        return -1;
      default:
        throw new Exception();
    }
  }

  private bool readLyResCompPos(int[][] lys, int lye, int ress, int rese, int comps, int compe)
  {
    int num1 = 10000;
    for (int index1 = comps; index1 < compe; ++index1)
    {
      if (index1 < this.mdl.Length)
      {
        for (int index2 = ress; index2 < rese; ++index2)
        {
          if (lys[index1] != null && index2 < lys[index1].Length && lys[index1][index2] < num1)
            num1 = lys[index1][index2];
        }
      }
    }
    int tileIdx = this.TileIdx;
    int num2 = this.firstPackOff[tileIdx][this.curTilePart] + this.tilePartLen[tileIdx][this.curTilePart] - 1 - this.tilePartHeadLen[tileIdx][this.curTilePart];
    int tileDef = (int) this.decSpec.nls.getTileDef(tileIdx);
    string str = $"Tile {(object) this.TileIdx} (tile-part:{(object) this.curTilePart}): offset, length, header length\n";
    bool flag1 = false;
    if ((bool) this.decSpec.pphs.getTileDef(tileIdx))
      flag1 = true;
    for (int l = num1; l < lye; ++l)
    {
      for (int r = ress; r < rese; ++r)
      {
        for (int c = comps; c < compe; ++c)
        {
          if (c < this.mdl.Length && r < lys[c].Length && r <= this.mdl[c] && l >= lys[c][r] && l < tileDef)
          {
            int numPrecinct = this.pktDec.getNumPrecinct(c, r);
            for (int p = 0; p < numPrecinct; ++p)
            {
              int pos = this.in_Renamed.Pos;
              if (flag1)
                this.pktDec.readPktHead(l, r, c, p, this.cbI[c][r], this.nBytes);
              if (pos > num2 && this.curTilePart < this.firstPackOff[tileIdx].Length - 1)
              {
                ++this.curTilePart;
                this.in_Renamed.seek(this.firstPackOff[tileIdx][this.curTilePart]);
                num2 = this.in_Renamed.Pos + this.tilePartLen[tileIdx][this.curTilePart] - 1 - this.tilePartHeadLen[tileIdx][this.curTilePart];
              }
              bool flag2 = this.pktDec.readSOPMarker(this.nBytes, p, c, r);
              if (flag2)
                return true;
              if (!flag1)
                flag2 = this.pktDec.readPktHead(l, r, c, p, this.cbI[c][r], this.nBytes);
              if (flag2)
                return true;
              int num3 = this.in_Renamed.Pos - pos;
              this.pktHL.Add((object) num3);
              bool flag3 = this.pktDec.readPktBody(l, r, c, p, this.cbI[c][r], this.nBytes);
              int num4 = this.in_Renamed.Pos - pos;
              str = $"{str} Pkt l={(object) l},r={(object) r},c={(object) c},p={(object) p}: {(object) pos}, {(object) num4}, {(object) num3}\n";
              if (flag3)
                return true;
            }
          }
        }
      }
    }
    return false;
  }

  private bool readResLyCompPos(int[][] lys, int lye, int ress, int rese, int comps, int compe)
  {
    int tileIdx = this.TileIdx;
    int num1 = this.firstPackOff[tileIdx][this.curTilePart] + this.tilePartLen[tileIdx][this.curTilePart] - 1 - this.tilePartHeadLen[tileIdx][this.curTilePart];
    int num2 = 10000;
    for (int index1 = comps; index1 < compe; ++index1)
    {
      if (index1 < this.mdl.Length)
      {
        for (int index2 = ress; index2 < rese; ++index2)
        {
          if (index2 <= this.mdl[index1] && lys[index1] != null && index2 < lys[index1].Length && lys[index1][index2] < num2)
            num2 = lys[index1][index2];
        }
      }
    }
    string str = $"Tile {(object) this.TileIdx} (tile-part:{(object) this.curTilePart}): offset, length, header length\n";
    int tileDef = (int) this.decSpec.nls.getTileDef(tileIdx);
    bool flag1 = false;
    if ((bool) this.decSpec.pphs.getTileDef(tileIdx))
      flag1 = true;
    for (int r = ress; r < rese; ++r)
    {
      for (int l = num2; l < lye; ++l)
      {
        for (int c = comps; c < compe; ++c)
        {
          if (c < this.mdl.Length && r <= this.mdl[c] && r < lys[c].Length && l >= lys[c][r] && l < tileDef)
          {
            int numPrecinct = this.pktDec.getNumPrecinct(c, r);
            for (int p = 0; p < numPrecinct; ++p)
            {
              int pos = this.in_Renamed.Pos;
              if (flag1)
                this.pktDec.readPktHead(l, r, c, p, this.cbI[c][r], this.nBytes);
              if (pos > num1 && this.curTilePart < this.firstPackOff[tileIdx].Length - 1)
              {
                ++this.curTilePart;
                this.in_Renamed.seek(this.firstPackOff[tileIdx][this.curTilePart]);
                num1 = this.in_Renamed.Pos + this.tilePartLen[tileIdx][this.curTilePart] - 1 - this.tilePartHeadLen[tileIdx][this.curTilePart];
              }
              bool flag2 = this.pktDec.readSOPMarker(this.nBytes, p, c, r);
              if (flag2)
                return true;
              if (!flag1)
                flag2 = this.pktDec.readPktHead(l, r, c, p, this.cbI[c][r], this.nBytes);
              if (flag2)
                return true;
              int num3 = this.in_Renamed.Pos - pos;
              this.pktHL.Add((object) num3);
              bool flag3 = this.pktDec.readPktBody(l, r, c, p, this.cbI[c][r], this.nBytes);
              int num4 = this.in_Renamed.Pos - pos;
              str = $"{str} Pkt l={(object) l},r={(object) r},c={(object) c},p={(object) p}: {(object) pos}, {(object) num4}, {(object) num3}\n";
              if (flag3)
                return true;
            }
          }
        }
      }
    }
    return false;
  }

  private bool readResPosCompLy(int[][] lys, int lye, int ress, int rese, int comps, int compe)
  {
    JPXImageCoordinates numTiles = this.getNumTiles((JPXImageCoordinates) null);
    JPXImageCoordinates tile = this.getTile((JPXImageCoordinates) null);
    int imgUlx = this.hd.ImgULX;
    int imgUly = this.hd.ImgULY;
    int num1 = imgUlx + this.hd.ImgWidth;
    int num2 = imgUly + this.hd.ImgHeight;
    int tilePartUlx = this.TilePartULX;
    int tilePartUly = this.TilePartULY;
    int nomTileWidth = this.NomTileWidth;
    int nomTileHeight = this.NomTileHeight;
    int num3 = tile.x == 0 ? imgUlx : tilePartUlx + tile.x * nomTileWidth;
    int num4 = tile.y == 0 ? imgUly : tilePartUly + tile.y * nomTileHeight;
    int num5 = tile.x != numTiles.x - 1 ? tilePartUlx + (tile.x + 1) * nomTileWidth : num1;
    int num6 = tile.y != numTiles.y - 1 ? tilePartUly + (tile.y + 1) * nomTileHeight : num2;
    int tileIdx = this.TileIdx;
    int x1_1 = 0;
    int x1_2 = 0;
    int num7 = 0;
    int[][] numArray = new int[compe][];
    int num8 = 100000;
    int num9 = num5;
    int num10 = num6;
    int num11 = num3;
    int num12 = num4;
    for (int c = comps; c < compe; ++c)
    {
      for (int r = ress; r < rese; ++r)
      {
        if (c < this.mdl.Length && r <= this.mdl[c])
        {
          numArray[c] = new int[this.mdl[c] + 1];
          if (lys[c] != null && r < lys[c].Length && lys[c][r] < num8)
            num8 = lys[c][r];
          for (int p = this.pktDec.getNumPrecinct(c, r) - 1; p >= 0; --p)
          {
            PrecInfo precInfo = this.pktDec.getPrecInfo(c, r, p);
            if (precInfo.rgulx != num3)
            {
              if (precInfo.rgulx < num9)
                num9 = precInfo.rgulx;
              if (precInfo.rgulx > num11)
                num11 = precInfo.rgulx;
            }
            if (precInfo.rguly != num4)
            {
              if (precInfo.rguly < num10)
                num10 = precInfo.rguly;
              if (precInfo.rguly > num12)
                num12 = precInfo.rguly;
            }
            if (num7 == 0)
            {
              x1_1 = precInfo.rgw;
              x1_2 = precInfo.rgh;
            }
            else
            {
              x1_1 = MathUtil.gcd(x1_1, precInfo.rgw);
              x1_2 = MathUtil.gcd(x1_2, precInfo.rgh);
            }
            ++num7;
          }
        }
      }
    }
    int num13 = (num12 - num10) / x1_2 + 1;
    int num14 = (num11 - num9) / x1_1 + 1;
    int num15 = this.firstPackOff[tileIdx][this.curTilePart] + this.tilePartLen[tileIdx][this.curTilePart] - 1 - this.tilePartHeadLen[tileIdx][this.curTilePart];
    int tileDef = (int) this.decSpec.nls.getTileDef(tileIdx);
    string str = $"Tile {(object) this.TileIdx} (tile-part:{(object) this.curTilePart}): offset, length, header length\n";
    bool flag1 = false;
    if ((bool) this.decSpec.pphs.getTileDef(tileIdx))
      flag1 = true;
    for (int r = ress; r < rese; ++r)
    {
      int num16 = num4;
      int num17 = num3;
      for (int index1 = 0; index1 <= num13; ++index1)
      {
        for (int index2 = 0; index2 <= num14; ++index2)
        {
          for (int c = comps; c < compe; ++c)
          {
            if (c < this.mdl.Length && r <= this.mdl[c] && numArray[c][r] < this.pktDec.getNumPrecinct(c, r))
            {
              PrecInfo precInfo = this.pktDec.getPrecInfo(c, r, numArray[c][r]);
              if (precInfo.rgulx == num17 && precInfo.rguly == num16)
              {
                for (int l = num8; l < lye; ++l)
                {
                  if (r < lys[c].Length && l >= lys[c][r] && l < tileDef)
                  {
                    int pos = this.in_Renamed.Pos;
                    if (flag1)
                      this.pktDec.readPktHead(l, r, c, numArray[c][r], this.cbI[c][r], this.nBytes);
                    if (pos > num15 && this.curTilePart < this.firstPackOff[tileIdx].Length - 1)
                    {
                      ++this.curTilePart;
                      this.in_Renamed.seek(this.firstPackOff[tileIdx][this.curTilePart]);
                      num15 = this.in_Renamed.Pos + this.tilePartLen[tileIdx][this.curTilePart] - 1 - this.tilePartHeadLen[tileIdx][this.curTilePart];
                    }
                    bool flag2 = this.pktDec.readSOPMarker(this.nBytes, numArray[c][r], c, r);
                    if (flag2)
                      return true;
                    if (!flag1)
                      flag2 = this.pktDec.readPktHead(l, r, c, numArray[c][r], this.cbI[c][r], this.nBytes);
                    if (flag2)
                      return true;
                    int num18 = this.in_Renamed.Pos - pos;
                    this.pktHL.Add((object) num18);
                    bool flag3 = this.pktDec.readPktBody(l, r, c, numArray[c][r], this.cbI[c][r], this.nBytes);
                    int num19 = this.in_Renamed.Pos - pos;
                    str = $"{str} Pkt l={(object) l},r={(object) r},c={(object) c},p={(object) numArray[c][r]}: {(object) pos}, {(object) num19}, {(object) num18}\n";
                    if (flag3)
                      return true;
                  }
                }
                ++numArray[c][r];
              }
            }
          }
          num17 = index2 == num14 ? num3 : num9 + index2 * x1_1;
        }
        num16 = index1 == num13 ? num4 : num10 + index1 * x1_2;
      }
    }
    return false;
  }

  private bool readPosCompResLy(int[][] lys, int lye, int ress, int rese, int comps, int compe)
  {
    JPXImageCoordinates numTiles = this.getNumTiles((JPXImageCoordinates) null);
    JPXImageCoordinates tile = this.getTile((JPXImageCoordinates) null);
    int imgUlx = this.hd.ImgULX;
    int imgUly = this.hd.ImgULY;
    int num1 = imgUlx + this.hd.ImgWidth;
    int num2 = imgUly + this.hd.ImgHeight;
    int tilePartUlx = this.TilePartULX;
    int tilePartUly = this.TilePartULY;
    int nomTileWidth = this.NomTileWidth;
    int nomTileHeight = this.NomTileHeight;
    int num3 = tile.x == 0 ? imgUlx : tilePartUlx + tile.x * nomTileWidth;
    int num4 = tile.y == 0 ? imgUly : tilePartUly + tile.y * nomTileHeight;
    int num5 = tile.x != numTiles.x - 1 ? tilePartUlx + (tile.x + 1) * nomTileWidth : num1;
    int num6 = tile.y != numTiles.y - 1 ? tilePartUly + (tile.y + 1) * nomTileHeight : num2;
    int tileIdx = this.TileIdx;
    int x1_1 = 0;
    int x1_2 = 0;
    int num7 = 0;
    int[][] numArray = new int[compe][];
    int num8 = 100000;
    int num9 = num5;
    int num10 = num6;
    int num11 = num3;
    int num12 = num4;
    for (int c = comps; c < compe; ++c)
    {
      for (int r = ress; r < rese; ++r)
      {
        if (c < this.mdl.Length && r <= this.mdl[c])
        {
          numArray[c] = new int[this.mdl[c] + 1];
          if (lys[c] != null && r < lys[c].Length && lys[c][r] < num8)
            num8 = lys[c][r];
          for (int p = this.pktDec.getNumPrecinct(c, r) - 1; p >= 0; --p)
          {
            PrecInfo precInfo = this.pktDec.getPrecInfo(c, r, p);
            if (precInfo.rgulx != num3)
            {
              if (precInfo.rgulx < num9)
                num9 = precInfo.rgulx;
              if (precInfo.rgulx > num11)
                num11 = precInfo.rgulx;
            }
            if (precInfo.rguly != num4)
            {
              if (precInfo.rguly < num10)
                num10 = precInfo.rguly;
              if (precInfo.rguly > num12)
                num12 = precInfo.rguly;
            }
            if (num7 == 0)
            {
              x1_1 = precInfo.rgw;
              x1_2 = precInfo.rgh;
            }
            else
            {
              x1_1 = MathUtil.gcd(x1_1, precInfo.rgw);
              x1_2 = MathUtil.gcd(x1_2, precInfo.rgh);
            }
            ++num7;
          }
        }
      }
    }
    int num13 = (num12 - num10) / x1_2 + 1;
    int num14 = (num11 - num9) / x1_1 + 1;
    int num15 = this.firstPackOff[tileIdx][this.curTilePart] + this.tilePartLen[tileIdx][this.curTilePart] - 1 - this.tilePartHeadLen[tileIdx][this.curTilePart];
    int tileDef = (int) this.decSpec.nls.getTileDef(tileIdx);
    string str = $"Tile {(object) this.TileIdx} (tile-part:{(object) this.curTilePart}): offset, length, header length\n";
    bool flag1 = false;
    if ((bool) this.decSpec.pphs.getTileDef(tileIdx))
      flag1 = true;
    int num16 = num4;
    int num17 = num3;
    for (int index1 = 0; index1 <= num13; ++index1)
    {
      for (int index2 = 0; index2 <= num14; ++index2)
      {
        for (int c = comps; c < compe; ++c)
        {
          if (c < this.mdl.Length)
          {
            for (int r = ress; r < rese; ++r)
            {
              if (r <= this.mdl[c] && numArray[c][r] < this.pktDec.getNumPrecinct(c, r))
              {
                PrecInfo precInfo = this.pktDec.getPrecInfo(c, r, numArray[c][r]);
                if (precInfo.rgulx == num17 && precInfo.rguly == num16)
                {
                  for (int l = num8; l < lye; ++l)
                  {
                    if (r < lys[c].Length && l >= lys[c][r] && l < tileDef)
                    {
                      int pos = this.in_Renamed.Pos;
                      if (flag1)
                        this.pktDec.readPktHead(l, r, c, numArray[c][r], this.cbI[c][r], this.nBytes);
                      if (pos > num15 && this.curTilePart < this.firstPackOff[tileIdx].Length - 1)
                      {
                        ++this.curTilePart;
                        this.in_Renamed.seek(this.firstPackOff[tileIdx][this.curTilePart]);
                        num15 = this.in_Renamed.Pos + this.tilePartLen[tileIdx][this.curTilePart] - 1 - this.tilePartHeadLen[tileIdx][this.curTilePart];
                      }
                      bool flag2 = this.pktDec.readSOPMarker(this.nBytes, numArray[c][r], c, r);
                      if (flag2)
                        return true;
                      if (!flag1)
                        flag2 = this.pktDec.readPktHead(l, r, c, numArray[c][r], this.cbI[c][r], this.nBytes);
                      if (flag2)
                        return true;
                      int num18 = this.in_Renamed.Pos - pos;
                      this.pktHL.Add((object) num18);
                      bool flag3 = this.pktDec.readPktBody(l, r, c, numArray[c][r], this.cbI[c][r], this.nBytes);
                      int num19 = this.in_Renamed.Pos - pos;
                      str = $"{str} Pkt l={(object) l},r={(object) r},c={(object) c},p={(object) numArray[c][r]}: {(object) pos}, {(object) num19}, {(object) num18}\n";
                      if (flag3)
                        return true;
                    }
                  }
                  ++numArray[c][r];
                }
              }
            }
          }
        }
        num17 = index2 == num14 ? num3 : num9 + index2 * x1_1;
      }
      num16 = index1 == num13 ? num4 : num10 + index1 * x1_2;
    }
    return false;
  }

  private bool readCompPosResLy(int[][] lys, int lye, int ress, int rese, int comps, int compe)
  {
    JPXImageCoordinates numTiles = this.getNumTiles((JPXImageCoordinates) null);
    JPXImageCoordinates tile = this.getTile((JPXImageCoordinates) null);
    int imgUlx = this.hd.ImgULX;
    int imgUly = this.hd.ImgULY;
    int num1 = imgUlx + this.hd.ImgWidth;
    int num2 = imgUly + this.hd.ImgHeight;
    int tilePartUlx = this.TilePartULX;
    int tilePartUly = this.TilePartULY;
    int nomTileWidth = this.NomTileWidth;
    int nomTileHeight = this.NomTileHeight;
    int num3 = tile.x == 0 ? imgUlx : tilePartUlx + tile.x * nomTileWidth;
    int num4 = tile.y == 0 ? imgUly : tilePartUly + tile.y * nomTileHeight;
    int num5 = tile.x != numTiles.x - 1 ? tilePartUlx + (tile.x + 1) * nomTileWidth : num1;
    int num6 = tile.y != numTiles.y - 1 ? tilePartUly + (tile.y + 1) * nomTileHeight : num2;
    int tileIdx = this.TileIdx;
    int x1_1 = 0;
    int x1_2 = 0;
    int num7 = 0;
    int[][] numArray = new int[compe][];
    int num8 = 100000;
    int num9 = num5;
    int num10 = num6;
    int num11 = num3;
    int num12 = num4;
    for (int c = comps; c < compe; ++c)
    {
      for (int r = ress; r < rese; ++r)
      {
        if (c < this.mdl.Length && r <= this.mdl[c])
        {
          numArray[c] = new int[this.mdl[c] + 1];
          if (lys[c] != null && r < lys[c].Length && lys[c][r] < num8)
            num8 = lys[c][r];
          for (int p = this.pktDec.getNumPrecinct(c, r) - 1; p >= 0; --p)
          {
            PrecInfo precInfo = this.pktDec.getPrecInfo(c, r, p);
            if (precInfo.rgulx != num3)
            {
              if (precInfo.rgulx < num9)
                num9 = precInfo.rgulx;
              if (precInfo.rgulx > num11)
                num11 = precInfo.rgulx;
            }
            if (precInfo.rguly != num4)
            {
              if (precInfo.rguly < num10)
                num10 = precInfo.rguly;
              if (precInfo.rguly > num12)
                num12 = precInfo.rguly;
            }
            if (num7 == 0)
            {
              x1_1 = precInfo.rgw;
              x1_2 = precInfo.rgh;
            }
            else
            {
              x1_1 = MathUtil.gcd(x1_1, precInfo.rgw);
              x1_2 = MathUtil.gcd(x1_2, precInfo.rgh);
            }
            ++num7;
          }
        }
      }
    }
    int num13 = (num12 - num10) / x1_2 + 1;
    int num14 = (num11 - num9) / x1_1 + 1;
    int num15 = this.firstPackOff[tileIdx][this.curTilePart] + this.tilePartLen[tileIdx][this.curTilePart] - 1 - this.tilePartHeadLen[tileIdx][this.curTilePart];
    int tileDef = (int) this.decSpec.nls.getTileDef(tileIdx);
    string str = $"Tile {(object) this.TileIdx} (tile-part:{(object) this.curTilePart}): offset, length, header length\n";
    bool flag1 = false;
    if ((bool) this.decSpec.pphs.getTileDef(tileIdx))
      flag1 = true;
    for (int c = comps; c < compe; ++c)
    {
      if (c < this.mdl.Length)
      {
        int num16 = num4;
        int num17 = num3;
        for (int index1 = 0; index1 <= num13; ++index1)
        {
          for (int index2 = 0; index2 <= num14; ++index2)
          {
            for (int r = ress; r < rese; ++r)
            {
              if (r <= this.mdl[c] && numArray[c][r] < this.pktDec.getNumPrecinct(c, r))
              {
                PrecInfo precInfo = this.pktDec.getPrecInfo(c, r, numArray[c][r]);
                if (precInfo.rgulx == num17 && precInfo.rguly == num16)
                {
                  for (int l = num8; l < lye; ++l)
                  {
                    if (r < lys[c].Length && l >= lys[c][r])
                    {
                      int pos = this.in_Renamed.Pos;
                      if (flag1)
                        this.pktDec.readPktHead(l, r, c, numArray[c][r], this.cbI[c][r], this.nBytes);
                      if (pos > num15 && this.curTilePart < this.firstPackOff[tileIdx].Length - 1)
                      {
                        ++this.curTilePart;
                        this.in_Renamed.seek(this.firstPackOff[tileIdx][this.curTilePart]);
                        num15 = this.in_Renamed.Pos + this.tilePartLen[tileIdx][this.curTilePart] - 1 - this.tilePartHeadLen[tileIdx][this.curTilePart];
                      }
                      bool flag2 = this.pktDec.readSOPMarker(this.nBytes, numArray[c][r], c, r);
                      if (flag2)
                        return true;
                      if (!flag1)
                        flag2 = this.pktDec.readPktHead(l, r, c, numArray[c][r], this.cbI[c][r], this.nBytes);
                      if (flag2)
                        return true;
                      int num18 = this.in_Renamed.Pos - pos;
                      this.pktHL.Add((object) num18);
                      bool flag3 = this.pktDec.readPktBody(l, r, c, numArray[c][r], this.cbI[c][r], this.nBytes);
                      int num19 = this.in_Renamed.Pos - pos;
                      str = $"{str} Pkt l={(object) l},r={(object) r},c={(object) c},p={(object) numArray[c][r]}: {(object) pos}, {(object) num19}, {(object) num18}\n";
                      if (flag3)
                        return true;
                    }
                  }
                  ++numArray[c][r];
                }
              }
            }
            num17 = index2 == num14 ? num3 : num9 + index2 * x1_1;
          }
          num16 = index1 == num13 ? num4 : num10 + index1 * x1_2;
        }
      }
    }
    return false;
  }

  private void readTilePkts(int t)
  {
    this.pktHL = new List<object>((IEnumerable<object>) new List<object>(10));
    int tileDef1 = (int) this.decSpec.nls.getTileDef(t);
    if ((bool) this.decSpec.pphs.getTileDef(t))
    {
      MemoryStream packedPktHead = this.hd.getPackedPktHead(t);
      this.cbI = this.pktDec.restart(this.nc, this.mdl, tileDef1, this.cbI, true, packedPktHead);
    }
    else
      this.cbI = this.pktDec.restart(this.nc, this.mdl, tileDef1, this.cbI, false, (MemoryStream) null);
    int[][] tileDef2 = (int[][]) this.decSpec.pcs.getTileDef(t);
    int length1 = tileDef2 == null ? 1 : tileDef2.Length;
    int[][] numArray1 = new int[length1][];
    for (int index = 0; index < length1; ++index)
      numArray1[index] = new int[6];
    int index1 = 0;
    numArray1[0][1] = 0;
    if (tileDef2 == null)
    {
      numArray1[index1][0] = (int) this.decSpec.pos.getTileDef(t);
      numArray1[index1][1] = tileDef1;
      numArray1[index1][2] = 0;
      numArray1[index1][3] = this.decSpec.dls.getMaxInTile(t) + 1;
      numArray1[index1][4] = 0;
      numArray1[index1][5] = this.nc;
    }
    else
    {
      for (int index2 = 0; index2 < length1; ++index2)
      {
        numArray1[index2][0] = tileDef2[index2][5];
        numArray1[index2][1] = tileDef2[index2][2];
        numArray1[index2][2] = tileDef2[index2][0];
        numArray1[index2][3] = tileDef2[index2][3];
        numArray1[index2][4] = tileDef2[index2][1];
        numArray1[index2][5] = tileDef2[index2][4];
      }
    }
    try
    {
      if (this.isTruncMode && this.firstPackOff == null || this.firstPackOff[t] == null)
        return;
      this.in_Renamed.seek(this.firstPackOff[t][0]);
    }
    catch (EndOfStreamException ex)
    {
      return;
    }
    this.curTilePart = 0;
    bool flag1 = false;
    int nByte = this.nBytes[t];
    int[][] lys = new int[this.nc][];
    for (int c = 0; c < this.nc; ++c)
      lys[c] = new int[(int) this.decSpec.dls.getTileCompVal(t, c) + 1];
    try
    {
      for (int index3 = 0; index3 < length1; ++index3)
      {
        int lye = numArray1[index3][1];
        int ress = numArray1[index3][2];
        int rese = numArray1[index3][3];
        int comps = numArray1[index3][4];
        int compe = numArray1[index3][5];
        switch (numArray1[index3][0])
        {
          case 0:
            flag1 = this.readLyResCompPos(lys, lye, ress, rese, comps, compe);
            break;
          case 1:
            flag1 = this.readResLyCompPos(lys, lye, ress, rese, comps, compe);
            break;
          case 2:
            flag1 = this.readResPosCompLy(lys, lye, ress, rese, comps, compe);
            break;
          case 3:
            flag1 = this.readPosCompResLy(lys, lye, ress, rese, comps, compe);
            break;
          case 4:
            flag1 = this.readCompPosResLy(lys, lye, ress, rese, comps, compe);
            break;
          default:
            throw new ArgumentException("Not recognized progression type");
        }
        for (int index4 = comps; index4 < compe; ++index4)
        {
          if (index4 < lys.Length)
          {
            for (int index5 = ress; index5 < rese; ++index5)
            {
              if (index5 < lys[index4].Length)
                lys[index4][index5] = lye;
            }
          }
        }
        if (!flag1)
        {
          if (this.usePOCQuit)
            break;
        }
        else
          break;
      }
    }
    catch (EndOfStreamException ex)
    {
      throw ex;
    }
    if (this.isTruncMode)
    {
      this.anbytes += nByte - this.nBytes[t];
      if (!flag1)
        return;
      this.nBytes[t] = 0;
    }
    else if (this.nBytes[t] < this.totTileLen[t] - this.totTileHeadLen[t])
    {
      bool flag2 = false;
      int[] numArray2 = new int[this.pktHL.Count];
      for (int index6 = this.pktHL.Count - 1; index6 >= 0; --index6)
        numArray2[index6] = (int) this.pktHL[index6];
      bool flag3 = false;
      for (int index7 = 0; index7 < tileDef1; ++index7)
      {
        if (this.cbI != null)
        {
          int length2 = this.cbI.Length;
          int num1 = 0;
          for (int index8 = 0; index8 < length2; ++index8)
          {
            if (this.cbI[index8] != null && this.cbI[index8].Length > num1)
              num1 = this.cbI[index8].Length;
          }
          for (int index9 = 0; index9 < num1; ++index9)
          {
            int num2 = 0;
            for (int index10 = 0; index10 < length2; ++index10)
            {
              if (this.cbI[index10] != null && this.cbI[index10][index9] != null && this.cbI[index10][index9].Length > num2)
                num2 = this.cbI[index10][index9].Length;
            }
            for (int index11 = 0; index11 < num2; ++index11)
            {
              if ((index9 != 0 || index11 == 0) && (index9 == 0 || index11 != 0))
              {
                int num3 = 0;
                for (int index12 = 0; index12 < length2; ++index12)
                {
                  if (this.cbI[index12] != null && this.cbI[index12][index9] != null && this.cbI[index12][index9][index11] != null && this.cbI[index12][index9][index11].Length > num3)
                    num3 = this.cbI[index12][index9][index11].Length;
                }
                for (int index13 = 0; index13 < num3; ++index13)
                {
                  int num4 = 0;
                  for (int index14 = 0; index14 < length2; ++index14)
                  {
                    if (this.cbI[index14] != null && this.cbI[index14][index9] != null && this.cbI[index14][index9][index11] != null && this.cbI[index14][index9][index11][index13] != null && this.cbI[index14][index9][index11][index13].Length > num4)
                      num4 = this.cbI[index14][index9][index11][index13].Length;
                  }
                  for (int index15 = 0; index15 < num4; ++index15)
                  {
                    for (int index16 = 0; index16 < length2; ++index16)
                    {
                      if (this.cbI[index16] != null && this.cbI[index16][index9] != null && this.cbI[index16][index9][index11] != null && this.cbI[index16][index9][index11][index13] != null && this.cbI[index16][index9][index11][index13][index15] != null)
                      {
                        Syncfusion.Pdf.CBlkInfo cblkInfo = this.cbI[index16][index9][index11][index13][index15];
                        if (!flag3)
                        {
                          if (this.nBytes[t] < numArray2[cblkInfo.pktIdx[index7]])
                          {
                            flag2 = true;
                            flag3 = true;
                          }
                          else if (!flag2)
                          {
                            this.nBytes[t] -= numArray2[cblkInfo.pktIdx[index7]];
                            this.anbytes += numArray2[cblkInfo.pktIdx[index7]];
                            numArray2[cblkInfo.pktIdx[index7]] = 0;
                          }
                        }
                        if (cblkInfo.len[index7] != 0)
                        {
                          if (cblkInfo.len[index7] < this.nBytes[t] && !flag3)
                          {
                            this.nBytes[t] -= cblkInfo.len[index7];
                            this.anbytes += cblkInfo.len[index7];
                          }
                          else
                          {
                            cblkInfo.len[index7] = cblkInfo.off[index7] = cblkInfo.ntp[index7] = 0;
                            flag3 = true;
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
    else
    {
      this.anbytes += this.totTileLen[t] - this.totTileHeadLen[t];
      if (t >= this.getNumTiles() - 1)
        return;
      this.nBytes[t + 1] += this.nBytes[t] - (this.totTileLen[t] - this.totTileHeadLen[t]);
    }
  }

  public override void setTile(int x, int y)
  {
    if (x < 0 || y < 0 || x >= this.ntX || y >= this.ntY)
      throw new ArgumentException();
    int t = y * this.ntX + x;
    if (t == 0)
    {
      this.anbytes = this.headLen;
      if (!this.isTruncMode)
        this.anbytes += 2;
      for (int index = 0; index < this.nt; ++index)
        this.nBytes[index] = this.baknBytes[index];
    }
    this.ctX = x;
    this.ctY = y;
    int num1 = x == 0 ? this.ax : this.px + x * this.ntW;
    int num2 = y == 0 ? this.ay : this.py + y * this.ntH;
    for (int c = this.nc - 1; c >= 0; --c)
    {
      this.culx[c] = (num1 + this.hd.getCompSubsX(c) - 1) / this.hd.getCompSubsX(c);
      this.culy[c] = (num2 + this.hd.getCompSubsY(c) - 1) / this.hd.getCompSubsY(c);
      this.offX[c] = (this.px + x * this.ntW + this.hd.getCompSubsX(c) - 1) / this.hd.getCompSubsX(c);
      this.offY[c] = (this.py + y * this.ntH + this.hd.getCompSubsY(c) - 1) / this.hd.getCompSubsY(c);
    }
    this.subbTrees = new SubbandSyn[this.nc];
    this.mdl = new int[this.nc];
    this.derived = new bool[this.nc];
    this.params_Renamed = new StdDequantizerParams[this.nc];
    this.gb = new int[this.nc];
    for (int c = 0; c < this.nc; ++c)
    {
      this.derived[c] = this.decSpec.qts.isDerived(t, c);
      this.params_Renamed[c] = (StdDequantizerParams) this.decSpec.qsss.getTileCompVal(t, c);
      this.gb[c] = (int) this.decSpec.gbs.getTileCompVal(t, c);
      this.mdl[c] = (int) this.decSpec.dls.getTileCompVal(t, c);
      this.subbTrees[c] = new SubbandSyn(this.getTileCompWidth(t, c, this.mdl[c]), this.getTileCompHeight(t, c, this.mdl[c]), this.getResULX(c, this.mdl[c]), this.getResULY(c, this.mdl[c]), this.mdl[c], (WaveletFilter[]) this.decSpec.wfs.getHFilters(t, c), (WaveletFilter[]) this.decSpec.wfs.getVFilters(t, c));
      this.initSubbandsFields(c, this.subbTrees[c]);
    }
    try
    {
      this.readTilePkts(t);
    }
    catch (IOException ex)
    {
    }
  }

  public override void nextTile()
  {
    if (this.ctX == this.ntX - 1 && this.ctY == this.ntY - 1)
      throw new Exception();
    if (this.ctX < this.ntX - 1)
      this.setTile(this.ctX + 1, this.ctY);
    else
      this.setTile(0, this.ctY + 1);
  }

  public override DecLyrdCBlk getCodeBlock(
    int c,
    int m,
    int n,
    SubbandSyn sb,
    int fl,
    int nl,
    DecLyrdCBlk ccb)
  {
    int tileIdx = this.TileIdx;
    int resLvl = sb.resLvl;
    int sbandIdx = sb.sbandIdx;
    int tileDef = (int) this.decSpec.nls.getTileDef(tileIdx);
    int tileCompVal = (int) this.decSpec.ecopts.getTileCompVal(tileIdx, c);
    if (nl < 0)
      nl = tileDef - fl + 1;
    if (this.lQuit != -1 && fl + nl > this.lQuit)
      nl = this.lQuit - fl;
    int num1 = this.targetRes + this.getSynSubbandTree(tileIdx, c).resLvl - this.decSpec.dls.Min;
    Syncfusion.Pdf.CBlkInfo cblkInfo;
    try
    {
      cblkInfo = this.cbI[c][resLvl][sbandIdx][m][n];
      if (fl >= 1 && fl <= tileDef)
      {
        if (fl + nl - 1 <= tileDef)
          goto label_10;
      }
      throw new ArgumentException();
    }
    catch (IndexOutOfRangeException ex)
    {
      throw new ArgumentException($"Code-block (t:{(object) tileIdx}, c:{(object) c}, r:{(object) resLvl}, s:{(object) sbandIdx}, {(object) m}x{(object) n}) not found in codestream");
    }
    catch (NullReferenceException ex)
    {
      throw new ArgumentException($"Code-block (t:{(object) tileIdx}, c:{(object) c}, r:{(object) resLvl}, s:{(object) sbandIdx}, {(object) m}x{(object) n}) not found in bit stream");
    }
label_10:
    if (ccb == null)
      ccb = new DecLyrdCBlk();
    ccb.m = m;
    ccb.n = n;
    ccb.nl = 0;
    ccb.dl = 0;
    ccb.nTrunc = 0;
    if (cblkInfo == null)
    {
      ccb.skipMSBP = 0;
      ccb.prog = false;
      ccb.w = ccb.h = ccb.ulx = ccb.uly = 0;
      return ccb;
    }
    ccb.skipMSBP = cblkInfo.msbSkipped;
    ccb.ulx = cblkInfo.ulx;
    ccb.uly = cblkInfo.uly;
    ccb.w = cblkInfo.w;
    ccb.h = cblkInfo.h;
    ccb.ftpIdx = 0;
    for (int index = 0; index < cblkInfo.len.Length && cblkInfo.len[index] == 0; ++index)
      ccb.ftpIdx += cblkInfo.ntp[index];
    for (int index = fl - 1; index < fl + nl - 1; ++index)
    {
      ++ccb.nl;
      ccb.dl += cblkInfo.len[index];
      ccb.nTrunc += cblkInfo.ntp[index];
    }
    int length;
    if ((tileCompVal & StdEntropyCoderOptions.OPT_TERM_PASS) != 0)
      length = ccb.nTrunc - ccb.ftpIdx;
    else if ((tileCompVal & StdEntropyCoderOptions.OPT_BYPASS) != 0)
    {
      if (ccb.nTrunc <= StdEntropyCoderOptions.FIRST_BYPASS_PASS_IDX)
      {
        length = 1;
      }
      else
      {
        length = 1;
        for (int ftpIdx = ccb.ftpIdx; ftpIdx < ccb.nTrunc; ++ftpIdx)
        {
          if (ftpIdx >= StdEntropyCoderOptions.FIRST_BYPASS_PASS_IDX - 1)
          {
            switch ((ftpIdx + StdEntropyCoderOptions.NUM_EMPTY_PASSES_IN_MS_BP) % StdEntropyCoderOptions.NUM_PASSES)
            {
              case 1:
              case 2:
                ++length;
                continue;
              default:
                continue;
            }
          }
        }
      }
    }
    else
      length = 1;
    if (ccb.data == null || ccb.data.Length < ccb.dl)
      ccb.data = new byte[ccb.dl];
    if (length > 1 && (ccb.tsLengths == null || ccb.tsLengths.Length < length))
      ccb.tsLengths = new int[length];
    else if (length > 1 && (tileCompVal & (StdEntropyCoderOptions.OPT_BYPASS | StdEntropyCoderOptions.OPT_TERM_PASS)) == StdEntropyCoderOptions.OPT_BYPASS)
      ArrayUtil.intArraySet(ccb.tsLengths, 0);
    int num2 = -1;
    int ftpIdx1 = ccb.ftpIdx;
    int ftpIdx2 = ccb.ftpIdx;
    int index1 = 0;
    for (int index2 = fl - 1; index2 < fl + nl - 1; ++index2)
    {
      ftpIdx2 += cblkInfo.ntp[index2];
      if (cblkInfo.len[index2] != 0)
      {
        try
        {
          this.in_Renamed.seek(cblkInfo.off[index2]);
          this.in_Renamed.readFully(ccb.data, num2 + 1, cblkInfo.len[index2]);
          num2 += cblkInfo.len[index2];
        }
        catch (IOException ex)
        {
        }
        if (length != 1)
        {
          if ((tileCompVal & StdEntropyCoderOptions.OPT_TERM_PASS) != 0)
          {
            int index3 = 0;
            for (; ftpIdx1 < ftpIdx2; ++ftpIdx1)
            {
              ccb.tsLengths[index1++] = cblkInfo.segLen[index2] == null ? cblkInfo.len[index2] : cblkInfo.segLen[index2][index3];
              ++index3;
            }
          }
          else
          {
            int index4 = 0;
            for (; ftpIdx1 < ftpIdx2; ++ftpIdx1)
            {
              if (ftpIdx1 >= StdEntropyCoderOptions.FIRST_BYPASS_PASS_IDX - 1 && (ftpIdx1 + StdEntropyCoderOptions.NUM_EMPTY_PASSES_IN_MS_BP) % StdEntropyCoderOptions.NUM_PASSES != 0)
              {
                if (cblkInfo.segLen[index2] != null)
                {
                  ccb.tsLengths[index1++] += cblkInfo.segLen[index2][index4++];
                  cblkInfo.len[index2] -= cblkInfo.segLen[index2][index4 - 1];
                }
                else
                {
                  ccb.tsLengths[index1++] += cblkInfo.len[index2];
                  cblkInfo.len[index2] = 0;
                }
              }
            }
            if (cblkInfo.segLen[index2] != null && index4 < cblkInfo.segLen[index2].Length)
            {
              ccb.tsLengths[index1] += cblkInfo.segLen[index2][index4];
              cblkInfo.len[index2] -= cblkInfo.segLen[index2][index4];
            }
            else if (index1 < length)
            {
              ccb.tsLengths[index1] += cblkInfo.len[index2];
              cblkInfo.len[index2] = 0;
            }
          }
        }
      }
    }
    if (length == 1 && ccb.tsLengths != null)
      ccb.tsLengths[0] = ccb.dl;
    int num3 = fl + nl - 1;
    if (num3 < tileDef - 1)
    {
      for (int index5 = num3 + 1; index5 < tileDef; ++index5)
      {
        if (cblkInfo.len[index5] != 0)
          ccb.prog = true;
      }
    }
    return ccb;
  }
}
