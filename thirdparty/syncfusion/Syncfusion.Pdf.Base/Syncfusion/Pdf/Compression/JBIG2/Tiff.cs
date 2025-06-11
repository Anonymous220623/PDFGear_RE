// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.JBIG2.Tiff
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Compression.JBIG2.Internal;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Compression.JBIG2;

internal class Tiff : IDisposable
{
  private const int TIFF_VERSION = 42;
  private const int TIFF_BIGTIFF_VERSION = 43;
  private const short TIFF_BIGENDIAN = 19789;
  private const short TIFF_LITTLEENDIAN = 18761;
  private const short MDI_LITTLEENDIAN = 20549;
  private const float D50_X0 = 96.425f;
  private const float D50_Y0 = 100f;
  private const float D50_Z0 = 82.468f;
  internal const int STRIP_SIZE_DEFAULT = 8192 /*0x2000*/;
  internal const TiffFlags STRIPCHOP_DEFAULT = TiffFlags.STRIPCHOP;
  internal const bool DEFAULT_EXTRASAMPLE_AS_ALPHA = true;
  internal const bool CHECK_JPEG_YCBCR_SUBSAMPLING = true;
  private const int NOSTRIP = -1;
  private const int NOTILE = -1;
  internal const int O_RDONLY = 0;
  internal const int O_WRONLY = 1;
  internal const int O_CREAT = 256 /*0x0100*/;
  internal const int O_TRUNC = 512 /*0x0200*/;
  internal const int O_RDWR = 2;
  internal static readonly Encoding Latin1Encoding = Encoding.GetEncoding("Latin1");
  internal string m_name;
  internal int m_mode;
  internal TiffFlags m_flags;
  internal uint m_diroff;
  internal TiffDirectory m_dir;
  internal int m_row;
  internal int m_curstrip;
  internal int m_curtile;
  internal int m_tilesize;
  internal TiffCodec m_currentCodec;
  internal int m_scanlinesize;
  internal byte[] m_rawdata;
  internal int m_rawdatasize;
  internal int m_rawcp;
  internal int m_rawcc;
  internal object m_clientdata;
  internal Tiff.PostDecodeMethodType m_postDecodeMethod;
  internal TiffTagMethods m_tagmethods;
  private uint m_nextdiroff;
  private uint[] m_dirlist;
  private int m_dirlistsize;
  private short m_dirnumber;
  private Syncfusion.Pdf.Compression.JBIG2.Internal.TiffHeader m_header;
  private int[] m_typeshift;
  private uint[] m_typemask;
  private short m_curdir;
  private uint m_curoff;
  private uint m_dataoff;
  private short m_nsubifd;
  private uint m_subifdoff;
  private int m_col;
  private bool m_decodestatus;
  private TiffFieldInfo[] m_fieldinfo;
  private int m_nfields;
  private TiffFieldInfo m_foundfield;
  private Tiff.clientInfoLink m_clientinfo;
  private TiffCodec[] m_builtInCodecs;
  private Tiff.codecList m_registeredCodecs;
  private TiffTagMethods m_defaultTagMethods;
  private bool m_disposed;
  private Stream m_fileStream;
  private TiffStream m_stream;
  private static readonly TiffFieldInfo[] tiffFieldInfo = new TiffFieldInfo[166]
  {
    new TiffFieldInfo(TiffTag.SUBFILETYPE, (short) 1, (short) 1, TiffType.LONG, (short) 5, true, false, "SubfileType"),
    new TiffFieldInfo(TiffTag.SUBFILETYPE, (short) 1, (short) 1, TiffType.SHORT, (short) 5, true, false, "SubfileType"),
    new TiffFieldInfo(TiffTag.OSUBFILETYPE, (short) 1, (short) 1, TiffType.SHORT, (short) 5, true, false, "OldSubfileType"),
    new TiffFieldInfo(TiffTag.IMAGEWIDTH, (short) 1, (short) 1, TiffType.LONG, (short) 1, false, false, "ImageWidth"),
    new TiffFieldInfo(TiffTag.IMAGEWIDTH, (short) 1, (short) 1, TiffType.SHORT, (short) 1, false, false, "ImageWidth"),
    new TiffFieldInfo(TiffTag.IMAGELENGTH, (short) 1, (short) 1, TiffType.LONG, (short) 1, true, false, "ImageLength"),
    new TiffFieldInfo(TiffTag.IMAGELENGTH, (short) 1, (short) 1, TiffType.SHORT, (short) 1, true, false, "ImageLength"),
    new TiffFieldInfo(TiffTag.BITSPERSAMPLE, (short) -1, (short) -1, TiffType.SHORT, (short) 6, false, false, "BitsPerSample"),
    new TiffFieldInfo(TiffTag.BITSPERSAMPLE, (short) -1, (short) -1, TiffType.LONG, (short) 6, false, false, "BitsPerSample"),
    new TiffFieldInfo(TiffTag.COMPRESSION, (short) -1, (short) 1, TiffType.SHORT, (short) 7, false, false, "Compression"),
    new TiffFieldInfo(TiffTag.COMPRESSION, (short) -1, (short) 1, TiffType.LONG, (short) 7, false, false, "Compression"),
    new TiffFieldInfo(TiffTag.PHOTOMETRIC, (short) 1, (short) 1, TiffType.SHORT, (short) 8, false, false, "PhotometricInterpretation"),
    new TiffFieldInfo(TiffTag.PHOTOMETRIC, (short) 1, (short) 1, TiffType.LONG, (short) 8, false, false, "PhotometricInterpretation"),
    new TiffFieldInfo(TiffTag.THRESHHOLDING, (short) 1, (short) 1, TiffType.SHORT, (short) 9, true, false, "Threshholding"),
    new TiffFieldInfo(TiffTag.CELLWIDTH, (short) 1, (short) 1, TiffType.SHORT, (short) 0, true, false, "CellWidth"),
    new TiffFieldInfo(TiffTag.CELLLENGTH, (short) 1, (short) 1, TiffType.SHORT, (short) 0, true, false, "CellLength"),
    new TiffFieldInfo(TiffTag.FILLORDER, (short) 1, (short) 1, TiffType.SHORT, (short) 10, false, false, "FillOrder"),
    new TiffFieldInfo(TiffTag.DOCUMENTNAME, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "DocumentName"),
    new TiffFieldInfo(TiffTag.IMAGEDESCRIPTION, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "ImageDescription"),
    new TiffFieldInfo(TiffTag.MAKE, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "Make"),
    new TiffFieldInfo(TiffTag.MODEL, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "Model"),
    new TiffFieldInfo(TiffTag.STRIPOFFSETS, (short) -1, (short) -1, TiffType.LONG, (short) 25, false, false, "StripOffsets"),
    new TiffFieldInfo(TiffTag.STRIPOFFSETS, (short) -1, (short) -1, TiffType.SHORT, (short) 25, false, false, "StripOffsets"),
    new TiffFieldInfo(TiffTag.ORIENTATION, (short) 1, (short) 1, TiffType.SHORT, (short) 15, false, false, "Orientation"),
    new TiffFieldInfo(TiffTag.SAMPLESPERPIXEL, (short) 1, (short) 1, TiffType.SHORT, (short) 16 /*0x10*/, false, false, "SamplesPerPixel"),
    new TiffFieldInfo(TiffTag.ROWSPERSTRIP, (short) 1, (short) 1, TiffType.LONG, (short) 17, false, false, "RowsPerStrip"),
    new TiffFieldInfo(TiffTag.ROWSPERSTRIP, (short) 1, (short) 1, TiffType.SHORT, (short) 17, false, false, "RowsPerStrip"),
    new TiffFieldInfo(TiffTag.STRIPBYTECOUNTS, (short) -1, (short) -1, TiffType.LONG, (short) 24, false, false, "StripByteCounts"),
    new TiffFieldInfo(TiffTag.STRIPBYTECOUNTS, (short) -1, (short) -1, TiffType.SHORT, (short) 24, false, false, "StripByteCounts"),
    new TiffFieldInfo(TiffTag.MINSAMPLEVALUE, (short) -2, (short) -1, TiffType.SHORT, (short) 18, true, false, "MinSampleValue"),
    new TiffFieldInfo(TiffTag.MAXSAMPLEVALUE, (short) -2, (short) -1, TiffType.SHORT, (short) 19, true, false, "MaxSampleValue"),
    new TiffFieldInfo(TiffTag.XRESOLUTION, (short) 1, (short) 1, TiffType.RATIONAL, (short) 3, true, false, "XResolution"),
    new TiffFieldInfo(TiffTag.YRESOLUTION, (short) 1, (short) 1, TiffType.RATIONAL, (short) 3, true, false, "YResolution"),
    new TiffFieldInfo(TiffTag.PLANARCONFIG, (short) 1, (short) 1, TiffType.SHORT, (short) 20, false, false, "PlanarConfiguration"),
    new TiffFieldInfo(TiffTag.PAGENAME, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "PageName"),
    new TiffFieldInfo(TiffTag.XPOSITION, (short) 1, (short) 1, TiffType.RATIONAL, (short) 4, true, false, "XPosition"),
    new TiffFieldInfo(TiffTag.YPOSITION, (short) 1, (short) 1, TiffType.RATIONAL, (short) 4, true, false, "YPosition"),
    new TiffFieldInfo(TiffTag.FREEOFFSETS, (short) -1, (short) -1, TiffType.LONG, (short) 0, false, false, "FreeOffsets"),
    new TiffFieldInfo(TiffTag.FREEBYTECOUNTS, (short) -1, (short) -1, TiffType.LONG, (short) 0, false, false, "FreeByteCounts"),
    new TiffFieldInfo(TiffTag.GRAYRESPONSEUNIT, (short) 1, (short) 1, TiffType.SHORT, (short) 0, true, false, "GrayResponseUnit"),
    new TiffFieldInfo(TiffTag.GRAYRESPONSECURVE, (short) -1, (short) -1, TiffType.SHORT, (short) 0, true, false, "GrayResponseCurve"),
    new TiffFieldInfo(TiffTag.RESOLUTIONUNIT, (short) 1, (short) 1, TiffType.SHORT, (short) 22, true, false, "ResolutionUnit"),
    new TiffFieldInfo(TiffTag.PAGENUMBER, (short) 2, (short) 2, TiffType.SHORT, (short) 23, true, false, "PageNumber"),
    new TiffFieldInfo(TiffTag.COLORRESPONSEUNIT, (short) 1, (short) 1, TiffType.SHORT, (short) 0, true, false, "ColorResponseUnit"),
    new TiffFieldInfo(TiffTag.TRANSFERFUNCTION, (short) -1, (short) -1, TiffType.SHORT, (short) 44, true, false, "TransferFunction"),
    new TiffFieldInfo(TiffTag.SOFTWARE, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "Software"),
    new TiffFieldInfo(TiffTag.DATETIME, (short) 20, (short) 20, TiffType.ASCII, (short) 65, true, false, "DateTime"),
    new TiffFieldInfo(TiffTag.ARTIST, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "Artist"),
    new TiffFieldInfo(TiffTag.HOSTCOMPUTER, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "HostComputer"),
    new TiffFieldInfo(TiffTag.WHITEPOINT, (short) 2, (short) 2, TiffType.RATIONAL, (short) 65, true, false, "WhitePoint"),
    new TiffFieldInfo(TiffTag.PRIMARYCHROMATICITIES, (short) 6, (short) 6, TiffType.RATIONAL, (short) 65, true, false, "PrimaryChromaticities"),
    new TiffFieldInfo(TiffTag.COLORMAP, (short) -1, (short) -1, TiffType.SHORT, (short) 26, true, false, "ColorMap"),
    new TiffFieldInfo(TiffTag.HALFTONEHINTS, (short) 2, (short) 2, TiffType.SHORT, (short) 37, true, false, "HalftoneHints"),
    new TiffFieldInfo(TiffTag.TILEWIDTH, (short) 1, (short) 1, TiffType.LONG, (short) 2, false, false, "TileWidth"),
    new TiffFieldInfo(TiffTag.TILEWIDTH, (short) 1, (short) 1, TiffType.SHORT, (short) 2, false, false, "TileWidth"),
    new TiffFieldInfo(TiffTag.TILELENGTH, (short) 1, (short) 1, TiffType.LONG, (short) 2, false, false, "TileLength"),
    new TiffFieldInfo(TiffTag.TILELENGTH, (short) 1, (short) 1, TiffType.SHORT, (short) 2, false, false, "TileLength"),
    new TiffFieldInfo(TiffTag.TILEOFFSETS, (short) -1, (short) 1, TiffType.LONG, (short) 25, false, false, "TileOffsets"),
    new TiffFieldInfo(TiffTag.TILEBYTECOUNTS, (short) -1, (short) 1, TiffType.LONG, (short) 24, false, false, "TileByteCounts"),
    new TiffFieldInfo(TiffTag.TILEBYTECOUNTS, (short) -1, (short) 1, TiffType.SHORT, (short) 24, false, false, "TileByteCounts"),
    new TiffFieldInfo(TiffTag.SUBIFD, (short) -1, (short) -1, TiffType.IFD, (short) 49, true, true, "SubIFD"),
    new TiffFieldInfo(TiffTag.SUBIFD, (short) -1, (short) -1, TiffType.LONG, (short) 49, true, true, "SubIFD"),
    new TiffFieldInfo(TiffTag.INKSET, (short) 1, (short) 1, TiffType.SHORT, (short) 65, false, false, "InkSet"),
    new TiffFieldInfo(TiffTag.INKNAMES, (short) -1, (short) -1, TiffType.ASCII, (short) 46, true, true, "InkNames"),
    new TiffFieldInfo(TiffTag.NUMBEROFINKS, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "NumberOfInks"),
    new TiffFieldInfo(TiffTag.DOTRANGE, (short) 2, (short) 2, TiffType.SHORT, (short) 65, false, false, "DotRange"),
    new TiffFieldInfo(TiffTag.DOTRANGE, (short) 2, (short) 2, TiffType.BYTE, (short) 65, false, false, "DotRange"),
    new TiffFieldInfo(TiffTag.TARGETPRINTER, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "TargetPrinter"),
    new TiffFieldInfo(TiffTag.EXTRASAMPLES, (short) -1, (short) -1, TiffType.SHORT, (short) 31 /*0x1F*/, false, true, "ExtraSamples"),
    new TiffFieldInfo(TiffTag.EXTRASAMPLES, (short) -1, (short) -1, TiffType.BYTE, (short) 31 /*0x1F*/, false, true, "ExtraSamples"),
    new TiffFieldInfo(TiffTag.SAMPLEFORMAT, (short) -1, (short) -1, TiffType.SHORT, (short) 32 /*0x20*/, false, false, "SampleFormat"),
    new TiffFieldInfo(TiffTag.SMINSAMPLEVALUE, (short) -2, (short) -1, TiffType.NOTYPE, (short) 33, true, false, "SMinSampleValue"),
    new TiffFieldInfo(TiffTag.SMAXSAMPLEVALUE, (short) -2, (short) -1, TiffType.NOTYPE, (short) 34, true, false, "SMaxSampleValue"),
    new TiffFieldInfo(TiffTag.CLIPPATH, (short) -1, (short) -3, TiffType.BYTE, (short) 65, false, true, "ClipPath"),
    new TiffFieldInfo(TiffTag.XCLIPPATHUNITS, (short) 1, (short) 1, TiffType.SLONG, (short) 65, false, false, "XClipPathUnits"),
    new TiffFieldInfo(TiffTag.XCLIPPATHUNITS, (short) 1, (short) 1, TiffType.SSHORT, (short) 65, false, false, "XClipPathUnits"),
    new TiffFieldInfo(TiffTag.XCLIPPATHUNITS, (short) 1, (short) 1, TiffType.SBYTE, (short) 65, false, false, "XClipPathUnits"),
    new TiffFieldInfo(TiffTag.YCLIPPATHUNITS, (short) 1, (short) 1, TiffType.SLONG, (short) 65, false, false, "YClipPathUnits"),
    new TiffFieldInfo(TiffTag.YCLIPPATHUNITS, (short) 1, (short) 1, TiffType.SSHORT, (short) 65, false, false, "YClipPathUnits"),
    new TiffFieldInfo(TiffTag.YCLIPPATHUNITS, (short) 1, (short) 1, TiffType.SBYTE, (short) 65, false, false, "YClipPathUnits"),
    new TiffFieldInfo(TiffTag.YCBCRCOEFFICIENTS, (short) 3, (short) 3, TiffType.RATIONAL, (short) 65, false, false, "YCbCrCoefficients"),
    new TiffFieldInfo(TiffTag.YCBCRSUBSAMPLING, (short) 2, (short) 2, TiffType.SHORT, (short) 39, false, false, "YCbCrSubsampling"),
    new TiffFieldInfo(TiffTag.YCBCRPOSITIONING, (short) 1, (short) 1, TiffType.SHORT, (short) 40, false, false, "YCbCrPositioning"),
    new TiffFieldInfo(TiffTag.REFERENCEBLACKWHITE, (short) 6, (short) 6, TiffType.RATIONAL, (short) 41, true, false, "ReferenceBlackWhite"),
    new TiffFieldInfo(TiffTag.REFERENCEBLACKWHITE, (short) 6, (short) 6, TiffType.LONG, (short) 41, true, false, "ReferenceBlackWhite"),
    new TiffFieldInfo(TiffTag.XMLPACKET, (short) -3, (short) -3, TiffType.BYTE, (short) 65, false, true, "XMLPacket"),
    new TiffFieldInfo(TiffTag.MATTEING, (short) 1, (short) 1, TiffType.SHORT, (short) 31 /*0x1F*/, false, false, "Matteing"),
    new TiffFieldInfo(TiffTag.DATATYPE, (short) -2, (short) -1, TiffType.SHORT, (short) 32 /*0x20*/, false, false, "DataType"),
    new TiffFieldInfo(TiffTag.IMAGEDEPTH, (short) 1, (short) 1, TiffType.LONG, (short) 35, false, false, "ImageDepth"),
    new TiffFieldInfo(TiffTag.IMAGEDEPTH, (short) 1, (short) 1, TiffType.SHORT, (short) 35, false, false, "ImageDepth"),
    new TiffFieldInfo(TiffTag.TILEDEPTH, (short) 1, (short) 1, TiffType.LONG, (short) 36, false, false, "TileDepth"),
    new TiffFieldInfo(TiffTag.TILEDEPTH, (short) 1, (short) 1, TiffType.SHORT, (short) 36, false, false, "TileDepth"),
    new TiffFieldInfo(TiffTag.PIXAR_IMAGEFULLWIDTH, (short) 1, (short) 1, TiffType.LONG, (short) 65, true, false, "ImageFullWidth"),
    new TiffFieldInfo(TiffTag.PIXAR_IMAGEFULLLENGTH, (short) 1, (short) 1, TiffType.LONG, (short) 65, true, false, "ImageFullLength"),
    new TiffFieldInfo(TiffTag.PIXAR_TEXTUREFORMAT, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "TextureFormat"),
    new TiffFieldInfo(TiffTag.PIXAR_WRAPMODES, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "TextureWrapModes"),
    new TiffFieldInfo(TiffTag.PIXAR_FOVCOT, (short) 1, (short) 1, TiffType.FLOAT, (short) 65, true, false, "FieldOfViewCotangent"),
    new TiffFieldInfo(TiffTag.PIXAR_MATRIX_WORLDTOSCREEN, (short) 16 /*0x10*/, (short) 16 /*0x10*/, TiffType.FLOAT, (short) 65, true, false, "MatrixWorldToScreen"),
    new TiffFieldInfo(TiffTag.PIXAR_MATRIX_WORLDTOCAMERA, (short) 16 /*0x10*/, (short) 16 /*0x10*/, TiffType.FLOAT, (short) 65, true, false, "MatrixWorldToCamera"),
    new TiffFieldInfo(TiffTag.COPYRIGHT, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "Copyright"),
    new TiffFieldInfo(TiffTag.RICHTIFFIPTC, (short) -3, (short) -3, TiffType.LONG, (short) 65, false, true, "RichTIFFIPTC"),
    new TiffFieldInfo(TiffTag.PHOTOSHOP, (short) -3, (short) -3, TiffType.BYTE, (short) 65, false, true, "Photoshop"),
    new TiffFieldInfo(TiffTag.EXIFIFD, (short) 1, (short) 1, TiffType.LONG, (short) 65, false, false, "EXIFIFDOffset"),
    new TiffFieldInfo(TiffTag.ICCPROFILE, (short) -3, (short) -3, TiffType.UNDEFINED, (short) 65, false, true, "ICC Profile"),
    new TiffFieldInfo(TiffTag.GPSIFD, (short) 1, (short) 1, TiffType.LONG, (short) 65, false, false, "GPSIFDOffset"),
    new TiffFieldInfo(TiffTag.STONITS, (short) 1, (short) 1, TiffType.DOUBLE, (short) 65, false, false, "StoNits"),
    new TiffFieldInfo(TiffTag.INTEROPERABILITYIFD, (short) 1, (short) 1, TiffType.LONG, (short) 65, false, false, "InteroperabilityIFDOffset"),
    new TiffFieldInfo(TiffTag.DNGVERSION, (short) 4, (short) 4, TiffType.BYTE, (short) 65, false, false, "DNGVersion"),
    new TiffFieldInfo(TiffTag.DNGBACKWARDVERSION, (short) 4, (short) 4, TiffType.BYTE, (short) 65, false, false, "DNGBackwardVersion"),
    new TiffFieldInfo(TiffTag.UNIQUECAMERAMODEL, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "UniqueCameraModel"),
    new TiffFieldInfo(TiffTag.LOCALIZEDCAMERAMODEL, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "LocalizedCameraModel"),
    new TiffFieldInfo(TiffTag.LOCALIZEDCAMERAMODEL, (short) -1, (short) -1, TiffType.BYTE, (short) 65, true, true, "LocalizedCameraModel"),
    new TiffFieldInfo(TiffTag.CFAPLANECOLOR, (short) -1, (short) -1, TiffType.BYTE, (short) 65, false, true, "CFAPlaneColor"),
    new TiffFieldInfo(TiffTag.CFALAYOUT, (short) 1, (short) 1, TiffType.SHORT, (short) 65, false, false, "CFALayout"),
    new TiffFieldInfo(TiffTag.LINEARIZATIONTABLE, (short) -1, (short) -1, TiffType.SHORT, (short) 65, false, true, "LinearizationTable"),
    new TiffFieldInfo(TiffTag.BLACKLEVELREPEATDIM, (short) 2, (short) 2, TiffType.SHORT, (short) 65, false, false, "BlackLevelRepeatDim"),
    new TiffFieldInfo(TiffTag.BLACKLEVEL, (short) -1, (short) -1, TiffType.LONG, (short) 65, false, true, "BlackLevel"),
    new TiffFieldInfo(TiffTag.BLACKLEVEL, (short) -1, (short) -1, TiffType.SHORT, (short) 65, false, true, "BlackLevel"),
    new TiffFieldInfo(TiffTag.BLACKLEVEL, (short) -1, (short) -1, TiffType.RATIONAL, (short) 65, false, true, "BlackLevel"),
    new TiffFieldInfo(TiffTag.BLACKLEVELDELTAH, (short) -1, (short) -1, TiffType.SRATIONAL, (short) 65, false, true, "BlackLevelDeltaH"),
    new TiffFieldInfo(TiffTag.BLACKLEVELDELTAV, (short) -1, (short) -1, TiffType.SRATIONAL, (short) 65, false, true, "BlackLevelDeltaV"),
    new TiffFieldInfo(TiffTag.WHITELEVEL, (short) -2, (short) -2, TiffType.LONG, (short) 65, false, false, "WhiteLevel"),
    new TiffFieldInfo(TiffTag.WHITELEVEL, (short) -2, (short) -2, TiffType.SHORT, (short) 65, false, false, "WhiteLevel"),
    new TiffFieldInfo(TiffTag.DEFAULTSCALE, (short) 2, (short) 2, TiffType.RATIONAL, (short) 65, false, false, "DefaultScale"),
    new TiffFieldInfo(TiffTag.BESTQUALITYSCALE, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, false, false, "BestQualityScale"),
    new TiffFieldInfo(TiffTag.DEFAULTCROPORIGIN, (short) 2, (short) 2, TiffType.LONG, (short) 65, false, false, "DefaultCropOrigin"),
    new TiffFieldInfo(TiffTag.DEFAULTCROPORIGIN, (short) 2, (short) 2, TiffType.SHORT, (short) 65, false, false, "DefaultCropOrigin"),
    new TiffFieldInfo(TiffTag.DEFAULTCROPORIGIN, (short) 2, (short) 2, TiffType.RATIONAL, (short) 65, false, false, "DefaultCropOrigin"),
    new TiffFieldInfo(TiffTag.DEFAULTCROPSIZE, (short) 2, (short) 2, TiffType.LONG, (short) 65, false, false, "DefaultCropSize"),
    new TiffFieldInfo(TiffTag.DEFAULTCROPSIZE, (short) 2, (short) 2, TiffType.SHORT, (short) 65, false, false, "DefaultCropSize"),
    new TiffFieldInfo(TiffTag.DEFAULTCROPSIZE, (short) 2, (short) 2, TiffType.RATIONAL, (short) 65, false, false, "DefaultCropSize"),
    new TiffFieldInfo(TiffTag.COLORMATRIX1, (short) -1, (short) -1, TiffType.SRATIONAL, (short) 65, false, true, "ColorMatrix1"),
    new TiffFieldInfo(TiffTag.COLORMATRIX2, (short) -1, (short) -1, TiffType.SRATIONAL, (short) 65, false, true, "ColorMatrix2"),
    new TiffFieldInfo(TiffTag.CAMERACALIBRATION1, (short) -1, (short) -1, TiffType.SRATIONAL, (short) 65, false, true, "CameraCalibration1"),
    new TiffFieldInfo(TiffTag.CAMERACALIBRATION2, (short) -1, (short) -1, TiffType.SRATIONAL, (short) 65, false, true, "CameraCalibration2"),
    new TiffFieldInfo(TiffTag.REDUCTIONMATRIX1, (short) -1, (short) -1, TiffType.SRATIONAL, (short) 65, false, true, "ReductionMatrix1"),
    new TiffFieldInfo(TiffTag.REDUCTIONMATRIX2, (short) -1, (short) -1, TiffType.SRATIONAL, (short) 65, false, true, "ReductionMatrix2"),
    new TiffFieldInfo(TiffTag.ANALOGBALANCE, (short) -1, (short) -1, TiffType.RATIONAL, (short) 65, false, true, "AnalogBalance"),
    new TiffFieldInfo(TiffTag.ASSHOTNEUTRAL, (short) -1, (short) -1, TiffType.SHORT, (short) 65, false, true, "AsShotNeutral"),
    new TiffFieldInfo(TiffTag.ASSHOTNEUTRAL, (short) -1, (short) -1, TiffType.RATIONAL, (short) 65, false, true, "AsShotNeutral"),
    new TiffFieldInfo(TiffTag.ASSHOTWHITEXY, (short) 2, (short) 2, TiffType.RATIONAL, (short) 65, false, false, "AsShotWhiteXY"),
    new TiffFieldInfo(TiffTag.BASELINEEXPOSURE, (short) 1, (short) 1, TiffType.SRATIONAL, (short) 65, false, false, "BaselineExposure"),
    new TiffFieldInfo(TiffTag.BASELINENOISE, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, false, false, "BaselineNoise"),
    new TiffFieldInfo(TiffTag.BASELINESHARPNESS, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, false, false, "BaselineSharpness"),
    new TiffFieldInfo(TiffTag.BAYERGREENSPLIT, (short) 1, (short) 1, TiffType.LONG, (short) 65, false, false, "BayerGreenSplit"),
    new TiffFieldInfo(TiffTag.LINEARRESPONSELIMIT, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, false, false, "LinearResponseLimit"),
    new TiffFieldInfo(TiffTag.CAMERASERIALNUMBER, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "CameraSerialNumber"),
    new TiffFieldInfo(TiffTag.LENSINFO, (short) 4, (short) 4, TiffType.RATIONAL, (short) 65, false, false, "LensInfo"),
    new TiffFieldInfo(TiffTag.CHROMABLURRADIUS, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, false, false, "ChromaBlurRadius"),
    new TiffFieldInfo(TiffTag.ANTIALIASSTRENGTH, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, false, false, "AntiAliasStrength"),
    new TiffFieldInfo(TiffTag.SHADOWSCALE, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, false, false, "ShadowScale"),
    new TiffFieldInfo(TiffTag.DNGPRIVATEDATA, (short) -1, (short) -1, TiffType.BYTE, (short) 65, false, true, "DNGPrivateData"),
    new TiffFieldInfo(TiffTag.MAKERNOTESAFETY, (short) 1, (short) 1, TiffType.SHORT, (short) 65, false, false, "MakerNoteSafety"),
    new TiffFieldInfo(TiffTag.CALIBRATIONILLUMINANT1, (short) 1, (short) 1, TiffType.SHORT, (short) 65, false, false, "CalibrationIlluminant1"),
    new TiffFieldInfo(TiffTag.CALIBRATIONILLUMINANT2, (short) 1, (short) 1, TiffType.SHORT, (short) 65, false, false, "CalibrationIlluminant2"),
    new TiffFieldInfo(TiffTag.RAWDATAUNIQUEID, (short) 16 /*0x10*/, (short) 16 /*0x10*/, TiffType.BYTE, (short) 65, false, false, "RawDataUniqueID"),
    new TiffFieldInfo(TiffTag.ORIGINALRAWFILENAME, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "OriginalRawFileName"),
    new TiffFieldInfo(TiffTag.ORIGINALRAWFILENAME, (short) -1, (short) -1, TiffType.BYTE, (short) 65, true, true, "OriginalRawFileName"),
    new TiffFieldInfo(TiffTag.ORIGINALRAWFILEDATA, (short) -1, (short) -1, TiffType.UNDEFINED, (short) 65, false, true, "OriginalRawFileData"),
    new TiffFieldInfo(TiffTag.ACTIVEAREA, (short) 4, (short) 4, TiffType.LONG, (short) 65, false, false, "ActiveArea"),
    new TiffFieldInfo(TiffTag.ACTIVEAREA, (short) 4, (short) 4, TiffType.SHORT, (short) 65, false, false, "ActiveArea"),
    new TiffFieldInfo(TiffTag.MASKEDAREAS, (short) -1, (short) -1, TiffType.LONG, (short) 65, false, true, "MaskedAreas"),
    new TiffFieldInfo(TiffTag.ASSHOTICCPROFILE, (short) -1, (short) -1, TiffType.UNDEFINED, (short) 65, false, true, "AsShotICCProfile"),
    new TiffFieldInfo(TiffTag.ASSHOTPREPROFILEMATRIX, (short) -1, (short) -1, TiffType.SRATIONAL, (short) 65, false, true, "AsShotPreProfileMatrix"),
    new TiffFieldInfo(TiffTag.CURRENTICCPROFILE, (short) -1, (short) -1, TiffType.UNDEFINED, (short) 65, false, true, "CurrentICCProfile"),
    new TiffFieldInfo(TiffTag.CURRENTPREPROFILEMATRIX, (short) -1, (short) -1, TiffType.SRATIONAL, (short) 65, false, true, "CurrentPreProfileMatrix")
  };
  private static readonly TiffFieldInfo[] exifFieldInfo = new TiffFieldInfo[58]
  {
    new TiffFieldInfo(TiffTag.EXIF_EXPOSURETIME, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, true, false, "ExposureTime"),
    new TiffFieldInfo(TiffTag.EXIF_FNUMBER, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, true, false, "FNumber"),
    new TiffFieldInfo(TiffTag.EXIF_EXPOSUREPROGRAM, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "ExposureProgram"),
    new TiffFieldInfo(TiffTag.EXIF_SPECTRALSENSITIVITY, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "SpectralSensitivity"),
    new TiffFieldInfo(TiffTag.EXIF_ISOSPEEDRATINGS, (short) -1, (short) -1, TiffType.SHORT, (short) 65, true, true, "ISOSpeedRatings"),
    new TiffFieldInfo(TiffTag.EXIF_OECF, (short) -1, (short) -1, TiffType.UNDEFINED, (short) 65, true, true, "OptoelectricConversionFactor"),
    new TiffFieldInfo(TiffTag.EXIF_EXIFVERSION, (short) 4, (short) 4, TiffType.UNDEFINED, (short) 65, true, false, "ExifVersion"),
    new TiffFieldInfo(TiffTag.EXIF_DATETIMEORIGINAL, (short) 20, (short) 20, TiffType.ASCII, (short) 65, true, false, "DateTimeOriginal"),
    new TiffFieldInfo(TiffTag.EXIF_DATETIMEDIGITIZED, (short) 20, (short) 20, TiffType.ASCII, (short) 65, true, false, "DateTimeDigitized"),
    new TiffFieldInfo(TiffTag.EXIF_COMPONENTSCONFIGURATION, (short) 4, (short) 4, TiffType.UNDEFINED, (short) 65, true, false, "ComponentsConfiguration"),
    new TiffFieldInfo(TiffTag.EXIF_COMPRESSEDBITSPERPIXEL, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, true, false, "CompressedBitsPerPixel"),
    new TiffFieldInfo(TiffTag.EXIF_SHUTTERSPEEDVALUE, (short) 1, (short) 1, TiffType.SRATIONAL, (short) 65, true, false, "ShutterSpeedValue"),
    new TiffFieldInfo(TiffTag.EXIF_APERTUREVALUE, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, true, false, "ApertureValue"),
    new TiffFieldInfo(TiffTag.EXIF_BRIGHTNESSVALUE, (short) 1, (short) 1, TiffType.SRATIONAL, (short) 65, true, false, "BrightnessValue"),
    new TiffFieldInfo(TiffTag.EXIF_EXPOSUREBIASVALUE, (short) 1, (short) 1, TiffType.SRATIONAL, (short) 65, true, false, "ExposureBiasValue"),
    new TiffFieldInfo(TiffTag.EXIF_MAXAPERTUREVALUE, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, true, false, "MaxApertureValue"),
    new TiffFieldInfo(TiffTag.EXIF_SUBJECTDISTANCE, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, true, false, "SubjectDistance"),
    new TiffFieldInfo(TiffTag.EXIF_METERINGMODE, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "MeteringMode"),
    new TiffFieldInfo(TiffTag.EXIF_LIGHTSOURCE, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "LightSource"),
    new TiffFieldInfo(TiffTag.EXIF_FLASH, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "Flash"),
    new TiffFieldInfo(TiffTag.EXIF_FOCALLENGTH, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, true, false, "FocalLength"),
    new TiffFieldInfo(TiffTag.EXIF_SUBJECTAREA, (short) -1, (short) -1, TiffType.SHORT, (short) 65, true, true, "SubjectArea"),
    new TiffFieldInfo(TiffTag.EXIF_MAKERNOTE, (short) -1, (short) -1, TiffType.UNDEFINED, (short) 65, true, true, "MakerNote"),
    new TiffFieldInfo(TiffTag.EXIF_USERCOMMENT, (short) -1, (short) -1, TiffType.UNDEFINED, (short) 65, true, true, "UserComment"),
    new TiffFieldInfo(TiffTag.EXIF_SUBSECTIME, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "SubSecTime"),
    new TiffFieldInfo(TiffTag.EXIF_SUBSECTIMEORIGINAL, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "SubSecTimeOriginal"),
    new TiffFieldInfo(TiffTag.EXIF_SUBSECTIMEDIGITIZED, (short) -1, (short) -1, TiffType.ASCII, (short) 65, true, false, "SubSecTimeDigitized"),
    new TiffFieldInfo(TiffTag.EXIF_FLASHPIXVERSION, (short) 4, (short) 4, TiffType.UNDEFINED, (short) 65, true, false, "FlashpixVersion"),
    new TiffFieldInfo(TiffTag.EXIF_COLORSPACE, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "ColorSpace"),
    new TiffFieldInfo(TiffTag.EXIF_PIXELXDIMENSION, (short) 1, (short) 1, TiffType.LONG, (short) 65, true, false, "PixelXDimension"),
    new TiffFieldInfo(TiffTag.EXIF_PIXELXDIMENSION, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "PixelXDimension"),
    new TiffFieldInfo(TiffTag.EXIF_PIXELYDIMENSION, (short) 1, (short) 1, TiffType.LONG, (short) 65, true, false, "PixelYDimension"),
    new TiffFieldInfo(TiffTag.EXIF_PIXELYDIMENSION, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "PixelYDimension"),
    new TiffFieldInfo(TiffTag.EXIF_RELATEDSOUNDFILE, (short) 13, (short) 13, TiffType.ASCII, (short) 65, true, false, "RelatedSoundFile"),
    new TiffFieldInfo(TiffTag.EXIF_FLASHENERGY, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, true, false, "FlashEnergy"),
    new TiffFieldInfo(TiffTag.EXIF_SPATIALFREQUENCYRESPONSE, (short) -1, (short) -1, TiffType.UNDEFINED, (short) 65, true, true, "SpatialFrequencyResponse"),
    new TiffFieldInfo(TiffTag.EXIF_FOCALPLANEXRESOLUTION, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, true, false, "FocalPlaneXResolution"),
    new TiffFieldInfo(TiffTag.EXIF_FOCALPLANEYRESOLUTION, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, true, false, "FocalPlaneYResolution"),
    new TiffFieldInfo(TiffTag.EXIF_FOCALPLANERESOLUTIONUNIT, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "FocalPlaneResolutionUnit"),
    new TiffFieldInfo(TiffTag.EXIF_SUBJECTLOCATION, (short) 2, (short) 2, TiffType.SHORT, (short) 65, true, false, "SubjectLocation"),
    new TiffFieldInfo(TiffTag.EXIF_EXPOSUREINDEX, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, true, false, "ExposureIndex"),
    new TiffFieldInfo(TiffTag.EXIF_SENSINGMETHOD, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "SensingMethod"),
    new TiffFieldInfo(TiffTag.EXIF_FILESOURCE, (short) 1, (short) 1, TiffType.UNDEFINED, (short) 65, true, false, "FileSource"),
    new TiffFieldInfo(TiffTag.EXIF_SCENETYPE, (short) 1, (short) 1, TiffType.UNDEFINED, (short) 65, true, false, "SceneType"),
    new TiffFieldInfo(TiffTag.EXIF_CFAPATTERN, (short) -1, (short) -1, TiffType.UNDEFINED, (short) 65, true, true, "CFAPattern"),
    new TiffFieldInfo(TiffTag.EXIF_CUSTOMRENDERED, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "CustomRendered"),
    new TiffFieldInfo(TiffTag.EXIF_EXPOSUREMODE, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "ExposureMode"),
    new TiffFieldInfo(TiffTag.EXIF_WHITEBALANCE, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "WhiteBalance"),
    new TiffFieldInfo(TiffTag.EXIF_DIGITALZOOMRATIO, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, true, false, "DigitalZoomRatio"),
    new TiffFieldInfo(TiffTag.EXIF_FOCALLENGTHIN35MMFILM, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "FocalLengthIn35mmFilm"),
    new TiffFieldInfo(TiffTag.EXIF_SCENECAPTURETYPE, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "SceneCaptureType"),
    new TiffFieldInfo(TiffTag.EXIF_GAINCONTROL, (short) 1, (short) 1, TiffType.RATIONAL, (short) 65, true, false, "GainControl"),
    new TiffFieldInfo(TiffTag.EXIF_CONTRAST, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "Contrast"),
    new TiffFieldInfo(TiffTag.EXIF_SATURATION, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "Saturation"),
    new TiffFieldInfo(TiffTag.EXIF_SHARPNESS, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "Sharpness"),
    new TiffFieldInfo(TiffTag.EXIF_DEVICESETTINGDESCRIPTION, (short) -1, (short) -1, TiffType.UNDEFINED, (short) 65, true, true, "DeviceSettingDescription"),
    new TiffFieldInfo(TiffTag.EXIF_SUBJECTDISTANCERANGE, (short) 1, (short) 1, TiffType.SHORT, (short) 65, true, false, "SubjectDistanceRange"),
    new TiffFieldInfo(TiffTag.EXIF_IMAGEUNIQUEID, (short) 33, (short) 33, TiffType.ASCII, (short) 65, true, false, "ImageUniqueID")
  };
  private static readonly uint[] typemask = new uint[14]
  {
    0U,
    (uint) byte.MaxValue,
    uint.MaxValue,
    (uint) ushort.MaxValue,
    uint.MaxValue,
    uint.MaxValue,
    (uint) byte.MaxValue,
    (uint) byte.MaxValue,
    (uint) ushort.MaxValue,
    uint.MaxValue,
    uint.MaxValue,
    uint.MaxValue,
    uint.MaxValue,
    uint.MaxValue
  };
  private static readonly int[] bigTypeshift = new int[14]
  {
    0,
    24,
    0,
    16 /*0x10*/,
    0,
    0,
    24,
    24,
    16 /*0x10*/,
    0,
    0,
    0,
    0,
    0
  };
  private static readonly int[] litTypeshift = new int[14];
  private static readonly byte[] TIFFBitRevTable = new byte[256 /*0x0100*/]
  {
    (byte) 0,
    (byte) 128 /*0x80*/,
    (byte) 64 /*0x40*/,
    (byte) 192 /*0xC0*/,
    (byte) 32 /*0x20*/,
    (byte) 160 /*0xA0*/,
    (byte) 96 /*0x60*/,
    (byte) 224 /*0xE0*/,
    (byte) 16 /*0x10*/,
    (byte) 144 /*0x90*/,
    (byte) 80 /*0x50*/,
    (byte) 208 /*0xD0*/,
    (byte) 48 /*0x30*/,
    (byte) 176 /*0xB0*/,
    (byte) 112 /*0x70*/,
    (byte) 240 /*0xF0*/,
    (byte) 8,
    (byte) 136,
    (byte) 72,
    (byte) 200,
    (byte) 40,
    (byte) 168,
    (byte) 104,
    (byte) 232,
    (byte) 24,
    (byte) 152,
    (byte) 88,
    (byte) 216,
    (byte) 56,
    (byte) 184,
    (byte) 120,
    (byte) 248,
    (byte) 4,
    (byte) 132,
    (byte) 68,
    (byte) 196,
    (byte) 36,
    (byte) 164,
    (byte) 100,
    (byte) 228,
    (byte) 20,
    (byte) 148,
    (byte) 84,
    (byte) 212,
    (byte) 52,
    (byte) 180,
    (byte) 116,
    (byte) 244,
    (byte) 12,
    (byte) 140,
    (byte) 76,
    (byte) 204,
    (byte) 44,
    (byte) 172,
    (byte) 108,
    (byte) 236,
    (byte) 28,
    (byte) 156,
    (byte) 92,
    (byte) 220,
    (byte) 60,
    (byte) 188,
    (byte) 124,
    (byte) 252,
    (byte) 2,
    (byte) 130,
    (byte) 66,
    (byte) 194,
    (byte) 34,
    (byte) 162,
    (byte) 98,
    (byte) 226,
    (byte) 18,
    (byte) 146,
    (byte) 82,
    (byte) 210,
    (byte) 50,
    (byte) 178,
    (byte) 114,
    (byte) 242,
    (byte) 10,
    (byte) 138,
    (byte) 74,
    (byte) 202,
    (byte) 42,
    (byte) 170,
    (byte) 106,
    (byte) 234,
    (byte) 26,
    (byte) 154,
    (byte) 90,
    (byte) 218,
    (byte) 58,
    (byte) 186,
    (byte) 122,
    (byte) 250,
    (byte) 6,
    (byte) 134,
    (byte) 70,
    (byte) 198,
    (byte) 38,
    (byte) 166,
    (byte) 102,
    (byte) 230,
    (byte) 22,
    (byte) 150,
    (byte) 86,
    (byte) 214,
    (byte) 54,
    (byte) 182,
    (byte) 118,
    (byte) 246,
    (byte) 14,
    (byte) 142,
    (byte) 78,
    (byte) 206,
    (byte) 46,
    (byte) 174,
    (byte) 110,
    (byte) 238,
    (byte) 30,
    (byte) 158,
    (byte) 94,
    (byte) 222,
    (byte) 62,
    (byte) 190,
    (byte) 126,
    (byte) 254,
    (byte) 1,
    (byte) 129,
    (byte) 65,
    (byte) 193,
    (byte) 33,
    (byte) 161,
    (byte) 97,
    (byte) 225,
    (byte) 17,
    (byte) 145,
    (byte) 81,
    (byte) 209,
    (byte) 49,
    (byte) 177,
    (byte) 113,
    (byte) 241,
    (byte) 9,
    (byte) 137,
    (byte) 73,
    (byte) 201,
    (byte) 41,
    (byte) 169,
    (byte) 105,
    (byte) 233,
    (byte) 25,
    (byte) 153,
    (byte) 89,
    (byte) 217,
    (byte) 57,
    (byte) 185,
    (byte) 121,
    (byte) 249,
    (byte) 5,
    (byte) 133,
    (byte) 69,
    (byte) 197,
    (byte) 37,
    (byte) 165,
    (byte) 101,
    (byte) 229,
    (byte) 21,
    (byte) 149,
    (byte) 85,
    (byte) 213,
    (byte) 53,
    (byte) 181,
    (byte) 117,
    (byte) 245,
    (byte) 13,
    (byte) 141,
    (byte) 77,
    (byte) 205,
    (byte) 45,
    (byte) 173,
    (byte) 109,
    (byte) 237,
    (byte) 29,
    (byte) 157,
    (byte) 93,
    (byte) 221,
    (byte) 61,
    (byte) 189,
    (byte) 125,
    (byte) 253,
    (byte) 3,
    (byte) 131,
    (byte) 67,
    (byte) 195,
    (byte) 35,
    (byte) 163,
    (byte) 99,
    (byte) 227,
    (byte) 19,
    (byte) 147,
    (byte) 83,
    (byte) 211,
    (byte) 51,
    (byte) 179,
    (byte) 115,
    (byte) 243,
    (byte) 11,
    (byte) 139,
    (byte) 75,
    (byte) 203,
    (byte) 43,
    (byte) 171,
    (byte) 107,
    (byte) 235,
    (byte) 27,
    (byte) 155,
    (byte) 91,
    (byte) 219,
    (byte) 59,
    (byte) 187,
    (byte) 123,
    (byte) 251,
    (byte) 7,
    (byte) 135,
    (byte) 71,
    (byte) 199,
    (byte) 39,
    (byte) 167,
    (byte) 103,
    (byte) 231,
    (byte) 23,
    (byte) 151,
    (byte) 87,
    (byte) 215,
    (byte) 55,
    (byte) 183,
    (byte) 119,
    (byte) 247,
    (byte) 15,
    (byte) 143,
    (byte) 79,
    (byte) 207,
    (byte) 47,
    (byte) 175,
    (byte) 111,
    (byte) 239,
    (byte) 31 /*0x1F*/,
    (byte) 159,
    (byte) 95,
    (byte) 223,
    (byte) 63 /*0x3F*/,
    (byte) 191,
    (byte) 127 /*0x7F*/,
    byte.MaxValue
  };
  private static readonly byte[] TIFFNoBitRevTable = new byte[256 /*0x0100*/]
  {
    (byte) 0,
    (byte) 1,
    (byte) 2,
    (byte) 3,
    (byte) 4,
    (byte) 5,
    (byte) 6,
    (byte) 7,
    (byte) 8,
    (byte) 9,
    (byte) 10,
    (byte) 11,
    (byte) 12,
    (byte) 13,
    (byte) 14,
    (byte) 15,
    (byte) 16 /*0x10*/,
    (byte) 17,
    (byte) 18,
    (byte) 19,
    (byte) 20,
    (byte) 21,
    (byte) 22,
    (byte) 23,
    (byte) 24,
    (byte) 25,
    (byte) 26,
    (byte) 27,
    (byte) 28,
    (byte) 29,
    (byte) 30,
    (byte) 31 /*0x1F*/,
    (byte) 32 /*0x20*/,
    (byte) 33,
    (byte) 34,
    (byte) 35,
    (byte) 36,
    (byte) 37,
    (byte) 38,
    (byte) 39,
    (byte) 40,
    (byte) 41,
    (byte) 42,
    (byte) 43,
    (byte) 44,
    (byte) 45,
    (byte) 46,
    (byte) 47,
    (byte) 48 /*0x30*/,
    (byte) 49,
    (byte) 50,
    (byte) 51,
    (byte) 52,
    (byte) 53,
    (byte) 54,
    (byte) 55,
    (byte) 56,
    (byte) 57,
    (byte) 58,
    (byte) 59,
    (byte) 60,
    (byte) 61,
    (byte) 62,
    (byte) 63 /*0x3F*/,
    (byte) 64 /*0x40*/,
    (byte) 65,
    (byte) 66,
    (byte) 67,
    (byte) 68,
    (byte) 69,
    (byte) 70,
    (byte) 71,
    (byte) 72,
    (byte) 73,
    (byte) 74,
    (byte) 75,
    (byte) 76,
    (byte) 77,
    (byte) 78,
    (byte) 79,
    (byte) 80 /*0x50*/,
    (byte) 81,
    (byte) 82,
    (byte) 83,
    (byte) 84,
    (byte) 85,
    (byte) 86,
    (byte) 87,
    (byte) 88,
    (byte) 89,
    (byte) 90,
    (byte) 91,
    (byte) 92,
    (byte) 93,
    (byte) 94,
    (byte) 95,
    (byte) 96 /*0x60*/,
    (byte) 97,
    (byte) 98,
    (byte) 99,
    (byte) 100,
    (byte) 101,
    (byte) 102,
    (byte) 103,
    (byte) 104,
    (byte) 105,
    (byte) 106,
    (byte) 107,
    (byte) 108,
    (byte) 109,
    (byte) 110,
    (byte) 111,
    (byte) 112 /*0x70*/,
    (byte) 113,
    (byte) 114,
    (byte) 115,
    (byte) 116,
    (byte) 117,
    (byte) 118,
    (byte) 119,
    (byte) 120,
    (byte) 121,
    (byte) 122,
    (byte) 123,
    (byte) 124,
    (byte) 125,
    (byte) 126,
    (byte) 127 /*0x7F*/,
    (byte) 128 /*0x80*/,
    (byte) 129,
    (byte) 130,
    (byte) 131,
    (byte) 132,
    (byte) 133,
    (byte) 134,
    (byte) 135,
    (byte) 136,
    (byte) 137,
    (byte) 138,
    (byte) 139,
    (byte) 140,
    (byte) 141,
    (byte) 142,
    (byte) 143,
    (byte) 144 /*0x90*/,
    (byte) 145,
    (byte) 146,
    (byte) 147,
    (byte) 148,
    (byte) 149,
    (byte) 150,
    (byte) 151,
    (byte) 152,
    (byte) 153,
    (byte) 154,
    (byte) 155,
    (byte) 156,
    (byte) 157,
    (byte) 158,
    (byte) 159,
    (byte) 160 /*0xA0*/,
    (byte) 161,
    (byte) 162,
    (byte) 163,
    (byte) 164,
    (byte) 165,
    (byte) 166,
    (byte) 167,
    (byte) 168,
    (byte) 169,
    (byte) 170,
    (byte) 171,
    (byte) 172,
    (byte) 173,
    (byte) 174,
    (byte) 175,
    (byte) 176 /*0xB0*/,
    (byte) 177,
    (byte) 178,
    (byte) 179,
    (byte) 180,
    (byte) 181,
    (byte) 182,
    (byte) 183,
    (byte) 184,
    (byte) 185,
    (byte) 186,
    (byte) 187,
    (byte) 188,
    (byte) 189,
    (byte) 190,
    (byte) 191,
    (byte) 192 /*0xC0*/,
    (byte) 193,
    (byte) 194,
    (byte) 195,
    (byte) 196,
    (byte) 197,
    (byte) 198,
    (byte) 199,
    (byte) 200,
    (byte) 201,
    (byte) 202,
    (byte) 203,
    (byte) 204,
    (byte) 205,
    (byte) 206,
    (byte) 207,
    (byte) 208 /*0xD0*/,
    (byte) 209,
    (byte) 210,
    (byte) 211,
    (byte) 212,
    (byte) 213,
    (byte) 214,
    (byte) 215,
    (byte) 216,
    (byte) 217,
    (byte) 218,
    (byte) 219,
    (byte) 220,
    (byte) 221,
    (byte) 222,
    (byte) 223,
    (byte) 224 /*0xE0*/,
    (byte) 225,
    (byte) 226,
    (byte) 227,
    (byte) 228,
    (byte) 229,
    (byte) 230,
    (byte) 231,
    (byte) 232,
    (byte) 233,
    (byte) 234,
    (byte) 235,
    (byte) 236,
    (byte) 237,
    (byte) 238,
    (byte) 239,
    (byte) 240 /*0xF0*/,
    (byte) 241,
    (byte) 242,
    (byte) 243,
    (byte) 244,
    (byte) 245,
    (byte) 246,
    (byte) 247,
    (byte) 248,
    (byte) 249,
    (byte) 250,
    (byte) 251,
    (byte) 252,
    (byte) 253,
    (byte) 254,
    byte.MaxValue
  };

