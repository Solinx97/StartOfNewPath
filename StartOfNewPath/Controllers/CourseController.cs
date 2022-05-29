using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StartOfNewPath.BusinessLayer.DTO;
using StartOfNewPath.BusinessLayer.Interfaces;
using StartOfNewPath.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StartOfNewPath.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly IService<CourseDto> _service;
        private readonly IMapper _mapper;

        public CourseController(IService<CourseDto> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<CourseModel>> Get()
        {
            var courses = await _service.GetAllAsync();
            var result = _mapper.Map<IEnumerable<CourseModel>>(courses);

            return result;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<CourseModel> GetById(int id)
        {
            var course = await _service.GetByIdAsync(id);
            var result = _mapper.Map<CourseModel>(course);

            return result;
        }

        [Authorize]
        [HttpPost]
        public async Task<int> Create(CourseModel course)
        {
            var mapCourse = _mapper.Map<CourseDto>(course);
            var result = await _service.CreateAsync(mapCourse);

            return result;
        }

        [Authorize]
        [HttpPut()]
        public async Task<int> Update(CourseModel course)
        {
            var mapCourse = _mapper.Map<CourseDto>(course);
            var result = await _service.UpdateAsync(mapCourse);

            return result;
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            var course = await _service.GetByIdAsync(id);
            var result = await _service.DeleteAsync(course);

            return result;
        }
    }
}