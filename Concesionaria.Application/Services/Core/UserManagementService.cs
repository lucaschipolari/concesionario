using Concesionario.Application.Dto.Core;
using Concesionario.Application.Dto.Vehicles;
using Concesionario.Application.Interfaces;
using Concesionario.Data.Identity;
using Concesionario.Domain.Entities.Core;
using Concesionario.Domain.Entities.Vehicles;
using Concesionario.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concesionario.Application.Services.Core
{
    public class UserManagementService : IUserService
    {
        private readonly IRepository _repository;

        private readonly UserManager<ApplicationUserIdentity> _userManager;

        public UserManagementService(IRepository repository, UserManager<ApplicationUserIdentity> userManager) {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<UserResponseDto?> AddUser(UserRequestDto userRequestDto)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(userRequestDto.FullName) ||
                string.IsNullOrWhiteSpace(userRequestDto.DNI) ||
                string.IsNullOrWhiteSpace(userRequestDto.Phone) ||
                string.IsNullOrWhiteSpace(userRequestDto.Email) ||
                string.IsNullOrWhiteSpace(userRequestDto.Password) ||
                userRequestDto.UserType != UserType.Customer && userRequestDto.UserType != UserType.Employee)
            {
                throw new ArgumentException("Valores para el usuario no válidos");
            }

            // Validar que no exista un usuario con el mismo DNI
            var existingUser = await _repository.First<User>(u => u.DNI == userRequestDto.DNI);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Ya existe un usuario registrado con ese DNI.");
            }

            // 1. Crear y guardar el User primero
            var user = new User
            {
                FullName = userRequestDto.FullName,
                DNI = userRequestDto.DNI,
                Phone = userRequestDto.Phone,
                Email = userRequestDto.Email,
                UserType = userRequestDto.UserType,
                IsActive = true
            };
            await _repository.Add<User>(user);

            // 2. Crear el ApplicationUserIdentity y asociar el User
            var appUser = new ApplicationUserIdentity
            {
                UserName = userRequestDto.Email,
                Email = userRequestDto.Email,
                DomainUserId = user.Id,
                DomainUser = user,
                FullName = userRequestDto.FullName,
                DNI = userRequestDto.DNI,
                Phone = userRequestDto.Phone,
                IsActive = true,
                UserType = userRequestDto.UserType
            };

            var result = await _userManager.CreateAsync(appUser, userRequestDto.Password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException("No se pudo crear el usuario en Identity: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            if (userRequestDto.UserType == UserType.Customer)
            {
                var customer = new Customer
                {
                    UserId = user.Id,
                    User = user
                };
                await _repository.Add<Customer>(customer);
            }
            else if (userRequestDto.UserType == UserType.Employee)
            {
                var employee = new Employee
                {
                    UserId = user.Id,
                    User = user,
                };
                await _repository.Add<Employee>(employee);
            }

            return new UserResponseDto(
                user.FullName,
                user.DNI,
                user.Phone,
                user.IsActive,
                user.Email,
                user.UserType
            );
        }


    }
}
