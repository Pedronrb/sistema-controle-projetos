using sistemadecontrole.Server.Services;

public class JwtAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtService _jwtService;  

    public JwtAuthMiddleware(RequestDelegate next, JwtService jwtService)
    {
        _next = next;
        _jwtService = jwtService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();

            var principal = _jwtService.ValidateToken(token);  
            if (principal != null)
            {
                context.User = principal;
            }
#if DEBUG
            else
            {
                Console.WriteLine("Token inválido.");
            }
#endif
        }

        await _next(context);
    }
}
