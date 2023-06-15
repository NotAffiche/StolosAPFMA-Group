using BusinessLayer.DTO;
using BusinessLayer.Interfaces;
using BusinessLayer.Mappers;
using BusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Managers;

public class DriverManager
{
    private IDriverRepository _repo;

	public DriverManager(IDriverRepository repo)
	{
		_repo = repo;
	}

    #region GET
    public List<DriverInfo> GetDriverInfos()
	{
        List<DriverInfo> driverInfos = new List<DriverInfo>();
        foreach (Driver driver in _repo.GetAllDrivers())
        {
            driverInfos.Add(DriverMapper.MapEntityToDto(driver));
        }
        return driverInfos;
	}

	public DriverInfo GetDriverInfoById(int id)
	{
		return DriverMapper.MapEntityToDto(_repo.GetDriverById(id));
	}
    #endregion

    #region ADD
    public void AddDriver(DriverInfo di)
    {
        Driver d = DriverMapper.MapDtoToEntity(di);
        _repo.AddDriver(d);
    }
    #endregion

    #region UPDATE
    public void UpdateDriver(DriverInfo di)
    {
        Driver d = DriverMapper.MapDtoToEntity(di);
        _repo.UpdateDriver(d);
    }
    #endregion

    #region DELETE (soft)
    public void DeleteDriver(int id)
    {
        _repo.DeleteDriver(id);
    }
    #endregion
}
