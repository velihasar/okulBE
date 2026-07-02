using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Extensions
{
	public static class UserInfoExtensions
	{
		private static readonly IHttpContextAccessor HttpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
		public static int GetUserId()
		{
			var httpContext = HttpContextAccessor?.HttpContext;
			if (httpContext == null || httpContext.User == null || !httpContext.User.Identity.IsAuthenticated)
			{
				throw new UnauthorizedAccessException("Kullanıcı kimlik doğrulaması yapılmamış");
			}

			var result = httpContext.User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier", StringComparison.OrdinalIgnoreCase))?.Value;
			if (string.IsNullOrEmpty(result))
			{
				throw new UnauthorizedAccessException("Kullanıcı kimliği token'da bulunamadı");
			}

			if (!int.TryParse(result, out int userId) || userId <= 0)
			{
				throw new UnauthorizedAccessException($"Geçersiz kullanıcı kimliği: {result}");
			}

			return userId;
		}

		public static int GetUserIdOrZero()
		{
			var httpContext = HttpContextAccessor?.HttpContext;
			if (httpContext == null || httpContext.User == null || !httpContext.User.Identity.IsAuthenticated)
			{
				return 0;
			}

			var result = httpContext.User.Claims.FirstOrDefault(x => x.Type.EndsWith("nameidentifier", StringComparison.OrdinalIgnoreCase))?.Value;
			if (string.IsNullOrEmpty(result))
			{
				return 0;
			}

			if (!int.TryParse(result, out int userId) || userId <= 0)
			{
				return 0;
			}

			return userId;
		}
	}
}
