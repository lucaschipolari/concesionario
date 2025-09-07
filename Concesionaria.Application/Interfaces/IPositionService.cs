using Concesionario.Application.Dto.Core.Employee;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Interfaces
{
    public interface IPositionService
    {
        Task<PositionResponseDto> AddPosition(PositionRequestDto positionRequestDto);
        Task<IEnumerable<PositionResponseDto>?> GetPositions();




    }
}
