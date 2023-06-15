using BusinessLayer.DTO;
using BusinessLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces;

public interface IGasCardRepository
{
    List<GasCard> GetAllGasCards();
    GasCard GetGasCard(string cardNum);
    void AddGasCard(GasCard gc);
    void UpdateGasCard(GasCard gc);
    void DeleteGasCard(string cardNum);
}
