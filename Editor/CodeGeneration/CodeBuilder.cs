using System;
using System.Collections.Generic;
using System.Text;

namespace RoRSkinBuilder
{
    public class CodeBuilder
    {
        private StringBuilder sb = new StringBuilder();
        private int indent = 0;

        public CodeBuilder A(string value)
        {
            sb.Append(value);
            return this; 
        }

        public CodeBuilder A(int value)
        {
            sb.Append(value);
            return this; 
        }

        public CodeBuilder L()
        {
            sb.AppendLine();
            if (indent > 0)
            {
                A(' ', indent);
            }
            return this;
        }

        public CodeBuilder L(string value) => L().A(value);

        public CodeBuilder A(char value, int count)
        {
            sb.Append(value, count);
            return this; 
        }

        public CodeBuilder A(char value)
        {
            sb.Append(value);
            return this; 
        }

        public CodeBuilder Literal(string value) => A('"').A(value.Escape()).A('"');
        public CodeBuilder Literal(bool value) => A(value ? "true" : "false");

        public CodeBuilder O(string value = "{")
        {
            if (!string.IsNullOrEmpty(value))
            {
                L().A("{");
            }
            indent += 4;

            return this;
        }

        public CodeBuilder C(string value = "}")
        {
            indent -= 4;
            if (!string.IsNullOrEmpty(value))
            {
                L().A("}");
            }

            return this;
        }

        public override string ToString()
        {
            return sb.ToString();
        }

        public string Build()
        {
            var result = sb.ToString();
            sb.Clear();
            return result;
        }
    }
}
