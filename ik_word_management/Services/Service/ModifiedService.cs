using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ik_word_management.Models.Domain;
using ik_word_management.Models.Enum;
using ik_word_management.Models.Options;
using ik_word_management.Services.IService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ik_word_management.Services.Service
{
    public class ModifiedService : IModifiedService
    {
        private IKWordContext _iKWordContext = null;
        private readonly ETagOptions _etagOptions;

        public ModifiedService(IKWordContext iKWordContext, IOptions<ETagOptions> etagOptions)
        {
            _iKWordContext = iKWordContext;
            _etagOptions = etagOptions.Value;
        }

        public Modified GetLastModified()
        {
            return _iKWordContext.Modified.OrderByDescending(o => o.Cdt).FirstOrDefault();
        }

        public int AddOneModified()
        {
            Modified modified = new Modified()
            {
                Id = Guid.NewGuid(),
                Cdt = DateTime.Now,
                Enable = (int)EnableEnum.Enable,
                Etag = _etagOptions.Version
            };
            _iKWordContext.Entry(modified).State = EntityState.Added;

            var result = _iKWordContext.SaveChanges();

            return result;
        }
    }
}
