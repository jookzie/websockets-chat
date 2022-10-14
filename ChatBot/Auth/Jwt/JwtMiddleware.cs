using System.Linq;
using System.Threading.Tasks;
using ChatBot.Services;
using Microsoft.AspNetCore.Http;

namespace ChatBot.Auth;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, UserService userService, JwtUtils jwtUtils)
    {
        //get token, split bearer
        var token = context.Request.Headers["Authentication"]
            .FirstOrDefault()?
            .Split(" ")
            .Last();
        
        //validate token
        var userId = jwtUtils.ValidateToken(token);
        
        //if token is valid, set user id to context
        if(userId is not null)
            context.Items["User"] = userService.GetUserById(userId); 
        
        await _next(context);
    }
}

