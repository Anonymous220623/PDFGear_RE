// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.JPXImage
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

#nullable disable
namespace Syncfusion.Pdf;

internal class JPXImage
{
  private string[][] decoder_pinfo = new string[20][]
  {
    new string[4]{ "u", "[on|off]", "", "off" },
    new string[4]{ "v", "[on|off]", "", "off" },
    new string[4]{ "verbose", "[on|off]", "", "on" },
    new string[4]{ "pfile", "", "   ", null },
    new string[4]{ "res", "", " ", null },
    new string[4]{ "i", "", "", null },
    new string[4]{ "o", "", " ", null },
    new string[4]{ "rate", "", " ", "-1" },
    new string[4]{ "nbytes", "", " ", "-1" },
    new string[4]{ "parsing", null, "true, ", "on" },
    new string[4]{ "ncb_quit", "", "", "-1" },
    new string[4]{ "l_quit", "", "", "-1" },
    new string[4]{ "m_quit", "", "", "-1" },
    new string[4]{ "poc_quit", null, "", "off" },
    new string[4]{ "one_tp", null, "", "off" },
    new string[4]{ "comp_transf", null, "", "on" },
    new string[4]{ "debug", null, "", "off" },
    new string[4]{ "cdstr_info", null, "", "off" },
    new string[4]{ "nocolorspace", null, "", "off" },
    new string[4]{ "", null, "", "off" }
  };

