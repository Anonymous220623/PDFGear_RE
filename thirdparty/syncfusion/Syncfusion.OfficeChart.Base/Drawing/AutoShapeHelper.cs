// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.AutoShapeHelper
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Drawing;

internal class AutoShapeHelper
{
  internal static Dictionary<string, int> dictionary;

  internal static AutoShapeConstant GetAutoShapeConstant(string shapeString)
  {
    string key = shapeString;
    if (key != null)
    {
      if (AutoShapeHelper.dictionary == null)
        AutoShapeHelper.dictionary = new Dictionary<string, int>(187)
        {
          {
            "line",
            0
          },
          {
            "lineInv",
            1
          },
          {
            "triangle",
            2
          },
          {
            "rtTriangle",
            3
          },
          {
            "rect",
            4
          },
          {
            "diamond",
            5
          },
          {
            "parallelogram",
            6
          },
          {
            "trapezoid",
            7
          },
          {
            "nonIsoscelesTrapezoid",
            8
          },
          {
            "pentagon",
            9
          },
          {
            "hexagon",
            10
          },
          {
            "heptagon",
            11
          },
          {
            "octagon",
            12
          },
          {
            "decagon",
            13
          },
          {
            "dodecagon",
            14
          },
          {
            "star4",
            15
          },
          {
            "star5",
            16 /*0x10*/
          },
          {
            "star6",
            17
          },
          {
            "star7",
            18
          },
          {
            "star8",
            19
          },
          {
            "star10",
            20
          },
          {
            "star12",
            21
          },
          {
            "star16",
            22
          },
          {
            "star24",
            23
          },
          {
            "star32",
            24
          },
          {
            "roundRect",
            25
          },
          {
            "round1Rect",
            26
          },
          {
            "round2SameRect",
            27
          },
          {
            "round2DiagRect",
            28
          },
          {
            "snipRoundRect",
            29
          },
          {
            "snip1Rect",
            30
          },
          {
            "snip2SameRect",
            31 /*0x1F*/
          },
          {
            "snip2DiagRect",
            32 /*0x20*/
          },
          {
            "plaque",
            33
          },
          {
            "ellipse",
            34
          },
          {
            "teardrop",
            35
          },
          {
            "homePlate",
            36
          },
          {
            "chevron",
            37
          },
          {
            "pieWedge",
            38
          },
          {
            "pie",
            39
          },
          {
            "blockArc",
            40
          },
          {
            "donut",
            41
          },
          {
            "noSmoking",
            42
          },
          {
            "rightArrow",
            43
          },
          {
            "leftArrow",
            44
          },
          {
            "upArrow",
            45
          },
          {
            "downArrow",
            46
          },
          {
            "stripedRightArrow",
            47
          },
          {
            "notchedRightArrow",
            48 /*0x30*/
          },
          {
            "bentUpArrow",
            49
          },
          {
            "leftRightArrow",
            50
          },
          {
            "upDownArrow",
            51
          },
          {
            "leftUpArrow",
            52
          },
          {
            "leftRightUpArrow",
            53
          },
          {
            "quadArrow",
            54
          },
          {
            "leftArrowCallout",
            55
          },
          {
            "rightArrowCallout",
            56
          },
          {
            "upArrowCallout",
            57
          },
          {
            "downArrowCallout",
            58
          },
          {
            "leftRightArrowCallout",
            59
          },
          {
            "upDownArrowCallout",
            60
          },
          {
            "quadArrowCallout",
            61
          },
          {
            "bentArrow",
            62
          },
          {
            "uturnArrow",
            63 /*0x3F*/
          },
          {
            "circularArrow",
            64 /*0x40*/
          },
          {
            "leftCircularArrow",
            65
          },
          {
            "leftRightCircularArrow",
            66
          },
          {
            "curvedRightArrow",
            67
          },
          {
            "curvedLeftArrow",
            68
          },
          {
            "curvedUpArrow",
            69
          },
          {
            "curvedDownArrow",
            70
          },
          {
            "swooshArrow",
            71
          },
          {
            "cube",
            72
          },
          {
            "can",
            73
          },
          {
            "lightningBolt",
            74
          },
          {
            "heart",
            75
          },
          {
            "sun",
            76
          },
          {
            "moon",
            77
          },
          {
            "smileyFace",
            78
          },
          {
            "irregularSeal1",
            79
          },
          {
            "irregularSeal2",
            80 /*0x50*/
          },
          {
            "foldedCorner",
            81
          },
          {
            "bevel",
            82
          },
          {
            "frame",
            83
          },
          {
            "halfFrame",
            84
          },
          {
            "corner",
            85
          },
          {
            "diagStripe",
            86
          },
          {
            "chord",
            87
          },
          {
            "arc",
            88
          },
          {
            "leftBracket",
            89
          },
          {
            "rightBracket",
            90
          },
          {
            "leftBrace",
            91
          },
          {
            "rightBrace",
            92
          },
          {
            "bracketPair",
            93
          },
          {
            "bracePair",
            94
          },
          {
            "straightConnector1",
            95
          },
          {
            "bentConnector2",
            96 /*0x60*/
          },
          {
            "bentConnector3",
            97
          },
          {
            "bentConnector4",
            98
          },
          {
            "bentConnector5",
            99
          },
          {
            "curvedConnector2",
            100
          },
          {
            "curvedConnector3",
            101
          },
          {
            "curvedConnector4",
            102
          },
          {
            "curvedConnector5",
            103
          },
          {
            "callout1",
            104
          },
          {
            "callout2",
            105
          },
          {
            "callout3",
            106
          },
          {
            "accentCallout1",
            107
          },
          {
            "accentCallout2",
            108
          },
          {
            "accentCallout3",
            109
          },
          {
            "borderCallout1",
            110
          },
          {
            "borderCallout2",
            111
          },
          {
            "borderCallout3",
            112 /*0x70*/
          },
          {
            "accentBorderCallout1",
            113
          },
          {
            "accentBorderCallout2",
            114
          },
          {
            "accentBorderCallout3",
            115
          },
          {
            "wedgeRectCallout",
            116
          },
          {
            "wedgeRoundRectCallout",
            117
          },
          {
            "wedgeEllipseCallout",
            118
          },
          {
            "cloudCallout",
            119
          },
          {
            "cloud",
            120
          },
          {
            "ribbon",
            121
          },
          {
            "ribbon2",
            122
          },
          {
            "ellipseRibbon",
            123
          },
          {
            "ellipseRibbon2",
            124
          },
          {
            "leftRightRibbon",
            125
          },
          {
            "verticalScroll",
            126
          },
          {
            "horizontalScroll",
            (int) sbyte.MaxValue
          },
          {
            "wave",
            128 /*0x80*/
          },
          {
            "doubleWave",
            129
          },
          {
            "plus",
            130
          },
          {
            "flowChartProcess",
            131
          },
          {
            "flowChartDecision",
            132
          },
          {
            "flowChartInputOutput",
            133
          },
          {
            "flowChartPredefinedProcess",
            134
          },
          {
            "flowChartInternalStorage",
            135
          },
          {
            "flowChartDocument",
            136
          },
          {
            "flowChartMultidocument",
            137
          },
          {
            "flowChartTerminator",
            138
          },
          {
            "flowChartPreparation",
            139
          },
          {
            "flowChartManualInput",
            140
          },
          {
            "flowChartManualOperation",
            141
          },
          {
            "flowChartConnector",
            142
          },
          {
            "flowChartPunchedCard",
            143
          },
          {
            "flowChartPunchedTape",
            144 /*0x90*/
          },
          {
            "flowChartSummingJunction",
            145
          },
          {
            "flowChartOr",
            146
          },
          {
            "flowChartCollate",
            147
          },
          {
            "flowChartSort",
            148
          },
          {
            "flowChartExtract",
            149
          },
          {
            "flowChartMerge",
            150
          },
          {
            "flowChartOfflineStorage",
            151
          },
          {
            "flowChartOnlineStorage",
            152
          },
          {
            "flowChartMagneticTape",
            153
          },
          {
            "flowChartMagneticDisk",
            154
          },
          {
            "flowChartMagneticDrum",
            155
          },
          {
            "flowChartDisplay",
            156
          },
          {
            "flowChartDelay",
            157
          },
          {
            "flowChartAlternateProcess",
            158
          },
          {
            "flowChartOffpageConnector",
            159
          },
          {
            "actionButtonBlank",
            160 /*0xA0*/
          },
          {
            "actionButtonHome",
            161
          },
          {
            "actionButtonHelp",
            162
          },
          {
            "actionButtonInformation",
            163
          },
          {
            "actionButtonForwardNext",
            164
          },
          {
            "actionButtonBackPrevious",
            165
          },
          {
            "actionButtonEnd",
            166
          },
          {
            "actionButtonBeginning",
            167
          },
          {
            "actionButtonReturn",
            168
          },
          {
            "actionButtonDocument",
            169
          },
          {
            "actionButtonSound",
            170
          },
          {
            "actionButtonMovie",
            171
          },
          {
            "gear6",
            172
          },
          {
            "gear9",
            173
          },
          {
            "funnel",
            174
          },
          {
            "mathPlus",
            175
          },
          {
            "mathMinus",
            176 /*0xB0*/
          },
          {
            "mathMultiply",
            177
          },
          {
            "mathDivide",
            178
          },
          {
            "mathEqual",
            179
          },
          {
            "mathNotEqual",
            180
          },
          {
            "cornerTabs",
            181
          },
          {
            "squareTabs",
            182
          },
          {
            "plaqueTabs",
            183
          },
          {
            "chartX",
            184
          },
          {
            "chartStar",
            185
          },
          {
            "chartPlus",
            186
          }
        };
      int num;
      if (AutoShapeHelper.dictionary.TryGetValue(key, out num))
      {
        switch (num)
        {
          case 0:
            return AutoShapeConstant.Index_0;
          case 1:
            return AutoShapeConstant.Index_1;
          case 2:
            return AutoShapeConstant.Index_2;
          case 3:
            return AutoShapeConstant.Index_3;
          case 4:
            return AutoShapeConstant.Index_4;
          case 5:
            return AutoShapeConstant.Index_5;
          case 6:
            return AutoShapeConstant.Index_6;
          case 7:
            return AutoShapeConstant.Index_7;
          case 8:
            return AutoShapeConstant.Index_8;
          case 9:
            return AutoShapeConstant.Index_9;
          case 10:
            return AutoShapeConstant.Index_10;
          case 11:
            return AutoShapeConstant.Index_11;
          case 12:
            return AutoShapeConstant.Index_12;
          case 13:
            return AutoShapeConstant.Index_13;
          case 14:
            return AutoShapeConstant.Index_14;
          case 15:
            return AutoShapeConstant.Index_15;
          case 16 /*0x10*/:
            return AutoShapeConstant.Index_16;
          case 17:
            return AutoShapeConstant.Index_17;
          case 18:
            return AutoShapeConstant.Index_18;
          case 19:
            return AutoShapeConstant.Index_19;
          case 20:
            return AutoShapeConstant.Index_20;
          case 21:
            return AutoShapeConstant.Index_21;
          case 22:
            return AutoShapeConstant.Index_22;
          case 23:
            return AutoShapeConstant.Index_23;
          case 24:
            return AutoShapeConstant.Index_24;
          case 25:
            return AutoShapeConstant.Index_25;
          case 26:
            return AutoShapeConstant.Index_26;
          case 27:
            return AutoShapeConstant.Index_27;
          case 28:
            return AutoShapeConstant.Index_28;
          case 29:
            return AutoShapeConstant.Index_29;
          case 30:
            return AutoShapeConstant.Index_30;
          case 31 /*0x1F*/:
            return AutoShapeConstant.Index_31;
          case 32 /*0x20*/:
            return AutoShapeConstant.Index_32;
          case 33:
            return AutoShapeConstant.Index_33;
          case 34:
            return AutoShapeConstant.Index_34;
          case 35:
            return AutoShapeConstant.Index_35;
          case 36:
            return AutoShapeConstant.Index_36;
          case 37:
            return AutoShapeConstant.Index_37;
          case 38:
            return AutoShapeConstant.Index_38;
          case 39:
            return AutoShapeConstant.Index_39;
          case 40:
            return AutoShapeConstant.Index_40;
          case 41:
            return AutoShapeConstant.Index_41;
          case 42:
            return AutoShapeConstant.Index_42;
          case 43:
            return AutoShapeConstant.Index_43;
          case 44:
            return AutoShapeConstant.Index_44;
          case 45:
            return AutoShapeConstant.Index_45;
          case 46:
            return AutoShapeConstant.Index_46;
          case 47:
            return AutoShapeConstant.Index_47;
          case 48 /*0x30*/:
            return AutoShapeConstant.Index_48;
          case 49:
            return AutoShapeConstant.Index_49;
          case 50:
            return AutoShapeConstant.Index_50;
          case 51:
            return AutoShapeConstant.Index_51;
          case 52:
            return AutoShapeConstant.Index_52;
          case 53:
            return AutoShapeConstant.Index_53;
          case 54:
            return AutoShapeConstant.Index_54;
          case 55:
            return AutoShapeConstant.Index_55;
          case 56:
            return AutoShapeConstant.Index_56;
          case 57:
            return AutoShapeConstant.Index_57;
          case 58:
            return AutoShapeConstant.Index_58;
          case 59:
            return AutoShapeConstant.Index_59;
          case 60:
            return AutoShapeConstant.Index_60;
          case 61:
            return AutoShapeConstant.Index_61;
          case 62:
            return AutoShapeConstant.Index_62;
          case 63 /*0x3F*/:
            return AutoShapeConstant.Index_63;
          case 64 /*0x40*/:
            return AutoShapeConstant.Index_64;
          case 65:
            return AutoShapeConstant.Index_65;
          case 66:
            return AutoShapeConstant.Index_66;
          case 67:
            return AutoShapeConstant.Index_67;
          case 68:
            return AutoShapeConstant.Index_68;
          case 69:
            return AutoShapeConstant.Index_69;
          case 70:
            return AutoShapeConstant.Index_70;
          case 71:
            return AutoShapeConstant.Index_71;
          case 72:
            return AutoShapeConstant.Index_72;
          case 73:
            return AutoShapeConstant.Index_73;
          case 74:
            return AutoShapeConstant.Index_74;
          case 75:
            return AutoShapeConstant.Index_75;
          case 76:
            return AutoShapeConstant.Index_76;
          case 77:
            return AutoShapeConstant.Index_77;
          case 78:
            return AutoShapeConstant.Index_78;
          case 79:
            return AutoShapeConstant.Index_79;
          case 80 /*0x50*/:
            return AutoShapeConstant.Index_80;
          case 81:
            return AutoShapeConstant.Index_81;
          case 82:
            return AutoShapeConstant.Index_82;
          case 83:
            return AutoShapeConstant.Index_83;
          case 84:
            return AutoShapeConstant.Index_84;
          case 85:
            return AutoShapeConstant.Index_85;
          case 86:
            return AutoShapeConstant.Index_86;
          case 87:
            return AutoShapeConstant.Index_87;
          case 88:
            return AutoShapeConstant.Index_88;
          case 89:
            return AutoShapeConstant.Index_89;
          case 90:
            return AutoShapeConstant.Index_90;
          case 91:
            return AutoShapeConstant.Index_91;
          case 92:
            return AutoShapeConstant.Index_92;
          case 93:
            return AutoShapeConstant.Index_93;
          case 94:
            return AutoShapeConstant.Index_94;
          case 95:
            return AutoShapeConstant.Index_95;
          case 96 /*0x60*/:
            return AutoShapeConstant.Index_96;
          case 97:
            return AutoShapeConstant.Index_97;
          case 98:
            return AutoShapeConstant.Index_98;
          case 99:
            return AutoShapeConstant.Index_99;
          case 100:
            return AutoShapeConstant.Index_100;
          case 101:
            return AutoShapeConstant.Index_101;
          case 102:
            return AutoShapeConstant.Index_102;
          case 103:
            return AutoShapeConstant.Index_103;
          case 104:
            return AutoShapeConstant.Index_104;
          case 105:
            return AutoShapeConstant.Index_105;
          case 106:
            return AutoShapeConstant.Index_106;
          case 107:
            return AutoShapeConstant.Index_107;
          case 108:
            return AutoShapeConstant.Index_108;
          case 109:
            return AutoShapeConstant.Index_109;
          case 110:
            return AutoShapeConstant.Index_110;
          case 111:
            return AutoShapeConstant.Index_111;
          case 112 /*0x70*/:
            return AutoShapeConstant.Index_112;
          case 113:
            return AutoShapeConstant.Index_113;
          case 114:
            return AutoShapeConstant.Index_114;
          case 115:
            return AutoShapeConstant.Index_115;
          case 116:
            return AutoShapeConstant.Index_116;
          case 117:
            return AutoShapeConstant.Index_117;
          case 118:
            return AutoShapeConstant.Index_118;
          case 119:
            return AutoShapeConstant.Index_119;
          case 120:
            return AutoShapeConstant.Index_120;
          case 121:
            return AutoShapeConstant.Index_121;
          case 122:
            return AutoShapeConstant.Index_122;
          case 123:
            return AutoShapeConstant.Index_123;
          case 124:
            return AutoShapeConstant.Index_124;
          case 125:
            return AutoShapeConstant.Index_125;
          case 126:
            return AutoShapeConstant.Index_126;
          case (int) sbyte.MaxValue:
            return AutoShapeConstant.Index_127;
          case 128 /*0x80*/:
            return AutoShapeConstant.Index_128;
          case 129:
            return AutoShapeConstant.Index_129;
          case 130:
            return AutoShapeConstant.Index_130;
          case 131:
            return AutoShapeConstant.Index_131;
          case 132:
            return AutoShapeConstant.Index_132;
          case 133:
            return AutoShapeConstant.Index_133;
          case 134:
            return AutoShapeConstant.Index_134;
          case 135:
            return AutoShapeConstant.Index_135;
          case 136:
            return AutoShapeConstant.Index_136;
          case 137:
            return AutoShapeConstant.Index_137;
          case 138:
            return AutoShapeConstant.Index_138;
          case 139:
            return AutoShapeConstant.Index_139;
          case 140:
            return AutoShapeConstant.Index_140;
          case 141:
            return AutoShapeConstant.Index_141;
          case 142:
            return AutoShapeConstant.Index_142;
          case 143:
            return AutoShapeConstant.Index_143;
          case 144 /*0x90*/:
            return AutoShapeConstant.Index_144;
          case 145:
            return AutoShapeConstant.Index_145;
          case 146:
            return AutoShapeConstant.Index_146;
          case 147:
            return AutoShapeConstant.Index_147;
          case 148:
            return AutoShapeConstant.Index_148;
          case 149:
            return AutoShapeConstant.Index_149;
          case 150:
            return AutoShapeConstant.Index_150;
          case 151:
            return AutoShapeConstant.Index_151;
          case 152:
            return AutoShapeConstant.Index_152;
          case 153:
            return AutoShapeConstant.Index_153;
          case 154:
            return AutoShapeConstant.Index_154;
          case 155:
            return AutoShapeConstant.Index_155;
          case 156:
            return AutoShapeConstant.Index_156;
          case 157:
            return AutoShapeConstant.Index_157;
          case 158:
            return AutoShapeConstant.Index_158;
          case 159:
            return AutoShapeConstant.Index_159;
          case 160 /*0xA0*/:
            return AutoShapeConstant.Index_160;
          case 161:
            return AutoShapeConstant.Index_161;
          case 162:
            return AutoShapeConstant.Index_162;
          case 163:
            return AutoShapeConstant.Index_163;
          case 164:
            return AutoShapeConstant.Index_164;
          case 165:
            return AutoShapeConstant.Index_165;
          case 166:
            return AutoShapeConstant.Index_166;
          case 167:
            return AutoShapeConstant.Index_167;
          case 168:
            return AutoShapeConstant.Index_168;
          case 169:
            return AutoShapeConstant.Index_169;
          case 170:
            return AutoShapeConstant.Index_170;
          case 171:
            return AutoShapeConstant.Index_171;
          case 172:
            return AutoShapeConstant.Index_172;
          case 173:
            return AutoShapeConstant.Index_173;
          case 174:
            return AutoShapeConstant.Index_174;
          case 175:
            return AutoShapeConstant.Index_175;
          case 176 /*0xB0*/:
            return AutoShapeConstant.Index_176;
          case 177:
            return AutoShapeConstant.Index_177;
          case 178:
            return AutoShapeConstant.Index_178;
          case 179:
            return AutoShapeConstant.Index_179;
          case 180:
            return AutoShapeConstant.Index_180;
          case 181:
            return AutoShapeConstant.Index_181;
          case 182:
            return AutoShapeConstant.Index_182;
          case 183:
            return AutoShapeConstant.Index_183;
          case 184:
            return AutoShapeConstant.Index_184;
          case 185:
            return AutoShapeConstant.Index_185;
          case 186:
            return AutoShapeConstant.Index_186;
        }
      }
    }
    return AutoShapeConstant.Index_187;
  }