  private static Tiff Open(string fileName, string mode, Tiff.TiffExtendProc extender)
  {
    FileMode m;
    FileAccess a;
    Tiff.getMode(mode, nameof (Open), out m, out a);
    FileStream clientData;
    try
    {
      clientData = a != FileAccess.Read ? File.Open(fileName, m, a) : File.Open(fileName, m, a, FileShare.Read);
    }
    catch (Exception ex)
    {
      return (Tiff) null;
    }
    Tiff tiff = Tiff.ClientOpen(fileName, mode, (object) clientData, new TiffStream(), extender);
    if (tiff == null)
      clientData.Dispose();
    else
      tiff.m_fileStream = (Stream) clientData;
    return tiff;
  }

  private static Tiff ClientOpen(
    string name,
    string mode,
    object clientData,
    TiffStream stream,
    Tiff.TiffExtendProc extender)
  {
    if (mode == null || mode.Length == 0)
      return (Tiff) null;
    int mode1 = Tiff.getMode(mode, nameof (ClientOpen), out FileMode _, out FileAccess _);
    Tiff tiff = new Tiff();
    tiff.m_name = name;
    tiff.m_mode = mode1 & -769;
    tiff.m_curdir = (short) -1;
    tiff.m_curoff = 0U;
    tiff.m_curstrip = -1;
    tiff.m_row = -1;
    tiff.m_clientdata = clientData;
    if (stream == null)
      return (Tiff) null;
    tiff.m_stream = stream;
    tiff.m_currentCodec = tiff.m_builtInCodecs[0];
    tiff.m_flags = TiffFlags.MSB2LSB;
    if (mode1 == 0 || mode1 == 2)
      tiff.m_flags |= TiffFlags.STRIPCHOP;
    int length = mode.Length;
    for (int index = 0; index < length; ++index)
    {
      switch (mode[index])
      {
        case 'B':
          tiff.m_flags = tiff.m_flags & ~TiffFlags.FILLORDER | TiffFlags.MSB2LSB;
          break;
        case 'C':
          if (mode1 == 0)
          {
            tiff.m_flags |= TiffFlags.STRIPCHOP;
            break;
          }
          break;
        case 'H':
          tiff.m_flags = tiff.m_flags & ~TiffFlags.FILLORDER | TiffFlags.LSB2MSB;
          break;
        case 'L':
          tiff.m_flags = tiff.m_flags & ~TiffFlags.FILLORDER | TiffFlags.LSB2MSB;
          break;
        case 'b':
          if ((mode1 & 256 /*0x0100*/) != 0)
          {
            tiff.m_flags |= TiffFlags.SWAB;
            break;
          }
          break;
        case 'c':
          if (mode1 == 0)
          {
            tiff.m_flags &= ~TiffFlags.STRIPCHOP;
            break;
          }
          break;
        case 'h':
          tiff.m_flags |= TiffFlags.HEADERONLY;
          break;
      }
    }
    if ((tiff.m_mode & 512 /*0x0200*/) != 0 || !tiff.readHeaderOk(ref tiff.m_header))
    {
      if (tiff.m_mode == 0)
        return (Tiff) null;
      tiff.m_header.tiff_magic = (tiff.m_flags & TiffFlags.SWAB) != TiffFlags.SWAB ? (short) 18761 : (short) 19789;
      tiff.m_header.tiff_version = (short) 42;
      if ((tiff.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
        Tiff.SwabShort(ref tiff.m_header.tiff_version);
      tiff.m_header.tiff_diroff = 0U;
      tiff.seekFile(0L, SeekOrigin.Begin);
      tiff.initOrder((int) tiff.m_header.tiff_magic);
      tiff.setupDefaultDirectory();
      tiff.m_diroff = 0U;
      tiff.m_dirlist = (uint[]) null;
      tiff.m_dirlistsize = 0;
      tiff.m_dirnumber = (short) 0;
      return tiff;
    }
    if (tiff.m_header.tiff_magic != (short) 19789 && tiff.m_header.tiff_magic != (short) 18761 && tiff.m_header.tiff_magic != (short) 20549)
    {
      tiff.m_mode = 0;
      return (Tiff) null;
    }
    tiff.initOrder((int) tiff.m_header.tiff_magic);
    if ((tiff.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
    {
      Tiff.SwabShort(ref tiff.m_header.tiff_version);
      Tiff.SwabUInt(ref tiff.m_header.tiff_diroff);
    }
    if (tiff.m_header.tiff_version == (short) 43)
    {
      tiff.m_mode = 0;
      return (Tiff) null;
    }
    if (tiff.m_header.tiff_version != (short) 42)
    {
      tiff.m_mode = 0;
      return (Tiff) null;
    }
    tiff.m_flags |= TiffFlags.MYBUFFER;
    tiff.m_rawcp = 0;
    tiff.m_rawdata = (byte[]) null;
    tiff.m_rawdatasize = 0;
    if ((tiff.m_flags & TiffFlags.HEADERONLY) == TiffFlags.HEADERONLY)
      return tiff;
    switch (mode[0])
    {
      case 'a':
        tiff.setupDefaultDirectory();
        return tiff;
      case 'r':
        tiff.m_nextdiroff = tiff.m_header.tiff_diroff;
        if (tiff.ReadDirectory())
        {
          tiff.m_rawcc = -1;
          tiff.m_flags |= TiffFlags.BUFFERSETUP;
          return tiff;
        }
        break;
    }
    tiff.m_mode = 0;
    return (Tiff) null;
  }

  private Tiff()
  {
    this.m_clientdata = (object) 0;
    this.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmNone;
    this.setupBuiltInCodecs();
    this.m_defaultTagMethods = new TiffTagMethods();
  }

  private void Dispose(bool disposing)
  {
    if (this.m_disposed)
      return;
    if (disposing)
    {
      this.Close();
      if (this.m_fileStream != null)
        this.m_fileStream.Dispose();
    }
    this.m_disposed = true;
  }

  internal static void SwabUInt(ref uint lp)
  {
    byte[] numArray = new byte[4]
    {
      (byte) lp,
      (byte) (lp >> 8),
      (byte) (lp >> 16 /*0x10*/),
      (byte) (lp >> 24)
    };
    byte num1 = numArray[3];
    numArray[3] = numArray[0];
    numArray[0] = num1;
    byte num2 = numArray[2];
    numArray[2] = numArray[1];
    numArray[1] = num2;
    lp = (uint) numArray[0] & (uint) byte.MaxValue;
    lp += (uint) (((int) numArray[1] & (int) byte.MaxValue) << 8);
    lp += (uint) (((int) numArray[2] & (int) byte.MaxValue) << 16 /*0x10*/);
    lp += (uint) numArray[3] << 24;
  }

  internal static uint[] Realloc(uint[] buffer, int elementCount, int newElementCount)
  {
    uint[] dst = new uint[newElementCount];
    if (buffer != null)
    {
      int num = Math.Min(elementCount, newElementCount);
      Buffer.BlockCopy((Array) buffer, 0, (Array) dst, 0, num * 4);
    }
    return dst;
  }

  internal static TiffFieldInfo[] Realloc(
    TiffFieldInfo[] buffer,
    int elementCount,
    int newElementCount)
  {
    TiffFieldInfo[] destinationArray = new TiffFieldInfo[newElementCount];
    if (buffer != null)
    {
      int length = Math.Min(elementCount, newElementCount);
      Array.Copy((Array) buffer, (Array) destinationArray, length);
    }
    return destinationArray;
  }

  internal static TiffTagValue[] Realloc(
    TiffTagValue[] buffer,
    int elementCount,
    int newElementCount)
  {
    TiffTagValue[] destinationArray = new TiffTagValue[newElementCount];
    if (buffer != null)
    {
      int length = Math.Min(elementCount, newElementCount);
      Array.Copy((Array) buffer, (Array) destinationArray, length);
    }
    return destinationArray;
  }

  internal bool setCompressionScheme(Syncfusion.Pdf.Compression.JBIG2.Compression scheme)
  {
    TiffCodec tiffCodec = this.FindCodec(scheme) ?? this.m_builtInCodecs[0];
    this.m_decodestatus = tiffCodec.CanDecode;
    this.m_flags &= ~(TiffFlags.NOBITREV | TiffFlags.NOREADRAW);
    this.m_currentCodec = tiffCodec;
    return tiffCodec.Init();
  }

  private void postDecode(byte[] buffer, int offset, int count)
  {
    switch (this.m_postDecodeMethod)
    {
      case Tiff.PostDecodeMethodType.pdmSwab16Bit:
        Tiff.swab16BitData(buffer, offset, count);
        break;
      case Tiff.PostDecodeMethodType.pdmSwab24Bit:
        Tiff.swab24BitData(buffer, offset, count);
        break;
      case Tiff.PostDecodeMethodType.pdmSwab32Bit:
        Tiff.swab32BitData(buffer, offset, count);
        break;
      case Tiff.PostDecodeMethodType.pdmSwab64Bit:
        Tiff.swab64BitData(buffer, offset, count);
        break;
    }
  }

  private static bool defaultTransferFunction(TiffDirectory td)
  {
    short[][] transferfunction = td.td_transferfunction;
    transferfunction[0] = (short[]) null;
    transferfunction[1] = (short[]) null;
    transferfunction[2] = (short[]) null;
    if (td.td_bitspersample >= (short) 30)
      return false;
    int length = 1 << (int) td.td_bitspersample;
    transferfunction[0] = new short[length];
    transferfunction[0][0] = (short) 0;
    for (int index = 1; index < length; ++index)
    {
      double x = (double) index / ((double) length - 1.0);
      transferfunction[0][index] = (short) Math.Floor((double) ushort.MaxValue * Math.Pow(x, 2.2) + 0.5);
    }
    if ((int) td.td_samplesperpixel - (int) td.td_extrasamples > 1)
    {
      transferfunction[1] = new short[length];
      Buffer.BlockCopy((Array) transferfunction[0], 0, (Array) transferfunction[1], 0, transferfunction[0].Length * 2);
      transferfunction[2] = new short[length];
      Buffer.BlockCopy((Array) transferfunction[0], 0, (Array) transferfunction[2], 0, transferfunction[0].Length * 2);
    }
    return true;
  }

  private static void defaultRefBlackWhite(TiffDirectory td)
  {
    td.td_refblackwhite = new float[6];
    if (td.td_photometric == Photometric.YCBCR)
    {
      td.td_refblackwhite[0] = 0.0f;
      td.td_refblackwhite[1] = td.td_refblackwhite[3] = td.td_refblackwhite[5] = (float) byte.MaxValue;
      td.td_refblackwhite[2] = td.td_refblackwhite[4] = 128f;
    }
    else
    {
      for (int index = 0; index < 3; ++index)
      {
        td.td_refblackwhite[2 * index] = 0.0f;
        td.td_refblackwhite[2 * index + 1] = (float) ((1L << (int) td.td_bitspersample) - 1L);
      }
    }
  }

  internal static int readInt(byte[] buffer, int offset)
  {
    return ((int) buffer[offset++] & (int) byte.MaxValue) + (((int) buffer[offset++] & (int) byte.MaxValue) << 8) + (((int) buffer[offset++] & (int) byte.MaxValue) << 16 /*0x10*/) + ((int) buffer[offset++] << 24);
  }

  internal static void writeInt(int value, byte[] buffer, int offset)
  {
    buffer[offset++] = (byte) value;
    buffer[offset++] = (byte) (value >> 8);
    buffer[offset++] = (byte) (value >> 16 /*0x10*/);
    buffer[offset++] = (byte) (value >> 24);
  }

  internal static short readShort(byte[] buffer, int offset)
  {
    return (short) ((int) (short) ((int) buffer[offset] & (int) byte.MaxValue) + (int) (short) (((int) buffer[offset + 1] & (int) byte.MaxValue) << 8));
  }

  private void setupBuiltInCodecs()
  {
    this.m_builtInCodecs = new TiffCodec[19]
    {
      new TiffCodec(this, (Syncfusion.Pdf.Compression.JBIG2.Compression) -1, "Not configured"),
      (TiffCodec) new DumpModeCodec(this, Syncfusion.Pdf.Compression.JBIG2.Compression.NONE, "None"),
      (TiffCodec) new LZWCodec(this, Syncfusion.Pdf.Compression.JBIG2.Compression.LZW, "LZW"),
      (TiffCodec) new PackBitsCodec(this, Syncfusion.Pdf.Compression.JBIG2.Compression.PACKBITS, "PackBits"),
      new TiffCodec(this, Syncfusion.Pdf.Compression.JBIG2.Compression.THUNDERSCAN, "ThunderScan"),
      new TiffCodec(this, Syncfusion.Pdf.Compression.JBIG2.Compression.NEXT, "NeXT"),
      (TiffCodec) new JpegCodec(this, Syncfusion.Pdf.Compression.JBIG2.Compression.JPEG, "JPEG"),
      (TiffCodec) new OJpegCodec(this, Syncfusion.Pdf.Compression.JBIG2.Compression.OJPEG, "Old-style JPEG"),
      (TiffCodec) new CCITTCodec(this, Syncfusion.Pdf.Compression.JBIG2.Compression.CCITTRLE, "CCITT RLE"),
      (TiffCodec) new CCITTCodec(this, Syncfusion.Pdf.Compression.JBIG2.Compression.CCITTRLEW, "CCITT RLE/W"),
      (TiffCodec) new CCITTCodec(this, Syncfusion.Pdf.Compression.JBIG2.Compression.CCITTFAX3, "CCITT Group 3"),
      (TiffCodec) new CCITTCodec(this, Syncfusion.Pdf.Compression.JBIG2.Compression.CCITTFAX4, "CCITT Group 4"),
      new TiffCodec(this, Syncfusion.Pdf.Compression.JBIG2.Compression.JBIG, "ISO JBIG"),
      (TiffCodec) new DeflateCodec(this, Syncfusion.Pdf.Compression.JBIG2.Compression.DEFLATE, "Deflate"),
      (TiffCodec) new DeflateCodec(this, Syncfusion.Pdf.Compression.JBIG2.Compression.ADOBE_DEFLATE, "AdobeDeflate"),
      new TiffCodec(this, Syncfusion.Pdf.Compression.JBIG2.Compression.PIXARLOG, "PixarLog"),
      new TiffCodec(this, Syncfusion.Pdf.Compression.JBIG2.Compression.SGILOG, "SGILog"),
      new TiffCodec(this, Syncfusion.Pdf.Compression.JBIG2.Compression.SGILOG24, "SGILog24"),
      null
    };
  }

  internal static bool isPseudoTag(TiffTag t) => t > TiffTag.DCSHUESHIFTVALUES;

  private bool isFillOrder(FillOrder o)
  {
    TiffFlags tiffFlags = (TiffFlags) o;
    return (this.m_flags & tiffFlags) == tiffFlags;
  }

  private static int BITn(int n) => 1 << n;

  private bool okToChangeTag(TiffTag tag)
  {
    TiffFieldInfo fieldInfo = this.FindFieldInfo(tag, TiffType.NOTYPE);
    return fieldInfo != null && (tag == TiffTag.IMAGELENGTH || (this.m_flags & TiffFlags.BEENWRITING) != TiffFlags.BEENWRITING || fieldInfo.OkToChange);
  }

  private void setupDefaultDirectory()
  {
    int size;
    this.setupFieldInfo(Tiff.getFieldInfo(out size), size);
    this.m_dir = new TiffDirectory();
    this.m_postDecodeMethod = Tiff.PostDecodeMethodType.pdmNone;
    this.m_foundfield = (TiffFieldInfo) null;
    this.m_tagmethods = this.m_defaultTagMethods;
    this.SetField(TiffTag.COMPRESSION, (object) Syncfusion.Pdf.Compression.JBIG2.Compression.NONE);
    this.m_flags &= ~TiffFlags.DIRTYDIRECT;
    this.m_flags &= ~TiffFlags.ISTILED;
    this.m_tilesize = -1;
    this.m_scanlinesize = -1;
  }

  internal static void setString(out string cpp, string cp) => cpp = cp;

  internal static void setShortArray(out short[] wpp, short[] wp, int n)
  {
    wpp = new short[n];
    for (int index = 0; index < n; ++index)
      wpp[index] = wp[index];
  }

  internal static void setLongArray(out int[] lpp, int[] lp, int n)
  {
    lpp = new int[n];
    for (int index = 0; index < n; ++index)
      lpp[index] = lp[index];
  }

  internal static void setFloatArray(out float[] fpp, float[] fp, int n)
  {
    fpp = new float[n];
    for (int index = 0; index < n; ++index)
      fpp[index] = fp[index];
  }

  internal bool fieldSet(int field)
  {
    return (this.m_dir.td_fieldsset[field / 32 /*0x20*/] & Tiff.BITn(field)) != 0;
  }

  internal void setFieldBit(int field)
  {
    this.m_dir.td_fieldsset[field / 32 /*0x20*/] |= Tiff.BITn(field);
  }

  internal void clearFieldBit(int field)
  {
    this.m_dir.td_fieldsset[field / 32 /*0x20*/] &= ~Tiff.BITn(field);
  }

  private static TiffFieldInfo[] getFieldInfo(out int size)
  {
    size = Tiff.tiffFieldInfo.Length;
    return Tiff.tiffFieldInfo;
  }

  private void setupFieldInfo(TiffFieldInfo[] info, int n)
  {
    this.m_nfields = 0;
    this.MergeFieldInfo(info, n);
  }

  private static TiffFieldInfo createAnonFieldInfo(TiffTag tag, TiffType field_type)
  {
    return new TiffFieldInfo(tag, (short) -3, (short) -3, field_type, (short) 65, true, true, (string) null)
    {
      Name = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Tag {0}", (object) tag)
    };
  }

  internal static int dataSize(TiffType type)
  {
    switch (type)
    {
      case TiffType.BYTE:
      case TiffType.ASCII:
      case TiffType.SBYTE:
      case TiffType.UNDEFINED:
        return 1;
      case TiffType.SHORT:
      case TiffType.SSHORT:
        return 2;
      case TiffType.LONG:
      case TiffType.RATIONAL:
      case TiffType.SLONG:
      case TiffType.SRATIONAL:
      case TiffType.FLOAT:
      case TiffType.IFD:
        return 4;
      case TiffType.DOUBLE:
        return 8;
      default:
        return 0;
    }
  }

  private int extractData(TiffDirEntry dir)
  {
    int tdirType = (int) dir.tdir_type;
    return this.m_header.tiff_magic == (short) 19789 ? (int) (dir.tdir_offset >> this.m_typeshift[tdirType]) & (int) this.m_typemask[tdirType] : (int) dir.tdir_offset & (int) this.m_typemask[tdirType];
  }

  private bool byteCountLooksBad(TiffDirectory td)
  {
    if (td.td_stripbytecount[0] == 0U && td.td_stripoffset[0] != 0U || td.td_compression == Syncfusion.Pdf.Compression.JBIG2.Compression.NONE && (long) td.td_stripbytecount[0] > this.getFileSize() - (long) td.td_stripoffset[0])
      return true;
    return this.m_mode == 0 && td.td_compression == Syncfusion.Pdf.Compression.JBIG2.Compression.NONE && (long) td.td_stripbytecount[0] < (long) (this.ScanlineSize() * td.td_imagelength);
  }

  private static int howMany8(int x) => (x & 7) == 0 ? x >> 3 : (x >> 3) + 1;

  private bool estimateStripByteCounts(TiffDirEntry[] dir, short dircount)
  {
    this.m_dir.td_stripbytecount = new uint[this.m_dir.td_nstrips];
    if (this.m_dir.td_compression != Syncfusion.Pdf.Compression.JBIG2.Compression.NONE)
    {
      long num1 = (long) (10 + (int) dircount * 12 + 4);
      long fileSize = this.getFileSize();
      for (short index = 0; (int) index < (int) dircount; ++index)
      {
        int num2 = Tiff.DataWidth(dir[(int) index].tdir_type);
        if (num2 == 0)
          return false;
        int num3 = num2 * dir[(int) index].tdir_count;
        if (num3 > 4)
          num1 += (long) num3;
      }
      long num4 = fileSize - num1;
      if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
        num4 /= (long) this.m_dir.td_samplesperpixel;
      int index1;
      for (index1 = 0; index1 < this.m_dir.td_nstrips; ++index1)
        this.m_dir.td_stripbytecount[index1] = (uint) num4;
      int index2 = index1 - 1;
      if ((long) (this.m_dir.td_stripoffset[index2] + this.m_dir.td_stripbytecount[index2]) > fileSize)
        this.m_dir.td_stripbytecount[index2] = (uint) ((ulong) fileSize - (ulong) this.m_dir.td_stripoffset[index2]);
    }
    else if (this.IsTiled())
    {
      int num = this.TileSize();
      for (int index = 0; index < this.m_dir.td_nstrips; ++index)
        this.m_dir.td_stripbytecount[index] = (uint) num;
    }
    else
    {
      int num5 = this.ScanlineSize();
      int num6 = this.m_dir.td_imagelength / this.m_dir.td_stripsperimage;
      for (int index = 0; index < this.m_dir.td_nstrips; ++index)
        this.m_dir.td_stripbytecount[index] = (uint) (num5 * num6);
    }
    this.setFieldBit(24);
    if (!this.fieldSet(17))
      this.m_dir.td_rowsperstrip = this.m_dir.td_imagelength;
    return true;
  }

  private void missingRequired(string tagname)
  {
  }

  private int fetchFailed(TiffDirEntry dir)
  {
    int bit = (int) this.FieldWithTag(dir.tdir_tag).Bit;
    return 0;
  }

  private static int readDirectoryFind(TiffDirEntry[] dir, short dircount, TiffTag tagid)
  {
    for (short index = 0; (int) index < (int) dircount; ++index)
    {
      if (dir[(int) index].tdir_tag == tagid)
        return (int) index;
    }
    return -1;
  }

  private bool checkDirOffset(uint diroff)
  {
    if (diroff == 0U)
      return false;
    for (short index = 0; (int) index < (int) this.m_dirnumber && this.m_dirlist != null; ++index)
    {
      if ((int) this.m_dirlist[(int) index] == (int) diroff)
        return false;
    }
    ++this.m_dirnumber;
    if ((int) this.m_dirnumber > this.m_dirlistsize)
    {
      uint[] numArray = Tiff.Realloc(this.m_dirlist, (int) this.m_dirnumber - 1, 2 * (int) this.m_dirnumber);
      this.m_dirlistsize = 2 * (int) this.m_dirnumber;
      this.m_dirlist = numArray;
    }
    this.m_dirlist[(int) this.m_dirnumber - 1] = diroff;
    return true;
  }

  private short fetchDirectory(uint diroff, out TiffDirEntry[] pdir, out uint nextdiroff)
  {
    this.m_diroff = diroff;
    nextdiroff = 0U;
    pdir = (TiffDirEntry[]) null;
    short dircount;
    if (!this.seekOK((long) this.m_diroff) || !this.readShortOK(out dircount))
      return 0;
    if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
      Tiff.SwabShort(ref dircount);
    TiffDirEntry[] dir = new TiffDirEntry[(int) dircount];
    if (!this.readDirEntryOk(dir, dircount))
      return 0;
    int num1;
    this.readIntOK(out num1);
    nextdiroff = (uint) num1;
    if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
    {
      int num2 = (int) nextdiroff;
      Tiff.SwabLong(ref num2);
      nextdiroff = (uint) num2;
    }
    pdir = dir;
    return dircount;
  }

  private bool fetchSubjectDistance(TiffDirEntry dir)
  {
    if (dir.tdir_count != 1 || dir.tdir_type != TiffType.RATIONAL)
      return false;
    bool flag = false;
    byte[] buffer = new byte[8];
    if (this.fetchData(dir, buffer) != 0)
    {
      int[] numArray = new int[2]
      {
        Tiff.readInt(buffer, 0),
        Tiff.readInt(buffer, 4)
      };
      float rv;
      if (this.cvtRational(dir, numArray[0], numArray[1], out rv))
        flag = this.SetField(dir.tdir_tag, (object) (float) (numArray[0] != -1 ? (double) rv : -(double) rv));
    }
    return flag;
  }

  private bool checkDirCount(TiffDirEntry dir, int count)
  {
    if (count > dir.tdir_count)
      return false;
    if (count >= dir.tdir_count)
      return true;
    dir.tdir_count = count;
    return true;
  }

  private int fetchData(TiffDirEntry dir, byte[] buffer)
  {
    int num1 = Tiff.DataWidth(dir.tdir_type);
    int num2 = dir.tdir_count * num1;
    if (dir.tdir_count == 0 || num1 == 0 || num2 / num1 != dir.tdir_count)
      this.fetchFailed(dir);
    if (!this.seekOK((long) dir.tdir_offset))
      this.fetchFailed(dir);
    if (!this.readOK(buffer, num2))
      this.fetchFailed(dir);
    if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
    {
      switch (dir.tdir_type)
      {
        case TiffType.SHORT:
        case TiffType.SSHORT:
          short[] shorts = Tiff.ByteArrayToShorts(buffer, 0, num2);
          Tiff.SwabArrayOfShort(shorts, dir.tdir_count);
          Tiff.ShortsToByteArray(shorts, 0, dir.tdir_count, buffer, 0);
          break;
        case TiffType.LONG:
        case TiffType.SLONG:
        case TiffType.FLOAT:
        case TiffType.IFD:
          int[] ints1 = Tiff.ByteArrayToInts(buffer, 0, num2);
          Tiff.SwabArrayOfLong(ints1, dir.tdir_count);
          Tiff.IntsToByteArray(ints1, 0, dir.tdir_count, buffer, 0);
          break;
        case TiffType.RATIONAL:
        case TiffType.SRATIONAL:
          int[] ints2 = Tiff.ByteArrayToInts(buffer, 0, num2);
          Tiff.SwabArrayOfLong(ints2, 2 * dir.tdir_count);
          Tiff.IntsToByteArray(ints2, 0, 2 * dir.tdir_count, buffer, 0);
          break;
        case TiffType.DOUBLE:
          Tiff.swab64BitData(buffer, 0, num2);
          break;
      }
    }
    return num2;
  }

  private int fetchString(TiffDirEntry dir, out string cp)
  {
    if (dir.tdir_count <= 4)
    {
      int tdirOffset = (int) dir.tdir_offset;
      if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
        Tiff.SwabLong(ref tdirOffset);
      byte[] numArray = new byte[4];
      Tiff.writeInt(tdirOffset, numArray, 0);
      cp = Tiff.Latin1Encoding.GetString(numArray, 0, dir.tdir_count);
      return 1;
    }
    byte[] numArray1 = new byte[dir.tdir_count];
    int num = this.fetchData(dir, numArray1);
    cp = Tiff.Latin1Encoding.GetString(numArray1, 0, dir.tdir_count);
    return num;
  }

  private bool cvtRational(TiffDirEntry dir, int num, int denom, out float rv)
  {
    if (denom == 0)
    {
      rv = float.NaN;
      return false;
    }
    rv = (float) num / (float) denom;
    return true;
  }

  private float fetchRational(TiffDirEntry dir)
  {
    byte[] buffer = new byte[8];
    if (this.fetchData(dir, buffer) != 0)
    {
      int[] numArray = new int[2]
      {
        Tiff.readInt(buffer, 0),
        Tiff.readInt(buffer, 4)
      };
      float rv;
      if (this.cvtRational(dir, numArray[0], numArray[1], out rv))
        return rv;
    }
    return 1f;
  }

  private float fetchFloat(TiffDirEntry dir)
  {
    return BitConverter.ToSingle(BitConverter.GetBytes(this.extractData(dir)), 0);
  }

  private bool fetchByteArray(TiffDirEntry dir, byte[] v)
  {
    if (dir.tdir_count > 4)
      return this.fetchData(dir, v) != 0;
    int tdirCount = dir.tdir_count;
    if (this.m_header.tiff_magic == (short) 19789)
    {
      if (tdirCount == 4)
        v[3] = (byte) (dir.tdir_offset & (uint) byte.MaxValue);
      if (tdirCount >= 3)
        v[2] = (byte) (dir.tdir_offset >> 8 & (uint) byte.MaxValue);
      if (tdirCount >= 2)
        v[1] = (byte) (dir.tdir_offset >> 16 /*0x10*/ & (uint) byte.MaxValue);
      if (tdirCount >= 1)
        v[0] = (byte) (dir.tdir_offset >> 24);
    }
    else
    {
      if (tdirCount == 4)
        v[3] = (byte) (dir.tdir_offset >> 24);
      if (tdirCount >= 3)
        v[2] = (byte) (dir.tdir_offset >> 16 /*0x10*/ & (uint) byte.MaxValue);
      if (tdirCount >= 2)
        v[1] = (byte) (dir.tdir_offset >> 8 & (uint) byte.MaxValue);
      if (tdirCount >= 1)
        v[0] = (byte) (dir.tdir_offset & (uint) byte.MaxValue);
    }
    return true;
  }

  private bool fetchShortArray(TiffDirEntry dir, short[] v)
  {
    if (dir.tdir_count <= 2)
    {
      int tdirCount = dir.tdir_count;
      if (this.m_header.tiff_magic == (short) 19789)
      {
        if (tdirCount == 2)
          v[1] = (short) ((int) dir.tdir_offset & (int) ushort.MaxValue);
        if (tdirCount >= 1)
          v[0] = (short) (dir.tdir_offset >> 16 /*0x10*/);
      }
      else
      {
        if (tdirCount == 2)
          v[1] = (short) (dir.tdir_offset >> 16 /*0x10*/);
        if (tdirCount >= 1)
          v[0] = (short) ((int) dir.tdir_offset & (int) ushort.MaxValue);
      }
      return true;
    }
    byte[] numArray = new byte[dir.tdir_count * 2];
    int num = this.fetchData(dir, numArray);
    if (num != 0)
      Buffer.BlockCopy((Array) numArray, 0, (Array) v, 0, numArray.Length);
    return num != 0;
  }

  private bool fetchShortPair(TiffDirEntry dir)
  {
    if (dir.tdir_count > 2)
      return false;
    switch (dir.tdir_type)
    {
      case TiffType.BYTE:
      case TiffType.SBYTE:
        byte[] v1 = new byte[4];
        if (!this.fetchByteArray(dir, v1))
          return false;
        return this.SetField(dir.tdir_tag, (object) v1[0], (object) v1[1]);
      case TiffType.SHORT:
      case TiffType.SSHORT:
        short[] v2 = new short[2];
        if (!this.fetchShortArray(dir, v2))
          return false;
        return this.SetField(dir.tdir_tag, (object) v2[0], (object) v2[1]);
      default:
        return false;
    }
  }

  private bool fetchLongArray(TiffDirEntry dir, int[] v)
  {
    if (dir.tdir_count == 1)
    {
      v[0] = (int) dir.tdir_offset;
      return true;
    }
    byte[] numArray = new byte[dir.tdir_count * 4];
    int num = this.fetchData(dir, numArray);
    if (num != 0)
      Buffer.BlockCopy((Array) numArray, 0, (Array) v, 0, numArray.Length);
    return num != 0;
  }

  private bool fetchRationalArray(TiffDirEntry dir, float[] v)
  {
    bool flag = false;
    byte[] buffer = new byte[dir.tdir_count * Tiff.DataWidth(dir.tdir_type)];
    if (this.fetchData(dir, buffer) != 0)
    {
      int offset1 = 0;
      int[] numArray = new int[2];
      for (int index = 0; index < dir.tdir_count; ++index)
      {
        numArray[0] = Tiff.readInt(buffer, offset1);
        int offset2 = offset1 + 4;
        numArray[1] = Tiff.readInt(buffer, offset2);
        offset1 = offset2 + 4;
        flag = this.cvtRational(dir, numArray[0], numArray[1], out v[index]);
        if (!flag)
          break;
      }
    }
    return flag;
  }

  private bool fetchFloatArray(TiffDirEntry dir, float[] v)
  {
    if (dir.tdir_count == 1)
    {
      v[0] = BitConverter.ToSingle(BitConverter.GetBytes(dir.tdir_offset), 0);
      return true;
    }
    int num1 = Tiff.DataWidth(dir.tdir_type);
    byte[] buffer = new byte[dir.tdir_count * num1];
    int num2 = this.fetchData(dir, buffer);
    if (num2 != 0)
    {
      int startIndex = 0;
      for (int index = 0; index < num2 / 4; ++index)
      {
        v[index] = BitConverter.ToSingle(buffer, startIndex);
        startIndex += 4;
      }
    }
    return num2 != 0;
  }

  private bool fetchDoubleArray(TiffDirEntry dir, double[] v)
  {
    int num1 = Tiff.DataWidth(dir.tdir_type);
    byte[] buffer = new byte[dir.tdir_count * num1];
    int num2 = this.fetchData(dir, buffer);
    if (num2 != 0)
    {
      int startIndex = 0;
      for (int index = 0; index < num2 / 8; ++index)
      {
        v[index] = BitConverter.ToDouble(buffer, startIndex);
        startIndex += 8;
      }
    }
    return num2 != 0;
  }

  private bool fetchAnyArray(TiffDirEntry dir, double[] v)
  {
    switch (dir.tdir_type)
    {
      case TiffType.BYTE:
      case TiffType.SBYTE:
        byte[] v1 = new byte[dir.tdir_count];
        bool flag1 = this.fetchByteArray(dir, v1);
        if (flag1)
        {
          for (int index = dir.tdir_count - 1; index >= 0; --index)
            v[index] = (double) v1[index];
        }
        if (!flag1)
          return false;
        break;
      case TiffType.SHORT:
      case TiffType.SSHORT:
        short[] v2 = new short[dir.tdir_count];
        bool flag2 = this.fetchShortArray(dir, v2);
        if (flag2)
        {
          for (int index = dir.tdir_count - 1; index >= 0; --index)
            v[index] = (double) v2[index];
        }
        if (!flag2)
          return false;
        break;
      case TiffType.LONG:
      case TiffType.SLONG:
        int[] v3 = new int[dir.tdir_count];
        bool flag3 = this.fetchLongArray(dir, v3);
        if (flag3)
        {
          for (int index = dir.tdir_count - 1; index >= 0; --index)
            v[index] = (double) v3[index];
        }
        if (!flag3)
          return false;
        break;
      case TiffType.RATIONAL:
      case TiffType.SRATIONAL:
        float[] v4 = new float[dir.tdir_count];
        bool flag4 = this.fetchRationalArray(dir, v4);
        if (flag4)
        {
          for (int index = dir.tdir_count - 1; index >= 0; --index)
            v[index] = (double) v4[index];
        }
        if (!flag4)
          return false;
        break;
      case TiffType.FLOAT:
        float[] v5 = new float[dir.tdir_count];
        bool flag5 = this.fetchFloatArray(dir, v5);
        if (flag5)
        {
          for (int index = dir.tdir_count - 1; index >= 0; --index)
            v[index] = (double) v5[index];
        }
        if (!flag5)
          return false;
        break;
      case TiffType.DOUBLE:
        return this.fetchDoubleArray(dir, v);
      default:
        return false;
    }
    return true;
  }

  private bool fetchNormalTag(TiffDirEntry dir)
  {
    bool flag = false;
    TiffFieldInfo tiffFieldInfo = this.FieldWithTag(dir.tdir_tag);
    if (dir.tdir_count > 1)
    {
      switch (dir.tdir_type)
      {
        case TiffType.BYTE:
        case TiffType.SBYTE:
          byte[] v1 = new byte[dir.tdir_count];
          flag = this.fetchByteArray(dir, v1);
          if (flag)
          {
            if (tiffFieldInfo.PassCount)
            {
              flag = this.SetField(dir.tdir_tag, (object) dir.tdir_count, (object) v1);
              break;
            }
            flag = this.SetField(dir.tdir_tag, (object) v1);
            break;
          }
          break;
        case TiffType.ASCII:
        case TiffType.UNDEFINED:
          string cp1;
          flag = this.fetchString(dir, out cp1) != 0;
          if (flag)
          {
            if (tiffFieldInfo.PassCount)
            {
              flag = this.SetField(dir.tdir_tag, (object) dir.tdir_count, (object) cp1);
              break;
            }
            flag = this.SetField(dir.tdir_tag, (object) cp1);
            break;
          }
          break;
        case TiffType.SHORT:
        case TiffType.SSHORT:
          short[] v2 = new short[dir.tdir_count];
          flag = this.fetchShortArray(dir, v2);
          if (flag)
          {
            if (tiffFieldInfo.PassCount)
            {
              flag = this.SetField(dir.tdir_tag, (object) dir.tdir_count, (object) v2);
              break;
            }
            flag = this.SetField(dir.tdir_tag, (object) v2);
            break;
          }
          break;
        case TiffType.LONG:
        case TiffType.SLONG:
          int[] v3 = new int[dir.tdir_count];
          flag = this.fetchLongArray(dir, v3);
          if (flag)
          {
            if (tiffFieldInfo.PassCount)
            {
              flag = this.SetField(dir.tdir_tag, (object) dir.tdir_count, (object) v3);
              break;
            }
            flag = this.SetField(dir.tdir_tag, (object) v3);
            break;
          }
          break;
        case TiffType.RATIONAL:
        case TiffType.SRATIONAL:
          float[] v4 = new float[dir.tdir_count];
          flag = this.fetchRationalArray(dir, v4);
          if (flag)
          {
            if (tiffFieldInfo.PassCount)
            {
              flag = this.SetField(dir.tdir_tag, (object) dir.tdir_count, (object) v4);
              break;
            }
            flag = this.SetField(dir.tdir_tag, (object) v4);
            break;
          }
          break;
        case TiffType.FLOAT:
          float[] v5 = new float[dir.tdir_count];
          flag = this.fetchFloatArray(dir, v5);
          if (flag)
          {
            if (tiffFieldInfo.PassCount)
            {
              flag = this.SetField(dir.tdir_tag, (object) dir.tdir_count, (object) v5);
              break;
            }
            flag = this.SetField(dir.tdir_tag, (object) v5);
            break;
          }
          break;
        case TiffType.DOUBLE:
          double[] v6 = new double[dir.tdir_count];
          flag = this.fetchDoubleArray(dir, v6);
          if (flag)
          {
            if (tiffFieldInfo.PassCount)
            {
              flag = this.SetField(dir.tdir_tag, (object) dir.tdir_count, (object) v6);
              break;
            }
            flag = this.SetField(dir.tdir_tag, (object) v6);
            break;
          }
          break;
      }
    }
    else if (this.checkDirCount(dir, 1))
    {
      switch (dir.tdir_type)
      {
        case TiffType.BYTE:
        case TiffType.SHORT:
        case TiffType.SBYTE:
        case TiffType.SSHORT:
          switch (tiffFieldInfo.Type)
          {
            case TiffType.LONG:
            case TiffType.SLONG:
              int data1 = this.extractData(dir);
              if (tiffFieldInfo.PassCount)
              {
                int[] numArray = new int[1]{ data1 };
                flag = this.SetField(dir.tdir_tag, (object) 1, (object) numArray);
                break;
              }
              flag = this.SetField(dir.tdir_tag, (object) data1);
              break;
            default:
              short data2 = (short) this.extractData(dir);
              if (tiffFieldInfo.PassCount)
              {
                short[] numArray = new short[1]{ data2 };
                flag = this.SetField(dir.tdir_tag, (object) 1, (object) numArray);
                break;
              }
              flag = this.SetField(dir.tdir_tag, (object) data2);
              break;
          }
          break;
        case TiffType.ASCII:
        case TiffType.UNDEFINED:
          string cp2;
          flag = this.fetchString(dir, out cp2) != 0;
          if (flag)
          {
            if (tiffFieldInfo.PassCount)
            {
              flag = this.SetField(dir.tdir_tag, (object) 1, (object) cp2);
              break;
            }
            flag = this.SetField(dir.tdir_tag, (object) cp2);
            break;
          }
          break;
        case TiffType.LONG:
        case TiffType.SLONG:
        case TiffType.IFD:
          int data3 = this.extractData(dir);
          if (tiffFieldInfo.PassCount)
          {
            int[] numArray = new int[1]{ data3 };
            flag = this.SetField(dir.tdir_tag, (object) 1, (object) numArray);
            break;
          }
          flag = this.SetField(dir.tdir_tag, (object) data3);
          break;
        case TiffType.RATIONAL:
        case TiffType.SRATIONAL:
        case TiffType.FLOAT:
          float num = dir.tdir_type == TiffType.FLOAT ? this.fetchFloat(dir) : this.fetchRational(dir);
          if (tiffFieldInfo.PassCount)
          {
            float[] numArray = new float[1]{ num };
            flag = this.SetField(dir.tdir_tag, (object) 1, (object) numArray);
            break;
          }
          flag = this.SetField(dir.tdir_tag, (object) num);
          break;
        case TiffType.DOUBLE:
          double[] v7 = new double[1];
          flag = this.fetchDoubleArray(dir, v7);
          if (flag)
          {
            if (tiffFieldInfo.PassCount)
            {
              flag = this.SetField(dir.tdir_tag, (object) 1, (object) v7);
              break;
            }
            flag = this.SetField(dir.tdir_tag, (object) v7[0]);
            break;
          }
          break;
      }
    }
    return flag;
  }

  private bool fetchPerSampleShorts(TiffDirEntry dir, out short pl)
  {
    pl = (short) 0;
    short tdSamplesperpixel = this.m_dir.td_samplesperpixel;
    bool flag1 = false;
    if (this.checkDirCount(dir, (int) tdSamplesperpixel))
    {
      short[] v = new short[dir.tdir_count];
      if (this.fetchShortArray(dir, v))
      {
        int num = dir.tdir_count;
        if ((int) tdSamplesperpixel < num)
          num = (int) tdSamplesperpixel;
        bool flag2 = false;
        for (ushort index = 1; (int) index < num; ++index)
        {
          if ((int) v[(int) index] != (int) v[0])
          {
            flag2 = true;
            break;
          }
        }
        if (!flag2)
        {
          pl = v[0];
          flag1 = true;
        }
      }
    }
    return flag1;
  }

  private bool fetchPerSampleLongs(TiffDirEntry dir, out int pl)
  {
    pl = 0;
    short tdSamplesperpixel = this.m_dir.td_samplesperpixel;
    bool flag1 = false;
    if (this.checkDirCount(dir, (int) tdSamplesperpixel))
    {
      int[] v = new int[dir.tdir_count];
      if (this.fetchLongArray(dir, v))
      {
        int num = dir.tdir_count;
        if ((int) tdSamplesperpixel < num)
          num = (int) tdSamplesperpixel;
        bool flag2 = false;
        for (ushort index = 1; (int) index < num; ++index)
        {
          if (v[(int) index] != v[0])
          {
            flag2 = true;
            break;
          }
        }
        if (!flag2)
        {
          pl = v[0];
          flag1 = true;
        }
      }
    }
    return flag1;
  }

  private bool fetchPerSampleAnys(TiffDirEntry dir, out double pl)
  {
    pl = 0.0;
    short tdSamplesperpixel = this.m_dir.td_samplesperpixel;
    bool flag1 = false;
    if (this.checkDirCount(dir, (int) tdSamplesperpixel))
    {
      double[] v = new double[dir.tdir_count];
      if (this.fetchAnyArray(dir, v))
      {
        int num = dir.tdir_count;
        if ((int) tdSamplesperpixel < num)
          num = (int) tdSamplesperpixel;
        bool flag2 = false;
        for (ushort index = 1; (int) index < num; ++index)
        {
          if (v[(int) index] != v[0])
          {
            flag2 = true;
            break;
          }
        }
        if (!flag2)
        {
          pl = v[0];
          flag1 = true;
        }
      }
    }
    return flag1;
  }

  private bool fetchStripThing(TiffDirEntry dir, int nstrips, ref int[] lpp)
  {
    this.checkDirCount(dir, nstrips);
    if (lpp == null)
      lpp = new int[nstrips];
    else
      Array.Clear((Array) lpp, 0, lpp.Length);
    bool flag;
    if (dir.tdir_type == TiffType.SHORT)
    {
      short[] v = new short[dir.tdir_count];
      flag = this.fetchShortArray(dir, v);
      if (flag)
      {
        for (int index = 0; index < nstrips && index < dir.tdir_count; ++index)
          lpp[index] = (int) v[index];
      }
    }
    else if (nstrips != dir.tdir_count)
    {
      int[] v = new int[dir.tdir_count];
      flag = this.fetchLongArray(dir, v);
      if (flag)
      {
        for (int index = 0; index < nstrips && index < dir.tdir_count; ++index)
          lpp[index] = v[index];
      }
    }
    else
      flag = this.fetchLongArray(dir, lpp);
    return flag;
  }

  private bool fetchStripThing(TiffDirEntry dir, int nstrips, ref uint[] lpp)
  {
    int[] lpp1 = (int[]) null;
    if (lpp != null)
      lpp1 = new int[lpp.Length];
    bool flag = this.fetchStripThing(dir, nstrips, ref lpp1);
    if (flag)
    {
      if (lpp == null)
        lpp = new uint[lpp1.Length];
      Buffer.BlockCopy((Array) lpp1, 0, (Array) lpp, 0, lpp1.Length * 4);
    }
    return flag;
  }

  private bool fetchRefBlackWhite(TiffDirEntry dir)
  {
    if (dir.tdir_type == TiffType.RATIONAL && this.fetchNormalTag(dir))
    {
      for (int index = 0; index < this.m_dir.td_refblackwhite.Length; ++index)
      {
        if ((double) this.m_dir.td_refblackwhite[index] > 1.0)
          return true;
      }
    }
    dir.tdir_type = TiffType.LONG;
    int[] v = new int[dir.tdir_count];
    bool flag = this.fetchLongArray(dir, v);
    dir.tdir_type = TiffType.RATIONAL;
    if (flag)
    {
      float[] numArray = new float[dir.tdir_count];
      for (int index = 0; index < dir.tdir_count; ++index)
        numArray[index] = (float) v[index];
      flag = this.SetField(dir.tdir_tag, (object) numArray);
    }
    return flag;
  }

  private void chopUpSingleUncompressedStrip()
  {
    uint x = this.m_dir.td_stripbytecount[0];
    uint num1 = this.m_dir.td_stripoffset[0];
    int num2 = this.VTileSize(1);
    uint y;
    int num3;
    if (num2 > 8192 /*0x2000*/)
    {
      y = (uint) num2;
      num3 = 1;
    }
    else
    {
      if (num2 <= 0)
        return;
      num3 = 8192 /*0x2000*/ / num2;
      y = (uint) (num2 * num3);
    }
    if (num3 >= this.m_dir.td_rowsperstrip)
      return;
    uint length = Tiff.howMany(x, y);
    if (length == 0U)
      return;
    uint[] numArray1 = new uint[(IntPtr) length];
    uint[] numArray2 = new uint[(IntPtr) length];
    for (int index = 0; (long) index < (long) length; ++index)
    {
      if (y > x)
        y = x;
      numArray1[index] = y;
      numArray2[index] = num1;
      num1 += y;
      x -= y;
    }
    this.m_dir.td_nstrips = (int) length;
    this.m_dir.td_stripsperimage = (int) length;
    this.SetField(TiffTag.ROWSPERSTRIP, (object) num3);
    this.m_dir.td_stripbytecount = numArray1;
    this.m_dir.td_stripoffset = numArray2;
    this.m_dir.td_stripbytecountsorted = true;
  }

  internal static int roundUp(int x, int y) => Tiff.howMany(x, y) * y;

  internal static int howMany(int x, int y)
  {
    long num = ((long) x + ((long) y - 1L)) / (long) y;
    return num > (long) int.MaxValue ? 0 : (int) num;
  }

  internal static uint howMany(uint x, uint y)
  {
    long num = ((long) x + ((long) y - 1L)) / (long) y;
    return num > (long) uint.MaxValue ? 0U : (uint) num;
  }

  private void initOrder(int magic)
  {
    this.m_typemask = Tiff.typemask;
    if (magic == 19789)
    {
      this.m_typeshift = Tiff.bigTypeshift;
      this.m_flags |= TiffFlags.SWAB;
    }
    else
      this.m_typeshift = Tiff.litTypeshift;
  }

  private static int getMode(string mode, string module, out FileMode m, out FileAccess a)
  {
    m = (FileMode) 0;
    a = (FileAccess) 0;
    int mode1 = -1;
    if (mode.Length == 0)
      return mode1;
    switch (mode[0])
    {
      case 'a':
        m = FileMode.Open;
        a = FileAccess.ReadWrite;
        mode1 = 258;
        break;
      case 'r':
        m = FileMode.Open;
        a = FileAccess.Read;
        mode1 = 0;
        if (mode.Length > 1 && mode[1] == '+')
        {
          a = FileAccess.ReadWrite;
          mode1 = 2;
          break;
        }
        break;
      case 'w':
        m = FileMode.Create;
        a = FileAccess.ReadWrite;
        mode1 = 770;
        break;
    }
    return mode1;
  }

  private int readFile(byte[] buf, int offset, int size)
  {
    return this.m_stream.Read(this.m_clientdata, buf, offset, size);
  }

  private long seekFile(long off, SeekOrigin whence)
  {
    return this.m_stream.Seek(this.m_clientdata, off, whence);
  }

  private long getFileSize() => this.m_stream.Size(this.m_clientdata);

  private bool readOK(byte[] buf, int size) => this.readFile(buf, 0, size) == size;

  private bool readShortOK(out short value)
  {
    byte[] buf = new byte[2];
    bool flag = this.readOK(buf, 2);
    value = (short) 0;
    if (flag)
    {
      value = (short) ((int) buf[0] & (int) byte.MaxValue);
      value += (short) (((int) buf[1] & (int) byte.MaxValue) << 8);
    }
    return flag;
  }

  private bool readUIntOK(out uint value)
  {
    int num;
    bool flag = this.readIntOK(out num);
    value = !flag ? 0U : (uint) num;
    return flag;
  }

  private bool readIntOK(out int value)
  {
    byte[] buf = new byte[4];
    bool flag = this.readOK(buf, 4);
    value = 0;
    if (flag)
    {
      value = (int) buf[0] & (int) byte.MaxValue;
      value += ((int) buf[1] & (int) byte.MaxValue) << 8;
      value += ((int) buf[2] & (int) byte.MaxValue) << 16 /*0x10*/;
      value += (int) buf[3] << 24;
    }
    return flag;
  }

  private bool readDirEntryOk(TiffDirEntry[] dir, short dircount)
  {
    int size = 12 * (int) dircount;
    byte[] numArray = new byte[size];
    bool flag = this.readOK(numArray, size);
    if (flag)
      Tiff.readDirEntry(dir, dircount, numArray, 0);
    return flag;
  }

  private static void readDirEntry(TiffDirEntry[] dir, short dircount, byte[] bytes, int offset)
  {
    int offset1 = offset;
    for (int index = 0; index < (int) dircount; ++index)
    {
      TiffDirEntry tiffDirEntry = new TiffDirEntry();
      tiffDirEntry.tdir_tag = (TiffTag) (ushort) Tiff.readShort(bytes, offset1);
      int offset2 = offset1 + 2;
      tiffDirEntry.tdir_type = (TiffType) Tiff.readShort(bytes, offset2);
      int offset3 = offset2 + 2;
      tiffDirEntry.tdir_count = Tiff.readInt(bytes, offset3);
      int offset4 = offset3 + 4;
      tiffDirEntry.tdir_offset = (uint) Tiff.readInt(bytes, offset4);
      offset1 = offset4 + 4;
      dir[index] = tiffDirEntry;
    }
  }

  private bool readHeaderOk(ref Syncfusion.Pdf.Compression.JBIG2.Internal.TiffHeader header)
  {
    bool flag = this.readShortOK(out header.tiff_magic);
    if (flag)
      flag = this.readShortOK(out header.tiff_version);
    if (flag)
      flag = this.readUIntOK(out header.tiff_diroff);
    return flag;
  }

  private bool seekOK(long off) => this.seekFile(off, SeekOrigin.Begin) == off;

  private bool seek(int row, short sample)
  {
    if (row >= this.m_dir.td_imagelength)
      return false;
    int strip;
    if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
    {
      if ((int) sample >= (int) this.m_dir.td_samplesperpixel)
        return false;
      strip = this.m_dir.td_rowsperstrip == -1 ? 0 : (int) sample * this.m_dir.td_stripsperimage + row / this.m_dir.td_rowsperstrip;
    }
    else
      strip = this.m_dir.td_rowsperstrip == -1 ? 0 : row / this.m_dir.td_rowsperstrip;
    if (strip != this.m_curstrip)
    {
      if (!this.fillStrip(strip))
        return false;
    }
    else if (row < this.m_row && !this.startStrip(strip))
      return false;
    if (row != this.m_row)
    {
      if (!this.m_currentCodec.Seek(row - this.m_row))
        return false;
      this.m_row = row;
    }
    return true;
  }

  private int readRawStrip1(int strip, byte[] buf, int offset, int size, string module)
  {
    return !this.seekOK((long) this.m_dir.td_stripoffset[strip]) || this.readFile(buf, offset, size) != size ? -1 : size;
  }

  private int readRawTile1(int tile, byte[] buf, int offset, int size, string module)
  {
    return !this.seekOK((long) this.m_dir.td_stripoffset[tile]) || this.readFile(buf, offset, size) != size ? -1 : size;
  }

  private bool startStrip(int strip)
  {
    if ((this.m_flags & TiffFlags.CODERSETUP) != TiffFlags.CODERSETUP)
    {
      if (!this.m_currentCodec.SetupDecode())
        return false;
      this.m_flags |= TiffFlags.CODERSETUP;
    }
    this.m_curstrip = strip;
    this.m_row = strip % this.m_dir.td_stripsperimage * this.m_dir.td_rowsperstrip;
    this.m_rawcp = 0;
    this.m_rawcc = (this.m_flags & TiffFlags.NOREADRAW) != TiffFlags.NOREADRAW ? (int) this.m_dir.td_stripbytecount[strip] : 0;
    return this.m_currentCodec.PreDecode((short) (strip / this.m_dir.td_stripsperimage));
  }

  private bool startTile(int tile)
  {
    if ((this.m_flags & TiffFlags.CODERSETUP) != TiffFlags.CODERSETUP)
    {
      if (!this.m_currentCodec.SetupDecode())
        return false;
      this.m_flags |= TiffFlags.CODERSETUP;
    }
    this.m_curtile = tile;
    this.m_row = tile % Tiff.howMany(this.m_dir.td_imagewidth, this.m_dir.td_tilewidth) * this.m_dir.td_tilelength;
    this.m_col = tile % Tiff.howMany(this.m_dir.td_imagelength, this.m_dir.td_tilelength) * this.m_dir.td_tilewidth;
    this.m_rawcp = 0;
    this.m_rawcc = (this.m_flags & TiffFlags.NOREADRAW) != TiffFlags.NOREADRAW ? (int) this.m_dir.td_stripbytecount[tile] : 0;
    return this.m_currentCodec.PreDecode((short) (tile / this.m_dir.td_stripsperimage));
  }

  private bool checkRead(bool tiles) => this.m_mode != 1 && !(tiles ^ this.IsTiled());

  private static void swab16BitData(byte[] buffer, int offset, int count)
  {
    short[] shorts = Tiff.ByteArrayToShorts(buffer, offset, count);
    Tiff.SwabArrayOfShort(shorts, count / 2);
    Tiff.ShortsToByteArray(shorts, 0, count / 2, buffer, offset);
  }

  private static void swab24BitData(byte[] buffer, int offset, int count)
  {
    Tiff.SwabArrayOfTriples(buffer, offset, count / 3);
  }

  private static void swab32BitData(byte[] buffer, int offset, int count)
  {
    int[] ints = Tiff.ByteArrayToInts(buffer, offset, count);
    Tiff.SwabArrayOfLong(ints, count / 4);
    Tiff.IntsToByteArray(ints, 0, count / 4, buffer, offset);
  }

  private static void swab64BitData(byte[] buffer, int offset, int count)
  {
    int count1 = count / 8;
    double[] array = new double[count1];
    int startIndex = offset;
    for (int index = 0; index < count1; ++index)
    {
      array[index] = BitConverter.ToDouble(buffer, startIndex);
      startIndex += 8;
    }
    Tiff.SwabArrayOfDouble(array, count1);
    int dstOffset = offset;
    for (int index = 0; index < count1; ++index)
    {
      byte[] bytes = BitConverter.GetBytes(array[index]);
      Buffer.BlockCopy((Array) bytes, 0, (Array) buffer, dstOffset, bytes.Length);
      dstOffset += bytes.Length;
    }
  }

  internal bool fillStrip(int strip)
  {
    if ((this.m_flags & TiffFlags.NOREADRAW) != TiffFlags.NOREADRAW)
    {
      int num = (int) this.m_dir.td_stripbytecount[strip];
      if (num <= 0)
        return false;
      if (num > this.m_rawdatasize)
      {
        this.m_curstrip = -1;
        if ((this.m_flags & TiffFlags.MYBUFFER) != TiffFlags.MYBUFFER)
          return false;
        this.ReadBufferSetup((byte[]) null, Tiff.roundUp(num, 1024 /*0x0400*/));
      }
      if (this.readRawStrip1(strip, this.m_rawdata, 0, num, nameof (fillStrip)) != num)
        return false;
      if (!this.isFillOrder(this.m_dir.td_fillorder) && (this.m_flags & TiffFlags.NOBITREV) != TiffFlags.NOBITREV)
        Tiff.ReverseBits(this.m_rawdata, num);
    }
    return this.startStrip(strip);
  }

  internal bool fillTile(int tile)
  {
    if ((this.m_flags & TiffFlags.NOREADRAW) != TiffFlags.NOREADRAW)
    {
      int num = (int) this.m_dir.td_stripbytecount[tile];
      if (num <= 0)
        return false;
      if (num > this.m_rawdatasize)
      {
        this.m_curtile = -1;
        if ((this.m_flags & TiffFlags.MYBUFFER) != TiffFlags.MYBUFFER)
          return false;
        this.ReadBufferSetup((byte[]) null, Tiff.roundUp(num, 1024 /*0x0400*/));
      }
      if (this.readRawTile1(tile, this.m_rawdata, 0, num, nameof (fillTile)) != num)
        return false;
      if (!this.isFillOrder(this.m_dir.td_fillorder) && (this.m_flags & TiffFlags.NOBITREV) != TiffFlags.NOBITREV)
        Tiff.ReverseBits(this.m_rawdata, num);
    }
    return this.startTile(tile);
  }

  private int summarize(int summand1, int summand2, string where)
  {
    int num = summand1 + summand2;
    if (num - summand1 != summand2)
      num = 0;
    return num;
  }

  private int multiply(int nmemb, int elem_size, string where)
  {
    int num = nmemb * elem_size;
    if (elem_size != 0 && num / elem_size != nmemb)
      num = 0;
    return num;
  }

  internal int newScanlineSize()
  {
    int nmemb;
    if (this.m_dir.td_planarconfig == PlanarConfig.CONTIG)
    {
      if (this.m_dir.td_photometric == Photometric.YCBCR && !this.IsUpSampled())
      {
        FieldValue[] field = this.GetField(TiffTag.YCBCRSUBSAMPLING);
        ushort num1 = field[0].ToUShort();
        ushort num2 = field[1].ToUShort();
        return (int) num1 * (int) num2 == 0 ? 0 : ((this.m_dir.td_imagewidth + (int) num1 - 1) / (int) num1 * ((int) num1 * (int) num2 + 2) * (int) this.m_dir.td_bitspersample + 7) / 8 / (int) num2;
      }
      nmemb = this.multiply(this.m_dir.td_imagewidth, (int) this.m_dir.td_samplesperpixel, "TIFFScanlineSize");
    }
    else
      nmemb = this.m_dir.td_imagewidth;
    return Tiff.howMany8(this.multiply(nmemb, (int) this.m_dir.td_bitspersample, "TIFFScanlineSize"));
  }

  internal int oldScanlineSize()
  {
    int num = this.multiply((int) this.m_dir.td_bitspersample, this.m_dir.td_imagewidth, "TIFFScanlineSize");
    if (this.m_dir.td_planarconfig == PlanarConfig.CONTIG)
      num = this.multiply(num, (int) this.m_dir.td_samplesperpixel, "TIFFScanlineSize");
    return Tiff.howMany8(num);
  }

  public TiffCodec FindCodec(Syncfusion.Pdf.Compression.JBIG2.Compression scheme)
  {
    for (Tiff.codecList codecList = this.m_registeredCodecs; codecList != null; codecList = codecList.next)
    {
      if (codecList.codec.m_scheme == scheme)
        return codecList.codec;
    }
    for (int index = 0; this.m_builtInCodecs[index] != null; ++index)
    {
      TiffCodec builtInCodec = this.m_builtInCodecs[index];
      if (builtInCodec.m_scheme == scheme)
        return builtInCodec;
    }
    return (TiffCodec) null;
  }

  public bool IsCodecConfigured(Syncfusion.Pdf.Compression.JBIG2.Compression scheme)
  {
    TiffCodec codec = this.FindCodec(scheme);
    return codec != null && codec.CanDecode;
  }

  public static Tiff Open(string fileName, string mode)
  {
    return Tiff.Open(fileName, mode, (Tiff.TiffExtendProc) null);
  }

  public static Tiff ClientOpen(string name, string mode, object clientData, TiffStream stream)
  {
    return Tiff.ClientOpen(name, mode, clientData, stream, (Tiff.TiffExtendProc) null);
  }

  public void Close()
  {
    this.m_stream.Close(this.m_clientdata);
    if (this.m_fileStream == null)
      return;
    this.m_fileStream.Close();
  }

  public void Dispose()
  {
    this.Dispose(true);
    GC.SuppressFinalize((object) this);
  }

  public void MergeFieldInfo(TiffFieldInfo[] info, int count)
  {
    this.m_foundfield = (TiffFieldInfo) null;
    if (this.m_nfields <= 0)
      this.m_fieldinfo = new TiffFieldInfo[count];
    for (int index = 0; index < count; ++index)
    {
      if (this.FindFieldInfo(info[index].Tag, info[index].Type) == null)
      {
        this.m_fieldinfo = Tiff.Realloc(this.m_fieldinfo, this.m_nfields, this.m_nfields + 1);
        this.m_fieldinfo[this.m_nfields] = info[index];
        ++this.m_nfields;
      }
    }
    Array.Sort((Array) this.m_fieldinfo, 0, this.m_nfields, (IComparer) new TagCompare());
  }

  public TiffFieldInfo FindFieldInfo(TiffTag tag, TiffType type)
  {
    if (this.m_foundfield != null && this.m_foundfield.Tag == tag && (type == TiffType.NOTYPE || type == this.m_foundfield.Type))
      return this.m_foundfield;
    if (this.m_fieldinfo == null)
      return (TiffFieldInfo) null;
    this.m_foundfield = (TiffFieldInfo) null;
    foreach (TiffFieldInfo tiffFieldInfo in this.m_fieldinfo)
    {
      if (tiffFieldInfo != null && tiffFieldInfo.Tag == tag && (type == TiffType.NOTYPE || type == tiffFieldInfo.Type))
      {
        this.m_foundfield = tiffFieldInfo;
        break;
      }
    }
    return this.m_foundfield;
  }

  public TiffFieldInfo FindFieldInfoByName(string name, TiffType type)
  {
    if (this.m_foundfield != null && this.m_foundfield.Name == name && (type == TiffType.NOTYPE || type == this.m_foundfield.Type))
      return this.m_foundfield;
    if (this.m_fieldinfo == null)
      return (TiffFieldInfo) null;
    this.m_foundfield = (TiffFieldInfo) null;
    foreach (TiffFieldInfo tiffFieldInfo in this.m_fieldinfo)
    {
      if (tiffFieldInfo != null && tiffFieldInfo.Name == name && (type == TiffType.NOTYPE || type == tiffFieldInfo.Type))
      {
        this.m_foundfield = tiffFieldInfo;
        break;
      }
    }
    return this.m_foundfield;
  }

  public TiffFieldInfo FieldWithTag(TiffTag tag)
  {
    return this.FindFieldInfo(tag, TiffType.NOTYPE) ?? (TiffFieldInfo) null;
  }

  public FieldValue[] GetField(TiffTag tag)
  {
    TiffFieldInfo fieldInfo = this.FindFieldInfo(tag, TiffType.NOTYPE);
    return fieldInfo != null && (Tiff.isPseudoTag(tag) || this.fieldSet((int) fieldInfo.Bit)) ? this.m_tagmethods.GetField(this, tag) : (FieldValue[]) null;
  }

  public FieldValue[] GetFieldDefaulted(TiffTag tag)
  {
    TiffDirectory dir = this.m_dir;
    FieldValue[] fieldDefaulted = this.GetField(tag);
    if (fieldDefaulted != null)
      return fieldDefaulted;
    switch (tag)
    {
      case TiffTag.SUBFILETYPE:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) dir.td_subfiletype);
        break;
      case TiffTag.BITSPERSAMPLE:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) dir.td_bitspersample);
        break;
      case TiffTag.THRESHHOLDING:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) dir.td_threshholding);
        break;
      case TiffTag.FILLORDER:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) dir.td_fillorder);
        break;
      case TiffTag.ORIENTATION:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) dir.td_orientation);
        break;
      case TiffTag.SAMPLESPERPIXEL:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) dir.td_samplesperpixel);
        break;
      case TiffTag.ROWSPERSTRIP:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) dir.td_rowsperstrip);
        break;
      case TiffTag.MINSAMPLEVALUE:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) dir.td_minsamplevalue);
        break;
      case TiffTag.MAXSAMPLEVALUE:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) dir.td_maxsamplevalue);
        break;
      case TiffTag.PLANARCONFIG:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) dir.td_planarconfig);
        break;
      case TiffTag.RESOLUTIONUNIT:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) dir.td_resolutionunit);
        break;
      case TiffTag.TRANSFERFUNCTION:
        if (dir.td_transferfunction[0] == null && !Tiff.defaultTransferFunction(dir))
          return (FieldValue[]) null;
        fieldDefaulted = new FieldValue[3];
        fieldDefaulted[0].Set((object) dir.td_transferfunction[0]);
        if ((int) dir.td_samplesperpixel - (int) dir.td_extrasamples > 1)
        {
          fieldDefaulted[1].Set((object) dir.td_transferfunction[1]);
          fieldDefaulted[2].Set((object) dir.td_transferfunction[2]);
          break;
        }
        break;
      case TiffTag.PREDICTOR:
        if (this.m_currentCodec is CodecWithPredictor currentCodec)
        {
          fieldDefaulted = new FieldValue[1];
          fieldDefaulted[0].Set((object) currentCodec.GetPredictorValue());
          break;
        }
        break;
      case TiffTag.WHITEPOINT:
        float[] o1 = new float[2]
        {
          0.345741928f,
          0.358560443f
        };
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) o1);
        break;
      case TiffTag.INKSET:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) InkSet.CMYK);
        break;
      case TiffTag.NUMBEROFINKS:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) 4);
        break;
      case TiffTag.DOTRANGE:
        fieldDefaulted = new FieldValue[2];
        fieldDefaulted[0].Set((object) 0);
        fieldDefaulted[1].Set((object) ((1 << (int) dir.td_bitspersample) - 1));
        break;
      case TiffTag.EXTRASAMPLES:
        fieldDefaulted = new FieldValue[2];
        fieldDefaulted[0].Set((object) dir.td_extrasamples);
        fieldDefaulted[1].Set((object) dir.td_sampleinfo);
        break;
      case TiffTag.SAMPLEFORMAT:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) dir.td_sampleformat);
        break;
      case TiffTag.YCBCRCOEFFICIENTS:
        float[] o2 = new float[3]
        {
          0.299f,
          0.587f,
          57f / 500f
        };
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) o2);
        break;
      case TiffTag.YCBCRSUBSAMPLING:
        fieldDefaulted = new FieldValue[2];
        fieldDefaulted[0].Set((object) dir.td_ycbcrsubsampling[0]);
        fieldDefaulted[1].Set((object) dir.td_ycbcrsubsampling[1]);
        break;
      case TiffTag.YCBCRPOSITIONING:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) dir.td_ycbcrpositioning);
        break;
      case TiffTag.REFERENCEBLACKWHITE:
        if (dir.td_refblackwhite == null)
          Tiff.defaultRefBlackWhite(dir);
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) dir.td_refblackwhite);
        break;
      case TiffTag.MATTEING:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) (bool) (dir.td_extrasamples != (short) 1 ? 0 : (dir.td_sampleinfo[0] == ExtraSample.ASSOCALPHA ? 1 : 0)));
        break;
      case TiffTag.DATATYPE:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) (dir.td_sampleformat - 1));
        break;
      case TiffTag.IMAGEDEPTH:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) dir.td_imagedepth);
        break;
      case TiffTag.TILEDEPTH:
        fieldDefaulted = new FieldValue[1];
        fieldDefaulted[0].Set((object) dir.td_tiledepth);
        break;
    }
    return fieldDefaulted;
  }

  public bool ReadDirectory()
  {
    this.m_diroff = this.m_nextdiroff;
    if (this.m_diroff == 0U || !this.checkDirOffset(this.m_nextdiroff))
      return false;
    this.m_currentCodec.Cleanup();
    ++this.m_curdir;
    TiffDirEntry[] pdir;
    short dircount = this.fetchDirectory(this.m_nextdiroff, out pdir, out this.m_nextdiroff);
    if (dircount == (short) 0)
      return false;
    this.m_flags &= ~TiffFlags.BEENWRITING;
    this.FreeDirectory();
    this.setupDefaultDirectory();
    this.SetField(TiffTag.PLANARCONFIG, (object) PlanarConfig.CONTIG);
    for (int index = 0; index < (int) dircount; ++index)
    {
      TiffDirEntry tiffDirEntry = pdir[index];
      if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
      {
        short num = (short) tiffDirEntry.tdir_tag;
        Tiff.SwabShort(ref num);
        tiffDirEntry.tdir_tag = (TiffTag) (ushort) num;
        num = (short) tiffDirEntry.tdir_type;
        Tiff.SwabShort(ref num);
        tiffDirEntry.tdir_type = (TiffType) num;
        Tiff.SwabLong(ref tiffDirEntry.tdir_count);
        Tiff.SwabUInt(ref tiffDirEntry.tdir_offset);
      }
      if (tiffDirEntry.tdir_tag == TiffTag.SAMPLESPERPIXEL)
      {
        if (!this.fetchNormalTag(pdir[index]))
          return false;
        tiffDirEntry.tdir_tag = TiffTag.IGNORE;
      }
    }
    int index1 = 0;
    bool flag1 = false;
    bool flag2 = false;
    for (int index2 = 0; index2 < (int) dircount; ++index2)
    {
      if (pdir[index2].tdir_tag != TiffTag.IGNORE)
      {
        if (index1 >= this.m_nfields)
          index1 = 0;
        if (pdir[index2].tdir_tag < this.m_fieldinfo[index1].Tag)
        {
          if (!flag1)
            flag1 = true;
          index1 = 0;
        }
        while (index1 < this.m_nfields && this.m_fieldinfo[index1].Tag < pdir[index2].tdir_tag)
          ++index1;
        if (index1 >= this.m_nfields || this.m_fieldinfo[index1].Tag != pdir[index2].tdir_tag)
          flag2 = true;
        else if (this.m_fieldinfo[index1].Bit == (short) 0)
        {
          pdir[index2].tdir_tag = TiffTag.IGNORE;
        }
        else
        {
          TiffFieldInfo tiffFieldInfo = this.m_fieldinfo[index1];
          bool flag3 = false;
          while (pdir[index2].tdir_type != tiffFieldInfo.Type && index1 < this.m_nfields && tiffFieldInfo.Type != TiffType.NOTYPE)
          {
            tiffFieldInfo = this.m_fieldinfo[++index1];
            if (index1 >= this.m_nfields || tiffFieldInfo.Tag != pdir[index2].tdir_tag)
            {
              pdir[index2].tdir_tag = TiffTag.IGNORE;
              flag3 = true;
              break;
            }
          }
          if (!flag3)
          {
            if (tiffFieldInfo.ReadCount != (short) -1 && tiffFieldInfo.ReadCount != (short) -3)
            {
              int count = (int) tiffFieldInfo.ReadCount;
              if (tiffFieldInfo.ReadCount == (short) -2)
                count = (int) this.m_dir.td_samplesperpixel;
              if (!this.checkDirCount(pdir[index2], count))
              {
                pdir[index2].tdir_tag = TiffTag.IGNORE;
                continue;
              }
            }
            switch (pdir[index2].tdir_tag)
            {
              case TiffTag.IMAGEWIDTH:
              case TiffTag.IMAGELENGTH:
              case TiffTag.ROWSPERSTRIP:
              case TiffTag.PLANARCONFIG:
              case TiffTag.TILEWIDTH:
              case TiffTag.TILELENGTH:
              case TiffTag.EXTRASAMPLES:
              case TiffTag.IMAGEDEPTH:
              case TiffTag.TILEDEPTH:
                if (!this.fetchNormalTag(pdir[index2]))
                  return false;
                pdir[index2].tdir_tag = TiffTag.IGNORE;
                continue;
              case TiffTag.COMPRESSION:
                if (pdir[index2].tdir_count == 1)
                {
                  int data = this.extractData(pdir[index2]);
                  if (!this.SetField(pdir[index2].tdir_tag, (object) data))
                    return false;
                  continue;
                }
                if (pdir[index2].tdir_type == TiffType.LONG)
                {
                  int pl;
                  if (this.fetchPerSampleLongs(pdir[index2], out pl))
                  {
                    if (this.SetField(pdir[index2].tdir_tag, (object) pl))
                      goto label_48;
                  }
                  return false;
                }
                short pl1;
                if (this.fetchPerSampleShorts(pdir[index2], out pl1))
                {
                  if (this.SetField(pdir[index2].tdir_tag, (object) pl1))
                    goto label_48;
                }
                return false;
label_48:
                pdir[index2].tdir_tag = TiffTag.IGNORE;
                continue;
              case TiffTag.STRIPOFFSETS:
              case TiffTag.STRIPBYTECOUNTS:
              case TiffTag.TILEOFFSETS:
              case TiffTag.TILEBYTECOUNTS:
                this.setFieldBit((int) tiffFieldInfo.Bit);
                continue;
              default:
                continue;
            }
          }
        }
      }
    }
    if (flag2)
    {
      int index3 = 0;
      for (int index4 = 0; index4 < (int) dircount; ++index4)
      {
        if (pdir[index4].tdir_tag != TiffTag.IGNORE)
        {
          if (index3 >= this.m_nfields || pdir[index4].tdir_tag < this.m_fieldinfo[index3].Tag)
            index3 = 0;
          while (index3 < this.m_nfields && this.m_fieldinfo[index3].Tag < pdir[index4].tdir_tag)
            ++index3;
          if (index3 >= this.m_nfields || this.m_fieldinfo[index3].Tag != pdir[index4].tdir_tag)
          {
            this.MergeFieldInfo(new TiffFieldInfo[1]
            {
              Tiff.createAnonFieldInfo(pdir[index4].tdir_tag, pdir[index4].tdir_type)
            }, 1);
            index3 = 0;
            while (index3 < this.m_nfields && this.m_fieldinfo[index3].Tag < pdir[index4].tdir_tag)
              ++index3;
          }
          TiffFieldInfo tiffFieldInfo = this.m_fieldinfo[index3];
          while (pdir[index4].tdir_type != tiffFieldInfo.Type && index3 < this.m_nfields && tiffFieldInfo.Type != TiffType.NOTYPE)
          {
            tiffFieldInfo = this.m_fieldinfo[++index3];
            if (index3 >= this.m_nfields || tiffFieldInfo.Tag != pdir[index4].tdir_tag)
            {
              pdir[index4].tdir_tag = TiffTag.IGNORE;
              break;
            }
          }
        }
      }
    }
    if (this.m_dir.td_compression == Syncfusion.Pdf.Compression.JBIG2.Compression.OJPEG && this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
    {
      int index5 = Tiff.readDirectoryFind(pdir, dircount, TiffTag.STRIPOFFSETS);
      if (index5 != -1 && pdir[index5].tdir_count == 1)
      {
        int index6 = Tiff.readDirectoryFind(pdir, dircount, TiffTag.STRIPBYTECOUNTS);
        if (index6 != -1 && pdir[index6].tdir_count == 1)
          this.m_dir.td_planarconfig = PlanarConfig.CONTIG;
      }
    }
    if (!this.fieldSet(1))
    {
      this.missingRequired("ImageLength");
      return false;
    }
    if (!this.fieldSet(2))
    {
      this.m_dir.td_nstrips = this.NumberOfStrips();
      this.m_dir.td_tilewidth = this.m_dir.td_imagewidth;
      this.m_dir.td_tilelength = this.m_dir.td_rowsperstrip;
      this.m_dir.td_tiledepth = this.m_dir.td_imagedepth;
      this.m_flags &= ~TiffFlags.ISTILED;
    }
    else
    {
      this.m_dir.td_nstrips = this.NumberOfTiles();
      this.m_flags |= TiffFlags.ISTILED;
    }
    if (this.m_dir.td_nstrips == 0)
      return false;
    this.m_dir.td_stripsperimage = this.m_dir.td_nstrips;
    if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
      this.m_dir.td_stripsperimage /= (int) this.m_dir.td_samplesperpixel;
    if (!this.fieldSet(25))
    {
      if (this.m_dir.td_compression == Syncfusion.Pdf.Compression.JBIG2.Compression.OJPEG && !this.IsTiled() && this.m_dir.td_nstrips == 1)
      {
        this.setFieldBit(25);
      }
      else
      {
        this.missingRequired(this.IsTiled() ? "TileOffsets" : "StripOffsets");
        return false;
      }
    }
    for (int index7 = 0; index7 < (int) dircount; ++index7)
    {
      if (pdir[index7].tdir_tag != TiffTag.IGNORE)
      {
        switch (pdir[index7].tdir_tag)
        {
          case TiffTag.OSUBFILETYPE:
            FileType fileType = (FileType) 0;
            switch (this.extractData(pdir[index7]))
            {
              case 2:
                fileType = FileType.REDUCEDIMAGE;
                break;
              case 3:
                fileType = FileType.PAGE;
                break;
            }
            if (fileType != (FileType) 0)
            {
              this.SetField(TiffTag.SUBFILETYPE, (object) fileType);
              continue;
            }
            continue;
          case TiffTag.BITSPERSAMPLE:
          case TiffTag.MINSAMPLEVALUE:
          case TiffTag.MAXSAMPLEVALUE:
          case TiffTag.SAMPLEFORMAT:
          case TiffTag.DATATYPE:
            if (pdir[index7].tdir_count == 1)
            {
              int data = this.extractData(pdir[index7]);
              if (!this.SetField(pdir[index7].tdir_tag, (object) data))
                return false;
              continue;
            }
            if (pdir[index7].tdir_tag == TiffTag.BITSPERSAMPLE && pdir[index7].tdir_type == TiffType.LONG)
            {
              int pl;
              if (this.fetchPerSampleLongs(pdir[index7], out pl))
              {
                if (this.SetField(pdir[index7].tdir_tag, (object) pl))
                  continue;
              }
              return false;
            }
            short pl2;
            if (this.fetchPerSampleShorts(pdir[index7], out pl2))
            {
              if (this.SetField(pdir[index7].tdir_tag, (object) pl2))
                continue;
            }
            return false;
          case TiffTag.STRIPOFFSETS:
          case TiffTag.TILEOFFSETS:
            if (!this.fetchStripThing(pdir[index7], this.m_dir.td_nstrips, ref this.m_dir.td_stripoffset))
              return false;
            continue;
          case TiffTag.STRIPBYTECOUNTS:
          case TiffTag.TILEBYTECOUNTS:
            if (!this.fetchStripThing(pdir[index7], this.m_dir.td_nstrips, ref this.m_dir.td_stripbytecount))
              return false;
            continue;
          case TiffTag.PAGENUMBER:
          case TiffTag.HALFTONEHINTS:
          case TiffTag.DOTRANGE:
          case TiffTag.YCBCRSUBSAMPLING:
            this.fetchShortPair(pdir[index7]);
            continue;
          case TiffTag.TRANSFERFUNCTION:
          case TiffTag.COLORMAP:
            int num1 = 1 << (int) this.m_dir.td_bitspersample;
            if (pdir[index7].tdir_tag != TiffTag.COLORMAP && pdir[index7].tdir_count == num1 || this.checkDirCount(pdir[index7], 3 * num1))
            {
              byte[] buffer = new byte[pdir[index7].tdir_count * 2];
              if (this.fetchData(pdir[index7], buffer) != 0)
              {
                int num2 = 1 << (int) this.m_dir.td_bitspersample;
                if (pdir[index7].tdir_count == num2)
                {
                  short[] shorts = Tiff.ByteArrayToShorts(buffer, 0, pdir[index7].tdir_count * 2);
                  this.SetField(pdir[index7].tdir_tag, (object) shorts, (object) shorts, (object) shorts);
                  continue;
                }
                int num3 = num1 * 2;
                short[] shorts1 = Tiff.ByteArrayToShorts(buffer, 0, num3);
                short[] shorts2 = Tiff.ByteArrayToShorts(buffer, num3, num3);
                short[] shorts3 = Tiff.ByteArrayToShorts(buffer, 2 * num3, num3);
                this.SetField(pdir[index7].tdir_tag, (object) shorts1, (object) shorts2, (object) shorts3);
                continue;
              }
              continue;
            }
            continue;
          case TiffTag.SMINSAMPLEVALUE:
          case TiffTag.SMAXSAMPLEVALUE:
            double pl3;
            if (this.fetchPerSampleAnys(pdir[index7], out pl3))
            {
              if (this.SetField(pdir[index7].tdir_tag, (object) pl3))
                continue;
            }
            return false;
          case TiffTag.REFERENCEBLACKWHITE:
            this.fetchRefBlackWhite(pdir[index7]);
            continue;
          default:
            this.fetchNormalTag(pdir[index7]);
            continue;
        }
      }
    }
    if (this.m_dir.td_compression == Syncfusion.Pdf.Compression.JBIG2.Compression.OJPEG)
    {
      if (!this.fieldSet(8))
      {
        if (!this.SetField(TiffTag.PHOTOMETRIC, (object) Photometric.YCBCR))
          return false;
      }
      else if (this.m_dir.td_photometric == Photometric.RGB)
        this.m_dir.td_photometric = Photometric.YCBCR;
      if (!this.fieldSet(6))
      {
        if (!this.SetField(TiffTag.BITSPERSAMPLE, (object) 8))
          return false;
      }
      if (!this.fieldSet(16 /*0x10*/))
      {
        if (this.m_dir.td_photometric == Photometric.RGB || this.m_dir.td_photometric == Photometric.YCBCR)
        {
          if (!this.SetField(TiffTag.SAMPLESPERPIXEL, (object) 3))
            return false;
        }
        else if (this.m_dir.td_photometric == Photometric.MINISWHITE || this.m_dir.td_photometric == Photometric.MINISBLACK)
        {
          if (!this.SetField(TiffTag.SAMPLESPERPIXEL, (object) 1))
            return false;
        }
      }
    }
    if (this.m_dir.td_photometric == Photometric.PALETTE && !this.fieldSet(26))
    {
      this.missingRequired("Colormap");
      return false;
    }
    if (this.m_dir.td_compression != Syncfusion.Pdf.Compression.JBIG2.Compression.OJPEG)
    {
      if (!this.fieldSet(24))
      {
        if (this.m_dir.td_planarconfig == PlanarConfig.CONTIG && this.m_dir.td_nstrips > 1 || this.m_dir.td_planarconfig == PlanarConfig.SEPARATE && this.m_dir.td_nstrips != (int) this.m_dir.td_samplesperpixel)
        {
          this.missingRequired("StripByteCounts");
          return false;
        }
        if (!this.estimateStripByteCounts(pdir, dircount))
          return false;
      }
      else if (this.m_dir.td_nstrips == 1 && this.m_dir.td_stripoffset[0] != 0U && this.byteCountLooksBad(this.m_dir))
      {
        if (!this.estimateStripByteCounts(pdir, dircount))
          return false;
      }
      else if (this.m_dir.td_planarconfig == PlanarConfig.CONTIG && this.m_dir.td_nstrips > 2 && this.m_dir.td_compression == Syncfusion.Pdf.Compression.JBIG2.Compression.NONE && (int) this.m_dir.td_stripbytecount[0] != (int) this.m_dir.td_stripbytecount[1] && !this.estimateStripByteCounts(pdir, dircount))
        return false;
    }
    pdir = (TiffDirEntry[]) null;
    if (!this.fieldSet(19))
      this.m_dir.td_maxsamplevalue = (ushort) ((1 << (int) this.m_dir.td_bitspersample) - 1);
    if (this.m_dir.td_nstrips > 1)
    {
      this.m_dir.td_stripbytecountsorted = true;
      for (int index8 = 1; index8 < this.m_dir.td_nstrips; ++index8)
      {
        if (this.m_dir.td_stripoffset[index8 - 1] > this.m_dir.td_stripoffset[index8])
        {
          this.m_dir.td_stripbytecountsorted = false;
          break;
        }
      }
    }
    if (!this.fieldSet(7))
      this.SetField(TiffTag.COMPRESSION, (object) Syncfusion.Pdf.Compression.JBIG2.Compression.NONE);
    if (this.m_dir.td_nstrips == 1 && this.m_dir.td_compression == Syncfusion.Pdf.Compression.JBIG2.Compression.NONE && (this.m_flags & TiffFlags.STRIPCHOP) == TiffFlags.STRIPCHOP && (this.m_flags & TiffFlags.ISTILED) != TiffFlags.ISTILED)
      this.chopUpSingleUncompressedStrip();
    this.m_row = -1;
    this.m_curstrip = -1;
    this.m_col = -1;
    this.m_curtile = -1;
    this.m_tilesize = -1;
    this.m_scanlinesize = this.ScanlineSize();
    if (this.m_scanlinesize == 0)
      return false;
    if (this.IsTiled())
    {
      this.m_tilesize = this.TileSize();
      if (this.m_tilesize == 0)
        return false;
    }
    else if (this.StripSize() == 0)
      return false;
    return true;
  }

  public bool ReadCustomDirectory(long offset, TiffFieldInfo[] info, int count)
  {
    this.setupFieldInfo(info, count);
    TiffDirEntry[] pdir;
    short num1 = this.fetchDirectory((uint) offset, out pdir, out uint _);
    if (num1 == (short) 0)
      return false;
    this.FreeDirectory();
    this.m_dir = new TiffDirectory();
    int index1 = 0;
    for (short index2 = 0; (int) index2 < (int) num1; ++index2)
    {
      if ((this.m_flags & TiffFlags.SWAB) == TiffFlags.SWAB)
      {
        short num2 = (short) pdir[(int) index2].tdir_tag;
        Tiff.SwabShort(ref num2);
        pdir[(int) index2].tdir_tag = (TiffTag) (ushort) num2;
        num2 = (short) pdir[(int) index2].tdir_type;
        Tiff.SwabShort(ref num2);
        pdir[(int) index2].tdir_type = (TiffType) num2;
        Tiff.SwabLong(ref pdir[(int) index2].tdir_count);
        Tiff.SwabUInt(ref pdir[(int) index2].tdir_offset);
      }
      if (index1 < this.m_nfields && pdir[(int) index2].tdir_tag != TiffTag.IGNORE)
      {
        while (index1 < this.m_nfields && this.m_fieldinfo[index1].Tag < pdir[(int) index2].tdir_tag)
          ++index1;
        if (index1 >= this.m_nfields || this.m_fieldinfo[index1].Tag != pdir[(int) index2].tdir_tag)
        {
          this.MergeFieldInfo(new TiffFieldInfo[1]
          {
            Tiff.createAnonFieldInfo(pdir[(int) index2].tdir_tag, pdir[(int) index2].tdir_type)
          }, 1);
          index1 = 0;
          while (index1 < this.m_nfields && this.m_fieldinfo[index1].Tag < pdir[(int) index2].tdir_tag)
            ++index1;
        }
        if (this.m_fieldinfo[index1].Bit == (short) 0)
        {
          pdir[(int) index2].tdir_tag = TiffTag.IGNORE;
        }
        else
        {
          TiffFieldInfo tiffFieldInfo = this.m_fieldinfo[index1];
          while (pdir[(int) index2].tdir_type != tiffFieldInfo.Type && index1 < this.m_nfields && tiffFieldInfo.Type != TiffType.NOTYPE)
          {
            tiffFieldInfo = this.m_fieldinfo[++index1];
            if (index1 >= this.m_nfields || tiffFieldInfo.Tag != pdir[(int) index2].tdir_tag)
              pdir[(int) index2].tdir_tag = TiffTag.IGNORE;
          }
          if (tiffFieldInfo.ReadCount != (short) -1 && tiffFieldInfo.ReadCount != (short) -3)
          {
            int count1 = (int) tiffFieldInfo.ReadCount;
            if (tiffFieldInfo.ReadCount == (short) -2)
              count1 = (int) this.m_dir.td_samplesperpixel;
            if (!this.checkDirCount(pdir[(int) index2], count1))
            {
              pdir[(int) index2].tdir_tag = TiffTag.IGNORE;
              continue;
            }
          }
          if (pdir[(int) index2].tdir_tag == TiffTag.EXIF_SUBJECTDISTANCE)
            this.fetchSubjectDistance(pdir[(int) index2]);
          else
            this.fetchNormalTag(pdir[(int) index2]);
        }
      }
    }
    return true;
  }

  public int ScanlineSize()
  {
    int nmemb;
    if (this.m_dir.td_planarconfig == PlanarConfig.CONTIG)
    {
      if (this.m_dir.td_photometric == Photometric.YCBCR && !this.IsUpSampled())
      {
        short y = this.GetFieldDefaulted(TiffTag.YCBCRSUBSAMPLING)[0].ToShort();
        if (y == (short) 0)
          return 0;
        int summand1 = Tiff.howMany8(this.multiply(Tiff.roundUp(this.m_dir.td_imagewidth, (int) y), (int) this.m_dir.td_bitspersample, nameof (ScanlineSize)));
        return this.summarize(summand1, this.multiply(2, summand1 / (int) y, "VStripSize"), "VStripSize");
      }
      nmemb = this.multiply(this.m_dir.td_imagewidth, (int) this.m_dir.td_samplesperpixel, nameof (ScanlineSize));
    }
    else
      nmemb = this.m_dir.td_imagewidth;
    return Tiff.howMany8(this.multiply(nmemb, (int) this.m_dir.td_bitspersample, nameof (ScanlineSize)));
  }

  public int StripSize()
  {
    int rowCount = this.m_dir.td_rowsperstrip;
    if (rowCount > this.m_dir.td_imagelength)
      rowCount = this.m_dir.td_imagelength;
    return this.VStripSize(rowCount);
  }

  public int VStripSize(int rowCount)
  {
    if (rowCount == -1)
      rowCount = this.m_dir.td_imagelength;
    if (this.m_dir.td_planarconfig != PlanarConfig.CONTIG || this.m_dir.td_photometric != Photometric.YCBCR || this.IsUpSampled())
      return this.multiply(rowCount, this.ScanlineSize(), nameof (VStripSize));
    FieldValue[] fieldDefaulted = this.GetFieldDefaulted(TiffTag.YCBCRSUBSAMPLING);
    short y1 = fieldDefaulted[0].ToShort();
    short y2 = fieldDefaulted[1].ToShort();
    int num = (int) y1 * (int) y2;
    if (num == 0)
      return 0;
    int elem_size = Tiff.howMany8(this.multiply(Tiff.roundUp(this.m_dir.td_imagewidth, (int) y1), (int) this.m_dir.td_bitspersample, nameof (VStripSize)));
    rowCount = Tiff.roundUp(rowCount, (int) y2);
    int summand1 = this.multiply(rowCount, elem_size, nameof (VStripSize));
    return this.summarize(summand1, this.multiply(2, summand1 / num, nameof (VStripSize)), nameof (VStripSize));
  }

  public long RawStripSize(int strip)
  {
    long num = (long) this.m_dir.td_stripbytecount[strip];
    if (num <= 0L)
      num = -1L;
    return num;
  }

  public int NumberOfStrips()
  {
    int nmemb = this.m_dir.td_rowsperstrip == -1 ? 1 : Tiff.howMany(this.m_dir.td_imagelength, this.m_dir.td_rowsperstrip);
    if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
      nmemb = this.multiply(nmemb, (int) this.m_dir.td_samplesperpixel, nameof (NumberOfStrips));
    return nmemb;
  }

  public int TileSize() => this.VTileSize(this.m_dir.td_tilelength);

  public int VTileSize(int rowCount)
  {
    if (this.m_dir.td_tilelength == 0 || this.m_dir.td_tilewidth == 0 || this.m_dir.td_tiledepth == 0)
      return 0;
    int nmemb;
    if (this.m_dir.td_planarconfig == PlanarConfig.CONTIG && this.m_dir.td_photometric == Photometric.YCBCR && !this.IsUpSampled())
    {
      int elem_size = Tiff.howMany8(this.multiply(Tiff.roundUp(this.m_dir.td_tilewidth, (int) this.m_dir.td_ycbcrsubsampling[0]), (int) this.m_dir.td_bitspersample, nameof (VTileSize)));
      int num = (int) this.m_dir.td_ycbcrsubsampling[0] * (int) this.m_dir.td_ycbcrsubsampling[1];
      if (num == 0)
        return 0;
      rowCount = Tiff.roundUp(rowCount, (int) this.m_dir.td_ycbcrsubsampling[1]);
      int summand1 = this.multiply(rowCount, elem_size, nameof (VTileSize));
      nmemb = this.summarize(summand1, this.multiply(2, summand1 / num, nameof (VTileSize)), nameof (VTileSize));
    }
    else
      nmemb = this.multiply(rowCount, this.TileRowSize(), nameof (VTileSize));
    return this.multiply(nmemb, this.m_dir.td_tiledepth, nameof (VTileSize));
  }

  public int TileRowSize()
  {
    if (this.m_dir.td_tilelength == 0 || this.m_dir.td_tilewidth == 0)
      return 0;
    int num = this.multiply((int) this.m_dir.td_bitspersample, this.m_dir.td_tilewidth, nameof (TileRowSize));
    if (this.m_dir.td_planarconfig == PlanarConfig.CONTIG)
      num = this.multiply(num, (int) this.m_dir.td_samplesperpixel, nameof (TileRowSize));
    return Tiff.howMany8(num);
  }

  public int ComputeTile(int x, int y, int z, short plane)
  {
    if (this.m_dir.td_imagedepth == 1)
      z = 0;
    int y1 = this.m_dir.td_tilewidth;
    if (y1 == -1)
      y1 = this.m_dir.td_imagewidth;
    int y2 = this.m_dir.td_tilelength;
    if (y2 == -1)
      y2 = this.m_dir.td_imagelength;
    int y3 = this.m_dir.td_tiledepth;
    if (y3 == -1)
      y3 = this.m_dir.td_imagedepth;
    int tile = 1;
    if (y1 != 0 && y2 != 0 && y3 != 0)
    {
      int num1 = Tiff.howMany(this.m_dir.td_imagewidth, y1);
      int num2 = Tiff.howMany(this.m_dir.td_imagelength, y2);
      int num3 = Tiff.howMany(this.m_dir.td_imagedepth, y3);
      tile = this.m_dir.td_planarconfig != PlanarConfig.SEPARATE ? num1 * num2 * (z / y3) + num1 * (y / y2) + x / y1 : num1 * num2 * num3 * (int) plane + num1 * num2 * (z / y3) + num1 * (y / y2) + x / y1;
    }
    return tile;
  }

  public bool CheckTile(int x, int y, int z, short plane)
  {
    return x < this.m_dir.td_imagewidth && y < this.m_dir.td_imagelength && z < this.m_dir.td_imagedepth && (this.m_dir.td_planarconfig != PlanarConfig.SEPARATE || (int) plane < (int) this.m_dir.td_samplesperpixel);
  }

  public int NumberOfTiles()
  {
    int y1 = this.m_dir.td_tilewidth;
    if (y1 == -1)
      y1 = this.m_dir.td_imagewidth;
    int y2 = this.m_dir.td_tilelength;
    if (y2 == -1)
      y2 = this.m_dir.td_imagelength;
    int y3 = this.m_dir.td_tiledepth;
    if (y3 == -1)
      y3 = this.m_dir.td_imagedepth;
    int nmemb = y1 == 0 || y2 == 0 || y3 == 0 ? 0 : this.multiply(this.multiply(Tiff.howMany(this.m_dir.td_imagewidth, y1), Tiff.howMany(this.m_dir.td_imagelength, y2), nameof (NumberOfTiles)), Tiff.howMany(this.m_dir.td_imagedepth, y3), nameof (NumberOfTiles));
    if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
      nmemb = this.multiply(nmemb, (int) this.m_dir.td_samplesperpixel, nameof (NumberOfTiles));
    return nmemb;
  }

  public bool IsTiled() => (this.m_flags & TiffFlags.ISTILED) == TiffFlags.ISTILED;

  public bool IsUpSampled() => (this.m_flags & TiffFlags.UPSAMPLED) == TiffFlags.UPSAMPLED;

  public TiffStream GetStream() => this.m_stream;

  public void ReadBufferSetup(byte[] buffer, int size)
  {
    this.m_rawdata = (byte[]) null;
    if (buffer != null)
    {
      this.m_rawdatasize = size;
      this.m_rawdata = buffer;
      this.m_flags &= ~TiffFlags.MYBUFFER;
    }
    else
    {
      this.m_rawdatasize = Tiff.roundUp(size, 1024 /*0x0400*/);
      if (this.m_rawdatasize > 0)
        this.m_rawdata = new byte[this.m_rawdatasize];
      else
        this.m_rawdatasize = 0;
      this.m_flags |= TiffFlags.MYBUFFER;
    }
  }

  public bool SetupStrips()
  {
    this.m_dir.td_nstrips = this.m_dir.td_stripsperimage;
    if (this.m_dir.td_planarconfig == PlanarConfig.SEPARATE)
      this.m_dir.td_stripsperimage /= (int) this.m_dir.td_samplesperpixel;
    this.m_dir.td_stripoffset = new uint[this.m_dir.td_nstrips];
    this.m_dir.td_stripbytecount = new uint[this.m_dir.td_nstrips];
    this.setFieldBit(25);
    this.setFieldBit(24);
    return true;
  }

  public void FreeDirectory()
  {
    if (this.m_dir == null)
      return;
    this.clearFieldBit(39);
    this.clearFieldBit(40);
    this.m_dir = (TiffDirectory) null;
  }

  public bool SetField(TiffTag tag, params object[] value)
  {
    return this.okToChangeTag(tag) && this.m_tagmethods.SetField(this, tag, FieldValue.FromParams(value));
  }

  public bool ReadScanline(byte[] buffer, int row, short plane)
  {
    return this.ReadScanline(buffer, 0, row, plane);
  }

  public bool ReadScanline(byte[] buffer, int offset, int row, short plane)
  {
    if (!this.checkRead(false))
      return false;
    bool flag = this.seek(row, plane);
    if (flag)
    {
      flag = this.m_currentCodec.DecodeRow(buffer, offset, this.m_scanlinesize, plane);
      this.m_row = row + 1;
      if (flag)
        this.postDecode(buffer, offset, this.m_scanlinesize);
    }
    return flag;
  }

  public static int DataWidth(TiffType type)
  {
    switch (type)
    {
      case TiffType.NOTYPE:
      case TiffType.BYTE:
      case TiffType.ASCII:
      case TiffType.SBYTE:
      case TiffType.UNDEFINED:
        return 1;
      case TiffType.SHORT:
      case TiffType.SSHORT:
        return 2;
      case TiffType.LONG:
      case TiffType.SLONG:
      case TiffType.FLOAT:
      case TiffType.IFD:
        return 4;
      case TiffType.RATIONAL:
      case TiffType.SRATIONAL:
      case TiffType.DOUBLE:
        return 8;
      default:
        return 0;
    }
  }

  public static void SwabShort(ref short value)
  {
    byte[] numArray = new byte[2]
    {
      (byte) value,
      (byte) ((uint) value >> 8)
    };
    byte num = numArray[1];
    numArray[1] = numArray[0];
    numArray[0] = num;
    value = (short) ((int) numArray[0] & (int) byte.MaxValue);
    value += (short) (((int) numArray[1] & (int) byte.MaxValue) << 8);
  }

  public static void SwabLong(ref int value)
  {
    byte[] numArray = new byte[4]
    {
      (byte) value,
      (byte) (value >> 8),
      (byte) (value >> 16 /*0x10*/),
      (byte) (value >> 24)
    };
    byte num1 = numArray[3];
    numArray[3] = numArray[0];
    numArray[0] = num1;
    byte num2 = numArray[2];
    numArray[2] = numArray[1];
    numArray[1] = num2;
    value = (int) numArray[0] & (int) byte.MaxValue;
    value += ((int) numArray[1] & (int) byte.MaxValue) << 8;
    value += ((int) numArray[2] & (int) byte.MaxValue) << 16 /*0x10*/;
    value += (int) numArray[3] << 24;
  }

  public static void SwabArrayOfShort(short[] array, int count)
  {
    Tiff.SwabArrayOfShort(array, 0, count);
  }

  public static void SwabArrayOfShort(short[] array, int offset, int count)
  {
    byte[] numArray = new byte[2];
    int num1 = 0;
    while (num1 < count)
    {
      numArray[0] = (byte) array[offset];
      numArray[1] = (byte) ((uint) array[offset] >> 8);
      byte num2 = numArray[1];
      numArray[1] = numArray[0];
      numArray[0] = num2;
      array[offset] = (short) ((int) numArray[0] & (int) byte.MaxValue);
      array[offset] += (short) (((int) numArray[1] & (int) byte.MaxValue) << 8);
      ++num1;
      ++offset;
    }
  }

  public static void SwabArrayOfTriples(byte[] array, int offset, int count)
  {
    while (count-- > 0)
    {
      byte num = array[offset + 2];
      array[offset + 2] = array[offset];
      array[offset] = num;
      offset += 3;
    }
  }

  public static void SwabArrayOfLong(int[] array, int count)
  {
    Tiff.SwabArrayOfLong(array, 0, count);
  }

  public static void SwabArrayOfLong(int[] array, int offset, int count)
  {
    byte[] numArray = new byte[4];
    int num1 = 0;
    while (num1 < count)
    {
      numArray[0] = (byte) array[offset];
      numArray[1] = (byte) (array[offset] >> 8);
      numArray[2] = (byte) (array[offset] >> 16 /*0x10*/);
      numArray[3] = (byte) (array[offset] >> 24);
      byte num2 = numArray[3];
      numArray[3] = numArray[0];
      numArray[0] = num2;
      byte num3 = numArray[2];
      numArray[2] = numArray[1];
      numArray[1] = num3;
      array[offset] = (int) numArray[0] & (int) byte.MaxValue;
      array[offset] += ((int) numArray[1] & (int) byte.MaxValue) << 8;
      array[offset] += ((int) numArray[2] & (int) byte.MaxValue) << 16 /*0x10*/;
      array[offset] += (int) numArray[3] << 24;
      ++num1;
      ++offset;
    }
  }

  public static void SwabArrayOfDouble(double[] array, int count)
  {
    Tiff.SwabArrayOfDouble(array, 0, count);
  }

  public static void SwabArrayOfDouble(double[] array, int offset, int count)
  {
    int[] numArray = new int[count * 8 / 4];
    Buffer.BlockCopy((Array) array, offset * 8, (Array) numArray, 0, numArray.Length * 4);
    Tiff.SwabArrayOfLong(numArray, numArray.Length);
    int index = 0;
    while (count-- > 0)
    {
      int num = numArray[index];
      numArray[index] = numArray[index + 1];
      numArray[index + 1] = num;
      index += 2;
    }
    Buffer.BlockCopy((Array) numArray, 0, (Array) array, offset * 8, numArray.Length * 4);
  }

  public static void ReverseBits(byte[] buffer, int count) => Tiff.ReverseBits(buffer, 0, count);

  public static void ReverseBits(byte[] buffer, int offset, int count)
  {
    for (; count > 8; count -= 8)
    {
      buffer[offset] = Tiff.TIFFBitRevTable[(int) buffer[offset]];
      buffer[offset + 1] = Tiff.TIFFBitRevTable[(int) buffer[offset + 1]];
      buffer[offset + 2] = Tiff.TIFFBitRevTable[(int) buffer[offset + 2]];
      buffer[offset + 3] = Tiff.TIFFBitRevTable[(int) buffer[offset + 3]];
      buffer[offset + 4] = Tiff.TIFFBitRevTable[(int) buffer[offset + 4]];
      buffer[offset + 5] = Tiff.TIFFBitRevTable[(int) buffer[offset + 5]];
      buffer[offset + 6] = Tiff.TIFFBitRevTable[(int) buffer[offset + 6]];
      buffer[offset + 7] = Tiff.TIFFBitRevTable[(int) buffer[offset + 7]];
      offset += 8;
    }
    while (count-- > 0)
    {
      buffer[offset] = Tiff.TIFFBitRevTable[(int) buffer[offset]];
      ++offset;
    }
  }

  public static byte[] GetBitRevTable(bool reversed)
  {
    return !reversed ? Tiff.TIFFNoBitRevTable : Tiff.TIFFBitRevTable;
  }

  public static int[] ByteArrayToInts(byte[] buffer, int offset, int count)
  {
    int length = count / 4;
    int[] dst = new int[length];
    Buffer.BlockCopy((Array) buffer, offset, (Array) dst, 0, length * 4);
    return dst;
  }

  public static void IntsToByteArray(
    int[] source,
    int srcOffset,
    int srcCount,
    byte[] bytes,
    int offset)
  {
    Buffer.BlockCopy((Array) source, srcOffset * 4, (Array) bytes, offset, srcCount * 4);
  }

  public static short[] ByteArrayToShorts(byte[] buffer, int offset, int count)
  {
    int length = count / 2;
    short[] dst = new short[length];
    Buffer.BlockCopy((Array) buffer, offset, (Array) dst, 0, length * 2);
    return dst;
  }

  public static void ShortsToByteArray(
    short[] source,
    int srcOffset,
    int srcCount,
    byte[] bytes,
    int offset)
  {
    Buffer.BlockCopy((Array) source, srcOffset * 2, (Array) bytes, offset, srcCount * 2);
  }

  internal enum PostDecodeMethodType
  {
    pdmNone,
    pdmSwab16Bit,
    pdmSwab24Bit,
    pdmSwab32Bit,
    pdmSwab64Bit,
  }

  private class codecList
  {
    public Tiff.codecList next;
    public TiffCodec codec;
  }

  private class clientInfoLink
  {
    public Tiff.clientInfoLink next;
    public object data;
    public string name;
  }

  public delegate void TiffExtendProc(Tiff tif);

  public delegate void FaxFillFunc(
    byte[] buffer,
    int offset,
    int[] runs,
    int thisRunOffset,
    int nextRunOffset,
    int width);
}
