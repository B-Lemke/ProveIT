﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MB.AgilePortfolio.BL;
using MB.AgilePortfolio.MVCUI.ViewModels;

namespace MB.AgilePortfolio.MVCUI.Controllers
{
    public class PortfolioController : Controller
    {
        Portfolio portfolio;
        PortfolioList portfolios;
        
        // GET: Portfolio
        public ActionResult Index()
        {
            portfolios = new PortfolioList();
            portfolios.Load();
            return View(portfolios);
        }

        // GET: Portfolio/Details/5
        public ActionResult Details(Guid id)
        {
            portfolio = new Portfolio();
            portfolio.LoadById(id);
            return View(portfolio);
        }

        // GET: Portfolio/Create
        public ActionResult Create()
        {
            PortfolioUsers pu = new PortfolioUsers()
            {
                Portfolio = new Portfolio(),
                Users = new UserList()
            };
            pu.Users.Load();
            return View(pu);
        }

        // POST: Portfolio/Create
        [HttpPost]
        public ActionResult Create(PortfolioUsers pu)
        {
            try
            {
                // TODO: Add insert logic here
                pu.Portfolio.Insert();
                return RedirectToAction("Index");
            }
            catch { return View(pu); }
        }

        // GET: Portfolio/Edit/5
        public ActionResult Edit(Guid id)
        {
            PortfolioUsers pu = new PortfolioUsers()
            {
                Portfolio = new Portfolio(),
                Users = new UserList()
            };
            pu.Portfolio.LoadById(id);
            pu.Users.Load();
            return View(pu);
        }

        // POST: Portfolio/Edit/5
        [HttpPost]
        public ActionResult Edit(Guid id, PortfolioUsers pu)
        {
            try
            {
                // TODO: Add update logic here
                pu.Portfolio.Update();
                return RedirectToAction("Index");
            }
            catch { return View(pu); }
        }

        // GET: Portfolio/Delete/5
        public ActionResult Delete(Guid id)
        {
            portfolio = new Portfolio();
            portfolio.LoadById(id);
            return View(portfolio);
        }

        // POST: Portfolio/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid id, Portfolio p)
        {
            try
            {
                // TODO: Add delete logic here
                p.Delete();
                return RedirectToAction("Index");
            }
            catch { return View(p); }
        }
    }
}