  internal static AutoShapeType GetAutoShapeType(AutoShapeConstant shapeConstant)
  {
    switch (shapeConstant)
    {
      case AutoShapeConstant.Index_0:
        return AutoShapeType.Line;
      case AutoShapeConstant.Index_2:
        return AutoShapeType.IsoscelesTriangle;
      case AutoShapeConstant.Index_3:
        return AutoShapeType.RightTriangle;
      case AutoShapeConstant.Index_4:
        return AutoShapeType.Rectangle;
      case AutoShapeConstant.Index_5:
        return AutoShapeType.Diamond;
      case AutoShapeConstant.Index_6:
        return AutoShapeType.Parallelogram;
      case AutoShapeConstant.Index_7:
        return AutoShapeType.Trapezoid;
      case AutoShapeConstant.Index_9:
        return AutoShapeType.RegularPentagon;
      case AutoShapeConstant.Index_10:
        return AutoShapeType.Hexagon;
      case AutoShapeConstant.Index_11:
        return AutoShapeType.Heptagon;
      case AutoShapeConstant.Index_12:
        return AutoShapeType.Octagon;
      case AutoShapeConstant.Index_13:
        return AutoShapeType.Decagon;
      case AutoShapeConstant.Index_14:
        return AutoShapeType.Dodecagon;
      case AutoShapeConstant.Index_15:
        return AutoShapeType.Star4Point;
      case AutoShapeConstant.Index_16:
        return AutoShapeType.Star5Point;
      case AutoShapeConstant.Index_17:
        return AutoShapeType.Star6Point;
      case AutoShapeConstant.Index_18:
        return AutoShapeType.Star7Point;
      case AutoShapeConstant.Index_19:
        return AutoShapeType.Star8Point;
      case AutoShapeConstant.Index_20:
        return AutoShapeType.Star10Point;
      case AutoShapeConstant.Index_21:
        return AutoShapeType.Star12Point;
      case AutoShapeConstant.Index_22:
        return AutoShapeType.Star16Point;
      case AutoShapeConstant.Index_23:
        return AutoShapeType.Star24Point;
      case AutoShapeConstant.Index_24:
        return AutoShapeType.Star32Point;
      case AutoShapeConstant.Index_25:
        return AutoShapeType.RoundedRectangle;
      case AutoShapeConstant.Index_26:
        return AutoShapeType.RoundSingleCornerRectangle;
      case AutoShapeConstant.Index_27:
        return AutoShapeType.RoundSameSideCornerRectangle;
      case AutoShapeConstant.Index_28:
        return AutoShapeType.RoundDiagonalCornerRectangle;
      case AutoShapeConstant.Index_29:
        return AutoShapeType.SnipAndRoundSingleCornerRectangle;
      case AutoShapeConstant.Index_30:
        return AutoShapeType.SnipSingleCornerRectangle;
      case AutoShapeConstant.Index_31:
        return AutoShapeType.SnipSameSideCornerRectangle;
      case AutoShapeConstant.Index_32:
        return AutoShapeType.SnipDiagonalCornerRectangle;
      case AutoShapeConstant.Index_33:
        return AutoShapeType.Plaque;
      case AutoShapeConstant.Index_34:
        return AutoShapeType.Oval;
      case AutoShapeConstant.Index_35:
        return AutoShapeType.Teardrop;
      case AutoShapeConstant.Index_36:
        return AutoShapeType.Pentagon;
      case AutoShapeConstant.Index_37:
        return AutoShapeType.Chevron;
      case AutoShapeConstant.Index_39:
        return AutoShapeType.Pie;
      case AutoShapeConstant.Index_40:
        return AutoShapeType.BlockArc;
      case AutoShapeConstant.Index_41:
        return AutoShapeType.Donut;
      case AutoShapeConstant.Index_42:
        return AutoShapeType.NoSymbol;
      case AutoShapeConstant.Index_43:
        return AutoShapeType.RightArrow;
      case AutoShapeConstant.Index_44:
        return AutoShapeType.LeftArrow;
      case AutoShapeConstant.Index_45:
        return AutoShapeType.UpArrow;
      case AutoShapeConstant.Index_46:
        return AutoShapeType.DownArrow;
      case AutoShapeConstant.Index_47:
        return AutoShapeType.StripedRightArrow;
      case AutoShapeConstant.Index_48:
        return AutoShapeType.NotchedRightArrow;
      case AutoShapeConstant.Index_49:
        return AutoShapeType.BentUpArrow;
      case AutoShapeConstant.Index_50:
        return AutoShapeType.LeftRightArrow;
      case AutoShapeConstant.Index_51:
        return AutoShapeType.UpDownArrow;
      case AutoShapeConstant.Index_52:
        return AutoShapeType.LeftUpArrow;
      case AutoShapeConstant.Index_53:
        return AutoShapeType.LeftRightUpArrow;
      case AutoShapeConstant.Index_54:
        return AutoShapeType.QuadArrow;
      case AutoShapeConstant.Index_55:
        return AutoShapeType.LeftArrowCallout;
      case AutoShapeConstant.Index_56:
        return AutoShapeType.RightArrowCallout;
      case AutoShapeConstant.Index_57:
        return AutoShapeType.UpArrowCallout;
      case AutoShapeConstant.Index_58:
        return AutoShapeType.DownArrowCallout;
      case AutoShapeConstant.Index_59:
        return AutoShapeType.LeftRightArrowCallout;
      case AutoShapeConstant.Index_60:
        return AutoShapeType.UpDownArrowCallout;
      case AutoShapeConstant.Index_61:
        return AutoShapeType.QuadArrowCallout;
      case AutoShapeConstant.Index_62:
        return AutoShapeType.BentArrow;
      case AutoShapeConstant.Index_63:
        return AutoShapeType.UTurnArrow;
      case AutoShapeConstant.Index_64:
        return AutoShapeType.CircularArrow;
      case AutoShapeConstant.Index_67:
        return AutoShapeType.CurvedRightArrow;
      case AutoShapeConstant.Index_68:
        return AutoShapeType.CurvedLeftArrow;
      case AutoShapeConstant.Index_69:
        return AutoShapeType.CurvedUpArrow;
      case AutoShapeConstant.Index_70:
        return AutoShapeType.CurvedDownArrow;
      case AutoShapeConstant.Index_72:
        return AutoShapeType.Cube;
      case AutoShapeConstant.Index_73:
        return AutoShapeType.Can;
      case AutoShapeConstant.Index_74:
        return AutoShapeType.LightningBolt;
      case AutoShapeConstant.Index_75:
        return AutoShapeType.Heart;
      case AutoShapeConstant.Index_76:
        return AutoShapeType.Sun;
      case AutoShapeConstant.Index_77:
        return AutoShapeType.Moon;
      case AutoShapeConstant.Index_78:
        return AutoShapeType.SmileyFace;
      case AutoShapeConstant.Index_79:
        return AutoShapeType.Explosion1;
      case AutoShapeConstant.Index_80:
        return AutoShapeType.Explosion2;
      case AutoShapeConstant.Index_81:
        return AutoShapeType.FoldedCorner;
      case AutoShapeConstant.Index_82:
        return AutoShapeType.Bevel;
      case AutoShapeConstant.Index_83:
        return AutoShapeType.Frame;
      case AutoShapeConstant.Index_84:
        return AutoShapeType.HalfFrame;
      case AutoShapeConstant.Index_85:
        return AutoShapeType.L_Shape;
      case AutoShapeConstant.Index_86:
        return AutoShapeType.DiagonalStripe;
      case AutoShapeConstant.Index_87:
        return AutoShapeType.Chord;
      case AutoShapeConstant.Index_88:
        return AutoShapeType.Arc;
      case AutoShapeConstant.Index_89:
        return AutoShapeType.LeftBracket;
      case AutoShapeConstant.Index_90:
        return AutoShapeType.RightBracket;
      case AutoShapeConstant.Index_91:
        return AutoShapeType.LeftBrace;
      case AutoShapeConstant.Index_92:
        return AutoShapeType.RightBrace;
      case AutoShapeConstant.Index_93:
        return AutoShapeType.DoubleBracket;
      case AutoShapeConstant.Index_94:
        return AutoShapeType.DoubleBrace;
      case AutoShapeConstant.Index_95:
        return AutoShapeType.StraightConnector;
      case AutoShapeConstant.Index_96:
        return AutoShapeType.BentConnector2;
      case AutoShapeConstant.Index_97:
        return AutoShapeType.ElbowConnector;
      case AutoShapeConstant.Index_98:
        return AutoShapeType.BentConnector4;
      case AutoShapeConstant.Index_99:
        return AutoShapeType.BentConnector5;
      case AutoShapeConstant.Index_100:
        return AutoShapeType.CurvedConnector2;
      case AutoShapeConstant.Index_101:
        return AutoShapeType.CurvedConnector;
      case AutoShapeConstant.Index_102:
        return AutoShapeType.CurvedConnector4;
      case AutoShapeConstant.Index_103:
        return AutoShapeType.CurvedConnector5;
      case AutoShapeConstant.Index_104:
        return AutoShapeType.LineCallout1NoBorder;
      case AutoShapeConstant.Index_105:
        return AutoShapeType.LineCallout2NoBorder;
      case AutoShapeConstant.Index_106:
        return AutoShapeType.LineCallout3NoBorder;
      case AutoShapeConstant.Index_107:
        return AutoShapeType.LineCallout1AccentBar;
      case AutoShapeConstant.Index_108:
        return AutoShapeType.LineCallout2AccentBar;
      case AutoShapeConstant.Index_109:
        return AutoShapeType.LineCallout3AccentBar;
      case AutoShapeConstant.Index_110:
        return AutoShapeType.LineCallout1;
      case AutoShapeConstant.Index_111:
        return AutoShapeType.LineCallout2;
      case AutoShapeConstant.Index_112:
        return AutoShapeType.LineCallout3;
      case AutoShapeConstant.Index_113:
        return AutoShapeType.LineCallout1BorderAndAccentBar;
      case AutoShapeConstant.Index_114:
        return AutoShapeType.LineCallout2BorderAndAccentBar;
      case AutoShapeConstant.Index_115:
        return AutoShapeType.LineCallout3BorderAndAccentBar;
      case AutoShapeConstant.Index_116:
        return AutoShapeType.RectangularCallout;
      case AutoShapeConstant.Index_117:
        return AutoShapeType.RoundedRectangularCallout;
      case AutoShapeConstant.Index_118:
        return AutoShapeType.OvalCallout;
      case AutoShapeConstant.Index_119:
        return AutoShapeType.CloudCallout;
      case AutoShapeConstant.Index_120:
        return AutoShapeType.Cloud;
      case AutoShapeConstant.Index_121:
        return AutoShapeType.DownRibbon;
      case AutoShapeConstant.Index_122:
        return AutoShapeType.UpRibbon;
      case AutoShapeConstant.Index_123:
        return AutoShapeType.CurvedDownRibbon;
      case AutoShapeConstant.Index_124:
        return AutoShapeType.CurvedUpRibbon;
      case AutoShapeConstant.Index_126:
        return AutoShapeType.VerticalScroll;
      case AutoShapeConstant.Index_127:
        return AutoShapeType.HorizontalScroll;
      case AutoShapeConstant.Index_128:
        return AutoShapeType.Wave;
      case AutoShapeConstant.Index_129:
        return AutoShapeType.DoubleWave;
      case AutoShapeConstant.Index_130:
        return AutoShapeType.Cross;
      case AutoShapeConstant.Index_131:
        return AutoShapeType.FlowChartProcess;
      case AutoShapeConstant.Index_132:
        return AutoShapeType.FlowChartDecision;
      case AutoShapeConstant.Index_133:
        return AutoShapeType.FlowChartData;
      case AutoShapeConstant.Index_134:
        return AutoShapeType.FlowChartPredefinedProcess;
      case AutoShapeConstant.Index_135:
        return AutoShapeType.FlowChartInternalStorage;
      case AutoShapeConstant.Index_136:
        return AutoShapeType.FlowChartDocument;
      case AutoShapeConstant.Index_137:
        return AutoShapeType.FlowChartMultiDocument;
      case AutoShapeConstant.Index_138:
        return AutoShapeType.FlowChartTerminator;
      case AutoShapeConstant.Index_139:
        return AutoShapeType.FlowChartPreparation;
      case AutoShapeConstant.Index_140:
        return AutoShapeType.FlowChartManualInput;
      case AutoShapeConstant.Index_141:
        return AutoShapeType.FlowChartManualOperation;
      case AutoShapeConstant.Index_142:
        return AutoShapeType.FlowChartConnector;
      case AutoShapeConstant.Index_143:
        return AutoShapeType.FlowChartCard;
      case AutoShapeConstant.Index_144:
        return AutoShapeType.FlowChartPunchedTape;
      case AutoShapeConstant.Index_145:
        return AutoShapeType.FlowChartSummingJunction;
      case AutoShapeConstant.Index_146:
        return AutoShapeType.FlowChartOr;
      case AutoShapeConstant.Index_147:
        return AutoShapeType.FlowChartCollate;
      case AutoShapeConstant.Index_148:
        return AutoShapeType.FlowChartSort;
      case AutoShapeConstant.Index_149:
        return AutoShapeType.FlowChartExtract;
      case AutoShapeConstant.Index_150:
        return AutoShapeType.FlowChartMerge;
      case AutoShapeConstant.Index_152:
        return AutoShapeType.FlowChartStoredData;
      case AutoShapeConstant.Index_153:
        return AutoShapeType.FlowChartSequentialAccessStorage;
      case AutoShapeConstant.Index_154:
        return AutoShapeType.FlowChartMagneticDisk;
      case AutoShapeConstant.Index_155:
        return AutoShapeType.FlowChartDirectAccessStorage;
      case AutoShapeConstant.Index_156:
        return AutoShapeType.FlowChartDisplay;
      case AutoShapeConstant.Index_157:
        return AutoShapeType.FlowChartDelay;
      case AutoShapeConstant.Index_158:
        return AutoShapeType.FlowChartAlternateProcess;
      case AutoShapeConstant.Index_159:
        return AutoShapeType.FlowChartOffPageConnector;
      case AutoShapeConstant.Index_175:
        return AutoShapeType.MathPlus;
      case AutoShapeConstant.Index_176:
        return AutoShapeType.MathMinus;
      case AutoShapeConstant.Index_177:
        return AutoShapeType.MathMultiply;
      case AutoShapeConstant.Index_178:
        return AutoShapeType.MathDivision;
      case AutoShapeConstant.Index_179:
        return AutoShapeType.MathEqual;
      case AutoShapeConstant.Index_180:
        return AutoShapeType.MathNotEqual;
      default:
        return AutoShapeType.Unknown;
    }
  }

