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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ik_word_management.Controllers
{
    [Route("api/Auth/")]
    public class AuthController : Controller
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly IRefreshService _refreshService;

        private readonly JwtIssuerOptions _jwtIssuerOptions;
        private IKWordContext _iKWordContext = null;

        public AuthController(IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> options, IKWordContext iKWordContext, IRefreshService refreshService)
        {
            _jwtFactory = jwtFactory;
            _jwtIssuerOptions = options.Value;
            _iKWordContext = iKWordContext;
            _refreshService = refreshService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody]RequsetLoginInputModel model)
        {
            var user = _iKWordContext.UserAccount.Where(o => o.Name == model.Name && o.Password == model.Password && o.Enable == (int)EnableEnum.Enable).FirstOrDefault();
            if (user == null)
            {
                return BadRequest(new ResponseResultBaseModel()
                {
                    Code = (int)CodeEnum.Fail,
                    Msg = "用户名或密码错误"
                });
            }
            var claimsIdentity = _jwtFactory.GenerateClaimsIdentity(user.Name, user.Id.ToString());
            var token = await _jwtFactory.GenerateEncodedToken(user.Name, claimsIdentity);

            _refreshService.AddRefresh(user.Id, token.ExpiresIn);

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

        public async Task<IActionResult> RefreshToken([FromBody]RequestRefreshTokenInputModel model)
        {
            var result = _iKWordContext.Refresh.Where(o => o.RefreshToken == model.RefreshToken && o.ExpiresIn >= DateTime.Now).FirstOrDefault();
            if (result == null)
            {
                return BadRequest(new ResponseResultBaseModel()
                {
                    Code = (int)CodeEnum.Fail,
                    Msg = "RefreshToken错误"
                });
            }
            if (result.Id == model.AccountID)
            {
                return BadRequest(new ResponseResultBaseModel()
                {
                    Code = (int)CodeEnum.Fail,
                    Msg = "RefreshToken错误"
                });
            }
            var name = _iKWordContext.UserAccount.Where(o => o.Id == model.AccountID).Select(p => p.Name).FirstOrDefault();
            var newRefreshToken = Guid.NewGuid();
            var claimsIdentity = _jwtFactory.GenerateClaimsIdentity(name, model.AccountID.ToString());
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