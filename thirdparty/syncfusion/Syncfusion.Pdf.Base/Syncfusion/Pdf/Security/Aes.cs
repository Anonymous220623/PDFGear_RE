// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.Aes
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Security;

public class Aes
{
  private int Nb;
  private int Nk;
  private int Nr;
  private byte[] key;
  private byte[,] Sbox;
  private byte[,] iSbox;
  private byte[,] w;
  private byte[,] Rcon;
  private byte[,] State;
  private Aes.KeySize mKeySize;

  public Aes(Aes.KeySize keySize, byte[] keyBytes)
  {
    this.mKeySize = keySize;
    this.SetNbNkNr(keySize);
    this.key = new byte[this.Nk * 4];
    keyBytes.CopyTo((Array) this.key, 0);
    this.Initialize();
  }

  private void Initialize()
  {
    this.BuildSbox();
    this.BuildInvSbox();
    this.BuildRcon();
    this.KeyExpansion();
  }

  public int Cipher(byte[] input, byte[] output, int outOff)
  {
    this.Initialize();
    this.State = new byte[4, this.Nb];
    for (int index = 0; index < 4 * this.Nb; ++index)
      this.State[index % 4, index / 4] = input[index];
    this.AddRoundKey(0);
    for (int round = 1; round <= this.Nr - 1; ++round)
    {
      this.SubBytes();
      this.ShiftRows();
      this.MixColumns();
      this.AddRoundKey(round);
    }
    this.SubBytes();
    this.ShiftRows();
    this.AddRoundKey(this.Nr);
    for (int index = 0; index < 4 * this.Nb; ++index)
      output[outOff++] = this.State[index % 4, index / 4];
    return 16 /*0x10*/;
  }

  public int InvCipher(byte[] input, byte[] output, int outOff)
  {
    this.State = new byte[4, this.Nb];
    for (int index = 0; index < 4 * this.Nb; ++index)
      this.State[index % 4, index / 4] = input[index];
    this.AddRoundKey(this.Nr);
    for (int round = this.Nr - 1; round >= 1; --round)
    {
      this.InvShiftRows();
      this.InvSubBytes();
      this.AddRoundKey(round);
      this.InvMixColumns();
    }
    this.InvShiftRows();
    this.InvSubBytes();
    this.AddRoundKey(0);
    for (int index = 0; index < 4 * this.Nb; ++index)
      output[outOff++] = this.State[index % 4, index / 4];
    return 16 /*0x10*/;
  }

  private void SetNbNkNr(Aes.KeySize keySize)
  {
    this.Nb = 4;
    switch (keySize)
    {
      case Aes.KeySize.Bits128:
        this.Nk = 4;
        this.Nr = 10;
        break;
      case Aes.KeySize.Bits192:
        this.Nk = 6;
        this.Nr = 12;
        break;
      case Aes.KeySize.Bits256:
        this.Nk = 8;
        this.Nr = 14;
        break;
    }
  }

