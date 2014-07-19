////////////////////////////////////////////////////////////////
//                                                            //
//  Neoforce Controls                                         //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//         File: KeyboardLayout.cs                            //
//                                                            //
//      Version: 0.7                                          //
//                                                            //
//         Date: 11/09/2010                                   //
//                                                            //
//       Author: Tom Shane                                    //
//                                                            //
////////////////////////////////////////////////////////////////
//                                                            //
//  Copyright (c) by Tom Shane                                //
//                                                            //
////////////////////////////////////////////////////////////////

#region //// Using /////////////
        
//////////////////////////////////////////////////////////////////////////////
using Microsoft.Xna.Framework.Input;
using System.Globalization;
using System.Collections.Generic;
using System;
using System.Text;
//////////////////////////////////////////////////////////////////////////////

#endregion


namespace TomShane.Neoforce.Controls
{     
		
	public class KeyboardLayout
	{    
        
    #region //// Fields ////////////
                
    //////////////////////////////////////////////////////////////////////////            
    private string name = "English";  
    public List<int> LayoutList = new List<int>();
    //////////////////////////////////////////////////////////////////////////    
   
    #endregion                
    
    #region //// Properties ////////
	  
	  //////////////////////////////////////////////////////////////////////////	  
    public virtual string Name
    {
      get { return name; }
      set { name = value; }
    }    
	  //////////////////////////////////////////////////////////////////////////
	  
	  #endregion        
	  
    #region //// Constructors //////
        
	  //////////////////////////////////////////////////////////////////////////
    public KeyboardLayout()
    {          
      LayoutList.Add(1033);
    }
    //////////////////////////////////////////////////////////////////////////
    
    #endregion      	  
        
    #region //// Methods ///////////
    
    ////////////////////////////////////////////////////////////////////////////    
    public virtual string GetKey(KeyEventArgs args)
    {
      string ret = "";
      
      if (args.Caps && !args.Shift) ret = KeyToString(args).ToUpper();
      else if (!args.Caps && args.Shift) ret = KeyToString(args).ToUpper();
      else if (args.Caps && args.Shift) ret = KeyToString(args).ToLower();
      else if (!args.Caps && !args.Shift) ret = KeyToString(args).ToLower();
      
      
      return ret;
    }
    ////////////////////////////////////////////////////////////////////////////    

    ////////////////////////////////////////////////////////////////////////////    
    protected virtual string KeyToString(KeyEventArgs args)
    {
      switch (args.Key)
      {
        case Keys.A:
          return "a";        
        case Keys.B:
          return "b";
        case Keys.C:
          return "c";
        case Keys.D:
          return "d";
        case Keys.E:
          return "e";
        case Keys.F:
          return "f";
        case Keys.G:
          return "g";
        case Keys.H:
          return "h";
        case Keys.I:
          return "i";
        case Keys.J:
          return "j";
        case Keys.K:
          return "k";
        case Keys.L:
          return "l";
        case Keys.M:
          return "m";
        case Keys.N:
          return "n";
        case Keys.O:
          return "o";
        case Keys.P:
          return "p";
        case Keys.Q:
          return "q";
        case Keys.R:
          return "r";
        case Keys.S:
          return "s";
        case Keys.T:
          return "t";
        case Keys.U:
          return "u";
        case Keys.V:
          return "v";
        case Keys.W:
          return "w";
        case Keys.X:
          return "x";
        case Keys.Y:
          return "y";
        case Keys.Z:
          return "z";

        case Keys.D0:
          return (args.Shift) ? ")" : "0";
        case Keys.D1:
          return (args.Shift) ? "!" : "1";
        case Keys.D2:
          return (args.Shift) ? "@" : "2";
        case Keys.D3:
          return (args.Shift) ? "#" : "3";
        case Keys.D4:
          return (args.Shift) ? "$" : "4";
        case Keys.D5:
          return (args.Shift) ? "%" : "5";
        case Keys.D6:
          return (args.Shift) ? "^" : "6";
        case Keys.D7:
          return (args.Shift) ? "&" : "7";
        case Keys.D8:
          return (args.Shift) ? "*" : "8";          
        case Keys.D9:
          return (args.Shift) ? "(" : "9";

        case Keys.OemPlus:
          return (args.Shift) ? "+" : "=";
        case Keys.OemMinus:
          return (args.Shift) ? "_" : "-";
        case Keys.OemOpenBrackets:
          return (args.Shift) ? "{" : "[";
        case Keys.OemCloseBrackets:
          return (args.Shift) ? "}" : "]";
        case Keys.OemQuestion:
          return (args.Shift) ? "?" : "/";
        case Keys.OemPeriod:
          return (args.Shift) ? ">" : ".";
        case Keys.OemComma:
          return (args.Shift) ? "<" : ",";
        case Keys.OemPipe:
          return (args.Shift) ? "|" : "\\";
        case Keys.Space:
          return " ";
        case Keys.OemSemicolon:
          return (args.Shift) ? ":" : ";";
        case Keys.OemQuotes:
          return (args.Shift) ? "\"" : "'";
        case Keys.OemTilde:
          return (args.Shift) ? "~" : "`";

        case Keys.NumPad0:
          return (args.Shift) ? "" : "0";
        case Keys.NumPad1:
          return (args.Shift) ? "" : "1";
        case Keys.NumPad2:
          return (args.Shift) ? "" : "2";
        case Keys.NumPad3:
          return (args.Shift) ? "" : "3";
        case Keys.NumPad4:
          return (args.Shift) ? "" : "4";
        case Keys.NumPad5:
          return (args.Shift) ? "" : "5";
        case Keys.NumPad6:
          return (args.Shift) ? "" : "6";
        case Keys.NumPad7:
          return (args.Shift) ? "" : "7";
        case Keys.NumPad8:
          return (args.Shift) ? "" : "8";
        case Keys.NumPad9:
          return (args.Shift) ? "" : "9";
        case Keys.Decimal:
          return (args.Shift) ? "" : ".";

        case Keys.Divide:
          return (args.Shift) ? "/" : "/";
        case Keys.Multiply:
          return (args.Shift) ? "*" : "*";
        case Keys.Subtract:
          return (args.Shift) ? "-" : "-";
        case Keys.Add:
          return (args.Shift) ? "+" : "+";
          
        default:
          return "";
      }           
    }    
    ////////////////////////////////////////////////////////////////////////////

