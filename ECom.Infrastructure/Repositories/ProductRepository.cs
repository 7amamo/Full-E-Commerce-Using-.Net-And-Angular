using AutoMapper;
using ECom.Core.DTO;
using ECom.Core.Entities.product;
using ECom.Core.Interfaces;
using ECom.Core.Services;
using ECom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IImageManagementService _imageManagement;

        public ProductRepository(AppDbContext DbContext , IMapper mapper , IImageManagementService imageManagement) : base(DbContext)
        {
            _dbContext = DbContext;
            this._mapper = mapper;
            _imageManagement = imageManagement;
        }

        public async Task<bool> AddProductAsync(AddProductDTO productDTO)
        {
            if (productDTO == null) return false;

            var Product = _mapper.Map<Product>(productDTO);
            await _dbContext.Products.AddAsync(Product);
            await _dbContext.SaveChangesAsync();

            var ImagePath = await _imageManagement.AddImageAsync(productDTO.Photo, productDTO.Name);

            var photo = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = Product.Id
            }).ToList();
            await _dbContext.Photos.AddRangeAsync(photo);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateProductAsync(UpdateProductDTO productDTO)
        {
            if (productDTO is null) return false;

            var produt =await _dbContext.Products.Include(p=>p.Category)
                .Include(p=>p.Photos)
                .FirstOrDefaultAsync(p=>p.Id == productDTO.Id);

            if (produt is null) return false;

            _mapper.Map<Product>(productDTO);

            var Photo =await _dbContext.Photos.Where(p=>p.ProductId == productDTO.Id).ToListAsync();

            foreach (var photo in Photo)
            {
                _imageManagement.DeleteImageAsync(photo.ImageName);
            }
            _dbContext.Photos.RemoveRange(Photo);

            var PathNewPhotos =await _imageManagement.AddImageAsync(productDTO.Photo, productDTO.Name);
            var newImage = PathNewPhotos.Select(path => new Photo
            {
                ImageName=path,
                ProductId=productDTO.Id
            }).ToList();
            await _dbContext.AddRangeAsync(newImage);
            await _dbContext.SaveChangesAsync();
            return true;

        }

        public async Task DeleteProductAsync(Product product)
        {
            var photos =await _dbContext.Photos.Where(p=>p.ProductId == product.Id).ToListAsync();
            foreach (var item in photos)
            {
                _imageManagement.DeleteImageAsync(item.ImageName);
            }
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
        }
    }
}