  private void BuildSbox()
  {
    this.Sbox = new byte[16 /*0x10*/, 16 /*0x10*/]
    {
      {
        (byte) 99,
        (byte) 124,
        (byte) 119,
        (byte) 123,
        (byte) 242,
        (byte) 107,
        (byte) 111,
        (byte) 197,
        (byte) 48 /*0x30*/,
        (byte) 1,
        (byte) 103,
        (byte) 43,
        (byte) 254,
        (byte) 215,
        (byte) 171,
        (byte) 118
      },
      {
        (byte) 202,
        (byte) 130,
        (byte) 201,
        (byte) 125,
        (byte) 250,
        (byte) 89,
        (byte) 71,
        (byte) 240 /*0xF0*/,
        (byte) 173,
        (byte) 212,
        (byte) 162,
        (byte) 175,
        (byte) 156,
        (byte) 164,
        (byte) 114,
        (byte) 192 /*0xC0*/
      },
      {
        (byte) 183,
        (byte) 253,
        (byte) 147,
        (byte) 38,
        (byte) 54,
        (byte) 63 /*0x3F*/,
        (byte) 247,
        (byte) 204,
        (byte) 52,
        (byte) 165,
        (byte) 229,
        (byte) 241,
        (byte) 113,
        (byte) 216,
        (byte) 49,
        (byte) 21
      },
      {
        (byte) 4,
        (byte) 199,
        (byte) 35,
        (byte) 195,
        (byte) 24,
        (byte) 150,
        (byte) 5,
        (byte) 154,
        (byte) 7,
        (byte) 18,
        (byte) 128 /*0x80*/,
        (byte) 226,
        (byte) 235,
        (byte) 39,
        (byte) 178,
        (byte) 117
      },
      {
        (byte) 9,
        (byte) 131,
        (byte) 44,
        (byte) 26,
        (byte) 27,
        (byte) 110,
        (byte) 90,
        (byte) 160 /*0xA0*/,
        (byte) 82,
        (byte) 59,
        (byte) 214,
        (byte) 179,
        (byte) 41,
        (byte) 227,
        (byte) 47,
        (byte) 132
      },
      {
        (byte) 83,
        (byte) 209,
        (byte) 0,
        (byte) 237,
        (byte) 32 /*0x20*/,
        (byte) 252,
        (byte) 177,
        (byte) 91,
        (byte) 106,
        (byte) 203,
        (byte) 190,
        (byte) 57,
        (byte) 74,
        (byte) 76,
        (byte) 88,
        (byte) 207
      },
      {
        (byte) 208 /*0xD0*/,
        (byte) 239,
        (byte) 170,
        (byte) 251,
        (byte) 67,
        (byte) 77,
        (byte) 51,
        (byte) 133,
        (byte) 69,
        (byte) 249,
        (byte) 2,
        (byte) 127 /*0x7F*/,
        (byte) 80 /*0x50*/,
        (byte) 60,
        (byte) 159,
        (byte) 168
      },
      {
        (byte) 81,
        (byte) 163,
        (byte) 64 /*0x40*/,
        (byte) 143,
        (byte) 146,
        (byte) 157,
        (byte) 56,
        (byte) 245,
        (byte) 188,
        (byte) 182,
        (byte) 218,
        (byte) 33,
        (byte) 16 /*0x10*/,
        byte.MaxValue,
        (byte) 243,
        (byte) 210
      },
      {
        (byte) 205,
        (byte) 12,
        (byte) 19,
        (byte) 236,
        (byte) 95,
        (byte) 151,
        (byte) 68,
        (byte) 23,
        (byte) 196,
        (byte) 167,
        (byte) 126,
        (byte) 61,
        (byte) 100,
        (byte) 93,
        (byte) 25,
        (byte) 115
      },
      {
        (byte) 96 /*0x60*/,
        (byte) 129,
        (byte) 79,
        (byte) 220,
        (byte) 34,
        (byte) 42,
        (byte) 144 /*0x90*/,
        (byte) 136,
        (byte) 70,
        (byte) 238,
        (byte) 184,
        (byte) 20,
        (byte) 222,
        (byte) 94,
        (byte) 11,
        (byte) 219
      },
      {
        (byte) 224 /*0xE0*/,
        (byte) 50,
        (byte) 58,
        (byte) 10,
        (byte) 73,
        (byte) 6,
        (byte) 36,
        (byte) 92,
        (byte) 194,
        (byte) 211,
        (byte) 172,
        (byte) 98,
        (byte) 145,
        (byte) 149,
        (byte) 228,
        (byte) 121
      },
      {
        (byte) 231,
        (byte) 200,
        (byte) 55,
        (byte) 109,
        (byte) 141,
        (byte) 213,
        (byte) 78,
        (byte) 169,
        (byte) 108,
        (byte) 86,
        (byte) 244,
        (byte) 234,
        (byte) 101,
        (byte) 122,
        (byte) 174,
        (byte) 8
      },
      {
        (byte) 186,
        (byte) 120,
        (byte) 37,
        (byte) 46,
        (byte) 28,
        (byte) 166,
        (byte) 180,
        (byte) 198,
        (byte) 232,
        (byte) 221,
        (byte) 116,
        (byte) 31 /*0x1F*/,
        (byte) 75,
        (byte) 189,
        (byte) 139,
        (byte) 138
      },
      {
        (byte) 112 /*0x70*/,
        (byte) 62,
        (byte) 181,
        (byte) 102,
        (byte) 72,
        (byte) 3,
        (byte) 246,
        (byte) 14,
        (byte) 97,
        (byte) 53,
        (byte) 87,
        (byte) 185,
        (byte) 134,
        (byte) 193,
        (byte) 29,
        (byte) 158
      },
      {
        (byte) 225,
        (byte) 248,
        (byte) 152,
        (byte) 17,
        (byte) 105,
        (byte) 217,
        (byte) 142,
        (byte) 148,
        (byte) 155,
        (byte) 30,
        (byte) 135,
        (byte) 233,
        (byte) 206,
        (byte) 85,
        (byte) 40,
        (byte) 223
      },
      {
        (byte) 140,
        (byte) 161,
        (byte) 137,
        (byte) 13,
        (byte) 191,
        (byte) 230,
        (byte) 66,
        (byte) 104,
        (byte) 65,
        (byte) 153,
        (byte) 45,
        (byte) 15,
        (byte) 176 /*0xB0*/,
        (byte) 84,
        (byte) 187,
        (byte) 22
      }
    };
  }

