using System;
using System.Linq.Expressions;
using System.Web;

namespace mongohelper
{
    public class ExpressionOption 
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public ExpressionType Type { get; set; }
    }
}
