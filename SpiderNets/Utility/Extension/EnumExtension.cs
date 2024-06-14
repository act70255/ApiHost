using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderNets.Utility.Extension
{
    public class PostfixAttribute : Attribute
    {
        public string Postfix { get; protected set; }

        public PostfixAttribute(string value)
        {
            Postfix = value;
        }
    }
    public class CodeAttribute : Attribute
    {
        public string Code { get; protected set; }

        public CodeAttribute(string value)
        {
            Code = value;
        }
    }
    public class CategoryNameAttribute : Attribute
    {
        public string CategoryName { get; protected set; }

        public CategoryNameAttribute(string value)
        {
            CategoryName = value;
        }
    }
    public class CounterNameAttribute : Attribute
    {
        public string CounterName { get; protected set; }

        public CounterNameAttribute(string value)
        {
            CounterName = value;
        }
    }
    public class InstanceNameAttribute : Attribute
    {
        public string InstanceName { get; protected set; }

        public InstanceNameAttribute(string value)
        {
            InstanceName = value;
        }
    }
    public static class EnumExtension
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
        public static string GetPostfix(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(PostfixAttribute)) as PostfixAttribute;
            return attribute == null ? value.ToString() : attribute.Postfix;
        }
        public static string GetCode(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(CodeAttribute)) as CodeAttribute;
            return attribute == null ? value.ToString() : attribute.Code;
        }
        public static string GetCategoryName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(CategoryNameAttribute)) as CategoryNameAttribute;
            return attribute == null ? value.ToString() : attribute.CategoryName;
        }
        public static string GetCounterName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(CounterNameAttribute)) as CounterNameAttribute;
            return attribute == null ? value.ToString() : attribute.CounterName;
        }
        public static string GetInstanceName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(InstanceNameAttribute)) as InstanceNameAttribute;
            return attribute == null ? "" : attribute.InstanceName;
        }
    }
}