  internal Image FromStream(Stream stream)
  {
    JPXRandomAccessStream randomAccessStream = new JPXRandomAccessStream(stream, 262144 /*0x040000*/, 262144 /*0x040000*/, int.MaxValue);
    JPXParameters pl = new JPXParameters(this.GetParameters(this.decoder_pinfo));
    JPXFormatReader jpxFormatReader = new JPXFormatReader(randomAccessStream);
    jpxFormatReader.readFileFormat();
    if (jpxFormatReader.JP2FFUsed)
      randomAccessStream.seek(jpxFormatReader.FirstCodeStreamPos);
    HeaderInformation hi = new HeaderInformation();
    HeaderDecoder hd = new HeaderDecoder(randomAccessStream, pl, hi);
    int numComps1 = hd.NumComps;
    int numTiles1 = hi.sizValue.NumTiles;
    DecodeHelper decoderHelper = hd.DecoderHelper;
    int[] numArray1 = new int[numComps1];
    for (int c = 0; c < numComps1; ++c)
      numArray1[c] = hd.GetActualBitDepth(c);
    BitstreamReader instance1 = BitstreamReader.createInstance(randomAccessStream, hd, pl, decoderHelper, false, hi);
    EntropyDecoder entropyDecoder = hd.createEntropyDecoder((CodedCBlkDataSrcDec) instance1, pl);
    DeScalerROI roiDeScaler = hd.createROIDeScaler((CBlkQuantDataSrcDec) entropyDecoder, pl, decoderHelper);
    WaveletTransformInverse instance2 = WaveletTransformInverse.createInstance((CBlkWTDataSrcDec) hd.createDequantizer((CBlkQuantDataSrcDec) roiDeScaler, numArray1, decoderHelper), decoderHelper);
    int imgRes = instance1.ImgRes;
    instance2.ImgResLevel = imgRes;
    BlockImageDataSource blockImageDataSource = (BlockImageDataSource) new InverseComponetTransformation((BlockImageDataSource) new ImageDataConverter((BlockImageDataSource) instance2, 0), decoderHelper, numArray1, pl);
    int numComps2 = blockImageDataSource.NumComps;
    int num1 = numComps2 == 4 ? 4 : 3;
    PixelFormat format;
    switch (numComps2)
    {
      case 1:
        format = PixelFormat.Format24bppRgb;
        break;
      case 3:
        format = PixelFormat.Format24bppRgb;
        break;
      case 4:
        format = PixelFormat.Format32bppArgb;
        break;
      default:
        throw new ApplicationException($"Unsupported PixelFormat.  {(object) numComps2} components.");
    }
    Bitmap bitmap = new Bitmap(blockImageDataSource.ImgWidth, blockImageDataSource.ImgHeight, format);
    int imgWidth = blockImageDataSource.ImgWidth;
    JPXImageCoordinates numTiles2 = blockImageDataSource.getNumTiles((JPXImageCoordinates) null);
    int t = 0;
    for (int y = 0; y < numTiles2.y; ++y)
    {
      int x1 = 0;
      while (x1 < numTiles2.x)
      {
        blockImageDataSource.setTile(x1, y);
        int tileComponentHeight = blockImageDataSource.getTileComponentHeight(t, 0);
        int tileComponentWidth = blockImageDataSource.getTileComponentWidth(t, 0);
        int x2 = blockImageDataSource.getCompUpperLeftCornerX(0) - (int) Math.Ceiling((double) blockImageDataSource.ImgULX / (double) blockImageDataSource.getCompSubsX(0));
        int num2 = blockImageDataSource.getCompUpperLeftCornerY(0) - (int) Math.Ceiling((double) blockImageDataSource.ImgULY / (double) blockImageDataSource.getCompSubsY(0));
        DataBlockInt[] dataBlockIntArray = new DataBlockInt[numComps2];
        int[] numArray2 = new int[numComps2];
        int[] numArray3 = new int[numComps2];
        int[] numArray4 = new int[numComps2];
        for (int index = 0; index < numComps2; ++index)
        {
          dataBlockIntArray[index] = new DataBlockInt();
          numArray2[index] = 1 << blockImageDataSource.getNomRangeBits(0) - 1;
          numArray3[index] = (1 << blockImageDataSource.getNomRangeBits(0)) - 1;
          numArray4[index] = blockImageDataSource.getFixedPoint(0);
        }
        for (int index1 = 0; index1 < tileComponentHeight; ++index1)
        {
          for (int c = numComps2 - 1; c >= 0; --c)
          {
            dataBlockIntArray[c].ulx = 0;
            dataBlockIntArray[c].uly = index1;
            dataBlockIntArray[c].w = tileComponentWidth;
            dataBlockIntArray[c].h = 1;
            blockImageDataSource.getInternCompData((DataBlock) dataBlockIntArray[c], c);
          }
          int[] numArray5 = new int[numComps2];
          for (int index2 = numComps2 - 1; index2 >= 0; --index2)
            numArray5[index2] = dataBlockIntArray[index2].offset + tileComponentWidth - 1;
          byte[] source = new byte[tileComponentWidth * num1];
          for (int index3 = tileComponentWidth - 1; index3 >= 0; --index3)
          {
            int[] numArray6 = new int[numComps2];
            for (int c = numComps2 - 1; c >= 0; --c)
            {
              numArray6[c] = (dataBlockIntArray[c].data_array[numArray5[c]--] >> numArray4[c]) + numArray2[c];
              numArray6[c] = numArray6[c] < 0 ? 0 : (numArray6[c] > numArray3[c] ? numArray3[c] : numArray6[c]);
              if (blockImageDataSource.getNomRangeBits(c) != 8)
                numArray6[c] = (int) Math.Round((double) numArray6[c] / Math.Pow(2.0, (double) blockImageDataSource.getNomRangeBits(c)) * (double) byte.MaxValue);
            }
            int index4 = index3 * num1;
            switch (numComps2)
            {
              case 1:
                source[index4] = (byte) numArray6[0];
                source[index4 + 1] = (byte) numArray6[0];
                source[index4 + 2] = (byte) numArray6[0];
                break;
              case 3:
                source[index4] = (byte) numArray6[2];
                source[index4 + 1] = (byte) numArray6[1];
                source[index4 + 2] = (byte) numArray6[0];
                break;
              case 4:
                double maxValue = (double) byte.MaxValue;
                double num3 = (double) numArray6[0] / maxValue;
                double num4 = (double) numArray6[1] / maxValue;
                double num5 = (double) numArray6[2] / maxValue;
                double num6 = (double) numArray6[3] / maxValue;
                double num7 = 0.0;
                double num8 = 0.0;
                double num9 = 0.0;
                double num10 = -1.0;
                double num11 = -1.12;
                double num12 = -1.12;
                double num13 = -1.21;
                if (num10 != num3 || num11 != num4 || num12 != num5 || num13 != num6)
                {
                  double num14 = num3;
                  double num15 = num4;
                  double num16 = num5;
                  double num17 = num6;
                  num7 = (double) byte.MaxValue * (1.0 - num14) * (1.0 - num17);
                  num8 = (double) byte.MaxValue * (1.0 - num15) * (1.0 - num17);
                  num9 = (double) byte.MaxValue * (1.0 - num16) * (1.0 - num17);
                }
                double num18 = num7 > (double) byte.MaxValue ? (double) byte.MaxValue : (num7 < 0.0 ? 0.0 : num7);
                double num19 = num8 > (double) byte.MaxValue ? (double) byte.MaxValue : (num8 < 0.0 ? 0.0 : num8);
                double num20 = num9 > (double) byte.MaxValue ? (double) byte.MaxValue : (num9 < 0.0 ? 0.0 : num9);
                source[index4 + 2] = (byte) num18;
                source[index4 + 1] = (byte) num19;
                source[index4] = (byte) num20;
                break;
            }
          }
          BitmapData bitmapdata = bitmap.LockBits(new Rectangle(x2, num2 + index1, tileComponentWidth, 1), ImageLockMode.ReadWrite, format);
          IntPtr scan0 = bitmapdata.Scan0;
          Marshal.Copy(source, 0, scan0, source.Length);
          bitmap.UnlockBits(bitmapdata);
        }
        ++x1;
        ++t;
      }
    }
    return (Image) bitmap;
  }

