﻿using BaltaStore.Domain.StoreContext.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaltaStore.Infra.Services
{
    public class EmailService : IEmailService
    {
        public void Send(string to, string from, string subject, string body)
        {
            
        }
    }
}
