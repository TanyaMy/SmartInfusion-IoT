﻿using System;

namespace SmartInfusion_IoT.Data.Api
{
    public class ApiException : Exception
    {
        public ApiException()
        {
        }

        public ApiException(string message) : base(message)
        {
        }
    }
}
