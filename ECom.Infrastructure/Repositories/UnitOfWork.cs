using AutoMapper;
using ECom.Core.Interfaces;
using ECom.Core.Services;
using ECom.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IImageManagementService _managementService;

        //private readonly IMapper mapper;
        //private readonly IImageManagementService managementService;


        public UnitOfWork(AppDbContext dbContext , IMapper mapper , IImageManagementService managementService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _managementService = managementService;

            CategoryRepository = new CategoryRepository(_dbContext);
            PhotoRepository = new PhotoRepository(_dbContext);
            ProductRepository = new ProductRepository(_dbContext ,  mapper , managementService);
        }

        public ICategoryRepository CategoryRepository{ get; }

        public IPhotoRepository PhotoRepository { get; }

        public IProductRepository ProductRepository { get; }
    }
}
