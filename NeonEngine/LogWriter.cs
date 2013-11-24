using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public class LogWriter : StreamWriter
    {
        TextWriter console;

        public LogWriter(string path, bool append, Encoding encoding, int bufferSize, TextWriter console)
            : base(path, append, encoding, bufferSize)
        {
            this.console = console;
            base.AutoFlush = true;

        }
        public override void Write(string value)
        {
            console.Write(value);
            base.Write(value);
        }

        public override void WriteLine(string value)
        {
            console.WriteLine(value);
            base.WriteLine(value);
        }

        public new void Dispose()
        {
            base.Dispose();
        }
    }
}
