using AutoMapper;
using dotnet_api_test.Models.Dtos;
using dotnet_api_test.Persistence.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using dotnet_api_test.Validation;

namespace dotnet_api_test.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly ILogger<DishController> _logger;
        private readonly IMapper _mapper;
        private readonly IDishRepository _dishRepository;

        public DishController(ILogger<DishController> logger, IMapper mapper, IDishRepository dishRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _dishRepository = dishRepository;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<DishesAndAveragePriceDto> GetDishesAndAverageDishPrice()
        {
            return new DishesAndAveragePriceDto()
            {
                Dishes = _mapper.Map<IEnumerable<ReadDishDto>>(_dishRepository.GetAllDishes()),
                AveragePrice = _dishRepository.GetAverageDishPrice()
            };
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<ReadDishDto> GetDishById(int id)
        {
            return Ok(_dishRepository.GetDishById(id));
        }

        [HttpPost]
        [Route("")]
        public ActionResult<ReadDishDto> CreateDish([FromBody] CreateDishDto createDishDto)
        {
            ModelValidation.ValidateCreateDishDto(createDishDto);
            var dishToCreate = _mapper.Map<Dish>(createDishDto);
            return Ok(_mapper.Map<ReadDishDto>(_dishRepository.CreateDish(dishToCreate)));
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<ReadDishDto> UpdateDishById(int id, UpdateDishDto updateDishDto)
        {
            ModelValidation.ValidateUpdateDishDto(updateDishDto);
            var dishToUpdate = _dishRepository.GetDishById(id);
            _mapper.Map(updateDishDto, dishToUpdate);
            return Ok(_mapper.Map<ReadDishDto>(_dishRepository.UpdateDish(dishToUpdate)));
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteDishById(int id)
        {
            _dishRepository.DeleteDishById(id);
            return Ok();
        }
    }
}