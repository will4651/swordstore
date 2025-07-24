using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IMapper _mapper;
    private readonly OrderServiceDBContext _db;

    public UserController(ILogger<UserController> logger, OrderServiceDBContext db, IMapper mapper)
    {
        _logger = logger;        
        _db = db;
        _mapper = mapper;
    }

    /*
        [HttpGet]
        public async Task<ActionResult<List<User>>> Get()
        {
            return await _db.Users.ToListAsync<User>();
        }
    */


    [HttpGet]
    public async Task<ActionResult<List<User>>> Get()
    {
        try
        {
            List<User> users = await _db.Users.ToListAsync();

            return Ok(new
            {
                Success = true,
                Message = "all users returned.",
                users
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }


    [HttpGet("withorders")]
    public async Task<ActionResult<List<User>>> GetWithOrders()
    {
        var users = await _db.Users.Include(u => u.Orders).ToListAsync();
        return Ok(users);
    }

    [HttpGet]
    [Route("{userGuid}")]
    public async Task<IActionResult> GetById(Guid userGuid)
    {
        try
        {            
            User? user = await _db.Users.Include(u => u.Orders).Where(u => u.UserGuid == userGuid).FirstOrDefaultAsync();

            return Ok(new
            {
                Success = true,
                Message = "User fetched.",
                user
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserDTO userDTO)
    {
        //Guid userGuid = new Guid("E8E369C0-960B-4584-9A81-F9FF9F98DBD6");
        try
        {            
            User? user2 = await _db.Users.Where(u => u.Email == userDTO.Email).FirstOrDefaultAsync();
            

            if (user2 != null)
            {
                return Ok(new
                {
                    Success = true,
                    Message = "User already exists!",
                    UserGuid = user2.UserGuid,
                    Email = user2.Email
                });
            }

            User userFinal = new User();

            // #1 approach with no Mapper, works
            // not necessary, schema will auto-generate...
            //userFinal.UserGuid = Guid.NewGuid();
            userFinal.Username = userDTO.Username;
            userFinal.Email = userDTO.Email;
            userFinal.Password = userDTO.Password;
            userFinal.CreatedDate = DateTime.Now;
            //userFinal.Orders = null;

            // #2 using Mapper - this blows away the above, which I left uncommented since it works the way it should already
            var userFinal2 = _mapper.Map<User>(userDTO);
            userFinal2.CreatedDate = DateTime.Now;

            //_db.Users.Add(userFinal);
            _db.Users.Add(userFinal2);
            await _db.SaveChangesAsync();

            //return Results.Created($"/{category.Id}", category);
            //var userGuid = await _userDBDapper.Create(user);

            return Ok(new
            {
                Success = true,
                Message = "User created.",
                UserGuid = userFinal2.UserGuid
            });

        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

}

