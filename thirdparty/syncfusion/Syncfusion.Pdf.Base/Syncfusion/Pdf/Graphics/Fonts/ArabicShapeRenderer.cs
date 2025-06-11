// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Fonts.ArabicShapeRenderer
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System.Collections.Generic;
using System.Text;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Fonts;

internal class ArabicShapeRenderer
{
  private const char Alef = 'ا';
  private const char AlefHamza = 'أ';
  private const char AlefHamzaBelow = 'إ';
  private const char AlefMadda = 'آ';
  private const char Lam = 'ل';
  private const char Hamza = 'ء';
  private const char ZeroWidthJoiner = '\u200D';
  private const char HamzaAbove = 'ٔ';
  private const char HamzaBelow = 'ٕ';
  private const char WawHamza = 'ؤ';
  private const char YehHamza = 'ئ';
  private const char Waw = 'و';
  private const char AlefMaksura = 'ى';
  private const char Yeh = 'ي';
  private const char FarsiYeh = 'ی';
  private const char Shadda = 'ّ';
  private const char Madda = 'ٓ';
  private const char Lwa = 'ﻻ';
  private const char Lwawh = 'ﻷ';
  private const char Lwawhb = 'ﻹ';
  private const char Lwawm = 'ﻵ';
  private const char Bwhb = 'ۓ';
  private const char Fathatan = 'ً';
  private const char SuperScriptAlef = 'ٰ';
  private const int Vowel = 1;
  private readonly char[][] ArabicCharTable = new char[76][]
  {
    new char[2]{ 'ء', 'ﺀ' },
    new char[3]{ 'آ', 'ﺁ', 'ﺂ' },
    new char[3]{ 'أ', 'ﺃ', 'ﺄ' },
    new char[3]{ 'ؤ', 'ﺅ', 'ﺆ' },
    new char[3]{ 'إ', 'ﺇ', 'ﺈ' },
    new char[5]{ 'ئ', 'ﺉ', 'ﺊ', 'ﺋ', 'ﺌ' },
    new char[3]{ 'ا', 'ﺍ', 'ﺎ' },
    new char[5]{ 'ب', 'ﺏ', 'ﺐ', 'ﺑ', 'ﺒ' },
    new char[3]{ 'ة', 'ﺓ', 'ﺔ' },
    new char[5]{ 'ت', 'ﺕ', 'ﺖ', 'ﺗ', 'ﺘ' },
    new char[5]{ 'ث', 'ﺙ', 'ﺚ', 'ﺛ', 'ﺜ' },
    new char[5]{ 'ج', 'ﺝ', 'ﺞ', 'ﺟ', 'ﺠ' },
    new char[5]{ 'ح', 'ﺡ', 'ﺢ', 'ﺣ', 'ﺤ' },
    new char[5]{ 'خ', 'ﺥ', 'ﺦ', 'ﺧ', 'ﺨ' },
    new char[3]{ 'د', 'ﺩ', 'ﺪ' },
    new char[3]{ 'ذ', 'ﺫ', 'ﺬ' },
    new char[3]{ 'ر', 'ﺭ', 'ﺮ' },
    new char[3]{ 'ز', 'ﺯ', 'ﺰ' },
    new char[5]{ 'س', 'ﺱ', 'ﺲ', 'ﺳ', 'ﺴ' },
    new char[5]{ 'ش', 'ﺵ', 'ﺶ', 'ﺷ', 'ﺸ' },
    new char[5]{ 'ص', 'ﺹ', 'ﺺ', 'ﺻ', 'ﺼ' },
    new char[5]{ 'ض', 'ﺽ', 'ﺾ', 'ﺿ', 'ﻀ' },
    new char[5]{ 'ط', 'ﻁ', 'ﻂ', 'ﻃ', 'ﻄ' },
    new char[5]{ 'ظ', 'ﻅ', 'ﻆ', 'ﻇ', 'ﻈ' },
    new char[5]{ 'ع', 'ﻉ', 'ﻊ', 'ﻋ', 'ﻌ' },
    new char[5]{ 'غ', 'ﻍ', 'ﻎ', 'ﻏ', 'ﻐ' },
    new char[5]{ 'ـ', 'ـ', 'ـ', 'ـ', 'ـ' },
    new char[5]{ 'ف', 'ﻑ', 'ﻒ', 'ﻓ', 'ﻔ' },
    new char[5]{ 'ق', 'ﻕ', 'ﻖ', 'ﻗ', 'ﻘ' },
    new char[5]{ 'ك', 'ﻙ', 'ﻚ', 'ﻛ', 'ﻜ' },
    new char[5]{ 'ل', 'ﻝ', 'ﻞ', 'ﻟ', 'ﻠ' },
    new char[5]{ 'م', 'ﻡ', 'ﻢ', 'ﻣ', 'ﻤ' },
    new char[5]{ 'ن', 'ﻥ', 'ﻦ', 'ﻧ', 'ﻨ' },
    new char[5]{ 'ه', 'ﻩ', 'ﻪ', 'ﻫ', 'ﻬ' },
    new char[3]{ 'و', 'ﻭ', 'ﻮ' },
    new char[5]{ 'ى', 'ﻯ', 'ﻰ', 'ﯨ', 'ﯩ' },
    new char[5]{ 'ي', 'ﻱ', 'ﻲ', 'ﻳ', 'ﻴ' },
    new char[3]{ 'ٱ', 'ﭐ', 'ﭑ' },
    new char[5]{ 'ٹ', 'ﭦ', 'ﭧ', 'ﭨ', 'ﭩ' },
    new char[5]{ 'ٺ', 'ﭞ', 'ﭟ', 'ﭠ', 'ﭡ' },
    new char[5]{ 'ٻ', 'ﭒ', 'ﭓ', 'ﭔ', 'ﭕ' },
    new char[5]{ 'پ', 'ﭖ', 'ﭗ', 'ﭘ', 'ﭙ' },
    new char[5]{ 'ٿ', 'ﭢ', 'ﭣ', 'ﭤ', 'ﭥ' },
    new char[5]{ 'ڀ', 'ﭚ', 'ﭛ', 'ﭜ', 'ﭝ' },
    new char[5]{ 'ڃ', 'ﭶ', 'ﭷ', 'ﭸ', 'ﭹ' },
    new char[5]{ 'ڄ', 'ﭲ', 'ﭳ', 'ﭴ', 'ﭵ' },
    new char[5]{ 'چ', 'ﭺ', 'ﭻ', 'ﭼ', 'ﭽ' },
    new char[5]{ 'ڇ', 'ﭾ', 'ﭿ', 'ﮀ', 'ﮁ' },
    new char[3]{ 'ڈ', 'ﮈ', 'ﮉ' },
    new char[3]{ 'ڌ', 'ﮄ', 'ﮅ' },
    new char[3]{ 'ڍ', 'ﮂ', 'ﮃ' },
    new char[3]{ 'ڎ', 'ﮆ', 'ﮇ' },
    new char[3]{ 'ڑ', 'ﮌ', 'ﮍ' },
    new char[3]{ 'ژ', 'ﮊ', 'ﮋ' },
    new char[5]{ 'ڤ', 'ﭪ', 'ﭫ', 'ﭬ', 'ﭭ' },
    new char[5]{ 'ڦ', 'ﭮ', 'ﭯ', 'ﭰ', 'ﭱ' },
    new char[5]{ 'ک', 'ﮎ', 'ﮏ', 'ﮐ', 'ﮑ' },
    new char[5]{ 'ڭ', 'ﯓ', 'ﯔ', 'ﯕ', 'ﯖ' },
    new char[5]{ 'گ', 'ﮒ', 'ﮓ', 'ﮔ', 'ﮕ' },
    new char[5]{ 'ڱ', 'ﮚ', 'ﮛ', 'ﮜ', 'ﮝ' },
    new char[5]{ 'ڳ', 'ﮖ', 'ﮗ', 'ﮘ', 'ﮙ' },
    new char[3]{ 'ں', 'ﮞ', 'ﮟ' },
    new char[5]{ 'ڻ', 'ﮠ', 'ﮡ', 'ﮢ', 'ﮣ' },
    new char[5]{ 'ھ', 'ﮪ', 'ﮫ', 'ﮬ', 'ﮭ' },
    new char[3]{ 'ۀ', 'ﮤ', 'ﮥ' },
    new char[5]{ 'ہ', 'ﮦ', 'ﮧ', 'ﮨ', 'ﮩ' },
    new char[3]{ 'ۅ', 'ﯠ', 'ﯡ' },
    new char[3]{ 'ۆ', 'ﯙ', 'ﯚ' },
    new char[3]{ 'ۇ', 'ﯗ', 'ﯘ' },
    new char[3]{ 'ۈ', 'ﯛ', 'ﯜ' },
    new char[3]{ 'ۉ', 'ﯢ', 'ﯣ' },
    new char[3]{ 'ۋ', 'ﯞ', 'ﯟ' },
    new char[5]{ 'ی', 'ﯼ', 'ﯽ', 'ﯾ', 'ﯿ' },
    new char[5]{ 'ې', 'ﯤ', 'ﯥ', 'ﯦ', 'ﯧ' },
    new char[3]{ 'ے', 'ﮮ', 'ﮯ' },
    new char[3]{ 'ۓ', 'ﮰ', 'ﮱ' }
  };
  private Dictionary<char, char[]> m_arabicMapTable = new Dictionary<char, char[]>();

