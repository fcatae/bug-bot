using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugBot
{
    public class CommandLine
    {
        string _cmd;
        List<string> _arguments = new List<string>();
        Dictionary<string, string> _namedArgs = new Dictionary<string, string>();

        public CommandLine(string line)
        {
            string[] src = line.Split(new char[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            this._cmd = (src.Length > 0) ? src[0] : "";

            for(int pos=1; pos<src.Length;)
            {
                string param = src[pos++];

                // Parameter
                if (param.StartsWith("-"))
                {
                    // Multiple Short Parameter
                    if ((!param.StartsWith("--")) && (param.Length > 2))
                    {
                        char[] keys = param.ToCharArray(1, param.Length-1);

                        foreach(char k in keys)
                        {
                            string key = k.ToString();
                            string val = "";

                            _namedArgs.Add(key, val);
                        }
                    }
                    else 
                    {
                        // Named Parameter

                        string key = (param.StartsWith("--")) ? param.Substring(2) : param.Substring(1);
                        string val = (pos < src.Length) ? src[pos++] : "";

                        if (val.StartsWith("-"))
                        {
                            val = "";
                            pos--;
                        }

                        _namedArgs.Add(key, val);                        
                    }
                }
                else // Argument
                {
                    _arguments.Add(param);
                }
            }
        }
        
        public string Command
        {
            get
            {
                return _cmd;
            }
        }

        public string GetValue(int position)
        {
            if (position < 0 || position >= _arguments.Count)
            {
                throw new IndexOutOfRangeException();
            }

            return _arguments[position];
        }

        public string GetValue(string parameterName)
        {
            string value = null;

            _namedArgs.TryGetValue(parameterName, out value);

            return value;
        }

    }
}