    #endregion
	  
  }

    // Thai Keyboard Layout added by Ittipon <ittipon.bay@gmail.com>
    public class ThaiKeyboardLayout : KeyboardLayout
    {

        #region //// Constructors //////

        //////////////////////////////////////////////////////////////////////////
        public ThaiKeyboardLayout()
        {
            Name = "Thai";
            LayoutList.Clear();
            LayoutList.Add(1054);
        }
        //////////////////////////////////////////////////////////////////////////

        #endregion

        #region //// Methods ///////////

        ////////////////////////////////////////////////////////////////////////////    
        protected override string KeyToString(KeyEventArgs args)
        {
            switch (args.Key)
            {
                case Keys.A:
                    return (args.Shift) ? "ƒ" : "ø";
                case Keys.B:
                    return (args.Shift) ? "⁄" : "‘";
                case Keys.C:
                    return (args.Shift) ? "©" : "·";
                case Keys.D:
                    return (args.Shift) ? "Ø" : "°";
                case Keys.E:
                    return (args.Shift) ? "Æ" : "”";
                case Keys.F:
                    return (args.Shift) ? "‚" : "¥";
                case Keys.G:
                    return (args.Shift) ? "¨" : "‡";
                case Keys.H:
                    return (args.Shift) ? "Á" : "È";
                case Keys.I:
                    return (args.Shift) ? "≥" : "√";
                case Keys.J:
                    return (args.Shift) ? "Î" : "Ë";
                case Keys.K:
                    return (args.Shift) ? "…" : "“";
                case Keys.L:
                    return (args.Shift) ? "»" : " ";
                case Keys.M:
                    return (args.Shift) ? "?" : "∑";
                case Keys.N:
                    return (args.Shift) ? "Ï" : "◊";
                case Keys.O:
                    return (args.Shift) ? "œ" : "π";
                case Keys.P:
                    return (args.Shift) ? "≠" : "¬";
                case Keys.Q:
                    return (args.Shift) ? "" : "Ê";
                case Keys.R:
                    return (args.Shift) ? "±" : "æ";
                case Keys.S:
                    return (args.Shift) ? "¶" : "À";
                case Keys.T:
                    return (args.Shift) ? "∏" : "–";
                case Keys.U:
                    return (args.Shift) ? "Í" : "’";
                case Keys.V:
                    return (args.Shift) ? "Œ" : "Õ";
                case Keys.W:
                    return (args.Shift) ? "\"" : "‰";
                case Keys.X:
                    return (args.Shift) ? ")" : "ª";
                case Keys.Y:
                    return (args.Shift) ? "Ì" : "—";
                case Keys.Z:
                    return (args.Shift) ? "(" : "º";

                case Keys.D0:
                    return (args.Shift) ? "˜" : "®";
                case Keys.D1:
                    return (args.Shift) ? "+" : "Â";
                case Keys.D2:
                    return (args.Shift) ? "Ò" : "/";
                case Keys.D3:
                    return (args.Shift) ? "Ú" : "-";
                case Keys.D4:
                    return (args.Shift) ? "Û" : "¿";
                case Keys.D5:
                    return (args.Shift) ? "Ù" : "∂";
                case Keys.D6:
                    return (args.Shift) ? "Ÿ" : "ÿ";
                case Keys.D7:
                    return (args.Shift) ? "ﬂ" : "÷";
                case Keys.D8:
                    return (args.Shift) ? "ı" : "§";
                case Keys.D9:
                    return (args.Shift) ? "ˆ" : "µ";

                case Keys.OemPlus:
                    return (args.Shift) ? "˘" : "™";
                case Keys.OemMinus:
                    return (args.Shift) ? "¯" : "¢";
                case Keys.OemOpenBrackets:
                    return (args.Shift) ? "∞" : "∫";
                case Keys.OemCloseBrackets:
                    return (args.Shift) ? "," : "≈";
                case Keys.OemQuestion:
                    return (args.Shift) ? "∆" : "Ω";
                case Keys.OemPeriod:
                    return (args.Shift) ? "Ã" : "„";
                case Keys.OemComma:
                    return (args.Shift) ? "≤" : "¡";
                case Keys.OemPipe:
                    return (args.Shift) ? "•" : "£";
                case Keys.Space:
                    return " ";
                case Keys.OemSemicolon:
                    return (args.Shift) ? "´" : "«";
                case Keys.OemQuotes:
                    return (args.Shift) ? "." : "ß";
                case Keys.OemTilde:
                    return (args.Shift) ? "~" : "`";

                case Keys.NumPad0:
                    return (args.Shift) ? "" : "0";
                case Keys.NumPad1:
                    return (args.Shift) ? "" : "1";
                case Keys.NumPad2:
                    return (args.Shift) ? "" : "2";
                case Keys.NumPad3:
                    return (args.Shift) ? "" : "3";
                case Keys.NumPad4:
                    return (args.Shift) ? "" : "4";
                case Keys.NumPad5:
                    return (args.Shift) ? "" : "5";
                case Keys.NumPad6:
                    return (args.Shift) ? "" : "6";
                case Keys.NumPad7:
                    return (args.Shift) ? "" : "7";
                case Keys.NumPad8:
                    return (args.Shift) ? "" : "8";
                case Keys.NumPad9:
                    return (args.Shift) ? "" : "9";
                case Keys.Decimal:
                    return (args.Shift) ? "" : ".";

                case Keys.Divide:
                    return (args.Shift) ? "/" : "/";
                case Keys.Multiply:
                    return (args.Shift) ? "*" : "*";
                case Keys.Subtract:
                    return (args.Shift) ? "-" : "-";
                case Keys.Add:
                    return (args.Shift) ? "+" : "+";

                default:
                    return "";
            }
        }
        ////////////////////////////////////////////////////////////////////////////

