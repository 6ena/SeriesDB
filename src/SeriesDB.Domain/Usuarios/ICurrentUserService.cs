using System;

namespace SeriesDB.Usuarios
{
    public interface ICurrentUserService
    {
        Guid? GetCurrentUserId();
    }
}