using Concesionario.Application.Dto.Core.Employee;
using Concesionario.Application.Interfaces;
using Concesionario.Domain.Entities.Core;
using Concesionario.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Services.Core
{
    public class PositionManagementService : IPositionService
    {
        private readonly IRepository _repository;
        public PositionManagementService(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<PositionResponseDto> AddPosition(PositionRequestDto positionRequestDto)
        {
            if (string.IsNullOrEmpty(positionRequestDto.Name) || string.IsNullOrEmpty(positionRequestDto.Description))
            {
                throw new ArgumentNullException("Alguno de los datos es nulo");
            }
            var position = await _repository.GetFiltered<Position>(p => p.Name.ToLower().Trim() == positionRequestDto.Name.ToLower().Trim());
            if (position != null && position.Any())
            {
                throw new DuplicateNameException("Ya existe una posicion con dicho nombre");
            }
            var newPosition = new Position
            {
                Name = positionRequestDto.Name,
                Description = positionRequestDto.Description
            };

            await _repository.Add<Position>(newPosition);

            return new PositionResponseDto(
                newPosition.Id,
                newPosition.Name,
                newPosition.Description
            );
        }

        public async Task<IEnumerable<PositionResponseDto>?> GetPositions()
        {
           return (await _repository.GetAll<Position>())?.Select(p => new PositionResponseDto(p.Id, p.Name, p.Description));   
        }
    }
}
