using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UWPLockStep.ApplicationLayer.Patients.Queries;

namespace LockStepMVC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMediator _mediator;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
            
        }

        public async Task<GetPatient.Model> GetPatient(string id)
        {

            return await _mediator.Send(new GetPatient.Query()
            {
                PatientId = id
            });
            
        }
        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var rng = new Random();
            var waiter =  await GetPatient("921330");
            var newx = waiter.QueriedPatient.GivenNames;
            
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = String.Join(" ",newx)
            })
            .ToArray();
        }
    }
}
