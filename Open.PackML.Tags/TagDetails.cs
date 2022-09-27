﻿using Autabee.Utility;
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
            ArrayType = arrayType;

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

            validation = MoveToLastBase(queue);
            if (!validation.Success) return validation;
            object result = validation.Object;

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

        private ValidationResult<object> MoveToLastBase(Queue<int> queue)
        {
            object result = baseObject;
            for (int i = 0; i < memberInfo.Length - 1; i++)
            {
                result = GetNextObject(ArrayType[i], (PropertyInfo)memberInfo[i], result, queue);
                if (result == null) return ObjectNotFound();
            }
            return new ValidationResult<object>(Object: result);
        }

        private static object GetNextObject(bool arraytype, PropertyInfo info, object obj, Queue<int> queue)
        {

            if (arraytype)
            {
                switch ((info).GetValue(obj))
                {
                    case Array a:
                        return a.GetValue(queue.Dequeue());
                    case IList b:
                        return b[queue.Dequeue()];
                    default:
                        return null;
                }
            }
            return info.GetValue(obj);
        }


        private static ValidationResult<object> ObjectNotFound()
        {
            return new ValidationResult<object>(false, null, "Object not found");
        }

        public Task<ValidationResult<object>> ExecuteAsync(Queue<int> queue, object[] args)
        {
            return new Task<ValidationResult<object>>(delegate { return Execute(queue, args); });
        }

        public ValidationResult<object> GetValue(Queue<int> queue)
        {
            if (!Readable) return new ValidationResult<object>(false, null, "Object not readable");
            if (!IsProperty) return new ValidationResult<object>(false, null, "Tag not a property");


            var validation = MoveToLastBase(queue);
            if (!validation.Success) return validation;;

            PropertyInfo info = (PropertyInfo)last;

            if (info.PropertyType.IsArray && queue.Count == 1)
                switch (info.GetValue(validation.Object))
                {
                    case Array a:
                        return new ValidationResult<object>(Object: a.GetValue(queue.Dequeue()));
                    case IList b:
                        return new ValidationResult<object>(Object: b[queue.Dequeue()]);
                    default:
                        return null;
                }
            else
                return new ValidationResult<object>(Object: info.GetValue(validation.Object));

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

        public ValidationResult SetValue(Queue<int> queue, object obj)
        {
            Type type = obj.GetType();
            if (!Writable && !DataType.IsArray) return new ValidationResult(false, "Object not writable");
            if (!IsProperty) return new ValidationResult(false, "Tag not a property");
            if ((type.IsArray && base.DataType.IsArray && base.DataType != type)
                || (!type.IsArray && base.DataType.IsArray && base.DataType.GetElementType() != type)
                || (!base.DataType.IsArray && base.DataType != type))
                return ObjectTypeMisMatch();


            var validation = MoveToLastBase(queue);
            if (!validation.Success) return validation;
            
            if (last is PropertyInfo info)
            {
                if (info.PropertyType.IsArray && queue.Count == 1)
                {
                    switch (info.GetValue(validation.Object))
                    {
                        case Array a:
                            a.SetValue(obj, queue.Dequeue());
                            break;
                        case IList b:
                            b[queue.Dequeue()] = obj;
                            break;
                        default:
                            return new ValidationResult(false, "Failed To write Array");
                    };
                }
                else if (!Writable) return new ValidationResult(false, "Object not writable");
                else
                    (info).SetValue(validation.Object, obj);
            }
            return new ValidationResult();

        }

        private static ValidationResult ObjectTypeMisMatch()
        {
            return new ValidationResult(false, "Object type mismatch");
        }

        public TagDetail[] ChildTags { get; }
    }
}