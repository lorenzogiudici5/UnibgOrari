using OrariUnibg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.Services
{
    public interface INotification
    {
        void Notify();
        void SendNotification(CorsoGiornaliero corso);
    }
}