  private void BuildInvSbox()
  {
    this.iSbox = new byte[16 /*0x10*/, 16 /*0x10*/]
    {
      {
        (byte) 82,
        (byte) 9,
        (byte) 106,
        (byte) 213,
        (byte) 48 /*0x30*/,
        (byte) 54,
        (byte) 165,
        (byte) 56,
        (byte) 191,
        (byte) 64 /*0x40*/,
        (byte) 163,
        (byte) 158,
        (byte) 129,
        (byte) 243,
        (byte) 215,
        (byte) 251
      },
      {
        (byte) 124,
        (byte) 227,
        (byte) 57,
        (byte) 130,
        (byte) 155,
        (byte) 47,
        byte.MaxValue,
        (byte) 135,
        (byte) 52,
        (byte) 142,
        (byte) 67,
        (byte) 68,
        (byte) 196,
        (byte) 222,
        (byte) 233,
        (byte) 203
      },
      {
        (byte) 84,
        (byte) 123,
        (byte) 148,
        (byte) 50,
        (byte) 166,
        (byte) 194,
        (byte) 35,
        (byte) 61,
        (byte) 238,
        (byte) 76,
        (byte) 149,
        (byte) 11,
        (byte) 66,
        (byte) 250,
        (byte) 195,
        (byte) 78
      },
      {
        (byte) 8,
        (byte) 46,
        (byte) 161,
        (byte) 102,
        (byte) 40,
        (byte) 217,
        (byte) 36,
        (byte) 178,
        (byte) 118,
        (byte) 91,
        (byte) 162,
        (byte) 73,
        (byte) 109,
        (byte) 139,
        (byte) 209,
        (byte) 37
      },
      {
        (byte) 114,
        (byte) 248,
        (byte) 246,
        (byte) 100,
        (byte) 134,
        (byte) 104,
        (byte) 152,
        (byte) 22,
        (byte) 212,
        (byte) 164,
        (byte) 92,
        (byte) 204,
        (byte) 93,
        (byte) 101,
        (byte) 182,
        (byte) 146
      },
      {
        (byte) 108,
        (byte) 112 /*0x70*/,
        (byte) 72,
        (byte) 80 /*0x50*/,
        (byte) 253,
        (byte) 237,
        (byte) 185,
        (byte) 218,
        (byte) 94,
        (byte) 21,
        (byte) 70,
        (byte) 87,
        (byte) 167,
        (byte) 141,
        (byte) 157,
        (byte) 132
      },
      {
        (byte) 144 /*0x90*/,
        (byte) 216,
        (byte) 171,
        (byte) 0,
        (byte) 140,
        (byte) 188,
        (byte) 211,
        (byte) 10,
        (byte) 247,
        (byte) 228,
        (byte) 88,
        (byte) 5,
        (byte) 184,
        (byte) 179,
        (byte) 69,
        (byte) 6
      },
      {
        (byte) 208 /*0xD0*/,
        (byte) 44,
        (byte) 30,
        (byte) 143,
        (byte) 202,
        (byte) 63 /*0x3F*/,
        (byte) 15,
        (byte) 2,
        (byte) 193,
        (byte) 175,
        (byte) 189,
        (byte) 3,
        (byte) 1,
        (byte) 19,
        (byte) 138,
        (byte) 107
      },
      {
        (byte) 58,
        (byte) 145,
        (byte) 17,
        (byte) 65,
        (byte) 79,
        (byte) 103,
        (byte) 220,
        (byte) 234,
        (byte) 151,
        (byte) 242,
        (byte) 207,
        (byte) 206,
        (byte) 240 /*0xF0*/,
        (byte) 180,
        (byte) 230,
        (byte) 115
      },
      {
        (byte) 150,
        (byte) 172,
        (byte) 116,
        (byte) 34,
        (byte) 231,
        (byte) 173,
        (byte) 53,
        (byte) 133,
        (byte) 226,
        (byte) 249,
        (byte) 55,
        (byte) 232,
        (byte) 28,
        (byte) 117,
        (byte) 223,
        (byte) 110
      },
      {
        (byte) 71,
        (byte) 241,
        (byte) 26,
        (byte) 113,
        (byte) 29,
        (byte) 41,
        (byte) 197,
        (byte) 137,
        (byte) 111,
        (byte) 183,
        (byte) 98,
        (byte) 14,
        (byte) 170,
        (byte) 24,
        (byte) 190,
        (byte) 27
      },
      {
        (byte) 252,
        (byte) 86,
        (byte) 62,
        (byte) 75,
        (byte) 198,
        (byte) 210,
        (byte) 121,
        (byte) 32 /*0x20*/,
        (byte) 154,
        (byte) 219,
        (byte) 192 /*0xC0*/,
        (byte) 254,
        (byte) 120,
        (byte) 205,
        (byte) 90,
        (byte) 244
      },
      {
        (byte) 31 /*0x1F*/,
        (byte) 221,
        (byte) 168,
        (byte) 51,
        (byte) 136,
        (byte) 7,
        (byte) 199,
        (byte) 49,
        (byte) 177,
        (byte) 18,
        (byte) 16 /*0x10*/,
        (byte) 89,
        (byte) 39,
        (byte) 128 /*0x80*/,
        (byte) 236,
        (byte) 95
      },
      {
        (byte) 96 /*0x60*/,
        (byte) 81,
        (byte) 127 /*0x7F*/,
        (byte) 169,
        (byte) 25,
        (byte) 181,
        (byte) 74,
        (byte) 13,
        (byte) 45,
        (byte) 229,
        (byte) 122,
        (byte) 159,
        (byte) 147,
        (byte) 201,
        (byte) 156,
        (byte) 239
      },
      {
        (byte) 160 /*0xA0*/,
        (byte) 224 /*0xE0*/,
        (byte) 59,
        (byte) 77,
        (byte) 174,
        (byte) 42,
        (byte) 245,
        (byte) 176 /*0xB0*/,
        (byte) 200,
        (byte) 235,
        (byte) 187,
        (byte) 60,
        (byte) 131,
        (byte) 83,
        (byte) 153,
        (byte) 97
      },
      {
        (byte) 23,
        (byte) 43,
        (byte) 4,
        (byte) 126,
        (byte) 186,
        (byte) 119,
        (byte) 214,
        (byte) 38,
        (byte) 225,
        (byte) 105,
        (byte) 20,
        (byte) 99,
        (byte) 85,
        (byte) 33,
        (byte) 12,
        (byte) 125
      }
    };
  }

