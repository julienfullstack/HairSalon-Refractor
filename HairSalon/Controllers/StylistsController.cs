using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using HairSalon.Models;

namespace HairSalon.Controllers
{
	public class StylistsController : Controller
	{
		private readonly HairSalonContext _db;

		public StylistsController(HairSalonContext db)
		{
			_db = db;
		}

		public ActionResult Index()
		{
			List<Stylist> model = _db.Stylists.ToList();
			return View(model);
		}

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(Stylist stylist)
		{
			_db.Stylists.Add(stylist);
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

public ActionResult Show(int id)
{
    Stylist thisStylist = _db.Stylists
                            .Include(stylist => stylist.JoinEntities)
														.ThenInclude(join => join.Client)
                            .FirstOrDefault(stylist => stylist.StylistId == id);
    return View(thisStylist);
}

public ActionResult AddClient(int id)
{
	Stylist thisStylist = _db.Stylists.FirstOrDefault(stylists => stylists.StylistId == id);
	ViewBag.ClientId = new SelectList(_db.Clients, "ClientId", "Name");
	return View(thisStylist);
}

[HttpPost] 
public ActionResult AddClient(Stylist stylist, int clientId)
{
	#nullable enable
	ClientStylist? joinEntity = _db.ClientStylists.FirstOrDefault(join => (join.ClientId == clientId && join.StylistId == stylist.StylistId));
	#nullable disable 
	if (joinEntity == null && clientId != 0)
	{
		_db.ClientStylists.Add(new ClientStylist() { ClientId = clientId, StylistId = stylist.StylistId});
		_db.SaveChanges();
	}
	return RedirectToAction("Show", new { id = stylist.StylistId });
}

	public ActionResult Edit(int id)
{
    Stylist thisStylist = _db.Stylists
                            .Include(stylist => stylist.Clients)
                            .FirstOrDefault(stylist => stylist.StylistId == id);
    return View(thisStylist);
}


		[HttpPost]
		public ActionResult Edit(Stylist stylist)
		{
			_db.Entry(stylist).State = EntityState.Modified;
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		public ActionResult Delete(int id)
		{
			Stylist thisStylist = _db.Stylists.FirstOrDefault(stylist => stylist.StylistId == id);
			return View(thisStylist);
		}

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			Stylist thisStylist = _db.Stylists.FirstOrDefault(stylist => stylist.StylistId == id);
			_db.Stylists.Remove(thisStylist);
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

