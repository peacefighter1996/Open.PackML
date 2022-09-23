using Autabee.Automation.Utility.IEC61131TypeConversion;
using Autabee.Utility;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Open.PackML.Tags
{
    public class TagDetail : TagConfig
    {

        private object baseObject;
        private MethodInfo getMethod;
        private object lastData;
        private MethodInfo setMethod;

        public TagDetail(TagConfig config, object baseObject, TagDetail[] childTags, PropertyInfo[] propertyInfos) : base(config)
        {
            if (baseObject == null)
            {
                throw new ArgumentNullException(nameof(baseObject));
            }
            DataType = config.DataType;
            this.baseObject = baseObject;
            if (childTags != null) ChildTags = childTags;
            else ChildTags = new TagDetail[0];

            PropertyInfos = propertyInfos;
            if (propertyInfos.Length >= 1)
            {
                PropertyInfo last = propertyInfos[propertyInfos.Length - 1];
                setMethod = last.GetSetMethod();
                getMethod = last.GetGetMethod();

            }

        }

        public ValidationResult<object> GetValue()
        {
            var result = baseObject;
            //if (PropertyInfos != null)
            //{
                foreach (var propertyInfo in PropertyInfos)
                {
                    result = propertyInfo.GetValue(result);
                }
            //}
            //else
            //{
            //    for (int j = 1; j < TagAddress.Length; j++)
            //    {
            //        if (result == null) return new ValidationResult<object>(false, null);
            //        result = result.GetType().GetProperty(TagAddress[j]).GetValue(result);
            //    }
            //}
            return new ValidationResult<object>(Object: result);
        }

        public ValidationResult<object> GetValueIsUpdated()
        {
            var result = GetValue();
            if (result.Success)
            {
                if (lastData == result.Object)
                {
                    return new ValidationResult<object>(false, null, "Data not updated");
                }
                else
                {
                    lastData = result.Object;
                    return new ValidationResult<object>(true, result.Object);
                }
            }
            else
            {
                return result;
            }
        }

        public ValidationResult<object> SetValue(object obj)
        {

            if (!Writable) return new ValidationResult<object>(false, null, "Object not writable");
            if (DataType != obj.GetType()) return new ValidationResult<object>(false, null, "Object type mismatch");
            var result = baseObject;
            //if (PropertyInfos != null)
            //{
                for (int i = 0; i < PropertyInfos.Length - 1; i++)
                {
                    result = PropertyInfos[i].GetValue(result);
                    if (result == null) return new ValidationResult<object>(false, null, "Object not found");
                }
                PropertyInfos[PropertyInfos.Length - 1].SetValue(result, obj);
            //}
            //else
            //{
            //    for (int j = 1; j < TagAddress.Length - 1; j++)
            //    {
            //        if (result == null) return new ValidationResult<object>(false, null);
            //        result = result.GetType().GetProperty(TagAddress[j]).GetValue(result);
            //        if (result == null) return new ValidationResult<object>(false, null, "Object not found");
            //    }
            //    result.GetType().GetProperty(TagAddress[TagAddress.Length - 1]).SetValue(result, obj);
            //}
            return new ValidationResult<object>(Object: result);
        }

        public TagDetail[] ChildTags { get; }
        public PropertyInfo[] PropertyInfos { get; }
        public bool Readable { get => !(getMethod != null && getMethod.IsPublic); }
        public bool Writable { get => !(setMethod != null && setMethod.IsPublic); }
    }
}