  internal static AutoShapeConstant GetAutoShapeConstant(AutoShapeType autoShapeType)
  {
    switch (autoShapeType)
    {
      case AutoShapeType.Rectangle:
        return AutoShapeConstant.Index_4;
      case AutoShapeType.Parallelogram:
        return AutoShapeConstant.Index_6;
      case AutoShapeType.Trapezoid:
        return AutoShapeConstant.Index_7;
      case AutoShapeType.Diamond:
        return AutoShapeConstant.Index_5;
      case AutoShapeType.RoundedRectangle:
        return AutoShapeConstant.Index_25;
      case AutoShapeType.Octagon:
        return AutoShapeConstant.Index_12;
      case AutoShapeType.IsoscelesTriangle:
        return AutoShapeConstant.Index_2;
      case AutoShapeType.RightTriangle:
        return AutoShapeConstant.Index_3;
      case AutoShapeType.Oval:
        return AutoShapeConstant.Index_34;
      case AutoShapeType.Hexagon:
        return AutoShapeConstant.Index_10;
      case AutoShapeType.Cross:
        return AutoShapeConstant.Index_130;
      case AutoShapeType.RegularPentagon:
        return AutoShapeConstant.Index_9;
      case AutoShapeType.Can:
        return AutoShapeConstant.Index_73;
      case AutoShapeType.Cube:
        return AutoShapeConstant.Index_72;
      case AutoShapeType.Bevel:
        return AutoShapeConstant.Index_82;
      case AutoShapeType.FoldedCorner:
        return AutoShapeConstant.Index_81;
      case AutoShapeType.SmileyFace:
        return AutoShapeConstant.Index_78;
      case AutoShapeType.Donut:
        return AutoShapeConstant.Index_41;
      case AutoShapeType.NoSymbol:
        return AutoShapeConstant.Index_42;
      case AutoShapeType.BlockArc:
        return AutoShapeConstant.Index_40;
      case AutoShapeType.Heart:
        return AutoShapeConstant.Index_75;
      case AutoShapeType.LightningBolt:
        return AutoShapeConstant.Index_74;
      case AutoShapeType.Sun:
        return AutoShapeConstant.Index_76;
      case AutoShapeType.Moon:
        return AutoShapeConstant.Index_77;
      case AutoShapeType.Arc:
        return AutoShapeConstant.Index_88;
      case AutoShapeType.DoubleBracket:
        return AutoShapeConstant.Index_93;
      case AutoShapeType.DoubleBrace:
        return AutoShapeConstant.Index_94;
      case AutoShapeType.Plaque:
        return AutoShapeConstant.Index_33;
      case AutoShapeType.LeftBracket:
        return AutoShapeConstant.Index_89;
      case AutoShapeType.RightBracket:
        return AutoShapeConstant.Index_90;
      case AutoShapeType.LeftBrace:
        return AutoShapeConstant.Index_91;
      case AutoShapeType.RightBrace:
        return AutoShapeConstant.Index_92;
      case AutoShapeType.RightArrow:
        return AutoShapeConstant.Index_43;
      case AutoShapeType.LeftArrow:
        return AutoShapeConstant.Index_44;
      case AutoShapeType.UpArrow:
        return AutoShapeConstant.Index_45;
      case AutoShapeType.DownArrow:
        return AutoShapeConstant.Index_46;
      case AutoShapeType.LeftRightArrow:
        return AutoShapeConstant.Index_50;
      case AutoShapeType.UpDownArrow:
        return AutoShapeConstant.Index_51;
      case AutoShapeType.QuadArrow:
        return AutoShapeConstant.Index_54;
      case AutoShapeType.LeftRightUpArrow:
        return AutoShapeConstant.Index_53;
      case AutoShapeType.BentArrow:
        return AutoShapeConstant.Index_62;
      case AutoShapeType.UTurnArrow:
        return AutoShapeConstant.Index_63;
      case AutoShapeType.LeftUpArrow:
        return AutoShapeConstant.Index_52;
      case AutoShapeType.BentUpArrow:
        return AutoShapeConstant.Index_49;
      case AutoShapeType.CurvedRightArrow:
        return AutoShapeConstant.Index_67;
      case AutoShapeType.CurvedLeftArrow:
        return AutoShapeConstant.Index_68;
      case AutoShapeType.CurvedUpArrow:
        return AutoShapeConstant.Index_69;
      case AutoShapeType.CurvedDownArrow:
        return AutoShapeConstant.Index_70;
      case AutoShapeType.StripedRightArrow:
        return AutoShapeConstant.Index_47;
      case AutoShapeType.NotchedRightArrow:
        return AutoShapeConstant.Index_48;
      case AutoShapeType.Pentagon:
        return AutoShapeConstant.Index_36;
      case AutoShapeType.Chevron:
        return AutoShapeConstant.Index_37;
      case AutoShapeType.RightArrowCallout:
        return AutoShapeConstant.Index_56;
      case AutoShapeType.LeftArrowCallout:
        return AutoShapeConstant.Index_55;
      case AutoShapeType.UpArrowCallout:
        return AutoShapeConstant.Index_57;
      case AutoShapeType.DownArrowCallout:
        return AutoShapeConstant.Index_58;
      case AutoShapeType.LeftRightArrowCallout:
        return AutoShapeConstant.Index_59;
      case AutoShapeType.UpDownArrowCallout:
        return AutoShapeConstant.Index_60;
      case AutoShapeType.QuadArrowCallout:
        return AutoShapeConstant.Index_61;
      case AutoShapeType.CircularArrow:
        return AutoShapeConstant.Index_64;
      case AutoShapeType.FlowChartProcess:
        return AutoShapeConstant.Index_131;
      case AutoShapeType.FlowChartAlternateProcess:
        return AutoShapeConstant.Index_158;
      case AutoShapeType.FlowChartDecision:
        return AutoShapeConstant.Index_132;
      case AutoShapeType.FlowChartData:
        return AutoShapeConstant.Index_133;
      case AutoShapeType.FlowChartPredefinedProcess:
        return AutoShapeConstant.Index_134;
      case AutoShapeType.FlowChartInternalStorage:
        return AutoShapeConstant.Index_135;
      case AutoShapeType.FlowChartDocument:
        return AutoShapeConstant.Index_136;
      case AutoShapeType.FlowChartMultiDocument:
        return AutoShapeConstant.Index_137;
      case AutoShapeType.FlowChartTerminator:
        return AutoShapeConstant.Index_138;
      case AutoShapeType.FlowChartPreparation:
        return AutoShapeConstant.Index_139;
      case AutoShapeType.FlowChartManualInput:
        return AutoShapeConstant.Index_140;
      case AutoShapeType.FlowChartManualOperation:
        return AutoShapeConstant.Index_141;
      case AutoShapeType.FlowChartConnector:
        return AutoShapeConstant.Index_142;
      case AutoShapeType.FlowChartOffPageConnector:
        return AutoShapeConstant.Index_159;
      case AutoShapeType.FlowChartCard:
        return AutoShapeConstant.Index_143;
      case AutoShapeType.FlowChartPunchedTape:
        return AutoShapeConstant.Index_144;
      case AutoShapeType.FlowChartSummingJunction:
        return AutoShapeConstant.Index_145;
      case AutoShapeType.FlowChartOr:
        return AutoShapeConstant.Index_146;
      case AutoShapeType.FlowChartCollate:
        return AutoShapeConstant.Index_147;
      case AutoShapeType.FlowChartSort:
        return AutoShapeConstant.Index_148;
      case AutoShapeType.FlowChartExtract:
        return AutoShapeConstant.Index_149;
      case AutoShapeType.FlowChartMerge:
        return AutoShapeConstant.Index_150;
      case AutoShapeType.FlowChartStoredData:
        return AutoShapeConstant.Index_152;
      case AutoShapeType.FlowChartDelay:
        return AutoShapeConstant.Index_157;
      case AutoShapeType.FlowChartSequentialAccessStorage:
        return AutoShapeConstant.Index_153;
      case AutoShapeType.FlowChartMagneticDisk:
        return AutoShapeConstant.Index_154;
      case AutoShapeType.FlowChartDirectAccessStorage:
        return AutoShapeConstant.Index_155;
      case AutoShapeType.FlowChartDisplay:
        return AutoShapeConstant.Index_156;
      case AutoShapeType.Explosion1:
        return AutoShapeConstant.Index_79;
      case AutoShapeType.Explosion2:
        return AutoShapeConstant.Index_80;
      case AutoShapeType.Star4Point:
        return AutoShapeConstant.Index_15;
      case AutoShapeType.Star5Point:
        return AutoShapeConstant.Index_16;
      case AutoShapeType.Star8Point:
        return AutoShapeConstant.Index_19;
      case AutoShapeType.Star16Point:
        return AutoShapeConstant.Index_22;
      case AutoShapeType.Star24Point:
        return AutoShapeConstant.Index_23;
      case AutoShapeType.Star32Point:
        return AutoShapeConstant.Index_24;
      case AutoShapeType.UpRibbon:
        return AutoShapeConstant.Index_122;
      case AutoShapeType.DownRibbon:
        return AutoShapeConstant.Index_121;
      case AutoShapeType.CurvedUpRibbon:
        return AutoShapeConstant.Index_124;
      case AutoShapeType.CurvedDownRibbon:
        return AutoShapeConstant.Index_123;
      case AutoShapeType.VerticalScroll:
        return AutoShapeConstant.Index_126;
      case AutoShapeType.HorizontalScroll:
        return AutoShapeConstant.Index_127;
      case AutoShapeType.Wave:
        return AutoShapeConstant.Index_128;
      case AutoShapeType.DoubleWave:
        return AutoShapeConstant.Index_129;
      case AutoShapeType.RectangularCallout:
        return AutoShapeConstant.Index_116;
      case AutoShapeType.RoundedRectangularCallout:
        return AutoShapeConstant.Index_117;
      case AutoShapeType.OvalCallout:
        return AutoShapeConstant.Index_118;
      case AutoShapeType.CloudCallout:
        return AutoShapeConstant.Index_119;
      case AutoShapeType.LineCallout1:
        return AutoShapeConstant.Index_110;
      case AutoShapeType.LineCallout2:
        return AutoShapeConstant.Index_111;
      case AutoShapeType.LineCallout3:
        return AutoShapeConstant.Index_112;
      case AutoShapeType.LineCallout1NoBorder:
        return AutoShapeConstant.Index_104;
      case AutoShapeType.LineCallout1AccentBar:
        return AutoShapeConstant.Index_107;
      case AutoShapeType.LineCallout2AccentBar:
        return AutoShapeConstant.Index_108;
      case AutoShapeType.LineCallout3AccentBar:
        return AutoShapeConstant.Index_109;
      case AutoShapeType.LineCallout2NoBorder:
        return AutoShapeConstant.Index_105;
      case AutoShapeType.LineCallout3NoBorder:
        return AutoShapeConstant.Index_106;
      case AutoShapeType.LineCallout1BorderAndAccentBar:
        return AutoShapeConstant.Index_113;
      case AutoShapeType.LineCallout2BorderAndAccentBar:
        return AutoShapeConstant.Index_114;
      case AutoShapeType.LineCallout3BorderAndAccentBar:
        return AutoShapeConstant.Index_115;
      case AutoShapeType.DiagonalStripe:
        return AutoShapeConstant.Index_86;
      case AutoShapeType.Pie:
        return AutoShapeConstant.Index_39;
      case AutoShapeType.Decagon:
        return AutoShapeConstant.Index_13;
      case AutoShapeType.Heptagon:
        return AutoShapeConstant.Index_11;
      case AutoShapeType.Dodecagon:
        return AutoShapeConstant.Index_14;
      case AutoShapeType.Star6Point:
        return AutoShapeConstant.Index_17;
      case AutoShapeType.Star7Point:
        return AutoShapeConstant.Index_18;
      case AutoShapeType.Star10Point:
        return AutoShapeConstant.Index_20;
      case AutoShapeType.Star12Point:
        return AutoShapeConstant.Index_21;
      case AutoShapeType.RoundSingleCornerRectangle:
        return AutoShapeConstant.Index_26;
      case AutoShapeType.RoundSameSideCornerRectangle:
        return AutoShapeConstant.Index_27;
      case AutoShapeType.RoundDiagonalCornerRectangle:
        return AutoShapeConstant.Index_28;
      case AutoShapeType.SnipAndRoundSingleCornerRectangle:
        return AutoShapeConstant.Index_29;
      case AutoShapeType.SnipSingleCornerRectangle:
        return AutoShapeConstant.Index_30;
      case AutoShapeType.SnipSameSideCornerRectangle:
        return AutoShapeConstant.Index_31;
      case AutoShapeType.SnipDiagonalCornerRectangle:
        return AutoShapeConstant.Index_32;
      case AutoShapeType.Frame:
        return AutoShapeConstant.Index_83;
      case AutoShapeType.HalfFrame:
        return AutoShapeConstant.Index_84;
      case AutoShapeType.Teardrop:
        return AutoShapeConstant.Index_35;
      case AutoShapeType.Chord:
        return AutoShapeConstant.Index_87;
      case AutoShapeType.L_Shape:
        return AutoShapeConstant.Index_85;
      case AutoShapeType.MathPlus:
        return AutoShapeConstant.Index_175;
      case AutoShapeType.MathMinus:
        return AutoShapeConstant.Index_176;
      case AutoShapeType.MathMultiply:
        return AutoShapeConstant.Index_177;
      case AutoShapeType.MathDivision:
        return AutoShapeConstant.Index_178;
      case AutoShapeType.MathEqual:
        return AutoShapeConstant.Index_179;
      case AutoShapeType.MathNotEqual:
        return AutoShapeConstant.Index_180;
      case AutoShapeType.Cloud:
        return AutoShapeConstant.Index_120;
      case AutoShapeType.Line:
        return AutoShapeConstant.Index_0;
      case AutoShapeType.StraightConnector:
        return AutoShapeConstant.Index_95;
      case AutoShapeType.ElbowConnector:
        return AutoShapeConstant.Index_97;
      case AutoShapeType.CurvedConnector:
        return AutoShapeConstant.Index_101;
      case AutoShapeType.BentConnector2:
        return AutoShapeConstant.Index_96;
      case AutoShapeType.BentConnector4:
        return AutoShapeConstant.Index_98;
      case AutoShapeType.BentConnector5:
        return AutoShapeConstant.Index_99;
      case AutoShapeType.CurvedConnector2:
        return AutoShapeConstant.Index_100;
      case AutoShapeType.CurvedConnector4:
        return AutoShapeConstant.Index_102;
      case AutoShapeType.CurvedConnector5:
        return AutoShapeConstant.Index_103;
      default:
        return AutoShapeConstant.Index_187;
    }
  }

