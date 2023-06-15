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
    public class GasCardManager
    {
        public IGasCardRepository _repo;

        public GasCardManager(IGasCardRepository repo)
        {
            _repo = repo;
        }

        #region GET
        public List<GasCardInfo> GetAllGasCardsInfos()
        {
            List<GasCardInfo> gasCardInfos = new List<GasCardInfo>();
            foreach (GasCard gc in _repo.GetAllGasCards())
            {
                gasCardInfos.Add(GasCardMapper.MapEntityToDto(gc));
            }
            return gasCardInfos;
        }

        public GasCardInfo GetGasCardInfo(string cn)
        {
            return GasCardMapper.MapEntityToDto(_repo.GetGasCard(cn));
        }
        #endregion

        #region ADD
        public void AddGasCard(GasCardInfo gci)
        {
            _repo.AddGasCard(GasCardMapper.MapDtoToEntity(gci));
        }
        #endregion

        #region UPDATE
        public void UpdateGasCard(GasCardInfo gci)
        {
            _repo.UpdateGasCard(GasCardMapper.MapDtoToEntity(gci));
        }
        #endregion

        #region DELETE (soft)
        public void DeleteGasCard(GasCardInfo gci)
        {
            _repo.DeleteGasCard(gci.CardNumber);
        }
        #endregion
    }
}
