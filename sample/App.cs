using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Whitaker
{
    public partial class App
    {
        static Dict _Dict;

        class Dict : Grammar
        {
            public Dict()
            {
            }

            bool _Loaded;

            public void Load()
            {
                if (this._Loaded)
                {
                    return;
                }

                DictionaryPath = "..\\..\\..\\";

                LOAD();

                this._Loaded = true;
            }

            public StringBuilder GetHTML(string q, bool search)
            {
                if (q != null)
                {
                    q = q.Trim();
                }
                char letter = 'a';
                StringBuilder HTML = new StringBuilder();
                Dictionary<Word, Node> keyValueList = SearchWords(q);
                if (keyValueList != null)
                {
                    foreach (KeyValuePair<Word, Node> i in keyValueList)
                    {
                        string arg_8A_1 = "({0}.) {1}";
                        char expr_6C = letter;
                        letter++;
                        HTML.AppendFormat(arg_8A_1, expr_6C, Formatter.GetDeclaration(i.Key, i.Value, null));
                        HTML.Append("<p>");
                        HTML.Append(Formatter.GetDefinition(i.Key));
                        HTML.Append("</p>");
                        Node node = i.Value;
                        bool added = false;

                        while (node != null)
                        {
                            if (node.Attributes != Attributes.None && Converter.Equals(q, node.Stem, node.Suffix))
                            {
                                HTML.AppendFormat("<span>{0}</span>", Formatter.GetLong(node.Stem, node.Suffix, i.Key.Flags, node.Attributes, false));
                                HTML.Append("<br/>");
                                added = true;
                            }
                            node = node.Next;
                        }

                        if (added)
                        {
                            HTML.Append("<br/>");
                        }
                    }
                }
                return HTML;
            }
        }

        public static void Main(params string[] args)
        {
            string q = "schola";

            if (_Dict == null)
            {
                _Dict = new Dict();
                _Dict.Load();
            }

            var en = _Dict.GetHTML(q, false);

            if (en == null || en.Length == 0)
            {
                if (q.EndsWith("que", StringComparison.OrdinalIgnoreCase))
                {
                    q = q.Substring(0, q.Length - 3);

                    en = _Dict.GetHTML(q, false);
                }
            }

            if (en != null && en.Length > 0)
            {
                Console.WriteLine(en.ToString());
            }

            Console.ReadKey();

        }
    }
}
