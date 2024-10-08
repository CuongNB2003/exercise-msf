﻿using MsfServer.Application.Contracts.User;
using Dapper;
using System.Data;
using MsfServer.Domain.Security;
using MsfServer.Domain.Shared.Responses;
using Microsoft.AspNetCore.Http;
using MsfServer.Domain.Shared.Exceptions;
using MsfServer.Application.Contracts.User.Dto;
using MsfServer.Domain.Shared.PagedResults;
using MsfServer.Application.Dapper;
using MsfServer.Application.Contracts.Role.Dto;
using Newtonsoft.Json;

namespace MsfServer.Application.Repositories
{
    public class UserRepository(string connectionString) : IUserRepository
    {
        private readonly string _connectionString = connectionString;

        // thêm user
        public async Task<ResponseText> CreateUserAsync(CreateUserInput input)
        {
            // Tạo dữ liệu
            byte[] salt = PasswordHashed.GenerateSalt();
            string hashedPassword = PasswordHashed.HashPassword("111111", salt);
            var user = UserDto.CreateUserAdminDto(input.Email, hashedPassword, input.RoleId, input.Avatar, salt);
            var userJson = JsonConvert.SerializeObject(user);
            // Thêm người dùng
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var userId = await connection.ExecuteAsync(
                "User_Create",
                 new { UserJson = userJson },
                 commandType: CommandType.StoredProcedure
            );

            return ResponseText.ResponseSuccess("Thêm thành công", StatusCodes.Status201Created);
        }

        // sửa user
        public async Task<ResponseText> UpdateUserAsync(UpdateUserInput input, int id)
        {
            // Chuyển đổi dữ liệu đầu vào thành JSON
            var userJson = JsonConvert.SerializeObject(new
            {
                input.Name,
                input.Email,
                input.RoleId,
                input.Avatar
            });

            // Cập nhật
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var result = await connection.ExecuteAsync(
                "User_Update",
                new { UserJson = userJson, Id = id },
                commandType: CommandType.StoredProcedure
            );
            return ResponseText.ResponseSuccess("Sửa thành công.", StatusCodes.Status204NoContent);
        }

        // xóa user
        public async Task<ResponseText> DeleteUserAsync(int id)
        {
            // Xóa người dùng
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var result = await connection.ExecuteAsync(
                "User_Delete",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
            return ResponseText.ResponseSuccess("Xóa thành công.", StatusCodes.Status204NoContent);
        }

        // lấy user theo id
        public async Task<ResponseObject<UserResponse>> GetUserByIdAsync(int id)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();

            using var multi = await connection.QueryMultipleAsync(
                "User_GetById",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );

            var user = await multi.ReadSingleOrDefaultAsync<UserResponse>();
            var role = await multi.ReadSingleOrDefaultAsync<RoleDto>();

            if (user == null || role == null)
            {
                throw new CustomException(StatusCodes.Status404NotFound, "Không tìm thấy User hoặc Role.");
            }

            user.Role = role;
            return ResponseObject<UserResponse>.CreateResponse("Lấy dữ liệu thành công.", user);
        }


        // lấy tất cả user
        public async Task<ResponseObject<PagedResult<UserResponse>>> GetUsersAsync(int page, int limit)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            using var multi = await connection.QueryMultipleAsync(
                "User_GetAll",
                new { Page = page, Limit = limit },
                commandType: CommandType.StoredProcedure);

            var usersData = await multi.ReadAsync<dynamic>();
            var users = usersData.Select(ur => new UserResponse
            {
                Id = ur.Id,
                Name = ur.Name,
                Email = ur.Email,
                RoleId = ur.RoleId,
                Avatar = ur.Avatar,
                CreatedAt = ur.CreatedAt,
                TotalUser = ur.TotalUser, // Đảm bảo rằng giá trị này được gán đúng cách
                Role = new RoleDto
                {
                    Id = ur.RoleId,
                    Name = ur.RoleName
                }
            }).ToList();

            var firstUser = users.FirstOrDefault();

            var pagedResult = new PagedResult<UserResponse>
            {
                TotalRecords = firstUser?.TotalUser ?? 0,
                Page = page,
                Limit = limit,
                Data = users
            };

            return ResponseObject<PagedResult<UserResponse>>.CreateResponse("Lấy dữ liệu thành công.", pagedResult);
        }


        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            using var multi = await connection.QueryMultipleAsync(
                "User_GetByEmail",
                new { Email = email }, 
                commandType: CommandType.StoredProcedure);

            var user = await multi.ReadSingleOrDefaultAsync<UserDto>() ?? throw new CustomException(StatusCodes.Status404NotFound, "Email chưa đúng.");
            var role = await multi.ReadSingleOrDefaultAsync<RoleDto>();
            user.Role = role ?? throw new CustomException(StatusCodes.Status404NotFound, "Role không tồn tại.");

            return user;
        }
    }

}