        ////////////////////////////////////////////////////////////////////////////    
        ////////////////////////////////////////////////////////////////////////////

        #endregion

    }

  public class CzechKeyboardLayout: KeyboardLayout
  {

    #region //// Constructors //////

    //////////////////////////////////////////////////////////////////////////
    public CzechKeyboardLayout()
    {
        Name = "Czech";
        LayoutList.Clear();
        LayoutList.Add(1029);
    }
    //////////////////////////////////////////////////////////////////////////

    #endregion

    #region //// Methods ///////////

    ////////////////////////////////////////////////////////////////////////////    
    protected override string KeyToString(KeyEventArgs args)
    {                            
      switch (args.Key)
      {        
        case Keys.D0:
          return (args.Shift) ? "0" : "È";
        case Keys.D1:
          return (args.Shift) ? "1" : "+";
        case Keys.D2:
          return (args.Shift) ? "2" : "Ï";
        case Keys.D3:
          return (args.Shift) ? "3" : "ö";
        case Keys.D4:
          return (args.Shift) ? "4" : "Ë";
        case Keys.D5:
          return (args.Shift) ? "5" : "¯";
        case Keys.D6:
          return (args.Shift) ? "6" : "û";
        case Keys.D7:
          return (args.Shift) ? "7" : "˝";
        case Keys.D8:
          return (args.Shift) ? "8" : "·";
        case Keys.D9:
          return (args.Shift) ? "9" : "Ì";

        case Keys.OemPlus:
          return (args.Shift) ? "°" : "¥";
        case Keys.OemMinus:
          return (args.Shift) ? "%" : "=";
        case Keys.OemOpenBrackets:
          return (args.Shift) ? "/" : "˙";
        case Keys.OemCloseBrackets:
          return (args.Shift) ? "(" : ")";
        case Keys.OemQuestion:
          return (args.Shift) ? "_" : "-";
        case Keys.OemPeriod:
          return (args.Shift) ? ":" : ".";
        case Keys.OemComma:
          return (args.Shift) ? "?" : ",";
        case Keys.OemPipe:
          return (args.Shift) ? "'" : "®";
        case Keys.Space:
          return " ";
        case Keys.OemSemicolon:
          return (args.Shift) ? "\"" : "˘";
        case Keys.OemQuotes:
          return (args.Shift) ? "!" : "ß";
        case Keys.OemTilde:
          return (args.Shift) ? "∞" : ";";
        
        case Keys.Decimal:
          return (args.Shift) ? "" : ",";

        default:
          return base.KeyToString(args);
      }            
    }
    ////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////    
    ////////////////////////////////////////////////////////////////////////////

