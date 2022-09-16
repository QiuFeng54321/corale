namespace PseudoCode.Core.Formatting;

public class PseudoFormatter : IDisposable
{
    private readonly TextWriter _writer;
    private char _indentChar = ' ';
    private int _indentLevel;
    private int _indentWidth = 4;

    public PseudoFormatter(TextWriter writer)
    {
        _writer = writer;
    }

    public void Dispose()
    {
        _writer?.Dispose();
    }

    public void SetIndentType(char indentChar, int width)
    {
        _indentChar = indentChar;
        _indentWidth = width;
    }

    public void Indent()
    {
        _indentLevel++;
    }

    public void Dedent()
    {
        if (_indentLevel <= 0) throw new InvalidOperationException("Dedent in level 0");
        _indentLevel--;
    }

    public void WriteIndent()
    {
        _writer.Write(new string(_indentChar, _indentLevel * _indentWidth));
    }

    public void WriteStatement(string stmt)
    {
        WriteIndent();
        _writer.WriteLine(stmt);
    }
}