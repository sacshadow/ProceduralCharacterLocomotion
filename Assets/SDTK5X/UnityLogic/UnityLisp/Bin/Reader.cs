using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UnityLisp {
	public class Reader {
		public class ParseError : ULThrowable {
            public ParseError(string msg) : base(msg) { }
        }
		
		public class TokenList {
			public List<string> tokens;
            public int position;
            
			public TokenList(List<string> t) {
                tokens = t;
                position = 0;
				
				// Debug.Log(String.Join(".", t.ToArray()));
            }

            public string Peek() {
                if (position >= tokens.Count) {
                    return null;
                } else {
                    return tokens[position];
                }
            }
            public string Next() {
                return tokens[position++];
            }
		}
		
		public static List<string> Tokenize(string str) {
            List<string> tokens = new List<string>();
            string pattern = @"[\s ,]*(~@|[\[\]{}()'`~@]|""(?:[\\].|[^\\""])*""|;.*|[^\s \[\]{}()'""`~@,;]*)";
            Regex regex = new Regex(pattern);
            foreach (Match match in regex.Matches(str)) {
                string token = match.Groups[1].Value;
                if ((token != null) && !(token == "") && !(token[0] == ';')) {
                    //Console.WriteLine("match: ^" + match.Groups[1] + "$");
                    tokens.Add(token);
                }
            }
            return tokens;
        }
		
		public static LispObject Read_Atom(TokenList tList) {
            string token = tList.Next();
            string pattern = @"(^-?[0-9]+$)|(^-?[0-9][0-9.]*$)|(^nil$)|(^true$)|(^false$)|^("".*"")$|:(.*)|(^[^""]*$)";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(token);
            //Debug.Log("token: ^" + token + "$");
            if (!match.Success) {
                throw new ParseError("unrecognized token '" + token + "'");
            }
            if (match.Groups[1].Value != String.Empty) {
                return new Integer(int.Parse(match.Groups[1].Value));
            } else if (match.Groups[3].Value != String.Empty) {
                return LispObject.nilValue;
            } else if (match.Groups[4].Value != String.Empty) {
                return LispObject.trueValue;
            } else if (match.Groups[5].Value != String.Empty) {
                return LispObject.falseValue;
            } else if (match.Groups[6].Value != String.Empty) {
                string str = match.Groups[6].Value;
                str = str.Substring(1, str.Length-2)
                    .Replace("\\\"", "\"")
                    .Replace("\\n", "\n")
                    .Replace("\\\\", "\\");
                return new ULString(str);
            } else if (match.Groups[7].Value != String.Empty) {
                return new ULString(ULString.kwMark + match.Groups[7].Value);
            } else if (match.Groups[8].Value != String.Empty) {
                return new Symbol(match.Groups[8].Value);
            } else {
                throw new ParseError("unrecognized '" + match.Groups[0] + "'");
            }
        }

        public static LispObject Read_List(TokenList tList, ULList lst, char start, char end) {
            string token = tList.Next();
            if (token[0] != start) {
                throw new ParseError("expected '" + start + "'");
            }
			
            while ((token = tList.Peek()) != null && token[0] != end) {
                lst.AddRange(Read_Form(tList));
            }

            if (token == null) {
                throw new ParseError("expected '" + end + "', got EOF");
            }
            tList.Next();

            return lst;
        }

        public static LispObject Read_Hashmap(TokenList tList) {
            ULList lst = (ULList)Read_List(tList, new ULList(), '{', '}');
            return new HashMap(lst);
        }


        public static LispObject Read_Form(TokenList tList) {
            string token = tList.Peek();
            if (token == null) { throw new ULContinue(); }

            switch (token) {
                case "'": tList.Next();
                    return new ULList(new Symbol("quote"), Read_Form(tList));
                case "`": tList.Next();
                    return new ULList(new Symbol("quasiquote"), Read_Form(tList));
                case "~":
                    tList.Next();
                    return new ULList(new Symbol("unquote"), Read_Form(tList));
                case "~@":
                    tList.Next();
                    return new ULList(new Symbol("splice-unquote"), Read_Form(tList));
                case "^": tList.Next();
                    LispObject meta = Read_Form(tList);
                    return new ULList(new Symbol("with-meta"),Read_Form(tList), meta);
                case "@": tList.Next();
                    return new ULList(new Symbol("deref"), Read_Form(tList));
		
                case "(": return Read_List(tList, new ULList(), '(' , ')');
                case ")": throw new ParseError("unexpected ')'");
                case "[": return Read_List(tList, new ULVector(), '[' , ']');
                case "]": throw new ParseError("unexpected ']'");
                case "{": return Read_Hashmap(tList);
                case "}": throw new ParseError("unexpected '}'");
                default:  return Read_Atom(tList);
            }
        }


        public static LispObject Read_Input(string input) {
            return Read_Form(new TokenList(Tokenize(input)));
        }
		
	}
}