  private void BuildRcon()
  {
    this.Rcon = new byte[11, 4]
    {
      {
        (byte) 0,
        (byte) 0,
        (byte) 0,
        (byte) 0
      },
      {
        (byte) 1,
        (byte) 0,
        (byte) 0,
        (byte) 0
      },
      {
        (byte) 2,
        (byte) 0,
        (byte) 0,
        (byte) 0
      },
      {
        (byte) 4,
        (byte) 0,
        (byte) 0,
        (byte) 0
      },
      {
        (byte) 8,
        (byte) 0,
        (byte) 0,
        (byte) 0
      },
      {
        (byte) 16 /*0x10*/,
        (byte) 0,
        (byte) 0,
        (byte) 0
      },
      {
        (byte) 32 /*0x20*/,
        (byte) 0,
        (byte) 0,
        (byte) 0
      },
      {
        (byte) 64 /*0x40*/,
        (byte) 0,
        (byte) 0,
        (byte) 0
      },
      {
        (byte) 128 /*0x80*/,
        (byte) 0,
        (byte) 0,
        (byte) 0
      },
      {
        (byte) 27,
        (byte) 0,
        (byte) 0,
        (byte) 0
      },
      {
        (byte) 54,
        (byte) 0,
        (byte) 0,
        (byte) 0
      }
    };
  }

