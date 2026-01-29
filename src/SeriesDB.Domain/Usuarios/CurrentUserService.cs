using System;
using Volo.Abp.Users;

namespace SeriesDB.Usuarios
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly ICurrentUser _currentUser;

        public CurrentUserService(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }

        public Guid? GetCurrentUserId()
        {
            return _currentUser.Id;
        }
    }
}