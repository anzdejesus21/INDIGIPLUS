﻿namespace INDIGIPLUS.Api.Common.Response
{
    public class RegisterResponse
    {
        #region Properties

        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserInfo? User { get; set; }

        #endregion Properties
    }
}