  private void AddRoundKey(int round)
  {
    for (int index1 = 0; index1 < 4; ++index1)
    {
      for (int index2 = 0; index2 < 4; ++index2)
        this.State[index1, index2] = (byte) ((uint) this.State[index1, index2] ^ (uint) this.w[round * 4 + index2, index1]);
    }
  }

  private void SubBytes()
  {
    for (int index1 = 0; index1 < 4; ++index1)
    {
      for (int index2 = 0; index2 < 4; ++index2)
        this.State[index1, index2] = this.Sbox[(int) this.State[index1, index2] >> 4, (int) this.State[index1, index2] & 15];
    }
  }

  private void InvSubBytes()
  {
    for (int index1 = 0; index1 < 4; ++index1)
    {
      for (int index2 = 0; index2 < 4; ++index2)
        this.State[index1, index2] = this.iSbox[(int) this.State[index1, index2] >> 4, (int) this.State[index1, index2] & 15];
    }
  }

  private void ShiftRows()
  {
    byte[,] numArray = new byte[4, 4];
    for (int index1 = 0; index1 < 4; ++index1)
    {
      for (int index2 = 0; index2 < 4; ++index2)
        numArray[index1, index2] = this.State[index1, index2];
    }
    for (int index3 = 1; index3 < 4; ++index3)
    {
      for (int index4 = 0; index4 < 4; ++index4)
        this.State[index3, index4] = numArray[index3, (index4 + index3) % this.Nb];
    }
  }

  private void InvShiftRows()
  {
    byte[,] numArray = new byte[4, 4];
    for (int index1 = 0; index1 < 4; ++index1)
    {
      for (int index2 = 0; index2 < 4; ++index2)
        numArray[index1, index2] = this.State[index1, index2];
    }
    for (int index3 = 1; index3 < 4; ++index3)
    {
      for (int index4 = 0; index4 < 4; ++index4)
        this.State[index3, (index4 + index3) % this.Nb] = numArray[index3, index4];
    }
  }

