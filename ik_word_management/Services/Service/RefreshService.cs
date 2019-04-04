using ik_word_management.Models.Domain;
using ik_word_management.Services.IService;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ik_word_management.Services.Service
{
    public class RefreshService : IRefreshService
    {
        private IKWordContext _iKWordContext = null;

        public RefreshService(IKWordContext iKWordContext)
        {
            _iKWordContext = iKWordContext;
        }

        public int AddRefresh(Guid id, int expiresIn)
        {
            Refresh refresh = new Refresh()
            {
                Id = Guid.NewGuid(),
                AccountId = id,
                RefreshToken = Guid.NewGuid(),
                ExpiresIn = DateTime.Now.AddSeconds(expiresIn + 60)
            };
            _iKWordContext.Entry(refresh).State = EntityState.Added;

            var result = _iKWordContext.SaveChanges();
            return result;
        }

        public int UpdateRefresh(Guid id, Guid newRefreshToken, int expiresIn)
        {
            var refresh = _iKWordContext.Set<Refresh>().Single(o => o.Id == id);

            refresh.RefreshToken = newRefreshToken;
            refresh.ExpiresIn = DateTime.Now.AddSeconds(expiresIn + 60);

            _iKWordContext.Set<Refresh>().Attach(refresh);

            _iKWordContext.Entry(refresh).Property(a => a.RefreshToken).IsModified = true;
            _iKWordContext.Entry(refresh).Property(a => a.ExpiresIn).IsModified = true;

            var result = _iKWordContext.SaveChanges();
            return result;
        }
    }
}
