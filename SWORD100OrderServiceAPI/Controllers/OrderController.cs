using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;


[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IMapper _mapper;
    private readonly OrderServiceDBContext _db;

    private readonly IOrderNotificationProducer _msgQueue;    


    public OrderController(ILogger<OrderController> logger, OrderServiceDBContext db, IMapper mapper, IOrderNotificationProducer msgQueue)
    {
        _logger = logger;
        _db = db;
        _mapper = mapper;
        _msgQueue = msgQueue;        

        var utcnow = DateTime.UtcNow;
        var localnow = DateTime.Now;
        string hostName = System.Net.Dns.GetHostName();
        string myIP = System.Net.Dns.GetHostEntry(hostName).AddressList[0].ToString();

        Console.WriteLine($"OrderController Constructor now: {localnow.ToString("MM/dd/yyyy H:mm")}, utc now: {utcnow.ToString("MM/dd/yyyy H:mm")}");
        Console.WriteLine($"OrderController Constructor, my IP Address is: {myIP}");

    }

    [HttpGet]
    [Route("test")]
    public async Task<IActionResult> Test1()
    {
        return Ok("hello from OrderController");
    }

    [HttpGet]
    [Route("getmyip")]
    public async Task<ActionResult<String>> GetMyIP()
    {
        var utcnow = DateTime.UtcNow;
        var localnow = DateTime.Now;
        string hostName = System.Net.Dns.GetHostName();
        string myIP = System.Net.Dns.GetHostEntry(hostName).AddressList[0].ToString();

        //Console.WriteLine(Environment.GetEnvironmentVariable("MY_ENV_VARIABLE01")); //coming from launchSettings.json
        //Console.WriteLine(Environment.GetEnvironmentVariable("MY_ENV_VARIABLE02")); // coming from launch.json
        //Console.WriteLine("ASPNETCORE_ENVIRONMENT: " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

        string s = $"now: {localnow.ToString("MM/dd/yyyy H:mm")}, utc now: {utcnow.ToString("MM/dd/yyyy H:mm")}, my IP Address is: {myIP}, my machine name is: {Environment.MachineName}\n";
        s += $"MY_ENV_VARIABLE01: {Environment.GetEnvironmentVariable("MY_ENV_VARIABLE01c")}, MY_ENV_VARIABLE02: {Environment.GetEnvironmentVariable("MY_ENV_VARIABLE02d")}, ASPNETCORE_ENVIRONMENT: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}";

        return s;
    }

    [HttpGet]
    [Route("testmsgqueue")]
    public async Task<ActionResult> TestMsgQueue()
    {
        OrderNotification orderNotification = new OrderNotification()
        {
            UserGuid = new Guid("4f9f399a-2bbd-455f-aa6a-d34c55c7af33"),
            OrderGuid = new Guid("df38059b-86f9-4e26-a638-939056f640fb"),
            Name = "john",
            Email = "john@yahoo.com",
            Message = "We really appreciate you! - from TestMsgQueue()"
        };

        CreateOrderNotification(orderNotification);

        return Ok();
    }


    // for some reason I had to make this private in order to raise a Swagger error
    private void CreateOrderNotification(OrderNotification orderNotification)
    {
        _msgQueue.SendMessage(orderNotification);
    }

    [HttpGet]
    public async Task<ActionResult<List<Order>>> Get()
    {
        try
        {
            List<Order> orders = await _db.Orders.ToListAsync();
            return Ok(new
            {
                Success = true,
                Message = "All Order items returned.",
                orders
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route("withswords")]
    public async Task<ActionResult<List<Order>>> GetAllWithSwords()
    {
        try
        {
            List<Order> orders = await _db.Orders.Include(o => o.Swords).ToListAsync();

            return Ok(new
            {
                Success = true,
                Message = "All Order items returned.",
                orders
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route("{orderGuid}")]
    public async Task<ActionResult<Order>> GetByOrderGuid(Guid orderGuid)
    {
        try
        {
            Order order = await _db.Orders.Include(o => o.Swords).Where(o => o.OrderGuid == orderGuid).FirstOrDefaultAsync();

            if (order == null) return NotFound();

            return Ok(new
            {
                Success = true,
                Message = "One Order item returned.",
                order
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }


    [HttpGet]
    [Route("user/{userGuid}")]
    public async Task<ActionResult<List<Order>>> GetByUserGuid(Guid userGuid)
    {
        try
        {
            List<Order> orders = await _db.Orders.Include(o => o.Swords).Where(o => o.UserGuid == userGuid).ToListAsync();

            return Ok(new
            {
                Success = true,
                Message = "Order items returned.",
                orders

            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

   // [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(OrderDTO orderDTO)
    {
        string userGuid = "E8E369C0-960B-4584-9A81-F9FF9F98DBD6";
        try
        {
            if (String.IsNullOrEmpty(userGuid)) throw new Exception("it was null...");

            Order orderFinal = _mapper.Map<Order>(orderDTO);
            orderFinal.UserGuid = new Guid(userGuid);
            orderFinal.CreatedDate = DateTime.Now;

            _db.Orders.Add(orderFinal);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = "Order created.",
                UserGuid = orderFinal.UserGuid,
                OrderGuid = orderFinal.OrderGuid
            });

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }


    [HttpDelete]
    [Route("{OrderGuid}")]
    public async Task<IActionResult> Delete(Guid orderGuid)
    {

        try
        {
            Order order = await _db.Orders.Include(o => o.Swords).FirstOrDefaultAsync(o => o.OrderGuid == orderGuid);            

            Console.WriteLine("cart GUID: " + order.CartGuid);
            Console.WriteLine("order GUID: " + order.OrderGuid);

            foreach (Sword sword in order.Swords)
            {
                Console.WriteLine(sword.SwordGuid + " " + sword.OrderGuid);
                _db.Swords.Remove(sword);
            }            

            return Ok(new
            {
                Success = true,
                Message = "Order deleted."
            });



        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return StatusCode(500, ex.Message);
        }       


        return StatusCode(500);
    }

}