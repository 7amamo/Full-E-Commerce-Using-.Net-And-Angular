using ECom.Core.Entities.product;
using ECom.Core.Interfaces;
using ECom.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Infrastructure.Repositories
{
    public class PhotoRepository : GenericRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(AppDbContext DbContext) : base(DbContext)
        {
        }
    }
}
