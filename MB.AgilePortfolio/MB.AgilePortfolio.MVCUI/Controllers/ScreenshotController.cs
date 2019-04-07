﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MB.AgilePortfolio.BL;
using MB.AgilePortfolio.MVCUI.ViewModels;
using MB.AgilePortfolio.MVCUI.Models;
using System.IO;

namespace MB.AgilePortfolio.MVCUI.Controllers
{
    public class ScreenshotController : Controller
    {
        Screenshot screenshot;
        ScreenshotList screenshots;

        // GET: Screenshot
        public ActionResult Index()
        {
            screenshots = new ScreenshotList();
            screenshots.Load();
            return View(screenshots);
        }

        // GET: Screenshot/Details/5
        public ActionResult Details(Guid id)
        {
            ScreenshotProjects sp = new ScreenshotProjects()
            {
                Screenshot = new Screenshot(),
                Projects = new ProjectList()
            };
            sp.Projects.Load();
            sp.Screenshot.LoadById(id);

            return View(sp);
        }

        // GET: Screenshot/Create
        public ActionResult Create()
        {
            ScreenshotProjects sp = new ScreenshotProjects()
            {
                Screenshot = new Screenshot(),
                Projects = new ProjectList()
            };
            sp.Projects.Load();
            return View(sp);
        }

        // POST: Screenshot/Create
        [HttpPost]
        public ActionResult Create(ScreenshotProjects sp)
        {
            try
            {
                // TODO: Add insert logic here
                sp.Screenshot.Insert();
                return RedirectToAction("Index");
            }
            catch { return View(sp); }
        }

        // GET: Screenshot/Edit/5
        public ActionResult Edit(Guid id)
        {
            ScreenshotProjects sp = new ScreenshotProjects()
            {
                Screenshot = new Screenshot(),
                Projects = new ProjectList()
            };
            sp.Projects.Load();
            sp.Screenshot.LoadById(id);
            return View(sp);
        }

        // POST: Screenshot/Edit/5
        [HttpPost]
        public ActionResult Edit(Guid id, ScreenshotProjects sp)
        {
            try
            {
                // TODO: Add update logic here
                sp.Screenshot.Update();
                return RedirectToAction("Index");
            }
            catch { return View(sp); }
        }

        // GET: Screenshot/Delete/5
        public ActionResult Delete(Guid id)
        {
            ScreenshotProjects sp = new ScreenshotProjects()
            {
                Screenshot = new Screenshot(),
                Projects = new ProjectList()
            };
            sp.Projects.Load();
            sp.Screenshot.LoadById(id);

            return View(sp);
        }

        // POST: Screenshot/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid id, Screenshot s)
        {
            try
            {
                Screenshot ss = new Screenshot();
                ss.LoadById(s.Id);
                Project p = new Project();
                p.LoadById(s.ProjectId);
                User userin = System.Web.HttpContext.Current.Session["user"] as User;
                var fullPath = Server.MapPath("~/" + p.Image);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    ViewBag.deleteSuccess = "true";
                }
                s.Delete();
                return RedirectToAction("UploadProjectSliderImage", new { id = ss.ProjectId });
            }
            catch { return View(s); }
        }

