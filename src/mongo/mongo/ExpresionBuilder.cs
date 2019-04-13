﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace mongohelper
{
    /// <summary>
    /// lambda表达式基础构建类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExpresionBuilder<T> where T:class,new()
    {

        public ExpresionBuilder()
        {
           
        }
        private BinaryExpression Left { get;set; }
        private BinaryExpression Right { get; set; }
        private List<ParameterExpression> Params { get; set; }
        private string Tag { get; set; }
        public ExpresionBuilder<T> SetTag(string tag)
        {
            Tag = tag;
            return this;
        }
        /// <summary>
        /// 构建参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ExpresionBuilder<T> Build(string key, object value, ExpressionType type)
        {
            //PropertyInfo info=typeof(T).GetProperty(key);
            ParameterExpression left = Expression.Parameter(value.GetType(), $"{Tag}.{key}");
            ConstantExpression right = Expression.Constant(value);
            BinaryExpression binary = Expression.MakeBinary(type, left, right);
            Right = binary;
           
            return this;
        }
        /// <summary>
        /// 合并表达式
        /// </summary>
        /// <param name="right"></param>
        /// <param name="type"></param>
        public ExpresionBuilder<T> Merge(ExpressionType type=ExpressionType.And)
        {
            if (Left == null)
            {
                Left = Right;
            }
            else
            {
                BinaryExpression binary = Expression.MakeBinary(type, Left,Right);
                Left = binary;
            }
            return this;
        }
        /// <summary>
        /// 转换表达式
        /// </summary>
        /// <returns></returns>
        public Expression<Func<T, bool>> ToLambda()
        {
          
           
            return Expression.Lambda<Func<T, bool>>(Left, Expression.Parameter(typeof(T), Tag));
        }

    }
}