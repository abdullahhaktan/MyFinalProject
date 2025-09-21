using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstract
{
    public interface ICvSummaryService
    {
        Task<string> GetSummaryAsync(string cvText);
    }
}