  internal static string GetAutoShapeString(AutoShapeConstant shapeConstant)
  {
    switch (shapeConstant)
    {
      case AutoShapeConstant.Index_0:
        return "line";
      case AutoShapeConstant.Index_1:
        return "lineInv";
      case AutoShapeConstant.Index_2:
        return "triangle";
      case AutoShapeConstant.Index_3:
        return "rtTriangle";
      case AutoShapeConstant.Index_4:
        return "rect";
      case AutoShapeConstant.Index_5:
        return "diamond";
      case AutoShapeConstant.Index_6:
        return "parallelogram";
      case AutoShapeConstant.Index_7:
        return "trapezoid";
      case AutoShapeConstant.Index_8:
        return "nonIsoscelesTrapezoid";
      case AutoShapeConstant.Index_9:
        return "pentagon";
      case AutoShapeConstant.Index_10:
        return "hexagon";
      case AutoShapeConstant.Index_11:
        return "heptagon";
      case AutoShapeConstant.Index_12:
        return "octagon";
      case AutoShapeConstant.Index_13:
        return "decagon";
      case AutoShapeConstant.Index_14:
        return "dodecagon";
      case AutoShapeConstant.Index_15:
        return "star4";
      case AutoShapeConstant.Index_16:
        return "star5";
      case AutoShapeConstant.Index_17:
        return "star6";
      case AutoShapeConstant.Index_18:
        return "star7";
      case AutoShapeConstant.Index_19:
        return "star8";
      case AutoShapeConstant.Index_20:
        return "star10";
      case AutoShapeConstant.Index_21:
        return "star12";
      case AutoShapeConstant.Index_22:
        return "star16";
      case AutoShapeConstant.Index_23:
        return "star24";
      case AutoShapeConstant.Index_24:
        return "star32";
      case AutoShapeConstant.Index_25:
        return "roundRect";
      case AutoShapeConstant.Index_26:
        return "round1Rect";
      case AutoShapeConstant.Index_27:
        return "round2SameRect";
      case AutoShapeConstant.Index_28:
        return "round2DiagRect";
      case AutoShapeConstant.Index_29:
        return "snipRoundRect";
      case AutoShapeConstant.Index_30:
        return "snip1Rect";
      case AutoShapeConstant.Index_31:
        return "snip2SameRect";
      case AutoShapeConstant.Index_32:
        return "snip2DiagRect";
      case AutoShapeConstant.Index_33:
        return "plaque";
      case AutoShapeConstant.Index_34:
        return "ellipse";
      case AutoShapeConstant.Index_35:
        return "teardrop";
      case AutoShapeConstant.Index_36:
        return "homePlate";
      case AutoShapeConstant.Index_37:
        return "chevron";
      case AutoShapeConstant.Index_38:
        return "pieWedge";
      case AutoShapeConstant.Index_39:
        return "pie";
      case AutoShapeConstant.Index_40:
        return "blockArc";
      case AutoShapeConstant.Index_41:
        return "donut";
      case AutoShapeConstant.Index_42:
        return "noSmoking";
      case AutoShapeConstant.Index_43:
        return "rightArrow";
      case AutoShapeConstant.Index_44:
        return "leftArrow";
      case AutoShapeConstant.Index_45:
        return "upArrow";
      case AutoShapeConstant.Index_46:
        return "downArrow";
      case AutoShapeConstant.Index_47:
        return "stripedRightArrow";
      case AutoShapeConstant.Index_48:
        return "notchedRightArrow";
      case AutoShapeConstant.Index_49:
        return "bentUpArrow";
      case AutoShapeConstant.Index_50:
        return "leftRightArrow";
      case AutoShapeConstant.Index_51:
        return "upDownArrow";
      case AutoShapeConstant.Index_52:
        return "leftUpArrow";
      case AutoShapeConstant.Index_53:
        return "leftRightUpArrow";
      case AutoShapeConstant.Index_54:
        return "quadArrow";
      case AutoShapeConstant.Index_55:
        return "leftArrowCallout";
      case AutoShapeConstant.Index_56:
        return "rightArrowCallout";
      case AutoShapeConstant.Index_57:
        return "upArrowCallout";
      case AutoShapeConstant.Index_58:
        return "downArrowCallout";
      case AutoShapeConstant.Index_59:
        return "leftRightArrowCallout";
      case AutoShapeConstant.Index_60:
        return "upDownArrowCallout";
      case AutoShapeConstant.Index_61:
        return "quadArrowCallout";
      case AutoShapeConstant.Index_62:
        return "bentArrow";
      case AutoShapeConstant.Index_63:
        return "uturnArrow";
      case AutoShapeConstant.Index_64:
        return "circularArrow";
      case AutoShapeConstant.Index_65:
        return "leftCircularArrow";
      case AutoShapeConstant.Index_66:
        return "leftRightCircularArrow";
      case AutoShapeConstant.Index_67:
        return "curvedRightArrow";
      case AutoShapeConstant.Index_68:
        return "curvedLeftArrow";
      case AutoShapeConstant.Index_69:
        return "curvedUpArrow";
      case AutoShapeConstant.Index_70:
        return "curvedDownArrow";
      case AutoShapeConstant.Index_71:
        return "swooshArrow";
      case AutoShapeConstant.Index_72:
        return "cube";
      case AutoShapeConstant.Index_73:
        return "can";
      case AutoShapeConstant.Index_74:
        return "lightningBolt";
      case AutoShapeConstant.Index_75:
        return "heart";
      case AutoShapeConstant.Index_76:
        return "sun";
      case AutoShapeConstant.Index_77:
        return "moon";
      case AutoShapeConstant.Index_78:
        return "smileyFace";
      case AutoShapeConstant.Index_79:
        return "irregularSeal1";
      case AutoShapeConstant.Index_80:
        return "irregularSeal2";
      case AutoShapeConstant.Index_81:
        return "foldedCorner";
      case AutoShapeConstant.Index_82:
        return "bevel";
      case AutoShapeConstant.Index_83:
        return "frame";
      case AutoShapeConstant.Index_84:
        return "halfFrame";
      case AutoShapeConstant.Index_85:
        return "corner";
      case AutoShapeConstant.Index_86:
        return "diagStripe";
      case AutoShapeConstant.Index_87:
        return "chord";
      case AutoShapeConstant.Index_88:
        return "arc";
      case AutoShapeConstant.Index_89:
        return "leftBracket";
      case AutoShapeConstant.Index_90:
        return "rightBracket";
      case AutoShapeConstant.Index_91:
        return "leftBrace";
      case AutoShapeConstant.Index_92:
        return "rightBrace";
      case AutoShapeConstant.Index_93:
        return "bracketPair";
      case AutoShapeConstant.Index_94:
        return "bracePair";
      case AutoShapeConstant.Index_95:
        return "straightConnector1";
      case AutoShapeConstant.Index_96:
        return "bentConnector2";
      case AutoShapeConstant.Index_97:
        return "bentConnector3";
      case AutoShapeConstant.Index_98:
        return "bentConnector4";
      case AutoShapeConstant.Index_99:
        return "bentConnector5";
      case AutoShapeConstant.Index_100:
        return "curvedConnector2";
      case AutoShapeConstant.Index_101:
        return "curvedConnector3";
      case AutoShapeConstant.Index_102:
        return "curvedConnector4";
      case AutoShapeConstant.Index_103:
        return "curvedConnector5";
      case AutoShapeConstant.Index_104:
        return "callout1";
      case AutoShapeConstant.Index_105:
        return "callout2";
      case AutoShapeConstant.Index_106:
        return "callout3";
      case AutoShapeConstant.Index_107:
        return "accentCallout1";
      case AutoShapeConstant.Index_108:
        return "accentCallout2";
      case AutoShapeConstant.Index_109:
        return "accentCallout3";
      case AutoShapeConstant.Index_110:
        return "borderCallout1";
      case AutoShapeConstant.Index_111:
        return "borderCallout2";
      case AutoShapeConstant.Index_112:
        return "borderCallout3";
      case AutoShapeConstant.Index_113:
        return "accentBorderCallout1";
      case AutoShapeConstant.Index_114:
        return "accentBorderCallout2";
      case AutoShapeConstant.Index_115:
        return "accentBorderCallout3";
      case AutoShapeConstant.Index_116:
        return "wedgeRectCallout";
      case AutoShapeConstant.Index_117:
        return "wedgeRoundRectCallout";
      case AutoShapeConstant.Index_118:
        return "wedgeEllipseCallout";
      case AutoShapeConstant.Index_119:
        return "cloudCallout";
      case AutoShapeConstant.Index_120:
        return "cloud";
      case AutoShapeConstant.Index_121:
        return "ribbon";
      case AutoShapeConstant.Index_122:
        return "ribbon2";
      case AutoShapeConstant.Index_123:
        return "ellipseRibbon";
      case AutoShapeConstant.Index_124:
        return "ellipseRibbon2";
      case AutoShapeConstant.Index_125:
        return "leftRightRibbon";
      case AutoShapeConstant.Index_126:
        return "verticalScroll";
      case AutoShapeConstant.Index_127:
        return "horizontalScroll";
      case AutoShapeConstant.Index_128:
        return "wave";
      case AutoShapeConstant.Index_129:
        return "doubleWave";
      case AutoShapeConstant.Index_130:
        return "plus";
      case AutoShapeConstant.Index_131:
        return "flowChartProcess";
      case AutoShapeConstant.Index_132:
        return "flowChartDecision";
      case AutoShapeConstant.Index_133:
        return "flowChartInputOutput";
      case AutoShapeConstant.Index_134:
        return "flowChartPredefinedProcess";
      case AutoShapeConstant.Index_135:
        return "flowChartInternalStorage";
      case AutoShapeConstant.Index_136:
        return "flowChartDocument";
      case AutoShapeConstant.Index_137:
        return "flowChartMultidocument";
      case AutoShapeConstant.Index_138:
        return "flowChartTerminator";
      case AutoShapeConstant.Index_139:
        return "flowChartPreparation";
      case AutoShapeConstant.Index_140:
        return "flowChartManualInput";
      case AutoShapeConstant.Index_141:
        return "flowChartManualOperation";
      case AutoShapeConstant.Index_142:
        return "flowChartConnector";
      case AutoShapeConstant.Index_143:
        return "flowChartPunchedCard";
      case AutoShapeConstant.Index_144:
        return "flowChartPunchedTape";
      case AutoShapeConstant.Index_145:
        return "flowChartSummingJunction";
      case AutoShapeConstant.Index_146:
        return "flowChartOr";
      case AutoShapeConstant.Index_147:
        return "flowChartCollate";
      case AutoShapeConstant.Index_148:
        return "flowChartSort";
      case AutoShapeConstant.Index_149:
        return "flowChartExtract";
      case AutoShapeConstant.Index_150:
        return "flowChartMerge";
      case AutoShapeConstant.Index_151:
        return "flowChartOfflineStorage";
      case AutoShapeConstant.Index_152:
        return "flowChartOnlineStorage";
      case AutoShapeConstant.Index_153:
        return "flowChartMagneticTape";
      case AutoShapeConstant.Index_154:
        return "flowChartMagneticDisk";
      case AutoShapeConstant.Index_155:
        return "flowChartMagneticDrum";
      case AutoShapeConstant.Index_156:
        return "flowChartDisplay";
      case AutoShapeConstant.Index_157:
        return "flowChartDelay";
      case AutoShapeConstant.Index_158:
        return "flowChartAlternateProcess";
      case AutoShapeConstant.Index_159:
        return "flowChartOffpageConnector";
      case AutoShapeConstant.Index_160:
        return "actionButtonBlank";
      case AutoShapeConstant.Index_161:
        return "actionButtonHome";
      case AutoShapeConstant.Index_162:
        return "actionButtonHelp";
      case AutoShapeConstant.Index_163:
        return "actionButtonInformation";
      case AutoShapeConstant.Index_164:
        return "actionButtonForwardNext";
      case AutoShapeConstant.Index_165:
        return "actionButtonBackPrevious";
      case AutoShapeConstant.Index_166:
        return "actionButtonEnd";
      case AutoShapeConstant.Index_167:
        return "actionButtonBeginning";
      case AutoShapeConstant.Index_168:
        return "actionButtonReturn";
      case AutoShapeConstant.Index_169:
        return "actionButtonDocument";
      case AutoShapeConstant.Index_170:
        return "actionButtonSound";
      case AutoShapeConstant.Index_171:
        return "actionButtonMovie";
      case AutoShapeConstant.Index_172:
        return "gear6";
      case AutoShapeConstant.Index_173:
        return "gear9";
      case AutoShapeConstant.Index_174:
        return "funnel";
      case AutoShapeConstant.Index_175:
        return "mathPlus";
      case AutoShapeConstant.Index_176:
        return "mathMinus";
      case AutoShapeConstant.Index_177:
        return "mathMultiply";
      case AutoShapeConstant.Index_178:
        return "mathDivide";
      case AutoShapeConstant.Index_179:
        return "mathEqual";
      case AutoShapeConstant.Index_180:
        return "mathNotEqual";
      case AutoShapeConstant.Index_181:
        return "cornerTabs";
      case AutoShapeConstant.Index_182:
        return "squareTabs";
      case AutoShapeConstant.Index_183:
        return "plaqueTabs";
      case AutoShapeConstant.Index_184:
        return "chartX";
      case AutoShapeConstant.Index_185:
        return "chartStar";
      case AutoShapeConstant.Index_186:
        return "chartPlus";
      default:
        return (string) null;
    }
  }
}
