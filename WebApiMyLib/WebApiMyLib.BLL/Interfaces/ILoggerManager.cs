﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiMyLib.BLL.Interfaces
{
    public interface ILoggerManager
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Debug(string message);

    }
}