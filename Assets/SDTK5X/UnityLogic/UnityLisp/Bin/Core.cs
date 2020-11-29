using UnityEngine;
//using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnityLisp {
	public static class Core {
		
		public static Constant Nil = LispObject.nilValue;
		public static Constant True = LispObject.trueValue;
		public static Constant False = LispObject.falseValue;
		
		public static LispObject Throw(ULList arg) {throw new ULException(arg[0]); }
		
		public static LispObject Compare(bool state) {return state?True:False; }
		
		public static LispObject IsNil(ULList arg) {return Compare(arg[0] == Nil);	}
		
		public static LispObject IsTrue(ULList arg) {return Compare(arg[0] == True); }
		
		public static LispObject IsFalse(ULList arg) {return Compare(arg[0] == False); }
		
		public static LispObject IsSymbol(ULList arg) {return Compare(arg[0] is Symbol); }
		
		public static LispObject IsString(ULList arg) {
			if(arg[0] is ULString) return Compare(!B_IsKeyword((ULString)arg[0]));
			return False;
		}
		
		public static LispObject Keyword(ULList arg) {
			var str = (ULString)arg[0];
			if(B_IsKeyword(str))
				return arg[0];
			else
				return new ULString(ULString.kwMark + str.GetValue());
		}
		
		public static string ToStrValue(LispObject lo) {return ((ULString)lo).GetValue(); }
		
		public static bool B_BoolenValue(Constant constant) {return constant == True ? true : false; }
		
		public static bool B_IsKeyword(ULString str) {return str.GetValue()[0] == ULString.kwMark; }
		
		public static LispObject IsKeyword(ULList arg) {
			if(arg[0] is ULString) return Compare(B_IsKeyword((ULString)arg[0]));
			return False;
		}
		
		public static LispObject Time_ms(ULList arg) {return new Integer(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond); }
		
		public static LispObject Print_str(ULList arg) {return new ULString(ULFormat.PrintArgs(arg, " ", true)); }
		
		public static LispObject Str(ULList arg) {return new ULString(ULFormat.PrintArgs(arg, "", false)); }
		
		public static LispObject Print(ULList arg) {Debug.Log(ULFormat.PrintArgs(arg, " ", true)); return Nil; }
		
		public static LispObject PrintLin(ULList arg) {Debug.Log(ULFormat.PrintArgs(arg, " ", false)); return Nil; }
		
		public static LispObject ReadLine(ULList arg) {throw new ULException("not implement, may support later"); }
		
		public static LispObject ReadString(ULList arg) {return Reader.Read_Input(ToStrValue(arg[0])); }
		
		public static LispObject Slurp(ULList arg) {return new ULString(File.ReadAllText(ToStrValue(arg[0]))); }
		
		public static LispObject IsList(ULList arg) {return Compare(arg[0].GetType() == typeof(ULList)); }
		
		public static LispObject IsVector(ULList arg) {return Compare(arg[0].GetType() == typeof(ULVector)); }
		
		public static LispObject IsHashMap(ULList arg) {return Compare(arg[0].GetType() == typeof(HashMap)); }
		
		public static LispObject IsContains(ULList arg) {
			var dict = ((HashMap)arg[0]).GetValue();
			return Compare(dict.ContainsKey(ToStrValue(arg[1])));
		}
		
		public static LispObject Assoc(ULList arg) {
			var new_hm = ((HashMap)arg[0]).Copy();
			return new_hm.AddRange(arg.Slice(1));
		}
		
		public static LispObject Dissoc(ULList arg) {
			var new_hm = ((HashMap)arg[0]).Copy();
			return new_hm.RemoveRange(arg.Slice(1));
		}
		
		public static LispObject Get(ULList arg) {
			if(arg[0] == Nil) return Nil;
			else {
				var key = ToStrValue(arg[1]);
				var dict = ((HashMap)arg[0]).GetValue();
				return dict.ContainsKey(key) ? dict[key] : Nil;
			}
		}
		
		public static LispObject GetKeys(ULList arg) {
			var dict = ((HashMap)arg[0]).GetValue();
			var rt = new ULList();
			foreach(var key in dict.Keys) rt.GetValue().Add(new ULString(key));
			return rt;
		}
		
		public static LispObject GetValues(ULList arg) {
			var dict = ((HashMap)arg[0]).GetValue();
			var rt = new ULList();
			foreach(var val in dict.Values) rt.GetValue().Add(val);
			return rt;
		}
		
		public static LispObject IsSequential(ULList arg) {return Compare(arg[0] is ULList); }
		
		public static LispObject Cons(ULList arg) {
			var list = new List<LispObject>();
			list.Add(arg[0]);
			list.AddRange(((ULList)arg[1]).GetValue());
			return new ULList(list);
		}
		
		public static LispObject Concat(ULList arg) {
			if(arg.GetSize() == 0) return new ULList();
			var list = new List<LispObject>();
			for(int i = 0; i< arg.GetSize(); i++) {
				list.AddRange(((ULList)arg[i]).GetValue());
			}
			return new ULList(list);
		}
		
		public static LispObject Nth(ULList arg) {
			var index = (int)((Integer)arg[1]).GetValue();
			if(index < ((ULList)arg[0]).GetSize())
				return ((ULList)arg[0])[index];
			else
				throw new ULException("nth: index out of range : " + index + " " + ((ULList)arg[0]).GetSize());
		}
		
		public static LispObject First(ULList arg) {return arg[0] == Nil ? (LispObject)Nil : ((ULList)arg[0])[0]; }
		
		public static LispObject Rest(ULList arg) {return arg[0] == Nil ? (LispObject)Nil : ((ULList)arg[0]).Rest(); }
		
		public static LispObject IsEmpty(ULList arg) {return Compare(((ULList)arg[0]).GetSize() == 0); }
		
		public static LispObject Count(ULList arg) {
			return arg[0] == Nil ? new Integer(0) : new Integer((long)((ULList)arg[0]).GetSize());
		}
		
		public static LispObject Conj(ULList arg) {
			var src_list = ((ULList)arg[0]).GetValue();
			var new_list = new List<LispObject>(src_list);
			if(arg[0] is ULVector)
				Loop.Count(arg.GetSize()).Do(x=>new_list.Add(arg[x]));
			else
				Loop.Count(arg.GetSize()).Do(x=>new_list.Insert(0,arg[x]));
			return new ULList(new_list);
		}
		
		public static LispObject Seq(ULList arg) {
			if(arg[0] == Nil) return Nil;
			else if(arg[0] is ULVector)
				return (((ULVector)arg[0]).GetSize() == 0) ? (LispObject)Nil : new ULList(((ULVector)arg[0]).GetValue());
			else if(arg[0] is ULList)
				return (((ULList)arg[0]).GetSize() == 0) ? (LispObject)Nil : arg[0];
			else if (arg[0] is ULString) {
				var str = ((ULString)arg[0]).GetValue();
				if(str.Length == 0) return Nil;
				var char_list = new List<LispObject>();
				Loop.Count(str.Length).Do(x=>char_list.Add(new ULString(str[x].ToString())));
				return new ULList(char_list);
			}
			return Nil;
		}
		
		public static LispObject Apply(ULList arg) {
			var f = (Function) arg[0];
			var list = new List<LispObject>();
			list.AddRange(arg.Slice(1,arg.GetSize()-1).GetValue());
			list.AddRange(((ULList)arg[arg.GetSize()-1]).GetValue());
			return f.Apply(new ULList(list));
		}
		
		public static LispObject Map(ULList arg) {
			var f = (Function)arg[0];
			var src_list = ((ULList)arg[1]).GetValue();
			var new_list = new List<LispObject>();
			Loop.Count(src_list.Count).Do(x=>new_list.Add(f.Apply(new ULList(src_list[x]))));
			return new ULList(new_list);
		}
		
		
		public static LispObject Meta(ULList arg) {return arg[0].GetMeta(); }
		
		public static LispObject With_meta(ULList arg) {return ((LispObject)arg[0]).Copy().SetMeta(arg[1]); }
		
		public static LispObject IsAtom(ULList arg) {return Compare(arg[0] is Atom); }
		
		public static LispObject Deref(ULList arg) {return ((Atom)arg[0]).GetValue(); }
		
		public static LispObject Reset_BANG(ULList arg) {return ((Atom)arg[0]).SetValue(arg[1]); }
		
		public static LispObject Swap_BANG(ULList arg) {
			Atom atom = (Atom)arg[0];
			Function f = (Function)arg[1];
			var new_list = new List<LispObject>();
			new_list.Add(atom.GetValue());
			new_list.AddRange(((ULList)arg.Slice(2)).GetValue());
			return atom.SetValue(f.Apply(new ULList(new_list)));
		}
		
		public static Dictionary<string, LispObject> function = new Dictionary<string, LispObject> {
			{"=", new Function(a=>Compare(ULCompare.IsEqual(a[0], a[1])))},
			{"throw", new Function(Throw)},
			{"nil?", new Function(IsNil)},
			{"true?", new Function(IsTrue)},
			{"false?", new Function(IsFalse)},
			{"symbol", new Function(a => new Symbol((ULString)a[0]))},
			{"symbol?", new Function(IsSymbol)},
			{"string?", new Function(IsString)},
			{"keyword", new Function(Keyword)},
			{"keyword?", new Function(IsKeyword)},
			
			{"pr-str", new Function(Print_str)},
			{"str", new Function(Str)},
			{"prn", new Function(Print)},
			{"readline", new Function(ReadLine)},
			{"read-string", new Function(ReadString)},
			{"slurp", new Function(Slurp)},
			{"<", new Function(a=>(Integer)a[0] < (Integer)a[1])},
			{"<=", new Function(a=>(Integer)a[0] <= (Integer)a[1])},
			{">", new Function(a=>(Integer)a[0] > (Integer)a[1])},
			{">=", new Function(a=>(Integer)a[0] >= (Integer)a[1])},
			{"+", new Function(a=>(Integer)a[0] + (Integer)a[1])},
			{"-", new Function(a=>(Integer)a[0] - (Integer)a[1])},
			{"*", new Function(a=>(Integer)a[0] * (Integer)a[1])},
			{"/", new Function(a=>(Integer)a[0] / (Integer)a[1])},
			{"time-ms", new Function(Time_ms)},
			
			{"list", new Function(a=> new ULList(a.GetValue()))},
			{"list?", new Function(IsList)},
			{"vector", new Function(a=> new ULVector(a.GetValue()))},
			{"vector?", new Function(IsVector)},
			{"hash-map", new Function(a=> new HashMap(a))},
			{"map?", new Function(IsHashMap)},
			{"contains?", new Function(IsContains)},
			{"assoc", new Function(Assoc)},
			{"dissoc", new Function(Dissoc)},
			{"get", new Function(Get)},
			{"keys", new Function(GetKeys)},
			{"vals", new Function(GetValues)},
			
			{"sequential?", new Function(IsSequential)},
			{"cons", new Function(Cons)},
			{"concat", new Function(Concat)},
			{"nth", new Function(Nth)},
			{"first", new Function(First)},
			{"rest", new Function(Rest)},
			{"empty?", new Function(IsEmpty)},
			{"count", new Function(Count)},
			{"conj", new Function(Conj)},
			{"seq", new Function(Seq)},
			{"apply", new Function(Apply)},
			{"map", new Function(Map)},
			
			{"with-meta", new Function(With_meta)},
			{"meta", new Function(Meta)},
			{"atom", new Function(a=> new Atom(a[0]))},
			{"atom?", new Function(IsAtom)},
			{"deref", new Function(Deref)},
			{"reset!", new Function(Reset_BANG)},
			{"swap!", new Function(Swap_BANG)},
		};
		
		
	}
}

















