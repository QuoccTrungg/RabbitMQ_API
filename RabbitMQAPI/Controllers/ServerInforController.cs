
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RabbitMQAPI.Data;
using RabbitMQAPI.Models;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RabbitMQAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerInforController : ControllerBase
    {
        private readonly MyDBContext myDBContext;
        public ServerInforController(MyDBContext myDBContext) {
            this.myDBContext = myDBContext;
        }
        // GET: api/<ServerInforController>
        [HttpGet]
        public IActionResult GetAll()
        {
            var ls = myDBContext.serverInfors.ToList();
            return Ok(ls);
        }
        /* [HttpGet]
         public IEnumerable<ServerInfor> Get()
         {
             DataTable dt = DataProvider.Instance.ExecuteQuery("select * from ServerInfor");
             return dt.AsEnumerable().Select(row => new ServerInfor
             {
                 id = (int)row[0],
                 CPU = (double)row[1],
                 RAM = (double)row[2],
                 D = (double)row[3],
                Time = (DateTime)row[4],
             });
         }*/
        [HttpGet("sendmail/{svname}")]
        public IActionResult sendmail(string svname)
        {
            var sv = myDBContext.serverInfors.Where(s => s.Name.Equals(svname)).OrderByDescending(t=>t.Time).Take(20).ToList();

            if (sv != null)
            {
                
                string from, to, pass, content="";
                from = "trungngo2901@gmail.com";
                //from = "n19dccn215@student.ptithcm.edu.vn";
                pass = "grqoffbudwccltes";
                to = "trungngo15t@gmail.com";
                foreach (ServerInfor item in sv)
                {
                    content += " ServerName " + item.Name + "\t\t\t" + " CPU: " + item.CPU + "\t\t\t" + " RAM: " + item.RAM + "\t\t\t"
                       + "  Disk: " + item.D + "\t\t\t" + "  Time: " + item.Time.ToString()+ "\t\t\t" + "  IP: " + item.IP + "\t\t\t" + "  Region: " + item.Region+ "\n";
                    if (item.CPU > 50) content += "Warning " + " Server " + item.Name + "!!! CPU : " + item.CPU + "\n";
                    if (item.RAM > 50) content += "Warning " + " Server " + item.Name + "!!!  RAM : " + item.RAM + "\n";
                    if (item.D > 50) content += "Warning " + " Server " + item.Name + "!!!  Disk : " + item.D + "\n";
                }
                //content = sv.ToString();
                MailMessage mail = new MailMessage();
                mail.To.Add(to);
                mail.From = new MailAddress(from);
                mail.Subject = " Information Server " + svname + " Time: " + DateTime.Now.ToString();     
                mail.Body = content;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Port = 587;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new NetworkCredential(from, pass); 
                try {
                    smtp.Send(mail);
                    return Ok(sv);
                } 
                catch(Exception e) {
                      Console.WriteLine(e.Message.ToString());
                    return Ok(e.Message.ToString());
                }
               
            }
            else return NotFound();
        }
        // GET api/<ServerInforController>/5

        [HttpGet("servername")]
        public IActionResult GetServerName() {
            var ls = myDBContext.serverInfors.ToList();
            var lsName = new List<string>();
            foreach (ServerInfor item in ls)
            {
                if (!lsName.Contains(item.Name))
                    lsName.Add(item.Name);
            }
            lsName.Sort();
            return Ok(lsName);

        }
        [HttpGet("{id}")]
        
        public IActionResult GetbyID(int id)
        {
            var sv = myDBContext.serverInfors.SingleOrDefault(s=> s.id==id);
            if (sv != null) return Ok(sv);
            else return NotFound();
        }
        
        
        [HttpGet("servername/{svname}")]

        public IActionResult GetbyName(string svname)
        {
            var sv = myDBContext.serverInfors.Where( s=> s.Name.Equals(svname)).OrderByDescending(x => x.Time).Take(10).ToList();
            if (sv != null) return Ok(sv);
            else return NotFound();
        }

        [HttpGet("servername1/{svname}")]

        public IActionResult GetbyName1(string svname)
        {
            var sv = myDBContext.serverInfors.Where(s => s.Name.Equals(svname)).OrderByDescending(x => x.Time).Take(6).ToList();
            sv = sv.OrderBy(v => v.Time).ToList();
            if (sv != null) return Ok(sv);
            else return NotFound();
        }
        // POST api/<ServerInforController>
        /*[HttpPost]
        public void Post([FromBody] ServerInfor value)
        {
        }*/
        [HttpPost]
        
        public IActionResult CreateServer( [FromBody]ServerInforModel value)
        {
            try {
                var sv = new ServerInfor {
                    Name = value.Name,
                    CPU = value.CPU,
                    RAM = value.RAM,
                    D = value.D,
                    Time = DateTime.Now,
                    IP = value.IP.ToString(),
                    Region = value.Region.ToString(),
                };
                myDBContext.Add(sv);
                myDBContext.SaveChanges();
                return Ok(sv);    
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("addserver")]
        public IActionResult AddServer([FromBody] ServerInforModel value)
        {
            try
            {
                var sv = new ServerInfor
                {
                    Name = value.Name,
                    CPU = value.CPU,
                    RAM = value.RAM,
                    D = value.D,
                    Time = DateTime.Now,
                    IP = value.IP.ToString(),
                    Region = value.Region.ToString(),
                };
                myDBContext.Add(sv);
                myDBContext.SaveChanges();
                return Ok(sv);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<ServerInforController>/5
        [HttpPut("{id}")]
        
        public IActionResult UpdatebyID(int id, ServerInforModel value)
        {
            var sv = myDBContext.serverInfors.SingleOrDefault(s => s.id == id);
            if (sv != null) { 
                sv.Name = value.Name;
                sv.CPU = value.CPU;
                sv.RAM = value.RAM;
                sv.D = value.D;

                myDBContext.SaveChanges();
                return Ok(sv); 
            }
            else return NotFound();
        }

        // DELETE api/<ServerInforController>/5
        [HttpDelete("{id}")]
        
        public IActionResult DeletebyID(int id)
        {
            var sv = myDBContext.serverInfors.SingleOrDefault(s => s.id == id);
            if (sv != null) 
            { 
                myDBContext.Remove(sv);
                myDBContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK);
             }
            else return NotFound();    
        }
    }
}
