using System;
using System.Collections.Generic;
using PhotoGalleryBackendService.Data;
using PhotoGalleryBackendService.Dtos;
using System.Data.Entity;
using System.Linq;
using PhotoGalleryBackendService.Models;

namespace PhotoGalleryBackendService.Services
{
    public class AppService : IAppService
    {
        public AppService(IUow uow, ICacheProvider cacheProvider)
        {
            _uow = uow;
            _repository = uow.Apps;
            _cache = cacheProvider.GetCache();
        }

        public AppAddOrUpdateResponseDto AddOrUpdate(AppAddOrUpdateRequestDto request)
        {
            var entity = _repository.GetAll()
                .FirstOrDefault(x => x.Id == request.Id && x.IsDeleted == false);
            if (entity == null) _repository.Add(entity = new App());
            entity.Name = request.Name;
            _uow.SaveChanges();
            return new AppAddOrUpdateResponseDto(entity);
        }

        public dynamic Remove(int id)
        {
            var entity = _repository.GetById(id);
            entity.IsDeleted = true;
            _uow.SaveChanges();
            return id;
        }

        public ICollection<AppDto> Get()
        {
            ICollection<AppDto> response = new HashSet<AppDto>();
            var entities = _repository.GetAll().Where(x => x.IsDeleted == false).ToList();
            foreach(var entity in entities) { response.Add(new AppDto(entity)); }    
            return response;
        }


        public AppDto GetById(int id)
        {
            return new AppDto(_repository.GetAll().Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault());
        }

        protected readonly IUow _uow;
        protected readonly IRepository<App> _repository;
        protected readonly ICache _cache;
    }
}
