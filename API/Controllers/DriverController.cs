using API.Exceptions;
using BusinessLayer;
using BusinessLayer.DTO;
using BusinessLayer.Exceptions;
using BusinessLayer.Managers;
using BusinessLayer.Mappers;
using BusinessLayer.Model;
using DataLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace API.Controllers
{
    [ApiController] // voor DI
    [Route("api/[controller]s")] // wordt .../Driver

    public class DriverController : Controller
    {
        private readonly ILogger<DriverController> logger;
        private IConfiguration iConfig;
        private DriverManager _driverManager;

        public DriverController(ILogger<DriverController> logger, IConfiguration config)
        {
            this.logger = logger;
            this.iConfig = config;

            _driverManager = new DriverManager(new DriverRepository(iConfig.GetValue<string>("ConnectionStrings:stolos")));
        }

        [HttpGet(Name = "GetDriverInfos")]
        public ActionResult<List<DriverInfo>> Get()
        {
            try
            {
                List <DriverInfo> driverInfos = _driverManager.GetDriverInfos();
                if (driverInfos == null) return NotFound();
                return Ok(driverInfos);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
                //throw new APIException("DriverController GetDriverInfos", ex);
            }
        }

        [HttpGet("{id}", Name = "GetDriverInfo")]
        public ActionResult<DriverInfo> Get(int id)
        {
            try
            {
                DriverInfo di = _driverManager.GetDriverInfoById(id);
                if (di == null) return NotFound();
                return Ok(di);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
                //throw new APIException($"DriverController GetDriverInfoById({id})", ex);
            }
        }

        [HttpPost(Name = "AddDriver")]
        public ActionResult Add([FromBody]DriverInfo di)
        {
            try
            {
                _driverManager.AddDriver(di);
                return Ok();
            }
            catch (Exception ex)
            {
                int code = 500;
                object? value = null;
                if (ex.InnerException is MySqlException && ex.InnerException.Message.Contains("Duplicate entry") && ex.InnerException.Message.Contains("Driver.uc_driver_natregnum"))
                {
                    code = 400;
                    value = $"A driver with National Registration Number {di.NatRegNum} already exists.";
                }
                else if (ex.InnerException is DomainException && ex.InnerException.Message.Contains("NatRegNumber"))
                {
                    code = 400;
                    value = $"{di.NatRegNum} is an invalid National Registration number.";
                }
                else if (ex.InnerException is DomainException && ex.InnerException.Message.Contains("BirthDate and RRN do not match"))
                {
                    code = 422;
                    value = $"{di.NatRegNum} BirthDate and RRN do not match.";
                }
                return StatusCode(code, value);
                //throw new APIException("DriverController AddDriver", ex);
            }
        }

        [HttpPut(Name = "UpdateDriver")]
        public ActionResult Put([FromBody]DriverInfo di)
        {
            try
            {
                if (_driverManager.GetDriverInfoById((int)di.DriverID) == null) return NotFound();
                _driverManager.UpdateDriver(di);
                return Ok();
            }
            catch (Exception ex)
            {
                int code = 500;
                object? value = null;
                if (ex.InnerException is MySqlException && ex.InnerException.Message.Contains("Duplicate entry") && ex.InnerException.Message.Contains("Driver.uc_driver_natregnum"))
                {
                    code = 400;
                    value = $"A different driver already has {di.NatRegNum} as their National Registration Number.";
                }
                else if (ex.InnerException is DomainException && ex.InnerException.Message.Contains("NatRegNumber"))
                {
                    code = 400;
                    value = $"{di.NatRegNum} is an invalid National Registration number.";
                }
                return StatusCode(code, value);
                //throw new APIException("DriverController UpdateDriver", ex);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                //soft delete
                DriverInfo di = _driverManager.GetDriverInfoById(id);
                if (di == null) return NotFound();
                _driverManager.DeleteDriver((int)di.DriverID);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
                //throw new APIException("DriverController DeleteDriver", ex);
            }
        }
    }
}
