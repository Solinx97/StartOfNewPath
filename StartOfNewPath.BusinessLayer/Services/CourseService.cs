using AutoMapper;
using StartOfNewPath.BusinessLayer.DTO;
using StartOfNewPath.BusinessLayer.Exceptions;
using StartOfNewPath.BusinessLayer.Interfaces;
using StartOfNewPath.DataAccessLayer.Entities;
using StartOfNewPath.DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StartOfNewPath.BusinessLayer.Services
{
    internal class CourseService : IService<CourseDto>
    {
        private readonly IGenericRepository<Course> _repository;
        private readonly IMapper _mapper;

        public CourseService(IGenericRepository<Course> userRepository, IMapper mapper)
        {
            _repository = userRepository;
            _mapper = mapper;
        }

        Task<int> IService<CourseDto>.CreateAsync(CourseDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<CourseDto>.DeleteAsync(CourseDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<CourseDto>> IService<CourseDto>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<CourseDto>>(allData);

            return result;
        }

        async Task<CourseDto> IService<CourseDto>.GetByIdAsync(int id)
        {
            var executeLoad = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<CourseDto>(executeLoad);

            return result;
        }

        Task<int> IService<CourseDto>.UpdateAsync(CourseDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<int> CreateInternalAsync(CourseDto item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(item.Name));
            }

            var numberEntries = await _repository.CreateAsync(_mapper.Map<Course>(item));

            return numberEntries;
        }

        private async Task<int> DeleteInternalAsync(CourseDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CourseDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<Course>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(CourseDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(CourseDto)} not found", nameof(allData));
            }

            if (string.IsNullOrEmpty(item.Name))
            {
                throw new ArgumentNullException(nameof(item.Name));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<Course>(item));
            return numberEntries;
        }
    }
}
