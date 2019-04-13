using MongoDB.Driver;
using mongohelper;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq;
namespace mongo
{
    public class User
    {

        public int Age { get; set; }
        public string Name { get; set; }

    }
    class Program
    {
        static void Main(string[] args)
        {

            //eg
            ExpresionBuilder<User> builder = new ExpresionBuilder<User>();
            builder.SetTag("u")
                   .Build("Age",20,ExpressionType.GreaterThan).
                    Merge().
                    Build("Name","lsh",ExpressionType.Equal).
                    Merge(ExpressionType.And);
            var expression = builder.ToLambda();
            Console.WriteLine(expression);
            List<User> users = new List<User>() {
                new User(){Age=20,Name="lsh"},
                new User(){ Age=11,Name="lsh01"}
            };

            List<User> items = users.Where(expression.Compile()).ToList();
            foreach (var item in items)
            {
                Console.WriteLine(item.Name);
            }
            Console.Read();
        }
    }
}
