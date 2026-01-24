using SeriesDB.Usuarios;
using System;

namespace SeriesDB.EntityFrameworkCore
{
    public class FakeCurrentUserService : ICurrentUserService
    {
        public Guid? GetCurrentUserId()
        {
            return Guid.NewGuid();
        }
    }
}