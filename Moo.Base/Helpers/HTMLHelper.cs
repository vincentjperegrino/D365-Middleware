using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTI.Moo.Base.Helpers;

public static class HTML
{

    public static string TableBorder(string innerhtml) => $"<table style='border: 1px solid black'>{innerhtml}</table>";
    public static string THBorder(string innerhtml) => $"<th style='border: 1px solid black'>{innerhtml}</th>";
    public static string TDBorder(string innerhtml) => $"<td style='border: 1px solid black'>{innerhtml}</td>";
   

}