        // GET: Admin
        public ActionResult UploadProjectSliderImage(Guid? id)
        {
            if (Authenticate.IsAuthenticated())
            {
                Guid ID = id.GetValueOrDefault();
                if (ID == Guid.Empty)
                {
                    if (Authenticate.IsAuthenticated())
                    {
                        return RedirectToAction("EditProjects", "UserProfile", new { returnurl = HttpContext.Request.Url });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
                    }
                }
                ScreenshotProjects sp = new ScreenshotProjects()
                {
                    Screenshot = new Screenshot(),
                    ScreenshotList = new ScreenshotList()
                };
                sp.ScreenshotList.LoadbyProjectID(ID);
                return View( sp);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }


        [HttpPost]
        public ActionResult UploadProjectSliderImage(Guid id, ScreenshotProjects sp)
        {

            HttpPostedFileBase fileupload= sp.Fileupload;
            if (fileupload != null)
            {
                User userin = System.Web.HttpContext.Current.Session["user"] as User;
                string username = userin.Username;
                string strUserID = userin.Id.ToString();
                string fileName = Path.GetFileName(fileupload.FileName);
                string savepath = "";
                if (Directory.Exists("~/Assets/Images/ScreenShots"))
                {
                    savepath = "Assets/Images/ScreenShots/" + username;
                }
                else
                {
                        Directory.CreateDirectory(Server.MapPath("~/Assets/Images/ScreenShots/" + username));
                        savepath = "Assets/Images/ScreenShots/" + username;
                }

                if (Directory.Exists("~/Assets/Images/ScreenShots/" + username +  "/" + sp.Project.Name))
                {
                    savepath = "Assets/Images/ScreenShots/" + username + "/" + sp.Project.Name;
                }
                else
                {
                        Directory.CreateDirectory(Server.MapPath("~/Assets/Images/ScreenShots/" + username + "/" + sp.Project.Name));
                        savepath = "Assets/Images/ScreenShots/" + username + "/" + sp.Project.Name;
                }
                
                
                Screenshot ss = new Screenshot();

                ss.ProjectId = id;


                // ss.FilePath = Server.MapPath(~Assets/Images/ScreenShots/" + username + "/" + fileName);


                fileupload.SaveAs(Server.MapPath("~/" + savepath + "/" + fileName));
                ss.FilePath = savepath +  "/" + fileName;
                ss.Insert();
            }
            return RedirectToAction("UploadProjectSliderImage");
        }

        // GET: UserProfile/EditProject
        public ActionResult EditProject(Guid? id)
        {
            Guid ID = id.GetValueOrDefault();
            if (ID == Guid.Empty)
            {
                if (Authenticate.IsAuthenticated())
                {

                    return RedirectToAction("EditProjects", "UserProfile", new { returnurl = HttpContext.Request.Url });
                }
                else
                {
                    return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
                }
            }
            else
            {
                ScreenshotProjects up = new ScreenshotProjects()
                {
                    Project = new Project(),
                    Privacies = new PrivacyList(),
                    User = new User(),
                    Statuses = new StatusList(),
                    ScreenshotList = new ScreenshotList()
                };

                if (Authenticate.IsAuthenticated())
                {
                    up.Project.LoadById(ID);
                    Project project = new Project();
                    project.LoadById(up.Project.Id);
                    up.DateCreated = project.DateCreated;
                    up.LastUpdated = project.LastUpdated;
                    up.Privacies.Load();
                    User userin = System.Web.HttpContext.Current.Session["user"] as User;
                    up.User.LoadById(userin.Id);
                    up.Statuses.Load();
                    up.Project.LoadById(ID);
                    up.ScreenshotList.LoadbyProjectID(ID);
                    return View(up);
                }
                else
                {
                    return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
                }
            }
        }

        // POST: UserProfile/EditProject
        [HttpPost]
        public ActionResult EditProject(Guid id, ScreenshotProjects ppus)
        {
            if (Authenticate.IsAuthenticated())
            {
                try
                {
                    PrivacyList plist = new PrivacyList();
                    plist.Load();
                    ppus.Privacies = plist;
                    StatusList slist = new StatusList();
                    slist.Load();
                    ppus.Statuses = slist;
                    ScreenshotList screenshots = new ScreenshotList();
                    screenshots.LoadbyProjectID(id);
                    
                    HttpPostedFileBase fileupload = ppus.Fileupload;
                    User userin = System.Web.HttpContext.Current.Session["user"] as User;
                    ProjectList Projects = new ProjectList();
                    Projects.LoadbyUser(userin);
                    string username = userin.Username;
                    string strUserID = userin.Id.ToString();
                    ppus.ScreenshotList = screenshots;
                    string fileName = Path.GetFileName(fileupload.FileName);
                    string savepath = "";
                    ppus.ScreenshotList = screenshots;
                    if (ppus.Project.Name == null)
                    {
                        ModelState.AddModelError(string.Empty, "Project requires a name!");
                    }
                    else
                    {
                        foreach (Project p in Projects)
                        {
                            if (ppus.Project.Name == p.Name)
                            {
                                if (ppus.Project.Id != p.Id)
                                {
                                    ModelState.AddModelError(string.Empty, "Another project already exists with this name!");
                                }

                            }
                        }

                        if (ppus.DateCreated == null)
                        {
                            ModelState.AddModelError(string.Empty, "Date Created required!");
                        }
                        else if (ppus.LastUpdated == null)
                        {
                            ppus.LastUpdated = ppus.DateCreated;
                        }
                    }

                    if (ppus.Project.Image == string.Empty)
                    {
                        ppus.Project.Image = "Assets/Images/UserProfiles/Default.png";
                    }
                    else
                    {
                        var fullPath = Server.MapPath("~/" + ppus.Project.Image);

                        if (System.IO.File.Exists(fullPath))
                        {
                            
                        }
                        else
                        {
                            ppus.Project.Image = "Assets/Images/UserProfiles/Default.png";
                        }

                        if (Directory.Exists("~/Images/ScreenShots"))
                        {
                            savepath = "Assets/Images/ScreenShots";
                        }
                        else
                        {
                            Directory.CreateDirectory(Server.MapPath("~/Assets/Images/ScreenShots"));
                            savepath = "Assets/Images/ScreenShots";
                        }

                        if (Directory.Exists("~/Assets/Images/ScreenShots/" + username))
                        {
                            savepath = "Assets/Images/ScreenShots/" + username;
                        }
                        else
                        {
                            Directory.CreateDirectory(Server.MapPath("~/Assets/Images/ScreenShots/" + username));
                            savepath = "Assets/Images/ScreenShots/" + username;
                        }

                        if (Directory.Exists("~/Assets/Images/ScreenShots/" + username + "/" + ppus.Project.Name))
                        {
                            savepath = "Assets/Images/ScreenShots/" + username + "/" + ppus.Project.Name;
                        }
                        else
                        {
                            Directory.CreateDirectory(Server.MapPath("~/Assets/Images/ScreenShots/" + username + "/" + ppus.Project.Name));
                            savepath = "Assets/Images/ScreenShots/" + username + "/" + ppus.Project.Name;
                        }

                        fullPath = Server.MapPath("~/Assets/Images/ScreenShots/" + username + "/" + ppus.Project.Name + "/" + fileName);

                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                            ViewBag.deleteSuccess = "true";
                        }

                        fileupload.SaveAs(Server.MapPath("~/" + savepath + "/" + fileName));
                        ppus.Project.Image = savepath + "/" + fileName;
                    }

                    if (!ModelState.IsValid)
                    {
                        ppus.Privacies = new PrivacyList();
                        ppus.Statuses = new StatusList();
                        ppus.User = new User();
                        ppus.User.LoadById(userin.Id);
                        ppus.Privacies.Load();
                        ppus.Statuses.Load();
                        return View(ppus);
                    }
                    ppus.Project.DateCreated = ppus.DateCreated;
                    ppus.Project.LastUpdated = ppus.LastUpdated;
                    ppus.Project.Update();
                    return RedirectToAction("EditProjects", "UserProfile");
                }
                catch { return View(ppus); }
            }
            else
            {
                return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
            }
        }
    }
}
