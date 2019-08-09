using mvcbooks.Data;
using mvcbooks.Models;
using System.Linq;
using System.Web.Http;

namespace mvcbooks.Controllers.Api
{
    public class ClientController : ApiController
    {
        private readonly LibraryEntities _context;

        public ClientController()
        {
            _context = new LibraryEntities();
        }


        [System.Web.Http.HttpGet]
        public IHttpActionResult FillClientData(string personalId)
        {
            var client = _context.Clients.FirstOrDefault(x => x.Personal_Id == personalId) ?? new Client();
            if (client == null)
                return NotFound();
            return Json(new
            {
                personalId = !string.IsNullOrEmpty(client.Personal_Id) ? client.Personal_Id : "",
                firstname = !string.IsNullOrEmpty(client.First_Name) ? client.First_Name : "",
                lastname = !string.IsNullOrEmpty(client.Last_Name) ? client.Last_Name : "",
                email = !string.IsNullOrEmpty(client.Email) ? client.Email : "",
                phoneNumber = !string.IsNullOrEmpty(client.Phone_Number) ? client.Phone_Number : "",
                address = !string.IsNullOrEmpty(client.Address) ? client.Address : ""
            });
        }
        [System.Web.Http.HttpPost]
        public IHttpActionResult SaveClient(ClientViewModel clientViewModel)
        {
            Client client = new Client();
            var json = Created("fs", new
            {
                personalId = !string.IsNullOrEmpty(client.Personal_Id) ? client.Personal_Id : "",
                firstname = !string.IsNullOrEmpty(client.First_Name) ? client.First_Name : "",
                lastname = !string.IsNullOrEmpty(client.Last_Name) ? client.Last_Name : "",
                email = !string.IsNullOrEmpty(client.Email) ? client.Email : "",
                phoneNumber = !string.IsNullOrEmpty(client.Phone_Number) ? client.Phone_Number : "",
                address = !string.IsNullOrEmpty(client.Address) ? client.Address : ""
            });
            if (!ModelState.IsValid)
                return BadRequest();
            if (!_context.Clients.Any(p => p.Personal_Id == clientViewModel.PersonalId))
            {
                client.First_Name = clientViewModel.FirstName;
                client.Last_Name = clientViewModel.LastName;
                client.Personal_Id = clientViewModel.PersonalId;
                client.Email = clientViewModel.Email;
                client.Address = clientViewModel.Address;
                client.Phone_Number = clientViewModel.PhoneNumber;
                _context.Clients.Add(client);
                _context.SaveChanges();
                return json;
            }
            else
            {
                var clientInDb = _context.Clients.SingleOrDefault(c => c.Personal_Id == clientViewModel.PersonalId);

                clientInDb.First_Name = clientViewModel.FirstName;
                clientInDb.Last_Name = clientViewModel.LastName;
                clientInDb.Personal_Id = clientViewModel.PersonalId;
                clientInDb.Email = clientViewModel.Email;
                clientInDb.Address = clientViewModel.Address;
                clientInDb.Phone_Number = clientViewModel.PhoneNumber;
                _context.SaveChanges();
                return json;
            }
        }
    }
}
