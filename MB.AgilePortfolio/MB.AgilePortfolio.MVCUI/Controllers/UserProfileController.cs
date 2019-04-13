﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using MB.AgilePortfolio.BL;
using MB.AgilePortfolio.MVCUI.Models;
using MB.AgilePortfolio.MVCUI.ViewModels;
using System.Net;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MB.AgilePortfolio.MVCUI.Controllers
{
    public class UserProfileController : Controller
    {
        public string Email;
        public string FirstName;
        public string LastName;
        public string FullName;
        public string ProfileImage;
        public string Username;
        public string UserTypesDescription;
        public Guid UserTypesID;
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LastUpdated { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }


        // GET: UserProfile
        public ActionResult Index()
        {
            UserProfile up = new UserProfile
            {
                Projects = new ProjectList(),
                Portfolios = new PortfolioList(),
                User = new User()
            };

            if (Authenticate.IsAuthenticated())
            {
                User userin = System.Web.HttpContext.Current.Session["user"] as User;
                up.User.LoadById(userin.Id);
                up.Projects.LoadbyUser(up.User);
                up.Portfolios.LoadbyUser(up.User);

                // REDIRECT TO PROJECT EDIT PROFILE GET
                return View(up);

            }
            else
            {
                return RedirectToAction("Index", "Login", new
                {
                    returnurl = HttpContext.Request.Url
                });
            }

        }

        public ActionResult EmployerIndex()
        {
            UserProfile up = new UserProfile
            {
                Projects = new ProjectList(),
                Portfolios = new PortfolioList(),
                User = new User()
            };

            if (Authenticate.IsAuthenticated())
            {
                User userin = System.Web.HttpContext.Current.Session["user"] as User;
                up.User.LoadById(userin.Id);
                up.Projects.LoadbyUser(up.User);
                up.Portfolios.LoadbyUser(up.User);

                // REDIRECT TO PROJECT EDIT PROFILE GET
                return View(up);

            }
            else
            {
                return RedirectToAction("Index", "Login", new
                {
                    returnurl = HttpContext.Request.Url
                });
            }

        }

        //Public facing profile page
        public ActionResult PublicProfile(string username)
        {
            UserProfile up = new UserProfile
            {
                Projects = new ProjectList(),
                Portfolios = new PortfolioList(),
                User = new User()
            };

            Guid idOfUser = up.User.CheckIfUsernameExists(username);

            if (idOfUser != Guid.Empty)
            {
                up.User.LoadById(idOfUser);
            }
            //If the username doesn't exist, it's sent into the view with an invalid user with an empty guid and it will trigger a profile not found message.

            up.Projects.LoadbyUser(up.User);
            up.Portfolios.LoadbyUser(up.User);


            return View(up);
        }

        public ActionResult EditProjects()
        {
            UserProfile up = new UserProfile()
            {
                Projects = new ProjectList(),
                User = new User()
            };
            if (Authenticate.IsAuthenticated())
            {
                User userin = System.Web.HttpContext.Current.Session["user"] as User;
                up.User.LoadById(userin.Id);
                up.Projects.LoadbyUser(up.User);

                // REDIRECT TO PROJECT EDIT PAGE
                return View(up);
                //return RedirectToAction("EditProject", "Screenshot");
            }
            else
            {
                return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
            }
        }

        [HttpPost]
        public ActionResult EditProjects(Guid id, UserProfile ppus)
        {
            ScreenshotProjects up = new ScreenshotProjects()
            {
                Project = new Project(),
                Privacy = new Privacy(),
                ScreenshotList = new ScreenshotList(),
                User = new User(),
                Status = new Status()
            };
            if (Authenticate.IsAuthenticated())
            {
                User userin = System.Web.HttpContext.Current.Session["user"] as User;
                up.User.LoadById(userin.Id);
                up.Project.LoadById(id);
                up.ScreenshotList.LoadbyProjectID(id);

                // REDIRECT TO PROJECT EDIT PAGE
                //return View(up);
                return RedirectToAction("EditProject", "Screenshot", new { id = id });
            }
            else
            {
                return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
            }
        }
        // GET: UserProfile/PublicProject
        public ActionResult PublicProject(Guid? id)
        {
            Guid ID = id.GetValueOrDefault();
            ScreenshotProjects sp = new ScreenshotProjects()
            {
                Project = new Project(),
                Privacy = new Privacy(),
                ScreenshotList = new ScreenshotList(),
                User = new User(),
                Status = new Status()
            };


            sp.Project.LoadById(ID);
            sp.User.LoadById(sp.Project.UserId); 
            
            sp.ScreenshotList.LoadbyProjectID(ID);

            return View(sp);


        }
        

        // GET: UserProfile/DeleteProject
        public ActionResult DeleteProject(Guid? id)
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
                UserProfile up = new UserProfile()
                {
                    Project = new Project(),
                    Privacies = new PrivacyList(),
                    User = new User(),
                    Statuses = new StatusList()
                };

                if (Authenticate.IsAuthenticated())
                {
                    up.Project.LoadById(ID);
                    up.Privacies.Load();
                    User userin = System.Web.HttpContext.Current.Session["user"] as User;
                    up.User.LoadById(userin.Id);
                    up.Statuses.Load();
                    return View(up);
                }
                else
                {
                    return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
                }
            }
        }

        // POST: UserProfile/DeleteProject
        [HttpPost]
        public ActionResult DeleteProject(Guid id, UserProfile up)
        {
            if (Authenticate.IsAuthenticated())
            {
                try
                {
                    User userin = System.Web.HttpContext.Current.Session["user"] as User;
                    Project Project = new Project();
                    Project.LoadById(id);
                    up.Project = Project;
                    UploadedImage ui = new UploadedImage();

                    // THIS DOESNT WORK LOCALLY IF THE DIRECTORY WAS MADE BEFORE IT WAS PUSHED
                    ui.DeleteProjectUploadFolder(userin.Username, up.Project.Name);
                    up.Project.Delete();
                    return RedirectToAction("ProjectDeleted");
                }
                catch { return View(up); }
            }
            else
            {
                return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
            }
        }

        //GET: UserProfile/EditPortfolios
        public ActionResult EditPortfolios()
        {
            UserProfile up = new UserProfile()
            {
                Portfolios = new PortfolioList(),
                Privacies = new PrivacyList(),
                User = new User()
            };

            if (Authenticate.IsAuthenticated())
            {
                User userin = System.Web.HttpContext.Current.Session["user"] as User;
                up.User.LoadById(userin.Id);
                up.Portfolios.LoadbyUser(up.User);
                return View(up);
            }
            else
            {
                return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
            }
        }

        public ActionResult ProfileNotFound()
        {
            return View();
        }

        // GET: UserProfile/PublicPortfolio
        public ActionResult PublicPortfolio(Guid? id)
        {
            Guid ID = id.GetValueOrDefault();
            UserProfile up = new UserProfile()
            {
                Portfolio = new Portfolio(),
                Portfolios = new PortfolioList(),
                Projects = new ProjectList(),
                Privacies = new PrivacyList(),
                User = new User()
            };
            if (ID != Guid.Empty)
            {
                up.Portfolio.LoadById(ID);
                up.User.LoadById(up.Portfolio.UserId);
                up.Projects.LoadbyPortfolioID(ID);
            }
            else
            {

            }
            return View(up);
        }

        // POST: UserProfile/PublicPortfolio
        [HttpPost]
        public ActionResult PublicPortfolio(Guid id, UserProfile up)
        {

            try
            {
                return View(up);


            }
            catch { return View(up); }
        }

        // GET: Edit User Portfolio Redirect (UserProfile/EditPortfolio)
        public ActionResult EditPortfolio(Guid? id)
        {
            Guid ID = id.GetValueOrDefault();
            if (ID == Guid.Empty)
            {
                if (Authenticate.IsAuthenticated())
                {

                    return RedirectToAction("EditPortfolios", "UserProfile", new { returnurl = HttpContext.Request.Url });
                }
                else
                {
                    return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
                }
            }
            else
            {
                UserProfile up = new UserProfile()
                {
                    Portfolio = new Portfolio(),
                    Privacies = new PrivacyList(),
                    Projects = new ProjectList(),
                    User = new User()
                };

                if (Authenticate.IsAuthenticated())
                {

                    up.Portfolio.LoadById(ID);
                    Portfolio portfolio = new Portfolio();
                    portfolio.LoadById(up.Portfolio.Id);
                    up.Privacies.Load();
                    User userin = System.Web.HttpContext.Current.Session["user"] as User;
                    up.Projects.LoadbyPortfolioID(up.Portfolio.Id);
                    foreach (Project p in up.Projects)
                    {
                        if (p.Image == string.Empty)
                        {
                            p.Image = "Images/UserProfiles/Default.png";
                        }
                    }
                    up.User.LoadById(userin.Id);
                    return View(up);
                }
                else
                {
                    return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
                }
            }
        }

        // POST: Edit User Profile Redirect (UserProfile/EditPortfolio)
        [HttpPost]
        public ActionResult EditPortfolio(Guid id, UserProfile up)
        {
            if (Authenticate.IsAuthenticated())
            {
                try
                {
                    User userin = System.Web.HttpContext.Current.Session["user"] as User;
                    PortfolioList Portfolios = new PortfolioList();
                    Portfolios.LoadbyUser(userin);
                    up.Portfolios = Portfolios;
                    ProjectList projects = new ProjectList();
                    projects.LoadbyPortfolioID(id);
                    PrivacyList privacies = new PrivacyList();
                    privacies.Load();
                    up.Privacies = privacies;
                    up.Projects = projects;
                    string username = userin.Username;

                    if (up.Portfolio.Name == null)
                    {
                        ModelState.AddModelError(string.Empty, "Portfolio requires a name!");
                    }
                    else
                    {
                        foreach (Portfolio p in Portfolios)
                        {
                            if (up.Portfolio.Name == p.Name)
                            {
                                if (up.Portfolio.Id != p.Id)
                                {
                                    ModelState.AddModelError(string.Empty, "Another portfolio already exists with this name!");
                                }
                            }
                        }
                    }
                    UploadedImage ui = new UploadedImage
                    {
                        FilePath = up.Portfolio.PortfolioImage,
                        Fileupload = up.Fileupload,
                        UserName = username,
                        ObjectType = "Portfolio",
                        ObjectName = up.Portfolio.Name
                    };

                    string fp = ui.Upload();

                    // fp will return null if no upload file was choosen else use upload file to save to database
                    if (fp != null)
                    {
                        up.Portfolio.PortfolioImage = fp;
                    }
                    else
                    {
                        // I honestly don't know when this would happen but just in case
                        ModelState.AddModelError(string.Empty, "Portfolio Image could not found");
                    }

                    if (!ModelState.IsValid)
                    {
                        up.Privacies = new PrivacyList();
                        up.User = new User();
                        up.User.LoadById(userin.Id);
                        up.Privacies.Load();
                        up.Projects.LoadbyPortfolioID(up.Portfolio.Id);
                        return View(up);
                    }
                    up.Portfolio.Update();
                    return RedirectToAction("EditPortfolios");
                }
                catch { return View(up); }
            }
            else
            {
                return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
            }
        }

        // GET: UserProfile/DeletePortfolio
        public ActionResult DeletePortfolio(Guid? id)
        {
            Guid ID = id.GetValueOrDefault();
            if (ID == Guid.Empty)
            {
                if (Authenticate.IsAuthenticated())
                {

                    return RedirectToAction("EditPortfolios", "UserProfile", new { returnurl = HttpContext.Request.Url });
                }
                else
                {
                    return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
                }
            }
            else
            {
                UserProfile up = new UserProfile()
                {
                    Portfolio = new Portfolio(),
                    Privacies = new PrivacyList(),
                    User = new User()
                };

                if (Authenticate.IsAuthenticated())
                {
                    up.Portfolio.LoadById(ID);
                    up.Privacies.Load();
                    User userin = System.Web.HttpContext.Current.Session["user"] as User;
                    up.User.LoadById(userin.Id);
                    return View(up);
                }
                else
                {
                    return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
                }
            }
        }

        // POST: UserProfile/DeletePortfolio
        [HttpPost]
        public ActionResult DeletePortfolio(Guid id, UserProfile up)
        {
            if (Authenticate.IsAuthenticated())
            {
                try
                {
                    User userin = System.Web.HttpContext.Current.Session["user"] as User;
                    Portfolio Portfolio = new Portfolio();
                    Portfolio.LoadById(id);
                    up.Portfolio = Portfolio;
                    UploadedImage ui = new UploadedImage();

                    // THIS DOESNT WORK LOCALLY IF THE DIRECTORY WAS MADE BEFORE IT WAS PUSHED
                    ui.DeleteProjectUploadFolder(userin.Username, up.Portfolio.Name);
                    up.Portfolio.Delete();
                    return RedirectToAction("PortfolioDeleted");
                }
                catch { return View(up); } //NEEDS A WAY TO SHOW ERROR MESSAGE RETURN OF WRONG OLD PASSWORD ENTERED
            }
            else
            {
                return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
            }
        }

        public ActionResult PortfolioDeleted(UserProfile up)
        {
            if (Authenticate.IsAuthenticated())
            {
                try
                {
                    return View(up);
                }
                catch { return View(up); } //NEEDS A WAY TO SHOW ERROR MESSAGE RETURN OF WRONG OLD PASSWORD ENTERED
            }
            else
            {
                return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
            }
        }

        public ActionResult ProjectDeleted(UserProfile up)
        {
            if (Authenticate.IsAuthenticated())
            {
                try
                {
                    return View(up);
                }
                catch { return View(up); } //NEEDS A WAY TO SHOW ERROR MESSAGE RETURN OF WRONG OLD PASSWORD ENTERED
            }
            else
            {
                return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
            }
        }

        // GET: UserProfile/EditProfilePassword
        public ActionResult EditProfilePassword(Guid? id)
        {
            Guid ID = id.GetValueOrDefault();
            if (ID == Guid.Empty)
            {
                if (Authenticate.IsAuthenticated())
                {

                    return RedirectToAction("EditProfile", "UserProfile", new { returnurl = HttpContext.Request.Url });
                }
                else
                {
                    return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
                }
            }
            else
            {
                UserProfile up = new UserProfile()
                {
                    Portfolio = new Portfolio(),
                    User = new User()
                };

                if (Authenticate.IsAuthenticated())
                {
                    User userin = System.Web.HttpContext.Current.Session["user"] as User;
                    up.User.LoadById(userin.Id);
                    return View(up);
                }
                else
                {
                    return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
                }
            }
        }

        // POST: UserProfile/EditProfilePassword
        [HttpPost]
        public ActionResult EditProfilePassword(Guid id, UserProfile up)
        {
            ModelState.Clear();
            if (Authenticate.IsAuthenticated())
            {
                try
                {
                    if (up.User.Password == null)
                    {
                        ModelState.AddModelError(string.Empty, "Password is required");
                    }

                    if (up.User.Password == null)
                    {
                        ModelState.AddModelError(string.Empty, "Password is required");
                    }

                    else if (up.User.Password.Length < 6)
                    {
                        ModelState.AddModelError(string.Empty, "Password needs to be at least 6 characters");
                    }

                    else if (up.User.Password.Length > 16)
                    {
                        ModelState.AddModelError(string.Empty, "Password needs to be less than 16 characters");
                    }

                    else if (up.ConfirmPassword != up.User.Password)
                    {
                        ModelState.AddModelError(string.Empty, "Passwords did not match");
                    }

                    if (!ModelState.IsValid)
                    {
                        return View(up);
                    }

                    User userin = System.Web.HttpContext.Current.Session["user"] as User;
                    userin.UpdatePassword(up.User.Password, up.OldPassword, userin.Id);
                    ModelState.Clear();
                    return RedirectToAction("PasswordUpdated");
                }
                catch
                {
                    ModelState.AddModelError("Password", "Incorrect Password");
                    return RedirectToAction("EditProfilePassword");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
            }
        }
        // Get: UserProfile/PasswordUpdated
        public ActionResult PasswordUpdated()
        {
            UserProfile up = new UserProfile();
            Session.Abandon();
            Session.Contents.Abandon();
            Session.Contents.RemoveAll();
            return View(up);
        }

        // GET: Edit User Profile (UserProfile/EditProfile)
        public ActionResult EditProfile()
        {
            UserProfile up = new UserProfile()
            {
                Projects = new ProjectList(),
                Portfolios = new PortfolioList(),
                User = new User(),
                UserTypes = new UserTypeList()
            };

            if (Authenticate.IsAuthenticated())
            {
                User userin = System.Web.HttpContext.Current.Session["user"] as User;
                up.User.LoadById(userin.Id);
                up.UserTypes.LoadNonAdmin();
                up.Portfolios.LoadbyUser(userin);
                foreach (Portfolio p in up.Portfolios)
                {
                    p.Projects.LoadbyPortfolioID(p.Id);

                }
                return View(up);
            }
            else
            {
                return RedirectToAction("Index", "Login", new { returnurl = HttpContext.Request.Url });
            }
        }


        // POST: Edit User Profile (UserProfile/EditProfile)
        [HttpPost]
        public ActionResult EditProfile(UserProfile up)
        {
            try
            {
                User userin = System.Web.HttpContext.Current.Session["user"] as User;
                FirstName = up.User.FirstName;
                LastName = up.User.LastName;
                Email = up.User.Email;
                UserTypesID = up.User.UserTypeId;
                UserTypesDescription = up.User.UserTypeDescription;
                ProfileImage = up.User.ProfileImage;
                Username = up.User.Username;
                User user = new User();
                user.LoadById(userin.Id);
                up.User = user;
                up.User.ProfileImage = ProfileImage;
                up.User.Username = Username;
                up.User.Email = Email;
                up.User.FirstName = FirstName;
                up.User.LastName = LastName;
                up.User.UserTypeId = UserTypesID;
                up.User.UserTypeDescription = UserTypesDescription;


                string currentemail = userin.Email;
                string currentUsername = userin.Username;



                if (up.User.Username == null)
                {
                    ModelState.AddModelError(string.Empty, "Username is required");
                }

                else if (up.User.Username != currentUsername)
                {
                    if (up.User.CheckIfUsernameExists(up.User.Username) != Guid.Empty)
                    {
                        ModelState.AddModelError(string.Empty, "Username Already Exists");
                    }
                }

                if (up.User.Email == null)
                {
                    ModelState.AddModelError(string.Empty, "Email address is required");
                }

                else if (up.User.Email != currentemail)
                {
                    if (up.User.CheckIfEmailExists(up.User.Email) == true)
                    {
                        ModelState.AddModelError(string.Empty, "Email Already In Use");
                    }
                }
                //POSSIBLE ADD EMAIL VERIFICATION ON CHANGE EMAIL OR RELOGIN HERE

                if (up.User.FirstName == null)
                {
                    ModelState.AddModelError(string.Empty, "First Name is required");
                }

                if (up.User.LastName == null)
                {
                    ModelState.AddModelError(string.Empty, "Last Name is required");
                }

                if (!ModelState.IsValid)
                {
                    up.UserTypes = new UserTypeList();
                    up.UserTypes.LoadNonAdmin();
                    return View(up);
                }
                up.User.Update();

                //TODO: Possible Redirect to confimration or Confirmation message of saved changes here!
                return RedirectToAction("Index", "UserProfile", new { returnurl = HttpContext.Request.Url });
            }
            catch
            {
                return View(up);
            }
        }




    }
}
