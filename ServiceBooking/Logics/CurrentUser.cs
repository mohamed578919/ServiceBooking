//using System.Security.Claims;

//namespace ServiceBooking.Logics
//{
//    public class CurrentUser : ICurrentUser
//    {
//        private readonly IHttpContextAccessor _http;
//        public CurrentUser(IHttpContextAccessor httpContextAccessor)
//        {
//            _http = httpContextAccessor;
//        }
//        public string UserId => _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
//        public bool IsInRole(string role) => _http.HttpContext?.User?.IsInRole(role) == true;
//        public string? GetUserId()
//        {
//            return _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
//        }
//    }
//}