  private void MixColumns()
  {
    byte[,] numArray = new byte[4, 4];
    for (int index1 = 0; index1 < 4; ++index1)
    {
      for (int index2 = 0; index2 < 4; ++index2)
        numArray[index1, index2] = this.State[index1, index2];
    }
    for (int index = 0; index < 4; ++index)
    {
      this.State[0, index] = (byte) ((uint) Aes.gfmultby02(numArray[0, index]) ^ (uint) Aes.gfmultby03(numArray[1, index]) ^ (uint) Aes.gfmultby01(numArray[2, index]) ^ (uint) Aes.gfmultby01(numArray[3, index]));
      this.State[1, index] = (byte) ((uint) Aes.gfmultby01(numArray[0, index]) ^ (uint) Aes.gfmultby02(numArray[1, index]) ^ (uint) Aes.gfmultby03(numArray[2, index]) ^ (uint) Aes.gfmultby01(numArray[3, index]));
      this.State[2, index] = (byte) ((uint) Aes.gfmultby01(numArray[0, index]) ^ (uint) Aes.gfmultby01(numArray[1, index]) ^ (uint) Aes.gfmultby02(numArray[2, index]) ^ (uint) Aes.gfmultby03(numArray[3, index]));
      this.State[3, index] = (byte) ((uint) Aes.gfmultby03(numArray[0, index]) ^ (uint) Aes.gfmultby01(numArray[1, index]) ^ (uint) Aes.gfmultby01(numArray[2, index]) ^ (uint) Aes.gfmultby02(numArray[3, index]));
    }
  }

  private void InvMixColumns()
  {
    byte[,] numArray = new byte[4, 4];
    for (int index1 = 0; index1 < 4; ++index1)
    {
      for (int index2 = 0; index2 < 4; ++index2)
        numArray[index1, index2] = this.State[index1, index2];
    }
    for (int index = 0; index < 4; ++index)
    {
      this.State[0, index] = (byte) ((uint) Aes.gfmultby0e(numArray[0, index]) ^ (uint) Aes.gfmultby0b(numArray[1, index]) ^ (uint) Aes.gfmultby0d(numArray[2, index]) ^ (uint) Aes.gfmultby09(numArray[3, index]));
      this.State[1, index] = (byte) ((uint) Aes.gfmultby09(numArray[0, index]) ^ (uint) Aes.gfmultby0e(numArray[1, index]) ^ (uint) Aes.gfmultby0b(numArray[2, index]) ^ (uint) Aes.gfmultby0d(numArray[3, index]));
      this.State[2, index] = (byte) ((uint) Aes.gfmultby0d(numArray[0, index]) ^ (uint) Aes.gfmultby09(numArray[1, index]) ^ (uint) Aes.gfmultby0e(numArray[2, index]) ^ (uint) Aes.gfmultby0b(numArray[3, index]));
      this.State[3, index] = (byte) ((uint) Aes.gfmultby0b(numArray[0, index]) ^ (uint) Aes.gfmultby0d(numArray[1, index]) ^ (uint) Aes.gfmultby09(numArray[2, index]) ^ (uint) Aes.gfmultby0e(numArray[3, index]));
    }
  }

  private static byte gfmultby01(byte b) => b;

  private static byte gfmultby02(byte b)
  {
    return b < (byte) 128 /*0x80*/ ? (byte) ((uint) b << 1) : (byte) ((int) b << 1 ^ 27);
  }

  private static byte gfmultby03(byte b) => (byte) ((uint) Aes.gfmultby02(b) ^ (uint) b);

  private static byte gfmultby09(byte b)
  {
    return (byte) ((uint) Aes.gfmultby02(Aes.gfmultby02(Aes.gfmultby02(b))) ^ (uint) b);
  }

  private static byte gfmultby0b(byte b)
  {
    return (byte) ((uint) Aes.gfmultby02(Aes.gfmultby02(Aes.gfmultby02(b))) ^ (uint) Aes.gfmultby02(b) ^ (uint) b);
  }