    #endregion

  }

  public class GermanKeyboardLayout : KeyboardLayout
  {

    #region //// Constructors //////

    //////////////////////////////////////////////////////////////////////////
    public GermanKeyboardLayout()
    {
      Name = "German";
      LayoutList.Clear();
      LayoutList.Add(1031);
    }
    //////////////////////////////////////////////////////////////////////////

    #endregion

    #region //// Methods ///////////

    ////////////////////////////////////////////////////////////////////////////    
    protected override string KeyToString(KeyEventArgs args)
    {
      switch (args.Key)
      {
        case Keys.D0:
          return (args.Shift) ? "=" : "0";
        case Keys.D1:
          return (args.Shift) ? "!" : "1";
        case Keys.D2:
          return (args.Shift) ? "\"": "2";
        case Keys.D3:
          return (args.Shift) ? "ß" : "3";
        case Keys.D4:
          return (args.Shift) ? "$" : "4";
        case Keys.D5:
          return (args.Shift) ? "%" : "5";
        case Keys.D6:
          return (args.Shift) ? "&" : "6";
        case Keys.D7:
          return (args.Shift) ? "/" : "7";
        case Keys.D8:
          return (args.Shift) ? "*" : "8";
        case Keys.D9:
          return (args.Shift) ? "(" : "9";

        case Keys.OemPlus:
          return (args.Shift) ? "*" : "+";
        case Keys.OemMinus:
          return (args.Shift) ? "_" : "-";
        case Keys.OemOpenBrackets:
          return (args.Shift) ? "?" : "ﬂ";
        case Keys.OemCloseBrackets:
          return (args.Shift) ? "`" : "¥";
        case Keys.OemQuestion:
          return (args.Shift) ? "'" : "#";
        case Keys.OemPeriod:
          return (args.Shift) ? ":" : ".";
        case Keys.OemComma:
          return (args.Shift) ? ";" : ",";
        case Keys.OemPipe:
          return (args.Shift) ? "∞" : "^";
        case Keys.Space:
          return " ";
        case Keys.OemSemicolon:
          return (args.Shift) ? "‹" : "¸";
        case Keys.OemQuotes:
          return (args.Shift) ? "ƒ" : "‰";
        case Keys.OemTilde:
          return (args.Shift) ? "÷" : "ˆ";
          
        case Keys.Decimal:
          return (args.Shift) ? "" : ".";

        default:
          return base.KeyToString(args);
      }
    }
    ////////////////////////////////////////////////////////////////////////////  
 
   
  #endregion
  
  }

  public class PolishKeyboardLayout: KeyboardLayout
  {

    #region //// Constructors //////
    //////////////////////////////////////////////////////////////////////////
    public PolishKeyboardLayout()
    {
      Name = "Polish"; 
      LayoutList.Clear();
      LayoutList.Add(1045);
    }
    //////////////////////////////////////////////////////////////////////////

    #endregion

    #region //// Methods ///////////

    ////////////////////////////////////////////////////////////////////////////
    protected override string KeyToString(KeyEventArgs args)
    {     
      if (args.Alt)
      {
        switch (args.Key)
        {
          case Keys.A:
            return (args.Shift) ? "•" : "π";
          case Keys.C:
            return (args.Shift) ? "∆" : "Ê";
          case Keys.E:
            return (args.Shift) ? " " : "Í";
          case Keys.L:
            return (args.Shift) ? "£" : "≥";
          case Keys.N:
            return (args.Shift) ? "—" : "Ò";
          case Keys.O:
            return (args.Shift) ? "”" : "Û";
          case Keys.S:
            return (args.Shift) ? "å" : "ú";
          case Keys.X:
            return (args.Shift) ? "è" : "ü";
          case Keys.Z:
            return (args.Shift) ? "Ø" : "ø";
        }
      }
      return base.KeyToString(args);
    }
    ////////////////////////////////////////////////////////////////////////////
    
    #endregion
  
  }
		
}

