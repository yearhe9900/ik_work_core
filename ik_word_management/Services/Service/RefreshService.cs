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

        public int AddRefresh(Guid id, int expiresIn, Guid refreshToken)
        {
            Refresh refresh = new Refresh()
            {
                Id = Guid.NewGuid(),
                AccountId = id,
                RefreshToken = refreshToken,
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

        public Refresh GetRefreshByUserAccountID(Guid userAccountID)
        {
            var result = _iKWordContext.Refresh.Where(o => o.AccountId == userAccountID).FirstOrDefault();
            return result;
        }

        public Refresh GetRefreshByRefreshToken(Guid refreshToken)
        {
            var result = _iKWordContext.Refresh.Where(o => o.RefreshToken == refreshToken && o.ExpiresIn >= DateTime.Now).FirstOrDefault();
            return result;
        }
    }
}