  internal JPXParameters GetParameters(string[][] pinfo)
  {
    JPXParameters parameters = new JPXParameters();
    string[][] parameterInfo1 = BitstreamReader.ParameterInfo;
    if (parameterInfo1 != null)
    {
      for (int index = parameterInfo1.Length - 1; index >= 0; --index)
        parameters.Add(parameterInfo1[index][0], parameterInfo1[index][3]);
    }
    string[][] parameterInfo2 = EntropyDecoder.ParameterInfo;
    if (parameterInfo2 != null)
    {
      for (int index = parameterInfo2.Length - 1; index >= 0; --index)
        parameters.Add(parameterInfo2[index][0], parameterInfo2[index][3]);
    }
    string[][] parameterInfo3 = DeScalerROI.ParameterInfo;
    if (parameterInfo3 != null)
    {
      for (int index = parameterInfo3.Length - 1; index >= 0; --index)
        parameters.Add(parameterInfo3[index][0], parameterInfo3[index][3]);
    }
    string[][] parameterInfo4 = Dequantizer.ParameterInfo;
    if (parameterInfo4 != null)
    {
      for (int index = parameterInfo4.Length - 1; index >= 0; --index)
        parameters.Add(parameterInfo4[index][0], parameterInfo4[index][3]);
    }
    string[][] parameterInfo5 = InverseComponetTransformation.ParameterInfo;
    if (parameterInfo5 != null)
    {
      for (int index = parameterInfo5.Length - 1; index >= 0; --index)
        parameters.Add(parameterInfo5[index][0], parameterInfo5[index][3]);
    }
    string[][] parameterInfo6 = HeaderDecoder.ParameterInfo;
    if (parameterInfo6 != null)
    {
      for (int index = parameterInfo6.Length - 1; index >= 0; --index)
        parameters.Add(parameterInfo6[index][0], parameterInfo6[index][3]);
    }
    string[][] strArray = pinfo;
    if (strArray != null)
    {
      for (int index = strArray.Length - 1; index >= 0; --index)
        parameters.Add(strArray[index][0], strArray[index][3]);
    }
    return parameters;
  }
}