  private static byte gfmultby0d(byte b)
  {
    return (byte) ((uint) Aes.gfmultby02(Aes.gfmultby02(Aes.gfmultby02(b))) ^ (uint) Aes.gfmultby02(Aes.gfmultby02(b)) ^ (uint) b);
  }

  private static byte gfmultby0e(byte b)
  {
    return (byte) ((uint) Aes.gfmultby02(Aes.gfmultby02(Aes.gfmultby02(b))) ^ (uint) Aes.gfmultby02(Aes.gfmultby02(b)) ^ (uint) Aes.gfmultby02(b));
  }

  private void KeyExpansion()
  {
    this.w = new byte[this.Nb * (this.Nr + 1), 4];
    for (int index = 0; index < this.Nk; ++index)
    {
      this.w[index, 0] = this.key[4 * index];
      this.w[index, 1] = this.key[4 * index + 1];
      this.w[index, 2] = this.key[4 * index + 2];
      this.w[index, 3] = this.key[4 * index + 3];
    }
    byte[] word = new byte[4];
    for (int nk = this.Nk; nk < this.Nb * (this.Nr + 1); ++nk)
    {
      word[0] = this.w[nk - 1, 0];
      word[1] = this.w[nk - 1, 1];
      word[2] = this.w[nk - 1, 2];
      word[3] = this.w[nk - 1, 3];
      if (nk % this.Nk == 0)
      {
        word = this.SubWord(this.RotWord(word));
        word[0] = (byte) ((uint) word[0] ^ (uint) this.Rcon[nk / this.Nk, 0]);
        word[1] = (byte) ((uint) word[1] ^ (uint) this.Rcon[nk / this.Nk, 1]);
        word[2] = (byte) ((uint) word[2] ^ (uint) this.Rcon[nk / this.Nk, 2]);
        word[3] = (byte) ((uint) word[3] ^ (uint) this.Rcon[nk / this.Nk, 3]);
      }
      else if (this.Nk > 6 && nk % this.Nk == 4)
        word = this.SubWord(word);
      this.w[nk, 0] = (byte) ((uint) this.w[nk - this.Nk, 0] ^ (uint) word[0]);
      this.w[nk, 1] = (byte) ((uint) this.w[nk - this.Nk, 1] ^ (uint) word[1]);
      this.w[nk, 2] = (byte) ((uint) this.w[nk - this.Nk, 2] ^ (uint) word[2]);
      this.w[nk, 3] = (byte) ((uint) this.w[nk - this.Nk, 3] ^ (uint) word[3]);
    }
  }

  private byte[] SubWord(byte[] word)
  {
    return new byte[4]
    {
      this.Sbox[(int) word[0] >> 4, (int) word[0] & 15],
      this.Sbox[(int) word[1] >> 4, (int) word[1] & 15],
      this.Sbox[(int) word[2] >> 4, (int) word[2] & 15],
      this.Sbox[(int) word[3] >> 4, (int) word[3] & 15]
    };
  }

  private byte[] RotWord(byte[] word)
  {
    return new byte[4]{ word[1], word[2], word[3], word[0] };
  }

  public void Dump()
  {
  }

  public string DumpKey()
  {
    string str = "";
    for (int index = 0; index < this.key.Length; ++index)
      str = $"{str}{this.key[index].ToString("x2")} ";
    return str;
  }

  public string DumpTwoByTwo(byte[,] a)
  {
    string str1 = "";
    for (int index1 = 0; index1 < a.GetLength(0); ++index1)
    {
      string str2 = $"{str1}[{(object) index1}] ";
      for (int index2 = 0; index2 < a.GetLength(1); ++index2)
        str2 = $"{str2}{a[index1, index2].ToString("x2")} ";
      str1 = str2 + "\n";
    }
    return str1;
  }

  public enum KeySize
  {
    Bits128,
    Bits192,
    Bits256,
  }
}
