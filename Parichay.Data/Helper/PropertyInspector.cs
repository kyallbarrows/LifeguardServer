using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Parichay.Data.Helper
{
   public static class PropertyInspector
    {
        public static object GetObjectProperty(object item, string property)
        {
            if (item == null)
                return null;

            int dotIdx = property.IndexOf('.');

            if (dotIdx > 0)
            {
                object obj = GetObjectProperty(item, property.Substring(0, dotIdx));

                return GetObjectProperty(obj, property.Substring(dotIdx + 1));
            }

            PropertyInfo propInfo = null;
            Type objectType = item.GetType();

            while (propInfo == null && objectType != null)
            {
                propInfo = objectType.GetProperty(property,
                          BindingFlags.Public
                        | BindingFlags.Instance
                        | BindingFlags.DeclaredOnly);

                objectType = objectType.BaseType;
            }

            if (propInfo != null)
                return propInfo.GetValue(item, null);

            FieldInfo fieldInfo = item.GetType().GetField(property,
                          BindingFlags.Public | BindingFlags.Instance);

            if (fieldInfo != null)
                return fieldInfo.GetValue(item);

            return null;
        }


        public static void SetPropertyValue(ref object theObject, string theProperty, object theValue)
        {
            //var msgInfo = theObject.GetType().GetProperty(theProperty);
            PropertyInfo propertyInfo = theObject.GetType().GetProperty(theProperty);
            if (propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(theObject, Convert.ChangeType(theValue, propertyInfo.PropertyType), null);
            }

        }

        public static object[] ListToTypeArray(System.Collections.IList objList, Type targetType)
        {
            object[] a = new object[objList.Count];

            for (int i = 0; i < objList.Count; i++)
            {
                object[] itm = (object[])objList[i];
                object obj = Activator.CreateInstance(targetType);

                int k = 0;
                foreach (PropertyInfo propertyInfo in targetType.GetProperties())
                {
                    // PropertyInfo propertyInfo = theObject.GetType().GetProperty(theProperty);
                    if (propertyInfo.CanWrite)
                    {
                        propertyInfo.SetValue(obj, Convert.ChangeType(itm[k], propertyInfo.PropertyType), null);
                    }
                    k++;
                }
                a[i] = obj;
            }
            return a;
        }


        public static object[] ListToTypeArray(System.Collections.IList objList, Type targetType, string[] propertyNamesInOrder)
        {
            object[] a = new object[objList.Count];

            for (int i = 0; i < objList.Count; i++)
            {
                object[] itm = (object[])objList[i];
                object obj = Activator.CreateInstance(targetType);

                int k = 0;
                foreach (var prop in propertyNamesInOrder)
                {
                    PropertyInfo propertyInfo = targetType.GetProperty(prop);
                    if (propertyInfo.CanWrite)
                    {
                        propertyInfo.SetValue(obj, Convert.ChangeType(itm[k], propertyInfo.PropertyType), null);
                    }
                    k++;
                }
                a[i] = obj;
            }
            return a;
        }

    }
}
