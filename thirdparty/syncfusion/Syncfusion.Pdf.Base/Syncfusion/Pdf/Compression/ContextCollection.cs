// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Compression.ContextCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Pdf.Compression;

internal static class ContextCollection
{
  private static List<Context> m_stateTable = new List<Context>();

  internal static List<Context> StateTable
  {
    get => ContextCollection.m_stateTable;
    set => ContextCollection.m_stateTable = value;
  }

  static ContextCollection()
  {
    ContextCollection.m_stateTable.Add(new Context((short) 22017, ContextCollection.F(1), ContextCollection.Switch((int) ContextCollection.F(1))));
    ContextCollection.m_stateTable.Add(new Context((short) 13313, ContextCollection.F(2), ContextCollection.F(6)));
    ContextCollection.m_stateTable.Add(new Context((short) 6145, ContextCollection.F(3), ContextCollection.F(9)));
    ContextCollection.m_stateTable.Add(new Context((short) 2753, ContextCollection.F(4), ContextCollection.F(12)));
    ContextCollection.m_stateTable.Add(new Context((short) 1313, ContextCollection.F(5), ContextCollection.F(29)));
    ContextCollection.m_stateTable.Add(new Context((short) 545, ContextCollection.F(38), ContextCollection.F(33)));
    ContextCollection.m_stateTable.Add(new Context((short) 22017, ContextCollection.F(7), ContextCollection.Switch((int) ContextCollection.F(6))));
    ContextCollection.m_stateTable.Add(new Context((short) 21505, ContextCollection.F(8), ContextCollection.F(14)));
    ContextCollection.m_stateTable.Add(new Context((short) 18433, ContextCollection.F(9), ContextCollection.F(14)));
    ContextCollection.m_stateTable.Add(new Context((short) 14337, ContextCollection.F(10), ContextCollection.F(14)));
    ContextCollection.m_stateTable.Add(new Context((short) 12289, ContextCollection.F(11), ContextCollection.F(17)));
    ContextCollection.m_stateTable.Add(new Context((short) 9217, ContextCollection.F(12), ContextCollection.F(18)));
    ContextCollection.m_stateTable.Add(new Context((short) 7169, ContextCollection.F(13), ContextCollection.F(20)));
    ContextCollection.m_stateTable.Add(new Context((short) 5633, ContextCollection.F(29), ContextCollection.F(21)));
    ContextCollection.m_stateTable.Add(new Context((short) 22017, ContextCollection.F(15), ContextCollection.Switch((int) ContextCollection.F(14))));
    ContextCollection.m_stateTable.Add(new Context((short) 21505, ContextCollection.F(16 /*0x10*/), ContextCollection.F(14)));
    ContextCollection.m_stateTable.Add(new Context((short) 20737, ContextCollection.F(17), ContextCollection.F(15)));
    ContextCollection.m_stateTable.Add(new Context((short) 18433, ContextCollection.F(18), ContextCollection.F(16 /*0x10*/)));
    ContextCollection.m_stateTable.Add(new Context((short) 14337, ContextCollection.F(19), ContextCollection.F(17)));
    ContextCollection.m_stateTable.Add(new Context((short) 13313, ContextCollection.F(20), ContextCollection.F(18)));
    ContextCollection.m_stateTable.Add(new Context((short) 12289, ContextCollection.F(21), ContextCollection.F(19)));
    ContextCollection.m_stateTable.Add(new Context((short) 10241, ContextCollection.F(22), ContextCollection.F(19)));
    ContextCollection.m_stateTable.Add(new Context((short) 9217, ContextCollection.F(23), ContextCollection.F(20)));
    ContextCollection.m_stateTable.Add(new Context((short) 8705, ContextCollection.F(24), ContextCollection.F(21)));
    ContextCollection.m_stateTable.Add(new Context((short) 7169, ContextCollection.F(25), ContextCollection.F(22)));
    ContextCollection.m_stateTable.Add(new Context((short) 6145, ContextCollection.F(26), ContextCollection.F(23)));
    ContextCollection.m_stateTable.Add(new Context((short) 5633, ContextCollection.F(27), ContextCollection.F(24)));
    ContextCollection.m_stateTable.Add(new Context((short) 5121, ContextCollection.F(28), ContextCollection.F(25)));
    ContextCollection.m_stateTable.Add(new Context((short) 4609, ContextCollection.F(29), ContextCollection.F(26)));
    ContextCollection.m_stateTable.Add(new Context((short) 4353, ContextCollection.F(30), ContextCollection.F(27)));
    ContextCollection.m_stateTable.Add(new Context((short) 2753, ContextCollection.F(31 /*0x1F*/), ContextCollection.F(28)));
    ContextCollection.m_stateTable.Add(new Context((short) 2497, ContextCollection.F(32 /*0x20*/), ContextCollection.F(29)));
    ContextCollection.m_stateTable.Add(new Context((short) 2209, ContextCollection.F(33), ContextCollection.F(30)));
    ContextCollection.m_stateTable.Add(new Context((short) 1313, ContextCollection.F(34), ContextCollection.F(31 /*0x1F*/)));
    ContextCollection.m_stateTable.Add(new Context((short) 1089, ContextCollection.F(35), ContextCollection.F(32 /*0x20*/)));
    ContextCollection.m_stateTable.Add(new Context((short) 673, ContextCollection.F(36), ContextCollection.F(33)));
    ContextCollection.m_stateTable.Add(new Context((short) 545, ContextCollection.F(37), ContextCollection.F(34)));
    ContextCollection.m_stateTable.Add(new Context((short) 321, ContextCollection.F(38), ContextCollection.F(35)));
    ContextCollection.m_stateTable.Add(new Context((short) 273, ContextCollection.F(39), ContextCollection.F(36)));
    ContextCollection.m_stateTable.Add(new Context((short) 133, ContextCollection.F(40), ContextCollection.F(37)));
    ContextCollection.m_stateTable.Add(new Context((short) 73, ContextCollection.F(41), ContextCollection.F(38)));
    ContextCollection.m_stateTable.Add(new Context((short) 37, ContextCollection.F(42), ContextCollection.F(39)));
    ContextCollection.m_stateTable.Add(new Context((short) 21, ContextCollection.F(43), ContextCollection.F(40)));
    ContextCollection.m_stateTable.Add(new Context((short) 9, ContextCollection.F(44), ContextCollection.F(41)));
    ContextCollection.m_stateTable.Add(new Context((short) 5, ContextCollection.F(45), ContextCollection.F(42)));
    ContextCollection.m_stateTable.Add(new Context((short) 1, ContextCollection.F(45), ContextCollection.F(43)));
    ContextCollection.m_stateTable.Add(new Context((short) 22017, ContextCollection.F(47), ContextCollection.Switch((int) ContextCollection.F(47))));
    ContextCollection.m_stateTable.Add(new Context((short) 13313, ContextCollection.F(48 /*0x30*/), ContextCollection.F(52)));
    ContextCollection.m_stateTable.Add(new Context((short) 6145, ContextCollection.F(49), ContextCollection.F(55)));
    ContextCollection.m_stateTable.Add(new Context((short) 2753, ContextCollection.F(50), ContextCollection.F(58)));
    ContextCollection.m_stateTable.Add(new Context((short) 1313, ContextCollection.F(51), ContextCollection.F(75)));
    ContextCollection.m_stateTable.Add(new Context((short) 545, ContextCollection.F(84), ContextCollection.F(79)));
    ContextCollection.m_stateTable.Add(new Context((short) 22017, ContextCollection.F(53), ContextCollection.Switch((int) ContextCollection.F(52))));
    ContextCollection.m_stateTable.Add(new Context((short) 21505, ContextCollection.F(54), ContextCollection.F(60)));
    ContextCollection.m_stateTable.Add(new Context((short) 18433, ContextCollection.F(55), ContextCollection.F(60)));
    ContextCollection.m_stateTable.Add(new Context((short) 14337, ContextCollection.F(56), ContextCollection.F(60)));
    ContextCollection.m_stateTable.Add(new Context((short) 12289, ContextCollection.F(57), ContextCollection.F(63 /*0x3F*/)));
    ContextCollection.m_stateTable.Add(new Context((short) 9217, ContextCollection.F(58), ContextCollection.F(64 /*0x40*/)));
    ContextCollection.m_stateTable.Add(new Context((short) 7169, ContextCollection.F(59), ContextCollection.F(66)));
    ContextCollection.m_stateTable.Add(new Context((short) 5633, ContextCollection.F(75), ContextCollection.F(67)));
    ContextCollection.m_stateTable.Add(new Context((short) 22017, ContextCollection.F(61), ContextCollection.Switch((int) ContextCollection.F(60))));
    ContextCollection.m_stateTable.Add(new Context((short) 21505, ContextCollection.F(62), ContextCollection.F(60)));
    ContextCollection.m_stateTable.Add(new Context((short) 20737, ContextCollection.F(63 /*0x3F*/), ContextCollection.F(61)));
    ContextCollection.m_stateTable.Add(new Context((short) 18433, ContextCollection.F(64 /*0x40*/), ContextCollection.F(62)));
    ContextCollection.m_stateTable.Add(new Context((short) 14337, ContextCollection.F(65), ContextCollection.F(63 /*0x3F*/)));
    ContextCollection.m_stateTable.Add(new Context((short) 13313, ContextCollection.F(66), ContextCollection.F(64 /*0x40*/)));
    ContextCollection.m_stateTable.Add(new Context((short) 12289, ContextCollection.F(67), ContextCollection.F(65)));
    ContextCollection.m_stateTable.Add(new Context((short) 10241, ContextCollection.F(68), ContextCollection.F(65)));
    ContextCollection.m_stateTable.Add(new Context((short) 9217, ContextCollection.F(69), ContextCollection.F(66)));
    ContextCollection.m_stateTable.Add(new Context((short) 8705, ContextCollection.F(70), ContextCollection.F(67)));
    ContextCollection.m_stateTable.Add(new Context((short) 7169, ContextCollection.F(71), ContextCollection.F(68)));
    ContextCollection.m_stateTable.Add(new Context((short) 6145, ContextCollection.F(72), ContextCollection.F(69)));
    ContextCollection.m_stateTable.Add(new Context((short) 5633, ContextCollection.F(73), ContextCollection.F(70)));
    ContextCollection.m_stateTable.Add(new Context((short) 5121, ContextCollection.F(74), ContextCollection.F(71)));
    ContextCollection.m_stateTable.Add(new Context((short) 4609, ContextCollection.F(75), ContextCollection.F(72)));
    ContextCollection.m_stateTable.Add(new Context((short) 4353, ContextCollection.F(76), ContextCollection.F(73)));
    ContextCollection.m_stateTable.Add(new Context((short) 2753, ContextCollection.F(77), ContextCollection.F(74)));
    ContextCollection.m_stateTable.Add(new Context((short) 2497, ContextCollection.F(78), ContextCollection.F(75)));
    ContextCollection.m_stateTable.Add(new Context((short) 2209, ContextCollection.F(79), ContextCollection.F(76)));
    ContextCollection.m_stateTable.Add(new Context((short) 1313, ContextCollection.F(80 /*0x50*/), ContextCollection.F(77)));
    ContextCollection.m_stateTable.Add(new Context((short) 1089, ContextCollection.F(81), ContextCollection.F(78)));
    ContextCollection.m_stateTable.Add(new Context((short) 673, ContextCollection.F(82), ContextCollection.F(79)));
    ContextCollection.m_stateTable.Add(new Context((short) 545, ContextCollection.F(83), ContextCollection.F(80 /*0x50*/)));
    ContextCollection.m_stateTable.Add(new Context((short) 321, ContextCollection.F(84), ContextCollection.F(81)));
    ContextCollection.m_stateTable.Add(new Context((short) 273, ContextCollection.F(85), ContextCollection.F(82)));
    ContextCollection.m_stateTable.Add(new Context((short) 133, ContextCollection.F(86), ContextCollection.F(83)));
    ContextCollection.m_stateTable.Add(new Context((short) 73, ContextCollection.F(87), ContextCollection.F(84)));
    ContextCollection.m_stateTable.Add(new Context((short) 37, ContextCollection.F(88), ContextCollection.F(85)));
    ContextCollection.m_stateTable.Add(new Context((short) 21, ContextCollection.F(89), ContextCollection.F(86)));
    ContextCollection.m_stateTable.Add(new Context((short) 9, ContextCollection.F(90), ContextCollection.F(87)));
    ContextCollection.m_stateTable.Add(new Context((short) 5, ContextCollection.F(91), ContextCollection.F(88)));
    ContextCollection.m_stateTable.Add(new Context((short) 1, ContextCollection.F(91), ContextCollection.F(89)));
  }

  private static char F(int x) => (char) x;

  private static char Switch(int x) => x <= 46 ? (char) (x + 46) : (char) (x - 46);
}
