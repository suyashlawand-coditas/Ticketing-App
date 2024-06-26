﻿namespace TicketingSystem.Core.ServiceContracts
{
    public interface ICacheService
    {
        Task<string?> Get(string key);

        Task Set(string key, object value, TimeSpan? timeSpan = null);

        Task Delete(string key);

        Task<bool> DoesExist(string key);
    }
}
