using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using HairSalon.Models; 
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HairSalon.Controllers
{
  public class ClientsController : Controller
  {
    private readonly HairSalonContext _db;

    public ClientsController(HairSalonContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Client> model = _db.Clients
                              .Include(client => client.Stylist)
                              .ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      // ViewBag.StylistId = new SelectList(_db.Stylists, "StylistId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Client client)
    {
      // if (client.StylistId == 0)
      // {
      //   return RedirectToAction("Index");
      // }
      _db.Clients.Add(client);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Show(int id)
    {
      Client thisClient = _db.Clients
                              .Include(client => client.JoinEntities)
                              .ThenInclude(join => join.Stylist)
                              .FirstOrDefault(client => client.ClientId == id);
      return View(thisClient);
    }

    public ActionResult AddStylist(int id)
    {
      Client thisClient = _db.Clients.FirstOrDefault(clients => clients.ClientId == id);
      ViewBag.StylistId = new SelectList(_db.Stylists, "StylistId", "Name");
      return View(thisClient);
    }

    [HttpPost]
    public ActionResult AddStylist(Client client, int stylistId)
    {
      #nullable enable
      ClientStylist? joinEntity = _db.ClientStylists.FirstOrDefault(join => (join.ClientId == client.ClientId && join.StylistId == stylistId));
      #nullable disable
      if (joinEntity == null && stylistId != 0)
      {
        _db.ClientStylists.Add(new ClientStylist() { StylistId = stylistId, ClientId = client.ClientId });
      }
      _db.SaveChanges();
      return RedirectToAction("Show", new { id = client.ClientId });
    }


    public ActionResult Edit(int id)
    {
      Client thisClient = _db.Clients.FirstOrDefault(client => client.ClientId == id);
      return View(thisClient);
    }

    [HttpPost]
    public ActionResult Edit(Client client)
    {
      _db.Entry(client).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Client thisClient = _db.Clients.FirstOrDefault(client => client.ClientId == id);
      return View(thisClient);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Client thisClient = _db.Clients.FirstOrDefault(client => client.ClientId == id);
      _db.Clients.Remove(thisClient);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

 	[HttpPost]
		public ActionResult DeleteJoin(int joinId)
		{
			ClientStylist joinEntry = _db.ClientStylists.FirstOrDefault(entry => entry.ClientStylistId == joinId);
			_db.ClientStylists.Remove(joinEntry);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}
	}

}