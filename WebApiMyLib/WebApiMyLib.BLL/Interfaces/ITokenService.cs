﻿using WebApiMyLib.Data.Models;

namespace WebApiMyLib.BLL.Interfaces
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, User user);
        bool ValidateToken(string key, string issuer, string audience, string token);
    }
}
