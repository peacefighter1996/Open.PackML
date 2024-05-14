using Autabee.Utility;
using Autabee.Utility.Messaging.Validation;
using Open.PackML.Tags.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Open.PackML.Tags
{
    internal class TagDetail : TagConfig
    {
        private int arrayTreeCount;
        private bool[] ArrayType;

        private object baseObject;
        private MethodInfo getMethod;
        private MemberInfo last;

        private MemberInfo[] memberInfo;
        private ParameterInfo[] parameters;
        private MethodInfo setMethod;

        internal TagDetail(TagConfig config, TagTreeBuilderProcessData tagBuilder, TagDetail[] childTags, int length = -1) : base(config)
        {
            if (tagBuilder.baseObject == null)
            {
                throw new ArgumentNullException(nameof(tagBuilder.baseObject));
            }
            baseObject = tagBuilder.baseObject;
            if (childTags != null) ChildTags = childTags;
            else ChildTags = new TagDetail[0];

            memberInfo = tagBuilder.PropertyChain.ToArray();
            if (memberInfo.Length >= 1)
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
            ArrayType = tagBuilder.ArrayChain.ToArray();

            arrayTreeCount = Name.Count(o => o == '[');
            Length = length;
        }

        private void CheckPameters(object[] args, ValidationResult<object> validation)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                var parType = parameters[i].ParameterType;
                if (args == null && parType.IsClass) continue;
                else if (args[i].GetType() == parType) continue;
                else if (args[i].GetType().GetNestedTypes().Contains(parType)) continue;
                else if (args[i].GetType().GetInterfaces().Contains(parType)) continue;
                validation.AddResult(false, "Parameter {0} is not of type {1}", i, parType.Name);
            }
        }
        
        private bool CheckTyping(Type type, bool InsertElement)
        {
            return IsArray && (!InsertElement && DataType != type
                            || InsertElement && DataType.GetElementType() != type
                            ) || !IsArray && DataType != type;

        }

        private static object GetNextObject(bool arraytype, PropertyInfo info, object obj, Queue<int> queue)
        {

            if (arraytype)
            {
                switch (info.GetValue(obj))
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

        private ValidationResult<object> MoveToLastBase(Queue<int> queue)
        {
            object result = baseObject;
            for (int i = 0; i < memberInfo.Length - 1; i++)
            {
                result = GetNextObject(ArrayType[i], (PropertyInfo)memberInfo[i], result, queue);
                if (result == null) return ObjectNotFound(TagAddress, i);
            }
            return new ValidationResult<object>(Object: result);
        }

        private static ValidationResult<object> ObjectNotFound(string tagName)
        {
            return new ValidationResult<object>(false, null, "Object not found at {0}", tagName);
        }
        private static ValidationResult<object> ObjectNotFound(string[] address, int i)
        {
            address = address.Take(i + 1).ToArray();
            return new ValidationResult<object>(false, null, "Object not found at {0}", string.Join(".", address));
        }
        private static ValidationResult<object> ObjectNotWritable(string TagName)
        {
            return new ValidationResult<object>(false, "{0} not writable", TagName);
        }

        private static ValidationResult<object> ObjectTypeMisMatch(Type expected, Type actually) => new ValidationResult<object>(false, "Object type mismatch. Expected {0} but got {1}", expected.Name, actually.Name);

        private static bool SetValue(bool arraytype, PropertyInfo info, object baseObj, object value, Queue<int> queue)
        {
            if (arraytype)
            {
                switch (info.GetValue(baseObj))
                {
                    case Array a:
                        a.SetValue(value, queue.Dequeue());
                        break;
                    case IList b:
                        b[queue.Dequeue()] = value;
                        break;
                    default:
                        return false;
                };
            }
            else
                info.SetValue(baseObj, value);
            return true;
        }
        private ValidationResult<object> TagNotAProperty()
        {
            return new ValidationResult<object>(false, null, "Tag {0} is not a property", Name);
        }
        private ValidationResult<object> TagNotReadable()
        {
            return new ValidationResult<object>(false, null, "Object not readable");
        }

        private bool IsArray { get => DataType.IsArray; }
        private bool IsMethod { get => last is MethodInfo; }
        private bool IsProperty { get => last is PropertyInfo; }
        private bool Readable { get => getMethod != null && getMethod.IsPublic; }
        private bool Writable { get => setMethod != null && setMethod.IsPublic; }

        public ValidationResult<object> Execute(Queue<int> queue, object[] args)
        {
            var validation = new ValidationResult<object>();
            if (queue.Count() != arrayTreeCount) validation.AddResult(false, "Array chain mismatch");
            if (!IsMethod) validation.AddResult(false, "Not a method");
            if (args == null) args = Array.Empty<object>();
            if (args.Length != parameters.Length) validation.AddResult(false, "tyring to call a function with {0} parmeters with {1}", formatObjects: new object[] { parameters.Length, args.Length });
            if (!validation.Success) return validation;
            CheckPameters(args, validation);
            if (!validation.Success) return validation;

            validation = MoveToLastBase(queue);
            if (!validation.Success) return validation;
            object result = validation.Object;

            try
            {
                result = ((MethodInfo)last).Invoke(result, args);
                if (DataType != typeof(void) && result == null) return ObjectNotFound(Name);
            }
            catch (Exception e)
            {
                return new ValidationResult<object>(false, null, e.Message);
            }
            return new ValidationResult<object>(Object: result);
        }



        public ValidationResult<object> GetValue(Queue<int> queue)
        {
            if (!Readable) return TagNotReadable();
            if (!IsProperty) return TagNotAProperty();

            var validation = MoveToLastBase(queue);
            if (!validation.Success) return validation;
            PropertyInfo info = (PropertyInfo)last;
            return new ValidationResult<object>(Object: GetNextObject(
                info.PropertyType.IsArray && queue.Count == 1, info, validation.Object, queue));

        }

        public ValidationResult SetValue(Queue<int> queue, object obj)
        {
            Type type = obj.GetType();
            var InsertElement = arrayTreeCount == queue.Count;
            if (!IsProperty)
                return TagNotAProperty();
            if (!Writable && (!IsArray || !InsertElement))
                return ObjectNotWritable(Name);
            if (CheckTyping(type, InsertElement))
                return ObjectTypeMisMatch(DataType, type);


            var validation = MoveToLastBase(queue);
            if (!validation.Success) return validation;

            PropertyInfo info = (PropertyInfo)last;
            SetValue(info.PropertyType.IsArray && queue.Count == 1, info, validation.Object, obj, queue);

            return new ValidationResult();

        }

        public TagDetail[] ChildTags { get; }
        public int Length { get; private set; }
    }
}