  internal ArabicShapeRenderer()
  {
    for (int index = 0; index < this.ArabicCharTable.Length; ++index)
      this.m_arabicMapTable[this.ArabicCharTable[index][0]] = this.ArabicCharTable[index];
  }

  private char GetCharacterShape(char input, int index)
  {
    if (input >= 'ء' && input <= 'ۓ')
    {
      char[] chArray;
      if (this.m_arabicMapTable.TryGetValue(input, out chArray))
        return chArray[index + 1];
    }
    else if (input >= 'ﻵ' && input <= 'ﻻ')
      return (char) ((uint) input + (uint) index);
    return input;
  }

  internal string Shape(char[] text, int level)
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    StringBuilder stringBuilder2 = new StringBuilder();
    for (int index = 0; index < text.Length; ++index)
    {
      char ch = text[index];
      if (ch >= '\u0600' && ch <= 'ۿ')
      {
        stringBuilder2.Append(ch);
      }
      else
      {
        if (stringBuilder2.Length > 0)
        {
          string str = this.DoShape(stringBuilder2.ToString().ToCharArray(), 0);
          stringBuilder1.Append(str);
          stringBuilder2 = new StringBuilder();
        }
        stringBuilder1.Append(ch);
      }
    }
    if (stringBuilder2.Length > 0)
    {
      string str = this.DoShape(stringBuilder2.ToString().ToCharArray(), 0);
      stringBuilder1.Append(str);
    }
    return stringBuilder1.ToString();
  }

  private string DoShape(char[] input, int level)
  {
    StringBuilder builder = new StringBuilder();
    int num1 = 0;
    ArabicShapeRenderer.ArabicShape shape1 = new ArabicShapeRenderer.ArabicShape();
    ArabicShapeRenderer.ArabicShape shape2 = new ArabicShapeRenderer.ArabicShape();
    while (num1 < input.Length)
    {
      char shape3 = input[num1++];
      if (this.Ligature(shape3, shape2) == 0)
      {
        int shapeCount = this.GetShapeCount(shape3);
        int num2 = shapeCount == 1 ? 0 : 2;
        if (shape1.Shapes > 2)
          ++num2;
        int index = num2 % shape2.Shapes;
        shape2.Value = this.GetCharacterShape(shape2.Value, index);
        this.Append(builder, shape1, level);
        shape1 = shape2;
        shape2 = new ArabicShapeRenderer.ArabicShape();
        shape2.Value = shape3;
        shape2.Shapes = shapeCount;
        ++shape2.Ligature;
      }
    }
    int index1 = (shape1.Shapes > 2 ? 1 : 0) % shape2.Shapes;
    shape2.Value = this.GetCharacterShape(shape2.Value, index1);
    this.Append(builder, shape1, level);
    this.Append(builder, shape2, level);
    return builder.ToString();
  }

  private void Append(StringBuilder builder, ArabicShapeRenderer.ArabicShape shape, int level)
  {
    if (shape.Value == char.MinValue)
      return;
    builder.Append(shape.Value);
    --shape.Ligature;
    if (shape.Type != char.MinValue)
    {
      if ((level & 1) == 0)
      {
        builder.Append(shape.Type);
        --shape.Ligature;
      }
      else
        --shape.Ligature;
    }
    if (shape.Vowel == char.MinValue)
      return;
    if ((level & 1) == 0)
    {
      builder.Append(shape.Vowel);
      --shape.Ligature;
    }
    else
      --shape.Ligature;
  }

  private int Ligature(char value, ArabicShapeRenderer.ArabicShape shape)
  {
    if (shape.Value == char.MinValue)
      return 0;
    int num1 = 0;
    if (value >= 'ً' && value <= 'ٕ' || value == 'ٰ')
    {
      int num2 = 1;
      if (shape.Vowel != char.MinValue && value != 'ّ')
        num2 = 2;
      switch (value)
      {
        case 'ّ':
          if (shape.Type != char.MinValue)
            return 0;
          shape.Type = 'ّ';
          break;
        case 'ٓ':
          if (shape.Value == 'ا')
          {
            shape.Value = 'آ';
            num2 = 2;
            break;
          }
          break;
        case 'ٔ':
          if (shape.Value == 'ا')
          {
            shape.Value = 'أ';
            num2 = 2;
            break;
          }
          if (shape.Value == 'ﻻ')
          {
            shape.Value = 'ﻷ';
            num2 = 2;
            break;
          }
          if (shape.Value == 'و')
          {
            shape.Value = 'ؤ';
            num2 = 2;
            break;
          }
          if (shape.Value == 'ي' || shape.Value == 'ى' || shape.Value == 'ی')
          {
            shape.Value = 'ئ';
            num2 = 2;
            break;
          }
          shape.Type = 'ٔ';
          break;
        case 'ٕ':
          if (shape.Value == 'ا')
          {
            shape.Value = 'إ';
            num2 = 2;
            break;
          }
          if (value == 'ﻻ')
          {
            shape.Value = 'ﻹ';
            num2 = 2;
            break;
          }
          shape.Type = 'ٕ';
          break;
        default:
          shape.Vowel = value;
          break;
      }
      if (num2 == 1)
        ++shape.Ligature;
      return num2;
    }
    if (shape.Vowel != char.MinValue)
      return 0;
    if (shape.Value == 'ل')
    {
      switch (value)
      {
        case 'آ':
          shape.Value = 'ﻵ';
          shape.Shapes = 2;
          num1 = 3;
          break;
        case 'أ':
          shape.Value = 'ﻷ';
          shape.Shapes = 2;
          num1 = 3;
          break;
        case 'إ':
          shape.Value = 'ﻹ';
          shape.Shapes = 2;
          num1 = 3;
          break;
        case 'ا':
          shape.Value = 'ﻻ';
          shape.Shapes = 2;
          num1 = 3;
          break;
      }
    }
    else if (shape.Value == char.MinValue)
    {
      shape.Value = value;
      shape.Shapes = this.GetShapeCount(value);
      num1 = 1;
    }
    return num1;
  }

  private int GetShapeCount(char shape)
  {
    if (shape >= 'ء' && shape <= 'ۓ' && (shape < 'ً' || shape > 'ٕ') && shape != 'ٰ')
    {
      char[] chArray;
      if (this.m_arabicMapTable.TryGetValue(shape, out chArray))
        return chArray.Length - 1;
    }
    else if (shape == '\u200D')
      return 4;
    return 1;
  }

  private class ArabicShape
  {
    private char m_value;
    private char m_type;
    private char m_vowel;
    private int m_ligature;
    private int m_shapes = 1;

    internal char Value
    {
      get => this.m_value;
      set => this.m_value = value;
    }

    internal char Type
    {
      get => this.m_type;
      set => this.m_type = value;
    }

    internal char Vowel
    {
      get => this.m_vowel;
      set => this.m_vowel = value;
    }

    internal int Ligature
    {
      get => this.m_ligature;
      set => this.m_ligature = value;
    }

    internal int Shapes
    {
      get => this.m_shapes;
      set => this.m_shapes = value;
    }
  }
}
