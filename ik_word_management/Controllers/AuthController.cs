using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ik_word_management.Helper;
using ik_word_management.Models.Domain;
using ik_word_management.Models.DTO.Input;
using ik_word_management.Models.DTO.Output;
using ik_word_management.Models.Enum;
using ik_word_management.Models.JWT;
using ik_word_management.Services.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ik_word_management.Controllers
{
    [Route("api/Auth/")]
    public class AuthController : Controller
    {
        private readonly IJwtFactory _jwtFactory = null;
        private readonly IRefreshService _refreshService = null;
        private readonly IUserAccountService _userAccountServicee = null;

        public AuthController(IJwtFactory jwtFactory, IRefreshService refreshService, IUserAccountService userAccountService)
        {
            _jwtFactory = jwtFactory;
            _refreshService = refreshService;
            _userAccountServicee = userAccountService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody]RequsetLoginInputModel model)
        {
            var user = _userAccountServicee.GetUserAccountByLoginInfo(model.Name, model.Password);
            if (user == null)
            {
                return BadRequest(new ResponseResultBaseModel()
                {
                    Code = (int)CodeEnum.Fail,
                    Msg = "用户名或密码错误"
                });
            }
            var newRefreshToken = Guid.NewGuid();
            var claimsIdentity = _jwtFactory.GenerateClaimsIdentity(user.Name, user.Id.ToString());
            var token = await _jwtFactory.GenerateEncodedToken(user.Name, claimsIdentity, newRefreshToken.ToString());

            var refresh = _refreshService.GetRefreshByUserAccountID(user.Id);

            if (refresh == null)
            {
                _refreshService.AddRefresh(user.Id, token.ExpiresIn, newRefreshToken);
            }
            else
            {
                _refreshService.UpdateRefresh(refresh.Id, newRefreshToken, token.ExpiresIn);
            }

            return new OkObjectResult(new ResponseResultBaseModel()
            {
                Code = (int)CodeEnum.Success,
                Msg = "登录成功",
                Content = new
                {
                    auth_token = token.AuthToken,
                    expires_in = token.ExpiresIn,
                    token_type = token.TokenType,
                    refresh_token = token.RefreshToken
                }
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken([FromBody]RequestRefreshTokenInputModel model)
        {

            var result = _refreshService.GetRefreshByRefreshToken(model.RefreshToken);
            if (result == null)
            {
                return BadRequest(new ResponseResultBaseModel()
                {
                    Code = (int)CodeEnum.Fail,
                    Msg = "RefreshToken错误"
                });
            }

            var name = _userAccountServicee.GetUserAccountByID(result.AccountId.Value)?.Name;
            var newRefreshToken = Guid.NewGuid();
            var claimsIdentity = _jwtFactory.GenerateClaimsIdentity(name, result.AccountId.Value.ToString());
            var token = await _jwtFactory.GenerateEncodedToken(name, claimsIdentity, newRefreshToken.ToString());

            _refreshService.UpdateRefresh(result.Id, newRefreshToken, token.ExpiresIn);

            return new OkObjectResult(new ResponseResultBaseModel()
            {
                Code = (int)CodeEnum.Success,
                Msg = "刷新成功",
                Content = new
                {
                    auth_token = token.AuthToken,
                    expires_in = token.ExpiresIn,
                    token_type = token.TokenType,
                    refresh_token = token.RefreshToken
                }
            });
        }

    }
}