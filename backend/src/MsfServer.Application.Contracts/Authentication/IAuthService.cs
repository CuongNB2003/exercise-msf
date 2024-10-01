﻿
using MsfServer.Application.Contracts.Authentication.AuthDtos;
using MsfServer.Application.Contracts.Authentication.AuthDtos.InputDtos;
using MsfServer.Application.Contracts.User.UserDtos;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Contracts.Authentication
{
    public interface IAuthService
    {
        Task<ResponseObject<LoginResultDto>> LoginAsync(LoginInputDto input);
        Task<ResponseText> RegisterAsync(RegisterInputDto input);
        Task<ResponseText> LogoutAsync(string userId);
        Task<ResponseText> ChangePasswordAsync(ChangePasswordInputDto input);
        Task<ResponseText> ForgotPasswordAsync(ForgotPasswordInputDto input);
        Task<ResponseText> ResetPasswordAsync(ResetPasswordInputDto input);
        Task<ResponseObject<UserLoginDto>> GetUserAsync(int IdUser);
    }
}
