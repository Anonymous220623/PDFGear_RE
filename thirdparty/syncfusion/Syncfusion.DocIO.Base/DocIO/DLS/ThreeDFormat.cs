// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ThreeDFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class ThreeDFormat
{
  internal const byte ContourWidthKey = 0;
  internal const byte ExtrusionHeightKey = 1;
  internal const byte PresetMaterialTypeKey = 2;
  internal const byte DistanceFromGroundKey = 3;
  internal const byte BevelBottomHeightKey = 4;
  internal const byte BevelBottomWidthKey = 5;
  internal const byte BevelBPresetTypeKey = 6;
  internal const byte BevelTopHeightKey = 7;
  internal const byte BevelTopWidthKey = 8;
  internal const byte BevelTPresetTypeKey = 9;
  internal const byte ContourColorKey = 10;
  internal const byte ContourOpacityKey = 11;
  internal const byte ExtrusionColorKey = 12;
  internal const byte ExtrusionOpacityKey = 13;
  internal const byte BackdropAnchorXKey = 14;
  internal const byte BackdropAnchorYKey = 15;
  internal const byte BackdropAnchorZKey = 16 /*0x10*/;
  internal const byte BackdropNormalXKey = 17;
  internal const byte BackdropNormalYKey = 18;
  internal const byte BackdropNormalZKey = 19;
  internal const byte BackdropUpXKey = 20;
  internal const byte BackdropUpYKey = 21;
  internal const byte BackdropUpZKey = 22;
  internal const byte FieldOfViewKey = 23;
  internal const byte CameraPresetTypeKey = 24;
  internal const byte ZoomKey = 25;
  internal const byte CameraRotationXKey = 26;
  internal const byte CameraRotationYKey = 27;
  internal const byte CameraRotationZKey = 28;
  internal const byte LightRigTypeKey = 29;
  internal const byte LightRigDirectionKey = 30;
  internal const byte LightRigRotationXKey = 31 /*0x1F*/;
  internal const byte LightRigRotationYKey = 32 /*0x20*/;
  internal const byte LightRigRotationZKey = 33;
  internal const byte BrightnessKey = 34;
  internal const byte ColorModeKey = 36;
  internal const byte DiffusityKey = 37;
  internal const byte EdgeKey = 38;
  internal const byte FacetKey = 39;
  internal const byte ForeDepthKey = 40;
  internal const byte LightLevelKey = 41;
  internal const byte LightLevel2Key = 42;
  internal const byte LightRigRotation2XKey = 43;
  internal const byte LightRigRotation2YKey = 44;
  internal const byte LightRigRotation2ZKey = 45;
  internal const byte RotationXKey = 46;
  internal const byte RotationYKey = 47;
  internal const byte RotationZKey = 48 /*0x30*/;
  internal const byte OrientationAngleKey = 49;
  internal const byte ExtrusionPlaneKey = 50;
  internal const byte ExtrusionTypeKey = 51;
  internal const byte ExtrusionRenderModeKey = 52;
  internal const byte RotationAngleXKey = 53;
  internal const byte RotationAngleYKey = 54;
  internal const byte RotationCenterXKey = 55;
  internal const byte RotationCenterYKey = 56;
  internal const byte RotationCenterZKey = 57;
  internal const byte ShininessKey = 58;
  internal const byte SkewAmountKey = 59;
  internal const byte SkewAngleKey = 60;
  internal const byte SpecularityKey = 61;
  internal const byte ViewPointXKey = 62;
  internal const byte ViewPointYKey = 63 /*0x3F*/;
  internal const byte ViewPointZKey = 64 /*0x40*/;
  internal const byte ViewPointOriginXKey = 65;
  internal const byte ViewPointOriginYKey = 66;
  internal const byte VisibleKey = 67;
  internal const byte LightFaceKey = 68;
  internal const byte LightHarshKey = 69;
  internal const byte LightHarsh2Key = 70;
  internal const byte LockRotationCenterKey = 71;
  internal const byte MetalKey = 72;
  internal const byte ExtensionKey = 73;
  internal const byte BackDepthKey = 74;
  internal const byte AutoRotationCenterKey = 75;
  private byte m_flagsA;
  private byte m_flagsB;
  private ShapeBase m_shape;
  private Dictionary<int, object> m_propertiesHash;
  internal Dictionary<string, Stream> m_docxProps = new Dictionary<string, Stream>();

  internal float ContourWidth
  {
    get => (float) this.PropertiesHash[0];
    set
    {
      if ((double) value < 0.0 && (double) value > 20116800.0)
        throw new ArgumentOutOfRangeException("Contour Width should be between 0 and 1584");
      this.SetKeyValue(0, (object) value);
    }
  }

  internal float ExtrusionHeight
  {
    get => (float) this.PropertiesHash[1];
    set
    {
      if ((double) value < 0.0 && (double) value > 20116800.0)
        throw new ArgumentOutOfRangeException("Extrusion Height should be between 0 and 1584");
      this.SetKeyValue(1, (object) value);
    }
  }

  internal PresetMaterialType PresetMaterialType
  {
    get
    {
      return (PresetMaterialType) Enum.Parse(typeof (PresetMaterialType), this.PropertiesHash[2].ToString(), true);
    }
    set => this.SetKeyValue(2, (object) value);
  }

  internal float DistanceFromGround
  {
    get => (float) this.PropertiesHash[3];
    set
    {
      if ((double) value < -50800000.0 && (double) value > 50800000.0)
        throw new ArgumentOutOfRangeException("Contour Width should be between -4000 and 4000");
      this.SetKeyValue(3, (object) value);
    }
  }

  internal float BevelBottomHeight
  {
    get => (float) this.PropertiesHash[4];
    set
    {
      if ((double) value < 0.0 && (double) value > 20116800.0)
        throw new ArgumentOutOfRangeException("Bevel Bottom Height should be between 0 and 1584");
      this.SetKeyValue(4, (object) value);
    }
  }

  internal float BevelBottomWidth
  {
    get => (float) this.PropertiesHash[5];
    set
    {
      if ((double) value < 0.0 && (double) value > 20116800.0)
        throw new ArgumentOutOfRangeException("Bevel Bottom Width should be between 0 and 1584");
      this.SetKeyValue(5, (object) value);
    }
  }

  internal BevelPresetType BevelBPresetType
  {
    get
    {
      return (BevelPresetType) Enum.Parse(typeof (BevelPresetType), this.PropertiesHash[6].ToString(), true);
    }
    set => this.SetKeyValue(6, (object) value);
  }

  internal float BevelTopHeight
  {
    get => (float) this.PropertiesHash[7];
    set
    {
      if ((double) value < 0.0 && (double) value > 20116800.0)
        throw new ArgumentOutOfRangeException("Bevel Top Height should be between 0 and 1584");
      this.SetKeyValue(7, (object) value);
    }
  }

  internal float BevelTopWidth
  {
    get => (float) this.PropertiesHash[8];
    set
    {
      if ((double) value < 0.0 && (double) value > 20116800.0)
        throw new ArgumentOutOfRangeException("Bevel Top Width should be between 0 and 1584");
      this.SetKeyValue(8, (object) value);
    }
  }

  internal BevelPresetType BevelTPresetType
  {
    get
    {
      return (BevelPresetType) Enum.Parse(typeof (BevelPresetType), this.PropertiesHash[9].ToString(), true);
    }
    set => this.SetKeyValue(9, (object) value);
  }

  internal Color ContourColor
  {
    get => (Color) this.PropertiesHash[10];
    set => this.SetKeyValue(10, (object) value);
  }

  internal float ContourOpacity
  {
    get => (float) this.PropertiesHash[11];
    set => this.SetKeyValue(11, (object) value);
  }

  internal Color ExtrusionColor
  {
    get => (Color) this.PropertiesHash[12];
    set => this.SetKeyValue(12, (object) value);
  }

  internal float ExtrusionOpacity
  {
    get => (float) this.PropertiesHash[13];
    set => this.SetKeyValue(13, (object) value);
  }

  internal float BackdropAnchorX
  {
    get => (float) this.PropertiesHash[14];
    set => this.SetKeyValue(14, (object) value);
  }

  internal float BackdropAnchorY
  {
    get => (float) this.PropertiesHash[15];
    set => this.SetKeyValue(15, (object) value);
  }

  internal float BackdropAnchorZ
  {
    get => (float) this.PropertiesHash[16 /*0x10*/];
    set => this.SetKeyValue(16 /*0x10*/, (object) value);
  }

  internal float BackdropNormalX
  {
    get => (float) this.PropertiesHash[17];
    set => this.SetKeyValue(17, (object) value);
  }

  internal float BackdropNormalY
  {
    get => (float) this.PropertiesHash[18];
    set => this.SetKeyValue(18, (object) value);
  }

  internal float BackdropNormalZ
  {
    get => (float) this.PropertiesHash[19];
    set => this.SetKeyValue(19, (object) value);
  }

  internal float BackdropUpX
  {
    get => (float) this.PropertiesHash[20];
    set => this.SetKeyValue(20, (object) value);
  }

  internal float BackdropUpY
  {
    get => (float) this.PropertiesHash[21];
    set => this.SetKeyValue(21, (object) value);
  }

  internal float BackdropUpZ
  {
    get => (float) this.PropertiesHash[22];
    set => this.SetKeyValue(22, (object) value);
  }

  internal float FieldOfView
  {
    get => (float) this.PropertiesHash[23];
    set
    {
      if ((double) value < 0.0 && (double) value > 10800000.0)
        throw new ArgumentOutOfRangeException("Field of View Angle should be between 0 and 180");
      this.SetKeyValue(23, (object) value);
    }
  }

  internal CameraPresetType CameraPresetType
  {
    get
    {
      return (CameraPresetType) Enum.Parse(typeof (CameraPresetType), this.PropertiesHash[24].ToString(), true);
    }
    set => this.SetKeyValue(24, (object) value);
  }

  internal float Zoom
  {
    get => (float) this.PropertiesHash[25];
    set => this.SetKeyValue(25, (object) value);
  }

  internal float CameraRotationX
  {
    get => (float) this.PropertiesHash[26];
    set
    {
      if ((double) value < 0.0 && (double) value > 21600000.0)
        throw new ArgumentOutOfRangeException("Camera Rotation X-axis should be between 0 and 360");
      this.SetKeyValue(26, (object) value);
    }
  }

  internal float CameraRotationY
  {
    get => (float) this.PropertiesHash[27];
    set
    {
      if ((double) value < 0.0 && (double) value > 21600000.0)
        throw new ArgumentOutOfRangeException("Camera Rotation Y-axis should be between 0 and 360");
      this.SetKeyValue(27, (object) value);
    }
  }

  internal float CameraRotationZ
  {
    get => (float) this.PropertiesHash[28];
    set
    {
      if ((double) value < 0.0 && (double) value > 21600000.0)
        throw new ArgumentOutOfRangeException("Camera Rotation Z-axis should be between 0 and 360");
      this.SetKeyValue(28, (object) value);
    }
  }

  internal LightRigType LightRigType
  {
    get
    {
      return (LightRigType) Enum.Parse(typeof (LightRigType), this.PropertiesHash[29].ToString(), true);
    }
    set => this.SetKeyValue(29, (object) value);
  }

  internal LightRigDirection LightRigDirection
  {
    get
    {
      return (LightRigDirection) Enum.Parse(typeof (LightRigDirection), this.PropertiesHash[30].ToString(), true);
    }
    set => this.SetKeyValue(30, (object) value);
  }

  internal float LightRigRotationX
  {
    get => (float) this.PropertiesHash[31 /*0x1F*/];
    set => this.SetKeyValue(31 /*0x1F*/, (object) value);
  }

  internal float LightRigRotationY
  {
    get => (float) this.PropertiesHash[32 /*0x20*/];
    set => this.SetKeyValue(32 /*0x20*/, (object) value);
  }

  internal float LightRigRotationZ
  {
    get => (float) this.PropertiesHash[33];
    set => this.SetKeyValue(33, (object) value);
  }

  internal float Brightness
  {
    get => (float) this.PropertiesHash[34];
    set => this.SetKeyValue(34, (object) value);
  }

  internal string ColorMode
  {
    get => this.PropertiesHash[11].ToString();
    set => this.SetKeyValue(36, (object) value);
  }

  internal float Diffusity
  {
    get => (float) this.PropertiesHash[37];
    set => this.SetKeyValue(37, (object) value);
  }

  internal float Edge
  {
    get => (float) this.PropertiesHash[38];
    set => this.SetKeyValue(38, (object) value);
  }

  internal float Facet
  {
    get => (float) this.PropertiesHash[39];
    set => this.SetKeyValue(39, (object) value);
  }

  internal float ForeDepth
  {
    get => (float) this.PropertiesHash[40];
    set => this.SetKeyValue(40, (object) value);
  }

  internal float BackDepth
  {
    get => (float) this.PropertiesHash[74];
    set => this.SetKeyValue(74, (object) value);
  }

  internal float LightLevel
  {
    get => (float) this.PropertiesHash[41];
    set => this.SetKeyValue(41, (object) value);
  }

  internal float LightLevel2
  {
    get => (float) this.PropertiesHash[42];
    set => this.SetKeyValue(42, (object) value);
  }

  internal float LightRigRotation2X
  {
    get => (float) this.PropertiesHash[43];
    set
    {
      if ((double) value < 0.0 && (double) value > 21600000.0)
        throw new ArgumentOutOfRangeException("LightRig Rotation X-axis should be between 0 and 360");
      this.SetKeyValue(43, (object) value);
    }
  }

  internal float LightRigRotation2Y
  {
    get => (float) this.PropertiesHash[44];
    set
    {
      if ((double) value < 0.0 && (double) value > 21600000.0)
        throw new ArgumentOutOfRangeException("LightRig Rotation Y-axis should be between 0 and 360");
      this.SetKeyValue(44, (object) value);
    }
  }

  internal float LightRigRotation2Z
  {
    get => (float) this.PropertiesHash[45];
    set
    {
      if ((double) value < 0.0 && (double) value > 21600000.0)
        throw new ArgumentOutOfRangeException("LightRig Rotation Z-axis should be between 0 and 360");
      this.SetKeyValue(45, (object) value);
    }
  }

  internal float RotationX
  {
    get => (float) this.PropertiesHash[46];
    set => this.SetKeyValue(46, (object) value);
  }

  internal float RotationY
  {
    get => (float) this.PropertiesHash[47];
    set => this.SetKeyValue(47, (object) value);
  }

  internal float RotationZ
  {
    get => (float) this.PropertiesHash[48 /*0x30*/];
    set => this.SetKeyValue(48 /*0x30*/, (object) value);
  }

  internal float OrientationAngle
  {
    get => (float) this.PropertiesHash[49];
    set => this.SetKeyValue(49, (object) value);
  }

  internal ExtrusionPlane ExtrusionPlane
  {
    get
    {
      return (ExtrusionPlane) Enum.Parse(typeof (ExtrusionPlane), this.PropertiesHash[50].ToString(), true);
    }
    set => this.SetKeyValue(50, (object) value);
  }

  internal ExtrusionType ExtrusionType
  {
    get
    {
      return (ExtrusionType) Enum.Parse(typeof (ExtrusionType), this.PropertiesHash[51].ToString(), true);
    }
    set => this.SetKeyValue(51, (object) value);
  }

  internal ExtrusionRenderMode ExtrusionRenderMode
  {
    get
    {
      return (ExtrusionRenderMode) Enum.Parse(typeof (ExtrusionRenderMode), this.PropertiesHash[52].ToString(), true);
    }
    set => this.SetKeyValue(52, (object) value);
  }

  internal float RotationAngleX
  {
    get => (float) this.PropertiesHash[53];
    set => this.SetKeyValue(53, (object) value);
  }

  internal float RotationAngleY
  {
    get => (float) this.PropertiesHash[54];
    set => this.SetKeyValue(54, (object) value);
  }

  internal float RotationCenterX
  {
    get => (float) this.PropertiesHash[55];
    set => this.SetKeyValue(55, (object) value);
  }

  internal float RotationCenterY
  {
    get => (float) this.PropertiesHash[56];
    set => this.SetKeyValue(56, (object) value);
  }

  internal float RotationCenterZ
  {
    get => (float) this.PropertiesHash[57];
    set => this.SetKeyValue(57, (object) value);
  }

  internal float Shininess
  {
    get => (float) this.PropertiesHash[58];
    set => this.SetKeyValue(58, (object) value);
  }

  internal float SkewAmount
  {
    get => (float) this.PropertiesHash[59];
    set => this.SetKeyValue(59, (object) value);
  }

  internal float SkewAngle
  {
    get => (float) this.PropertiesHash[60];
    set => this.SetKeyValue(60, (object) value);
  }

  internal float Specularity
  {
    get => (float) this.PropertiesHash[61];
    set => this.SetKeyValue(61, (object) value);
  }

  internal float ViewPointX
  {
    get => (float) this.PropertiesHash[62];
    set => this.SetKeyValue(62, (object) value);
  }

  internal float ViewPointY
  {
    get => (float) this.PropertiesHash[63 /*0x3F*/];
    set => this.SetKeyValue(63 /*0x3F*/, (object) value);
  }

  internal float ViewPointZ
  {
    get => (float) this.PropertiesHash[64 /*0x40*/];
    set => this.SetKeyValue(64 /*0x40*/, (object) value);
  }

  internal float ViewPointOriginX
  {
    get => (float) this.PropertiesHash[65];
    set => this.SetKeyValue(65, (object) value);
  }

  internal float ViewPointOriginY
  {
    get => (float) this.PropertiesHash[66];
    set => this.SetKeyValue(66, (object) value);
  }

  internal bool Visible
  {
    get => ((int) this.m_flagsA & 1) != 0;
    set
    {
      this.m_flagsA = (byte) ((int) this.m_flagsA & 254 | (value ? 1 : 0));
      this.SetKeyValue(67, (object) value);
    }
  }

  internal bool LightFace
  {
    get => ((int) this.m_flagsA & 2) >> 1 != 0;
    set
    {
      this.m_flagsA = (byte) ((int) this.m_flagsA & 253 | (value ? 1 : 0) << 1);
      this.SetKeyValue(68, (object) value);
    }
  }

  internal bool LightHarsh
  {
    get => ((int) this.m_flagsA & 4) >> 2 != 0;
    set
    {
      this.m_flagsA = (byte) ((int) this.m_flagsA & 251 | (value ? 1 : 0) << 2);
      this.SetKeyValue(69, (object) value);
    }
  }

  internal bool LightHarsh2
  {
    get => ((int) this.m_flagsA & 8) >> 3 != 0;
    set
    {
      this.m_flagsA = (byte) ((int) this.m_flagsA & 247 | (value ? 1 : 0) << 3);
      this.SetKeyValue(70, (object) value);
    }
  }

  internal bool LockRotationCenter
  {
    get => ((int) this.m_flagsA & 16 /*0x10*/) >> 4 != 0;
    set
    {
      this.m_flagsA = (byte) ((int) this.m_flagsA & 239 | (value ? 1 : 0) << 4);
      this.SetKeyValue(71, (object) value);
    }
  }

  internal bool Metal
  {
    get => ((int) this.m_flagsA & 32 /*0x20*/) >> 5 != 0;
    set
    {
      this.m_flagsA = (byte) ((int) this.m_flagsA & 223 | (value ? 1 : 0) << 5);
      this.SetKeyValue(72, (object) value);
    }
  }

  internal bool AutoRotationCenter
  {
    get => ((int) this.m_flagsA & 64 /*0x40*/) >> 6 != 0;
    set
    {
      this.m_flagsA = (byte) ((int) this.m_flagsA & 191 | (value ? 1 : 0) << 6);
      this.SetKeyValue(75, (object) value);
    }
  }

  internal string Extension
  {
    get => this.PropertiesHash[73].ToString();
    set => this.SetKeyValue(73, (object) value);
  }

  internal bool HasBackdropEffect
  {
    get => ((int) this.m_flagsB & 1) != 0;
    set => this.m_flagsB = (byte) ((int) this.m_flagsB & 254 | (value ? 1 : 0));
  }

  internal bool HasCameraEffect
  {
    get => ((int) this.m_flagsB & 2) >> 1 != 0;
    set => this.m_flagsB = (byte) ((int) this.m_flagsB & 253 | (value ? 1 : 0) << 1);
  }

  internal bool HasLightRigEffect
  {
    get => ((int) this.m_flagsB & 4) >> 2 != 0;
    set => this.m_flagsB = (byte) ((int) this.m_flagsB & 251 | (value ? 1 : 0) << 2);
  }

  internal bool HasBevelBottom
  {
    get => ((int) this.m_flagsB & 8) >> 3 != 0;
    set => this.m_flagsB = (byte) ((int) this.m_flagsB & 247 | (value ? 1 : 0) << 3);
  }

  internal bool HasBevelTop
  {
    get => ((int) this.m_flagsB & 16 /*0x10*/) >> 4 != 0;
    set => this.m_flagsB = (byte) ((int) this.m_flagsB & 239 | (value ? 1 : 0) << 4);
  }

  internal bool HasContourColor
  {
    get => ((int) this.m_flagsB & 32 /*0x20*/) >> 5 != 0;
    set => this.m_flagsB = (byte) ((int) this.m_flagsB & 223 | (value ? 1 : 0) << 5);
  }

  internal bool HasExtrusionColor
  {
    get => ((int) this.m_flagsB & 64 /*0x40*/) >> 6 != 0;
    set => this.m_flagsB = (byte) ((int) this.m_flagsB & 191 | (value ? 1 : 0) << 6);
  }

  internal Dictionary<int, object> PropertiesHash
  {
    get
    {
      if (this.m_propertiesHash == null)
        this.m_propertiesHash = new Dictionary<int, object>();
      return this.m_propertiesHash;
    }
  }

  internal Dictionary<string, Stream> DocxProps
  {
    get
    {
      if (this.m_docxProps == null)
        this.m_docxProps = new Dictionary<string, Stream>();
      return this.m_docxProps;
    }
  }

  internal ThreeDFormat(ShapeBase shape) => this.m_shape = shape;

  internal void Close()
  {
    if (this.m_shape != null)
      this.m_shape = (ShapeBase) null;
    if (this.m_docxProps != null && this.m_docxProps.Count > 0)
    {
      this.m_docxProps.Clear();
      this.m_docxProps = (Dictionary<string, Stream>) null;
    }
    if (this.m_propertiesHash == null || this.m_propertiesHash.Count <= 0)
      return;
    this.m_propertiesHash.Clear();
    this.m_propertiesHash = (Dictionary<int, object>) null;
  }

  internal ThreeDFormat Clone()
  {
    ThreeDFormat threeDformat = (ThreeDFormat) this.MemberwiseClone();
    if (this.PropertiesHash != null && this.PropertiesHash.Count > 0)
    {
      threeDformat.m_propertiesHash = new Dictionary<int, object>();
      foreach (KeyValuePair<int, object> keyValuePair in this.PropertiesHash)
        threeDformat.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
    }
    if (this.m_docxProps != null && this.m_docxProps.Count > 0)
      this.m_shape.Document.CloneProperties(this.m_docxProps, ref threeDformat.m_docxProps);
    return threeDformat;
  }

  private void SetKeyValue(int propKey, object value) => this[propKey] = value;

  protected object this[int key]
  {
    get => (object) key;
    set => this.PropertiesHash[key] = value;
  }
}
