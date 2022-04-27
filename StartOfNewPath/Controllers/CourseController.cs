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
        readonly IService<CourseDto> _service;
        readonly IMapper _mapper;

        public CourseController(IService<CourseDto> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<CourseModel>> Get()
        {
            var courses = await _service.GetAllAsync();
            var result = _mapper.Map<IEnumerable<CourseModel>>(courses);

            return result;
        }

        [HttpGet("{id}")]
        public CourseModel GetById(int id)
        {
            return null;
        }

        [Authorize]
        [HttpPost]
        public async Task<int> Post(CourseModel course)
        {
            var mapCourse = _mapper.Map<CourseDto>(course);
            var result = await _service.CreateAsync(mapCourse);

            return result;
        }

        [Authorize]
        [HttpPut("{id}")]
        public void Put(int id, CourseModel course)
        {
        }

        [Authorize]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}