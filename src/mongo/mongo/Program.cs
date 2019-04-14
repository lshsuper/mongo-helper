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
            //1.创建构造器
            ExpresionBuilder<User> builder = new ExpresionBuilder<User>();
            //2.构造参数体
            builder.SetTag("u")
                   .Build("Age",20,ExpressionType.GreaterThan).
                    Merge().
                    Build("Name","lsh",ExpressionType.Equal).
                    Merge(ExpressionType.And);
            var expression = builder.ToExpression();
            Console.WriteLine("表达式:"+expression);
            //3.测试
            List<User> users = new List<User>() {
                new User(){Age=277,Name="lsh"},
                new User(){ Age=11,Name="lsh01"}
            };
            List<User> items = users.Where(builder.ToLambda()).ToList();
            foreach (var item in items)
            {
                Console.WriteLine(item.Name);
            }
            Console.Read();
        }
    }
}
