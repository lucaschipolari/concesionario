using Concesionario.Application.Dto.Core;
using Concesionario.Application.Dto.Vehicles;
using Concesionario.Application.Interfaces;
using Concesionario.Data.Identity;
using Concesionario.Domain.Entities.Core;
using Concesionario.Domain.Entities.Vehicles;
using BranchEntity = Concesionario.Domain.Entities.Branches.Branch;
using Concesionario.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
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
                userRequestDto.Email.Contains("ñ") ||
                string.IsNullOrWhiteSpace(userRequestDto.Password) ||
                userRequestDto.UserType != UserType.Customer && userRequestDto.UserType != UserType.Employee)
            {
                throw new ArgumentException("Valores para el usuario no válidos");
            }

            var existingUser = await _repository.First<User>(u => u.DNI == userRequestDto.DNI);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Ya existe un usuario registrado con ese DNI.");
            }

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

                var existingBranch = await _repository.GetById<BranchEntity>(userRequestDto.BranchId.Value) ?? throw new ArgumentException("La sucursal especificada no existe.");
                var existingPosition = await _repository.GetById<Position>(userRequestDto.PositionId.Value) ?? throw new ArgumentException("La posición especificada no existe.");

                if (existingBranch == null || existingPosition == null)
                {
                    throw new ArgumentException("La sucursal o la posición especificada no existen.");
                }

                var employee = new Employee
                {
                    UserId = user.Id,
                    User = user,
                    BranchId = existingBranch.Id,
                    PositionId = existingPosition.Id,
                };
                await _repository.Add<Employee>(employee);
            }

            return new UserResponseDto(
                user.Id,
                user.FullName,
                user.DNI,
                user.Phone,
                user.IsActive,
                user.Email,
                user.UserType
            );
        }

        public async Task<IEnumerable<UserResponseDto>?> GetUsers()
        {
            var users = await _repository.GetFiltered<User>(u => u.IsActive == true);
            return users?.Select(user => new UserResponseDto(
                user.Id,
                user.FullName,
                user.DNI,
                user.Phone,
                user.IsActive,
                user.Email,
                user.UserType
            ));
        }
        public async Task<UserResponseDto?> GetUserByName(string email) {
            var user = await _repository.First<User>(u => u.Email == email);
            if (user == null) { 
                return null;
            }
            return new UserResponseDto(
                user.Id,
                user.FullName,
                user.DNI,
                user.Phone,
                user.IsActive,
                user.Email,
                user.UserType
            );
        } 
        public async Task<IEnumerable<UserResponseDto>?> GetEmployeesByBranchId(Guid id) {

            var employees = await _repository.GetFiltered<Employee>(e => e.BranchId == id && e.User.IsActive == true);
            return employees?.Select(e => new UserResponseDto(
                e.User.Id,
                e.User.FullName,
                e.User.DNI,
                e.User.Phone,
                e.User.IsActive,
                e.User.Email,
                e.User.UserType
            )); 
        }
    }
}
