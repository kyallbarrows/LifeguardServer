﻿using System;

namespace Parichay.Security.Entity
{
    [Serializable]
    public class BusinessException : ApplicationException
    {
        private readonly string _Key;

        public BusinessException(string key, string message) : base(message)
        {
            _Key = key;
        }

        public string Key
        {
            get { return _Key; }
        }
    }
}
