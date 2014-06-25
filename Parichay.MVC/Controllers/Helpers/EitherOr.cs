using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Parichay.MVC.Models
{
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class EitherOrAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "Either '{0}' or '{1}' must have a value.";
        private readonly object _typeId = new object();

        public EitherOrAttribute(string primaryProperty, string secondaryProperty)
            : base(_defaultErrorMessage)
        {
            PrimaryProperty = primaryProperty;
            SecondaryProperty = secondaryProperty;
        }

        public string PrimaryProperty { get; private set; }
        public string SecondaryProperty { get; private set; }

        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                PrimaryProperty, SecondaryProperty);
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object primaryValue = properties.Find(PrimaryProperty, true /* ignoreCase */).GetValue(value);
            object secondaryValue = properties.Find(SecondaryProperty, true /* ignoreCase */).GetValue(value);
            return primaryValue != null || secondaryValue != null;
        }
    }
}