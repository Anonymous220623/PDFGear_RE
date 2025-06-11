// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.SystemFontRD
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.Pdf.Parsing;

internal class SystemFontRD : SystemFontPostScriptOperator
{
  public static bool IsRDOperator(string name) => name == "RD" || name == "-|";

  public override void Execute(SystemFontInterpreter interpreter)
  {
    int num = (int) interpreter.Reader.Read();
    int lastAsInt = interpreter.Operands.GetLastAsInt();
    SystemFontCharStringEncryption encrypt = new SystemFontCharStringEncryption(interpreter.Reader);
    interpreter.Reader.PushEncryption((SystemFontEncryptionBase) encrypt);
    encrypt.Initialize();
    interpreter.Operands.AddLast((object) (lastAsInt - encrypt.RandomBytesCount));
    interpreter.ExecuteProcedure(interpreter.RD);
    interpreter.Reader.PopEncryption();
  }
}
