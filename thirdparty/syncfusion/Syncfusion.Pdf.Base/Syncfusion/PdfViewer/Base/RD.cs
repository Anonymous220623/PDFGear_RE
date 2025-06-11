// Decompiled with JetBrains decompiler
// Type: Syncfusion.PdfViewer.Base.RD
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

#nullable disable
namespace Syncfusion.PdfViewer.Base;

internal class RD : PostScriptOperators
{
  public static bool IsRDOperator(string name) => name == nameof (RD) || name == "-|";

  public override void Execute(FontInterpreter interpreter)
  {
    int num = (int) interpreter.Reader.Read();
    int lastAsInt = interpreter.Operands.GetLastAsInt();
    CharStringEncryption encrypt = new CharStringEncryption(interpreter.Reader);
    interpreter.Reader.PushEncryption((EncryptionStdHelper) encrypt);
    encrypt.Initialize();
    interpreter.Operands.AddLast((object) (lastAsInt - encrypt.RandomBytesCount));
    interpreter.ExecuteProcedure(interpreter.RD);
    interpreter.Reader.PopEncryption();
  }
}
