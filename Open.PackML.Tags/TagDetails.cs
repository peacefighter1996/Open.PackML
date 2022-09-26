using Autabee.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Open.PackML.Tags
{
    public class TagDetail : TagConfig
    {
        private int arrayTreeCount;
        private bool[] ArrayType;

        private object baseObject;
        private MethodInfo getMethod;
        private MemberInfo last;

        private MemberInfo[] memberInfo;
        private ParameterInfo[] parameters;
        private MethodInfo setMethod;

        public TagDetail(TagConfig config, object baseObject, TagDetail[] childTags, MemberInfo[] memberInfos, bool[] arrayType) : base(config)
        {
            if (baseObject == null)
            {
                throw new ArgumentNullException(nameof(baseObject));
            }
            DataType = config.DataType;
            this.baseObject = baseObject;
            if (childTags != null) ChildTags = childTags;
            else ChildTags = new TagDetail[0];

            memberInfo = memberInfos;
            if (memberInfos.Length >= 1)
            {
                last = memberInfo[memberInfo.Length - 1];
                if (last is PropertyInfo propertyInfo)
                {
                    last = propertyInfo;
                    getMethod = propertyInfo.GetGetMethod();
                    setMethod = propertyInfo.GetSetMethod();
                }
                else if (last is MethodInfo methodInfo)
                {
                    last = methodInfo;
                    parameters = methodInfo.GetParameters();
                }
            }


            arrayTreeCount = TagName.Count(o => o == '[');
        }

        private bool IsMethod { get => last is MethodInfo; }
        private bool IsProperty { get => last is PropertyInfo; }
        private bool Readable { get => (getMethod != null && getMethod.IsPublic); }
        private bool Writable { get => (setMethod != null && setMethod.IsPublic); }

        public ValidationResult<object> Execute(Queue<int> queue, object[] args)
        {
            if (queue.Count() != arrayTreeCount) return new ValidationResult<object>(false, unSuccesfullText: "Array chain mismatch");
            if (!IsMethod) return new ValidationResult<object>(false, unSuccesfullText: "Not a method");
            if (args == null) args = Array.Empty<object>();
            if (args.Length != parameters.Length) return new ValidationResult<object>(false, unSuccesfullText: "tyring to call a function with {0} parmeters with {1}", formatObjects: new object[] { parameters.Length, args.Length });

            var validation = new ValidationResult<object>();
            for (int i = 0; i < parameters.Length; i++)
            {
                var parType = parameters[i].ParameterType;
                if (args == null && parType.IsClass) continue;
                else if (args[i].GetType() == parType) continue;
                else if (args[i].GetType().GetNestedTypes().Contains(parType)) continue;
                else if (args[i].GetType().GetInterfaces().Contains(parType)) continue;
                validation.AddResult(false, "Parameter {0} is not of type {1}", i, parType.Name);
            }
            if (!validation.Success) return validation;

            object result = baseObject;
            for (int i = 0; i < memberInfo.Length - 1; i++)
            {
                result = ArrayType[i]
                    ? ((PropertyInfo)memberInfo[i]).GetValue(result)
                    : (((PropertyInfo)memberInfo[i]).GetValue(result, new object[] { queue.Dequeue() }));
                if (result == null) return new ValidationResult<object>(false, null, "Object not found");
            }
            try
            {
                result = ((MethodInfo)last).Invoke(result, args);
                if (DataType != typeof(void) && result == null) return new ValidationResult<object>(false, null, "Object not found");
            }
            catch (Exception e)
            {
                return new ValidationResult<object>(false, null, e.Message);
            }
            return new ValidationResult<object>(Object: result);
        }
        public Task<ValidationResult<object>> ExecuteAsync(Queue<int> queue, object[] args)
        {
            return new Task<ValidationResult<object>>(delegate { return Execute(queue, args); });
        }

        public ValidationResult<T> GetValue<T>(Queue<int> queue)
        {
            if (!Readable) return new ValidationResult<T>(false, default, "Object not readable");
            if (!IsProperty) return new ValidationResult<T>(false, default, "Tag not a property");

            var result = baseObject;
            for (int i = 0; i < memberInfo.Length - 1; i++)
            {
                result = ((PropertyInfo)memberInfo[i]).GetValue(result);
            }
            PropertyInfo info = (PropertyInfo)last;

            if (info.PropertyType.IsArray && queue.Count == 1)
                return new ValidationResult<T>(Object: ((T[])info.GetValue(result))[queue.Dequeue()]);
            else
                return new ValidationResult<T>(Object: (T)info.GetValue(result));

        }

        //public ValidationResult<object> GetValueIsUpdated()
        //{
        //    var result = GetValue();
        //    if (result.Success)
        //    {
        //        if (lastData == result.Object)
        //        {
        //            return new ValidationResult<object>(false, null, "Data not updated");
        //        }
        //        else
        //        {
        //            lastData = result.Object;
        //            return new ValidationResult<object>(true, result.Object);
        //        }
        //    }
        //    else
        //    {
        //        return result;
        //    }
        //}

        public ValidationResult SetValue<T>(Queue<int> queue, T obj)
        {
            if (!Writable && !obj.GetType().IsArray) return new ValidationResult(false, "Object not writable");
            if (!IsProperty) return new ValidationResult(false, "Tag not a property");
            if (DataType.IsArray && DataType.GetElementType() != obj.GetType()
             || !DataType.IsArray && DataType != obj.GetType()) return new ValidationResult<T>(false, unSuccesfullText: "Object type mismatch");

            var result = baseObject;
            for (int i = 0; i < memberInfo.Length - 1; i++)
            {
                result = ((PropertyInfo)memberInfo[i]).GetValue(result);
                if (result == null) return new ValidationResult(false, "Object not found");
            }
            if (last is PropertyInfo info)
            {
                if (info.PropertyType.IsArray && queue.Count == 1)
                    ((T[])info.GetValue(result))[queue.Dequeue()] = obj;
                else if (!Writable) return new ValidationResult(false, "Object not writable");
                else
                    (info).SetValue(result, obj);
            }
            return new ValidationResult();

        }

        public TagDetail[] ChildTags { get; }
    }
}