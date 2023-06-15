using BusinessLayer.DTO;
using BusinessLayer.Interfaces;
using BusinessLayer.Mappers;
using BusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Managers
{
    public class VehicleManager
    {
        private IVehicleRepository _repo;
        public VehicleManager(IVehicleRepository repo)
        {
            _repo = repo;
        }

        #region GET
        public List<VehicleInfo> GetAllVehicleInfos()
        {
            List<VehicleInfo> vehicleInfos = new List<VehicleInfo>();
            foreach (Vehicle vehicle in _repo.GetAllVehicles())
            {
                vehicleInfos.Add(VehicleMapper.MapEntityToDto(vehicle));
            }
            return vehicleInfos;
        }

        public VehicleInfo GetVehicleByVIN(string vin)
        {
            return VehicleMapper.MapEntityToDto(_repo.GetVehicleByVIN(vin));
        }
        #endregion

        #region ADD
        public void AddVehicle(VehicleInfo vehicleInfo)
        {
            _repo.AddVehicle(VehicleMapper.MapDtoToEntity(vehicleInfo));
        }
        #endregion

        #region UPDATE
        public void UpdateVehicle(VehicleInfo vehicleInfo)
        {
            _repo.UpdateVehicle(VehicleMapper.MapDtoToEntity(vehicleInfo));
        }
        #endregion

        #region DELETE (soft)
        public void DeleteVehicle(VehicleInfo vehicleInfo)
        {
            _repo.DeleteVehicle(vehicleInfo.VIN);
        }
        #endregion
    }
}
